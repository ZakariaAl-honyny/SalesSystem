using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة مراكز التكلفة — تعيد بيانات من MockCostCentersData.
/// </summary>
public class MockCostCenterApiService : ICostCenterApiService
{
    public Task<Result<IReadOnlyList<CostCenterDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<CostCenterDto> query = MockCostCentersData.CostCenters;

        if (!includeInactive)
            query = query.Where(c => c.Status == 1);

        var result = query.OrderBy(c => c.Code).ToList().AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<CostCenterDto>>.Success(result));
    }

    public Task<Result<CostCenterDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var center = MockCostCentersData.CostCenters.FirstOrDefault(c => c.Id == id);
        return center is null
            ? Task.FromResult(Result<CostCenterDto>.Failure("مركز التكلفة غير موجود"))
            : Task.FromResult(Result<CostCenterDto>.Success(center));
    }

    public Task<Result<CostCenterDto>> CreateAsync(CostCenterDto dto, CancellationToken ct = default)
    {
        // التحقق من عدم تكرار الكود
        if (MockCostCentersData.CostCenters.Any(c => c.Code == dto.Code))
            return Task.FromResult(Result<CostCenterDto>.Failure("يوجد مركز تكلفة بنفس الكود مسبقاً"));

        var nextId = MockCostCentersData.CostCenters.Count > 0
            ? MockCostCentersData.CostCenters.Max(c => c.Id) + 1
            : 1;

        var center = dto with { Id = nextId };
        MockCostCentersData.CostCenters.Add(center);
        return Task.FromResult(Result<CostCenterDto>.Success(center));
    }

    public Task<Result<CostCenterDto>> UpdateAsync(int id, CostCenterDto dto, CancellationToken ct = default)
    {
        var existing = MockCostCentersData.CostCenters.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result<CostCenterDto>.Failure("مركز التكلفة غير موجود"));

        // التحقق من عدم تكرار الكود (باستثناء السجل نفسه)
        if (MockCostCentersData.CostCenters.Any(c => c.Code == dto.Code && c.Id != id))
            return Task.FromResult(Result<CostCenterDto>.Failure("يوجد مركز تكلفة آخر بنفس الكود"));

        var updated = existing with
        {
            Code = dto.Code,
            Name = dto.Name,
            NameEn = dto.NameEn,
            ParentCode = dto.ParentCode,
            Level = dto.Level,
            IsLeaf = dto.IsLeaf,
            Budget = dto.Budget,
            ManagerName = dto.ManagerName,
            Status = dto.Status,
            Notes = dto.Notes
        };

        var index = MockCostCentersData.CostCenters.IndexOf(existing);
        MockCostCentersData.CostCenters[index] = updated;
        return Task.FromResult(Result<CostCenterDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockCostCentersData.CostCenters.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("مركز التكلفة غير موجود"));

        // التحقق من عدم وجود مراكز فرعية مرتبطة
        var hasChildren = MockCostCentersData.CostCenters.Any(c => c.ParentCode == existing.Code);
        if (hasChildren)
            return Task.FromResult(Result.Failure("لا يمكن حذف مركز تكلفة يحتوي على مراكز فرعية"));

        var index = MockCostCentersData.CostCenters.IndexOf(existing);
        MockCostCentersData.CostCenters[index] = existing with { Status = 2 };
        return Task.FromResult(Result.Success());
    }
}
