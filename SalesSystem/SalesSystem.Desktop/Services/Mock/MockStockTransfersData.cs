using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للتحويلات المخزنية — 4 تحويلات واقعية ببيانات عربية.
/// </summary>
/// <remarks>
/// TODO: الحقول التالية أُزيلت من DTOs التحويلات ولم تُعد موجودة:
/// StockTransferDto → FromWarehouseName, ToWarehouseName, DriverName, VehiclePlate,
///                    Reference, TotalCost, TotalQuantity, CreatedByUserId
/// StockTransferItemDto → UnitCost, TotalCost, UnitName
/// (تُحسب هذه القيم الآن في طبقة الأعمال، وليست جزءاً من نموذج العرض.)
/// </remarks>
internal static class MockStockTransfersData
{
    public static List<StockTransferDto> Transfers { get; } = new()
    {
        // ── 1: تحويل مرحّل — المخزن الرئيسي → مخزن المعرض ──
        new StockTransferDto(
            Id: 1,
            TransferNo: "1",
            TransferDate: new DateTime(2026, 9, 10),
            FromWarehouseId: 1,
            ToWarehouseId: 2,
            Notes: "تجديد بضاعة المعرض — موسم سبتمبر",
            Status: 2,
            Items: new List<StockTransferItemDto>
            {
                new(1, "102030", "شاي ليبتون 250غ", 50m),
                new(2, "102031", "زيت نباتي 1 لتر", 30m),
                new(4, "102033", "سكر أبيض 1 كغ", 40m)
            }),

        // ── 2: تحويل مسودة — المخزن الرئيسي → مخزن الفرع عدن ──
        new StockTransferDto(
            Id: 2,
            TransferNo: "2",
            TransferDate: new DateTime(2026, 9, 15),
            FromWarehouseId: 1,
            ToWarehouseId: 3,
            Notes: "إمداد فرع عدن — طلبية شهر سبتمبر",
            Status: 1,
            Items: new List<StockTransferItemDto>
            {
                new(3, "102032", "أرز بسمتي 5 كغ", 100m),
                new(5, "102034", "دقيق فاخر 2 كغ", 80m),
                new(6, "102035", "معجون طماطم 400غ", 60m),
                new(8, "102037", "حليب مجفف 900غ", 25m)
            }),

        // ── 3: تحويل مرحل — مخزن المعرض → المخزن الرئيسي (إعادة) ──
        new StockTransferDto(
            Id: 3,
            TransferNo: "3",
            TransferDate: new DateTime(2026, 9, 18),
            FromWarehouseId: 2,
            ToWarehouseId: 1,
            Notes: "إعادة بضاعة غير مبيعة من المعرض للمخزن الرئيسي",
            Status: 2,
            Items: new List<StockTransferItemDto>
            {
                new(7, "102036", "تونا معلبة 185غ", 15m),
                new(9, "102038", "مناديل ورقية 150 ورقة", 20m)
            }),

        // ── 4: تحويل ملغية — المخزن الرئيسي → مخزن المعرض ──
        new StockTransferDto(
            Id: 4,
            TransferNo: "4",
            TransferDate: new DateTime(2026, 9, 20),
            FromWarehouseId: 1,
            ToWarehouseId: 2,
            Notes: "تم الإلغاء بسبب تلف البضاعة أثناء التحميل",
            Status: 3,
            Items: new List<StockTransferItemDto>
            {
                new(10, "102039", "صابون غسيل 1 كغ", 45m)
            })
    };
}
