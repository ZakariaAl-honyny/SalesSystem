using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لمرتجعات المبيعات والمشتريات — مستقلة عن MockData الرئيسي.
/// </summary>
internal static class MockReturnsData
{
    // ═══════════════════════════════════════════════
    // مرتجعات المبيعات — Sales Returns
    // ═══════════════════════════════════════════════
    private static List<SalesReturnDto> _salesReturns = CreateSeedSalesReturns();
    public static List<SalesReturnDto> SalesReturns => _salesReturns;

    private static List<SalesReturnDto> CreateSeedSalesReturns()
    {
        return new List<SalesReturnDto>
        {
            // ── 1: مسودة — أحمد محمد علي — منتجات تالفة ──
            new SalesReturnDto(
                Id: 1,
                ReturnNo: 1001,
                WarehouseId: 1,
                WarehouseName: "المخزن الرئيسي",
                CustomerId: 1,
                CustomerName: "أحمد محمد علي",
                SalesInvoiceId: 1,
                ReturnDate: DateTime.Today.AddDays(-2),
                SubTotal: 2500m,
                TaxAmount: 0m,
                DiscountAmount: 0m,
                TotalAmount: 2500m,
                ReturnedDiscountAmount: 0m,
                ReturnedTaxAmount: 0m,
                ReturnedChargeAmount: 0m,
                TaxId: null,
                Notes: "شايليبتون — عبوة واحدة تالفة",
                ReturnReason: "منتجات تالفة",
                Status: 1,
                CashBoxId: null,
                CashBoxName: null,
                RefundAmount: 0m,
                Items: new List<SalesReturnItemDto>
                {
                    new SalesReturnItemDto(Id: 1, ProductId: 1, ProductUnitId: 1,
                        ProductName: "شاي ليبتون 250غ", Quantity: 5m, UnitPrice: 500m,
                        DiscountAmount: 0m, LineTotal: 2500m, Mode: 1)
                }.AsReadOnly(),
                BranchId: 1),

            // ── 2: مرحل — شركة النور للتجارة — بضاعة غير مطابقة ──
            new SalesReturnDto(
                Id: 2,
                ReturnNo: 1002,
                WarehouseId: 1,
                WarehouseName: "المخزن الرئيسي",
                CustomerId: 2,
                CustomerName: "شركة النور للتجارة",
                SalesInvoiceId: 2,
                ReturnDate: DateTime.Today.AddDays(-1),
                SubTotal: 50000m,
                TaxAmount: 0m,
                DiscountAmount: 0m,
                TotalAmount: 50000m,
                ReturnedDiscountAmount: 0m,
                ReturnedTaxAmount: 0m,
                ReturnedChargeAmount: 0m,
                TaxId: null,
                Notes: "معجون طماطم — نوع مختلف عن المطلوب",
                ReturnReason: "بضاعة غير مطابقة للمواصفات",
                Status: 2,
                CashBoxId: null,
                CashBoxName: null,
                RefundAmount: 50000m,
                Items: new List<SalesReturnItemDto>
                {
                    new SalesReturnItemDto(Id: 2, ProductId: 6, ProductUnitId: 6,
                        ProductName: "معجون طماطم 400غ", Quantity: 50m, UnitPrice: 600m,
                        DiscountAmount: 0m, LineTotal: 30000m, Mode: 1),
                    new SalesReturnItemDto(Id: 3, ProductId: 7, ProductUnitId: 7,
                        ProductName: "تونا معلبة 185غ", Quantity: 20m, UnitPrice: 1000m,
                        DiscountAmount: 0m, LineTotal: 20000m, Mode: 1)
                }.AsReadOnly(),
                BranchId: 1),

            // ── 3: ملغي — محمد عبدالرحمن السالمي — خطأ في الطلب ──
            new SalesReturnDto(
                Id: 3,
                ReturnNo: 1003,
                WarehouseId: 2,
                WarehouseName: "مخزن المعرض",
                CustomerId: 4,
                CustomerName: "محمد عبدالرحمن السالمي",
                SalesInvoiceId: 5,
                ReturnDate: DateTime.Today.AddDays(-5),
                SubTotal: 15000m,
                TaxAmount: 0m,
                DiscountAmount: 0m,
                TotalAmount: 15000m,
                ReturnedDiscountAmount: 0m,
                ReturnedTaxAmount: 0m,
                ReturnedChargeAmount: 0m,
                TaxId: null,
                Notes: "الغاء — تم التراجع عن الإرجاع",
                ReturnReason: "خطأ في الطلب",
                Status: 3,
                CashBoxId: null,
                CashBoxName: null,
                RefundAmount: 0m,
                Items: new List<SalesReturnItemDto>
                {
                    new SalesReturnItemDto(Id: 4, ProductId: 2, ProductUnitId: 2,
                        ProductName: "زيت نباتي 1 لتر", Quantity: 10m, UnitPrice: 1500m,
                        DiscountAmount: 0m, LineTotal: 15000m, Mode: 1)
                }.AsReadOnly(),
                BranchId: 1)
        };
    }

