# Database Schema Design
# Sales Management System — v5.0.0 (WinForms N-Layer)
# Platform: SQL Server 2019+
# MVP (V1): ~35 Active Tables | V2 Deferred: ~18 Tables | decimal-only financials | nvarchar text | Soft delete | All FK Restrict

> **MVP Scope**: Single shop, single user (Admin), single branch, single warehouse, single cash box.
> Tables marked `⛔ V2 DEFERRED` are defined in the schema but **hidden from the UI**. They remain in the database for future upgrade compatibility.

---

## Base Inheritance Hierarchy
Five base classes control column inheritance:
- **Entity**: `Id int PK`
- **AuditableEntity**: `Id int PK`, `CreatedAt datetime2`, `CreatedByUserId int FK`, `UpdatedAt datetime2`, `UpdatedByUserId int FK`
- **ActivatableEntity**: AuditableEntity + `IsActive bit`
- **DocumentEntity**: AuditableEntity + `Status tinyint`
- **LongEntity**: `Id bigint PK`

## Key Constraints
- **All FK relationships**: `DeleteBehavior.Restrict` — NO cascade delete anywhere.
- **Soft delete**: Global query filter `IsActive == true` on ActivatableEntity.
- **CHECK constraints**: Stock quantities (`WarehouseStocks.Quantity >= 0`), financial amounts, debit/credit rules enforced at DB level.

---

## Module 1: Core & Security
Includes: `Customers`, `Suppliers`, `Users`, `Roles`, `Permissions`, `RolePermissions`, `SystemLogs`, `AuditLogs`, `Branches`

## Module 2: Products & Inventory
Includes: `Categories`, `Products`, `Units`, `ProductUnits`, `UnitBarcodes`, `ProductPrices`

## Module 3: Warehousing
Includes: `Warehouses`, `WarehouseStocks`, `InventoryBatches`, `InventoryTransactions`, `InventoryTransactionLines`, `WarehouseTransfers`, `WarehouseTransferLines`

## Module 4: Sales & Purchases
Includes: `SalesInvoices`, `SalesInvoiceLines`, `PurchaseInvoices`, `PurchaseInvoiceLines`, `SalesReturns`, `SalesReturnLines`, `PurchaseReturns`, `PurchaseReturnLines`

## Module 5: Accounting
Includes: `Accounts`, `Currencies`, `JournalEntries`, `JournalEntryLines`, `CashBoxes`, `Banks`, `SystemAccountMappings`, `ReceiptVouchers`, `PaymentVouchers`, `Expenses`, `FiscalYears`

*(Note: For full column definitions of all 65 tables, refer to the EF Core Entity Configurations in the `SalesSystem.DataAccess` project. This document serves as the high-level schema blueprint.)*


##########################

# Database Schema Design
# Sales Management System — MVP V1 (Single-Shop Edition)
# Platform: SQL Server 2019+
# ~35 Active Tables (MVP) + ~18 V2 Deferred | decimal-only financials | nvarchar text | Soft delete | All FK Restrict

---

# 1) Important Design Rules Before Tables

## Data Types
- **Primary Keys**: `int IDENTITY(1,1)` — Named `Id` in all entities. High-volume audit tables use `bigint`. Small lookup tables use `smallint`.
- **Texts**: `nvarchar` (to support Arabic/English).
- **Barcodes**: `varchar(50)` — barcodes are ASCII-only.
- **Currency/Money**: `decimal(18,2)` — Precision 18, Scale 2.
- **Quantities**: `decimal(18,3)` — Precision 18, Scale 3.
- **Percentages**: `decimal(5,2)` — Precision 5, Scale 2.
- **Status and Types**: `tinyint` (for Enums).
- **Flags**: `bit` (0/1).
- **Dates**: `date` for document dates, `datetime2` for timestamps.

## Base Inheritance Hierarchy

Five base classes control column inheritance:

| Class | Id | CreatedAt | CreatedByUserId | UpdatedAt | UpdatedByUserId | IsActive | Status |
|-------|----|-----------|-----------------|-----------|-----------------|----------|--------|
| **Entity** | int PK | — | — | — | — | — | — |
| **AuditableEntity** | int PK | datetime2 | int null | datetime2 null | int null | — | — |
| **ActivatableEntity** | int PK | datetime2 | int null | datetime2 null | int null | bit default 1 | — |
| **DocumentEntity** | int PK | datetime2 | int null | datetime2 null | int null | — | tinyint |
| **LongEntity** | bigint PK | — | — | — | — | — | — |

Status values for DocumentEntity: `1=Draft, 2=Posted, 3=Cancelled`

