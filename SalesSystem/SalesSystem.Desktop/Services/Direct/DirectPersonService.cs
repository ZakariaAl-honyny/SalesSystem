using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة الأشخاص — يستدعي خدمات طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectPersonService : IPersonApiService
{
    private readonly BizInterfaces.IPersonService _people;

    public DirectPersonService(BizInterfaces.IPersonService people)
    {
        _people = people;
    }

    public async Task<Result<IReadOnlyList<PersonDto>>> GetAllAsync(
        string? search = null, bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // BLL returns a raw list (no Result) and has no includeInactive filter.
            // TODO: includeInactive غير مدعوم في طبقة الأعمال حالياً.
            var list = await _people.GetAllAsync(search);
            return Result<IReadOnlyList<PersonDto>>.Success(list);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<PersonDto>>.Failure("حدث خطأ أثناء جلب قائمة الأشخاص");
        }
    }

    public async Task<Result<PersonDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _people.GetByIdAsync(id);
        }
        catch (Exception)
        {
            return Result<PersonDto>.Failure("حدث خطأ أثناء جلب بيانات الشخص");
        }
    }

    public async Task<Result<PersonDto>> GetByNationalNoAsync(string nationalNo, CancellationToken ct = default)
    {
        try
        {
            return await _people.GetByNationalNoAsync(nationalNo);
        }
        catch (Exception)
        {
            return Result<PersonDto>.Failure("حدث خطأ أثناء جلب بيانات الشخص بالرقم الوطني");
        }
    }

    public async Task<Result<PersonDto>> CreateAsync(CreatePersonRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _people.CreateAsync(r);
        }
        catch (Exception)
        {
            return Result<PersonDto>.Failure("حدث خطأ أثناء إنشاء بيانات الشخص");
        }
    }

    public async Task<Result<PersonDto>> UpdateAsync(int id, UpdatePersonRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _people.UpdateAsync(id, r);
        }
        catch (Exception)
        {
            return Result<PersonDto>.Failure("حدث خطأ أثناء تحديث بيانات الشخص");
        }
    }

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _people.DeactivateAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("حدث خطأ أثناء تعطيل الشخص");
        }
    }
}
