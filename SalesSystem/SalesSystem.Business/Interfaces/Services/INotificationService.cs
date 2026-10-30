using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Business.Interfaces.Services;

public interface INotificationService
{
    Task<Result<List<NotificationDto>>> GetUserNotificationsAsync(int userId, CancellationToken ct);
    Task<Result<NotificationDto>> MarkAsReadAsync(int id, CancellationToken ct);
    Task<Result> MarkAllAsReadAsync(int userId, CancellationToken ct = default);
    Task<Result<int>> GetUnreadCountAsync(int userId, CancellationToken ct);
}
