using SalesSystem.DTO;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لوحدة الأمان (الأدوار + المستخدمون + الجلسات).
/// مبنية على INITIAL_ROLES / INITIAL_USERS / INITIAL_SESSIONS المرجعية.
/// </summary>
internal static class MockSecurityData
{
    // ═══════════════════════════════════════════════
    // الأدوار — Roles (9 أدوار نظام)
    // ═══════════════════════════════════════════════
    public static List<RoleDto> Roles { get; } = new()
    {
        new RoleDto(
            Id: 1, Name: "مدير النظام الأعلى", Description: "Super Admin - كامل الصلاحيات غير قابل للحذف أو التعديل",
            IsActive: true, IsSystem: true,
            SalesPermissions: -1, InventoryPermissions: -1,
            AccountingPermissions: -1, SystemPermissions: -1),

        new RoleDto(
            Id: 2, Name: "مدير عام", Description: "Manager - صلاحيات إدارية وتشغيلية واسعة",
            IsActive: true, IsSystem: true,
            SalesPermissions: 255, InventoryPermissions: 255,
            AccountingPermissions: 255, SystemPermissions: 127),

        new RoleDto(
            Id: 3, Name: "محاسب", Description: "Accountant - القيود اليومية، الدليل الشجري، وتقارير الأرباح",
            IsActive: true, IsSystem: true,
            SalesPermissions: 15, InventoryPermissions: 7,
            AccountingPermissions: 255, SystemPermissions: 3),

        new RoleDto(
            Id: 4, Name: "أمين صندوق", Description: "Treasurer - إدارة النقدية وسندات القبض والصرف",
            IsActive: true, IsSystem: true,
            SalesPermissions: 23, InventoryPermissions: 0,
            AccountingPermissions: 59, SystemPermissions: 0),

        new RoleDto(
            Id: 5, Name: "كاشير", Description: "Cashier - شاشة نقطة البيع السريعة وإصدار الفواتير",
            IsActive: true, IsSystem: true,
            SalesPermissions: 119, InventoryPermissions: 1,
            AccountingPermissions: 8, SystemPermissions: 0),

        new RoleDto(
            Id: 6, Name: "مشرف مخازن", Description: "Warehouse Supervisor - بطاقة الأصناف، الدفعات، والتحويلات",
            IsActive: true, IsSystem: true,
            SalesPermissions: 1, InventoryPermissions: 255,
            AccountingPermissions: 0, SystemPermissions: 0),

        new RoleDto(
            Id: 7, Name: "مندوب مبيعات", Description: "Sales Employee - أوامر البيع والعملاء",
            IsActive: true, IsSystem: true,
            SalesPermissions: 115, InventoryPermissions: 3,
            AccountingPermissions: 0, SystemPermissions: 0),

        new RoleDto(
            Id: 8, Name: "مراقب", Description: "Observer - صلاحيات العرض والاستعلام فقط",
            IsActive: true, IsSystem: true,
            SalesPermissions: 5, InventoryPermissions: 3,
            AccountingPermissions: 35, SystemPermissions: 1),

        new RoleDto(
            Id: 9, Name: "مدير فرع", Description: "Branch Manager - صلاحيات كاملة مقيدة بالفرع",
            IsActive: true, IsSystem: true,
            SalesPermissions: 251, InventoryPermissions: 255,
            AccountingPermissions: 255, SystemPermissions: 1),
    };

