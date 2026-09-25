using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لعروض الأسعار — 3 عروض بأرقام 4001-4003.
/// </summary>
internal static class MockQuotationsData
{
    private static List<QuotationDto> _quotations = CreateSeedQuotations();
    public static List<QuotationDto> Quotations => _quotations;

    private static List<QuotationDto> CreateSeedQuotations()
    {
        DateTime today = DateTime.Today;

        return new List<QuotationDto>
        {
            // ═══ عرض 4001: مسودة — أحمد محمد علي ═══
            new QuotationDto(
                Id: 1,
                QuotationNo: 4001,
                QuotationDate: today.AddDays(-5),
                ValidUntil: today.AddDays(25),
                CustomerId: 1,
                CustomerName: "أحمد محمد علي",
                WarehouseId: 1,
                WarehouseName: "المخزن الرئيسي",
                SubTotal: 22500m,
                DiscountAmount: 0m,
                TaxAmount: 0m,
                TotalAmount: 22500m,
                Notes: "عرض سعر لمواد غذائية متنوعة",
                Status: 1, // مسودة
                ConvertedToInvoiceId: null,
                Items: new List<QuotationLineDto>
                {
                    new QuotationLineDto(Id: 1, ProductId: 1, ProductName: "شاي ليبتون 250غ",  Quantity: 20m, UnitPrice: 500m,  DiscountAmount: 0m, LineTotal: 10000m),
                    new QuotationLineDto(Id: 2, ProductId: 2, ProductName: "زيت نباتي 1 لتر",   Quantity: 5m,  UnitPrice: 1500m, DiscountAmount: 0m, LineTotal: 7500m),
                    new QuotationLineDto(Id: 3, ProductId: 4, ProductName: "سكر أبيض 1 كغ",    Quantity: 10m, UnitPrice: 500m,  DiscountAmount: 0m, LineTotal: 5000m)
                }),

            // ═══ عرض 4002: مرسل — شركة النور للتجارة ═══
            new QuotationDto(
                Id: 2,
                QuotationNo: 4002,
                QuotationDate: today.AddDays(-3),
                ValidUntil: today.AddDays(12),
                CustomerId: 2,
                CustomerName: "شركة النور للتجارة",
                WarehouseId: 1,
                WarehouseName: "المخزن الرئيسي",
                SubTotal: 136000m,
                DiscountAmount: 5000m,
                TaxAmount: 0m,
                TotalAmount: 131000m,
                Notes: "عرض سعر جملة — أسعار خاصة",
                Status: 2, // مرسل
                ConvertedToInvoiceId: null,
                Items: new List<QuotationLineDto>
                {
                    new QuotationLineDto(Id: 4, ProductId: 3, ProductName: "أرز بسمتي 5 كغ",     Quantity: 20m,  UnitPrice: 3500m, DiscountAmount: 0m, LineTotal: 70000m),
                    new QuotationLineDto(Id: 5, ProductId: 6, ProductName: "معجون طماطم 400غ",   Quantity: 50m,  UnitPrice: 600m,  DiscountAmount: 0m, LineTotal: 30000m),
                    new QuotationLineDto(Id: 6, ProductId: 8, ProductName: "حليب مجفف 900غ",     Quantity: 8m,   UnitPrice: 4500m, DiscountAmount: 0m, LineTotal: 36000m)
                }),

            // ═══ عرض 4003: مُحوَّل — مؤسسة اليمن الحديثة ═══
            new QuotationDto(
                Id: 3,
                QuotationNo: 4003,
                QuotationDate: today.AddDays(-10),
                ValidUntil: today.AddDays(-1),
                CustomerId: 3,
                CustomerName: "مؤسسة اليمن الحديثة",
                WarehouseId: 2,
                WarehouseName: "مخزن المعرض",
                SubTotal: 68000m,
                DiscountAmount: 3000m,
                TaxAmount: 0m,
                TotalAmount: 65000m,
                Notes: "عرض سعر مُحوَّل إلى فاتورة بيع",
                Status: 4, // مُحوَّل
                ConvertedToInvoiceId: 5,
                Items: new List<QuotationLineDto>
                {
                    new QuotationLineDto(Id: 7, ProductId: 5, ProductName: "دقيق فاخر 2 كغ",     Quantity: 30m, UnitPrice: 1200m, DiscountAmount: 0m, LineTotal: 36000m),
                    new QuotationLineDto(Id: 8, ProductId: 7, ProductName: "تونا معلبة 185غ",    Quantity: 20m, UnitPrice: 1000m, DiscountAmount: 0m, LineTotal: 20000m),
                    new QuotationLineDto(Id: 9, ProductId: 9, ProductName: "مناديل ورقية 150 ورقة", Quantity: 24m, UnitPrice: 500m, DiscountAmount: 0m, LineTotal: 12000m)
                })
        };
    }
}
