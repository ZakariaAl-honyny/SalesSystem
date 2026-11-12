using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة القيود اليومية — تعيد بيانات من MockJournalEntriesData.
/// </summary>
public class MockJournalEntryDesktopService : IJournalEntryDesktopService
{
    public Task<Result<IReadOnlyList<JournalEntryListDto>>> GetAllAsync(
        CancellationToken ct = default,
        JournalEntryType? entryType = null,
        byte? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? search = null)
    {
        IEnumerable<JournalEntryListDto> query = MockJournalEntriesData.Entries;

        if (entryType.HasValue)
            query = query.Where(e => e.EntryTypeValue == entryType.Value);

        if (status.HasValue)
            query = query.Where(e => e.Status == status.Value);

        if (fromDate.HasValue)
            query = query.Where(e => e.EntryDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(e => e.EntryDate <= toDate.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(e =>
                (e.EntryNumber is not null && e.EntryNumber.Contains(term, StringComparison.OrdinalIgnoreCase))
                || (e.Description is not null && e.Description.Contains(term, StringComparison.OrdinalIgnoreCase))
                || (e.ReferenceNumber is not null && e.ReferenceNumber.Contains(term, StringComparison.OrdinalIgnoreCase)));
        }

        IReadOnlyList<JournalEntryListDto> result = query
            .OrderByDescending(e => e.EntryDate)
            .ThenByDescending(e => e.Id)
            .ToList()
            .AsReadOnly();

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
        // التحقق من البنود
        if (request.Lines is null || request.Lines.Count == 0)
            return Task.FromResult(Result<int>.Failure("يجب إضافة بند واحد على الأقل"));

        if (request.Lines.Count < 2)
            return Task.FromResult(Result<int>.Failure("يجب إضافة بندين على الأقل لقيد محاسبي مزدوج"));

        // حساب المجاميع
        var totalDebit = request.Lines.Sum(l => l.Debit);
        var totalCredit = request.Lines.Sum(l => l.Credit);

        if (Math.Abs(totalDebit - totalCredit) > 0.001m)
            return Task.FromResult(Result<int>.Failure(
                $"القيد غير متوازن — مجموع المدين ({totalDebit:N2}) لا يساوي مجموع الدائن ({totalCredit:N2})"));

        foreach (var line in request.Lines)
        {
            if (line.Debit < 0 || line.Credit < 0)
                return Task.FromResult(Result<int>.Failure("لا يمكن أن تكون قيم الخصم أو الإيداع سالبة"));

            if (line.Debit == 0 && line.Credit == 0)
                return Task.FromResult(Result<int>.Failure("يجب أن يكون للبند قيمة خصم أو إيداع"));
        }

        var nextId = MockJournalEntriesData.Entries.Count > 0
            ? MockJournalEntriesData.Entries.Max(e => e.Id) + 1
            : 1;

        var nextEntryNo = MockJournalEntriesData.Entries.Count > 0
            ? MockJournalEntriesData.Entries.Max(e => int.Parse(e.EntryNumber)) + 1
            : 101;

        var statusDisplay = "مسودة";

        // التسمية العربية تُشتق من القيمة الرقمية — مصدر واحد للحقيقة
        var entryTypeLabel = JournalEntryTypeDisplay.GetArabicName(request.EntryType);

        // إنشاء بنود القيد
        var lineId = NextLineId();

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
            EntryType: entryTypeLabel,
            ReferenceType: request.ReferenceType,
            ReferenceId: request.ReferenceId,
            ReferenceNumber: request.ReferenceNumber,
            TotalDebit: totalDebit,
            TotalCredit: totalCredit,
            Status: 1,
            StatusDisplay: statusDisplay,
            CreatedAt: DateTime.Now,
            CreatedByUserId: 1,
            EntryTypeValue: request.EntryType
        );
        MockJournalEntriesData.Entries.Add(listDto);

        // إنشاء القيد في قائمة التفاصيل
        var detailDto = new JournalEntryDetailDto(
            Id: nextId,
            EntryNumber: nextEntryNo.ToString(),
            EntryDate: request.EntryDate,
            Description: request.Description ?? string.Empty,
            EntryType: entryTypeLabel,
            ReferenceType: request.ReferenceType,
            ReferenceId: request.ReferenceId,
            ReferenceNumber: request.ReferenceNumber,
            Status: 1,
            StatusDisplay: statusDisplay,
            ReversedByEntryId: null,
            CreatedAt: DateTime.Now,
            CreatedByUserId: 1,
            Lines: lineDtos,
            EntryTypeValue: request.EntryType
        );
        MockJournalEntriesData.EntryDetails.Add(detailDto);

        return Task.FromResult(Result<int>.Success(nextId));
    }

