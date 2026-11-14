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
        liveTip = new ToolTip(components);
        cmsLines = new ContextMenuStrip(components);
        miAddProduct = new ToolStripMenuItem();
        miRemoveLine = new ToolStripMenuItem();
        lblMode = new Label();
        gbToolbar = new GroupBox();
        btnAddProduct = new Button();
        btnRemoveProduct = new Button();
        btnSave = new Button();
        btnCancel = new Button();
        gbInfo = new GroupBox();
        lblWarehouse = new Label();
        cmbWarehouse = new ComboBox();
        lblDate = new Label();
        dtpDate = new DateTimePicker();
        gbItems = new GroupBox();
        dgvLines = new DataGridView();
        colProductName = new DataGridViewTextBoxColumn();
        colUnit = new DataGridViewComboBoxColumn();
        colQuantity = new DataGridViewTextBoxColumn();
        colUnitCost = new DataGridViewTextBoxColumn();
        colTotalCost = new DataGridViewTextBoxColumn();
        lblProductsHeader = new Label();
        gbTotals = new GroupBox();
        lblItemCountCaption = new Label();
        lblItemCountValue = new Label();
        lblTotalCostCaption = new Label();
        txtTotalCost = new TextBox();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        cmsLines.SuspendLayout();
        gbToolbar.SuspendLayout();
        gbInfo.SuspendLayout();
        gbItems.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        gbTotals.SuspendLayout();
        SuspendLayout();
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        // 
        // tooltip
        // 
        tooltip.AutoPopDelay = 6000;
        tooltip.InitialDelay = 300;
        tooltip.ReshowDelay = 100;
        // 
        // liveTip
        // 
        liveTip.AutoPopDelay = 4000;
        liveTip.InitialDelay = 200;
        liveTip.ReshowDelay = 100;
        // 
        // cmsLines
        // 
        cmsLines.Font = new Font("Tahoma", 10F);
        cmsLines.Items.AddRange(new ToolStripItem[] { miAddProduct, miRemoveLine });
        cmsLines.Name = "cmsLines";
        cmsLines.RightToLeft = RightToLeft.Yes;
        cmsLines.Size = new Size(210, 52);
        // 
        // miAddProduct
        // 
        miAddProduct.Name = "miAddProduct";
        miAddProduct.Size = new Size(209, 24);
        miAddProduct.Text = "إضافة منتج (F2)";
        miAddProduct.Click += miAddProduct_Click;
        // 
        // miRemoveLine
        // 
        miRemoveLine.Name = "miRemoveLine";
        miRemoveLine.Size = new Size(209, 24);
        miRemoveLine.Text = "حذف السطر المحدد";
        miRemoveLine.Click += miRemoveLine_Click;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Top;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1000, 42);
        lblMode.TabIndex = 0;
        lblMode.Text = "بضاعة أول المدة";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = Color.FromArgb(235, 245, 235);
        gbToolbar.Controls.Add(btnCancel);
        gbToolbar.Controls.Add(btnSave);
        gbToolbar.Controls.Add(btnRemoveProduct);
        gbToolbar.Controls.Add(btnAddProduct);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbToolbar.ForeColor = Color.FromArgb(14, 82, 52);
        gbToolbar.Location = new Point(0, 42);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Padding = new Padding(8);
        gbToolbar.RightToLeft = RightToLeft.Yes;
        gbToolbar.Size = new Size(1000, 68);
        gbToolbar.TabIndex = 1;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر المستند";
        // 
        // btnAddProduct
        // 
        btnAddProduct.BackColor = Color.FromArgb(13, 110, 253);
        btnAddProduct.Cursor = Cursors.Hand;
        btnAddProduct.FlatAppearance.BorderSize = 0;
        btnAddProduct.FlatStyle = FlatStyle.Flat;
        btnAddProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddProduct.ForeColor = Color.White;
        btnAddProduct.Location = new Point(12, 16);
        btnAddProduct.MinimumSize = new Size(100, 34);
        btnAddProduct.Name = "btnAddProduct";
        btnAddProduct.Size = new Size(170, 36);
        btnAddProduct.TabIndex = 0;
        btnAddProduct.Text = "+ إضافة منتج";
        btnAddProduct.UseVisualStyleBackColor = false;
        btnAddProduct.Click += btnAddProduct_Click;
        tooltip.SetToolTip(btnAddProduct, "إضافة منتج جديد للقائمة (F2)");
        // 
        // btnRemoveProduct
        // 
        btnRemoveProduct.BackColor = Color.FromArgb(220, 53, 69);
        btnRemoveProduct.Cursor = Cursors.Hand;
        btnRemoveProduct.FlatAppearance.BorderSize = 0;
        btnRemoveProduct.FlatStyle = FlatStyle.Flat;
        btnRemoveProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRemoveProduct.ForeColor = Color.White;
        btnRemoveProduct.Location = new Point(192, 16);
        btnRemoveProduct.MinimumSize = new Size(100, 34);
        btnRemoveProduct.Name = "btnRemoveProduct";
        btnRemoveProduct.Size = new Size(140, 36);
        btnRemoveProduct.TabIndex = 1;
        btnRemoveProduct.Text = "حذف السطر";
        btnRemoveProduct.UseVisualStyleBackColor = false;
        btnRemoveProduct.Click += btnRemoveProduct_Click;
        tooltip.SetToolTip(btnRemoveProduct, "حذف المنتج المحدد من القائمة");
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(345, 16);
        btnSave.MinimumSize = new Size(100, 34);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(150, 36);
        btnSave.TabIndex = 2;
        btnSave.Text = "حفظ وترحيل";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        tooltip.SetToolTip(btnSave, "حفظ وترحيل الأرصدة الافتتاحية (F4)");
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
        btnCancel.Location = new Point(505, 16);
        btnCancel.MinimumSize = new Size(100, 34);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(140, 36);
        btnCancel.TabIndex = 3;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        tooltip.SetToolTip(btnCancel, "إلغاء وإغلاق النافذة (Esc)");
        // 
        // gbInfo
        // 
        gbInfo.BackColor = Color.FromArgb(235, 245, 235);
        gbInfo.Controls.Add(dtpDate);
        gbInfo.Controls.Add(lblDate);
        gbInfo.Controls.Add(cmbWarehouse);
        gbInfo.Controls.Add(lblWarehouse);
        gbInfo.Dock = DockStyle.Top;
        gbInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbInfo.ForeColor = Color.FromArgb(14, 82, 52);
        gbInfo.Location = new Point(0, 110);
        gbInfo.Name = "gbInfo";
        gbInfo.Padding = new Padding(8);
        gbInfo.RightToLeft = RightToLeft.Yes;
        gbInfo.Size = new Size(1000, 86);
        gbInfo.TabIndex = 2;
        gbInfo.TabStop = false;
        gbInfo.Text = "بيانات المستند";
        // 
        // lblWarehouse
        // 
        lblWarehouse.AutoSize = true;
        lblWarehouse.Font = new Font("Tahoma", 10F);
        lblWarehouse.ForeColor = Color.FromArgb(33, 37, 41);
        lblWarehouse.Location = new Point(12, 30);
        lblWarehouse.Name = "lblWarehouse";
        lblWarehouse.Size = new Size(76, 21);
        lblWarehouse.TabIndex = 0;
        lblWarehouse.Text = "المخزن:";
        // 
        // cmbWarehouse
        // 
        cmbWarehouse.BackColor = Color.White;
        cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbWarehouse.FlatStyle = FlatStyle.Flat;
        cmbWarehouse.Font = new Font("Tahoma", 10F);
        cmbWarehouse.ForeColor = Color.FromArgb(33, 37, 41);
        cmbWarehouse.FormattingEnabled = true;
        cmbWarehouse.Location = new Point(110, 26);
        cmbWarehouse.Name = "cmbWarehouse";
        cmbWarehouse.Size = new Size(280, 29);
        cmbWarehouse.TabIndex = 1;
        tooltip.SetToolTip(cmbWarehouse, "اختر المخزن الذي ستضاف إليه الأرصدة");
        // 
        // lblDate
        // 
        lblDate.AutoSize = true;
        lblDate.Font = new Font("Tahoma", 10F);
        lblDate.ForeColor = Color.FromArgb(33, 37, 41);
        lblDate.Location = new Point(420, 30);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(70, 21);
        lblDate.TabIndex = 2;
        lblDate.Text = "التاريخ:";
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(505, 26);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(280, 27);
        dtpDate.TabIndex = 3;
        tooltip.SetToolTip(dtpDate, "تاريخ الجرد الافتتاحي");
        // 
        // gbItems
        // 
        gbItems.BackColor = Color.FromArgb(235, 245, 235);
        gbItems.Controls.Add(dgvLines);
        gbItems.Controls.Add(lblProductsHeader);
        gbItems.Dock = DockStyle.Fill;
        gbItems.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbItems.ForeColor = Color.FromArgb(14, 82, 52);
        gbItems.Location = new Point(0, 196);
        gbItems.Name = "gbItems";
        gbItems.Padding = new Padding(8);
        gbItems.RightToLeft = RightToLeft.Yes;
        gbItems.Size = new Size(1000, 392);
        gbItems.TabIndex = 3;
        gbItems.TabStop = false;
        gbItems.Text = "الأصناف";
        // 
        // lblProductsHeader
        // 
        lblProductsHeader.Dock = DockStyle.Top;
        lblProductsHeader.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblProductsHeader.ForeColor = Color.FromArgb(14, 82, 52);
        lblProductsHeader.Location = new Point(8, 24);
        lblProductsHeader.Name = "lblProductsHeader";
        lblProductsHeader.Size = new Size(984, 26);
        lblProductsHeader.TabIndex = 0;
        lblProductsHeader.Text = "أرصدة المنتجات — أدخل الكمية وسعر التكلفة لكل صنف، والوحدة من القائمة المنسدلة";
        lblProductsHeader.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // dgvLines
        // 
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;
        dgvLines.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
        dgvLines.AutoGenerateColumns = false;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvLines.BorderStyle = BorderStyle.None;
        dgvLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dgvLines.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvLines.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
        dgvLines.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dgvLines.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvLines.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
        dgvLines.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        dgvLines.ColumnHeadersHeight = 42;
        dgvLines.Columns.AddRange(new DataGridViewColumn[] { colProductName, colUnit, colQuantity, colUnitCost, colTotalCost });
        dgvLines.ContextMenuStrip = cmsLines;
        dgvLines.DefaultCellStyle.BackColor = Color.White;
        dgvLines.DefaultCellStyle.Font = new Font("Tahoma", 10F);
        dgvLines.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
        dgvLines.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
        dgvLines.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.EnableHeadersVisualStyles = false;
        dgvLines.GridColor = Color.FromArgb(205, 225, 205);
        dgvLines.Location = new Point(8, 50);
        dgvLines.Name = "dgvLines";
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowTemplate.Height = 30;
        dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLines.Size = new Size(984, 334);
        dgvLines.TabIndex = 1;
        dgvLines.CellValueChanged += dgvLines_CellValueChanged;
        dgvLines.CurrentCellDirtyStateChanged += dgvLines_CurrentCellDirtyStateChanged;
        dgvLines.EditingControlShowing += dgvLines_EditingControlShowing;
        tooltip.SetToolTip(dgvLines, "أدخل الكميات وتكاليف الوحدة للمنتجات. اختر الوحدة المناسبة من القائمة المنسدلة.");
        // 
        // colProductName
        // 
        colProductName.DataPropertyName = "ProductName";
        colProductName.HeaderText = "اسم المنتج";
        colProductName.MinimumWidth = 150;
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
        colUnit.MinimumWidth = 90;
        colUnit.Name = "colUnit";
        colUnit.ValueMember = "Id";
        // 
        // colQuantity
        // 
        colQuantity.DataPropertyName = "Quantity";
        colQuantity.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colQuantity.DefaultCellStyle.Format = "N3";
        colQuantity.HeaderText = "الكمية";
        colQuantity.MinimumWidth = 90;
        colQuantity.Name = "colQuantity";
        // 
        // colUnitCost
        // 
        colUnitCost.DataPropertyName = "UnitCost";
        colUnitCost.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colUnitCost.DefaultCellStyle.Format = "N2";
        colUnitCost.HeaderText = "سعر التكلفة";
        colUnitCost.MinimumWidth = 100;
        colUnitCost.Name = "colUnitCost";
        // 
        // colTotalCost
        // 
        colTotalCost.DataPropertyName = "TotalCost";
        colTotalCost.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colTotalCost.DefaultCellStyle.Format = "N2";
        colTotalCost.HeaderText = "الإجمالي";
        colTotalCost.MinimumWidth = 100;
        colTotalCost.Name = "colTotalCost";
        colTotalCost.ReadOnly = true;
        // 
        // gbTotals
        // 
        gbTotals.BackColor = Color.FromArgb(235, 245, 235);
        gbTotals.Controls.Add(txtTotalCost);
        gbTotals.Controls.Add(lblTotalCostCaption);
        gbTotals.Controls.Add(lblItemCountValue);
        gbTotals.Controls.Add(lblItemCountCaption);
        gbTotals.Dock = DockStyle.Bottom;
        gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbTotals.ForeColor = Color.FromArgb(14, 82, 52);
        gbTotals.Location = new Point(0, 588);
        gbTotals.Name = "gbTotals";
        gbTotals.Padding = new Padding(8);
        gbTotals.RightToLeft = RightToLeft.Yes;
        gbTotals.Size = new Size(1000, 72);
        gbTotals.TabIndex = 4;
        gbTotals.TabStop = false;
        gbTotals.Text = "الإجماليات";
        // 
        // lblItemCountCaption
        // 
        lblItemCountCaption.AutoSize = true;
        lblItemCountCaption.Font = new Font("Tahoma", 10F);
        lblItemCountCaption.ForeColor = Color.FromArgb(33, 37, 41);
        lblItemCountCaption.Location = new Point(12, 30);
        lblItemCountCaption.Name = "lblItemCountCaption";
        lblItemCountCaption.Size = new Size(90, 21);
        lblItemCountCaption.TabIndex = 0;
        lblItemCountCaption.Text = "عدد الأصناف:";
        // 
        // lblItemCountValue
        // 
        lblItemCountValue.AutoSize = true;
        lblItemCountValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblItemCountValue.ForeColor = Color.FromArgb(14, 82, 52);
        lblItemCountValue.Location = new Point(112, 30);
        lblItemCountValue.Name = "lblItemCountValue";
        lblItemCountValue.Size = new Size(23, 21);
        lblItemCountValue.TabIndex = 1;
        lblItemCountValue.Text = "0";
        // 
        // lblTotalCostCaption
        // 
        lblTotalCostCaption.AutoSize = true;
        lblTotalCostCaption.Font = new Font("Tahoma", 10F);
        lblTotalCostCaption.ForeColor = Color.FromArgb(33, 37, 41);
        lblTotalCostCaption.Location = new Point(200, 30);
        lblTotalCostCaption.Name = "lblTotalCostCaption";
        lblTotalCostCaption.Size = new Size(110, 21);
        lblTotalCostCaption.TabIndex = 2;
        lblTotalCostCaption.Text = "إجمالي التكلفة:";
        // 
        // txtTotalCost
        // 
        txtTotalCost.BackColor = Color.White;
        txtTotalCost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        txtTotalCost.ForeColor = Color.FromArgb(14, 82, 52);
        txtTotalCost.Location = new Point(320, 26);
        txtTotalCost.Name = "txtTotalCost";
        txtTotalCost.ReadOnly = true;
        txtTotalCost.Size = new Size(200, 27);
        txtTotalCost.TabIndex = 3;
        txtTotalCost.Text = "0.00";
        txtTotalCost.TextAlign = HorizontalAlignment.Center;
        tooltip.SetToolTip(txtTotalCost, "إجمالي تكلفة الأرصدة الافتتاحية (اقترب بالمؤشر لعرض التفقيط)");
        // 
        // frmOpeningStock
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnCancel;
        ClientSize = new Size(1000, 660);
        Controls.Add(gbItems);
        Controls.Add(gbTotals);
        Controls.Add(gbInfo);
        Controls.Add(gbToolbar);
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
        cmsLines.ResumeLayout(false);
        gbToolbar.ResumeLayout(false);
        gbInfo.ResumeLayout(false);
        gbInfo.PerformLayout();
        gbItems.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        gbTotals.ResumeLayout(false);
        gbTotals.PerformLayout();
        ResumeLayout(false);
    }

    private ErrorProvider errorProvider;
    private ToolTip tooltip;
    private ToolTip liveTip;
    private ContextMenuStrip cmsLines;
    private ToolStripMenuItem miAddProduct;
    private ToolStripMenuItem miRemoveLine;
    private Label lblMode;
    private GroupBox gbToolbar;
    private Button btnAddProduct;
    private Button btnRemoveProduct;
    private Button btnSave;
    private Button btnCancel;
    private GroupBox gbInfo;
    private Label lblWarehouse;
    private ComboBox cmbWarehouse;
    private Label lblDate;
    private DateTimePicker dtpDate;
    private GroupBox gbItems;
    private Label lblProductsHeader;
    private DataGridView dgvLines;
    private DataGridViewTextBoxColumn colProductName;
    private DataGridViewComboBoxColumn colUnit;
    private DataGridViewTextBoxColumn colQuantity;
    private DataGridViewTextBoxColumn colUnitCost;
    private DataGridViewTextBoxColumn colTotalCost;
    private GroupBox gbTotals;
    private Label lblItemCountCaption;
    private Label lblItemCountValue;
    private Label lblTotalCostCaption;
    private TextBox txtTotalCost;
}
