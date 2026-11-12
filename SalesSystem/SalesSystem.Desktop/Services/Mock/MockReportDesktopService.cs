using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة تقارير المبيعات والمشتريات والمخزون — تُشتق كل الأرقام من مخازن بيانات التجربة:
/// <c>MockData.Invoices</c> و<c>MockPurchasesData.Purchases</c> (المستندات وسطورها)،
/// <c>MockReturnsData</c> (المرتجعات)، <c>MockInventoryData</c> (الدفعات والتسويات)،
/// <c>MockData.Products/ProductUnits/Warehouses/Customers</c>، <c>MockSuppliersData</c>
/// و<c>MockSupplierPaymentsData</c>.
/// </summary>
/// <remarks>
/// الفلترة بنفس دلالة طبقة الأعمال: المستند المرحّل فقط (Posted = 2)، النطاقات الزمنية شاملة،
/// والنتيجة الفارغة تُعيد قائمة فارغة بنجاح لا <c>null</c>.
/// لا تلمس قاعدة البيانات ولا أي اتصال شبكي ولا ترمي استثناءً.
/// </remarks>
public class MockReportDesktopService : IReportDesktopService
{
    /// <summary>المستند المرحّل فقط (Draft = 1, Posted = 2, Cancelled = 3).</summary>
    private const byte Posted = 2;

    private const string DateRangeError = "تاريخ البداية يجب أن يكون قبل تاريخ النهاية";
    private const string Uncategorized = "غير مصنف";
    private const string DefaultWarehouseName = "المخزن الرئيسي";
    private const string DefaultUnitName = "وحدة";
    private const string NoValue = "-";
    private const string SystemUser = "النظام";

    private const string SalesReturnType = "مبيعات";
    private const string PurchaseReturnType = "مشتريات";
    private const string SalesMovement = "مبيعات";
    private const string PurchaseMovement = "مشتريات";
    private const string SalesReturnMovement = "مرتجع مبيعات";
    private const string PurchaseReturnMovement = "مرتجع مشتريات";
    private const string AdjustmentMovement = "تسوية";

    private const string SalesError = "حدث خطأ أثناء جلب تقرير المبيعات";
    private const string PurchasesError = "حدث خطأ أثناء جلب تقرير المشتريات";
    private const string StockError = "حدث خطأ أثناء جلب تقرير المخزون";
    private const string CustomerBalancesError = "حدث خطأ أثناء جلب أرصدة العملاء";
    private const string SupplierBalancesError = "حدث خطأ أثناء جلب أرصدة الموردين";
    private const string MovementsError = "حدث خطأ أثناء جلب تقرير حركة المنتجات";
    private const string LowStockError = "حدث خطأ أثناء جلب تقرير المخزون المنخفض";
    private const string ExpiredError = "حدث خطأ أثناء جلب تقرير المنتجات منتهية الصلاحية";
    private const string StockBalanceError = "حدث خطأ أثناء جلب تقرير أرصدة المخزون";
    private const string WarehouseMovementsError = "حدث خطأ أثناء جلب تقرير حركة المستودعات";
    private const string DetailedLedgerError = "حدث خطأ أثناء جلب دفتر حركة المخزون التفصيلي";
    private const string ReturnsError = "حدث خطأ أثناء جلب تقرير المرتجعات";
    private const string AgingError = "حدث خطأ أثناء جلب تقرير الأعمار";

