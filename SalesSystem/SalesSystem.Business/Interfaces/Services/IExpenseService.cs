using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Business.Interfaces.Services;

public interface IExpenseService
{
    Task<Result<ExpenseDto>> CreateAsync(CreateExpenseRequest request, int userId, CancellationToken ct);
    Task<Result<ExpenseDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<PagedResult<ExpenseDto>>> GetAllAsync(string? search, DateTime? from, DateTime? to, int page, int pageSize, CancellationToken ct);
    Task<Result<ExpenseDto>> UpdateAsync(int id, UpdateExpenseRequest request, int userId, CancellationToken ct);
    Task<Result> DeleteAsync(int id, CancellationToken ct);
    Task<Result<ExpenseDto>> PostAsync(int id, int userId, CancellationToken ct);
    Task<Result<ExpenseDto>> CancelAsync(int id, CancellationToken ct);
}
