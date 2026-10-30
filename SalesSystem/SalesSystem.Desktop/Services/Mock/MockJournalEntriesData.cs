using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للقيود اليومية — تُستخدم بواسطة MockJournalEntryApiService.
/// 4 قيود متوازنة مع بنودها (EntryNo 101..104) مأخوذة من INITIAL_JOURNAL_ENTRIES.
/// </summary>
/// <remarks>
/// TODO: الحقول التالية أُزيلت من DTOs القيود ولم تُعد موجودة:
/// JournalEntryListDto / JournalEntryDetailDto → SourceModule
/// JournalEntryLineDetailDto → CostCenterCode, CostCenterName
/// (مراكز التكلفة المحذوفة كانت: 201 فرع حدة، 202 فرع التحرير، 102 إدارة IT.)
/// </remarks>
internal static class MockJournalEntriesData
{
    // ═══════════════════════════════════════════════
    // بنود القيود — Journal Entry Lines
    // ═══════════════════════════════════════════════

    private static List<JournalEntryLineDetailDto> Lines1 => new()
    {
        new JournalEntryLineDetailDto(1, 1, "الصندوق الرئيسي (الخزينة المركزية)", "1111", 500000m, 0m, "رصيد افتتاحي كاش"),
        new JournalEntryLineDetailDto(2, 2, "رأس المال المدفوع", "221", 0m, 500000m, "إيداع من رأس المال"),
    };

    private static List<JournalEntryLineDetailDto> Lines2 => new()
    {
        new JournalEntryLineDetailDto(3, 3, "حسابات الموردين والدائنون", "211", 200000m, 0m, "سداد دفعة من حساب هائل سعيد"),
        new JournalEntryLineDetailDto(4, 4, "حساب بنك الكريمي للتمويل الأصغر (YER)", "1121", 0m, 200000m, "تحويل عبر تطبيق الكريمي المميز"),
    };

    private static List<JournalEntryLineDetailDto> Lines3 => new()
    {
        new JournalEntryLineDetailDto(5, 5, "مصروفات الإيجار والكهرباء والمياه", "421", 45000m, 0m, "إيجار فرع حدة لشهر سبتمبر"),
        new JournalEntryLineDetailDto(6, 5, "مصروفات الإيجار والكهرباء والمياه", "421", 30000m, 0m, "إيجار فرع التحرير لشهر سبتمبر"),
        new JournalEntryLineDetailDto(7, 1, "الصندوق الرئيسي (الخزينة المركزية)", "1111", 0m, 75000m, "صرف كاش بموجب سند صرف رقم 2002"),
    };

    private static List<JournalEntryLineDetailDto> Lines4 => new()
    {
        new JournalEntryLineDetailDto(8, 6, "مصروفات الصيانة وضيافة المحل", "423", 8000m, 0m, "صيانة سيرفرات وشبكة POS الفرعية"),
        new JournalEntryLineDetailDto(9, 1, "الصندوق الرئيسي (الخزينة المركزية)", "1111", 0m, 8000m, "سداد نقدي لمهندس الصيانة"),
    };

    // ═══════════════════════════════════════════════
    // القيود اليومية — Journal Entries (Headers)
    // ═══════════════════════════════════════════════

