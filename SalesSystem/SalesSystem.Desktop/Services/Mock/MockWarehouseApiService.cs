using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة المخازن — تعيد بيانات من MockData.
/// </summary>
public class MockWarehouseApiService : IWarehouseApiService
{
    public Task<Result<IReadOnlyList<WarehouseDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<WarehouseDto> query = MockData.Warehouses;

        if (!includeInactive)
            query = query.Where(w => w.IsActive);

        return Task.FromResult(Result<IReadOnlyList<WarehouseDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<WarehouseDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)id);
        return warehouse is null
            ? Task.FromResult(Result<WarehouseDto>.Failure("المخزن غير موجود"))
            : Task.FromResult(Result<WarehouseDto>.Success(warehouse));
    }

    public Task<Result<IReadOnlyList<WarehouseStockDto>>> GetStockAsync(int warehouseId, CancellationToken ct = default)
    {
        // إرجاع مخزون تجريبي للمنتجات
        var stocks = MockData.Products
            .Where(p => p.IsActive)
            .Select(p => new WarehouseStockDto(
                WarehouseId: warehouseId,
                WarehouseName: MockData.Warehouses.FirstOrDefault(w => w.Id == (short)warehouseId)?.Name,
                ProductId: p.Id,
                ProductName: p.Name,
                UnitName: MockData.ProductUnits.FirstOrDefault(u => u.ProductId == p.Id)?.UnitName,
                Quantity: p.CurrentStock,
                AvgCost: 0m))
            .ToList()
            .AsReadOnly();

        return Task.FromResult(Result<IReadOnlyList<WarehouseStockDto>>.Success(stocks));
    }

    public Task<Result<WarehouseDto>> CreateAsync(CreateWarehouseRequest r, CancellationToken ct = default)
    {
        var nextId = (short)(MockData.Warehouses.Max(w => w.Id) + 1);
        var warehouse = new WarehouseDto(
            Id: nextId, Name: r.Name, Phone: r.Phone,
            Address: r.Address, Notes: r.Notes, IsActive: true,
            BranchId: r.BranchId);
        MockData.Warehouses.Add(warehouse);
        return Task.FromResult(Result<WarehouseDto>.Success(warehouse));
    }

    public Task<Result<WarehouseDto>> UpdateAsync(int id, UpdateWarehouseRequest r, CancellationToken ct = default)
    {
        var existing = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)id);
        if (existing is null)
            return Task.FromResult(Result<WarehouseDto>.Failure("المخزن غير موجود"));

        var updated = existing with
        {
            Name = r.Name,
            Phone = r.Phone,
            Address = r.Address,
            Notes = r.Notes,
            IsActive = r.IsActive
        };

        var index = MockData.Warehouses.IndexOf(existing);
        MockData.Warehouses[index] = updated;
        return Task.FromResult(Result<WarehouseDto>.Success(updated));
    }

    public Task<Result> SetDefaultAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)id);
        if (existing is null)
            return Task.FromResult(Result.Failure("المخزن غير موجود"));

        // في المحاكاة، لا نحتاج لتعديل شيء — نعيد نجاح فقط
        return Task.FromResult(Result.Success());
    }
}
