using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// واجهة خدمة عروض الأسعار — CRUD عبر API.
/// </summary>
public interface IQuotationApiService
{
    Task<Result<IReadOnlyList<QuotationDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<QuotationDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<QuotationDto>> CreateAsync(QuotationDto request, CancellationToken ct = default);
    Task<Result<QuotationDto>> UpdateAsync(int id, QuotationDto request, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
