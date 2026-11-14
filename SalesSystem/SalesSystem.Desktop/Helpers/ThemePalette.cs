// -----------------------------------------------------------------------------
// ThemePalette.cs — المصدر الوحيد **القابل للتغيير** لألوان النظام (RULE-N54)
//
// قبل هذا الملف كانت ألوان الثيم الأخضر ثوابت `static readonly` موزّعة بين
// GreenThemeHelper و ThemeHelper، فكان تغيير أي لون يستلزم إعادة بناء التطبيق.
// الآن:
//
//     ThemePalette (قيم حيّة + التعريفات العربية)
//            ↓
//     GreenThemeHelper / ThemeHelper (خصائص مرور فقط — بلا أي لون حرفي)
//            ↓
//     الشاشات (بلا تغيير — نفس الأسماء ونفس النوع Color)
//
// قواعد ملزمة:
//   • لا يُشير هذا الملف إلى GreenThemeHelper إطلاقاً (منعاً للتهيئة الدائرية).
//   • لا يوجد أي `Color.FromArgb` خارج جدول الافتراضيات أدناه (RULE-N112).
//   • التغيير هنا لا يُعيد إنشاء الشاشات: ApplyToOpenForms تُعيد تلوين المفتوح فقط.
// -----------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Drawing;

namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// تعريف لون واحد من لوحة ألوان النظام: المفتاح الثابت + التسمية العربية
/// الظاهرة في شاشة «ألوان النظام» + اسم المجموعة + القيمة الافتراضية.
/// </summary>
/// <param name="Key">المفتاح الثابت (مطابق لأسماء الحقول في الجدول).</param>
/// <param name="ArabicLabel">التسمية العربية الظاهرة للمستخدم.</param>
/// <param name="GroupLabel">اسم المجموعة التي يظهر تحتها اللون.</param>
/// <param name="DefaultValue">القيمة الافتراضية التي تُعيدها <see cref="ThemePalette.ResetToDefaults"/>.</param>
public sealed record ThemeColorDefinition(string Key, string ArabicLabel, string GroupLabel, Color DefaultValue);

/// <summary>
/// لوحة ألوان النظام الرسمية: نقطة التغيير الوحيدة لألوان الواجهة كاملة.
/// <para>
/// كل عضو عام في <see cref="GreenThemeHelper"/> و<see cref="ThemeHelper"/> يقرأ من
/// هنا وقت العرض، فبمجرد <see cref="TrySet"/> أو <see cref="ResetToDefaults"/>
/// تتغيّر كل الشاشات المفتوحة عبر <see cref="ApplyToOpenForms"/> بلا إعادة فتح.
/// </para>
/// </summary>
public static class ThemePalette
{
    // ═════════════════════════════════════════════════════════════════
    //  مفاتيح اللوحة (ثوابت — تُستهلك من الشاشات والخدمات بالاسم)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>خلفية الشاشة الرئيسية (خلفية كل النماذج).</summary>
    public const string FrmMainColor = "frmMainColor";

    /// <summary>شريط العنوان.</summary>
    public const string HeaderBarColor = "headerBarColor";

    /// <summary>نص شريط العنوان.</summary>
    public const string HeaderTextColor = "headerTextColor";

    /// <summary>النص المميّز (أخضر داكن).</summary>
    public const string TextStrongColor = "textStrongColor";

    /// <summary>زر «جديد / إضافة».</summary>
    public const string BtnAddNewButtonColor = "btnAddNewButtonColor";

    /// <summary>زر «حفظ».</summary>
    public const string BtnSaveButtonColor = "btnSaveButtonColor";

    /// <summary>زر «حذف».</summary>
    public const string BtnDeleteButtonColor = "btnDeleteButtonColor";

    /// <summary>زر «إغلاق / إلغاء».</summary>
    public const string BtnCancelButtonColor = "btnCancelButtonColor";

    /// <summary>زر «تصدير / معلومات».</summary>
    public const string BtnInfoButtonColor = "btnInfoButtonColor";

    /// <summary>زر «تحذير».</summary>
    public const string BtnWarningButtonColor = "btnWarningButtonColor";

    /// <summary>زر «ثانوي».</summary>
    public const string BtnSecondaryButtonColor = "btnSecondaryButtonColor";

