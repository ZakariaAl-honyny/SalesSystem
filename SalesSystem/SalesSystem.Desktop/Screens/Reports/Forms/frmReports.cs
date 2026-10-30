using SalesSystem.Desktop.Screens.Reports.Tabs;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmReports : Form
{
    private readonly IServiceProvider _serviceProvider;
    private TabControl tabControl;

    public frmReports(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);

        tabControl = new TabControl
        {
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes,
            RightToLeftLayout = true
        };

        // ═══════════════════════════════════════════════════════════════════════
        // MVP: التقارير الأساسية الستة المعروضة للمستخدم فقط (HIDE, never delete)
        // ═══════════════════════════════════════════════════════════════════════
        AddTab("تقرير المبيعات", () => new SalesReportTab(_serviceProvider));                       // MVP: أساسي 1
        AddTab("تقرير المشتريات", () => new PurchasesReportTab(_serviceProvider));                  // MVP: أساسي 2
        AddTab("تقرير المخزون الحالي", () => new InventoryReportTab(_serviceProvider));             // MVP: أساسي 3
        AddTab("تقرير الأرباح", () => new ProductProfitabilityReportTab(_serviceProvider));         // MVP: أساسي 4
        AddTab("كشف حساب عميل", () => new AccountStatementReportTab(_serviceProvider));             // MVP: أساسي 5
        AddTab("كشف حساب مورد", () => new AccountStatementReportTab(_serviceProvider, true));        // MVP: أساسي 6

        // ═══════════════════════════════════════════════════════════════════════
        // MVP: مخفي — التقارير التالية محفوظة كاملة في الكود لكنها لا تُعرض
        // لإعادة تفعيل أي تقرير: احذف الوسيط الأخير (false) من السطر المعني
        // ═══════════════════════════════════════════════════════════════════════

        // Basic Reports
        AddTab("كشف حساب عملاء", () => new CustomerBalanceReportTab(_serviceProvider), false);      // MVP: مخفي
        AddTab("كشف حساب موردين", () => new SupplierBalanceReportTab(_serviceProvider), false);     // MVP: مخفي
        AddTab("حركات منتج", () => new ProductMovementReportTab(_serviceProvider), false);          // MVP: مخفي
        AddTab("تنبيه المخزون المنخفض", () => new LowStockReportTab(_serviceProvider), false);      // MVP: مخفي

        // Financial Reports
        AddTab("قائمة الدخل", () => new IncomeStatementReportTab(_serviceProvider), false);         // MVP: مخفي
        AddTab("الميزانية العمومية", () => new BalanceSheetReportTab(_serviceProvider), false);     // MVP: مخفي
        AddTab("ميزان المراجعة", () => new TrialBalanceReportTab(_serviceProvider), false);         // MVP: مخفي
        AddTab("دفتر الأستاذ العام", () => new GeneralLedgerReportTab(_serviceProvider), false);    // MVP: مخفي
        AddTab("التدفق النقدي", () => new CashFlowReportTab(_serviceProvider), false);              // MVP: مخفي
        AddTab("رأس المال العامل", () => new WorkingCapitalReportTab(_serviceProvider), false);     // MVP: مخفي
        AddTab("ضريبة القيمة المضافة", () => new VatReportTab(_serviceProvider), false);            // MVP: مخفي

        // Sales Reports
        AddTab("مبيعات بمنتج", () => new SalesByProductReportTab(_serviceProvider), false);         // MVP: مخفي
        AddTab("مبيعات بعملاء", () => new SalesByCustomerReportTab(_serviceProvider), false);       // MVP: مخفي
        AddTab("مبيعات بتصنيف", () => new SalesByCategoryReportTab(_serviceProvider), false);       // MVP: مخفي
        AddTab("مبيعات يومية", () => new DailySalesReportTab(_serviceProvider), false);             // MVP: مخفي
        AddTab("الربح بعملاء", () => new ProfitByCustomerReportTab(_serviceProvider), false);       // MVP: مخفي

        // Purchase Reports
        AddTab("مشتريات بمورد", () => new PurchasesBySupplierReportTab(_serviceProvider), false);   // MVP: مخفي
        AddTab("مشتريات بمنتج", () => new PurchasesByProductReportTab(_serviceProvider), false);    // MVP: مخفي

        // Inventory Reports
        AddTab("رصيد المخزون", () => new StockBalanceReportTab(_serviceProvider), false);           // MVP: مخفي
        AddTab("دفتر المخزون التفصيلي", () => new DetailedStockLedgerReportTab(_serviceProvider), false); // MVP: مخفي
        AddTab("حركات المخازن", () => new WarehouseMovementReportTab(_serviceProvider), false);     // MVP: مخفي
        AddTab("المنتجات المهلكة", () => new ExpiredProductsReportTab(_serviceProvider), false);    // MVP: مخفي

        // Other Reports
        AddTab("تقرير المرتجعات", () => new ReturnsReportTab(_serviceProvider), false);             // MVP: مخفي
        AddTab("إغلاق يومي", () => new DailyClosureReportTab(_serviceProvider), false);             // MVP: مخفي
        AddTab("كشف صندوق", () => new CashBoxSummaryReportTab(_serviceProvider), false);            // MVP: مخفي
        AddTab("الأعمار (Aging)", () => new AgingReportTab(_serviceProvider), false);               // MVP: مخفي

        this.Controls.Add(tabControl);
    }

    /// <summary>
    /// إضافة تبويب تقرير. عند تمرير <paramref name="mvpVisible"/> = false لا يُنشأ التبويب
    /// ولا يُضاف إلى شريط التبويبات (مخفي عن المستخدم) مع الاحتفاظ الكامل بكود التقرير.
    /// </summary>
    private void AddTab(string title, Func<UserControl> reportFactory, bool mvpVisible = true)
    {
        if (!mvpVisible) return; // MVP: مخفي — التقرير غير معروض للمستخدم

        var control = reportFactory();
        control.Dock = DockStyle.Fill;
        var page = new TabPage(title);
        page.Controls.Add(control);
        tabControl.TabPages.Add(page);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
