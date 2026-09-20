export type PaymentType = 'CASH' | 'KURAYMI' | 'CREDIT' | 'JEB_WALLET' | 'BANK_TRANSFER' | 'MIXED';

export type MainTabType = 
  | 'DASHBOARD'
  | 'POS' 
  | 'SALES_INVOICE' 
  | 'SALES_RETURNS'
  | 'INVENTORY' 
  | 'PURCHASES' 
  | 'CUSTOMERS' 
  | 'ACCOUNTING' 
  | 'REPORTS' 
  | 'OTHER_REPORTS'
  | 'CURRENCIES'
  | 'CURRENCY_RATES'
  | 'MOHASEB_ITEMS'
  | 'EXCEL_IMPORT'
  | 'SALES_PRICES'
  | 'ITEM_UNITS'
  | 'PRICE_QUOTATION'
  | 'PURCHASE_ORDER'
  | 'MOHASEB_ACCOUNTS'
  | 'COMMERCIAL_PURCHASE'
  | 'ARCHITECTURE' 
  | 'SECURITY'
  | 'PETTY_CASH'
  | 'EMPLOYEES'
  | 'SALES_STRATEGIES_HUB'
  | 'SALES_STRATEGY_INLINE_GRID'
  | 'SALES_STRATEGY_RAPID_BAR'
  | 'SALES_STRATEGY_SCANNER'
  | 'SALES_STRATEGY_SMART_F3'
  | 'SALES_STRATEGY_TOUCH_GRID'
  | 'SALES_STRATEGY_IMPORT_CONVERT';

// --- Employee Models (V1 Lean: Card, Optional User Link, Sales Commission / Salary) ---
export interface EmployeeItem {
  id: number; // Pure Integer sequence: 1, 2, 3...
  code: string; // e.g. "EMP-01" or "101"
  name: string;
  jobTitle: string; // كاشير، بائع، مندوب، أمين مخزن، عامل خدمات، سائق...
  phone: string;
  nationalId?: string;
  basicSalary: number; // الراتب الأساسي
  hireDate: string;
  isActive: boolean;
  notes?: string;
  accountCode?: string; // كود حساب الموظف في الدليل المحاسبي (مثل 2141-01)
  linkedUserId?: string; // معرف المستخدم المرتبط (اختياري - الموظف لا يشترط أن يكون له يوزر)
  commissionRate?: number; // نسبة العمولة على المبيعات % (اختياري)
}

// --- Petty Cash Models ---
export interface PettyCashRequest {
  id: string;
  requestNo: string;
  date: string;
  employeeName: string;
  employeeId?: string;
  amount: number;
  reason: string;
  status: 'PENDING' | 'APPROVED' | 'DISBURSED' | 'SETTLED' | 'REJECTED';
  approvedBy?: string;
  disbursedDate?: string;
  settlementDate?: string;
  settlementAmount?: number; // How much was actually spent
  refundAmount?: number; // How much was returned to the company
  notes?: string;
}

export interface SettlementExpense {
  id: string;
  date: string;
  description: string;
  amount: number;
  invoiceNumber?: string;
  expenseAccountId?: string;
}

export interface UserSession {
  id: string;
  userId: string;
  userName: string;
  device: string;
  ipAddress: string;
  lastActivity: string;
  expiresAt: string;
  isRevoked: boolean;
}

export interface SystemUser {
  id: string;
  userName: string;
  fullName: string;
  roleId: number;
  roleName: string;
  branchId: number | null; // null = all branches
  branchName: string;
  password?: string; // Default password e.g. "1" or hashed
  mustChangePassword: boolean;
  loginAttempts: number;
  isLocked: boolean;
  lockedUntil: string | null;
  lastLoginAt: string | null;
  isActive: boolean;
  defaultCashboxCode?: string; // كود الصندوق الافتراضي في الدليل
  defaultWarehouseId?: string; // معرف المستودع الافتراضي
  employeeId?: number; // رقم بطاقة الموظف المرتبطة (اختياري)
}

export interface SystemRole {
  id: number;
  name: string;
  description: string;
  isSystem: boolean; // protected from deletion
  salesPermissions: number; // Bitmask (or -1 for Super Admin)
  inventoryPermissions: number;
  accountingPermissions: number;
  systemPermissions: number;
}

export interface UnitDefinition {
  name: string;
  factor: number; // e.g., 1 carton = 12 packets
  price: number;
  cost: number;
  barcode: string;
}

export interface WarehouseStock {
  warehouseId: string;
  quantity: number;
}

