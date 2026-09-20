using System.Web;
using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class PaymentVoucherApiService : IPaymentVoucherApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/payment-vouchers";

    public PaymentVoucherApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<PaymentVoucherDto>>> GetAllAsync(string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrEmpty(search)) query["search"] = search;
        if (from.HasValue) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to.HasValue) query["to"] = to.Value.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<PaymentVoucherDto>($"{BasePath}?{query}", ct);
    }

    public async Task<Result<PaymentVoucherDto>> GetByIdAsync(int id, CancellationToken ct = default)
        => await _http.GetAsync<PaymentVoucherDto>($"{BasePath}/{id}", ct);

    public async Task<Result<PaymentVoucherDto>> CreateAsync(CreatePaymentVoucherRequest request, CancellationToken ct = default)
        => await _http.PostAsync<PaymentVoucherDto>(BasePath, request, ct);

    public async Task<Result<PaymentVoucherDto>> UpdateAsync(int id, UpdatePaymentVoucherRequest request, CancellationToken ct = default)
        => await _http.PutAsync<PaymentVoucherDto>($"{BasePath}/{id}", request, ct);

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
        => await _http.DeleteAsync($"{BasePath}/{id}", ct);

    public async Task<Result<PaymentVoucherDto>> PostAsync(int id, CancellationToken ct = default)
        => await _http.PutAsync<PaymentVoucherDto>($"{BasePath}/{id}/post", null, ct);

    public async Task<Result<PaymentVoucherDto>> CancelAsync(int id, CancellationToken ct = default)
        => await _http.PutAsync<PaymentVoucherDto>($"{BasePath}/{id}/cancel", null, ct);
}
