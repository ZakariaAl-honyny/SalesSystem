namespace SalesSystem.DTO;

public record UserDto(int Id, string UserName,
    short? BranchId, bool MustChangePassword, bool IsLocked, bool IsActive,
    DateTime? LastLoginAt, short LoginAttempts, int? DefaultCashBoxId)
{
    public string StatusDisplay => IsActive ? (IsLocked ? "مقفل" : "نشط") : "معطل";
}

public record CurrentUserDto(int Id, string UserName,
    short? BranchId);

public record ChangePasswordRequest(string CurrentPassword, string NewPassword, string ConfirmPassword);
