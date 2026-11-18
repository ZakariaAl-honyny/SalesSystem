using SalesSystem.Common.Enums;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة المنتجات (منتجات + وحدات) — يستدعي طبقة الأعمال داخل نفس العملية (بدون HTTP).
/// </summary>
public class DirectProductService : IProductDesktopService
{
    private readonly BizInterfaces.IProductService _products;
    private readonly BizInterfaces.IProductUnitService _productUnits;

    public DirectProductService(
        BizInterfaces.IProductService products,
        BizInterfaces.IProductUnitService productUnits)
    {
        _products = products;
        _productUnits = productUnits;
    }

    public async Task<Result<IReadOnlyList<ProductDto>>> GetAllAsync(
        string? search = null, int? categoryId = null, bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // PagedResult<ProductDto> → IReadOnlyList<ProductDto>
            var r = await _products.GetAllAsync(search, categoryId, 1, int.MaxValue, includeInactive, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<ProductDto>>.Success(r.Value!.Items.ToList())
                : Result<IReadOnlyList<ProductDto>>.Failure(r.Error ?? "تعذر تحميل قائمة المنتجات", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<ProductDto>>.Failure("تعذر تحميل قائمة المنتجات", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<ProductDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _products.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<ProductDto>.Failure("تعذر تحميل بيانات المنتج", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<ProductDto>> GetByBarcodeAsync(string barcode, CancellationToken ct = default)
    {
        try
        {
            return await _products.GetByBarcodeAsync(barcode, ct);
        }
        catch (Exception)
        {
            return Result<ProductDto>.Failure("تعذر تحميل المنتج بالباركود", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _products.CreateAsync(r, ct);
        }
        catch (Exception)
        {
            return Result<ProductDto>.Failure("تعذر إضافة المنتج", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _products.UpdateAsync(id, r, ct);
        }
        catch (Exception)
        {
            return Result<ProductDto>.Failure("تعذر تحديث بيانات المنتج", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _products.DeleteAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر تعطيل المنتج", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _products.ReactivateAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر إعادة تفعيل المنتج", ErrorCodes.InvalidOperation);
        }
    }

    // ─── وحدات المنتج ────────────────────────────────────────────────────────

    public async Task<Result<IReadOnlyList<ProductUnitDto>>> GetProductUnitsAsync(int productId, CancellationToken ct = default)
    {
        try
        {
            var r = await _productUnits.GetByProductIdAsync(productId, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<ProductUnitDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<ProductUnitDto>>.Failure(r.Error ?? "تعذر تحميل وحدات المنتج", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<ProductUnitDto>>.Failure("تعذر تحميل وحدات المنتج", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<ProductUnitDto>> AddProductUnitAsync(int productId, AddProductUnitRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _productUnits.AddUnitAsync(productId, r, ct);
        }
        catch (Exception)
        {
            return Result<ProductUnitDto>.Failure("تعذر إضافة وحدة المنتج", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<ProductUnitDto>> UpdateProductUnitAsync(int productId, int productUnitId, UpdateProductUnitRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _productUnits.UpdateUnitAsync(productId, productUnitId, r, ct);
        }
        catch (Exception)
        {
            return Result<ProductUnitDto>.Failure("تعذر تحديث وحدة المنتج", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result> DeleteProductUnitAsync(int productId, int productUnitId, DeleteStrategy strategy = DeleteStrategy.Permanent, CancellationToken ct = default)
    {
        try
        {
            return await _productUnits.DeleteUnitAsync(productId, productUnitId, strategy, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر حذف وحدة المنتج", ErrorCodes.InvalidOperation);
        }
    }
}
