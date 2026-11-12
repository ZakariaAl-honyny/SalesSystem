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
//   4) تجمع عناصر التصفية في صندوق مجموعة واحد gbFilter («بحث وتصفية») داخل صفّ
//      جديد أعلى الشبكة، مع بقاء أزرار الإجراءات فوقه خارج الصندوق
//      (RULE-N56 … RULE-N62). النقل Reparent — لا إعادة إنشاء — فلا تُفقد أي
//      معالجات أحداث ولا تُلمس قيم التصفية.
//   5) تنتهي بـ GreenThemeHelper.Apply(root) — آخر خطوة دائماً (RULE-N96)،
//      لأن أي تنسيق لاحق كان سيمحو الأخضر.
//
// ما لا تفعله (حدود متعمّدة):
//   • لا تُعيد بناء أي منطق، ولا تُعيد تسمية عنصر قائم، ولا تلمس أي منطق تقرير
//     أو SQL أو تصفية أو تصدير أو نص عربي. العنصر الوحيد الذي يُنشأ هو صندوق
//     التصفية نفسه (gbFilter/pnlFilters) وعناصره الحاوية لا غير.
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

    /// <summary>اسم صندوق التصفية الموحّد (RULE-N56).</summary>
    private const string FilterGroupControlName = "gbFilter";

    /// <summary>اسم اللوحة التي تحمل صندوق التصفية كصف مستقل أعلى الشبكة.</summary>
    private const string FilterPanelControlName = "pnlFilters";

    /// <summary>اسم حاوية عناصر التصفية داخل الصندوق.</summary>
    private const string FilterFlowControlName = "flpFilterControls";

    /// <summary>نص صندوق التصفية الموحّد.</summary>
    private const string FilterGroupCaption = "بحث وتصفية";

    /// <summary>ارتفاع صف التصفية الجديد بالبكسل (RULE-N56).</summary>
    private const int FilterPanelHeight = 68;

    /// <summary>الحد الأدنى لعناصر التصفية قبل إنشاء الصندوق (RULE-N56).</summary>
    private const int MinFilterControls = 2;

    /// <summary>سجل الشجرات التي أُنشئ فيها صندوق التصفية (منع التنفيذ المزدوج).</summary>
    private static readonly ConditionalWeakTable<Control, object> FilterGrouped = new();

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

            // RULE-N56 … RULE-N62: صندوق التصفية الموحّد قبل الثيم الأخير.
            EnsureFilterGroup(root);

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
    //  صندوق التصفية الموحّد (RULE-N56 … RULE-N62)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يجمع عناصر تصفية التقرير في صندوق مجموعة واحد <c>gbFilter</c> («بحث وتصفية»)
    /// داخل صفّ جديد أعلى الشبكة، ويترك أزرار الإجراءات في مكانها خارج الصندوق
    /// (RULE-N56 … RULE-N62). ينقل العناصر (Reparent) ولا يُعيد إنشاءها، فلا تُفقد
    /// أي معالجات أحداث ولا مراجع في كود التقرير.
    /// <para>حدود آمنة: لا يفعل شيئاً إذا لم تُطابق بنية التقرير المتوقعة، أو كانت
    /// عناصر التصفية أقل من عنصرين، أو كان الصندوق موجوداً مسبقاً.</para>
    /// </summary>
    private static void EnsureFilterGroup(Control root)
    {
        try
        {
            if (root is null) return;

            // (6) لا تُنفَّذ مرتين على الشجرة نفسها.
            if (FilterGrouped.TryGetValue(root, out _)) return;

            // (3) الصندوق موجود مسبقاً ⇒ لا شيء يُفعل.
            if (FindChildByName(root, FilterGroupControlName) is not null) return;

            // (1) جدول التخطيط الذي يستضيف شريط الإجراءات في صفّه الأول.
            var layout = FindReportLayout(root);
            if (layout is null) return;

            var host = FindFilterHost(layout);
            if (host is null) return;

            // (2) أقل من عنصرين ⇒ التقرير يبقى على تخطيطه الحالي.
            var filterControls = CollectFilterControls(host);
            if (filterControls.Count < MinFilterControls) return;

            BuildFilterGroup(layout, host, filterControls);
        }
        catch (Exception ex)
        {
            // لا نُفشل فتح تقرير بسبب تخطيط: نسجّل الخطأ فقط وتبقى الشاشة كما هي.
            Serilog.Log.Error(ex, "تعذر إنشاء صندوق التصفية الموحّد في تقرير: {Root}", root?.GetType().Name);
        }
        finally
        {
            MarkFilterGrouped(root);
        }
    }

    /// <summary>هل هذا العنصر حقل تصفية (لا تسمية)؟</summary>
    private static bool IsFilterInput(Control control) => control switch
    {
        DateTimePicker => true,
        ComboBox => true,
        TextBox => true,
        CheckBox => true,
        RadioButton => true,
        NumericUpDown => true,
        _ => false
    };

    /// <summary>يبحث عن عنصر بالاسم داخل الشجرة (بلا استثناء).</summary>
    private static Control? FindChildByName(Control parent, string name)
    {
        foreach (Control child in parent.Controls)
        {
            if (string.Equals(child.Name, name, StringComparison.OrdinalIgnoreCase)) return child;

            if (child.HasChildren)
            {
                var nested = FindChildByName(child, name);
                if (nested is not null) return nested;
            }
        }

        return null;
    }

    /// <summary>
    /// يجد جدول التخطيط المرجعي للتقارير: صفوفه ثلاثة على الأقل، وصفّه الأول
    /// يستضيف حاوية تضم عناصر تصفية (شريط الأدوات).
    /// </summary>
    private static TableLayoutPanel? FindReportLayout(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            if (child is TableLayoutPanel layout && layout.RowCount >= 3 && FindFilterHost(layout) is not null)
                return layout;

            if (child.HasChildren)
            {
                var nested = FindReportLayout(child);
                if (nested is not null) return nested;
            }
        }

        return null;
    }

    /// <summary>يبحث عن حاوية عناصر التصفية داخل صفوف الجدول الأول (Row = 0).</summary>
    private static Control? FindFilterHost(TableLayoutPanel layout)
    {
        foreach (Control child in layout.Controls)
        {
            if (layout.GetRow(child) != 0) continue;

            var host = FindFilterHostIn(child);
            if (host is not null) return host;
        }

        return null;
    }

    /// <summary>
    /// الحاوية الأعمق التي تضم عناصر التصفية **مباشرةً** (شريط الأدوات عادةً).
    /// لا نُعمّق البحث داخل حقول التصفية نفسها — <c>NumericUpDown</c> له مربع نص داخلي
    /// كان سيُحسب عنصر تصفية مستقلاً.
    /// </summary>
    private static Control? FindFilterHostIn(Control container)
    {
        if (!IsFilterInput(container))
        {
            foreach (Control child in container.Controls)
            {
                if (!child.HasChildren) continue;

                var nested = FindFilterHostIn(child);
                if (nested is not null) return nested;
            }
        }

        return CollectFilterControls(container).Count >= MinFilterControls ? container : null;
    }

    /// <summary>
    /// عناصر التصفية المباشرة داخل الحاوية بترتيب العرض الحالي: حقول الإدخال
    /// متبوعةً بتسمياتها كما وردت في <see cref="Control.ControlCollection"/>.
    /// التسمية (<see cref="Label"/>) تُعدّ عنواناً لحقل لأن الحاوية تضم حقلاً بالفعل.
    /// </summary>
    private static List<Control> CollectFilterControls(Control host)
    {
        var result = new List<Control>();

        bool hasInput = false;
        foreach (Control child in host.Controls)
        {
            if (!IsFilterInput(child)) continue;
            hasInput = true;
            break;
        }

        if (!hasInput) return result;

        foreach (Control child in host.Controls)
        {
            if (IsFilterInput(child) || child is Label) result.Add(child);
        }

        return result;
    }

    /// <summary>
    /// ينشئ <c>pnlFilters</c> + <c>gbFilter</c> + حاوية تدفّق، وينقل عناصر التصفية
    /// إليها بالترتيب والهوامش والمقاسات نفسها، ثم يُدخلها كصفّ جديد رقم 1.
    /// </summary>
    private static void BuildFilterGroup(TableLayoutPanel layout, Control host, List<Control> filterControls)
    {
        var flow = new FlowLayoutPanel
        {
            Name = FilterFlowControlName,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = true,
            Margin = new Padding(0)
        };

        var groupBox = new GroupBox
        {
            Name = FilterGroupControlName,
            Text = FilterGroupCaption,
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
            RightToLeft = RightToLeft.Yes,
            Font = new Font("Tahoma", 9F, FontStyle.Bold)
        };
        groupBox.Controls.Add(flow);

        // نقل (Reparent) — يحفظ المعالجات والمراجع والهوامش والمقاسات كما هي.
        foreach (var control in filterControls)
        {
            switch (control)
            {
                case CheckBox check:
                    check.AutoSize = true;
                    break;

                case Label label:
                    label.AutoSize = true;
                    label.TextAlign = ContentAlignment.MiddleRight;
                    break;
            }

            flow.Controls.Add(control);   // الإضافة إلى حاوية جديدة تنقل العنصر من حاويته القديمة
        }

        var panel = new Panel
        {
            Name = FilterPanelControlName,
            Dock = DockStyle.Top,
            Height = FilterPanelHeight,
            Margin = new Padding(0)
        };
        panel.Controls.Add(groupBox);

        InsertFilterRow(layout, panel);
    }

    /// <summary>
    /// يُدخل لوحة التصفية كصفّ جديد رقم 1: يزيد عدد الصفوف، يُزحزح صفوف العناصر
    /// القائمة بمقدار واحد، يُدرج <see cref="RowStyle"/> مطلق بارتفاع الصف الجديد،
    /// ثم يضع اللوحة في العمود 0 (ممتدة على كل الأعمدة إن كان الجدول متعدد الأعمدة).
    /// </summary>
    private static void InsertFilterRow(TableLayoutPanel layout, Control panel)
    {
        const int newRowIndex = 1;

        layout.RowCount += 1;

        var existingChildren = new List<Control>();
        foreach (Control child in layout.Controls) existingChildren.Add(child);

        foreach (var child in existingChildren)
        {
            int row = layout.GetRow(child);
            if (row >= newRowIndex) layout.SetRow(child, row + 1);
        }

        InsertRowStyle(layout, newRowIndex, FilterPanelHeight);

        layout.Controls.Add(panel);
        layout.SetRow(panel, newRowIndex);
        layout.SetColumn(panel, 0);

        if (layout.ColumnCount > 1)
            layout.SetColumnSpan(panel, layout.ColumnCount);
    }

    /// <summary>
    /// يُدرج <see cref="RowStyle"/> في موضع محدّد. <c>RowStyleCollection</c> لا يوفّر
    /// <c>Insert</c>، فتُعاد بناء القائمة بأنماطها الأصلية وترتيبها ثم يُضاف النمط الجديد
    /// في موضعه، مع مزامنة العدد مع <see cref="TableLayoutPanel.RowCount"/>.
    /// </summary>
    private static void InsertRowStyle(TableLayoutPanel layout, int index, int height)
    {
        var existing = new List<RowStyle>();
        foreach (RowStyle style in layout.RowStyles)
            existing.Add(new RowStyle(style.SizeType, style.Height));

        if (index < 0) index = 0;
        if (index > existing.Count) index = existing.Count;

        layout.RowStyles.Clear();

        for (int i = 0; i < existing.Count; i++)
        {
            if (i == index) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
            layout.RowStyles.Add(existing[i]);
        }

        if (index >= existing.Count) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, height));

        while (layout.RowStyles.Count < layout.RowCount)
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
    }

    /// <summary>يسجّل الشجرة كمعالَجة حتى لا يُنشأ الصندوق مرتين.</summary>
    private static void MarkFilterGrouped(Control root)
    {
        if (root is null) return;

        try
        {
            if (!FilterGrouped.TryGetValue(root, out _))
                FilterGrouped.Add(root, AppliedMarker);
        }
        catch
        {
            // مفتاح مُضاف بالفعل — لا شيء يُفعل.
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
