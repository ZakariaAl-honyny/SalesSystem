'use client';

import React, { useState, useEffect } from 'react';
import { 
  INITIAL_PRODUCTS, 
  INITIAL_CUSTOMERS, 
  INITIAL_SUPPLIERS, 
  INITIAL_CHART_OF_ACCOUNTS, 
  INITIAL_JOURNAL_ENTRIES, 
  INITIAL_INVOICES, 
  INITIAL_SHIFT,
  INITIAL_CLOSED_SHIFTS,
  INITIAL_USERS,
  INITIAL_ROLES,
  INITIAL_SESSIONS,
  INITIAL_COST_CENTERS,
  INITIAL_VOUCHERS,
  INITIAL_FISCAL_PERIODS,
  INITIAL_WAREHOUSES,
  INITIAL_STOCK_TRANSFERS,
  INITIAL_PURCHASE_INVOICES,
  INITIAL_STOCKTAKES,
  INITIAL_STOCK_ADJUSTMENTS,
  INITIAL_CURRENCIES,
  INITIAL_CURRENCY_RATES,
  INITIAL_EMPLOYEES,
  updateProductWarehouseStock,
  calculateWeightedAverageCost,
  processFifoDeduction,
  addFifoPurchaseLayer,
} from '@/lib/mock';
import { 
  ProductItem, 
  Customer, 
  Supplier, 
  AccountNode, 
  JournalEntry, 
  Invoice, 
  HeldOrder, 
  ShiftInfo,
  ClosedShift,
  ShiftMode,
  SystemUser,
  SystemRole,
  UserSession,
  CostCenterNode,
  CashVoucher,
  FiscalPeriod,
  Warehouse,
  StockTransfer,
  StockTransferStatus,
  StocktakingSession,
  StockAdjustment,
  VoucherInvoiceAllocation,
  MainTabType,
  CurrencyItem,
  CurrencyRateItem,
  EmployeeItem,
} from '@/types/pos';
import { DesktopTitleBar } from '@/components/DesktopTitleBar';
import { DesktopMenuBar } from '@/components/DesktopMenuBar';
import { POSModule } from '@/components/POSModule';
import { InventoryModule } from '@/components/InventoryModule';
import { PurchasesModule } from '@/components/PurchasesModule';
import { CustomersModule } from '@/components/CustomersModule';
import { AccountingModule } from '@/components/AccountingModule';
import { ReportsModule } from '@/components/ReportsModule';
import { SecurityModule } from '@/components/SecurityModule';
import { EmployeesModule } from '@/components/EmployeesModule';
import { WinFormsArchitectureModal } from '@/components/WinFormsArchitectureModal';
import { ReceiptModal } from '@/components/ReceiptModal';
import { CalculatorModal } from '@/components/CalculatorModal';
import { DesktopStatusBar } from '@/components/DesktopStatusBar';
import { LoginModal } from '@/components/LoginModal';
import { ChangePasswordModal } from '@/components/ChangePasswordModal';
import { MohasebSoftDashboard } from '@/components/MohasebSoftDashboard';
import { CommercialSalesInvoiceView } from '@/components/CommercialSalesInvoiceView';
import { OtherReportsView } from '@/components/OtherReportsView';
import { CurrenciesAndRatesView } from '@/components/CurrenciesAndRatesView';
import { MohasebProductsModuleView } from '@/components/MohasebProductsModuleView';
import { MohasebAccountsView } from '@/components/MohasebAccountsView';
import { CommercialPurchaseInvoiceView } from '@/components/CommercialPurchaseInvoiceView';
import { PriceQuotationView } from '@/components/PriceQuotationView';
import { PurchaseOrderView } from '@/components/PurchaseOrderView';
import { SalesReturnsView } from '@/components/SalesReturnsView';
import { PettyCashModule } from '@/components/PettyCashModule';
import { MobileAppView } from '@/components/MobileAppView';
import { WebPortalView } from '@/components/WebPortalView';
import { PlatformComparisonModal } from '@/components/PlatformComparisonModal';
import { BackupRestoreModal } from '@/components/BackupRestoreModal';
import { WinFormsDialogProvider } from '@/context/WinFormsDialogContext';
import { WinFormsShowDialogShowcase } from '@/components/WinFormsShowDialogShowcase';
import {
  FrmSalesInvoice_InlineGrid,
  FrmSalesInvoice_RapidBar,
  FrmSalesInvoice_ContinuousScanner,
  FrmSalesInvoice_SmartF3Lookup,
  FrmSalesInvoice_TouchGrid,
  FrmSalesInvoice_ImportConvert,
  SalesStrategiesLabView,
} from '@/components/sales';
import { generateNextId, generateJournalEntryNumber } from '@/lib/idGenerator';

