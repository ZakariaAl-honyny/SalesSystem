using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة الأدوار — تعيد بيانات من MockSecurityData.
/// </summary>
public class MockRoleApiService : IRoleApiService
{
    public Task<Result<IReadOnlyList<RoleDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<RoleDto> query = MockSecurityData.Roles;

        if (!includeInactive)
            query = query.Where(r => r.IsActive);

        return Task.FromResult(Result<IReadOnlyList<RoleDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<RoleDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var role = MockSecurityData.Roles.FirstOrDefault(r => r.Id == id);
        return role is null
            ? Task.FromResult(Result<RoleDto>.Failure("الدور غير موجود"))
            : Task.FromResult(Result<RoleDto>.Success(role));
    }

    public Task<Result<RoleDto>> CreateAsync(CreateRoleRequest request, CancellationToken ct = default)
    {
        var nextId = MockSecurityData.Roles.Max(r => r.Id) + 1;
        var role = new RoleDto(
            Id: nextId,
            Name: request.Name,
            Description: request.Description,
            IsActive: true,
            IsSystem: false,
            SalesPermissions: request.SalesPermissions,
            InventoryPermissions: request.InventoryPermissions,
            AccountingPermissions: request.AccountingPermissions,
            SystemPermissions: request.SystemPermissions);

        MockSecurityData.Roles.Add(role);
        return Task.FromResult(Result<RoleDto>.Success(role));
    }

    public Task<Result<RoleDto>> UpdateAsync(int id, UpdateRoleRequest request, CancellationToken ct = default)
    {
        var existing = MockSecurityData.Roles.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult(Result<RoleDto>.Failure("الدور غير موجود"));

        if (existing.IsSystem)
            return Task.FromResult(Result<RoleDto>.Failure("لا يمكن تعديل دور نظام — الدور محمي."));

        var updated = existing with
        {
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive,
            SalesPermissions = request.SalesPermissions ?? existing.SalesPermissions,
            InventoryPermissions = request.InventoryPermissions ?? existing.InventoryPermissions,
            AccountingPermissions = request.AccountingPermissions ?? existing.AccountingPermissions,
            SystemPermissions = request.SystemPermissions ?? existing.SystemPermissions
        };

        var index = MockSecurityData.Roles.IndexOf(existing);
        MockSecurityData.Roles[index] = updated;
        return Task.FromResult(Result<RoleDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockSecurityData.Roles.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الدور غير موجود"));

        if (existing.IsSystem)
            return Task.FromResult(Result.Failure("لا يمكن حذف دور نظام — الدور محمي."));

        var index = MockSecurityData.Roles.IndexOf(existing);
        MockSecurityData.Roles[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }
}
