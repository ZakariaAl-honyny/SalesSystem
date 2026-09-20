using SalesSystem.Models.Accounting;
using SalesSystem.Common;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Represents a cash register location. Balance is tracked on the linked Account
/// in the Chart of Accounts. This entity is a lightweight register identifier
/// with metadata for operational use.
/// Schema: §4.3 CashBoxes — Id, AccountId (int not null FK),
/// Name, Description (nullable), IsActive, CreatedByUserId, UpdatedByUserId, CreatedAt, UpdatedAt.
/// </summary>
public class CashBox : ActivatableEntity
{
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// FK to the Chart of Accounts Account that holds this cash box's balance.
    /// Required — the service layer resolves the account before entity creation.
    /// </summary>
    public int AccountId { get; private set; }
    public Account? Account { get; private set; }

    /// <summary>
    /// Optional description or notes about this cash box.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Branch identifier for multi-branch support (smallint FK, nullable for company-wide cash boxes).
    /// </summary>
    public short? BranchId { get; private set; }

    /// <summary>
    /// Navigation property to the Branch this cash box belongs to.
    /// </summary>
    public virtual Branch? Branch { get; private set; }

    private CashBox() { } // EF Core

    /// <summary>
    /// Creates a new cash box with the specified name, account, and currency.
    /// AccountId is required — the service layer resolves auto-creation before calling this factory.
    /// </summary>
    public static CashBox Create(
        string name,
        int accountId,
        string? description = null,
        short? branchId = null,
        int? createdByUserId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم الصندوق مطلوب");

        if (accountId <= 0)
            throw new DomainException("الحساب المحاسبي مطلوب");

        var box = new CashBox
        {
            Name = name.Trim(),
            AccountId = accountId,
            Description = description?.Trim(),
            BranchId = branchId
        };
        box.SetCreatedBy(createdByUserId);
        return box;
    }

    // ─── Domain Methods ───────────────────────────

    /// <summary>
    /// Updates the mutable fields of the cash box.
    /// </summary>
    public void Update(
        string name,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم الصندوق مطلوب");

        Name = name.Trim();
        Description = description?.Trim();
        UpdateTimestamp();
    }
}
