using System.ComponentModel;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.DTO;

namespace SalesSystem.Desktop.Screens.Customers.Controls;

/// <summary>
/// بطاقة عرض العميل — <b>قراءة فقط</b> (F-002).
/// <para>
/// البطاقة <b>لا تعرف أي خدمة</b> ولا تقرأ من أي طبقة: تستقبل <see cref="CustomerDto"/> جاهزاً من
/// الحاضن (<c>LoadData</c>) — فيبقى مسار البيانات واحداً عبر <c>ICustomerDesktopService</c>
/// (RULE-N05 / RULE-N06).
/// </para>
/// <para>
/// كل السلوك المشترك يُستهلك من بيته: الثيم من <see cref="GreenThemeHelper"/>، والتنسيق من
/// <see cref="clsFormat"/> — ولا لون ولا صيغة مكتوبة داخل هذه الشاشة (RULE-N112 / RULE-N169).
/// </para>
/// </summary>
public partial class ctrlCustomerCard : UserControl
{
    /// <summary>قيمة بديلة لحقل نصّي غير مُدخل — نص عربي صريح (RULE-N28، قرار المالك D12).</summary>
    private const string MissingText = "غير مُدخل";

    /// <summary>بديل أي قيمة غائبة (المعرّف/الحالة) — شرطة عربية بدل رموز لاتينية (D12).</summary>
    private const string DashText = "—";

    /// <summary>السجل المعروض — <c>null</c> بعد <see cref="ResetCustomerInfo"/>.</summary>
    private CustomerDto? _customer;

    public ctrlCustomerCard()
    {
        InitializeComponent();

        // البطاقة عرض للقراءة فقط: لا حقول إدخال فيها، فلا تحتاج تنقّل Enter/الأسهم
        // (RULE-N40…RULE-N47 تخصّ شاشات الإدخال)، ولا اختصارات (WireGlobalShortcuts تعمل على Form فقط).
        ApplyUiTheme();
        ResetCustomerInfo();
    }

    // ── العقد المشترك لبطاقات الكيانات (F-002 — لا يُخترع لكل وحدة) ────────────

    // WFO1000: واجهة وقت تشغيل لا خاصية مصمّم — المصمّم لا يعرضها ولا يُسلسلها.
    /// <summary>معرّف العميل المعروض، أو <c>null</c> بعد التصفير.</summary>
    [Browsable(false)]
    public int? CustomerId => _customer?.Id;

    // WFO1000: واجهة وقت تشغيل لا خاصية مصمّم — المصمّم لا يعرضها ولا يُسلسلها.
    /// <summary>سجل العميل المعروض (نسخة الخدمة كما هي) — <c>null</c> بعد التصفير.</summary>
    [Browsable(false)]
    public CustomerDto? SelectedCustomerInfo => _customer;

    // WFO1000: واجهة وقت تشغيل لا خاصية مصمّم — المصمّم لا يعرضها ولا يُسلسلها.
    /// <summary>
    /// إظهار/إخفاء رابط «تعديل بيانات العميل». البطاقة المفردة في نموذج العرض تُبقيه ظاهراً
    /// (تُستهلك <see cref="EditRequested"/> هناك)، أما المنتقيات التي لا تُنفّذ التعديل فتُخفيه —
    /// فلا يبقى رابط ظاهر بلا فعل.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ShowEditLink
    {
        get => lnkEdit.Visible;
        set => lnkEdit.Visible = value;
    }

    /// <summary>
    /// يُطلقها رابط «تعديل بيانات العميل» — <b>الحاضن</b> يقرّر ماذا يفتح
    /// (نموذج العرض يفتح شاشة الدليل). البطاقة لا تفتح شيئاً بنفسها.
    /// </summary>
    public event EventHandler? EditRequested;

