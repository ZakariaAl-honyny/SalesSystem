using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ISystemAccountMappingApiService
{
    Task<Result<IReadOnlyList<SystemAccountMappingDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<SystemAccountMappingDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result> UpdateAsync(int id, UpdateSystemAccountMappingRequest request, CancellationToken ct = default);
}
