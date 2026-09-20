using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class UnitApiService : IUnitApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/units";

    public UnitApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<UnitDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var path = $"{BasePath}?includeInactive={includeInactive.ToString().ToLower()}";
        return await _http.GetListAsync<UnitDto>(path, ct);
    }

    public async Task<Result<UnitDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetAsync<UnitDto>($"{BasePath}/{id}", ct);
    }

    public async Task<Result<UnitDto>> CreateAsync(CreateUnitRequest r, CancellationToken ct = default)
    {
        return await _http.PostAsync<UnitDto>(BasePath, r, ct);
    }

    public async Task<Result<UnitDto>> UpdateAsync(int id, UpdateUnitRequest r, CancellationToken ct = default)
    {
        return await _http.PutAsync<UnitDto>($"{BasePath}/{id}", r, ct);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        return await _http.DeleteAsync($"{BasePath}/{id}", ct);
    }
}

