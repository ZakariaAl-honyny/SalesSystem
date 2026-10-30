using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة الجرد الفعلي — ينادي طبقة الأعمال داخل نفس العملية (بدون HTTP).
/// </summary>
public class DirectInventoryCountService : IInventoryCountApiService
{
    private readonly BizInterfaces.IInventoryCountService _bll;
    private readonly ISessionService _session;

    public DirectInventoryCountService(BizInterfaces.IInventoryCountService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<InventoryCountDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // ملاحظة: خدمة الأعمال لا تدعم تصفية غير النشط — المعامل includeInactive مُهمَل.
            var result = await _bll.GetAllAsync(ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<InventoryCountDto>>.Success(result.Value!)
                : Result<IReadOnlyList<InventoryCountDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<InventoryCountDto>>.Failure("تعذر تحميل أوامر الجرد. حاول مرة أخرى.");
        }
    }

    public async Task<Result<InventoryCountDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<InventoryCountDto>.Failure("تعذر تحميل أمر الجرد. حاول مرة أخرى.");
        }
    }

    public async Task<Result<InventoryCountDto>> CreateAsync(CreateInventoryCountRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, UserId, ct);
        }
        catch (Exception)
        {
            return Result<InventoryCountDto>.Failure("تعذر إنشاء أمر الجرد. حاول مرة أخرى.");
        }
    }

    public async Task<Result<InventoryCountDto>> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var posted = await _bll.PostAsync(id, UserId, ct);
            if (!posted.IsSuccess)
                return Result<InventoryCountDto>.Failure(posted.Error!, posted.ErrorCode);

            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<InventoryCountDto>.Failure("تعذر ترحيل أمر الجرد. حاول مرة أخرى.");
        }
    }

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CancelAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر إلغاء أمر الجرد. حاول مرة أخرى.");
        }
    }

    public async Task<Result<InventoryCountDto>> AddLineAsync(int id, AddInventoryCountLineRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.AddLineAsync(id, request, ct);
        }
        catch (Exception)
        {
            return Result<InventoryCountDto>.Failure("تعذر إضافة الصنف إلى أمر الجرد. حاول مرة أخرى.");
        }
    }
}
