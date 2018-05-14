namespace XML_Exporter.SubWindow
{
    partial class CopyOptionsDialog
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
            this.chkBoth = new System.Windows.Forms.CheckBox();
            this.chkOnlyAccnum = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkBoth
            // 
            this.chkBoth.AutoSize = true;
            this.chkBoth.Location = new System.Drawing.Point(27, 52);
            this.chkBoth.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkBoth.Name = "chkBoth";
            this.chkBoth.Size = new System.Drawing.Size(429, 20);
            this.chkBoth.TabIndex = 2;
            this.chkBoth.Text = "คัดลอกเฉพาะรายการที่เลขที่บัญชี และ ชื่อบัญชีตรงกันกับที่เก็บข้อมูลปัจจุบัน";
            this.chkBoth.UseVisualStyleBackColor = true;
            // 
            // chkOnlyAccnum
            // 
            this.chkOnlyAccnum.AutoSize = true;
            this.chkOnlyAccnum.Checked = true;
            this.chkOnlyAccnum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOnlyAccnum.Enabled = false;
            this.chkOnlyAccnum.Location = new System.Drawing.Point(27, 24);
            this.chkOnlyAccnum.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkOnlyAccnum.Name = "chkOnlyAccnum";
            this.chkOnlyAccnum.Size = new System.Drawing.Size(356, 20);
            this.chkOnlyAccnum.TabIndex = 1;
            this.chkOnlyAccnum.Text = "คัดลอกเฉพาะรายการที่เลขที่บัญชีตรงกันกับที่เก็บข้อมูลปัจจุบัน";
            this.chkOnlyAccnum.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(166, 96);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 33);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "ตกลง";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(246, 96);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 33);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "ยกเลิก";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // CopyOptionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 146);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkOnlyAccnum);
            this.Controls.Add(this.chkBoth);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyOptionsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ตัวเลือกในการคัดลอก Taxonomy";
            this.Load += new System.EventHandler(this.CopyOptionsDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkBoth;
        private System.Windows.Forms.CheckBox chkOnlyAccnum;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}