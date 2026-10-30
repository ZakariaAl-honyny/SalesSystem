using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة القيود اليومية — تعيد بيانات من MockJournalEntriesData.
/// </summary>
public class MockJournalEntryApiService : IJournalEntryApiService
{
    public Task<Result<IReadOnlyList<JournalEntryListDto>>> GetAllAsync(CancellationToken ct = default)
    {
        IReadOnlyList<JournalEntryListDto> result = MockJournalEntriesData.Entries.ToList().AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<JournalEntryListDto>>.Success(result));
    }

    public Task<Result<JournalEntryDetailDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entry = MockJournalEntriesData.EntryDetails.FirstOrDefault(e => e.Id == id);
        return entry is null
            ? Task.FromResult(Result<JournalEntryDetailDto>.Failure("القيد المحاسبي غير موجود"))
            : Task.FromResult(Result<JournalEntryDetailDto>.Success(entry));
    }

    public Task<Result<int>> CreateAsync(CreateJournalEntryRequest request, CancellationToken ct = default)
    {
        // حساب المجاميع
        var totalDebit = request.Lines.Sum(l => l.Debit);
        var totalCredit = request.Lines.Sum(l => l.Credit);

        if (Math.Abs(totalDebit - totalCredit) > 0.001m)
            return Task.FromResult(Result<int>.Failure("القيد غير متوازن — مجموع المدين لا يساوي مجموع الدائن"));

        if (request.Lines.Count < 2)
            return Task.FromResult(Result<int>.Failure("يجب إضافة بندين على الأقل لقيد محاسبي مزدوج"));

        var nextId = MockJournalEntriesData.Entries.Count > 0
            ? MockJournalEntriesData.Entries.Max(e => e.Id) + 1
            : 1;

        var nextEntryNo = MockJournalEntriesData.Entries.Count > 0
            ? int.Parse(MockJournalEntriesData.Entries.Max(e => e.EntryNumber)) + 1
            : 101;

        var statusDisplay = "مسودة";

        // إنشاء بنود القيد
        var lineId = MockJournalEntriesData.EntryDetails
            .SelectMany(d => d.Lines)
            .DefaultIfEmpty()
            .Max(l => l?.Id ?? 0) + 1;

        var lineDtos = request.Lines.Select(l =>
        {
            var dto = new JournalEntryLineDetailDto(
                Id: lineId++,
                AccountId: l.AccountId,
                AccountName: null,
                AccountCode: null,
                Debit: l.Debit,
                Credit: l.Credit,
                Description: l.Description
            );
            return dto;
        }).ToList();

        // إنشاء القيد في قائمة العرض
        var listDto = new JournalEntryListDto(
            Id: nextId,
            EntryNumber: nextEntryNo.ToString(),
            EntryDate: request.EntryDate,
            Description: request.Description ?? string.Empty,
            EntryType: "يدوي",
            ReferenceType: request.ReferenceType,
            ReferenceId: request.ReferenceId,
            ReferenceNumber: request.ReferenceNumber,
            TotalDebit: totalDebit,
            TotalCredit: totalCredit,
            Status: 1,
            StatusDisplay: statusDisplay,
            CreatedAt: DateTime.Now,
            CreatedByUserId: 1
        );
        // TODO: SourceModule removed from JournalEntryListDto
        MockJournalEntriesData.Entries.Add(listDto);

        // إنشاء القيد في قائمة التفاصيل
        var detailDto = new JournalEntryDetailDto(
            Id: nextId,
            EntryNumber: nextEntryNo.ToString(),
            EntryDate: request.EntryDate,
            Description: request.Description ?? string.Empty,
            EntryType: "يدوي",
            ReferenceType: request.ReferenceType,
            ReferenceId: request.ReferenceId,
            ReferenceNumber: request.ReferenceNumber,
            Status: 1,
            StatusDisplay: statusDisplay,
            ReversedByEntryId: null,
            CreatedAt: DateTime.Now,
            CreatedByUserId: 1,
            Lines: lineDtos
        );
        // TODO: SourceModule removed from JournalEntryDetailDto
        MockJournalEntriesData.EntryDetails.Add(detailDto);

        return Task.FromResult(Result<int>.Success(nextId));
    }

    public Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        var listEntry = MockJournalEntriesData.Entries.FirstOrDefault(e => e.Id == id);
        var detailEntry = MockJournalEntriesData.EntryDetails.FirstOrDefault(e => e.Id == id);

        if (listEntry is null || detailEntry is null)
            return Task.FromResult(Result.Failure("القيد المحاسبي غير موجود"));

        if (listEntry.Status != 1)
            return Task.FromResult(Result.Failure("لا يمكن ترحيل إلا القيود في حالة مسودة"));

        // تحديث حالة القيد
        var listIndex = MockJournalEntriesData.Entries.IndexOf(listEntry);
        MockJournalEntriesData.Entries[listIndex] = listEntry with { Status = 2, StatusDisplay = "مرحل" };

        var detailIndex = MockJournalEntriesData.EntryDetails.IndexOf(detailEntry);
        MockJournalEntriesData.EntryDetails[detailIndex] = detailEntry with { Status = 2, StatusDisplay = "مرحل" };

        return Task.FromResult(Result.Success());
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        var listEntry = MockJournalEntriesData.Entries.FirstOrDefault(e => e.Id == id);
        var detailEntry = MockJournalEntriesData.EntryDetails.FirstOrDefault(e => e.Id == id);

        if (listEntry is null || detailEntry is null)
            return Task.FromResult(Result.Failure("القيد المحاسبي غير موجود"));

        if (listEntry.Status != 2)
            return Task.FromResult(Result.Failure("لا يمكن إلغاء إلا القيود المرحلة"));

        // تحديث حالة القيد
        var listIndex = MockJournalEntriesData.Entries.IndexOf(listEntry);
        MockJournalEntriesData.Entries[listIndex] = listEntry with { Status = 3, StatusDisplay = "ملغي" };

        var detailIndex = MockJournalEntriesData.EntryDetails.IndexOf(detailEntry);
        MockJournalEntriesData.EntryDetails[detailIndex] = detailEntry with { Status = 3, StatusDisplay = "ملغي" };

        return Task.FromResult(Result.Success());
    }
}
