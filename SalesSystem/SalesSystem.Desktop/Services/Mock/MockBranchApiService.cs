using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة الفروع — قائمة في الذاكرة بلا قاعدة بيانات (وضع التجربة).
/// تعيد نفس الفرع الذي ينشئه DbSeeder: «الفرع الرئيسي».
/// </summary>
public class MockBranchApiService : IBranchApiService
{
    private static readonly IReadOnlyList<BranchDto> Branches = new List<BranchDto>
    {
        new BranchDto(1, "الفرع الرئيسي", "الفرع الرئيسي", null, true)
    }.AsReadOnly();

    public Task<Result<IReadOnlyList<BranchDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        // لا توجد فروع معطّلة حالياً — نفس القائمة تُعاد في الحالتين.
        return Task.FromResult(Result<IReadOnlyList<BranchDto>>.Success(Branches));
    }
}
