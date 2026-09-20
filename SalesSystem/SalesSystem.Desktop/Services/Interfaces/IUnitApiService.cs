using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IUnitApiService
{
    Task<Result<IReadOnlyList<UnitDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<UnitDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<UnitDto>> CreateAsync(CreateUnitRequest r, CancellationToken ct = default);
    Task<Result<UnitDto>> UpdateAsync(int id, UpdateUnitRequest r, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}

