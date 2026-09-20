import React, { useState, useMemo, useRef, useEffect } from 'react';
import {
  FileText,
  Search,
  Barcode,
  Plus,
  Trash2,
  Printer,
  Save,
  ArrowRight,
  Sparkles,
  Calendar,
  Building2,
  Layers,
  CheckCircle2,
  AlertCircle,
  FileCheck,
  RefreshCw,
  Send,
  Eye,
  X
} from 'lucide-react';
import { ProductItem, Customer, Warehouse } from '@/types/pos';

export interface QuotationItem {
  id: string;
  product: ProductItem;
  quantity: number;
  unitPrice: number;
  discount: number;
  total: number;
  unit: string;
  unitFactor: number;
  expiryDate?: string;
  notes?: string;
}

export interface SavedQuotation {
  quotationNo: string;
  customerName: string;
  date: string;
  validUntil: string;
  warehouseId: string;
  notes: string;
  items: QuotationItem[];
  subtotal: number;
  discountType: 'AMOUNT' | 'PERCENT';
  discountVal: string;
  discountAmount: number;
  grandTotal: number;
  status: 'PENDING' | 'CONVERTED' | 'CANCELLED';
  createdAt: string;
}

interface PriceQuotationViewProps {
  products: ProductItem[];
  customers: Customer[];
  warehouses: Warehouse[];
  currency?: string;
  cashierName?: string;
  onConvertToSalesInvoice: (data: {
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
  }) => void;
  onBack: () => void;
}

