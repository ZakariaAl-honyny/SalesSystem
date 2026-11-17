using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للقيود اليومية — تُستخدم بواسطة MockJournalEntryDesktopService.
/// 4 قيود متوازنة مع بنودها (EntryNo 101..104) مأخوذة من INITIAL_JOURNAL_ENTRIES.
/// </summary>
/// <remarks>
/// الالتزام: التسمية العربية لنوع القيد <c>EntryType</c> تُشتق دائماً من
/// <see cref="JournalEntryTypeDisplay.GetArabicName"/> انطلاقاً من القيمة الرقمية
/// <c>EntryTypeValue</c> — فالقيمة الرقمية هي المصدر ولا يمكن أن تنفصل عن التسمية.
/// </remarks>
internal static class MockJournalEntriesData
{
    // ═══════════════════════════════════════════════
    // بنود القيود — Journal Entry Lines
    // ═══════════════════════════════════════════════

    // ⚠️ قاعدة ربط البنود بالدليل: `AccountId` = معرّف الحساب في `MockAccountsData.Accounts`،
    // و`AccountCode` = كود نفس الحساب حرفياً — والاثنان يجب أن يطابقا اسم البند العربي.
    // لا يُقيَّد بند على حساب مجموعة (مثل `1101`) بل على **الورقة** التي يسمّيها البند (`11010001`).
    private static List<JournalEntryLineDetailDto> Lines1 => new()
    {
        new JournalEntryLineDetailDto(1, 4, "الصندوق الرئيسي (الخزينة المركزية)", "11010001", 500000m, 0m, "رصيد افتتاحي كاش"),
        new JournalEntryLineDetailDto(2, 22, "رأس المال المدفوع", "2201", 0m, 500000m, "إيداع من رأس المال"),
    };

    private static List<JournalEntryLineDetailDto> Lines2 => new()
    {
        new JournalEntryLineDetailDto(3, 18, "حسابات الموردين والدائنون", "2101", 200000m, 0m, "سداد دفعة من حساب هائل سعيد"),
        new JournalEntryLineDetailDto(4, 8, "حساب بنك الكريمي للتمويل الأصغر (YER)", "11020001", 0m, 200000m, "تحويل عبر تطبيق الكريمي المميز"),
    };

    private static List<JournalEntryLineDetailDto> Lines3 => new()
    {
        new JournalEntryLineDetailDto(5, 33, "مصروفات الإيجار والكهرباء والمياه", "4201", 45000m, 0m, "إيجار فرع حدة لشهر سبتمبر"),
        new JournalEntryLineDetailDto(6, 33, "مصروفات الإيجار والكهرباء والمياه", "4201", 30000m, 0m, "إيجار فرع التحرير لشهر سبتمبر"),
        new JournalEntryLineDetailDto(7, 4, "الصندوق الرئيسي (الخزينة المركزية)", "11010001", 0m, 75000m, "صرف كاش بموجب سند صرف رقم 2002"),
    };

    private static List<JournalEntryLineDetailDto> Lines4 => new()
    {
        new JournalEntryLineDetailDto(8, 35, "مصروفات الصيانة وضيافة المحل", "4203", 8000m, 0m, "صيانة سيرفرات وشبكة POS الفرعية"),
        new JournalEntryLineDetailDto(9, 4, "الصندوق الرئيسي (الخزينة المركزية)", "11010001", 0m, 8000m, "سداد نقدي لمهندس الصيانة"),
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
            EntryType: JournalEntryTypeDisplay.GetArabicName(JournalEntryType.OpeningBalance),
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "افتتاح الخزينة والوردية الأولى",
            TotalDebit: 500000m,
            TotalCredit: 500000m,
            Status: 2,
            StatusDisplay: "مرحل",
            CreatedAt: new DateTime(2026, 8, 30),
            CreatedByUserName: "مدير النظام",
            EntryTypeValue: JournalEntryType.OpeningBalance
        ),

        new JournalEntryListDto(
            Id: 2,
            EntryNumber: "102",
            EntryDate: new DateTime(2026, 8, 31),
            Description: "تحويل بنكي من حساب الكريمي لحساب المورد هائل سعيد",
            EntryType: JournalEntryTypeDisplay.GetArabicName(JournalEntryType.SupplierPayment),
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "سداد دفعة للمورد هائل سعيد",
            TotalDebit: 200000m,
            TotalCredit: 200000m,
            Status: 2,
            StatusDisplay: "مرحل",
            CreatedAt: new DateTime(2026, 8, 31),
            CreatedByUserName: "مدير النظام",
            EntryTypeValue: JournalEntryType.SupplierPayment
        ),

