namespace SalesSystem.Desktop.Screens
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            msMainMenu = new MenuStrip();
            // ── المنتجات ────────────────────────────────────────────────────────────────────────────────
            msBasis = new ToolStripMenuItem();
            msbtnManageProducts = new ToolStripMenuItem();
            msbtnAddProduct = new ToolStripMenuItem();
            msbtnManageCustomers = new ToolStripMenuItem();
            msbtnAddCustomer = new ToolStripMenuItem();
            msbtnManageSuppliers = new ToolStripMenuItem();
            msbtnAddSupplier = new ToolStripMenuItem();
            msbtnManageCategories = new ToolStripMenuItem();
            msbtnManageUnits = new ToolStripMenuItem();
            msbtnProductPrices = new ToolStripMenuItem();
            msbtnProductUnits = new ToolStripMenuItem();
            msbtnProductCategories = new ToolStripMenuItem();
            // ── المخزون ──────────────────────────────────────────────────────────────────────────────
            msStore = new ToolStripMenuItem();
            msbtnManageWarehouses = new ToolStripMenuItem();
            msbtnStockTransfer = new ToolStripMenuItem();
            msbtnInventoryMovement = new ToolStripMenuItem();
            msbtnInventoryAdjustments = new ToolStripMenuItem();
            msbtnInventoryCounts = new ToolStripMenuItem();
            // ── المشتريات ──────────────────────────────────────────────────────────────────────────
            msBuy = new ToolStripMenuItem();
            msbtnManagePurchases = new ToolStripMenuItem();
            msbtnPurchaseInvoice = new ToolStripMenuItem();
            msbtnPurchaseReturn = new ToolStripMenuItem();
            // ── المبيعات ────────────────────────────────────────────────────────────────────────────
            msSale = new ToolStripMenuItem();
            msbtnManageSales = new ToolStripMenuItem();
            msbtnSalesInvoice = new ToolStripMenuItem();
            msbtnSalesReturn = new ToolStripMenuItem();
            // ── الخزينة ──────────────────────────────────────────────────────────────────────────────
            msRevenue = new ToolStripMenuItem();
            msbtnManagePayments = new ToolStripMenuItem();
            msbtnCashBoxes = new ToolStripMenuItem();
            msbtnBanks = new ToolStripMenuItem();
            msbtnExpenses = new ToolStripMenuItem();
            msbtnCustomerReceipts = new ToolStripMenuItem();
            msbtnPaymentVouchers = new ToolStripMenuItem();
            msbtnSupplierPayments = new ToolStripMenuItem();
            // ── حسابات ────────────────────────────────────────────────────────────────────────────────
            msAccounting = new ToolStripMenuItem();
            msbtnChartOfAccounts = new ToolStripMenuItem();
            msbtnJournalEntries = new ToolStripMenuItem();
            msbtnAccountCategories = new ToolStripMenuItem();
            msbtnSystemAccountMappings = new ToolStripMenuItem();
            msbtnReceiptVouchers = new ToolStripMenuItem();
            // ── بيانات ────────────────────────────────────────────────────────────────────────────────
            msData = new ToolStripMenuItem();
            msbtnManageUsers = new ToolStripMenuItem();
            msbtnRoles = new ToolStripMenuItem();
            msbtnPermissions = new ToolStripMenuItem();
            msbtnAuditLog = new ToolStripMenuItem();
            msbtnSystemLogs = new ToolStripMenuItem();
            msbtnUserSessions = new ToolStripMenuItem();
            msbtnDocumentSequences = new ToolStripMenuItem();
            msbtnTaxes = new ToolStripMenuItem();
            // ── تقارير ────────────────────────────────────────────────────────────────────────────────
            msReport = new ToolStripMenuItem();
            msbtnReports = new ToolStripMenuItem();
            // ── الإعدادات ────────────────────────────────────────────────────────────────────────────
            msSettings = new ToolStripMenuItem();
            msbtnSettings = new ToolStripMenuItem();
            msbtnCompanySettings = new ToolStripMenuItem();
            msbtnNotifications = new ToolStripMenuItem();
            msbtnUpdateCheck = new ToolStripMenuItem();
            msbtnBackup = new ToolStripMenuItem();
            // ── إعدادات الحساب ────────────────────────────────────────────────────────────────────────
            msAccoutSettings = new ToolStripMenuItem();
            msbtnLogout = new ToolStripMenuItem();
            // ── ملف ──────────────────────────────────────────────────────────────────────────────────
            msFile = new ToolStripMenuItem();
            msbtnChangePassword = new ToolStripMenuItem();
            msbtnExit = new ToolStripMenuItem();
            // ── المبيعات — نقطة البيع وأدوات الفاتورة (جديد) ───────────────────────────────────────
            msbtnPOS = new ToolStripMenuItem();
            msbtnProductSelection = new ToolStripMenuItem();
            msbtnCustomerPicker = new ToolStripMenuItem();
            msbtnDiscountExpenses = new ToolStripMenuItem();
            msbtnCurrencySelection = new ToolStripMenuItem();
            // ── الموظفون (جديد) ────────────────────────────────────────────────────────────────────
            msbtnEmployees = new ToolStripMenuItem();
            // ── شاشات الوحدات الرئيسية (جديد) ────────────────────────────────────────────────
            msbtnCashVouchers = new ToolStripMenuItem();
            msbtnPurchases = new ToolStripMenuItem();
            msbtnInventory = new ToolStripMenuItem();
            msbtnAccounting = new ToolStripMenuItem();
            msbtnReportsDashboard = new ToolStripMenuItem();

            errorProvider1 = new ErrorProvider(components);
            msMainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();

            // 
            // msMainMenu
            // 
            msMainMenu.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            msMainMenu.GripStyle = ToolStripGripStyle.Visible;
            msMainMenu.ImageScalingSize = new Size(24, 24);
            msMainMenu.Items.AddRange(new ToolStripItem[] {
                msFile, msSale, msBuy, msBasis, msStore, msRevenue,
                msAccounting, msData, msReport, msSettings, msAccoutSettings
            });
            msMainMenu.Location = new Point(0, 0);
            msMainMenu.Name = "msMainMenu";
            msMainMenu.Size = new Size(1378, 37);
            msMainMenu.TabIndex = 0;
            msMainMenu.Text = "Main Menu";
            // 
            // msFile
            // 
            msFile.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnChangePassword,
                new ToolStripSeparator(),
                msbtnExit
            });
            msFile.Name = "msFile";
            msFile.Size = new Size(62, 33);
            msFile.Text = "ملف";
            // 
            // msbtnChangePassword
            // 
            msbtnChangePassword.Name = "msbtnChangePassword";
            msbtnChangePassword.Size = new Size(300, 38);
            msbtnChangePassword.Text = "تغيير كلمة المرور";
            msbtnChangePassword.Click += msbtnChangePassword_Click;
            // 
            // msbtnExit
            // 
            msbtnExit.Name = "msbtnExit";
            msbtnExit.Size = new Size(300, 38);
            msbtnExit.Text = "خروج";
            msbtnExit.Click += msbtnExit_Click;
            // 
            // msBasis
            // 
            msBasis.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnManageProducts, msbtnAddProduct,
                new ToolStripSeparator(),
                msbtnProductPrices, msbtnProductUnits, msbtnProductCategories
            });
            msBasis.Name = "msBasis";
            msBasis.Size = new Size(105, 33);
            msBasis.Text = "المنتجات";
            // 
            // msbtnManageProducts
            // 
            msbtnManageProducts.Name = "msbtnManageProducts";
            msbtnManageProducts.Size = new Size(300, 38);
            msbtnManageProducts.Text = "إدارة المنتجات";
            msbtnManageProducts.Click += msbtnManageProducts_Click;
            // 
            // msbtnAddProduct
            // 
            msbtnAddProduct.Name = "msbtnAddProduct";
            msbtnAddProduct.Size = new Size(300, 38);
            msbtnAddProduct.Text = "إضافة منتج جديد";
            msbtnAddProduct.Click += msbtnAddProduct_Click;
            // 
            // msbtnManageCustomers
            // 
            msbtnManageCustomers.Name = "msbtnManageCustomers";
            msbtnManageCustomers.Size = new Size(300, 38);
            msbtnManageCustomers.Text = "إدارة العملاء";
            msbtnManageCustomers.Click += msbtnManageCustomers_Click;
            // 
            // msbtnAddCustomer
            // 
            msbtnAddCustomer.Name = "msbtnAddCustomer";
            msbtnAddCustomer.Size = new Size(300, 38);
            msbtnAddCustomer.Text = "إضافة عميل جديد";
            msbtnAddCustomer.Click += msbtnAddCustomer_Click;
            // 
            // msbtnManageSuppliers
            // 
            msbtnManageSuppliers.Name = "msbtnManageSuppliers";
            msbtnManageSuppliers.Size = new Size(300, 38);
            msbtnManageSuppliers.Text = "إدارة الموردين";
            msbtnManageSuppliers.Click += msbtnManageSuppliers_Click;
            // 
            // msbtnAddSupplier
            // 
            msbtnAddSupplier.Name = "msbtnAddSupplier";
            msbtnAddSupplier.Size = new Size(300, 38);
            msbtnAddSupplier.Text = "إضافة مورد جديد";
            msbtnAddSupplier.Click += msbtnAddSupplier_Click;
            // 
            // msbtnManageCategories
            // 
            msbtnManageCategories.Name = "msbtnManageCategories";
            msbtnManageCategories.Size = new Size(300, 38);
            msbtnManageCategories.Text = "التصنيفات";
            msbtnManageCategories.Click += msbtnManageCategories_Click;
            // 
            // msbtnManageUnits
            // 
            msbtnManageUnits.Name = "msbtnManageUnits";
            msbtnManageUnits.Size = new Size(300, 38);
            msbtnManageUnits.Text = "الوحدات";
            msbtnManageUnits.Click += msbtnManageUnits_Click;
            // 
            // msbtnProductPrices
            // 
            msbtnProductPrices.Name = "msbtnProductPrices";
            msbtnProductPrices.Size = new Size(300, 38);
            msbtnProductPrices.Text = "أسعار المنتجات";
            msbtnProductPrices.Click += msbtnProductPrices_Click;
            // 
            // msbtnProductUnits
            // 
            msbtnProductUnits.Name = "msbtnProductUnits";
            msbtnProductUnits.Size = new Size(300, 38);
            msbtnProductUnits.Text = "وحدات المنتجات";
            msbtnProductUnits.Click += msbtnProductUnits_Click;
            // 
            // msbtnProductCategories
            // 
            msbtnProductCategories.Name = "msbtnProductCategories";
            msbtnProductCategories.Size = new Size(300, 38);
            msbtnProductCategories.Text = "تصنيفات المنتجات";
            msbtnProductCategories.Click += msbtnProductCategories_Click;
            // 
            // msStore
            // 
            msStore.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnInventory,
                new ToolStripSeparator(),
                msbtnManageWarehouses, msbtnStockTransfer, msbtnInventoryMovement,
                msbtnInventoryAdjustments, msbtnInventoryCounts
            });
            msStore.Name = "msStore";
            msStore.Size = new Size(95, 33);
            msStore.Text = "المخزون";
            // 
            // msbtnInventory
            // 
            msbtnInventory.Name = "msbtnInventory";
            msbtnInventory.Size = new Size(300, 38);
            msbtnInventory.Text = "إدارة المخزون";
            msbtnInventory.Click += msbtnInventory_Click;
            // 
            // msbtnManageWarehouses
            // 
            msbtnManageWarehouses.Name = "msbtnManageWarehouses";
            msbtnManageWarehouses.Size = new Size(300, 38);
            msbtnManageWarehouses.Text = "إدارة المخازن";
            msbtnManageWarehouses.Click += msbtnManageWarehouses_Click;
            // 
            // msbtnStockTransfer
            // 
            msbtnStockTransfer.Name = "msbtnStockTransfer";
            msbtnStockTransfer.Size = new Size(300, 38);
            msbtnStockTransfer.Text = "تحويل مخزون";
            msbtnStockTransfer.Click += msbtnStockTransfer_Click;
            // 
            // msbtnInventoryMovement
            // 
            msbtnInventoryMovement.Name = "msbtnInventoryMovement";
            msbtnInventoryMovement.Size = new Size(300, 38);
            msbtnInventoryMovement.Text = "حركة مخزون";
            msbtnInventoryMovement.Click += msbtnInventoryMovement_Click;
            // 
            // msbtnInventoryAdjustments
            // 
            msbtnInventoryAdjustments.Name = "msbtnInventoryAdjustments";
            msbtnInventoryAdjustments.Size = new Size(300, 38);
            msbtnInventoryAdjustments.Text = "تعديل المخزون";
            msbtnInventoryAdjustments.Click += msbtnInventoryAdjustments_Click;
            // 
            // msbtnInventoryCounts
            // 
            msbtnInventoryCounts.Name = "msbtnInventoryCounts";
            msbtnInventoryCounts.Size = new Size(300, 38);
            msbtnInventoryCounts.Text = "جرد مخزني";
            msbtnInventoryCounts.Click += msbtnInventoryCounts_Click;
            // 
            // msBuy
            // 
            msBuy.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnPurchases,
                new ToolStripSeparator(),
                msbtnPurchaseInvoice, msbtnPurchaseReturn, msbtnManagePurchases,
                new ToolStripSeparator(),
                msbtnManageSuppliers, msbtnAddSupplier,
                new ToolStripSeparator(),
                msbtnPaymentVouchers, msbtnSupplierPayments
            });
            msBuy.Name = "msBuy";
            msBuy.Size = new Size(115, 33);
            msBuy.Text = "المشتريات";
            // 
            // msbtnPurchases
            // 
            msbtnPurchases.Name = "msbtnPurchases";
            msbtnPurchases.Size = new Size(300, 38);
            msbtnPurchases.Text = "إدارة المشتريات";
            msbtnPurchases.Click += msbtnPurchases_Click;
            // 
            // msbtnManagePurchases
            // 
            msbtnManagePurchases.Name = "msbtnManagePurchases";
            msbtnManagePurchases.Size = new Size(300, 38);
            msbtnManagePurchases.Text = "إدارة المشتريات";
            msbtnManagePurchases.Click += msbtnManagePurchases_Click;
            // 
            // msbtnPurchaseInvoice
            // 
            msbtnPurchaseInvoice.Name = "msbtnPurchaseInvoice";
            msbtnPurchaseInvoice.Size = new Size(300, 38);
            msbtnPurchaseInvoice.Text = "فاتورة شراء";
            msbtnPurchaseInvoice.Click += msbtnPurchaseInvoice_Click;
            // 
            // msbtnPurchaseReturn
            // 
            msbtnPurchaseReturn.Name = "msbtnPurchaseReturn";
            msbtnPurchaseReturn.Size = new Size(300, 38);
            msbtnPurchaseReturn.Text = "مرتجع شراء";
            msbtnPurchaseReturn.Click += msbtnPurchaseReturn_Click;
            // 
            // msSale
            // 
            msSale.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnPOS,
                new ToolStripSeparator(),
                msbtnSalesInvoice, msbtnManageSales, msbtnSalesReturn,
                new ToolStripSeparator(),
                msbtnProductSelection, msbtnCustomerPicker, msbtnDiscountExpenses, msbtnCurrencySelection,
                new ToolStripSeparator(),
                msbtnManageCustomers, msbtnAddCustomer,
                new ToolStripSeparator(),
                msbtnCustomerReceipts, msbtnManagePayments
            });
            msSale.Name = "msSale";
            msSale.Size = new Size(105, 33);
            msSale.Text = "المبيعات";
            // 
            // msbtnManageSales
            // 
            msbtnManageSales.Name = "msbtnManageSales";
            msbtnManageSales.Size = new Size(300, 38);
            msbtnManageSales.Text = "إدارة المبيعات";
            msbtnManageSales.Click += msbtnManageSales_Click;
            // 
            // msbtnSalesInvoice
            // 
            msbtnSalesInvoice.Name = "msbtnSalesInvoice";
            msbtnSalesInvoice.Size = new Size(300, 38);
            msbtnSalesInvoice.Text = "فاتورة بيع";
            msbtnSalesInvoice.Click += msbtnSalesInvoice_Click;
            // 
            // msbtnSalesReturn
            // 
            msbtnSalesReturn.Name = "msbtnSalesReturn";
            msbtnSalesReturn.Size = new Size(300, 38);
            msbtnSalesReturn.Text = "مرتجع بيع";
            msbtnSalesReturn.Click += msbtnSalesReturn_Click;
            // 
            // msbtnPOS
            // 
            msbtnPOS.Name = "msbtnPOS";
            msbtnPOS.Size = new Size(300, 38);
            msbtnPOS.Text = "نقطة البيع";
            msbtnPOS.Click += msbtnPOS_Click;
            // 
            // msbtnProductSelection
            // 
            msbtnProductSelection.Name = "msbtnProductSelection";
            msbtnProductSelection.Size = new Size(300, 38);
            msbtnProductSelection.Text = "اختيار منتج [F3]";
            msbtnProductSelection.Click += msbtnProductSelection_Click;
            // 
            // msbtnCustomerPicker
            // 
            msbtnCustomerPicker.Name = "msbtnCustomerPicker";
            msbtnCustomerPicker.Size = new Size(300, 38);
            msbtnCustomerPicker.Text = "اختيار عميل";
            msbtnCustomerPicker.Click += msbtnCustomerPicker_Click;
            // 
            // msbtnDiscountExpenses
            // 
            msbtnDiscountExpenses.Name = "msbtnDiscountExpenses";
            msbtnDiscountExpenses.Size = new Size(300, 38);
            msbtnDiscountExpenses.Text = "الخصم والمصروفات [F7]";
            msbtnDiscountExpenses.Click += msbtnDiscountExpenses_Click;
            // 
            // msbtnCurrencySelection
            // 
            msbtnCurrencySelection.Name = "msbtnCurrencySelection";
            msbtnCurrencySelection.Size = new Size(300, 38);
            msbtnCurrencySelection.Text = "العملة";
            msbtnCurrencySelection.Click += msbtnCurrencySelection_Click;
            // 
            // msRevenue
            // 
            msRevenue.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnCashVouchers,
                new ToolStripSeparator(),
                msbtnCashBoxes, msbtnBanks, msbtnExpenses
            });
            msRevenue.Name = "msRevenue";
            msRevenue.Size = new Size(95, 33);
            msRevenue.Text = "الخزينة";
            // 
            // msbtnCashVouchers
            // 
            msbtnCashVouchers.Name = "msbtnCashVouchers";
            msbtnCashVouchers.Size = new Size(300, 38);
            msbtnCashVouchers.Text = "سندات القبض والصرف";
            msbtnCashVouchers.Click += msbtnCashVouchers_Click;
            // 
            // msbtnManagePayments
            // 
            msbtnManagePayments.Name = "msbtnManagePayments";
            msbtnManagePayments.Size = new Size(300, 38);
            msbtnManagePayments.Text = "المدفوعات";
            msbtnManagePayments.Click += msbtnManagePayments_Click;
            // 
            // msbtnCashBoxes
            // 
            msbtnCashBoxes.Name = "msbtnCashBoxes";
            msbtnCashBoxes.Size = new Size(300, 38);
            msbtnCashBoxes.Text = "صناديق النقدية";
            msbtnCashBoxes.Click += msbtnCashBoxes_Click;
            // 
            // msbtnBanks
            // 
            msbtnBanks.Name = "msbtnBanks";
            msbtnBanks.Size = new Size(300, 38);
            msbtnBanks.Text = "البنوك";
            msbtnBanks.Click += msbtnBanks_Click;
            // 
            // msbtnExpenses
            // 
            msbtnExpenses.Name = "msbtnExpenses";
            msbtnExpenses.Size = new Size(300, 38);
            msbtnExpenses.Text = "المصروفات";
            msbtnExpenses.Click += msbtnExpenses_Click;
            // 
            // msbtnCustomerReceipts
            // 
            msbtnCustomerReceipts.Name = "msbtnCustomerReceipts";
            msbtnCustomerReceipts.Size = new Size(300, 38);
            msbtnCustomerReceipts.Text = "سندات قبض العملاء";
            msbtnCustomerReceipts.Click += msbtnCustomerReceipts_Click;
            // 
            // msbtnPaymentVouchers
            // 
            msbtnPaymentVouchers.Name = "msbtnPaymentVouchers";
            msbtnPaymentVouchers.Size = new Size(300, 38);
            msbtnPaymentVouchers.Text = "سندات صرف الموردين";
            msbtnPaymentVouchers.Click += msbtnPaymentVouchers_Click;
            // 
            // msbtnSupplierPayments
            // 
            msbtnSupplierPayments.Name = "msbtnSupplierPayments";
            msbtnSupplierPayments.Size = new Size(300, 38);
            msbtnSupplierPayments.Text = "سداد الموردين";
            msbtnSupplierPayments.Click += msbtnSupplierPayments_Click;
            // 
            // msAccounting
            // 
            msAccounting.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnAccounting,
                new ToolStripSeparator(),
                msbtnChartOfAccounts, msbtnJournalEntries, msbtnAccountCategories,
                msbtnSystemAccountMappings, msbtnReceiptVouchers
            });
            msAccounting.Name = "msAccounting";
            msAccounting.Size = new Size(104, 33);
            msAccounting.Text = "حسابات";
            // 
            // msbtnAccounting
            // 
            msbtnAccounting.Name = "msbtnAccounting";
            msbtnAccounting.Size = new Size(300, 38);
            msbtnAccounting.Text = "الحسابات";
            msbtnAccounting.Click += msbtnAccounting_Click;
            // 
            // msbtnChartOfAccounts
            // 
            msbtnChartOfAccounts.Name = "msbtnChartOfAccounts";
            msbtnChartOfAccounts.Size = new Size(300, 38);
            msbtnChartOfAccounts.Text = "دليل الحسابات";
            msbtnChartOfAccounts.Click += msbtnChartOfAccounts_Click;
            // 
            // msbtnJournalEntries
            // 
            msbtnJournalEntries.Name = "msbtnJournalEntries";
            msbtnJournalEntries.Size = new Size(300, 38);
            msbtnJournalEntries.Text = "القيود اليومية";
            msbtnJournalEntries.Click += msbtnJournalEntries_Click;
            // 
            // msbtnAccountCategories
            // 
            msbtnAccountCategories.Name = "msbtnAccountCategories";
            msbtnAccountCategories.Size = new Size(300, 38);
            msbtnAccountCategories.Text = "تصنيفات الحسابات";
            msbtnAccountCategories.Click += msbtnAccountCategories_Click;
            // 
            // msbtnSystemAccountMappings
            // 
            msbtnSystemAccountMappings.Name = "msbtnSystemAccountMappings";
            msbtnSystemAccountMappings.Size = new Size(300, 38);
            msbtnSystemAccountMappings.Text = "ربط الحسابات النظامية";
            msbtnSystemAccountMappings.Click += msbtnSystemAccountMappings_Click;
            // 
            // msbtnReceiptVouchers
            // 
            msbtnReceiptVouchers.Name = "msbtnReceiptVouchers";
            msbtnReceiptVouchers.Size = new Size(300, 38);
            msbtnReceiptVouchers.Text = "سندات القبض";
            msbtnReceiptVouchers.Click += msbtnReceiptVouchers_Click;
            // 
            // msData
            // 
            msData.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnManageUsers, msbtnRoles, msbtnEmployees, msbtnPermissions, msbtnAuditLog,
                msbtnSystemLogs, msbtnUserSessions, msbtnDocumentSequences, msbtnTaxes
            });
            msData.Name = "msData";
            msData.Size = new Size(80, 33);
            msData.Text = "بيانات";
            // 
            // msbtnManageUsers
            // 
            msbtnManageUsers.Name = "msbtnManageUsers";
            msbtnManageUsers.Size = new Size(300, 38);
            msbtnManageUsers.Text = "المستخدمين";
            msbtnManageUsers.Click += msbtnManageUsers_Click;
            // 
            // msbtnRoles
            // 
            msbtnRoles.Name = "msbtnRoles";
            msbtnRoles.Size = new Size(300, 38);
            msbtnRoles.Text = "الأدوار";
            msbtnRoles.Click += msbtnRoles_Click;
            // 
            // msbtnEmployees
            // 
            msbtnEmployees.Name = "msbtnEmployees";
            msbtnEmployees.Size = new Size(300, 38);
            msbtnEmployees.Text = "الموظفون";
            msbtnEmployees.Click += msbtnEmployees_Click;
            // 
            // msbtnPermissions
            // 
            msbtnPermissions.Name = "msbtnPermissions";
            msbtnPermissions.Size = new Size(300, 38);
            msbtnPermissions.Text = "الصلاحيات";
            msbtnPermissions.Click += msbtnPermissions_Click;
            // 
            // msbtnAuditLog
            // 
            msbtnAuditLog.Name = "msbtnAuditLog";
            msbtnAuditLog.Size = new Size(300, 38);
            msbtnAuditLog.Text = "سجل التدقيق";
            msbtnAuditLog.Click += msbtnAuditLog_Click;
            // 
            // msbtnSystemLogs
            // 
            msbtnSystemLogs.Name = "msbtnSystemLogs";
            msbtnSystemLogs.Size = new Size(300, 38);
            msbtnSystemLogs.Text = "سجلات النظام";
            msbtnSystemLogs.Click += msbtnSystemLogs_Click;
            // 
            // msbtnUserSessions
            // 
            msbtnUserSessions.Name = "msbtnUserSessions";
            msbtnUserSessions.Size = new Size(300, 38);
            msbtnUserSessions.Text = "جلسات المستخدمين";
            msbtnUserSessions.Click += msbtnUserSessions_Click;
            // 
            // msbtnDocumentSequences
            // 
            msbtnDocumentSequences.Name = "msbtnDocumentSequences";
            msbtnDocumentSequences.Size = new Size(300, 38);
            msbtnDocumentSequences.Text = "تسلسل المستندات";
            msbtnDocumentSequences.Click += msbtnDocumentSequences_Click;
            // 
            // msbtnTaxes
            // 
            msbtnTaxes.Name = "msbtnTaxes";
            msbtnTaxes.Size = new Size(300, 38);
            msbtnTaxes.Text = "الضرائب";
            msbtnTaxes.Click += msbtnTaxes_Click;
            // 
            // msReport
            // 
            msReport.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnReportsDashboard,
                new ToolStripSeparator(),
                msbtnReports
            });
            msReport.Name = "msReport";
            msReport.Size = new Size(79, 33);
            msReport.Text = "تقارير";
            // 
            // msbtnReportsDashboard
            // 
            msbtnReportsDashboard.Name = "msbtnReportsDashboard";
            msbtnReportsDashboard.Size = new Size(300, 38);
            msbtnReportsDashboard.Text = "لوحة التقارير";
            msbtnReportsDashboard.Click += msbtnReportsDashboard_Click;
            // 
            // msbtnReports
            // 
            msbtnReports.Name = "msbtnReports";
            msbtnReports.Size = new Size(300, 38);
            msbtnReports.Text = "التقارير";
            msbtnReports.Click += msbtnReports_Click;
            // 
            // msSettings
            // 
            msSettings.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnSettings, msbtnCompanySettings, msbtnNotifications, msbtnUpdateCheck, msbtnBackup
            });
            msSettings.Name = "msSettings";
            msSettings.Size = new Size(120, 33);
            msSettings.Text = "الإعدادات";
            // 
            // msbtnSettings
            // 
            msbtnSettings.Name = "msbtnSettings";
            msbtnSettings.Size = new Size(300, 38);
            msbtnSettings.Text = "الإعدادات";
            msbtnSettings.Click += msbtnSettings_Click;
            // 
            // msbtnCompanySettings
            // 
            msbtnCompanySettings.Name = "msbtnCompanySettings";
            msbtnCompanySettings.Size = new Size(300, 38);
            msbtnCompanySettings.Text = "إعدادات الشركة";
            msbtnCompanySettings.Click += msbtnCompanySettings_Click;
            // 
            // msbtnNotifications
            // 
            msbtnNotifications.Name = "msbtnNotifications";
            msbtnNotifications.Size = new Size(300, 38);
            msbtnNotifications.Text = "التنبيهات";
            msbtnNotifications.Click += msbtnNotifications_Click;
            // 
            // msbtnUpdateCheck
            // 
            msbtnUpdateCheck.Name = "msbtnUpdateCheck";
            msbtnUpdateCheck.Size = new Size(300, 38);
            msbtnUpdateCheck.Text = "التحديث";
            msbtnUpdateCheck.Click += msbtnUpdateCheck_Click;
            // 
            // msbtnBackup
            // 
            msbtnBackup.Name = "msbtnBackup";
            msbtnBackup.Size = new Size(300, 38);
            msbtnBackup.Text = "النسخ الاحتياطي";
            msbtnBackup.Click += msbtnBackup_Click;
            // 
            // msAccoutSettings
            // 
            msAccoutSettings.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnLogout
            });
            msAccoutSettings.Name = "msAccoutSettings";
            msAccoutSettings.Size = new Size(176, 33);
            msAccoutSettings.Text = "إعدادات الحساب";
            // 
            // msbtnLogout
            // 
            msbtnLogout.Name = "msbtnLogout";
            msbtnLogout.Size = new Size(300, 38);
            msbtnLogout.Text = "تسجيل الخروج";
            msbtnLogout.Click += msbtnLogout_Click;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            errorProvider1.RightToLeft = true;
            // 
            // frmMain
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1378, 944);
            Controls.Add(msMainMenu);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = SystemColors.ControlText;
            IsMdiContainer = true;
            MainMenuStrip = msMainMenu;
            Margin = new Padding(4, 3, 4, 3);
            Name = "frmMain";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "نظام إدارة المبيعات";
            WindowState = FormWindowState.Maximized;
            Load += frmMain_Load;

            msMainMenu.ResumeLayout(false);
            msMainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip msMainMenu;
        private ToolStripMenuItem msFile;
        private ToolStripMenuItem msbtnChangePassword;
        private ToolStripMenuItem msbtnExit;
        private ToolStripMenuItem msbtnPOS;
        private ToolStripMenuItem msbtnProductSelection;
        private ToolStripMenuItem msbtnCustomerPicker;
        private ToolStripMenuItem msbtnDiscountExpenses;
        private ToolStripMenuItem msbtnCurrencySelection;
        private ToolStripMenuItem msbtnEmployees;
        private ToolStripMenuItem msBasis;
        private ToolStripMenuItem msbtnManageProducts;
        private ToolStripMenuItem msbtnAddProduct;
        private ToolStripMenuItem msbtnManageCustomers;
        private ToolStripMenuItem msbtnAddCustomer;
        private ToolStripMenuItem msbtnManageSuppliers;
        private ToolStripMenuItem msbtnAddSupplier;
        private ToolStripMenuItem msbtnManageCategories;
        private ToolStripMenuItem msbtnManageUnits;
        private ToolStripMenuItem msbtnProductPrices;
        private ToolStripMenuItem msbtnProductUnits;
        private ToolStripMenuItem msbtnProductCategories;
        private ToolStripMenuItem msStore;
        private ToolStripMenuItem msbtnManageWarehouses;
        private ToolStripMenuItem msbtnStockTransfer;
        private ToolStripMenuItem msbtnInventoryMovement;
        private ToolStripMenuItem msbtnInventoryAdjustments;
        private ToolStripMenuItem msbtnInventoryCounts;
        private ToolStripMenuItem msBuy;
        private ToolStripMenuItem msbtnManagePurchases;
        private ToolStripMenuItem msbtnPurchaseInvoice;
        private ToolStripMenuItem msbtnPurchaseReturn;
        private ToolStripMenuItem msSale;
        private ToolStripMenuItem msbtnManageSales;
        private ToolStripMenuItem msbtnSalesInvoice;
        private ToolStripMenuItem msbtnSalesReturn;
        private ToolStripMenuItem msRevenue;
        private ToolStripMenuItem msbtnManagePayments;
        private ToolStripMenuItem msbtnCashBoxes;
        private ToolStripMenuItem msbtnBanks;
        private ToolStripMenuItem msbtnExpenses;
        private ToolStripMenuItem msbtnCustomerReceipts;
        private ToolStripMenuItem msbtnPaymentVouchers;
        private ToolStripMenuItem msbtnSupplierPayments;
        private ToolStripMenuItem msAccounting;
        private ToolStripMenuItem msbtnChartOfAccounts;
        private ToolStripMenuItem msbtnJournalEntries;
        private ToolStripMenuItem msbtnAccountCategories;
        private ToolStripMenuItem msbtnSystemAccountMappings;
        private ToolStripMenuItem msbtnReceiptVouchers;
        private ToolStripMenuItem msData;
        private ToolStripMenuItem msbtnManageUsers;
        private ToolStripMenuItem msbtnRoles;
        private ToolStripMenuItem msbtnPermissions;
        private ToolStripMenuItem msbtnAuditLog;
        private ToolStripMenuItem msbtnSystemLogs;
        private ToolStripMenuItem msbtnUserSessions;
        private ToolStripMenuItem msbtnDocumentSequences;
        private ToolStripMenuItem msbtnTaxes;
        private ToolStripMenuItem msReport;
        private ToolStripMenuItem msbtnReports;
        private ToolStripMenuItem msbtnReportsDashboard;
        private ToolStripMenuItem msbtnCashVouchers;
        private ToolStripMenuItem msbtnPurchases;
        private ToolStripMenuItem msbtnInventory;
        private ToolStripMenuItem msbtnAccounting;
        private ToolStripMenuItem msSettings;
        private ToolStripMenuItem msbtnSettings;
        private ToolStripMenuItem msbtnCompanySettings;
        private ToolStripMenuItem msbtnNotifications;
        private ToolStripMenuItem msbtnUpdateCheck;
        private ToolStripMenuItem msbtnBackup;
        private ToolStripMenuItem msAccoutSettings;
        private ToolStripMenuItem msbtnLogout;
        private ErrorProvider errorProvider1;
    }
}
