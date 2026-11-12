# AGENTS.md — Sales Management System (v5.0.0 — 2-Tier WinForms Edition)
# READ THIS FILE FIRST — BEFORE WRITING ANY CODE
# Platform: .NET 10 | 2-Tier Architecture
# Code doctrine: Classic WinForms (NO MVVM) · Simple N-Layer (NO Clean Architecture) · No API layer · Reusable Helpers — see §2.30
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

**Mock mode (UI-first testing):** when `AppSettings.UseMockData = true`, each `I*DesktopService` is implemented by a `Mock*DesktopService` (in `Services/Mock/`) backed by in-memory `MockData` — no database required. When `false`, the same interface is implemented by a `Direct*Service` (in `Services/Direct/`) that calls the BLL in-process.

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
| RULE-N04 | Resolved services are exposed as typed properties (e.g., `private ICustomerDesktopService DesktopService => _serviceProvider.GetRequiredService<ICustomerDesktopService>();`) |
| RULE-N05 | `I*DesktopService` interfaces live in `Services/Interfaces/`; real implementations (`Direct*Service`) live in `Services/Direct/`, mock implementations (`Mock*DesktopService`) in `Services/Mock/` |
| RULE-N06 | `Direct*Service` classes call BLL services in-process via constructor injection (e.g. `SalesSystem.Business.Interfaces.Services.ICustomerService`) — NEVER `HttpClient`, never HTTP |
| RULE-N07 | Global exception handlers in `Program.cs`: `Application.ThreadException` + `AppDomain.CurrentDomain.UnhandledException` |
| RULE-N08 | `MessageBox.Show` is the standard user feedback mechanism — use `MessageBoxIcon.Error`/`Information`/`Warning` appropriately |
| RULE-N09 | Forms are shown modally via `ShowDialog()` — NEVER non-modal for data-entry screens |
| RULE-N10 | Every `I*DesktopService` interface is registered in the `Program.cs` DI container — mapped to `Mock*DesktopService` (mock mode) or `Direct*Service` (real mode) |

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

    private ICustomerDesktopService DesktopService
        => _serviceProvider.GetRequiredService<ICustomerDesktopService>();
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
    services.AddTransient<ICustomerDesktopService, MockCustomerDesktopService>();
    services.AddTransient<IProductDesktopService, MockProductDesktopService>();
    services.AddTransient<IWarehouseDesktopService, MockWarehouseDesktopService>();
    services.AddTransient<ISalesInvoiceDesktopService, MockSalesInvoiceDesktopService>();
    services.AddTransient<ISettingsDesktopService, MockSettingsDesktopService>();
    services.AddScoped<BizInterfaces.IAuthService, MockAuthService>();
}
else
{
    services.AddTransient<ICustomerDesktopService, DirectCustomerService>();
    services.AddTransient<IProductDesktopService, DirectProductService>();
    services.AddTransient<IWarehouseDesktopService, DirectWarehouseService>();
    services.AddTransient<ISalesInvoiceDesktopService, DirectSalesInvoiceService>();
    services.AddTransient<ISettingsDesktopService, DirectSettingsService>();
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
| RULE-N11 | `Direct*Service` implements the `I*DesktopService` interface and delegates to a single BLL service — no business logic of its own |
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

القائمة أدناه مقروءة من ملفات `.csproj` الفعلية (تحقّق: `Select-String -Path *.csproj -Pattern "PackageReference"`).

| Package | Version | Project |
|---------|---------|---------|
| `Microsoft.EntityFrameworkCore.SqlServer` | 10.0.0 | DataAccess |
| `Microsoft.EntityFrameworkCore.Tools` | 10.0.0 | DataAccess |
| `Microsoft.EntityFrameworkCore.Design` | 10.0.0 | DataAccess |
| `Microsoft.EntityFrameworkCore.Design` | 10.0.10 | Desktop (تصميم/هجرات) |
| `Microsoft.EntityFrameworkCore` | 10.0.0 | Business |
| `Microsoft.Extensions.Hosting` | 10.0.0 | Business |
| `Microsoft.Extensions.Hosting` | 10.0.7 | Desktop |
| `Microsoft.Extensions.Logging.Abstractions` | 10.0.0 | Common |
| `FluentValidation` | 11.3.0 | Business |
| `BCrypt.Net-Next` | 4.0.3 | Business · DataAccess |
| `Microsoft.AspNetCore.DataProtection` | 10.0.10 | DataAccess |
| `ClosedXML` | 0.105.0 | DataAccess · Desktop (Excel export) |
| `QuestPDF` | 2024.3.0 | DataAccess (A4 PDF) |
| `System.Drawing.Common` | 10.0.0 | DataAccess |
| `Serilog` | 4.3.1 | Desktop |
| `Serilog.Sinks.File` | 7.0.0 | Desktop |
| `xunit` · `xunit.runner.visualstudio` · `Microsoft.NET.Test.Sdk` · `coverlet.collector` | 2.9.3 · 3.1.4 · 17.14.1 · 6.0.4 | Tests |
| `Microsoft.Extensions.Http` | 10.0.7 | DataAccess — ⚠️ مرجع ميت — يُزال (`RULE-N123`) |
| `Serilog.AspNetCore` | 10.0.0 | DataAccess — ⚠️ مرجع ميت — يُزال (`RULE-N123`) |

> حُذفت مع مشروع الـ API خمس حزم: `FluentValidation.AspNetCore`، `Microsoft.AspNetCore.Authentication.JwtBearer`، `Swashbuckle.AspNetCore`، `Microsoft.Extensions.Hosting.WindowsServices`، `Serilog.Sinks.EventLog`.
> **تنبيه:** `Serilog.AspNetCore` و`Microsoft.Extensions.Http` **لا تزالان معلنتين في `SalesSystem.DataAccess.csproj` ولا يستخدمهما أي كود** — مرجع ميت يُزال عند أول مساس بالملف (`RULE-N123`). كذلك `System.Text.Json` و`SixLabors.ImageSharp` لم تعودا مرجعيتين في أي مشروع، وأُزيلت صفوفهما من الجدول.

### 2.12 WinForms UI Conventions

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N20 | ALL forms use `RightToLeft = Yes` and `RightToLeftLayout = true` for Arabic UI |
| RULE-N21 | ALL forms use `Tahoma` font family |
| RULE-N22 | DataGridView with `AllowUserToAddRows = false`, `ReadOnly = true`, `SelectionMode = FullRowSelect` |
| RULE-N22a | استثناء موثّق ومقصود: شبكة إدخال سطور الفاتورة `dgvItems` في `frmAddUpdateSalesInvoice` تستخدم `AllowUserToAddRows = true` و `SelectionMode = CellSelect` وتحريراً داخل الخلية (إدخال مباشر بالباركود/التحرير السريع) | `RULE-N22` تبقى إلزامية لكل شبكات العرض والقوائم والتقارير؛ الاستثناء لا يُعمَّم على أي شبكة أخرى |
| RULE-N23 | ContextMenuStrip on DataGridView for row operations — 🔄 14 شبكة من 77 شبكة حتى الآن؛ إلزامية لشاشات القوائم والمستندات، وليست شرط إغلاق لكل شاشة عرض |
| RULE-N24 | نمط التصفية: `cbFilterBy` + `txtFilterValue` **داخل `gbFilter`** (§2.22) مع تصفية فورية؛ وبناء `DataView.RowFilter` حصراً عبر `RowFilterHelper.Like/Escape` على أعمدة معروفة (`Name`/`Barcode`/`Phone`) — ممنوع إدراج قيمة المستخدم خاماً أو المقارنة بنص عربي مباشر (`RULE-N60`) |
| RULE-N25 | Loading data: `async void` handlers with try/catch and `MessageBox.Show` on error |
| RULE-N26 | Cursor feedback: `Cursor = Cursors.WaitCursor` before async operations, restore in finally |
| RULE-N27 | Form pattern: Header label (lblMode, Dock=Top, colored) + Filter bar + DataGridView + Footer buttons |
| RULE-N28 | All user-facing messages in Arabic |
| RULE-N29 | Error messages log via Serilog, show `MessageBox.Show` to user |
| RULE-N30 | Forms handle dispose pattern: `components?.Dispose()` in `Dispose(bool)` |

> **إلزامي:** أي نموذج جديد (أو نموذج يُعاد بناؤه) يجب أن يجتاز قائمة الإلزام في §2.21 قبل اعتباره مكتملاً.

### 2.13 المستخدمون (Users) — مصادقة MVP + الصلاحيات مؤجلة إلى V2 (§2.29)

النظام في MVP أحادي المستخدم: مصادقة واحدة (دخول + كلمة سر) بلا نظام صلاحيات. الشاشات: `frmUserMaster` (الدليل المدمج) + `frmPasswordChange` + `frmBackup`.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N35 | ✅ مطبَّق — `User.Create()` is passwordless — `MustChangePassword = true` by default |
| RULE-N36 | ✅ مطبَّق — Password hashing via BCrypt (work factor 12) in service layer (`UserService`/`AuthService`) |
| RULE-N37 | ✅ مطبَّق — Lockout after 5 failed login attempts (`LoginAttempts >= 5 → IsLocked = true`) |

**🔒 مؤجل إلى V2 — أُزيل من الكود:**

`RULE-N31`…`RULE-N34` (الأدوار والصلاحيات) **لم تُنفَّذ ولم تبقَ في الكود**: لا يوجد `PermissionsMask` على `User`، ولا كيان `Role` في `SalesSystem.Models`، ولا شاشة `frmRoles` (ولا أي زر/فحص `HasPermission` في الشاشات) — وهذا متوافق مع §2.29 (`RULE-N105`). يبقى `UserRole` في `SalesSystem.Common.Enums` **كوسم جلسة فقط**: `frmLogin` يضبط `Admin`/`Observer` في `UserSession.Role`، ويُستهلك في `NavigationItem.MinRole` وعرض لوحة التحكم — لا في أي فحص صلاحيات.

### 2.14 Desktop Service Interface Pattern (`I*DesktopService` → `Direct*Service`)

العقد يُسمّى `I<Entity>DesktopService` — لا علاقة له بأي HTTP؛ هو واجهة الطبقة المكتبية فوق BLL. التنفيذ الحقيقي `Direct*Service` غلاف رقيق فوق BLL (ليس HTTP).

```csharp
// Interface (Services/Interfaces/)
public interface IUserDesktopService
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

public class DirectCustomerService : ICustomerDesktopService
{
    private readonly BizInterfaces.ICustomerService _bll;
    private readonly ISessionService _session;

    public DirectCustomerService(BizInterfaces.ICustomerService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<CustomerDto>>> GetAllAsync(
        string? search = null, bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // PagedResult<CustomerDto> → IReadOnlyList<CustomerDto>
            var r = await _bll.GetAllAsync(search, 1, int.MaxValue, includeInactive, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<CustomerDto>>.Success(r.Value!.Items.ToList())
                : Result<IReadOnlyList<CustomerDto>>.Failure(r.Error ?? "تعذر تحميل قائمة العملاء", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<CustomerDto>>.Failure("تعذر تحميل قائمة العملاء");
        }
    }

    public Task<Result<CustomerDto>> GetByIdAsync(int id, CancellationToken ct = default)
        => _bll.GetByIdAsync(id, ct);

    public Task<Result<CustomerDto>> CreateAsync(CreateCustomerRequest r, CancellationToken ct = default)
        => _bll.CreateAsync(r, UserId, ct);

    public Task<Result<CustomerDto>> UpdateAsync(int id, UpdateCustomerRequest r, CancellationToken ct = default)
        => _bll.UpdateAsync(id, r, UserId, ct);

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
        => _bll.DeleteAsync(id, UserId, ct);

    public Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
        => _bll.ReactivateAsync(id, ct);
}
```

> كل عضو في الـ wrapper مُغلَّف بـ try/catch يُعيد `Result.Failure` برسالة عربية (`RULE-N14`).
>
> BLL interfaces live in `SalesSystem.Business.Interfaces.Services` (Aliased as `BizInterfaces`). Implementations are registered as `Scoped` BLL services in `Program.cs`.

### 2.16 Mock Data / UI-First Testing

The Desktop app supports a mock-data mode so the UI can be built and tested without a database.

| Aspect | Detail |
|--------|--------|
| Flag | `AppSettings.UseMockData` in `SalesSystem.Desktop/appsettings.json` (default `true`) |
| Mock implementations | `SalesSystem.Desktop/Services/Mock/` (43 ملفاً) — `MockAuthService` plus mock implementations of the `I*DesktopService` interfaces (`MockCustomerDesktopService`, `MockProductDesktopService`, `MockWarehouseDesktopService`, `MockSalesInvoiceDesktopService`, `MockSettingsDesktopService`, `MockDashboardDesktopService` — `RULE-N68`) and the five report mocks (`MockSalesReportDesktopService`, `MockPurchaseReportDesktopService`, `MockFinancialReportDesktopService`, `MockCashBoxReportDesktopService`, `MockReportDesktopService` — `RULE-N75`) مع `MockData` مشترك في الذاكرة |
| Switching | Set `UseMockData` to `true` (mock) or `false` (real BLL/DB) in `appsettings.json`; `Program.cs` maps each `I*DesktopService` to `Mock*DesktopService` or `Direct*Service` at startup |
| Indicator | `frmMain` appends `وضع التجربة (Mock)` to the title bar when mock mode is active |
| Login | In mock mode, login accepts any credentials (no database required) |

**Workflow rule:** Test UI/UX with mock data **before** wiring the real BLL (UI-first). Mock mode requires no database and no server running.

### 2.17 مراحل التنفيذ (Phases)

خطة تطوير الواجهة تُنفَّذ على ثلاث مراحل، ولكل مرحلة معيار إغلاق قابل للتحقق:

| المرحلة | النطاق | معيار الإغلاق | الحالة |
|---|---|---|---|
| أ — الدين المعماري | إغلاق `RULE-N05`/`RULE-N06` بالكامل: كل شاشة في `Desktop/Screens/**` تتحدث مع BLL **فقط** عبر `I*DesktopService` → `Direct*Service` | `grep -r "SalesSystem.Business" SalesSystem/SalesSystem.Desktop/Screens` = **0 نتائج**، و`HttpClient` = 0، و`dotnet build` = 0 أخطاء | ✅ **مغلقة** — `SalesSystem.Business` في `Screens` = 0، و`ViewModel\|HttpClient` في المشروع كله = 0 |
| ب — UX دليل الأصناف | `frmProductMaster` + `frmProductUnitMaster`: مؤشر وضع، آلة حالة الأزرار، تنقل لوحة المفاتيح، شبكة أغنى، تحقق دقيق | البنود B1–B11 من خطة التحسين منفَّذة ومُختبرة يدوياً | ✅ **مغلقة** — الشاشتان موجودتان ومكتملتان (هما النمط المرجعي في §2.22) |
| ج — شاشة فاتورة البيع | تثبيت استثناء `RULE-N22a` + تحسينات متقدمة (تفقيط حيّ داخل تحرير الخلية عبر `EditingControlShowing`) | الاستثناء موثّق هنا + الشاشة تعمل بوضعي Mock/Real | ✅ **مغلقة** — `RULE-N22a` موثّق ونشط، و`frmAddUpdateSalesInvoice` هي الشاشة المرجعية للتفقيط الحيّ |

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
| 1 | تنقل لوحة المفاتيح (Enter/Tab + أسهم RTL + تحديد القيمة + تظليل الحقل) | `RULE-N40`…`RULE-N47` + `KeyboardHelper` | 🔄 تعميم جارٍ | **37** شاشة تستدعي الأربعة، و**23** شاشة تستدعي الستة كاملة من أصل 95 نموذج `frm*` |
| 2 | الثيم الأخضر في وقت التشغيل | `RULE-N50`…`RULE-N54` + `GreenThemeHelper` | 🔄 تعميم جارٍ | `frmAddUpdateSalesInvoice` هي الشاشة المرجعية |
| 3 | التفقيط الحيّ كتلميح (ToolTip) على الحقول الرقمية | `LiveTafqeetHelper` | 🔄 تعميم جارٍ | **6 شاشات**: `frmAddUpdateSalesInvoice` · `frmPOS` · `frmJournalEntries` · `frmAccounts` · `frmListExpenses` · `frmEmployeeMaster` |
| 4 | شبكة الدليل: بحث فوري + نقر مزدوج + ContextMenu | `RULE-N23` + `RULE-N24` | 🔄 تعميم جارٍ | النمط المرجعي `frmProductMaster` — `ContextMenuStrip` في **14 شبكة من 77** |
| 5 | مؤشر الوضع + آلة حالة الأزرار | `RULE-N43`/`RULE-N44` | 🔄 تعميم جارٍ | `WireButtonDisabledVisual` في **23** شاشة |
| 6 | صندوق التصفية الموحّد `gbFilter` + تصفية فورية + مربعات اختيار + شريط ملخص | `RULE-N56`…`N63` | 🔄 تعميم جارٍ | **22 شاشة** فيها `gbFilter` + `pnlSummary` معاً؛ النمطان المرجعيان: `frmListSales` و`frmProductMaster` |
| 7 | لوحة التحكم كمكوّن `ctrlDashboard` قابل للتضمين | `RULE-N64`…`N69` | ✅ منفّذ | `Screens/Dashboard/Controls/ctrlDashboard` + غلاف `frmDashboard` + `DirectDashboardService`/`MockDashboardDesktopService` + `IEventBus` |
| 8 | تقرير واحد لكل شاشة + تصنيفات قائمة التقارير | `RULE-N70`…`N76` | ✅ منفّذ | **31 تقريراً** في `Screens/Reports/Forms/`، نموذج واحد لكل تقرير + تنقل داخلي بـ `ReportNavigationHelper` + خمسة `Mock*ReportDesktopService` |
| 9 | نقطة بيع متجر تجزئة بنمط `frmAddUpdateSalesInvoice` | `RULE-N80`…`N85` | ✅ منفّذ | `frmPOS` (1371 سطراً) + شبكة `dgvCart` + خمس خدمات Desktop (منتج · فاتورة بيع · عميل · مستودع · صندوق) |
| 10 | دورة «تعطيل → إظهار غير النشط → إعادة تنشيط» | `RULE-N90`…`N94` | 🔄 بدأ | العميل مكتمل (خدمة + زر `btnReactivate` في `frmCustomerMaster`)؛ الأصناف والموردون لديهما `ReactivateAsync` بلا زر؛ الباقي لم يبدأ |

**مراحل التعميم:**

| المرحلة | الشاشات | معيار الإغلاق |
|---|---|---|
| P1 — الشاشات اليومية | `frmListSales`, `frmAddUpdateSalesInvoice`, `frmProductMaster`, `frmProductUnitMaster`, `frmProductSelection`, `frmListPurchases`, `frmPurchaseInvoice`, `frmListReturns`, `frmSalesReturn`, `frmPurchaseReturn` | كل شاشة: ثيم أخضر + تنقل كامل + شبكة خضراء + تجربة يدوية |
| P2 — البيانات الأساسية | Categories, Units, Customers, Suppliers, Warehouses, CashBoxes, Employees, Users, Expenses, Payments, CustomerReceipts | كما أعلاه |
| P3 — التقارير والمخزون والباقي | `Screens/Reports/Forms/**` (31), `Screens/Inventory/**` (16), Dashboard, POS, Tools, Login | كما أعلاه، مع مراعاة أن شاشات التقارير تُعفى من بنود لوحة المفاتيح و`gbFilter` (`RULE-N121`) وشاشات الطباعة/المعاينة تُعفى من تعبئة النماذج |

> **قاعدة:** تُنفَّذ المراحل بالترتيب P1 → P2 → P3، ولا تُعتبر الشاشة «مكتملة» حتى تلتزم بالميزات 1 و2 أعلاه.

---

### 2.21 قائمة الإلزام لكل نموذج جديد (New Form Definition of Done)

أي نموذج جديد `frm*` — أو نموذج يُعاد بناؤه بالكامل — **لا يُعتبر مكتملاً** حتى يجتاز هذه القائمة بالكامل. تُراجَع قبل تسليم أي مهمة تُنشئ أو تعيد بناء شاشة، وتُذكر نتائجها في تقرير التسليم.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N55 | إنشاء نموذج بدون استيفاء بنود §2.21 يُعدّ عملاً غير مكتمل ويُرفض في المراجعة — بلا استثناء إلا شاشات المعاينة/الطباعة للقراءة فقط |
| RULE-N79 | هيكلان مرجعيان فقط للشاشات: (أ) مستند/قائمة = `pnlHeader`+`pnlToolbar`+`pnlFilters`+`pnlSummary`+`pnlGrid`+`pnlFooter` (ب) `frm*Master` مدمجة = محرر + `gbFilter` + `pnlSummary` + `pnlGrid` — و`lblMode` إلزامي في الحالتين |

| # | البند | المرجع | كيف يُتحقق |
|---|------|--------|-------------|
| 1 | الاسم `frm*` والوراثة من `Form` | RULE-N01 | اسم الملف والصنف |
| 2 | حقن `IServiceProvider` في المُنشئ وعدم استخدام `new frmXxx()` | RULE-N02/N03 | المُنشئ + `Program.cs` |
| 3 | تسجيل النموذج في حاوية DI داخل `Program.cs` | RULE-N10 | `services.AddTransient<frmXxx>();` |
| 4 | كل الخدمات عبر `I*DesktopService` فقط — لا `DbContext` ولا `SalesSystem.Business` ولا `HttpClient` | RULE-N05/N06 | `grep "SalesSystem.Business\|HttpClient"` = 0 |
| 5 | `RightToLeft = Yes` + `RightToLeftLayout = true` + خط `Tahoma` | RULE-N20/N21 | المصمّم + `ApplyUiTheme` |
| 6 | الهيكل المرجعي (RULE-N79): شاشة مستند/قائمة = `pnlHeader`(`lblMode`) + `pnlToolbar` + `pnlFilters`/`gbFilter` + `pnlSummary` + `pnlGrid` + `pnlFooter` · شاشة `frm*Master` مدمجة = لوحة المحرر + `gbFilter` + `pnlSummary` + `pnlGrid` (بلا شريط أدوات منفصل) — و`lblMode` إلزامي في الحالتين | RULE-N79 | جدول العناصر في تقرير التسليم |
| 7 | كل الستة من `KeyboardHelper` في المُنشئ (تنقل + تحديد + تظليل + أسهم + أزرار + اختصارات) | = البند 23 (RULE-N95) | نص الاستدعاء |
| 8 | تحديد القيمة الحالية يعمل بالفأرة وبلوحة المفاتيح | RULE-N41 | سلوك `KeyboardHelper` (تلقائي عند استدعاء البند 7) |
| 9 | أسهم RTL: يمين/أعلى = السابق، يسار/أسفل = التالي | RULE-N45 | سلوك `KeyboardHelper` |
| 10 | تظليل الحقل النشط `(0xD6,0xE4,0xF0)` | RULE-N46 | معامل `WireFocusHighlight` |
| 11 | مؤشر الوضع `lblMode` يتحدث مع كل تغيير حالة (إضافة/تعديل) | RULE-N44 | `_UpdateModeHeader()` |
| 12 | آلة حالة الأزرار + `WireButtonDisabledVisual` | RULE-N43 | `_UpdateButtonsState()` |
| 13 | **الثيم الأخضر**: `GreenThemeHelper.ApplyHeader(lblMode)` + `GreenThemeHelper.ApplyGrid(dgv)` لكل شبكة + `GreenThemeHelper.Apply(this)` **كآخر سطر** في `ApplyUiTheme()` | RULE-N50…N54 | نهاية `ApplyUiTheme` |
| 14 | الشبكة: `ReadOnly = true` + `AllowUserToAddRows = false` + `SelectionMode = FullRowSelect` + `ContextMenuStrip` + بحث فوري + نقر مزدوج + سطر «لا توجد بيانات» | RULE-N22/N23/N24 | خصائص الشبكة + الأحداث |
| 15 | التفقيط الحيّ إلزامي في شاشات إدخال المال (فواتير · POS · قيود · حسابات · مصروفات · سندات) عبر `LiveTafqeetHelper.Attach(liveTip, …)` — 6 شاشات منفّذة حتى الآن | §2.20 بند 3 + `RULE-N97` | نص `Attach` |
| 16 | `ToolTip` لكل زر يتضمن اختصاره، واختصارات `F2` جديد · `F3` عرض/تعديل · `F4` ترحيل · `F5` تحديث · `Delete` إلغاء · `Esc` خروج | RULE-N47 | `WireGlobalShortcuts` |
| 17 | تحقق بـ `errorProvider` + رسائل عربية + **لا** `ex.Message` للمستخدم (Serilog فقط) | RULE-N28/N29 + FORBIDDEN | `grep "ex.Message"` = 0 |
| 18 | `async void` مع `try/catch` + `Cursor.WaitCursor` واستعادته في `finally` + `CancellationToken` | RULE-N25/N26 | مراجعة المعالجات |
| 19 | المال `decimal` وعرضه `N2`، الكميات `decimal(18,3)` | RULE-001/RULE-002 | أنواع الخصائص |
| 20 | عدم كسر أي سلوك قائم: تُحفظ أسماء العناصر والأحداث المُستدعاة من خارج الشاشة | — | `git diff` محدود |
| 21 | صندوق تصفية موحّد `gbFilter` بأدوات التصفية القياسية والتصفية الفورية | `RULE-N56`…`N63` | عناصر `gbFilter` في المُصمّم — شاشات التقارير تُعفى من `gbFilter` لأنها تُوحَّد بـ `ReportUiStandardizer` (`RULE-N121`) |
| 22 | شريط ملخص (`pnlSummary`) يعدّ النتائج والإجماليات `N2` بعد كل تصفية | `RULE-N61` | `_UpdateSummary()` |
| 23 | كل الشاشات تستدعي الستة من `KeyboardHelper` (تنقل + تحديد + تظليل + أسهم + أزرار + اختصارات) | `RULE-N95` | grep على `WireArrowNavigation` في كل `frm*.cs` |
| 24 | الثيم الأخضر مطبَّق على الشاشة وكل شبكاتها | `RULE-N96` | `ApplyGrid` + آخر سطر `Apply(this)` |
| 25 | زر «إعادة تنشيط» يعمل على السطر غير النشط فقط | `RULE-N90`…`N94` | تجربة يدوية: عطّل ثم أعد التنشيط |
| 26 | كل التصفية داخل `gbFilter` بلا استثناء | `RULE-N56`…`N63` | عناصر `gbFilter` في المُصمّم — شاشات التقارير تُعفى من `gbFilter` لأنها تُوحَّد بـ `ReportUiStandardizer` (`RULE-N121`) |
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
| RULE-N63 | زر اختياري `btnToggleFilters` («إخفاء/إظهار التصفية») يطوي `pnlFilters` إلى ارتفاع 34 للحصول على مساحة أكبر للشبكة، ويُحفظ حالته في `SystemSettings` بمفتاح `UI.FiltersCollapsed` — 🔄 لم يُنفَّذ بعد في أي شاشة (`btnToggleFilters` = 0)؛ يُنفَّذ عند توسيع P2/P3 |

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
| RULE-N68 | `ctrlDashboard` يعمل في وضعي Mock/Real: يجب وجود `MockDashboardDesktopService` مسجَّل في فرع Mock حسب `RULE-N10` |
| RULE-N69 | `ctrlDashboard` ينتهي بـ `GreenThemeHelper.Apply(this)` ويستمع لرسائل `EventBus` ليُحدّث أرقامه دون إعادة فتح |

---

### 2.24 معيار التقارير وقائمة التقارير (Reports Standard)

**تقرير واحد = شاشة واحدة.** لا نماذج بتُبويبات تقارير متعددة.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N70 | كل تقرير شاشة `frm*Report` مستقلة تعرض تقريراً **واحداً** فقط؛ نموذج التُبويبات المتعددة (`frmReports`) يُلغى دوره ولا يُضاف تقرير إليه |
| RULE-N71 | كل تقرير يُسجَّل في DI ويُفتح من **عنصر قائمة واحد** في `frmMain` — لا تكرار ولا شاشة يتيمة |
| RULE-N72 | قائمة «التقارير» تُقسَّم إلى قوائم فرعية ثابتة: **المبيعات** · **المشتريات** · **المخزون** · **المحاسبة والمالية** · **العملاء والموردون** · **الصندوق** — كل تصنيف `ToolStripMenuItem` يحتوي تقاريره فقط |
| RULE-N73 | كل تقرير يستوفي §2.21 **عدا بنود لوحة المفاتيح والتصفية**؛ يُوحَّد مظهره وسلوكه حصراً عبر `ReportUiStandardizer.ApplyForm(this)` + `ReportNavigationHelper.Attach(this, _serviceProvider)` كآخر سطرين في المُنشئ، مع تصدير Excel عبر `IExportableReport` (`RULE-N121`) |
| RULE-N74 | ملفات التقارير في مجلد واحد `Screens/Reports/Forms/`؛ التصنيف يُطبَّق في قائمة `frmMain` فقط (`RULE-N72`) — لا مجلدات `{Sales,Purchases,…}` |
| RULE-N75 | كل `I*ReportDesktopService` تحتاج تنفيذاً وهمياً `Mock*ReportDesktopService` وإلا انكسر وضع التجربة |
| RULE-N76 | لا يُعتبر التصنيف مكتملاً حتى تصل كل تقاريره من القائمة في نقرتين على الأكثر |
| RULE-N77 | لا يوجد مجلد `Tabs` للتقارير: كل تقرير **نموذج واحد** `frm*Report` بملفَي `.cs` + `.Designer.cs` ومنطقه داخله — ممنوع `UserControl` وسيط لكل تقرير، وممنوع إبقاء تاب ميت بلا مرجع |
| RULE-N78 | التنقل بين التقارير يكون **بأزرار داخل النموذج** عبر `ReportNavigationHelper.Attach(this, _serviceProvider)` (سطر واحد كآخر سطر في المُنشئ) — ممنوع `TabControl` وممنوع تكرار شريط الأزرار داخل أي تقرير |

---

### 2.25 معيار نقطة البيع (POS Standard)

`frmPOS` شاشة بيع سريعة لمتجر تجزئة: الأصناف تُعرض للاختيار السريع، والهدف إتمام البيع في أقل عدد خطوات. الشاشة تعليمية/عملية ولا تُضاف إليها وحدات أو شاشات فرعية غير مطلوبة.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N80 | `frmPOS` تُبنى على نمط `frmAddUpdateSalesInvoice`: باركود → شبكة سطور → مجاميع → دفع → ترحيل |
| RULE-N81 | الشاشة تعتمد الاختيار السريع للأصناف: أزرار/قائمة أصناف + حقل باركود، والتركيز على أقل عدد نقرات لإتمام البيع — ولا تُضاف أي وحدة أو تبويب أو خطوة لا يطلبها البيع السريع |
| RULE-N82 | تعمل بوضعي Mock/Real عبر الخمس خدمات المستخدمة فعلياً: `IProductDesktopService` + `ISalesInvoiceDesktopService` + `ICustomerDesktopService` + `IWarehouseDesktopService` + `ICashBoxDesktopService` (+ `IEventBus` للتحديث) — لا وصول مباشر لأي طبقة أخرى |
| RULE-N83 | شبكة سطور POS هي الاستثناء الثاني المسموح لـ `RULE-N22a` — اسمها `dgvCart` في `frmPOS` (مع `dgvItems` في `frmAddUpdateSalesInvoice`)، ولا يُعمَّم على غيرها |
| RULE-N84 | كل بيع يُرحَّل كـ `SalesInvoice` بحالة `Posted` داخل `BeginTransactionAsync` مع خصم المخزون وتسجيل `InventoryTransactions` والقيد المحاسبي |
| RULE-N85 | بعد الحفظ لا تُغلق الشاشة: تُفرَّغ السلة وتُعاد التهيئة لبيع جديد ويبقى التركيز على حقل الباركود |

---

### 2.26 ثابت إعادة التنشيط (Reactivation Invariant)

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N90 | كل كيان يرث `ActivatableEntity` يجب أن يدعم الدورة كاملة: تعطيل → إظهار غير النشط → إعادة تنشيط — 🔄 التعميم جزئي: العميل مكتمل (خدمة + زر)، الأصناف/الفئات/الموردون خدمة بلا زر (`RULE-N122`) |
| RULE-N91 | كل `I*DesktopService` لكيان قابل للتعطيل يعرّض `ReactivateAsync` ويُسندها للـ BLL — لا منطق في الـ wrapper (`RULE-N11`) |
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
| `KeyboardHelper` | `SalesSystem.Desktop/Helpers/KeyboardHelper.cs` | `WireEnterAsTab` · `WireSelectAllOnFocus` · `WireFocusHighlight` · `WireArrowNavigation` · `WireEnterToClick` · `WireButtonDisabledVisual` · `WireGlobalShortcuts` |
| `GreenThemeHelper` | `SalesSystem.Desktop/Helpers/GreenThemeHelper.cs` | `Apply` · `ApplyHeader` · `ApplyGrid` · `TextStrong` |
| `LiveTafqeetHelper` | `SalesSystem.Desktop/Helpers/LiveTafqeetHelper.cs` | `Attach` (تلميح التفقيط الحيّ) |
| `ThemeHelper` | `SalesSystem.Desktop/Helpers/ThemeHelper.cs` | `ApplyMainFormStyle` · `ApplyButtonStyle` (يُستدعى **قبل** الثيم الأخضر) |
| `RowFilterHelper` | `SalesSystem.Desktop/Helpers/RowFilterHelper.cs` | `Escape` · `Like` (تهريب قيمة التصفية وبناء `RowFilter` آمن — `RULE-N24`/`RULE-N60`) |
| `ReportNavigationHelper` | `SalesSystem.Desktop/Helpers/ReportNavigationHelper.cs` | `Attach` (شريط تنقل التقارير داخل النموذج — `RULE-N78`) |
| `ReportUiStandardizer` | `SalesSystem.Desktop/Helpers/ReportUiStandardizer.cs` | `Apply` · `ApplyForm` (توحيد مظهر شاشات التقارير — `RULE-N121`) |
| `ExportHelper` | `SalesSystem.Desktop/Helpers/ExportHelper.cs` | `ExportToExcel` · `ExportToCsv` (التصدير الموحّد للشبكات) |
| `ArabicNumberToWords` | `SalesSystem.Desktop/Helpers/ArabicNumberToWords.cs` | `ToWords` (التفقيط: مبلغ + عملة + كسر) |

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
| 1 | الأدوار والصلاحيات (`PermissionsMask`، `RULE-N31`…`N34`) | مؤجل — مستخدم واحد بصلاحيات كاملة | كل الشاشات متاحة للأدمن؛ لا فحص صلاحيات (المصادقة `RULE-N35`…`N37` مطبَّقة — §2.13) | 🔒 V2 |
| 2 | نطاق DI لكل عملية (بدل نطاق واحد لعمر التطبيق) | مؤجل | نطاق واحد مُنشأ في `Program.cs` | 🔒 V2 |
| 3 | وحدة الموظفين كوحدة مُخزَّنة (لا جدول `Employees` في MVP) | مؤجل — موجودة كواجهة تجريبية | `MockEmployeeDesktopService` في وضع التجربة | 🔒 V2 |

---

### 2.30 عقيدة الكود: WinForms كلاسيكي + N-Layer بسيط (Code Doctrine) — إلزامي

النظام **WinForms كلاسيكي** بطبقات N-Layer بسيطة. الهدف: كود قصير، واضح، بلا تجريدات زائدة، ويُعاد استخدامه من مكان واحد.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N109 | **الواجهة كلاسيكية:** منطق الشاشة في معالجات الأحداث داخل `frm*.cs` (code-behind). ممنوع MVVM: لا أصناف `*ViewModel`، ولا `RelayCommand`/`ICommand` مخصّص، ولا أطر ربط (ReactiveUI / CommunityToolkit.Mvvm / Prism). الاستثناء الوحيد المسموح: `INotifyPropertyChanged` على صنف سطر واحد داخل `BindingList<T>` لأن `DataGridView` يحتاجه لتحديث `LineTotal` — ويُسمّى الصنف باسم الكيان (`InvoiceLineItem`) لا `*ViewModel` |
| RULE-N110 | **الباك-إند N-Layer بسيط:** الطبقات هي `Common` · `Models` · `DTO` · `DataAccess` · `Business` · `Desktop`. ممنوع Clean Architecture: لا مجلدات `Domain`/`Application`/`Infrastructure`/`UseCases`، ولا MediatR/CQRS/Handlers، ولا AutoMapper Profiles، ولا Specification Pattern، ولا مستودعات عامة إضافية فوق `IUnitOfWork` القائم، ولا واجهات تُضاف بلا سلوك مقابل |
| RULE-N111 | **لا طبقة API إطلاقاً:** لا `HttpClient`، لا `System.Net.Http`، لا Controllers، ولا تسلسل JSON في مسار البيانات. التسمية المعتمدة: عقد الشاشة `I<Entity>DesktopService` · التنفيذ الحقيقي `Direct<Entity>Service` (يستدعي BLL داخل العملية) · التنفيذ الوهمي `Mock<Entity>DesktopService` |
| RULE-N112 | **إعادة الاستخدام أولاً:** كل لون/خط/تنقل/اختصار/تفقيط/تهريب فلترة يعيش في `SalesSystem.Desktop/Helpers/` فقط. أي سلوك مشترك جديد يُضاف إلى Helper أولاً ثم تُستهلكه الشاشات — وممنوع نسخه داخل شاشة (`RULE-N98`) |
| RULE-N113 | **بوابة البساطة:** الحل الأصغر القابل للقراءة هو الصحيح. إن تطلّب الطلب أكثر من 3 تجريدات جديدة (واجهة + مصنع + وسيط…) توقّف وأبلغ المدير بسبب الحاجة الفعلية قبل الكتابة |
| RULE-N114 | **الملفات المشتركة تُعدَّل من مكان واحد:** `Program.cs` و`appsettings.json` وملف الحل ومشروع `.csproj` يملكها وكيل واحد فقط في كل مهمة — البقية تُبلّغ المدير بالحاجة ولا تعدّلها |

**مصادر الحقيقة للـ Helpers (لا تُكرَّر قيمها):** `GreenThemeHelper` (كل ألوان النظام) · `KeyboardHelper` (التنقل/التحديد/التظليل/الأسهم/الأزرار/الاختصارات) · `LiveTafqeetHelper` (التفقيط الحيّ) · `RowFilterHelper` (تهريب قيم التصفية) · `ThemeHelper` (تنسيق النموذج/الزر الأساسي، يُستدعى قبل الثيم الأخضر) · `ReportUiStandardizer` (توحيد شاشات التقارير).

---

### 2.31 مسؤولية الاختبار (Testing Responsibility) — الاختبار ليس عمل الوكلاء

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N115 | **الوكلاء لا يكتبون اختبارات آلية ولا يعدّلون `SalesSystem.Tests/**`** — هذا المشروع ملك المالك ويغطي `Common/Helpers` فقط |
| RULE-N116 | **كل تسليم ينتهي بقائمة اختبار يدوي** يكتبها الوكيل في تقريره: جدول (الشاشة · الخطوة · النتيجة المتوقعة) بالعربية، تغطي المسار السعيد + حالات الرفض + الأثر المالي/المخزني، وينفّذها المالك بنفسه |
| RULE-N117 | ممنوع `dotnet test` أو تشغيل التطبيق من أي وكيل. المسموح فقط `dotnet build` للتحقق من الترجمة |
| RULE-N118 | مراجعة الكود/الأمن لا تُبلغ «نقص تغطية اختبارات» كعيب — تُغلق بإحالة إلى هذا القسم |
| RULE-N119 | ملف تسليم لا يحتوي قائمة اختبار يدوي يُعدّ ناقصاً ويُرفض |

---

### 2.32 قواعد نظافة الواجهة والتوثيق (UI Housekeeping)

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N120 | مصفوفة §2.20 وجدول §2.17 وحالات §9 تُحدَّث عند إغلاق كل شاشة/مرحلة — يُمنع بقاء «⬜ لم يبدأ» على ما هو منفّذ |
| RULE-N121 | شاشات `frm*Report` للقراءة فقط تُدار حصراً بـ `ReportUiStandardizer.ApplyForm` + `ReportNavigationHelper.Attach` + `IExportableReport`، وتُعفى من بنود لوحة المفاتيح الستة و`gbFilter` |
| RULE-N122 | كل كيان يرث `ActivatableEntity` جديد يجب أن يخرج بـ `ReactivateAsync` في الواجهة وBLL **وزر إعادة تنشيط في شاشته** — لا يُغلق الكيان دون الدورة كاملة |
| RULE-N123 | لا تبقى مراجع NuGet ميتة (مُعلنة وغير مستخدمة) — تُزال عند أول مساس بملف `.csproj` المعني |
| RULE-N124 | أسماء مجلدات الشاشات تُكتب `Forms` — أي مجلد مكتوب `Froms` يُصحَّح عند أول مساس بمحتواه |

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
❌ MVVM: أي صنف `*ViewModel` أو `ICommand`/`RelayCommand` أو إطار ربط (RULE-N109)
❌ Clean Architecture: Domain/Application/Infrastructure/UseCases/MediatR/CQRS/AutoMapper (RULE-N110)
❌ `HttpClient` / `System.Net.Http` / أي نقطة HTTP — التسمية `I*DesktopService` (RULE-N111)
❌ نسخ لون/خط/اختصار/تنقل داخل شاشة بدل استخدام Helpers (RULE-N112)
❌ كتابة أو تعديل أو تشغيل اختبارات آلية من أي وكيل (RULE-N115/RULE-N117)
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

There is no HTTP surface. Each Desktop data screen talks to the BLL through one `I*DesktopService` interface:

- Interface: `Services/Interfaces/I<Entity>DesktopService.cs`
- Real implementation: `Services/Direct/Direct<Entity>Service.cs` → calls `Business.Interfaces.Services.I<Entity>Service`
- Mock implementation: `Services/Mock/Mock<Entity>DesktopService.cs` → in-memory `MockData`
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
│   ├── Screens/                      ← 23 module folders (Sales, Products, Customers, etc.)
│   │   ├── Sales/                    ← frm* forms for sales
│   │   ├── Products/                 ← frm* forms for products
│   │   ├── Customers/                ← frm* forms for customers
│   │   └── ... (23 modules)
│   ├── Services/                     ← Service implementations
│   │   ├── Direct/                   ← Direct*Service wrappers (I*DesktopService → BLL)
│   │   ├── Interfaces/               ← I*DesktopService interfaces
│   │   └── Mock/                     ← Mock*DesktopService + MockData (in-memory, no DB)
│   ├── Controls/                     ← Custom WinForms controls
│   ├── Helpers/                      ← Desktop utility classes
│   ├── Models/                       ← Desktop-specific models (InvoiceLineItem, UserSession, NavigationItem, Notification, SalesLineDto)
│   ├── Printing/                     ← Desktop print handling
│   └── Configuration/                ← App configuration (AppSettings.cs)
│
└── SalesSystem.Tests/                ← Unit tests for Common/Helpers only (owner-owned)
```

---

## 9. Current Module Status

| Module | Status | Desktop Screens | Business | DB |
|--------|--------|-----------------|----------|----|
| Users | ✅ Done | `frmUserMaster`, `frmPasswordChange`, `frmBackup` | ✅ `UserService` · `AuthService` | ✅ `Users` |
| Roles & Permissions | 🔒 V2 (§2.29) | — (لا `frmRoles`) | — | — (لا كيان `Role`/`PermissionsMask`) |
| Employees | 🔄 Mock فقط | `frmEmployeeMaster` | ❌ لا خدمة BLL | ❌ لا كيان `Employee` |
| Settings | 🔄 BLL+DB بلا شاشة | — (`Screens/Settings` فارغ) | ✅ `CompanySettingsService` · `StoreSettingsService` | ✅ `SystemSettings` |
| Currencies | ❌ Not Implemented | — (لا `frmCurrencies`) | ❌ | ❌ |
| Chart of Accounts | ✅ Done | `frmAccounts` | ✅ `AccountService` | ✅ `Accounts` |
| Journal Entries | ✅ Done | `frmJournalEntries` | ✅ `JournalEntryService` + `AccountingIntegrationService` | ✅ `JournalEntries` |
| Customers | ✅ Done | `frmCustomerMaster`, `frmCustomerSelection`, `frmCustomerStatement` | ✅ `CustomerService` | ✅ `Customers` |
| Suppliers | ✅ Done | `frmSupplierMaster`, `frmSupplierSelection` | ✅ `SupplierService` | ✅ `Suppliers` |
| Categories | ✅ Done | `frmCategoryMaster` | ✅ `ProductCategoryService` | ✅ `ProductCategories` |
| Units | ✅ Done | `frmUnitMaster` | ✅ `UnitService` | ✅ `Units` |
| Taxes | ❌ Not Implemented | — (لا `frmTaxes`) | ❌ | ❌ |
| Warehouses | ✅ Done | `frmWarehouseMaster` | ✅ `WarehouseService` | ✅ `Warehouses` |
| Products | ✅ Done | `frmProductMaster`, `frmProductUnitMaster`, `frmProductSelection`, `frmImportProductsExcel` | ✅ `ProductService` · `ProductUnitService` · `ProductImportService` | ✅ `Products` · `ProductUnits` |
| CashBoxes | ✅ Done | `frmCashBoxMaster`, `frmCashBoxTransactions` | ✅ `CashBoxService` | ✅ `CashBoxes` |
| Banks | ❌ Not Implemented | — (لا `frmBanks`) | ❌ | ❌ |
| Purchases | ✅ Done | `frmListPurchases`, `frmPurchaseInvoice`, `frmPurchases` | ✅ `PurchaseService` | ✅ `PurchaseInvoices` |
| Sales | ✅ Done | `frmListSales`, `frmAddUpdateSalesInvoice` | ✅ `SalesService` | ✅ `SalesInvoices` |
| Sales Returns | ✅ Done | `frmSalesReturn`, `frmListReturns` | ✅ `SalesReturnService` | ✅ `SalesReturns` |
| Purchase Returns | ✅ Done | `frmPurchaseReturn` | ✅ `PurchaseReturnService` | ✅ `PurchaseReturns` |
| Receipts (سندات القبض) | ✅ Done | `frmListCustomerReceipts`, `frmAddUpdateCustomerReceipt` | ✅ `CustomerReceiptService` | ✅ `CustomerReceipts` |
| Payments (سندات الصرف) | ✅ Done | `frmListPayments`, `frmListSupplierPayments`, `frmSupplierPaymentDialog` | ✅ `SupplierPaymentService` · `PaymentVoucherService` | ✅ `SupplierPayments` |
| Reports | ✅ Done | **31** نموذج `frm*Report` في `Screens/Reports/Forms/` | ✅ 5 خدمات تقارير (+ Mocks) | ✅ (تقرأ من الجداول القائمة) |
| Dashboard | ✅ Done | `frmDashboard` (غلاف) + `ctrlDashboard` | ✅ `DirectDashboardService`/`MockDashboardDesktopService` | ✅ (إحصاءات فوق الجداول القائمة) |
| Inventory (Transfers/Adjustments) | ✅ Done | `frmInventory`, `frmListInventoryTransactions`, `frmListInventoryBatches`, `frmListInventoryAdjustments`, `frmOpeningStock`, `frmWarehouseTransferNew`, `frmStocktakingNew` + نوافذ الطباعة | ✅ `InventoryService` · `InventoryBatchService` · `InventoryAdjustmentService` | ✅ `InventoryTransactions` · `InventoryBatches` · `InventoryAdjustments` |
| Expenses | ✅ Done | `frmListExpenses`, `frmAddUpdateExpense` | ✅ `ExpenseService` | ✅ `Expense` (عبر `ExpenseConfiguration`) |
| POS | ✅ Done | `frmPOS` (1371 سطراً) | ✅ `SalesService` (نفس مسار الفاتورة) | ✅ `SalesInvoices` |
| Accounting Engine | ✅ Done | — (auto) | ✅ `AccountingIntegrationService` | ✅ `JournalEntries` |
| Printing | ✅ Done | نوافذ طباعة داخلية (فاتورة شراء/مخزون) | ✅ `Business/Printing` · `DataAccess/Printing` (QuestPDF) | ✅ |

> The former `API` column was removed — there is no HTTP/API layer; every module's Desktop layer goes straight to Business.
>
> **قاعدة:** كل صف في هذا الجدول يُتحقَّق من وجود نموذجه فعلياً في `Screens/<Module>/` قبل ادّعاء «Done» — أسماء لا وجود لها (`frmUsers`, `frmRoles`, `frmSettings`, `frmCurrencies`, `frmTaxes`, `frmBanks`, `frmCategories`, `frmProducts`, `frmWarehouses`, `frmCashBoxes`, `frmSuppliers`, `frmCustomers`, `frmUnits`, `frmSales`, `frmReturns`, `frmReceipts`, `frmPayments`) أُزيلت أو استُبدلت بالاسم الحقيقي (`frm*Master`).

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
 I*DesktopService  →  Direct*Service (Services/Direct/)  or  Mock*DesktopService (Services/Mock/)
```

No HTTP, no controllers, no JSON serialization on the data path. If you find yourself importing `System.Net.Http`, you are in the wrong layer.
