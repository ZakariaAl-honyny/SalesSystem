namespace SalesSystem.Desktop.Screens.Employees.Forms
{
    partial class frmListEmployees
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
        /// Required method for Designer support - do NOT modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblHeader = new Label();
            lblFilter = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            dgvEmployees = new DataGridView();
            cmsEmployees = new ContextMenuStrip(components);
            addNewToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            disableToolStripMenuItem = new ToolStripMenuItem();
            btnAdd = new Button();
            btnClose = new Button();
            lblRecordsCount = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).BeginInit();
            cmsEmployees.SuspendLayout();
            SuspendLayout();
            // 
            // lblHeader
            // 
            lblHeader.BackColor = Color.WhiteSmoke;
            lblHeader.Dock = DockStyle.Top;
            lblHeader.FlatStyle = FlatStyle.Flat;
            lblHeader.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblHeader.ForeColor = Color.Firebrick;
            lblHeader.Location = new Point(0, 0);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(1200, 48);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "الموظفون";
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFilter
            // 
            lblFilter.AutoSize = true;
            lblFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblFilter.Location = new Point(21, 68);
            lblFilter.Name = "lblFilter";
            lblFilter.Size = new Size(130, 24);
            lblFilter.TabIndex = 8;
            lblFilter.Text = "البحث عن طريق:";
            // 
            // cbFilterBy
            // 
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيء", "الاسم", "المسمى" });
            cbFilterBy.Location = new Point(200, 64);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(240, 32);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Location = new Point(450, 64);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(450, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.Visible = false;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // dgvEmployees
            // 
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.AllowUserToDeleteRows = false;
            dgvEmployees.AllowUserToOrderColumns = true;
            dgvEmployees.BackgroundColor = SystemColors.Window;
            dgvEmployees.BorderStyle = BorderStyle.Fixed3D;
            dgvEmployees.ColumnHeadersHeight = 50;
            dgvEmployees.ContextMenuStrip = cmsEmployees;
            dgvEmployees.Location = new Point(12, 120);
            dgvEmployees.MultiSelect = false;
            dgvEmployees.Name = "dgvEmployees";
            dgvEmployees.ReadOnly = true;
            dgvEmployees.RowHeadersWidth = 62;
            dgvEmployees.RowTemplate.Height = 28;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.Size = new Size(1176, 500);
            dgvEmployees.TabIndex = 3;
            dgvEmployees.TabStop = false;
            dgvEmployees.DoubleClick += dgvEmployees_DoubleClick;
            // 
            // cmsEmployees
            // 
            cmsEmployees.ImageScalingSize = new Size(20, 20);
            cmsEmployees.Items.AddRange(new ToolStripItem[] { addNewToolStripMenuItem, editToolStripMenuItem, disableToolStripMenuItem });
            cmsEmployees.Name = "cmsEmployees";
            cmsEmployees.RightToLeft = RightToLeft.Yes;
            cmsEmployees.Size = new Size(211, 76);
            cmsEmployees.Opening += cmsEmployees_Opening;
            // 
            // addNewToolStripMenuItem
            // 
            addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            addNewToolStripMenuItem.Size = new Size(210, 24);
            addNewToolStripMenuItem.Text = "إضافة موظف";
            addNewToolStripMenuItem.Click += addNewToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(210, 24);
            editToolStripMenuItem.Text = "تعديل";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // disableToolStripMenuItem
            // 
            disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            disableToolStripMenuItem.Size = new Size(210, 24);
            disableToolStripMenuItem.Text = "تعطيل";
            disableToolStripMenuItem.Click += disableToolStripMenuItem_Click;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAdd.BackColor = Color.White;
            btnAdd.FlatAppearance.BorderColor = Color.Black;
            btnAdd.FlatAppearance.BorderSize = 2;
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAdd.Location = new Point(1007, 650);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(181, 61);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "إضافة";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 650);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.Location = new Point(930, 668);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(52, 24);
            lblRecordsCount.TabIndex = 9;
            lblRecordsCount.Text = "[0]";
            // 
            // frmListEmployees
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1200, 723);
            Controls.Add(lblRecordsCount);
            Controls.Add(btnClose);
            Controls.Add(btnAdd);
            Controls.Add(dgvEmployees);
            Controls.Add(txtFilterValue);
            Controls.Add(cbFilterBy);
            Controls.Add(lblFilter);
            Controls.Add(lblHeader);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListEmployees";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "الموظفون";
            Load += frmListEmployees_Load;
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).EndInit();
            cmsEmployees.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.DataGridView dgvEmployees;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.ContextMenuStrip cmsEmployees;
        private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
    }
}