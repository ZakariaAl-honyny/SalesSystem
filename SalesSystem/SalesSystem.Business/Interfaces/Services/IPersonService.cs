using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Business.Interfaces.Services;

public interface IPersonService
{
    Task<List<PersonDto>> GetAllAsync();
    Task<List<PersonDto>> GetAllAsync(string? search);
    Task<Result<PersonDto>> GetByIdAsync(int id);
    Task<Result<PersonDto>> GetByNationalNoAsync(string nationalNo);
    Task<Result<PersonDto>> CreateAsync(CreatePersonRequest request);
    Task<Result<PersonDto>> UpdateAsync(int id, UpdatePersonRequest request);
}

