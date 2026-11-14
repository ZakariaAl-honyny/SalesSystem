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
    private const string GroupStates = "حالات العرض";

    private static readonly ThemeColorDefinition[] AllDefinitions =
    {
        new(FrmMainColor,            "خلفية الشاشة الرئيسية", GroupMain,    Color.FromArgb(170, 220, 170)),
        new(HeaderBarColor,          "شريط العنوان",          GroupMain,    Color.FromArgb(14, 165, 103)),
        new(HeaderTextColor,         "نص شريط العنوان",       GroupMain,    Color.White),
        new(TextStrongColor,         "النص المميّز",          GroupMain,    Color.FromArgb(14, 82, 52)),
        new(BtnAddNewButtonColor,    "زر «جديد / إضافة»",     GroupButtons, Color.FromArgb(13, 110, 253)),
        new(BtnSaveButtonColor,      "زر «حفظ»",              GroupButtons, Color.FromArgb(25, 135, 84)),
        new(BtnDeleteButtonColor,    "زر «حذف»",              GroupButtons, Color.FromArgb(220, 53, 69)),
        new(BtnCancelButtonColor,    "زر «إغلاق / إلغاء»",    GroupButtons, Color.FromArgb(108, 117, 125)),
        new(BtnInfoButtonColor,      "زر «تصدير / معلومات»",  GroupButtons, Color.FromArgb(23, 162, 184)),
        new(BtnWarningButtonColor,   "زر «تحذير»",            GroupButtons, Color.FromArgb(255, 193, 7)),
        new(BtnSecondaryButtonColor, "زر «ثانوي»",            GroupButtons, Color.FromArgb(248, 249, 250)),
        new(BarBackgroundColor,      "أشرطة الأدوات والحاويات", GroupBars,  Color.FromArgb(235, 240, 235)),
        new(GroupBoxColor,           "صناديق المجموعات",      GroupBars,    Color.FromArgb(235, 245, 235)),
        new(GridBackgroundColor,     "خلفية الشبكة",          GroupGrid,    Color.FromArgb(220, 240, 220)),
        new(GridHeaderColor,         "رؤوس أعمدة الشبكة",     GroupGrid,    Color.FromArgb(14, 165, 103)),
        new(GridHeaderTextColor,     "نص رؤوس الأعمدة",       GroupGrid,    Color.White),
        new(GridLineColor,           "خطوط الشبكة",           GroupGrid,    Color.FromArgb(205, 225, 205)),
        new(GridAlternateRowColor,   "الأسطر المتبادلة",      GroupGrid,    Color.FromArgb(245, 252, 245)),
        new(GridCellBackgroundColor, "خلفية خلايا الشبكة",    GroupGrid,    Color.White),
        new(GridCellTextColor,       "نص خلايا الشبكة",       GroupGrid,    Color.FromArgb(33, 37, 41)),
        new(SelectionRowColor,       "لون السطر المحدد",      GroupGrid,    Color.FromArgb(255, 255, 180)),
        new(DisabledBackColor,       "خلفية العنصر المعطّل",  GroupStates,  Color.FromArgb(224, 224, 224)),
        new(DisabledForeColor,       "نص العنصر المعطّل",     GroupStates,  Color.FromArgb(150, 150, 150)),
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

    /// <summary>تعريفات اللوحة (23 لوناً بترتيب العرض في الشاشة).</summary>
    public static IReadOnlyList<ThemeColorDefinition> Definitions => DefinitionsReadOnly;

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
