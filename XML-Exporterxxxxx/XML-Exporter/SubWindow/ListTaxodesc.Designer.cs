namespace XML_Exporter.SubWindow
{
    partial class ListTaxodesc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.lblAccnam = new MetroFramework.Controls.MetroLabel();
            this.dgvTaxonomy = new System.Windows.Forms.DataGridView();
            this.btnOK = new MetroFramework.Controls.MetroButton();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxonomy)).BeginInit();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel1.Location = new System.Drawing.Point(8, 64);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(136, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "Taxonomy สำหรับ : ";
            // 
            // lblAccnam
            // 
            this.lblAccnam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAccnam.Location = new System.Drawing.Point(139, 64);
            this.lblAccnam.Name = "lblAccnam";
            this.lblAccnam.Size = new System.Drawing.Size(510, 19);
            this.lblAccnam.TabIndex = 1;
            this.lblAccnam.Text = "11001 สินทรัพย์หมุนเวียน";
            // 
            // dgvTaxonomy
            // 
            this.dgvTaxonomy.AllowUserToAddRows = false;
            this.dgvTaxonomy.AllowUserToDeleteRows = false;
            this.dgvTaxonomy.AllowUserToResizeColumns = false;
            this.dgvTaxonomy.AllowUserToResizeRows = false;
            this.dgvTaxonomy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTaxonomy.BackgroundColor = System.Drawing.Color.White;
            this.dgvTaxonomy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvTaxonomy.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTaxonomy.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTaxonomy.ColumnHeadersHeight = 27;
            this.dgvTaxonomy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTaxonomy.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvTaxonomy.EnableHeadersVisualStyles = false;
            this.dgvTaxonomy.Location = new System.Drawing.Point(7, 86);
            this.dgvTaxonomy.MultiSelect = false;
            this.dgvTaxonomy.Name = "dgvTaxonomy";
            this.dgvTaxonomy.ReadOnly = true;
            this.dgvTaxonomy.RowHeadersVisible = false;
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(2);
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTaxonomy.RowTemplate.Height = 25;
            this.dgvTaxonomy.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTaxonomy.Size = new System.Drawing.Size(651, 138);
            this.dgvTaxonomy.TabIndex = 6;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Enabled = false;
            this.btnOK.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnOK.Location = new System.Drawing.Point(10, 229);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 31);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "ตกลง";
            this.btnOK.UseSelectable = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnCancel.Location = new System.Drawing.Point(98, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 31);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "ยกเลิก";
            this.btnCancel.UseSelectable = true;
            // 
            // ListTaxodesc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 270);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvTaxonomy);
            this.Controls.Add(this.lblAccnam);
            this.Controls.Add(this.metroLabel1);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 250);
            this.Name = "ListTaxodesc";
            this.ShowInTaskbar = false;
            this.Text = "เลือก Taxonomy";
            this.Load += new System.EventHandler(this.ListTaxodesc_Load);
            this.Shown += new System.EventHandler(this.ListTaxodesc_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxonomy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel lblAccnam;
        private System.Windows.Forms.DataGridView dgvTaxonomy;
        private MetroFramework.Controls.MetroButton btnOK;
        private MetroFramework.Controls.MetroButton btnCancel;
    }
}