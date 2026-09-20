using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ITaxesApiService
{
    Task<Result<IReadOnlyList<TaxDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<TaxDto>> CreateAsync(CreateTaxRequest request, CancellationToken ct = default);
    Task<Result<TaxDto>> UpdateAsync(int id, UpdateTaxRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    Task<Result> DeletePermanentlyAsync(int id, CancellationToken ct = default);
}
