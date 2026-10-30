using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة القيود اليومية — ينادي طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectJournalEntryService : IJournalEntryApiService
{
    private readonly BizInterfaces.IJournalEntryService _bll;
    private readonly ISessionService _session;

    public DirectJournalEntryService(BizInterfaces.IJournalEntryService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<JournalEntryListDto>>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllAsync(page: 1, pageSize: int.MaxValue, ct: ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<JournalEntryListDto>>.Success(result.Value!)
                : Result<IReadOnlyList<JournalEntryListDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<JournalEntryListDto>>.Failure("تعذر تحميل القيود اليومية. حاول مرة أخرى.");
        }
    }

    public async Task<Result<JournalEntryDetailDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<JournalEntryDetailDto>.Failure("تعذر تحميل القيد اليومي. حاول مرة أخرى.");
        }
    }

    public async Task<Result<int>> CreateAsync(CreateJournalEntryRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateJournalEntryAsync(request, UserId, ct);
        }
        catch (Exception)
        {
            return Result<int>.Failure("تعذر إنشاء القيد اليومي. حاول مرة أخرى.");
        }
    }

    public async Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.PostJournalEntryAsync(id, UserId, ct);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر ترحيل القيد اليومي. حاول مرة أخرى.");
        }
    }

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.CancelJournalEntryAsync(id, UserId, ct);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر إلغاء القيد اليومي. حاول مرة أخرى.");
        }
    }
}
