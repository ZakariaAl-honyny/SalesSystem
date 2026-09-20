using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ISupplierPaymentApiService
{
    Task<Result<IReadOnlyList<SupplierPaymentDto>>> GetAllAsync(int? supplierId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
    Task<Result<SupplierPaymentDto>> CreateAsync(CreateSupplierPaymentRequest r, CancellationToken ct = default);
}

