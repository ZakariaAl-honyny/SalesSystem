namespace SalesSystem.DTO.Requests;

public record CreateProductRequest(
    string Name,
    int CategoryId,
    string? Description = null,
    decimal ReorderLevel = 0,
    bool TrackExpiry = false,
    string? ImagePath = null,
    string? Barcode = null,
    string? ForeignName = null,
    string? LocationInStore = null,
    short? ExpiryAlertDays = null,
    short? BranchId = null
);

public record UpdateProductRequest(
    string Name,
    int CategoryId,
    string? Description = null,
    decimal ReorderLevel = 0,
    bool TrackExpiry = false,
    string? ImagePath = null,
    string? Barcode = null,
    string? ForeignName = null,
    string? LocationInStore = null,
    short? ExpiryAlertDays = null,
    bool IsActive = true
);
