using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// واجهة خدمة API العهد النثرية (Petty Cash) — استدعاءات HTTP لشاشة العهد النثرية.
/// </summary>
public interface IPettyCashApiService
{
    /// <summary>جلب جميع طلبات العهد النثرية مع إمكانية التصفية.</summary>
    Task<Result<IReadOnlyList<PettyCashRequestDto>>> GetAllAsync(
        bool includeInactive = false,
        CancellationToken ct = default);

    /// <summary>جلب طلب عهدة نثرية واحد حسب المعرّف.</summary>
    Task<Result<PettyCashRequestDto>> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>إنشاء طلب عهدة نثرية جديد.</summary>
    Task<Result<PettyCashRequestDto>> CreateAsync(PettyCashRequestDto request, CancellationToken ct = default);

    /// <summary>تعديل طلب عهدة نثرية موجود.</summary>
    Task<Result<PettyCashRequestDto>> UpdateAsync(int id, PettyCashRequestDto request, CancellationToken ct = default);

    /// <summary>حذف (إلغاء) طلب عهدة نثرية.</summary>
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
