namespace SalesSystem.DTO.Requests;

public record CreatePersonRequest(
    string FirstName,
    string SecondName,
    string? ThirdName,
    string LastName,
    string NationalNo,
    byte Gender,
    DateTime DateOfBirth,
    string? Phone,
    string? Address,
    string? Email,
    string? ImagePath,
    int NationalityCountryId
);

public record UpdatePersonRequest(
    string FirstName,
    string SecondName,
    string? ThirdName,
    string LastName,
    string NationalNo,
    byte Gender,
    DateTime DateOfBirth,
    string? Phone,
    string? Address,
    string? Email,
    string? ImagePath,
    int NationalityCountryId,
    bool IsActive
);
