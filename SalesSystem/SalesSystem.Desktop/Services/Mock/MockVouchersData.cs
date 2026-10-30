using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لسندات القبض والصرف — مستوحاة من INITIAL_Vouchers في واجهة الويب.
/// </summary>
internal static class MockVouchersData
{
    // ═══════════════════════════════════════════════
    // سندات القبض — Receipt Vouchers
    // ═══════════════════════════════════════════════
    public static List<ReceiptVoucherDto> Receipts { get; } = new()
    {
        new ReceiptVoucherDto(
            Id: 1,
            VoucherNo: 1001,
            VoucherDate: new DateTime(2026, 9, 1),
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            AccountId: 102,
            AccountName: "العملاء والذمم المدينة",
            TotalAmount: 45000m,
            Notes: "تحصيل دفعة نقدية بموجب إيصال استلام من مؤسسة النور",
            Status: 2, // Posted
            CreatedAt: new DateTime(2026, 9, 1, 9, 30, 0),
            PostedAt: new DateTime(2026, 9, 1, 9, 35, 0),
            CancelledAt: null // TODO: PaymentMethod, Currency, PartyName, ContraAccountCode, ContraAccountName, Reference, CheckNumber, BankName, CostCenterCode, CostCenterName removed from DTO
        ),
        new ReceiptVoucherDto(
            Id: 2,
            VoucherNo: 1002,
            VoucherDate: new DateTime(2026, 9, 2),
            CashBoxId: 2,
            CashBoxName: "بنك التضامن الإسلامي (حساب جاري)",
            AccountId: 103,
            AccountName: "العملاء والذمم المدينة",
            TotalAmount: 120000m,
            Notes: "تحويل بنكي مباشر لحساب المؤسسة سداداً لفاتورة بيع جملة",
            Status: 2, // Posted
            CreatedAt: new DateTime(2026, 9, 2, 11, 15, 0),
            PostedAt: new DateTime(2026, 9, 2, 11, 20, 0),
            CancelledAt: null // TODO: PaymentMethod, Currency, PartyName, ContraAccountCode, ContraAccountName, Reference, CheckNumber, BankName, CostCenterCode, CostCenterName removed from DTO
        )
    };

    // ═══════════════════════════════════════════════
    // سندات الصرف — Payment Vouchers
    // ═══════════════════════════════════════════════
    public static List<PaymentVoucherDto> Payments { get; } = new()
    {
        new PaymentVoucherDto(
            Id: 1,
            VoucherNo: 2001,
            VoucherDate: new DateTime(2026, 9, 1),
            CashBoxId: 2,
            CashBoxName: "بنك التضامن الإسلامي (حساب جاري)",
            AccountId: 201,
            AccountName: "الموردون وحسابات الدائنين",
            TotalAmount: 200000m,
            Notes: "سداد شيك بنكي لتوريد شاي كبوس فرط وكراتين",
            Status: 2, // Posted
            CreatedAt: new DateTime(2026, 9, 1, 12, 40, 0),
            PostedAt: new DateTime(2026, 9, 1, 12, 45, 0),
            CancelledAt: null // TODO: PaymentMethod, Currency, PartyName, ContraAccountCode, ContraAccountName, Reference, CheckNumber, BankName, CostCenterCode, CostCenterName removed from DTO
        ),
        new PaymentVoucherDto(
            Id: 2,
            VoucherNo: 2002,
            VoucherDate: new DateTime(2026, 9, 1),
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            AccountId: 421,
            AccountName: "مصروفات الإيجار والكهرباء والمياه",
            TotalAmount: 75000m,
            Notes: "صرف إيجار فرع حدة وفرع التحرير التجاري لشهر سبتمبر",
            Status: 2, // Posted
            CreatedAt: new DateTime(2026, 9, 1, 14, 0, 0),
            PostedAt: new DateTime(2026, 9, 1, 14, 5, 0),
            CancelledAt: null // TODO: PaymentMethod, Currency, PartyName, ContraAccountCode, ContraAccountName, Reference, CheckNumber, BankName, CostCenterCode, CostCenterName removed from DTO
        ),
        new PaymentVoucherDto(
            Id: 3,
            VoucherNo: 2003,
            VoucherDate: new DateTime(2026, 9, 2),
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            AccountId: 423,
            AccountName: "مصروفات الصيانة وضيافة المحل",
            TotalAmount: 8000m,
            Notes: "صرف تكلفة صيانة خوادم وشبكة كاشيرات نقاط البيع",
            Status: 2, // Posted
            CreatedAt: new DateTime(2026, 9, 2, 16, 30, 0),
            PostedAt: new DateTime(2026, 9, 2, 16, 35, 0),
            CancelledAt: null // TODO: PaymentMethod, Currency, PartyName, ContraAccountCode, ContraAccountName, Reference, CheckNumber, BankName, CostCenterCode, CostCenterName removed from DTO
        )
    };
}
