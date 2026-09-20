using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class LogsApiService : ILogsApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/logs";

    public LogsApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<SystemLogDto>>> GetAllAsync(byte? level = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        var query = new List<string>();
        if (level.HasValue) query.Add($"level={level.Value}");
        if (from.HasValue) query.Add($"from={from.Value:yyyy-MM-dd}");
        if (to.HasValue) query.Add($"to={to.Value:yyyy-MM-dd}");
        var path = BasePath;
        if (query.Any()) path += "?" + string.Join("&", query);
        return await _http.GetListAsync<SystemLogDto>(path, ct);
    }
}
