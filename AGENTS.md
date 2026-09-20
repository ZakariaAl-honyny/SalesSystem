# AGENTS.md — Sales Management System (v5.0.0 — N-Layer WinForms Edition)
# READ THIS FILE FIRST — BEFORE WRITING ANY CODE
# Platform: .NET 10 | N-Layer Architecture
# ASP.NET Core 10 API + WinForms Desktop + SQL Server

---

## 1. Project Overview

A local desktop sales management system for small retail shops.

**Solution Structure (8 Projects):**

```text
SalesSystem/
├── SalesSystem.Common/       ← Base entities + Enums (18) + Exceptions
├── SalesSystem.Models/       ← Domain entities/Models (~54 entities)
├── SalesSystem.DTO/          ← DTOs + Requests + Responses + Result<T>
├── SalesSystem.DataAccess/   ← EF Core + DbContext + Repositories + Migrations
├── SalesSystem.Business/     ← Business Logic Services (57 services)
├── SalesSystem.API/          ← ASP.NET Core API + Controllers (59) + Validators
├── SalesSystem.Desktop/      ← WinForms Desktop (frm* forms)
└── SalesSystem.Tests/        ← Tests
```

**Data Flow (NEVER break this chain):**

```text
# CRUD / data screens (API path)
Desktop (WinForms) → (HttpClient) → API Controllers → Business Services → DataAccess (EF Core) → SQL Server

# Authentication + Reports (in-process BLL path)
Desktop (WinForms) → Business Services → DataAccess (EF Core) → SQL Server
```

The Desktop never accesses the database directly (no `DbContext`/EF/SQL in the Desktop project); it always goes through BLL services, either in-process or via the API.

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
| RULE-007 | CRUD/data screens call the API via `HttpClient`; auth + reports call BLL in-process | Desktop NEVER touches `DbContext`/EF/SQL directly |
| RULE-022 | Controllers delegate to Services | Controllers NEVER have business logic |
| RULE-024 | Use `IUnitOfWork` for multi-table ops | NEVER use raw DbContext in Services |

### 2.7 Stock Integrity

`WarehouseStocks.Quantity` has `CHECK (Quantity >= 0)`. Record EVERY stock change in `InventoryTransactions`.

### 2.8 Desktop WinForms Patterns (N-Layer)

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N01 | ALL WinForms use `frm*` naming convention — NEVER `*View` or `*Window` |
| RULE-N02 | Forms are resolved via DI (`Program.ServiceProvider.GetRequiredService<T>()`) — NEVER `new frmXxx()` |
| RULE-N03 | Forms receive `IServiceProvider` via constructor injection and resolve services with `_serviceProvider.GetRequiredService<T>()` |
| RULE-N04 | Resolved services are exposed as typed properties (e.g., `private ICustomerApiService ApiService => _serviceProvider.GetRequiredService<ICustomerApiService>();`) |
| RULE-N05 | ALL HTTP API services are in `Services/Http/` with interfaces in `Services/Interfaces/` |
| RULE-N06 | API Services use `HttpClientService` via DI (`IHttpClientFactory` + `AuthTokenHandler`) |
| RULE-N07 | Global exception handlers in `Program.cs`: `Application.ThreadException` + `AppDomain.CurrentDomain.UnhandledException` |
| RULE-N08 | `MessageBox.Show` is the standard user feedback mechanism — use `MessageBoxIcon.Error`/`Information`/`Warning` appropriately |
| RULE-N09 | Forms are shown modally via `ShowDialog()` — NEVER non-modal for data-entry screens |
| RULE-N10 | Every API service interface + implementation pair registered in `Program.cs` DI container |

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
// Http Services
services.AddTransient<AuthTokenHandler>();
services.AddHttpClient<HttpClientService>(client =>
{
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
}).AddHttpMessageHandler<AuthTokenHandler>();

// API Services (CRUD / data screens)
services.AddTransient<IAuthApiService, AuthApiService>();
services.AddTransient<IUserApiService, UserApiService>();
// ... all API services

