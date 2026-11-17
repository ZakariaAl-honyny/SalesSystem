using SalesSystem.Common;
using SalesSystem.Common.Enums;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Represents an inventory adjustment operation that corrects stock levels.
/// Maps to "InventoryAdjustments" table.
/// Schema: nvarchar(50) AdjustmentNo (unique), tinyint AdjustmentType (Addition=1,Deduction=2,Correction=3),
/// nvarchar(500) Reason, tinyint Status (Draft=1,Posted=2,Cancelled=3),
/// nvarchar CreatedByUserName, datetime2 CreatedAt, datetime2? PostedAt, datetime2? CancelledAt.
/// BaseEntity with CreatedAt (like DocumentEntity without UpdatedAt/UpdatedByUserName).
/// </summary>
public class InventoryAdjustment : Entity
{
    public string AdjustmentNo { get; private set; } = string.Empty;
    public short WarehouseId { get; private set; }
    public InventoryAdjustmentType AdjustmentType { get; private set; }
    public string? Reason { get; private set; }
    public InventoryCountStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public string? CreatedByUserName { get; private set; }
    public DateTime? PostedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    // Navigation properties
    public virtual Warehouse? Warehouse { get; private set; }

    private readonly List<InventoryAdjustmentLine> _lines = new();
    public IReadOnlyCollection<InventoryAdjustmentLine> Lines => _lines.AsReadOnly();

    private InventoryAdjustment() { }

    public static InventoryAdjustment Create(
        string adjustmentNo,
        short warehouseId,
        InventoryAdjustmentType adjustmentType,
        string? reason = null,
        string? createdByUserName = null)
    {
        if (string.IsNullOrWhiteSpace(adjustmentNo))
            throw new DomainException("رقم التسوية مطلوب.");
        if (warehouseId <= 0)
            throw new DomainException("المستودع مطلوب.");

        var adjustment = new InventoryAdjustment
        {
            AdjustmentNo = adjustmentNo.Trim(),
            WarehouseId = warehouseId,
            AdjustmentType = adjustmentType,
            Reason = reason,
            Status = InventoryCountStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserName = string.IsNullOrWhiteSpace(createdByUserName) ? null : createdByUserName.Trim()
        };
        return adjustment;
    }

    public void AddLine(InventoryAdjustmentLine line)
    {
        if (line == null)
            throw new DomainException("صنف التسوية مطلوب.");
        if (Status != InventoryCountStatus.Draft)
            throw new DomainException("لا يمكن إضافة أصناف لتسوية غير مسودة.");
        _lines.Add(line);
    }

    public void Post()
    {
        if (Status != InventoryCountStatus.Draft)
            throw new DomainException("فقط التسويات المسودة يمكن ترحيلها.");
        if (!_lines.Any())
            throw new DomainException("لا يمكن ترحيل تسوية بدون أصناف.");
        Status = InventoryCountStatus.Posted;
        PostedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == InventoryCountStatus.Cancelled)
            throw new DomainException("التسوية ملغاة بالفعل.");
        Status = InventoryCountStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
    }
}
