using SalesSystem.Common;
using SalesSystem.Common.Enums;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Represents an expense transaction (cash outflow from a cash box).
/// Supports lifecycle: Draft → Posted → Cancelled (terminal state).
/// </summary>
public class Expense : DocumentEntity
{
    public int ExpenseNo { get; private set; }
    public DateTime ExpenseDate { get; private set; }
    public int CashBoxId { get; private set; }

    public decimal Amount { get; private set; }

    public string? Notes { get; private set; }
    public InvoiceStatus Status { get; private set; }

    // Navigation properties
    public virtual CashBox? CashBox { get; private set; }
    private Expense() { } // EF Core

    /// <summary>
    /// Creates a new expense in Draft status.
    /// </summary>
    public static Expense Create(
        int expenseNo,
        DateTime expenseDate,
        int cashBoxId,
        decimal amount,
        string? notes = null,
        string? createdByUserName = null)
    {
        if (expenseNo <= 0)
            throw new DomainException("رقم المصروف يجب أن يكون أكبر من الصفر.");
        if (cashBoxId <= 0)
            throw new DomainException("الصندوق مطلوب.");
        if (amount <= 0)
            throw new DomainException("المبلغ يجب أن يكون أكبر من الصفر.");

        var expense = new Expense
        {
            ExpenseNo = expenseNo,
            ExpenseDate = expenseDate.Kind == DateTimeKind.Utc
                ? expenseDate
                : expenseDate.ToUniversalTime(),
            CashBoxId = cashBoxId,
            Amount = amount,
            Notes = notes,
            Status = InvoiceStatus.Draft
        };
        if (!string.IsNullOrWhiteSpace(createdByUserName)) expense.SetCreatedBy(createdByUserName.Trim());
        return expense;
    }

    /// <summary>
    /// Posts the expense — confirms the transaction. Only drafts can be posted.
    /// </summary>
    public void Post(DateTime? postedAt = null)
    {
        if (Status != InvoiceStatus.Draft)
            throw new DomainException("فقط المصروفات المسودة يمكن ترحيلها.");

        Status = InvoiceStatus.Posted;
        PostedAt = postedAt ?? DateTime.UtcNow;
        UpdateTimestamp();
    }

    /// <summary>
    /// Cancels the expense. Only drafts and posted expenses can be cancelled.
    /// Once cancelled, the expense cannot be modified or re-posted.
    /// </summary>
    public void Cancel()
    {
        if (Status == InvoiceStatus.Cancelled)
            throw new DomainException("المصروف ملغي بالفعل.");

        CancelledAt = DateTime.UtcNow;
        Status = InvoiceStatus.Cancelled;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates the expense fields. Only allowed while in Draft status.
    /// </summary>
    public void Update(
        DateTime expenseDate,
        int cashBoxId,
        decimal amount,
        string? notes)
    {
        if (Status != InvoiceStatus.Draft)
            throw new DomainException("لا يمكن تعديل مصروف بعد ترحيله.");

        if (cashBoxId <= 0)
            throw new DomainException("الصندوق مطلوب.");
        if (amount <= 0)
            throw new DomainException("المبلغ يجب أن يكون أكبر من الصفر.");

        ExpenseDate = expenseDate.Kind == DateTimeKind.Utc
            ? expenseDate
            : expenseDate.ToUniversalTime();
        CashBoxId = cashBoxId;
        Amount = amount;
        Notes = notes;

        UpdateTimestamp();
    }
}
