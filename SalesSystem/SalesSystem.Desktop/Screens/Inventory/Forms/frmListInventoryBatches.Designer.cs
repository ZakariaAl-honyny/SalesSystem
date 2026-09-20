namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmListInventoryBatches
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblMode = new Label();
            dgvBatches = new DataGridView();
            btnClose = new Button();
            lblRecordsCount = new Label();
            label1 = new Label();
            label2 = new Label();
            txtFilterValue = new TextBox();
            btnSearch = new Button();
            btnRefresh = new Button();
            lblProduct = new Label();
            cmbProduct = new ComboBox();

            ((System.ComponentModel.ISupportInitialize)dgvBatches).BeginInit();
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
            lblMode.Text = "دفعات المخزون";

            // dgvBatches
            dgvBatches.AllowUserToAddRows = false;
            dgvBatches.AllowUserToDeleteRows = false;
            dgvBatches.AllowUserToOrderColumns = true;
            dgvBatches.BackgroundColor = SystemColors.Window;
            dgvBatches.BorderStyle = BorderStyle.Fixed3D;
            dgvBatches.ColumnHeadersHeight = 50;
            dgvBatches.Location = new Point(12, 160);
            dgvBatches.MultiSelect = false;
            dgvBatches.Name = "dgvBatches"; dgvBatches.ReadOnly = true;
            dgvBatches.RowHeadersWidth = 62;
            dgvBatches.RowTemplate.Height = 28;
            dgvBatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBatches.Size = new Size(1540, 530);
            dgvBatches.TabStop = false;
            dgvBatches.CellFormatting += dgvBatches_CellFormatting;

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
            label1.Location = new Point(1240, 725);
            label1.Name = "label1"; label1.Size = new Size(140, 24);
            label1.Text = "عدد الدفعات:";

            // label2
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(21, 115);
            label2.Name = "label2"; label2.Size = new Size(107, 24);
            label2.Text = "بحث عن:";

            // txtFilterValue
            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Location = new Point(145, 111);
            txtFilterValue.Name = "txtFilterValue"; txtFilterValue.Size = new Size(300, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;

            // btnSearch
            btnSearch.BackColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Popup;
            btnSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSearch.Location = new Point(455, 109);
            btnSearch.Name = "btnSearch"; btnSearch.Size = new Size(90, 36);
            btnSearch.Text = "بحث";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;

            // btnRefresh
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Popup;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(555, 109);
            btnRefresh.Name = "btnRefresh"; btnRefresh.Size = new Size(90, 36);
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;

            // lblProduct
            lblProduct.AutoSize = true;
            lblProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblProduct.Location = new Point(670, 115);
            lblProduct.Name = "lblProduct"; lblProduct.Size = new Size(70, 24);
            lblProduct.Text = "المنتج:";

            // cmbProduct
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.Font = new Font("Tahoma", 10F);
            cmbProduct.Location = new Point(755, 111);
            cmbProduct.Name = "cmbProduct"; cmbProduct.Size = new Size(300, 32);
            cmbProduct.TabIndex = 8;
            cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;

            // frmListInventoryBatches
            AcceptButton = btnSearch;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(cmbProduct); Controls.Add(lblProduct);
            Controls.Add(btnRefresh); Controls.Add(btnSearch);
            Controls.Add(lblMode); Controls.Add(dgvBatches);
            Controls.Add(label2); Controls.Add(txtFilterValue);
            Controls.Add(label1); Controls.Add(lblRecordsCount);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListInventoryBatches";
            RightToLeft = RightToLeft.Yes; RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "دفعات المخزون";
            Load += frmListInventoryBatches_Load;
            ((System.ComponentModel.ISupportInitialize)dgvBatches).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #region Fields
        private Label lblMode;
        private DataGridView dgvBatches;
        private Button btnClose;
        private Label lblRecordsCount; private Label label1; private Label label2;
        private TextBox txtFilterValue; private Button btnSearch; private Button btnRefresh;
        private Label lblProduct; private ComboBox cmbProduct;
        #endregion
    }
}
