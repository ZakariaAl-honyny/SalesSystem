using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IRoleApiService
{
    Task<Result<IReadOnlyList<RoleDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<RoleDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<RoleDto>> CreateAsync(CreateRoleRequest request, CancellationToken ct = default);
    Task<Result<RoleDto>> UpdateAsync(int id, UpdateRoleRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
