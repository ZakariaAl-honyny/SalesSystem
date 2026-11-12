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

        try
        {
            form.BackColor = FormBackground;

            // RULE-N21: خط Tahoma — نغيّره فقط إن كان ThemeHelper قد فرض Segoe UI.
            if (form.Font != null
                && string.Equals(form.Font.FontFamily.Name, "Segoe UI", StringComparison.OrdinalIgnoreCase))
            {
                form.Font = new Font("Tahoma", form.Font.Size);
            }

            Walk(form);
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
            // ترتيب الحالات مشروط: الأكثر تخصيصاً أولاً (FlowLayoutPanel قبل Panel).
            switch (child)
            {
                case FlowLayoutPanel:
                case TableLayoutPanel:
                    child.BackColor = BarBackground;
                    break;

                case Panel:
                    child.BackColor = BarBackground;
                    break;

                case GroupBox:
                    child.BackColor = GroupBackground;
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
