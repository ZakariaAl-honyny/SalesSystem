// -----------------------------------------------------------------------------
// GreenThemeHelper.cs — الثيم الأخضر الموحّد لكل شاشات النظام (RULE-N50…RULE-N54)
//
// المصدر الوحيد لقيم الثيم الأخضر. ممنوع تكرار هذه الأرقام داخل أي شاشة؛
// الشاشات تستهلك الثوابت من هنا فقط (RULE-N54).
//
// ⚠️ ترتيب التطبيق إلزامي (RULE-N50):
//     ThemeHelper.ApplyMainFormStyle(this)   ← يفرض خلفية بيضاء وخط Segoe UI
//     ThemeHelper.ApplyButtonStyle(...)      ← يفرض Segoe UI على الأزرار
//     GreenThemeHelper.Apply(this)           ← يُستدعى **أخيراً** ليردّ الأخضر
//   لأن ThemeHelper يعمل في الاتجاه المعاكس (يُلغي ألوان المصمّم)، فأي تطبيق
//   للثيم الأخضر قبله سيُمحى بلا أثر.
//
// ⚠️ الشبكة الخضراء: بعد ApplyGrid(dgv) **يُمنع** استدعاء
//   ThemeHelper.ApplyDataGridViewStyle(dgv) — فهو يفرض رؤوساً رمادية
//   (248,249,250) وخط Segoe UI وخلفية بيضاء، فيمحو الثيم الأخضر (RULE-N51).
//
// ما لا تلمسه هذه الأداة (مملوك لكل شاشة حسب RULE-N22/RULE-N22a):
//   ReadOnly · AllowUserToAddRows · SelectionMode · MultiSelect · DataSource
//   وحالة أزرار الشاشة وآلة الحالة، وربط الأحداث، والخطوط الفردية لكل عنصر.
//
// حالة التعطيل (RULE-N43): Apply(container) تُوصِّل بعد Walk مظهر التعطيل الموحّد
//   KeyboardHelper.WireDisabledVisuals — كل حقل إدخال/زر معطّل يأخذ DisabledBack
//   و DisabledFore ويعود للونه الطبيعي عند التفعيل، بلا أي كود داخل الشاشة.
//
// الاستخدام النموذجي:
//     KeyboardHelper.WireEnterAsTab(this);
//     ...
//     ApplyStyles();                    // ينتهي بـ GreenThemeHelper.Apply(this)
//     GreenThemeHelper.ApplyGrid(dgvItems);
//     dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect; // إن كانت شاشة إدخال
// -----------------------------------------------------------------------------

using System.Drawing;

namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// الثيم الأخضر الرسمي للنظام: لوحة ألوان واحدة + أدوات تطبيق متكررة وآمنة،
/// مأخوذة حرفياً من شاشة الفاتورة المرجعية <c>frmAddUpdateSalesInvoice</c>.
/// </summary>
public static class GreenThemeHelper
{
    // ── لوحة الألوان (المصدر الوحيد — RULE-N54) ─────────────────────
    /// <summary>خلفية النموذج.</summary>
    public static readonly Color FormBackground = Color.FromArgb(170, 220, 170);

    /// <summary>شريط العنوان ورؤوس أعمدة الشبكة.</summary>
    public static readonly Color HeaderBackground = Color.FromArgb(14, 165, 103);

    /// <summary>نص شريط العنوان ورؤوس الأعمدة.</summary>
    public static readonly Color HeaderForeground = Color.White;

    /// <summary>خلفية صناديق المجموعات (GroupBox) ولوحات الإدخال.</summary>
    public static readonly Color GroupBackground = Color.FromArgb(235, 245, 235);

    /// <summary>خلفية أشرطة الأدوات/الإجراءات ولوحات الحاويات.</summary>
    public static readonly Color BarBackground = Color.FromArgb(235, 240, 235);

    /// <summary>خلفية الشبكة.</summary>
    public static readonly Color GridBackground = Color.FromArgb(220, 240, 220);

    /// <summary>لون خطوط الشبكة.</summary>
    public static readonly Color GridLine = Color.FromArgb(205, 225, 205);

    /// <summary>لون الأسطر المتبادلة.</summary>
    public static readonly Color GridAlternate = Color.FromArgb(245, 252, 245);

    /// <summary>لون النص المميّز (أخضر داكن).</summary>
    public static readonly Color TextStrong = Color.FromArgb(14, 82, 52);

    /// <summary>خلفية الخلية/السطر المحدد.</summary>
    public static readonly Color SelectionBack = Color.FromArgb(255, 255, 180);

    /// <summary>خلفية إجراء معلوماتي (تركوازي) — زر «تصدير» في الشاشة المرجعية.</summary>
    public static readonly Color ActionInfoBackground = Color.FromArgb(23, 162, 184);

