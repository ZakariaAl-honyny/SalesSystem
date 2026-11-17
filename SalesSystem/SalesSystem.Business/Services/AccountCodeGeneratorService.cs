using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Services;

/// <summary>
/// Thread-safe service for generating hierarchical account codes.
/// Official ladder (docs/database-schema.md §4.1.2.1 — owner decision 2026-11-17):
///   Level 1 (1 digit)  → "1"
///   Level 2 (2 digits) → "11"        (parent "1" + 1 digit)
///   Level 3 (4 digits) → "1101"      (parent "11" + 2 digits)
///   Level 4 (8 digits) → "11010001"  (parent "1101" + 4 digits)
///   deeper levels      → plain increment (parent code + the next number)
/// A candidate code that already exists among the same parent's children is skipped until free.
/// </summary>
public class AccountCodeGeneratorService : IAccountCodeGeneratorService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<AccountCodeGeneratorService> _logger;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public AccountCodeGeneratorService(IUnitOfWork uow, ILogger<AccountCodeGeneratorService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<string>> GenerateCodeAsync(int? parentId, byte level, CancellationToken ct)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            string code;

            if (parentId == null)
            {
                // Level 1: no parent — find max single-digit code
                var existingCodes = await _uow.Accounts.ToListAsync(
                    a => a.Level == 1, ct: ct);

                var usedRootCodes = new HashSet<string>(
                    existingCodes.Select(a => a.AccountCode), StringComparer.Ordinal);

                var maxNum = existingCodes
                    .Select(a => int.TryParse(a.AccountCode, out var n) ? n : 0)
                    .DefaultIfEmpty(0)
                    .Max();

                var nextRoot = maxNum + 1;
                while (usedRootCodes.Contains(nextRoot.ToString()))
                    nextRoot++;

                code = nextRoot.ToString();
            }
            else
            {
                var parent = await _uow.Accounts.GetByIdAsync(parentId.Value, ct);
                if (parent == null)
                    return Result<string>.Failure("الحساب الأب غير موجود", ErrorCodes.NotFound);

                var parentCode = parent.AccountCode;
                var prefixLength = parentCode.Length;

                // Find max child code with this parent prefix
                var children = await _uow.Accounts.ToListAsync(
                    a => a.ParentId == parentId.Value, ct: ct);

                var maxSuffix = 0;
                foreach (var child in children)
                {
                    if (child.AccountCode.Length > prefixLength &&
                        int.TryParse(child.AccountCode[prefixLength..], out var num))
                    {
                        maxSuffix = Math.Max(maxSuffix, num);
                    }
                }

                // Collision guard: skip every candidate that already exists under this parent
                var usedChildCodes = new HashSet<string>(
                    children.Select(c => c.AccountCode), StringComparer.Ordinal);

                var nextSuffix = maxSuffix + 1;
                code = FormatChildCode(parentCode, level, nextSuffix);
                while (usedChildCodes.Contains(code))
                {
                    nextSuffix++;
                    code = FormatChildCode(parentCode, level, nextSuffix);
                }
            }

            _logger.LogDebug("Generated account code '{Code}' at level {Level} (parentId: {ParentId})",
                code, level, parentId);

            return Result<string>.Success(code);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Applies the official suffix width: level 2 ⇒ 1 digit · level 3 ⇒ 2 digits · level 4 ⇒ 4 digits ·
    /// deeper levels ⇒ plain increment.
    /// </summary>
    private static string FormatChildCode(string parentCode, byte level, int suffix) => level switch
    {
        2 => parentCode + suffix.ToString("0"),
        3 => parentCode + suffix.ToString("00"),
        4 => parentCode + suffix.ToString("0000"),
        _ => parentCode + suffix.ToString()
    };
}
