using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.DTO.Requests;

/// <summary>
/// Request to create or update a system account mapping.
/// </summary>
public record CreateSystemAccountMappingRequest(
    SystemAccountKey MappingKey,
    int AccountId,
    short? BranchId = null
);

/// <summary>
/// Request to update an existing system account mapping.
/// </summary>
public record UpdateSystemAccountMappingRequest(
    int AccountId
);
