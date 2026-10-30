using SalesSystem.Business.Services;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة أسعار المنتجات — يستدعي طبقة الأعمال داخل نفس العملية (بدون HTTP).
/// ملاحظة: السعر الفعلي في طبقة الأعمال يُعاد كـ EffectivePriceDto ويُحوَّل إلى ProductPriceDto هنا.
/// </summary>
public class DirectProductPriceService : IProductPriceApiService
{
    private readonly IProductPriceService _bll;
    private readonly ISessionService _session;

    public DirectProductPriceService(IProductPriceService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<ProductPriceDto>> GetEffectivePriceAsync(
        int productUnitId, DateTime? effectiveDate = null, CancellationToken ct = default)
    {
        try
        {
            var r = await _bll.GetEffectivePriceAsync(productUnitId, effectiveDate, ct);
            return r.IsSuccess
                ? Result<ProductPriceDto>.Success(MapEffectivePrice(r.Value!))
                : Result<ProductPriceDto>.Failure(r.Error ?? "تعذر جلب السعر الفعلي", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<ProductPriceDto>.Failure("تعذر جلب السعر الفعلي", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<ProductPriceDto>> GetEffectivePriceForInvoiceAsync(int productUnitId, CancellationToken ct = default)
    {
        try
        {
            var r = await _bll.GetEffectivePriceForInvoiceAsync(productUnitId, ct);
            return r.IsSuccess
                ? Result<ProductPriceDto>.Success(MapEffectivePrice(r.Value!))
                : Result<ProductPriceDto>.Failure(r.Error ?? "تعذر جلب السعر الفعلي للفاتورة", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<ProductPriceDto>.Failure("تعذر جلب السعر الفعلي للفاتورة", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<IReadOnlyList<ProductPriceDto>>> GetByProductUnitAsync(int productUnitId, CancellationToken ct = default)
    {
        try
        {
            var r = await _bll.GetByProductUnitAsync(productUnitId, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<ProductPriceDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<ProductPriceDto>>.Failure(r.Error ?? "تعذر تحميل أسعار الوحدة", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<ProductPriceDto>>.Failure("تعذر تحميل أسعار الوحدة", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<ProductPriceDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<ProductPriceDto>.Failure("تعذر تحميل بيانات السعر", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<ProductPriceDto>> CreateAsync(CreateProductPriceRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, UserId, ct);
        }
        catch (Exception)
        {
            return Result<ProductPriceDto>.Failure("تعذر إضافة السعر", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<ProductPriceDto>> UpdateAsync(int id, UpdateProductPriceRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(id, request, UserId, ct);
        }
        catch (Exception)
        {
            return Result<ProductPriceDto>.Failure("تعذر تحديث السعر", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.DeactivateAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر تعطيل السعر", ErrorCodes.InvalidOperation);
        }
    }

    // ─── مابر ────────────────────────────────────────────────────────────────

    /// <summary>
    /// يحوّل نتيجة حساب السعر الفعلي (EffectivePriceDto) إلى سعر منتج للعرض.
    /// </summary>
    private static ProductPriceDto MapEffectivePrice(EffectivePriceDto p) => new(
        Id: 0,                                  // TODO: لا يوجد معرّف لسجل السعر الفعلي المحسوب.
        ProductUnitId: p.ProductUnitId,
        ProductUnitName: null,                  // TODO: غير متوفر في EffectivePriceDto.
        Price: p.Price,
        EffectiveFrom: DateTime.UtcNow,         // السعر الفعلي ساري من الآن.
        EffectiveTo: null);
}
