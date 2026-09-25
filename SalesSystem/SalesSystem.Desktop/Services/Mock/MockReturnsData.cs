using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// ط¨ظٹط§ظ†ط§طھ طھط¬ط±ظٹط¨ظٹط© ظ„ظ…ط±طھط¬ط¹ط§طھ ط§ظ„ظ…ط¨ظٹط¹ط§طھ ظˆط§ظ„ظ…ط´طھط±ظٹط§طھ â€” ظ…ط³طھظ‚ظ„ط© ط¹ظ† MockData ط§ظ„ط±ط¦ظٹط³ظٹ.
/// </summary>
internal static class MockReturnsData
{
    // â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ
    // ظ…ط±طھط¬ط¹ط§طھ ط§ظ„ظ…ط¨ظٹط¹ط§طھ â€” Sales Returns
    // â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ
    private static List<SalesReturnDto> _salesReturns = CreateSeedSalesReturns();
    public static List<SalesReturnDto> SalesReturns => _salesReturns;

    private static List<SalesReturnDto> CreateSeedSalesReturns()
    {
        return new List<SalesReturnDto>
        {
            // â”€â”€ 1: ظ…ط³ظˆط¯ط© â€” ط£ط­ظ…ط¯ ظ…ط­ظ…ط¯ ط¹ظ„ظٹ â€” ظ…ظ†طھط¬ط§طھ طھط§ظ„ظپط© â”€â”€
            new SalesReturnDto(
                Id: 1,
                ReturnNo: 1001,
                WarehouseId: 1,
                WarehouseName: "ط§ظ„ظ…ط®ط²ظ† ط§ظ„ط±ط¦ظٹط³ظٹ",
                CustomerId: 1,
                CustomerName: "ط£ط­ظ…ط¯ ظ…ط­ظ…ط¯ ط¹ظ„ظٹ",
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
                Notes: "ط´ط§ظٹظ„ظٹط¨طھظˆظ† â€” ط¹ط¨ظˆط© ظˆط§ط­ط¯ط© طھط§ظ„ظپط©",
                ReturnReason: "ظ…ظ†طھط¬ط§طھ طھط§ظ„ظپط©",
                Status: 1,
                CashBoxId: null,
                CashBoxName: null,
                RefundAmount: 0m,
                Items: new List<SalesReturnItemDto>
                {
                    new SalesReturnItemDto(Id: 1, ProductId: 1, ProductUnitId: 1,
                        ProductName: "ط´ط§ظٹ ظ„ظٹط¨طھظˆظ† 250ط؛", Quantity: 5m, UnitPrice: 500m,
                        DiscountAmount: 0m, LineTotal: 2500m, Mode: 1)
                }.AsReadOnly(),
                BranchId: 1,
                Currency: "YER"),

            // â”€â”€ 2: ظ…ط±ط­ظ„ â€” ط´ط±ظƒط© ط§ظ„ظ†ظˆط± ظ„ظ„طھط¬ط§ط±ط© â€” ط¨ط¶ط§ط¹ط© ط؛ظٹط± ظ…ط·ط§ط¨ظ‚ط© â”€â”€
            new SalesReturnDto(
                Id: 2,
                ReturnNo: 1002,
                WarehouseId: 1,
                WarehouseName: "ط§ظ„ظ…ط®ط²ظ† ط§ظ„ط±ط¦ظٹط³ظٹ",
                CustomerId: 2,
                CustomerName: "ط´ط±ظƒط© ط§ظ„ظ†ظˆط± ظ„ظ„طھط¬ط§ط±ط©",
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
                Notes: "ظ…ط¹ط¬ظˆظ† ط·ظ…ط§ط·ظ… â€” ظ†ظˆط¹ ظ…ط®طھظ„ظپ ط¹ظ† ط§ظ„ظ…ط·ظ„ظˆط¨",
                ReturnReason: "ط¨ط¶ط§ط¹ط© ط؛ظٹط± ظ…ط·ط§ط¨ظ‚ط© ظ„ظ„ظ…ظˆط§طµظپط§طھ",
                Status: 2,
                CashBoxId: null,
                CashBoxName: null,
                RefundAmount: 50000m,
                Items: new List<SalesReturnItemDto>
                {
                    new SalesReturnItemDto(Id: 2, ProductId: 6, ProductUnitId: 6,
                        ProductName: "ظ…ط¹ط¬ظˆظ† ط·ظ…ط§ط·ظ… 400ط؛", Quantity: 50m, UnitPrice: 600m,
                        DiscountAmount: 0m, LineTotal: 30000m, Mode: 1),
                    new SalesReturnItemDto(Id: 3, ProductId: 7, ProductUnitId: 7,
                        ProductName: "طھظˆظ†ط§ ظ…ط¹ظ„ط¨ط© 185ط؛", Quantity: 20m, UnitPrice: 1000m,
                        DiscountAmount: 0m, LineTotal: 20000m, Mode: 1)
                }.AsReadOnly(),
                BranchId: 1,
                Currency: "YER"),

            // â”€â”€ 3: ظ…ظ„ط؛ظٹ â€” ظ…ط­ظ…ط¯ ط¹ط¨ط¯ط§ظ„ط±ط­ظ…ظ† ط§ظ„ط³ط§ظ„ظ…ظٹ â€” ط®ط·ط£ ظپظٹ ط§ظ„ط·ظ„ط¨ â”€â”€
            new SalesReturnDto(
                Id: 3,
                ReturnNo: 1003,
                WarehouseId: 2,
                WarehouseName: "ظ…ط®ط²ظ† ط§ظ„ظ…ط¹ط±ط¶",
                CustomerId: 4,
                CustomerName: "ظ…ط­ظ…ط¯ ط¹ط¨ط¯ط§ظ„ط±ط­ظ…ظ† ط§ظ„ط³ط§ظ„ظ…ظٹ",
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
                Notes: "ط§ظ„ط؛ط§ط، â€” طھظ… ط§ظ„طھط±ط§ط¬ط¹ ط¹ظ† ط§ظ„ط¥ط±ط¬ط§ط¹",
                ReturnReason: "ط®ط·ط£ ظپظٹ ط§ظ„ط·ظ„ط¨",
                Status: 3,
                CashBoxId: null,
                CashBoxName: null,
                RefundAmount: 0m,
                Items: new List<SalesReturnItemDto>
                {
                    new SalesReturnItemDto(Id: 4, ProductId: 2, ProductUnitId: 2,
                        ProductName: "ط²ظٹطھ ظ†ط¨ط§طھظٹ 1 ظ„طھط±", Quantity: 10m, UnitPrice: 1500m,
                        DiscountAmount: 0m, LineTotal: 15000m, Mode: 1)
                }.AsReadOnly(),
                BranchId: 1,
                Currency: "YER")
        };
    }

