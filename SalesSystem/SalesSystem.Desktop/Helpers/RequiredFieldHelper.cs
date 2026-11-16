using System.Globalization;
using System.Runtime.CompilerServices;

namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// قاعدة حقل مطلوب: الحقل + رسالة الرفض العربية + شرط التفعيل (null = مطلوب دائماً).
/// </summary>
/// <param name="Field">الحقل المطلوب (TextBox / MaskedTextBox / ComboBox).</param>
/// <param name="Message">رسالة الرفض العربية التي تظهر في أيقونة الخطأ وتحتها.</param>
/// <param name="When">شرط التفعيل — <c>null</c> يعني «مطلوب دائماً».</param>
public readonly record struct RequiredField(Control Field, string Message, Func<bool>? When = null);

/// <summary>
/// البيت الواحد لتغذية الحقول المطلوبة (RULE-N169…RULE-N172 / RULE-N180…N183):
/// عندما يترك المستخدم حقلاً مطلوباً فارغاً (أو يضغط الزر الأساسي) يظهر رمز ✗ في
/// <see cref="ErrorProvider"/> مع رسالته العربية، وتُصبغ خلفية الحقل بلون التحديد الحيّ،
/// وتزول العلامة في اللحظة التي تصبح القيمة صحيحة.
/// <para>
/// <b>الشاشة تُعلن القواعد فقط</b> — لا لون ولا نداء <see cref="ErrorProvider.SetError(Control, string)"/>
/// ولا استعادة خلفية داخل أي <c>frm*.cs</c>.
/// </para>
/// <para>
/// <b>اللون حيّ (RULE-N54a/N54b):</b> يُقرأ عند كل تقييم من
/// <see cref="ThemePalette.Get(string)"/> على مفتاح <see cref="ThemePalette.SelectionRowColor"/> —
/// لا قيمة ARGB مكتوبة هنا ولا نسخة مخزَّنة.
/// </para>
/// </summary>
public static class RequiredFieldHelper
{
    /// <summary>
    /// سجل قواعد كل نموذج. الاختيار صريح: <see cref="ConditionalWeakTable{TKey, TValue}"/> وليس
    /// <c>Dictionary&lt;Form, …&gt;</c> — لأنه لا يُبقي مرجعاً قوياً إلى النماذج التي أُغلقت
    /// (الجدول الثابت كان سيُسرّب كل نموذج يُفتح ويُغلق)، بنفس نمط سجل التوصيل في
    /// <see cref="KeyboardHelper"/> (<c>MarkWired</c> / <c>FocusHighlightWired</c>).
    /// </summary>
    private static readonly ConditionalWeakTable<Form, FormState> Registry = new();

    /// <summary>مزوّد أيقونة الخطأ المُعلَن في المصمّم + قواعد النموذج (RULE-N145: لا إنشاء وقت التشغيل).</summary>
    private sealed class FormState
    {
        /// <summary>مزوّد الخطأ الذي تتبنّاه الشاشة من مصمّمها.</summary>
        public ErrorProvider? Provider { get; set; }

        /// <summary>قواعد الحقول المطلوبة بترتيب إعلانها في الشاشة.</summary>
        public List<RuleState> Rules { get; } = new();
    }

    /// <summary>حالة قاعدة واحدة: القاعدة + لون الخلفية الأصلي الملتقَط لحظة التوصيل + هل العلامة ظاهرة.</summary>
    private sealed class RuleState
    {
        /// <summary>القاعدة المعلنة من الشاشة.</summary>
        public RequiredField Rule { get; init; }

        /// <summary>خلفية الحقل الأصلية (لون المصمّم) — لا تُستعاد إلا هي (RULE-N139/RULE-N142).</summary>
        public Color OriginalBack { get; init; }

        /// <summary>هل الأيقونة/الخلفية الحمراء ظاهرة الآن؟</summary>
        public bool Marked;
    }