    /// <summary>أشرطة الأدوات والحاويات.</summary>
    public const string BarBackgroundColor = "barBackgroundColor";

    /// <summary>صناديق المجموعات.</summary>
    public const string GroupBoxColor = "groupBoxColor";

    /// <summary>خلفية الشبكة.</summary>
    public const string GridBackgroundColor = "gridBackgroundColor";

    /// <summary>رؤوس أعمدة الشبكة.</summary>
    public const string GridHeaderColor = "gridHeaderColor";

    /// <summary>نص رؤوس الأعمدة.</summary>
    public const string GridHeaderTextColor = "gridHeaderTextColor";

    /// <summary>خطوط الشبكة.</summary>
    public const string GridLineColor = "gridLineColor";

    /// <summary>الأسطر المتبادلة في الشبكة.</summary>
    public const string GridAlternateRowColor = "gridAlternateRowColor";

    /// <summary>خلفية خلايا الشبكة.</summary>
    public const string GridCellBackgroundColor = "gridCellBackgroundColor";

    /// <summary>نص خلايا الشبكة.</summary>
    public const string GridCellTextColor = "gridCellTextColor";

    /// <summary>لون السطر المحدد.</summary>
    public const string SelectionRowColor = "selectionRowColor";

    /// <summary>
    /// لون الحقل النشط (تظليل التركيز). قيمته الافتراضية هي نفس اللون الحرفي الذي
    /// تستخدمه كل الشاشات عبر <c>KeyboardHelper.WireFocusHighlight</c> —
    /// <c>(0xD6,0xE4,0xF0)</c> = (214,228,240) — فيبقى «المصمّم = التشغيل» صحيحاً.
    /// الشاشات لا تحتاج أي تعديل: التوصيل يقرأ القيمة الحيّة من هنا (RULE-N54a).
    /// </summary>
    public const string ActiveFieldColor = "activeFieldColor";

    /// <summary>خلفية العنصر المعطّل.</summary>
    public const string DisabledBackColor = "disabledBackColor";

    /// <summary>نص العنصر المعطّل.</summary>
    public const string DisabledForeColor = "disabledForeColor";

    // ═════════════════════════════════════════════════════════════════
    //  جدول الافتراضيات (القيم الحالية حرفياً — التشغيل = المصمّم)
    // ═════════════════════════════════════════════════════════════════

    private const string GroupMain = "الشاشة الرئيسية";
    private const string GroupButtons = "الأزرار";
    private const string GroupBars = "الصناديق والأشرطة";
    private const string GroupGrid = "الشبكة";
    private const string GroupSelection = "التحديد والحقل النشط";
    private const string GroupStates = "حالات العرض";

