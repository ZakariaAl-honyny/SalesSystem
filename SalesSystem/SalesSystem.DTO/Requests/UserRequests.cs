namespace SalesSystem.DTO.Requests;

// ─── User CRUD ────────────────────────────────────────

public record CreateUserRequest(
    string UserName,
    short? BranchId = null,
    string? Password = null,
    int? DefaultCashBoxId = null);

public record UpdateUserRequest(
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
