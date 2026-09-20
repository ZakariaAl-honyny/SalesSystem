using System.Web;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class ReportApiService : IReportApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/reports";

    public ReportApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<SalesReportDto>>> GetSalesAsync(DateTime? from, DateTime? to, int? customerId, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (from.HasValue) query["dateFrom"] = from.Value.ToString("o");
        if (to.HasValue) query["dateTo"] = to.Value.ToString("o");
        if (customerId.HasValue) query["customerId"] = customerId.Value.ToString();
        return await _http.GetListAsync<SalesReportDto>($"{BasePath}/sales?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<PurchaseReportDto>>> GetPurchasesAsync(DateTime? from, DateTime? to, int? supplierId, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (from.HasValue) query["dateFrom"] = from.Value.ToString("o");
        if (to.HasValue) query["dateTo"] = to.Value.ToString("o");
        if (supplierId.HasValue) query["supplierId"] = supplierId.Value.ToString();
        return await _http.GetListAsync<PurchaseReportDto>($"{BasePath}/purchases?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<StockReportDto>>> GetStockReportAsync(int? warehouseId, CancellationToken ct = default)
    {
        var path = warehouseId.HasValue ? $"{BasePath}/stock?warehouseId={warehouseId}" : $"{BasePath}/stock";
        return await _http.GetListAsync<StockReportDto>(path, ct);
    }

    public async Task<Result<IReadOnlyList<CustomerBalanceReportDto>>> GetCustomerBalancesAsync(int? customerId, CancellationToken ct = default)
    {
        var path = customerId.HasValue ? $"{BasePath}/customer-balances?customerId={customerId}" : $"{BasePath}/customer-balances";
        return await _http.GetListAsync<CustomerBalanceReportDto>(path, ct);
    }

    public async Task<Result<IReadOnlyList<SupplierBalanceReportDto>>> GetSupplierBalancesAsync(int? supplierId, CancellationToken ct = default)
    {
        var path = supplierId.HasValue ? $"{BasePath}/supplier-balances?supplierId={supplierId}" : $"{BasePath}/supplier-balances";
        return await _http.GetListAsync<SupplierBalanceReportDto>(path, ct);
    }

    public async Task<Result<IReadOnlyList<ProductMovementReportDto>>> GetMovementsAsync(int? productId, int? warehouseId, DateTime? from, DateTime? to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (productId.HasValue) query["productId"] = productId.Value.ToString();
        if (warehouseId.HasValue) query["warehouseId"] = warehouseId.Value.ToString();
        if (from.HasValue) query["dateFrom"] = from.Value.ToString("o");
        if (to.HasValue) query["dateTo"] = to.Value.ToString("o");
        return await _http.GetListAsync<ProductMovementReportDto>($"{BasePath}/product-movements?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<LowStockReportDto>>> GetLowStockAsync(CancellationToken ct = default)
    {
        return await _http.GetListAsync<LowStockReportDto>($"{BasePath}/low-stock", ct);
    }

    public async Task<Result<IReadOnlyList<ExpiredProductDto>>> GetExpiredProductsAsync(int thresholdDays, CancellationToken ct = default)
    {
        return await _http.GetListAsync<ExpiredProductDto>($"{BasePath}/expired-products?thresholdDays={thresholdDays}", ct);
    }

    public async Task<Result<IReadOnlyList<StockBalanceReportDto>>> GetStockBalanceAsync(int? warehouseId, CancellationToken ct = default)
    {
        var path = warehouseId.HasValue ? $"{BasePath}/stock-balance?warehouseId={warehouseId}" : $"{BasePath}/stock-balance";
        return await _http.GetListAsync<StockBalanceReportDto>(path, ct);
    }

    public async Task<Result<IReadOnlyList<WarehouseMovementReportDto>>> GetWarehouseMovementsAsync(int? warehouseId, DateTime? from, DateTime? to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (warehouseId.HasValue) query["warehouseId"] = warehouseId.Value.ToString();
        if (from.HasValue) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to.HasValue) query["to"] = to.Value.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<WarehouseMovementReportDto>($"{BasePath}/warehouse-movements?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<DetailedStockLedgerDto>>> GetDetailedStockLedgerAsync(int? productId, int? warehouseId, DateTime? from, DateTime? to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (productId.HasValue) query["productId"] = productId.Value.ToString();
        if (warehouseId.HasValue) query["warehouseId"] = warehouseId.Value.ToString();
        if (from.HasValue) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to.HasValue) query["to"] = to.Value.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<DetailedStockLedgerDto>($"{BasePath}/detailed-stock-ledger?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<ReturnsReportDto>>> GetReturnsReportAsync(string? returnType, DateTime? from, DateTime? to, int? productId, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrEmpty(returnType)) query["returnType"] = returnType;
        if (from.HasValue) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to.HasValue) query["to"] = to.Value.ToString("yyyy-MM-dd");
        if (productId.HasValue) query["productId"] = productId.Value.ToString();
        return await _http.GetListAsync<ReturnsReportDto>($"{BasePath}/returns?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<AgingReportDto>>> GetAgingReportAsync(string partyType, int? partyId, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["partyType"] = partyType;
        if (partyId.HasValue) query["partyId"] = partyId.Value.ToString();
        return await _http.GetListAsync<AgingReportDto>($"{BasePath}/aging?{query}", ct);
    }
}
