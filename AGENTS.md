# AGENTS.md — Sales Management System (v5.0.0 — 2-Tier WinForms Edition)
# READ THIS FILE FIRST — BEFORE WRITING ANY CODE
# Platform: .NET 10 | 2-Tier Architecture
# WinForms Desktop + Business Logic Layer + SQL Server (no HTTP/API layer)

---

## 1. Project Overview

A local desktop sales management system for small retail shops.

**Solution Structure (7 Projects):**

```text
SalesSystem/
├── SalesSystem.Common/       ← Base entities + Enums (18) + Exceptions
├── SalesSystem.Models/       ← Domain entities/Models (~54 entities)
├── SalesSystem.DTO/          ← DTOs + Requests + Responses + Result<T>
├── SalesSystem.DataAccess/   ← EF Core + DbContext + Repositories + Migrations
├── SalesSystem.Business/     ← Business Logic Services (57 services)
├── SalesSystem.Desktop/      ← WinForms Desktop (frm* forms)
└── SalesSystem.Tests/        ← Tests
```

> **No API project.** The former `SalesSystem.API/` (ASP.NET Core + Controllers + Validators) has been removed. The solution is 2-Tier: the Desktop host calls BLL services in-process.

**Data Flow (NEVER break this chain):**

```text
Desktop (WinForms) → BLL Services → DataAccess (EF Core) → SQL Server
```

The Desktop never accesses the database directly (no `DbContext`/EF/SQL in the Desktop project); it always calls Business Layer services in-process.

**Mock mode (UI-first testing):** when `AppSettings.UseMockData = true`, each `I*ApiService` is implemented by a `Mock*ApiService` (in `Services/Mock/`) backed by in-memory `MockData` — no database required. When `false`, the same interface is implemented by a `Direct*Service` (in `Services/Direct/`) that calls the BLL in-process.

---

## 2. CONSTITUTION — Non-Negotiable Rules

### 2.1 Money and Quantity Types

| RULE | WHAT TO DO | WHAT NOT TO DO |
|------|-----------|----------------|
| RULE-001 | Use `decimal(18,2)` for ALL money | NEVER use `float`, `double`, `real`, or SQL `money` |
| RULE-002 | Use `decimal(18,3)` for ALL quantities | NEVER use `int` for quantities |
| RULE-009 | Use `decimal` in SQL Server | NEVER use `float`, `real`, or `money` in SQL |

### 2.2 Financial Formulas

```csharp
// Sales LineTotal — compute inside SalesInvoiceLine entity
LineTotal = (Quantity * UnitPrice) - DiscountAmount;

// Purchase LineTotal — compute inside PurchaseInvoiceLine entity
LineTotal = (Quantity * UnitCost) - DiscountAmount;

// Invoice totals — compute inside Invoice entity
SubTotal    = Items.Sum(i => i.LineTotal);
NetTotal    = SubTotal - InvoiceDiscount + TaxAmount + OtherCharges;
RemainingAmount = NetTotal - PaidAmount;
```

### 2.3 Database Transactions

ALL financial operations inside `BeginTransactionAsync`. Use `IUnitOfWork` for multi-table atomicity.

### 2.4 Invoice Lifecycle

```text
Draft (1) → Posted (2) → Cancelled (3)
```

| RULE | DIRECTIVE |
|------|-----------|
| RULE-004 | NO hard delete for invoices — use `Status = Cancelled` |
| RULE-019 | Invoice states: Draft=1, Posted=2, Cancelled=3 |

### 2.5 Result Pattern

| RULE | DIRECTIVE |
|------|-----------|
| RULE-006 | ALL service methods return `Result<T>` or `Result` |

**Result<T> pattern (in SalesSystem.DTO.Common):**
```csharp
public class Result<T> : Result
{
    public T? Value { get; private set; }
    public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
    public static new Result<T> Failure(string error, string? errorCode = null) => new Result<T> { IsSuccess = false, Error = error, ErrorCode = errorCode };
}
```

### 2.6 Architecture Boundaries

| RULE | DO THIS | DON'T DO THIS |
|------|---------|---------------|
| RULE-007 | Desktop screens call the BLL in-process — through a `Direct*Service` wrapper, never `HttpClient` | Desktop NEVER touches `DbContext`/EF/SQL directly |
| RULE-022 | `Direct*Service` wrappers translate DTOs only — all business logic lives in BLL services | `Direct*Service` NEVER contains business logic |
| RULE-024 | Use `IUnitOfWork` for multi-table ops | NEVER use raw DbContext in Services |

### 2.7 Stock Integrity

`WarehouseStocks.Quantity` has `CHECK (Quantity >= 0)`. Record EVERY stock change in `InventoryTransactions`.

### 2.8 Desktop WinForms Patterns (2-Tier)

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N01 | ALL WinForms use `frm*` naming convention — NEVER `*View` or `*Window` |
| RULE-N02 | Forms are resolved via DI (`Program.ServiceProvider.GetRequiredService<T>()`) — NEVER `new frmXxx()` |
| RULE-N03 | Forms receive `IServiceProvider` via constructor injection and resolve services with `_serviceProvider.GetRequiredService<T>()` |
| RULE-N04 | Resolved services are exposed as typed properties (e.g., `private ICustomerApiService ApiService => _serviceProvider.GetRequiredService<ICustomerApiService>();`) |
| RULE-N05 | `I*ApiService` interfaces live in `Services/Interfaces/`; real implementations (`Direct*Service`) live in `Services/Direct/`, mock implementations (`Mock*ApiService`) in `Services/Mock/` |
| RULE-N06 | `Direct*Service` classes call BLL services in-process via constructor injection (e.g. `SalesSystem.Business.Interfaces.Services.ICustomerService`) — NEVER `HttpClient`, never HTTP |
| RULE-N07 | Global exception handlers in `Program.cs`: `Application.ThreadException` + `AppDomain.CurrentDomain.UnhandledException` |
| RULE-N08 | `MessageBox.Show` is the standard user feedback mechanism — use `MessageBoxIcon.Error`/`Information`/`Warning` appropriately |
| RULE-N09 | Forms are shown modally via `ShowDialog()` — NEVER non-modal for data-entry screens |
| RULE-N10 | Every `I*ApiService` interface is registered in the `Program.cs` DI container — mapped to `Mock*ApiService` (mock mode) or `Direct*Service` (real mode) |

**Service Resolution Pattern:**
```csharp
// SalesSystem.Desktop/Screens/Customers/Forms/frmAddUpdateCustomer.cs
public partial class frmAddUpdateCustomer : Form
{
    private readonly IServiceProvider _serviceProvider;

    public frmAddUpdateCustomer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    private ICustomerApiService ApiService
        => _serviceProvider.GetRequiredService<ICustomerApiService>();
}
```

**Program.cs DI Registration:**
```csharp
// DataAccess + Business Logic (2-Tier: Desktop calls BLL in-process)
services.AddDbContext<SalesDbContext>(options =>
    options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<BizInterfaces.ICustomerService, BizServices.CustomerService>();
// ... all BLL services

// Mock / Real service toggle (switch UseMockData in appsettings.json)
if (appSettings.UseMockData)
{
    services.AddTransient<ICustomerApiService, MockCustomerApiService>();
    services.AddTransient<IProductApiService, MockProductApiService>();
    services.AddTransient<IWarehouseApiService, MockWarehouseApiService>();
    services.AddTransient<ISalesInvoiceApiService, MockSalesInvoiceApiService>();
    services.AddTransient<ISettingsApiService, MockSettingsApiService>();
    services.AddScoped<BizInterfaces.IAuthService, MockAuthService>();
}
else
{
    services.AddTransient<ICustomerApiService, DirectCustomerService>();
    services.AddTransient<IProductApiService, DirectProductService>();
    services.AddTransient<IWarehouseApiService, DirectWarehouseService>();
    services.AddTransient<ISalesInvoiceApiService, DirectSalesInvoiceService>();
    services.AddTransient<ISettingsApiService, DirectSettingsService>();
    services.AddScoped<BizInterfaces.IAuthService, BizServices.AuthService>();
}

// Forms
services.AddTransient<frmLogin>();
services.AddTransient<frmMain>();
// ... all forms
```

### 2.9 Direct BLL Pattern

