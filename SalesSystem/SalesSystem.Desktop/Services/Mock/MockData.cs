using SalesSystem.DTO;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية مشتركة لاختبار واجهة المستخدم بدون API أو قاعدة بيانات.
/// </summary>
internal static class MockData
{
    // ═══════════════════════════════════════════════
    // العملاء — Customers
    // ═══════════════════════════════════════════════
    public static List<CustomerDto> Customers { get; } = new()
    {
        new CustomerDto(1, "أحمد محمد علي", "777123456", "ahmed@email.com",
            "شارع الزبيري - صنعاء", null, 500000m, true, 101,
            "عملاء - أحمد محمد علي", null, null, 125000m),
        new CustomerDto(2, "شركة النور للتجارة", "777987654", "alnoor@trade.com",
            "شارع الستين - صنعاء", "1234567890", 2000000m, true, 102,
            "عملاء - شركة النور للتجارة", null, "عميل جملة", 850000m),
        new CustomerDto(3, "مؤسسة اليمن الحديثة", "733456789", null,
            "شارع تعز - عدن", null, 1000000m, true, 103,
            "عملاء - مؤسسة اليمن الحديثة", null, null, 0m),
        new CustomerDto(4, "محمد عبدالرحمن السالمي", "771234567", null,
            "حجة - ميدي", null, 200000m, true, 104,
            "عملاء - محمد عبدالرحمن السالمي", null, null, 45000m),
        new CustomerDto(5, "عبدالله حسن المقطري", "778889990", null,
            "ذمار - شارع جمال", null, 0m, true, 105,
            "عملاء - عبدالله حسن المقطري", null, null, 15000m),
        new CustomerDto(6, "محلات البدر للمواد الغذائية", "775556667", "albadr@food.com",
            "إب - شارع الحرية", null, 3000000m, true, 106,
            "عملاء - محلات البدر للمواد الغذائية", null, "جملة", 1200000m),
        new CustomerDto(7, "سالم أحمد باوزير", "770001112", null,
            "الحديدة - شارع الصيادلة", null, 0m, false, 107,
            "عملاء - سالم أحمد باوزير", null, "معطل", 0m)
    };

    // ═══════════════════════════════════════════════
    // المخازن — Warehouses
    // ═══════════════════════════════════════════════
    public static List<WarehouseDto> Warehouses { get; } = new()
    {
        new WarehouseDto(1, "المخزن الرئيسي", "01-234567", "شارع الستين - صنعاء", "المخزن الرئيسي للمحل", true),
        new WarehouseDto(2, "مخزن المعرض", "01-765432", "شارع الزبيري - صنعاء", "مخزن معرض العرض", true),
        new WarehouseDto(3, "مخزن الفرع - عدن", null, "عدن - المنصورة", "مخزن فرع عدن", true)
    };

    // ═══════════════════════════════════════════════
    // الصناديق النقدية — Cash Boxes
    // ═══════════════════════════════════════════════
    public static List<CashBoxDto> CashBoxes { get; } = new();

    // ═══════════════════════════════════════════════
    // البنوك — Banks
    // ═══════════════════════════════════════════════
    public static List<BankDto> Banks { get; } = new();

