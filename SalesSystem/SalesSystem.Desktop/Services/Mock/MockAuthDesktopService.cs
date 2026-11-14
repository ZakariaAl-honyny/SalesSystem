// -----------------------------------------------------------------------------
// MockAuthDesktopService.cs — النسخة الوهمية لعقد المصادقة (المرحلة أ)
//
// الوضع التجريبي يعمل بلا قاعدة بيانات وبلا Business/** وبلا HTTP (AGENTS.md §2.16):
// يبحث عن المستخدم في MockSecurityData.Users ويطابق كلمة المرور مع المخزن التجريبي
// في MockSecurityData (كلمة المرور الأولية «123456» لكل حساب — لا هاش في الذاكرة)،
// ثم يعيد جلسة صالحة للبيانات الأولية نفسها.
//
// تكافؤ المحاكاة (RULE-N167) — يرفض ما ترفضه الخدمة الحقيقية/طبقة الأعمال:
// الدخول (بالترتيب):
//   1) اسم مستخدم فارغ                ⇒ «اسم المستخدم مطلوب»
//   2) اسم مستخدم غير موجود            ⇒ «اسم المستخدم غير موجود.»
//   3) حساب مقفل مسبقاً (IsLocked)     ⇒ «الحساب مقفل بعد محاولات دخول فاشلة. رجاءً راجع المدير.»
//   4) حساب معطّل (IsActive=false)      ⇒ «هذا المستخدم معطّل.»
//   5) كلمة مرور فارغة                 ⇒ «يرجى إدخال كلمة المرور.»
//   6) كلمة مرور لا تطابق المخزن       ⇒ «كلمة المرور غير صحيحة.» (تُزيد المحاولات في المخزن)
//      وعند المحاولة الخامسة           ⇒ «تم قفل الحساب بعد 5 محاولات فاشلة. رجاءً راجع المدير.» + IsLocked
//   7) النجاح                          ⇒ تصفير المحاولات + LastLoginAt = الآن + الجلسة
//
// تغيير كلمة المرور — الخدمة الذاتية (المستخدم الحالي نفسه):
//   الحالية فارغة · الحالية لا تطابق المخزن · الجديدة فارغة · الجديدة أقصر من الحد الأدنى
//   (clsValidation.PasswordMinLength) · الجديدة مطابقة للحالية · التأكيد غير مطابق.
//   والنجاح يُخزَّن فعلياً: كلمة المرور الجديدة + MustChangePassword = false + تصفير المحاولات،
//   فلا تعود الشاشة تطلب التغيير في الدخول التالي (RULE-N167).
// إعادة التعيين الإدارية (مستخدم آخر بمستخدم مُسجَّل دخوله): لا تُطلب الحالية (المدير لا يعرفها)،
//   وتُطبَّق قواعد الجديدة/التأكيد ويُخزَّن الناتج نفسه.
//
// ولا استثناء يفلت (RULE-N14): كل خطأ يُسجَّل في Serilog ويُعاد Result فاشل بعربية.
// -----------------------------------------------------------------------------

using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// تنفيذ وهمي (في الذاكرة) لعقد <see cref="IAuthDesktopService"/> — للوضع التجريبي فقط.
/// يستهلك بيانات <see cref="MockSecurityData"/> نفسها التي تستهلكها شاشة الدخول من
/// <c>MockUserDesktopService</c>، فلا يفترق ما يُعرض عمّا يُقبل، ويكتب فيها كلمة المرور
/// الجديدة وحالة <c>MustChangePassword</c> بعد النجاح.
/// </summary>
public sealed class MockAuthDesktopService : IAuthDesktopService
{
    /// <summary>رسالة رفض الدخول بلا اسم مستخدم — مطابقة لقاعدة طبقة الأعمال.</summary>
    private const string MissingUserNameMessage = "اسم المستخدم مطلوب";

    /// <summary>رسالة رفض اسم مستخدم غير موجود في البيانات الأولية.</summary>
    private const string UnknownUserMessage = "اسم المستخدم غير موجود.";

    /// <summary>رسالة رفض الحساب المقفل بعد المحاولات الفاشلة.</summary>
    private const string LockedUserMessage = "الحساب مقفل بعد محاولات دخول فاشلة. رجاءً راجع المدير.";

    /// <summary>رسالة رفض الحساب المعطّل.</summary>
    private const string InactiveUserMessage = "هذا المستخدم معطّل.";

    /// <summary>رسالة رفض كلمة المرور الفارغة — الدخول بلا كلمة مرور مرفوض دائماً.</summary>
    private const string MissingPasswordMessage = "يرجى إدخال كلمة المرور.";

    /// <summary>رسالة رفض كلمة المرور التي لا تطابق المخزن التجريبي.</summary>
    private const string WrongPasswordMessage = "كلمة المرور غير صحيحة.";

    /// <summary>رسالة رفض كلمة المرور الحالية الفارغة عند التغيير.</summary>
    private const string MissingCurrentPasswordMessage = "كلمة المرور الحالية مطلوبة.";

