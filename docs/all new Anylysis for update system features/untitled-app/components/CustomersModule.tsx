'use client';

import React, { useState, useMemo } from 'react';
import {
  Users,
  Search,
  Plus,
  Filter,
  FileText,
  CreditCard,
  ArrowDownLeft,
  ArrowUpRight,
  Printer,
  X,
  AlertTriangle,
  CheckCircle2,
  Phone,
  MapPin,
  Clock,
  DollarSign,
  ChevronRight,
  ChevronLeft,
  Building,
  Hash,
  FileSpreadsheet,
  Layers,
  Edit3,
  Trash2,
  Eye,
  SlidersHorizontal,
  Wallet,
  Landmark,
  Receipt,
  BookOpen,
  Calendar,
  Package,
  Truck,
  Coins,
} from 'lucide-react';
import {
  Customer,
  Invoice,
  CashVoucher,
  JournalEntry,
  AccountNode,
  ShiftInfo,
  SystemUser,
  CostCenterNode,
  CustomerStatementMovement,
  VoucherInvoiceAllocation,
  MainTabType,
} from '@/types/pos';
import { tafqeet } from '@/lib/tafqeet';
import { calculateCustomerStatement, calculateCustomerAging } from '@/lib/mock';
import { generateNextId, getCurrentDateString, getCurrentTimeString } from '@/lib/idGenerator';
import {
  CustomerFormModal,
  CustomerReceiptVoucherModal,
  CustomerPaymentVoucherModal,
  CustomerPrintStatementModal,
  CustomerPrintVoucherModal,
} from '@/components/customers';

interface CustomersModuleProps {
  customers: Customer[];
  salesInvoices: Invoice[];
  vouchers: CashVoucher[];
  journalEntries: JournalEntry[];
  chartOfAccounts: AccountNode[];
  costCenters: CostCenterNode[];
  shift: ShiftInfo;
  currentUser: SystemUser | null;
  onAddCustomer: (customer: Customer) => void;
  onUpdateCustomer: (customer: Customer) => void;
  onDeleteCustomer: (customerId: string) => { success: boolean; message: string };
  onAddCustomerVoucher: (voucher: CashVoucher, allocations?: VoucherInvoiceAllocation[]) => void;
  onSettleInvoice?: (invoiceId: string, amount: number, paymentMethod: 'CASH' | 'BANK') => void;
  onSelectTab?: (tab: MainTabType) => void;
}

type CustomersSubTab = 'CUSTOMERS_LIST' | 'CUSTOMER_STATEMENT' | 'VOUCHERS' | 'RECEIVABLES_MATCHING';

