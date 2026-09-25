using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Enums;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// ظ…ط­ط§ظƒط§ط© ط®ط¯ظ…ط© ظپظˆط§طھظٹط± ط§ظ„ظ…ط´طھط±ظٹط§طھ â€” طھط¯ط¹ظ… CRUD + طھط±ط­ظٹظ„ + ط¥ظ„ط؛ط§ط،.
/// </summary>
public class MockPurchaseInvoiceApiService : IPurchaseInvoiceApiService
{
    public Task<Result<IReadOnlyList<PurchaseInvoiceDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null,
        byte? status = null, CancellationToken ct = default)
    {
        IEnumerable<PurchaseInvoiceDto> query = MockPurchasesData.Purchases;

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        if (from.HasValue)
        {
            var fromDate = DateOnly.FromDateTime(from.Value);
            query = query.Where(i => i.InvoiceDate >= fromDate);
        }

        if (to.HasValue)
        {
            var toDate = DateOnly.FromDateTime(to.Value);
            query = query.Where(i => i.InvoiceDate <= toDate);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(i =>
                i.InvoiceNo.ToString().Contains(s) ||
                (i.SupplierName != null && i.SupplierName.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                (i.Notes != null && i.Notes.Contains(s, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult(Result<IReadOnlyList<PurchaseInvoiceDto>>.Success(
            query.OrderByDescending(i => i.InvoiceDate).ThenByDescending(i => i.InvoiceNo).ToList().AsReadOnly()));
    }

    public Task<Result<PurchaseInvoiceDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var invoice = MockPurchasesData.Purchases.FirstOrDefault(i => i.Id == id);
        return invoice is null
            ? Task.FromResult(Result<PurchaseInvoiceDto>.Failure("ظپط§طھظˆط±ط© ط§ظ„ظ…ط´طھط±ظٹط§طھ ط؛ظٹط± ظ…ظˆط¬ظˆط¯ط©"))
            : Task.FromResult(Result<PurchaseInvoiceDto>.Success(invoice));
    }

    public Task<Result<PurchaseInvoiceDto>> CreateAsync(CreatePurchaseInvoiceRequest r, CancellationToken ct = default)
    {
        var nextId = MockPurchasesData.Purchases.Count == 0 ? 1 : MockPurchasesData.Purchases.Max(i => i.Id) + 1;
        var nextInvoiceNo = MockPurchasesData.Purchases.Count == 0 ? 5001 : MockPurchasesData.Purchases.Max(i => i.InvoiceNo) + 1;
        var supplierName = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == r.SupplierId)?.Name
            ?? "ظ…ظˆط±ط¯ ط؛ظٹط± ظ…ط¹ط±ظˆظپ";
        var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)r.WarehouseId);

        var items = r.Items.Select((line, idx) =>
        {
            var productName = MockData.Products.FirstOrDefault(p => p.Id == line.ProductId)?.Name ?? "ظ…ظ†طھط¬ ط؛ظٹط± ظ…ط¹ط±ظˆظپ";
            decimal lineDiscount = line.DiscountType == DiscountType.Percentage && line.DiscountRate.HasValue
                ? line.Quantity * line.UnitPrice * line.DiscountRate.Value / 100m
                : line.DiscountRate ?? 0m;
            decimal lineTotal = (line.Quantity * line.UnitPrice) - lineDiscount;
            return new PurchaseInvoiceLineDto(
                Id: idx + 1, ProductId: line.ProductId, ProductName: productName,
                ProductUnitId: line.ProductUnitId, ProductUnitName: null,
                Quantity: line.Quantity, UnitPrice: line.UnitPrice, LineTotal: lineTotal,
                LandedUnitCost: line.UnitPrice,
                DiscountType: (byte)(line.DiscountType ?? 0), DiscountAmount: lineDiscount,
                CostInBaseCurrency: null, AdditionalFeesAmount: 0m);
        }).ToList();

        decimal subTotal = items.Sum(i => i.LineTotal);
        decimal netTotal = subTotal - r.DiscountAmount + r.TaxAmount + r.OtherCharges;
        decimal remaining = netTotal - r.PaidAmount;

        var invoice = new PurchaseInvoiceDto(
            Id: nextId, InvoiceNo: nextInvoiceNo,
            SupplierId: r.SupplierId, SupplierName: supplierName,
            WarehouseId: r.WarehouseId,
            WarehouseName: warehouse?.Name ?? "ظ…ط®ط²ظ† ط؛ظٹط± ظ…ط¹ط±ظˆظپ",
            InvoiceDate: DateOnly.FromDateTime(r.InvoiceDate ?? DateTime.Today),
            PaymentType: (byte)r.PaymentType,
            SubTotal: subTotal, DiscountAmount: r.DiscountAmount,
            DiscountType: (byte)(r.DiscountType ?? 0), CostInBaseCurrency: null,
            TaxAmount: r.TaxAmount, OtherCharges: r.OtherCharges,
            NetTotal: netTotal, PaidAmount: r.PaidAmount, RemainingAmount: remaining,
            Notes: r.Notes, SupplierInvoiceNo: null,
            AttachmentPath: r.AttachmentPath, Status: 1,
            TaxId: r.TaxId, TaxName: null, TaxRate: null,
            CashBoxId: null, Items: items);

        MockPurchasesData.Purchases.Add(invoice);
        return Task.FromResult(Result<PurchaseInvoiceDto>.Success(invoice));
    }

    public Task<Result<PurchaseInvoiceDto>> UpdateAsync(int id, CreatePurchaseInvoiceRequest r, CancellationToken ct = default)
    {
        var existing = MockPurchasesData.Purchases.FirstOrDefault(i => i.Id == id);
        if (existing is null)
            return Task.FromResult(Result<PurchaseInvoiceDto>.Failure("ظپط§طھظˆط±ط© ط§ظ„ظ…ط´طھط±ظٹط§طھ ط؛ظٹط± ظ…ظˆط¬ظˆط¯ط©"));

        if (existing.Status == 2)
            return Task.FromResult(Result<PurchaseInvoiceDto>.Failure("ظ„ط§ ظٹظ…ظƒظ† طھط¹ط¯ظٹظ„ ظپط§طھظˆط±ط© ظ…ط±ط­ظ„ط©"));
        if (existing.Status == 3)
            return Task.FromResult(Result<PurchaseInvoiceDto>.Failure("ظ„ط§ ظٹظ…ظƒظ† طھط¹ط¯ظٹظ„ ظپط§طھظˆط±ط© ظ…ظ„ط؛ظٹط©"));

        var supplierName = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == r.SupplierId)?.Name
            ?? existing.SupplierName;
        var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)r.WarehouseId);

        var items = r.Items.Select((line, idx) =>
        {
            var productName = MockData.Products.FirstOrDefault(p => p.Id == line.ProductId)?.Name ?? "ظ…ظ†طھط¬ ط؛ظٹط± ظ…ط¹ط±ظˆظپ";
            decimal lineDiscount = line.DiscountType == DiscountType.Percentage && line.DiscountRate.HasValue
                ? line.Quantity * line.UnitPrice * line.DiscountRate.Value / 100m
                : line.DiscountRate ?? 0m;
            decimal lineTotal = (line.Quantity * line.UnitPrice) - lineDiscount;
            return new PurchaseInvoiceLineDto(
                Id: idx + 1, ProductId: line.ProductId, ProductName: productName,
                ProductUnitId: line.ProductUnitId, ProductUnitName: null,
                Quantity: line.Quantity, UnitPrice: line.UnitPrice, LineTotal: lineTotal,
                LandedUnitCost: line.UnitPrice,
                DiscountType: (byte)(line.DiscountType ?? 0), DiscountAmount: lineDiscount,
                CostInBaseCurrency: null, AdditionalFeesAmount: 0m);
        }).ToList();

        decimal subTotal = items.Sum(i => i.LineTotal);
        decimal netTotal = subTotal - r.DiscountAmount + r.TaxAmount + r.OtherCharges;
        decimal remaining = netTotal - r.PaidAmount;

        var updated = existing with
        {
            SupplierId = r.SupplierId,
            SupplierName = supplierName,
            WarehouseId = r.WarehouseId,
            WarehouseName = warehouse?.Name ?? existing.WarehouseName,
            InvoiceDate = r.InvoiceDate.HasValue ? DateOnly.FromDateTime(r.InvoiceDate.Value) : existing.InvoiceDate,
            PaymentType = (byte)r.PaymentType,
            SubTotal = subTotal,
            DiscountAmount = r.DiscountAmount,
            DiscountType = (byte)(r.DiscountType ?? 0),
            TaxAmount = r.TaxAmount,
            OtherCharges = r.OtherCharges,
            NetTotal = netTotal,
            PaidAmount = r.PaidAmount,
            RemainingAmount = remaining,
            Notes = r.Notes,
            TaxId = r.TaxId,
            AttachmentPath = r.AttachmentPath,
            Items = items
        };

        var index = MockPurchasesData.Purchases.IndexOf(existing);
        MockPurchasesData.Purchases[index] = updated;
        return Task.FromResult(Result<PurchaseInvoiceDto>.Success(updated));
    }

    public Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockPurchasesData.Purchases.FirstOrDefault(i => i.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("ظپط§طھظˆط±ط© ط§ظ„ظ…ط´طھط±ظٹط§طھ ط؛ظٹط± ظ…ظˆط¬ظˆط¯ط©"));

        if (existing.Status == 2)
            return Task.FromResult(Result.Failure("ط§ظ„ظپط§طھظˆط±ط© ظ…ط±ط­ظ„ط© ط¨ط§ظ„ظپط¹ظ„"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("ظ„ط§ ظٹظ…ظƒظ† طھط±ط­ظٹظ„ ظپط§طھظˆط±ط© ظ…ظ„ط؛ظٹط©"));

        var index = MockPurchasesData.Purchases.IndexOf(existing);
        MockPurchasesData.Purchases[index] = existing with { Status = 2 };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockPurchasesData.Purchases.FirstOrDefault(i => i.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("ظپط§طھظˆط±ط© ط§ظ„ظ…ط´طھط±ظٹط§طھ ط؛ظٹط± ظ…ظˆط¬ظˆط¯ط©"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("ط§ظ„ظپط§طھظˆط±ط© ظ…ظ„ط؛ظٹط© ط¨ط§ظ„ظپط¹ظ„"));

        var index = MockPurchasesData.Purchases.IndexOf(existing);
        MockPurchasesData.Purchases[index] = existing with { Status = 3 };
        return Task.FromResult(Result.Success());
    }
}

