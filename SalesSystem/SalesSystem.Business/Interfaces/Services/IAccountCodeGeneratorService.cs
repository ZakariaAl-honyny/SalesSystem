using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Interfaces.Services;

/// <summary>
/// Thread-safe service for generating hierarchical account codes.
/// Official ladder (docs/database-schema.md §4.1.2.1):
/// level 1 = 1 digit (1) · level 2 = 2 digits (11) · level 3 = 4 digits (1101) · level 4 = 8 digits (11010001).
/// ورمز الابن = كود الأب + لاحقة بعدد خانات المستوى (1 · 2 · 4)، وإن كان الكود المرشّح موجوداً يُزاد حتى يفرغ.
/// </summary>
public interface IAccountCodeGeneratorService
{
    /// <summary>
    /// Generates the next account code for a given parent account and level.
    /// Thread-safe via SemaphoreSlim.
    /// </summary>
    Task<Result<string>> GenerateCodeAsync(int? parentId, byte level, CancellationToken ct = default);

    /// <summary>
    /// Gets the default color code for a given account nature.
    /// Asset=#2196F3, Liability=#F44336, Equity=#4CAF50,
    /// Revenue=#4CAF50, Expense=#FF9800.
    /// </summary>
    static string GetColorCode(byte nature) => nature switch
    {
        1 => "#2196F3",  // Asset - Blue
        2 => "#F44336",  // Liability - Red
        3 => "#4CAF50",  // Equity - Green
        4 => "#4CAF50",  // Revenue - Green
        5 => "#FF9800",  // Expense - Orange
        _ => "#9E9E9E"   // Unknown - Grey
    };
}
