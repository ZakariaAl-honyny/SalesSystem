using SalesSystem.Common.Enums;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة المنتجات — تعيد بيانات من MockData.
/// </summary>
public class MockProductApiService : IProductApiService
{
    public Task<Result<IReadOnlyList<ProductDto>>> GetAllAsync(string? search = null, int? categoryId = null, bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<ProductDto> query = MockData.Products;

        if (!includeInactive)
            query = query.Where(p => p.IsActive);

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(p =>
                p.Name.Contains(s, StringComparison.OrdinalIgnoreCase) ||
                (p.Barcode != null && p.Barcode.Contains(s)) ||
                (p.CategoryName != null && p.CategoryName.Contains(s, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult(Result<IReadOnlyList<ProductDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<ProductDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var product = MockData.Products.FirstOrDefault(p => p.Id == id);
        return product is null
            ? Task.FromResult(Result<ProductDto>.Failure("المنتج غير موجود"))
            : Task.FromResult(Result<ProductDto>.Success(product));
    }

    public Task<Result<ProductDto>> GetByBarcodeAsync(string barcode, CancellationToken ct = default)
    {
        var product = MockData.Products.FirstOrDefault(p => p.Barcode == barcode);
        return product is null
            ? Task.FromResult(Result<ProductDto>.Failure("المنتج غير موجود بالباركود"))
            : Task.FromResult(Result<ProductDto>.Success(product));
    }

    public Task<Result<ProductDto>> CreateAsync(CreateProductRequest r, CancellationToken ct = default)
    {
        var nextId = MockData.Products.Max(p => p.Id) + 1;
        var categoryName = MockData.Products.FirstOrDefault(p => p.CategoryId == r.CategoryId)?.CategoryName;
        var product = new ProductDto(
            Id: nextId, Name: r.Name, CategoryId: r.CategoryId,
            CategoryName: categoryName, Description: r.Description,
            ReorderLevel: r.ReorderLevel, TrackExpiry: r.TrackExpiry,
            ImagePath: r.ImagePath, IsActive: true, Barcode: r.Barcode);
        MockData.Products.Add(product);
        return Task.FromResult(Result<ProductDto>.Success(product));
    }

    public Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductRequest r, CancellationToken ct = default)
    {
        var existing = MockData.Products.FirstOrDefault(p => p.Id == id);
        if (existing is null)
            return Task.FromResult(Result<ProductDto>.Failure("المنتج غير موجود"));

        var categoryName = MockData.Products.FirstOrDefault(p => p.CategoryId == r.CategoryId)?.CategoryName;
        var updated = existing with
        {
            Name = r.Name,
            CategoryId = r.CategoryId,
            CategoryName = categoryName,
            Description = r.Description,
            ReorderLevel = r.ReorderLevel,
            TrackExpiry = r.TrackExpiry,
            ImagePath = r.ImagePath,
            Barcode = r.Barcode,
            IsActive = r.IsActive
        };

        var index = MockData.Products.IndexOf(existing);
        MockData.Products[index] = updated;
        return Task.FromResult(Result<ProductDto>.Success(updated));
    }

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.Products.FirstOrDefault(p => p.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("المنتج غير موجود"));

        var index = MockData.Products.IndexOf(existing);
        MockData.Products[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.Products.FirstOrDefault(p => p.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("المنتج غير موجود"));

        var index = MockData.Products.IndexOf(existing);
        MockData.Products[index] = existing with { IsActive = true };
        return Task.FromResult(Result.Success());
    }

    // ── Product Units ────────────────────────────────────────────────────
    public Task<Result<IReadOnlyList<ProductUnitDto>>> GetProductUnitsAsync(int productId, CancellationToken ct = default)
    {
        var units = MockData.ProductUnits
            .Where(u => u.ProductId == productId)
            .ToList()
            .AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<ProductUnitDto>>.Success(units));
    }

    public Task<Result<ProductUnitDto>> AddProductUnitAsync(int productId, AddProductUnitRequest r, CancellationToken ct = default)
    {
        if (!MockData.Products.Any(p => p.Id == productId))
            return Task.FromResult(Result<ProductUnitDto>.Failure("المنتج غير موجود"));

        if (MockData.ProductUnits.Any(u => u.ProductId == productId && u.UnitId == r.UnitId))
            return Task.FromResult(Result<ProductUnitDto>.Failure("هذه الوحدة مضافة بالفعل للمنتج"));

        var unitName = MockLookupsData.Units.FirstOrDefault(u => u.Id == r.UnitId)?.Name;
        var nextId = MockData.ProductUnits.Count == 0 ? 1 : MockData.ProductUnits.Max(u => u.Id) + 1;

        var productUnit = new ProductUnitDto(
            Id: nextId,
            ProductId: productId,
            UnitId: r.UnitId,
            UnitName: unitName,
            ConversionFactor: r.IsBaseUnit ? 1m : r.Factor,
            IsBaseUnit: r.IsBaseUnit,
            Barcode: string.IsNullOrWhiteSpace(r.Barcode) ? null : r.Barcode.Trim(),
            SalePrice: r.SalePrice,
            WholesalePrice: r.WholesalePrice,
            CostPrice: r.CostPrice,
            MaxDiscountAmount: r.MaxDiscountAmount);

        MockData.ProductUnits.Add(productUnit);
        return Task.FromResult(Result<ProductUnitDto>.Success(productUnit));
    }

    public Task<Result<ProductUnitDto>> UpdateProductUnitAsync(int productId, int productUnitId, UpdateProductUnitRequest r, CancellationToken ct = default)
    {
        var existing = MockData.ProductUnits.FirstOrDefault(u => u.Id == productUnitId && u.ProductId == productId);
        if (existing is null)
            return Task.FromResult(Result<ProductUnitDto>.Failure("وحدة المنتج غير موجودة"));

        var unitName = MockLookupsData.Units.FirstOrDefault(u => u.Id == r.UnitId)?.Name;

        var updated = existing with
        {
            UnitId = r.UnitId,
            UnitName = unitName,
            Barcode = string.IsNullOrWhiteSpace(r.Barcode) ? null : r.Barcode.Trim(),
            SalePrice = r.SalePrice,
            WholesalePrice = r.WholesalePrice,
            CostPrice = r.CostPrice,
            MaxDiscountAmount = r.MaxDiscountAmount
        };
        MockData.ProductUnits[MockData.ProductUnits.IndexOf(existing)] = updated;
        return Task.FromResult(Result<ProductUnitDto>.Success(updated));
    }

    public Task<Result> DeleteProductUnitAsync(int productId, int productUnitId, DeleteStrategy strategy = DeleteStrategy.Permanent, CancellationToken ct = default)
    {
        var existing = MockData.ProductUnits.FirstOrDefault(u => u.Id == productUnitId && u.ProductId == productId);
        if (existing is null)
            return Task.FromResult(Result.Failure("وحدة المنتج غير موجودة"));

        if (MockData.ProductUnits.Count(u => u.ProductId == productId) <= 1)
            return Task.FromResult(Result.Failure("يجب أن يكون للمنتج وحدة قياس واحدة على الأقل"));

        MockData.ProductUnits.Remove(existing);
        return Task.FromResult(Result.Success());
    }

    // ── Product Prices ───────────────────────────────────────────────────
    public Task<Result<IReadOnlyList<ProductPriceDto>>> GetProductPricesByUnitAsync(int productUnitId, CancellationToken ct = default)
    {
        var prices = MockData.ProductPrices
            .Where(p => p.ProductUnitId == productUnitId)
            .ToList()
            .AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<ProductPriceDto>>.Success(prices));
    }

    public Task<Result<ProductPriceDto>> CreateProductPriceAsync(CreateProductPriceRequest r, CancellationToken ct = default)
    {
        var nextId = MockData.ProductPrices.Count == 0 ? 1 : MockData.ProductPrices.Max(p => p.Id) + 1;
        var unitName = MockData.ProductUnits.FirstOrDefault(u => u.Id == r.ProductUnitId)?.UnitName;
        var price = new ProductPriceDto(
            Id: nextId, ProductUnitId: r.ProductUnitId,
            ProductUnitName: unitName, Price: r.Price,
            EffectiveFrom: r.EffectiveFrom, EffectiveTo: r.EffectiveTo);
        MockData.ProductPrices.Add(price);
        return Task.FromResult(Result<ProductPriceDto>.Success(price));
    }

    public Task<Result> DeleteProductPriceAsync(int priceId, CancellationToken ct = default)
    {
        var existing = MockData.ProductPrices.FirstOrDefault(p => p.Id == priceId);
        if (existing is null)
            return Task.FromResult(Result.Failure("السعر غير موجود"));

        MockData.ProductPrices.Remove(existing);
        return Task.FromResult(Result.Success());
    }
}
