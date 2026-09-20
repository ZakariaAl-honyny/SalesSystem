# خطة تنفيذ إدارة المستخدمين والصلاحيات — Users & Roles Management
## نظام: SalesSystem_N-Layer | تاريخ: 2026-07-16

---

> **ملاحظة للـ AI Agent المنفذ:** هذا المستند هو المرجع الكامل والوحيد لتنفيذ ميزة إدارة المستخدمين والصلاحيات. اقرأه كاملاً قبل كتابة أي سطر من الكود. كل قسم يحتوي على الملف المستهدف ونوع التغيير المطلوب بدقة.

---

## ملخص الوضع الحالي (What's Already Done ✅)

بعد مراجعة الكود، هذه العناصر **مكتملة وتعمل بشكل صحيح — لا تلمسها:**

| الملف | الحالة | الملاحظة |
|-------|--------|----------|
| `SalesSystem.Models/Role.cs` | ✅ مكتمل | يحتوي على `IsSystem`, `SalesPermissions`, `InventoryPermissions`, `AccountingPermissions`, `SystemPermissions` |
| `SalesSystem.Models/User.cs` | ✅ مكتمل | يحتوي على `RoleId` (short), `BranchId` (short?), `MustChangePassword`, `LoginAttempts` |
| `SalesSystem.Common/Enums/Permissions/SalesPermissions.cs` | ✅ مكتمل | `[Flags] enum` بـ `long` |
| `SalesSystem.Common/Enums/Permissions/InventoryPermissions.cs` | ✅ مكتمل | `[Flags] enum` بـ `long` |
| `SalesSystem.Common/Enums/Permissions/AccountingPermissions.cs` | ⚠️ خطأ namespace | الملف يحتوي على `namespace SalesSystem.Common.Enums;` بدلاً من `SalesSystem.Common.Enums.Permissions;` |
| `SalesSystem.Common/Enums/Permissions/SystemPermissions.cs` | ✅ مكتمل | `[Flags] enum` بـ `long` |
| `SalesSystem.Common/Helpers/PermissionHelper.cs` | ✅ مكتمل | `HasPermission`, `IsSuperAdmin`, `CombinePermissions` |
| `SalesSystem.DataAccess/Configurations/RoleConfiguration.cs` | ✅ مكتمل | ضبط الأعمدة الأربعة وعلاقة `IsSystem` |
| `SalesSystem.DataAccess/Configurations/UserConfiguration.cs` | ✅ مكتمل | `RoleId → Roles(Id)` One-to-Many |
| `SalesSystem.DataAccess/DbSeeder.cs` | ✅ مكتمل | 9 أدوار افتراضية، حساب admin بكلمة مرور `12345678` و `MustChangePassword=true` |
| `SalesSystem.Business/Services/RoleService.cs` | ✅ مكتمل | CRUD كامل مع حماية `IsSystem` |
| `SalesSystem.Business/Services/AuthService.cs` | ✅ مكتمل | يُعيد `LoginResponse` يحتوي على الـ Bitmasks الأربعة + `MustChangePassword` |
| `SalesSystem.API/Attributes/RequirePermissionAttribute.cs` | ✅ مكتمل | فلتر `[RequirePermission("Sales", SalesPermissions.X)]` |
| `SalesSystem.Desktop/Services/SessionManager.cs` | ✅ مكتمل | `HasSalesPermission`, `HasInventoryPermission`, إلخ |
| `SalesSystem.Desktop/Screens/frmLogin.cs` | ✅ مكتمل | يتعامل مع `MustChangePassword` ويفتح `frmPasswordChange` |
| `SalesSystem.Desktop/Screens/Users/Forms/frmPasswordChange.cs` | ⚠️ مشكلة | يفرض حد 6 أحرف على كلمة المرور (يجب إزالته) |
| `SalesSystem.Desktop/Screens/Users/Forms/frmAddUpdateUser.cs` | ✅ مكتمل | `ComboBox` لاختيار `Role` واحد |
| `SalesSystem.Desktop/Screens/Roles/Forms/frmAddUpdateRole.cs` | ✅ مكتمل | `FlowLayoutPanel` + `CheckBoxes` + `Bitwise OR` |

---

## المشاكل المكتشفة التي تحتاج إصلاحاً (Issues Found ❌)

