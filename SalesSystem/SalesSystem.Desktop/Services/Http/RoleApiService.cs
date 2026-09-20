using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class RoleApiService : IRoleApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/roles";

    public RoleApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<RoleDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var path = $"{BasePath}?includeInactive={includeInactive.ToString().ToLower()}";
        return await _http.GetListAsync<RoleDto>(path, ct);
    }

    public async Task<Result<RoleDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetAsync<RoleDto>($"{BasePath}/{id}", ct);
    }

    public async Task<Result<RoleDto>> CreateAsync(CreateRoleRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<RoleDto>(BasePath, request, ct);
    }

    public async Task<Result<RoleDto>> UpdateAsync(int id, UpdateRoleRequest request, CancellationToken ct = default)
    {
        return await _http.PutAsync<RoleDto>($"{BasePath}/{id}", request, ct);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        return await _http.DeleteAsync($"{BasePath}/{id}", ct);
    }
}
