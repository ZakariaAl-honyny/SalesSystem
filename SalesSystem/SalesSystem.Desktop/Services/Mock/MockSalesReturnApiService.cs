using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة مرتجعات المبيعات — تعيد بيانات من MockReturnsData.
/// </summary>
public class MockSalesReturnApiService : ISalesReturnApiService
{
    public Task<Result<IReadOnlyList<SalesReturnDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null,
        int? customerId = null, CancellationToken ct = default)
    {
        IEnumerable<SalesReturnDto> query = MockReturnsData.SalesReturns;

        if (customerId.HasValue)
            query = query.Where(r => r.CustomerId == customerId.Value);

        if (from.HasValue)
            query = query.Where(r => r.ReturnDate >= from.Value);

        if (to.HasValue)
            query = query.Where(r => r.ReturnDate <= to.Value.Date.AddDays(1).AddTicks(-1));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(r =>
                r.ReturnNo.ToString().Contains(s) ||
                (r.CustomerName != null && r.CustomerName.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                (r.ReturnReason != null && r.ReturnReason.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                (r.Notes != null && r.Notes.Contains(s, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult(Result<IReadOnlyList<SalesReturnDto>>.Success(
            query.OrderByDescending(r => r.ReturnDate).ThenByDescending(r => r.ReturnNo).ToList().AsReadOnly()));
    }

    public Task<Result<SalesReturnDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var item = MockReturnsData.SalesReturns.FirstOrDefault(r => r.Id == id);
        return item is null
            ? Task.FromResult(Result<SalesReturnDto>.Failure("مرتجع المبيعات غير موجود"))
            : Task.FromResult(Result<SalesReturnDto>.Success(item));
    }

    public Task<Result<SalesReturnDto>> CreateAsync(CreateSalesReturnRequest r, CancellationToken ct = default)
    {
        var nextId = MockReturnsData.SalesReturns.Count == 0
            ? 1
            : MockReturnsData.SalesReturns.Max(x => x.Id) + 1;

        var nextReturnNo = MockReturnsData.SalesReturns.Count == 0
            ? 1001
            : MockReturnsData.SalesReturns.Max(x => x.ReturnNo) + 1;

        var customerName = r.CustomerId.HasValue
            ? MockData.Customers.FirstOrDefault(c => c.Id == r.CustomerId.Value)?.Name ?? "عميل نقدي"
            : "عميل نقدي";

        var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == r.WarehouseId);

        var items = (r.Items ?? new List<ReturnItemRequest>()).Select((line, idx) =>
        {
            var productName = MockData.Products.FirstOrDefault(p => p.Id == line.ProductId)?.Name ?? "منتج غير معروف";
            decimal lineTotal = (line.Quantity * line.UnitPrice) - line.DiscountAmount;
            return new SalesReturnItemDto(
                Id: idx + 1,
                ProductId: line.ProductId,
                ProductUnitId: line.ProductUnitId,
                ProductName: productName,
                Quantity: line.Quantity,
                UnitPrice: line.UnitPrice,
                DiscountAmount: line.DiscountAmount,
                LineTotal: lineTotal,
                Mode: line.Mode);
        }).ToList();

        decimal subTotal = items.Sum(i => i.LineTotal);

        var salesReturn = new SalesReturnDto(
            Id: nextId,
            ReturnNo: nextReturnNo,
            WarehouseId: r.WarehouseId,
            WarehouseName: warehouse?.Name ?? "مخزن غير معروف",
            CustomerId: r.CustomerId ?? 0,
            CustomerName: customerName,
            SalesInvoiceId: r.SalesInvoiceId,
            ReturnDate: r.ReturnDate ?? DateTime.Today,
            SubTotal: subTotal,
            TaxAmount: 0m,
            DiscountAmount: 0m,
            TotalAmount: subTotal,
            ReturnedDiscountAmount: 0m,
            ReturnedTaxAmount: 0m,
            ReturnedChargeAmount: 0m,
            TaxId: null,
            Notes: r.Notes,
            ReturnReason: r.ReturnReason,
            Status: 1,
            CashBoxId: null,
            CashBoxName: null,
            RefundAmount: 0m,
            Items: items.AsReadOnly(),
            BranchId: r.BranchId ?? 0);

        // TODO: Currency removed from SalesReturnDto — العملة تُحدد من إعدادات النظام.

        MockReturnsData.SalesReturns.Add(salesReturn);
        return Task.FromResult(Result<SalesReturnDto>.Success(salesReturn));
    }

    public Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockReturnsData.SalesReturns.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("مرتجع المبيعات غير موجود"));

        if (existing.Status == 2)
            return Task.FromResult(Result.Failure("المرتجع مرحّل بالفعل"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("لا يمكن ترحيل مرتجع ملغي"));

        var index = MockReturnsData.SalesReturns.IndexOf(existing);
        MockReturnsData.SalesReturns[index] = existing with { Status = 2, RefundAmount = existing.TotalAmount };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockReturnsData.SalesReturns.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("مرتجع المبيعات غير موجود"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("المرتجع ملغي بالفعل"));

        var index = MockReturnsData.SalesReturns.IndexOf(existing);
        MockReturnsData.SalesReturns[index] = existing with { Status = 3 };
        return Task.FromResult(Result.Success());
    }
}
