using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة دفعات المخزون — تعيد بيانات من MockInventoryData.
/// </summary>
/// <remarks>
/// RULE-N167 — تكافؤ الرفض: رقم الدفعة/الصنف/المخزن إلزامية · كمية الدفعة أكبر من صفر ·
/// لا تكرار لرقم الدفعة لنفس الصنف.
/// </remarks>
public class MockInventoryBatchDesktopService : IInventoryBatchDesktopService
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
        try
        {
            // RULE-N167 — الحقول الإلزامية.
            if (string.IsNullOrWhiteSpace(request.BatchNo))
                return Task.FromResult(Result<InventoryBatchDto>.Failure("رقم الدفعة مطلوب"));

            if (request.ProductId <= 0)
                return Task.FromResult(Result<InventoryBatchDto>.Failure("الصنف مطلوب"));

            if (request.WarehouseId <= 0)
                return Task.FromResult(Result<InventoryBatchDto>.Failure("المخزن مطلوب"));

            // RULE-N167 — كمية الدفعة أكبر من صفر.
            if (request.QuantityReceived <= 0m)
                return Task.FromResult(Result<InventoryBatchDto>.Failure("الكمية يجب أن تكون أكبر من صفر"));

            string batchNo = request.BatchNo.Trim();

            // RULE-N167 — لا رقم دفعة مكرر لنفس الصنف.
            if (MockInventoryData.Batches.Any(b => b.ProductId == request.ProductId
                    && string.Equals(b.BatchNo, batchNo, StringComparison.OrdinalIgnoreCase)))
                return Task.FromResult(Result<InventoryBatchDto>.Failure("رقم الدفعة مستخدم بالفعل لهذا الصنف"));

            var nextId = MockInventoryData.Batches.Select(b => b.Id).DefaultIfEmpty(0).Max() + 1;
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
                BatchNo: batchNo,
                QuantityReceived: request.QuantityReceived,
                QuantityRemaining: request.QuantityReceived,
                UnitCost: request.UnitCost,
                ExpiryDate: request.ExpiryDate,
                SupplierBatchNo: request.SupplierBatchNo);

            MockInventoryData.Batches.Add(batch);
            return Task.FromResult(Result<InventoryBatchDto>.Success(batch));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<InventoryBatchDto>.Failure("تعذر حفظ الدفعة"));
        }
    }

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        try
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
        catch (Exception)
        {
            return Task.FromResult(Result.Failure("تعذر تعطيل الدفعة"));
        }
    }
}