    /// <summary>
    /// يوصّل قواعد الحقول المطلوبة لنموذج واحد: يلتقط خلفية كل حقل الحالية كأصل، ثم يستمع
    /// <c>Leave</c> (التقييم عند المغادرة بـ Enter/Tab/الفأرة) و<c>Enter</c> (صبغ الخلفية عند الدخول)
    /// و<c>TextChanged</c>/<c>SelectedIndexChanged</c> (التحديث الحيّ عند الكتابة/الاختيار)
    /// و<c>EnabledChanged</c> (التعطيل يرفع العلامة ويُعيد اللون الأصلي قبل رمادي التعطيل).
    /// <para>
    /// <b>ترتيب الاستدعاء إلزامي (RULE-N183):</b> يجب أن يأتي هذا الاستدعاء <b>بعد</b>
    /// <see cref="KeyboardHelper.WireFocusHighlight"/> — لأن WinForms ينفّذ معالجات الحدث نفسه بترتيب
    /// الاشتراك، وتظليل الحقل النشط يستعيد الخلفية الأصلية في <c>Leave</c> أيضاً، فلو سبقناها لمحا تلوين الرفض —
    /// و<b>قبل</b> <see cref="GreenThemeHelper.Apply"/> (وبالتالي قبل <see cref="KeyboardHelper.WireDisabledVisuals"/>):
    /// لو اشترك مظهر التعطيل أولاً لالتقط لون الرفض كـ«لون طبيعي» ثم محا معالج هذا الـHelper
    /// رمادي التعطيل عن حقل **معطّل** حتى يُعاد تفعيله (`RULE-N154`). لا تُعِد ترتيب هذه الأسطر.
    /// </para>
    /// <para>آمن للاستدعاء المتكرر على النموذج نفسه: الحقل الموصول سابقاً لا يُوصَّل مرتين.</para>
    /// </summary>
    /// <param name="form">النموذج (يُتجاهل الاستدعاء بلا استثناء إن كان <c>null</c> أو مُغلَقاً).</param>
    /// <param name="provider">مزوّد أيقونة الخطأ المُعلَن في <c>*.Designer.cs</c>.</param>
    /// <param name="rules">قواعد الحقول المطلوبة — تُعلنها الشاشة فقط.</param>
    public static void Attach(Form form, ErrorProvider provider, params RequiredField[] rules)
    {
        if (form is null || form.IsDisposed || provider is null || rules is null || rules.Length == 0) return;

        try
        {
            var state = Registry.GetValue(form, _ => new FormState());
            state.Provider = provider;

            foreach (var rule in rules)
            {
                var field = rule.Field;

                if (field is null || field.IsDisposed) continue;
                if (IsAlreadyAttached(state, field)) continue;

                var ruleState = new RuleState
                {
                    Rule = rule,
                    OriginalBack = field.BackColor,
                };

                state.Rules.Add(ruleState);

                // المغادرة: التقييم — يُظهر العلامة أو يزيلها.
                field.Leave += (_, _) => Safe(() => Evaluate(ruleState, state));

                // الدخول: صبغ خلفية الرفض بنفس لون الحقل النشط (RULE-N46) بلا صراع بصري.
                field.Enter += (_, _) => Safe(() => OnEnter(ruleState, state));

                // التفعيل/التعطيل: الحقل المعطّل لا يحمل علامة، واللون الأصلي يُستعاد **قبل** أن يلتقط
                // WireDisabledVisuals الخلفية كلون طبيعي (RULE-N154). وهذا **التزام على الشاشة** لا ضمانة
                // من الـHelper: Attach يجب أن يُستدعى بعد WireFocusHighlight **وقبل** GreenThemeHelper.Apply /
                // WireDisabledVisuals (RULE-N183)؛ فإن اشترك مظهر التعطيل أولاً التقط لون الرفض كـ«لون طبيعي»
                // ثم محا هذا المعالج رمادي التعطيل عن حقل **معطّل** حتى يُعاد تفعيله.
                field.EnabledChanged += (_, _) => Safe(() => OnEnabledChanged(ruleState, state));

                // التحديث الحيّ: الكتابة/الاختيار يزيلان العلامة فور صحة القيمة.
                switch (field)
                {
                    case TextBoxBase:
                        field.TextChanged += (_, _) => Safe(() => OnValueChanged(ruleState, state));
                        break;

                    case ComboBox combo:
                        combo.SelectedIndexChanged += (_, _) => Safe(() => OnValueChanged(ruleState, state));
                        break;
                }
            }
        }
        catch
        {
            // تجاهل — قاعدة عرضية لا تُسقط شاشة.
        }
    }

