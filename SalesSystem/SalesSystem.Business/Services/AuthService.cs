using System.Security.Cryptography;
using System.Text;
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

/// <summary>
/// Implementation of the authentication service.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly JwtSettings _jwtSettings;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUnitOfWork uow,
        IJwtTokenGenerator jwtTokenGenerator,
        JwtSettings jwtSettings,
        IAuditLogService auditLogService,
        ILogger<AuthService> logger)
    {
        _uow = uow;
        _jwtTokenGenerator = jwtTokenGenerator;
        _jwtSettings = jwtSettings;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Login attempt for user: {UserName}", request.UserName);

            // 1. Find user — load with Role navigation for permissions
            var user = await _uow.Users.FirstOrDefaultAsync(
                u => u.UserName.ToLower() == request.UserName.ToLower().Trim(), ct, "Role");

            if (user == null)
            {
                _logger.LogWarning("Login failed: User {UserName} not found", request.UserName);
                return Result<LoginResponse>.Failure("بيانات الاعتماد غير صالحة", ErrorCodes.Unauthorized);
            }

            // 2. Check if account is locked
            if (user.IsLocked)
            {
                _logger.LogWarning("Login failed: User {UserName} account is locked", request.UserName);
                await _auditLogService.LogAsync(user.Id, "LoginFailed_Locked", "User", user.Id,
                    "محاولة دخول لحساب مقفول", null, ct, autoSave: false);
                await _uow.SaveChangesAsync(ct);
                return Result<LoginResponse>.Failure("الحساب مقفول — يرجى الاتصال بالمسؤول", ErrorCodes.AccountLocked);
            }

            // 3. Check if account is active (not soft-deleted)
            if (!user.IsActive)
            {
                _logger.LogWarning("Login failed: User {UserName} is inactive", request.UserName);
                return Result<LoginResponse>.Failure("الحساب معطل", ErrorCodes.Forbidden);
            }

            // 4. Verify password (skip if user has no password yet — passwordless user)
            bool isPasswordValid = !string.IsNullOrWhiteSpace(user.PasswordHash)
                ? BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)
                : true;

            // 5. Record login attempt (always, regardless of success/failure)
            user.RecordLoginAttempt(isPasswordValid);
            await _uow.SaveChangesAsync(ct);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Login failed: Incorrect password for user {UserName} (attempt {LoginAttempts})",
                    request.UserName, user.LoginAttempts);
                await _auditLogService.LogAsync(user.Id, "LoginFailed", "User", user.Id,
                    $"محاولة دخول فاشلة — المحاولة {user.LoginAttempts}", null, ct, autoSave: false);
                await _uow.SaveChangesAsync(ct);
                return Result<LoginResponse>.Failure("بيانات الاعتماد غير صالحة", ErrorCodes.Unauthorized);
            }

            // 7. Check if password change is required
            if (user.MustChangePassword)
            {
                _logger.LogInformation("Login requires password change for user: {UserName}", request.UserName);
                await _auditLogService.LogAsync(user.Id, "LoginRequiresPasswordChange", "User", user.Id,
                    "تسجيل الدخول يتطلب تغيير كلمة المرور", null, ct, autoSave: false);

                // Return a token anyway so the client can navigate to password change screen
                string token = _jwtTokenGenerator.GenerateToken(user);
                var expiresAt = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours);

                // Create session
                var session = UserSession.Create(user.Id, HashToken(token), _jwtSettings.ExpirationHours);
                await _uow.UserSessions.AddAsync(session, ct);

                await _uow.SaveChangesAsync(ct);

                return Result<LoginResponse>.Success(new LoginResponse(
                    user.Id, user.UserName, user.RoleId, user.Role?.Name,
                    user.BranchId, token, expiresAt, MustChangePassword: true,
                    IsPasswordless: string.IsNullOrWhiteSpace(user.PasswordHash),
                    SalesPermissions: user.Role?.SalesPermissions ?? 0,
                    InventoryPermissions: user.Role?.InventoryPermissions ?? 0,
                    AccountingPermissions: user.Role?.AccountingPermissions ?? 0,
                    SystemPermissions: user.Role?.SystemPermissions ?? 0));
            }

            // 8. Generate JWT
            string jwtToken = _jwtTokenGenerator.GenerateToken(user);
            var jwtExpiresAt = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours);

            // Create session
            var userSession = UserSession.Create(user.Id, HashToken(jwtToken), _jwtSettings.ExpirationHours);
            await _uow.UserSessions.AddAsync(userSession, ct);

            _logger.LogInformation("Login successful for user: {UserName} (Id={UserId})", user.UserName, user.Id);
            await _auditLogService.LogAsync(user.Id, "LoginSuccess", "User", user.Id,
                "تسجيل دخول ناجح", null, ct, autoSave: false);

            // 9. Persist all changes (user session + audit log) atomically
            await _uow.SaveChangesAsync(ct);

            // 10. Return success
            return Result<LoginResponse>.Success(new LoginResponse(
                user.Id, user.UserName, user.RoleId, user.Role?.Name,
                user.BranchId, jwtToken, jwtExpiresAt, MustChangePassword: false,
                SalesPermissions: user.Role?.SalesPermissions ?? 0,
                InventoryPermissions: user.Role?.InventoryPermissions ?? 0,
                AccountingPermissions: user.Role?.AccountingPermissions ?? 0,
                SystemPermissions: user.Role?.SystemPermissions ?? 0));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for user: {UserName}", request.UserName);
            return Result<LoginResponse>.Failure("حدث خطأ أثناء تسجيل الدخول.");
        }
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request, int userId, CancellationToken ct = default)
    {
        try
        {
            var user = await _uow.Users.GetByIdAsync(userId, ct);
            if (user == null)
            {
                _logger.LogWarning("ChangePassword failed: User {UserId} not found", userId);
                return Result.Failure("المستخدم غير موجود", ErrorCodes.NotFound);
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return Result.Failure("كلمة المرور الجديدة وتأكيدها غير متطابقين", ErrorCodes.ValidationError);
            }

            // Verify current password (skip if user has no password yet — passwordless user)
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                {
                    _logger.LogWarning("ChangePassword failed: Incorrect current password for user {UserId}", userId);
                    await _auditLogService.LogAsync(userId, "PasswordChangeFailed", "User", userId,
                        "محاولة تغيير كلمة المرور — كلمة المرور الحالية غير صحيحة", null, ct, autoSave: false);
                    await _uow.SaveChangesAsync(ct);
                    return Result.Failure("كلمة المرور الحالية غير صحيحة", ErrorCodes.Unauthorized);
                }
            }
            else
            {
                _logger.LogInformation("ChangePassword: Skipping current password verification for passwordless user {UserId}", userId);
            }

            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, workFactor: 12);
            user.ChangePassword(newPasswordHash);

            _logger.LogInformation("Password changed for user {UserId}", userId);
            await _auditLogService.LogAsync(userId, "PasswordChanged", "User", userId,
                "تم تغيير كلمة المرور بنجاح", null, ct, autoSave: false);

            // Persist all changes (user state + audit log) atomically
            await _uow.SaveChangesAsync(ct);

            return Result.Success();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user {UserId}", userId);
            return Result.Failure("حدث خطأ أثناء تغيير كلمة المرور.");
        }
    }

    private static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
