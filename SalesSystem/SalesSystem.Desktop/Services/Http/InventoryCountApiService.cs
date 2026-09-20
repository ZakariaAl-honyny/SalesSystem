using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class InventoryCountApiService : IInventoryCountApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/inventory-counts";

    public InventoryCountApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<InventoryCountDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var path = $"{BasePath}?includeInactive={includeInactive.ToString().ToLower()}&pageSize=1000";
        return await _http.GetListAsync<InventoryCountDto>(path, ct);
    }

    public async Task<Result<InventoryCountDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetAsync<InventoryCountDto>($"{BasePath}/{id}", ct);
    }

    public async Task<Result<InventoryCountDto>> CreateAsync(CreateInventoryCountRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<InventoryCountDto>(BasePath, request, ct);
    }

    public async Task<Result<InventoryCountDto>> PostAsync(int id, CancellationToken ct = default)
    {
        return await _http.PostAsync<InventoryCountDto>($"{BasePath}/{id}/post", null, ct);
    }

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        return await _http.PostAsync($"{BasePath}/{id}/cancel", null, ct);
    }

    public async Task<Result<InventoryCountDto>> AddLineAsync(int id, AddInventoryCountLineRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<InventoryCountDto>($"{BasePath}/{id}/lines", request, ct);
    }
}