        new JournalEntryListDto(
            Id: 3,
            EntryNumber: "103",
            EntryDate: new DateTime(2026, 9, 1),
            Description: "صرف إيجار فرع حدة وفرع التحرير نقداً محمل على مراكز التكلفة",
            EntryType: JournalEntryTypeDisplay.GetArabicName(JournalEntryType.Expense),
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "سداد إيجار وكهرباء الفروع التشغيلية",
            TotalDebit: 75000m,
            TotalCredit: 75000m,
            Status: 2,
            StatusDisplay: "مرحل",
            CreatedAt: new DateTime(2026, 9, 1),
            CreatedByUserName: "مدير النظام",
            EntryTypeValue: JournalEntryType.Expense
        ),

        new JournalEntryListDto(
            Id: 4,
            EntryNumber: "104",
            EntryDate: new DateTime(2026, 9, 2),
            Description: "فاتورة صيانة وتحديث أجهزة IT محملة على مركز تقنية المعلومات",
            EntryType: JournalEntryTypeDisplay.GetArabicName(JournalEntryType.Expense),
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "صيانة خوادم وشبكات نقاط البيع",
            TotalDebit: 8000m,
            TotalCredit: 8000m,
            Status: 2,
            StatusDisplay: "مرحل",
            CreatedAt: new DateTime(2026, 9, 2),
            CreatedByUserName: "مدير النظام",
            EntryTypeValue: JournalEntryType.Expense
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
            EntryType: JournalEntryTypeDisplay.GetArabicName(JournalEntryType.OpeningBalance),
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "افتتاح الخزينة والوردية الأولى",
            Status: 2,
            StatusDisplay: "مرحل",
            ReversedByEntryId: null,
            CreatedAt: new DateTime(2026, 8, 30),
            CreatedByUserName: "مدير النظام",
            Lines: Lines1,
            EntryTypeValue: JournalEntryType.OpeningBalance
        ),

        new JournalEntryDetailDto(
            Id: 2,
            EntryNumber: "102",
            EntryDate: new DateTime(2026, 8, 31),
            Description: "تحويل بنكي من حساب الكريمي لحساب المورد هائل سعيد",
            EntryType: JournalEntryTypeDisplay.GetArabicName(JournalEntryType.SupplierPayment),
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "سداد دفعة للمورد هائل سعيد",
            Status: 2,
            StatusDisplay: "مرحل",
            ReversedByEntryId: null,
            CreatedAt: new DateTime(2026, 8, 31),
            CreatedByUserName: "مدير النظام",
            Lines: Lines2,
            EntryTypeValue: JournalEntryType.SupplierPayment
        ),

        new JournalEntryDetailDto(
            Id: 3,
            EntryNumber: "103",
            EntryDate: new DateTime(2026, 9, 1),
            Description: "صرف إيجار فرع حدة وفرع التحرير نقداً محمل على مراكز التكلفة",
            EntryType: JournalEntryTypeDisplay.GetArabicName(JournalEntryType.Expense),
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "سداد إيجار وكهرباء الفروع التشغيلية",
            Status: 2,
            StatusDisplay: "مرحل",
            ReversedByEntryId: null,
            CreatedAt: new DateTime(2026, 9, 1),
            CreatedByUserName: "مدير النظام",
            Lines: Lines3,
            EntryTypeValue: JournalEntryType.Expense
        ),

        new JournalEntryDetailDto(
            Id: 4,
            EntryNumber: "104",
            EntryDate: new DateTime(2026, 9, 2),
            Description: "فاتورة صيانة وتحديث أجهزة IT محملة على مركز تقنية المعلومات",
            EntryType: JournalEntryTypeDisplay.GetArabicName(JournalEntryType.Expense),
            ReferenceType: null,
            ReferenceId: null,
            ReferenceNumber: "صيانة خوادم وشبكات نقاط البيع",
            Status: 2,
            StatusDisplay: "مرحل",
            ReversedByEntryId: null,
            CreatedAt: new DateTime(2026, 9, 2),
            CreatedByUserName: "مدير النظام",
            Lines: Lines4,
            EntryTypeValue: JournalEntryType.Expense
        ),
    };
}
