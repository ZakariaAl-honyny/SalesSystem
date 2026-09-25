using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// ط¨ظٹط§ظ†ط§طھ طھط¬ط±ظٹط¨ظٹط© ظ„ظپظˆط§طھظٹط± ط§ظ„ظ…ط´طھط±ظٹط§طھ â€” ظ…ط³طھظˆط­ط§ط© ظ…ظ† INITIAL_PURCHASE_INVOICES ظپظٹ ظˆط§ط¬ظ‡ط© ط§ظ„ظˆظٹط¨.
/// </summary>
internal static class MockPurchasesData
{
    // â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ
    // ظپظˆط§طھظٹط± ط§ظ„ظ…ط´طھط±ظٹط§طھ â€” Purchase Invoices
    // â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ
    private static List<PurchaseInvoiceDto> _purchases = CreateSeedPurchases();
    public static List<PurchaseInvoiceDto> Purchases => _purchases;

    private static List<PurchaseInvoiceDto> CreateSeedPurchases()
    {
        return new List<PurchaseInvoiceDto>
        {
            // ظپط§طھظˆط±ط© 1: ظ…ط³ظˆط¯ط© â€” ظ…ط¬ظ…ظˆط¹ط© ظ‡ط§ط¦ظ„ ط³ط¹ظٹط¯ â€” ط¢ط¬ظ„ â€” ظ„ظ… طھظڈط¯ظپط¹
            CreatePurchase(
                id: 1, invoiceNo: 5001, supplierId: 1,
                supplierName: "ظ…ط¬ظ…ظˆط¹ط© ظ‡ط§ط¦ظ„ ط³ط¹ظٹط¯ ط£ظ†ط¹ظ… ظˆط´ط±ظƒط§ظ‡",
                whId: 1, whName: "ط§ظ„ظ…ط®ط²ظ† ط§ظ„ط±ط¦ظٹط³ظٹ",
                date: new DateOnly(2026, 8, 25), payType: 2, status: 1,
                paid: 0m, disc: 0m, tax: 0m, other: 0m,
                notes: "طھظˆط±ظٹط¯ ط¯ظپط¹ط© ظ…ظˆط§ط¯ ط؛ط°ط§ط¦ظٹط© ط£ظˆظ„ظٹط© ظ„ظ„ط´ظ‡ط±",
                supplierInvoiceNo: "HSA-INV-9921",
                lines: new[]
                {
                    (1, 1, "ط´ط§ظٹ ظ„ظٹط¨طھظˆظ† 250ط؛",    1, "ظ‚ط·ط¹ط©",  50m, 1200m),
                    (2, 2, "ط²ظٹطھ ظ†ط¨ط§طھظٹ 1 ظ„طھط±",     2, "ط²ط¬ط§ط¬ط©", 40m, 1500m)
                }),

            // ظپط§طھظˆط±ط© 2: ظ…ط±ط­ظ„ â€” ط´ط±ظƒط© ظ…ظٹط§ظ‡ ط­ط¯ط© â€” ظ†ظ‚ط¯ظٹ â€” ظ…ط¯ظپظˆط¹ط© ط¨ط§ظ„ظƒط§ظ…ظ„
            CreatePurchase(
                id: 2, invoiceNo: 5002, supplierId: 2,
                supplierName: "ط´ط±ظƒط© ظ…ظٹط§ظ‡ ظˆظ…ط´ط±ظˆط¨ط§طھ ط­ط¯ط© ط§ظ„ظ…ط­ط¯ظˆط¯ط©",
                whId: 2, whName: "ظ…ط®ط²ظ† ط§ظ„ظ…ط¹ط±ط¶",
                date: new DateOnly(2026, 8, 28), payType: 1, status: 2,
                paid: 19000m, disc: 1000m, tax: 0m, other: 0m,
                notes: "ط´ط±ط§ط، ظ†ظ‚ط¯ظٹ ظ…ظ† ظ…طµظ†ط¹ ظ…ظٹط§ظ‡ ط­ط¯ط© ظ…ط¹ ط®طµظ… طھط´ط¬ظٹط¹ظٹ",
                supplierInvoiceNo: "HADA-664",
                lines: new[]
                {
                    (3, 6, "ظ…ط¹ط¬ظˆظ† ط·ظ…ط§ط·ظ… 400ط؛", 6, "ط¹ظ„ط¨ط©", 100m, 200m)
                }),

            // ظپط§طھظˆط±ط© 3: ظ…ط±ط­ظ„ â€” ط´ط±ظƒط© ط§ظ„ظƒط¨ظˆط³ â€” ط¢ط¬ظ„ â€” ظ„ظ… طھظڈط¯ظپط¹
            CreatePurchase(
                id: 3, invoiceNo: 5003, supplierId: 4,
                supplierName: "ط´ط±ظƒط© ط§ظ„ظƒط¨ظˆط³ ظ„ظ„طھط¬ط§ط±ط© ظˆط§ظ„طµظ†ط§ط¹ط©",
                whId: 1, whName: "ط§ظ„ظ…ط®ط²ظ† ط§ظ„ط±ط¦ظٹط³ظٹ",
                date: new DateOnly(2026, 8, 30), payType: 2, status: 2,
                paid: 0m, disc: 0m, tax: 0m, other: 0m,
                notes: "ط´ط§ظٹ ط§ظ„ظƒط¨ظˆط³ ط§ظ„ظ…ظ…طھط§ط² طھظˆط±ظٹط¯ ط¢ط¬ظ„",
                supplierInvoiceNo: "KAB-8832",
                lines: new[]
                {
                    (4, 7, "طھظˆظ†ط§ ظ…ط¹ظ„ط¨ط© 185ط؛", 7, "ط¹ظ„ط¨ط©", 30m, 1750m)
                })
        };
    }

    /// <summary>
    /// ظ…ط³ط§ط¹ط¯ ظ„ط¥ظ†ط´ط§ط، ظپط§طھظˆط±ط© ظ…ط´طھط±ظٹط§طھ ظ…ط¹ ط§ظ„طھط­ظ‚ظ‚ ظ…ظ† طµط­ط© ط§ظ„ط­ط³ط§ط¨ط§طھ ط§ظ„ظ…ط§ظ„ظٹط©.
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
            CostInBaseCurrency: null,
            TaxAmount: tax, OtherCharges: other,
            NetTotal: netTotal, PaidAmount: paid, RemainingAmount: remaining,
            Notes: notes, SupplierInvoiceNo: supplierInvoiceNo,
            AttachmentPath: null, Status: status,
            TaxId: null, TaxName: null, TaxRate: null,
            CashBoxId: null, Items: items);
    }
}

