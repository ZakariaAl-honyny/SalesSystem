using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة جرد المخزون — تعيد بيانات من MockInventoryData.
/// </summary>
public class MockInventoryCountApiService : IInventoryCountApiService
{
    public Task<Result<IReadOnlyList<InventoryCountDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<InventoryCountDto> query = MockInventoryData.Counts;

        if (!includeInactive)
            query = query.Where(c => c.Status != 3); // Exclude Cancelled

        return Task.FromResult(Result<IReadOnlyList<InventoryCountDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<InventoryCountDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var count = MockInventoryData.Counts.FirstOrDefault(c => c.Id == id);
        return count is null
            ? Task.FromResult(Result<InventoryCountDto>.Failure("سجل الجرد غير موجود"))
            : Task.FromResult(Result<InventoryCountDto>.Success(count));
    }

    public Task<Result<InventoryCountDto>> CreateAsync(CreateInventoryCountRequest request, CancellationToken ct = default)
    {
        var nextId = MockInventoryData.Counts.Max(c => c.Id) + 1;
        var countNo = $"ST-2026-{nextId:D3}";
        var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == request.WarehouseId);

        var count = new InventoryCountDto(
            Id: nextId,
            CountNo: countNo,
            WarehouseId: request.WarehouseId,
            WarehouseName: warehouse?.Name ?? $"مستودع {request.WarehouseId}",
            Status: 1,
            StatusName: "مسودة",
            Notes: request.Notes,
            CreatedAt: DateTime.Now,
            CreatedByUserId: 1,
            Lines: new List<InventoryCountLineDto>());
        // TODO: BranchId / BranchName removed from InventoryCountDto.

        MockInventoryData.Counts.Add(count);
        return Task.FromResult(Result<InventoryCountDto>.Success(count));
    }

    public Task<Result<InventoryCountDto>> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockInventoryData.Counts.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result<InventoryCountDto>.Failure("سجل الجرد غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result<InventoryCountDto>.Failure("فقط الجرد المسودة يمكن ترحيله"));

        var index = MockInventoryData.Counts.IndexOf(existing);
        var posted = existing with
        {
            Status = 2,
            StatusName = "مرحّل"
        };
        // TODO: PostedAt removed from InventoryCountDto.
        MockInventoryData.Counts[index] = posted;
        return Task.FromResult(Result<InventoryCountDto>.Success(posted));
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockInventoryData.Counts.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("سجل الجرد غير موجود"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("الجرد ملغى بالفعل"));

        var index = MockInventoryData.Counts.IndexOf(existing);
        MockInventoryData.Counts[index] = existing with
        {
            Status = 3,
            StatusName = "ملغي"
        };
        // TODO: CancelledAt removed from InventoryCountDto.
        return Task.FromResult(Result.Success());
    }

    public Task<Result<InventoryCountDto>> AddLineAsync(int id, AddInventoryCountLineRequest request, CancellationToken ct = default)
    {
        var existing = MockInventoryData.Counts.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result<InventoryCountDto>.Failure("سجل الجرد غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result<InventoryCountDto>.Failure("لا يمكن إضافة أصناف لجرد غير مسودة"));

        var nextLineId = MockInventoryData.Counts
            .SelectMany(c => c.Lines ?? new List<InventoryCountLineDto>())
            .Max(l => l.Id) + 1;

        var productUnit = MockData.ProductUnits.FirstOrDefault(pu => pu.Id == request.ProductUnitId);
        var productName = MockData.Products.FirstOrDefault(p => p.Id == productUnit?.ProductId)?.Name;

        var line = new InventoryCountLineDto(
            Id: nextLineId,
            InventoryCountId: id,
            ProductUnitId: request.ProductUnitId,
            ProductUnitName: $"{productName} — {productUnit?.UnitName ?? "وحدة"}",
            ExpectedQuantity: request.ExpectedQuantity,
            ActualQuantity: request.ActualQuantity,
            Difference: request.ActualQuantity - request.ExpectedQuantity,
            Notes: request.Notes);
        // TODO: ProductName / ProductId / UnitName removed from InventoryCountLineDto.

        var lines = existing.Lines is { Count: > 0 }
            ? new List<InventoryCountLineDto>(existing.Lines) { line }
            : new List<InventoryCountLineDto> { line };

        var index = MockInventoryData.Counts.IndexOf(existing);
        MockInventoryData.Counts[index] = existing with { Lines = lines };

        return Task.FromResult(Result<InventoryCountDto>.Success(MockInventoryData.Counts[index]));
    }
}
