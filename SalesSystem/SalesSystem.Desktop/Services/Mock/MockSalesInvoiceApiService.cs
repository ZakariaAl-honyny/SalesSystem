using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Enums;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة فواتير البيع — تدعم CRUD + ترحيل + إلغاء.
/// </summary>
public class MockSalesInvoiceApiService : ISalesInvoiceApiService
{
    public Task<Result<IReadOnlyList<SalesInvoiceDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null,
        byte? status = null, CancellationToken ct = default)
    {
        IEnumerable<SalesInvoiceDto> query = MockData.Invoices;

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        if (from.HasValue)
            query = query.Where(i => i.InvoiceDate >= from.Value);

        if (to.HasValue)
            query = query.Where(i => i.InvoiceDate <= to.Value.Date.AddDays(1).AddTicks(-1));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(i =>
                i.InvoiceNo.ToString().Contains(s) ||
                (i.CustomerName != null && i.CustomerName.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                (i.Notes != null && i.Notes.Contains(s, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult(Result<IReadOnlyList<SalesInvoiceDto>>.Success(
            query.OrderByDescending(i => i.InvoiceDate).ThenByDescending(i => i.InvoiceNo).ToList().AsReadOnly()));
    }

    public Task<Result<SalesInvoiceDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var invoice = MockData.Invoices.FirstOrDefault(i => i.Id == id);
        return invoice is null
            ? Task.FromResult(Result<SalesInvoiceDto>.Failure("الفاتورة غير موجودة"))
            : Task.FromResult(Result<SalesInvoiceDto>.Success(invoice));
    }

    public Task<Result<SalesInvoiceDto>> CreateAsync(CreateSalesInvoiceRequest r, CancellationToken ct = default)
    {
        var nextId = MockData.Invoices.Count == 0 ? 1 : MockData.Invoices.Max(i => i.Id) + 1;
        var nextInvoiceNo = MockData.Invoices.Count == 0 ? 1001 : MockData.Invoices.Max(i => i.InvoiceNo) + 1;
        var customerName = MockData.Customers.FirstOrDefault(c => c.Id == r.CustomerId)?.Name
            ?? (r.CustomerId == 0 ? "عميل نقدي" : null);
        var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)r.WarehouseId);

        var items = r.Items.Select((line, idx) =>
        {
            var productName = MockData.Products.FirstOrDefault(p => p.Id == line.ProductId)?.Name ?? "منتج غير معروف";
            decimal lineDiscount = line.DiscountType == DiscountType.Percentage && line.DiscountRate.HasValue
                ? line.Quantity * line.UnitPrice * line.DiscountRate.Value / 100m
                : 0m;
            decimal lineTotal = (line.Quantity * line.UnitPrice) - lineDiscount;
            return new SalesInvoiceLineDto(
                Id: idx + 1, ProductId: line.ProductId, ProductName: productName,
                Quantity: line.Quantity, UnitPrice: line.UnitPrice, LineTotal: lineTotal,
                ProductUnitId: line.ProductUnitId,
                DiscountType: (byte)line.DiscountType, DiscountRate: line.DiscountRate,
                DiscountAmount: lineDiscount);
        }).ToList();

        decimal subTotal = items.Sum(i => i.LineTotal);
        decimal netTotal = subTotal - r.DiscountAmount + r.TaxAmount + r.OtherCharges;
        decimal remaining = netTotal - r.PaidAmount;

        var invoice = new SalesInvoiceDto(
            Id: nextId, InvoiceNo: nextInvoiceNo, CustomerId: r.CustomerId,
            CustomerName: customerName, WarehouseId: r.WarehouseId,
            WarehouseName: warehouse?.Name ?? "مخزن غير معروف",
            InvoiceDate: r.InvoiceDate ?? DateTime.Today,
            PaymentType: (byte)r.PaymentType, SubTotal: subTotal,
            DiscountAmount: r.DiscountAmount, DiscountType: (byte)r.DiscountType,
            DiscountRate: r.DiscountRate, TaxAmount: r.TaxAmount,
            OtherCharges: r.OtherCharges, NetTotal: netTotal,
            PaidAmount: r.PaidAmount, RemainingAmount: remaining,
            CostInBaseCurrency: null, Notes: r.Notes, Status: 1,
            TaxId: r.TaxId, TaxName: null, TaxRate: null,
            CashBoxId: r.CashBoxId, CashBoxName: null, Items: items,
            BranchId: r.BranchId ?? 0);

        MockData.Invoices.Add(invoice);
        return Task.FromResult(Result<SalesInvoiceDto>.Success(invoice));
    }

    public Task<Result<SalesInvoiceDto>> UpdateAsync(int id, CreateSalesInvoiceRequest r, CancellationToken ct = default)
    {
        var existing = MockData.Invoices.FirstOrDefault(i => i.Id == id);
        if (existing is null)
            return Task.FromResult(Result<SalesInvoiceDto>.Failure("الفاتورة غير موجودة"));

        if (existing.Status == 2)
            return Task.FromResult(Result<SalesInvoiceDto>.Failure("لا يمكن تعديل فاتورة مرحلة"));
        if (existing.Status == 3)
            return Task.FromResult(Result<SalesInvoiceDto>.Failure("لا يمكن تعديل فاتورة ملغية"));

        var customerName = MockData.Customers.FirstOrDefault(c => c.Id == r.CustomerId)?.Name
            ?? (r.CustomerId == 0 ? "عميل نقدي" : null);
        var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)r.WarehouseId);

        var items = r.Items.Select((line, idx) =>
        {
            var productName = MockData.Products.FirstOrDefault(p => p.Id == line.ProductId)?.Name ?? "منتج غير معروف";
            decimal lineDiscount = line.DiscountType == DiscountType.Percentage && line.DiscountRate.HasValue
                ? line.Quantity * line.UnitPrice * line.DiscountRate.Value / 100m
                : 0m;
            decimal lineTotal = (line.Quantity * line.UnitPrice) - lineDiscount;
            return new SalesInvoiceLineDto(
                Id: idx + 1, ProductId: line.ProductId, ProductName: productName,
                Quantity: line.Quantity, UnitPrice: line.UnitPrice, LineTotal: lineTotal,
                ProductUnitId: line.ProductUnitId,
                DiscountType: (byte)line.DiscountType, DiscountRate: line.DiscountRate,
                DiscountAmount: lineDiscount);
        }).ToList();

        decimal subTotal = items.Sum(i => i.LineTotal);
        decimal netTotal = subTotal - r.DiscountAmount + r.TaxAmount + r.OtherCharges;
        decimal remaining = netTotal - r.PaidAmount;

        var updated = existing with
        {
            CustomerId = r.CustomerId,
            CustomerName = customerName,
            WarehouseId = r.WarehouseId,
            WarehouseName = warehouse?.Name ?? existing.WarehouseName,
            InvoiceDate = r.InvoiceDate ?? existing.InvoiceDate,
            PaymentType = (byte)r.PaymentType,
            SubTotal = subTotal,
            DiscountAmount = r.DiscountAmount,
            DiscountType = (byte)r.DiscountType,
            DiscountRate = r.DiscountRate,
            TaxAmount = r.TaxAmount,
            OtherCharges = r.OtherCharges,
            NetTotal = netTotal,
            PaidAmount = r.PaidAmount,
            RemainingAmount = remaining,
            Notes = r.Notes,
            TaxId = r.TaxId,
            CashBoxId = r.CashBoxId,
            Items = items,
            BranchId = r.BranchId ?? existing.BranchId
        };

        var index = MockData.Invoices.IndexOf(existing);
        MockData.Invoices[index] = updated;
        return Task.FromResult(Result<SalesInvoiceDto>.Success(updated));
    }

    public Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.Invoices.FirstOrDefault(i => i.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الفاتورة غير موجودة"));

        if (existing.Status == 2)
            return Task.FromResult(Result.Failure("الفاتورة مرحلة بالفعل"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("لا يمكن ترحيل فاتورة ملغية"));

        var index = MockData.Invoices.IndexOf(existing);
        MockData.Invoices[index] = existing with { Status = 2 };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.Invoices.FirstOrDefault(i => i.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الفاتورة غير موجودة"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("الفاتورة ملغية بالفعل"));

        var index = MockData.Invoices.IndexOf(existing);
        MockData.Invoices[index] = existing with { Status = 3 };
        return Task.FromResult(Result.Success());
    }
}