There are no HTTP controllers, no routes, no JWT policies to configure for data screens. The Desktop communicates with the backend through thin `Direct*Service` wrappers.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N11 | `Direct*Service` implements the `I*ApiService` interface and delegates to a single BLL service — no business logic of its own |
| RULE-N12 | `Direct*Service` returns the BLL's `Result<T>` unchanged (or maps paged results to the interface's shape) — never rewraps error messages |
| RULE-N13 | Every method passes `CancellationToken` through and supplies the current `UserId` from `ISessionService.Current?.UserId` where the BLL requires it |
| RULE-N14 | Exceptions never escape a `Direct*Service` — BLL returns `Result<T>`; unexpected exceptions are returned as `Result.Failure` with an Arabic message |
| RULE-N15 | FluentValidation (where used) lives in the BLL/validators — there is no API pipeline to run it |

### 2.10 Entity Base Classes

| Class | Id | CreatedAt | CreatedByUserId | UpdatedAt | UpdatedByUserId | IsActive | Status |
|-------|----|-----------|-----------------|-----------|-----------------|----------|--------|
| **Entity** | int PK | — | — | — | — | — | — |
| **AuditableEntity** | int PK | datetime2 | int null | datetime2 null | int null | — | — |
| **ActivatableEntity** | int PK | datetime2 | int null | datetime2 null | int null | bit default 1 | — |
| **DocumentEntity** | int PK | datetime2 | int null | datetime2 null | int null | — | tinyint |
| **LongEntity** | bigint PK | — | — | — | — | — | — |

### 2.11 NuGet Packages

| Package | Project |
|---------|---------|
| `Microsoft.EntityFrameworkCore.SqlServer` 10.x | DataAccess |
| `Microsoft.EntityFrameworkCore.Tools` 10.x | DataAccess |
| `BCrypt.Net-Next` 4.x | DataAccess |
| `Serilog.Sinks.File` 5.x | Desktop |
| `System.Text.Json` 10.x | Desktop |
| `ClosedXML` 0.102.x | Desktop (Excel export) |
| `QuestPDF` 2024.3.x | DataAccess (A4 PDF) |
| `SixLabors.ImageSharp` 3.1.x | DataAccess |
| `System.Drawing.Common` 10.x | DataAccess |
| `Microsoft.Extensions.Hosting` 10.x | Desktop |
| `Microsoft.AspNetCore.DataProtection` 10.x | DataAccess |

> The API-only packages (`FluentValidation.AspNetCore`, `Serilog.AspNetCore`, `Microsoft.AspNetCore.Authentication.JwtBearer`, `Swashbuckle.AspNetCore`, `Microsoft.Extensions.Hosting.WindowsServices`, `Serilog.Sinks.EventLog`) and `Microsoft.Extensions.Http` were removed with the API project.

### 2.12 WinForms UI Conventions

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N20 | ALL forms use `RightToLeft = Yes` and `RightToLeftLayout = true` for Arabic UI |
| RULE-N21 | ALL forms use `Tahoma` font family |
| RULE-N22 | DataGridView with `AllowUserToAddRows = false`, `ReadOnly = true`, `SelectionMode = FullRowSelect` |
| RULE-N22a | استثناء موثّق ومقصود: شبكة إدخال سطور الفاتورة `dgvItems` في `frmAddUpdateSalesInvoice` تستخدم `AllowUserToAddRows = true` و `SelectionMode = CellSelect` وتحريراً داخل الخلية (إدخال مباشر بالباركود/التحرير السريع) | `RULE-N22` تبقى إلزامية لكل شبكات العرض والقوائم والتقارير؛ الاستثناء لا يُعمَّم على أي شبكة أخرى |
| RULE-N23 | ContextMenuStrip on DataGridView for row operations |
| RULE-N24 | Filter pattern: `ComboBox` (cbFilterBy) + `TextBox` (txtFilterValue) with `DataView.RowFilter` |
| RULE-N25 | Loading data: `async void` handlers with try/catch and `MessageBox.Show` on error |
| RULE-N26 | Cursor feedback: `Cursor = Cursors.WaitCursor` before async operations, restore in finally |
| RULE-N27 | Form pattern: Header label (lblMode, Dock=Top, colored) + Filter bar + DataGridView + Footer buttons |
| RULE-N28 | All user-facing messages in Arabic |
| RULE-N29 | Error messages log via Serilog, show `MessageBox.Show` to user |
| RULE-N30 | Forms handle dispose pattern: `components?.Dispose()` in `Dispose(bool)` |

> **إلزامي:** أي نموذج جديد (أو نموذج يُعاد بناؤه) يجب أن يجتاز قائمة الإلزام في §2.21 قبل اعتباره مكتملاً.

### 2.13 Users & Permissions (2-Tier)

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N31 | `User` entity uses `PermissionsMask` (bigint) for bitwise permission flags — `-1` = Super Admin |
| RULE-N32 | Roles are templates stored in `Role` entity — assigning a role copies `PermissionsMask` to the user |
| RULE-N33 | 9 seeded roles: Admin(1), Manager(2), Accountant(3), Treasurer(4), Cashier(5), WarehouseSupervisor(6), SalesEmployee(7), Observer(8), BranchManager(9) |
| RULE-N34 | `UserRole` enum in `SalesSystem.Common.Enums` with all 9 roles |
| RULE-N35 | `User.Create()` is passwordless — `MustChangePassword = true` by default |
| RULE-N36 | Password hashing via BCrypt (work factor 12) in service layer |
| RULE-N37 | Lockout after 5 failed login attempts (`LoginAttempts >= 5 → IsLocked = true`) |

### 2.14 Desktop Service Interface Pattern (`I*ApiService` → `Direct*Service`)

Interfaces are **unchanged** by the shift to 2-Tier. The name `I*ApiService` is historical; the implementation is now a `Direct*Service` wrapper over the BLL (not HTTP).

```csharp
// Interface (Services/Interfaces/) — unchanged
public interface IUserApiService
{
    Task<Result<List<UserDto>>> GetAllAsync();
    Task<Result<UserDto>> GetByIdAsync(int id);
    Task<Result<UserDto>> CreateAsync(CreateUserRequest request);
    Task<Result<UserDto>> UpdateAsync(int id, UpdateUserRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> ResetPasswordAsync(int id, ResetPasswordRequest request);
}
```

```csharp
// Implementation (Services/Direct/) — calls the BLL in-process
using SalesSystem.Desktop.Services.Interfaces;
using ISessionService = SalesSystem.Desktop.Services.Interfaces.ISessionService;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

public class DirectCustomerService : ICustomerApiService
{
    private readonly BizInterfaces.ICustomerService _customers;
    private readonly ISessionService _session;

    public DirectCustomerService(BizInterfaces.ICustomerService customers, ISessionService session)
    {
        _customers = customers;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<CustomerDto>>> GetAllAsync(
        string? search = null, bool includeInactive = false, CancellationToken ct = default)
    {
        var result = await _customers.GetAllAsync(search, 1, 1000, includeInactive, ct);
        return result.IsSuccess
            ? Result<IReadOnlyList<CustomerDto>>.Success(result.Value!.Items.ToList())
            : Result<IReadOnlyList<CustomerDto>>.Failure(result.Error!, result.ErrorCode);
    }

    public Task<Result<CustomerDto>> GetByIdAsync(int id, CancellationToken ct = default)
        => _customers.GetByIdAsync(id, ct);

    public Task<Result<CustomerDto>> CreateAsync(CreateCustomerRequest r, CancellationToken ct = default)
        => _customers.CreateAsync(r, UserId, ct);

    public Task<Result<CustomerDto>> UpdateAsync(int id, UpdateCustomerRequest r, CancellationToken ct = default)
        => _customers.UpdateAsync(id, r, UserId, ct);

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
        => _customers.DeleteAsync(id, UserId, ct);

    // ReactivateAsync / other members map to the remaining ICustomerService operations
}
```

> BLL interfaces live in `SalesSystem.Business.Interfaces.Services` (Aliased as `BizInterfaces`). Implementations are registered as `Scoped` BLL services in `Program.cs`.

### 2.16 Mock Data / UI-First Testing

The Desktop app supports a mock-data mode so the UI can be built and tested without a database.

