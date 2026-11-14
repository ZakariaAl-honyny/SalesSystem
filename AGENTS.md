# AGENTS.md — Sales Management System (v5.0.0 — 2-Tier WinForms Edition)
# READ THIS FILE FIRST — BEFORE WRITING ANY CODE
# Platform: .NET 10 | 2-Tier Architecture
# Code doctrine: Classic WinForms (NO MVVM) · Simple N-Layer (NO Clean Architecture) · No API layer · Reusable Helpers — one home per shared behaviour (`Helpers/` · `Global Classes/` · `Common/Helpers/`) — see §2.30 (`RULE-N169`…`N172`)
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

**Two-stage law (binding — `RULE-N165`…`N168` in §2.37):** every new screen or feature with a data path is delivered in **Stage A (mock)** and handed to the owner as the **UI & UX QA round** — `frm*` + `I*DesktopService` + `Mock*DesktopService` + in-memory seeds, `UseMockData = true`, no `Business/**`, no `DataAccess/**`, no `Services/Direct/**`, no migration, no real save. **Stage B (real backend + DB save) starts only after the owner's explicit written approval** of the Stage A result. Mock login/credentials and seeded demo data exist only so the owner can review behaviour; they are not a substitute for the approval gate.

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
| RULE-N46 | لون الحقل النشط موحّد: `(214, 228, 240)` = `ThemePalette.ActiveFieldColor` يُقرأ **حيّاً** — تُمرَّر الشاشة `ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)` فيلتقط الـHelper القيمة الحيّة عند كل دخول (`RULE-N54b`)، ويُعاد اللون الأصلي عند `Leave` |
| RULE-N47 | اختصارات موحّدة للشاشات: `F2` = جديد، `F5` = تحديث، `Esc` = إلغاء الحقل/مسحه — وتظهر في `ToolTip` لكل زر |
| RULE-N147 | **Enter = الزر الأساسي للوضع الحالي:** وضع العرض ⇒ «جديد» · وضع الإضافة ⇒ «إضافة/حفظ» · وضع التعديل ⇒ «تعديل». التوصيل حصراً عبر `KeyboardHelper.WireModeEnter(this, _PrimaryActionButton)` في المُنشئ + `AcceptButton = _PrimaryActionButton()` داخل `_ApplyModeState()` — و`Enter` داخل حقل إدخال يبقى «الحقل التالي» (`RULE-N40`)، وعلى زر/مربع اختيار يبقى تنفيذ العنصر نفسه. الشاشة المرجعية: `frmProductMaster` — ✅ منفّذ في **26/26** شاشة تصفية (2026-11-14) |
| RULE-N153 | **Enter داخل شبكة السطور لا يُنفِّذ الزر الأساسي أبداً:** في شاشات المستندات (فواتير · مرتجعات · تسويات · جرد · تحويل · بضاعة أول المدة · POS) الشبكة هي مكان الإدخال السريع بـ`Enter`، فتُعيد `_PrimaryActionButton()` قيمة `null` عند `ContainsFocus`/`IsCurrentCellInEditMode` للشبكة، ويُفرَّغ `AcceptButton` عند دخول الشبكة ويُعاد عند `Leave` — وممنوع ترحيل/حفظ مستند من داخل شبكة السطور، والترحيل المالي يبقى على زرّه و`F4` + تأكيد صريح |

---

### 2.19 الثيم الأخضر الموحّد (Green Theme) — إلزامي لكل الشاشات

الثيم الأخضر هو الثيم الرسمي للنظام. مصدر القيم الوحيد للشاشات هو `GreenThemeHelper` (في `SalesSystem.Desktop/Helpers/`)، وقيمه الحيّة تُقرأ من `ThemePalette` (RULE-N54a)، وممنوع تكرار قيم الألوان داخل أي شاشة.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N50 | كل شاشة تستدعي `GreenThemeHelper.Apply(this)` **كآخر خطوة** في تهيئة المظهر (بعد `ThemeHelper.ApplyMainFormStyle`/`ApplyButtonStyle`)، لأن `ThemeHelper` يفرض خلفية بيضاء وخط `Segoe UI` فيمحو الأخضر |
| RULE-N51 | كل `DataGridView` تُطبَّق عليها `GreenThemeHelper.ApplyGrid(dgv)` (رؤوس خضراء `(14,165,103)` بنص أبيض، خلفية `(220,240,220)`، أسطر متبادلة `(245,252,245)`). `ThemeHelper.ApplyDataGridViewStyle` **ممنوعة** بعدها |
| RULE-N51a | **الثيم لا يمحو ألوان المصمّم.** `GreenThemeHelper.Apply` لا يُعيد تلوين حاوية (`Panel` · `FlowLayoutPanel` · `TableLayoutPanel` · `GroupBox`) في ثلاث حالات: (1) **ورثت** خلفية أبيها لأن المصمّم لم يضبط لها لوناً (Ambient)، (2) كانت معطّلة، (3) لوّنها المصمّم بلونٍ خاص خارج لوحة الثيم. التلوين التلقائي مقصور على حاوية لوّنها المصمّم صراحةً بأحد ألوان الثيم. السبب: فرض `BarBackground`/`GroupBackground` على حاوية ورثت الأخضر يحوّل الشاشة وقت التشغيل إلى رمادي باهت بخلاف المصمّم (`RULE-N134`/`RULE-N139`) |
| RULE-N52 | `ApplyGrid` لا تلمس `ReadOnly` ولا `SelectionMode` ولا `AllowUserToAddRows` ولا `DataSource` — كل شبكة تعلن سلوكها بنفسها حسب `RULE-N22`/`RULE-N22a` |
| RULE-N53 | لون شريط العنوان (`lblMode`/`lblTitle`) يُطبَّق بـ `GreenThemeHelper.ApplyHeader(label)`، والنص الأخضر الداكن للنصوص المهمة هو `GreenThemeHelper.TextStrong` |
| RULE-N54 | ممنوع كتابة قيم ألوان الثيم الأخضر كأرقام داخل الشاشات — تُستهلك من ثوابت `GreenThemeHelper` فقط؛ وأي لون جديد يُضاف هناك لا في الشاشة — ويجب أن تظهر نفس القيم في `*.Designer.cs` بقيم حرفية مطابقة (انظر `RULE-N134`): المصمّم = وقت التشغيل — وتطابق الاتجاهين (المصمّم = التشغيل) معيار مستقل في §2.35 (`RULE-N140`…`N144`). |
| RULE-N54a | **اللوحة القابلة للتخصيص (2026-11-14 — المرحلة أ):** `GreenThemeHelper`/`ThemeHelper` تبقى **الواجهة الوحيدة** التي تستهلكها الشاشات (نفس الأسماء ونفس النوع `Color`)، لكن قيمها تُقرأ وقت العرض من `ThemePalette` (في `Helpers/`) — **24 لوناً** في ست مجموعات (الشاشة الرئيسية · الأزرار · الصناديق والأشرطة · الشبكة · **التحديد والحقل النشط** · حالات العرض) قابلة للتعديل من `frmChangeSystemColorsSettings`. افتراضيات `ThemePalette` هي نفس القيم الحرفية القديمة، فيبقى «المصمّم = التشغيل» صحيحاً على الثيم الافتراضي (`RULE-N134`/`RULE-N140`). **إلزامي:** ممنوع أن تكرّر أي شاشة/خدمة قائمة المفاتيح أو التسميات أو المجموعات أو الافتراضيات — تُستهلك من `ThemePalette.Definitions` وثوابت `ThemePalette.*` فقط؛ وممنوع كتابة قيمة لون جديدة خارج `ThemePalette` (`RULE-N112`). `ThemePalette.ApplyToOpenForms()` هي الطريقة **الوحيدة** لتطبيق تغيير الألوان على الشاشات المفتوحة (بلا إغلاق ولا إعادة إنشاء)، وتُستدعى بعد نجاح الحفظ/الاستعادة فقط. وتبقى `RULE-N51a`/`RULE-N139` ساريَين: لا تُلوَّن حاوية Ambient ولا حاوية بلون مخصّص — التلوين مقصور على ما يحمل لون ثيم حيّاً أو افتراضياً أو سبق أن طبّقته اللوحة |
| RULE-N54b | **التحديد والحقل النشط لونان نظاميان لا لونان محليان:** `SelectionRowColor` (`(255,255,180)`) و`ActiveFieldColor` (`(214,228,240)`) يُستهلكان من `ThemePalette` حصراً — `GreenThemeHelper.ApplyGrid` يبثّ `SelectionRowColor` على `DefaultCellStyle.SelectionBackColor` و`ColumnHeadersDefaultCellStyle.SelectionBackColor` لكل شبكة، و`KeyboardHelper.WireFocusHighlight` يقرأ `ActiveFieldColor` **حيّاً** عند كل دخول حقل بدل اللون الحرفي `0xD6E4F0`، ويعيد تلوين الحقل المركَّز الحالي عبر `RefreshFocusHighlight` من `ThemePalette.ApplyToOpenForms` بلا إغلاق الشاشة. **ممنوع:** كتابة `Color.FromArgb(0xD6,0xE4,0xF0)` أو `(255,255,180)` داخل أي شاشة جديدة (الشاشات القديمة تُحوَّل عند أول مساس بها — `RULE-N141`)، وممنوع `BackColor`/`SelectionBackColor` محلي لأي شبكة أو حقل نشط |
| RULE-N54c | **الاقتراح الذكي لا يُكرَّر حكمه:** منطق اقتراح الألوان وقواعد التباين يعيش مرة واحدة في `ColorSuggestionHelper` (الاقتراح · السلم · الأفضل · القبول) فوق `ThemePalette.ContrastRatio`/`MinimumContrastFor`/`ContrastPartnerColor`. أي شاشة/خدمة تحتاج حكماً على لون تستهلك هذه الدوال — وممنوع نسخ معادلة WCAG أو خريطة الألوان الشريكة أو عتبات التباين في شاشة أو في تنفيذ Mock (`RULE-N169`…`RULE-N172`)، وممنوع أن تقترح الأداة لوناً يمكن أن يرفضه الحفظ (المقترَحات مُتحقَّق منها مسبقاً — `RULE-N167`) |
| RULE-N154 | **مظهر التعطيل موحّد كالأزرار المعطّلة (لكل الشاشات):** كل عنصر إدخال معطّل — `TextBox` · `MaskedTextBox` · `ComboBox` · `NumericUpDown` · `DateTimePicker` · `Button` — يأخذ خلفية `GreenThemeHelper.DisabledBack` = `Color.FromArgb(224, 224, 224)` ونصاً `DisabledFore` = `Color.FromArgb(150, 150, 150)` **تلقائياً** عبر `KeyboardHelper.WireDisabledVisuals` الذي يستدعيه `GreenThemeHelper.Apply` (آخر سطر ثيم في كل شاشة) — فلا يُكتب لون تعطيل داخل أي شاشة ولا يُكرَّر (`RULE-N112`)، ويُعاد لون المصمّم حرفياً فور التفعيل؛ و`CheckBox`/`RadioButton` يُباهت نصّهما فقط بلا مسحة رمادية خلف النص. لونا التعطيل **لونا حالة** لا لونا تصميم: لا يُعلنان في `*.Designer.cs` (الاستثناء الوحيد على `RULE-N134`) لأنهما يُطبَّقان فقط ما دام `Enabled == false` |

**قيم الثيم الأخضر:**

