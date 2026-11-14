using System;
using System.Collections.Generic;
using System.Globalization;

namespace SalesSystem.Desktop.GlobalClasses
{
    /// <summary>نوع إعداد التنسيق: منازل عشرية · مفتاح نعم/لا · اختيار من قيم محددة · صيغة تاريخ.</summary>
    public enum FormatSettingKind
    {
        Decimals = 0,
        Toggle = 1,
        Choice = 2,
        Pattern = 3
    }

    /// <summary>
    /// تعريف إعداد تنسيق واحد — تُقرأ هذه التعريفات من <c>clsFormat.Definitions</c> فقط،
    /// وتستهلكها شاشة <c>frmFormatingSetting</c> والخدمة الوهمية بلا أي نسخة ثانية من المفاتيح أو النصوص.
    /// </summary>
    public sealed record FormatSettingDefinition(
        string Key,
        string ArabicLabel,
        string GroupLabel,
        FormatSettingKind Kind,
        string DefaultValue,
        int Min = 0,
        int Max = 0,
        IReadOnlyList<string>? AllowedValues = null);

    /// <summary>خيار معروض في قائمة اختيار: القيمة المخزَّنة والنص العربي الذي يراه المستخدم.</summary>
    public sealed record FormatOption(string Value, string DisplayText)
    {
        public override string ToString() => DisplayText;
    }

    /// <summary>
    /// تنسيق وقراءة التواريخ والأرقام والمبالغ — البيت الواحد لكل تنسيق يُعرض في الشاشات
    /// (<c>RULE-N169</c>…<c>RULE-N172</c>): التعريفات والقيم الحيّة والتحقق والتنسيق والقراءة كلها هنا،
    /// ولا يُكرَّر أي مفتاح أو افتراضي أو رسالة رفض في أي ملف آخر.
    /// </summary>
    public static class clsFormat
    {
        // ── المفاتيح (مصدر الحقيقة الوحيد — لا تُكتب في أي شاشة أو خدمة) ──
        public const string MoneyDecimals = "moneyDecimals";
        public const string QuantityDecimals = "quantityDecimals";
        public const string NumberDecimals = "numberDecimals";
        public const string PercentDecimals = "percentDecimals";
        public const string ThousandsSeparator = "thousandsSeparator";
        public const string DecimalSeparator = "decimalSeparator";
        public const string ArabicIndicDigits = "arabicIndicDigits";
        public const string DateShortPattern = "dateShortPattern";
        public const string DateDayMonthYearPattern = "dateDayMonthYearPattern";
        public const string DateLongPattern = "dateLongPattern";

        // ── أسماء المجموعات ──
        public const string NumbersGroup = "المبالغ والأرقام";
        public const string DatesGroup = "التواريخ";

        // ── التمثيل القانوني لقيم نعم/لا (البيت الواحد — لا يُكتب حرفياً في أي شاشة) ──
        /// <summary>التمثيل القانوني لقيمة نعم — البيت الواحد (لا يُكتب حرفياً في أي شاشة).</summary>
        public const string ToggleTrue = "true";
        /// <summary>التمثيل القانوني لقيمة لا.</summary>
        public const string ToggleFalse = "false";

        private const string InvariantExamplePattern = "dd/MM/yyyy";
        private const int MaxPatternLength = 32;

        /// <summary>جدول التعريفات — بهذا الترتيب وهذه الافتراضيات حصراً (مصدر الحقيقة الوحيد للشاشة والخدمة).</summary>
        public static IReadOnlyList<FormatSettingDefinition> Definitions { get; } = BuildDefinitions();

        private static readonly Dictionary<string, FormatSettingDefinition> _definitionsByKey = BuildDefinitionsIndex();

        // المخزن الحيّ — يُستبدل بتبديل مرجعي واحد في <see cref="TryApplyAll"/> (RULE-N167: لا دفعة نصف مطبَّقة).
        private static Dictionary<string, string> _values = BuildInitialValues();

        // ───────────────────────────── التعريفات والقيم الحيّة ─────────────────────────────

        /// <summary>القيمة الحيّة للمفتاح، وإن كان مجهولاً يُعيد نصاً فارغاً.</summary>
        public static string Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return string.Empty;

