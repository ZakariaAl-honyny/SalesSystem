namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmListAccounts
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
            cmsAccounts = new ContextMenuStrip(components);
            refreshToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            showDetailsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            editToolStripMenuItem = new ToolStripMenuItem();
            addNewToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            lblMode = new Label();
            dgvAccounts = new DataGridView();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            label2 = new Label();
            lblRecordsCount = new Label();
            label1 = new Label();
            btnAddNew = new Button();
            btnClose = new Button();
            cmsAccounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
            SuspendLayout();

            // cmsAccounts
            cmsAccounts.BackColor = Color.White;
            cmsAccounts.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmsAccounts.ImageScalingSize = new Size(24, 24);
            cmsAccounts.Items.AddRange(new ToolStripItem[] {
                refreshToolStripMenuItem, toolStripSeparator1,
                showDetailsToolStripMenuItem, toolStripSeparator2,
                editToolStripMenuItem, addNewToolStripMenuItem, toolStripSeparator3,
                deleteToolStripMenuItem });
            cmsAccounts.Name = "cmsAccounts";
            cmsAccounts.RightToLeft = RightToLeft.Yes;
            cmsAccounts.Size = new Size(200, 176);
            cmsAccounts.Opening += cmsAccounts_Opening;

            // refreshToolStripMenuItem
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            refreshToolStripMenuItem.Size = new Size(199, 30);
            refreshToolStripMenuItem.Text = "&تحديث";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;

            // toolStripSeparator1
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(196, 6);

            // showDetailsToolStripMenuItem
            showDetailsToolStripMenuItem.Name = "showDetailsToolStripMenuItem";
            showDetailsToolStripMenuItem.Size = new Size(199, 30);
            showDetailsToolStripMenuItem.Text = "&عرض التفاصيل";
            showDetailsToolStripMenuItem.Click += showDetailsToolStripMenuItem_Click;

            // toolStripSeparator2
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(196, 6);

            // editToolStripMenuItem
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(199, 30);
            editToolStripMenuItem.Text = "&تعديل";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;

            // addNewToolStripMenuItem
            addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            addNewToolStripMenuItem.Size = new Size(199, 30);
            addNewToolStripMenuItem.Text = "&إضافة حساب جديد";
            addNewToolStripMenuItem.Click += addNewToolStripMenuItem_Click;

            // toolStripSeparator3
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(196, 6);

            // deleteToolStripMenuItem
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(199, 30);
            deleteToolStripMenuItem.Text = "&حذف";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;

            // lblMode
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(505, 20);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(196, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "دليل الحسابات";

            // dgvAccounts
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;
            dgvAccounts.AllowUserToOrderColumns = true;
            dgvAccounts.BackgroundColor = SystemColors.Window;
            dgvAccounts.BorderStyle = BorderStyle.Fixed3D;
            dgvAccounts.ColumnHeadersHeight = 50;
            dgvAccounts.ContextMenuStrip = cmsAccounts;
            dgvAccounts.Location = new Point(12, 160);
            dgvAccounts.MultiSelect = false;
            dgvAccounts.Name = "dgvAccounts";
            dgvAccounts.ReadOnly = true;
            dgvAccounts.RowHeadersWidth = 62;
            dgvAccounts.RowTemplate.Height = 28;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.Size = new Size(1176, 480);
            dgvAccounts.TabIndex = 1;
            dgvAccounts.TabStop = false;
            dgvAccounts.DoubleClick += dgvAccounts_DoubleClick;
            // 
            // label2
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(21, 115);
            label2.Name = "label2";
            label2.Size = new Size(130, 24);
            label2.TabIndex = 6;
            label2.Text = "البحث عن طريق:";
            // 
            // cbFilterBy
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "كود الحساب", "اسم الحساب", "الطبيعة", "المستوى" });
            cbFilterBy.Location = new Point(200, 111);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(260, 32);
            cbFilterBy.TabIndex = 2;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Location = new Point(470, 111);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(500, 32);
            txtFilterValue.TabIndex = 3;
            txtFilterValue.Visible = false;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // lblRecordsCount
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.Location = new Point(1100, 665);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(52, 24);
            lblRecordsCount.TabIndex = 12;
            lblRecordsCount.Text = "[0]";
            // 
            // label1
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(1250, 665);
            label1.Name = "label1";
            label1.Size = new Size(155, 24);
            label1.TabIndex = 11;
            label1.Text = "عدد الحسابات:";
            // 
            // btnAddNew
            btnAddNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddNew.BackColor = Color.White;
            btnAddNew.FlatAppearance.BorderColor = Color.Black;
            btnAddNew.FlatAppearance.BorderSize = 2;
            btnAddNew.FlatStyle = FlatStyle.Popup;
            btnAddNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAddNew.Location = new Point(1103, 80);
            btnAddNew.Name = "btnAddNew";
            btnAddNew.Size = new Size(75, 75);
            btnAddNew.TabIndex = 13;
            btnAddNew.Text = "+ جديد";
            btnAddNew.UseVisualStyleBackColor = true;
            btnAddNew.Click += btnAddNew_Click;
            // 
            // btnClose
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
            // frmListAccounts
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1200, 723);
            Controls.Add(btnClose);
            Controls.Add(btnAddNew);
            Controls.Add(lblRecordsCount);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(txtFilterValue);
            Controls.Add(cbFilterBy);
            Controls.Add(dgvAccounts);
            Controls.Add(lblMode);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmListAccounts";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "دليل الحسابات";
            Load += frmListAccounts_Load;
            cmsAccounts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsAccounts;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem showDetailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button btnClose;
    }
}
