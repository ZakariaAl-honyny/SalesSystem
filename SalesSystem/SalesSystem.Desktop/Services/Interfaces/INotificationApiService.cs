using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface INotificationApiService
{
    Task<Result<List<NotificationDto>>> GetAllAsync(int? userId = null, bool unreadOnly = false, int page = 1, int pageSize = 50);
    Task<Result> MarkAsReadAsync(int id);
    Task<Result> MarkAllAsReadAsync(int userId);
}
