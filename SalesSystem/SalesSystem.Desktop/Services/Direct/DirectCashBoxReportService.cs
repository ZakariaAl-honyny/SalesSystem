using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لتقارير الصناديق — يستدعي خدمات طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectCashBoxReportService : ICashBoxReportApiService
{
    private readonly BizInterfaces.ICashBoxReportService _reports;

    public DirectCashBoxReportService(BizInterfaces.ICashBoxReportService reports)
    {
        _reports = reports;
    }

    public async Task<Result<IReadOnlyList<CashBoxSummaryDto>>> GetCashBoxSummaryAsync(
        DateTime? asOfDate = null, CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetCashBoxSummaryAsync(asOfDate, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<CashBoxSummaryDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<CashBoxSummaryDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<CashBoxSummaryDto>>.Failure("حدث خطأ أثناء جلب ملخص الصناديق");
        }
    }
}