| Aspect | Detail |
|--------|--------|
| Flag | `AppSettings.UseMockData` in `SalesSystem.Desktop/appsettings.json` (default `true`) |
| Mock implementations | `SalesSystem.Desktop/Services/Mock/` — `MockAuthService` plus mock implementations of the `I*ApiService` interfaces (`MockCustomerApiService`, `MockProductApiService`, `MockWarehouseApiService`, `MockSalesInvoiceApiService`, `MockSettingsApiService`) and shared in-memory `MockData` |
| Switching | Set `UseMockData` to `true` (mock) or `false` (real BLL/DB) in `appsettings.json`; `Program.cs` maps each `I*ApiService` to `Mock*ApiService` or `Direct*Service` at startup |
| Indicator | `frmMain` appends `وضع التجربة (Mock)` to the title bar when mock mode is active |
| Login | In mock mode, login accepts any credentials (no database required) |

**Workflow rule:** Test UI/UX with mock data **before** wiring the real BLL (UI-first). Mock mode requires no database and no server running.

### 2.17 مراحل التنفيذ (Phases)

خطة تطوير الواجهة تُنفَّذ على ثلاث مراحل، ولكل مرحلة معيار إغلاق قابل للتحقق:

| المرحلة | النطاق | معيار الإغلاق |
|---|---|---|
| أ — الدين المعماري | إغلاق `RULE-N05`/`RULE-N06` بالكامل: كل شاشة في `Desktop/Screens/**` تتحدث مع BLL **فقط** عبر `I*ApiService` → `Direct*Service` | `grep -r "SalesSystem.Business" SalesSystem/SalesSystem.Desktop/Screens` = **0 نتائج**، و`HttpClient` = 0، و`dotnet build` = 0 أخطاء |
| ب — UX دليل الأصناف | `frmProductMaster` + `frmProductUnitMaster`: مؤشر وضع، آلة حالة الأزرار، تنقل لوحة المفاتيح، شبكة أغنى، تحقق دقيق | البنود B1–B11 من خطة التحسين منفَّذة ومُختبرة يدوياً |
| ج — شاشة فاتورة البيع | تثبيت استثناء `RULE-N22a` + تحسينات متقدمة (تفقيط حيّ داخل تحرير الخلية عبر `EditingControlShowing`) | الاستثناء موثّق هنا + الشاشة تعمل بوضعي Mock/Real |

> **قاعدة:** لا تُفتح مرحلة جديدة قبل إغلاق سابقتها، ولا يُعاد فتح مرحلة مغلقة إلا بقرار صريح من المالك.

### 2.18 معيار لوحة المفاتيح والتركيز (Keyboard & Focus Standard)

كل شاشة إدخال/دليل (Master) يجب أن تلتزم بهذه القواعد، وتُستدعى الأدوات من `KeyboardHelper` وليس بكود مكرر داخل الشاشة:

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N40 | كل شاشة إدخال تستدعي الأربعة معاً في المُنشئ: `WireEnterAsTab` + `WireSelectAllOnFocus` + `WireFocusHighlight` + `WireArrowNavigation` — الاستثناء الوحيد: شاشات العرض للقراءة فقط |
| RULE-N41 | **تحديد القيمة الحالية:** عند دخول المؤشر إلى أي حقل نصي/رقمي تُحدَّد قيمته بالكامل، ويجب أن يعمل ذلك **بالفأرة وبلوحة المفاتيح معاً** (النقر بالفأرة يجب أن يحدد القيمة أيضاً، دون إلغاء إمكانية وضع المؤشر أو تحديد جزء بالسحب) |
| RULE-N42 | `Enter` في حقل الباركود = بحث فوري: إن كان الباركود مسجّلاً يُحمَّل السجل للتعديل ويُحدَّد سطره في الشبكة؛ وإن كان جديداً يبقى الكود في الحقل وينتقل المؤشر إلى حقل الاسم (إدخال سريع بالسكانر) |
| RULE-N43 | آلة حالة الأزرار: «جديد/حفظ» مفعّلان في وضع الإضافة، و«تعديل/حذف» مفعّلان فقط عند وجود سجل محدد؛ ومعطّلة = رمادية بنص باهت (`WireButtonDisabledVisual`) وليس فقط `Enabled = false` |
| RULE-N44 | مؤشر الوضع إلزامي: `lblMode` أعلى الشاشة يوضّح «إضافة … جديد» أو «تعديل …: <الاسم>»، ويتحدث مع كل تغيير للحالة |
| RULE-N45 | أسهم RTL بين الحقول: يمين/أعلى = الحقل السابق، يسار/أسفل = الحقل التالي؛ ولا تُطبَّق على `ComboBox` ولا `DataGridView` حتى تحتفظ بسلوكها الأصلي |
| RULE-N46 | لون الحقل النشط موحّد: `Color.FromArgb(0xD6, 0xE4, 0xF0)`، ويُعاد اللون الأصلي عند `Leave` |
| RULE-N47 | اختصارات موحّدة للشاشات: `F2` = جديد، `F5` = تحديث، `Esc` = إلغاء الحقل/مسحه — وتظهر في `ToolTip` لكل زر |

---

### 2.19 الثيم الأخضر الموحّد (Green Theme) — إلزامي لكل الشاشات

الثيم الأخضر هو الثيم الرسمي للنظام. مصدر القيم الوحيد هو `GreenThemeHelper` (في `SalesSystem.Desktop/Helpers/`)، وممنوع تكرار قيم الألوان داخل أي شاشة.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N50 | كل شاشة تستدعي `GreenThemeHelper.Apply(this)` **كآخر خطوة** في تهيئة المظهر (بعد `ThemeHelper.ApplyMainFormStyle`/`ApplyButtonStyle`)، لأن `ThemeHelper` يفرض خلفية بيضاء وخط `Segoe UI` فيمحو الأخضر |
| RULE-N51 | كل `DataGridView` تُطبَّق عليها `GreenThemeHelper.ApplyGrid(dgv)` (رؤوس خضراء `(14,165,103)` بنص أبيض، خلفية `(220,240,220)`، أسطر متبادلة `(245,252,245)`). `ThemeHelper.ApplyDataGridViewStyle` **ممنوعة** بعدها |
| RULE-N52 | `ApplyGrid` لا تلمس `ReadOnly` ولا `SelectionMode` ولا `AllowUserToAddRows` ولا `DataSource` — كل شبكة تعلن سلوكها بنفسها حسب `RULE-N22`/`RULE-N22a` |
| RULE-N53 | لون شريط العنوان (`lblMode`/`lblTitle`) يُطبَّق بـ `GreenThemeHelper.ApplyHeader(label)`، والنص الأخضر الداكن للنصوص المهمة هو `GreenThemeHelper.TextStrong` |
| RULE-N54 | ممنوع كتابة قيم ألوان الثيم الأخضر كأرقام داخل الشاشات — تُستهلك من ثوابت `GreenThemeHelper` فقط؛ وأي لون جديد يُضاف هناك لا في الشاشة |

**قيم الثيم الأخضر:**

| العنصر | القيمة |
|---|---|
| خلفية النموذج | `Color.FromArgb(170, 220, 170)` |
| شريط العنوان | `Color.FromArgb(14, 165, 103)` + نص أبيض |
| صناديق المجموعات | `Color.FromArgb(235, 245, 235)` |
| أشرطة الأدوات/الإجراءات | `Color.FromArgb(235, 240, 235)` |
| خلفية الشبكة | `Color.FromArgb(220, 240, 220)` · خطوط `(205, 225, 205)` |
| رؤوس الأعمدة | `(14, 165, 103)` + نص أبيض، Tahoma 10 Bold |
| التحديد | `Color.FromArgb(255, 255, 180)` |
| نص مميّز | `Color.FromArgb(14, 82, 52)` |

---

### 2.20 مصفوفة تعميم الميزات (Feature Rollout Matrix)

الميزات التالية إلزامية في كل شاشة، وتُعمَّم على المراحل التالية. أي شاشة تُنشأ أو تُعدَّل يجب أن تخرج وقد التزمت بكل الميزات المعلّمة «إلزامي».

