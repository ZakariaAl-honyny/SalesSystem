using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Business.Interfaces.Services;

public interface ISupplierService
{
    Task<Result<SupplierDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<PagedResult<SupplierDto>>> GetAllAsync(string? search, int page, int pageSize, bool includeInactive = false, CancellationToken ct = default);
    Task<Result<SupplierDto>> CreateAsync(CreateSupplierRequest request, int userId, CancellationToken ct);
    Task<Result<SupplierDto>> UpdateAsync(int id, UpdateSupplierRequest request, int userId, CancellationToken ct);
    Task<Result> DeleteAsync(int id, int userId, CancellationToken ct);
    Task<Result> PermanentDeleteAsync(int id, int userId, CancellationToken ct);
    Task<Result> ReactivateAsync(int id, CancellationToken ct = default);
}