    /// <summary>
    /// يقيّم كل قواعد النموذج الآن (فتظهر الأيقونة **وخلفية لون التحديد** معاً، لا الأيقونة وحدها)
    /// ويُعيد <c>true</c> إذا كان كل المطلوب مكتملاً — أو إذا لم يكن النموذج موصولاً أصلاً.
    /// </summary>
    /// <param name="form">النموذج.</param>
    public static bool Validate(Form form)
    {
        if (form is null || form.IsDisposed) return true;
        if (!Registry.TryGetValue(form, out var state)) return true;

        bool isValid = true;

        foreach (var ruleState in state.Rules)
        {
            try
            {
                if (!Evaluate(ruleState, state)) isValid = false;
            }
            catch
            {
                // تجاهل — قاعدة واحدة لا تُسقط التحقق كله.
            }
        }

        return isValid;
    }

    /// <summary>
    /// أول حقل مطلوب ناقص الآن (بعد تقييم <see cref="Validate"/> الذي يُوسمه)، أو <c>null</c> إن كان كل المطلوب مكتملاً.
    /// يُستهلك لنقل المؤشر إلى أول ناقص بدل تكرار حكم «فارغ» داخل الشاشة (RULE-N172/RULE-N184).
    /// </summary>
    /// <param name="form">النموذج (يُعيد <c>null</c> بلا استثناء إن كان <c>null</c> أو مُغلَقاً أو غير موصول).</param>
    public static Control? FirstMissing(Form form)
    {
        if (form is null || form.IsDisposed) return null;
        if (!Registry.TryGetValue(form, out var state)) return null;

        try
        {
            // ترتيب إعلان القواعد في الشاشة هو ترتيب البحث — والحكم هنا وحده (RULE-N172).
            foreach (var ruleState in state.Rules)
            {
                var field = ruleState.Rule.Field;

                if (field is null || field.IsDisposed) continue;
                if (!IsRuleActive(ruleState.Rule) || !IsEffectivelyEnabled(field)) continue;
                if (IsEmpty(field)) return field;
            }
        }
        catch
        {
            // تجاهل — نقل المؤشر لا يُسقط شاشة.
        }

        return null;
    }

    /// <summary>
    /// يمسح أيقونة الخطأ ويُعيد خلفية كل حقل إلى لونها الأصلي الملتقَط (لا إلى لون حرفي).
    /// الحقل المعطّل لا تُكتب خلفيته — مظهر التعطيل يملكه <see cref="KeyboardHelper.WireDisabledVisuals"/> (RULE-N154).
    /// </summary>
    /// <param name="form">النموذج (يُتجاهل الاستدعاء بلا استثناء إن كان <c>null</c> أو مُغلَقاً أو غير موصول).</param>
    public static void Clear(Form form)
    {
        if (form is null || form.IsDisposed) return;
        if (!Registry.TryGetValue(form, out var state)) return;

        foreach (var ruleState in state.Rules)
        {
            try
            {
                Unmark(ruleState, state);
            }
            catch
            {
                // تجاهل — علامة لا تُسقط شاشة.
            }
        }
    }

    // ── التقييم ───────────────────────────────────────────────────────────────

    /// <summary>
    /// يقيّم قاعدة واحدة ويُعيد هل هي مكتملة الآن:
    /// قاعدة غير سارية أو حقل معطّل ⇒ لا علامة أبداً · فارغ ⇒ أيقونة + لون التحديد · ممتلئ ⇒ إزالة العلامة.
    /// </summary>
    private static bool Evaluate(RuleState ruleState, FormState state)
    {
        var field = ruleState.Rule.Field;
        if (field is null || field.IsDisposed) return true;

        // قاعدة غير سارية الآن أو حقل معطّل ⇒ لا تُوسَم أبداً (RULE-N154 يملك مظهر التعطيل).
        if (!IsRuleActive(ruleState.Rule) || !IsEffectivelyEnabled(field))
        {
            Unmark(ruleState, state);
            return true;
        }

        if (!IsEmpty(field))
        {
            Unmark(ruleState, state);
            return true;
        }

        TrySetError(state.Provider, field, ruleState.Rule.Message);
        field.BackColor = ThemePalette.Get(ThemePalette.SelectionRowColor);   // لون حيّ في كل تقييم (RULE-N54b)
        ruleState.Marked = true;

        return false;
    }

