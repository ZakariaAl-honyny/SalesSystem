using System.Web;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class CashBoxReportApiService : ICashBoxReportApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/reports/cash-boxes";

    public CashBoxReportApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<CashBoxSummaryDto>>> GetCashBoxSummaryAsync(DateTime? asOfDate, CancellationToken ct = default)
    {
        var path = asOfDate.HasValue
            ? $"{BasePath}/summary?asOfDate={asOfDate.Value:yyyy-MM-dd}"
            : $"{BasePath}/summary";
        return await _http.GetListAsync<CashBoxSummaryDto>(path, ct);
    }
}
