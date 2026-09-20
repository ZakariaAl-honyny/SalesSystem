namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmListJournalEntries
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
            cmsJournalEntries = new ContextMenuStrip(components);
            refreshToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            showDetailsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            editToolStripMenuItem = new ToolStripMenuItem();
            addNewToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            postToolStripMenuItem = new ToolStripMenuItem();
            cancelToolStripMenuItem = new ToolStripMenuItem();
            lblMode = new Label();
            dgvJournalEntries = new DataGridView();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            label2 = new Label();
            lblRecordsCount = new Label();
            label1 = new Label();
            btnAddNew = new Button();
            btnClose = new Button();
            cmsJournalEntries.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvJournalEntries).BeginInit();
            SuspendLayout();

            // cmsJournalEntries
            cmsJournalEntries.BackColor = Color.White;
            cmsJournalEntries.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmsJournalEntries.ImageScalingSize = new Size(24, 24);
            cmsJournalEntries.Items.AddRange(new ToolStripItem[] {
                refreshToolStripMenuItem, toolStripSeparator1,
                showDetailsToolStripMenuItem, toolStripSeparator2,
                editToolStripMenuItem, addNewToolStripMenuItem, toolStripSeparator3,
                postToolStripMenuItem, cancelToolStripMenuItem });
            cmsJournalEntries.Name = "cmsJournalEntries";
            cmsJournalEntries.RightToLeft = RightToLeft.Yes;
            cmsJournalEntries.Size = new Size(200, 220);
            cmsJournalEntries.Opening += cmsJournalEntries_Opening;

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
            addNewToolStripMenuItem.Text = "&إضافة قيد جديد";
            addNewToolStripMenuItem.Click += addNewToolStripMenuItem_Click;

            // toolStripSeparator3
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(196, 6);

            // postToolStripMenuItem
            postToolStripMenuItem.Name = "postToolStripMenuItem";
            postToolStripMenuItem.Size = new Size(199, 30);
            postToolStripMenuItem.Text = "&ترحيل";
            postToolStripMenuItem.Click += postToolStripMenuItem_Click;

            // cancelToolStripMenuItem
            cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            cancelToolStripMenuItem.Size = new Size(199, 30);
            cancelToolStripMenuItem.Text = "&إلغاء";
            cancelToolStripMenuItem.Click += cancelToolStripMenuItem_Click;

            // lblMode
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(480, 20);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(240, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "القيود اليومية";
            // 
            // dgvJournalEntries
            dgvJournalEntries.AllowUserToAddRows = false;
            dgvJournalEntries.AllowUserToDeleteRows = false;
            dgvJournalEntries.AllowUserToOrderColumns = true;
            dgvJournalEntries.BackgroundColor = SystemColors.Window;
            dgvJournalEntries.BorderStyle = BorderStyle.Fixed3D;
            dgvJournalEntries.ColumnHeadersHeight = 50;
            dgvJournalEntries.ContextMenuStrip = cmsJournalEntries;
            dgvJournalEntries.Location = new Point(12, 160);
            dgvJournalEntries.MultiSelect = false;
            dgvJournalEntries.Name = "dgvJournalEntries";
            dgvJournalEntries.ReadOnly = true;
            dgvJournalEntries.RowHeadersWidth = 62;
            dgvJournalEntries.RowTemplate.Height = 28;
            dgvJournalEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJournalEntries.Size = new Size(1176, 480);
            dgvJournalEntries.TabIndex = 1;
            dgvJournalEntries.TabStop = false;
            dgvJournalEntries.DoubleClick += dgvJournalEntries_DoubleClick;
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
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم القيد", "البيان", "نوع القيد", "المرجع", "الحالة" });
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
            label1.Size = new Size(163, 24);
            label1.TabIndex = 11;
            label1.Text = "عدد القيود:";
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
            // frmListJournalEntries
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
            Controls.Add(dgvJournalEntries);
            Controls.Add(lblMode);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmListJournalEntries";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "القيود اليومية";
            Load += frmListJournalEntries_Load;
            cmsJournalEntries.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvJournalEntries).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsJournalEntries;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem showDetailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem postToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.DataGridView dgvJournalEntries;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button btnClose;
    }
}
