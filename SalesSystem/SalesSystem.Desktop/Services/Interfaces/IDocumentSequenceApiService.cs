using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IDocumentSequenceApiService
{
    Task<Result<IReadOnlyList<DocumentSequenceDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result> UpdateAsync(int id, UpdateDocumentSequenceRequest request, CancellationToken ct = default);
}
