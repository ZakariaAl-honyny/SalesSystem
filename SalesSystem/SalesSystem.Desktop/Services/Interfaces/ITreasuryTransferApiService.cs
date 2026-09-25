using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// خدمة API لتحويلات الخزينة — تحويل النقد بين الصناديق والبنوك.
/// </summary>
public interface ITreasuryTransferApiService
{
    Task<Result<IReadOnlyList<TreasuryTransferDto>>> GetAllAsync(
        bool includeInactive = false,
        string? search = null,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken ct = default);

    Task<Result<TreasuryTransferDto>> GetByIdAsync(int id, CancellationToken ct = default);

    Task<Result<TreasuryTransferDto>> CreateAsync(TreasuryTransferDto dto, CancellationToken ct = default);

    Task<Result<TreasuryTransferDto>> UpdateAsync(int id, TreasuryTransferDto dto, CancellationToken ct = default);

    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
