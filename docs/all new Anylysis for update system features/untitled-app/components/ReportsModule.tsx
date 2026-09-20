'use client';

import React, { useState, useMemo } from 'react';
import { Invoice, ProductItem, ShiftInfo, Warehouse, ClosedShift, ShiftMode, CashVoucher, JournalEntry, SystemUser, MainTabType } from '@/types/pos';
import { INITIAL_WAREHOUSES } from '@/lib/mock/warehouses';
import { INITIAL_CLOSED_SHIFTS } from '@/lib/mock/shifts';
import { getProductStockInWarehouse } from '@/lib/mock/inventoryUtils';
import { generateNextId } from '@/lib/idGenerator';
import { ZReportModal } from './pos/ZReportModal';
import { OpenShiftModal } from './pos/OpenShiftModal';
import { 
  BarChart3, 
  TrendingUp, 
  DollarSign, 
  CreditCard, 
  Coins, 
  Package, 
  Calendar, 
  PieChart as PieIcon, 
  ArrowUpRight, 
  Printer,
  ShieldAlert,
  Building2,
  Store,
  Filter,
  CheckCircle2,
  Play,
  History,
  ArrowDownLeft,
  AlertTriangle,
  Sparkles,
  User,
  RefreshCw,
  Layers,
  Check,
  Calculator,
  FileText,
  Clock,
  ChevronLeft,
  ChevronRight,
  Users,
  Shield,
  Cpu,
} from 'lucide-react';
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
  Legend
} from 'recharts';

interface ReportsModuleProps {
  salesInvoices: Invoice[];
  purchaseInvoices: Invoice[];
  products: ProductItem[];
  shift: ShiftInfo;
  currency: string;
  warehouses?: Warehouse[];
  users?: SystemUser[];
  currentUser?: SystemUser | null;
  closedShifts?: ClosedShift[];
  shiftMode?: ShiftMode;
  onUpdateShiftMode?: (mode: ShiftMode) => void;
  onCloseShift?: (closedShift: ClosedShift, transferVoucher?: CashVoucher, transferEntry?: JournalEntry) => void;
  onOpenShift?: (newShift: ShiftInfo) => void;
  nextTransferVoucherNo?: number;
  onSelectTab?: (tab: MainTabType) => void;
}

