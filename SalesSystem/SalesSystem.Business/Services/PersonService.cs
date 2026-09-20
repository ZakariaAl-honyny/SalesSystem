using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.Common.Helpers;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Business.Services;

public class PersonService : IPersonService
{
    private readonly ILogger<PersonService> _logger;
    private static readonly List<PersonDto> _people = new();
    private static int _nextId = 1;

    public PersonService(ILogger<PersonService> logger)
    {
        _logger = logger;
    }

    public Task<List<PersonDto>> GetAllAsync()
    {
        return Task.FromResult(_people.ToList());
    }

    public Task<List<PersonDto>> GetAllAsync(string? search)
    {
        if (search.IsNullOrWhiteSpace())
            return GetAllAsync();

        var results = _people.Where(p =>
            p.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
            p.NationalNo.Contains(search)).ToList();

        return Task.FromResult(results);
    }

    public Task<Result<PersonDto>> GetByIdAsync(int id)
    {
        try
        {
            var person = _people.FirstOrDefault(p => p.Id == id);
            if (person == null)
                return Task.FromResult(Result<PersonDto>.Failure("ظ„ط§ ظٹظˆط¬ط¯ ط´ط®طµ ط¨ظ‡ط°ط§ ط§ظ„ظ…ط¹ط±ظپ", ErrorCodes.NotFound));

            return Task.FromResult(Result<PersonDto>.Success(person));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving person {Id}", id);
            return Task.FromResult(Result<PersonDto>.Failure("ط­ط¯ط« ط®ط·ط£ ط£ط«ظ†ط§ط، ط§ط³طھط±ط¬ط§ط¹ ط¨ظٹط§ظ†ط§طھ ط§ظ„ط´ط®طµ"));
        }
    }

    public Task<Result<PersonDto>> GetByNationalNoAsync(string nationalNo)
    {
        try
        {
            var person = _people.FirstOrDefault(p => p.NationalNo == nationalNo);
            if (person == null)
                return Task.FromResult(Result<PersonDto>.Failure("ظ„ط§ ظٹظˆط¬ط¯ ط´ط®طµ ط¨ظ‡ط°ط§ ط§ظ„ط±ظ‚ظ… ط§ظ„ظˆط·ظ†ظٹ", ErrorCodes.NotFound));

            return Task.FromResult(Result<PersonDto>.Success(person));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving person by national no {NationalNo}", nationalNo);
            return Task.FromResult(Result<PersonDto>.Failure("ط­ط¯ط« ط®ط·ط£ ط£ط«ظ†ط§ط، ط§ط³طھط±ط¬ط§ط¹ ط¨ظٹط§ظ†ط§طھ ط§ظ„ط´ط®طµ"));
        }
    }

    public Task<Result<PersonDto>> CreateAsync(CreatePersonRequest request)
    {
        try
        {
            var person = new PersonDto(
                Id: _nextId++,
                FirstName: request.FirstName,
                SecondName: request.SecondName,
                ThirdName: request.ThirdName,
                LastName: request.LastName,
                NationalNo: request.NationalNo,
                Gender: request.Gender,
                DateOfBirth: request.DateOfBirth,
                Phone: request.Phone,
                Address: request.Address,
                Email: request.Email,
                ImagePath: request.ImagePath,
                NationalityCountryId: request.NationalityCountryId,
                CountryName: null,
                IsActive: true);

            _people.Add(person);

            _logger.LogInformation("Person created: {FullName} (ID: {PersonId})", person.FullName, person.Id);

            return Task.FromResult(Result<PersonDto>.Success(person));
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain rule violation creating person: {Message}", ex.Message);
            return Task.FromResult(Result<PersonDto>.Failure(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating person");
            return Task.FromResult(Result<PersonDto>.Failure("ط­ط¯ط« ط®ط·ط£ ط£ط«ظ†ط§ط، ط¥ظ†ط´ط§ط، ط¨ظٹط§ظ†ط§طھ ط§ظ„ط´ط®طµ"));
        }
    }

    public Task<Result<PersonDto>> UpdateAsync(int id, UpdatePersonRequest request)
    {
        try
        {
            var index = _people.FindIndex(p => p.Id == id);
            if (index < 0)
                return Task.FromResult(Result<PersonDto>.Failure("ظ„ط§ ظٹظˆط¬ط¯ ط´ط®طµ ط¨ظ‡ط°ط§ ط§ظ„ظ…ط¹ط±ظپ", ErrorCodes.NotFound));

            var updated = new PersonDto(
                Id: id,
                FirstName: request.FirstName,
                SecondName: request.SecondName,
                ThirdName: request.ThirdName,
                LastName: request.LastName,
                NationalNo: request.NationalNo,
                Gender: request.Gender,
                DateOfBirth: request.DateOfBirth,
                Phone: request.Phone,
                Address: request.Address,
                Email: request.Email,
                ImagePath: request.ImagePath,
                NationalityCountryId: request.NationalityCountryId,
                CountryName: _people[index].CountryName,
                IsActive: request.IsActive);

            _people[index] = updated;

            _logger.LogInformation("Person updated: {FullName} (ID: {PersonId})", updated.FullName, updated.Id);

            return Task.FromResult(Result<PersonDto>.Success(updated));
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain rule violation updating person {Id}: {Message}", id, ex.Message);
            return Task.FromResult(Result<PersonDto>.Failure(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating person {Id}", id);
            return Task.FromResult(Result<PersonDto>.Failure("ط­ط¯ط« ط®ط·ط£ ط£ط«ظ†ط§ط، طھط­ط¯ظٹط« ط¨ظٹط§ظ†ط§طھ ط§ظ„ط´ط®طµ"));
        }
    }
}

