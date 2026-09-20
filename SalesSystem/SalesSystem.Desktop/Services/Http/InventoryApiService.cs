using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class InventoryApiService : IInventoryApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/inventory";

    public InventoryApiService(HttpClientService http) => _http = http;

    // ─── Stock ────────────────────────────────────────────────
    public async Task<Result<IReadOnlyList<WarehouseStockDto>>> GetStockByWarehouseAsync(int warehouseId, CancellationToken ct = default)
    {
        return await _http.GetListAsync<WarehouseStockDto>($"{BasePath}/warehouse/{warehouseId}", ct);
    }

    public async Task<Result<IReadOnlyList<WarehouseStockDto>>> GetStockByProductAsync(int productId, CancellationToken ct = default)
    {
        return await _http.GetListAsync<WarehouseStockDto>($"{BasePath}/product/{productId}", ct);
    }

    // ─── Transactions ─────────────────────────────────────────
    public async Task<Result<IReadOnlyList<InventoryTransactionDto>>> GetMovementsAsync(int? productId = null, int? warehouseId = null, int? movementType = null, int page = 1, int pageSize = 50, CancellationToken ct = default)
    {
        var query = $"{BasePath}/movements?page={page}&pageSize={pageSize}";
        if (productId.HasValue) query += $"&productId={productId.Value}";
        if (warehouseId.HasValue) query += $"&warehouseId={warehouseId.Value}";
        if (movementType.HasValue) query += $"&movementType={movementType.Value}";
        return await _http.GetListAsync<InventoryTransactionDto>(query, ct);
    }

    public async Task<Result<InventoryTransactionDto>> CreateInventoryTransactionAsync(CreateInventoryTransactionRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<InventoryTransactionDto>($"{BasePath}/transactions", request, ct);
    }

    public async Task<Result<InventoryTransactionDto>> PostInventoryTransactionAsync(int id, CancellationToken ct = default)
    {
        return await _http.PostAsync<InventoryTransactionDto>($"{BasePath}/transactions/{id}/post", null, ct);
    }

    public async Task<Result<InventoryTransactionDto>> CancelInventoryTransactionAsync(int id, CancellationToken ct = default)
    {
        return await _http.PostAsync<InventoryTransactionDto>($"{BasePath}/transactions/{id}/cancel", null, ct);
    }

    public async Task<Result<InventoryTransactionDto>> GetInventoryTransactionByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetAsync<InventoryTransactionDto>($"{BasePath}/transactions/{id}", ct);
    }
}

