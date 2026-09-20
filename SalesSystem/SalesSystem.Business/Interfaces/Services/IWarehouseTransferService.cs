using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Business.Interfaces.Services;

/// <summary>
/// Dedicated service for warehouse transfer operations.
/// Handles Draft→Posted→Cancelled lifecycle with stock movement.
/// </summary>
public interface IWarehouseTransferService
{
    Task<Result<WarehouseTransferDto>> CreateAsync(CreateWarehouseTransferRequest request, int userId, CancellationToken ct);
    Task<Result<WarehouseTransferDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<PagedResult<WarehouseTransferDto>>> GetAllAsync(int? sourceWarehouseId, int? destinationWarehouseId, int page, int pageSize, CancellationToken ct);
    Task<Result<WarehouseTransferDto>> PostAsync(int id, int userId, CancellationToken ct);
    Task<Result<WarehouseTransferDto>> CancelAsync(int id, int userId, CancellationToken ct);
}
