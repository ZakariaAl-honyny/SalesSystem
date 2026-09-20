using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class AuditLogApiService : IAuditLogApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/audit-logs";

    public AuditLogApiService(HttpClientService http) => _http = http;

    public async Task<Result<PaginatedResult<AuditLogDto>>> QueryAsync(AuditLogQuery query, CancellationToken ct = default)
    {
        return await _http.PostAsync<PaginatedResult<AuditLogDto>>($"{BasePath}/query", query, ct);
    }

    public async Task<Result<IReadOnlyList<AuditLogDto>>> GetLoginHistoryAsync(int? userId = null, CancellationToken ct = default)
    {
        var path = $"{BasePath}/login-history";
        if (userId.HasValue) path += $"?userId={userId.Value}";
        return await _http.GetListAsync<AuditLogDto>(path, ct);
    }
}
