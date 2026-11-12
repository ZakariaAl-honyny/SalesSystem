namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmAddUpdateSalesInvoice
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            toolTip1 = new ToolTip(components);
            btnAddLine = new Button();
            btnCancelLine = new Button();
            btnSaveDraft = new Button();
            btnPost = new Button();
            btnCancelInvoice = new Button();
            btnPrintA4 = new Button();
            btnPrintReceipt = new Button();
            chkPrintSummary = new CheckBox();
            btnClose = new Button();
            btnNew = new Button();
            btnSalesReturn = new Button();
            btnEditRow = new Button();
            btnDeleteRow = new Button();
            btnRefreshCatalog = new Button();
            errorProvider = new ErrorProvider(components);
            tooltip = new ToolTip(components);
            txtNotes = new TextBox();
            txtBarcode = new TextBox();
            txtItemCode = new TextBox();
            lblProductName = new Label();
            cmbUnit = new ComboBox();
            txtQuantity = new TextBox();
            txtUnitPrice = new TextBox();
            lblLineTotal = new Label();
            txtLineDiscount = new TextBox();
            dtpLineExpiry = new DateTimePicker();
            cmbCashBox = new ComboBox();
            cmbDiscountType = new ComboBox();
            lblTafqitValue = new Label();
            lblAvailable = new Label();
            liveTip = new ToolTip(components);
            lblNotes = new Label();
            lblCashBox = new Label();
            lblTafqitLabel = new Label();
            gbInvoiceInfo = new GroupBox();
            rbtnPaymentTypeCredit = new RadioButton();
            rbtnPaymentTypeCash = new RadioButton();
            lblCustomer = new Label();
            cmbCustomer = new ComboBox();
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            lblPaymentType = new Label();
            lblInvoiceNoLabel = new Label();
            lblDate = new Label();
            lblInvoiceNo = new Label();
            dtpDate = new DateTimePicker();
            lblStatus = new Label();
            lblExpensesLabel = new Label();
            txtExpenses = new TextBox();
            cmbAccount = new ComboBox();
            gbEntry = new GroupBox();
            pnlEntry = new Panel();
            lblBarcodeLabel = new Label();
            lblItemCodeLabel = new Label();
            lblProductLabel = new Label();
            lblUnitLabel = new Label();
            lblQtyLabel = new Label();
            lblPriceLabel = new Label();
            lblDiscountLabel = new Label();
            lblLineExpiryLabel = new Label();
            lblTotalHeaderLabel = new Label();
            gbItems = new GroupBox();
            dgvItems = new DataGridView();
            colDelete = new DataGridViewImageColumn();
            colBarcode = new DataGridViewTextBoxColumn();
            colItemCode = new DataGridViewTextBoxColumn();
            colProductName = new DataGridViewTextBoxColumn();
            colUnit = new DataGridViewComboBoxColumn();
            colQuantity = new DataGridViewTextBoxColumn();
            colUnitPrice = new DataGridViewTextBoxColumn();
            colDiscount = new DataGridViewTextBoxColumn();
            colLineTotal = new DataGridViewTextBoxColumn();
            colExpiry = new DataGridViewTextBoxColumn();
            colRowNum = new DataGridViewTextBoxColumn();
            cmsGridContext = new ContextMenuStrip(components);
            tsmiEditRow = new ToolStripMenuItem();
            tsmiCancelEdit = new ToolStripMenuItem();
            tsmiDeleteRow = new ToolStripMenuItem();
            gbItemActions = new GroupBox();
            flpItemActions = new FlowLayoutPanel();
            lblLockIndicator = new Label();
            gbTotals = new GroupBox();
            label1 = new Label();
            numInvoiceDiscount = new NumericUpDown();
            lblPaid = new Label();
            txtPaid = new SalesSystem.Desktop.Controls.Common.MoneyTextBox();
            lblQuickInfo = new Label();
            gbToolbar = new GroupBox();
            flpToolbar = new FlowLayoutPanel();
            lblMode = new Label();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            gbInvoiceInfo.SuspendLayout();
            gbEntry.SuspendLayout();
            pnlEntry.SuspendLayout();
            gbItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            cmsGridContext.SuspendLayout();
            gbItemActions.SuspendLayout();
            flpItemActions.SuspendLayout();
            gbTotals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numInvoiceDiscount).BeginInit();
            gbToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 12000;
            toolTip1.InitialDelay = 250;
            toolTip1.ReshowDelay = 100;
            // 
            // btnAddLine
            // 
            btnAddLine.BackColor = Color.FromArgb(25, 135, 84);
            btnAddLine.FlatAppearance.BorderSize = 0;
            btnAddLine.FlatStyle = FlatStyle.Flat;
            btnAddLine.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddLine.ForeColor = Color.White;
            btnAddLine.Location = new Point(1463, 3);
            btnAddLine.Name = "btnAddLine";
            btnAddLine.Size = new Size(110, 36);
            btnAddLine.TabIndex = 0;
            btnAddLine.Text = "➕ إضافة";
            toolTip1.SetToolTip(btnAddLine, "إضافة الصنف المدخل إلى الجدول (Enter)");
            btnAddLine.UseVisualStyleBackColor = false;
            btnAddLine.Click += btnAddLine_Click;
            // 
            // btnCancelLine
            // 
            btnCancelLine.BackColor = Color.FromArgb(108, 117, 125);
            btnCancelLine.FlatAppearance.BorderSize = 0;
            btnCancelLine.FlatStyle = FlatStyle.Flat;
            btnCancelLine.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelLine.ForeColor = Color.White;
            btnCancelLine.Location = new Point(847, 3);
            btnCancelLine.Name = "btnCancelLine";
            btnCancelLine.Size = new Size(197, 36);
            btnCancelLine.TabIndex = 3;
            btnCancelLine.Text = "✖ إلغاء التعديل";
            toolTip1.SetToolTip(btnCancelLine, "إلغاء وضع التعديل والعودة للإضافة");
            btnCancelLine.UseVisualStyleBackColor = false;
            btnCancelLine.Click += btnCancelLine_Click;
            // 
            // btnSaveDraft
            // 
            btnSaveDraft.BackColor = Color.FromArgb(25, 135, 84);
            btnSaveDraft.FlatAppearance.BorderSize = 0;
            btnSaveDraft.FlatStyle = FlatStyle.Flat;
            btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSaveDraft.ForeColor = Color.White;
            btnSaveDraft.Location = new Point(1190, 3);
            btnSaveDraft.Name = "btnSaveDraft";
            btnSaveDraft.Size = new Size(199, 36);
            btnSaveDraft.TabIndex = 1;
            btnSaveDraft.Text = "💾 حفظ كمسودة";
            toolTip1.SetToolTip(btnSaveDraft, "حفظ كمسودة: يحفظ الفاتورة دون ترحيلها.\nلا يخصم من المخزون ولا يُرحّل أي قيد محاسبي،\nويمكن تعديلها أو إلغاؤها لاحقاً.");
            btnSaveDraft.UseVisualStyleBackColor = false;
            btnSaveDraft.Click += btnSaveDraft_Click;
            // 
            // btnPost
            // 
            btnPost.BackColor = Color.FromArgb(13, 110, 253);
            btnPost.FlatAppearance.BorderSize = 0;
            btnPost.FlatStyle = FlatStyle.Flat;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPost.ForeColor = Color.White;
            btnPost.Location = new Point(1001, 3);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(183, 36);
            btnPost.TabIndex = 2;
            btnPost.Text = "✅ حفظ وترحيل (F5)";
            toolTip1.SetToolTip(btnPost, "حفظ وترحيل (F5): يحفظ الفاتورة ويُرحّلها فوراً.\nيخصم الكميات من المخزن ويُرحّل القيد المحاسبي،\nوبعدها تُقفل الفاتورة نهائياً ولا يمكن تعديلها.");
            btnPost.UseVisualStyleBackColor = false;
            btnPost.Click += btnPost_Click;
            // 
            // btnCancelInvoice
            // 
            btnCancelInvoice.BackColor = Color.FromArgb(220, 53, 69);
            btnCancelInvoice.FlatAppearance.BorderSize = 0;
            btnCancelInvoice.FlatStyle = FlatStyle.Flat;
            btnCancelInvoice.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelInvoice.ForeColor = Color.White;
            btnCancelInvoice.Location = new Point(212, 3);
            btnCancelInvoice.Name = "btnCancelInvoice";
            btnCancelInvoice.Size = new Size(204, 36);
            btnCancelInvoice.TabIndex = 6;
            btnCancelInvoice.Text = "🚫 إلغاء الفاتورة";
            toolTip1.SetToolTip(btnCancelInvoice, "إلغاء الفاتورة المرحّلة (تُعلَّم كملغاة ولا تُحذف)");
            btnCancelInvoice.UseVisualStyleBackColor = false;
            btnCancelInvoice.Click += btnCancelInvoice_Click;
            // 
            // btnPrintA4
            // 
            btnPrintA4.BackColor = Color.FromArgb(73, 80, 87);
            btnPrintA4.FlatAppearance.BorderSize = 0;
            btnPrintA4.FlatStyle = FlatStyle.Flat;
            btnPrintA4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrintA4.ForeColor = Color.White;
            btnPrintA4.Location = new Point(915, 3);
            btnPrintA4.Name = "btnPrintA4";
            btnPrintA4.Size = new Size(80, 36);
            btnPrintA4.TabIndex = 3;
            btnPrintA4.Text = "🖨 A4";
            toolTip1.SetToolTip(btnPrintA4, "معاينة/طباعة الفاتورة A4");
            btnPrintA4.UseVisualStyleBackColor = false;
            btnPrintA4.Click += btnPrintA4_Click;
            // 
            // btnPrintReceipt
            // 
            btnPrintReceipt.BackColor = Color.FromArgb(108, 117, 125);
            btnPrintReceipt.FlatAppearance.BorderSize = 0;
            btnPrintReceipt.FlatStyle = FlatStyle.Flat;
            btnPrintReceipt.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrintReceipt.ForeColor = Color.White;
            btnPrintReceipt.Location = new Point(793, 3);
            btnPrintReceipt.Name = "btnPrintReceipt";
            btnPrintReceipt.Size = new Size(116, 36);
            btnPrintReceipt.TabIndex = 4;
            btnPrintReceipt.Text = "\U0001f9fe إيصال";
            toolTip1.SetToolTip(btnPrintReceipt, "طباعة إيصال حراري");
            btnPrintReceipt.UseVisualStyleBackColor = false;
            btnPrintReceipt.Click += btnPrintReceipt_Click;
            // 
            // chkPrintSummary
            // 
            chkPrintSummary.CheckAlign = ContentAlignment.MiddleRight;
            chkPrintSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkPrintSummary.ForeColor = Color.FromArgb(33, 37, 41);
            chkPrintSummary.Location = new Point(640, 3);
            chkPrintSummary.Name = "chkPrintSummary";
            chkPrintSummary.Size = new Size(147, 36);
            chkPrintSummary.TabIndex = 5;
            chkPrintSummary.Text = "طباعة فاتورة إجمالية";
            toolTip1.SetToolTip(chkPrintSummary, "طباعة نسخة إجمالية مختصرة (صنف/وحدة/كمية فقط)");
            chkPrintSummary.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(106, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 36);
            btnClose.TabIndex = 4;
            btnClose.Text = "✖ خروج";
            toolTip1.SetToolTip(btnClose, "إغلاق الشاشة بدون حفظ");
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnNew
            // 
            btnNew.BackColor = Color.FromArgb(40, 167, 69);
            btnNew.FlatAppearance.BorderSize = 0;
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNew.ForeColor = Color.White;
            btnNew.Location = new Point(1395, 3);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(184, 36);
            btnNew.TabIndex = 0;
            btnNew.Text = "➕ فاتورة جديدة";
            toolTip1.SetToolTip(btnNew, "فاتورة جديدة فارغة (F2)");
            btnNew.UseVisualStyleBackColor = false;
            btnNew.Click += btnNew_Click;
            // 
            // btnSalesReturn
            // 
            btnSalesReturn.BackColor = Color.FromArgb(111, 66, 193);
            btnSalesReturn.FlatAppearance.BorderSize = 0;
            btnSalesReturn.FlatStyle = FlatStyle.Flat;
            btnSalesReturn.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSalesReturn.ForeColor = Color.White;
            btnSalesReturn.Location = new Point(422, 3);
            btnSalesReturn.Name = "btnSalesReturn";
            btnSalesReturn.Size = new Size(212, 36);
            btnSalesReturn.TabIndex = 5;
            btnSalesReturn.Text = "↩ فاتورة مردود بيع";
            toolTip1.SetToolTip(btnSalesReturn, "فتح شاشة مردود بيع مباشر");
            btnSalesReturn.UseVisualStyleBackColor = false;
            btnSalesReturn.Click += btnSalesReturn_Click;
            // 
            // btnEditRow
            // 
            btnEditRow.BackColor = Color.FromArgb(255, 193, 7);
            btnEditRow.FlatAppearance.BorderSize = 0;
            btnEditRow.FlatStyle = FlatStyle.Flat;
            btnEditRow.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEditRow.ForeColor = Color.FromArgb(60, 45, 5);
            btnEditRow.Location = new Point(1347, 3);
            btnEditRow.Name = "btnEditRow";
            btnEditRow.Size = new Size(110, 36);
            btnEditRow.TabIndex = 1;
            btnEditRow.Text = "✏ تعديل";
            toolTip1.SetToolTip(btnEditRow, "تعديل السطر المحدد في لوحة الإدخال");
            btnEditRow.UseVisualStyleBackColor = false;
            btnEditRow.Click += btnEditRow_Click;
            // 
            // btnDeleteRow
            // 
            btnDeleteRow.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteRow.FlatAppearance.BorderSize = 0;
            btnDeleteRow.FlatStyle = FlatStyle.Flat;
            btnDeleteRow.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDeleteRow.ForeColor = Color.White;
            btnDeleteRow.Location = new Point(1231, 3);
            btnDeleteRow.Name = "btnDeleteRow";
            btnDeleteRow.Size = new Size(110, 36);
            btnDeleteRow.TabIndex = 2;
            btnDeleteRow.Text = "🗑 حذف";
            toolTip1.SetToolTip(btnDeleteRow, "حذف السطر المحدد (Delete)");
            btnDeleteRow.UseVisualStyleBackColor = false;
            btnDeleteRow.Click += btnDeleteRow_Click;
            // 
            // btnRefreshCatalog
            // 
            btnRefreshCatalog.BackColor = Color.FromArgb(108, 117, 125);
            btnRefreshCatalog.FlatAppearance.BorderSize = 0;
            btnRefreshCatalog.FlatStyle = FlatStyle.Flat;
            btnRefreshCatalog.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefreshCatalog.ForeColor = Color.White;
            btnRefreshCatalog.Location = new Point(1050, 3);
            btnRefreshCatalog.Name = "btnRefreshCatalog";
            btnRefreshCatalog.Size = new Size(175, 36);
            btnRefreshCatalog.TabIndex = 7;
            btnRefreshCatalog.Text = "🔄 تحديث بيانات الأصناف";
            toolTip1.SetToolTip(btnRefreshCatalog, "إعادة تحميل بيانات الأصناف والوحدات والأسعار والأرصدة");
            btnRefreshCatalog.UseVisualStyleBackColor = false;
            btnRefreshCatalog.Click += btnRefreshCatalog_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // tooltip
            // 
            tooltip.IsBalloon = true;
            tooltip.ToolTipIcon = ToolTipIcon.Info;
            tooltip.ToolTipTitle = "مساعدة";
            // 
            // txtNotes
            // 
            txtNotes.BackColor = Color.White;
            txtNotes.BorderStyle = BorderStyle.FixedSingle;
            txtNotes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNotes.Location = new Point(280, 112);
            txtNotes.MaxLength = 500;
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.PlaceholderText = "ملاحظات...";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(1230, 52);
            txtNotes.TabIndex = 14;
            tooltip.SetToolTip(txtNotes, "إدخال ملاحظات إضافية على الفاتورة");
            txtNotes.Validating += _ValidateEmptyTextBox;
            // 
            // txtBarcode
            // 
            txtBarcode.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtBarcode.Location = new Point(1324, 33);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(260, 34);
            txtBarcode.TabIndex = 7;
            txtBarcode.TextAlign = HorizontalAlignment.Center;
            tooltip.SetToolTip(txtBarcode, "مسح الباركود أو إدخال كود المنتج");
            // 
            // txtItemCode
            // 
            txtItemCode.BackColor = Color.FromArgb(240, 240, 240);
            txtItemCode.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtItemCode.Location = new Point(1210, 31);
            txtItemCode.Name = "txtItemCode";
            txtItemCode.ReadOnly = true;
            txtItemCode.Size = new Size(104, 34);
            txtItemCode.TabIndex = 14;
            txtItemCode.TabStop = false;
            txtItemCode.TextAlign = HorizontalAlignment.Center;
            tooltip.SetToolTip(txtItemCode, "رقم الصنف (رقم المنتج)");
            // 
            // lblProductName
            // 
            lblProductName.BackColor = Color.White;
            lblProductName.BorderStyle = BorderStyle.FixedSingle;
            lblProductName.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProductName.Location = new Point(930, 32);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(270, 34);
            lblProductName.TabIndex = 8;
            lblProductName.TextAlign = ContentAlignment.MiddleCenter;
            tooltip.SetToolTip(lblProductName, "اسم المنتج المحدد");
            // 
            // cmbUnit
            // 
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnit.FlatStyle = FlatStyle.Flat;
            cmbUnit.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbUnit.Location = new Point(786, 32);
            cmbUnit.Name = "cmbUnit";
            cmbUnit.Size = new Size(134, 35);
            cmbUnit.TabIndex = 9;
            tooltip.SetToolTip(cmbUnit, "اختيار وحدة القياس");
            // 
            // txtQuantity
            // 
            txtQuantity.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtQuantity.Location = new Point(660, 31);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(116, 34);
            txtQuantity.TabIndex = 10;
            txtQuantity.Text = "1";
            txtQuantity.TextAlign = HorizontalAlignment.Center;
            tooltip.SetToolTip(txtQuantity, "الكمية المطلوبة");
            // 
            // txtUnitPrice
            // 
            txtUnitPrice.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUnitPrice.Location = new Point(534, 31);
            txtUnitPrice.Name = "txtUnitPrice";
            txtUnitPrice.Size = new Size(116, 34);
            txtUnitPrice.TabIndex = 11;
            txtUnitPrice.Text = "0.00";
            txtUnitPrice.TextAlign = HorizontalAlignment.Center;
            tooltip.SetToolTip(txtUnitPrice, "سعر الوحدة");
            // 
            // lblLineTotal
            // 
            lblLineTotal.BackColor = Color.White;
            lblLineTotal.BorderStyle = BorderStyle.FixedSingle;
            lblLineTotal.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLineTotal.ForeColor = Color.FromArgb(25, 135, 84);
            lblLineTotal.Location = new Point(63, 31);
            lblLineTotal.Name = "lblLineTotal";
            lblLineTotal.Size = new Size(142, 34);
            lblLineTotal.TabIndex = 13;
            lblLineTotal.Text = "0.00";
            lblLineTotal.TextAlign = ContentAlignment.MiddleCenter;
            tooltip.SetToolTip(lblLineTotal, "إجمالي سعر هذا الصنف");
            // 
            // txtLineDiscount
            // 
            txtLineDiscount.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtLineDiscount.Location = new Point(418, 31);
            txtLineDiscount.Name = "txtLineDiscount";
            txtLineDiscount.Size = new Size(106, 34);
            txtLineDiscount.TabIndex = 12;
            txtLineDiscount.Text = "0";
            txtLineDiscount.TextAlign = HorizontalAlignment.Center;
            tooltip.SetToolTip(txtLineDiscount, "الخصم المسموح به للصنف (مبلغ ثابت)");
            // 
            // dtpLineExpiry
            // 
            dtpLineExpiry.CalendarMonthBackground = Color.White;
            dtpLineExpiry.CustomFormat = "yyyy/MM/dd";
            dtpLineExpiry.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dtpLineExpiry.Format = DateTimePickerFormat.Custom;
            dtpLineExpiry.Location = new Point(211, 33);
            dtpLineExpiry.Name = "dtpLineExpiry";
            dtpLineExpiry.ShowCheckBox = true;
            dtpLineExpiry.Size = new Size(197, 32);
            dtpLineExpiry.TabIndex = 13;
            tooltip.SetToolTip(dtpLineExpiry, "تاريخ انتهاء صلاحية هذا الصنف — غير محدد = بدون تاريخ انتهاء");
            // 
            // cmbCashBox
            // 
            cmbCashBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCashBox.Location = new Point(215, 31);
            cmbCashBox.Name = "cmbCashBox";
            cmbCashBox.Size = new Size(230, 32);
            cmbCashBox.TabIndex = 18;
            tooltip.SetToolTip(cmbCashBox, "الصندوق الذي تُقبض فيه نقدية الفاتورة");
            // 
            // cmbDiscountType
            // 
            cmbDiscountType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDiscountType.Items.AddRange(new object[] { "مبلغ", "نسبة" });
            cmbDiscountType.Location = new Point(1110, 27);
            cmbDiscountType.Name = "cmbDiscountType";
            cmbDiscountType.Size = new Size(205, 32);
            cmbDiscountType.TabIndex = 6;
            tooltip.SetToolTip(cmbDiscountType, "نوع الخصم: مبلغ ثابت أو نسبة من الإجمالي");
            // 
            // lblTafqitValue
            // 
            lblTafqitValue.BackColor = Color.White;
            lblTafqitValue.BorderStyle = BorderStyle.FixedSingle;
            lblTafqitValue.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTafqitValue.ForeColor = Color.FromArgb(73, 80, 87);
            lblTafqitValue.Location = new Point(143, 45);
            lblTafqitValue.Name = "lblTafqitValue";
            lblTafqitValue.Padding = new Padding(4);
            lblTafqitValue.Size = new Size(380, 56);
            lblTafqitValue.TabIndex = 12;
            lblTafqitValue.TextAlign = ContentAlignment.MiddleRight;
            tooltip.SetToolTip(lblTafqitValue, "إجمالي الفاتورة كتابةً (تفقيط)");
            // 
            // lblAvailable
            // 
            lblAvailable.BackColor = Color.FromArgb(233, 245, 233);
            lblAvailable.BorderStyle = BorderStyle.FixedSingle;
            lblAvailable.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAvailable.ForeColor = Color.FromArgb(25, 135, 84);
            lblAvailable.Location = new Point(588, 66);
            lblAvailable.Name = "lblAvailable";
            lblAvailable.Padding = new Padding(4, 0, 4, 0);
            lblAvailable.Size = new Size(430, 26);
            lblAvailable.TabIndex = 13;
            lblAvailable.Text = "المتاح: —";
            lblAvailable.TextAlign = ContentAlignment.MiddleCenter;
            tooltip.SetToolTip(lblAvailable, "الكمية المتاحة في المستودع للوحدة المختارة");
            // 
            // liveTip
            // 
            liveTip.AutoPopDelay = 4000;
            liveTip.InitialDelay = 100;
            liveTip.ReshowDelay = 50;
            liveTip.ShowAlways = true;
            // 
            // lblNotes
            // 
            lblNotes.BackColor = Color.Transparent;
            lblNotes.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNotes.Location = new Point(1520, 118);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(64, 24);
            lblNotes.TabIndex = 15;
            lblNotes.Text = "ملاحظات:";
            lblNotes.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCashBox
            // 
            lblCashBox.AutoSize = true;
            lblCashBox.Location = new Point(455, 33);
            lblCashBox.Name = "lblCashBox";
            lblCashBox.Size = new Size(101, 24);
            lblCashBox.TabIndex = 17;
            lblCashBox.Text = "الصندوق:";
            lblCashBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTafqitLabel
            // 
            lblTafqitLabel.AutoSize = true;
            lblTafqitLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTafqitLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblTafqitLabel.Location = new Point(538, 62);
            lblTafqitLabel.Name = "lblTafqitLabel";
            lblTafqitLabel.Size = new Size(79, 22);
            lblTafqitLabel.TabIndex = 11;
            lblTafqitLabel.Text = "التفقيط:";
            lblTafqitLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // gbInvoiceInfo
            // 
            gbInvoiceInfo.BackColor = Color.FromArgb(235, 245, 235);
            gbInvoiceInfo.Controls.Add(rbtnPaymentTypeCredit);
            gbInvoiceInfo.Controls.Add(rbtnPaymentTypeCash);
            gbInvoiceInfo.Controls.Add(lblCustomer);
            gbInvoiceInfo.Controls.Add(cmbCustomer);
            gbInvoiceInfo.Controls.Add(lblWarehouse);
            gbInvoiceInfo.Controls.Add(cmbWarehouse);
            gbInvoiceInfo.Controls.Add(lblCashBox);
            gbInvoiceInfo.Controls.Add(cmbCashBox);
            gbInvoiceInfo.Controls.Add(lblPaymentType);
            gbInvoiceInfo.Controls.Add(lblInvoiceNoLabel);
            gbInvoiceInfo.Controls.Add(lblDate);
            gbInvoiceInfo.Controls.Add(lblInvoiceNo);
            gbInvoiceInfo.Controls.Add(dtpDate);
            gbInvoiceInfo.Controls.Add(lblStatus);
            gbInvoiceInfo.Controls.Add(txtNotes);
            gbInvoiceInfo.Controls.Add(lblNotes);
            gbInvoiceInfo.Dock = DockStyle.Top;
            gbInvoiceInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbInvoiceInfo.Location = new Point(0, 39);
            gbInvoiceInfo.Name = "gbInvoiceInfo";
            gbInvoiceInfo.Size = new Size(1600, 175);
            gbInvoiceInfo.TabIndex = 0;
            gbInvoiceInfo.TabStop = false;
            gbInvoiceInfo.Text = "معلومات الفاتورة";
            // 
            // rbtnPaymentTypeCredit
            // 
            rbtnPaymentTypeCredit.AutoSize = true;
            rbtnPaymentTypeCredit.Location = new Point(573, 33);
            rbtnPaymentTypeCredit.Name = "rbtnPaymentTypeCredit";
            rbtnPaymentTypeCredit.Size = new Size(72, 28);
            rbtnPaymentTypeCredit.TabIndex = 16;
            rbtnPaymentTypeCredit.Text = "اجل";
            rbtnPaymentTypeCredit.UseVisualStyleBackColor = true;
            // 
            // rbtnPaymentTypeCash
            // 
            rbtnPaymentTypeCash.AutoSize = true;
            rbtnPaymentTypeCash.Checked = true;
            rbtnPaymentTypeCash.Location = new Point(651, 32);
            rbtnPaymentTypeCash.Name = "rbtnPaymentTypeCash";
            rbtnPaymentTypeCash.Size = new Size(65, 28);
            rbtnPaymentTypeCash.TabIndex = 15;
            rbtnPaymentTypeCash.TabStop = true;
            rbtnPaymentTypeCash.Text = "نقد";
            rbtnPaymentTypeCash.UseVisualStyleBackColor = true;
            // 
            // lblCustomer
            // 
            lblCustomer.AutoSize = true;
            lblCustomer.Location = new Point(1454, 78);
            lblCustomer.Name = "lblCustomer";
            lblCustomer.Size = new Size(79, 24);
            lblCustomer.TabIndex = 5;
            lblCustomer.Text = "العميل:";
            lblCustomer.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbCustomer
            // 
            cmbCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbCustomer.Location = new Point(1178, 75);
            cmbCustomer.Name = "cmbCustomer";
            cmbCustomer.Size = new Size(270, 32);
            cmbCustomer.TabIndex = 6;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Location = new Point(1060, 33);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(110, 24);
            lblWarehouse.TabIndex = 7;
            lblWarehouse.Text = "المستودع:";
            lblWarehouse.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.Location = new Point(845, 31);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(195, 32);
            cmbWarehouse.TabIndex = 8;
            // 
            // lblPaymentType
            // 
            lblPaymentType.AutoSize = true;
            lblPaymentType.Location = new Point(722, 31);
            lblPaymentType.Name = "lblPaymentType";
            lblPaymentType.Size = new Size(105, 24);
            lblPaymentType.TabIndex = 9;
            lblPaymentType.Text = "نوع الدفع:";
            lblPaymentType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblInvoiceNoLabel
            // 
            lblInvoiceNoLabel.AutoSize = true;
            lblInvoiceNoLabel.Location = new Point(1454, 38);
            lblInvoiceNoLabel.Name = "lblInvoiceNoLabel";
            lblInvoiceNoLabel.Size = new Size(127, 24);
            lblInvoiceNoLabel.TabIndex = 0;
            lblInvoiceNoLabel.Text = "رقم الفاتورة:";
            lblInvoiceNoLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Location = new Point(1060, 78);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(77, 24);
            lblDate.TabIndex = 2;
            lblDate.Text = "التاريخ:";
            lblDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblInvoiceNo
            // 
            lblInvoiceNo.AutoSize = true;
            lblInvoiceNo.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInvoiceNo.ForeColor = Color.FromArgb(13, 110, 253);
            lblInvoiceNo.Location = new Point(1361, 34);
            lblInvoiceNo.Name = "lblInvoiceNo";
            lblInvoiceNo.Size = new Size(87, 29);
            lblInvoiceNo.TabIndex = 1;
            lblInvoiceNo.Text = "تلقائي";
            lblInvoiceNo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpDate
            // 
            dtpDate.CustomFormat = "yyyy/MM/dd";
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.Location = new Point(845, 75);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(195, 32);
            dtpDate.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.BackColor = Color.FromArgb(13, 110, 253);
            lblStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(636, 76);
            lblStatus.MinimumSize = new Size(80, 24);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(4, 2, 4, 2);
            lblStatus.Size = new Size(80, 28);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "جديد";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblExpensesLabel
            // 
            lblExpensesLabel.AutoSize = true;
            lblExpensesLabel.Location = new Point(1321, 69);
            lblExpensesLabel.Name = "lblExpensesLabel";
            lblExpensesLabel.Size = new Size(107, 24);
            lblExpensesLabel.TabIndex = 5;
            lblExpensesLabel.Text = "المصاريف:";
            lblExpensesLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtExpenses
            // 
            txtExpenses.Location = new Point(926, 64);
            txtExpenses.Name = "txtExpenses";
            txtExpenses.Size = new Size(178, 32);
            txtExpenses.TabIndex = 6;
            txtExpenses.Text = "0.00";
            txtExpenses.TextAlign = HorizontalAlignment.Center;
            txtExpenses.TextChanged += OnCalculateTotals;
            // 
            // cmbAccount
            // 
            cmbAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAccount.Location = new Point(1110, 65);
            cmbAccount.Name = "cmbAccount";
            cmbAccount.Size = new Size(205, 32);
            cmbAccount.TabIndex = 8;
            // 
            // gbEntry
            // 
            gbEntry.BackColor = Color.FromArgb(235, 245, 235);
            gbEntry.Controls.Add(pnlEntry);
            gbEntry.Dock = DockStyle.Top;
            gbEntry.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbEntry.Location = new Point(0, 297);
            gbEntry.Name = "gbEntry";
            gbEntry.Size = new Size(1600, 125);
            gbEntry.TabIndex = 15;
            gbEntry.TabStop = false;
            gbEntry.Text = "إدخال الأصناف";
            // 
            // pnlEntry
            // 
            pnlEntry.BackColor = Color.FromArgb(235, 245, 235);
            pnlEntry.BorderStyle = BorderStyle.FixedSingle;
            pnlEntry.Controls.Add(lblBarcodeLabel);
            pnlEntry.Controls.Add(lblItemCodeLabel);
            pnlEntry.Controls.Add(lblProductLabel);
            pnlEntry.Controls.Add(lblUnitLabel);
            pnlEntry.Controls.Add(lblQtyLabel);
            pnlEntry.Controls.Add(lblPriceLabel);
            pnlEntry.Controls.Add(lblDiscountLabel);
            pnlEntry.Controls.Add(lblLineExpiryLabel);
            pnlEntry.Controls.Add(lblTotalHeaderLabel);
            pnlEntry.Controls.Add(lblAvailable);
            pnlEntry.Controls.Add(txtBarcode);
            pnlEntry.Controls.Add(txtItemCode);
            pnlEntry.Controls.Add(lblProductName);
            pnlEntry.Controls.Add(cmbUnit);
            pnlEntry.Controls.Add(txtQuantity);
            pnlEntry.Controls.Add(txtUnitPrice);
            pnlEntry.Controls.Add(txtLineDiscount);
            pnlEntry.Controls.Add(dtpLineExpiry);
            pnlEntry.Controls.Add(lblLineTotal);
            pnlEntry.Dock = DockStyle.Fill;
            pnlEntry.Location = new Point(3, 28);
            pnlEntry.Name = "pnlEntry";
            pnlEntry.Size = new Size(1594, 94);
            pnlEntry.TabIndex = 15;
            pnlEntry.Paint += pnlEntry_Paint;
            // 
            // lblBarcodeLabel
            // 
            lblBarcodeLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBarcodeLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblBarcodeLabel.Location = new Point(1324, 6);
            lblBarcodeLabel.Name = "lblBarcodeLabel";
            lblBarcodeLabel.Size = new Size(260, 22);
            lblBarcodeLabel.TabIndex = 0;
            lblBarcodeLabel.Text = "الباركود";
            lblBarcodeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblItemCodeLabel
            // 
            lblItemCodeLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblItemCodeLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblItemCodeLabel.Location = new Point(1210, 6);
            lblItemCodeLabel.Name = "lblItemCodeLabel";
            lblItemCodeLabel.Size = new Size(104, 22);
            lblItemCodeLabel.TabIndex = 1;
            lblItemCodeLabel.Text = "رقم الصنف";
            lblItemCodeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblProductLabel
            // 
            lblProductLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProductLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblProductLabel.Location = new Point(930, 6);
            lblProductLabel.Name = "lblProductLabel";
            lblProductLabel.Size = new Size(270, 22);
            lblProductLabel.TabIndex = 1;
            lblProductLabel.Text = "المنتج";
            lblProductLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUnitLabel
            // 
            lblUnitLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUnitLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblUnitLabel.Location = new Point(786, 6);
            lblUnitLabel.Name = "lblUnitLabel";
            lblUnitLabel.Size = new Size(134, 22);
            lblUnitLabel.TabIndex = 2;
            lblUnitLabel.Text = "الوحدة";
            lblUnitLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblQtyLabel
            // 
            lblQtyLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblQtyLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblQtyLabel.Location = new Point(660, 6);
            lblQtyLabel.Name = "lblQtyLabel";
            lblQtyLabel.Size = new Size(116, 22);
            lblQtyLabel.TabIndex = 3;
            lblQtyLabel.Text = "الكمية";
            lblQtyLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPriceLabel
            // 
            lblPriceLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPriceLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblPriceLabel.Location = new Point(534, 6);
            lblPriceLabel.Name = "lblPriceLabel";
            lblPriceLabel.Size = new Size(116, 22);
            lblPriceLabel.TabIndex = 4;
            lblPriceLabel.Text = "السعر";
            lblPriceLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDiscountLabel
            // 
            lblDiscountLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDiscountLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblDiscountLabel.Location = new Point(418, 6);
            lblDiscountLabel.Name = "lblDiscountLabel";
            lblDiscountLabel.Size = new Size(106, 22);
            lblDiscountLabel.TabIndex = 5;
            lblDiscountLabel.Text = "الخصم";
            lblDiscountLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblLineExpiryLabel
            // 
            lblLineExpiryLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLineExpiryLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblLineExpiryLabel.Location = new Point(262, 6);
            lblLineExpiryLabel.Name = "lblLineExpiryLabel";
            lblLineExpiryLabel.Size = new Size(146, 22);
            lblLineExpiryLabel.TabIndex = 7;
            lblLineExpiryLabel.Text = "تاريخ الانتهاء";
            lblLineExpiryLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalHeaderLabel
            // 
            lblTotalHeaderLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalHeaderLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblTotalHeaderLabel.Location = new Point(63, 6);
            lblTotalHeaderLabel.Name = "lblTotalHeaderLabel";
            lblTotalHeaderLabel.Size = new Size(142, 22);
            lblTotalHeaderLabel.TabIndex = 6;
            lblTotalHeaderLabel.Text = "الإجمالي";
            lblTotalHeaderLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbItems
            // 
            gbItems.BackColor = Color.FromArgb(235, 245, 235);
            gbItems.Controls.Add(dgvItems);
            gbItems.Controls.Add(gbItemActions);
            gbItems.Dock = DockStyle.Fill;
            gbItems.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbItems.Location = new Point(0, 422);
            gbItems.Name = "gbItems";
            gbItems.Size = new Size(1600, 319);
            gbItems.TabIndex = 16;
            gbItems.TabStop = false;
            gbItems.Text = "الأصناف";
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.AllowUserToResizeRows = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvItems.BorderStyle = BorderStyle.None;
            dgvItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(14, 165, 103);
            dataGridViewCellStyle1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvItems.ColumnHeadersHeight = 45;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvItems.Columns.AddRange(new DataGridViewColumn[] { colDelete, colBarcode, colItemCode, colProductName, colUnit, colQuantity, colUnitPrice, colDiscount, colLineTotal, colExpiry, colRowNum });
            dgvItems.ContextMenuStrip = cmsGridContext;
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = Color.White;
            dataGridViewCellStyle12.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle12.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle12.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dataGridViewCellStyle12.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.False;
            dgvItems.DefaultCellStyle = dataGridViewCellStyle12;
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.GridColor = Color.FromArgb(222, 226, 230);
            dgvItems.Location = new Point(3, 98);
            dgvItems.MultiSelect = false;
            dgvItems.Name = "dgvItems";
            dgvItems.RowHeadersVisible = false;
            dgvItems.RowHeadersWidth = 62;
            dgvItems.RowTemplate.Height = 32;
            dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvItems.Size = new Size(1594, 218);
            dgvItems.TabIndex = 16;
            // 
            // colDelete
            // 
            colDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDelete.HeaderText = "";
            colDelete.MinimumWidth = 8;
            colDelete.Name = "colDelete";
            colDelete.ReadOnly = true;
            colDelete.Width = 40;
            // 
            // colBarcode
            // 
            colBarcode.DataPropertyName = "Barcode";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colBarcode.DefaultCellStyle = dataGridViewCellStyle2;
            colBarcode.HeaderText = "الباركود";
            colBarcode.MinimumWidth = 8;
            colBarcode.Name = "colBarcode";
            // 
            // colItemCode
            // 
            colItemCode.DataPropertyName = "ProductId";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colItemCode.DefaultCellStyle = dataGridViewCellStyle3;
            colItemCode.HeaderText = "رقم الصنف";
            colItemCode.MinimumWidth = 8;
            colItemCode.Name = "colItemCode";
            colItemCode.ReadOnly = true;
            // 
            // colProductName
            // 
            colProductName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colProductName.DataPropertyName = "ProductName";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colProductName.DefaultCellStyle = dataGridViewCellStyle4;
            colProductName.HeaderText = "المنتج";
            colProductName.MinimumWidth = 8;
            colProductName.Name = "colProductName";
            colProductName.ReadOnly = true;
            // 
            // colUnit
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colUnit.DefaultCellStyle = dataGridViewCellStyle5;
            colUnit.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            colUnit.FlatStyle = FlatStyle.Flat;
            colUnit.HeaderText = "الوحدة";
            colUnit.MinimumWidth = 8;
            colUnit.Name = "colUnit";
            colUnit.Resizable = DataGridViewTriState.True;
            colUnit.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // colQuantity
            // 
            colQuantity.DataPropertyName = "Quantity";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "N3";
            colQuantity.DefaultCellStyle = dataGridViewCellStyle6;
            colQuantity.HeaderText = "الكمية";
            colQuantity.MinimumWidth = 8;
            colQuantity.Name = "colQuantity";
            // 
            // colUnitPrice
            // 
            colUnitPrice.DataPropertyName = "UnitPrice";
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Format = "N2";
            colUnitPrice.DefaultCellStyle = dataGridViewCellStyle7;
            colUnitPrice.HeaderText = "السعر";
            colUnitPrice.MinimumWidth = 8;
            colUnitPrice.Name = "colUnitPrice";
            // 
            // colDiscount
            // 
            colDiscount.DataPropertyName = "DiscountAmount";
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.ForeColor = Color.FromArgb(220, 53, 69);
            dataGridViewCellStyle8.Format = "N2";
            colDiscount.DefaultCellStyle = dataGridViewCellStyle8;
            colDiscount.HeaderText = "الخصم";
            colDiscount.MinimumWidth = 8;
            colDiscount.Name = "colDiscount";
            // 
            // colLineTotal
            // 
            colLineTotal.DataPropertyName = "LineTotal";
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle9.Format = "N2";
            colLineTotal.DefaultCellStyle = dataGridViewCellStyle9;
            colLineTotal.HeaderText = "الإجمالي";
            colLineTotal.MinimumWidth = 8;
            colLineTotal.Name = "colLineTotal";
            colLineTotal.ReadOnly = true;
            // 
            // colExpiry
            // 
            colExpiry.DataPropertyName = "ExpiryDate";
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.ForeColor = Color.FromArgb(13, 110, 253);
            colExpiry.DefaultCellStyle = dataGridViewCellStyle10;
            colExpiry.HeaderText = "تاريخ الانتهاء";
            colExpiry.MinimumWidth = 8;
            colExpiry.Name = "colExpiry";
            // 
            // colRowNum
            // 
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colRowNum.DefaultCellStyle = dataGridViewCellStyle11;
            colRowNum.HeaderText = "#";
            colRowNum.MinimumWidth = 8;
            colRowNum.Name = "colRowNum";
            colRowNum.ReadOnly = true;
            // 
            // cmsGridContext
            // 
            cmsGridContext.ImageScalingSize = new Size(24, 24);
            cmsGridContext.Items.AddRange(new ToolStripItem[] { tsmiEditRow, tsmiCancelEdit, tsmiDeleteRow });
            cmsGridContext.Name = "cmsGridContext";
            cmsGridContext.Size = new Size(213, 100);
            // 
            // tsmiEditRow
            // 
            tsmiEditRow.Name = "tsmiEditRow";
            tsmiEditRow.Size = new Size(212, 32);
            tsmiEditRow.Text = "✏️ تعديل السطر";
            // 
            // tsmiCancelEdit
            // 
            tsmiCancelEdit.Enabled = false;
            tsmiCancelEdit.Name = "tsmiCancelEdit";
            tsmiCancelEdit.Size = new Size(212, 32);
            tsmiCancelEdit.Text = "↩️ إلغاء التعديل";
            // 
            // tsmiDeleteRow
            // 
            tsmiDeleteRow.Name = "tsmiDeleteRow";
            tsmiDeleteRow.Size = new Size(212, 32);
            tsmiDeleteRow.Text = "🗑️ حذف السطر";
            // 
            // gbItemActions
            // 
            gbItemActions.BackColor = Color.FromArgb(235, 240, 235);
            gbItemActions.Controls.Add(flpItemActions);
            gbItemActions.Dock = DockStyle.Top;
            gbItemActions.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbItemActions.Location = new Point(3, 28);
            gbItemActions.Name = "gbItemActions";
            gbItemActions.Size = new Size(1594, 70);
            gbItemActions.TabIndex = 0;
            gbItemActions.TabStop = false;
            gbItemActions.Text = "أوامر الأصناف";
            // 
            // flpItemActions
            // 
            flpItemActions.BackColor = Color.FromArgb(235, 240, 235);
            flpItemActions.Controls.Add(btnAddLine);
            flpItemActions.Controls.Add(btnEditRow);
            flpItemActions.Controls.Add(btnDeleteRow);
            flpItemActions.Controls.Add(btnRefreshCatalog);
            flpItemActions.Controls.Add(btnCancelLine);
            flpItemActions.Controls.Add(lblLockIndicator);
            flpItemActions.Dock = DockStyle.Fill;
            flpItemActions.Location = new Point(3, 28);
            flpItemActions.Name = "flpItemActions";
            flpItemActions.Padding = new Padding(6, 0, 6, 0);
            flpItemActions.Size = new Size(1588, 39);
            flpItemActions.TabIndex = 0;
            flpItemActions.WrapContents = false;
            // 
            // lblLockIndicator
            // 
            lblLockIndicator.BackColor = Color.Honeydew;
            lblLockIndicator.BorderStyle = BorderStyle.FixedSingle;
            lblLockIndicator.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLockIndicator.ForeColor = Color.DarkGreen;
            lblLockIndicator.Location = new Point(645, 6);
            lblLockIndicator.Margin = new Padding(3, 6, 3, 3);
            lblLockIndicator.Name = "lblLockIndicator";
            lblLockIndicator.Size = new Size(196, 28);
            lblLockIndicator.TabIndex = 8;
            lblLockIndicator.Text = "✏ قابلة للتعديل";
            lblLockIndicator.TextAlign = ContentAlignment.MiddleCenter;
            lblLockIndicator.Click += lblLockIndicator_Click;
            // 
            // gbTotals
            // 
            gbTotals.BackColor = Color.FromArgb(235, 245, 235);
            gbTotals.Controls.Add(label1);
            gbTotals.Controls.Add(numInvoiceDiscount);
            gbTotals.Controls.Add(cmbDiscountType);
            gbTotals.Controls.Add(lblExpensesLabel);
            gbTotals.Controls.Add(txtExpenses);
            gbTotals.Controls.Add(cmbAccount);
            gbTotals.Controls.Add(lblPaid);
            gbTotals.Controls.Add(txtPaid);
            gbTotals.Controls.Add(lblTafqitLabel);
            gbTotals.Controls.Add(lblTafqitValue);
            gbTotals.Dock = DockStyle.Bottom;
            gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbTotals.Location = new Point(0, 741);
            gbTotals.Name = "gbTotals";
            gbTotals.Size = new Size(1600, 107);
            gbTotals.TabIndex = 17;
            gbTotals.TabStop = false;
            gbTotals.Text = "إجماليات الفاتورة";
            gbTotals.Enter += gbTotals_Enter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1321, 30);
            label1.Name = "label1";
            label1.Size = new Size(80, 24);
            label1.TabIndex = 10;
            label1.Text = "الخصم:";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numInvoiceDiscount
            // 
            numInvoiceDiscount.DecimalPlaces = 2;
            numInvoiceDiscount.Location = new Point(926, 26);
            numInvoiceDiscount.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numInvoiceDiscount.Name = "numInvoiceDiscount";
            numInvoiceDiscount.Size = new Size(178, 32);
            numInvoiceDiscount.TabIndex = 5;
            numInvoiceDiscount.TextAlign = HorizontalAlignment.Center;
            numInvoiceDiscount.ThousandsSeparator = true;
            numInvoiceDiscount.ValueChanged += OnCalculateTotals;
            // 
            // lblPaid
            // 
            lblPaid.AutoSize = true;
            lblPaid.Location = new Point(813, 68);
            lblPaid.Name = "lblPaid";
            lblPaid.Size = new Size(91, 24);
            lblPaid.TabIndex = 8;
            lblPaid.Text = "المدفوع:";
            lblPaid.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPaid
            // 
            txtPaid.Location = new Point(624, 64);
            txtPaid.Name = "txtPaid";
            txtPaid.Size = new Size(178, 32);
            txtPaid.TabIndex = 9;
            txtPaid.Text = "0.00";
            txtPaid.TextAlign = HorizontalAlignment.Center;
            txtPaid.TextChanged += OnCalculateTotals;
            // 
            // lblQuickInfo
            // 
            lblQuickInfo.BackColor = Color.FromArgb(235, 245, 235);
            lblQuickInfo.BorderStyle = BorderStyle.FixedSingle;
            lblQuickInfo.Dock = DockStyle.Bottom;
            lblQuickInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblQuickInfo.ForeColor = Color.FromArgb(14, 82, 52);
            lblQuickInfo.Location = new Point(0, 848);
            lblQuickInfo.Name = "lblQuickInfo";
            lblQuickInfo.Padding = new Padding(5);
            lblQuickInfo.Size = new Size(1600, 41);
            lblQuickInfo.TabIndex = 12;
            lblQuickInfo.Text = "عدد الأصناف: 0 | إجمالي الكمية: 0 | الإجمالي: 0.00 | المدفوع: 0.00 | المتبقي: 0.00";
            lblQuickInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbToolbar
            // 
            gbToolbar.BackColor = Color.FromArgb(235, 240, 235);
            gbToolbar.Controls.Add(flpToolbar);
            gbToolbar.Dock = DockStyle.Top;
            gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbToolbar.Location = new Point(0, 214);
            gbToolbar.Name = "gbToolbar";
            gbToolbar.Size = new Size(1600, 83);
            gbToolbar.TabIndex = 1;
            gbToolbar.TabStop = false;
            gbToolbar.Text = "أوامر الفاتورة";
            // 
            // flpToolbar
            // 
            flpToolbar.BackColor = Color.FromArgb(235, 240, 235);
            flpToolbar.Controls.Add(btnNew);
            flpToolbar.Controls.Add(btnSaveDraft);
            flpToolbar.Controls.Add(btnPost);
            flpToolbar.Controls.Add(btnPrintA4);
            flpToolbar.Controls.Add(btnPrintReceipt);
            flpToolbar.Controls.Add(chkPrintSummary);
            flpToolbar.Controls.Add(btnSalesReturn);
            flpToolbar.Controls.Add(btnCancelInvoice);
            flpToolbar.Controls.Add(btnClose);
            flpToolbar.Dock = DockStyle.Fill;
            flpToolbar.Location = new Point(3, 28);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Padding = new Padding(6, 0, 6, 0);
            flpToolbar.Size = new Size(1594, 52);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(14, 165, 103);
            lblMode.Dock = DockStyle.Top;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1600, 39);
            lblMode.TabIndex = 99;
            lblMode.Text = "فاتورة بيع جديدة";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmAddUpdateSalesInvoice
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.FromArgb(170, 220, 170);
            ClientSize = new Size(1600, 889);
            Controls.Add(gbItems);
            Controls.Add(gbTotals);
            Controls.Add(lblQuickInfo);
            Controls.Add(gbEntry);
            Controls.Add(gbToolbar);
            Controls.Add(gbInvoiceInfo);
            Controls.Add(lblMode);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddUpdateSalesInvoice";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "فاتورة بيع";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            gbInvoiceInfo.ResumeLayout(false);
            gbInvoiceInfo.PerformLayout();
            gbEntry.ResumeLayout(false);
            pnlEntry.ResumeLayout(false);
            pnlEntry.PerformLayout();
            gbItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            cmsGridContext.ResumeLayout(false);
            gbItemActions.ResumeLayout(false);
            flpItemActions.ResumeLayout(false);
            gbTotals.ResumeLayout(false);
            gbTotals.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numInvoiceDiscount).EndInit();
            gbToolbar.ResumeLayout(false);
            flpToolbar.ResumeLayout(false);
            ResumeLayout(false);
        }

        // ========================================================================
        // Field declarations — required by code-behind
        // ========================================================================
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip liveTip;

        // Mode banner (top)
        private System.Windows.Forms.Label lblMode;

        // Header GroupBox (replaces pnlHeader)
        private System.Windows.Forms.GroupBox gbInvoiceInfo;
        private System.Windows.Forms.Label lblInvoiceNoLabel;
        private System.Windows.Forms.Label lblInvoiceNo;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Label lblWarehouse;
        private System.Windows.Forms.ComboBox cmbWarehouse;
        private System.Windows.Forms.Label lblCashBox;
        private System.Windows.Forms.ComboBox cmbCashBox;
        private System.Windows.Forms.Label lblPaymentType;

        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblNotes;

        // Entry GroupBox (wraps pnlEntry)
        private System.Windows.Forms.GroupBox gbEntry;
        private System.Windows.Forms.Panel pnlEntry;
        private System.Windows.Forms.Label lblBarcodeLabel;
        private System.Windows.Forms.Label lblItemCodeLabel;
        private System.Windows.Forms.Label lblProductLabel;
        private System.Windows.Forms.Label lblUnitLabel;
        private System.Windows.Forms.Label lblQtyLabel;
        private System.Windows.Forms.Label lblPriceLabel;
        private System.Windows.Forms.Label lblDiscountLabel;
        private System.Windows.Forms.Label lblLineExpiryLabel;
        private System.Windows.Forms.Label lblTotalHeaderLabel;
        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.TextBox txtItemCode;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.TextBox txtLineDiscount;
        private System.Windows.Forms.DateTimePicker dtpLineExpiry;
        private System.Windows.Forms.Label lblLineTotal;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.Button btnCancelLine;

        // Items GroupBox (wraps dgvItems + gbItemActions)
        private System.Windows.Forms.GroupBox gbItems;
        private System.Windows.Forms.GroupBox gbItemActions;
        private System.Windows.Forms.FlowLayoutPanel flpItemActions;
        private System.Windows.Forms.Button btnEditRow;
        private System.Windows.Forms.Button btnDeleteRow;
        private System.Windows.Forms.Button btnRefreshCatalog;
        private System.Windows.Forms.Label lblLockIndicator;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemCode;
        private System.Windows.Forms.DataGridViewComboBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpiry;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineTotal;
        private System.Windows.Forms.DataGridViewImageColumn colDelete;
        private System.Windows.Forms.ContextMenuStrip cmsGridContext;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditRow;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteRow;
        private System.Windows.Forms.ToolStripMenuItem tsmiCancelEdit;

        // Totals GroupBox (replaces pnlTotals)
        private System.Windows.Forms.GroupBox gbTotals;
        private System.Windows.Forms.NumericUpDown numInvoiceDiscount;
        private System.Windows.Forms.ComboBox cmbDiscountType;
        private System.Windows.Forms.Label lblTafqitLabel;
        private System.Windows.Forms.Label lblTafqitValue;
        private System.Windows.Forms.Label lblExpensesLabel;
        private System.Windows.Forms.TextBox txtExpenses;
        private System.Windows.Forms.ComboBox cmbAccount;
        private SalesSystem.Desktop.Controls.Common.MoneyTextBox txtPaid;

        // Quick Info (summary bar between gbTotals and the item-actions bar)
        private System.Windows.Forms.Label lblQuickInfo;

        // Invoice-level toolbar GroupBox (wraps flpToolbar)
        private System.Windows.Forms.GroupBox gbToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpToolbar;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSalesReturn;
        private System.Windows.Forms.Button btnSaveDraft;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnCancelInvoice;
        private System.Windows.Forms.Button btnPrintA4;
        private System.Windows.Forms.Button btnPrintReceipt;
        private System.Windows.Forms.CheckBox chkPrintSummary;
        private System.Windows.Forms.Button btnClose;
        private Label label1;
        private Label lblPaid;
        private RadioButton rbtnPaymentTypeCash;
        private RadioButton rbtnPaymentTypeCredit;
    }
}

