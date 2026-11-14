using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة سندات قبض العملاء — تعيد بيانات من MockCustomerReceiptsData.
/// </summary>
public class MockCustomerReceiptDesktopService : ICustomerReceiptDesktopService
{
    // RULE-N167 — نصوص الرفض العربية في مكان واحد داخل الملف (RULE-N169).
    private const string NotFoundError = "سند القبض غير موجود";
    private const string ZeroAmountError = "المبلغ يجب أن يكون أكبر من صفر";
    private const string OverBalanceError = "المبلغ أكبر من الرصيد المتبقي للعميل";
    private const string AllocationOverAmountError = "إجمالي التخصيص أكبر من مبلغ السند";
    private const string UnexpectedError = "تعذر تنفيذ العملية على سند القبض. يرجى إعادة المحاولة.";

    public Task<Result<IReadOnlyList<CustomerReceiptDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            IEnumerable<CustomerReceiptDto> query = MockCustomerReceiptsData.Receipts;

            if (!includeInactive)
                query = query.Where(r => r.IsActive);

            return Task.FromResult(Result<IReadOnlyList<CustomerReceiptDto>>.Success(query.ToList().AsReadOnly()));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<IReadOnlyList<CustomerReceiptDto>>.Failure(UnexpectedError));
        }
    }

    public Task<Result<CustomerReceiptDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var receipt = MockCustomerReceiptsData.Receipts.FirstOrDefault(r => r.Id == id);
            return receipt is null
                ? Task.FromResult(Result<CustomerReceiptDto>.Failure(NotFoundError))
                : Task.FromResult(Result<CustomerReceiptDto>.Success(receipt));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<CustomerReceiptDto>.Failure(UnexpectedError));
        }
    }

    public Task<Result<CustomerReceiptDto>> CreateAsync(CreateCustomerReceiptRequest request, CancellationToken ct = default)
    {
        try
        {
            // RULE-N167 — الرفض نفسه الذي سيرفضه الباك-إند: مبلغ صفر/سالب · مبلغ أكبر من المتبقي للعميل.
            if (request.Amount <= 0m)
                return Task.FromResult(Result<CustomerReceiptDto>.Failure(ZeroAmountError));

            if (request.Amount > RemainingBalanceOf(request.CustomerId))
                return Task.FromResult(Result<CustomerReceiptDto>.Failure(OverBalanceError));

            var all = MockCustomerReceiptsData.Receipts;
            var nextId = all.Count > 0 ? all.Max(r => r.Id) + 1 : 1;
            var nextNo = all.Count > 0 ? all.Max(r => r.ReceiptNo) + 1 : 1001;

            // Resolve display names from MockData
            var customer = MockData.Customers.FirstOrDefault(c => c.Id == request.CustomerId);
            var cashBox = MockData.CashBoxes.FirstOrDefault(b => b.Id == request.CashBoxId);

            var receipt = new CustomerReceiptDto(
                Id: nextId,
                ReceiptNo: nextNo,
                ReceiptDate: DateTime.UtcNow,
                CustomerId: request.CustomerId,
                CustomerName: customer?.Name,
                CashBoxId: request.CashBoxId,
                CashBoxName: cashBox?.Name,
                Amount: request.Amount,
                PaymentMethod: request.PaymentMethod,
                PaymentMethodName: PaymentMethodNameOf(request.PaymentMethod),
                Notes: request.Notes,
                Status: 1, // Draft
                StatusName: "مسودة",
                PostedAt: null,
                Applications: new List<CustomerReceiptApplicationDto>(),
                IsActive: true
            );

            all.Add(receipt);
            return Task.FromResult(Result<CustomerReceiptDto>.Success(receipt));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<CustomerReceiptDto>.Failure(UnexpectedError));
        }
    }

    public Task<Result<CustomerReceiptDto>> UpdateAsync(int id, UpdateCustomerReceiptRequest request, CancellationToken ct = default)
    {
        try
        {
            var existing = MockCustomerReceiptsData.Receipts.FirstOrDefault(r => r.Id == id);
            if (existing is null)
                return Task.FromResult(Result<CustomerReceiptDto>.Failure(NotFoundError));

            if (existing.Status != 1)
                return Task.FromResult(Result<CustomerReceiptDto>.Failure("لا يمكن تعديل سند قبض مرحّل أو ملغي"));

            // RULE-N167 — مبلغ صفر/سالب · تخصيص أكبر من مبلغ السند · مبلغ أكبر من المتبقي للعميل.
            if (request.Amount <= 0m)
                return Task.FromResult(Result<CustomerReceiptDto>.Failure(ZeroAmountError));

            if (AllocatedTotalOf(existing.Applications) > request.Amount)
                return Task.FromResult(Result<CustomerReceiptDto>.Failure(AllocationOverAmountError));

            if (request.Amount > RemainingBalanceOf(existing.CustomerId))
                return Task.FromResult(Result<CustomerReceiptDto>.Failure(OverBalanceError));

            var cashBox = MockData.CashBoxes.FirstOrDefault(b => b.Id == request.CashBoxId);

            var updated = existing with
            {
                CashBoxId = request.CashBoxId,
                CashBoxName = cashBox?.Name,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                PaymentMethodName = PaymentMethodNameOf(request.PaymentMethod),
                Notes = request.Notes
            };

            var index = MockCustomerReceiptsData.Receipts.IndexOf(existing);
            MockCustomerReceiptsData.Receipts[index] = updated;
            return Task.FromResult(Result<CustomerReceiptDto>.Success(updated));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<CustomerReceiptDto>.Failure(UnexpectedError));
        }
    }

    public Task<Result<CustomerReceiptDto>> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockCustomerReceiptsData.Receipts.FirstOrDefault(r => r.Id == id);
            if (existing is null)
                return Task.FromResult(Result<CustomerReceiptDto>.Failure(NotFoundError));

            if (existing.Status != 1)
                return Task.FromResult(Result<CustomerReceiptDto>.Failure("لا يمكن ترحيل السند إلا في حالة المسودة"));

            // RULE-N167 — لا يُرحَّل سند بمبلغ صفر/سالب أو بتخصيص يتجاوز مبلغه.
            if (existing.Amount <= 0m)
                return Task.FromResult(Result<CustomerReceiptDto>.Failure(ZeroAmountError));

            if (AllocatedTotalOf(existing.Applications) > existing.Amount)
                return Task.FromResult(Result<CustomerReceiptDto>.Failure(AllocationOverAmountError));

            var posted = existing with
            {
                Status = 2,
                StatusName = "مرحّل",
                PostedAt = DateTime.UtcNow
            };

            var index = MockCustomerReceiptsData.Receipts.IndexOf(existing);
            MockCustomerReceiptsData.Receipts[index] = posted;
            return Task.FromResult(Result<CustomerReceiptDto>.Success(posted));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<CustomerReceiptDto>.Failure(UnexpectedError));
        }
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockCustomerReceiptsData.Receipts.FirstOrDefault(r => r.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure(NotFoundError));

            if (existing.Status == 3)
                return Task.FromResult(Result.Failure("السند ملغي بالفعل"));

            var cancelled = existing with
            {
                Status = 3,
                StatusName = "ملغي"
            };

            var index = MockCustomerReceiptsData.Receipts.IndexOf(existing);
            MockCustomerReceiptsData.Receipts[index] = cancelled;
            return Task.FromResult(Result.Success());
        }
        catch (Exception)
        {
            return Task.FromResult(Result.Failure(UnexpectedError));
        }
    }

    // ═══════════════════════════════════════════════
    // مساعدات الرصيد والتخصيص (RULE-N167)
    // ═══════════════════════════════════════════════

    /// <summary>
    /// الرصيد المتبقي للعميل: مجموع متبقي فواتير البيع غير الملغاة (3 = ملغاة)،
    /// وإذا لم تحمل الفواتير متبقياً يُستهلك رصيد العميل الدفتري من البيانات الأولية.
    /// </summary>
    private static decimal RemainingBalanceOf(int customerId)
    {
        var openInvoicesRemaining = MockData.Invoices
            .Where(i => i.CustomerId == customerId && i.Status != 3)
            .Sum(i => i.RemainingAmount);

        return openInvoicesRemaining > 0m
            ? openInvoicesRemaining
            : MockData.Customers.FirstOrDefault(c => c.Id == customerId)?.CurrentBalance ?? 0m;
    }

    /// <summary>إجمالي المبالغ المخصَّصة على السند.</summary>
    private static decimal AllocatedTotalOf(List<CustomerReceiptApplicationDto>? applications)
        => applications?.Sum(a => a.AppliedAmount) ?? 0m;

    /// <summary>اسم طريقة الدفع من رقمها (مكان واحد للخريطة داخل الملف).</summary>
    private static string PaymentMethodNameOf(byte paymentMethod) => paymentMethod switch
    {
        1 => "نقدي",
        2 => "شيك",
        3 => "تحويل بنكي",
        4 => "بطاقة ائتمان",
        _ => "نقدي"
    };
}
