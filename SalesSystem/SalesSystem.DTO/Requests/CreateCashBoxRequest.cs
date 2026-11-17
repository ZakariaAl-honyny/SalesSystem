using System.Text.Json.Serialization;

namespace SalesSystem.DTO.Requests;

/// <summary>
/// Request to create a new cash box.
/// </summary>
public record CreateCashBoxRequest(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")] string? Description = null,
    [property: JsonPropertyName("openingBalance")] decimal OpeningBalance = 0m,
    [property: JsonPropertyName("sourceCashBoxId")] int? SourceCashBoxId = null
);
