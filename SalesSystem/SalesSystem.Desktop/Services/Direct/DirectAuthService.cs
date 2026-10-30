using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL authentication service (in-process, no HTTP).
/// </summary>
public class DirectAuthService : IAuthApiService
{
    private readonly BizInterfaces.IAuthService _bll;
    private readonly ISessionService _session;

    public DirectAuthService(BizInterfaces.IAuthService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.LoginAsync(request, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectAuthService.LoginAsync failed");
            return Result<LoginResponse>.Failure("حدث خطأ غير متوقع أثناء تسجيل الدخول.");
        }
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.ChangePasswordAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectAuthService.ChangePasswordAsync failed");
            return Result.Failure("حدث خطأ غير متوقع أثناء تغيير كلمة المرور.");
        }
    }
}
