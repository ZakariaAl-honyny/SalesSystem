using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IBranchApiService
{
    Task<Result<IReadOnlyList<BranchDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
}
