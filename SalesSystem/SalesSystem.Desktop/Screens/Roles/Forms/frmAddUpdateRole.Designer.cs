namespace SalesSystem.Desktop.Screens.Roles.Forms{
    using SalesSystem.Common.Enums;
    using SalesSystem.Common.Enums.Permissions;

    partial class frmAddUpdateRole
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblMode = new Label();
            lblName = new Label();
            txtName = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            grpSales = new GroupBox();
            flpSales = new FlowLayoutPanel();
            chkSalesSelectAll = new CheckBox();
            chkSalesViewInvoices = new CheckBox();
            chkSalesCreateInvoices = new CheckBox();
            chkSalesEditInvoices = new CheckBox();
            chkSalesDeleteInvoices = new CheckBox();
            chkSalesCancelInvoices = new CheckBox();
            chkSalesPrintInvoices = new CheckBox();
            chkSalesViewReturns = new CheckBox();
            chkSalesCreateReturns = new CheckBox();
            chkSalesViewReceipts = new CheckBox();
            chkSalesCreateReceipts = new CheckBox();
            chkSalesManagePrices = new CheckBox();
            chkSalesManageDiscounts = new CheckBox();
            grpInventory = new GroupBox();
            flpInventory = new FlowLayoutPanel();
            chkInventorySelectAll = new CheckBox();
            chkInventoryViewProducts = new CheckBox();
            chkInventoryCreateProducts = new CheckBox();
            chkInventoryEditProducts = new CheckBox();
            chkInventoryViewStock = new CheckBox();
            chkInventoryAdjustStock = new CheckBox();
            chkInventoryTransferStock = new CheckBox();
            chkInventoryCountStock = new CheckBox();
            chkInventoryViewBatches = new CheckBox();
            chkInventoryManageCategories = new CheckBox();
            chkInventoryManageUnits = new CheckBox();
            grpAccounting = new GroupBox();
            flpAccounting = new FlowLayoutPanel();
            chkAccountingSelectAll = new CheckBox();
            chkAccountingViewJournalEntries = new CheckBox();
            chkAccountingCreateJournalEntries = new CheckBox();
            chkAccountingViewAccounts = new CheckBox();
            chkAccountingManageAccounts = new CheckBox();
            chkAccountingViewCashBoxes = new CheckBox();
            chkAccountingManageCashBoxes = new CheckBox();
            chkAccountingViewExpenses = new CheckBox();
            chkAccountingCreateExpenses = new CheckBox();
            chkAccountingViewReceiptVouchers = new CheckBox();
            chkAccountingCreateReceiptVouchers = new CheckBox();
            chkAccountingViewPaymentVouchers = new CheckBox();
            chkAccountingCreatePaymentVouchers = new CheckBox();
            chkAccountingViewReports = new CheckBox();
            grpSystem = new GroupBox();
            flpSystem = new FlowLayoutPanel();
            chkSystemSelectAll = new CheckBox();
            chkSystemManageUsers = new CheckBox();
            chkSystemManageRoles = new CheckBox();
            chkSystemManageSettings = new CheckBox();
            chkSystemViewAuditLogs = new CheckBox();
            chkSystemManageBranches = new CheckBox();
            chkSystemManageBackups = new CheckBox();
            chkSystemManageFiscalYears = new CheckBox();
            chkSystemManageTaxes = new CheckBox();
            grpSales.SuspendLayout();
            flpSales.SuspendLayout();
            grpInventory.SuspendLayout();
            flpInventory.SuspendLayout();
            grpAccounting.SuspendLayout();
            flpAccounting.SuspendLayout();
            grpSystem.SuspendLayout();
            flpSystem.SuspendLayout();
            errorProvider = new ErrorProvider(components);
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.DarkSlateGray;
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 10.8F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1526, 42);
            lblMode.TabIndex = 0;
            lblMode.Text = "إضافة دور جديد";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Tahoma", 10F);
            lblName.Location = new Point(330, 60);
            lblName.Name = "lblName";
            lblName.Size = new Size(99, 24);
            lblName.TabIndex = 1;
            lblName.Text = "اسم الدور:";
            // 
            // txtName
            // 
            txtName.Font = new Font("Tahoma", 10F);
            txtName.Location = new Point(435, 57);
            txtName.Name = "txtName";
            txtName.Size = new Size(656, 32);
            txtName.TabIndex = 2;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Font = new Font("Tahoma", 10F);
            lblDescription.Location = new Point(359, 93);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(70, 24);
            lblDescription.TabIndex = 3;
            lblDescription.Text = "الوصف:";
            // 
            // txtDescription
            // 
            txtDescription.Font = new Font("Tahoma", 10F);
            txtDescription.Location = new Point(435, 93);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(656, 50);
            txtDescription.TabIndex = 4;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(554, 628);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(205, 40);
            btnSave.TabIndex = 5;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(765, 628);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 40);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // grpSales
            // 
            grpSales.Controls.Add(flpSales);
            grpSales.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpSales.Location = new Point(12, 162);
            grpSales.Name = "grpSales";
            grpSales.Size = new Size(750, 210);
            grpSales.TabIndex = 0;
            grpSales.TabStop = false;
            grpSales.Text = "صلاحيات المبيعات";
            // 
            // flpSales
            // 
            flpSales.AutoScroll = true;
            flpSales.Controls.Add(chkSalesSelectAll);
            flpSales.Controls.Add(chkSalesViewInvoices);
            flpSales.Controls.Add(chkSalesCreateInvoices);
            flpSales.Controls.Add(chkSalesEditInvoices);
            flpSales.Controls.Add(chkSalesDeleteInvoices);
            flpSales.Controls.Add(chkSalesCancelInvoices);
            flpSales.Controls.Add(chkSalesPrintInvoices);
            flpSales.Controls.Add(chkSalesViewReturns);
            flpSales.Controls.Add(chkSalesCreateReturns);
            flpSales.Controls.Add(chkSalesViewReceipts);
            flpSales.Controls.Add(chkSalesCreateReceipts);
            flpSales.Controls.Add(chkSalesManagePrices);
            flpSales.Controls.Add(chkSalesManageDiscounts);
            flpSales.Dock = DockStyle.Fill;
            flpSales.FlowDirection = FlowDirection.TopDown;
            flpSales.Font = new Font("Tahoma", 8F);
            flpSales.Location = new Point(3, 28);
            flpSales.Name = "flpSales";
            flpSales.Size = new Size(744, 179);
            flpSales.TabIndex = 0;
            // 
            // chkSalesSelectAll
            // 
            chkSalesSelectAll.Location = new Point(598, 3);
            chkSalesSelectAll.Name = "chkSalesSelectAll";
            chkSalesSelectAll.Size = new Size(143, 24);
            chkSalesSelectAll.TabIndex = 0;
            chkSalesSelectAll.Tag = 0L;
            chkSalesSelectAll.Text = "☑ تحديد الكل";
            chkSalesSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
            // 
            // chkSalesViewInvoices
            // 
            chkSalesViewInvoices.Location = new Point(605, 33);
            chkSalesViewInvoices.Name = "chkSalesViewInvoices";
            chkSalesViewInvoices.Size = new Size(136, 24);
            chkSalesViewInvoices.TabIndex = 1;
            chkSalesViewInvoices.Tag = 1L;
            chkSalesViewInvoices.Text = "عرض الفواتير";
            // 
            // chkSalesCreateInvoices
            // 
            chkSalesCreateInvoices.Location = new Point(605, 63);
            chkSalesCreateInvoices.Name = "chkSalesCreateInvoices";
            chkSalesCreateInvoices.Size = new Size(136, 24);
            chkSalesCreateInvoices.TabIndex = 2;
            chkSalesCreateInvoices.Tag = 2L;
            chkSalesCreateInvoices.Text = "إنشاء الفواتير";
            // 
            // chkSalesEditInvoices
            // 
            chkSalesEditInvoices.Location = new Point(598, 93);
            chkSalesEditInvoices.Name = "chkSalesEditInvoices";
            chkSalesEditInvoices.Size = new Size(143, 24);
            chkSalesEditInvoices.TabIndex = 3;
            chkSalesEditInvoices.Tag = 4L;
            chkSalesEditInvoices.Text = "تعديل الفواتير";
            // 
            // chkSalesDeleteInvoices
            // 
            chkSalesDeleteInvoices.Location = new Point(605, 123);
            chkSalesDeleteInvoices.Name = "chkSalesDeleteInvoices";
            chkSalesDeleteInvoices.Size = new Size(136, 24);
            chkSalesDeleteInvoices.TabIndex = 4;
            chkSalesDeleteInvoices.Tag = 8L;
            chkSalesDeleteInvoices.Text = "حذف الفواتير";
            // 
            // chkSalesCancelInvoices
            // 
            chkSalesCancelInvoices.Location = new Point(449, 3);
            chkSalesCancelInvoices.Name = "chkSalesCancelInvoices";
            chkSalesCancelInvoices.Size = new Size(143, 24);
            chkSalesCancelInvoices.TabIndex = 5;
            chkSalesCancelInvoices.Tag = 16L;
            chkSalesCancelInvoices.Text = "إلغاء الفواتير";
            // 
            // chkSalesPrintInvoices
            // 
            chkSalesPrintInvoices.Location = new Point(454, 33);
            chkSalesPrintInvoices.Name = "chkSalesPrintInvoices";
            chkSalesPrintInvoices.Size = new Size(138, 24);
            chkSalesPrintInvoices.TabIndex = 6;
            chkSalesPrintInvoices.Tag = 32L;
            chkSalesPrintInvoices.Text = "طباعة الفواتير";
            // 
            // chkSalesViewReturns
            // 
            chkSalesViewReturns.Location = new Point(445, 63);
            chkSalesViewReturns.Name = "chkSalesViewReturns";
            chkSalesViewReturns.Size = new Size(147, 24);
            chkSalesViewReturns.TabIndex = 7;
            chkSalesViewReturns.Tag = 64L;
            chkSalesViewReturns.Text = "عرض المرتجعات";
            // 
            // chkSalesCreateReturns
            // 
            chkSalesCreateReturns.Location = new Point(436, 93);
            chkSalesCreateReturns.Name = "chkSalesCreateReturns";
            chkSalesCreateReturns.Size = new Size(156, 24);
            chkSalesCreateReturns.TabIndex = 8;
            chkSalesCreateReturns.Tag = 128L;
            chkSalesCreateReturns.Text = "إنشاء المرتجعات";
            // 
            // chkSalesViewReceipts
            // 
            chkSalesViewReceipts.Location = new Point(445, 123);
            chkSalesViewReceipts.Name = "chkSalesViewReceipts";
            chkSalesViewReceipts.Size = new Size(147, 24);
            chkSalesViewReceipts.TabIndex = 9;
            chkSalesViewReceipts.Tag = 256L;
            chkSalesViewReceipts.Text = "عرض المقبوضات";
            // 
            // chkSalesCreateReceipts
            // 
            chkSalesCreateReceipts.Location = new Point(283, 3);
            chkSalesCreateReceipts.Name = "chkSalesCreateReceipts";
            chkSalesCreateReceipts.Size = new Size(147, 24);
            chkSalesCreateReceipts.TabIndex = 10;
            chkSalesCreateReceipts.Tag = 512L;
            chkSalesCreateReceipts.Text = "إنشاء المقبوضات";
            chkSalesCreateReceipts.CheckedChanged += chkSalesCreateReceipts_CheckedChanged;
            // 
            // chkSalesManagePrices
            // 
            chkSalesManagePrices.Location = new Point(283, 33);
            chkSalesManagePrices.Name = "chkSalesManagePrices";
            chkSalesManagePrices.Size = new Size(147, 24);
            chkSalesManagePrices.TabIndex = 11;
            chkSalesManagePrices.Tag = 1024L;
            chkSalesManagePrices.Text = "إدارة الأسعار";
            chkSalesManagePrices.CheckedChanged += chkSalesManagePrices_CheckedChanged;
            // 
            // chkSalesManageDiscounts
            // 
            chkSalesManageDiscounts.Location = new Point(287, 63);
            chkSalesManageDiscounts.Name = "chkSalesManageDiscounts";
            chkSalesManageDiscounts.Size = new Size(143, 24);
            chkSalesManageDiscounts.TabIndex = 12;
            chkSalesManageDiscounts.Tag = 2048L;
            chkSalesManageDiscounts.Text = "إدارة الخصومات";
            // 
            // grpInventory
            // 
            grpInventory.Controls.Add(flpInventory);
            grpInventory.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            grpInventory.Location = new Point(768, 383);
            grpInventory.Name = "grpInventory";
            grpInventory.Size = new Size(750, 210);
            grpInventory.TabIndex = 1;
            grpInventory.TabStop = false;
            grpInventory.Text = "صلاحيات المخزون";
            // 
            // flpInventory
            // 
            flpInventory.AutoScroll = true;
            flpInventory.Controls.Add(chkInventorySelectAll);
            flpInventory.Controls.Add(chkInventoryViewProducts);
            flpInventory.Controls.Add(chkInventoryCreateProducts);
            flpInventory.Controls.Add(chkInventoryEditProducts);
            flpInventory.Controls.Add(chkInventoryViewStock);
            flpInventory.Controls.Add(chkInventoryAdjustStock);
            flpInventory.Controls.Add(chkInventoryTransferStock);
            flpInventory.Controls.Add(chkInventoryCountStock);
            flpInventory.Controls.Add(chkInventoryViewBatches);
            flpInventory.Controls.Add(chkInventoryManageCategories);
            flpInventory.Controls.Add(chkInventoryManageUnits);
            flpInventory.Dock = DockStyle.Fill;
            flpInventory.FlowDirection = FlowDirection.TopDown;
            flpInventory.Font = new Font("Tahoma", 8F);
            flpInventory.Location = new Point(3, 28);
            flpInventory.Name = "flpInventory";
            flpInventory.Size = new Size(744, 179);
            flpInventory.TabIndex = 0;
            // 
            // chkInventorySelectAll
            // 
            chkInventorySelectAll.Location = new Point(600, 3);
            chkInventorySelectAll.Name = "chkInventorySelectAll";
            chkInventorySelectAll.Size = new Size(141, 24);
            chkInventorySelectAll.TabIndex = 0;
            chkInventorySelectAll.Tag = 0L;
            chkInventorySelectAll.Text = "☑ تحديد الكل";
            chkInventorySelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
            // 
            // chkInventoryViewProducts
            // 
            chkInventoryViewProducts.Location = new Point(600, 33);
            chkInventoryViewProducts.Name = "chkInventoryViewProducts";
            chkInventoryViewProducts.Size = new Size(141, 24);
            chkInventoryViewProducts.TabIndex = 1;
            chkInventoryViewProducts.Tag = 1L;
            chkInventoryViewProducts.Text = "عرض المنتجات";
            // 
            // chkInventoryCreateProducts
            // 
            chkInventoryCreateProducts.Location = new Point(600, 63);
            chkInventoryCreateProducts.Name = "chkInventoryCreateProducts";
            chkInventoryCreateProducts.Size = new Size(141, 24);
            chkInventoryCreateProducts.TabIndex = 2;
            chkInventoryCreateProducts.Tag = 2L;
            chkInventoryCreateProducts.Text = "إنشاء المنتجات";
            // 
            // chkInventoryEditProducts
            // 
            chkInventoryEditProducts.Location = new Point(595, 93);
            chkInventoryEditProducts.Name = "chkInventoryEditProducts";
            chkInventoryEditProducts.Size = new Size(146, 24);
            chkInventoryEditProducts.TabIndex = 3;
            chkInventoryEditProducts.Tag = 4L;
            chkInventoryEditProducts.Text = "تعديل المنتجات";
            // 
            // chkInventoryViewStock
            // 
            chkInventoryViewStock.Location = new Point(595, 123);
            chkInventoryViewStock.Name = "chkInventoryViewStock";
            chkInventoryViewStock.Size = new Size(146, 24);
            chkInventoryViewStock.TabIndex = 4;
            chkInventoryViewStock.Tag = 8L;
            chkInventoryViewStock.Text = "عرض المخزون";
            // 
            // chkInventoryAdjustStock
            // 
            chkInventoryAdjustStock.Location = new Point(443, 3);
            chkInventoryAdjustStock.Name = "chkInventoryAdjustStock";
            chkInventoryAdjustStock.Size = new Size(146, 24);
            chkInventoryAdjustStock.TabIndex = 5;
            chkInventoryAdjustStock.Tag = 16L;
            chkInventoryAdjustStock.Text = "تسوية المخزون";
            // 
            // chkInventoryTransferStock
            // 
            chkInventoryTransferStock.Location = new Point(455, 33);
            chkInventoryTransferStock.Name = "chkInventoryTransferStock";
            chkInventoryTransferStock.Size = new Size(134, 24);
            chkInventoryTransferStock.TabIndex = 6;
            chkInventoryTransferStock.Tag = 32L;
            chkInventoryTransferStock.Text = "تحويل مخزون";
            // 
            // chkInventoryCountStock
            // 
            chkInventoryCountStock.Location = new Point(455, 63);
            chkInventoryCountStock.Name = "chkInventoryCountStock";
            chkInventoryCountStock.Size = new Size(134, 24);
            chkInventoryCountStock.TabIndex = 7;
            chkInventoryCountStock.Tag = 64L;
            chkInventoryCountStock.Text = "جرد المخزون";
            // 
            // chkInventoryViewBatches
            // 
            chkInventoryViewBatches.Location = new Point(463, 93);
            chkInventoryViewBatches.Name = "chkInventoryViewBatches";
            chkInventoryViewBatches.Size = new Size(126, 24);
            chkInventoryViewBatches.TabIndex = 8;
            chkInventoryViewBatches.Tag = 128L;
            chkInventoryViewBatches.Text = "عرض الدفعات";
            // 
            // chkInventoryManageCategories
            // 
            chkInventoryManageCategories.Location = new Point(444, 123);
            chkInventoryManageCategories.Name = "chkInventoryManageCategories";
            chkInventoryManageCategories.Size = new Size(145, 24);
            chkInventoryManageCategories.TabIndex = 9;
            chkInventoryManageCategories.Tag = 256L;
            chkInventoryManageCategories.Text = "إدارة التصنيفات";
            // 
            // chkInventoryManageUnits
            // 
            chkInventoryManageUnits.Location = new Point(303, 3);
            chkInventoryManageUnits.Name = "chkInventoryManageUnits";
            chkInventoryManageUnits.Size = new Size(134, 24);
            chkInventoryManageUnits.TabIndex = 10;
            chkInventoryManageUnits.Tag = 512L;
            chkInventoryManageUnits.Text = "إدارة الوحدات";
            // 
            // grpAccounting
            // 
            grpAccounting.Controls.Add(flpAccounting);
            grpAccounting.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            grpAccounting.Location = new Point(765, 162);
            grpAccounting.Name = "grpAccounting";
            grpAccounting.Size = new Size(750, 210);
            grpAccounting.TabIndex = 2;
            grpAccounting.TabStop = false;
            grpAccounting.Text = "صلاحيات المحاسبة";
            grpAccounting.Enter += grpAccounting_Enter;
            // 
            // flpAccounting
            // 
            flpAccounting.AutoScroll = true;
            flpAccounting.Controls.Add(chkAccountingSelectAll);
            flpAccounting.Controls.Add(chkAccountingViewJournalEntries);
            flpAccounting.Controls.Add(chkAccountingCreateJournalEntries);
            flpAccounting.Controls.Add(chkAccountingViewAccounts);
            flpAccounting.Controls.Add(chkAccountingManageAccounts);
            flpAccounting.Controls.Add(chkAccountingViewCashBoxes);
            flpAccounting.Controls.Add(chkAccountingManageCashBoxes);
            flpAccounting.Controls.Add(chkAccountingViewExpenses);
            flpAccounting.Controls.Add(chkAccountingCreateExpenses);
            flpAccounting.Controls.Add(chkAccountingViewReceiptVouchers);
            flpAccounting.Controls.Add(chkAccountingCreateReceiptVouchers);
            flpAccounting.Controls.Add(chkAccountingViewPaymentVouchers);
            flpAccounting.Controls.Add(chkAccountingCreatePaymentVouchers);
            flpAccounting.Controls.Add(chkAccountingViewReports);
            flpAccounting.Dock = DockStyle.Fill;
            flpAccounting.FlowDirection = FlowDirection.TopDown;
            flpAccounting.Font = new Font("Tahoma", 8F);
            flpAccounting.Location = new Point(3, 28);
            flpAccounting.Name = "flpAccounting";
            flpAccounting.Size = new Size(744, 179);
            flpAccounting.TabIndex = 0;
            // 
            // chkAccountingSelectAll
            // 
            chkAccountingSelectAll.Location = new Point(597, 3);
            chkAccountingSelectAll.Name = "chkAccountingSelectAll";
            chkAccountingSelectAll.Size = new Size(144, 24);
            chkAccountingSelectAll.TabIndex = 0;
            chkAccountingSelectAll.Tag = 0L;
            chkAccountingSelectAll.Text = "☑ تحديد الكل";
            chkAccountingSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
            // 
            // chkAccountingViewJournalEntries
            // 
            chkAccountingViewJournalEntries.Location = new Point(616, 33);
            chkAccountingViewJournalEntries.Name = "chkAccountingViewJournalEntries";
            chkAccountingViewJournalEntries.Size = new Size(125, 24);
            chkAccountingViewJournalEntries.TabIndex = 1;
            chkAccountingViewJournalEntries.Tag = 1L;
            chkAccountingViewJournalEntries.Text = "عرض قيود اليومية";
            // 
            // chkAccountingCreateJournalEntries
            // 
            chkAccountingCreateJournalEntries.Location = new Point(616, 63);
            chkAccountingCreateJournalEntries.Name = "chkAccountingCreateJournalEntries";
            chkAccountingCreateJournalEntries.Size = new Size(125, 24);
            chkAccountingCreateJournalEntries.TabIndex = 2;
            chkAccountingCreateJournalEntries.Tag = 2L;
            chkAccountingCreateJournalEntries.Text = "إنشاء قيود اليومية";
            // 
            // chkAccountingViewAccounts
            // 
            chkAccountingViewAccounts.Location = new Point(597, 93);
            chkAccountingViewAccounts.Name = "chkAccountingViewAccounts";
            chkAccountingViewAccounts.Size = new Size(144, 24);
            chkAccountingViewAccounts.TabIndex = 3;
            chkAccountingViewAccounts.Tag = 4L;
            chkAccountingViewAccounts.Text = "عرض الحسابات";
            // 
            // chkAccountingManageAccounts
            // 
            chkAccountingManageAccounts.Location = new Point(597, 123);
            chkAccountingManageAccounts.Name = "chkAccountingManageAccounts";
            chkAccountingManageAccounts.Size = new Size(144, 24);
            chkAccountingManageAccounts.TabIndex = 4;
            chkAccountingManageAccounts.Tag = 8L;
            chkAccountingManageAccounts.Text = "إدارة الحسابات";
            // 
            // chkAccountingViewCashBoxes
            // 
            chkAccountingViewCashBoxes.Location = new Point(447, 3);
            chkAccountingViewCashBoxes.Name = "chkAccountingViewCashBoxes";
            chkAccountingViewCashBoxes.Size = new Size(144, 24);
            chkAccountingViewCashBoxes.TabIndex = 5;
            chkAccountingViewCashBoxes.Tag = 16L;
            chkAccountingViewCashBoxes.Text = "عرض الصناديق";
            // 
            // chkAccountingManageCashBoxes
            // 
            chkAccountingManageCashBoxes.Location = new Point(452, 33);
            chkAccountingManageCashBoxes.Name = "chkAccountingManageCashBoxes";
            chkAccountingManageCashBoxes.Size = new Size(139, 24);
            chkAccountingManageCashBoxes.TabIndex = 6;
            chkAccountingManageCashBoxes.Tag = 32L;
            chkAccountingManageCashBoxes.Text = "إدارة الصناديق";
            // 
            // chkAccountingViewExpenses
            // 
            chkAccountingViewExpenses.Location = new Point(441, 63);
            chkAccountingViewExpenses.Name = "chkAccountingViewExpenses";
            chkAccountingViewExpenses.Size = new Size(150, 24);
            chkAccountingViewExpenses.TabIndex = 7;
            chkAccountingViewExpenses.Tag = 64L;
            chkAccountingViewExpenses.Text = "عرض المصروفات";
            // 
            // chkAccountingCreateExpenses
            // 
            chkAccountingCreateExpenses.Location = new Point(441, 93);
            chkAccountingCreateExpenses.Name = "chkAccountingCreateExpenses";
            chkAccountingCreateExpenses.Size = new Size(150, 24);
            chkAccountingCreateExpenses.TabIndex = 8;
            chkAccountingCreateExpenses.Tag = 128L;
            chkAccountingCreateExpenses.Text = "إنشاء المصروفات";
            // 
            // chkAccountingViewReceiptVouchers
            // 
            chkAccountingViewReceiptVouchers.Location = new Point(441, 123);
            chkAccountingViewReceiptVouchers.Name = "chkAccountingViewReceiptVouchers";
            chkAccountingViewReceiptVouchers.Size = new Size(150, 24);
            chkAccountingViewReceiptVouchers.TabIndex = 9;
            chkAccountingViewReceiptVouchers.Tag = 256L;
            chkAccountingViewReceiptVouchers.Text = "عرض سندات القبض";
            // 
            // chkAccountingCreateReceiptVouchers
            // 
            chkAccountingCreateReceiptVouchers.Location = new Point(285, 3);
            chkAccountingCreateReceiptVouchers.Name = "chkAccountingCreateReceiptVouchers";
            chkAccountingCreateReceiptVouchers.Size = new Size(150, 24);
            chkAccountingCreateReceiptVouchers.TabIndex = 10;
            chkAccountingCreateReceiptVouchers.Tag = 512L;
            chkAccountingCreateReceiptVouchers.Text = "إنشاء سندات القبض";
            // 
            // chkAccountingViewPaymentVouchers
            // 
            chkAccountingViewPaymentVouchers.Location = new Point(296, 33);
            chkAccountingViewPaymentVouchers.Name = "chkAccountingViewPaymentVouchers";
            chkAccountingViewPaymentVouchers.Size = new Size(139, 24);
            chkAccountingViewPaymentVouchers.TabIndex = 11;
            chkAccountingViewPaymentVouchers.Tag = 1024L;
            chkAccountingViewPaymentVouchers.Text = "عرض سندات الصرف";
            // 
            // chkAccountingCreatePaymentVouchers
            // 
            chkAccountingCreatePaymentVouchers.Location = new Point(296, 63);
            chkAccountingCreatePaymentVouchers.Name = "chkAccountingCreatePaymentVouchers";
            chkAccountingCreatePaymentVouchers.Size = new Size(139, 24);
            chkAccountingCreatePaymentVouchers.TabIndex = 12;
            chkAccountingCreatePaymentVouchers.Tag = 2048L;
            chkAccountingCreatePaymentVouchers.Text = "إنشاء سندات الصرف";
            // 
            // chkAccountingViewReports
            // 
            chkAccountingViewReports.Location = new Point(285, 93);
            chkAccountingViewReports.Name = "chkAccountingViewReports";
            chkAccountingViewReports.Size = new Size(150, 24);
            chkAccountingViewReports.TabIndex = 13;
            chkAccountingViewReports.Tag = 4096L;
            chkAccountingViewReports.Text = "عرض التقارير";
            // 
            // grpSystem
            // 
            grpSystem.Controls.Add(flpSystem);
            grpSystem.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            grpSystem.Location = new Point(12, 383);
            grpSystem.Name = "grpSystem";
            grpSystem.Size = new Size(750, 210);
            grpSystem.TabIndex = 3;
            grpSystem.TabStop = false;
            grpSystem.Text = "صلاحيات النظام";
            // 
            // flpSystem
            // 
            flpSystem.AutoScroll = true;
            flpSystem.Controls.Add(chkSystemSelectAll);
            flpSystem.Controls.Add(chkSystemManageUsers);
            flpSystem.Controls.Add(chkSystemManageRoles);
            flpSystem.Controls.Add(chkSystemManageSettings);
            flpSystem.Controls.Add(chkSystemViewAuditLogs);
            flpSystem.Controls.Add(chkSystemManageBranches);
            flpSystem.Controls.Add(chkSystemManageBackups);
            flpSystem.Controls.Add(chkSystemManageFiscalYears);
            flpSystem.Controls.Add(chkSystemManageTaxes);
            flpSystem.Dock = DockStyle.Fill;
            flpSystem.FlowDirection = FlowDirection.TopDown;
            flpSystem.Font = new Font("Tahoma", 8F);
            flpSystem.Location = new Point(3, 28);
            flpSystem.Name = "flpSystem";
            flpSystem.Size = new Size(744, 179);
            flpSystem.TabIndex = 0;
            // 
            // chkSystemSelectAll
            // 
            chkSystemSelectAll.Location = new Point(602, 3);
            chkSystemSelectAll.Name = "chkSystemSelectAll";
            chkSystemSelectAll.Size = new Size(139, 24);
            chkSystemSelectAll.TabIndex = 0;
            chkSystemSelectAll.Tag = 0L;
            chkSystemSelectAll.Text = "☑ تحديد الكل";
            chkSystemSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
            // 
            // chkSystemManageUsers
            // 
            chkSystemManageUsers.Location = new Point(569, 33);
            chkSystemManageUsers.Name = "chkSystemManageUsers";
            chkSystemManageUsers.Size = new Size(172, 24);
            chkSystemManageUsers.TabIndex = 1;
            chkSystemManageUsers.Tag = 1L;
            chkSystemManageUsers.Text = "إدارة المستخدمين";
            // 
            // chkSystemManageRoles
            // 
            chkSystemManageRoles.Location = new Point(584, 63);
            chkSystemManageRoles.Name = "chkSystemManageRoles";
            chkSystemManageRoles.Size = new Size(157, 24);
            chkSystemManageRoles.TabIndex = 2;
            chkSystemManageRoles.Tag = 2L;
            chkSystemManageRoles.Text = "إدارة الأدوار";
            // 
            // chkSystemManageSettings
            // 
            chkSystemManageSettings.Location = new Point(584, 93);
            chkSystemManageSettings.Name = "chkSystemManageSettings";
            chkSystemManageSettings.Size = new Size(157, 24);
            chkSystemManageSettings.TabIndex = 3;
            chkSystemManageSettings.Tag = 4L;
            chkSystemManageSettings.Text = "إدارة الإعدادات";
            // 
            // chkSystemViewAuditLogs
            // 
            chkSystemViewAuditLogs.Location = new Point(595, 123);
            chkSystemViewAuditLogs.Name = "chkSystemViewAuditLogs";
            chkSystemViewAuditLogs.Size = new Size(146, 24);
            chkSystemViewAuditLogs.TabIndex = 4;
            chkSystemViewAuditLogs.Tag = 8L;
            chkSystemViewAuditLogs.Text = "عرض سجل التدقيق";
            // 
            // chkSystemManageBranches
            // 
            chkSystemManageBranches.Location = new Point(417, 3);
            chkSystemManageBranches.Name = "chkSystemManageBranches";
            chkSystemManageBranches.Size = new Size(146, 24);
            chkSystemManageBranches.TabIndex = 5;
            chkSystemManageBranches.Tag = 16L;
            chkSystemManageBranches.Text = "إدارة الفروع";
            // 
            // chkSystemManageBackups
            // 
            chkSystemManageBackups.Location = new Point(417, 33);
            chkSystemManageBackups.Name = "chkSystemManageBackups";
            chkSystemManageBackups.Size = new Size(146, 24);
            chkSystemManageBackups.TabIndex = 6;
            chkSystemManageBackups.Tag = 32L;
            chkSystemManageBackups.Text = "إدارة النسخ الاحتياطي";
            // 
            // chkSystemManageFiscalYears
            // 
            chkSystemManageFiscalYears.Location = new Point(417, 63);
            chkSystemManageFiscalYears.Name = "chkSystemManageFiscalYears";
            chkSystemManageFiscalYears.Size = new Size(146, 24);
            chkSystemManageFiscalYears.TabIndex = 7;
            chkSystemManageFiscalYears.Tag = 64L;
            chkSystemManageFiscalYears.Text = "إدارة السنوات المالية";
            // 
            // chkSystemManageTaxes
            // 
            chkSystemManageTaxes.Location = new Point(414, 93);
            chkSystemManageTaxes.Name = "chkSystemManageTaxes";
            chkSystemManageTaxes.Size = new Size(149, 24);
            chkSystemManageTaxes.TabIndex = 8;
            chkSystemManageTaxes.Tag = 128L;
            chkSystemManageTaxes.Text = "إدارة الضرائب";
            // 
            // frmAddUpdateRole
            // 
            AutoScaleDimensions = new SizeF(13F, 29F);
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1526, 680);
            Controls.Add(grpSales);
            Controls.Add(grpInventory);
            Controls.Add(grpAccounting);
            Controls.Add(grpSystem);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtDescription);
            Controls.Add(lblDescription);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Controls.Add(lblMode);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddUpdateRole";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة دور جديد";
            grpSales.ResumeLayout(false);
            flpSales.ResumeLayout(false);
            grpInventory.ResumeLayout(false);
            flpInventory.ResumeLayout(false);
            grpAccounting.ResumeLayout(false);
            flpAccounting.ResumeLayout(false);
            grpSystem.ResumeLayout(false);
            flpSystem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        // Sales permissions
        private System.Windows.Forms.GroupBox grpSales;
        private System.Windows.Forms.FlowLayoutPanel flpSales;
        private System.Windows.Forms.CheckBox chkSalesSelectAll;
        private System.Windows.Forms.CheckBox chkSalesViewInvoices;
        private System.Windows.Forms.CheckBox chkSalesCreateInvoices;
        private System.Windows.Forms.CheckBox chkSalesEditInvoices;
        private System.Windows.Forms.CheckBox chkSalesDeleteInvoices;
        private System.Windows.Forms.CheckBox chkSalesCancelInvoices;
        private System.Windows.Forms.CheckBox chkSalesPrintInvoices;
        private System.Windows.Forms.CheckBox chkSalesViewReturns;
        private System.Windows.Forms.CheckBox chkSalesCreateReturns;
        private System.Windows.Forms.CheckBox chkSalesViewReceipts;
        private System.Windows.Forms.CheckBox chkSalesCreateReceipts;
        private System.Windows.Forms.CheckBox chkSalesManagePrices;
        private System.Windows.Forms.CheckBox chkSalesManageDiscounts;

        // Inventory permissions
        private System.Windows.Forms.GroupBox grpInventory;
        private System.Windows.Forms.FlowLayoutPanel flpInventory;
        private System.Windows.Forms.CheckBox chkInventorySelectAll;
        private System.Windows.Forms.CheckBox chkInventoryViewProducts;
        private System.Windows.Forms.CheckBox chkInventoryCreateProducts;
        private System.Windows.Forms.CheckBox chkInventoryEditProducts;
        private System.Windows.Forms.CheckBox chkInventoryViewStock;
        private System.Windows.Forms.CheckBox chkInventoryAdjustStock;
        private System.Windows.Forms.CheckBox chkInventoryTransferStock;
        private System.Windows.Forms.CheckBox chkInventoryCountStock;
        private System.Windows.Forms.CheckBox chkInventoryViewBatches;
        private System.Windows.Forms.CheckBox chkInventoryManageCategories;
        private System.Windows.Forms.CheckBox chkInventoryManageUnits;

        // Accounting permissions
        private System.Windows.Forms.GroupBox grpAccounting;
        private System.Windows.Forms.FlowLayoutPanel flpAccounting;
        private System.Windows.Forms.CheckBox chkAccountingSelectAll;
        private System.Windows.Forms.CheckBox chkAccountingViewJournalEntries;
        private System.Windows.Forms.CheckBox chkAccountingCreateJournalEntries;
        private System.Windows.Forms.CheckBox chkAccountingViewAccounts;
        private System.Windows.Forms.CheckBox chkAccountingManageAccounts;
        private System.Windows.Forms.CheckBox chkAccountingViewCashBoxes;
        private System.Windows.Forms.CheckBox chkAccountingManageCashBoxes;
        private System.Windows.Forms.CheckBox chkAccountingViewExpenses;
        private System.Windows.Forms.CheckBox chkAccountingCreateExpenses;
        private System.Windows.Forms.CheckBox chkAccountingViewReceiptVouchers;
        private System.Windows.Forms.CheckBox chkAccountingCreateReceiptVouchers;
        private System.Windows.Forms.CheckBox chkAccountingViewPaymentVouchers;
        private System.Windows.Forms.CheckBox chkAccountingCreatePaymentVouchers;
        private System.Windows.Forms.CheckBox chkAccountingViewReports;

        // System permissions
        private System.Windows.Forms.GroupBox grpSystem;
        private System.Windows.Forms.FlowLayoutPanel flpSystem;
        private System.Windows.Forms.CheckBox chkSystemSelectAll;
        private System.Windows.Forms.CheckBox chkSystemManageUsers;
        private System.Windows.Forms.CheckBox chkSystemManageRoles;
        private System.Windows.Forms.CheckBox chkSystemManageSettings;
        private System.Windows.Forms.CheckBox chkSystemViewAuditLogs;
        private System.Windows.Forms.CheckBox chkSystemManageBranches;
        private System.Windows.Forms.CheckBox chkSystemManageBackups;
        private System.Windows.Forms.CheckBox chkSystemManageFiscalYears;
        private System.Windows.Forms.CheckBox chkSystemManageTaxes;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
