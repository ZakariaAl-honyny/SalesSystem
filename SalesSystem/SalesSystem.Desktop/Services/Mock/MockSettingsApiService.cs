using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة الإعدادات — تعيد إعدادات افتراضية صالحة.
/// </summary>
public class MockSettingsApiService : ISettingsApiService
{
    public Task<Result<dynamic>> GetSettingsAsync(CancellationToken ct = default)
    {
        dynamic settings = new System.Dynamic.ExpandoObject();
        settings.storeName = "محل التجريبي";
        settings.phone = "01-234567";
        settings.address = "شارع الستين - صنعاء";
        settings.logoPath = (string?)null;
        settings.email = (string?)null;
        settings.defaultTaxRate = 5m;
        settings.isTaxEnabled = true;
        settings.taxNumber = "1234567890";
        settings.enableStockAlerts = true;
        settings.allowNegativeStock = false;
        settings.invoicePrefix = "INV";
        settings.backupPath = (string?)null;
        settings.backupScheduleTime = "02:00";
        settings.backupRetentionDays = 30;
        settings.updateServerUrl = (string?)null;
        settings.signaturePath = (string?)null;

        return Task.FromResult(Result<dynamic>.Success(settings));
    }

    public Task<Result> UpdateSettingsAsync(object settings, CancellationToken ct = default)
    {
        return Task.FromResult(Result.Success());
    }

    public Task<Result<Dictionary<string, string>>> GetAllSystemSettingsAsync(CancellationToken ct = default)
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["StoreName"] = "محل التجريبي",
            ["Phone"] = "01-234567",
            ["Address"] = "شارع الستين - صنعاء",
            ["DefaultTaxRate"] = "5",
            ["IsTaxEnabled"] = "true",
            ["TaxNumber"] = "1234567890",
            ["EnableStockAlerts"] = "true",
            ["AllowNegativeStock"] = "false",
            ["InvoicePrefix"] = "INV"
        };
        return Task.FromResult(Result<Dictionary<string, string>>.Success(dict));
    }

    public Task<Result> UpdateSystemSettingsAsync(Dictionary<string, string> settings, CancellationToken ct = default)
    {
        return Task.FromResult(Result.Success());
    }

    public void RefreshCache()
    {
        // محاكاة — لا حاجة لفعل شيء
    }
}
