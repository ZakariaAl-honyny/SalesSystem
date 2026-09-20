using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IInventoryBatchApiService
{
    Task<Result<IReadOnlyList<InventoryBatchDto>>> GetByProductAsync(int productId, int? warehouseId = null, CancellationToken ct = default);
    Task<Result<InventoryBatchDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<InventoryBatchDto>> CreateAsync(CreateInventoryBatchRequest request, CancellationToken ct = default);
    Task<Result> DeactivateAsync(int id, CancellationToken ct = default);
}