    // ═══════════════════════════════════════════════
    // المنتجات — Products
    // ═══════════════════════════════════════════════
    public static List<ProductDto> Products { get; } = new()
    {
        new ProductDto(1,  "شاي ليبتون 250غ",          1, "مشروبات", null,  10m, false, null, true, "102030", 150m),
        new ProductDto(2,  "زيت نباتي 1 لتر",          2, "زيوت",    null,  20m, false, null, true, "102031",  80m),
        new ProductDto(3,  "أرز بسمتي 5 كغ",           3, "حبوب",    null,  15m, false, null, true, "102032",  60m),
        new ProductDto(4,  "سكر أبيض 1 كغ",            3, "حبوب",    null,  25m, false, null, true, "102033", 200m),
        new ProductDto(5,  "دقيق فاخر 2 كغ",           3, "حبوب",    null,  10m, false, null, true, "102034", 120m),
        new ProductDto(6,  "معجون طماطم 400غ",         4, "معلبات",  null,  30m, true,  null, true, "102035",  90m),
        new ProductDto(7,  "تونا معلبة 185غ",           4, "معلبات",  null,  20m, true,  null, true, "102036",  45m),
        new ProductDto(8,  "حليب مجفف 900غ",           5, "ألبان",   null,  10m, true,  null, true, "102037",  30m),
        new ProductDto(9,  "مناديل ورقية 150 ورقة",    6, "منظفات",  null,  50m, false, null, true, "102038", 200m),
        new ProductDto(10, "صابون غسيل 1 كغ",          6, "منظفات",  null,  30m, false, null, true, "102039",  75m)
    };

    // ═══════════════════════════════════════════════
    // وحدات المنتجات — Product Units
    // ═══════════════════════════════════════════════
    public static List<ProductUnitDto> ProductUnits { get; } = new()
    {
        // Id, ProductId, UnitId, UnitName, ConversionFactor, IsBaseUnit,
        // Barcode, SalePrice, WholesalePrice, CostPrice, MaxDiscountAmount
        new ProductUnitDto(1,  1,  1, "قطعة",  1m, true,
            "6200000000011", 150m, 140m, 120m, 10m),
        new ProductUnitDto(2,  2,  1, "زجاجة", 1m, true,
            "6200000000028",  80m, null,  65m, null),
        new ProductUnitDto(3,  3,  1, "كيس",   1m, true,
            null,             60m, 55m,  50m,  5m),
        new ProductUnitDto(4,  4,  1, "كيس",   1m, true,
            null,            200m, null, 180m, null),
        new ProductUnitDto(5,  5,  1, "كيس",   1m, true,
            "6200000000059", 120m, 115m, 105m, null),
        new ProductUnitDto(6,  6,  1, "علبة",  1m, true,
            null,             90m, null,  75m, null),
        new ProductUnitDto(7,  7,  1, "علبة",  1m, true,
            null,             45m, null,  38m, null),
        new ProductUnitDto(8,  8,  1, "علبة",  1m, true,
            null,             30m, null,  25m, null),
        new ProductUnitDto(9,  9,  1, "علبة",  1m, true,
            null,            200m, null, 170m, null),
        new ProductUnitDto(10, 10, 1, "كيس",   1m, true,
            null,             75m, null,  62m, null)
    };

    // ═══════════════════════════════════════════════
    // أسعار المنتجات — Product Prices
    // ═══════════════════════════════════════════════
    public static List<ProductPriceDto> ProductPrices { get; } = new()
    {
        new ProductPriceDto(1,  1,  "قطعة",  500m,  DateTime.UtcNow.AddMonths(-3), null),
        new ProductPriceDto(2,  2,  "زجاجة", 1500m, DateTime.UtcNow.AddMonths(-3), null),
        new ProductPriceDto(3,  3,  "كيس",   3500m, DateTime.UtcNow.AddMonths(-3), null),
        new ProductPriceDto(4,  4,  "كيس",   800m,  DateTime.UtcNow.AddMonths(-3), null),
        new ProductPriceDto(5,  5,  "كيس",   1200m, DateTime.UtcNow.AddMonths(-3), null),
        new ProductPriceDto(6,  6,  "علبة",  600m,  DateTime.UtcNow.AddMonths(-3), null),
        new ProductPriceDto(7,  7,  "علبة",  1000m, DateTime.UtcNow.AddMonths(-3), null),
        new ProductPriceDto(8,  8,  "علبة",  4500m, DateTime.UtcNow.AddMonths(-3), null),
        new ProductPriceDto(9,  9,  "علبة",  500m,  DateTime.UtcNow.AddMonths(-3), null),
        new ProductPriceDto(10, 10, "كيس",   700m,  DateTime.UtcNow.AddMonths(-3), null)
    };