## Inheritance Rules by Table Category
- **Admin entities** (Customers, Suppliers, etc.): `ActivatableEntity` — has IsActive for soft delete
- **Document entities** (Invoices, Vouchers, etc.): `DocumentEntity` — has Status (Draft/Posted/Cancelled)
- **Junction tables** (ProductUnits, etc.): \`Entity\` — no audit fields needed
- **Live balances** (WarehouseStocks): `AuditableEntity` — no soft delete, tracks who updated
- **High-volume logs** (AuditLogs, SystemLogs): `LongEntity` — bigint PK

## Key Constraints
- **All FK relationships**: `DeleteBehavior.Restrict` — NO cascade delete anywhere.
- **Soft delete**: Global query filter `IsActive == true` on ActivatableEntity inheritors.
- **CHECK constraints**: Stock quantities, financial amounts, debit/credit rules enforced at DB level.
- **Filtered unique indexes**: Soft-deletable entities include `AND [IsActive] = 1` to prevent conflicts with soft-deleted records.

## Branch Strategy (الممارسة المعمارية الآمنة)
- **Branches table** exists in V1 with one default branch (Id=1, Name="المركز الرئيسي")
- **BranchId FK** is `smallint not null` on most sovereign/operational entities (hardcoded to 1 in V1)
- **Exceptions — nullable BranchId**: `CashBoxes`, `Banks`, `SystemAccountMappings` — these entities support company-wide (NULL) or branch-specific (set) assignment
- **V1 behavior**: BranchId is hardcoded to 1 (single-branch). Branch dropdowns are hidden in UI.
- **V2 upgrade**: Enable branch dropdowns, add Branch management screen, assign users to branches
- **Pattern**: The database is fully multi-branch ready. The UI just hides the feature.

### BranchId Nullability Rules

| Entity | BranchId | Reason |
|--------|----------|--------|
| Customers, Suppliers | **NOT NULL** (default=1) | Every entity belongs to exactly one branch |
| SalesInvoices, PurchaseInvoices, SalesReturns, PurchaseReturns | **NOT NULL** (default=1) | Every transaction belongs to exactly one branch |
| CustomerReceipts, SupplierPayments, Expenses | **NOT NULL** (default=1) | Every financial document belongs to exactly one branch |
| JournalEntries, ReceiptVouchers, PaymentVouchers | **NOT NULL** (default=1) | Every accounting entry belongs to exactly one branch |
| WarehouseStocks (via Warehouses) | **NOT NULL** (via Warehouse) | Warehouse owns the stock record |
| InventoryTransactions, InventoryCounts, InventoryAdjustments, WarehouseTransfers | **NOT NULL** (default=1) | Every inventory operation belongs to exactly one branch |
| **CashBoxes** | **NULLABLE** | POS boxes = branch-specific; custody/HQ/payment gateway = company-wide |
| **Banks** | **NULLABLE** | Bank accounts typically serve all branches |
| **SystemAccountMappings** | **NULLABLE** | NULL = default system mapping; set = branch-specific override |

---

# 2) Database
Database Name: **`SalesSystemDb`**
Default schema: **`dbo`**

---

# 3) Module-by-Module Table Definitions

---

## Module 1: Core & Security (النواة والأمان)

### 1.1 Customers
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `Name` | nvarchar(200) not null | |
| `Phone` | nvarchar(20) null | |
| `Email` | nvarchar(100) null | |
| `Address` | nvarchar(500) null | |
| `TaxNumber` | nvarchar(30) null | |
| `Notes` | nvarchar(1000) null | |
| `AccountId` | int not null FK → Accounts(Id) | links to accounting tree |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `CreditLimit` | decimal(18,2) not null default 0 | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **Indexes** | `AccountId`, `Name`, `Phone` | |

### 1.2 Suppliers
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `Name` | nvarchar(200) not null | |
| `Phone` | nvarchar(20) null | |
| `Email` | nvarchar(100) null | |
| `Address` | nvarchar(500) null | |
| `TaxNumber` | nvarchar(30) null | |
| `Notes` | nvarchar(1000) null | |
| `AccountId` | int not null FK → Accounts(Id) | link to accounting; balance maintained in Accounting module |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `CreditLimit` | decimal(18,2) not null default 0 | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **Indexes** | `AccountId`, `Name`, `Phone` | |

### 1.3 Users
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `UserName` | nvarchar(50) not null | unique |
| `PasswordHash` | nvarchar(max) not null | |
| `RoleId` | smallint not null FK → Roles(Id) | Dynamic RBAC (Role-Based Access Control) |
| `EmployeeId` | int null FK → Employees(Id) | Optional link to employee card |
| `BranchId` | smallint null FK → Branches(Id) | null = all branches |
| `MustChangePassword` | bit not null default 0 | |
| `LoginAttempts` | smallint not null default 0 | |
| `IsLocked` | bit not null default 0 | |
| `LastLoginAt` | datetime2 null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **Index** | `UNIQUE(UserName)` | |

### 1.4 Roles
| Column | Type | Notes |
|--------|------|-------|
| `Id` | **smallint** PK | |
| `Name` | nvarchar(100) not null | unique |
| `SalesPermissions` | bigint not null default 0 | Module Bitmask (`-1` for Super Admin) |
| `InventoryPermissions` | bigint not null default 0 | Module Bitmask (`-1` for Super Admin) |
| `AccountingPermissions` | bigint not null default 0 | Module Bitmask (`-1` for Super Admin) |
| `SystemPermissions` | bigint not null default 0 | Module Bitmask (`-1` for Super Admin) |
| `IsSystem` | bit not null default 0 | System roles (e.g. Super Admin) protected from deletion |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**Seeded Roles**:

| Id | RoleName (Arabic) | Permissions | IsSystem | Editable? |
|----|---|---|---|---|
| 1 | مدير النظام الأعلى | `-1` in all modules (Super Admin) | 1 | No |
| 2 | مدير | Pre-defined bitmask | 1 | Yes |
| 3 | محاسب | Pre-defined bitmask | 1 | Yes |
| 4 | أمين صندوق | Pre-defined bitmask | 1 | Yes |
| 5 | كاشير | Pre-defined bitmask | 1 | Yes |
| 6 | مشرف مخازن | Pre-defined bitmask | 1 | Yes |
| 7 | مندوب مبيعات | Pre-defined bitmask | 1 | Yes |
| 8 | مراقب | Pre-defined bitmask | 1 | Yes |
| 9 | مدير فرع | Pre-defined bitmask | 1 | Yes |

> **Dynamic Permission Architecture (Bitmask)**: 
> 1. Roles define the authorization templates via multiple `bigint` (64-bit) columns per module.
> 2. `SuperAdmin` receives `-1` (all bits set to 1) for all permission columns, bypassing checks mathematically (`Perm & -1 = Perm`).
> 3. Custom roles are generated dynamically from the UI and saved directly to this table.
> 4. Users strictly inherit all permissions from their linked `RoleId`.

### 1.5 UserSessions ⛔ V2 DEFERRED

> **MVP Decision**: Single user (Admin) — no session tracking needed. Table exists in DB, UI hidden.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `UserId` | int not null FK → Users(Id) | |
| `SessionToken` | nvarchar(200) not null | |
| `DeviceName` | nvarchar(200) null | |
| `IpAddress` | nvarchar(50) null | |
| `UserAgent` | nvarchar(500) null | |
| `LastActivityAt` | datetime2 not null | |
| `ExpiresAt` | datetime2 not null | |
| `IsRevoked` | bit not null default 0 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **Index** | `(UserId, IsRevoked)` | for active session lookup |

---

## Module 2: Organization & Settings (التنظيم والإعدادات)

### 2.1 Warehouses
| Column | Type | Notes |
|--------|------|-------|
| `Id` | **smallint** PK | |
| `Name` | nvarchar(150) not null | |
| `Phone` | nvarchar(30) null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 — V1: all warehouses belong to branch 1 |
| `Address` | nvarchar(300) null | |
| `Notes` | nvarchar(500) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**Warehouse-Branch relationship (per analysis):**
- V1: All warehouses belong to the single default branch (BranchId=1)
- V2 options:
  - **Dedicated**: Each branch has its own warehouses (BranchId set per warehouse)
  - **Shared**: Central warehouse serves multiple branches (BranchId = NULL, stock allocated per branch via WarehouseStocks)
  - **Mixed**: Some warehouses dedicated, some shared
- Stock tracking (`WarehouseStocks`) is always per (Warehouse, Product) — branch-level stock is derived from warehouse ownership


### 2.2 Taxes ⛔ V2 DEFERRED
| Column | Type | Notes |
|--------|------|-------|
| `Id` | **smallint** PK | |
| `Name` | nvarchar(100) not null | |
| `Code` | nvarchar(20) not null | e.g., "VAT-15" |
| `Rate` | decimal(5,2) not null | CHK 0-100 |
| `TaxType` | tinyint not null | 1=Standard, 2=ZeroRated, 3=Exempt |
| `IsDefault` | bit not null default 0 | unique filtered `IsDefault=1 AND IsActive=1` |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

> **MVP Decision**: tax MANAGEMENT is hidden (V2). Invoices keep working with a single seeded default tax (e.g. VAT-15); adding/editing tax rates is deferred to V2.

### 2.3 CompanySettings
| Column | Type | Notes |
|--------|------|-------|
| `Id` | tinyint PK default 1 | singleton row |
| `CompanyName` | nvarchar(200) not null | |
| `Phone` | nvarchar(30) null | |
| `Email` | nvarchar(100) null | |
| `Address` | nvarchar(300) null | |
| `TaxNumber` | nvarchar(50) null | |
| `LogoPath` | nvarchar(500) null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

### 2.4 SystemSettings
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `SettingKey` | nvarchar(100) not null | unique |
| `SettingValue` | nvarchar(500) not null | |
| `SettingType` | tinyint not null | 1=String, 2=Integer, 3=Decimal, 4=Boolean |
| `Category` | nvarchar(100) not null | |
| `DisplayName` | nvarchar(200) not null | |
| `Description` | nvarchar(1000) null | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

### 2.5 DocumentSequences
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `DocumentType` | nvarchar(50) not null | e.g., "SalesInvoice", "PurchaseInvoice" |
| `NextNumber` | int not null | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

### 2.6 FiscalYears ⛔ V2 DEFERRED

> **MVP Decision**: Not required for initial release. Table exists in DB but not exposed in UI. JournalEntries reference will use a seeded default FiscalYear row.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `YearName` | nvarchar(20) not null | e.g., "2026" |
| `StartDate` | date not null | |
| `EndDate` | date not null | CHK `EndDate > StartDate` |
| `IsClosed` | bit not null default 0 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

### 2.7 Notifications ⛔ V2 DEFERRED

> **MVP Decision**: Single user — no multi-user notification system needed. Table exists, UI hidden.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `UserId` | int not null FK → Users(Id) | |
| `Type` | tinyint not null | 1=LowStock, 2=ExpirySoon, 3=CreditLimitExceeded, 4=System, 5=Reminder |
| `Title` | nvarchar(200) not null | |
| `Message` | nvarchar(1000) not null | |
| `ReferenceType` | nvarchar(50) null | |
| `ReferenceId` | int null | |
| `IsRead` | bit not null default 0 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **Indexes** | `(UserId, IsRead, CreatedAt DESC)` | for unread notification queries |

### 2.8 Attachments ⛔ V2 DEFERRED

> **MVP Decision**: Not essential for first release. Table exists, UI hidden.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `ReferenceType` | nvarchar(50) not null | e.g., "PurchaseInvoice", "InventoryAdjustment" |
| `ReferenceId` | int not null | |
| `FileName` | nvarchar(255) not null | |
| `FilePath` | nvarchar(500) not null | |
| `FileSize` | bigint not null | |
| `ContentType` | nvarchar(100) null | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **Index** | `(ReferenceType, ReferenceId)` | for document attachment lookup |

### 2.9 Branches (الفروع) — 🔒 MVP: Hardcoded Single Branch

> **MVP Decision**: Table exists with one seeded row `(Id=1, Name='المركز الرئيسي')`. `BranchId=1` is hardcoded everywhere. Branch management UI is **hidden**. V2 enables multi-branch UI.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | smallint PK | IDENTITY(1,1) |
| `Name` | nvarchar(150) not null | |
| `Address` | nvarchar(500) null | |
| `Phone` | nvarchar(30) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**MVP Seed**: `Id=1, Name='المركز الرئيسي', IsActive=1`. All BranchId FKs default to 1. No branch picker in UI.
**V2 Upgrade**: Enable branch dropdown, add branch management screen, user-branch assignment, branch-scoped reporting.

---

## Module 3: Products (المنتجات)

  ### 3.1 ProductCategories
  | Column | Type | Notes |
  |--------|------|-------|
  | `Id` | int PK | |
  | `Name` | nvarchar(100) not null | unique |
  | `Description` | nvarchar(500) null | |
  | `IsActive` | bit not null default 1 | |
  | `CreatedByUserId` | int null FK | |
  | `UpdatedByUserId` | int null FK | |
  | `CreatedAt` | datetime2 not null | |
  | `UpdatedAt` | datetime2 null | |

### 3.2 Products
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `Name` | nvarchar(200) not null | |
| `ForeignName` | nvarchar(200) null | |
| `CategoryId` | int not null FK → ProductCategories(Id) | |
| `Description` | nvarchar(500) null | |
| `TrackExpiry` | bit not null default 0 | |
| `ExpiryAlertDays` | smallint null | |
| `LocationInStore` | nvarchar(100) null | |
| `MinSalePrice` | decimal(18,2) not null default 0 | |
| `ImagePath` | nvarchar(500) null | primary image |
| `ReorderLevel` | decimal(18,3) not null default 0 | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

### 3.3 Units
| Column | Type | Notes |
|--------|------|-------|
| `Id` | **smallint** PK | |
| `Name` | nvarchar(50) not null | e.g., "حبة", "كرتون" |
| `Symbol` | nvarchar(20) null | e.g., "pc", "box" |
| `IsSystem` | bit not null default 0 | system units protected from modification |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

### 3.4 ProductUnits
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `ProductId` | int not null FK → Products(Id) | |
| `UnitId` | smallint not null FK → Units(Id) | |
| `Barcode` | nvarchar(50) null | unique filtered `Barcode IS NOT NULL AND IsActive=1` |
| `Factor` | decimal(18,3) not null | معامل التحويل: الوحدة الأساسية=1، كرتون=24 |
| `IsBaseUnit` | bit not null default 0 | وحدة أساسية واحدة فقط لكل منتج |
| `SalePrice` | decimal(18,2) not null default 0 | سعر التجزئة (Retail Price) |
| `WholesalePrice` | decimal(18,2) null | سعر الجملة (Wholesale Price) — اختياري |
| `CostPrice` | decimal(18,2) not null default 0 | سعر التكلفة المرجعي أو متوسط التكلفة المحسوبة |
| `MaxDiscountAmount` | decimal(18,2) null | أقصى خصم مسموح به (مبلغ ثابت) لهذه الوحدة |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

> ⚠️ **Cost Price Note**: The `CostPrice` here is a reference/standard cost. Actual purchase prices (which vary per batch) are stored in the `PurchaseInvoiceLines` and tracked via costing methods in the `InventoryBatches` and transactions.
> ⚠️ **Discount Rule**: `MaxDiscountAmount` is a **fixed amount only** — percentage discounts are NOT supported in V1.
> ⚠️ **NO CurrencyId in V1** — pricing is per ProductUnit only.



---

## Module 4: Accounting (المحاسبة)

### 4.1 Accounts
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `ParentId` | int null FK → Accounts(Id) | self-referencing hierarchy |
| `AccountCode` | nvarchar(20) not null | unique filtered `[IsActive]=1` |
| `NameAr` | nvarchar(200) not null | Arabic name |
| `NameEn` | nvarchar(200) null | English name for bilingual support |
| `Nature` | tinyint not null | 1=Asset, 2=Liability, 3=Equity, 4=Revenue, 5=Expense |
| `Level` | tinyint not null default 1 | CHECK 1–10 (`CHK_Account_Level_Range`); standard scheme uses tiers 1=Group(1 digit), 2=Main(2 digits), 3=Sub(4 digits), 4=Detail(8 digits) — see hierarchical numbering below |
| `IsLeaf` | bit not null default 1 | leaf accounts allow transactions (service-layer `AllowTransactions` alias) |
| `IsSystem` | bit not null default 0 | system accounts protected from modification |
| `CategoryId` | smallint null FK → AccountCategories(Id) | optional classification (DeleteBehavior.Restrict) |
| `Description` | nvarchar(500) null | Explanation of account purpose |
| `ColorCode` | nvarchar(7) null | Auto-generated hex color from Nature: #2B579A (Asset), #D32F2F (Liability), #6A1B9A (Equity), #2E7D32 (Revenue), #795548 (Expense) |
| `Notes` | nvarchar(300) null | Additional notes |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

#### 4.1.1 AccountCategories (تصنيفات الدليل المحاسبي) ⛔ V2 DEFERRED

> **MVP Decision**: Unnecessary complexity for single-shop MVP. `CategoryId` on Accounts is nullable — no impact. Table exists, UI hidden.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | smallint PK identity | |
| `Name` | nvarchar(100) not null | unique index `IX_AccountCategories_Name` |
| `Description` | nvarchar(300) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**Per analysis (`AccountNode`) — derived/not stored:** `treePath` ("/1/11/111/"), `financialStatement` (derived from `Nature`), `nature` DEBIT/CREDIT (derived from `Nature`), special-category detection (CUSTOMER / SUPPLIER / CASHBOX / BANK / EMPLOYEE / STANDARD — derived from linked entities). `allowCostCenter` becomes a stored bit only if CostCenters (§4.11) is adopted.

### Hierarchical Account Numbering Scheme

Account codes follow an expanding hierarchical pattern that encodes the account's level:

| Level | Digits | Pattern | Example |
|-------|--------|---------|---------|
| 1 (Group) | 1 | Single digit | `1` (Assets) |
| 2 (Main) | 2 | Level1 + 1-digit sequence | `11` (Current Assets) |
| 3 (Sub) | 4 | Level2 + 2-digit sequence | `1101` (Cash & Equivalents) |
| 4 (Detail) | 8 | Level3 + 4-digit sequence | `11010001` (Cash on Hand) |

This scheme allows up to 9,999 detail accounts per sub-category (e.g., 9,999 customer accounts under `1103` — Accounts Receivable).

**Auto-generation rule:** When creating a detail account under parent `1103`, the system queries `LIKE '1103%'` for the current max code, increments the suffix, and assigns the new code (e.g., `11030001` → `11030002`). Thread safety via `SemaphoreSlim`.

### 4.2 CashBoxes
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `AccountId` | int not null FK → Accounts(Id) | link to accounting; balance maintained in Accounting module |
| `Name` | nvarchar(150) not null | |
| `BranchId` | smallint null FK → Branches(Id) | NULL = company-wide (HQ treasury, employee custody, payment gateway); set = branch-specific POS box |
| `Description` | nvarchar(300) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**BranchId nullable design (per analysis):**
- **BranchId = specific value**: POS cashier box — cashier sees only boxes for his branch
- **BranchId = NULL**: Employee custody / HQ treasury / payment gateway — accessible from any branch

**Balance rule:** CashBoxes stores **no balance column** — the box balance is always the balance of the linked `Accounts` row (`AccountId`).

**Per analysis (`CashboxRecord`) — planned additions:**

| Column | Type | Notes |
|--------|------|-------|
| `Code` | nvarchar(20) null | short box code (e.g. "POS-01") |
| `Currency` | nvarchar(10) not null default 'YER' | display currency |
| `MaxCashLimit` | decimal(18,2) null | overdraft guard — withdrawal above limit blocked except Admin (analysis §2.c) |
| `CashierUserId` | int null FK → Users(Id) | default cashier bound to the box |
| `OpeningBalance` | decimal(18,2) null | informational only; posted via opening journal entry |

### 4.3 Banks ⛔ V2 DEFERRED

> **MVP Decision**: Single cash box covers all cash needs. No bank account management in MVP. Table exists, UI hidden. Enable in V2 when needed.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `AccountId` | int not null FK → Accounts(Id) | links to accounting tree |
| `Name` | nvarchar(150) not null | |
| `BranchId` | smallint null FK → Branches(Id) | NULL = company-wide bank account (serves all branches); set = branch-specific |
| `AccountNumber` | nvarchar(100) null | |
| `IBAN` | nvarchar(100) null | |
| `ContactPerson` | nvarchar(100) null | contact person at the bank branch |
| `ContactPhone` | nvarchar(50) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**Per analysis (`BankRecord`) — planned additions:**

| Column | Type | Notes |
|--------|------|-------|
| `Type` | tinyint not null default 1 | 1=BANK, 2=WALLET (e.g. JEB محفظة جيب), 3=POS_NETWORK |
| `Currency` | nvarchar(10) not null default 'YER' | |
| `BranchName` | nvarchar(100) null | bank branch name |
| `OverdraftLimit` | decimal(18,2) null | max negative balance allowed |
| `LastReconciliationDate` | date null | reconciliation tracking |

### 4.4 JournalEntries
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `EntryNumber` | nvarchar(50) not null | unique display string derived from `EntryNo` (unique index) |
| `EntryNo` | int not null | unique (unique index `IX_JournalEntries_EntryNo`); pure integer sequence (101, 102, 103…) per analysis |
| `EntryDate` | date not null | index `IX_JournalEntries_EntryDate` |
| `FiscalYearId` | smallint not null FK → FiscalYears(Id) | Restrict — period reporting + closed-period protection |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `EntryType` | tinyint not null | 1=Manual, 2=Sales, 3=Purchase, 4=Receipt, 5=Payment, 6=Inventory, 7=Adjustment |
| `ReferenceType` | nvarchar(50) null | 'SalesInvoice', 'PurchaseInvoice', etc. |
| `ReferenceId` | int null | |
| `ReferenceNumber` | nvarchar(50) null | external document number (display) |
| `AttachmentPath` | nvarchar(500) null | local scan/reference path — no cloud storage in V1 (per analysis §7) |
| `Description` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `IsReversed` | bit not null default 0 | |
| `ReversedByEntryId` | int null FK → JournalEntries(Id) | Restrict |
| `ReviewedByUserId` | int null FK → Users(Id) | set when posted (reviewer) |
| `ReviewedAt` | datetime2 null | set when posted |
| `CreatedByUserId` | int null FK → Users(Id) | extracted from JWT, never client-supplied |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |
| **Indexes** | unique `EntryNumber`, unique `EntryNo`, `EntryDate`, filtered `(ReferenceType, ReferenceId)` | |
| **Notes** | query filter hides `Status = 3 (Cancelled)` from default LINQ queries | |
| **Per analysis** | planned: `SourceModule` nvarchar(30) null — originating module tag (`createdBy`/`sourceModule` in `JournalEntry`) | |

### 4.5 JournalEntryLines
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `JournalEntryId` | int not null FK → JournalEntries(Id) | index + composite `(JournalEntryId, AccountId)` |
| `AccountId` | int not null FK → Accounts(Id) | index |
| `Debit` | decimal(18,2) not null default 0 | |
| `Credit` | decimal(18,2) not null default 0 | |
| `Description` | nvarchar(300) null | |
| `SortOrder` | smallint not null default 0 | |
| **CHK** | `CHK_DebitOrCredit` | `(Debit > 0 AND Credit = 0) OR (Credit > 0 AND Debit = 0) OR (Debit = 0 AND Credit = 0)` |
| **CHK** | `CHK_NoNegativeValues` | Debit >= 0 AND Credit >= 0 |
| **Per analysis** | planned: `CostCenterId` int null FK → CostCenters(Id) — expense/branch attribution (§4.11) | |

### 4.6 ReceiptVouchers (سندات قبض)
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `VoucherNo` | int not null | unique |
| `VoucherDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `CashBoxId` | int not null FK → CashBoxes(Id) | |
| `AccountId` | int not null FK → Accounts(Id) | |
| `TotalAmount` | decimal(18,2) not null | |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |

