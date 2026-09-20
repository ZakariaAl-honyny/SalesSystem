using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class SystemAccountMappingApiService : ISystemAccountMappingApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/system-account-mappings";

    public SystemAccountMappingApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<SystemAccountMappingDto>>> GetAllAsync(CancellationToken ct = default)
    {
        return await _http.GetListAsync<SystemAccountMappingDto>(BasePath, ct);
    }

    public async Task<Result<SystemAccountMappingDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetAsync<SystemAccountMappingDto>($"{BasePath}/{id}", ct);
    }

    public async Task<Result> UpdateAsync(int id, UpdateSystemAccountMappingRequest request, CancellationToken ct = default)
    {
        return await _http.PutAsync($"{BasePath}/{id}", request, ct);
    }
}
