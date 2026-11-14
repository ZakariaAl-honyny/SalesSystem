using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Models;
using SalesSystem.Common.Exceptions;
namespace SalesSystem.Business.Services;

public class UserService : IUserService
{
    /// <summary>
    /// Default password assigned to new users and used for admin-initiated resets.
    /// </summary>
    private const string DefaultPassword = "12345678";

    private readonly IUnitOfWork _uow;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUnitOfWork uow,
        IAuditLogService auditLogService,
        ILogger<UserService> logger)
    {
        _uow = uow;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    public async Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var user = await _uow.Users.FirstOrDefaultIgnoreFiltersAsync(u => u.Id == id, ct);
        if (user == null)
            return Result<UserDto>.Failure("المستخدم غير موجود", ErrorCodes.NotFound);

        return Result<UserDto>.Success(MapToDto(user));
    }

    public async Task<Result<UserDto>> GetByUserNameAsync(string userName, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return Result<UserDto>.Failure("اسم المستخدم مطلوب", ErrorCodes.ValidationError);

        var user = await _uow.Users.FirstOrDefaultIgnoreFiltersAsync(
            u => u.UserName == userName, ct);
        if (user == null)
            return Result<UserDto>.Failure("المستخدم غير موجود", ErrorCodes.NotFound);

        return Result<UserDto>.Success(MapToDto(user));
    }

    public async Task<Result<IReadOnlyList<UserDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        List<User> users;
        if (includeInactive)
        {
            users = await _uow.Users.ToListIgnoreFiltersAsync(ct);
        }
        else
        {
            users = await _uow.Users.ToListAsync(ct);
        }

        var dtos = users.Select(MapToDto).OrderByDescending(x => x.Id).ToList();
        return Result<IReadOnlyList<UserDto>>.Success(dtos);
    }

    public async Task<Result<UserDto>> CreateAsync(CreateUserRequest request, CancellationToken ct)
    {
        try
        {
            // 1. Check duplicate username
            var duplicateExists = await _uow.Users.AnyIgnoreFiltersAsync(
                u => u.UserName.ToLower() == request.UserName.ToLower().Trim(), ct);
            if (duplicateExists)
            {
                return Result<UserDto>.Failure("اسم المستخدم موجود بالفعل", ErrorCodes.DuplicateEntry);
            }

            // 2. Hash the provided password or fall back to the default password.
            string passwordToHash = string.IsNullOrWhiteSpace(request.Password) ? DefaultPassword : request.Password;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordToHash, workFactor: 12);

            // 3. Create user with BranchId directly
            var user = User.CreateWithPassword(
                request.UserName,
                passwordHash,
                branchId: request.BranchId,
                employeeId: null,
                createdByUserId: null,
                mustChangePassword: true
            );

            await _uow.Users.AddAsync(user, ct);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Created new user: {UserName} (Id={UserId})",
                user.UserName, user.Id);

            return Result<UserDto>.Success(MapToDto(user));
        }
        catch (DomainException ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating user");
            return Result<UserDto>.Failure("حدث خطأ أثناء إضافة المستخدم.");
        }
    }

    public async Task<Result<UserDto>> UpdateAsync(int id, UpdateUserRequest request, CancellationToken ct)
    {
        try
        {
            var user = await _uow.Users.FirstOrDefaultIgnoreFiltersAsync(u => u.Id == id, ct);
            if (user == null)
                return Result<UserDto>.Failure("المستخدم غير موجود", ErrorCodes.NotFound);

            // Update branch directly
            user.Update(
                branchId: request.BranchId,
                employeeId: null,
                updatedByUserId: null
            );

            // Update lock state
            if (request.IsLocked.HasValue)
            {
                if (request.IsLocked.Value) user.Lock();
                else user.Unlock();
            }

            // Update active/inactive state
            if (request.IsActive.HasValue)
            {
                if (request.IsActive.Value) user.Activate();
                else user.Deactivate();
            }

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);
                user.ChangePassword(passwordHash);
            }

            await _uow.SaveChangesAsync(ct);
            _logger.LogInformation("Updated user: {UserName} (Id={UserId})",
                user.UserName, user.Id);

            return Result<UserDto>.Success(MapToDto(user));
        }
        catch (DomainException ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user {Id}", id);
            return Result<UserDto>.Failure("حدث خطأ أثناء تحديث بيانات المستخدم.");
        }
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct)
    {
        try
        {
            var user = await _uow.Users.FirstOrDefaultIgnoreFiltersAsync(u => u.Id == id, ct);
            if (user == null)
                return Result.Failure("المستخدم غير موجود", ErrorCodes.NotFound);

            await _uow.Users.SoftDeleteAsync(id, ct);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Deactivated user: {UserName} (Id={UserId})", user.UserName, user.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating user {Id}", id);
            return Result.Failure("حدث خطأ أثناء تعطيل المستخدم.");
        }
    }

    public async Task<Result> PermanentDeleteAsync(int id, CancellationToken ct)
    {
        _logger.LogWarning("Attempt to hard-delete user {UserId} blocked — soft delete only", id);
        return Result.Failure("لا يمكن حذف المستخدمين بشكل نهائي — استخدم خاصية تعطيل الحساب بدلاً من ذلك",
            ErrorCodes.InvalidOperation);
    }

    public async Task<Result> ReactivateAsync(int id, CancellationToken ct)
    {
        try
        {
            var user = await _uow.Users.FirstOrDefaultIgnoreFiltersAsync(u => u.Id == id, ct);
            if (user == null)
                return Result.Failure("المستخدم غير موجود", ErrorCodes.NotFound);

            user.Restore();
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("User reactivated: {UserName} (ID: {Id})", user.UserName, id);
            return Result.Success();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain rule violation reactivating user {Id}: {Message}", id, ex.Message);
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reactivating user {Id}", id);
            return Result.Failure("حدث خطأ أثناء إعادة تنشيط المستخدم");
        }
    }

    public async Task<Result<ResetPasswordResponse>> ResetPasswordAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var user = await _uow.Users.FirstOrDefaultIgnoreFiltersAsync(u => u.Id == id, ct);
            if (user == null)
                return Result<ResetPasswordResponse>.Failure("المستخدم غير موجود", ErrorCodes.NotFound);

            string defaultPasswordHash = BCrypt.Net.BCrypt.HashPassword(DefaultPassword, workFactor: 12);
            user.ResetPassword(defaultPasswordHash);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Password reset for user {UserName} (Id={UserId}) to default", user.UserName, user.Id);
            await _auditLogService.LogAsync(null, "AdminForcePasswordChange", "User", user.Id,
                $"تم إعادة تعيين كلمة المرور للمستخدم {user.UserName} إلى الافتراضية من قبل المسؤول", null, ct);

            return Result<ResetPasswordResponse>.Success(new ResetPasswordResponse(
                user.Id, "تم إعادة تعيين كلمة المرور إلى 12345678"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for user {Id}", id);
            return Result<ResetPasswordResponse>.Failure("حدث خطأ أثناء إعادة تعيين كلمة المرور.");
        }
    }

    public async Task<Result<CurrentUserDto>> GetCurrentUserAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var user = await _uow.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
            if (user == null)
                return Result<CurrentUserDto>.Failure("المستخدم غير موجود", ErrorCodes.NotFound);

            // Single-admin system: full access across all modules.
            return Result<CurrentUserDto>.Success(new CurrentUserDto(
                Id: user.Id,
                UserName: user.UserName,
                BranchId: user.BranchId
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user data for {Id}", id);
            return Result<CurrentUserDto>.Failure("حدث خطأ أثناء تحميل بيانات المستخدم.");
        }
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto(
            Id: user.Id,
            UserName: user.UserName,
            BranchId: user.BranchId,
            MustChangePassword: user.MustChangePassword,
            IsLocked: user.IsLocked,
            IsActive: user.IsActive,
            LastLoginAt: user.LastLoginAt,
            LoginAttempts: user.LoginAttempts,
            DefaultCashBoxId: null
        );
    }
}