**Per analysis (`CashVoucher` type=RECEIPT) — planned additions:**

| Column | Type | Notes |
|--------|------|-------|
| `PaymentMethod` | tinyint not null default 1 | 1=Cash, 2=BankTransfer, 3=Cheque (analysis `paymentMethod`) |
| `PartyType` | tinyint null | 1=Customer, 2=Supplier, 3=Expense, 4=Other (analysis `partyType`) |
| `PartyId` | int null | FK target depends on PartyType (CustomerId/SupplierId/…) |
| `Reference` | nvarchar(100) null | cheque / حوالة / سند يدوي number (analysis `reference`/`referenceNo`) |
| `CheckNumber` | nvarchar(50) null | cheque attribute stored on voucher — no separate Cheques table in V1 |
| `BankName` | nvarchar(100) null | issuing bank name for cheques/transfers |
| `DueDate` | date null | cheque maturity date |
| `CostCenterId` | int null FK → CostCenters(Id) | attribution (§4.11) |
| `VoidedAt` | datetime2 null | analysis `VOID` state maps to existing `Status=3 (Cancelled)` + audit columns |
| `VoidReason` | nvarchar(300) null | |

> Invoice settlement: use **§4.12 VoucherInvoiceAllocations (planned)** — mirrors analysis `allocatedInvoices`.