            return _values.TryGetValue(key, out var value) ? value : string.Empty;
        }

        /// <summary>
        /// خيارات مفتاح تنسيق واحد — تُبنى من التعريف وحده (لا نسخة ثانية من التسميات في أي شاشة ولا في أي خدمة).
        /// <para>
        /// <c>Decimals</c>: خيار لكل قيمة من <c>Min</c> إلى <c>Max</c> بنص وصفي («منزلتان»…).
        /// <c>Toggle</c>: «نعم» ثم «لا». <c>Choice</c>: خيار لكل قيمة مسموحة بنص ودّي.
        /// <c>Pattern</c>: خيار لكل صيغة مسموحة بنصها كما هي (المثال الحيّ يعرضه صندوق المعاينة).
        /// </para>
        /// <para>لا ترمي استثناءً أبداً: المفتاح المجهول أو الفارغ يُعيد قائمة فارغة.</para>
        /// </summary>
        public static IReadOnlyList<FormatOption> Options(string key)
        {
            var definition = _Find(key);
            if (definition is null) return Array.Empty<FormatOption>();

            try
            {
                if (definition.Kind == FormatSettingKind.Decimals)
                {
                    var decimals = new List<FormatOption>(Math.Max(definition.Max - definition.Min + 1, 0));

                    for (var count = definition.Min; count <= definition.Max; count++)
                    {
                        decimals.Add(new FormatOption(
                            count.ToString(CultureInfo.InvariantCulture),
                            $"{count} — {_DecimalsWord(count)}"));
                    }

                    return decimals;
                }

                if (definition.Kind == FormatSettingKind.Toggle)
                {
                    return new List<FormatOption>(2)
                    {
                        new FormatOption(ToggleTrue, "نعم"),
                        new FormatOption(ToggleFalse, "لا")
                    };
                }

                var allowed = definition.AllowedValues;
                if (allowed is null || allowed.Count == 0) return Array.Empty<FormatOption>();

                var options = new List<FormatOption>(allowed.Count);

                foreach (var value in allowed)
                {
                    options.Add(new FormatOption(
                        value,
                        definition.Kind == FormatSettingKind.Choice ? _ChoiceText(value) : value));
                }

                return options;
            }
            catch (Exception)
            {
                return Array.Empty<FormatOption>();
            }
        }

        /// <summary>نسخة مستقلة من كل القيم الحيّة بمفاتيح التعريفات (آمنة للتعديل والفحص خارجياً).</summary>
        public static IReadOnlyDictionary<string, string> Snapshot()
        {
            var copy = new Dictionary<string, string>(Definitions.Count, StringComparer.Ordinal);

            foreach (var definition in Definitions)
                copy[definition.Key] = Get(definition.Key);

            return copy;
        }

        /// <summary>
        /// يتحقق من القيمة ثم يعيّنها (مع تطبيع قيم نعم/لا والمنازل) — يُعيد <c>false</c> عند الرفض.
        /// ويرفض كذلك تعييناً منفرداً لـ«فاصل الآلاف» أو «فاصل الكسر» يُنتج الحالة الملتبسة (فاصل آلاف مفعّل + «,» فاصلَ كسر)
        /// مقابل القيم الحيّة، بنفس قاعدة <see cref="ValidateAll"/> الوحيدة.
        /// <para>للمفتاح الواحد فقط؛ أما حفظ دفعة إعدادات فيمرّ عبر <see cref="TryApplyAll"/> حصراً.</para>
        /// </summary>
        public static bool TrySet(string key, string value)
        {
            if (Validate(key, value) is not null) return false;

            var definition = _Find(key);
            if (definition is null) return false;

            var normalized = NormalizeValue(definition, value);

            if (definition.Key is ThousandsSeparator or DecimalSeparator)
            {
                bool thousands = definition.Key == ThousandsSeparator
                    ? NormalizeToggle(normalized) == ToggleTrue
                    : _Toggle(ThousandsSeparator, null);

                string decimalSeparator = definition.Key == DecimalSeparator
                    ? normalized
                    : _Read(DecimalSeparator, null);

                if (_PairRuleError(thousands, decimalSeparator) is not null) return false;
            }

            _values[definition.Key] = normalized;
            return true;
        }

