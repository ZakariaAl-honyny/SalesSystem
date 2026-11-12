// -----------------------------------------------------------------------------
// LiveTafqeetHelper.cs — تفقيط حيّ يظهر كتلميح (ToolTip) بجوار الحقل الرقمي
//
// الفكرة:
//   عند كل تغيير في الحقل الرقمي (أو عند دخول المؤشر إليه) يظهر تلميح أسفل الحقل
//   مباشرةً يعرض قيمته بالحروف العربية، فيقرأ المستخدم الرقم ويتحقق من عدد الأصفار
//   قبل الحفظ — بدل أن يكتشف الخطأ بعد الترحيل.
//
// الاستخدام:
//     LiveTafqeetHelper.Attach(
//         liveTip,
//         (txtQuantity,  "الكمية", LiveTafqeetHelper.ReadMode.Quantity),
//         (txtUnitPrice, "السعر",  LiveTafqeetHelper.ReadMode.Money));
//
// ملاحظات:
//   * لا يحتفظ بأي حالة إلا ذاكرة آخر نص معروض لكل حقل (لمنع الوميض والتكرار).
//   * لا يرمي استثناءً أبداً: يعمل داخل حدث كتابة، وأي فشل يُخفي التلميح فقط.
//   * يعتمد على ArabicNumberToWords (لا يعدّله) للتفقيط نفسه.
// -----------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;

namespace SalesSystem.Desktop.Helpers;

/// <summary>يربط حقولاً رقمية بتلميح واحد يعرض قيمتها بالحروف العربية عند كل تغيير.</summary>
public static class LiveTafqeetHelper
{
    /// <summary>نمط القراءة: مبلغ مالي (بالعملة) أو كمية (بلا عملة).</summary>
    public enum ReadMode { Money, Quantity }

    /// <summary>مدة بقاء التلميح على الشاشة (بالميلي ثانية).</summary>
    private const int DisplayMs = 4000;

    /// <summary>الاسم الظاهر لحدث تغيير القيمة في العناصر المخصّصة (غير NumericUpDown).</summary>
    private const string ValueChangedEventName = "ValueChanged";

    /// <summary>آخر نص عُرض لكل حقل — لمنع إعادة العرض المتطابق (وميض).</summary>
    private static readonly ConcurrentDictionary<Control, string> _lastShown = new();

    /// <summary>
    /// يربط حقولاً رقمية بتلميح واحد. يقبل TextBox ومشتقاته و NumericUpDown.
    /// </summary>
    public static void Attach(
        ToolTip tip,
        params (Control Field, string Caption, ReadMode Mode)[] bindings)
    {
        if (tip is null || bindings is null || bindings.Length == 0)
            return;

        foreach (var binding in bindings)
        {
            var field = binding.Field;
            if (field is null)
                continue;

            var caption = binding.Caption ?? string.Empty;
            var mode = binding.Mode;

            try
            {
                if (field is NumericUpDown nud)
                {
                    nud.ValueChanged += (_, _) => Render(tip, nud, caption, mode, force: false);
                }
                else
                {
                    // TextBox ومشتقاته (مثل MoneyTextBox) تُطلق TextChanged.
                    field.TextChanged += (_, _) => Render(tip, field, caption, mode, force: false);

                    // عناصر رقمية مخصّصة قد تعرّف ValueChanged دون أن تكون NumericUpDown.
                    SubscribeValueChangedIfAvailable(field, tip, caption, mode);
                }

                // دخول المؤشر إلى الحقل = عرض فوري لقيمته الحالية (بدون انتظار كتابة).
                field.Enter += (_, _) => ShowOnEnter(tip, field, caption, mode);

                // الخروج من الحقل = إخفاء التلميح وتنظيف ذاكرة آخر نص معروض.
                field.Leave += (_, _) => Forget(tip, field);
            }
            catch
            {
                // ربط حقل واحد فشل (عنصر غريب/مُدمَّر) — نتجاهله ولا نُسقط النموذج.
            }
        }
    }

    /// <summary>
    /// يشترك في حدث ValueChanged إن كان العنصر يعرّفه بنوع EventHandler (عناصر مخصّصة غير NumericUpDown).
    /// </summary>
    private static void SubscribeValueChangedIfAvailable(
        Control field, ToolTip tip, string caption, ReadMode mode)
    {
        try
        {
            var eventInfo = field.GetType().GetEvent(
                ValueChangedEventName,
                BindingFlags.Public | BindingFlags.Instance);

            if (eventInfo is null || eventInfo.EventHandlerType != typeof(EventHandler))
                return;

            eventInfo.AddEventHandler(
                field,
                new EventHandler((_, _) => Render(tip, field, caption, mode, force: false)));
        }
        catch
        {
            // لا حدث بهذا الاسم/التوقيع — نكتفي بـ TextChanged.
        }
    }

