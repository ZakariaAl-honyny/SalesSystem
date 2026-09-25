using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للموردين — مستقلة عن MockData الرئيسي.
/// </summary>
internal static class MockSuppliersData
{
    public static List<SupplierDto> Suppliers { get; } = new()
    {
        new SupplierDto(
            Id: 1, Name: "مجموعة هائل سعيد أنعم وشركاه", Phone: "01-200300",
            Email: "info@hsagroup.com", Address: "تعز / صنعاء - شارع الزبيري",
            TaxNumber: "TAX-YE-900188",
            Notes: "المورد الرئيسي للزيوت والمواد التموينية والألبان",
            CreditLimit: 2500000m, IsActive: true,
            AccountId: 301, AccountName: "موردون - مجموعة هائل سعيد أنعم وشركاه",
            Code: "211001", Company: "شركة هائل سعيد أنعم للتجارة والصناعة",
            Currency: "YER", ContactPerson: null, OpeningBalance: 0m,
            CurrentBalance: 450000m, CreditDays: 30),

        new SupplierDto(
            Id: 2, Name: "شركة مياه ومشروبات حدة المحدودة", Phone: "01-443322",
            Email: "sales@hadawater.ye", Address: "صنعاء - منطقة حدة القديمة",
            TaxNumber: "TAX-YE-774011",
            Notes: "مورد المياه الصحية والعصائر",
            CreditLimit: 500000m, IsActive: true,
            AccountId: 302, AccountName: "موردون - شركة مياه ومشروبات حدة المحدودة",
            Code: "211002", Company: "مصنع مياه حدة المعدنية",
            Currency: "YER", ContactPerson: null, OpeningBalance: 0m,
            CurrentBalance: 120000m, CreditDays: 15),

        new SupplierDto(
            Id: 3, Name: "مؤسسة التضامن لتجارة المواد الغذائية", Phone: "778998877",
            Email: "tadamon.trade@gmail.com", Address: "صنعاء - سوق الملح / شارع خولان",
            TaxNumber: "TAX-YE-332190",
            Notes: "مورد الأرز والحبوب والبقوليات المستوردة",
            CreditLimit: 1200000m, IsActive: true,
            AccountId: 303, AccountName: "موردون - مؤسسة التضامن لتجارة المواد الغذائية",
            Code: "211003", Company: "مؤسسة التضامن العامة",
            Currency: "YER", ContactPerson: null, OpeningBalance: 0m,
            CurrentBalance: 310000m, CreditDays: 45),

        new SupplierDto(
            Id: 4, Name: "شركة الكبوس للتجارة والصناعة", Phone: "01-505050",
            Email: "info@alkabous.com", Address: "صنعاء - الروضة / شارع المطار",
            TaxNumber: "TAX-YE-454120",
            Notes: "مورد الشاي والبن والبهارات",
            CreditLimit: 800000m, IsActive: true,
            AccountId: 304, AccountName: "موردون - شركة الكبوس للتجارة والصناعة",
            Code: "211004", Company: "شاي ومواد الكبوس الاستهلاكية",
            Currency: "YER", ContactPerson: null, OpeningBalance: 0m,
            CurrentBalance: 88000m, CreditDays: 30),

        new SupplierDto(
            Id: 5, Name: "مؤسسة الحمادي للتوريدات الغذائية", Phone: "777555666",
            Email: null, Address: "عدن - كريتر - شارع العباس",
            TaxNumber: null,
            Notes: "مورد السكر والزيوت والمعكرونة للسوق الجنوبي",
            CreditLimit: 600000m, IsActive: false,
            AccountId: 305, AccountName: "موردون - مؤسسة الحمادي للتوريدات الغذائية",
            Code: "211005", Company: "مؤسسة الحمادي العامة",
            Currency: "YER", ContactPerson: "أحمد الحمادي", OpeningBalance: 0m,
            CurrentBalance: 0m, CreditDays: null)
    };
}
