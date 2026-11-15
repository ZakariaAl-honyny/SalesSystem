namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// البيت الواحد للنصوص الثابتة المشتركة بين شاشات قوائم المستندات
/// (<c>RULE-N169</c>…<c>RULE-N172</c>): اسم العميل النقدي الافتراضي ونص حالة المستند.
/// <para>
/// كانت هذه النصوص مكرّرة حرفياً في شاشات القوائم — نسختان لاسم العميل النقدي ونسختان لنص
/// الحالة — فكان تصحيح نص عربي واحد يتطلّب لمس أكثر من ملف شاشة، وهو ما ينفيه معيار التعديل
/// الواحد (<c>RULE-N171</c>) وسقف التكرار (<c>RULE-N172</c>).
/// </para>
/// <para>
/// النصوص هنا منقولة **حرفياً** من الشاشات القائمة (لا ترجمة ولا إعادة صياغة) كي يبقى ما يراه
/// المستخدم في الشبكة والتصفية والتصدير كما هو بالضبط بعد توحيد المصدر. والحالات تطابق
/// <c>InvoiceStatus</c> (<c>Draft = 1</c> · <c>Posted = 2</c> · <c>Cancelled = 3</c> — <c>RULE-019</c>).
/// </para>
/// <para>
/// لا رسائل ولا ألوان ولا وصول لخدمات: هذا الـHelper يُعيد نصاً فقط.
/// </para>
/// </remarks>
public static class DocumentTextHelper
{
    /// <summary>
    /// اسم العميل النقدي الافتراضي للفاتورة بلا عميل مسجَّل — يُستهلك في
    /// <c>frmListSales</c> و<c>frmListReturns</c> (الشاشتان كانتا تعلنانه ثابتاً محلّياً — <c>RULE-N172</c>).
    /// </summary>
    /// <remarks>النص منقول حرفياً من <c>frmListSales.cs:15</c> و<c>frmListReturns.cs:13</c>.</remarks>
    public const string CashCustomerName = "عميل نقدي";

    /// <summary>
    /// اسم العميل المعروض: اسم العميل إن وُجد، وإلا <see cref="CashCustomerName"/>
    /// (<c>RULE-N169</c> — بيت واحد بدل تكرار الشرط في كل شاشة قوائم).
    /// </summary>
    /// <param name="name">اسم العميل كما جاء من عقد البيانات (قد يكون فارغاً).</param>
    public static string CashCustomerNameOr(string? name)
        => string.IsNullOrWhiteSpace(name) ? CashCustomerName : name;

    /// <summary>
    /// نص حالة المستند بالعربية لحالة <c>InvoiceStatus</c>: مسودة · مرحل · ملغي · غير معروف
    /// — يُستهلك في <c>frmListSales</c> و<c>frmListPurchases</c> (كانتا تكرّران نفس الدالة المحلّية — <c>RULE-N172</c>).
    /// </summary>
    /// <param name="status">حالة المستند (<c>Draft = 1</c> · <c>Posted = 2</c> · <c>Cancelled = 3</c>).</param>
    public static string StatusText(byte status) => status switch
    {
        1 => "مسودة",
        2 => "مرحل",
        3 => "ملغي",
        _ => "غير معروف"
    };

    /// <summary>
    /// أزواج (قيمة الحالة ↔ نصها) لقوائم التصفية — المصدر الوحيد، فلا تُكتب الأزواج في شاشة (<c>RULE-N169</c>).
    /// </summary>
    /// <remarks>
    /// النصوص تُقرأ من <see cref="StatusText(byte)"/> نفسها ولا تُكرَّر هنا (<c>RULE-N171</c>).
    /// العنصر المحايد «الكل» (القيمة <c>0</c>) ليس نص حالة، فيبقى محلّياً في كل شاشة.
    /// </remarks>
    public static IReadOnlyList<(byte Value, string Text)> StatusOptions { get; } =
        new (byte Value, string Text)[]
        {
            (1, StatusText(1)),
            (2, StatusText(2)),
            (3, StatusText(3))
        };
}
