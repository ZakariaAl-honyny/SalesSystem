namespace SalesSystem.DTO.Responses;

public record LoginResponse(int UserId, string UserName, short RoleId, string? RoleName,
    short? BranchId, string Token, DateTime ExpiresAt, bool MustChangePassword = false,
    bool IsPasswordless = false,
    long SalesPermissions = 0, long InventoryPermissions = 0,
    long AccountingPermissions = 0, long SystemPermissions = 0)
{
    // Backward-compatible properties
    public string FullName => UserName;
}
