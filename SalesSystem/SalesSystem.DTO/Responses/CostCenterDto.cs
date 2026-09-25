using System.Text.Json.Serialization;

namespace SalesSystem.DTO.Responses;

/// <summary>
/// DTO لمركز التكلفة — يدعم شجرة مراكز التكلفة متعددة المستويات.
/// </summary>
public record CostCenterDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("nameEn")] string? NameEn,
    [property: JsonPropertyName("parentCode")] string? ParentCode,
    [property: JsonPropertyName("level")] byte Level,
    [property: JsonPropertyName("isLeaf")] bool IsLeaf,
    [property: JsonPropertyName("budget")] decimal Budget,
    [property: JsonPropertyName("managerName")] string? ManagerName,
    [property: JsonPropertyName("status")] byte Status,
    [property: JsonPropertyName("notes")] string? Notes
)
{
    /// <summary>عرض الحالة بالعربية.</summary>
    public string StatusDisplay => Status switch
    {
        1 => "نشط",
        2 => "معطل",
        _ => "غير معروف"
    };
};
