using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للتحويلات المخزنية — 4 تحويلات واقعية ببيانات عربية.
/// </summary>
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
                new(1, "102030", "شاي ليبتون 250غ", 50m)   { UnitCost = 150m, TotalCost = 7500m, UnitName = "قطعة" },
                new(2, "102031", "زيت نباتي 1 لتر", 30m)   { UnitCost = 80m,  TotalCost = 2400m, UnitName = "زجاجة" },
                new(4, "102033", "سكر أبيض 1 كغ", 40m)     { UnitCost = 200m, TotalCost = 8000m, UnitName = "كيس" }
            })
        {
            FromWarehouseName = "المخزن الرئيسي",
            ToWarehouseName   = "مخزن المعرض",
            DriverName        = "علي حسن",
            VehiclePlate      = "أ ب ج 1234",
            Reference         = "إذن صرف رقم 501",
            TotalCost         = 17900m,
            TotalQuantity     = 120m,
            CreatedByUserId   = 1
        },

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
                new(3, "102032", "أرز بسمتي 5 كغ", 100m)  { UnitCost = 60m,  TotalCost = 6000m, UnitName = "كيس" },
                new(5, "102034", "دقيق فاخر 2 كغ", 80m)    { UnitCost = 120m, TotalCost = 9600m, UnitName = "كيس" },
                new(6, "102035", "معجون طماطم 400غ", 60m)  { UnitCost = 90m,  TotalCost = 5400m, UnitName = "علبة" },
                new(8, "102037", "حليب مجفف 900غ", 25m)    { UnitCost = 30m,  TotalCost = 750m,  UnitName = "علبة" }
            })
        {
            FromWarehouseName = "المخزن الرئيسي",
            ToWarehouseName   = "مخزن الفرع - عدن",
            DriverName        = "محمد سالم",
            VehiclePlate      = "ع د ن 5678",
            Reference         = null,
            TotalCost         = 21750m,
            TotalQuantity     = 265m,
            CreatedByUserId   = 1
        },

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
                new(7, "102036", "تونا معلبة 185غ", 15m)  { UnitCost = 45m,  TotalCost = 675m, UnitName = "علبة" },
                new(9, "102038", "مناديل ورقية 150 ورقة", 20m) { UnitCost = 200m, TotalCost = 4000m, UnitName = "علبة" }
            })
        {
            FromWarehouseName = "مخزن المعرض",
            ToWarehouseName   = "المخزن الرئيسي",
            DriverName        = "علي حسن",
            VehiclePlate      = "أ ب ج 1234",
            Reference         = "إذن استلام رقم 302",
            TotalCost         = 4675m,
            TotalQuantity     = 35m,
            CreatedByUserId   = 2
        },

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
                new(10, "102039", "صابون غسيل 1 كغ", 45m) { UnitCost = 75m, TotalCost = 3375m, UnitName = "كيس" }
            })
        {
            FromWarehouseName = "المخزن الرئيسي",
            ToWarehouseName   = "مخزن المعرض",
            DriverName        = null,
            VehiclePlate      = null,
            Reference         = null,
            TotalCost         = 3375m,
            TotalQuantity     = 45m,
            CreatedByUserId   = 1
        }
    };
}