export const ReportsModule: React.FC<ReportsModuleProps> = ({
  salesInvoices,
  purchaseInvoices,
  products,
  shift,
  currency,
  warehouses = INITIAL_WAREHOUSES,
  users = [],
  currentUser = null,
  closedShifts = INITIAL_CLOSED_SHIFTS,
  shiftMode = 'ENTERPRISE_MULTI',
  onUpdateShiftMode,
  onCloseShift,
  onOpenShift,
  nextTransferVoucherNo = 3002,
  onSelectTab,
}) => {
  const [selectedReportView, setSelectedReportView] = useState<'OVERVIEW' | 'SALES_ANALYSIS' | 'INCOME_STATEMENT' | 'SHIFT_CLOSING'>('OVERVIEW');
  const [shiftSubTab, setShiftSubTab] = useState<'ACTIVE_SHIFT' | 'SHIFT_HISTORY'>('ACTIVE_SHIFT');
  const [showShiftPreview, setShowShiftPreview] = useState<boolean>(false);

  // Shift closing state
  const [actualDrawerCashInput, setActualDrawerCashInput] = useState<string>('');
  const [transferToTreasury, setTransferToTreasury] = useState<boolean>(true);
  const [transferTargetAccount, setTransferTargetAccount] = useState<string>('1111 - الخزينة المركزية (الصندوق الرئيسي)');
  const [transferAmountChoice, setTransferAmountChoice] = useState<'ALL_ACTUAL' | 'SALES_ONLY' | 'CUSTOM'>('ALL_ACTUAL');
  const [customTransferAmount, setCustomTransferAmount] = useState<string>('');
  const [transferNotes, setTransferNotes] = useState<string>('');

  // Quick denomination calculator
  const [showDenomCalc, setShowDenomCalc] = useState<boolean>(false);
  const [denom1000, setDenom1000] = useState<number>(0);
  const [denom500, setDenom500] = useState<number>(0);
  const [denom250, setDenom250] = useState<number>(0);
  const [denom100, setDenom100] = useState<number>(0);

  // Modals state
  const [showOpenShiftModal, setShowOpenShiftModal] = useState<boolean>(false);
  const [viewingZReportShift, setViewingZReportShift] = useState<ClosedShift | null>(null);

  // Cascading Branch & Warehouse Filter State
  const [selectedBranchId, setSelectedBranchId] = useState<number | 'ALL'>('ALL');
  const [selectedWarehouseId, setSelectedWarehouseId] = useState<string>('ALL');

  // Extract unique branches from warehouses
  const branchesList = useMemo(() => {
    const map = new Map<number, string>();
    warehouses.forEach(w => {
      if (w.branchId) {
        map.set(w.branchId, w.branchName || `فرع ${w.branchId}`);
      }
    });
    return Array.from(map.entries()).map(([id, name]) => ({ id, name }));
  }, [warehouses]);

  // Available warehouses cascading based on selected branch
  const availableWarehouses = useMemo(() => {
    if (selectedBranchId === 'ALL') {
      return warehouses;
    }
    return warehouses.filter(w => w.branchId === selectedBranchId);
  }, [warehouses, selectedBranchId]);

  // Handle branch change: resets warehouse to 'ALL' of that branch
  const handleBranchChange = (branchVal: string) => {
    if (branchVal === 'ALL') {
      setSelectedBranchId('ALL');
    } else {
      setSelectedBranchId(Number(branchVal));
    }
    setSelectedWarehouseId('ALL');
  };

  // Filter Sales Invoices by Branch and Warehouse
  const filteredSalesInvoices = useMemo(() => {
    return salesInvoices.filter(inv => {
      const invWh = inv.warehouseId || 'WH-01';
      if (selectedWarehouseId !== 'ALL') {
        return invWh === selectedWarehouseId;
      }
      if (selectedBranchId !== 'ALL') {
        const whObj = warehouses.find(w => w.id === invWh);
        return whObj ? whObj.branchId === selectedBranchId : false;
      }
      return true;
    });
  }, [salesInvoices, selectedBranchId, selectedWarehouseId, warehouses]);

  // Filter Purchase Invoices by Branch and Warehouse
  const filteredPurchaseInvoices = useMemo(() => {
    return purchaseInvoices.filter(inv => {
      const invWh = inv.warehouseId || 'WH-01';
      if (selectedWarehouseId !== 'ALL') {
        return invWh === selectedWarehouseId;
      }
      if (selectedBranchId !== 'ALL') {
        const whObj = warehouses.find(w => w.id === invWh);
        return whObj ? whObj.branchId === selectedBranchId : false;
      }
      return true;
    });
  }, [purchaseInvoices, selectedBranchId, selectedWarehouseId, warehouses]);

  // Calculations based on filtered invoices
  const totalSalesRevenue = filteredSalesInvoices.reduce((sum, inv) => sum + inv.grandTotal, 0);
  const totalPurchasesCost = filteredPurchaseInvoices.reduce((sum, inv) => sum + inv.grandTotal, 0);
  const totalCashSales = filteredSalesInvoices.filter((i) => i.paymentType === 'CASH').reduce((sum, i) => sum + i.grandTotal, 0);
  const totalCreditSales = filteredSalesInvoices.filter((i) => i.paymentType === 'CREDIT').reduce((sum, i) => sum + i.grandTotal, 0);
  
  // Cost of goods sold for sales invoices
  const estimatedCOGS = filteredSalesInvoices.reduce((sum, inv) => {
    return sum + inv.items.reduce((itemSum, item) => itemSum + (item.quantity * item.unitCost), 0);
  }, 0);

  const estimatedGrossProfit = totalSalesRevenue - estimatedCOGS;
  const grossProfitMargin = totalSalesRevenue > 0 ? (estimatedGrossProfit / totalSalesRevenue) * 100 : 0;

  // Chart Data: Sales by Category
  const categorySalesMap: Record<string, number> = {};
  filteredSalesInvoices.forEach((inv) => {
    inv.items.forEach((item) => {
      const cat = item.item.category || 'أخرى';
      categorySalesMap[cat] = (categorySalesMap[cat] || 0) + item.total;
    });
  });

  const categoryChartData = Object.keys(categorySalesMap).map((cat) => ({
    name: cat,
    value: categorySalesMap[cat],
  }));

  // Chart Data: Top Products
  const productSalesMap: Record<string, { name: string; total: number; qty: number }> = {};
  filteredSalesInvoices.forEach((inv) => {
    inv.items.forEach((item) => {
      if (!productSalesMap[item.item.name]) {
        productSalesMap[item.item.name] = { name: item.item.name, total: 0, qty: 0 };
      }
      productSalesMap[item.item.name].total += item.total;
      productSalesMap[item.item.name].qty += item.quantity;
    });
  });

  const topProductsChartData = Object.values(productSalesMap)
    .sort((a, b) => b.total - a.total)
    .slice(0, 5);

  const PIE_COLORS = ['#0284c7', '#10b981', '#f59e0b', '#8b5cf6', '#ec4899', '#64748b'];

  // Current shift sales metrics
  const shiftCashSales = useMemo(() => {
    return filteredSalesInvoices.reduce((sum, inv) => {
      if (inv.splitPayments?.cash) return sum + inv.splitPayments.cash;
      if (inv.paymentType === 'CASH') return sum + inv.grandTotal;
      return sum;
    }, 0);
  }, [filteredSalesInvoices]);

  const shiftCardSales = useMemo(() => {
    return filteredSalesInvoices.reduce((sum, inv) => {
      if (inv.splitPayments?.network) return sum + inv.splitPayments.network;
      if (inv.paymentType === 'KURAYMI' || inv.paymentType === 'MIXED') return sum + inv.grandTotal;
      return sum;
    }, 0);
  }, [filteredSalesInvoices]);

  const shiftCreditSales = useMemo(() => {
    return filteredSalesInvoices.reduce((sum, inv) => {
      if (inv.remainingAmount > 0) return sum + inv.remainingAmount;
      if (inv.paymentType === 'CREDIT') return sum + inv.grandTotal;
      return sum;
    }, 0);
  }, [filteredSalesInvoices]);

  const shiftTotalSales = filteredSalesInvoices.reduce((sum, inv) => sum + inv.grandTotal, 0);

  // Expected cash in drawer: Opening float + Cash collected from sales
  const expectedDrawerCash = shift.openingCash + shiftCashSales;

  // Denomination counter total
  const denomTotal = denom1000 * 1000 + denom500 * 500 + denom250 * 250 + denom100 * 100;

  // Actual counted cash: manual input takes precedence if typed, otherwise denom count, otherwise fallback
  const effectiveActualCash = actualDrawerCashInput !== '' 
    ? (parseFloat(actualDrawerCashInput) || 0) 
    : (denomTotal > 0 ? denomTotal : (shift.currentCashInDrawer || expectedDrawerCash));

  const cashDifference = effectiveActualCash - (shift.currentCashInDrawer || expectedDrawerCash);

  // Amount to transfer to central treasury
  const calculatedTransferAmount = useMemo(() => {
    if (!transferToTreasury) return 0;
    if (transferAmountChoice === 'ALL_ACTUAL') {
      return effectiveActualCash;
    }
    if (transferAmountChoice === 'SALES_ONLY') {
      return Math.max(0, effectiveActualCash - shift.openingCash);
    }
    return parseFloat(customTransferAmount) || 0;
  }, [transferToTreasury, transferAmountChoice, effectiveActualCash, shift.openingCash, customTransferAmount]);

  // Handle shift closing
  const handleConfirmCloseShift = () => {
    const now = new Date();
    const dateStr = now.toISOString().split('T')[0];
    const timeStr = now.toLocaleTimeString('ar-YE', { hour: '2-digit', minute: '2-digit' });
    const closedAt = `${dateStr} ${timeStr}`;

    const voucherNo = nextTransferVoucherNo.toString();
    const entryId = generateNextId(`JV-TR-${shift.shiftNumber}`);

    let generatedVoucher: CashVoucher | undefined;
    let generatedEntry: JournalEntry | undefined;

    if (transferToTreasury && calculatedTransferAmount > 0) {
      generatedEntry = {
        id: entryId,
        entryNo: (100 + nextTransferVoucherNo).toString(),
        date: dateStr,
        reference: `إقفال وردية #${shift.shiftNumber}`,
        description: `تحويل وتوريد نقدية إغلاق الوردية #${shift.shiftNumber} للكاشير (${shift.cashierName}) إلى الخزينة المركزية`,
        lines: [
          {
            id: generateNextId('line'),
            accountCode: '1111',
            accountName: 'الخزينة المركزية (الصندوق الرئيسي)',
            debit: calculatedTransferAmount,
            credit: 0,
            note: `استلام وتوريد نقدية الوردية #${shift.shiftNumber}`,
          },
          {
            id: generateNextId('line'),
            accountCode: '1112',
            accountName: `صندوق الكاشير (${shift.cashierName})`,
            debit: 0,
            credit: calculatedTransferAmount,
            note: `تسليم نقدية الدرج بنهاية الوردية #${shift.shiftNumber}`,
          },
        ],
        totalDebit: calculatedTransferAmount,
        totalCredit: calculatedTransferAmount,
        isBalanced: true,
        status: 'POSTED',
        createdBy: shift.cashierName,
        createdAt: closedAt,
        sourceModule: 'SHIFT_CLOSING',
      };

      generatedVoucher = {
        id: generateNextId('VOUCH-TR'),
        voucherNo,
        type: 'TRANSFER',
        paymentMethod: 'CASH',
        date: dateStr,
        amount: calculatedTransferAmount,
        currency,
        cashAccountCode: '1112',
        cashAccountName: `صندوق الكاشير (${shift.cashierName})`,
        partyType: 'OTHER',
        partyName: `الخزينة المركزية (استلام عهدة الوردية #${shift.shiftNumber})`,
        contraAccountCode: '1111',
        contraAccountName: 'الخزينة المركزية (الصندوق الرئيسي)',
        reference: `إقفال الوردية #${shift.shiftNumber}`,
        description: transferNotes || `تسليم وتوريد نقدية إغلاق الوردية #${shift.shiftNumber} للكاشير ${shift.cashierName}`,
        status: 'POSTED',
        journalEntryId: entryId,
        createdBy: shift.cashierName,
        createdAt: closedAt,
      };
    }

    const closedShiftRecord: ClosedShift = {
      id: `CSHIFT-${shift.shiftNumber}`,
      shiftNumber: shift.shiftNumber,
      mode: shiftMode,
      cashierId: shift.cashierId,
      cashierName: shift.cashierName,
      branchName: shift.branchName || 'الفرع الرئيسي',
      openedAt: shift.openedAt,
      closedAt,
      openingCash: shift.openingCash,
      expectedCash: shift.currentCashInDrawer || expectedDrawerCash,
      actualCash: effectiveActualCash,
      cashDifference,
      totalSales: shiftTotalSales,
      cashSales: shiftCashSales,
      cardSales: shiftCardSales,
      creditSales: shiftCreditSales,
      totalReturns: shift.totalReturns || 0,
      invoiceCount: filteredSalesInvoices.length,
      transferVoucherNo: transferToTreasury ? voucherNo : undefined,
      transferToAccount: transferToTreasury ? transferTargetAccount : undefined,
      transferredAmount: transferToTreasury ? calculatedTransferAmount : 0,
      notes: transferNotes || (cashDifference === 0 ? 'جرد مطابق تماماً وتوريد النقدية' : `جرد مع فارق: ${cashDifference > 0 ? 'فائض' : 'عجز'}`),
      closedBy: shift.cashierName,
    };

    onCloseShift?.(closedShiftRecord, generatedVoucher, generatedEntry);
    setViewingZReportShift(closedShiftRecord);
    setActualDrawerCashInput('');
  };

  // Selected scope human text
  const selectedScopeLabel = useMemo(() => {
    if (selectedBranchId === 'ALL' && selectedWarehouseId === 'ALL') {
      return 'كافة الفروع والمستودعات (تقرير مالي مجمّع)';
    }
    const branchName = selectedBranchId === 'ALL' 
      ? 'كافة الفروع' 
      : branchesList.find(b => b.id === selectedBranchId)?.name || `فرع ${selectedBranchId}`;
    const warehouseName = selectedWarehouseId === 'ALL'
      ? (selectedBranchId === 'ALL' ? 'كافة المستودعات' : 'كافة مستودعات هذا الفرع')
      : warehouses.find(w => w.id === selectedWarehouseId)?.name || selectedWarehouseId;

    return `${branchName} ⟵ [ ${warehouseName} ]`;
  }, [selectedBranchId, selectedWarehouseId, branchesList, warehouses]);

  return (
    <div className="flex-1 flex flex-col h-full bg-[#f8fafc] text-slate-800 p-2 gap-2 select-none overflow-hidden font-sans">
      {/* 1. Phase 5 Operational Stage Ribbon (شريط المرحلة الخامسة: التقارير الشاملة وإدارة الموظفين والنظام) */}
      <div className="bg-white border border-slate-200 px-3 py-1.5 rounded-lg flex flex-wrap items-center justify-between gap-2 text-xs shadow-xs shrink-0 text-slate-800">
        {/* Left: Phase Title & Workflow Steps Navigation */}
        <div className="flex items-center gap-2 overflow-x-auto">
          {/* Phase Badge */}
          <div className="flex items-center gap-1.5 bg-purple-700 text-white px-2.5 py-0.5 rounded-full border border-purple-800 shadow-xs">
            <span className="w-4 h-4 rounded-full bg-white text-purple-900 flex items-center justify-center font-black text-[10px]">
              5
            </span>
            <span className="font-bold text-[11px]">المرحلة الخامسة: التقارير الشاملة، الموظفين، والأمان</span>
          </div>

          <div className="h-4 w-px bg-slate-200" />

          {/* Direct Switchers between Phase 5 Tabs */}
          <div className="flex items-center gap-1">
            <button
              type="button"
              onClick={() => setSelectedReportView('OVERVIEW')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                selectedReportView === 'OVERVIEW'
                  ? 'bg-purple-700 text-white border border-purple-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="لوحة المؤشرات العامة والرقابة اليومية"
            >
              <BarChart3 className="w-3 h-3 text-purple-500" />
              <span>لوحة المؤشرات</span>
            </button>

            <button
              type="button"
              onClick={() => setSelectedReportView('SALES_ANALYSIS')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                selectedReportView === 'SALES_ANALYSIS'
                  ? 'bg-purple-700 text-white border border-purple-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="تحليل ربحية ومبيعات الأصناف والمجموعات"
            >
              <TrendingUp className="w-3 h-3 text-sky-600" />
              <span>تحليل المبيعات</span>
            </button>

            <button
              type="button"
              onClick={() => setSelectedReportView('INCOME_STATEMENT')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                selectedReportView === 'INCOME_STATEMENT'
                  ? 'bg-purple-700 text-white border border-purple-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="قائمة الدخل والأرباح والخسائر P&L"
            >
              <DollarSign className="w-3 h-3 text-emerald-600" />
              <span>قائمة الدخل P&L</span>
            </button>

            {onSelectTab && (
              <>
                <button
                  type="button"
                  onClick={() => onSelectTab('OTHER_REPORTS')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="مكتبة التقارير المحاسبية والمخزنية المتخصصة (18 تقريراً)"
                >
                  <FileText className="w-3 h-3 text-teal-600" />
                  <span>تقارير أخرى مصنفة (18)</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('EMPLOYEES')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="إدارة شؤون الموظفين، الرواتب، السلف، والعمولات [Ctrl+E]"
                >
                  <Users className="w-3 h-3 text-amber-600" />
                  <span>دليل الموظفين والكوادر</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('SECURITY')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="مصفوفة الصلاحيات وإدارة المستخدمين والرقابة"
                >
                  <Shield className="w-3 h-3 text-rose-600" />
                  <span>الصلاحيات والأمان</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('ARCHITECTURE')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="المعمارية الهندسية ومخطط التكامل C# WinForms"
                >
                  <Cpu className="w-3 h-3 text-cyan-600" />
                  <span>المعمارية الفنية</span>
                </button>
              </>
            )}
          </div>
        </div>

        {/* Right: Architectural Rules & Prev/Next Stage Shortcuts */}
        <div className="flex items-center gap-2">
          {/* Rules Indicators */}
          <div className="hidden xl:flex items-center gap-1 text-[10px] text-slate-600">
            <span className="bg-purple-50 text-purple-700 px-1.5 py-0.5 rounded border border-purple-200 font-mono font-bold">
              ✓ الموظف ≠ المستخدم
            </span>
            <span className="bg-emerald-50 text-emerald-700 px-1.5 py-0.5 rounded border border-emerald-200 font-bold">
              ✓ ربط البائعين بالمبيعات
            </span>
            <span className="bg-sky-50 text-sky-700 px-1.5 py-0.5 rounded border border-sky-200 font-bold">
              ✓ تقارير مجمعة ومتعددة
            </span>
          </div>

          {onSelectTab && (
            <div className="flex items-center gap-1">
              <button
                type="button"
                onClick={() => onSelectTab('ACCOUNTING')}
                className="bg-slate-100 hover:bg-slate-200 text-slate-700 text-[11px] font-bold px-2 py-1 rounded flex items-center gap-1 transition border border-slate-200 cursor-pointer"
                title="الرجوع إلى المرحلة الرابعة: الحسابات العامة ودفتر الأستاذ"
              >
                <ChevronRight className="w-3.5 h-3.5" />
                <span>المرحلة 4: الحسابات</span>
              </button>

              <button
                type="button"
                onClick={() => onSelectTab('SALES_INVOICE')}
                className="bg-emerald-600 hover:bg-emerald-500 text-white text-[11px] font-bold px-2.5 py-1 rounded flex items-center gap-1 transition shadow-xs border border-emerald-700 cursor-pointer"
                title="العودة إلى بداية الدورة: المرحلة الأولى (المبيعات ونقاط البيع)"
              >
                <span>المرحلة 1: المبيعات [F1]</span>
                <ChevronLeft className="w-3.5 h-3.5" />
              </button>
            </div>
          )}
        </div>
      </div>

      {/* Sub Tabs */}
      <div className="bg-white p-2 rounded-lg border border-slate-200 shadow-xs flex items-center justify-between">
        <div className="flex items-center gap-2">
          <button
            onClick={() => setSelectedReportView('OVERVIEW')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              selectedReportView === 'OVERVIEW'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <BarChart3 className="w-3.5 h-3.5" />
            <span>لوحة المؤشرات العامة (Dashboard)</span>
          </button>

          <button
            onClick={() => setSelectedReportView('SALES_ANALYSIS')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              selectedReportView === 'SALES_ANALYSIS'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <TrendingUp className="w-3.5 h-3.5" />
            <span>تحليل المبيعات والأصناف</span>
          </button>

          <button
            onClick={() => setSelectedReportView('INCOME_STATEMENT')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              selectedReportView === 'INCOME_STATEMENT'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <DollarSign className="w-3.5 h-3.5" />
            <span>قائمة الدخل والأرباح (P&L)</span>
          </button>

          <button
            onClick={() => setSelectedReportView('SHIFT_CLOSING')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-2 cursor-pointer ${
              selectedReportView === 'SHIFT_CLOSING'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <Coins className="w-3.5 h-3.5 text-amber-500" />
            <span>إدارة الورديات وجرد الصناديق</span>
            <span className="bg-amber-100 text-amber-900 border border-amber-300 text-[10px] font-black px-1.5 py-0.5 rounded leading-none">
              قريباً (Coming Soon)
            </span>
          </button>
        </div>

        <button
          onClick={() => window.print()}
          className="bg-slate-700 hover:bg-slate-800 text-white text-xs font-semibold px-3 py-1.5 rounded flex items-center gap-1 cursor-pointer shadow-xs"
        >
          <Printer className="w-3.5 h-3.5" />
          <span>طباعة التقرير</span>
        </button>
      </div>

      {/* Cascading Branch & Warehouse Filter Bar */}
      <div className="bg-white text-slate-800 p-2.5 rounded-lg border border-slate-200 shadow-xs flex flex-wrap items-center justify-between gap-3">
        <div className="flex flex-wrap items-center gap-3">
          <div className="flex items-center gap-1.5 text-slate-700 text-xs font-bold shrink-0">
            <Filter className="w-4 h-4 text-sky-600" />
            <span>نطاق تصفية التقارير:</span>
          </div>

          {/* 1. Branch Filter Dropdown */}
          <div className="flex items-center gap-1.5 bg-slate-50 border border-slate-200 rounded-md px-2 py-1">
            <Building2 className="w-3.5 h-3.5 text-sky-600" />
            <label className="text-[11px] text-slate-600 font-semibold">الفرع:</label>
            <select
              value={selectedBranchId === 'ALL' ? 'ALL' : selectedBranchId.toString()}
              onChange={(e) => handleBranchChange(e.target.value)}
              className="bg-white text-slate-800 text-xs font-bold rounded px-2 py-0.5 border border-slate-300 focus:outline-none focus:border-sky-500 cursor-pointer"
            >
              <option value="ALL">🏢 كافة الفروع (مجمّع المركز العام)</option>
              {branchesList.map((b) => (
                <option key={b.id} value={b.id.toString()}>
                  🏢 {b.name}
                </option>
              ))}
            </select>
          </div>

          {/* 2. Warehouse Cascading Filter Dropdown */}
          <div className="flex items-center gap-1.5 bg-slate-50 border border-slate-200 rounded-md px-2 py-1">
            <Store className="w-3.5 h-3.5 text-amber-600" />
            <label className="text-[11px] text-slate-600 font-semibold">المستودع:</label>
            <select
              value={selectedWarehouseId}
              onChange={(e) => setSelectedWarehouseId(e.target.value)}
              className="bg-white text-slate-800 text-xs font-bold rounded px-2 py-0.5 border border-slate-300 focus:outline-none focus:border-amber-500 cursor-pointer max-w-xs"
            >
              <option value="ALL">
                {selectedBranchId === 'ALL' ? '🏬 كافة المستودعات (كل الفروع)' : '🏬 كافة مستودعات الفرع المختار'}
              </option>
              {availableWarehouses.map((w) => (
                <option key={w.id} value={w.id}>
                  🏬 {w.name} ({w.code})
                </option>
              ))}
            </select>
          </div>

          {/* Reset Filters Quick Button if filtered */}
          {(selectedBranchId !== 'ALL' || selectedWarehouseId !== 'ALL') && (
            <button
              onClick={() => {
                setSelectedBranchId('ALL');
                setSelectedWarehouseId('ALL');
              }}
              className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200 text-[11px] font-bold px-2 py-1 rounded transition cursor-pointer flex items-center gap-1"
            >
              <span>عرض المجمّع العام</span>
            </button>
          )}
        </div>

        {/* Current Scope Badge */}
        <div className="flex items-center gap-1.5 bg-sky-50 border border-sky-200 text-sky-800 px-2.5 py-1 rounded-md text-[11px] font-medium">
          <CheckCircle2 className="w-3.5 h-3.5 text-emerald-600" />
          <span>النطاق النشط:</span>
          <span className="font-bold text-sky-950 font-mono-numbers">{selectedScopeLabel}</span>
          <span className="bg-sky-200 text-sky-900 px-1.5 py-0.2 rounded text-[10px] font-bold mr-1">
            {filteredSalesInvoices.length} فاتورة
          </span>
        </div>
      </div>

      {/* Main Container */}
      <div className="flex-1 min-h-0 bg-white rounded-lg border border-slate-300 shadow-sm overflow-hidden flex flex-col">
        {/* VIEW 1: OVERVIEW DASHBOARD */}
        {selectedReportView === 'OVERVIEW' && (
          <div className="flex-1 overflow-y-auto p-3 flex flex-col gap-3">
            {/* KPI Cards Row */}
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-3 text-xs">
              <div className="bg-gradient-to-br from-sky-50 to-sky-100/80 p-3 rounded-lg border border-sky-300 flex flex-col justify-between">
                <div className="flex items-center justify-between text-sky-800 font-semibold mb-1">
                  <span>إجمالي المبيعات (Sales)</span>
                  <BarChart3 className="w-4 h-4 text-sky-700" />
                </div>
                <div className="text-xl font-black text-sky-950 font-mono-numbers">
                  {totalSalesRevenue.toLocaleString()} {currency}
                </div>
                <span className="text-[11px] text-sky-700 mt-1 font-medium">
                  {filteredSalesInvoices.length} فواتير بيع منجزة
                </span>
              </div>

              <div className="bg-gradient-to-br from-emerald-50 to-emerald-100/80 p-3 rounded-lg border border-emerald-300 flex flex-col justify-between">
                <div className="flex items-center justify-between text-emerald-800 font-semibold mb-1">
                  <span>إجمالي الربح التقديري</span>
                  <TrendingUp className="w-4 h-4 text-emerald-700" />
                </div>
                <div className="text-xl font-black text-emerald-950 font-mono-numbers">
                  {estimatedGrossProfit.toLocaleString()} {currency}
                </div>
                <span className="text-[11px] text-emerald-700 mt-1 font-medium">
                  هامش الربح: {grossProfitMargin.toFixed(1)}%
                </span>
              </div>

              <div className="bg-gradient-to-br from-amber-50 to-amber-100/80 p-3 rounded-lg border border-amber-300 flex flex-col justify-between">
                <div className="flex items-center justify-between text-amber-800 font-semibold mb-1">
                  <span>نقدية الصندوق الفعلية</span>
                  <Coins className="w-4 h-4 text-amber-700" />
                </div>
                <div className="text-xl font-black text-amber-950 font-mono-numbers">
                  {shift.currentCashInDrawer.toLocaleString()} {currency}
                </div>
                <span className="text-[11px] text-amber-700 mt-1 font-medium">
                  المستخدم: {shift.cashierName}
                </span>
              </div>

              <div className="bg-gradient-to-br from-purple-50 to-purple-100/80 p-3 rounded-lg border border-purple-300 flex flex-col justify-between">
                <div className="flex items-center justify-between text-purple-800 font-semibold mb-1">
                  <span>مبيعات آجلة (ذمم عملاء)</span>
                  <CreditCard className="w-4 h-4 text-purple-700" />
                </div>
                <div className="text-xl font-black text-purple-950 font-mono-numbers">
                  {totalCreditSales.toLocaleString()} {currency}
                </div>
                <span className="text-[11px] text-purple-700 mt-1 font-medium">
                  مستحقة التحصيل
                </span>
              </div>
            </div>

            {/* Charts Grid */}
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-3 flex-1 min-h-[300px]">
              {/* Top Products Bar Chart */}
              <div className="border border-slate-300 rounded-lg p-3 flex flex-col bg-slate-50">
                <h4 className="font-bold text-xs text-slate-800 mb-2 flex items-center gap-1.5">
                  <Package className="w-4 h-4 text-sky-700" />
                  <span>الأصناف الأكثر مبيعاً في النطاق المحدد ({currency})</span>
                </h4>
                <div className="flex-1 h-60">
                  <ResponsiveContainer width="100%" height="100%">
                    <BarChart data={topProductsChartData} layout="vertical">
                      <CartesianGrid strokeDasharray="3 3" horizontal={false} />
                      <XAxis type="number" tickFormatter={(val) => `${val / 1000}k`} />
                      <YAxis type="category" dataKey="name" width={110} tick={{ fontSize: 10 }} />
                      <Tooltip formatter={(value: any) => [`${Number(value).toLocaleString()} ${currency}`, 'المبيعات']} />
                      <Bar dataKey="total" fill="#0284c7" radius={[0, 4, 4, 0]} />
                    </BarChart>
                  </ResponsiveContainer>
                </div>
              </div>

              {/* Sales By Category Pie Chart */}
              <div className="border border-slate-300 rounded-lg p-3 flex flex-col bg-slate-50">
                <h4 className="font-bold text-xs text-slate-800 mb-2 flex items-center gap-1.5">
                  <PieIcon className="w-4 h-4 text-emerald-700" />
                  <span>توزيع المبيعات حسب التصنيفات في النطاق</span>
                </h4>
                <div className="flex-1 h-60">
                  <ResponsiveContainer width="100%" height="100%">
                    <PieChart>
                      <Pie
                        data={categoryChartData.length > 0 ? categoryChartData : [{ name: 'مواد غذائية', value: 100 }]}
                        cx="50%"
                        cy="50%"
                        innerRadius={45}
                        outerRadius={75}
                        paddingAngle={3}
                        dataKey="value"
                        label={({ name, percent }) => `${name} (${((percent || 0) * 100).toFixed(0)}%)`}
                      >
                        {categoryChartData.map((_, index) => (
                          <Cell key={`cell-${index}`} fill={PIE_COLORS[index % PIE_COLORS.length]} />
                        ))}
                      </Pie>
                      <Tooltip formatter={(value: any) => [`${Number(value).toLocaleString()} ${currency}`, 'الإيراد']} />
                    </PieChart>
                  </ResponsiveContainer>
                </div>
              </div>
            </div>
          </div>
        )}

        {/* VIEW 2: SALES INVOICES ANALYSIS */}
        {selectedReportView === 'SALES_ANALYSIS' && (
          <div className="flex-1 overflow-auto">
            <table className="w-full text-right border-collapse datagrid-table">
              <thead className="sticky top-0">
                <tr>
                  <th className="w-24">رقم الفاتورة</th>
                  <th className="w-24">التاريخ والوقت</th>
                  <th>العميل</th>
                  <th className="w-28">المستودع والفرع</th>
                  <th className="w-20">طريقة الدفع</th>
                  <th className="w-20 text-center">عدد الأصناف</th>
                  <th className="w-24 text-left">المجموع ({currency})</th>
                  <th className="w-20 text-left">الخصم</th>
                  <th className="w-28 text-left">الصافي ({currency})</th>
                  <th className="w-24">الكاشير</th>
                </tr>
              </thead>
              <tbody>
                {filteredSalesInvoices.length > 0 ? (
                  filteredSalesInvoices.map((inv) => {
                    const whObj = warehouses.find(w => w.id === (inv.warehouseId || 'WH-01'));
                    return (
                      <tr key={inv.id} className="border-b border-slate-200 hover:bg-sky-50/50">
                        <td className="font-mono-numbers font-bold text-sky-800">{inv.invoiceNo}</td>
                        <td className="font-mono-numbers text-xs text-slate-600">{inv.date} {inv.time}</td>
                        <td className="font-bold text-slate-900">{inv.customerOrSupplierName}</td>
                        <td>
                          <span className="bg-sky-100 text-sky-900 text-[10px] px-2 py-0.5 rounded font-bold inline-flex items-center gap-1">
                            <span>{whObj?.name || inv.warehouseName || 'المستودع الرئيسي'}</span>
                          </span>
                        </td>
                        <td>
                          <span className="bg-slate-200 text-slate-800 text-[11px] px-2 py-0.5 rounded font-semibold">
                            {inv.paymentType}
                          </span>
                        </td>
                        <td className="text-center font-mono-numbers">{inv.items.length}</td>
                        <td className="text-left font-mono-numbers">{inv.subtotal.toLocaleString()}</td>
                        <td className="text-left font-mono-numbers text-rose-600">
                          {inv.discountTotal > 0 ? inv.discountTotal.toLocaleString() : '-'}
                        </td>
                        <td className="text-left font-mono-numbers font-black text-slate-900">
                          {inv.grandTotal.toLocaleString()}
                        </td>
                        <td className="text-xs text-slate-600">{inv.cashierName}</td>
                      </tr>
                    );
                  })
                ) : (
                  <tr>
                    <td colSpan={10} className="p-8 text-center text-slate-500 font-bold">
                      لا توجد فواتير مبيعات مسجلة في الفرع أو المستودع المختار ({selectedScopeLabel})
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        )}

        {/* VIEW 3: INCOME STATEMENT P&L */}
        {selectedReportView === 'INCOME_STATEMENT' && (
          <div className="flex-1 overflow-y-auto p-4 flex flex-col items-center">
            <div className="w-full max-w-2xl bg-white border border-slate-300 rounded-xl shadow-sm p-5 text-xs flex flex-col gap-4">
              <div className="text-center border-b border-slate-200 pb-3">
                <h3 className="text-base font-bold text-slate-900">قائمة الدخل التقديرية (الأرباح والخسائر - P&L)</h3>
                <span className="text-slate-500">الفترة المالية الحالية | العملة: ريال يمني ({currency})</span>
              </div>

              <div className="flex flex-col gap-2">
                {/* Revenues */}
                <div className="flex items-center justify-between py-1.5 border-b border-slate-200 font-bold text-slate-900">
                  <span>إيرادات المبيعات الإجمالية (Gross Sales Revenue)</span>
                  <span className="font-mono-numbers text-sm text-emerald-800">
                    +{totalSalesRevenue.toLocaleString()} {currency}
                  </span>
                </div>

                {/* Cost of Goods Sold */}
                <div className="flex items-center justify-between py-1.5 border-b border-slate-200 text-slate-700">
                  <span>تكلفة البضاعة المباعة (Cost of Goods Sold - COGS)</span>
                  <span className="font-mono-numbers font-bold text-rose-700">
                    -{estimatedCOGS.toLocaleString()} {currency}
                  </span>
                </div>

                {/* Gross Profit */}
                <div className="flex items-center justify-between py-2 bg-emerald-50 px-3 rounded font-extrabold text-emerald-950 border border-emerald-200">
                  <span>إجمالي هامش الربح التجاري (Gross Profit)</span>
                  <span className="font-mono-numbers text-base">
                    {estimatedGrossProfit.toLocaleString()} {currency}
                  </span>
                </div>

                {/* Operational Expenses Mock */}
                <div className="flex items-center justify-between py-1.5 border-b border-slate-200 text-slate-600">
                  <span>مصروفات تشغيلية وخدمات تقديرية</span>
                  <span className="font-mono-numbers font-semibold text-rose-700">-15,000 {currency}</span>
                </div>

                {/* Net Income */}
                <div className="flex items-center justify-between py-3 bg-emerald-50 text-emerald-950 border border-emerald-300 px-4 rounded-lg font-black text-sm mt-2 shadow-2xs">
                  <span className="text-emerald-900">صافي الربح التقديري (Estimated Net Income):</span>
                  <span className="font-mono-numbers text-lg text-emerald-800">
                    {(estimatedGrossProfit - 15000).toLocaleString()} {currency}
                  </span>
                </div>
              </div>
            </div>
          </div>
        )}

        {/* VIEW 4: SHIFT CLOSING (COMING SOON / ROADMAP V2) */}
        {selectedReportView === 'SHIFT_CLOSING' && !showShiftPreview && (
          <div className="flex-1 overflow-y-auto p-4 sm:p-8 flex flex-col items-center justify-center text-xs bg-slate-100/70">
            <div className="max-w-3xl w-full bg-white rounded-2xl border border-slate-300 shadow-sm p-6 sm:p-8 flex flex-col gap-6">
              {/* Coming Soon Badge & Header */}
              <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 pb-5 border-b border-slate-200">
                <div className="flex items-center gap-3">
                  <div className="w-12 h-12 rounded-xl bg-amber-100 border border-amber-200 flex items-center justify-center text-amber-800">
                    <Coins className="w-6 h-6" />
                  </div>
                  <div>
                    <div className="flex items-center gap-2">
                      <h3 className="font-extrabold text-slate-900 text-lg">
                        إدارة الورديات وجرد الصناديق (Shift Management)
                      </h3>
                      <span className="bg-amber-100 text-amber-900 border border-amber-300 text-[11px] font-black px-2.5 py-0.5 rounded-full">
                        قريباً (Coming Soon - V2)
                      </span>
                    </div>
                    <p className="text-slate-500 text-xs mt-1">
                      ميزة مجدولة للتطوير والإطلاق في الإصدارات القادمة المخصصة للمؤسسات والسوبرماركت.
                    </p>
                  </div>
                </div>

                <span className="self-start sm:self-auto bg-slate-100 text-slate-700 font-bold px-3 py-1 rounded-lg border border-slate-300 text-[11px]">
                  خارطة الطريق: الإصدار 2.0
                </span>
              </div>

              {/* Current Version Direct Sales Notice */}
              <div className="bg-emerald-50 border border-emerald-200 rounded-xl p-4 flex items-start gap-3">
                <div className="w-7 h-7 rounded-full bg-emerald-600 text-white flex items-center justify-center shrink-0 mt-0.5 font-bold text-xs">
                  ✓
                </div>
                <div>
                  <h4 className="font-bold text-emerald-950 text-xs sm:text-sm">
                    البيع المباشر الفوري في الإصدار الحالي (V1)
                  </h4>
                  <p className="text-emerald-800 text-xs mt-1 leading-relaxed">
                    تم تصميم الإصدار الحالي ليعمل بسلاسة وسرعة فائقة دون اشتراط فتح أو إغلاق وردية؛ حيث تسجل المبيعات مباشرة في نقطة البيع وتُرحل آلياً إلى صندوق الكاشير الافتراضي وحساب المبيعات والمخزن المختار دون تعقيد أو تعطيل.
                  </p>
                </div>
              </div>

              {/* Roadmap Features Grid */}
              <div>
                <h4 className="font-bold text-slate-800 text-xs mb-3 flex items-center gap-1.5">
                  <span>الميزات المجدولة في نظام الورديات المستقبلي:</span>
                </h4>
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                  <div className="bg-slate-50 border border-slate-200 rounded-xl p-3.5 flex flex-col gap-1">
                    <span className="font-bold text-slate-800 text-xs flex items-center gap-1.5">
                      <span className="w-2 h-2 rounded-full bg-sky-600"></span>
                      تعدد الكاشيرات والورديات 24/7
                    </span>
                    <p className="text-slate-600 text-[11px] leading-relaxed">
                      إدارة ورديات مستقلة ومتعاقبة للموظفين مع تسجيل العهدة الافتتاحية لكل كاشير.
                    </p>
                  </div>

                  <div className="bg-slate-50 border border-slate-200 rounded-xl p-3.5 flex flex-col gap-1">
                    <span className="font-bold text-slate-800 text-xs flex items-center gap-1.5">
                      <span className="w-2 h-2 rounded-full bg-amber-600"></span>
                      حاسبة الفئات النقدية ومطابقة العجز
                    </span>
                    <p className="text-slate-600 text-[11px] leading-relaxed">
                      جرد فئات العملة ومقارنة النقدية الفعلية مع مبيعات النظام واحتساب الفوارق فوراً.
                    </p>
                  </div>

                  <div className="bg-slate-50 border border-slate-200 rounded-xl p-3.5 flex flex-col gap-1">
                    <span className="font-bold text-slate-800 text-xs flex items-center gap-1.5">
                      <span className="w-2 h-2 rounded-full bg-emerald-600"></span>
                      التوريد المحاسبي الآلي للخزينة
                    </span>
                    <p className="text-slate-600 text-[11px] leading-relaxed">
                      توليد سندات تحويل نقدية وقيود يومية مزدوجة متوازنة لترحيل مبالغ الوردية للخزينة الرئيسية.
                    </p>
                  </div>

                  <div className="bg-slate-50 border border-slate-200 rounded-xl p-3.5 flex flex-col gap-1">
                    <span className="font-bold text-slate-800 text-xs flex items-center gap-1.5">
                      <span className="w-2 h-2 rounded-full bg-indigo-600"></span>
                      تقارير Z-Report وأرشيف الإغلاقات
                    </span>
                    <p className="text-slate-600 text-[11px] leading-relaxed">
                      طباعة تقرير إقفال شامل معتمد لكل وردية مع أرشيف تاريخي لمراجعة وتدقيق الورديات المقفلة.
                    </p>
                  </div>
                </div>
              </div>

              {/* Actions Footer */}
              <div className="flex flex-wrap items-center justify-between gap-3 pt-4 border-t border-slate-200">
                <button
                  type="button"
                  onClick={() => setSelectedReportView('OVERVIEW')}
                  className="bg-sky-800 hover:bg-sky-700 text-white font-bold px-4 py-2 rounded-lg text-xs transition cursor-pointer"
                >
                  العودة إلى لوحة المؤشرات (Dashboard)
                </button>

                <button
                  type="button"
                  onClick={() => setShowShiftPreview(true)}
                  className="text-slate-600 hover:text-slate-900 border border-slate-300 hover:bg-slate-100 font-semibold px-3.5 py-2 rounded-lg text-xs transition cursor-pointer flex items-center gap-1.5"
                >
                  <span>استعراض النموذج الأولي التجريبي (Preview Sandbox)</span>
                  <span className="text-[10px] bg-slate-200 text-slate-700 px-1.5 py-0.5 rounded">تجريبي</span>
                </button>
              </div>
            </div>
          </div>
        )}

        {/* VIEW 4 (EXPERIMENTAL PREVIEW): SHIFT CLOSING */}
        {selectedReportView === 'SHIFT_CLOSING' && showShiftPreview && (
          <div className="flex-1 overflow-y-auto p-3 sm:p-4 flex flex-col gap-3 text-xs bg-slate-100/70">
            {/* Experimental Notice Banner */}
            <div className="bg-amber-50 border border-amber-300 rounded-xl p-3 flex items-center justify-between gap-3 text-amber-900">
              <div className="flex items-center gap-2">
                <span className="text-base">⚠️</span>
                <span className="font-bold">أنت تستعرض حالياً النموذج الأولي التجريبي لنظام الورديات (المجدول للإصدار القادم V2 - Coming Soon).</span>
              </div>
              <button
                type="button"
                onClick={() => setShowShiftPreview(false)}
                className="bg-amber-800 hover:bg-amber-700 text-white font-bold px-3 py-1.5 rounded-lg text-xs transition cursor-pointer shrink-0"
              >
                العودة لصفحة خارطة الطريق
              </button>
            </div>

            {/* Operational Mode Header Banner */}
            <div className="bg-white p-3 sm:p-4 rounded-xl border border-slate-300 shadow-sm flex flex-wrap items-center justify-between gap-3">
              <div>
                <div className="flex items-center gap-2">
                  <span className="font-bold text-slate-900 text-sm">إدارة الورديات وجرد الصناديق (Cashier Shifts & Z-Report)</span>
                  <span className={`px-2 py-0.5 rounded text-[11px] font-bold ${
                    shiftMode === 'DAILY_SIMPLE' ? 'bg-amber-100 text-amber-900 border border-amber-300' : 'bg-sky-100 text-sky-900 border border-sky-300'
                  }`}>
                    {shiftMode === 'DAILY_SIMPLE' ? '🟢 النمط اليومي البسيط (المتاجر الصغيرة)' : '🏢 النمط المؤسسي المتعدد (السوبرماركت والمؤسسات)'}
                  </span>
                </div>
                <p className="text-[11px] text-slate-500 mt-0.5">
                  {shiftMode === 'DAILY_SIMPLE'
                    ? 'نظام مبيعات يومي مبسط يناسب المتاجر ذات الكاشير الواحد أو صاحب المحل دون الحاجة لتحاسب بين موظفين متعددين.'
                    : 'نظام مؤسسي كامل متعدد الكاشيرات والورديات، مع عهدة افتتاحية لكل كاشير، وجرد الصندوق، وتوليد سند تحويل آلي للخزينة الرئيسية.'}
                </p>
              </div>

              {/* Mode Toggle Button */}
              {onUpdateShiftMode && (
                <div className="flex items-center gap-2 bg-slate-100 p-1 rounded-lg border border-slate-300">
                  <button
                    type="button"
                    onClick={() => onUpdateShiftMode('DAILY_SIMPLE')}
                    className={`px-3 py-1.5 rounded text-xs font-bold transition cursor-pointer flex items-center gap-1.5 ${
                      shiftMode === 'DAILY_SIMPLE'
                        ? 'bg-amber-600 text-white shadow-xs'
                        : 'text-slate-600 hover:text-slate-900'
                    }`}
                    title="النمط اليومي للمتاجر الصغيرة والبقالات"
                  >
                    <Store className="w-3.5 h-3.5" />
                    <span>المتجر الصغير (يومي)</span>
                  </button>

                  <button
                    type="button"
                    onClick={() => onUpdateShiftMode('ENTERPRISE_MULTI')}
                    className={`px-3 py-1.5 rounded text-xs font-bold transition cursor-pointer flex items-center gap-1.5 ${
                      shiftMode === 'ENTERPRISE_MULTI'
                        ? 'bg-sky-700 text-white shadow-xs'
                        : 'text-slate-600 hover:text-slate-900'
                    }`}
                    title="النمط المؤسسي المتعدد للسوبرماركت والشركات"
                  >
                    <Layers className="w-3.5 h-3.5" />
                    <span>المؤسسات والسوبرماركت</span>
                  </button>
                </div>
              )}
            </div>

            {/* Inner Sub Tabs & Actions */}
            <div className="flex flex-wrap items-center justify-between gap-2 border-b border-slate-300 pb-2">
              <div className="flex items-center gap-2">
                <button
                  type="button"
                  onClick={() => setShiftSubTab('ACTIVE_SHIFT')}
                  className={`px-3 py-1.5 rounded-lg font-bold text-xs transition cursor-pointer flex items-center gap-1.5 ${
                    shiftSubTab === 'ACTIVE_SHIFT'
                      ? 'bg-sky-800 text-white shadow-xs'
                      : 'bg-white text-slate-700 hover:bg-slate-100 border border-slate-200'
                  }`}
                >
                  <Coins className="w-3.5 h-3.5" />
                  <span>الوردية الحالية وجرد الصندوق</span>
                  {shift.status !== 'CLOSED' && (
                    <span className="w-2 h-2 rounded-full bg-emerald-400 animate-pulse"></span>
                  )}
                </button>

                <button
                  type="button"
                  onClick={() => setShiftSubTab('SHIFT_HISTORY')}
                  className={`px-3 py-1.5 rounded-lg font-bold text-xs transition cursor-pointer flex items-center gap-1.5 ${
                    shiftSubTab === 'SHIFT_HISTORY'
                      ? 'bg-sky-800 text-white shadow-xs'
                      : 'bg-white text-slate-700 hover:bg-slate-100 border border-slate-200'
                  }`}
                >
                  <History className="w-3.5 h-3.5" />
                  <span>سجل وأرشيف الورديات المقفلة ({closedShifts.length})</span>
                </button>
              </div>

              {/* Open Shift Modal Trigger */}
              <button
                type="button"
                onClick={() => setShowOpenShiftModal(true)}
                className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-3.5 py-1.5 rounded-lg text-xs flex items-center gap-1.5 shadow transition cursor-pointer"
              >
                <Play className="w-3.5 h-3.5" />
                <span>{shift.status === 'CLOSED' ? 'بدء وردية جديدة الآن' : 'فتح وردية لكاشير آخر'}</span>
              </button>
            </div>

            {/* SUB-TAB 1: ACTIVE SHIFT & HANDOVER */}
            {shiftSubTab === 'ACTIVE_SHIFT' && (
              <div className="flex flex-col gap-3">
                {/* Notice if shift is closed */}
                {shift.status === 'CLOSED' ? (
                  <div className="bg-white p-6 rounded-xl border-2 border-dashed border-amber-300 shadow-sm flex flex-col items-center justify-center text-center gap-3 my-4">
                    <div className="p-3 bg-amber-100 text-amber-800 rounded-full">
                      <Clock className="w-8 h-8" />
                    </div>
                    <div>
                      <h4 className="font-bold text-slate-900 text-sm">الوردية رقم #{shift.shiftNumber} مقفلة حالياً</h4>
                      <p className="text-slate-500 text-xs mt-1 max-w-md">
                        تم اعتماد إقفال هذه الوردية وتوريد مبالغها المحصلة. لا يمكن تسجيل مبيعات جديدة حتى فتح وردية جديدة.
                      </p>
                    </div>
                    <div className="flex items-center gap-2 mt-2">
                      <button
                        type="button"
                        onClick={() => setShowOpenShiftModal(true)}
                        className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-4 py-2 rounded-lg text-xs flex items-center gap-1.5 shadow cursor-pointer"
                      >
                        <Play className="w-4 h-4" />
                        <span>فتح وردية جديدة</span>
                      </button>
                      {closedShifts.length > 0 && (
                        <button
                          type="button"
                          onClick={() => setViewingZReportShift(closedShifts[0])}
                          className="bg-sky-700 hover:bg-sky-600 text-white font-bold px-4 py-2 rounded-lg text-xs flex items-center gap-1.5 shadow cursor-pointer"
                        >
                          <FileText className="w-4 h-4" />
                          <span>معاينة Z-Report لآخر وردية</span>
                        </button>
                      )}
                    </div>
                  </div>
                ) : (
                  <div className="grid grid-cols-1 lg:grid-cols-12 gap-3">
                    {/* Right Column: Shift Details & Sales Summary */}
                    <div className="lg:col-span-6 flex flex-col gap-3">
                      {/* Shift Card */}
                      <div className="bg-white rounded-xl border border-slate-300 shadow-sm p-4 flex flex-col gap-3">
                        <div className="flex items-center justify-between border-b border-slate-200 pb-2">
                          <div className="flex items-center gap-2">
                            <span className="bg-sky-800 text-white font-mono font-bold px-2 py-0.5 rounded text-xs">
                              الوردية #{shift.shiftNumber}
                            </span>
                            <span className="font-bold text-slate-800 text-sm">البيانات التشغيلية الحالية</span>
                          </div>
                          <span className="bg-emerald-100 text-emerald-800 font-bold px-2 py-0.5 rounded-full text-[10px] flex items-center gap-1">
                            <span className="w-1.5 h-1.5 rounded-full bg-emerald-600"></span>
                            نشطة (OPEN)
                          </span>
                        </div>

                        <div className="grid grid-cols-2 gap-2 text-[11px]">
                          <div className="bg-slate-50 p-2 rounded border border-slate-200">
                            <span className="text-slate-500 block">الكاشير المسؤول:</span>
                            <span className="font-bold text-slate-900 text-xs truncate block">{shift.cashierName}</span>
                          </div>

                          <div className="bg-slate-50 p-2 rounded border border-slate-200">
                            <span className="text-slate-500 block">الفرع / نقطة البيع:</span>
                            <span className="font-bold text-slate-900 text-xs">{shift.branchName || 'الفرع الرئيسي'}</span>
                          </div>

                          <div className="bg-slate-50 p-2 rounded border border-slate-200">
                            <span className="text-slate-500 block">توقيت الفتح:</span>
                            <span className="font-mono font-bold text-slate-800 text-xs">{shift.openedAt}</span>
                          </div>

                          <div className="bg-slate-50 p-2 rounded border border-slate-200">
                            <span className="text-slate-500 block">العهدة الافتتاحية (الفكة):</span>
                            <span className="font-mono font-bold text-sky-800 text-xs">
                              {shift.openingCash.toLocaleString()} {currency}
                            </span>
                          </div>
                        </div>
                      </div>

                      {/* Sales Revenue Breakdown Card */}
                      <div className="bg-white rounded-xl border border-slate-300 shadow-sm p-4 flex flex-col gap-3">
                        <div className="flex items-center justify-between border-b border-slate-200 pb-2">
                          <span className="font-bold text-slate-800">إيرادات المبيعات المحققة بالوردية</span>
                          <span className="text-[11px] text-slate-500">
                            الفواتير الصادرة: <strong>{filteredSalesInvoices.length}</strong>
                          </span>
                        </div>

                        <div className="grid grid-cols-3 gap-2 text-center">
                          <div className="bg-emerald-50 p-2.5 rounded-lg border border-emerald-200">
                            <span className="text-[10px] text-emerald-800 font-bold block">كاش (نقداً)</span>
                            <span className="font-mono font-black text-emerald-800 text-sm">
                              {shiftCashSales.toLocaleString()}
                            </span>
                            <span className="text-[9px] text-emerald-600 block">{currency}</span>
                          </div>

                          <div className="bg-sky-50 p-2.5 rounded-lg border border-sky-200">
                            <span className="text-[10px] text-sky-800 font-bold block">شبكة / بطاقات</span>
                            <span className="font-mono font-black text-sky-800 text-sm">
                              {shiftCardSales.toLocaleString()}
                            </span>
                            <span className="text-[9px] text-sky-600 block">{currency}</span>
                          </div>

                          <div className="bg-amber-50 p-2.5 rounded-lg border border-amber-200">
                            <span className="text-[10px] text-amber-800 font-bold block">آجل (ذمم)</span>
                            <span className="font-mono font-black text-amber-800 text-sm">
                              {shiftCreditSales.toLocaleString()}
                            </span>
                            <span className="text-[9px] text-amber-600 block">{currency}</span>
                          </div>
                        </div>

                        <div className="flex items-center justify-between bg-slate-50 p-2.5 rounded-lg border border-slate-200 font-bold">
                          <span className="text-slate-700">إجمالي المبيعات الشاملة:</span>
                          <span className="font-mono text-slate-900 text-sm font-black">
                            {shiftTotalSales.toLocaleString()} {currency}
                          </span>
                        </div>
                      </div>
                    </div>

                    {/* Left Column: Cash Drawer Audit & Treasury Handover */}
                    <div className="lg:col-span-6 flex flex-col gap-3">
                      {/* Drawer Reconciliation & Denomination Box */}
                      <div className="bg-white rounded-xl border border-slate-300 shadow-sm p-4 flex flex-col gap-3">
                        <div className="flex items-center justify-between border-b border-slate-200 pb-2">
                          <div className="flex items-center gap-1.5">
                            <Coins className="w-4 h-4 text-emerald-700" />
                            <span className="font-bold text-slate-900">جرد ومطابقة الدرج النقدي</span>
                          </div>
                          <button
                            type="button"
                            onClick={() => setShowDenomCalc(!showDenomCalc)}
                            className="bg-slate-100 hover:bg-slate-200 text-slate-700 text-[11px] font-bold px-2 py-1 rounded border border-slate-300 flex items-center gap-1 cursor-pointer"
                          >
                            <Calculator className="w-3.5 h-3.5" />
                            <span>{showDenomCalc ? 'إخفاء حاسبة الفئات' : 'حاسبة فئات النقد'}</span>
                          </button>
                        </div>

                        {/* Denomination Counter Drawer */}
                        {showDenomCalc && (
                          <div className="bg-slate-50 border border-slate-300 rounded-lg p-3 flex flex-col gap-2">
                            <span className="font-bold text-slate-800 text-[11px]">
                              عدّ الأوراق النقدية (أدخل عدد الأوراق لكل فئة):
                            </span>
                            <div className="grid grid-cols-2 sm:grid-cols-4 gap-2">
                              <div>
                                <label className="text-[10px] text-slate-600 font-bold block">فئة 1000:</label>
                                <input
                                  type="number"
                                  min="0"
                                  value={denom1000 || ''}
                                  onChange={(e) => setDenom1000(parseInt(e.target.value) || 0)}
                                  className="w-full bg-white border border-slate-300 rounded p-1 font-mono text-center font-bold text-xs"
                                  placeholder="0"
                                />
                                <span className="text-[9px] text-slate-500 text-center block mt-0.5">
                                  ={(denom1000 * 1000).toLocaleString()}
                                </span>
                              </div>

                              <div>
                                <label className="text-[10px] text-slate-600 font-bold block">فئة 500:</label>
                                <input
                                  type="number"
                                  min="0"
                                  value={denom500 || ''}
                                  onChange={(e) => setDenom500(parseInt(e.target.value) || 0)}
                                  className="w-full bg-white border border-slate-300 rounded p-1 font-mono text-center font-bold text-xs"
                                  placeholder="0"
                                />
                                <span className="text-[9px] text-slate-500 text-center block mt-0.5">
                                  ={(denom500 * 500).toLocaleString()}
                                </span>
                              </div>

                              <div>
                                <label className="text-[10px] text-slate-600 font-bold block">فئة 250:</label>
                                <input
                                  type="number"
                                  min="0"
                                  value={denom250 || ''}
                                  onChange={(e) => setDenom250(parseInt(e.target.value) || 0)}
                                  className="w-full bg-white border border-slate-300 rounded p-1 font-mono text-center font-bold text-xs"
                                  placeholder="0"
                                />
                                <span className="text-[9px] text-slate-500 text-center block mt-0.5">
                                  ={(denom250 * 250).toLocaleString()}
                                </span>
                              </div>

                              <div>
                                <label className="text-[10px] text-slate-600 font-bold block">فئة 100:</label>
                                <input
                                  type="number"
                                  min="0"
                                  value={denom100 || ''}
                                  onChange={(e) => setDenom100(parseInt(e.target.value) || 0)}
                                  className="w-full bg-white border border-slate-300 rounded p-1 font-mono text-center font-bold text-xs"
                                  placeholder="0"
                                />
                                <span className="text-[9px] text-slate-500 text-center block mt-0.5">
                                  ={(denom100 * 100).toLocaleString()}
                                </span>
                              </div>
                            </div>
                            <div className="flex items-center justify-between pt-1 border-t border-slate-200">
                              <span className="text-[10px] text-slate-600">مجموع الفئات النقدية:</span>
                              <span className="font-mono font-bold text-sky-800 text-xs">
                                {denomTotal.toLocaleString()} {currency}
                              </span>
                            </div>
                          </div>
                        )}

                        <div className="flex flex-col gap-2 bg-slate-50 p-3 rounded-lg border border-slate-200">
                          <div className="flex justify-between items-center text-[11px]">
                            <span className="text-slate-600">العهدة الافتتاحية:</span>
                            <span className="font-mono font-bold text-slate-800">{shift.openingCash.toLocaleString()} {currency}</span>
                          </div>
                          <div className="flex justify-between items-center text-[11px]">
                            <span className="text-slate-600">+ المقبوضات النقدية:</span>
                            <span className="font-mono font-bold text-emerald-700">+{shiftCashSales.toLocaleString()} {currency}</span>
                          </div>
                          <div className="flex justify-between items-center pt-2 border-t border-slate-200 font-bold text-xs">
                            <span className="text-slate-900">المبلغ المتوقع نظاماً في الدرج:</span>
                            <span className="font-mono text-sky-900">{expectedDrawerCash.toLocaleString()} {currency}</span>
                          </div>
                        </div>

                        {/* Actual Cash Input */}
                        <div className="flex flex-col gap-1.5">
                          <label className="font-bold text-slate-800 text-xs">
                            المبلغ الفعلي الموجود بالدرج (الجرد الفعلي):
                          </label>
                          <div className="flex items-center gap-2">
                            <input
                              type="number"
                              placeholder={expectedDrawerCash.toString()}
                              value={actualDrawerCashInput}
                              onChange={(e) => setActualDrawerCashInput(e.target.value)}
                              className="flex-1 border-2 border-sky-600 rounded-lg px-3 py-2 font-mono font-bold text-slate-900 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
                            />
                            <span className="font-bold text-slate-600">{currency}</span>
                          </div>
                        </div>

                        {/* Reconciliation Status Badge */}
                        <div className={`p-2.5 rounded-lg border flex items-center justify-between font-bold ${
                          cashDifference === 0
                            ? 'bg-emerald-50 border-emerald-300 text-emerald-800'
                            : cashDifference > 0
                              ? 'bg-amber-50 border-amber-300 text-amber-800'
                              : 'bg-rose-50 border-rose-300 text-rose-800'
                        }`}>
                          <span className="flex items-center gap-1.5">
                            {cashDifference === 0 ? (
                              <CheckCircle2 className="w-4 h-4 text-emerald-600" />
                            ) : (
                              <AlertTriangle className="w-4 h-4 text-rose-600" />
                            )}
                            <span>حالة الجرد:</span>
                          </span>
                          <span className="font-mono text-xs">
                            {cashDifference === 0
                              ? 'متطابق تماماً (0 فارق)'
                              : cashDifference > 0
                                ? `يوجد فائض: +${cashDifference.toLocaleString()} ${currency}`
                                : `يوجد عجز: ${cashDifference.toLocaleString()} ${currency}`}
                          </span>
                        </div>
                      </div>

                      {/* Treasury Cash Handover Card (تسليم الفلوس للخزينة) */}
                      <div className="bg-sky-50/60 border border-sky-300 rounded-xl p-4 flex flex-col gap-2.5 shadow-sm">
                        <div className="flex items-center justify-between border-b border-sky-200 pb-2">
                          <div className="flex items-center gap-1.5">
                            <ArrowDownLeft className="w-4 h-4 text-sky-800" />
                            <span className="font-bold text-sky-950">تسليم النقدية وتوريدها للخزينة الرئيسية</span>
                          </div>
                          <label className="flex items-center gap-1.5 cursor-pointer text-[11px] font-bold text-sky-900">
                            <input
                              type="checkbox"
                              checked={transferToTreasury}
                              onChange={(e) => setTransferToTreasury(e.target.checked)}
                              className="w-4 h-4 text-sky-600 rounded cursor-pointer"
                            />
                            <span>إنشاء سند تحويل آلي</span>
                          </label>
                        </div>

                        {transferToTreasury && (
                          <div className="flex flex-col gap-2 text-[11px]">
                            <div className="grid grid-cols-2 gap-2">
                              <div>
                                <span className="text-slate-500 block">رقم سند التحويل المتسلسل:</span>
                                <span className="font-mono font-bold text-sky-800 text-xs">
                                  سند تحويل رقم #{nextTransferVoucherNo}
                                </span>
                              </div>
                              <div>
                                <span className="text-slate-500 block">الجهة المستلمة:</span>
                                <span className="font-bold text-slate-800 truncate block">
                                  {transferTargetAccount}
                                </span>
                              </div>
                            </div>

                            <div>
                              <span className="text-slate-600 font-bold block mb-1">المبلغ المراد تسليمه للخزينة:</span>
                              <div className="grid grid-cols-3 gap-1.5 mb-1.5">
                                <button
                                  type="button"
                                  onClick={() => setTransferAmountChoice('ALL_ACTUAL')}
                                  className={`p-1.5 rounded text-[10px] font-bold border transition cursor-pointer ${
                                    transferAmountChoice === 'ALL_ACTUAL'
                                      ? 'bg-sky-700 text-white border-sky-800'
                                      : 'bg-white text-slate-700 border-slate-300'
                                  }`}
                                >
                                  كامل مبلغ الجرد ({effectiveActualCash.toLocaleString()})
                                </button>
                                <button
                                  type="button"
                                  onClick={() => setTransferAmountChoice('SALES_ONLY')}
                                  className={`p-1.5 rounded text-[10px] font-bold border transition cursor-pointer ${
                                    transferAmountChoice === 'SALES_ONLY'
                                      ? 'bg-sky-700 text-white border-sky-800'
                                      : 'bg-white text-slate-700 border-slate-300'
                                  }`}
                                >
                                  المبيعات فقط وإبقاء العهدة
                                </button>
                                <button
                                  type="button"
                                  onClick={() => setTransferAmountChoice('CUSTOM')}
                                  className={`p-1.5 rounded text-[10px] font-bold border transition cursor-pointer ${
                                    transferAmountChoice === 'CUSTOM'
                                      ? 'bg-sky-700 text-white border-sky-800'
                                      : 'bg-white text-slate-700 border-slate-300'
                                  }`}
                                >
                                  مبلغ مخصص
                                </button>
                              </div>

                              {transferAmountChoice === 'CUSTOM' && (
                                <input
                                  type="number"
                                  value={customTransferAmount}
                                  onChange={(e) => setCustomTransferAmount(e.target.value)}
                                  placeholder="أدخل المبلغ المورد..."
                                  className="w-full bg-white border border-sky-400 rounded p-1.5 font-mono text-xs font-bold"
                                />
                              )}

                              <div className="flex justify-between items-center bg-white p-2 rounded border border-sky-200 mt-1 font-bold">
                                <span className="text-sky-900">المبلغ النهائي المورد بسند التحويل:</span>
                                <span className="font-mono text-emerald-700 text-sm font-black">
                                  {calculatedTransferAmount.toLocaleString()} {currency}
                                </span>
                              </div>
                            </div>

                            <div>
                              <input
                                type="text"
                                value={transferNotes}
                                onChange={(e) => setTransferNotes(e.target.value)}
                                placeholder="ملاحظات تسليم العهدة أو أسباب الفارق (إن وجدت)..."
                                className="w-full bg-white border border-slate-300 rounded p-1.5 text-xs"
                              />
                            </div>
                          </div>
                        )}

                        {/* Confirmation Button */}
                        <button
                          type="button"
                          onClick={handleConfirmCloseShift}
                          className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold py-2.5 rounded-lg text-xs shadow-md flex items-center justify-center gap-2 cursor-pointer mt-1"
                        >
                          <Coins className="w-4 h-4" />
                          <span>اعتماد إقفال الوردية، ترحيل النقدية، وتوليد Z-Report</span>
                        </button>
                      </div>
                    </div>
                  </div>
                )}
              </div>
            )}

            {/* SUB-TAB 2: CLOSED SHIFTS HISTORY & ARCHIVE */}
            {shiftSubTab === 'SHIFT_HISTORY' && (
              <div className="bg-white rounded-xl border border-slate-300 shadow-sm overflow-hidden flex flex-col">
                <div className="p-3 bg-slate-50 border-b border-slate-200 flex items-center justify-between">
                  <div className="flex items-center gap-2">
                    <History className="w-4 h-4 text-sky-700" />
                    <span className="font-bold text-slate-800 text-xs">أرشيف الورديات المقفلة ومحاضر التسليم المالي</span>
                  </div>
                  <span className="text-[11px] text-slate-500">
                    إجمالي الورديات المقفلة: <strong>{closedShifts.length}</strong>
                  </span>
                </div>

                <div className="overflow-x-auto">
                  <table className="w-full text-right text-xs">
                    <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                      <tr>
                        <th className="p-2.5">رقم الوردية</th>
                        <th className="p-2.5">الكاشير</th>
                        <th className="p-2.5">الفرع</th>
                        <th className="p-2.5">النمط</th>
                        <th className="p-2.5">توقيت الفتح</th>
                        <th className="p-2.5">توقيت الإقفال</th>
                        <th className="p-2.5">المبيعات الإجمالية</th>
                        <th className="p-2.5">النقدية الموردة</th>
                        <th className="p-2.5">الفارق</th>
                        <th className="p-2.5">سند التحويل</th>
                        <th className="p-2.5 text-center">الإجراءات</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-slate-100">
                      {closedShifts.length === 0 ? (
                        <tr>
                          <td colSpan={11} className="p-6 text-center text-slate-400">
                            لا توجد ورديات مقفلة حتى الآن في السجل.
                          </td>
                        </tr>
                      ) : (
                        closedShifts.map((cs) => {
                          const isMatched = cs.cashDifference === 0;
                          return (
                            <tr key={cs.id} className="hover:bg-slate-50">
                              <td className="p-2.5 font-mono font-bold text-sky-800">
                                #{cs.shiftNumber}
                              </td>
                              <td className="p-2.5 font-medium text-slate-900">
                                {cs.cashierName}
                              </td>
                              <td className="p-2.5 text-slate-600">
                                {cs.branchName || 'الفرع الرئيسي'}
                              </td>
                              <td className="p-2.5">
                                <span className={`px-2 py-0.5 rounded text-[10px] font-bold ${
                                  cs.mode === 'DAILY_SIMPLE' ? 'bg-amber-100 text-amber-800' : 'bg-sky-100 text-sky-800'
                                }`}>
                                  {cs.mode === 'DAILY_SIMPLE' ? 'يومي' : 'مؤسسي'}
                                </span>
                              </td>
                              <td className="p-2.5 font-mono text-[11px] text-slate-600">
                                {cs.openedAt}
                              </td>
                              <td className="p-2.5 font-mono text-[11px] text-slate-900 font-bold">
                                {cs.closedAt}
                              </td>
                              <td className="p-2.5 font-mono font-bold text-slate-900">
                                {cs.totalSales.toLocaleString()} {currency}
                              </td>
                              <td className="p-2.5 font-mono font-bold text-emerald-700">
                                {(cs.transferredAmount || cs.actualCash).toLocaleString()} {currency}
                              </td>
                              <td className="p-2.5">
                                <span className={`px-2 py-0.5 rounded font-mono font-bold text-[11px] ${
                                  isMatched 
                                    ? 'bg-emerald-100 text-emerald-800' 
                                    : cs.cashDifference > 0 
                                      ? 'bg-amber-100 text-amber-800' 
                                      : 'bg-rose-100 text-rose-800'
                                }`}>
                                  {isMatched ? '0' : cs.cashDifference > 0 ? `+${cs.cashDifference}` : cs.cashDifference}
                                </span>
                              </td>
                              <td className="p-2.5 font-mono text-slate-700">
                                {cs.transferVoucherNo ? `سند #${cs.transferVoucherNo}` : '-'}
                              </td>
                              <td className="p-2.5 text-center">
                                <button
                                  type="button"
                                  onClick={() => setViewingZReportShift(cs)}
                                  className="bg-sky-700 hover:bg-sky-600 text-white font-bold px-2.5 py-1 rounded text-[11px] flex items-center gap-1 mx-auto shadow-xs cursor-pointer"
                                >
                                  <Printer className="w-3 h-3" />
                                  <span>Z-Report</span>
                                </button>
                              </td>
                            </tr>
                          );
                        })
                      )}
                    </tbody>
                  </table>
                </div>
              </div>
            )}

            {/* Modal 1: Formal Z-Report Modal */}
            <ZReportModal
              closedShift={viewingZReportShift}
              currency={currency}
              onClose={() => setViewingZReportShift(null)}
            />

            {/* Modal 2: Open Shift Modal */}
            <OpenShiftModal
              isOpen={showOpenShiftModal}
              onClose={() => setShowOpenShiftModal(false)}
              onOpenShift={(newShift) => {
                onOpenShift?.(newShift);
                setShiftSubTab('ACTIVE_SHIFT');
              }}
              lastShiftNumber={shift.shiftNumber}
              users={users}
              currentUser={currentUser}
              currency={currency}
              shiftMode={shiftMode}
            />
          </div>
        )}
      </div>
    </div>
  );
};
