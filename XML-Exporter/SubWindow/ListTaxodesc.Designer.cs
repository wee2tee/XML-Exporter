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
            this.dgvTaxonomy = new System.Windows.Forms.DataGridView();
            this.lblLabel = new System.Windows.Forms.Label();
            this.lblAccnam = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxonomy)).BeginInit();
            this.SuspendLayout();
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
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTaxonomy.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTaxonomy.ColumnHeadersHeight = 27;
            this.dgvTaxonomy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTaxonomy.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvTaxonomy.EnableHeadersVisualStyles = false;
            this.dgvTaxonomy.Location = new System.Drawing.Point(8, 34);
            this.dgvTaxonomy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvTaxonomy.MultiSelect = false;
            this.dgvTaxonomy.Name = "dgvTaxonomy";
            this.dgvTaxonomy.ReadOnly = true;
            this.dgvTaxonomy.RowHeadersVisible = false;
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(2);
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvTaxonomy.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTaxonomy.RowTemplate.Height = 25;
            this.dgvTaxonomy.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTaxonomy.Size = new System.Drawing.Size(709, 229);
            this.dgvTaxonomy.TabIndex = 6;
            // 
            // lblLabel
            // 
            this.lblLabel.AutoSize = true;
            this.lblLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblLabel.Location = new System.Drawing.Point(14, 10);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(130, 16);
            this.lblLabel.TabIndex = 9;
            this.lblLabel.Text = "Taxonomy สำหรับ : ";
            // 
            // lblAccnam
            // 
            this.lblAccnam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAccnam.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblAccnam.Location = new System.Drawing.Point(143, 10);
            this.lblAccnam.Name = "lblAccnam";
            this.lblAccnam.Size = new System.Drawing.Size(570, 16);
            this.lblAccnam.TabIndex = 10;
            this.lblAccnam.Text = "accnum accnam";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(8, 272);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(92, 37);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "ตกลง";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(106, 272);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 37);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "ยกเลิก";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ListTaxodesc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 319);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblAccnam);
            this.Controls.Add(this.lblLabel);
            this.Controls.Add(this.dgvTaxonomy);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(697, 190);
            this.Name = "ListTaxodesc";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "เลือก Taxonomy";
            this.Load += new System.EventHandler(this.ListTaxodesc_Load);
            this.Shown += new System.EventHandler(this.ListTaxodesc_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxonomy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTaxonomy;
        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.Label lblAccnam;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}