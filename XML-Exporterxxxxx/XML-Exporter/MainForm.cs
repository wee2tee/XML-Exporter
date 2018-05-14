using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using DBFHelper;
using XML_Exporter.DataModel;
using XML_Exporter.MiscClass;
using XML_Exporter.SubWindow;
using MetroFramework;
using MetroFramework.Forms;
using MetroFramework.Controls;
using CustomControl;
using System.Data.SQLite;
using System.Xml.Linq;

namespace XML_Exporter
{
    public partial class MainForm : MetroForm
    {
        #region public object/vars
        #endregion public object/vars

        #region private object/vars
        private BindingSource bs_sccomp;
        private BindingSource bs_glacc_asset;
        private BindingSource bs_glacc_lia;
        private BindingSource bs_glacc_cap;
        private BindingSource bs_glacc_rev;
        private BindingSource bs_glacc_exp;

        private Sccomp selected_comp;
        private Glacc selected_acc;

        private List<dataItem4Sccomp> list_sccomp_item; // storing company name
        private List<dataItem4Glacc> list_glacc_item; // storing account sequence in parent-child format
        private List<dataItem4Glacc> tmp_list; // before recursing to parent-child format account sequence
        private List<dataItem4Glacc> list_posting_item; // storing account sequence in parent-child format in final step(use to generate trial balance or generate xml file)
        private List<Glbal> list_glbal; // storing begining balance
        private List<Gljnlit> list_gljnlit; // storing period movement transaction
        private List<Taxonomy> taxonomy_list;

        private CustomBrowseField cb_taxonomy;
        #endregion private object/vars

        public bool prevent_change_tab = true;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Step1_Init();
            this.LoadControlEventHandler();
            this.SetSelectedTab(this.tabStep1);
            this.list_sccomp_item = new List<dataItem4Sccomp>();
            this.list_glacc_item = new List<dataItem4Glacc>();
            this.list_posting_item = new List<dataItem4Glacc>();
            this.list_glbal = new List<Glbal>();
            this.list_gljnlit = new List<Gljnlit>();
            this.bs_sccomp = new BindingSource();
            this.bs_glacc_asset = new BindingSource();
            this.bs_glacc_lia = new BindingSource();
            this.bs_glacc_cap = new BindingSource();
            this.bs_glacc_rev = new BindingSource();
            this.bs_glacc_exp = new BindingSource();

            this.taxonomy_list = Taxonomy.GetTaxonomyList(this);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.LoadDependenciesData();
            this.FillFirstTab();
            this.dgvComp.Focus();
        }

        private void LoadControlEventHandler()
        {
            this.dgvComp.CurrentCellChanged += delegate(object sender, EventArgs e)
            {
                if (this.dgvComp.CurrentCell != null)
                {
                    this.btnNext2Glacc.Enabled = true;
                    this.selected_comp = (Sccomp)this.dgvComp.Rows[this.dgvComp.CurrentCell.RowIndex].Cells[0].Value;
                }
            };

            this.dgvComp.MouseDoubleClick += delegate(object sender, MouseEventArgs e)
            {
                int row_index = this.dgvComp.HitTest(e.X, e.Y).RowIndex;
                if (row_index > -1)
                {
                    this.btnNext2Glacc.PerformClick();
                }
            };

            this.tabControlStep.Selected += delegate(object sender, TabControlEventArgs e)
            {
                if (e.TabPage == this.tabStep2)
                {
                    this.tabControlGlacc.SelectedTab = this.tabAsset;
                }
            };

            this.tabControlGlacc.SelectedIndexChanged += delegate
            {
                if (this.tabControlGlacc.SelectedTab == this.tabAsset)
                {
                    this.dgvGlaccAsset.Focus();
                    this.SetStatText(this.tabAsset);
                }
                if (this.tabControlGlacc.SelectedTab == this.tabLia)
                {
                    this.dgvGlaccLia.Focus();
                    this.SetStatText(this.tabLia);
                }
                if (this.tabControlGlacc.SelectedTab == this.tabCap)
                {
                    this.dgvGlaccCap.Focus();
                    this.SetStatText(this.tabCap);
                }
                if (this.tabControlGlacc.SelectedTab == this.tabRev)
                {
                    this.dgvGlaccRev.Focus();
                    this.SetStatText(this.tabRev);
                }
                if (this.tabControlGlacc.SelectedTab == this.tabExp)
                {
                    this.dgvGlaccExp.Focus();
                    this.SetStatText(this.tabExp);
                }
            };

            this.dgvGlaccAsset.CurrentCellChanged += new EventHandler(this.SetCurrentSelectedAcc);
            this.dgvGlaccLia.CurrentCellChanged += new EventHandler(this.SetCurrentSelectedAcc);
            this.dgvGlaccCap.CurrentCellChanged += new EventHandler(this.SetCurrentSelectedAcc);
            this.dgvGlaccRev.CurrentCellChanged += new EventHandler(this.SetCurrentSelectedAcc);
            this.dgvGlaccExp.CurrentCellChanged += new EventHandler(this.SetCurrentSelectedAcc);
            
            this.dgvGlaccAsset.RowPostPaint += new DataGridViewRowPostPaintEventHandler(this.SetRowStyleByAcctyp);
            this.dgvGlaccLia.RowPostPaint += new DataGridViewRowPostPaintEventHandler(this.SetRowStyleByAcctyp);
            this.dgvGlaccCap.RowPostPaint += new DataGridViewRowPostPaintEventHandler(this.SetRowStyleByAcctyp);
            this.dgvGlaccRev.RowPostPaint += new DataGridViewRowPostPaintEventHandler(this.SetRowStyleByAcctyp);
            this.dgvGlaccExp.RowPostPaint += new DataGridViewRowPostPaintEventHandler(this.SetRowStyleByAcctyp);

            this.dgvGlaccAsset.MouseDoubleClick += new MouseEventHandler(this.DgvGlaccDoubleClickHandler);
            this.dgvGlaccLia.MouseDoubleClick += new MouseEventHandler(this.DgvGlaccDoubleClickHandler);
            this.dgvGlaccCap.MouseDoubleClick += new MouseEventHandler(this.DgvGlaccDoubleClickHandler);
            this.dgvGlaccRev.MouseDoubleClick += new MouseEventHandler(this.DgvGlaccDoubleClickHandler);
            this.dgvGlaccExp.MouseDoubleClick += new MouseEventHandler(this.DgvGlaccDoubleClickHandler);

            this.rbAccnamThai.CheckedChanged += new EventHandler(this.SwithAccnamLanguage);
        }

