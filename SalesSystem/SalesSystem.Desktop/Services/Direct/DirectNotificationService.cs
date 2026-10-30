using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL notification service (in-process, no HTTP).
/// </summary>
public class DirectNotificationService : INotificationApiService
{
    private readonly BizInterfaces.INotificationService _bll;
    private readonly ISessionService _session;

    public DirectNotificationService(BizInterfaces.INotificationService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<List<NotificationDto>>> GetAllAsync(
        int? userId = null, bool unreadOnly = false, int page = 1, int pageSize = 50)
    {
        try
        {
            var result = await _bll.GetUserNotificationsAsync(userId ?? UserId, CancellationToken.None);
            if (!result.IsSuccess)
                return result;

            IEnumerable<NotificationDto> items = result.Value ?? new List<NotificationDto>();
            if (unreadOnly)
                items = items.Where(n => !n.IsRead);

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            var paged = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Result<List<NotificationDto>>.Success(paged);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectNotificationService.GetAllAsync failed");
            return Result<List<NotificationDto>>.Failure("حدث خطأ غير متوقع أثناء تحميل التنبيهات.");
        }
    }

    public async Task<Result> MarkAsReadAsync(int id)
    {
        try
        {
            var result = await _bll.MarkAsReadAsync(id, CancellationToken.None);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error ?? "تعذر تحديد التنبيه كمقروء.", result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectNotificationService.MarkAsReadAsync failed for id {Id}", id);
            return Result.Failure("حدث خطأ غير متوقع أثناء تحديد التنبيه كمقروء.");
        }
    }

    public async Task<Result> MarkAllAsReadAsync(int userId)
    {
        try
        {
            return await _bll.MarkAllAsReadAsync(userId, CancellationToken.None);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectNotificationService.MarkAllAsReadAsync failed for user {UserId}", userId);
            return Result.Failure("حدث خطأ غير متوقع أثناء تحديد جميع التنبيهات كمقروءة.");
        }
    }
}
