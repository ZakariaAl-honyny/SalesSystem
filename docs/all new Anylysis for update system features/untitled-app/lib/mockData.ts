/**
 * Central Mock Data Entry Point (Re-exports from domain-specific modules in /lib/mock)
 *
 * This modular design follows "Divide and Conquer" (فرق تسد) to keep each domain
 * isolated, maintainable, performant, and clean:
 * - /lib/mock/currencies.ts
 * - /lib/mock/products.ts
 * - /lib/mock/suppliers.ts
 * - /lib/mock/customers.ts
 * - /lib/mock/accounts.ts
 * - /lib/mock/journalEntries.ts
 * - /lib/mock/invoices.ts
 * - /lib/mock/vouchers.ts
 * - /lib/mock/shifts.ts
 * - /lib/mock/security.ts
 * - /lib/mock/warehouses.ts
 * - /lib/mock/inventoryUtils.ts
 */

export * from './mock';
