// -----------------------------------------------------------------------------
// frmAccountsTree.cs — غلاف رقيق لعرض شجرة دليل الحسابات (قراءة فقط)
//
// • الشاشة لا تعرف الخدمة ولا تبني الشجرة: كل ذلك في ctrlTreeOfAccounts، وهي تُهيَّأ
//   بـAttach ثم RefreshAsync (RULE-N66 — الغلاف رقيق بلا منطق تحميل).
// • عرض فقط: لا أوامر ولا محرّر هنا. تحرير الحساب بيته frmAccounts (RULE-N100).
// • التحميل والخدمة عبر IServiceProvider (RULE-N02/N03/N04).
// -----------------------------------------------------------------------------

using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Forms;

/// <summary>
/// نافذة عرض مستقلة لشجرة الحسابات — عنصر <c>ctrlTreeOfAccounts</c> يملؤها.
/// </summary>
public partial class frmAccountsTree : Form
{
    private const string LoadFailedMessage = "تعذر تحميل دليل الحسابات";

    private readonly IServiceProvider _serviceProvider;

    public frmAccountsTree(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        InitializeComponent();

        // RULE-N40…RULE-N47 — الستة كاملة في نموذج جديد (§2.21 بند 7)
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                        // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnClose);                                               // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.Escape, () => Close()));                          // RULE-N47

        ApplyUiTheme();
    }

    /// <summary>يُهيّئ الشجرة ويحمّلها.</summary>
    public async Task LoadAsync(CancellationToken ct = default)
    {
        ctrlTreeOfAccounts1.Attach(_serviceProvider);
        await ctrlTreeOfAccounts1.RefreshAsync(ct);
    }

    private async void frmAccountsTree_Load(object? sender, EventArgs e)
    {
        try
        {
            await LoadAsync();
        }
        catch (Exception ex)
        {
            // الحدّ عند تحميل الشاشة (RULE-N205): رسالة عربية بلا ex.Message (RULE-N28/N29)
            Log.Error(ex, "شجرة دليل الحسابات: تعذر تحميل الشاشة");
            clsDialogs.Warning(LoadFailedMessage);
        }
    }

    // ── المظهر ────────────────────────────────────────────────────────────────

    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblTitle);   // RULE-N53
        GreenThemeHelper.Apply(this);             // آخر سطر دائماً (RULE-N50)
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();
}
