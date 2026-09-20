using SalesSystem.Common;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Represents a system user for authentication and authorization.
/// Schema §1.9 — Users table. Uses two bit flags for state:
///   IsActive (from ActivatableEntity): active=1, soft-deleted=0
///   IsLocked: unlocked=0, locked=1 (after 5 failed login attempts)
/// Roles are assigned via RoleId FK to the Role entity (no join table).
/// Uses passwordless creation by default (MustChangePassword = true).
/// </summary>
public class User : ActivatableEntity
{
    public string UserName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public short RoleId { get; private set; }
    public short? BranchId { get; private set; }
    public int? EmployeeId { get; private set; }
    public bool IsLocked { get; private set; }
    public bool MustChangePassword { get; private set; } = true;
    public DateTime? LastLoginAt { get; private set; }
    public short LoginAttempts { get; private set; }

    // Navigation
    public Role Role { get; private set; } = null!;

    protected User() { }

    /// <summary>
    /// Creates a new user WITHOUT a password (passwordless setup).
    /// MustChangePassword is set to true — use <see cref="SetInitialPassword"/> or
    /// <see cref="ChangePassword"/> to set the password later.
    /// </summary>
    public static User Create(string userName, short roleId, short? branchId = null,
        int? employeeId = null, int? createdByUserId = null)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new DomainException("اسم المستخدم مطلوب.");

        var user = new User
        {
            UserName = userName.Trim(),
            RoleId = roleId,
            BranchId = branchId,
            EmployeeId = employeeId,
            MustChangePassword = true,
            LoginAttempts = 0
        };
        user.SetCreatedBy(createdByUserId);
        return user;
    }

    /// <summary>
    /// Creates a user directly with a pre-hashed password (internal/seed/admin use only).
    /// MustChangePassword is false by default for seeds.
    /// </summary>
    public static User CreateWithPassword(string userName, string passwordHash, short roleId,
        short? branchId = null, int? employeeId = null,
        int? createdByUserId = null, bool mustChangePassword = false)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new DomainException("اسم المستخدم مطلوب.");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("كلمة المرور مطلوبة.");

        var user = new User
        {
            UserName = userName.Trim(),
            PasswordHash = passwordHash,
            RoleId = roleId,
            BranchId = branchId,
            EmployeeId = employeeId,
            MustChangePassword = mustChangePassword,
            LoginAttempts = 0
        };
        user.SetCreatedBy(createdByUserId);
        return user;
    }

    /// <summary>
    /// Updates the user's profile information.
    /// Does NOT change UserName or password — those have dedicated methods.
    /// </summary>
    public void Update(short roleId, short? branchId = null, int? employeeId = null, int? updatedByUserId = null)
    {
        RoleId = roleId;
        BranchId = branchId;
        EmployeeId = employeeId;
        SetUpdatedBy(updatedByUserId);
        UpdateTimestamp();
    }

    /// <summary>
    /// Sets the initial password for a user created without one.
    /// Only allowed when MustChangePassword is true.
    /// </summary>
    public void SetInitialPassword(string passwordHash)
    {
        if (!MustChangePassword)
            throw new DomainException("كلمة المرور تم تعيينها مسبقاً.");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("كلمة المرور مطلوبة.");
        PasswordHash = passwordHash;
        MustChangePassword = false;
        UpdateTimestamp();
    }

    /// <summary>
    /// Changes an existing password (requires a non-empty new hash).
    /// </summary>
    public void ChangePassword(string newPasswordHash, int? updatedByUserId = null)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("كلمة المرور الجديدة مطلوبة.");
        PasswordHash = newPasswordHash;
        MustChangePassword = false;
        LoginAttempts = 0;
        SetUpdatedBy(updatedByUserId);
        UpdateTimestamp();
    }

    /// <summary>
    /// Resets the password to a new hash (admin-initiated reset).
    /// Sets MustChangePassword = true to force the user to change on next login.
    /// </summary>
    public void ResetPassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("كلمة المرور مطلوبة.");
        PasswordHash = newPasswordHash;
        MustChangePassword = true;
        LoginAttempts = 0;
        UpdateTimestamp();
    }

    /// <summary>
    /// Records a login attempt. On success, resets attempts and updates LastLoginAt.
    /// On failure, increments attempts and locks the account after 5 failures.
    /// </summary>
    public void RecordLoginAttempt(bool success)
    {
        if (success)
        {
            LoginAttempts = 0;
            IsLocked = false;
            LastLoginAt = DateTime.UtcNow;
        }
        else
        {
            LoginAttempts++;
            if (LoginAttempts >= 5)
                IsLocked = true;
        }
        UpdateTimestamp();
    }

    /// <summary>
    /// Locks the user account manually (admin action).
    /// </summary>
    public void Lock()
    {
        IsLocked = true;
        UpdateTimestamp();
    }

    /// <summary>
    /// Unlocks the user account and resets login attempts.
    /// </summary>
    public void Unlock()
    {
        IsLocked = false;
        LoginAttempts = 0;
        UpdateTimestamp();
    }

    /// <summary>
    /// Deactivates the user (soft delete via IsActive = false).
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }

    /// <summary>
    /// Reactivates the user.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }

    /// <summary>
    /// Sets whether the user must change password on next login.
    /// </summary>
    public void SetMustChangePassword(bool value)
    {
        MustChangePassword = value;
        UpdateTimestamp();
    }

    /// <summary>
    /// Soft-deletes the user (sets IsActive = false, unlocks).
    /// </summary>
    public override void MarkAsDeleted()
    {
        IsActive = false;
        IsLocked = false;
        base.MarkAsDeleted();
    }

    /// <summary>
    /// Restores a soft-deleted user.
    /// </summary>
    public override void Restore()
    {
        IsActive = true;
        IsLocked = false;
        base.Restore();
    }

    /// <summary>
    /// Updates the user's branch assignment.
    /// </summary>
    public void SetBranchId(short? branchId)
    {
        BranchId = branchId;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates the user's role assignment.
    /// </summary>
    public void SetRoleId(short roleId)
    {
        RoleId = roleId;
        UpdateTimestamp();
    }
}
