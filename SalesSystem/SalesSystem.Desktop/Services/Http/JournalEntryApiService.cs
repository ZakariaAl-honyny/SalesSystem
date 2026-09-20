using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class JournalEntryApiService : IJournalEntryApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/journal-entries";

    public JournalEntryApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<JournalEntryListDto>>> GetAllAsync(CancellationToken ct = default)
    {
        return await _http.GetListAsync<JournalEntryListDto>(BasePath, ct);
    }

    public async Task<Result<JournalEntryDetailDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetAsync<JournalEntryDetailDto>($"{BasePath}/{id}", ct);
    }

    public async Task<Result<int>> CreateAsync(CreateJournalEntryRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<int>(BasePath, request, ct);
    }

    public async Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        return await _http.PostAsync($"{BasePath}/{id}/post", null, ct);
    }

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        return await _http.PostAsync($"{BasePath}/{id}/cancel", null, ct);
    }
}
