using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للمصروفات — 6 مصروفات واقعية ببيانات عربية.
/// </summary>
internal static class MockExpensesData
{
    public static List<ExpenseDto> Expenses { get; } = new()
    {
        // ── 1: إيجار المحل — مرحّل ──
        new ExpenseDto(
            Id: 1,
            ExpenseNo: 1001,
            ExpenseDate: new DateTime(2026, 9, 1),
            ExpenseAccountId: 33,
            ExpenseAccountName: "مصروفات الإيجار والكهرباء والمياه",
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            Amount: 150000m,
            Notes: "إيجار شهر سبتمبر 2026 — محل شارع الستين",
            Status: 2,
            StatusName: "مرحّل",
            IsActive: true // TODO: Currency, ExpenseAccountCode, BranchName removed from DTO
        ),

        // ── 2: كهرباء وמים — مرحّل ──
        new ExpenseDto(
            Id: 2,
            ExpenseNo: 1002,
            ExpenseDate: new DateTime(2026, 9, 5),
            ExpenseAccountId: 33,
            ExpenseAccountName: "مصروفات الإيجار والكهرباء والمياه",
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            Amount: 35000m,
            Notes: "فاتورة كهرباء شهر أغسطس 2026 — شركة كهرباء صنعاء",
            Status: 2,
            StatusName: "مرحّل",
            IsActive: true // TODO: Currency, ExpenseAccountCode, BranchName removed from DTO
        ),

        // ── 3: رواتب الموظفين — مرحّل ──
        new ExpenseDto(
            Id: 3,
            ExpenseNo: 1003,
            ExpenseDate: new DateTime(2026, 9, 10),
            ExpenseAccountId: 34,
            ExpenseAccountName: "رواتب وأجور الموظفين والعمال",
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            Amount: 250000m,
            Notes: "رواتب 4 موظفين — شهر أغسطس 2026",
            Status: 2,
            StatusName: "مرحّل",
            IsActive: true // TODO: Currency, ExpenseAccountCode, BranchName removed from DTO
        ),

        // ── 4: صيانة ثلاجات العرض — مسودة ──
        new ExpenseDto(
            Id: 4,
            ExpenseNo: 1004,
            ExpenseDate: new DateTime(2026, 9, 18),
            ExpenseAccountId: 35,
            ExpenseAccountName: "مصروفات الصيانة وضيافة المحل",
            CashBoxId: 4,
            CashBoxName: "صندوق عهدة النثريات والمصروفات اليومية",
            Amount: 18000m,
            Notes: "صيانة دورية لثلاجة عرض الألبان — فني أحمد",
            Status: 1,
            StatusName: "مسودة",
            IsActive: true // TODO: Currency, ExpenseAccountCode, BranchName removed from DTO
        ),

        // ── 5: نقل بضاعة من المخزن للمعرض — مسودة ──
        new ExpenseDto(
            Id: 5,
            ExpenseNo: 1005,
            ExpenseDate: new DateTime(2026, 9, 20),
            ExpenseAccountId: 36,
            ExpenseAccountName: "مصروفات أجور النقل والمواصلات",
            CashBoxId: 4,
            CashBoxName: "صندوق عهدة النثريات والمصروفات اليومية",
            Amount: 12000m,
            Notes: "أجرة نقل بضاعة من المخزن الرئيسي إلى مخزن المعرض",
            Status: 1,
            StatusName: "مسودة",
            IsActive: true // TODO: Currency, ExpenseAccountCode, BranchName removed from DTO
        ),

        // ── 6: مشتريات نثرية (قرطاسية ومواد تنظيف) — ملغية ──
        new ExpenseDto(
            Id: 6,
            ExpenseNo: 1006,
            ExpenseDate: new DateTime(2026, 9, 15),
            ExpenseAccountId: 39,
            ExpenseAccountName: "مصروفات نثرية وتشغيلية أخرى",
            CashBoxId: 4,
            CashBoxName: "صندوق عهدة النثريات والمصروفات اليومية",
            Amount: 5500m,
            Notes: "قرطاسية ومواد تنظيف — تم إلغاؤها بسبب تكرار الطلب",
            Status: 3,
            StatusName: "ملغي",
            IsActive: false // TODO: Currency, ExpenseAccountCode, BranchName removed from DTO
        )
    };
}
