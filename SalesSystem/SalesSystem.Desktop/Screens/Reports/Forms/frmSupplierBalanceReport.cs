using SalesSystem.Desktop.Screens.Reports.Tabs;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmSupplierBalanceReport : Form
{
    private readonly IServiceProvider _serviceProvider;
    private SupplierBalanceReportTab _supplierBalanceTab;

    public frmSupplierBalanceReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);

        _supplierBalanceTab = new SupplierBalanceReportTab(_serviceProvider);
        _supplierBalanceTab.Dock = DockStyle.Fill;
        this.Controls.Add(_supplierBalanceTab);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
