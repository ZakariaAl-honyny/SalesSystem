// -----------------------------------------------------------------------------
// ReportUiStandardizer.cs — معيار واجهة موحّد لكل شاشات التقارير (31 تقريراً)
//
// المشكلة: شاشات التقارير بُنيت على نمطين قديمين — بعضها يبني شبكته بـ Tahoma
// يدوياً وخلفية بيضاء، وبعضها يستدعي ThemeHelper.ApplyDataGridViewStyle فتخرج
// رؤوس رمادية وخط Segoe UI، وكلها بلا تنقل بالوحة المفاتيح. إصلاح ذلك يدوياً
// في 62 ملفاً مكرر ومحفوف بالانحراف، لذلك يُطبَّق المعيار من نقطة واحدة.
//
// الاستهلاك: سطر واحد في نهاية المُنشئ (أو بعد InitializeComponent):
//     ReportUiStandardizer.Apply(this);       // من داخل *ReportTab : UserControl
//     ReportUiStandardizer.ApplyForm(this);   // من داخل frm*Report : Form
//
// ما تفعله الأداة:
//   1) تمرّ على شجرة العناصر وتُطبّق GreenThemeHelper.ApplyGrid على **كل**
//      DataGridView — وهذا يتجاوز أي ThemeHelper.ApplyDataGridViewStyle سابق
//      (RULE-N51: الشبكة الخضراء هي النمط المعتمد، والتجاوز مقصود).
//   2) تفرض خط Tahoma على العناصر التي تلمسها (شبكات/تسميات 9F، أزرار 10F)،
//      ولا تُغيّر حجم الخط إن كان Tahoma أصلاً (حفاظاً على أحجام العناوين
//      المقصودة في المصمّم) — تتغيّر الأحجام فقط عند التحويل من Segoe UI.
//   3) تُوصِّل تنقل لوحة المفاتيح مرة واحدة لكل شجرة:
//      WireEnterAsTab + WireSelectAllOnFocus + WireFocusHighlight + WireArrowNavigation
//      (RULE-N40 … RULE-N46).
//   4) تنتهي بـ GreenThemeHelper.Apply(root) — آخر خطوة دائماً (RULE-N96)،
//      لأن أي تنسيق لاحق كان سيمحو الأخضر.
//
// ما لا تفعله (حدود متعمّدة):
//   • لا تُعيد بناء أي تخطيط، ولا تُعيد تسمية عنصر، ولا تلمس أي منطق تقرير
//     أو SQL أو تصفية أو تصدير أو نص عربي.
//   • لا تلمس ReadOnly / AllowUserToAddRows / SelectionMode / DataSource
//     (RULE-N52) — كل تقرير يعلن سلوك شبكته بنفسه.
//   • كل الأساليب دفاعية: null-safe، ولا تُطلق استثناءً إلى النموذج أبداً.
//   • آمنة للاستدعاء المتكرر (idempotent): الاستدعاء الثاني لا يُضاعف التوصيل
//     ولا يُعيد تنسيق الشبكة.
// -----------------------------------------------------------------------------

using System.Drawing;
using System.Runtime.CompilerServices;

namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// معيار واجهة موحّد لشاشات التقارير: الثيم الأخضر + تنقل لوحة المفاتيح + شبكات خضراء،
/// من سطر واحد لكل شاشة بدل تعديل كل تقرير يدوياً.
/// </summary>
public static class ReportUiStandardizer
{
    /// <summary>علامة "تم التوحيد" — تُخزَّن في <see cref="Control.Tag"/> للجذر (RULE-N96).</summary>
    private const string AppliedTag = "ReportUiStandardized";

    /// <summary>قيمة العلامة المخزَّنة في السجل الضعيف (هوية مرجعية بلا تسريب ذاكرة).</summary>
    private static readonly object AppliedMarker = new();

    /// <summary>
    /// سجل الجذور التي وُحِّدت بالفعل. <see cref="ConditionalWeakTable{TKey,TValue}"/>
    /// وليس <c>HashSet</c> حتى لا يُبقى مرجع قوي إلى النماذج المغلقة.
    /// </summary>
    private static readonly ConditionalWeakTable<Control, object> Applied = new();

    /// <summary>سجل الجذور التي وُصِّل فيها تنقل لوحة المفاتيح (منع التوصيل المزدوج).</summary>
    private static readonly ConditionalWeakTable<Control, object> WiredTrees = new();

    /// <summary>لون تظليل الحقل النشط الموحّد (RULE-N46).</summary>
    private static readonly Color FocusHighlight = Color.FromArgb(0xD6, 0xE4, 0xF0);

    /// <summary>أسماء تسميات شريط العنوان المعتمدة.</summary>
    private static readonly string[] HeaderLabelNames = { "lblMode", "lblTitle", "lblHeader", "lblPageTitle" };

