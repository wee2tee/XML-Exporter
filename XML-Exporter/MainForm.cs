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
using CustomControl;
using System.Data.SQLite;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using CSGDBDConnectLib;
using Comm;
using Newtonsoft.Json;
using System.Reflection;

namespace XML_Exporter
{
    public partial class MainForm : Form
    {
        #region public object/vars
        public string export_path = "";
        //public string token_key = string.Empty;
        public LOC_RegisterData local_reg = null;
        public List<dataItem4Glacc> list_glacc_item; // storing account sequence in parent-child format
        public List<Taxonomy> taxonomy_list;
        public HardwareInfo hw_info;
        public string taxonomy_db_path;
        #endregion public object/vars

        #region private object/vars
        //private Helper csgHelper;

        //private BindingSource bs_sccomp;
        private BindingSource bs_glacc_asset;
        private BindingSource bs_glacc_lia;
        private BindingSource bs_glacc_cap;
        private BindingSource bs_glacc_rev;
        private BindingSource bs_glacc_exp;

        private Sccomp selected_comp;
        private Glacc selected_acc;

        private Isinfo isinfo; // storing isinfo data
        private Isprd isprd; // storing isprd data
        private List<dataItem4Sccomp> list_sccomp_item; // storing company name
        private List<dataItem4Glacc> tmp_list; // before recursing to parent-child format account sequence
        private List<dataItem4Glacc> list_posting_item; // storing account sequence in parent-child format in final step(use to generate trial balance or generate xml file)

        private CustomBrowseField cb_taxonomy;
        //private CustomButtonLabel btn_taxonomy2;
        private CustomBrowseField btn_taxonomy2;
        private enum TAXO_TYPE
        {
            NATURE,
            OPP_NATURE
        }
        private ACCNAM_REGION accnam_region;
        private enum ACCNAM_REGION
        {
            ENG,
            THA
        }
        #endregion private object/vars

        public MainForm()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            InitializeComponent();
        }

