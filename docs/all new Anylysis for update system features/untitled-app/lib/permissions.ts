import { SystemRole, SystemUser } from '@/types/pos';

// 1. Sales Permissions Bitmask Flags
export enum SalesPermissionFlags {
  ViewInvoices = 1 << 0,       // 1: عرض فواتير المبيعات
  CreateInvoices = 1 << 1,     // 2: إنشاء وإصدار فاتورة جديدة
  PrintInvoices = 1 << 2,      // 4: طباعة الفواتير الحرارية
  ManageDiscounts = 1 << 3,    // 8: منح وتعديل الخصومات
  SalesReturns = 1 << 4,       // 16: إصدار مرتجع مبيعات
  CancelInvoices = 1 << 5,     // 32: إلغاء أو حذف الفواتير
  ViewCostPrice = 1 << 6,      // 64: رؤية سعر التكلفة وهوامش الربح
  ChangePrice = 1 << 7,        // 128: تغيير سعر بيع الصنف يدوياً
  HoldResumeOrders = 1 << 8,   // 256: تعليق واسترجاع الفواتير (Hold/Resume)
  ReprintOldInvoices = 1 << 9, // 512: إعادة طباعة الفواتير السابقة
}

// 2. Inventory Permissions Bitmask Flags
export enum InventoryPermissionFlags {
  ViewProducts = 1 << 0,       // 1: عرض بطاقة الأصناف والمخزون
  CreateProduct = 1 << 1,      // 2: إضافة صنف جديد
  EditProduct = 1 << 2,        // 4: تعديل بيانات وأسعار الأصناف
  DeleteProduct = 1 << 3,      // 8: حذف أو أرشفة الأصناف
  TransferStock = 1 << 4,      // 16: إجراء تحويلات مخزنية بين الفروع
  CountStock = 1 << 5,         // 32: تسوية وجرد المخزون الفعلي
  BarcodePrinting = 1 << 6,    // 64: طباعة باركود الأصناف
  BatchManagement = 1 << 7,    // 128: إدارة تواريخ الصلاحية ورقم الدفعة
}

// 3. Accounting Permissions Bitmask Flags
export enum AccountingPermissionFlags {
  ViewChartOfAccounts = 1 << 0,   // 1: استعراض دليل الحسابات الشجري
  CreateJournalEntries = 1 << 1,  // 2: إنشاء قيود اليومية العامة
  PostJournalEntries = 1 << 2,    // 4: ترحيل واعتماد القيود المحاسبية
  IssueVouchers = 1 << 3,         // 8: إصدار سندات القبض والصرف
  ViewAccountStatement = 1 << 4,  // 16: كشف حساب العملاء والموردين
  ViewTrialBalance = 1 << 5,      // 32: ميزان المراجعة وقائمة الدخل
  ViewProfitLoss = 1 << 6,        // 64: تقارير الأرباح والخسائر
  CloseFinancialPeriod = 1 << 7,  // 128: إغلاق الفترات والسنوات المالية
}

// 4. System & Security Permissions Bitmask Flags
export enum SystemPermissionFlags {
  ViewUsers = 1 << 0,          // 1: عرض قائمة المستخدمين
  ManageUsers = 1 << 1,        // 2: إضافة وتعديل وتفعيل المستخدمين
  ManageRoles = 1 << 2,        // 4: إدارة مصفوفة الأدوار والصلاحيات
  ViewSessions = 1 << 3,       // 8: استعراض الجلسات النشطة
  RevokeSessions = 1 << 4,     // 16: إنهاء وفصل جلسات المستخدمين
  DatabaseBackup = 1 << 5,     // 32: أخذ نسخ احتياطية لقاعدة البيانات
  AuditLogView = 1 << 6,       // 64: سجل الرقابة والتدقيق الأمني
  GeneralSettings = 1 << 7,    // 128: إعدادات النظام والفرع العامة
}

export interface PermissionItemDef {
  key: string;
  flag: number;
  labelAr: string;
  labelEn: string;
  descAr: string;
  isSensitive?: boolean;
}

