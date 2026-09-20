using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IPaymentVoucherApiService
{
    Task<Result<IReadOnlyList<PaymentVoucherDto>>> GetAllAsync(string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
    Task<Result<PaymentVoucherDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<PaymentVoucherDto>> CreateAsync(CreatePaymentVoucherRequest request, CancellationToken ct = default);
    Task<Result<PaymentVoucherDto>> UpdateAsync(int id, UpdatePaymentVoucherRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    Task<Result<PaymentVoucherDto>> PostAsync(int id, CancellationToken ct = default);
    Task<Result<PaymentVoucherDto>> CancelAsync(int id, CancellationToken ct = default);
}