### 4.7 PaymentVouchers (سندات صرف)
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `VoucherNo` | int not null | unique |
| `VoucherDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `CashBoxId` | int not null FK → CashBoxes(Id) | |
| `AccountId` | int not null FK → Accounts(Id) | |
| `TotalAmount` | decimal(18,2) not null | |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |

**Per analysis (`CashVoucher` type=PAYMENT) — planned additions:** identical set as §4.6 (`PaymentMethod`, `PartyType`, `PartyId`, `Reference`, `CheckNumber`, `BankName`, `DueDate`, `CostCenterId`, `VoidedAt`, `VoidReason`) + **§4.12 VoucherInvoiceAllocations** for supplier-payment settlement.

### 4.8 TreasuryTransfers (تحويلات الخزينة) ⛔ V2 DEFERRED

> **MVP Decision**: Single cash box — no transfers between boxes needed. Table not yet in EF. UI hidden.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `TransferNo` | int not null | unique, pure integer sequence (3001, 3002...) |
| `TransferDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `FromCashBoxId` | int not null FK → CashBoxes(Id) | source box |
| `ToCashBoxId` | int not null FK → CashBoxes(Id) | destination box |
| `Amount` | decimal(18,2) not null | |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |

> ⚠️ **Implementation status:** `TreasuryTransfers` is **not yet an EF entity/table** (no model, no migration). The Desktop currently runs this screen via `MockTreasuryTransferApiService`. Create the table + entity before switching off mock mode.

**Per analysis (`FrmCash_TreasuryTransfer`) — planned columns beyond box→box:**

| Column | Type | Notes |
|--------|------|-------|
| `FromBankId` | int null FK → Banks(Id) | nullable box/bank legs allow box↔bank and bank↔bank transfers |
| `ToBankId` | int null FK → Banks(Id) | |
| `CostCenterId` | int null FK → CostCenters(Id) | attribution (§4.11) |

### 4.9 Expenses
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `ExpenseNo` | int not null | unique |
| `ExpenseDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `ExpenseAccountId` | int not null FK → Accounts(Id) | |
| `CashBoxId` | int not null FK → CashBoxes(Id) | |
| `Amount` | decimal(18,2) not null | |
| `BaseNetTotal` | decimal(18,2) null | amount converted to base currency using the exchange rate; NULL = same as Amount |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |

### 4.10 SystemAccountMappings (NEW: Accounting Defaults)
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | inherits `Entity` — **no audit columns** |
| `MappingKey` | nvarchar(100) not null | e.g., "SalesRevenue", "COGS"; matches `SystemAccountKey` enum names |
| `BranchId` | smallint null FK → Branches(Id) | column exists; see unique-index caveat below |
| `AccountId` | int not null FK → Accounts(Id) | Restrict |
| **Indexes** | unique filtered `IX_SystemAccountMappings_MappingKey` on `(MappingKey)` WHERE `MappingKey IS NOT NULL`, plus non-unique `AccountId` | |
| **Notes** | Flexible key-value design | replaces fixed-column approach |

> ⚠️ **Discrepancy vs analysis branch-override design:** the actual unique index is on `MappingKey` **alone** (one row per key), so a default row + branch-specific override row cannot coexist. To realize the branch routing pattern documented below, the index must be changed to `UNIQUE(MappingKey, BranchId)` and null-distinguished — tracked as a known gap.

**Known `SystemAccountKey` mapping keys (seeded via `AccountingSeeder`):**

| # | Key | # | Key |
|---|-----|---|-----|
| 1 | `DefaultCash` | 12 | `Capital` |
| 2 | `DefaultBank` | 13 | `OpeningBalanceEquity` |
| 3 | `AccountsReceivable` | 14 | `RetainedEarnings` |
| 4 | `AccountsPayable` | 15 | `UndistributedProfits` |
| 5 | `Inventory` | 16 | `InventoryShortage` |
| 6 | `CostOfGoodsSold` | 17 | `InventorySurplus` |
| 7 | `SalesRevenue` | 18 | `GeneralExpense` |
| 8 | `SalesReturns` | 19 | `SpoilageLoss` |
| 9 | `PurchaseReturns` | 20 | `EmployeeCustody` |
| 10 | `VatOutput` | 21 | `DeliveryChargesRevenue` |
| 11 | `VatInput` | | |

**Branch routing pattern (per analysis):**
- `BranchId IS NULL` → default system mapping (applies to ALL branches)
- `BranchId = 1` → branch-specific override (only for branch 1)
- **Lookup order**: First search for branch-specific row; if not found, fall back to NULL (default)
- Example: `SalesRevenue` + `BranchId=NULL` → account 1520 (default). `SalesRevenue` + `BranchId=2` → account 1525 (branch 2 override)

**Design Notes:**
- **OpeningBalance** is NOT stored on the Account entity. When an account is created with an opening balance, the system generates an automatic Journal Entry (Debit/Credit based on Nature) against the `OpeningBalanceEquity` (1422) account within a database transaction. This ensures double-entry integrity.
- NO CashTransactions table (replaced by ReceiptVouchers/PaymentVouchers)
- NO DailyClosures table (removed from V1)
- NO Cheques table (removed from V1)
- SystemAccountMappings uses flexible key-value design with BranchId override support

### Branch-Specific Account Routing (per analysis)

SystemAccountMappings supports branch-level overrides via nullable `BranchId`:

```
Scenario: SalesRevenue routing
┌──────────────┬──────────┬────────────┐
│ MappingKey   │ BranchId │ AccountId  │
├──────────────┼──────────┼────────────┤
│ SalesRevenue │ NULL     │ 1520 (default) │
│ SalesRevenue │ 2        │ 1525 (override) │
│ COGS         │ NULL     │ 5101       │
└──────────────┴──────────┴────────────┘

