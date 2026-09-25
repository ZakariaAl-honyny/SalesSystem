using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة التصنيفات — تعيد بيانات من MockLookupsData.
/// </summary>
public class MockCategoryApiService : ICategoryApiService
{
    public Task<Result<IReadOnlyList<CategoryDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<CategoryDto> query = MockLookupsData.Categories;

        if (!includeInactive)
            query = query.Where(c => c.IsActive);

        return Task.FromResult(Result<IReadOnlyList<CategoryDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<CategoryDto>> CreateAsync(CreateCategoryRequest r, CancellationToken ct = default)
    {
        // التحقق من عدم تكرار الاسم
        if (MockLookupsData.Categories.Any(c => c.Name == r.Name))
            return Task.FromResult(Result<CategoryDto>.Failure("اسم التصنيف مستخدم بالفعل"));

        var nextId = MockLookupsData.Categories.Count > 0
            ? MockLookupsData.Categories.Max(c => c.Id) + 1
            : 1;

        var category = new CategoryDto(nextId, r.Name, r.Description, true);
        MockLookupsData.Categories.Add(category);

        return Task.FromResult(Result<CategoryDto>.Success(category));
    }

    public Task<Result<CategoryDto>> UpdateAsync(int id, UpdateCategoryRequest r, CancellationToken ct = default)
    {
        var existing = MockLookupsData.Categories.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result<CategoryDto>.Failure("التصنيف غير موجود"));

        // التحقق من عدم تكرار الاسم (باستثناء السجل الحالي)
        if (MockLookupsData.Categories.Any(c => c.Name == r.Name && c.Id != id))
            return Task.FromResult(Result<CategoryDto>.Failure("اسم التصنيف مستخدم بالفعل"));

        var updated = existing with
        {
            Name = r.Name,
            Description = r.Description,
            IsActive = r.IsActive
        };

        var index = MockLookupsData.Categories.IndexOf(existing);
        MockLookupsData.Categories[index] = updated;

        return Task.FromResult(Result<CategoryDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockLookupsData.Categories.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("التصنيف غير موجود"));

        var index = MockLookupsData.Categories.IndexOf(existing);
        MockLookupsData.Categories[index] = existing with { IsActive = false };

        return Task.FromResult(Result.Success());
    }
}
