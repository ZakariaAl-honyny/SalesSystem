using System.Runtime.CompilerServices;

namespace SalesSystem.Desktop.Helpers;

public static class KeyboardHelper
{
    /// <summary>اسم حقل البحث — لا يُطبَّق عليه تحديد النص عند دخول المؤشر (لتسهيل الكتابة المتتابعة).</summary>
    private const string SearchControlName = "txtSearch";

    /// <summary>اسم حقل تصفية الشبكة — لا يُطبَّق عليه تحديد النص عند دخول المؤشر.</summary>
    private const string FilterValueControlName = "txtFilterValue";

    /// <summary>
    /// سجل الحقول التي وُصِّلت بالفعل لتحديد القيمة الحالية عند دخول المؤشر.
    /// الاختيار صريح: <see cref="ConditionalWeakTable{TKey, TValue}"/> وليس <c>HashSet&lt;Control&gt;</c>،
    /// لأنه لا يُبقي مرجعاً قوياً إلى النماذج التي أُغلقت (الجدول الثابت كان سيُسرّب كل نموذج يُفتح ويُغلق)،
    /// ويعتمد هوية المرجع نفسه (Reference Identity) كما لو استخدمنا ReferenceEqualityComparer.
    /// الفائدة: التوصيل يحدث مرة واحدة لكل حقل ⇒ لا تحديد مزدوج ولا حلقات تكرارية.
    /// </summary>
    private static readonly ConditionalWeakTable<Control, object> SelectAllWired = new();

    /// <summary>سجل الحقول التي وُصِّلت بالفعل للتنقل بالأسهم (منع التوصيل المزدوج).</summary>
    private static readonly ConditionalWeakTable<Control, object> ArrowNavigationWired = new();

    /// <summary>سجل النماذج التي وُصِّل فيها زر الوضع مع Enter (منع التوصيل المزدوج).</summary>
    private static readonly ConditionalWeakTable<Control, object> ModeEnterWired = new();

    /// <summary>سجل العناصر التي وُصِّل لها مظهر التعطيل الموحّد (منع التوصيل المزدوج).</summary>
    private static readonly ConditionalWeakTable<Control, object> DisabledVisualWired = new();

    /// <summary>علامة مشتركة تُخزَّن في جدولي التوصيل (قيمة واحدة لكل مفتاح).</summary>
    private static readonly object WiredMarker = new();

    /// <summary>
    /// حالة توصيل الفأرة لحقل واحد: هل دخل المؤشر بضغطة؟ وهل التحديد ما زال منتظراً؟
    /// </summary>
    private sealed class SelectAllState
    {
        /// <summary>دخل المؤشر إلى الحقل بضغطة فأرة في هذه الضغطة نفسها (لا من لوحة المفاتيح).</summary>
        public bool EnteredByMouse;

        /// <summary>التحديد منتظر — يُلغى عند سحب الفأرة لتحديد جزء من النص.</summary>
        public bool PendingSelectAll;
    }

    /// <summary>
    /// يحوّل مفتاح Enter إلى انتقال للحقل التالي (مثل Tab) في كل أنواع حقول الإدخال:
    /// TextBox / MaskedTextBox / ComboBox / NumericUpDown / DateTimePicker.
    /// </summary>
    public static void WireEnterAsTab(Control parent)
    {
        foreach (Control ctrl in parent.Controls)
        {
            if (IsEnterNavigationInput(ctrl))
            {
                var input = ctrl;
                input.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
                    {
                        // البحث عن الحقل التالي من جذر النموذج حتى يعبر حدود الحاويات (GroupBox / Panel).
                        var container = input.FindForm() ?? parent;
                        container.SelectNextControl(input, true, true, true, true);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                };
            }

            if (ctrl.HasChildren)
                WireEnterAsTab(ctrl);
        }
    }

