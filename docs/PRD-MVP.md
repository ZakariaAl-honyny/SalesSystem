# Product Requirements Document (PRD) 
# Sales Management System (v5.0.0 — WinForms N-Layer)

## 1. Executive Summary
A local desktop sales management system for small retail shops. Built on N-Layer Architecture with WinForms Desktop + ASP.NET Core 10 API + SQL Server. 

## 2. Functional Scope
- **User Authentication**: JWT-based login with 9 DB-driven roles (Admin, Manager, Accountant, Treasurer, Cashier, Warehouse Supervisor, Sales Employee, Observer, Branch Manager).
- **Product Management**: Independent Units table (Base vs Derived), Multi-Currency Pricing with date ranges. Inventory batches are tracked FIFO/FEFO via `InventoryBatches` for stock allocation; COGS is computed using weighted-average cost.
- **Warehouse Management**: Multi-warehouse with `WarehouseTransfers`. Strict non-negative stock validation.
- **Purchases**: Purchase invoices (Cash/Credit/Mixed). Auto-generates InventoryBatches. FEFO tracking.
- **Sales**: Sales invoices (Cash/Credit/Mixed) with real-time stock validation and multi-currency support.
- **Returns**: Sales and Purchase returns linked to original invoices or standalone.
- **Accounting Engine**: Auto-journal entries for all financial operations. Multi-currency Chart of Accounts (60 seeded accounts).
- **Cash & Banks**: CashBox and Bank management linked directly to the Chart of Accounts.
- **Printing**: A4 PDF via System.Drawing.Printing + 80mm thermal receipt printing.

## 3. Core Entities & Rules
- **Invoice Lifecycle**: Draft (1) → Posted (2) → Cancelled (3). No hard deletes. Stock/Balance changes only on Posted.
- **Inventory Integration**: Perpetual inventory. All inventory costs go directly to the Inventory Asset account. No intermediate "Purchases" clearing account.
- **Multi-Currency**: Transactions support `CurrencyId` and track exchange rates. Base currency is immutable.
- **Roles & Visibility**: Access to API controllers and WinForms screens is governed by the user's role.

## 4. Technical Non-Functional Requirements
- **Architecture**: N-Layer hybrid: CRUD/data screens call the API via `HttpClient`; authentication and reports call the BLL in-process. The Desktop never touches `DbContext`/EF/SQL directly.
- **Data Integrity**: `decimal(18,2)` for money, `decimal(18,3)` for quantity. `DeleteBehavior.Restrict` on all FKs.
- **Security**: Soft-delete only. Passwords hashed with BCrypt (WorkFactor 12). Connection strings in environment variables.
- **Logging**: Serilog for all critical operations (exceptions, invoice changes, stock changes, logins).

*Note: For specific architectural rules, refer to `AGENTS.md`. For schema details, refer to `database-schema.md`.*
