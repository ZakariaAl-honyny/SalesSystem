using System.Text.Json.Serialization;

namespace SalesSystem.DTO.Responses;

/// <summary>
/// DTO لبطاقة الموظف — بيانات العرض في واجهة سطح المكتب.
/// </summary>
public record EmployeeDto(
    int Id,
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("jobTitle")] string? JobTitle,
    [property: JsonPropertyName("phone")] string? Phone,
    [property: JsonPropertyName("nationalId")] string? NationalId,
    [property: JsonPropertyName("basicSalary")] decimal BasicSalary,
    [property: JsonPropertyName("hireDate")] DateTime? HireDate,
    [property: JsonPropertyName("isActive")] bool IsActive,
    [property: JsonPropertyName("notes")] string? Notes,
    [property: JsonPropertyName("accountCode")] string? AccountCode,
    [property: JsonPropertyName("linkedUserId")] int? LinkedUserId,
    [property: JsonPropertyName("commissionRate")] decimal? CommissionRate
);
