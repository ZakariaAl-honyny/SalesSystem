using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ICustomerReceiptApiService
{
    Task<Result<IReadOnlyList<CustomerReceiptDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<CustomerReceiptDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<CustomerReceiptDto>> CreateAsync(CreateCustomerReceiptRequest request, CancellationToken ct = default);
    Task<Result<CustomerReceiptDto>> UpdateAsync(int id, UpdateCustomerReceiptRequest request, CancellationToken ct = default);
    Task<Result<CustomerReceiptDto>> PostAsync(int id, CancellationToken ct = default);
    Task<Result> CancelAsync(int id, CancellationToken ct = default);
}