| العنصر | القيمة |
|---|---|
| خلفية النموذج | `Color.FromArgb(170, 220, 170)` |
| شريط العنوان | `Color.FromArgb(14, 165, 103)` + نص أبيض |
| صناديق المجموعات | `Color.FromArgb(235, 245, 235)` |
| أشرطة الأدوات/الإجراءات | `Color.FromArgb(235, 240, 235)` |
| خلفية الشبكة | `Color.FromArgb(220, 240, 220)` · خطوط `(205, 225, 205)` |
| رؤوس الأعمدة | `(14, 165, 103)` + نص أبيض، Tahoma 10 Bold |
| التحديد (سطر/خلية الشبكة) | `Color.FromArgb(255, 255, 180)` — من `ThemePalette.SelectionRowColor` عبر `GreenThemeHelper.ApplyGrid` (`RULE-N54b`) |
| الحقل النشط (تظليل التركيز) | `Color.FromArgb(214, 228, 240)` — من `ThemePalette.ActiveFieldColor` عبر `KeyboardHelper.WireFocusHighlight` حيّاً (`RULE-N54b`) |
| نص مميّز | `Color.FromArgb(14, 82, 52)` |
| عنصر معطّل (حقل أو زر) | خلفية `Color.FromArgb(224, 224, 224)` + نص `Color.FromArgb(150, 150, 150)` — من `DisabledBack`/`DisabledFore` عبر `KeyboardHelper.WireDisabledVisuals` (`RULE-N154`) |

---

### 2.20 مصفوفة تعميم الميزات (Feature Rollout Matrix)

الميزات التالية إلزامية في كل شاشة، وتُعمَّم على المراحل التالية. أي شاشة تُنشأ أو تُعدَّل يجب أن تخرج وقد التزمت بكل الميزات المعلّمة «إلزامي».

| # | الميزة | المرجع | الحالة | ملاحظات |
|---|---|---|---|---|
| 1 | تنقل لوحة المفاتيح (Enter/Tab + أسهم RTL + تحديد القيمة + تظليل الحقل) | `RULE-N40`…`RULE-N47` + `KeyboardHelper` | 🔄 تعميم جارٍ | ✅ **2026-11-13 (عدّ فعلي): 55 من 93 نموذجاً `frm*` تستدعي الستة كاملة** (`WireEnterAsTab` + `WireSelectAllOnFocus` + `WireFocusHighlight` + `WireArrowNavigation` + `WireButtonDisabledVisual` + `WireGlobalShortcuts`) — والعدّ لكل دالة على حدة: `WireArrowNavigation` في **55** · `WireButtonDisabledVisual` في **56** · `WireSelectAllOnFocus` و`WireFocusHighlight` في **56** · `WireEnterAsTab` في **57** · `WireGlobalShortcuts` في **61**. المتبقي من الشاشات الملزمة (62 نموذجاً غير تقريري): `frmLogin` (5 من 6 — بلا `WireArrowNavigation`) · `frmMain` (شاشة هيكلية) · 5 نوافذ طباعة (`frmStocktakingPrint` · `frmStocktakingPrintCard` · `frmStocktakingPrintAdjustment` · `frmWarehouseTransferPrint` · `frmPurchaseInvoicePrint`) تستدعي `WireGlobalShortcuts` فقط. و**31 تقريراً** بلا استدعاء لأنها معفاة (`RULE-N121`) |
| 2 | الثيم الأخضر في وقت التشغيل | `RULE-N50`…`RULE-N54` + `GreenThemeHelper` | 🔄 تعميم جارٍ | `frmAddUpdateSalesInvoice` هي الشاشة المرجعية — ومنها **مطابقة المصمّم**: كل شاشة تُظهر نفس الألوان في المصمّم ووقت التشغيل (`RULE-N134`). ✅ **2026-11-13 (عدّ فعلي): `GreenThemeHelper.Apply(this)` في 62 من 62 نموذجاً غير تقريري** (93 نموذجاً − 31 تقريراً) بما فيها نوافذ الطباعة، وفي `ctrlDashboard` كذلك · `ApplyGrid` في **53** موضعاً · و**31 تقريراً** تُوحَّد بـ`ReportUiStandardizer.ApplyForm` (31/31) لا بالثيم المباشر (`RULE-N121`) · `SystemColors.Control` غائب عن كل المصمّمات إلا موضعاً واحداً في `frmMain.Designer`. المتبقي: مطابقة القيم الحرفية في مصمّمات الشاشات غير التقريرية (البند 12) · ✅ **2026-11-14: مظهر التعطيل موحّد كالأزرار المعطّلة في الشاشات كلها تلقائياً** — `GreenThemeHelper.DisabledBack/DisabledFore` + `KeyboardHelper.WireDisabledVisuals` المستدعى من `GreenThemeHelper.Apply` (RULE-N154) |
| 3 | التفقيط الحيّ كتلميح (ToolTip) على الحقول الرقمية | `LiveTafqeetHelper` | 🔄 تعميم جارٍ | ✅ **2026-11-13 (عدّ فعلي): 22 نموذجاً تستدعي `LiveTafqeetHelper.Attach`** (كانت 6 قبل الموجة) — `frmAddUpdateSalesInvoice` · `frmPOS` · `frmJournalEntries` · `frmAccounts` · `frmListExpenses` · `frmAddUpdateExpense` · `frmEmployeeMaster` · `frmCustomerMaster` · `frmSupplierMaster` · `frmSupplierStatement` · `frmAddUpdateCustomerReceipt` · `frmSupplierPaymentDialog` · `frmAddUpdateInventoryAdjustment` · `frmInventoryAddProduct` · `frmOpeningStock` · `frmPurchaseInvoice` · `frmPurchaseItem` · `frmPurchaseCurrency` · `frmPurchaseDiscount` · `frmPurchaseOtherDetails` · `frmPurchaseReturn` · `frmSalesReturn`. المتبقي (شاشات حركة مالية بلا `Attach` حتى الآن): `frmWarehouseTransferNew` · `frmStocktakingNew` · `frmCashBoxTransactions` |
| 4 | شبكة الدليل: بحث فوري + نقر مزدوج + ContextMenu | `RULE-N23` + `RULE-N24` | 🔄 تعميم جارٍ | النمط المرجعي `frmProductMaster` — ✅ **2026-11-13 (عدّ فعلي): 83 شبكة `DataGridView` في 76 مصمّماً** (52 شبكة في 45 مصمّماً غير تقريري + 31 شبكة تقريرية)، و`ContextMenuStrip` معلن ومُسنَد في **33 مصمّماً غير تقريري** (33 من 45). المتبقي **12 شاشة شبكة بلا `ContextMenuStrip`**: `frmProductMaster` · `frmProductUnitMaster` · `frmPOS` · `frmPurchaseInvoice` · `frmPurchaseReturn` · `frmSalesReturn` · `frmImportProductsExcel` · `frmInventoryAddProduct` · `frmStocktakingPrintCard` · والمنتقيات الثلاث `frmCustomerSelection` · `frmProductSelection` · `frmSupplierSelection` — والتقارير لا تحتاجه (شاشات عرض `RULE-N121`) |
| 5 | مؤشر الوضع + آلة حالة الأزرار | `RULE-N43`/`RULE-N44` | 🔄 تعميم جارٍ | ✅ **2026-11-13 (عدّ فعلي): `WireButtonDisabledVisual` في 56 نموذجاً** (كانت 23) — أي كل الشاشات ذات الستة كاملة (55) + `frmLogin` · و`lblMode` معلن في **47 من 63 مصمّماً غير تقريري** (المتبقي: النوافذ المساعدة والمنتقيات وشاشات الطباعة) |
| 6 | صندوق التصفية الموحّد `gbFilter` + تصفية فورية + مربعات اختيار + شريط ملخص | `RULE-N56`…`N63` | 🔄 تعميم جارٍ | ✅ **2026-11-13 (عدّ فعلي): 26 شاشة غير تقريرية فيها `gbFilter` + `pnlSummary` + `btnToggleFilters` معاً — تعادل تام 26/26: كل شاشة تُعلن `gbFilter` تُعلن `btnToggleFilters` في المصمّم (`RULE-N145`)، وصفر شاشة ناقصة** · `gbFilter` معلن في **54 مصمّماً** (26 غير تقريري + 28 تقريراً) · `pnlSummary` في **59** (28 غير تقريري + 31 تقريراً) · `chkShowInactive` في **12 مصمّماً**. النمطان المرجعيان: `frmListSales` و`frmProductMaster`. المتبقي **19 شاشة شبكة بلا `gbFilter`**: منها مستندات/محررات (`frmAddUpdateSalesInvoice` · `frmPurchaseInvoice` · `frmPurchaseReturn` · `frmSalesReturn` · `frmPOS` · `frmOpeningStock` · `frmStocktakingNew` · `frmWarehouseTransferNew` · `frmAddUpdateInventoryAdjustment` · `frmImportProductsExcel` · `frmInventoryAddProduct`) ومنتقيات ونوافذ مساعدة (`frmProductSelection` · `frmCustomerSelection` · `frmSupplierSelection` · `frmCustomerStatement` · `frmPurchaseItemInfo` · `frmInventoryFifo` · `frmStocktakingPrintCard` · `frmBackup`) — ✅ **2026-11-14: 26/26 شاشة تصفية تُعلن `btnClearFilters` في المصمّم** ويُفصل «مسح التصفية» عن «تحديث» (RULE-N148) |
| 7 | لوحة التحكم كمكوّن `ctrlDashboard` قابل للتضمين | `RULE-N64`…`N69` | ✅ منفّذ | `Screens/Dashboard/Controls/ctrlDashboard` + غلاف `frmDashboard` + `DirectDashboardService`/`MockDashboardDesktopService` + `IEventBus` |
| 8 | تقرير واحد لكل شاشة + تصنيفات قائمة التقارير | `RULE-N70`…`N76` | ✅ منفّذ | **31 تقريراً** في `Screens/Reports/Forms/**` موزّعة على ستة مجلدات تصنيفية (المبيعات · المشتريات · المخزون · المحاسبة والمالية · العملاء والموردون · الصندوق)، نموذج واحد لكل تقرير + تنقل داخلي بـ `ReportNavigationHelper` + خمسة `Mock*ReportDesktopService` |
| 9 | نقطة بيع متجر تجزئة بنمط `frmAddUpdateSalesInvoice` | `RULE-N80`…`N85` | ✅ منفّذ | `frmPOS` (1371 سطراً) + شبكة `dgvCart` + خمس خدمات Desktop (منتج · فاتورة بيع · عميل · مستودع · صندوق) |
| 10 | دورة «تعطيل → إظهار غير النشط → إعادة تنشيط» | `RULE-N90`…`N94` | 🔄 بدأ | ✅ **2026-11-13 (عدّ فعلي): ثلاثة كيانات مكتملة الخدمة والزر — العميل (`frmCustomerMaster`) والصنف (`frmProductMaster`) والمورد (`frmSupplierMaster`)**: `ReactivateAsync` معرَّضة في `ICustomerDesktopService` و`IProductDesktopService` و`ISupplierDesktopService` فقط، وزر `btnReactivate` معلن في المصمّمات الثلاثة · `chkShowInactive` معلن في **12 مصمّماً**. المتبقي: الفئات · الوحدات · المستودعات · الصناديق · المصروفات · المستخدمون — ✅ **2026-11-14: اكتمل التعميم 9/9** — أُضيف `ReactivateAsync` إلى `IUnitService`/`IWarehouseService`/`IUserService` في BLL (وإلى `I*DesktopService` + `Direct*Service` + `Mock*DesktopService` للفئة/الوحدة/المخزن/الصندوق/المستخدم/الموظف) وزر «↺ إعادة تنشيط» في `frmCategoryMaster` · `frmUnitMaster` · `frmWarehouseMaster` · `frmCashBoxMaster` · `frmUserMaster` · `frmEmployeeMaster` · وأُصلح `DirectUserService.GetAllAsync` (كان يعيد النشطين فقط في الوضع الحقيقي فتُقطع الدورة) |
| 11 | ترتيب وتجميع أجزاء الشاشة (عنوان · أوامر · تصفية · محتوى · إجماليات · أسفل) | `RULE-N135`…`N138` | 🔄 بدأ | الشاشة النموذج المنفَّذة: `frmAccountStatementReport` — وبقية الشاشات عند أول مساس بها · ✅ **التقارير الـ31 كلها محقَّقة (2026-11-13، عدّ فعلي): `pnlReportRoot` معلن في 31/31 و`pnlReportNav` في 31/31، بستة صفوف + كتل معنونة («أوامر التقرير» · «بحث وتصفية» · «بيانات التقرير» · «الإجماليات» · الأسفل) — 28 تقريراً بـ`gbFilter` و3 بلا كتلة تصفية (`frmExpiredProductsReport` · `frmLowStockReport` · `frmStockBalanceReport`)، وصفر صندوق داخل صندوق وصفر صندوق فارغ** · الشاشات غير التقريرية: **128 صندوق مجموعة في 63 مصمّماً**، منها **25** مصمّماً فيه كتلة «بحث وتصفية» معنونة و**21** فيه كتلة «أوامر» — والباقي يُطبَّق عند أول مساس بالشاشة |
| 12 | تطابق ألوان التشغيل مع المصمّم (بلا رمادي · حقول بيضاء · الشاشة المرجعية `frmProductMaster`) | `RULE-N140`…`N144` | 🔄 بدأ | ✅ **التقارير (2026-11-13، عدّ فعلي): 47/47 حقل إدخال في مصمّمات التقارير الـ31 بخلفية `Color.White` وخط `Tahoma` — صفر استثناء، و`SystemColors.Control` = 0** · الشاشات غير التقريرية: **62/62 تستدعي `GreenThemeHelper.Apply(this)`** و`SystemColors.Control` غائب عنها كلها (موضع واحد فقط في `frmMain.Designer`)، لكن **193 من 280 حقل إدخال (69%) فقط تُعلن `Color.White` صريحاً في المصمّم** — أي **87 حقلاً** تتلوّن وقت التشغيل فقط (معظمها `TextBox` في الماسترات: `frmProductMaster` 12 حقلاً · `frmListSales` · `frmListReturns` · `frmListPurchases` · `frmPurchaseItem` · `frmEmployeeMaster` …). **البند مفتوح** ويُغلق شاشة بشاشة (`RULE-N143`) |
| 13 | وضع الإدخال: الحقول معطّلة افتراضياً + آلة حالة `Browsing/Adding/Editing` + تحديث تلقائي بعد الحفظ/الحذف | `RULE-N149`…`N152` | ✅ **2026-11-14: مكتملة 10/10** — `frmProductMaster` (المرجع) + `frmCategoryMaster` · `frmUnitMaster` · `frmWarehouseMaster` · `frmCashBoxMaster` · `frmUserMaster` · `frmEmployeeMaster` · `frmSupplierMaster` · `frmCustomerMaster` · `frmProductUnitMaster` — كلها بـ`EntryMode` + `_ApplyModeState` + تحديث تلقائي + `btnClearFilters` |
| 14 | ألوان النظام قابلة للتخصيص من مكان واحد (`frmChangeSystemColorsSettings` + `ThemePalette`) | `RULE-N54a` + `RULE-N54b` + `RULE-N54c` + §2.19 | 🔄 **المرحلة أ (Mock) — 2026-11-14**: **24 لوناً في ست مجموعات** (منها «التحديد والحقل النشط») · `ThemePalette.Definitions` مصدر وحيد · `GreenThemeHelper`/`ThemeHelper` خصائص تمرير · `ApplyToOpenForms` تُعيد تلوين المفتوح فوراً (بما فيه الحقل المركَّز عبر `KeyboardHelper.RefreshFocusHighlight`) · `ApplyGrid` يبثّ لون السطر المحدد على كل شبكة · `ColorSuggestionHelper` (اقتراح/سلم/أفضل/قبول فوق `ThemePalette.ContrastRatio`) · `IThemeColorDesktopService` + `MockThemeColorDesktopService` (رفض عربي + تحقق تباين WCAG بلا نسخة ثانية من القواعد) — **لا BLL/DB بعد**؛ المرحلة ب تبدأ فقط بموافقة المالك المكتوبة (`RULE-N166`) |
| 15 | **طبقة الكود المشترك `Global Classes/`** (بيت واحد يُستهلك من كل الشاشات — `RULE-N169`…`N172`) | `RULE-N169`…`N172` + §2.30 | 🔄 **2026-11-14**: الخمسة مكتوبة/مُعاد بناؤها في بيتها الواحد — `clsGlobal` (فتح نماذج آمن) · `clsFormat` (`Money`/`Quantity`/`Number`/`Percent`/`DateToShort`/`DateToLong`/`TryParse*`) · `clsDialogs` (Info/Success/Warning/Error/Error(ex,msg)/Confirm بعناوين عربية ثابتة + Serilog) · `clsValidation` (تحقق) · `clsInputFeedback` (تلميح رفض/معلومة/نجاح بلا تسريب مؤقّتات) — و`clsValidatoin` القديم حُذف (صفر مستهلكين). **الاستهلاك بدأ فعلياً: 87 استبدال تنسيق في 25 ملفاً + 75 `MessageBox` في 4 شاشات.** المتبقي: بقية `MessageBox` (نحو 770 موضعاً) تُرحَّل شاشة بشاشة عند أول مساس بها (`RULE-N169`) |
| 16 | إعدادات التنسيق من مكان واحد (`frmFormatingSetting` + `clsFormat`) | `RULE-N169`…`N172` + §2.30 | 🔄 **المرحلة أ (Mock) — 2026-11-14**: **10 إعدادات في مجموعتين** («المبالغ والأرقام»: منازل المبالغ 2 · الكميات 3 · الأعداد 0 · النسب 2 · فاصل الآلاف · فاصل الكسر · أرقام عربية ٠-٩ — و«التواريخ»: صيغتا `dd/MMM/yyyy` و`dd/MM/yyyy` و`dd/MM/yyyy HH:mm`) · **`clsFormat` هو البيت الواحد**: `Definitions` + القيم الحيّة + `Validate`/`ValidateAll` + `TryApplyAll` (ذرّي: الدفعة كاملة أو لا شيء) + المنسّقات · **معاينة فورية بالمسودة** عبر نفس المنسّقات (لا صيغة مكرّرة في الشاشة) · `IFormatSettingsDesktopService` + `MockFormatSettingsDesktopService` (رفض عربي: مفتاح مفقود/غير معروف · منازل خارج 0–4 · «,» مع فاصل الآلاف · صيغة تاريخ غير صالحة) — **لا BLL/DB بعد**؛ المرحلة ب بانتظار موافقة المالك المكتوبة (`RULE-N166`) |