### ⚠️ تحذير 1: خطأ في namespace ملف `AccountingPermissions.cs`
- **الملف:** `SalesSystem.Common/Enums/Permissions/AccountingPermissions.cs`
- **المشكلة:** السطر الأول يقول `namespace SalesSystem.Common.Enums;` لكن يجب أن يكون `namespace SalesSystem.Common.Enums.Permissions;` مثل بقية ملفات الصلاحيات.
- **التأثير:** قد يسبب `using` مكرراً في `DbSeeder.cs` (السطر 9: `using AccountingPermissions = SalesSystem.Common.Enums.AccountingPermissions;` موجود كحل مؤقت).
- **الإصلاح:** تغيير السطر الأول في الملف.

### ⚠️ تحذير 2: حد الأحرف على كلمة المرور (Password Length Restriction)
المستخدم يريد عدم إجبار أي حد أدنى لكلمة المرور. هذه الأماكن تحتاج تعديل:

| الملف | السطر | المشكلة |
|-------|-------|---------|
| `SalesSystem.API/Validators/LoginRequestValidator.cs` | 19 | `.MinimumLength(6)` — يجب الحذف |
| `SalesSystem.API/Validators/AuthRequestValidators.cs` | 16 | `.MinimumLength(8)` — يجب الحذف |
| `SalesSystem.API/Validators/UserRequestValidators.cs` | 32 | `.MinimumLength(8)` — يجب الحذف |
| `SalesSystem.Business/Services/AuthService.cs` | 165 | `if (request.NewPassword.Length < 6)` — يجب الحذف |
| `SalesSystem.Desktop/Screens/Users/Forms/frmPasswordChange.cs` | 47-53 | `if (txtNewPassword.Text.Length < 6)` — يجب الحذف |

### ⚠️ تحذير 3: `frmListUsers.cs` — ⬅ يجب مراجعته
- **الملف:** `SalesSystem.Desktop/Screens/Users/Forms/frmListUsers.cs`
- **المطلوب:** التحقق من أن الشاشة تعرض `RoleName` وليس `RoleId` الرقمي، وأن أزرار التعديل والحذف تعمل بشكل صحيح.
- **حماية مدير النظام:** إذا كان المستخدم المعروض هو حساب `admin` (أو إذا كان `RoleId == 1`)، لا تسمح بحذفه.

### ⚠️ تحذير 4: `frmListRoles.cs` — ⬅ يجب مراجعته
- **الملف:** `SalesSystem.Desktop/Screens/Roles/Forms/frmListRoles.cs`
- **المطلوب:** إذا كان الدور `IsSystem = true`، إخفاء أو تعطيل زر الحذف في الـ `DataGridView` أو قائمة السياق (`ContextMenuStrip`).
- **ملاحظة:** دور "مدير النظام" (`Id = 1`) يجب ألا يُسمح بتعديل اسمه أو صلاحياته أيضاً — `frmAddUpdateRole.cs` تعالجه بالفعل (`btnSave.Enabled = false` عند `IsSystem`).

---

## خطة التنفيذ المفصلة (Detailed Implementation Plan)

---

### الخطوة 1: إصلاح `AccountingPermissions.cs` namespace

**الملف:** `c:\Users\ALlahabi\Desktop\Sales Management System\SalesSystem\SalesSystem_N-Layer\SalesSystem.Common\Enums\Permissions\AccountingPermissions.cs`

**التغيير:** تغيير السطر الأول من:
```csharp
namespace SalesSystem.Common.Enums;
```
إلى:
```csharp
namespace SalesSystem.Common.Enums.Permissions;
```

**بعد الإصلاح:** احذف السطر 9 من `DbSeeder.cs`:
```csharp
// احذف هذا السطر:
using AccountingPermissions = SalesSystem.Common.Enums.AccountingPermissions;
```
وأضف بدلاً منه في أعلى الملف:
```csharp
using SalesSystem.Common.Enums.Permissions;
```

---

### الخطوة 2: إزالة قيود كلمة المرور — API Validators

#### الملف 2أ: `SalesSystem.API/Validators/LoginRequestValidator.cs`
**احذف** السطر الذي يحتوي على `.MinimumLength(6)`:
```csharp
// قبل التعديل:
RuleFor(x => x.Password)
    .NotEmpty().WithMessage("كلمة المرور مطلوبة")
    .MinimumLength(6).WithMessage("كلمة المرور يجب أن تكون 6 أحرف على الأقل");

// بعد التعديل:
RuleFor(x => x.Password)
    .NotEmpty().WithMessage("كلمة المرور مطلوبة");
```

