using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة العملات — تعيد بيانات من MockCurrenciesData.
/// </summary>
public class MockCurrencyApiService : ICurrencyApiService
{
    public Task<Result<IReadOnlyList<CurrencyDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        // العملات لا تحتوي حقل IsActive في هذا الإصدار — أعد القائمة كاملة دائماً
        var result = MockCurrenciesData.Currencies.ToList().AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<CurrencyDto>>.Success(result));
    }

    public Task<Result<CurrencyDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var currency = MockCurrenciesData.Currencies.FirstOrDefault(c => c.Id == id);
        return currency is null
            ? Task.FromResult(Result<CurrencyDto>.Failure("العملة غير موجودة"))
            : Task.FromResult(Result<CurrencyDto>.Success(currency));
    }

    public Task<Result<CurrencyDto>> CreateAsync(CurrencyDto request, CancellationToken ct = default)
    {
        // تحقق من عدم تكرار الكود
        if (MockCurrenciesData.Currencies.Any(c => c.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase)))
            return Task.FromResult(Result<CurrencyDto>.Failure("يوجد عملة بنفس الكود مسبقاً"));

        var nextId = MockCurrenciesData.Currencies.Count > 0
            ? MockCurrenciesData.Currencies.Max(c => c.Id) + 1
            : 1;

        var currency = request with { Id = nextId };
        MockCurrenciesData.Currencies.Add(currency);
        return Task.FromResult(Result<CurrencyDto>.Success(currency));
    }

    public Task<Result<CurrencyDto>> UpdateAsync(int id, CurrencyDto request, CancellationToken ct = default)
    {
        var existing = MockCurrenciesData.Currencies.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result<CurrencyDto>.Failure("العملة غير موجودة"));

        // تحقق من عدم تكرار الكود (باستثناء السجل نفسه)
        if (MockCurrenciesData.Currencies.Any(c =>
                c.Id != id && c.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase)))
            return Task.FromResult(Result<CurrencyDto>.Failure("يوجد عملة بنفس الكود مسبقاً"));

        var updated = existing with
        {
            Code = request.Code,
            Name = request.Name,
            SubUnit = request.SubUnit,
            IsLocal = request.IsLocal
        };

        var index = MockCurrenciesData.Currencies.IndexOf(existing);
        MockCurrenciesData.Currencies[index] = updated;
        return Task.FromResult(Result<CurrencyDto>.Success(updated));
    }

    public Task<Result<IReadOnlyList<CurrencyRateDto>>> GetRatesAsync(int currencyId, CancellationToken ct = default)
    {
        var rates = MockCurrenciesData.Rates
            .Where(r => r.CurrencyId == currencyId)
            .OrderByDescending(r => r.IsCurrent)
            .ThenByDescending(r => r.FromDate)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(Result<IReadOnlyList<CurrencyRateDto>>.Success(rates));
    }
}
