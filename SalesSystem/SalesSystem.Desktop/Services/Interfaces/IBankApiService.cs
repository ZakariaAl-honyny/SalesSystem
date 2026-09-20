using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IBankApiService
{
    Task<Result<IReadOnlyList<BankDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<BankDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<BankDto>> CreateAsync(CreateBankRequest request, CancellationToken ct = default);
    Task<Result<BankDto>> UpdateAsync(int id, UpdateBankRequest request, CancellationToken ct = default);
    Task<Result> DeactivateAsync(int id, CancellationToken ct = default);
}