export const SALES_PERMISSION_DEFS: PermissionItemDef[] = [
  { key: 'ViewInvoices', flag: SalesPermissionFlags.ViewInvoices, labelAr: 'عرض فواتير المبيعات', labelEn: 'View Invoices', descAr: 'استعراض قائمة فواتير المبيعات والبحث فيها' },
  { key: 'CreateInvoices', flag: SalesPermissionFlags.CreateInvoices, labelAr: 'إصدار فاتورة مبيعات', labelEn: 'Create Invoices', descAr: 'إنشاء وحفظ فواتير كاشير نقدية وآجلة' },
  { key: 'PrintInvoices', flag: SalesPermissionFlags.PrintInvoices, labelAr: 'طباعة الإيصالات والفواتير', labelEn: 'Print Invoices', descAr: 'طباعة الفاتورة الحرارية 80mm أو A4' },
  { key: 'ManageDiscounts', flag: SalesPermissionFlags.ManageDiscounts, labelAr: 'منح خصم إضافي', labelEn: 'Manage Discounts', descAr: 'إمكانية إعطاء خصم مالي أو نسبة على الفاتورة', isSensitive: true },
  { key: 'SalesReturns', flag: SalesPermissionFlags.SalesReturns, labelAr: 'إصدار مرتجع مبيعات', labelEn: 'Sales Returns', descAr: 'إرجاع أصناف مباعة واسترداد القيمة', isSensitive: true },
  { key: 'CancelInvoices', flag: SalesPermissionFlags.CancelInvoices, labelAr: 'إلغاء وشطب الفواتير', labelEn: 'Cancel Invoices', descAr: 'حذف أو إلغاء فواتير معتمدة', isSensitive: true },
  { key: 'ViewCostPrice', flag: SalesPermissionFlags.ViewCostPrice, labelAr: 'رؤية أسعار التكلفة', labelEn: 'View Cost Price', descAr: 'إظهار تكلفة الشراء وهامش الربح للكاشير', isSensitive: true },
  { key: 'ChangePrice', flag: SalesPermissionFlags.ChangePrice, labelAr: 'تعديل سعر البيع يدوياً', labelEn: 'Manual Price Change', descAr: 'تغيير السعر الافتراضي المحدد في بطاقة الصنف', isSensitive: true },
  { key: 'HoldResumeOrders', flag: SalesPermissionFlags.HoldResumeOrders, labelAr: 'تعليق واسترجاع الفواتير', labelEn: 'Hold & Resume Orders', descAr: 'تعليق طلب زبون وخدمة عميل آخر ثم استرجاعه' },
  { key: 'ReprintOldInvoices', flag: SalesPermissionFlags.ReprintOldInvoices, labelAr: 'إعادة طباعة الفواتير السابقة', labelEn: 'Reprint Invoices', descAr: 'طباعة نسخة ثانية من فواتير مغلقة' },
];

export const INVENTORY_PERMISSION_DEFS: PermissionItemDef[] = [
  { key: 'ViewProducts', flag: InventoryPermissionFlags.ViewProducts, labelAr: 'عرض بطاقة الأصناف والمخزون', labelEn: 'View Products', descAr: 'استعراض الأصناف، الكميات المتوفرة، والأسعار' },
  { key: 'CreateProduct', flag: InventoryPermissionFlags.CreateProduct, labelAr: 'إضافة صنف جديد', labelEn: 'Create Product', descAr: 'تعريف منتجات جديدة وباركودات ووحدات قياس' },
  { key: 'EditProduct', flag: InventoryPermissionFlags.EditProduct, labelAr: 'تعديل بيانات وأسعار الأصناف', labelEn: 'Edit Product', descAr: 'تغيير الاسم، السعر، التصنيف، وحدود المخزون', isSensitive: true },
  { key: 'DeleteProduct', flag: InventoryPermissionFlags.DeleteProduct, labelAr: 'حذف وأرشفة الأصناف', labelEn: 'Delete Product', descAr: 'إلغاء تنشيط أو شطب صنف من النظام', isSensitive: true },
  { key: 'TransferStock', flag: InventoryPermissionFlags.TransferStock, labelAr: 'التحويل المخزني بين الفروع', labelEn: 'Stock Transfers', descAr: 'إصدار وتأكيد أوامر التحويل بين المستودعات' },
  { key: 'CountStock', flag: InventoryPermissionFlags.CountStock, labelAr: 'الجرد والتسويات المخزنية', labelEn: 'Stock Audit / Count', descAr: 'تعديل الأرصدة الفعلية وإثبات العجز أو الفائض', isSensitive: true },
  { key: 'BarcodePrinting', flag: InventoryPermissionFlags.BarcodePrinting, labelAr: 'طباعة ملصقات الباركود', labelEn: 'Barcode Printing', descAr: 'توليد وطباعة استيكرات باركود للمنتجات' },
  { key: 'BatchManagement', flag: InventoryPermissionFlags.BatchManagement, labelAr: 'إدارة الدفعات وتواريخ الصلاحية', labelEn: 'Batch & Expiry Control', descAr: 'إدخال وتتبع أرقام التشغيلات والدفعات' },
];

