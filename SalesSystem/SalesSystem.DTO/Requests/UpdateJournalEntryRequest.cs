using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.DTO.Requests;

/// <summary>
/// Request to update an existing Draft journal entry.
/// Mirrors CreateJournalEntryRequest structure for consistency.
/// </summary>
public record UpdateJournalEntryRequest(
    DateTime EntryDate,
    string Description,
    JournalEntryType EntryType,
    string? ReferenceType,
    int? ReferenceId,
    string? ReferenceNumber,
    List<JournalEntryLineRequest> Lines
);
