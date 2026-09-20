using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IInventoryApiService
{
    // Stock
    Task<Result<IReadOnlyList<WarehouseStockDto>>> GetStockByWarehouseAsync(int warehouseId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<WarehouseStockDto>>> GetStockByProductAsync(int productId, CancellationToken ct = default);

    // Transactions
    Task<Result<IReadOnlyList<InventoryTransactionDto>>> GetMovementsAsync(int? productId = null, int? warehouseId = null, int? movementType = null, int page = 1, int pageSize = 50, CancellationToken ct = default);
    Task<Result<InventoryTransactionDto>> CreateInventoryTransactionAsync(CreateInventoryTransactionRequest request, CancellationToken ct = default);
    Task<Result<InventoryTransactionDto>> PostInventoryTransactionAsync(int id, CancellationToken ct = default);
    Task<Result<InventoryTransactionDto>> CancelInventoryTransactionAsync(int id, CancellationToken ct = default);
    Task<Result<InventoryTransactionDto>> GetInventoryTransactionByIdAsync(int id, CancellationToken ct = default);
}

