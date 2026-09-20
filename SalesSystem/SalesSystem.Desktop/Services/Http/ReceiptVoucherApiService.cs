using System.Web;
using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class ReceiptVoucherApiService : IReceiptVoucherApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/receipt-vouchers";

    public ReceiptVoucherApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<ReceiptVoucherDto>>> GetAllAsync(string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrEmpty(search)) query["search"] = search;
        if (from.HasValue) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to.HasValue) query["to"] = to.Value.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<ReceiptVoucherDto>($"{BasePath}?{query}", ct);
    }

    public async Task<Result<ReceiptVoucherDto>> GetByIdAsync(int id, CancellationToken ct = default)
        => await _http.GetAsync<ReceiptVoucherDto>($"{BasePath}/{id}", ct);

    public async Task<Result<ReceiptVoucherDto>> CreateAsync(CreateReceiptVoucherRequest request, CancellationToken ct = default)
        => await _http.PostAsync<ReceiptVoucherDto>(BasePath, request, ct);

    public async Task<Result<ReceiptVoucherDto>> PostAsync(int id, CancellationToken ct = default)
        => await _http.PutAsync<ReceiptVoucherDto>($"{BasePath}/{id}/post", null, ct);

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
        => await _http.PutAsync($"{BasePath}/{id}/cancel", null, ct);
}