    /// <summary>
    /// عند دخول المؤشر إلى حقل إدخال، يحدد كل قيمته الحالية حتى تستبدلها الكتابة مباشرة.
    /// يعمل مع لوحة المفاتيح (Tab) ومع ضغطة الفأرة:
    /// <list type="bullet">
    /// <item>دخول بضغطة فأرة إلى حقل غير نشط ⇒ تحديد كامل بعد استقرار موضع المؤشر.</item>
    /// <item>ضغطة ثانية داخل حقل نشط ⇒ سلوك طبيعي (تحديد موضع المؤشر) بلا تحديد كامل.</item>
    /// <item>سحب الفأرة لتحديد جزء من النص ⇒ لا يُلغى اختيار المستخدم.</item>
    /// </list>
    /// لا يُطبَّق على حقول البحث/التصفية، ولا المقروءة فقط، ولا عديدة الأسطر، ولا كلمات المرور،
    /// ولا القوائم المنسدلة المغلقة (DropDownList)، ولا الشبكات ومحرراتها.
    /// </summary>
    public static void WireSelectAllOnFocus(Control parent)
    {
        foreach (Control ctrl in parent.Controls)
        {
            if (IsSelectAllTarget(ctrl) && MarkWired(SelectAllWired, ctrl))
            {
                var target = ctrl;
                var state = new SelectAllState();

                // (1) لوحة المفاتيح: دخول المؤشر بـ Tab/السهم ⇒ تحديد القيمة الحالية فوراً.
                target.Enter += SelectAllCurrentValue;

                // (2) تمييز دخول المؤشر بضغطة فأرة (Control.MouseButtons يقرأ حالة الزر الحقيقية)،
                // ثم تأجيل التحديد إلى ما بعد معالجة الرسالة كي لا يُلغيه وضع المؤشر الذي تليه الضغطة.
                target.Enter += (s, e) =>
                {
                    try
                    {
                        state.EnteredByMouse = (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left;
                        state.PendingSelectAll = state.EnteredByMouse;

                        if (!state.EnteredByMouse) return; // دخول بلوحة المفاتيح: تم التحديد أعلاه.

                        // يشمل ComboBox لأن الضغطة تقع على محرره الداخلي ولا يرفع بالضرورة MouseDown.
                        target.BeginInvoke((Action)(() =>
                        {
                            try
                            {
                                if (!state.PendingSelectAll) return; // تم إلغاؤه (سحب) أو نُفِّذ فعلاً.
                                state.PendingSelectAll = false;
                                SelectAllTarget(target);
                            }
                            catch
                            {
                                // تجاهل
                            }
                        }));
                    }
                    catch
                    {
                        // تجاهل
                    }
                };

                // (3) الضغطة قد تقع على الحقل نفسه أو على محرره الداخلي:
                //     NumericUpDown يستضيف TextBoxBase في Controls[1] (UpDownEdit) وأزراراً في Controls[0]،
                //     و ComboBox يستضيف أبناءه إن وُجدت — لذلك نُوصِّل الحقل وأبناءه بنفس الحالة.
                AttachSelectAllMouseHandlers(target, target, state);

                foreach (Control child in target.Controls)
                    AttachSelectAllMouseHandlers(child, target, state);
            }

            // لا ننزل داخل NumericUpDown/ComboBox: محررهما الداخلي عُولج أعلاه كجزء من الحقل نفسه
            // (ولو نزلنا لاعتُبر محرر NumericUpDown حقلاً مستقلاً ⇒ توصيل مزدوج وتحديد مزدوج).
            if (ctrl.HasChildren && ctrl is not NumericUpDown && ctrl is not ComboBox)
                WireSelectAllOnFocus(ctrl);
        }
    }

    /// <summary>
    /// يوصّل أحداث الفأرة لمصدر ضغطة واحد (الحقل أو محرره الداخلي) نيابةً عن الحقل المنطقي الهدف.
    /// MouseDown: تفعيل الانتظار فقط عند دخول المؤشر بهذه الضغطة. MouseMove: إلغاء الانتظار عند السحب.
    /// MouseUp: تنفيذ التحديد مؤجَّلاً (BeginInvoke) بعد أن يستقر موضع المؤشر.
    /// </summary>
    private static void AttachSelectAllMouseHandlers(Control mouseSource, Control target, SelectAllState state)
    {
        mouseSource.MouseDown += (s, e) =>
        {
            try
            {
                if (e.Button != MouseButtons.Left)
                {
                    state.PendingSelectAll = false;
                    return;
                }

                // ضغطة داخل حقل يحمل المؤشر مسبقاً ⇒ لا Enter هنا ⇒ سلوك طبيعي (تحديد موضع المؤشر).
                state.PendingSelectAll = state.EnteredByMouse;
            }
            catch
            {
                // تجاهل
            }
        };

        mouseSource.MouseMove += (s, e) =>
        {
            try
            {
                // سحب لتحديد جزء من النص ⇒ لا نُلغِ اختيار المستخدم عند رفع الزر.
                if ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left)
                    state.PendingSelectAll = false;
            }
            catch
            {
                // تجاهل
            }
        };

        mouseSource.MouseUp += (s, e) =>
        {
            try
            {
                bool shouldSelect = e.Button == MouseButtons.Left && state.PendingSelectAll && target.CanSelect;
                state.PendingSelectAll = false;
                state.EnteredByMouse = false;

                if (!shouldSelect) return;

                // BeginInvoke: التحديد بعد انتهاء معالجة الضغطة كي لا يعيد المؤشر إلغاءه.
                target.BeginInvoke((Action)(() =>
                {
                    try
                    {
                        SelectAllTarget(target);
                    }
                    catch
                    {
                        // تجاهل
                    }
                }));
            }
            catch
            {
                // تجاهل
            }
        };
    }