export interface FifoStockLayer {
  id: string;
  purchaseInvoiceNo?: string;
  date: string;
  warehouseId?: string;
  batchNo?: string;
  expiryDate?: string;
  initialQuantity: number;
  remainingQuantity: number;
  unitCost: number; // Cost per basic unit in YER
}

export interface ProductItem {
  id: string;
  code: string;
  barcode: string;
  name: string;
  category: string;
  unit: string;
  units?: UnitDefinition[];
  costPrice: number; // YER (Current / Active FIFO cost)
  salePrice: number; // YER
  minSalePrice?: number;
  stock: number;
  minStock: number;
  hasExpiryDate?: boolean; // هل الصنف خاضع لتاريخ انتهاء الصلاحية
  valuationMethod?: 'FIFO' | 'FEFO'; // طريقة التقييم والصرف: FEFO/EEFO (الأقرب انتهاءً أولاً) أو FIFO (الوارد أولاً)
  batchNo?: string;
  expiryDate?: string;
  location?: string;
  warehouseStocks?: WarehouseStock[];
  stockByWarehouse?: Record<string, number>;
  fifoLayers?: FifoStockLayer[]; // FIFO layers queue: Oldest layers first
}

export interface CartItem {
  item: ProductItem;
  quantity: number;
  selectedUnit: string;
  unitPrice: number;
  unitCost: number;
  discount: number; // in YER (direct line discount)
  taxRate: number; // percentage (usually 0 or 5%)
  total: number;
  unitFactor?: number;
  batchNo?: string;
  expiryDate?: string;
  allocatedDiscount?: number; // حصة السطر الموزعة من الخصم التجاري الإجمالي
  allocatedTax?: number;      // قيمة الضريبة الموزعة على السطر
  netUnitPrice?: number;      // صافي سعر البيع للوحدة بعد كافّة الخصومات
  fifoCostTotal?: number; // Total Cost of Goods Sold under FIFO
  fifoLayersDeducted?: Array<{ layerId: string; batchNo?: string; quantity: number; unitCost: number; totalCost: number }>;
}

// Normalized Lightweight Database Schema for Invoice Lines (تطبيع أسطر الفواتير لقواعد البيانات)
export interface InvoiceLineRecord {
  id: string;
  invoiceId: string;
  productId: string;
  productCode: string;
  productName: string;
  unitName: string;
  unitFactor: number;
  quantity: number;
  unitPrice: number;
  unitCost: number;
  discount: number;
  taxRate: number;
  total: number;
  allocatedDiscount?: number; // حصة السطر الموزعة من خصم الفاتورة الإجمالي + الخصم المباشر
  allocatedTax?: number;      // حصة السطر الموزعة من ضريبة الفاتورة
  netUnitPrice?: number;      // صافي سعر الوحدة بعد الخصم الموزع
  effectiveTotal?: number;    // الإجمالي الصافي النهائي للسطر مع الضريبة
  batchNo?: string;
  expiryDate?: string;
}

export interface Customer {
  id: string;
  code: string;
  name: string;
  phone: string;
  address: string;
  creditLimit: number;
  currentBalance: number; // positive = debit (owes us)
  accountCode: string;
  taxNumber?: string;
  email?: string;
  contactPerson?: string;
  creditDays?: number; // فترة الائتمان بالأيام (مثلاً 30 أو 45 يوم)
  status?: 'ACTIVE' | 'BLOCKED' | 'SUSPENDED';
  currency?: string;
  notes?: string;
  openingBalance?: number;
  openingBalanceDate?: string;
}

export interface Supplier {
  id: string;
  code: string;
  name: string;
  phone: string;
  company: string;
  currentBalance: number; // positive = credit (we owe them)
  accountCode: string;
  currency?: string;
  address?: string;
  creditLimit?: number;
  taxNumber?: string;
  email?: string;
  notes?: string;
}

export interface SplitPaymentBreakdown {
  cash: number;
  cashAccountCode?: string; // كود حساب الصندوق
  cashAccountName?: string; // اسم الصندوق
  network: number; // شبكة / بنك
  bankAccountCode?: string; // كود الحساب البنكي من شجرة الحسابات
  bankAccountName?: string; // اسم الحساب البنكي
  bankRefNo?: string; // رقم الحوالة أو العملية البنكية
  wallet?: number; // محفظة إلكترونية
  walletAccountCode?: string; // كود حساب المحفظة
  walletAccountName?: string; // اسم المحفظة
  credit: number; // آجل على الحساب (المتبقي من الفاتورة)
  creditCustomerId?: string; // معرف العميل الآجل
  creditCustomerName?: string; // اسم العميل الآجل
  notes?: string;
}

