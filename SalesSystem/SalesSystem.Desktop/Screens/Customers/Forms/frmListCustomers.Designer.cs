namespace SalesSystem.Desktop.Screens.Customers.Forms
{
    partial class frmListCustomers
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            cmsCustomers = new ContextMenuStrip(components);
            refreshToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            editToolStripMenuItem = new ToolStripMenuItem();
            addNewCustomerToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            deactivateToolStripMenuItem = new ToolStripMenuItem();
            reactivateToolStripMenuItem = new ToolStripMenuItem();
            lblMode = new Label();
            dgvCustomers = new DataGridView();
            btnAdd = new Button();
            btnClose = new Button();
            lblRecordsCount = new Label();
            label1 = new Label();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            chkShowInactive = new CheckBox();
            cmsCustomers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).BeginInit();
            SuspendLayout();
            // 
            // cmsCustomers
            // 
            cmsCustomers.BackColor = Color.White;
            cmsCustomers.Font = new Font("Tahoma", 10F);
            cmsCustomers.ImageScalingSize = new Size(24, 24);
            cmsCustomers.Items.AddRange(new ToolStripItem[] { refreshToolStripMenuItem, toolStripSeparator1, editToolStripMenuItem, addNewCustomerToolStripMenuItem, toolStripSeparator2, deactivateToolStripMenuItem, reactivateToolStripMenuItem });
            cmsCustomers.Name = "cmsCustomers";
            cmsCustomers.RightToLeft = RightToLeft.Yes;
            cmsCustomers.Size = new Size(250, 170);
            cmsCustomers.Opening += cmsCustomers_Opening;
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            refreshToolStripMenuItem.Size = new Size(249, 32);
            refreshToolStripMenuItem.Text = "&تحديث";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(246, 6);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(249, 32);
            editToolStripMenuItem.Text = "تعديل";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // addNewCustomerToolStripMenuItem
            // 
            addNewCustomerToolStripMenuItem.Name = "addNewCustomerToolStripMenuItem";
            addNewCustomerToolStripMenuItem.Size = new Size(249, 32);
            addNewCustomerToolStripMenuItem.Text = "&إضافة عميل جديد";
            addNewCustomerToolStripMenuItem.Click += addNewCustomerToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(246, 6);
            // 
            // deactivateToolStripMenuItem
            // 
            deactivateToolStripMenuItem.Name = "deactivateToolStripMenuItem";
            deactivateToolStripMenuItem.Size = new Size(249, 32);
            deactivateToolStripMenuItem.Text = "تعطيل";
            deactivateToolStripMenuItem.Click += deactivateToolStripMenuItem_Click;
            // 
            // reactivateToolStripMenuItem
            // 
            reactivateToolStripMenuItem.Name = "reactivateToolStripMenuItem";
            reactivateToolStripMenuItem.Size = new Size(249, 32);
            reactivateToolStripMenuItem.Text = "تنشيط";
            reactivateToolStripMenuItem.Click += reactivateToolStripMenuItem_Click;
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(667, 20);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(190, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إدارة العملاء";
            // 
            // dgvCustomers
            // 
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.AllowUserToOrderColumns = true;
            dgvCustomers.BackgroundColor = SystemColors.Window;
            dgvCustomers.BorderStyle = BorderStyle.Fixed3D;
            dgvCustomers.ColumnHeadersHeight = 50;
            dgvCustomers.ContextMenuStrip = cmsCustomers;
            dgvCustomers.Location = new Point(12, 160);
            dgvCustomers.MultiSelect = false;
            dgvCustomers.Name = "dgvCustomers";
            dgvCustomers.ReadOnly = true;
            dgvCustomers.RowHeadersWidth = 62;
            dgvCustomers.RowTemplate.Height = 28;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.Size = new Size(1540, 540);
            dgvCustomers.TabIndex = 34;
            dgvCustomers.TabStop = false;
            dgvCustomers.DoubleClick += dgvCustomers_DoubleClick;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.BackColor = Color.White;
            btnAdd.FlatAppearance.BorderColor = Color.Black;
            btnAdd.FlatAppearance.BorderSize = 2;
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAdd.Location = new Point(1470, 80);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 75);
            btnAdd.TabIndex = 5;
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
            btnClose.Location = new Point(12, 710);
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
            lblRecordsCount.Location = new Point(1380, 720);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(72, 24);
            lblRecordsCount.TabIndex = 4;
            lblRecordsCount.Text = "[????]";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(1220, 720);
            label1.Name = "label1";
            label1.Size = new Size(152, 24);
            label1.TabIndex = 3;
            label1.Text = "عدد العملاء:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(21, 115);
            label2.Name = "label2";
            label2.Size = new Size(130, 24);
            label2.TabIndex = 5;
            label2.Text = "البحث عن طريق:";
            // 
            // cbFilterBy
            // 
            cbFilterBy.BackColor = SystemColors.Control;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم العميل", "اسم العميل", "رقم الهاتف" });
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
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // chkShowInactive
            // 
            chkShowInactive.AutoSize = true;
            chkShowInactive.Font = new Font("Tahoma", 10F);
            chkShowInactive.Location = new Point(1000, 115);
            chkShowInactive.Name = "chkShowInactive";
            chkShowInactive.Size = new Size(170, 28);
            chkShowInactive.TabIndex = 3;
            chkShowInactive.Text = "إظهار غير النشط";
            chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
            // 
            // frmListCustomers
            // 
            AcceptButton = btnAdd;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(lblMode);
            Controls.Add(dgvCustomers);
            Controls.Add(label2);
            Controls.Add(cbFilterBy);
            Controls.Add(txtFilterValue);
            Controls.Add(chkShowInactive);
            Controls.Add(label1);
            Controls.Add(lblRecordsCount);
            Controls.Add(btnAdd);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListCustomers";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدارة العملاء";
            Load += frmListCustomers_Load;
            cmsCustomers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.ContextMenuStrip cmsCustomers;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewCustomerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deactivateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reactivateToolStripMenuItem;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.CheckBox chkShowInactive;
    }
}