| # | الميزة | المرجع | الحالة | ملاحظات |
|---|---|---|---|---|
| 1 | تنقل لوحة المفاتيح (Enter/Tab + أسهم RTL + تحديد القيمة + تظليل الحقل) | `RULE-N40`…`RULE-N47` + `KeyboardHelper` | 🔄 تعميم جارٍ | 10 شاشات فقط تستدعي الأربعة حتى الآن |
| 2 | الثيم الأخضر في وقت التشغيل | `RULE-N50`…`RULE-N54` + `GreenThemeHelper` | 🔄 تعميم جارٍ | `frmAddUpdateSalesInvoice` هي الشاشة المرجعية |
| 3 | التفقيط الحيّ كتلميح (ToolTip) على الحقول الرقمية | `LiveTafqeetHelper` | 🔄 شاشة واحدة | `frmAddUpdateSalesInvoice`؛ يُعمَّم على الحقول المالية في باقي الشاشات |
| 4 | شبكة الدليل: بحث فوري + نقر مزدوج + ContextMenu | `RULE-N23` + `RULE-N24` | 🔄 تعميم جارٍ | النمط المرجعي `frmProductMaster` |
| 5 | مؤشر الوضع + آلة حالة الأزرار | `RULE-N43`/`RULE-N44` | 🔄 تعميم جارٍ | — |
| 6 | صندوق التصفية الموحّد `gbFilter` + تصفية فورية + مربعات اختيار + شريط ملخص | `RULE-N56`…`N63` | 🔄 تعميم جارٍ | النمطان المرجعيان: `frmListSales` و`frmProductMaster` |
| 7 | لوحة التحكم كمكوّن `ctrlDashboard` قابل للتضمين | `RULE-N64`…`N69` | ⬜ لم يبدأ | يُستهلك في `frmDashboard` و`frmMain` |
| 8 | تقرير واحد لكل شاشة + تصنيفات قائمة التقارير | `RULE-N70`…`N76` | ⬜ لم يبدأ | إلغاء نموذج التُبويبات |
| 9 | نقطة بيع متجر تجزئة بنمط `frmAddUpdateSalesInvoice` | `RULE-N80`…`N85` | ⬜ لم يبدأ | بيع سريع باختيار الأصناف |
| 10 | دورة «تعطيل → إظهار غير النشط → إعادة تنشيط» | `RULE-N90`…`N94` | ⬜ لم يبدأ | `ReactivateAsync` + زر في الشاشة |

**مراحل التعميم:**

| المرحلة | الشاشات | معيار الإغلاق |
|---|---|---|
| P1 — الشاشات اليومية | `frmListSales`, `frmAddUpdateSalesInvoice`, `frmProductMaster`, `frmProductUnitMaster`, `frmProductSelection`, `frmListPurchases`, `frmPurchaseInvoice`, `frmListReturns`, `frmSalesReturn`, `frmPurchaseReturn` | كل شاشة: ثيم أخضر + تنقل كامل + شبكة خضراء + تجربة يدوية |
| P2 — البيانات الأساسية | Categories, Units, Customers, Suppliers, Warehouses, CashBoxes, Employees, Users, Expenses, Payments, CustomerReceipts | كما أعلاه |
| P3 — التقارير والمخزون والباقي | `Screens/Reports/**` (33), `Screens/Inventory/**` (16), Dashboard, POS, Tools, Login | كما أعلاه، مع مراعاة أن شاشات الطباعة/المعاينة تُعفى من تعبئة النماذج |

> **قاعدة:** تُنفَّذ المراحل بالترتيب P1 → P2 → P3، ولا تُعتبر الشاشة «مكتملة» حتى تلتزم بالميزات 1 و2 أعلاه.

---

### 2.21 قائمة الإلزام لكل نموذج جديد (New Form Definition of Done)

أي نموذج جديد `frm*` — أو نموذج يُعاد بناؤه بالكامل — **لا يُعتبر مكتملاً** حتى يجتاز هذه القائمة بالكامل. تُراجَع قبل تسليم أي مهمة تُنشئ أو تعيد بناء شاشة، وتُذكر نتائجها في تقرير التسليم.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N55 | إنشاء نموذج بدون استيفاء بنود §2.21 يُعدّ عملاً غير مكتمل ويُرفض في المراجعة — بلا استثناء إلا شاشات المعاينة/الطباعة للقراءة فقط |

| # | البند | المرجع | كيف يُتحقق |
|---|------|--------|-------------|
| 1 | الاسم `frm*` والوراثة من `Form` | RULE-N01 | اسم الملف والصنف |
| 2 | حقن `IServiceProvider` في المُنشئ وعدم استخدام `new frmXxx()` | RULE-N02/N03 | المُنشئ + `Program.cs` |
| 3 | تسجيل النموذج في حاوية DI داخل `Program.cs` | RULE-N10 | `services.AddTransient<frmXxx>();` |
| 4 | كل الخدمات عبر `I*ApiService` فقط — لا `DbContext` ولا `SalesSystem.Business` ولا `HttpClient` | RULE-N05/N06 | `grep "SalesSystem.Business\|HttpClient"` = 0 |
| 5 | `RightToLeft = Yes` + `RightToLeftLayout = true` + خط `Tahoma` | RULE-N20/N21 | المصمّم + `ApplyUiTheme` |
| 6 | هيكل الأشرطة: `pnlHeader` (Dock=Top, 56) + `lblMode` + `pnlToolbar` + `pnlFilters` + `pnlSummary` + `pnlGrid` + `pnlFooter` | RULE-N27 | جدول العناصر في تقرير التسليم |
| 7 | استدعاء الأربعة في المُنشئ: `WireEnterAsTab` + `WireSelectAllOnFocus` + `WireFocusHighlight` + `WireArrowNavigation` | RULE-N40 | نص الاستدعاء |
| 8 | تحديد القيمة الحالية يعمل بالفأرة وبلوحة المفاتيح | RULE-N41 | سلوك `KeyboardHelper` (تلقائي عند استدعاء البند 7) |
| 9 | أسهم RTL: يمين/أعلى = السابق، يسار/أسفل = التالي | RULE-N45 | سلوك `KeyboardHelper` |
| 10 | تظليل الحقل النشط `(0xD6,0xE4,0xF0)` | RULE-N46 | معامل `WireFocusHighlight` |
| 11 | مؤشر الوضع `lblMode` يتحدث مع كل تغيير حالة (إضافة/تعديل) | RULE-N44 | `_UpdateModeHeader()` |
| 12 | آلة حالة الأزرار + `WireButtonDisabledVisual` | RULE-N43 | `_UpdateButtonsState()` |
| 13 | **الثيم الأخضر**: `GreenThemeHelper.ApplyHeader(lblMode)` + `GreenThemeHelper.ApplyGrid(dgv)` لكل شبكة + `GreenThemeHelper.Apply(this)` **كآخر سطر** في `ApplyUiTheme()` | RULE-N50…N54 | نهاية `ApplyUiTheme` |
| 14 | الشبكة: `ReadOnly = true` + `AllowUserToAddRows = false` + `SelectionMode = FullRowSelect` + `ContextMenuStrip` + بحث فوري + نقر مزدوج + سطر «لا توجد بيانات» | RULE-N22/N23/N24 | خصائص الشبكة + الأحداث |
| 15 | التفقيط الحيّ على الحقول الرقمية/المالية عبر `LiveTafqeetHelper.Attach(liveTip, …)` | §2.20 بند 3 | نص `Attach` |
| 16 | `ToolTip` لكل زر يتضمن اختصاره، واختصارات `F2` جديد · `F3` عرض/تعديل · `F4` ترحيل · `F5` تحديث · `Delete` إلغاء · `Esc` خروج | RULE-N47 | `WireGlobalShortcuts` |
| 17 | تحقق بـ `errorProvider` + رسائل عربية + **لا** `ex.Message` للمستخدم (Serilog فقط) | RULE-N28/N29 + FORBIDDEN | `grep "ex.Message"` = 0 |
| 18 | `async void` مع `try/catch` + `Cursor.WaitCursor` واستعادته في `finally` + `CancellationToken` | RULE-N25/N26 | مراجعة المعالجات |
| 19 | المال `decimal` وعرضه `N2`، الكميات `decimal(18,3)` | RULE-001/RULE-002 | أنواع الخصائص |
| 20 | عدم كسر أي سلوك قائم: تُحفظ أسماء العناصر والأحداث المُستدعاة من خارج الشاشة | — | `git diff` محدود |
| 21 | صندوق تصفية موحّد `gbFilter` بأدوات التصفية القياسية والتصفية الفورية | `RULE-N56`…`N63` | عناصر `gbFilter` في المُصمّم |
| 22 | شريط ملخص (`pnlSummary`) يعدّ النتائج والإجماليات `N2` بعد كل تصفية | `RULE-N61` | `_UpdateSummary()` |
| 23 | كل الشاشات تستدعي الستة من `KeyboardHelper` (تنقل + تحديد + تظليل + أسهم + أزرار + اختصارات) | `RULE-N95` | grep على `WireArrowNavigation` في كل `frm*.cs` |
| 24 | الثيم الأخضر مطبَّق على الشاشة وكل شبكاتها | `RULE-N96` | `ApplyGrid` + آخر سطر `Apply(this)` |
| 25 | زر «إعادة تنشيط» يعمل على السطر غير النشط فقط | `RULE-N90`…`N94` | تجربة يدوية: عطّل ثم أعد التنشيط |
| 26 | كل التصفية داخل `gbFilter` بلا استثناء | `RULE-N56`…`N63` | عناصر `gbFilter` في المُصمّم |
| 27 | النمط الصحيح للشاشة: بيانات أساسية ⇒ `frm*Master` مدمج · مستند ⇒ `frmList*` + `frmAddUpdate*` | `RULE-N100`…`N104` | لا يوجد `frmAddUpdate*`/`frmList*` لكيان له Master |
| 28 | لا يُضاف أي بند من §2.29 (صلاحيات/نطاق DI/الموظفون المُخزَّنون) إلى MVP | `RULE-N105`…`N108` | مراجعة القائمة قبل كل تسليم |