    private static readonly ThemeColorDefinition[] AllDefinitions =
    {
        // ── الشاشة الرئيسية (4) ──
        new(FrmMainColor,            "خلفية الشاشة الرئيسية", GroupMain,      Color.FromArgb(170, 220, 170)),
        new(HeaderBarColor,          "شريط العنوان",          GroupMain,      Color.FromArgb(14, 165, 103)),
        new(HeaderTextColor,         "نص شريط العنوان",       GroupMain,      Color.White),
        new(TextStrongColor,         "النص المميّز",          GroupMain,      Color.FromArgb(14, 82, 52)),
        // ── الأزرار (7) ──
        new(BtnAddNewButtonColor,    "زر «جديد / إضافة»",     GroupButtons,   Color.FromArgb(13, 110, 253)),
        new(BtnSaveButtonColor,      "زر «حفظ»",              GroupButtons,   Color.FromArgb(25, 135, 84)),
        new(BtnDeleteButtonColor,    "زر «حذف»",              GroupButtons,   Color.FromArgb(220, 53, 69)),
        new(BtnCancelButtonColor,    "زر «إغلاق / إلغاء»",    GroupButtons,   Color.FromArgb(108, 117, 125)),
        new(BtnInfoButtonColor,      "زر «تصدير / معلومات»",  GroupButtons,   Color.FromArgb(23, 162, 184)),
        new(BtnWarningButtonColor,   "زر «تحذير»",            GroupButtons,   Color.FromArgb(255, 193, 7)),
        new(BtnSecondaryButtonColor, "زر «ثانوي»",            GroupButtons,   Color.FromArgb(248, 249, 250)),
        // ── الصناديق والأشرطة (2) ──
        new(BarBackgroundColor,      "أشرطة الأدوات والحاويات", GroupBars,    Color.FromArgb(235, 240, 235)),
        new(GroupBoxColor,           "صناديق المجموعات",      GroupBars,      Color.FromArgb(235, 245, 235)),
        // ── الشبكة (7 — لون التحديد انتقل إلى مجموعة «التحديد والحقل النشط») ──
        new(GridBackgroundColor,     "خلفية الشبكة",          GroupGrid,      Color.FromArgb(220, 240, 220)),
        new(GridHeaderColor,         "رؤوس أعمدة الشبكة",     GroupGrid,      Color.FromArgb(14, 165, 103)),
        new(GridHeaderTextColor,     "نص رؤوس الأعمدة",       GroupGrid,      Color.White),
        new(GridLineColor,           "خطوط الشبكة",           GroupGrid,      Color.FromArgb(205, 225, 205)),
        new(GridAlternateRowColor,   "الأسطر المتبادلة",      GroupGrid,      Color.FromArgb(245, 252, 245)),
        new(GridCellBackgroundColor, "خلفية خلايا الشبكة",    GroupGrid,      Color.White),
        new(GridCellTextColor,       "نص خلايا الشبكة",       GroupGrid,      Color.FromArgb(33, 37, 41)),
        // ── التحديد والحقل النشط (2) ──
        new(SelectionRowColor,       "لون السطر المحدد",      GroupSelection, Color.FromArgb(255, 255, 180)),
        new(ActiveFieldColor,        "لون الحقل النشط",       GroupSelection, Color.FromArgb(214, 228, 240)),
        // ── حالات العرض (2) ──
        new(DisabledBackColor,       "خلفية العنصر المعطّل",  GroupStates,    Color.FromArgb(224, 224, 224)),
        new(DisabledForeColor,       "نص العنصر المعطّل",     GroupStates,    Color.FromArgb(150, 150, 150)),
    };

    private static readonly ReadOnlyCollection<ThemeColorDefinition> DefinitionsReadOnly =
        Array.AsReadOnly(AllDefinitions);

    /// <summary>القيم الحيّة (المفاتيح بمقارنة ترتيبية — نفس أسماء الثوابت).</summary>
    private static readonly Dictionary<string, Color> Live = BuildLive();

    /// <summary>
    /// لقطة القيم التي **طُبِّقت فعلاً** في آخر <see cref="ApplyToOpenForms"/> —
    /// تُهيّأ بالافتراضيات، ولا تتقدّم إلا في نهاية <see cref="ApplyToOpenForms"/>
    /// (لا في <see cref="TrySet"/> ولا في <see cref="ResetToDefaults"/>).
    /// <para>
    /// وظيفتان: (1) إعادة تلوين أزرار النماذج **المفتوحة** التي تحمل لون الزر السابق،
    /// (2) إخبار <see cref="GreenThemeHelper.IsThemeColor"/> أن اللون الذي سبق أن
    /// طبّقته اللوحة هو لون ثيم لا لون مصمّم مخصّص — فتتبع الحاويات والأزرار تغييراً
    /// ثانياً وثالثاً بلا إعادة فتح الشاشة.
    /// </para>
    /// </summary>
    private static readonly Dictionary<string, Color> PreviousLive = BuildLive();

    /// <summary>مفاتيح ألوان الأزرار السبعة (خلفيات أزرار الشاشات — <c>ThemeHelper</c>).</summary>
    private static readonly string[] ButtonKeys =
    {
        BtnAddNewButtonColor,
        BtnSaveButtonColor,
        BtnDeleteButtonColor,
        BtnCancelButtonColor,
        BtnInfoButtonColor,
        BtnWarningButtonColor,
        BtnSecondaryButtonColor
    };

    // ═════════════════════════════════════════════════════════════════
    //  القراءة والكتابة
    // ═════════════════════════════════════════════════════════════════

    /// <summary>تعريفات اللوحة (24 لوناً بترتيب العرض في الشاشة، في ست مجموعات).</summary>
    public static IReadOnlyList<ThemeColorDefinition> Definitions => DefinitionsReadOnly;

