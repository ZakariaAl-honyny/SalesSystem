using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة سندات صرف الموردين — تعيد بيانات من MockSupplierPaymentsData.
/// </summary>
public class MockSupplierPaymentApiService : ISupplierPaymentApiService
{
    public Task<Result<IReadOnlyList<SupplierPaymentDto>>> GetAllAsync(int? supplierId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        IEnumerable<SupplierPaymentDto> query = MockSupplierPaymentsData.Payments;

        if (supplierId.HasValue)
            query = query.Where(p => p.SupplierId == supplierId.Value);

        if (from.HasValue)
            query = query.Where(p => p.PaymentDate >= DateOnly.FromDateTime(from.Value));

        if (to.HasValue)
            query = query.Where(p => p.PaymentDate <= DateOnly.FromDateTime(to.Value));

        return Task.FromResult(Result<IReadOnlyList<SupplierPaymentDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<SupplierPaymentDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var payment = MockSupplierPaymentsData.Payments.FirstOrDefault(p => p.Id == id);
        return payment is null
            ? Task.FromResult(Result<SupplierPaymentDto>.Failure("سند الصرف غير موجود"))
            : Task.FromResult(Result<SupplierPaymentDto>.Success(payment));
    }

    public Task<Result<SupplierPaymentDto>> CreateAsync(CreateSupplierPaymentRequest r, CancellationToken ct = default)
    {
        var nextId = MockSupplierPaymentsData.Payments.Count > 0
            ? MockSupplierPaymentsData.Payments.Max(p => p.Id) + 1 : 1;
        var nextNo = MockSupplierPaymentsData.Payments.Count > 0
            ? MockSupplierPaymentsData.Payments.Max(p => int.Parse(p.PaymentNo)) + 1 : 2001;

        // Resolve supplier name from mock data
        var supplier = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == r.SupplierId);
        var supplierName = supplier?.Name ?? "غير معروف";

        // TODO: CashBoxId / CashBoxName / Status / Currency / CreatedAt / PostedAt / CancelledAt
        //       removed from SupplierPaymentDto — السند لا يحمل صندوقاً أو حالة في هذا الإصدار.
        var payment = new SupplierPaymentDto(
            Id: nextId,
            PaymentNo: nextNo.ToString(),
            SupplierId: r.SupplierId,
            SupplierName: supplierName,
            Amount: r.Amount,
            PaymentMethod: (byte)r.PaymentMethod,
            PaymentDate: r.PaymentDate.HasValue
                ? DateOnly.FromDateTime(r.PaymentDate.Value)
                : DateOnly.FromDateTime(DateTime.UtcNow),
            PurchaseInvoiceId: r.PurchaseInvoiceId,
            Notes: r.Notes,
            BranchId: r.BranchId ?? 1);

        MockSupplierPaymentsData.Payments.Add(payment);
        return Task.FromResult(Result<SupplierPaymentDto>.Success(payment));
    }

    public Task<Result<SupplierPaymentDto>> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockSupplierPaymentsData.Payments.FirstOrDefault(p => p.Id == id);
        if (existing is null)
            return Task.FromResult(Result<SupplierPaymentDto>.Failure("سند الصرف غير موجود"));

        // TODO: Status / PostedAt removed from SupplierPaymentDto — لا يمكن تعقّب حالة الترحيل في البيانات التجريبية.
        return Task.FromResult(Result<SupplierPaymentDto>.Success(existing));
    }

    public Task<Result<SupplierPaymentDto>> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockSupplierPaymentsData.Payments.FirstOrDefault(p => p.Id == id);
        if (existing is null)
            return Task.FromResult(Result<SupplierPaymentDto>.Failure("سند الصرف غير موجود"));

        // TODO: Status / CancelledAt removed from SupplierPaymentDto — الإلغاء يُنفذ بحذف السند من البيانات التجريبية.
        var index = MockSupplierPaymentsData.Payments.IndexOf(existing);
        MockSupplierPaymentsData.Payments.RemoveAt(index);
        return Task.FromResult(Result<SupplierPaymentDto>.Success(existing));
    }
}
