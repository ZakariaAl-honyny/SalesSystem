using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.DTO;

namespace SalesSystem.Desktop.Screens.Customers.Forms;

/// <summary>
/// منتقي العميل (F-002) — نموذج <b>نمطي</b> رقيق: يلفّ <c>ctrlCustomerCardWithFilter</c>
/// ويُرجع العميل المختار عبر <see cref="SelectedCustomer"/> مع <see cref="DialogResult.OK"/>
/// — نفس نمط منتقيات المشروع القائمة (<c>frmCustomerSelection</c>/<c>frmProductSelection</c>)
/// فلا حاجة إلى حدث <c>DataBack</c> (USAGE.md §3–4).
/// </summary>
public partial class frmFindCustomer : Form
{
    public frmFindCustomer()
    {
        InitializeComponent();

        // RULE-N40…RULE-N47 — الحقل الوحيد للإدخال هو حقل التصفية (داخل العنصر المضمَّن).
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);
        KeyboardHelper.WireButtonDisabledVisual(btnSelect);   // RULE-N43

        // Enter = «اختيار» (AcceptButton) · Esc = «إغلاق» (CancelButton + الاختصار) — RULE-N47.
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.Escape, Close));

        ctrlCustomerCardWithFilter1.CustomerSelected += _OnCustomerSelected;

        ApplyUiTheme();
        _UpdateButtonsState();
    }

    public frmFindCustomer(IServiceProvider serviceProvider) : this()
    {
        // إلزامي قبل أي بحث — بدونه يبقى العنصر خامداً ويُبلّغ برسالة عربية (لا استثناء).
        ctrlCustomerCardWithFilter1.Attach(serviceProvider);
    }

    /// <summary>العميل المختار — يقرأه المستدعي بعد <c>ShowDialog</c> و<see cref="DialogResult.OK"/>.</summary>
    public CustomerDto? SelectedCustomer { get; private set; }

    // ── المظهر ────────────────────────────────────────────────────────────────

    /// <summary>الثيم (RULE-N50): آخر خطوة دائماً هي <c>GreenThemeHelper.Apply</c>.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ── المعالجات ─────────────────────────────────────────────────────────────

    /// <summary>التركيز يبدأ في حقل التصفية (نمط المرجع: البحث أول ما يراه المستخدم).</summary>
    private void frmFindCustomer_Shown(object? sender, EventArgs e)
        => ctrlCustomerCardWithFilter1.FilterFocus();

    private void _OnCustomerSelected(object? sender, CustomerDto? customer)
    {
        SelectedCustomer = customer;
        _UpdateButtonsState();
    }

    /// <summary>آلة حالة الأزرار (RULE-N43): «اختيار» يعمل على عميل معروض فقط.</summary>
    private void _UpdateButtonsState()
    {
        btnSelect.Enabled = SelectedCustomer is not null;
    }

    private void btnSelect_Click(object? sender, EventArgs e)
    {
        if (SelectedCustomer is null)
        {
            // إلغاء الإغلاق التلقائي الذي يضبطه DialogResult = OK على الزر (RULE-N28: لا نجاح كاذب).
            DialogResult = DialogResult.None;
            clsDialogs.Warning("يرجى البحث عن عميل واختياره أولاً.");
            ctrlCustomerCardWithFilter1.FilterFocus();
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();
}
