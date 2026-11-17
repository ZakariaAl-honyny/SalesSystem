using System.Text.Json.Serialization;

namespace SalesSystem.DTO.Responses;

/// <summary>
/// DTO for a cash box.
/// Schema: CashBoxes — Id, Name, Description (nullable).
/// </summary>
public record CashBoxDto(
    int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")] string? Description,
    [property: JsonPropertyName("isActive")] bool IsActive
);
