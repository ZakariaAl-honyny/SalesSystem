using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Business.Interfaces.Services;

public interface ITaxService
{
    Task<Result<List<TaxDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<TaxDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<TaxDto>> CreateAsync(CreateTaxRequest request, CancellationToken ct = default);
    Task<Result<TaxDto>> UpdateAsync(int id, UpdateTaxRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    Task<Result> PermanentDeleteAsync(int id, CancellationToken ct = default);
}
