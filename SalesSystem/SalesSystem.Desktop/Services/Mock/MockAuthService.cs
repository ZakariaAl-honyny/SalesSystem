using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة المصادقة — تقبل أي بيانات دخول وتعيد نجاحاً دائماً.
/// </summary>
public class MockAuthService : IAuthService
{
    public Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var response = new LoginResponse(
            UserId: 1,
            UserName: request.UserName,
            RoleId: 1,
            RoleName: "مدير النظام",
            BranchId: null,
            Token: "mock-token-0000000000000000",
            ExpiresAt: DateTime.UtcNow.AddHours(8),
            MustChangePassword: false,
            IsPasswordless: false,
            SalesPermissions: -1,
            InventoryPermissions: -1,
            AccountingPermissions: -1,
            SystemPermissions: -1);

        return Task.FromResult(Result<LoginResponse>.Success(response));
    }

    public Task<Result> ChangePasswordAsync(ChangePasswordRequest request, int userId, CancellationToken ct = default)
    {
        return Task.FromResult(Result.Success());
    }
}