        private void SetCurrentSelectedAcc(object sender, EventArgs e)
        {
            DataGridView target_dgv = null;
            if (this.tabControlGlacc.SelectedTab == this.tabAsset)
                target_dgv = this.dgvGlaccAsset;
            if (this.tabControlGlacc.SelectedTab == this.tabLia)
                target_dgv = this.dgvGlaccLia;
            if (this.tabControlGlacc.SelectedTab == this.tabCap)
                target_dgv = this.dgvGlaccCap;
            if (this.tabControlGlacc.SelectedTab == this.tabRev)
                target_dgv = this.dgvGlaccRev;
            if (this.tabControlGlacc.SelectedTab == this.tabExp)
                target_dgv = this.dgvGlaccExp;

            if (target_dgv == null || target_dgv.CurrentCell == null)
            {
                this.selected_acc = null;
                return;
            }

            if (this.selected_acc == null || ((Glacc)target_dgv.Rows[target_dgv.CurrentCell.RowIndex].Cells[0].Value).accnum != this.selected_acc.accnum)
            {
                this.selected_acc = (Glacc)target_dgv.Rows[target_dgv.CurrentCell.RowIndex].Cells[0].Value;

                if (((Glacc)target_dgv.Rows[target_dgv.CurrentCell.RowIndex].Cells[0].Value).acctyp == "0")
                {
                    this.ClearInlineTaxodescSelector();
                    this.ShowInlineTaxodescSelector(target_dgv.Rows[target_dgv.CurrentCell.RowIndex]);
                }
                else
                {
                    this.ClearInlineTaxodescSelector();
                }
            }
        }

