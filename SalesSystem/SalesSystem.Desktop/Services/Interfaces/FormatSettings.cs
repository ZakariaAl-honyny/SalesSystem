namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// إعدادات التنسيق كما تُحفظ وتُقرأ من الطبقة المكتبية (المرحلة أ — وضع التجربة).
/// <para>
/// المفاتيح هي نفس مفاتيح <c>SalesSystem.Desktop.GlobalClasses.clsFormat</c>، والقيم نصية
/// («2» · «true» · «.» · «dd/MM/yyyy»). السجل يعيش في مشروع Desktop عن قصد: المرحلة أ
/// ممنوعة من مسّ <c>SalesSystem.DTO</c>، وفي المرحلة ب يُسقَط على مفاتيح <c>SystemSettings</c>.
/// </para>
/// </summary>
public sealed record FormatSettings(IReadOnlyDictionary<string, string> Values);