    /// <summary>فهرس التعريفات بالمفتاح (تطابق ترتيبي — نفس أسماء الثوابت).</summary>
    private static readonly Dictionary<string, ThemeColorDefinition> DefinitionsByKey = BuildDefinitionsByKey();

    private static Dictionary<string, ThemeColorDefinition> BuildDefinitionsByKey()
    {
        var map = new Dictionary<string, ThemeColorDefinition>(StringComparer.Ordinal);
        foreach (var definition in AllDefinitions)
            map[definition.Key] = definition;

        return map;
    }

    // ═════════════════════════════════════════════════════════════════
    //  الافتراضيات والتباين (WCAG 2.1) — خريطة واحدة يستهلكها الجميع
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// الحد الأدنى لتباين **نص المتن العادي** بنظام WCAG 2.1 (4.5:1).
    /// <para>
    /// الموطن الوحيد لهذا الرقم (RULE-N172): تُستهلك هنا في <see cref="ContrastRules"/>
    /// وفي <see cref="ColorSuggestionHelper.ReadableForeFor"/> — وممنوع كتابته حرفياً
    /// في أي شاشة أو خدمة (RULE-N112).
    /// </para>
    /// </summary>
    public const double BodyTextMinimumContrast = 4.5;

    /// <summary>
    /// خريطة التباين **المتناظرة** (المفتاح ← اللون الشريك + الحد الأدنى). مصدر واحد
    /// فقط: الشاشة والخدمة الوهمية و<see cref="ColorSuggestionHelper"/> تقرأ منه —
    /// وممنوع تكرارها في أي ملف آخر (RULE-N54a / RULE-N112).
    /// <para>
    /// لا يظهر المفتاح هنا معناه «لا شرط تباين» (<see cref="MinimumContrastFor"/> = 0.0)
    /// و<see cref="ContrastPartnerColor"/> = <c>null</c>.
    /// </para>
    /// </summary>
    private static readonly Dictionary<string, (string Partner, double Minimum)> ContrastRules =
        BuildContrastRules();

    private static Dictionary<string, (string Partner, double Minimum)> BuildContrastRules()
    {
        var map = new Dictionary<string, (string Partner, double Minimum)>(StringComparer.Ordinal);

        void Add(string key, string partner, double minimum) => map[key] = (partner, minimum);

        // نصوص/رؤوس فوق خلفيات: 3.0 لنص كبير (شريط/رؤوس) و4.5 لنص عادي.
        Add(HeaderTextColor, HeaderBarColor, 3.0);
        Add(HeaderBarColor, HeaderTextColor, 3.0);
        Add(GridHeaderTextColor, GridHeaderColor, 3.0);
        Add(GridHeaderColor, GridHeaderTextColor, 3.0);
        Add(GridCellTextColor, GridCellBackgroundColor, BodyTextMinimumContrast);
        Add(GridCellBackgroundColor, GridCellTextColor, BodyTextMinimumContrast);
        Add(SelectionRowColor, GridCellTextColor, BodyTextMinimumContrast);
        Add(TextStrongColor, FrmMainColor, BodyTextMinimumContrast);
        Add(FrmMainColor, TextStrongColor, BodyTextMinimumContrast);
        // حالات العرض: تباين مقروء لا أكثر (رمادي على رمادي).
        Add(DisabledForeColor, DisabledBackColor, 2.0);
        Add(DisabledBackColor, DisabledForeColor, 2.0);

        return map;
    }

    /// <summary>
    /// القيمة الافتراضية لمفتاح معروف؛ والمفتاح المجهول يُعيد افتراضي خلفية الشاشة
    /// الرئيسية (لا استثناء — عرض اللون لا يتوقّف بسبب مفتاح خاطئ).
    /// </summary>
    public static Color DefaultFor(string key)
    {
        if (key is not null && DefinitionsByKey.TryGetValue(key, out var definition))
            return definition.DefaultValue;

        return DefinitionsByKey.TryGetValue(FrmMainColor, out var fallback)
            ? fallback.DefaultValue
            : Color.White;
    }

