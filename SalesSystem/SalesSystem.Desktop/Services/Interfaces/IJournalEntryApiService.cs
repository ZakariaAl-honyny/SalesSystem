using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IJournalEntryApiService
{
    Task<Result<IReadOnlyList<JournalEntryListDto>>> GetAllAsync(
        CancellationToken ct = default,
        JournalEntryType? entryType = null,
        byte? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? search = null);
    Task<Result<JournalEntryDetailDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<int>> CreateAsync(CreateJournalEntryRequest request, CancellationToken ct = default);
    Task<Result> PostAsync(int id, CancellationToken ct = default);
    Task<Result> CancelAsync(int id, CancellationToken ct = default);
    Task<Result<JournalEntryDetailDto>> UpdateAsync(int id, UpdateJournalEntryRequest request, CancellationToken ct = default);
    Task<Result> DeleteDraftAsync(int id, CancellationToken ct = default);
}
