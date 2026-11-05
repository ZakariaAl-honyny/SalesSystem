namespace SalesSystem.DTO;

public record ProductDto(
    int Id,
    string Name,
    int CategoryId,
    string? CategoryName,
    string? Description,
    decimal ReorderLevel,
    bool TrackExpiry,
    string? ImagePath,
    bool IsActive,
    string? Barcode = null,
    decimal CurrentStock = 0,
    short BranchId = 0)
{
    public bool IsOutOfStock => CurrentStock <= 0;
    public bool IsLowStock => CurrentStock > 0 && CurrentStock <= ReorderLevel;
    public string StockStatusLabel => IsOutOfStock ? "نفذ" : IsLowStock ? "محدود" : "";

    // Backward-compatible properties
    public decimal SalePrice => 0m;
    public decimal PurchasePrice => 0m;
    public int UnitId => 0;
    public string? UnitName => null;
    public decimal MinStock => ReorderLevel;
}

// Product Import DTOs
// ──────────────────────────────────────────────
public record ProductImportRowDto(
    string ProductName,
    string? CategoryName,
    string? Barcode,
    short? BaseUnitId,
    decimal? MinStockLevel,
    string? Description
);

public record ProductImportResultDto(
    int TotalRows,
    int SuccessCount,
    int FailureCount,
    List<ProductImportErrorDto> Errors
);

public record ProductImportErrorDto(
    int RowNumber,
    string ProductName,
    string ErrorMessage
);