Lookup: Branch 1 → no override found → falls back to 1520
Lookup: Branch 2 → override found → uses 1525
```

This eliminates the need for a separate `BranchFinancialSettings` table — one unified table handles both global defaults and per-branch overrides.

*(subject to the `MappingKey` unique-index caveat in §4.10 above)*

### 4.11 CostCenters (مراكز التكلفة — per analysis, **planned / not yet in EF**)

> Source: analysis `CostCenterNode` + `FrmAccount_CostCenters` / `AccountingCostCenterModal`. No `CostCenters` table exists in migrations yet.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `Code` | nvarchar(20) not null | unique (e.g. "201") |
| `Name` | nvarchar(200) not null | Arabic name |
| `NameEn` | nvarchar(200) null | |
| `ParentId` | int null FK → CostCenters(Id) | self-referencing hierarchy (Restrict) |
| `Level` | tinyint not null default 1 | CHECK 1–10 |
| `IsLeaf` | bit not null default 1 | leaf centers accept postings |
| `Budget` | decimal(18,2) not null default 0 | الموازنة التقديرية |
| `ManagerName` | nvarchar(100) null | |
| `IsActive` | bit not null default 1 | analysis `status ACTIVE/INACTIVE` |
| `Notes` | nvarchar(300) null | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**Consumers (when adopted):** `JournalEntryLines.CostCenterId` (§4.5), Receipt/Payment Vouchers `CostCenterId` (§4.6/§4.7), TreasuryTransfers `CostCenterId` (§4.8).

### 4.12 VoucherInvoiceAllocations (تسوية السندات على الفواتير — per analysis, **planned / not yet in EF**)

> Source: analysis `VoucherInvoiceAllocation` (`allocatedInvoices` on `CashVoucher`) — lets a receipt/payment be spread across several invoices (partial settlement). Solution currently has this only for `CustomerReceipts` (§6.5/§6.6) and `SupplierPayments` (§7.5/§7.6), **not** for accounting `ReceiptVouchers`/`PaymentVouchers`.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `VoucherType` | tinyint not null | 1=ReceiptVoucher, 2=PaymentVoucher |
| `VoucherId` | int not null | FK to ReceiptVouchers/PaymentVouchers per `VoucherType` |
| `InvoiceType` | tinyint not null default 1 | 1=SalesInvoice, 2=PurchaseInvoice |
| `InvoiceId` | int not null | FK to SalesInvoices/PurchaseInvoices per `InvoiceType` |
| `AllocatedAmount` | decimal(18,2) not null | must be > 0 and ≤ voucher remaining |
| **UK** | `UNIQUE(VoucherType, VoucherId, InvoiceType, InvoiceId)` | one allocation row per invoice per voucher |
| **Notes** | index `(InvoiceType, InvoiceId)` for statement/aging queries | |

### 4.13 FiscalPeriods (الفترات المالية — per analysis, **planned / not yet in EF**)

> Source: analysis `FiscalPeriod` (`AccountingModule` close/reopen with `closingEntryId`). Solution V1 currently tracks only **whole fiscal years** (`FiscalYears`, §2.6: `IsOpen`, `OpenedAt/ClosedAt`, `OpenedBy/ClosedBy`); month/quarter periods are not yet stored.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `FiscalYearId` | smallint not null FK → FiscalYears(Id) | |
| `PeriodName` | nvarchar(100) not null | e.g. "يناير 2026", "الربع الأول 2026" |
| `PeriodType` | tinyint not null | 1=Month, 2=Quarter, 3=Year |
| `StartDate` | date not null | |
| `EndDate` | date not null | |
| `IsClosed` | bit not null default 0 | closed periods block voucher/entry dates (analysis §2.د) |
| `ClosedAt` | datetime2 null | |
| `ClosedByUserId` | int null FK → Users(Id) | |
| `ClosingNotes` | nvarchar(500) null | |
| `ClosingEntryId` | int null FK → JournalEntries(Id) | automated period-closing entry (revenue/expense → retained earnings) |
| **Notes** | index `(FiscalYearId, StartDate, EndDate)` | date-range lookup for period protection |

### Module 4 Notes (per analysis `AGENTS.md`/`GEMINI.md`)

- **Pure integer document numbers** (no dates/strings embedded): Receipts `1001…`, Payments `2001…`, Treasury Transfers `3001…`, Journal entries `101…` — enforced via `DocumentSequences` (§2.5).
- **Strict lifecycle enums as tinyint**: journal/voucher states 1=Draft, 2=Posted, 3=Cancelled (analysis additionally shows `VOID` for vouchers → maps to Cancelled + void audit columns).
- **Balance enforcement**: `SUM(Debit) = SUM(Credit)` validated in the service layer before `Status=2` (DB CHECK allows all-zero rows only as placeholder lines).
- **Closed-period protection**: `FiscalYears.IsOpen` (§2.6) and planned `FiscalPeriods.IsClosed` (§4.13) block posting entries/vouchers dated inside a closed period.
- **Cheques**: no standalone Cheques table in V1 — cheque attributes live on the voucher (`PaymentMethod=3`, `CheckNumber`, `BankName`, `DueDate`).
- **Petty cash / employee advances**: no separate tables — petty-cash requests post as `Expenses` (§4.9) or PaymentVouchers; employee advances/custody use `SystemAccountKey.EmployeeCustody` (20) and employee AR accounts (analysis §6).
- **Accounting permission flags** (analysis `lib/permissions.ts`): enforced at UI level (e.g. `ViewTrialBalance`); DB-side enforcement is via `User.PermissionsMask` (§1.4).

---

## Module 5: Inventory (المخزون)

### 5.1 WarehouseStocks
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `ProductId` | int not null FK → Products(Id) | |
| `Quantity` | decimal(18,3) not null default 0 | CHK `>= 0` |
| `CreatedAt` | datetime2 not null | |
| `CreatedByUserId` | int null FK | |
| `UpdatedAt` | datetime2 null | |
| `UpdatedByUserId` | int null FK | |
| **UK** | `UNIQUE(WarehouseId, ProductId)` | one stock row per product per warehouse |
| **Notes** | NO IsActive | live balance entity |

### 5.2 InventoryBatches
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `BatchNo` | int not null | internal batch number | from DocumentSequences optional end user can changed
| `ProductId` | int not null FK → Products(Id) | |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `PurchaseInvoiceId` | int null FK → PurchaseInvoices(Id) | source purchase invoice |
| `PurchaseInvoiceLineId` | int null FK → PurchaseInvoiceLines(Id) | source purchase invoice line |
| `SupplierBatchNo` | int null | supplier's batch reference |
| `ExpiryDate` | date null | for FEFO tracking |
| `QuantityReceived` | decimal(18,3) not null | CHK `>= 0` |
| `QuantityRemaining` | decimal(18,3) not null | CHK `>= 0` |
| `UnitCost` | decimal(18,2) not null | CHK `>= 0` |
| `IsClosed` | bit not null default 0 | CHK: 0 when QtyRemaining > 0, 1 when fully consumed |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **Indexes** | `(ProductId, WarehouseId)`, `(ExpiryDate)` filtered, `(PurchaseInvoiceId)` | |

### 5.3 InventoryTransactions
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `TransactionNo` | int not null | unique |
| `TransactionType` | tinyint not null | 1=Purchase, 2=PurchaseReturn, 3=Sale, 4=SaleReturn, 5=TransferOut, 6=TransferIn, 7=Count, 8=Adjustment, 9=Damage, 10=OpeningBalance, 11=InternalIssue, 12=InternalReceipt |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `ReferenceType` | tinyint null | 1=PurchaseInvoice, 2=SalesInvoice, 3=PurchaseReturn, 4=SalesReturn, 5=Transfer, 6=Count, 7=Adjustment |
| `ReferenceId` | int null | FK to the reference document |
| `TransactionDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **Indexes** | `(WarehouseId, TransactionDate DESC)`, `(ReferenceType, ReferenceId)` | |