export interface Invoice {
  id: string;
  invoiceNo: string;
  type: 'SALE' | 'SALE_RETURN' | 'PURCHASE' | 'PURCHASE_RETURN';
  date: string;
  time: string;
  customerOrSupplierId: string;
  customerOrSupplierName: string;
  items: CartItem[];
  subtotal: number;
  discountTotal: number;
  taxTotal: number;
  otherCharges?: number;
  otherChargesName?: string;
  otherChargesAccountCode?: string;
  otherChargesAccountName?: string;
  grandTotal: number;
  paidAmount: number;
  remainingAmount: number;
  paymentType: PaymentType;
  splitPayments?: SplitPaymentBreakdown;
  referenceNo?: string;
  cashierName: string;
  notes?: string;
  warehouseId?: string;
  warehouseName?: string;
  status?: 'DRAFT' | 'POSTED' | 'CANCELLED';
  currency?: string;
  exchangeRate?: number;
  journalEntryId?: string;
  originalInvoiceNo?: string;
  fifoCostTotal?: number; // Total Cost of Goods Sold under FIFO
  grossProfit?: number; // grandTotal - fifoCostTotal
  treasuryAccountCode?: string; // كود حساب الصندوق أو البنك المرتبط بالسداد
  treasuryAccountName?: string; // اسم الصندوق أو البنك
}

export interface HeldOrder {
  id: string;
  heldAt: string;
  customerName: string;
  customerId?: string;
  items: CartItem[];
  notes?: string;
  grandTotal?: number;
  discountOverall?: number;
  discountType?: 'AMOUNT' | 'PERCENT';
}

export interface AccountNode {
  code: string;
  name: string;
  nameEn?: string;
  type: 'ASSET' | 'LIABILITY' | 'EQUITY' | 'REVENUE' | 'EXPENSE';
  financialStatement?: 'BALANCE_SHEET' | 'INCOME_STATEMENT';
  balance: number;
  openingBalance?: number;
  nature: 'DEBIT' | 'CREDIT';
  level?: number;
  treePath?: string; // e.g. "/1/11/111/"
  isLeaf?: boolean; // false = رئيسي جامع (تجميعي)، true = فرعي تحليلي يقبل القيود
  isSubAccount?: boolean; // alias for isLeaf (فرعي يقبل تسجيل العمليات)
  currency?: string; // YER, SAR, USD
  allowCostCenter?: boolean; // هل يقبل مركز تكلفة
  status?: 'ACTIVE' | 'INACTIVE';
  notes?: string;
  parentCode?: string;
  children?: AccountNode[];
}

export interface CostCenterNode {
  id: string;
  code: string;
  name: string;
  nameEn?: string;
  parentCode?: string;
  level: number;
  isLeaf: boolean; // false = رئيسي، true = مركز تحليلي تشغيلي
  budget: number; // الموازنة التقديرية
  managerName?: string;
  status: 'ACTIVE' | 'INACTIVE';
  notes?: string;
  children?: CostCenterNode[];
}

export interface JournalEntryLine {
  id: string;
  accountCode: string;
  accountName: string;
  costCenterId?: string;
  costCenterCode?: string;
  costCenterName?: string;
  debit: number;
  credit: number;
  note: string;
}

export interface JournalEntry {
  id: string;
  entryNo: string;
  date: string;
  reference: string;
  description: string;
  lines: JournalEntryLine[];
  totalDebit: number;
  totalCredit: number;
  isBalanced: boolean;
  status: 'POSTED' | 'DRAFT';
  createdBy?: string;
  createdAt?: string;
  sourceModule?: string;
}

export type ShiftMode = 'DAILY_SIMPLE' | 'ENTERPRISE_MULTI';

export interface ShiftInfo {
  shiftNumber: number;
  cashierName: string;
  cashierId?: string;
  branchName?: string;
  openedAt: string;
  openingCash: number;
  currentCashInDrawer: number;
  totalSales: number;
  totalReturns: number;
  totalCreditSales: number;
  status?: 'OPEN' | 'CLOSED';
  mode?: ShiftMode;
}

export interface ClosedShift {
  id: string;
  shiftNumber: number;
  mode: ShiftMode;
  cashierId?: string;
  cashierName: string;
  branchName?: string;
  openedAt: string;
  closedAt: string;
  openingCash: number;
  expectedCash: number;
  actualCash: number;
  cashDifference: number; // actualCash - expectedCash (0 = matched, >0 overage, <0 shortage)
  totalSales: number;
  cashSales: number;
  cardSales: number;
  creditSales: number;
  totalReturns: number;
  invoiceCount: number;
  transferVoucherNo?: string;
  transferToAccount?: string;
  transferredAmount?: number;
  notes?: string;
  closedBy: string;
}

