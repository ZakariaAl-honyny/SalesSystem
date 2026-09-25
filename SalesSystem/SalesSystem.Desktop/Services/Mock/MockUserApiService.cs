using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة المستخدمين — تعيد بيانات من MockSecurityData.
/// </summary>
public class MockUserApiService : IUserApiService
{
    public Task<Result<IReadOnlyList<UserDto>>> GetAllAsync(CancellationToken ct = default)
    {
        return Task.FromResult(Result<IReadOnlyList<UserDto>>.Success(
            MockSecurityData.Users.ToList().AsReadOnly()));
    }

    public Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var user = MockSecurityData.Users.FirstOrDefault(u => u.Id == id);
        return user is null
            ? Task.FromResult(Result<UserDto>.Failure("المستخدم غير موجود"))
            : Task.FromResult(Result<UserDto>.Success(user));
    }

    public Task<Result<UserDto>> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        // التحقق من عدم تكرار اسم المستخدم
        if (MockSecurityData.Users.Any(u => u.UserName.Equals(request.UserName, StringComparison.OrdinalIgnoreCase)))
            return Task.FromResult(Result<UserDto>.Failure("اسم المستخدم موجود مسبقاً"));

        var nextId = MockSecurityData.Users.Max(u => u.Id) + 1;
        var roleName = MockSecurityData.Roles.FirstOrDefault(r => r.Id == request.RoleId)?.Name;

        var user = new UserDto(
            Id: nextId,
            UserName: request.UserName,
            RoleId: request.RoleId,
            RoleName: roleName,
            BranchId: request.BranchId,
            MustChangePassword: true,
            IsLocked: false,
            IsActive: true,
            LastLoginAt: null,
            LoginAttempts: 0,
            DefaultCashBoxId: request.DefaultCashBoxId);

        MockSecurityData.Users.Add(user);
        return Task.FromResult(Result<UserDto>.Success(user));
    }

    public Task<Result<UserDto>> UpdateAsync(int id, UpdateUserRequest request, CancellationToken ct = default)
    {
        var existing = MockSecurityData.Users.FirstOrDefault(u => u.Id == id);
        if (existing is null)
            return Task.FromResult(Result<UserDto>.Failure("المستخدم غير موجود"));

        var roleName = MockSecurityData.Roles.FirstOrDefault(r => r.Id == request.RoleId)?.Name;

        var updated = existing with
        {
            RoleId = request.RoleId,
            RoleName = roleName ?? existing.RoleName,
            BranchId = request.BranchId,
            IsLocked = request.IsLocked ?? existing.IsLocked,
            IsActive = request.IsActive ?? existing.IsActive
        };

        var index = MockSecurityData.Users.IndexOf(existing);
        MockSecurityData.Users[index] = updated;
        return Task.FromResult(Result<UserDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockSecurityData.Users.FirstOrDefault(u => u.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("المستخدم غير موجود"));

        // لا يمكن حذف مدير النظام
        if (existing.RoleId == 1)
            return Task.FromResult(Result.Failure("لا يمكن حذف حساب مدير النظام."));

        var index = MockSecurityData.Users.IndexOf(existing);
        MockSecurityData.Users[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }

    public Task<Result<ResetPasswordResponse>> ResetPasswordAsync(int id, CancellationToken ct = default)
    {
        var existing = MockSecurityData.Users.FirstOrDefault(u => u.Id == id);
        if (existing is null)
            return Task.FromResult(Result<ResetPasswordResponse>.Failure("المستخدم غير موجود"));

        var index = MockSecurityData.Users.IndexOf(existing);
        MockSecurityData.Users[index] = existing with { MustChangePassword = true, IsLocked = false, LoginAttempts = 0 };

        var response = new ResetPasswordResponse(
            UserId: id,
            Message: "تم إعادة تعيين كلمة المرور إلى الافتراضية (12345678). يجب على المستخدم تغييرها عند تسجيل الدخول التالي.");

        return Task.FromResult(Result<ResetPasswordResponse>.Success(response));
    }
}