**مراحل التعميم:**

| المرحلة | الشاشات | معيار الإغلاق |
|---|---|---|
| P1 — الشاشات اليومية | `frmListSales`, `frmAddUpdateSalesInvoice`, `frmProductMaster`, `frmProductUnitMaster`, `frmProductSelection`, `frmListPurchases`, `frmPurchaseInvoice`, `frmListReturns`, `frmSalesReturn`, `frmPurchaseReturn` | كل شاشة: ثيم أخضر + تنقل كامل + شبكة خضراء + تجربة يدوية |
| P2 — البيانات الأساسية | Categories, Units, Customers, Suppliers, Warehouses, CashBoxes, Employees, Users, Expenses, Payments, CustomerReceipts | كما أعلاه |
| P3 — التقارير والمخزون والباقي | `Screens/Reports/Forms/**` (31 تقريراً في ستة مجلدات تصنيفية), `Screens/Inventory/**` (16), Dashboard, POS, Tools, Login | كما أعلاه، مع مراعاة أن شاشات التقارير تُعفى من بنود لوحة المفاتيح و`gbFilter` (`RULE-N121`) وشاشات الطباعة/المعاينة تُعفى من تعبئة النماذج |

> **قاعدة:** تُنفَّذ المراحل بالترتيب P1 → P2 → P3، ولا تُعتبر الشاشة «مكتملة» حتى تلتزم بالميزات 1 و2 أعلاه.

> **موجة تعميم 2026-11-13:** رُحِّلت **62 شاشة** (كل النماذج غير التقريرية من أصل 93 نموذجاً `frm*` في `Screens/**`) إلى معيار §2.21/§2.27 مع الالتزام بـ§2.34 (الترتيب والتجميع) و§2.35 (التشغيل = المصمّم): **62/62** تستدعي `GreenThemeHelper.Apply(this)` · **55** تستدعي الستة كاملة من `KeyboardHelper` و**56** تستدعي `WireButtonDisabledVisual` · **22** تستدعي `LiveTafqeetHelper.Attach` · **26/26** شاشة تصفية تُعلن `gbFilter` + `pnlSummary` + `btnToggleFilters` في المصمّم (تعادل تام · `RULE-N145`) · **33 من 45** شاشة شبكة تُعلن `ContextMenuStrip` (`RULE-N23`) · **3** ماسترات أتمّت دورة إعادة التنشيط (عميل · صنف · مورد) · و**31/31** تقريراً موحَّدة بـ`ReportUiStandardizer` + `ReportNavigationHelper` + `pnlReportNav` معلن في المصمّم.
>
> **البنود المفتوحة (لم تُنفَّذ فعلاً):** دورة إعادة التنشيط في الماسترات التي لا تعرّض `ReactivateAsync` (الفئات · الوحدات · المستودعات · الصناديق · المصروفات · المستخدمون) · **87 حقلاً من 280** لا يُعلن لوناً أبيض في المصمّم (`RULE-N143`) · **12 شاشة شبكة** بلا `ContextMenuStrip` · **19 شاشة شبكة** بلا `gbFilter` · **7 نماذج** لا تستدعي الستة كاملة (`frmLogin` · `frmMain` · 5 نوافذ طباعة) · توحيد ألوان الحالة الدلالية كثوابت في `GreenThemeHelper`.
>
> **✅ مغلق (2026-11-13):** المجلد `Screens/Dashboard/Froms` أُعيدت تسميته إلى `Forms` (نقل خالص بلا تغيير محتوى ولا `namespace` — الـ`namespace` كان أصلاً `SalesSystem.Desktop.Screens.Dashboard.Forms`) — `RULE-N124` **مُغلَق**، ولم يبقَ منه بند مفتوح.

---

### 2.21 قائمة الإلزام لكل نموذج جديد (New Form Definition of Done)

أي نموذج جديد `frm*` — أو نموذج يُعاد بناؤه بالكامل — **لا يُعتبر مكتملاً** حتى يجتاز هذه القائمة بالكامل. تُراجَع قبل تسليم أي مهمة تُنشئ أو تعيد بناء شاشة، وتُذكر نتائجها في تقرير التسليم.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N55 | إنشاء نموذج بدون استيفاء بنود §2.21 يُعدّ عملاً غير مكتمل ويُرفض في المراجعة — بلا استثناء إلا شاشات المعاينة/الطباعة للقراءة فقط |
| RULE-N79 | هيكلان مرجعيان فقط للشاشات: (أ) مستند/قائمة = `pnlHeader`+`pnlToolbar`+`pnlFilters`+`pnlSummary`+`pnlGrid`+`pnlFooter` (ب) `frm*Master` مدمجة = محرر + `gbFilter` + `pnlSummary` + `pnlGrid` — و`lblMode` إلزامي في الحالتين — والترتيب والتجميع داخل هذه الهياكل كما في §2.34 (`RULE-N135`…`N138`) |

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
| 10 | تظليل الحقل النشط `(214,228,240)` = `ThemePalette.ActiveFieldColor` حيّاً | RULE-N46 + RULE-N54b | معامل `WireFocusHighlight` (يُمرَّر `ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)` لا مفتاحاً نصياً) |
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
| 29 | **المصمّم = وقت التشغيل:** كل لون/خط يطبّقه وقت التشغيل على عنصر موجود في المصمّم مُعلَن بقيمة حرفية مطابقة في `*.Designer.cs`، ووقت التشغيل لا يُعيد تلوين أي سطح لوّنه المصمّم | `RULE-N134` + `RULE-N139` + `RULE-N51a` | افتح النموذج في مصمّم VS وقارنه بالتشغيل (أخضر الشريط/الشبكة/الأزرار/التسميات) + `grep Color.FromArgb` في المصمّم مقابل ثوابت `GreenThemeHelper`، وتأكد أن اللوحات وصناديق المجموعة والتصفية والملخّص تحتفظ بلون المصمّم، وأن كل **عنصر دائم** تراه وقت التشغيل (زر «إخفاء/إظهار التصفية» · شرائط · حقول) موجود في المصمّم ولا يُبنى وقت التشغيل فقط (`RULE-N145`) + شريط تنقل التقارير (`pnlReportNav`) ظاهر في المصمّم لا وقت التشغيل فقط (`RULE-N146`) — ✅ **رُحِّلت شاشات الإدخال والقوائم والماسترات في 2026-11-13:** 62/62 تستدعي `GreenThemeHelper.Apply(this)` · 26/26 شاشة تصفية تُعلن `btnToggleFilters` في المصمّم · `SystemColors.Control` = 0 (موضع واحد في `frmMain.Designer`) · 31/31 تقريراً `pnlReportNav` معلن — **مع بقاء ما لم يُمسّ:** 87 حقلاً من 280 بلا لون أبيض معلن في المصمّم · 12 شاشة شبكة بلا `ContextMenuStrip` · 19 شاشة شبكة بلا `gbFilter` |
| 30 | **الترتيب والتجميع:** الكتل بالترتيب الثابت وكل كتلة داخل صندوق مجموعة بعنوان | `RULE-N135`…`N138` | جدول العناصر في تقرير التسليم + فتح الشاشة وعدّ الصناديق والعناوين من أعلى لأسفل |
| 31 | **الألوان: التشغيل = المصمّم بلا رمادي** — لا عنصر على `SystemColors.Control`/الافتراضي، والحقول بيضاء، وألوان الشاشة من ثوابت `GreenThemeHelper` | `RULE-N140`…`N144` | صورة للمصمّم مقابل صورة للتشغيل لنفس الشاشة + `grep SystemColors.Control` في المصمّم = 0 + جدول الألوان في تقرير التسليم |
| 32 | **لا تكرار — المكان الواحد:** كل سلوك/دالة مشتركة يستهلكها الشاشة من بيتها المعلن (`Helpers/` · `Global Classes/` · `Common/Helpers/` · `Business/Helpers/`) ولا نسخة ثانية داخل `frm*.cs`، وأي سلوك جديد ذُكر بيته في تقرير التسليم | `RULE-N169`…`N172` | `grep` على اسم السلوك في `Screens/**` = مواضع استهلاك فقط (بلا جسم دالة مكرّر) + جدول «بيوت الكود المشترك» في تقرير التسليم |

