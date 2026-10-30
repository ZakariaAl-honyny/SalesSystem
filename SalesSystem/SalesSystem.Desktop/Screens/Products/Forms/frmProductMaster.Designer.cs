namespace SalesSystem.Desktop.Screens.Products.Forms;

partial class frmProductMaster
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblMode = new Label();
        pnlFields = new Panel();
        lblCode = new Label();
        txtCode = new TextBox();
        lblName = new Label();
        txtName = new TextBox();
        lblForeignName = new Label();
        txtForeignName = new TextBox();
        lblCategory = new Label();
        cmbCategory = new ComboBox();
        btnAddCategory = new Button();
        lblLocation = new Label();
        txtLocationInStore = new TextBox();
        lblReorderLevel = new Label();
        numReorderLevel = new NumericUpDown();
        chkTrackExpiry = new CheckBox();
        lblExpiryDays = new Label();
        numExpiryAlertDays = new NumericUpDown();
        grpMainUnit = new GroupBox();
        lblMainUnit = new Label();
        cmbMainUnit = new ComboBox();
        lblMainUnitCostPrice = new Label();
        numMainUnitCostPrice = new NumericUpDown();
        lblMainUnitSalePrice = new Label();
        numMainUnitSalePrice = new NumericUpDown();
        lblMainUnitBarcode = new Label();
        txtMainUnitBarcode = new TextBox();
        lblMainUnitMaxDiscount = new Label();
        numMainUnitMaxDiscount = new NumericUpDown();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnRefresh = new Button();
        btnSave = new Button();
        btnDelete = new Button();
        btnManageUnits = new Button();
        pnlGrid = new Panel();
        dgvProducts = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colCategoryName = new DataGridViewTextBoxColumn();
        colBarcode = new DataGridViewTextBoxColumn();
        colReorderLevel = new DataGridViewTextBoxColumn();
        colIsActive = new DataGridViewTextBoxColumn();
        pnlFilter = new Panel();
        txtSearch = new TextBox();
        lblSearch = new Label();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numReorderLevel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numExpiryAlertDays).BeginInit();
        grpMainUnit.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numMainUnitCostPrice).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numMainUnitSalePrice).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numMainUnitMaxDiscount).BeginInit();
        pnlButtons.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
        pnlFilter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1432, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "(( دليل الأصناف ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(lblCode);
        pnlFields.Controls.Add(txtCode);
        pnlFields.Controls.Add(lblName);
        pnlFields.Controls.Add(txtName);
        pnlFields.Controls.Add(lblForeignName);
        pnlFields.Controls.Add(txtForeignName);
        pnlFields.Controls.Add(lblCategory);
        pnlFields.Controls.Add(cmbCategory);
        pnlFields.Controls.Add(btnAddCategory);
        pnlFields.Controls.Add(lblLocation);
        pnlFields.Controls.Add(txtLocationInStore);
        pnlFields.Controls.Add(lblReorderLevel);
        pnlFields.Controls.Add(numReorderLevel);
        pnlFields.Controls.Add(chkTrackExpiry);
        pnlFields.Controls.Add(lblExpiryDays);
        pnlFields.Controls.Add(numExpiryAlertDays);
        pnlFields.Controls.Add(grpMainUnit);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1432, 200);
        pnlFields.TabIndex = 1;
        // 
        // lblCode
        // 
        lblCode.AutoSize = true;
        lblCode.Location = new Point(1267, 71);
        lblCode.Name = "lblCode";
        lblCode.Size = new Size(96, 24);
        lblCode.TabIndex = 0;
        lblCode.Text = "رقم الصنف";
        // 
        // txtCode
        // 
        txtCode.Location = new Point(1267, 98);
        txtCode.Name = "txtCode";
        txtCode.ReadOnly = true;
        txtCode.Size = new Size(110, 32);
        txtCode.TabIndex = 0;
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new Point(967, 3);
        lblName.Name = "lblName";
        lblName.Size = new Size(106, 24);
        lblName.TabIndex = 2;
        lblName.Text = "اسم الصنف";
        // 
        // txtName
        // 
        txtName.Location = new Point(911, 30);
        txtName.Name = "txtName";
        txtName.Size = new Size(240, 32);
        txtName.TabIndex = 1;
        // 
        // lblForeignName
        // 
        lblForeignName.AutoSize = true;
        lblForeignName.Location = new Point(694, 3);
        lblForeignName.Name = "lblForeignName";
        lblForeignName.Size = new Size(190, 24);
        lblForeignName.TabIndex = 4;
        lblForeignName.Text = "الاسم الأجنبي للصنف";
        // 
        // txtForeignName
        // 
        txtForeignName.Location = new Point(675, 30);
        txtForeignName.Name = "txtForeignName";
        txtForeignName.Size = new Size(230, 32);
        txtForeignName.TabIndex = 2;
        // 
        // lblCategory
        // 
        lblCategory.AutoSize = true;
        lblCategory.Location = new Point(1200, 3);
        lblCategory.Name = "lblCategory";
        lblCategory.Size = new Size(135, 24);
        lblCategory.TabIndex = 6;
        lblCategory.Text = "مجموعة الصنف";
        // 
        // cmbCategory
        // 
        cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategory.Location = new Point(1157, 30);
        cmbCategory.Name = "cmbCategory";
        cmbCategory.Size = new Size(220, 32);
        cmbCategory.TabIndex = 3;
        // 
        // btnAddCategory
        // 
        btnAddCategory.Location = new Point(1383, 31);
        btnAddCategory.Name = "btnAddCategory";
        btnAddCategory.Size = new Size(46, 31);
        btnAddCategory.TabIndex = 4;
        // 
        // lblLocation
        // 
        lblLocation.AutoSize = true;
        lblLocation.Location = new Point(1050, 134);
        lblLocation.Name = "lblLocation";
        lblLocation.Size = new Size(163, 24);
        lblLocation.TabIndex = 9;
        lblLocation.Text = "العنوان في المخزن";
        // 
        // txtLocationInStore
        // 
        txtLocationInStore.Location = new Point(989, 161);
        txtLocationInStore.Name = "txtLocationInStore";
        txtLocationInStore.Size = new Size(290, 32);
        txtLocationInStore.TabIndex = 5;
        // 
        // lblReorderLevel
        // 
        lblReorderLevel.AutoSize = true;
        lblReorderLevel.Location = new Point(1106, 71);
        lblReorderLevel.Name = "lblReorderLevel";
        lblReorderLevel.Size = new Size(155, 24);
        lblReorderLevel.TabIndex = 11;
        lblReorderLevel.Text = "نقطة اعادة الطلب";
        // 
        // numReorderLevel
        // 
        numReorderLevel.Location = new Point(1106, 99);
        numReorderLevel.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numReorderLevel.Name = "numReorderLevel";
        numReorderLevel.Size = new Size(155, 32);
        numReorderLevel.TabIndex = 6;
        // 
        // chkTrackExpiry
        // 
        chkTrackExpiry.AutoSize = true;
        chkTrackExpiry.Location = new Point(947, 102);
        chkTrackExpiry.Name = "chkTrackExpiry";
        chkTrackExpiry.Size = new Size(153, 28);
        chkTrackExpiry.TabIndex = 7;
        chkTrackExpiry.Text = "الصنف ينتهي؟";
        // 
        // lblExpiryDays
        // 
        lblExpiryDays.AutoSize = true;
        lblExpiryDays.Location = new Point(730, 74);
        lblExpiryDays.Name = "lblExpiryDays";
        lblExpiryDays.Size = new Size(175, 24);
        lblExpiryDays.TabIndex = 14;
        lblExpiryDays.Text = "نقطة التنبيه بالانتهاء";
        // 
        // numExpiryAlertDays
        // 
        numExpiryAlertDays.Location = new Point(730, 101);
        numExpiryAlertDays.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numExpiryAlertDays.Name = "numExpiryAlertDays";
        numExpiryAlertDays.Size = new Size(175, 32);
        numExpiryAlertDays.TabIndex = 8;
        // 
        // grpMainUnit
        // 
        grpMainUnit.BackColor = Color.FromArgb(200, 230, 200);
        grpMainUnit.Controls.Add(lblMainUnit);
        grpMainUnit.Controls.Add(cmbMainUnit);
        grpMainUnit.Controls.Add(lblMainUnitCostPrice);
        grpMainUnit.Controls.Add(numMainUnitCostPrice);
        grpMainUnit.Controls.Add(lblMainUnitSalePrice);
        grpMainUnit.Controls.Add(numMainUnitSalePrice);
        grpMainUnit.Controls.Add(lblMainUnitBarcode);
        grpMainUnit.Controls.Add(txtMainUnitBarcode);
        grpMainUnit.Controls.Add(lblMainUnitMaxDiscount);
        grpMainUnit.Controls.Add(numMainUnitMaxDiscount);
        grpMainUnit.Location = new Point(199, 3);
        grpMainUnit.Name = "grpMainUnit";
        grpMainUnit.Size = new Size(470, 170);
        grpMainUnit.TabIndex = 9;
        grpMainUnit.TabStop = false;
        grpMainUnit.Text = "الوحدة الرئيسية";
        // 
        // lblMainUnit
        // 
        lblMainUnit.AutoSize = true;
        lblMainUnit.Location = new Point(302, 27);
        lblMainUnit.Name = "lblMainUnit";
        lblMainUnit.Size = new Size(140, 24);
        lblMainUnit.TabIndex = 0;
        lblMainUnit.Text = "الوحدة الرئيسية";
        // 
        // cmbMainUnit
        // 
        cmbMainUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMainUnit.Location = new Point(302, 54);
        cmbMainUnit.Name = "cmbMainUnit";
        cmbMainUnit.Size = new Size(140, 32);
        cmbMainUnit.TabIndex = 0;
        // 
        // lblMainUnitCostPrice
        // 
        lblMainUnitCostPrice.AutoSize = true;
        lblMainUnitCostPrice.Location = new Point(185, 28);
        lblMainUnitCostPrice.Name = "lblMainUnitCostPrice";
        lblMainUnitCostPrice.Size = new Size(111, 24);
        lblMainUnitCostPrice.TabIndex = 2;
        lblMainUnitCostPrice.Text = "سعر التكلفة";
        // 
        // numMainUnitCostPrice
        // 
        numMainUnitCostPrice.Location = new Point(169, 54);
        numMainUnitCostPrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numMainUnitCostPrice.Name = "numMainUnitCostPrice";
        numMainUnitCostPrice.Size = new Size(127, 32);
        numMainUnitCostPrice.TabIndex = 1;
        // 
        // lblMainUnitSalePrice
        // 
        lblMainUnitSalePrice.AutoSize = true;
        lblMainUnitSalePrice.Location = new Point(52, 27);
        lblMainUnitSalePrice.Name = "lblMainUnitSalePrice";
        lblMainUnitSalePrice.Size = new Size(111, 24);
        lblMainUnitSalePrice.TabIndex = 4;
        lblMainUnitSalePrice.Text = "سعر التجزئة";
        // 
        // numMainUnitSalePrice
        // 
        numMainUnitSalePrice.Location = new Point(36, 54);
        numMainUnitSalePrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numMainUnitSalePrice.Name = "numMainUnitSalePrice";
        numMainUnitSalePrice.Size = new Size(127, 32);
        numMainUnitSalePrice.TabIndex = 2;
        // 
        // lblMainUnitBarcode
        // 
        lblMainUnitBarcode.AutoSize = true;
        lblMainUnitBarcode.Location = new Point(76, 98);
        lblMainUnitBarcode.Name = "lblMainUnitBarcode";
        lblMainUnitBarcode.Size = new Size(104, 24);
        lblMainUnitBarcode.TabIndex = 6;
        lblMainUnitBarcode.Text = "رقم الباركود";
        // 
        // txtMainUnitBarcode
        // 
        txtMainUnitBarcode.Location = new Point(36, 125);
        txtMainUnitBarcode.Name = "txtMainUnitBarcode";
        txtMainUnitBarcode.Size = new Size(180, 32);
        txtMainUnitBarcode.TabIndex = 3;
        // 
        // lblMainUnitMaxDiscount
        // 
        lblMainUnitMaxDiscount.AutoSize = true;
        lblMainUnitMaxDiscount.Location = new Point(246, 98);
        lblMainUnitMaxDiscount.Name = "lblMainUnitMaxDiscount";
        lblMainUnitMaxDiscount.Size = new Size(196, 24);
        lblMainUnitMaxDiscount.TabIndex = 8;
        lblMainUnitMaxDiscount.Text = "الخصم المسموح به %";
        // 
        // numMainUnitMaxDiscount
        // 
        numMainUnitMaxDiscount.Location = new Point(282, 125);
        numMainUnitMaxDiscount.Name = "numMainUnitMaxDiscount";
        numMainUnitMaxDiscount.Size = new Size(127, 32);
        numMainUnitMaxDiscount.TabIndex = 4;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnManageUnits);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 250);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1432, 60);
        pnlButtons.TabIndex = 2;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.White;
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Location = new Point(1267, 13);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(142, 35);
        btnNew.TabIndex = 0;
        btnNew.Text = "🆕 إضافة";
        btnNew.UseVisualStyleBackColor = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.White;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Location = new Point(1133, 13);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(128, 35);
        btnSave.TabIndex = 1;
        btnSave.Text = "💾 تعديل";
        btnSave.UseVisualStyleBackColor = false;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = Color.White;
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Location = new Point(989, 13);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(138, 35);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "🗑 حذف";
        btnDelete.UseVisualStyleBackColor = false;
        // 
        // btnManageUnits
        // 
        btnManageUnits.BackColor = Color.FromArgb(30, 86, 160);
        btnManageUnits.FlatStyle = FlatStyle.Flat;
        btnManageUnits.ForeColor = Color.White;
        btnManageUnits.Location = new Point(803, 13);
        btnManageUnits.Name = "btnManageUnits";
        btnManageUnits.Size = new Size(180, 35);
        btnManageUnits.TabIndex = 3;
        btnManageUnits.Text = "📦 الوحدات والأسعار";
        btnManageUnits.UseVisualStyleBackColor = false;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvProducts);
        pnlGrid.Controls.Add(pnlFilter);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 310);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1432, 360);
        pnlGrid.TabIndex = 3;
        // 
        // dgvProducts
        // 
        dgvProducts.AllowUserToAddRows = false;
        dgvProducts.AllowUserToDeleteRows = false;
        dgvProducts.AutoGenerateColumns = false;
        dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvProducts.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvProducts.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colCategoryName, colBarcode, colReorderLevel, colIsActive });
        dgvProducts.Dock = DockStyle.Fill;
        dgvProducts.Location = new Point(10, 50);
        dgvProducts.MultiSelect = false;
        dgvProducts.Name = "dgvProducts";
        dgvProducts.ReadOnly = true;
        dgvProducts.RowHeadersVisible = false;
        dgvProducts.RowHeadersWidth = 62;
        dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProducts.Size = new Size(1412, 300);
        dgvProducts.TabIndex = 0;
        // 
        // colId
        // 
        colId.DataPropertyName = "Id";
        colId.FillWeight = 7F;
        colId.HeaderText = "رقم الصنف";
        colId.MinimumWidth = 80;
        colId.Name = "colId";
        colId.ReadOnly = true;
        // 
        // colName
        // 
        colName.DataPropertyName = "Name";
        colName.FillWeight = 22F;
        colName.HeaderText = "اسم الصنف";
        colName.MinimumWidth = 200;
        colName.Name = "colName";
        colName.ReadOnly = true;
        // 
        // colCategoryName
        // 
        colCategoryName.DataPropertyName = "CategoryName";
        colCategoryName.FillWeight = 12F;
        colCategoryName.HeaderText = "المجموعة";
        colCategoryName.MinimumWidth = 120;
        colCategoryName.Name = "colCategoryName";
        colCategoryName.ReadOnly = true;
        // 
        // colBarcode
        // 
        colBarcode.DataPropertyName = "Barcode";
        colBarcode.FillWeight = 12F;
        colBarcode.HeaderText = "الباركود";
        colBarcode.MinimumWidth = 110;
        colBarcode.Name = "colBarcode";
        colBarcode.ReadOnly = true;
        // 
        // colReorderLevel
        // 
        colReorderLevel.DataPropertyName = "ReorderLevel";
        colReorderLevel.FillWeight = 10F;
        colReorderLevel.HeaderText = "نقطة إعادة الطلب";
        colReorderLevel.MinimumWidth = 110;
        colReorderLevel.Name = "colReorderLevel";
        colReorderLevel.ReadOnly = true;
        // 
        // colIsActive
        // 
        colIsActive.DataPropertyName = "IsActive";
        colIsActive.FillWeight = 6F;
        colIsActive.HeaderText = "نشط";
        colIsActive.MinimumWidth = 60;
        colIsActive.Name = "colIsActive";
        colIsActive.ReadOnly = true;
        // 
        // pnlFilter
        // 
        pnlFilter.Controls.Add(btnRefresh);
        pnlFilter.Controls.Add(txtSearch);
        pnlFilter.Controls.Add(lblSearch);
        pnlFilter.Dock = DockStyle.Top;
        pnlFilter.Location = new Point(10, 10);
        pnlFilter.Name = "pnlFilter";
        pnlFilter.Size = new Size(1412, 40);
        pnlFilter.TabIndex = 1;
        // 
        // txtSearch
        // 
        txtSearch.Location = new Point(690, 7);
        txtSearch.Name = "txtSearch";
        txtSearch.Size = new Size(300, 32);
        txtSearch.TabIndex = 99;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Location = new Point(550, 4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 32);
        btnRefresh.TabIndex = 98;
        btnRefresh.Text = "🔄 تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        // 
        // lblSearch
        // 
        lblSearch.AutoSize = true;
        lblSearch.Location = new Point(1000, 10);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(56, 24);
        lblSearch.TabIndex = 1;
        lblSearch.Text = "بحث:";
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmProductMaster
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(1432, 670);
        Controls.Add(pnlGrid);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        Name = "frmProductMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل الأصناف";
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numReorderLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)numExpiryAlertDays).EndInit();
        grpMainUnit.ResumeLayout(false);
        grpMainUnit.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numMainUnitCostPrice).EndInit();
        ((System.ComponentModel.ISupportInitialize)numMainUnitSalePrice).EndInit();
        ((System.ComponentModel.ISupportInitialize)numMainUnitMaxDiscount).EndInit();
        pnlButtons.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
        pnlFilter.ResumeLayout(false);
        pnlFilter.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.Label lblCode;
    private System.Windows.Forms.TextBox txtCode;
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.Label lblForeignName;
    private System.Windows.Forms.TextBox txtForeignName;
    private System.Windows.Forms.Label lblCategory;
    private System.Windows.Forms.ComboBox cmbCategory;
    private System.Windows.Forms.Button btnAddCategory;
    private System.Windows.Forms.Label lblLocation;
    private System.Windows.Forms.TextBox txtLocationInStore;
    private System.Windows.Forms.Label lblReorderLevel;
    private System.Windows.Forms.NumericUpDown numReorderLevel;
    private System.Windows.Forms.CheckBox chkTrackExpiry;
    private System.Windows.Forms.Label lblExpiryDays;
    private System.Windows.Forms.NumericUpDown numExpiryAlertDays;
    
    // Main Unit Group
    private System.Windows.Forms.GroupBox grpMainUnit;
    private System.Windows.Forms.Label lblMainUnit;
    private System.Windows.Forms.ComboBox cmbMainUnit;
    private System.Windows.Forms.Label lblMainUnitCostPrice;
    private System.Windows.Forms.NumericUpDown numMainUnitCostPrice;
    private System.Windows.Forms.Label lblMainUnitSalePrice;
    private System.Windows.Forms.NumericUpDown numMainUnitSalePrice;
    private System.Windows.Forms.Label lblMainUnitBarcode;
    private System.Windows.Forms.TextBox txtMainUnitBarcode;
    private System.Windows.Forms.Label lblMainUnitMaxDiscount;
    private System.Windows.Forms.NumericUpDown numMainUnitMaxDiscount;

    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnManageUnits;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvProducts;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCategoryName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colBarcode;
    private System.Windows.Forms.DataGridViewTextBoxColumn colReorderLevel;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
    private System.Windows.Forms.Panel pnlFilter;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.ErrorProvider errorProvider;
}

