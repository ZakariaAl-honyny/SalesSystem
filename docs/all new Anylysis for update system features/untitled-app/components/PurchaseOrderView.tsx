import React, { useState, useMemo, useRef, useEffect } from 'react';
import {
  ClipboardList,
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
  Truck,
  X
} from 'lucide-react';
import { ProductItem, Supplier, Warehouse } from '@/types/pos';

export interface PurchaseOrderItem {
  id: string;
  product: ProductItem;
  quantity: number;
  unitCost: number;
  discount: number;
  total: number;
  unit: string;
  unitFactor: number;
  batchNo?: string;
  expiryDate?: string;
  notes?: string;
}

export interface SavedPurchaseOrder {
  orderNo: string;
  supplierId: string;
  supplierName: string;
  orderDate: string;
  expectedDate: string;
  warehouseId: string;
  notes: string;
  items: PurchaseOrderItem[];
  subtotal: number;
  discountType: 'AMOUNT' | 'PERCENT';
  discountVal: string;
  discountAmount: number;
  grandTotal: number;
  status: 'PENDING' | 'CONVERTED' | 'CANCELLED';
  createdAt: string;
}

interface PurchaseOrderViewProps {
  products: ProductItem[];
  suppliers: Supplier[];
  warehouses: Warehouse[];
  currency?: string;
  cashierName?: string;
  onConvertToPurchaseInvoice: (data: {
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
  }) => void;
  onBack: () => void;
}