    /// <summary>رسالة رفض كلمة المرور الحالية التي لا تطابق المخزن التجريبي.</summary>
    private const string WrongCurrentPasswordMessage = "كلمة المرور الحالية غير صحيحة.";

    /// <summary>رسالة رفض كلمة المرور الجديدة الفارغة — مطابقة لقاعدة طبقة الأعمال.</summary>
    private const string MissingNewPasswordMessage = "كلمة المرور الجديدة مطلوبة";

    /// <summary>رسالة رفض المستخدم غير الموجود في البيانات الأولية.</summary>
    private const string UnknownUserIdMessage = "المستخدم غير موجود.";

    /// <summary>رسالة رفض طلب تغيير كلمة مرور غير صالح (وصول دفاعي — لا تُصلها الشاشات).</summary>
    private const string InvalidRequestMessage = "طلب تغيير كلمة المرور غير صالح.";

    /// <summary>رسالة رفض كلمة مرور جديدة مطابقة للحالية.</summary>
    private const string SameNewPasswordMessage = "كلمة المرور الجديدة يجب أن تختلف عن الحالية.";

    /// <summary>رسالة رفض تأكيد كلمة المرور غير المطابق.</summary>
    private const string PasswordMismatchMessage = "كلمة المرور غير متطابقة.";

    /// <summary>حد المحاولات الفاشلة قبل القفل (RULE-N37).</summary>
    private const int MaxFailedAttempts = 5;

    /// <summary>مدة الجلسة الوهمية بالساعات.</summary>
    private const int SessionHours = 8;

    /// <summary>
    /// رسالة رفض كلمة المرور الجديدة الأقصر من الحد الأدنى — النص من بيت التحقق الواحد
    /// (<see cref="clsValidation.PasswordMinLength"/>) فلا رقم مكرّر (RULE-N172).
    /// </summary>
    private static readonly string ShortNewPasswordMessage =
        $"كلمة المرور الجديدة يجب ألا تقل عن {clsValidation.PasswordMinLength} خانات.";

    /// <summary>رسالة قفل الحساب عند بلوغ حد المحاولات الفاشلة — الرقم من الثابت وحده.</summary>
    private static readonly string LockedByAttemptsMessage =
        $"تم قفل الحساب بعد {MaxFailedAttempts} محاولات فاشلة. رجاءً راجع المدير.";

    /// <summary>الجلسة الحالية — لتمييز الخدمة الذاتية عن إعادة التعيين الإدارية.</summary>
    private readonly ISessionService _sessionService;

