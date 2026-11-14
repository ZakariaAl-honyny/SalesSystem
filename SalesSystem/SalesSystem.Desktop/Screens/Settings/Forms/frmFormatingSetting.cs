using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Settings.Forms;

/// <summary>
/// شاشة إعدادات التنسيق (المرحلة أ — Mock · UI &amp; UX QA).
/// <para>
/// شاشة إعدادات واحدة يجمع فيها المالك **كل** تنسيقات الأرقام والتواريخ (10 إعدادات) في مكان
/// واحد، مع معاينة فورية تُبنى من المنسّق الحقيقي نفسه قبل الحفظ.
/// </para>
/// <para>
/// المصدر الوحيد للصفوف والتسميات والمجموعات والافتراضيات هو <see cref="clsFormat.Definitions"/>،
/// والمصدر الوحيد للتنسيق والتحقق هو <see cref="clsFormat"/> — فلا يُكتب مفتاح ولا صيغة ولا
/// قاعدة تحقق داخل الشاشة (<c>RULE-N169</c>…<c>RULE-N172</c>).
/// </para>
/// <para>
/// لا تحفظ الشاشة في أي قاعدة بيانات ولا تمسّ <c>SystemSettings</c>: تخاطب
/// <see cref="IFormatSettingsDesktopService"/> فقط (المرحلة أ: <c>MockFormatSettingsDesktopService</c>)،
/// ولا تُطبَّق القيم على بقية الشاشات إلا بعد نجاح «حفظ وتطبيق» — والمعاينة داخلها تعمل على
/// <b>مسودّة</b> غير محفوظة تُمرَّر لمنسّقات <see cref="clsFormat"/> كوسيط <c>values</c>.
/// </para>
/// </summary>
public partial class frmFormatingSetting : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IFormatSettingsDesktopService DesktopService
        => _serviceProvider.GetRequiredService<IFormatSettingsDesktopService>();

    // ── نصوص ثابتة ────────────────────────────────────────────────────────────
    private const string ModeSaved = "إعدادات التنسيق محفوظة";
    private const string ModeDirty = "إعدادات التنسيق — تغييرات غير محفوظة";

    // ── عيّنات المعاينة (القيم فقط — الصيغة تأتي من clsFormat حصراً) ──────────
    private const decimal PreviewAmount = 1234567.891m;
    private const decimal PreviewPercent = 12.345m;

    private static readonly DateTime PreviewDate = new(2026, 11, 14);
    private static readonly DateTime PreviewDateTime = new(2026, 11, 14, 15, 30, 0);

    // ── State ────────────────────────────────────────────────────────────────
    /// <summary>قيم الشاشة الحالية (مسودّة غير محفوظة بعد).</summary>
    private readonly Dictionary<string, string> _draft = new(StringComparer.Ordinal);

    /// <summary>القيم كما هي في المخزن — أساس حساب «المعدّلة».</summary>
    private readonly Dictionary<string, string> _baseline = new(StringComparer.Ordinal);

    /// <summary>يمنع إعادة الدخول إلى تحديث المسودّة/المعاينة أثناء البناء البرمجي للعناصر.</summary>
    private bool _isBinding;

    /// <summary>يمنع تكرار الحفظ/الاستعادة من نقرة ثانية أو <c>Enter</c> أثناء تنفيذ العملية (RULE-N25).</summary>
    private bool _isBusy;

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmFormatingSetting()
    {
        InitializeComponent();
    }

    public frmFormatingSetting(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط (اللون الحيّ من اللوحة القابلة للتخصيص)
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر التعطيل الموحّد لأزرار الحالة
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnReset);

        // RULE-N47 — الاختصارات الموحّدة: F5 تحديث · Esc إغلاق
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F5, () => { _ = _LoadAsync(); }),
            (Keys.Escape, Close));

        // RULE-N147 — Enter = الزر الأساسي (خارج حقول الإدخال: Enter هناك = الحقل التالي)
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // تلميح أمثلة التواريخ من تعريفات clsFormat وحدها (لا صيغة مكتوبة داخل الشاشة)
        _BindDateHint();

        ApplyUiTheme();
    }

    // ── Theme (RULE-N50…RULE-N54) ─────────────────────────────────────────────

    /// <summary>يُطبَّق مرة واحدة في المُنشئ، وينتهي بـ <c>GreenThemeHelper.Apply(this)</c> (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);

        ThemeHelper.ApplyButtonStyle(btnSave, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnReset, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        GreenThemeHelper.ApplyHeader(lblMode);

        // عناوين صناديق المجموعات + التسميات + الشرائط — لون النص المميّز من الـHelper
        gbNumbers.ForeColor = GreenThemeHelper.TextStrong;
        gbDates.ForeColor = GreenThemeHelper.TextStrong;
        gbPreview.ForeColor = GreenThemeHelper.TextStrong;

        lblMoneyDecimals.ForeColor = GreenThemeHelper.TextStrong;
        lblQuantityDecimals.ForeColor = GreenThemeHelper.TextStrong;
        lblNumberDecimals.ForeColor = GreenThemeHelper.TextStrong;
        lblPercentDecimals.ForeColor = GreenThemeHelper.TextStrong;
        lblThousandsSeparator.ForeColor = GreenThemeHelper.TextStrong;
        lblArabicIndicDigits.ForeColor = GreenThemeHelper.TextStrong;
        lblDecimalSeparator.ForeColor = GreenThemeHelper.TextStrong;

        // أزرار الاختيار تجلس على لوحة ملوّنة، فيُثبَّت نصّها بلون النص المميّز (RULE-N54)
        rdoThousandsYes.ForeColor = GreenThemeHelper.TextStrong;
        rdoThousandsNo.ForeColor = GreenThemeHelper.TextStrong;
        rdoArabicYes.ForeColor = GreenThemeHelper.TextStrong;
        rdoArabicNo.ForeColor = GreenThemeHelper.TextStrong;

        lblDateShortPattern.ForeColor = GreenThemeHelper.TextStrong;
        lblDateDayMonthYearPattern.ForeColor = GreenThemeHelper.TextStrong;
        lblDateLongPattern.ForeColor = GreenThemeHelper.TextStrong;
        lblDateHint.ForeColor = GreenThemeHelper.TextStrong;

        lblPreviewMoneyCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewQuantityCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewNumberCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewPercentCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewDateShortCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewDateDMYCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewDateLongCaption.ForeColor = GreenThemeHelper.TextStrong;

        lblPreviewMoney.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewQuantity.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewNumber.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewPercent.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewDateShort.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewDateDMY.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewDateLong.ForeColor = GreenThemeHelper.TextStrong;

        lblPreviewSample.ForeColor = GreenThemeHelper.TextStrong;
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;
        lblFooterHint.ForeColor = GreenThemeHelper.TextStrong;

        // رسالة الرفض بلون الخطر — المفتاح من اللوحة وليس لوناً مكتوباً هنا (RULE-N54)
        lblValidation.ForeColor = ThemePalette.Get(ThemePalette.BtnDeleteButtonColor);

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>
    /// تلميح الأمثلة المقبولة يُبنى من افتراضيات تعريفات <c>clsFormat</c> وحدها — فلا تُكتب
    /// صيغة تاريخ داخل الشاشة (RULE-N169)، و«المصمّم = التشغيل» يبقى صحيحاً لأن التلميح شرح لا إعداد.
    /// </summary>
    private void _BindDateHint()
    {
        try
        {
            var dates = clsFormat.Definitions
                .Where(definition => string.Equals(definition.GroupLabel, clsFormat.DatesGroup, StringComparison.Ordinal))
                .Select(definition => definition.DefaultValue);

            lblDateHint.Text = "أمثلة مقبولة: " + string.Join(" · ", dates);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر بناء تلميح أمثلة صيغ التاريخ");
        }
    }

    // ── Load / Reload ─────────────────────────────────────────────────────────

    private async void frmFormatingSetting_Load(object? sender, EventArgs e)
    {
        try
        {
            await _LoadAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة إعدادات التنسيق");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// يقرأ الإعدادات من المخزن ويملأ المسودّة والأساس ويوزّع القيم على العناصر ثم يحدّث
    /// المعاينة والتحقق والحالة والملخص. لا يرمي استثناءً (كل المسارات محمية) فيصلح لـ«تحديث»/F5.
    /// </summary>
    private async Task _LoadAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await DesktopService.GetAsync();
            var settings = result.Value;

            if (!result.IsSuccess || settings is null)
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل إعدادات التنسيق.");
                return;
            }

            _baseline.Clear();
            _draft.Clear();

            // التعريفات هي المصدر الوحيد لقائمة المفاتيح — لا قائمة مفاتيح ثانية هنا.
            foreach (var definition in clsFormat.Definitions)
            {
                string value = settings.Values is not null
                    && settings.Values.TryGetValue(definition.Key, out var stored)
                    && !string.IsNullOrWhiteSpace(stored)
                        ? stored.Trim()
                        : definition.DefaultValue;

                _baseline[definition.Key] = value;
                _draft[definition.Key] = value;
            }

            _BindControls();
            _UpdatePreview();
            _ValidateDraft();
            _UpdateMode();
            _UpdateSummary();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحميل إعدادات التنسيق");
            clsDialogs.Error("تعذر تحميل إعدادات التنسيق. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// يوزّع قيم المسودّة على العناصر **بالمفتاح** — تعيين واحد لكل عنصر وبلا أي منطق تنسيق
    /// مكرّر هنا. تُستدعى داخل <c>_isBinding</c> حتى لا تُعيد التوزيعات تشغيل حلقة التحديث.
    /// <para>
    /// القيمة التي تُعرض فعلاً هي المرجع: تُعاد إلى <c>_draft</c> بعد كل تعيين، فلا تختلف
    /// «القيمة المعروضة» عن «القيمة التي يحفظها الزر» عند أي مطابقة فاشلة (RULE-N167).
    /// </para>
    /// </summary>
    private void _BindControls()
    {
        _isBinding = true;
        try
        {
            foreach (var definition in clsFormat.Definitions)
            {
                string value = _draft.TryGetValue(definition.Key, out var draft)
                    ? draft
                    : definition.DefaultValue;

                switch (definition.Key)
                {
                    case clsFormat.MoneyDecimals:
                        _draft[definition.Key] = _SelectOption(cmbMoneyDecimals, definition.Key, value);
                        break;

                    case clsFormat.QuantityDecimals:
                        _draft[definition.Key] = _SelectOption(cmbQuantityDecimals, definition.Key, value);
                        break;

                    case clsFormat.NumberDecimals:
                        _draft[definition.Key] = _SelectOption(cmbNumberDecimals, definition.Key, value);
                        break;

                    case clsFormat.PercentDecimals:
                        _draft[definition.Key] = _SelectOption(cmbPercentDecimals, definition.Key, value);
                        break;

                    case clsFormat.ThousandsSeparator:
                        _draft[definition.Key] = _SetToggle(rdoThousandsYes, rdoThousandsNo, value);
                        break;

                    case clsFormat.DecimalSeparator:
                        _draft[definition.Key] = _SelectOption(cmbDecimalSeparator, definition.Key, value);
                        break;

                    case clsFormat.ArabicIndicDigits:
                        _draft[definition.Key] = _SetToggle(rdoArabicYes, rdoArabicNo, value);
                        break;

                    case clsFormat.DateShortPattern:
                        _draft[definition.Key] = _SelectOrType(cmbDateShortPattern, definition.Key, value);
                        break;

                    case clsFormat.DateDayMonthYearPattern:
                        _draft[definition.Key] = _SelectOrType(cmbDateDayMonthYearPattern, definition.Key, value);
                        break;

                    case clsFormat.DateLongPattern:
                        _draft[definition.Key] = _SelectOrType(cmbDateLongPattern, definition.Key, value);
                        break;
                }
            }
        }
        finally
        {
            _isBinding = false;
        }
    }

    /// <summary>
    /// يملأ قائمة اختيار من <c>clsFormat.Options</c> (المصدر الوحيد للتسميات العربية — لا نص خيار مكتوب في الشاشة)،
    /// ثم يختار الخيار المطابق للقيمة؛ وإن لم توجد ضمن الخيارات يختار الأول — وهو الافتراضي في قائمتي
    /// <c>Choice</c>/<c>Pattern</c> حصراً، أما <c>Decimals</c> فيبدأ من <c>Min</c> («0») لا من الافتراضي.
    /// يُعيد القيمة المعروضة فعلاً كي تُكتب عائدة إلى المسودّة. تُستدعى داخل <c>_isBinding</c> فلا يُشغّل
    /// إعادة البناء تحديث المسودّة/المعاينة.
    /// </summary>
    private static string _SelectOption(ComboBox combo, string key, string value)
    {
        var options = _FillOptions(combo, key);

        int index = _IndexOfOption(options, value);
        combo.SelectedIndex = index >= 0 ? index : (options.Count > 0 ? 0 : -1);

        return _OptionValue(combo);
    }

    /// <summary>
    /// كالسابق لكن لقائمة صيغ التواريخ القابلة للكتابة: إن لم تكن قيمة المسودّة ضمن الاقتراحات تُكتب كما هي
    /// في الحقل (صيغة مخصّصة يكتبها المستخدم) فلا تُفقد ولا تتحوّل إلى الافتراضي. يُعيد القيمة المعروضة فعلاً.
    /// </summary>
    private static string _SelectOrType(ComboBox combo, string key, string value)
    {
        var options = _FillOptions(combo, key);

        int index = _IndexOfOption(options, value);
        if (index >= 0) combo.SelectedIndex = index;
        else combo.Text = value;

        return _OptionValue(combo);
    }

    /// <summary>قيمة الخيار المختار المخزَّنة (لا نصّه المعروض) — وللصيغة المكتوبة يدوياً نصّ الحقل مُشذَّباً.</summary>
    private static string _OptionValue(ComboBox combo)
        => combo.SelectedItem is FormatOption option ? option.Value : combo.Text.Trim();

    /// <summary>
    /// يعكس مفتاح نعم/لا على زوج أزرار اختيار — القراءة عبر <c>clsFormat.IsOn</c> فتُقبل كل صيغ العقد
    /// («true»/«1»/«نعم»)، ويُعيد التمثيل القانوني المعروض فعلاً (<c>ToggleTrue</c>/<c>ToggleFalse</c>).
    /// </summary>
    private static string _SetToggle(RadioButton yes, RadioButton no, string value)
    {
        bool isOn = clsFormat.IsOn(value);

        yes.Checked = isOn;
        no.Checked = !isOn;

        return yes.Checked ? clsFormat.ToggleTrue : clsFormat.ToggleFalse;
    }

    /// <summary>يملأ القائمة من البيت الواحد ويعيد خياراتها (بلا نسخة ثانية من التسميات).</summary>
    private static IReadOnlyList<FormatOption> _FillOptions(ComboBox combo, string key)
    {
        var options = clsFormat.Options(key);

        combo.BeginUpdate();
        try
        {
            combo.Items.Clear();

            foreach (var option in options) combo.Items.Add(option);
        }
        finally
        {
            combo.EndUpdate();
        }

        return options;
    }

    /// <summary>رقم الخيار المطابق للقيمة المخزَّنة، أو <c>-1</c> إن لم تكن ضمن الاقتراحات.</summary>
    private static int _IndexOfOption(IReadOnlyList<FormatOption> options, string value)
    {
        for (var index = 0; index < options.Count; index++)
        {
            if (string.Equals(options[index].Value, value, StringComparison.Ordinal)) return index;
        }

        return -1;
    }

    // ── التغيير → المسودّة → المعاينة → التحقق → الحالة (مسار واحد لكل عنصر) ────

    /// <summary>
    /// المعالج الوحيد لكل عناصر القيم (قائمة منازل · فاصل الكسر · زوج أزرار نعم/لا · صيغة تاريخ).
    /// كل تغيير يمرّ بالخط الأربعة نفسه بلا تكرار منطق في أي معالج آخر.
    /// </summary>
    private void Value_Changed(object? sender, EventArgs e)
    {
        if (_isBinding) return;

        _RefreshDraft();
        _UpdatePreview();
        _ValidateDraft();
        _UpdateMode();
        _UpdateSummary();
    }

    /// <summary>يقرأ العناصر إلى المسودّة — المفاتيح من <c>clsFormat</c> والقيم من خيارات القوائم وأزرار الاختيار.</summary>
    private void _RefreshDraft()
    {
        _draft[clsFormat.MoneyDecimals] = _OptionValue(cmbMoneyDecimals);
        _draft[clsFormat.QuantityDecimals] = _OptionValue(cmbQuantityDecimals);
        _draft[clsFormat.NumberDecimals] = _OptionValue(cmbNumberDecimals);
        _draft[clsFormat.PercentDecimals] = _OptionValue(cmbPercentDecimals);

        _draft[clsFormat.ThousandsSeparator] = rdoThousandsYes.Checked ? clsFormat.ToggleTrue : clsFormat.ToggleFalse;
        _draft[clsFormat.ArabicIndicDigits] = rdoArabicYes.Checked ? clsFormat.ToggleTrue : clsFormat.ToggleFalse;

        // فاصل الكسر لا يبقى فارغاً أبداً: عند تعذّر الاختيار يُستهلك الافتراضي من clsFormat (لا نص مكتوب هنا).
        string separator = _OptionValue(cmbDecimalSeparator);
        _draft[clsFormat.DecimalSeparator] = string.IsNullOrEmpty(separator)
            ? _DefaultValue(clsFormat.DecimalSeparator)
            : separator;

        _draft[clsFormat.DateShortPattern] = _OptionValue(cmbDateShortPattern);
        _draft[clsFormat.DateDayMonthYearPattern] = _OptionValue(cmbDateDayMonthYearPattern);
        _draft[clsFormat.DateLongPattern] = _OptionValue(cmbDateLongPattern);
    }

    /// <summary>
    /// المعاينة الفورية: تستدعي **المنسّق الحقيقي** <see cref="clsFormat"/> وتمرّر له المسودّة
    /// كوسيط <c>values</c> — فلا صيغة ولا فاصل ولا رقم مكتوب داخل الشاشة (RULE-N169).
    /// </summary>
    private void _UpdatePreview()
    {
        try
        {
            lblPreviewMoney.Text = clsFormat.Money(PreviewAmount, _draft);
            lblPreviewQuantity.Text = clsFormat.Quantity(PreviewAmount, _draft);
            lblPreviewNumber.Text = clsFormat.Number(PreviewAmount, _draft);
            lblPreviewPercent.Text = clsFormat.Percent(PreviewPercent, _draft);

            lblPreviewDateShort.Text = clsFormat.DateToShort(PreviewDate, _draft);
            lblPreviewDateDMY.Text = clsFormat.DateToDayMonthYear(PreviewDate, _draft);
            lblPreviewDateLong.Text = clsFormat.DateToLong(PreviewDateTime, _draft);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر بناء معاينة إعدادات التنسيق");
        }
    }

    /// <summary>
    /// التحقق الوحيد للدفعة عبر <c>clsFormat.ValidateAll</c> (لا قاعدة محلية). عند الرفض تُعرض
    /// الرسالة العربية ويُعطَّل الحفظ، وتبقى المعاينة على آخر شكل صالح — لا تُفرَّغ.
    /// </summary>
    private void _ValidateDraft()
    {
        string? error = clsFormat.ValidateAll(_draft);

        lblValidation.Text = error ?? string.Empty;
        btnSave.Enabled = error is null;
    }

    /// <summary>مؤشر الوضع (RULE-N44): محفوظة ⟷ تغييرات غير محفوظة + عددها، وربط Enter بالزر الأساسي.</summary>
    private void _UpdateMode()
    {
        int changed = _ChangedCount();

        lblMode.Text = changed == 0
            ? ModeSaved
            : $"{ModeDirty} ({changed})";

        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>شريط الملخص (RULE-N61): عدد الإعدادات وعدد المعدّل منها.</summary>
    private void _UpdateSummary()
    {
        lblSummary.Text = $"عدد الإعدادات: {clsFormat.Definitions.Count} · المعدّلة: {_ChangedCount()}";
    }

    private Button? _PrimaryActionButton() => btnSave;

    // ── Actions ───────────────────────────────────────────────────────────────

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (_isBusy) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            _isBusy = true;

            // نسخة مستقلة من المسودّة — لا يُلمس _draft بعد الإرسال.
            var batch = new Dictionary<string, string>(_draft, StringComparer.Ordinal);

            var result = await DesktopService.SaveAsync(new FormatSettings(batch));

            if (!result.IsSuccess)
            {
                // رفض الخدمة (RULE-N167): لا حالة جزئية — الرسالة العربية في الشاشة وفي صندوق.
                string message = result.Error ?? "تعذر حفظ إعدادات التنسيق.";
                lblValidation.Text = message;
                clsDialogs.Warning(message, "لم يتم الحفظ");
                return;
            }

            // النجاح فقط: إعادة القراءة من المخزن (المصدر الوحيد للحقيقة).
            await _LoadAsync();

            clsDialogs.Success(
                "تم حفظ إعدادات التنسيق وتطبيقها. ستظهر القيم الجديدة في الشاشات عند تحديث بياناتها (F5) — بلا حاجة لإعادة تشغيل التطبيق.",
                "تم الحفظ");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ إعدادات التنسيق");
            clsDialogs.Error("حدث خطأ أثناء حفظ إعدادات التنسيق. يرجى إعادة المحاولة.");
        }
        finally
        {
            _isBusy = false;
            Cursor = Cursors.Default;
        }
    }

    private async void btnReset_Click(object? sender, EventArgs e)
    {
        if (_isBusy) return;

        if (!clsDialogs.Confirm("هل تريد إعادة كل إعدادات التنسيق إلى قيمها الافتراضية؟", "تأكيد الاستعادة"))
        {
            return;
        }

        Cursor = Cursors.WaitCursor;
        try
        {
            _isBusy = true;

            var result = await DesktopService.ResetToDefaultsAsync();

            if (!result.IsSuccess)
            {
                clsDialogs.Warning(result.Error ?? "تعذر استعادة إعدادات التنسيق الافتراضية.", "لم تتم الاستعادة");
                return;
            }

            await _LoadAsync();

            clsDialogs.Success("تمت استعادة إعدادات التنسيق الافتراضية.", "تمت الاستعادة");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء استعادة إعدادات التنسيق الافتراضية");
            clsDialogs.Error("حدث خطأ أثناء استعادة إعدادات التنسيق. يرجى إعادة المحاولة.");
        }
        finally
        {
            _isBusy = false;
            Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>عدد الإعدادات التي تختلف مسودّتها عن قيمتها المحفوظة (مقارنة نصية ترتيبية).</summary>
    private int _ChangedCount()
    {
        int changed = 0;

        foreach (var definition in clsFormat.Definitions)
        {
            string current = _draft.TryGetValue(definition.Key, out var draft) ? draft : definition.DefaultValue;
            string saved = _baseline.TryGetValue(definition.Key, out var baseline) ? baseline : definition.DefaultValue;

            if (!string.Equals(current, saved, StringComparison.Ordinal)) changed++;
        }

        return changed;
    }

    /// <summary>القيمة الافتراضية للمفتاح — من تعريفات <c>clsFormat</c> وحدها (لا نص مكرّر في الشاشة).</summary>
    private static string _DefaultValue(string key)
        => clsFormat.Definitions
            .FirstOrDefault(definition => string.Equals(definition.Key, key, StringComparison.Ordinal))
            ?.DefaultValue
           ?? string.Empty;
}
