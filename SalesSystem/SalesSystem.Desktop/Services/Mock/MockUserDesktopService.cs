using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة المستخدمين — تعيد بيانات من MockSecurityData.
/// </summary>
public class MockUserDesktopService : IUserDesktopService
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
        try
        {
            // 1) اسم المستخدم مطلوب — الرفض الأول قبل أي فحص آخر (RULE-N167).
            if (request is null || string.IsNullOrWhiteSpace(request.UserName))
                return Task.FromResult(Result<UserDto>.Failure("اسم المستخدم مطلوب"));

            var userName = request.UserName.Trim();

            // 2) التحقق من عدم تكرار اسم المستخدم
            if (MockSecurityData.Users.Any(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)))
                return Task.FromResult(Result<UserDto>.Failure("اسم المستخدم موجود مسبقاً"));

            var nextId = MockSecurityData.Users.Max(u => u.Id) + 1;

            var user = new UserDto(
                Id: nextId,
                UserName: userName,
                BranchId: request.BranchId,
                MustChangePassword: true,
                IsLocked: false,
                IsActive: true,
                LastLoginAt: null,
                LoginAttempts: 0,
                DefaultCashBoxId: request.DefaultCashBoxId);

            MockSecurityData.Users.Add(user);

            // 3) كلمة المرور المكتوبة في الشاشة هي كلمة المرور الفعلية للدخول (والمخزن يعيد
            //    الافتراضية «123456» إن تُركت فارغة) — فلا يسقط كل حساب جديد على الافتراضية.
            MockSecurityData.SetPassword(user.Id, request.Password);

            return Task.FromResult(Result<UserDto>.Success(user));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "MockUserDesktopService.CreateAsync failed for user {UserName}", request?.UserName);
            return Task.FromResult(Result<UserDto>.Failure("حدث خطأ غير متوقع أثناء إضافة المستخدم."));
        }
    }

    public Task<Result<UserDto>> UpdateAsync(int id, UpdateUserRequest request, CancellationToken ct = default)
    {
        try
        {
            var existing = MockSecurityData.Users.FirstOrDefault(u => u.Id == id);
            if (existing is null)
                return Task.FromResult(Result<UserDto>.Failure("المستخدم غير موجود"));

            var updated = existing with
            {
                BranchId = request.BranchId,
                IsLocked = request.IsLocked ?? existing.IsLocked,
                IsActive = request.IsActive ?? existing.IsActive
            };

            MockSecurityData.ReplaceUser(existing, updated);

            // كلمة مرور جديدة واردة في الطلب (اختيارية) تُخزَّن هي بعينها — والفراغ يترك السابقة.
            if (!string.IsNullOrWhiteSpace(request.Password))
                MockSecurityData.SetPassword(id, request.Password);

            return Task.FromResult(Result<UserDto>.Success(updated));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "MockUserDesktopService.UpdateAsync failed for userId {UserId}", id);
            return Task.FromResult(Result<UserDto>.Failure("حدث خطأ غير متوقع أثناء تعديل المستخدم."));
        }
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockSecurityData.Users.FirstOrDefault(u => u.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("المستخدم غير موجود"));

            // لا يمكن حذف مدير النظام (المستخدم الجذري)
            if (existing.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(Result.Failure("لا يمكن حذف حساب مدير النظام."));

            MockSecurityData.ReplaceUser(existing, existing with { IsActive = false });

            // الحساب المعطّل لا يحتفظ بكلمة مرور: يُزال سطره من المخزن التجريبي.
            MockSecurityData.RemovePassword(existing.Id);

            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "MockUserDesktopService.DeleteAsync failed for userId {UserId}", id);
            return Task.FromResult(Result.Failure("حدث خطأ غير متوقع أثناء تعطيل المستخدم."));
        }
    }

    public Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockSecurityData.Users.FirstOrDefault(u => u.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("المستخدم غير موجود"));

            MockSecurityData.ReplaceUser(existing, existing with { IsActive = true });

            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "MockUserDesktopService.ReactivateAsync failed for userId {UserId}", id);
            return Task.FromResult(Result.Failure("حدث خطأ غير متوقع أثناء إعادة تنشيط المستخدم."));
        }
    }

    public Task<Result<ResetPasswordResponse>> ResetPasswordAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockSecurityData.Users.FirstOrDefault(u => u.Id == id);
            if (existing is null)
                return Task.FromResult(Result<ResetPasswordResponse>.Failure("المستخدم غير موجود"));

            // إعادة التعيين تكتب الكلمة الافتراضية في المخزن نفسه (لا رسالة بلا أثر)،
            // وتعيد الكلمة المخزَّنة فعلاً كي تعرض الرسالة القيمة التي يُدخل بها المالك.
            var password = MockSecurityData.SetPassword(existing.Id, null);

            MockSecurityData.ReplaceUser(existing,
                existing with { MustChangePassword = true, IsLocked = false, LoginAttempts = 0 });

            var response = new ResetPasswordResponse(
                UserId: id,
                Message: $"تم إعادة تعيين كلمة المرور إلى الافتراضية ({password}). يجب على المستخدم تغييرها عند تسجيل الدخول التالي.");

            return Task.FromResult(Result<ResetPasswordResponse>.Success(response));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "MockUserDesktopService.ResetPasswordAsync failed for userId {UserId}", id);
            return Task.FromResult(Result<ResetPasswordResponse>.Failure("حدث خطأ غير متوقع أثناء إعادة تعيين كلمة المرور."));
        }
    }
}
