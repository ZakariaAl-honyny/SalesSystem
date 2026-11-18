using SalesSystem.Common.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة الصناديق النقدية — تعيد بيانات من MockData.
/// </summary>
public class MockCashBoxDesktopService : ICashBoxDesktopService
{
    public Task<Result<IReadOnlyList<CashBoxDto>>> GetAllAsync(CancellationToken ct = default)
    {
        IEnumerable<CashBoxDto> query = MockData.CashBoxes;
        return Task.FromResult(Result<IReadOnlyList<CashBoxDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<CashBoxDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var box = MockData.CashBoxes.FirstOrDefault(c => c.Id == id);
        return box is null
            ? Task.FromResult(Result<CashBoxDto>.Failure("الصندوق النقدي غير موجود"))
            : Task.FromResult(Result<CashBoxDto>.Success(box));
    }

    // ── تكافؤ الرفض مع الـBLL (RULE-N167): المقارنة النصية في البيت الواحد StringExtensions.SameText ──
    private static bool _NameExists(string? name, int? exceptId)
        => !string.IsNullOrWhiteSpace(name) &&
           MockData.CashBoxes.Any(c => c.Id != exceptId &&
               c.Name.SameText(name));

    public Task<Result<CashBoxDto>> CreateAsync(CreateCashBoxRequest r, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(r.Name))
            return Task.FromResult(Result<CashBoxDto>.Failure("اسم الصندوق مطلوب"));

        if (_NameExists(r.Name, null))
            return Task.FromResult(Result<CashBoxDto>.Failure("اسم الصندوق مستخدم بالفعل"));

        var nextId = MockData.CashBoxes.Count > 0 ? MockData.CashBoxes.Max(c => c.Id) + 1 : 1;

        var box = new CashBoxDto(
            Id: nextId,
            Name: r.Name,
            Description: r.Description,
            IsActive: true
        );
        MockData.CashBoxes.Add(box);
        return Task.FromResult(Result<CashBoxDto>.Success(box));
    }

    public Task<Result<CashBoxDto>> UpdateAsync(int id, UpdateCashBoxRequest r, CancellationToken ct = default)
    {
        var existing = MockData.CashBoxes.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result<CashBoxDto>.Failure("الصندوق النقدي غير موجود"));

        if (string.IsNullOrWhiteSpace(r.Name))
            return Task.FromResult(Result<CashBoxDto>.Failure("اسم الصندوق مطلوب"));

        if (_NameExists(r.Name, id))
            return Task.FromResult(Result<CashBoxDto>.Failure("اسم الصندوق مستخدم بالفعل"));

        var updated = existing with
        {
            Name = r.Name,
            Description = r.Description
        };

        var index = MockData.CashBoxes.IndexOf(existing);
        MockData.CashBoxes[index] = updated;
        return Task.FromResult(Result<CashBoxDto>.Success(updated));
    }

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.CashBoxes.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الصندوق النقدي غير موجود"));

        var index = MockData.CashBoxes.IndexOf(existing);
        MockData.CashBoxes[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.CashBoxes.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الصندوق النقدي غير موجود"));

        var index = MockData.CashBoxes.IndexOf(existing);
        MockData.CashBoxes[index] = existing with { IsActive = true };
        return Task.FromResult(Result.Success());
    }

    // التحويل بين الصناديق غير مدعوم في V1 — يُرفض بنفس نص الوضع الحقيقي.
    public Task<Result> TransferAsync(CashTransferRequest request, CancellationToken ct = default)
    {
        return Task.FromResult(Result.Failure(
            "التحويل بين الصناديق غير مدعوم حالياً — لا توجد آلية سندات أو قيود لتسجيل العملية"));
    }
}
