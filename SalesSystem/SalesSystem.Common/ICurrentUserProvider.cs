namespace SalesSystem.Common;

/// <summary>
/// Single source of the acting user's identity for the DataAccess audit interceptor.
/// Desktop implements it from the session (ISessionService).
/// </summary>
public interface ICurrentUserProvider
{
    /// <summary>The authenticated user's ID, or null if not logged in.</summary>
    int? UserId { get; }

    /// <summary>The authenticated user's display name, or null if not logged in.</summary>
    string? UserName { get; }
}
