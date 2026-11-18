namespace SalesSystem.DTO.Responses;

public record DashboardSummaryResponse(
    decimal TotalSalesToday,
    decimal TotalPurchasesToday,
    int LowStockItemsCount,
    decimal TotalReceivables,
    decimal TotalPayables,
    int PendingInvoices,
    decimal TotalStockValue,
    List<TopProductResponse> TopProducts
);

public record TopProductResponse(string Name, decimal Quantity);

public record SettingsResponse(
    string StoreName, string? Address, string? Phone,
    string? LogoUrl, string Currency
);

public record UserResponse(
    int Id, string Username, string Role, bool IsActive, bool IsLocked
);

/// <summary>
/// Response returned after admin resets a user's password.
/// The password is reset to the default "12345678" and the user must change on next login.
/// </summary>
public record ResetPasswordResponse(
    int UserId,
    string Message
);
