using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة الأدوار — يستدعي خدمات طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectRoleService : IRoleApiService
{
    private readonly BizInterfaces.IRoleService _roles;

    public DirectRoleService(BizInterfaces.IRoleService roles)
    {
        _roles = roles;
    }

    public async Task<Result<IReadOnlyList<RoleDto>>> GetAllAsync(
        bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            return await _roles.GetAllAsync(includeInactive, ct);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<RoleDto>>.Failure("حدث خطأ أثناء جلب قائمة الأدوار");
        }
    }

    public async Task<Result<RoleDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _roles.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<RoleDto>.Failure("حدث خطأ أثناء جلب بيانات الدور");
        }
    }

    public async Task<Result<RoleDto>> CreateAsync(CreateRoleRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _roles.CreateAsync(request, ct);
        }
        catch (Exception)
        {
            return Result<RoleDto>.Failure("حدث خطأ أثناء إنشاء الدور");
        }
    }

    public async Task<Result<RoleDto>> UpdateAsync(int id, UpdateRoleRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _roles.UpdateAsync(id, request, ct);
        }
        catch (Exception)
        {
            return Result<RoleDto>.Failure("حدث خطأ أثناء تحديث بيانات الدور");
        }
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _roles.DeleteAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("حدث خطأ أثناء حذف الدور");
        }
    }
}
