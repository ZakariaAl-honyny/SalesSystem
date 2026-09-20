using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class InventoryAdjustmentApiService : IInventoryAdjustmentApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/inventory-adjustments";

    public InventoryAdjustmentApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<InventoryAdjustmentDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var path = $"{BasePath}?includeInactive={includeInactive.ToString().ToLower()}&pageSize=1000";
        return await _http.GetListAsync<InventoryAdjustmentDto>(path, ct);
    }

    public async Task<Result<InventoryAdjustmentDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetAsync<InventoryAdjustmentDto>($"{BasePath}/{id}", ct);
    }

    public async Task<Result<InventoryAdjustmentDto>> CreateAsync(CreateInventoryAdjustmentRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<InventoryAdjustmentDto>(BasePath, request, ct);
    }

    public async Task<Result<InventoryAdjustmentDto>> PostAsync(int id, CancellationToken ct = default)
    {
        return await _http.PostAsync<InventoryAdjustmentDto>($"{BasePath}/{id}/post", null, ct);
    }

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        return await _http.PostAsync($"{BasePath}/{id}/cancel", null, ct);
    }

    public async Task<Result<InventoryAdjustmentDto>> AddLineAsync(int id, AddInventoryAdjustmentLineRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<InventoryAdjustmentDto>($"{BasePath}/{id}/lines", request, ct);
    }
}
