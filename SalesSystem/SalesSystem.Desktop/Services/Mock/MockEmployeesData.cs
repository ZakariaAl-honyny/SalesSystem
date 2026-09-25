using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للموظفين — 8 موظفين من تحليل النظام.
/// </summary>
internal static class MockEmployeesData
{
    public static List<EmployeeDto> Employees { get; } = new()
    {
        // ── 1: مدير النظام الأعلى ──
        new EmployeeDto(
            Id: 7,
            Code: "100",
            Name: "مدير النظام الأعلى (Admin)",
            JobTitle: "مدير عام",
            Phone: "770000001",
            NationalId: "01010000001",
            BasicSalary: 350000m,
            HireDate: new DateTime(2024, 1, 1),
            IsActive: true,
            Notes: "إدارة النظام بالكامل والمراجعة الإدارية العليا",
            AccountCode: "2141-07",
            LinkedUserId: 1,
            CommissionRate: 0m
        ),

        // ── 2: كاشير مبيعات ──
        new EmployeeDto(
            Id: 1,
            Code: "101",
            Name: "أحمد علي عبده",
            JobTitle: "كاشير مبيعات",
            Phone: "771234567",
            NationalId: "01010045892",
            BasicSalary: 180000m,
            HireDate: new DateTime(2025, 1, 15),
            IsActive: true,
            Notes: "كاشير رئيسي في نقطة البيع رقم 1",
            AccountCode: "2141-01",
            LinkedUserId: 2,
            CommissionRate: 0.5m
        ),

        // ── 3: مندوب وبائع ميداني ──
        new EmployeeDto(
            Id: 2,
            Code: "102",
            Name: "محمد عبد الله الصلوي",
            JobTitle: "مندوب وبائع ميداني",
            Phone: "733987654",
            NationalId: "01010087412",
            BasicSalary: 220000m,
            HireDate: new DateTime(2025, 3, 1),
            IsActive: true,
            Notes: "مندوب خط توزيع صنعاء وتعز",
            AccountCode: "2141-02",
            LinkedUserId: null,
            CommissionRate: 1.5m
        ),

        // ── 4: محاسب عام ──
        new EmployeeDto(
            Id: 6,
            Code: "106",
            Name: "سامي عبدالملك الجوفي",
            JobTitle: "محاسب عام",
            Phone: "772223344",
            NationalId: "01010077889",
            BasicSalary: 250000m,
            HireDate: new DateTime(2024, 10, 1),
            IsActive: true,
            Notes: "إدخال ومراجعة القيود اليومية والحسابات الختامية والضريبية",
            AccountCode: "2141-06",
            LinkedUserId: 3,
            CommissionRate: 0m
        ),

        // ── 5: أمين مخزن رئيسي ──
        new EmployeeDto(
            Id: 3,
            Code: "103",
            Name: "خالد صالح القاضي",
            JobTitle: "أمين مخزن رئيسي",
            Phone: "711456789",
            NationalId: "01010023654",
            BasicSalary: 250000m,
            HireDate: new DateTime(2024, 11, 10),
            IsActive: true,
            Notes: "مسؤول الاستلام والصرف وجرد المخازن",
            AccountCode: "2141-03",
            LinkedUserId: 4,
            CommissionRate: 0m
        ),

        // ── 6: عامل خدمات وحمال ──
        new EmployeeDto(
            Id: 4,
            Code: "104",
            Name: "سامي عبدالملك المنصوري",
            JobTitle: "عامل خدمات وحمال",
            Phone: "777001122",
            NationalId: "01010099887",
            BasicSalary: 120000m,
            HireDate: new DateTime(2025, 6, 1),
            IsActive: true,
            Notes: "عامل خدمات ومسؤول تحميل وتنزيل البضاعة",
            AccountCode: "2141-04",
            LinkedUserId: null,
            CommissionRate: 0m
        ),

        // ── 7: سائق نقل وتوزيع ──
        new EmployeeDto(
            Id: 5,
            Code: "105",
            Name: "ياسر طارق الوجيه",
            JobTitle: "سائق نقل وتوزيع",
            Phone: "735112233",
            NationalId: "01010033445",
            BasicSalary: 150000m,
            HireDate: new DateTime(2025, 8, 20),
            IsActive: true,
            Notes: "سائق دينا التوزيع",
            AccountCode: "2141-05",
            LinkedUserId: null,
            CommissionRate: 0m
        ),

        // ── 8: مستخدم تجريبي مقفل ──
        new EmployeeDto(
            Id: 8,
            Code: "108",
            Name: "مستخدم تجريبي مقفل",
            JobTitle: "كاشير مبيعات",
            Phone: "778888888",
            NationalId: "01010088888",
            BasicSalary: 150000m,
            HireDate: new DateTime(2025, 9, 1),
            IsActive: true,
            Notes: "حساب تجريبي لاختبار القفل التلقائي",
            AccountCode: "2141-08",
            LinkedUserId: 5,
            CommissionRate: 0m
        ),
    };
}
