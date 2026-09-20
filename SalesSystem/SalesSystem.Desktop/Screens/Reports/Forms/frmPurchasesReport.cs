using SalesSystem.Desktop.Screens.Reports.Tabs;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmPurchasesReport : Form
{
    private readonly IServiceProvider _serviceProvider;
    private PurchasesReportTab _purchasesTab;

    public frmPurchasesReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);

        _purchasesTab = new PurchasesReportTab(_serviceProvider);
        _purchasesTab.Dock = DockStyle.Fill;
        this.Controls.Add(_purchasesTab);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