**الهيكل المرجعي للمُنشئ وشريط المظهر:**

```csharp
public frmNewScreen(IServiceProvider serviceProvider)
{
    _serviceProvider = serviceProvider;
    InitializeComponent();

    // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط
    KeyboardHelper.WireEnterAsTab(this);
    KeyboardHelper.WireSelectAllOnFocus(this);
    KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)); // RULE-N54b — لون حيّ من اللوحة
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
| RULE-N63 | زر `btnToggleFilters` («إخفاء/إظهار التصفية») يطوي `pnlFilters` إلى ارتفاع 34 للحصول على مساحة أكبر للشبكة، ويُحفظ حالته في `SystemSettings` بمفتاح `UI.FiltersCollapsed` — ✅ منفّذ عبر `FilterPanelHelper.Attach` (`F9` + تلميح + حالة محفوظة)، والزر **مُعلَن في `*.Designer.cs`** ويتبنّاه الـHelper (`RULE-N145`) — ✅ **26/26 شاشة تصفية (2026-11-14)** |
| RULE-N148 | **فصل «مسح التصفية» عن «تحديث»:** زر `btnClearFilters` («مسح التصفية» · `Tahoma 9 Bold` · `GreenThemeHelper.ActionNeutralBackground` · **داخل `gbFilter`** · معطّل ما لم توجد تصفية نشطة — `RULE-N43`) يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي **بلا أي إعادة تحميل** ولا لمس للمحرر؛ وزر `btnRefresh` (`F5`) يبقى هو المسؤول عن **إعادة تحميل كل بيانات الشاشة** (الدلائل + الشبكة) و**إرجاع المحرر وكل الإعدادات إلى الافتراضي** (تصفية فارغة · وضع العرض · حقول معطّلة). ممنوع توحيدهما في زر واحد أو استخدام «تحديث» لمسح التصفية فقط. الشاشة المرجعية: `frmProductMaster` — ✅ منفّذ في **26/26** شاشة تصفية (2026-11-14) |

**مخطط عناصر `gbFilter`:**

| العنصر | النوع | الموضع داخل `gbFilter` |
|---|---|---|
| `label2` «تصفية بـ:» + `cbFilterBy` | ComboBox | الصف الأول — أقصى اليمين |
| `txtFilterValue` | TextBox | الصف الأول — يسار `cbFilterBy` |
| `label1` «الحالة:» + `cbStatus` | ComboBox | الصف الأول |
| `label3` «من:» + `dtpFrom` · `label4` «إلى:» + `dtpTo` | DateTimePicker | الصف الأول |
| `btnRangeToday` «اليوم» · `btnRangeMonth` «هذا الشهر» · `btnRange30` «آخر 30 يوماً» | Button (Tahoma 9 Bold) | الصف الثاني — يمين |
| `chkShowInactive` · `chkOnlyDue` · `chkOnlyCancelled` · `chkOnlyOutOfStock` | CheckBox | الصف الثاني — يسار |
| `btnClearFilters` «مسح التصفية» | Button (Tahoma 9 Bold · `ActionNeutralBackground`) | بجانب مربعات الاختيار — معطّل ما لم توجد تصفية نشطة (`RULE-N148`) |
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
| RULE-N74 | تُنظَّم ملفات التقارير في `Screens/Reports/Forms/` داخل **ستة مجلدات تصنيفية** تعكس قوائم `RULE-N72`: `Sales/` (8) · `Purchases/` (3) · `Inventory/` (7) · `Accounting/` (8) · `CustomersSuppliers/` (3) · `CashBox/` (2) — والمجلد الذي يوضع فيه التقرير يُحدَّد بتصنيفه في القائمة (`frmMain` / `ReportNavigationHelper.Catalog`)؛ و`namespace` كل تقرير يبقى `SalesSystem.Desktop.Screens.Reports.Forms` **ولا يُشتق من المجلد** (تسمية متعمَّدة — نقل الملفات لا ينعكس على `frmMain` ولا `Program.cs` ولا `Catalog` ولا تسجيلات DI، فلا «يُصحَّح» أحد الـ namespace ليطابق المجلد)؛ وإضافة تقرير = وضع ملفاته الثلاثة (`frm*.cs` + `frm*.Designer.cs` + `frm*.resx`) في مجلد تصنيفه ثم تسجيله في DI و`frmMain` و`Catalog` — والتصنيف يجب أن يملك مجلداً، ولا مجلد علوي جديد بلا تصنيف قائمة |
| RULE-N75 | كل `I*ReportDesktopService` تحتاج تنفيذاً وهمياً `Mock*ReportDesktopService` وإلا انكسر وضع التجربة |
| RULE-N76 | لا يُعتبر التصنيف مكتملاً حتى تصل كل تقاريره من القائمة في نقرتين على الأكثر |
| RULE-N77 | لا يوجد مجلد `Tabs` للتقارير: كل تقرير **نموذج واحد** `frm*Report` بملفَي `.cs` + `.Designer.cs` ومنطقه داخله — ممنوع `UserControl` وسيط لكل تقرير، وممنوع إبقاء تاب ميت بلا مرجع |
| RULE-N78 | التنقل بين التقارير يكون **بأزرار داخل النموذج** عبر `ReportNavigationHelper.Attach(this, _serviceProvider)` (سطر واحد كآخر سطر في المُنشئ) — ممنوع `TabControl` وممنوع تكرار شريط الأزرار داخل أي تقرير — والنقر يفتح التقرير الهدف في **نموذج جديد** عبر النمط المعتمد `clsGlobal._OpenForm` (نموذج فرعي MDI غير نمطي، مع إعادة تنشيط نافذة مفتوحة بدل تكرارها) **ولا يُغلق التقرير الحالي** — والتقارير شاشات قراءة فيُسمح بالفتح غير النمطي، و`RULE-N09` تخص شاشات الإدخال حصراً |
| RULE-N78a | شريط تنقل التقارير (`pnlReportNav` · `lblNavCaption` · `flpReportNav` · `btnNavAllReports`) **يُعلَن في `*.Designer.cs`** لكل `frm*Report` بقيم `GreenThemeHelper` الحرفية، ويتبنّاه `ReportNavigationHelper` بالاسم ويضيف أزرار التصنيفات فقط وقت التشغيل — فلا يغيب الشريط عن المصمّم (`RULE-N146` / `RULE-N145`). ✅ **مغلقة (2026-11-13): الشريط مُعلَن في مصمّمات التقارير الـ31 كلها (31/31)** — `Controls.Add(pnlReportNav)` هو آخر `Controls.Add` في كل مصمّم، و`ClientSize` كبُر 58 فقط، وصفر `SetChildIndex` وصفر مراجع `Helpers` داخل المصمّمات، والقالب `frmAccountStatementReport` |

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
| RULE-N90 | كل كيان يرث `ActivatableEntity` يجب أن يدعم الدورة كاملة: تعطيل → إظهار غير النشط → إعادة تنشيط — ✅ **مكتمل 2026-11-14 لكل كيانات التنشيط التسعة**: العميل · الصنف · المورد · الفئة · الوحدة · المخزن · الصندوق · المستخدم (+ الموظف في وضع التجربة) — كلها تعرّض `ReactivateAsync` في `I*DesktopService` و`Direct*Service` و`Mock*DesktopService`، وفي شاشتها زر «↺ إعادة تنشيط» مفعّل على السطر المعطّل فقط (`RULE-N92`/`RULE-N122`) |
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
| `KeyboardHelper` | `SalesSystem.Desktop/Helpers/KeyboardHelper.cs` | `WireEnterAsTab` · `WireSelectAllOnFocus` · `WireFocusHighlight` · `WireArrowNavigation` · `WireEnterToClick` · `WireButtonDisabledVisual` · `WireGlobalShortcuts` · `WireModeEnter` · `WireDisabledVisual` · `WireDisabledVisuals` (Enter = الزر الأساسي للوضع — `RULE-N147`) |
| `GreenThemeHelper` | `SalesSystem.Desktop/Helpers/GreenThemeHelper.cs` | `Apply` · `ApplyHeader` · `ApplyGrid` · `TextStrong` · `DisabledBack` · `DisabledFore` (`RULE-N154`) |
| `LiveTafqeetHelper` | `SalesSystem.Desktop/Helpers/LiveTafqeetHelper.cs` | `Attach` (تلميح التفقيط الحيّ) |
| `ThemeHelper` | `SalesSystem.Desktop/Helpers/ThemeHelper.cs` | `ApplyMainFormStyle` · `ApplyButtonStyle` (يُستدعى **قبل** الثيم الأخضر) |
| `RowFilterHelper` | `SalesSystem.Desktop/Helpers/RowFilterHelper.cs` | `Escape` · `Like` (تهريب قيمة التصفية وبناء `RowFilter` آمن — `RULE-N24`/`RULE-N60`) |
| `ReportNavigationHelper` | `SalesSystem.Desktop/Helpers/ReportNavigationHelper.cs` | `Attach` (شريط تنقل التقارير داخل النموذج — `RULE-N78`) |
| `ReportUiStandardizer` | `SalesSystem.Desktop/Helpers/ReportUiStandardizer.cs` | `Apply` · `ApplyForm` (توحيد مظهر شاشات التقارير — `RULE-N121`) |
| `ExportHelper` | `SalesSystem.Desktop/Helpers/ExportHelper.cs` | `ExportToExcel` · `ExportToCsv` (التصدير الموحّد للشبكات) |
| `ArabicNumberToWords` | `SalesSystem.Desktop/Helpers/ArabicNumberToWords.cs` | `ToWords` (التفقيط: مبلغ + عملة + كسر) |
| `FilterPanelHelper` | `SalesSystem.Desktop/Helpers/FilterPanelHelper.cs` | `Attach` (يتبنّى زر `btnToggleFilters` المُعلَن في المصمّم ويُوصِّل طيّ التصفية + `F9` + حفظ الحالة — `RULE-N63`/`RULE-N145`) |

**بيوت الكود المشترك الأخرى (لا تُنسخ محتوياتها — `RULE-N169`…`N172`، التفصيل في §2.30):**

| البيت | الاستهلاك |
|---|---|
| `SalesSystem.Desktop/Global Classes/` | `clsGlobal._OpenForm(form)` لفتح أي نموذج من داخل نموذج (بدل `ShowDialog`/`Show` المخصّص — `RULE-N132`) · `clsFormat.Money/Quantity/Number/Percent/DateToShort/DateToLong/TryParseMoney/TryParseQuantity` لكل تنسيق رقمي/تاريخي · `clsDialogs.Info/Success/Warning/Error/Error(ex,msg)/Confirm` لكل رسالة `MessageBox` (العناوين العربية الموحّدة) · `clsValidation.ValidateEmail/IsRequired/IsDigits/IsPhone/IsPositiveDecimal/TryParseDecimal` · `clsInputFeedback.ShowRejectTooltip/ShowInfoTooltip/ShowSuccessTooltip` |
| `SalesSystem.Common/Helpers/` | `Guard` · `StringExtensions` · `DateTimeExtensions` · `FileHelper` — أدوات محايدة لأي طبقة |

**سطر الإلزام في كل مُنشئ شاشة:**

```csharp
KeyboardHelper.WireEnterAsTab(this);
KeyboardHelper.WireSelectAllOnFocus(this);
KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)); // RULE-N46 + RULE-N54b
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
| 4 | وحدات **العملات · الضرائب · البنوك · شاشة الإعدادات** (لا كيان ولا خدمة ولا شاشة) | **مؤجل بقرار المالك (2026-11-12)** — غير مطلوب في MVP | ضريبة القيمة المضافة تُدخل يدوياً في الفاتورة ويقرأها `frmVatReport`؛ الإعدادات تُقرأ من `SystemSettings` بلا شاشة (تُدار بذراع البذور/الخدمة) | 🔒 V2 |
| 5 | **إعدادات التنسيق** (منازل المبالغ/الكميات/الأعداد/النسب · فاصل الآلاف وفاصل الكسر · الأرقام العربية · صيغ التواريخ) | **قرار المالك (2026-11-14): مطلوبة الآن — شاشة `frmFormatingSetting`، المرحلة أ (Mock) فقط؛ لا BLL/DB قبل موافقته المكتوبة (`RULE-N166`)**. ولا يفتح هذا البند شاشة إعدادات عامة (يبقى بند 4 مؤجلاً) | `clsFormat` هو البيت الواحد للتعريفات والقيم الحيّة والتحقق والمنسّقات؛ القيم في الذاكرة فقط في المرحلة أ | 🔄 المرحلة أ |

