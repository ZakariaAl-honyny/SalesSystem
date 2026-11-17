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
/// «بطاقة العميل + بحث» — عنصر قابل للتضمين (F-002).
/// <para>
/// <b>قرار المالك 2026-11-17:</b> صندوق بحث واحد باقتراحات (<see cref="ComboBox"/>) بدل
/// «تصفية بـ + حقل نص + زر بحث»: الكتابة تُقترح الأسماء/الهواتف، واختيار أي اقتراح يُحمّل
/// البطاقة فوراً — فلا خطوة «اضغط بحث» ولا حقلان متزامنان.
/// </para>
/// <para>
/// البيانات كلها من <c>ICustomerDesktopService</c> فقط، والبطاقة <see cref="ctrlCustomerCard"/>
/// لا تعرف أي خدمة. والقائمة تُحمَّل مرة واحدة (كسولة) وتُحدَّث بزر «تحديث القائمة» — فلا طلب
/// خدمة مع كل حرف يكتبه المستخدم.
/// </para>
/// </summary>
public partial class ctrlCustomerCardWithFilter : UserControl
{
    private const string NotAttachedMessage = "لم يتم تهيئة مكوّن البحث عن العميل. أعد فتح الشاشة.";

    private const string NoMatchMessage = "لا يوجد عميل مطابق";

    private const string EmptySearchMessage = "يرجى كتابة اسم العميل أو رقم هاتفه أولاً.";

    private const string ListLoadFailedMessage = "تعذر تحميل قائمة العملاء. يرجى إعادة المحاولة.";

    private IServiceProvider? _serviceProvider;

    /// <summary>اقتراحات صندوق البحث (اسم معروض + معرّف) — مرتَّبة بالاسم.</summary>
    private readonly List<CustomerPick> _picks = [];

    /// <summary>مقارنة فورية: المعرّف ← الاقتراح (بلا بحث خطّي).</summary>
    private readonly Dictionary<int, CustomerPick> _pickById = [];

    /// <summary>سجلات الخدمة كاملة (للاسم/الهاتف في المطابقة) — تُملأ مع القائمة بلا طلب إضافي.</summary>
    private readonly Dictionary<int, CustomerDto> _byId = [];

    /// <summary>هل حُمّلت القائمة مرة واحدة؟ (التحميل كسول — أول استخدام فقط).</summary>
    private bool _listLoaded;

    /// <summary>تحميل بطاقة جارٍ — يمنع تداخل تحميلين على البطاقة نفسها.</summary>
    private bool _isSearching;

    /// <summary>منع إعادة الدخول: نضبط الاختيار برمجياً فلا يجب أن يُطلق تحميلاً.</summary>
    private bool _syncingCombo;

    public ctrlCustomerCardWithFilter()
    {
        InitializeComponent();

        // المنتقي/العنصر المضمَّن لا يُنفّذ «تعديل بيانات العميل» ⇒ يُخفى الرابط بدل تركه بلا فعل
        // (نموذج العرض وحده يعرضه ويستهلك EditRequested).
        ctrlCustomerCard1.ShowEditLink = false;

        // عناصر مصدر الاقتراح: سجلات مُسمّاة، والعرض من خاصية Display لا من نص حر (RULE-N60).
        cbCustomer.DisplayMember = nameof(CustomerPick.Display);

        // RULE-N40…RULE-N47 — النسخة ثنائية الوسيط تُعلن Enter مفتاحَ إدخال (RULE-N193) وتحترم
        // e.Handled ⇒ Enter في صندوق البحث يُنفّذ بحثنا ولا ينقل التركيز ولا ينفّذ AcceptButton.
        KeyboardHelper.WireEnterAsTab(gbFilters, () => btnReload);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);
        KeyboardHelper.WireButtonDisabledVisual(btnReload, btnAddNew);   // RULE-N43

        // WireGlobalShortcuts غير منطبقة على UserControl (توقيعها Form) — الاختصارات في النماذج الحاضنة.

