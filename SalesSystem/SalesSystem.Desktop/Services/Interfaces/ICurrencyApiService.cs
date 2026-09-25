using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// واجهة خدمة العملات — تُستخدم في وضع التجربة (Mock) أولاً.
/// </summary>
public interface ICurrencyApiService
{
    Task<Result<IReadOnlyList<CurrencyDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<CurrencyDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<CurrencyDto>> CreateAsync(CurrencyDto request, CancellationToken ct = default);
    Task<Result<CurrencyDto>> UpdateAsync(int id, CurrencyDto request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<CurrencyRateDto>>> GetRatesAsync(int currencyId, CancellationToken ct = default);
}
