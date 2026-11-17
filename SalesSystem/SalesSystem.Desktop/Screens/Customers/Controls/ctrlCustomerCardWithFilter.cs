using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Screens.Customers.Forms;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using Serilog;

namespace SalesSystem.Desktop.Screens.Customers.Controls;

/// <summary>
/// «بطاقة العميل + تصفية» — عنصر قابل للتضمين (F-002).
/// <para>
/// يبحث عن العميل بثلاثة طرق (المعرّف · الاسم · الهاتف) عبر <c>ICustomerDesktopService</c> فقط،
/// ويعرض النتيجة داخل <see cref="ctrlCustomerCard"/> التي لا تعرف أي خدمة.
/// </para>
/// <para>
/// <b>الاستخدام:</b> استدعِ <see cref="Attach"/> أولاً (مثل أي عنصر يُحلّ منه شيء من DI)،
/// ثم <see cref="LoadCustomerAsync(int,CancellationToken)"/> أو <see cref="LoadCustomerAsync(string,CancellationToken)"/>،
/// واستمع إلى <see cref="CustomerSelected"/>. إن لم يُستدعَ <see cref="Attach"/> يبقى العنصر
/// <b>خامداً</b> ويُبلّغ برسالة عربية بدل رمي استثناء (RULE-N28).
/// </para>
/// </summary>
public partial class ctrlCustomerCardWithFilter : UserControl
{
    private const string NotAttachedMessage = "لم يتم تهيئة مكوّن البحث عن العميل. أعد فتح الشاشة.";

    private const string NoMatchMessage = "لا يوجد عميل مطابق";

    private IServiceProvider? _serviceProvider;

    /// <summary>يمنع بحثين متوازيين على نفس البطاقة (نتيجتان تكتبان فوق بعضهما).</summary>
    private bool _isSearching;

    public ctrlCustomerCardWithFilter()
    {
        InitializeComponent();

        // المنتقي/العنصر المضمَّن لا يُنفّذ «تعديل بيانات العميل» ⇒ يُخفى الرابط بدل تركه بلا فعل
        // (نموذج العرض وحده يعرضه ويستهلك EditRequested).
        ctrlCustomerCard1.ShowEditLink = false;

        // القائمة تُملأ بعناصر مُسمّاة: المنطق يقرأ العنصر لا النص العربي (RULE-N60/RULE-N24).
        cbFilterBy.DisplayMember = nameof(FilterOption.Text);
        cbFilterBy.Items.AddRange(new object[]
        {
            new FilterOption("المعرّف", CustomerFilterKind.Id),
            new FilterOption("الاسم", CustomerFilterKind.Name),
            new FilterOption("الهاتف", CustomerFilterKind.Phone)
        });
        cbFilterBy.SelectedIndex = 0;

        // RULE-N40…RULE-N47 — تنقّل وتحديد وتظليل الحقل النشط وأسهم RTL.
        // ملاحظة مقيسة (PILOT-13 / RULE-N193): النسخة ثنائية الوسيط هي التي تُعلن Enter مفتاح إدخال
        // فيصل إلى KeyDown قبل أن يبتلعه AcceptButton، وتحترم e.Handled فلا تنقل التركيز مع بحث Enter.
        KeyboardHelper.WireEnterAsTab(gbFilters, () => btnFind);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);
        KeyboardHelper.WireButtonDisabledVisual(btnFind, btnAddNew);   // RULE-N43

        // WireGlobalShortcuts غير منطبقة على UserControl (توقيعها Form) — الاختصارات في النماذج الحاضنة.

