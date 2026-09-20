using System.Web;
using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class ExpenseApiService : IExpenseApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/expenses";

    public ExpenseApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<ExpenseDto>>> GetAllAsync(string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrEmpty(search)) query["search"] = search;
        if (from.HasValue) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to.HasValue) query["to"] = to.Value.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<ExpenseDto>($"{BasePath}?{query}", ct);
    }

    public async Task<Result<ExpenseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        => await _http.GetAsync<ExpenseDto>($"{BasePath}/{id}", ct);

    public async Task<Result<ExpenseDto>> CreateAsync(CreateExpenseRequest request, CancellationToken ct = default)
        => await _http.PostAsync<ExpenseDto>(BasePath, request, ct);

    public async Task<Result<ExpenseDto>> UpdateAsync(int id, UpdateExpenseRequest request, CancellationToken ct = default)
        => await _http.PutAsync<ExpenseDto>($"{BasePath}/{id}", request, ct);

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
        => await _http.DeleteAsync($"{BasePath}/{id}", ct);

    public async Task<Result<ExpenseDto>> PostAsync(int id, CancellationToken ct = default)
        => await _http.PutAsync<ExpenseDto>($"{BasePath}/{id}/post", null, ct);

    public async Task<Result<ExpenseDto>> CancelAsync(int id, CancellationToken ct = default)
        => await _http.PutAsync<ExpenseDto>($"{BasePath}/{id}/cancel", null, ct);
}
