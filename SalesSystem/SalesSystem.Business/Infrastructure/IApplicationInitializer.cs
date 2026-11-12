using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Infrastructure;

public interface IApplicationInitializer
{
    Task<Result> InitializeAsync(CancellationToken ct = default);
}