    /// <summary>الدخول إلى حقل مطلوب فارغ: صبغ خلفيته بلون التحديد نفسه (RULE-N46) — بلا علامة جديدة.</summary>
    private static void OnEnter(RuleState ruleState, FormState state)
    {
        var field = ruleState.Rule.Field;
        if (field is null || field.IsDisposed) return;
        if (!IsRuleActive(ruleState.Rule) || !IsEffectivelyEnabled(field)) return;
        if (!IsEmpty(field)) return;

        field.BackColor = ThemePalette.Get(ThemePalette.SelectionRowColor);
    }

    /// <summary>
    /// تغيّرت قيمة الحقل: إن كانت العلامة ظاهرة والقيمة صحيحة الآن تُزال فوراً.
    /// المركَّز يُنظَّف من الأيقونة فقط وتُترك خلفيته — تظليل الحقل النشط يملكها (RULE-N46).
    /// </summary>
    private static void OnValueChanged(RuleState ruleState, FormState state)
    {
        if (!ruleState.Marked) return;   // لا تلوين أثناء الكتابة قبل المغادرة.

        var field = ruleState.Rule.Field;
        if (field is null || field.IsDisposed) return;

        if (!IsRuleActive(ruleState.Rule) || !IsEffectivelyEnabled(field) || !IsEmpty(field))
        {
            TrySetError(state.Provider, field, "");
            ruleState.Marked = false;

            if (!field.Focused) RestoreOriginal(ruleState);
        }
    }

    /// <summary>
    /// تغيّرت فعالية الحقل: عند التعطيل تُفرَّغ العلامة وتُستعاد الخلفية الأصلية **قبل** أن يلتقط
    /// مظهر التعطيل الخلفية كلون طبيعي (RULE-N154)؛ وعند التفعيل تُفرَّغ العلامة فقط —
    /// لا يُوسَم حقل تلقائياً بمجرد تفعيله (العلامة تظهر عند مغادرة حقل مطلوب فارغ أو عند محاولة الحفظ).
    /// </summary>
    private static void OnEnabledChanged(RuleState ruleState, FormState state)
    {
        var field = ruleState.Rule.Field;
        if (field is null || field.IsDisposed) return;

        if (!field.Enabled)
        {
            TrySetError(state.Provider, field, "");
            ruleState.Marked = false;
            RestoreOriginal(ruleState, force: true);
            return;
        }

        Unmark(ruleState, state);
    }

    /// <summary>يزيل العلامة ويعيد الخلفية الأصلية (إن كان الحقل مفعّلاً).</summary>
    private static void Unmark(RuleState ruleState, FormState state)
    {
        var field = ruleState.Rule.Field;
        if (field is null || field.IsDisposed) return;

        TrySetError(state.Provider, field, "");
        RestoreOriginal(ruleState);
        ruleState.Marked = false;
    }

    /// <summary>
    /// يُعيد خلفية الحقل إلى اللون الأصلي **الملتقَط لحظة التوصيل** — لا إلى أبيض ولا إلى لون حرفي
    /// (RULE-N139/RULE-N142). الحقل المعطّل لا تُكتب خلفيته إطلاقاً (RULE-N154) إلا عند
    /// <paramref name="force"/> = <c>true</c> أي في لحظة التعطيل نفسها (قبل رمادي التعطيل).
    /// </summary>
    /// <param name="ruleState">حالة القاعدة.</param>
    /// <param name="force">إعادة الخلفية حتى لو كان الحقل معطّلاً (لحظة التعطيل فقط).</param>
    private static void RestoreOriginal(RuleState ruleState, bool force = false)
    {
        var field = ruleState.Rule.Field;
        if (field is null || field.IsDisposed) return;
        if (!force && !IsEffectivelyEnabled(field)) return;

        try
        {
            field.BackColor = ruleState.OriginalBack;
        }
        catch
        {
            // تجاهل — لون لا يُسقط شاشة.
        }
    }

    // ── الحكم على «فارغ» — مرة واحدة للنظام كله ───────────────────────────────

