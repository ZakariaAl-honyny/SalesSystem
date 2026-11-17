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

    private ProductCategory() { } // EF Core

    // ─── Factory ─────────────────────────────────────────

    /// <summary>
    /// Creates a new product category.
    /// </summary>
    /// <param name="name">Category name (required).</param>
    /// <param name="description">Optional description.</param>
    /// <returns>The newly created ProductCategory entity.</returns>
    public static ProductCategory Create(
        string name,
        string? description = null,
        string? createdByUserName = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم التصنيف مطلوب.");

        var category = new ProductCategory
        {
            Name = name.Trim(),
            Description = description?.Trim(),
            IsActive = true
        };
        if (!string.IsNullOrWhiteSpace(createdByUserName)) category.SetCreatedBy(createdByUserName.Trim());
        return category;
    }

    // ─── Domain Methods ──────────────────────────────────

    /// <summary>
    /// Updates the category properties. Throws if name is empty.
    /// </summary>
    public void Update(
        string name,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم التصنيف مطلوب.");

        Name = name.Trim();
        Description = description?.Trim();
        UpdateTimestamp();
    }
}