**الهيكل المرجعي للمُنشئ وشريط المظهر:**

```csharp
public frmNewScreen(IServiceProvider serviceProvider)
{
    _serviceProvider = serviceProvider;
    InitializeComponent();

    // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط
    KeyboardHelper.WireEnterAsTab(this);
    KeyboardHelper.WireSelectAllOnFocus(this);
    KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));
    KeyboardHelper.WireArrowNavigation(this);
    KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete);   // RULE-N43
    KeyboardHelper.WireGlobalShortcuts(this,                                 // RULE-N47
        (Keys.F2, btnNew.PerformClick),
        (Keys.F5, () => _RefreshAsync()));

    ApplyUiTheme();   // RULE-N50 — ينتهي بالثيم الأخضر
}

private void ApplyUiTheme()
{
    ThemeHelper.ApplyMainFormStyle(this);
    ThemeHelper.ApplyButtonStyle(btnSave, ThemeHelper.ButtonType.Primary);
    ThemeHelper.ApplyButtonStyle(btnDelete, ThemeHelper.ButtonType.Danger);
    // … بقية الأزرار

    GreenThemeHelper.ApplyHeader(lblMode);
    GreenThemeHelper.ApplyGrid(dgvMain);          // RULE-N51/N52 — لا يلمس ReadOnly/SelectionMode
    GreenThemeHelper.Apply(this);                 // ← الأخير دائماً (RULE-N50)
}
```

> **تذكير:** `ThemeHelper` يفرض خلفية بيضاء وخط `Segoe UI`؛ لذلك أي استدعاء للثيم الأخضر قبله سيُمحى. الثيم الأخضر آخر سطر — بلا استثناء.

---

### 2.22 معيار التصفية الموحّد (Filter Group Standard)

كل شاشة قائمة أو دليل تعرض بيانات تُصفّى داخل صندوق مجموعة واحد اسمه `gbFilter`، ولا يُسمح بأدوات تصفية متناثرة في الشاشة. النمطان المرجعيان: `frmListSales` و `frmProductMaster`.

| RULE | DIRECTIVE |
|---|---|
| RULE-N56 | كل شاشة قائمة/دليل تحتوي `pnlFilters` (`Dock = Top`، ارتفاع 96–120) وبداخلها `gbFilter` (`GroupBox`، النص «بحث وتصفية»، `Dock = Fill`، `Padding = 8`) |
| RULE-N57 | ترتيب عناصر `gbFilter` من اليمين إلى اليسار ثابت: (1) `cbFilterBy` + `txtFilterValue` (2) `cbStatus` (3) `dtpFrom` + `dtpTo` + أزرار النطاقات السريعة (4) صفّ ثانٍ لمربعات الاختيار |
| RULE-N58 | مربعات الاختيار القياسية بأسماء ثابتة: `chkShowInactive` (إظهار غير النشط) · `chkOnlyDue` (غير المسددة فقط — الشاشات المالية) · `chkOnlyCancelled` (الملغاة فقط) · `chkOnlyOutOfStock` (النافد فقط — المخزون) |
| RULE-N59 | التصفية **فورية**: تُطبَّق عند `TextChanged` و`CheckedChanged` و`SelectedIndexChanged` — لا يوجد زر «بحث»؛ `Enter` يطبّق فوراً، `Esc` يمسح كل أدوات التصفية، `F5` يحدّث |
| RULE-N60 | التصفية بـ`SelectedIndex` لا بمقارنة نص عربي، والبحث النصي على أعمدة معروفة (`Name`/`Barcode`/`Phone`…) لا على `DataView.RowFilter` المبني من إدخال المستخدم مباشرة |
| RULE-N61 | شريط `pnlSummary` يعرض عدد النتائج والإجماليات (`N2`) ويُعاد حسابه بعد كل تصفية |
| RULE-N62 | أي فلتر جديد يُضاف داخل `gbFilter` فقط — ممنوع أزرار/حقول تصفية خارجها |
| RULE-N63 | زر اختياري `btnToggleFilters` («إخفاء/إظهار التصفية») يطوي `pnlFilters` إلى ارتفاع 34 للحصول على مساحة أكبر للشبكة، ويُحفظ حالته في `SystemSettings` بمفتاح `UI.FiltersCollapsed` |

**مخطط عناصر `gbFilter`:**

| العنصر | النوع | الموضع داخل `gbFilter` |
|---|---|---|
| `label2` «تصفية بـ:» + `cbFilterBy` | ComboBox | الصف الأول — أقصى اليمين |
| `txtFilterValue` | TextBox | الصف الأول — يسار `cbFilterBy` |
| `label1` «الحالة:» + `cbStatus` | ComboBox | الصف الأول |
| `label3` «من:» + `dtpFrom` · `label4` «إلى:» + `dtpTo` | DateTimePicker | الصف الأول |
| `btnRangeToday` «اليوم» · `btnRangeMonth` «هذا الشهر» · `btnRange30` «آخر 30 يوماً» | Button (Tahoma 9 Bold) | الصف الثاني — يمين |
| `chkShowInactive` · `chkOnlyDue` · `chkOnlyCancelled` · `chkOnlyOutOfStock` | CheckBox | الصف الثاني — يسار |
| `btnToggleFilters` | Button | الصف الثاني — أقصى اليسار |

> **تعميم:** يُطبّق المعيار على المراحل P1 → P2 → P3 المذكورة في §2.20، وتُعدّ الشاشة غير مكتملة إن كانت أدوات تصفيتها خارج `gbFilter`.

---

### 2.23 معيار لوحة التحكم كـ UserControl (Dashboard Control Standard)

لوحة التحكم **مكوّن قابل للتضمين** وليست نموذجاً؛ تُبنى مرة واحدة وتُستهلك في أي حاوية.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N64 | لوحة التحكم تُبنى كـ `UserControl` باسم `ctrlDashboard` في `SalesSystem.Desktop/Screens/Dashboard/Controls/` — ممنوع بناؤها كـ `Form` |
| RULE-N65 | `ctrlDashboard` يستقبل `IServiceProvider` في المُنشئ ويحل خدماته بـ `GetRequiredService<T>()`؛ يعمل بلا تعديل داخل `frmDashboard` وداخل `frmMain` |
| RULE-N66 | `frmDashboard` يبقى غلافاً رقيقاً: `ctrlDashboard` بـ `Dock = Fill` + زر إغلاق فقط، ولا منطق تحميل فيه |
| RULE-N67 | كل `UserControl` جديد يأخذ بادئة `ctrl*` ويُسجَّل في حاوية DI (`services.AddTransient<ctrlXxx>();`) |
| RULE-N68 | `ctrlDashboard` يعمل في وضعي Mock/Real: يجب وجود `MockDashboardApiService` مسجَّل في فرع Mock حسب `RULE-N10` |
| RULE-N69 | `ctrlDashboard` ينتهي بـ `GreenThemeHelper.Apply(this)` ويستمع لرسائل `EventBus` ليُحدّث أرقامه دون إعادة فتح |

