using SalesSystem.Desktop.Screens.Reports.Tabs;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmSalesReport : Form
{
    private readonly IServiceProvider _serviceProvider;
    private SalesReportTab _salesTab;

    public frmSalesReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);

        _salesTab = new SalesReportTab(_serviceProvider);
        _salesTab.Dock = DockStyle.Fill;
        this.Controls.Add(_salesTab);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
