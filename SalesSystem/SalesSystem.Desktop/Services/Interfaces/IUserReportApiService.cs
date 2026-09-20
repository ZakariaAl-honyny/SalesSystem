using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IUserReportApiService
{
    Task<Result<IReadOnlyList<LoginHistoryDto>>> GetLoginHistoryAsync(int? userId, DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<IReadOnlyList<UserActivityReportDto>>> GetUserActivityAsync(int? userId, DateTime from, DateTime to, CancellationToken ct = default);
}