---

### 2.24 معيار التقارير وقائمة التقارير (Reports Standard)

**تقرير واحد = شاشة واحدة.** لا نماذج بتُبويبات تقارير متعددة.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N70 | كل تقرير شاشة `frm*Report` مستقلة تعرض تقريراً **واحداً** فقط؛ نموذج التُبويبات المتعددة (`frmReports`) يُلغى دوره ولا يُضاف تقرير إليه |
| RULE-N71 | كل تقرير يُسجَّل في DI ويُفتح من **عنصر قائمة واحد** في `frmMain` — لا تكرار ولا شاشة يتيمة |
| RULE-N72 | قائمة «التقارير» تُقسَّم إلى قوائم فرعية ثابتة: **المبيعات** · **المشتريات** · **المخزون** · **المحاسبة والمالية** · **العملاء والموردون** · **الصندوق** — كل تصنيف `ToolStripMenuItem` يحتوي تقاريره فقط |
| RULE-N73 | كل تقرير يستوفي §2.21 + §2.22 (`gbFilter` + `pnlSummary`) + تصدير Excel عبر `IExportableReport` |
| RULE-N74 | كود التقارير يُوزَّع حسب التصنيف: `Screens/Reports/{Sales,Purchases,Inventory,Finance,Parties,CashBox}/` — لا حشر كل التقارير في مجلد واحد |
| RULE-N75 | كل `I*ReportApiService` تحتاج تنفيذاً وهمياً `Mock*ReportApiService` وإلا انكسر وضع التجربة |
| RULE-N76 | لا يُعتبر التصنيف مكتملاً حتى تصل كل تقاريره من القائمة في نقرتين على الأكثر |

---

### 2.25 معيار نقطة البيع (POS Standard)

`frmPOS` شاشة بيع سريعة لمتجر تجزئة: الأصناف تُعرض للاختيار السريع، والهدف إتمام البيع في أقل عدد خطوات. الشاشة تعليمية/عملية ولا تُضاف إليها وحدات أو شاشات فرعية غير مطلوبة.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N80 | `frmPOS` تُبنى على نمط `frmAddUpdateSalesInvoice`: باركود → شبكة سطور → مجاميع → دفع → ترحيل |
| RULE-N81 | الشاشة تعتمد الاختيار السريع للأصناف: أزرار/قائمة أصناف + حقل باركود، والتركيز على أقل عدد نقرات لإتمام البيع — ولا تُضاف أي وحدة أو تبويب أو خطوة لا يطلبها البيع السريع |
| RULE-N82 | تعمل بوضعي Mock/Real عبر `IProductApiService` + `ISalesInvoiceApiService` + `ICustomerApiService` فقط — لا وصول مباشر لأي طبقة أخرى |
| RULE-N83 | شبكة سطور POS هي الاستثناء الثاني المسموح لـ `RULE-N22a` (مع `dgvItems`)، ولا يُعمَّم على غيرها |
| RULE-N84 | كل بيع يُرحَّل كـ `SalesInvoice` بحالة `Posted` داخل `BeginTransactionAsync` مع خصم المخزون وتسجيل `InventoryTransactions` والقيد المحاسبي |
| RULE-N85 | بعد الحفظ لا تُغلق الشاشة: تُفرَّغ السلة وتُعاد التهيئة لبيع جديد ويبقى التركيز على حقل الباركود |

---

### 2.26 ثابت إعادة التنشيط (Reactivation Invariant)

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N90 | كل كيان يرث `ActivatableEntity` يجب أن يدعم الدورة كاملة: تعطيل → إظهار غير النشط → إعادة تنشيط |
| RULE-N91 | كل `I*ApiService` لكيان قابل للتعطيل يعرّض `ReactivateAsync` ويُسندها للـ BLL — لا منطق في الـ wrapper (`RULE-N11`) |
| RULE-N92 | شاشة الدليل تُظهر زر «إعادة تنشيط» مفعّلاً على السطر **غير النشط فقط**، ومعطّلاً على النشط (`RULE-N43`) |
| RULE-N93 | إعادة التنشيط تُنفَّذ داخل معاملة، وتُعيد تنشيط الحساب المحاسبي المرتبط إن وُجد، وتُستخدم `IgnoreQueryFilters` للوصول للسجل المعطَّل |
| RULE-N94 | اختبار القبول الإلزامي لكل كيان: «عطّل → أعد التنشيط» يجب أن ينجح، ويُحدَّد السطر بعد التحديث، والرسالة عربية |

---

### 2.27 اعتماد الـ Helpers إلزامياً (Mandatory Helpers)

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N95 | كل شاشة إدخال/دليل تستدعي من `KeyboardHelper` الستة معاً: `WireEnterAsTab` + `WireSelectAllOnFocus` + `WireFocusHighlight` + `WireArrowNavigation` + `WireButtonDisabledVisual` + `WireGlobalShortcuts` — بلا استثناء |
| RULE-N96 | كل `DataGridView` تأخذ `GreenThemeHelper.ApplyGrid(dgv)`، وكل شاشة تنتهي بـ `GreenThemeHelper.Apply(this)` كآخر سطر — بلا استثناء |
| RULE-N97 | كل حقل مالي/رقمي يأخذ تلميح التفقيط الحيّ عبر `LiveTafqeetHelper.Attach` |
| RULE-N98 | ممنوع تكرار كود التنقل أو الألوان أو الاختصارات داخل الشاشة؛ أي سلوك ناقص يُضاف إلى الـ Helper أولاً ثم يُستهلك من كل الشاشات |
| RULE-N99 | شاشة لا تجتاز §2.27 تُعدّ غير مكتملة وتُرفض في المراجعة |

**مرجع الـ Helpers (المصدر الوحيد):**

| Helper | الملف | الوظائف المستهلكة |
|---|---|---|
| `KeyboardHelper` | `SalesSystem.Desktop/Helpers/KeyboardHelper.cs` | `WireEnterAsTab` · `WireSelectAllOnFocus` · `WireFocusHighlight` · `WireArrowNavigation` · `WireButtonDisabledVisual` · `WireGlobalShortcuts` |
| `GreenThemeHelper` | `SalesSystem.Desktop/Helpers/GreenThemeHelper.cs` | `Apply` · `ApplyHeader` · `ApplyGrid` · `TextStrong` |
| `LiveTafqeetHelper` | `SalesSystem.Desktop/Helpers/LiveTafqeetHelper.cs` | `Attach` (تلميح التفقيط الحيّ) |
| `ThemeHelper` | `SalesSystem.Desktop/Helpers/ThemeHelper.cs` | `ApplyMainFormStyle` · `ApplyButtonStyle` (يُستدعى **قبل** الثيم الأخضر) |

**سطر الإلزام في كل مُنشئ شاشة:**

```csharp
KeyboardHelper.WireEnterAsTab(this);
KeyboardHelper.WireSelectAllOnFocus(this);
KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete);      // RULE-N43
KeyboardHelper.WireGlobalShortcuts(this, (Keys.F2, btnNew.PerformClick), (Keys.F5, () => _RefreshAsync())); // RULE-N47
```

---

### 2.28 نمط الشاشات: Master مدمج مقابل Add/List منفصل