        public MainForm(string[] args) // With data path passing
            : this()
        {
            if (args.Count() > 0)
            {
                try
                {
                    string cmd_line = Environment.CommandLine;
                    int quote1 = cmd_line.IndexOf("'", 0);
                    int quote2 = cmd_line.IndexOf("'", quote1 + 1);
                    int quote3 = cmd_line.IndexOf("'", quote2 + 1);
                    int quote4 = cmd_line.IndexOf("'", quote3 + 1);

                    string path = cmd_line.Substring(quote1 + 1, quote2 - quote1 - 1).RemoveBracketFromPath();
                    string user_id = cmd_line.Substring(quote3 + 1, quote4 - quote3 - 1);

                    Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

                    if (File.Exists(path.ToLower() + "/isinfo.dbf"))
                    {
                        this.selected_comp = new Sccomp()
                        {
                            path = path
                        };
                        this.isinfo = this.LoadIsinfo();
                        this.taxonomy_db_path = (path.Contains(":\\") ? path : AppResource.PATH_PREFIX + path);
                        this.btnCopy.Enabled = true;
                        this.rbAccnamEng.Enabled = true;
                        this.rbAccnamThai.Enabled = true;
                    }
                    else
                    {
                        this.taxonomy_db_path = string.Empty;
                        this.btnCopy.Enabled = false;
                        this.rbAccnamEng.Enabled = false;
                        this.rbAccnamThai.Enabled = false;
                    }
                }
                catch(Exception ex)
                {
                    
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //SRV_RegisterData srv_regdata = null;

            //try
            //{
            //    srv_regdata = Licensing.GetSrvRegisterData(RegisterDialog.ReadSernum(), Licensing.GetHDDSerialNumber(Licensing.GetSystemDriveLetter()));
            //}
            //catch (Exception ex)
            //{

            //}
            //if (srv_regdata != null)
            //{
            //    if (srv_regdata.status_code == ((int)Licensing.STATUS.REGISTERED).ToString())
            //    {
            //        this.btnRegister.Enabled = false;
            //    }
            //}
            this.Text = AppResource.APP_NAME;
            this.accnam_region = ACCNAM_REGION.THA;

            Licensing.CreateTokenKeyDB();
            local_reg = Licensing.GetLocalRegData();

            this.LoadControlEventHandler();
            this.list_sccomp_item = new List<dataItem4Sccomp>();
            this.list_glacc_item = new List<dataItem4Glacc>();
            this.list_posting_item = new List<dataItem4Glacc>();

            this.taxonomy_list = Taxonomy.GetTaxonomyList(this);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            DateTime assembly_date = Assembly.GetExecutingAssembly().GetLinkerTime();
            this.ProgramDateLabel.Text = "วันที่โปรแกรม : " + assembly_date.ToString("dd/MM/yy (HHmmss)", new CultureInfo(CultureInfo.CurrentCulture.Name, true));
            this.lblProgramPath.Text = AppDomain.CurrentDomain.BaseDirectory;

            if (this.selected_comp == null) // Run program without passing data path
            {
                this.btnOpenData.PerformClick();
            }
            else // Run Program with passing data path
            {
                this.LoadDataOfSelectedCompany();
            }
        }

        private void LoadControlEventHandler()
        {
            this.tabControlGlacc.SelectedIndexChanged += delegate
            {
                this.ClearInlineTaxodescSelector();
                if (this.tabControlGlacc.SelectedTab == this.tabAsset)
                {
                    this.dgvGlaccAsset.Focus();
                    if (this.dgvGlaccAsset.Rows.Count > 0)
                        this.dgvGlaccAsset.Rows[0].Cells[1].Selected = true;
                    this.SetStatText(this.tabAsset);
                }
                if (this.tabControlGlacc.SelectedTab == this.tabLia)
                {
                    this.dgvGlaccLia.Focus();
                    if (this.dgvGlaccLia.Rows.Count > 0)
                        this.dgvGlaccLia.Rows[0].Cells[1].Selected = true;
                    this.SetStatText(this.tabLia);
                }
                if (this.tabControlGlacc.SelectedTab == this.tabCap)
                {
                    this.dgvGlaccCap.Focus();
                    if (this.dgvGlaccCap.Rows.Count > 0)
                        this.dgvGlaccCap.Rows[0].Cells[1].Selected = true;
                    this.SetStatText(this.tabCap);
                }
                if (this.tabControlGlacc.SelectedTab == this.tabRev)
                {
                    this.dgvGlaccRev.Focus();
                    if (this.dgvGlaccRev.Rows.Count > 0)
                        this.dgvGlaccRev.Rows[0].Cells[1].Selected = true;
                    this.SetStatText(this.tabRev);
                }
                if (this.tabControlGlacc.SelectedTab == this.tabExp)
                {
                    this.dgvGlaccExp.Focus();
                    if (this.dgvGlaccExp.Rows.Count > 0)
                        this.dgvGlaccExp.Rows[0].Cells[1].Selected = true;
                    this.SetStatText(this.tabExp);
                }
            };

            this.dgvGlaccAsset.CurrentCellChanged += new EventHandler(this.SetCurrentSelectedAcc);
            this.dgvGlaccLia.CurrentCellChanged += new EventHandler(this.SetCurrentSelectedAcc);
            this.dgvGlaccCap.CurrentCellChanged += new EventHandler(this.SetCurrentSelectedAcc);
            this.dgvGlaccRev.CurrentCellChanged += new EventHandler(this.SetCurrentSelectedAcc);
            this.dgvGlaccExp.CurrentCellChanged += new EventHandler(this.SetCurrentSelectedAcc);

            this.dgvGlaccAsset.Paint += new PaintEventHandler(this.DrawRowBorder4Glacc);
            this.dgvGlaccLia.Paint += new PaintEventHandler(this.DrawRowBorder4Glacc);
            this.dgvGlaccCap.Paint += new PaintEventHandler(this.DrawRowBorder4Glacc);
            this.dgvGlaccRev.Paint += new PaintEventHandler(this.DrawRowBorder4Glacc);
            this.dgvGlaccExp.Paint += new PaintEventHandler(this.DrawRowBorder4Glacc);

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

        public void FillForm()
        {
            if (this.selected_comp != null)
            {
                this.lblCompNam.Text = this.isinfo.thinam; //this.selected_comp.compnam;
                this.lblCompCod.Text = this.selected_comp.compcod;
                this.lblDataPath.Text = (this.selected_comp.path.Contains(":\\") ? this.selected_comp.path : Path.GetFullPath("../" + this.selected_comp.path));

                this.bs_glacc_asset = new BindingSource();
                this.bs_glacc_asset.DataSource = this.list_glacc_item.Where(g => g.group == "1").ToList<dataItem4Glacc>();
                this.dgvGlaccAsset.DataSource = this.bs_glacc_asset;
                this.SetDgvGlaccVisualStyle(this.dgvGlaccAsset);

                this.bs_glacc_lia = new BindingSource();
                this.bs_glacc_lia.DataSource = this.list_glacc_item.Where(g => g.group == "2").ToList<dataItem4Glacc>();
                this.dgvGlaccLia.DataSource = this.bs_glacc_lia;
                this.SetDgvGlaccVisualStyle(this.dgvGlaccLia);

                this.bs_glacc_cap = new BindingSource();
                this.bs_glacc_cap.DataSource = this.list_glacc_item.Where(g => g.group == "3").ToList<dataItem4Glacc>();
                this.dgvGlaccCap.DataSource = this.bs_glacc_cap;
                this.SetDgvGlaccVisualStyle(this.dgvGlaccCap);

                this.bs_glacc_rev = new BindingSource();
                this.bs_glacc_rev.DataSource = this.list_glacc_item.Where(g => g.group == "4").ToList<dataItem4Glacc>();
                this.dgvGlaccRev.DataSource = this.bs_glacc_rev;
                this.SetDgvGlaccVisualStyle(this.dgvGlaccRev);

                this.bs_glacc_exp = new BindingSource();
                this.bs_glacc_exp.DataSource = this.list_glacc_item.Where(g => g.group == "5").ToList<dataItem4Glacc>();
                this.dgvGlaccExp.DataSource = this.bs_glacc_exp;
                this.SetDgvGlaccVisualStyle(this.dgvGlaccExp);

                this.bs_glacc_asset.ResetBindings(true);
                this.bs_glacc_lia.ResetBindings(true);
                this.bs_glacc_cap.ResetBindings(true);
                this.bs_glacc_rev.ResetBindings(true);
                this.bs_glacc_exp.ResetBindings(true);

                this.SetStatText(this.tabControlGlacc.SelectedTab);

            }
        }

        public void DrawRowBorder(object sender, PaintEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            if (dgv.CurrentCell == null)
                return;

            using (Pen p = new Pen(Color.Red))
            {
                Rectangle rect = dgv.GetRowDisplayRectangle(dgv.CurrentCell.RowIndex, false);
                e.Graphics.DrawLine(p, rect.X, rect.Y, rect.X + rect.Width, rect.Y);
                e.Graphics.DrawLine(p, rect.X, rect.Y + rect.Height - 2, rect.X + rect.Width, rect.Y + rect.Height - 2);
            }
        }

        private void DrawRowBorder4Glacc(object sender, PaintEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            if (dgv.CurrentCell == null)
                return;

            if (((Glacc)dgv.Rows[dgv.CurrentCell.RowIndex].Cells[0].Value).acctyp == "1")
                return;

            using (Pen p = new Pen(Color.Red))
            {
                Rectangle rect = dgv.GetRowDisplayRectangle(dgv.CurrentCell.RowIndex, false);
                e.Graphics.DrawLine(p, rect.X, rect.Y, rect.X + rect.Width, rect.Y);
                e.Graphics.DrawLine(p, rect.X, rect.Y + rect.Height - 2, rect.X + rect.Width, rect.Y + rect.Height - 2);
            }
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
                this.ClearInlineTaxodescSelector();

                this.selected_acc = (Glacc)target_dgv.Rows[target_dgv.CurrentCell.RowIndex].Cells[0].Value;

                if (((Glacc)target_dgv.Rows[target_dgv.CurrentCell.RowIndex].Cells[0].Value).acctyp == "0")
                {
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
                    else if (cell.ColumnIndex == 13)
                    {
                        cell.Style.ForeColor = Color.Red;
                        cell.Style.SelectionForeColor = Color.Red;
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
            int col_index = ((DataGridView)sender).HitTest(e.X, e.Y).ColumnIndex;
            if (row_index > -1)
            {
                this.ClearInlineTaxodescSelector();
                this.ShowInlineTaxodescSelector(((DataGridView)sender).Rows[((DataGridView)sender).CurrentCell.RowIndex]);
                if (col_index == 12)
                {
                    if (this.cb_taxonomy != null)
                        this.cb_taxonomy.Focus();
                }

                if (col_index == 13)
                {
                    if (this.btn_taxonomy2 != null)
                        this.btn_taxonomy2.Focus();
                }
            }
        }

        private void SetStatText(TabPage tabpage)
        {
            if (tabpage == this.tabAsset)
            {
                if (this.list_glacc_item.Where(g => g.group == "1" && g.acctyp == "0").Count<dataItem4Glacc>() == 0)
                {
                    this.lblInfo.Text = "";
                    return;
                }

                this.lblInfo.Text = "บัญชีย่อยในหมวดสินทรัพย์ทั้งหมด " + this.list_glacc_item.Where(g => g.group == "1" && g.acctyp == "0").Count<dataItem4Glacc>().ToString() + " บัญชี , กำหนด Taxonomy ไว้แล้ว " + this.list_glacc_item.Where(g => g.group == "1" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>() + " บัญชี";
                if (this.list_glacc_item.Where(g => g.group == "1" && g.acctyp == "0").Count<dataItem4Glacc>() == this.list_glacc_item.Where(g => g.group == "1" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>())
                {
                    this.lblInfo.ForeColor = AppResource.COLOR_SUCCESS;
                }
                else
                {
                    this.lblInfo.ForeColor = AppResource.COLOR_ALERT;
                }
                return;
            }
            if (tabpage == this.tabLia)
            {
                if (this.list_glacc_item.Where(g => g.group == "2" && g.acctyp == "0").Count<dataItem4Glacc>() == 0)
                {
                    this.lblInfo.Text = "";
                    return;
                }

                this.lblInfo.Text = "บัญชีย่อยในหมวดหนี้สินทั้งหมด " + this.list_glacc_item.Where(g => g.group == "2" && g.acctyp == "0").Count<dataItem4Glacc>().ToString() + " บัญชี , กำหนด Taxonomy ไว้แล้ว " + this.list_glacc_item.Where(g => g.group == "2" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>() + " บัญชี";
                if (this.list_glacc_item.Where(g => g.group == "2" && g.acctyp == "0").Count<dataItem4Glacc>() == this.list_glacc_item.Where(g => g.group == "2" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>())
                {
                    this.lblInfo.ForeColor = AppResource.COLOR_SUCCESS;
                }
                else
                {
                    this.lblInfo.ForeColor = AppResource.COLOR_ALERT;
                }
                return;
            }
            if (tabpage == this.tabCap)
            {
                if (this.list_glacc_item.Where(g => g.group == "3" && g.acctyp == "0").Count<dataItem4Glacc>() == 0)
                {
                    this.lblInfo.Text = "";
                    return;
                }

                this.lblInfo.Text = "บัญชีย่อยในหมวดทุนทั้งหมด " + this.list_glacc_item.Where(g => g.group == "3" && g.acctyp == "0").Count<dataItem4Glacc>().ToString() + " บัญชี , กำหนด Taxonomy ไว้แล้ว " + this.list_glacc_item.Where(g => g.group == "3" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>() + " บัญชี";
                if (this.list_glacc_item.Where(g => g.group == "3" && g.acctyp == "0").Count<dataItem4Glacc>() == this.list_glacc_item.Where(g => g.group == "3" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>())
                {
                    this.lblInfo.ForeColor = AppResource.COLOR_SUCCESS;
                }
                else
                {
                    this.lblInfo.ForeColor = AppResource.COLOR_ALERT;
                }
                return;
            }
            if (tabpage == this.tabRev)
            {
                if (this.list_glacc_item.Where(g => g.group == "4" && g.acctyp == "0").Count<dataItem4Glacc>() == 0)
                {
                    this.lblInfo.Text = "";
                    return;
                }

                this.lblInfo.Text = "บัญชีย่อยในหมวดรายได้ทั้งหมด " + this.list_glacc_item.Where(g => g.group == "4" && g.acctyp == "0").Count<dataItem4Glacc>().ToString() + " บัญชี , กำหนด Taxonomy ไว้แล้ว " + this.list_glacc_item.Where(g => g.group == "4" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>() + " บัญชี";
                if (this.list_glacc_item.Where(g => g.group == "4" && g.acctyp == "0").Count<dataItem4Glacc>() == this.list_glacc_item.Where(g => g.group == "4" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>())
                {
                    this.lblInfo.ForeColor = AppResource.COLOR_SUCCESS;
                }
                else
                {
                    this.lblInfo.ForeColor = AppResource.COLOR_ALERT;
                }
                return;
            }
            if (tabpage == this.tabExp)
            {
                if (this.list_glacc_item.Where(g => g.group == "5" && g.acctyp == "0").Count<dataItem4Glacc>() == 0)
                {
                    this.lblInfo.Text = "";
                    return;
                }

                this.lblInfo.Text = "บัญชีย่อยในหมวดค่าใช้จ่ายทั้งหมด " + this.list_glacc_item.Where(g => g.group == "5" && g.acctyp == "0").Count<dataItem4Glacc>().ToString() + " บัญชี , กำหนด Taxonomy ไว้แล้ว " + this.list_glacc_item.Where(g => g.group == "5" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>() + " บัญชี";
                if (this.list_glacc_item.Where(g => g.group == "5" && g.acctyp == "0").Count<dataItem4Glacc>() == this.list_glacc_item.Where(g => g.group == "5" && g.acctyp == "0" && g.taxonomy.Length > 0).Count<dataItem4Glacc>())
                {
                    this.lblInfo.ForeColor = AppResource.COLOR_SUCCESS;
                }
                else
                {
                    this.lblInfo.ForeColor = AppResource.COLOR_ALERT;
                }
                return;
            }
        }

        private void ShowInlineTaxodescSelector(DataGridViewRow row, TAXO_TYPE taxo_type = TAXO_TYPE.NATURE)
        {
            if (((Glacc)row.Cells[0].Value).acctyp == "1")
                return;

            if (this.cb_taxonomy != null || this.btn_taxonomy2 != null)
                this.ClearInlineTaxodescSelector();

            this.btn_taxonomy2 = new CustomBrowseField();
            this.btn_taxonomy2._ForeColor = Color.Red;
            this.btn_taxonomy2._BackgroundColor = Color.FromArgb(195, 252, 155);
            this.btn_taxonomy2._BorderColor = Color.LightGray;
            this.btn_taxonomy2._Text = (row.Cells[13].Value != null && ((string)row.Cells[13].Value).Trim().Length > 0 ? (string)row.Cells[13].Value : "");
            this.btn_taxonomy2._Enabled = true;
            this.btn_taxonomy2.btnBrowse.Click += delegate
            {
                //this.ClearInlineTaxodescSelector(true, false);

                Glacc curr_acc = (Glacc)row.Cells[0].Value;
                string acc_group = curr_acc.group;
                Taxonomy curr_taxonomy = this.taxonomy_list.Find(t => t.taxodesc == this.list_glacc_item.Find(g => g.accnum == curr_acc.accnum).taxonomy2);

                ListTaxodesc td = new ListTaxodesc(this, ListTaxodesc.TAXO_TYPE.OPPOSITE_NATURE, curr_acc, this.taxonomy_list, curr_taxonomy);
                td.StartPosition = FormStartPosition.Manual;

                Point cb_point = this.btn_taxonomy2.PointToScreen(Point.Empty);
                int screen_height = SystemInformation.VirtualScreen.Height;
                if (cb_point.Y <= screen_height - (this.btn_taxonomy2.ClientSize.Height + td.ClientSize.Height + 70)) // drop-down
                {
                    td.Location = new Point(cb_point.X - (td.ClientSize.Width + 15) + this.btn_taxonomy2.ClientSize.Width, cb_point.Y + this.btn_taxonomy2.ClientSize.Height);
                }
                else // drop-up
                {
                    td.Location = new Point(cb_point.X - (td.ClientSize.Width + 15) + this.btn_taxonomy2.ClientSize.Width, cb_point.Y - (td.ClientSize.Height + 40));
                }

                if (td.ShowDialog() == DialogResult.OK)
                {
                    this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum).taxonomy2 = td.current_taxonomy.taxodesc;
                    this.SaveTaxoDescDB(this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum));
                    //this.SetStatText(this.tabControlGlacc.SelectedTab);

                    this.RefreshDGV();
                    this.ClearInlineTaxodescSelector();

                    //this.CheckCompleteFillTaxodesc();
                    //this.SetFocusToNextItem();
                }
            };
            row.DataGridView.Parent.Controls.Add(this.btn_taxonomy2);

            this.cb_taxonomy = new CustomBrowseField();
            this.cb_taxonomy.Name = "cb_taxonomy";
            this.cb_taxonomy.Tag = taxo_type;
            this.cb_taxonomy._ForeColor = Color.Blue;
            this.cb_taxonomy._BackgroundColor = Color.FromArgb(195, 252, 155);
            this.cb_taxonomy._BorderColor = Color.LightGray;
            this.cb_taxonomy._Text = (((string)row.Cells[12].Value).Trim().Length > 0 ? (string)row.Cells[12].Value : "");
            this.cb_taxonomy._Enabled = true;
            #region no use
            //this.cb_taxonomy.Leave += delegate
            //{
            //    if (this.cb_taxonomy._Text.Trim().Length > 0) // taxonomy has specified
            //    {
            //        if ((TAXO_TYPE)this.cb_taxonomy.Tag == TAXO_TYPE.NATURE) // nature taxonomy of an account
            //        {
            //            if (this.ValidateTaxonomy(this.cb_taxonomy._Text, this.selected_acc.group.Trim())) // validate only in acc_group taxonomy
            //            {
            //                this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum).taxonomy1 = this.cb_taxonomy._Text.Trim();
            //                this.SaveTaxoDescDB(this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum));
            //                this.SetStatText(this.tabControlGlacc.SelectedTab);

            //                this.RefreshDGV();
            //                this.CheckCompleteFillTaxodesc();
            //            }
            //            else
            //            {
            //                this.cb_taxonomy.Focus();
            //                this.cb_taxonomy.btnBrowse.PerformClick();
            //                return;
            //            }
            //        }
            //        else // opposite nature taxonomy of an account
            //        {
            //            if (this.ValidateTaxonomy(this.cb_taxonomy._Text)) // validate with all taxonomy
            //            {
            //                this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum).taxonomy2 = this.cb_taxonomy._Text.Trim();
            //                this.SaveTaxoDescDB(this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum));
            //                this.SetStatText(this.tabControlGlacc.SelectedTab);

            //                this.RefreshDGV();
            //            }
            //            else
            //            {
            //                this.cb_taxonomy.Focus();
            //                this.cb_taxonomy.btnBrowse.PerformClick();
            //                return;
            //            }
            //        }
            //    }
            //    else // taxonomy not specified
            //    {
            //        if ((TAXO_TYPE)this.cb_taxonomy.Tag == TAXO_TYPE.NATURE) // nature taxonomy of an account
            //        {
            //            this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum).taxonomy1 = "";
            //        }
            //        else // opposite nature taxonomy of an account
            //        {
            //            this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum).taxonomy2 = "";
            //        }

            //        this.SaveTaxoDescDB(this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum));
            //        this.SetStatText(this.tabControlGlacc.SelectedTab);
            //        this.RefreshDGV();
            //    }
            //};
            #endregion no use
            this.cb_taxonomy.btnBrowse.Click += delegate
            {
                Glacc curr_acc = (Glacc)row.Cells[0].Value;
                string acc_group = curr_acc.group;
                Taxonomy curr_taxonomy = this.taxonomy_list.Find(t => t.taxodesc == this.list_glacc_item.Find(g => g.accnum == curr_acc.accnum).taxonomy);

                ListTaxodesc td = new ListTaxodesc(this, ListTaxodesc.TAXO_TYPE.NATURE, curr_acc, this.taxonomy_list.Where(t => t.group == acc_group).ToList<Taxonomy>(), curr_taxonomy);
                td.StartPosition = FormStartPosition.Manual;

                Point cb_point = this.cb_taxonomy.PointToScreen(Point.Empty);
                int screen_height = SystemInformation.VirtualScreen.Height;
                if (cb_point.Y <= screen_height - (this.cb_taxonomy.ClientSize.Height + td.ClientSize.Height + 70)) // drop-down
                {
                    td.Location = new Point(cb_point.X - (td.ClientSize.Width + 15) + this.cb_taxonomy.ClientSize.Width, cb_point.Y + this.cb_taxonomy.ClientSize.Height);
                }
                else // drop-up
                {
                    td.Location = new Point(cb_point.X - (td.ClientSize.Width + 15) + this.cb_taxonomy.ClientSize.Width, cb_point.Y - (td.ClientSize.Height + 40));
                }
                
                if (td.ShowDialog() == DialogResult.OK)
                {
                    this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum).taxonomy = td.current_taxonomy.taxodesc;
                    this.SaveTaxoDescDB(this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum));
                    this.SetStatText(this.tabControlGlacc.SelectedTab);

                    this.RefreshDGV();
                    this.ClearInlineTaxodescSelector();
                    #region no use
                    //if (acc_group == "1")
                    //{
                    //    this.dgvGlaccAsset.Refresh();
                    //    this.ClearInlineTaxodescSelector();
                    //}
                    //if (acc_group == "2")
                    //{
                    //    this.dgvGlaccLia.Refresh();
                    //    this.ClearInlineTaxodescSelector();
                    //}
                    //if (acc_group == "3")
                    //{
                    //    this.dgvGlaccCap.Refresh();
                    //    this.ClearInlineTaxodescSelector();
                    //}
                    //if (acc_group == "4")
                    //{
                    //    this.dgvGlaccRev.Refresh();
                    //    this.ClearInlineTaxodescSelector();
                    //}
                    //if (acc_group == "5")
                    //{
                    //    this.dgvGlaccExp.Refresh();
                    //    this.ClearInlineTaxodescSelector();
                    //}
                    #endregion no use

                    this.CheckCompleteFillTaxodesc();
                    this.SetFocusToNextItem();
                }
            };

            row.DataGridView.Parent.Controls.Add(this.cb_taxonomy);
            this.SetInlineTaxodescPosition(row);

            this.cb_taxonomy.BringToFront();
            this.btn_taxonomy2.BringToFront();
            row.DataGridView.SendToBack();
            this.cb_taxonomy.Focus();
        }

        private void SetInlineTaxodescPosition(DataGridViewRow row)
        {
            if (this.cb_taxonomy != null)
            {
                Rectangle rect_taxo1 = row.DataGridView.GetCellDisplayRectangle(12, row.Index, true);
                this.cb_taxonomy.SetBounds(rect_taxo1.X + 3, rect_taxo1.Y + 5, rect_taxo1.Width - 1, rect_taxo1.Height - 3);
            }

            if (this.btn_taxonomy2 != null)
            {
                Rectangle rect_taxo2 = row.DataGridView.GetCellDisplayRectangle(13, row.Index, true);
                //this.btn_taxonomy2.SetBounds(rect_taxo2.X + rect_taxo2.Width - (rect_taxo2.Height - 7), rect_taxo2.Y + 5, rect_taxo2.Height - 3, rect_taxo2.Height - 3);
                this.btn_taxonomy2.SetBounds(rect_taxo2.X + 3, rect_taxo2.Y + 5, rect_taxo2.Width, rect_taxo2.Height - 3);
            }
        }

        private void ClearInlineTaxodescSelector(bool clear_taxo1 = true, bool clear_taxo2 = true)
        {
            if (this.cb_taxonomy != null && clear_taxo1 == true)
            {
                this.cb_taxonomy.Dispose();
                this.cb_taxonomy = null;
            }

            if (this.btn_taxonomy2 != null && clear_taxo2 == true)
            {
                this.btn_taxonomy2.Dispose();
                this.btn_taxonomy2 = null;
            }
        }

        private void RefreshDGV()
        {
            if (this.tabControlGlacc.SelectedTab == this.tabAsset)
            {
                this.dgvGlaccAsset.Refresh();
                return;
            }
            if (this.tabControlGlacc.SelectedTab == this.tabLia)
            {
                this.dgvGlaccLia.Refresh();
                return;
            }
            if (this.tabControlGlacc.SelectedTab == this.tabCap)
            {
                this.dgvGlaccCap.Refresh();
                return;
            }
            if (this.tabControlGlacc.SelectedTab == this.tabRev)
            {
                this.dgvGlaccRev.Refresh();
                return;
            }
            if (this.tabControlGlacc.SelectedTab == this.tabExp)
            {
                this.dgvGlaccExp.Refresh();
                return;
            }
        }

        private void CheckCompleteFillTaxodesc()
        {
            if (this.list_glacc_item.Where(g => g.acctyp == "0" && g.taxonomy.Trim().Length == 0).Count<dataItem4Glacc>() == 0)
            {
                MessageBox.Show("    ท่าได้กำหนด Taxonomy ให้กับบัญชีย่อยครบทุกบัญชีแล้ว", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ClearInlineTaxodescSelector();

                this.btnExportCSG.Enabled = true;
                this.btnExportXML.Enabled = true;
                if (this.tabControlGlacc.SelectedTab == this.tabAsset)
                    this.dgvGlaccAsset.Focus();
                if (this.tabControlGlacc.SelectedTab == this.tabLia)
                    this.dgvGlaccLia.Focus();
                if (this.tabControlGlacc.SelectedTab == this.tabCap)
                    this.dgvGlaccCap.Focus();
                if (this.tabControlGlacc.SelectedTab == this.tabRev)
                    this.dgvGlaccRev.Focus();
                if (this.tabControlGlacc.SelectedTab == this.tabExp)
                    this.dgvGlaccExp.Focus();
            }
            else
            {
                this.btnExportCSG.Enabled = false;
                this.btnExportXML.Enabled = false;
            }
        }

        private void SwithAccnamLanguage(object sender, EventArgs e)
        {
            this.accnam_region = (this.rbAccnamThai.Checked ? ACCNAM_REGION.THA : ACCNAM_REGION.ENG);

            this.SetDgvGlaccVisualStyle(this.dgvGlaccAsset);
        }

        private void LoadGlacc(List<dataItem4Glacc> list_glacc_item = null, Sccomp selected_comp = null)
        {
            try
            {
                List<dataItem4Glacc> list_glacc = (list_glacc_item == null ? this.list_glacc_item : list_glacc_item);
                Sccomp comp = (selected_comp == null ? this.selected_comp : selected_comp);

                list_glacc.Clear();
                DataTable dt;
                if (comp.path.Contains(":\\"))
                {
                    dt = DBFParse.ReadDBF(comp.path + "/glacc.dbf");
                }
                else
                {
                    dt = DBFParse.ReadDBF(AppResource.PATH_PREFIX + comp.path + "/glacc.dbf");
                }

                this.tmp_list = new List<dataItem4Glacc>();
                foreach (DataRow row in dt.Rows)
                {
                    string space = "";
                    for (int space_fill = 1; space_fill <= ((int)row[3] - 1) * 4; space_fill++)
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
                this.tmp_list = this.tmp_list.OrderBy(t => t.accnum).OrderBy(t => t.level).OrderBy(t => t.group).ToList<dataItem4Glacc>();

                foreach (dataItem4Glacc item in this.tmp_list.Where(t => t.level == 1).ToList<dataItem4Glacc>().OrderBy(t => t.group).ToList<dataItem4Glacc>())
                {
                    this.RecursDataItem(item, list_glacc_item);
                }

                if (list_glacc.Count == 0)
                {
                    MessageBox.Show("ผังบัญขีจากข้อมูลที่ท่านเลือกไม่มีรายการอยู่", AppResource.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (IOException ex)
            {
                if (MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.Retry)
                {
                    this.LoadGlacc();
                }
                else
                {
                    return;
                }
            }
        }

        private void RecursDataItem(dataItem4Glacc item, List<dataItem4Glacc> list_glacc_item = null)
        {
            List<dataItem4Glacc> list_glacc = (list_glacc_item == null ? this.list_glacc_item : list_glacc_item);

            list_glacc.Add(item);
            
            foreach (dataItem4Glacc it in this.tmp_list.Where(t => t.parent == item.accnum).ToList<dataItem4Glacc>())
            {
                this.RecursDataItem(it, list_glacc);
            }
        }

        private void UpdateDBVer(string db_file_path)
        {
            if (File.Exists(db_file_path))
            {
                try
                {
                    string conn_str = "Data Source=" + db_file_path + ";Version=3;";
                    using (SQLiteConnection conn = new SQLiteConnection(conn_str, true))
                    {
                        conn.Open();

                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = @"PRAGMA user_version";
                            int user_version = Convert.ToInt32(cmd.ExecuteScalar());

                            //if (user_version < 2) // Upgrade db schema to version 2
                            //{
                            //    cmd.CommandText = @"PRAGMA user_version=2";
                            //    cmd.ExecuteNonQuery();

                            //    cmd.CommandText = @"ALTER TABLE glacc_match ADD COLUMN accnam TEXT";
                            //    cmd.ExecuteNonQuery();
                            //}

                            if (user_version < 1) // Upgrade db schema to version 1
                            {
                                cmd.CommandText = @"PRAGMA user_version=1";
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = @"ALTER TABLE glacc_match ADD COLUMN taxodesc2 TEXT";
                                cmd.ExecuteNonQuery();
                            }
                        }
                        conn.Close();
                    }
                }
                catch (SQLiteException ex)
                {
                    //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private List<MatchingData> GetMatchingData()
        {
            if (File.Exists(this.taxonomy_db_path + "/TAXONOMY.DB"))
            {
                File.Move(this.taxonomy_db_path + "/TAXONOMY.DB", this.taxonomy_db_path + "/TAXONOMY.RDB");
            }

            List<MatchingData> data = new List<MatchingData>();
            if (File.Exists(this.taxonomy_db_path + "/TAXONOMY.RDB"))
            {
                try
                {
                    string conn_str = "Data Source=" + this.taxonomy_db_path + "/TAXONOMY.RDB;Version=3;";
                    using (SQLiteConnection conn = new SQLiteConnection(conn_str, true))
                    {
                        conn.Open();

                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = @"Select * From glacc_match Order by accnum";
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    data.Add(new MatchingData()
                                    {
                                        id = reader.GetInt32(0),
                                        accnum = (reader.IsDBNull(1) ? "" : reader.GetString(1)),
                                        depcod = (reader.IsDBNull(2) ? "" : reader.GetString(2)),
                                        taxodesc1 = (reader.IsDBNull(3) ? "" : reader.GetString(3)),
                                        taxodesc2 = (reader.IsDBNull(4) ? "" : reader.GetString(4))
                                    });
                                }
                            }
                        }

                        using (SQLiteTransaction tr = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = tr;

                                // Get all data
                            }

                            tr.Commit();
                        }
                        conn.Close();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return data;
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
            dgv.Columns[2].Visible = (this.accnam_region == ACCNAM_REGION.THA ? true : false);
            dgv.Columns[2].AutoSizeMode = (dgv.Columns[2].Visible ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.None);
            dgv.Columns[2].HeaderText = "ชื่อบัญชี (ไทย)";
            dgv.Columns[3].Visible = (this.accnam_region == ACCNAM_REGION.ENG ? true : false);
            dgv.Columns[3].AutoSizeMode = (dgv.Columns[3].Visible ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.None);
            dgv.Columns[3].HeaderText = "ชื่อบัญชี (Eng)";
            dgv.Columns[12].Width = 400;
            dgv.Columns[12].HeaderText = "Taxonomy Description (ยอดคงเหลือปกติ)";
            dgv.Columns[13].Width = 400;
            dgv.Columns[13].HeaderText = "Taxonomy Description (ยอดคงเหลือติดลบ)";
            dgv.Columns[14].Visible = false;
        }

        private bool ValidateTaxonomy(string taxodesc, string acc_group = "*")
        {
            bool result = false;

            if (acc_group == "*")
            {
                if (this.taxonomy_list.Find(t => t.taxodesc.Trim() == taxodesc.Trim()) != null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else if (this.taxonomy_list.Find(t => t.taxodesc.Trim() == taxodesc.Trim() && t.group.Trim() == acc_group.Trim()) != null)
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

        private void SetFocusToNextItem()
        {
            if (this.selected_acc == null)
                return;

            this.SaveTaxoDescDB(this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum));

            dataItem4Glacc current_item = this.list_glacc_item.Find(t => t.accnum.Trim() == this.selected_acc.accnum.Trim());
            dataItem4Glacc next_item = this.GetNextPostItem(current_item);
            if (next_item != null) // At least 1 account next
            {
                if (next_item.group == "1") // Is assets
                {
                    this.tabControlGlacc.SelectedTab = this.tabAsset;
                    if (this.dgvGlaccAsset.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).Count<DataGridViewRow>() > 0)
                        this.dgvGlaccAsset.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).First<DataGridViewRow>().Cells[1].Selected = true;

                    return;
                }

                if (next_item.group == "2") // Is Liabilities
                {
                    this.tabControlGlacc.SelectedTab = this.tabLia;
                    if (this.dgvGlaccLia.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).Count<DataGridViewRow>() > 0)
                        this.dgvGlaccLia.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).First<DataGridViewRow>().Cells[1].Selected = true;
                    return;
                }

                if (next_item.group == "3") // Is Capitalize
                {
                    this.tabControlGlacc.SelectedTab = this.tabCap;
                    if (this.dgvGlaccCap.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).Count<DataGridViewRow>() > 0)
                        this.dgvGlaccCap.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).First<DataGridViewRow>().Cells[1].Selected = true;
                    return;
                }

                if (next_item.group == "4") // Is Revenue
                {
                    this.tabControlGlacc.SelectedTab = this.tabRev;
                    if (this.dgvGlaccRev.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).Count<DataGridViewRow>() > 0)
                        this.dgvGlaccRev.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).First<DataGridViewRow>().Cells[1].Selected = true;
                    return;
                }

                if (next_item.group == "5") // Is Expense
                {
                    this.tabControlGlacc.SelectedTab = this.tabExp;
                    if (this.dgvGlaccExp.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).Count<DataGridViewRow>() > 0)
                        this.dgvGlaccExp.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells[1].Value == next_item.accnum).First<DataGridViewRow>().Cells[1].Selected = true;
                    return;
                }

            }
            else // It's the last account
            {
                this.ClearInlineTaxodescSelector();
                return;
            }
        }

        private void SaveTaxoDescDB(dataItem4Glacc item)
        {
            item.taxonomy = (item.taxonomy == null ? "" : item.taxonomy);
            item.taxonomy2 = (item.taxonomy2 == null ? "" : item.taxonomy2);

            //if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\taxodesc_" + RewriteDataPath(this.selected_comp.path) + ".db"))
            //{
            //    SQLiteConnection.CreateFile(AppDomain.CurrentDomain.BaseDirectory + "Data\\taxodesc_" + RewriteDataPath(this.selected_comp.path) + ".db");
            //}
            if (!File.Exists(this.taxonomy_db_path + "/TAXONOMY.RDB"))
            {
                SQLiteConnection.CreateFile(this.taxonomy_db_path + "/TAXONOMY.RDB");
            }

            try
            {
                //string conn_str = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "Data\\taxodesc_" + RewriteDataPath(this.selected_comp.path) + ".db;Version=3;";
                string conn_str = "Data Source=" + this.taxonomy_db_path + "/TAXONOMY.RDB;Version=3;";
                using (SQLiteConnection conn = new SQLiteConnection(conn_str, true))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        // Set pragma user_version
                        cmd.CommandText = @"PRAGMA user_version=1";
                        cmd.ExecuteNonQuery();

                        // Create a new table if not exist
                        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS glacc_match(id INTEGER PRIMARY KEY, accnum TEXT, depcod TEXT, taxodesc TEXT, taxodesc2 TEXT)";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteTransaction tr = conn.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tr;

                            if (item.taxonomy.Trim().Length > 0 || item.taxonomy2.Trim().Length > 0)
                            {
                                cmd.CommandText = "INSERT OR REPLACE INTO glacc_match(id, accnum, depcod, taxodesc, taxodesc2) VALUES((Select id From glacc_match Where accnum='" + item.accnum.Trim() + "'), '" + item.accnum.Trim() + "', '', '" + item.taxonomy.Trim() + "','" + item.taxonomy2.Trim() + "')";
                                cmd.ExecuteNonQuery();
                            }
                            if (item.taxonomy.Trim().Length == 0 && item.taxonomy2.Trim().Length == 0)
                            {
                                cmd.CommandText = "DELETE From glacc_match Where accnum='"+ item.accnum.Trim() +"'";
                                cmd.ExecuteNonQuery();
                            }

                        }
                        tr.Commit();
                    }
                    conn.Close();
                }
            }
            catch (SQLiteException ex)
            {
                if (MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    this.SaveTaxoDescDB(item);
                }
            }
        }

        /**
         *  Remove colon,backslash OR getlast foldername of given path(depend on (bool)folder_name_only)
         */
        public static string RewriteDataPath(string original_path, bool folder_name_only = false)
        {
            if (folder_name_only == true)
            {
                int last_backslash = original_path.LastIndexOf("\\");
                if (original_path == string.Empty || last_backslash == -1)
                {
                    return original_path;
                }
                else
                {
                    return original_path.Substring(last_backslash + 1, original_path.Length - (last_backslash + 1));
                }
            }
            else
            {
                string path = original_path.Replace("\\", "{b}").Replace(":", "{c}");
                return path;
            }
        }

        private void btnExportXML_Click(object sender, EventArgs e)
        {
            XMLRequireDialog xd = new XMLRequireDialog(this, this.selected_comp);
            if (xd.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("ส่งออกข้อมูลงบทดลองเป็นไฟล์ .xml สำหรับเปิดด้วย Excel เสร็จสมบูรณ์แล้ว", AppResource.APP_NAME, MessageBoxButtons.OK);
            }
        }

        private void btnExportCSG_Click(object sender, EventArgs e)
        {
            LoadingScreen ls = new LoadingScreen();
            ls.ShowCenterParent(this);

            bool is_connected = false;
            //bool is_correct_license = false;
            SRV_RegisterData srv_regdata = null;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate
            {
                is_connected = Licensing.IsServerConnected();
                if (is_connected)
                {
                    //is_correct_license = Licensing.CheckLicense();
                    //srv_regdata = Licensing.GetSrvRegisterData(Licensing.GetLocalRegData());
                    srv_regdata = Licensing.GetSrvRegisterData(RegisterDialog.ReadSernum(), Licensing.GetHDDSerialNumber(Licensing.GetSystemDriveLetter()));
                }
            };
            worker.RunWorkerCompleted += delegate
            {
                ls.Close();

                if (!is_connected)
                {
                    MessageBox.Show("ไม่สามารถทำงานในส่วนนี้ได้ เนื่องจากไม่ได้เชื่อมต่ออินเทอร์เน็ต, กรุณาตรวจสอบการเชื่อมต่ออินเทอร์เน็ต", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if(srv_regdata == null)
                {
                    if (MessageBox.Show("เครื่องของท่าน ยังไม่ได้ลงทะเบียน,\nท่านต้องการลงทะเบียนโปรแกรมเพื่อเปิดใช้งานความสามารถในส่วนนี้ใช่หรือไม่?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.btnRegister.PerformClick();
                    }
                    return;
                }
                else
                {
                    if (srv_regdata.status_code == ((int)Licensing.STATUS.NOT_REGISTER).ToString())
                    {
                        if (MessageBox.Show("เครื่องของท่าน ยังไม่ได้ลงทะเบียน,\nท่านต้องการลงทะเบียนโปรแกรมเพื่อเปิดใช้งานความสามารถในส่วนนี้ใช่หรือไม่?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.btnRegister.PerformClick();
                        }
                        return;
                    }

                    if (srv_regdata.status_code == ((int)Licensing.STATUS.WAIT_FOR_VERIFY).ToString())
                    {
                        MessageBox.Show("โปรแกรมชุดนี้ " + srv_regdata.status + ", ท่านจะสามารถใช้งานในส่วนนี้ได้เมื่อผ่านการตรวจสอบแล้ว");
                        return;
                    }

                    if (srv_regdata.status_code == ((int)Licensing.STATUS.REGISTERED).ToString())
                    {
                        CSGRequireDialog cfd = new CSGRequireDialog(this, this.selected_comp);
                        if (cfd.ShowDialog() == DialogResult.OK)
                        {
                            MessageBox.Show("ส่งออกข้อมูลงบทดลองเป็นไฟล์ .csg สำหรับใช้งานกับ SmartBiz DBD Connect เสร็จสมบูรณ์แล้ว", AppResource.APP_NAME, MessageBoxButtons.OK);
                        }
                    }
                }
            };
            worker.RunWorkerAsync();
        }

        public Isprd LoadIsprd()
        {
            Isprd isprd;

            try
            {
                isprd = new Isprd();
                DataTable dt;
                if (this.selected_comp.path.Contains(":\\"))
                {
                    dt = DBFParse.ReadDBF(this.selected_comp.path + "/isprd.dbf");
                }
                else
                {
                    dt = DBFParse.ReadDBF(AppResource.PATH_PREFIX + this.selected_comp.path + "/isprd.dbf");
                }
                foreach (DataRow row in dt.Rows)
                {
                    isprd.beg1 = (DateTime)row[0];
                    isprd.end12 = (DateTime)row[34];
                    isprd.end12ny = (DateTime)row[70];
                }
            }
            catch (Exception ex)
            {
                isprd = null;

                if (MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1) == DialogResult.Retry)
                {
                    return this.LoadIsprd();
                }
            }

            return isprd;
        }

        public Isinfo LoadIsinfo()
        {
            Isinfo isinfo;

            try
            {
                isinfo = new Isinfo();
                DataTable dt;
                if (this.selected_comp.path.Contains(":\\"))
                {
                    dt = DBFParse.ReadDBF(this.selected_comp.path + "/isinfo.dbf");
                }
                else
                {
                    dt = DBFParse.ReadDBF(AppResource.PATH_PREFIX + this.selected_comp.path + "/isinfo.dbf");
                }
                foreach (DataRow row in dt.Rows)
                {
                    isinfo.thinam = ((string)row[0]).Trim(); //this.ReplaceInvalidSpace(((string)row[0]).Trim());
                    isinfo.taxid = ((string)row[8]).Trim();
                }
            }
            catch (Exception ex)
            {
                isinfo = null;

                if (MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    return this.LoadIsinfo();
                }
            }

            return isinfo;
        }

        public List<Gljnlit> LoadGljnlit()
        {
            List<Gljnlit> list_gljnlit = new List<Gljnlit>();

            try
            {
                DataTable dt;
                if (this.selected_comp.path.Contains(":\\"))
                {
                    dt = DBFParse.ReadDBF(this.selected_comp.path + "/gljnlit.dbf");
                }
                else
                {
                    dt = DBFParse.ReadDBF(AppResource.PATH_PREFIX + this.selected_comp.path + "/gljnlit.dbf");
                }

                foreach (DataRow row in dt.Rows)
                {
                    list_gljnlit.Add(new Gljnlit()
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
                if (MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1) == DialogResult.Retry)
                {
                    return this.LoadGljnlit();
                }
            }

            return list_gljnlit;
        }

        public List<Glbal> LoadGlbal()
        {
            List<Glbal> list_glbal = new List<Glbal>();

            try
            {
                DataTable dt;
                if (this.selected_comp.path.Contains(":\\"))
                {
                    dt = DBFParse.ReadDBF(this.selected_comp.path + "/glbal.dbf");
                }
                else
                {
                    dt = DBFParse.ReadDBF(AppResource.PATH_PREFIX + this.selected_comp.path + "/glbal.dbf");
                }

                foreach (DataRow row in dt.Rows)
                {
                    list_glbal.Add(new Glbal()
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
                if (MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1) == DialogResult.Retry)
                {
                    return this.LoadGlbal();
                }
            }

            return list_glbal;
        }

        /**
         * Get Beginning balance forward, Period movement debit, Period movement credit, Carry forward
         * */
        public AccAmountInfo GetAccAmountInfo(dataItem4Glacc acc, DateTime from_date, DateTime to_date, List<Glbal> list_glbal, List<Gljnlit> list_gljnlit)
        {
            List<Glbal> glbal = (list_glbal.Where(g => g.accnum.Trim() == acc.accnum.Trim()).Count<Glbal>() > 0 ? list_glbal.Where(g => g.accnum.Trim() == acc.accnum.Trim()).ToList<Glbal>() : new List<Glbal>());
            List<Gljnlit> gljnlit = (list_gljnlit.Where(g => g.accnum.Trim() == acc.accnum.Trim()).Count<Gljnlit>() > 0 ? list_gljnlit.Where(g => g.accnum.Trim() == acc.accnum.Trim()).ToList<Gljnlit>() : new List<Gljnlit>());
            double tmp_dr = 0d;
            double tmp_cr = 0d;

            AccAmountInfo info = new AccAmountInfo();
            info.bal_fwd = 0d;
            info.prd_dr = 0d;
            info.prd_cr = 0d;
            info.carr_fwd = 0d;

            // Calculate Balance Forward
            tmp_dr += Math.Round(gljnlit.Where(g => this.CompareDate(g.voudat, from_date) < 0 && g.trntyp.Trim() == "0").Sum(g => g.amount), 2); // Debit (trntyp = "0")
            tmp_dr += Math.Round(glbal.Sum(g => g.debit1ly), 2);
            tmp_dr += Math.Round(glbal.Sum(g => g.debit2ly), 2);
            tmp_dr += Math.Round(glbal.Sum(g => g.debit3ly), 2);
            tmp_dr += Math.Round(glbal.Sum(g => g.debit4ly), 2);
            tmp_dr += Math.Round(glbal.Sum(g => g.debit5ly), 2);
            tmp_dr += Math.Round(glbal.Sum(g => g.debit6ly), 2);
            tmp_dr += Math.Round(glbal.Sum(g => g.debit7ly), 2);
            tmp_dr += Math.Round(glbal.Sum(g => g.debit8ly), 2);
            tmp_dr += Math.Round(glbal.Sum(g => g.debit9ly), 2);
            tmp_dr += Math.Round(glbal.Sum(g => g.debit10ly), 2);
            tmp_dr += Math.Round(glbal.Sum(g => g.debit11ly), 2);
            tmp_dr += Math.Round(glbal.Sum(g => g.debit12ly), 2);

            tmp_cr += Math.Round(gljnlit.Where(g => this.CompareDate(g.voudat, from_date) < 0 && g.trntyp.Trim() == "1").Sum(g => g.amount), 2); // Credit (trntyp = "1")
            tmp_cr += Math.Round(glbal.Sum(g => g.credit1ly), 2);
            tmp_cr += Math.Round(glbal.Sum(g => g.credit2ly), 2);
            tmp_cr += Math.Round(glbal.Sum(g => g.credit3ly), 2);
            tmp_cr += Math.Round(glbal.Sum(g => g.credit4ly), 2);
            tmp_cr += Math.Round(glbal.Sum(g => g.credit5ly), 2);
            tmp_cr += Math.Round(glbal.Sum(g => g.credit6ly), 2);
            tmp_cr += Math.Round(glbal.Sum(g => g.credit7ly), 2);
            tmp_cr += Math.Round(glbal.Sum(g => g.credit8ly), 2);
            tmp_cr += Math.Round(glbal.Sum(g => g.credit9ly), 2);
            tmp_cr += Math.Round(glbal.Sum(g => g.credit10ly), 2);
            tmp_cr += Math.Round(glbal.Sum(g => g.credit11ly), 2);
            tmp_cr += Math.Round(glbal.Sum(g => g.credit12ly), 2);

            if (acc.nature == "0") // Nature Debit
            {
                info.bal_fwd = Math.Round(glbal.Sum(g => g.begly) + (tmp_dr - tmp_cr), 2);
            }
            else // Nature Credit
            {
                info.bal_fwd = Math.Round((glbal.Sum(g => g.begly) * -1) + (tmp_cr - tmp_dr), 2);
            }


            // Calculate Period Movement
            info.prd_dr = Math.Round(gljnlit.Where(g => this.CompareDate(g.voudat, from_date) >= 0 && this.CompareDate(g.voudat, to_date) <= 0 && g.trntyp == "0").Sum(g => g.amount), 2); // Debit (trntyp = "0")
            info.prd_cr = Math.Round(gljnlit.Where(g => this.CompareDate(g.voudat, from_date) >= 0 && this.CompareDate(g.voudat, to_date) <= 0 && g.trntyp == "1").Sum(g => g.amount), 2); // Credit (trntyp = "1")


            // Calculate Carry Forward
            if (acc.nature == "0") // Nature Debit
            {
                info.carr_fwd = Math.Round(info.bal_fwd + (info.prd_dr - info.prd_cr), 2);
            }
            else // Nature Credit
            {
                info.carr_fwd = Math.Round(info.bal_fwd + (info.prd_cr - info.prd_dr), 2);
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

        private string ReplaceInvalidSpace(string strIn)
        {
            // Replace invalid space char with normal white space
            try
            {
                return Regex.Replace(strIn, @"[^\w\.@-]", " ", RegexOptions.None);
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        private void btnOpenData_Click(object sender, EventArgs e)
        {
            SelectComp sc = new SelectComp(this);
            if (sc.ShowDialog() == DialogResult.OK)
            {
                this.selected_comp = sc.selected_comp;
                this.btnCopy.Enabled = true;
                this.rbAccnamEng.Enabled = true;
                this.rbAccnamThai.Enabled = true;

                if (this.selected_comp.path.Contains(":\\"))
                {
                    this.taxonomy_db_path = this.selected_comp.path;
                }
                else
                {
                    this.taxonomy_db_path = AppResource.PATH_PREFIX + this.selected_comp.path;
                }

                this.LoadDataOfSelectedCompany();
            }
            else
            {
                this.taxonomy_db_path = string.Empty;
                this.btnCopy.Enabled = (this.selected_comp != null ? true : false);
                this.rbAccnamEng.Enabled = (this.selected_comp != null ? true : false);
                this.rbAccnamThai.Enabled = (this.selected_comp != null ? true : false);
            }
        }

        private void LoadDataOfSelectedCompany()
        {
            this.LoadGlacc();
            this.isinfo = this.LoadIsinfo();

            if (this.list_glacc_item.Count == 0)
                return;

            this.UpdateDBVer(this.taxonomy_db_path + "/TAXONOMY.RDB");
            List<MatchingData> matching = this.GetMatchingData();
            List<MatchingData> valid_matching = this.RemoveInvalidTaxonomyFromDB(matching);

            foreach (MatchingData d in valid_matching)
            {
                if (this.list_glacc_item.Find(g => g.accnum == d.accnum) != null)
                {
                    this.list_glacc_item.Find(g => g.accnum == d.accnum).taxonomy = d.taxodesc1;
                    this.list_glacc_item.Find(g => g.accnum == d.accnum).taxonomy2 = d.taxodesc2;
                }
            }

            this.FillForm();
            this.CheckCompleteFillTaxodesc();
        }

        private List<MatchingData> RemoveInvalidTaxonomyFromDB(List<MatchingData> matching_data, bool validate_both_accnum_accnam = false, List<dataItem4Glacc> pattern_glacc = null)
        {
            List<MatchingData> valid_matching = new List<MatchingData>();

            if (File.Exists(this.taxonomy_db_path + "/TAXONOMY.RDB"))
            {
                try
                {
                    string conn_str = "Data Source=" + this.taxonomy_db_path + "/TAXONOMY.RDB;Version=3;";
                    using (SQLiteConnection conn = new SQLiteConnection(conn_str, true))
                    {
                        conn.Open();

                        using (SQLiteTransaction tr = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = tr;
                                cmd.CommandText = string.Empty;

                                foreach (MatchingData m in matching_data)
                                {
                                    if (validate_both_accnum_accnam == true) // Perform remove item that not match (validate by accnum + accnam)
                                    {
                                        if (this.list_glacc_item.Find(g => g.accnum.Trim() == m.accnum.Trim() && g.acctyp == "0") != null) // check accnum
                                        {
                                            string accnum = this.list_glacc_item.Find(g => g.accnum.Trim() == m.accnum.Trim() && g.acctyp == "0").accnum.Trim();
                                            string accnam = this.list_glacc_item.Find(g => g.accnum.Trim() == m.accnum.Trim() && g.acctyp == "0").accnam.Trim();
                                            string accnam2 = this.list_glacc_item.Find(g => g.accnum.Trim() == m.accnum.Trim() && g.acctyp == "0").accnam2.Trim();

                                            if (this.accnam_region == ACCNAM_REGION.THA) // if accnam display in THA.
                                            {
                                                if (pattern_glacc.Find(g => g.accnum.Trim() == accnum && g.accnam.Trim() == accnam) != null)
                                                {
                                                    valid_matching.Add(m);
                                                    continue;
                                                }
                                                else
                                                {
                                                    cmd.CommandText += "DELETE From glacc_match Where accnum='" + m.accnum.Trim() + "';";
                                                }
                                            }
                                            else // if accnam display in ENG.
                                            {
                                                if (pattern_glacc.Find(g => g.accnum.Trim() == accnum && g.accnam2.Trim() == accnam2) != null)
                                                {
                                                    valid_matching.Add(m);
                                                    continue;
                                                }
                                                else
                                                {
                                                    cmd.CommandText += "DELETE From glacc_match Where accnum='" + m.accnum.Trim() + "';";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            cmd.CommandText += "DELETE From glacc_match Where accnum='" + m.accnum.Trim() + "';";
                                        }
                                    }
                                    else // Perform remove item that not match (validate by accnum only)
                                    {
                                        if (this.list_glacc_item.Find(g => g.accnum.Trim() == m.accnum.Trim() && g.acctyp == "0") != null)
                                        {
                                            valid_matching.Add(m);
                                            continue;
                                        }
                                        else
                                        {
                                            cmd.CommandText += "DELETE From glacc_match Where accnum='" + m.accnum.Trim() + "';";
                                        }
                                    }
                                }
                                cmd.ExecuteNonQuery();
                            }
                            tr.Commit();
                        }
                        conn.Close();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return valid_matching;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
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
                            return true;
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

                if (this.btn_taxonomy2 != null && this.btn_taxonomy2._Focused)
                {
                    if (this.btn_taxonomy2._Text.Trim().Length > 0)
                    {
                        if (this.ValidateTaxonomy(this.btn_taxonomy2._Text.Trim()) == true)
                        {
                            this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum).taxonomy2 = this.btn_taxonomy2._Text.Trim();
                            this.SaveTaxoDescDB(this.list_glacc_item.Find(g => g.accnum.Trim() == this.selected_acc.accnum.Trim()));
                            this.ClearInlineTaxodescSelector(true, true);
                        }
                        else
                        {
                            this.btn_taxonomy2.btnBrowse.PerformClick();
                            return true;
                        }
                    }
                    else
                    {
                        this.list_glacc_item.Find(g => g.accnum == this.selected_acc.accnum).taxonomy2 = "";
                        this.SaveTaxoDescDB(this.list_glacc_item.Find(g => g.accnum.Trim() == this.selected_acc.accnum.Trim()));
                        this.ClearInlineTaxodescSelector(true, true);
                    }

                    return true;
                }
            }

            if (keyData == Keys.Escape)
            {
                if (this.cb_taxonomy != null || this.btn_taxonomy2 != null)
                {
                    this.ClearInlineTaxodescSelector();

                    if (this.tabControlGlacc.SelectedTab == this.tabAsset)
                        this.dgvGlaccAsset.Focus();
                    if (this.tabControlGlacc.SelectedTab == this.tabLia)
                        this.dgvGlaccLia.Focus();
                    if (this.tabControlGlacc.SelectedTab == this.tabCap)
                        this.dgvGlaccCap.Focus();
                    if (this.tabControlGlacc.SelectedTab == this.tabRev)
                        this.dgvGlaccRev.Focus();
                    if (this.tabControlGlacc.SelectedTab == this.tabExp)
                        this.dgvGlaccExp.Focus();

                    return true;
                }
            }

            if (keyData == Keys.Tab)
            {
                if ((this.cb_taxonomy != null && this.cb_taxonomy._Focused) || (this.btn_taxonomy2 != null && this.btn_taxonomy2._Focused))
                {
                    if (this.list_glacc_item.Find(t => t.accnum.Trim() == this.selected_acc.accnum.Trim()) != null)
                    {
                        this.SetFocusToNextItem();
                    }

                    return true;
                }
            }

            if (keyData == Keys.F3)
            {
                this.btnOpenData.PerformClick();
                return true;
            }

            if (keyData == Keys.F4)
            {
                this.btnExportXML.PerformClick();
                return true;
            }

            if (keyData == Keys.F5)
            {
                this.btnExportCSG.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (RegisterDialog.ReadSernum().Trim().Length == 0)
            {
                MessageBox.Show("ไม่สามารถลงทะเบียนโปรแกรมได้, สาเหตุอาจเนื่องมาจากท่านติดตั้งโปรแกรมผิดที่\n(โปรแกรม Express e-Filing จะต้องติดตั้งไว้ภายใต้โฟลเดอร์ที่เก็บโปรแกรมเอ็กซ์เพรส),\nหรืออาจเกิดจากท่านยังไม่ได้ทำการลงทะเบียนโปรแกรมเอ็กซ์เพรส");
                return;
            }

            LoadingScreen ls = new LoadingScreen();
            ls.ShowCenterParent(this);

            Licensing.CreateTokenKeyDB();

            if (Licensing.GetHDDSerialNumber(Licensing.GetSystemDriveLetter()).Trim().Length == 0)
            {
                ls.Close();
                MessageBox.Show("เครื่องของท่านไม่สามารถลงทะเบียนโปรแกรมได้ เนื่องจากไม่สามารถอ่านค่าฮาร์ดแวร์ได้", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            bool isServerConnected = false;

            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += delegate
                {
                    isServerConnected = Licensing.IsServerConnected();
                };
                worker.RunWorkerCompleted += delegate
                {
                    ls.Close();
                    if (!isServerConnected)
                    {
                        MessageBox.Show("ไม่สามารถติดต่อกับเซิร์ฟเวอร์เพื่อทำการลงทะเบียนได้, กรุณาตรวจสอบการเชื่อมต่ออินเทอร์เน็ต", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        this.ShowRegistForm();
                    }
                };
                worker.RunWorkerAsync();
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            SelectComp sel_comp = new SelectComp(this);
            List<dataItem4Sccomp> list_sccomp_item = sel_comp.LoadSccomp();
            List<dataItem4Sccomp> list_comp_to_copy = new List<dataItem4Sccomp>();

            foreach (dataItem4Sccomp item in list_sccomp_item)
            {
                if (item.path.RemoveBracketFromPath() == this.selected_comp.path.RemoveBracketFromPath())
                    continue;

                string taxo_path = string.Empty;
                if (item.path.Contains(":\\"))
                {
                    taxo_path = item.path.RemoveBracketFromPath();
                }
                else
                {
                    taxo_path = AppResource.PATH_PREFIX + item.path.RemoveBracketFromPath();
                }

                if (File.Exists(taxo_path + "/TAXONOMY.RDB"))
                {
                    list_comp_to_copy.Add(list_sccomp_item.Where(i => i.path == item.path).First<dataItem4Sccomp>());
                }
            }

            SelectComp comp = new SelectComp(this, list_comp_to_copy);
            comp.Text = "เลือกที่เก็บข้อมูลที่ต้องการคัดลอก";
            
            if (comp.ShowDialog() == DialogResult.OK)
            {
                CopyOptionsDialog od = new CopyOptionsDialog();
                if (od.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("คัดลอกการจับคู่ Taxonomy จากที่เก็บข้อมูล \"" + comp.selected_comp.path + "\" มาใส่ในที่เก็บข้อมูล \"" + this.selected_comp.path + "\"\nยืนยันการทำงานนี้?", AppResource.APP_NAME, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        string source_file = string.Empty;
                        if (comp.selected_comp.path.Contains(":\\"))
                        {
                            source_file = comp.selected_comp.path.RemoveBracketFromPath() + "/TAXONOMY.RDB";
                        }
                        else
                        {
                            source_file = AppResource.PATH_PREFIX + comp.selected_comp.path.RemoveBracketFromPath() + "/TAXONOMY.RDB";
                        }

                        string destination_file = string.Empty;
                        if (this.selected_comp.path.Contains(":\\"))
                        {
                            destination_file = this.selected_comp.path.RemoveBracketFromPath() + "/TAXONOMY.RDB";
                        }
                        else
                        {
                            destination_file = AppResource.PATH_PREFIX + this.selected_comp.path.RemoveBracketFromPath() + "/TAXONOMY.RDB";
                        }

                        try
                        {
                            File.Copy(source_file, destination_file, true);
                            List<MatchingData> matching = this.GetMatchingData();
                            List<dataItem4Glacc> pattern_glacc = new List<dataItem4Glacc>();
                            this.LoadGlacc(pattern_glacc, comp.selected_comp);
                            this.RemoveInvalidTaxonomyFromDB(matching, od.is_match_both, pattern_glacc);
                            this.LoadDataOfSelectedCompany();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                        }
                    }

                    list_comp_to_copy = null;
                }
            }
            list_sccomp_item = null;
        }

        private void ShowRegistForm()
        {
            LoadingScreen ls = new LoadingScreen();
            ls.ShowCenterParent(this);

            using (BackgroundWorker worker = new BackgroundWorker())
            {
                SRV_RegisterData srv_regdata = null;
                worker.DoWork += delegate
                {
                    srv_regdata = Licensing.GetSrvRegisterData(RegisterDialog.ReadSernum(), Licensing.GetHDDSerialNumber(Licensing.GetSystemDriveLetter()));
                };
                worker.RunWorkerCompleted += delegate
                {
                    ls.Close();
                    RegisterDialog rg = new RegisterDialog();

                    if (srv_regdata == null) // not register yet
                    {
                        rg.register_status = Licensing.STATUS.NOT_REGISTER;
                    }
                    else
                    {
                        if ((Licensing.IsLAN() && srv_regdata.reg_type == "LAN") || (!Licensing.IsLAN() && srv_regdata.reg_type == "LOCAL")) // Validate reg_type
                        {
                            if (srv_regdata.status_code == ((int)Licensing.STATUS.NOT_REGISTER).ToString())
                                rg.register_status = Licensing.STATUS.NOT_REGISTER;

                            if (srv_regdata.status_code == ((int)Licensing.STATUS.WAIT_FOR_VERIFY).ToString())
                                rg.register_status = Licensing.STATUS.WAIT_FOR_VERIFY;

                            if (srv_regdata.status_code == ((int)Licensing.STATUS.REGISTERED).ToString())
                                rg.register_status = Licensing.STATUS.REGISTERED;
                        }
                        else // reg_type is incorrect
                        {
                            rg.register_status = Licensing.STATUS.NOT_REGISTER;
                        }
                    }

                    if (rg.ShowDialog() == DialogResult.OK)
                    {
                        this.local_reg = Licensing.GetLocalRegData();
                    }
                };
                worker.RunWorkerAsync();
            }

            //using (BackgroundWorker worker = new BackgroundWorker())
            //{
            //    SRV_RegisterData srv_regdata = null;

            //    worker.DoWork += delegate
            //    {
            //        srv_regdata = Licensing.GetSrvRegisterData(Licensing.GetLocalRegData());
            //        System.Threading.Thread.Sleep(500);
            //    };
            //    worker.RunWorkerCompleted += delegate
            //    {
            //        ls.Close();

            //        RegisterDialog rg = new RegisterDialog();

            //        if (srv_regdata == null) // not register yet
            //        {
            //            rg.register_status = Licensing.STATUS.NOT_REGISTER;
            //        }
            //        else
            //        {
            //            if ((Licensing.IsLAN() && srv_regdata.reg_type == "LAN") || (!Licensing.IsLAN() && srv_regdata.reg_type == "LOCAL")) // Validate reg_type
            //            {
            //                if (srv_regdata.status_code == ((int)Licensing.STATUS.NOT_REGISTER).ToString())
            //                    rg.register_status = Licensing.STATUS.NOT_REGISTER;

            //                if (srv_regdata.status_code == ((int)Licensing.STATUS.WAIT_FOR_VERIFY).ToString())
            //                    rg.register_status = Licensing.STATUS.WAIT_FOR_VERIFY;

            //                if (srv_regdata.status_code == ((int)Licensing.STATUS.REGISTERED).ToString())
            //                    rg.register_status = Licensing.STATUS.REGISTERED;
            //            }
            //            else // reg_type is incorrect
            //            {
            //                rg.register_status = Licensing.STATUS.NOT_REGISTER;
            //            }
            //        }
                        
            //        if (rg.ShowDialog() == DialogResult.OK)
            //        {
            //            this.local_reg = Licensing.GetLocalRegData();
            //        }
            //    };
            //    worker.RunWorkerAsync();
            //}
        }
    }
}