    /// <summary>
    /// نسبة التباين بنظام WCAG 2.1 بين لونين (1.00 … 21.00) **مقرَّبة بمنزلتين عشريتين** —
    /// للعرض على المستخدم فقط. أي مقارنة تحقّق تستخدم <see cref="ContrastRatioRaw"/>
    /// (التقريب قد يرفع/يخفض نصاً على الحد تماماً — RULE-N167).
    /// </summary>
    public static double ContrastRatio(Color a, Color b) => Math.Round(ContrastRatioRaw(a, b), 2);

    /// <summary>
    /// نسبة التباين **غير المقرَّبة** (WCAG 2.1) — تُستخدم في كل مقارنة تحقّق
    /// («هل بلغ التباين الحد الأدنى؟») لأن التقريب يمكن أن يقبل لوناً على الحد أو يرفضه.
    /// لا يوجد تحويل ثانٍ لمعادلة WCAG في أي ملف آخر (RULE-N172).
    /// </summary>
    internal static double ContrastRatioRaw(Color a, Color b)
    {
        double luminanceA = RelativeLuminance(a);
        double luminanceB = RelativeLuminance(b);

        double lighter = Math.Max(luminanceA, luminanceB);
        double darker = Math.Min(luminanceA, luminanceB);

        return (lighter + 0.05) / (darker + 0.05);
    }

    /// <summary>الحد الأدنى لتباين المفتاح (0.0 = لا شرط).</summary>
    public static double MinimumContrastFor(string key)
        => key is not null && ContrastRules.TryGetValue(key, out var rule) ? rule.Minimum : 0.0;

    /// <summary>
    /// مفتاح اللون الشريك الذي يُقاس عليه تباين المفتاح، أو <c>null</c> إن لا شرط تباين.
    /// <para>
    /// هذه هي القراءة الوحيدة لخريطة <see cref="ContrastRules"/> في اتجاه الشريك؛
    /// تُستهلك هنا في <see cref="ContrastPartnerColor"/> وفي الخدمة الوهمية التي تحتاج
    /// **المفتاح** لا اللون لتقرأ قيمة الطلب المقدَّم بدل القيمة الحيّة (RULE-N172).
    /// </para>
    /// </summary>
    public static string? ContrastPartnerKey(string key)
        => key is not null && ContrastRules.TryGetValue(key, out var rule) ? rule.Partner : null;

    /// <summary>اللون الشريك الحيّ الذي يُقاس عليه تباين المفتاح، أو <c>null</c> إن لا شرط.</summary>
    public static Color? ContrastPartnerColor(string key)
        => ContrastPartnerKey(key) is { } partnerKey ? Get(partnerKey) : null;

    /// <summary>الإضاءة النسبية (WCAG) للون: 0.0 أسود … 1.0 أبيض.</summary>
    private static double RelativeLuminance(Color color)
        => (0.2126 * Linearize(color.R))
         + (0.7152 * Linearize(color.G))
         + (0.0722 * Linearize(color.B));

    /// <summary>تحويل قناة (0..255) إلى مساحة الإضاءة الخطية في WCAG.</summary>
    private static double Linearize(byte channel)
    {
        double value = channel / 255.0;

        return value <= 0.03928
            ? value / 12.92
            : Math.Pow((value + 0.055) / 1.055, 2.4);
    }

    /// <summary>
    /// القيمة الحيّة للون. المفتاح المجهول يُعيد لون خلفية الشاشة الرئيسية
    /// (لا استثناء — العرض لا يتوقّف بسبب مفتاح خاطئ).
    /// </summary>
    public static Color Get(string key)
    {
        if (TryGet(key, out var color)) return color;

        return TryGet(FrmMainColor, out var fallback) ? fallback : Color.White;
    }

    /// <summary>هل المفتاح معروف؟ يُعيد قيمته الحيّة في <paramref name="color"/>.</summary>
    public static bool TryGet(string key, out Color color)
    {
        color = Color.Empty;
        return key is not null && Live.TryGetValue(key, out color);
    }

    /// <summary>
    /// يغيّر لوناً واحداً. المفتاح المجهول يُعيد <c>false</c> **بلا أي تغيير**
    /// (التحقق هنا وليس في الشاشة — RULE-N112).
    /// </summary>
    public static bool TrySet(string key, Color color)
    {
        if (key is null || !Live.ContainsKey(key)) return false;

        Live[key] = color;
        return true;
    }