#### الملف 2ب: `SalesSystem.API/Validators/AuthRequestValidators.cs`
**احذف** `.MinimumLength(8)` من `NewPassword`:
```csharp
// قبل التعديل:
RuleFor(x => x.NewPassword)
    .NotEmpty().WithMessage("كلمة المرور الجديدة مطلوبة")
    .MinimumLength(8).WithMessage("كلمة المرور يجب أن تكون 8 أحرف على الأقل");

// بعد التعديل:
RuleFor(x => x.NewPassword)
    .NotEmpty().WithMessage("كلمة المرور الجديدة مطلوبة");
```

#### الملف 2ج: `SalesSystem.API/Validators/UserRequestValidators.cs`
**احذف** `.MinimumLength(8)` من `UpdateUserRequestValidator`:
```csharp
// قبل التعديل:
RuleFor(x => x.Password)
    .MinimumLength(8).WithMessage("كلمة المرور يجب أن تكون 8 أحرف على الأقل")
    .When(x => !string.IsNullOrEmpty(x.Password));

// بعد التعديل:
// احذف الـ RuleFor هذا كاملاً أو اتركه بدون MinimumLength:
RuleFor(x => x.Password)
    .NotEmpty().WithMessage("كلمة المرور لا يمكن أن تكون فارغة إذا تم تقديمها")
    .When(x => x.Password is not null);
```

---

### الخطوة 3: إزالة قيود كلمة المرور — Business Layer

**الملف:** `SalesSystem.Business/Services/AuthService.cs`

**احذف** هذا الكتلة (السطران 165-168):
```csharp
// احذف هذا الكود:
if (request.NewPassword.Length < 6)
{
    return Result.Failure("كلمة المرور يجب أن تكون 6 أحرف على الأقل", ErrorCodes.ValidationError);
}
```

---

### الخطوة 4: إزالة قيود كلمة المرور — Desktop UI

**الملف:** `SalesSystem.Desktop/Screens/Users/Forms/frmPasswordChange.cs`

**احذف** هذه الكتلة (السطور 47-53):
```csharp
// احذف هذا الكود:
if (txtNewPassword.Text.Length < 6)
{
    MessageBox.Show("كلمة المرور يجب أن تكون على الأقل 6 أحرف.", "خطأ في الإدخال",
        MessageBoxButtons.OK, MessageBoxIcon.Warning);
    txtNewPassword.Focus();
    return false;
}
```

---

### الخطوة 5: مراجعة `frmListUsers.cs` وحماية حساب Admin

**الملف:** `SalesSystem.Desktop/Screens/Users/Forms/frmListUsers.cs`

**الأشياء التي يجب التحقق منها:**
1. هل `DataGridView` يعرض عمود `RoleName` (وليس `RoleId` الرقمي)؟
   - إذا كان يعرض الرقم، أضف عمود مخصص يقرأ من `UserDto.RoleName`.
2. هل زر الحذف (أو خيار السياق `ContextMenuStrip`) يتحقق من أن المستخدم ليس `IsSystemAdmin`؟
   - **أضف هذا الشرط:** قبل تنفيذ الحذف، تحقق من أن `selectedUser.RoleId != 1` أو أن `selectedUser.UserName != "admin"`. إذا كان كذلك، أظهر رسالة خطأ ولا تنفذ الحذف.

**كود يجب إضافته في حدث الحذف:**
```csharp
var selectedUser = _users.FirstOrDefault(u => u.Id == selectedId);
if (selectedUser?.RoleId == 1)
{
    MessageBox.Show("لا يمكن حذف حساب مدير النظام.", "محظور",
        MessageBoxButtons.OK, MessageBoxIcon.Warning);
    return;
}
```

---

### الخطوة 6: مراجعة `frmListRoles.cs` وحماية الأدوار النظامية

**الملف:** `SalesSystem.Desktop/Screens/Roles/Forms/frmListRoles.cs`

**الأشياء التي يجب التحقق منها:**
1. هل يوجد زر أو خيار لحذف دور؟ إذا نعم:
   - **أضف هذا الشرط:** تحقق من أن `selectedRole.IsSystem == false` قبل السماح بالحذف.
   - إذا كان `IsSystem == true`، أظهر رسالة خطأ ولا تنفذ الحذف.