    // ═══════════════════════════════════════════════
    // 1) تقرير المبيعات
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<SalesReportDto>>> GetSalesAsync(
        DateTime? from = null, DateTime? to = null, int? customerId = null, CancellationToken ct = default)
    {
        if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            return Task.FromResult(Result<IReadOnlyList<SalesReportDto>>.Failure(DateRangeError));

        try
        {
            var rows = MockData.Invoices
                .Where(i => i.Status == Posted)
                .Where(i => !from.HasValue || i.InvoiceDate.Date >= from.Value.Date)
                .Where(i => !to.HasValue || i.InvoiceDate.Date <= to.Value.Date)
                .Where(i => !customerId.HasValue || i.CustomerId == customerId.Value)
                .OrderByDescending(i => i.InvoiceDate)
                .ThenByDescending(i => i.InvoiceNo)
                .Select(i => new SalesReportDto(
                    InvoiceDate: i.InvoiceDate.Date,
                    Id: i.InvoiceNo,
                    CustomerName: i.CustomerName,
                    SubTotal: Money(i.SubTotal),
                    DiscountAmount: Money(i.DiscountAmount),
                    TaxAmount: Money(i.TaxAmount),
                    TotalAmount: Money(i.NetTotal),
                    PaidAmount: Money(i.PaidAmount),
                    DueAmount: Money(i.RemainingAmount)))
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<SalesReportDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<SalesReportDto>>.Failure(SalesError));
        }
    }

    // ═══════════════════════════════════════════════
    // 2) تقرير المشتريات
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<PurchaseReportDto>>> GetPurchasesAsync(
        DateTime? from = null, DateTime? to = null, int? supplierId = null, CancellationToken ct = default)
    {
        if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            return Task.FromResult(Result<IReadOnlyList<PurchaseReportDto>>.Failure(DateRangeError));

        try
        {
            var rows = MockPurchasesData.Purchases
                .Where(p => p.Status == Posted)
                .Where(p => !supplierId.HasValue || p.SupplierId == supplierId.Value)
                .Select(p => new { Purchase = p, Date = p.InvoiceDate.ToDateTime(TimeOnly.MinValue).Date })
                .Where(x => !from.HasValue || x.Date >= from.Value.Date)
                .Where(x => !to.HasValue || x.Date <= to.Value.Date)
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Purchase.InvoiceNo)
                .Select(x => new PurchaseReportDto(
                    InvoiceDate: x.Date,
                    Id: x.Purchase.InvoiceNo,
                    SupplierName: x.Purchase.SupplierName,
                    SubTotal: Money(x.Purchase.SubTotal),
                    DiscountAmount: Money(x.Purchase.DiscountAmount),
                    TaxAmount: Money(x.Purchase.TaxAmount),
                    TotalAmount: Money(x.Purchase.NetTotal),
                    PaidAmount: Money(x.Purchase.PaidAmount),
                    DueAmount: Money(x.Purchase.RemainingAmount)))
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<PurchaseReportDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<PurchaseReportDto>>.Failure(PurchasesError));
        }
    }

    // ═══════════════════════════════════════════════
    // 3) تقرير المخزون
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<StockReportDto>>> GetStockReportAsync(
        int? warehouseId = null, CancellationToken ct = default)
    {
        try
        {
            var rows = BuildStockRows()
                .Where(r => !warehouseId.HasValue || r.WarehouseId == warehouseId.Value)
                .OrderBy(r => r.WarehouseName)
                .ThenBy(r => r.ProductName)
                .Select(r => new StockReportDto(
                    ProductId: r.ProductId,
                    ProductName: r.ProductName,
                    CategoryName: r.CategoryName,
                    UnitName: r.UnitName,
                    WarehouseName: r.WarehouseName,
                    CurrentStock: Qty(r.Quantity),
                    ReorderLevel: ReorderLevelOf(r.ProductId),
                    PurchasePrice: Money(r.UnitCost),
                    TotalValue: Money(r.Quantity * r.UnitCost)))
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<StockReportDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<StockReportDto>>.Failure(StockError));
        }
    }

    // ═══════════════════════════════════════════════
    // 4) أرصدة العملاء
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<CustomerBalanceReportDto>>> GetCustomerBalancesAsync(
        int? customerId = null, CancellationToken ct = default)
    {
        try
        {
            var rows = MockData.Customers
                .Where(c => !customerId.HasValue || c.Id == customerId.Value)
                .OrderBy(c => c.Name)
                .Select(c => new CustomerBalanceReportDto(
                    Id: c.Id,
                    Name: c.Name,
                    Phone: c.Phone,
                    GroupName: null,
                    CurrentBalance: Money(c.CurrentBalance),
                    CreditLimit: Money(c.CreditLimit),
                    BalanceStatus: c.CurrentBalance > 0m ? "مدين" : c.CurrentBalance < 0m ? "دائن" : "متوازن"))
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<CustomerBalanceReportDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<CustomerBalanceReportDto>>.Failure(CustomerBalancesError));
        }
    }

    // ═══════════════════════════════════════════════
    // 5) أرصدة الموردين
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<SupplierBalanceReportDto>>> GetSupplierBalancesAsync(
        int? supplierId = null, CancellationToken ct = default)
    {
        try
        {
            var postedPurchases = MockPurchasesData.Purchases.Where(p => p.Status == Posted).ToList();
            var postedReturns = MockReturnsData.PurchaseReturns.Where(r => r.Status == Posted).ToList();

            var rows = MockSuppliersData.Suppliers
                .Where(s => !supplierId.HasValue || s.Id == supplierId.Value)
                .OrderBy(s => s.Name)
                .Select(s =>
                {
                    var purchases = Money(postedPurchases.Where(p => p.SupplierId == s.Id).Sum(p => p.NetTotal));
                    var returns = Money(postedReturns.Where(r => r.SupplierId == s.Id).Sum(r => r.TotalAmount));
                    var payments = Money(MockSupplierPaymentsData.Payments
                        .Where(p => p.SupplierId == s.Id)
                        .Sum(p => p.Amount));
                    var debit = Money(payments + returns);

                    return new SupplierBalanceReportDto(
                        SupplierId: s.Id,
                        SupplierName: s.Name,
                        OpeningBalance: 0m,
                        TotalPurchases: purchases,
                        TotalReturns: returns,
                        TotalPayments: payments,
                        TotalDebit: debit,
                        CurrentBalance: Money(purchases - debit));
                })
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<SupplierBalanceReportDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<SupplierBalanceReportDto>>.Failure(SupplierBalancesError));
        }
    }

    // ═══════════════════════════════════════════════
    // 6) حركة المنتجات
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<ProductMovementReportDto>>> GetMovementsAsync(
        int? productId = null, int? warehouseId = null, DateTime? from = null, DateTime? to = null,
        CancellationToken ct = default)
    {
        if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            return Task.FromResult(Result<IReadOnlyList<ProductMovementReportDto>>.Failure(DateRangeError));

        try
        {
            var rows = ApplyRunningBalances(BuildMovements())
                .Where(x => !productId.HasValue || x.Movement.ProductId == productId.Value)
                .Where(x => !warehouseId.HasValue || x.Movement.WarehouseId == warehouseId.Value)
                .Where(x => !from.HasValue || x.Movement.Date >= from.Value.Date)
                .Where(x => !to.HasValue || x.Movement.Date <= to.Value.Date)
                .OrderBy(x => x.Movement.Date)
                .ThenBy(x => x.Movement.ReferenceNo, StringComparer.Ordinal)
                .Select(x => new ProductMovementReportDto(
                    Date: x.Movement.Date,
                    WarehouseName: x.Movement.WarehouseName,
                    MovementType: x.Movement.MovementType,
                    ReferenceNo: x.Movement.ReferenceNo,
                    QuantityChange: Qty(x.Movement.QuantityChange),
                    QuantityAfter: Qty(x.QuantityAfter)))
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<ProductMovementReportDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<ProductMovementReportDto>>.Failure(MovementsError));
        }
    }

    // ═══════════════════════════════════════════════
    // 7) المخزون المنخفض
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<LowStockReportDto>>> GetLowStockAsync(CancellationToken ct = default)
    {
        try
        {
            var rows = BuildStockRows()
                .Where(r => ReorderLevelOf(r.ProductId) > 0m && r.Quantity <= ReorderLevelOf(r.ProductId))
                .OrderBy(r => r.ProductName)
                .Select(r =>
                {
                    var reorder = ReorderLevelOf(r.ProductId);
                    var deficit = Money(reorder - r.Quantity);
                    var units = UnitsOf(r.ProductId);

                    var boxes = 0m;
                    var remainder = deficit;
                    if (units.Factor > 1m)
                    {
                        boxes = Math.Floor(deficit / units.Factor);
                        remainder = Money(deficit - boxes * units.Factor);
                    }

                    return new LowStockReportDto(
                        ProductId: r.ProductId,
                        ProductName: r.ProductName,
                        CategoryName: r.CategoryName,
                        WarehouseName: r.WarehouseName,
                        CurrentRetailQty: Qty(r.Quantity),
                        ReorderLevelRetailQty: Qty(reorder),
                        DeficitRetailQty: Qty(deficit),
                        SuggestedWholesaleBoxes: Qty(boxes),
                        SuggestedRetailRemainder: Qty(remainder),
                        WholesaleUnitName: units.Wholesale,
                        RetailUnitName: units.Retail,
                        ConversionFactor: Qty(units.Factor));
                })
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<LowStockReportDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<LowStockReportDto>>.Failure(LowStockError));
        }
    }

    // ═══════════════════════════════════════════════
    // 8) المنتجات منتهية الصلاحية
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<ExpiredProductDto>>> GetExpiredProductsAsync(
        int thresholdDays = 0, CancellationToken ct = default)
    {
        try
        {
            var today = DateTime.Today;
            var limit = today.AddDays(Math.Max(0, thresholdDays));

            var rows = MockInventoryData.Batches
                .Where(b => b.ExpiryDate.HasValue)
                .Where(b => b.ExpiryDate!.Value.ToDateTime(TimeOnly.MinValue).Date <= limit)
                .OrderBy(b => b.ExpiryDate)
                .Select(b =>
                {
                    var expiry = b.ExpiryDate!.Value.ToDateTime(TimeOnly.MinValue).Date;
                    return new ExpiredProductDto(
                        ProductId: b.ProductId,
                        ProductName: ProductNameOf(b.ProductId, b.ProductName),
                        CategoryName: CategoryNameOf(b.ProductId),
                        WarehouseName: b.WarehouseName,
                        CurrentStock: Qty(b.QuantityRemaining),
                        ExpirationDate: expiry,
                        DaysExpired: (today - expiry).Days);
                })
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<ExpiredProductDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<ExpiredProductDto>>.Failure(ExpiredError));
        }
    }

    // ═══════════════════════════════════════════════
    // 9) أرصدة المخزون
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<StockBalanceReportDto>>> GetStockBalanceAsync(
        int? warehouseId = null, CancellationToken ct = default)
    {
        try
        {
            var rows = BuildStockRows()
                .Where(r => !warehouseId.HasValue || r.WarehouseId == warehouseId.Value)
                .OrderBy(r => r.WarehouseName)
                .ThenBy(r => r.ProductName)
                .Select(r => new StockBalanceReportDto(
                    ProductId: r.ProductId,
                    ProductName: r.ProductName,
                    CategoryName: r.CategoryName,
                    WarehouseId: r.WarehouseId,
                    WarehouseName: r.WarehouseName,
                    CurrentStock: Qty(r.Quantity),
                    ReorderLevel: Qty(ReorderLevelOf(r.ProductId)),
                    Cost: Money(r.UnitCost),
                    TotalValue: Money(r.Quantity * r.UnitCost)))
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<StockBalanceReportDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<StockBalanceReportDto>>.Failure(StockBalanceError));
        }
    }

    // ═══════════════════════════════════════════════
    // 10) حركة المستودعات
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<WarehouseMovementReportDto>>> GetWarehouseMovementsAsync(
        int? warehouseId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            return Task.FromResult(Result<IReadOnlyList<WarehouseMovementReportDto>>.Failure(DateRangeError));

        try
        {
            var rows = ApplyRunningBalances(BuildMovements())
                .Where(x => !warehouseId.HasValue || x.Movement.WarehouseId == warehouseId.Value)
                .Where(x => !from.HasValue || x.Movement.Date >= from.Value.Date)
                .Where(x => !to.HasValue || x.Movement.Date <= to.Value.Date)
                .OrderByDescending(x => x.Movement.Date)
                .ThenBy(x => x.Movement.ProductName)
                .Select(x => new WarehouseMovementReportDto(
                    Date: x.Movement.Date,
                    ProductId: x.Movement.ProductId,
                    ProductName: x.Movement.ProductName,
                    WarehouseName: x.Movement.WarehouseName,
                    MovementType: x.Movement.MovementType,
                    QuantityChange: Qty(x.Movement.QuantityChange),
                    QuantityBefore: Qty(x.QuantityBefore),
                    QuantityAfter: Qty(x.QuantityAfter),
                    ReferenceType: x.Movement.ReferenceType,
                    ReferenceId: x.Movement.ReferenceId))
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<WarehouseMovementReportDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<WarehouseMovementReportDto>>.Failure(WarehouseMovementsError));
        }
    }

    // ═══════════════════════════════════════════════
    // 11) دفتر حركة المخزون التفصيلي
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<DetailedStockLedgerDto>>> GetDetailedStockLedgerAsync(
        int? productId = null, int? warehouseId = null, DateTime? from = null, DateTime? to = null,
        CancellationToken ct = default)
    {
        if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            return Task.FromResult(Result<IReadOnlyList<DetailedStockLedgerDto>>.Failure(DateRangeError));

        try
        {
            var rows = ApplyRunningBalances(BuildMovements())
                .Where(x => !productId.HasValue || x.Movement.ProductId == productId.Value)
                .Where(x => !warehouseId.HasValue || x.Movement.WarehouseId == warehouseId.Value)
                .Where(x => !from.HasValue || x.Movement.Date >= from.Value.Date)
                .Where(x => !to.HasValue || x.Movement.Date <= to.Value.Date)
                .OrderBy(x => x.Movement.Date)
                .ThenBy(x => x.Movement.ReferenceNo, StringComparer.Ordinal)
                .Select(x => new DetailedStockLedgerDto(
                    Date: x.Movement.Date,
                    ReferenceNo: x.Movement.ReferenceNo,
                    ReferenceType: x.Movement.ReferenceType,
                    MovementType: x.Movement.MovementType,
                    QuantityBefore: Qty(x.QuantityBefore),
                    QuantityChange: Qty(x.Movement.QuantityChange),
                    QuantityAfter: Qty(x.QuantityAfter),
                    UnitCost: Money(x.Movement.UnitCost),
                    TotalCost: Money(Math.Abs(x.Movement.QuantityChange) * x.Movement.UnitCost),
                    CreatedBy: x.Movement.CreatedBy))
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<DetailedStockLedgerDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<DetailedStockLedgerDto>>.Failure(DetailedLedgerError));
        }
    }

    // ═══════════════════════════════════════════════
    // 12) تقرير المرتجعات
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<ReturnsReportDto>>> GetReturnsReportAsync(
        string? returnType = null, DateTime? from = null, DateTime? to = null, int? productId = null,
        CancellationToken ct = default)
    {
        if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            return Task.FromResult(Result<IReadOnlyList<ReturnsReportDto>>.Failure(DateRangeError));

        try
        {
            var rows = new List<ReturnsReportDto>();

            if (string.IsNullOrWhiteSpace(returnType)
                || returnType.Equals("Sales", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var salesReturn in MockReturnsData.SalesReturns
                             .Where(r => r.Status == Posted)
                             .Where(r => !from.HasValue || r.ReturnDate.Date >= from.Value.Date)
                             .Where(r => !to.HasValue || r.ReturnDate.Date <= to.Value.Date))
                {
                    foreach (var item in salesReturn.Items
                                 .Where(i => !productId.HasValue || i.ProductId == productId.Value))
                    {
                        rows.Add(new ReturnsReportDto(
                            ReturnNo: salesReturn.ReturnNo.ToString(),
                            Date: salesReturn.ReturnDate.Date,
                            Type: SalesReturnType,
                            PartyName: salesReturn.CustomerName,
                            ProductName: item.ProductName,
                            Quantity: Qty(item.Quantity),
                            Amount: Money(item.LineTotal),
                            Reason: salesReturn.ReturnReason,
                            Status: PostedLabel(salesReturn.Status)));
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(returnType)
                || returnType.Equals("Purchases", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var purchaseReturn in MockReturnsData.PurchaseReturns
                             .Where(r => r.Status == Posted)
                             .Where(r => !from.HasValue || r.ReturnDate.ToDateTime(TimeOnly.MinValue).Date >= from.Value.Date)
                             .Where(r => !to.HasValue || r.ReturnDate.ToDateTime(TimeOnly.MinValue).Date <= to.Value.Date))
                {
                    var date = purchaseReturn.ReturnDate.ToDateTime(TimeOnly.MinValue).Date;

                    foreach (var item in purchaseReturn.Items
                                 .Where(i => !productId.HasValue || i.ProductId == productId.Value))
                    {
                        rows.Add(new ReturnsReportDto(
                            ReturnNo: purchaseReturn.ReturnNo.ToString(),
                            Date: date,
                            Type: PurchaseReturnType,
                            PartyName: purchaseReturn.SupplierName,
                            ProductName: item.ProductName,
                            Quantity: Qty(item.Quantity),
                            Amount: Money(item.LineTotal),
                            Reason: purchaseReturn.Notes,
                            Status: PostedLabel(purchaseReturn.Status)));
                    }
                }
            }

            return Task.FromResult(Result<IReadOnlyList<ReturnsReportDto>>.Success(
                rows.OrderByDescending(r => r.Date).ToList()));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<ReturnsReportDto>>.Failure(ReturnsError));
        }
    }

    // ═══════════════════════════════════════════════
    // 13) تقرير الأعمار
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<AgingReportDto>>> GetAgingReportAsync(
        string partyType = "Customers", int? partyId = null, CancellationToken ct = default)
    {
        try
        {
            var today = DateTime.Today;
            var rows = new List<AgingReportDto>();

            var isCustomers = string.IsNullOrWhiteSpace(partyType)
                              || partyType.Equals("Customers", StringComparison.OrdinalIgnoreCase)
                              || partyType.Equals("العملاء", StringComparison.OrdinalIgnoreCase);

            if (isCustomers)
            {
                foreach (var customer in MockData.Customers
                             .Where(c => c.IsActive)
                             .Where(c => !partyId.HasValue || c.Id == partyId.Value))
                {
                    var invoices = MockData.Invoices
                        .Where(i => i.CustomerId == customer.Id && i.Status == Posted)
                        .ToList();

                    rows.Add(BuildAgingRow(
                        customer.Name,
                        invoices.Select(i => (Date: i.InvoiceDate.Date, Due: i.RemainingAmount)),
                        DayNumber(today)));
                }
            }
            else
            {
                foreach (var supplier in MockSuppliersData.Suppliers
                             .Where(s => s.IsActive)
                             .Where(s => !partyId.HasValue || s.Id == partyId.Value))
                {
                    var invoices = MockPurchasesData.Purchases
                        .Where(p => p.SupplierId == supplier.Id && p.Status == Posted)
                        .Select(p => (
                            Date: p.InvoiceDate.ToDateTime(TimeOnly.MinValue).Date,
                            Due: p.RemainingAmount))
                        .ToList();

                    rows.Add(BuildAgingRow(supplier.Name, invoices, DayNumber(today)));
                }
            }

            return Task.FromResult(Result<IReadOnlyList<AgingReportDto>>.Success(
                rows.OrderByDescending(r => r.TotalDue).ToList()));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<AgingReportDto>>.Failure(AgingError));
        }
    }

    // ═══════════════════════════════════════════════
    // مخازن البيانات المساعدة — الأصناف والمخزون
    // ═══════════════════════════════════════════════

    private sealed record StockRow(
        int ProductId,
        string ProductName,
        string CategoryName,
        string UnitName,
        int WarehouseId,
        string WarehouseName,
        decimal Quantity,
        decimal UnitCost);

    /// <summary>حركة مخزنية واحدة مشتقة من مستند مرحّل.</summary>
    private sealed record StockMovement(
        DateTime Date,
        int ProductId,
        string ProductName,
        int WarehouseId,
        string WarehouseName,
        string MovementType,
        string ReferenceNo,
        string? ReferenceType,
        int? ReferenceId,
        decimal QuantityChange,
        decimal UnitCost,
        string? CreatedBy);

    private sealed record MovementBalance(
        StockMovement Movement,
        decimal QuantityBefore,
        decimal QuantityAfter);

    /// <summary>
    /// الأرصدة لكل صنف/مخزن: من دفعات المخزون عند وجودها، وإلا من رصيد الصنف الحالي في المخزن الرئيسي.
    /// </summary>
    private static List<StockRow> BuildStockRows()
    {
        var rows = new List<StockRow>();

        foreach (var product in MockData.Products)
        {
            var batches = MockInventoryData.Batches
                .Where(b => b.ProductId == product.Id)
                .ToList();

            if (batches.Count > 0)
            {
                foreach (var group in batches.GroupBy(b => b.WarehouseId))
                {
                    var quantity = group.Sum(b => b.QuantityRemaining);
                    var value = group.Sum(b => b.QuantityRemaining * b.UnitCost);

                    rows.Add(new StockRow(
                        product.Id,
                        product.Name,
                        CategoryNameOf(product.Id),
                        BaseUnitNameOf(product.Id),
                        group.Key,
                        WarehouseNameOf(group.Key),
                        Qty(quantity),
                        Money(quantity > 0m ? value / quantity : group.Max(b => b.UnitCost))));
                }
            }
            else
            {
                rows.Add(new StockRow(
                    product.Id,
                    product.Name,
                    CategoryNameOf(product.Id),
                    BaseUnitNameOf(product.Id),
                    1,
                    WarehouseNameOf(1),
                    Qty(product.CurrentStock),
                    Money(BaseUnitCostOf(product.Id))));
            }
        }

        return rows;
    }

    /// <summary>الحركات المخزنية من فواتير البيع/الشراء والمرتجعات والتسويات المرحّلة.</summary>
    private static List<StockMovement> BuildMovements()
    {
        var movements = new List<StockMovement>();
        var createdBy = UserNameOf(1);

        foreach (var invoice in MockData.Invoices.Where(i => i.Status == Posted))
        {
            foreach (var line in invoice.Items)
            {
                movements.Add(new StockMovement(
                    invoice.InvoiceDate.Date,
                    line.ProductId,
                    ProductNameOf(line.ProductId, line.ProductName),
                    invoice.WarehouseId,
                    WarehouseNameOf(invoice.WarehouseId),
                    SalesMovement,
                    invoice.InvoiceNo.ToString(),
                    "SalesInvoice",
                    invoice.Id,
                    -line.Quantity,
                    Money(UnitCostOf(line.ProductId, line.ProductUnitId, line.UnitCost)),
                    createdBy));
            }
        }

        foreach (var invoice in MockPurchasesData.Purchases.Where(p => p.Status == Posted))
        {
            var date = invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue).Date;

            foreach (var line in invoice.Items)
            {
                movements.Add(new StockMovement(
                    date,
                    line.ProductId,
                    ProductNameOf(line.ProductId, line.ProductName),
                    invoice.WarehouseId,
                    WarehouseNameOf(invoice.WarehouseId),
                    PurchaseMovement,
                    invoice.InvoiceNo.ToString(),
                    "PurchaseInvoice",
                    invoice.Id,
                    line.Quantity,
                    Money(line.UnitPrice),
                    createdBy));
            }
        }

        foreach (var salesReturn in MockReturnsData.SalesReturns.Where(r => r.Status == Posted))
        {
            foreach (var item in salesReturn.Items)
            {
                movements.Add(new StockMovement(
                    salesReturn.ReturnDate.Date,
                    item.ProductId,
                    ProductNameOf(item.ProductId, item.ProductName),
                    salesReturn.WarehouseId,
                    WarehouseNameOf(salesReturn.WarehouseId),
                    SalesReturnMovement,
                    salesReturn.ReturnNo.ToString(),
                    "SalesReturn",
                    salesReturn.Id,
                    item.Quantity,
                    Money(UnitCostOf(item.ProductId, item.ProductUnitId, 0m)),
                    createdBy));
            }
        }

        foreach (var purchaseReturn in MockReturnsData.PurchaseReturns.Where(r => r.Status == Posted))
        {
            var date = purchaseReturn.ReturnDate.ToDateTime(TimeOnly.MinValue).Date;

            foreach (var item in purchaseReturn.Items)
            {
                movements.Add(new StockMovement(
                    date,
                    item.ProductId,
                    ProductNameOf(item.ProductId, item.ProductName),
                    purchaseReturn.WarehouseId,
                    WarehouseNameOf(purchaseReturn.WarehouseId),
                    PurchaseReturnMovement,
                    purchaseReturn.ReturnNo.ToString(),
                    "PurchaseReturn",
                    purchaseReturn.Id,
                    -item.Quantity,
                    Money(item.UnitCost),
                    createdBy));
            }
        }

        foreach (var adjustment in MockInventoryData.Adjustments.Where(a => a.Status == Posted))
        {
            foreach (var line in adjustment.Lines)
            {
                var unit = MockData.ProductUnits.FirstOrDefault(u => u.Id == line.ProductUnitId);
                var productId = unit?.ProductId ?? 0;
                if (productId == 0)
                    continue;

                var change = Qty(line.ActualQuantity - line.ExpectedQuantity);

                movements.Add(new StockMovement(
                    adjustment.CreatedAt.Date,
                    productId,
                    ProductNameOf(productId, line.ProductUnitName),
                    adjustment.WarehouseId,
                    WarehouseNameOf(adjustment.WarehouseId),
                    AdjustmentMovement,
                    adjustment.AdjustmentNo,
                    "InventoryAdjustment",
                    adjustment.Id,
                    change,
                    Money(line.UnitCost),
                    createdBy));
            }
        }

        return movements
            .OrderBy(m => m.Date)
            .ThenBy(m => m.ReferenceNo, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>الرصيد قبل/بعد لكل حركة، محسوباً لكل (صنف، مخزن) على كل الحركات قبل الفلترة.</summary>
    private static List<MovementBalance> ApplyRunningBalances(List<StockMovement> movements)
    {
        var balances = new Dictionary<(int ProductId, int WarehouseId), decimal>();
        var result = new List<MovementBalance>(movements.Count);

        foreach (var movement in movements)
        {
            var key = (movement.ProductId, movement.WarehouseId);
            var before = balances.GetValueOrDefault(key, 0m);
            var after = Qty(before + movement.QuantityChange);
            balances[key] = after;

            result.Add(new MovementBalance(movement, Qty(before), after));
        }

        return result;
    }

    private static AgingReportDto BuildAgingRow(
        string name, IEnumerable<(DateTime Date, decimal Due)> invoices, int todayNumber)
    {
        var items = invoices.ToList();
        var totalDue = Money(items.Sum(i => i.Due));

        decimal current = 0m, days1To30 = 0m, days31To60 = 0m, days61To90 = 0m, days90Plus = 0m;

        foreach (var invoice in items.Where(i => i.Due > 0m))
        {
            var ageDays = todayNumber - DayNumber(invoice.Date);

            if (ageDays <= 0) current += invoice.Due;
            else if (ageDays <= 30) days1To30 += invoice.Due;
            else if (ageDays <= 60) days31To60 += invoice.Due;
            else if (ageDays <= 90) days61To90 += invoice.Due;
            else days90Plus += invoice.Due;
        }

        return new AgingReportDto(
            name,
            totalDue,
            Money(current),
            Money(days1To30),
            Money(days31To60),
            Money(days61To90),
            Money(days90Plus),
            totalDue);
    }

    // ═══════════════════════════════════════════════
    // مخازن البيانات المساعدة — الأصناف والوحدات والمستخدمون
    // ═══════════════════════════════════════════════

    private static string ProductNameOf(int productId, string? fallback)
    {
        var product = MockData.Products.FirstOrDefault(p => p.Id == productId);
        if (product is not null)
            return product.Name;

        return string.IsNullOrWhiteSpace(fallback) ? $"صنف {productId}" : fallback!;
    }

    private static string CategoryNameOf(int productId)
    {
        var product = MockData.Products.FirstOrDefault(p => p.Id == productId);
        if (product is null || string.IsNullOrWhiteSpace(product.CategoryName))
            return Uncategorized;

        return product.CategoryName!;
    }

    private static decimal ReorderLevelOf(int productId)
        => MockData.Products.FirstOrDefault(p => p.Id == productId)?.ReorderLevel ?? 0m;

    private static string WarehouseNameOf(int warehouseId)
    {
        var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == warehouseId);
        return string.IsNullOrWhiteSpace(warehouse?.Name) ? DefaultWarehouseName : warehouse!.Name;
    }

    private static string BaseUnitNameOf(int productId)
        => MockData.ProductUnits.FirstOrDefault(u => u.ProductId == productId && u.IsBaseUnit)?.UnitName
           ?? DefaultUnitName;

    private static decimal BaseUnitCostOf(int productId)
        => MockData.ProductUnits.FirstOrDefault(u => u.ProductId == productId && u.IsBaseUnit)?.CostPrice ?? 0m;

    private static (string Retail, string Wholesale, decimal Factor) UnitsOf(int productId)
    {
        var units = MockData.ProductUnits.Where(u => u.ProductId == productId).ToList();
        var baseUnit = units.FirstOrDefault(u => u.IsBaseUnit) ?? units.FirstOrDefault();
        var wholesale = units
            .Where(u => !u.IsBaseUnit && u.ConversionFactor > 1m)
            .OrderBy(u => u.ConversionFactor)
            .FirstOrDefault();

        return (baseUnit?.UnitName ?? DefaultUnitName,
                wholesale?.UnitName ?? NoValue,
                wholesale?.ConversionFactor ?? 1m);
    }

    private static decimal UnitCostOf(int productId, int productUnitId, decimal lineCost)
    {
        if (lineCost > 0m)
            return lineCost;

        var unit = MockData.ProductUnits.FirstOrDefault(u => u.Id == productUnitId);
        if (unit is not null && unit.ProductId == productId)
            return unit.CostPrice;

        return BaseUnitCostOf(productId);
    }

    private static string? UserNameOf(int userId)
    {
        var user = MockSecurityData.Users.FirstOrDefault(u => u.Id == userId);
        return string.IsNullOrWhiteSpace(user?.UserName) ? SystemUser : user!.UserName;
    }

    private static string PostedLabel(int status) => status == Posted ? "مرحل" : status == 1 ? "مسودة" : "ملغي";

    private static int DayNumber(DateTime date) => DateOnly.FromDateTime(date).DayNumber;

    private static decimal Money(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);

    private static decimal Qty(decimal value) => Math.Round(value, 3, MidpointRounding.AwayFromZero);
}
