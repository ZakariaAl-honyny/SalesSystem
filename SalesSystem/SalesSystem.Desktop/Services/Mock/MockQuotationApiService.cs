using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة عروض الأسعار — تعيد بيانات من MockQuotationsData.
/// </summary>
public class MockQuotationApiService : IQuotationApiService
{
    public Task<Result<IReadOnlyList<QuotationDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var query = MockQuotationsData.Quotations.AsEnumerable();

        // الحالة 5 = مرفوض — تعتبر "غير نشط" لعروض الأسعار
        if (!includeInactive)
            query = query.Where(q => q.Status != 5);

        return Task.FromResult(
            Result<IReadOnlyList<QuotationDto>>.Success(query.OrderByDescending(q => q.QuotationDate).ToList().AsReadOnly()));
    }

    public Task<Result<QuotationDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var quotation = MockQuotationsData.Quotations.FirstOrDefault(q => q.Id == id);
        return quotation is null
            ? Task.FromResult(Result<QuotationDto>.Failure("عرض السعر غير موجود"))
            : Task.FromResult(Result<QuotationDto>.Success(quotation));
    }

    public Task<Result<QuotationDto>> CreateAsync(QuotationDto request, CancellationToken ct = default)
    {
        var nextId = MockQuotationsData.Quotations.Count == 0 ? 1 : MockQuotationsData.Quotations.Max(q => q.Id) + 1;
        var nextNo = MockQuotationsData.Quotations.Count == 0 ? 4001 : MockQuotationsData.Quotations.Max(q => q.QuotationNo) + 1;

        var quotation = request with
        {
            Id = nextId,
            QuotationNo = nextNo,
            Status = 1 // مسودة
        };

        MockQuotationsData.Quotations.Add(quotation);
        return Task.FromResult(Result<QuotationDto>.Success(quotation));
    }

    public Task<Result<QuotationDto>> UpdateAsync(int id, QuotationDto request, CancellationToken ct = default)
    {
        var existing = MockQuotationsData.Quotations.FirstOrDefault(q => q.Id == id);
        if (existing is null)
            return Task.FromResult(Result<QuotationDto>.Failure("عرض السعر غير موجود"));

        if (existing.Status == 4)
            return Task.FromResult(Result<QuotationDto>.Failure("لا يمكن تعديل عرض سعر مُحوَّل"));

        var updated = request with { Id = id, QuotationNo = existing.QuotationNo };
        var index = MockQuotationsData.Quotations.IndexOf(existing);
        MockQuotationsData.Quotations[index] = updated;
        return Task.FromResult(Result<QuotationDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockQuotationsData.Quotations.FirstOrDefault(q => q.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("عرض السعر غير موجود"));

        if (existing.Status == 4)
            return Task.FromResult(Result.Failure("لا يمكن حذف عرض سعر مُحوَّل"));

        // حذف ناعم: تغيير الحالة إلى مرفوض (5)
        var index = MockQuotationsData.Quotations.IndexOf(existing);
        MockQuotationsData.Quotations[index] = existing with { Status = 5 };
        return Task.FromResult(Result.Success());
    }
}
