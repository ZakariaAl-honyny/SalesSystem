using System.Web;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class SalesReportApiService : ISalesReportApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/reports/sales";

    public SalesReportApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<SalesByCustomerDto>>> GetSalesByCustomerAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<SalesByCustomerDto>($"{BasePath}/by-customer?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<SalesByProductDto>>> GetSalesByProductAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<SalesByProductDto>($"{BasePath}/by-product?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<SalesByCategoryDto>>> GetSalesByCategoryAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<SalesByCategoryDto>($"{BasePath}/by-category?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<DailySalesSummaryDto>>> GetDailySalesSummaryAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<DailySalesSummaryDto>($"{BasePath}/daily-summary?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<ProductProfitabilityDto>>> GetProductProfitabilityAsync(int? productId, int? categoryId, DateTime? from, DateTime? to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (productId.HasValue) query["productId"] = productId.Value.ToString();
        if (categoryId.HasValue) query["categoryId"] = categoryId.Value.ToString();
        if (from.HasValue) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to.HasValue) query["to"] = to.Value.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<ProductProfitabilityDto>($"{BasePath}/product-profitability?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<ProfitByCustomerDto>>> GetProfitByCustomerAsync(int? customerId, DateTime? from, DateTime? to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (customerId.HasValue) query["customerId"] = customerId.Value.ToString();
        if (from.HasValue) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to.HasValue) query["to"] = to.Value.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<ProfitByCustomerDto>($"{BasePath}/profit-by-customer?{query}", ct);
    }
}
