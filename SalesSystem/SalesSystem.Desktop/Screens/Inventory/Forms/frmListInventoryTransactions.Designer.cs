namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmListInventoryTransactions
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
            dgvTransactions = new DataGridView();
            btnClose = new Button();
            lblRecordsCount = new Label();
            label1 = new Label();
            label2 = new Label();
            txtFilterValue = new TextBox();
            btnSearch = new Button();
            btnRefresh = new Button();
            contextMenuStrip = new ContextMenuStrip(components);
            tsmiView = new ToolStripMenuItem();

            ((System.ComponentModel.ISupportInitialize)dgvTransactions).BeginInit();
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
            lblMode.Name = "lblMode"; lblMode.Size = new Size(270, 39);
            lblMode.Text = "حركات المخزون";

            // dgvTransactions
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.AllowUserToOrderColumns = true;
            dgvTransactions.BackgroundColor = SystemColors.Window;
            dgvTransactions.BorderStyle = BorderStyle.Fixed3D;
            dgvTransactions.ColumnHeadersHeight = 50;
            dgvTransactions.Location = new Point(12, 160);
            dgvTransactions.MultiSelect = false;
            dgvTransactions.Name = "dgvTransactions"; dgvTransactions.ReadOnly = true;
            dgvTransactions.RowHeadersWidth = 62;
            dgvTransactions.RowTemplate.Height = 28;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.Size = new Size(1540, 530);
            dgvTransactions.TabStop = false;
            dgvTransactions.CellDoubleClick += dgvTransactions_CellDoubleClick;
            dgvTransactions.MouseDown += dgvTransactions_MouseDown;

            // contextMenuStrip
            contextMenuStrip.Items.Add(tsmiView);
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.RightToLeft = RightToLeft.Yes;
            contextMenuStrip.Size = new Size(180, 30);

            // tsmiView
            tsmiView.Name = "tsmiView"; tsmiView.Size = new Size(179, 30);
            tsmiView.Text = "عرض التفاصيل";
            tsmiView.Click += tsmiView_Click;

            // btnClose
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 710);
            btnClose.Name = "btnClose"; btnClose.Size = new Size(181, 61);
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

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
            label1.Location = new Point(1220, 725);
            label1.Name = "label1"; label1.Size = new Size(160, 24);
            label1.Text = "عدد الحركات:";

            // label2
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(21, 115);
            label2.Name = "label2"; label2.Size = new Size(107, 24);
            label2.Text = "بحث عن:";

            // txtFilterValue
            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Location = new Point(145, 111);
            txtFilterValue.Name = "txtFilterValue"; txtFilterValue.Size = new Size(400, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;

            // btnSearch
            btnSearch.BackColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Popup;
            btnSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSearch.Location = new Point(555, 109);
            btnSearch.Name = "btnSearch"; btnSearch.Size = new Size(100, 36);
            btnSearch.Text = "بحث";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;

            // btnRefresh
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Popup;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(665, 109);
            btnRefresh.Name = "btnRefresh"; btnRefresh.Size = new Size(100, 36);
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;

            // frmListInventoryTransactions
            AcceptButton = btnSearch;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(btnRefresh); Controls.Add(btnSearch);
            Controls.Add(lblMode); Controls.Add(dgvTransactions);
            Controls.Add(label2); Controls.Add(txtFilterValue);
            Controls.Add(label1); Controls.Add(lblRecordsCount);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListInventoryTransactions";
            RightToLeft = RightToLeft.Yes; RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "حركات المخزون";
            Load += frmListInventoryTransactions_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTransactions).EndInit();
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #region Fields
        private Label lblMode;
        private DataGridView dgvTransactions;
        private Button btnClose;
        private Label lblRecordsCount; private Label label1; private Label label2;
        private TextBox txtFilterValue; private Button btnSearch; private Button btnRefresh;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem tsmiView;
        #endregion
    }
}
