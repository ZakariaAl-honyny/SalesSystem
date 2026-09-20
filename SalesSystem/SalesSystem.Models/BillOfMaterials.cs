using SalesSystem.Common;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Represents a product assembly/manufacturing bill of materials.
/// Tracks which products (components) are used to assemble another product (finished good).
/// Maps to "BillOfMaterials" table.
/// </summary>
public class BillOfMaterials : ActivatableEntity
{
    /// <summary>
    /// FK to Product — the assembled/finished product.
    /// </summary>
    public int FinishedProductId { get; private set; }

    /// <summary>
    /// FK to Product — the component/raw material.
    /// </summary>
    public int ComponentProductId { get; private set; }

    /// <summary>
    /// Quantity of component needed per unit of finished product.
    /// decimal(18,3) per RULE-002 — ALL quantities must be decimal(18,3).
    /// </summary>
    public decimal QuantityRequired { get; private set; }

    /// <summary>
    /// FK to Unit — optional, which unit the quantity is measured in (smallint).
    /// </summary>
    public short? UnitId { get; private set; }

    /// <summary>
    /// Optional notes about this BOM entry (nvarchar(500)).
    /// </summary>
    public string? Notes { get; private set; }

    // ─── Navigation Properties ────────────────────────────

    /// <summary>
    /// The assembled/finished product.
    /// </summary>
    public virtual Product? FinishedProduct { get; private set; }

    /// <summary>
    /// The component/raw material.
    /// </summary>
    public virtual Product? ComponentProduct { get; private set; }

    /// <summary>
    /// The unit of measure for the quantity (optional).
    /// </summary>
    public virtual Unit? Unit { get; private set; }

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private BillOfMaterials() { }

    // ─── Factory ─────────────────────────────────────────

    /// <summary>
    /// Creates a new BillOfMaterials entry.
    /// </summary>
    /// <param name="finishedProductId">FK to the assembled/finished product.</param>
    /// <param name="componentProductId">FK to the component/raw material.</param>
    /// <param name="quantityRequired">Quantity of component needed per unit of finished product.</param>
    /// <param name="unitId">Optional FK to Unit (smallint).</param>
    /// <param name="notes">Optional notes.</param>
    /// <param name="createdByUserId">User who created this record.</param>
    /// <returns>The newly created BillOfMaterials entity.</returns>
    public static BillOfMaterials Create(
        int finishedProductId,
        int componentProductId,
        decimal quantityRequired,
        short? unitId = null,
        string? notes = null,
        int? createdByUserId = null)
    {
        if (finishedProductId <= 0)
            throw new DomainException("المنتج النهائي مطلوب.");

        if (componentProductId <= 0)
            throw new DomainException("المنتج المكون مطلوب.");

        if (componentProductId == finishedProductId)
            throw new DomainException("لا يمكن أن يكون المنتج النهائي هو نفسه المنتج المكون.");

        if (quantityRequired <= 0)
            throw new DomainException("الكمية المطلوبة يجب أن تكون أكبر من صفر.");

        var bom = new BillOfMaterials
        {
            FinishedProductId = finishedProductId,
            ComponentProductId = componentProductId,
            QuantityRequired = quantityRequired,
            UnitId = unitId,
            Notes = notes?.Trim(),
            IsActive = true
        };
        bom.SetCreatedBy(createdByUserId);
        return bom;
    }

    // ─── Domain Methods ─────────────────────────────────

    /// <summary>
    /// Updates the required quantity for this BOM entry.
    /// </summary>
    /// <param name="newQuantity">New quantity value (must be greater than zero).</param>
    public void UpdateQuantity(decimal newQuantity)
    {
        if (newQuantity <= 0)
            throw new DomainException("الكمية المطلوبة يجب أن تكون أكبر من صفر.");

        QuantityRequired = newQuantity;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates the notes for this BOM entry.
    /// </summary>
    /// <param name="notes">New notes text (nullable).</param>
    public void UpdateNotes(string? notes)
    {
        Notes = notes?.Trim();
        UpdateTimestamp();
    }
}
