using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة الموردين — تعيد بيانات من MockSuppliersData.
/// </summary>
public class MockSupplierApiService : ISupplierApiService
{
    public Task<Result<IReadOnlyList<SupplierDto>>> GetAllAsync(string? search = null, bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<SupplierDto> query = MockSuppliersData.Suppliers;

        if (!includeInactive)
            query = query.Where(s => s.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(sup =>
                sup.Name.Contains(s, StringComparison.OrdinalIgnoreCase) ||
                (sup.Phone != null && sup.Phone.Contains(s)) ||
                // TODO: Company removed from SupplierDto and requests.
                (sup.Email != null && sup.Email.Contains(s, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult(Result<IReadOnlyList<SupplierDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<SupplierDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var supplier = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == id);
        return supplier is null
            ? Task.FromResult(Result<SupplierDto>.Failure("المورد غير موجود"))
            : Task.FromResult(Result<SupplierDto>.Success(supplier));
    }

    public Task<Result<SupplierDto>> CreateAsync(CreateSupplierRequest r, CancellationToken ct = default)
    {
        var nextId = MockSuppliersData.Suppliers.Count > 0 ? MockSuppliersData.Suppliers.Max(s => s.Id) + 1 : 1;
        var nextAccountId = MockSuppliersData.Suppliers.Count > 0 ? MockSuppliersData.Suppliers.Max(s => s.AccountId) + 1 : 301;

        var supplier = new SupplierDto(
            Id: nextId, Name: r.Name, Phone: r.Phone, Email: r.Email,
            Address: r.Address, TaxNumber: r.TaxNumber, Notes: r.Notes,
            CreditLimit: r.CreditLimit, IsActive: true,
            AccountId: nextAccountId, AccountName: $"موردون - {r.Name}",
            CategoryId: r.CategoryId, BranchId: r.BranchId ?? 0);
        // TODO: Code / Company / Currency / ContactPerson / CreditDays removed from
        // SupplierDto and CreateSupplierRequest.

        MockSuppliersData.Suppliers.Add(supplier);
        return Task.FromResult(Result<SupplierDto>.Success(supplier));
    }

    public Task<Result<SupplierDto>> UpdateAsync(int id, UpdateSupplierRequest r, CancellationToken ct = default)
    {
        var existing = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == id);
        if (existing is null)
            return Task.FromResult(Result<SupplierDto>.Failure("المورد غير موجود"));

        var updated = existing with
        {
            Name = r.Name,
            Phone = r.Phone,
            Email = r.Email,
            Address = r.Address,
            TaxNumber = r.TaxNumber,
            Notes = r.Notes,
            CreditLimit = r.CreditLimit,
            IsActive = r.IsActive,
            CategoryId = r.CategoryId
        };
        // TODO: Company / Currency / ContactPerson / CreditDays removed from
        // UpdateSupplierRequest and SupplierDto.

        var index = MockSuppliersData.Suppliers.IndexOf(existing);
        MockSuppliersData.Suppliers[index] = updated;
        return Task.FromResult(Result<SupplierDto>.Success(updated));
    }

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("المورد غير موجود"));

        var index = MockSuppliersData.Suppliers.IndexOf(existing);
        MockSuppliersData.Suppliers[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("المورد غير موجود"));

        var index = MockSuppliersData.Suppliers.IndexOf(existing);
        MockSuppliersData.Suppliers[index] = existing with { IsActive = true };
        return Task.FromResult(Result.Success());
    }
}
