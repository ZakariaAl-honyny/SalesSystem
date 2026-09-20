<h1 align="center">🧾 Sales Management System</h1>

<p align="center">
  <strong>A comprehensive sales management platform for small-to-medium retail businesses</strong><br/>
  <em>v5.0.0 — N-Layer WinForms Edition</em>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10%20LTS-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10"/>
  <img src="https://img.shields.io/badge/WinForms-Desktop-0078D4?style=for-the-badge&logo=windows&logoColor=white" alt="WinForms"/>
  <img src="https://img.shields.io/badge/SQL%20Server-2019+-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server"/>
  <img src="https://img.shields.io/badge/Architecture-N--Layer-2ECC71?style=for-the-badge" alt="N-Layer Architecture"/>
  <img src="https://img.shields.io/badge/API-ASP.NET%20Core%2010-512BD4?style=for-the-badge" alt="ASP.NET Core"/>
</p>

---

## 📋 Overview

A **comprehensive sales management platform** built with an N-Layer Architecture, designed for small-to-medium retail businesses. The current version delivers a **WinForms Desktop client** backed by an **ASP.NET Core Web API**, handling sales, purchases, inventory, returns, and financial tracking with full Arabic support.

The API-first architecture ensures that the backend can support future web and mobile clients without any changes to the core business logic.

---

## 🏗️ Architecture (N-Layer)

The system follows a strict 8-project N-Layer solution structure:

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

### 🔄 Data Flow

```text
# CRUD / data screens (API path)
Desktop (WinForms) → (HttpClient) → API Controllers → Business Services → DataAccess (EF Core) → SQL Server

# Authentication + Reports (in-process BLL path)
Desktop (WinForms) → Business Services → DataAccess (EF Core) → SQL Server
```

> **Key Principle:** The Desktop app **never** connects to the database directly (no `DbContext`/EF/SQL in the Desktop project). CRUD and data screens call the Web API via `HttpClient`, while authentication and reports call the Business Logic layer in-process. Both paths strictly adhere to the `Result<T>` pattern.

---

## 🧪 Mock Mode (UI-first testing)

Mock mode is **ON by default** (`UseMockData: true` in `appsettings.json`), letting you explore and build the UI without a running API or database.

- No API or database required — services resolve to in-memory `Mock*` implementations under `Services/Mock/`.
- Login accepts any credentials.
- The main window title bar shows `وضع التجربة (Mock)`.
- Set `UseMockData: false` in `appsettings.json` to switch to the real API + database.

---

## 🚀 Core Features

- 🏪 **Purpose-built for retail** — Sales invoices, purchase orders, returns, and stock transfers.
- 💰 **Robust Financials** — Chart of Accounts, Journal Entries, Cash Boxes, and Bank tracking.
- 📦 **Inventory Management** — Multi-warehouse tracking, FIFO/FEFO costing, and automated low-stock alerts.
- 👥 **Role-Based Permissions** — 9 DB-driven roles with granular access control.
- 🖨️ **Print Integration** — A4 Invoice generation via QuestPDF and ESC/POS thermal receipt printing.
- 🛡️ **Data Integrity** — All monetary values use `decimal(18,2)`. All inventory quantities use `decimal(18,3)`.

---

## 📚 Documentation & Guidelines

All core architectural rules, coding standards, and AI Agent constraints are documented in the master rules file:

👉 **[AGENTS.md](AGENTS.md)** (Read this before contributing)

For detailed module rules and database schemas, refer to the `docs/` folder:
- `docs/CONSTITUTION.md` (Financial rules and constraints)
- `docs/PRD-MVP.md` (Product Requirements Document)
- `docs/database-schema.md` (Database Architecture)
- `docs/BUSINESS-RULES.md` (Domain logic)
- `docs/ui-screens.md` (WinForms guidelines)