    // ═══════════════════════════════════════════════
    // المستخدمون — Users (6 مستخدمين)
    // ═══════════════════════════════════════════════
    public static List<UserDto> Users { get; } = new()
    {
        // 1. مدير النظام الأعلى (Admin)
        new UserDto(
            Id: 1, UserName: "admin", RoleId: 1,
            RoleName: "مدير النظام الأعلى",
            BranchId: null, MustChangePassword: false,
            IsLocked: false, IsActive: true,
            LastLoginAt: new DateTime(2026, 9, 1, 8, 30, 0),
            LoginAttempts: 0, DefaultCashBoxId: 1)
        {
            FullName = "مدير النظام الأعلى (Admin)",
            EmployeeId = 7,
            BranchName = "جميع الفروع (المركز الرئيسي)",
            DefaultCashboxName = "الصندوق الرئيسي (الخزينة المركزية)",
            DefaultWarehouseName = "المخزن الرئيسي",
        },

        // 2. كاشير — أحمد علي عبده
        new UserDto(
            Id: 2, UserName: "cashier1", RoleId: 5,
            RoleName: "كاشير",
            BranchId: 1, MustChangePassword: false,
            IsLocked: false, IsActive: true,
            LastLoginAt: new DateTime(2026, 9, 1, 8, 0, 0),
            LoginAttempts: 0, DefaultCashBoxId: 2)
        {
            FullName = "أحمد علي عبده",
            EmployeeId = 1,
            BranchName = "المركز الرئيسي (صنعاء)",
            DefaultCashboxName = "درج كاشير نقطة البيع 1 (الصالة)",
            DefaultWarehouseName = "المخزن الرئيسي",
        },

        // 3. محاسب — سامي عبدالملك الجوفي
        new UserDto(
            Id: 3, UserName: "accountant1", RoleId: 3,
            RoleName: "محاسب",
            BranchId: 1, MustChangePassword: false,
            IsLocked: false, IsActive: true,
            LastLoginAt: new DateTime(2026, 8, 31, 14, 15, 0),
            LoginAttempts: 0, DefaultCashBoxId: 1)
        {
            FullName = "سامي عبدالملك الجوفي",
            EmployeeId = 6,
            BranchName = "المركز الرئيسي (صنعاء)",
            DefaultCashboxName = "الصندوق الرئيسي (الخزينة المركزية)",
            DefaultWarehouseName = "المخزن الرئيسي",
        },

        // 4. مشرف مخازن — خالد صالح القاضي
        new UserDto(
            Id: 4, UserName: "warehouse1", RoleId: 6,
            RoleName: "مشرف مخازن",
            BranchId: 1, MustChangePassword: true,
            IsLocked: false, IsActive: true,
            LastLoginAt: null,
            LoginAttempts: 0, DefaultCashBoxId: null)
        {
            FullName = "خالد صالح القاضي",
            EmployeeId = 3,
            BranchName = "مستودع صنعاء الرئيسي",
            DefaultCashboxName = null,
            DefaultWarehouseName = "المخزن الرئيسي",
        },

        // 5. مستخدم تجريبي مقفل — locked_test
        new UserDto(
            Id: 5, UserName: "locked_test", RoleId: 5,
            RoleName: "كاشير",
            BranchId: 1, MustChangePassword: false,
            IsLocked: true, IsActive: true,
            LastLoginAt: new DateTime(2026, 8, 30, 10, 0, 0),
            LoginAttempts: 5, DefaultCashBoxId: null)
        {
            FullName = "مستخدم تجريبي مقفل",
            EmployeeId = 8,
            BranchName = "فرع عدن",
            DefaultCashboxName = null,
            DefaultWarehouseName = null,
        },
    };

    // ═══════════════════════════════════════════════
    // الجلسات — Sessions (3 جلسات)
    // ═══════════════════════════════════════════════
    public static List<UserSessionDto> Sessions { get; } = new()
    {
        // جلسة 1: مدير النظام — نشطة
        new UserSessionDto(
            Id: 101, UserId: 1, UserName: "admin",
            DeviceName: "POS-SERVER-01 (Windows 11 WinForms)",
            IpAddress: "192.168.1.10",
            CreatedAt: DateTime.UtcNow.AddHours(-8),
            LastActivityAt: DateTime.UtcNow,
            ExpiresAt: DateTime.UtcNow.AddHours(16),
            IsRevoked: false),

        // جلسة 2: كاشير — نشطة
        new UserSessionDto(
            Id: 102, UserId: 2, UserName: "cashier1",
            DeviceName: "POS-CASHIER-PC2 (Terminal 1)",
            IpAddress: "192.168.1.15",
            CreatedAt: DateTime.UtcNow.AddHours(-6),
            LastActivityAt: DateTime.UtcNow.AddMinutes(-5),
            ExpiresAt: DateTime.UtcNow.AddHours(10),
            IsRevoked: false),

        // جلسة 3: محاسب — نشطة
        new UserSessionDto(
            Id: 103, UserId: 3, UserName: "accountant1",
            DeviceName: "FINANCE-PC (Accounting Desk)",
            IpAddress: "192.168.1.22",
            CreatedAt: DateTime.UtcNow.AddHours(-4),
            LastActivityAt: DateTime.UtcNow.AddMinutes(-40),
            ExpiresAt: DateTime.UtcNow.AddHours(8),
            IsRevoked: false),
    };
}
