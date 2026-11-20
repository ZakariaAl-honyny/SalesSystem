namespace SalesSystem.DTO.Requests;

// ─── User CRUD ────────────────────────────────────────

public record CreateUserRequest(
    string UserName,
    string? Password = null,
    int? DefaultCashBoxId = null);

public record UpdateUserRequest(
    bool? IsLocked = null,
    bool? IsActive = null,
    string? Password = null,
    int? DefaultCashBoxId = null);
