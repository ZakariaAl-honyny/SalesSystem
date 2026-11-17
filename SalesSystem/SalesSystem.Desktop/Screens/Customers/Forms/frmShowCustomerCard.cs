using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Customers.Forms;

/// <summary>
/// نموذج عرض بطاقة العميل (F-002) — غلاف <b>رقيق</b> حول <c>ctrlCustomerCard</c>:
/// يجلب السجل عبر <c>ICustomerDesktopService.GetByIdAsync</c> ويمرّره للبطاقة،
/// ويُغلقه/يفتح شاشة الدليل عبر البيت الواحد <see cref="clsGlobal._OpenForm"/> (RULE-N132).
/// </summary>
public partial class frmShowCustomerCard : Form
{
    private readonly IServiceProvider? _serviceProvider;

    /// <summary>
    /// منشئ التصميم — عام حتى يفتح النموذج في مصمّم Visual Studio بلا خدمات
    /// (RULE-N134: «المصمّم = التشغيل»)؛ حاوية DI تختار المُنشئ الأكثر وسائط.
    /// </summary>
    public frmShowCustomerCard()
    {
        InitializeComponent();

        // RULE-N40…RULE-N47 — الحقول/الروابط في هذه النافذة لا تُدخل بيانات، فالمستهلك منها
        // التحديد والتظليل والأسهم ومظهر التعطيل والاختصارات (RULE-N95).
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);
        KeyboardHelper.WireButtonDisabledVisual(btnEdit, btnClose);   // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.Escape, Close)); // RULE-N47

        // رابط البطاقة يُنفّذ نفس زر «تعديل» — مسار واحد للفتح، بلا منطق مكرر (RULE-N169).
        ctrlCustomerCard1.EditRequested += (_, _) => btnEdit.PerformClick();

        ApplyUiTheme();
        _UpdateButtonsState();
    }

    public frmShowCustomerCard(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
    }

    private ICustomerDesktopService Service
        => _serviceProvider!.GetRequiredService<ICustomerDesktopService>();

    // ── التحميل ───────────────────────────────────────────────────────────────

    /// <summary>
    /// يحمّل بطاقة العميل بمعرّفه. عند فشل القراءة: رسالة عربية عبر <see cref="clsDialogs"/> + تصفير البطاقة
    /// (لا يُعرض نصف سجل ولا يُسرَّب نص استثناء للمستخدم).
    /// </summary>
    public async Task LoadCustomerAsync(int id, CancellationToken ct = default)
    {
        if (_serviceProvider is null) return;

        if (id <= 0)
        {
            clsDialogs.Warning("تعذر تحميل بطاقة العميل: المعرّف غير صالح.");
            ctrlCustomerCard1.ResetCustomerInfo();
            _UpdateButtonsState();
            return;
        }

        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await Service.GetByIdAsync(id, ct);

            if (!result.IsSuccess || result.Value is null)
            {
                clsDialogs.Warning(result.Error ?? "تعذر تحميل بطاقة العميل. يرجى إعادة المحاولة.");
                ctrlCustomerCard1.ResetCustomerInfo();
                return;
            }

            ctrlCustomerCard1.LoadData(result.Value);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل بطاقة العميل {CustomerId}", id);
            clsDialogs.Warning("تعذر تحميل بطاقة العميل. يرجى إعادة المحاولة.");
            ctrlCustomerCard1.ResetCustomerInfo();
        }
        finally
        {
            Cursor = Cursors.Default;
            _UpdateButtonsState();
        }
    }

    // ── المظهر ────────────────────────────────────────────────────────────────

    /// <summary>الثيم (RULE-N50): آخر خطوة دائماً هي <c>GreenThemeHelper.Apply</c>.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblTitle);
        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ── آلة حالة الأزرار (RULE-N43) ───────────────────────────────────────────

    /// <summary>«تعديل» يعمل على بطاقة تحمل سجلاً فقط — لا زر بلا هدف.</summary>
    private void _UpdateButtonsState()
    {
        btnEdit.Enabled = ctrlCustomerCard1.CustomerId.HasValue;
    }

    // ── المعالجات ─────────────────────────────────────────────────────────────

    /// <summary>
    /// «تعديل»: يفتح شاشة الدليل من DI (RULE-N02) عبر البيت الواحد لفتح النماذج (RULE-N132)،
    /// ثم تُغلق البطاقة لأنها قراءة فقط ولن تعرض التعديل الحادث.
    /// </summary>
    private void btnEdit_Click(object? sender, EventArgs e)
    {
        if (ctrlCustomerCard1.CustomerId is null) return;

        if (_serviceProvider is null)
        {
            clsDialogs.Warning("تعذر فتح دليل العملاء: الشاشة غير مهيأة.");
            return;
        }

        try
        {
            var master = _serviceProvider.GetRequiredService<frmCustomerMaster>();
            clsGlobal._OpenForm(master);
            Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء فتح دليل العملاء من بطاقة العميل");
            clsDialogs.Error("تعذر فتح دليل العملاء. يرجى إعادة المحاولة.");
        }
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();
}
