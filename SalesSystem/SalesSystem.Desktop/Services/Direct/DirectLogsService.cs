using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لسجلات النظام — يستدعي خدمات طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectLogsService : ILogsApiService
{
    private readonly BizInterfaces.ILogService _logs;

    public DirectLogsService(BizInterfaces.ILogService logs)
    {
        _logs = logs;
    }

    public async Task<Result<IReadOnlyList<SystemLogDto>>> GetAllAsync(
        byte? level = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        try
        {
            // BLL exposes a paged query — fetch everything and flatten to a list.
            var r = await _logs.QueryLogsAsync(
                level: (int?)level,
                source: null,
                search: null,
                from: from,
                to: to,
                page: 1,
                pageSize: int.MaxValue,
                ct: ct);

            return r.IsSuccess
                ? Result<IReadOnlyList<SystemLogDto>>.Success(r.Value!.Items.ToList())
                : Result<IReadOnlyList<SystemLogDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<SystemLogDto>>.Failure("حدث خطأ أثناء جلب سجلات النظام");
        }
    }
}
