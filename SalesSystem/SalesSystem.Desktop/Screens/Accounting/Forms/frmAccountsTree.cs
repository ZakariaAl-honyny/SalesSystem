// -----------------------------------------------------------------------------
// frmAccountsTree.cs — غلاف رقيق لشجرة دليل الحسابات (F-003 · المرحلة أ)
//
// • الشاشة لا تعرف الخدمة ولا تبني الشجرة: كل ذلك في ctrlTreeOfAccounts، وهي تُهيَّأ
//   بـAttach ثم RefreshAsync (RULE-N66 — الغلاف رقيق بلا منطق تحميل).
// • «إضافة حساب فرعي» و«تعديل» لا يُنفَّذان هنا: يُطلق العنصر حدثاً، ونحن نفتح
//   frmAccounts — البيت الواحد لتحرير الحساب (RULE-N100) — عبر clsGlobal._OpenForm
//   (RULE-N132) وبلا إغلاق هذه الشاشة.
// • التحميل والفتح والخدمة عبر IServiceProvider (RULE-N02/N03/N04).
// -----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Forms;

/// <summary>
/// نافذة اختبار/استخدام مستقلة للشجرة. الغرض أن تكون قابلة للتضمين لاحقاً داخل
/// <c>frmAccounts</c> بعد إقفال المهمة التي تملك تلك الشاشة.
/// </summary>
public partial class frmAccountsTree : Form
{
    private const string LoadFailedMessage = "تعذر تحميل دليل الحسابات";

    private readonly IServiceProvider _serviceProvider;

    public frmAccountsTree(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        InitializeComponent();

        // التوجيه إلى البيت الواحد للتحرير (RULE-N100): العنصر يُطلق، والنموذج يفتح
        ctrlTreeOfAccounts1.AddChildRequested += CtrlTree_AddChildRequested;
        ctrlTreeOfAccounts1.EditRequested += CtrlTree_EditRequested;

        // RULE-N40…RULE-N47 — الستة كاملة في نموذج جديد (§2.21 بند 7)
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                        // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnClose);                                               // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.Escape, () => Close()));                          // RULE-N47

        ApplyUiTheme();
    }

    /// <summary>
    /// يُهيّئ الشجرة ويحمّلها. عام لأن حاضناً مستقبلياً (دمج الشجرة في <c>frmAccounts</c>)
    /// يستطيع قيادة هذه النافذة بلا معرفة تفاصيلها.
    /// </summary>
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

    // ── التوجيه إلى محرّر الحسابات (RULE-N100) ────────────────────────────────

    private void CtrlTree_AddChildRequested(object? sender, int parentAccountId) => OpenAccountsScreen();

    private void CtrlTree_EditRequested(object? sender, int accountId) => OpenAccountsScreen();

    /// <summary>
    /// يفتح شاشة دليل الحسابات (محرّرها الوحيد) من DI عبر النمط المعتمد.
    /// <para>
    /// لا استهداف بمعرّف الحساب: <c>frmAccounts</c> لا يعرّض واجهة «اختر حساباً بالمعرّف»
    /// حتى اليوم، فتُفتح الشاشة ويختار المستخدم الحساب فيها — بند مفتوح في تقرير F-003.
    /// </para>
    /// </summary>
    private void OpenAccountsScreen()
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmAccounts>());

    // ── المظهر ────────────────────────────────────────────────────────────────

    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblTitle);   // RULE-N53
        GreenThemeHelper.Apply(this);             // آخر سطر دائماً (RULE-N50)
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();
}