        /// <summary>
        /// يطبّق دفعة إعدادات **ذرّية**: يتحقق من الدفعة كاملة (<see cref="ValidateAll"/>) ثم يستبدل
        /// القيم الحيّة مرة واحدة؛ فلا يمكن أن ترفض الدفعة بسبب ترتيب المفاتيح، ولا يبقى تطبيق جزئي.
        /// يُعيد <c>null</c> عند النجاح، أو رسالة الرفض العربية نفسها التي يُعيدها <see cref="ValidateAll"/>.
        /// </summary>
        public static string? TryApplyAll(IReadOnlyDictionary<string, string> values)
        {
            if (values is null || values.Count == 0) return "لا توجد إعدادات تنسيق للحفظ.";

            // (1) التحقق بقاعدة البيت الواحد حصراً — لا نسخة ثانية لأي شرط أو رسالة
            var error = ValidateAll(values);
            if (error is not null) return error;

            // (2) نسخة مؤقتة مطبَّعة من كل مفاتيح التعريفات — القيم الحيّة لا تُلمس أثناء البناء
            var next = new Dictionary<string, string>(Definitions.Count, StringComparer.Ordinal);

            foreach (var definition in Definitions)
                next[definition.Key] = NormalizeValue(definition, values[definition.Key]);

            // (3) استبدال المخزن دفعة واحدة — لا قارئ يرى حالة نصف مطبَّقة
            _values = next;

            return null;
        }

        /// <summary>يُعيد كل القيم إلى افتراضيات جدول التعريفات.</summary>
        public static void ResetToDefaults()
        {
            foreach (var definition in Definitions)
                _values[definition.Key] = definition.DefaultValue;
        }

