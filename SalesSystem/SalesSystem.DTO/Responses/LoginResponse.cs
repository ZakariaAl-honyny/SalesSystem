namespace SalesSystem.DTO.Responses;

public record LoginResponse(int UserId, string UserName, bool IsAdmin,
    short? BranchId, string Token, DateTime ExpiresAt, bool MustChangePassword = false,
    bool IsPasswordless = false,
    long SalesPermissions = -1, long InventoryPermissions = -1,
    long AccountingPermissions = -1, long SystemPermissions = -1)
{
    // Backward-compatible properties
    public string FullName => UserName;
}
