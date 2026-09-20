using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ICashBoxApiService
{
    Task<Result<IReadOnlyList<CashBoxDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<CashBoxDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<CashBoxDto>> CreateAsync(CreateCashBoxRequest request, CancellationToken ct = default);
    Task<Result<CashBoxDto>> UpdateAsync(int id, UpdateCashBoxRequest request, CancellationToken ct = default);
    Task<Result> DeactivateAsync(int id, CancellationToken ct = default);
    Task<Result> TransferAsync(CashTransferRequest request, CancellationToken ct = default);
}
