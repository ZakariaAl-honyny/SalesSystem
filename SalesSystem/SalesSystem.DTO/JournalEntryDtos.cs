namespace SalesSystem.DTO;

// ─── Journal Entry List/Detail DTOs ──────────────────

public record JournalEntryListDto(
    int Id,
    string EntryNumber,
    DateTime EntryDate,
    string Description,
    string EntryType,
    string? ReferenceType,
    int? ReferenceId,
    string? ReferenceNumber,
    decimal TotalDebit,
    decimal TotalCredit,
    int Status,
    string? StatusDisplay,
    DateTime CreatedAt,
    int? CreatedByUserId
);

public record JournalEntryDetailDto(
    int Id,
    string EntryNumber,
    DateTime EntryDate,
    string Description,
    string EntryType,
    string? ReferenceType,
    int? ReferenceId,
    string? ReferenceNumber,
    int Status,
    string? StatusDisplay,
    int? ReversedByEntryId,
    DateTime CreatedAt,
    int? CreatedByUserId,
    List<JournalEntryLineDetailDto> Lines
);

public record JournalEntryLineDetailDto(
    int Id,
    int AccountId,
    string? AccountName,
    string? AccountCode,
    decimal Debit,
    decimal Credit,
    string? Description
);

public record AccountStatementDto(
    DateTime Date,
    string Description,
    string ReferenceNumber,
    decimal Debit,
    decimal Credit,
    decimal Balance
);
