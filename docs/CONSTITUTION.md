# Project Constitution — Sales Management System
# Platform: .NET 10 | N-Layer Architecture | WinForms

## 1. Single Source of Truth
The master rules for architecture, UI patterns, and API services are defined in `AGENTS.md`. This Constitution file defines strictly the unchangeable core financial and transactional rules.

## 2. Core Financial Rules
1. **Monetary Values**: MUST use `decimal(18,2)`. NEVER `float`, `double`, or `money`.
2. **Quantities**: MUST use `decimal(18,3)`.
3. **Formulas (Domain Layer Only)**:
   - LineTotal = (Quantity * UnitPrice) - DiscountAmount
   - SubTotal = Sum(LineTotal)
   - NetTotal = SubTotal - InvoiceDiscount + TaxAmount + OtherCharges
   - RemainingAmount = NetTotal - PaidAmount
4. **Validation**: `PaidAmount <= NetTotal` is enforced at both Domain and DB CHECK constraint levels.

## 3. Core Transaction Rules (IUnitOfWork)
Every multi-table operation MUST use `IUnitOfWork.ExecuteTransactionAsync()`.
1. Validate all preconditions (stock, balances) BEFORE the transaction delegate.
2. Inside execution strategy: BeginTransactionAsync.
3. Save entity to get ID.
4. Execute side-effects (deduct stock, create inventory transactions, update balances).
5. CommitAsync. On ANY exception, RollbackAsync. NO partial commits.

## 4. Invoice & Stock Integrity
1. **Invoice States**: Draft(1) -> Posted(2) -> Cancelled(3).
2. NO hard deletes for invoices. NO editing a Posted invoice.
3. Cancellation MUST reverse all stock and balance effects.
4. **Perpetual Inventory**: All inventory costs go directly to the Inventory Asset account. NO intermediate "Purchases" clearing account.
5. **Stock Validation**: Validated before opening a transaction. `WarehouseStocks.Quantity` has DB CHECK constraint `>= 0`. Every stock change creates an `InventoryTransaction`.

## 5. Security Integrity
1. Passwords: Hash with BCrypt (WorkFactor 12).
2. Connection Strings: Use environment variables or encrypted configs.
3. Hard Deletes: Strictly FORBIDDEN. Use soft-delete `IsActive = false`.
4. API: All endpoints require `[Authorize]` except login.

*For all UI and API guidelines, refer to `AGENTS.md`.*
