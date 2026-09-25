using System.Text.Json.Serialization;

namespace SalesSystem.DTO.Responses;

/// <summary>
/// DTO for a currency exchange-rate record.
/// يمثل سعر صرف العملة المحلية مقابل عملة أخرى بتاريخ محدّد.
/// </summary>
public record CurrencyRateDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("currencyId")] int CurrencyId,
    [property: JsonPropertyName("currencyName")] string CurrencyName,
    [property: JsonPropertyName("rate")] decimal Rate,
    [property: JsonPropertyName("fromDate")] DateTime FromDate,
    [property: JsonPropertyName("toDate")] DateTime? ToDate,
    [property: JsonPropertyName("isCurrent")] bool IsCurrent,
    [property: JsonPropertyName("minRate")] decimal? MinRate,
    [property: JsonPropertyName("maxRate")] decimal? MaxRate
);
