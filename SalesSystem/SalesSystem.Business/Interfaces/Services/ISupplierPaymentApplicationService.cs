using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Business.Interfaces.Services;

public interface ISupplierPaymentApplicationService
{
    Task<Result<List<SupplierPaymentApplicationDto>>> GetAllAsync(int? supplierPaymentId, int? purchaseInvoiceId, CancellationToken ct);
    Task<Result<SupplierPaymentApplicationDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<SupplierPaymentApplicationDto>> CreateAsync(CreateSupplierPaymentApplicationRequest request, CancellationToken ct);
    Task<Result> DeleteAsync(int id, CancellationToken ct);
}
