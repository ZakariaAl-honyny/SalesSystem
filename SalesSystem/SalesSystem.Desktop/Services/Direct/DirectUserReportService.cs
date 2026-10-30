using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لتقارير المستخدمين — يستدعي خدمات طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectUserReportService : IUserReportApiService
{
    private readonly BizInterfaces.IUserReportService _reports;

    public DirectUserReportService(BizInterfaces.IUserReportService reports)
    {
        _reports = reports;
    }

    public async Task<Result<IReadOnlyList<LoginHistoryDto>>> GetLoginHistoryAsync(
        int? userId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetLoginHistoryAsync(userId, from, to, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<LoginHistoryDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<LoginHistoryDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<LoginHistoryDto>>.Failure("حدث خطأ أثناء جلب سجل الدخول");
        }
    }

    public async Task<Result<IReadOnlyList<UserActivityReportDto>>> GetUserActivityAsync(
        int? userId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetUserActivityAsync(userId, from, to, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<UserActivityReportDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<UserActivityReportDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<UserActivityReportDto>>.Failure("حدث خطأ أثناء جلب نشاط المستخدمين");
        }
    }
}
