using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IInventoryCountApiService
{
    Task<Result<IReadOnlyList<InventoryCountDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<InventoryCountDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<InventoryCountDto>> CreateAsync(CreateInventoryCountRequest request, CancellationToken ct = default);
    Task<Result<InventoryCountDto>> PostAsync(int id, CancellationToken ct = default);
    Task<Result> CancelAsync(int id, CancellationToken ct = default);
    Task<Result<InventoryCountDto>> AddLineAsync(int id, AddInventoryCountLineRequest request, CancellationToken ct = default);
}
