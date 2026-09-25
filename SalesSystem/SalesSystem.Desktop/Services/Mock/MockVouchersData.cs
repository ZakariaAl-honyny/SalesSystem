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
            CancelledAt: null,
            PaymentMethod: 1, // Cash
            Currency: "YER",
            PartyName: "مؤسسة النور للتجارة والتوزيع",
            ContraAccountCode: "1131",
            ContraAccountName: "العملاء والذمم المدينة",
            Reference: "سداد دفعة نقدية من الحساب الآجل",
            CheckNumber: null,
            BankName: null,
            CostCenterCode: "201",
            CostCenterName: "فرع حدة الرئيسي (صنعاء)"
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
            CancelledAt: null,
            PaymentMethod: 2, // Bank
            Currency: "YER",
            PartyName: "سوبرماركت البركة",
            ContraAccountCode: "1131",
            ContraAccountName: "العملاء والذمم المدينة",
            Reference: "حوالة بنكية رقم TR-88392",
            CheckNumber: null,
            BankName: "بنك التضامن الإسلامي",
            CostCenterCode: "202",
            CostCenterName: "فرع التحرير التجاري (صنعاء)"
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
            CancelledAt: null,
            PaymentMethod: 2, // Bank
            Currency: "YER",
            PartyName: "شركة كبوس للصناعة والتجارة",
            ContraAccountCode: "2111",
            ContraAccountName: "الموردون وحسابات الدائنين",
            Reference: "شيك بنكي رقم 440912",
            CheckNumber: "440912",
            BankName: "بنك التضامن الإسلامي",
            CostCenterCode: "301",
            CostCenterName: "مستودع صنعاء المركزي والشحن"
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
            CancelledAt: null,
            PaymentMethod: 1, // Cash
            Currency: "YER",
            PartyName: "مالك عقار فروع حدة والتحرير",
            ContraAccountCode: "421",
            ContraAccountName: "مصروفات الإيجار والكهرباء والمياه",
            Reference: "إيصال إيجار رقم RNT-09",
            CheckNumber: null,
            BankName: null,
            CostCenterCode: "201",
            CostCenterName: "فرع حدة الرئيسي (صنعاء)"
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
            CancelledAt: null,
            PaymentMethod: 1, // Cash
            Currency: "YER",
            PartyName: "ورشة التقنية لخدمات الشبكات",
            ContraAccountCode: "423",
            ContraAccountName: "مصروفات الصيانة وضيافة المحل",
            Reference: "فاتورة صيانة IT-77",
            CheckNumber: null,
            BankName: null,
            CostCenterCode: "102",
            CostCenterName: "إدارة تكنولوجيا المعلومات ونقاط البيع IT"
        )
    };
}
