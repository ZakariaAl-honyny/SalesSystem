namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmListInventoryCounts
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblMode = new Label();
            dgvCounts = new DataGridView();
            btnClose = new Button();
            btnAdd = new Button();
            btnView = new Button();
            btnPost = new Button();
            btnCancelCount = new Button();
            lblRecordsCount = new Label();
            label1 = new Label();
            label2 = new Label();
            txtFilterValue = new TextBox();
            btnSearch = new Button();
            btnRefresh = new Button();
            chkIncludeCancelled = new CheckBox();
            contextMenuStrip = new ContextMenuStrip(components);
            tsmiView = new ToolStripMenuItem();
            tsmiPost = new ToolStripMenuItem();
            tsmiCancel = new ToolStripMenuItem();

            ((System.ComponentModel.ISupportInitialize)dgvCounts).BeginInit();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();

            // lblMode
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(667, 15);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(220, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "جرد المخزون";

            // dgvCounts
            dgvCounts.AllowUserToAddRows = false;
            dgvCounts.AllowUserToDeleteRows = false;
            dgvCounts.AllowUserToOrderColumns = true;
            dgvCounts.BackgroundColor = SystemColors.Window;
            dgvCounts.BorderStyle = BorderStyle.Fixed3D;
            dgvCounts.ColumnHeadersHeight = 50;
            dgvCounts.Location = new Point(12, 160);
            dgvCounts.MultiSelect = false;
            dgvCounts.Name = "dgvCounts";
            dgvCounts.ReadOnly = true;
            dgvCounts.RowHeadersWidth = 62;
            dgvCounts.RowTemplate.Height = 28;
            dgvCounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCounts.Size = new Size(1540, 470);
            dgvCounts.TabIndex = 34;
            dgvCounts.TabStop = false;
            dgvCounts.CellDoubleClick += dgvCounts_CellDoubleClick;
            dgvCounts.MouseDown += dgvCounts_MouseDown;

            // contextMenuStrip
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { tsmiView, tsmiPost, tsmiCancel });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.RightToLeft = RightToLeft.Yes;
            contextMenuStrip.Size = new Size(180, 100);

            // tsmiView
            tsmiView.Name = "tsmiView"; tsmiView.Size = new Size(179, 30);
            tsmiView.Text = "عرض التفاصيل"; tsmiView.Click += tsmiView_Click;
            // tsmiPost
            tsmiPost.Name = "tsmiPost"; tsmiPost.Size = new Size(179, 30);
            tsmiPost.Text = "ترحيل"; tsmiPost.Click += tsmiPost_Click;
            // tsmiCancel
            tsmiCancel.Name = "tsmiCancel"; tsmiCancel.Size = new Size(179, 30);
            tsmiCancel.Text = "إلغاء"; tsmiCancel.Click += tsmiCancel_Click;

            // btnClose
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 710);
            btnClose.Name = "btnClose"; btnClose.Size = new Size(120, 50);
            btnClose.TabIndex = 0; btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false; btnClose.Click += btnClose_Click;

            // btnAdd
            btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAdd.BackColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAdd.Location = new Point(140, 710);
            btnAdd.Name = "btnAdd"; btnAdd.Size = new Size(120, 50);
            btnAdd.TabIndex = 1; btnAdd.Text = "جرد جديد";
            btnAdd.UseVisualStyleBackColor = false; btnAdd.Click += btnAdd_Click;

            // btnView
            btnView.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnView.BackColor = Color.White;
            btnView.FlatStyle = FlatStyle.Popup;
            btnView.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnView.Location = new Point(270, 710);
            btnView.Name = "btnView"; btnView.Size = new Size(120, 50);
            btnView.TabIndex = 2; btnView.Text = "عرض";
            btnView.UseVisualStyleBackColor = false; btnView.Click += btnView_Click;

            // btnPost
            btnPost.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPost.BackColor = Color.White;
            btnPost.FlatStyle = FlatStyle.Popup;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPost.Location = new Point(400, 710);
            btnPost.Name = "btnPost"; btnPost.Size = new Size(120, 50);
            btnPost.TabIndex = 3; btnPost.Text = "ترحيل";
            btnPost.UseVisualStyleBackColor = false; btnPost.Click += btnPost_Click;

            // btnCancelCount
            btnCancelCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancelCount.BackColor = Color.White;
            btnCancelCount.FlatStyle = FlatStyle.Popup;
            btnCancelCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnCancelCount.Location = new Point(530, 710);
            btnCancelCount.Name = "btnCancelCount"; btnCancelCount.Size = new Size(120, 50);
            btnCancelCount.TabIndex = 4; btnCancelCount.Text = "إلغاء الترحيل";
            btnCancelCount.UseVisualStyleBackColor = false; btnCancelCount.Click += btnCancel_Click;

            // lblRecordsCount
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.Location = new Point(1390, 725);
            lblRecordsCount.Name = "lblRecordsCount"; lblRecordsCount.Size = new Size(52, 24);
            lblRecordsCount.Text = "[0]";

            // label1
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(1250, 725);
            label1.Name = "label1"; label1.Size = new Size(130, 24);
            label1.Text = "عدد الجردات:";

            // label2
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(21, 115);
            label2.Name = "label2"; label2.Size = new Size(107, 24);
            label2.Text = "بحث عن:";

            // txtFilterValue
            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Location = new Point(145, 111);
            txtFilterValue.Name = "txtFilterValue"; txtFilterValue.Size = new Size(350, 32);
            txtFilterValue.TabIndex = 2; txtFilterValue.KeyPress += txtFilterValue_KeyPress;

            // btnSearch
            btnSearch.BackColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Popup;
            btnSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSearch.Location = new Point(505, 109);
            btnSearch.Name = "btnSearch"; btnSearch.Size = new Size(90, 36);
            btnSearch.TabIndex = 6; btnSearch.Text = "بحث";
            btnSearch.UseVisualStyleBackColor = true; btnSearch.Click += btnSearch_Click;

            // btnRefresh
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Popup;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(605, 109);
            btnRefresh.Name = "btnRefresh"; btnRefresh.Size = new Size(90, 36);
            btnRefresh.TabIndex = 7; btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = true; btnRefresh.Click += btnRefresh_Click;

            // chkIncludeCancelled
            chkIncludeCancelled.AutoSize = true;
            chkIncludeCancelled.Font = new Font("Tahoma", 10F);
            chkIncludeCancelled.Location = new Point(720, 112);
            chkIncludeCancelled.Name = "chkIncludeCancelled"; chkIncludeCancelled.Size = new Size(130, 28);
            chkIncludeCancelled.Text = "عرض الملغاة";
            chkIncludeCancelled.UseVisualStyleBackColor = true;
            chkIncludeCancelled.CheckedChanged += chkIncludeCancelled_CheckedChanged;

            // frmListInventoryCounts
            AcceptButton = btnSearch;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(chkIncludeCancelled);
            Controls.Add(btnRefresh); Controls.Add(btnSearch);
            Controls.Add(lblMode); Controls.Add(dgvCounts);
            Controls.Add(label2); Controls.Add(txtFilterValue);
            Controls.Add(label1); Controls.Add(lblRecordsCount);
            Controls.Add(btnCancelCount); Controls.Add(btnPost);
            Controls.Add(btnView); Controls.Add(btnAdd); Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListInventoryCounts";
            RightToLeft = RightToLeft.Yes; RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "جرد المخزون";
            Load += frmListInventoryCounts_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCounts).EndInit();
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #region Fields
        private Label lblMode;
        private DataGridView dgvCounts;
        private Button btnClose; private Button btnAdd; private Button btnView;
        private Button btnPost; private Button btnCancelCount;
        private Label lblRecordsCount; private Label label1; private Label label2;
        private TextBox txtFilterValue; private Button btnSearch; private Button btnRefresh;
        private CheckBox chkIncludeCancelled;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem tsmiView; private ToolStripMenuItem tsmiPost; private ToolStripMenuItem tsmiCancel;
        #endregion
    }
}
