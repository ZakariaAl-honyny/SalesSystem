using SalesSystem.Common.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة الوحدات — تعيد بيانات من MockLookupsData.
/// </summary>
public class MockUnitDesktopService : IUnitDesktopService
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
        // تكافؤ الرفض مع الـBLL (RULE-N167): الاسم مطلوب
        if (string.IsNullOrWhiteSpace(r.Name))
            return Task.FromResult(Result<UnitDto>.Failure("اسم الوحدة مطلوب"));

        // التحقق من عدم تكرار الاسم — المقارنة في البيت الواحد StringExtensions.SameText (RULE-N169)
        if (MockLookupsData.Units.Any(u => u.Name.SameText(r.Name)))
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

        // تكافؤ الرفض مع الـBLL (RULE-N167): الاسم مطلوب
        if (string.IsNullOrWhiteSpace(r.Name))
            return Task.FromResult(Result<UnitDto>.Failure("اسم الوحدة مطلوب"));

        // التحقق من عدم تكرار الاسم (باستثناء السجل الحالي) — المقارنة في البيت الواحد StringExtensions.SameText
        if (MockLookupsData.Units.Any(u => u.Id != id && u.Name.SameText(r.Name)))
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

    /// <summary>
    /// حذف الوحدة (تعطيل) — <b>يرفض حذف وحدة مستخدمة في أي صنف</b>
    /// (تكافؤ <c>DeleteBehavior.Restrict</c> في الـBLL — RULE-N167):
    /// المرجع هو <c>MockData.ProductUnits.UnitId</c>.
    /// </summary>
    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockLookupsData.Units.FirstOrDefault(u => u.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("الوحدة غير موجودة"));

            // التحقق قبل أي تغيير — لا تعطيل لوحدة يعتمد عليها صنف
            if (MockData.ProductUnits.Any(pu => pu.UnitId == id))
                return Task.FromResult(Result.Failure("لا يمكن حذف وحدة مستخدمة في أصناف."));

            var index = MockLookupsData.Units.IndexOf(existing);
            MockLookupsData.Units[index] = existing with { IsActive = false };

            return Task.FromResult(Result.Success());
        }
        catch (Exception)
        {
            return Task.FromResult(Result.Failure("تعذر حذف الوحدة"));
        }
    }

    public Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockLookupsData.Units.FirstOrDefault(u => u.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الوحدة غير موجودة"));

        var index = MockLookupsData.Units.IndexOf(existing);
        MockLookupsData.Units[index] = existing with { IsActive = true };

        return Task.FromResult(Result.Success());
    }
}
