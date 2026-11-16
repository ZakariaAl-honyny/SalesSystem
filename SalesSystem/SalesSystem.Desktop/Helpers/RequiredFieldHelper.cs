using System.Globalization;
using System.Runtime.CompilerServices;
using SalesSystem.Desktop.GlobalClasses;

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
/// نفس سلوك تحقق كلمة المرور في <c>frmLogin</c> حرفياً — عند مغادرة حقل مطلوب فارغ
/// (أو عند محاولة الحفظ) تظهر **أيقونة ✗** في <see cref="ErrorProvider"/> برسالتها العربية،
/// **و**ومضة رفض قصيرة + تلميح + صوت تنبيه من <see cref="clsInputFeedback.ShowRejectTooltip"/>
/// تعود إلى لون المصمّم تلقائياً. **لا تُصبغ خلفية أي حقل هنا ولا تُترك ملوّنة**: هذا الـHelper
/// لا يكتب خلفية أي حقل ولا يقرأ أي لون — اللون ملك <c>clsInputFeedback</c> وحده.
/// وتزول العلامة في اللحظة التي تصبح القيمة صحيحة.
/// <para>
/// <b>الشاشة تُعلن القواعد فقط</b> — لا أيقونة ولا لون ولا استعادة خلفية داخل أي <c>frm*.cs</c>.
/// </para>
/// <para>
/// <b>علامة واحدة في النموذج:</b> الحقل الذي يُوسَم يُفرّغ علامة سابقه (PILOT-8)، ويُبقى تظليل
/// الحقل النشط (الأصفر) ملكاً لـ<see cref="KeyboardHelper.WireFocusHighlight"/> فلا تصادمه هذه التغذية.
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

    /// <summary>حالة قاعدة واحدة: القاعدة + هل الأيقونة ظاهرة الآن.</summary>
    private sealed class RuleState
    {
        /// <summary>القاعدة المعلنة من الشاشة.</summary>
        public RequiredField Rule { get; init; }

        /// <summary>هل أيقونة الخطأ ظاهرة الآن؟</summary>
        public bool Marked;
    }

    /// <summary>
    /// يوصّل قواعد الحقول المطلوبة لنموذج واحد: يستمع <c>Leave</c> (التقييم عند المغادرة بـ Enter/Tab/الفأرة)
    /// و<c>TextChanged</c>/<c>SelectedIndexChanged</c> (التحديث الحيّ عند الكتابة/الاختيار)
    /// و<c>EnabledChanged</c> (التعطيل يرفع العلامة — مظهر التعطيل يملكه <c>WireDisabledVisuals</c>).
    /// <para>
    /// <b>ترتيب الاستدعاء إلزامي (RULE-N183):</b> يجب أن يأتي هذا الاستدعاء <b>بعد</b>
    /// <see cref="KeyboardHelper.WireFocusHighlight"/> — لأن WinForms ينفّذ معالجات الحدث نفسه بترتيب
    /// الاشتراك، وتظليل الحقل النشط يستعيد الخلفية الأصلية في <c>Leave</c> — و<b>قبل</b>
    /// <see cref="GreenThemeHelper.Apply"/> (وبالتالي قبل <see cref="KeyboardHelper.WireDisabledVisuals"/>)
    /// حتى لا يلتقط مظهر التعطيل لوناً مؤقتاً كلون طبيعي.
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
                };

                state.Rules.Add(ruleState);

                // المغادرة: التقييم — أيقونة ✗ + ومضة رفض، أو إزالة العلامة.
                field.Leave += (_, _) => Safe(() => Evaluate(ruleState, state));

                // التفعيل/التعطيل: الحقل المعطّل لا يحمل علامة (RULE-N154 يملك مظهر التعطيل).
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
    /// يقيّم كل قواعد النموذج الآن: **أيقونة ✗ على كل ناقص**، و**ومضة/صوت واحد فقط** لأول ناقص
    /// (لا ثلاث نغمات عند الحفظ)، ويُعيد <c>true</c> إذا كان كل المطلوب مكتملاً — أو إذا لم يكن
    /// النموذج موصولاً أصلاً.
    /// </summary>
    /// <param name="form">النموذج.</param>
    public static bool Validate(Form form)
    {
        if (form is null || form.IsDisposed) return true;
        if (!Registry.TryGetValue(form, out var state)) return true;

        bool isValid = true;
        bool flashed = false;   // ومضة واحدة لكل محاولة حفظ

        foreach (var ruleState in state.Rules)
        {
            try
            {
                // العلامة تُوسَم على **كل** ناقص (collapseOthers: false) — والومضة لأوله فقط.
                if (!Evaluate(ruleState, state, collapseOthers: false, flash: !flashed))
                {
                    isValid = false;
                    flashed = true;
                }
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
    /// هل يمنع هذا الحقل انتقال <c>Enter</c> الآن لأنه **حقل مطلوب فارغ**؟
    /// <para>
    /// <b>هذا هو البيت الواحد للحكم (RULE-N194 / RULE-N169…RULE-N172):</b> «الحقل المطلوب الفارغ لا
    /// يُغادَر بـ<c>Enter</c>» — ويُستهلك <b>فقط</b> من
    /// <see cref="KeyboardHelper.WireEnterAsTab(Control, Func{Button?})"/> (فرع <c>Enter</c> ذي الوسيطين)
    /// قبل أن ينقل التركيز. <b>ولا شاشة تستدعيه:</b> الشاشة تُعلن قواعدها في <see cref="Attach"/> فقط،
    /// فلا تتعلّم أي شاشة قاعدة جديدة ولا تحمل نسخة ثانية من حكم «فارغ» (RULE-N172).
    /// </para>
    /// <para>
    /// <b>لماذا قبل النقل؟</b> لأن نقل التركيز يُطلق <c>Leave</c> **بعد** أن يكون المؤشر قد غادر الحقل،
    /// فكانت الأيقونة والرسالة والومضة تظهر على الحقل السابق وقد قفز المؤشر إلى التالي. هنا يُسأل الحكم
    /// **أولاً**، فإن كان الحقل ناقصاً بقي التركيز عليه وظهرت العلامة في مكانها.
    /// </para>
    /// <para>
    /// <b>التقييم هو نفسه تقييم <c>Leave</c> حرفياً</b> عبر <see cref="Evaluate"/> بالافتراضيات
    /// (<c>collapseOthers: true</c> · <c>flash: true</c>): قاعدة غير سارية أو حقل معطّل ⇒ لا حجب ولا علامة ·
    /// فارغ ⇒ أيقونة ✗ + الرسالة + ومضة **وحجب** · ممتلئ ⇒ إزالة العلامة ولا حجب.
    /// </para>
    /// <para>
    /// <b>لا لون هنا:</b> الومضة واستعادة لون المصمّم ملك <c>clsInputFeedback</c>، وهذا الـHelper
    /// لا يكتب خلفية أي حقل أبداً (RULE-N180…RULE-N182). وأي خطأ ⇒ <c>false</c> (فشل مفتوح):
    /// قاعدة معطوبة لا تُجمّد التنقل.
    /// </para>
    /// </summary>
    /// <param name="field">الحقل الذي يُضغط فيه <c>Enter</c> (قد يكون ابناً داخلياً لعنصر مركّب).</param>
    /// <returns><c>true</c> = امنع الانتقال وأبقِ التركيز (مع وسم الرفض) · <c>false</c> = انتقل عادياً.</returns>
    public static bool BlocksEnterNavigation(Control? field)
    {
        if (field is null || field.IsDisposed) return false;

        try
        {
            var form = field.FindForm();
            if (form is null || form.IsDisposed) return false;
            if (!Registry.TryGetValue(form, out var state)) return false;

            // القاعدة المعلنة تُطابَق بالحقل نفسه ثم بأسلافه: العنصر المركّب يُعلن الشاشة أصله
            // بينما التركيز قد يكون على ابنه الداخلي (NumericUpDown ⇒ _upDownEdit).
            for (Control? candidate = field; candidate is not null; candidate = candidate.Parent)
            {
                foreach (var ruleState in state.Rules)
                {
                    if (!ReferenceEquals(ruleState.Rule.Field, candidate)) continue;

                    // نفس تقييم Leave بالافتراضيات — لا حكم ثانٍ على «فارغ» (RULE-N172).
                    return !Evaluate(ruleState, state);
                }
            }

            return false;   // ليس حقل «مطلوب» معلناً في هذه الشاشة ⇒ لا حجب.
        }
        catch
        {
            return false;   // فشل مفتوح — قاعدة معطوبة لا تُجمّد التنقل.
        }
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
    /// قاعدة غير سارية أو حقل معطّل ⇒ لا علامة أبداً · فارغ ⇒ أيقونة ✗ + ومضة رفض · ممتلئ ⇒ إزالة العلامة.
    /// <para>
    /// <b>علامة واحدة في النموذج (PILOT-8):</b> ما إن تُوسَم قاعدة حتى تُفرَّغ كل علامة سابقة
    /// (<paramref name="collapseOthers"/> = <c>true</c>) — فمع Enter ينتقل رفض الحقل الذي غادرته
    /// للتوّ. أمّا <see cref="Validate"/> فتمرّر <c>false</c> لأن محاولة الحفظ فعل صريح
    /// يُوسم **كل** ناقص ثم ينقل المؤشر إلى أوله.
    /// </para>
    /// <para>
    /// <b>لا لون هنا:</b> الومضة والتلميح والصوت من <see cref="clsInputFeedback.ShowRejectTooltip"/>
    /// (نفس سلوك <c>frmLogin</c>) وتُعيد لون المصمّم تلقائياً — والـHelper لا يكتب خلفية أي حقل أبداً.
    /// </para>
    /// </summary>
    /// <param name="ruleState">حالة القاعدة.</param>
    /// <param name="state">حالة النموذج (قائمة القواعد + مزوّد الأيقونة).</param>
    /// <param name="collapseOthers">إبقاء علامة واحدة فقط: تُفرَّغ كل قاعدة موسومة أخرى قبل الوسم.</param>
    /// <param name="flash">إطلاق ومضة/تلميح/صوت الرفض الآن (تُلغى لباقي الحقول عند الحفظ — نغمة واحدة).</param>
    private static bool Evaluate(RuleState ruleState, FormState state, bool collapseOthers = true, bool flash = true)
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

        // الوسم على وشك الحدوث ⇒ نُبقي علامة واحدة: السابقة تُفرَّغ أيقونتها (PILOT-8).
        if (collapseOthers) UnmarkAllExcept(state, ruleState);

        TrySetError(state.Provider, field, ruleState.Rule.Message);
        if (flash) TryFlashReject(field, ruleState.Rule.Message);
        ruleState.Marked = true;

        return false;
    }

    /// <summary>يُبقي علامة واحدة: كل قاعدة موسومة أخرى تُفرَّغ أيقونتها (RULE-N182 / PILOT-8).</summary>
    private static void UnmarkAllExcept(FormState state, RuleState keep)
    {
        foreach (var ruleState in state.Rules)
        {
            if (ReferenceEquals(ruleState, keep) || !ruleState.Marked) continue;

            try
            {
                Unmark(ruleState, state);
            }
            catch
            {
                // تجاهل — علامة أخرى لا تُسقط شاشة.
            }
        }
    }

    /// <summary>
    /// ومضة الرفض + التلميح + صوت التنبيه من البيت الواحد <see cref="clsInputFeedback"/>
    /// (نفس سلوك تحقق كلمة المرور في <c>frmLogin</c>): تُستدعى مرة واحدة عند وسم الحقل،
    /// وتُعيد لون المصمّم بعد <c>200</c> مللي ثانية بنفسها. لا ترمي أبداً.
    /// </summary>
    private static void TryFlashReject(Control field, string message)
    {
        try
        {
            clsInputFeedback.ShowRejectTooltip(field, message);
        }
        catch
        {
            // تجاهل — التغذية البصرية لا تُسقط شاشة.
        }
    }

    /// <summary>
    /// تغيّرت قيمة الحقل: إن كانت الأيقونة ظاهرة والقيمة صحيحة الآن تُزال فوراً.
    /// لا لون هنا إطلاقاً — الومضة تعيد لون المصمّم بنفسها.
    /// </summary>
    private static void OnValueChanged(RuleState ruleState, FormState state)
    {
        if (!ruleState.Marked) return;   // لا علامة أثناء الكتابة قبل المغادرة.

        var field = ruleState.Rule.Field;
        if (field is null || field.IsDisposed) return;

        if (!IsRuleActive(ruleState.Rule) || !IsEffectivelyEnabled(field) || !IsEmpty(field))
            Unmark(ruleState, state);
    }

    /// <summary>
    /// تغيّرت فعالية الحقل: التعطيل يرفع الأيقونة (مظهر التعطيل يملكه
    /// <see cref="KeyboardHelper.WireDisabledVisuals"/> — RULE-N154)، والتفعيل لا يُوسم تلقائياً
    /// (العلامة تظهر عند مغادرة حقل مطلوب فارغ أو عند محاولة الحفظ).
    /// </summary>
    private static void OnEnabledChanged(RuleState ruleState, FormState state)
    {
        var field = ruleState.Rule.Field;
        if (field is null || field.IsDisposed) return;

        Unmark(ruleState, state);
    }

    /// <summary>يزيل أيقونة الخطأ فقط — لا لون هنا: ومضة الرفض تُعيد لون المصمّم بنفسها.</summary>
    private static void Unmark(RuleState ruleState, FormState state)
    {
        var field = ruleState.Rule.Field;
        if (field is null || field.IsDisposed) return;

        TrySetError(state.Provider, field, "");
        ruleState.Marked = false;
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