    /// <summary>يعرض سجلاً جاهزاً في البطاقة (بلا أي نداء خدمة).</summary>
    public void LoadData(CustomerDto dto)
    {
        if (dto is null)
        {
            ResetCustomerInfo();
            return;
        }

        _customer = dto;

        lblIdValue.Text = dto.Id > 0 ? clsFormat.Number(dto.Id) : DashText;
        lblNameValue.Text = _OrMissing(dto.Name);
        lblPhoneValue.Text = _OrMissing(dto.Phone);
        lblEmailValue.Text = _OrMissing(dto.Email);
        lblAddressValue.Text = _OrMissing(dto.Address);

        // المال يُعرض بمنسّق البيت الواحد دائماً (RULE-N169).
        lblCreditLimitValue.Text = clsFormat.Money(dto.CreditLimit);
        lblBalanceValue.Text = clsFormat.Money(dto.CurrentBalance);

        // ملاحظات: سطر واحد (يُقصّ بصرياً) والتلميح يعرض النص الكامل.
        string notes = _SingleLine(dto.Notes);
        lblNotesValue.Text = notes.Length == 0 ? MissingText : notes;
        toolTip1.SetToolTip(lblNotesValue, _SingleLine(dto.Notes));

        _ApplyStatusColors(dto);
        lnkEdit.Enabled = dto.Id > 0;
    }

    /// <summary>يُفرّغ البطاقة ويعيدها إلى حالتها المحايدة (بلا سجل).</summary>
    public void ResetCustomerInfo()
    {
        _customer = null;

        lblIdValue.Text = DashText;
        lblNameValue.Text = MissingText;
        lblPhoneValue.Text = MissingText;
        lblEmailValue.Text = MissingText;
        lblAddressValue.Text = MissingText;
        lblCreditLimitValue.Text = clsFormat.Money(0m);
        lblBalanceValue.Text = clsFormat.Money(0m);
        lblNotesValue.Text = MissingText;
        toolTip1.SetToolTip(lblNotesValue, string.Empty);

        _ApplyStatusColors(null);
        lnkEdit.Enabled = false;
    }

    // ── المظهر ────────────────────────────────────────────────────────────────

    /// <summary>الثيم (RULE-N50): آخر خطوة دائماً هي <c>GreenThemeHelper.Apply</c>.</summary>
    private void ApplyUiTheme()
    {
        _ApplyStatusColors(null);        // لون حالة وقت التشغيل فقط (لا يُعلَن في المصمّم)
        GreenThemeHelper.Apply(this);    // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>
    /// ألوان الحالة تُطبَّق <b>وقت التشغيل فقط</b> من ثوابت البيت الواحد
    /// (استثناء موثَّق على RULE-N134 لأنها ألوان حالة لا ألوان تصميم).
    /// </summary>
    private void _ApplyStatusColors(CustomerDto? dto)
    {
        if (dto is null)
        {
            lblStatusValue.Text = DashText;
            lblStatusValue.ForeColor = GreenThemeHelper.StatusMuted;

            lblCreditFlagValue.Text = DashText;
            lblCreditFlagValue.ForeColor = GreenThemeHelper.StatusNeutral;
            return;
        }

        lblStatusValue.Text = dto.IsActive ? "نشط" : "معطّل";
        lblStatusValue.ForeColor = dto.IsActive
            ? GreenThemeHelper.StatusSuccess
            : GreenThemeHelper.StatusMuted;

        lblCreditFlagValue.Text = dto.HasCreditLimit ? "نعم" : "لا";
        lblCreditFlagValue.ForeColor = GreenThemeHelper.StatusNeutral;
    }

    // ── المعالجات ─────────────────────────────────────────────────────────────

    /// <summary>الرابط لا يعمل على بطاقة فارغة — والحاضن وحده يقرّر ما يُفتح.</summary>
    private void lnkEdit_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        if (_customer is null) return;

        EditRequested?.Invoke(this, EventArgs.Empty);
    }

    // ── أدوات محلية صغيرة ─────────────────────────────────────────────────────

    private static string _OrMissing(string? value)
        => string.IsNullOrWhiteSpace(value) ? MissingText : value.Trim();

    /// <summary>يحوّل نصاً متعدد الأسطر إلى سطر واحد (البطاقة سطر واحد لكل قيمة).</summary>
    private static string _SingleLine(string? value)
        => string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Replace("\r", " ").Replace("\n", " ").Trim();
}
