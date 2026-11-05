# V2 Archive (removed from MVP)

The following V2-deferred features were removed from the MVP codebase. Their full code (entities, EF configs, migrations, BLL services, DTOs, Desktop Direct/Mock/forms) is preserved in git history.

## Recover any removed file
git checkout c135e5b -- <path>  # or e6d8323^ for the earlier V2-backend removal

## Removed V2 modules
- Banks, Taxes, FiscalYears, AccountCategories, Notifications, Attachments, UserSessions
- InventoryCounts, WarehouseTransfers
- Phantom modules: Currency, CostCenter, PettyCash, PurchaseOrder, Quotation, TreasuryTransfer, CustomerPayment
- Roles & Permissions (Role entity, IRoleService, frmRoleMaster) and the permission bitmask system (SalesPermissions/InventoryPermissions/AccountingPermissions/SystemPermissions enums, PermissionService, PermissionHelper, SessionManager.Has*Permission) — single admin user only
- JWT auth (JwtTokenGenerator, IJwtTokenGenerator, JwtSettings) — desktop 2-tier, no HTTP tokens
- Update Check (frmUpdateCheck + UpdaterService/GitHubUpdaterService/IUpdaterService/UpdateManifest) — removed
- HealthCheck (HealthCheckDto, DatabaseHealthCheck) — API-era leftover
- BillOfMaterials + AssemblyService — deferred
- CustomerContact + SupplierContact (entities, configs, services, DTOs) — deferred
- MinStockAlert (MinStockAlertService, MinStockAlertWorker) — notifications V2
- Hidden V2 Desktop UI: accounting screens (chart of accounts, journal entries, receipt/payment vouchers, system account mappings, cost centers), cash vouchers, smart sales input methods (frmSales*), POS helpers (held orders, receipt print, discount, pickers), audit/system logs, document sequences, old product price/unit list screens
- Orphaned Desktop wrappers: I*ApiService/Direct*Service/Mock*ApiService for the above (SystemAccountMapping, AuditLog, Logs, DocumentSequence, UserReport, ProductPrice, CompanySettings, ReceiptVoucher, PaymentVoucher)

## Removed settings screens (simplified to Backup/Restore only)
- frmSettings (general settings), frmCompanySettings (company settings), frmUpdateCheck (update check)

Reference: docs/database-schema.md (V2 Deferred Tables section)