    /// <summary>
    /// تظليل الحقل النشط: عند دخول المؤشر إلى حقل إدخال تُصبغ خلفيته بلون التظليل،
    /// وعند مغادرته تُعاد خلفيته الأصلية. قاموس الألوان الأصلية واحد لكل استدعاء (يُبنى مرة واحدة)
    /// ⇒ الاستعادة صحيحة دائماً عند نداء <see cref="WireFocusHighlight"/> مرة واحدة لكل نموذج.
    /// </summary>
    public static void WireFocusHighlight(Control parent, Color focusColor)
    {
        var originalColors = new Dictionary<Control, Color>();
        WireFocusHighlightCore(parent, focusColor, originalColors);
    }

    private static void WireFocusHighlightCore(Control parent, Color focusColor, Dictionary<Control, Color> originalColors)
    {
        foreach (Control ctrl in parent.Controls)
        {
            if (IsFocusHighlightTarget(ctrl))
            {
                var input = ctrl;
                originalColors[input] = input.BackColor;

                input.Enter += (s, e) =>
                {
                    try
                    {
                        input.BackColor = focusColor;
                    }
                    catch
                    {
                        // تجاهل
                    }
                };

                input.Leave += (s, e) =>
                {
                    try
                    {
                        input.BackColor = originalColors.TryGetValue(input, out var original)
                            ? original
                            : SystemColors.Window;
                    }
                    catch
                    {
                        // تجاهل
                    }
                };
            }

            if (ctrl.HasChildren)
                WireFocusHighlightCore(ctrl, focusColor, originalColors);
        }
    }

