using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XML_Exporter.SubWindow
{
    public partial class SearchBox : Form
    {
        public string _Text
        {
            set
            {
                this.txtKeyword.Text = value;
            }
            get
            {
                return this.txtKeyword.Text;
            }
        }

        public SearchBox()
        {
            InitializeComponent();
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

        private void SearchBox_Shown(object sender, EventArgs e)
        {
            this.txtKeyword.SelectionStart = this.txtKeyword.Text.Length;
        }
    }
}