        ApplyUiTheme();
    }

    // ── العقد المشترك لبطاقات الكيانات (F-002 — لا يُخترع لكل وحدة) ────────────

    /// <summary>يُطلق بعد كل عرض/تصفير — يُقرأ منه العميل المختار (وعنصر <c>null</c> عند التصفير).</summary>
    public event EventHandler<CustomerDto?>? CustomerSelected;

    // WFO1000: واجهة وقت تشغيل لا خاصية مصمّم — المصمّم لا يعرضها ولا يُسلسلها.
    /// <summary>
    /// تعطيل/تفعيل صندوق التصفية كاملاً. التعطيل <b>لا يُصفّر</b> البطاقة (نمط المرجع:
    /// يُقفل بعد الاختيار فيبقى السجل معروضاً).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool FilterEnabled
    {
        get => gbFilters.Enabled;
        set => gbFilters.Enabled = value;
    }

    // WFO1000: واجهة وقت تشغيل لا خاصية مصمّم — المصمّم لا يعرضها ولا يُسلسلها.
    /// <summary>إظهار/إخفاء زر «جديد» (منتقيات لا تُنشئ سجلات تخفيه).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ShowAddNew
    {
        get => btnAddNew.Visible;
        set => btnAddNew.Visible = value;
    }

    /// <summary>يربط العنصر بحاوية الخدمات — إلزامي قبل أي بحث.</summary>
    public void Attach(IServiceProvider serviceProvider)
    {
        if (serviceProvider is null)
        {
            clsDialogs.Warning(NotAttachedMessage);
            return;
        }

        _serviceProvider = serviceProvider;
    }

    /// <summary>تحميل بطاقة عميل بمعرّفه عبر <c>GetByIdAsync</c>.</summary>
    public async Task LoadCustomerAsync(int id, CancellationToken ct = default)
    {
        if (!_EnsureAttached()) return;

        if (id <= 0)
        {
            _ClearSelection("يرجى إدخال معرّف عميل صحيح.");
            return;
        }

        if (_isSearching) return;

        _isSearching = true;
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await Service.GetByIdAsync(id, ct);

            if (!result.IsSuccess || result.Value is null)
            {
                _ClearSelection(NoMatchMessage);
                return;
            }

            _Show(result.Value);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل بطاقة العميل {CustomerId}", id);
            _ClearSelection("تعذر تحميل بطاقة العميل. يرجى إعادة المحاولة.");
        }
        finally
        {
            _isSearching = false;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>البحث عن عميل بالاسم — <b>حدُّ الخطأ</b> لواجهة العنصر العامة (RULE-N14/RULE-N25).</summary>
    public async Task LoadCustomerAsync(string name, CancellationToken ct = default)
    {
        string value = name?.Trim() ?? string.Empty;

        if (value.Length == 0)
        {
            _ClearSelection("يرجى إدخال اسم العميل أولاً.");
            return;
        }

        try
        {
            await _FindByNameOrPhoneAsync(CustomerFilterKind.Name, value, ct);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء البحث عن عميل بالاسم {Name}", value);
            _ClearSelection("تعذر البحث عن العميل. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>يُفرّغ البطاقة (بلا بحث وبلا رسالة) — يُستدعى عند تغيير السياق في الحاضن.</summary>
    public void ResetCustomerInfo() => ctrlCustomerCard1.ResetCustomerInfo();

    /// <summary>يضع المؤشر في حقل التصفية ويحدد قيمته (نمط المرجع: بعد الإضافة أو عند بدء البحث).</summary>
    public void FilterFocus()
    {
        if (!gbFilters.Enabled) return;

        txtFilterValue.Focus();
        txtFilterValue.SelectAll();
    }

    // ── المظهر ────────────────────────────────────────────────────────────────

    /// <summary>الثيم (RULE-N50): آخر خطوة دائماً هي <c>GreenThemeHelper.Apply</c>.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ── البحث ─────────────────────────────────────────────────────────────────

    /// <summary>«بحث» — يقرأ نوع التصفية من العنصر المحدد لا من نصه.</summary>
    private void btnFind_Click(object? sender, EventArgs e) => _ = _FindFromFilterAsync();

    /// <summary>
    /// Enter داخل حقل التصفية = بحث فوري (مثل RULE-N42/RULE-N59).
    /// يُعلَن كمُعالَج قبل مُعالج <c>KeyboardHelper</c> (اشتراك المصمّم أسبق)، والـHelper يحترم
    /// <c>e.Handled</c> فلا ينقل التركيز — كما يمنع <c>SuppressKeyPress</c> تنفيذ
    /// <c>AcceptButton</c> للنموذج الحاضن (PILOT-13 / RULE-N153 / RULE-N193).
    /// </summary>
    private void txtFilterValue_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        _ = _FindFromFilterAsync();
    }

    /// <summary>تغيير نوع التصفية يُفرّغ القيمة (لا يبقى معرّف في حقل بحث بالاسم).</summary>
    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => txtFilterValue.Clear();

    /// <summary>
    /// مسار البحث الواحد من صندوق التصفية (زر «بحث» أو <c>Enter</c>).
    /// يُستدعى بلا انتظار من معالجَين (<c>_ = …</c>) ⇒ جسده كله داخل <c>try/catch</c>
    /// فلا يفلت استثناء من مهمة غير مراقَبة (RULE-N25/RULE-N29).
    /// </summary>
    private async Task _FindFromFilterAsync()
    {
        try
        {
            if (!_EnsureAttached()) return;

            CustomerFilterKind kind = _SelectedFilterKind();
            string value = txtFilterValue.Text.Trim();

            if (value.Length == 0)
            {
                _ClearSelection("يرجى إدخال قيمة البحث أولاً.");
                FilterFocus();
                return;
            }

            if (kind == CustomerFilterKind.Id)
            {
                // غير الرقمي = رفض عربي واضح بلا استثناء ولا بحث عشوائي.
                if (!int.TryParse(value, out int id))
                {
                    _ClearSelection("معرّف العميل يجب أن يكون رقماً.");
                    FilterFocus();
                    return;
                }

                await LoadCustomerAsync(id);
                return;
            }

            await _FindByNameOrPhoneAsync(kind, value, CancellationToken.None);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء البحث عن عميل من صندوق التصفية");
            clsDialogs.Error("تعذر تنفيذ البحث. يرجى المحاولة مرة أخرى.");
        }
    }

    /// <summary>
    /// البحث بالاسم/الهاتف: يُطلب من الخدمة ما يطابق القيمة، ثم يُفضَّل <b>المطابق تماماً</b>
    /// (تجاهل حالة الأحرف والمسافات)، وإلا أول نتيجة — وإن لم توجد نتيجة رُفض البحث برسالة عربية.
    /// <para>
    /// <b>بلا <c>catch</c> بقرار المالك (2026-11-17):</b> دالة داخلية يناديها حدّان يملكان معالجة الخطأ
    /// (<see cref="LoadCustomerAsync(string, CancellationToken)"/> و<c>_FindFromFilterAsync</c>) ⇒
    /// الاستثناء يصعد إليهما، و<see cref="finally"/> يبقى لأن إعادة ضبط <c>_isSearching</c> والمؤشر واجب لا يُتجاوَز.
    /// </para>
    /// </summary>
    private async Task _FindByNameOrPhoneAsync(
        CustomerFilterKind kind, string value, CancellationToken ct)
    {
        if (_isSearching) return;

        _isSearching = true;
        Cursor = Cursors.WaitCursor;
        try
        {
            // includeInactive: البحث في القائمة الكاملة — البطاقة تُظهر «معطّل» بدل إخفاء السجل.
            var result = await Service.GetAllAsync(value, includeInactive: true, ct);

            if (!result.IsSuccess || result.Value is null)
            {
                _ClearSelection(result.Error ?? "تعذر البحث عن العميل. يرجى إعادة المحاولة.");
                return;
            }

            if (result.Value.Count == 0)
            {
                _ClearSelection(NoMatchMessage);
                return;
            }

            var exact = result.Value.FirstOrDefault(c => _IsExactMatch(c, kind, value));
            _Show(exact ?? result.Value[0]);
        }
        finally
        {
            _isSearching = false;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>زر «جديد»: يفتح دليل العملاء من DI عبر البيت الواحد لفتح النماذج (RULE-N132).</summary>
    private void btnAddNew_Click(object? sender, EventArgs e)
    {
        if (!_EnsureAttached()) return;

        try
        {
            var master = _serviceProvider!.GetRequiredService<frmCustomerMaster>();
            clsGlobal._OpenForm(master);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء فتح دليل العملاء من بطاقة العميل");
            clsDialogs.Error("تعذر فتح دليل العملاء. يرجى إعادة المحاولة.");
        }
    }

    // ── الحالة الداخلية ───────────────────────────────────────────────────────

    private ICustomerDesktopService Service
        => _serviceProvider!.GetRequiredService<ICustomerDesktopService>();

    private bool _EnsureAttached()
    {
        if (_serviceProvider is not null) return true;

        clsDialogs.Warning(NotAttachedMessage);
        return false;
    }

    private CustomerFilterKind _SelectedFilterKind()
        => cbFilterBy.SelectedItem is FilterOption option ? option.Kind : CustomerFilterKind.Name;

    /// <summary>يعرض سجلاً ويُبلّغ الحاضن — الإبلاغ مقصور على وضع التصفية المفعّلة (نمط المرجع).</summary>
    private void _Show(CustomerDto dto)
    {
        ctrlCustomerCard1.LoadData(dto);

        if (FilterEnabled) CustomerSelected?.Invoke(this, dto);
    }

    /// <summary>يُصفّر البطاقة ويُبلّغ الحاضن بالتصفير (حتى يُعطّل زر «اختيار» في المنتقي).</summary>
    private void _ClearSelection(string? message)
    {
        ctrlCustomerCard1.ResetCustomerInfo();

        if (!string.IsNullOrWhiteSpace(message)) clsDialogs.Warning(message);

        if (FilterEnabled) CustomerSelected?.Invoke(this, null);
    }

    private static bool _IsExactMatch(CustomerDto customer, CustomerFilterKind kind, string value)
        => kind == CustomerFilterKind.Phone
            ? string.Equals(customer.Phone?.Trim(), value, StringComparison.OrdinalIgnoreCase)
            : string.Equals(customer.Name?.Trim(), value, StringComparison.OrdinalIgnoreCase);

    // ── عناصر داخلية ──────────────────────────────────────────────────────────

    /// <summary>خيار «تصفية بـ» — نص معروض + نوع يُقرأ في المنطق (لا مقارنة نص عربي).</summary>
    private sealed class FilterOption
    {
        public FilterOption(string text, CustomerFilterKind kind)
        {
            Text = text;
            Kind = kind;
        }

        public string Text { get; }

        public CustomerFilterKind Kind { get; }

        public override string ToString() => Text;
    }

    private enum CustomerFilterKind
    {
        Id,
        Name,
        Phone
    }
}
