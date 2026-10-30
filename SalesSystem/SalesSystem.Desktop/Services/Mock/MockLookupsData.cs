using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للتصنيفات والوحدات — بيانات أساسية لاختبار الواجهة بدون API.
/// </summary>
internal static class MockLookupsData
{
    // ═══════════════════════════════════════════════
    // التصنيفات — Categories (8 تصنيفات)
    // ═══════════════════════════════════════════════
    public static List<CategoryDto> Categories { get; } = new()
    {
        new CategoryDto(1, "مشروبات ومياه",     "المشروبات الغازية والعصائر والمياه المعدنية",              true),
        new CategoryDto(2, "منتجات ألبان",      "الحليب والزبادي والجبنة والقشطة ومنتجات الألبان",          true),
        new CategoryDto(3, "حلويات وبسكويتات",  "البسكويت والشوكولاتة والحلويات والكيك",                    true),
        new CategoryDto(4, "مواد غذائية",       "الأرز والسكر والزيت والدقيق والمعكرونة والتوابل",          true),
        new CategoryDto(5, "معلبات",            "التونة والمعجون والمخللات والفواكه المعلبة",                true),
        new CategoryDto(6, "منظفات",            "مساحيق الغسيل والصابون والمطهرات ومنظفات الأسطح",         true),
        new CategoryDto(7, "أدوات منزلية",      "الأكياس والممسحة والأدوات البلاستيكية والزجاجية",          true),
        new CategoryDto(8, "عام",               "أصناف متنوعة لا تنتمي لتصنيف محدد",                       true)
    };

    // ═══════════════════════════════════════════════
    // الوحدات — Units (8 وحدات)
    // ═══════════════════════════════════════════════
    // TODO: Description removed from UnitDto — الوصف لم يعد جزءاً من DTO الوحدة
    public static List<UnitDto> Units { get; } = new()
    {
        new UnitDto(1, "حبة",   "حبة",   true),
        new UnitDto(2, "كرتون", "كرتون", true),
        new UnitDto(3, "باكت",  "باكت",  true),
        new UnitDto(4, "كيس",   "كيس",   true),
        new UnitDto(5, "علبة",  "علبة",  true),
        new UnitDto(6, "طقم",   "طقم",   true),
        new UnitDto(7, "رول",   "رول",   true),
        new UnitDto(8, "كجم",   "كجم",   true)
    };
}
