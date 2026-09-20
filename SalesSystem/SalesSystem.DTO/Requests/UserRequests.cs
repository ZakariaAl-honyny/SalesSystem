namespace SalesSystem.DTO.Requests;

// ─── User CRUD ────────────────────────────────────────

public record CreateUserRequest(
    string UserName,
    short RoleId,
    short? BranchId = null,
    string? Password = null,
    int? DefaultCashBoxId = null);

public record UpdateUserRequest(
    short RoleId,
    short? BranchId = null,
    bool? IsLocked = null,
    bool? IsActive = null,
    string? Password = null,
    int? DefaultCashBoxId = null);

// ─── Password management ──────────────────────────────

public record SetPasswordRequest(
    string Password,
    string ConfirmPassword,
    string Token);  // Required: one-time password reset token (from admin reset or creation flow)

public record ResetUserPasswordRequest(int UserId);

// ─── Role management ─────────────────────────────────

public record UpdateUserRoleRequest(short RoleId);