    /// <summary>يعرض القيمة عند دخول المؤشر إلى الحقل بعد استقرار التركيز.</summary>
    private static void ShowOnEnter(ToolTip tip, Control field, string caption, ReadMode mode)
    {
        try
        {
            if (field is null || field.IsDisposed)
                return;

            if (field.IsHandleCreated)
            {
                // التركيز قد لا يكون قد استقر بعد عند لحظة حدث Enter — نؤجّل العرض قليلاً.
                field.BeginInvoke(new Action(() => Render(tip, field, caption, mode, force: true)));
            }
            else
            {
                Render(tip, field, caption, mode, force: true);
            }
        }
        catch
        {
            HideTip(tip, field);
        }
    }

    /// <summary>
    /// يقرأ قيمة الحقل ويعرضها بالحروف العربية في تلميح أسفل الحقل مباشرةً.
    /// لا يرمي استثناءً في أي حال (يعمل داخل حدث كتابة).
    /// </summary>
    private static void Render(ToolTip tip, Control field, string caption, ReadMode mode, bool force)
    {
        try
        {
            if (tip is null || field is null || field.IsDisposed || field.Disposing)
                return;

            // التلميح يظهر فقط للحقل الذي يكتب فيه المستخدم الآن.
            if (!HasFocus(field))
            {
                HideTip(tip, field);
                return;
            }

            if (!TryReadValue(field, out var value))
            {
                Forget(tip, field);
                return;
            }

            string words;
            if (mode == ReadMode.Quantity)
            {
                // الكمية قد تكون كسرية (decimal(18,3)) — نقرأها "صحيح فاصلة كسر"
                // لأن ArabicNumberToWords يعامل الكسر كوحدة نقدية فرعية فتقرأ 1.5 خطأً "واحد وخمسون".
                var wholePart = decimal.Truncate(value);
                if (value == wholePart)
                {
                    words = ArabicNumberToWords.ToWords(wholePart, currency: string.Empty, subUnit: string.Empty, appendOnly: false);
                }
                else
                {
                    var wholeText = ArabicNumberToWords.ToWords(wholePart, currency: string.Empty, subUnit: string.Empty, appendOnly: false);

                    var fractionDigits = Math.Abs(value - wholePart).ToString("0.###", CultureInfo.InvariantCulture)
                        .Split('.')[1];                       // "5" / "25" / "125"

                    var fractionAsNumber = decimal.Parse("0." + fractionDigits, CultureInfo.InvariantCulture);
                    var scale = 1m;
                    for (var i = 0; i < fractionDigits.Length; i++) scale *= 10m;
                    var fractionWhole = (long)(fractionAsNumber * scale);

                    var fractionText = ArabicNumberToWords.ToWords(fractionWhole, currency: string.Empty, subUnit: string.Empty, appendOnly: false);

                    words = $"{wholeText} فاصلة {fractionText}";
                }
            }
            else
            {
                words = ArabicNumberToWords.ToWords(value, appendOnly: false);
            }

            if (string.IsNullOrWhiteSpace(words))
            {
                Forget(tip, field);
                return;
            }

            var text = $"{caption}: {words}";

            // نفس النص المعروض سابقاً لنفس الحقل — لا داعي لإعادة إظهاره (منع الوميض).
            if (!force
                && _lastShown.TryGetValue(field, out var last)
                && string.Equals(last, text, StringComparison.Ordinal))
            {
                return;
            }

            _lastShown[field] = text;

            tip.Show(text, field, 0, field.Height + 4, DisplayMs);
        }
        catch
        {
            HideTip(tip, field);
        }
    }

    /// <summary>هل الحقل (أو أحد أبنائه مثل مربع نص NumericUpDown) هو صاحب التركيز؟</summary>
    private static bool HasFocus(Control field)
    {
        try
        {
            return field is not null && (field.Focused || field.ContainsFocus);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>يخفي التلميح وينسى آخر نص معروض للحقل.</summary>
    private static void Forget(ToolTip tip, Control field)
    {
        try
        {
            if (field is not null)
                _lastShown.TryRemove(field, out _);
        }
        catch
        {
            // تجاهل — لا نُسقط النموذج أثناء الكتابة.
        }

        HideTip(tip, field);
    }

    /// <summary>يخفي التلميح بأمان (قد لا يكون معروضاً أصلاً).</summary>
    private static void HideTip(ToolTip tip, Control field)
    {
        try
        {
            if (tip is not null && field is not null && !field.IsDisposed && !field.Disposing)
                tip.Hide(field);
        }
        catch
        {
            // تجاهل — لا نُسقط النموذج أثناء الكتابة.
        }
    }

    /// <summary>يقرأ القيمة الرقمية من الحقل (NumericUpDown.Value أو نص الحقل).</summary>
    private static bool TryReadValue(Control field, out decimal value)
    {
        value = 0m;

        if (field is null)
            return false;

        if (field is NumericUpDown nud)
        {
            value = nud.Value;
            return true;
        }

        var text = (field.Text ?? string.Empty).Trim();
        if (text.Length == 0)
            return false;

        // 1) الأرقام المنسّقة بفواصل الآلاف (1,500.00) — بأسلوب محايد الثقافة.
        if (decimal.TryParse(text.Replace(",", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture, out var invariant))
        {
            value = invariant;
            return true;
        }

        // 2) ثقافة الجهاز الحالية (فواصل عشرية / آلاف عربية).
        if (decimal.TryParse(text, out var current))
        {
            value = current;
            return true;
        }

        return false;
    }
}