قاعدة تصميم إلزامية تمنع تكرار الشاشات وتوحّد تجربة المستخدم.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N100 | شاشات **البيانات الأساسية** (الأصناف · العملاء · الموردون · الفئات · الوحدات · المستودعات · الصناديق · المستخدمون · الموظفون · وحدات الأصناف) تُبنى **شاشة واحدة مدمجة** باسم `frm*Master` تجمع القائمة + المحرر + أدوات الإجراءات في شاشة واحدة — ولا يُنشأ لها `frmAddUpdate*` ولا `frmList*` منفصل |
| RULE-N101 | شاشات **المستندات والحركات** (فاتورة بيع · فاتورة شراء · مرتجع بيع · مرتجع شراء · سند قبض · سند صرف · قيد محاسبي · تسوية مخزون · تحويل مخزني) تبقى **منفصلة**: `frmList*` للعرض والتصفية + `frmAddUpdate*` للإدخال والتحرير — ولا تُدمج الآن |
| RULE-N102 | أي `frmAddUpdateXxx` أو `frmListXxx` قديم يخص كياناً له `frmXxxMaster` يُعدّ **كوداً ميتاً**: يُحذف من المشروع ومن تسجيلات DI — لا يُبقى للتوافق ولا يُذكر في القوائم |
| RULE-N103 | لا يُنشأ `frm*Master` لكيان مستندي، ولا تُدمج شاشة مستند قبل قرار معماري صريح من المالك — القاعدة الافتراضية هي الفصل |
| RULE-N104 | الشاشة المدمجة `frm*Master` تستوفي §2.21 كاملاً + `gbFilter` (§2.22) + الستة من `KeyboardHelper` (§2.27) + الثيم الأخضر (§2.19) — لا استثناء |

**أمثلة مرجعية (إلزامية):**

| الكيان | النمط | الشاشة المرجعية |
|---|---|---|
| الصنف | مدمج | `frmProductMaster` |
| العميل | مدمج | `frmCustomerMaster` |
| المورد | مدمج | `frmSupplierMaster` |
| فاتورة البيع | منفصل | `frmListSales` + `frmAddUpdateSalesInvoice` |
| فاتورة الشراء | منفصل | `frmListPurchases` + `frmPurchaseInvoice` |
| القيد المحاسبي | منفصل | `frmJournalEntries` (قائمة + محرر داخلي) |

---

### 2.29 المؤجل إلى V2 (Deferred Register) — لا يُعدّ نقصاً في MVP

هذه البنود **مؤجلة بقرار المالك**، ولا تُرفع كملاحظات نقص أو ثغرات في مراجعات MVP، ولا يُعمل عليها دون قرار صريح جديد.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N105 | **لا نظام صلاحيات في MVP.** النظام أحادي المستخدم: حساب أدمن واحد بصلاحيات كاملة (`AGENTS.md §1`). ممنوع إضافة شاشة أدوار، أو فحص `PermissionsMask`/`HasPermission` داخل الشاشات، أو تعريض `PermissionsMask` في `LoginResponse` — كل ذلك مؤجل إلى V2 |
| RULE-N106 | **نطاق DI واحد لعمر التطبيق مقبول في MVP** (يُنشأ مرة في `Program.cs` قبل شاشة الدخول). لا يُعاد هيكلته إلى «نطاق لكل عملية» قبل V2، لأن الاستخدام أحادي المستخدم ومتسلسل |
| RULE-N107 | أي مراجعة أمنية أو مراجعة كود تُبلغ عن هذين البندين تُغلق بإحالة إلى هذا القسم، لا بإصلاح |
| RULE-N108 | إضافة أي بند إلى هذا القسم تتطلب قراراً صريحاً من المالك يُدوَّن هنا مع تاريخه |

**سجل المؤجل:**

| # | البند | القرار | البديل المعتمد في MVP | الحالة |
|---|------|--------|------------------------|--------|
| 1 | الأدوار والصلاحيات (`PermissionsMask`، `RULE-N31`…`N37`) | مؤجل — مستخدم واحد بصلاحيات كاملة | كل الشاشات متاحة للأدمن؛ لا فحص صلاحيات | 🔒 V2 |
| 2 | نطاق DI لكل عملية (بدل نطاق واحد لعمر التطبيق) | مؤجل | نطاق واحد مُنشأ في `Program.cs` | 🔒 V2 |
| 3 | وحدة الموظفين كوحدة مُخزَّنة (لا جدول `Employees` في MVP) | مؤجل — موجودة كواجهة تجريبية | `MockEmployeeApiService` في وضع التجربة | 🔒 V2 |

## 3. Enums

```csharp
public enum InvoiceStatus : byte { Draft = 1, Posted = 2, Cancelled = 3 }
public enum PaymentType : byte { Cash = 1, Credit = 2, Mixed = 3 }
public enum MovementType : byte { PurchaseIn = 1, SaleOut = 2, SaleReturnIn = 3, PurchaseReturnOut = 4, TransferOut = 5, TransferIn = 6, Adjustment = 7 }
public enum UnitType : byte { Retail = 0, Wholesale = 1 }
public enum SaleMode : byte { Retail = 1, Wholesale = 2 }
public enum DeleteStrategy { Cancel = 0, Deactivate = 1, Permanent = 2 }
public enum CostingMethod : byte { WeightedAverage = 1, LastPurchasePrice = 2, SupplierPrice = 3 }
public enum UserRole : byte { Admin = 1, Manager = 2, Accountant = 3, Treasurer = 4, Cashier = 5, WarehouseSupervisor = 6, SalesEmployee = 7, Observer = 8, BranchManager = 9 }
public enum CashTransactionType : byte { OpeningBalance = 1, SalesIncome = 2, Expense = 3, TransferOut = 4, TransferIn = 5, RefundOut = 6, SupplierPayment = 7, CustomerPayment = 8 }
public enum InvoiceTypePrint : byte { Sales = 1, Purchase = 2, SalesReturn = 3, PurchaseReturn = 4, Test = 5 }
public enum PriceLevel : byte { Retail = 1, Wholesale = 2, VIP = 3, Distributor = 4 }
public enum WarehouseType : byte { Main = 1, Store = 2, Showroom = 3 }
public enum StockAdjustmentType : byte { Addition = 1, Deduction = 2, Correction = 3 }
public enum StockIssueReason : byte { SalesReturn = 1, Damage = 2, Expiry = 3, InternalUse = 4, Other = 5 }
public enum PaymentMethod : byte { Cash = 1, Cheque = 2, BankTransfer = 3, CreditCard = 4 }
public enum ChequeStatus : byte { Pending = 1, Cleared = 2, Bounced = 3, Cancelled = 4 }
public enum AccountNature : byte { Asset = 1, Liability = 2, Equity = 3, Revenue = 4, Expense = 5 }
public enum JournalEntryType : byte { Manual = 1, Sales = 2, Purchase = 3, Receipt = 4, Payment = 5, Inventory = 6, Adjustment = 7 }
public enum InventoryTransactionType : byte { Purchase = 1, PurchaseReturn = 2, Sale = 3, SaleReturn = 4, TransferOut = 5, TransferIn = 6, Count = 7, Adjustment = 8, Damage = 9, OpeningBalance = 10, InternalIssue = 11, InternalReceipt = 12 }
public enum SettingsType : byte { String = 1, Integer = 2, Decimal = 3, Boolean = 4 }
public enum NotificationType : byte { LowStock = 1, ExpirySoon = 2, CreditLimitExceeded = 3, System = 4, Reminder = 5 }
public enum DiscountType : byte { Amount = 0, Percentage = 1 }
public enum AdjustmentType : byte { Opening = 1, Increase = 2, Shortage = 3, Damage = 4 }
```

---

## 4. FORBIDDEN — Never Do These

```text
❌ float / double / real for money or quantity
❌ varchar (always use nvarchar for Arabic-supporting text)
❌ Console.WriteLine (use Serilog)
❌ Direct DB from Desktop
❌ Desktop calling HttpClient / any HTTP endpoint — always call BLL services in-process
❌ Business logic in `Direct*Service` wrappers (they delegate to BLL only)
❌ Throwing exceptions from Services (use Result<T>)
❌ Hard-deleting invoices or users
❌ Editing a Posted invoice
❌ Cascade delete on any FK
❌ new frmXxx() — always resolve from DI
❌ Resolving services outside DI (no `new frmXxx()`, no manual `new` of services)
❌ Async void handlers without try/catch
❌ English user-facing messages (must be Arabic)
❌ Showing exception details directly to users
```

---

## 5. Solution Build

```powershell
# Build entire solution (from SalesSystem directory)
dotnet build .\SalesSystem.slnx

# Build individual projects
dotnet build .\SalesSystem.Desktop\SalesSystem.Desktop.csproj
dotnet build .\SalesSystem.Business\SalesSystem.Business.csproj
dotnet build .\SalesSystem.DataAccess\SalesSystem.DataAccess.csproj
```

