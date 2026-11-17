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
    string? Notes,
    string? ParentAccountCode = null,
    string TreePath = "",
    byte FinancialStatement = 0,
    // STORED normal balance — mirrors the schema column (`tinyint NULL`):
    // 0 = not stored (derive from `Nature`) · 1 = مدين · 2 = دائن.
    // The effective side is `EffectiveNormalBalance`; the label is `NormalBalanceDisplay`.
    byte NormalBalance = 0,
    bool AllowCostCenter = false,
    decimal Balance = 0m,
    string? CategoryLabel = null)
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

    /// <summary>Derived: an account that has a parent is a sub-account (never stored).</summary>
    public bool IsSubAccount => ParentId.HasValue;

    /// <summary>
    /// Derived: the effective normal balance — the stored override when present (1/2), otherwise the
    /// natural side of <see cref="Nature"/> (Debit = 1 for Asset/Expense, Credit = 2 otherwise).
    /// Mirrors <c>Account.EffectiveNormalBalance</c>.
    /// </summary>
    public byte EffectiveNormalBalance => NormalBalance is 1 or 2 ? NormalBalance : (Nature is 1 or 5 ? (byte)1 : (byte)2);

    /// <summary>
    /// Arabic label of the effective normal balance — مدين (1) / دائن (2) — suffixed with « (تلقائي) »
    /// when the stored value is 0 (not stored: derived from <see cref="Nature"/>).
    /// </summary>
    public string NormalBalanceDisplay
        => DescribeNormalBalance(EffectiveNormalBalance) + (NormalBalance == 0 ? " (تلقائي)" : string.Empty);

    /// <summary>Arabic label of the financial statement: الميزانية العمومية (1) / قائمة الدخل (2).</summary>
    public string FinancialStatementDisplay => DescribeFinancialStatement(FinancialStatement);

    // Backward-compatible properties
    public byte AccountType => Nature;
    public string AccountTypeDisplay => NatureDisplay;
    public bool AllowTransactions => IsLeaf;
    public int? ParentAccountId => ParentId;
    public bool IsSystemAccount => IsSystem;

    // ─── Single home for the two Arabic labels (also consumed by AccountTreeNodeDto) ───

    /// <summary>Arabic label of a normal balance value: 1 = مدين · 2 = دائن.</summary>
    public static string DescribeNormalBalance(byte normalBalance) => normalBalance switch
    {
        1 => "مدين",
        2 => "دائن",
        _ => "—"
    };

    /// <summary>Arabic label of a financial statement value: 1 = الميزانية العمومية · 2 = قائمة الدخل.</summary>
    public static string DescribeFinancialStatement(byte financialStatement) => financialStatement switch
    {
        1 => "الميزانية العمومية",
        2 => "قائمة الدخل",
        _ => "—"
    };
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
    List<AccountTreeNodeDto> Children,
    string? ParentAccountCode = null,
    string TreePath = "",
    byte FinancialStatement = 0,
    // STORED normal balance — mirrors the schema column (`tinyint NULL`):
    // 0 = not stored (derive from `Nature`) · 1 = مدين · 2 = دائن.
    // The effective side is `EffectiveNormalBalance`; the label is `NormalBalanceDisplay`.
    byte NormalBalance = 0,
    bool AllowCostCenter = false,
    string? CategoryLabel = null)
{
    /// <summary>
    /// Derived: the effective normal balance — the stored override when present (1/2), otherwise the
    /// natural side of <see cref="Nature"/> (Debit = 1 for Asset/Expense, Credit = 2 otherwise).
    /// Mirrors <c>Account.EffectiveNormalBalance</c>.
    /// </summary>
    public byte EffectiveNormalBalance => NormalBalance is 1 or 2 ? NormalBalance : (Nature is 1 or 5 ? (byte)1 : (byte)2);

    /// <summary>
    /// Arabic label of the effective normal balance — مدين (1) / دائن (2) — suffixed with « (تلقائي) »
    /// when the stored value is 0 (not stored: derived from <see cref="Nature"/>).
    /// </summary>
    public string NormalBalanceDisplay
        => AccountDto.DescribeNormalBalance(EffectiveNormalBalance) + (NormalBalance == 0 ? " (تلقائي)" : string.Empty);

    /// <summary>Arabic label of the financial statement: الميزانية العمومية (1) / قائمة الدخل (2).</summary>
    public string FinancialStatementDisplay => AccountDto.DescribeFinancialStatement(FinancialStatement);

    // Backward-compatible properties
    public byte AccountType => Nature;
    public bool AllowTransactions => IsLeaf;
}

/// <summary>
/// Preview of the next account code that would be generated under a given parent — returned by
/// <c>GetNextChildCodeAsync</c> so the screen can show the code, its level and its tree path before saving.
/// </summary>
public record AccountCodePreviewDto(
    string Code,
    string ParentAccountCode,
    byte Level,
    string TreePath);


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