    public Task<Result<JournalEntryDetailDto>> UpdateAsync(int id, UpdateJournalEntryRequest request, CancellationToken ct = default)
    {
        var listEntry = MockJournalEntriesData.Entries.FirstOrDefault(e => e.Id == id);
        var detailEntry = MockJournalEntriesData.EntryDetails.FirstOrDefault(e => e.Id == id);

        if (listEntry is null || detailEntry is null)
            return Task.FromResult(Result<JournalEntryDetailDto>.Failure("القيد غير موجود"));

        if (listEntry.Status != 1)
            return Task.FromResult(Result<JournalEntryDetailDto>.Failure("لا يمكن تعديل قيد مُرحَّل أو ملغى"));

        if (request.Lines is null || request.Lines.Count == 0)
            return Task.FromResult(Result<JournalEntryDetailDto>.Failure("يجب إضافة بند واحد على الأقل"));

        if (request.Lines.Count < 2)
            return Task.FromResult(Result<JournalEntryDetailDto>.Failure("يجب إضافة بندين على الأقل لقيد محاسبي مزدوج"));

        var totalDebit = request.Lines.Sum(l => l.Debit);
        var totalCredit = request.Lines.Sum(l => l.Credit);

        if (Math.Abs(totalDebit - totalCredit) > 0.001m)
            return Task.FromResult(Result<JournalEntryDetailDto>.Failure(
                $"القيد غير متوازن — مجموع المدين ({totalDebit:N2}) لا يساوي مجموع الدائن ({totalCredit:N2})"));

        foreach (var line in request.Lines)
        {
            if (line.Debit < 0 || line.Credit < 0)
                return Task.FromResult(Result<JournalEntryDetailDto>.Failure("لا يمكن أن تكون قيم الخصم أو الإيداع سالبة"));

            if (line.Debit == 0 && line.Credit == 0)
                return Task.FromResult(Result<JournalEntryDetailDto>.Failure("يجب أن يكون للبند قيمة خصم أو إيداع"));
        }

        // التسمية العربية تُشتق من القيمة الرقمية — مصدر واحد للحقيقة
        var entryTypeLabel = JournalEntryTypeDisplay.GetArabicName(request.EntryType);

        // استبدال البنود القديمة ببنود جديدة
        var lineId = NextLineId(excludeEntryId: id);

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

        // تحديث رأس القيد في قائمة العرض
        var listIndex = MockJournalEntriesData.Entries.IndexOf(listEntry);
        MockJournalEntriesData.Entries[listIndex] = listEntry with
        {
            EntryDate = request.EntryDate,
            Description = request.Description ?? string.Empty,
            EntryType = entryTypeLabel,
            EntryTypeValue = request.EntryType,
            ReferenceType = request.ReferenceType,
            ReferenceId = request.ReferenceId,
            ReferenceNumber = request.ReferenceNumber,
            TotalDebit = totalDebit,
            TotalCredit = totalCredit
        };

        // تحديث رأس القيد مع البنود
        var updatedDetail = detailEntry with
        {
            EntryDate = request.EntryDate,
            Description = request.Description ?? string.Empty,
            EntryType = entryTypeLabel,
            EntryTypeValue = request.EntryType,
            ReferenceType = request.ReferenceType,
            ReferenceId = request.ReferenceId,
            ReferenceNumber = request.ReferenceNumber,
            Lines = lineDtos
        };

        var detailIndex = MockJournalEntriesData.EntryDetails.IndexOf(detailEntry);
        MockJournalEntriesData.EntryDetails[detailIndex] = updatedDetail;

        return Task.FromResult(Result<JournalEntryDetailDto>.Success(updatedDetail));
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

    public Task<Result> DeleteDraftAsync(int id, CancellationToken ct = default)
    {
        var listEntry = MockJournalEntriesData.Entries.FirstOrDefault(e => e.Id == id);
        var detailEntry = MockJournalEntriesData.EntryDetails.FirstOrDefault(e => e.Id == id);

        if (listEntry is null || detailEntry is null)
            return Task.FromResult(Result.Failure("القيد المحاسبي غير موجود"));

        if (listEntry.Status != 1)
            return Task.FromResult(Result.Failure("لا يمكن حذف قيد مُرحَّل أو ملغى"));

        MockJournalEntriesData.Entries.Remove(listEntry);
        MockJournalEntriesData.EntryDetails.Remove(detailEntry);

        return Task.FromResult(Result.Success());
    }

    // ─── مساعدات داخلية ───────────────────────────────

    /// <summary>أول معرّف بند متاح بعد أكبر معرّف قائم (مع استثناء اختياري لقيد يُستبدل).</summary>
    private static int NextLineId(int? excludeEntryId = null)
        => MockJournalEntriesData.EntryDetails
            .Where(e => e.Id != excludeEntryId)
            .SelectMany(e => e.Lines)
            .DefaultIfEmpty()
            .Max(l => l?.Id ?? 0) + 1;
}
