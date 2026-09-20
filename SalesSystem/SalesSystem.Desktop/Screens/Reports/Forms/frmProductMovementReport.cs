using SalesSystem.Desktop.Screens.Reports.Tabs;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmProductMovementReport : Form
{
    private readonly IServiceProvider _serviceProvider;
    private ProductMovementReportTab _productMovementTab;

    public frmProductMovementReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);

        _productMovementTab = new ProductMovementReportTab(_serviceProvider);
        _productMovementTab.Dock = DockStyle.Fill;
        this.Controls.Add(_productMovementTab);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