### 5.4 InventoryTransactionLines
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `InventoryTransactionId` | int not null FK → InventoryTransactions(Id) | |
| `ProductId` | int not null FK → Products(Id) | |
| `ProductUnitId` | int not null FK → ProductUnits(Id) | |
| `BatchId` | int null FK → InventoryBatches(Id) | null for non-batched products |
| `Quantity` | decimal(18,3) not null | |
| `UnitCost` | decimal(18,2) not null | |
| `TotalCost` | decimal(18,2) not null | UnitCost x Quantity |

### 5.5 InventoryCounts ⛔ V2 DEFERRED

> **MVP Decision**: Manual inventory counting is sufficient for a small shop MVP. Use InventoryAdjustments for opening balances and corrections. Enable full counting module in V2.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `CountNo` | int not null | unique |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `CountDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |

### 5.6 InventoryCountLines ⛔ V2 DEFERRED

> **MVP Decision**: Deferred with InventoryCounts (§5.5).

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `InventoryCountId` | int not null FK → InventoryCounts(Id) | |
| `ProductId` | int not null FK → Products(Id) | |
| `BatchId` | int not null FK → InventoryBatches(Id) | links count to specific batch |
| `SystemQuantity` | decimal(18,3) not null | expected quantity from system |
| `ActualQuantity` | decimal(18,3) not null | counted quantity |
| `DifferenceQuantity` | decimal(18,3) not null | ActualQuantity - SystemQuantity |

### 5.7 InventoryAdjustments
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `AdjustmentNo` | int not null | unique |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `AdjustmentType` | tinyint not null | 1=Opening, 2=Increase, 3=Shortage, 4=Damage |
| `AdjustmentDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |

### 5.8 InventoryAdjustmentLines
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `InventoryAdjustmentId` | int not null FK → InventoryAdjustments(Id) | |
| `ProductId` | int not null FK → Products(Id) | |
| `BatchId` | int null FK → InventoryBatches(Id) | null for opening adjustments |
| `Quantity` | decimal(18,3) not null | |
| `UnitCost` | decimal(18,2) not null | |
| `TotalCost` | decimal(18,2) not null | UnitCost x Quantity |

### 5.9 WarehouseTransfers ⛔ V2 DEFERRED

> **MVP Decision**: Single warehouse — no transfers needed. `WarehouseId=1` hardcoded everywhere. Table exists, UI hidden. Enable in V2 when multiple warehouses are needed.

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `TransferNo` | int not null | unique |
| `FromWarehouseId` | smallint not null FK → Warehouses(Id) | source warehouse |
| `ToWarehouseId` | smallint not null FK → Warehouses(Id) | destination warehouse |
| `TransferDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |

### 5.10 WarehouseTransferLines ⛔ V2 DEFERRED

> **MVP Decision**: Deferred with WarehouseTransfers (§5.9).

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `WarehouseTransferId` | int not null FK → WarehouseTransfers(Id) | |
| `ProductId` | int not null FK → Products(Id) | |
| `BatchId` | int not null FK → InventoryBatches(Id) | specific batch being transferred |
| `Quantity` | decimal(18,3) not null | |
| `UnitCost` | decimal(18,2) not null | |
| `TotalCost` | decimal(18,2) not null | UnitCost x Quantity |

**Design Notes:**
- NO InventoryMovements table (replaced by InventoryTransactions/InventoryTransactionLines)
- NO InventoryOperations table (merged into InventoryTransactions)
- NO StockTransfers (renamed to WarehouseTransfers)
- NO StockWriteOffs (covered by InventoryAdjustments with Damage type)
- WarehouseStocks has NO IsActive (live balance)
- Transfers do NOT create accounting entries (only adjustments do)

---

## Module 6: Sales (المبيعات)

### 6.1 SalesInvoices
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `InvoiceNo` | int not null | user-facing number, UNIQUE per table |
| `InvoiceDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `CustomerId` | int not null FK → Customers(Id) | |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `PaymentType` | tinyint not null | 1=Cash, 2=Credit, 3=Mixed |
| `CashBoxId` | int null FK → CashBoxes(Id) | |
| `TaxId` | smallint null FK → Taxes(Id) | |
| `SubTotal` | decimal(18,2) not null | |
| `DiscountAmount` | decimal(18,2) not null default 0 | خصم الفاتورة — مبلغ ثابت فقط |
| `CostInBaseCurrency` | decimal(18,2) null | total cost in base currency for profit calculation |
| `TaxAmount` | decimal(18,2) not null default 0 | |
| `OtherCharges` | decimal(18,2) not null default 0 | |
| `NetTotal` | decimal(18,2) not null | SubTotal - Discount + Tax + OtherCharges |
| `PaidAmount` | decimal(18,2) not null default 0 | CHK 0 to NetTotal |
| `RemainingAmount` | decimal(18,2) not null default 0 | NetTotal - PaidAmount |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |
| **UK** | `UNIQUE(InvoiceNo)` | |

### 6.2 SalesInvoiceLines
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `SalesInvoiceId` | int not null FK → SalesInvoices(Id) | |
| `ProductId` | int not null FK → Products(Id) | |
| `ProductUnitId` | int not null FK → ProductUnits(Id) | |
| `Quantity` | decimal(18,3) not null | |
| `UnitPrice` | decimal(18,2) not null | |
| `DiscountType` | tinyint not null default 0 | 0=Amount, 1=Percentage per line |
| `DiscountRate` | decimal(5,2) null | percentage rate when line DiscountType=1 |
| `DiscountAmount` | decimal(18,2) not null default 0 | line-level discount |
| `UnitCost` | decimal(18,2) not null default 0 | average cost at time of sale |
| `CostInBaseCurrency` | decimal(18,2) null | cost converted to base currency |
| `ProfitAmount` | decimal(18,2) not null default 0 | LineTotal - (CostInBaseCurrency × Quantity) |
| `LineTotal` | decimal(18,2) not null | (Quantity × UnitPrice) - DiscountAmount |
| **Notes** | Line-level DiscountType/DiscountAmount supported. CostInBaseCurrency populated at posting from FIFO batches. | |

### 6.3 SalesReturns
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `ReturnNo` | int not null | unique |
| `ReturnDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `SalesInvoiceId` | int not null FK → SalesInvoices(Id) | |
| `CustomerId` | int not null FK → Customers(Id) | |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `TotalAmount` | decimal(18,2) not null | |
| `ReturnedDiscountAmount` | decimal(18,2) not null default 0 | proportional discount from original invoice |
| `ReturnedTaxAmount` | decimal(18,2) not null default 0 | proportional tax from original invoice |
| `ReturnedChargeAmount` | decimal(18,2) not null default 0 | proportional other charges from original invoice |
| `RefundAmount` | decimal(18,2) not null default 0 | amount refunded to customer |
| `ReturnReason` | nvarchar(500) null | optional reason for return |
| `TaxId` | smallint null FK → Taxes(Id) | tax rate used from original invoice |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |

### 6.4 SalesReturnLines
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `SalesReturnId` | int not null FK → SalesReturns(Id) | |
| `SalesInvoiceLineId` | int not null FK → SalesInvoiceLines(Id) | links to original line |
| `Quantity` | decimal(18,3) not null | |
| `Amount` | decimal(18,2) not null | |
| `CostInBaseCurrency` | decimal(18,2) null | cost of returned goods in base currency |

### 6.5 CustomerReceipts (سندات قبض)
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `ReceiptNo` | int not null | unique |
| `ReceiptDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `CustomerId` | int not null FK → Customers(Id) | |
| `CashBoxId` | int not null FK → CashBoxes(Id) | |
| `PaymentMethod` | tinyint not null default 1 | 1=Cash, 2=Cheque, 3=BankTransfer, 4=CreditCard |
| `Amount` | decimal(18,2) not null | |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |

### 6.6 CustomerReceiptApplications
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `CustomerReceiptId` | int not null FK → CustomerReceipts(Id) | |
| `SalesInvoiceId` | int not null FK → SalesInvoices(Id) | |
| `AppliedAmount` | decimal(18,2) not null | |
| **Notes** | Optional — only created when user explicitly distributes payment to specific invoices | |

**Design Notes:**
- NO CustomerPayments table (replaced by CustomerReceipts)
- SalesReturnLines link to SalesInvoiceLineId (not ProductId directly)

---

## Module 7: Purchases (المشتريات)

