namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// ألوان النظام المختارة كما تُحفظ وتُقرأ من الطبقة المكتبية (المرحلة أ — وضع التجربة).
/// <para>
/// المفاتيح هي نفس مفاتيح <c>SalesSystem.Desktop.Helpers.ThemePalette</c> (23 لوناً)،
/// والقيم بصيغة <c>#RRGGBB</c>. السجل يعيش في مشروع Desktop عن قصد: المرحلة أ ممنوعة من
/// مسّ <c>SalesSystem.DTO</c>، وفي المرحلة ب يُسقَط على مفاتيح <c>SystemSettings</c>.
/// </para>
/// </summary>
public sealed record ThemeColorSettings(IReadOnlyDictionary<string, string> Colors);