> **ملاحظة (2026-11-12):** غياب `Currencies`/`Taxes`/`Banks`/`frmSettings` **ليس نقصاً في MVP** — قرار مالك مسجّل في السجل أعلاه (بند 4). أي مراجعة تُبلغ عنه تُغلق بإحالة إلى هنا (`RULE-N107`).
> **ملاحظة (2026-11-14):** `frmFormatingSetting` **ليست** `frmSettings` العامة — هي شاشة تنسيق الأرقام والتواريخ فقط (بند 5)، وحفظها الحقيقي مؤجل للمرحلة ب.

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
| RULE-N169 | **قانون المكان الواحد (Single Home) — قرار المالك 2026-11-14:** كل دالة/سلوك/قيمة تُستخدم في أكثر من موضع **تعيش في مكان واحد مُعلَن مرة واحدة** ويستهلكها كل من يحتاجها؛ ويُحرَّم إنشاء نسخة ثانية منها في أي ملف آخر. صياغة المالك: «كود نظيف قابل لإعادة الاستخدام وسهل الصيانة — يُكتب في مكان واحد ويُستهلك في النظام كله، فتُعدّله مرة واحدة بسرعة وبأقل وقت وأقل أخطاء». بيوت الكود المشترك في الجدول أدناه هي **الوجهة الوحيدة** لأي كود مشترك جديد (`RULE-N112` تبقى سارية) |
| RULE-N170 | **ابحث قبل أن تكتب:** قبل كتابة أي دالة مساعدة يبحث الوكيل بالترتيب في `Desktop/Helpers/` ← `Desktop/Global Classes/` ← `Common/Helpers/` ← `Business/Helpers/` (وبـ`grep` على اسم السلوك)؛ فإن وُجدت **يستهلكها أو يوسّعها في بيتها**، وإن لم تُوجد يكتبها في بيتها الصحيح **مرة واحدة** ثم يستهلكها. ممنوع كتابة النسخة الأولى داخل شاشة بنيّة «نقلها لاحقاً» |
| RULE-N171 | **معيار التعديل الواحد (One Edit Rule):** أي تعديل على سلوك مشترك يجب أن يتم في **ملف واحد** ويسري تلقائياً على كل من يستهلكه؛ وإذا استلزم التعديل لمس أكثر من ملف شاشة ⇒ فالسّلوك ليس في مكان واحد ⇒ **يُنقل إلى بيته الواحد أولاً** ثم يُنفَّذ التعديل — وممنوع «إصلاح» نفس السلوك في عدة شاشات |
| RULE-N172 | **سقف التكرار:** نسختان لسلوك مالي/تحقّقي، أو ثلاث نسخ لأي سلوك آخر = مخالفة تُستخرج إلى بيتها الواحد في نفس الموجة، وتُرفض الشاشة/المهمة إن بقيت مكرّرة (`RULE-N55`). مراجعة الكود (بوابة `§2.37`) تفحص التكرار قبل القبول (RULE-N161) |

**بيوت الكود المشترك (المكان الواحد — لا تُنسخ محتوياتها):**

| البيت | ماذا يعيش فيه | نماذج قائمة |
|---|---|---|
| `SalesSystem.Desktop/Helpers/` | سلوك واجهة/عرض مشترك: ثيم · تنقل · تصفية · تفقيط · تصدير · توحيد التقارير | `GreenThemeHelper` · `KeyboardHelper` · `ThemePalette` · `LiveTafqeetHelper` · `RowFilterHelper` · `FilterPanelHelper` · `ExportHelper` · `ReportUiStandardizer` · `ReportNavigationHelper` · `ColorSuggestionHelper` |
| `SalesSystem.Desktop/Global Classes/` | أدوات عامة صغيرة بصيغة `cls*` (namespace `SalesSystem.Desktop.GlobalClasses`) — **نمط المالك المرجعي**: `clsGlobal` (فتح النماذج) · `clsFormat` (كل تنسيق رقمي/تاريخي **+ تعريفات إعدادات التنسيق وقيمها الحيّة والتحقق منها**: `Definitions` · `Get/Snapshot/TrySet/TryApplyAll` · `Validate/ValidateAll` · `ResetToDefaults`) · `clsDialogs` (كل رسالة `MessageBox` بعناوين عربية ثابتة) · `clsValidation` (التحقق) · `clsInputFeedback` (تغذية الرفض/النجاح البصرية) — **لا تُنسخ أعضاؤها إلى أي شاشة** |
| `SalesSystem.Common/Helpers/` | أدوات محايدة لا تعرف الواجهة ولا قاعدة البيانات، مشتركة بين كل الطبقات | `Guard` · `StringExtensions` · `DateTimeExtensions` · `FileHelper` |
| `SalesSystem.Business/Helpers/` | حسابات العمل المشتركة بين الخدمات | حسابات المجاميع · التقريب · الضريبة |
| `SalesSystem.DTO/Common/` | عقود النتائج | `Result` · `Result<T>` |

**مصادر الحقيقة للـ Helpers (لا تُكرَّر قيمها):** `GreenThemeHelper` (كل ألوان النظام) · `KeyboardHelper` (التنقل/التحديد/التظليل/الأسهم/الأزرار/الاختصارات) · `LiveTafqeetHelper` (التفقيط الحيّ) · `RowFilterHelper` (تهريب قيم التصفية) · `ThemeHelper` (تنسيق النموذج/الزر الأساسي، يُستدعى قبل الثيم الأخضر) · `ReportUiStandardizer` (توحيد شاشات التقارير) · `clsGlobal`/`clsFormat`/`clsDialogs`/`clsValidation`/`clsInputFeedback` (أدوات `Global Classes/` العامة) · `Common/Helpers/` (`Guard`/`StringExtensions`/`DateTimeExtensions`/`FileHelper`).

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

---

### 2.33 سجل الأخطاء الشائعة في WinForms (WinForms Pitfall Register) — إلزامي

هذه أخطاء وقعت فعلاً في هذا المستودع وكلِّفت دورات مراجعة، ويجب فحصها قبل أي عمل تخطيط/ثيم — كل صف أدناه قاعدة مُلزمة لا نصيحة.

