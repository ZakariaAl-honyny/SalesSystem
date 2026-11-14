using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// عقد الشاشة المكتبية لألوان النظام (المرحلة أ: تنفيذه الوحيد <c>MockThemeColorDesktopService</c>).
/// كل الطرق تُعيد <see cref="Result"/> — لا استثناءات تعبر هذا العقد.
/// </summary>
public interface IThemeColorDesktopService
{
    /// <summary>قراءة الألوان الحالية (23 مفتاحاً بصيغة <c>#RRGGBB</c>).</summary>
    Task<Result<ThemeColorSettings>> GetAsync(CancellationToken ct = default);

    /// <summary>حفظ مجموعة ألوان كاملة بعد التحقق منها (كل المفاتيح أو لا شيء).</summary>
    Task<Result> SaveAsync(ThemeColorSettings settings, CancellationToken ct = default);

    /// <summary>إرجاع كل الألوان إلى قيمها الافتراضية.</summary>
    Task<Result> ResetToDefaultsAsync(CancellationToken ct = default);
}
