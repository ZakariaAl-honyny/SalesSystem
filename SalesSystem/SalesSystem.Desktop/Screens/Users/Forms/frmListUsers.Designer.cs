namespace SalesSystem.Desktop.Screens.Users.Forms
{
    partial class frmListUsers
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblMode = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            dgvUsers = new DataGridView();
            cmsUsers = new ContextMenuStrip(components);
            addNewToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            changePasswordToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            resetPasswordToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            toggleStatusToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            btnAdd = new Button();
            btnClose = new Button();
            lblRecordsCount = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            cmsUsers.SuspendLayout();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(505, 20);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(190, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إدارة المستخدمين";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(21, 115);
            label2.Name = "label2";
            label2.Size = new Size(130, 24);
            label2.TabIndex = 6;
            label2.Text = "البحث عن طريق:";
            // 
            // cbFilterBy
            // 
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم المستخدم", "اسم المستخدم", "الحالة", "الدور" });
            cbFilterBy.Location = new Point(200, 111);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(260, 32);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Location = new Point(470, 111);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(500, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.Visible = false;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AllowUserToOrderColumns = true;
            dgvUsers.BackgroundColor = SystemColors.Window;
            dgvUsers.BorderStyle = BorderStyle.Fixed3D;
            dgvUsers.ColumnHeadersHeight = 50;
            dgvUsers.ContextMenuStrip = cmsUsers;
            dgvUsers.Location = new Point(12, 160);
            dgvUsers.MultiSelect = false;
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersWidth = 62;
            dgvUsers.RowTemplate.Height = 28;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.Size = new Size(1176, 480);
            dgvUsers.TabIndex = 3;
            dgvUsers.TabStop = false;
            dgvUsers.DoubleClick += dgvUsers_DoubleClick;
            // 
            // cmsUsers
            // 
            cmsUsers.ImageScalingSize = new Size(20, 20);
            cmsUsers.Items.AddRange(new ToolStripItem[] { addNewToolStripMenuItem, editToolStripMenuItem, changePasswordToolStripMenuItem, toolStripSeparator3, resetPasswordToolStripMenuItem, toolStripSeparator1, toggleStatusToolStripMenuItem, toolStripSeparator2, refreshToolStripMenuItem });
            cmsUsers.Name = "cmsUsers";
            cmsUsers.RightToLeft = RightToLeft.Yes;
            cmsUsers.Size = new Size(260, 214);
            cmsUsers.Opening += cmsUsers_Opening;
            // 
            // addNewToolStripMenuItem
            // 
            addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            addNewToolStripMenuItem.Size = new Size(259, 32);
            addNewToolStripMenuItem.Text = "إضافة مستخدم جديد";
            addNewToolStripMenuItem.Click += addNewToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(259, 32);
            editToolStripMenuItem.Text = "تعديل المستخدم";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // changePasswordToolStripMenuItem
            // 
            changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            changePasswordToolStripMenuItem.Size = new Size(259, 32);
            changePasswordToolStripMenuItem.Text = "تغيير كلمة المرور";
            changePasswordToolStripMenuItem.Click += changePasswordToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(256, 6);
            // 
            // resetPasswordToolStripMenuItem
            // 
            resetPasswordToolStripMenuItem.Name = "resetPasswordToolStripMenuItem";
            resetPasswordToolStripMenuItem.Size = new Size(259, 32);
            resetPasswordToolStripMenuItem.Text = "إعادة تعيين كلمة المرور";
            resetPasswordToolStripMenuItem.Click += resetPasswordToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(256, 6);
            // 
            // toggleStatusToolStripMenuItem
            // 
            toggleStatusToolStripMenuItem.Name = "toggleStatusToolStripMenuItem";
            toggleStatusToolStripMenuItem.Size = new Size(259, 32);
            toggleStatusToolStripMenuItem.Text = "تنشيط / تعطيل";
            toggleStatusToolStripMenuItem.Click += toggleStatusToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(256, 6);
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new Size(259, 32);
            refreshToolStripMenuItem.Text = "تحديث";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.BackColor = Color.White;
            btnAdd.FlatAppearance.BorderColor = Color.Black;
            btnAdd.FlatAppearance.BorderSize = 2;
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAdd.Location = new Point(1103, 80);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 75);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "+ جديد";
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
            lblRecordsCount.Location = new Point(1100, 665);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(52, 24);
            lblRecordsCount.TabIndex = 7;
            lblRecordsCount.Text = "[0]";
            // 
            // frmListUsers
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1200, 723);
            Controls.Add(lblRecordsCount);
            Controls.Add(label2);
            Controls.Add(btnClose);
            Controls.Add(btnAdd);
            Controls.Add(dgvUsers);
            Controls.Add(txtFilterValue);
            Controls.Add(cbFilterBy);
            Controls.Add(lblMode);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListUsers";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدارة المستخدمين";
            Load += frmListUsers_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            cmsUsers.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip cmsUsers;
        private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toggleStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem resetPasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    }
}
