using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لبيانات لوحة المعلومات — يستدعي خدمات طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectDashboardService : IDashboardApiService
{
    private readonly BizInterfaces.IReportService _reports;

    public DirectDashboardService(BizInterfaces.IReportService reports)
    {
        _reports = reports;
    }

    public async Task<Result<DashboardSummaryDto>> GetSummaryAsync(CancellationToken ct = default)
    {
        try
        {
            return await _reports.GetDashboardSummaryAsync(ct);
        }
        catch (Exception)
        {
            return Result<DashboardSummaryDto>.Failure("حدث خطأ أثناء جلب ملخص لوحة المعلومات");
        }
    }
}
