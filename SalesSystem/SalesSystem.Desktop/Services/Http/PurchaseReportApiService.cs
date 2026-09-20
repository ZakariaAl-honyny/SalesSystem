using System.Web;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class PurchaseReportApiService : IPurchaseReportApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/reports/purchases";

    public PurchaseReportApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<PurchasesBySupplierDto>>> GetPurchasesBySupplierAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<PurchasesBySupplierDto>($"{BasePath}/by-supplier?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<PurchasesByProductDto>>> GetPurchasesByProductAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<PurchasesByProductDto>($"{BasePath}/by-product?{query}", ct);
    }
}
