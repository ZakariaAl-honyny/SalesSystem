using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Models;
using SalesSystem.Common.Enums;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Business.Services;

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<SupplierService> _logger;

    public SupplierService(
        IUnitOfWork uow,
        ILogger<SupplierService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<Result<SupplierDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var supplier = await _uow.Suppliers.FirstOrDefaultIgnoreFiltersAsync(
            s => s.Id == id, ct);
        if (supplier == null)
            return Result<SupplierDto>.Failure("المورد غير موجود", ErrorCodes.NotFound);

        return Result<SupplierDto>.Success(MapToDto(supplier));
    }

    public async Task<Result<PagedResult<SupplierDto>>> GetAllAsync(string? search, int page, int pageSize, bool includeInactive = false, CancellationToken ct = default)
    {
        System.Linq.Expressions.Expression<System.Func<Supplier, bool>>? predicate = null;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search;
            predicate = sup => sup.Name.Contains(s) || (sup.Phone != null && sup.Phone.Contains(s));
        }

        var (items, total) = await _uow.Suppliers.GetPagedAsync(
            predicate, q => q.OrderByDescending(s => s.Id), page, pageSize, ct, includeInactive);

        var dtos = items.Select(MapToDto).ToList();
        return Result<PagedResult<SupplierDto>>.Success(PagedResult<SupplierDto>.Create(dtos, total, page, pageSize));
    }

    public async Task<Result<SupplierDto>> CreateAsync(CreateSupplierRequest request, int userId, CancellationToken ct)
    {
        return await _uow.ExecuteTransactionAsync<Result<SupplierDto>>(async () =>
        {
            try
            {
                // Create Supplier record with direct contact fields
                var supplier = Supplier.Create(
                    name: request.Name,
                    phone: request.Phone,
                    email: request.Email,
                    address: request.Address,
                    taxNumber: request.TaxNumber,
                    notes: request.Notes,
                    creditLimit: request.CreditLimit,
                    categoryId: request.CategoryId);
                await _uow.Suppliers.AddAsync(supplier, ct);
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation("Supplier created: {SupplierName} (ID: {SupplierId})", supplier.Name, supplier.Id);

                return Result<SupplierDto>.Success(MapToDto(supplier));
            }
            catch (DomainException ex)
            {
                return Result<SupplierDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating supplier");
                return Result<SupplierDto>.Failure("حدث خطأ أثناء إضافة المورد.");
            }
        }, ct);
    }

    public async Task<Result<SupplierDto>> UpdateAsync(int id, UpdateSupplierRequest request, int userId, CancellationToken ct)
    {
        try
        {
            var supplier = await _uow.Suppliers.FirstOrDefaultIgnoreFiltersAsync(
                s => s.Id == id, ct);
            if (supplier == null)
                return Result<SupplierDto>.Failure("المورد غير موجود", ErrorCodes.NotFound);

            // ---- entity+account modification wrapped in atomic transaction ----
            return await _uow.ExecuteTransactionAsync<Result<SupplierDto>>(async () =>
            {
                try
                {
                    // Capture old values for Account sync BEFORE update
                    var oldIsActive = supplier.IsActive;

                    // Update supplier fields including contact information
                    supplier.Update(
                        name: request.Name,
                        phone: request.Phone,
                        email: request.Email,
                        address: request.Address,
                        taxNumber: request.TaxNumber,
                        notes: request.Notes,
                        creditLimit: request.CreditLimit,
                        categoryId: request.CategoryId);

                    if (request.IsActive != oldIsActive)
                    {
                        if (request.IsActive) supplier.Restore();
                        else supplier.MarkAsDeleted();
                    }

                    await _uow.Suppliers.UpdateAsync(supplier, ct);
                    await _uow.SaveChangesAsync(ct);

                    _logger.LogInformation("Supplier updated: {SupplierName} (ID: {SupplierId})", supplier.Name, supplier.Id);

                    return Result<SupplierDto>.Success(MapToDto(supplier));
                }
                catch (DomainException ex)
                {
                    return Result<SupplierDto>.Failure(ex.Message);
                }
            }, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating supplier {Id}", id);
            return Result<SupplierDto>.Failure("حدث خطأ أثناء تحديث بيانات المورد.");
        }
    }

    public async Task<Result> DeleteAsync(int id, int userId, CancellationToken ct)
    {
        var supplier = await _uow.Suppliers.FirstOrDefaultIgnoreFiltersAsync(s => s.Id == id, ct);
        if (supplier == null)
            return Result.Failure("المورد غير موجود", ErrorCodes.NotFound);

        // ---- entity modification wrapped in atomic transaction ----
        return await _uow.ExecuteTransactionAsync<Result>(async () =>
        {
            try
            {
                await _uow.Suppliers.SoftDeleteAsync(id, ct);
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation("Supplier soft-deleted: {SupplierId} by user {UserId}", id, userId);
                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Message);
            }
        }, ct);
    }

    public async Task<Result> PermanentDeleteAsync(int id, int userId, CancellationToken ct)
    {
        var supplier = await _uow.Suppliers.FirstOrDefaultIgnoreFiltersAsync(s => s.Id == id, ct);
        if (supplier == null)
            return Result.Failure("المورد غير موجود", ErrorCodes.NotFound);

        if (await _uow.PurchaseInvoices.AnyAsync(pi => pi.SupplierId == id, ct))
            return Result.Failure("لا يمكن حذف المورد نهائياً لأنه مرتبط بفواتير شراء");

        if (await _uow.SupplierPayments.AnyAsync(sp => sp.SupplierId == id, ct))
            return Result.Failure("لا يمكن حذف المورد نهائياً لأنه مرتبط بسندات صرف");

        // ---- entity modification wrapped in atomic transaction ----
        return await _uow.ExecuteTransactionAsync<Result>(async () =>
        {
            try
            {
                await _uow.Suppliers.HardDeleteAsync(id, ct);
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation("Supplier permanently deleted: {SupplierId} by user {UserId}", id, userId);
                return Result.Success();
            }
            catch (Exception ex) when (ex.GetType().Name.Contains("DbUpdate") || ex.GetType().Name.Contains("Sql"))
            {
                _logger.LogError(ex, "Failed to permanently delete supplier {SupplierId} due to database constraint", id);
                return Result.Failure("لا يمكن حذف المورد نهائياً. قد يكون مرتبطاً ببيانات أخرى في النظام.");
            }
        }, ct);
    }

    public async Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
    {
        var supplier = await _uow.Suppliers.FirstOrDefaultIgnoreFiltersAsync(s => s.Id == id, ct);
        if (supplier == null)
            return Result.Failure("المورد غير موجود", ErrorCodes.NotFound);

        if (supplier.IsActive)
            return Result.Failure("المورد نشط بالفعل");

        return await _uow.ExecuteTransactionAsync<Result>(async () =>
        {
            try
            {
                supplier.Restore();
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation("Supplier reactivated: {SupplierName} (ID: {SupplierId})", supplier.Name, supplier.Id);
                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while reactivating supplier {Id}", id);
                return Result.Failure("حدث خطأ أثناء إعادة تنشيط المورد.");
            }
        }, ct);
    }

    private static SupplierDto MapToDto(Supplier s)
    {
        return new SupplierDto(
            s.Id,
            s.Name,
            s.Phone,
            s.Email,
            s.Address,
            s.TaxNumber,
            s.Notes,
            s.CreditLimit,
            s.IsActive,
            CategoryId: s.CategoryId
        );
    }
}
