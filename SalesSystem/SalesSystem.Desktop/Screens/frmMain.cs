using Serilog;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Products.Forms;
using SalesSystem.Desktop.CustomerReceipts.Forms;

using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Screens.Users.Forms;
using SalesSystem.Desktop.Screens.Units.Forms;
using SalesSystem.Desktop.Screens.Sales.Forms;
using SalesSystem.Desktop.Screens.POS.Forms;
using SalesSystem.Desktop.Screens.Employees.Forms;
using SalesSystem.Desktop.Screens.Products.Forms;
using SalesSystem.Desktop.Screens.Purchases.Forms;
using SalesSystem.Desktop.Screens.Returns.Forms;
using SalesSystem.Desktop.Screens.Customers.Forms;
using SalesSystem.Desktop.Screens.Accounting.Forms;
using SalesSystem.Desktop.Screens.Categories.Forms;
using SalesSystem.Desktop.Screens.Payments.Forms;
using SalesSystem.Desktop.Screens.Expenses.Forms;
using SalesSystem.Desktop.Screens.CashBoxes.Forms;
using SalesSystem.Desktop.Screens.Inventory.Forms;
using SalesSystem.Desktop.Screens.Warehouses.Forms;
using SalesSystem.Desktop.Suppliers.Forms;
using SalesSystem.Desktop.Screens.Reports.Forms;
using SalesSystem.Desktop.Screens.Settings.Forms;
using SalesSystem.Desktop.Screens.Dashboard.Forms;
using SalesSystem.Desktop.Screens.Tools.Forms;
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

        ApplyUiTheme();   // RULE-N50 — آخر خطوة في تهيئة المظهر
    }

    // ── المظهر (RULE-N50/N53/N54) ─────────────────────────────────────────────

    /// <summary>
    /// مظهر الهيكل: شريط القوائم من لوحة الثيم الأخضر (لا قيم لونية جديدة هنا)،
    /// وتنتهي الدالة بـ <see cref="GreenThemeHelper.Apply(Form)"/> كآخر سطر (RULE-N50).
    /// <para>
    /// <c>frmMain</c> حاوية MDI للتنقّل لا شاشة بيانات: أسطح المحتوى (منطقة MDI)
    /// تُترك كما ضبطها المصمّم (RULE-N139/RULE-N140) ولا تُعاد تلوينها.
    /// </para>
    /// </summary>
    private void ApplyUiTheme()
    {
        msMainMenu.BackColor = GreenThemeHelper.BarBackground;   // RULE-N54
        msMainMenu.ForeColor = GreenThemeHelper.TextStrong;      // RULE-N53

        GreenThemeHelper.Apply(this);                            // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>
    /// MVP (محل واحد / مستخدم واحد / فرع واحد): إخفاء عناصر القوائم المؤجلة إلى V2.
    /// </summary>
    private void ApplyMvpMenuVisibility()
    {
        // ── المبيعات ────────────────────────────────────────────────────────
        msbtnCurrencySelection.Visible = false;     // MVP: محذوف (وحدة العملات الوهمية) — يبقى مخفياً

        // ── الإعدادات ───────────────────────────────────────────────────────
        // الإعدادات العامة وإعدادات الشركة أُزيلتا — تبقى شاشتا النسخ الاحتياطي والتحديث فقط
        // (شاشتا النسخ الاحتياطي والتحديث مخفيتان في MVP — انظر msbtnUpdateCheck أدناه)
        msbtnSettings.Visible = false;
        msbtnCompanySettings.Visible = false;
        msbtnUpdateCheck.Visible = false; // V2
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

        // لون منطقة MDI = خلفية النموذج المصمَّمة (RULE-N139/RULE-N140: التشغيل = المصمّم)
        var MdiContainerPage = this.Controls.OfType<MdiClient>().FirstOrDefault();
        if (MdiContainerPage != null)
            MdiContainerPage.BackColor = GreenThemeHelper.FormBackground;

        // MVP: إخفاء شاشات V2 (يُطبَّق أيضاً في المنشئ — إبقاء العناصر المؤجلة مخفية بعد التحميل)
        ApplyMvpMenuVisibility();

        // Mock-mode visual indicator
        if (AppSettings.Current.UseMockData)
        {
            this.Text += "  — وضع التجربة (Mock)";
        }
    }

    // ── فتح الشاشات (RULE-N02 / RULE-N132 / RULE-N133) ────────────────────────

    /// <summary>
    /// يفتح الشاشة المطلوبة من حاوية DI حصراً — ممنوع إنشاء النماذج يدوياً (RULE-N02).
    /// يفحص النوافذ المفتوحة **قبل** إنشاء النسخة فلا تُنشأ نسخة مهملة (RULE-N133)،
    /// ثم يفوّض الفتح إلى النمط المعتمد <see cref="clsGlobal._OpenForm"/> (RULE-N132).
    /// </summary>
    private void OpenScreen<T>() where T : Form
    {
        string screenName = typeof(T).Name;

        foreach (Form openForm in Application.OpenForms)
        {
            if (!string.Equals(openForm.Name, screenName, StringComparison.Ordinal))
                continue;

            // النافذة مفتوحة: إعادة تنشيطها بدل تكرارها
            openForm.WindowState = FormWindowState.Normal;
            openForm.Activate();
            return;
        }

        try
        {
            clsGlobal._OpenForm(_serviceProvider.GetRequiredService<T>());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر فتح الشاشة {Screen}", screenName);
            clsDialogs.Error("تعذر فتح الشاشة المطلوبة. يرجى المحاولة مرة أخرى.");
        }
    }

    // ── لوحة التحكم (Dashboard) ───────────────────────────────────────────────

    private void msbtnDashboard_Click(object sender, EventArgs e)
        => OpenScreen<frmDashboard>();

    // ── المنتجات (Products) ───────────────────────────────────────────────────

    private void msbtnManageProducts_Click(object sender, EventArgs e)
        => OpenScreen<frmProductMaster>();

    private void msbtnImportProducts_Click(object sender, EventArgs e)
        => OpenScreen<frmImportProductsExcel>();

    private void msbtnManageCategories_Click(object sender, EventArgs e)
        => OpenScreen<frmCategoryMaster>();

    private void msbtnManageUnits_Click(object sender, EventArgs e)
        => OpenScreen<frmUnitMaster>();

    // ── العملاء والموردون ────────────────────────────────────────────────────

    private void msbtnManageCustomers_Click(object sender, EventArgs e)
        => OpenScreen<frmCustomerMaster>();

    private void msbtnManageSuppliers_Click(object sender, EventArgs e)
        => OpenScreen<frmSupplierMaster>();

    // ── المخزون (Warehouse / Stock Transfers / Inventory) ────────────────────

    private void msbtnManageWarehouses_Click(object sender, EventArgs e)
        => OpenScreen<frmWarehouseMaster>();

    private void msbtnInventoryMovement_Click(object sender, EventArgs e)
        => OpenScreen<frmInventoryList>();

    private void msbtnInventoryTransactions_Click(object sender, EventArgs e)
        => OpenScreen<frmListInventoryTransactions>();

    private void msbtnInventoryBatches_Click(object sender, EventArgs e)
        => OpenScreen<frmListInventoryBatches>();

    private void msbtnInventoryAdjustments_Click(object sender, EventArgs e)
        => OpenScreen<frmListInventoryAdjustments>();

    private void msbtnOpeningStock_Click(object sender, EventArgs e)
        => OpenScreen<frmOpeningStock>();

    /// <summary>جرد المخزون — كان معالجاً فارغاً في MVP، والشاشة <c>frmStocktakingNew</c> موجودة.</summary>
    private void msbtnInventoryCounts_Click(object sender, EventArgs e)
        => OpenScreen<frmStocktakingNew>();

    /// <summary>تحويل مخزني بين المستودعات.</summary>
    private void msbtnInventoryTransfer_Click(object sender, EventArgs e)
        => OpenScreen<frmWarehouseTransferNew>();

    /// <summary>عرض توزيع الدفعات (FIFO / FEFO).</summary>
    private void msbtnInventoryFifo_Click(object sender, EventArgs e)
        => OpenScreen<frmInventoryFifo>();

    /// <summary>طباعة باركود الأصناف.</summary>
    private void msbtnInventoryBarcode_Click(object sender, EventArgs e)
        => OpenScreen<frmInventoryBarcode>();

    private void msbtnInventory_Click(object sender, EventArgs e)
        => OpenScreen<frmInventory>();

    // ── المشتريات (Purchases) ────────────────────────────────────────────────

    private void msbtnManagePurchases_Click(object sender, EventArgs e)
        => OpenScreen<frmListPurchases>();

    private void msbtnPurchaseInvoice_Click(object sender, EventArgs e)
        => OpenScreen<frmPurchaseInvoice>();

    private void msbtnPurchaseReturn_Click(object sender, EventArgs e)
        => OpenScreen<frmPurchaseReturn>();

    // ── المبيعات (Sales) ─────────────────────────────────────────────────────

    private void msbtnManageSales_Click(object sender, EventArgs e)
        => OpenScreen<frmListSales>();

    private void msbtnSalesInvoice_Click(object sender, EventArgs e)
        => OpenScreen<frmAddUpdateSalesInvoice>();

    private void msbtnSalesReturn_Click(object sender, EventArgs e)
        => OpenScreen<frmSalesReturn>();

    private void msbtnReturnsList_Click(object sender, EventArgs e)
        => OpenScreen<frmListReturns>();

    // ── المبيعات — نقطة البيع وأدوات الفاتورة (جديد) ────────────────

    private void msbtnPOS_Click(object sender, EventArgs e)
        => OpenScreen<frmPOS>();

    private void msbtnCurrencySelection_Click(object sender, EventArgs e)
        => clsDialogs.Info("وحدة العملات غير متوفرة في نسخة MVP الحالية؛ العملة الافتراضية هي الريال اليمني.",
            "العملات");

    // ── الخزينة (Treasury) ───────────────────────────────────────────────────

    private void msbtnManagePayments_Click(object sender, EventArgs e)
        => OpenScreen<frmListPayments>();

    private void msbtnCashBoxes_Click(object sender, EventArgs e)
        => OpenScreen<frmCashBoxMaster>();

    private void msbtnCashBoxTransactions_Click(object sender, EventArgs e)
        => OpenScreen<frmCashBoxTransactions>();

    private void msbtnExpenses_Click(object sender, EventArgs e)
        => OpenScreen<frmListExpenses>();

    private void msbtnCustomerReceipts_Click(object sender, EventArgs e)
        => OpenScreen<frmListCustomerReceipts>();

    private void msbtnSupplierPayments_Click(object sender, EventArgs e)
        => OpenScreen<frmListSupplierPayments>();

    // ── بيانات (Data / Users) ────────────────────────────────────────────────

    private void msbtnManageUsers_Click(object sender, EventArgs e)
        => OpenScreen<frmUserMaster>();

    private void msbtnEmployees_Click(object sender, EventArgs e)
        => OpenScreen<frmEmployeeMaster>();

    // ── المحاسبة (Accounting) ────────────────────────────────────────────────

    private void msbtnChartOfAccounts_Click(object sender, EventArgs e)
        => OpenScreen<frmAccounts>();

    private void msbtnJournalEntries_Click(object sender, EventArgs e)
        => OpenScreen<frmJournalEntries>();

    // ── تقارير (Reports) ─────────────────────────────────────────────────────
    // RULE-N70/N71/N72: تقرير واحد = شاشة واحدة، كل تقرير يُفتح من عنصره في تصنيفه.

    // تقارير المبيعات
    private void msReportSalesReport_Click(object sender, EventArgs e)
        => OpenScreen<frmSalesReport>();

    private void msReportDailySales_Click(object sender, EventArgs e)
        => OpenScreen<frmDailySalesReport>();

    private void msReportSalesByCustomer_Click(object sender, EventArgs e)
        => OpenScreen<frmSalesByCustomerReport>();

    private void msReportSalesByProduct_Click(object sender, EventArgs e)
        => OpenScreen<frmSalesByProductReport>();

    private void msReportSalesByCategory_Click(object sender, EventArgs e)
        => OpenScreen<frmSalesByCategoryReport>();

    private void msReportProfitByCustomer_Click(object sender, EventArgs e)
        => OpenScreen<frmProfitByCustomerReport>();

    private void msReportProductProfitability_Click(object sender, EventArgs e)
        => OpenScreen<frmProductProfitabilityReport>();

    private void msReportReturns_Click(object sender, EventArgs e)
        => OpenScreen<frmReturnsReport>();

    // تقارير المشتريات
    private void msReportPurchasesReport_Click(object sender, EventArgs e)
        => OpenScreen<frmPurchasesReport>();

    private void msReportPurchasesBySupplier_Click(object sender, EventArgs e)
        => OpenScreen<frmPurchasesBySupplierReport>();

    private void msReportPurchasesByProduct_Click(object sender, EventArgs e)
        => OpenScreen<frmPurchasesByProductReport>();

    // تقارير المخزون
    private void msReportInventoryReport_Click(object sender, EventArgs e)
        => OpenScreen<frmInventoryReport>();

    private void msReportStockBalance_Click(object sender, EventArgs e)
        => OpenScreen<frmStockBalanceReport>();

    private void msReportWarehouseMovement_Click(object sender, EventArgs e)
        => OpenScreen<frmWarehouseMovementReport>();

    private void msReportDetailedStockLedger_Click(object sender, EventArgs e)
        => OpenScreen<frmDetailedStockLedgerReport>();

    private void msReportProductMovement_Click(object sender, EventArgs e)
        => OpenScreen<frmProductMovementReport>();

    private void msReportLowStock_Click(object sender, EventArgs e)
        => OpenScreen<frmLowStockReport>();

    private void msReportExpiredProducts_Click(object sender, EventArgs e)
        => OpenScreen<frmExpiredProductsReport>();

    // التقارير المالية والمحاسبية
    private void msReportTrialBalance_Click(object sender, EventArgs e)
        => OpenScreen<frmTrialBalanceReport>();

    private void msReportGeneralLedger_Click(object sender, EventArgs e)
        => OpenScreen<frmGeneralLedgerReport>();

    private void msReportIncomeStatement_Click(object sender, EventArgs e)
        => OpenScreen<frmIncomeStatementReport>();

    private void msReportBalanceSheet_Click(object sender, EventArgs e)
        => OpenScreen<frmBalanceSheetReport>();

    private void msReportCashFlow_Click(object sender, EventArgs e)
        => OpenScreen<frmCashFlowReport>();

    private void msReportWorkingCapital_Click(object sender, EventArgs e)
        => OpenScreen<frmWorkingCapitalReport>();

    private void msReportVat_Click(object sender, EventArgs e)
        => OpenScreen<frmVatReport>();

    private void msReportAccountStatement_Click(object sender, EventArgs e)
        => OpenScreen<frmAccountStatementReport>();

    // تقارير العملاء والموردين
    private void msReportCustomerBalance_Click(object sender, EventArgs e)
        => OpenScreen<frmCustomerBalanceReport>();

    private void msReportSupplierBalance_Click(object sender, EventArgs e)
        => OpenScreen<frmSupplierBalanceReport>();

    private void msReportAging_Click(object sender, EventArgs e)
        => OpenScreen<frmAgingReport>();

    // تقارير الصندوق
    private void msReportCashBoxSummary_Click(object sender, EventArgs e)
        => OpenScreen<frmCashBoxSummaryReport>();

    private void msReportDailyClosure_Click(object sender, EventArgs e)
        => OpenScreen<frmDailyClosureReport>();

    // ── أدوات (Tools) ────────────────────────────────────────────────────────

    private void msbtnCalculator_Click(object sender, EventArgs e)
        => OpenScreen<frmCalculator>();

    // ── الإعدادات (Settings) ──────────────────────────────────────────────────
    // الإعدادات العامة وإعدادات الشركة أُزيلتا من الواجهة — المعالجات تبقى فارغة
    // فقط لأنهما مرتبطتان في ملف الـ Designer.

    /// <summary>
    /// يفتح شاشة «ألوان النظام» من الحاوية (RULE-N02) عبر النمط المعتمد
    /// <see cref="OpenScreen{T}"/> — الشاشة تُعدّل <see cref="ThemePalette"/> ثم
    /// تُعيد تلوين كل النماذج المفتوحة بلا إغلاق أي منها.
    /// </summary>
    private void msbtnSystemColors_Click(object sender, EventArgs e)
        => OpenScreen<frmChangeSystemColorsSettings>();

    /// <summary>
    /// يفتح شاشة «إعدادات التنسيق» من الحاوية (RULE-N02) عبر النمط المعتمد
    /// <see cref="OpenScreen{T}"/> — الشاشة تُعدّل تنسيق الأرقام والتواريخ والمبالغ.
    /// </summary>
    private void msbtnFormatSettings_Click(object sender, EventArgs e)
        => OpenScreen<frmFormatingSetting>();

    private void msbtnSettings_Click(object sender, EventArgs e)
    {
        // أُزيلت شاشة الإعدادات العامة (Settings فقط: نسخ احتياطي + تحديث)
    }

    private void msbtnCompanySettings_Click(object sender, EventArgs e)
    {
        // أُزيلت شاشة إعدادات الشركة
    }

    private void msbtnUpdateCheck_Click(object sender, EventArgs e)
        { /* V2: update-check removed */ }

    private void msbtnBackup_Click(object sender, EventArgs e)
        => OpenScreen<frmBackup>();

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