### 7.1 PurchaseInvoices
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `InvoiceNo` | int not null | user-facing number, UNIQUE per table |
| `InvoiceDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `SupplierId` | int not null FK → Suppliers(Id) | |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `PaymentType` | tinyint not null | 1=Cash, 2=Credit, 3=Mixed |
| `CashBoxId` | int null FK → CashBoxes(Id) | |
| `TaxId` | smallint null FK → Taxes(Id) |
| `SubTotal` | decimal(18,2) not null | |
| `DiscountAmount` | decimal(18,2) not null default 0 | خصم الفاتورة — مبلغ ثابت فقط |
| `TaxAmount` | decimal(18,2) not null default 0 | |
| `OtherCharges` | decimal(18,2) not null default 0 | additional costs (transport, customs) |
| `NetTotal` | decimal(18,2) not null | SubTotal - Discount + Tax + OtherCharges |
| `PaidAmount` | decimal(18,2) not null default 0 | |
| `RemainingAmount` | decimal(18,2) not null default 0 | NetTotal - PaidAmount |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |
| **UK** | `UNIQUE(InvoiceNo)` | |

### 7.2 PurchaseInvoiceLines
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `PurchaseInvoiceId` | int not null FK → PurchaseInvoices(Id) | |
| `ProductId` | int not null FK → Products(Id) | |
| `ProductUnitId` | int not null FK → ProductUnits(Id) | |
| `Quantity` | decimal(18,3) not null | |
| `UnitPrice` | decimal(18,2) not null | purchase unit cost |
| `LineTotal` | decimal(18,2) not null | Quantity x UnitPrice |

### 7.3 PurchaseReturns
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `ReturnNo` | int not null | unique |
| `ReturnDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `PurchaseInvoiceId` | int null FK → PurchaseInvoices(Id) | null for standalone returns (RULE-487) |
| `SupplierId` | int not null FK → Suppliers(Id) | |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `TotalAmount` | decimal(18,2) not null | |
| `ReturnedDiscountAmount` | decimal(18,2) not null default 0 | proportional discount from original invoice |
| `ReturnedTaxAmount` | decimal(18,2) not null default 0 | proportional tax from original invoice |
| `ReturnedChargeAmount` | decimal(18,2) not null default 0 | proportional other charges from original invoice |
| `TaxId` | smallint null FK → Taxes(Id) | tax rate used from original invoice |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |

### 7.4 PurchaseReturnLines
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `PurchaseReturnId` | int not null FK → PurchaseReturns(Id) | |
| `PurchaseInvoiceLineId` | int null FK → PurchaseInvoiceLines(Id) | null for standalone returns |
| `Quantity` | decimal(18,3) not null | |
| `Amount` | decimal(18,2) not null | |

### 7.5 SupplierPayments
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `PaymentNo` | int not null | unique |
| `PaymentDate` | date not null | |
| `BranchId` | smallint not null FK → Branches(Id) | default=1 |
| `SupplierId` | int not null FK → Suppliers(Id) | |
| `CashBoxId` | int not null FK → CashBoxes(Id) | |
| `Amount` | decimal(18,2) not null | |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft, 2=Posted, 3=Cancelled |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | set when Status=2 |
| `CancelledAt` | datetime2 null | set when Status=3 |

### 7.6 SupplierPaymentApplications
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `SupplierPaymentId` | int not null FK → SupplierPayments(Id) | |
| `PurchaseInvoiceId` | int not null FK → PurchaseInvoices(Id) | |
| `AppliedAmount` | decimal(18,2) not null | |
| **Notes** | Optional — only created when user explicitly distributes payment to specific invoices | |

**Design Notes:**
- NO PurchaseOrders table (removed from V1)
- NO AdditionalFees/AdditionalFeeAllocations (OtherCharges on invoice handles this)
- NO per-line discount on purchase lines (discounts at invoice header level only)
- PurchaseReturnLines link to PurchaseInvoiceLineId
- On posting: creates InventoryBatch with BatchNo, QuantityReceived, QuantityRemaining, UnitCost

---

## Module 8: Infrastructure & Support (البنية التحتية والدعم)

### 8.1 AuditLogs
| Column | Type | Notes |
|--------|------|-------|
| `Id` | **bigint** PK | high-volume logging |
| `UserId` | int null FK → Users(Id) | performing user |
| `Action` | nvarchar(100) not null | e.g., "CreateCustomer", "CancelInvoice" |
| `EntityName` | nvarchar(100) null | e.g., "SalesInvoice", "Product" |
| `EntityId` | nvarchar(50) null | entity identifier |
| `Details` | nvarchar(max) null | JSON payload |
| `IpAddress` | nvarchar(50) null | client IP |
| `CreatedAt` | datetime2 not null | |
| **Indexes** | `(UserId, CreatedAt DESC)`, `(EntityName, EntityId)`, `(CreatedAt DESC)` | |

### 8.2 SystemLogs
| Column | Type | Notes |
|--------|------|-------|
| `Id` | **bigint** PK | high-volume logging |
| `Level` | tinyint not null | 1=Info, 2=Warning, 3=Error, 4=Critical |
| `Source` | nvarchar(100) null | component name |
| `Message` | nvarchar(max) not null | |
| `Exception` | nvarchar(max) null | serialized exception |
| `CreatedAt` | datetime2 not null | |
| **Index** | `(Level, CreatedAt DESC)` | for error monitoring |

---

# 4) Enums Reference

| Enum | Values |
|------|--------|
| `InvoiceStatus` | Draft=1, Posted=2, Cancelled=3 |
| `PaymentType` | Cash=1, Credit=2, Mixed=3 |
| `AccountNature` | Asset=1, Liability=2, Equity=3, Revenue=4, Expense=5 |
| `TaxType` | Standard=1, ZeroRated=2, Exempt=3 |
| `JournalEntryType` | Manual=1, Sales=2, Purchase=3, Receipt=4, Payment=5, Inventory=6, Adjustment=7 |
| `InventoryTransactionType` | Purchase=1, PurchaseReturn=2, Sale=3, SaleReturn=4, TransferOut=5, TransferIn=6, Count=7, Adjustment=8, Damage=9, OpeningBalance=10, InternalIssue=11, InternalReceipt=12 |
| `InventoryReferenceType` | PurchaseInvoice=1, SalesInvoice=2, PurchaseReturn=3, SalesReturn=4, Transfer=5, Count=6, Adjustment=7 |
| `AdjustmentType` | Opening=1, Increase=2, Shortage=3, Damage=4 |

| `PaymentMethod` | Cash=1, Cheque=2, BankTransfer=3, CreditCard=4 |
| `SettingType` | String=1, Integer=2, Decimal=3, Boolean=4 |
| `NotificationType` | LowStock=1, ExpirySoon=2, CreditLimitExceeded=3, System=4, Reminder=5 |
| `LogLevel` | Info=1, Warning=2, Error=3, Critical=4 |

---

# 5) Table Count Summary

## ✅ MVP Active Tables (~35)

| Module | Count | Tables (Active in MVP) |
|--------|-------|------------------------|
| 1. Core & Security | 3 | Customers, Suppliers, Users, Roles |
| 2. Organization & Settings | 4 | Warehouses🔒, CompanySettings, SystemSettings, DocumentSequences, Branches🔒 |
| 3. Products | 4 | ProductCategories, Products, Units, ProductUnits |
| 4. Accounting | 8 | Accounts, CashBoxes🔒, JournalEntries, JournalEntryLines, ReceiptVouchers, PaymentVouchers, Expenses, SystemAccountMappings |
| 5. Inventory | 6 | WarehouseStocks, InventoryBatches, InventoryTransactions, InventoryTransactionLines, InventoryAdjustments, InventoryAdjustmentLines |
| 6. Sales | 6 | SalesInvoices, SalesInvoiceLines, SalesReturns, SalesReturnLines, CustomerReceipts, CustomerReceiptApplications |
| 7. Purchases | 6 | PurchaseInvoices, PurchaseInvoiceLines, PurchaseReturns, PurchaseReturnLines, SupplierPayments, SupplierPaymentApplications |
| 8. Infrastructure | 2 | AuditLogs, SystemLogs |
| **MVP Total** | **~35** | 🔒 = hardcoded single instance in MVP |

## ⛔ V2 Deferred Tables (~18)

| Table | Module | Reason Deferred |
|-------|--------|-----------------|
| UserSessions | Core | Single user — no session tracking |
| FiscalYears | Settings | Not needed for MVP |
| Notifications | Settings | Single user — no multi-user notifications |
| Attachments | Settings | Not essential for MVP |
| Taxes | Settings | Management UI hidden — single seeded default tax used |
| AccountCategories | Accounting | Unnecessary complexity |
| Banks | Accounting | Single cash box covers all needs |
| TreasuryTransfers | Accounting | Single cash box — no transfers |
| CostCenters | Accounting | Not yet in EF — planned |
| VoucherInvoiceAllocations | Accounting | Not yet in EF — planned |
| FiscalPeriods | Accounting | Not yet in EF — planned |
| InventoryCounts | Inventory | Manual counting sufficient for MVP |
| InventoryCountLines | Inventory | Deferred with InventoryCounts |
| WarehouseTransfers | Inventory | Single warehouse — no transfers |
| WarehouseTransferLines | Inventory | Deferred with WarehouseTransfers |
| Currencies | V2 | Multi-currency not needed |
| CurrencyRates | V2 | Multi-currency not needed |
| SalesQuotations | V2 | Quotations deferred |
| SalesQuotationItems | V2 | Deferred with SalesQuotations |
| **V2 Total** | **~18** | |

