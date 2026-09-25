using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة الوحدات — تعيد بيانات من MockLookupsData.
/// </summary>
public class MockUnitApiService : IUnitApiService
{
    public Task<Result<IReadOnlyList<UnitDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<UnitDto> query = MockLookupsData.Units;

        if (!includeInactive)
            query = query.Where(u => u.IsActive);

        return Task.FromResult(Result<IReadOnlyList<UnitDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<UnitDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var unit = MockLookupsData.Units.FirstOrDefault(u => u.Id == id);
        if (unit is null)
            return Task.FromResult(Result<UnitDto>.Failure("الوحدة غير موجودة"));

        return Task.FromResult(Result<UnitDto>.Success(unit));
    }

    public Task<Result<UnitDto>> CreateAsync(CreateUnitRequest r, CancellationToken ct = default)
    {
        // التحقق من عدم تكرار الاسم
        if (MockLookupsData.Units.Any(u => u.Name == r.Name))
            return Task.FromResult(Result<UnitDto>.Failure("اسم الوحدة مستخدم بالفعل"));

        var nextId = MockLookupsData.Units.Count > 0
            ? MockLookupsData.Units.Max(u => u.Id) + 1
            : 1;

        var unit = new UnitDto(nextId, r.Name, r.Symbol, true);
        MockLookupsData.Units.Add(unit);

        return Task.FromResult(Result<UnitDto>.Success(unit));
    }

    public Task<Result<UnitDto>> UpdateAsync(int id, UpdateUnitRequest r, CancellationToken ct = default)
    {
        var existing = MockLookupsData.Units.FirstOrDefault(u => u.Id == id);
        if (existing is null)
            return Task.FromResult(Result<UnitDto>.Failure("الوحدة غير موجودة"));

        // التحقق من عدم تكرار الاسم (باستثناء السجل الحالي)
        if (MockLookupsData.Units.Any(u => u.Name == r.Name && u.Id != id))
            return Task.FromResult(Result<UnitDto>.Failure("اسم الوحدة مستخدم بالفعل"));

        var updated = existing with
        {
            Name = r.Name,
            Symbol = r.Symbol,
            IsActive = r.IsActive
        };

        var index = MockLookupsData.Units.IndexOf(existing);
        MockLookupsData.Units[index] = updated;

        return Task.FromResult(Result<UnitDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockLookupsData.Units.FirstOrDefault(u => u.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الوحدة غير موجودة"));

        var index = MockLookupsData.Units.IndexOf(existing);
        MockLookupsData.Units[index] = existing with { IsActive = false };

        return Task.FromResult(Result.Success());
    }
}
