using System.Collections.Concurrent;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class PersonApiService : IPersonApiService
{
    private static int _nextId = 1;
    private static readonly ConcurrentDictionary<int, PersonDto> _store = new();
    private static readonly object _lock = new();

    static PersonApiService()
    {
        Seed();
    }

    private static void Seed()
    {
        var sample = new[]
        {
            new PersonDto(1, "أحمد", "محمد", "علي", "السعيد", "11998877", 1, new DateTime(1990, 5, 15), "0512345678", "صنعاء", "ahmed@test.com", null, 1, "اليمن", true),
            new PersonDto(2, "فاطمة", "عبدالله", "حسين", "العمري", "22887766", 2, new DateTime(1995, 8, 20), "0598765432", "عدن", "fatima@test.com", null, 1, "اليمن", true),
            new PersonDto(3, "خالد", "عبدالرحمن", null, "البكر", "33776655", 1, new DateTime(1985, 3, 10), "0555123456", "تعز", null, null, 1, "اليمن", true),
            new PersonDto(4, "نورة", "سالم", "بنت", "الحربي", "44665544", 2, new DateTime(1992, 11, 25), "0544987654", "الرياض", "noura@test.com", null, 2, "السعودية", true),
        };
        foreach (var p in sample)
            _store.TryAdd(p.Id, p);
        _nextId = 5;
    }

    public Task<Result<IReadOnlyList<PersonDto>>> GetAllAsync(string? search = null, bool includeInactive = false, CancellationToken ct = default)
    {
        var list = _store.Values.AsEnumerable();
        if (!includeInactive) list = list.Where(p => p.IsActive);
        if (!string.IsNullOrWhiteSpace(search))
            list = list.Where(p =>
                p.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.NationalNo.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Phone != null && p.Phone.Contains(search, StringComparison.OrdinalIgnoreCase));
        var result = list.OrderByDescending(p => p.Id).ToList().AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<PersonDto>>.Success(result));
    }

    public Task<Result<PersonDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        if (_store.TryGetValue(id, out var person))
            return Task.FromResult(Result<PersonDto>.Success(person));
        return Task.FromResult(Result<PersonDto>.Failure("الشخص غير موجود"));
    }

    public Task<Result<PersonDto>> GetByNationalNoAsync(string nationalNo, CancellationToken ct = default)
    {
        var person = _store.Values.FirstOrDefault(p => p.NationalNo == nationalNo);
        if (person != null)
            return Task.FromResult(Result<PersonDto>.Success(person));
        return Task.FromResult(Result<PersonDto>.Failure("لا يوجد شخص بهذا الرقم الوطني"));
    }

    public Task<Result<PersonDto>> CreateAsync(CreatePersonRequest r, CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (_store.Values.Any(p => p.NationalNo == r.NationalNo))
                return Task.FromResult(Result<PersonDto>.Failure("الرقم الوطني محجوز لشخص آخر"));

            var id = Interlocked.Increment(ref _nextId);
            var dto = new PersonDto(
                id, r.FirstName, r.SecondName, r.ThirdName, r.LastName,
                r.NationalNo, r.Gender, r.DateOfBirth, r.Phone, r.Address,
                r.Email, r.ImagePath, r.NationalityCountryId, null, true);
            _store.TryAdd(id, dto);
            return Task.FromResult(Result<PersonDto>.Success(dto));
        }
    }

    public Task<Result<PersonDto>> UpdateAsync(int id, UpdatePersonRequest r, CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (!_store.TryGetValue(id, out var existing))
                return Task.FromResult(Result<PersonDto>.Failure("الشخص غير موجود"));

            if (_store.Values.Any(p => p.NationalNo == r.NationalNo && p.Id != id))
                return Task.FromResult(Result<PersonDto>.Failure("الرقم الوطني محجوز لشخص آخر"));

            var updated = new PersonDto(
                id, r.FirstName, r.SecondName, r.ThirdName, r.LastName,
                r.NationalNo, r.Gender, r.DateOfBirth, r.Phone, r.Address,
                r.Email, r.ImagePath, r.NationalityCountryId, existing.CountryName, r.IsActive);
            _store[id] = updated;
            return Task.FromResult(Result<PersonDto>.Success(updated));
        }
    }

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (!_store.TryGetValue(id, out var existing))
                return Task.FromResult(Result.Failure("الشخص غير موجود"));

            var updated = new PersonDto(
                existing.Id, existing.FirstName, existing.SecondName, existing.ThirdName,
                existing.LastName, existing.NationalNo, existing.Gender, existing.DateOfBirth,
                existing.Phone, existing.Address, existing.Email, existing.ImagePath,
                existing.NationalityCountryId, existing.CountryName, false);
            _store[id] = updated;
            return Task.FromResult(Result.Success());
        }
    }
}
