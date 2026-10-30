using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة التحويلات المخزنية — تعيد بيانات من MockStockTransfersData.
/// </summary>
public class MockStockTransferApiService : IStockTransferApiService
{
    public Task<Result<IReadOnlyList<StockTransferDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        IEnumerable<StockTransferDto> query = MockStockTransfersData.Transfers;

        if (from.HasValue)
            query = query.Where(t => t.TransferDate >= from.Value);

        if (to.HasValue)
            query = query.Where(t => t.TransferDate <= to.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t =>
                t.TransferNo.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (t.SourceWarehouseName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (t.DestinationWarehouseName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (t.Notes ?? "").Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var result = query.OrderByDescending(t => t.TransferDate)
                          .ThenByDescending(t => t.Id)
                          .ToList()
                          .AsReadOnly();

        return Task.FromResult(Result<IReadOnlyList<StockTransferDto>>.Success(result));
    }

    public Task<Result<StockTransferDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var transfer = MockStockTransfersData.Transfers.FirstOrDefault(t => t.Id == id);
        return transfer is null
            ? Task.FromResult(Result<StockTransferDto>.Failure("التحويل غير موجود"))
            : Task.FromResult(Result<StockTransferDto>.Success(transfer));
    }

    public Task<Result<StockTransferDto>> CreateAsync(CreateStockTransferRequest r, CancellationToken ct = default)
    {
        var nextId = MockStockTransfersData.Transfers.Count > 0
            ? MockStockTransfersData.Transfers.Max(t => t.Id) + 1
            : 1;

        var nextTransferNo = MockStockTransfersData.Transfers.Count > 0
            ? MockStockTransfersData.Transfers.Max(t => int.TryParse(t.TransferNo, out var n) ? n : 0) + 1
            : 1;

        // Resolve warehouse names from shared mock data
        var srcWh = MockData.Warehouses.FirstOrDefault(w => w.Id == r.FromWarehouseId);
        var dstWh = MockData.Warehouses.FirstOrDefault(w => w.Id == r.ToWarehouseId);

        if (srcWh is null || dstWh is null)
            return Task.FromResult(Result<StockTransferDto>.Failure("المستودع المصدر أو الوجهة غير موجود"));

        if (r.FromWarehouseId == r.ToWarehouseId)
            return Task.FromResult(Result<StockTransferDto>.Failure("المستودع المصدر والوجهة يجب أن يكونا مختلفين"));

        if (r.Items is null || r.Items.Count == 0)
            return Task.FromResult(Result<StockTransferDto>.Failure("يجب إضافة صنف واحد على الأقل للتحويل"));

        // Build line items — ProductName/ProductCode from shared mock data
        // TODO: UnitCost / TotalCost / UnitName removed from StockTransferItemDto
        var items = r.Items.Select(item =>
        {
            var product = MockData.Products.FirstOrDefault(p => p.Id == item.ProductId);
            return new StockTransferItemDto(
                ProductId:   item.ProductId,
                ProductCode: product?.Barcode,
                ProductName: product?.Name ?? $"منتج {item.ProductId}",
                Quantity:    item.Quantity);
        }).ToList();

        // TODO: FromWarehouseName / ToWarehouseName / TotalCost / TotalQuantity / CreatedByUserId
        //       removed from StockTransferDto — warehouse names are resolved by the caller.
        var transfer = new StockTransferDto(
            Id:               nextId,
            TransferNo:       nextTransferNo.ToString(),
            TransferDate:     r.TransferDate,
            FromWarehouseId:  r.FromWarehouseId,
            ToWarehouseId:    r.ToWarehouseId,
            Notes:            r.Notes,
            Status:           1, // مسودة
            Items:            items);

        MockStockTransfersData.Transfers.Add(transfer);
        return Task.FromResult(Result<StockTransferDto>.Success(transfer));
    }

    public Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockStockTransfersData.Transfers.FirstOrDefault(t => t.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("التحويل غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result.Failure("فقط التحويلات المسودة يمكن ترحيلها"));

        var index = MockStockTransfersData.Transfers.IndexOf(existing);
        MockStockTransfersData.Transfers[index] = existing with
        {
            Status = 2
        };

        return Task.FromResult(Result.Success());
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockStockTransfersData.Transfers.FirstOrDefault(t => t.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("التحويل غير موجود"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("التحويل ملغي بالفعل"));

        var index = MockStockTransfersData.Transfers.IndexOf(existing);
        MockStockTransfersData.Transfers[index] = existing with
        {
            Status = 3
        };

        return Task.FromResult(Result.Success());
    }
}
