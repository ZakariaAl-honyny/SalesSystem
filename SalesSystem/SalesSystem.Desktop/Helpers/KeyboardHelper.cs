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

    /// <summary>
    /// سجل حقول الإدخال التي وُصِّل لها <c>Enter</c> (منع التوصيل المزدوج).
    /// <para>
    /// الفرعان يشتركان في الحارس نفسه — <see cref="WireEnterAsTab(Control)"/> و
    /// <see cref="WireEnterAsTab(Control, Func{Button?})"/> — لأن اشتراك الحقل مرتين يُنفّذ
    /// معالج الخطوة مرتين فينتقل المؤشر خطوتين بضغطة واحدة.
    /// </para>
    /// <para>
    /// الحارس يمنع **الاشتراك الثاني** فقط ولا يغيّر سلوك الاشتراك الأول: الفرع ذو الوسيط الواحد
    /// كان يشترك بلا حارس، وسلوكه المرئي يبقى كما هو.
    /// </para>
    /// </summary>
    private static readonly ConditionalWeakTable<Control, object> EnterNavigationWired = new();

    /// <summary>
    /// سجل العناصر التي أُعلن لها <c>Enter</c> مفتاح إدخال (منع الإعلان المزدوج).
    /// <para>
    /// جدول مستقل عن <see cref="EnterNavigationWired"/> لأن الإعلان يجب أن يصل إلى **الأبناء الداخليين**
    /// للعناصر المركّبة (<c>NumericUpDown</c> ⇒ <c>_upDownEdit</c>) وهم ليسوا حقول تنقل مستقلة،
    /// فلا يصح أن يدخلوا في جدول توصيل الـ<em>KeyDown</em> (انظر <see cref="DeclareEnterAsInputKey"/>).
    /// </para>
    /// </summary>
    private static readonly ConditionalWeakTable<Control, object> EnterInputKeyDeclared = new();

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
            if (IsEnterNavigationInput(ctrl) && MarkWired(EnterNavigationWired, ctrl))
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
    /// يحوّل <c>Enter</c> إلى انتقال للحقل التالي **بترتيب العرض** داخل نطاق محدد (RULE-N191):
    /// حقلاً بحقل في الصف نفسه من اليمين إلى اليسار (RTL) ثم إلى الصف الذي يليه أدناه،
    /// وبعد **آخر** حقل يُركِّز زر الوضع الأساسي الحالي بدل الدوران إلى أول حقل.
    /// <para>
    /// <b>نطاق التنقل</b> هو <paramref name="scope"/> (لوحة حقول المحرر عادةً): الحقول داخله تتبع
    /// ترتيب العرض، والحقول خارجه (حقل البحث وأدوات التصفية) تحتفظ بسلوك
    /// <see cref="WireEnterAsTab(Control)"/> نفسه (<c>SelectNextControl</c> حسب <c>TabIndex</c>)
    /// — فلا تتغيّر تصفية الشاشة.
    /// </para>
    /// <para>
    /// <b>ترتيب العرض</b> يُحسب من إحداثيات الشاشة عبر <c>PointToScreen(Point.Empty)</c> لا من
    /// <c>TabIndex</c> ولا من <c>Location</c> المخزّن، فيطابق ما يراه المستخدم فعلاً: صفوف من الأعلى
    /// إلى الأسفل، وفي كل صف حقول الصف نفسه (فرق رأسي لا يزيد عن نصف ارتفاع أول حقل في الصف وبحد أدنى
    /// <see cref="NavigationOptions.RowBandTolerancePixels"/>) من اليمين إلى اليسار في RTL. والقائمة
    /// تُعاد بناؤها عند كل ضغطة لأن أوضاع الشاشة (عرض/إضافة/تعديل) تغيّر تفعيل الحقول.
    /// </para>
    /// <para>
    /// <b>قيم السلوك من بيتها الواحد (<c>RULE-N192</c>):</b> ترتيب التنقل
    /// (<see cref="NavigationOptions.EnterFollowsDisplayOrder"/>) والخروج إلى زر الوضع
    /// (<see cref="NavigationOptions.EnterExitsToModeButton"/>) ونطاق الصف
    /// (<see cref="NavigationOptions.RowBandTolerancePixels"/>) تُقرأ حيّة من
    /// <see cref="NavigationOptions"/> عند كل ضغطة — فلا قيمة حرفية هنا، ولا شاشة تضبطها.
    /// </para>
    /// <para>
    /// <b>لا تنفيذ بلا طلب:</b> الزر الأساسي يُركَّز فقط ولا يُنقَر (<c>PerformClick</c> ممنوعة هنا)،
    /// فالحفظ/الترحيل يبقى على زرّه صراحةً ولا يحفظ <c>Enter</c> وحده أبداً.
    /// </para>
    /// </summary>
    /// <param name="scope">نطاق التنقل بترتيب العرض (لوحة حقول المحرر).</param>
    /// <param name="primaryButton">يُرجع الزر الأساسي للوضع الحالي (RULE-N147) — يُقرأ عند كل ضغطة لا مرة واحدة.</param>
    public static void WireEnterAsTab(Control scope, Func<Button?> primaryButton)
    {
        if (scope is null || primaryButton is null) return;

        // الاشتراك في النموذج كله (لا في النطاق وحده) حتى يبقى Enter في حقل البحث/التصفية يعمل كما هو،
        // والقرار «داخل النطاق أم خارجه» يُتخذ عند كل ضغطة في MoveNextByDisplayOrder.
        var root = scope.FindForm() ?? scope;
        WireEnterDisplayOrder(root, scope, primaryButton);
    }

    /// <summary>
    /// يوصّل Enter على كل حقول الإدخال في الشجرة (الحقول خارجه تُوجَّه إلى سلوك <c>SelectNextControl</c>).
    /// لا ينزل داخل <c>NumericUpDown</c>/<c>ComboBox</c> **لتوصيل التنقل** لأن محررهما الداخلي ليس حقلاً
    /// مستقلاً (ولو نزلنا لاعتُبر حقلاً ثانياً فانتقل المؤشر خطوتين بضغطة واحدة) — أما إعلان
    /// <c>Enter</c> مفتاحَ إدخال فينزل إلى الأبناء عمداً عبر <see cref="DeclareEnterAsInputKey"/>
    /// لأن <c>PreviewKeyDown</c> يُرفع على مالك المقبض لا على الأب.

    /// <para>
    /// <b>لماذا <c>PreviewKeyDown</c> إلزامي هنا (RULE-N153 / RULE-N191) — لا تحذفه:</b>
    /// ترتيب الإطار (مصدر .NET WinForms) هو
    /// <c>WM_KEYDOWN</c> ⇒ <c>Control.PreProcessControlMessageInternal</c> ⇒ يرفع
    /// <c>PreviewKeyDown</c> أولاً، فإن أُعلن فيه <c>IsInputKey = true</c> أعاد
    /// <c>MessageNeeded</c> **فوراً** وتُوصَل الرسالة عادياً فيُرفع <c>KeyDown</c>؛
    /// وإلا استمر إلى <c>PreProcessMessage</c> ⇒ <c>ProcessDialogKey</c> ⇒
    /// <c>Form.ProcessDialogKey</c> الذي على <c>Keys.Return</c> يُنفّذ
    /// <c>AcceptButton.PerformClick()</c> ويُعيد <c>true</c> فيبتلع المفتاح.
    /// و<c>TextBoxBase.IsInputKey</c> لا يَعُدّ <c>Enter</c> مفتاح إدخال أبداً
    /// (Tab/Escape/Back/PageUp/PageDown/Home/End فقط).
    /// </para>
    /// <para>
    /// النتيجة بدون هذا الإعلان: كل شاشة تضبط <c>AcceptButton</c> (RULE-N147) ينقر Enter فيها
    /// زر الوضع ولا يصل <c>KeyDown</c> إلى معالج التنقل أصلاً — وهو العيب الذي رصده المالك
    /// («Enter يقفز إلى الزر بدل أن يمشي صفاً صفاً»). بحذف هذا الاشتراك يعود العيب حرفياً.
    /// </para>
    /// </summary>
    private static void WireEnterDisplayOrder(Control parent, Control scope, Func<Button?> primaryButton)
    {
        foreach (Control ctrl in parent.Controls)
        {
            if (IsEnterNavigationInput(ctrl) && MarkWired(EnterNavigationWired, ctrl))
            {
                var input = ctrl;

                // Enter مفتاح إدخال على الحقل **وعلى أبنائه الداخليين** ⇒ يُرفع KeyDown قبل
                // ProcessDialogKey/AcceptButton (RULE-N153 / RULE-N191 / RULE-N193).
                DeclareEnterAsInputKey(input);

                input.KeyDown += (s, e) =>
                {
                    if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None) return;

                    // معالج سابق في الشاشة استهلك Enter (باركود RULE-N42 · بحث RULE-N59): لا ننقل التركيز
                    // أيضاً — وإلا نُفِّذ الأمران بضغطة واحدة. كانت هذه المعالجات ميتة لأن
                    // ProcessDialogKey كان يبتلع Enter قبل KeyDown (RULE-N153).
                    if (e.Handled) return;

                    // القائمة المنسدلة المفتوحة تملك Enter: يُثبّت العنصر المظلَّل ويُغلق القائمة
                    // بسلوكه الأصلي (لا تحريك تركيز ولا ابتلاع للمفتاح).
                    if (input is ComboBox { DroppedDown: true }) return;

                    try
                    {
                        // الحكم قبل النقل: نقل التركيز يُطلق Leave بعد مغادرة الحقل فتظهر العلامة في المكان الخطأ (RULE-N194).
                        // SuppressKeyPress يمنع رنّة ويندوز المصاحبة للمفتاح غير المُنفَّذ.
                        if (RequiredFieldHelper.BlocksEnterNavigation(input))
                        {
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                            return;
                        }

                        MoveNextByDisplayOrder(input, scope, primaryButton);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    catch
                    {
                        // تجاهل — حقل مُتلَف أو إحداثيات غير متاحة يجب ألا تُسقط الشاشة.
                    }
                };
            }

            if (ctrl.HasChildren && ctrl is not NumericUpDown && ctrl is not ComboBox)
                WireEnterDisplayOrder(ctrl, scope, primaryButton);
        }
    }

    /// <summary>
    /// يعلن <c>Enter</c> مفتاحَ إدخال على <paramref name="input"/> **وعلى كل عنصر داخلي تحته**، مرة واحدة لكل عنصر.
    /// <para>
    /// <b>لماذا النزول إلى الأبناء؟</b> الإطار يرفع <c>PreviewKeyDown</c> على العنصر الذي يملك مقبض الرسالة
    /// (<c>Control.PreProcessControlMessageInternal</c> ⇒ <c>FromChildHandle(message.HWnd)</c>) لا على الأب.
    /// والعناصر المركّبة تنشئ حقلاً داخلياً وتعطي **له** التركيز — مثل <c>NumericUpDown</c>
    /// (<c>UpDownBase</c>) الذي حقلُه <c>_upDownEdit</c> (<c>UpDownBase.Focused =&gt; _upDownEdit.Focused</c>).
    /// فإعلان المفتاح على <c>NumericUpDown</c> وحده **لا يُنفَّذ أبداً**، ويبقى <c>Form.ProcessDialogKey</c>
    /// ينقر <c>AcceptButton</c> داخل الحقول الرقمية (RULE-N191 / RULE-N193). الإعلان على الأبناء هو ما يُغلق الطريق.
    /// </para>
    /// <para>
    /// <b>ولا يُوصَّل <c>KeyDown</c> على الأبناء:</b> <c>UpDownBase</c> يشترك <c>_upDownEdit.KeyDown</c>
    /// ويُنادي <c>OnKeyDown(e)</c> فيصل الحدث إلى <c>NumericUpDown</c> الأب أصلاً — واشتراك ثانٍ على الابن
    /// يعني تنقّلاً خطوتين بضغطة واحدة. اشتراك التنقل يبقى على الحقل الأب وحده في <see cref="WireEnterDisplayOrder"/>.
    /// </para>
    /// </summary>
    private static void DeclareEnterAsInputKey(Control input)
    {
        if (input is null) return;

        if (MarkWired(EnterInputKeyDeclared, input))
        {
            input.PreviewKeyDown += (s, e) =>
            {
                try
                {
                    if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
                        e.IsInputKey = true;
                }
                catch
                {
                    // تجاهل — إعلان المفتاح يجب ألا يُسقط الشاشة.
                }
            };
        }

        // العنصر المركّب يضع التركيز في ابن داخلي ⇒ الإعلان يجب أن يصل إليه هو أيضاً
        // (الحارس أعلاه يمنع الاشتراك المزدوج عند تكرار المشي على نفس الشجرة).
        foreach (Control child in input.Controls)
            DeclareEnterAsInputKey(child);
    }

    /// <summary>
    /// خطوة Enter الواحدة: داخل النطاق ⇒ التالي في ترتيب العرض (أو حسب <c>TabIndex</c> إذا غيّر المالك
    /// <see cref="NavigationOptions.EnterFollowsDisplayOrder"/>)، أو زر الوضع عند آخر حقل (تركيزاً فقط)؛
    /// خارج النطاق (البحث/التصفية) ⇒ <c>SelectNextControl</c> كما في <see cref="WireEnterAsTab(Control)"/>.
    /// </summary>
    private static void MoveNextByDisplayOrder(Control input, Control scope, Func<Button?> primaryButton)
    {
        if (!IsInsideScope(input, scope))
        {
            // البحث/التصفية: سلوك اليوم نفسه بلا أي تغيير (RULE-N59).
            var container = input.FindForm() ?? scope;
            container.SelectNextControl(input, true, true, true, true);
            return;
        }

        // (1) الحقل التالي — ترتيب العرض أو ترتيب TabIndex، حسب الخيار الحيّ في NavigationOptions (RULE-N192).
        //     القائمة تبقى فارغة في وضع TabIndex (لا تُبنى بلا داعٍ).
        var order = new List<Control>();

        if (NavigationOptions.EnterFollowsDisplayOrder)
        {
            order = CollectInputsInDisplayOrder(scope);
            int index = order.IndexOf(input);

            if (index >= 0)
            {
                if (index + 1 < order.Count)
                {
                    order[index + 1].Focus();
                    return;
                }
            }
            else
            {
                // الحقل ليس في السلسلة (نحو txtCode: للقراءة فقط + TabStop = false ⇒ يُبلَغ بالفأرة
                // ولا يُجمَع) ⇒ التالي **في ترتيب العرض** نفسه، لا القفز إلى زر الوضع (RULE-N191).
                var nextAfter = FindFirstAfterInDisplayOrder(order, input, scope.FindForm());

                if (nextAfter is not null)
                {
                    nextAfter.Focus();
                    return;
                }
            }
        }
        else
        {
            // ترتيب TabIndex المعلن في المصمّم (nested: true يعبر حدود الحاويات) وبلا دوران،
            // حتى يُحسم الخروج أدناه. الإرجاع false = لا حقل تالٍ.
            var formByTabIndex = input.FindForm();

            if (formByTabIndex is not null &&
                formByTabIndex.SelectNextControl(input, true, true, true, false))
            {
                return;
            }
        }

        // (2) لا حقل تالٍ (أو حقل خارج السلسلة مثل حقل الرقم للقراءة فقط) ⇒ زر الوضع الحالي؟
        //     تركيزاً فقط بلا تنفيذ، ودائماً حسب الخيار الحيّ (RULE-N192).
        if (NavigationOptions.EnterExitsToModeButton)
        {
            Button? primary = null;
            try
            {
                primary = primaryButton();
            }
            catch
            {
                // تجاهل — حكم الزر الأساسي لا يُسقط التنقل.
            }

            if (primary is not null && !primary.IsDisposed && primary.Enabled && primary.Visible && primary.CanFocus)
            {
                primary.Focus();
                return;
            }
        }

        // (3) لا خروج إلى الزر (أو لا زر أساسي متاح) ⇒ أول حقل في السلسلة حتى لا يبقى المستخدم عالقاً.
        if (order.Count > 0)
        {
            order[0].Focus();
            return;
        }

        // سلسلة ترتيب العرض لم تُبنَ (وضع TabIndex) ⇒ الدوران (wrap: true) هو سلوك SelectNextControl الأصلي.
        var wrapContainer = input.FindForm() ?? scope;
        wrapContainer.SelectNextControl(input, true, true, true, true);
    }

    /// <summary>هل الحقل داخل النطاق المطلوب؟ (الصعود في سلسلة <c>Parent</c> حتى النطاق).</summary>
    private static bool IsInsideScope(Control control, Control scope)
    {
        for (var node = control; node is not null; node = node.Parent)
        {
            if (ReferenceEquals(node, scope)) return true;
        }

        return false;
    }

    /// <summary>حقول الإدخال القابلة للتنقل داخل النطاق الآن، مرتَّبة بترتيب العرض.</summary>
    private static List<Control> CollectInputsInDisplayOrder(Control scope)
    {
        var inputs = new List<Control>();
        CollectNavigableInputs(scope, inputs);
        return OrderByDisplayRows(inputs, scope.FindForm());
    }

    /// <summary>يجمع حقول الإدخال القابلة للتنقل (نوع الحقل + الحالة الحالية) بشكل recursive.</summary>
    private static void CollectNavigableInputs(Control parent, List<Control> inputs)
    {
        foreach (Control ctrl in parent.Controls)
        {
            if (IsEnterNavigationInput(ctrl) && IsNavigableNow(ctrl))
                inputs.Add(ctrl);

            if (ctrl.HasChildren && ctrl is not NumericUpDown && ctrl is not ComboBox)
                CollectNavigableInputs(ctrl, inputs);
        }
    }

    /// <summary>
    /// هل يدخل الحقل في سلسلة تنقل Enter **الآن**؟ (مفعّل فعلياً أي مع حاوياته + ظاهر + داخل ترتيب Tab
    /// + يقبل التركيز) — تُقاس عند كل ضغطة لأن وضع الشاشة (عرض/إضافة/تعديل) يغيّر التفعيل.
    /// الحقول للقراءة فقط ذات <c>TabStop = false</c> (مثل رقم الصنف) تبقى خارج السلسلة.
    /// </summary>
    private static bool IsNavigableNow(Control ctrl)
        => ctrl.TabStop && ctrl.Visible && ctrl.CanSelect && IsEffectivelyEnabled(ctrl);

    /// <summary>
    /// يرتّب حقول الإدخال بترتيب العرض كما يراه المستخدم: صفوف من الأعلى إلى الأسفل،
    /// وفي كل صف من اليمين إلى اليسار عند RTL (ومن اليسار إلى اليمين خلافه).
    /// الإحداثيات من الشاشة (<c>PointToScreen</c>) لا من <c>TabIndex</c>، فتبقى صحيحة سواء
    /// عكس <c>RightToLeftLayout</c> إحداثيات المصمّم أم لا (لا تُقرأ ألوان ولا تُكرَّر قيم).
    /// </summary>
    private static List<Control> OrderByDisplayRows(List<Control> inputs, Form? form)
    {
        if (inputs.Count <= 1) return inputs;

        bool rightToLeft = form?.RightToLeft == RightToLeft.Yes;

        var measured = new List<(Control Control, int X, int Y, int Height)>(inputs.Count);

        foreach (var ctrl in inputs)
        {
            try
            {
                var screen = ctrl.PointToScreen(Point.Empty);
                measured.Add((ctrl, screen.X, screen.Y, Math.Max(1, ctrl.Height)));
            }
            catch
            {
                return inputs;   // تعذّر القياس (عنصر غير منشأ) ⇒ يبقى ترتيب الاجتياز كما هو
            }
        }

        measured.Sort((a, b) => a.Y.CompareTo(b.Y));

        var ordered = new List<Control>(measured.Count);
        var band = new List<(Control Control, int X, int Y, int Height)>();
        int bandTop = 0;
        int bandTolerance = 0;

        void FlushBand()
        {
            if (band.Count == 0) return;

            // داخل الصف: RTL من اليمين إلى اليسار (X تنازلياً)، وإلا من اليسار إلى اليمين.
            if (rightToLeft) band.Sort((a, b) => b.X.CompareTo(a.X));
            else band.Sort((a, b) => a.X.CompareTo(b.X));

            foreach (var item in band) ordered.Add(item.Control);
            band.Clear();
        }

        foreach (var item in measured)
        {
            if (band.Count == 0)
            {
                bandTop = item.Y;
                bandTolerance = Math.Max(NavigationOptions.RowBandTolerancePixels, item.Height / 2);
            }
            else if (item.Y - bandTop > bandTolerance)
            {
                FlushBand();
                bandTop = item.Y;
                bandTolerance = Math.Max(NavigationOptions.RowBandTolerancePixels, item.Height / 2);
            }

            band.Add(item);
        }

        FlushBand();
        return ordered;
    }

    /// <summary>
    /// أول حقل في سلسلة ترتيب العرض يأتي **بعد** حقل خارج السلسلة (حقل رقم للقراءة فقط، أو
    /// <c>TabStop = false</c>) — بنفس مقارن الصفوف واتجاه RTL المستخدم في
    /// <see cref="OrderByDisplayRows"/>، فلا منطق ترتيب ثانٍ هنا ولا في أي شاشة (RULE-N169 / RULE-N191).
    /// <para>
    /// الطريقة: الحقل الخارجي يُضاف إلى نفس قائمة السلسلة ثم تُرتَّب القائمة كلها بنفس الدالة،
    /// فيُعرف موضعه الحقيقي بين الحقول بدل تخمينه بإحداثيات منفصلة.
    /// </para>
    /// </summary>
    /// <returns>الحقل التالي، أو <c>null</c> إن لم يكن بعد الخارجي حقل في السلسلة (⇒ يُسلَّم الأمر لزر الوضع).</returns>
    private static Control? FindFirstAfterInDisplayOrder(List<Control> order, Control input, Form? form)
    {
        if (order.Count == 0) return null;

        try
        {
            var withInput = new List<Control>(order.Count + 1);
            withInput.AddRange(order);
            withInput.Add(input);

            var full = OrderByDisplayRows(withInput, form);
            int inputIndex = full.IndexOf(input);

            // موضعه غير معروف (تعذّر القياس) ⇒ لا حقل بعده ⇒ يعود السلوك إلى زر الوضع كما هو.
            if (inputIndex < 0) return null;

            for (int i = inputIndex + 1; i < full.Count; i++)
            {
                if (order.Contains(full[i])) return full[i];
            }
        }
        catch
        {
            // تجاهل — فشل القياس يُبقي السلوك القديم (زر الوضع).
        }

        return null;
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

    /// <summary>سجل الجذور التي وُصِّل لها تظليل الحقل النشط (منع التوصيل المزدوج + حصر التحديث)</summary>
    private static readonly ConditionalWeakTable<Control, object> FocusHighlightWired = new();

    /// <summary>
    /// تظليل الحقل النشط: عند دخول المؤشر إلى حقل إدخال تُصبغ خلفيته بلون التظليل،
    /// وعند مغادرته تُعاد خلفيته الأصلية. قاموس الألوان الأصلية واحد لكل استدعاء (يُبنى مرة واحدة)
    /// ⇒ الاستعادة صحيحة دائماً عند نداء <see cref="WireFocusHighlight"/> مرة واحدة لكل نموذج.
    /// <para>
    /// <b>اللون حيّ (RULE-N54a):</b> الشاشات تُمرّر اللون الحرفي القديم
    /// <c>(214,228,240)</c> — أي <see cref="ThemePalette.DefaultFor"/> لمفتاح
    /// <see cref="ThemePalette.SelectionRowColor"/> — أو <see cref="Color.Empty"/>،
    /// فيُستبدل عند كل <c>Enter</c> بالقيمة الحيّة من <see cref="ThemePalette.Get(string)"/>؛
    /// والقيمة الحيّة الافتراضية هي <c>#FFF3CD</c> = (255,243,205) — لون واحد للتحديد والحقل النشط
    /// (ولون التظليل السابق المسجَّل في <c>ThemePalette.RetiredDefaults</c> ما زال يُعاد تلوينه تلقائياً).
    /// وأي لون مخصّص تمرّره شاشة يبقى كما هو (لا نُعيد كتابة اختيار الشاشة).
    /// </para>
    /// </summary>
    public static void WireFocusHighlight(Control parent, Color focusColor)
    {
        var originalColors = new Dictionary<Control, Color>();
        WireFocusHighlightCore(parent, focusColor, originalColors);

        // وسم الجذر: RefreshFocusHighlight لا تلمس إلا الشاشات الموصولة فعلاً.
        MarkWired(FocusHighlightWired, parent);
    }

    /// <summary>
    /// يُعيد تلوين **الحقل النشط الحالي** باللون الحيّ للحقل النشط بعد تغيير اللوحة
    /// (تُستدعى من <see cref="ThemePalette.ApplyToOpenForms"/>). لا تفعل شيئاً إن لم
    /// يكن الجذر موصولاً، أو لم يكن لشجرته حقل مركَّز، أو كان الحقل معطّلاً —
    /// ولا تُغيّر أي حقل آخر، فلا تتأثر بقية ألوان الشاشة.
    /// </summary>
    public static void RefreshFocusHighlight(Control root)
    {
        if (root is null || root.IsDisposed) return;

        // الجذر غير موصول ⇒ لا تظليل أصلاً ⇒ لا شيء نُعيد تلوينه.
        if (!FocusHighlightWired.TryGetValue(root, out _)) return;

        try
        {
            var focused = FindFocusedControl(root);

            if (focused is null || !focused.Enabled || !IsFocusHighlightTarget(focused)) return;

            focused.BackColor = ThemePalette.Get(ThemePalette.SelectionRowColor);
        }
        catch
        {
            // تجاهل — لون لا يُسقط شاشة.
        }
    }

    /// <summary>أعمق عنصر يحمل التركيز داخل الشجرة، أو <c>null</c>.</summary>
    private static Control? FindFocusedControl(Control root)
    {
        if (root.Focused) return root;

        foreach (Control child in root.Controls)
        {
            var found = FindFocusedControl(child);
            if (found is not null) return found;
        }

        return null;
    }

    /// <summary>
    /// هل المؤشر داخل حقل إدخال (نصي/رقمي/قائمة/تاريخ/محرِّر خلية شبكة)؟
    /// تُستهلك لمنع اختصار Delete من حذف سجل أثناء الكتابة — بيت واحد لكل الشاشات (RULE-N169/N171).
    /// </summary>
    public static bool IsTextInputFocused(Form? form)
    {
        if (form is null) return false;
        var focus = FindFocusedControl(form);
        return focus is TextBoxBase or NumericUpDown or ComboBox or DateTimePicker
            or IDataGridViewEditingControl;
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
                        input.BackColor = ResolveFocusHighlightColor(focusColor);
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
    /// يحسم لون التظليل عند كل <c>Enter</c>: اللون الحرفي القديم
    /// <c>(214,228,240)</c> = <see cref="ThemePalette.DefaultFor"/> لمفتاح
    /// <see cref="ThemePalette.SelectionRowColor"/>،
    /// أو <see cref="Color.Empty"/> ⇒ القيمة الحيّة من اللوحة (الافتراضي <c>#FFF3CD</c> = (255,243,205))؛ وأي لون آخر
    /// (اختيار الشاشة المخصّص) يُحترم كما هو. لا لون حرفي هنا — المصدر <see cref="ThemePalette"/>.
    /// </summary>
    private static Color ResolveFocusHighlightColor(Color requested)
    {
        if (requested.IsEmpty)
            return ThemePalette.Get(ThemePalette.SelectionRowColor);

        int requestedArgb = requested.ToArgb();

        return requestedArgb == ThemePalette.DefaultFor(ThemePalette.SelectionRowColor).ToArgb()
            ? ThemePalette.Get(ThemePalette.SelectionRowColor)
            : requested;
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
                    // Delete ملك الحقل النصي المركَّز: لا يُبتلع كاختصار (RULE-N47) — وإلا تعذّر حذف حرف بالكيبورد.
                    if (key == Keys.Delete && IsTextInputFocused(form)) return;

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