export const ACCOUNTING_PERMISSION_DEFS: PermissionItemDef[] = [
  { key: 'ViewChartOfAccounts', flag: AccountingPermissionFlags.ViewChartOfAccounts, labelAr: 'استعراض الدليل الشجري للحسابات', labelEn: 'View Chart of Accounts', descAr: 'تصفح شجرة الحسابات اليمنية القياسية وأرصدتها' },
  { key: 'CreateJournalEntries', flag: AccountingPermissionFlags.CreateJournalEntries, labelAr: 'إنشاء قيود اليومية العامة', labelEn: 'Create Journal Entries', descAr: 'تسجيل قيود محاسبية مركبة يدوياً' },
  { key: 'PostJournalEntries', flag: AccountingPermissionFlags.PostJournalEntries, labelAr: 'ترحيل واعتماد القيود', labelEn: 'Post Entries', descAr: 'ترحيل القيود للحسابات العامة بشكل نهائي', isSensitive: true },
  { key: 'IssueVouchers', flag: AccountingPermissionFlags.IssueVouchers, labelAr: 'إصدار سندات القبض والصرف', labelEn: 'Cash Vouchers', descAr: 'سندات قبض نقدية وسندات صرف للموردين والمصاريف' },
  { key: 'ViewAccountStatement', flag: AccountingPermissionFlags.ViewAccountStatement, labelAr: 'كشف حساب العملاء والموردين', labelEn: 'Account Statements', descAr: 'توليد وطباعة كشوفات الأرصدة والحركات' },
  { key: 'ViewTrialBalance', flag: AccountingPermissionFlags.ViewTrialBalance, labelAr: 'ميزان المراجعة وقائمة المركز المالي', labelEn: 'Trial Balance & Balance Sheet', descAr: 'استخراج القوائم المالية الإجمالية والفرعية' },
  { key: 'ViewProfitLoss', flag: AccountingPermissionFlags.ViewProfitLoss, labelAr: 'تقارير الأرباح والخسائر والمبيعات', labelEn: 'P&L Reports', descAr: 'استعراض صافي الأرباح اليومية والشهرية', isSensitive: true },
  { key: 'CloseFinancialPeriod', flag: AccountingPermissionFlags.CloseFinancialPeriod, labelAr: 'إغلاق الفترة والوردية المالية', labelEn: 'Close Fiscal Period', descAr: 'إقفال حسابات الفترة وترحيل الأرصدة', isSensitive: true },
];

