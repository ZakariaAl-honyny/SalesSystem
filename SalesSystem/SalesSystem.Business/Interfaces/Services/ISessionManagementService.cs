using SalesSystem.DTO.Common;
using SalesSystem.DTO;

namespace SalesSystem.Business.Interfaces.Services;

public interface ISessionManagementService
{
    Task<Result<List<UserSessionDto>>> GetAllAsync(int? userId, bool includeRevoked, CancellationToken ct);
    Task<Result> RevokeAsync(long sessionId, CancellationToken ct);
}
