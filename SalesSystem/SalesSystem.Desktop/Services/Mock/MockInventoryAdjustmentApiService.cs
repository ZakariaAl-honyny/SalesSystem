using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة تسويات المخزون — تعيد بيانات من MockInventoryData.
/// </summary>
public class MockInventoryAdjustmentApiService : IInventoryAdjustmentApiService
{
    public Task<Result<IReadOnlyList<InventoryAdjustmentDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<InventoryAdjustmentDto> query = MockInventoryData.Adjustments;

        if (!includeInactive)
            query = query.Where(a => a.Status != 3); // Exclude Cancelled

        return Task.FromResult(Result<IReadOnlyList<InventoryAdjustmentDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<InventoryAdjustmentDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var adj = MockInventoryData.Adjustments.FirstOrDefault(a => a.Id == id);
        return adj is null
            ? Task.FromResult(Result<InventoryAdjustmentDto>.Failure("تسوية المخزون غير موجودة"))
            : Task.FromResult(Result<InventoryAdjustmentDto>.Success(adj));
    }

    public Task<Result<InventoryAdjustmentDto>> CreateAsync(CreateInventoryAdjustmentRequest request, CancellationToken ct = default)
    {
        var nextId = MockInventoryData.Adjustments.Max(a => a.Id) + 1;
        var adjNo = $"ADJ-2026-{nextId:D3}";
        var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == request.WarehouseId);

        string adjTypeName = request.AdjustmentType switch
        {
            1 => "إضافة",
            2 => "خصم",
            3 => "تصحيح",
            _ => "غير محدد"
        };

        var adj = new InventoryAdjustmentDto(
            Id: nextId,
            AdjustmentNo: adjNo,
            WarehouseId: request.WarehouseId,
            WarehouseName: warehouse?.Name ?? $"مستودع {request.WarehouseId}",
            AdjustmentType: request.AdjustmentType,
            AdjustmentTypeName: adjTypeName,
            Reason: request.Reason,
            Status: 1,
            StatusName: "مسودة",
            CreatedAt: DateTime.Now,
            CreatedByUserId: 1,
            PostedAt: null,
            CancelledAt: null,
            Lines: new List<InventoryAdjustmentLineDto>());
        // TODO: BranchId / BranchName removed from InventoryAdjustmentDto.

        MockInventoryData.Adjustments.Add(adj);
        return Task.FromResult(Result<InventoryAdjustmentDto>.Success(adj));
    }

    public Task<Result<InventoryAdjustmentDto>> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockInventoryData.Adjustments.FirstOrDefault(a => a.Id == id);
        if (existing is null)
            return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("تسوية المخزون غير موجودة"));

        if (existing.Status != 1)
            return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("فقط التسويات المسودة يمكن ترحيلها"));

        if (existing.Lines is null || existing.Lines.Count == 0)
            return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("لا يمكن ترحيل تسوية بدون أصناف"));

        var index = MockInventoryData.Adjustments.IndexOf(existing);
        var posted = existing with
        {
            Status = 2,
            StatusName = "مرحّل",
            PostedAt = DateTime.Now
        };
        MockInventoryData.Adjustments[index] = posted;
        return Task.FromResult(Result<InventoryAdjustmentDto>.Success(posted));
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockInventoryData.Adjustments.FirstOrDefault(a => a.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("تسوية المخزون غير موجودة"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("التسوية ملغاة بالفعل"));

        var index = MockInventoryData.Adjustments.IndexOf(existing);
        MockInventoryData.Adjustments[index] = existing with
        {
            Status = 3,
            StatusName = "ملغي",
            CancelledAt = DateTime.Now
        };
        return Task.FromResult(Result.Success());
    }

    public Task<Result<InventoryAdjustmentDto>> AddLineAsync(int id, AddInventoryAdjustmentLineRequest request, CancellationToken ct = default)
    {
        var existing = MockInventoryData.Adjustments.FirstOrDefault(a => a.Id == id);
        if (existing is null)
            return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("تسوية المخزون غير موجودة"));

        if (existing.Status != 1)
            return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("لا يمكن إضافة أصناف لتسوية غير مسودة"));

        var nextLineId = MockInventoryData.Adjustments
            .SelectMany(a => a.Lines ?? new List<InventoryAdjustmentLineDto>())
            .Max(l => l.Id) + 1;

        var productUnit = MockData.ProductUnits.FirstOrDefault(pu => pu.Id == request.ProductUnitId);
        var productName = MockData.Products.FirstOrDefault(p => p.Id == productUnit?.ProductId)?.Name;

        var line = new InventoryAdjustmentLineDto(
            Id: nextLineId,
            InventoryAdjustmentId: id,
            ProductUnitId: request.ProductUnitId,
            ProductUnitName: $"{productName} — {productUnit?.UnitName ?? "وحدة"}",
            ExpectedQuantity: request.ExpectedQuantity,
            ActualQuantity: request.ActualQuantity,
            UnitCost: request.UnitCost);
        // TODO: ProductName / ProductId / UnitName removed from InventoryAdjustmentLineDto.

        var lines = existing.Lines is { Count: > 0 }
            ? new List<InventoryAdjustmentLineDto>(existing.Lines) { line }
            : new List<InventoryAdjustmentLineDto> { line };

        var index = MockInventoryData.Adjustments.IndexOf(existing);
        MockInventoryData.Adjustments[index] = existing with { Lines = lines };

        return Task.FromResult(Result<InventoryAdjustmentDto>.Success(MockInventoryData.Adjustments[index]));
    }
}
