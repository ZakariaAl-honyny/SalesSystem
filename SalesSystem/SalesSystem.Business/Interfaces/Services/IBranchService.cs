using SalesSystem.DTO.Common;
using SalesSystem.DTO;

namespace SalesSystem.Business.Interfaces.Services;

public interface IBranchService
{
    Task<Result<List<BranchDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<BranchDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<BranchDto>> CreateAsync(string name, string? address, string? phone, int userId, CancellationToken ct);
    Task<Result<BranchDto>> UpdateAsync(int id, string name, string? address, string? phone, int userId, CancellationToken ct);
    Task<Result> DeactivateAsync(int id, CancellationToken ct);
    Task<Result> RestoreAsync(int id, CancellationToken ct);
}