    // ═══════════════════════════════════════════════
    // مرتجعات المشتريات — Purchase Returns
    // ═══════════════════════════════════════════════
    private static List<PurchaseReturnDto> _purchaseReturns = CreateSeedPurchaseReturns();
    public static List<PurchaseReturnDto> PurchaseReturns => _purchaseReturns;

    private static List<PurchaseReturnDto> CreateSeedPurchaseReturns()
    {
        return new List<PurchaseReturnDto>
        {
            // ── 1: مرحل — مجموعة هائل سعيد أنعم — بضاعة منتهية الصلاحية ──
            new PurchaseReturnDto(
                Id: 1,
                ReturnNo: 2001,
                WarehouseId: 1,
                WarehouseName: "المخزن الرئيسي",
                SupplierId: 1,
                SupplierName: "مجموعة هائل سعيد أنعم وشركاه",
                PurchaseInvoiceId: 1,
                LinkToInvoice: true,
                DiscountType: 0, DiscountRate: null,
                ReturnDate: new DateOnly(2026, 9, 18),
                SubTotal: 45000m,
                TotalAmount: 45000m,
                ReturnedDiscountAmount: 0m,
                ReturnedTaxAmount: 0m,
                ReturnedChargeAmount: 0m,
                TaxId: null,
                Notes: "زيت نباتي — قرب انتهاء الصلاحية",
                Status: 2,
                Items: new List<PurchaseReturnItemDto>
                {
                    new PurchaseReturnItemDto(Id: 1, ProductId: 2, ProductName: "زيت نباتي 1 لتر",
                        ProductUnitId: 2, ProductUnitName: "زجاجة",
                        Quantity: 30m, UnitCost: 1500m, LineTotal: 45000m,
                        PurchaseInvoiceLineId: 1, CostInBaseCurrency: null)
                }.AsReadOnly(),
                BranchId: 1),

            // ── 2: مسودة — مؤسسة التضامن لتجارة المواد الغذائية — بضاعة متضررة ──
            new PurchaseReturnDto(
                Id: 2,
                ReturnNo: 2002,
                WarehouseId: 1,
                WarehouseName: "المخزن الرئيسي",
                SupplierId: 3,
                SupplierName: "مؤسسة التضامن لتجارة المواد الغذائية",
                PurchaseInvoiceId: null,
                LinkToInvoice: false,
                DiscountType: 0, DiscountRate: null,
                ReturnDate: new DateOnly(2026, 9, 20),
                SubTotal: 35000m,
                TotalAmount: 35000m,
                ReturnedDiscountAmount: 0m,
                ReturnedTaxAmount: 0m,
                ReturnedChargeAmount: 0m,
                TaxId: null,
                Notes: "أرز بسمتي — أكياس متضررة من النقل",
                Status: 1,
                Items: new List<PurchaseReturnItemDto>
                {
                    new PurchaseReturnItemDto(Id: 2, ProductId: 3, ProductName: "أرز بسمتي 5 كغ",
                        ProductUnitId: 3, ProductUnitName: "كيس",
                        Quantity: 10m, UnitCost: 3500m, LineTotal: 35000m,
                        PurchaseInvoiceLineId: null, CostInBaseCurrency: null)
                }.AsReadOnly(),
                BranchId: 1)
        };
    }
}