2. هل عند النقر على "تعديل" دور نظام، يتم فتح `frmAddUpdateRole` وتعطيل الحفظ؟
   - هذا مطبق بالفعل في `frmAddUpdateRole.cs` (السطر 54: `btnSave.Enabled = false`). ✅

**كود يجب التحقق منه وإضافته:**
```csharp
var selectedRole = _roles.FirstOrDefault(r => r.Id == selectedId);
if (selectedRole?.IsSystem == true)
{
    MessageBox.Show("لا يمكن حذف دور نظام — الدور محمي.", "محظور",
        MessageBoxButtons.OK, MessageBoxIcon.Warning);
    return;
}
```

---

### الخطوة 7: إنشاء Migration جديد (إذا لم تكن قاعدة البيانات محدثة بعد)

**الأمر:**
```powershell
cd "c:\Users\ALlahabi\Desktop\Sales Management System\SalesSystem\SalesSystem_N-Layer"
dotnet ef migrations add AddModulePermissionColumns --project SalesSystem.DataAccess --startup-project SalesSystem.API
dotnet ef database update --project SalesSystem.DataAccess --startup-project SalesSystem.API
```

> **ملاحظة:** إذا كانت قاعدة البيانات موجودة بالفعل وتحتوي على البيانات القديمة، سيقوم `DbSeeder.cs` بإصلاح الأدوار ذات الصلاحيات الصفرية تلقائياً (كود الإصلاح موجود في السطور 222-280 من `DbSeeder.cs`).

---

### الخطوة 8: التحقق النهائي من تكامل الـ API

**تحقق من أن Controllers تستخدم `[RequirePermission]` بشكل صحيح:**

```csharp
// مثال صحيح على Controller المبيعات:
[HttpPost]
[RequirePermission("Sales", (long)SalesPermissions.CreateInvoices)]
public async Task<IActionResult> Create(...)

// مثال صحيح على Controller المخازن:
[HttpGet]
[RequirePermission("Inventory", (long)InventoryPermissions.ViewStock)]
public async Task<IActionResult> GetAll(...)
```

**⚠️ تنبيه:** تحقق من أن `UsersController` و `RolesController` يستخدمان:
```csharp
[RequirePermission("System", (long)SystemPermissions.ManageUsers)]
[RequirePermission("System", (long)SystemPermissions.ManageRoles)]
```

---

## جدول ملخص المهام (Task Summary Table)

| # | الملف | نوع التغيير | الأولوية | الحالة |
|---|-------|------------|----------|--------|
| 1 | `Common/Enums/Permissions/AccountingPermissions.cs` | إصلاح namespace | 🔴 عالية | ❌ ينتظر |
| 2 | `DataAccess/DbSeeder.cs` (سطر 9) | حذف using مؤقت | 🔴 عالية | ❌ ينتظر |
| 3 | `API/Validators/LoginRequestValidator.cs` | إزالة MinimumLength(6) | 🟡 متوسطة | ❌ ينتظر |
| 4 | `API/Validators/AuthRequestValidators.cs` | إزالة MinimumLength(8) | 🟡 متوسطة | ❌ ينتظر |
| 5 | `API/Validators/UserRequestValidators.cs` | إزالة MinimumLength(8) | 🟡 متوسطة | ❌ ينتظر |
| 6 | `Business/Services/AuthService.cs` | إزالة فحص الطول (سطر 165) | 🟡 متوسطة | ❌ ينتظر |
| 7 | `Desktop/Screens/Users/Forms/frmPasswordChange.cs` | إزالة فحص الطول (سطر 47) | 🟡 متوسطة | ❌ ينتظر |
| 8 | `Desktop/Screens/Users/Forms/frmListUsers.cs` | مراجعة وحماية حساب admin | 🟡 متوسطة | ❌ ينتظر |
| 9 | `Desktop/Screens/Roles/Forms/frmListRoles.cs` | حماية الأدوار النظامية | 🟡 متوسطة | ❌ ينتظر |
| 10 | `API/Controllers/UsersController.cs` | التحقق من `[RequirePermission]` | 🟢 منخفضة | ❌ ينتظر |
| 11 | `API/Controllers/RolesController.cs` | التحقق من `[RequirePermission]` | 🟢 منخفضة | ❌ ينتظر |
| 12 | تشغيل `dotnet build` للتحقق من عدم وجود أخطاء | بناء | 🔴 عالية | ❌ ينتظر |

---

## المعمارية الكاملة — للمرجع السريع

### كيف يعمل نظام الصلاحيات (Architecture Overview)