    /// <summary>يستقبل خدمة الجلسة من الحاوية — لا إنشاء يدوي (RULE-N03).</summary>
    public MockAuthDesktopService(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    /// <summary>
    /// دخول تجريبي: يتحقق من وجود المستخدم وحالته، ثم يطابق كلمة المرور مع المخزن التجريبي
    /// (لا يقبل أي كلمة مرور)، ويُحدّث المحاولات/آخر دخول في <see cref="MockSecurityData"/>.
    /// </summary>
    public Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        try
        {
            if (request is null || string.IsNullOrWhiteSpace(request.UserName))
                return Task.FromResult(Result<LoginResponse>.Failure(MissingUserNameMessage));

            var userName = request.UserName.Trim();

            var user = MockSecurityData.Users
                .FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (user is null)
                return Task.FromResult(Result<LoginResponse>.Failure(UnknownUserMessage, ErrorCodes.NotFound));

            if (user.IsLocked)
                return Task.FromResult(Result<LoginResponse>.Failure(LockedUserMessage, ErrorCodes.AccountLocked));

            if (!user.IsActive)
                return Task.FromResult(Result<LoginResponse>.Failure(InactiveUserMessage, ErrorCodes.Unauthorized));

            if (string.IsNullOrWhiteSpace(request.Password))
                return Task.FromResult(Result<LoginResponse>.Failure(MissingPasswordMessage, ErrorCodes.ValidationError));

            // الحساب الذي أنشأه المستخدم من الدليل لا سجل له في المخزن ⇒ كلمة المرور الافتراضية.
            var storedPassword = MockSecurityData.TryGetPassword(user.UserName, out var stored)
                ? stored
                : MockSecurityData.DefaultPassword;

            if (!string.Equals(request.Password, storedPassword, StringComparison.Ordinal))
                return RejectWrongPassword(user);

            MockSecurityData.ReplaceUser(user, user with { LoginAttempts = 0, LastLoginAt = DateTime.Now });

            var response = new LoginResponse(
                UserId: user.Id,
                UserName: user.UserName,
                IsAdmin: string.Equals(user.UserName, "admin", StringComparison.OrdinalIgnoreCase),
                BranchId: user.BranchId,
                Token: $"mock-token-{user.Id:D4}",
                ExpiresAt: DateTime.UtcNow.AddHours(SessionHours),
                MustChangePassword: user.MustChangePassword,
                IsPasswordless: false);

            return Task.FromResult(Result<LoginResponse>.Success(response));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "MockAuthDesktopService.LoginAsync failed");
            return Task.FromResult(Result<LoginResponse>.Failure("حدث خطأ غير متوقع أثناء تسجيل الدخول."));
        }
    }

    /// <summary>
    /// تغيير كلمة المرور للمستخدم المُسجَّل دخوله (من <see cref="ISessionService"/> — لا
    /// <c>userId: 1</c> ثابتاً) — خدمة ذاتية تُلزم كلمة المرور الحالية.
    /// </summary>
    public Task<Result> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default)
        => ChangePasswordAsync(request, _sessionService.Current?.UserId ?? 0, ct);

    /// <summary>
    /// تغيير/إعادة تعيين كلمة المرور لمستخدم محدد.
    /// <para>
    /// <b>خدمة ذاتية</b> (<paramref name="userId"/> هو المستخدم الحالي، أو لا توجد جلسة أصلاً):
    /// كلمة المرور الحالية مطلوبة ويجب أن تطابق المخزن.
    /// </para>
    /// <para>
    /// <b>إعادة تعيين إدارية</b> (مستخدم آخر والجلسة لمستخدم مُسجَّل دخوله): لا تُطلب الحالية —
    /// المدير لا يعرفها — وتبقى قواعد الجديدة/التأكيد إلزامية.
    /// </para>
    /// والنجاح يُخزَّن فعلياً في <see cref="MockSecurityData"/> (كلمة المرور الجديدة +
    /// <c>MustChangePassword = false</c> + تصفير المحاولات)، فلا تعود الشاشة تطلب التغيير.
    /// </summary>
    public Task<Result> ChangePasswordAsync(ChangePasswordRequest request, int userId, CancellationToken ct = default)
    {
        try
        {
            if (request is null)
                return Task.FromResult(Result.Failure(InvalidRequestMessage, ErrorCodes.ValidationError));

            var sessionUserId = _sessionService.Current?.UserId;

            // لا توجد جلسة ⇒ لا دليل على صلاحية إدارية ⇒ تُعامَل كخدمة ذاتية (أصرم، لا أوسع).
            var isSelfService = sessionUserId is null || userId == sessionUserId.Value;

            var user = MockSecurityData.Users.FirstOrDefault(u => u.Id == userId);
            if (user is null)
                return Task.FromResult(Result.Failure(UnknownUserIdMessage, ErrorCodes.NotFound));

            if (isSelfService)
            {
                if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                    return Task.FromResult(Result.Failure(MissingCurrentPasswordMessage, ErrorCodes.ValidationError));

                var storedPassword = MockSecurityData.TryGetPassword(user.UserName, out var stored)
                    ? stored
                    : MockSecurityData.DefaultPassword;

                if (!string.Equals(request.CurrentPassword, storedPassword, StringComparison.Ordinal))
                    return Task.FromResult(Result.Failure(WrongCurrentPasswordMessage, ErrorCodes.ValidationError));
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
                return Task.FromResult(Result.Failure(MissingNewPasswordMessage, ErrorCodes.ValidationError));

            if (request.NewPassword.Length < clsValidation.PasswordMinLength)
                return Task.FromResult(Result.Failure(ShortNewPasswordMessage, ErrorCodes.ValidationError));

            if (isSelfService && string.Equals(request.NewPassword, request.CurrentPassword, StringComparison.Ordinal))
                return Task.FromResult(Result.Failure(SameNewPasswordMessage, ErrorCodes.ValidationError));

            if (!string.Equals(request.ConfirmPassword, request.NewPassword, StringComparison.Ordinal))
                return Task.FromResult(Result.Failure(PasswordMismatchMessage, ErrorCodes.ValidationError));

            // الحفظ الحقيقي في الذاكرة: الكلمة الجديدة + إغلاق إلزام التغيير + تصفير المحاولات.
            MockSecurityData.SetPassword(user.UserName, request.NewPassword);
            MockSecurityData.ReplaceUser(user, user with { MustChangePassword = false, LoginAttempts = 0 });

            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "MockAuthDesktopService.ChangePasswordAsync failed for userId {UserId}", userId);
            return Task.FromResult(Result.Failure("حدث خطأ غير متوقع أثناء تغيير كلمة المرور."));
        }
    }

    /// <summary>
    /// يرفض كلمة مرور خاطئة: يُزيد <c>LoginAttempts</c> في المخزن ويقفل الحساب عند بلوغ
    /// <see cref="MaxFailedAttempts"/> — سلوك <c>RULE-N37</c> نفسه، والرسالة تتبع المحاولة.
    /// </summary>
    private static Task<Result<LoginResponse>> RejectWrongPassword(UserDto user)
    {
        var attempts = (short)Math.Min(user.LoginAttempts + 1, MaxFailedAttempts);
        var shouldLock = attempts >= MaxFailedAttempts;

        MockSecurityData.ReplaceUser(user, user with
        {
            LoginAttempts = attempts,
            IsLocked = user.IsLocked || shouldLock
        });

        return Task.FromResult(Result<LoginResponse>.Failure(
            shouldLock ? LockedByAttemptsMessage : WrongPasswordMessage,
            shouldLock ? ErrorCodes.AccountLocked : ErrorCodes.Unauthorized));
    }
}
