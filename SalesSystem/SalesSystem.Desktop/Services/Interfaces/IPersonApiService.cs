using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IPersonApiService
{
    Task<Result<IReadOnlyList<PersonDto>>> GetAllAsync(string? search = null, bool includeInactive = false, CancellationToken ct = default);
    Task<Result<PersonDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<PersonDto>> GetByNationalNoAsync(string nationalNo, CancellationToken ct = default);
    Task<Result<PersonDto>> CreateAsync(CreatePersonRequest r, CancellationToken ct = default);
    Task<Result<PersonDto>> UpdateAsync(int id, UpdatePersonRequest r, CancellationToken ct = default);
    Task<Result> DeactivateAsync(int id, CancellationToken ct = default);
}
