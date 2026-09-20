using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة الضرائب — تعيد بيانات من MockData.
/// </summary>
public class MockTaxesApiService : ITaxesApiService
{
    public Task<Result<IReadOnlyList<TaxDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<TaxDto> query = MockData.Taxes;

        if (!includeInactive)
            query = query.Where(t => t.IsActive);

        return Task.FromResult(Result<IReadOnlyList<TaxDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<TaxDto>> CreateAsync(CreateTaxRequest request, CancellationToken ct = default)
    {
        var nextId = MockData.Taxes.Count > 0 ? MockData.Taxes.Max(t => t.Id) + 1 : 1;
        var tax = new TaxDto(nextId, request.Name, request.Code, request.Rate, request.TaxType, request.IsDefault, true);
        MockData.Taxes.Add(tax);
        return Task.FromResult(Result<TaxDto>.Success(tax));
    }

    public Task<Result<TaxDto>> UpdateAsync(int id, UpdateTaxRequest request, CancellationToken ct = default)
    {
        var existing = MockData.Taxes.FirstOrDefault(t => t.Id == id);
        if (existing is null)
            return Task.FromResult(Result<TaxDto>.Failure("الضريبة غير موجودة"));

        var updated = existing with
        {
            Name = request.Name,
            Code = request.Code,
            Rate = request.Rate,
            TaxType = request.TaxType,
            IsDefault = request.IsDefault,
            IsActive = request.IsActive
        };

        var index = MockData.Taxes.IndexOf(existing);
        MockData.Taxes[index] = updated;
        return Task.FromResult(Result<TaxDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.Taxes.FirstOrDefault(t => t.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الضريبة غير موجودة"));

        var index = MockData.Taxes.IndexOf(existing);
        MockData.Taxes[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeletePermanentlyAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.Taxes.FirstOrDefault(t => t.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الضريبة غير موجودة"));

        MockData.Taxes.Remove(existing);
        return Task.FromResult(Result.Success());
    }
}
