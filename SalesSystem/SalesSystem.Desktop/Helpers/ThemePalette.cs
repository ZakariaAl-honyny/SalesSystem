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
//   • ممنوع أن يشير أي حقل أو تهيئة ثابتة (field / static initializer) في هذا
//     الملف إلى GreenThemeHelper أو إلى أي عضو في Helpers/ — منعاً للتهيئة
//     الدائرية وقت تحميل الأنواع. والاستثناء المشروع الوحيد هو داخل أجسام
//     الدوال (method bodies) لا في المُهيّئات: ApplyToOpenForms تنادي
//     GreenThemeHelper.Apply و KeyboardHelper.RefreshFocusHighlight بحق،
//     لأن النداء يقع وقت التشغيل بعد اكتمال التهيئة.
//   • لا يوجد أي `Color.FromArgb` خارج **جدولَي الألوان** أدناه (RULE-N112):
//       (أ) AllDefinitions  = الافتراضيات **الحالية** (24 لوناً تُعرَض في شاشة الألوان)،
//       (ب) RetiredDefaults = **تاريخ الافتراضيات المتقاعدة**: كل قيمة افتراضية سبق
//           شحنها ثم تغيّرت — بمفتاحها ولونها القديم حرفياً.
//   • الغرض من RetiredDefaults: لوحة المصمّمين (95 مصمّماً) تحمل القيم الحرفية
//     للافتراضيات **القديمة**، فبعد تغيير الافتراضي يصير لون المصمّم «لوناً خاصاً»
//     ولا يُعاد تلوينه (RULE-N134/RULE-N139). بفضل ThemePalette.IsRetiredDefault
//     يبقى لون المصمّم «لون ثيم» فيُعاد تلوينه فوراً — فلا يلزم تعديل أي
//     `*.Designer.cs` في أي تغيير ألوان مستقبلي: القيمة القديمة تُنقل إلى
//     RetiredDefaults ويُحدَّث الافتراضي، لا أكثر.
//   • التغيير هنا لا يُعيد إنشاء الشاشات: ApplyToOpenForms تُعيد تلوين المفتوح فقط.
// -----------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Drawing;
using System.Text.RegularExpressions;

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
/// <para>
/// وهي أيضاً **البيت الواحد** لقاعدة التحقق من مجموعة ألوان كاملة وتطبيقها ذرّياً
/// (<see cref="ValidateAll"/> / <see cref="TryApplyAll"/>): الرسائل العربية والترتيب
/// وصيغة <c>#RRGGBB</c> وقواعد التباين تعيش هنا مرة واحدة، ويستهلكها أي تنفيذ
/// (وهمي أو حقيقي) بلا نسخة ثانية (RULE-N169 / RULE-N172).
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

    /// <summary>النص المميّز (داكن — يتباين مع خلفية الشاشة).</summary>
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

    /// <summary>
    /// لون التحديد والحقل النشط — **مفتاح واحد لاستخدامين** (قرار المالك 2026-11-15):
    /// سطر/خلية الشبكة المحددة (<c>GreenThemeHelper.ApplyGrid</c>) وحقل الإدخال المركَّز
    /// (<c>KeyboardHelper.WireFocusHighlight</c>) يتشاركان القيمة نفسها.
    /// الافتراضي <c>#FFFFB4</c> = (255,255,180) — والقيمتان السابقتان <c>#FFF3CD</c> = (255,243,205)
    /// و<c>#BAE6FD</c> = (186,230,253) مسجَّلتان في <c>RetiredDefaults</c> فيُعاد تلوين أي مصمّم يحملهما.
    /// </summary>
    public const string SelectionRowColor = "selectionRowColor";

    /// <summary>خلفية العنصر المعطّل.</summary>
    public const string DisabledBackColor = "disabledBackColor";

    /// <summary>نص العنصر المعطّل.</summary>
    public const string DisabledForeColor = "disabledForeColor";

    /// <summary>
    /// خلفية **الرفض/الخطأ** الوحيدة في النظام (قرار المالك 2026-11-16: «غيّر اللون إلى الأحمر»
    /// ثم «مثل كلمة المرور في شاشة الدخول»). يستهلكها <c>clsInputFeedback.ShowRejectTooltip</c>
    /// عبر <see cref="GreenThemeHelper.RejectBackground"/> — وميض قصير يعود إلى لون المصمّم تلقائياً،
    /// فلا يُترك أي حقل ملوّناً. لون **حالة** لا لون تصميم، ولا يُعلَن في أي `*.Designer.cs`
    /// (نفس استثناء `RULE-N154` على `RULE-N134`). الافتراضي `#FFDCDC` = (255,220,220) —
    /// أحمر حقل باهت (المصدر الوحيد له هو
    /// <see cref="AllDefinitions"/>؛ لا يُسمّى هنا جيلٌ بعينه كي لا يشيخ هذا الشرح مع كل
    /// تغيير لوحة — <c>RULE-N188</c>). والقيمة السابقة `#F8D7DA` = (248,215,218) مسجَّلة
    /// في <c>RetiredDefaults</c> فيُعاد تلوين أي شاشة تحملها.
    /// </summary>
    public const string RejectBackgroundColor = "rejectBackgroundColor";

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
        // ── أشرطة الأدوات والصناديق (2) ──
        new(BarBackgroundColor,      "أشرطة الأدوات والحاويات", GroupBars,    Color.FromArgb(235, 240, 235)),
        new(GroupBoxColor,           "صناديق المجموعات",      GroupBars,      Color.FromArgb(235, 245, 235)),
        // ── الشبكة (7) ──
        new(GridBackgroundColor,     "خلفية الشبكة",          GroupGrid,      Color.FromArgb(220, 240, 220)),
        new(GridHeaderColor,         "رؤوس أعمدة الشبكة",     GroupGrid,      Color.FromArgb(14, 165, 103)),
        new(GridHeaderTextColor,     "نص رؤوس الأعمدة",       GroupGrid,      Color.White),
        new(GridLineColor,           "خطوط الشبكة",           GroupGrid,      Color.FromArgb(205, 225, 205)),
        new(GridAlternateRowColor,   "الأسطر المتبادلة",      GroupGrid,      Color.FromArgb(245, 252, 245)),
        new(GridCellBackgroundColor, "خلفية خلايا الشبكة",    GroupGrid,      Color.White),
        new(GridCellTextColor,       "نص خلايا الشبكة",       GroupGrid,      Color.FromArgb(33, 37, 41)),
        // ── التحديد والحقل النشط (1) ──
        new(SelectionRowColor,       "لون التحديد والحقل النشط", GroupSelection, Color.FromArgb(255, 255, 180)),
        // ── حالات العرض (3) ──
        new(DisabledBackColor,       "خلفية العنصر المعطّل",  GroupStates,    Color.FromArgb(224, 224, 224)),
        new(DisabledForeColor,       "نص العنصر المعطّل",     GroupStates,    Color.FromArgb(150, 150, 150)),
        new(RejectBackgroundColor,   "خلفية الرفض والخطأ",   GroupStates,    Color.FromArgb(255, 220, 220)),
    };

    // ═════════════════════════════════════════════════════════════════
    //  تاريخ الافتراضيات المتقاعدة (Retired defaults) — RULE-N169/N172
    //  كل لون كان **افتراضياً** ثم تغيّر يُسجَّل هنا بمفتاحه وقيمته القديمة
    //  حرفياً. سبب وجوده: مصمّمات الشاشات تحمل القيمة الحرفية للافتراضي وقت
    //  التصميم، فبعد تغيير الافتراضي يصير لون المصمّم «لوناً خاصاً» لا «لون ثيم»
    //  فلا يُعاد تلوينه (RULE-N134/RULE-N139). بـ IsRetiredDefault يبقى لون
    //  المصمّم معروفاً كافتراضي **سبق شحنه** فيُعاد تلوينه — فلا يلزم مسّ أي
    //  *.Designer.cs عند أي تغيير ألوان مستقبلي (قرار المالك 2026-11-16).
    //  ويحمل الجدول الآن **ثلاثة أجيال متقاعدة**: الأول (أخضر → محايد) والثاني
    //  (محايد → أزرق) والثالث (أزرق → العودة للأخضر) — والمطابقة **بالمفتاح والقيمة
    //  معاً**، فتكرار قيمةٍ واحدة تحت مفتاحين مختلفين مقصود ولا يُدمَج.
    //  القاعدة عند أي تغيير جديد: أضف (المفتاح، القيمة القديمة) هنا ثم حدّث
    //  الافتراضي في AllDefinitions — لا شيء غير ذلك.
    // ═════════════════════════════════════════════════════════════════

    private static readonly (string Key, Color Value)[] RetiredDefaults =
    {
        // ── الجيل الأول: أخضر → محايد ──
        (FrmMainColor,            Color.FromArgb(170, 220, 170)),   // → 240, 242, 245
        (HeaderBarColor,          Color.FromArgb(14, 165, 103)),    // → 16, 150, 96
        (TextStrongColor,         Color.FromArgb(14, 82, 52)),      // → 33, 37, 41
        (BarBackgroundColor,      Color.FromArgb(235, 240, 235)),   // → White
        (GroupBoxColor,           Color.FromArgb(235, 245, 235)),   // → White
        (GridBackgroundColor,     Color.FromArgb(220, 240, 220)),   // → White
        (GridHeaderColor,         Color.FromArgb(14, 165, 103)),    // → 52, 58, 64
        (GridLineColor,           Color.FromArgb(205, 225, 205)),   // → 222, 226, 230
        (GridAlternateRowColor,   Color.FromArgb(245, 252, 245)),   // → 248, 249, 250
        (SelectionRowColor,       Color.FromArgb(255, 255, 180)),   // → 255, 243, 205
        (DisabledBackColor,       Color.FromArgb(224, 224, 224)),   // → 233, 236, 239
        (DisabledForeColor,       Color.FromArgb(150, 150, 150)),   // → 108, 117, 125
        (RejectBackgroundColor,   Color.FromArgb(255, 220, 220)),   // → 248, 215, 218
        // ── الجيل الثاني: محايد → أزرق ──
        (FrmMainColor,            Color.FromArgb(240, 242, 245)),   // → 238, 242, 246
        (HeaderBarColor,          Color.FromArgb(16, 150, 96)),     // → 15, 45, 74
        (TextStrongColor,         Color.FromArgb(33, 37, 41)),      // → 15, 23, 42
        (BtnAddNewButtonColor,    Color.FromArgb(13, 110, 253)),    // → 2, 119, 189
        (BtnSaveButtonColor,      Color.FromArgb(25, 135, 84)),     // → 5, 150, 105
        (BtnDeleteButtonColor,    Color.FromArgb(220, 53, 69)),     // → 225, 29, 72
        (BtnCancelButtonColor,    Color.FromArgb(108, 117, 125)),   // → 100, 116, 139
        (BtnInfoButtonColor,      Color.FromArgb(23, 162, 184)),    // → 8, 145, 178
        (BtnWarningButtonColor,   Color.FromArgb(255, 193, 7)),     // → 245, 158, 11
        (BtnSecondaryButtonColor, Color.FromArgb(248, 249, 250)),   // → 248, 250, 252
        (GridHeaderColor,         Color.FromArgb(52, 58, 64)),      // → 15, 45, 74
        (GridLineColor,           Color.FromArgb(222, 226, 230)),   // → 226, 232, 240
        (GridAlternateRowColor,   Color.FromArgb(248, 249, 250)),   // → 248, 250, 252
        (GridCellTextColor,       Color.FromArgb(33, 37, 41)),      // → 15, 23, 42
        (SelectionRowColor,       Color.FromArgb(255, 243, 205)),   // → 186, 230, 253
        (DisabledBackColor,       Color.FromArgb(233, 236, 239)),   // → 226, 232, 240
        (DisabledForeColor,       Color.FromArgb(108, 117, 125)),   // → 100, 116, 139
        // ── تصحيحات مطابقة لوحة المصدر (4) ──
        (FrmMainColor,            Color.FromArgb(238, 242, 246)),   // → 240, 244, 249
        (HeaderBarColor,          Color.FromArgb(15, 45, 74)),      // → 10, 75, 140
        (TextStrongColor,         Color.FromArgb(15, 23, 42)),      // → 30, 41, 59
        (GridCellTextColor,       Color.FromArgb(15, 23, 42)),      // → 30, 41, 59
        // ── تصحيح لون زر «معلومات» إلى لون المصدر (sky-600) ──
        (BtnInfoButtonColor,      Color.FromArgb(8, 145, 178)),     // → 2, 132, 199
        // ── الجيل الثالث: أزرق «المحاسب سوفت» → العودة للأخضر (2026-11-16) ──
        (FrmMainColor,            Color.FromArgb(240, 244, 249)),
        (HeaderBarColor,          Color.FromArgb(10, 75, 140)),
        (TextStrongColor,         Color.FromArgb(30, 41, 59)),
        (GridCellTextColor,       Color.FromArgb(30, 41, 59)),
        (GridHeaderColor,         Color.FromArgb(15, 45, 74)),
        (GridLineColor,           Color.FromArgb(226, 232, 240)),
        (GridAlternateRowColor,   Color.FromArgb(248, 250, 252)),
        (SelectionRowColor,       Color.FromArgb(186, 230, 253)),
        (DisabledBackColor,       Color.FromArgb(226, 232, 240)),
        (DisabledForeColor,       Color.FromArgb(100, 116, 139)),
        (RejectBackgroundColor,   Color.FromArgb(248, 215, 218)),
        (BtnAddNewButtonColor,    Color.FromArgb(2, 119, 189)),
        (BtnSaveButtonColor,      Color.FromArgb(5, 150, 105)),
        (BtnDeleteButtonColor,    Color.FromArgb(225, 29, 72)),
        (BtnCancelButtonColor,    Color.FromArgb(100, 116, 139)),
        (BtnInfoButtonColor,      Color.FromArgb(2, 132, 199)),
        (BtnWarningButtonColor,   Color.FromArgb(245, 158, 11)),
        (BtnSecondaryButtonColor, Color.FromArgb(248, 250, 252)),
    };

    /// <summary>
    /// هل <paramref name="color"/> قيمة افتراضية **متقاعدة** (سبق أن شحنها النظام ثم
    /// تغيّرت) للمفتاح <paramref name="key"/>؟
    /// <para>
    /// تُطابق المفتاح **والقيمة معاً** بمقارنة ترتيبية — فلا يعود لون مفتاحٍ آخر
    /// افتراضياً لمفتاح مختلف. لا ترمي استثناءً أبداً: مفتاح <c>null</c> أو فراغي
    /// يُعيد <c>false</c> (العرض لا يتوقّف بسبب قيمة مشوّهة).
    /// </para>
    /// <para>
    /// <b>المستهلك الوحيد:</b> <c>GreenThemeHelper.IsThemeColor</c> و
    /// <c>GreenThemeHelper.IsPaletteOwnedColor</c> — لتمييز لون مصمّمٍ مكتوبٍ بقيمة
    /// افتراضية قديمة عن لون مصمّم مخصّص (RULE-N139/RULE-N142).
    /// </para>
    /// </summary>
    internal static bool IsRetiredDefault(string key, Color color)
    {
        if (string.IsNullOrEmpty(key)) return false;

        int argb = color.ToArgb();

        foreach (var retired in RetiredDefaults)
        {
            if (string.Equals(retired.Key, key, StringComparison.Ordinal)
                && retired.Value.ToArgb() == argb)
            {
                return true;
            }
        }

        return false;
    }

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
        // الشريط لون واحد مسطّح، فقاعدة واحدة تحمي النص الأبيض عليه.
        Add(HeaderTextColor, HeaderBarColor, 3.0);
        Add(HeaderBarColor, HeaderTextColor, 3.0);
        Add(GridHeaderTextColor, GridHeaderColor, 3.0);
        Add(GridHeaderColor, GridHeaderTextColor, 3.0);
        Add(GridCellTextColor, GridCellBackgroundColor, BodyTextMinimumContrast);
        Add(GridCellBackgroundColor, GridCellTextColor, BodyTextMinimumContrast);
        Add(SelectionRowColor, GridCellTextColor, BodyTextMinimumContrast);
        Add(RejectBackgroundColor, GridCellTextColor, BodyTextMinimumContrast);
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
    /// <para>
    /// <b>هذه هي القراءة الوحيدة لمقام <see cref="PreviousLive"/></b> في الملف كله
    /// (<c>RULE-N171</c>): يستهلكها <c>GreenThemeHelper.IsThemeColor</c> لتمييز «لون ثيم قديم»
    /// عن «لون مصمّم مخصّص» (<c>RULE-N139</c>)، ويستهلكها مُحلِّل مفتاح الزر
    /// <see cref="ButtonKeyFor"/> في خطوته الثالثة — فلا نسخة ثانية من مطابقة «القيمة السابقة»
    /// في أي مكان. داخلية، وليست جزءاً من عقد اللوحة العام.
    /// </para>
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

    // ─────────────────────────────────────────────────────────────────
    //  مُحلِّل مفتاح الزر — البيت الواحد لقرار «هل هذا الزر بلون اللوحة؟»
    // ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// **مُحلِّل مفتاح الزر**: أي مفتاح من <see cref="ButtonKeys"/> يملك خلفية هذا الزر؟
    /// <para>
    /// <b>ترتيب المطابقة ملزم، وأول مطابقة تفوز</b> (RULE-N169…RULE-N172 — لا نسخة
    /// ثانية من هذا الحكم في أي ملف، ولا لون حرفي هنا):
    /// </para>
    /// <list type="number">
    /// <item>(1) القيمة الحيّة <see cref="Get"/> — زر أعادت اللوحة تلوينه في تطبيق سابق،
    /// فالخلفية الحالية هي القيمة الحيّة نفسها.</item>
    /// <item>(2) الافتراضي المشحون <see cref="DefaultFor"/> — وهو **اللون الحرفي** في
    /// <c>*.Designer.cs</c>، فالأزرار تُعرَف من أول فتح بلا مسّ أي مصمّم (<c>RULE-N134</c>).</item>
    /// <item>(3) آخر قيمة طبّقتها اللوحة <see cref="WasAppliedValue"/> — فتتبع الأزرار
    /// المفتوحة تغييراً ثانياً وثالثاً بلا إعادة فتح الشاشة.</item>
    /// <item>(4) افتراضي **متقاعد** <see cref="IsRetiredDefault"/> — لون مصمّمٍ مكتوب بقيمة
    /// افتراضية سبق شحنها ثم تغيّرت: يبقى «لون ثيم» فيُعاد تلوينه (<c>RULE-N139</c>).
    /// وهذه هي الخطوة التي كانت **غائبة**: زر بخلفية افتراضي متقاعد كان يبقى مجمَّداً
    /// على اللون القديم إلى الأبد لأن المُحوِّل القديم استشار «القيمة السابقة» وحدها.</item>
    /// </list>
    /// <para>
    /// <b>ولون المصمّم المخصّص يبقى سليماً:</b> إن لم يطابق أيّاً من الأربعة ⇒ <c>null</c>
    /// ⇒ لا كتابة على الزر إطلاقاً، فالألوان المخصّصة خارج لوحة الأزرار لا تُمسّ
    /// (<c>RULE-N142</c>/<c>RULE-N139</c>). وهذا هو **الحارس الوحيد** لبقائها.
    /// </para>
    /// </summary>
    /// <param name="color">خلفية الزر كما هي الآن.</param>
    /// <returns>مفتاح الزر المالك للون، أو <c>null</c> إن كان لوناً خاصاً بالمصمّم.</returns>
    internal static string? ButtonKeyFor(Color color)
    {
        int argb = color.ToArgb();

        // (1) القيمة الحيّة للمفتاح.
        foreach (var key in ButtonKeys)
        {
            if (Get(key).ToArgb() == argb) return key;
        }

        // (2) الافتراضي المشحون = اللون الحرفي الذي كتبه المصمّم.
        foreach (var key in ButtonKeys)
        {
            if (DefaultFor(key).ToArgb() == argb) return key;
        }

        // (3) آخر قيمة طبّقتها اللوحة عليه (القراءة عبر WasAppliedValue وحدها).
        foreach (var key in ButtonKeys)
        {
            if (WasAppliedValue(key, color)) return key;
        }

        // (4) افتراضي متقاعد: اللون القديم يُعاد تلوينه بلا تعديل المصمّم.
        foreach (var key in ButtonKeys)
        {
            if (IsRetiredDefault(key, color)) return key;
        }

        return null;
    }

    /// <summary>
    /// يمشي على كل أزرار شجرة العناصر ويعيد تلوين ما كان يحمل لون زرٍّ من اللوحة
    /// بالقيمة الحيّة — و`CheckBox`/`RadioButton` مستثناة صريحاً، والنص (<c>ForeColor</c>)
    /// وحالة الزر و<see cref="Control.Enabled"/> لا تُلمس أبداً لأن الزر الثانوي فاتح
    /// بنص داكن (RULE-N142).
    /// <para>
    /// <b>نقطة الدخول الوحيدة لهذا المشي</b> (<c>RULE-N171</c>):
    /// <see cref="ApplyToOpenForms"/> بعد حفظ ألوان جديدة، و
    /// <see cref="GreenThemeHelper.Apply(Form)"/> **عند كل فتح شاشة** — فالأزرار تتبع
    /// اللوحة دائماً، لا بعد «حفظ الألوان» فقط.
    /// </para>
    /// </summary>
    internal static void RepaintThemeButtons(Control parent)
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
    /// زر واحد: يُحلّ مفتاحه بـ <see cref="ButtonKeyFor"/> ثم تُقارن قيمته الحيّة بخلفية الزر —
    /// إن اختلفتا تُكتب الخلفية الحيّة وتُضبط خلفية المرور بنفس قاعدة
    /// <c>ThemeHelper.ApplyButtonStyle</c> (<c>ControlPaint.Light(color, 0.1f)</c>)،
    /// وإن تساوتا فلا لمس إطلاقاً.
    /// <para>
    /// <c>null</c> من المُحلِّل = اللون ليس من لوحة الأزرار (لا حيّاً ولا افتراضياً ولا سابقاً
    /// ولا متقاعداً) ⇒ لون مصمّم مخصّص ⇒ يُترك الزر كما هو (<c>RULE-N142</c>).
    /// </para>
    /// </summary>
    private static void RepaintThemeButton(Button button)
    {
        if (ButtonKeyFor(button.BackColor) is not { } key) return;

        var current = Get(key);

        // اللون الحيّ نفسه ⇒ لا لمس (يبقى المرور المخصّص كما هو).
        if (current.ToArgb() == button.BackColor.ToArgb()) return;

        button.BackColor = current;
        button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(current, 0.1f);
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
    //  التحقق من مجموعة ألوان كاملة وتطبيقها ذرّياً — البيت الواحد
    //  (نُقلت هنا حرفياً من الخدمة الوهمية — RULE-N169 / RULE-N172)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// الصيغة الوحيدة المقبولة لقيمة اللون (<c>#RRGGBB</c>) — العبارة النمطية الوحيدة
    /// لفحص قيمة لون في النظام كله (RULE-N172): لا نسخة ثانية في أي خدمة أو شاشة.
    /// </summary>
    private static readonly Regex HexPattern = new("^#[0-9A-Fa-f]{6}$", RegexOptions.Compiled);

    /// <summary>
    /// يتحقّق من **مجموعة ألوان كاملة** (24 مفتاحاً بصيغة <c>#RRGGBB</c>) ويُعيد **أول**
    /// رسالة رفض عربية، أو <c>null</c> إن كانت المجموعة كلها مقبولة.
    /// <para>
    /// <b>البيت الواحد (RULE-N169 · RULE-N172):</b> كانت هذه الفحوص الخمسة مكتوبة داخل
    /// <c>MockThemeColorDesktopService</c>؛ نُقلت إلى هنا بالترتيب نفسه والصياغة العربية
    /// نفسها، فتُستهلك من أي تنفيذ (وهمي أو حقيقي) بلا نسخة ثانية.
    /// </para>
    /// <para>
    /// الترتيب ملزم: (1) طلب فارغ · (2) مفتاح من <see cref="Definitions"/> مفقود أو فارغ ·
    /// (3) مفتاح غير معروف · (4) قيمة لا تطابق <see cref="HexPattern"/> أو لا تُقرأ بـ
    /// <see cref="TryParseHex"/> · (5) تباين ضعيف بين النص وشريكه وفق
    /// <see cref="MinimumContrastFor"/> و<see cref="ContrastPartnerKey"/> و<see cref="ContrastRatioRaw"/>.
    /// </para>
    /// </summary>
    /// <param name="colors">المفاتيح وقيمها كما وصلت من الشاشة (قد تكون <c>null</c>).</param>
    /// <returns>أول رسالة رفض عربية، أو <c>null</c> إن كانت المجموعة مقبولة.</returns>
    public static string? ValidateAll(IReadOnlyDictionary<string, string>? colors)
    {
        // (1) لا شيء للحفظ
        if (colors is null || colors.Count == 0)
            return "لا توجد ألوان للحفظ.";

        // (2) مفتاح من التعريفات مفقود من الطلب
        foreach (var definition in AllDefinitions)
        {
            if (!colors.TryGetValue(definition.Key, out var value)
                || string.IsNullOrWhiteSpace(value))
            {
                return $"اللون المطلوب غير موجود: {definition.ArabicLabel} ({definition.Key}).";
            }
        }

        // (3) مفتاح غير معروف (ليس من التعريفات)
        var known = new HashSet<string>(AllDefinitions.Select(d => d.Key), StringComparer.Ordinal);
        foreach (var key in colors.Keys)
        {
            if (!known.Contains(key))
                return $"مفتاح لون غير معروف: {key}.";
        }

        // (4) قيمة ليست بصيغة #RRGGBB
        var parsed = new Dictionary<string, Color>(StringComparer.Ordinal);
        foreach (var definition in AllDefinitions)
        {
            string hex = colors[definition.Key];

            if (!HexPattern.IsMatch(hex) || !TryParseHex(hex, out var color))
                return $"قيمة اللون غير صالحة لـ «{definition.ArabicLabel}». استخدم الصيغة #RRGGBB.";

            parsed[definition.Key] = color;
        }

        // (5) تباين ضعيف بين النص وخلفيته
        return ValidateContrast(parsed);
    }

    /// <summary>
    /// يُطبِّق مجموعة ألوان **ذرّياً**: يتحقّق منها أولاً بـ <see cref="ValidateAll"/> ويُحوِّل
    /// كل قيمها، ولا ينادي <see cref="TrySet"/> إلا بعد أن تصير كل المفاتيح صالحة — فلا تطبيق
    /// جزئي أبداً: مجموعة فيها قيمة واحدة خاطئة لا تغيّر أي لون.
    /// <para>
    /// <b>العقد:</b> <c>string?</c> = أول رسالة رفض عربية، أو <c>null</c> عند النجاح — نفس عقد
    /// <c>clsFormat.TryApplyAll</c> (RULE-N171: شكل واحد لسلوك واحد). رسالة الرفض هي **نفسها**
    /// التي أنتجها <see cref="ValidateAll"/> لهذه المجموعة؛ وإن نجح التحقق وفشل
    /// <see cref="TrySet"/> لسبب آخر فتُعاد رسالة التطبيق العامة أدناه.
    /// </para>
    /// </summary>
    /// <param name="colors">المفاتيح وقيمها كما وصلت من الشاشة (قد تكون <c>null</c>).</param>
    /// <returns>أول رسالة رفض عربية، أو <c>null</c> إذا طُبِّقت المجموعة كاملة.</returns>
    public static string? TryApplyAll(IReadOnlyDictionary<string, string>? colors)
    {
        // (1) كل الفحوص الخمسة ورسائلها العربية من البيت الواحد — تُعاد كما هي بلا نسخة ثانية
        var validationError = ValidateAll(colors);
        if (validationError is not null || colors is null) return validationError;

        // (2) تحويل كل القيم قبل أي TrySet — لا تطبيق جزئي
        var parsed = new Dictionary<string, Color>(AllDefinitions.Length, StringComparer.Ordinal);
        foreach (var definition in AllDefinitions)
        {
            if (!TryParseHex(colors[definition.Key], out var color)) return ApplyFailureMessage;
            parsed[definition.Key] = color;
        }

        // (3) التطبيق دفعة واحدة — أي فشل في TrySet = رسالة التطبيق العامة
        bool allApplied = true;
        foreach (var pair in parsed)
            allApplied &= TrySet(pair.Key, pair.Value);

        return allApplied ? null : ApplyFailureMessage;
    }

    /// <summary>
    /// رسالة فشل التطبيق العامة (بعد نجاح التحقق) — تُعلَن هنا مرة واحدة، فلا تعيد أي خدمة
    /// كتابة نصها (RULE-N169 / RULE-N171).
    /// </summary>
    private const string ApplyFailureMessage = "تعذر تطبيق بعض الألوان. يرجى إعادة المحاولة.";

    /// <summary>
    /// يطبّق لوحة **محفوظة** مع التوافق الرجعي: أي مفتاح جديد أُضيف بعد الحفظ يأخذ افتراضيه،
    /// وأي مفتاح غير معروف يُتجاهل — بدل رفض اللوحة كلها وضياع ألوان المالك عند الإقلاع.
    /// لا يُستهلك في مسار الحفظ: هناك يبقى <see cref="ValidateAll"/> الصارم (RULE-N167).
    /// <para>
    /// البناء: ركيزة كاملة من افتراضيات <see cref="Definitions"/> (كل قيمة تُحوَّل بـ
    /// <see cref="ToHex(Color)"/> — المنسّق الوحيد في النظام، لا مُسلسِل ثانٍ)، ثم تراكب
    /// المحفوظ للمفاتيح المعروفة فقط، ثم <see cref="TryApplyAll"/> نفسه (تحقّق + تحويل + تطبيق ذرّي).
    /// </para>
    /// </summary>
    /// <param name="colors">القيم المحفوظة (قد تكون <c>null</c> أو ناقصة أو تحمل مفاتيح قديمة).</param>
    /// <returns>رسالة الرفض العربية إن فشل التطبيق، أو <c>null</c> عند النجاح/عدم وجود ما يُطبَّق.</returns>
    public static string? TryApplyStored(IReadOnlyDictionary<string, string>? colors)
    {
        // لا شيء محفوظ (أول تشغيل أو صفوف غائبة) ⇒ الافتراضيات قائمة بالفعل، لا نلمس شيئاً.
        if (colors is null || colors.Count == 0) return null;

        try
        {
            // (1) ركيزة كاملة من الافتراضيات — بصيغة #RRGGBB عبر المنسّق الوحيد
            var merged = new Dictionary<string, string>(AllDefinitions.Length, StringComparer.Ordinal);
            foreach (var definition in AllDefinitions)
                merged[definition.Key] = ToHex(definition.DefaultValue);

            // (2) تراكب المحفوظ: المفاتيح المعروفة فقط، وغير المعروف/الفراغ يُتجاهل
            foreach (var pair in colors)
            {
                if (pair.Key is null || string.IsNullOrWhiteSpace(pair.Value)) continue;
                if (!DefinitionsByKey.ContainsKey(pair.Key)) continue;
                if (!TryParseHex(pair.Value, out _)) continue;   // قيمة مشوّهة ⇒ يبقى الافتراضي، لا تُسقط اللوحة كلها

                merged[pair.Key] = pair.Value.Trim();
            }

            // (3) التطبيق الذرّي نفسه — لا نسخة ثانية من التحقق ولا من التحويل (RULE-N171)
            return TryApplyAll(merged);
        }
        catch
        {
            // لا نُسقط الإقلاع بسبب لوحة محفوظة مشوّهة — البقاء على الافتراضيات أسلم.
            return null;
        }
    }

    // ── التباين: العتبات والمفاتيح تأتي من خريطة ContrastRules — لا ثوابت محلية ──

    /// <summary>
    /// يمرّ على كل تعريفات اللوحة ويطبّق **قاعدة اللوحة نفسها**: لكل مفتاح له حد أدنى
    /// (<see cref="MinimumContrastFor"/>) وشريك (<see cref="ContrastPartnerKey"/>)
    /// يُقاس التباين بـ <see cref="ContrastRatioRaw"/> — لا قاعدة ولا عتبة محلية هنا.
    /// <para>
    /// <b>لماذا الطلب لا القيمة الحيّة:</b> قد تُغيّر المسودة مفتاح النص **و**شريكه الخلفي
    /// في نفس الحفظ، فلا يجوز الحكم على زوج (جديد، قديم) — يُقاس الزوج (جديد، جديد)
    /// من الطلب المقدَّم نفسه؛ والقيمة الحيّة تُستخدم فقط إذا لم يكن الشريك في الطلب
    /// (لا يحدث عملياً لأن كل المفاتيح إلزامية، لكنها تُبقي الدالة آمنة).
    /// </para>
    /// يرجع أول رسالة رفض عربية، أو <c>null</c> إن كانت كل الأزواج مقبولة.
    /// </summary>
    private static string? ValidateContrast(IReadOnlyDictionary<string, Color> colors)
    {
        foreach (var definition in AllDefinitions)
        {
            double minimum = MinimumContrastFor(definition.Key);
            if (minimum <= 0) continue;                       // لا متطلب لهذا المفتاح

            if (!colors.TryGetValue(definition.Key, out var candidate)) continue;

            string? partnerKey = ContrastPartnerKey(definition.Key);
            if (partnerKey is null) continue;                 // لا شريك لهذا المفتاح

            var partner = colors.TryGetValue(partnerKey, out var submitted)
                ? submitted                                   // ★ الزوج (جديد، جديد) — المسودة كاملة
                : Get(partnerKey);                            // احتياط: القيمة الحيّة

            if (ContrastRatioRaw(candidate, partner) < minimum)
                return _ContrastMessage(definition.Key, definition.ArabicLabel);
        }

        return null;
    }

    /// <summary>
    /// الرسائل العربية الخمس الأصلية مربوطة بمفتاحها؛ وأي مفتاح آخر له متطلب تباين
    /// يأخذ رسالة عامة بنفس الصياغة (لا تُضاف رسالة لكل لون في الشاشة).
    /// </summary>
    private static string _ContrastMessage(string key, string arabicLabel) => key switch
    {
        HeaderTextColor
            => "تباين نص شريط العنوان مع خلفيته ضعيف — اختر لوناً أغمق أو أفتح.",

        GridHeaderTextColor
            => "تباين نص رؤوس الأعمدة مع خلفيتها ضعيف — اختر لوناً أغمق أو أفتح.",

        GridCellTextColor
            => "تباين نص الخلايا مع خلفية الشبكة ضعيف — اختر لوناً أغمق أو أفتح.",

        TextStrongColor
            => "تباين النص المميّز مع خلفية الشاشة ضعيف — اختر لوناً أغمق أو أفتح.",

        DisabledForeColor
            => "تباين نص العنصر المعطّل مع خلفيته ضعيف.",

        _ => $"تباين «{arabicLabel}» مع اللون المرتبط به ضعيف — اختر لوناً أغمق أو أفتح."
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
