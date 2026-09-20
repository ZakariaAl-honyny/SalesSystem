namespace SalesSystem.DTO.Requests;

public record CreateStockTransferRequest(
    int FromWarehouseId,
    int ToWarehouseId,
    DateTime TransferDate,
    string? Notes,
    List<CreateStockTransferItemRequest> Items,
    short? BranchId = null);

public record CreateStockTransferItemRequest(
    int ProductId,
    decimal Quantity,
    string? Notes);
