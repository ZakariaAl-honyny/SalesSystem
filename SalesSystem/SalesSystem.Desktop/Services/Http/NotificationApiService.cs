using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class NotificationApiService : INotificationApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/notifications";

    public NotificationApiService(HttpClientService http) => _http = http;

    public async Task<Result<List<NotificationDto>>> GetAllAsync(int? userId = null, bool unreadOnly = false, int page = 1, int pageSize = 50)
    {
        var path = $"{BasePath}?page={page}&pageSize={pageSize}&unreadOnly={unreadOnly}";
        if (userId.HasValue)
            path += $"&userId={userId.Value}";
        var result = await _http.GetListAsync<NotificationDto>(path);
        if (result.IsSuccess && result.Value != null)
            return Result<List<NotificationDto>>.Success(result.Value.ToList());
        return Result<List<NotificationDto>>.Failure(result.Error ?? "فشل تحميل الإشعارات");
    }

    public async Task<Result> MarkAsReadAsync(int id)
    {
        return await _http.PutAsync($"{BasePath}/{id}/mark-read");
    }

    public async Task<Result> MarkAllAsReadAsync(int userId)
    {
        return await _http.PutAsync($"{BasePath}/mark-all-read?userId={userId}");
    }
}