        /// <summary>تحقق قيمة واحدة — <c>null</c> = صالحة، وإلا رسالة عربية جاهزة للعرض.</summary>
        public static string? Validate(string key, string value)
        {
            var definition = _Find(key);
            if (definition is null) return $"مفتاح تنسيق غير معروف: {key}.";

            var text = (value ?? string.Empty).Trim();

            switch (definition.Kind)
            {
                case FormatSettingKind.Decimals:
                {
                    var isNumber = int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var count);
                    if (!isNumber || count < definition.Min || count > definition.Max)
                        return $"عدد المنازل العشرية لـ «{definition.ArabicLabel}» يجب أن يكون بين {definition.Min} و {definition.Max}.";

                    return null;
                }

                case FormatSettingKind.Toggle:
                    return NormalizeToggle(text) is null
                        ? $"قيمة «{definition.ArabicLabel}» يجب أن تكون نعم أو لا."
                        : null;

                case FormatSettingKind.Choice:
                    return _IsAllowed(definition, text)
                        ? null
                        : $"قيمة «{definition.ArabicLabel}» غير مسموحة. المسموح: {string.Join(" أو ", definition.AllowedValues ?? Array.Empty<string>())}.";

                case FormatSettingKind.Pattern:
                    return _IsValidPattern(text)
                        ? null
                        : $"صيغة «{definition.ArabicLabel}» غير صالحة — مثال صحيح: {InvariantExamplePattern}.";

                default:
                    return null;
            }
        }

        /// <summary>
        /// التحقق المجمّع الوحيد (تستهلكه الخدمة الوهمية وحدها): اكتمال المفاتيح · عدم وجود مفاتيح مجهولة ·
        /// تحقق كل قيمة · ثم القاعدة المتقاطعة. <c>null</c> = الدفعة صالحة بالكامل.
        /// </summary>
        public static string? ValidateAll(IReadOnlyDictionary<string, string> values)
        {
            values ??= new Dictionary<string, string>(StringComparer.Ordinal);

            foreach (var definition in Definitions)
            {
                if (!values.ContainsKey(definition.Key))
                    return $"الإعداد المطلوب غير موجود: {definition.ArabicLabel} ({definition.Key}).";
            }

            foreach (var pair in values)
            {
                if (!_definitionsByKey.ContainsKey(pair.Key))
                    return $"مفتاح تنسيق غير معروف: {pair.Key}.";
            }

            foreach (var definition in Definitions)
            {
                var error = Validate(definition.Key, values[definition.Key]);
                if (error is not null) return error;
            }

            return _PairRuleError(_Toggle(ThousandsSeparator, values), _Read(DecimalSeparator, values));
        }

        // ───────────────────────────── التنسيق ─────────────────────────────

        /// <summary>تاريخ قصير للعرض — الصيغة الحيّة أو <c>values</c> للمعاينة.</summary>
        public static string DateToShort(DateTime Dt1, IReadOnlyDictionary<string, string>? values = null)
            => _FormatDate(Dt1, DateShortPattern, values);

        /// <summary>تاريخ قصير بالصيغة الرقمية يوم/شهر/سنة — الصيغة الحيّة أو <c>values</c> للمعاينة.</summary>
        public static string DateToDayMonthYear(DateTime dt, IReadOnlyDictionary<string, string>? values = null)
            => _FormatDate(dt, DateDayMonthYearPattern, values);

        /// <summary>تاريخ ووقت للعرض — الصيغة الحيّة أو <c>values</c> للمعاينة.</summary>
        public static string DateToLong(DateTime dt, IReadOnlyDictionary<string, string>? values = null)
            => _FormatDate(dt, DateLongPattern, values);

        /// <summary>مبلغ مالي بمنازل المبالغ الحيّة (<c>decimal(18,2)</c> افتراضاً).</summary>
        public static string Money(decimal value, IReadOnlyDictionary<string, string>? values = null)
            => _FormatNumber(value, MoneyDecimals, string.Empty, values);

        /// <summary>كمية بمنازل الكميات الحيّة (<c>decimal(18,3)</c> افتراضاً).</summary>
        public static string Quantity(decimal value, IReadOnlyDictionary<string, string>? values = null)
            => _FormatNumber(value, QuantityDecimals, string.Empty, values);

        /// <summary>عدد بمنازل الأعداد الحيّة (عدد النتائج والإحصاءات — صفر منازل افتراضاً).</summary>
        public static string Number(decimal value, IReadOnlyDictionary<string, string>? values = null)
            => _FormatNumber(value, NumberDecimals, string.Empty, values);

        /// <summary>نسبة مئوية بمنازل النسب الحيّة متبوعة بعلامة <c>%</c>.</summary>
        public static string Percent(decimal value, IReadOnlyDictionary<string, string>? values = null)
            => _FormatNumber(value, PercentDecimals, "%", values);

        // ───────────────────────────── القراءة ─────────────────────────────

        /// <summary>
        /// يقرأ نعم/لا بأي صيغة يقبلها العقد (<c>"true"</c>/<c>"1"</c>/<c>"نعم"</c>) — <c>true</c> للنعمى فقط،
        /// وأي قيمة أخرى (بما فيها الفارغة أو غير المعروفة) تُقرأ «لا» كما في <c>NormalizeToggle</c>.
        /// </summary>
        public static bool IsOn(string? value) => NormalizeToggle(value) == ToggleTrue;

        /// <summary>يقرأ مبلغاً من نص المستخدم — يُعيد <c>false</c> للنص الفارغ أو غير الرقمي.</summary>
        public static bool TryParseMoney(string? text, out decimal value)
        {
            return TryParseCore(text, out value);
        }

        /// <summary>يقرأ كمية من نص المستخدم — يُعيد <c>false</c> للنص الفارغ أو غير الرقمي.</summary>
        public static bool TryParseQuantity(string? text, out decimal value)
        {
            return TryParseCore(text, out value);
        }

        /// <summary>
        /// القارئ الوحيد للأرقام (لا ثاني له): يطبّع الأرقام العربية والفواصل، ويحلّ فاصل الكسر
        /// على أساس الإعداد الحيّ، ثم يقرأ بثقافة ثابتة — ولا يرمي استثناءً أبداً.
        /// </summary>
        private static bool TryParseCore(string? text, out decimal value)
        {
            value = 0m;

            if (string.IsNullOrWhiteSpace(text)) return false;

            var work = _MapArabicDigits(text.Trim());

            var configured = _Read(DecimalSeparator, null);
            var decimalSeparator = configured == "," ? ',' : '.';
            var otherSeparator = decimalSeparator == ',' ? '.' : ',';

            if (work.IndexOf(decimalSeparator) >= 0)
            {
                // فاصل الكسر الحيّ هو الفاصل العشري، ومعه تُعد كل تكرارات الفاصل الآخر تجميعاً.
                work = _KeepLastAsDecimal(work, decimalSeparator);
                work = work.Replace(otherSeparator.ToString(), string.Empty);
            }
            else
            {
                var index = work.IndexOf(otherSeparator);

                if (index >= 0 && work.IndexOf(otherSeparator, index + 1) < 0 && _IsToleratedDecimal(work, index))
                    work = work.Substring(0, index) + "." + work.Substring(index + 1);
                else
                    work = work.Replace(otherSeparator.ToString(), string.Empty);
            }

            if (decimalSeparator == ',') work = work.Replace(',', '.');

            return decimal.TryParse(
                work,
                NumberStyles.Number | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out value);
        }

        // ───────────────────────────── داخلي ─────────────────────────────

        private static List<FormatSettingDefinition> BuildDefinitions()
        {
            return new List<FormatSettingDefinition>
            {
                new(MoneyDecimals, "منازل المبالغ", NumbersGroup, FormatSettingKind.Decimals, "2", 0, 4),
                new(QuantityDecimals, "منازل الكميات", NumbersGroup, FormatSettingKind.Decimals, "3", 0, 4),
                new(NumberDecimals, "منازل الأعداد", NumbersGroup, FormatSettingKind.Decimals, "0", 0, 4),
                new(PercentDecimals, "منازل النسب", NumbersGroup, FormatSettingKind.Decimals, "2", 0, 4),
                new(ThousandsSeparator, "فاصل الآلاف", NumbersGroup, FormatSettingKind.Toggle, ToggleTrue),
                new(DecimalSeparator, "فاصل الكسر", NumbersGroup, FormatSettingKind.Choice, ".", AllowedValues: new[] { ".", "," }),
                new(ArabicIndicDigits, "أرقام عربية (٠١٢٣٤٥٦٧٨٩)", NumbersGroup, FormatSettingKind.Toggle, ToggleFalse),

                // الافتراضي أولاً في كل قائمة (يُختار تلقائياً عند استعادة الافتراضي) — ولا تُضيَّق الصيغة المكتوبة يدوياً:
                // التحقق يبقى عبر _IsValidPattern لا عبر القائمة، فالقائمة اقتراح سريع للمستخدم.
                new(DateShortPattern, "صيغة التاريخ القصير", DatesGroup, FormatSettingKind.Pattern, "dd/MMM/yyyy",
                    AllowedValues: new[] { "dd/MMM/yyyy", "dd/MM/yyyy", "dd-MM-yyyy", "yyyy/MM/dd" }),
                new(DateDayMonthYearPattern, "صيغة يوم/شهر/سنة", DatesGroup, FormatSettingKind.Pattern, "dd/MM/yyyy",
                    AllowedValues: new[] { "dd/MM/yyyy", "dd-MM-yyyy", "yyyy/MM/dd", "dd/MM/yy" }),
                new(DateLongPattern, "صيغة التاريخ والوقت", DatesGroup, FormatSettingKind.Pattern, "dd/MM/yyyy HH:mm",
                    AllowedValues: new[] { "dd/MM/yyyy HH:mm", "dd/MM/yyyy hh:mm tt", "yyyy/MM/dd HH:mm", "dd/MM/yyyy HH:mm:ss" })
            };
        }

        private static Dictionary<string, FormatSettingDefinition> BuildDefinitionsIndex()
        {
            var index = new Dictionary<string, FormatSettingDefinition>(Definitions.Count, StringComparer.Ordinal);

            foreach (var definition in Definitions)
                index[definition.Key] = definition;

            return index;
        }

        private static Dictionary<string, string> BuildInitialValues()
        {
            var initial = new Dictionary<string, string>(Definitions.Count, StringComparer.Ordinal);

            foreach (var definition in Definitions)
                initial[definition.Key] = definition.DefaultValue;

            return initial;
        }

        private static FormatSettingDefinition? _Find(string? key)
            => key is not null && _definitionsByKey.TryGetValue(key, out var definition) ? definition : null;

        /// <summary>النص العربي الوصفي لعدد المنازل العشرية (يُستهلك في خيارات القوائم فقط).</summary>
        private static string _DecimalsWord(int count) => count switch
        {
            0 => "بلا كسور",
            1 => "منزلة واحدة",
            2 => "منزلتان",
            3 => "ثلاث منازل",
            _ => "أربع منازل"
        };

        /// <summary>النص الودّي لقيمة فاصل (يُستهلك في خيارات القوائم فقط — قيمة غير معروفة تُعرض كما هي).</summary>
        private static string _ChoiceText(string value) => value switch
        {
            "." => "نقطة (.) — مثال: 1,234.56",
            "," => "فاصلة (,) — مثال: 1.234,56",
            _ => value
        };

        /// <summary>القارئ الوحيد للقيم: دفعة المعاينة ← القيمة الحيّة ← الافتراضي (بلا نسخة ثانية من الافتراضيات).</summary>
        private static string _Read(string key, IReadOnlyDictionary<string, string>? values)
        {
            if (values is not null && values.TryGetValue(key, out var draft) && !string.IsNullOrWhiteSpace(draft))
                return draft.Trim();

            if (_values.TryGetValue(key, out var live) && !string.IsNullOrWhiteSpace(live))
                return live.Trim();

            return _Find(key)?.DefaultValue ?? string.Empty;
        }

        private static int _Decimals(string key, IReadOnlyDictionary<string, string>? values)
        {
            var definition = _Find(key);
            var min = definition?.Min ?? 0;
            var max = definition?.Max ?? 4;

            if (int.TryParse(_Read(key, values), NumberStyles.Integer, CultureInfo.InvariantCulture, out var count))
                return count < min ? min : count > max ? max : count;

            return int.TryParse(definition?.DefaultValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var fallback)
                ? fallback
                : min;
        }

        private static bool _Toggle(string key, IReadOnlyDictionary<string, string>? values)
            => IsOn(_Read(key, values));

        /// <summary>
        /// القاعدة المتقاطعة الوحيدة (فاصل آلاف مفعّل + «,» فاصلَ كسر = التباس) — يستهلكها
        /// <c>ValidateAll</c> و<c>TrySet</c> معاً، فلا تُنسخ الرسالة ولا الشرط في أي موضع آخر.
        /// </summary>
        private static string? _PairRuleError(bool thousandsSeparator, string? decimalSeparator)
        {
            if (!thousandsSeparator) return null;

            return (decimalSeparator ?? string.Empty).Trim() == ","
                ? "لا يمكن استخدام «,» فاصلَ كسر مع فاصل الآلاف في الوقت نفسه — اختر «.» أو أوقف فاصل الآلاف."
                : null;
        }

        private static NumberFormatInfo _NumberFormat(IReadOnlyDictionary<string, string>? values)
        {
            var format = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

            var separator = _Read(DecimalSeparator, values);
            format.NumberDecimalSeparator = separator == "," ? "," : ".";

            if (_Toggle(ThousandsSeparator, values))
            {
                format.NumberGroupSeparator = ",";
                format.NumberGroupSizes = new[] { 3 };
            }
            else
            {
                format.NumberGroupSeparator = string.Empty;
                format.NumberGroupSizes = new[] { 0 };
            }

            return format;
        }

        private static string _FormatNumber(
            decimal value,
            string decimalsKey,
            string suffix,
            IReadOnlyDictionary<string, string>? values)
        {
            var decimals = _Decimals(decimalsKey, values);
            var format = "N" + decimals.ToString(CultureInfo.InvariantCulture);

            return _ApplyDigits(value.ToString(format, _NumberFormat(values)) + suffix, values);
        }

        private static string _FormatDate(DateTime dt, string patternKey, IReadOnlyDictionary<string, string>? values)
        {
            var pattern = _Read(patternKey, values);
            var fallback = _Find(patternKey)?.DefaultValue ?? InvariantExamplePattern;

            string text;
            try
            {
                text = dt.ToString(pattern, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                text = dt.ToString(fallback, CultureInfo.InvariantCulture);
            }

            return _ApplyDigits(text, values);
        }

        /// <summary>يحوّل الأرقام اللاتينية 0-9 في النص النهائي إلى عربية-هندية ٠-٩ (الفواصل والنسب كما هي).</summary>
        private static string _ApplyDigits(string text, IReadOnlyDictionary<string, string>? values)
        {
            if (string.IsNullOrEmpty(text) || !_Toggle(ArabicIndicDigits, values)) return text;

            var buffer = new char[text.Length];
            for (var i = 0; i < text.Length; i++)
            {
                var character = text[i];
                buffer[i] = character >= '0' && character <= '9'
                    ? (char)('\u0660' + (character - '0'))
                    : character;
            }

            return new string(buffer);
        }

        private static string _MapArabicDigits(string text)
        {
            var buffer = new char[text.Length];
            for (var i = 0; i < text.Length; i++)
            {
                var character = text[i];
                buffer[i] = character switch
                {
                    >= '\u0660' and <= '\u0669' => (char)('0' + (character - '\u0660')),
                    '\u066B' => '.',
                    '\u066C' => ',',
                    _ => character
                };
            }

            return new string(buffer);
        }

        /// <summary>يُبقي آخر ظهور لفاصل الكسر كعلامة عشرية ويعامل ما قبله كتجميع (نص المستخدم قد يكون ملتبساً).</summary>
        private static string _KeepLastAsDecimal(string text, char separator)
        {
            var last = text.LastIndexOf(separator);
            if (last < 0 || text.IndexOf(separator) == last) return text;

            var buffer = new char[text.Length];
            var length = 0;

            for (var i = 0; i < text.Length; i++)
            {
                if (i == last) buffer[length++] = separator;
                else if (text[i] != separator) buffer[length++] = text[i];
            }

            return new string(buffer, 0, length);
        }

        /// <summary>تسامح: فاصل واحد يتبعه 1–4 أرقام فقط يُقرأ كفاصل كسر (مستخدم كتب <c>1234.56</c> والإعداد «,»).</summary>
        private static bool _IsToleratedDecimal(string text, int separatorIndex)
        {
            var digitsAfter = text.Length - separatorIndex - 1;
            if (digitsAfter < 1 || digitsAfter > 4) return false;

            for (var i = separatorIndex + 1; i < text.Length; i++)
            {
                if (text[i] < '0' || text[i] > '9') return false;
            }

            return true;
        }

        private static bool _IsAllowed(FormatSettingDefinition definition, string text)
        {
            if (definition.AllowedValues is null) return false;

            foreach (var allowed in definition.AllowedValues)
            {
                if (string.Equals(allowed, text, StringComparison.Ordinal)) return true;
            }

            return false;
        }

        private static bool _IsValidPattern(string pattern)
        {
            if (pattern.Length == 0 || pattern.Length > MaxPatternLength) return false;
            if (pattern.IndexOf('d') < 0 && pattern.IndexOf('M') < 0 && pattern.IndexOf('y') < 0) return false;

            try
            {
                _ = DateTime.Now.ToString(pattern, CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string? NormalizeToggle(string? text)
        {
            return (text ?? string.Empty).Trim().ToLowerInvariant() switch
            {
                "true" or "1" or "نعم" => ToggleTrue,
                "false" or "0" or "لا" => ToggleFalse,
                _ => null
            };
        }

        private static string NormalizeValue(FormatSettingDefinition definition, string? value)
        {
            var text = (value ?? string.Empty).Trim();

            return definition.Kind switch
            {
                FormatSettingKind.Decimals => int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var count)
                    ? count.ToString(CultureInfo.InvariantCulture)
                    : definition.DefaultValue,
                FormatSettingKind.Toggle => NormalizeToggle(text) ?? definition.DefaultValue,
                _ => text
            };
        }
    }
}
