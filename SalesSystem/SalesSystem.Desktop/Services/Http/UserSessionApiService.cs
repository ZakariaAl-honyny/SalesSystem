using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class UserSessionApiService : IUserSessionApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/sessions";

    public UserSessionApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<UserSessionDto>>> GetAllAsync(int? userId = null, bool includeRevoked = false, CancellationToken ct = default)
    {
        var query = new List<string> { $"includeRevoked={includeRevoked.ToString().ToLower()}" };
        if (userId.HasValue) query.Add($"userId={userId.Value}");
        var path = $"{BasePath}?{string.Join("&", query)}";
        return await _http.GetListAsync<UserSessionDto>(path, ct);
    }

    public async Task<Result> RevokeAsync(long sessionId, CancellationToken ct = default)
    {
        return await _http.PostAsync($"{BasePath}/{sessionId}/revoke", null, ct);
    }
}
