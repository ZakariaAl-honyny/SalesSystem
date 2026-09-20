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

        // Basic Reports
        AddTab("\u062a\u0642\u0631\u064a\u0631 \u0627\u0644\u0645\u0628\u064a\u0639\u0627\u062a", new SalesReportTab(_serviceProvider));
        AddTab("\u062a\u0642\u0631\u064a\u0631 \u0627\u0644\u0645\u0634\u062a\u0631\u064a\u0627\u062a", new PurchasesReportTab(_serviceProvider));
        AddTab("\u062a\u0642\u0631\u064a\u0631 \u0627\u0644\u0645\u062e\u0632\u0648\u0646", new InventoryReportTab(_serviceProvider));
        AddTab("\u0643\u0634\u0641 \u062d\u0633\u0627\u0628 \u0639\u0645\u0644\u0627\u0621", new CustomerBalanceReportTab(_serviceProvider));
        AddTab("\u0643\u0634\u0641 \u062d\u0633\u0627\u0628 \u0645\u0648\u0631\u062f\u064a\u0646", new SupplierBalanceReportTab(_serviceProvider));
        AddTab("\u062d\u0631\u0643\u0627\u062a \u0645\u0646\u062a\u062c", new ProductMovementReportTab(_serviceProvider));
        AddTab("\u062a\u0646\u0628\u064a\u0647 \u0627\u0644\u0645\u062e\u0632\u0648\u0646 \u0627\u0644\u0645\u0646\u062e\u0641\u0636", new LowStockReportTab(_serviceProvider));

        // Financial Reports
        AddTab("\u0642\u0627\u0626\u0645\u0629 \u0627\u0644\u062f\u062e\u0644", new IncomeStatementReportTab(_serviceProvider));
        AddTab("\u0627\u0644\u0645\u0631\u062c\u0639 \u0627\u0644\u0645\u0627\u0644\u064a", new BalanceSheetReportTab(_serviceProvider));
        AddTab("\u0627\u0644\u0645\u0631\u062c\u0639 \u0627\u0644\u0645\u062a\u0648\u0627\u0632\u0646", new TrialBalanceReportTab(_serviceProvider));
        AddTab("\u0627\u0644\u0623\u0633\u062c\u062f \u0627\u0644\u0639\u0627\u0645", new GeneralLedgerReportTab(_serviceProvider));
        AddTab("\u062a\u0642\u0631\u064a\u0631 \u0627\u0644\u062a\u062f\u0641\u0642 \u0627\u0644\u0646\u0642\u062f\u064a", new CashFlowReportTab(_serviceProvider));
        AddTab("\u0627\u0644\u0631\u0623\u0645\u0627\u0644 \u0627\u0644\u0639\u0627\u0645\u0644", new WorkingCapitalReportTab(_serviceProvider));
        AddTab("\u0636\u0631\u064a\u0628\u0629 \u0627\u0644\u0642\u0637\u0648\u0633 \u0627\u0644\u0645\u0631\u062c\u0639\u064a\u0629", new VatReportTab(_serviceProvider));
        AddTab("\u0643\u0634\u0641 \u062d\u0633\u0627\u0628 \u0639\u0645\u064a\u0644", new AccountStatementReportTab(_serviceProvider));

        // Sales Reports
        AddTab("\u0645\u0628\u064a\u0639\u0627\u062a \u0628\u0645\u0646\u062a\u062c", new SalesByProductReportTab(_serviceProvider));
        AddTab("\u0645\u0628\u064a\u0639\u0627\u062a \u0628\u0639\u0645\u0644\u0627\u0621", new SalesByCustomerReportTab(_serviceProvider));
        AddTab("\u0645\u0628\u064a\u0639\u0627\u062a \u0628\u062a\u0635\u0646\u064a\u0641", new SalesByCategoryReportTab(_serviceProvider));
        AddTab("\u0645\u0628\u064a\u0639\u0627\u062a \u064a\u0648\u0645\u064a\u0629", new DailySalesReportTab(_serviceProvider));
        AddTab("\u0627\u0644\u0631\u0628\u062d \u0645\u0646 \u0627\u0644\u0645\u0646\u062a\u062c", new ProductProfitabilityReportTab(_serviceProvider));
        AddTab("\u0627\u0644\u0631\u0628\u062d \u0628\u0639\u0645\u0644\u0627\u0621", new ProfitByCustomerReportTab(_serviceProvider));

        // Purchase Reports
        AddTab("\u0645\u0634\u062a\u0631\u064a\u0627\u062a \u0628\u0645\u0648\u0631\u062f", new PurchasesBySupplierReportTab(_serviceProvider));
        AddTab("\u0645\u0634\u062a\u0631\u064a\u0627\u062a \u0628\u0645\u0646\u062a\u062c", new PurchasesByProductReportTab(_serviceProvider));

        // Inventory Reports
        AddTab("\u0631\u0635\u064a\u062f \u0627\u0644\u0645\u062e\u0632\u0648\u0646", new StockBalanceReportTab(_serviceProvider));
        AddTab("\u062f\u0641\u062a\u0631 \u0627\u0644\u0645\u062e\u0632\u0648\u0646 \u0627\u0644\u062a\u0641\u0635\u064a\u0644\u064a", new DetailedStockLedgerReportTab(_serviceProvider));
        AddTab("\u062d\u0631\u0643\u0627\u062a \u0627\u0644\u0645\u062e\u0627\u0632\u0646", new WarehouseMovementReportTab(_serviceProvider));
        AddTab("\u0627\u0644\u0645\u0646\u062a\u062c\u0627\u062a \u0627\u0644\u0645\u0647\u0644\u0643\u0629", new ExpiredProductsReportTab(_serviceProvider));

        // Other Reports
        AddTab("\u062a\u0642\u0631\u064a\u0631 \u0627\u0644\u0645\u0631\u062a\u062c\u0639\u0627\u062a", new ReturnsReportTab(_serviceProvider));
        AddTab("\u0625\u063a\u0644\u0627\u0642 \u064a\u0648\u0645\u064a", new DailyClosureReportTab(_serviceProvider));
        AddTab("\u0643\u0634\u0641 \u0635\u0646\u062f\u064a\u0642", new CashBoxSummaryReportTab(_serviceProvider));
        AddTab("\u0627\u0644\u0623\u0642\u0633\u0627\u0645 \u0627\u0644\u0645\u062a\u0639\u0644\u0642\u0629", new AgingReportTab(_serviceProvider));

        this.Controls.Add(tabControl);
    }

    private void AddTab(string title, UserControl control)
    {
        control.Dock = DockStyle.Fill;
        var page = new TabPage(title);
        page.Controls.Add(control);
        tabControl.TabPages.Add(page);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