// Mock / Real service toggle (switch UseMockData in appsettings.json)
if (appSettings.UseMockData)
{
    services.AddTransient<ISalesInvoiceApiService, MockSalesInvoiceApiService>();
    services.AddTransient<ICustomerApiService, MockCustomerApiService>();
    services.AddTransient<IProductApiService, MockProductApiService>();
    services.AddTransient<IWarehouseApiService, MockWarehouseApiService>();
    services.AddTransient<ISettingsApiService, MockSettingsApiService>();
    services.AddScoped<BizInterfaces.IAuthService, MockAuthService>();
}
else
{
    services.AddTransient<ISalesInvoiceApiService, SalesInvoiceApiService>();
    services.AddTransient<ICustomerApiService, CustomerApiService>();
    services.AddTransient<IProductApiService, ProductApiService>();
    services.AddTransient<IWarehouseApiService, WarehouseApiService>();
    services.AddTransient<ISettingsApiService, SettingsApiService>();
    services.AddScoped<BizInterfaces.IAuthService, BizServices.AuthService>();
}

// DataAccess + Business Logic (in-process BLL path — auth + reports)
services.AddDbContext<SalesDbContext>(options =>
    options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<BizInterfaces.IAccountService, BizServices.AccountService>();
// ... all BLL services

// Forms
services.AddTransient<frmLogin>();
services.AddTransient<frmMain>();
// ... all forms
```

### 2.9 API Patterns

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N11 | ALL controllers use `[Authorize]` attribute (except AuthController) |
| RULE-N12 | Route prefix: `api/v1/[controller]` |
| RULE-N13 | JWT auth with role-based policies: `AdminOnly` (role 1), `ManagerAndAbove` (roles 1,2), `AllStaff` (roles 1,2,3) |
| RULE-N14 | FluentValidation on ALL Create/Update request DTOs |
| RULE-N15 | Error responses return `Result.Failure` with Arabic message + ErrorCode |
| RULE-N16 | `ExceptionMiddleware` catches all unhandled exceptions and returns structured JSON |

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
| `FluentValidation.AspNetCore` 11.x | API |
| `Serilog.AspNetCore` 8.x | API |
| `Serilog.Sinks.File` 5.x | API + Desktop |
| `Microsoft.AspNetCore.Authentication.JwtBearer` 10.x | API |
| `Swashbuckle.AspNetCore` 6.x | API |
| `Microsoft.Extensions.Http` 10.x | Desktop |
| `System.Text.Json` 10.x | Desktop |
| `ClosedXML` 0.102.x | Desktop (Excel export) |
| `QuestPDF` 2024.3.x | DataAccess (A4 PDF) |
| `SixLabors.ImageSharp` 3.1.x | DataAccess |
| `System.Drawing.Common` 10.x | DataAccess |
| `Microsoft.Extensions.Hosting` 10.x | Desktop |
| `Microsoft.Extensions.Hosting.WindowsServices` 10.x | API |
| `Microsoft.AspNetCore.DataProtection` 10.x | DataAccess |
| `Serilog.Sinks.EventLog` 8.x | API |

### 2.12 WinForms UI Conventions

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N20 | ALL forms use `RightToLeft = Yes` and `RightToLeftLayout = true` for Arabic UI |
| RULE-N21 | ALL forms use `Tahoma` font family |
| RULE-N22 | DataGridView with `AllowUserToAddRows = false`, `ReadOnly = true`, `SelectionMode = FullRowSelect` |
| RULE-N23 | ContextMenuStrip on DataGridView for row operations |
| RULE-N24 | Filter pattern: `ComboBox` (cbFilterBy) + `TextBox` (txtFilterValue) with `DataView.RowFilter` |
| RULE-N25 | Loading data: `async void` handlers with try/catch and `MessageBox.Show` on error |
| RULE-N26 | Cursor feedback: `Cursor = Cursors.WaitCursor` before async operations, restore in finally |
| RULE-N27 | Form pattern: Header label (lblMode, Dock=Top, colored) + Filter bar + DataGridView + Footer buttons |
| RULE-N28 | All user-facing messages in Arabic |
| RULE-N29 | Error messages log via Serilog, show `MessageBox.Show` to user |
| RULE-N30 | Forms handle dispose pattern: `components?.Dispose()` in `Dispose(bool)` |

### 2.13 Users & Permissions (N-Layer)

| RULE | DIRECTIVE |
|------|-----------|
| RULE-N31 | `User` entity uses `PermissionsMask` (bigint) for bitwise permission flags — `-1` = Super Admin |
| RULE-N32 | Roles are templates stored in `Role` entity — assigning a role copies `PermissionsMask` to the user |
| RULE-N33 | 9 seeded roles: Admin(1), Manager(2), Accountant(3), Treasurer(4), Cashier(5), WarehouseSupervisor(6), SalesEmployee(7), Observer(8), BranchManager(9) |
| RULE-N34 | `UserRole` enum in `SalesSystem.Common.Enums` with all 9 roles |
| RULE-N35 | `User.Create()` is passwordless — `MustChangePassword = true` by default |
| RULE-N36 | Password hashing via BCrypt (work factor 12) in service layer |
| RULE-N37 | Lockout after 5 failed login attempts (`LoginAttempts >= 5 → IsLocked = true`) |

### 2.14 API Service Interface Pattern

```csharp
// Interface (Services/Interfaces/)
public interface IUserApiService
{
    Task<Result<List<UserDto>>> GetAllAsync();
    Task<Result<UserDto>> GetByIdAsync(int id);
    Task<Result<UserDto>> CreateAsync(CreateUserRequest request);
    Task<Result<UserDto>> UpdateAsync(int id, UpdateUserRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> ResetPasswordAsync(int id, ResetPasswordRequest request);
}

// Implementation (Services/Http/)
public class UserApiService : IUserApiService
{
    private readonly HttpClientService _http;
    public UserApiService(HttpClientService http) => _http = http;
    
    public async Task<Result<List<UserDto>>> GetAllAsync()
        => await _http.GetAsync<List<UserDto>>("api/v1/users");
    // ...
}
```

### 2.15 Controller Pattern

```csharp
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService) => _userService = userService;
    
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _userService.GetAllAsync(ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }
}
```

### 2.16 Mock Data / UI-First Testing

The Desktop app supports a mock-data mode so the UI can be built and tested before (or without) a running API and database.

| Aspect | Detail |
|--------|--------|
| Flag | `AppSettings.UseMockData` in `SalesSystem.Desktop/appsettings.json` (default `true`) |
| Mock implementations | `SalesSystem.Desktop/Services/Mock/` — `MockAuthService` plus mock implementations of the API interfaces (`MockCustomerApiService`, `MockProductApiService`, `MockWarehouseApiService`, `MockSalesInvoiceApiService`, `MockSettingsApiService`) and shared `MockData` |
| Switching | Set `UseMockData` to `true` (mock) or `false` (real API/DB) in `appsettings.json`; `Program.cs` registers the matching service set at startup |
| Indicator | `frmMain` appends `وضع التجربة (Mock)` to the title bar when mock mode is active |
| Login | In mock mode, login accepts any credentials (no API/database required) |

**Workflow rule:** Test UI/UX with mock data **before** wiring the real backend (UI-first). Mock mode requires no API or database running.

---

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
❌ Business logic in Controllers
❌ Throwing exceptions from Services (use Result<T>)
❌ Hard-deleting invoices or users
❌ Editing a Posted invoice
❌ Cascade delete on any FK
❌ new frmXxx() — always resolve from DI
❌ Resolving services outside DI (no `new frmXxx()`, no manual `new` of services)
❌ Async void handlers without try/catch
❌ English user-facing messages (must be Arabic)
❌ Showing exception details directly to users
❌ Missing authorization on API controllers
```

