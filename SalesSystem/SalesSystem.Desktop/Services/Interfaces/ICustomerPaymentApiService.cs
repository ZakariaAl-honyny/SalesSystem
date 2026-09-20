using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ICustomerPaymentApiService
{
    Task<Result<IReadOnlyList<CustomerPaymentDto>>> GetAllAsync(int? customerId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
    Task<Result<CustomerPaymentDto>> CreateAsync(CreateCustomerPaymentRequest r, CancellationToken ct = default);
}

