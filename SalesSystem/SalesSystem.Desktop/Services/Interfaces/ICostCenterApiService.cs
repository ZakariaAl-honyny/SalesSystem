using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// واجهة خدمة مراكز التكلفة — CRUD عبر API.
/// </summary>
public interface ICostCenterApiService
{
    Task<Result<IReadOnlyList<CostCenterDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<CostCenterDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<CostCenterDto>> CreateAsync(CostCenterDto dto, CancellationToken ct = default);
    Task<Result<CostCenterDto>> UpdateAsync(int id, CostCenterDto dto, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
