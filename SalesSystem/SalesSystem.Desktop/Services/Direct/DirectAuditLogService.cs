using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لسجل التدقيق — يستدعي خدمات طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectAuditLogService : IAuditLogApiService
{
    private const int DefaultLoginHistoryLimit = 50;

    private readonly BizInterfaces.IAuditLogService _audit;

    public DirectAuditLogService(BizInterfaces.IAuditLogService audit)
    {
        _audit = audit;
    }

    public async Task<Result<PaginatedResult<AuditLogDto>>> QueryAsync(
        AuditLogQuery query, CancellationToken ct = default)
    {
        try
        {
            return await _audit.QueryAsync(query, ct);
        }
        catch (Exception)
        {
            return Result<PaginatedResult<AuditLogDto>>.Failure("حدث خطأ أثناء جلب سجل التدقيق");
        }
    }

    public async Task<Result<IReadOnlyList<AuditLogDto>>> GetLoginHistoryAsync(
        int? userId = null, CancellationToken ct = default)
    {
        try
        {
            return await _audit.GetLoginHistoryAsync(userId, DefaultLoginHistoryLimit, ct);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<AuditLogDto>>.Failure("حدث خطأ أثناء جلب سجل الدخول");
        }
    }
}
