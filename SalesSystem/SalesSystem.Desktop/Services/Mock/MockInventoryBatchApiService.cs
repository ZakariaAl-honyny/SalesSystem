using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة دفعات المخزون — تعيد بيانات من MockInventoryData.
/// </summary>
public class MockInventoryBatchApiService : IInventoryBatchApiService
{
    public Task<Result<IReadOnlyList<InventoryBatchDto>>> GetByProductAsync(int productId, int? warehouseId = null, CancellationToken ct = default)
    {
        IEnumerable<InventoryBatchDto> query = MockInventoryData.Batches
            .Where(b => b.ProductId == productId);

        if (warehouseId.HasValue)
            query = query.Where(b => b.WarehouseId == warehouseId.Value);

        return Task.FromResult(Result<IReadOnlyList<InventoryBatchDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<InventoryBatchDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var batch = MockInventoryData.Batches.FirstOrDefault(b => b.Id == id);
        return batch is null
            ? Task.FromResult(Result<InventoryBatchDto>.Failure("الدفعة غير موجودة"))
            : Task.FromResult(Result<InventoryBatchDto>.Success(batch));
    }

    public Task<Result<InventoryBatchDto>> CreateAsync(CreateInventoryBatchRequest request, CancellationToken ct = default)
    {
        var nextId = MockInventoryData.Batches.Max(b => b.Id) + 1;
        var product = MockData.Products.FirstOrDefault(p => p.Id == request.ProductId);
        var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == request.WarehouseId);

        var batch = new InventoryBatchDto(
            Id: nextId,
            ProductId: request.ProductId,
            ProductName: product?.Name ?? $"منتج {request.ProductId}",
            PurchaseInvoiceId: request.PurchaseInvoiceId,
            PurchaseInvoiceLineId: request.PurchaseInvoiceLineId,
            WarehouseId: request.WarehouseId,
            WarehouseName: warehouse?.Name ?? $"مستودع {request.WarehouseId}",
            BatchNo: request.BatchNo,
            QuantityReceived: request.QuantityReceived,
            QuantityRemaining: request.QuantityReceived,
            UnitCost: request.UnitCost,
            ExpiryDate: request.ExpiryDate,
            SupplierBatchNo: request.SupplierBatchNo);

        MockInventoryData.Batches.Add(batch);
        return Task.FromResult(Result<InventoryBatchDto>.Success(batch));
    }

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockInventoryData.Batches.FirstOrDefault(b => b.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الدفعة غير موجودة"));

        // In mock mode, "deactivate" means setting QuantityRemaining to 0
        var index = MockInventoryData.Batches.IndexOf(existing);
        MockInventoryData.Batches[index] = existing with
        {
            QuantityRemaining = 0m
        };
        return Task.FromResult(Result.Success());
    }
}
