using System.Linq.Expressions;
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

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        IUnitOfWork uow,
        ILogger<CustomerService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<Result<CustomerDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var customer = await _uow.Customers.FirstOrDefaultIgnoreFiltersAsync(
            c => c.Id == id, ct);
        if (customer == null)
            return Result<CustomerDto>.Failure("العميل غير موجود", ErrorCodes.NotFound);

        return Result<CustomerDto>.Success(MapToDto(customer));
    }

    public async Task<Result<PagedResult<CustomerDto>>> GetAllAsync(string? search, int page, int pageSize, bool includeInactive = false, CancellationToken ct = default)
    {
        Expression<Func<Customer, bool>>? predicate = null;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search;
            predicate = c => c.Name.Contains(s) || (c.Phone != null && c.Phone.Contains(s));
        }

        var (items, total) = await _uow.Customers.GetPagedAsync(
            predicate, q => q.OrderByDescending(c => c.Id), page, pageSize, ct, includeInactive);

        var dtos = items.Select(MapToDto).ToList();
        return Result<PagedResult<CustomerDto>>.Success(PagedResult<CustomerDto>.Create(dtos, total, page, pageSize));
    }

    public async Task<Result<CustomerDto>> CreateAsync(CreateCustomerRequest request, int userId, CancellationToken ct)
    {
        return await _uow.ExecuteTransactionAsync<Result<CustomerDto>>(async () =>
        {
            try
            {
                // Create Customer record with direct contact fields
                var customer = Customer.Create(
                    name: request.Name,
                    phone: request.Phone,
                    email: request.Email,
                    address: request.Address,
                    notes: request.Notes,
                    creditLimit: request.CreditLimit);
                await _uow.Customers.AddAsync(customer, ct);
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation("Customer created: {CustomerName} (ID: {CustomerId})", customer.Name, customer.Id);

                return Result<CustomerDto>.Success(MapToDto(customer));
            }
            catch (DomainException ex)
            {
                return Result<CustomerDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating customer");
                return Result<CustomerDto>.Failure("حدث خطأ أثناء إضافة العميل.");
            }
        }, ct);
    }

    public async Task<Result<CustomerDto>> UpdateAsync(int id, UpdateCustomerRequest request, int userId, CancellationToken ct)
    {
        var customer = await _uow.Customers.FirstOrDefaultIgnoreFiltersAsync(
            c => c.Id == id, ct);
        if (customer == null)
            return Result<CustomerDto>.Failure("العميل غير موجود", ErrorCodes.NotFound);

        // Capture old values for Account sync BEFORE transaction
        var oldName = customer.Name;
        var oldIsActive = customer.IsActive;

        return await _uow.ExecuteTransactionAsync<Result<CustomerDto>>(async () =>
        {
            try
            {
                // Update customer fields including contact information
                customer.Update(
                    name: request.Name,
                    phone: request.Phone,
                    email: request.Email,
                    address: request.Address,
                    notes: request.Notes,
                    creditLimit: request.CreditLimit);

                if (request.IsActive != oldIsActive)
                {
                    if (request.IsActive) customer.Restore();
                    else customer.MarkAsDeleted();
                }

                await _uow.Customers.UpdateAsync(customer, ct);
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation("Customer updated: {CustomerName} (ID: {CustomerId})", customer.Name, customer.Id);

                return Result<CustomerDto>.Success(MapToDto(customer));
            }
            catch (DomainException ex)
            {
                return Result<CustomerDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating customer {Id}", id);
                return Result<CustomerDto>.Failure("حدث خطأ أثناء تحديث بيانات العميل.");
            }
        }, ct);
    }

    public async Task<Result> DeleteAsync(int id, int userId, CancellationToken ct)
    {
        var customer = await _uow.Customers.FirstOrDefaultIgnoreFiltersAsync(c => c.Id == id, ct);
        if (customer == null)
            return Result.Failure("العميل غير موجود", ErrorCodes.NotFound);

        return await _uow.ExecuteTransactionAsync(async () =>
        {
            customer.MarkAsDeleted();
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Customer soft-deleted: {CustomerId} by user {UserId}", id, userId);
            return Result.Success();
        }, ct);
    }

    public async Task<Result> PermanentDeleteAsync(int id, int userId, CancellationToken ct)
    {
        var customer = await _uow.Customers.FirstOrDefaultIgnoreFiltersAsync(c => c.Id == id, ct);
        if (customer == null)
            return Result.Failure("العميل غير موجود", ErrorCodes.NotFound);

        if (await _uow.SalesInvoices.AnyAsync(si => si.CustomerId == id, ct))
            return Result.Failure("لا يمكن حذف العميل نهائياً لأنه مرتبط بفواتير بيع");

        return await _uow.ExecuteTransactionAsync<Result>(async () =>
        {
            try
            {
                _uow.Customers.DeleteRange(new[] { customer });
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation("Customer permanently deleted: {CustomerId} by user {UserId}", id, userId);
                return Result.Success();
            }
            catch (Exception ex) when (ex.GetType().Name.Contains("DbUpdate") || ex.GetType().Name.Contains("Sql"))
            {
                _logger.LogError(ex, "Failed to permanently delete customer {CustomerId} due to database constraint", id);
                return Result.Failure("لا يمكن حذف العميل نهائياً. قد يكون مرتبطاً ببيانات أخرى في النظام.");
            }
        }, ct);
    }

    public async Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
    {
        var customer = await _uow.Customers.FirstOrDefaultIgnoreFiltersAsync(c => c.Id == id, ct);
        if (customer == null)
            return Result.Failure("العميل غير موجود", ErrorCodes.NotFound);

        if (customer.IsActive)
            return Result.Failure("العميل نشط بالفعل");

        return await _uow.ExecuteTransactionAsync<Result>(async () =>
        {
            try
            {
                customer.Restore();
                await _uow.SaveChangesAsync(ct);

                _logger.LogInformation("Customer reactivated: {CustomerName} (ID: {CustomerId})", customer.Name, customer.Id);
                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while reactivating customer {Id}", id);
                return Result.Failure("حدث خطأ أثناء إعادة تنشيط العميل.");
            }
        }, ct);
    }

    public async Task<Result<PagedResult<CustomerBalanceReportDto>>> GetCustomerBalanceReportAsync(int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        try
        {
            Expression<Func<Customer, bool>>? predicate = null;
            if (!string.IsNullOrWhiteSpace(search))
                predicate = c => c.Name.Contains(search);

            var (items, total) = await _uow.Customers.GetPagedAsync(
                predicate,
                q => q.OrderByDescending(c => c.Id),
                page, pageSize, ct);

            var dtos = items.Select(c =>
            {
                return new CustomerBalanceReportDto(
                    c.Id, c.Name, c.Phone, null,
                    CreditLimit: c.CreditLimit,
                    CurrentBalance: 0,
                    BalanceStatus: c.CreditLimit > 0 ? "له حد ائتماني" : "بدون حد ائتماني"
                );
            }).ToList();

            return Result<PagedResult<CustomerBalanceReportDto>>.Success(
                PagedResult<CustomerBalanceReportDto>.Create(dtos, total, page, pageSize));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching customer balance report");
            return Result<PagedResult<CustomerBalanceReportDto>>.Failure("حدث خطأ أثناء تحميل تقرير أرصدة العملاء");
        }
    }

    public async Task<Result<PagedResult<CustomerAgingReportDto>>> GetCustomerAgingReportAsync(int page, int pageSize, CancellationToken ct = default)
    {
        try
        {
            var (items, total) = await _uow.Customers.GetPagedAsync(
                null,
                q => q.OrderByDescending(c => c.Id),
                page, pageSize, ct, false);

            var dtos = items.Select(c =>
            {
                var agingBucket = c.CreditLimit > 0 ? "له حد ائتماني" : "بدون حد ائتماني";
                return new CustomerAgingReportDto(
                    c.Id, c.Name, c.Phone, 0,
                    agingBucket, DateTime.UtcNow);
            }).ToList();

            return Result<PagedResult<CustomerAgingReportDto>>.Success(
                PagedResult<CustomerAgingReportDto>.Create(dtos, total, page, pageSize));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching customer aging report");
            return Result<PagedResult<CustomerAgingReportDto>>.Failure("حدث خطأ أثناء تحميل تقرير أعمار العملاء");
        }
    }

    private static CustomerDto MapToDto(Customer c)
    {
        return new CustomerDto(
            c.Id,
            c.Name,
            c.Phone,
            c.Email,
            c.Address,
            c.CreditLimit,
            c.IsActive,
            CategoryId: c.CategoryId,
            Notes: c.Notes
        );
    }
}
