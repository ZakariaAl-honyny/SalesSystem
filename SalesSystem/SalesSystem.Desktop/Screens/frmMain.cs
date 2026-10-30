using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Products.Forms;
using SalesSystem.Desktop.Controls.Users;
using SalesSystem.Desktop.CustomerReceipts.Forms;
using SalesSystem.Common.Enums.Permissions;

using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Screens.Taxes.Forms;
using SalesSystem.Desktop.Screens.Roles.Forms;
using SalesSystem.Desktop.Screens.Users.Forms;
using SalesSystem.Desktop.Screens.StockTransfers.Forms;
using SalesSystem.Desktop.Screens.Units.Forms;
using SalesSystem.Desktop.Screens.Sales.Forms;
using SalesSystem.Desktop.Screens.POS.Forms;
using SalesSystem.Desktop.Screens.Employees.Forms;
using SalesSystem.Desktop.Screens.Products.Forms;
using SalesSystem.Desktop.Screens.Purchases.Forms;
using SalesSystem.Desktop.Screens.Returns.Forms;
using SalesSystem.Desktop.Screens.PaymentVouchers.Forms;
using SalesSystem.Desktop.Screens.Customers.Forms;
using SalesSystem.Desktop.Screens.Categories.Forms;
using SalesSystem.Desktop.Screens.Payments.Forms;
using SalesSystem.Desktop.Screens.Expenses.Forms;
using SalesSystem.Desktop.Screens.CashBoxes.Forms;
using SalesSystem.Desktop.Screens.CashVouchers.Forms;
using SalesSystem.Desktop.Screens.Banks.Forms;
using SalesSystem.Desktop.Screens.Accounting.Forms;
using SalesSystem.Desktop.Screens.Inventory.Forms;
using SalesSystem.Desktop.Screens.Warehouses.Forms;
using SalesSystem.Desktop.Suppliers.Forms;
using SalesSystem.Desktop.Screens.Settings.Forms;
using SalesSystem.Desktop.Screens.Reports.Forms;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Configuration;
 
namespace SalesSystem.Desktop.Screens;

public partial class frmMain : Form
{
    private readonly ISessionService _sessionService;
    private readonly IServiceProvider _serviceProvider;

    public frmMain(ISessionService sessionService, IServiceProvider serviceProvider)
    {
        _sessionService = sessionService;
        _serviceProvider = serviceProvider;
        InitializeComponent();

        KeyboardHelper.WireEnterAsTab(this);

        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;

        // MVP: إخفاء شاشات النسخة الثانية (V2) — الكود سليم ومحفوظ لإعادة التفعيل لاحقاً
        ApplyMvpMenuVisibility();
    }

