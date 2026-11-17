namespace SalesSystem.DTO.Responses;

/// <summary>
/// Result of the effective price lookup with fallback chain information.
/// </summary>
public record EffectivePriceDto(
    int ProductUnitId,
    decimal Price,
    decimal? OriginalPrice,
    string? FallbackDescription);
