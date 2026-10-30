using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة إعدادات المتجر — ينادي طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectSettingsService : ISettingsApiService
{
    private readonly BizInterfaces.IStoreSettingsService _bll;
    private readonly ISessionService _session;

    public DirectSettingsService(BizInterfaces.IStoreSettingsService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<dynamic>> GetSettingsAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetSettingsAsync(ct);
            return result.IsSuccess
                ? Result<dynamic>.Success(result.Value!)
                : Result<dynamic>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<dynamic>.Failure("تعذر تحميل إعدادات المتجر. حاول مرة أخرى.");
        }
    }

    public async Task<Result> UpdateSettingsAsync(object settings, CancellationToken ct = default)
    {
        try
        {
            var request = ToUpdateRequest(settings);
            if (request is null)
                return Result.Failure("بيانات الإعدادات غير صالحة.");

            return await _bll.UpdateSettingsAsync(request, UserId, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر تحديث إعدادات المتجر. حاول مرة أخرى.");
        }
    }

    public async Task<Result<Dictionary<string, string>>> GetAllSystemSettingsAsync(CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetAllSystemSettingsAsync(ct);
        }
        catch (Exception)
        {
            return Result<Dictionary<string, string>>.Failure("تعذر تحميل إعدادات النظام. حاول مرة أخرى.");
        }
    }

    public async Task<Result> UpdateSystemSettingsAsync(Dictionary<string, string> settings, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateSystemSettingsAsync(settings, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر تحديث إعدادات النظام. حاول مرة أخرى.");
        }
    }

    public void RefreshCache()
    {
        // TODO: تحديث ذاكرة الواجهة المؤقتة بعد تغيير الإعدادات — لا يوجد مقابل في طبقة الأعمال
        // (خدمة الأعمال تقرأ الإعدادات من قاعدة البيانات مباشرة، فلا حاجة لعملية إضافية هنا).
    }

    // ─── Mapping: object → UpdateSettingsRequest ──────────────────────────

    private static UpdateSettingsRequest? ToUpdateRequest(object? settings) => settings switch
    {
        null => null,
        UpdateSettingsRequest request => request,
        StoreSettingsDto dto => new UpdateSettingsRequest(
            StoreName: dto.StoreName,
            Address: dto.Address,
            Phone: dto.Phone,
            Email: dto.Email,
            LogoUrl: dto.LogoPath,
            DefaultTaxRate: dto.DefaultTaxRate,
            IsTaxEnabled: dto.IsTaxEnabled,
            TaxNumber: dto.TaxNumber,
            EnableStockAlerts: dto.EnableStockAlerts,
            AllowNegativeStock: dto.AllowNegativeStock,
            InvoicePrefix: dto.InvoicePrefix,
            BackupPath: dto.BackupPath,
            BackupScheduleTime: dto.BackupScheduleTime,
            BackupRetentionDays: dto.BackupRetentionDays,
            UpdateServerUrl: dto.UpdateServerUrl,
            SignatureUrl: dto.SignaturePath),
        _ => null
    };
}
