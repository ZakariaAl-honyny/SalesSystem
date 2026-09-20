'use client';

import React, { useState, useMemo, useRef } from 'react';
import { ProductItem, Supplier, Invoice, Warehouse } from '@/types/pos';
import { getCurrentTimeString, generatePurchaseInvoiceNumber } from '@/lib/idGenerator';
import {
  ArrowRight,
  Save,
  Camera,
  Plus,
  X,
  Trash2,
  Image as ImageIcon,
  Check,
  AlertCircle,
  FileText,
  ChevronDown,
  Search,
  Calendar,
  Info,
  Calculator,
  ChevronLeft,
  Layers,
  Scale,
  FileCheck,
} from 'lucide-react';

import {
  PurchaseCurrencyModal,
  PurchaseCameraModal,
  PurchaseItemInfoModal,
  PurchaseOtherDetailsModal,
  PurchaseDiscountModal,
  PurchaseItemModal
} from './purchases';

interface CommercialPurchaseInvoiceViewProps {
  products: ProductItem[];
  suppliers: Supplier[];
  warehouses: Warehouse[];
  currency: string;
  cashierName: string;
  onSaveInvoice: (invoice: Invoice) => void;
  onBack: () => void;
}

interface PurchaseLineItem {
  id: string;
  product: ProductItem;
  quantity: number;
  unitPrice: number;
  total: number;
  unit: string;
  expiryDate?: string;
  batchNo?: string;
}

