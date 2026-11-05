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

## Removed settings screens (simplified to Backup/Restore only)
- frmSettings (general settings), frmCompanySettings (company settings), frmUpdateCheck (update check)

Reference: docs/database-schema.md (V2 Deferred Tables section)