    public static List<JournalEntryListDto> Entries { get; } = new()
    {
        new JournalEntryListDto(
            Id: 1,
            EntryNumber: "101",
            EntryDate: new DateTime(2026, 8, 30),
            Description: "قيد إيداع الرصيد الافتتاحي في الصندوق الرئيسي",
            EntryType: "رصيد افتتاحي",
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "افتتاح الخزينة والوردية الأولى",
            TotalDebit: 500000m,
            TotalCredit: 500000m,
            Status: 2,
            StatusDisplay: "مرحل",
            CreatedAt: new DateTime(2026, 8, 30),
            CreatedByUserId: 1
        ),

        new JournalEntryListDto(
            Id: 2,
            EntryNumber: "102",
            EntryDate: new DateTime(2026, 8, 31),
            Description: "تحويل بنكي من حساب الكريمي لحساب المورد هائل سعيد",
            EntryType: "مدفوعات",
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "سداد دفعة للمورد هائل سعيد",
            TotalDebit: 200000m,
            TotalCredit: 200000m,
            Status: 2,
            StatusDisplay: "مرحل",
            CreatedAt: new DateTime(2026, 8, 31),
            CreatedByUserId: 1
        ),

        new JournalEntryListDto(
            Id: 3,
            EntryNumber: "103",
            EntryDate: new DateTime(2026, 9, 1),
            Description: "صرف إيجار فرع حدة وفرع التحرير نقداً محمل على مراكز التكلفة",
            EntryType: "مصروف",
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "سداد إيجار وكهرباء الفروع التشغيلية",
            TotalDebit: 75000m,
            TotalCredit: 75000m,
            Status: 2,
            StatusDisplay: "مرحل",
            CreatedAt: new DateTime(2026, 9, 1),
            CreatedByUserId: 1
        ),

        new JournalEntryListDto(
            Id: 4,
            EntryNumber: "104",
            EntryDate: new DateTime(2026, 9, 2),
            Description: "فاتورة صيانة وتحديث أجهزة IT محملة على مركز تقنية المعلومات",
            EntryType: "مصروف",
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "صيانة خوادم وشبكات نقاط البيع",
            TotalDebit: 8000m,
            TotalCredit: 8000m,
            Status: 2,
            StatusDisplay: "مرحل",
            CreatedAt: new DateTime(2026, 9, 2),
            CreatedByUserId: 1
        ),
    };

    // ═══════════════════════════════════════════════
    // القيود مع البنود — Detail DTOs
    // ═══════════════════════════════════════════════

    public static List<JournalEntryDetailDto> EntryDetails { get; } = new()
    {
        new JournalEntryDetailDto(
            Id: 1,
            EntryNumber: "101",
            EntryDate: new DateTime(2026, 8, 30),
            Description: "قيد إيداع الرصيد الافتتاحي في الصندوق الرئيسي",
            EntryType: "رصيد افتتاحي",
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "افتتاح الخزينة والوردية الأولى",
            Status: 2,
            StatusDisplay: "مرحل",
            ReversedByEntryId: null,
            CreatedAt: new DateTime(2026, 8, 30),
            CreatedByUserId: 1,
            Lines: Lines1
        ),

        new JournalEntryDetailDto(
            Id: 2,
            EntryNumber: "102",
            EntryDate: new DateTime(2026, 8, 31),
            Description: "تحويل بنكي من حساب الكريمي لحساب المورد هائل سعيد",
            EntryType: "مدفوعات",
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "سداد دفعة للمورد هائل سعيد",
            Status: 2,
            StatusDisplay: "مرحل",
            ReversedByEntryId: null,
            CreatedAt: new DateTime(2026, 8, 31),
            CreatedByUserId: 1,
            Lines: Lines2
        ),

        new JournalEntryDetailDto(
            Id: 3,
            EntryNumber: "103",
            EntryDate: new DateTime(2026, 9, 1),
            Description: "صرف إيجار فرع حدة وفرع التحرير نقداً محمل على مراكز التكلفة",
            EntryType: "مصروف",
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "سداد إيجار وكهرباء الفروع التشغيلية",
            Status: 2,
            StatusDisplay: "مرحل",
            ReversedByEntryId: null,
            CreatedAt: new DateTime(2026, 9, 1),
            CreatedByUserId: 1,
            Lines: Lines3
        ),

        new JournalEntryDetailDto(
            Id: 4,
            EntryNumber: "104",
            EntryDate: new DateTime(2026, 9, 2),
            Description: "فاتورة صيانة وتحديث أجهزة IT محملة على مركز تقنية المعلومات",
            EntryType: "مصروف",
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "صيانة خوادم وشبكات نقاط البيع",
            Status: 2,
            StatusDisplay: "مرحل",
            ReversedByEntryId: null,
            CreatedAt: new DateTime(2026, 9, 2),
            CreatedByUserId: 1,
            Lines: Lines4
        ),
    };
}
