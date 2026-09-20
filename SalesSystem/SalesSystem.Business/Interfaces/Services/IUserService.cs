using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Business.Interfaces.Services;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<UserDto>> GetByUserNameAsync(string userName, CancellationToken ct);
    Task<Result<IReadOnlyList<UserDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<UserDto>> CreateAsync(CreateUserRequest request, CancellationToken ct);
    Task<Result<UserDto>> UpdateAsync(int id, UpdateUserRequest request, CancellationToken ct);
    Task<Result> DeleteAsync(int id, CancellationToken ct);
    Task<Result> PermanentDeleteAsync(int id, CancellationToken ct);

    /// <summary>
    /// Gets the current user's profile with permissions from the assigned role.
    /// </summary>
    Task<Result<CurrentUserDto>> GetCurrentUserAsync(int userId, CancellationToken ct);

    /// <summary>
    /// Resets a user's password (admin function) to the default "12345678"
    /// and sets MustChangePassword = true.
    /// </summary>
    Task<Result<ResetPasswordResponse>> ResetPasswordAsync(int id, CancellationToken ct);

    // ─── Role Management (single-role model) ────────

    /// <summary>
    /// Gets the single role assigned to a user.
    /// </summary>
    Task<Result<RoleDto>> GetUserRoleAsync(int userId, CancellationToken ct);

    /// <summary>
    /// Updates the user's role assignment.
    /// </summary>
    Task<Result> UpdateUserRoleAsync(int userId, short roleId, CancellationToken ct);
}
