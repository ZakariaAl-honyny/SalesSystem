using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.DTO;

// ═══════════════════════════════════════════════════════
// Phase 22 — Chart of Accounts DTOs
// ═══════════════════════════════════════════════════════

public record AccountDto(
    int Id,
    string AccountCode,
    string NameAr,
    string? NameEn,
    byte Nature,
    bool IsLeaf,
    int? ParentId,
    string? ParentAccountName,
    bool IsSystem,
    bool IsActive,
    short? CategoryId,
    byte Level,
    string? Description,
    string? ColorCode,
    string? Notes)
{
    public string NatureDisplay => Nature switch
    {
        1 => "أصل",
        2 => "خصم",
        3 => "حق ملكية",
        4 => "إيراد",
        5 => "مصروف",
        _ => "غير معروف"
    };

    public string LevelDisplay => Level switch
    {
        1 => "مجموعة رئيسية",
        2 => "مجموعة فرعية",
        3 => "فرعي",
        4 => "تفصيلي",
        _ => $"المستوى {Level}"
    };

    // Backward-compatible properties
    public byte AccountType => Nature;
    public string AccountTypeDisplay => NatureDisplay;
    public bool AllowTransactions => IsLeaf;
    public int? ParentAccountId => ParentId;
    public bool IsSystemAccount => IsSystem;
}

public record AccountTreeNodeDto(
    int Id,
    string AccountCode,
    string NameAr,
    byte Nature,
    bool IsLeaf,
    short? CategoryId,
    byte Level,
    string? ColorCode,
    string? Description,
    List<AccountTreeNodeDto> Children)
{
    // Backward-compatible properties
    public byte AccountType => Nature;
    public bool AllowTransactions => IsLeaf;
}

// ─── Accounting DTOs ─────────────────────────────────
public record AccountBalanceDto(
    int AccountId,
    string AccountCode,
    string AccountNameAr,
    byte Nature,
    decimal TotalDebit,
    decimal TotalCredit,
    decimal Balance,
    bool IsDebitNormal
)
{
    public AccountType AccountType => (AccountType)Nature;
}

public record AccountLedgerDto(
    string AccountCode,
    string AccountNameAr,
    decimal OpeningBalance,
    List<AccountLedgerLineDto> Lines,
    decimal TotalDebit,
    decimal TotalCredit,
    decimal ClosingBalance
);

public record AccountLedgerLineDto(
    DateTime Date,
    string EntryNumber,
    string Description,
    string? ReferenceNumber,
    decimal Debit,
    decimal Credit,
    decimal RunningBalance
);
