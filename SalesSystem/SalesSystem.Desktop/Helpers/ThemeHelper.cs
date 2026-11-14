using System.Drawing;
using System.Drawing.Drawing2D;

namespace SalesSystem.Desktop.Helpers;

public static class ThemeHelper
{
    // Institutional Professional Palette (Modern Flat Colors)
    // الألوان الستة أدناه تمرّر قيمها الحيّة من ThemePalette (المصدر الوحيد القابل
    // للتغيير) — لا لون حرفي هنا، فأي تعديل في اللوحة ينعكس فوراً على كل الأزرار
    // بلا تعديل أي شاشة (RULE-N54 / RULE-N112).
    public static Color Primary => ThemePalette.Get(ThemePalette.BtnAddNewButtonColor);     // Modern Blue
    public static Color Success => ThemePalette.Get(ThemePalette.BtnSaveButtonColor);      // Eye-friendly Green
    public static Color Danger => ThemePalette.Get(ThemePalette.BtnDeleteButtonColor);     // Muted Red
    public static Color Warning => ThemePalette.Get(ThemePalette.BtnWarningButtonColor);   // Amber
    public static Color Neutral => ThemePalette.Get(ThemePalette.BtnCancelButtonColor);    // Gray
    public static Color Secondary => ThemePalette.Get(ThemePalette.BtnSecondaryButtonColor); // Light Gray

    // ── الألوان المحيطة (GAP-19): كل لون له مالك واحد فقط ──
    // ما له مقابل في GreenThemeHelper يُستهلك منه مباشرةً (خاصية حيّة، بلا لون حرفي
    // هنا)؛ وما لا مفتاح له في ThemePalette بعد يبقى هنا مُعلَناً مرة واحدة بتعليق
    // ترحيل صريح — ولا تُضاف مفاتيح إلى ThemePalette في هذه الموجة (RULE-N112).
    /// <summary>حدود الأزرار الثانوية/الشبحية والخط السفلي لأشرطة الأدوات — لون خطوط الثيم الأخضر (RULE-N112).</summary>
    public static Color BorderColor => GreenThemeHelper.GridLine;

    // ثابت قديم — لا مفتاح في ThemePalette بعد؛ يُرحَّل عند أول مساس (GAP-19)
    private static readonly Color SidebarDark = Color.FromArgb(33, 43, 54);

    /// <summary>خلفية الشريط الجانبي في الشاشة الرئيسية.</summary>
    public static readonly Color SidebarBg = SidebarDark;

    /// <summary>خلفية منطقة المحتوى — لون صناديق المجموعات من الثيم الأخضر (RULE-N112).</summary>
    public static Color ContentBg => GreenThemeHelper.GroupBackground;

    /// <summary>النص الأساسي — لون النص المميّز من الثيم الأخضر (RULE-N112).</summary>
    public static Color TextPrimary => GreenThemeHelper.TextStrong;

    /// <summary>النص الثانوي (تسميات مساعدة) — لون الحالة الباهتة من الثيم الأخضر (RULE-N112).</summary>
    public static Color TextSecondary => GreenThemeHelper.StatusMuted;

    // ثابت قديم — لا مفتاح في ThemePalette بعد؛ يُرحَّل عند أول مساس (GAP-19)
    private static readonly Color TextDark = Color.FromArgb(50, 50, 50);

    /// <summary>نص الخلايا الداكن.</summary>
    public static readonly Color TextDarkGray = TextDark;

    /// <summary>
    /// الأبيض النقي: نص الأزرار على الخلفية الصلبة (أزرق/أخضر/أحمر/رمادي) وخلفية
    /// حقول الإدخال (RULE-N143) — القيمة مُعلنة هنا مرة واحدة فقط، فلا يتكرر أي لون
    /// في الملف (GAP-19 / RULE-N169).
    /// </summary>
    private static readonly Color ButtonForeOnSolid = Color.White;

    // Standard Dimensions
    public static readonly int StandardControlHeight = 35;
    public static readonly int StandardButtonWidth = 100;
    
    public enum ButtonType { Primary, Success, Danger, Warning, Neutral, Secondary, Ghost }

    public static void ApplyMainFormStyle(Form form)
    {
        form.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        // ملاحظة (RULE-N139/RULE-N142): لا تُفرَض خلفية بيضاء على النموذج — كان ذلك
        // يمحو الخلفية الخضراء التي أعلنها المصمّم فتُظهر الشاشة رمادية وقت التشغيل.
        // لون خلفية النموذج يبقى مالكه الوحيد: المصمّم ثم GreenThemeHelper.
        form.RightToLeft = RightToLeft.Yes;
        form.RightToLeftLayout = true;
    }

