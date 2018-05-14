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
    public partial class CopyOptionsDialog : Form
    {
        public bool is_match_both = false;

        public CopyOptionsDialog()
        {
            InitializeComponent();
        }

        private void CopyOptionsDialog_Load(object sender, EventArgs e)
        {
            this.BindingEventHandler();
        }

        private void BindingEventHandler()
        {
            this.chkBoth.CheckedChanged += delegate
            {
                this.is_match_both = (this.chkBoth.CheckState == CheckState.Checked ? true : false);
            };
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (this.btnOK.Focused || this.btnCancel.Focused)
                    return false;

                SendKeys.Send("{TAB}");
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