    /// <summary>خلفية إجراء محايد (رمادي) — زر «إغلاق» (زر الخروج في الشاشة المرجعية).</summary>
    public static readonly Color ActionNeutralBackground = Color.FromArgb(108, 117, 125);

    // ── ألوان حالة التعطيل (RULE-N43) — الاستثناء الوحيد لألوان وقت التشغيل ─────
    /// <summary>
    /// خلفية العنصر **المعطّل** (حقل إدخال أو زر) — رمادية موحّدة لكل النظام.
    /// <para>
    /// هذا اللون وهزيله <see cref="DisabledFore"/> هما **الاستثناء الوحيد** لقاعدة
    /// «التشغيل = المصمّم» (RULE-N140/RULE-N142): يُطبَّقان وقت التشغيل **فقط** ما دام
    /// <c>Enabled == false</c>، ولا يُعلَنان في أي <c>*.Designer.cs</c> لأن المصمّم لا
    /// يحفظ حالة التعطيل أصلاً (RULE-N134)، ولا يُنسخان داخل أي شاشة (RULE-N112).
    /// </para>
    /// <para>المستهلك الوحيد: <c>KeyboardHelper.WireDisabledVisual</c> عبر
    /// <c>KeyboardHelper.WireDisabledVisuals</c> الذي يُوصَّل تلقائياً من
    /// <see cref="Apply(Control)"/>.</para>
    /// </summary>
    public static readonly Color DisabledBack = Color.FromArgb(224, 224, 224);

    /// <summary>
    /// نص العنصر **المعطّل** (باهت) — رفيق <see cref="DisabledBack"/>، يُطبَّق معه
    /// وقت التشغيل فقط أثناء <c>Enabled == false</c> ولا يُعلَن في أي مصمّم.
    /// </summary>
    public static readonly Color DisabledFore = Color.FromArgb(150, 150, 150);

