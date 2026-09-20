using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة العملاء — تعيد بيانات من MockData.
/// </summary>
public class MockCustomerApiService : ICustomerApiService
{
    public Task<Result<IReadOnlyList<CustomerDto>>> GetAllAsync(string? search = null, bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<CustomerDto> query = MockData.Customers;

        if (!includeInactive)
            query = query.Where(c => c.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(c =>
                c.Name.Contains(s, StringComparison.OrdinalIgnoreCase) ||
                (c.Phone != null && c.Phone.Contains(s)) ||
                (c.Email != null && c.Email.Contains(s, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult(Result<IReadOnlyList<CustomerDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<CustomerDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var customer = MockData.Customers.FirstOrDefault(c => c.Id == id);
        return customer is null
            ? Task.FromResult(Result<CustomerDto>.Failure("العميل غير موجود"))
            : Task.FromResult(Result<CustomerDto>.Success(customer));
    }

    public Task<Result<CustomerDto>> CreateAsync(CreateCustomerRequest r, CancellationToken ct = default)
    {
        var nextId = MockData.Customers.Max(c => c.Id) + 1;
        var nextAccountId = MockData.Customers.Max(c => c.AccountId) + 1;
        var customer = new CustomerDto(
            Id: nextId, Name: r.Name, Phone: r.Phone, Email: r.Email,
            Address: r.Address, TaxNumber: r.TaxNumber, CreditLimit: r.CreditLimit,
            IsActive: true, AccountId: nextAccountId, Notes: r.Notes);
        MockData.Customers.Add(customer);
        return Task.FromResult(Result<CustomerDto>.Success(customer));
    }

    public Task<Result<CustomerDto>> UpdateAsync(int id, UpdateCustomerRequest r, CancellationToken ct = default)
    {
        var existing = MockData.Customers.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result<CustomerDto>.Failure("العميل غير موجود"));

        var updated = existing with
        {
            Name = r.Name,
            Phone = r.Phone,
            Email = r.Email,
            Address = r.Address,
            TaxNumber = r.TaxNumber,
            Notes = r.Notes,
            CreditLimit = r.CreditLimit,
            IsActive = r.IsActive
        };

        var index = MockData.Customers.IndexOf(existing);
        MockData.Customers[index] = updated;
        return Task.FromResult(Result<CustomerDto>.Success(updated));
    }

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.Customers.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("العميل غير موجود"));

        var index = MockData.Customers.IndexOf(existing);
        MockData.Customers[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.Customers.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("العميل غير موجود"));

        var index = MockData.Customers.IndexOf(existing);
        MockData.Customers[index] = existing with { IsActive = true };
        return Task.FromResult(Result.Success());
    }
}
