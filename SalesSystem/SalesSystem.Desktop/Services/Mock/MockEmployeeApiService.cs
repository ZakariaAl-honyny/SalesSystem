using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة الموظفين — تعيد بيانات من MockEmployeesData.
/// </summary>
public class MockEmployeeApiService : IEmployeeApiService
{
    public Task<Result<IReadOnlyList<EmployeeDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<EmployeeDto> query = MockEmployeesData.Employees;

        if (!includeInactive)
            query = query.Where(e => e.IsActive);

        var result = query.OrderBy(e => e.Id).ToList().AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<EmployeeDto>>.Success(result));
    }

    public Task<Result<EmployeeDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var employee = MockEmployeesData.Employees.FirstOrDefault(e => e.Id == id);
        return employee is null
            ? Task.FromResult(Result<EmployeeDto>.Failure("الموظف غير موجود"))
            : Task.FromResult(Result<EmployeeDto>.Success(employee));
    }

    public Task<Result<EmployeeDto>> CreateAsync(EmployeeDto request, CancellationToken ct = default)
    {
        var nextId = MockEmployeesData.Employees.Count > 0
            ? MockEmployeesData.Employees.Max(e => e.Id) + 1
            : 1;

        var nextCode = MockEmployeesData.Employees.Count > 0
            ? (int.Parse(MockEmployeesData.Employees.Max(e => e.Code)) + 1).ToString()
            : "100";

        var employee = request with { Id = nextId, Code = nextCode };
        MockEmployeesData.Employees.Add(employee);
        return Task.FromResult(Result<EmployeeDto>.Success(employee));
    }

    public Task<Result<EmployeeDto>> UpdateAsync(int id, EmployeeDto request, CancellationToken ct = default)
    {
        var existing = MockEmployeesData.Employees.FirstOrDefault(e => e.Id == id);
        if (existing is null)
            return Task.FromResult(Result<EmployeeDto>.Failure("الموظف غير موجود"));

        var updated = request with { Id = id, Code = existing.Code };
        var index = MockEmployeesData.Employees.IndexOf(existing);
        MockEmployeesData.Employees[index] = updated;
        return Task.FromResult(Result<EmployeeDto>.Success(updated));
    }

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockEmployeesData.Employees.FirstOrDefault(e => e.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الموظف غير موجود"));

        var index = MockEmployeesData.Employees.IndexOf(existing);
        MockEmployeesData.Employees[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }
}
