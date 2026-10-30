using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalesSystem.Models.Accounting;
using SalesSystem.Models;
using SalesSystem.Common.Enums;
using SalesSystem.Common.Enums.Permissions;
using SalesSystem.DataAccess.Seeders;
using System.Linq;

namespace SalesSystem.DataAccess;

public static class DbSeeder
{
    public static async Task SeedAsync(SalesDbContext db, ILogger? logger = null)
    {
        // 1. Seed default branch (must be first — other entities reference BranchId=1)
        if (!await db.Branches.AnyAsync())
        {
            db.Branches.Add(Branch.Create("المركز الرئيسي", address: "المقر الرئيسي"));
            await db.SaveChangesAsync();
            logger?.LogInformation("Seeded default branch 'المركز الرئيسي'.");
        }

        if (!await db.Set<ProductCategory>().AnyAsync())
        {
            var generalCategory = ProductCategory.Create(branchId: 1, "عام", description: "التصنيف الافتراضي لجميع المنتجات", createdByUserId: 1);
            db.Set<ProductCategory>().Add(generalCategory);
            logger?.LogInformation("Seeded default product category 'عام'.");
        }

        await AccountingSeeder.SeedAsync(db, logger);

        if (!await db.Customers.AnyAsync())
        {
            var cashCustomerAccount = await db.Set<Account>()
                .FirstAsync(a => a.AccountCode == "11030001");
            db.Customers.Add(Customer.Create(
                branchId: 1,
                name: "عميل نقدي",
                accountId: cashCustomerAccount.Id,
                createdByUserId: null
            ));
            await db.SaveChangesAsync();
            logger?.LogInformation("Seeded default customer.");
        }

        if (!await db.Suppliers.AnyAsync())
        {
            var cashSupplierAccount = await db.Set<Account>()
                .FirstAsync(a => a.AccountCode == "21010001");
            db.Suppliers.Add(Supplier.Create(
                branchId: 1,
                name: "المورد الافتراضي في النظام",
                accountId: cashSupplierAccount.Id,
                createdByUserId: null
            ));
            await db.SaveChangesAsync();
            logger?.LogInformation("Seeded default supplier.");
        }

        if (!await db.SystemSettings.AnyAsync())
        {
            var settings = new List<SystemSetting>
            {
                SystemSetting.Create("AllowNegativeStock", "false", settingType: 4, category: "Inventory", displayName: "السماح بالمخزون السالب", description: "السماح بجعل كمية المخزون أقل من صفر"),
                SystemSetting.Create("EnableFefo", "false", settingType: 4, category: "Inventory", displayName: "استخدام FEFO", description: "استخدام طريقة الصادر أولاً حسب تاريخ الانتهاء"),
                SystemSetting.Create("StockAlertDays", "5", settingType: 2, category: "Inventory", displayName: "تحذير المخزون (أيام)", description: "عدد الأيام للتحذير قبل نفاد المخزون"),
                SystemSetting.Create("AutoPostInvoices", "true", settingType: 4, category: "Sales", displayName: "الترحيل التلقائي", description: "ترحيل فاتورة البيع مباشرة عند الحفظ"),
                SystemSetting.Create("AllowDrafts", "true", settingType: 4, category: "Sales", displayName: "السماح بالمسودات", description: "السماح بحفظ فاتورة البيع كمسودة"),
                SystemSetting.Create("ShowProfitInInvoice", "true", settingType: 4, category: "Sales", displayName: "إظهار الربح", description: "إظهار هامش الربح في شاشة البيع"),
                SystemSetting.Create("PreventBelowRetailPrice", "false", settingType: 4, category: "Sales", displayName: "منع البيع أقل من السعر", description: "منع البيع بسعر أقل من السعر الرسمي"),
                SystemSetting.Create("AllowBelowCostSale", "true", settingType: 4, category: "Sales", displayName: "البيع أقل من التكلفة", description: "السماح بالبيع بسعر أقل من التكلفة مع تحذير"),
                SystemSetting.Create("DefaultCashCustomerId", "1", settingType: 2, category: "Sales", displayName: "العميل النقدي", description: "العميل الافتراضي لمبيعات النقد"),
                SystemSetting.Create("PurchaseAutoPost", "true", settingType: 4, category: "Purchases", displayName: "ترحيل المشتريات تلقائياً", description: "ترحيل فاتورة الشراء مباشرة عند الحفظ"),
                SystemSetting.Create("DefaultCashSupplierId", "1", settingType: 2, category: "Purchases", displayName: "المورد النقدي", description: "المورد الافتراضي لمشتريات النقد"),

                SystemSetting.Create("AutoGenerateBarcode", "true", settingType: 4, category: "Barcode", displayName: "توليد باركود تلقائي", description: "توليد باركود تلقائي للمنتجات الجديدة"),
                SystemSetting.Create("AutoCreateJournalEntry", "true", settingType: 4, category: "Accounting", displayName: "إنشاء قيود محاسبية", description: "إنشاء قيد محاسبي عند ترحيل كل فاتورة"),

                SystemSetting.Create("PaperSize", "A4", settingType: 1, category: "Print", displayName: "حجم الورق", description: "حجم الورق الافتراضي للطباعة"),
                SystemSetting.Create("PrintCopies", "1", settingType: 2, category: "Print", displayName: "عدد النسخ", description: "عدد نسخ الطباعة الافتراضية"),
                SystemSetting.Create("ShowBalanceOnPrint", "true", settingType: 4, category: "Print", displayName: "إظهار الرصيد", description: "إظهار رصيد الحساب في الفاتورة المطبوعة"),
                SystemSetting.Create("PrintSignature", "false", settingType: 4, category: "Print", displayName: "طباعة التوقيع", description: "طباعة التوقيع في أسفل الفاتورة"),
                SystemSetting.Create("HideTaxInSales", "false", settingType: 4, category: "Sales", displayName: "إخفاء الضريبة في المبيعات", description: "إخفاء حقل الضريبة في شاشة فاتورة البيع"),
                SystemSetting.Create("ShowExpiryInInvoices", "false", settingType: 4, category: "Sales", displayName: "إظهار تاريخ الانتهاء", description: "إظهار تاريخ انتهاء الصلاحية في الفاتورة"),
                SystemSetting.Create("HideTaxInPurchases", "false", settingType: 4, category: "Purchases", displayName: "إخفاء الضريبة في المشتريات", description: "إخفاء حقل الضريبة في شاشة فاتورة الشراء"),
                SystemSetting.Create("ShowLogo", "true", settingType: 4, category: "Print", displayName: "إظهار الشعار", description: "إظهار شعار المتجر في الفواتير المطبوعة"),
                SystemSetting.Create("FooterNote", "", settingType: 1, category: "Print", displayName: "ملاحظة في التذييل", description: "نص يظهر في تذييل جميع الفواتير المطبوعة"),
                SystemSetting.Create("LowStockAlert", "true", settingType: 4, category: "Notifications", displayName: "تنبيه المخزون المنخفض", description: "تفعيل التنبيه عند انخفاض المخزون عن الحد الأدنى"),
                SystemSetting.Create("ExpiryAlert", "true", settingType: 4, category: "Notifications", displayName: "تنبيه تواريخ الانتهاء", description: "تفعيل التنبيه عند اقتراب تاريخ انتهاء المنتجات"),
                SystemSetting.Create("ExpiryAlertDays", "30", settingType: 2, category: "Notifications", displayName: "أيام تنبيه الانتهاء", description: "عدد الأيام قبل تاريخ الانتهاء لإرسال التنبيه"),
                SystemSetting.Create("CreditLimitAlert", "true", settingType: 4, category: "Notifications", displayName: "تنبيه الحد الائتماني", description: "تفعيل التنبيه عند تجاوز الحد الائتماني للعميل"),
                // ─── New Settings (v4.10.7+) ─────────────────────────────
                // Inventory
                SystemSetting.Create("DefaultWarehouse", "1", settingType: 2, category: "Inventory", displayName: "المخزن الافتراضي", description: "معرف المخزن الافتراضي للمعاملات"),
                // Sales
                SystemSetting.Create("AutoPrintAfterPosting", "false", settingType: 4, category: "Sales", displayName: "طباعة تلقائية بعد الترحيل", description: "طباعة الفاتورة تلقائياً بعد الترحيل"),
                SystemSetting.Create("DefaultSalesTax", "0", settingType: 1, category: "Sales", displayName: "الضريبة الافتراضية للمبيعات", description: "نسبة الضريبة الافتراضية لفواتير البيع"),
                // Purchases
                SystemSetting.Create("DefaultPurchaseTax", "0", settingType: 1, category: "Purchases", displayName: "الضريبة الافتراضية للمشتريات", description: "نسبة الضريبة الافتراضية لفواتير الشراء"),
                SystemSetting.Create("RequireBatchOnPurchase", "false", settingType: 4, category: "Purchases", displayName: "إلزام رقم التشغيلة عند الشراء", description: "إجبار إدخال رقم التشغيلة عند إنشاء فاتورة شراء"),
                SystemSetting.Create("RequireExpiryOnPurchase", "false", settingType: 4, category: "Purchases", displayName: "إلزام تاريخ الانتهاء عند الشراء", description: "إجبار إدخال تاريخ انتهاء الصلاحية عند إنشاء فاتورة شراء"),
                // Barcode
                SystemSetting.Create("AllowDuplicateBarcode", "false", settingType: 4, category: "Barcode", displayName: "السماح بالباركود المكرر", description: "السماح بوجود باركود مكرر لمنتجات مختلفة"),
                // General
                SystemSetting.Create("EnableAttachments", "true", settingType: 4, category: "General", displayName: "تفعيل المرفقات", description: "إظهار وإخفاء ميزة إرفاق الملفات في الفواتير"),
                // Security — رمز الدخول الرقمي (MVP) لشاشة تسجيل الدخول في الوضع الحقيقي
                SystemSetting.Create("AdminPin", "1234", settingType: 1, category: "Security", displayName: "رمز دخول المدير", description: "رمز الدخول الرقمي (PIN) الخاص بالمدير في شاشة تسجيل الدخول"),
                // Print
                SystemSetting.Create("PrintBarcode", "false", settingType: 4, category: "Print", displayName: "طباعة الباركود", description: "طباعة باركود المنتج على الفاتورة"),
                SystemSetting.Create("PrintQRCode", "false", settingType: 4, category: "Print", displayName: "طباعة رمز QR", description: "طباعة رمز QR على الفاتورة"),
                SystemSetting.Create("PrintCompanyAddress", "true", settingType: 4, category: "Print", displayName: "طباعة عنوان الشركة", description: "طباعة عنوان الشركة في الفواتير المطبوعة"),
                // Notifications
                SystemSetting.Create("EnableNotifications", "true", settingType: 4, category: "Notifications", displayName: "تفعيل التنبيهات", description: "التشغيل/إيقاف تشغيل نظام التنبيهات بالكامل"),
                // CashBox
                SystemSetting.Create("AllowNegativeCash", "false", settingType: 4, category: "CashBox", displayName: "السماح بالرصيد السالب للصندوق", description: "السماح بجعل رصيد الصندوق النقدي أقل من صفر"),
            };
            db.SystemSettings.AddRange(settings);
            logger?.LogInformation("Seeded {Count} SystemSettings key-value pairs.", settings.Count);
        }

        if (!await db.Set<Tax>().AnyAsync())
        {
            var taxes = new List<Tax>
            {
                Tax.Create("بدون ضريبة", "TAX-EXEMPT", 0m, taxType: 3, isDefault: true),
                Tax.Create("ضريبة القيمة المضافة 5%", "VAT-5", 5m),
                Tax.Create("ضريبة القيمة المضافة 15%", "VAT-15", 15m),
            };
            db.Set<Tax>().AddRange(taxes);
            logger?.LogInformation("Seeded {Count} tax records.", taxes.Count);
        }

        if (!await db.Set<SystemSetting>().AnyAsync(s => s.SettingKey == "Store.Name"))
        {
            var storeSettings = new List<SystemSetting>
            {
                SystemSetting.Create("Store.Name", "متجري", settingType: 1, category: "Store"),
                SystemSetting.Create("Store.Phone", "", settingType: 1, category: "Store"),
                SystemSetting.Create("Store.Address", "", settingType: 1, category: "Store"),
                SystemSetting.Create("Store.LogoPath", "", settingType: 1, category: "Store"),
                SystemSetting.Create("Store.Email", "", settingType: 1, category: "Store"),
                SystemSetting.Create("Store.TaxNumber", "", settingType: 1, category: "Store"),
                SystemSetting.Create("Store.EnableStockAlerts", "true", settingType: 4, category: "Store"),
                SystemSetting.Create("Store.AllowNegativeStock", "false", settingType: 4, category: "Store"),
                SystemSetting.Create("Store.SignaturePath", "", settingType: 1, category: "Store"),
            };
            db.Set<SystemSetting>().AddRange(storeSettings);
            logger?.LogInformation("Seeded Store settings into SystemSettings.");
        }

        if (!await db.Set<Role>().AnyAsync())
        {
            var adminRole = Role.Create("مدير النظام", "Administrator - كامل الصلاحيات",
                salesPermissions: (long)SalesPermissions.All,
                inventoryPermissions: (long)InventoryPermissions.All,
                accountingPermissions: (long)AccountingPermissions.All,
                systemPermissions: (long)SystemPermissions.All,
                isSystem: true);

            var managerRole = Role.Create("مدير", "Manager - صلاحيات إدارية",
                salesPermissions: (long)(SalesPermissions.All),
                inventoryPermissions: (long)(InventoryPermissions.All),
                accountingPermissions: (long)(AccountingPermissions.All),
                systemPermissions: (long)(SystemPermissions.ManageSettings | SystemPermissions.ViewAuditLogs | SystemPermissions.ManageBranches | SystemPermissions.ManageBackups | SystemPermissions.ManageFiscalYears | SystemPermissions.ManageTaxes),
                isSystem: true);

            var accountantRole = Role.Create("محاسب", "Accountant - صلاحيات محاسبية",
                salesPermissions: (long)(SalesPermissions.ViewInvoices | SalesPermissions.ViewReturns | SalesPermissions.ViewReceipts),
                inventoryPermissions: (long)(InventoryPermissions.ViewProducts | InventoryPermissions.ViewStock | InventoryPermissions.ViewBatches),
                accountingPermissions: (long)(AccountingPermissions.ViewJournalEntries | AccountingPermissions.CreateJournalEntries | AccountingPermissions.ViewAccounts | AccountingPermissions.ManageAccounts | AccountingPermissions.ViewCashBoxes | AccountingPermissions.ManageCashBoxes | AccountingPermissions.ViewExpenses | AccountingPermissions.ViewReceiptVouchers | AccountingPermissions.ViewPaymentVouchers | AccountingPermissions.ViewReports),
                systemPermissions: (long)(SystemPermissions.ViewAuditLogs),
                isSystem: true);

            var treasurerRole = Role.Create("أمين صندوق", "Treasurer - صلاحيات الصندوق",
                salesPermissions: (long)(SalesPermissions.ViewInvoices | SalesPermissions.ViewReceipts | SalesPermissions.CreateReceipts),
                inventoryPermissions: 0L,
                accountingPermissions: (long)(AccountingPermissions.ViewCashBoxes | AccountingPermissions.ViewReceiptVouchers | AccountingPermissions.CreateReceiptVouchers | AccountingPermissions.ViewPaymentVouchers),
                systemPermissions: 0L,
                isSystem: true);

            var cashierRole = Role.Create("كاشير", "Cashier - صلاحيات مبيعات محدودة",
                salesPermissions: (long)(SalesPermissions.ViewInvoices | SalesPermissions.CreateInvoices | SalesPermissions.CancelInvoices | SalesPermissions.PrintInvoices | SalesPermissions.ViewReturns | SalesPermissions.CreateReturns | SalesPermissions.ViewReceipts | SalesPermissions.CreateReceipts | SalesPermissions.ManageDiscounts),
                inventoryPermissions: (long)(InventoryPermissions.ViewProducts),
                accountingPermissions: (long)(AccountingPermissions.ViewCashBoxes),
                systemPermissions: 0L,
                isSystem: true);

            var warehouseRole = Role.Create("مشرف مخازن", "Warehouse Supervisor - صلاحيات المخازن",
                salesPermissions: (long)(SalesPermissions.ViewInvoices),
                inventoryPermissions: (long)(InventoryPermissions.ViewProducts | InventoryPermissions.CreateProducts | InventoryPermissions.EditProducts | InventoryPermissions.ViewStock | InventoryPermissions.AdjustStock | InventoryPermissions.TransferStock | InventoryPermissions.CountStock | InventoryPermissions.ViewBatches | InventoryPermissions.ManageCategories | InventoryPermissions.ManageUnits),
                accountingPermissions: 0L,
                systemPermissions: 0L,
                isSystem: true);

            var salesEmployeeRole = Role.Create("مندوب مبيعات", "Sales Employee - صلاحيات بيع محدودة",
                salesPermissions: (long)(SalesPermissions.ViewInvoices | SalesPermissions.CreateInvoices | SalesPermissions.ViewReturns | SalesPermissions.CreateReturns | SalesPermissions.ViewReceipts | SalesPermissions.CreateReceipts | SalesPermissions.ManageDiscounts | SalesPermissions.PrintInvoices),
                inventoryPermissions: (long)(InventoryPermissions.ViewProducts | InventoryPermissions.ViewStock),
                accountingPermissions: 0L,
                systemPermissions: 0L,
                isSystem: true);

            var observerRole = Role.Create("مراقب", "Observer - صلاحيات مشاهدة فقط",
                salesPermissions: (long)(SalesPermissions.ViewInvoices | SalesPermissions.ViewReturns | SalesPermissions.ViewReceipts),
                inventoryPermissions: (long)(InventoryPermissions.ViewProducts | InventoryPermissions.ViewStock),
                accountingPermissions: (long)(AccountingPermissions.ViewJournalEntries | AccountingPermissions.ViewAccounts | AccountingPermissions.ViewCashBoxes | AccountingPermissions.ViewExpenses | AccountingPermissions.ViewReports),
                systemPermissions: (long)(SystemPermissions.ViewAuditLogs),
                isSystem: true);

            var branchManagerRole = Role.Create("مدير فرع", "Branch Manager - صلاحيات محدودة بفرع محدد",
                salesPermissions: (long)(SalesPermissions.All & ~SalesPermissions.DeleteInvoices),
                inventoryPermissions: (long)(InventoryPermissions.ViewProducts | InventoryPermissions.CreateProducts | InventoryPermissions.EditProducts | InventoryPermissions.ViewStock | InventoryPermissions.AdjustStock | InventoryPermissions.TransferStock | InventoryPermissions.CountStock | InventoryPermissions.ViewBatches),
                accountingPermissions: (long)(AccountingPermissions.ViewJournalEntries | AccountingPermissions.ViewAccounts | AccountingPermissions.ViewCashBoxes | AccountingPermissions.ViewExpenses | AccountingPermissions.ViewReceiptVouchers | AccountingPermissions.ViewPaymentVouchers | AccountingPermissions.ViewReports),
                systemPermissions: (long)(SystemPermissions.ViewAuditLogs),
                isSystem: true);

            db.Set<Role>().AddRange(adminRole, managerRole, accountantRole, treasurerRole,
                cashierRole, warehouseRole, salesEmployeeRole, observerRole, branchManagerRole);
            await db.SaveChangesAsync();
            logger?.LogInformation("Seeded 9 roles with per-module permission bitmasks.");
        }

        // Fix existing roles that have zero permissions (post-migration fix)
        // Migration renamed old PermissionsMask → SystemPermissions, but new columns default to 0.
        var zeroPermRoles = await db.Set<Role>()
            .Where(r => r.SalesPermissions == 0 && r.InventoryPermissions == 0
                     && r.AccountingPermissions == 0 && r.Name != null)
            .ToListAsync();
        foreach (var role in zeroPermRoles)
        {
            switch (role.Name)
            {
                case "مدير النظام":
                    role.SetSalesPermissions((long)SalesPermissions.All);
                    role.SetInventoryPermissions((long)InventoryPermissions.All);
                    role.SetAccountingPermissions((long)AccountingPermissions.All);
                    break;
                case "مدير":
                    role.SetSalesPermissions((long)SalesPermissions.All);
                    role.SetInventoryPermissions((long)InventoryPermissions.All);
                    role.SetAccountingPermissions((long)AccountingPermissions.All);
                    break;
                case "محاسب":
                    role.SetSalesPermissions((long)(SalesPermissions.ViewInvoices | SalesPermissions.ViewReturns | SalesPermissions.ViewReceipts));
                    role.SetInventoryPermissions((long)(InventoryPermissions.ViewProducts | InventoryPermissions.ViewStock | InventoryPermissions.ViewBatches));
                    role.SetAccountingPermissions((long)(AccountingPermissions.All));
                    break;
                case "أمين صندوق":
                    role.SetSalesPermissions((long)(SalesPermissions.ViewInvoices | SalesPermissions.ViewReceipts | SalesPermissions.CreateReceipts));
                    role.SetAccountingPermissions((long)(AccountingPermissions.ViewCashBoxes | AccountingPermissions.ViewReceiptVouchers | AccountingPermissions.CreateReceiptVouchers | AccountingPermissions.ViewPaymentVouchers));
                    break;
                case "كاشير":
                    role.SetSalesPermissions((long)(SalesPermissions.ViewInvoices | SalesPermissions.CreateInvoices | SalesPermissions.CancelInvoices | SalesPermissions.PrintInvoices | SalesPermissions.ViewReturns | SalesPermissions.CreateReturns | SalesPermissions.ViewReceipts | SalesPermissions.CreateReceipts | SalesPermissions.ManageDiscounts));
                    role.SetInventoryPermissions((long)(InventoryPermissions.ViewProducts));
                    role.SetAccountingPermissions((long)(AccountingPermissions.ViewCashBoxes));
                    break;
                case "مشرف مخازن":
                    role.SetSalesPermissions((long)(SalesPermissions.ViewInvoices));
                    role.SetInventoryPermissions((long)(InventoryPermissions.ViewProducts | InventoryPermissions.CreateProducts | InventoryPermissions.EditProducts | InventoryPermissions.ViewStock | InventoryPermissions.AdjustStock | InventoryPermissions.TransferStock | InventoryPermissions.CountStock | InventoryPermissions.ViewBatches | InventoryPermissions.ManageCategories | InventoryPermissions.ManageUnits));
                    break;
                case "مندوب مبيعات":
                    role.SetSalesPermissions((long)(SalesPermissions.ViewInvoices | SalesPermissions.CreateInvoices | SalesPermissions.ViewReturns | SalesPermissions.CreateReturns | SalesPermissions.ViewReceipts | SalesPermissions.CreateReceipts | SalesPermissions.ManageDiscounts | SalesPermissions.PrintInvoices));
                    role.SetInventoryPermissions((long)(InventoryPermissions.ViewProducts | InventoryPermissions.ViewStock));
                    break;
                case "مراقب":
                    role.SetSalesPermissions((long)(SalesPermissions.ViewInvoices | SalesPermissions.ViewReturns | SalesPermissions.ViewReceipts));
                    role.SetInventoryPermissions((long)(InventoryPermissions.ViewProducts | InventoryPermissions.ViewStock));
                    role.SetAccountingPermissions((long)(AccountingPermissions.ViewJournalEntries | AccountingPermissions.ViewAccounts | AccountingPermissions.ViewCashBoxes | AccountingPermissions.ViewExpenses | AccountingPermissions.ViewReports));
                    break;
                case "مدير فرع":
                    role.SetSalesPermissions((long)(SalesPermissions.All & ~SalesPermissions.DeleteInvoices));
                    role.SetInventoryPermissions((long)(InventoryPermissions.ViewProducts | InventoryPermissions.CreateProducts | InventoryPermissions.EditProducts | InventoryPermissions.ViewStock | InventoryPermissions.AdjustStock | InventoryPermissions.TransferStock | InventoryPermissions.CountStock | InventoryPermissions.ViewBatches));
                    role.SetAccountingPermissions((long)(AccountingPermissions.ViewJournalEntries | AccountingPermissions.ViewAccounts | AccountingPermissions.ViewCashBoxes | AccountingPermissions.ViewExpenses | AccountingPermissions.ViewReceiptVouchers | AccountingPermissions.ViewPaymentVouchers | AccountingPermissions.ViewReports));
                    break;
            }
        }
        if (zeroPermRoles.Count > 0)
        {
            await db.SaveChangesAsync();
            logger?.LogInformation("Fixed {Count} existing roles with zero permissions (post-migration).", zeroPermRoles.Count);
        }

        // ─── Default CashBox (linked to Chart of Accounts) ────────
        if (!await db.CashBoxes.AnyAsync())
        {
            var cashAccount = await db.Set<Account>()
                .FirstAsync(a => a.AccountCode == "11010001");

            var defaultCashBox = CashBox.Create(
                name: "الصندوق الرئيسي",
                accountId: cashAccount.Id,
                description: "الصندوق النقدي الافتراضي للمنشأة",
                branchId: 1,
                createdByUserId: null
            );
            db.CashBoxes.Add(defaultCashBox);
            await db.SaveChangesAsync();
            logger?.LogInformation("Seeded default CashBox 'الصندوق الرئيسي' linked to account '{Code}'.", cashAccount.AccountCode);
        }
        else
        {
            // Fix the DEFAULT CashBox only — set BranchId = 1
            var fixedCount = await db.CashBoxes
                .Where(cb => cb.Name == "الصندوق الرئيسي" && cb.BranchId == null)
                .ExecuteUpdateAsync(s => s.SetProperty(cb => cb.BranchId, (short?)1));
            if (fixedCount > 0)
                logger?.LogInformation("Fixed default CashBox BranchId → set to 1.");
        }

        // ─── Default Warehouse (must be before early return) ────────
        if (!await db.Warehouses.AnyAsync())
        {
            var defaultWarehouse = Warehouse.Create(
                name: "المخزن الرئيسي",
                branchId: 1,
                createdByUserId: null
            );
            db.Warehouses.Add(defaultWarehouse);
            await db.SaveChangesAsync();
            logger?.LogInformation("Seeded default Warehouse 'المخزن الرئيسي'.");
        }
        else
        {
            // Fix the DEFAULT Warehouse only — set BranchId = 1
            var fixedCount = await db.Warehouses
                .Where(w => w.Name == "المخزن الرئيسي" && w.BranchId == null)
                .ExecuteUpdateAsync(s => s.SetProperty(w => w.BranchId, (short?)1));
            if (fixedCount > 0)
                logger?.LogInformation("Fixed default Warehouse BranchId → set to 1.");
        }

        // ─── Default FiscalYear (current year) ─────────────────────
        if (!await db.FiscalYears.AnyAsync())
        {
            var currentYear = DateTime.UtcNow.Year;
            db.FiscalYears.Add(FiscalYear.Create(currentYear));
            await db.SaveChangesAsync();
            logger?.LogInformation("Seeded default FiscalYear for {Year}.", currentYear);
        }

        // ─── ProductCategories ──────────────────────────────────────
        if (!await db.ProductCategories.AnyAsync())
        {
            db.ProductCategories.AddRange(
                ProductCategory.Create(branchId: 1, "مواد غذائية"),
                ProductCategory.Create(branchId: 1, "مشروبات"),
                ProductCategory.Create(branchId: 1, "منظفات")
            );
            await db.SaveChangesAsync();
            logger?.LogInformation("Seeded 3 product categories.");
        }

        if (await db.Users.AnyAsync())
        {
            logger?.LogInformation("Database already seeded. Skipping...");
            return;
        }

        var adminRoleEntity = await db.Set<Role>().FirstAsync(r => r.Name == "مدير النظام");
        short adminRoleId = adminRoleEntity.Id;

        string adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("12345678", workFactor: 12);
        var adminUser = User.CreateWithPassword(
            userName: "admin",
            passwordHash: adminPasswordHash,
            roleId: adminRoleId,
            branchId: null,
            employeeId: null,
            createdByUserId: null,
            mustChangePassword: true
        );
        db.Users.Add(adminUser);
        await db.SaveChangesAsync();
        logger?.LogInformation("Created admin user with role '{Role}' (ID: {RoleId}).", adminRoleEntity.Name, adminRoleId);

        var unitPiece = Unit.Create(name: "حبة", symbol: "pcs", isSystem: false, createdByUserId: null);
        var unitKg = Unit.Create(name: "كيلو", symbol: "kg", isSystem: false, createdByUserId: null);
        var unitLiter = Unit.Create(name: "لتر", symbol: "ltr", isSystem: false, createdByUserId: null);
        var unitMeter = Unit.Create(name: "متر", symbol: "m", isSystem: false, createdByUserId: null);
        var unitCarton = Unit.Create(name: "كرتون", symbol: "ctn", isSystem: false, createdByUserId: null);
        var unitBox = Unit.Create(name: "علبة", symbol: "box", isSystem: false, createdByUserId: null);
        var unitGram = Unit.Create(name: "جرام", symbol: "g", isSystem: false, createdByUserId: null);
        db.Units.AddRange(unitPiece, unitKg, unitLiter, unitMeter, unitCarton, unitBox, unitGram);
        await db.SaveChangesAsync();
        logger?.LogInformation("Seeded {Count} base units of measure.", 7);

        db.DocumentSequences.Add(DocumentSequence.Create("SalesInvoice"));
        db.DocumentSequences.Add(DocumentSequence.Create("PurchaseInvoice"));
        db.DocumentSequences.Add(DocumentSequence.Create("SalesReturn"));
        db.DocumentSequences.Add(DocumentSequence.Create("PurchaseReturn"));
        db.DocumentSequences.Add(DocumentSequence.Create("Transfer"));
        db.DocumentSequences.Add(DocumentSequence.Create("CustomerPayment"));
        db.DocumentSequences.Add(DocumentSequence.Create("SupplierPayment"));

        var productsWithoutUnits = await db.Products
            .Where(p => !db.ProductUnits.Any(pu => pu.ProductId == p.Id))
            .ToListAsync();

        foreach (var product in productsWithoutUnits)
        {
            db.ProductUnits.Add(ProductUnit.CreateBaseUnit(
                product.Id,
                unitPiece.Id
            ));
        }

        if (productsWithoutUnits.Any())
            logger?.LogInformation("Seeded base ProductUnits for {Count} products", productsWithoutUnits.Count);

        await db.SaveChangesAsync();

        if (!await db.Set<ProductPrice>().AnyAsync())
        {
            var baseProductUnits = await db.ProductUnits
                .Where(pu => pu.IsBaseUnit)
                .ToListAsync();

            if (baseProductUnits.Any())
            {
                var productPrices = new List<ProductPrice>();
                foreach (var pu in baseProductUnits)
                {
                    productPrices.Add(ProductPrice.Create(
                        productUnitId: pu.Id,
                        price: 100m,
                        effectiveFrom: DateTime.UtcNow,
                        createdByUserId: 1
                    ));
                }

                db.Set<ProductPrice>().AddRange(productPrices);
                await db.SaveChangesAsync();
                logger?.LogInformation("Seeded {Count} ProductPrices for {ProductUnitCount} base units (PriceLevel removed).",
                    productPrices.Count, baseProductUnits.Count);
            }
        }

        logger?.LogInformation("Seed data completed successfully.");
    }


}
