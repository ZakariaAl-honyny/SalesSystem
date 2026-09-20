using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.Models;

namespace SalesSystem.Business.Services;

public class BranchService : IBranchService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<BranchService> _logger;

    public BranchService(IUnitOfWork uow, ILogger<BranchService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<Result<List<BranchDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            var query = await _uow.Branches.ToListAsync(ct);
            if (!includeInactive)
                query = query.Where(x => x.IsActive).ToList();
            var dtos = query.OrderByDescending(x => x.Id).Select(MapToDto).ToList();
            return Result<List<BranchDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving branches");
            return Result<List<BranchDto>>.Failure("حدث خطأ أثناء استرجاع قائمة الفروع");
        }
    }

    public async Task<Result<BranchDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var branch = await _uow.Branches.GetByIdAsync(id, ct);
            if (branch == null)
                return Result<BranchDto>.Failure("الفرع غير موجود");
            return Result<BranchDto>.Success(MapToDto(branch));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving branch {Id}", id);
            return Result<BranchDto>.Failure("حدث خطأ أثناء استرجاع بيانات الفرع");
        }
    }

    public async Task<Result<BranchDto>> CreateAsync(string name, string? address, string? phone, int userId, CancellationToken ct)
    {
        try
        {
            var branch = Branch.Create(name, address, phone, userId);
            await _uow.Branches.AddAsync(branch, ct);
            await _uow.SaveChangesAsync(ct);
            return Result<BranchDto>.Success(MapToDto(branch));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating branch");
            return Result<BranchDto>.Failure("حدث خطأ أثناء إنشاء الفرع");
        }
    }

    public async Task<Result<BranchDto>> UpdateAsync(int id, string name, string? address, string? phone, int userId, CancellationToken ct)
    {
        try
        {
            var branch = await _uow.Branches.GetByIdAsync(id, ct);
            if (branch == null)
                return Result<BranchDto>.Failure("الفرع غير موجود");
            branch.Update(name, address, phone, userId);
            await _uow.SaveChangesAsync(ct);
            return Result<BranchDto>.Success(MapToDto(branch));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating branch {Id}", id);
            return Result<BranchDto>.Failure("حدث خطأ أثناء تحديث الفرع");
        }
    }

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct)
    {
        try
        {
            var branch = await _uow.Branches.GetByIdAsync(id, ct);
            if (branch == null)
                return Result.Failure("الفرع غير موجود");
            branch.MarkAsDeleted();
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating branch {Id}", id);
            return Result.Failure("حدث خطأ أثناء تعطيل الفرع");
        }
    }

    public async Task<Result> RestoreAsync(int id, CancellationToken ct)
    {
        try
        {
            var branch = await _uow.Branches.GetByIdAsync(id, ct);
            if (branch == null)
                return Result.Failure("الفرع غير موجود");
            branch.Restore();
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring branch {Id}", id);
            return Result.Failure("حدث خطأ أثناء استعادة الفرع");
        }
    }

    private static BranchDto MapToDto(Branch branch) => new(
        branch.Id,
        branch.Name,
        branch.Address,
        branch.Phone,
        branch.IsActive);
}