    /// <summary>أسماء شريط العنوان/العنوان التي يُطبَّق عليها <see cref="ApplyHeader"/>.</summary>
    private static readonly HashSet<string> HeaderLabelNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "lblMode", "lblTitle", "lblHeader", "lblPageTitle"
    };

    // ═════════════════════════════════════════════════════════════════
    //  Apply — للشاشة كاملة
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يطبّق الثيم الأخضر على النموذج وكل شجرة عناصره. آمن للاستدعاء المتكرر
    /// (idempotent)، ولا يلمس خطوط العناصر الفردية ولا سلوك الشبكات.
    /// يجب أن يُستدعى **بعد** <see cref="ThemeHelper"/> (يفرض أبيض/Segoe UI).
    /// </summary>
    public static void Apply(Form form)
    {
        if (form == null) return;
        Apply((Control)form);   // نفس السلوك تماماً — القيم أدناه تُطبَّق مرة واحدة
    }

    /// <summary>
    /// نسخة الحاويات غير النموذجية (UserControl / Panel): تُطبّق نفس قواعد
    /// <see cref="Apply(Form)"/> على أي <see cref="Control"/> حاضن — تُستهلك من
    /// المكوّنات القابلة للتضمين مثل <c>ctrlDashboard</c> (RULE-N69).
    /// آمنة للاستدعاء المتكرر (idempotent).
    /// </summary>
    public static void Apply(Control container)
    {
        if (container == null) return;

        try
        {
            container.BackColor = FormBackground;

            // RULE-N21: خط Tahoma — نغيّره فقط إن كان ThemeHelper قد فرض Segoe UI.
            if (container.Font != null
                && string.Equals(container.Font.FontFamily.Name, "Segoe UI", StringComparison.OrdinalIgnoreCase))
            {
                container.Font = new Font("Tahoma", container.Font.Size);
            }

            Walk(container);

            // RULE-N43: مظهر التعطيل الموحّد (خلفية رمادية + نص باهت) لكل حقل إدخال
            // وزر في الشجرة — يُوصَّل مرة واحدة من هنا، فأي شاشة تنتهي بـ Apply(this)
            // تحصل عليه بلا كود داخلها وبلا لون مكرر (RULE-N112 / RULE-N98).
            KeyboardHelper.WireDisabledVisuals(container);
        }
        catch (Exception ex)
        {
            // لا نُفشل فتح الشاشة بسبب لون: نُسجّل الخطأ فقط.
            Serilog.Log.Error(ex, "تعذر تطبيق الثيم الأخضر");
        }
    }

    /// <summary>يمشي على شجرة العناصر recursively ويطبّق الألوان العامة.</summary>
    private static void Walk(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            // لوحة التحميل الشفافة (Color.FromArgb(100,255,255,255)) تُترك كما هي:
            // إعادة رسمها بلون الشريط المعتم تُلغي الشفافية وتُحوّلها إلى طبقة صلبة
            // تحجب الشبكة. القاعدة على الخلفيات المعتمة فقط، ولا تمسّ RULE-N52/N54.
            bool translucent = child.BackColor.A < 255;

            // ترتيب الحالات مشروط: الأكثر تخصيصاً أولاً (FlowLayoutPanel قبل Panel).
            // ولا تُعاد تلوين حاوية لم يلوّنها المصمّم — وإلا صار التشغيل رمادياً بخلاف
            // المصمّم (RULE-N134 / RULE-N139).
            switch (child)
            {
                case FlowLayoutPanel:
                case TableLayoutPanel:
                    if (!translucent && ShouldRepaint(child)) child.BackColor = BarBackground;
                    break;

                case Panel:
                    if (!translucent && ShouldRepaint(child)) child.BackColor = BarBackground;
                    break;

                case GroupBox:
                    if (!translucent && ShouldRepaint(child)) child.BackColor = GroupBackground;
                    break;

                case DataGridView grid:
                    ApplyGrid(grid);
                    break;
            }

            if (child is Label lbl && IsHeaderLabel(lbl))
                ApplyHeader(lbl);

            if (child.HasChildren)
                Walk(child);
        }
    }

    /// <summary>
    /// هل يجوز للثيم إعادة تلوين هذه الحاوية؟ (RULE-N51a / RULE-N139)
    /// <para>
    /// لا — في ثلاث حالات: (1) الحاوية **ترث** خلفية أبيها لأن المصمّم لم يضبط لها لوناً
    /// («Ambient»)، (2) الحاوية معطّلة، (3) المصمّم لوّنها بلونٍ خاص خارج لوحة الثيم.
    /// </para>
    /// <para>
    /// السبب: فرض <c>BarBackground</c>/<c>GroupBackground</c> على حاوية ورثت الأخضر من
    /// النموذج يحوّل الشاشة وقت التشغيل إلى درجات رمادية باهتة، بينما المصمّم يبدو أخضر
    /// — وهو عيب «المصمّم ≠ وقت التشغيل» بعينه.
    /// </para>
    /// </summary>
    private static bool ShouldRepaint(Control child)
    {
        if (!child.Enabled) return false;

        var parent = child.Parent;
        if (parent is null) return false;

        // Control.BackColor يُعيد لون الأب ما لم تُضبط الخاصية (Ambient) → تُترك للتوريث.
        if (child.BackColor.ToArgb() == parent.BackColor.ToArgb()) return false;

        // لون خاص من المصمّم (ليس من لوحة الثيم) → يُترك كما هو.
        return IsThemeColor(child.BackColor);
    }

    /// <summary>هل اللون من لوحة الثيم الرسمية؟ (RULE-N54: المصدر الوحيد للقيم)</summary>
    private static bool IsThemeColor(Color color)
    {
        int argb = color.ToArgb();
        return argb == FormBackground.ToArgb()
            || argb == BarBackground.ToArgb()
            || argb == GroupBackground.ToArgb()
            || argb == GridBackground.ToArgb()
            || argb == HeaderBackground.ToArgb();
    }

    private static bool IsHeaderLabel(Label label)
        => !string.IsNullOrEmpty(label.Name) && HeaderLabelNames.Contains(label.Name);

    // ═════════════════════════════════════════════════════════════════
    //  ApplyGrid — للشبكة
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// نمط الشبكة الأخضر (بديل <see cref="ThemeHelper.ApplyDataGridViewStyle"/>).
    /// لا تلمس <c>ReadOnly</c> ولا <c>AllowUserToAddRows</c> ولا <c>SelectionMode</c>
    /// ولا <c>MultiSelect</c> ولا <c>DataSource</c> — كل شاشة تعلن سلوكها بنفسها
    /// حسب RULE-N22/RULE-N22a (RULE-N52).
    /// </summary>
    public static void ApplyGrid(DataGridView dgv)
    {
        if (dgv == null) return;

        try
        {
            dgv.BackgroundColor = GridBackground;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = GridLine;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeRows = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ── الرؤوس ──
            dgv.ColumnHeadersHeight = 45;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = HeaderBackground;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = HeaderBackground;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = HeaderForeground;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ── الخلايا ──
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgv.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgv.DefaultCellStyle.SelectionBackColor = SelectionBack;
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgv.RowTemplate.Height = 32;

            // ── الأسطر المتبادلة ──
            dgv.AlternatingRowsDefaultCellStyle.BackColor = GridAlternate;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق ثيم الشبكة الأخضر");
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  ApplyHeader — لشريط العنوان
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// شريط العنوان: خلفية خضراء ونص أبيض موسّط. الخط يبقى كما ضبطه المصمّم
    /// (RULE-N53) حتى لا تتغيّر أحجام العناوين بين الشاشات.
    /// </summary>
    public static void ApplyHeader(Label header)
    {
        if (header == null) return;

        try
        {
            header.BackColor = HeaderBackground;
            header.ForeColor = HeaderForeground;
            header.TextAlign = ContentAlignment.MiddleCenter;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق ثيم شريط العنوان");
        }
    }
}