| RULE | الخطأ الشائع | القاعدة الملزمة | كيف يُكتشف |
|------|-------------|----------------|------------|
| RULE-N125 | **ترتيب الإرساء (Dock) بعد إضافة عنصر مراسٍ** — إعادة دفع أبٍ `Dock=Fill` إلى آخر مجموعة `Controls` بعد إضافة شريط `Dock=Bottom` تقلب ترتيب الإرساء فيغطي الشريط الصف السفلي (أخفى `btnClose` في 31 شاشة تقرير) | WinForms يُرسي الأبناء **من آخر المجموعة إلى أولها**؛ لا تُعِد ترتيب `Controls` لعنصر `Dock != None` — اترك الترتيب الطبيعي `[العنصر الأمامي(0) … العنصر المراسي المضاف(آخر)]` ويُرسى الشريط أولاً ثم يتقلّص الأب الممتد فوقه | افتح الشاشة وتحقق أن عناصر الصف السفلي (`pnlFooter`/`btnClose`) ظاهرة فوق أي شريط مثبّت |
| RULE-N126 | **`Anchor` يُحسب نسبةً إلى الأب** — نقل عنصر إلى حاوية جديدة مع إبقاء `Anchor` القديم يُعيد تموضعه عشوائياً | كل Reparent يبدأ بـ`control.Anchor = AnchorStyles.Top \| AnchorStyles.Left` (أو `Dock = Fill` للشبكات) قبل `destination.Controls.Add(control)` | ابحث عن `Controls.Add(` على عنصر منقول وتأكد أن `Anchor` أُلغي |
| RULE-N127 | **ترتيب التدفّق في `FlowLayoutPanel` بـRTL** — أول عنصر يُضاف يُرسم في أقصى اليمين؛ وفرز عناصر مطلقة الموضع بـ`Top` فقط يضع العنوان على يسار حقله بدل يمينه (فوارق 4–9 بكسل بين عنوان المصمّم وحقله) | رتّب عناصر التصفية بترتيب **بصري**: تجميع الصفوف بسماحية رأسية ≥ 12 بكسل ثم `Left` تنازلياً داخل كل صف، وأضف العنوان قبل حقله | افتح تقريراً متعدد الفلاتر وتحقق أن كل عنوان ملتصق بحقله من اليمين |
| RULE-N128 | **مرجع إلى ثابت Helper داخل `InitializeComponent`** — المصمّم لا يستوفي التعبير فيُعيد كتابته لوناً حرفياً (`Color.FromArgb`) عند أول تعديل، فيعود تكرار قيم الثيم | ملفات `*.Designer.cs` لا تُشير إلى أي عضو في `Helpers/` — الألوان تُطبَّق وقت التشغيل من الـHelper (`RULE-N54`/`RULE-N112`) | `Select-String -Path *.Designer.cs -Pattern "GreenThemeHelper\|ThemeHelper"` = 0 |
| RULE-N129 | **إعادة تلوين الطبقات الشفافة** — مشي الثيم يعيد رسم `Panel` شفاف (`BackColor.A < 255`) فيصبح معتماً ويغطي المحتوى (لوحة «جارٍ التحميل») | أي عنصر `BackColor.A < 255` لا يُعاد تلوينه إطلاقاً؛ الطبقات الشفافة تُترك كما هي | افتح شاشة أثناء التحميل وتحقق أن الطبقة شفافة فوق المحتوى |
| RULE-N130 | **عنصر معلن وغير مضاف** — حقل مُعرَّف ومُهيَّأ في المصمّم لكن `Controls.Add(x)` مفقود (مثل `label1` في `frmAccountStatementReport`) فيبدو الكود سليماً ويضيع العنصر | أي عنصر جديد يُتحقَّق بوجود سطر `Controls.Add` له، لا بالاكتفاء بتعريف الحقل؛ والعناصر الميتة تُزال عند أول مساس بالملف | `grep "Controls.Add("` مقابل قائمة الحقول في نفس `InitializeComponent` |
| RULE-N131 | **إغلاق Lambda خاطئ** — إنهاء اشتراك حدث بـ`});` بدل `};` يكسر البناء (`CS1002`/`CS1513`) | `+= (s, e) => { … };` — القوس الزائد ممنوع ما لم تُفتح مجموعة صريحة (`Action(( ) => …)`) | `dotnet build` = 0 أخطاء قبل أي تقرير تسليم |
| RULE-N132 | **فتح تقرير بـ`ShowDialog` ثم إغلاق الشاشة الحالية** — خطأ وقع فعلاً في `ReportNavigationHelper`: الضغط على زر تقرير كان يستبدل الشاشة ويغلقها بعد إغلاق الهدف، بخلاف `frmMain` الذي يفتح التقارير كنماذج فرعية MDI غير نمطية | أي فتح لنموذج من داخل نموذج آخر يمرّ عبر **`clsGlobal._OpenForm`** (نمط البيت الواحد) — لا `ShowDialog` ولا `Show()` مخصّص ولا إغلاق للنموذج الحالي؛ الاستثناء الوحيد: شاشات الإدخال النمطية بند `RULE-N09` | ابحث في الشاشة عن `ShowDialog\|\.Close()` بعد فتح نموذج، وقارن سلوك الزر بسلوك نفس التقرير من `frmMain` |
| RULE-N133 | **التحقق من التكرار بعد إنشاء نسخة جديدة** — فحص `Application.OpenForms` بعد `GetService` يعني أن النسخة المُنشأة حديثاً تُهمَل وتتسرّب | افحص وجود نافذة مفتوحة بالاسم **قبل** إنشاء/حلّ النسخة (`ServiceProvider.GetService`)، ثم فعّلها؛ وإن لم توجد → أنشئ وافتح عبر `_OpenForm` | اقرأ ترتيب السطور: `GetService` يجب أن يكون **بعد** حلقة `Application.OpenForms` |
| RULE-N134 | **مصمّم الشاشة لا يُظهر ألوان وقت التشغيل** — التنسيق كله يُطبَّق وقت التشغيل من `GreenThemeHelper`، فيُفتح النموذج في مصمّم Visual Studio أبيض/رمادياً بينما التطبيق أخضر، فيظن المطوّر أن الشاشة بلا ثيم | كل لون يطبّقه وقت التشغيل على عنصر موجود في المصمّم **يُعلَن أيضاً بقيمة حرفية في `*.Designer.cs` مطابقة تماماً لثابت `GreenThemeHelper`** (نفس أرقام `Color.FromArgb`) — فيتساوى المصمّم والتشغيل؛ وممنوع استدعاء أي عضو من `Helpers/` داخل المصمّم (`RULE-N128` يبقى سارياً)؛ وأي اختلاف بين القيمة الحرفية وثابت الـHelper يُعدّ عيباً يُصحَّح لصالح الثابت؛ والعناصر التي تُبنى وقت التشغيل فقط (`pnlReportRoot` وشل التقارير · طبقة التحميل الشفافة) لا تُمثَّل في المصمّم وهذا مقبول **بشرط ألا تكون عناصر دائمة**: كل زر/شريط/حقل دائم يُعلَن في المصمّم (`RULE-N145`) — وسريانها **على مستوى النظام كله**: كل `frm*` وكل `ctrl*` UserControl في `SalesSystem.Desktop` (شاشات الإدخال والقوائم والتقارير ولوحات التحكم والنوافذ الحوارية) — لا استثناء ولا «مرحلة تجريبية»؛ وأي شاشة تُنشأ أو تُعدَّل تخرج وفي مصمّمها نفس ألوان وقت تشغيلها، وإلا فهي غير مكتملة (RULE-N55) | افتح النموذج في مصمّم VS وقارنه بالتشغيل: شريط العنوان والشبكة والأزرار والتسميات يجب أن تكون بنفس الأخضر؛ و`grep Color.FromArgb` في المصمّم يجب أن يطابق قيم `GreenThemeHelper` |
| RULE-N139 | **الثيم يمحو ألوان المصمّم — الشاشة تصبح رمادية وقت التشغيل** — `GreenThemeHelper.Apply` كان يفرض `BarBackground`/`GroupBackground` على **كل** `Panel`/`GroupBox`، فتُفقد الخلفية الخضراء الموروثة من النموذج ويظهر المحرر والتصفية والشبكة والملخّص بدرجات رمادية باهتة، بينما المصمّم (Ambient) يبدو أخضر أنيقاً | **وقت التشغيل يلتزم حرفياً بألوان المصمّم:** الحاوية التي لم يضبط المصمّم لها خلفية (Ambient) **تُترك** فترث لون الأب؛ والحاوية التي لوّنها المصمّم بلون خاص **تُترك** بلونها؛ والتلوين التلقائي مقصور على الألوان المعلنة من لوحة الثيم (`RULE-N51a`). وأي فرق بين المصمّم والتشغيل في أي سطح = عيب يُصلَح **لصالح المصمّم** لا لصالح الـHelper. ويُمنع «إصلاح» الفرق بتغميق المصمّم — القاعدة: المصمّم هو المرجع البصري | افتح الشاشة وقارنها بالمصمّم على مستوى **كل سطح** لا شريط العنوان فقط: النموذج · لوحات الإدخال · صناديق المجموعة · شريط الأدوات · صندوق التصفية · شريط الملخص · شبكة البيانات (رؤوس/خلايا/تحديد) · الأزرار · التسميات |
| RULE-N145 | **عنصر يُبنى وقت التشغيل فقط لا يظهر في المصمّم («غير واضح» في المصمّم)** — `FilterPanelHelper` كان يُنشئ زر «إخفاء/إظهار التصفية» (`btnToggleFilters`) **وقت التشغيل فقط**، فلا وجود له في `*.Designer.cs`: لوحة التصفية تظهر في المصمّم بلا زرها، وأي تعديل لاحق في المصمّم يُعيد ترتيب/يمحو الزر من التخطيط، ويظن المطوّر أن الشاشة ناقصة أو أن التصفية لا تُطوى | **كل عنصر دائم في الشاشة يُعلَن في `*.Designer.cs`** (زر · شريط · حقل · تسمية) بقيم حرفية مطابقة لِما يُطبَّق وقت التشغيل (`RULE-N134`)، **وممنوع إنشاء عنصر دائم من داخل Helper**. أدوات الـHelpers **تتبنّى** العنصر المعلَن بالاسم (`FindControl`) وتُوصِّل سلوكه فقط (نقرة · اختصار · تلميح · حالة محفوظة)، وإنشاء عنصر وقت التشغيل يبقى **بديلاً احتياطياً** للشاشات التي لم تُرحَّل بعد. وعليه `FilterPanelHelper.Attach` يوصِّل زر `btnToggleFilters` المعلَن في المصمّم ولا يتخطّاه | افتح النموذج في مصمّم VS وعدّ العناصر: كل زر/شريط تراه وقت التشغيل يجب أن تراه في المصمّم بنفس اللون والموضع؛ و`grep "new Button\|new Label\|new Panel\|new CheckBox"` داخل `Helpers/` مقابل وجود العنصر بنفس الاسم في `*.Designer.cs` للشاشة المعنية |
| RULE-N146 | أزرار فتح التقارير لا تظهر في المصمّم — شريط تنقل التقارير (`pnlReportNav` · `lblNavCaption` «تقارير هذه المجموعة:» · `flpReportNav` · `btnNavAllReports` «كل التقارير ▾») يُبنى **وقت التشغيل فقط** في `ReportNavigationHelper`، فيفتح المطوّر `*.Designer.cs` لتقرير فلا يرى شريطاً ولا أزراراً ولا يستطيع ضبط موضعها أو التحقق منها | شريط التنقل يُعلَن في المصمّم: `pnlReportNav` (`Dock = Bottom` · ارتفاع 58 · `BarBackground`) بداخله `lblNavCaption` («تقارير هذه المجموعة:» · `Tahoma 9 Bold` · `TextStrong`) و`flpReportNav` (`Dock = Fill`) و`btnNavAllReports` («كل التقارير ▾» · `GroupBackground` · `UseVisualStyleBackColor = false`)، و`ReportNavigationHelper` **يتبنّى** هذه العناصر بالاسم ويُنشئ **أزرار التصنيفات فقط** وقت التشغيل (`RULE-N145`) — وممنوع أن يظهر شريط التنقل لأول مرة عند التشغيل | افتح أي `frm*Report` في مصمّم VS: يجب أن ترى شريط التنقل وأزراره أسفل الشاشة فوق زر «إغلاق»؛ و`grep "new Panel\|new Button\|new Label" Helpers/ReportNavigationHelper.cs` = 0 لعناصر الشريط الدائمة |

> **قرار المالك (2026-11-13): «ما يظهر في المصمّم هو ما يظهر وقت التشغيل» (`runtime == design time`) — قاعدة قائمة على مستوى النظام كله:** تشمل **كل** شاشات `SalesSystem.Desktop` (`frm*` و`ctrl*`)، وهي **شرط قبول دائم** في §2.21 بند 29، لا تجربة محدودة بشاشة واحدة. الشاشة النموذج المرجعي المنفَّذ: `frmAccountStatementReport` — وبقية الشاشات تُطابق عند أول مساس بها.

**قاعدة التسليم:** لا يُسلَّم عمل تخطيط/ثيم قبل تشغيل فحص البناء (RULE-N131) وفحص واحد على الأقل للبنود N125–N130 في الشاشة المتأثرة، ويُذكر البند المختبَر في تقرير التسليم.

---

### 2.34 معيار ترتيب وتجميع أجزاء الشاشة (Layout Order & Grouping Standard) — إلزامي

ترتيب وتجميع أجزاء الشاشة كما في الشاشة المرجعية `frmAddUpdateSalesInvoice` هو المعيار الملزم لكل شاشة في النظام كله، للحصول على نفس تجربة الاستخدام في كل مكان.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N135 | الترتيب الرأسي الثابت: العنوان ← أوامر الشاشة ← معايير البحث/التصفية ← المحتوى الرئيسي ← الإجماليات/الملخص ← شريط الأسفل — لا يُقدَّم بند على بند ولا تُضاف كتلة بينها. |
| RULE-N136 | كل كتلة في البنود 2–5 داخل `GroupBox` بعنوان عربي قصير: «أوامر التقرير» · «بحث وتصفية» · «بيانات التقرير» · «الإجماليات» (والمكافئات في شاشات الإدخال: «أوامر الفاتورة» · «بيانات …» · «الأصناف» · «الإجماليات») — والعنوان/الألوان من `GreenThemeHelper` (`TextStrong` + `GroupBackground`) وخط `Tahoma 10 Bold`. |
| RULE-N137 | ممنوع: صندوق داخل صندوق، صندوق فارغ، كتلة بلا صندوق (عدا شريط العنوان وشريط الأسفل)، أو ترتيب مختلف بين شاشتين لنفس النوع. |
| RULE-N138 | السريان على مستوى النظام كله: كل `frm*` و`ctrl*` (إدخال · قوائم · تقارير · لوحات تحكم · حوارات)؛ الشاشة الجديدة تخرج ملتزمة، والقائمة تُطبَّق عند أول مساس بها؛ والمخالفة = شاشة غير مكتملة (`RULE-N55`). |

> **الشاشة المرجعية:** frmAddUpdateSalesInvoice (‏gbToolbar · gbInvoiceInfo · gbEntry · gbItems · gbTotals) — والقياس بالتجربة اليدوية: افتح الشاشة وتحقق من الصندوق والعنوان لكل كتلة ومن الترتيب أعلاه.

---

### 2.35 معيار تطابق ألوان وقت التشغيل مع المصمّم (Runtime == Design Colours) — إلزامي

