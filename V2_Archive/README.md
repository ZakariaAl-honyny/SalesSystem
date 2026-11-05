# V2 Archive (removed from MVP)

The following V2-deferred features were removed from the MVP codebase. Their full code (entities, EF configs, migrations, BLL services, DTOs, Desktop Direct/Mock/forms) is preserved in git history.

## Recover any removed file
git checkout e6d8323^ -- <path>

## Removed V2 modules
- Banks, Taxes, FiscalYears, AccountCategories, Notifications, Attachments, UserSessions
- InventoryCounts, WarehouseTransfers
- Phantom modules: Currency, CostCenter, PettyCash, PurchaseOrder, Quotation, TreasuryTransfer, CustomerPayment

## Removed settings screens (simplified to Backup/Restore + Update Check)
- frmSettings (general settings), frmCompanySettings (company settings), frmUpdateCheck (update check)

Reference: docs/database-schema.md (V2 Deferred Tables section)