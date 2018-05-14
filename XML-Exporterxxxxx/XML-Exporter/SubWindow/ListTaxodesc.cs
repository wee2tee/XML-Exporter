using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XML_Exporter.DataModel;
using XML_Exporter.MiscClass;
using MetroFramework.Forms;

namespace XML_Exporter.SubWindow
{
    public partial class ListTaxodesc : MetroForm
    {
        public Glacc current_acc;
        public Taxonomy current_taxonomy;
        private List<Taxonomy> list_taxonomy;

        private BindingSource bs;

        public ListTaxodesc(MainForm parent_form, Glacc current_acc, List<Taxonomy> list_taxonomy, Taxonomy current_taxonomy = null)
        {
            InitializeComponent();
            this.current_acc = current_acc;
            this.current_taxonomy = current_taxonomy;
            this.list_taxonomy = list_taxonomy;
        }

        private void ListTaxodesc_Load(object sender, EventArgs e)
        {
            this.BindingControlEventHandler();

            this.lblAccnam.Text = this.current_acc.accnum.Trim() + " " + this.current_acc.accnam.Trim();

            this.bs = new BindingSource();
            this.bs.DataSource = this.list_taxonomy;
            this.bs.ResetBindings(false);
            this.dgvTaxonomy.DataSource = this.bs;

            this.SetDgvVisualStyle();
        }

        private void ListTaxodesc_Shown(object sender, EventArgs e)
        {
            if (this.current_taxonomy != null)
            {
                if (this.dgvTaxonomy.Rows.Cast<DataGridViewRow>().Where(r => ((string)r.Cells[3].Value).Trim() == this.current_taxonomy.taxodesc.Trim()).Count<DataGridViewRow>() > 0)
                {
                    this.dgvTaxonomy.Rows.Cast<DataGridViewRow>().Where(r => ((string)r.Cells[3].Value).Trim() == this.current_taxonomy.taxodesc.Trim()).First<DataGridViewRow>().Cells[2].Selected = true;
                }
            }
        }

        private void BindingControlEventHandler()
        {
            this.dgvTaxonomy.CurrentCellChanged += delegate
            {
                if (this.dgvTaxonomy.CurrentCell == null)
                {
                    this.btnOK.Enabled = false;
                }
                else
                {
                    this.btnOK.Enabled = true;
                }
            };

            this.dgvTaxonomy.MouseDoubleClick += delegate(object sender, MouseEventArgs e)
            {
                int row_index = this.dgvTaxonomy.HitTest(e.X, e.Y).RowIndex;

                if (row_index > -1)
                {
                    this.btnOK.PerformClick();
                }
            };
        }

        private void SetDgvVisualStyle()
        {
            this.dgvTaxonomy.Columns[0].Visible = false;
            this.dgvTaxonomy.Columns[1].Visible = false;
            this.dgvTaxonomy.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvTaxonomy.Columns[3].Width = 350;

            this.dgvTaxonomy.Columns[2].HeaderText = "Name";
            this.dgvTaxonomy.Columns[3].HeaderText = "Taxonomy Description";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.current_taxonomy = this.list_taxonomy.Find(t => t.taxodesc == (string)this.dgvTaxonomy.Rows[this.dgvTaxonomy.CurrentCell.RowIndex].Cells[3].Value);
            this.DialogResult = DialogResult.OK;
            this.Close();
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
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
