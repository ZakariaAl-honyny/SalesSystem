using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ICustomerApiService
{
    Task<Result<IReadOnlyList<CustomerDto>>> GetAllAsync(string? search = null, bool includeInactive = false, CancellationToken ct = default);
    Task<Result<CustomerDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<CustomerDto>> CreateAsync(CreateCustomerRequest r, CancellationToken ct = default);
    Task<Result<CustomerDto>> UpdateAsync(int id, UpdateCustomerRequest r, CancellationToken ct = default);
    Task<Result> DeactivateAsync(int id, CancellationToken ct = default);
    Task<Result> ReactivateAsync(int id, CancellationToken ct = default);
}

