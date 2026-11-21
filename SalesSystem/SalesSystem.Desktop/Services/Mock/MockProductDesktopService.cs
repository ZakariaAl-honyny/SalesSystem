using SalesSystem.Common.Enums;
using SalesSystem.Common.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة المنتجات — تعيد بيانات من MockData.
/// </summary>
public class MockProductDesktopService : IProductDesktopService
{
    private readonly SalesSystem.Common.ICurrentUserProvider _currentUser;

    public MockProductDesktopService(SalesSystem.Common.ICurrentUserProvider currentUser)
    {
        _currentUser = currentUser;
    }

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
                (p.CategoryName != null && p.CategoryName.Contains(s, StringComparison.OrdinalIgnoreCase)));
        }

        // ProductDto.Barcode قيمة مشتقة من باركود الوحدة الأساسية — لا باركود على الصنف نفسه (TASK-031)
        var products = query.Select(p => p with { Barcode = _BaseUnitBarcode(p.Id) }).ToList().AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<ProductDto>>.Success(products));
    }

    public Task<Result<ProductDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var product = MockData.Products.FirstOrDefault(p => p.Id == id);
        return product is null
            ? Task.FromResult(Result<ProductDto>.Failure("المنتج غير موجود"))
            : Task.FromResult(Result<ProductDto>.Success(product with { Barcode = _BaseUnitBarcode(product.Id) }));
    }

    public Task<Result<ProductDto>> GetByBarcodeAsync(string barcode, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            return Task.FromResult(Result<ProductDto>.Failure("الباركود مطلوب"));

        var code = barcode.Trim();

        // باركود الوحدة هو مصدر الهوية الوحيد (لا باركود على الصنف — قرار المالك 2026-11-20)
        var unit = MockData.ProductUnits.FirstOrDefault(u => !string.IsNullOrWhiteSpace(u.Barcode) && u.Barcode.SameText(code));
        var product = unit is null ? null : MockData.Products.FirstOrDefault(p => p.Id == unit.ProductId);

        return unit is null || product is null
            ? Task.FromResult(Result<ProductDto>.Failure("الباركود غير موجود"))
            : Task.FromResult(Result<ProductDto>.Success(product with { Barcode = unit.Barcode }));
    }

    public Task<Result<BarcodeResolutionDto>> ResolveBarcodeAsync(string barcode, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            return Task.FromResult(Result<BarcodeResolutionDto>.Failure("الباركود مطلوب"));

        var code = barcode.Trim();

        var unit = MockData.ProductUnits.FirstOrDefault(u => !string.IsNullOrWhiteSpace(u.Barcode) && u.Barcode.SameText(code));
        var product = unit is null ? null : MockData.Products.FirstOrDefault(p => p.Id == unit.ProductId);

        if (unit is null || product is null)
            return Task.FromResult(Result<BarcodeResolutionDto>.Failure("الباركود غير موجود"));

        return Task.FromResult(Result<BarcodeResolutionDto>.Success(new BarcodeResolutionDto(
            ProductId: product.Id,
            ProductName: product.Name,
            ProductUnitId: unit.Id,
            UnitId: unit.UnitId,
            UnitName: unit.UnitName,
            ConversionFactor: unit.ConversionFactor,
            Barcode: unit.Barcode)));
    }

    // ── تكافؤ الرفض مع الـBLL (RULE-N167): المقارنة النصية في البيت الواحد StringExtensions.SameText ──
    private static bool _NameExists(string? name, int? exceptId)
        => !string.IsNullOrWhiteSpace(name) &&
           MockData.Products.Any(p => p.Id != exceptId &&
               p.Name.SameText(name));

    /// <summary>
    /// باركود الوحدة الأساسية للصنف — <c>ProductDto.Barcode</c> قيمة مشتقة لا عموداً على الصنف:
    /// من <c>MockData.ProductUnits</c> للوحدة الأساسية، وإلا أول وحدة للصنف (قرار المالك 2026-11-20 — TASK-031).
    /// </summary>
    private static string? _BaseUnitBarcode(int productId)
    {
        var units = MockData.ProductUnits.Where(u => u.ProductId == productId).ToList();
        var unit = units.FirstOrDefault(u => u.IsBaseUnit) ?? units.FirstOrDefault();
        return unit?.Barcode;
    }

    /// <summary>
    /// تكافؤ الرفض مع الفهرس الفريد <c>IX_ProductUnits_Barcode</c>: الباركود فريد على مستوى الوحدات كلها.
    /// </summary>
    private static bool _UnitBarcodeExists(string? barcode, int? exceptUnitId)
        => !string.IsNullOrWhiteSpace(barcode) &&
           MockData.ProductUnits.Any(u => u.Id != exceptUnitId &&
               !string.IsNullOrWhiteSpace(u.Barcode) && u.Barcode.SameText(barcode));

    public Task<Result<ProductDto>> CreateAsync(CreateProductRequest r, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(r.Name))
            return Task.FromResult(Result<ProductDto>.Failure("اسم الصنف مطلوب"));

        if (_NameExists(r.Name, null))
            return Task.FromResult(Result<ProductDto>.Failure("اسم الصنف مستخدم بالفعل"));

        var nextId = MockData.Products.Count == 0 ? 1 : MockData.Products.Max(p => p.Id) + 1;

        // اسم المجموعة يُقرأ من دليل المجموعات (لا من منتج قائم) حتى يظهر صحيحاً للأصناف الجديدة.
        var categoryName = MockLookupsData.Categories.FirstOrDefault(c => c.Id == r.CategoryId)?.Name;

        // الباركود لا يُطلب عند إنشاء الصنف: يُسجَّل لاحقاً على الوحدة (وحدات الصنف لم تُنشأ بعد ⇒ Barcode = null)
        var product = new ProductDto(
            Id: nextId, Name: r.Name, CategoryId: r.CategoryId,
            CategoryName: categoryName, Description: r.Description,
            ReorderLevel: r.ReorderLevel, TrackExpiry: r.TrackExpiry,
            ImagePath: r.ImagePath, IsActive: true,
            ForeignName: r.ForeignName, LocationInStore: r.LocationInStore,
            ExpiryAlertDays: r.ExpiryAlertDays,
            CreatedByUserName: _currentUser.UserName,
            UpdatedByUserName: _currentUser.UserName);
        MockData.Products.Add(product);
        return Task.FromResult(Result<ProductDto>.Success(product));
    }

    public Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductRequest r, CancellationToken ct = default)
    {
        var existing = MockData.Products.FirstOrDefault(p => p.Id == id);
        if (existing is null)
            return Task.FromResult(Result<ProductDto>.Failure("المنتج غير موجود"));

        if (string.IsNullOrWhiteSpace(r.Name))
            return Task.FromResult(Result<ProductDto>.Failure("اسم الصنف مطلوب"));

        if (_NameExists(r.Name, id))
            return Task.FromResult(Result<ProductDto>.Failure("اسم الصنف مستخدم بالفعل"));

        var categoryName = MockLookupsData.Categories.FirstOrDefault(c => c.Id == r.CategoryId)?.Name;

        var updated = existing with
        {
            Name = r.Name,
            CategoryId = r.CategoryId,
            CategoryName = categoryName,
            Description = r.Description,
            ReorderLevel = r.ReorderLevel,
            TrackExpiry = r.TrackExpiry,
            ImagePath = r.ImagePath,
            // الباركود قيمة مشتقة من الوحدة الأساسية — لا يُكتب من الصنف (TASK-031)
            Barcode = _BaseUnitBarcode(id),
            ForeignName = r.ForeignName,
            LocationInStore = r.LocationInStore,
            ExpiryAlertDays = r.ExpiryAlertDays,
            IsActive = r.IsActive,
            // CreatedByUserName يبقى كما هو (لا يُعاد كتابته أبداً في التعديل)
            UpdatedByUserName = _currentUser.UserName
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

        // الفهرس الفريد IX_ProductUnits_Barcode: باركود الوحدة فريد على مستوى النظام (RULE-N167)
        if (_UnitBarcodeExists(r.Barcode, null))
            return Task.FromResult(Result<ProductUnitDto>.Failure("الباركود مستخدم لوحدة أخرى"));

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

        // الفهرس الفريد IX_ProductUnits_Barcode: الباركود مستثنى منه السجل نفسه عند التعديل (RULE-N167)
        if (_UnitBarcodeExists(r.Barcode, productUnitId))
            return Task.FromResult(Result<ProductUnitDto>.Failure("الباركود مستخدم لوحدة أخرى"));

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
}
