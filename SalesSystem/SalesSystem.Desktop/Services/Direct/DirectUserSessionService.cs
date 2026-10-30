using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لجلسات المستخدمين — يستدعي خدمات طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectUserSessionService : IUserSessionApiService
{
    private readonly BizInterfaces.ISessionManagementService _sessions;

    public DirectUserSessionService(BizInterfaces.ISessionManagementService sessions)
    {
        _sessions = sessions;
    }

    public async Task<Result<IReadOnlyList<UserSessionDto>>> GetAllAsync(
        int? userId = null, bool includeRevoked = false, CancellationToken ct = default)
    {
        try
        {
            var r = await _sessions.GetAllAsync(userId, includeRevoked, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<UserSessionDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<UserSessionDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<UserSessionDto>>.Failure("حدث خطأ أثناء جلب جلسات المستخدمين");
        }
    }

    public async Task<Result> RevokeAsync(long sessionId, CancellationToken ct = default)
    {
        try
        {
            return await _sessions.RevokeAsync(sessionId, ct);
        }
        catch (Exception)
        {
            return Result.Failure("حدث خطأ أثناء إنهاء الجلسة");
        }
    }
}
