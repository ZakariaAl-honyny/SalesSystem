using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// واجهة خدمة API الموظفين — استعلامات CRUD عبر HTTP.
/// </summary>
public interface IEmployeeApiService
{
    Task<Result<IReadOnlyList<EmployeeDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<EmployeeDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<EmployeeDto>> CreateAsync(EmployeeDto request, CancellationToken ct = default);
    Task<Result<EmployeeDto>> UpdateAsync(int id, EmployeeDto request, CancellationToken ct = default);
    Task<Result> DeactivateAsync(int id, CancellationToken ct = default);
}