        ApplyUiTheme();
    }

    // ── العقد المشترك لبطاقات الكيانات (F-002 — لا يُخترع لكل وحدة) ────────────

    /// <summary>يُطلق بعد كل عرض/تصفير — يُقرأ منه العميل المختار (وعنصر <c>null</c> عند التصفير).</summary>
    public event EventHandler<CustomerDto?>? CustomerSelected;

    /// <summary>
    /// تعطيل/تفعيل صندوق البحث كاملاً (التسمية + الصندوق + زرّيه). التعطيل <b>لا يُصفّر</b>
    /// البطاقة (نمط المرجع: يُقفل بعد الاختيار فيبقى السجل معروضاً).
    /// </summary>
    // WFO1000: واجهة وقت تشغيل لا خاصية مصمّم — المصمّم لا يعرضها ولا يُسلسلها.
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

    /// <summary>تحميل بطاقة عميل بمعرّفه (مع مزامنة صندوق البحث إن كان السجل في القائمة).</summary>
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
        try
        {
            await _EnsureListAsync(ct);   // تملك مؤشر الانتظار الخاص بها

            Cursor = Cursors.WaitCursor;
            var result = await Service.GetByIdAsync(id, ct);

            if (!result.IsSuccess || result.Value is null)
            {
                _ClearSelection(NoMatchMessage);
                return;
            }

            _Show(result.Value, syncCombo: true);
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

    /// <summary>
    /// البحث داخل القائمة المحمّلة بالاسم أو الهاتف (بلا طلب قائمة جديد)، ثم طلب واحد
    /// للسجل عبر <c>GetByIdAsync</c> — فالبيانات المعروضة هي بيانات الخدمة لا نص القائمة.
    /// </summary>
    public async Task LoadCustomerAsync(string name, CancellationToken ct = default)
    {
        string typed = name?.Trim() ?? string.Empty;

        if (typed.Length == 0)
        {
            _ClearSelection(EmptySearchMessage);
            return;
        }

        try
        {
            if (!_EnsureAttached()) return;

            if (!await _EnsureListAsync(ct))
            {
                _ClearSelection(ListLoadFailedMessage);
                return;
            }

            var pick = _ResolveTyped(typed);
            if (pick is null)
            {
                _ClearSelection(NoMatchMessage);
                return;
            }

            await LoadCustomerAsync(pick.Id, ct);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء البحث عن عميل بالاسم {Name}", typed);
            _ClearSelection("تعذر البحث عن العميل. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>يُفرّغ البطاقة (بلا بحث وبلا رسالة) — يُستدعى عند تغيير السياق في الحاضن.</summary>
    public void ResetCustomerInfo() => ctrlCustomerCard1.ResetCustomerInfo();

    /// <summary>يضع المؤشر في صندوق البحث ويحدد نصه (نمط المرجع: بعد الإضافة أو عند بدء البحث).</summary>
    public void FilterFocus()
    {
        if (!gbFilters.Enabled) return;

        cbCustomer.Focus();
        cbCustomer.SelectAll();
    }

    // ── المظهر ────────────────────────────────────────────────────────────────

    /// <summary>الثيم (RULE-N50): آخر خطوة دائماً هي <c>GreenThemeHelper.Apply</c>.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ── قائمة الاقتراحات ──────────────────────────────────────────────────────

    /// <summary>
    /// تحميل القائمة مرة واحدة (كسول). تُعيد <c>false</c> عند فشل القراءة — والمتصل هو من
    /// يُظهر الرسالة العربية، فالدالة نفسها بلا <c>catch</c> (RULE-N205: الحدّ عند المستدعي).
    /// </summary>
    private async Task<bool> _EnsureListAsync(CancellationToken ct)
    {
        if (_listLoaded) return true;
        if (_serviceProvider is null) return false;

        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await Service.GetAllAsync(search: null, includeInactive: true, ct);

            if (!result.IsSuccess || result.Value is null) return false;

            _picks.Clear();
            _pickById.Clear();
            _byId.Clear();

            foreach (var customer in result.Value.OrderBy(c => c.Name, StringComparer.CurrentCultureIgnoreCase))
            {
                var pick = new CustomerPick(customer.Id, _BuildDisplay(customer));

                _picks.Add(pick);
                _pickById[customer.Id] = pick;
                _byId[customer.Id] = customer;
            }

            _listLoaded = true;

            _BindCombo((cbCustomer.SelectedItem as CustomerPick)?.Id);
            return true;
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>يملأ الصندوق بالاقتراحات ويُعيد تحديد العميل الحالي إن بقي في القائمة.</summary>
    private void _BindCombo(int? selectedId)
    {
        _syncingCombo = true;
        try
        {
            cbCustomer.BeginUpdate();
            cbCustomer.Items.Clear();
            cbCustomer.Items.AddRange(_picks.Cast<object>().ToArray());
            cbCustomer.EndUpdate();

            if (selectedId is int id && _pickById.TryGetValue(id, out var pick))
                cbCustomer.SelectedItem = pick;
            else
                cbCustomer.SelectedIndex = -1;
        }
        finally
        {
            _syncingCombo = false;
        }
    }

    /// <summary>
    /// مزامنة الصندوق مع سجل معروض بلا إعادة دخول: موجود في القائمة ⇒ يُحدَّد؛ غير موجود
    /// (سجل معطّل/جديد لم تُحدَّث القائمة) ⇒ يُعرض نصه بلا اختيار فلا تظهر قائمة فارغة.
    /// </summary>
    private void _SyncComboTo(CustomerDto customer)
    {
        _syncingCombo = true;
        try
        {
            if (_pickById.TryGetValue(customer.Id, out var pick))
            {
                if (!ReferenceEquals(cbCustomer.SelectedItem, pick)) cbCustomer.SelectedItem = pick;
                return;
            }

            cbCustomer.SelectedIndex = -1;
            cbCustomer.Text = _BuildDisplay(customer);
        }
        finally
        {
            _syncingCombo = false;
        }
    }

    /// <summary>
    /// مطابقة النص المكتوب بالترتيب: العرض الكامل ← الاسم بالضبط ← الهاتف بالضبط ←
    /// أول اقتراح يبدأ بالنص (اقتراح لا يطابق يبقى «لا يوجد عميل مطابق»).
    /// </summary>
    private CustomerPick? _ResolveTyped(string typed)
    {
        var byDisplay = _picks.FirstOrDefault(p =>
            string.Equals(p.Display, typed, StringComparison.CurrentCultureIgnoreCase));

        if (byDisplay is not null) return byDisplay;

        var byName = _picks.FirstOrDefault(p =>
            _byId.TryGetValue(p.Id, out var customer) &&
            string.Equals(customer.Name?.Trim(), typed, StringComparison.CurrentCultureIgnoreCase));

        if (byName is not null) return byName;

        var byPhone = _picks.FirstOrDefault(p =>
            _byId.TryGetValue(p.Id, out var customer) &&
            string.Equals(customer.Phone?.Trim(), typed, StringComparison.OrdinalIgnoreCase));

        if (byPhone is not null) return byPhone;

        return _picks.FirstOrDefault(p =>
            p.Display.StartsWith(typed, StringComparison.CurrentCultureIgnoreCase));
    }

    /// <summary>
    /// نص الاقتراح: «الاسم — الهاتف» وينحدر بنظافة عند غياب أحدهما (ولا يظهر «— —» أبداً).
    /// </summary>
    private static string _BuildDisplay(CustomerDto customer)
    {
        string name = customer.Name?.Trim() ?? string.Empty;
        string phone = customer.Phone?.Trim() ?? string.Empty;

        if (name.Length == 0 && phone.Length == 0) return "عميل #" + customer.Id;
        if (phone.Length == 0) return name;
        if (name.Length == 0) return phone;

        return $"{name} — {phone}";
    }

    // ── المعالجات ─────────────────────────────────────────────────────────────

    /// <summary>اختيار اقتراح = تحميل فوري للبطاقة (بلا زر بحث) — والمزامنة البرمجية لا تُعيد الدخول.</summary>
    private void cbCustomer_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_syncingCombo || _isSearching) return;
        if (cbCustomer.SelectedItem is not CustomerPick pick) return;

        // حدّ الخطأ موجود داخل LoadCustomerAsync (RULE-N205) ⇒ لا حارس ثانٍ هنا.
        _ = LoadCustomerAsync(pick.Id);
    }

    /// <summary>
    /// Enter في صندوق البحث = ابحث عن النص المكتوب (مثل RULE-N42/RULE-N59)، ويُعلَن كمُعالَج
    /// فلا ينقل التركيز ولا ينفّذ <c>AcceptButton</c> للحاضن (PILOT-13 / RULE-N153).
    /// </summary>
    private void cbCustomer_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        _ = _SearchTypedAsync();
    }

    /// <summary>«تحديث القائمة»: يُفرّغ القائمة ويُعيد تحميلها ويُعيد تحديد العميل الحالي إن بقي.</summary>
    private async void btnReload_Click(object? sender, EventArgs e)
    {
        if (!_EnsureAttached()) return;

        try
        {
            int? currentId = (cbCustomer.SelectedItem as CustomerPick)?.Id;

            _listLoaded = false;
            _BindCombo(null);

            if (!await _EnsureListAsync(CancellationToken.None))
            {
                clsDialogs.Error(ListLoadFailedMessage);
                return;
            }

            _BindCombo(currentId);

            // السجل السابق لم يعد في القائمة (حُذف) ⇒ لا تُترك بطاقة معلّقة على سجل غير موجود.
            if (currentId is int id && !_pickById.ContainsKey(id)) _ClearSelection(null);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث قائمة العملاء في مكوّن البحث");
            clsDialogs.Error("تعذر تحديث قائمة العملاء. يرجى إعادة المحاولة.");
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

    /// <summary>
    /// بحث Enter على النص المكتوب — يُستدعى بلا انتظار من معالج متزامن ⇒ <b>هو</b> حدّ الخطأ
    /// (RULE-N205). نجاحه يُطلق مساراً واحداً: تحديد الاقتراح ⇒ <c>SelectedIndexChanged</c> ⇒ تحميل.
    /// </summary>
    private async Task _SearchTypedAsync()
    {
        try
        {
            if (!_EnsureAttached()) return;

            string typed = cbCustomer.Text.Trim();

            if (typed.Length == 0)
            {
                _ClearSelection(EmptySearchMessage);
                return;
            }

            if (!await _EnsureListAsync(CancellationToken.None))
            {
                _ClearSelection(ListLoadFailedMessage);
                return;
            }

            var pick = _ResolveTyped(typed);

            if (pick is null)
            {
                _ClearSelection(NoMatchMessage);
                return;
            }

            if (ReferenceEquals(cbCustomer.SelectedItem, pick))
                await LoadCustomerAsync(pick.Id);   // مُحدَّد أصلاً ⇒ لا حدث تغيير ليُعتمد عليه
            else
                cbCustomer.SelectedItem = pick;     // يُطلق المسار الواحد
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء البحث عن عميل من مربع الاقتراح");
            clsDialogs.Error("تعذر تنفيذ البحث. يرجى المحاولة مرة أخرى.");
        }
    }

    /// <summary>يعرض سجلاً ويُبلّغ الحاضن — الإبلاغ مقصور على وضع التصفية المفعّلة (نمط المرجع).</summary>
    private void _Show(CustomerDto customer, bool syncCombo)
    {
        if (syncCombo) _SyncComboTo(customer);

        ctrlCustomerCard1.LoadData(customer);

        if (FilterEnabled) CustomerSelected?.Invoke(this, customer);
    }

    /// <summary>يُصفّر البطاقة ويُبلّغ الحاضن بالتصفير (حتى يُعطّل زر «اختيار» في المنتقي).</summary>
    private void _ClearSelection(string? message)
    {
        ctrlCustomerCard1.ResetCustomerInfo();

        _syncingCombo = true;
        try
        {
            cbCustomer.SelectedIndex = -1;
            cbCustomer.Text = string.Empty;
        }
        finally
        {
            _syncingCombo = false;
        }

        if (!string.IsNullOrWhiteSpace(message)) clsDialogs.Warning(message);

        if (FilterEnabled) CustomerSelected?.Invoke(this, null);
    }

    // ── عناصر داخلية ──────────────────────────────────────────────────────────

    /// <summary>اقتراح واحد في صندوق البحث: معرّف + نص معروض (DisplayMember).</summary>
    private sealed record CustomerPick(int Id, string Display);
}
