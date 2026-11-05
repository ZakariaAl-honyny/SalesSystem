namespace SalesSystem.DTO;

// ═══════════════════════════════════════════════════════════════
// New Inventory Module DTOs (v4.10+)
// ═══════════════════════════════════════════════════════════════

public record InventoryTransactionDto(
    int Id,
    string TransactionNo,
    byte MovementType,
    short WarehouseId,
    string? WarehouseName,
    int? ReferenceId,
    byte? ReferenceType,
    string? Notes,
    DateTime CreatedAt,
    int CreatedByUserId,
    IReadOnlyList<InventoryTransactionLineDto> Lines)
{
    public string MovementTypeDisplay => MovementType switch
    {
        1 => "مشتريات",
        2 => "مرتجع مشتريات",
        3 => "مبيعات",
        4 => "مرتجع مبيعات",
        5 => "تحويل خارج",
        6 => "تحويل داخل",
        7 => "جرد",
        8 => "تسوية",
        9 => "تلف",
        10 => "رصيد افتتاحي",
        11 => "صرف داخلي",
        12 => "استلام داخلي",
        _ => "غير معروف"
    };
}

public record InventoryTransactionLineDto(
    int Id,
    int InventoryTransactionId,
    int ProductUnitId,
    string? ProductUnitName,
    decimal Quantity,
    decimal UnitCost,
    string? BatchNo,
    DateOnly? ExpiryDate,
    short? WarehouseId);