export default function POSDesktopApp() {
  const [currency] = useState<string>('ريال يمني');
  const [activeTab, setActiveTab] = useState<MainTabType>('REPORTS');
  const [currentWarehouseId, setCurrentWarehouseId] = useState<string>('WH-01');
  
  // In-memory persistent state for the simulation session
  const [products, setProducts] = useState<ProductItem[]>(INITIAL_PRODUCTS);
  const [customers, setCustomers] = useState<Customer[]>(INITIAL_CUSTOMERS);
  const [suppliers, setSuppliers] = useState<Supplier[]>(INITIAL_SUPPLIERS);
  const [chartOfAccounts, setChartOfAccounts] = useState<AccountNode[]>(INITIAL_CHART_OF_ACCOUNTS);
  const [costCenters, setCostCenters] = useState<CostCenterNode[]>(INITIAL_COST_CENTERS);
  const [journalEntries, setJournalEntries] = useState<JournalEntry[]>(INITIAL_JOURNAL_ENTRIES);
  const [vouchers, setVouchers] = useState<CashVoucher[]>(INITIAL_VOUCHERS);
  const [fiscalPeriods, setFiscalPeriods] = useState<FiscalPeriod[]>(INITIAL_FISCAL_PERIODS);
  const [warehouses, setWarehouses] = useState<Warehouse[]>(INITIAL_WAREHOUSES);
  const [stockTransfers, setStockTransfers] = useState<StockTransfer[]>(INITIAL_STOCK_TRANSFERS);
  const [stocktakes, setStocktakes] = useState<StocktakingSession[]>(INITIAL_STOCKTAKES);
  const [stockAdjustments, setStockAdjustments] = useState<StockAdjustment[]>(INITIAL_STOCK_ADJUSTMENTS);
  const [salesInvoices, setSalesInvoices] = useState<Invoice[]>(INITIAL_INVOICES);
  const [purchaseInvoices, setPurchaseInvoices] = useState<Invoice[]>(INITIAL_PURCHASE_INVOICES);
  const [heldOrders, setHeldOrders] = useState<HeldOrder[]>([]);
  const [shift, setShift] = useState<ShiftInfo>(INITIAL_SHIFT);
  const [closedShifts, setClosedShifts] = useState<ClosedShift[]>(INITIAL_CLOSED_SHIFTS);
  const [shiftMode, setShiftMode] = useState<ShiftMode>('ENTERPRISE_MULTI');
  const [users, setUsers] = useState<SystemUser[]>(INITIAL_USERS);
  const [roles, setRoles] = useState<SystemRole[]>(INITIAL_ROLES);
  const [sessions, setSessions] = useState<UserSession[]>(INITIAL_SESSIONS);
  const [currencies, setCurrencies] = useState<CurrencyItem[]>(INITIAL_CURRENCIES);
  const [currencyRates, setCurrencyRates] = useState<CurrencyRateItem[]>(INITIAL_CURRENCY_RATES);
  const [employees, setEmployees] = useState<EmployeeItem[]>(INITIAL_EMPLOYEES);

  const handleAddEmployee = (emp: Omit<EmployeeItem, 'id'>) => {
    const newId = employees.length > 0 ? Math.max(...employees.map((e) => e.id)) + 1 : 1;
    setEmployees((prev) => [...prev, { ...emp, id: newId }]);
  };

  const handleUpdateEmployee = (emp: EmployeeItem) => {
    setEmployees((prev) => prev.map((e) => (e.id === emp.id ? emp : e)));
  };

  const handleDeleteEmployee = (id: number) => {
    setEmployees((prev) => prev.filter((e) => e.id !== id));
  };

  // Authentication & Security State (Module 01 - Sprint 1.1)
  const [currentUser, setCurrentUser] = useState<SystemUser | null>(() => users[1] || users[0]);
  const [showLoginModal, setShowLoginModal] = useState<boolean>(false);
  const [isLockScreenMode, setIsLockScreenMode] = useState<boolean>(false);
  const [showChangePasswordModal, setShowChangePasswordModal] = useState<boolean>(false);

  // Modals & Multi-Platform Mode (Desktop, Web, Mobile)
  const [platformMode, setPlatformMode] = useState<'DESKTOP' | 'WEB' | 'MOBILE'>('DESKTOP');
  const [showPlatformRoadmapModal, setShowPlatformRoadmapModal] = useState<boolean>(false);
  const [showArchitectureModal, setShowArchitectureModal] = useState<boolean>(false);
  const [showCalculatorModal, setShowCalculatorModal] = useState<boolean>(false);
  const [showBackupRestoreModal, setShowBackupRestoreModal] = useState<boolean>(false);
  const [showShowDialogShowcase, setShowShowDialogShowcase] = useState<boolean>(false);
  const [printingInvoice, setPrintingInvoice] = useState<Invoice | null>(null);
  const [selectedOtherReportId, setSelectedOtherReportId] = useState<string | null>(null);

  // Conversion Transfer State (One-Click Conversion: Quotation -> Sales Invoice, PO -> Purchases)
  const [quotationForSalesInvoice, setQuotationForSalesInvoice] = useState<{
    customerName: string;
    notes?: string;
    items: Array<{
      product: ProductItem;
      quantity: number;
      unitPrice: number;
      discount?: number;
      unit?: string;
      unitFactor?: number;
      expiryDate?: string;
      notes?: string;
    }>;
  } | null>(null);

  const [orderForPurchaseInvoice, setOrderForPurchaseInvoice] = useState<{
    supplierId: string;
    notes?: string;
    items: Array<{
      product: ProductItem;
      quantity: number;
      selectedUnit: string;
      unitCost: number;
      discount?: number;
      batchNo?: string;
      expiryDate?: string;
      notes?: string;
    }>;
  } | null>(null);

  // Handle Sales Invoice Completion with FIFO (الوارد أولاً يصرف أولاً)
  const handleCompleteSale = (invoice: Invoice) => {
    let invoiceTotalCogs = 0;
    const updatedInvoiceItems = invoice.items.map((ci) => ({ ...ci }));

    // 1. Deduct inventory stocks using FIFO (الوارد أولاً يصرف أولاً)
    setProducts((prev) => {
      return prev.map((prod) => {
        const itemIdx = updatedInvoiceItems.findIndex((ci) => ci.item.id === prod.id);
        if (itemIdx > -1) {
          const soldItem = updatedInvoiceItems[itemIdx];
          const unitFactor = prod.units?.find((u) => u.name === soldItem.selectedUnit)?.factor || 1;
          const totalQtyDeducted = soldItem.quantity * unitFactor;

          const fifoResult = processFifoDeduction(prod, totalQtyDeducted, invoice.warehouseId || 'WH-01');
          invoiceTotalCogs += fifoResult.totalCogs;
          updatedInvoiceItems[itemIdx].fifoCostTotal = fifoResult.totalCogs;
          updatedInvoiceItems[itemIdx].fifoLayersDeducted = fifoResult.layersDeducted;

          return fifoResult.updatedProduct;
        }
        return prod;
      });
    });

    const enrichedInvoice: Invoice = {
      ...invoice,
      items: updatedInvoiceItems,
      fifoCostTotal: invoiceTotalCogs,
      grossProfit: invoice.grandTotal - invoiceTotalCogs,
    };

    // 2. Add invoice to sales list
    setSalesInvoices((prev) => [enrichedInvoice, ...prev]);

    // 3. Update customer balance if CREDIT or MIXED split with credit remaining
    const creditPart = invoice.splitPayments ? invoice.splitPayments.credit : (invoice.paymentType === 'CREDIT' ? invoice.remainingAmount : 0);
    if (creditPart > 0 && invoice.customerOrSupplierId) {
      setCustomers((prev) =>
        prev.map((c) =>
          c.id === invoice.customerOrSupplierId
            ? { ...c, currentBalance: c.currentBalance + creditPart }
            : c
        )
      );
    }

    // 4. Update Shift cash drawer and sales totals
    const cashPart = invoice.splitPayments ? invoice.splitPayments.cash : (invoice.paymentType === 'CASH' ? invoice.grandTotal : 0);
    const nonCashSales = invoice.grandTotal - cashPart;

    setShift((prev) => ({
      ...prev,
      currentCashInDrawer: prev.currentCashInDrawer + cashPart,
      totalSales: prev.totalSales + invoice.grandTotal,
      totalCreditSales: prev.totalCreditSales + creditPart,
    }));

    // 5. Automatically create corresponding accounting Journal Entry (ترحيل قيد اليومية آلياً بنظام FIFO وتعدد الدفع)
    const journalId = `JV-SALE-${invoice.invoiceNo}`;
    const hasDiscount = invoice.discountTotal > 0;
    const hasTax = (invoice.taxTotal || 0) > 0;
    const grossSales = hasDiscount ? invoice.subtotal : (invoice.grandTotal - (invoice.taxTotal || 0));

    const debitLines: Array<{ id: string; accountCode: string; accountName: string; debit: number; credit: number; note: string }> = [];

    if (invoice.splitPayments) {
      // Multi-tender split journal lines
      if (invoice.splitPayments.cash && invoice.splitPayments.cash > 0) {
        debitLines.push({
          id: 'L-CASH',
          accountCode: invoice.splitPayments.cashAccountCode || invoice.treasuryAccountCode || '1112',
          accountName: invoice.splitPayments.cashAccountName || invoice.treasuryAccountName || 'صندوق نقطة بيع POS كاشير',
          debit: invoice.splitPayments.cash,
          credit: 0,
          note: `سداد نقدي فاتورة ${invoice.invoiceNo}`,
        });
      }
      if (invoice.splitPayments.network && invoice.splitPayments.network > 0) {
        debitLines.push({
          id: 'L-NET',
          accountCode: invoice.splitPayments.bankAccountCode || '1121',
          accountName: invoice.splitPayments.bankAccountName || 'حساب بنك الكريمي / شبكة مدفوعات',
          debit: invoice.splitPayments.network,
          credit: 0,
          note: `سداد بنكي/شبكة فاتورة ${invoice.invoiceNo} ${invoice.splitPayments.bankRefNo ? `(مرجع: ${invoice.splitPayments.bankRefNo})` : ''}`,
        });
      }
      if (invoice.splitPayments.wallet && invoice.splitPayments.wallet > 0) {
        debitLines.push({
          id: 'L-WAL',
          accountCode: invoice.splitPayments.walletAccountCode || '1122',
          accountName: invoice.splitPayments.walletAccountName || 'محفظة جيب الإلكترونية',
          debit: invoice.splitPayments.wallet,
          credit: 0,
          note: `سداد محفظة إلكترونية فاتورة ${invoice.invoiceNo}`,
        });
      }
      if (invoice.splitPayments.credit && invoice.splitPayments.credit > 0) {
        debitLines.push({
          id: 'L-CRE',
          accountCode: '113',
          accountName: `حساب العميل (${invoice.customerOrSupplierName})`,
          debit: invoice.splitPayments.credit,
          credit: 0,
          note: `مبيعات آجلة فاتورة ${invoice.invoiceNo}`,
        });
      }
    } else {
      // Single tender
      const debitAccountCode = invoice.paymentType === 'CASH' 
        ? (invoice.treasuryAccountCode || '1112') 
        : invoice.paymentType === 'KURAYMI' 
        ? (invoice.treasuryAccountCode || '1121') 
        : invoice.paymentType === 'JEB_WALLET' 
        ? '1122' 
        : '113';
      const debitAccountName = invoice.paymentType === 'CASH' 
        ? (invoice.treasuryAccountName || 'صندوق نقطة بيع POS كاشير') 
        : invoice.paymentType === 'KURAYMI' 
        ? 'حساب بنك الكريمي للتمويل الأصغر' 
        : invoice.paymentType === 'JEB_WALLET' 
        ? 'محفظة جيب الإلكترونية' 
        : `حساب العميل (${invoice.customerOrSupplierName})`;
      
      debitLines.push({
        id: 'L1',
        accountCode: debitAccountCode,
        accountName: debitAccountName,
        debit: invoice.grandTotal,
        credit: 0,
        note: `استحقاق الفاتورة ${invoice.invoiceNo}`,
      });
    }

    const journalLines = [
      ...debitLines,
      ...(hasDiscount
        ? [
            {
              id: 'L-DISC',
              accountCode: '421',
              accountName: 'الخصم التجاري المسموح به (خصم المبيعات)',
              debit: invoice.discountTotal,
              credit: 0,
              note: `خصم تجاري على مستوى الفاتورة ${invoice.invoiceNo}`,
            },
          ]
        : []),
      {
        id: 'L2',
        accountCode: '31',
        accountName: 'إيراد مبيعات البضاعة',
        debit: 0,
        credit: grossSales,
        note: `إيراد مبيعات أصناف`,
      },
      ...(hasTax
        ? [
            {
              id: 'L-TAX',
              accountCode: '2151',
              accountName: 'أمانات مصلحة الضرائب والرسوم (VAT)',
              debit: 0,
              credit: invoice.taxTotal,
              note: `ضريبة القيمة المضافة للفاتورة ${invoice.invoiceNo}`,
            },
          ]
        : []),
      ...(invoice.otherCharges && invoice.otherCharges > 0
        ? [
            {
              id: 'L-OTHER',
              accountCode: invoice.otherChargesAccountCode || '32',
              accountName: invoice.otherChargesAccountName || invoice.otherChargesName || 'إيرادات خدمات وتوصيل ورسوم',
              debit: 0,
              credit: invoice.otherCharges,
              note: `رسوم ومصاريف (${invoice.otherChargesName || 'إضافية'}) للفاتورة ${invoice.invoiceNo}`,
            },
          ]
        : []),
    ];

    // Add FIFO Cost of Goods Sold & Inventory deduction lines if COGS > 0
    if (invoiceTotalCogs > 0) {
      journalLines.push(
        {
          id: 'L3',
          accountCode: '41',
          accountName: 'تكلفة البضاعة المباعة (FIFO COGS)',
          debit: invoiceTotalCogs,
          credit: 0,
          note: `تكلفة البضاعة المباعة بنظام FIFO للفاتورة ${invoice.invoiceNo}`,
        },
        {
          id: 'L4',
          accountCode: '114',
          accountName: 'بضاعة آخر المدة (مخزون المستودعات)',
          debit: 0,
          credit: invoiceTotalCogs,
          note: `صرف بضاعة مباعة من المخزون`,
        }
      );
    }

    const totalDebitCalc = journalLines.reduce((acc, l) => acc + l.debit, 0);
    const totalCreditCalc = journalLines.reduce((acc, l) => acc + l.credit, 0);

    const autoJournal: JournalEntry = {
      id: journalId,
      entryNo: generateJournalEntryNumber(),
      date: invoice.date,
      reference: `مبيعات فاتورة رقم ${invoice.invoiceNo}`,
      description: `إثبات مبيعات ${invoice.customerOrSupplierName} وتكلفة FIFO - ${invoice.paymentType}`,
      lines: journalLines,
      totalDebit: totalDebitCalc,
      totalCredit: totalCreditCalc,
      isBalanced: true,
      status: 'POSTED',
    };
    setJournalEntries((prev) => [autoJournal, ...prev]);
  };

  // Handle Dedicated Sales Return (سندات مردودات ومسموحات المبيعات)
  const handleProcessSalesReturn = (returnRecord: any) => {
    // 1. Return stock to warehouse if RESTOCK
    setProducts((prev) => {
      return prev.map((prod) => {
        const matchingReturnItem = returnRecord.items.find(
          (it: any) => it.product.id === prod.id && it.condition === 'RESTOCK'
        );
        if (matchingReturnItem) {
          const qtyToAdd = matchingReturnItem.returnQty * (matchingReturnItem.unitFactor || 1);
          const currentWhStock = prod.stockByWarehouse?.[returnRecord.warehouseId] || 0;
          const updatedStockByWarehouse = {
            ...(prod.stockByWarehouse || {}),
            [returnRecord.warehouseId]: currentWhStock + qtyToAdd,
          };
          return {
            ...prod,
            stock: prod.stock + qtyToAdd,
            stockByWarehouse: updatedStockByWarehouse,
          };
        }
        return prod;
      });
    });

    // 2. Customer balance reduction if CREDIT_DEDUCT
    if (returnRecord.refundMethod === 'CREDIT_DEDUCT' && returnRecord.customerId) {
      setCustomers((prev) =>
        prev.map((c) =>
          c.id === returnRecord.customerId
            ? { ...c, currentBalance: Math.max(0, c.currentBalance - returnRecord.totalRefundAmount) }
            : c
        )
      );
    }

    // 3. Create accounting journal entry
    const newJournalId = `JE-SR-${Date.now()}`;
    const creditAccountCode =
      returnRecord.refundMethod === 'CASH'
        ? '1111'
        : returnRecord.refundMethod === 'BANK'
        ? '1121'
        : '1131';
    const creditAccountName =
      returnRecord.refundMethod === 'CASH'
        ? 'الصندوق الرئيسي'
        : returnRecord.refundMethod === 'BANK'
        ? 'البنك / حساب الكريمي'
        : `العملاء - ${returnRecord.customerName}`;

    const journalLines = [
      {
        id: `L-SR-1-${Date.now()}`,
        accountCode: '412',
        accountName: 'مردودات ومسموحات المبيعات',
        debit: returnRecord.totalRefundAmount,
        credit: 0,
        note: `مردود مبيعات سند رقم ${returnRecord.returnNo} من الفاتورة ${returnRecord.originalInvoiceNo}`,
      },
      {
        id: `L-SR-2-${Date.now()}`,
        accountCode: creditAccountCode,
        accountName: creditAccountName,
        debit: 0,
        credit: returnRecord.totalRefundAmount,
        note: `رد قيمة مردود مبيعات سند رقم ${returnRecord.returnNo}`,
      },
    ];

    const newJournalEntry: JournalEntry = {
      id: newJournalId,
      entryNo: String(100 + journalEntries.length + 1),
      date: returnRecord.returnDate,
      reference: String(returnRecord.returnNo),
      description: `قيد مردود مبيعات سند رقم ${returnRecord.returnNo} (${returnRecord.customerName})`,
      lines: journalLines,
      totalDebit: returnRecord.totalRefundAmount,
      totalCredit: returnRecord.totalRefundAmount,
      isBalanced: true,
      status: 'POSTED',
      createdBy: shift.cashierName,
      createdAt: `${returnRecord.returnDate}T${returnRecord.returnTime || '12:00:00'}`,
    };

    setJournalEntries((prev) => [newJournalEntry, ...prev]);
  };

  // Handle Purchases Invoice & Returns with FIFO Layers (الوارد أولاً يصرف أولاً)
  const handleAddPurchaseInvoice = (invoice: Invoice) => {
    setPurchaseInvoices((prev) => [invoice, ...prev]);

    const isReturn = invoice.type === 'PURCHASE_RETURN';
    const targetWhId = invoice.warehouseId || 'WH-01';
    const warehouseObj = warehouses.find((w) => w.id === targetWhId);
    const invWarehouseName = warehouseObj?.name || invoice.warehouseName || 'المستودع الرئيسي';
    const supplierObj = suppliers.find((s) => s.id === invoice.customerOrSupplierId);
    const supplierAccountCode = supplierObj?.accountCode || '211001';

    // 1. Update Product Stock (Global + Warehouse specific) & FIFO Layers Queue
    setProducts((prev) =>
      prev.map((prod) => {
        const item = invoice.items.find((ci) => ci.item.id === prod.id);
        if (item) {
          const qtyDelta = isReturn ? -item.quantity : item.quantity;
          const newWhStocks = updateProductWarehouseStock(prod, targetWhId, qtyDelta);

          if (!isReturn && item.quantity > 0) {
            const withNewFifo = addFifoPurchaseLayer(prod, {
              purchaseInvoiceNo: invoice.invoiceNo,
              date: invoice.date,
              warehouseId: targetWhId,
              batchNo: item.batchNo || prod.batchNo,
              expiryDate: item.expiryDate || prod.expiryDate,
              quantity: item.quantity,
              unitCost: item.unitPrice || item.unitCost || prod.costPrice,
            });
            return {
              ...withNewFifo,
              warehouseStocks: newWhStocks,
            };
          } else {
            const returnDeduction = processFifoDeduction(prod, item.quantity, targetWhId);
            return {
              ...returnDeduction.updatedProduct,
              warehouseStocks: newWhStocks,
            };
          }
        }
        return prod;
      })
    );

    // 2. Automated Double-Entry Journal Entry
    let journalLines: any[] = [];
    const split = invoice.splitPayments;

    if (isReturn) {
      // --- Purchase Return Entry ---
      // Debit Side: Sources of Refund (Cash, Bank, Supplier Payable decrease)
      if (split && (split.cash > 0 || split.network > 0 || (split.credit && split.credit > 0) || (split.wallet && split.wallet > 0))) {
        if (split.cash > 0) {
          journalLines.push({
            id: generateNextId('JEL'),
            accountCode: split.cashAccountCode || '1111',
            accountName: split.cashAccountName || 'الصندوق الرئيسي',
            debit: split.cash,
            credit: 0,
            note: `استرداد نقدي لمردود مشتريات سند ${invoice.invoiceNo}`,
          });
        }
        if (split.network > 0) {
          journalLines.push({
            id: generateNextId('JEL'),
            accountCode: split.bankAccountCode || '1121',
            accountName: split.bankAccountName || 'حساب بنك الكريمي',
            debit: split.network,
            credit: 0,
            note: `استرداد بنكي لمردود مشتريات سند ${invoice.invoiceNo}`,
          });
        }
        if (split.wallet && split.wallet > 0) {
          journalLines.push({
            id: generateNextId('JEL'),
            accountCode: split.walletAccountCode || '1122',
            accountName: split.walletAccountName || 'محفظة إلكترونية',
            debit: split.wallet,
            credit: 0,
            note: `استرداد لمحفظة لمردود مشتريات سند ${invoice.invoiceNo}`,
          });
        }
        if (split.credit && split.credit > 0) {
          journalLines.push({
            id: generateNextId('JEL'),
            accountCode: supplierAccountCode,
            accountName: `المورد: ${invoice.customerOrSupplierName}`,
            debit: split.credit,
            credit: 0,
            note: `تخفيض ذمة المورد لمردود مشتريات سند ${invoice.invoiceNo}`,
          });
        }
      } else {
        journalLines.push({
          id: generateNextId('JEL'),
          accountCode: invoice.paymentType === 'CREDIT' ? supplierAccountCode : (invoice.paymentType === 'CASH' ? '1111' : '1121'),
          accountName: invoice.paymentType === 'CREDIT' ? `المورد: ${invoice.customerOrSupplierName}` : (invoice.paymentType === 'CASH' ? 'الصندوق الرئيسي' : 'البنك / حساب الكريمي'),
          debit: invoice.grandTotal,
          credit: 0,
          note: `تخفيض ذمة المورد / استرداد قيمة مردودات المشتريات`,
        });
      }

      // Credit Side: Inventory reduction & Tax refund
      journalLines.push({
        id: generateNextId('JEL'),
        accountCode: warehouseObj?.accountCode || '1141',
        accountName: `مخزون البضائع - ${invWarehouseName}`,
        debit: 0,
        credit: invoice.subtotal,
        note: `خصم تكلفة البضاعة المرجوعة من المخزون`,
      });

      if (invoice.discountTotal > 0) {
        journalLines.push({
          id: generateNextId('JEL'),
          accountCode: '34',
          accountName: 'الخصم المكتسب من الموردين (مرتجع)',
          debit: invoice.discountTotal,
          credit: 0,
          note: `عكس الخصم المكتسب لمردود المشتريات`,
        });
      }

      if (invoice.taxTotal > 0) {
        journalLines.push({
          id: generateNextId('JEL'),
          accountCode: '1152',
          accountName: 'أمانات ضريبة المشتريات المستردة',
          debit: 0,
          credit: invoice.taxTotal,
          note: `استرداد ضريبة القيمة المضافة على المردود`,
        });
      }

      if (invoice.otherCharges && invoice.otherCharges > 0) {
        journalLines.push({
          id: generateNextId('JEL'),
          accountCode: invoice.otherChargesAccountCode || '424',
          accountName: invoice.otherChargesAccountName || invoice.otherChargesName || 'مصاريف ورسوم مردودات مشتريات',
          debit: 0,
          credit: invoice.otherCharges,
          note: `رسوم ومصاريف مردودات (${invoice.otherChargesName || 'إضافية'}) لسند ${invoice.invoiceNo}`,
        });
      }
    } else {
      // --- Standard Purchase Invoice Entry ---
      // Debit Side: Inventory arrival & Tax receivable
      journalLines.push({
        id: generateNextId('JEL'),
        accountCode: warehouseObj?.accountCode || '1141',
        accountName: `مخزون البضائع - ${invWarehouseName}`,
        debit: invoice.subtotal,
        credit: 0,
        note: `توريد بضاعة إلى المستودع`,
      });

      if (invoice.discountTotal > 0) {
        journalLines.push({
          id: generateNextId('JEL'),
          accountCode: '34',
          accountName: 'الخصم المكتسب من الموردين',
          debit: 0,
          credit: invoice.discountTotal,
          note: `إثبات الخصم المكتسب من المورد`,
        });
      }

      if (invoice.taxTotal > 0) {
        journalLines.push({
          id: generateNextId('JEL'),
          accountCode: '1152',
          accountName: 'أمانات ضريبة القيمة المضافة على المشتريات',
          debit: invoice.taxTotal,
          credit: 0,
          note: `ضريبة القيمة المضافة للمشتريات`,
        });
      }

      if (invoice.otherCharges && invoice.otherCharges > 0) {
        journalLines.push({
          id: generateNextId('JEL'),
          accountCode: invoice.otherChargesAccountCode || '424',
          accountName: invoice.otherChargesAccountName || invoice.otherChargesName || 'مصاريف شحن ونقل مشتريات ورسوم',
          debit: invoice.otherCharges,
          credit: 0,
          note: `رسوم ومصاريف (${invoice.otherChargesName || 'إضافية'}) لفاتورة مشتريات ${invoice.invoiceNo}`,
        });
      }

      // Credit Side: Payment Sources (Cash funds, Banks, Supplier Payable)
      if (split && (split.cash > 0 || split.network > 0 || (split.credit && split.credit > 0) || (split.wallet && split.wallet > 0))) {
        if (split.cash > 0) {
          journalLines.push({
            id: generateNextId('JEL'),
            accountCode: split.cashAccountCode || '1111',
            accountName: split.cashAccountName || 'الصندوق الرئيسي',
            debit: 0,
            credit: split.cash,
            note: `سداد نقدي من الصندوق لفاتورة مشتريات ${invoice.invoiceNo}`,
          });
        }
        if (split.network > 0) {
          journalLines.push({
            id: generateNextId('JEL'),
            accountCode: split.bankAccountCode || '1121',
            accountName: split.bankAccountName || 'حساب بنك الكريمي',
            debit: 0,
            credit: split.network,
            note: `سداد بنكي/حوالة لفاتورة مشتريات ${invoice.invoiceNo}`,
          });
        }
        if (split.wallet && split.wallet > 0) {
          journalLines.push({
            id: generateNextId('JEL'),
            accountCode: split.walletAccountCode || '1122',
            accountName: split.walletAccountName || 'محفظة إلكترونية',
            debit: 0,
            credit: split.wallet,
            note: `سداد من محفظة إلكترونية لفاتورة مشتريات ${invoice.invoiceNo}`,
          });
        }
        if (split.credit && split.credit > 0) {
          journalLines.push({
            id: generateNextId('JEL'),
            accountCode: supplierAccountCode,
            accountName: `المورد: ${invoice.customerOrSupplierName}`,
            debit: 0,
            credit: split.credit,
            note: `استحقاق آجل للمورد لفاتورة مشتريات ${invoice.invoiceNo}`,
          });
        }
      } else {
        journalLines.push({
          id: generateNextId('JEL'),
          accountCode: invoice.paymentType === 'CREDIT' ? supplierAccountCode : (invoice.paymentType === 'CASH' ? '1111' : '1121'),
          accountName: invoice.paymentType === 'CREDIT' ? `المورد: ${invoice.customerOrSupplierName}` : (invoice.paymentType === 'CASH' ? 'الصندوق الرئيسي' : 'البنك / حساب الكريمي'),
          debit: 0,
          credit: invoice.grandTotal,
          note: invoice.paymentType === 'CREDIT' ? `استحقاق آجل للمورد` : `سداد فوري لقيمة المشتريات`,
        });
      }
    }

    const calculatedDebit = journalLines.reduce((acc, l) => acc + (l.debit || 0), 0);
    const calculatedCredit = journalLines.reduce((acc, l) => acc + (l.credit || 0), 0);

    const autoJournal: JournalEntry = {
      id: generateNextId('JE'),
      entryNo: generateJournalEntryNumber(),
      date: invoice.date,
      description: isReturn
        ? `إثبات مردودات مشتريات سند رقم ${invoice.invoiceNo} - المورد: ${invoice.customerOrSupplierName} (${invWarehouseName})`
        : `إثبات فاتورة مشتريات وتوريد مخزني رقم ${invoice.invoiceNo} - المورد: ${invoice.customerOrSupplierName} (${invWarehouseName})`,
      reference: invoice.referenceNo || invoice.invoiceNo,
      lines: journalLines,
      totalDebit: calculatedDebit,
      totalCredit: calculatedCredit,
      isBalanced: Math.abs(calculatedDebit - calculatedCredit) < 0.01,
      status: 'POSTED',
    };

    setJournalEntries((prev) => [autoJournal, ...prev]);

    // 3. Update Supplier Balance & Drawer Balances
    if (split) {
      const creditAmount = split.credit || 0;
      if (creditAmount > 0) {
        const balanceDelta = isReturn ? -creditAmount : creditAmount;
        setSuppliers((prev) =>
          prev.map((s) =>
            s.id === invoice.customerOrSupplierId
              ? { ...s, currentBalance: Math.max(0, s.currentBalance + balanceDelta) }
              : s
          )
        );
      }
      if (split.cash > 0) {
        const cashDelta = isReturn ? split.cash : -split.cash;
        setShift((prev) => ({
          ...prev,
          currentCashInDrawer: Math.max(0, prev.currentCashInDrawer + cashDelta),
        }));
      }
    } else {
      if (invoice.paymentType === 'CREDIT') {
        const balanceDelta = isReturn ? -invoice.grandTotal : invoice.grandTotal;
        setSuppliers((prev) =>
          prev.map((s) =>
            s.id === invoice.customerOrSupplierId
              ? { ...s, currentBalance: Math.max(0, s.currentBalance + balanceDelta) }
              : s
          )
        );
      } else if (invoice.paymentType === 'CASH') {
        const cashDelta = isReturn ? invoice.grandTotal : -invoice.grandTotal;
        setShift((prev) => ({
          ...prev,
          currentCashInDrawer: Math.max(0, prev.currentCashInDrawer + cashDelta),
        }));
      }
    }
  };

  // Hold / Recall Orders
  const handleHoldOrder = (order: HeldOrder) => {
    setHeldOrders((prev) => [order, ...prev]);
  };

  const handleRecallOrder = (orderId: string) => {
    const found = heldOrders.find((h) => h.id === orderId);
    if (found) {
      setHeldOrders((prev) => prev.filter((h) => h.id !== orderId));
      setActiveTab('POS');
      return found.items;
    }
    return [];
  };

  // Add Product
  const handleAddProduct = (item: ProductItem) => {
    setProducts((prev) => [item, ...prev]);
  };

  // Update Product
  const handleUpdateProduct = (item: ProductItem) => {
    setProducts((prev) => prev.map((p) => (p.id === item.id ? item : p)));
  };

  // Delete Product
  const handleDeleteProduct = (itemId: string) => {
    setProducts((prev) => prev.filter((p) => p.id !== itemId));
  };

  // Add Supplier
  const handleAddSupplier = (sup: Supplier) => {
    setSuppliers((prev) => [...prev, sup]);
  };

  // Add Journal Entry
  const handleAddJournalEntry = (entry: JournalEntry) => {
    setJournalEntries((prev) => [entry, ...prev]);
  };

  // Vouchers and Fiscal Periods Handlers (Sprint 1.3)
  const handleAddVoucher = (newVoucher: CashVoucher, generatedEntry: JournalEntry) => {
    setVouchers((prev) => [newVoucher, ...prev]);
    handleAddJournalEntry(generatedEntry);

    // Adjust customer or supplier balances
    if (newVoucher.partyType === 'CUSTOMER' && newVoucher.partyId) {
      setCustomers((prev) =>
        prev.map((c) => {
          if (c.id === newVoucher.partyId) {
            const diff = newVoucher.type === 'RECEIPT' ? -newVoucher.amount : newVoucher.amount;
            return { ...c, currentBalance: c.currentBalance + diff };
          }
          return c;
        })
      );
    }

    if (newVoucher.partyType === 'SUPPLIER' && newVoucher.partyId) {
      setSuppliers((prev) =>
        prev.map((s) => {
          if (s.id === newVoucher.partyId) {
            const diff = newVoucher.type === 'PAYMENT' ? -newVoucher.amount : newVoucher.amount;
            return { ...s, currentBalance: s.currentBalance + diff };
          }
          return s;
        })
      );
    }
  };

  // Next Transfer Voucher Number (Pure Integer >= 3001)
  const nextTransferVoucherNo = React.useMemo(() => {
    const nums = vouchers
      .map((v) => parseInt(v.voucherNo, 10))
      .filter((n) => !isNaN(n) && n >= 3000);
    const max = nums.length > 0 ? Math.max(...nums) : 3000;
    return max + 1;
  }, [vouchers]);

  // Shift Management Handlers (Cashier Shifts, Reconciliation & Handover)
  const handleCloseShift = (
    closedShiftRecord: ClosedShift,
    transferVoucher?: CashVoucher,
    transferEntry?: JournalEntry
  ) => {
    setClosedShifts((prev) => [closedShiftRecord, ...prev]);

    if (transferVoucher && transferEntry) {
      handleAddVoucher(transferVoucher, transferEntry);
    }

    setShift((prev) => ({
      ...prev,
      status: 'CLOSED',
      currentCashInDrawer: Math.max(0, prev.currentCashInDrawer - (closedShiftRecord.transferredAmount || 0)),
    }));
  };

  const handleOpenShift = (newShift: ShiftInfo) => {
    setShift(newShift);
  };

  // Customer & Accounts Receivable Handlers (Module 04 - Sprint 4.1)
  const handleAddCustomer = (newCustomer: Customer) => {
    setCustomers((prev) => [...prev, newCustomer]);

    // Automatically sync chart of accounts under 113
    setChartOfAccounts((prev) => {
      const addNode = (nodes: AccountNode[]): AccountNode[] => {
        return nodes.map((node) => {
          if (node.code === '113') {
            const exists = node.children?.some((c) => c.code === newCustomer.accountCode);
            if (exists) return node;

            const newChild: AccountNode = {
              code: newCustomer.accountCode,
              name: `حساب العميل: ${newCustomer.name}`,
              type: 'ASSET',
              level: 4,
              nature: 'DEBIT',
              balance: newCustomer.openingBalance || 0,
              parentCode: node.code,
              isLeaf: true,
              children: [],
            };
            return {
              ...node,
              isLeaf: false,
              children: [...(node.children || []), newChild],
            };
          }
          if (node.children && node.children.length > 0) {
            return { ...node, children: addNode(node.children) };
          }
          return node;
        });
      };
      return addNode(prev);
    });
  };

  const handleUpdateCustomer = (updatedCustomer: Customer) => {
    setCustomers((prev) => prev.map((c) => (c.id === updatedCustomer.id ? updatedCustomer : c)));
  };

  const handleDeleteCustomer = (customerId: string): { success: boolean; message: string } => {
    const cust = customers.find((c) => c.id === customerId);
    if (!cust) return { success: false, message: 'العميل غير موجود في السجلات' };
    if (cust.currentBalance !== 0) {
      return {
        success: false,
        message: `لا يمكن حذف العميل لأن رصيده ليس صفراً (${cust.currentBalance.toLocaleString()} ر.ي)`,
      };
    }
    const hasInvoices = salesInvoices.some((inv) => inv.customerOrSupplierId === customerId);
    if (hasInvoices) {
      return { success: false, message: 'لا يمكن حذف العميل لوجود فواتير مبيعات سابقة مرتبطة بسجله' };
    }
    setCustomers((prev) => prev.filter((c) => c.id !== customerId));
    return { success: true, message: 'تم حذف العميل بنجاح' };
  };

  const handleAddCustomerVoucher = (voucher: CashVoucher, allocations?: VoucherInvoiceAllocation[]) => {
    const isReceipt = voucher.type === 'RECEIPT';
    const entrySeq = journalEntries.length + 1;
    const entryNo = `JV-${entrySeq < 10 ? '00' : entrySeq < 100 ? '0' : ''}${entrySeq}`;

    // Balanced double entry
    const generatedEntry: JournalEntry = {
      id: `JV-${voucher.id}`,
      entryNo,
      date: voucher.date,
      reference: voucher.voucherNo,
      description: voucher.description,
      totalDebit: voucher.amount,
      totalCredit: voucher.amount,
      isBalanced: true,
      sourceModule: 'CUSTOMERS_AR',
      status: 'POSTED',
      createdBy: currentUser?.userName || 'cashier1',
      createdAt: voucher.createdAt || `${voucher.date} 12:00:00`,
      lines: isReceipt
        ? [
            {
              id: 'L1',
              accountCode: voucher.cashAccountCode,
              accountName: voucher.cashAccountName,
              costCenterId: voucher.costCenterId,
              debit: voucher.amount,
              credit: 0,
              note: `قبض مالي من العميل: ${voucher.partyName}`,
            },
            {
              id: 'L2',
              accountCode: voucher.contraAccountCode,
              accountName: voucher.contraAccountName,
              costCenterId: voucher.costCenterId,
              debit: 0,
              credit: voucher.amount,
              note: voucher.description,
            },
          ]
        : [
            {
              id: 'L1',
              accountCode: voucher.contraAccountCode,
              accountName: voucher.contraAccountName,
              costCenterId: voucher.costCenterId,
              debit: voucher.amount,
              credit: 0,
              note: voucher.description,
            },
            {
              id: 'L2',
              accountCode: voucher.cashAccountCode,
              accountName: voucher.cashAccountName,
              costCenterId: voucher.costCenterId,
              debit: 0,
              credit: voucher.amount,
              note: `صرف ورد مالي للعميل: ${voucher.partyName}`,
            },
          ],
    };

    const finalVoucher: CashVoucher = {
      ...voucher,
      journalEntryId: entryNo,
    };

    setVouchers((prev) => [finalVoucher, ...prev]);
    handleAddJournalEntry(generatedEntry);

    // Update customer balance
    setCustomers((prev) =>
      prev.map((c) => {
        if (c.id === voucher.partyId) {
          const diff = isReceipt ? -voucher.amount : voucher.amount;
          return { ...c, currentBalance: c.currentBalance + diff };
        }
        return c;
      })
    );

    // Update shift cash drawer if cash
    if (voucher.paymentMethod === 'CASH' && (voucher.cashAccountCode === '1111' || voucher.cashAccountCode === '1112')) {
      setShift((prev) => ({
        ...prev,
        currentCashInDrawer: isReceipt
          ? prev.currentCashInDrawer + voucher.amount
          : prev.currentCashInDrawer - voucher.amount,
      }));
    }

    // Update allocated credit invoices
    if (allocations && allocations.length > 0) {
      setSalesInvoices((prev) =>
        prev.map((inv) => {
          const alloc = allocations.find((a) => a.invoiceId === inv.id);
          if (alloc && alloc.allocatedAmount > 0) {
            const newPaid = inv.paidAmount + alloc.allocatedAmount;
            const newRemaining = Math.max(0, inv.grandTotal - newPaid);
            return {
              ...inv,
              paidAmount: newPaid,
              remainingAmount: newRemaining,
              notes: inv.notes
                ? `${inv.notes} | مسدد بالسند ${voucher.voucherNo}`
                : `مسدد بالسند ${voucher.voucherNo}`,
            };
          }
          return inv;
        })
      );
    }
  };

  const handleVoidVoucher = (voucherId: string, reason: string) => {
    setVouchers((prev) =>
      prev.map((v) =>
        v.id === voucherId
          ? { ...v, status: 'VOID' as const, voidReason: reason, voidedAt: new Date().toISOString() }
          : v
      )
    );
  };

  const handleCloseFiscalPeriod = (periodId: string, closingEntry: JournalEntry, notes: string) => {
    setFiscalPeriods((prev) =>
      prev.map((p) =>
        p.id === periodId
          ? {
              ...p,
              status: 'CLOSED' as const,
              isClosed: true,
              closedAt: new Date().toLocaleString('ar-YE'),
              closedBy: currentUser?.userName || 'مدير النظام',
              closingNotes: notes,
              closingEntryId: closingEntry.id,
            }
          : p
      )
    );
    handleAddJournalEntry(closingEntry);
  };

  const handleReopenFiscalPeriod = (periodId: string) => {
    setFiscalPeriods((prev) =>
      prev.map((p) =>
        p.id === periodId
          ? { ...p, status: 'OPEN' as const, isClosed: false }
          : p
      )
    );
  };

  // Multi-Warehouse & Stock Transfers Handlers (Module 02 - Sprint 2.1)
  const handleAddTransfer = (transfer: StockTransfer) => {
    setStockTransfers((prev) => [transfer, ...prev]);
  };

  const handleUpdateTransferStatus = (transferId: string, newStatus: StockTransferStatus, notes?: string) => {
    setStockTransfers((prev) =>
      prev.map((t) =>
        t.id === transferId
          ? {
              ...t,
              status: newStatus,
              notes: notes ? `${t.notes ? t.notes + ' | ' : ''}${notes}` : t.notes,
              ...(newStatus === 'IN_TRANSIT' ? { dispatchedAt: new Date().toLocaleString('ar-YE') } : {}),
              ...(newStatus === 'RECEIVED' ? { receivedAt: new Date().toLocaleString('ar-YE') } : {}),
            }
          : t
      )
    );
  };

  const handleAddWarehouse = (newWh: Warehouse) => {
    setWarehouses((prev) => [...prev, newWh]);
  };

  const handleUpdateWarehouse = (updatedWh: Warehouse) => {
    setWarehouses((prev) => prev.map((w) => (w.id === updatedWh.id ? updatedWh : w)));
  };

  const handleUpdateProductsStock = (updatedProducts: ProductItem[]) => {
    setProducts(updatedProducts);
  };

  // Module 04: Stocktaking & Adjustment Handlers
  const handleAddStocktake = (session: StocktakingSession) => {
    setStocktakes((prev) => [session, ...prev]);
  };

  const handleUpdateStocktake = (session: StocktakingSession) => {
    setStocktakes((prev) => prev.map((st) => (st.id === session.id ? session : st)));
  };

  const handlePostStockAdjustment = (adjustment: StockAdjustment, updatedProducts: ProductItem[]) => {
    // 1. Add adjustment voucher
    setStockAdjustments((prev) => [adjustment, ...prev]);

    // 2. Update inventory product items
    setProducts(updatedProducts);

    // 3. Automated Double-Entry Journal Entry
    const lines: JournalEntry['lines'] = [];

    // Surplus: Dr 1141 (Inventory), Cr 43 (Stock Surplus Revenue)
    if (adjustment.totalSurplusCost > 0) {
      lines.push({
        id: generateNextId('JEL'),
        accountCode: '1141',
        accountName: `مخزون البضائع - ${adjustment.warehouseName}`,
        debit: adjustment.totalSurplusCost,
        credit: 0,
        note: `إثبات فائض الجرد المخزني - سند ${adjustment.adjustmentNo}`,
      });
      lines.push({
        id: generateNextId('JEL'),
        accountCode: '43',
        accountName: 'أرباح وفائض الجرد المخزني',
        debit: 0,
        credit: adjustment.totalSurplusCost,
        note: `أرباح فروقات جردية للمستودع: ${adjustment.warehouseName}`,
      });
    }

    // Deficit: Dr 55 (Stock Deficit Expense), Cr 1141 (Inventory)
    if (adjustment.totalDeficitCost > 0) {
      lines.push({
        id: generateNextId('JEL'),
        accountCode: '55',
        accountName: 'عجز وتلفيات الجرد المخزني',
        debit: adjustment.totalDeficitCost,
        credit: 0,
        note: `إثبات عجز وتلفيات الجرد الفعلي - سند ${adjustment.adjustmentNo}`,
      });
      lines.push({
        id: generateNextId('JEL'),
        accountCode: '1141',
        accountName: `مخزون البضائع - ${adjustment.warehouseName}`,
        debit: 0,
        credit: adjustment.totalDeficitCost,
        note: `تخفيض المخزون بالعجز الفعلي للمستودع: ${adjustment.warehouseName}`,
      });
    }

    if (lines.length > 0) {
      const entryId = adjustment.journalEntryId || `JE-${adjustment.adjustmentNo}`;
      const totalDebit = lines.reduce((acc, l) => acc + l.debit, 0);
      const totalCredit = lines.reduce((acc, l) => acc + l.credit, 0);

      const newEntry: JournalEntry = {
        id: entryId,
        entryNo: entryId,
        date: adjustment.date,
        reference: adjustment.adjustmentNo,
        description: `تسوية فروقات الجرد المخزني - ${adjustment.reason}`,
        lines: lines,
        totalDebit: totalDebit,
        totalCredit: totalCredit,
        isBalanced: totalDebit === totalCredit,
        status: 'POSTED',
        createdAt: `${adjustment.date} ${adjustment.time || ''}`,
        createdBy: currentUser?.fullName || 'المسؤول الإداري',
        sourceModule: 'INVENTORY',
      };

      setJournalEntries((prev) => [newEntry, ...prev]);

      // Update Chart of Accounts balances
      setChartOfAccounts((prev) =>
        prev.map((node) => {
          let delta = 0;
          if (node.code === '1141') {
            delta = (adjustment.totalSurplusCost || 0) - (adjustment.totalDeficitCost || 0);
          } else if (node.code === '43') {
            delta = adjustment.totalSurplusCost || 0;
          } else if (node.code === '55') {
            delta = adjustment.totalDeficitCost || 0;
          }

          if (delta !== 0) {
            return {
              ...node,
              balance: node.balance + delta,
            };
          }
          return node;
        })
      );
    }
  };

  // Security & Users Handlers (Sprint 1)
  const handleAddUser = (newUser: SystemUser) => {
    setUsers((prev) => [newUser, ...prev]);
  };

  const handleUpdateUser = (updatedUser: SystemUser) => {
    setUsers((prev) => prev.map((u) => (u.id === updatedUser.id ? updatedUser : u)));
  };

  const handleToggleUserActive = (userId: string) => {
    setUsers((prev) =>
      prev.map((u) => {
        if (u.id === userId && u.roleId !== 1) {
          return { ...u, isActive: !u.isActive };
        }
        return u;
      })
    );
  };

  const handleResetUserPassword = (userId: string) => {
    setUsers((prev) =>
      prev.map((u) => {
        if (u.id === userId) {
          return {
            ...u,
            password: '1',
            mustChangePassword: true,
            loginAttempts: 0,
            isLocked: false,
            lockedUntil: null,
          };
        }
        return u;
      })
    );
  };

  const handleUnlockUser = (userId: string) => {
    setUsers((prev) =>
      prev.map((u) => {
        if (u.id === userId) {
          return {
            ...u,
            isLocked: false,
            loginAttempts: 0,
            lockedUntil: null,
          };
        }
        return u;
      })
    );
  };

  const handleUpdateUserPassword = (userId: string, newPass: string) => {
    setUsers((prev) =>
      prev.map((u) =>
        u.id === userId
          ? {
              ...u,
              password: newPass,
              mustChangePassword: false,
            }
          : u
      )
    );
    if (currentUser?.id === userId) {
      setCurrentUser((prev) => prev ? { ...prev, password: newPass, mustChangePassword: false } : null);
    }
  };

  const handleUpdateUserAttempts = (
    userId: string,
    attempts: number,
    isLocked: boolean,
    lockedUntil: string | null
  ) => {
    setUsers((prev) =>
      prev.map((u) =>
        u.id === userId
          ? {
              ...u,
              loginAttempts: attempts,
              isLocked,
              lockedUntil,
            }
          : u
      )
    );
    if (currentUser?.id === userId) {
      setCurrentUser((prev) =>
        prev
          ? {
              ...prev,
              loginAttempts: attempts,
              isLocked,
              lockedUntil,
            }
          : null
      );
    }
  };

  const handleLoginSuccess = (user: SystemUser, branchName: string, shiftNumber: number) => {
    setCurrentUser(user);
    setShift((prev) => ({
      ...prev,
      cashierName: user.fullName,
      branchName: branchName || prev.branchName,
      shiftNumber: shiftNumber || prev.shiftNumber,
    }));

    // Record session
    const newSession: UserSession = {
      id: `SESS-${Date.now().toString().slice(-4)}`,
      userId: user.id,
      userName: user.userName,
      device: 'POS-TERMINAL-01 (Windows 11 WinForms)',
      ipAddress: '192.168.1.105',
      lastActivity: 'الآن (نشطة)',
      expiresAt: new Date(Date.now() + 8 * 3600 * 1000).toLocaleTimeString('ar-YE'),
      isRevoked: false,
    };
    setSessions((prev) => [newSession, ...prev]);

    // Update user record
    setUsers((prev) =>
      prev.map((u) =>
        u.id === user.id
          ? {
              ...u,
              lastLoginAt: new Date().toLocaleDateString('ar-YE') + ' ' + new Date().toLocaleTimeString('ar-YE', { hour: '2-digit', minute: '2-digit', hour12: true }),
              loginAttempts: 0,
              isLocked: false,
              lockedUntil: null,
            }
          : u
      )
    );

    setShowLoginModal(false);
    setIsLockScreenMode(false);
  };

  const handleOpenLogin = () => {
    setIsLockScreenMode(false);
    setShowLoginModal(true);
  };

  const handleLockScreen = () => {
    setIsLockScreenMode(true);
    setShowLoginModal(true);
  };

  const handleOpenChangePassword = () => {
    setShowChangePasswordModal(true);
  };

  const handleRevokeSession = (sessionId: string) => {
    setSessions((prev) =>
      prev.map((s) => (s.id === sessionId ? { ...s, isRevoked: true } : s))
    );
  };

  const handleUpdateRolePermissions = (
    roleId: number,
    permissions: { sales: number; inventory: number; accounting: number; system: number }
  ) => {
    if (roleId === 1) return; // Super admin permissions cannot be altered
    setRoles((prev) =>
      prev.map((r) =>
        r.id === roleId
          ? {
              ...r,
              salesPermissions: permissions.sales,
              inventoryPermissions: permissions.inventory,
              accountingPermissions: permissions.accounting,
              systemPermissions: permissions.system,
            }
          : r
      )
    );
  };

  const handleAddRole = (newRole: SystemRole) => {
    setRoles((prev) => [...prev, newRole]);
  };

  // Global Keyboard shortcuts
  useEffect(() => {
    const handleGlobalKeys = (e: KeyboardEvent) => {
      // Don't override if typing in an input unless it's an F-key or Ctrl combination
      if ((e.ctrlKey || e.metaKey) && e.altKey && (e.key === 'l' || e.key === 'L')) {
        e.preventDefault();
        handleLockScreen();
      } else if ((e.ctrlKey || e.metaKey) && (e.key === 'l' || e.key === 'L')) {
        e.preventDefault();
        handleOpenLogin();
      } else if (e.key === 'F4') {
        e.preventDefault();
        setActiveTab('CUSTOMERS');
      } else if (e.key === 'F5') {
        e.preventDefault();
        setActiveTab('INVENTORY');
      } else if (e.key === 'F6') {
        e.preventDefault();
        setActiveTab('PURCHASES');
      } else if (e.key === 'F7') {
        e.preventDefault();
        setActiveTab('ACCOUNTING');
      } else if (e.key === 'F8') {
        e.preventDefault();
        setActiveTab('REPORTS');
      } else if (e.key === 'F9') {
        e.preventDefault();
        setShowCalculatorModal((prev) => !prev);
      } else if (e.key === 'F10') {
        e.preventDefault();
        setShowArchitectureModal((prev) => !prev);
      } else if ((e.ctrlKey || e.metaKey) && (e.key === 'b' || e.key === 'B')) {
        e.preventDefault();
        setShowBackupRestoreModal((prev) => !prev);
      }
    };

    window.addEventListener('keydown', handleGlobalKeys);
    return () => window.removeEventListener('keydown', handleGlobalKeys);
  }, []);

  const getTabTitle = (tab: MainTabType): string => {
    switch (tab) {
      case 'DASHBOARD': return 'محاسب سوفت - الشاشة الرئيسية';
      case 'SALES_INVOICE': return 'فاتورة مبيعات تجارية [frmSalesInvoice]';
      case 'POS': return 'شاشة نقطة البيع السريعة للكاشير [frmPOS]';
      case 'INVENTORY': return 'إدارة المخزون والأصناف والدفعات';
      case 'PURCHASES': return 'المشتريات وتوريد البضاعة والموردين';
      case 'CUSTOMERS': return 'إدارة العملاء والذمم المدينة وسندات القبض (Accounts Receivable)';
      case 'ACCOUNTING': return 'القيود اليومية ودليل الحسابات';
      case 'REPORTS': return 'التقارير المالية والأرباح P&L';
      case 'OTHER_REPORTS': return 'تقارير أخرى مصنفة (18 تقريراً تفصيلياً)';
      case 'EMPLOYEES': return 'إدارة الموظفين والكوادر البشرية [HR]';
      case 'PETTY_CASH': return 'إدارة العهد والمصروفات النثرية [Petty Cash]';
      case 'PRICE_QUOTATION': return 'عروض الأسعار للعملاء [Price Quotations]';
      case 'PURCHASE_ORDER': return 'أوامر الشراء والتوريد [Purchase Orders]';
      case 'SALES_RETURNS': return 'مردودات المبيعات وإشعار الدائن';
      case 'COMMERCIAL_PURCHASE': return 'فاتورة مشتريات تجارية [Commercial Purchase]';
      case 'SALES_STRATEGIES_HUB': return 'مختبر مقارنة استراتيجيات إدخال الأصناف الـ 6 [UX Lab]';
      case 'SALES_STRATEGY_INLINE_GRID': return 'الجدول التفاعلي المباشر';
      case 'SALES_STRATEGY_RAPID_BAR': return 'شريط الإدخال السريع';
      case 'SALES_STRATEGY_SCANNER': return 'المسح الضوئي الذكي المتواصل';
      case 'SALES_STRATEGY_SMART_F3': return 'البحث الفوري الذكي بالزر F3';
      case 'SALES_STRATEGY_TOUCH_GRID': return 'شاشة اللمس وبطاقات الأصناف';
      case 'SALES_STRATEGY_IMPORT_CONVERT': return 'التحويل والاستيراد الذكي';
      case 'MOHASEB_ITEMS': return 'دليل الأصناف وبطاقة الصنف';
      case 'EXCEL_IMPORT': return 'استيراد الأصناف من ملف إكسل';
      case 'SALES_PRICES': return 'تسعير الأصناف وقوائم الأسعار';
      case 'ITEM_UNITS': return 'وحدات قياس الأصناف ومعاملات التحويل';
      case 'MOHASEB_ACCOUNTS': return 'دليل الحسابات الشامل وكشف حساب الصناديق';
      case 'CURRENCIES': return 'دليل العملات النقدية';
      case 'CURRENCY_RATES': return 'سجل أسعار صرف العملات';
      case 'SECURITY': return 'الأمان وإدارة المستخدمين ومصفوفة الصلاحيات (RBAC)';
      case 'ARCHITECTURE': return 'الدليل الهندسي للمنظومة';
      default: return 'المحاسب الذكي';
    }
  };

  return (
    <WinFormsDialogProvider>
      <div className="flex flex-col h-screen max-h-screen bg-slate-100 text-slate-900 overflow-hidden font-sans select-none">
        {/* 1. Desktop Window Title Bar with 3-Platform Switcher */}
      <DesktopTitleBar
        shiftNumber={shift.shiftNumber}
        cashierName={shift.cashierName}
        activeTabTitle={platformMode === 'DESKTOP' ? getTabTitle(activeTab) : platformMode === 'WEB' ? 'بوابة السحابة (Web ERP Portal)' : 'تطبيق الموبايل والكفي (Mobile App)'}
        currentUser={currentUser}
        currentPlatform={platformMode}
        onChangePlatform={(p) => setPlatformMode(p)}
        onOpenPlatformRoadmap={() => setShowPlatformRoadmapModal(true)}
        onOpenArchitecture={() => setShowArchitectureModal(true)}
        onOpenCalculator={() => setShowCalculatorModal(true)}
        onOpenLogin={handleOpenLogin}
        onLockScreen={handleLockScreen}
        onChangePassword={handleOpenChangePassword}
      />

      {/* Mode 1: Web Portal Viewport */}
      {platformMode === 'WEB' && (
        <div className="flex-1 min-h-0 overflow-hidden">
          <WebPortalView
            products={products}
            customers={customers}
            suppliers={suppliers}
            salesInvoices={salesInvoices}
            purchaseInvoices={purchaseInvoices}
            vouchers={vouchers}
            warehouses={warehouses}
            chartOfAccounts={chartOfAccounts}
            currentUser={currentUser}
            currentShift={shift}
            currency={currency}
            onSwitchToDesktop={() => setPlatformMode('DESKTOP')}
            onSwitchToMobile={() => setPlatformMode('MOBILE')}
            onOpenSalesInvoice={() => {
              setPlatformMode('DESKTOP');
              setActiveTab('POS');
            }}
          />
        </div>
      )}

      {/* Mode 2: Mobile App Viewport */}
      {platformMode === 'MOBILE' && (
        <div className="flex-1 min-h-0 overflow-hidden">
          <MobileAppView
            products={products}
            customers={customers}
            salesInvoices={salesInvoices}
            vouchers={vouchers}
            currentUser={currentUser}
            currentShift={shift}
            currency={currency}
            onCompleteSale={handleCompleteSale}
            onAddVoucher={(newVoucher) => {
              // Generate a balanced journal entry for the voucher
              const entryId = `JV-VOUCH-${Date.now()}`;
              const entrySeq = journalEntries.length + 1;
              const entryNo = String(100 + entrySeq);
              const amountNum = newVoucher.amount;

              let debitLine = {
                id: 'L1',
                accountCode: newVoucher.cashAccountCode,
                accountName: newVoucher.cashAccountName,
                debit: amountNum,
                credit: 0,
                note: newVoucher.description,
              };

              let creditLine = {
                id: 'L2',
                accountCode: newVoucher.contraAccountCode,
                accountName: newVoucher.contraAccountName,
                debit: 0,
                credit: amountNum,
                note: newVoucher.description,
              };

              if (newVoucher.type === 'PAYMENT') {
                debitLine = {
                  id: 'L1',
                  accountCode: newVoucher.contraAccountCode,
                  accountName: newVoucher.contraAccountName,
                  debit: amountNum,
                  credit: 0,
                  note: newVoucher.description,
                };
                creditLine = {
                  id: 'L2',
                  accountCode: newVoucher.cashAccountCode,
                  accountName: newVoucher.cashAccountName,
                  debit: 0,
                  credit: amountNum,
                  note: newVoucher.description,
                };
              }

              const generatedEntry: JournalEntry = {
                id: entryId,
                entryNo,
                date: newVoucher.date,
                reference: `${newVoucher.voucherNo} (سند آلي ميداني)`,
                description: `${newVoucher.type === 'RECEIPT' ? 'سند قبض ميداني' : 'سند صرف ميداني'}: ${newVoucher.description} - ${newVoucher.partyName}`,
                lines: [debitLine, creditLine],
                totalDebit: amountNum,
                totalCredit: amountNum,
                isBalanced: true,
                status: 'POSTED',
              };

              handleAddVoucher(newVoucher, generatedEntry);
            }}
            onSwitchToDesktop={() => setPlatformMode('DESKTOP')}
            onSwitchToWeb={() => setPlatformMode('WEB')}
          />
        </div>
      )}

      {/* Mode 3: Traditional High-Performance Desktop App (Default) */}
      {platformMode === 'DESKTOP' && (
        <>
          {/* 2. Desktop Ribbon / Menu Bar with WinForms MenuStrip and ToolStrip */}
          <DesktopMenuBar
            activeTab={activeTab}
            onSelectTab={(tab) => {
              if (tab === 'ARCHITECTURE') {
                setShowArchitectureModal(true);
              } else {
                setActiveTab(tab);
              }
            }}
            heldOrdersCount={heldOrders.length}
            onOpenHeldOrders={() => {
              setActiveTab('POS');
            }}
            onNewPOSBill={() => {
              setActiveTab('POS');
            }}
            onHoldBill={() => {
              setActiveTab('POS');
            }}
            onQuickSearch={() => {
              setActiveTab('POS');
            }}
            onOpenCalculator={() => setShowCalculatorModal(true)}
            onOpenArchitecture={() => setShowArchitectureModal(true)}
            onPrintReport={() => window.print()}
            onOpenLogin={handleOpenLogin}
            onLockScreen={handleLockScreen}
            onOpenBackupRestore={() => setShowBackupRestoreModal(true)}
            onOpenShowDialogShowcase={() => setShowShowDialogShowcase(true)}
            onOpenReportItem={(repId) => {
              setSelectedOtherReportId(repId || null);
              setActiveTab('OTHER_REPORTS');
            }}
            currency={currency}
            drawerCash={shift.currentCashInDrawer}
          />

      {/* 3. Main Dynamic Workspace Module */}
      <main className="flex-1 flex flex-col min-h-0 bg-[#eef2f6] overflow-hidden">
        {activeTab === 'DASHBOARD' && (
          <MohasebSoftDashboard
            onNavigate={(tab, subView) => {
              setActiveTab(tab);
            }}
            warehouses={warehouses}
            currentWarehouseId={currentWarehouseId}
            onChangeWarehouse={(id) => setCurrentWarehouseId(id)}
            productsCount={products.length}
            customersCount={customers.length}
            salesCount={salesInvoices.length}
            onOpenQuickSale={() => setActiveTab('SALES_INVOICE')}
            onOpenQuickPurchase={() => setActiveTab('COMMERCIAL_PURCHASE')}
            onOpenReceiptVoucher={() => setActiveTab('CUSTOMERS')}
            onOpenPaymentVoucher={() => setActiveTab('CUSTOMERS')}
            onOpenChartOfAccounts={() => setActiveTab('MOHASEB_ACCOUNTS')}
            onOpenCashMovement={() => setActiveTab('MOHASEB_ACCOUNTS')}
            onOpenOtherReports={() => setActiveTab('OTHER_REPORTS')}
            onToggleDesktopPOSView={() => setActiveTab('POS')}
          />
        )}

        {/* Screenshot 1, 7, 8: دليل الحسابات، التصنيفات، وكشف حساب الصناديق */}
        {activeTab === 'MOHASEB_ACCOUNTS' && (
          <MohasebAccountsView
            initialSubView="CHART_OF_ACCOUNTS"
            chartOfAccounts={chartOfAccounts}
            journalEntries={journalEntries}
            onUpdateChartOfAccounts={(updated) => setChartOfAccounts(updated)}
            onAddAccount={(newAcc) => setChartOfAccounts((prev) => [...prev, newAcc])}
            onBack={() => setActiveTab('DASHBOARD')}
          />
        )}

        {/* Screenshot 2, 3: العملات وأسعار العملات */}
        {activeTab === 'CURRENCIES' && (
          <CurrenciesAndRatesView
            initialMode="CURRENCIES"
            currencies={currencies}
            currencyRates={currencyRates}
            onAddCurrency={(newCurr) => setCurrencies((prev) => [...prev, newCurr])}
            onAddCurrencyRate={(newRate) => setCurrencyRates((prev) => [newRate, ...prev])}
            onDeleteCurrencyRate={(id) => setCurrencyRates((prev) => prev.filter((r) => r.id !== id))}
            onBack={() => setActiveTab('DASHBOARD')}
          />
        )}

        {activeTab === 'CURRENCY_RATES' && (
          <CurrenciesAndRatesView
            initialMode="RATES"
            currencies={currencies}
            currencyRates={currencyRates}
            onAddCurrency={(newCurr) => setCurrencies((prev) => [...prev, newCurr])}
            onAddCurrencyRate={(newRate) => setCurrencyRates((prev) => [newRate, ...prev])}
            onDeleteCurrencyRate={(id) => setCurrencyRates((prev) => prev.filter((r) => r.id !== id))}
            onBack={() => setActiveTab('DASHBOARD')}
          />
        )}

        {/* Screenshot 4, 5, 6: قائمة الأصناف، إستيراد إكسل، وتسعير الأصناف */}
        {activeTab === 'MOHASEB_ITEMS' && (
          <MohasebProductsModuleView
            initialSubView="PRODUCTS_LIST"
            products={products}
            onAddProduct={handleAddProduct}
            onUpdateProduct={handleUpdateProduct}
            onBack={() => setActiveTab('DASHBOARD')}
          />
        )}

        {activeTab === 'EXCEL_IMPORT' && (
          <MohasebProductsModuleView
            initialSubView="EXCEL_IMPORT"
            products={products}
            onAddProduct={handleAddProduct}
            onUpdateProduct={handleUpdateProduct}
            onBack={() => setActiveTab('DASHBOARD')}
          />
        )}

        {activeTab === 'SALES_PRICES' && (
          <MohasebProductsModuleView
            initialSubView="SALES_PRICES"
            products={products}
            onAddProduct={handleAddProduct}
            onUpdateProduct={handleUpdateProduct}
            onBack={() => setActiveTab('DASHBOARD')}
          />
        )}

        {activeTab === 'ITEM_UNITS' && (
          <MohasebProductsModuleView
            initialSubView="ITEM_UNITS"
            products={products}
            onAddProduct={handleAddProduct}
            onUpdateProduct={handleUpdateProduct}
            onBack={() => setActiveTab('DASHBOARD')}
          />
        )}

        {activeTab === 'PRICE_QUOTATION' && (
          <PriceQuotationView
            products={products}
            customers={customers}
            warehouses={warehouses}
            currency={currency}
            cashierName={shift.cashierName}
            onConvertToSalesInvoice={(data) => {
              setQuotationForSalesInvoice(data);
              setActiveTab('SALES_INVOICE');
            }}
            onBack={() => setActiveTab('DASHBOARD')}
          />
        )}

        {activeTab === 'PURCHASE_ORDER' && (
          <PurchaseOrderView
            products={products}
            suppliers={suppliers}
            warehouses={warehouses}
            currency={currency}
            cashierName={shift.cashierName}
            onConvertToPurchaseInvoice={(data) => {
              setOrderForPurchaseInvoice(data);
              setActiveTab('PURCHASES');
            }}
            onBack={() => setActiveTab('DASHBOARD')}
          />
        )}

        {/* Screenshot 9, 10: فاتورة شراء آجل / نقد مع نافذة إرفاق صورة الكاميرا والهاتف */}
        {activeTab === 'COMMERCIAL_PURCHASE' && (
          <CommercialPurchaseInvoiceView
            products={products}
            suppliers={suppliers}
            warehouses={warehouses}
            currency={currency}
            cashierName={shift.cashierName}
            onSaveInvoice={handleAddPurchaseInvoice}
            onBack={() => setActiveTab('DASHBOARD')}
          />
        )}

        {activeTab === 'SALES_INVOICE' && (
          <CommercialSalesInvoiceView
            key={quotationForSalesInvoice ? `quote-conv-${quotationForSalesInvoice.customerName}-${quotationForSalesInvoice.items.length}` : 'sales-invoice'}
            products={products}
            customers={customers}
            warehouses={warehouses}
            currency={currency}
            cashierName={shift.cashierName}
            initialCustomerName={quotationForSalesInvoice?.customerName}
            initialNotes={quotationForSalesInvoice?.notes}
            initialItems={quotationForSalesInvoice?.items}
            onSaveInvoice={(inv, isDraft) => {
              if (isDraft) {
                setSalesInvoices((prev) => [inv, ...prev]);
              } else {
                handleCompleteSale(inv);
              }
            }}
            onRequestPrintReceipt={(inv) => setPrintingInvoice(inv)}
            onBackToDashboard={() => setActiveTab('DASHBOARD')}
            invoicesHistory={salesInvoices}
            defaultWarehouseId={currentUser?.defaultWarehouseId}
            defaultCashboxCode={currentUser?.defaultCashboxCode}
            onSelectTab={setActiveTab}
          />
        )}

        {activeTab === 'SALES_RETURNS' && (
          <SalesReturnsView
            products={products}
            customers={customers}
            warehouses={warehouses}
            invoicesHistory={salesInvoices}
            currency={currency}
            cashierName={shift.cashierName}
            onProcessReturn={handleProcessSalesReturn}
            onBack={() => setActiveTab('DASHBOARD')}
          />
        )}

        {activeTab === 'OTHER_REPORTS' && (
          <OtherReportsView
            products={products}
            customers={customers}
            suppliers={suppliers}
            chartOfAccounts={chartOfAccounts}
            journalEntries={journalEntries}
            salesInvoices={salesInvoices}
            purchaseInvoices={purchaseInvoices}
            vouchers={vouchers}
            warehouses={warehouses}
            currency={currency}
            initialReportId={selectedOtherReportId}
            onOpenReportItem={(repId) => setSelectedOtherReportId(repId)}
            onBack={() => {
              setSelectedOtherReportId(null);
              setActiveTab('DASHBOARD');
            }}
          />
        )}

        {activeTab === 'POS' && (
          <POSModule
            products={products}
            customers={customers}
            heldOrders={heldOrders}
            chartOfAccounts={chartOfAccounts}
            onCompleteSale={handleCompleteSale}
            onHoldOrder={handleHoldOrder}
            onRecallOrder={handleRecallOrder}
            currency={currency}
            cashierName={shift.cashierName}
            onOpenQuickProductModal={() => setActiveTab('INVENTORY')}
            onRequestPrintReceipt={(inv) => setPrintingInvoice(inv)}
            defaultCashboxCode={currentUser?.defaultCashboxCode}
            shift={shift}
            onNavigateToShiftClosing={() => setActiveTab('REPORTS')}
          />
        )}

        {activeTab === 'INVENTORY' && (
          <InventoryModule
            products={products}
            warehouses={warehouses}
            transfers={stockTransfers}
            salesInvoices={salesInvoices}
            purchaseInvoices={purchaseInvoices}
            stocktakes={stocktakes}
            stockAdjustments={stockAdjustments}
            journalEntries={journalEntries}
            onAddProduct={handleAddProduct}
            onUpdateProduct={handleUpdateProduct}
            onDeleteProduct={handleDeleteProduct}
            onAddTransfer={handleAddTransfer}
            onUpdateTransferStatus={handleUpdateTransferStatus}
            onAddWarehouse={handleAddWarehouse}
            onUpdateWarehouse={handleUpdateWarehouse}
            onUpdateProductsStock={handleUpdateProductsStock}
            onAddStocktake={handleAddStocktake}
            onUpdateStocktake={handleUpdateStocktake}
            onPostStockAdjustment={handlePostStockAdjustment}
            currency={currency}
            cashierName={shift.cashierName}
            onSelectTab={setActiveTab}
          />
        )}

        {activeTab === 'PURCHASES' && (
          <PurchasesModule
            key={orderForPurchaseInvoice ? `order-conv-${orderForPurchaseInvoice.supplierId}-${orderForPurchaseInvoice.items.length}` : 'purchases-mod'}
            suppliers={suppliers}
            products={products}
            warehouses={warehouses}
            purchaseInvoices={purchaseInvoices}
            chartOfAccounts={chartOfAccounts}
            journalEntries={journalEntries}
            onAddPurchaseInvoice={handleAddPurchaseInvoice}
            onAddSupplier={handleAddSupplier}
            currency={currency}
            cashierName={shift.cashierName}
            initialSupplierId={orderForPurchaseInvoice?.supplierId}
            initialNotes={orderForPurchaseInvoice?.notes}
            initialItems={orderForPurchaseInvoice?.items}
            onSelectTab={setActiveTab}
          />
        )}

        {activeTab === 'CUSTOMERS' && (
          <CustomersModule
            customers={customers}
            salesInvoices={salesInvoices}
            vouchers={vouchers}
            journalEntries={journalEntries}
            chartOfAccounts={chartOfAccounts}
            costCenters={costCenters}
            shift={shift}
            currentUser={currentUser}
            onAddCustomer={handleAddCustomer}
            onUpdateCustomer={handleUpdateCustomer}
            onDeleteCustomer={handleDeleteCustomer}
            onAddCustomerVoucher={handleAddCustomerVoucher}
            onSelectTab={setActiveTab}
          />
        )}

        {activeTab === 'ACCOUNTING' && (
          <AccountingModule
            chartOfAccounts={chartOfAccounts}
            costCenters={costCenters}
            journalEntries={journalEntries}
            vouchers={vouchers}
            fiscalPeriods={fiscalPeriods}
            customers={customers}
            suppliers={suppliers}
            onAddJournalEntry={handleAddJournalEntry}
            onAddVoucher={handleAddVoucher}
            onVoidVoucher={handleVoidVoucher}
            onCloseFiscalPeriod={handleCloseFiscalPeriod}
            onReopenFiscalPeriod={handleReopenFiscalPeriod}
            onUpdateChartOfAccounts={(updated) => setChartOfAccounts(updated)}
            onUpdateCostCenters={(updated) => setCostCenters(updated)}
            currency={currency}
            defaultCashboxCode={currentUser?.defaultCashboxCode}
            onSelectTab={setActiveTab}
          />
        )}

        {activeTab === 'REPORTS' && (
          <ReportsModule
            salesInvoices={salesInvoices}
            purchaseInvoices={purchaseInvoices}
            products={products}
            shift={shift}
            currency={currency}
            warehouses={warehouses}
            users={users}
            currentUser={currentUser}
            closedShifts={closedShifts}
            shiftMode={shiftMode}
            onUpdateShiftMode={(m) => setShiftMode(m)}
            onCloseShift={handleCloseShift}
            onOpenShift={handleOpenShift}
            nextTransferVoucherNo={nextTransferVoucherNo}
            onSelectTab={setActiveTab}
          />
        )}

        {activeTab === 'SECURITY' && (
          <SecurityModule
            currentUser={currentUser}
            users={users}
            roles={roles}
            sessions={sessions}
            onAddUser={handleAddUser}
            onAddRole={handleAddRole}
            onUpdateUser={handleUpdateUser}
            onToggleUserActive={handleToggleUserActive}
            onResetUserPassword={handleResetUserPassword}
            onUnlockUser={handleUnlockUser}
            onRevokeSession={handleRevokeSession}
            onUpdateRolePermissions={handleUpdateRolePermissions}
            onUpdateCurrentUserPassword={handleUpdateUserPassword}
            onTestLoginAsUser={(u) => {
              setCurrentUser(u);
              handleOpenLogin();
            }}
          />
        )}

        {activeTab === 'EMPLOYEES' && (
          <EmployeesModule
            employees={employees}
            onAddEmployee={handleAddEmployee}
            onUpdateEmployee={handleUpdateEmployee}
            onDeleteEmployee={handleDeleteEmployee}
            users={users}
            onOpenSalaryAdvanceVoucher={() => {
              setActiveTab('CUSTOMERS');
            }}
            onOpenSalaryPaymentVoucher={() => {
              setActiveTab('CUSTOMERS');
            }}
          />
        )}

        {activeTab === 'PETTY_CASH' && (
          <PettyCashModule
            onBack={() => setActiveTab('ACCOUNTING')}
            currency={currency}
            cashierName={shift.cashierName}
          />
        )}

        {/* 6 Standalone Sales Invoice Entry Strategies & UX Lab */}
        {activeTab === 'SALES_STRATEGIES_HUB' && (
          <SalesStrategiesLabView
            onSelectTab={setActiveTab}
            products={products}
            customers={customers}
          />
        )}

        {activeTab === 'SALES_STRATEGY_INLINE_GRID' && (
          <FrmSalesInvoice_InlineGrid
            products={products}
            customers={customers}
            onSelectTab={setActiveTab}
            onSaveSuccess={(invoiceNo) => alert(`تم حفظ الفاتورة بنجاح: ${invoiceNo}`)}
          />
        )}

        {activeTab === 'SALES_STRATEGY_RAPID_BAR' && (
          <FrmSalesInvoice_RapidBar
            products={products}
            customers={customers}
            onSelectTab={setActiveTab}
            onSaveSuccess={(invoiceNo) => alert(`تم حفظ الفاتورة بنجاح: ${invoiceNo}`)}
          />
        )}

        {activeTab === 'SALES_STRATEGY_SCANNER' && (
          <FrmSalesInvoice_ContinuousScanner
            products={products}
            customers={customers}
            onSelectTab={setActiveTab}
            onSaveSuccess={(invoiceNo) => alert(`تم حفظ الفاتورة بنجاح: ${invoiceNo}`)}
          />
        )}

        {activeTab === 'SALES_STRATEGY_SMART_F3' && (
          <FrmSalesInvoice_SmartF3Lookup
            products={products}
            customers={customers}
            onSelectTab={setActiveTab}
            onSaveSuccess={(invoiceNo) => alert(`تم حفظ الفاتورة بنجاح: ${invoiceNo}`)}
          />
        )}

        {activeTab === 'SALES_STRATEGY_TOUCH_GRID' && (
          <FrmSalesInvoice_TouchGrid
            products={products}
            customers={customers}
            onSelectTab={setActiveTab}
            onSaveSuccess={(invoiceNo) => alert(`تم حفظ الفاتورة بنجاح: ${invoiceNo}`)}
          />
        )}

        {activeTab === 'SALES_STRATEGY_IMPORT_CONVERT' && (
          <FrmSalesInvoice_ImportConvert
            products={products}
            customers={customers}
            onSelectTab={setActiveTab}
            onSaveSuccess={(invoiceNo) => alert(`تم حفظ الفاتورة بنجاح: ${invoiceNo}`)}
          />
        )}
      </main>

      {/* 4. Desktop Status Bar */}
      <DesktopStatusBar
        cashierName={shift.cashierName}
        shiftNumber={shift.shiftNumber}
        currency={currency}
        totalProductsCount={products.length}
        totalInvoicesCount={salesInvoices.length}
        onOpenLogin={handleOpenLogin}
      />
        </>
      )}

      {/* 3-Platform UX Comparison & Blueprint Modal */}
      <PlatformComparisonModal
        isOpen={showPlatformRoadmapModal}
        onClose={() => setShowPlatformRoadmapModal(false)}
        onSelectPlatform={(p) => setPlatformMode(p)}
      />

      {/* C# WinForms N-Layer Architecture Modal */}
      <WinFormsArchitectureModal
        isOpen={showArchitectureModal}
        onClose={() => setShowArchitectureModal(false)}
        currency={currency}
      />

      {/* Quick Calculator Modal */}
      <CalculatorModal
        isOpen={showCalculatorModal}
        onClose={() => setShowCalculatorModal(false)}
      />

      {/* SQL Server Database Backup & Restore Modal */}
      <BackupRestoreModal
        isOpen={showBackupRestoreModal}
        onClose={() => setShowBackupRestoreModal(false)}
      />

      {/* Printable Receipt Modal */}
      <ReceiptModal
        invoice={printingInvoice}
        onClose={() => setPrintingInvoice(null)}
        currency={currency}
      />

      {/* WinForms frmLogin Interactive Modal (Sprint 1.1) */}
      <LoginModal
        isOpen={showLoginModal}
        currentUser={currentUser}
        users={users}
        roles={roles}
        currentShift={shift}
        isLockScreenMode={isLockScreenMode}
        onClose={() => setShowLoginModal(false)}
        onLoginSuccess={handleLoginSuccess}
        onUnlockUser={handleUnlockUser}
        onUpdateUserPassword={handleUpdateUserPassword}
        onUpdateUserAttempts={handleUpdateUserAttempts}
      />

      {/* WinForms ShowDialog & ErrorProvider Showcase Modal */}
      <WinFormsShowDialogShowcase
        isOpen={showShowDialogShowcase}
        onClose={() => setShowShowDialogShowcase(false)}
        currency={currency}
      />

      {/* User Self-Service Change Password Modal */}
      {showChangePasswordModal && currentUser && (
        <ChangePasswordModal
          isOpen={showChangePasswordModal}
          user={currentUser}
          isMandatory={false}
          onClose={() => setShowChangePasswordModal(false)}
          onPasswordChanged={(userId, newPass) => {
            handleUpdateUserPassword(userId, newPass);
            setShowChangePasswordModal(false);
            alert('تم تغيير وتحديث كلمة المرور بنجاح.');
          }}
        />
      )}
    </div>
    </WinFormsDialogProvider>
  );
}
