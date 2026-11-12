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
            msbtnDashboard = new ToolStripMenuItem();
            // ── المنتجات ────────────────────────────────────────────────────────────────────────────────
            msBasis = new ToolStripMenuItem();
            msbtnManageProducts = new ToolStripMenuItem();
            msbtnManageCustomers = new ToolStripMenuItem();
            msbtnManageSuppliers = new ToolStripMenuItem();
            msbtnManageCategories = new ToolStripMenuItem();
            msbtnManageUnits = new ToolStripMenuItem();
            // ── المخزون ──────────────────────────────────────────────────────────────────────────────
            msStore = new ToolStripMenuItem();
            msbtnManageWarehouses = new ToolStripMenuItem();
            msbtnInventoryMovement = new ToolStripMenuItem();
            msbtnInventoryAdjustments = new ToolStripMenuItem();
            msbtnInventoryTransactions = new ToolStripMenuItem();
            msbtnInventoryBatches = new ToolStripMenuItem();
            msbtnOpeningStock = new ToolStripMenuItem();
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
            msbtnReturnsList = new ToolStripMenuItem();
            // ── الخزينة ──────────────────────────────────────────────────────────────────────────────
            msRevenue = new ToolStripMenuItem();
            msbtnManagePayments = new ToolStripMenuItem();
            msbtnCashBoxes = new ToolStripMenuItem();
            msbtnCashBoxTransactions = new ToolStripMenuItem();
            msbtnExpenses = new ToolStripMenuItem();
            msbtnCustomerReceipts = new ToolStripMenuItem();
            msbtnSupplierPayments = new ToolStripMenuItem();
            // ── بيانات ────────────────────────────────────────────────────────────────────────────────
            msData = new ToolStripMenuItem();
            msbtnManageUsers = new ToolStripMenuItem();
            // ── تقارير ────────────────────────────────────────────────────────────────────────────────
            // RULE-N70/N72: قائمة التقارير مقسّمة إلى 6 تصنيفات ثابتة، تقرير واحد = شاشة واحدة
            msReport = new ToolStripMenuItem();
            msReportSales = new ToolStripMenuItem();
            // ملاحظة: اسم صنف "المبيعات" = msReportSalesReport لأن msReportSales محجوز لعنصر التصنيف
            msReportSalesReport = new ToolStripMenuItem();
            msReportDailySales = new ToolStripMenuItem();
            msReportSalesByCustomer = new ToolStripMenuItem();
            msReportSalesByProduct = new ToolStripMenuItem();
            msReportSalesByCategory = new ToolStripMenuItem();
            msReportProfitByCustomer = new ToolStripMenuItem();
            msReportProductProfitability = new ToolStripMenuItem();
            msReportReturns = new ToolStripMenuItem();
            msReportPurchases = new ToolStripMenuItem();
            msReportPurchasesReport = new ToolStripMenuItem();
            msReportPurchasesBySupplier = new ToolStripMenuItem();
            msReportPurchasesByProduct = new ToolStripMenuItem();
            msReportInventory = new ToolStripMenuItem();
            msReportInventoryReport = new ToolStripMenuItem();
            msReportStockBalance = new ToolStripMenuItem();
            msReportWarehouseMovement = new ToolStripMenuItem();
            msReportDetailedStockLedger = new ToolStripMenuItem();
            msReportProductMovement = new ToolStripMenuItem();
            msReportLowStock = new ToolStripMenuItem();
            msReportExpiredProducts = new ToolStripMenuItem();
            msReportFinance = new ToolStripMenuItem();
            msReportTrialBalance = new ToolStripMenuItem();
            msReportGeneralLedger = new ToolStripMenuItem();
            msReportIncomeStatement = new ToolStripMenuItem();
            msReportBalanceSheet = new ToolStripMenuItem();
            msReportCashFlow = new ToolStripMenuItem();
            msReportWorkingCapital = new ToolStripMenuItem();
            msReportVat = new ToolStripMenuItem();
            msReportAccountStatement = new ToolStripMenuItem();
            msReportParties = new ToolStripMenuItem();
            msReportCustomerBalance = new ToolStripMenuItem();
            msReportSupplierBalance = new ToolStripMenuItem();
            msReportAging = new ToolStripMenuItem();
            msReportCashBox = new ToolStripMenuItem();
            msReportCashBoxSummary = new ToolStripMenuItem();
            msReportDailyClosure = new ToolStripMenuItem();
            // ── الإعدادات ────────────────────────────────────────────────────────────────────────────
            msSettings = new ToolStripMenuItem();
            msbtnSettings = new ToolStripMenuItem();
            msbtnCompanySettings = new ToolStripMenuItem();
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
            msbtnCurrencySelection = new ToolStripMenuItem();
            // ── الموظفون (جديد) ────────────────────────────────────────────────────────────────────
            msbtnEmployees = new ToolStripMenuItem();
            // ── شاشات الوحدات الرئيسية (جديد) ────────────────────────────────────────────────
            msbtnPurchases = new ToolStripMenuItem();
            msbtnInventory = new ToolStripMenuItem();
            // ── المحاسبة (جديد) ───────────────────────────────────────────────────────────────────
            msAccounting = new ToolStripMenuItem();
            msbtnChartOfAccounts = new ToolStripMenuItem();
            msbtnJournalEntries = new ToolStripMenuItem();

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
                msbtnDashboard, msFile, msSale, msBuy, msBasis, msStore, msRevenue,
                msAccounting, msData, msReport, msSettings, msAccoutSettings
            });
            msMainMenu.Location = new Point(0, 0);
            msMainMenu.Name = "msMainMenu";
            msMainMenu.Size = new Size(1378, 37);
            msMainMenu.TabIndex = 0;
            msMainMenu.Text = "Main Menu";
            // 
            // msbtnDashboard
            // 
            msbtnDashboard.Name = "msbtnDashboard";
            msbtnDashboard.Size = new Size(140, 33);
            msbtnDashboard.Text = "لوحة التحكم";
            msbtnDashboard.Click += msbtnDashboard_Click;
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
                msbtnManageProducts
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
            // msbtnManageCustomers
            // 
            msbtnManageCustomers.Name = "msbtnManageCustomers";
            msbtnManageCustomers.Size = new Size(300, 38);
            msbtnManageCustomers.Text = "إدارة العملاء";
            msbtnManageCustomers.Click += msbtnManageCustomers_Click;
            // 
            // msbtnManageSuppliers
            // 
            msbtnManageSuppliers.Name = "msbtnManageSuppliers";
            msbtnManageSuppliers.Size = new Size(300, 38);
            msbtnManageSuppliers.Text = "إدارة الموردين";
            msbtnManageSuppliers.Click += msbtnManageSuppliers_Click;
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
            // msStore
            // 
            msStore.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnInventory,
                new ToolStripSeparator(),
                msbtnManageWarehouses, msbtnInventoryMovement,
                msbtnInventoryTransactions, msbtnInventoryBatches,
                msbtnInventoryAdjustments, msbtnOpeningStock
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
            // msbtnInventoryTransactions
            // 
            msbtnInventoryTransactions.Name = "msbtnInventoryTransactions";
            msbtnInventoryTransactions.Size = new Size(300, 38);
            msbtnInventoryTransactions.Text = "حركة المخزون التفصيلية";
            msbtnInventoryTransactions.Click += msbtnInventoryTransactions_Click;
            // 
            // msbtnInventoryBatches
            // 
            msbtnInventoryBatches.Name = "msbtnInventoryBatches";
            msbtnInventoryBatches.Size = new Size(300, 38);
            msbtnInventoryBatches.Text = "دفعات المخزون (الصلاحية)";
            msbtnInventoryBatches.Click += msbtnInventoryBatches_Click;
            // 
            // msbtnOpeningStock
            // 
            msbtnOpeningStock.Name = "msbtnOpeningStock";
            msbtnOpeningStock.Size = new Size(300, 38);
            msbtnOpeningStock.Text = "بضاعة أول المدة";
            msbtnOpeningStock.Click += msbtnOpeningStock_Click;
            // 
            // msBuy
            // 
            msBuy.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnPurchases,
                new ToolStripSeparator(),
                msbtnPurchaseInvoice, msbtnPurchaseReturn, msbtnManagePurchases,
                new ToolStripSeparator(),
                msbtnManageSuppliers,
                new ToolStripSeparator(),
                msbtnSupplierPayments
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
                msbtnSalesInvoice, msbtnManageSales, msbtnSalesReturn, msbtnReturnsList,
                new ToolStripSeparator(),
                msbtnCurrencySelection,
                new ToolStripSeparator(),
                msbtnManageCustomers,
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
            // msbtnReturnsList
            // 
            msbtnReturnsList.Name = "msbtnReturnsList";
            msbtnReturnsList.Size = new Size(300, 38);
            msbtnReturnsList.Text = "قائمة المرتجعات";
            msbtnReturnsList.Click += msbtnReturnsList_Click;
            // 
            // msbtnPOS
            // 
            msbtnPOS.Name = "msbtnPOS";
            msbtnPOS.Size = new Size(300, 38);
            msbtnPOS.Text = "نقطة البيع";
            msbtnPOS.Click += msbtnPOS_Click;
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
                msbtnCashBoxes, msbtnCashBoxTransactions, msbtnExpenses
            });
            msRevenue.Name = "msRevenue";
            msRevenue.Size = new Size(95, 33);
            msRevenue.Text = "الخزينة";
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
            // msbtnCashBoxTransactions
            // 
            msbtnCashBoxTransactions.Name = "msbtnCashBoxTransactions";
            msbtnCashBoxTransactions.Size = new Size(300, 38);
            msbtnCashBoxTransactions.Text = "حركات الصندوق";
            msbtnCashBoxTransactions.Click += msbtnCashBoxTransactions_Click;
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
            // msbtnSupplierPayments
            // 
            msbtnSupplierPayments.Name = "msbtnSupplierPayments";
            msbtnSupplierPayments.Size = new Size(300, 38);
            msbtnSupplierPayments.Text = "سداد الموردين";
            msbtnSupplierPayments.Click += msbtnSupplierPayments_Click;
            // 
            // msData
            // 
            msData.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnManageUsers, msbtnEmployees
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
            // msbtnEmployees
            // 
            msbtnEmployees.Name = "msbtnEmployees";
            msbtnEmployees.Size = new Size(300, 38);
            msbtnEmployees.Text = "الموظفون";
            msbtnEmployees.Click += msbtnEmployees_Click;
            // 
            // msAccounting
            // 
            msAccounting.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnChartOfAccounts,
                msbtnJournalEntries
            });
            msAccounting.Name = "msAccounting";
            msAccounting.Size = new Size(105, 33);
            msAccounting.Text = "المحاسبة";
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
            msbtnJournalEntries.Text = "القيود المحاسبية";
            msbtnJournalEntries.Click += msbtnJournalEntries_Click;
            // 
            // msReport
            // 
            msReport.DropDownItems.AddRange(new ToolStripItem[] {
                msReportSales,
                msReportPurchases,
                msReportInventory,
                msReportFinance,
                msReportParties,
                msReportCashBox
            });
            msReport.Name = "msReport";
            msReport.Size = new Size(79, 33);
            msReport.Text = "تقارير";
            // 
            // msReportSales
            // 
            msReportSales.DropDownItems.AddRange(new ToolStripItem[] {
                msReportSalesReport,
                msReportDailySales,
                msReportSalesByCustomer,
                msReportSalesByProduct,
                msReportSalesByCategory,
                msReportProfitByCustomer,
                msReportProductProfitability,
                msReportReturns
            });
            msReportSales.Name = "msReportSales";
            msReportSales.Size = new Size(300, 38);
            msReportSales.Text = "تقارير المبيعات";
            // 
            // msReportSalesReport
            // 
            msReportSalesReport.Name = "msReportSalesReport";
            msReportSalesReport.Size = new Size(280, 34);
            msReportSalesReport.Text = "المبيعات";
            msReportSalesReport.Click += msReportSalesReport_Click;
            // 
            // msReportDailySales
            // 
            msReportDailySales.Name = "msReportDailySales";
            msReportDailySales.Size = new Size(280, 34);
            msReportDailySales.Text = "المبيعات اليومية";
            msReportDailySales.Click += msReportDailySales_Click;
            // 
            // msReportSalesByCustomer
            // 
            msReportSalesByCustomer.Name = "msReportSalesByCustomer";
            msReportSalesByCustomer.Size = new Size(280, 34);
            msReportSalesByCustomer.Text = "المبيعات حسب العميل";
            msReportSalesByCustomer.Click += msReportSalesByCustomer_Click;
            // 
            // msReportSalesByProduct
            // 
            msReportSalesByProduct.Name = "msReportSalesByProduct";
            msReportSalesByProduct.Size = new Size(280, 34);
            msReportSalesByProduct.Text = "المبيعات حسب الصنف";
            msReportSalesByProduct.Click += msReportSalesByProduct_Click;
            // 
            // msReportSalesByCategory
            // 
            msReportSalesByCategory.Name = "msReportSalesByCategory";
            msReportSalesByCategory.Size = new Size(280, 34);
            msReportSalesByCategory.Text = "المبيعات حسب التصنيف";
            msReportSalesByCategory.Click += msReportSalesByCategory_Click;
            // 
            // msReportProfitByCustomer
            // 
            msReportProfitByCustomer.Name = "msReportProfitByCustomer";
            msReportProfitByCustomer.Size = new Size(280, 34);
            msReportProfitByCustomer.Text = "الربحية حسب العميل";
            msReportProfitByCustomer.Click += msReportProfitByCustomer_Click;
            // 
            // msReportProductProfitability
            // 
            msReportProductProfitability.Name = "msReportProductProfitability";
            msReportProductProfitability.Size = new Size(280, 34);
            msReportProductProfitability.Text = "ربحية الأصناف";
            msReportProductProfitability.Click += msReportProductProfitability_Click;
            // 
            // msReportReturns
            // 
            msReportReturns.Name = "msReportReturns";
            msReportReturns.Size = new Size(280, 34);
            msReportReturns.Text = "المرتجعات";
            msReportReturns.Click += msReportReturns_Click;
            // 
            // msReportPurchases
            // 
            msReportPurchases.DropDownItems.AddRange(new ToolStripItem[] {
                msReportPurchasesReport,
                msReportPurchasesBySupplier,
                msReportPurchasesByProduct
            });
            msReportPurchases.Name = "msReportPurchases";
            msReportPurchases.Size = new Size(300, 38);
            msReportPurchases.Text = "تقارير المشتريات";
            // 
            // msReportPurchasesReport
            // 
            msReportPurchasesReport.Name = "msReportPurchasesReport";
            msReportPurchasesReport.Size = new Size(280, 34);
            msReportPurchasesReport.Text = "المشتريات";
            msReportPurchasesReport.Click += msReportPurchasesReport_Click;
            // 
            // msReportPurchasesBySupplier
            // 
            msReportPurchasesBySupplier.Name = "msReportPurchasesBySupplier";
            msReportPurchasesBySupplier.Size = new Size(280, 34);
            msReportPurchasesBySupplier.Text = "المشتريات حسب المورد";
            msReportPurchasesBySupplier.Click += msReportPurchasesBySupplier_Click;
            // 
            // msReportPurchasesByProduct
            // 
            msReportPurchasesByProduct.Name = "msReportPurchasesByProduct";
            msReportPurchasesByProduct.Size = new Size(280, 34);
            msReportPurchasesByProduct.Text = "المشتريات حسب الصنف";
            msReportPurchasesByProduct.Click += msReportPurchasesByProduct_Click;
            // 
            // msReportInventory
            // 
            msReportInventory.DropDownItems.AddRange(new ToolStripItem[] {
                msReportInventoryReport,
                msReportStockBalance,
                msReportWarehouseMovement,
                msReportDetailedStockLedger,
                msReportProductMovement,
                msReportLowStock,
                msReportExpiredProducts
            });
            msReportInventory.Name = "msReportInventory";
            msReportInventory.Size = new Size(300, 38);
            msReportInventory.Text = "تقارير المخزون";
            // 
            // msReportInventoryReport
            // 
            msReportInventoryReport.Name = "msReportInventoryReport";
            msReportInventoryReport.Size = new Size(280, 34);
            msReportInventoryReport.Text = "المخزون";
            msReportInventoryReport.Click += msReportInventoryReport_Click;
            // 
            // msReportStockBalance
            // 
            msReportStockBalance.Name = "msReportStockBalance";
            msReportStockBalance.Size = new Size(280, 34);
            msReportStockBalance.Text = "أرصدة المخزون";
            msReportStockBalance.Click += msReportStockBalance_Click;
            // 
            // msReportWarehouseMovement
            // 
            msReportWarehouseMovement.Name = "msReportWarehouseMovement";
            msReportWarehouseMovement.Size = new Size(280, 34);
            msReportWarehouseMovement.Text = "حركة المستودعات";
            msReportWarehouseMovement.Click += msReportWarehouseMovement_Click;
            // 
            // msReportDetailedStockLedger
            // 
            msReportDetailedStockLedger.Name = "msReportDetailedStockLedger";
            msReportDetailedStockLedger.Size = new Size(280, 34);
            msReportDetailedStockLedger.Text = "دفتر حركة المخزون التفصيلي";
            msReportDetailedStockLedger.Click += msReportDetailedStockLedger_Click;
            // 
            // msReportProductMovement
            // 
            msReportProductMovement.Name = "msReportProductMovement";
            msReportProductMovement.Size = new Size(280, 34);
            msReportProductMovement.Text = "حركة صنف";
            msReportProductMovement.Click += msReportProductMovement_Click;
            // 
            // msReportLowStock
            // 
            msReportLowStock.Name = "msReportLowStock";
            msReportLowStock.Size = new Size(280, 34);
            msReportLowStock.Text = "النواقص";
            msReportLowStock.Click += msReportLowStock_Click;
            // 
            // msReportExpiredProducts
            // 
            msReportExpiredProducts.Name = "msReportExpiredProducts";
            msReportExpiredProducts.Size = new Size(280, 34);
            msReportExpiredProducts.Text = "منتهي الصلاحية";
            msReportExpiredProducts.Click += msReportExpiredProducts_Click;
            // 
            // msReportFinance
            // 
            msReportFinance.DropDownItems.AddRange(new ToolStripItem[] {
                msReportTrialBalance,
                msReportGeneralLedger,
                msReportIncomeStatement,
                msReportBalanceSheet,
                msReportCashFlow,
                msReportWorkingCapital,
                msReportVat,
                msReportAccountStatement
            });
            msReportFinance.Name = "msReportFinance";
            msReportFinance.Size = new Size(300, 38);
            msReportFinance.Text = "التقارير المالية والمحاسبية";
            // 
            // msReportTrialBalance
            // 
            msReportTrialBalance.Name = "msReportTrialBalance";
            msReportTrialBalance.Size = new Size(280, 34);
            msReportTrialBalance.Text = "ميزان المراجعة";
            msReportTrialBalance.Click += msReportTrialBalance_Click;
            // 
            // msReportGeneralLedger
            // 
            msReportGeneralLedger.Name = "msReportGeneralLedger";
            msReportGeneralLedger.Size = new Size(280, 34);
            msReportGeneralLedger.Text = "دفتر الأستاذ";
            msReportGeneralLedger.Click += msReportGeneralLedger_Click;
            // 
            // msReportIncomeStatement
            // 
            msReportIncomeStatement.Name = "msReportIncomeStatement";
            msReportIncomeStatement.Size = new Size(280, 34);
            msReportIncomeStatement.Text = "قائمة الدخل";
            msReportIncomeStatement.Click += msReportIncomeStatement_Click;
            // 
            // msReportBalanceSheet
            // 
            msReportBalanceSheet.Name = "msReportBalanceSheet";
            msReportBalanceSheet.Size = new Size(280, 34);
            msReportBalanceSheet.Text = "الميزانية العمومية";
            msReportBalanceSheet.Click += msReportBalanceSheet_Click;
            // 
            // msReportCashFlow
            // 
            msReportCashFlow.Name = "msReportCashFlow";
            msReportCashFlow.Size = new Size(280, 34);
            msReportCashFlow.Text = "التدفقات النقدية";
            msReportCashFlow.Click += msReportCashFlow_Click;
            // 
            // msReportWorkingCapital
            // 
            msReportWorkingCapital.Name = "msReportWorkingCapital";
            msReportWorkingCapital.Size = new Size(280, 34);
            msReportWorkingCapital.Text = "رأس المال العامل";
            msReportWorkingCapital.Click += msReportWorkingCapital_Click;
            // 
            // msReportVat
            // 
            msReportVat.Name = "msReportVat";
            msReportVat.Size = new Size(280, 34);
            msReportVat.Text = "ضريبة القيمة المضافة";
            msReportVat.Click += msReportVat_Click;
            // 
            // msReportAccountStatement
            // 
            msReportAccountStatement.Name = "msReportAccountStatement";
            msReportAccountStatement.Size = new Size(280, 34);
            msReportAccountStatement.Text = "كشف حساب";
            msReportAccountStatement.Click += msReportAccountStatement_Click;
            // 
            // msReportParties
            // 
            msReportParties.DropDownItems.AddRange(new ToolStripItem[] {
                msReportCustomerBalance,
                msReportSupplierBalance,
                msReportAging
            });
            msReportParties.Name = "msReportParties";
            msReportParties.Size = new Size(300, 38);
            msReportParties.Text = "تقارير العملاء والموردين";
            // 
            // msReportCustomerBalance
            // 
            msReportCustomerBalance.Name = "msReportCustomerBalance";
            msReportCustomerBalance.Size = new Size(280, 34);
            msReportCustomerBalance.Text = "أرصدة العملاء";
            msReportCustomerBalance.Click += msReportCustomerBalance_Click;
            // 
            // msReportSupplierBalance
            // 
            msReportSupplierBalance.Name = "msReportSupplierBalance";
            msReportSupplierBalance.Size = new Size(280, 34);
            msReportSupplierBalance.Text = "أرصدة الموردين";
            msReportSupplierBalance.Click += msReportSupplierBalance_Click;
            // 
            // msReportAging
            // 
            msReportAging.Name = "msReportAging";
            msReportAging.Size = new Size(280, 34);
            msReportAging.Text = "أعمار الديون";
            msReportAging.Click += msReportAging_Click;
            // 
            // msReportCashBox
            // 
            msReportCashBox.DropDownItems.AddRange(new ToolStripItem[] {
                msReportCashBoxSummary,
                msReportDailyClosure
            });
            msReportCashBox.Name = "msReportCashBox";
            msReportCashBox.Size = new Size(300, 38);
            msReportCashBox.Text = "تقارير الصندوق";
            // 
            // msReportCashBoxSummary
            // 
            msReportCashBoxSummary.Name = "msReportCashBoxSummary";
            msReportCashBoxSummary.Size = new Size(280, 34);
            msReportCashBoxSummary.Text = "ملخص الصندوق";
            msReportCashBoxSummary.Click += msReportCashBoxSummary_Click;
            // 
            // msReportDailyClosure
            // 
            msReportDailyClosure.Name = "msReportDailyClosure";
            msReportDailyClosure.Size = new Size(280, 34);
            msReportDailyClosure.Text = "الإغلاق اليومي";
            msReportDailyClosure.Click += msReportDailyClosure_Click;
            // 
            // msSettings
            // 
            msSettings.DropDownItems.AddRange(new ToolStripItem[] {
                msbtnSettings, msbtnCompanySettings, msbtnUpdateCheck, msbtnBackup
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
        private ToolStripMenuItem msbtnDashboard;
        private ToolStripMenuItem msFile;
        private ToolStripMenuItem msbtnChangePassword;
        private ToolStripMenuItem msbtnExit;
        private ToolStripMenuItem msbtnPOS;
        private ToolStripMenuItem msbtnCurrencySelection;
        private ToolStripMenuItem msbtnEmployees;
        private ToolStripMenuItem msBasis;
        private ToolStripMenuItem msbtnManageProducts;
        private ToolStripMenuItem msbtnManageCustomers;
        private ToolStripMenuItem msbtnManageSuppliers;
        private ToolStripMenuItem msbtnManageCategories;
        private ToolStripMenuItem msbtnManageUnits;
        private ToolStripMenuItem msStore;
        private ToolStripMenuItem msbtnManageWarehouses;
        private ToolStripMenuItem msbtnInventoryMovement;
        private ToolStripMenuItem msbtnInventoryAdjustments;
        private ToolStripMenuItem msbtnInventoryTransactions;
        private ToolStripMenuItem msbtnInventoryBatches;
        private ToolStripMenuItem msbtnOpeningStock;
        private ToolStripMenuItem msBuy;
        private ToolStripMenuItem msbtnManagePurchases;
        private ToolStripMenuItem msbtnPurchaseInvoice;
        private ToolStripMenuItem msbtnPurchaseReturn;
        private ToolStripMenuItem msSale;
        private ToolStripMenuItem msbtnManageSales;
        private ToolStripMenuItem msbtnSalesInvoice;
        private ToolStripMenuItem msbtnSalesReturn;
        private ToolStripMenuItem msbtnReturnsList;
        private ToolStripMenuItem msRevenue;
        private ToolStripMenuItem msbtnManagePayments;
        private ToolStripMenuItem msbtnCashBoxes;
        private ToolStripMenuItem msbtnCashBoxTransactions;
        private ToolStripMenuItem msbtnExpenses;
        private ToolStripMenuItem msbtnCustomerReceipts;
        private ToolStripMenuItem msbtnSupplierPayments;
        private ToolStripMenuItem msData;
        private ToolStripMenuItem msbtnManageUsers;
        private ToolStripMenuItem msReport;
        // تصنيفات التقارير الستة (RULE-N72)
        private ToolStripMenuItem msReportSales;
        private ToolStripMenuItem msReportPurchases;
        private ToolStripMenuItem msReportInventory;
        private ToolStripMenuItem msReportFinance;
        private ToolStripMenuItem msReportParties;
        private ToolStripMenuItem msReportCashBox;
        // تقارير المبيعات (8)
        private ToolStripMenuItem msReportSalesReport;
        private ToolStripMenuItem msReportDailySales;
        private ToolStripMenuItem msReportSalesByCustomer;
        private ToolStripMenuItem msReportSalesByProduct;
        private ToolStripMenuItem msReportSalesByCategory;
        private ToolStripMenuItem msReportProfitByCustomer;
        private ToolStripMenuItem msReportProductProfitability;
        private ToolStripMenuItem msReportReturns;
        // تقارير المشتريات (3)
        private ToolStripMenuItem msReportPurchasesReport;
        private ToolStripMenuItem msReportPurchasesBySupplier;
        private ToolStripMenuItem msReportPurchasesByProduct;
        // تقارير المخزون (7)
        private ToolStripMenuItem msReportInventoryReport;
        private ToolStripMenuItem msReportStockBalance;
        private ToolStripMenuItem msReportWarehouseMovement;
        private ToolStripMenuItem msReportDetailedStockLedger;
        private ToolStripMenuItem msReportProductMovement;
        private ToolStripMenuItem msReportLowStock;
        private ToolStripMenuItem msReportExpiredProducts;
        // التقارير المالية والمحاسبية (8)
        private ToolStripMenuItem msReportTrialBalance;
        private ToolStripMenuItem msReportGeneralLedger;
        private ToolStripMenuItem msReportIncomeStatement;
        private ToolStripMenuItem msReportBalanceSheet;
        private ToolStripMenuItem msReportCashFlow;
        private ToolStripMenuItem msReportWorkingCapital;
        private ToolStripMenuItem msReportVat;
        private ToolStripMenuItem msReportAccountStatement;
        // تقارير العملاء والموردين (3)
        private ToolStripMenuItem msReportCustomerBalance;
        private ToolStripMenuItem msReportSupplierBalance;
        private ToolStripMenuItem msReportAging;
        // تقارير الصندوق (2)
        private ToolStripMenuItem msReportCashBoxSummary;
        private ToolStripMenuItem msReportDailyClosure;
        private ToolStripMenuItem msAccounting;
        private ToolStripMenuItem msbtnChartOfAccounts;
        private ToolStripMenuItem msbtnJournalEntries;
        private ToolStripMenuItem msbtnPurchases;
        private ToolStripMenuItem msbtnInventory;
        private ToolStripMenuItem msSettings;
        private ToolStripMenuItem msbtnSettings;
        private ToolStripMenuItem msbtnCompanySettings;
        private ToolStripMenuItem msbtnUpdateCheck;
        private ToolStripMenuItem msbtnBackup;
        private ToolStripMenuItem msAccoutSettings;
        private ToolStripMenuItem msbtnLogout;
        private ErrorProvider errorProvider1;
    }
}
