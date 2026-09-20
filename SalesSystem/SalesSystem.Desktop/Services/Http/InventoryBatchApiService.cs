using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class InventoryBatchApiService : IInventoryBatchApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/inventory-batches";

    public InventoryBatchApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<InventoryBatchDto>>> GetByProductAsync(int productId, int? warehouseId = null, CancellationToken ct = default)
    {
        var path = $"{BasePath}/by-product/{productId}";
        if (warehouseId.HasValue)
            path += $"?warehouseId={warehouseId.Value}";
        return await _http.GetListAsync<InventoryBatchDto>(path, ct);
    }

    public async Task<Result<InventoryBatchDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetAsync<InventoryBatchDto>($"{BasePath}/{id}", ct);
    }

    public async Task<Result<InventoryBatchDto>> CreateAsync(CreateInventoryBatchRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<InventoryBatchDto>(BasePath, request, ct);
    }

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        return await _http.PostAsync($"{BasePath}/{id}/deactivate", null, ct);
    }
}
