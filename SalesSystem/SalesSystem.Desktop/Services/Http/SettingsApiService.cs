using SalesSystem.Common;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Http;

public sealed class SettingsApiService : ISettingsApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/settings";
    private const string SystemPath = "api/v1/settings/system";

    public SettingsApiService(HttpClientService http) => _http = http;

    public async Task<Result<dynamic>> GetSettingsAsync(CancellationToken ct = default)
    {
        return await _http.GetAsync<dynamic>(BasePath, ct);
    }

    public async Task<Result> UpdateSettingsAsync(object settings, CancellationToken ct = default)
    {
        return await _http.PutAsync<object>(BasePath, settings, ct);
    }

    public async Task<Result<Dictionary<string, string>>> GetAllSystemSettingsAsync(CancellationToken ct = default)
    {
        return await _http.GetAsync<Dictionary<string, string>>(SystemPath, ct);
    }

    public async Task<Result> UpdateSystemSettingsAsync(Dictionary<string, string> settings, CancellationToken ct = default)
    {
        return await _http.PutAsync<object>(SystemPath, settings, ct);
    }

    public void RefreshCache()
    {
        // Cache invalidation — currently no cache in Desktop client
    }
}