export interface VoucherInvoiceAllocation {
  invoiceId: string;
  invoiceNo: string;
  invoiceDate?: string;
  invoiceTotal?: number;
  allocatedAmount: number;
}

export interface CashVoucher {
  id: string;
  voucherNo: string;
  type: 'RECEIPT' | 'PAYMENT' | 'TRANSFER'; // RECEIPT = سند قبض، PAYMENT = سند صرف، TRANSFER = تحويل بين الصناديق والبنوك
  paymentMethod: 'CASH' | 'BANK' | 'CHEQUE'; // نقداً أو تحويل بنكي أو شيك
  date: string; // YYYY-MM-DD
  amount: number;
  currency: string;
  cashAccountCode: string; // كود حساب الصندوق أو البنك (مثل 1111 الصندوق الرئيسي، 1121 بنك التضامن)
  cashAccountName: string;
  partyType: 'CUSTOMER' | 'SUPPLIER' | 'EXPENSE' | 'OTHER';
  partyId?: string;
  partyName: string;
  contraAccountCode: string; // الحساب المقابل (عميل، مورد، مصروف، إلخ)
  contraAccountName: string;
  costCenterId?: string;
  costCenterCode?: string;
  costCenterName?: string;
  reference?: string; // رقم الشيك أو الحوالة أو السند اليدوي
  description: string;
  status: 'POSTED' | 'DRAFT' | 'VOID';
  journalEntryId?: string;
  createdBy: string;
  createdAt?: string;
  voidedAt?: string;
  voidReason?: string;
  allocatedInvoices?: VoucherInvoiceAllocation[];
  checkNumber?: string;
  bankName?: string;
  dueDate?: string;
}

export interface CustomerStatementMovement {
  id: string;
  date: string;
  time?: string;
  type: 'OPENING_BALANCE' | 'SALE_INVOICE' | 'SALE_RETURN' | 'RECEIPT_VOUCHER' | 'PAYMENT_VOUCHER' | 'JOURNAL_ENTRY';
  typeLabel: string;
  documentNo: string;
  referenceNo?: string;
  description: string;
  debit: number; // مدين (المسحوبات والمبيعات)
  credit: number; // دائن (المقبوضات والمردودات)
  balanceAfter: number; // الرصيد التراكمي
  status?: string;
}

export interface CustomerAgingBucket {
  periodLabel: string; // 0-30, 31-60, 61-90, +90
  amount: number;
  invoiceCount: number;
  percentage: number;
}

export interface FiscalPeriod {
  id: string;
  periodName: string; // e.g. "يناير 2026", "الربع الأول 2026", "السنة المالية 2026"
  fiscalYear: number;
  startDate: string; // YYYY-MM-DD
  endDate: string; // YYYY-MM-DD
  status: 'OPEN' | 'CLOSED';
  isClosed: boolean;
  closedAt?: string;
  closedBy?: string;
  closingNotes?: string;
  closingEntryId?: string;
  netProfitLoss?: number;
  totalRevenues?: number;
  totalExpenses?: number;
}

export interface Warehouse {
  id: string;
  code: string;
  name: string;
  branchId: number;
  branchName: string;
  managerName: string;
  phone: string;
  location: string;
  isDefault: boolean;
  isActive: boolean;
  accountCode?: string; // كود حساب مخزون المستودع في الدليل المحاسبي
  notes?: string;
}

export type StockTransferStatus = 'DRAFT' | 'PENDING' | 'IN_TRANSIT' | 'RECEIVED' | 'CANCELLED';

export interface StockTransferItem {
  id: string;
  productId: string;
  productCode: string;
  barcode: string;
  productName: string;
  unit: string;
  quantity: number;
  unitCost: number;
  totalCost: number;
  availableStock?: number;
  batchNo?: string;
  expiryDate?: string;
  notes?: string;
}

export interface StockTransfer {
  id: string;
  transferNo: string;
  date: string; // YYYY-MM-DD
  fromWarehouseId: string;
  fromWarehouseName: string;
  toWarehouseId: string;
  toWarehouseName: string;
  status: StockTransferStatus;
  items: StockTransferItem[];
  totalQuantity: number;
  totalCost: number;
  reference?: string; // رقم إذن الصرف أو الوثيقة الورقية
  notes?: string;
  driverName?: string; // اسم السائق / مسؤول النقل
  vehiclePlate?: string; // رقم لوحة وسيلة النقل
  dispatchedBy?: string;
  dispatchedAt?: string;
  receivedBy?: string;
  receivedAt?: string;
  journalEntryId?: string;
  createdAt: string;
  createdBy: string;
}

