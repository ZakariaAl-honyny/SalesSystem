using SalesSystem.Models.Accounting.Enums;
using SalesSystem.Common;
using SalesSystem.Models;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models.Accounting;

/// <summary>
/// Represents a journal entry (double-entry accounting record) with 3-state lifecycle:
/// Draft (1) → Posted (2) → Cancelled (3).
/// FK to FiscalYear for period-based reporting.
/// Lines must be added via AddDebitLine / AddCreditLine methods.
/// </summary>
public class JournalEntry : DocumentEntity
{
    /// <summary>
    /// Formatted display string derived from EntryNo (kept for thick-client display).
    /// </summary>
    public string EntryNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Integer sequence number for this journal entry (matches DB sequence).
    /// </summary>
    public int EntryNo { get; private set; }

    /// <summary>
    /// Date the entry was recorded (maps to schema EntryDate).
    /// </summary>
    public DateTime EntryDate { get; private set; }

    public string Description { get; private set; } = string.Empty;
    public JournalEntryType EntryType { get; private set; }
    public JournalEntryStatus Status { get; private set; }

    /// <summary>Optional attachment file path (nvarchar(500)).</summary>
    public string? AttachmentPath { get; private set; }

    public string? ReferenceType { get; private set; }
    public int? ReferenceId { get; private set; }
    public string? ReferenceNumber { get; private set; }
    public bool IsReversed { get; private set; }
    public int? ReversedByEntryId { get; private set; }
    public JournalEntry? ReversedByEntry { get; private set; }

    /// <summary>
    /// Historical snapshot of the name of the user who reviewed/posted this entry (nullable).
    /// Captured from the session at post time — not a foreign key.
    /// </summary>
    public string? ReviewedByUserName { get; private set; }

    /// <summary>Timestamp when this entry was reviewed (nullable).</summary>
    public DateTime? ReviewedAt { get; private set; }

    private readonly List<JournalEntryLine> _lines = new();
    public IReadOnlyList<JournalEntryLine> Lines => _lines.AsReadOnly();

    // ─── Computed Totals ─────────────────────────────

    public decimal TotalDebit => _lines.Sum(l => l.Debit);
    public decimal TotalCredit => _lines.Sum(l => l.Credit);

    private JournalEntry() { } // EF Core

    public static JournalEntry Create(
        string entryNumber,
        int entryNo,
        DateTime entryDate,
        string description,
        JournalEntryType entryType,
        string? createdByUserName = null,
        string? referenceType = null,
        int? referenceId = null,
        string? referenceNumber = null,
        string? attachmentPath = null)
    {
        if (string.IsNullOrWhiteSpace(entryNumber))
            throw new DomainException("رقم القيد المحاسبي مطلوب");

        if (entryNo <= 0)
            throw new DomainException("الرقم التسلسلي للقيد المحاسبي مطلوب");

        if (entryDate == default)
            throw new DomainException("تاريخ القيد المحاسبي مطلوب");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("الوصف مطلوب");

        if (!Enum.IsDefined(typeof(JournalEntryType), entryType))
            throw new DomainException("نوع القيد المحاسبي غير صالح");

        if (attachmentPath?.Length > 500)
            throw new DomainException("مسار المرفق لا يمكن أن يتجاوز 500 حرف");

        var entry = new JournalEntry
        {
            EntryNumber = entryNumber.Trim(),
            EntryNo = entryNo,
            EntryDate = entryDate,
            Description = description.Trim(),
            EntryType = entryType,
            Status = JournalEntryStatus.Draft,
            ReferenceType = referenceType?.Trim(),
            ReferenceId = referenceId,
            ReferenceNumber = referenceNumber?.Trim(),
            AttachmentPath = attachmentPath?.Trim()
        };
        entry.SetCreatedBy(createdByUserName);
        return entry;
    }

    // ─── Line Management ─────────────────────────────

    public void AddDebitLine(
        int accountId,
        decimal amount,
        string? description = null)
    {
        if (Status != JournalEntryStatus.Draft)
            throw new DomainException("لا يمكن تعديل قيد محاسبي تم ترحيله أو إلغاؤه");

        var line = JournalEntryLine.CreateDebit(
            accountId, amount, description);
        _lines.Add(line);
        UpdateTimestamp();
    }

    public void AddCreditLine(
        int accountId,
        decimal amount,
        string? description = null)
    {
        if (Status != JournalEntryStatus.Draft)
            throw new DomainException("لا يمكن تعديل قيد محاسبي تم ترحيله أو إلغاؤه");

        var line = JournalEntryLine.CreateCredit(
            accountId, amount, description);
        _lines.Add(line);
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates mutable header fields of the journal entry.
    /// Only allowed in Draft status. Lines are managed separately.
    /// </summary>
    public void Update(
        DateTime entryDate,
        string description,
        JournalEntryType entryType,
        string? referenceType,
        int? referenceId,
        string? referenceNumber)
    {
        if (Status != JournalEntryStatus.Draft)
            throw new DomainException("لا يمكن تعديل قيد محاسبي تم ترحيله أو إلغاؤه");

        EntryDate = entryDate;
        Description = description;
        EntryType = entryType;
        ReferenceType = referenceType;
        ReferenceId = referenceId;
        ReferenceNumber = referenceNumber;
        UpdateTimestamp();
    }

    // ─── Balance & Lifecycle ─────────────────────────

    /// <summary>
    /// Returns true if total debits equal total credits (within 0.001 tolerance).
    /// </summary>
    public bool IsBalanced() => Math.Abs(TotalDebit - TotalCredit) < 0.001m;

    /// <summary>
    /// Posts this journal entry. Validates Draft status, balanced, and has lines.
    /// The posting user is passed as a name snapshot (nullable) — never as a numeric user id.
    /// </summary>
    public void Post(string? reviewedByUserName = null)
    {
        if (Status != JournalEntryStatus.Draft)
            throw new DomainException("لا يمكن ترحيل إلا القيود المحاسبية في حالة مسودة");

        if (_lines.Count == 0)
            throw new DomainException("لا يمكن ترحيل قيد محاسبي بدون بنود");

        if (!IsBalanced())
            throw new DomainException(
                $"القيد المحاسبي غير متوازن — مجموع المدين ({TotalDebit:N2}) لا يساوي مجموع الدائن ({TotalCredit:N2})");

        PostedAt = DateTime.UtcNow;
        Status = JournalEntryStatus.Posted;
        ReviewedByUserName = string.IsNullOrWhiteSpace(reviewedByUserName) ? null : reviewedByUserName.Trim();
        ReviewedAt = DateTime.UtcNow;
        UpdateTimestamp();
    }

    /// <summary>
    /// Cancels this posted journal entry. Optionally links to a reversal entry.
    /// </summary>
    public void Cancel(int? reversedByEntryId = null)
    {
        if (Status != JournalEntryStatus.Posted)
            throw new DomainException("لا يمكن إلغاء إلا القيود المحاسبية المرحلة");

        CancelledAt = DateTime.UtcNow;
        Status = JournalEntryStatus.Cancelled;
        ReversedByEntryId = reversedByEntryId;
        UpdateTimestamp();
    }
}