    /// <summary>
    /// هل الحقل فارغ؟ المصدر الوحيد لهذا الحكم في النظام:
    /// <list type="bullet">
    /// <item><c>TextBoxBase</c> (ويشمل <c>MaskedTextBox</c>): نص فارغ أو مسافات فقط.</item>
    /// <item><c>ComboBox</c>: لا اختيار · قيمة <c>null</c>/<c>DBNull</c> · أو صفر رقمي (سطر العنصر النائب).</item>
    /// <item>أي عنصر آخر (<c>NumericUpDown</c> وغير المذكور): غير مدعوم ⇒ لا يُوسَم أبداً.</item>
    /// </list>
    /// </summary>
    private static bool IsEmpty(Control field) => field switch
    {
        MaskedTextBox masked => string.IsNullOrWhiteSpace(masked.Text),
        TextBoxBase text => string.IsNullOrWhiteSpace(text.Text),
        ComboBox combo => IsComboEmpty(combo),
        _ => false,
    };

    /// <summary>
    /// قائمة منسدلة فارغة؟ لا اختيار، أو قيمة غير رقمية سالبة الدلالة، أو صفر رقمي (سطر نائب).
    /// التحويل **دفاعي** على أنواع القيمة الحقيقية (<c>byte</c>/<c>short</c>/<c>int</c>/<c>long</c>
    /// /<c>decimal</c>) — ممنوع <c>(int)combo.SelectedValue</c> لأنه يرمي <c>InvalidCastException</c>
    /// حين يكون المعرّف <c>short</c>؛ والنص غير الرقمي (بلا <c>ValueMember</c>) يُعدّ قيمة حقيقية.
    /// </summary>
    private static bool IsComboEmpty(ComboBox combo)
    {
        if (combo.SelectedIndex < 0) return true;

        object? value = combo.SelectedValue;

        return value is null || value is DBNull || IsZeroNumber(value);
    }

    /// <summary>هل القيمة المختارة صفر رقمي (= سطر العنصر النائب)؟ بلا أي تحويل صريح قد يرمي.</summary>
    private static bool IsZeroNumber(object value) => value switch
    {
        string text => decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out var parsed) && parsed == 0m,
        byte byteValue => byteValue == 0,
        short shortValue => shortValue == 0,
        int intValue => intValue == 0,
        long longValue => longValue == 0,
        decimal decimalValue => decimalValue == 0m,
        float floatValue => floatValue == 0f,
        double doubleValue => doubleValue == 0d,
        _ => false,
    };

    // ── أدوات صغيرة ───────────────────────────────────────────────────────────

    /// <summary>هل القاعدة سارية الآن؟ (لا شرط ⇒ مطلوبة دائماً).</summary>
    private static bool IsRuleActive(RequiredField rule)
    {
        if (rule.When is null) return true;

        try
        {
            return rule.When();
        }
        catch
        {
            return true;   // شرط معطوب ⇒ نُبقي المطلوب مطلوباً (الأكثر أماناً للبيانات)
        }
    }

    /// <summary>هل الحقل موصول بقاعدة سابقة في هذا النموذج؟ (منع التوصيل المزدوج).</summary>
    private static bool IsAlreadyAttached(FormState state, Control field)
    {
        foreach (var ruleState in state.Rules)
        {
            if (ReferenceEquals(ruleState.Rule.Field, field)) return true;
        }

        return false;
    }

    /// <summary>ضبط أيقونة الخطأ بحماية من مزوّد مُلغى أو غير جاهز (لا استثناء يفلت).</summary>
    private static void TrySetError(ErrorProvider? provider, Control field, string message)
    {
        if (provider is null || field is null || field.IsDisposed) return;

        try
        {
            provider.SetError(field, message);
        }
        catch
        {
            // تجاهل — الأيقونة ليست سبباً لإسقاط شاشة.
        }
    }

    /// <summary>
    /// هل العنصر مفعّل فعلياً؟ (تعطيل الحاوية يمنع أبناءها دون تغيير <c>Enabled</c> المحلي — نفس قياس
    /// <see cref="KeyboardHelper"/>). المفهوم مكرَّر هنا بحدّه الأدنى لأن دالة الـHelper هناك خاصة.
    /// </summary>
    private static bool IsEffectivelyEnabled(Control field)
    {
        for (Control? node = field; node is not null; node = node.Parent)
        {
            if (!node.Enabled) return false;
        }

        return true;
    }

    /// <summary>يُنفّذ معالج الحدث بلا أن يفلت أي استثناء إلى الشاشة.</summary>
    private static void Safe(Action action)
    {
        try
        {
            action();
        }
        catch
        {
            // تجاهل — معالج عرضي يعمل مع كل ضغطة يجب ألا يُسقط النموذج.
        }
    }
}
