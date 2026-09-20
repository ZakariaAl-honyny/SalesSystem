using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IReportApiService
{
    Task<Result<IReadOnlyList<SalesReportDto>>> GetSalesAsync(DateTime? from = null, DateTime? to = null, int? customerId = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<PurchaseReportDto>>> GetPurchasesAsync(DateTime? from = null, DateTime? to = null, int? supplierId = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<StockReportDto>>> GetStockReportAsync(int? warehouseId = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<CustomerBalanceReportDto>>> GetCustomerBalancesAsync(int? customerId = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<SupplierBalanceReportDto>>> GetSupplierBalancesAsync(int? supplierId = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ProductMovementReportDto>>> GetMovementsAsync(int? productId = null, int? warehouseId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<LowStockReportDto>>> GetLowStockAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<ExpiredProductDto>>> GetExpiredProductsAsync(int thresholdDays = 0, CancellationToken ct = default);
    Task<Result<IReadOnlyList<StockBalanceReportDto>>> GetStockBalanceAsync(int? warehouseId = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<WarehouseMovementReportDto>>> GetWarehouseMovementsAsync(int? warehouseId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<DetailedStockLedgerDto>>> GetDetailedStockLedgerAsync(int? productId = null, int? warehouseId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ReturnsReportDto>>> GetReturnsReportAsync(string? returnType = null, DateTime? from = null, DateTime? to = null, int? productId = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<AgingReportDto>>> GetAgingReportAsync(string partyType = "Customers", int? partyId = null, CancellationToken ct = default);
}
