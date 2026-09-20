using SalesSystem.Desktop.Screens.Reports.Tabs;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmLowStockReport : Form
{
    private readonly IServiceProvider _serviceProvider;
    private LowStockReportTab _lowStockTab;

    public frmLowStockReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);

        _lowStockTab = new LowStockReportTab(_serviceProvider);
        _lowStockTab.Dock = DockStyle.Fill;
        this.Controls.Add(_lowStockTab);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
