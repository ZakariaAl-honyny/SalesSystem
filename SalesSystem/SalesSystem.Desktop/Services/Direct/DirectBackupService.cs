using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL backup service (in-process, no HTTP).
/// </summary>
public class DirectBackupService : IBackupApiService
{
    private readonly BizInterfaces.IBackupService _bll;

    public DirectBackupService(BizInterfaces.IBackupService bll)
    {
        _bll = bll;
    }

    public async Task<Result<string>> CreateBackupAsync(CancellationToken ct = default)
    {
        try
        {
            // Null folder path lets the BLL use its configured default backup directory.
            return await _bll.CreateBackupAsync(null, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectBackupService.CreateBackupAsync failed");
            return Result<string>.Failure("حدث خطأ غير متوقع أثناء إنشاء النسخة الاحتياطية.");
        }
    }

    public async Task<Result<List<string>>> GetBackupListAsync(CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetBackupListAsync(ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectBackupService.GetBackupListAsync failed");
            return Result<List<string>>.Failure("حدث خطأ غير متوقع أثناء تحميل قائمة النسخ الاحتياطية.");
        }
    }

    public async Task<Result> RestoreBackupAsync(string fileName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Failure("يجب تحديد ملف النسخة الاحتياطية.");

        try
        {
            // BLL expects a full file path; the UI supplies the file name from the backup folder listing.
            return await _bll.RestoreBackupAsync(fileName, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectBackupService.RestoreBackupAsync failed for {FileName}", fileName);
            return Result.Failure("حدث خطأ غير متوقع أثناء استعادة النسخة الاحتياطية.");
        }
    }
}
