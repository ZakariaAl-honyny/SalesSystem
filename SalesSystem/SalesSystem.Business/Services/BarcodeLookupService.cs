using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Services;

public class BarcodeLookupService : IBarcodeLookupService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<BarcodeLookupService> _logger;

    public BarcodeLookupService(IUnitOfWork uow, ILogger<BarcodeLookupService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<Result<BarcodeSearchResult>> LookupByBarcodeAsync(
        string barcode,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            return Result<BarcodeSearchResult>.Failure("الباركود مطلوب", ErrorCodes.ValidationError);

        // Look up by ProductUnit.Barcode (barcode is per-unit, not on product entity)
        var unit = await _uow.ProductUnits.FirstOrDefaultAsync(
            u => u.Barcode != null && u.Barcode == barcode.Trim(), ct, "Product", "Unit");

        if (unit == null)
        {
            _logger.LogWarning("Barcode not found: {Barcode}", barcode);
            return Result<BarcodeSearchResult>.Failure("الباركود غير موجود", ErrorCodes.NotFound);
        }

        _logger.LogInformation("Barcode resolved: {Barcode} → {ProductName} (ID: {ProductId}, ProductUnitId: {ProductUnitId})",
            barcode, unit.Product?.Name ?? "", unit.ProductId, unit.Id);

        return Result<BarcodeSearchResult>.Success(new BarcodeSearchResult(
            ProductId: unit.ProductId,
            ProductName: unit.Product?.Name ?? "",
            Barcode: unit.Barcode,
            ProductUnitId: unit.Id,
            UnitName: unit.Unit?.Name ?? "",
            ConversionFactor: unit.Factor,
            IsBaseUnit: unit.IsBaseUnit,
            SalesPrice: unit.SalePrice,
            CurrentStockInBaseUnits: 0m));
    }
}
