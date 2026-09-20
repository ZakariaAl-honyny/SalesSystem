using SalesSystem.Desktop.Screens.Reports.Tabs;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;
public partial class frmCustomerBalanceReport : Form
{
    private readonly IServiceProvider _serviceProvider;
    private CustomerBalanceReportTab _customerBalanceTab;

    public frmCustomerBalanceReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);

        _customerBalanceTab = new CustomerBalanceReportTab(_serviceProvider);
        _customerBalanceTab.Dock = DockStyle.Fill;
        this.Controls.Add(_customerBalanceTab);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
