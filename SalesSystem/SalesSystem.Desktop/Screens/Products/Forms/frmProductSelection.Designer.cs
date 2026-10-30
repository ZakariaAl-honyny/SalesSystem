namespace SalesSystem.Desktop.Products.Forms
{
    partial class frmProductSelection
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _scope?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            tooltip = new ToolTip(components);
            dgvProducts = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colBarcode = new DataGridViewTextBoxColumn();
            btnSelect = new Button();
            btnCancel = new Button();
            txtSearch = new TextBox();
            lblSearch = new Label();
            lblMode = new Label();
            lblRecordsCount = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.BorderStyle = BorderStyle.Fixed3D;
            dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvProducts.ColumnHeadersHeight = 45;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvProducts.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colCategory, colBarcode });
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.GridColor = Color.FromArgb(222, 226, 230);
            dgvProducts.Location = new Point(12, 109);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.RowHeadersWidth = 62;
            dgvProducts.RowTemplate.Height = 32;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(886, 442);
            dgvProducts.TabIndex = 1;
            dgvProducts.TabStop = false;
            tooltip.SetToolTip(dgvProducts, "انقر مرتين لاختيار المنتج");
            dgvProducts.DoubleClick += dgvProducts_DoubleClick;
            // 
            // colId
            // 
            colId.HeaderText = "الرقم";
            colId.MinimumWidth = 8;
            colId.Name = "colId";
            colId.ReadOnly = true;
            // 
            // colName
            // 
            colName.HeaderText = "اسم المنتج";
            colName.MinimumWidth = 8;
            colName.Name = "colName";
            colName.ReadOnly = true;
            // 
            // colCategory
            // 
            colCategory.HeaderText = "التصنيف";
            colCategory.MinimumWidth = 8;
            colCategory.Name = "colCategory";
            colCategory.ReadOnly = true;
            // 
            // colBarcode
            // 
            colBarcode.HeaderText = "الباركود";
            colBarcode.MinimumWidth = 8;
            colBarcode.Name = "colBarcode";
            colBarcode.ReadOnly = true;
            // 
            // btnSelect
            // 
            btnSelect.BackColor = Color.FromArgb(25, 135, 84);
            btnSelect.Cursor = Cursors.Hand;
            btnSelect.FlatAppearance.BorderSize = 0;
            btnSelect.FlatStyle = FlatStyle.Flat;
            btnSelect.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSelect.ForeColor = Color.White;
            btnSelect.Location = new Point(12, 560);
            btnSelect.MinimumSize = new Size(100, 35);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(118, 40);
            btnSelect.TabIndex = 2;
            btnSelect.Text = "تحديد";
            tooltip.SetToolTip(btnSelect, "اختيار المنتج المحدد");
            btnSelect.UseVisualStyleBackColor = false;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(144, 560);
            btnCancel.MinimumSize = new Size(100, 35);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(118, 40);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "إلغاء";
            tooltip.SetToolTip(btnCancel, "إلغاء و إغلاق النافذة");
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Tahoma", 10F);
            txtSearch.Location = new Point(130, 65);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(400, 32);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyPress += txtSearch_KeyPress;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSearch.Location = new Point(12, 68);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(60, 24);
            lblSearch.TabIndex = 4;
            lblSearch.Text = "بحث:";
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.Dock = DockStyle.Top;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(910, 40);
            lblMode.TabIndex = 30;
            lblMode.Text = "اختيار منتج";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.Location = new Point(800, 68);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(41, 24);
            lblRecordsCount.TabIndex = 5;
            lblRecordsCount.Text = "[0]";
            // 
            // frmProductSelection
            // 
            AcceptButton = btnSelect;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(910, 610);
            Controls.Add(lblRecordsCount);
            Controls.Add(lblMode);
            Controls.Add(lblSearch);
            Controls.Add(btnCancel);
            Controls.Add(btnSelect);
            Controls.Add(dgvProducts);
            Controls.Add(txtSearch);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmProductSelection";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "اختيار منتج";
            Load += frmProductSelection_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewTextBoxColumn colBarcode;
        private Label lblSearch;
        private Label lblMode;
        private Label lblRecordsCount;
        private ToolTip tooltip;
    }
}