قرار المالك: المظهر المصمّم هو الهدف — تعرض الشاشة وقت التشغيل شكلها المصمّم بالضبط ولا تظهر رمادية أبداً.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N140 | ألوان وقت التشغيل = ألوان المصمّم حرفياً: كل لون مؤلَّف في `*.Designer.cs` (`Form.BackColor` · الصناديق · الألواح · التسميات · الأزرار · حقول الإدخال · الشبكة) يظهر في التشغيل بنفس القيمة؛ وأي اختلاف بينهما عيب يُصحَّح لصالح المصمّم (`RULE-N134` تسري في الاتجاهين). |
| RULE-N141 | ممنوع الظهور الرمادي: لا يُترك أي عنصر على `SystemColors.Control` أو `Control.DefaultBackColor` أو `BackColor` غير معلن، ولا `UseVisualStyleBackColor = true`/`FlatStyle = System` على أزرار وحقول داخل شاشة ثيمها أخضر — الشاشة التي تظهر رمادية في التشغيل شاشة غير مكتملة (`RULE-N55`). |
| RULE-N142 | لا يُعيد أي Helper كتابة لون مؤلَّف بقيمة أخرى: `GreenThemeHelper.Apply`/`Walk`/`ApplyGrid` تطبّق ثوابت الثيم على العناصر **غير المؤلَّفة** فقط، وتحترم القيمة الحرفية الموجودة في المصمّم؛ والقيم حصراً من `GreenThemeHelper` (ممنوع لون جديد) — وأي تعارض يُحسم لصالح المصمّم ثم يُوحَّد في الـHelper. |
| RULE-N143 | حقول الإدخال موحّدة: `ComboBox`/`TextBox`/`NumericUpDown`/`DateTimePicker` بخلفية `Color.White` وخط `Tahoma` في المصمّم وفي التشغيل معاً — كما في الشاشة المرجعية `frmProductMaster` (`cmbCategory`/`cmbMainUnit`). — ✅ محقَّق في التقارير الـ31 (2026-11-13): **47/47** حقل إدخال أبيض + `Tahoma`، بصفر استثناء (فحص `grep` على `*.Designer.cs`) |
| RULE-N144 | الشاشة المرجعية للألوان هي `frmProductMaster` وملف مصمّمها: أي شاشة تُنشأ أو تُعدَّل تُطابق ألوانها ألوان هذه الشاشة حرفياً (الشريط الأخضر · الألواح · الصناديق · الأزرار السبعة · الحقول البيضاء · الشبكة الخضراء)، والمخالفة تُرفض في المراجعة. |

> **التحقق:** افتح الشاشة في مصمّم VS وصوّرها، ثم شغّلها وصوّرها — يجب أن تتطابق الصورتان لوناً وتخطيطاً؛ وأي فرق (خصوصاً الرمادي) عيب، لا اختيار تصميمي.

---

### 2.36 معيار وضع الإدخال (Entry Mode Standard) — إلزامي لشاشات `frm*Master`

شاشة الدليل المدمجة تفتح في **وضع عرض** لا في وضع إدخال: الحقول معطّلة حتى يطلب المستخدم الإضافة أو يختار سجلاً. الشاشة المرجعية: `frmProductMaster`.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N149 | الشاشة تبدأ في **وضع العرض** (`Browsing`): كل حقول الإدخال **معطّلة**، والمفعّل فقط «جديد/تحديث/خروج»، و`lblMode` = «وضع العرض» — لا حقول مفتوحة بلا سبب عند فتح الشاشة |
| RULE-N150 | آلة حالة **واحدة** `EntryMode { Browsing, Adding, Editing }` + دالة مركزية `_ApplyModeState()` تُستدعى بعد **كل** انتقال (تحميل · جديد · اختيار سطر · حفظ · حذف · إعادة تنشيط · تحديث · Esc) — وممنوع تعيين `btn*.Enabled` أو `input.Enabled` متفرقاً داخل المعالجات |
| RULE-N151 | «جديد»/`F2` ⇒ `Adding` (تفريغ + تفعيل + «إضافة … جديد») · اختيار سطر ⇒ `Editing` (تعبئة + تفعيل + «تعديل …: <الاسم>») · وبعد الحفظ/الحذف/إعادة التنشيط **تحديث تلقائي** للشبكة والمحرر والشريط مع إبقاء السطر المحدد (بلا إعادة فتح الشاشة) |
| RULE-N152 | الأزرار المعطّلة تُرسم بـ `WireButtonDisabledVisual` (رمادية بنص باهت) وليس `Enabled = false` فقط، و`Enter`/`AcceptButton` يتبعان الوضع (`RULE-N147`)، ومظهر التعطيل موحّد كالأزرار المعطّلة (`RULE-N154`) |

> **قاعدة:** أي `frm*Master` لا تلتزم بـ§2.36 تُعدّ غير مكتملة (`RULE-N55`)، وتُنفَّذ شاشة بشاشة — وكيل واحد لكل شاشة. ✅ **مكتمل 10/10 ماسترات (2026-11-14)**.

---

### 2.37 Agent Orchestration & Task Partitioning — mandatory

