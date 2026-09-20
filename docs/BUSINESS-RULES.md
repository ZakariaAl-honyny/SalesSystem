# Business Rules — Sales Management System

> **Version**: v5.0.0 (WinForms N-Layer)
> **Source**: AGENTS.md, CONSTITUTION.md

---

## 1. Money & Quantity
| Rule | Description |
|------|-------------|
| BR-001 | ALL money fields = `decimal(18,2)` — NEVER `float`/`double`/`int` |
| BR-002 | ALL quantities = `decimal(18,3)` — NEVER `int` |

## 2. Account Auto-Creation (Chart of Accounts)
| Rule | Description |
|------|-------------|
| BR-010 | **CashBox** without `AccountId` → auto-create L4 sub-account under `"1101"` (Cash & Cash Equivalents). |
| BR-011 | **Bank** without `AccountId` → auto-create L4 sub-account under `"1102"` (Bank Accounts). |
| BR-012 | **Customer** without `AccountId` → auto-create L4 sub-account under `"1103"` (Accounts Receivable). |
| BR-013 | **Supplier** without `AccountId` → auto-create L4 sub-account under `"1320"` (Accounts Payable). |
| BR-015 | Auto-created accounts are Level 4, Asset or Liability, `AllowTransactions = true`, `IsSystemAccount = false`. |

## 3. Flexible Input (Sales & Purchase Lines)
| Rule | Description |
|------|-------------|
| BR-020 | User enters ANY TWO of (Quantity, Price, Total) — system calculates the third. |

## 4. Invoice Lifecycle
| Rule | Description |
|------|-------------|
| BR-030 | States: Draft (1) → Posted (2) → Cancelled (3) — terminal at Cancelled. |
| BR-031 | Stock/balance changes ONLY when Status = Posted. |
| BR-032 | No editing a Posted invoice — cancel + create new. |
| BR-033 | Cancellation MUST reverse ALL stock and balance. |

## 5. Sales Price Enforcement
| Rule | Description |
|------|-------------|
| BR-050 | If `PreventBelowRetailPrice` is ON → reject sale if `UnitPrice < registered price` in `ProductPrices`. |
| BR-051 | If `AllowBelowCostSale` is OFF → reject sale if `UnitPrice < AvgCost`. |

## 6. Accounting Integration
| Rule | Description |
|------|-------------|
| BR-080 | Every money-affecting operation creates balanced double-entry journal entries. |
| BR-081 | Use `Customer.AccountId` / `Supplier.AccountId` — NOT fixed AR/AP from SystemAccountMappings. |
| BR-084 | COGS uses `AverageCost` (weighted average) — NOT `PurchaseCost`. |

## 7. WinForms UI Rules
| Rule | Description |
|------|-------------|
| BR-140 | Validate on click with `MessageBox.Show` (Arabic). |
| BR-142 | Editors open modally via `ShowDialog()`. |
| BR-143 | No MVVM binding or `INotifyDataErrorInfo`. Extract data manually from controls. |

## 8. General Database Constraints
| Rule | Description |
|------|-------------|
| BR-130 | ALL FKs use `DeleteBehavior.Restrict` — no cascade. |
| BR-131 | `WarehouseStocks.Quantity` has `CHECK (Quantity >= 0)`. |
