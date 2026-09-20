using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IUserSessionApiService
{
    Task<Result<IReadOnlyList<UserSessionDto>>> GetAllAsync(int? userId = null, bool includeRevoked = false, CancellationToken ct = default);
    Task<Result> RevokeAsync(long sessionId, CancellationToken ct = default);
}
