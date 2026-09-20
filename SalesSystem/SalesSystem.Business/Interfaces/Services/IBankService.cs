using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Business.Interfaces.Services;

public interface IBankService
{
    Task<Result<List<BankDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<BankDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<BankDto>> CreateAsync(CreateBankRequest request, int userId, CancellationToken ct);
    Task<Result<BankDto>> UpdateAsync(int id, UpdateBankRequest request, int userId, CancellationToken ct);
    Task<Result> DeactivateAsync(int id, CancellationToken ct);
    Task<Result> PermanentDeleteAsync(int id, CancellationToken ct);
    Task<Result> RestoreAsync(int id, CancellationToken ct);
}
