using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using XML_Exporter.DataModel;
using XML_Exporter.MiscClass;
using DBFHelper;

namespace XML_Exporter.SubWindow
{
    public partial class SelectComp : Form
    {
        private MainForm main_form;
        public Sccomp selected_comp;
        private List<dataItem4Sccomp> list_sccomp_item;
        private BindingSource bs_sccomp;
        private SORT sort_item = SORT.COMPNAM;
        private enum SORT : int
        {
            COMPNAM = 1,
            COMPCOD = 2
        }

        public SelectComp(MainForm main_form)
        {
            InitializeComponent();
            this.main_form = main_form;
        }

        public SelectComp(MainForm main_form, List<dataItem4Sccomp> list_sccomp_item)
        {
            InitializeComponent();
            this.main_form = main_form;
            this.list_sccomp_item = list_sccomp_item;
        }

        private void SelectComp_Load(object sender, EventArgs e)
        {
            this.BindingControlEventHandler();
            this.list_sccomp_item = (this.list_sccomp_item == null ? this.LoadSccomp() : this.list_sccomp_item);

            this.bs_sccomp = new BindingSource();
            this.bs_sccomp.DataSource = this.list_sccomp_item;
            this.dgvComp.DataSource = this.bs_sccomp;
            this.bs_sccomp.ResetBindings(true);
            this.SetDgvSccompVisualStyle();
        }

        private void BindingControlEventHandler()
        {
            this.dgvComp.CurrentCellChanged += delegate(object sender, EventArgs e)
            {
                if (this.dgvComp.CurrentCell != null)
                {
                    this.selected_comp = (Sccomp)this.dgvComp.Rows[this.dgvComp.CurrentCell.RowIndex].Cells[0].Value;
                }
            };

            this.dgvComp.MouseDoubleClick += delegate(object sender, MouseEventArgs e)
            {
                int row_index = this.dgvComp.HitTest(e.X, e.Y).RowIndex;
                if (row_index > -1)
                {
                    this.btnOK.PerformClick();
                }
            };

            this.dgvComp.CellClick += delegate(object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex == -1)
                {
                    if (e.ColumnIndex == 1 && this.sort_item == SORT.COMPCOD)
                    {
                        this.btnSort.PerformClick();
                        return;
                    }

                    if (e.ColumnIndex == 2 && this.sort_item == SORT.COMPNAM)
                    {
                        this.btnSort.PerformClick();
                        return;
                    }
                }
            };

            this.dgvComp.Paint += new PaintEventHandler(main_form.DrawRowBorder);
        }

        public List<dataItem4Sccomp> LoadSccomp()
        {
            List<dataItem4Sccomp> tmp_list = new List<dataItem4Sccomp>();
            try
            {
                DataTable dt = DBFParse.ReadDBF(AppResource.PATH_PREFIX + "secure/sccomp.dbf");
                
                foreach (DataRow row in dt.Rows)
                {
                    tmp_list.Add(new dataItem4Sccomp()
                    {
                        sccomp = new Sccomp()
                        {
                            compnam = (string)row[0],
                            compcod = ((string)row[1]).Trim(),
                            path = ((string)row[2]).Trim(),
                            gendat = (row[3] is DBNull ? DateTime.Now : (DateTime)row[3]),
                            candel = ((string)row[4]).Trim()
                        },
                        compnam = (string)row[0],
                        compcod = ((string)row[1]).Trim(),
                        path = ((string)row[2]).Trim(),
                    });
                }


            }
            catch (IOException ex)
            {
                if (MessageBox.Show(ex.Message, AppResource.APP_NAME, MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1) == DialogResult.Retry)
                {
                    this.LoadSccomp();
                }
                else
                {
                    this.Close();
                }
            }
            return (this.sort_item == SORT.COMPNAM ? tmp_list.OrderBy(s => s.compnam).ToList<dataItem4Sccomp>() : tmp_list.OrderBy(s => s.compcod).ToList<dataItem4Sccomp>());
        }

        private void SetDgvSccompVisualStyle()
        {
            if (this.dgvComp.Columns.Count > 0)
            {
                this.dgvComp.Columns[0].Visible = false;
                this.dgvComp.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dgvComp.Columns[2].Width = 120;
                this.dgvComp.Columns[3].Width = 400;
                this.dgvComp.Columns[1].HeaderText = "ชื่อข้อมูล";
                this.dgvComp.Columns[2].HeaderText = "รหัส";
                this.dgvComp.Columns[3].HeaderText = "ที่เก็บข้อมูล";
            }
        }

        //public static string RemoveBracketFromPath(string comp_path)
        //{
        //    string path = comp_path;

        //    //if (comp_path.Contains("("))
        //    //{
        //    //    path = comp_path.Substring(0, comp_path.IndexOf("("));
        //    //}

        //    int last_backslash = comp_path.LastIndexOf("\\");
        //    //Console.WriteLine(" >>> last_backslash : " + last_backslash);

        //    int last_open_bracket = comp_path.LastIndexOf("(");
        //    //Console.WriteLine(" >>> last_open_bracket : " + last_open_bracket);

        //    if (last_open_bracket > -1 && last_open_bracket > last_backslash)
        //    {
        //        path = comp_path.Substring(0, comp_path.Substring(last_open_bracket).Length);
        //    }

