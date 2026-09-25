using System.Text.Json.Serialization;

namespace SalesSystem.DTO.Responses;

/// <summary>
/// DTO for a currency record.
/// معرّف بسيط بدون كيان EF — يُستخدم في وضع التجربة (Mock) أولاً.
/// </summary>
public record CurrencyDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("subUnit")] string? SubUnit,
    [property: JsonPropertyName("isLocal")] bool IsLocal
);
