using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Users.Forms;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Controls.Users
{
    public partial class frmUsers : Form
    {
        private readonly IServiceProvider _serviceProvider;

        public frmUsers(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        private void btnOpenList_Click(object sender, EventArgs e)
        {
            var listForm = _serviceProvider.GetRequiredService<frmListUsers>();
            clsGlobal._OpenForm(listForm);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
