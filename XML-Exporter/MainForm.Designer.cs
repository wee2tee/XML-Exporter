namespace XML_Exporter
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControlGlacc = new System.Windows.Forms.TabControl();
            this.tabAsset = new System.Windows.Forms.TabPage();
            this.dgvGlaccAsset = new System.Windows.Forms.DataGridView();
            this.tabLia = new System.Windows.Forms.TabPage();
            this.dgvGlaccLia = new System.Windows.Forms.DataGridView();
            this.tabCap = new System.Windows.Forms.TabPage();
            this.dgvGlaccCap = new System.Windows.Forms.DataGridView();
            this.tabRev = new System.Windows.Forms.TabPage();
            this.dgvGlaccRev = new System.Windows.Forms.DataGridView();
            this.tabExp = new System.Windows.Forms.TabPage();
            this.dgvGlaccExp = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbAccnamEng = new System.Windows.Forms.RadioButton();
            this.rbAccnamThai = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnOpenData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.btnExportXML = new System.Windows.Forms.ToolStripButton();
            this.btnExportCSG = new System.Windows.Forms.ToolStripButton();
            this.btnRegister = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgramDateLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblCompNam = new System.Windows.Forms.Label();
            this.lblCompCod = new System.Windows.Forms.Label();
            this.lblDataPath = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblProgramPath = new System.Windows.Forms.Label();
            this.tabControlGlacc.SuspendLayout();
            this.tabAsset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlaccAsset)).BeginInit();
            this.tabLia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlaccLia)).BeginInit();
            this.tabCap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlaccCap)).BeginInit();
            this.tabRev.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlaccRev)).BeginInit();
            this.tabExp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlaccExp)).BeginInit();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlGlacc
            // 
            this.tabControlGlacc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlGlacc.Controls.Add(this.tabAsset);
            this.tabControlGlacc.Controls.Add(this.tabLia);
            this.tabControlGlacc.Controls.Add(this.tabCap);
            this.tabControlGlacc.Controls.Add(this.tabRev);
            this.tabControlGlacc.Controls.Add(this.tabExp);
            this.tabControlGlacc.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.tabControlGlacc.Location = new System.Drawing.Point(5, 150);
            this.tabControlGlacc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControlGlacc.Name = "tabControlGlacc";
            this.tabControlGlacc.SelectedIndex = 0;
            this.tabControlGlacc.Size = new System.Drawing.Size(1222, 522);
            this.tabControlGlacc.TabIndex = 16;
            // 
            // tabAsset
            // 
            this.tabAsset.Controls.Add(this.dgvGlaccAsset);
            this.tabAsset.Location = new System.Drawing.Point(4, 25);
            this.tabAsset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabAsset.Name = "tabAsset";
            this.tabAsset.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabAsset.Size = new System.Drawing.Size(1214, 493);
            this.tabAsset.TabIndex = 0;
            this.tabAsset.Text = "  สินทรัพย์  ";
            this.tabAsset.UseVisualStyleBackColor = true;
            // 
            // dgvGlaccAsset
            // 
            this.dgvGlaccAsset.AllowUserToAddRows = false;
            this.dgvGlaccAsset.AllowUserToDeleteRows = false;
            this.dgvGlaccAsset.AllowUserToResizeColumns = false;
            this.dgvGlaccAsset.AllowUserToResizeRows = false;
            this.dgvGlaccAsset.BackgroundColor = System.Drawing.Color.White;
            this.dgvGlaccAsset.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGlaccAsset.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGlaccAsset.ColumnHeadersHeight = 27;
            this.dgvGlaccAsset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGlaccAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGlaccAsset.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvGlaccAsset.EnableHeadersVisualStyles = false;
            this.dgvGlaccAsset.Location = new System.Drawing.Point(3, 4);
            this.dgvGlaccAsset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvGlaccAsset.MultiSelect = false;
            this.dgvGlaccAsset.Name = "dgvGlaccAsset";
            this.dgvGlaccAsset.ReadOnly = true;
            this.dgvGlaccAsset.RowHeadersVisible = false;
            this.dgvGlaccAsset.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvGlaccAsset.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvGlaccAsset.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.dgvGlaccAsset.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvGlaccAsset.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(2);
            this.dgvGlaccAsset.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvGlaccAsset.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvGlaccAsset.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGlaccAsset.RowTemplate.Height = 25;
            this.dgvGlaccAsset.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGlaccAsset.Size = new System.Drawing.Size(1208, 485);
            this.dgvGlaccAsset.StandardTab = true;
            this.dgvGlaccAsset.TabIndex = 5;
            // 
            // tabLia
            // 
            this.tabLia.Controls.Add(this.dgvGlaccLia);
            this.tabLia.Location = new System.Drawing.Point(4, 25);
            this.tabLia.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabLia.Name = "tabLia";
            this.tabLia.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabLia.Size = new System.Drawing.Size(1214, 456);
            this.tabLia.TabIndex = 1;
            this.tabLia.Text = "  หนี้สิน  ";
            this.tabLia.UseVisualStyleBackColor = true;
            // 
            // dgvGlaccLia
            // 
            this.dgvGlaccLia.AllowUserToAddRows = false;
            this.dgvGlaccLia.AllowUserToDeleteRows = false;
            this.dgvGlaccLia.AllowUserToResizeColumns = false;
            this.dgvGlaccLia.AllowUserToResizeRows = false;
            this.dgvGlaccLia.BackgroundColor = System.Drawing.Color.White;
            this.dgvGlaccLia.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGlaccLia.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGlaccLia.ColumnHeadersHeight = 27;
            this.dgvGlaccLia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGlaccLia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGlaccLia.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvGlaccLia.EnableHeadersVisualStyles = false;
            this.dgvGlaccLia.Location = new System.Drawing.Point(3, 4);
            this.dgvGlaccLia.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvGlaccLia.MultiSelect = false;
            this.dgvGlaccLia.Name = "dgvGlaccLia";
            this.dgvGlaccLia.ReadOnly = true;
            this.dgvGlaccLia.RowHeadersVisible = false;
            this.dgvGlaccLia.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvGlaccLia.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvGlaccLia.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.dgvGlaccLia.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvGlaccLia.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(2);
            this.dgvGlaccLia.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvGlaccLia.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvGlaccLia.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGlaccLia.RowTemplate.Height = 25;
            this.dgvGlaccLia.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGlaccLia.Size = new System.Drawing.Size(1208, 448);
            this.dgvGlaccLia.StandardTab = true;
            this.dgvGlaccLia.TabIndex = 5;
            // 
            // tabCap
            // 
            this.tabCap.Controls.Add(this.dgvGlaccCap);
            this.tabCap.Location = new System.Drawing.Point(4, 25);
            this.tabCap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabCap.Name = "tabCap";
            this.tabCap.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabCap.Size = new System.Drawing.Size(1214, 456);
            this.tabCap.TabIndex = 2;
            this.tabCap.Text = "  ทุน  ";
            this.tabCap.UseVisualStyleBackColor = true;
            // 
            // dgvGlaccCap
            // 
            this.dgvGlaccCap.AllowUserToAddRows = false;
            this.dgvGlaccCap.AllowUserToDeleteRows = false;
            this.dgvGlaccCap.AllowUserToResizeColumns = false;
            this.dgvGlaccCap.AllowUserToResizeRows = false;
            this.dgvGlaccCap.BackgroundColor = System.Drawing.Color.White;
            this.dgvGlaccCap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGlaccCap.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvGlaccCap.ColumnHeadersHeight = 27;
            this.dgvGlaccCap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGlaccCap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGlaccCap.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvGlaccCap.EnableHeadersVisualStyles = false;
            this.dgvGlaccCap.Location = new System.Drawing.Point(3, 4);
            this.dgvGlaccCap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvGlaccCap.MultiSelect = false;
            this.dgvGlaccCap.Name = "dgvGlaccCap";
            this.dgvGlaccCap.ReadOnly = true;
            this.dgvGlaccCap.RowHeadersVisible = false;
            this.dgvGlaccCap.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvGlaccCap.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvGlaccCap.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.dgvGlaccCap.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvGlaccCap.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(2);
            this.dgvGlaccCap.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvGlaccCap.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvGlaccCap.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGlaccCap.RowTemplate.Height = 25;
            this.dgvGlaccCap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGlaccCap.Size = new System.Drawing.Size(1208, 448);
            this.dgvGlaccCap.StandardTab = true;
            this.dgvGlaccCap.TabIndex = 5;
            // 
            // tabRev
            // 
            this.tabRev.Controls.Add(this.dgvGlaccRev);
            this.tabRev.Location = new System.Drawing.Point(4, 25);
            this.tabRev.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabRev.Name = "tabRev";
            this.tabRev.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabRev.Size = new System.Drawing.Size(1214, 456);
            this.tabRev.TabIndex = 3;
            this.tabRev.Text = "  รายได้  ";
            this.tabRev.UseVisualStyleBackColor = true;
            // 
            // dgvGlaccRev
            // 
            this.dgvGlaccRev.AllowUserToAddRows = false;
            this.dgvGlaccRev.AllowUserToDeleteRows = false;
            this.dgvGlaccRev.AllowUserToResizeColumns = false;
            this.dgvGlaccRev.AllowUserToResizeRows = false;
            this.dgvGlaccRev.BackgroundColor = System.Drawing.Color.White;
            this.dgvGlaccRev.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGlaccRev.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvGlaccRev.ColumnHeadersHeight = 27;
            this.dgvGlaccRev.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGlaccRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGlaccRev.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvGlaccRev.EnableHeadersVisualStyles = false;
            this.dgvGlaccRev.Location = new System.Drawing.Point(3, 4);
            this.dgvGlaccRev.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvGlaccRev.MultiSelect = false;
            this.dgvGlaccRev.Name = "dgvGlaccRev";
            this.dgvGlaccRev.ReadOnly = true;
            this.dgvGlaccRev.RowHeadersVisible = false;
            this.dgvGlaccRev.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvGlaccRev.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvGlaccRev.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.dgvGlaccRev.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvGlaccRev.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(2);
            this.dgvGlaccRev.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvGlaccRev.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvGlaccRev.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGlaccRev.RowTemplate.Height = 25;
            this.dgvGlaccRev.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGlaccRev.Size = new System.Drawing.Size(1208, 448);
            this.dgvGlaccRev.StandardTab = true;
            this.dgvGlaccRev.TabIndex = 5;
            // 
            // tabExp
            // 
            this.tabExp.Controls.Add(this.dgvGlaccExp);
            this.tabExp.Location = new System.Drawing.Point(4, 25);
            this.tabExp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabExp.Name = "tabExp";
            this.tabExp.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabExp.Size = new System.Drawing.Size(1214, 456);
            this.tabExp.TabIndex = 4;
            this.tabExp.Text = "  ค่าใช้จ่าย  ";
            this.tabExp.UseVisualStyleBackColor = true;
            // 
            // dgvGlaccExp
            // 
            this.dgvGlaccExp.AllowUserToAddRows = false;
            this.dgvGlaccExp.AllowUserToDeleteRows = false;
            this.dgvGlaccExp.AllowUserToResizeColumns = false;
            this.dgvGlaccExp.AllowUserToResizeRows = false;
            this.dgvGlaccExp.BackgroundColor = System.Drawing.Color.White;
            this.dgvGlaccExp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(207)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGlaccExp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvGlaccExp.ColumnHeadersHeight = 27;
            this.dgvGlaccExp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGlaccExp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGlaccExp.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvGlaccExp.EnableHeadersVisualStyles = false;
            this.dgvGlaccExp.Location = new System.Drawing.Point(3, 4);
            this.dgvGlaccExp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvGlaccExp.MultiSelect = false;
            this.dgvGlaccExp.Name = "dgvGlaccExp";
            this.dgvGlaccExp.ReadOnly = true;
            this.dgvGlaccExp.RowHeadersVisible = false;
            this.dgvGlaccExp.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvGlaccExp.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvGlaccExp.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.dgvGlaccExp.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvGlaccExp.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(2);
            this.dgvGlaccExp.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvGlaccExp.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvGlaccExp.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGlaccExp.RowTemplate.Height = 25;
            this.dgvGlaccExp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGlaccExp.Size = new System.Drawing.Size(1208, 448);
            this.dgvGlaccExp.StandardTab = true;
            this.dgvGlaccExp.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.rbAccnamEng);
            this.panel2.Controls.Add(this.rbAccnamThai);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(964, 143);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(258, 31);
            this.panel2.TabIndex = 18;
            // 
            // rbAccnamEng
            // 
            this.rbAccnamEng.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbAccnamEng.AutoSize = true;
            this.rbAccnamEng.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.rbAccnamEng.Location = new System.Drawing.Point(202, 4);
            this.rbAccnamEng.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbAccnamEng.Name = "rbAccnamEng";
            this.rbAccnamEng.Size = new System.Drawing.Size(51, 20);
            this.rbAccnamEng.TabIndex = 2;
            this.rbAccnamEng.Text = "Eng.";
            this.rbAccnamEng.UseVisualStyleBackColor = true;
            // 
            // rbAccnamThai
            // 
            this.rbAccnamThai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbAccnamThai.AutoSize = true;
            this.rbAccnamThai.Checked = true;
            this.rbAccnamThai.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.rbAccnamThai.Location = new System.Drawing.Point(144, 4);
            this.rbAccnamThai.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbAccnamThai.Name = "rbAccnamThai";
            this.rbAccnamThai.Size = new System.Drawing.Size(50, 20);
            this.rbAccnamThai.TabIndex = 1;
            this.rbAccnamThai.TabStop = true;
            this.rbAccnamThai.Text = "ไทย";
            this.rbAccnamThai.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(6, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "แสดงชื่อบัญชีเป็นภาษา";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenData,
            this.toolStripSeparator1,
            this.btnCopy,
            this.btnExportXML,
            this.btnExportCSG,
            this.btnRegister});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1231, 39);
            this.toolStrip1.TabIndex = 26;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnOpenData
            // 
            this.btnOpenData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpenData.Image = global::XML_Exporter.Properties.Resources.icon_folder;
            this.btnOpenData.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnOpenData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenData.Name = "btnOpenData";
            this.btnOpenData.Padding = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnOpenData.Size = new System.Drawing.Size(37, 36);
            this.btnOpenData.Text = "เลือกที่เก็บข้อมูล <F3>";
            this.btnOpenData.Click += new System.EventHandler(this.btnOpenData_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // btnCopy
            // 
            this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCopy.Enabled = false;
            this.btnCopy.Image = global::XML_Exporter.Properties.Resources.copy;
            this.btnCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(36, 36);
            this.btnCopy.Text = "คัดลอกการจับคู่ Taxonomy จากที่เก็บข้อมูลอื่น";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnExportXML
            // 
            this.btnExportXML.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExportXML.Enabled = false;
            this.btnExportXML.Image = global::XML_Exporter.Properties.Resources.icon_xml;
            this.btnExportXML.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnExportXML.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExportXML.Name = "btnExportXML";
            this.btnExportXML.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btnExportXML.Size = new System.Drawing.Size(38, 36);
            this.btnExportXML.Text = "ส่งออกเป็นไฟล์ .xml สำหรับเปิดด้วยโปรแกรม Excel <F4>";
            this.btnExportXML.Click += new System.EventHandler(this.btnExportXML_Click);
            // 
            // btnExportCSG
            // 
            this.btnExportCSG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExportCSG.Enabled = false;
            this.btnExportCSG.Image = global::XML_Exporter.Properties.Resources.icon_csg;
            this.btnExportCSG.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnExportCSG.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExportCSG.Name = "btnExportCSG";
            this.btnExportCSG.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btnExportCSG.Size = new System.Drawing.Size(38, 36);
            this.btnExportCSG.Text = "ส่งออกเป็นไฟล์ .csg สำหรับใช้งานกับ SmartBiz DBD Connect <F5>";
            this.btnExportCSG.Click += new System.EventHandler(this.btnExportCSG_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRegister.Image = global::XML_Exporter.Properties.Resources.key_register;
            this.btnRegister.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(36, 36);
            this.btnRegister.Text = "ลงทะเบียนโปรแกรม";
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblInfo,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.ProgramDateLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 674);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1231, 22);
            this.statusStrip1.TabIndex = 27;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblInfo
            // 
            this.lblInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblInfo.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1204, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel2.Text = "|";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ProgramDateLabel
            // 
            this.ProgramDateLabel.BackColor = System.Drawing.SystemColors.Control;
            this.ProgramDateLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ProgramDateLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.ProgramDateLabel.Name = "ProgramDateLabel";
            this.ProgramDateLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ProgramDateLabel.Size = new System.Drawing.Size(0, 17);
            this.ProgramDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 28;
            this.label1.Text = "ชื่อข้อมูล";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(654, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 16);
            this.label2.TabIndex = 29;
            this.label2.Text = "รหัส";
            this.label2.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 16);
            this.label4.TabIndex = 30;
            this.label4.Text = "ที่เก็บข้อมูล";
            // 
            // lblCompNam
            // 
            this.lblCompNam.BackColor = System.Drawing.Color.White;
            this.lblCompNam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCompNam.Location = new System.Drawing.Point(113, 60);
            this.lblCompNam.Name = "lblCompNam";
            this.lblCompNam.Size = new System.Drawing.Size(525, 23);
            this.lblCompNam.TabIndex = 31;
            this.lblCompNam.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCompCod
            // 
            this.lblCompCod.BackColor = System.Drawing.Color.White;
            this.lblCompCod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCompCod.Location = new System.Drawing.Point(742, 60);
            this.lblCompCod.Name = "lblCompCod";
            this.lblCompCod.Size = new System.Drawing.Size(110, 23);
            this.lblCompCod.TabIndex = 32;
            this.lblCompCod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCompCod.Visible = false;
            // 
            // lblDataPath
            // 
            this.lblDataPath.BackColor = System.Drawing.Color.White;
            this.lblDataPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDataPath.Location = new System.Drawing.Point(113, 88);
            this.lblDataPath.Name = "lblDataPath";
            this.lblDataPath.Size = new System.Drawing.Size(525, 23);
            this.lblDataPath.TabIndex = 33;
            this.lblDataPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 16);
            this.label5.TabIndex = 34;
            this.label5.Text = "ที่เก็บโปรแกรม";
            // 
            // lblProgramPath
            // 
            this.lblProgramPath.BackColor = System.Drawing.Color.White;
            this.lblProgramPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblProgramPath.Location = new System.Drawing.Point(113, 116);
            this.lblProgramPath.Name = "lblProgramPath";
            this.lblProgramPath.Size = new System.Drawing.Size(525, 23);
            this.lblProgramPath.TabIndex = 35;
            this.lblProgramPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1231, 696);
            this.Controls.Add(this.lblProgramPath);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblDataPath);
            this.Controls.Add(this.lblCompCod);
            this.Controls.Add(this.lblCompNam);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tabControlGlacc);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(650, 300);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XML Exporter";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.tabControlGlacc.ResumeLayout(false);
            this.tabAsset.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlaccAsset)).EndInit();
            this.tabLia.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlaccLia)).EndInit();
            this.tabCap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlaccCap)).EndInit();
            this.tabRev.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlaccRev)).EndInit();
            this.tabExp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlaccExp)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbAccnamEng;
        private System.Windows.Forms.RadioButton rbAccnamThai;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControlGlacc;
        private System.Windows.Forms.TabPage tabAsset;
        private System.Windows.Forms.DataGridView dgvGlaccAsset;
        private System.Windows.Forms.TabPage tabLia;
        private System.Windows.Forms.DataGridView dgvGlaccLia;
        private System.Windows.Forms.TabPage tabCap;
        private System.Windows.Forms.DataGridView dgvGlaccCap;
        private System.Windows.Forms.TabPage tabRev;
        private System.Windows.Forms.DataGridView dgvGlaccRev;
        private System.Windows.Forms.TabPage tabExp;
        private System.Windows.Forms.DataGridView dgvGlaccExp;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnOpenData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnExportXML;
        private System.Windows.Forms.ToolStripButton btnExportCSG;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCompNam;
        private System.Windows.Forms.Label lblCompCod;
        private System.Windows.Forms.Label lblDataPath;
        private System.Windows.Forms.ToolStripStatusLabel lblInfo;
        public System.Windows.Forms.ToolStripButton btnRegister;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblProgramPath;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripStatusLabel ProgramDateLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;

    }
}

