using SalesSystem.Desktop.Screens.Reports.Tabs;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmInventoryReport : Form
{
    private readonly IServiceProvider _serviceProvider;
    private InventoryReportTab _inventoryTab;

    public frmInventoryReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);

        _inventoryTab = new InventoryReportTab(_serviceProvider);
        _inventoryTab.Dock = DockStyle.Fill;
        this.Controls.Add(_inventoryTab);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