// ----------------------------------------------------
// MODULE 04: Stocktaking & Stock Adjustment Types
// ----------------------------------------------------

export type StocktakeStatus = 'DRAFT' | 'IN_PROGRESS' | 'COMPLETED' | 'ADJUSTED' | 'CANCELLED';

export interface StocktakeItem {
  id: string;
  productId: string;
  productCode: string;
  barcode: string;
  productName: string;
  unit: string;
  unitCost: number;
  bookStock: number; // الرصيد الدفتري في النظام
  physicalStock: number; // الرصيد الفعلي المعدود
  differenceQty: number; // physicalStock - bookStock (+ فائض، - عجز)
  differenceCost: number; // differenceQty * unitCost
  batchNo?: string;
  expiryDate?: string;
  notes?: string;
}

export interface StocktakingSession {
  id: string;
  sessionNo: string; // e.g. "ST-2026-001"
  warehouseId: string;
  warehouseName: string;
  date: string;
  time?: string;
  committeeMembers: string[]; // أعضاء لجنة الجرد (رئيس اللجنة، عضو، أمين المستودع)
  status: StocktakeStatus;
  notes?: string;
  items: StocktakeItem[];
  totalBookQty: number;
  totalPhysicalQty: number;
  totalSurplusQty: number;
  totalDeficitQty: number;
  totalSurplusValue: number;
  totalDeficitValue: number;
  netDifferenceValue: number;
  adjustmentVoucherNo?: string;
  adjustmentEntryId?: string;
  adjustedAt?: string;
  createdAt: string;
  createdBy: string;
}

export type StockAdjustmentType = 'SURPLUS' | 'DEFICIT' | 'BOTH';

export interface StockAdjustmentItem {
  id: string;
  productId: string;
  productCode: string;
  barcode: string;
  productName: string;
  unit: string;
  unitCost: number;
  bookQty: number;
  physicalQty: number;
  adjustmentQty: number; // الكمية المراد تسويتها (+ أو -)
  totalCost: number; // adjustmentQty * unitCost
  batchNo?: string;
  notes?: string;
}

export interface StockAdjustment {
  id: string;
  adjustmentNo: string; // e.g. "ADJ-2026-001"
  date: string;
  time?: string;
  warehouseId: string;
  warehouseName: string;
  stocktakeSessionId?: string;
  stocktakeSessionNo?: string;
  type: StockAdjustmentType;
  reason: string; // سبب التسوية (فروقات جرد دوري، تلف مخزني، أخطاء إدخال، إلخ)
  items: StockAdjustmentItem[];
  totalItems: number;
  netAdjustmentCost: number;
  totalSurplusCost: number;
  totalDeficitCost: number;
  status: 'POSTED' | 'DRAFT';
  journalEntryId?: string;
  approvedBy: string;
  notes?: string;
  createdAt: string;
  createdBy: string;
}

// ----------------------------------------------------
// Stock Ledger Card (كرت حركة الصنف)
// ----------------------------------------------------

export type StockMovementType = 
  | 'OPENING_BALANCE' 
  | 'PURCHASE' 
  | 'PURCHASE_RETURN' 
  | 'SALE' 
  | 'SALE_RETURN' 
  | 'TRANSFER_IN' 
  | 'TRANSFER_OUT' 
  | 'ADJUSTMENT_SURPLUS' 
  | 'ADJUSTMENT_DEFICIT';

export interface StockLedgerItem {
  id: string;
  date: string;
  time: string;
  movementType: StockMovementType;
  movementLabel: string;
  documentNo: string;
  warehouseId: string;
  warehouseName: string;
  inQty: number;
  outQty: number;
  balanceAfter: number;
  unitPriceOrCost: number;
  totalMovementValue: number;
  partyName?: string; // المورد أو العميل أو السائق
  notes?: string;
}

export interface CurrencyItem {
  id: string;
  name: string; // محلي، دولار، سعودي
  subUnit: string; // فلس، سنت
  code: string; // YR, USD, SAR
  accountsCount: number; // 2, 1, 0
  isLocal?: boolean;
}

export interface CurrencyRateItem {
  id: string;
  currencyName: string; // دولار، سعودي
  rate: number; // 700, 600, 500, 900
  fromDate: string; // 2026-05-25
  toDate?: string; // 2026-05-24 or empty
  isCurrent?: boolean;
  minRate?: number; // الحد الأدنى المسموح به لسعر الصرف لمنع أخطاء الإدخال
  maxRate?: number; // الحد الأقصى المسموح به لسعر الصرف
}