    /// <summary>
    /// التنقل بالأسهم بين حقول الإدخال: السهم الأعلى/الأيمن ينتقل للحقل السابق،
    /// والأسفل/الأيسر ينتقل للحقل التالي (مثل Tab). لا يُطبَّق على القوائم المنسدلة
    /// ولا على الشبكات حتى تحتفظ بسلوك الأسهم الأصلي، ولا على الحقول عديدة الأسطر
    /// (حتى يبقى تحريك المؤشر داخلها) ولا المقروءة فقط.
    /// لا تُمسّ مفاتيح HOME / END / PAGE-UP / PAGE-DOWN.
    /// </summary>
    public static void WireArrowNavigation(Control parent)
    {
        foreach (Control ctrl in parent.Controls)
        {
            if (IsArrowNavigationInput(ctrl) && MarkWired(ArrowNavigationWired, ctrl))
            {
                var input = ctrl;
                input.KeyDown += (s, e) =>
                {
                    try
                    {
                        if (e.Modifiers != Keys.None) return;

                        // RTL: السهم الأيمن/الأعلى = الحقل السابق، الأيسر/الأسفل = الحقل التالي.
                        bool backward = e.KeyCode == Keys.Up || e.KeyCode == Keys.Right;
                        bool forward = e.KeyCode == Keys.Down || e.KeyCode == Keys.Left;

                        if (!backward && !forward) return; // بقية المفاتيح (HOME/END/PAGE-*) بلا تدخل

                        // الحاوية الأقرب أولاً (GroupBox/Panel/TabPage) حتى يتبع التنقل ترتيب TabIndex
                        // داخل المجموعة نفسها، ثم النموذج كحل أخير.
                        var container = FindNavigationContainer(input);
                        if (container is null) return;

                        container.SelectNextControl(input, forward, true, true, true);

                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    catch
                    {
                        // تجاهل
                    }
                };
            }

            if (ctrl.HasChildren)
                WireArrowNavigation(ctrl);
        }
    }

    /// <summary>
    /// أقرب حاوية تحتوي حقلين قابلين للتركيز على الأقل: نصعد من الأب المباشر حتى نجدها،
    /// وإلا نرجع إلى النموذج (حدود GroupBox/Panel لا تُعبَر إلا عند عدم وجود بديل داخلها).
    /// </summary>
    private static Control? FindNavigationContainer(Control input)
    {
        for (var node = input.Parent; node is not null; node = node.Parent)
        {
            if (node is Form) break; // النموذج آخر خيار ⇒ حُسم أدناه (fallback)
            if (CountFocusableChildren(node) >= 2) return node;
        }

        return input.FindForm() ?? input.Parent;
    }

    /// <summary>يعدّ الأبناء القابلين للتركيز (حتى 2 يكفي) — بلا LINQ لأنه يُنفَّذ مع كل سهم.</summary>
    private static int CountFocusableChildren(Control container)
    {
        int count = 0;

        foreach (Control child in container.Controls)
        {
            if (IsFocusableSibling(child))
            {
                count++;
                if (count >= 2) return count;
            }
        }

        return count;
    }

    private static bool IsFocusableSibling(Control ctrl)
        => ctrl.TabStop && ctrl.Enabled && ctrl.Visible && ctrl.CanSelect;

    private static bool IsFocusHighlightTarget(Control ctrl) => ctrl switch
    {
        TextBox tb => !tb.ReadOnly,
        MaskedTextBox mtb => !mtb.ReadOnly,
        NumericUpDown nud => !nud.ReadOnly,
        ComboBox cb => true,
        _ => false,
    };

    private static bool IsArrowNavigationInput(Control ctrl) => ctrl switch
    {
        // الحقول عديدة الأسطر تُستثنى حتى تبقى الأسهم لتحريك المؤشر داخل النص.
        TextBox tb => !tb.ReadOnly && !tb.Multiline,
        MaskedTextBox mtb => !mtb.ReadOnly,
        NumericUpDown nud => !nud.ReadOnly,
        _ => false,
    };

    private static bool IsEnterNavigationInput(Control ctrl)
        => ctrl is TextBox or MaskedTextBox or ComboBox or NumericUpDown or DateTimePicker;

    private static bool IsSelectAllTarget(Control ctrl)
    {
        // استثناءات صريحة بالاسم: حقل البحث وحقل تصفية الشبكة.
        if (string.Equals(ctrl.Name, SearchControlName, StringComparison.OrdinalIgnoreCase))
            return false;

        if (string.Equals(ctrl.Name, FilterValueControlName, StringComparison.OrdinalIgnoreCase))
            return false;

        // الشبكة ومحرر تحرير الخلية لا يُحدَّد نصهما تلقائياً.
        if (ctrl is DataGridView || ctrl is IDataGridViewEditingControl)
            return false;

        return ctrl switch
        {
            // MaskedTextBox لا يملك Multiline (أحادي السطر دائماً).
            MaskedTextBox mtb => !mtb.ReadOnly && mtb.PasswordChar == '\0',
            TextBox tb => !tb.ReadOnly && !tb.Multiline && tb.PasswordChar == '\0',
            NumericUpDown nud => !nud.ReadOnly,
            ComboBox cb => cb.DropDownStyle != ComboBoxStyle.DropDownList,
            _ => false,
        };
    }

    /// <summary>يحدد كل قيمة الحقل الحالية بحسب نوعه — القلب المشترك للوحة المفاتيح والفأرة.</summary>
    private static void SelectAllTarget(Control target)
    {
        switch (target)
        {
            case MaskedTextBox mtb when mtb.CanSelect:
                mtb.SelectAll();
                break;

            case TextBox tb when tb.CanSelect:
                tb.SelectAll();
                break;

            case NumericUpDown nud:
                nud.Select(0, nud.Text.Length);
                break;

            case ComboBox cb when cb.DropDownStyle != ComboBoxStyle.DropDownList && cb.CanSelect:
                cb.SelectAll();
                break;
        }
    }

    private static void SelectAllCurrentValue(object? sender, EventArgs e)
    {
        try
        {
            if (sender is Control ctrl)
                SelectAllTarget(ctrl);
        }
        catch
        {
            // تجاهل — الخطأ في معالج يعمل مع كل ضغطة يجب ألا يُسقط النموذج.
        }
    }

    /// <summary>يسجّل الحقل في جدول التوصيل؛ يرجع false إذا كان موصولاً سابقاً (منع التوصيل المزدوج).</summary>
    private static bool MarkWired(ConditionalWeakTable<Control, object> table, Control ctrl)
    {
        try
        {
            if (table.TryGetValue(ctrl, out _)) return false;

            table.Add(ctrl, WiredMarker);
            return true;
        }
        catch
        {
            // مفتاح مُضاف بالفعل (حالة نادرة جداً) ⇒ لا نُوصِّل مرة ثانية.
            return false;
        }
    }

    public static void WireEnterToClick(Control trigger, Button target)
    {
        trigger.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
            {
                target.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        };
    }

    public static void WireEnterToClick(Control trigger, Action action)
    {
        trigger.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
            {
                action();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        };
    }

    public static void WireGlobalShortcuts(Form form, params (Keys Key, Action Action)[] shortcuts)
    {
        form.KeyPreview = true;
        form.KeyDown += (s, e) =>
        {
            foreach (var (key, action) in shortcuts)
            {
                if (e.KeyData == key)
                {
                    action();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                }
            }
        };
    }

    /// <summary>
    /// يوجّه <c>Enter</c> إلى الزر الأساسي للوضع الحالي (RULE-N147):
    /// وضع العرض ⇒ «جديد» · وضع الإضافة ⇒ «إضافة/حفظ» · وضع التعديل ⇒ «تعديل».
    /// <para>
    /// لا يعمل عندما يكون المؤشر داخل حقل إدخال (Enter هناك = الحقل التالي — `RULE-N40`)،
    /// ولا على زر/مربع اختيار (Enter هناك = تنفيذ العنصر نفسه).
    /// </para>
    /// </summary>
    /// <param name="form">الشاشة.</param>
    /// <param name="primaryButton">يُرجع الزر الأساسي للوضع الحالي — يُقرأ عند كل ضغطة لا مرة واحدة.</param>
    public static void WireModeEnter(Form form, Func<Button?> primaryButton)
    {
        if (form is null || primaryButton is null) return;
        if (!MarkWired(ModeEnterWired, form)) return;

        form.KeyPreview = true;
        form.KeyDown += (_, e) =>
        {
            if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None) return;

            // Enter داخل حقل إدخال أو على زر = سلوك العنصر نفسه، لا زر الوضع.
            var focused = FindFocusedControl(form);
            if (focused is TextBoxBase or ComboBox or NumericUpDown or DateTimePicker or ButtonBase) return;

            var primary = primaryButton();
            if (primary is null || primary.IsDisposed || !primary.Enabled || !primary.Visible) return;

            e.Handled = true;
            e.SuppressKeyPress = true;
            primary.PerformClick();
        };
    }