export const SYSTEM_PERMISSION_DEFS: PermissionItemDef[] = [
  { key: 'ViewUsers', flag: SystemPermissionFlags.ViewUsers, labelAr: 'استعراض حسابات المستخدمين', labelEn: 'View Users', descAr: 'عرض المستخدمين، الفروع، والحالة' },
  { key: 'ManageUsers', flag: SystemPermissionFlags.ManageUsers, labelAr: 'إدارة المستخدمين وكلمات المرور', labelEn: 'Manage Users', descAr: 'إضافة حسابات، إعادة التعيين، وفك القفل', isSensitive: true },
  { key: 'ManageRoles', flag: SystemPermissionFlags.ManageRoles, labelAr: 'إدارة مصفوفة الأدوار والصلاحيات', labelEn: 'Manage Roles & RBAC', descAr: 'تعديل قناع البتات وتعيين الصلاحيات', isSensitive: true },
  { key: 'ViewSessions', flag: SystemPermissionFlags.ViewSessions, labelAr: 'مراقبة الجلسات النشطة', labelEn: 'View Active Sessions', descAr: 'استعراض الأجهزة وعناوين IP المتصلة' },
  { key: 'RevokeSessions', flag: SystemPermissionFlags.RevokeSessions, labelAr: 'إنهاء وفصل جلسات المستخدمين', labelEn: 'Revoke Sessions', descAr: 'تسجيل خروج إجباري لأي جهاز متصل', isSensitive: true },
  { key: 'DatabaseBackup', flag: SystemPermissionFlags.DatabaseBackup, labelAr: 'النسخ الاحتياطي لقاعدة البيانات', labelEn: 'Database Backup', descAr: 'توليد ملفات النسخ الاحتياطي واسترجاعها', isSensitive: true },
  { key: 'AuditLogView', flag: SystemPermissionFlags.AuditLogView, labelAr: 'سجل الرقابة وتتبع الحركات', labelEn: 'Audit Log Trail', descAr: 'تتبع كل العمليات الحساسة التي قام بها المستخدمون' },
  { key: 'GeneralSettings', flag: SystemPermissionFlags.GeneralSettings, labelAr: 'الإعدادات العامة وإعدادات الفروع', labelEn: 'General Settings', descAr: 'العملة، الضرائب، الترويسة وطابعات الكاشير' },
];

// Helper: Check if role has full Super Admin Bypass (-1L or ID 1)
export function isSuperAdmin(roleOrId: SystemRole | number | null | undefined): boolean {
  if (!roleOrId) return false;
  if (typeof roleOrId === 'number') {
    return roleOrId === 1;
  }
  return roleOrId.id === 1 || roleOrId.salesPermissions === -1;
}

// Core RBAC Verification Engine
export function hasPermission(
  role: SystemRole | null | undefined,
  module: 'sales' | 'inventory' | 'accounting' | 'system',
  flag: number
): boolean {
  if (!role) return false;
  if (isSuperAdmin(role)) return true;

  let currentBitmask = 0;
  switch (module) {
    case 'sales':
      currentBitmask = role.salesPermissions;
      break;
    case 'inventory':
      currentBitmask = role.inventoryPermissions;
      break;
    case 'accounting':
      currentBitmask = role.accountingPermissions;
      break;
    case 'system':
      currentBitmask = role.systemPermissions;
      break;
  }

  // Check if mask has -1 (all permissions) or bitwise AND matches flag
  if (currentBitmask === -1) return true;
  return (currentBitmask & flag) === flag;
}

// Check permission from SystemUser directly with fallback
export function userHasPermission(
  user: SystemUser | null | undefined,
  roles: SystemRole[],
  module: 'sales' | 'inventory' | 'accounting' | 'system',
  flag: number
): boolean {
  if (!user) return false;
  if (user.roleId === 1) return true; // Super Admin ID
  const userRole = roles.find((r) => r.id === user.roleId);
  return hasPermission(userRole, module, flag);
}

// Convert bitmask to formatted Hex representation (e.g. 0x000000FF)
export function toHexMask(mask: number): string {
  if (mask === -1) return '0xFFFFFFFF (All Access)';
  const hex = (mask >>> 0).toString(16).toUpperCase().padStart(8, '0');
  return `0x${hex}`;
}

// Convert bitmask to binary representation (e.g. 0000 0001 0111)
export function toBinaryMask(mask: number, bitsCount: number = 8): string {
  if (mask === -1) return '11111111 (Bypass)';
  return (mask >>> 0).toString(2).padStart(bitsCount, '0');
}