    // ═══════════════════════════════════════════════
    // الضرائب — Taxes
    // ═══════════════════════════════════════════════
    public static List<TaxDto> Taxes { get; } = new()
    {
        new TaxDto(1, "ضريبة القيمة المضافة 15%", "VAT15", 15m, 1, true, true),
        new TaxDto(2, "ضريبة القيمة المضافة 5%", "VAT5", 5m, 1, false, true)
    };

    // ═══════════════════════════════════════════════
    // فواتير البيع — Sales Invoices
    // ═══════════════════════════════════════════════
    private static List<SalesInvoiceDto> _invoices = CreateSeedInvoices();
    public static List<SalesInvoiceDto> Invoices => _invoices;

    private static List<SalesInvoiceDto> CreateSeedInvoices()
    {
        return new List<SalesInvoiceDto>
        {
            // فاتورة 1: مسودة — أحمد محمد — بدون خصم/ضريبة — لم تُدفع
            CreateInvoice(
                id: 1, invoiceNo: 1001, custId: 1, custName: "أحمد محمد علي",
                whId: 1, whName: "المخزن الرئيسي", date: DateTime.Today.AddDays(-5),
                payType: 1, status: 1, paid: 0m, disc: 0m, tax: 0m, other: 0m, notes: null,
                lines: new[]
                {
                    (1, 1, "شاي ليبتون 250غ",  5m,  500m,  1),
                    (2, 2, "زيت نباتي 1 لتر",   3m, 1500m,  2),
                    (3, 4, "سكر أبيض 1 كغ",   10m,  800m,  4)
                }),

            // فاتورة 2: مرحل — شركة النور — بدون خصم/ضريبة — مدفوعة بالكامل
            CreateInvoice(
                id: 2, invoiceNo: 1002, custId: 2, custName: "شركة النور للتجارة",
                whId: 1, whName: "المخزن الرئيسي", date: DateTime.Today.AddDays(-3),
                payType: 1, status: 2, paid: 175000m, disc: 0m, tax: 0m, other: 0m, notes: "بيع جملة",
                lines: new[]
                {
                    (4, 3, "أرز بسمتي 5 كغ",    20m, 3500m,  3),
                    (5, 6, "معجون طماطم 400غ",  50m,  600m,  6),
                    (6, 7, "تونا معلبة 185غ",   30m, 1000m,  7),
                    (7, 8, "حليب مجفف 900غ",    10m, 4500m,  8)
                }),

            // فاتورة 3: ملغية — عبدالله المقطري
            CreateInvoice(
                id: 3, invoiceNo: 1003, custId: 5, custName: "عبدالله حسن المقطري",
                whId: 1, whName: "المخزن الرئيسي", date: DateTime.Today.AddDays(-10),
                payType: 2, status: 3, paid: 0m, disc: 0m, tax: 0m, other: 0m, notes: "فاتورة ملغية",
                lines: new[]
                {
                    (8,  9, "مناديل ورقية 150 ورقة", 10m, 500m, 9),
                    (9, 10, "صابون غسيل 1 كغ",         5m, 700m, 10)
                }),

            // فاتورة 4: مسودة — مؤسسة اليمن الحديثة — خصم 5000 + ضريبة 5% — لم تُدفع
            CreateInvoice(
                id: 4, invoiceNo: 1004, custId: 3, custName: "مؤسسة اليمن الحديثة",
                whId: 1, whName: "المخزن الرئيسي", date: DateTime.Today.AddDays(-1),
                payType: 2, status: 1, paid: 0m, disc: 5000m, tax: 7050m, other: 0m, notes: null,
                lines: new[]
                {
                    (10, 1, "شاي ليبتون 250غ",  100m, 500m, 1),
                    (11, 5, "دقيق فاخر 2 كغ",    40m, 1200m, 5),
                    (12, 4, "سكر أبيض 1 كغ",     60m,  800m, 4)
                }),

            // فاتورة 5: مرحل — محمد السالمي — بدون خصم/ضريبة — مدفوعة جزئياً
            CreateInvoice(
                id: 5, invoiceNo: 1005, custId: 4, custName: "محمد عبدالرحمن السالمي",
                whId: 2, whName: "مخزن المعرض", date: DateTime.Today.AddDays(-2),
                payType: 3, status: 2, paid: 20000m, disc: 0m, tax: 0m, other: 0m, notes: null,
                lines: new[]
                {
                    (13, 2, "زيت نباتي 1 لتر", 10m, 1500m, 2),
                    (14, 3, "أرز بسمتي 5 كغ",   5m, 3500m, 3)
                }),

            // فاتورة 6: مسودة — محلات البدر — بدون خصم/ضريبة — لم تُدفع
            CreateInvoice(
                id: 6, invoiceNo: 1006, custId: 6, custName: "محلات البدر للمواد الغذائية",
                whId: 1, whName: "المخزن الرئيسي", date: DateTime.Today,
                payType: 2, status: 1, paid: 0m, disc: 0m, tax: 0m, other: 0m, notes: null,
                lines: new[]
                {
                    (15, 6, "معجون طماطم 400غ", 200m, 600m,  6),
                    (16, 7, "تونا معلبة 185غ",  100m, 1000m, 7)
                }),

            // فاتورة 7: مرحل — أحمد محمد — مدفوعة بالكامل
            CreateInvoice(
                id: 7, invoiceNo: 1007, custId: 1, custName: "أحمد محمد علي",
                whId: 1, whName: "المخزن الرئيسي", date: DateTime.Today,
                payType: 1, status: 2, paid: 17500m, disc: 0m, tax: 0m, other: 0m, notes: null,
                lines: new[]
                {
                    (17, 1, "شاي ليبتون 250غ",           20m, 500m, 1),
                    (18, 9, "مناديل ورقية 150 ورقة",    15m, 500m, 9)
                })
        };
    }