    /// <summary>يعيد العنصر الذي يحمل التركيز فعلياً داخل الشجرة (null = التركيز على الشاشة نفسها).</summary>
    private static Control? FindFocusedControl(Control root)
    {
        foreach (Control child in root.Controls)
        {
            if (child.Focused) return child;

            var nested = FindFocusedControl(child);
            if (nested is not null) return nested;
        }

        return null;
    }

    /// <summary>
    /// يمنح الأزرار شكلاً واضحاً عند تعطيلها (خلفية رمادية + نص باهت) بدلاً من المظهر الافتراضي غير الواضح.
    /// <para>
    /// التوقيع باقٍ كما هو لأن أكثر من 60 شاشة تستدعيه؛ والتنفيذ موحَّد في
    /// <see cref="WireDisabledVisual"/> حتى لا تتكرر قيم الألوان (RULE-N112 / RULE-N98).
    /// </para>
    /// </summary>
    public static void WireButtonDisabledVisual(params Button[] buttons)
    {
        if (buttons is null) return;

        // Button : ButtonBase : Control — نفس التوصيل الموحّد بلا ألوان مكررة هنا.
        WireDisabledVisual(buttons);
    }

    /// <summary>
    /// يوصّل مظهر التعطيل الموحّد (RULE-N43) لعنصر إدخال أو زر واحد أو أكثر:
    /// خلفية <see cref="GreenThemeHelper.DisabledBack"/> ونص <see cref="GreenThemeHelper.DisabledFore"/>
    /// أثناء <c>Enabled == false</c>، واللونان الطبيعيان عند التفعيل — تماماً كما في الأزرار.
    /// <para>
    /// اللونان من <see cref="GreenThemeHelper"/> وحدهما (RULE-N112)، واللونان الطبيعيان
    /// يُلتقطان لحظة التوصيل ويُحدَّثان لحظة كل تعطيل (لأن الثيم قد يلوّن العنصر بعد التوصيل).
    /// </para>
    /// <para>
    /// <c>CheckBox</c>/<c>RadioButton</c> يُباهت نصّها فقط ولا تُلوَّن خلفيتها رمادياً (المربع نفسه
    /// يُعطَّل أصلياً) — بخلاف <c>Button</c> الذي يأخذ الخلفية الرمادية كاملة.
    /// </para>
    /// <para>آمن للاستدعاء المتكرر: التوصيل مرة واحدة لكل عنصر، والتوصيل المزدوج لا يُضاعف المعالجات.</para>
    /// </summary>
    /// <param name="controls">عناصر الإدخال/الأزرار المطلوب توصيلها.</param>
    public static void WireDisabledVisual(params Control[] controls)
    {
        if (controls is null) return;

        foreach (var control in controls)
        {
            if (control is null) continue;

            try
            {
                if (!MarkWired(DisabledVisualWired, control)) continue;

                // الخلفية الشفافة (طبقة التحميل) لا تُعاد تلوينها إطلاقاً (RULE-N129).
                if (control.BackColor.A < 255) continue;

                var normalBack = control.BackColor;
                var normalFore = control.ForeColor;
                bool wasEnabled = IsEffectivelyEnabled(control);

                // CheckBox/RadioButton يرسمان BackColor كمستطيل خلف النص، فتعطيلهما
                // يُباهت النص فقط بلا مسحة رمادية (المربع الصغير يُعطَّل أصلياً).
                bool paintsBacking = control is not CheckBox && control is not RadioButton;

                void Apply()
                {
                    try
                    {
                        bool nowEnabled = IsEffectivelyEnabled(control);

                        // التقاط اللون الطبيعي لحظة التعطيل الفعلي فقط: لا يُلتقط الرمادي
                        // كلون طبيعي عند تعطيل حاوية فوق عنصر معطّل أصلاً، ولا يُستعاد
                        // لون قديم غيّره الثيم بعد لحظة التوصيل.
                        if (wasEnabled && !nowEnabled)
                        {
                            // الخلفية تُلتقط لما يرسم خلفية فقط، فلا يتذكّر مربع الاختيار
                            // لوناً رمادياً أبداً ويبقى نصّه وحده هو المتغيّر.
                            if (paintsBacking) normalBack = control.BackColor;
                            normalFore = control.ForeColor;
                        }

                        wasEnabled = nowEnabled;

                        if (paintsBacking)
                            control.BackColor = nowEnabled ? normalBack : GreenThemeHelper.DisabledBack;

                        control.ForeColor = nowEnabled ? normalFore : GreenThemeHelper.DisabledFore;
                    }
                    catch
                    {
                        // تجاهل — لون لا يُسقط شاشة.
                    }
                }

                control.EnabledChanged += (s, e) => Apply();

                // تعطيل حاوية كاملة (gbEntry / pnlFooter / pnlHeader) لا يُغيّر Enabled
                // المحلي للأبناء، فلا بدّ من الاستماع لكل حاوية أعلى العنصر أيضاً (RULE-N43).
                for (var ancestor = control.Parent; ancestor is not null; ancestor = ancestor.Parent)
                    ancestor.EnabledChanged += (s, e) => Apply();

                Apply();
            }
            catch
            {
                // تجاهل — عنصر غير قابل لإعادة التلوين يُترك كما هو.
            }
        }
    }