export const PurchaseOrderView: React.FC<PurchaseOrderViewProps> = ({
  products,
  suppliers,
  warehouses,
  currency = 'ريال يمني',
  cashierName = 'المحاسب التجاري',
  onConvertToPurchaseInvoice,
  onBack,
}) => {
  // PO Header State
  const [orderNo, setOrderNo] = useState(() => `${Math.floor(3001 + Math.random() * 900)}`);
  const [selectedSupplierId, setSelectedSupplierId] = useState<string>(suppliers[0]?.id || 'SUP-01');
  const [orderDate, setOrderDate] = useState(() => new Date().toISOString().split('T')[0]);
  const [expectedDate, setExpectedDate] = useState(() => {
    const d = new Date();
    d.setDate(d.getDate() + 7);
    return d.toISOString().split('T')[0];
  });
  const [selectedWarehouseId, setSelectedWarehouseId] = useState(warehouses[0]?.id || 'WH-01');
  const [notes, setNotes] = useState('شحن بحري/بري - التوريد والتسليم إلى المستودع المركزي');

  // Barcode & Product Search
  const [barcodeInput, setBarcodeInput] = useState('');
  const [selectedProductId, setSelectedProductId] = useState<string>(products[0]?.id || '');
  const barcodeInputRef = useRef<HTMLInputElement>(null);

  // DataGrid Items
  const [items, setItems] = useState<PurchaseOrderItem[]>(() => {
    const p1 = products[0];
    const p2 = products[1] || products[0];
    const list: PurchaseOrderItem[] = [];
    if (p1) {
      list.push({
        id: 'POITEM-1',
        product: p1,
        quantity: 50,
        unitCost: p1.costPrice,
        discount: 0,
        total: 50 * p1.costPrice,
        unit: p1.unit || 'حبة',
        unitFactor: 1,
        batchNo: `B-${new Date().getFullYear()}-01`,
        expiryDate: p1.expiryDate || '2028-12-31',
        notes: 'دفعة استيراد دورية'
      });
    }
    if (p2 && p2.id !== p1?.id) {
      list.push({
        id: 'POITEM-2',
        product: p2,
        quantity: 20,
        unitCost: p2.costPrice,
        discount: 0,
        total: 20 * p2.costPrice,
        unit: p2.unit || 'حبة',
        unitFactor: 1,
        batchNo: `B-${new Date().getFullYear()}-02`,
        expiryDate: p2.expiryDate || '2028-09-30',
        notes: ''
      });
    }
    return list;
  });

  // Discount Header State
  const [discountType, setDiscountType] = useState<'AMOUNT' | 'PERCENT'>('AMOUNT');
  const [discountVal, setDiscountVal] = useState<string>('0');

  // Feedback & Modals
  const [feedback, setFeedback] = useState<{ type: 'success' | 'info' | 'error'; text: string } | null>(null);
  const [showPrintModal, setShowPrintModal] = useState(false);
  const [savedOrders, setSavedOrders] = useState<SavedPurchaseOrder[]>([]);
  const [showHistoryModal, setShowHistoryModal] = useState(false);

  // Active Supplier
  const activeSupplier = useMemo(() => {
    return suppliers.find(s => s.id === selectedSupplierId) || suppliers[0];
  }, [suppliers, selectedSupplierId]);

  // Calculations
  const subtotal = useMemo(() => {
    return items.reduce((acc, it) => acc + (it.quantity * it.unitCost - (it.discount || 0)), 0);
  }, [items]);

  const discountAmount = useMemo(() => {
    const val = parseFloat(discountVal) || 0;
    if (discountType === 'PERCENT') {
      return (subtotal * val) / 100;
    }
    return Math.min(subtotal, val);
  }, [discountVal, discountType, subtotal]);

  const grandTotal = Math.max(0, subtotal - discountAmount);

  // Handle Inline Grid Edits
  const handleGridChange = (
    index: number,
    field: 'quantity' | 'unitCost' | 'discount' | 'unit' | 'batchNo' | 'expiryDate' | 'notes',
    value: any
  ) => {
    setItems(prev => {
      const updated = [...prev];
      const item = { ...updated[index] };

      if (field === 'unit') {
        item.unit = value;
        const uInfo = item.product.units?.find(u => u.name === value);
        if (uInfo) {
          item.unitCost = uInfo.cost || item.product.costPrice;
          item.unitFactor = uInfo.factor || 1;
        } else {
          item.unitCost = item.product.costPrice;
          item.unitFactor = 1;
        }
        item.total = Math.max(0, (item.quantity * item.unitCost) - item.discount);
      } else if (field === 'quantity') {
        const q = parseFloat(value) || 0;
        item.quantity = Math.max(0.001, q);
        item.total = Math.max(0, (item.quantity * item.unitCost) - item.discount);
      } else if (field === 'unitCost') {
        const c = parseFloat(value) || 0;
        item.unitCost = Math.max(0, c);
        item.total = Math.max(0, (item.quantity * item.unitCost) - item.discount);
      } else if (field === 'discount') {
        const d = parseFloat(value) || 0;
        item.discount = Math.max(0, d);
        item.total = Math.max(0, (item.quantity * item.unitCost) - item.discount);
      } else if (field === 'batchNo') {
        item.batchNo = value;
      } else if (field === 'expiryDate') {
        item.expiryDate = value;
      } else if (field === 'notes') {
        item.notes = value;
      }

      item.total = Number(item.total.toFixed(4));
      updated[index] = item;
      return updated;
    });
  };

  // Add Product to Grid
  const handleAddProductRow = (product: ProductItem, unitName?: string) => {
    const uName = unitName || product.unit || 'حبة';
    const uInfo = product.units?.find(u => u.name === uName);
    const cost = uInfo?.cost || product.costPrice;
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
              total: Math.max(0, (newQty * it.unitCost) - it.discount)
            };
          }
          return it;
        });
      }

      const newItem: PurchaseOrderItem = {
        id: `POITEM-${Date.now()}-${Math.random().toString(36).substr(2, 5)}`,
        product,
        quantity: 10,
        unitCost: cost,
        discount: 0,
        total: 10 * cost,
        unit: uName,
        unitFactor: factor,
        batchNo: `B-${new Date().getFullYear()}-${Math.floor(Math.random() * 90 + 10)}`,
        expiryDate: product.expiryDate || '2028-12-31',
        notes: ''
      };
      return [...prev, newItem];
    });

    setFeedback({ type: 'info', text: `تم إدراج الصنف: ${product.name} إلى أمر الشراء` });
    setTimeout(() => setFeedback(null), 2500);
  };

  // Barcode Submit Handler
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

  // Save Purchase Order Draft
  const handleSaveOrder = () => {
    if (items.length === 0) {
      setFeedback({ type: 'error', text: 'لا يمكن حفظ أمر شراء فارغ بدون بنود' });
      setTimeout(() => setFeedback(null), 3000);
      return;
    }

    const newOrder: SavedPurchaseOrder = {
      orderNo,
      supplierId: selectedSupplierId,
      supplierName: activeSupplier?.name || 'مورد عام',
      orderDate,
      expectedDate,
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

    setSavedOrders(prev => [newOrder, ...prev.filter(o => o.orderNo !== orderNo)]);
    setFeedback({
      type: 'success',
      text: `تم حفظ أمر الشراء رقم (${orderNo}) للمورد [${activeSupplier?.name}] بصافي تقديري ${grandTotal.toLocaleString()} ${currency} بنجاح!`,
    });
    setTimeout(() => setFeedback(null), 4000);
  };

  // Real One-Click Conversion to Purchase Invoice
  const handlePerformConversion = () => {
    if (items.length === 0) {
      setFeedback({ type: 'error', text: 'يرجى إدراج أصناف أولاً قبل التحويل إلى فاتورة مشتريات' });
      setTimeout(() => setFeedback(null), 3000);
      return;
    }

    // Convert items into format expected by PurchasesModule
    const payload = {
      supplierId: selectedSupplierId,
      notes: `محولة آلياً من أمر الشراء رقم: ${orderNo} (${notes})`,
      items: items.map(it => ({
        product: it.product,
        quantity: it.quantity,
        selectedUnit: it.unit,
        unitCost: it.unitCost,
        discount: it.discount,
        batchNo: it.batchNo,
        expiryDate: it.expiryDate,
        notes: it.notes,
      })),
    };

    // Update status in saved orders
    setSavedOrders(prev =>
      prev.map(o => (o.orderNo === orderNo ? { ...o, status: 'CONVERTED' } : o))
    );

    // Call callback to open PurchasesModule prefilled!
    onConvertToPurchaseInvoice(payload);
  };

  // Keyboard Shortcuts (F2: Save, F9: Convert, F10: Print)
  const handleSaveOrderRef = useRef(handleSaveOrder);
  const handlePerformConversionRef = useRef(handlePerformConversion);

  useEffect(() => {
    handleSaveOrderRef.current = handleSaveOrder;
    handlePerformConversionRef.current = handlePerformConversion;
  });

  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'F2') {
        e.preventDefault();
        handleSaveOrderRef.current();
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

  return (
    <div className="flex flex-col h-full bg-[#f0f4f8] text-slate-800 select-none overflow-hidden" dir="rtl">
      {/* Top Title Bar */}
      <div className="bg-white text-slate-800 px-3 py-1.5 flex items-center justify-between shadow-xs border-b border-slate-200 shrink-0">
        <div className="flex items-center gap-2">
          <div className="w-7 h-7 rounded-lg bg-emerald-50 border border-emerald-200 flex items-center justify-center text-emerald-700 shadow-xs">
            <ClipboardList className="w-4 h-4 text-emerald-700" />
          </div>
          <span className="font-bold text-xs text-slate-900 tracking-wide">
            أمر شراء وتوريد بضاعة [Purchase Order - Classic DataGrid]
          </span>
          <span className="bg-emerald-50 text-emerald-800 text-[10px] px-2 py-0.5 rounded-full border border-emerald-200 font-bold">
            غير محاسبي (طلب توريد استرشادي)
          </span>
        </div>

        <div className="flex items-center gap-2 text-xs">
          <button
            onClick={() => setShowHistoryModal(true)}
            className="bg-slate-50 hover:bg-slate-100 text-slate-700 px-2.5 py-1 rounded text-xs flex items-center gap-1 border border-slate-200 transition cursor-pointer font-medium"
          >
            <Eye className="w-3.5 h-3.5 text-emerald-600" />
            <span>سجل الأوامر ({savedOrders.length})</span>
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
          {/* Order No */}
          <div className="space-y-1">
            <label className="font-bold text-slate-700 block">رقم أمر الشراء:</label>
            <div className="flex gap-1">
              <input
                type="text"
                value={orderNo}
                onChange={(e) => setOrderNo(e.target.value)}
                className="w-full bg-slate-50 border border-slate-300 rounded px-2 py-1 font-mono font-bold text-emerald-900 focus:bg-white focus:border-emerald-500 outline-hidden"
              />
              <button
                type="button"
                onClick={() => setOrderNo(`PO-${new Date().getFullYear()}-${String(Math.floor(Math.random() * 9000) + 1000)}`)}
                title="توليد رقم جديد"
                className="p-1 bg-slate-100 border border-slate-300 rounded hover:bg-slate-200 text-slate-600"
              >
                <RefreshCw className="w-3.5 h-3.5" />
              </button>
            </div>
          </div>

          {/* Supplier Selection */}
          <div className="space-y-1 md:col-span-2">
            <label className="font-bold text-slate-700 flex items-center justify-between">
              <span>المورد المطلوب منه:</span>
              <span className="text-[10px] text-emerald-700 font-normal">
                الرصيد الدائن: {activeSupplier?.currentBalance?.toLocaleString() || 0} {currency}
              </span>
            </label>
            <select
              value={selectedSupplierId}
              onChange={(e) => setSelectedSupplierId(e.target.value)}
              className="w-full bg-white border border-slate-300 rounded px-2 py-1 font-bold text-slate-900 focus:border-emerald-500 outline-hidden"
            >
              {suppliers.map(s => (
                <option key={s.id} value={s.id}>
                  [{s.code || s.id}] {s.name} - هاتف: {s.phone || 'غير مسجل'}
                </option>
              ))}
            </select>
          </div>

          {/* Dates */}
          <div className="space-y-1">
            <label className="font-bold text-slate-700 block">تاريخ الطلب والتوريد المتوقع:</label>
            <div className="grid grid-cols-2 gap-1">
              <input
                type="date"
                value={orderDate}
                onChange={(e) => setOrderDate(e.target.value)}
                className="w-full bg-slate-50 border border-slate-300 rounded px-1.5 py-1 text-[11px] font-mono text-slate-800"
                title="تاريخ تحرير أمر الشراء"
              />
              <input
                type="date"
                value={expectedDate}
                onChange={(e) => setExpectedDate(e.target.value)}
                className="w-full bg-slate-50 border border-slate-300 rounded px-1.5 py-1 text-[11px] font-mono text-slate-800"
                title="تاريخ الاستلام والتوريد المتوقع"
              />
            </div>
          </div>

          {/* Receiving Warehouse */}
          <div className="space-y-1">
            <label className="font-bold text-slate-700 block">مستودع الاستلام المستهدف:</label>
            <select
              value={selectedWarehouseId}
              onChange={(e) => setSelectedWarehouseId(e.target.value)}
              className="w-full bg-white border border-slate-300 rounded px-2 py-1 font-bold text-slate-800 focus:border-emerald-500 outline-hidden"
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
          <span className="font-bold text-slate-600 shrink-0">شروط التوريد والنقل:</span>
          <input
            type="text"
            value={notes}
            onChange={(e) => setNotes(e.target.value)}
            placeholder="تعليمات المستودع، التغليف، وتواريخ الصلاحية المشروطة..."
            className="flex-1 bg-slate-50 border border-slate-300 rounded px-2 py-1 text-slate-800 focus:bg-white focus:border-emerald-500 outline-hidden text-xs"
          />
        </div>
      </div>

      {/* Barcode & Fast Search Toolbar */}
      <div className="bg-[#e8f0ea] px-3 py-1.5 border-b border-slate-300 flex flex-wrap items-center justify-between gap-2 shrink-0">
        <div className="flex items-center gap-2 flex-1 max-w-2xl">
          {/* Barcode Scanner Form */}
          <form onSubmit={handleBarcodeSubmit} className="flex items-center gap-1.5">
            <div className="relative flex items-center">
              <Barcode className="w-4 h-4 text-emerald-800 absolute right-2 pointer-events-none" />
              <input
                ref={barcodeInputRef}
                type="text"
                value={barcodeInput}
                onChange={(e) => setBarcodeInput(e.target.value)}
                placeholder="امسح باركود الصنف [Enter]..."
                className="w-52 pr-8 pl-2 py-1 bg-white border border-slate-300 rounded text-xs font-mono font-bold text-slate-900 focus:border-emerald-600 focus:ring-1 focus:ring-emerald-500 outline-hidden shadow-2xs"
              />
            </div>
            <button
              type="submit"
              className="bg-[#1b4332] hover:bg-emerald-800 text-white px-2.5 py-1 rounded text-xs font-bold transition cursor-pointer"
            >
              إدراج
            </button>
          </form>

          {/* Product Quick Dropdown Selector */}
          <div className="flex items-center gap-1 flex-1">
            <select
              value={selectedProductId}
              onChange={(e) => setSelectedProductId(e.target.value)}
              className="flex-1 bg-white border border-slate-300 rounded px-2 py-1 text-xs font-bold text-slate-800 focus:border-emerald-500 outline-hidden"
            >
              {products.map(p => (
                <option key={p.id} value={p.id}>
                  [{p.code}] {p.name} - التكلفة: {p.costPrice.toLocaleString()} {currency} (المتوفر: {p.stock} {p.unit})
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

        {/* Info badges */}
        <div className="flex items-center gap-2 text-xs">
          <span className="bg-emerald-100 text-emerald-900 font-bold px-2 py-0.5 rounded border border-emerald-300 text-[11px]">
            العملة الأساسية: {currency} (V1 Base Currency)
          </span>
          <span className="text-slate-500 font-mono text-[11px]">
            عدد البنود المطلوبة: {items.length}
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
              : 'bg-emerald-100 text-emerald-900 border-b border-emerald-300'
          }`}
        >
          <div className="flex items-center gap-1.5">
            {feedback.type === 'success' && <CheckCircle2 className="w-3.5 h-3.5 text-emerald-700" />}
            {feedback.type === 'error' && <AlertCircle className="w-3.5 h-3.5 text-rose-700" />}
            {feedback.type === 'info' && <Truck className="w-3.5 h-3.5 text-emerald-700" />}
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
            <tr className="bg-[#1b4332] text-white sticky top-0 z-10 select-none text-[11px]">
              <th className="py-2 px-2 text-center w-10 border-l border-emerald-800">#</th>
              <th className="py-2 px-2 border-l border-emerald-800 w-28">كود / باركود</th>
              <th className="py-2 px-3 border-l border-emerald-800">الصنف المطلوب</th>
              <th className="py-2 px-2 border-l border-emerald-800 w-28">الوحدة</th>
              <th className="py-2 px-2 border-l border-emerald-800 text-center w-20">المخزون الحالي</th>
              <th className="py-2 px-2 border-l border-emerald-800 text-center w-24">الكمية المطلوبة</th>
              <th className="py-2 px-2 border-l border-emerald-800 text-center w-28">التكلفة التقديرية ({currency})</th>
              <th className="py-2 px-2 border-l border-emerald-800 text-center w-20">الخصم</th>
              <th className="py-2 px-3 border-l border-emerald-800 text-left w-28">الإجمالي التقديري</th>
              <th className="py-2 px-2 border-l border-emerald-800 w-24">رقم الشحنة</th>
              <th className="py-2 px-2 border-l border-emerald-800 w-28">الصلاحية المتوقعة</th>
              <th className="py-2 px-2 border-l border-emerald-800">ملاحظات التوريد</th>
              <th className="py-2 px-1 text-center w-10">حذف</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-200">
            {items.length === 0 ? (
              <tr>
                <td colSpan={13} className="py-16 text-center text-slate-400">
                  <div className="flex flex-col items-center justify-center gap-2">
                    <ClipboardList className="w-12 h-12 text-slate-300 stroke-1" />
                    <p className="font-bold text-slate-600 text-sm">أمر الشراء فارغ حالياً</p>
                    <p className="text-xs text-slate-400">
                      امسح الباركود أو حدد الصنف من القائمة لإضافته إلى أمر الشراء
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
                  <tr key={item.id} className="hover:bg-emerald-50/40 transition group">
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
                        className="w-full bg-white border border-slate-300 hover:border-emerald-500 rounded px-1 py-1 text-xs font-bold text-slate-800 outline-hidden cursor-pointer"
                      >
                        {availableUnits.map(u => (
                          <option key={u.name} value={u.name}>
                            {u.name} {u.factor > 1 ? `(×${u.factor})` : ''}
                          </option>
                        ))}
                      </select>
                    </td>

                    {/* Current Stock */}
                    <td className="py-1.5 px-2 text-center border-l border-slate-200">
                      <span className={`px-1.5 py-0.5 rounded text-[10px] font-mono font-bold ${
                        item.product.stock <= item.product.minStock
                          ? 'bg-rose-100 text-rose-800'
                          : 'bg-slate-100 text-slate-700'
                      }`}>
                        {item.product.stock}
                      </span>
                    </td>

                    {/* Quantity Required - Inline Edit */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <input
                        type="number"
                        min={0.01}
                        step="any"
                        value={item.quantity || ''}
                        onChange={(e) => handleGridChange(idx, 'quantity', e.target.value)}
                        className="w-full bg-white border border-transparent focus:border-emerald-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers font-bold text-slate-900 focus:outline-hidden focus:bg-emerald-50 transition-colors"
                      />
                    </td>

                    {/* Estimated Cost - Inline Edit */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <input
                        type="number"
                        min={0}
                        step="any"
                        value={item.unitCost || ''}
                        onChange={(e) => handleGridChange(idx, 'unitCost', e.target.value)}
                        className="w-full bg-white border border-transparent focus:border-emerald-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers font-bold text-slate-900 focus:outline-hidden focus:bg-emerald-50 transition-colors"
                      />
                    </td>

                    {/* Direct Discount */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <input
                        type="number"
                        min={0}
                        step="any"
                        value={item.discount || ''}
                        onChange={(e) => handleGridChange(idx, 'discount', e.target.value)}
                        placeholder="0"
                        className="w-full bg-white border border-transparent focus:border-emerald-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers text-rose-700 focus:outline-hidden focus:bg-rose-50 transition-colors"
                      />
                    </td>

                    {/* Total */}
                    <td className="py-1.5 px-3 border-l border-slate-200 text-left bg-emerald-50/20">
                      <span className="font-mono-numbers font-black text-emerald-950 text-sm">
                        {item.total.toLocaleString()}
                      </span>
                    </td>

                    {/* Batch No */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <input
                        type="text"
                        value={item.batchNo || ''}
                        onChange={(e) => handleGridChange(idx, 'batchNo', e.target.value)}
                        placeholder="تشغيلة..."
                        className="w-full bg-white border border-transparent focus:border-emerald-500 hover:border-slate-300 rounded px-1 py-1 text-[11px] font-mono text-slate-700 outline-hidden"
                      />
                    </td>

                    {/* Expiry Date */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <input
                        type="date"
                        value={item.expiryDate || ''}
                        onChange={(e) => handleGridChange(idx, 'expiryDate', e.target.value)}
                        className="w-full bg-white border border-transparent focus:border-emerald-500 hover:border-slate-300 rounded px-1 py-1 text-[10px] font-mono text-slate-700 outline-hidden"
                      />
                    </td>

                    {/* Item Notes */}
                    <td className="py-1 px-2 border-l border-slate-200">
                      <input
                        type="text"
                        value={item.notes || ''}
                        onChange={(e) => handleGridChange(idx, 'notes', e.target.value)}
                        placeholder="ملاحظات..."
                        className="w-full bg-white border border-transparent focus:border-emerald-500 hover:border-slate-300 rounded px-1.5 py-1 text-slate-700 text-xs outline-hidden"
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
              <span className="text-slate-500 block text-[10px] font-bold">المجموع التقديري:</span>
              <span className="font-mono-numbers font-bold text-slate-800 text-sm">
                {subtotal.toLocaleString()} <span className="text-xs font-normal">{currency}</span>
              </span>
            </div>

            {/* Discount Control */}
            <div className="bg-white border border-slate-300 rounded px-3 py-1.5 flex items-center gap-2 shadow-2xs">
              <div>
                <div className="flex items-center gap-1 mb-0.5">
                  <span className="text-slate-500 text-[10px] font-bold">خصم أمر الشراء:</span>
                  <button
                    type="button"
                    onClick={() => setDiscountType(prev => (prev === 'AMOUNT' ? 'PERCENT' : 'AMOUNT'))}
                    className="bg-emerald-100 text-emerald-800 text-[10px] font-bold px-1 rounded hover:bg-emerald-200 transition cursor-pointer"
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
            <div className="bg-[#1b4332] text-white rounded px-4 py-1.5 shadow-xs border border-emerald-950">
              <span className="text-emerald-200 block text-[10px] font-bold">الصافي التقديري لأمر الشراء:</span>
              <span className="font-mono-numbers font-black text-lg text-emerald-300">
                {grandTotal.toLocaleString()} <span className="text-xs text-white font-normal">{currency}</span>
              </span>
            </div>

            {/* Accounting Notice */}
            <div className="hidden lg:flex items-center gap-1.5 text-[11px] text-emerald-900 bg-emerald-50 px-2.5 py-1.5 rounded border border-emerald-200">
              <FileCheck className="w-4 h-4 text-emerald-700 shrink-0" />
              <span>لا يتم توليد قيود محاسبية أو إدخال بضاعة للمستودع إلا بعد تحويله إلى فاتورة مشتريات وتوريد فعلي.</span>
            </div>
          </div>

          {/* Right Side: Operational Action Buttons */}
          <div className="flex items-center gap-2">
            {/* Print PO */}
            <button
              type="button"
              onClick={() => setShowPrintModal(true)}
              className="bg-white hover:bg-slate-100 text-slate-700 border border-slate-300 px-3 py-2 rounded text-xs font-bold flex items-center gap-1.5 transition cursor-pointer shadow-2xs"
              title="طباعة أمر شراء رسمي للمورد [F10]"
            >
              <Printer className="w-4 h-4 text-slate-600" />
              <span>طباعة الأمر [F10]</span>
            </button>

            {/* Save Order Draft */}
            <button
              type="button"
              onClick={handleSaveOrder}
              className="bg-emerald-800 hover:bg-emerald-900 text-white px-3.5 py-2 rounded text-xs font-bold flex items-center gap-1.5 transition cursor-pointer shadow-xs"
              title="حفظ أمر الشراء في السجل [F2]"
            >
              <Save className="w-4 h-4" />
              <span>حفظ الأمر [F2]</span>
            </button>

            {/* Highlighted One-Click Conversion Button */}
            <button
              type="button"
              onClick={handlePerformConversion}
              className="bg-sky-700 hover:bg-sky-800 text-white px-4 py-2 rounded text-xs font-black flex items-center gap-2 transition cursor-pointer shadow-md border border-sky-600 animate-pulse hover:animate-none"
              title="تحويل مباشر إلى فاتورة مشتريات وتوريد مخزني بضغطة زر [F9]"
            >
              <Sparkles className="w-4 h-4 text-amber-300" />
              <span>تحويل إلى فاتورة مشتريات ➔</span>
            </button>
          </div>
        </div>
      </div>

      {/* Printable A4 Purchase Order Modal */}
      {showPrintModal && (
        <div className="fixed inset-0 z-50 bg-black/60 flex items-center justify-center p-4 backdrop-blur-xs">
          <div className="bg-white rounded-lg shadow-2xl max-w-2xl w-full p-6 text-slate-800 space-y-4 max-h-[90vh] overflow-auto">
            <div className="flex items-center justify-between border-b pb-3">
              <div>
                <h3 className="font-bold text-base text-slate-900">أمر شراء بضاعة رسمي (Purchase Order Document)</h3>
                <p className="text-xs text-slate-500">جاهز للتصدير أو الإرسال المباشر إلى المورد</p>
              </div>
              <button onClick={() => setShowPrintModal(false)} className="text-slate-400 hover:text-slate-600">
                <X className="w-5 h-5" />
              </button>
            </div>

            {/* Printable A4 Template */}
            <div className="border border-slate-300 p-5 rounded space-y-4 bg-white text-xs font-sans">
              <div className="flex justify-between items-start border-b pb-3">
                <div>
                  <h2 className="font-bold text-base text-emerald-950">شركة الأمل للتجارة العامة والتوزيع</h2>
                  <p className="text-[11px] text-slate-500">إدارة المشتريات والمستودعات - المركز الرئيسي</p>
                  <p className="text-[11px] text-slate-500">صنعاء - الجمهورية اليمنية | هاتف: +967 1 234567</p>
                </div>
                <div className="text-left font-mono">
                  <div className="font-bold text-sm text-emerald-900">أمر شراء: {orderNo}</div>
                  <div className="text-slate-500 text-[11px]">تاريخ الطلب: {orderDate}</div>
                  <div className="text-emerald-700 font-bold text-[11px]">التوريد المتوقع: {expectedDate}</div>
                </div>
              </div>

              <div className="bg-slate-50 p-2.5 rounded border border-slate-200">
                <div className="font-bold text-slate-900">السيد المورد / الشركة: {activeSupplier?.name}</div>
                <div className="text-slate-600 text-[11px] mt-0.5">
                  يرجى تزويدنا بالأصناف المبينة أدناه وفقاً للمواصفات والأسعار المعتمدة:
                </div>
              </div>

              <table className="w-full text-right border-collapse text-xs">
                <thead>
                  <tr className="bg-slate-100 text-slate-800 border-b border-slate-300">
                    <th className="p-1.5 text-center w-8">#</th>
                    <th className="p-1.5">اسم الصنف والمواصفات</th>
                    <th className="p-1.5 text-center w-16">الوحدة</th>
                    <th className="p-1.5 text-center w-16">الكمية</th>
                    <th className="p-1.5 text-left w-24">التكلفة المتفق عليها</th>
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
                      <td className="p-1.5 text-left font-mono">{it.unitCost.toLocaleString()}</td>
                      <td className="p-1.5 text-left font-bold font-mono text-emerald-900">{it.total.toLocaleString()}</td>
                    </tr>
                  ))}
                </tbody>
              </table>

              <div className="flex justify-between items-start border-t pt-3">
                <div className="text-[11px] text-slate-600 space-y-1">
                  <p className="font-bold text-slate-800">ملاحظات وشروط التوريد للمستودع:</p>
                  <p>{notes}</p>
                </div>
                <div className="w-48 space-y-1 text-xs">
                  <div className="flex justify-between">
                    <span className="text-slate-500">المجموع:</span>
                    <span className="font-mono font-bold">{subtotal.toLocaleString()}</span>
                  </div>
                  {discountAmount > 0 && (
                    <div className="flex justify-between text-rose-600">
                      <span>الخصم الممنوح:</span>
                      <span className="font-mono font-bold">-{discountAmount.toLocaleString()}</span>
                    </div>
                  )}
                  <div className="flex justify-between border-t pt-1 font-bold text-emerald-950 text-sm">
                    <span>الصافي التقديري:</span>
                    <span className="font-mono text-emerald-700">{grandTotal.toLocaleString()} {currency}</span>
                  </div>
                </div>
              </div>

              <div className="pt-6 border-t flex justify-between text-center text-[11px] text-slate-600">
                <div>
                  <p className="font-bold">مسؤول المشتريات</p>
                  <p className="mt-4">{cashierName}</p>
                </div>
                <div>
                  <p className="font-bold">مدير المستودعات</p>
                  <p className="mt-4">......................................</p>
                </div>
                <div>
                  <p className="font-bold">المدير المالي والاعتماد</p>
                  <p className="mt-4">......................................</p>
                </div>
              </div>
            </div>

            <div className="flex items-center justify-end gap-2 pt-2">
              <button
                onClick={() => window.print()}
                className="bg-[#1b4332] hover:bg-emerald-800 text-white px-4 py-2 rounded text-xs font-bold flex items-center gap-1.5 cursor-pointer"
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
                <ClipboardList className="w-5 h-5 text-emerald-700" />
                <h3 className="font-bold text-sm text-slate-900">سجل أوامر الشراء المسجلة</h3>
              </div>
              <button onClick={() => setShowHistoryModal(false)} className="text-slate-400 hover:text-slate-600">
                <X className="w-5 h-5" />
              </button>
            </div>

            <div className="flex-1 overflow-auto">
              {savedOrders.length === 0 ? (
                <div className="text-center py-12 text-slate-400 text-xs">
                  لا توجد أوامر شراء محفوظة حالياً في الجلسة.
                </div>
              ) : (
                <table className="w-full text-right text-xs border-collapse">
                  <thead>
                    <tr className="bg-slate-100 border-b text-slate-700">
                      <th className="p-2">رقم الأمر</th>
                      <th className="p-2">المورد</th>
                      <th className="p-2">تاريخ الطلب</th>
                      <th className="p-2">التوريد المتوقع</th>
                      <th className="p-2">البنود</th>
                      <th className="p-2 text-left">الصافي ({currency})</th>
                      <th className="p-2 text-center">الحالة</th>
                      <th className="p-2 text-center">إجراء</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y">
                    {savedOrders.map((o) => (
                      <tr key={o.orderNo} className="hover:bg-slate-50">
                        <td className="p-2 font-mono font-bold text-emerald-900">{o.orderNo}</td>
                        <td className="p-2 font-bold">{o.supplierName}</td>
                        <td className="p-2 font-mono text-slate-500">{o.orderDate}</td>
                        <td className="p-2 font-mono text-emerald-700">{o.expectedDate}</td>
                        <td className="p-2">{o.items.length} صنف</td>
                        <td className="p-2 text-left font-mono font-bold text-emerald-800">
                          {o.grandTotal.toLocaleString()}
                        </td>
                        <td className="p-2 text-center">
                          <span
                            className={`px-2 py-0.5 rounded-full text-[10px] font-bold ${
                              o.status === 'CONVERTED'
                                ? 'bg-sky-100 text-sky-800'
                                : 'bg-emerald-100 text-emerald-800'
                            }`}
                          >
                            {o.status === 'CONVERTED' ? 'تم تحويله لمشتريات' : 'أمر شراء نشط'}
                          </span>
                        </td>
                        <td className="p-2 text-center">
                          <button
                            onClick={() => {
                              setOrderNo(o.orderNo);
                              setSelectedSupplierId(o.supplierId);
                              setOrderDate(o.orderDate);
                              setExpectedDate(o.expectedDate);
                              setNotes(o.notes);
                              setItems(o.items);
                              setDiscountType(o.discountType);
                              setDiscountVal(o.discountVal);
                              setShowHistoryModal(false);
                            }}
                            className="text-emerald-700 hover:text-emerald-900 font-bold text-[11px]"
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
