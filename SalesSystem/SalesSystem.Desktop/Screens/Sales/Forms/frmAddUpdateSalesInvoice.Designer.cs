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
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            errorProvider = new ErrorProvider(components);
            tooltip = new ToolTip(components);
            txtNotes = new TextBox();
            txtBarcode = new TextBox();
            lblProductName = new Label();
            cmbUnit = new ComboBox();
            txtQuantity = new TextBox();
            txtUnitPrice = new TextBox();
            lblLineTotal = new Label();
            btnAddLine = new Button();
            btnCancelLine = new Button();
            btnSaveDraft = new Button();
            btnPost = new Button();
            btnCancelInvoice = new Button();
            btnPrintA4 = new Button();
            btnPrintReceipt = new Button();
            btnClose = new Button();
            cmbTax = new ComboBox();
            chkApplyTax = new CheckBox();
            gbInvoiceInfo = new GroupBox();
            lblCustomer = new Label();
            cmbCustomer = new ComboBox();
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            lblPaymentType = new Label();
            cmbPaymentType = new ComboBox();
            lblInvoiceNoLabel = new Label();
            lblDate = new Label();
            lblInvoiceNo = new Label();
            dtpDate = new DateTimePicker();
            lblStatus = new Label();
            cmbDiscountType = new ComboBox();
            lblExpensesLabel = new Label();
            txtExpenses = new TextBox();
            cmbAccount = new ComboBox();
            gbEntry = new GroupBox();
            pnlEntry = new Panel();
            lblBarcodeLabel = new Label();
            lblProductLabel = new Label();
            lblUnitLabel = new Label();
            lblQtyLabel = new Label();
            lblPriceLabel = new Label();
            lblTotalHeaderLabel = new Label();
            gbItems = new GroupBox();
            dgvItems = new DataGridView();
            colRowNum = new DataGridViewTextBoxColumn();
            colBarcode = new DataGridViewTextBoxColumn();
            colProductName = new DataGridViewTextBoxColumn();
            colUnit = new DataGridViewComboBoxColumn();
            colQuantity = new DataGridViewTextBoxColumn();
            colUnitPrice = new DataGridViewTextBoxColumn();
            colLineTotal = new DataGridViewTextBoxColumn();
            colDelete = new DataGridViewImageColumn();
            cmsGridContext = new ContextMenuStrip(components);
            tsmiEditRow = new ToolStripMenuItem();
            tsmiCancelEdit = new ToolStripMenuItem();
            tsmiDeleteRow = new ToolStripMenuItem();
            gbTotals = new GroupBox();
            numInvoiceDiscount = new NumericUpDown();
            lblDiscountAmountVal = new Label();
            lblSubTotal = new Label();
            lblSubTotalVal = new Label();
            lblTotal = new Label();
            lblTotalVal = new Label();
            lblPaid = new Label();
            txtPaid = new SalesSystem.Desktop.Controls.Common.MoneyTextBox();
            lblBankAmount = new Label();
            txtBankAmount = new SalesSystem.Desktop.Controls.Common.MoneyTextBox();
            lblBank = new Label();
            cmbBank = new ComboBox();
            lblDue = new Label();
            lblDueVal = new Label();
            lblQuickInfo = new Label();
            gbActions = new GroupBox();
            pnlButtons = new Panel();
            lblMode = new Label();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            gbInvoiceInfo.SuspendLayout();
            gbEntry.SuspendLayout();
            pnlEntry.SuspendLayout();
            gbItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            cmsGridContext.SuspendLayout();
            gbTotals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numInvoiceDiscount).BeginInit();
            gbActions.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();
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
            txtNotes.Dock = DockStyle.Bottom;
            txtNotes.Location = new Point(3, 127);
            txtNotes.MaxLength = 500;
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.PlaceholderText = "ملاحظات...";
            txtNotes.Size = new Size(1594, 45);
            txtNotes.TabIndex = 14;
            tooltip.SetToolTip(txtNotes, "إدخال ملاحظات إضافية على الفاتورة");
            txtNotes.Validating += _ValidateEmptyTextBox;
            // 
            // txtBarcode
            // 
            txtBarcode.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtBarcode.Location = new Point(1313, 33);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(271, 34);
            txtBarcode.TabIndex = 7;
            txtBarcode.TextAlign = HorizontalAlignment.Center;
            tooltip.SetToolTip(txtBarcode, "مسح الباركود أو إدخال كود المنتج");
            // 
            // lblProductName
            // 
            lblProductName.BackColor = Color.White;
            lblProductName.BorderStyle = BorderStyle.FixedSingle;
            lblProductName.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProductName.Location = new Point(1037, 32);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(271, 34);
            lblProductName.TabIndex = 8;
            lblProductName.TextAlign = ContentAlignment.MiddleCenter;
            tooltip.SetToolTip(lblProductName, "اسم المنتج المحدد");
            // 
            // cmbUnit
            // 
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnit.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbUnit.Location = new Point(884, 32);
            cmbUnit.Name = "cmbUnit";
            cmbUnit.Size = new Size(142, 35);
            cmbUnit.TabIndex = 9;
            tooltip.SetToolTip(cmbUnit, "اختيار وحدة القياس");
            // 
            // txtQuantity
            // 
            txtQuantity.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtQuantity.Location = new Point(736, 31);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(142, 34);
            txtQuantity.TabIndex = 10;
            txtQuantity.Text = "1";
            txtQuantity.TextAlign = HorizontalAlignment.Center;
            tooltip.SetToolTip(txtQuantity, "الكمية المطلوبة");
            // 
            // txtUnitPrice
            // 
            txtUnitPrice.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUnitPrice.Location = new Point(588, 31);
            txtUnitPrice.Name = "txtUnitPrice";
            txtUnitPrice.Size = new Size(142, 34);
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
            lblLineTotal.Location = new Point(280, 31);
            lblLineTotal.Name = "lblLineTotal";
            lblLineTotal.Size = new Size(142, 34);
            lblLineTotal.TabIndex = 13;
            lblLineTotal.Text = "0.00";
            lblLineTotal.TextAlign = ContentAlignment.MiddleCenter;
            tooltip.SetToolTip(lblLineTotal, "إجمالي سعر هذا الصنف");
            // 
            // btnAddLine
            // 
            btnAddLine.BackColor = Color.FromArgb(25, 135, 84);
            btnAddLine.FlatAppearance.BorderSize = 0;
            btnAddLine.FlatStyle = FlatStyle.Flat;
            btnAddLine.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddLine.ForeColor = Color.White;
            btnAddLine.Location = new Point(136, 31);
            btnAddLine.Name = "btnAddLine";
            btnAddLine.Size = new Size(120, 35);
            btnAddLine.TabIndex = 14;
            btnAddLine.Text = "➕ إضافة";
            tooltip.SetToolTip(btnAddLine, "إضافة هذا الصنف إلى قائمة الفاتورة (Enter)");
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
            btnCancelLine.Location = new Point(19, 31);
            btnCancelLine.Name = "btnCancelLine";
            btnCancelLine.Size = new Size(111, 35);
            btnCancelLine.TabIndex = 14;
            btnCancelLine.Text = "🔄 إلغاء";
            tooltip.SetToolTip(btnCancelLine, "إلغاء إدخال هذا الصنف ومسح الحقول");
            btnCancelLine.UseVisualStyleBackColor = false;
            btnCancelLine.Click += btnCancelLine_Click;
            // 
            // btnSaveDraft
            // 
            btnSaveDraft.BackColor = Color.FromArgb(25, 135, 84);
            btnSaveDraft.Dock = DockStyle.Left;
            btnSaveDraft.FlatAppearance.BorderSize = 0;
            btnSaveDraft.FlatStyle = FlatStyle.Flat;
            btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSaveDraft.ForeColor = Color.White;
            btnSaveDraft.Location = new Point(196, 5);
            btnSaveDraft.Name = "btnSaveDraft";
            btnSaveDraft.Size = new Size(180, 39);
            btnSaveDraft.TabIndex = 0;
            btnSaveDraft.Text = "💾 حفظ مسودة";
            tooltip.SetToolTip(btnSaveDraft, "حفظ الفاتورة كمسودة دون ترحيل للمخزون");
            btnSaveDraft.UseVisualStyleBackColor = false;
            btnSaveDraft.Click += btnSaveDraft_Click;
            // 
            // btnPost
            // 
            btnPost.BackColor = Color.FromArgb(13, 110, 253);
            btnPost.Dock = DockStyle.Left;
            btnPost.FlatAppearance.BorderSize = 0;
            btnPost.FlatStyle = FlatStyle.Flat;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPost.ForeColor = Color.White;
            btnPost.Location = new Point(5, 5);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(191, 39);
            btnPost.TabIndex = 1;
            btnPost.Text = "🚀 ترحيل (F5)";
            tooltip.SetToolTip(btnPost, "ترحيل الفاتورة نهائياً — سيتم تحديث المخزون والرصيد");
            btnPost.UseVisualStyleBackColor = false;
            btnPost.Click += btnPost_Click;
            // 
            // btnCancelInvoice
            // 
            btnCancelInvoice.BackColor = Color.FromArgb(220, 53, 69);
            btnCancelInvoice.Dock = DockStyle.Right;
            btnCancelInvoice.FlatAppearance.BorderSize = 0;
            btnCancelInvoice.FlatStyle = FlatStyle.Flat;
            btnCancelInvoice.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelInvoice.ForeColor = Color.White;
            btnCancelInvoice.Location = new Point(1277, 5);
            btnCancelInvoice.Name = "btnCancelInvoice";
            btnCancelInvoice.Size = new Size(185, 39);
            btnCancelInvoice.TabIndex = 2;
            btnCancelInvoice.Text = "🗑️ إلغاء الفاتورة";
            tooltip.SetToolTip(btnCancelInvoice, "إلغاء الفاتورة — متاح فقط للفواتير المرحلة");
            btnCancelInvoice.UseVisualStyleBackColor = false;
            btnCancelInvoice.Click += btnCancelInvoice_Click;
            // 
            // btnPrintA4
            // 
            btnPrintA4.BackColor = Color.FromArgb(73, 80, 87);
            btnPrintA4.Dock = DockStyle.Right;
            btnPrintA4.FlatAppearance.BorderSize = 0;
            btnPrintA4.FlatStyle = FlatStyle.Flat;
            btnPrintA4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrintA4.ForeColor = Color.White;
            btnPrintA4.Location = new Point(1045, 5);
            btnPrintA4.Name = "btnPrintA4";
            btnPrintA4.Size = new Size(100, 39);
            btnPrintA4.TabIndex = 3;
            btnPrintA4.Text = "🖨️ A4";
            tooltip.SetToolTip(btnPrintA4, "طباعة فاتورة بحجم A4");
            btnPrintA4.UseVisualStyleBackColor = false;
            btnPrintA4.Click += btnPrintA4_Click;
            // 
            // btnPrintReceipt
            // 
            btnPrintReceipt.BackColor = Color.FromArgb(108, 117, 125);
            btnPrintReceipt.Dock = DockStyle.Right;
            btnPrintReceipt.FlatAppearance.BorderSize = 0;
            btnPrintReceipt.FlatStyle = FlatStyle.Flat;
            btnPrintReceipt.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrintReceipt.ForeColor = Color.White;
            btnPrintReceipt.Location = new Point(1145, 5);
            btnPrintReceipt.Name = "btnPrintReceipt";
            btnPrintReceipt.Size = new Size(132, 39);
            btnPrintReceipt.TabIndex = 4;
            btnPrintReceipt.Text = "\U0001f9fe إيصال";
            tooltip.SetToolTip(btnPrintReceipt, "طباعة إيصال صغير");
            btnPrintReceipt.UseVisualStyleBackColor = false;
            btnPrintReceipt.Click += btnPrintReceipt_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.Dock = DockStyle.Right;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1462, 5);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(127, 39);
            btnClose.TabIndex = 5;
            btnClose.Text = "❌ خروج";
            tooltip.SetToolTip(btnClose, "إغلاق نافذة الفاتورة");
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // cmbTax
            // 
            cmbTax.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTax.Enabled = false;
            cmbTax.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbTax.Location = new Point(1271, 39);
            cmbTax.Name = "cmbTax";
            cmbTax.Size = new Size(206, 32);
            cmbTax.TabIndex = 14;
            tooltip.SetToolTip(cmbTax, "اختيار ضريبة القيمة المضافة");
            cmbTax.SelectedIndexChanged += OnCalculateTotals;
            // 
            // chkApplyTax
            // 
            chkApplyTax.AutoSize = true;
            chkApplyTax.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkApplyTax.Location = new Point(1483, 41);
            chkApplyTax.Name = "chkApplyTax";
            chkApplyTax.Size = new Size(98, 28);
            chkApplyTax.TabIndex = 13;
            chkApplyTax.Text = "شاملة";
            chkApplyTax.UseVisualStyleBackColor = true;
            chkApplyTax.CheckedChanged += chkApplyTax_CheckedChanged;
            // 
            // gbInvoiceInfo
            // 
            gbInvoiceInfo.Controls.Add(lblCustomer);
            gbInvoiceInfo.Controls.Add(cmbCustomer);
            gbInvoiceInfo.Controls.Add(lblWarehouse);
            gbInvoiceInfo.Controls.Add(cmbWarehouse);
            gbInvoiceInfo.Controls.Add(lblPaymentType);
            gbInvoiceInfo.Controls.Add(cmbPaymentType);
            gbInvoiceInfo.Controls.Add(lblInvoiceNoLabel);
            gbInvoiceInfo.Controls.Add(lblDate);
            gbInvoiceInfo.Controls.Add(lblInvoiceNo);
            gbInvoiceInfo.Controls.Add(dtpDate);
            gbInvoiceInfo.Controls.Add(lblStatus);
            gbInvoiceInfo.Controls.Add(txtNotes);
            gbInvoiceInfo.Dock = DockStyle.Top;
            gbInvoiceInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbInvoiceInfo.Location = new Point(0, 39);
            gbInvoiceInfo.Name = "gbInvoiceInfo";
            gbInvoiceInfo.Size = new Size(1600, 175);
            gbInvoiceInfo.TabIndex = 0;
            gbInvoiceInfo.TabStop = false;
            gbInvoiceInfo.Text = "معلومات الفاتورة";
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
            // cmbPaymentType
            // 
            cmbPaymentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentType.Items.AddRange(new object[] { "نقدا", "آجل", "مختلط" });
            cmbPaymentType.Location = new Point(521, 28);
            cmbPaymentType.Name = "cmbPaymentType";
            cmbPaymentType.Size = new Size(195, 32);
            cmbPaymentType.TabIndex = 10;
            cmbPaymentType.SelectedIndexChanged += cmbPaymentType_SelectedIndexChanged;
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
            // cmbDiscountType — HIDDEN: V1 uses amount-only discount
            // 
            cmbDiscountType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDiscountType.Items.AddRange(new object[] { "مبلغ ثابت" });
            cmbDiscountType.Location = new Point(1127, 37);
            cmbDiscountType.Name = "cmbDiscountType";
            cmbDiscountType.Size = new Size(138, 32);
            cmbDiscountType.TabIndex = 14;
            cmbDiscountType.Visible = false; // Amount-only — no percentage in V1
            // 
            // lblExpensesLabel
            // 
            lblExpensesLabel.AutoSize = true;
            lblExpensesLabel.Location = new Point(763, 39);
            lblExpensesLabel.Name = "lblExpensesLabel";
            lblExpensesLabel.Size = new Size(107, 24);
            lblExpensesLabel.TabIndex = 5;
            lblExpensesLabel.Text = "المصاريف:";
            lblExpensesLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtExpenses
            // 
            txtExpenses.Location = new Point(368, 34);
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
            cmbAccount.Location = new Point(552, 35);
            cmbAccount.Name = "cmbAccount";
            cmbAccount.Size = new Size(205, 32);
            cmbAccount.TabIndex = 8;
            // 
            // gbEntry
            // 
            gbEntry.Controls.Add(pnlEntry);
            gbEntry.Dock = DockStyle.Top;
            gbEntry.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbEntry.Location = new Point(0, 214);
            gbEntry.Name = "gbEntry";
            gbEntry.Size = new Size(1600, 125);
            gbEntry.TabIndex = 15;
            gbEntry.TabStop = false;
            gbEntry.Text = "إدخال الأصناف";
            // 
            // pnlEntry
            // 
            pnlEntry.BackColor = Color.FromArgb(248, 249, 250);
            pnlEntry.BorderStyle = BorderStyle.FixedSingle;
            pnlEntry.Controls.Add(lblBarcodeLabel);
            pnlEntry.Controls.Add(lblProductLabel);
            pnlEntry.Controls.Add(lblUnitLabel);
            pnlEntry.Controls.Add(lblQtyLabel);
            pnlEntry.Controls.Add(lblPriceLabel);
            pnlEntry.Controls.Add(lblDiscountLabel);
            pnlEntry.Controls.Add(lblTotalHeaderLabel);
            pnlEntry.Controls.Add(txtBarcode);
            pnlEntry.Controls.Add(lblProductName);
            pnlEntry.Controls.Add(cmbUnit);
            pnlEntry.Controls.Add(txtQuantity);
            pnlEntry.Controls.Add(txtUnitPrice);
            pnlEntry.Controls.Add(txtLineDiscount);
            pnlEntry.Controls.Add(lblLineTotal);
            pnlEntry.Controls.Add(btnAddLine);
            pnlEntry.Controls.Add(btnCancelLine);
            pnlEntry.Dock = DockStyle.Fill;
            pnlEntry.Location = new Point(3, 28);
            pnlEntry.Name = "pnlEntry";
            pnlEntry.Size = new Size(1594, 94);
            pnlEntry.TabIndex = 15;
            // 
            // lblBarcodeLabel
            // 
            lblBarcodeLabel.AutoSize = true;
            lblBarcodeLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBarcodeLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblBarcodeLabel.Location = new Point(1424, 7);
            lblBarcodeLabel.Name = "lblBarcodeLabel";
            lblBarcodeLabel.Size = new Size(73, 22);
            lblBarcodeLabel.TabIndex = 0;
            lblBarcodeLabel.Text = "الباركود";
            lblBarcodeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblProductLabel
            // 
            lblProductLabel.AutoSize = true;
            lblProductLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProductLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblProductLabel.Location = new Point(1144, 7);
            lblProductLabel.Name = "lblProductLabel";
            lblProductLabel.Size = new Size(62, 22);
            lblProductLabel.TabIndex = 1;
            lblProductLabel.Text = "المنتج";
            lblProductLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUnitLabel
            // 
            lblUnitLabel.AutoSize = true;
            lblUnitLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUnitLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblUnitLabel.Location = new Point(922, 7);
            lblUnitLabel.Name = "lblUnitLabel";
            lblUnitLabel.Size = new Size(66, 22);
            lblUnitLabel.TabIndex = 2;
            lblUnitLabel.Text = "الوحدة";
            lblUnitLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblQtyLabel
            // 
            lblQtyLabel.AutoSize = true;
            lblQtyLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblQtyLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblQtyLabel.Location = new Point(771, 8);
            lblQtyLabel.Name = "lblQtyLabel";
            lblQtyLabel.Size = new Size(64, 22);
            lblQtyLabel.TabIndex = 3;
            lblQtyLabel.Text = "الكمية";
            lblQtyLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPriceLabel
            // 
            lblPriceLabel.AutoSize = true;
            lblPriceLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPriceLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblPriceLabel.Location = new Point(627, 6);
            lblPriceLabel.Name = "lblPriceLabel";
            lblPriceLabel.Size = new Size(63, 22);
            lblPriceLabel.TabIndex = 4;
            lblPriceLabel.Text = "السعر";
            lblPriceLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDiscountLabel
            // 
            lblDiscountLabel = new Label();
            lblDiscountLabel.AutoSize = true;
            lblDiscountLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDiscountLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblDiscountLabel.Location = new Point(478, 6);
            lblDiscountLabel.Name = "lblDiscountLabel";
            lblDiscountLabel.Size = new Size(75, 22);
            lblDiscountLabel.TabIndex = 5;
            lblDiscountLabel.Text = "الخصم";
            lblDiscountLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtLineDiscount
            // 
            txtLineDiscount = new TextBox();
            txtLineDiscount.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtLineDiscount.Location = new Point(440, 31);
            txtLineDiscount.Name = "txtLineDiscount";
            txtLineDiscount.Size = new Size(112, 34);
            txtLineDiscount.TabIndex = 12;
            txtLineDiscount.Text = "0";
            txtLineDiscount.TextAlign = HorizontalAlignment.Center;
            tooltip.SetToolTip(txtLineDiscount, "الخصم المسموح به للصنف (مبلغ ثابت)");
            // 
            // lblTotalHeaderLabel
            // 
            lblTotalHeaderLabel.AutoSize = true;
            lblTotalHeaderLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalHeaderLabel.ForeColor = Color.FromArgb(108, 117, 125);
            lblTotalHeaderLabel.Location = new Point(310, 6);
            lblTotalHeaderLabel.Name = "lblTotalHeaderLabel";
            lblTotalHeaderLabel.Size = new Size(87, 22);
            lblTotalHeaderLabel.TabIndex = 6;
            lblTotalHeaderLabel.Text = "الإجمالي";
            lblTotalHeaderLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbItems
            // 
            gbItems.Controls.Add(dgvItems);
            gbItems.Dock = DockStyle.Fill;
            gbItems.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbItems.Location = new Point(0, 339);
            gbItems.Name = "gbItems";
            gbItems.Size = new Size(1600, 284);
            gbItems.TabIndex = 16;
            gbItems.TabStop = false;
            gbItems.Text = "الأصناف";
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.AllowUserToResizeRows = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.BackgroundColor = Color.White;
            dgvItems.BorderStyle = BorderStyle.None;
            dgvItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = Color.FromArgb(52, 73, 94);
            dataGridViewCellStyle10.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle10.ForeColor = Color.White;
            dataGridViewCellStyle10.SelectionBackColor = Color.FromArgb(52, 73, 94);
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            dgvItems.ColumnHeadersHeight = 45;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvItems.Columns.AddRange(new DataGridViewColumn[] { colRowNum, colBarcode, colProductName, colUnit, colQuantity, colUnitPrice, colLineTotal, colDelete });
            dgvItems.ContextMenuStrip = cmsGridContext;
            dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = Color.White;
            dataGridViewCellStyle18.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle18.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle18.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dataGridViewCellStyle18.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle18.WrapMode = DataGridViewTriState.False;
            dgvItems.DefaultCellStyle = dataGridViewCellStyle18;
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.GridColor = Color.FromArgb(222, 226, 230);
            dgvItems.Location = new Point(3, 28);
            dgvItems.MultiSelect = false;
            dgvItems.Name = "dgvItems";
            dgvItems.RowHeadersVisible = false;
            dgvItems.RowHeadersWidth = 62;
            dgvItems.RowTemplate.Height = 32;
            dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvItems.Size = new Size(1594, 253);
            dgvItems.TabIndex = 16;
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
            // colBarcode
            // 
            colBarcode.DataPropertyName = "ProductCode";
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colBarcode.DefaultCellStyle = dataGridViewCellStyle12;
            colBarcode.HeaderText = "الباركود";
            colBarcode.MinimumWidth = 8;
            colBarcode.Name = "colBarcode";
            // 
            // colProductName
            // 
            colProductName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colProductName.DataPropertyName = "ProductName";
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colProductName.DefaultCellStyle = dataGridViewCellStyle13;
            colProductName.HeaderText = "المنتج";
            colProductName.MinimumWidth = 8;
            colProductName.Name = "colProductName";
            colProductName.ReadOnly = true;
            // 
            // colUnit
            // 
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colUnit.DefaultCellStyle = dataGridViewCellStyle14;
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
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.Format = "N3";
            colQuantity.DefaultCellStyle = dataGridViewCellStyle15;
            colQuantity.HeaderText = "الكمية";
            colQuantity.MinimumWidth = 8;
            colQuantity.Name = "colQuantity";
            // 
            // colUnitPrice
            // 
            colUnitPrice.DataPropertyName = "UnitPrice";
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.Format = "N2";
            colUnitPrice.DefaultCellStyle = dataGridViewCellStyle16;
            colUnitPrice.HeaderText = "السعر";
            colUnitPrice.MinimumWidth = 8;
            colUnitPrice.Name = "colUnitPrice";
            // 
            // colLineTotal
            // 
            colLineTotal.DataPropertyName = "LineTotal";
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle17.Format = "N2";
            colLineTotal.DefaultCellStyle = dataGridViewCellStyle17;
            colLineTotal.HeaderText = "الإجمالي";
            colLineTotal.MinimumWidth = 8;
            colLineTotal.Name = "colLineTotal";
            colLineTotal.ReadOnly = true;
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
            // gbTotals
            // 
            gbTotals.Controls.Add(chkApplyTax);
            gbTotals.Controls.Add(cmbTax);
            gbTotals.Controls.Add(cmbDiscountType);
            gbTotals.Controls.Add(numInvoiceDiscount);
            gbTotals.Controls.Add(lblDiscountAmountVal);
            gbTotals.Controls.Add(lblExpensesLabel);
            gbTotals.Controls.Add(txtExpenses);
            gbTotals.Controls.Add(cmbAccount);
            gbTotals.Controls.Add(lblSubTotal);
            gbTotals.Controls.Add(lblSubTotalVal);
            gbTotals.Controls.Add(lblTotal);
            gbTotals.Controls.Add(lblTotalVal);
            gbTotals.Controls.Add(lblPaid);
            gbTotals.Controls.Add(txtPaid);
            gbTotals.Controls.Add(lblBankAmount);
            gbTotals.Controls.Add(txtBankAmount);
            gbTotals.Controls.Add(lblBank);
            gbTotals.Controls.Add(cmbBank);
            gbTotals.Controls.Add(lblDue);
            gbTotals.Controls.Add(lblDueVal);
            gbTotals.Dock = DockStyle.Bottom;
            gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbTotals.Location = new Point(0, 623);
            gbTotals.Name = "gbTotals";
            gbTotals.Size = new Size(1600, 145);
            gbTotals.TabIndex = 17;
            gbTotals.TabStop = false;
            gbTotals.Text = "إجماليات الفاتورة";
            // 
            // numInvoiceDiscount
            // 
            numInvoiceDiscount.DecimalPlaces = 2;
            numInvoiceDiscount.Location = new Point(943, 37);
            numInvoiceDiscount.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numInvoiceDiscount.Name = "numInvoiceDiscount";
            numInvoiceDiscount.Size = new Size(178, 32);
            numInvoiceDiscount.TabIndex = 5;
            numInvoiceDiscount.TextAlign = HorizontalAlignment.Center;
            numInvoiceDiscount.ThousandsSeparator = true;
            numInvoiceDiscount.ValueChanged += OnCalculateTotals;
            // 
            // lblDiscountAmountVal
            // 
            lblDiscountAmountVal.ForeColor = Color.DarkOrange;
            lblDiscountAmountVal.Location = new Point(1086, 74);
            lblDiscountAmountVal.Name = "lblDiscountAmountVal";
            lblDiscountAmountVal.Size = new Size(178, 27);
            lblDiscountAmountVal.TabIndex = 7;
            lblDiscountAmountVal.Text = "0.00";
            lblDiscountAmountVal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubTotal
            // 
            lblSubTotal.AutoSize = true;
            lblSubTotal.Location = new Point(1402, 108);
            lblSubTotal.Name = "lblSubTotal";
            lblSubTotal.Size = new Size(179, 24);
            lblSubTotal.TabIndex = 6;
            lblSubTotal.Text = "الإجمالي الفرعي:";
            lblSubTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSubTotalVal
            // 
            lblSubTotalVal.ForeColor = Color.FromArgb(33, 37, 41);
            lblSubTotalVal.Location = new Point(1260, 107);
            lblSubTotalVal.Name = "lblSubTotalVal";
            lblSubTotalVal.Size = new Size(140, 27);
            lblSubTotalVal.TabIndex = 7;
            lblSubTotalVal.Text = "0.00";
            lblSubTotalVal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotal.Location = new Point(1064, 106);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(199, 27);
            lblTotal.TabIndex = 6;
            lblTotal.Text = "الإجمالي النهائي:";
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalVal
            // 
            lblTotalVal.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalVal.ForeColor = Color.FromArgb(13, 110, 253);
            lblTotalVal.Location = new Point(929, 109);
            lblTotalVal.Name = "lblTotalVal";
            lblTotalVal.Size = new Size(140, 27);
            lblTotalVal.TabIndex = 7;
            lblTotalVal.Text = "0.00";
            lblTotalVal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPaid
            // 
            lblPaid.AutoSize = true;
            lblPaid.Location = new Point(830, 109);
            lblPaid.Name = "lblPaid";
            lblPaid.Size = new Size(91, 24);
            lblPaid.TabIndex = 8;
            lblPaid.Text = "المدفوع:";
            lblPaid.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPaid
            // 
            txtPaid.Location = new Point(641, 107);
            txtPaid.Name = "txtPaid";
            txtPaid.Size = new Size(178, 32);
            txtPaid.TabIndex = 9;
            txtPaid.Text = "0.00";
            txtPaid.TextAlign = HorizontalAlignment.Center;
            txtPaid.TextChanged += OnCalculateTotals;
            // 
            // lblBankAmount
            // 
            lblBankAmount.AutoSize = true;
            lblBankAmount.Location = new Point(855, 70);
            lblBankAmount.Name = "lblBankAmount";
            lblBankAmount.Size = new Size(134, 24);
            lblBankAmount.TabIndex = 8;
            lblBankAmount.Text = "مدفوع (بنك):";
            lblBankAmount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtBankAmount
            // 
            txtBankAmount.Location = new Point(671, 68);
            txtBankAmount.Name = "txtBankAmount";
            txtBankAmount.Size = new Size(178, 32);
            txtBankAmount.TabIndex = 9;
            txtBankAmount.Text = "0.00";
            txtBankAmount.TextAlign = HorizontalAlignment.Center;
            txtBankAmount.TextChanged += OnCalculateTotals;
            // 
            // lblBank
            // 
            lblBank.AutoSize = true;
            lblBank.Location = new Point(590, 70);
            lblBank.Name = "lblBank";
            lblBank.Size = new Size(64, 24);
            lblBank.TabIndex = 8;
            lblBank.Text = "البنك:";
            lblBank.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbBank
            // 
            cmbBank.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBank.Location = new Point(261, 72);
            cmbBank.Name = "cmbBank";
            cmbBank.Size = new Size(323, 32);
            cmbBank.TabIndex = 8;
            // 
            // lblDue
            // 
            lblDue.AutoSize = true;
            lblDue.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDue.Location = new Point(526, 109);
            lblDue.Name = "lblDue";
            lblDue.Size = new Size(106, 27);
            lblDue.TabIndex = 10;
            lblDue.Text = "المتبقي:";
            lblDue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDueVal
            // 
            lblDueVal.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDueVal.ForeColor = Color.Red;
            lblDueVal.Location = new Point(363, 112);
            lblDueVal.Name = "lblDueVal";
            lblDueVal.Size = new Size(150, 27);
            lblDueVal.TabIndex = 11;
            lblDueVal.Text = "0.00";
            lblDueVal.TextAlign = ContentAlignment.MiddleCenter;
            lblDueVal.Click += lblDueVal_Click;
            // 
            // lblQuickInfo
            // 
            lblQuickInfo.BackColor = Color.FromArgb(248, 249, 250);
            lblQuickInfo.BorderStyle = BorderStyle.FixedSingle;
            lblQuickInfo.Dock = DockStyle.Bottom;
            lblQuickInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblQuickInfo.ForeColor = Color.FromArgb(73, 80, 87);
            lblQuickInfo.Location = new Point(0, 768);
            lblQuickInfo.Name = "lblQuickInfo";
            lblQuickInfo.Padding = new Padding(5);
            lblQuickInfo.Size = new Size(1600, 41);
            lblQuickInfo.TabIndex = 12;
            lblQuickInfo.Text = "عدد الأصناف: 0 | الإجمالي: 0.00 | المدفوع: 0.00 | المتبقي: 0.00";
            lblQuickInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // gbActions
            // 
            gbActions.Controls.Add(pnlButtons);
            gbActions.Dock = DockStyle.Bottom;
            gbActions.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbActions.Location = new Point(0, 809);
            gbActions.Name = "gbActions";
            gbActions.Size = new Size(1600, 80);
            gbActions.TabIndex = 18;
            gbActions.TabStop = false;
            gbActions.Text = "الإجراءات";
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnSaveDraft);
            pnlButtons.Controls.Add(btnPost);
            pnlButtons.Controls.Add(btnPrintA4);
            pnlButtons.Controls.Add(btnPrintReceipt);
            pnlButtons.Controls.Add(btnCancelInvoice);
            pnlButtons.Controls.Add(btnClose);
            pnlButtons.Dock = DockStyle.Fill;
            pnlButtons.Location = new Point(3, 28);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(5);
            pnlButtons.Size = new Size(1594, 49);
            pnlButtons.TabIndex = 18;
            pnlButtons.Paint += pnlButtons_Paint;
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
            lblMode.Size = new Size(1600, 39);
            lblMode.TabIndex = 99;
            lblMode.Text = "فاتورة بيع جديدة";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmAddUpdateSalesInvoice
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1600, 889);
            Controls.Add(gbItems);
            Controls.Add(gbTotals);
            Controls.Add(lblQuickInfo);
            Controls.Add(gbActions);
            Controls.Add(gbEntry);
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
            gbTotals.ResumeLayout(false);
            gbTotals.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numInvoiceDiscount).EndInit();
            gbActions.ResumeLayout(false);
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        // ========================================================================
        // Field declarations — required by code-behind
        // ========================================================================
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ToolTip tooltip;

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
        private System.Windows.Forms.Label lblPaymentType;
        private System.Windows.Forms.ComboBox cmbPaymentType;

        private System.Windows.Forms.TextBox txtNotes;

        // Entry GroupBox (wraps pnlEntry)
        private System.Windows.Forms.GroupBox gbEntry;
        private System.Windows.Forms.Panel pnlEntry;
        private System.Windows.Forms.Label lblBarcodeLabel;
        private System.Windows.Forms.Label lblProductLabel;
        private System.Windows.Forms.Label lblUnitLabel;
        private System.Windows.Forms.Label lblQtyLabel;
        private System.Windows.Forms.Label lblPriceLabel;
        private System.Windows.Forms.Label lblDiscountLabel;
        private System.Windows.Forms.Label lblTotalHeaderLabel;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.TextBox txtLineDiscount;
        private System.Windows.Forms.Label lblLineTotal;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.Button btnCancelLine;

        // Items GroupBox (wraps dgvItems)
        private System.Windows.Forms.GroupBox gbItems;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineTotal;
        private System.Windows.Forms.DataGridViewImageColumn colDelete;
        private System.Windows.Forms.ContextMenuStrip cmsGridContext;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditRow;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteRow;
        private System.Windows.Forms.ToolStripMenuItem tsmiCancelEdit;

        // Totals GroupBox (replaces pnlTotals)
        private System.Windows.Forms.GroupBox gbTotals;
        private System.Windows.Forms.CheckBox chkApplyTax;
        private System.Windows.Forms.ComboBox cmbTax;
        private System.Windows.Forms.ComboBox cmbDiscountType;
        private System.Windows.Forms.NumericUpDown numInvoiceDiscount;
        private System.Windows.Forms.Label lblExpensesLabel;
        private System.Windows.Forms.TextBox txtExpenses;
        private System.Windows.Forms.ComboBox cmbAccount;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalVal;
        private System.Windows.Forms.Label lblPaid;
        private SalesSystem.Desktop.Controls.Common.MoneyTextBox txtPaid;
        private System.Windows.Forms.Label lblDue;
        private System.Windows.Forms.Label lblDueVal;
        private System.Windows.Forms.Label lblBankAmount;
        private SalesSystem.Desktop.Controls.Common.MoneyTextBox txtBankAmount;
        private System.Windows.Forms.Label lblBank;
        private System.Windows.Forms.ComboBox cmbBank;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblSubTotalVal;
        private System.Windows.Forms.Label lblDiscountAmountVal;

        // Quick Info (summary bar between gbTotals and gbActions)
        private System.Windows.Forms.Label lblQuickInfo;

        // Actions GroupBox (wraps pnlButtons)
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnSaveDraft;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnCancelInvoice;
        private System.Windows.Forms.Button btnPrintA4;
        private System.Windows.Forms.Button btnPrintReceipt;
        private System.Windows.Forms.Button btnClose;
    }
}