    // ═════════════════════════════════════════════════════════════════
    //  Apply — لتبويب تقرير أو أي حاوية (UserControl / Panel)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يوحّد واجهة الحاوية: شبكات خضراء + خط Tahoma + تنقل لوحة مفاتيح + الثيم الأخضر.
    /// آمن للاستدعاء المتكرر: الاستدعاء الثاني لا يفعل شيئاً.
    /// </summary>
    /// <param name="root">جذر الشجرة (تبويب تقرير عادةً) — قد يكون null.</param>
    public static void Apply(Control? root)
    {
        if (root is null) return;

        try
        {
            if (IsApplied(root)) return;

            StyleTree(root, skipAppliedSubtrees: false);
            WireKeyboard(root);

            // RULE-N96: الثيم الأخضر آخر خطوة — بعد أي تنسيق سابق في المُنشئ.
            GreenThemeHelper.Apply(root);
        }
        catch (Exception ex)
        {
            // لا نُفشل فتح تقرير بسبب لون أو خط: نسجّل الخطأ فقط.
            Serilog.Log.Error(ex, "تعذر توحيد واجهة تقرير: {Root}", root.GetType().Name);
        }
        finally
        {
            MarkApplied(root);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  ApplyForm — لنموذج تقرير (frm*Report)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يوحّد نموذج التقرير: يطبّق <see cref="Apply"/> على شجرته، ويُلوّن تسمية العنوان
    /// العلوية إن وُجدت، ويُوصِّل <c>F5</c> بزر التحديث المرئي إن أمكن العثور عليه.
    /// </summary>
    /// <param name="form">نموذج التقرير — قد يكون null.</param>
    public static void ApplyForm(Form? form)
    {
        if (form is null) return;

        try
        {
            if (IsApplied(form)) return;

            // النماذج التي تستضيف تبويباً موحَّداً سبق أن وُصِّل تنقله داخل التبويب؛
            // إعادة التوصيل من جذر النموذج تُضاعف معالج Enter فيتخطّى التركيز حقلين.
            if (HasStandardizedSubtree(form))
            {
                StyleTree(form, skipAppliedSubtrees: true);
                GreenThemeHelper.Apply(form);
                MarkApplied(form);
            }
            else
            {
                Apply(form);   // النموذج المستقل: توحيد كامل + توصيل كامل
            }

            ApplyTitleHeader(form);
            WireRefreshShortcut(form);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر توحيد واجهة نموذج تقرير: {Form}", form.GetType().Name);
        }
        finally
        {
            MarkApplied(form);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  التنسيق: شبكات + خطوط
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يمشي على شجرة العناصر ويُطبّق الشبكة الخضراء وخط Tahoma على ما يلمسه.</summary>
    private static void StyleTree(Control parent, bool skipAppliedSubtrees)
    {
        try
        {
            StyleControl(parent);

            foreach (Control child in parent.Controls)
            {
                if (skipAppliedSubtrees && IsApplied(child)) continue;
                StyleTree(child, skipAppliedSubtrees);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تنسيق عنصر تقرير: {Control}", parent?.GetType().Name);
        }
    }

    /// <summary>يُطبّق ثيم الشبكة الأخضر ويوحّد خط العنصر حسب نوعه.</summary>
    private static void StyleControl(Control control)
    {
        if (control is null) return;

        try
        {
            switch (control)
            {
                case DataGridView grid:
                    // الخط قبل الثيم: ApplyGrid يضبط خط الخلايا/الرؤوس (Tahoma 10F) فيبقى هو الحاكم.
                    ApplyTahoma(grid, 9F);
                    GreenThemeHelper.ApplyGrid(grid);   // يتجاوز ThemeHelper.ApplyDataGridViewStyle (RULE-N51)
                    break;

                case Label label:
                    ApplyTahoma(label, 9F);
                    break;

                case Button button:
                    ApplyTahoma(button, 10F);
                    break;
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق خط Tahoma على عنصر تقرير");
        }
    }

    /// <summary>
    /// يفرض عائلة Tahoma على العنصر. الحجم يُثبَّت على <paramref name="defaultSize"/>
    /// فقط عند التحويل من عائلة أخرى (Segoe UI)، وإلا يبقى الحجم المقصود كما هو.
    /// </summary>
    private static void ApplyTahoma(Control control, float defaultSize)
    {
        try
        {
            var font = control.Font;
            if (font is null) return;

            if (string.Equals(font.FontFamily.Name, "Tahoma", StringComparison.OrdinalIgnoreCase))
                return;   // الخط صحيح — لا نلمس الحجم (RULE-N53: أحجام العناوين تبقى كما ضبطها المصمّم)

            control.Font = new Font("Tahoma", defaultSize, font.Style);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر فرض خط Tahoma على عنصر تقرير");
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  لوحة المفاتيح (RULE-N40 … RULE-N46)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يُوصِّل الأربعة معاً مرة واحدة فقط لكل جذر (RULE-N40).</summary>
    private static void WireKeyboard(Control root)
    {
        try
        {
            if (WiredTrees.TryGetValue(root, out _)) return;

            KeyboardHelper.WireEnterAsTab(root);
            KeyboardHelper.WireSelectAllOnFocus(root);          // RULE-N41
            KeyboardHelper.WireFocusHighlight(root, FocusHighlight); // RULE-N46
            KeyboardHelper.WireArrowNavigation(root);           // RULE-N45

            WiredTrees.Add(root, AppliedMarker);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر توصيل تنقل لوحة المفاتيح في تقرير: {Root}", root?.GetType().Name);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  عنوان النموذج + اختصار التحديث
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يُلوّن تسمية العنوان العلوية إن وُجدت، ويتجاهل بصمت إن لم توجد.</summary>
    private static void ApplyTitleHeader(Form form)
    {
        try
        {
            var header = FindTitleLabel(form);
            if (header is not null)
                GreenThemeHelper.ApplyHeader(header);   // RULE-N53
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق ثيم شريط عنوان التقرير");
        }
    }

    /// <summary>يبحث عن تسمية عنوان: بالاسم المعتمد أولاً، ثم أول تسمية ملتصقة بأعلى الحاوية.</summary>
    private static Label? FindTitleLabel(Control parent)
    {
        Label? firstTopDocked = null;

        foreach (Control child in parent.Controls)
        {
            if (child is Label lbl)
            {
                if (!string.IsNullOrEmpty(lbl.Name)
                    && HeaderLabelNames.Any(n => string.Equals(n, lbl.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    return lbl;
                }

                if (firstTopDocked is null && lbl.Dock == DockStyle.Top && !string.IsNullOrWhiteSpace(lbl.Text))
                    firstTopDocked = lbl;
            }

            if (child.HasChildren)
            {
                var nested = FindTitleLabel(child);
                if (nested is not null) return nested;
            }
        }

        return firstTopDocked;
    }

    /// <summary>يُوصِّل F5 إلى زر التحديث المرئي إن أمكن العثور عليه (RULE-N47).</summary>
    private static void WireRefreshShortcut(Form form)
    {
        try
        {
            var refresh = FindRefreshButton(form);
            if (refresh is null)
            {
                // لا يوجد زر تحديث يمكن اكتشافه: نتخطى F5 بدل ربطه بإجراء عشوائي.
                Serilog.Log.Information("لا يوجد زر تحديث في {Form} — لم يُوصَّل اختصار F5", form.GetType().Name);
                return;
            }

            KeyboardHelper.WireGlobalShortcuts(form, (Keys.F5, () => RaiseRefresh(refresh)));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر توصيل اختصار F5 في تقرير: {Form}", form.GetType().Name);
        }
    }

    /// <summary>يجد زر التحديث: بالاسم <c>btnRefresh</c> ثم بنص يحتوي «تحديث».</summary>
    private static Button? FindRefreshButton(Control parent)
    {
        Button? byText = null;

        foreach (Control child in parent.Controls)
        {
            if (child is Button btn)
            {
                if (string.Equals(btn.Name, "btnRefresh", StringComparison.OrdinalIgnoreCase))
                    return btn;

                if (byText is null && !string.IsNullOrEmpty(btn.Text) && btn.Text.Contains("تحديث"))
                    byText = btn;
            }

            if (child.HasChildren)
            {
                var nested = FindRefreshButton(child);
                if (nested is not null) return nested;
            }
        }

        return byText;
    }

    /// <summary>ينقر زر التحديث برمجياً بلا استثناءات (يُتجاهل الزر المعطّل).</summary>
    private static void RaiseRefresh(Button button)
    {
        try
        {
            if (button.IsDisposed || !button.Enabled) return;
            button.PerformClick();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تنفيذ تحديث التقرير من اختصار F5");
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  علامات "تم التوحيد"
    // ═════════════════════════════════════════════════════════════════

    /// <summary>هل وُحِّد هذا الجذر سابقاً؟</summary>
    private static bool IsApplied(Control control)
    {
        if (control is null) return true;

        try
        {
            if (Applied.TryGetValue(control, out _)) return true;
            return control.Tag is string tag && string.Equals(tag, AppliedTag, StringComparison.Ordinal);
        }
        catch
        {
            return true;   // في حال تعذّر الفحص: لا نُعيد التوحيد
        }
    }

    /// <summary>يسجّل الجذر كموحَّد (سجل ضعيف + علامة في Tag).</summary>
    private static void MarkApplied(Control control)
    {
        if (control is null) return;

        try
        {
            if (!Applied.TryGetValue(control, out _))
                Applied.Add(control, AppliedMarker);
        }
        catch
        {
            // مفتاح مُضاف بالفعل — لا شيء يُفعل.
        }

        try
        {
            if (control.Tag is null || control.Tag is string)
                control.Tag = AppliedTag;
        }
        catch
        {
            // بعض العناصر ترفض تغيير Tag في حالات نادرة — العلامة في السجل تكفي.
        }
    }

    /// <summary>هل تحتوي الشجرة على جذر فرعي موحَّد سابقاً (تبويب داخلي)؟</summary>
    private static bool HasStandardizedSubtree(Control root)
    {
        foreach (Control child in root.Controls)
        {
            if (IsApplied(child)) return true;
            if (child.HasChildren && HasStandardizedSubtree(child)) return true;
        }

        return false;
    }
}
