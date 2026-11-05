namespace SalesSystem.DTO.Responses;

public record LoginResponse(int UserId, string UserName, bool IsAdmin,
    short? BranchId, string Token, DateTime ExpiresAt, bool MustChangePassword = false,
    bool IsPasswordless = false)
{
    // Backward-compatible properties
    public string FullName => UserName;
}
