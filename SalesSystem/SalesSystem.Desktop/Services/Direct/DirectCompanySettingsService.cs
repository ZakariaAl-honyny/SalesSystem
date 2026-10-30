using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL company-settings service (in-process, no HTTP).
/// </summary>
public class DirectCompanySettingsService : ICompanySettingsApiService
{
    private readonly BizInterfaces.ICompanySettingsService _bll;
    private readonly ISessionService _session;

    public DirectCompanySettingsService(BizInterfaces.ICompanySettingsService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<CompanySettingsDto>> GetAsync(CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetAsync(ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCompanySettingsService.GetAsync failed");
            return Result<CompanySettingsDto>.Failure("حدث خطأ غير متوقع أثناء تحميل إعدادات الشركة.");
        }
    }

    public async Task<Result<CompanySettingsDto>> UpdateAsync(UpdateCompanySettingsRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCompanySettingsService.UpdateAsync failed");
            return Result<CompanySettingsDto>.Failure("حدث خطأ غير متوقع أثناء تحديث إعدادات الشركة.");
        }
    }
}
