using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomControl
{
    public partial class CustomButtonLabel : UserControl
    {
        private string _text = string.Empty;
        public string _Text
        {
            get
            {
                return this._text;
                //return this.btnBrowse.Text;
            }
            set
            {
                this._text = value;
                this.Refresh();
                //this.btnBrowse.Text = value;
            }
        }

        private Color _foreColor = Color.Black;
        public Color _ForeColor
        {
            get
            {
                return this._foreColor;
            }
            set
            {
                this._foreColor = value;
            }
        }

        public CustomButtonLabel()
        {
            InitializeComponent();
        }

        private void CustomButtonLabel_Load(object sender, EventArgs e)
        {
            this.BindingControlEventHandler();
        }

        private void BindingControlEventHandler()
        {
            //this.Paint += delegate(object sender, PaintEventArgs e)
            //{
            //    using (SolidBrush brush = new SolidBrush(Color.Black))
            //    {
            //        e.Graphics.DrawString(this._text, this.Font, brush, new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 20, e.ClipRectangle.Height), new StringFormat(){ Alignment = StringAlignment.Near, FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit});
            //    }
            //};

            this.btnBrowse.Paint += delegate(object sender, PaintEventArgs e)
            {
                using (SolidBrush brush = new SolidBrush(this._foreColor))
                {
                    e.Graphics.DrawString(this._text, this.Font, brush, new Rectangle(e.ClipRectangle.X + 3, e.ClipRectangle.Y + 3, e.ClipRectangle.Width - 20, e.ClipRectangle.Height), new StringFormat() { Alignment = StringAlignment.Near, FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit });
                }
            };
        }
    }
}
