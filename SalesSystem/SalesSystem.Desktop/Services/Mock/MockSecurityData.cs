using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لوحدة الأمان (المستخدمون).
/// مبنية على INITIAL_USERS المرجعية.
/// </summary>
internal static class MockSecurityData
{
    // ═══════════════════════════════════════════════
    // المستخدمون — Users (5 مستخدمين)
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
    };
}
