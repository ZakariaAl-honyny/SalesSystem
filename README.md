<h1 align="center">🧾 Sales Management System</h1>

<p align="center">
  <strong>Local desktop sales management for small retail shops</strong><br/>
  <em>v6.0.0 — Classic WinForms · N-Layer 2-Tier</em>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10"/>
  <img src="https://img.shields.io/badge/WinForms-Desktop-0078D4?style=for-the-badge&logo=windows&logoColor=white" alt="WinForms"/>
  <img src="https://img.shields.io/badge/SQL%20Server-2019+-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server"/>
  <img src="https://img.shields.io/badge/Architecture-N--Layer%202--Tier-2ECC71?style=for-the-badge" alt="N-Layer 2-Tier"/>
</p>

---

## Overview

A desktop application for a single retail shop: sales invoices, purchases, returns, inventory and
warehouses, cash boxes, chart of accounts, journal entries, and reports. The UI is Arabic (RTL,
`Tahoma`), and the whole product runs on one machine against a local SQL Server instance.

One admin user with full access. No roles, no permission checks.

**Desktop-only.** There is no server, no HTTP, no API. The WinForms client loads the business layer
in the same process and talks to SQL Server itself.

---

## Architecture — N-Layer, 2-Tier

Seven projects, one solution:

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

Data flow:

```text
Desktop (WinForms) → Business (BLL) → DataAccess (EF Core) → SQL Server
```

| Layer | Its only job |
|---|---|
| `SalesSystem.Desktop` | Forms (`frm*`), DI wiring, helpers, desktop services. No rules, no SQL. |
| `SalesSystem.Business` | Business rules, validation, calculations, transactions, journal automation. |
| `SalesSystem.DataAccess` | EF Core `SalesDbContext`, Fluent configurations, repositories, `IUnitOfWork`, migrations, seeding. |
| `SalesSystem.Models` / `SalesSystem.DTO` / `SalesSystem.Common` | Entities, request/response contracts, enums, `Result<T>`, shared helpers. |

> **No API layer, ever.** The Desktop never uses `HttpClient`, never serializes JSON on the data
> path, and never touches `DbContext`. Anything that looks like one is a bug.

---

## Screen ⇄ Service pattern (Direct)

Every screen resolves an interface from DI and calls it:

- Interface: `I<Entity>DesktopService` (in `Desktop/Services/Interfaces/`)
- Real mode: `Direct<Entity>Service` — a thin wrapper that calls the Business service in-process
- Mock mode: `Mock<Entity>DesktopService` — in-memory data, no database
- Every method returns `Result` / `Result<T>`; no exceptions cross the boundary

```csharp
public frmCustomerMaster(IServiceProvider serviceProvider)
{
    _serviceProvider = serviceProvider;
    InitializeComponent();
}

private ICustomerDesktopService CustomerService
    => _serviceProvider.GetRequiredService<ICustomerDesktopService>();
```

The wrapper holds no logic of its own — it maps DTOs and forwards the BLL's `Result<T>` unchanged.

---

## UI doctrine — classic WinForms, reusable Helpers

- **Code-behind only.** Event handlers (`btnSave_Click`, `dgvMain_CellDoubleClick`, …) call services.
  No MVVM, no ViewModel classes, no binding frameworks, no `ICommand` plumbing.
- **Helpers own all repeated behaviour.** Screens consume them; they never re-implement them.

| Helper | Owns |
|---|---|
| `GreenThemeHelper` | Every colour of the system — form background, header bar, grids, group boxes, highlight text. |
| `KeyboardHelper` | Enter-as-Tab, select-all-on-focus, focus highlight, RTL arrow navigation, disabled-button visuals, global shortcuts. |
| `LiveTafqeetHelper` | Live Arabic amount-in-words tooltip on money fields. |
| `RowFilterHelper` | Safe `DataView.RowFilter` values (escaping, known columns only). |
| `ThemeHelper` | Base form/button styling; applied before the green theme. |
| `ReportUiStandardizer` | Shared layout, filter panel and summary bar for report screens. |

**Changing a colour anywhere in the system = editing one constant in `GreenThemeHelper`.** Screens
must not contain colour literals.

---

## Mock mode (UI-first)

`appsettings.json` ships with `UseMockData: true`.

- No database and no server needed — services resolve to in-memory `Mock*` implementations.
- Login accepts any credentials.
- The main window title shows `وضع التجربة (Mock)`.
- Set `UseMockData: false` to run against real SQL Server; at startup the app applies migrations and
  seeds the baseline data automatically before the login screen appears.

---

## Financial integrity

Money is `decimal(18,2)`. Quantities are `decimal(18,3)`. Never `float`, `double`, or `money`.

```csharp
// sales line
LineTotal = (Quantity * UnitPrice) - DiscountAmount;

// purchase line
LineTotal = (Quantity * UnitCost) - DiscountAmount;

// invoice
SubTotal        = Items.Sum(i => i.LineTotal);
NetTotal        = SubTotal - InvoiceDiscount + TaxAmount + OtherCharges;
RemainingAmount = NetTotal - PaidAmount;
```

- Invoice lifecycle: `Draft (1) → Posted (2) → Cancelled (3)`. No hard delete, no editing a posted invoice.
- Every financial write happens inside a transaction (`IUnitOfWork`).
- Every stock change is recorded in `InventoryTransactions`.
- `WarehouseStocks.Quantity` carries a `CHECK (Quantity >= 0)` constraint — negative stock is impossible.

---

## Build & run

From the solution folder (`SalesSystem/`):

```powershell
dotnet build .\SalesSystem.slnx
dotnet build .\SalesSystem.Desktop\SalesSystem.Desktop.csproj
dotnet build .\SalesSystem.Business\SalesSystem.Business.csproj
dotnet build .\SalesSystem.DataAccess\SalesSystem.DataAccess.csproj
```

From the repo root:

```powershell
dotnet run --project .\SalesSystem\SalesSystem.Desktop\SalesSystem.Desktop.csproj
```

---

## Testing

Automated tests are **not** written by AI agents. `SalesSystem.Tests` covers the `SalesSystem.Common`
helpers only and is maintained by the project owner.

Every delivered change ships with a **manual test checklist** in its delivery report — screen →
action → expected result — which the owner executes.

---

## Documentation & rules

| Document | Contents |
|---|---|
| [`AGENTS.md`](AGENTS.md) | Must read first. Architecture, rules, enums, forbidden patterns, UI standards. |
| [`docs/CONSTITUTION.md`](docs/CONSTITUTION.md) | Financial formulas, transaction protocol, non-negotiables. |
| [`docs/BUSINESS-RULES.md`](docs/BUSINESS-RULES.md) | Domain rules per module. |
| [`docs/database-schema.md`](docs/database-schema.md) | Tables, types, constraints, indexes. |
| [`docs/PRD-MVP.md`](docs/PRD-MVP.md) | Product requirements (reference). |
| [`docs/ui-screens.md`](docs/ui-screens.md) | WinForms screen guidelines. |

`docs/archive/**` is historical only — it describes earlier plans and does not reflect the current
architecture.

---

## Deferred to V2

- Roles and permissions (single admin with full access today).
- Per-operation DI scope (one application-lifetime scope is used today).
- Stored Employees module (`MockEmployeeDesktopService` only today).
