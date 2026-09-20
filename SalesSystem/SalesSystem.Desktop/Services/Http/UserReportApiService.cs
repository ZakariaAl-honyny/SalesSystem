using System.Web;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class UserReportApiService : IUserReportApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/reports/users";

    public UserReportApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<LoginHistoryDto>>> GetLoginHistoryAsync(int? userId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        if (userId.HasValue) query["userId"] = userId.Value.ToString();
        return await _http.GetListAsync<LoginHistoryDto>($"{BasePath}/login-history?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<UserActivityReportDto>>> GetUserActivityAsync(int? userId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        if (userId.HasValue) query["userId"] = userId.Value.ToString();
        return await _http.GetListAsync<UserActivityReportDto>($"{BasePath}/activity?{query}", ct);
    }
}