---

## 6. Desktop ↔ BLL Service Convention

There is no HTTP surface. Each Desktop data screen talks to the BLL through one `I*ApiService` interface:

- Interface: `Services/Interfaces/I<Entity>ApiService.cs`
- Real implementation: `Services/Direct/Direct<Entity>Service.cs` → calls `Business.Interfaces.Services.I<Entity>Service`
- Mock implementation: `Services/Mock/Mock<Entity>ApiService.cs` → in-memory `MockData`
- Return type: always `Result` / `Result<T>` — never raw values, never exceptions
- Registration: `Program.cs` maps the interface to the mock or direct implementation based on `AppSettings.UseMockData`

---

## 7. Common Helpers (SalesSystem.Common.Helpers)

| Class | Description |
|-------|-------------|
| `Guard` | Guard clauses for input validation — exception-style (fail-fast) and Result-style (business rules) |
| `StringExtensions` | Extension methods for string: `Truncate`, `HasValue`, `ToNullIfEmpty`, `DefaultIfEmpty`, `IsNullOrWhiteSpace` |
| `DateTimeExtensions` | Extension methods for DateTime: `StartOfWeek`, `EndOfWeek`, `StartOfMonth`, `EndOfMonth`, `IsWeekend`, `IsFriday`, `IsSaturday` |
| `FileHelper` | File I/O utilities: read/write/delete files, ensure directories, sanitize file names |

---

## 8. Project File Map

Solution root: `SalesSystem/`

```text
SalesSystem.slnx
├── SalesSystem.Common/               ← Base entities + Enums + Exceptions + Helpers
│   ├── BaseEntity.cs, AuditableEntity.cs, ActivatableEntity.cs, DocumentEntity.cs, LongEntity.cs
│   ├── Enums/                        ← All 22+ enums (InvoiceStatus, PaymentType, etc.)
│   ├── Exceptions/                   ← DomainException, etc.
│   └── Helpers/                      ← Guard, StringExtensions, DateTimeExtensions, FileHelper
│
├── SalesSystem.Models/               ← ~48 domain entities (Product, Customer, SalesInvoice, etc.)
│   └── Accounting/                   ← Account, JournalEntry, JournalEntryLine, etc.
│
├── SalesSystem.DTO/                  ← DTOs + Request/Response models + Result<T>
│   └── Common/Result.cs              ← Result<T> pattern used by ALL services
│
├── SalesSystem.DataAccess/           ← EF Core + SQL Server
│   ├── SalesDbContext.cs             ← Single DbContext
│   ├── UnitOfWork.cs                 ← IUnitOfWork implementation
│   ├── Configurations/               ← Fluent API entity configs
│   ├── Repositories/                 ← Repository implementations
│   ├── Migrations/                   ← EF Core migrations
│   ├── Seeders/                      ← DbSeeder + seed data
│   ├── Security/                     ← DPAPI, password hashing
│   ├── Printing/                     ← QuestPDF print services
│   └── Services/                     ← Data-layer services
│
├── SalesSystem.Business/             ← Business Logic Layer
│   ├── Interfaces/                   ← Business service interfaces
│   │   └── Services/                 ← ICustomerService, ISalesService, IProductService, etc. (BizInterfaces alias)
│   ├── Services/                     ← Service implementations
│   ├── Accounting/                   ← Journal entry automation
│   ├── Helpers/                      ← Business calculation helpers
│   └── Printing/                     ← Print business logic
│
├── SalesSystem.Desktop/              ← WinForms Desktop Client
│   ├── Program.cs                    ← DI container, entry point
│   ├── Screens/                      ← 24 module folders (Sales, Products, Customers, etc.)
│   │   ├── Sales/                    ← frm* forms for sales
│   │   ├── Products/                 ← frm* forms for products
│   │   ├── Customers/                ← frm* forms for customers
│   │   └── ... (24 modules)
│   ├── Services/                     ← Service implementations
│   │   ├── Direct/                   ← Direct*Service wrappers (I*ApiService → BLL)
│   │   ├── Interfaces/               ← I*ApiService interfaces (unchanged)
│   │   └── Mock/                     ← Mock*ApiService + MockData (in-memory, no DB)
│   ├── Controls/                     ← Custom WinForms controls
│   ├── Helpers/                      ← Desktop utility classes
│   ├── Models/                       ← Desktop-specific view models
│   ├── Printing/                     ← Desktop print handling
│   └── Configuration/                ← App configuration (AppSettings.cs)
│
└── SalesSystem.Tests/                ← Unit + Integration tests
```

---

## 9. Current Module Status

| Module | Status | Desktop Screens | Business | DB |
|--------|--------|-----------------|----------|----|
| Users & Permissions | ✅ Done | frmUsers, frmRoles | ✅ | ✅ |
| Settings | ✅ Done | frmSettings | ✅ | ✅ |
| Currencies | ✅ Done | frmCurrencies | ✅ | ✅ |
| Chart of Accounts | ✅ Done | frmAccounts | ✅ | ✅ |
| Customers | ✅ Done | frmCustomers | ✅ | ✅ |
| Suppliers | ✅ Done | frmSuppliers | ✅ | ✅ |
| Categories | ✅ Done | frmCategories | ✅ | ✅ |
| Units | ✅ Done | frmUnits | ✅ | ✅ |
| Taxes | ✅ Done | frmTaxes | ✅ | ✅ |
| Warehouses | ✅ Done | frmWarehouses | ✅ | ✅ |
| Products | ✅ Done | frmProducts | ✅ | ✅ |
| CashBoxes | ✅ Done | frmCashBoxes | ✅ | ✅ |
| Banks | ✅ Done | frmBanks | ✅ | ✅ |
| Purchases | 🔄 In Progress | frmPurchases | ✅ | ✅ |
| Sales | 🔄 In Progress | frmSales | ✅ | ✅ |
| Sales Returns | 🔄 In Progress | frmReturns | 🔄 | ✅ |
| Receipts & Payments | 🔄 In Progress | frmReceipts, frmPayments | 🔄 | ✅ |
| Journal Entries | 🔄 In Progress | frmJournalEntries | 🔄 | ✅ |
| Reports | ❌ Not Started | — | — | — |
| Dashboard | ❌ Not Started | frmDashboard (shell) | — | — |
| Inventory (Transfers/Adjustments) | 🔄 In Progress | frmInventory | 🔄 | ✅ |
| Expenses | ✅ Done | frmExpenses | ✅ | ✅ |
| Accounting Engine | ✅ Done | — (auto) | ✅ | ✅ |
| Printing | 🔄 In Progress | — | ✅ | ✅ |

> The former `API` column was removed — there is no HTTP/API layer; every module's Desktop layer goes straight to Business.

---

## 10. AI Agent Context Guide

### File Priority (read in this order)

| Priority | File | What it contains |
|----------|------|------------------|
| 1 (MUST) | `AGENTS.md` | Architecture, rules, patterns, enums, forbidden list |
| 2 | `docs/CONSTITUTION.md` | Financial formulas, transaction protocol, non-negotiable rules |
| 3 | `docs/BUSINESS-RULES.md` | Domain-specific business logic rules |
| 4 | `docs/database-schema.md` | SQL table definitions, CHECK constraints, indexes |
| 5 | `docs/PRD-MVP.md` | Full product requirements (reference only — very large) |

### Reference Code (for pattern examples)

`SalesSystem/SalesManagement/` — Owner's own WinForms code for reference on UI patterns.
This is NOT part of the solution build. Use it to understand how the owner writes WinForms code.

### Architecture (NEVER violate)

```text
Desktop (WinForms)  →  BLL Services  →  DataAccess (EF Core)  →  SQL Server

 frm* forms          Result<T>            IUnitOfWork          decimal(18,2)/(18,3)
 GetRequiredService  All logic here       Fluent API           CHECK constraints
 DI resolved         No exceptions        Repositories
     ↓
 I*ApiService  →  Direct*Service (Services/Direct/)  or  Mock*ApiService (Services/Mock/)
```

No HTTP, no controllers, no JSON serialization on the data path. If you find yourself importing `System.Net.Http`, you are in the wrong layer.
