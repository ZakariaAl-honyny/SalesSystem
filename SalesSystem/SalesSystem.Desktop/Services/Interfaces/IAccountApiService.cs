using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IAccountApiService
{
    Task<Result<IReadOnlyList<AccountDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<AccountDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<AccountTreeNodeDto>>> GetTreeAsync(CancellationToken ct = default);
    Task<Result<int>> CreateAsync(CreateAccountRequest request, CancellationToken ct = default);
    Task<Result> UpdateAsync(int id, UpdateAccountRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    Task<Result> PermanentDeleteAsync(int id, CancellationToken ct = default);
}