        //    return path.Trim();
        //}

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.dgvComp.CurrentCell != null)
            {
                if (((Sccomp)this.dgvComp.Rows[this.dgvComp.CurrentCell.RowIndex].Cells[0].Value).path.Contains(":\\"))
                {
                    if (Directory.Exists(((Sccomp)this.dgvComp.Rows[this.dgvComp.CurrentCell.RowIndex].Cells[0].Value).path))
                    {
                        this.selected_comp.path = ((Sccomp)this.dgvComp.Rows[this.dgvComp.CurrentCell.RowIndex].Cells[0].Value).path.RemoveBracketFromPath();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("ที่เก็บข้อมูลที่ท่านเลือก ไม่มีข้อมูลอยู่", AppResource.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    if (Directory.Exists(AppResource.PATH_PREFIX + ((Sccomp)this.dgvComp.Rows[this.dgvComp.CurrentCell.RowIndex].Cells[0].Value).path.RemoveBracketFromPath()))
                    {
                        this.selected_comp.path = ((Sccomp)this.dgvComp.Rows[this.dgvComp.CurrentCell.RowIndex].Cells[0].Value).path.RemoveBracketFromPath();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("ที่เก็บข้อมูลที่ท่านเลือก ไม่มีข้อมูลอยู่", AppResource.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchBox sb = new SearchBox();

            Point dgv_location = this.dgvComp.PointToScreen(Point.Empty);
            sb.Location = new Point(dgv_location.X, dgv_location.Y + this.dgvComp.Bounds.Height);

            if (sb.ShowDialog() == DialogResult.OK)
            {
                if (this.dgvComp.Rows.Cast<DataGridViewRow>().Where(r => ((string)r.Cells[1].Value).Trim().Length >= sb._Text.Trim().Length && ((string)r.Cells[1].Value).Trim().Substring(0, sb._Text.Trim().Length) == sb._Text.Trim()).Count<DataGridViewRow>() > 0)
                {
                    this.dgvComp.Rows.Cast<DataGridViewRow>().Where(r => ((string)r.Cells[1].Value).Trim().Length >= sb._Text.Trim().Length && ((string)r.Cells[1].Value).Trim().Substring(0, sb._Text.Trim().Length) == sb._Text.Trim()).First<DataGridViewRow>().Cells[1].Selected = true;
                }
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            List<dataItem4Sccomp> tmp_list;
            if (this.sort_item == SORT.COMPNAM)
            {
                this.sort_item = SORT.COMPCOD;
                tmp_list = this.list_sccomp_item.OrderBy(s => s.compcod).ToList<dataItem4Sccomp>();
                this.bs_sccomp.DataSource = tmp_list;
                this.bs_sccomp.ResetBindings(true);
                return;
            }

            if (this.sort_item == SORT.COMPCOD)
            {
                this.sort_item = SORT.COMPNAM;
                tmp_list = this.list_sccomp_item.OrderBy(s => s.compnam).ToList<dataItem4Sccomp>();
                this.bs_sccomp.DataSource = tmp_list;
                this.bs_sccomp.ResetBindings(true);
                return;
            }
        }

        private void SelectComp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString().Length > 0)
            {
                SearchBox sb = new SearchBox()
                {
                    _Text = e.KeyChar.ToString()
                };

                Point dgv_location = this.dgvComp.PointToScreen(Point.Empty);
                sb.Location = new Point(dgv_location.X, dgv_location.Y + this.dgvComp.Bounds.Height);

                if (sb.ShowDialog() == DialogResult.OK)
                {
                    if (this.dgvComp.Rows.Cast<DataGridViewRow>().Where(r => ((string)r.Cells[1].Value).Trim().Length >= sb._Text.Trim().Length && ((string)r.Cells[1].Value).Trim().Substring(0, sb._Text.Trim().Length) == sb._Text.Trim()).Count<DataGridViewRow>() > 0)
                    {
                        this.dgvComp.Rows.Cast<DataGridViewRow>().Where(r => ((string)r.Cells[1].Value).Trim().Length >= sb._Text.Trim().Length && ((string)r.Cells[1].Value).Trim().Substring(0, sb._Text.Trim().Length) == sb._Text.Trim()).First<DataGridViewRow>().Cells[1].Selected = true;
                    }
                }
            }
        }

        private void dgvComp_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                e.PaintBackground(e.CellBounds, true);
                e.PaintContent(e.CellBounds);
                using (SolidBrush brush = new SolidBrush(Color.RosyBrown))
                {
                    if (e.ColumnIndex == 1 && this.sort_item == SORT.COMPNAM)
                    {
                        e.Graphics.FillPolygon(brush, new Point[] { new Point(e.CellBounds.Right - 12, e.CellBounds.Top + 10), new Point(e.CellBounds.Right - 16, e.CellBounds.Top + 18), new Point(e.CellBounds.Right - 8, e.CellBounds.Top + 18) });
                        e.CellStyle.BackColor = Color.Purple;
                    }

                    if (e.ColumnIndex == 2 && this.sort_item == SORT.COMPCOD)
                    {
                        e.Graphics.FillPolygon(brush, new Point[] { new Point(e.CellBounds.Right - 12, e.CellBounds.Top + 10), new Point(e.CellBounds.Right - 16, e.CellBounds.Top + 18), new Point(e.CellBounds.Right - 8, e.CellBounds.Top + 18) });
                        e.CellStyle.BackColor = Color.Purple;
                    }
                }
                e.Handled = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                this.btnOK.PerformClick();
                return true;
            }

            if (keyData == Keys.Escape)
            {
                this.btnCancel.PerformClick();
                return true;
            }

            if (keyData == Keys.Tab)
            {
                this.btnSort.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
