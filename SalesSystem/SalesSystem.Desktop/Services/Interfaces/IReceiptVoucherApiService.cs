using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IReceiptVoucherApiService
{
    Task<Result<IReadOnlyList<ReceiptVoucherDto>>> GetAllAsync(string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
    Task<Result<ReceiptVoucherDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<ReceiptVoucherDto>> CreateAsync(CreateReceiptVoucherRequest request, CancellationToken ct = default);
    Task<Result<ReceiptVoucherDto>> PostAsync(int id, CancellationToken ct = default);
    Task<Result> CancelAsync(int id, CancellationToken ct = default);
}
