using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة أوامر الشراء — تعيد بيانات من MockPurchaseOrdersData.
/// </summary>
public class MockPurchaseOrderApiService : IPurchaseOrderApiService
{
    public Task<Result<IReadOnlyList<PurchaseOrderDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var query = MockPurchaseOrdersData.Orders.AsEnumerable();

        // الحالة 5 = ملغي — تعتبر "غير نشط" لأوامر الشراء
        if (!includeInactive)
            query = query.Where(o => o.Status != 5);

        return Task.FromResult(
            Result<IReadOnlyList<PurchaseOrderDto>>.Success(query.OrderByDescending(o => o.OrderDate).ToList().AsReadOnly()));
    }

    public Task<Result<PurchaseOrderDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var order = MockPurchaseOrdersData.Orders.FirstOrDefault(o => o.Id == id);
        return order is null
            ? Task.FromResult(Result<PurchaseOrderDto>.Failure("أمر الشراء غير موجود"))
            : Task.FromResult(Result<PurchaseOrderDto>.Success(order));
    }

    public Task<Result<PurchaseOrderDto>> CreateAsync(PurchaseOrderDto request, CancellationToken ct = default)
    {
        var nextId = MockPurchaseOrdersData.Orders.Count == 0 ? 1 : MockPurchaseOrdersData.Orders.Max(o => o.Id) + 1;
        var nextNo = MockPurchaseOrdersData.Orders.Count == 0 ? 8001 : MockPurchaseOrdersData.Orders.Max(o => o.OrderNo) + 1;

        var order = request with
        {
            Id = nextId,
            OrderNo = nextNo,
            Status = 1 // مسودة
        };

        MockPurchaseOrdersData.Orders.Add(order);
        return Task.FromResult(Result<PurchaseOrderDto>.Success(order));
    }

    public Task<Result<PurchaseOrderDto>> UpdateAsync(int id, PurchaseOrderDto request, CancellationToken ct = default)
    {
        var existing = MockPurchaseOrdersData.Orders.FirstOrDefault(o => o.Id == id);
        if (existing is null)
            return Task.FromResult(Result<PurchaseOrderDto>.Failure("أمر الشراء غير موجود"));

        if (existing.Status == 3)
            return Task.FromResult(Result<PurchaseOrderDto>.Failure("لا يمكن تعديل أمر شراء مستلم"));

        if (existing.Status == 4)
            return Task.FromResult(Result<PurchaseOrderDto>.Failure("لا يمكن تعديل أمر شراء مُحوَّل"));

        var updated = request with { Id = id, OrderNo = existing.OrderNo };
        var index = MockPurchaseOrdersData.Orders.IndexOf(existing);
        MockPurchaseOrdersData.Orders[index] = updated;
        return Task.FromResult(Result<PurchaseOrderDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockPurchaseOrdersData.Orders.FirstOrDefault(o => o.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("أمر الشراء غير موجود"));

        if (existing.Status == 3 || existing.Status == 4)
            return Task.FromResult(Result.Failure("لا يمكن حذف أمر شراء مستلم أو مُحوَّل"));

        // حذف ناعم: تغيير الحالة إلى ملغي (5)
        var index = MockPurchaseOrdersData.Orders.IndexOf(existing);
        MockPurchaseOrdersData.Orders[index] = existing with { Status = 5 };
        return Task.FromResult(Result.Success());
    }
}
