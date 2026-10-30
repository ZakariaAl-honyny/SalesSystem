using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة المستودعات — يستدعي طبقة الأعمال (المستودعات + المخزون) داخل نفس العملية (بدون HTTP).
/// </summary>
public class DirectWarehouseService : IWarehouseApiService
{
    private readonly BizInterfaces.IWarehouseService _warehouses;
    private readonly BizInterfaces.IInventoryService _inventory;

    public DirectWarehouseService(
        BizInterfaces.IWarehouseService warehouses,
        BizInterfaces.IInventoryService inventory)
    {
        _warehouses = warehouses;
        _inventory = inventory;
    }

    public async Task<Result<IReadOnlyList<WarehouseDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // PagedResult<WarehouseDto> → IReadOnlyList<WarehouseDto>
            var r = await _warehouses.GetAllAsync(null, 1, int.MaxValue, includeInactive, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<WarehouseDto>>.Success(r.Value!.Items.ToList())
                : Result<IReadOnlyList<WarehouseDto>>.Failure(r.Error ?? "تعذر تحميل قائمة المستودعات", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<WarehouseDto>>.Failure("تعذر تحميل قائمة المستودعات", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<WarehouseDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _warehouses.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<WarehouseDto>.Failure("تعذر تحميل بيانات المستودع", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<IReadOnlyList<WarehouseStockDto>>> GetStockAsync(int warehouseId, CancellationToken ct = default)
    {
        try
        {
            // IEnumerable<WarehouseStockDto> → IReadOnlyList<WarehouseStockDto>
            var r = await _inventory.GetWarehouseStockAsync(warehouseId, null, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<WarehouseStockDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<WarehouseStockDto>>.Failure(r.Error ?? "تعذر تحميل أرصدة المستودع", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<WarehouseStockDto>>.Failure("تعذر تحميل أرصدة المستودع", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<WarehouseDto>> CreateAsync(CreateWarehouseRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _warehouses.CreateAsync(r, ct);
        }
        catch (Exception)
        {
            return Result<WarehouseDto>.Failure("تعذر إضافة المستودع", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<WarehouseDto>> UpdateAsync(int id, UpdateWarehouseRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _warehouses.UpdateAsync(id, r, ct);
        }
        catch (Exception)
        {
            return Result<WarehouseDto>.Failure("تعذر تحديث بيانات المستودع", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result> SetDefaultAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _warehouses.SetDefaultAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر تعيين المستودع الافتراضي", ErrorCodes.InvalidOperation);
        }
    }
}
