using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IJournalEntryApiService
{
    Task<Result<IReadOnlyList<JournalEntryListDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<JournalEntryDetailDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<int>> CreateAsync(CreateJournalEntryRequest request, CancellationToken ct = default);
    Task<Result> PostAsync(int id, CancellationToken ct = default);
    Task<Result> CancelAsync(int id, CancellationToken ct = default);
}