    /// <summary>
    /// هل العنصر مفعّل فعلياً؟ (تعطيل الحاوية يمنع أبناءها من العمل دون تغيير
    /// <c>Enabled</c> المحلي لكل ابن — فالصعود إلى الجذر هو القياس الصحيح.)
    /// </summary>
    private static bool IsEffectivelyEnabled(Control control)
    {
        for (Control? node = control; node is not null; node = node.Parent)
        {
            if (!node.Enabled) return false;
        }

        return true;
    }

    /// <summary>
    /// يوصّل مظهر التعطيل الموحّد (<see cref="WireDisabledVisual(Control[])"/>) لكل حقول
    /// الإدخال والأزرار داخل شجرة العناصر recursively — يُستدعى تلقائياً من
    /// <see cref="GreenThemeHelper.Apply(Control)"/> فتغطيه كل شاشة بلا كود داخلها (RULE-N112).
    /// <para>
    /// يُوصَّل: <c>TextBox</c>/<c>MaskedTextBox</c>/<c>RichTextBox</c> (TextBoxBase) ·
    /// <c>ComboBox</c> · <c>NumericUpDown</c> · <c>DateTimePicker</c> ·
    /// <c>Button</c>/<c>CheckBox</c>/<c>RadioButton</c> (ButtonBase) · <c>DomainUpDown</c> · <c>ListBox</c>.
    /// </para>
    /// <para>
    /// لا يُوصَّل ولا يُنزَل داخله: <c>Label</c> · <c>Panel</c>/<c>TableLayoutPanel</c>/
    /// <c>FlowLayoutPanel</c> · <c>GroupBox</c> · <c>DataGridView</c> · <c>PictureBox</c> ·
    /// <c>ProgressBar</c> · <c>TabControl</c> (يُنزَل إلى أبنائها) · <c>UserControl</c> (يُنزَل
    /// إلى أبنائه) — كما لا يُنزَل داخل <c>NumericUpDown</c>/<c>ComboBox</c> لأن محرّرهما
    /// الداخلي ليس حقلاً مستقلاً، ولا يُلمس أي عنصر بخلفية شفافة (RULE-N129).
    /// </para>
    /// </summary>
    /// <param name="root">جذر الشجرة (النموذج أو أي حاوية).</param>
    public static void WireDisabledVisuals(Control root)
    {
        if (root is null) return;

        foreach (Control child in root.Controls)
        {
            if (child is null) continue;

            if (IsDisabledVisualTarget(child))
            {
                WireDisabledVisual(child);
                continue;   // لا ننزل داخل الحقل نفسه (محرر NumericUpDown/ComboBox الداخلي)
            }

            if (child.HasChildren)
                WireDisabledVisuals(child);
        }
    }

    /// <summary>
    /// هل هذا العنصر من حقول الإدخال/الأزرار التي يجب أن تأخذ مظهر التعطيل؟
    /// (الحاويات والتسميات والشبكات وغيرها لا تُوصَّل — ويُنزَل إلى أبنائها بدلاً من ذلك.)
    /// </summary>
    private static bool IsDisabledVisualTarget(Control ctrl) => ctrl switch
    {
        // حاويات وكتل عرض: لا تُوصَّل (ننزل إلى أبنائها).
        DataGridView => false,
        PictureBox => false,
        ProgressBar => false,
        TabControl => false,
        UserControl => false,
        GroupBox => false,
        Panel => false,          // يشمل FlowLayoutPanel / TableLayoutPanel
        Label => false,

        // حقول الإدخال والأزرار: تُوصَّل.
        TextBoxBase => true,     // TextBox / MaskedTextBox / RichTextBox
        ComboBox => true,
        NumericUpDown => true,
        DateTimePicker => true,
        ButtonBase => true,      // Button / CheckBox / RadioButton
        DomainUpDown => true,
        ListBox => true,

        _ => false,
    };
}
