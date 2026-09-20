using SalesSystem.Common;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Represents a product category. Simple flat structure — no hierarchical support.
/// Maps to "ProductCategories" table in the database schema (§3.1).
/// Schema: int PK, Name (nvarchar 100 unique), Description (nvarchar 500), IsActive, audit.
/// </summary>
public class ProductCategory : ActivatableEntity
{
    /// <summary>
    /// Category display name (Arabic, e.g., "مشروبات", "منظفات").
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Optional description of the category.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Branch identifier for multi-branch support (smallint FK).
    /// </summary>
    public short BranchId { get; private set; }

    /// <summary>
    /// Navigation property to the Branch this category belongs to.
    /// </summary>
    public virtual Branch? Branch { get; private set; }

    private ProductCategory() { } // EF Core

    // ─── Factory ─────────────────────────────────────────

    /// <summary>
    /// Creates a new product category.
    /// </summary>
    /// <param name="branchId">Branch identifier (required).</param>
    /// <param name="name">Category name (required).</param>
    /// <param name="description">Optional description.</param>
    /// <param name="createdByUserId">User who created this record.</param>
    /// <returns>The newly created ProductCategory entity.</returns>
    public static ProductCategory Create(
        short branchId,
        string name,
        string? description = null,
        int? createdByUserId = null)
    {
        if (branchId <= 0)
            throw new DomainException("الفرع مطلوب.");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم التصنيف مطلوب.");

        var category = new ProductCategory
        {
            BranchId = branchId,
            Name = name.Trim(),
            Description = description?.Trim(),
            IsActive = true
        };
        category.SetCreatedBy(createdByUserId);
        return category;
    }

    // ─── Domain Methods ──────────────────────────────────

    /// <summary>
    /// Updates the category properties. Throws if name is empty.
    /// </summary>
    public void Update(
        string name,
        string? description = null,
        int? updatedByUserId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم التصنيف مطلوب.");

        Name = name.Trim();
        Description = description?.Trim();
        SetUpdatedBy(updatedByUserId);
        UpdateTimestamp();
    }
}
