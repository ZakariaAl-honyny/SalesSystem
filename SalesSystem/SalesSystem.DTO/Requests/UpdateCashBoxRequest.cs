using System.Text.Json.Serialization;

namespace SalesSystem.DTO.Requests;

/// <summary>
/// Request to update an existing cash box.
/// </summary>
public record UpdateCashBoxRequest(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")] string? Description = null
);
