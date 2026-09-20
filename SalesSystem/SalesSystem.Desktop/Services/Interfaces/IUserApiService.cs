using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IUserApiService
{
    Task<Result<IReadOnlyList<UserDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<UserDto>> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
    Task<Result<UserDto>> UpdateAsync(int id, UpdateUserRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    Task<Result<ResetPasswordResponse>> ResetPasswordAsync(int id, CancellationToken ct = default);
}

