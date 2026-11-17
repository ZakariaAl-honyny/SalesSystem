using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Business.Interfaces.Services;

/// <summary>
/// Service for managing cash boxes — CRUD operations and balance queries.
/// Balance is computed from operational documents: sales, receipts, payments, and expenses.
/// </summary>
public interface ICashBoxService
{
    // Cash Box CRUD
    Task<Result<List<CashBoxDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<CashBoxDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<CashBoxDto>> CreateAsync(CreateCashBoxRequest request, int userId, CancellationToken ct = default);
    Task<Result<CashBoxDto>> UpdateAsync(int id, UpdateCashBoxRequest request, int userId, CancellationToken ct = default);
    Task<Result> DeactivateAsync(int id, CancellationToken ct = default);
    Task<Result> PermanentDeleteAsync(int id, CancellationToken ct = default);
    Task<Result> RestoreAsync(int id, CancellationToken ct = default);

    // Balance query
    Task<Result<decimal>> GetBalanceAsync(int cashBoxId, CancellationToken ct = default);
}
