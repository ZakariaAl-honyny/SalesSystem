// -----------------------------------------------------------------------------
// frmDashboard.cs — حاضن رقيق لـ ctrlDashboard (RULE-N66)
//
// لا يوجد هنا أي منطق لوحة تحكم: لا بطاقات، لا استدعاء IDashboardDesktopService،
// ولا اشتراكات EventBus. كل ذلك داخل المكوّن ctrlDashboard، وهذا النموذج
// يعرضه فقط ويتيح إغلاقه.
//
// الاستثناء الوحيد المسموح: أسطر توصيل مساعدة (KeyboardHelper) واختصار F5 الذي
// يفوّض التنفيذ إلى المكوّن — بلا أي قراءة بيانات داخل النموذج.
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

            // ── لوحة المفاتيح: الستة كاملة (RULE-N40…RULE-N47) ──
            KeyboardHelper.WireEnterAsTab(this);
            KeyboardHelper.WireSelectAllOnFocus(this);
            KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)); // RULE-N46 + RULE-N54b
            KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
            KeyboardHelper.WireButtonDisabledVisual(btnClose);                         // RULE-N43
            KeyboardHelper.WireGlobalShortcuts(this,                                   // RULE-N47
                (Keys.F5, RefreshDashboardCommand));                                   // تفويض للمكوّن

            ApplyUiTheme();
        }

        /// <summary>F5 — يفوّض التحديث للمكوّن بلا أي قراءة بيانات داخل النموذج (RULE-N66).</summary>
        private void RefreshDashboardCommand() => _ = _dashboard.RefreshAsync();

        private void ApplyUiTheme()
        {
            // لا يُستدعى ThemeHelper هنا: ألوان المصمّم هي المرجع وقت التشغيل
            // (RULE-N140 / RULE-N141) والثيم الأخضر يثبّتها لا يغيّرها.
            GreenThemeHelper.Apply(this);   // آخر سطر دائماً (RULE-N50)
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();
    }
}
