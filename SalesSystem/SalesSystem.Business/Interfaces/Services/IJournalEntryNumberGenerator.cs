using SalesSystem.Business.Accounting.Services;
using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Interfaces.Services;

public interface IJournalEntryNumberGenerator
{
    Task<Result<JournalEntryNumberResult>> GenerateAsync(CancellationToken ct = default);
}