        private void SetRowStyleByAcctyp(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (((Glacc)((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value).acctyp == "1")
            {
                foreach (DataGridViewCell cell in ((DataGridView)sender).Rows[e.RowIndex].Cells)
                {
                    cell.Style.ForeColor = Color.LightGray;
                    cell.Style.SelectionForeColor = Color.LightGray;
                    cell.Style.SelectionBackColor = Color.FromArgb(248, 248, 248);
                }
            }
            else
            {
                foreach (DataGridViewCell cell in ((DataGridView)sender).Rows[e.RowIndex].Cells)
                {
                    if (cell.ColumnIndex == 12)
                    {
                        cell.Style.ForeColor = Color.Blue;
                        cell.Style.SelectionForeColor = Color.Blue;
                    }
                    else
                    {
                        cell.Style.ForeColor = Color.FromArgb(64, 64, 64);
                        cell.Style.SelectionForeColor = Color.FromArgb(64, 64, 64);
                    }
                }
            }

            if (((DataGridView)sender).CurrentCell != null)
            {
                this.SetInlineTaxodescPosition(((DataGridView)sender).Rows[((DataGridView)sender).CurrentCell.RowIndex]);
            }
        }

        private void DgvGlaccDoubleClickHandler(object sender, MouseEventArgs e)
        {
            int row_index = ((DataGridView)sender).HitTest(e.X, e.Y).RowIndex;
            if (row_index > -1)
            {
                this.ClearInlineTaxodescSelector();
                this.ShowInlineTaxodescSelector(((DataGridView)sender).Rows[((DataGridView)sender).CurrentCell.RowIndex]);
            }
        }

        private void SetStatText(TabPage tabpage)
        {
            if (tabpage == this.tabAsset)
            {
                this.lblStatAssets.Text = "บัญชีย่อยในหมวดสินทรัพย์ทั้งหมด " + this.list_glacc_item.Where(g => g.group == "1" && g.acctyp == "0").Count<dataItem4Glacc>().ToString() + " บัญชี , กำหนด Taxonomy ไว้แล้ว " + this.list_glacc_item.Where(g => g.group == "1" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>() + " บัญชี";
                if (this.list_glacc_item.Where(g => g.group == "1" && g.acctyp == "0").Count<dataItem4Glacc>() == this.list_glacc_item.Where(g => g.group == "1" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>())
                {
                    this.lblStatAssets.ForeColor = AppResource.TITLE_THEME;
                }
                else
                {
                    this.lblStatAssets.ForeColor = Color.Red;
                }
                return;
            }
            if (tabpage == this.tabLia)
            {
                this.lblStatLia.Text = "บัญชีย่อยในหมวดหนี้สินทั้งหมด " + this.list_glacc_item.Where(g => g.group == "2" && g.acctyp == "0").Count<dataItem4Glacc>().ToString() + " บัญชี , กำหนด Taxonomy ไว้แล้ว " + this.list_glacc_item.Where(g => g.group == "2" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>() + " บัญชี";
                if (this.list_glacc_item.Where(g => g.group == "2" && g.acctyp == "0").Count<dataItem4Glacc>() == this.list_glacc_item.Where(g => g.group == "2" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>())
                {
                    this.lblStatLia.ForeColor = AppResource.TITLE_THEME;
                }
                else
                {
                    this.lblStatLia.ForeColor = Color.Red;
                }
                return;
            }
            if (tabpage == this.tabCap)
            {
                this.lblStatCap.Text = "บัญชีย่อยในหมวดทุนทั้งหมด " + this.list_glacc_item.Where(g => g.group == "3" && g.acctyp == "0").Count<dataItem4Glacc>().ToString() + " บัญชี , กำหนด Taxonomy ไว้แล้ว " + this.list_glacc_item.Where(g => g.group == "3" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>() + " บัญชี";
                if (this.list_glacc_item.Where(g => g.group == "3" && g.acctyp == "0").Count<dataItem4Glacc>() == this.list_glacc_item.Where(g => g.group == "3" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>())
                {
                    this.lblStatCap.ForeColor = AppResource.TITLE_THEME;
                }
                else
                {
                    this.lblStatCap.ForeColor = Color.Red;
                }
                return;
            }
            if (tabpage == this.tabRev)
            {
                this.lblStatRev.Text = "บัญชีย่อยในหมวดรายได้ทั้งหมด " + this.list_glacc_item.Where(g => g.group == "4" && g.acctyp == "0").Count<dataItem4Glacc>().ToString() + " บัญชี , กำหนด Taxonomy ไว้แล้ว " + this.list_glacc_item.Where(g => g.group == "4" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>() + " บัญชี";
                if (this.list_glacc_item.Where(g => g.group == "4" && g.acctyp == "0").Count<dataItem4Glacc>() == this.list_glacc_item.Where(g => g.group == "4" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>())
                {
                    this.lblStatRev.ForeColor = AppResource.TITLE_THEME;
                }
                else
                {
                    this.lblStatRev.ForeColor = Color.Red;
                }
                return;
            }
            if (tabpage == this.tabExp)
            {
                this.lblStatExp.Text = "บัญชีย่อยในหมวดค่าใช้จ่ายทั้งหมด " + this.list_glacc_item.Where(g => g.group == "5" && g.acctyp == "0").Count<dataItem4Glacc>().ToString() + " บัญชี , กำหนด Taxonomy ไว้แล้ว " + this.list_glacc_item.Where(g => g.group == "5" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>() + " บัญชี";
                if (this.list_glacc_item.Where(g => g.group == "5" && g.acctyp == "0").Count<dataItem4Glacc>() == this.list_glacc_item.Where(g => g.group == "5" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>())
                {
                    this.lblStatExp.ForeColor = AppResource.TITLE_THEME;
                }
                else
                {
                    this.lblStatExp.ForeColor = Color.Red;
                }
                return;
            }
        }

        private void ShowInlineTaxodescSelector(DataGridViewRow row)
        {
            if (((Glacc)row.Cells[0].Value).acctyp == "1")
                return;

            if (this.cb_taxonomy != null)
                this.ClearInlineTaxodescSelector();

            this.cb_taxonomy = new CustomBrowseField();
            this.cb_taxonomy.Name = "cb_taxonomy";
            this.cb_taxonomy._BorderColor = Color.FromArgb(0, 174, 219);
            this.cb_taxonomy._Text = (((string)row.Cells[12].Value).Trim().Length > 0 ? (string)row.Cells[12].Value : "");
            this.cb_taxonomy._Enabled = true;
            this.cb_taxonomy.btnBrowse.Click += delegate
            {
                Glacc curr_acc = (Glacc)row.Cells[0].Value;
                string acc_group = curr_acc.group;
                Taxonomy curr_taxonomy = this.taxonomy_list.Find(t => t.taxodesc == this.list_glacc_item.Find(g => g.accnum == curr_acc.accnum).taxonomy);

                ListTaxodesc td = new ListTaxodesc(this, curr_acc, this.taxonomy_list.Where(t => t.group == acc_group).ToList<Taxonomy>(), curr_taxonomy);
                td.StartPosition = FormStartPosition.Manual;

                Point cb_point = this.cb_taxonomy.PointToScreen(Point.Empty);
                int screen_height = SystemInformation.VirtualScreen.Height;
                if (cb_point.Y <= screen_height - (this.cb_taxonomy.ClientSize.Height + td.ClientSize.Height))
                {
                    td.Location = new Point(cb_point.X - td.ClientSize.Width + this.cb_taxonomy.ClientSize.Width, cb_point.Y + (this.cb_taxonomy.ClientSize.Height + 6));
                }
                else
                {
                    td.Location = new Point(cb_point.X - td.ClientSize.Width + this.cb_taxonomy.ClientSize.Width, cb_point.Y - (td.ClientSize.Height + 6));
                }
                
                if (td.ShowDialog() == DialogResult.OK)
                {
                    this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum).taxonomy = td.current_taxonomy.taxodesc;
                    this.SetStatText(this.tabControlGlacc.SelectedTab);

                    if (acc_group == "1")
                        this.bs_glacc_asset.ResetBindings(true);
                    if (acc_group == "2")
                        this.bs_glacc_lia.ResetBindings(true);
                    if (acc_group == "3")
                        this.bs_glacc_cap.ResetBindings(true);
                    if (acc_group == "4")
                        this.bs_glacc_rev.ResetBindings(true);
                    if (acc_group == "5")
                        this.bs_glacc_exp.ResetBindings(true);

                    this.cb_taxonomy._SelectionStart = this.cb_taxonomy._Text.Length;
                    SendKeys.Send("{TAB}");
                }
            };
            row.DataGridView.Parent.Controls.Add(this.cb_taxonomy);
            this.SetInlineTaxodescPosition(row);

            this.cb_taxonomy.BringToFront();
            row.DataGridView.SendToBack();
            this.cb_taxonomy.Focus();
        }

        private void SetInlineTaxodescPosition(DataGridViewRow row)
        {
            if (this.cb_taxonomy == null)
                return;

            Rectangle rect = row.DataGridView.GetCellDisplayRectangle(12, row.Index, true);

            this.cb_taxonomy.SetBounds(rect.X + 3, rect.Y + 4, rect.Width, rect.Height-2);
        }

        private void ClearInlineTaxodescSelector()
        {
            if (this.cb_taxonomy != null)
            {
                this.cb_taxonomy.Dispose();
                this.cb_taxonomy = null;
            }
        }

        private void CheckCompleteFillTaxodesc()
        {
            if (this.list_glacc_item.Where(g => g.acctyp == "0" && g.taxonomy.Trim().Length == 0).Count<dataItem4Glacc>() == 0)
            {
                MetroMessageBox.Show(this, "ท่าได้กำหนด Taxonomy ให้กับบัญชีย่อยครบทุกบัญชีแล้ว", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ClearInlineTaxodescSelector();
            }
        }

        private void SwithAccnamLanguage(object sender, EventArgs e)
        {
            this.FillSecondTab();
        }

        private void LoadDependenciesData()
        {
            this.LoadSccomp();
        }

        #region 1st Tab
        private void SetDgvSccompVisualStyle()
        {
            if (this.dgvComp.Columns.Count > 0)
            {
                this.dgvComp.Columns[0].Visible = false;
                this.dgvComp.Columns[1].Width = 400;
                this.dgvComp.Columns[2].Width = 120;
                this.dgvComp.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dgvComp.Columns[1].HeaderText = "ชื่อบริษัท";
                this.dgvComp.Columns[2].HeaderText = "รหัสข้อมูล";
                this.dgvComp.Columns[3].HeaderText = "ที่เก็บข้อมูล";
            }
        }

        private void LoadSccomp()
        {
            try
            {
                DataTable dt = DBFParse.ReadDBF(AppResource.PATH_PREFIX + "secure/sccomp.dbf");

                this.list_sccomp_item.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    this.list_sccomp_item.Add(new dataItem4Sccomp()
                    {
                        sccomp = new Sccomp()
                        {
                            compnam = ((string)row[0]).Trim(),
                            compcod = ((string)row[1]).Trim(),
                            path = ((string)row[2]).Trim(),
                            gendat = (row[3] is DBNull ? DateTime.Now : (DateTime)row[3]),
                            candel = ((string)row[4]).Trim()
                        },
                        compnam = ((string)row[0]).Trim(),
                        compcod = ((string)row[1]).Trim(),
                        path = ((string)row[2]).Trim(),
                    });
                }
            }
            catch (IOException ex)
            {
                if (MetroMessageBox.Show(this, ex.Message, "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, 370) == DialogResult.Retry)
                {
                    this.LoadSccomp();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void FillFirstTab()
        {
            if (this.list_sccomp_item.Count == 0)
            {
                if (MetroMessageBox.Show(this, "ไม่พบรายชื่อบริษัทในโปรแกรมเอ็กซ์เพรส, กรุณาตรวจสอบอีกครั้งว่าท่านเรียกโปรแกรมนี้จากภายในที่เก็บโปรแกรมเอ็กซ์เพรส", "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, 370) == DialogResult.Retry)
                {
                    this.LoadSccomp();
                    this.FillFirstTab();
                }
                else
                {
                    this.Close();
                }
                return;
            }
            this.bs_sccomp.DataSource = this.list_sccomp_item;
            this.bs_sccomp.ResetBindings(true);
            this.dgvComp.DataSource = this.bs_sccomp;
            this.SetDgvSccompVisualStyle();
            this.btnNext2Glacc.Visible = true;
        }

        private void btnNext2Glacc_Click(object sender, EventArgs e)
        {
            if (this.selected_comp != null)
            {
                this.LoadGlacc();
                List<MatchingData> matching = this.GetMatchingData();

                foreach (MatchingData d in matching)
                {
                    if (this.list_glacc_item.Find(g => g.accnum == d.accnum) != null)
                    {
                        this.list_glacc_item.Find(g => g.accnum == d.accnum).taxonomy = d.taxodesc;
                    }
                }
                this.FillSecondTab();
                this.ClearInlineTaxodescSelector();
                if (this.list_glacc_item.Count > 0)
                {
                    this.SetSelectedTab(this.tabStep2);
                    this.dgvGlaccAsset.Focus();
                    this.lblCompnamStep2.Text = this.selected_comp.compnam.Trim() + " [ ที่เก็บข้อมูล : " + this.selected_comp.path.Trim() + " ]";
                    this.lblCompnamStep3.Text = this.selected_comp.compnam.Trim() + " [ ที่เก็บข้อมูล : " + this.selected_comp.path.Trim() + " ]";
                }
            }
            else
            {
                MetroMessageBox.Show(this, "กรุณาเลือกที่เก็บข้อมูลก่อนทำขั้นตอนต่อไป", "", MessageBoxButtons.OK);
            }
        }
        #endregion 1st Tab

        #region 2nd Tab
        private void LoadGlacc()
        {
            try
            {
                this.list_glacc_item.Clear();
                DataTable dt = DBFParse.ReadDBF(AppResource.PATH_PREFIX + this.selected_comp.path + "/glacc.dbf");

                this.tmp_list = new List<dataItem4Glacc>();
                foreach (DataRow row in dt.Rows)
                {
                    string space = "";
                    for (int space_fill = 1; space_fill <= ((int)row[3]) * 4; space_fill++)
                    {
                        space += " ";
                    }

                    this.tmp_list.Add(new dataItem4Glacc()
                    {
                        glacc = new Glacc()
                        {
                            accnum = ((string)row[0]).Trim(),
                            accnam = ((string)row[1]).Trim(),
                            accnam2 = ((string)row[2]).Trim(),
                            level = (int)row[3],
                            parent = ((string)row[4]).Trim(),
                            group = ((string)row[5]).Trim(),
                            acctyp = ((string)row[6]).Trim(),
                            usedep = ((string)row[7]).Trim(),
                            usejob = ((string)row[8]).Trim(),
                            nature = ((string)row[9]).Trim(),
                            consol = ((string)row[10]).Trim(),
                            status = ((string)row[11]).Trim(),
                            creby = ((string)row[12]).Trim(),
                            credat = (row[13] is DBNull ? DateTime.Now : (DateTime)row[13]),
                            userid = ((string)row[14]).Trim(),
                            chgdat = (row[15] is DBNull ? DateTime.Now : (DateTime)row[15])
                        },
                        accnum = ((string)row[0]).Trim(),
                        accnam = space + ((string)row[1]).Trim(),
                        accnam2 = space + ((string)row[2]).Trim(),
                        level = (int)row[3],
                        parent = ((string)row[4]).Trim(),
                        group = ((string)row[5]).Trim(),
                        acctyp = ((string)row[6]).Trim(),
                        usedep = ((string)row[7]).Trim(),
                        usejob = ((string)row[8]).Trim(),
                        nature = ((string)row[9]).Trim(),
                        consol = ((string)row[10]).Trim(),
                        taxonomy = "",
                        operation = ""
                    });
                }

                foreach (dataItem4Glacc item in this.tmp_list.Where(t => t.level == 1).ToList<dataItem4Glacc>().OrderBy(t => t.group).ToList<dataItem4Glacc>())
                {
                    this.RecursDataItem(item);
                }
            }
            catch (IOException ex)
            {
                if (MetroMessageBox.Show(this, ex.Message, "เกิดข้อผิดพลาด :", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, 370) == DialogResult.Retry)
                {
                    this.LoadGlacc();
                }
                else
                {
                    return;
                }
            }
        }

        private void RecursDataItem(dataItem4Glacc item)
        {
            this.list_glacc_item.Add(item);
            
            foreach (dataItem4Glacc it in this.tmp_list.Where(t => t.parent == item.accnum).ToList<dataItem4Glacc>())
            {
                this.RecursDataItem(it);
            }
        }

        private List<MatchingData> GetMatchingData()
        {
            List<MatchingData> data = new List<MatchingData>();

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Data/taxodesc_" + this.selected_comp.path + ".db"))
            {
                try
                {
                    string conn_str = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "/Data/taxodesc_" + this.selected_comp.path + ".db;Version=3;";
                    using (SQLiteConnection conn = new SQLiteConnection(conn_str))
                    {
                        conn.Open();

                        using (SQLiteTransaction tr = conn.BeginTransaction())
                        {
                            string sql = "Select * From glacc_match Order by accnum";
                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                using (SQLiteDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        data.Add(new MatchingData()
                                        {
                                            id = reader.GetInt32(0),
                                            accnum = (reader.IsDBNull(1) ? "" : reader.GetString(1)),
                                            depcod = (reader.IsDBNull(2) ? "" : reader.GetString(2)),
                                            taxodesc = (reader.IsDBNull(3) ? "" : reader.GetString(3))
                                        });
                                    }
                                }
                            }
                        }
                        conn.Close();
                    }
                }
                catch (SQLiteException ex)
                {

                }
            }

            return data;
        }

        private void FillSecondTab()
        {
            if (this.list_glacc_item.Count == 0)
            {
                if (MetroMessageBox.Show(this, "ไม่พบรายการผังบัญชีจากที่เก็บข้อมูลที่ท่านเลือก", "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, 370) == DialogResult.Retry)
                {
                    this.LoadGlacc();
                    this.FillSecondTab();
                }
                else
                {
                    this.SetSelectedTab(this.tabStep1);
                    this.dgvComp.Focus();
                    return;
                }
            }
            else {
                this.bs_glacc_exp.DataSource = this.list_glacc_item.Where(g => g.group == "5").ToList<dataItem4Glacc>();
                this.bs_glacc_exp.ResetBindings(true);
                this.dgvGlaccExp.DataSource = this.bs_glacc_exp;
                this.SetDgvGlaccVisualStyle(this.dgvGlaccExp);

                this.bs_glacc_rev.DataSource = this.list_glacc_item.Where(g => g.group == "4").ToList<dataItem4Glacc>();
                this.bs_glacc_rev.ResetBindings(true);
                this.dgvGlaccRev.DataSource = this.bs_glacc_rev;
                this.SetDgvGlaccVisualStyle(this.dgvGlaccRev);

                this.bs_glacc_cap.DataSource = this.list_glacc_item.Where(g => g.group == "3").ToList<dataItem4Glacc>();
                this.bs_glacc_cap.ResetBindings(true);
                this.dgvGlaccCap.DataSource = this.bs_glacc_cap;
                this.SetDgvGlaccVisualStyle(this.dgvGlaccCap);

                this.bs_glacc_lia.DataSource = this.list_glacc_item.Where(g => g.group == "2").ToList<dataItem4Glacc>();
                this.bs_glacc_lia.ResetBindings(true);
                this.dgvGlaccLia.DataSource = this.bs_glacc_lia;
                this.SetDgvGlaccVisualStyle(this.dgvGlaccLia);

                this.bs_glacc_asset.DataSource = this.list_glacc_item.Where(g => g.group == "1").ToList<dataItem4Glacc>();
                this.bs_glacc_asset.ResetBindings(true);
                this.dgvGlaccAsset.DataSource = this.bs_glacc_asset;
                this.SetDgvGlaccVisualStyle(this.dgvGlaccAsset);

                this.btnPrev2Comp.Visible = true;
                this.btnExportXML.Visible = true;
            }
        }

        private void SetDgvGlaccVisualStyle(DataGridView dgv)
        {
            dgv.Columns[0].Visible = false;
            for (int col = 4; col <= 11; col++)
            {
                dgv.Columns[col].Visible = false;
            }
                
            dgv.Columns[1].Width = 150;
            dgv.Columns[1].HeaderText = "เลขที่บัญชี";
            dgv.Columns[2].Visible = (this.rbAccnamThai.Checked ? true : false);
            dgv.Columns[2].AutoSizeMode = (dgv.Columns[2].Visible ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.None);
            dgv.Columns[2].HeaderText = "ชื่อบัญชี (ไทย)";
            dgv.Columns[3].Visible = (this.rbAccnamEng.Checked ? true : false);
            dgv.Columns[3].AutoSizeMode = (dgv.Columns[3].Visible ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.None);
            dgv.Columns[3].HeaderText = "ชื่อบัญชี (Eng)";
            dgv.Columns[12].Width = 400;
            dgv.Columns[12].HeaderText = "DBD Taxonomy";
            dgv.Columns[13].HeaderText = "";
            dgv.Columns[13].Visible = false;
        }

        private bool ValidateTaxonomy(string taxodesc, string acc_group)
        {
            bool result = false;

            if (this.taxonomy_list.Find(t => t.taxodesc.Trim() == taxodesc.Trim() && t.group.Trim() == acc_group.Trim()) != null)
            {
                result = true;
            }

            return result;
        }

        private dataItem4Glacc GetFirstPostItemInGroup(string acc_group)
        {
            dataItem4Glacc item = null;

            foreach (dataItem4Glacc it in this.list_glacc_item.Where(i => i.group.Trim() == acc_group).ToList<dataItem4Glacc>())
            {
                if (it.acctyp == "0")
                {
                    item = it;
                    break;
                }
            }

            return item;
        }

        private dataItem4Glacc GetNextPostItem(dataItem4Glacc current_item)
        {
            int current_item_index = -1;
            if (current_item != null)
            {
                current_item_index = this.list_glacc_item.FindIndex(t => t.accnum.Trim() == current_item.accnum.Trim());
            }

            dataItem4Glacc item = null;
            for (int i = current_item_index + 1; i <= this.list_glacc_item.Count -1; i++)
            {
                if (this.list_glacc_item[i].acctyp == "1") // Summary account
                {
                    continue;
                }
                else // Posting account
                {
                    item = this.list_glacc_item[i];
                    break;
                }
            }
            return item;
        }

        private void SaveTaxoDescDB()
        {
            if (this.list_glacc_item.Where(g => g.acctyp == "0" && g.taxonomy.Trim().Length == 0).Count<dataItem4Glacc>() > 0)
            {
                MetroMessageBox.Show(this, "บัญชีย่อยบางบัญชียังไม่ได้กำหนด Taxonomy, ท่านสามารถบันทึกการเปลี่ยนแปลงนี้ได้ แต่ยังไม่สามารถเข้าสู่ขั้นตอนต่อไปได้", "คำเตือน :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Data/taxodesc_" + this.selected_comp.path + ".db"))
            {
                SQLiteConnection.CreateFile(AppDomain.CurrentDomain.BaseDirectory + "/Data/taxodesc_" + this.selected_comp.path + ".db");
            }

            try
            {
                string conn_str = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "/Data/taxodesc_" + this.selected_comp.path + ".db;Version=3;";
                using (SQLiteConnection conn = new SQLiteConnection(conn_str))
                {
                    conn.Open();

                    using (SQLiteTransaction tr = conn.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tr;
                            cmd.CommandText = "DROP TABLE IF EXISTS glacc_match";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = @"CREATE TABLE glacc_match(id INTEGER PRIMARY KEY, accnum TEXT, depcod TEXT, taxodesc TEXT)";
                            cmd.ExecuteNonQuery();

                            foreach (dataItem4Glacc item in this.list_glacc_item.Where(g => g.acctyp == "0" && g.taxonomy.Length > 0).ToList<dataItem4Glacc>())
                            {
                                cmd.CommandText = "INSERT INTO glacc_match(accnum, taxodesc) VALUES('" + item.accnum.Trim() + "','" + item.taxonomy.Trim() + "')";
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tr.Commit();
                    }
                    conn.Close();
                }
                MetroMessageBox.Show(this, "บันทึกข้อมูลเรียบร้อย", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SQLiteException ex)
            {
                if (MetroMessageBox.Show(this, ex.Message, "ข้อผิดพลาด", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    this.SaveTaxoDescDB();
                }
            }
        }

        private void btnPrev2Comp_Click(object sender, EventArgs e)
        {
            if (MetroMessageBox.Show(this, "ข้อมูลที่ท่านทำการแก้ไขในหน้าต่างนี้จะไม่ถูกบันทึก, ดำเนินการต่อหรือไม่?", "คำเตือน :", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.SetSelectedTab(this.tabStep1);
                this.list_glacc_item.Clear();
            }
        }

        private void btnSaveTaxodesc_Click(object sender, EventArgs e)
        {
            this.SaveTaxoDescDB();
            if (this.list_glacc_item.Where(g => g.acctyp == "0" && g.taxonomy.Trim().Length == 0).Count<dataItem4Glacc>() == 0)
            {
                this.list_posting_item = this.list_glacc_item.ConvertAll(x => x).ToList<dataItem4Glacc>();
                this.list_glacc_item.Clear();
                this.SetSelectedTab(this.tabStep3);
                this.dtFrom.Focus();
            }
        }

        private void btnNext2Export_Click(object sender, EventArgs e)
        {

        }
        #endregion 2nd Tab

        #region 3rd Tab
        private void btnBack2Glacc_Click(object sender, EventArgs e)
        {
            this.list_glacc_item = this.list_posting_item.ConvertAll(x => x).ToList<dataItem4Glacc>();
            this.list_posting_item.Clear();
            this.SetSelectedTab(this.tabStep2);
        }

        private void btnExportXML_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.DefaultExt = "xml";
            sf.Filter = "xml files (*.xml)|*.xml";
            sf.RestoreDirectory = true;
            sf.CheckPathExists = true;
            sf.InitialDirectory = @AppDomain.CurrentDomain.BaseDirectory;

            if (sf.ShowDialog() == DialogResult.OK)
            {
                BackgroundWorker worker = new BackgroundWorker();
                this.loadingIcon.Visible = true;
                this.btnExportXML.Enabled = false;
                this.dtFrom.Enabled = false;
                this.dtTo.Enabled = false;
                worker.DoWork += delegate(object obj, DoWorkEventArgs ev)
                {
                    this.LoadGlbal();
                    this.LoadGljnlit();
                };
                worker.RunWorkerCompleted += delegate(object obj, RunWorkerCompletedEventArgs ev)
                {
                    this.loadingIcon.Visible = false;
                    this.btnExportXML.Enabled = true;
                    this.dtFrom.Enabled = true;
                    this.dtTo.Enabled = true;

                    this.CreateXMLFile(sf.FileName);
                };
                worker.RunWorkerAsync();
            }

        }

        private void LoadGljnlit()
        {
            try
            {
                this.list_gljnlit.Clear();
                DataTable dt = DBFParse.ReadDBF(AppResource.PATH_PREFIX + this.selected_comp.path + "/gljnlit.dbf");

                foreach (DataRow row in dt.Rows)
                {
                    this.list_gljnlit.Add(new Gljnlit()
                    {
                        voucher = ((string)row[0]).Trim(),
                        seqit = (string)row[1],
                        voudat = Convert.ToDateTime(row[2]),
                        accnum = ((string)row[3]).Trim(),
                        depcod = ((string)row[4]).Trim(),
                        jobcod = ((string)row[5]).Trim(),
                        phase = ((string)row[6]).Trim(),
                        coscod = ((string)row[7]).Trim(),
                        descrp = ((string)row[8]).Trim(),
                        trntyp = ((string)row[9]).Trim(),
                        amount = Convert.ToDouble(row[10]),
                        chgdat = (row[11] is DBNull ? DateTime.Now : Convert.ToDateTime(row[11])),
                        chgtim = ((string)row[12]).Trim(),
                        adjust = ((string)row[13]).Trim(),
                        chgaccfrom = ((string)row[14]).Trim()
                    });
                }
            }
            catch (IOException ex)
            {
                if (MetroMessageBox.Show(this, ex.Message, "เกิดข้อผิดพลาด :", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, 370) == DialogResult.Retry)
                {
                    this.LoadGljnlit();
                }
                else
                {
                    return;
                }
            }
        }

        private void LoadGlbal()
        {
            try
            {
                this.list_glbal.Clear();
                DataTable dt = DBFParse.ReadDBF(AppResource.PATH_PREFIX + this.selected_comp.path + "/glbal.dbf");

                foreach (DataRow row in dt.Rows)
                {
                    this.list_glbal.Add(new Glbal()
                    {
                         accnum = ((string)row[0]).Trim(),
                         depcod = ((string)row[1]).Trim(),
                         jobcod = (string)row[2],
                         calsta = ((string)row[3]).Trim(),
                         begly = (double)row[4],
                         debit1ly = (double)row[5],
                         debit2ly = (double)row[6],
                         debit3ly = (double)row[7],
                         debit4ly = (double)row[8],
                         debit5ly = (double)row[9],
                         debit6ly = (double)row[10],
                         debit7ly = (double)row[11],
                         debit8ly = (double)row[12],
                         debit9ly = (double)row[13],
                         debit10ly = (double)row[14],
                         debit11ly = (double)row[15],
                         debit12ly = (double)row[16],
                         debitcls = (double)row[17],
                         debit1 = (double)row[18],
                         debit2 = (double)row[19],
                         debit3 = (double)row[20],
                         debit4 = (double)row[21],
                         debit5 = (double)row[22],
                         debit6 = (double)row[23],
                         debit7 = (double)row[24],
                         debit8 = (double)row[25],
                         debit9 = (double)row[26],
                         debit10 = (double)row[27],
                         debit11 = (double)row[28],
                         debit12 = (double)row[29],
                         debit1ny = (double)row[30],
                         debit2ny = (double)row[31],
                         debit3ny = (double)row[32],
                         debit4ny = (double)row[33],
                         debit5ny = (double)row[34],
                         debit6ny = (double)row[35],
                         debit7ny = (double)row[36],
                         debit8ny = (double)row[37],
                         debit9ny = (double)row[38],
                         debit10ny = (double)row[39],
                         debit11ny = (double)row[40],
                         debit12ny = (double)row[41],
                         credit1ly = (double)row[42],
                         credit2ly = (double)row[43],
                         credit3ly = (double)row[44],
                         credit4ly = (double)row[45],
                         credit5ly = (double)row[46],
                         credit6ly = (double)row[47],
                         credit7ly = (double)row[48],
                         credit8ly = (double)row[49],
                         credit9ly = (double)row[50],
                         credit10ly = (double)row[51],
                         credit11ly = (double)row[52],
                         credit12ly = (double)row[53],
                         creditcls = (double)row[54],
                         credit1 = (double)row[55],
                         credit2 = (double)row[56],
                         credit3 = (double)row[57],
                         credit4 = (double)row[58],
                         credit5 = (double)row[59],
                         credit6 = (double)row[60],
                         credit7 = (double)row[61],
                         credit8 = (double)row[62],
                         credit9 = (double)row[63],
                         credit10 = (double)row[64],
                         credit11 = (double)row[65],
                         credit12 = (double)row[66],
                         credit1ny = (double)row[67],
                         credit2ny = (double)row[68],
                         credit3ny = (double)row[69],
                         credit4ny = (double)row[70],
                         credit5ny = (double)row[71],
                         credit6ny = (double)row[72],
                         credit7ny = (double)row[73],
                         credit8ny = (double)row[74],
                         credit9ny = (double)row[75],
                         credit10ny = (double)row[76],
                         credit11ny = (double)row[77],
                         credit12ny = (double)row[78],
                    });
                }
            }
            catch (IOException ex)
            {
                if (MetroMessageBox.Show(this, ex.Message, "เกิดข้อผิดพลาด :", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, 370) == DialogResult.Retry)
                {
                    this.LoadGlbal();
                }
                else
                {
                    return;
                }
            }
        }

        private void CreateXMLFile(string path)
        {
            //using (XmlWriter writer = XmlWriter.Create(path))
            //{
            //    writer.WriteStartDocument(true);
            //    writer.WriteStartElement("express_gl");

            //    foreach (Taxonomy taxo in this.taxonomy_list)
            //    {
            //        writer.WriteStartElement("Taxononomy", taxo.taxodesc);//("Taxonomy", "group", taxo.group);
            //        writer.WriteElementString("GROUP", taxo.group);
            //        writer.WriteElementString("CODE", taxo.code);
            //        writer.WriteElementString("NAME", taxo.name);
            //        writer.WriteEndElement();
            //    }

            //    writer.WriteEndElement();
            //    writer.WriteEndDocument();
            //}
            XNamespace ns_m = "urn:schemas-microsoft-com:office:spreadsheet";
            XNamespace ns_o = "urn:schemas-microsoft-com:office:office";
            XNamespace ns_x = "urn:schemas-microsoft-com:office:excel";
            XNamespace ns_ss = "urn:schemas-microsoft-com:office:spreadsheet";

            XElement table_elem = new XElement(ns_ss + "Table", new XAttribute(ns_ss + "ExpandedColumnCount", "8"), new XAttribute(ns_ss + "ExpandedRowCount", (this.list_posting_item.Where(g => g.acctyp == "0").Count<dataItem4Glacc>() + 2).ToString()),
                new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                new XElement(ns_ss + "Column", new XAttribute(ns_ss + "AutoFitWidth", "1")),
                new XElement(ns_ss + "Row", new XAttribute(ns_ss + "row_type", "report-header"),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "1"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "ตั้งแต่วันที่")
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "2"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), this.dtFrom.Value.ToString("yyyy-MM-dd", new CultureInfo("en-US")))
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "3"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "ถึงวันที่")
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "4"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), this.dtTo.Value.ToString("yyyy-MM-dd", new CultureInfo("en-US")))
                    )
                ),
                new XElement(ns_ss + "Row", new XAttribute(ns_ss + "row_type", "column-header"),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "1"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "qntaxodesc")
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "2"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "qctaxodesc")
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "3"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "qcacchart")
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "4"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "qnacchart")
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "5"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "bfamt")
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "6"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "perddramt")
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "7"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "perdcramt")
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "8"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), "cfamt")
                    )
                )
            );
            XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(ns_m + "Workbook", new XAttribute("xmlns", ns_m), new XAttribute(XNamespace.Xmlns + "o", ns_o), new XAttribute(XNamespace.Xmlns + "x", ns_x), new XAttribute(XNamespace.Xmlns + "ss", ns_ss),
                    new XElement(ns_ss + "Worksheet", new XAttribute(ns_ss + "Name", "Express_GL_Data"),
                        table_elem
                    )
                )
            );

            foreach (dataItem4Glacc item in this.list_posting_item.Where(g => g.acctyp == "0").OrderBy(g => g.group).ToList<dataItem4Glacc>())
            {
                AccAmountInfo amount = this.GetBalFwd(item, this.dtFrom.Value, this.dtTo.Value);

                table_elem.Add(new XElement(ns_ss + "Row", new XAttribute(ns_ss + "row_type", "data"),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "1"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), this.taxonomy_list.Find(t => t.taxodesc == item.taxonomy).name)
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "2"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), item.taxonomy)
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "3"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), item.accnum)
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "4"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "String"), item.accnam.Trim())
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "5"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "Number"), amount.bal_fwd.ToString())
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "6"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "Number"), amount.prd_dr.ToString())
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "7"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "Number"), amount.prd_cr.ToString())
                    ),
                    new XElement(ns_ss + "Cell", new XAttribute(ns_ss + "Index", "8"),
                        new XElement(ns_ss + "Data", new XAttribute(ns_ss + "Type", "Number"), amount.carr_fwd.ToString())
                    ))
                );
                //table_elem.Add(new XElement(""))
            }

            xdoc.Save(path);
            MessageBox.Show("save complete");
            
        }

        private AccAmountInfo GetBalFwd(dataItem4Glacc acc, DateTime from_date, DateTime to_date)
        {
            List<Glbal> glbal = (this.list_glbal.Where(g => g.accnum.Trim() == acc.accnum.Trim()).Count<Glbal>() > 0 ? this.list_glbal.Where(g => g.accnum.Trim() == acc.accnum.Trim()).ToList<Glbal>() : new List<Glbal>());
            List<Gljnlit> gljnlit = (this.list_gljnlit.Where(g => g.accnum.Trim() == acc.accnum.Trim()).Count<Gljnlit>() > 0 ? this.list_gljnlit.Where(g => g.accnum.Trim() == acc.accnum.Trim()).ToList<Gljnlit>() : new List<Gljnlit>());
            double tmp_dr = 0d;
            double tmp_cr = 0d;

            AccAmountInfo info = new AccAmountInfo();
            info.bal_fwd = 0d;
            info.prd_dr = 0d;
            info.prd_cr = 0d;
            info.carr_fwd = 0d;

            // Calculate Balance Forward
            tmp_dr += gljnlit.Where(g => this.CompareDate(g.voudat, from_date) < 0 && g.trntyp.Trim() == "0").Sum(g => g.amount); // Debit trntyp = "0"
            //if (acc.accnum.Trim() == "1111-00")
            //{
            //    foreach (Gljnlit x in gljnlit.Where(g => this.CompareDate(g.voudat, from_date) < 0 && g.trntyp.Trim() == "0").ToList<Gljnlit>())
            //    {
            //        Console.WriteLine(" >>> .. Voucher # " + x.voucher + " เลขที่บัญชี " + x.accnum + " ยอดเงิน " + x.amount + " voudat " + x.voudat.ToString() + " from_date " + from_date.ToString());
            //    }
            //    Console.WriteLine(" >>>> ... รวม " + gljnlit.Where(g => this.CompareDate(g.voudat, from_date) < 0 && g.trntyp.Trim() == "0").Sum(g => g.amount));
            //}
            tmp_dr += glbal.Sum(g => g.debit1ly);
            tmp_dr += glbal.Sum(g => g.debit2ly);
            tmp_dr += glbal.Sum(g => g.debit3ly);
            tmp_dr += glbal.Sum(g => g.debit4ly);
            tmp_dr += glbal.Sum(g => g.debit5ly);
            tmp_dr += glbal.Sum(g => g.debit6ly);
            tmp_dr += glbal.Sum(g => g.debit7ly);
            tmp_dr += glbal.Sum(g => g.debit8ly);
            tmp_dr += glbal.Sum(g => g.debit9ly);
            tmp_dr += glbal.Sum(g => g.debit10ly);
            tmp_dr += glbal.Sum(g => g.debit11ly);
            tmp_dr += glbal.Sum(g => g.debit12ly);

            tmp_cr += gljnlit.Where(g => this.CompareDate(g.voudat, from_date) < 0 && g.trntyp.Trim() == "1").Sum(g => g.amount); // Credit trntyp = "1"
            //if (acc.accnum.Trim() == "1111-00")
            //{
            //    foreach (Gljnlit x in gljnlit.Where(g => this.CompareDate(g.voudat, from_date) < 0 && g.trntyp.Trim() == "1").ToList<Gljnlit>())
            //    {
            //        Console.WriteLine(" >>> .. Voucher # " + x.voucher + " เลขที่บัญชี " + x.accnum + " ยอดเงิน " + x.amount + " voudat " + x.voudat.ToString() + " from_date " + from_date.ToString());
            //    }
            //    Console.WriteLine(" >>>> ... รวม " + gljnlit.Where(g => this.CompareDate(g.voudat, from_date) < 0 && g.trntyp.Trim() == "1").Sum(g => g.amount));
            //}
            tmp_cr += glbal.Sum(g => g.credit1ly);
            tmp_cr += glbal.Sum(g => g.credit2ly);
            tmp_cr += glbal.Sum(g => g.credit3ly);
            tmp_cr += glbal.Sum(g => g.credit4ly);
            tmp_cr += glbal.Sum(g => g.credit5ly);
            tmp_cr += glbal.Sum(g => g.credit6ly);
            tmp_cr += glbal.Sum(g => g.credit7ly);
            tmp_cr += glbal.Sum(g => g.credit8ly);
            tmp_cr += glbal.Sum(g => g.credit9ly);
            tmp_cr += glbal.Sum(g => g.credit10ly);
            tmp_cr += glbal.Sum(g => g.credit11ly);
            tmp_cr += glbal.Sum(g => g.credit12ly);

            if (acc.nature == "0") // Nature Debit
            {
                info.bal_fwd = glbal.Sum(g => g.begly) + (tmp_dr - tmp_cr);
            }
            else // Nature Credit
            {
                info.bal_fwd = (glbal.Sum(g => g.begly) * -1) + (tmp_cr - tmp_dr);
            }


            // Calculate Period Movement
            info.prd_dr = gljnlit.Where(g => this.CompareDate(g.voudat, from_date) >= 0 && this.CompareDate(g.voudat, to_date) <= 0 && g.trntyp == "0").Sum(g => g.amount); // Debit trntyp = "0"
            info.prd_cr = gljnlit.Where(g => this.CompareDate(g.voudat, from_date) >= 0 && this.CompareDate(g.voudat, to_date) <= 0 && g.trntyp == "1").Sum(g => g.amount); // Credit trntyp = "1"


            // Calculate Carry Forward
            if (acc.nature == "0") // Nature Debit
            {
                info.carr_fwd = info.bal_fwd + (info.prd_dr - info.prd_cr);
            }
            else // Nature Credit
            {
                info.carr_fwd = info.bal_fwd + (info.prd_cr - info.prd_dr);
            }

            
            return info;
        }

        private int CompareDate(DateTime first_date, DateTime second_date)
        {
            CultureInfo cinfo_en = new CultureInfo("th-TH");

            DateTime only_date_first = DateTime.Parse(first_date.ToString("yyyy-MM-dd", cinfo_en).Substring(0,10));
            DateTime only_date_second = DateTime.Parse(second_date.ToString("yyyy-MM-dd", cinfo_en).Substring(0,10));

            return DateTime.Compare(only_date_first, only_date_second);
        }
        #endregion 3rd Tab

        private void SetSelectedTab(TabPage tabpage)
        {
            this.prevent_change_tab = false;
            this.tabControlStep.SelectedTab = tabpage;
            this.prevent_change_tab = true;
        }

        private void tabControlStep_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (this.prevent_change_tab)
            {
                e.Cancel = true;
                return;
            }
        }

        private void Step1_Init()
        {
            this.btnNext2Glacc.Enabled = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.list_glacc_item.Count > 0)
            {
                DialogResult result = MetroMessageBox.Show(this, "ต้องการบันทึกข้อมูล ก่อนปิดโปรแกรมหรือไม่", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    this.SaveTaxoDescDB();
                    return;
                }
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (this.tabControlStep.SelectedTab == this.tabStep1)
                {
                    if (this.dgvComp.CurrentCell != null)
                    {
                        this.btnNext2Glacc.PerformClick();
                        return true;
                    }
                }

                if (this.tabControlStep.SelectedTab == this.tabStep3)
                {
                    if (this.btnExportXML.Focused)
                    {
                        return false;
                    }

                    SendKeys.Send("{TAB}");
                    return true;
                }

                if (this.cb_taxonomy != null && this.cb_taxonomy._Focused)
                {
                    if (this.cb_taxonomy._Text.Trim().Length > 0)
                    {
                        if (this.ValidateTaxonomy(this.cb_taxonomy._Text.Trim(), this.selected_acc.group.Trim()) == true)
                        {
                            this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum).taxonomy = this.cb_taxonomy._Text.Trim();
                            SendKeys.Send("{TAB}");
                        }
                        else
                        {
                            this.cb_taxonomy.btnBrowse.PerformClick();
                        }
                    }
                    else
                    {
                        this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum).taxonomy = "";
                        SendKeys.Send("{TAB}");
                    }

                    this.SetStatText(this.tabControlGlacc.SelectedTab);
                    this.CheckCompleteFillTaxodesc();
                    return true;
                }
            }

            if (keyData == Keys.F6)
            {
                if (this.dtFrom.Focused || this.dtTo.Focused)
                {
                    SendKeys.Send("{F4}");
                    return true;
                }
            }

            if (keyData == Keys.Tab)
            {
                if (this.cb_taxonomy != null && this.cb_taxonomy._Focused)
                {
                    if (this.list_glacc_item.Find(t => t.accnum.Trim() == this.selected_acc.accnum.Trim()) != null)
                    {
                        dataItem4Glacc current_item = this.list_glacc_item.Find(t => t.accnum.Trim() == this.selected_acc.accnum.Trim());
                        dataItem4Glacc next_item = this.GetNextPostItem(current_item);
                        if (next_item != null) // At least 1 account next
                        {
                            if (next_item.group == "1") // Is assets
                            {
                                this.tabControlGlacc.SelectedTab = this.tabAsset;
                                if (this.dgvGlaccAsset.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).Count<DataGridViewRow>() > 0)
                                    this.dgvGlaccAsset.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).First<DataGridViewRow>().Cells[1].Selected = true;

                                return true;
                            }

                            if (next_item.group == "2") // Is Liabilities
                            {
                                this.tabControlGlacc.SelectedTab = this.tabLia;
                                if (this.dgvGlaccLia.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).Count<DataGridViewRow>() > 0)
                                    this.dgvGlaccLia.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).First<DataGridViewRow>().Cells[1].Selected = true;
                                return true;
                            }

                            if (next_item.group == "3") // Is Capitalize
                            {
                                this.tabControlGlacc.SelectedTab = this.tabCap;
                                if (this.dgvGlaccCap.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).Count<DataGridViewRow>() > 0)
                                    this.dgvGlaccCap.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).First<DataGridViewRow>().Cells[1].Selected = true;
                                return true;
                            }

                            if (next_item.group == "4") // Is Revenue
                            {
                                this.tabControlGlacc.SelectedTab = this.tabRev;
                                if (this.dgvGlaccRev.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).Count<DataGridViewRow>() > 0)
                                    this.dgvGlaccRev.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).First<DataGridViewRow>().Cells[1].Selected = true;
                                return true;
                            }

                            if (next_item.group == "5") // Is Expense
                            {
                                this.tabControlGlacc.SelectedTab = this.tabExp;
                                if (this.dgvGlaccExp.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).Count<DataGridViewRow>() > 0)
                                    this.dgvGlaccExp.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).First<DataGridViewRow>().Cells[1].Selected = true;
                                return true;
                            }

                        }
                        else // It's the last account
                        {
                            this.ClearInlineTaxodescSelector();
                            //Console.WriteLine(" >> This is the last item");
                        }
                    }

                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
