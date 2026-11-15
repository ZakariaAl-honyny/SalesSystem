namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// البيت الواحد لحصر قيمة رقمية داخل مدى عنصر إدخال (<see cref="NumericUpDown"/>) قبل إسنادها
/// إليه (<c>RULE-N169</c>…<c>RULE-N172</c>).
/// <para>
/// كان الحصر مكتوباً **خمس مرات** كدالة محلّية متطابقة
/// (<c>frmCustomerMaster</c> · <c>frmEmployeeMaster</c> · <c>frmProductMaster</c> ·
/// <c>frmProductUnitMaster</c> · <c>frmSupplierMaster</c>)؛ فأي تعديل على سلوك الحصر كان يتطلّب
/// لمس خمسة ملفات، وهو ما ينفيه معيار التعديل الواحد (<c>RULE-N171</c>) وسقف التكرار
/// (<c>RULE-N172</c>).
/// </para>
/// <para>
/// <b>لا يرمي استثناءً أبداً</b> (على نمط أمان بقية Helpers هذا المجلد): مرور <c>null</c> بدل
/// العنصر يُعيد القيمة كما هي بدل إسقاط مسار التحميل/الإدخال.
/// </para>
/// <para>
/// و<see cref="Math.Clamp(decimal, decimal, decimal)"/> هنا مكافئ دلالياً للصيغة القديمة
/// <c>Math.Min(Math.Max(value, Minimum), Maximum)</c> ما دام <c>Minimum &lt;= Maximum</c>، وهو
/// ما يكفله <see cref="NumericUpDown"/> نفسه (ضابط <c>Minimum</c> يرفع <c>Maximum</c> تلقائياً
/// والعكس، فلا يُخزَّن مدى مقلوب)، وعندها لا يمكن أن يرمي <c>Clamp</c>.
/// </para>
/// <para>
/// لا ألوان ولا رسائل ولا وصول لخدمات: هذا الـHelper يحصر رقماً فقط.
/// </para>
/// </remarks>
public static class NumericInputHelper
{
    /// <summary>
    /// يحصر القيمة داخل مدى عنصر الإدخال الرقمي قبل إسنادها (<c>RULE-N172</c> — بيت واحد).
    /// تُستهلك من شاشات الدليل الخمس التي كانت تكرّر الحصر محلّياً.
    /// </summary>
    /// <param name="control">عنصر الإدخال الرقمي — <c>null</c> ⇒ تُعاد القيمة بلا حصر.</param>
    /// <param name="value">القيمة المطلوب حصرها.</param>
    /// <returns>القيمة داخل <c>[Minimum, Maximum]</c>.</returns>
    public static decimal Clamp(NumericUpDown control, decimal value)
    {
        if (control is null) return value;

        return Math.Clamp(value, control.Minimum, control.Maximum);
    }

    /// <summary>
    /// الصيغة المقبولة للقيم الغائبة (قد تأتي من DTO <c>decimal?</c>): <c>null</c> ⇒
    /// <paramref name="fallback"/> ثم الحصر في مدى العنصر (<c>RULE-N172</c> — بيت واحد).
    /// </summary>
    /// <param name="control">عنصر الإدخال الرقمي — <c>null</c> ⇒ تُعاد القيمة المحلولة بلا حصر.</param>
    /// <param name="value">القيمة القابلة للغياب.</param>
    /// <param name="fallback">البديل عند غياب القيمة (صفر افتراضياً).</param>
    public static decimal Clamp(NumericUpDown control, decimal? value, decimal fallback = 0m)
        => Clamp(control, value ?? fallback);
}
