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
            // RULE-N134: ألوان المصمّم = ثوابت GreenThemeHelper حرفياً (المصمّم = وقت التشغيل).
            tooltip = new ToolTip(components);
            dgvProducts = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colBarcode = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            dgvUnits = new DataGridView();
            colUnitId = new DataGridViewTextBoxColumn();
            colUnitName = new DataGridViewTextBoxColumn();
            colConversion = new DataGridViewTextBoxColumn();
            colAvailable = new DataGridViewTextBoxColumn();
            colSalePrice = new DataGridViewTextBoxColumn();
            lblSelectedProduct = new Label();
            lblNoUnits = new Label();
            btnSelect = new Button();
            btnCancel = new Button();
            txtSearch = new TextBox();
            lblSearch = new Label();
            lblMode = new Label();
            lblRecordsCount = new Label();
            lblNoData = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
            SuspendLayout();
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AllowUserToResizeRows = false;
            dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dgvProducts.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvProducts.ColumnHeadersHeight = 45;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvProducts.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colCategory, colBarcode, colStock });
            dgvProducts.DefaultCellStyle.BackColor = Color.White;
            dgvProducts.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgvProducts.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.GridColor = Color.FromArgb(205, 225, 205);
            dgvProducts.Location = new Point(12, 96);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.RowHeadersWidth = 62;
            dgvProducts.RowTemplate.Height = 32;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(886, 230);
            dgvProducts.TabIndex = 1;
            dgvProducts.TabStop = false;
            tooltip.SetToolTip(dgvProducts, "انقر مرتين (أو Enter) للانتقال إلى وحدات المنتج");
            dgvProducts.SelectionChanged += dgvProducts_SelectionChanged;
            dgvProducts.DoubleClick += dgvProducts_DoubleClick;
            dgvProducts.KeyDown += dgvProducts_KeyDown;
            // 
            // colId
            // 
            colId.DataPropertyName = "Id";
            colId.HeaderText = "الرقم";
            colId.MinimumWidth = 8;
            colId.Name = "colId";
            colId.ReadOnly = true;
            // 
            // colName
            // 
            colName.DataPropertyName = "Name";
            colName.HeaderText = "اسم المنتج";
            colName.MinimumWidth = 8;
            colName.Name = "colName";
            colName.ReadOnly = true;
            // 
            // colCategory
            // 
            colCategory.DataPropertyName = "CategoryName";
            colCategory.HeaderText = "التصنيف";
            colCategory.MinimumWidth = 8;
            colCategory.Name = "colCategory";
            colCategory.ReadOnly = true;
            // 
            // colBarcode
            // 
            colBarcode.DataPropertyName = "Barcode";
            colBarcode.HeaderText = "الباركود";
            colBarcode.MinimumWidth = 8;
            colBarcode.Name = "colBarcode";
            colBarcode.ReadOnly = true;
            // 
            // colStock
            // 
            colStock.DataPropertyName = "Available";
            colStock.HeaderText = "المتاح (وحدة أساسية)";
            colStock.MinimumWidth = 8;
            colStock.Name = "colStock";
            colStock.ReadOnly = true;
            colStock.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvUnits
            // 
            dgvUnits.AllowUserToAddRows = false;
            dgvUnits.AllowUserToDeleteRows = false;
            dgvUnits.AllowUserToResizeRows = false;
            dgvUnits.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
            dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnits.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvUnits.BorderStyle = BorderStyle.None;
            dgvUnits.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUnits.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvUnits.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
            dgvUnits.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgvUnits.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUnits.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dgvUnits.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvUnits.ColumnHeadersHeight = 45;
            dgvUnits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUnits.Columns.AddRange(new DataGridViewColumn[] { colUnitId, colUnitName, colConversion, colAvailable, colSalePrice });
            dgvUnits.DefaultCellStyle.BackColor = Color.White;
            dgvUnits.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgvUnits.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvUnits.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dgvUnits.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvUnits.EnableHeadersVisualStyles = false;
            dgvUnits.GridColor = Color.FromArgb(205, 225, 205);
            dgvUnits.Location = new Point(12, 364);
            dgvUnits.MultiSelect = false;
            dgvUnits.Name = "dgvUnits";
            dgvUnits.ReadOnly = true;
            dgvUnits.RowHeadersVisible = false;
            dgvUnits.RowHeadersWidth = 62;
            dgvUnits.RowTemplate.Height = 32;
            dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnits.Size = new Size(886, 240);
            dgvUnits.TabIndex = 6;
            tooltip.SetToolTip(dgvUnits, "انقر مرتين (أو Enter) لاختيار الوحدة");
            dgvUnits.DoubleClick += dgvUnits_DoubleClick;
            dgvUnits.KeyDown += dgvUnits_KeyDown;
            // 
            // colUnitId
            // 
            colUnitId.DataPropertyName = "UnitId";
            colUnitId.HeaderText = "معرف الوحدة";
            colUnitId.MinimumWidth = 8;
            colUnitId.Name = "colUnitId";
            colUnitId.ReadOnly = true;
            colUnitId.Visible = false;
            // 
            // colUnitName
            // 
            colUnitName.DataPropertyName = "UnitName";
            colUnitName.FillWeight = 60F;
            colUnitName.HeaderText = "الوحدة";
            colUnitName.MinimumWidth = 8;
            colUnitName.Name = "colUnitName";
            colUnitName.ReadOnly = true;
            // 
            // colConversion
            // 
            colConversion.DataPropertyName = "ConversionFactor";
            colConversion.FillWeight = 40F;
            colConversion.HeaderText = "معامل التحويل";
            colConversion.MinimumWidth = 8;
            colConversion.Name = "colConversion";
            colConversion.ReadOnly = true;
            colConversion.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colAvailable
            // 
            colAvailable.DataPropertyName = "Available";
            colAvailable.FillWeight = 45F;
            colAvailable.HeaderText = "الكمية المتاحة";
            colAvailable.MinimumWidth = 8;
            colAvailable.Name = "colAvailable";
            colAvailable.ReadOnly = true;
            colAvailable.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colSalePrice
            // 
            colSalePrice.DataPropertyName = "SalePrice";
            colSalePrice.FillWeight = 45F;
            colSalePrice.HeaderText = "سعر البيع";
            colSalePrice.MinimumWidth = 8;
            colSalePrice.Name = "colSalePrice";
            colSalePrice.ReadOnly = true;
            colSalePrice.DefaultCellStyle.Format = "N2";
            // 
            // lblSelectedProduct
            // 
            lblSelectedProduct.AutoEllipsis = true;
            lblSelectedProduct.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblSelectedProduct.ForeColor = Color.FromArgb(14, 82, 52);
            lblSelectedProduct.Location = new Point(12, 332);
            lblSelectedProduct.Name = "lblSelectedProduct";
            lblSelectedProduct.Size = new Size(640, 26);
            lblSelectedProduct.TabIndex = 7;
            lblSelectedProduct.Text = "المنتج المحدد: —";
            lblSelectedProduct.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNoUnits
            // 
            lblNoUnits.AutoSize = true;
            lblNoUnits.Font = new Font("Tahoma", 10F);
            lblNoUnits.ForeColor = Color.FromArgb(14, 82, 52);
            lblNoUnits.Location = new Point(660, 335);
            lblNoUnits.Name = "lblNoUnits";
            lblNoUnits.Size = new Size(196, 24);
            lblNoUnits.TabIndex = 8;
            lblNoUnits.Text = "لا توجد وحدات لهذا الصنف";
            lblNoUnits.Visible = false;
            // 
            // btnSelect
            // 
            btnSelect.BackColor = Color.FromArgb(40, 167, 69);
            btnSelect.Cursor = Cursors.Hand;
            btnSelect.FlatAppearance.BorderSize = 0;
            btnSelect.FlatStyle = FlatStyle.Flat;
            btnSelect.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSelect.ForeColor = Color.White;
            btnSelect.Location = new Point(12, 616);
            btnSelect.MinimumSize = new Size(100, 35);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(118, 40);
            btnSelect.TabIndex = 2;
            btnSelect.Text = "اختيار";
            tooltip.SetToolTip(btnSelect, "اختيار المنتج والوحدة المحددة (Enter)");
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
            btnCancel.Location = new Point(144, 616);
            btnCancel.MinimumSize = new Size(100, 35);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(118, 40);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "إلغاء";
            tooltip.SetToolTip(btnCancel, "إلغاء و إغلاق النافذة (Esc)");
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.White;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Tahoma", 10F);
            txtSearch.Location = new Point(130, 52);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(400, 32);
            txtSearch.TabIndex = 0;
            tooltip.SetToolTip(txtSearch, "بحث فوري بالاسم أو الباركود أو التصنيف — F5 لتحديث القائمة");
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
            txtSearch.KeyPress += txtSearch_KeyPress;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSearch.ForeColor = Color.FromArgb(14, 82, 52);
            lblSearch.Location = new Point(12, 55);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(60, 24);
            lblSearch.TabIndex = 4;
            lblSearch.Text = "بحث:";
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(14, 165, 103);
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
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
            lblRecordsCount.ForeColor = Color.FromArgb(14, 82, 52);
            lblRecordsCount.Location = new Point(800, 55);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(41, 24);
            lblRecordsCount.TabIndex = 5;
            lblRecordsCount.Text = "[0]";
            // 
            // lblNoData
            // 
            lblNoData.AutoSize = true;
            lblNoData.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblNoData.ForeColor = Color.FromArgb(14, 82, 52);
            lblNoData.Location = new Point(620, 55);
            lblNoData.Name = "lblNoData";
            lblNoData.Size = new Size(130, 24);
            lblNoData.TabIndex = 9;
            lblNoData.Text = "لا توجد بيانات";
            lblNoData.Visible = false;
            // 
            // frmProductSelection
            // 
            AcceptButton = btnSelect;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            CancelButton = btnCancel;
            ClientSize = new Size(910, 668);
            Controls.Add(lblNoData);
            Controls.Add(lblNoUnits);
            Controls.Add(lblSelectedProduct);
            Controls.Add(dgvUnits);
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
            ((System.ComponentModel.ISupportInitialize)dgvUnits).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.DataGridView dgvUnits;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewTextBoxColumn colBarcode;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colUnitId;
        private DataGridViewTextBoxColumn colUnitName;
        private DataGridViewTextBoxColumn colConversion;
        private DataGridViewTextBoxColumn colAvailable;
        private DataGridViewTextBoxColumn colSalePrice;
        private Label lblSelectedProduct;
        private Label lblNoUnits;
        private Label lblSearch;
        private Label lblMode;
        private Label lblRecordsCount;
        private Label lblNoData;
        private ToolTip tooltip;
    }
}
