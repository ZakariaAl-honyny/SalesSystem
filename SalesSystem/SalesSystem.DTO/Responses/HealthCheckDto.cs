namespace SalesSystem.DTO.Responses;

public record HealthCheckDto(
    string Status,
    string DatabaseStatus,
    DateTime Timestamp
);
