using SalesSystem.Common;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Represents a cash register location. This entity is a lightweight register identifier
/// with metadata for operational use.
/// Schema: §4.3 CashBoxes — Id,
/// Name, Description (nullable), IsActive, CreatedByUserName, UpdatedByUserName, CreatedAt, UpdatedAt.
/// </summary>
public class CashBox : ActivatableEntity
{
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Optional description or notes about this cash box.
    /// </summary>
    public string? Description { get; private set; }

    private CashBox() { } // EF Core

    /// <summary>
    /// Creates a new cash box with the specified name.
    /// </summary>
    public static CashBox Create(
        string name,
        string? description = null,
        string? createdByUserName = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم الصندوق مطلوب");

        var box = new CashBox
        {
            Name = name.Trim(),
            Description = description?.Trim()
        };
        if (!string.IsNullOrWhiteSpace(createdByUserName)) box.SetCreatedBy(createdByUserName.Trim());
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
