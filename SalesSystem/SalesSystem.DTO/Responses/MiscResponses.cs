namespace SalesSystem.DTO.Responses;

/// <summary>
/// Response returned after admin resets a user's password.
/// The password is reset to the default "12345678" and the user must change on next login.
/// </summary>
public record ResetPasswordResponse(
    int UserId,
    string Message
);
