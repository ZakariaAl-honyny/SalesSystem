using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Business.Interfaces.Services;

public interface ICustomerContactService
{
    Task<Result<List<CustomerContactDto>>> GetAllAsync(int customerId, CancellationToken ct);
    Task<Result<CustomerContactDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<CustomerContactDto>> CreateAsync(CreateCustomerContactRequest request, int userId, CancellationToken ct);
    Task<Result<CustomerContactDto>> UpdateAsync(int id, UpdateCustomerContactRequest request, int userId, CancellationToken ct);
    Task<Result> DeactivateAsync(int id, CancellationToken ct);
}
