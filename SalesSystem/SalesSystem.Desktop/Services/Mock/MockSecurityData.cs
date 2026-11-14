using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لوحدة الأمان (المستخدمون + كلمات المرور التجريبية).
/// مبنية على INITIAL_USERS المرجعية.
/// <para>
/// كلمات المرور ليست مخزَّنة في <see cref="Users"/> (لا يوجد هاش في الوضع التجريبي)،
/// بل في مخزن مستقل داخلي — كلمة مرور تجريبية واحدة معروفة لكل حساب أولي —
/// حتى يتمكن المالك من اختبار مسار الرفض «كلمة المرور غير صحيحة» (<c>RULE-N167</c>).
/// </para>
/// </summary>
internal static class MockSecurityData
{
    /// <summary>
    /// كلمة المرور التجريبية الافتراضية لكل حساب أولي — قيمة واحدة معلنة صراحةً في مكانها
    /// الواحد، لا تُكتب حرفياً في أي شاشة أو خدمة.
    /// </summary>
    internal const string DefaultPassword = "123456";

    // ═══════════════════════════════════════════════
    // المستخدمون — Users (6 مستخدمين)
    // ═══════════════════════════════════════════════
    public static List<UserDto> Users { get; } = new()
    {
        // 1. مدير النظام الأعلى (Admin)
        new UserDto(
            Id: 1, UserName: "admin",
            BranchId: null, MustChangePassword: false,
            IsLocked: false, IsActive: true,
            LastLoginAt: new DateTime(2026, 9, 1, 8, 30, 0),
            LoginAttempts: 0, DefaultCashBoxId: 1),

        // 2. كاشير — أحمد علي عبده
        new UserDto(
            Id: 2, UserName: "cashier1",
            BranchId: 1, MustChangePassword: false,
            IsLocked: false, IsActive: true,
            LastLoginAt: new DateTime(2026, 9, 1, 8, 0, 0),
            LoginAttempts: 0, DefaultCashBoxId: 2),

        // 3. محاسب — سامي عبدالملك الجوفي
        new UserDto(
            Id: 3, UserName: "accountant1",
            BranchId: 1, MustChangePassword: false,
            IsLocked: false, IsActive: true,
            LastLoginAt: new DateTime(2026, 8, 31, 14, 15, 0),
            LoginAttempts: 0, DefaultCashBoxId: 1),

        // 4. مشرف مخازن — خالد صالح القاضي
        new UserDto(
            Id: 4, UserName: "warehouse1",
            BranchId: 1, MustChangePassword: true,
            IsLocked: false, IsActive: true,
            LastLoginAt: null,
            LoginAttempts: 0, DefaultCashBoxId: null),

        // 5. مستخدم تجريبي مقفل — locked_test
        new UserDto(
            Id: 5, UserName: "locked_test",
            BranchId: 1, MustChangePassword: false,
            IsLocked: true, IsActive: true,
            LastLoginAt: new DateTime(2026, 8, 30, 10, 0, 0),
            LoginAttempts: 5, DefaultCashBoxId: null),

        // 6. مستخدم تجريبي معطّل — inactive_test
        //    موجود ليصبح رفض «الحساب معطّل» قابلاً للاختبار من شاشة الدخول (RULE-N167):
        //    حسابات المسار السعيد كلها نشطة، فبلا هذا السجل لا يُختبر الرفض الرابع إطلاقاً.
        new UserDto(
            Id: 6, UserName: "inactive_test",
            BranchId: 1, MustChangePassword: false,
            IsLocked: false, IsActive: false,
            LastLoginAt: new DateTime(2026, 8, 25, 9, 0, 0),
            LoginAttempts: 0, DefaultCashBoxId: null),
    };

    // ═══════════════════════════════════════════════
    // كلمات المرور التجريبية — بلا هاش (وضع التجربة فقط)
    // ═══════════════════════════════════════════════

    /// <summary>
    /// مخزن كلمات المرور التجريبية في الذاكرة — مفتاحه اسم المستخدم (بلا حساسية لحالة الأحرف).
    /// يعيش ما دام التطبيق مثل <see cref="Users"/>، فلا تُصفَّر كلمة المرور عند إغلاق الشاشة
    /// وإعادة فتحها، ولا يُصفَّر شيئاً إلا بإعادة تشغيل التطبيق.
    /// </summary>
    private static readonly Dictionary<string, string> Passwords =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// يزرع كلمة المرور التجريبية الافتراضية لكل حساب أولي بعد تهيئة <see cref="Users"/>.
    /// </summary>
    static MockSecurityData()
    {
        foreach (var user in Users)
            Passwords[user.UserName] = DefaultPassword;
    }

    /// <summary>
    /// يقرأ كلمة المرور التجريبية لاسم مستخدم — <c>false</c> للاسم الفارغ أو غير المزروع.
    /// عندها تستخدم خدمة المصادقة الافتراضية <see cref="DefaultPassword"/> (حساب أنشأه المستخدم).
    /// </summary>
    internal static bool TryGetPassword(string? userName, out string password)
    {
        password = DefaultPassword;

        if (string.IsNullOrWhiteSpace(userName)) return false;

        if (!Passwords.TryGetValue(userName.Trim(), out var stored)) return false;

        password = stored;
        return true;
    }

    /// <summary>
    /// يخزّن كلمة مرور جديدة لاسم مستخدم (يُستهلك بعد نجاح تغيير/إعادة تعيين كلمة المرور).
    /// لا يُخزَّن نص فارغ إطلاقاً — تبقى الكلمة السابقة.
    /// </summary>
    internal static void SetPassword(string? userName, string? password)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password)) return;

        Passwords[userName.Trim()] = password;
    }

    /// <summary>
    /// يستبدل سجلاً في <see cref="Users"/> بنسخته المعدَّلة (سجلات <c>UserDto</c> غير قابلة للتعديل)
    /// — البيت الواحد لعملية الاستبدال، فتُعدَّل الحالة في مكان واحد.
    /// </summary>
    internal static void ReplaceUser(UserDto existing, UserDto updated)
    {
        if (existing is null || updated is null) return;

        var index = Users.IndexOf(existing);
        if (index >= 0) Users[index] = updated;
    }
}
