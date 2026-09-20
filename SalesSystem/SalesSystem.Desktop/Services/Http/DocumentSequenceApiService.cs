using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class DocumentSequenceApiService : IDocumentSequenceApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/document-sequences";

    public DocumentSequenceApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<DocumentSequenceDto>>> GetAllAsync(CancellationToken ct = default)
    {
        return await _http.GetListAsync<DocumentSequenceDto>(BasePath, ct);
    }

    public async Task<Result> UpdateAsync(int id, UpdateDocumentSequenceRequest request, CancellationToken ct = default)
    {
        return await _http.PutAsync($"{BasePath}/{id}", request, ct);
    }
}
