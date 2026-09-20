'use client';

import React, { useState, useMemo, useRef, useEffect } from 'react';
import { ProductItem, Customer, Invoice, Warehouse, CartItem } from '@/types/pos';
import { generateSalesReturnNumber } from '@/lib/idGenerator';
import {
  RotateCcw,
  Search,
  Printer,
  Save,
  CheckCircle2,
  AlertCircle,
  ArrowRight,
  Barcode,
  Calendar,
  DollarSign,
  User,
  Package,
  FileText,
  Clock,
  Trash2,
  ChevronDown,
  Building2,
  RefreshCw,
  Eye,
  X,
  CreditCard,
  Layers,
} from 'lucide-react';

interface SalesReturnItem {
  id: string;
  product: ProductItem;
  originalQty: number;
  previouslyReturnedQty: number;
  returnQty: number;
  unitPrice: number;       // Gross list price per unit
  unitDiscount: number;    // Effective discount per unit (line discount + header discount share)
  unitTax: number;         // Effective tax per unit (allocated line tax / header tax)
  netUnitPrice: number;    // unitPrice - unitDiscount
  effectiveUnitPriceWithTax: number; // netUnitPrice + unitTax
  unit: string;
  unitFactor: number;
  reason: string;
  condition: 'RESTOCK' | 'DAMAGED_QUARANTINE';
  notes?: string;
}

interface SavedSalesReturn {
  id: string;
  returnNo: string;
  originalInvoiceNo: string;
  returnDate: string;
  returnTime: string;
  customerName: string;
  customerId?: string;
  warehouseId: string;
  warehouseName: string;
  refundMethod: 'CASH' | 'CREDIT_DEDUCT' | 'BANK' | 'VOUCHER';
  cashierName: string;
  items: SalesReturnItem[];
  totalRefundAmount: number;
  totalReturnDiscount?: number;
  totalReturnTax?: number;
  notes?: string;
}

interface SalesReturnsViewProps {
  products: ProductItem[];
  customers: Customer[];
  warehouses: Warehouse[];
  invoicesHistory: Invoice[];
  currency: string;
  cashierName?: string;
  onProcessReturn: (returnRecord: SavedSalesReturn) => void;
  onBack: () => void;
}

const RETURN_REASONS = [
  'رغبة العميل (عدم الحاجة للصنف)',
  'صنف تالف أو به عيب مصنعي',
  'صنف غير مطابق للمواصفات أو الطلب',
  'قرب أو انتهاء تاريخ الصلاحية',
  'خطأ في التسعير أو المحاسبة',
  'أخرى (يرجى التوضيح في الملاحظات)',
];