---

# 6) Key Financial Formulas

## Sales Invoice
```
LineTotal = (Quantity × UnitPrice) - DiscountAmount
SubTotal  = SUM(LineTotal)
NetTotal  = SubTotal - DiscountAmount + TaxAmount + OtherCharges
RemainingAmount = NetTotal - PaidAmount
Constraint: PaidAmount <= NetTotal
```

## Purchase Invoice
```
LineTotal = Quantity x UnitPrice
SubTotal  = SUM(LineTotal)
NetTotal  = SubTotal - DiscountAmount + TaxAmount + OtherCharges
RemainingAmount = NetTotal - PaidAmount
Constraint: PaidAmount <= NetTotal
```

## FIFO Costing (Default Method)
```
-On Sale: consume from earliest batch first (oldest QuantityRemaining > 0)
-On Purchase Return: debit from the original batch's QuantityRemaining
-On Expiry/FEFO: consume nearest expiry date first (when TrackExpiry = true)
```

## Journal Entry Balance
```
Per Line: (Debit > 0 AND Credit = 0) OR (Credit > 0 AND Debit = 0) OR (Debit = 0 AND Credit = 0)
Per Entry: SUM(Debit) = SUM(Credit)
```

---

# 7) Soft Delete Strategy

- **Soft delete**: `IsActive = false` on all ActivatableEntity inheritors
- **Filter**: Global EF Core query filter `.HasQueryFilter(x => x.IsActive)`
- **Live balances**: WarehouseStocks has NO IsActive (auditable entity only)
- **Invoices**: `Status = Cancelled` — NEVER hard delete invoiced transactions
- **Users**: NEVER hard-delete — soft delete via `IsActive = false`
- **Document entities**: use Status (Draft→Posted→Cancelled), not IsActive

---

# 8) FK Rules

- **ALL foreign keys**: `DeleteBehavior.Restrict` — NO cascade delete anywhere
- **Self-referencing FKs**: Accounts.ParentId
- **Soft-delete reference safety**: Filtered unique indexes include `AND [IsActive] = 1`

---

# 9) V2 Deferred Entities (متأخر إلى الإصدار 2)

The following entities are **NOT in V1**. They are deferred to V2 for scope management. V1 codebase contains NO entity definitions, EF configurations, services, controllers, or Desktop UI for these tables.

---

> **Note**: Branches table has been moved to V1 (see section 2.9). All other entities remain deferred to V2.

---

## V2.2 Departments (الأقسام)
| Column | Type | Notes |
|--------|------|-------|
| `Id` | smallint PK | |
| `Name` | nvarchar(150) not null | |
| `Description` | nvarchar(300) null | |
| `BranchId` | smallint null FK → Branches(Id) | null = company-wide |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**Purpose**: Organizational structure. Groups employees by function (Sales, Warehouse, Accounting, HR).

---



## V2.4 Currencies (العملات)
| Column | Type | Notes |
|--------|------|-------|
| `Id` | smallint PK | |
| `Code` | char(3) not null | ISO 4217 (e.g., "SAR", "USD", "YER") |
| `NameAr` | nvarchar(100) not null | Arabic name |
| `NameEn` | nvarchar(100) not null | English name |
| `Symbol` | nvarchar(10) null | e.g., "﷼", "$" |
| `FractionName` | nvarchar(50) null | e.g., "فلس" for YER, "Cent" for USD |
| `DecimalPlaces` | tinyint not null default 2 | display precision |
| `IsBaseCurrency` | bit not null default 0 | unique filtered `[IsBaseCurrency]=1 AND [IsActive]=1` |
| `IsSystem` | bit not null default 0 | system currencies (SAR, USD) protected from deletion |
| `ExchangeRate` | decimal(18,6) not null default 1 | rate to base currency (base = 1.0) |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**Purpose**: Multi-currency support. V1 is single-currency (SAR). V2 adds currency conversion, multi-currency invoices, and exchange rate management.

**V2 entities that reference CurrencyId**:
- `SalesInvoices.CurrencyId` (FK) + `ExchangeRate`
- `PurchaseInvoices.CurrencyId` (FK) + `ExchangeRate`
- `CustomerReceipts.CurrencyId` (FK) + `ExchangeRate`
- `SupplierPayments.CurrencyId` (FK) + `ExchangeRate`
- `ProductPrices.CurrencyId` (FK) — multi-currency pricing

---

## V2.5 CurrencyRates ( Historical Exchange Rates)
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `CurrencyId` | smallint not null FK → Currencies(Id) | |
| `RateToBase` | decimal(18,6) not null | rate at date |
| `EffectiveDate` | date not null | |
| `CreatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |

**Purpose**: Exchange rate history. V2 records daily rates for audit trail. V1 uses fixed rate from SystemSettings.

---

## V2.6 CustomerContacts (جهات اتصال العملاء)
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `CustomerId` | int not null FK → Customers(Id) | |
| `Name` | nvarchar(150) not null | |
| `Phone` | nvarchar(30) null | |
| `Email` | nvarchar(100) null | |
| `Position` | nvarchar(100) null | job title / role at customer |
| `Notes` | nvarchar(300) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**Purpose**: Multiple contacts per customer (e.g., purchasing manager, accounts payable, warehouse receiver). V1 has single Phone/Email on Customer.

---

## V2.7 SupplierContacts (جهات اتصال الموردين)
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `SupplierId` | int not null FK → Suppliers(Id) | |
| `Name` | nvarchar(150) not null | |
| `Phone` | nvarchar(30) null | |
| `Email` | nvarchar(100) null | |
| `Position` | nvarchar(100) null | job title / role at supplier |
| `Notes` | nvarchar(300) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**Purpose**: Multiple contacts per supplier. V1 has single Phone/Email on Supplier.

---

## V2.8 SalesQuotations (عروض الأسعار)
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `QuotationNo` | int not null | user-facing number, UNIQUE |
| `QuotationDate` | date not null | |
| `ValidUnti` | date null | optional expiry date |
| `CustomerId` | int not null FK → Customers(Id) | |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `PaymentType` | tinyint not null | 1=Cash, 2=Credit |
| `SubTotal` | decimal(18,2) not null | SUM(LineTotal) |
| `DiscountAmount` | decimal(18,2) not null | header-level discount |
| `TaxAmount` | decimal(18,2) not null | |
| `TotalAmount` | decimal(18,2) not null | SubTotal - Discount + Tax |
| `Notes` | nvarchar(500) null | |
| `TermsAndConditions` | nvarchar(2000) null | |
| `Status` | tinyint not null | 1=Draft, 2=Sent, 3=Accepted, 4=Converted, 5=Rejected |
| `ConvertedToInvoiceId` | int null FK → SalesInvoices(Id) | set when converted |
| `RejectionReason` | nvarchar(1000) null | |
| `CreatedByUserId` | int null FK | |
| `UpdatedByUserId` | int null FK | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **UK** | `UNIQUE(QuotationNo)` | |

---

## V2.9 SalesQuotationItems (بنود عروض الأسعار)
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | |
| `SalesQuotationId` | int not null FK → SalesQuotations(Id) | |
| `ProductId` | int not null FK → Products(Id) | |
| `ProductUnitId` | int not null FK → ProductUnits(Id) | |
| `Quantity` | decimal(18,3) not null | |
| `UnitPrice` | decimal(18,2) not null | |
| `DiscountAmount` | decimal(18,2) not null default 0 | line-level discount |
| `LineTotal` | decimal(18,2) not null | (Qty × UnitPrice) − Discount |
| `Notes` | nvarchar(500) null | |

**Design Notes:**
- NO stock or accounting impact until quotation is converted to SalesInvoice
- QuotationItems DO support per-line DiscountAmount (unlike SalesInvoiceLines which only have header discount in V1)
- 5-state lifecycle: Draft → Sent → Accepted → Converted / Rejected

---

**V2 Scope Summary**:

| Entity | V2 Purpose | V1 Impact |
|--------|-----------|-----------|
| Departments | Organizational structure | Not needed in V1 |
| Currencies | Multi-currency invoices | V1 uses single currency (YER) |
| CurrencyRates | Exchange rate history | V1 uses fixed rate |
| CustomerContacts | Multiple contacts per customer | V1 has single contact fields |
| SupplierContacts | Multiple contacts per supplier | V1 has single contact fields |
| SalesQuotations | Quotation → Invoice conversion | Deferred to V2 |
| SalesQuotationItems | Quotation line items | Deferred to V2 |

**Total V1 Tables: 53 | Total V2 Tables: 8 | Combined: 61**