    /// <summary>يُعيد كل الألوان إلى الافتراضيات المعلنة في <see cref="Definitions"/>.</summary>
    public static void ResetToDefaults()
    {
        Live.Clear();
        foreach (var definition in AllDefinitions)
            Live[definition.Key] = definition.DefaultValue;
    }

    /// <summary>نسخة من القيم الحيّة (لا تُعدّل الداخلية).</summary>
    public static IReadOnlyDictionary<string, Color> Snapshot()
        => new Dictionary<string, Color>(Live, StringComparer.Ordinal);

    // ═════════════════════════════════════════════════════════════════
    //  إعادة تلوين المفتوح (بلا إغلاق ولا إعادة إنشاء)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يُعيد تطبيق ألوان اللوحة على **كل النماذج المفتوحة** بلا إغلاق أو إعادة فتح:
    /// <see cref="GreenThemeHelper.Apply(Form)"/> (الحاويات والعناوين والشبكات)،
    /// ثم أزرار النموذج السبعة ألواناً (RULE-N154 غير معنية — النص لا يُلمس)،
    /// ثم منطقة MDI وقوائم النموذج، ثم إبطال الرسم لتنعكس الألوان فوراً.
    /// لا تُرمى أي استثناءات من هنا (سبب واحد فقط: خطأ يُسجَّل ولا يُسقِط الشاشة).
    /// </summary>
    public static void ApplyToOpenForms()
    {
        Form[] forms;
        try
        {
            forms = Application.OpenForms.Cast<Form>().ToArray();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر قراءة النماذج المفتوحة عند تطبيق ألوان النظام");
            return;
        }

        foreach (var form in forms)
        {
            try
            {
                if (form is null || form.IsDisposed || form.Disposing) continue;

                GreenThemeHelper.Apply(form);
                RepaintThemeButtons(form);

                // الحقل النشط المفتوح يعيد التلوين فوراً باللون الحيّ (RULE-N54a)
                KeyboardHelper.RefreshFocusHighlight(form);

                // منطقة MDI: خلفية النموذج المصمَّمة (RULE-N139/RULE-N140)
                foreach (var mdi in form.Controls.OfType<MdiClient>())
                    mdi.BackColor = GreenThemeHelper.FormBackground;

                // شريط القوائم (الرئيسي أو المضمَّن في شجرة العناصر)
                if (form.MainMenuStrip is not null)
                    PaintMenuStrip(form.MainMenuStrip);

                PaintMenuStrips(form);

                form.Invalidate(true);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "تعذر تطبيق ألوان النظام على النموذج {Form}", form?.Name);
            }
        }

