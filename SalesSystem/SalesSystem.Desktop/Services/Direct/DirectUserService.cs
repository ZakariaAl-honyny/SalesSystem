using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة المستخدمين — يستدعي خدمات طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectUserService : IUserApiService
{
    private readonly BizInterfaces.IUserService _users;

    public DirectUserService(BizInterfaces.IUserService users)
    {
        _users = users;
    }

    public async Task<Result<IReadOnlyList<UserDto>>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            return await _users.GetAllAsync(includeInactive: false, ct);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<UserDto>>.Failure("حدث خطأ أثناء جلب قائمة المستخدمين");
        }
    }

    public async Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _users.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<UserDto>.Failure("حدث خطأ أثناء جلب بيانات المستخدم");
        }
    }

    public async Task<Result<UserDto>> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _users.CreateAsync(request, ct);
        }
        catch (Exception)
        {
            return Result<UserDto>.Failure("حدث خطأ أثناء إنشاء المستخدم");
        }
    }

    public async Task<Result<UserDto>> UpdateAsync(int id, UpdateUserRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _users.UpdateAsync(id, request, ct);
        }
        catch (Exception)
        {
            return Result<UserDto>.Failure("حدث خطأ أثناء تحديث بيانات المستخدم");
        }
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _users.DeleteAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("حدث خطأ أثناء حذف المستخدم");
        }
    }

    public async Task<Result<ResetPasswordResponse>> ResetPasswordAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _users.ResetPasswordAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<ResetPasswordResponse>.Failure("حدث خطأ أثناء إعادة تعيين كلمة المرور");
        }
    }
}
