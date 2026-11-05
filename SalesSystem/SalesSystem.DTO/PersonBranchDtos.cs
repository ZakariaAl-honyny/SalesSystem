namespace SalesSystem.DTO;

public record PersonDto(
    int Id,
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
    string? CountryName,
    bool IsActive)
{
    public string FullName => $"{FirstName} {SecondName} {ThirdName ?? ""} {LastName}".Trim();
    public string GenderDisplay => Gender == 1 ? "ذكر" : "أنثى";
}

public record BranchDto(
    short Id,
    string Name,
    string? Address = null,
    string? Phone = null,
    bool IsActive = true);
