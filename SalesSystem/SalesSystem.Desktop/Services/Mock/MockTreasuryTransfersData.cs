using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لتحويلات الخزينة — مستوحاة من CashVoucher type TRANSFER في واجهة الويب.
/// أرقام التحويلات تبدأ من 3001.
/// </summary>
internal static class MockTreasuryTransfersData
{
    public static List<TreasuryTransferDto> Transfers { get; } = new()
    {
        // ── 1: تحويل مرحّل — درج كاشير 1 → الصندوق الرئيسي ──
        new TreasuryTransferDto(
            Id: 1,
            TransferNo: 3001,
            TransferDate: new DateTime(2026, 9, 15),
            FromCashBoxId: 2,
            FromCashBoxName: "درج كاشير نقطة البيع 1 (الصالة)",
            ToCashBoxId: 1,
            ToCashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            FromAccountCode: "111101",
            ToAccountCode: "1111",
            Amount: 180000m,
            Notes: "تحصيل إيرادات يوم الأحد — تسليم الكاشير للخزينة المركزية",
            Status: 2
        ),

        // ── 2: تحويل مرحّل — بنك التضامن → الصندوق الرئيسي ──
        new TreasuryTransferDto(
            Id: 2,
            TransferNo: 3002,
            TransferDate: new DateTime(2026, 9, 18),
            FromCashBoxId: null,
            FromCashBoxName: "بنك التضامن الإسلامي (حساب جاري رئيسي)",
            ToCashBoxId: 1,
            ToCashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            FromAccountCode: "111201",
            ToAccountCode: "1111",
            Amount: 500000m,
            Notes: "سحب نقدي من بنك التضامن لدعم السيولة في الخزينة — مصروفات يومية",
            Status: 2
        ),

        // ── 3: تحويل مسودة — الصندوق الرئيسي → درج كاشير 2 ──
        new TreasuryTransferDto(
            Id: 3,
            TransferNo: 3003,
            TransferDate: new DateTime(2026, 9, 20),
            FromCashBoxId: 1,
            FromCashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            ToCashBoxId: 3,
            ToCashBoxName: "درج كاشير نقطة البيع 2 (سريع)",
            FromAccountCode: "1111",
            ToAccountCode: "111102",
            Amount: 100000m,
            Notes: "تزوية درج كاشير الصالة السريعة — بداية الوردية المسائية",
            Status: 1
        ),

        // ── 4: تحويل ملغية — الصندوق الرئيسي → محفظة جيب ──
        new TreasuryTransferDto(
            Id: 4,
            TransferNo: 3004,
            TransferDate: new DateTime(2026, 9, 21),
            FromCashBoxId: 1,
            FromCashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            ToCashBoxId: null,
            ToCashBoxName: "محفظة جيب الإلكترونية (Jaib Wallet)",
            FromAccountCode: "1111",
            ToAccountCode: "111203",
            Amount: 75000m,
            Notes: "تم الإلغاء — خطأ في المبلغ المطلوب، يُعاد الإدخال",
            Status: 3
        )
    };
}