    /// <summary>
    /// مساعد لإنشاء فاتورة بيع مع التحقق من صحة الحسابات المالية.
    /// </summary>
    private static SalesInvoiceDto CreateInvoice(
        int id, int invoiceNo, int custId, string custName,
        int whId, string whName, DateTime date, byte payType, byte status,
        decimal paid, decimal disc, decimal tax, decimal other, string? notes,
        (int lineId, int productId, string productName, decimal qty, decimal price, int unitId)[] lines)
    {
        var items = lines.Select(l =>
        {
            decimal lineTotal = l.qty * l.price;
            return new SalesInvoiceLineDto(
                Id: l.lineId, ProductId: l.productId, ProductName: l.productName,
                Quantity: l.qty, UnitPrice: l.price, LineTotal: lineTotal,
                ProductUnitId: l.unitId);
        }).ToList();

        decimal subTotal = items.Sum(i => i.LineTotal);
        decimal netTotal = subTotal - disc + tax + other;
        decimal remaining = netTotal - paid;

        return new SalesInvoiceDto(
            Id: id, InvoiceNo: invoiceNo, CustomerId: custId, CustomerName: custName,
            WarehouseId: whId, WarehouseName: whName, InvoiceDate: date,
            PaymentType: payType, SubTotal: subTotal, DiscountAmount: disc,
            DiscountType: 0, DiscountRate: null, TaxAmount: tax, OtherCharges: other,
            NetTotal: netTotal, PaidAmount: paid, RemainingAmount: remaining,
            CostInBaseCurrency: null, Notes: notes, Status: status,
            TaxId: null, TaxName: null, TaxRate: null,
            CashBoxId: null, CashBoxName: null, Items: items);
    }
}
