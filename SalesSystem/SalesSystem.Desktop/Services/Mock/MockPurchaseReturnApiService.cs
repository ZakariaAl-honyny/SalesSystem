using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Enums;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة مرتجعات المشتريات — تعيد بيانات من MockReturnsData.
/// </summary>
public class MockPurchaseReturnApiService : IPurchaseReturnApiService
{
    public Task<Result<IReadOnlyList<PurchaseReturnDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null,
        int? supplierId = null, CancellationToken ct = default)
    {
        IEnumerable<PurchaseReturnDto> query = MockReturnsData.PurchaseReturns;

        if (supplierId.HasValue)
            query = query.Where(r => r.SupplierId == supplierId.Value);

        if (from.HasValue)
            query = query.Where(r => r.ReturnDate >= DateOnly.FromDateTime(from.Value));

        if (to.HasValue)
            query = query.Where(r => r.ReturnDate <= DateOnly.FromDateTime(to.Value));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(r =>
                r.ReturnNo.ToString().Contains(s) ||
                (r.SupplierName != null && r.SupplierName.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                (r.Notes != null && r.Notes.Contains(s, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult(Result<IReadOnlyList<PurchaseReturnDto>>.Success(
            query.OrderByDescending(r => r.ReturnDate).ThenByDescending(r => r.ReturnNo).ToList().AsReadOnly()));
    }

    public Task<Result<PurchaseReturnDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var item = MockReturnsData.PurchaseReturns.FirstOrDefault(r => r.Id == id);
        return item is null
            ? Task.FromResult(Result<PurchaseReturnDto>.Failure("مرتجع المشتريات غير موجود"))
            : Task.FromResult(Result<PurchaseReturnDto>.Success(item));
    }

    public Task<Result<PurchaseReturnDto>> CreateAsync(CreatePurchaseReturnRequest r, CancellationToken ct = default)
    {
        var nextId = MockReturnsData.PurchaseReturns.Count == 0
            ? 1
            : MockReturnsData.PurchaseReturns.Max(x => x.Id) + 1;

        var nextReturnNo = MockReturnsData.PurchaseReturns.Count == 0
            ? 2001
            : MockReturnsData.PurchaseReturns.Max(x => x.ReturnNo) + 1;

        var supplierName = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == r.SupplierId)?.Name
            ?? "مورد غير معروف";

        var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)r.WarehouseId);

        var items = r.Items.Select((line, idx) =>
        {
            var productName = MockData.Products.FirstOrDefault(p => p.Id == line.ProductId)?.Name ?? "منتج غير معروف";
            return new PurchaseReturnItemDto(
                Id: nextId * 100 + idx + 1,
                ProductId: line.ProductId,
                ProductName: productName,
                ProductUnitId: line.ProductUnitId,
                ProductUnitName: null,
                Quantity: line.Quantity,
                UnitCost: line.UnitCost,
                LineTotal: line.Amount,
                PurchaseInvoiceLineId: line.PurchaseInvoiceLineId > 0 ? line.PurchaseInvoiceLineId : null,
                CostInBaseCurrency: null);
        }).ToList();

        decimal totalAmount = items.Sum(i => i.LineTotal);
        DateOnly returnDate = r.ReturnDate.HasValue
            ? DateOnly.FromDateTime(r.ReturnDate.Value)
            : DateOnly.FromDateTime(DateTime.Today);

        var purchaseReturn = new PurchaseReturnDto(
            Id: nextId,
            ReturnNo: nextReturnNo,
            WarehouseId: r.WarehouseId,
            WarehouseName: warehouse?.Name ?? "مخزن غير معروف",
            SupplierId: r.SupplierId,
            SupplierName: supplierName,
            PurchaseInvoiceId: r.PurchaseInvoiceId,
            LinkToInvoice: r.PurchaseInvoiceId.HasValue,
            ReturnDate: returnDate,
            SubTotal: totalAmount,
            TotalAmount: totalAmount,
            ReturnedDiscountAmount: 0m,
            ReturnedTaxAmount: 0m,
            ReturnedChargeAmount: 0m,
            TaxId: null,
            DiscountType: (byte)(r.DiscountType ?? DiscountType.Amount),
            DiscountRate: r.DiscountRate,
            Notes: r.Notes,
            Status: 1,
            Items: items.AsReadOnly(),
            BranchId: r.BranchId ?? 0);

        // TODO: Currency removed from PurchaseReturnDto — العملة تُحدد من إعدادات النظام.

        MockReturnsData.PurchaseReturns.Add(purchaseReturn);
        return Task.FromResult(Result<PurchaseReturnDto>.Success(purchaseReturn));
    }

    public Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockReturnsData.PurchaseReturns.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("مرتجع المشتريات غير موجود"));

        if (existing.Status == 2)
            return Task.FromResult(Result.Failure("المرتجع مرحّل بالفعل"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("لا يمكن ترحيل مرتجع ملغي"));

        var index = MockReturnsData.PurchaseReturns.IndexOf(existing);
        MockReturnsData.PurchaseReturns[index] = existing with { Status = 2 };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockReturnsData.PurchaseReturns.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("مرتجع المشتريات غير موجود"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("المرتجع ملغي بالفعل"));

        var index = MockReturnsData.PurchaseReturns.IndexOf(existing);
        MockReturnsData.PurchaseReturns[index] = existing with { Status = 3 };
        return Task.FromResult(Result.Success());
    }
}