        // التقدّم هنا فقط: حتى يكون «السابق» هو ما يراه المستخدم فعلاً، فيلتقط
        // التطبيق التالي الأزرار التي لُوِّنت الآن ويعيد تلوينها بالقيمة الجديدة.
        AdvancePreviousLive();
    }

    /// <summary>
    /// هل يطابق <paramref name="color"/> قيمةً **سبق أن طبّقتها اللوحة** على هذا المفتاح؟
    /// تُستهلك من <see cref="GreenThemeHelper"/> وحدها لتمييز «لون ثيم قديم» عن
    /// «لون مصمّم مخصّص» (RULE-N139) — داخلية، وليست جزءاً من عقد اللوحة العام.
    /// </summary>
    internal static bool WasAppliedValue(string key, Color color)
        => key is not null
           && PreviousLive.TryGetValue(key, out var previous)
           && previous.ToArgb() == color.ToArgb();

    /// <summary>يُعيد «السابق» = «الحالي» (بعد انتهاء التطبيق فقط).</summary>
    private static void AdvancePreviousLive()
    {
        PreviousLive.Clear();
        foreach (var pair in Live)
            PreviousLive[pair.Key] = pair.Value;
    }

    /// <summary>
    /// يمشي على كل أزرار شجرة العناصر ويعيد تلوين ما كان يحمل لون زرٍّ من اللوحة
    /// (قبل التطبيق الحالي) بالقيمة الحيّة الجديدة — و`CheckBox`/`RadioButton`
    /// مستثناة صريحاً، والنص (<c>ForeColor</c>) لا يُلمس أبداً لأن الزر الثانوي
    /// فاتح بنص داكن.
    /// </summary>
    private static void RepaintThemeButtons(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            if (child is Button button && child is not CheckBox && child is not RadioButton)
                RepaintThemeButton(button);

            if (child.HasChildren)
                RepaintThemeButtons(child);
        }
    }

    /// <summary>
    /// زر واحد: إن كانت خلفيته تساوي القيمة **السابقة** لأحد مفاتيح الأزرار السبعة،
    /// تُستبدل بالقيمة الحيّة لنفس المفتاح وتُضبط خلفية المرور بالفعل بنفس قاعدة
    /// <c>ThemeHelper.ApplyButtonStyle</c> (<c>ControlPaint.Light(color, 0.1f)</c>).
    /// الزر الذي لم يكن بلون ثيم (لون مصمّم مخصّص) لا يُلمس إطلاقاً.
    /// </summary>
    private static void RepaintThemeButton(Button button)
    {
        int back = button.BackColor.ToArgb();

        foreach (var key in ButtonKeys)
        {
            if (!PreviousLive.TryGetValue(key, out var previous) || previous.ToArgb() != back)
                continue;

            var current = Get(key);

            // اللون لم يتغيّر بين التطبيقين ⇒ لا لمس (يبقى المرور المخصّص كما هو).
            if (current.ToArgb() == back) break;

            button.BackColor = current;
            button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(current, 0.1f);
            break;
        }
    }

    private static void PaintMenuStrips(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            if (child is MenuStrip menu) PaintMenuStrip(menu);
            if (child.HasChildren) PaintMenuStrips(child);
        }
    }

    private static void PaintMenuStrip(MenuStrip menu)
    {
        menu.BackColor = GreenThemeHelper.BarBackground;
        menu.ForeColor = GreenThemeHelper.TextStrong;
    }

    // ═════════════════════════════════════════════════════════════════
    //  تحويل HEX («#RRGGBB» فقط)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يحوّل لوناً إلى النص <c>#RRGGBB</c> (بلا شفافية).</summary>
    public static string ToHex(Color color)
        => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

    /// <summary>
    /// يقرأ <c>#RRGGBB</c> فقط (ست خانات لونية بعد إشارة #، بلا حساسية لحالة الحروف).
    /// أي صيغة أخرى — مثل <c>red</c> أو <c>#FFF</c> — تُعيد <c>false</c>.
    /// </summary>
    public static bool TryParseHex(string? hex, out Color color)
    {
        color = Color.Empty;
        if (string.IsNullOrWhiteSpace(hex)) return false;

        var text = hex.Trim();
        if (text.Length != 7 || text[0] != '#') return false;

        if (!TryParseByte(text, 1, out int r)
            || !TryParseByte(text, 3, out int g)
            || !TryParseByte(text, 5, out int b))
        {
            return false;
        }

        color = Color.FromArgb(r, g, b);
        return true;
    }

    /// <summary>
    /// يقرأ **نص لون** وصل من خارج الشاشة (الحافظة · كتابة يدوية) بصيغ متعددة، ويُعيد اللون:
    /// <list type="bullet">
    /// <item><c>#RRGGBB</c> · <c>RRGGBB</c> · <c>0xRRGGBB</c></item>
    /// <item><c>#AARRGGBB</c> / <c>AARRGGBB</c> — تُتجاهل الشفافية ويُقرأ اللون من الخانات الست الدنيا</item>
    /// <item><c>RGB(r,g,b)</c> / <c>rgb(r, g, b)</c> — بلا حساسية لحالة الحروف وبمسافات حرة</item>
    /// <item><c>r,g,b</c> و <c>r; g; b</c> — كل قناة 0..255 (خارج النطاق ⇒ <c>false</c>)</item>
    /// </list>
    /// <para>
    /// متسامح مع المسافات وعلامات التنصيص حول القيمة ومع الأرقام العربية ٠-٩ (تُطوى إلى 0-9 قبل
    /// القراءة). **المكان الوحيد** لتحويل نص المستخدم إلى لون في النظام كله (RULE-N169…N172):
    /// الشاشات تستدعي هذه الدالة ولا تكتب تحليلاً خاصاً بها. الصيغ الست عشرية تُمرَّر إلى
    /// <see cref="TryParseHex"/> نفسه — لا محلّل ست عشري ثانٍ. لا ترمي استثناءً أبداً.
    /// </para>
    /// </summary>
    public static bool TryParseColorText(string? text, out Color color)
    {
        color = Color.Empty;
        if (string.IsNullOrWhiteSpace(text)) return false;

        try
        {
            string value = NormalizeColorText(text);
            if (value.Length == 0) return false;

            // (1) rgb(r, g, b) — لاحقة/بادئة بلا حساسية لحالة الحروف، ومسافات مسموحة حول rgb وحول الأقواس
            if (value.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
            {
                int open = value.IndexOf('(');
                int close = value.LastIndexOf(')');

                if (open < 0 || close <= open) return false;
                if (!string.Equals(value[..open].Trim(), "rgb", StringComparison.OrdinalIgnoreCase)) return false;
                if (value[(close + 1)..].Trim().Length != 0) return false;

                return TryParseRgbChannels(value[(open + 1)..close], out color);
            }

            // (2) r,g,b  /  r; g; b
            if (value.Contains(',') || value.Contains(';'))
                return TryParseRgbChannels(value, out color);

            // (3) الصيغ الست عشرية — بعد تجريد # و 0x وشفافية AA، يبقى التحليل لـ TryParseHex وحده
            string hex = value;
            if (hex.StartsWith('#')) hex = hex[1..];
            if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) hex = hex[2..];

            if (hex.Length == 8) hex = hex[2..];        // AARRGGBB: تُرمى الشفافية
            else if (hex.Length != 6) return false;     // غير ذلك ليس قيمة لون

            return TryParseHex("#" + hex, out color);
        }
        catch
        {
            // نص غريب لا يُسقط الشاشة — يُعدّ غير مفهوم فقط.
            color = Color.Empty;
            return false;
        }
    }

    /// <summary>قراءة ثلاث قنوات <c>r</c> · <c>g</c> · <c>b</c> من نص مفصول بفاصلة أو فاصلة منقوطة.</summary>
    private static bool TryParseRgbChannels(string channels, out Color color)
    {
        color = Color.Empty;

        var parts = channels.Split(new[] { ',', ';' }, StringSplitOptions.TrimEntries);
        if (parts.Length != 3) return false;

        var values = new int[3];

        for (int i = 0; i < values.Length; i++)
        {
            if (!int.TryParse(parts[i], out int channel)) return false;
            if (channel < 0 || channel > 255) return false;

            values[i] = channel;
        }

        color = Color.FromArgb(values[0], values[1], values[2]);
        return true;
    }

    /// <summary>علامات التنصيص التي تُطوى حول النص الملصوق («#0EA567» · "#0EA567" · '#0EA567').</summary>
    private static readonly char[] QuoteCharacters = { '"', '\'', '`', '«', '»' };

    /// <summary>تطبيع نص اللون: أرقام عربية ٠-٩ ⇒ 0-9، ثم تقليم المسافات وعلامات التنصيص حول القيمة.</summary>
    private static string NormalizeColorText(string text)
    {
        string value = text;

        for (int digit = 0; digit <= 9; digit++)
            value = value.Replace((char)('\u0660' + digit), (char)('0' + digit));

        return value.Trim().Trim(QuoteCharacters).Trim();
    }

    /// <summary>يقرأ خانتين ست عشريتين من <paramref name="start"/> (رفض أي محرف غير ست عشري).</summary>
    private static bool TryParseByte(string text, int start, out int value)
    {
        value = 0;
        int high = HexDigit(text[start]);
        int low = HexDigit(text[start + 1]);
        if (high < 0 || low < 0) return false;

        value = (high << 4) | low;
        return true;
    }

    private static int HexDigit(char c) => c switch
    {
        >= '0' and <= '9' => c - '0',
        >= 'A' and <= 'F' => c - 'A' + 10,
        >= 'a' and <= 'f' => c - 'a' + 10,
        _ => -1
    };

    // ═════════════════════════════════════════════════════════════════
    //  بناء الجداول (نقطة واحدة — لا تكرار خارجها)
    // ═════════════════════════════════════════════════════════════════

    private static Dictionary<string, Color> BuildLive()
    {
        var map = new Dictionary<string, Color>(StringComparer.Ordinal);
        foreach (var definition in AllDefinitions)
            map[definition.Key] = definition.DefaultValue;

        return map;
    }
}
