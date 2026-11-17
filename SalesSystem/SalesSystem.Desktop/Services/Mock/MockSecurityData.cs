using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لوحدة الأمان (المستخدمون + كلمات المرور التجريبية).
/// مبنية على INITIAL_USERS المرجعية.
/// <para>
/// كلمات المرور ليست مخزَّنة في <see cref="Users"/> (لا يوجد هاش في الوضع التجريبي)،
/// بل في مخزن واحد داخلي هنا (<see cref="Passwords"/>) — البيت الوحيد لكلمة مرور كل مستخدم،
/// يستهلكه <c>MockUserDesktopService</c> (إنشاء/تعديل/إعادة تعيين/تعطيل) و<c>MockAuthDesktopService</c>
/// (الدخول وتغيير كلمة المرور) معاً (<c>RULE-N169</c>/<c>RULE-N171</c>): فكلمة المرور المكتوبة في
/// <c>frmUserMaster</c> هي بعينها التي يُقبل بها الدخول، ومسار الرفض «كلمة المرور غير صحيحة»
/// يبقى قابلاً للاختبار (<c>RULE-N167</c>).
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
            MustChangePassword: false,
            IsLocked: false, IsActive: true,
            LastLoginAt: new DateTime(2026, 9, 1, 8, 30, 0),
            LoginAttempts: 0, DefaultCashBoxId: 1),

        // 2. كاشير — أحمد علي عبده
        new UserDto(
            Id: 2, UserName: "cashier1",
            MustChangePassword: false,
            IsLocked: false, IsActive: true,
            LastLoginAt: new DateTime(2026, 9, 1, 8, 0, 0),
            LoginAttempts: 0, DefaultCashBoxId: 2),

        // 3. محاسب — سامي عبدالملك الجوفي
        new UserDto(
            Id: 3, UserName: "accountant1",
            MustChangePassword: false,
            IsLocked: false, IsActive: true,
            LastLoginAt: new DateTime(2026, 8, 31, 14, 15, 0),
            LoginAttempts: 0, DefaultCashBoxId: 1),

        // 4. مشرف مخازن — خالد صالح القاضي
        new UserDto(
            Id: 4, UserName: "warehouse1",
            MustChangePassword: true,
            IsLocked: false, IsActive: true,
            LastLoginAt: null,
            LoginAttempts: 0, DefaultCashBoxId: null),

        // 5. مستخدم تجريبي مقفل — locked_test
        new UserDto(
            Id: 5, UserName: "locked_test",
            MustChangePassword: false,
            IsLocked: true, IsActive: true,
            LastLoginAt: new DateTime(2026, 8, 30, 10, 0, 0),
            LoginAttempts: 5, DefaultCashBoxId: null),

        // 6. مستخدم تجريبي معطّل — inactive_test
        //    موجود ليصبح رفض «الحساب معطّل» قابلاً للاختبار من شاشة الدخول (RULE-N167):
        //    حسابات المسار السعيد كلها نشطة، فبلا هذا السجل لا يُختبر الرفض الرابع إطلاقاً.
        new UserDto(
            Id: 6, UserName: "inactive_test",
            MustChangePassword: false,
            IsLocked: false, IsActive: false,
            LastLoginAt: new DateTime(2026, 8, 25, 9, 0, 0),
            LoginAttempts: 0, DefaultCashBoxId: null),
    };

    // ═══════════════════════════════════════════════
    // كلمات المرور التجريبية — بلا هاش (وضع التجربة فقط)
    // ═══════════════════════════════════════════════

    /// <summary>
    /// مخزن كلمات المرور التجريبية في الذاكرة — مفتاحه <c>UserId</c> (المعرّف الثابت للسجل) لا اسم
    /// المستخدم، فتبقى كلمة المرور مرتبطة بالسجل نفسه ولا تتأثر بأي كتابة/تنسيق للاسم.
    /// يعيش ما دام التطبيق مثل <see cref="Users"/>، فلا تُصفَّر كلمة المرور عند إغلاق الشاشة
    /// وإعادة فتحها، ولا يُصفَّر شيء إلا بإعادة تشغيل التطبيق.
    /// </summary>
    private static readonly Dictionary<int, string> Passwords = new();

    /// <summary>
    /// يزرع كلمة المرور التجريبية الافتراضية لكل حساب أولي بعد تهيئة <see cref="Users"/>.
    /// </summary>
    static MockSecurityData()
    {
        foreach (var user in Users)
            Passwords[user.Id] = DefaultPassword;
    }

    /// <summary>
    /// يقرأ كلمة المرور التجريبية لمستخدم بمعرّفه — <c>false</c> للمعرّف غير الصالح أو غير المزروع.
    /// وعندها تستخدم خدمة المصادقة الافتراضية <see cref="DefaultPassword"/> كسقوط آمن.
    /// </summary>
    internal static bool TryGetPassword(int userId, out string password)
    {
        password = DefaultPassword;

        if (userId <= 0) return false;

        if (!Passwords.TryGetValue(userId, out var stored)) return false;

        password = stored;
        return true;
    }

    /// <summary>
    /// يخزّن كلمة مرور لمستخدم بمعرّفه (إنشاء · تعديل · إعادة تعيين)، ويعيد **الكلمة المخزَّنة فعلاً**
    /// حتى تعرض الشاشة القيمة الحقيقية لا قيمة مخمَّنة. النص الفارغ يعني «الكلمة الافتراضية»
    /// (<see cref="DefaultPassword"/>) — لا يبقى الحساب بلا كلمة مرور.
    /// </summary>
    internal static string SetPassword(int userId, string? password)
    {
        var effective = string.IsNullOrWhiteSpace(password) ? DefaultPassword : password;

        if (userId > 0) Passwords[userId] = effective;

        return effective;
    }

    /// <summary>
    /// يزيل كلمة مرور مستخدم (يُستهلك عند تعطيل الحساب)، فيعود القراءة إلى
    /// الافتراضية عند أول كتابة جديدة له.
    /// </summary>
    internal static void RemovePassword(int userId)
    {
        if (userId > 0) Passwords.Remove(userId);
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
