using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IAccountCategoryApiService
{
    Task<Result<IReadOnlyList<AccountCategoryDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<AccountCategoryDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<AccountCategoryDto>> CreateAsync(CreateAccountCategoryRequest request, CancellationToken ct = default);
    Task<Result<AccountCategoryDto>> UpdateAsync(int id, UpdateAccountCategoryRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
