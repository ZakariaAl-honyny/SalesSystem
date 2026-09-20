using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IInventoryAdjustmentApiService
{
    Task<Result<IReadOnlyList<InventoryAdjustmentDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<InventoryAdjustmentDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<InventoryAdjustmentDto>> CreateAsync(CreateInventoryAdjustmentRequest request, CancellationToken ct = default);
    Task<Result<InventoryAdjustmentDto>> PostAsync(int id, CancellationToken ct = default);
    Task<Result> CancelAsync(int id, CancellationToken ct = default);
    Task<Result<InventoryAdjustmentDto>> AddLineAsync(int id, AddInventoryAdjustmentLineRequest request, CancellationToken ct = default);
}
