using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.Common.Enums;
using SalesSystem.Common.Exceptions;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Models;

namespace SalesSystem.Business.Services;

/// <summary>
/// Service for managing cash boxes and computing their balances from operational documents.
/// Balance is derived from: cash sales, customer receipts, supplier payments, and expenses.
/// </summary>
public class CashBoxService : ICashBoxService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<CashBoxService> _logger;

    public CashBoxService(IUnitOfWork uow, ILogger<CashBoxService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    // ═══════════════════════════════════════════════════════════
    // Cash Box CRUD
    // ═══════════════════════════════════════════════════════════

    public async Task<Result<List<CashBoxDto>>> GetAllAsync(CancellationToken ct)
    {
        try
        {
            var boxes = await _uow.CashBoxes.ToListAsync(ct: ct);
            var dtos = boxes.Select(MapToDto).ToList();
            return Result<List<CashBoxDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all cash boxes");
            return Result<List<CashBoxDto>>.Failure("حدث خطأ أثناء استرجاع قائمة الصناديق النقدية");
        }
    }

    public async Task<Result<CashBoxDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var box = await _uow.CashBoxes.FirstOrDefaultIgnoreFiltersAsync(
                b => b.Id == id, ct);
            if (box == null)
                return Result<CashBoxDto>.Failure("الصندوق النقدي غير موجود", ErrorCodes.NotFound);

            return Result<CashBoxDto>.Success(MapToDto(box));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cash box {Id}", id);
            return Result<CashBoxDto>.Failure("حدث خطأ أثناء استرجاع بيانات الصندوق النقدي");
        }
    }

    public async Task<Result<CashBoxDto>> CreateAsync(CreateCashBoxRequest request, int userId, CancellationToken ct)
    {
        return await _uow.ExecuteTransactionAsync<Result<CashBoxDto>>(async () =>
        {
            try
            {
                // Opening balance cannot be stored without a receipt mechanism
                if (request.OpeningBalance > 0)
                {
                    _logger.LogWarning(
                        "Opening balance {Amount} requested for new cash box but cannot be stored — no receipt mechanism available",
                        request.OpeningBalance);
                    return Result<CashBoxDto>.Failure(
                        "لا يمكن إنشاء صندوق برصيد افتتاحي — آلية استلام الرصيد غير متوفرة حالياً");
                }

                var box = CashBox.Create(
                    request.Name,
                    description: request.Description,
                    createdByUserName: userId.ToString());

                await _uow.CashBoxes.AddAsync(box, ct);
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation(
                    "CashBox created: {Name} (ID: {Id}) by User {UserId}",
                    box.Name, box.Id, userId);

                return Result<CashBoxDto>.Success(MapToDto(box));
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain rule violation creating cash box: {Message}", ex.Message);
                return Result<CashBoxDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cash box");
                return Result<CashBoxDto>.Failure("حدث خطأ أثناء إنشاء الصندوق النقدي");
            }
        }, ct);
    }

    public async Task<Result<CashBoxDto>> UpdateAsync(int id, UpdateCashBoxRequest request, int userId, CancellationToken ct)
    {
        var box = await _uow.CashBoxes.FirstOrDefaultIgnoreFiltersAsync(
            b => b.Id == id, ct);
        if (box == null)
            return Result<CashBoxDto>.Failure("الصندوق النقدي غير موجود", ErrorCodes.NotFound);

        return await _uow.ExecuteTransactionAsync<Result<CashBoxDto>>(async () =>
        {
            try
            {
                box.Update(
                    request.Name,
                    description: request.Description);

                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation(
                    "CashBox updated: {Name} (ID: {Id}) by User {UserId}",
                    box.Name, id, userId);

                return Result<CashBoxDto>.Success(MapToDto(box));
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain rule violation updating cash box {Id}: {Message}", id, ex.Message);
                return Result<CashBoxDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cash box {Id}", id);
                return Result<CashBoxDto>.Failure("حدث خطأ أثناء تحديث بيانات الصندوق النقدي");
            }
        }, ct);
    }

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct)
    {
        var box = await _uow.CashBoxes.GetByIdAsync(id, ct);
        if (box == null)
            return Result.Failure("الصندوق النقدي غير موجود", ErrorCodes.NotFound);

        return await _uow.ExecuteTransactionAsync<Result>(async () =>
        {
            try
            {
                box.MarkAsDeleted();
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation(
                    "CashBox deactivated: {Name} (ID: {Id})",
                    box.Name, id);
                return Result.Success();
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain rule violation deactivating cash box {Id}: {Message}", id, ex.Message);
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating cash box {Id}", id);
                return Result.Failure("حدث خطأ أثناء إلغاء تنشيط الصندوق النقدي");
            }
        }, ct);
    }

    public async Task<Result> PermanentDeleteAsync(int id, CancellationToken ct)
    {
        var box = await _uow.CashBoxes.FirstOrDefaultIgnoreFiltersAsync(
            b => b.Id == id, ct);
        if (box == null)
            return Result.Failure("الصندوق النقدي غير موجود", ErrorCodes.NotFound);

        // Check for operational references
        var hasSales = await _uow.SalesInvoices.AnyAsync(
            si => si.CashBoxId == id, ct);
        if (hasSales)
            return Result.Failure(
                "لا يمكن حذف الصندوق نهائياً — لديه فواتير مبيعات مرتبطة");

        var hasReceipts = await _uow.CustomerReceipts.AnyAsync(
            cr => cr.CashBoxId == id, ct);
        if (hasReceipts)
            return Result.Failure(
                "لا يمكن حذف الصندوق نهائياً — لديه سندات قبض مرتبطة");

        var hasPayments = await _uow.SupplierPayments.AnyAsync(
            sp => sp.CashBoxId == id, ct);
        if (hasPayments)
            return Result.Failure(
                "لا يمكن حذف الصندوق نهائياً — لديه مدفوعات مرتبطة");

        var hasExpenses = await _uow.Expenses.AnyAsync(
            e => e.CashBoxId == id, ct);
        if (hasExpenses)
            return Result.Failure(
                "لا يمكن حذف الصندوق نهائياً — لديه مصروفات مرتبطة");

        return await _uow.ExecuteTransactionAsync<Result>(async () =>
        {
            try
            {
                _uow.CashBoxes.DeleteRange(new[] { box });
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation(
                    "CashBox permanently deleted: {Name} (ID: {Id})",
                    box.Name, id);
                return Result.Success();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error permanently deleting cash box {Id}", id);
                return Result.Failure("لا يمكن حذف الصندوق — بيانات مرتبطة في قاعدة البيانات");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error permanently deleting cash box {Id}", id);
                return Result.Failure("حدث خطأ أثناء حذف الصندوق النقدي نهائياً");
            }
        }, ct);
    }

    public async Task<Result> RestoreAsync(int id, CancellationToken ct)
    {
        var box = await _uow.CashBoxes.FirstOrDefaultIgnoreFiltersAsync(
            b => b.Id == id, ct);
        if (box == null)
            return Result.Failure("الصندوق النقدي غير موجود", ErrorCodes.NotFound);

        if (box.IsActive)
            return Result.Failure("الصندوق النقدي نشط بالفعل");

        return await _uow.ExecuteTransactionAsync<Result>(async () =>
        {
            try
            {
                box.Restore();
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation(
                    "CashBox restored: {Name} (ID: {Id})",
                    box.Name, id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring cash box {Id}", id);
                return Result.Failure("حدث خطأ أثناء استعادة الصندوق النقدي");
            }
        }, ct);
    }

    // ═══════════════════════════════════════════════════════════
    // Balance Computation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Computes the cash box balance from operational documents:
    /// + Cash sales settled into the box (Posted invoices with CashBoxId, PaymentType == Cash)
    /// + Customer receipts linked to the box (Posted status)
    /// - Supplier payments linked to the box (Posted status)
    /// - Expenses linked to the box (Posted status)
    /// </summary>
    public async Task<Result<decimal>> GetBalanceAsync(int cashBoxId, CancellationToken ct)
    {
        try
        {
            var box = await _uow.CashBoxes.FirstOrDefaultAsync(b => b.Id == cashBoxId, ct);
            if (box == null)
                return Result<decimal>.Failure("الصندوق النقدي غير موجود", ErrorCodes.NotFound);

            // + Cash sales: Posted invoices where CashBoxId matches and PaymentType == Cash
            var cashSales = await _uow.SalesInvoices.ToListAsync(
                si => si.CashBoxId == cashBoxId
                   && si.Status == InvoiceStatus.Posted
                   && si.PaymentType == PaymentType.Cash,
                ct: ct);
            var cashSalesTotal = cashSales.Sum(si => si.PaidAmount);

            // + Customer receipts: Posted receipts where CashBoxId matches
            var customerReceipts = await _uow.CustomerReceipts.ToListAsync(
                cr => cr.CashBoxId == cashBoxId && cr.Status == InvoiceStatus.Posted,
                ct: ct);
            var receiptsTotal = customerReceipts.Sum(cr => cr.Amount);

            // - Supplier payments: Posted payments where CashBoxId matches
            var supplierPayments = await _uow.SupplierPayments.ToListAsync(
                sp => sp.CashBoxId == cashBoxId && sp.Status == InvoiceStatus.Posted,
                ct: ct);
            var paymentsTotal = supplierPayments.Sum(sp => sp.Amount);

            // - Expenses: Posted expenses where CashBoxId matches
            var expenses = await _uow.Expenses.ToListAsync(
                e => e.CashBoxId == cashBoxId && e.Status == InvoiceStatus.Posted,
                ct: ct);
            var expensesTotal = expenses.Sum(e => e.Amount);

            var balance = cashSalesTotal + receiptsTotal - paymentsTotal - expensesTotal;

            _logger.LogInformation(
                "CashBox {Id} balance computed: {Balance} (Sales: {Sales}, Receipts: {Receipts}, Payments: {Payments}, Expenses: {Expenses})",
                cashBoxId, balance, cashSalesTotal, receiptsTotal, paymentsTotal, expensesTotal);

            return Result<decimal>.Success(balance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error computing balance for cash box {CashBoxId}", cashBoxId);
            return Result<decimal>.Failure("حدث خطأ أثناء حساب رصيد الصندوق النقدي");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Transfer (Not Supported)
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Transfer between cash boxes is not supported without a voucher or transaction mechanism.
    /// Returns a failure with an Arabic message explaining the limitation.
    /// </summary>
    public Task<Result> TransferAsync(CashTransferRequest request, int userId, CancellationToken ct)
    {
        _logger.LogWarning(
            "TransferAsync called but is not supported — no voucher mechanism available. Request: {Amount} from {SourceId} to {DestId}",
            request.Amount, request.SourceCashBoxId, request.DestinationCashBoxId);

        return Task.FromResult(Result.Failure(
            "التحويل بين الصناديق غير مدعوم في هذا الإصدار — لا يوجد مستند تحويل نقدية بين صندوقين"));
    }

    // ═══════════════════════════════════════════════════════════
    // Mapping Helpers
    // ═══════════════════════════════════════════════════════════

    private static CashBoxDto MapToDto(CashBox box)
    {
        return new CashBoxDto(
            box.Id,
            box.Name,
            box.Description,
            box.IsActive
        );
    }
}