The **Orchestrator** is the only agent that partitions work; an agent never delegates to another agent. This section is the **partitioning law**. The operative playbook — brief templates per agent, quality gates, the two-stage mock-first gate, the scenario map, the commit protocol, the anti-patterns — lives in `docs/prompts/ORCHESTRATOR.md` and is written in English for best agent comprehension.

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N155 | **The unit of delegation is a slice — never a file, never "all screens":** every task is sized **S/M/L** (table below) *before* delegation, and the **number of agents is decided by the size**, not by convenience. A delivery without a written size classification is incomplete |
| RULE-N156 | **Heavy work ⇒ one agent per screen (no exception):** rebuilding a screen · an `EntryMode` state machine · converting to a Master screen · a UX wave — each screen is its own slice owned by one agent that reads its designer + code-behind in full. Never put two heavy screens in one agent even if they look similar, and never split one screen across two agents (designer + code-behind are a single contract) |
| RULE-N157 | **Small-fix batches (S-Batches):** an identical touch on many screens ⇒ **one agent per batch**, max **8 files (up to 4 screens × their two files)**; the batch must be **literally identical in pattern** and its files listed in a **closed list**. Any file that needs judgement (behaviour · money · grid · contract) leaves the batch and gets its own agent. "Inspect the screens and fix them" is forbidden |
| RULE-N158 | **One owner per file:** a file belongs to exactly one agent per wave; parallelism is allowed only across **fully disjoint** file sets. Shared files (`Program.cs` · `appsettings.json` · any `.csproj` · `SalesSystem.slnx` · `AGENTS.md` · `docs/**`) have **one named owner** in the brief; everyone else reports the need and does not edit (`RULE-N114`) |
| RULE-N159 | **Waves and builds:** max **4 agents per wave**; agents **never build and never run** — the Orchestrator runs `dotnet build` after every wave. Inside **Stage B** a slice is built in **DB → BLL → Desktop** order (never build a layer before the contract of the layer beneath it); **Stage A is UI-first** by `RULE-N165` |
| RULE-N160 | **Mandatory brief template (all fields, no exception):** Goal (one sentence) · **Stage** (A = mock/UI & UX QA · B = real — and a Stage B brief quotes the owner's approval with its date) · In scope (explicit paths) · **Out of scope** (what must not be touched, by name) · Contract (names/signatures) · Definition of done (verifiable output) · **Mock parity** (Stage A: the refusals the mock must enforce) · Validation ("do not build — the Orchestrator builds · no tests") · Report back + an **Arabic manual-test checklist** at the end |
| RULE-N161 | **Failure and escalation:** an agent failing twice on the same file ⇒ the Orchestrator stops, splits smaller, or does it itself; an agent touching a file outside its scope ⇒ **its diff is rejected** and the brief is reissued with tighter bounds; a request needing more than 3 new abstractions ⇒ stop and escalate to the owner (`RULE-N113`) |
| RULE-N162 | **Commit gates:** no commit before (1) the **stage gate is honoured** — no Stage B file (BLL · DTO beyond the contract · EF · migration · `Direct*Service`) is committed before the owner's recorded approval (`RULE-N166`) (2) a green build with **0 Errors** (3) **one code review** of the finished slice — not of every file (4) zero out-of-scope files in `git status` (5) `AGENTS.md` updated (§2.20 · §2.37 · §9) when a status changes (`RULE-N120`) (6) an Arabic manual-test checklist in the delivery (`RULE-N119`) (7) a clean tree and `local == origin` after the push |
| RULE-N163 | **The scenario map is binding:** the ready-made partition decision for every recurring scenario (one screen · N screens · a full slice · red build · review · printing · report · docs) lives in `docs/prompts/ORCHESTRATOR.md` §9 — read it before partitioning, and never invent a new split when a matching scenario exists |
| RULE-N164 | **The Orchestrator playbook is an operative reference:** `docs/prompts/ORCHESTRATOR.md` (S/M/L sizing · partition algorithm · file ownership · brief templates · waves · quality gates · scenario map · commit protocol · report shape · anti-patterns) is read first by any agent taking the Orchestrator role, and is updated in the same wave as any change to this section. The role is **registered as the `orchestrator` agent** in `.opencode/agents/orchestrator.md` (`mode: primary` · its prompt embeds this law and points to that file · `edit` is denied everywhere except `AGENTS.md` / `docs/**` / `opencode.json`, and `dotnet test` is denied) |
| RULE-N165 | **Mock-first delivery order — every new screen or feature with a data path is proven in a mock sandbox *before* the production code is touched, and delivered in two stages. Stage A is the owner's UI & UX QA round.** **Stage A (mock · UI & UX QA):** `frm*` + designer + the contract `I<Entity>DesktopService` + `Mock<Entity>DesktopService` + in-memory `MockData` seeds, `UseMockData = true`, build **0 Errors**, then handed to the owner to click through on his machine. Stage A **never** writes `SalesSystem.Business/**`, `SalesSystem.DataAccess/**`, `SalesSystem.Models/**`, `Services/Direct/**`, a migration, or any real save path. **Stage B (real):** entity/config/migration → BLL → `Direct<Entity>Service` → real DB save → DI swap, in that order (`RULE-N159`). The point is to **protect the working production code**: the feature is designed, rejected and reworked in the mock sandbox first, so the real files are edited once, reviewed once and broken never |
| RULE-N166 | **Owner approval gate — Stage B starts only on an explicit written approval.** After reviewing Stage A the owner must say it plainly (e.g. «موافق على الباك-إند» / "approved — start the backend"); silence, a bug-fix request, a review of another slice, or a general "looks fine" is **not** approval. The approval (**date + the owner's exact wording**) is recorded in the delivery report *before* the first Stage B file is written, and any behaviour change to Stage A afterwards requires **re-approval**. An agent whose brief carries no recorded approval must refuse Stage B and report back (`RULE-N161`) |
| RULE-N167 | **Mock parity — the mock must behave like the future backend.** `Mock<Entity>DesktopService` mirrors the exact final contract and enforces the refusals the BLL will enforce (required fields · duplicates · quantity 0 · paid > total · editing a `Posted` invoice · negative stock · missing permissions-equivalent checks) returning Arabic `Result.Failure` messages, and its seeds cover the happy path, the rejection path and the money/stock effect. A stub that always returns success is **rejected in review** — it makes the owner's manual checklist meaningless (`RULE-N116`) |
| RULE-N168 | **Never ship a broken mode.** Stage A registers only `Mock*` and leaves `UseMockData = true`; Stage B adds `Direct*` and keeps **both** branches registered (`RULE-N10`). No commit may leave `UseMockData = false` pointing at a missing or empty real path, and the real-mode branch is verified only in Stage B's checklist |

**Why the stage gate exists:** the owner tests the **feature itself** — UI & UX on his own machine with mock data — **before any production file is touched**. The current working code, schema and database are never at risk while the feature is still being shaped: a rejected Stage A costs a mock edit, not a rollback, a migration or a half-broken build. Result: **fewer edits to real files, one review instead of many, fewer errors, and a safe place to try new features.**

**Size classification and agent count (read before every delegation):**

| Size | Definition | Mandatory split | Repo example |
|---|---|---|---|
| **S — small patch** | ≤ 8 files · **identical** edit · no logic, no money | one batch, one agent (up to 4 screens) | add `WireModeEnter` to a screen · fix a `using` · a button outside `Padding` |
| **M — medium change** | one screen (designer + code-behind) with no contract change · or two files in one layer | one agent per screen (+ one service agent if the contract changes) | new button + handler + designer block + state machine · fix `gbFilter` geometry |
| **L — full screen / slice** | rebuild a screen · or a full service slice (Entity + Config + Migration + DTO + BLL + Direct + Mock + UI) | **one agent per screen**, sequential across layers · **Stage A (mock) first, Stage B only after approval** (`RULE-N165`/`N166`) | a new `frm*Master` · an entity reactivation cycle · a new report · `ctrlDashboard` |

**Forbidden delegation patterns (rejected in review):** two agents on one screen · two agents on `Program.cs` · an agent that builds, runs the app, or writes automated tests · an agent editing `AGENTS.md` or `docs/**` · a brief without an out-of-scope list · a brief without a closed file list · "fix all screens" · fixing a layer defect inside another layer (route it to the file owner) · **writing BLL/EF/migration/`Direct*Service` for a slice whose Stage A has not been approved** (`RULE-N166`) · **a Stage B brief that does not quote the owner's approval verbatim with its date**.

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
❌ كتابة دالة/سلوك مشترك داخل شاشة أو نسخه في ملف ثانٍ بدل بيته الواحد (`Helpers/` · `Global Classes/` · `Common/Helpers/` · `Business/Helpers/`) — ولا «إصلاح» نفس السلوك في عدة شاشات (RULE-N169…RULE-N172)
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
| Settings | 🔒 V2 (قرار المالك — §2.29 بند 4) | — (لا شاشة — §2.29 بند 4) | ✅ `CompanySettingsService` · `StoreSettingsService` | ✅ `SystemSettings` |
| Currencies | 🔒 V2 (قرار المالك — §2.29 بند 4) | — (لا `frmCurrencies`) | ❌ | ❌ |
| Chart of Accounts | ✅ Done | `frmAccounts` | ✅ `AccountService` | ✅ `Accounts` |
| Journal Entries | ✅ Done | `frmJournalEntries` | ✅ `JournalEntryService` + `AccountingIntegrationService` | ✅ `JournalEntries` |
| Customers | ✅ Done | `frmCustomerMaster`, `frmCustomerSelection`, `frmCustomerStatement` | ✅ `CustomerService` | ✅ `Customers` |
| Suppliers | ✅ Done | `frmSupplierMaster`, `frmSupplierSelection` | ✅ `SupplierService` | ✅ `Suppliers` |
| Categories | ✅ Done | `frmCategoryMaster` | ✅ `ProductCategoryService` | ✅ `ProductCategories` |
| Units | ✅ Done | `frmUnitMaster` | ✅ `UnitService` | ✅ `Units` |
| Taxes | 🔒 V2 (قرار المالك — §2.29 بند 4) | — (لا `frmTaxes`) | ❌ | ❌ |
| Warehouses | ✅ Done | `frmWarehouseMaster` | ✅ `WarehouseService` | ✅ `Warehouses` |
| Products | ✅ Done | `frmProductMaster`, `frmProductUnitMaster`, `frmProductSelection`, `frmImportProductsExcel` | ✅ `ProductService` · `ProductUnitService` · `ProductImportService` | ✅ `Products` · `ProductUnits` |
| CashBoxes | ✅ Done | `frmCashBoxMaster`, `frmCashBoxTransactions` | ✅ `CashBoxService` | ✅ `CashBoxes` |
| Banks | 🔒 V2 (قرار المالك — §2.29 بند 4) | — (لا `frmBanks`) | ❌ | ❌ |
| Purchases | ✅ Done | `frmListPurchases`, `frmPurchaseInvoice` | ✅ `PurchaseService` | ✅ `PurchaseInvoices` |
| Sales | ✅ Done | `frmListSales`, `frmAddUpdateSalesInvoice` (كلاهما مسجَّل في DI ويُفتح من قائمة `frmMain` عبر `OpenScreen<T>()` — تحقّق 2026-11-13) | ✅ `SalesService` | ✅ `SalesInvoices` |
| Sales Returns | ✅ Done | `frmSalesReturn`, `frmListReturns` | ✅ `SalesReturnService` | ✅ `SalesReturns` |
| Purchase Returns | ✅ Done | `frmPurchaseReturn` | ✅ `PurchaseReturnService` | ✅ `PurchaseReturns` |
| Receipts (سندات القبض) | ✅ Done | `frmListCustomerReceipts`, `frmAddUpdateCustomerReceipt` | ✅ `CustomerReceiptService` | ✅ `CustomerReceipts` |
| Payments (سندات الصرف) | ✅ Done | `frmListPayments`, `frmListSupplierPayments`, `frmSupplierPaymentDialog` | ✅ `SupplierPaymentService` · `PaymentVoucherService` | ✅ `SupplierPayments` |
| Reports | ✅ Done | **31** نموذج `frm*Report` في `Screens/Reports/Forms/` داخل ستة مجلدات تصنيفية (`Sales/`, `Purchases/`, `Inventory/`, `Accounting/`, `CustomersSuppliers/`, `CashBox/`) | ✅ 5 خدمات تقارير (+ Mocks) | ✅ (تقرأ من الجداول القائمة) — ✅ **محقَّق (2026-11-13): 31/31** هيكل مرجعي + تجميع معنون · شريط تنقل معلَن في المصمّم · `loadingPanel` + `ShowLoading` بـ`finally` · تصدير عبر `ExportHelper` (صفر `ClosedXML` محلي) · **47/47** حقل إدخال أبيض `Tahoma` · صفر `SystemColors` وصفر مراجع `Helpers` في المصمّمات · حارس تصدير قائم على عدد الصفوف 31/31 (لا ملف برؤوس فقط) · كميات `N3` ومبالغ `N2` · `ShowLoading` موحّد 31/31 (يُعيد المقدمة + مؤشر الانتظار + `finally`) · صفر `ex.Message` للمستخدم |
| Dashboard | ✅ Done | `frmDashboard` (غلاف) + `ctrlDashboard` — كلاهما مسجَّل في DI (`services.AddTransient<frmDashboard>()` و`services.AddTransient<...ctrlDashboard>()` حسب `RULE-N67`) ويُفتح من `frmMain` | ✅ `DirectDashboardService`/`MockDashboardDesktopService` | ✅ (إحصاءات فوق الجداول القائمة) |
| Inventory (Transfers/Adjustments) | ✅ Done | **16 نموذجاً في `Screens/Inventory/` — كلها مسجَّلة في DI (16/16)**: `frmInventory`, `frmInventoryList`, `frmInventoryAddProduct`, `frmInventoryBarcode`, `frmInventoryFifo`, `frmListInventoryTransactions`, `frmListInventoryBatches`, `frmListInventoryAdjustments`, `frmAddUpdateInventoryAdjustment`, `frmOpeningStock`, `frmWarehouseTransferNew`, `frmStocktakingNew` + نوافذ الطباعة (`frmStocktakingPrint`, `frmStocktakingPrintAdjustment`, `frmStocktakingPrintCard`, `frmWarehouseTransferPrint`) — **10 تُفتح من قائمة `frmMain`** و6 نوافذ فرعية (محرر التسوية `frmAddUpdateInventoryAdjustment` · منتقي إضافة صنف `frmInventoryAddProduct` · 4 نوافذ طباعة) تُفتح من شاشاتها. تحقّق 2026-11-13: `frmStocktakingNew` · `frmWarehouseTransferNew` · `frmInventoryFifo` · `frmInventoryBarcode` مسجَّلة وتُفتح من القائمة | ✅ `InventoryService` · `InventoryBatchService` · `InventoryAdjustmentService` | ✅ `InventoryTransactions` · `InventoryBatches` · `InventoryAdjustments` |
| Expenses | ✅ Done | `frmListExpenses`, `frmAddUpdateExpense` | ✅ `ExpenseService` | ✅ `Expense` (عبر `ExpenseConfiguration`) |
| POS | ✅ Done | `frmPOS` (1371 سطراً) | ✅ `SalesService` (نفس مسار الفاتورة) | ✅ `SalesInvoices` |
| Accounting Engine | ✅ Done | — (auto) | ✅ `AccountingIntegrationService` | ✅ `JournalEntries` |
| Printing | ✅ Done | نوافذ طباعة داخلية (فاتورة شراء/مخزون) | ✅ `Business/Printing` · `DataAccess/Printing` (QuestPDF) | ✅ |
| System Colors (Theme) | 🔄 Stage A (Mock) — 2026-11-14 | `frmChangeSystemColorsSettings` + `ThemePalette` (**24 لوناً في ست مجموعات** + `ColorSuggestionHelper` للاقتراح الذكي) — مسجَّلة في DI وتُفتح من «الإعدادات ← ألوان النظام» | ❌ لا خدمة BLL — المرحلة ب بانتظار موافقة المالك المكتوبة (`RULE-N166`) | ❌ لا حفظ حقيقي: `MockThemeColorDesktopService` + `ThemePalette` في الذاكرة |
| Formatting Settings | 🔄 Stage A (Mock) — 2026-11-14 | `frmFormatingSetting` (**10 إعدادات في مجموعتين** + معاينة فورية بالمسودة) + `clsFormat` (البيت الواحد: `Definitions`/القيم الحيّة/`Validate`/`ValidateAll`/`TryApplyAll`/المنسّقات) — مسجَّلة في DI وتُفتح من «الإعدادات ← إعدادات التنسيق» | ❌ لا خدمة BLL — المرحلة ب بانتظار موافقة المالك المكتوبة (`RULE-N166`) | ❌ لا حفظ حقيقي: `MockFormatSettingsDesktopService` + قيم `clsFormat` في الذاكرة |

> The former `API` column was removed — there is no HTTP/API layer; every module's Desktop layer goes straight to Business.
>
> **قاعدة:** كل صف في هذا الجدول يُتحقَّق من وجود نموذجه فعلياً في `Screens/<Module>/` قبل ادّعاء «Done» — أسماء لا وجود لها (`frmUsers`, `frmRoles`, `frmSettings`, `frmCurrencies`, `frmTaxes`, `frmBanks`, `frmCategories`, `frmProducts`, `frmWarehouses`, `frmCashBoxes`, `frmSuppliers`, `frmCustomers`, `frmUnits`, `frmSales`, `frmReturns`, `frmReceipts`, `frmPayments`) أُزيلت أو استُبدلت بالاسم الحقيقي (`frm*Master`).
>
> **تحقّق 2026-11-13 (DI + القائمة):** **93 نموذجاً `frm*`** في `Screens/**` — كلها مسجَّلة في حاوية DI داخل `Program.cs` (93/93؛ ثلاثة منها بالاسم المؤهَّل: `frmAccounts` · `frmJournalEntries` · `Products.Forms.frmProductSelection`) — و`ctrlDashboard` مسجَّل أيضاً (`RULE-N67`). والشاشات التي جرى التحقق منها مسجَّلة في DI وتُفتح من `frmMain` عبر `OpenScreen<T>()`: `frmStocktakingNew` · `frmWarehouseTransferNew` · `frmInventoryFifo` · `frmInventoryBarcode` · `frmImportProductsExcel` · `frmCalculator` (الأخيرة ضمن أدوات `frmMain` ولا صف مستقل لها في هذا الجدول).

---

## 10. AI Agent Context Guide

### File Priority (read in this order)

| Priority | File | What it contains |
|----------|------|------------------|
| 1 (MUST) | `AGENTS.md` | Architecture, rules, patterns, enums, forbidden list |
| 1a (MUST before delegating) | `docs/prompts/ORCHESTRATOR.md` | **Agent orchestration law (English):** S/M/L sizing · partition algorithm · one-owner-per-file · brief templates per agent · waves and build gates · quality gates · scenario map · commit protocol · report shape · anti-patterns |
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
