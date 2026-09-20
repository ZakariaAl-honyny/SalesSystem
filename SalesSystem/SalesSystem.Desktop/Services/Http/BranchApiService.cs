using System.Web;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class BranchApiService : IBranchApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/branches";

    public BranchApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<BranchDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["includeInactive"] = includeInactive.ToString().ToLower();
        return await _http.GetListAsync<BranchDto>($"{BasePath}?{query}", ct);
    }
}
