// -----------------------------------------------------------------------------
// frmDashboard.cs — حاضن رقيق لـ ctrlDashboard (RULE-N66)
//
// لا يوجد هنا أي منطق لوحة تحكم: لا بطاقات، لا استدعاء IDashboardDesktopService،
// ولا اشتراكات EventBus. كل ذلك داخل المكوّن ctrlDashboard، وهذا النموذج
// يعرضه فقط ويتيح إغلاقه.
// -----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Screens.Dashboard.Controls;

namespace SalesSystem.Desktop.Screens.Dashboard.Forms
{
    public partial class frmDashboard : Form
    {
        private readonly ctrlDashboard _dashboard;

        public frmDashboard(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // RULE-N64…N67 — المكوّن يُحلّ من DI ولا يُنشأ بـ new
            _dashboard = serviceProvider.GetRequiredService<ctrlDashboard>();
            _dashboard.Dock = DockStyle.Fill;
            pnlHost.Controls.Add(_dashboard);

            ApplyUiTheme();
        }

        private void ApplyUiTheme()
        {
            // الترتيب إلزامي: ThemeHelper أولاً ثم الثيم الأخضر آخر سطر (RULE-N50)
            ThemeHelper.ApplyMainFormStyle(this);
            ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Secondary);
            GreenThemeHelper.Apply(this);
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();
    }
}