    public static void ApplyButtonStyle(Button btn, ButtonType type = ButtonType.Primary)
    {
        btn.FlatStyle = FlatStyle.Flat;
        btn.Cursor = Cursors.Hand;
        btn.Height = StandardControlHeight;
        btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btn.Padding = new Padding(12, 0, 12, 0);
        btn.AutoSize = true;
        btn.MinimumSize = new Size(StandardButtonWidth, StandardControlHeight);
        btn.FlatAppearance.BorderSize = 0;

        switch (type)
        {
            case ButtonType.Primary:
                btn.BackColor = Primary;
                btn.ForeColor = ButtonForeOnSolid;
                break;
            case ButtonType.Success:
                btn.BackColor = Success;
                btn.ForeColor = ButtonForeOnSolid;
                break;
            case ButtonType.Danger:
                btn.BackColor = Danger;
                btn.ForeColor = ButtonForeOnSolid;
                break;
            case ButtonType.Secondary:
                btn.BackColor = Secondary;
                btn.ForeColor = TextPrimary;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = BorderColor;
                break;
            case ButtonType.Ghost:
                btn.BackColor = Color.Transparent;
                btn.ForeColor = TextSecondary;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = BorderColor;
                break;
            case ButtonType.Neutral:
                btn.BackColor = Neutral;
                btn.ForeColor = ButtonForeOnSolid;
                break;
            default:
                btn.BackColor = Secondary;
                btn.ForeColor = TextPrimary;
                break;
        }

        btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(btn.BackColor, 0.1f);
        if (type == ButtonType.Ghost || type == ButtonType.Secondary)
            btn.FlatAppearance.MouseOverBackColor = GhostHoverBack;
    }

    /// <summary>لون تحويم الأزرار الشبحية/الثانوية (بلا مفتاح في اللوحة).</summary>
    // ثابت قديم — لا مفتاح في ThemePalette بعد؛ يُرحَّل عند أول مساس (GAP-19)
    private static readonly Color GhostHoverBack = Color.FromArgb(240, 240, 240);

    public static void ApplyToolbarStyle(Panel toolbar)
    {
        toolbar.Padding = new Padding(12);
        toolbar.BackColor = GreenThemeHelper.BarBackground;
        toolbar.Height = 65;
        
        // Use a bottom border for the toolbar
        toolbar.Paint += (s, e) => {
            using var pen = new Pen(BorderColor, 1);
            e.Graphics.DrawLine(pen, 0, toolbar.Height - 1, toolbar.Width, toolbar.Height - 1);
        };
    }

    /// <summary>
    /// ملاحظة (RULE-N51 / RULE-N112): شكل الشبكة الأخضر مالكه الوحيد
    /// <see cref="GreenThemeHelper.ApplyGrid"/> — هذا العضو يبقى للتوافق مع النداءات
    /// القائمة ويُمرِّر إليها، فلا نسخة ثانية من ألوان الشبكة هنا.
    /// </summary>
    public static void ApplyDataGridViewStyle(DataGridView dgv)
    {
        GreenThemeHelper.ApplyGrid(dgv);
    }

    public static void ApplySearchBoxStyle(TextBox txt)
    {
        txt.Height = StandardControlHeight;
        txt.Font = new Font("Segoe UI", 11F);
        txt.BorderStyle = BorderStyle.FixedSingle;
        txt.BackColor = ButtonForeOnSolid;   // RULE-N143: حقول الإدخال بيضاء (نفس الأبيض المُعلَن مرة واحدة)
        // لون تظليل التركيز لا يُكتب هنا: مالكه KeyboardHelper.WireFocusHighlight
        // الذي يقرأ ThemePalette.ActiveFieldColor حيّاً (RULE-N54b) — فلا توصيل مزدوج.
    }

    public static void WireFocusIndicator(params Control[] controls)
    {
        foreach (Control ctrl in controls)
        {
            if (ctrl == null) continue;
            Color originalBack = ctrl.BackColor;
            ctrl.Enter += (s, e) =>
            {
                // قراءة حيّة من اللوحة في كل دخول (RULE-N46 / RULE-N54b).
                if (ctrl.Enabled) ctrl.BackColor = ThemePalette.Get(ThemePalette.SelectionRowColor);
            };
            ctrl.Leave += (s, e) => ctrl.BackColor = originalBack;
            ctrl.EnabledChanged += (s, e) =>
            {
                if (!ctrl.Enabled) ctrl.BackColor = originalBack;
            };
        }
    }

    public static void ApplyDialogStyle(Form form)
    {
        ApplyMainFormStyle(form);
        form.StartPosition = FormStartPosition.CenterParent;
        form.FormBorderStyle = FormBorderStyle.FixedDialog;
        form.MaximizeBox = false;
        form.MinimizeBox = false;
        form.Padding = new Padding(20);
    }
}
