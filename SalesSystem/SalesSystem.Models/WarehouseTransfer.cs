using SalesSystem.Common;
using SalesSystem.Common.Enums;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Represents a stock transfer between two warehouses.
/// Maps to "WarehouseTransfers" table.
/// Schema: nvarchar(50) TransferNo (unique), smallint SourceWarehouseId FK,
/// smallint DestinationWarehouseId FK, nvarchar(300) Notes,
/// tinyint Status (Draft=1,Posted=2,Cancelled=3).
/// BaseEntity with CreatedAt only.
/// </summary>
public class WarehouseTransfer : Entity
{
    public string TransferNo { get; private set; } = string.Empty;
    public short SourceWarehouseId { get; private set; }
    public short DestinationWarehouseId { get; private set; }
    public string? Notes { get; private set; }
    public InvoiceStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public int CreatedByUserId { get; private set; }
    public DateTime? PostedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    /// <summary>
    /// Branch identifier for multi-branch support (smallint FK).
    /// </summary>
    public short BranchId { get; private set; }

    // Navigation properties
    public virtual Warehouse? SourceWarehouse { get; private set; }
    public virtual Warehouse? DestinationWarehouse { get; private set; }

    /// <summary>
    /// Navigation property to the Branch this transfer belongs to.
    /// </summary>
    public virtual Branch? Branch { get; private set; }
    private readonly List<WarehouseTransferLine> _lines = new();
    public IReadOnlyCollection<WarehouseTransferLine> Lines => _lines.AsReadOnly();

    private WarehouseTransfer() { } // EF Core

    public static WarehouseTransfer Create(
        short branchId,
        string transferNo,
        short sourceWarehouseId,
        short destinationWarehouseId,
        string? notes = null,
        int? createdByUserId = null)
    {
        if (branchId <= 0)
            throw new DomainException("الفرع مطلوب.");
        if (string.IsNullOrWhiteSpace(transferNo))
            throw new DomainException("رقم التحويل مطلوب.");
        if (sourceWarehouseId <= 0)
            throw new DomainException("المستودع المصدر مطلوب.");
        if (destinationWarehouseId <= 0)
            throw new DomainException("المستودع الوجهة مطلوب.");
        if (sourceWarehouseId == destinationWarehouseId)
            throw new DomainException("المستودع المصدر والوجهة يجب أن يكونا مختلفين.");

        var t = new WarehouseTransfer
        {
            BranchId = branchId,
            Status = InvoiceStatus.Draft,
            TransferNo = transferNo.Trim(),
            SourceWarehouseId = sourceWarehouseId,
            DestinationWarehouseId = destinationWarehouseId,
            Notes = notes,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = createdByUserId ?? 0
        };
        return t;
    }

    public void AddLine(WarehouseTransferLine line)
    {
        if (line == null)
            throw new DomainException("الصنف مطلوب.");
        if (Status != InvoiceStatus.Draft)
            throw new DomainException("لا يمكن إضافة أصناف لتحويل غير مسودة.");
        _lines.Add(line);
    }

    public void Post()
    {
        if (Status != InvoiceStatus.Draft)
            throw new DomainException("فقط التحويلات المسودة يمكن ترحيلها.");
        if (!_lines.Any())
            throw new DomainException("لا يمكن ترحيل تحويل بدون أصناف.");
        Status = InvoiceStatus.Posted;
        PostedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == InvoiceStatus.Cancelled)
            throw new DomainException("التحويل ملغي بالفعل.");
        Status = InvoiceStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
    }
}