export const CommercialPurchaseInvoiceView: React.FC<CommercialPurchaseInvoiceViewProps> = ({
  products,
  suppliers,
  warehouses,
  currency,
  cashierName,
  onSaveInvoice,
  onBack,
}) => {
  // Top Form State (Matching Screenshots 2, 3, 4, 6)
  const [isReturn, setIsReturn] = useState(false);
  const [selectedWarehouseId, setSelectedWarehouseId] = useState(warehouses[0]?.id || 'WH-01');
  const [isCash, setIsCash] = useState(false); // false = "آجل" (Credit), true = "نقد" (Cash)
  const [selectedCurrency, setSelectedCurrency] = useState('دولار');
  const [showCurrencyModal, setShowCurrencyModal] = useState(false);
  const [selectedCashBox, setSelectedCashBox] = useState('الصندوق الرئيسي');

  // Supplier & Date
  const [supplierName, setSupplierName] = useState(suppliers[0]?.name || 'زكريا عبدالعزيز محمد الحنيني');
  const [invoiceDate, setInvoiceDate] = useState('2026-06-04');
  const [invoiceNo, setInvoiceNo] = useState<string>(() => generatePurchaseInvoiceNumber());
  const [notes, setNotes] = useState('');

  // Camera Attachment Modal State (Matching Screenshot 10)
  const [showCameraModal, setShowCameraModal] = useState(false);
  const [attachedImageName, setAttachedImageName] = useState<string | null>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);
  const cameraInputRef = useRef<HTMLInputElement>(null);

  // Item Search & Quick Add
  const [itemSearchText, setItemSearchText] = useState('');
  const [showSuggestions, setShowSuggestions] = useState(false);

  // Default initial product for Screenshot 4 & 6
  const defaultProduct = useMemo(() => {
    return (
      products.find((p) => p.name.includes('بطاطس')) ||
      products[0] || {
        id: 'P-0999',
        code: '0999',
        barcode: '629100099999',
        name: 'بطاطس عمان',
        category: 'عام',
        unit: 'حبة',
        costPrice: 0.07143,
        salePrice: 0.1,
        minSalePrice: 0.08,
        stock: 100,
        minStock: 10,
      }
    );
  }, [products]);

  // Invoice Items
  const [invoiceItems, setInvoiceItems] = useState<PurchaseLineItem[]>([
    {
      id: 'ITEM-1',
      product: defaultProduct,
      quantity: 10,
      unitPrice: 0.07143,
      total: 0.7143,
      unit: 'حبة',
      expiryDate: '2026-12-31',
    },
  ]);

  // ==============================================================
  // 1. ITEM ADD / EDIT MODAL STATE (Screenshots 1, 5, 8)
  // ==============================================================
  const [showItemModal, setShowItemModal] = useState(false);
  const [itemModalMode, setItemModalMode] = useState<'ADD' | 'EDIT'>('ADD');
  const [itemModalIndex, setItemModalIndex] = useState<number | null>(null);
  const [modalProduct, setModalProduct] = useState<ProductItem | null>(null);
  const [modalQty, setModalQty] = useState<string>('10');
  const [modalPrice, setModalPrice] = useState<string>('0.07143');
  const [modalTotal, setModalTotal] = useState<string>('0.7143');
  const [modalExpiryDate, setModalExpiryDate] = useState<string>('2026-12-31');
  const [modalUnit, setModalUnit] = useState<string>('حبة');
  const [activeModalField, setActiveModalField] = useState<'quantity' | 'unitPrice' | 'total'>('quantity');
  const [showItemInfoModal, setShowItemInfoModal] = useState(false);
  const [showOtherDetailsModal, setShowOtherDetailsModal] = useState(false);

  // Expense Accounts for Other Charges Dropdown
  const EXPENSE_ACCOUNTS_OPTIONS = [
    { code: '424', name: 'أجور نقل ومواصلات وشحن' },
    { code: '425', name: 'شحن وتوصيل طلبات' },
    { code: '426', name: 'عتالة وتحميل وتفريغ' },
    { code: '423', name: 'صيانة وضيافة ومصاريف المحل' },
    { code: '421', name: 'إيجار وكهرباء ومياه' },
    { code: '429', name: 'مصروفات تشغيلية ونثرية أخرى' },
  ];

  // ==============================================================
  // 2. DISCOUNT, TAXES & EXPENSES MODAL STATE (Screenshot 10)
  // ==============================================================
  const [showDiscountModal, setShowDiscountModal] = useState(false);
  const [discountMode, setDiscountMode] = useState<'AMOUNT' | 'PERCENT'>('AMOUNT');
  const [discountValue, setDiscountValue] = useState<string>('0.0');
  const [taxPercent, setTaxPercent] = useState<number>(0);
  const [otherExpenseAccountCode, setOtherExpenseAccountCode] = useState<string>('424');
  const [otherExpenseName, setOtherExpenseName] = useState<string>('أجور نقل ومواصلات وشحن');
  const [otherExpenseAmount, setOtherExpenseAmount] = useState<string>('0.0');
  const [customPaidAmount, setCustomPaidAmount] = useState<string>('0.0');
  const [activeDiscountField, setActiveDiscountField] = useState<'discount' | 'otherExpense' | 'paid'>('paid');
  const [showAccountingEntryPreview, setShowAccountingEntryPreview] = useState(false);

  const [feedback, setFeedback] = useState<string | null>(null);

  // Filter product suggestions
  const suggestedProducts = useMemo(() => {
    if (!itemSearchText.trim()) return [];
    const query = itemSearchText.toLowerCase();
    return products
      .filter(
        (p) =>
          p.name.toLowerCase().includes(query) ||
          p.barcode.includes(query) ||
          p.code.includes(query)
      )
      .slice(0, 6);
  }, [products, itemSearchText]);

  // Calculations
  const invoiceSubtotal = useMemo(() => {
    return invoiceItems.reduce((acc, item) => acc + item.total, 0);
  }, [invoiceItems]);

  const discountAmount = useMemo(() => {
    const val = parseFloat(discountValue) || 0;
    if (discountMode === 'PERCENT') {
      return (invoiceSubtotal * val) / 100;
    }
    return val;
  }, [discountValue, discountMode, invoiceSubtotal]);

  const afterDiscount = Math.max(0, invoiceSubtotal - discountAmount);
  const taxAmount = (afterDiscount * taxPercent) / 100;
  const otherExpenses = parseFloat(otherExpenseAmount) || 0;
  const grandTotal = Math.max(0, afterDiscount + taxAmount + otherExpenses);

  const effectivePaidAmount = useMemo(() => {
    if (isCash) return grandTotal; // Cash purchases are fully paid by default
    const manual = parseFloat(customPaidAmount);
    return isNaN(manual) ? 0 : manual;
  }, [isCash, grandTotal, customPaidAmount]);

  const remainingAmount = Math.max(0, grandTotal - effectivePaidAmount);

  // ==============================================================
  // OPEN ITEM MODAL (Add or Edit)
  // ==============================================================
  const openAddItemModal = (prod: ProductItem) => {
    setModalProduct(prod);
    setItemModalMode('ADD');
    setItemModalIndex(null);
    setModalQty('1');
    const unitP = prod.costPrice || 0.07143;
    setModalPrice(String(unitP));
    setModalTotal(String(unitP));
    setModalUnit(prod.unit || 'حبة');
    setModalExpiryDate(prod.expiryDate || '2026-12-31');
    setActiveModalField('quantity');
    setShowItemModal(true);
  };

  const openEditItemModal = (index: number) => {
    const item = invoiceItems[index];
    if (!item) return;
    setModalProduct(item.product);
    setItemModalMode('EDIT');
    setItemModalIndex(index);
    setModalQty(String(item.quantity));
    setModalPrice(String(item.unitPrice));
    setModalTotal(String(item.total));
    setModalUnit(item.unit);
    setModalExpiryDate(item.expiryDate || '2026-12-31');
    setActiveModalField('quantity');
    setShowItemModal(true);
  };

  // Virtual Keypad handling for Item Dialog (Screenshots 1, 5, 8)
  const handleItemKeypadPress = (val: string) => {
    let currentStr =
      activeModalField === 'quantity'
        ? modalQty
        : activeModalField === 'unitPrice'
        ? modalPrice
        : modalTotal;

    if (val === 'BACKSPACE') {
      currentStr = currentStr.length > 1 ? currentStr.slice(0, -1) : '0';
    } else if (val === 'CLEAR') {
      currentStr = '0';
    } else if (val === '000') {
      currentStr = currentStr === '0' ? '0' : currentStr + '000';
    } else if (val === 'Next') {
      // Rotate active field: quantity -> unitPrice -> total -> quantity
      if (activeModalField === 'quantity') setActiveModalField('unitPrice');
      else if (activeModalField === 'unitPrice') setActiveModalField('total');
      else setActiveModalField('quantity');
      return;
    } else {
      if (val === '.' && currentStr.includes('.')) return;
      currentStr = currentStr === '0' && val !== '.' ? val : currentStr + val;
    }

    const num = parseFloat(currentStr) || 0;

    if (activeModalField === 'quantity') {
      setModalQty(currentStr);
      const priceNum = parseFloat(modalPrice) || 0;
      setModalTotal((num * priceNum).toFixed(4).replace(/\.?0+$/, ''));
    } else if (activeModalField === 'unitPrice') {
      setModalPrice(currentStr);
      const qtyNum = parseFloat(modalQty) || 0;
      setModalTotal((qtyNum * num).toFixed(4).replace(/\.?0+$/, ''));
    } else {
      // Total edited -> calculate unit price
      setModalTotal(currentStr);
      const qtyNum = parseFloat(modalQty) || 1;
      if (qtyNum > 0) {
        setModalPrice((num / qtyNum).toFixed(4).replace(/\.?0+$/, ''));
      }
    }
  };

  // Add three zeros button
  const handleAppendZeros = () => {
    handleItemKeypadPress('000');
  };

  const handleSaveItemModal = (closeAfter: boolean) => {
    if (!modalProduct) return;
    const qty = parseFloat(modalQty) || 1;
    const price = parseFloat(modalPrice) || 0;
    const tot = parseFloat(modalTotal) || qty * price;

    if (itemModalMode === 'EDIT' && itemModalIndex !== null) {
      setInvoiceItems((prev) =>
        prev.map((it, idx) =>
          idx === itemModalIndex
            ? {
                ...it,
                quantity: qty,
                unitPrice: price,
                total: tot,
                unit: modalUnit,
                expiryDate: modalExpiryDate,
              }
            : it
        )
      );
    } else {
      // Add new
      const newItem: PurchaseLineItem = {
        id: `PUR-${Date.now()}`,
        product: modalProduct,
        quantity: qty,
        unitPrice: price,
        total: tot,
        unit: modalUnit,
        expiryDate: modalExpiryDate,
      };
      setInvoiceItems((prev) => [newItem, ...prev]);
    }

    if (closeAfter) {
      setShowItemModal(false);
      setItemSearchText('');
    } else {
      // Keep open for next addition
      setModalQty('1');
    }
  };

  const handleRemoveItem = (index: number) => {
    setInvoiceItems((prev) => prev.filter((_, idx) => idx !== index));
  };

  // Virtual Keypad handling for Discount & Expenses Dialog (Screenshot 10)
  const handleDiscountKeypadPress = (val: string) => {
    let currentStr =
      activeDiscountField === 'discount'
        ? discountValue
        : activeDiscountField === 'otherExpense'
        ? otherExpenseAmount
        : customPaidAmount;

    if (val === 'BACKSPACE') {
      currentStr = currentStr.length > 1 ? currentStr.slice(0, -1) : '0';
    } else if (val === 'Done') {
      setShowDiscountModal(false);
      return;
    } else {
      if (val === '.' && currentStr.includes('.')) return;
      currentStr = currentStr === '0' && val !== '.' ? val : currentStr + val;
    }

    if (activeDiscountField === 'discount') {
      setDiscountValue(currentStr);
    } else if (activeDiscountField === 'otherExpense') {
      setOtherExpenseAmount(currentStr);
    } else {
      setCustomPaidAmount(currentStr);
    }
  };

  // Save Purchase Invoice
  const handleSaveInvoice = () => {
    if (invoiceItems.length === 0) {
      setFeedback('يرجى إضافة صنف واحد على الأقل لحفظ الفاتورة');
      setTimeout(() => setFeedback(null), 3000);
      return;
    }

    const targetWarehouse = warehouses.find((w) => w.id === selectedWarehouseId) || warehouses[0];
    const matchedSupplier = suppliers.find((s) => s.name === supplierName) || suppliers[0];

    // Calculate proportional line allocation for header discount & tax on purchase invoice
    const lineNetSubtotals = invoiceItems.map(it => Math.max(0, it.quantity * it.unitPrice));
    const sumLineNets = lineNetSubtotals.reduce((a, b) => a + b, 0);

    const generatedInvoice: Invoice = {
      id: `PINV-${Date.now()}`,
      invoiceNo: invoiceNo,
      date: invoiceDate,
      time: getCurrentTimeString(),
      type: isReturn ? 'PURCHASE_RETURN' : 'PURCHASE',
      customerOrSupplierId: matchedSupplier?.id || 'SUP-01',
      customerOrSupplierName: supplierName,
      items: invoiceItems.map((it, idx) => {
        const lineNet = lineNetSubtotals[idx];
        const ratio = sumLineNets > 0 ? lineNet / sumLineNets : 1 / (invoiceItems.length || 1);
        const allocatedHeaderDisc = discountAmount * ratio;
        const allocatedTaxAmount = taxAmount * ratio;
        const netUnitPriceVal = it.quantity > 0 ? Math.max(0, (lineNet - allocatedHeaderDisc) / it.quantity) : 0;

        return {
          item: it.product,
          quantity: it.quantity,
          selectedUnit: it.unit,
          unitPrice: it.unitPrice,
          unitCost: it.unitPrice,
          discount: 0,
          allocatedDiscount: Number(allocatedHeaderDisc.toFixed(4)),
          allocatedTax: Number(allocatedTaxAmount.toFixed(4)),
          netUnitPrice: Number(netUnitPriceVal.toFixed(4)),
          taxRate: taxPercent,
          total: it.total,
        };
      }),
      subtotal: invoiceSubtotal,
      discountTotal: discountAmount,
      taxTotal: taxAmount,
      otherCharges: otherExpenses > 0 ? otherExpenses : undefined,
      otherChargesName: otherExpenses > 0 ? otherExpenseName : undefined,
      otherChargesAccountCode: otherExpenses > 0 ? otherExpenseAccountCode : undefined,
      otherChargesAccountName: otherExpenses > 0 ? otherExpenseName : undefined,
      grandTotal,
      paidAmount: effectivePaidAmount,
      remainingAmount,
      paymentType: isCash ? 'CASH' : 'CREDIT',
      cashierName,
      notes: notes || (attachedImageName ? `مرفق: ${attachedImageName}` : ''),
      warehouseId: targetWarehouse.id,
      warehouseName: targetWarehouse.name,
      status: 'POSTED',
      currency: selectedCurrency === 'محلي' ? 'YER' : selectedCurrency,
      exchangeRate: 1,
    };

    if (!isCash && remainingAmount > 0 && matchedSupplier && matchedSupplier.creditLimit && matchedSupplier.creditLimit > 0) {
      const newSupplierBalance = (matchedSupplier.currentBalance || 0) + remainingAmount;
      if (newSupplierBalance > matchedSupplier.creditLimit) {
        const confirmProceed = window.confirm(
          `تنبيه سقف الائتمان للمورد:\n\nالشراء الآجل بمبلغ (${remainingAmount.toLocaleString()}) يرفع مديونيتنا للمورد (${matchedSupplier.name}) إلى (${newSupplierBalance.toLocaleString()})، متجاوزاً سقف الائتمان المسموح به (${matchedSupplier.creditLimit.toLocaleString()}).\n\nهل ترغب في متابعة الشراء الآجل واعتماد الفاتورة؟`
        );
        if (!confirmProceed) {
          return;
        }
      }
    }

    onSaveInvoice(generatedInvoice);
    setFeedback(`تم حفظ فاتورة المشتريات رقم #${invoiceNo} بنجاح!`);
    setTimeout(() => {
      setFeedback(null);
      onBack();
    }, 1200);
  };

  return (
    <div id="commercial-purchase-invoice-view" className="flex flex-col h-full bg-[#f3f4f6] font-sans select-none" dir="rtl">
      {/* 1. Top Header Bar (Executive Light Theme) */}
      <header className="bg-white text-slate-800 px-4 py-3 flex items-center justify-between shadow-2xs shrink-0 border-b border-slate-200">
        <div className="flex items-center gap-3">
          <button
            onClick={onBack}
            className="p-1.5 hover:bg-slate-100 rounded-lg transition cursor-pointer text-slate-600 hover:text-slate-900"
            title="رجوع"
          >
            <ArrowRight className="w-5 h-5" />
          </button>
          <div className="flex items-center gap-2">
            <h1 className="text-base font-bold text-slate-900">
              {isReturn ? 'مرتجع مشتريات' : isCash ? 'فاتورة شراء نقدي' : 'فاتورة شراء آجل'}
            </h1>
            <span className="text-[11px] bg-slate-100 text-sky-700 px-2 py-0.5 rounded border border-slate-300 font-mono font-bold">
              #{invoiceNo}
            </span>
          </div>
        </div>

        {/* Save Icon Button on Far Left */}
        <button
          onClick={handleSaveInvoice}
          className="px-3.5 py-1.5 bg-emerald-600 hover:bg-emerald-500 rounded-lg transition text-white flex items-center gap-1.5 cursor-pointer text-xs font-bold shadow-xs"
          title="حفظ الفاتورة"
        >
          <Save className="w-4 h-4 text-white" />
          <span>حفظ الفاتورة</span>
        </button>
      </header>

      {/* 2. Top Form Controls */}
      <div className="bg-white border-b border-slate-200 p-3 space-y-2.5 shrink-0 text-xs shadow-xs">
        {/* Row 1: مرتجع | المخزن الرئيسي | نقد/آجل Toggle Switch */}
        <div className="flex items-center justify-between gap-2">
          {/* Checkbox: مرتجع */}
          <label className="flex items-center gap-1.5 cursor-pointer font-bold text-slate-800">
            <input
              type="checkbox"
              checked={isReturn}
              onChange={(e) => setIsReturn(e.target.checked)}
              className="w-4 h-4 text-sky-700 rounded cursor-pointer"
            />
            <span>مرتجع</span>
          </label>

          {/* Warehouse Dropdown */}
          <div className="flex items-center gap-1 bg-slate-50 border border-slate-300 rounded px-2.5 py-1">
            <span className="text-slate-500 font-medium">المستودع:</span>
            <select
              value={selectedWarehouseId}
              onChange={(e) => setSelectedWarehouseId(e.target.value)}
              className="bg-transparent font-bold text-slate-800 outline-hidden cursor-pointer"
            >
              {warehouses.map((w) => (
                <option key={w.id} value={w.id}>
                  {w.name}
                </option>
              ))}
            </select>
          </div>

          {/* Toggle Switch: نقد vs آجل */}
          <div className="flex items-center gap-1.5 font-bold text-slate-800">
            <span>{isCash ? 'نقد' : 'آجل'}</span>
            <button
              type="button"
              onClick={() => setIsCash(!isCash)}
              className={`w-11 h-6 rounded-full p-0.5 transition cursor-pointer flex items-center ${
                isCash ? 'bg-emerald-600' : 'bg-slate-400'
              }`}
            >
              <div
                className={`w-5 h-5 rounded-full bg-white shadow-xs transition-transform transform ${
                  isCash ? '-translate-x-5' : 'translate-x-0'
                }`}
              />
            </button>
          </div>
        </div>

        {/* Row 2: In Cash mode -> الصندوق ▼ & Currency ▼ | In Credit mode -> Currency ▼ directly */}
        <div className="flex items-center gap-3">
          {isCash && (
            <div className="flex-1 flex items-center gap-1 bg-slate-50 border border-slate-300 rounded px-2 py-1">
              <span className="font-bold text-slate-600">الصندوق:</span>
              <select
                value={selectedCashBox}
                onChange={(e) => setSelectedCashBox(e.target.value)}
                className="w-full bg-transparent font-bold text-slate-800 outline-hidden cursor-pointer"
              >
                <option value="الصندوق الرئيسي">الصندوق الرئيسي</option>
                <option value="صندوق المبيعات">صندوق المبيعات</option>
                <option value="صندوق فرع 1">صندوق فرع 1</option>
              </select>
            </div>
          )}

          {/* Currency Dropdown / Click opens Modal */}
          <div className="relative">
            <button
              type="button"
              onClick={() => setShowCurrencyModal(true)}
              className="flex items-center gap-1 bg-slate-50 border border-slate-300 rounded px-3 py-1 font-bold text-slate-800 hover:bg-slate-100 transition cursor-pointer"
            >
              <span>{selectedCurrency}</span>
              <ChevronDown className="w-3.5 h-3.5 text-slate-500" />
            </button>
          </div>
        </div>

        {/* Row 3: التاريخ | المورد مع زر (x) لمسح الاسم */}
        <div className="flex items-center gap-3">
          <div className="font-mono text-slate-700 font-bold shrink-0 bg-slate-100 px-2.5 py-1 rounded border border-slate-200">
            {invoiceDate}
          </div>

          <div className="flex-1 flex items-center gap-1 border-b-2 border-slate-300 pb-1 relative">
            <span className="font-bold text-slate-600 shrink-0">المورد:</span>
            <input
              type="text"
              value={supplierName}
              onChange={(e) => setSupplierName(e.target.value)}
              placeholder="اسم المورد أو الشركة..."
              className="flex-1 bg-transparent font-bold text-slate-900 outline-hidden pr-1"
            />
            {supplierName && (
              <button
                type="button"
                onClick={() => setSupplierName('')}
                className="text-slate-400 hover:text-slate-600 p-0.5 rounded cursor-pointer"
                title="مسح"
              >
                <X className="w-4 h-4" />
              </button>
            )}
          </div>
        </div>

        {/* Row 4: الكاميرا 📷 | ملاحظات | رقم# 3 */}
        <div className="flex items-center gap-3">
          {/* Camera Button */}
          <button
            type="button"
            onClick={() => setShowCameraModal(true)}
            className="p-1.5 bg-slate-100 hover:bg-slate-200 border border-slate-300 rounded-md text-slate-700 transition cursor-pointer relative"
            title="مرفقات وصور الفاتورة (V2 - قريباً)"
          >
            <Camera className="w-5 h-5 text-slate-700" />
            {attachedImageName && (
              <span className="absolute -top-1 -right-1 w-2.5 h-2.5 bg-emerald-500 rounded-full border border-white" />
            )}
          </button>

          <div className="flex-1 flex items-center gap-1 border-b-2 border-slate-300 pb-1">
            <input
              type="text"
              value={notes}
              onChange={(e) => setNotes(e.target.value)}
              placeholder="ملاحظات..."
              className="w-full bg-transparent text-slate-800 outline-hidden"
            />
          </div>

          <div className="w-24 flex items-center gap-1 border-b-2 border-slate-300 pb-1 shrink-0">
            <span className="font-bold text-slate-600">رقم#</span>
            <input
              type="text"
              value={invoiceNo}
              onChange={(e) => setInvoiceNo(e.target.value)}
              className="w-full bg-transparent font-mono font-bold text-slate-900 outline-hidden"
            />
          </div>
        </div>

        {/* Row 5: Quick Add / Search Item Bar */}
        <div className="relative">
          <div className="bg-slate-50 border border-slate-300 focus-within:border-sky-500 focus-within:ring-1 focus-within:ring-sky-400 rounded-md p-1.5 flex items-center gap-2 shadow-2xs">
            {/* (+) Plus Button to open Item Add Modal */}
            <button
              type="button"
              onClick={() => {
                const found = products.find(
                  (p) =>
                    p.name.includes(itemSearchText) ||
                    p.barcode.includes(itemSearchText) ||
                    p.code.includes(itemSearchText)
                );
                openAddItemModal(
                  found || {
                    id: `P-${Date.now()}`,
                    code: '0999',
                    barcode: '629100099999',
                    name: itemSearchText.trim() || 'صنف جديد',
                    category: 'عام',
                    unit: 'حبة',
                    costPrice: 0.07143,
                    salePrice: 0.1,
                    minSalePrice: 0.08,
                    stock: 100,
                    minStock: 10,
                  }
                );
              }}
              className="w-8 h-8 bg-sky-700 hover:bg-sky-600 text-white rounded-md flex items-center justify-center font-bold transition cursor-pointer shrink-0 shadow-2xs"
              title="إضافة صنف"
            >
              <Plus className="w-5 h-5" />
            </button>

            {/* Clear Button (x) */}
            {itemSearchText && (
              <button
                type="button"
                onClick={() => setItemSearchText('')}
                className="text-slate-400 hover:text-slate-600 p-1 cursor-pointer"
                title="مسح البحث"
              >
                <X className="w-4 h-4" />
              </button>
            )}

            {/* Search Input */}
            <input
              type="text"
              value={itemSearchText}
              onFocus={() => setShowSuggestions(true)}
              onChange={(e) => {
                setItemSearchText(e.target.value);
                setShowSuggestions(true);
              }}
              placeholder="إكتب اسم الصنف أو امسح الباركود..."
              className="flex-1 font-bold text-slate-900 outline-hidden text-xs"
            />
          </div>

          {/* Autocomplete Dropdown */}
          {showSuggestions && suggestedProducts.length > 0 && (
            <div className="absolute top-full left-0 right-0 mt-1 bg-white rounded-lg shadow-xl border border-slate-300 z-30 divide-y divide-slate-100 text-xs">
              {suggestedProducts.map((p) => (
                <div
                  key={p.id}
                  onClick={() => {
                    openAddItemModal(p);
                    setShowSuggestions(false);
                  }}
                  className="p-2.5 hover:bg-sky-50 cursor-pointer flex items-center justify-between"
                >
                  <div>
                    <span className="font-bold text-slate-900">{p.name}</span>
                    <span className="text-[10px] text-slate-500 block">باركود: {p.barcode}</span>
                  </div>
                  <span className="font-mono text-slate-700 font-bold">{p.costPrice} {selectedCurrency}</span>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>

      {feedback && (
        <div className="p-2 bg-emerald-600 text-white font-bold text-xs text-center animate-in fade-in">
          {feedback}
        </div>
      )}

      {/* 3. Items Table */}
      <main className="flex-1 overflow-y-auto bg-slate-50">
        <table className="w-full text-right text-sm border-collapse">
          <thead>
            <tr className="bg-slate-100 text-slate-700 border-b border-slate-200">
              <th className="py-2.5 px-4 font-bold text-xs">الصنف</th>
              <th className="py-2.5 px-3 font-bold text-xs text-center">الكمية</th>
              <th className="py-2.5 px-3 font-bold text-xs text-center">السعر</th>
              <th className="py-2.5 px-4 font-bold text-xs text-left">الاجمالي</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-200 bg-white">
            {invoiceItems.map((item, idx) => (
              <tr
                key={item.id}
                onClick={() => openEditItemModal(idx)}
                className="hover:bg-sky-50 transition cursor-pointer"
              >
                <td className="py-2.5 px-4">
                  <div className="font-bold text-slate-900 text-sm">{item.product.name}</div>
                  <div className="text-[11px] text-slate-500 font-sans">{item.unit}</div>
                </td>

                {/* Quantity */}
                <td className="py-2 px-3 text-center font-mono font-bold text-slate-900">
                  {item.quantity}
                </td>

                {/* Unit Price */}
                <td className="py-2 px-3 text-center font-mono font-bold text-slate-900">
                  {item.unitPrice}
                </td>

                {/* Total & Delete Button */}
                <td className="py-2.5 px-4 text-left">
                  <div className="flex items-center justify-between">
                    <button
                      onClick={(e) => {
                        e.stopPropagation();
                        handleRemoveItem(idx);
                      }}
                      className="text-slate-300 hover:text-rose-600 transition p-1"
                      title="حذف الصنف"
                    >
                      <Trash2 className="w-4 h-4" />
                    </button>
                    <span className="font-mono font-bold text-slate-900 text-sm">
                      {item.total}
                    </span>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </main>

      {/* 4. Live Double-Entry Accounting Preview */}
      {showAccountingEntryPreview && (
        <div className="bg-white border-t-2 border-sky-500 p-3 text-[11px] font-mono shadow-md animate-fade-in shrink-0">
          <div className="flex items-center justify-between border-b pb-1 mb-2 font-bold text-slate-800 font-sans">
            <span className="flex items-center gap-1.5 text-sky-800">
              <FileCheck className="w-4 h-4 text-sky-700" />
              معاينة القيد المحاسبي المزدوج التلقائي لفاتورة المشتريات [Auto Double-Entry]:
            </span>
            <span className="text-slate-500 font-mono text-[10px]">مرجع: {invoiceNo}</span>
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-2 text-[11px]">
            <div className="bg-sky-50/80 p-2.5 rounded-lg border border-sky-200 space-y-1">
              <span className="font-bold text-sky-900 block font-sans">الطرف المدين (Debit):</span>
              <div>• من حـ/ مخزون البضائع ({warehouses.find((w) => w.id === selectedWarehouseId)?.name || 'المستودع'}): <b>{invoiceSubtotal.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 3 })}</b> {selectedCurrency}</div>
              {taxAmount > 0 && (
                <div>• من حـ/ أمانات ضريبة المشتريات (VAT): <b>{taxAmount.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 3 })}</b> {selectedCurrency}</div>
              )}
              {otherExpenses > 0 && (
                <div>• من حـ/ مصاريف وأجور نقل المشتريات: <b>{otherExpenses.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 3 })}</b> {selectedCurrency}</div>
              )}
            </div>

            <div className="bg-emerald-50/80 p-2.5 rounded-lg border border-emerald-200 space-y-1">
              <span className="font-bold text-emerald-900 block font-sans">الطرف الدائن (Credit):</span>
              {discountAmount > 0 && (
                <div className="text-rose-800 font-bold">• إلى حـ/ الخصم المكتسب (521) [خصم تجاري عام]: <b>{discountAmount.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 3 })}</b> {selectedCurrency}</div>
              )}
              {isCash ? (
                <div>• إلى حـ/ الصندوق الرئيسي: <b>{effectivePaidAmount.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 3 })}</b> {selectedCurrency}</div>
              ) : (
                <>
                  <div>• إلى حـ/ الموردين ({supplierName}): <b>{remainingAmount.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 3 })}</b> {selectedCurrency}</div>
                  {effectivePaidAmount > 0 && (
                    <div>• إلى حـ/ الصندوق الرئيسي (الدفعة المسددة): <b>{effectivePaidAmount.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 3 })}</b> {selectedCurrency}</div>
                  )}
                </>
              )}
            </div>
          </div>
        </div>
      )}

      {/* 5. Bottom Summary Bar */}
      <footer
        onClick={() => setShowDiscountModal(true)}
        className="bg-white border-t border-slate-200 p-3 shrink-0 flex flex-wrap items-center justify-between text-xs cursor-pointer hover:bg-slate-50 transition gap-3 shadow-xs"
        title="انقر لتعديل الخصم والضرائب والمصاريف وطريقة الدفع"
      >
        {/* Left Column: Pill badge with item count e.g. 1 & Total amount + Accounting preview toggle */}
        <div className="flex items-center gap-2.5">
          <div className="w-8 h-8 bg-sky-700 text-white rounded-full flex items-center justify-center font-bold text-xs shadow-xs">
            {invoiceItems.length}
          </div>
          <div className="font-mono text-lg font-black text-slate-900">
            {grandTotal.toFixed(3).replace(/\.?0+$/, '')} <span className="text-xs font-normal text-slate-500 font-sans">{selectedCurrency}</span>
          </div>
          <button
            type="button"
            onClick={(e) => {
              e.stopPropagation();
              setShowAccountingEntryPreview(!showAccountingEntryPreview);
            }}
            className="mr-2 text-[11px] font-bold text-sky-800 hover:text-sky-950 bg-slate-100 hover:bg-slate-200 px-2.5 py-1 rounded-md border border-slate-300 flex items-center gap-1 shadow-2xs transition cursor-pointer"
          >
            <Scale className="w-3.5 h-3.5 text-sky-700" />
            <span>{showAccountingEntryPreview ? 'إخفاء القيد' : 'معاينة القيد'}</span>
          </button>
        </div>

        {/* Middle Column: الصافي | المبلغ المتبقي */}
        <div className="text-right space-y-1">
          <div className="flex items-center gap-1.5 justify-end font-bold text-slate-700">
            <span>الصافي:</span>
            <span className="font-mono text-slate-900">{afterDiscount.toFixed(3).replace(/\.?0+$/, '')}</span>
          </div>
          {!isCash && (
            <div className="flex items-center gap-1.5 justify-end font-bold text-amber-700">
              <span>المبلغ المتبقي:</span>
              <span className="font-mono">{remainingAmount.toFixed(3).replace(/\.?0+$/, '')}</span>
            </div>
          )}
        </div>

        {/* Right Column: الخصم العام | الضريبة% | رسوم أخرى | المبلغ المدفوع */}
        <div className="text-right space-y-1 font-bold text-slate-700 text-[11px]">
          <div className="flex items-center gap-1.5 justify-end">
            <span className="text-slate-600">الخصم:</span>
            <button
              type="button"
              onClick={(e) => {
                e.stopPropagation();
                setDiscountMode((prev) => (prev === 'AMOUNT' ? 'PERCENT' : 'AMOUNT'));
              }}
              className="text-[10px] px-1.5 py-0.5 bg-slate-100 border border-slate-300 rounded text-sky-800 hover:bg-sky-50 font-bold cursor-pointer"
              title="تبديل نوع الخصم بين مبلغ ونسبة"
            >
              {discountMode === 'AMOUNT' ? 'مبلغ' : '%'}
            </button>
            <span className="font-mono text-rose-700 font-bold">
              {discountAmount > 0 ? `-${discountAmount}` : '0'}
            </span>
          </div>
          <div className="flex items-center gap-1.5 justify-end">
            <span>الضريبة%:</span>
            <span className="font-mono text-slate-900">{taxPercent} ▼</span>
          </div>
          <div className="flex items-center gap-1.5 justify-end">
            <span>رسوم أخرى:</span>
            <span className="font-mono text-slate-900">{otherExpenses}</span>
          </div>
          {!isCash && (
            <div className="flex items-center gap-1.5 justify-end text-emerald-700">
              <span>المبلغ المدفوع:</span>
              <span className="font-mono">{effectivePaidAmount}</span>
            </div>
          )}
        </div>
      </footer>

            {/* ============================================================== */}
      {/* POPUP MODAL 1: ITEM ADD / EDIT MODAL (Screenshots 1, 5, 8)      */}
      {/* ============================================================== */}
      <PurchaseItemModal
        isOpen={showItemModal}
        modalProduct={modalProduct}
        activeModalField={activeModalField}
        setActiveModalField={setActiveModalField}
        modalQty={modalQty}
        modalPrice={modalPrice}
        modalTotal={modalTotal}
        modalUnit={modalUnit}
        itemModalMode={itemModalMode}
        handleAppendZeros={handleAppendZeros}
        handleItemKeypadPress={handleItemKeypadPress}
        handleSaveItemModal={handleSaveItemModal}
        setShowItemInfoModal={setShowItemInfoModal}
        setShowOtherDetailsModal={setShowOtherDetailsModal}
        onClose={() => setShowItemModal(false)}
      />

      {/* ============================================================== */}
      {/* POPUP MODAL 2: DISCOUNT, TAXES & EXPENSES (Screenshot 10)       */}
      {/* ============================================================== */}
      <PurchaseDiscountModal
        isOpen={showDiscountModal}
        discountMode={discountMode}
        setDiscountMode={setDiscountMode}
        activeDiscountField={activeDiscountField}
        setActiveDiscountField={setActiveDiscountField}
        discountValue={discountValue}
        taxPercent={taxPercent}
        setTaxPercent={setTaxPercent}
        handleDiscountKeypadPress={handleDiscountKeypadPress}
        onClose={() => setShowDiscountModal(false)}
      />

      {/* ============================================================== */}
      {/* POPUP MODAL 3: CURRENCY SELECTION (Screenshot 2)                */}
      {/* ============================================================== */}
      <PurchaseCurrencyModal
        isOpen={showCurrencyModal}
        selectedCurrency={selectedCurrency}
        onSelect={(curr) => {
          setSelectedCurrency(curr);
          setShowCurrencyModal(false);
        }}
        onClose={() => setShowCurrencyModal(false)}
      />

      {/* ============================================================== */}
      {/* POPUP MODAL 4: CAMERA / PHONE ATTACHMENT (Screenshot 10)       */}
      {/* ============================================================== */}
      <PurchaseCameraModal
        isOpen={showCameraModal}
        attachedImageName={attachedImageName}
        onAttach={(name) => setAttachedImageName(name)}
        onClose={() => setShowCameraModal(false)}
      />

      {/* ============================================================== */}
      {/* POPUP MODAL 5: PRODUCT INFO MODAL (Info icon in Item Dialog)    */}
      {/* ============================================================== */}
      <PurchaseItemInfoModal
        isOpen={showItemInfoModal}
        product={modalProduct}
        currency={selectedCurrency}
        onClose={() => setShowItemInfoModal(false)}
      />

      {/* ============================================================== */}
      {/* POPUP MODAL 6: OTHER DETAILS (تفاصيل أخرى & تاريخ الإنتهاء)      */}
      {/* ============================================================== */}
      <PurchaseOtherDetailsModal
        isOpen={showOtherDetailsModal}
        expiryDate={modalExpiryDate}
        onExpiryDateChange={setModalExpiryDate}
        unit={modalUnit}
        onUnitChange={setModalUnit}
        onClose={() => setShowOtherDetailsModal(false)}
      />
    </div>
  );
};