---

## 5. Solution Build

```powershell
# Build entire solution (from SalesSystem directory)
dotnet build .\SalesSystem.slnx

# Build individual projects
dotnet build .\SalesSystem.Desktop\SalesSystem.Desktop.csproj
dotnet build .\SalesSystem.API\SalesSystem.API.csproj
```

---

## 6. API Endpoint Naming Convention

All controllers under `SalesSystem.API/Controllers/` use:
- Route: `api/v1/[controller]` (kebab-case for multi-word: `cash-boxes`, `sales-invoices`)
- HTTP methods: GET (list), GET/{id} (single), POST (create), PUT/{id} (update), DELETE/{id} (soft delete)
- Authorization: `[Authorize]` with policy where needed
- Response: `Result<T>` model

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
│   ├── Interfaces/                   ← IProductService, ISalesInvoiceService, etc.
│   ├── Services/                     ← Service implementations
│   ├── Accounting/                   ← Journal entry automation
│   ├── Helpers/                      ← Business calculation helpers
│   └── Printing/                     ← Print business logic
│
├── SalesSystem.API/                  ← ASP.NET Core 10 Web API
│   ├── Controllers/                  ← 59 API controllers
│   ├── Validators/                   ← FluentValidation validators
│   ├── Middleware/                   ← ExceptionMiddleware
│   └── Program.cs                    ← DI setup, JWT auth, Swagger
│
├── SalesSystem.Desktop/              ← WinForms Desktop Client
│   ├── Program.cs                    ← DI container, entry point
│   ├── Screens/                      ← 24 module folders (Sales, Products, Customers, etc.)
│   │   ├── Sales/                    ← frm* forms for sales
│   │   ├── Products/                 ← frm* forms for products
│   │   ├── Customers/                ← frm* forms for customers
│   │   └── ... (24 modules)
│   ├── Services/                     ← HTTP API services + HttpClientService
│   │   ├── Http/                     ← API service implementations
│   │   ├── Interfaces/               ← API service interfaces
│   │   └── Mock/                     ← Mock API services + MockAuthService + MockData (UI-first testing)
│   ├── Controls/                     ← Custom WinForms controls
│   ├── Helpers/                      ← Desktop utility classes
│   ├── Models/                       ← Desktop-specific view models
│   ├── Printing/                     ← Desktop print handling
│   └── Configuration/                ← App configuration (ApiSettings.cs, AppSettings.cs)
│
└── SalesSystem.Tests/                ← Unit + Integration tests
```

---

## 9. Current Module Status

| Module | Status | Desktop Screens | API | Business | DB |
|--------|--------|-----------------|-----|----------|----|
| Users & Permissions | ✅ Done | frmUsers, frmRoles | ✅ | ✅ | ✅ |
| Settings | ✅ Done | frmSettings | ✅ | ✅ | ✅ |
| Currencies | ✅ Done | frmCurrencies | ✅ | ✅ | ✅ |
| Chart of Accounts | ✅ Done | frmAccounts | ✅ | ✅ | ✅ |
| Customers | ✅ Done | frmCustomers | ✅ | ✅ | ✅ |
| Suppliers | ✅ Done | frmSuppliers | ✅ | ✅ | ✅ |
| Categories | ✅ Done | frmCategories | ✅ | ✅ | ✅ |
| Units | ✅ Done | frmUnits | ✅ | ✅ | ✅ |
| Taxes | ✅ Done | frmTaxes | ✅ | ✅ | ✅ |
| Warehouses | ✅ Done | frmWarehouses | ✅ | ✅ | ✅ |
| Products | 🔄 In Progress | frmProducts | ✅ | ✅ | ✅ |
| CashBoxes | ✅ Done | frmCashBoxes | ✅ | ✅ | ✅ |
| Banks | ✅ Done | frmBanks | ✅ | ✅ | ✅ |
| Purchases | 🔄 In Progress | frmPurchases | ✅ | ✅ | ✅ |
| Sales | 🔄 In Progress | frmSales | ✅ | ✅ | ✅ |
| Sales Returns | 🔄 In Progress | frmReturns | 🔄 | 🔄 | ✅ |
| Receipts & Payments | 🔄 In Progress | frmReceipts, frmPayments | 🔄 | 🔄 | ✅ |
| Journal Entries | 🔄 In Progress | frmJournalEntries | 🔄 | 🔄 | ✅ |
| Reports | ❌ Not Started | — | — | — | — |
| Dashboard | ❌ Not Started | frmDashboard (shell) | — | — | — |
| Inventory (Transfers/Adjustments) | 🔄 In Progress | frmInventory | 🔄 | 🔄 | ✅ |
| Expenses | ✅ Done | frmExpenses | ✅ | ✅ | ✅ |
| Accounting Engine | ✅ Done | — (auto) | ✅ | ✅ | ✅ |
| Printing | 🔄 In Progress | — | ✅ | ✅ | ✅ |

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
# CRUD / data screens (API path)
Desktop (WinForms)  →  HttpClient  →  API Controllers  →  Business Services  →  DataAccess (EF Core)  →  SQL Server

# Authentication + Reports (in-process BLL path)
Desktop (WinForms)  →  Business Services  →  DataAccess (EF Core)  →  SQL Server

     frm* forms          HTTP            [Authorize]        Result<T>              IUnitOfWork
     GetRequiredService  JSON             Thin              All logic here         Fluent API
     DI resolved         REST             Validate           No exceptions          Repositories
```
