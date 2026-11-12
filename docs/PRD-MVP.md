# Product Requirements Document (PRD) 
# Sales Management System (v5.0.0 — WinForms N-Layer)

## 1. Executive Summary
A local desktop sales management system for small retail shops. Built on a simple N-Layer 2-Tier architecture: WinForms Desktop → Business → DataAccess → SQL Server (all in-process, no HTTP/API layer).

## 2. Functional Scope
- **User Authentication**: Local login for a single admin account (full access). Roles/permissions (RBAC) are deferred to V2 per `AGENTS.md §2.29`.
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
- **Roles & Visibility**: Not implemented in MVP — single admin account, every screen is reachable. RBAC is deferred to V2 (`AGENTS.md §2.29`).

## 4. Technical Non-Functional Requirements
- **Architecture**: Simple N-Layer 2-Tier — every screen calls the Business layer in-process (`Desktop → Business → DataAccess → SQL Server`) through `I*DesktopService` / `Direct*Service` wrappers. No HTTP, no API layer, no MVVM, no Clean Architecture. The Desktop never touches `DbContext`/EF/SQL directly.
- **Data Integrity**: `decimal(18,2)` for money, `decimal(18,3)` for quantity. `DeleteBehavior.Restrict` on all FKs.
- **Security**: Soft-delete only. Passwords hashed with BCrypt (WorkFactor 12). Connection strings in environment variables.
- **Logging**: Serilog for all critical operations (exceptions, invoice changes, stock changes, logins).

*Note: For specific architectural rules, refer to `AGENTS.md`. For schema details, refer to `database-schema.md`.*
