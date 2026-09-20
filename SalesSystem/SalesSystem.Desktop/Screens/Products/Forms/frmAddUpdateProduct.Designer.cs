namespace SalesSystem.Desktop.Screens.Products.Forms;

partial class frmAddUpdateProduct
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            _catSubscription?.Dispose();
            _unitSubscription?.Dispose();
            errorProvider?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        errorProvider = new ErrorProvider(components);
        tooltip = new ToolTip(components);
        txtCode = new TextBox();
        txtBarcode = new TextBox();
        txtName = new TextBox();
        cmbCategory = new ComboBox();
        cmbUnit = new ComboBox();
        numReorderLevel = new NumericUpDown();
        chkTrackExpiry = new CheckBox();
        txtDescription = new TextBox();
        lblMode = new Label();
        tabControl = new TabControl();
        tabGeneral = new TabPage();
        genLayout = new TableLayoutPanel();
        lblCode = new Label();
        lblBarcode = new Label();
        lblName = new Label();
        lblCategory = new Label();
        catPanel = new TableLayoutPanel();
        btnAddCategory = new Button();
        lblUnit = new Label();
        baseUnitPanel = new TableLayoutPanel();
        btnAddUnit = new Button();
        lblReorder = new Label();
        lblOptions = new Label();
        lblDesc = new Label();
        tabPrices = new TabPage();
        pricesLayout = new TableLayoutPanel();
        lblPricesHeader = new Label();
        pricesToolbar = new FlowLayoutPanel();
        btnOpenPricesScreen = new Button();
        dgvPrices = new DataGridView();
        lblPricesInfo = new Label();
        tabUnits = new TabPage();
        unitsLayout = new TableLayoutPanel();
        lblUnitsHeader = new Label();
        unitsToolbar = new FlowLayoutPanel();
        btnOpenUnitsScreen = new Button();
        dgvUnits = new DataGridView();
        lblUnitsInfo = new Label();
        tabStock = new TabPage();
        stockLayout = new TableLayoutPanel();
        stockToolbar = new FlowLayoutPanel();
        btnRefreshBatches = new Button();
        btnViewStock = new Button();
        dgvBatches = new DataGridView();
        lblStockInfo = new Label();
        tabBom = new TabPage();
        lblBomPlaceholder = new Label();
        buttonPanel = new FlowLayoutPanel();
        btnSave = new Button();
        btnCancel = new Button();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numReorderLevel).BeginInit();
        tabControl.SuspendLayout();
        tabGeneral.SuspendLayout();
        genLayout.SuspendLayout();
        catPanel.SuspendLayout();
        baseUnitPanel.SuspendLayout();
        tabPrices.SuspendLayout();
        pricesLayout.SuspendLayout();
        pricesToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPrices).BeginInit();
        tabUnits.SuspendLayout();
        unitsLayout.SuspendLayout();
        unitsToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
        tabStock.SuspendLayout();
        stockLayout.SuspendLayout();
        stockToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBatches).BeginInit();
        tabBom.SuspendLayout();
        buttonPanel.SuspendLayout();
        SuspendLayout();
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        // 
        // txtCode
        // 
        txtCode.BackColor = Color.LightGray;
        txtCode.Dock = DockStyle.Fill;
        txtCode.Location = new Point(13, 13);
        txtCode.Name = "txtCode";
        txtCode.ReadOnly = true;
        txtCode.Size = new Size(740, 34);
        txtCode.TabIndex = 1;
        tooltip.SetToolTip(txtCode, "الكود التلقائي للمنتج (يتم إنشاؤه تلقائياً)");
        // 
        // txtBarcode
        // 
        txtBarcode.Dock = DockStyle.Fill;
        txtBarcode.Location = new Point(13, 51);
        txtBarcode.Name = "txtBarcode";
        txtBarcode.Size = new Size(740, 34);
        txtBarcode.TabIndex = 3;
        tooltip.SetToolTip(txtBarcode, "الباركود الخاص بالمنتج (اختياري)");
        // 
        // txtName
        // 
        txtName.Dock = DockStyle.Fill;
        txtName.Location = new Point(13, 89);
        txtName.Name = "txtName";
        txtName.Size = new Size(740, 34);
        txtName.TabIndex = 5;
        tooltip.SetToolTip(txtName, "اسم المنتج كما يظهر في الفواتير");
        // 
        // cmbCategory
        // 
        cmbCategory.Dock = DockStyle.Fill;
        cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategory.Location = new Point(48, 3);
        cmbCategory.Name = "cmbCategory";
        cmbCategory.Size = new Size(689, 35);
        cmbCategory.TabIndex = 0;
        tooltip.SetToolTip(cmbCategory, "اختر التصنيف المناسب للمنتج");
        // 
        // cmbUnit
        // 
        cmbUnit.Dock = DockStyle.Fill;
        cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbUnit.Location = new Point(48, 3);
        cmbUnit.Name = "cmbUnit";
        cmbUnit.Size = new Size(689, 35);
        cmbUnit.TabIndex = 0;
        tooltip.SetToolTip(cmbUnit, "الوحدة الأساسية هي أصغر وحدة قياس للمنتج\nمثال: حبة، قطعة، كيلو جرام");
        // 
        // numReorderLevel
        // 
        numReorderLevel.DecimalPlaces = 3;
        numReorderLevel.Dock = DockStyle.Fill;
        numReorderLevel.Location = new Point(13, 203);
        numReorderLevel.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numReorderLevel.Name = "numReorderLevel";
        numReorderLevel.Size = new Size(740, 34);
        numReorderLevel.TabIndex = 11;
        tooltip.SetToolTip(numReorderLevel, "عند وصول المخزون لهذا الحد يتم تنبيهك لإعادة الطلب");
        // 
        // chkTrackExpiry
        // 
        chkTrackExpiry.Dock = DockStyle.Fill;
        chkTrackExpiry.Font = new Font("Tahoma", 11F);
        chkTrackExpiry.Location = new Point(13, 241);
        chkTrackExpiry.Name = "chkTrackExpiry";
        chkTrackExpiry.Size = new Size(740, 32);
        chkTrackExpiry.TabIndex = 13;
        chkTrackExpiry.Text = "تتبع تاريخ الصلاحية";
        chkTrackExpiry.TextAlign = ContentAlignment.MiddleRight;
        tooltip.SetToolTip(chkTrackExpiry, "فعّل إذا كان للمنتج تاريخ صلاحية");
        // 
        // txtDescription
        // 
        txtDescription.Dock = DockStyle.Fill;
        txtDescription.Location = new Point(13, 279);
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        genLayout.SetRowSpan(txtDescription, 2);
        txtDescription.ScrollBars = ScrollBars.Vertical;
        txtDescription.Size = new Size(740, 218);
        txtDescription.TabIndex = 15;
        tooltip.SetToolTip(txtDescription, "وصف اختياري للمنتج");
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(278, 10);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(263, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "إضافة منتج جديد";
        // 
        // tabControl
        // 
        tabControl.Controls.Add(tabGeneral);
        tabControl.Controls.Add(tabPrices);
        tabControl.Controls.Add(tabUnits);
        tabControl.Controls.Add(tabStock);
        tabControl.Controls.Add(tabBom);
        tabControl.Dock = DockStyle.Fill;
        tabControl.Font = new Font("Tahoma", 11F);
        tabControl.Location = new Point(0, 0);
        tabControl.Name = "tabControl";
        tabControl.Padding = new Point(10, 6);
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(924, 576);
        tabControl.TabIndex = 0;
        // 
        // tabGeneral
        // 
        tabGeneral.BackColor = Color.White;
        tabGeneral.Controls.Add(genLayout);
        tabGeneral.Location = new Point(4, 42);
        tabGeneral.Name = "tabGeneral";
        tabGeneral.Padding = new Padding(10);
        tabGeneral.Size = new Size(916, 530);
        tabGeneral.TabIndex = 0;
        tabGeneral.Text = "عام";
        // 
        // genLayout
        // 
        genLayout.ColumnCount = 2;
        genLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        genLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        genLayout.Controls.Add(lblCode, 0, 0);
        genLayout.Controls.Add(txtCode, 1, 0);
        genLayout.Controls.Add(lblBarcode, 0, 1);
        genLayout.Controls.Add(txtBarcode, 1, 1);
        genLayout.Controls.Add(lblName, 0, 2);
        genLayout.Controls.Add(txtName, 1, 2);
        genLayout.Controls.Add(lblCategory, 0, 3);
        genLayout.Controls.Add(catPanel, 1, 3);
        genLayout.Controls.Add(lblUnit, 0, 4);
        genLayout.Controls.Add(baseUnitPanel, 1, 4);
        genLayout.Controls.Add(lblReorder, 0, 5);
        genLayout.Controls.Add(numReorderLevel, 1, 5);
        genLayout.Controls.Add(lblOptions, 0, 6);
        genLayout.Controls.Add(chkTrackExpiry, 1, 6);
        genLayout.Controls.Add(lblDesc, 0, 7);
        genLayout.Controls.Add(txtDescription, 1, 7);
        genLayout.Dock = DockStyle.Fill;
        genLayout.Location = new Point(10, 10);
        genLayout.Name = "genLayout";
        genLayout.Padding = new Padding(10);
        genLayout.RowCount = 9;
        genLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        genLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        genLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        genLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        genLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        genLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        genLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        genLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        genLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        genLayout.Size = new Size(896, 510);
        genLayout.TabIndex = 0;
        // 
        // lblCode
        // 
        lblCode.Dock = DockStyle.Fill;
        lblCode.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblCode.Location = new Point(759, 10);
        lblCode.Name = "lblCode";
        lblCode.Size = new Size(124, 38);
        lblCode.TabIndex = 0;
        lblCode.Text = "الكود:";
        lblCode.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblBarcode
        // 
        lblBarcode.Dock = DockStyle.Fill;
        lblBarcode.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblBarcode.Location = new Point(759, 48);
        lblBarcode.Name = "lblBarcode";
        lblBarcode.Size = new Size(124, 38);
        lblBarcode.TabIndex = 2;
        lblBarcode.Text = "الباركود:";
        lblBarcode.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblName
        // 
        lblName.Dock = DockStyle.Fill;
        lblName.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblName.Location = new Point(759, 86);
        lblName.Name = "lblName";
        lblName.Size = new Size(124, 38);
        lblName.TabIndex = 4;
        lblName.Text = "اسم المنتج *:";
        lblName.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblCategory
        // 
        lblCategory.Dock = DockStyle.Fill;
        lblCategory.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblCategory.Location = new Point(759, 124);
        lblCategory.Name = "lblCategory";
        lblCategory.Size = new Size(124, 38);
        lblCategory.TabIndex = 6;
        lblCategory.Text = "التصنيف *:";
        lblCategory.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // catPanel
        // 
        catPanel.ColumnCount = 2;
        catPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        catPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));
        catPanel.Controls.Add(cmbCategory, 0, 0);
        catPanel.Controls.Add(btnAddCategory, 1, 0);
        catPanel.Dock = DockStyle.Fill;
        catPanel.Location = new Point(13, 127);
        catPanel.Name = "catPanel";
        catPanel.RowCount = 1;
        catPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        catPanel.Size = new Size(740, 32);
        catPanel.TabIndex = 7;
        // 
        // btnAddCategory
        // 
        btnAddCategory.BackColor = Color.FromArgb(13, 110, 253);
        btnAddCategory.Cursor = Cursors.Hand;
        btnAddCategory.Dock = DockStyle.Fill;
        btnAddCategory.FlatAppearance.BorderSize = 0;
        btnAddCategory.FlatStyle = FlatStyle.Flat;
        btnAddCategory.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnAddCategory.ForeColor = Color.White;
        btnAddCategory.Location = new Point(3, 3);
        btnAddCategory.MinimumSize = new Size(100, 35);
        btnAddCategory.Name = "btnAddCategory";
        btnAddCategory.Padding = new Padding(12, 0, 12, 0);
        btnAddCategory.Size = new Size(100, 35);
        btnAddCategory.TabIndex = 1;
        btnAddCategory.Text = "+";
        btnAddCategory.UseVisualStyleBackColor = false;
        btnAddCategory.Click += btnAddCategory_Click;
        // 
        // lblUnit
        // 
        lblUnit.Dock = DockStyle.Fill;
        lblUnit.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblUnit.Location = new Point(759, 162);
        lblUnit.Name = "lblUnit";
        lblUnit.Size = new Size(124, 38);
        lblUnit.TabIndex = 8;
        lblUnit.Text = "الوحدة الأساسية (أصغر وحدة):";
        lblUnit.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // baseUnitPanel
        // 
        baseUnitPanel.ColumnCount = 2;
        baseUnitPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        baseUnitPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));
        baseUnitPanel.Controls.Add(cmbUnit, 0, 0);
        baseUnitPanel.Controls.Add(btnAddUnit, 1, 0);
        baseUnitPanel.Dock = DockStyle.Fill;
        baseUnitPanel.Location = new Point(13, 165);
        baseUnitPanel.Name = "baseUnitPanel";
        baseUnitPanel.RowCount = 1;
        baseUnitPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        baseUnitPanel.Size = new Size(740, 32);
        baseUnitPanel.TabIndex = 9;
        // 
        // btnAddUnit
        // 
        btnAddUnit.BackColor = Color.FromArgb(13, 110, 253);
        btnAddUnit.Cursor = Cursors.Hand;
        btnAddUnit.Dock = DockStyle.Fill;
        btnAddUnit.FlatAppearance.BorderSize = 0;
        btnAddUnit.FlatStyle = FlatStyle.Flat;
        btnAddUnit.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnAddUnit.ForeColor = Color.White;
        btnAddUnit.Location = new Point(3, 3);
        btnAddUnit.MinimumSize = new Size(100, 35);
        btnAddUnit.Name = "btnAddUnit";
        btnAddUnit.Padding = new Padding(12, 0, 12, 0);
        btnAddUnit.Size = new Size(100, 35);
        btnAddUnit.TabIndex = 1;
        btnAddUnit.Text = "+";
        btnAddUnit.UseVisualStyleBackColor = false;
        btnAddUnit.Click += btnAddUnit_Click;
        // 
        // lblReorder
        // 
        lblReorder.Dock = DockStyle.Fill;
        lblReorder.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblReorder.Location = new Point(759, 200);
        lblReorder.Name = "lblReorder";
        lblReorder.Size = new Size(124, 38);
        lblReorder.TabIndex = 10;
        lblReorder.Text = "حد إعادة الطلب:";
        lblReorder.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblOptions
        // 
        lblOptions.Dock = DockStyle.Fill;
        lblOptions.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblOptions.Location = new Point(759, 238);
        lblOptions.Name = "lblOptions";
        lblOptions.Size = new Size(124, 38);
        lblOptions.TabIndex = 12;
        lblOptions.Text = "خيارات:";
        lblOptions.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblDesc
        // 
        lblDesc.Dock = DockStyle.Fill;
        lblDesc.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblDesc.Location = new Point(759, 276);
        lblDesc.Name = "lblDesc";
        lblDesc.Padding = new Padding(0, 2, 0, 0);
        lblDesc.Size = new Size(124, 112);
        lblDesc.TabIndex = 14;
        lblDesc.Text = "الوصف:";
        // 
        // tabPrices
        // 
        tabPrices.BackColor = Color.White;
        tabPrices.Controls.Add(pricesLayout);
        tabPrices.Location = new Point(4, 42);
        tabPrices.Name = "tabPrices";
        tabPrices.Padding = new Padding(10);
        tabPrices.Size = new Size(916, 530);
        tabPrices.TabIndex = 1;
        tabPrices.Text = "الأسعار";
        // 
        // pricesLayout
        // 
        pricesLayout.ColumnCount = 1;
        pricesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        pricesLayout.Controls.Add(lblPricesHeader, 0, 0);
        pricesLayout.Controls.Add(pricesToolbar, 0, 1);
        pricesLayout.Controls.Add(dgvPrices, 0, 2);
        pricesLayout.Controls.Add(lblPricesInfo, 0, 3);
        pricesLayout.Dock = DockStyle.Fill;
        pricesLayout.Location = new Point(10, 10);
        pricesLayout.Name = "pricesLayout";
        pricesLayout.Padding = new Padding(5);
        pricesLayout.RowCount = 4;
        pricesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        pricesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        pricesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pricesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        pricesLayout.Size = new Size(896, 510);
        pricesLayout.TabIndex = 0;
        // 
        // lblPricesHeader
        // 
        lblPricesHeader.Dock = DockStyle.Fill;
        lblPricesHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPricesHeader.ForeColor = Color.DimGray;
        lblPricesHeader.Location = new Point(8, 5);
        lblPricesHeader.Name = "lblPricesHeader";
        lblPricesHeader.Size = new Size(880, 35);
        lblPricesHeader.TabIndex = 0;
        lblPricesHeader.Text = "إدارة الأسعار تتم من خلال شاشة إدارة الأسعار المتخصصة.";
        lblPricesHeader.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pricesToolbar
        // 
        pricesToolbar.Controls.Add(btnOpenPricesScreen);
        pricesToolbar.Dock = DockStyle.Fill;
        pricesToolbar.FlowDirection = FlowDirection.RightToLeft;
        pricesToolbar.Location = new Point(8, 43);
        pricesToolbar.Name = "pricesToolbar";
        pricesToolbar.Size = new Size(880, 39);
        pricesToolbar.TabIndex = 1;
        // 
        // btnOpenPricesScreen
        // 
        btnOpenPricesScreen.AutoSize = true;
        btnOpenPricesScreen.BackColor = Color.FromArgb(13, 110, 253);
        btnOpenPricesScreen.Cursor = Cursors.Hand;
        btnOpenPricesScreen.FlatAppearance.BorderSize = 0;
        btnOpenPricesScreen.FlatStyle = FlatStyle.Flat;
        btnOpenPricesScreen.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnOpenPricesScreen.ForeColor = Color.White;
        btnOpenPricesScreen.Location = new Point(3, 3);
        btnOpenPricesScreen.MinimumSize = new Size(100, 35);
        btnOpenPricesScreen.Name = "btnOpenPricesScreen";
        btnOpenPricesScreen.Padding = new Padding(12, 0, 12, 0);
        btnOpenPricesScreen.Size = new Size(186, 35);
        btnOpenPricesScreen.TabIndex = 0;
        btnOpenPricesScreen.Text = "فتح شاشة الأسعار";
        btnOpenPricesScreen.UseVisualStyleBackColor = false;
        btnOpenPricesScreen.Click += btnOpenPricesScreen_Click;
        // 
        // dgvPrices
        // 
        dgvPrices.AllowUserToAddRows = false;
        dgvPrices.AllowUserToDeleteRows = false;
        dgvPrices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPrices.BackgroundColor = Color.White;
        dgvPrices.BorderStyle = BorderStyle.Fixed3D;
        dgvPrices.ColumnHeadersHeight = 40;
        dgvPrices.Dock = DockStyle.Fill;
        dgvPrices.Location = new Point(8, 88);
        dgvPrices.Name = "dgvPrices";
        dgvPrices.ReadOnly = true;
        dgvPrices.RowHeadersVisible = false;
        dgvPrices.RowHeadersWidth = 62;
        dgvPrices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPrices.Size = new Size(880, 384);
        dgvPrices.TabIndex = 2;
        // 
        // lblPricesInfo
        // 
        lblPricesInfo.Dock = DockStyle.Fill;
        lblPricesInfo.Font = new Font("Tahoma", 9F, FontStyle.Italic);
        lblPricesInfo.ForeColor = Color.Gray;
        lblPricesInfo.Location = new Point(8, 475);
        lblPricesInfo.Name = "lblPricesInfo";
        lblPricesInfo.Size = new Size(880, 30);
        lblPricesInfo.TabIndex = 3;
        lblPricesInfo.Text = "تعرض الأسعار لكل وحدة منتج. يمكن إضافة أسعار متعددة بوحدات وعملات مختلفة من شاشة الأسعار.";
        lblPricesInfo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tabUnits
        // 
        tabUnits.BackColor = Color.White;
        tabUnits.Controls.Add(unitsLayout);
        tabUnits.Location = new Point(4, 42);
        tabUnits.Name = "tabUnits";
        tabUnits.Padding = new Padding(10);
        tabUnits.Size = new Size(916, 530);
        tabUnits.TabIndex = 2;
        tabUnits.Text = "الوحدات";
        // 
        // unitsLayout
        // 
        unitsLayout.ColumnCount = 1;
        unitsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        unitsLayout.Controls.Add(lblUnitsHeader, 0, 0);
        unitsLayout.Controls.Add(unitsToolbar, 0, 1);
        unitsLayout.Controls.Add(dgvUnits, 0, 2);
        unitsLayout.Controls.Add(lblUnitsInfo, 0, 3);
        unitsLayout.Dock = DockStyle.Fill;
        unitsLayout.Location = new Point(10, 10);
        unitsLayout.Name = "unitsLayout";
        unitsLayout.Padding = new Padding(5);
        unitsLayout.RowCount = 4;
        unitsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        unitsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        unitsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        unitsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        unitsLayout.Size = new Size(896, 510);
        unitsLayout.TabIndex = 0;
        // 
        // lblUnitsHeader
        // 
        lblUnitsHeader.Dock = DockStyle.Fill;
        lblUnitsHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblUnitsHeader.ForeColor = Color.DimGray;
        lblUnitsHeader.Location = new Point(8, 5);
        lblUnitsHeader.Name = "lblUnitsHeader";
        lblUnitsHeader.Size = new Size(880, 35);
        lblUnitsHeader.TabIndex = 0;
        lblUnitsHeader.Text = "إدارة الوحدات تتم من خلال شاشة إدارة الوحدات المتخصصة.";
        lblUnitsHeader.TextAlign = ContentAlignment.MiddleRight;
        // 
        // unitsToolbar
        // 
        unitsToolbar.Controls.Add(btnOpenUnitsScreen);
        unitsToolbar.Dock = DockStyle.Fill;
        unitsToolbar.FlowDirection = FlowDirection.RightToLeft;
        unitsToolbar.Location = new Point(8, 43);
        unitsToolbar.Name = "unitsToolbar";
        unitsToolbar.Size = new Size(880, 39);
        unitsToolbar.TabIndex = 1;
        // 
        // btnOpenUnitsScreen
        // 
        btnOpenUnitsScreen.AutoSize = true;
        btnOpenUnitsScreen.BackColor = Color.FromArgb(13, 110, 253);
        btnOpenUnitsScreen.Cursor = Cursors.Hand;
        btnOpenUnitsScreen.FlatAppearance.BorderSize = 0;
        btnOpenUnitsScreen.FlatStyle = FlatStyle.Flat;
        btnOpenUnitsScreen.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnOpenUnitsScreen.ForeColor = Color.White;
        btnOpenUnitsScreen.Location = new Point(3, 3);
        btnOpenUnitsScreen.MinimumSize = new Size(100, 35);
        btnOpenUnitsScreen.Name = "btnOpenUnitsScreen";
        btnOpenUnitsScreen.Padding = new Padding(12, 0, 12, 0);
        btnOpenUnitsScreen.Size = new Size(235, 35);
        btnOpenUnitsScreen.TabIndex = 0;
        btnOpenUnitsScreen.Text = "فتح شاشة إدارة الوحدات";
        btnOpenUnitsScreen.UseVisualStyleBackColor = false;
        btnOpenUnitsScreen.Click += btnOpenUnitsScreen_Click;
        // 
        // dgvUnits
        // 
        dgvUnits.AllowUserToAddRows = false;
        dgvUnits.AllowUserToDeleteRows = false;
        dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvUnits.BackgroundColor = Color.White;
        dgvUnits.BorderStyle = BorderStyle.Fixed3D;
        dgvUnits.ColumnHeadersHeight = 40;
        dgvUnits.Dock = DockStyle.Fill;
        dgvUnits.Location = new Point(8, 88);
        dgvUnits.Name = "dgvUnits";
        dgvUnits.ReadOnly = true;
        dgvUnits.RowHeadersVisible = false;
        dgvUnits.RowHeadersWidth = 62;
        dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUnits.Size = new Size(880, 384);
        dgvUnits.TabIndex = 2;
        // 
        // lblUnitsInfo
        // 
        lblUnitsInfo.Dock = DockStyle.Fill;
        lblUnitsInfo.Font = new Font("Tahoma", 9F, FontStyle.Italic);
        lblUnitsInfo.ForeColor = Color.Gray;
        lblUnitsInfo.Location = new Point(8, 475);
        lblUnitsInfo.Name = "lblUnitsInfo";
        lblUnitsInfo.Size = new Size(880, 30);
        lblUnitsInfo.TabIndex = 3;
        lblUnitsInfo.Text = "يمكن إضافة وحدات متعددة للمنتج مع معامل تحويل لكل وحدة من شاشة إدارة الوحدات.";
        lblUnitsInfo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tabStock
        // 
        tabStock.BackColor = Color.White;
        tabStock.Controls.Add(stockLayout);
        tabStock.Location = new Point(4, 42);
        tabStock.Name = "tabStock";
        tabStock.Padding = new Padding(10);
        tabStock.Size = new Size(916, 530);
        tabStock.TabIndex = 3;
        tabStock.Text = "المخزون";
        // 
        // stockLayout
        // 
        stockLayout.ColumnCount = 1;
        stockLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        stockLayout.Controls.Add(stockToolbar, 0, 0);
        stockLayout.Controls.Add(dgvBatches, 0, 1);
        stockLayout.Controls.Add(lblStockInfo, 0, 2);
        stockLayout.Dock = DockStyle.Fill;
        stockLayout.Location = new Point(10, 10);
        stockLayout.Name = "stockLayout";
        stockLayout.Padding = new Padding(5);
        stockLayout.RowCount = 3;
        stockLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        stockLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        stockLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        stockLayout.Size = new Size(896, 510);
        stockLayout.TabIndex = 0;
        // 
        // stockToolbar
        // 
        stockToolbar.Controls.Add(btnRefreshBatches);
        stockToolbar.Controls.Add(btnViewStock);
        stockToolbar.Dock = DockStyle.Fill;
        stockToolbar.FlowDirection = FlowDirection.RightToLeft;
        stockToolbar.Location = new Point(8, 8);
        stockToolbar.Name = "stockToolbar";
        stockToolbar.Size = new Size(880, 39);
        stockToolbar.TabIndex = 0;
        // 
        // btnRefreshBatches
        // 
        btnRefreshBatches.AutoSize = true;
        btnRefreshBatches.BackColor = Color.FromArgb(13, 110, 253);
        btnRefreshBatches.Cursor = Cursors.Hand;
        btnRefreshBatches.FlatAppearance.BorderSize = 0;
        btnRefreshBatches.FlatStyle = FlatStyle.Flat;
        btnRefreshBatches.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnRefreshBatches.ForeColor = Color.White;
        btnRefreshBatches.Location = new Point(3, 3);
        btnRefreshBatches.MinimumSize = new Size(100, 35);
        btnRefreshBatches.Name = "btnRefreshBatches";
        btnRefreshBatches.Padding = new Padding(12, 0, 12, 0);
        btnRefreshBatches.Size = new Size(100, 35);
        btnRefreshBatches.TabIndex = 0;
        btnRefreshBatches.Text = "تحديث";
        btnRefreshBatches.UseVisualStyleBackColor = false;
        btnRefreshBatches.Click += btnRefreshBatches_Click;
        // 
        // btnViewStock
        // 
        btnViewStock.AutoSize = true;
        btnViewStock.BackColor = Color.FromArgb(248, 249, 250);
        btnViewStock.Cursor = Cursors.Hand;
        btnViewStock.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnViewStock.FlatStyle = FlatStyle.Flat;
        btnViewStock.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnViewStock.ForeColor = Color.FromArgb(33, 37, 41);
        btnViewStock.Location = new Point(109, 3);
        btnViewStock.MinimumSize = new Size(100, 35);
        btnViewStock.Name = "btnViewStock";
        btnViewStock.Padding = new Padding(12, 0, 12, 0);
        btnViewStock.Size = new Size(157, 37);
        btnViewStock.TabIndex = 1;
        btnViewStock.Text = "عرض المخزون";
        btnViewStock.UseVisualStyleBackColor = false;
        btnViewStock.Click += btnViewStock_Click;
        // 
        // dgvBatches
        // 
        dgvBatches.AllowUserToAddRows = false;
        dgvBatches.AllowUserToDeleteRows = false;
        dgvBatches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvBatches.BackgroundColor = Color.White;
        dgvBatches.BorderStyle = BorderStyle.Fixed3D;
        dgvBatches.ColumnHeadersHeight = 40;
        dgvBatches.Dock = DockStyle.Fill;
        dgvBatches.Location = new Point(8, 53);
        dgvBatches.Name = "dgvBatches";
        dgvBatches.ReadOnly = true;
        dgvBatches.RowHeadersVisible = false;
        dgvBatches.RowHeadersWidth = 62;
        dgvBatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBatches.Size = new Size(880, 419);
        dgvBatches.TabIndex = 1;
        // 
        // lblStockInfo
        // 
        lblStockInfo.Dock = DockStyle.Fill;
        lblStockInfo.Font = new Font("Tahoma", 9F, FontStyle.Italic);
        lblStockInfo.ForeColor = Color.Gray;
        lblStockInfo.Location = new Point(8, 475);
        lblStockInfo.Name = "lblStockInfo";
        lblStockInfo.Size = new Size(880, 30);
        lblStockInfo.TabIndex = 2;
        lblStockInfo.Text = "عرض تفاصيل الدفعات المخزنية لهذا المنتج. يتم تحديثها تلقائياً من حركات المخزون.";
        lblStockInfo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tabBom
        // 
        tabBom.BackColor = Color.White;
        tabBom.Controls.Add(lblBomPlaceholder);
        tabBom.Location = new Point(4, 42);
        tabBom.Name = "tabBom";
        tabBom.Padding = new Padding(10);
        tabBom.Size = new Size(916, 530);
        tabBom.TabIndex = 4;
        tabBom.Text = "المكونات";
        // 
        // lblBomPlaceholder
        // 
        lblBomPlaceholder.Dock = DockStyle.Fill;
        lblBomPlaceholder.Font = new Font("Tahoma", 14F);
        lblBomPlaceholder.ForeColor = Color.Gray;
        lblBomPlaceholder.Location = new Point(10, 10);
        lblBomPlaceholder.Name = "lblBomPlaceholder";
        lblBomPlaceholder.Size = new Size(896, 510);
        lblBomPlaceholder.TabIndex = 0;
        lblBomPlaceholder.Text = "قريباً — إدارة المكونات والتركيبات (BOM)\nيمكنك تركيب منتج من عدة مكونات مع تحديد الكميات المطلوبة.";
        lblBomPlaceholder.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // buttonPanel
        // 
        buttonPanel.BackColor = Color.White;
        buttonPanel.BorderStyle = BorderStyle.FixedSingle;
        buttonPanel.Controls.Add(btnSave);
        buttonPanel.Controls.Add(btnCancel);
        buttonPanel.Dock = DockStyle.Bottom;
        buttonPanel.FlowDirection = FlowDirection.RightToLeft;
        buttonPanel.Location = new Point(0, 576);
        buttonPanel.Name = "buttonPanel";
        buttonPanel.Padding = new Padding(12, 8, 12, 8);
        buttonPanel.Size = new Size(924, 55);
        buttonPanel.TabIndex = 1;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(15, 11);
        btnSave.MinimumSize = new Size(100, 35);
        btnSave.Name = "btnSave";
        btnSave.Padding = new Padding(12, 0, 12, 0);
        btnSave.Size = new Size(100, 35);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = Color.FromArgb(108, 117, 125);
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnCancel.ForeColor = Color.White;
        btnCancel.Location = new Point(121, 11);
        btnCancel.MinimumSize = new Size(100, 35);
        btnCancel.Name = "btnCancel";
        btnCancel.Padding = new Padding(12, 0, 12, 0);
        btnCancel.Size = new Size(100, 35);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // frmAddUpdateProduct
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        ClientSize = new Size(924, 631);
        Controls.Add(tabControl);
        Controls.Add(buttonPanel);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(700, 600);
        Name = "frmAddUpdateProduct";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "إضافة منتج جديد";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ((System.ComponentModel.ISupportInitialize)numReorderLevel).EndInit();
        tabControl.ResumeLayout(false);
        tabGeneral.ResumeLayout(false);
        genLayout.ResumeLayout(false);
        genLayout.PerformLayout();
        catPanel.ResumeLayout(false);
        baseUnitPanel.ResumeLayout(false);
        tabPrices.ResumeLayout(false);
        pricesLayout.ResumeLayout(false);
        pricesToolbar.ResumeLayout(false);
        pricesToolbar.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPrices).EndInit();
        tabUnits.ResumeLayout(false);
        unitsLayout.ResumeLayout(false);
        unitsToolbar.ResumeLayout(false);
        unitsToolbar.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUnits).EndInit();
        tabStock.ResumeLayout(false);
        stockLayout.ResumeLayout(false);
        stockToolbar.ResumeLayout(false);
        stockToolbar.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBatches).EndInit();
        tabBom.ResumeLayout(false);
        buttonPanel.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private TabControl tabControl;
    private Button btnSave;
    private Button btnCancel;
    private ErrorProvider errorProvider;
    private Label lblMode;
    private TextBox txtCode;
    private TextBox txtBarcode;
    private TextBox txtName;
    private ComboBox cmbCategory;
    private ComboBox cmbUnit;
    private NumericUpDown numReorderLevel;
    private TextBox txtDescription;
    private CheckBox chkTrackExpiry;
    private Button btnAddCategory;
    private Button btnAddUnit;
    private DataGridView dgvPrices;
    private Button btnOpenPricesScreen;
    private Button btnOpenUnitsScreen;
    private DataGridView dgvUnits;
    private DataGridView dgvBatches;
    private Button btnRefreshBatches;
    private Button btnViewStock;
    private Label lblBomPlaceholder;
    private ToolTip tooltip;
    private TabPage tabGeneral;
    private TableLayoutPanel genLayout;
    private Label lblCode;
    private Label lblBarcode;
    private Label lblName;
    private Label lblCategory;
    private TableLayoutPanel catPanel;
    private Label lblUnit;
    private TableLayoutPanel baseUnitPanel;
    private Label lblReorder;
    private Label lblOptions;
    private Label lblDesc;
    private TabPage tabPrices;
    private TableLayoutPanel pricesLayout;
    private Label lblPricesHeader;
    private FlowLayoutPanel pricesToolbar;
    private Label lblPricesInfo;
    private TabPage tabUnits;
    private TableLayoutPanel unitsLayout;
    private Label lblUnitsHeader;
    private FlowLayoutPanel unitsToolbar;
    private Label lblUnitsInfo;
    private TabPage tabStock;
    private TableLayoutPanel stockLayout;
    private FlowLayoutPanel stockToolbar;
    private Label lblStockInfo;
    private TabPage tabBom;
    private FlowLayoutPanel buttonPanel;
}