export const PriceQuotationView: React.FC<PriceQuotationViewProps> = ({
  products,
  customers,
  warehouses,
  currency = 'ريال يمني',
  cashierName = 'المحاسب التجاري',
  onConvertToSalesInvoice,
  onBack,
}) => {
  // Quotation Header State
  const [quotationNo, setQuotationNo] = useState(() => `${Math.floor(4001 + Math.random() * 900)}`);
  const [customerName, setCustomerName] = useState(customers[0]?.name || 'شركة الأمل للتجارة والمقاولات');
  const [quotationDate, setQuotationDate] = useState(() => new Date().toISOString().split('T')[0]);
  const [validUntilDate, setValidUntilDate] = useState(() => {
    const d = new Date();
    d.setDate(d.getDate() + 15);
    return d.toISOString().split('T')[0];
  });
  const [selectedWarehouseId, setSelectedWarehouseId] = useState(warehouses[0]?.id || 'WH-01');
  const [notes, setNotes] = useState('الأسعار سارية لمدة 15 يوماً من تاريخه - شاملة النقل والتسليم إلى مخازن العميل');

  // Barcode and Item Search
  const [barcodeInput, setBarcodeInput] = useState('');
  const [productSearchTerm, setProductSearchTerm] = useState('');
  const [selectedProductId, setSelectedProductId] = useState<string>(products[0]?.id || '');
  const barcodeInputRef = useRef<HTMLInputElement>(null);

  // DataGrid Items
  const [items, setItems] = useState<QuotationItem[]>(() => {
    const p1 = products.find(p => p.name.includes('عمان') || p.name.includes('بطاطس')) || products[0];
    const p2 = products[1] || products[0];
    const list: QuotationItem[] = [];
    if (p1) {
      list.push({
        id: 'QITEM-1',
        product: p1,
        quantity: 25,
        unitPrice: p1.salePrice,
        discount: 0,
        total: 25 * p1.salePrice,
        unit: p1.unit || 'حبة',
        unitFactor: 1,
        expiryDate: p1.expiryDate || '2028-12-31',
        notes: 'توريد دفعة أولى'
      });
    }
    if (p2 && p2.id !== p1?.id) {
      list.push({
        id: 'QITEM-2',
        product: p2,
        quantity: 10,
        unitPrice: p2.salePrice,
        discount: 0,
        total: 10 * p2.salePrice,
        unit: p2.unit || 'حبة',
        unitFactor: 1,
        expiryDate: p2.expiryDate || '2028-06-30',
        notes: ''
      });
    }
    return list;
  });

  // Discount Header State
  const [discountType, setDiscountType] = useState<'AMOUNT' | 'PERCENT'>('AMOUNT');
  const [discountVal, setDiscountVal] = useState<string>('0');

  // UI Feedback & Modals
  const [feedback, setFeedback] = useState<{ type: 'success' | 'info' | 'error'; text: string } | null>(null);
  const [showPrintModal, setShowPrintModal] = useState(false);
  const [savedQuotations, setSavedQuotations] = useState<SavedQuotation[]>([]);
  const [showHistoryModal, setShowHistoryModal] = useState(false);

  // Calculations
  const subtotal = useMemo(() => {
    return items.reduce((acc, it) => acc + (it.quantity * it.unitPrice - (it.discount || 0)), 0);
  }, [items]);

  const discountAmount = useMemo(() => {
    const val = parseFloat(discountVal) || 0;
    if (discountType === 'PERCENT') {
      return (subtotal * val) / 100;
    }
    return Math.min(subtotal, val);
  }, [discountVal, discountType, subtotal]);

  const grandTotal = Math.max(0, subtotal - discountAmount);

  // Inline Grid Change Handler
  const handleGridChange = (
    index: number,
    field: 'quantity' | 'unitPrice' | 'discount' | 'unit' | 'notes' | 'expiryDate',
    value: any
  ) => {
    setItems(prev => {
      const updated = [...prev];
      const item = { ...updated[index] };

      if (field === 'unit') {
        item.unit = value;
        const uInfo = item.product.units?.find(u => u.name === value);
        if (uInfo) {
          item.unitPrice = uInfo.price || item.product.salePrice;
          item.unitFactor = uInfo.factor || 1;
        } else {
          item.unitPrice = item.product.salePrice;
          item.unitFactor = 1;
        }
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'quantity') {
        const q = parseFloat(value) || 0;
        item.quantity = Math.max(0.001, q);
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'unitPrice') {
        const p = parseFloat(value) || 0;
        item.unitPrice = Math.max(0, p);
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'discount') {
        const d = parseFloat(value) || 0;
        item.discount = Math.max(0, d);
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'notes') {
        item.notes = value;
      } else if (field === 'expiryDate') {
        item.expiryDate = value;
      }

      item.total = Number(item.total.toFixed(4));
      updated[index] = item;
      return updated;
    });
  };

  // Add Product Row
  const handleAddProductRow = (product: ProductItem, unitName?: string) => {
    const uName = unitName || product.unit || 'حبة';
    const uInfo = product.units?.find(u => u.name === uName);
    const price = uInfo?.price || product.salePrice;
    const factor = uInfo?.factor || 1;

    setItems(prev => {
      const existing = prev.find(it => it.product.id === product.id && it.unit === uName);
      if (existing) {
        return prev.map(it => {
          if (it.product.id === product.id && it.unit === uName) {
            const newQty = it.quantity + 1;
            return {
              ...it,
              quantity: newQty,
              total: Math.max(0, (newQty * it.unitPrice) - it.discount)
            };
          }
          return it;
        });
      }

      const newItem: QuotationItem = {
        id: `QITEM-${Date.now()}-${Math.random().toString(36).substr(2, 5)}`,
        product,
        quantity: 1,
        unitPrice: price,
        discount: 0,
        total: price,
        unit: uName,
        unitFactor: factor,
        expiryDate: product.expiryDate || '2028-12-31',
        notes: ''
      };
      return [...prev, newItem];
    });

    setFeedback({ type: 'info', text: `تم إضافة الصنف: ${product.name} إلى عرض السعر` });
    setTimeout(() => setFeedback(null), 2500);
  };

  // Barcode Submit
  const handleBarcodeSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!barcodeInput.trim()) return;
    const code = barcodeInput.trim().toLowerCase();

    let foundProd: ProductItem | undefined;
    let foundUnitName: string | undefined;

    for (const p of products) {
      if (p.barcode?.toLowerCase() === code || p.code?.toLowerCase() === code) {
        foundProd = p;
        foundUnitName = p.unit;
        break;
      }
      if (p.units) {
        const u = p.units.find(u => u.barcode?.toLowerCase() === code);
        if (u) {
          foundProd = p;
          foundUnitName = u.name;
          break;
        }
      }
    }

    if (foundProd) {
      handleAddProductRow(foundProd, foundUnitName);
    } else {
      setFeedback({ type: 'error', text: `لم يتم العثور على صنف بالباركود: ${barcodeInput}` });
      setTimeout(() => setFeedback(null), 3000);
    }
    setBarcodeInput('');
    barcodeInputRef.current?.focus();
  };

  // Save Quotation Draft
  const handleSaveQuotation = () => {
    if (items.length === 0) {
      setFeedback({ type: 'error', text: 'لا يمكن حفظ عرض سعر فارغ بدون أصناف' });
      setTimeout(() => setFeedback(null), 3000);
      return;
    }

    const newQuotation: SavedQuotation = {
      quotationNo,
      customerName,
      date: quotationDate,
      validUntil: validUntilDate,
      warehouseId: selectedWarehouseId,
      notes,
      items,
      subtotal,
      discountType,
      discountVal,
      discountAmount,
      grandTotal,
      status: 'PENDING',
      createdAt: new Date().toLocaleString('ar-YE'),
    };

    setSavedQuotations(prev => [newQuotation, ...prev.filter(q => q.quotationNo !== quotationNo)]);
    setFeedback({
      type: 'success',
      text: `تم حفظ عرض السعر رقم (${quotationNo}) للعميل [${customerName}] بإجمالي ${grandTotal.toLocaleString()} ${currency} بنجاح!`,
    });
    setTimeout(() => setFeedback(null), 4000);
  };

  // Real One-Click Conversion to Sales Invoice
  const handlePerformConversion = () => {
    if (items.length === 0) {
      setFeedback({ type: 'error', text: 'يرجى إدراج أصناف أولاً قبل التحويل إلى فاتورة مبيعات' });
      setTimeout(() => setFeedback(null), 3000);
      return;
    }

    // Prepare items payload for Sales Invoice
    const conversionPayload = {
      customerName,
      notes: `محولة آلياً من عرض السعر رقم: ${quotationNo} (${notes})`,
      items: items.map(it => ({
        product: it.product,
        quantity: it.quantity,
        unitPrice: it.unitPrice,
        discount: it.discount,
        unit: it.unit,
        unitFactor: it.unitFactor,
        expiryDate: it.expiryDate,
        notes: it.notes,
      })),
    };

    // Update status in saved list
    setSavedQuotations(prev =>
      prev.map(q => (q.quotationNo === quotationNo ? { ...q, status: 'CONVERTED' } : q))
    );

    // Call navigation callback
    onConvertToSalesInvoice(conversionPayload);
  };

  // Keyboard Shortcuts (F2: Save, F9: Convert, F10: Print)
  const handleSaveQuotationRef = useRef(handleSaveQuotation);
  const handlePerformConversionRef = useRef(handlePerformConversion);

  useEffect(() => {
    handleSaveQuotationRef.current = handleSaveQuotation;
    handlePerformConversionRef.current = handlePerformConversion;
  });

  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'F2') {
        e.preventDefault();
        handleSaveQuotationRef.current();
      } else if (e.key === 'F9') {
        e.preventDefault();
        handlePerformConversionRef.current();
      } else if (e.key === 'F10') {
        e.preventDefault();
        setShowPrintModal(true);
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, []);

  const selectedWarehouse = warehouses.find(w => w.id === selectedWarehouseId);

  return (
    <div className="flex flex-col h-full bg-[#f0f4f8] text-slate-800 select-none overflow-hidden" dir="rtl">
      {/* Top Title Bar - WinForms ERP Style */}
      <div className="bg-white text-slate-800 px-3 py-1.5 flex items-center justify-between shadow-xs border-b border-slate-200 shrink-0">
        <div className="flex items-center gap-2">
          <div className="w-7 h-7 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
            <FileText className="w-4 h-4 text-sky-700" />
          </div>
          <span className="font-bold text-xs text-slate-900 tracking-wide">
            فاتورة عرض أسعار للعملاء [Commercial Price Quotation - DataGrid Back-Office]
          </span>
          <span className="bg-amber-50 text-amber-800 text-[10px] px-2 py-0.5 rounded-full border border-amber-200 font-bold">
            غير محاسبي (وثيقة تقديرية)
          </span>
        </div>

        <div className="flex items-center gap-2 text-xs">
          <button
            onClick={() => setShowHistoryModal(true)}
            className="bg-slate-50 hover:bg-slate-100 text-slate-700 px-2.5 py-1 rounded text-xs flex items-center gap-1 border border-slate-200 transition cursor-pointer font-medium"
          >
            <Eye className="w-3.5 h-3.5 text-sky-600" />
            <span>سجل العروض ({savedQuotations.length})</span>
          </button>
          <button
            onClick={onBack}
            className="bg-rose-50 hover:bg-rose-100 text-rose-700 px-2.5 py-1 rounded text-xs flex items-center gap-1 border border-rose-200 transition cursor-pointer font-medium"
          >
            <ArrowRight className="w-3.5 h-3.5" />
            <span>إغلاق / رجوع</span>
          </button>
        </div>
      </div>

      {/* Header Info Form */}
      <div className="bg-white border-b border-slate-300 p-2.5 shrink-0 shadow-2xs">
        <div className="grid grid-cols-1 md:grid-cols-5 gap-2 text-xs">
          {/* Quotation No & Warehouse */}
          <div className="space-y-1">
            <label className="font-bold text-slate-700 block">رقم عرض السعر:</label>
            <div className="flex gap-1">
              <input
                type="text"
                value={quotationNo}
                onChange={(e) => setQuotationNo(e.target.value)}
                className="w-full bg-slate-50 border border-slate-300 rounded px-2 py-1 font-mono font-bold text-sky-900 focus:bg-white focus:border-sky-500 outline-hidden"
              />
              <button
                type="button"
                onClick={() => setQuotationNo(`QT-${new Date().getFullYear()}-${String(Math.floor(Math.random() * 9000) + 1000)}`)}
                title="توليد رقم جديد"
                className="p-1 bg-slate-100 border border-slate-300 rounded hover:bg-slate-200 text-slate-600"
              >
                <RefreshCw className="w-3.5 h-3.5" />
              </button>
            </div>
          </div>

          {/* Customer Selection or Input */}
          <div className="space-y-1 md:col-span-2">
            <label className="font-bold text-slate-700 flex items-center justify-between">
              <span>العميل / الجهة المستلمة:</span>
              <span className="text-[10px] text-sky-700 font-normal">اختر من القائمة أو اكتب اسماً جديداً</span>
            </label>
            <div className="flex gap-1">
              <input
                type="text"
                value={customerName}
                onChange={(e) => setCustomerName(e.target.value)}
                placeholder="اسم العميل أو المؤسسة..."
                className="flex-1 bg-white border border-slate-300 rounded px-2 py-1 font-bold text-slate-900 focus:border-sky-500 outline-hidden"
              />
              <select
                onChange={(e) => e.target.value && setCustomerName(e.target.value)}
                className="w-28 bg-slate-50 border border-slate-300 rounded px-1 text-[11px] text-slate-700 focus:border-sky-500 outline-hidden"
              >
                <option value="">اختيار سريع...</option>
                {customers.map(c => (
                  <option key={c.id} value={c.name}>{c.name}</option>
                ))}
              </select>
            </div>
          </div>

          {/* Dates */}
          <div className="space-y-1">
            <label className="font-bold text-slate-700 block">تاريخ العرض والصلاحية:</label>
            <div className="grid grid-cols-2 gap-1">
              <input
                type="date"
                value={quotationDate}
                onChange={(e) => setQuotationDate(e.target.value)}
                className="w-full bg-slate-50 border border-slate-300 rounded px-1.5 py-1 text-[11px] font-mono text-slate-800"
                title="تاريخ تحرير العرض"
              />
              <input
                type="date"
                value={validUntilDate}
                onChange={(e) => setValidUntilDate(e.target.value)}
                className="w-full bg-slate-50 border border-slate-300 rounded px-1.5 py-1 text-[11px] font-mono text-slate-800"
                title="تاريخ انتهاء سريان العرض"
              />
            </div>
          </div>

          {/* Reference Warehouse */}
          <div className="space-y-1">
            <label className="font-bold text-slate-700 block">المستودع الاسترشادي:</label>
            <select
              value={selectedWarehouseId}
              onChange={(e) => setSelectedWarehouseId(e.target.value)}
              className="w-full bg-white border border-slate-300 rounded px-2 py-1 font-bold text-slate-800 focus:border-sky-500 outline-hidden"
            >
              {warehouses.map(w => (
                <option key={w.id} value={w.id}>
                  {w.name}
                </option>
              ))}
            </select>
          </div>
        </div>

        {/* Notes Row */}
        <div className="mt-2 pt-2 border-t border-slate-200 flex items-center gap-2 text-xs">
          <span className="font-bold text-slate-600 shrink-0">شروط العرض:</span>
          <input
            type="text"
            value={notes}
            onChange={(e) => setNotes(e.target.value)}
            placeholder="شروط التسليم والدفع والضمان..."
            className="flex-1 bg-slate-50 border border-slate-300 rounded px-2 py-1 text-slate-800 focus:bg-white focus:border-sky-500 outline-hidden text-xs"
          />
        </div>
      </div>

      {/* Barcode & Fast Search Toolbar */}
      <div className="bg-[#e8eff5] px-3 py-1.5 border-b border-slate-300 flex flex-wrap items-center justify-between gap-2 shrink-0">
        <div className="flex items-center gap-2 flex-1 max-w-2xl">
          {/* Rapid Barcode Scanner Form */}
          <form onSubmit={handleBarcodeSubmit} className="flex items-center gap-1.5">
            <div className="relative flex items-center">
              <Barcode className="w-4 h-4 text-sky-800 absolute right-2 pointer-events-none" />
              <input
                ref={barcodeInputRef}
                type="text"
                value={barcodeInput}
                onChange={(e) => setBarcodeInput(e.target.value)}
                placeholder="امسح الباركود [Enter]..."
                className="w-52 pr-8 pl-2 py-1 bg-white border border-slate-300 rounded text-xs font-mono font-bold text-slate-900 focus:border-sky-600 focus:ring-1 focus:ring-sky-500 outline-hidden shadow-2xs"
              />
            </div>
            <button
              type="submit"
              className="bg-[#0e3b64] hover:bg-sky-800 text-white px-2.5 py-1 rounded text-xs font-bold transition cursor-pointer"
            >
              إدراج
            </button>
          </form>

          {/* Product Quick Dropdown Selector */}
          <div className="flex items-center gap-1 flex-1">
            <select
              value={selectedProductId}
              onChange={(e) => setSelectedProductId(e.target.value)}
              className="flex-1 bg-white border border-slate-300 rounded px-2 py-1 text-xs font-bold text-slate-800 focus:border-sky-500 outline-hidden"
            >
              {products.map(p => (
                <option key={p.id} value={p.id}>
                  [{p.code}] {p.name} - السعر: {p.salePrice.toLocaleString()} {currency} (المتوفر: {p.stock} {p.unit})
                </option>
              ))}
            </select>
            <button
              type="button"
              onClick={() => {
                const p = products.find(prod => prod.id === selectedProductId);
                if (p) handleAddProductRow(p);
              }}
              className="bg-emerald-700 hover:bg-emerald-800 text-white px-3 py-1 rounded text-xs font-bold flex items-center gap-1 shadow-2xs transition cursor-pointer shrink-0"
            >
              <Plus className="w-3.5 h-3.5" />
              <span>إضافة سطر</span>
            </button>
          </div>
        </div>

        {/* Currency & Base Info */}
        <div className="flex items-center gap-2 text-xs">
          <span className="bg-sky-100 text-sky-900 font-bold px-2 py-0.5 rounded border border-sky-300 text-[11px]">
            العملة المحلية: {currency} (V1 Base Currency)
          </span>
          <span className="text-slate-500 font-mono text-[11px]">
            عدد البنود: {items.length}
          </span>
        </div>
      </div>

      {/* Feedback Banner */}
      {feedback && (
        <div
          className={`px-3 py-1 text-xs font-bold flex items-center justify-between transition-all ${
            feedback.type === 'success'
              ? 'bg-emerald-100 text-emerald-900 border-b border-emerald-300'
              : feedback.type === 'error'
              ? 'bg-rose-100 text-rose-900 border-b border-rose-300'
              : 'bg-sky-100 text-sky-900 border-b border-sky-300'
          }`}
        >
          <div className="flex items-center gap-1.5">
            {feedback.type === 'success' && <CheckCircle2 className="w-3.5 h-3.5 text-emerald-700" />}
            {feedback.type === 'error' && <AlertCircle className="w-3.5 h-3.5 text-rose-700" />}
            {feedback.type === 'info' && <Sparkles className="w-3.5 h-3.5 text-sky-700" />}
            <span>{feedback.text}</span>
          </div>
          <button onClick={() => setFeedback(null)} className="text-slate-500 hover:text-slate-700">
            <X className="w-3.5 h-3.5" />
          </button>
        </div>
      )}

      {/* Classic Desktop High-Density DataGrid */}
      <div className="flex-1 overflow-auto bg-white">
        <table className="w-full text-right text-xs border-collapse">
          <thead>
            <tr className="bg-slate-100 text-slate-800 border-b border-slate-300 sticky top-0 z-10 select-none text-[11px] font-bold">
              <th className="py-2 px-2 text-center w-10 border-l border-slate-200">#</th>
              <th className="py-2 px-2 border-l border-slate-200 w-28">كود / باركود</th>
              <th className="py-2 px-3 border-l border-slate-200">اسم الصنف والمواصفات</th>
              <th className="py-2 px-2 border-l border-slate-200 w-28">الوحدة</th>
              <th className="py-2 px-2 border-l border-slate-200 text-center w-20">المخزون</th>
              <th className="py-2 px-2 border-l border-slate-200 text-center w-24">الكمية</th>
              <th className="py-2 px-2 border-l border-slate-200 text-center w-28">سعر البيع ({currency})</th>
              <th className="py-2 px-2 border-l border-slate-200 text-center w-20">الخصم</th>
              <th className="py-2 px-3 border-l border-slate-200 text-left w-28">الإجمالي الصافي</th>
              <th className="py-2 px-2 border-l border-slate-200 w-28">الصلاحية</th>
              <th className="py-2 px-2 border-l border-slate-200">ملاحظات</th>
              <th className="py-2 px-1 text-center w-10">حذف</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-200">
            {items.length === 0 ? (
              <tr>
                <td colSpan={12} className="py-16 text-center text-slate-400">
                  <div className="flex flex-col items-center justify-center gap-2">
                    <FileText className="w-12 h-12 text-slate-300 stroke-1" />
                    <p className="font-bold text-slate-600 text-sm">عرض السعر فارغ حالياً</p>
                    <p className="text-xs text-slate-400">
                      امسح الباركود أو اختر الصنف ثم اضغط &quot;إضافة سطر&quot; للبدء بإعداد عرض السعر
                    </p>
                  </div>
                </td>
              </tr>
            ) : (
              items.map((item, idx) => {
                const availableUnits = item.product.units && item.product.units.length > 0
                  ? item.product.units
                  : [{ name: item.product.unit || 'حبة', factor: 1, price: item.product.salePrice, cost: item.product.costPrice, barcode: item.product.barcode }];

                return (
                  <tr key={item.id} className="hover:bg-sky-50/50 transition group">
                    {/* # */}
                    <td className="py-1.5 px-2 text-center text-slate-500 font-mono font-bold bg-slate-50/60 border-l border-slate-200">
                      {idx + 1}
                    </td>

                    {/* Barcode / Code */}
                    <td className="py-1.5 px-2 font-mono text-[11px] text-slate-600 border-l border-slate-200 truncate">
                      {item.product.barcode || item.product.code}
                    </td>

                    {/* Product Name */}
                    <td className="py-1.5 px-3 border-l border-slate-200">
                      <span className="font-bold text-slate-900 block">{item.product.name}</span>
                      <span className="text-[10px] text-slate-400 font-mono">{item.product.category}</span>
                    </td>

                    {/* Unit Dropdown */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <select
                        value={item.unit}
                        onChange={(e) => handleGridChange(idx, 'unit', e.target.value)}
                        className="w-full bg-white border border-slate-300 hover:border-sky-500 rounded px-1 py-1 text-xs font-bold text-slate-800 outline-hidden cursor-pointer"
                      >
                        {availableUnits.map(u => (
                          <option key={u.name} value={u.name}>
                            {u.name} {u.factor > 1 ? `(×${u.factor})` : ''}
                          </option>
                        ))}
                      </select>
                    </td>

                    {/* Warehouse Available Stock */}
                    <td className="py-1.5 px-2 text-center border-l border-slate-200">
                      <span className={`px-1.5 py-0.5 rounded text-[10px] font-mono font-bold ${
                        item.product.stock > 10
                          ? 'bg-emerald-100 text-emerald-800'
                          : item.product.stock > 0
                          ? 'bg-amber-100 text-amber-800'
                          : 'bg-rose-100 text-rose-800'
                      }`}>
                        {item.product.stock}
                      </span>
                    </td>

                    {/* Quantity - Inline Edit */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <input
                        type="number"
                        min={0.01}
                        step="any"
                        value={item.quantity || ''}
                        onChange={(e) => handleGridChange(idx, 'quantity', e.target.value)}
                        className="w-full bg-white border border-transparent focus:border-sky-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers font-bold text-slate-900 focus:outline-hidden focus:bg-sky-50 transition-colors"
                      />
                    </td>

                    {/* Unit Price - Inline Edit */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <input
                        type="number"
                        min={0}
                        step="any"
                        value={item.unitPrice || ''}
                        onChange={(e) => handleGridChange(idx, 'unitPrice', e.target.value)}
                        className="w-full bg-white border border-transparent focus:border-sky-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers font-bold text-slate-900 focus:outline-hidden focus:bg-sky-50 transition-colors"
                      />
                    </td>

                    {/* Direct Line Discount */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <input
                        type="number"
                        min={0}
                        step="any"
                        value={item.discount || ''}
                        onChange={(e) => handleGridChange(idx, 'discount', e.target.value)}
                        placeholder="0"
                        className="w-full bg-white border border-transparent focus:border-sky-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers text-rose-700 focus:outline-hidden focus:bg-rose-50 transition-colors"
                      />
                    </td>

                    {/* Net Total */}
                    <td className="py-1.5 px-3 border-l border-slate-200 text-left bg-sky-50/20">
                      <span className="font-mono-numbers font-black text-sky-950 text-sm">
                        {item.total.toLocaleString()}
                      </span>
                    </td>

                    {/* Expiry Date */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <input
                        type="date"
                        value={item.expiryDate || ''}
                        onChange={(e) => handleGridChange(idx, 'expiryDate', e.target.value)}
                        className="w-full bg-white border border-transparent focus:border-sky-500 hover:border-slate-300 rounded px-1 py-1 text-[10px] font-mono text-slate-700 outline-hidden"
                      />
                    </td>

                    {/* Item Notes */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <input
                        type="text"
                        value={item.notes || ''}
                        onChange={(e) => handleGridChange(idx, 'notes', e.target.value)}
                        placeholder="ملاحظات..."
                        className="w-full bg-white border border-transparent focus:border-sky-500 hover:border-slate-300 rounded px-1.5 py-1 text-slate-700 text-xs outline-hidden"
                      />
                    </td>

                    {/* Delete Action */}
                    <td className="py-1.5 px-1 text-center">
                      <button
                        type="button"
                        onClick={() => setItems(prev => prev.filter((_, i) => i !== idx))}
                        className="p-1 rounded hover:bg-rose-100 text-slate-400 hover:text-rose-600 transition cursor-pointer"
                        title="حذف البند"
                      >
                        <Trash2 className="w-3.5 h-3.5 mx-auto" />
                      </button>
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
      </div>

      {/* Classic Desktop ERP Bottom Summary & Action Controls */}
      <div className="bg-[#f0f4f8] border-t border-slate-300 p-2.5 shrink-0 shadow-lg">
        <div className="flex flex-wrap items-center justify-between gap-3">
          {/* Left Side: Financial Totals */}
          <div className="flex flex-wrap items-center gap-3 text-xs">
            {/* Subtotal */}
            <div className="bg-white border border-slate-300 rounded px-3 py-1.5 shadow-2xs">
              <span className="text-slate-500 block text-[10px] font-bold">المجموع قبل الخصم:</span>
              <span className="font-mono-numbers font-bold text-slate-800 text-sm">
                {subtotal.toLocaleString()} <span className="text-xs font-normal">{currency}</span>
              </span>
            </div>

            {/* Discount Control */}
            <div className="bg-white border border-slate-300 rounded px-3 py-1.5 flex items-center gap-2 shadow-2xs">
              <div>
                <div className="flex items-center gap-1 mb-0.5">
                  <span className="text-slate-500 text-[10px] font-bold">خصم العرض:</span>
                  <button
                    type="button"
                    onClick={() => setDiscountType(prev => (prev === 'AMOUNT' ? 'PERCENT' : 'AMOUNT'))}
                    className="bg-sky-100 text-sky-800 text-[10px] font-bold px-1 rounded hover:bg-sky-200 transition cursor-pointer"
                  >
                    {discountType === 'AMOUNT' ? 'مبلغ' : 'نسبة %'}
                  </button>
                </div>
                <div className="flex items-center gap-1">
                  <input
                    type="number"
                    min={0}
                    value={discountVal}
                    onChange={(e) => setDiscountVal(e.target.value)}
                    className="w-20 bg-slate-50 border border-slate-300 rounded px-1.5 py-0.5 text-center font-mono-numbers font-bold text-xs"
                  />
                  {discountAmount > 0 && (
                    <span className="font-mono-numbers text-rose-600 font-bold text-xs">
                      (-{discountAmount.toLocaleString()})
                    </span>
                  )}
                </div>
              </div>
            </div>

            {/* Grand Total Net */}
            <div className="bg-[#0e3b64] text-white rounded px-4 py-1.5 shadow-xs border border-sky-950">
              <span className="text-sky-200 block text-[10px] font-bold">الصافي الإجمالي لعرض السعر:</span>
              <span className="font-mono-numbers font-black text-lg text-emerald-300">
                {grandTotal.toLocaleString()} <span className="text-xs text-white font-normal">{currency}</span>
              </span>
            </div>

            {/* Accounting Notice */}
            <div className="hidden lg:flex items-center gap-1.5 text-[11px] text-amber-800 bg-amber-50 px-2.5 py-1.5 rounded border border-amber-200">
              <FileCheck className="w-4 h-4 text-amber-600 shrink-0" />
              <span>لا يترتب على حفظ عرض السعر أي قيود محاسبية أو خصم مخزني حتى يتم تحويله لفاتورة.</span>
            </div>
          </div>

          {/* Right Side: Operational Action Buttons */}
          <div className="flex items-center gap-2">
            {/* Print Quotation */}
            <button
              type="button"
              onClick={() => setShowPrintModal(true)}
              className="bg-white hover:bg-slate-100 text-slate-700 border border-slate-300 px-3 py-2 rounded text-xs font-bold flex items-center gap-1.5 transition cursor-pointer shadow-2xs"
              title="طباعة عرض سعر رسمي [F10]"
            >
              <Printer className="w-4 h-4 text-slate-600" />
              <span>طباعة العرض [F10]</span>
            </button>

            {/* Save Quotation Draft */}
            <button
              type="button"
              onClick={handleSaveQuotation}
              className="bg-sky-700 hover:bg-sky-800 text-white px-3.5 py-2 rounded text-xs font-bold flex items-center gap-1.5 transition cursor-pointer shadow-xs"
              title="حفظ عرض السعر في السجل [F2]"
            >
              <Save className="w-4 h-4" />
              <span>حفظ عرض السعر [F2]</span>
            </button>

            {/* Highlighted One-Click Conversion Button */}
            <button
              type="button"
              onClick={handlePerformConversion}
              className="bg-emerald-600 hover:bg-emerald-700 text-white px-4 py-2 rounded text-xs font-black flex items-center gap-2 transition cursor-pointer shadow-md border border-emerald-500 animate-pulse hover:animate-none"
              title="تحويل مباشر إلى فاتورة مبيعات معتمدة بضغطة زر [F9]"
            >
              <Sparkles className="w-4 h-4 text-amber-300" />
              <span>تحويل إلى فاتورة مبيعات ➔</span>
            </button>
          </div>
        </div>
      </div>

      {/* Printable A4 Quotation Modal */}
      {showPrintModal && (
        <div className="fixed inset-0 z-50 bg-black/60 flex items-center justify-center p-4 backdrop-blur-xs">
          <div className="bg-white rounded-lg shadow-2xl max-w-2xl w-full p-6 text-slate-800 space-y-4 max-h-[90vh] overflow-auto">
            <div className="flex items-center justify-between border-b pb-3">
              <div>
                <h3 className="font-bold text-base text-slate-900">عرض أسعار تجاري معتمد (Quotation Document)</h3>
                <p className="text-xs text-slate-500">جاهز للتصدير أو الطباعة المباشرة للعميل</p>
              </div>
              <button onClick={() => setShowPrintModal(false)} className="text-slate-400 hover:text-slate-600">
                <X className="w-5 h-5" />
              </button>
            </div>

            {/* Printable A4 Template */}
            <div className="border border-slate-300 p-5 rounded space-y-4 bg-white text-xs font-sans">
              <div className="flex justify-between items-start border-b pb-3">
                <div>
                  <h2 className="font-bold text-base text-sky-950">شركة الأمل للتجارة العامة والتوزيع</h2>
                  <p className="text-[11px] text-slate-500">المركز الرئيسي - صنعاء - الجمهورية اليمنية</p>
                  <p className="text-[11px] text-slate-500">هاتف: +967 1 234567 | تليجرام: @alamal_trade</p>
                </div>
                <div className="text-left font-mono">
                  <div className="font-bold text-sm text-sky-900">رقم العرض: {quotationNo}</div>
                  <div className="text-slate-500 text-[11px]">التاريخ: {quotationDate}</div>
                  <div className="text-rose-600 font-bold text-[11px]">ساري حتى: {validUntilDate}</div>
                </div>
              </div>

              <div className="bg-slate-50 p-2.5 rounded border border-slate-200">
                <div className="font-bold text-slate-900">السادة المحترمون: {customerName}</div>
                <div className="text-slate-600 text-[11px] mt-0.5">تحية طيبة وبعد، يسعدنا أن نقدم لكم هذا العرض للبنود التالية:</div>
              </div>

              <table className="w-full text-right border-collapse text-xs">
                <thead>
                  <tr className="bg-slate-100 text-slate-800 border-b border-slate-300">
                    <th className="p-1.5 text-center w-8">#</th>
                    <th className="p-1.5">بيان الصنف والمواصفات</th>
                    <th className="p-1.5 text-center w-16">الوحدة</th>
                    <th className="p-1.5 text-center w-16">الكمية</th>
                    <th className="p-1.5 text-left w-24">سعر الوحدة</th>
                    <th className="p-1.5 text-left w-28">الإجمالي ({currency})</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-200">
                  {items.map((it, idx) => (
                    <tr key={idx}>
                      <td className="p-1.5 text-center text-slate-500">{idx + 1}</td>
                      <td className="p-1.5 font-bold text-slate-900">{it.product.name}</td>
                      <td className="p-1.5 text-center text-slate-600">{it.unit}</td>
                      <td className="p-1.5 text-center font-bold font-mono">{it.quantity}</td>
                      <td className="p-1.5 text-left font-mono">{it.unitPrice.toLocaleString()}</td>
                      <td className="p-1.5 text-left font-bold font-mono text-sky-900">{it.total.toLocaleString()}</td>
                    </tr>
                  ))}
                </tbody>
              </table>

              <div className="flex justify-between items-start border-t pt-3">
                <div className="text-[11px] text-slate-600 space-y-1">
                  <p className="font-bold text-slate-800">ملاحظات وشروط التوريد:</p>
                  <p>{notes}</p>
                </div>
                <div className="w-48 space-y-1 text-xs">
                  <div className="flex justify-between">
                    <span className="text-slate-500">المجموع:</span>
                    <span className="font-mono font-bold">{subtotal.toLocaleString()}</span>
                  </div>
                  {discountAmount > 0 && (
                    <div className="flex justify-between text-rose-600">
                      <span>الخصم التجاري:</span>
                      <span className="font-mono font-bold">-{discountAmount.toLocaleString()}</span>
                    </div>
                  )}
                  <div className="flex justify-between border-t pt-1 font-bold text-sky-950 text-sm">
                    <span>الصافي النهائي:</span>
                    <span className="font-mono text-emerald-700">{grandTotal.toLocaleString()} {currency}</span>
                  </div>
                </div>
              </div>

              <div className="pt-6 border-t flex justify-between text-center text-[11px] text-slate-600">
                <div>
                  <p className="font-bold">إعداد المبيعات</p>
                  <p className="mt-4">{cashierName}</p>
                </div>
                <div>
                  <p className="font-bold">الاعتماد الإداري والمالي</p>
                  <p className="mt-4">......................................</p>
                </div>
                <div>
                  <p className="font-bold">موافقة العميل والختم</p>
                  <p className="mt-4">......................................</p>
                </div>
              </div>
            </div>

            <div className="flex items-center justify-end gap-2 pt-2">
              <button
                onClick={() => window.print()}
                className="bg-[#0e3b64] hover:bg-sky-800 text-white px-4 py-2 rounded text-xs font-bold flex items-center gap-1.5 cursor-pointer"
              >
                <Printer className="w-4 h-4" />
                <span>طباعة الأمر الآن</span>
              </button>
              <button
                onClick={() => setShowPrintModal(false)}
                className="bg-slate-200 hover:bg-slate-300 text-slate-700 px-4 py-2 rounded text-xs font-bold cursor-pointer"
              >
                إلغاء
              </button>
            </div>
          </div>
        </div>
      )}

      {/* History Modal */}
      {showHistoryModal && (
        <div className="fixed inset-0 z-50 bg-black/50 flex items-center justify-center p-4 backdrop-blur-xs">
          <div className="bg-white rounded-lg shadow-2xl max-w-3xl w-full p-5 text-slate-800 space-y-4 max-h-[85vh] flex flex-col">
            <div className="flex items-center justify-between border-b pb-2">
              <div className="flex items-center gap-2">
                <FileText className="w-5 h-5 text-sky-700" />
                <h3 className="font-bold text-sm text-slate-900">سجل عروض الأسعار المسجلة في الجلسة</h3>
              </div>
              <button onClick={() => setShowHistoryModal(false)} className="text-slate-400 hover:text-slate-600">
                <X className="w-5 h-5" />
              </button>
            </div>

            <div className="flex-1 overflow-auto">
              {savedQuotations.length === 0 ? (
                <div className="text-center py-12 text-slate-400 text-xs">
                  لا توجد عروض أسعار محفوظة حالياً في الجلسة.
                </div>
              ) : (
                <table className="w-full text-right text-xs border-collapse">
                  <thead>
                    <tr className="bg-slate-100 border-b text-slate-700">
                      <th className="p-2">رقم العرض</th>
                      <th className="p-2">العميل</th>
                      <th className="p-2">التاريخ</th>
                      <th className="p-2">البنود</th>
                      <th className="p-2 text-left">الصافي ({currency})</th>
                      <th className="p-2 text-center">الحالة</th>
                      <th className="p-2 text-center">إجراء</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y">
                    {savedQuotations.map((q) => (
                      <tr key={q.quotationNo} className="hover:bg-slate-50">
                        <td className="p-2 font-mono font-bold text-sky-900">{q.quotationNo}</td>
                        <td className="p-2 font-bold">{q.customerName}</td>
                        <td className="p-2 font-mono text-slate-500">{q.date}</td>
                        <td className="p-2">{q.items.length} صنف</td>
                        <td className="p-2 text-left font-mono font-bold text-emerald-700">
                          {q.grandTotal.toLocaleString()}
                        </td>
                        <td className="p-2 text-center">
                          <span
                            className={`px-2 py-0.5 rounded-full text-[10px] font-bold ${
                              q.status === 'CONVERTED'
                                ? 'bg-emerald-100 text-emerald-800'
                                : 'bg-amber-100 text-amber-800'
                            }`}
                          >
                            {q.status === 'CONVERTED' ? 'تم تحويله لفاتورة' : 'مسودة نشطة'}
                          </span>
                        </td>
                        <td className="p-2 text-center">
                          <button
                            onClick={() => {
                              setQuotationNo(q.quotationNo);
                              setCustomerName(q.customerName);
                              setQuotationDate(q.date);
                              setValidUntilDate(q.validUntil);
                              setNotes(q.notes);
                              setItems(q.items);
                              setDiscountType(q.discountType);
                              setDiscountVal(q.discountVal);
                              setShowHistoryModal(false);
                            }}
                            className="text-sky-600 hover:text-sky-800 font-bold text-[11px]"
                          >
                            استرجاع للتحرير
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
