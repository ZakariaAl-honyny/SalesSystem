namespace SalesSystem.Common;

/// <summary>
/// Entity with audit fields tracking creation and modification.
/// No activation (IsActive) — use <see cref="ActivatableEntity"/> for soft-deletable entities.
/// No document life cycle — use <see cref="DocumentEntity"/> for entities with Draft/Posted/Cancelled status.
/// 
/// The UserName fields are historical snapshots captured from the user session at the time of
/// the operation — they are NOT foreign keys and are not updated when a user is renamed.
/// </summary>
public abstract class AuditableEntity : Entity
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    
    /// <summary>
    /// Historical snapshot of the user's display name at creation time.
    /// Captured from the session (ISessionService.Current.UserName) — not a foreign key.
    /// </summary>
    public string? CreatedByUserName { get; protected set; }
    
    /// <summary>
    /// Historical snapshot of the user's display name at last modification time.
    /// Captured from the session (ISessionService.Current.UserName) — not a foreign key.
    /// </summary>
    public string? UpdatedByUserName { get; protected set; }

    protected AuditableEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public virtual void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the creator's user name snapshot only (no user ID).
    /// The value is a historical snapshot of the session username, taken from 
    /// <c>ICurrentUserProvider.UserName</c> at the time of creation — never typed by the user 
    /// and never a foreign key. Empty or whitespace-only values are normalized to null.
    /// </summary>
    public virtual void SetCreatedBy(string? userName)
    {
        CreatedByUserName = string.IsNullOrWhiteSpace(userName) ? null : userName.Trim();
    }

    /// <summary>
    /// Sets the last modifier's user name snapshot only (no user ID).
    /// The value is a historical snapshot of the session username, taken from 
    /// <c>ICurrentUserProvider.UserName</c> at the time of modification — never typed by the user 
    /// and never a foreign key. Empty or whitespace-only values are normalized to null.
    /// </summary>
    public virtual void SetUpdatedBy(string? userName)
    {
        UpdatedByUserName = string.IsNullOrWhiteSpace(userName) ? null : userName.Trim();
    }
}
