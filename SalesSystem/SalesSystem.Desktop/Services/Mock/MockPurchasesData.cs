using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لفواتير المشتريات — مستوحاة من INITIAL_PURCHASE_INVOICES في واجهة الويب.
/// </summary>
internal static class MockPurchasesData
{
    // ═══════════════════════════════════════════════
    // فواتير المشتريات — Purchase Invoices
    // ═══════════════════════════════════════════════
    private static List<PurchaseInvoiceDto> _purchases = CreateSeedPurchases();
    public static List<PurchaseInvoiceDto> Purchases => _purchases;

    private static List<PurchaseInvoiceDto> CreateSeedPurchases()
    {
        return new List<PurchaseInvoiceDto>
        {
            // فاتورة 1: مسودة — مجموعة هائل سعيد — آجل — لم تُدفع
            CreatePurchase(
                id: 1, invoiceNo: 5001, supplierId: 1,
                supplierName: "مجموعة هائل سعيد أنعم وشركاه",
                whId: 1, whName: "المخزن الرئيسي",
                date: new DateOnly(2026, 8, 25), payType: 2, status: 1,
                paid: 0m, disc: 0m, tax: 0m, other: 0m,
                notes: "توريد دفعة مواد غذائية أولية للشهر",
                supplierInvoiceNo: "HSA-INV-9921",
                lines: new[]
                {
                    (1, 1, "شاي ليبتون 250غ",    1, "قطعة",  50m, 1200m),
                    (2, 2, "زيت نباتي 1 لتر",     2, "زجاجة", 40m, 1500m)
                }),

            // فاتورة 2: مرحل — شركة مياه حدة — نقدي — مدفوعة بالكامل
            CreatePurchase(
                id: 2, invoiceNo: 5002, supplierId: 2,
                supplierName: "شركة مياه ومشروبات حدة المحدودة",
                whId: 2, whName: "مخزن المعرض",
                date: new DateOnly(2026, 8, 28), payType: 1, status: 2,
                paid: 19000m, disc: 1000m, tax: 0m, other: 0m,
                notes: "شراء نقدي من مصنع مياه حدة مع خصم تشجيعي",
                supplierInvoiceNo: "HADA-664",
                lines: new[]
                {
                    (3, 6, "معجون طماطم 400غ", 6, "علبة", 100m, 200m)
                }),

            // فاتورة 3: مرحل — شركة الكبوس — آجل — لم تُدفع
            CreatePurchase(
                id: 3, invoiceNo: 5003, supplierId: 4,
                supplierName: "شركة الكبوس للتجارة والصناعة",
                whId: 1, whName: "المخزن الرئيسي",
                date: new DateOnly(2026, 8, 30), payType: 2, status: 2,
                paid: 0m, disc: 0m, tax: 0m, other: 0m,
                notes: "شاي الكبوس الممتاز توريد آجل",
                supplierInvoiceNo: "KAB-8832",
                lines: new[]
                {
                    (4, 7, "تونا معلبة 185غ", 7, "علبة", 30m, 1750m)
                })
        };
    }

    /// <summary>
    /// مساعد لإنشاء فاتورة مشتريات مع التحقق من صحة الحسابات المالية.
    /// </summary>
    private static PurchaseInvoiceDto CreatePurchase(
        int id, int invoiceNo, int supplierId, string supplierName,
        int whId, string whName, DateOnly date, byte payType, byte status,
        decimal paid, decimal disc, decimal tax, decimal other, string? notes,
        string? supplierInvoiceNo,
        (int lineId, int productId, string productName, int unitId, string unitName, decimal qty, decimal cost)[] lines)
    {
        var items = lines.Select(l =>
        {
            decimal lineTotal = l.qty * l.cost;
            return new PurchaseInvoiceLineDto(
                Id: l.lineId, ProductId: l.productId, ProductName: l.productName,
                ProductUnitId: l.unitId, ProductUnitName: l.unitName,
                Quantity: l.qty, UnitPrice: l.cost, LineTotal: lineTotal,
                LandedUnitCost: l.cost,
                DiscountType: 0,
                DiscountRate: null,
                DiscountAmount: 0m,
                CostInBaseCurrency: null, AdditionalFeesAmount: 0m);
        }).ToList();

        decimal subTotal = items.Sum(i => i.LineTotal);
        decimal netTotal = subTotal - disc + tax + other;
        decimal remaining = netTotal - paid;

        return new PurchaseInvoiceDto(
            Id: id, InvoiceNo: invoiceNo,
            SupplierId: supplierId, SupplierName: supplierName,
            WarehouseId: whId, WarehouseName: whName,
            InvoiceDate: date, PaymentType: payType,
            SubTotal: subTotal, DiscountAmount: disc,
            DiscountType: 0, DiscountRate: null,
            CostInBaseCurrency: null,
            TaxAmount: tax, OtherCharges: other,
            NetTotal: netTotal, PaidAmount: paid, RemainingAmount: remaining,
            Notes: notes, SupplierInvoiceNo: supplierInvoiceNo,
            AttachmentPath: null, Status: status,
            TaxId: null, TaxName: null, TaxRate: null,
            CashBoxId: null, Items: items);
    }
}

