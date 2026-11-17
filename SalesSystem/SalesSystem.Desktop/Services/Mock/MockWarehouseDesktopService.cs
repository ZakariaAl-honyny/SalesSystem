using SalesSystem.Common.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة المخازن — تعيد بيانات من MockData.
/// </summary>
public class MockWarehouseDesktopService : IWarehouseDesktopService
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

    // ── تكافؤ الرفض مع الـBLL (RULE-N167): المقارنة النصية في البيت الواحد StringExtensions.SameText ──
    private static bool _NameExists(string? name, short? exceptId)
        => !string.IsNullOrWhiteSpace(name) &&
           MockData.Warehouses.Any(w => w.Id != exceptId &&
               w.Name.SameText(name));

    public Task<Result<WarehouseDto>> CreateAsync(CreateWarehouseRequest r, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(r.Name))
            return Task.FromResult(Result<WarehouseDto>.Failure("اسم المخزن مطلوب"));

        if (_NameExists(r.Name, null))
            return Task.FromResult(Result<WarehouseDto>.Failure("اسم المخزن مستخدم بالفعل"));

        var nextId = (short)(MockData.Warehouses.Max(w => w.Id) + 1);
        var warehouse = new WarehouseDto(
            Id: nextId, Name: r.Name, Phone: r.Phone,
            Address: r.Address, Notes: r.Notes, IsActive: true);
        MockData.Warehouses.Add(warehouse);
        return Task.FromResult(Result<WarehouseDto>.Success(warehouse));
    }

    /// <summary>
    /// تعديل المخزن — <b>لا يُعطَّل مخزن عليه أرصدة</b> (تكافؤ <c>DeleteBehavior.Restrict</c>
    /// في الـBLL — RULE-N167). الأرصدة المخزنية الوحيدة المرتبطة بمخزن في بيانات التجربة
    /// هي كميات <see cref="MockInventoryData.Batches"/> المتبقية لكل مخزن.
    /// </summary>
    public Task<Result<WarehouseDto>> UpdateAsync(int id, UpdateWarehouseRequest r, CancellationToken ct = default)
    {
        try
        {
            var existing = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)id);
            if (existing is null)
                return Task.FromResult(Result<WarehouseDto>.Failure("المخزن غير موجود"));

            if (string.IsNullOrWhiteSpace(r.Name))
                return Task.FromResult(Result<WarehouseDto>.Failure("اسم المخزن مطلوب"));

            if (_NameExists(r.Name, (short)id))
                return Task.FromResult(Result<WarehouseDto>.Failure("اسم المخزن مستخدم بالفعل"));

            // رفض التعطيل حين يوجد رصيد متبقٍ في دفعات هذا المخزن — والتحقق قبل أي تغيير
            if (!r.IsActive && existing.IsActive && _RemainingStockInWarehouse((short)id) > 0m)
                return Task.FromResult(Result<WarehouseDto>.Failure("لا يمكن تعطيل مخزن عليه أرصدة."));

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
        catch (Exception)
        {
            return Task.FromResult(Result<WarehouseDto>.Failure("تعذر تعديل المخزن"));
        }
    }

    /// <summary>الرصيد المتبقي في مخزن واحد — من كميات الدفعات المتبقية (بيانات التجربة الوحيدة المرتبطة بمخزن).</summary>
    private static decimal _RemainingStockInWarehouse(short warehouseId)
        => MockInventoryData.Batches
            .Where(b => b.WarehouseId == warehouseId && b.QuantityRemaining > 0m)
            .Sum(b => b.QuantityRemaining);

    /// <summary>
    /// تعيين المخزن الافتراضي — <b>رفض صريح لا نجاح كاذب</b> (RULE-N167).
    /// <para>
    /// <b>سبب الرفض:</b> <c>WarehouseDto.IsDefault</c> في عقد الـDTO خاصية <b>محسوبة للقراءة فقط</b>
    /// قيمتها ثابتة <c>false</c> — ليست عَلَماً قابلاً للضبط ولا معاملاً في السجل، فلا يمكن تثبيت
    /// مخزن افتراضي في المحاكاة بلا تغيير عقد الـDTO (وهو خارج نطاق المرحلة أ). وإعلان النجاح
    /// بلا أثر هو زرّ يكذب على المالك، فالمحاكاة ترفض بوضوح حتى يُتاح العَلَم في العقد.
    /// </para>
    /// </summary>
    public Task<Result> SetDefaultAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)id);
            if (existing is null)
                return Task.FromResult(Result.Failure("المخزن غير موجود"));

            if (!existing.IsActive)
                return Task.FromResult(Result.Failure("لا يمكن تعيين مخزن معطّل كمخزن افتراضي"));

            return Task.FromResult(Result.Failure(
                "تعيين المخزن الافتراضي غير مدعوم في وضع التجربة بعد.", ErrorCodes.InvalidOperation));
        }
        catch (Exception)
        {
            return Task.FromResult(Result.Failure("تعذر تعيين المخزن الافتراضي"));
        }
    }

    public Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)id);
        if (existing is null)
            return Task.FromResult(Result.Failure("المخزن غير موجود"));

        var index = MockData.Warehouses.IndexOf(existing);
        MockData.Warehouses[index] = existing with { IsActive = true };

        return Task.FromResult(Result.Success());
    }
}