    /// <summary>
    /// MVP (محل واحد / مستخدم واحد / فرع واحد): إخفاء عناصر القوائم المؤجلة إلى V2.
    /// لا يتم حذف أي عنصر — الإخفاء فقط، مع إبقاء المعالجات وكود الشاشات كما هو.
    /// </summary>
    private void ApplyMvpMenuVisibility()
    {
        // ── المنتجات ─────────────────────────────────────────────────────────
        msbtnProductPrices.Visible = false;         // MVP: مخفي (V2)
        msbtnProductUnits.Visible = false;          // MVP: مخفي (V2)
        msbtnProductCategories.Visible = false;     // MVP: مخفي (V2)

        // ── إضافة سريعة (أصبحت داخل شاشات الـ Master) ───────────────────────
        msbtnAddProduct.Visible = false;            // MVP: مخفي — add handled inside Master form
        msbtnAddCustomer.Visible = false;           // MVP: مخفي — add handled inside Master form
        msbtnAddSupplier.Visible = false;           // MVP: مخفي — add handled inside Master form

        // ── المخزون ─────────────────────────────────────────────────────────
        msbtnStockTransfer.Visible = false;         // MVP: مخفي (V2)
        msbtnInventoryCounts.Visible = false;       // MVP: مخفي (V2)

        // ── المبيعات ────────────────────────────────────────────────────────
        msbtnCurrencySelection.Visible = false;     // MVP: مخفي (V2)
        msbtnSalesProfitBreakdown.Visible = false;  // MVP: مخفي (V2)
        msbtnSalesImportConvert.Visible = false;    // MVP: مخفي (V2)
        msbtnSalesSmartF3Lookup.Visible = false;    // MVP: مخفي (V2)
        msbtnSalesInputStrategies.Visible = false;  // MVP: مخفي (V2)
        msbtnSalesContinuousScanner.Visible = false;// MVP: مخفي (V2)
        msbtnSalesTouchGrid.Visible = false;        // MVP: مخفي (V2)
        msbtnSalesRapidBar.Visible = false;         // MVP: مخفي (V2)
        msbtnSalesInlineGrid.Visible = false;       // MVP: مخفي (V2)

        // ── الخزينة ─────────────────────────────────────────────────────────
        msbtnCashVouchers.Visible = false;          // MVP: مخفي (V2)
        msbtnBanks.Visible = false;                 // MVP: مخفي (V2)
        msbtnPaymentVouchers.Visible = false;       // MVP: مخفي (V2)

        // ── حسابات (القائمة بالكامل — المحاسبة آلية في MVP) ──────────────────
        msAccounting.Visible = false;               // MVP: مخفي (V2)
        msbtnAccounting.Visible = false;            // MVP: مخفي (V2)
        msbtnChartOfAccounts.Visible = false;       // MVP: مخفي (V2)
        msbtnJournalEntries.Visible = false;        // MVP: مخفي (V2)
        msbtnAccountCategories.Visible = false;     // MVP: مخفي (V2)
        msbtnSystemAccountMappings.Visible = false; // MVP: مخفي (V2)
        msbtnReceiptVouchers.Visible = false;       // MVP: مخفي (V2)

        // ── بيانات ──────────────────────────────────────────────────────────
        msbtnManageUsers.Visible = false;           // MVP: مخفي (V2)
        msbtnRoles.Visible = false;                 // MVP: مخفي (V2)
        msbtnEmployees.Visible = false;             // MVP: مخفي (V2)
        msbtnPermissions.Visible = false;           // MVP: مخفي (V2)
        msbtnAuditLog.Visible = false;              // MVP: مخفي (V2)
        msbtnSystemLogs.Visible = false;            // MVP: مخفي (V2)
        msbtnUserSessions.Visible = false;          // MVP: مخفي (V2)
        msbtnDocumentSequences.Visible = false;     // MVP: مخفي (V2)
        msbtnTaxes.Visible = false;                 // MVP: مخفي (V2) — الضرائب

        // ── الإعدادات ───────────────────────────────────────────────────────
        msbtnNotifications.Visible = false;         // MVP: مخفي (V2)
        msbtnUpdateCheck.Visible = false;           // MVP: مخفي (V2)
        // msbtnSettings / msbtnCompanySettings / msbtnBackup تبقى ظاهرة
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
        if (!_sessionService.IsAuthenticated)
        {
            Application.Exit();
            return;
        }

        // Store reference for clsGlobal._OpenForm()
        clsGlobal.frmMdiMain = this;

        // Set MdiClient background to WhiteSmoke
        var MdiContainerPage = this.Controls.OfType<MdiClient>().FirstOrDefault();
        if (MdiContainerPage != null)
            MdiContainerPage.BackColor = Color.WhiteSmoke;

        // ── التحقق من صلاحيات المستخدم وإخفاء الأزرار غير المصرح بها ──────
        try
        {
            // النظام (SystemPermissions)
            msbtnManageUsers.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageUsers);
            msbtnRoles.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageRoles);
            msbtnEmployees.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageUsers);
            msbtnPermissions.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageRoles);
            msbtnAuditLog.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ViewAuditLogs);
            msbtnSystemLogs.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ViewAuditLogs);
            msbtnUserSessions.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageUsers);
            msbtnDocumentSequences.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageSettings);
            msbtnSettings.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageSettings);
            msbtnCompanySettings.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageSettings);
            msbtnNotifications.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageSettings);
            msbtnBackup.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageBackups);
            msbtnTaxes.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageTaxes);
            msbtnUpdateCheck.Visible = SessionManager.HasSystemPermission((long)SystemPermissions.ManageSettings);

            // المبيعات (SalesPermissions)
            msbtnManageSales.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.ViewInvoices);
            msbtnSalesInvoice.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnSalesReturn.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateReturns);
            msbtnCustomerReceipts.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.ViewReceipts);
            msbtnManagePayments.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.ViewReceipts);
            // المبيعات — نقطة البيع وأدوات الفاتورة
            msbtnPOS.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnProductSelection.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnCustomerPicker.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnDiscountExpenses.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.ManageDiscounts);
            msbtnCurrencySelection.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnSalesProfitBreakdown.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnPOSHeldOrders.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnSalesImportConvert.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnReceiptPrint.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnSalesSmartF3Lookup.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnSalesInputStrategies.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnSalesContinuousScanner.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnSalesTouchGrid.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnSalesRapidBar.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
            msbtnSalesInlineGrid.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);

            // العملاء (مرتبط بالمبيعات)
            msbtnManageCustomers.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.ViewInvoices);
            msbtnAddCustomer.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);

            // المخزون (InventoryPermissions)
            msbtnManageProducts.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ViewProducts);
            msbtnAddProduct.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.CreateProducts);
            msbtnProductPrices.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ViewProducts);
            msbtnProductUnits.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ManageUnits);
            msbtnProductCategories.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ManageCategories);
            msbtnManageCategories.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ManageCategories);
            msbtnManageUnits.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ManageUnits);
            msbtnManageWarehouses.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ViewStock);
            msbtnStockTransfer.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.TransferStock);
            msbtnInventoryMovement.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ViewStock);
            msbtnInventory.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ViewStock);
            msbtnInventoryAdjustments.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.AdjustStock);
            msbtnInventoryCounts.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.CountStock);

            // المشتريات (مرتبط بالمخزون)
            msbtnManagePurchases.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ViewProducts);
            msbtnPurchases.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ViewProducts);
            msbtnPurchaseInvoice.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.CreateProducts);
            msbtnPurchaseReturn.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.AdjustStock);
            msbtnSupplierPayments.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ViewProducts);

            // الموردين (مرتبط بالمشتريات)
            msbtnManageSuppliers.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.ViewProducts);
            msbtnAddSupplier.Visible = SessionManager.HasInventoryPermission((long)InventoryPermissions.CreateProducts);

            // المحاسبة (AccountingPermissions)
            msbtnChartOfAccounts.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ViewAccounts);
            msbtnAccounting.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ViewAccounts);
            msbtnJournalEntries.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ViewJournalEntries);
            msbtnAccountCategories.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ManageAccounts);
            msbtnSystemAccountMappings.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ManageAccounts);
            msbtnReceiptVouchers.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ViewReceiptVouchers);
            msbtnPaymentVouchers.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ViewPaymentVouchers);
            msbtnCashBoxes.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ViewCashBoxes);
            msbtnCashVouchers.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ViewCashBoxes);
            msbtnBanks.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ViewCashBoxes);
            msbtnExpenses.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ViewExpenses);

            // التقارير
            msbtnReports.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ViewReports);
            msbtnReportsDashboard.Visible = SessionManager.HasAccountingPermission((long)AccountingPermissions.ViewReports);
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء التحقق من صلاحيات المستخدم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // MVP: إعادة تطبيق إخفاء شاشات V2 بعد منطق الصلاحيات (الصلاحيات تُظهر بعض العناصر المخفية)
        ApplyMvpMenuVisibility();

        // Mock-mode visual indicator
        if (AppSettings.Current.UseMockData)
        {
            this.Text += "  — وضع التجربة (Mock)";
        }
    }

    // ── المنتجات (Products) ───────────────────────────────────────────────────

    private void msbtnManageProducts_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmProductMaster>());

    private void msbtnAddProduct_Click(object sender, EventArgs e)
    {
        // MVP: مخفي — add handled inside Master form
        // clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateProduct>());
    }

    private void msbtnManageCustomers_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCustomerMaster>());

    private void msbtnAddCustomer_Click(object sender, EventArgs e)
    {
        // MVP: مخفي — add handled inside Master form
        // clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateCustomer>());
    }

    private void msbtnManageSuppliers_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmSupplierMaster>());

    private void msbtnAddSupplier_Click(object sender, EventArgs e)
    {
        // MVP: مخفي — add handled inside Master form
        // clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateSupplier>());
    }

    private void msbtnManageCategories_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCategoryMaster>());

    private void msbtnManageUnits_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmUnitMaster>());

    private void msbtnProductPrices_Click(object sender, EventArgs e)
    {
        // TODO: product prices screen was removed — restore or repoint
        // clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmProductPrices>());
    }

    private void msbtnProductUnits_Click(object sender, EventArgs e)
    {
        // TODO: product units screen was removed — restore or repoint
        // clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmProductUnitsList>());
    }

    private void msbtnProductCategories_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCategoryMaster>());

    // ── المخزون (Warehouse / Stock Transfers / Inventory) ────────────────────

    private void msbtnManageWarehouses_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmWarehouseMaster>());

    private void msbtnStockTransfer_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListStockTransfers>());

    private void msbtnInventoryMovement_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmInventoryList>());

    private void msbtnInventoryAdjustments_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListInventoryAdjustments>());

    private void msbtnInventoryCounts_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListInventoryCounts>());

    private void msbtnInventory_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmInventory>());

    // ── المشتريات (Purchases) ────────────────────────────────────────────────

    private void msbtnManagePurchases_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListPurchases>());

    private void msbtnPurchaseInvoice_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmPurchaseInvoice>());

    private void msbtnPurchaseReturn_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmPurchaseReturn>());

    private void msbtnPurchases_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmPurchases>());

    // ── المبيعات (Sales) ─────────────────────────────────────────────────────

    private void msbtnManageSales_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListSales>());

    private void msbtnSalesInvoice_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmSalesInvoice>());

    private void msbtnSalesReturn_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmSalesReturn>());

    // ── المبيعات — نقطة البيع وأدوات الفاتورة (جديد) ────────────────

    private void msbtnPOS_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmPOS>());

    private void msbtnProductSelection_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<SalesSystem.Desktop.Screens.Sales.Forms.frmSmartProductSelection>().ShowDialog(this);

    private void msbtnCustomerPicker_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmCustomerPicker>().ShowDialog(this);

    private void msbtnDiscountExpenses_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmDiscountExpenses>().ShowDialog(this);

    private void msbtnCurrencySelection_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmCurrencySelection>().ShowDialog(this);

    // ── المبيعات — أدوات الفاتورة الإضافية (جديد) ───────────────────────

    private void msbtnSalesProfitBreakdown_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmSalesProfitBreakdown>().ShowDialog(this);

    private void msbtnPOSHeldOrders_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmPOSHeldOrders>().ShowDialog(this);

    private void msbtnSalesImportConvert_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmSalesImportConvert>().ShowDialog(this);

    private void msbtnReceiptPrint_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmReceiptPrint>().ShowDialog(this);

    private void msbtnSalesSmartF3Lookup_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmSalesSmartF3Lookup>().ShowDialog(this);

    private void msbtnSalesContinuousScanner_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmSalesContinuousScanner>().ShowDialog(this);

    private void msbtnSalesTouchGrid_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmSalesTouchGrid>().ShowDialog(this);

    private void msbtnSalesRapidBar_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmSalesRapidBar>().ShowDialog(this);

    private void msbtnSalesInlineGrid_Click(object sender, EventArgs e)
        => _serviceProvider.GetRequiredService<frmSalesInlineGrid>().ShowDialog(this);

    // ── الخزينة (Treasury) ───────────────────────────────────────────────────

    private void msbtnManagePayments_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListPayments>());

    private void msbtnCashBoxes_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCashBoxMaster>());

    private void msbtnBanks_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListBanks>());

    private void msbtnExpenses_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListExpenses>());

    private void msbtnCustomerReceipts_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListCustomerReceipts>());

    private void msbtnPaymentVouchers_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListPaymentVouchers>());

    private void msbtnSupplierPayments_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListSupplierPayments>());

    private void msbtnCashVouchers_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCashVouchers>());

    // ── حسابات (Accounting) ─────────────────────────────────────────────────

    private void msbtnChartOfAccounts_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListAccounts>());

    private void msbtnJournalEntries_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListJournalEntries>());

    private void msbtnAccountCategories_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListAccountCategories>());

    private void msbtnSystemAccountMappings_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmSystemAccountMappings>());

    private void msbtnReceiptVouchers_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListReceiptVouchers>());

    private void msbtnAccounting_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmAccounting>());

    // ── بيانات (Data / Users) ────────────────────────────────────────────────

    private void msbtnManageUsers_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListUsers>());

    private void msbtnRoles_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListRoles>());

    private void msbtnEmployees_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListEmployees>());

    private void msbtnPermissions_Click(object sender, EventArgs e)
    {
        // Permissions are now managed per-role via the Roles editor
        // Redirect to Roles list where permissions are configured
        MessageBox.Show("يتم إدارة الصلاحيات من خلال الأدوار. الرجاء استخدام قائمة الأدوار.", 
            "إدارة الصلاحيات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListRoles>());
    }

    private void msbtnAuditLog_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmAuditLog>());

    private void msbtnSystemLogs_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmSystemLogs>());

    private void msbtnUserSessions_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmUserSessions>());

    private void msbtnDocumentSequences_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmDocumentSequences>());

    private void msbtnTaxes_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListTaxes>());

    // ── تقارير (Reports) ─────────────────────────────────────────────────────

    private void msbtnReports_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmReports>());

    private void msbtnReportsDashboard_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmReportsDashboard>());

    // ── الإعدادات (Settings) ──────────────────────────────────────────────────

    private void msbtnSettings_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmSettings>());

    private void msbtnCompanySettings_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCompanySettings>());

    private void msbtnNotifications_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmNotifications>());

    private void msbtnUpdateCheck_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmUpdateCheck>());

    private void msbtnBackup_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmBackup>());

    // ── إعدادات الحساب (Logout) ──────────────────────────────────────────────

    private void msbtnLogout_Click(object sender, EventArgs e)
    {
        _sessionService.SignOut();
        Application.Restart();
    }

    // ── ملف (File) ────────────────────────────────────────────────────────────

    private void msbtnChangePassword_Click(object sender, EventArgs e)
    {
        using var frm = _serviceProvider.GetRequiredService<frmPasswordChange>();
        frm.ShowDialog(this);
    }

    private void msbtnExit_Click(object sender, EventArgs e)
    {
        Application.Exit();
        //this.Close();
    }
}
