using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Interfaces.Services;

public interface IBarcodeLookupService
{
    Task<Result<BarcodeSearchResult>> LookupByBarcodeAsync(string barcode, CancellationToken ct = default);
}

public record BarcodeSearchResult(
    int ProductId,
    string ProductName,
    string? Barcode,
    int ProductUnitId,
    string UnitName,
    decimal ConversionFactor,
    bool IsBaseUnit,
    decimal SalesPrice,
    decimal CurrentStockInBaseUnits
)
{
    // Phase 25: SalesPrice is now sourced from ProductUnit.SalePrice.
    // SalesPrice comes from ProductUnit.SalePrice (per-unit pricing on ProductUnits).
}