export const CustomersModule: React.FC<CustomersModuleProps> = ({
  customers,
  salesInvoices,
  vouchers,
  journalEntries,
  chartOfAccounts,
  costCenters,
  shift,
  currentUser,
  onAddCustomer,
  onUpdateCustomer,
  onDeleteCustomer,
  onAddCustomerVoucher,
  onSelectTab,
}) => {
  // Navigation
  const [subTab, setSubTab] = useState<CustomersSubTab>('CUSTOMERS_LIST');

  // Customer List Search & Filters
  const [searchQuery, setSearchQuery] = useState('');
  const [statusFilter, setStatusFilter] = useState<'ALL' | 'ACTIVE' | 'OVER_LIMIT' | 'WITH_BALANCE'>('ALL');

  // Selected Customer for Details / Statement
  const [selectedCustomerId, setSelectedCustomerId] = useState<string>(() => customers[1]?.id || customers[0]?.id || '');

  // Statement Filters
  const [statementDateFrom, setStatementDateFrom] = useState('');
  const [statementDateTo, setStatementDateTo] = useState('');

  // Modals
  const [showCustomerModal, setShowCustomerModal] = useState(false);
  const [editingCustomer, setEditingCustomer] = useState<Customer | null>(null);

  const [showReceiptVoucherModal, setShowReceiptVoucherModal] = useState(false);
  const [showPaymentVoucherModal, setShowPaymentVoucherModal] = useState(false);
  const [voucherTargetCustomer, setVoucherTargetCustomer] = useState<Customer | null>(null);

  const [showStatementPrintModal, setShowStatementPrintModal] = useState(false);
  const [showVoucherPrintModal, setShowVoucherPrintModal] = useState(false);
  const [lastPrintedVoucher, setLastPrintedVoucher] = useState<CashVoucher | null>(null);

  // Quick Notification Banner
  const [toastMessage, setToastMessage] = useState<string | null>(null);
  const showToast = (msg: string) => {
    setToastMessage(msg);
    setTimeout(() => setToastMessage(null), 4000);
  };

  // Selected customer object
  const activeCustomer = useMemo(() => {
    return customers.find((c) => c.id === selectedCustomerId) || customers[0];
  }, [customers, selectedCustomerId]);

  // KPIs
  const totalReceivables = useMemo(() => {
    return customers.reduce((sum, c) => sum + (c.currentBalance > 0 ? c.currentBalance : 0), 0);
  }, [customers]);

  const overLimitCustomersCount = useMemo(() => {
    return customers.filter((c) => c.creditLimit > 0 && c.currentBalance > c.creditLimit).length;
  }, [customers]);

  const customersWithDebtCount = useMemo(() => {
    return customers.filter((c) => c.currentBalance > 0).length;
  }, [customers]);

  // Filtered Customers List
  const filteredCustomers = useMemo(() => {
    return customers.filter((c) => {
      if (searchQuery.trim()) {
        const q = searchQuery.toLowerCase();
        const matches =
          c.name.toLowerCase().includes(q) ||
          c.code.toLowerCase().includes(q) ||
          c.phone.includes(q) ||
          (c.address && c.address.toLowerCase().includes(q)) ||
          (c.contactPerson && c.contactPerson.toLowerCase().includes(q));
        if (!matches) return false;
      }

      if (statusFilter === 'ACTIVE') {
        if (c.status === 'BLOCKED' || c.status === 'SUSPENDED') return false;
      } else if (statusFilter === 'OVER_LIMIT') {
        if (c.creditLimit <= 0 || c.currentBalance <= c.creditLimit) return false;
      } else if (statusFilter === 'WITH_BALANCE') {
        if (c.currentBalance <= 0) return false;
      }

      return true;
    });
  }, [customers, searchQuery, statusFilter]);

  // Customer Statement Data
  const statementData = useMemo(() => {
    if (!selectedCustomerId) return { openingBalance: 0, movements: [], totalDebit: 0, totalCredit: 0, closingBalance: 0 };
    return calculateCustomerStatement({
      customerId: selectedCustomerId,
      customers,
      salesInvoices,
      vouchers,
      journalEntries,
      dateFrom: statementDateFrom || undefined,
      dateTo: statementDateTo || undefined,
    });
  }, [selectedCustomerId, customers, salesInvoices, vouchers, journalEntries, statementDateFrom, statementDateTo]);

  // Customer Aging Data
  const agingData = useMemo(() => {
    return calculateCustomerAging({
      customerId: selectedCustomerId || undefined,
      salesInvoices,
      asOfDate: getCurrentDateString(),
    });
  }, [selectedCustomerId, salesInvoices]);

  // Overall Unpaid Invoices for Matching Tab
  const unpaidCreditInvoices = useMemo(() => {
    return salesInvoices
      .filter((inv) => inv.type === 'SALE' && inv.paymentType === 'CREDIT' && inv.remainingAmount > 0)
      .sort((a, b) => a.date.localeCompare(b.date));
  }, [salesInvoices]);

  // Customer Vouchers (Receipts & Payments)
  const customerVouchers = useMemo(() => {
    return vouchers
      .filter((v) => v.partyType === 'CUSTOMER' || customers.some((c) => c.id === v.partyId || c.accountCode === v.contraAccountCode))
      .sort((a, b) => b.date.localeCompare(a.date));
  }, [vouchers, customers]);

  // Handle open Customer Form
  const handleOpenAddCustomer = () => {
    const nextNum = customers.length + 1;
    const paddedCode = `1130${nextNum < 10 ? '0' + nextNum : nextNum}`;
    setEditingCustomer({
      id: generateNextId('CUST'),
      code: paddedCode,
      name: '',
      phone: '',
      address: '',
      creditLimit: 100000,
      currentBalance: 0,
      accountCode: paddedCode,
      taxNumber: '',
      email: '',
      contactPerson: '',
      creditDays: 30,
      status: 'ACTIVE',
      notes: '',
      openingBalance: 0,
      openingBalanceDate: getCurrentDateString(),
    });
    setShowCustomerModal(true);
  };

  const handleOpenEditCustomer = (c: Customer) => {
    setEditingCustomer({ ...c });
    setShowCustomerModal(true);
  };

  const handleSaveCustomer = (e: React.FormEvent) => {
    e.preventDefault();
    if (!editingCustomer || !editingCustomer.name.trim()) {
      showToast('يرجى إدخال اسم العميل بشكل صحيح');
      return;
    }

    const isNew = !customers.some((c) => c.id === editingCustomer.id);
    if (isNew) {
      onAddCustomer(editingCustomer);
      showToast(`تمت إضافة بطاقة العميل [${editingCustomer.name}] بنجاح`);
    } else {
      onUpdateCustomer(editingCustomer);
      showToast(`تم تحديث بيانات العميل [${editingCustomer.name}] بنجاح`);
    }

    setShowCustomerModal(false);
    setEditingCustomer(null);
  };

  const handleDeleteCustomer = (c: Customer) => {
    if (c.currentBalance !== 0) {
      showToast(`لا يمكن حذف العميل [${c.name}] لأن رصيده الحالي ليس صفراً (${c.currentBalance.toLocaleString()} ر.ي)`);
      return;
    }
    const hasInvoices = salesInvoices.some((inv) => inv.customerOrSupplierId === c.id);
    if (hasInvoices) {
      showToast(`لا يمكن حذف العميل [${c.name}] لوجود فواتير مبيعات سابقة مرتبطة بسجله`);
      return;
    }

    const res = onDeleteCustomer(c.id);
    if (res.success) {
      showToast(`تم حذف العميل [${c.name}] بنجاح`);
    } else {
      showToast(res.message);
    }
  };

  // Open Receipt Voucher for specific customer
  const handleOpenReceiptForCustomer = (cust: Customer) => {
    setVoucherTargetCustomer(cust);
    setShowReceiptVoucherModal(true);
  };

  // Quick navigation to statement
  const handleViewStatementForCustomer = (cust: Customer) => {
    setSelectedCustomerId(cust.id);
    setSubTab('CUSTOMER_STATEMENT');
  };

  return (
    <div id="customers-module-root" className="flex-1 flex flex-col h-full bg-[#f8fafc] p-2 overflow-hidden text-slate-800 select-none">
      {/* Toast alert banner */}
      {toastMessage && (
        <div className="absolute top-12 left-1/2 -translate-x-1/2 z-50 bg-slate-900 text-white border border-slate-700 px-4 py-2 rounded-lg shadow-lg text-xs font-bold flex items-center gap-2 animate-bounce">
          <CheckCircle2 className="w-4 h-4 text-emerald-400" />
          <span>{toastMessage}</span>
        </div>
      )}

      {/* 1. Phase 3 Operational Stage Ribbon (شريط المرحلة الثالثة: دورة العملاء والسيولة والعهد) */}
      <div className="bg-white border border-slate-200 px-3 py-1.5 rounded-t-lg flex flex-wrap items-center justify-between gap-2 text-xs shadow-xs shrink-0 text-slate-800">
        {/* Left: Phase Title & Workflow Steps Navigation */}
        <div className="flex items-center gap-2 overflow-x-auto">
          {/* Phase Badge */}
          <div className="flex items-center gap-1.5 bg-teal-700 text-white px-2.5 py-0.5 rounded-full border border-teal-800 shadow-xs">
            <span className="w-4 h-4 rounded-full bg-white text-teal-900 flex items-center justify-center font-black text-[10px]">
              3
            </span>
            <span className="font-bold text-[11px]">المرحلة الثالثة: العملاء والسيولة النقدية والعهد</span>
          </div>

          <div className="h-4 w-px bg-slate-200" />

          {/* Direct Switchers between Phase 3 Tabs */}
          <div className="flex items-center gap-1">
            <button
              type="button"
              onClick={() => setSubTab('CUSTOMERS_LIST')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                subTab === 'CUSTOMERS_LIST'
                  ? 'bg-teal-700 text-white border border-teal-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="دليل العملاء وسقوف الائتمان"
            >
              <Users className="w-3 h-3 text-teal-600" />
              <span>دليل العملاء</span>
            </button>

            <button
              type="button"
              onClick={() => setSubTab('CUSTOMER_STATEMENT')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                subTab === 'CUSTOMER_STATEMENT'
                  ? 'bg-teal-700 text-white border border-teal-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="كشف حساب عميل تفصيلي وأعمار الديون"
            >
              <FileText className="w-3 h-3 text-sky-600" />
              <span>كشف حساب وأعمار ديون</span>
            </button>

            <button
              type="button"
              onClick={() => setSubTab('VOUCHERS')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                subTab === 'VOUCHERS'
                  ? 'bg-teal-700 text-white border border-teal-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="سندات القبض وسندات الصرف النقدية والبنكية"
            >
              <Receipt className="w-3 h-3 text-amber-600" />
              <span>سندات القبض والصرف</span>
            </button>

            <button
              type="button"
              onClick={() => setSubTab('RECEIVABLES_MATCHING')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                subTab === 'RECEIVABLES_MATCHING'
                  ? 'bg-teal-700 text-white border border-teal-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="تسوية ومطابقة الفواتير الآجلة بالأقساط"
            >
              <CreditCard className="w-3 h-3 text-cyan-600" />
              <span>تسوية الفواتير الآجلة</span>
            </button>

            {onSelectTab && (
              <button
                type="button"
                onClick={() => onSelectTab('PETTY_CASH')}
                className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                title="إدارة العهد والمصروفات النثرية للموظفين"
              >
                <Coins className="w-3 h-3 text-yellow-600" />
                <span>العهد النثرية [Petty Cash]</span>
              </button>
            )}
          </div>
        </div>

        {/* Right: Architectural Rules & Prev/Next Stage Shortcuts */}
        <div className="flex items-center gap-2">
          {/* Rules Indicators */}
          <div className="hidden xl:flex items-center gap-1 text-[10px] text-slate-600">
            <span className="bg-emerald-50 text-emerald-700 px-1.5 py-0.5 rounded border border-emerald-200 font-mono font-bold">
              ✓ سندات قبض: 1001...
            </span>
            <span className="bg-amber-50 text-amber-700 px-1.5 py-0.5 rounded border border-amber-200 font-mono font-bold">
              ✓ سندات صرف: 2001...
            </span>
            <span className="bg-sky-50 text-sky-700 px-1.5 py-0.5 rounded border border-sky-200 font-bold">
              ✓ سداد متعدد وقيد آلي
            </span>
          </div>

          {onSelectTab && (
            <div className="flex items-center gap-1">
              <button
                type="button"
                onClick={() => onSelectTab('INVENTORY')}
                className="bg-slate-100 hover:bg-slate-200 text-slate-700 text-[11px] font-bold px-2 py-1 rounded flex items-center gap-1 transition border border-slate-200 cursor-pointer"
                title="الرجوع إلى المرحلة الثانية: المخازن والمشتريات"
              >
                <ChevronRight className="w-3.5 h-3.5" />
                <span>المرحلة 2: المخازن والتوريد</span>
              </button>

              <button
                type="button"
                onClick={() => onSelectTab('ACCOUNTING')}
                className="bg-blue-700 hover:bg-blue-600 text-white text-[11px] font-bold px-2.5 py-1 rounded flex items-center gap-1 transition shadow-xs border border-blue-800 cursor-pointer"
                title="الانتقال إلى المرحلة الرابعة: الحسابات العامة ودفتر الأستاذ والتقارير المالية"
              >
                <span>المرحلة 4: الحسابات العامة</span>
                <ChevronLeft className="w-3.5 h-3.5" />
              </button>
            </div>
          )}
        </div>
      </div>

      {/* Header ToolStrip / Ribbon */}
      <div className="bg-white text-slate-800 px-3 py-2 border-x border-b border-slate-200 flex flex-wrap items-center justify-between gap-2 shadow-xs">
        <div className="flex items-center gap-3">
          <div className="bg-sky-50 p-1.5 rounded border border-sky-200 text-sky-700">
            <Users className="w-5 h-5" />
          </div>
          <div>
            <div className="flex items-center gap-2">
              <h1 className="text-sm font-bold tracking-wide text-slate-800">نظام إدارة العملاء والذمم المدينة وسندات القبض</h1>
              <span className="text-[10px] bg-emerald-50 text-emerald-700 border border-emerald-200 px-2 py-0.5 rounded font-mono font-bold">
                Accounts Receivable & Vouchers
              </span>
            </div>
            <p className="text-[11px] text-slate-500">
              دليل العملاء، سقوف الائتمان، كشوفات الحسابات التفصيلية، التحصيل المالي، وتسوية الفواتير الآجلة
            </p>
          </div>
        </div>

        {/* Action Buttons */}
        <div className="flex items-center gap-2">
          <button
            id="btn-new-receipt-voucher"
            onClick={() => {
              setVoucherTargetCustomer(activeCustomer || customers[1] || customers[0]);
              setShowReceiptVoucherModal(true);
            }}
            className="bg-emerald-600 hover:bg-emerald-500 text-white text-xs font-bold px-3 py-1.5 rounded flex items-center gap-1.5 shadow-xs transition cursor-pointer"
          >
            <ArrowDownLeft className="w-4 h-4 text-white" />
            <span>سند قبض عميل [F2]</span>
          </button>

          <button
            id="btn-new-payment-voucher"
            onClick={() => {
              setVoucherTargetCustomer(activeCustomer || customers[1] || customers[0]);
              setShowPaymentVoucherModal(true);
            }}
            className="bg-amber-600 hover:bg-amber-500 text-white text-xs font-bold px-3 py-1.5 rounded flex items-center gap-1.5 shadow-xs transition cursor-pointer"
          >
            <ArrowUpRight className="w-4 h-4 text-white" />
            <span>سند صرف / رد [F3]</span>
          </button>

          <button
            id="btn-add-customer-main"
            onClick={handleOpenAddCustomer}
            className="bg-sky-600 hover:bg-sky-500 text-white text-xs font-bold px-3 py-1.5 rounded flex items-center gap-1.5 shadow-xs transition cursor-pointer"
          >
            <Plus className="w-4 h-4 text-white" />
            <span>عميل جديد [F4]</span>
          </button>
        </div>
      </div>

      {/* Sub-Navigation Tabs Bar */}
      <div className="bg-slate-50 border-x border-b border-slate-200 px-3 py-1.5 flex flex-wrap items-center justify-between gap-2 text-xs">
        <div className="flex items-center gap-1.5 overflow-x-auto">
          <button
            id="subtab-customers-list"
            onClick={() => setSubTab('CUSTOMERS_LIST')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              subTab === 'CUSTOMERS_LIST'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-white text-slate-700 hover:bg-slate-100 border border-slate-200'
            }`}
          >
            <Users className="w-3.5 h-3.5 text-sky-500" />
            <span>دليل العملاء وسقوف الائتمان ({customers.length})</span>
          </button>

          <button
            id="subtab-customer-statement"
            onClick={() => setSubTab('CUSTOMER_STATEMENT')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              subTab === 'CUSTOMER_STATEMENT'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-white text-slate-700 hover:bg-slate-100 border border-slate-200'
            }`}
          >
            <FileText className="w-3.5 h-3.5 text-emerald-500" />
            <span>كشف حساب عميل تفصيلي وأعمار الديون</span>
          </button>

          <button
            id="subtab-receipt-vouchers"
            onClick={() => setSubTab('VOUCHERS')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              subTab === 'VOUCHERS'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-white text-slate-700 hover:bg-slate-100 border border-slate-200'
            }`}
          >
            <Receipt className="w-3.5 h-3.5 text-teal-600" />
            <span>سندات القبض والصرف ({customerVouchers.length})</span>
          </button>

          <button
            id="subtab-receivables-matching"
            onClick={() => setSubTab('RECEIVABLES_MATCHING')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              subTab === 'RECEIVABLES_MATCHING'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-white text-slate-700 hover:bg-slate-100 border border-slate-200'
            }`}
          >
            <CreditCard className="w-3.5 h-3.5 text-rose-500" />
            <span>تسوية ومطابقة الفواتير الآجلة ({unpaidCreditInvoices.length})</span>
          </button>
        </div>

        {/* Right Status Badge */}
        <div className="flex items-center gap-2 font-mono text-[11px] text-slate-600">
          <span>إجمالي ذمم العملاء:</span>
          <span className="font-bold text-rose-700 bg-rose-50 border border-rose-200 px-2 py-0.5 rounded font-mono-numbers">
            {totalReceivables.toLocaleString()} ر.ي
          </span>
        </div>
      </div>

      {/* Main Workspace Body */}
      <div className="flex-1 min-h-0 bg-white rounded-b-lg border-x border-b border-slate-300 shadow-xs overflow-hidden flex flex-col">
        {/* ======================================================== */}
        {/* TAB 1: CUSTOMERS MASTER DIRECTORY (دليل العملاء والائتمان) */}
        {/* ======================================================== */}
        {subTab === 'CUSTOMERS_LIST' && (
          <div className="flex-1 flex flex-col h-full overflow-hidden">
            {/* KPI Summary Strip */}
            <div className="grid grid-cols-2 sm:grid-cols-4 gap-2 p-2.5 bg-slate-50 border-b border-slate-300 text-xs">
              <div className="bg-white p-2 rounded border border-slate-200 flex items-center justify-between">
                <div>
                  <span className="text-slate-500 block text-[10px]">إجمالي العملاء المسجلين:</span>
                  <span className="font-bold text-slate-800 text-sm">{customers.length} عميل</span>
                </div>
                <Users className="w-5 h-5 text-sky-600" />
              </div>

              <div className="bg-white p-2 rounded border border-slate-200 flex items-center justify-between">
                <div>
                  <span className="text-slate-500 block text-[10px]">إجمالي الذمم المدينة (المستحقة):</span>
                  <span className="font-bold text-rose-700 text-sm font-mono-numbers">
                    {totalReceivables.toLocaleString()} ر.ي
                  </span>
                </div>
                <DollarSign className="w-5 h-5 text-rose-600" />
              </div>

              <div className="bg-white p-2 rounded border border-slate-200 flex items-center justify-between">
                <div>
                  <span className="text-slate-500 block text-[10px]">عملاء عليهم رصيد مدين:</span>
                  <span className="font-bold text-amber-700 text-sm">{customersWithDebtCount} عميل</span>
                </div>
                <CreditCard className="w-5 h-5 text-amber-600" />
              </div>

              <div className="bg-white p-2 rounded border border-slate-200 flex items-center justify-between">
                <div>
                  <span className="text-slate-500 block text-[10px]">تجاوزوا سقف الائتمان:</span>
                  <span className={`font-bold text-sm ${overLimitCustomersCount > 0 ? 'text-red-700' : 'text-emerald-700'}`}>
                    {overLimitCustomersCount} عملاء
                  </span>
                </div>
                <AlertTriangle className={`w-5 h-5 ${overLimitCustomersCount > 0 ? 'text-red-600' : 'text-emerald-600'}`} />
              </div>
            </div>

            {/* Filter and Search Bar */}
            <div className="p-2 bg-slate-100 border-b border-slate-300 flex flex-wrap items-center justify-between gap-2 text-xs">
              <div className="flex items-center gap-2 flex-1 min-w-[300px]">
                <div className="relative flex-1">
                  <Search className="w-3.5 h-3.5 text-slate-400 absolute right-2.5 top-2.5" />
                  <input
                    id="input-customer-search"
                    type="text"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                    placeholder="بحث برقم الحساب، اسم العميل، الهاتف، العنوان أو الشخص المسؤول..."
                    className="w-full bg-white border border-slate-300 rounded pr-8 pl-2.5 py-1 text-slate-900 focus:outline-hidden focus:border-sky-500 font-medium"
                  />
                  {searchQuery && (
                    <button
                      onClick={() => setSearchQuery('')}
                      className="absolute left-2 top-2 text-slate-400 hover:text-slate-600 cursor-pointer"
                    >
                      <X className="w-3 h-3" />
                    </button>
                  )}
                </div>

                <div className="flex items-center gap-1 shrink-0">
                  <SlidersHorizontal className="w-3.5 h-3.5 text-slate-500" />
                  <select
                    id="select-customer-status-filter"
                    value={statusFilter}
                    onChange={(e) => setStatusFilter(e.target.value as any)}
                    className="bg-white border border-slate-300 rounded px-2 py-1 font-semibold text-slate-800"
                  >
                    <option value="ALL">جميع الحالات</option>
                    <option value="ACTIVE">العملاء النشطون</option>
                    <option value="WITH_BALANCE">عليهم أرصدة مدينة</option>
                    <option value="OVER_LIMIT">متجاوزو سقف الائتمان</option>
                  </select>
                </div>
              </div>

              <div className="flex items-center gap-1.5 text-slate-500 text-[11px]">
                <span>المعروض: {filteredCustomers.length} من {customers.length}</span>
              </div>
            </div>

            {/* Customers DataGridView */}
            <div className="flex-1 overflow-auto">
              <table className="w-full text-right border-collapse text-xs">
                <thead>
                  <tr className="bg-slate-200/90 text-slate-700 font-bold border-b border-slate-300 sticky top-0 z-10 shadow-2xs">
                    <th className="p-2 w-20">كود الحساب</th>
                    <th className="p-2">اسم العميل والمنشأة</th>
                    <th className="p-2 w-28">الهاتف</th>
                    <th className="p-2 w-24 text-center">مهلة السداد</th>
                    <th className="p-2 w-40">سقف الائتمان ومعدل الاستهلاك</th>
                    <th className="p-2 w-32 text-left">الرصيد الحالي (ر.ي)</th>
                    <th className="p-2 w-24 text-center">الحالة</th>
                    <th className="p-2 w-48 text-center">العمليات السريعة</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredCustomers.length > 0 ? (
                    filteredCustomers.map((cust) => {
                      const isOverLimit = cust.creditLimit > 0 && cust.currentBalance > cust.creditLimit;
                      const usagePercent =
                        cust.creditLimit > 0 ? Math.min(100, Math.round((cust.currentBalance / cust.creditLimit) * 100)) : 0;

                      return (
                        <tr
                          key={cust.id}
                          className={`border-b border-slate-200 hover:bg-sky-50/60 transition ${
                            selectedCustomerId === cust.id ? 'bg-sky-100/70 font-semibold' : 'bg-white'
                          }`}
                          onClick={() => setSelectedCustomerId(cust.id)}
                        >
                          <td className="p-2 font-mono text-slate-700 font-bold">{cust.code}</td>
                          <td className="p-2">
                            <div className="flex flex-col">
                              <span className="font-bold text-slate-900">{cust.name}</span>
                              {cust.contactPerson && (
                                <span className="text-[11px] text-slate-500 flex items-center gap-1">
                                  <Users className="w-3 h-3 text-slate-400" />
                                  <span>{cust.contactPerson}</span>
                                </span>
                              )}
                              {cust.address && (
                                <span className="text-[10px] text-slate-400 truncate max-w-xs">{cust.address}</span>
                              )}
                            </div>
                          </td>
                          <td className="p-2 font-mono text-slate-700">{cust.phone || '-'}</td>
                          <td className="p-2 text-center font-mono">
                            {cust.creditDays ? `${cust.creditDays} يوم` : 'فوري'}
                          </td>
                          <td className="p-2">
                            {cust.creditLimit > 0 ? (
                              <div className="flex flex-col gap-1">
                                <div className="flex items-center justify-between text-[11px] font-mono">
                                  <span className="text-slate-600">{cust.creditLimit.toLocaleString()}</span>
                                  <span className={isOverLimit ? 'text-red-700 font-bold' : 'text-slate-500'}>
                                    {usagePercent}%
                                  </span>
                                </div>
                                <div className="w-full bg-slate-200 h-1.5 rounded-full overflow-hidden">
                                  <div
                                    className={`h-full rounded-full ${
                                      isOverLimit
                                        ? 'bg-red-600'
                                        : usagePercent > 75
                                        ? 'bg-amber-500'
                                        : 'bg-emerald-500'
                                    }`}
                                    style={{ width: `${Math.min(100, usagePercent)}%` }}
                                  />
                                </div>
                              </div>
                            ) : (
                              <span className="text-slate-400 text-[11px]">بدون سقف (نقدي)</span>
                            )}
                          </td>
                          <td className="p-2 text-left font-mono-numbers font-bold">
                            <span
                              className={
                                cust.currentBalance > 0
                                  ? 'text-rose-700'
                                  : cust.currentBalance < 0
                                  ? 'text-emerald-700'
                                  : 'text-slate-500'
                              }
                            >
                              {cust.currentBalance.toLocaleString()}
                            </span>
                          </td>
                          <td className="p-2 text-center">
                            {isOverLimit ? (
                              <span className="bg-red-100 text-red-800 border border-red-300 text-[10px] px-2 py-0.5 rounded font-bold">
                                متجاوز الائتمان
                              </span>
                            ) : cust.currentBalance > 0 ? (
                              <span className="bg-amber-100 text-amber-800 border border-amber-300 text-[10px] px-2 py-0.5 rounded font-bold">
                                عليه رصيد
                              </span>
                            ) : (
                              <span className="bg-emerald-100 text-emerald-800 border border-emerald-300 text-[10px] px-2 py-0.5 rounded font-bold">
                                رصيد سليم
                              </span>
                            )}
                          </td>
                          <td className="p-2">
                            <div className="flex items-center justify-center gap-1">
                              <button
                                onClick={(e) => {
                                  e.stopPropagation();
                                  handleOpenReceiptForCustomer(cust);
                                }}
                                className="bg-emerald-600 hover:bg-emerald-500 text-white text-[11px] font-bold px-2 py-1 rounded flex items-center gap-1 cursor-pointer"
                                title="إصدار سند قبض من العميل"
                              >
                                <ArrowDownLeft className="w-3 h-3" />
                                <span>قبض</span>
                              </button>

                              <button
                                onClick={(e) => {
                                  e.stopPropagation();
                                  handleViewStatementForCustomer(cust);
                                }}
                                className="bg-sky-700 hover:bg-sky-600 text-white text-[11px] font-bold px-2 py-1 rounded flex items-center gap-1 cursor-pointer"
                                title="عرض كشف الحساب التفصيلي"
                              >
                                <FileText className="w-3 h-3" />
                                <span>كشف</span>
                              </button>

                              <button
                                onClick={(e) => {
                                  e.stopPropagation();
                                  handleOpenEditCustomer(cust);
                                }}
                                className="bg-slate-200 hover:bg-slate-300 text-slate-700 p-1 rounded cursor-pointer"
                                title="تعديل بيانات العميل"
                              >
                                <Edit3 className="w-3.5 h-3.5" />
                              </button>

                              <button
                                onClick={(e) => {
                                  e.stopPropagation();
                                  handleDeleteCustomer(cust);
                                }}
                                className="bg-rose-50 hover:bg-rose-100 text-rose-700 border border-rose-200 p-1 rounded cursor-pointer"
                                title="حذف العميل"
                              >
                                <Trash2 className="w-3.5 h-3.5" />
                              </button>
                            </div>
                          </td>
                        </tr>
                      );
                    })
                  ) : (
                    <tr>
                      <td colSpan={9} className="p-10 text-center text-slate-400">
                        لم يتم العثور على عملاء مطابقين لمعايير البحث.
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {/* ======================================================== */}
        {/* TAB 2: CUSTOMER DETAILED STATEMENT (كشف حساب عميل وأعمار) */}
        {/* ======================================================== */}
        {subTab === 'CUSTOMER_STATEMENT' && (
          <div className="flex-1 flex flex-col h-full overflow-hidden">
            {/* Top Customer Selector & Filter Controls */}
            <div className="bg-slate-100 border-b border-slate-300 p-2.5 flex flex-wrap items-center justify-between gap-3 text-xs">
              <div className="flex items-center gap-2 flex-1 min-w-[320px]">
                <label className="font-bold text-slate-700 shrink-0">اختر العميل:</label>
                <select
                  id="select-statement-customer"
                  value={selectedCustomerId}
                  onChange={(e) => setSelectedCustomerId(e.target.value)}
                  className="flex-1 bg-white border border-slate-300 rounded px-2.5 py-1.5 font-bold text-slate-900"
                >
                  {customers.map((c) => (
                    <option key={c.id} value={c.id}>
                      [{c.code}] {c.name} - الرصيد: {c.currentBalance.toLocaleString()} ر.ي
                    </option>
                  ))}
                </select>
              </div>

              {/* Date Filters */}
              <div className="flex items-center gap-2">
                <span className="text-slate-600 font-semibold">من تاريخ:</span>
                <input
                  type="date"
                  value={statementDateFrom}
                  onChange={(e) => setStatementDateFrom(e.target.value)}
                  className="bg-white border border-slate-300 rounded px-2 py-1 text-slate-800"
                />
                <span className="text-slate-600 font-semibold">إلى تاريخ:</span>
                <input
                  type="date"
                  value={statementDateTo}
                  onChange={(e) => setStatementDateTo(e.target.value)}
                  className="bg-white border border-slate-300 rounded px-2 py-1 text-slate-800"
                />

                {(statementDateFrom || statementDateTo) && (
                  <button
                    onClick={() => {
                      setStatementDateFrom('');
                      setStatementDateTo('');
                    }}
                    className="bg-slate-200 hover:bg-slate-300 text-slate-700 px-2 py-1 rounded font-semibold cursor-pointer"
                  >
                    إلغاء الفترة
                  </button>
                )}
              </div>

              {/* Action Buttons */}
              <div className="flex items-center gap-2">
                <button
                  id="btn-print-customer-statement"
                  onClick={() => setShowStatementPrintModal(true)}
                  className="bg-sky-700 hover:bg-sky-600 text-white font-bold px-3 py-1.5 rounded flex items-center gap-1.5 shadow-xs cursor-pointer"
                >
                  <Printer className="w-3.5 h-3.5" />
                  <span>طباعة كشف الحساب [Ctrl+P]</span>
                </button>

                {activeCustomer && (
                  <button
                    onClick={() => handleOpenReceiptForCustomer(activeCustomer)}
                    className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-3 py-1.5 rounded flex items-center gap-1.5 shadow-xs cursor-pointer"
                  >
                    <ArrowDownLeft className="w-3.5 h-3.5" />
                    <span>سند قبض</span>
                  </button>
                )}
              </div>
            </div>

            {/* Customer Profile Banner */}
            {activeCustomer && (
              <div className="bg-slate-50 border-b border-slate-300 p-3 flex flex-wrap items-center justify-between gap-3 text-xs">
                <div className="flex items-center gap-3">
                  <div className="w-10 h-10 rounded-full bg-sky-100 border border-sky-200 text-sky-800 flex items-center justify-center font-bold text-sm shadow-xs">
                    {activeCustomer.name.slice(0, 1)}
                  </div>
                  <div>
                    <div className="flex items-center gap-2">
                      <h2 className="text-sm font-bold text-slate-900">{activeCustomer.name}</h2>
                      <span className="font-mono text-slate-500 bg-slate-200 px-2 py-0.5 rounded text-[11px]">
                        كود: {activeCustomer.code}
                      </span>
                      {activeCustomer.taxNumber && (
                        <span className="text-slate-500 text-[11px]">رقم ضريبي: {activeCustomer.taxNumber}</span>
                      )}
                    </div>
                    <div className="flex items-center gap-4 text-slate-600 text-[11px] mt-0.5">
                      <span className="flex items-center gap-1">
                        <Phone className="w-3 h-3 text-slate-400" />
                        <span>{activeCustomer.phone || 'بدون هاتف'}</span>
                      </span>
                      <span className="flex items-center gap-1">
                        <MapPin className="w-3 h-3 text-slate-400" />
                        <span>{activeCustomer.address || 'صنعاء'}</span>
                      </span>
                      {activeCustomer.contactPerson && (
                        <span className="flex items-center gap-1">
                          <Users className="w-3 h-3 text-slate-400" />
                          <span>المسؤول: {activeCustomer.contactPerson}</span>
                        </span>
                      )}
                    </div>
                  </div>
                </div>

                {/* Financial Overview Cards */}
                <div className="flex items-center gap-2">
                  <div className="bg-white p-2 rounded border border-slate-200 text-center min-w-[110px]">
                    <span className="text-[10px] text-slate-500 block">سقف الائتمان</span>
                    <span className="font-mono-numbers font-bold text-slate-800">
                      {activeCustomer.creditLimit.toLocaleString()} ر.ي
                    </span>
                  </div>

                  <div className="bg-white p-2 rounded border border-slate-200 text-center min-w-[110px]">
                    <span className="text-[10px] text-slate-500 block">مهلة السداد</span>
                    <span className="font-mono font-bold text-slate-800">
                      {activeCustomer.creditDays ? `${activeCustomer.creditDays} يوم` : 'فوري'}
                    </span>
                  </div>

                  <div className="bg-white p-2 rounded border border-slate-200 text-center min-w-[130px]">
                    <span className="text-[10px] text-slate-500 block">الرصيد الصافي الحالي</span>
                    <span className="font-mono-numbers font-bold text-rose-700 text-sm">
                      {activeCustomer.currentBalance.toLocaleString()} ر.ي
                    </span>
                  </div>
                </div>
              </div>
            )}

            {/* Debt Aging Analysis Strip */}
            <div className="bg-slate-100/90 border-b border-slate-300 p-2 flex flex-wrap items-center justify-between gap-2 text-xs">
              <div className="flex items-center gap-1.5 font-bold text-slate-700">
                <Clock className="w-4 h-4 text-sky-600" />
                <span>تحليل أعمار الديون (Accounts Receivable Aging):</span>
              </div>

              <div className="flex items-center gap-2 flex-1 max-w-2xl">
                {agingData.buckets.map((b, idx) => (
                  <div key={idx} className="flex-1 bg-white p-1.5 rounded border border-slate-200 text-center">
                    <div className="flex items-center justify-between text-[10px] text-slate-500">
                      <span>{b.periodLabel.split(' ')[0]} يوم</span>
                      <span className="font-mono font-bold text-slate-700">{b.invoiceCount} فاتورة</span>
                    </div>
                    <span className={`font-mono-numbers font-bold text-[11px] block mt-0.5 ${
                      idx === 3 ? 'text-rose-700' : idx === 2 ? 'text-amber-700' : 'text-slate-800'
                    }`}>
                      {b.amount.toLocaleString()} ر.ي
                    </span>
                  </div>
                ))}
              </div>
            </div>

            {/* Movements DataGridView */}
            <div className="flex-1 overflow-auto">
              <table className="w-full text-right border-collapse text-xs">
                <thead>
                  <tr className="bg-slate-200 text-slate-700 font-bold border-b border-slate-300 sticky top-0 z-10 shadow-2xs">
                    <th className="p-2 w-24">التاريخ</th>
                    <th className="p-2 w-32">نوع الحركة</th>
                    <th className="p-2 w-28">رقم المستند</th>
                    <th className="p-2">البيان والشرح المفصل</th>
                    <th className="p-2 w-28 text-left">مدين (عليه)</th>
                    <th className="p-2 w-28 text-left">دائن (له)</th>
                    <th className="p-2 w-32 text-left">الرصيد التراكمي</th>
                    <th className="p-2 w-24 text-center">حالة السند</th>
                  </tr>
                </thead>
                <tbody>
                  {/* Opening Balance Row */}
                  <tr className="bg-slate-50 border-b border-slate-300 font-semibold text-slate-600">
                    <td className="p-2 font-mono">{statementDateFrom || 'رصيد سابق'}</td>
                    <td className="p-2">
                      <span className="bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[10px]">
                        رصيد افتتاحي
                      </span>
                    </td>
                    <td className="p-2 font-mono">-</td>
                    <td className="p-2">الرصيد الافتتاحي المنقول ما قبل فترة الكشف</td>
                    <td className="p-2 text-left font-mono-numbers">
                      {statementData.openingBalance > 0 ? statementData.openingBalance.toLocaleString() : '-'}
                    </td>
                    <td className="p-2 text-left font-mono-numbers">
                      {statementData.openingBalance < 0 ? Math.abs(statementData.openingBalance).toLocaleString() : '-'}
                    </td>
                    <td className="p-2 text-left font-mono-numbers font-bold text-slate-800">
                      {statementData.openingBalance.toLocaleString()}
                    </td>
                    <td className="p-2 text-center text-[10px] text-slate-400">افتتاحي</td>
                  </tr>

                  {statementData.movements.length > 0 ? (
                    statementData.movements.map((m) => (
                      <tr key={m.id} className="border-b border-slate-200 hover:bg-sky-50/50 bg-white">
                        <td className="p-2 font-mono text-slate-600">
                          <div>{m.date}</div>
                          {m.time && <div className="text-[10px] text-slate-400">{m.time}</div>}
                        </td>
                        <td className="p-2">
                          <span
                            className={`px-2 py-0.5 rounded text-[10px] font-bold ${
                              m.type === 'SALE_INVOICE'
                                ? 'bg-sky-100 text-sky-800 border border-sky-200'
                                : m.type === 'RECEIPT_VOUCHER'
                                ? 'bg-emerald-100 text-emerald-800 border border-emerald-200'
                                : m.type === 'SALE_RETURN'
                                ? 'bg-indigo-100 text-indigo-800 border border-indigo-200'
                                : 'bg-amber-100 text-amber-800 border border-amber-200'
                            }`}
                          >
                            {m.typeLabel}
                          </span>
                        </td>
                        <td className="p-2 font-mono text-slate-800 font-bold">{m.documentNo}</td>
                        <td className="p-2 text-slate-700">{m.description}</td>
                        <td className="p-2 text-left font-mono-numbers font-bold text-rose-700">
                          {m.debit > 0 ? m.debit.toLocaleString() : '-'}
                        </td>
                        <td className="p-2 text-left font-mono-numbers font-bold text-emerald-700">
                          {m.credit > 0 ? m.credit.toLocaleString() : '-'}
                        </td>
                        <td className="p-2 text-left font-mono-numbers font-bold text-slate-900 bg-slate-50/80">
                          {m.balanceAfter.toLocaleString()}
                        </td>
                        <td className="p-2 text-center">
                          <span className="text-[10px] bg-slate-100 text-slate-600 px-1.5 py-0.5 rounded">
                            {m.status || 'معتمد'}
                          </span>
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan={8} className="p-8 text-center text-slate-400">
                        لا توجد حركات مسجلة خلال الفترة المحددة لهذا العميل.
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>

            {/* Statement Summary Footer with Tafqeet */}
            <div className="p-3 bg-slate-100 border-t border-slate-300 flex flex-wrap items-center justify-between gap-3 text-xs">
              <div className="flex-1 min-w-[280px]">
                <span className="text-slate-500 block text-[11px]">التفقيط المالي للرصيد النهائي:</span>
                <span className="font-bold text-sky-900 font-sans text-xs">
                  {tafqeet(statementData.closingBalance)}
                </span>
              </div>

              <div className="flex items-center gap-4 bg-white p-2 rounded border border-slate-300 font-mono-numbers">
                <div>
                  <span className="text-[10px] text-slate-500 block">إجمالي المسحوبات (مدين):</span>
                  <span className="font-bold text-rose-700">{statementData.totalDebit.toLocaleString()} ر.ي</span>
                </div>
                <div className="w-px h-6 bg-slate-300" />
                <div>
                  <span className="text-[10px] text-slate-500 block">إجمالي السدادات (دائن):</span>
                  <span className="font-bold text-emerald-700">{statementData.totalCredit.toLocaleString()} ر.ي</span>
                </div>
                <div className="w-px h-6 bg-slate-300" />
                <div>
                  <span className="text-[10px] text-slate-500 block">صافي الرصيد الختامي:</span>
                  <span className="font-bold text-slate-900 text-sm">
                    {statementData.closingBalance.toLocaleString()} ر.ي
                  </span>
                </div>
              </div>
            </div>
          </div>
        )}

        {/* ======================================================== */}
        {/* TAB 3: RECEIPT & PAYMENT VOUCHERS (سندات القبض والصرف)      */}
        {/* ======================================================== */}
        {subTab === 'VOUCHERS' && (
          <div className="flex-1 flex flex-col h-full overflow-hidden">
            {/* Toolbar */}
            <div className="p-2 bg-slate-100 border-b border-slate-300 flex flex-wrap items-center justify-between gap-2 text-xs">
              <div className="flex items-center gap-2">
                <span className="font-bold text-slate-700">سجل سندات القبض والصرف الخاصة بالعملاء:</span>
                <span className="font-mono text-slate-500">({customerVouchers.length} سند)</span>
              </div>

              <div className="flex items-center gap-2">
                <button
                  onClick={() => {
                    setVoucherTargetCustomer(activeCustomer || customers[1] || customers[0]);
                    setShowReceiptVoucherModal(true);
                  }}
                  className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-3 py-1.5 rounded flex items-center gap-1.5 shadow-xs cursor-pointer"
                >
                  <ArrowDownLeft className="w-3.5 h-3.5" />
                  <span>سند قبض عميل جديد [F2]</span>
                </button>
                <button
                  onClick={() => {
                    setVoucherTargetCustomer(activeCustomer || customers[1] || customers[0]);
                    setShowPaymentVoucherModal(true);
                  }}
                  className="bg-amber-600 hover:bg-amber-500 text-white font-bold px-3 py-1.5 rounded flex items-center gap-1.5 shadow-xs cursor-pointer"
                >
                  <ArrowUpRight className="w-3.5 h-3.5" />
                  <span>سند صرف جديد [F3]</span>
                </button>
              </div>
            </div>

            {/* Vouchers DataGridView */}
            <div className="flex-1 overflow-auto">
              <table className="w-full text-right border-collapse text-xs">
                <thead>
                  <tr className="bg-slate-200 text-slate-700 font-bold border-b border-slate-300 sticky top-0 z-10 shadow-2xs">
                    <th className="p-2 w-24">رقم السند</th>
                    <th className="p-2 w-24">التاريخ</th>
                    <th className="p-2 w-24">نوع السند</th>
                    <th className="p-2 w-32">طريقة الدفع</th>
                    <th className="p-2">العميل / الطرف المستلم</th>
                    <th className="p-2">حساب الخزينة / البنك</th>
                    <th className="p-2 w-28 text-left">المبلغ (ر.ي)</th>
                    <th className="p-2">البيان والشرح</th>
                    <th className="p-2 w-24 text-center">القيد المحاسبي</th>
                    <th className="p-2 w-20 text-center">طباعة</th>
                  </tr>
                </thead>
                <tbody>
                  {customerVouchers.length > 0 ? (
                    customerVouchers.map((v) => (
                      <tr key={v.id} className="border-b border-slate-200 hover:bg-sky-50/50 bg-white">
                        <td className="p-2 font-mono text-sky-900 font-bold">{v.voucherNo}</td>
                        <td className="p-2 font-mono text-slate-600">{v.date}</td>
                        <td className="p-2">
                          <span
                            className={`px-2 py-0.5 rounded text-[10px] font-bold ${
                              v.type === 'RECEIPT'
                                ? 'bg-emerald-100 text-emerald-800 border border-emerald-300'
                                : 'bg-amber-100 text-amber-800 border border-amber-300'
                            }`}
                          >
                            {v.type === 'RECEIPT' ? 'سند قبض' : 'سند صرف'}
                          </span>
                        </td>
                        <td className="p-2">
                          <div className="flex items-center gap-1">
                            {v.paymentMethod === 'CASH' ? (
                              <Wallet className="w-3.5 h-3.5 text-emerald-600" />
                            ) : (
                              <Landmark className="w-3.5 h-3.5 text-sky-600" />
                            )}
                            <span>
                              {v.paymentMethod === 'CASH'
                                ? 'نقداً (الصندوق)'
                                : v.paymentMethod === 'BANK'
                                ? 'تحويل بنكي'
                                : 'شيك'}
                            </span>
                          </div>
                        </td>
                        <td className="p-2 font-bold text-slate-900">{v.partyName}</td>
                        <td className="p-2 font-mono text-slate-600">[{v.cashAccountCode}] {v.cashAccountName}</td>
                        <td className="p-2 text-left font-mono-numbers font-bold text-slate-900">
                          {v.amount.toLocaleString()}
                        </td>
                        <td className="p-2 text-slate-600 text-[11px] truncate max-w-xs">{v.description}</td>
                        <td className="p-2 text-center font-mono">
                          {v.journalEntryId ? (
                            <span className="bg-sky-50 text-sky-800 border border-sky-200 px-1.5 py-0.5 rounded text-[10px] font-bold">
                              {v.journalEntryId}
                            </span>
                          ) : (
                            <span className="text-slate-400 text-[10px]">مرحل آلياً</span>
                          )}
                        </td>
                        <td className="p-2 text-center">
                          <button
                            onClick={() => {
                              setLastPrintedVoucher(v);
                              setShowVoucherPrintModal(true);
                            }}
                            className="bg-slate-100 hover:bg-slate-200 text-slate-700 p-1 rounded cursor-pointer"
                            title="معاينة وطباعة السند"
                          >
                            <Printer className="w-3.5 h-3.5" />
                          </button>
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan={10} className="p-8 text-center text-slate-400">
                        لا توجد سندات قبض أو صرف مسجلة حالياً.
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {/* ======================================================== */}
        {/* TAB 4: RECEIVABLES MATCHING & UNPAID INVOICES (المطابقة)  */}
        {/* ======================================================== */}
        {subTab === 'RECEIVABLES_MATCHING' && (
          <div className="flex-1 flex flex-col h-full overflow-hidden">
            {/* Toolbar */}
            <div className="p-2.5 bg-slate-100 border-b border-slate-300 flex flex-wrap items-center justify-between gap-2 text-xs">
              <div className="flex items-center gap-2">
                <span className="font-bold text-slate-800">
                  فواتير المبيعات الآجلة المفتوحة (غير المسددة بالكامل):
                </span>
                <span className="bg-rose-100 text-rose-800 px-2 py-0.5 rounded font-mono font-bold">
                  {unpaidCreditInvoices.length} فاتورة
                </span>
              </div>

              <div className="flex items-center gap-2">
                <span className="text-slate-500 font-semibold">إجمالي المتبقي غير المحصل:</span>
                <span className="font-mono-numbers font-bold text-rose-700 bg-white border border-rose-200 px-2 py-0.5 rounded">
                  {unpaidCreditInvoices.reduce((sum, inv) => sum + inv.remainingAmount, 0).toLocaleString()} ر.ي
                </span>
              </div>
            </div>

            {/* Unpaid Invoices Table */}
            <div className="flex-1 overflow-auto">
              <table className="w-full text-right border-collapse text-xs">
                <thead>
                  <tr className="bg-slate-200 text-slate-700 font-bold border-b border-slate-300 sticky top-0 z-10 shadow-2xs">
                    <th className="p-2 w-28">رقم الفاتورة</th>
                    <th className="p-2 w-24">التاريخ</th>
                    <th className="p-2">اسم العميل</th>
                    <th className="p-2 w-24 text-center">عمر الفاتورة</th>
                    <th className="p-2 w-28 text-left">إجمالي الفاتورة</th>
                    <th className="p-2 w-28 text-left">المسدد منها</th>
                    <th className="p-2 w-28 text-left">المتبقي المطلوب</th>
                    <th className="p-2">البيان والملاحظات</th>
                    <th className="p-2 w-32 text-center">التحصيل السريع</th>
                  </tr>
                </thead>
                <tbody>
                  {unpaidCreditInvoices.length > 0 ? (
                    unpaidCreditInvoices.map((inv) => {
                      const invDate = new Date(inv.date);
                      const now = new Date();
                      const daysOld = Math.max(0, Math.floor((now.getTime() - invDate.getTime()) / (1000 * 60 * 60 * 24)));
                      const isVeryOld = daysOld > 60;

                      return (
                        <tr key={inv.id} className="border-b border-slate-200 hover:bg-sky-50/50 bg-white">
                          <td className="p-2 font-mono text-sky-900 font-bold">{inv.invoiceNo}</td>
                          <td className="p-2 font-mono text-slate-600">{inv.date}</td>
                          <td className="p-2 font-bold text-slate-900">{inv.customerOrSupplierName}</td>
                          <td className="p-2 text-center font-mono">
                            <span
                              className={`px-2 py-0.5 rounded text-[10px] font-bold ${
                                isVeryOld
                                  ? 'bg-rose-100 text-rose-800 border border-rose-300'
                                  : daysOld > 30
                                  ? 'bg-amber-100 text-amber-800 border border-amber-300'
                                  : 'bg-slate-100 text-slate-700'
                              }`}
                            >
                              {daysOld} يوم
                            </span>
                          </td>
                          <td className="p-2 text-left font-mono-numbers">{inv.grandTotal.toLocaleString()}</td>
                          <td className="p-2 text-left font-mono-numbers text-emerald-700">
                            {inv.paidAmount.toLocaleString()}
                          </td>
                          <td className="p-2 text-left font-mono-numbers font-bold text-rose-700 bg-rose-50/60">
                            {inv.remainingAmount.toLocaleString()}
                          </td>
                          <td className="p-2 text-slate-600 text-[11px]">{inv.notes || 'مبيعات آجلة'}</td>
                          <td className="p-2 text-center">
                            <button
                              onClick={() => {
                                const target = customers.find((c) => c.id === inv.customerOrSupplierId);
                                if (target) {
                                  setVoucherTargetCustomer(target);
                                  setShowReceiptVoucherModal(true);
                                }
                              }}
                              className="bg-emerald-600 hover:bg-emerald-500 text-white text-[11px] font-bold px-2.5 py-1 rounded flex items-center justify-center gap-1 w-full shadow-2xs cursor-pointer"
                            >
                              <ArrowDownLeft className="w-3 h-3" />
                              <span>سداد الفاتورة</span>
                            </button>
                          </td>
                        </tr>
                      );
                    })
                  ) : (
                    <tr>
                      <td colSpan={9} className="p-10 text-center text-slate-400">
                        رائع! لا توجد فواتير مبيعات آجلة غير مسددة. جميع الذمم محصلة.
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        )}
      </div>

      {/* ======================================================== */}
      {/* MODAL 1: ADD / EDIT CUSTOMER DIALOG (بطاقة عميل جديد)      */}
      {/* ======================================================== */}
      {showCustomerModal && editingCustomer && (
        <CustomerFormModal
          editingCustomer={editingCustomer}
          customers={customers}
          setEditingCustomer={setEditingCustomer}
          onClose={() => setShowCustomerModal(false)}
          onSave={handleSaveCustomer}
        />
      )}

      {/* ======================================================== */}
      {/* MODAL 2: CUSTOMER RECEIPT VOUCHER (سند قبض نقدي / بنكي)     */}
      {/* ======================================================== */}
      {showReceiptVoucherModal && voucherTargetCustomer && (
        <CustomerReceiptVoucherModal
          customer={voucherTargetCustomer}
          customers={customers}
          salesInvoices={salesInvoices}
          chartOfAccounts={chartOfAccounts}
          costCenters={costCenters}
          shift={shift}
          currentUser={currentUser}
          onClose={() => setShowReceiptVoucherModal(false)}
          onSave={(voucher, allocations) => {
            onAddCustomerVoucher(voucher, allocations);
            setShowReceiptVoucherModal(false);
            setLastPrintedVoucher(voucher);
            setShowVoucherPrintModal(true);
            showToast(`تم حفظ وترحيل سند القبض [${voucher.voucherNo}] وتوليد القيد المحاسبي بنجاح`);
          }}
        />
      )}

      {/* ======================================================== */}
      {/* MODAL 3: CUSTOMER PAYMENT VOUCHER (سند صرف / رد مبالغ)    */}
      {/* ======================================================== */}
      {showPaymentVoucherModal && voucherTargetCustomer && (
        <CustomerPaymentVoucherModal
          customer={voucherTargetCustomer}
          customers={customers}
          chartOfAccounts={chartOfAccounts}
          costCenters={costCenters}
          currentUser={currentUser}
          onClose={() => setShowPaymentVoucherModal(false)}
          onSave={(voucher) => {
            onAddCustomerVoucher(voucher);
            setShowPaymentVoucherModal(false);
            setLastPrintedVoucher(voucher);
            setShowVoucherPrintModal(true);
            showToast(`تم حفظ وترحيل سند الصرف [${voucher.voucherNo}] بنجاح`);
          }}
        />
      )}

      {/* ======================================================== */}
      {/* MODAL 4: PRINT STATEMENT PREVIEW (معاينة كشف الحساب)      */}
      {/* ======================================================== */}
      {showStatementPrintModal && activeCustomer && (
        <CustomerPrintStatementModal
          customer={activeCustomer}
          statementData={statementData}
          dateFrom={statementDateFrom}
          dateTo={statementDateTo}
          onClose={() => setShowStatementPrintModal(false)}
        />
      )}

      {/* ======================================================== */}
      {/* MODAL 5: PRINT VOUCHER PREVIEW (معاينة سند القبض/الصرف)   */}
      {/* ======================================================== */}
      {showVoucherPrintModal && lastPrintedVoucher && (
        <CustomerPrintVoucherModal
          voucher={lastPrintedVoucher}
          onClose={() => setShowVoucherPrintModal(false)}
        />
      )}
    </div>
  );
}













export default CustomersModule;
