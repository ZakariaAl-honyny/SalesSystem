using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// عقد الشاشة المكتبية لإعدادات التنسيق (المرحلة أ: تنفيذه الوحيد <c>MockFormatSettingsDesktopService</c>).
/// كل الطرق تُعيد <see cref="Result"/> — لا استثناءات تعبر هذا العقد.
/// </summary>
public interface IFormatSettingsDesktopService
{
    /// <summary>قراءة إعدادات التنسيق الحالية (المفاتيح من تعريفات <c>clsFormat</c>).</summary>
    Task<Result<FormatSettings>> GetAsync(CancellationToken ct = default);

    /// <summary>حفظ مجموعة إعدادات كاملة بعد التحقق منها (كل المفاتيح أو لا شيء).</summary>
    Task<Result> SaveAsync(FormatSettings settings, CancellationToken ct = default);

    /// <summary>إرجاع كل إعدادات التنسيق إلى قيمها الافتراضية.</summary>
    Task<Result> ResetToDefaultsAsync(CancellationToken ct = default);
}
