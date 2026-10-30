using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL document-sequence service (in-process, no HTTP).
/// </summary>
public class DirectDocumentSequenceService : IDocumentSequenceApiService
{
    private readonly BizInterfaces.IDocumentSequenceService _bll;

    public DirectDocumentSequenceService(BizInterfaces.IDocumentSequenceService bll)
    {
        _bll = bll;
    }

    public async Task<Result<IReadOnlyList<DocumentSequenceDto>>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllAsync(ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<DocumentSequenceDto>>.Success(
                    result.Value?.ToList() ?? new List<DocumentSequenceDto>())
                : Result<IReadOnlyList<DocumentSequenceDto>>.Failure(
                    result.Error ?? "تعذر تحميل متسلسلات المستندات.", result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectDocumentSequenceService.GetAllAsync failed");
            return Result<IReadOnlyList<DocumentSequenceDto>>.Failure(
                "حدث خطأ غير متوقع أثناء تحميل متسلسلات المستندات.");
        }
    }

    public async Task<Result> UpdateAsync(int id, UpdateDocumentSequenceRequest request, CancellationToken ct = default)
    {
        if (request is null)
            return Result.Failure("بيانات التسلسل مطلوبة.");

        try
        {
            var result = await _bll.UpdateSequenceAsync(id, request.NextNumber, ct);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error ?? "تعذر تحديث الرقم التالي للتسلسل.", result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectDocumentSequenceService.UpdateAsync failed for id {Id}", id);
            return Result.Failure("حدث خطأ غير متوقع أثناء تحديث التسلسل.");
        }
    }
}
