namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmOpeningStock
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
        errorProvider = new ErrorProvider(components);
        tooltip = new ToolTip(components);
        lblMode = new Label();
        lblWarehouse = new Label();
        cmbWarehouse = new ComboBox();
        lblDate = new Label();
        dtpDate = new DateTimePicker();
        dgvLines = new DataGridView();
        colProductName = new DataGridViewTextBoxColumn();
        colUnit = new DataGridViewComboBoxColumn();
        colQuantity = new DataGridViewTextBoxColumn();
        colUnitCost = new DataGridViewTextBoxColumn();
        colTotalCost = new DataGridViewTextBoxColumn();
        btnAddProduct = new Button();
        btnRemoveProduct = new Button();
        btnSave = new Button();
        btnCancel = new Button();
        lblProductsHeader = new Label();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        SuspendLayout();
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
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
        lblMode.Size = new Size(1000, 39);
        lblMode.TabIndex = 0;
        lblMode.Text = "بضاعة أول المدة";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblWarehouse
        // 
        lblWarehouse.AutoSize = true;
        lblWarehouse.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblWarehouse.Location = new Point(12, 63);
        lblWarehouse.Name = "lblWarehouse";
        lblWarehouse.Size = new Size(84, 24);
        lblWarehouse.TabIndex = 1;
        lblWarehouse.Text = "المخزن:";
        // 
        // cmbWarehouse
        // 
        cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbWarehouse.Font = new Font("Tahoma", 10F);
        cmbWarehouse.FormattingEnabled = true;
        cmbWarehouse.Location = new Point(102, 60);
        cmbWarehouse.Name = "cmbWarehouse";
        cmbWarehouse.Size = new Size(280, 32);
        cmbWarehouse.TabIndex = 2;
        tooltip.SetToolTip(cmbWarehouse, "اختر المخزن الذي ستضاف إليه الأرصدة");
        // 
        // lblDate
        // 
        lblDate.AutoSize = true;
        lblDate.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblDate.Location = new Point(402, 63);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(77, 24);
        lblDate.TabIndex = 3;
        lblDate.Text = "التاريخ:";
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(485, 60);
        dtpDate.Name = "dtpDate";
        dtpDate.Size = new Size(280, 32);
        dtpDate.TabIndex = 4;
        tooltip.SetToolTip(dtpDate, "تاريخ الجرد الافتتاحي");
        // 
        // dgvLines
        // 
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;
        dgvLines.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvLines.AutoGenerateColumns = false;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = Color.White;
        dgvLines.BorderStyle = BorderStyle.Fixed3D;
        dgvLines.ColumnHeadersHeight = 40;
        dgvLines.Columns.AddRange(new DataGridViewColumn[] { colProductName, colUnit, colQuantity, colUnitCost, colTotalCost });
        dgvLines.Location = new Point(12, 150);
        dgvLines.Name = "dgvLines";
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowTemplate.Height = 28;
        dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLines.Size = new Size(976, 400);
        dgvLines.TabIndex = 8;
        dgvLines.CellValueChanged += dgvLines_CellValueChanged;
        dgvLines.CurrentCellDirtyStateChanged += dgvLines_CurrentCellDirtyStateChanged;
        dgvLines.EditingControlShowing += dgvLines_EditingControlShowing;
        tooltip.SetToolTip(dgvLines, "أدخل الكميات وتكاليف الوحدة للمنتجات. اختر الوحدة المناسبة من القائمة المنسدلة.");
        // 
        // colProductName
        // 
        colProductName.DataPropertyName = "ProductName";
        colProductName.HeaderText = "اسم المنتج";
        colProductName.MinimumWidth = 8;
        colProductName.Name = "colProductName";
        colProductName.ReadOnly = true;
        // 
        // colUnit
        // 
        colUnit.DataPropertyName = "ProductUnitId";
        colUnit.DisplayMember = "UnitName";
        colUnit.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
        colUnit.FlatStyle = FlatStyle.Flat;
        colUnit.HeaderText = "الوحدة";
        colUnit.MinimumWidth = 8;
        colUnit.Name = "colUnit";
        colUnit.ValueMember = "Id";
        // 
        // colQuantity
        // 
        colQuantity.DataPropertyName = "Quantity";
        colQuantity.HeaderText = "الكمية";
        colQuantity.MinimumWidth = 8;
        colQuantity.Name = "colQuantity";
        // 
        // colUnitCost
        // 
        colUnitCost.DataPropertyName = "UnitCost";
        colUnitCost.HeaderText = "سعر التكلفة";
        colUnitCost.MinimumWidth = 8;
        colUnitCost.Name = "colUnitCost";
        // 
        // colTotalCost
        // 
        colTotalCost.DataPropertyName = "TotalCost";
        colTotalCost.HeaderText = "الإجمالي";
        colTotalCost.MinimumWidth = 8;
        colTotalCost.Name = "colTotalCost";
        colTotalCost.ReadOnly = true;
        // 
        // btnAddProduct
        // 
        btnAddProduct.BackColor = Color.FromArgb(13, 110, 253);
        btnAddProduct.Cursor = Cursors.Hand;
        btnAddProduct.FlatAppearance.BorderSize = 0;
        btnAddProduct.FlatStyle = FlatStyle.Flat;
        btnAddProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddProduct.ForeColor = Color.White;
        btnAddProduct.Location = new Point(12, 105);
        btnAddProduct.MinimumSize = new Size(100, 35);
        btnAddProduct.Name = "btnAddProduct";
        btnAddProduct.Size = new Size(160, 35);
        btnAddProduct.TabIndex = 5;
        btnAddProduct.Text = "+ إضافة منتج";
        btnAddProduct.UseVisualStyleBackColor = false;
        btnAddProduct.Click += btnAddProduct_Click;
        tooltip.SetToolTip(btnAddProduct, "إضافة منتج جديد للقائمة");
        // 
        // btnRemoveProduct
        // 
        btnRemoveProduct.BackColor = Color.FromArgb(220, 53, 69);
        btnRemoveProduct.Cursor = Cursors.Hand;
        btnRemoveProduct.FlatAppearance.BorderSize = 0;
        btnRemoveProduct.FlatStyle = FlatStyle.Flat;
        btnRemoveProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRemoveProduct.ForeColor = Color.White;
        btnRemoveProduct.Location = new Point(182, 105);
        btnRemoveProduct.MinimumSize = new Size(100, 35);
        btnRemoveProduct.Name = "btnRemoveProduct";
        btnRemoveProduct.Size = new Size(120, 35);
        btnRemoveProduct.TabIndex = 6;
        btnRemoveProduct.Text = "حذف";
        btnRemoveProduct.UseVisualStyleBackColor = false;
        btnRemoveProduct.Click += btnRemoveProduct_Click;
        tooltip.SetToolTip(btnRemoveProduct, "حذف المنتج المحدد من القائمة");
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(12, 570);
        btnSave.MinimumSize = new Size(100, 35);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(140, 40);
        btnSave.TabIndex = 9;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        tooltip.SetToolTip(btnSave, "حفظ وترحيل الأرصدة الافتتاحية");
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnCancel.BackColor = Color.FromArgb(108, 117, 125);
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancel.ForeColor = Color.White;
        btnCancel.Location = new Point(162, 570);
        btnCancel.MinimumSize = new Size(100, 35);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(140, 40);
        btnCancel.TabIndex = 10;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        tooltip.SetToolTip(btnCancel, "إلغاء وإغلاق النافذة");
        // 
        // lblProductsHeader
        // 
        lblProductsHeader.AutoSize = true;
        lblProductsHeader.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        lblProductsHeader.ForeColor = Color.DimGray;
        lblProductsHeader.Location = new Point(810, 112);
        lblProductsHeader.Name = "lblProductsHeader";
        lblProductsHeader.Size = new Size(178, 27);
        lblProductsHeader.TabIndex = 7;
        lblProductsHeader.Text = "أرصدة المنتجات";
        // 
        // frmOpeningStock
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnCancel;
        ClientSize = new Size(1000, 630);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(dgvLines);
        Controls.Add(btnRemoveProduct);
        Controls.Add(btnAddProduct);
        Controls.Add(lblProductsHeader);
        Controls.Add(dtpDate);
        Controls.Add(lblDate);
        Controls.Add(cmbWarehouse);
        Controls.Add(lblWarehouse);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmOpeningStock";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "بضاعة أول المدة";
        Load += frmOpeningStock_Load;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private ErrorProvider errorProvider;
    private ToolTip tooltip;
    private Label lblMode;
    private Label lblWarehouse;
    private ComboBox cmbWarehouse;
    private Label lblDate;
    private DateTimePicker dtpDate;
    private Label lblProductsHeader;
    private Button btnAddProduct;
    private Button btnRemoveProduct;
    private DataGridView dgvLines;
    private Button btnSave;
    private Button btnCancel;
    private DataGridViewTextBoxColumn colProductName;
    private DataGridViewComboBoxColumn colUnit;
    private DataGridViewTextBoxColumn colQuantity;
    private DataGridViewTextBoxColumn colUnitCost;
    private DataGridViewTextBoxColumn colTotalCost;
}