```
تسجيل الدخول (frmLogin)
    │
    ▼
AuthService.LoginAsync()
    │── يجلب User مع Role navigation
    │── يتحقق: IsLocked, IsActive, Password
    │── يعيد LoginResponse مع 4 أعمدة Bitmask:
    │     SalesPermissions, InventoryPermissions,
    │     AccountingPermissions, SystemPermissions
    │
    ▼
SessionManager.Initialize(loginResponse)
    │── يحفظ الـ Bitmasks في الذاكرة
    │── HasSalesPermission(bit) → userMask == -1 || (mask & bit) == bit
    │
    ▼
كل شاشة عند فتحها:
    SessionManager.HasXPermission(flag) → true/false
    button.Visible = SessionManager.HasSalesPermission((long)SalesPermissions.CreateInvoices);
    │
    ▼
كل API Call:
    [RequirePermission("Sales", (long)SalesPermissions.CreateInvoices)]
    يقرأ JWT Claim → نفس منطق الفحص بـ Bitwise AND
```

### هيكل الأدوار الافتراضية (Default Roles — Seeded in DbSeeder)

| Id | الاسم | IsSystem | يمكن تعديله | يمكن حذفه |
|----|-------|----------|-------------|-----------|
| 1 | مدير النظام | ✅ | ❌ | ❌ |
| 2 | مدير | ✅ | ✅ | ❌ |
| 3 | محاسب | ✅ | ✅ | ❌ |
| 4 | أمين صندوق | ✅ | ✅ | ❌ |
| 5 | كاشير | ✅ | ✅ | ❌ |
| 6 | مشرف مخازن | ✅ | ✅ | ❌ |
| 7 | مندوب مبيعات | ✅ | ✅ | ❌ |
| 8 | مراقب | ✅ | ✅ | ❌ |
| 9 | مدير فرع | ✅ | ✅ | ❌ |

> **⚠️ قاعدة:** جميع الأدوار بـ `IsSystem = true` لا يمكن حذفها من الكود (يرمي `DomainException` في `Role.MarkAsDeleted()`). الفرق الوحيد: دور "مدير النظام" (Id=1) لا يمكن تعديل صلاحياته أيضاً (يتم التحقق في `frmAddUpdateRole.cs`). بقية الأدوار يمكن تعديل صلاحياتها.

### الحساب الافتراضي عند التهيئة (Default Admin Account — Seeded)

| الحقل | القيمة |
|-------|--------|
| UserName | `admin` |
| Password | `12345678` (يتم Hash بـ BCrypt) |
| MustChangePassword | `true` (يُجبر على تغييرها أول دخول) |
| RoleId | 1 (مدير النظام) |

### كيف يعمل تغيير كلمة المرور الإجبارية (First Login Flow)

```
1. المستخدم يدخل username + password في frmLogin
2. AuthService يتحقق من البيانات → يعيد MustChangePassword=true
3. frmLogin يرى MustChangePassword=true:
   - إذا IsPasswordless (PasswordHash فارغ): يفتح frmPasswordChange._SetUpdateMode()
     → لا يطلب كلمة المرور الحالية
   - إذا ليس passwordless: يفتح frmPasswordChange._SetSelfServiceMode()
     → يطلب كلمة المرور الحالية
4. المستخدم يدخل كلمة المرور الجديدة (أي كلمة حتى حرف واحد — بدون قيود)
5. API تستدعي AuthService.ChangePasswordAsync() → تُحدث PasswordHash + MustChangePassword=false
6. frmPasswordChange يُعيد DialogResult.OK
7. frmLogin يفتح frmMain (الشاشة الرئيسية)
```

---

## ⛔ أشياء لا يجب فعلها (Do NOT Do)

```
❌ لا تعيد إضافة جدول UserRole أو RolePermission أو Permission — تم حذفها بالتصميم
❌ لا تضع PermissionsMask واحداً في User — المستخدم يرث من Role فقط
❌ لا تجبر المستخدم على حد أدنى لكلمة المرور بعد التعديل
❌ لا تسمح بحذف أي دور بـ IsSystem = true
❌ لا تسمح بتعديل صلاحيات دور "مدير النظام" (Id=1)
❌ لا تضع business logic في Controllers — فقط في Services
❌ لا تستخدم string.IsNullOrEmpty للتحقق من كلمة المرور — string.IsNullOrWhiteSpace أصح
```
