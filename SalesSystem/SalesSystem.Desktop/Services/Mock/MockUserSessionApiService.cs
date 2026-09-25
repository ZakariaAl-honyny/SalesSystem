using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة الجلسات — تعيد بيانات من MockSecurityData.
/// </summary>
public class MockUserSessionApiService : IUserSessionApiService
{
    public Task<Result<IReadOnlyList<UserSessionDto>>> GetAllAsync(
        int? userId = null, bool includeRevoked = false, CancellationToken ct = default)
    {
        IEnumerable<UserSessionDto> query = MockSecurityData.Sessions;

        // تصفية حسب المستخدم
        if (userId.HasValue)
            query = query.Where(s => s.UserId == userId.Value);

        // استبعاد الملغاة إذا لم يُطلب عرضها
        if (!includeRevoked)
            query = query.Where(s => !s.IsRevoked);

        return Task.FromResult(Result<IReadOnlyList<UserSessionDto>>.Success(
            query.ToList().AsReadOnly()));
    }

    public Task<Result> RevokeAsync(long sessionId, CancellationToken ct = default)
    {
        var existing = MockSecurityData.Sessions.FirstOrDefault(s => s.Id == sessionId);
        if (existing is null)
            return Task.FromResult(Result.Failure("الجلسة غير موجودة"));

        if (existing.IsRevoked)
            return Task.FromResult(Result.Failure("الجلسة ملغاة بالفعل"));

        var index = MockSecurityData.Sessions.IndexOf(existing);
        MockSecurityData.Sessions[index] = existing with { IsRevoked = true };
        return Task.FromResult(Result.Success());
    }
}
