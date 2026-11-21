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
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
        errorProvider = new ErrorProvider(components);
        tooltip = new ToolTip(components);
        liveTip = new ToolTip(components);
        cmsLines = new ContextMenuStrip(components);
        miEditLine = new ToolStripMenuItem();
        miRemoveLine = new ToolStripMenuItem();
        miPickProduct = new ToolStripMenuItem();
        lblMode = new Label();
        gbToolbar = new GroupBox();
        flpToolbar = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnClose = new Button();
        gbInfo = new GroupBox();
        cmbWarehouse = new ComboBox();
        lblWarehouse = new Label();
        dtpDate = new DateTimePicker();
        lblDate = new Label();
        gbEntry = new GroupBox();
        pnlEntry = new Panel();
        lblBarcodeLabel = new Label();
        lblProductLabel = new Label();
        lblUnitLabel = new Label();
        lblQtyLabel = new Label();
        lblCostLabel = new Label();
        lblLineTotalHeaderLabel = new Label();
        txtBarcode = new TextBox();
        btnPickProduct = new Button();
        lblProductName = new Label();
        cmbUnit = new ComboBox();
        txtQuantity = new TextBox();
        txtUnitCost = new TextBox();
        lblLineTotal = new Label();
        btnAddLine = new Button();
        btnCancelLine = new Button();
        gbItems = new GroupBox();
        lblProductsHeader = new Label();
        dgvLines = new DataGridView();
        colRowNum = new DataGridViewTextBoxColumn();
        colBarcode = new DataGridViewTextBoxColumn();
        colProductName = new DataGridViewTextBoxColumn();
        colUnit = new DataGridViewTextBoxColumn();
        colQuantity = new DataGridViewTextBoxColumn();
        colUnitCost = new DataGridViewTextBoxColumn();
        colTotalCost = new DataGridViewTextBoxColumn();
        gbTotals = new GroupBox();
        lblItemCountCaption = new Label();
        lblItemCountValue = new Label();
        lblTotalQtyCaption = new Label();
        lblTotalQtyValue = new Label();
        lblTotalCostCaption = new Label();
        txtTotalCost = new TextBox();
        lblTafqitLabel = new Label();
        lblTafqitValue = new Label();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        cmsLines.SuspendLayout();
        gbToolbar.SuspendLayout();
        flpToolbar.SuspendLayout();
        gbInfo.SuspendLayout();
        gbEntry.SuspendLayout();
        pnlEntry.SuspendLayout();
        gbItems.SuspendLayout();
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
        liveTip.ShowAlways = true;
        // 
        // cmsLines
        // 
        cmsLines.Font = new Font("Tahoma", 10F);
        cmsLines.ImageScalingSize = new Size(24, 24);
        cmsLines.Items.AddRange(new ToolStripItem[] { miEditLine, miRemoveLine, miPickProduct });
        cmsLines.Name = "cmsLines";
        cmsLines.RightToLeft = RightToLeft.Yes;
        cmsLines.Size = new Size(245, 94);
        // 
        // miEditLine
        // 
        miEditLine.Name = "miEditLine";
        miEditLine.Size = new Size(244, 30);
        miEditLine.Text = "تعديل السطر";
        miEditLine.Click += miEditLine_Click;
        // 
        // miRemoveLine
        // 
        miRemoveLine.Name = "miRemoveLine";
        miRemoveLine.Size = new Size(244, 30);
        miRemoveLine.Text = "حذف السطر";
        miRemoveLine.Click += miRemoveLine_Click;
        // 
        // miPickProduct
        // 
        miPickProduct.Name = "miPickProduct";
        miPickProduct.Size = new Size(244, 30);
        miPickProduct.Text = "اختيار منتج (F4)";
        miPickProduct.Click += miPickProduct_Click;
        // 
        // lblMode
        // 
        lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        lblMode.Dock = DockStyle.Top;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = System.Drawing.Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1600, 42);
        lblMode.TabIndex = 0;
        lblMode.Text = "بضاعة أول المدة";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        gbToolbar.Controls.Add(flpToolbar);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbToolbar.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbToolbar.Location = new Point(0, 42);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Size = new Size(1600, 83);
        gbToolbar.TabIndex = 1;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر المستند";
        // 
        // flpToolbar
        // 
        flpToolbar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        flpToolbar.Controls.Add(btnNew);
        flpToolbar.Controls.Add(btnSave);
        flpToolbar.Controls.Add(btnClose);
        flpToolbar.Dock = DockStyle.Fill;
        flpToolbar.Location = new Point(3, 28);
        flpToolbar.Name = "flpToolbar";
        flpToolbar.Padding = new Padding(6, 4, 6, 4);
        flpToolbar.Size = new Size(1594, 52);
        flpToolbar.TabIndex = 0;
        flpToolbar.WrapContents = false;
        // 
        // btnNew
        // 
        btnNew.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(13), Convert.ToInt32(110), Convert.ToInt32(253));
        btnNew.Cursor = Cursors.Hand;
        btnNew.FlatAppearance.BorderSize = 0;
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = System.Drawing.Color.White;
        btnNew.Margin = new Padding(6, 4, 6, 4);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(150, 36);
        btnNew.TabIndex = 0;
        btnNew.Text = "جديد (F2)";
        tooltip.SetToolTip(btnNew, "بدء مستند بضاعة أول المدة جديد (F2)");
        btnNew.UseVisualStyleBackColor = false;
        btnNew.Click += btnNew_Click;
        // 
        // btnSave
        // 
        btnSave.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = System.Drawing.Color.White;
        btnSave.Margin = new Padding(6, 4, 6, 4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(180, 36);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ وترحيل (F4)";
        tooltip.SetToolTip(btnSave, "حفظ وترحيل الأرصدة الافتتاحية ثم بدء مستند جديد (F4)");
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClose.Cursor = Cursors.Hand;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = System.Drawing.Color.White;
        btnClose.Margin = new Padding(6, 4, 6, 4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(150, 36);
        btnClose.TabIndex = 2;
        btnClose.Text = "إغلاق (Esc)";
        tooltip.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // gbInfo
        // 
        gbInfo.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        gbInfo.Controls.Add(cmbWarehouse);
        gbInfo.Controls.Add(lblWarehouse);
        gbInfo.Controls.Add(dtpDate);
        gbInfo.Controls.Add(lblDate);
        gbInfo.Dock = DockStyle.Top;
        gbInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbInfo.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbInfo.Location = new Point(0, 125);
        gbInfo.Name = "gbInfo";
        gbInfo.Size = new Size(1600, 86);
        gbInfo.TabIndex = 2;
        gbInfo.TabStop = false;
        gbInfo.Text = "بيانات المستند";
        // 
        // cmbWarehouse
        // 
        cmbWarehouse.BackColor = System.Drawing.Color.White;
        cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbWarehouse.FlatStyle = FlatStyle.Flat;
        cmbWarehouse.Font = new Font("Tahoma", 10F);
        cmbWarehouse.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cmbWarehouse.FormattingEnabled = true;
        cmbWarehouse.Location = new Point(1190, 26);
        cmbWarehouse.Name = "cmbWarehouse";
        cmbWarehouse.Size = new Size(240, 32);
        cmbWarehouse.TabIndex = 0;
        tooltip.SetToolTip(cmbWarehouse, "المخزن الذي تُضاف إليه الأرصدة الافتتاحية");
        // 
        // lblWarehouse
        // 
        lblWarehouse.Font = new Font("Tahoma", 10F);
        lblWarehouse.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblWarehouse.Location = new Point(1440, 30);
        lblWarehouse.Name = "lblWarehouse";
        lblWarehouse.Size = new Size(110, 24);
        lblWarehouse.TabIndex = 1;
        lblWarehouse.Text = "المخزن:";
        lblWarehouse.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpDate
        // 
        dtpDate.BackColor = System.Drawing.Color.White;
        dtpDate.CustomFormat = "yyyy/MM/dd";
        dtpDate.Enabled = false;
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Custom;
        dtpDate.Location = new Point(880, 26);
        dtpDate.Name = "dtpDate";
        dtpDate.Size = new Size(190, 32);
        dtpDate.TabIndex = 2;
        tooltip.SetToolTip(dtpDate, "تاريخ الترحيل يُسجَّل تلقائياً بتاريخ اليوم");
        // 
        // lblDate
        // 
        lblDate.Font = new Font("Tahoma", 10F);
        lblDate.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblDate.Location = new Point(1080, 30);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(100, 24);
        lblDate.TabIndex = 3;
        lblDate.Text = "التاريخ:";
        lblDate.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // gbEntry
        // 
        gbEntry.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        gbEntry.Controls.Add(pnlEntry);
        gbEntry.Dock = DockStyle.Top;
        gbEntry.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbEntry.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbEntry.Location = new Point(0, 211);
        gbEntry.Name = "gbEntry";
        gbEntry.Size = new Size(1600, 125);
        gbEntry.TabIndex = 3;
        gbEntry.TabStop = false;
        gbEntry.Text = "إدخال صنف";
        // 
        // pnlEntry
        // 
        pnlEntry.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        pnlEntry.BorderStyle = BorderStyle.FixedSingle;
        pnlEntry.Controls.Add(lblBarcodeLabel);
        pnlEntry.Controls.Add(lblProductLabel);
        pnlEntry.Controls.Add(lblUnitLabel);
        pnlEntry.Controls.Add(lblQtyLabel);
        pnlEntry.Controls.Add(lblCostLabel);
        pnlEntry.Controls.Add(lblLineTotalHeaderLabel);
        pnlEntry.Controls.Add(txtBarcode);
        pnlEntry.Controls.Add(btnPickProduct);
        pnlEntry.Controls.Add(lblProductName);
        pnlEntry.Controls.Add(cmbUnit);
        pnlEntry.Controls.Add(txtQuantity);
        pnlEntry.Controls.Add(txtUnitCost);
        pnlEntry.Controls.Add(lblLineTotal);
        pnlEntry.Controls.Add(btnAddLine);
        pnlEntry.Controls.Add(btnCancelLine);
        pnlEntry.Dock = DockStyle.Fill;
        pnlEntry.Location = new Point(3, 28);
        pnlEntry.Name = "pnlEntry";
        pnlEntry.Size = new Size(1594, 94);
        pnlEntry.TabIndex = 0;
        // 
        // lblBarcodeLabel
        // 
        lblBarcodeLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblBarcodeLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        lblBarcodeLabel.Location = new Point(1300, 6);
        lblBarcodeLabel.Name = "lblBarcodeLabel";
        lblBarcodeLabel.Size = new Size(270, 22);
        lblBarcodeLabel.TabIndex = 0;
        lblBarcodeLabel.Text = "باركود الوحدة";
        lblBarcodeLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblProductLabel
        // 
        lblProductLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblProductLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        lblProductLabel.Location = new Point(890, 6);
        lblProductLabel.Name = "lblProductLabel";
        lblProductLabel.Size = new Size(240, 22);
        lblProductLabel.TabIndex = 1;
        lblProductLabel.Text = "الصنف";
        lblProductLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblUnitLabel
        // 
        lblUnitLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblUnitLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        lblUnitLabel.Location = new Point(780, 6);
        lblUnitLabel.Name = "lblUnitLabel";
        lblUnitLabel.Size = new Size(100, 22);
        lblUnitLabel.TabIndex = 2;
        lblUnitLabel.Text = "الوحدة";
        lblUnitLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblQtyLabel
        // 
        lblQtyLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblQtyLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        lblQtyLabel.Location = new Point(670, 6);
        lblQtyLabel.Name = "lblQtyLabel";
        lblQtyLabel.Size = new Size(100, 22);
        lblQtyLabel.TabIndex = 3;
        lblQtyLabel.Text = "الكمية";
        lblQtyLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblCostLabel
        // 
        lblCostLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblCostLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        lblCostLabel.Location = new Point(550, 6);
        lblCostLabel.Name = "lblCostLabel";
        lblCostLabel.Size = new Size(110, 22);
        lblCostLabel.TabIndex = 4;
        lblCostLabel.Text = "تكلفة الوحدة";
        lblCostLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblLineTotalHeaderLabel
        // 
        lblLineTotalHeaderLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblLineTotalHeaderLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        lblLineTotalHeaderLabel.Location = new Point(410, 6);
        lblLineTotalHeaderLabel.Name = "lblLineTotalHeaderLabel";
        lblLineTotalHeaderLabel.Size = new Size(130, 22);
        lblLineTotalHeaderLabel.TabIndex = 5;
        lblLineTotalHeaderLabel.Text = "إجمالي السطر";
        lblLineTotalHeaderLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // txtBarcode
        // 
        txtBarcode.BackColor = System.Drawing.Color.White;
        txtBarcode.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        txtBarcode.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtBarcode.Location = new Point(1300, 31);
        txtBarcode.Name = "txtBarcode";
        txtBarcode.Size = new Size(270, 34);
        txtBarcode.TabIndex = 6;
        txtBarcode.TextAlign = HorizontalAlignment.Center;
        tooltip.SetToolTip(txtBarcode, "امسح باركود وحدة الصنف أو اكتبه ثم Enter — وEnter على حقل فارغ يفتح منتقي الأصناف");
        // 
        // btnPickProduct
        // 
        btnPickProduct.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnPickProduct.Cursor = Cursors.Hand;
        btnPickProduct.FlatAppearance.BorderSize = 0;
        btnPickProduct.FlatStyle = FlatStyle.Flat;
        btnPickProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPickProduct.ForeColor = System.Drawing.Color.White;
        btnPickProduct.Location = new Point(1140, 28);
        btnPickProduct.Name = "btnPickProduct";
        btnPickProduct.Size = new Size(150, 40);
        btnPickProduct.TabIndex = 7;
        btnPickProduct.Text = "اختيار منتج (F4)";
        tooltip.SetToolTip(btnPickProduct, "فتح قائمة الأصناف واختيار صنف (F4)");
        btnPickProduct.UseVisualStyleBackColor = false;
        btnPickProduct.Click += btnPickProduct_Click;
        // 
        // lblProductName
        // 
        lblProductName.BackColor = System.Drawing.Color.White;
        lblProductName.BorderStyle = BorderStyle.FixedSingle;
        lblProductName.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        lblProductName.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblProductName.Location = new Point(890, 31);
        lblProductName.Name = "lblProductName";
        lblProductName.Size = new Size(240, 34);
        lblProductName.TabIndex = 8;
        lblProductName.TextAlign = ContentAlignment.MiddleCenter;
        tooltip.SetToolTip(lblProductName, "اسم الصنف المحدد — للعرض فقط");
        // 
        // cmbUnit
        // 
        cmbUnit.BackColor = System.Drawing.Color.White;
        cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbUnit.FlatStyle = FlatStyle.Flat;
        cmbUnit.Font = new Font("Tahoma", 10F);
        cmbUnit.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cmbUnit.FormattingEnabled = true;
        cmbUnit.Location = new Point(780, 32);
        cmbUnit.Name = "cmbUnit";
        cmbUnit.Size = new Size(100, 32);
        cmbUnit.TabIndex = 9;
        tooltip.SetToolTip(cmbUnit, "وحدة القياس التي تُدخل بها الكمية والتكلفة");
        // 
        // txtQuantity
        // 
        txtQuantity.BackColor = System.Drawing.Color.White;
        txtQuantity.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        txtQuantity.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtQuantity.Location = new Point(670, 31);
        txtQuantity.Name = "txtQuantity";
        txtQuantity.Size = new Size(100, 34);
        txtQuantity.TabIndex = 10;
        txtQuantity.Text = "1";
        txtQuantity.TextAlign = HorizontalAlignment.Center;
        tooltip.SetToolTip(txtQuantity, "الكمية الافتتاحية بالوحدة المختارة");
        // 
        // txtUnitCost
        // 
        txtUnitCost.BackColor = System.Drawing.Color.White;
        txtUnitCost.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        txtUnitCost.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtUnitCost.Location = new Point(550, 31);
        txtUnitCost.Name = "txtUnitCost";
        txtUnitCost.Size = new Size(110, 34);
        txtUnitCost.TabIndex = 11;
        txtUnitCost.TextAlign = HorizontalAlignment.Center;
        tooltip.SetToolTip(txtUnitCost, "تكلفة الوحدة الافتتاحية — Enter هنا يضيف السطر");
        // 
        // lblLineTotal
        // 
        lblLineTotal.BackColor = System.Drawing.Color.White;
        lblLineTotal.BorderStyle = BorderStyle.FixedSingle;
        lblLineTotal.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        lblLineTotal.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblLineTotal.Location = new Point(410, 31);
        lblLineTotal.Name = "lblLineTotal";
        lblLineTotal.Size = new Size(130, 34);
        lblLineTotal.TabIndex = 12;
        lblLineTotal.Text = "0.00";
        lblLineTotal.TextAlign = ContentAlignment.MiddleCenter;
        tooltip.SetToolTip(lblLineTotal, "الكمية × تكلفة الوحدة");
        // 
        // btnAddLine
        // 
        btnAddLine.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnAddLine.Cursor = Cursors.Hand;
        btnAddLine.FlatAppearance.BorderSize = 0;
        btnAddLine.FlatStyle = FlatStyle.Flat;
        btnAddLine.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddLine.ForeColor = System.Drawing.Color.White;
        btnAddLine.Location = new Point(240, 28);
        btnAddLine.Name = "btnAddLine";
        btnAddLine.Size = new Size(160, 40);
        btnAddLine.TabIndex = 13;
        btnAddLine.Text = "إضافة السطر";
        tooltip.SetToolTip(btnAddLine, "إضافة السطر إلى المستند (Enter في آخر حقل يفعل نفس الشيء)");
        btnAddLine.UseVisualStyleBackColor = false;
        btnAddLine.Click += btnAddLine_Click;
        // 
        // btnCancelLine
        // 
        btnCancelLine.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnCancelLine.Cursor = Cursors.Hand;
        btnCancelLine.Enabled = false;
        btnCancelLine.FlatAppearance.BorderSize = 0;
        btnCancelLine.FlatStyle = FlatStyle.Flat;
        btnCancelLine.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancelLine.ForeColor = System.Drawing.Color.White;
        btnCancelLine.Location = new Point(70, 28);
        btnCancelLine.Name = "btnCancelLine";
        btnCancelLine.Size = new Size(160, 40);
        btnCancelLine.TabIndex = 14;
        btnCancelLine.Text = "إلغاء السطر";
        tooltip.SetToolTip(btnCancelLine, "إلغاء تعديل السطر وإفراغ لوحة الإدخال (Esc)");
        btnCancelLine.UseVisualStyleBackColor = false;
        btnCancelLine.Click += btnCancelLine_Click;
        // 
        // gbItems
        // 
        gbItems.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        gbItems.Controls.Add(dgvLines);
        gbItems.Controls.Add(lblProductsHeader);
        gbItems.Dock = DockStyle.Fill;
        gbItems.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbItems.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbItems.Location = new Point(0, 336);
        gbItems.Name = "gbItems";
        gbItems.Size = new Size(1600, 368);
        gbItems.TabIndex = 4;
        gbItems.TabStop = false;
        gbItems.Text = "الأصناف";
        // 
        // lblProductsHeader
        // 
        lblProductsHeader.Dock = DockStyle.Top;
        lblProductsHeader.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblProductsHeader.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblProductsHeader.Location = new Point(8, 33);
        lblProductsHeader.Name = "lblProductsHeader";
        lblProductsHeader.Size = new Size(1584, 26);
        lblProductsHeader.TabIndex = 0;
        lblProductsHeader.Text = "امسح الباركود أو اختر صنفاً (F4) ثم الكمية وتكلفة الوحدة — Enter في آخر حقل يضيف السطر ويعيدك إلى الباركود";
        lblProductsHeader.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // dgvLines
        // 
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;
        dgvLines.AllowUserToResizeRows = false;
        dgvLines.AutoGenerateColumns = false;
        dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
        dgvLines.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
        dgvLines.BorderStyle = BorderStyle.None;
        dgvLines.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvLines.ColumnHeadersHeight = 45;
        dgvLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvLines.Columns.AddRange(new DataGridViewColumn[] { colRowNum, colBarcode, colProductName, colUnit, colQuantity, colUnitCost, colTotalCost });
        dgvLines.ContextMenuStrip = cmsLines;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
        dataGridViewCellStyle3.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
        dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvLines.DefaultCellStyle = dataGridViewCellStyle3;
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.EnableHeadersVisualStyles = false;
        dgvLines.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        dgvLines.Location = new Point(8, 59);
        dgvLines.MultiSelect = false;
        dgvLines.Name = "dgvLines";
        dgvLines.ReadOnly = true;
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowHeadersWidth = 62;
        dgvLines.RowTemplate.Height = 32;
        dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLines.Size = new Size(1584, 301);
        dgvLines.TabIndex = 1;
        tooltip.SetToolTip(dgvLines, "أسطر المستند — نقرة مزدوجة على سطر تحمّله في لوحة الإدخال للتعديل");
        dgvLines.CellDoubleClick += dgvLines_CellDoubleClick;
        // 
        // colRowNum
        // 
        colRowNum.DataPropertyName = "RowNum";
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colRowNum.DefaultCellStyle = dataGridViewCellStyle4;
        colRowNum.FillWeight = 35F;
        colRowNum.HeaderText = "#";
        colRowNum.MinimumWidth = 45;
        colRowNum.Name = "colRowNum";
        colRowNum.ReadOnly = true;
        colRowNum.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colBarcode
        // 
        colBarcode.DataPropertyName = "Barcode";
        colBarcode.FillWeight = 80F;
        colBarcode.HeaderText = "الباركود";
        colBarcode.MinimumWidth = 90;
        colBarcode.Name = "colBarcode";
        colBarcode.ReadOnly = true;
        colBarcode.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colProductName
        // 
        colProductName.DataPropertyName = "ProductName";
        colProductName.FillWeight = 170F;
        colProductName.HeaderText = "الصنف";
        colProductName.MinimumWidth = 140;
        colProductName.Name = "colProductName";
        colProductName.ReadOnly = true;
        colProductName.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colUnit
        // 
        colUnit.DataPropertyName = "UnitName";
        colUnit.FillWeight = 70F;
        colUnit.HeaderText = "الوحدة";
        colUnit.MinimumWidth = 70;
        colUnit.Name = "colUnit";
        colUnit.ReadOnly = true;
        colUnit.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colQuantity
        // 
        colQuantity.DataPropertyName = "Quantity";
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle5.Format = "N3";
        colQuantity.DefaultCellStyle = dataGridViewCellStyle5;
        colQuantity.FillWeight = 70F;
        colQuantity.HeaderText = "الكمية";
        colQuantity.MinimumWidth = 70;
        colQuantity.Name = "colQuantity";
        colQuantity.ReadOnly = true;
        colQuantity.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colUnitCost
        // 
        colUnitCost.DataPropertyName = "UnitCost";
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle6.Format = "N2";
        colUnitCost.DefaultCellStyle = dataGridViewCellStyle6;
        colUnitCost.FillWeight = 80F;
        colUnitCost.HeaderText = "تكلفة الوحدة";
        colUnitCost.MinimumWidth = 90;
        colUnitCost.Name = "colUnitCost";
        colUnitCost.ReadOnly = true;
        colUnitCost.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colTotalCost
        // 
        colTotalCost.DataPropertyName = "TotalCost";
        dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle7.Format = "N2";
        colTotalCost.DefaultCellStyle = dataGridViewCellStyle7;
        colTotalCost.FillWeight = 90F;
        colTotalCost.HeaderText = "إجمالي السطر";
        colTotalCost.MinimumWidth = 100;
        colTotalCost.Name = "colTotalCost";
        colTotalCost.ReadOnly = true;
        colTotalCost.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // gbTotals
        // 
        gbTotals.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        gbTotals.Controls.Add(lblItemCountCaption);
        gbTotals.Controls.Add(lblItemCountValue);
        gbTotals.Controls.Add(lblTotalQtyCaption);
        gbTotals.Controls.Add(lblTotalQtyValue);
        gbTotals.Controls.Add(lblTotalCostCaption);
        gbTotals.Controls.Add(txtTotalCost);
        gbTotals.Controls.Add(lblTafqitLabel);
        gbTotals.Controls.Add(lblTafqitValue);
        gbTotals.Dock = DockStyle.Bottom;
        gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbTotals.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbTotals.Location = new Point(0, 704);
        gbTotals.Name = "gbTotals";
        gbTotals.Size = new Size(1600, 96);
        gbTotals.TabIndex = 5;
        gbTotals.TabStop = false;
        gbTotals.Text = "الإجماليات";
        // 
        // lblItemCountCaption
        // 
        lblItemCountCaption.Font = new Font("Tahoma", 10F);
        lblItemCountCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblItemCountCaption.Location = new Point(1430, 30);
        lblItemCountCaption.Name = "lblItemCountCaption";
        lblItemCountCaption.Size = new Size(120, 24);
        lblItemCountCaption.TabIndex = 0;
        lblItemCountCaption.Text = "عدد الأصناف:";
        lblItemCountCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblItemCountValue
        // 
        lblItemCountValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblItemCountValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblItemCountValue.Location = new Point(1350, 30);
        lblItemCountValue.Name = "lblItemCountValue";
        lblItemCountValue.Size = new Size(70, 24);
        lblItemCountValue.TabIndex = 1;
        lblItemCountValue.Text = "0";
        lblItemCountValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblTotalQtyCaption
        // 
        lblTotalQtyCaption.Font = new Font("Tahoma", 10F);
        lblTotalQtyCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblTotalQtyCaption.Location = new Point(1200, 30);
        lblTotalQtyCaption.Name = "lblTotalQtyCaption";
        lblTotalQtyCaption.Size = new Size(140, 24);
        lblTotalQtyCaption.TabIndex = 2;
        lblTotalQtyCaption.Text = "إجمالي الكميات:";
        lblTotalQtyCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalQtyValue
        // 
        lblTotalQtyValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTotalQtyValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblTotalQtyValue.Location = new Point(1110, 30);
        lblTotalQtyValue.Name = "lblTotalQtyValue";
        lblTotalQtyValue.Size = new Size(80, 24);
        lblTotalQtyValue.TabIndex = 3;
        lblTotalQtyValue.Text = "0.000";
        lblTotalQtyValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblTotalCostCaption
        // 
        lblTotalCostCaption.Font = new Font("Tahoma", 10F);
        lblTotalCostCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblTotalCostCaption.Location = new Point(950, 30);
        lblTotalCostCaption.Name = "lblTotalCostCaption";
        lblTotalCostCaption.Size = new Size(150, 24);
        lblTotalCostCaption.TabIndex = 4;
        lblTotalCostCaption.Text = "إجمالي التكلفة:";
        lblTotalCostCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtTotalCost
        // 
        txtTotalCost.BackColor = System.Drawing.Color.White;
        txtTotalCost.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        txtTotalCost.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtTotalCost.Location = new Point(750, 26);
        txtTotalCost.Name = "txtTotalCost";
        txtTotalCost.ReadOnly = true;
        txtTotalCost.Size = new Size(190, 34);
        txtTotalCost.TabIndex = 5;
        txtTotalCost.TabStop = false;
        txtTotalCost.Text = "0.00";
        txtTotalCost.TextAlign = HorizontalAlignment.Center;
        tooltip.SetToolTip(txtTotalCost, "إجمالي تكلفة المستند (اقترب بالمؤشر لعرض التفقيط)");
        // 
        // lblTafqitLabel
        // 
        lblTafqitLabel.Font = new Font("Tahoma", 10F);
        lblTafqitLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblTafqitLabel.Location = new Point(660, 30);
        lblTafqitLabel.Name = "lblTafqitLabel";
        lblTafqitLabel.Size = new Size(80, 24);
        lblTafqitLabel.TabIndex = 6;
        lblTafqitLabel.Text = "التفقيط:";
        lblTafqitLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTafqitValue
        // 
        lblTafqitValue.BackColor = System.Drawing.Color.White;
        lblTafqitValue.BorderStyle = BorderStyle.FixedSingle;
        lblTafqitValue.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblTafqitValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblTafqitValue.Location = new Point(230, 20);
        lblTafqitValue.Name = "lblTafqitValue";
        lblTafqitValue.Padding = new Padding(4);
        lblTafqitValue.Size = new Size(420, 56);
        lblTafqitValue.TabIndex = 7;
        lblTafqitValue.TextAlign = ContentAlignment.MiddleRight;
        tooltip.SetToolTip(lblTafqitValue, "إجمالي تكلفة المستند كتابةً (تفقيط)");
        // 
        // frmOpeningStock
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        ClientSize = new Size(1600, 800);
        Controls.Add(gbItems);
        Controls.Add(gbTotals);
        Controls.Add(gbEntry);
        Controls.Add(gbInfo);
        Controls.Add(gbToolbar);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
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
        cmsLines.ResumeLayout(false);
        gbToolbar.ResumeLayout(false);
        flpToolbar.ResumeLayout(false);
        gbInfo.ResumeLayout(false);
        gbEntry.ResumeLayout(false);
        pnlEntry.ResumeLayout(false);
        pnlEntry.PerformLayout();
        gbItems.ResumeLayout(false);
        gbTotals.ResumeLayout(false);
        gbTotals.PerformLayout();
        ResumeLayout(false);
    }

    // ═════════════════════════════════════════════════════════════════
    //  حقول الشاشة — كل عنصر دائم معلن هنا (RULE-N145)
    // ═════════════════════════════════════════════════════════════════

    private ErrorProvider errorProvider;
    private ToolTip tooltip;
    private ToolTip liveTip;

    // شريط العنوان
    private Label lblMode;

    // أوامر المستند
    private GroupBox gbToolbar;
    private FlowLayoutPanel flpToolbar;
    private Button btnNew;
    private Button btnSave;
    private Button btnClose;

    // بيانات المستند
    private GroupBox gbInfo;
    private Label lblWarehouse;
    private ComboBox cmbWarehouse;
    private Label lblDate;
    private DateTimePicker dtpDate;

    // إدخال صنف
    private GroupBox gbEntry;
    private Panel pnlEntry;
    private Label lblBarcodeLabel;
    private TextBox txtBarcode;
    private Button btnPickProduct;
    private Label lblProductLabel;
    private Label lblProductName;
    private Label lblUnitLabel;
    private ComboBox cmbUnit;
    private Label lblQtyLabel;
    private TextBox txtQuantity;
    private Label lblCostLabel;
    private TextBox txtUnitCost;
    private Label lblLineTotalHeaderLabel;
    private Label lblLineTotal;
    private Button btnAddLine;
    private Button btnCancelLine;

    // الأصناف
    private GroupBox gbItems;
    private Label lblProductsHeader;
    private DataGridView dgvLines;
    private DataGridViewTextBoxColumn colRowNum;
    private DataGridViewTextBoxColumn colBarcode;
    private DataGridViewTextBoxColumn colProductName;
    private DataGridViewTextBoxColumn colUnit;
    private DataGridViewTextBoxColumn colQuantity;
    private DataGridViewTextBoxColumn colUnitCost;
    private DataGridViewTextBoxColumn colTotalCost;
    private ContextMenuStrip cmsLines;
    private ToolStripMenuItem miEditLine;
    private ToolStripMenuItem miRemoveLine;
    private ToolStripMenuItem miPickProduct;

    // الإجماليات
    private GroupBox gbTotals;
    private Label lblItemCountCaption;
    private Label lblItemCountValue;
    private Label lblTotalQtyCaption;
    private Label lblTotalQtyValue;
    private Label lblTotalCostCaption;
    private TextBox txtTotalCost;
    private Label lblTafqitLabel;
    private Label lblTafqitValue;
}
