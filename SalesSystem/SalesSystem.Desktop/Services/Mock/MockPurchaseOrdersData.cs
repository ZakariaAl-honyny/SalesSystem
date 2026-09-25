using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لأوامر الشراء — 3 أوامر بأرقام 8001-8003.
/// </summary>
internal static class MockPurchaseOrdersData
{
    private static List<PurchaseOrderDto> _orders = CreateSeedOrders();
    public static List<PurchaseOrderDto> Orders => _orders;

    private static List<PurchaseOrderDto> CreateSeedOrders()
    {
        DateTime today = DateTime.Today;

        return new List<PurchaseOrderDto>
        {
            // ═══ أمر 8001: مسودة — مجموعة هائل سعيد ═══
            new PurchaseOrderDto(
                Id: 1,
                OrderNo: 8001,
                OrderDate: today.AddDays(-4),
                SupplierId: 1,
                SupplierName: "مجموعة هائل سعيد أنعم وشركاه",
                WarehouseId: 1,
                WarehouseName: "المخزن الرئيسي",
                SubTotal: 105000m,
                DiscountAmount: 0m,
                TaxAmount: 0m,
                TotalAmount: 105000m,
                Notes: "طلب توريد زيوت ومواد غذائية",
                Status: 1, // مسودة
                ConvertedToPurchaseInvoiceId: null,
                Items: new List<PurchaseOrderLineDto>
                {
                    new PurchaseOrderLineDto(Id: 1, ProductId: 2, ProductName: "زيت نباتي 1 لتر",   Quantity: 40m, UnitPrice: 1500m, LineTotal: 60000m),
                    new PurchaseOrderLineDto(Id: 2, ProductId: 1, ProductName: "شاي ليبتون 250غ",  Quantity: 30m, UnitPrice: 500m,  LineTotal: 15000m),
                    new PurchaseOrderLineDto(Id: 3, ProductId: 4, ProductName: "سكر أبيض 1 كغ",   Quantity: 60m, UnitPrice: 500m,  LineTotal: 30000m)
                }),

            // ═══ أمر 8002: مرسل — شركة مياه حدة ═══
            new PurchaseOrderDto(
                Id: 2,
                OrderNo: 8002,
                OrderDate: today.AddDays(-2),
                SupplierId: 2,
                SupplierName: "شركة مياه ومشروبات حدة المحدودة",
                WarehouseId: 2,
                WarehouseName: "مخزن المعرض",
                SubTotal: 28000m,
                DiscountAmount: 2000m,
                TaxAmount: 0m,
                TotalAmount: 26000m,
                Notes: "طلب مياه ومعصيرات للصيف",
                Status: 2, // مرسل
                ConvertedToPurchaseInvoiceId: null,
                Items: new List<PurchaseOrderLineDto>
                {
                    new PurchaseOrderLineDto(Id: 4, ProductId: 6, ProductName: "معجون طماطم 400غ", Quantity: 100m, UnitPrice: 200m, LineTotal: 20000m),
                    new PurchaseOrderLineDto(Id: 5, ProductId: 3, ProductName: "أرز بسمتي 5 كغ",  Quantity: 2m,   UnitPrice: 4000m, LineTotal: 8000m)
                }),

            // ═══ أمر 8003: مستلم — مؤسسة التضامن ═══
            new PurchaseOrderDto(
                Id: 3,
                OrderNo: 8003,
                OrderDate: today.AddDays(-12),
                SupplierId: 3,
                SupplierName: "مؤسسة التضامن لتجارة المواد الغذائية",
                WarehouseId: 1,
                WarehouseName: "المخزن الرئيسي",
                SubTotal: 162000m,
                DiscountAmount: 0m,
                TaxAmount: 0m,
                TotalAmount: 162000m,
                Notes: "توريد أرز وبقوليات — تم الاستلام",
                Status: 3, // مستلم
                ConvertedToPurchaseInvoiceId: 3,
                Items: new List<PurchaseOrderLineDto>
                {
                    new PurchaseOrderLineDto(Id: 6, ProductId: 3, ProductName: "أرز بسمتي 5 كغ",   Quantity: 30m, UnitPrice: 3500m, LineTotal: 105000m),
                    new PurchaseOrderLineDto(Id: 7, ProductId: 5, ProductName: "دقيق فاخر 2 كغ",   Quantity: 20m, UnitPrice: 1200m, LineTotal: 24000m),
                    new PurchaseOrderLineDto(Id: 8, ProductId: 10, ProductName: "صابون غسيل 1 كغ",  Quantity: 50m, UnitPrice: 660m,  LineTotal: 33000m)
                })
        };
    }
}
