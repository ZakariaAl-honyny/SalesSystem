using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Common.Enums;

namespace SalesSystem.Business.Interfaces.Services;

public interface IProductUnitService
{
    Task<Result<List<ProductUnitDto>>> GetByProductIdAsync(int productId, CancellationToken ct);
    Task<Result<ProductUnitDto>> AddUnitAsync(int productId, AddProductUnitRequest req, CancellationToken ct);
    Task<Result<ProductUnitDto>> UpdateUnitAsync(int productId, int unitId, UpdateProductUnitRequest req, CancellationToken ct);
    Task<Result> DeleteUnitAsync(int productId, int unitId, DeleteStrategy strategy, CancellationToken ct);
    Task<Result<BarcodeResolutionDto>> ResolveBarcodeAsync(string barcode, CancellationToken ct);
}