export const SalesReturnsView: React.FC<SalesReturnsViewProps> = ({
  products,
  customers,
  warehouses,
  invoicesHistory,
  currency,
  cashierName = 'المحاسب الرئيسي',
  onProcessReturn,
  onBack,
}) => {
  // Tabs: NEW_RETURN vs RETURNS_HISTORY
  const [activeTab, setActiveTab] = useState<'NEW_RETURN' | 'RETURNS_HISTORY'>('NEW_RETURN');

  // Search & lookup state
  const [searchInvoiceInput, setSearchInvoiceInput] = useState<string>('');
  const [selectedInvoice, setSelectedInvoice] = useState<Invoice | null>(null);
  const [selectedWarehouseId, setSelectedWarehouseId] = useState<string>(warehouses[0]?.id || 'WH-01');

  // Return Header details
  const [returnNo, setReturnNo] = useState<string>(() => generateSalesReturnNumber());
  const [returnDate, setReturnDate] = useState<string>('2026-06-04');
  const [returnTime, setReturnTime] = useState<string>('12:00');
  const [customerName, setCustomerName] = useState<string>('');
  const [customerId, setCustomerId] = useState<string>('');
  const [refundMethod, setRefundMethod] = useState<'CASH' | 'CREDIT_DEDUCT' | 'BANK' | 'VOUCHER'>('CASH');
  const [generalNotes, setGeneralNotes] = useState<string>('');

  // Return items grid
  const [returnItems, setReturnItems] = useState<SalesReturnItem[]>([]);

  // Saved Returns in memory
  const [savedReturns, setSavedReturns] = useState<SavedSalesReturn[]>([
    {
      id: 'RET-001',
      returnNo: '6001',
      originalInvoiceNo: '1001',
      returnDate: '2026-06-03',
      returnTime: '14:30',
      customerName: 'سوبرماركت البركة',
      customerId: 'CUST-01',
      warehouseId: 'WH-01',
      warehouseName: 'المستودع الرئيسي - صنعاء',
      refundMethod: 'CASH',
      cashierName: 'المحاسب الرئيسي',
      totalRefundAmount: 2.50,
      notes: 'إرجاع 2 حبة عصير لعدم المطابقة',
      items: [
        {
          id: 'R-ITEM-1',
          product: products[1] || products[0],
          originalQty: 5,
          previouslyReturnedQty: 0,
          returnQty: 2,
          unitPrice: 1.25,
          unitDiscount: 0,
          unitTax: 0,
          netUnitPrice: 1.25,
          effectiveUnitPriceWithTax: 1.25,
          unit: 'حبة',
          unitFactor: 1,
          reason: 'صنف غير مطابق للمواصفات أو الطلب',
          condition: 'RESTOCK',
        },
      ],
    },
  ]);

  // Modal & Printing
  const [feedback, setFeedback] = useState<{ type: 'success' | 'error'; text: string } | null>(null);
  const [showPrintModal, setShowPrintModal] = useState<boolean>(false);
  const [activePrintReturn, setActivePrintReturn] = useState<SavedSalesReturn | null>(null);

  // Auto-search suggestions for invoices
  const invoiceSuggestions = useMemo(() => {
    if (!searchInvoiceInput.trim()) return [];
    const q = searchInvoiceInput.trim().toLowerCase();
    return invoicesHistory.filter(
      (inv) =>
        inv.invoiceNo.toLowerCase().includes(q) ||
        (inv.customerOrSupplierName && inv.customerOrSupplierName.toLowerCase().includes(q))
    ).slice(0, 6);
  }, [searchInvoiceInput, invoicesHistory]);

  // Load an invoice into the return form with smart tax & discount allocation
  const handleSelectInvoice = (inv: Invoice) => {
    setSelectedInvoice(inv);
    setSearchInvoiceInput(inv.invoiceNo);
    setCustomerName(inv.customerOrSupplierName || 'عميل نقدي');
    setCustomerId(inv.customerOrSupplierId || '');
    if (inv.warehouseId) {
      setSelectedWarehouseId(inv.warehouseId);
    }

    const items = inv.items || [];
    // Compute net subtotal before header discount across lines to derive proportions
    const rawLineSubtotals = items.map((item) => {
      const gross = item.quantity * item.unitPrice;
      const lineDisc = item.discount || 0;
      return Math.max(0, gross - lineDisc);
    });
    const sumNetLines = rawLineSubtotals.reduce((a, b) => a + b, 0);

    const overallHeaderDiscount = inv.discountTotal || 0;
    const overallHeaderTax = inv.taxTotal || 0;

    // Populate return items with exact smart tax and discount allocation per line
    const loadedItems: SalesReturnItem[] = items.map((item, idx) => {
      const grossLine = item.quantity * item.unitPrice;
      const lineDisc = item.discount || 0;
      const netLineBeforeHeaderDisc = Math.max(0, grossLine - lineDisc);

      // Line ratio in invoice
      const lineRatio = sumNetLines > 0 ? netLineBeforeHeaderDisc / sumNetLines : 1 / (items.length || 1);

      // Allocate header discount proportionally
      const allocatedHeaderDisc = overallHeaderDiscount * lineRatio;
      const totalLineDisc = lineDisc + allocatedHeaderDisc;
      const unitDiscount = item.quantity > 0 ? totalLineDisc / item.quantity : 0;

      // Net unit sales price after all discounts
      const netUnitPrice = Math.max(0, item.unitPrice - unitDiscount);

      // Tax per unit
      let unitTax = 0;
      if (item.taxRate && item.taxRate > 0) {
        unitTax = (netUnitPrice * item.taxRate) / 100;
      } else if (overallHeaderTax > 0) {
        const allocatedLineTax = overallHeaderTax * lineRatio;
        unitTax = item.quantity > 0 ? allocatedLineTax / item.quantity : 0;
      }

      const effectiveUnitPriceWithTax = netUnitPrice + unitTax;

      return {
        id: `RI-${idx}-${Date.now()}`,
        product: item.item,
        originalQty: item.quantity,
        previouslyReturnedQty: 0,
        returnQty: 0, // initially 0, user enters what they want to return
        unitPrice: item.unitPrice,
        unitDiscount: Number(unitDiscount.toFixed(4)),
        unitTax: Number(unitTax.toFixed(4)),
        netUnitPrice: Number(netUnitPrice.toFixed(4)),
        effectiveUnitPriceWithTax: Number(effectiveUnitPriceWithTax.toFixed(4)),
        unit: item.selectedUnit || item.item.unit || 'حبة',
        unitFactor: item.unitFactor || 1,
        reason: RETURN_REASONS[0],
        condition: 'RESTOCK',
        notes: '',
      };
    });

    setReturnItems(loadedItems);
    setFeedback({
      type: 'success',
      text: `تم تحميل الفاتورة ${inv.invoiceNo} وتوزيع الخصومات والضرائب بدقة على كل صنف بنجاح (${loadedItems.length} أصناف).`,
    });
    setTimeout(() => setFeedback(null), 3500);
  };

  // Update a specific line item in the return grid
  const handleUpdateLineItem = (index: number, field: keyof SalesReturnItem, value: any) => {
    setReturnItems((prev) => {
      const copy = [...prev];
      const target = { ...copy[index] };

      if (field === 'returnQty') {
        const numVal = Math.max(0, Number(value) || 0);
        const maxAllowed = target.originalQty - target.previouslyReturnedQty;
        if (numVal > maxAllowed) {
          target.returnQty = maxAllowed;
          setFeedback({
            type: 'error',
            text: `الكمية المرتجعة لا يمكن أن تتجاوز الكمية المباعة (${maxAllowed} ${target.unit})`,
          });
          setTimeout(() => setFeedback(null), 3000);
        } else {
          target.returnQty = numVal;
        }
      } else {
        (target as any)[field] = value;
      }

      copy[index] = target;
      return copy;
    });
  };

  // Return all items completely helper
  const handleReturnAll = () => {
    setReturnItems((prev) =>
      prev.map((it) => ({
        ...it,
        returnQty: it.originalQty - it.previouslyReturnedQty,
      }))
    );
  };

  // Reset return quantities to 0
  const handleClearQuantities = () => {
    setReturnItems((prev) =>
      prev.map((it) => ({
        ...it,
        returnQty: 0,
      }))
    );
  };

  // Calculations with Smart Proportional Tax & Discount Allocation
  const activeReturnLines = useMemo(() => {
    return returnItems.filter((it) => it.returnQty > 0);
  }, [returnItems]);

  const totalReturnGrossAmount = useMemo(() => {
    return activeReturnLines.reduce((sum, it) => sum + it.returnQty * it.unitPrice, 0);
  }, [activeReturnLines]);

  const totalReturnDiscountAmount = useMemo(() => {
    return activeReturnLines.reduce((sum, it) => sum + it.returnQty * (it.unitDiscount || 0), 0);
  }, [activeReturnLines]);

  const totalReturnTaxAmount = useMemo(() => {
    return activeReturnLines.reduce((sum, it) => sum + it.returnQty * (it.unitTax || 0), 0);
  }, [activeReturnLines]);

  const totalReturnNetAmount = useMemo(() => {
    return activeReturnLines.reduce((sum, it) => sum + it.returnQty * (it.netUnitPrice || it.unitPrice), 0);
  }, [activeReturnLines]);

  const totalReturnAmount = useMemo(() => {
    return activeReturnLines.reduce(
      (sum, it) => sum + it.returnQty * (it.effectiveUnitPriceWithTax || it.unitPrice),
      0
    );
  }, [activeReturnLines]);

  const totalReturnUnitsCount = useMemo(() => {
    return activeReturnLines.reduce((sum, it) => sum + it.returnQty, 0);
  }, [activeReturnLines]);

  // Process & Save Return
  const handleSaveAndProcessReturn = () => {
    if (!selectedInvoice && !searchInvoiceInput.trim()) {
      setFeedback({ type: 'error', text: 'يرجى تحديد أو إدخال رقم الفاتورة الأصلية أولاً.' });
      setTimeout(() => setFeedback(null), 3000);
      return;
    }

    if (activeReturnLines.length === 0) {
      setFeedback({ type: 'error', text: 'يرجى إدخال كمية مرتجعة لصنف واحد على الأقل.' });
      setTimeout(() => setFeedback(null), 3000);
      return;
    }

    const targetWarehouse = warehouses.find((w) => w.id === selectedWarehouseId);

    const newReturnRecord: SavedSalesReturn = {
      id: `SR-${Date.now()}`,
      returnNo,
      originalInvoiceNo: selectedInvoice ? selectedInvoice.invoiceNo : searchInvoiceInput,
      returnDate,
      returnTime: new Date().toLocaleTimeString('ar-YE', { hour: '2-digit', minute: '2-digit' }),
      customerName: customerName || 'عميل نقدي',
      customerId,
      warehouseId: selectedWarehouseId,
      warehouseName: targetWarehouse ? targetWarehouse.name : 'المستودع الرئيسي',
      refundMethod,
      cashierName,
      items: activeReturnLines,
      totalRefundAmount: totalReturnAmount,
      totalReturnDiscount: totalReturnDiscountAmount,
      totalReturnTax: totalReturnTaxAmount,
      notes: generalNotes,
    };

    // Save in local state
    setSavedReturns((prev) => [newReturnRecord, ...prev]);

    // Send to parent ERP engine (to update product stock & create journal entry)
    onProcessReturn(newReturnRecord);

    // Show print dialog
    setActivePrintReturn(newReturnRecord);
    setShowPrintModal(true);

    setFeedback({
      type: 'success',
      text: `تم ترحيل سند المردود رقم ${returnNo} بمبلغ ${totalReturnAmount.toLocaleString()} ${currency} وتحديث المخزون بنجاح.`,
    });

    // Reset form
    setReturnNo(generateSalesReturnNumber());
    setSelectedInvoice(null);
    setSearchInvoiceInput('');
    setReturnItems([]);
    setGeneralNotes('');
  };

  // Keyboard Shortcuts (F2: Save & Process, F10: Print)
  const handleSaveAndProcessReturnRef = useRef(handleSaveAndProcessReturn);
  useEffect(() => {
    handleSaveAndProcessReturnRef.current = handleSaveAndProcessReturn;
  });

  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'F2') {
        e.preventDefault();
        handleSaveAndProcessReturnRef.current();
      } else if (e.key === 'F10') {
        e.preventDefault();
        if (savedReturns.length > 0) {
          setActivePrintReturn(savedReturns[0]);
          setShowPrintModal(true);
        }
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [savedReturns]);

  return (
    <div className="flex flex-col h-full bg-[#f4f7fa] text-slate-800 select-none overflow-hidden" dir="rtl">
      {/* Top Title Bar - WinForms Style */}
      <div className="bg-white text-slate-800 px-3 py-1.5 flex items-center justify-between shadow-xs border-b border-slate-200 shrink-0">
        <div className="flex items-center gap-2">
          <div className="w-7 h-7 rounded-lg bg-rose-50 border border-rose-200 flex items-center justify-center text-rose-700 shadow-xs">
            <RotateCcw className="w-4 h-4 text-rose-700" />
          </div>
          <span className="font-bold text-xs text-slate-900 tracking-wide">
            شاشة مردودات المبيعات واسترجاع البضاعة [Sales Returns & Credit Notes - Desktop DataGrid]
          </span>
          <span className="bg-rose-50 text-rose-700 text-[10px] px-2 py-0.5 rounded-full border border-rose-200 font-bold">
            تأثير محاسبي ومخزني مباشر
          </span>
        </div>
        <div className="flex items-center gap-2 text-xs">
          <button
            onClick={() => setActiveTab('NEW_RETURN')}
            className={`px-3 py-1 rounded-lg text-xs font-bold transition-all cursor-pointer ${
              activeTab === 'NEW_RETURN'
                ? 'bg-rose-600 text-white shadow-xs'
                : 'text-slate-600 hover:bg-slate-100'
            }`}
          >
            سند مردود جديد [F2]
          </button>
          <button
            onClick={() => setActiveTab('RETURNS_HISTORY')}
            className={`px-3 py-1 rounded-lg text-xs font-bold transition-all cursor-pointer ${
              activeTab === 'RETURNS_HISTORY'
                ? 'bg-rose-600 text-white shadow-xs'
                : 'text-slate-600 hover:bg-slate-100'
            }`}
          >
            سجل المردودات ({savedReturns.length})
          </button>
          <button
            onClick={onBack}
            className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2.5 py-1 rounded-lg text-xs flex items-center gap-1 border border-slate-300 transition cursor-pointer font-medium"
          >
            <ArrowRight className="w-3.5 h-3.5" />
            <span>خروج</span>
          </button>
        </div>
      </div>

      {/* Feedback Toast */}
      {feedback && (
        <div
          className={`px-4 py-2 flex items-center justify-between text-xs font-bold shrink-0 ${
            feedback.type === 'success' ? 'bg-emerald-600 text-white' : 'bg-rose-600 text-white'
          }`}
        >
          <div className="flex items-center gap-2">
            {feedback.type === 'success' ? <CheckCircle2 className="w-4 h-4" /> : <AlertCircle className="w-4 h-4" />}
            <span>{feedback.text}</span>
          </div>
          <button onClick={() => setFeedback(null)} className="hover:opacity-80">
            <X className="w-3.5 h-3.5" />
          </button>
        </div>
      )}

      {activeTab === 'NEW_RETURN' ? (
        <div className="flex-1 flex flex-col p-2.5 gap-2 overflow-hidden">
          {/* Header Controls Card */}
          <div className="bg-white rounded-md border border-slate-300 shadow-xs p-3 shrink-0">
            <div className="grid grid-cols-1 md:grid-cols-4 lg:grid-cols-6 gap-2.5 text-xs">
              {/* Return Number */}
              <div>
                <label className="font-bold text-slate-700 flex items-center gap-1 mb-1">
                  <FileText className="w-3.5 h-3.5 text-rose-600" />
                  <span>رقم سند المردود:</span>
                </label>
                <input
                  type="text"
                  value={returnNo}
                  onChange={(e) => setReturnNo(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-300 rounded px-2 py-1 font-bold text-rose-700"
                />
              </div>

              {/* Original Invoice Search */}
              <div className="relative md:col-span-2">
                <label className="font-bold text-slate-700 flex items-center justify-between mb-1">
                  <span className="flex items-center gap-1">
                    <Search className="w-3.5 h-3.5 text-sky-600" />
                    <span>رقم الفاتورة الأصلية / العميل:</span>
                  </span>
                  {selectedInvoice && (
                    <span className="text-[10px] text-emerald-600 font-bold">
                      تم الربط ({selectedInvoice.invoiceNo})
                    </span>
                  )}
                </label>
                <div className="relative">
                  <input
                    type="text"
                    placeholder="ابحث برقم الفاتورة أو اسم العميل..."
                    value={searchInvoiceInput}
                    onChange={(e) => setSearchInvoiceInput(e.target.value)}
                    className="w-full bg-white border border-sky-400 rounded px-2 py-1 text-slate-800 font-semibold focus:ring-1 focus:ring-sky-500 outline-hidden"
                  />
                  {invoiceSuggestions.length > 0 && (
                    <div className="absolute top-full right-0 left-0 mt-1 bg-white border border-slate-300 rounded shadow-lg z-30 max-h-48 overflow-y-auto">
                      {invoiceSuggestions.map((inv) => (
                        <button
                          key={inv.id}
                          type="button"
                          onClick={() => handleSelectInvoice(inv)}
                          className="w-full text-right px-3 py-2 text-xs hover:bg-sky-50 border-b border-slate-100 flex items-center justify-between"
                        >
                          <div>
                            <span className="font-bold text-sky-700">{inv.invoiceNo}</span>
                            <span className="text-slate-600 mr-2">| {inv.customerOrSupplierName || 'عميل نقدي'}</span>
                          </div>
                          <span className="text-emerald-700 font-bold">
                            {inv.grandTotal?.toLocaleString()} {currency}
                          </span>
                        </button>
                      ))}
                    </div>
                  )}
                </div>
              </div>

              {/* Customer Name */}
              <div>
                <label className="font-bold text-slate-700 flex items-center gap-1 mb-1">
                  <User className="w-3.5 h-3.5 text-slate-600" />
                  <span>اسم العميل:</span>
                </label>
                <input
                  type="text"
                  value={customerName}
                  onChange={(e) => setCustomerName(e.target.value)}
                  placeholder="اسم العميل المسترجع..."
                  className="w-full bg-white border border-slate-300 rounded px-2 py-1 text-slate-800 font-semibold"
                />
              </div>

              {/* Return Warehouse */}
              <div>
                <label className="font-bold text-slate-700 flex items-center gap-1 mb-1">
                  <Building2 className="w-3.5 h-3.5 text-amber-600" />
                  <span>المستودع المستلم:</span>
                </label>
                <select
                  value={selectedWarehouseId}
                  onChange={(e) => setSelectedWarehouseId(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded px-2 py-1 text-slate-800 font-semibold"
                >
                  {warehouses.map((w) => (
                    <option key={w.id} value={w.id}>
                      {w.name}
                    </option>
                  ))}
                </select>
              </div>

              {/* Refund Method */}
              <div>
                <label className="font-bold text-slate-700 flex items-center gap-1 mb-1">
                  <DollarSign className="w-3.5 h-3.5 text-emerald-600" />
                  <span>طريقة رد المبلغ:</span>
                </label>
                <select
                  value={refundMethod}
                  onChange={(e) => setRefundMethod(e.target.value as any)}
                  className="w-full bg-white border border-slate-300 rounded px-2 py-1 font-bold text-slate-800"
                >
                  <option value="CASH">نقداً من الصندوق (Cash)</option>
                  <option value="CREDIT_DEDUCT">خصم من رصيد العميل الآجل (Credit)</option>
                  <option value="BANK">تحويل بنكي / كريمي (Bank/Kuraymi)</option>
                  <option value="VOUCHER">قسيمة شراء رصيد محل (Store Credit)</option>
                </select>
              </div>
            </div>

            {/* Quick Actions Bar */}
            <div className="mt-2.5 pt-2 border-t border-slate-200 flex flex-wrap items-center justify-between gap-2 text-xs">
              <div className="flex items-center gap-2">
                <button
                  type="button"
                  onClick={handleReturnAll}
                  disabled={returnItems.length === 0}
                  className="bg-rose-50 hover:bg-rose-100 text-rose-800 border border-rose-300 px-2.5 py-1 rounded font-bold disabled:opacity-50"
                >
                  إرجاع كامل الأصناف بالكامل
                </button>
                <button
                  type="button"
                  onClick={handleClearQuantities}
                  disabled={returnItems.length === 0}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-300 px-2.5 py-1 rounded font-semibold disabled:opacity-50"
                >
                  تصفير الكميات
                </button>
              </div>

              <div className="flex flex-wrap items-center gap-2">
                <span className="text-slate-600">
                  الأصناف المحددة: <strong className="text-rose-700">{activeReturnLines.length}</strong> صنف (
                  {totalReturnUnitsCount} وحدة)
                </span>
                {totalReturnDiscountAmount > 0 && (
                  <span className="bg-amber-50 text-amber-900 border border-amber-300 px-2 py-0.5 rounded font-bold text-xs">
                    خصم مسترجع: {totalReturnDiscountAmount.toLocaleString()} {currency}
                  </span>
                )}
                {totalReturnTaxAmount > 0 && (
                  <span className="bg-sky-50 text-sky-900 border border-sky-300 px-2 py-0.5 rounded font-bold text-xs">
                    ضريبة مسترجعة: {totalReturnTaxAmount.toLocaleString()} {currency}
                  </span>
                )}
                <span className="bg-rose-100 text-rose-900 border border-rose-300 px-3 py-1 rounded-md font-bold text-sm">
                  صافي المردود المستحق: {totalReturnAmount.toLocaleString()} {currency}
                </span>
              </div>
            </div>
          </div>

          {/* DataGrid Table Card */}
          <div className="flex-1 bg-white rounded-md border border-slate-300 shadow-xs flex flex-col overflow-hidden">
            <div className="bg-slate-100 px-3 py-2 border-b border-slate-200 flex items-center justify-between text-xs font-bold text-slate-700">
              <div className="flex items-center gap-2">
                <Layers className="w-4 h-4 text-rose-600" />
                <span>جدول أصناف الفاتورة الأصلية وتحديد الكميات المسترجعة</span>
              </div>
              <span className="text-[11px] text-emerald-700 font-bold bg-emerald-50 px-2 py-0.5 rounded border border-emerald-200">
                ⚡ نظام توزيع الضريبة والخصم الذكي (Proportional Tax & Discount)
              </span>
            </div>

            <div className="flex-1 overflow-auto">
              {returnItems.length === 0 ? (
                <div className="flex flex-col items-center justify-center h-full p-8 text-slate-400">
                  <Search className="w-12 h-12 text-slate-300 mb-2" />
                  <p className="font-bold text-slate-600 text-sm">لم يتم تحديد فاتورة مبيعات بعد</p>
                  <p className="text-xs text-slate-500 mt-1">
                    أدخل رقم الفاتورة في الحقل العلوي أو اختر من سجل الفواتير لعرض الأصناف القابلة للإرجاع.
                  </p>
                  {invoicesHistory.length > 0 && (
                    <div className="mt-4 flex flex-wrap gap-2 justify-center max-w-lg">
                      <span className="text-xs font-bold text-slate-600 w-full text-center">
                        أو اختر فاتورة سريعة من الفواتير الأخيرة:
                      </span>
                      {invoicesHistory.slice(0, 4).map((inv) => (
                        <button
                          key={inv.id}
                          onClick={() => handleSelectInvoice(inv)}
                          className="bg-sky-50 hover:bg-sky-100 text-sky-800 border border-sky-300 px-2.5 py-1 rounded text-xs font-bold"
                        >
                          {inv.invoiceNo} ({inv.customerOrSupplierName || 'عميل نقدي'})
                        </button>
                      ))}
                    </div>
                  )}
                </div>
              ) : (
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-[#f1f5f9] text-slate-700 sticky top-0 z-10 border-b border-slate-300 font-bold">
                    <tr>
                      <th className="p-2 border-l border-slate-300 w-10 text-center">#</th>
                      <th className="p-2 border-l border-slate-300">اسم الصنف والباركود</th>
                      <th className="p-2 border-l border-slate-300 w-20 text-center">الوحدة</th>
                      <th className="p-2 border-l border-slate-300 w-20 text-center">الكمية المباعة</th>
                      <th className="p-2 border-l border-slate-300 w-24 text-center bg-rose-50 text-rose-900">
                        الكمية المرتجعة
                      </th>
                      <th className="p-2 border-l border-slate-300 w-24 text-left">السعر الأصلي</th>
                      <th className="p-2 border-l border-slate-300 w-24 text-left text-amber-700 bg-amber-50/50">
                        الخصم الموزع/الوحدة
                      </th>
                      <th className="p-2 border-l border-slate-300 w-24 text-left text-sky-700 bg-sky-50/50">
                        الضريبة/الوحدة
                      </th>
                      <th className="p-2 border-l border-slate-300 w-24 text-left font-bold text-slate-900">
                        صافي الوحدة
                      </th>
                      <th className="p-2 border-l border-slate-300 w-28 text-left bg-rose-50 text-rose-900">
                        إجمالي المردود
                      </th>
                      <th className="p-2 border-l border-slate-300 w-44">سبب الإرجاع</th>
                      <th className="p-2 border-l border-slate-300 w-32">حالة البضاعة</th>
                      <th className="p-2">ملاحظات</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-200">
                    {returnItems.map((item, idx) => {
                      const isReturning = item.returnQty > 0;
                      const lineRefundTotal = item.returnQty * item.effectiveUnitPriceWithTax;

                      return (
                        <tr
                          key={item.id}
                          className={`hover:bg-slate-50 transition-colors ${
                            isReturning ? 'bg-rose-50/40 font-medium' : ''
                          }`}
                        >
                          <td className="p-2 border-l border-slate-200 text-center font-bold text-slate-500">
                            {idx + 1}
                          </td>
                          <td className="p-2 border-l border-slate-200">
                            <div className="font-bold text-slate-900">{item.product.name}</div>
                            <div className="text-[10px] text-slate-500 font-mono">
                              {item.product.barcode || item.product.code}
                            </div>
                          </td>
                          <td className="p-2 border-l border-slate-200 text-center">{item.unit}</td>
                          <td className="p-2 border-l border-slate-200 text-center font-bold text-slate-700">
                            {item.originalQty}
                          </td>
                          <td className="p-1 border-l border-slate-200 bg-rose-50/60">
                            <input
                              type="number"
                              min="0"
                              max={item.originalQty - item.previouslyReturnedQty}
                              step="any"
                              value={item.returnQty === 0 ? '' : item.returnQty}
                              placeholder="0"
                              onChange={(e) => handleUpdateLineItem(idx, 'returnQty', e.target.value)}
                              className="w-full text-center bg-white border border-rose-400 rounded px-1 py-1 font-bold text-rose-800 text-sm focus:ring-1 focus:ring-rose-500 outline-hidden"
                            />
                          </td>
                          <td className="p-2 border-l border-slate-200 text-left font-mono text-slate-600">
                            {item.unitPrice.toLocaleString()} {currency}
                          </td>
                          <td className="p-2 border-l border-slate-200 text-left font-mono text-amber-700 bg-amber-50/30">
                            {item.unitDiscount > 0 ? `-${item.unitDiscount.toLocaleString()}` : '0'}
                          </td>
                          <td className="p-2 border-l border-slate-200 text-left font-mono text-sky-700 bg-sky-50/30">
                            {item.unitTax > 0 ? `+${item.unitTax.toLocaleString()}` : '0'}
                          </td>
                          <td className="p-2 border-l border-slate-200 text-left font-mono font-bold text-slate-900">
                            {item.effectiveUnitPriceWithTax.toLocaleString()}
                          </td>
                          <td className="p-2 border-l border-slate-200 text-left font-mono font-bold bg-rose-50/60 text-rose-900">
                            {lineRefundTotal.toLocaleString()} {currency}
                          </td>
                          <td className="p-1 border-l border-slate-200">
                            <select
                              value={item.reason}
                              disabled={!isReturning}
                              onChange={(e) => handleUpdateLineItem(idx, 'reason', e.target.value)}
                              className="w-full bg-white border border-slate-300 rounded px-1.5 py-1 text-xs text-slate-800 disabled:bg-slate-100 disabled:text-slate-400"
                            >
                              {RETURN_REASONS.map((r) => (
                                <option key={r} value={r}>
                                  {r}
                                </option>
                              ))}
                            </select>
                          </td>
                          <td className="p-1 border-l border-slate-200">
                            <select
                              value={item.condition}
                              disabled={!isReturning}
                              onChange={(e) => handleUpdateLineItem(idx, 'condition', e.target.value as any)}
                              className="w-full bg-white border border-slate-300 rounded px-1.5 py-1 text-xs font-bold text-slate-800 disabled:bg-slate-100 disabled:text-slate-400"
                            >
                              <option value="RESTOCK">إعادة للمخزن للبيع (سليم)</option>
                              <option value="DAMAGED_QUARANTINE">حجر تالف (غير صالح للبيع)</option>
                            </select>
                          </td>
                          <td className="p-1">
                            <input
                              type="text"
                              value={item.notes || ''}
                              placeholder="ملاحظات الصنف..."
                              disabled={!isReturning}
                              onChange={(e) => handleUpdateLineItem(idx, 'notes', e.target.value)}
                              className="w-full bg-white border border-slate-300 rounded px-1.5 py-1 text-xs disabled:bg-slate-100"
                            />
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              )}
            </div>

            {/* Bottom Bar Controls & Notes */}
            <div className="p-2.5 bg-slate-50 border-t border-slate-300 flex flex-wrap items-center justify-between gap-3 text-xs shrink-0">
              <div className="flex-1 max-w-xl">
                <input
                  type="text"
                  value={generalNotes}
                  onChange={(e) => setGeneralNotes(e.target.value)}
                  placeholder="ملاحظات عامة حول سبب الإرجاع وسند المردود..."
                  className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 text-xs text-slate-800"
                />
              </div>

              <div className="flex items-center gap-2">
                <button
                  type="button"
                  onClick={handleSaveAndProcessReturn}
                  disabled={activeReturnLines.length === 0}
                  className="bg-[#881337] hover:bg-rose-900 text-white font-bold px-4 py-2 rounded shadow-xs flex items-center gap-1.5 text-xs transition-all disabled:opacity-50"
                >
                  <Save className="w-4 h-4 text-rose-200" />
                  <span>حفظ وترحيل سند المردود [F2]</span>
                </button>
              </div>
            </div>
          </div>
        </div>
      ) : (
        /* Srv History Tab */
        <div className="flex-1 flex flex-col p-2.5 overflow-hidden">
          <div className="bg-white rounded-md border border-slate-300 shadow-xs flex flex-col flex-1 overflow-hidden">
            <div className="bg-slate-100 px-3 py-2 border-b border-slate-200 flex items-center justify-between text-xs font-bold text-slate-700">
              <div className="flex items-center gap-2">
                <RotateCcw className="w-4 h-4 text-rose-600" />
                <span>سجل سندات مردودات المبيعات المحفوظة</span>
              </div>
              <span className="text-slate-500 font-normal">إجمالي السندات: {savedReturns.length}</span>
            </div>

            <div className="flex-1 overflow-auto">
              <table className="w-full text-right border-collapse text-xs">
                <thead className="bg-[#f1f5f9] text-slate-700 sticky top-0 border-b border-slate-300 font-bold">
                  <tr>
                    <th className="p-2.5 border-l border-slate-300">رقم السند</th>
                    <th className="p-2.5 border-l border-slate-300">رقم الفاتورة الأصلية</th>
                    <th className="p-2.5 border-l border-slate-300">التاريخ والوقت</th>
                    <th className="p-2.5 border-l border-slate-300">العميل</th>
                    <th className="p-2.5 border-l border-slate-300">المستودع</th>
                    <th className="p-2.5 border-l border-slate-300">طريقة رد المبلغ</th>
                    <th className="p-2.5 border-l border-slate-300 text-left">قيمة المردود</th>
                    <th className="p-2.5 border-l border-slate-300 text-center">عدد الأصناف</th>
                    <th className="p-2.5 text-center w-24">إجراءات</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-200">
                  {savedReturns.map((ret) => (
                    <tr key={ret.id} className="hover:bg-slate-50">
                      <td className="p-2.5 border-l border-slate-200 font-bold text-rose-700 font-mono">
                        {ret.returnNo}
                      </td>
                      <td className="p-2.5 border-l border-slate-200 font-bold text-sky-700 font-mono">
                        {ret.originalInvoiceNo}
                      </td>
                      <td className="p-2.5 border-l border-slate-200 text-slate-600">
                        {ret.returnDate} {ret.returnTime}
                      </td>
                      <td className="p-2.5 border-l border-slate-200 font-bold text-slate-800">
                        {ret.customerName}
                      </td>
                      <td className="p-2.5 border-l border-slate-200 text-slate-700">{ret.warehouseName}</td>
                      <td className="p-2.5 border-l border-slate-200">
                        <span className="bg-slate-100 text-slate-800 px-2 py-0.5 rounded text-[11px] font-semibold border border-slate-200">
                          {ret.refundMethod === 'CASH'
                            ? 'نقداً من الصندوق'
                            : ret.refundMethod === 'CREDIT_DEDUCT'
                            ? 'خصم من رصيد الآجل'
                            : ret.refundMethod === 'BANK'
                            ? 'تحويل بنكي'
                            : 'قسيمة شراء'}
                        </span>
                      </td>
                      <td className="p-2.5 border-l border-slate-200 text-left font-bold font-mono text-rose-800">
                        {ret.totalRefundAmount.toLocaleString()} {currency}
                      </td>
                      <td className="p-2.5 border-l border-slate-200 text-center font-bold">
                        {ret.items.length} صنف
                      </td>
                      <td className="p-2 text-center">
                        <button
                          onClick={() => {
                            setActivePrintReturn(ret);
                            setShowPrintModal(true);
                          }}
                          className="bg-rose-50 hover:bg-rose-100 text-rose-800 border border-rose-300 p-1.5 rounded flex items-center justify-center gap-1 mx-auto text-xs font-bold"
                          title="طباعة سند المردود"
                        >
                          <Printer className="w-3.5 h-3.5" />
                          <span>طباعة</span>
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      )}

      {/* Print Return Receipt Modal */}
      {showPrintModal && activePrintReturn && (
        <div className="fixed inset-0 z-50 bg-black/60 backdrop-blur-xs flex items-center justify-center p-4">
          <div className="bg-white rounded-lg shadow-2xl max-w-md w-full border border-slate-300 overflow-hidden flex flex-col max-h-[90vh]">
            <div className="bg-[#881337] text-white px-4 py-2.5 flex items-center justify-between font-bold text-xs">
              <div className="flex items-center gap-2">
                <Printer className="w-4 h-4 text-rose-300" />
                <span>معاينة سند إرجاع مبيعات (Sales Return Voucher)</span>
              </div>
              <button onClick={() => setShowPrintModal(false)} className="hover:opacity-80">
                <X className="w-4 h-4" />
              </button>
            </div>

            <div className="p-4 overflow-y-auto font-mono text-xs text-slate-800 bg-white" id="printable-return-receipt">
              <div className="text-center pb-3 border-b border-dashed border-slate-400 space-y-1">
                <h3 className="font-bold text-sm text-slate-900">سوبرماركت ومحلات اليمن الحديثة</h3>
                <p className="text-[11px] text-slate-600">سند إرجاع مبيعات وإشعار دائن للعميل</p>
                <div className="inline-block bg-rose-100 text-rose-900 px-2 py-0.5 rounded text-[11px] font-bold mt-1">
                  رقم السند: {activePrintReturn.returnNo}
                </div>
              </div>

              <div className="py-2.5 border-b border-dashed border-slate-400 space-y-1 text-[11px]">
                <div className="flex justify-between">
                  <span>الفاتورة الأصلية:</span>
                  <strong>{activePrintReturn.originalInvoiceNo}</strong>
                </div>
                <div className="flex justify-between">
                  <span>التاريخ والوقت:</span>
                  <span>
                    {activePrintReturn.returnDate} {activePrintReturn.returnTime}
                  </span>
                </div>
                <div className="flex justify-between">
                  <span>العميل:</span>
                  <strong>{activePrintReturn.customerName}</strong>
                </div>
                <div className="flex justify-between">
                  <span>المستودع المستلم:</span>
                  <span>{activePrintReturn.warehouseName}</span>
                </div>
                <div className="flex justify-between">
                  <span>طريقة رد المبلغ:</span>
                  <span>
                    {activePrintReturn.refundMethod === 'CASH'
                      ? 'نقداً من الصندوق'
                      : activePrintReturn.refundMethod === 'CREDIT_DEDUCT'
                      ? 'خصم من رصيد الآجل'
                      : 'تحويل بنكي'}
                  </span>
                </div>
                <div className="flex justify-between">
                  <span>الكاشير / الموظف:</span>
                  <span>{activePrintReturn.cashierName}</span>
                </div>
              </div>

              {/* Items */}
              <div className="py-2.5 border-b border-dashed border-slate-400">
                <table className="w-full text-[11px]">
                  <thead>
                    <tr className="border-b border-slate-300 text-slate-600">
                      <th className="text-right pb-1">الصنف</th>
                      <th className="text-center pb-1">الكمية</th>
                      <th className="text-left pb-1">السعر</th>
                      <th className="text-left pb-1">المجموع</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {activePrintReturn.items.map((it, idx) => (
                      <tr key={idx} className="py-1">
                        <td className="py-1">
                          <div className="font-bold">{it.product.name}</div>
                          <div className="text-[9px] text-slate-500">{it.reason}</div>
                        </td>
                        <td className="text-center font-bold">
                          {it.returnQty} {it.unit}
                        </td>
                        <td className="text-left font-mono">{it.unitPrice.toLocaleString()}</td>
                        <td className="text-left font-bold font-mono">
                          {(it.returnQty * it.unitPrice).toLocaleString()}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>

              {/* Summary */}
              <div className="py-2.5 space-y-1 text-[11px]">
                <div className="flex justify-between font-bold text-sm text-rose-900 pt-1 border-t border-slate-300">
                  <span>إجمالي المبلغ المسترجع:</span>
                  <span>
                    {activePrintReturn.totalRefundAmount.toLocaleString()} {currency}
                  </span>
                </div>
              </div>

              {activePrintReturn.notes && (
                <div className="mt-2 p-1.5 bg-slate-50 border border-slate-200 rounded text-[10px] text-slate-600">
                  <strong>ملاحظات:</strong> {activePrintReturn.notes}
                </div>
              )}

              <div className="text-center pt-4 text-[10px] text-slate-500 space-y-0.5">
                <p>نظام اليمن للمحاسبة ونقاط البيع | POS & ERP System</p>
                <p>شكراً لتعاملكم معنا</p>
              </div>
            </div>

            <div className="p-3 bg-slate-100 border-t border-slate-300 flex items-center justify-between">
              <button
                onClick={() => window.print()}
                className="bg-[#881337] hover:bg-rose-900 text-white px-4 py-1.5 rounded font-bold text-xs flex items-center gap-1.5"
              >
                <Printer className="w-3.5 h-3.5" />
                <span>طباعة السند الآن</span>
              </button>
              <button
                onClick={() => setShowPrintModal(false)}
                className="bg-slate-300 hover:bg-slate-400 text-slate-800 px-3 py-1.5 rounded font-bold text-xs"
              >
                إغلاق
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