    // â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ
    // ظ…ط±طھط¬ط¹ط§طھ ط§ظ„ظ…ط´طھط±ظٹط§طھ â€” Purchase Returns
    // â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ
    private static List<PurchaseReturnDto> _purchaseReturns = CreateSeedPurchaseReturns();
    public static List<PurchaseReturnDto> PurchaseReturns => _purchaseReturns;

    private static List<PurchaseReturnDto> CreateSeedPurchaseReturns()
    {
        return new List<PurchaseReturnDto>
        {
            // â”€â”€ 1: ظ…ط±ط­ظ„ â€” ظ…ط¬ظ…ظˆط¹ط© ظ‡ط§ط¦ظ„ ط³ط¹ظٹط¯ ط£ظ†ط¹ظ… â€” ط¨ط¶ط§ط¹ط© ظ…ظ†طھظ‡ظٹط© ط§ظ„طµظ„ط§ط­ظٹط© â”€â”€
            new PurchaseReturnDto(
                Id: 1,
                ReturnNo: 2001,
                WarehouseId: 1,
                WarehouseName: "ط§ظ„ظ…ط®ط²ظ† ط§ظ„ط±ط¦ظٹط³ظٹ",
                SupplierId: 1,
                SupplierName: "ظ…ط¬ظ…ظˆط¹ط© ظ‡ط§ط¦ظ„ ط³ط¹ظٹط¯ ط£ظ†ط¹ظ… ظˆط´ط±ظƒط§ظ‡",
                PurchaseInvoiceId: 1,
                LinkToInvoice: true,
                ReturnDate: new DateOnly(2026, 9, 18),
                SubTotal: 45000m,
                TotalAmount: 45000m,
                ReturnedDiscountAmount: 0m,
                ReturnedTaxAmount: 0m,
                ReturnedChargeAmount: 0m,
                TaxId: null,
                Notes: "ط²ظٹطھ ظ†ط¨ط§طھظٹ â€” ظ‚ط±ط¨ ط§ظ†طھظ‡ط§ط، ط§ظ„طµظ„ط§ط­ظٹط©",
                Status: 2,
                Items: new List<PurchaseReturnItemDto>
                {
                    new PurchaseReturnItemDto(Id: 1, ProductId: 2, ProductName: "ط²ظٹطھ ظ†ط¨ط§طھظٹ 1 ظ„طھط±",
                        ProductUnitId: 2, ProductUnitName: "ط²ط¬ط§ط¬ط©",
                        Quantity: 30m, UnitCost: 1500m, LineTotal: 45000m,
                        PurchaseInvoiceLineId: 1, CostInBaseCurrency: null)
                }.AsReadOnly(),
                BranchId: 1,
                Currency: "YER"),

            // â”€â”€ 2: ظ…ط³ظˆط¯ط© â€” ظ…ط¤ط³ط³ط© ط§ظ„طھط¶ط§ظ…ظ† ظ„طھط¬ط§ط±ط© ط§ظ„ظ…ظˆط§ط¯ ط§ظ„ط؛ط°ط§ط¦ظٹط© â€” ط¨ط¶ط§ط¹ط© ظ…طھط¶ط±ط±ط© â”€â”€
            new PurchaseReturnDto(
                Id: 2,
                ReturnNo: 2002,
                WarehouseId: 1,
                WarehouseName: "ط§ظ„ظ…ط®ط²ظ† ط§ظ„ط±ط¦ظٹط³ظٹ",
                SupplierId: 3,
                SupplierName: "ظ…ط¤ط³ط³ط© ط§ظ„طھط¶ط§ظ…ظ† ظ„طھط¬ط§ط±ط© ط§ظ„ظ…ظˆط§ط¯ ط§ظ„ط؛ط°ط§ط¦ظٹط©",
                PurchaseInvoiceId: null,
                LinkToInvoice: false,
                ReturnDate: new DateOnly(2026, 9, 20),
                SubTotal: 35000m,
                TotalAmount: 35000m,
                ReturnedDiscountAmount: 0m,
                ReturnedTaxAmount: 0m,
                ReturnedChargeAmount: 0m,
                TaxId: null,
                Notes: "ط£ط±ط² ط¨ط³ظ…طھظٹ â€” ط£ظƒظٹط§ط³ ظ…طھط¶ط±ط±ط© ظ…ظ† ط§ظ„ظ†ظ‚ظ„",
                Status: 1,
                Items: new List<PurchaseReturnItemDto>
                {
                    new PurchaseReturnItemDto(Id: 2, ProductId: 3, ProductName: "ط£ط±ط² ط¨ط³ظ…طھظٹ 5 ظƒط؛",
                        ProductUnitId: 3, ProductUnitName: "ظƒظٹط³",
                        Quantity: 10m, UnitCost: 3500m, LineTotal: 35000m,
                        PurchaseInvoiceLineId: null, CostInBaseCurrency: null)
                }.AsReadOnly(),
                BranchId: 1,
                Currency: "YER")
        };
    }
}

