using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO.Common;
using SalesSystem.Models;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Business.Services;

/// <summary>
/// Manages Bill of Materials (BOM) entries: component definitions, cost roll-up,
/// and circular reference validation.
///
/// This service modifies entities in the EF Core change tracker but does NOT
/// call SaveChangesAsync — the caller is responsible for persistence within
/// their own transaction scope (typically via ExecuteTransactionAsync).
/// </summary>
public class AssemblyService : IAssemblyService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<AssemblyService> _logger;

    public AssemblyService(IUnitOfWork uow, ILogger<AssemblyService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    // ─── Get Components ────────────────────────────────────────────────

    public async Task<Result<List<BillOfMaterialsDto>>> GetComponentsAsync(
        int finishedProductId, CancellationToken ct)
    {
        try
        {
            // Validate finished product exists
            var product = await _uow.Products.GetByIdAsync(finishedProductId, ct);
            if (product == null)
                return Result<List<BillOfMaterialsDto>>.Failure("المنتج النهائي غير موجود");

            var boms = await _uow.BillOfMaterials.ToListAsync(
                b => b.FinishedProductId == finishedProductId && b.IsActive,
                q => q.OrderBy(b => b.ComponentProduct != null ? b.ComponentProduct.Name : ""),
                ct: ct,
                includePaths: "ComponentProduct,Unit");

            var dtos = boms.Select(MapToDto).ToList();
            return Result<List<BillOfMaterialsDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving BOM components for FinishedProduct {ProductId}", finishedProductId);
            return Result<List<BillOfMaterialsDto>>.Failure("حدث خطأ أثناء استرجاع مكونات المنتج");
        }
    }

    // ─── Get Parent Assemblies ────────────────────────────────────────

    public async Task<Result<List<BillOfMaterialsDto>>> GetParentAssembliesAsync(
        int componentProductId, CancellationToken ct)
    {
        try
        {
            // Validate component product exists
            var product = await _uow.Products.GetByIdAsync(componentProductId, ct);
            if (product == null)
                return Result<List<BillOfMaterialsDto>>.Failure("المنتج المكون غير موجود");

            var boms = await _uow.BillOfMaterials.ToListAsync(
                b => b.ComponentProductId == componentProductId && b.IsActive,
                q => q.OrderBy(b => b.FinishedProduct != null ? b.FinishedProduct.Name : ""),
                ct: ct,
                includePaths: "FinishedProduct,Unit");

            var dtos = boms.Select(MapToDto).ToList();
            return Result<List<BillOfMaterialsDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving parent assemblies for ComponentProduct {ProductId}", componentProductId);
            return Result<List<BillOfMaterialsDto>>.Failure("حدث خطأ أثناء استرجاع التجميعات الأب");
        }
    }

    // ─── Add Component ────────────────────────────────────────────────

    public async Task<Result<BillOfMaterialsDto>> AddComponentAsync(
        CreateBillOfMaterialsRequest request, CancellationToken ct)
    {
        try
        {
            // Validate both products exist
            var finishedProduct = await _uow.Products.GetByIdAsync(request.FinishedProductId, ct);
            if (finishedProduct == null)
                return Result<BillOfMaterialsDto>.Failure("المنتج النهائي غير موجود");

            var componentProduct = await _uow.Products.GetByIdAsync(request.ComponentProductId, ct);
            if (componentProduct == null)
                return Result<BillOfMaterialsDto>.Failure("المنتج المكون غير موجود");

            // Check for duplicate BOM entry
            var existing = await _uow.BillOfMaterials.AnyAsync(
                b => b.FinishedProductId == request.FinishedProductId
                     && b.ComponentProductId == request.ComponentProductId
                     && b.IsActive,
                ct);
            if (existing)
                return Result<BillOfMaterialsDto>.Failure(
                    "هذا المكون مضاف بالفعل للمنتج النهائي");

            // Validate no circular reference
            var circularResult = await ValidateNoCircularReferenceAsync(
                request.FinishedProductId, request.ComponentProductId, ct);
            if (!circularResult.IsSuccess)
                return Result<BillOfMaterialsDto>.Failure(circularResult.Error!);

            if (circularResult.Value)
                return Result<BillOfMaterialsDto>.Failure(
                    "لا يمكن إضافة هذا المكون لأنه يؤدي إلى مرجع دائري (المنتج النهائي يستخدم " +
                    "هذا المكون والذي بدوره يستخدم المنتج النهائي كمكون له).");

            // Validate unit if provided
            if (request.UnitId.HasValue)
            {
                var unit = await _uow.Units.GetByIdAsync(request.UnitId.Value, ct);
                if (unit == null)
                    return Result<BillOfMaterialsDto>.Failure("الوحدة غير موجودة");
            }

            // Create the BOM entry
            var bom = BillOfMaterials.Create(
                request.FinishedProductId,
                request.ComponentProductId,
                request.QuantityRequired,
                request.UnitId,
                request.Notes);

            await _uow.BillOfMaterials.AddAsync(bom, ct);

            _logger.LogInformation(
                "BOM entry created: FinishedProduct {FinishedProductId} ← Component {ComponentProductId}, " +
                "Qty {Quantity}",
                request.FinishedProductId, request.ComponentProductId, request.QuantityRequired);

            // Re-fetch with includes for the DTO
            var savedBom = await _uow.BillOfMaterials.FirstOrDefaultAsync(
                b => b.Id == bom.Id,
                ct: ct,
                includePaths: "FinishedProduct,ComponentProduct,Unit");

            var dto = MapToDto(savedBom ?? bom);
            return Result<BillOfMaterialsDto>.Success(dto);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain rule violation adding BOM component");
            return Result<BillOfMaterialsDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding BOM component for FinishedProduct {ProductId}",
                request.FinishedProductId);
            return Result<BillOfMaterialsDto>.Failure("حدث خطأ أثناء إضافة المكون");
        }
    }

    // ─── Update Component Quantity ────────────────────────────────────

    public async Task<Result<BillOfMaterialsDto>> UpdateComponentQuantityAsync(
        int id, decimal newQuantity, CancellationToken ct)
    {
        try
        {
            var bom = await _uow.BillOfMaterials.FirstOrDefaultAsync(
                b => b.Id == id,
                ct: ct,
                includePaths: "FinishedProduct,ComponentProduct,Unit");

            if (bom == null)
                return Result<BillOfMaterialsDto>.Failure("المكون غير موجود");

            bom.UpdateQuantity(newQuantity);

            await _uow.BillOfMaterials.UpdateAsync(bom, ct);

            _logger.LogInformation(
                "BOM entry {BomId} quantity updated to {NewQuantity}",
                id, newQuantity);

            var dto = MapToDto(bom);
            return Result<BillOfMaterialsDto>.Success(dto);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain rule violation updating BOM quantity for entry {BomId}", id);
            return Result<BillOfMaterialsDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating BOM quantity for entry {BomId}", id);
            return Result<BillOfMaterialsDto>.Failure("حدث خطأ أثناء تحديث كمية المكون");
        }
    }

    // ─── Remove Component ─────────────────────────────────────────────

    public async Task<Result> RemoveComponentAsync(int id, CancellationToken ct)
    {
        try
        {
            var bom = await _uow.BillOfMaterials.GetByIdAsync(id, ct);
            if (bom == null)
                return Result.Failure("المكون غير موجود");

            bom.MarkAsDeleted();

            _logger.LogInformation(
                "BOM entry {BomId} removed (soft-delete): FinishedProduct {FinishedProductId} " +
                "← Component {ComponentProductId}",
                id, bom.FinishedProductId, bom.ComponentProductId);

            return Result.Success();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain rule violation removing BOM entry {BomId}", id);
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing BOM entry {BomId}", id);
            return Result.Failure("حدث خطأ أثناء حذف المكون");
        }
    }

    // ─── Calculate Total Cost ─────────────────────────────────────────

    public async Task<Result<decimal>> CalculateTotalCostAsync(
        int finishedProductId, short warehouseId, CancellationToken ct)
    {
        try
        {
            var product = await _uow.Products.GetByIdAsync(finishedProductId, ct);
            if (product == null)
                return Result<decimal>.Failure("المنتج النهائي غير موجود");

            var warehouse = await _uow.Warehouses.GetByIdAsync(warehouseId, ct);
            if (warehouse == null)
                return Result<decimal>.Failure("المستودع غير موجود");

            // Get all active components for this finished product
            var boms = await _uow.BillOfMaterials.ToListAsync(
                b => b.FinishedProductId == finishedProductId && b.IsActive,
                ct: ct);

            if (boms.Count == 0)
            {
                _logger.LogInformation(
                    "No BOM entries found for FinishedProduct {ProductId} — total cost is 0",
                    finishedProductId);
                return Result<decimal>.Success(0m);
            }

            decimal totalCost = 0m;

            foreach (var bom in boms)
            {
                // Get weighted average cost for this component from InventoryBatches
                var batches = await _uow.InventoryBatches.ToListAsync(
                    b => b.ProductId == bom.ComponentProductId
                         && b.WarehouseId == warehouseId
                         && b.QuantityRemaining > 0,
                    ct: ct);

                decimal unitCost = 0m;

                if (batches.Count > 0)
                {
                    var totalValue = batches.Sum(b => b.QuantityRemaining * b.UnitCost);
                    var totalQuantity = batches.Sum(b => b.QuantityRemaining);

                    if (totalQuantity > 0)
                        unitCost = Math.Round(totalValue / totalQuantity, 2);
                }

                var componentCost = unitCost * bom.QuantityRequired;
                totalCost += componentCost;

                _logger.LogInformation(
                    "BOM cost: Component {ComponentId} needs {Quantity} × UnitCost {UnitCost} = {LineCost}",
                    bom.ComponentProductId, bom.QuantityRequired, unitCost, componentCost);
            }

            totalCost = Math.Round(totalCost, 2);

            _logger.LogInformation(
                "Total BOM cost for FinishedProduct {ProductId}: {TotalCost}",
                finishedProductId, totalCost);

            return Result<decimal>.Success(totalCost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error calculating total BOM cost for FinishedProduct {ProductId}, Warehouse {WarehouseId}",
                finishedProductId, warehouseId);
            return Result<decimal>.Failure("حدث خطأ أثناء حساب التكلفة الإجمالية للمكونات");
        }
    }

    // ─── Validate Circular Reference ─────────────────────────────────

    public async Task<Result<bool>> ValidateNoCircularReferenceAsync(
        int finishedProductId, int componentProductId, CancellationToken ct)
    {
        try
        {
            // Direct circular reference check:
            // Does the component product itself have a BOM entry where it is the finished product
            // AND the original finished product is used as its component?
            // This catches A→B→A cycles directly.

            var hasCircular = await _uow.BillOfMaterials.AnyAsync(
                b => b.FinishedProductId == componentProductId
                     && b.ComponentProductId == finishedProductId
                     && b.IsActive,
                ct);

            _logger.LogInformation(
                "Circular reference check: FinishedProduct {FinishedProductId} " +
                "← Component {ComponentProductId} → IsCircular: {IsCircular}",
                finishedProductId, componentProductId, hasCircular);

            return Result<bool>.Success(hasCircular);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error validating circular reference for FinishedProduct {FinishedProductId}, " +
                "Component {ComponentProductId}",
                finishedProductId, componentProductId);
            return Result<bool>.Failure("حدث خطأ أثناء التحقق من المراجع الدائرية");
        }
    }

    // ─── Private Helpers ─────────────────────────────────────────────

    private static BillOfMaterialsDto MapToDto(BillOfMaterials bom)
    {
        return new BillOfMaterialsDto(
            Id: bom.Id,
            FinishedProductId: bom.FinishedProductId,
            FinishedProductName: bom.FinishedProduct?.Name,
            ComponentProductId: bom.ComponentProductId,
            ComponentProductName: bom.ComponentProduct?.Name,
            QuantityRequired: bom.QuantityRequired,
            UnitId: bom.UnitId,
            UnitName: bom.Unit?.Name,
            Notes: bom.Notes,
            IsActive: bom.IsActive
        );
    }
}
