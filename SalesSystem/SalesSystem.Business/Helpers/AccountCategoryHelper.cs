namespace SalesSystem.Business.Helpers;

/// <summary>
/// البيت الواحد لتصنيف الحساب المالي الخاص («الدفتر المساعد») في دليل الحسابات — عميل · مورّد · صندوق · بنك · موظف.
/// القواعد مأخوذة من <c>docs/database-schema.md</c> §4.1.1 و§4.1.2: التصنيف بادئة الكود أولاً، والاسم كاحتياط أخير فقط.
/// ممنوع تكرار خريطة البادئات أو التسميات العربية في أي شاشة أو خدمة أو Mock — تُستهلك من هنا حصراً (`RULE-N169`…`RULE-N172`).
/// </summary>
public static class AccountCategoryHelper
{
    public const string CategoryCustomer = "CUSTOMER";
    public const string CategorySupplier = "SUPPLIER";
    public const string CategoryCashBox = "CASHBOX";
    public const string CategoryBank = "BANK";
    public const string CategoryEmployee = "EMPLOYEE";
    public const string CategoryStandard = "STANDARD";

    public const string LabelCustomer = "حساب عميل (دفتر مساعد)";
    public const string LabelSupplier = "حساب مورّد (دفتر مساعد)";
    public const string LabelCashBox = "حساب صندوق نقدي";
    public const string LabelBank = "حساب بنك أو محفظة";
    public const string LabelEmployee = "حساب موظف (عهدة)";
    public const string LabelStandard = "حساب مالي عام";

    /// <summary>
    /// خريطة البادئات الرسمية (§4.1.1) مبنيّة على <b>الأكواد الحقيقية</b> لبذور <c>AccountingSeeder</c> — المستوى الثالث (4 خانات).
    /// التصنيف يعتمد <b>فرع</b> الشجرة لا الورقة: كود من 8 خانات تحت <c>1103</c> (مثل <c>11030001</c>) يبقى «عميلاً» مثل أبيه،
    /// و<c>1101</c> و<c>11010001</c> يصنّفان صندوقاً — وهذا مقصود.
    /// الترتيب من الأطول إلى الأقصر حتى لا يبتلع القصير الخاص: المطابقة بـ<c>StartsWith</c>،
    /// فأي بادئة أطول تُضاف مستقبلاً تُكتب <b>فوق</b> البادئة التي هي مبدؤها.
    /// <para>
    /// <b>حُذفت البادئات القديمة عمداً (2026-11-17) — لا تُعاد من مراجعة قديمة للمواصفة:</b>
    /// <c>1121</c> · <c>1131</c> · <c>2141</c> · <c>2111</c> · <c>1111</c> · <c>1112</c> — أكواد 4 خانات لا وجود لها في الدليل الرسمي (الأكواد الحقيقية هي <c>1103</c> · <c>2101</c> · <c>1107</c> · <c>1102</c> · <c>1101</c>)؛
    /// و<c>113</c> · <c>112</c> · <c>111</c> · <c>214</c> · <c>211</c> — <b>3 خانات ويستحيل أن يولّدها السلّم الرسمي</b>:
    /// <c>docs/database-schema.md</c> §4.1.2.1 يفرض 1 ثم 2 ثم 4 ثم 8 خانات (مستوى 3 = 4 خانات دائماً)، والسلّم مطبَّق في <c>AccountCodeGeneratorService</c>.
    /// فمصدر الحقيقة هو الأكواد التي تُنتجها البذور فعلاً لا الصفوف المكرَّرة في المواصفة.
    /// </para>
    /// </summary>
    private static readonly (string Prefix, string Category, string Label)[] PrefixMap =
    {
        ("1103", CategoryCustomer, LabelCustomer),   // Accounts Receivable — العملاء
        ("2101", CategorySupplier, LabelSupplier),   // Accounts Payable — الموردون
        ("1107", CategoryEmployee, LabelEmployee),   // Employee Custody — عهد الموظفين
        ("1102", CategoryBank, LabelBank),           // Bank Accounts — البنوك
        ("1101", CategoryCashBox, LabelCashBox),     // Cash & Cash Equivalents — النقدية صناديق
    };

    /// <summary>
    /// احتياط أخير: دلالة الاسم العربي — يُستخدم فقط عندما لا يطابق الكود أي بادئة معروفة.
    /// <para>
    /// <b>ممنوع إضافة الكلمة «موظف» هنا (2026-11-17):</b> هي جزء شائع من اسم حساب <b>مصروف</b> غير متخصص
    /// (مثل «الرواتب والأجور» · «رواتب وأجور الموظفين والعمال»)، فكانت تُصنّف مصروف الرواتب «عهدة موظف».
    /// الكلمة الخاصة بالعهد هي «عهدة» وحدها، وحسابات العهد الحقيقية (<c>1107</c> وفروعه) تُلتقط ببادئتها.
    /// </para>
    /// </summary>
    private static readonly (string Keyword, string Category, string Label)[] NameHints =
    {
        ("عملاء", CategoryCustomer, LabelCustomer),
        ("عميل", CategoryCustomer, LabelCustomer),
        ("مدينون", CategoryCustomer, LabelCustomer),
        ("مورد", CategorySupplier, LabelSupplier),
        ("دائنون", CategorySupplier, LabelSupplier),
        ("صندوق", CategoryCashBox, LabelCashBox),
        ("صناديق", CategoryCashBox, LabelCashBox),
        ("خزينة", CategoryCashBox, LabelCashBox),
        ("خزائن", CategoryCashBox, LabelCashBox),
        ("بنك", CategoryBank, LabelBank),
        ("بنوك", CategoryBank, LabelBank),
        ("محفظة", CategoryBank, LabelBank),
        ("محافظ", CategoryBank, LabelBank),
        ("عهدة", CategoryEmployee, LabelEmployee),
    };

    /// <summary>
    /// يحدد تصنيف الحساب من كوده أولاً ثم من اسمه العربي كاحتياط.
    /// </summary>
    /// <param name="accountCode">كود الحساب (مثال: 1103 · 2101 · 1107 · 1102 · 1101 — أو فرع من 8 خانات تحتها).</param>
    /// <param name="nameAr">اسم الحساب بالعربية — يُستخدم فقط عند فشل مطابقة البادئة.</param>
    /// <returns>التصنيف · التسمية العربية · وهل الحساب متخصص (دفتر مساعد) أم حساب مالي عام.</returns>
    public static (string Category, string Label, bool IsSpecialized) Detect(string accountCode, string nameAr)
    {
        var code = accountCode?.Trim() ?? string.Empty;

        if (code.Length > 0)
        {
            foreach (var (prefix, category, label) in PrefixMap)
            {
                if (code.StartsWith(prefix, StringComparison.Ordinal))
                    return (category, label, true);
            }
        }

        var name = nameAr?.Trim() ?? string.Empty;
        if (name.Length > 0)
        {
            foreach (var (keyword, category, label) in NameHints)
            {
                if (name.Contains(keyword, StringComparison.Ordinal))
                    return (category, label, true);
            }
        }

        return (CategoryStandard, LabelStandard, false);
    }
}
