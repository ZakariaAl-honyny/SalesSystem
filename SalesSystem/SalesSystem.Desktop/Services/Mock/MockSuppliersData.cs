using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للموردين — مستقلة عن MockData الرئيسي.
/// حقول المورّد كما في النموذج (بلا حساب مرتبط):
/// رصيد المورد يُشتق من مستنداته.
/// </summary>
internal static class MockSuppliersData
{
    public static List<SupplierDto> Suppliers { get; } = new()
    {
        new SupplierDto(
            Id: 1, Name: "مجموعة هائل سعيد أنعم وشركاه", Phone: "01-200300",
            Email: "info@hsagroup.com", Address: "تعز / صنعاء - شارع الزبيري",
            Notes: "المورد الرئيسي للزيوت والمواد التموينية والألبان",
            CreditLimit: 2500000m, IsActive: true),

        new SupplierDto(
            Id: 2, Name: "شركة مياه ومشروبات حدة المحدودة", Phone: "01-443322",
            Email: "sales@hadawater.ye", Address: "صنعاء - منطقة حدة القديمة",
            Notes: "مورد المياه الصحية والعصائر",
            CreditLimit: 500000m, IsActive: true),

        new SupplierDto(
            Id: 3, Name: "مؤسسة التضامن لتجارة المواد الغذائية", Phone: "778998877",
            Email: "tadamon.trade@gmail.com", Address: "صنعاء - سوق الملح / شارع خولان",
            Notes: "مورد الأرز والحبوب والبقوليات المستوردة",
            CreditLimit: 1200000m, IsActive: true),

        new SupplierDto(
            Id: 4, Name: "شركة الكبوس للتجارة والصناعة", Phone: "01-505050",
            Email: "info@alkabous.com", Address: "صنعاء - الروضة / شارع المطار",
            Notes: "مورد الشاي والبن والبهارات",
            CreditLimit: 800000m, IsActive: true),

        new SupplierDto(
            Id: 5, Name: "مؤسسة الحمادي للتوريدات الغذائية", Phone: "777555666",
            Email: null, Address: "عدن - كريتر - شارع العباس",
            Notes: "مورد السكر والزيوت والمعكرونة للسوق الجنوبي",
            CreditLimit: 600000m, IsActive: false)
    };
}
