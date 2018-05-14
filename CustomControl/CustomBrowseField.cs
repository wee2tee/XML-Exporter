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
    public partial class CustomBrowseField: UserControl
    {
        private bool _focused = false;
        public bool _Focused
        {
            get
            {
                return this._focused;
            }
        }

        private Color _backgroundColor = Color.White;
        public Color _BackgroundColor
        {
            get
            {
                return this._backgroundColor;
            }
            set
            {
                this._backgroundColor = value;
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
                this.txtText.ForeColor = value;
            }
        }

        private Color _borderColor = Color.Gray;
        public Color _BorderColor
        { 
            get { 
                return this._borderColor; 
            }
            set
            {
                this._borderColor = value;
            }
        }

        public CustomBrowseField()
        {
            InitializeComponent();
        }

        public string _Text
        {
            get
            {
                return this.txtText.Text;
            }
            set
            {
                this.txtText.Text = value;
            }
        }

        private bool _readOnly = false;
        public bool _ReadOnly
        {
            get
            {
                return this.txtText.ReadOnly;
            }
            set
            {
                this._readOnly = value;
                this.txtText.ReadOnly = value;
            }
        }

        private bool _enabled = true;
        public bool _Enabled
        {
            get
            {
                return this._enabled;
            }
            set
            {
                this._enabled = value;
                this.txtText.Visible = value;
                this.btnBrowse.Enabled = value;
            }
        }

        public int _SelectionStart
        {
            get
            {
                return this.txtText.SelectionStart;
            }
            set
            {
                this.txtText.SelectionStart = value;
            }
        }

        public int _SelectionLength
        {
            get
            {
                return this.txtText.SelectionLength;
            }
            set
            {
                this.txtText.SelectionLength = value;
            }
        }

        private void CustomBrowseField_Load(object sender, EventArgs e)
        {
            this.BindingControlEvent();
        }

        private void BindingControlEvent()
        {
            this.GotFocus += delegate
            {
                if (this._enabled && !(this._readOnly))
                {
                    this.txtText.Focus();
                }
            };

            this.Leave += delegate
            {
                this._focused = false;
                this.BackColor = Color.White;
                this.txtText.BackColor = Color.White;
            };

            this.txtText.GotFocus += delegate
            {
                this._focused = true;
                this.BackColor = (this._readOnly ? Color.White : this._backgroundColor);
                this.txtText.BackColor = (this._readOnly ? Color.White : this._backgroundColor);
            };

            this.btnBrowse.Enter += delegate
            {
                this._focused = true;
                this.BackColor = (this._readOnly ? Color.White : this._backgroundColor);
                this.txtText.BackColor = (this._readOnly ? Color.White : this._backgroundColor);
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen p = new Pen(_borderColor))
            {
                e.Graphics.DrawRectangle(p, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width, e.ClipRectangle.Height - 1);
            }

            if (!this._enabled || this._readOnly)
            {
                using (SolidBrush brush_normal = new SolidBrush(this.txtText.ForeColor))
                {
                    using (SolidBrush brush_disabled = new SolidBrush(Color.FromArgb(120, 120, 120)))
                    {
                        SolidBrush brush = (!this._enabled ? brush_disabled : brush_normal);

                        e.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(this.DisplayRectangle.X, this.DisplayRectangle.Y, this.DisplayRectangle.Width, this.DisplayRectangle.Height));
                        e.Graphics.DrawString(this._Text, this.txtText.Font, brush, new RectangleF(this.DisplayRectangle.X + 3, this.DisplayRectangle.Y + 4, this.DisplayRectangle.Width, this.DisplayRectangle.Height));
                    }
                }
            }

            if (this._enabled && this._focused)
            {
                this.BackColor = this._backgroundColor;
                this.txtText.BackColor = this._backgroundColor;
            }
            else
            {
                this.BackColor = Color.White;
                this.txtText.BackColor = Color.White;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F6)
            {
                this.btnBrowse.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
