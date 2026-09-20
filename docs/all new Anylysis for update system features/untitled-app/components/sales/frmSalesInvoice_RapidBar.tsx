'use client';

import React, { useState, useRef, useMemo } from 'react';
import { 
  Plus, 
  Trash2, 
  CheckCircle2, 
  ArrowRight, 
  Sparkles, 
  Info, 
  RefreshCw, 
  CornerDownLeft,
  Search,
  Layers
} from 'lucide-react';
import { ProductItem, Customer, MainTabType } from '@/types/pos';
import { ProductSelectionModal, ProductSelectionResult } from './ProductSelectionModal';

interface Props {
  products: ProductItem[];
  customers: Customer[];
  onSelectTab?: (tab: MainTabType) => void;
  onSaveSuccess?: (invoiceNo: string) => void;
}

export const FrmSalesInvoice_RapidBar: React.FC<Props> = ({
  products,
  customers,
  onSelectTab,
  onSaveSuccess,
}) => {
  const [invoiceNo, setInvoiceNo] = useState('1005');
  const [customerName, setCustomerName] = useState(customers[0]?.name || 'شركة المحضار التجارية');
  
  const activeCustomer = useMemo(() => {
    return customers.find(c => c.name === customerName || c.id === customerName) || customers[0];
  }, [customers, customerName]);

  // Rapid Bar state
  const [selectedProductId, setSelectedProductId] = useState<string>(products[0]?.id || '');
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [showDropdown, setShowDropdown] = useState<boolean>(false);
  const [showProductSelectionModal, setShowProductSelectionModal] = useState<boolean>(false);
  const [quantityInput, setQuantityInput] = useState<string>('1');
  const [unitInput, setUnitInput] = useState<string>(products[0]?.unit || 'حبة');
  const [priceInput, setPriceInput] = useState<string>(String(products[0]?.salePrice || 10));
  const [discountInput, setDiscountInput] = useState<string>('0');
  const [itemNotesInput, setItemNotesInput] = useState<string>('');

  const qtyRef = useRef<HTMLInputElement>(null);

  // Global F3 Shortcut
  React.useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'F3') {
        e.preventDefault();
        setShowProductSelectionModal(true);
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, []);

  const filteredMatches = React.useMemo(() => {
    if (!searchQuery.trim()) return products.slice(0, 10);
    const q = searchQuery.toLowerCase().trim();
    return products
      .filter(
        (p) =>
          p.name.toLowerCase().includes(q) ||
          p.code.toLowerCase().includes(q) ||
          (p.barcode && p.barcode.toLowerCase().includes(q))
      )
      .slice(0, 10);
  }, [products, searchQuery]);

  // Invoice Lines
  const [items, setItems] = useState<Array<{
    id: string;
    product: ProductItem;
    quantity: number;
    unit: string;
    unitPrice: number;
    discount: number;
    total: number;
    notes: string;
  }>>([
    {
      id: 'S2-1',
      product: products[0] || { id: 'P1', name: 'بطاطس عمان 25غ', code: '1001', salePrice: 0.0714, unit: 'حبة' },
      quantity: 10,
      unit: 'حبة',
      unitPrice: 0.0714,
      discount: 0,
      total: 0.714,
      notes: 'مُدرج من شريط الإدخال السريع',
    }
  ]);

  const [alert, setAlert] = useState<{ type: 'success' | 'info' | 'warning'; text: string } | null>({
    type: 'info',
    text: '💡 وضع الاختبار 2 [Rapid Bar]: إدخال البيانات عبر الشريط العلوي الموحد بدون التعديل المباشر بالجدول لتفادي أخطاء الضغط.',
  });

  const activeProduct = products.find(p => p.id === selectedProductId) || products[0];

  const handleProductSelect = (prod: ProductItem) => {
    setSelectedProductId(prod.id);
    setSearchQuery(prod.name);
    setShowDropdown(false);
    setUnitInput(prod.unit || 'حبة');
    setPriceInput(String(prod.salePrice || 10));
  };

  const handleAddItemFromBar = (e?: React.FormEvent) => {
    if (e) e.preventDefault();
    if (!activeProduct) return;

    const qty = parseFloat(quantityInput) || 1;
    const price = parseFloat(priceInput) || 0;
    const disc = parseFloat(discountInput) || 0;
    const total = Math.max(0, (qty * price) - disc);

    setItems(prev => [
      ...prev,
      {
        id: `S2-${Date.now()}`,
        product: activeProduct,
        quantity: qty,
        unit: unitInput,
        unitPrice: price,
        discount: disc,
        total: Number(total.toFixed(4)),
        notes: itemNotesInput,
      }
    ]);

    setAlert({ type: 'success', text: `تم إدراج [${activeProduct.name}] بالكمية (${qty} ${unitInput}) في الجدول السفلي.` });
    setQuantityInput('1');
    setDiscountInput('0');
    setItemNotesInput('');
    if (qtyRef.current) qtyRef.current.focus();
  };

  const handleModalConfirmItem = (result: ProductSelectionResult) => {
    setItems(prev => [
      ...prev,
      {
        id: `S2-${Date.now()}-${Math.random().toString(36).substring(2, 6)}`,
        product: result.product,
        quantity: result.quantity,
        unit: result.selectedUnit,
        unitPrice: result.unitPrice,
        discount: result.discount,
        total: result.total,
        notes: result.notes || '',
      }
    ]);
    setSelectedProductId(result.product.id);
    setSearchQuery(result.product.name);
    setUnitInput(result.selectedUnit);
    setPriceInput(String(result.unitPrice));
    setQuantityInput(String(result.quantity));
    setDiscountInput(String(result.discount));
    setItemNotesInput(result.notes || '');
    setAlert({
      type: 'success',
      text: `تم بنجاح إدراج الصنف [${result.product.name}] من الشاشة المنفصلة بكمية (${result.quantity} ${result.selectedUnit}) وسعر ${result.unitPrice} وإجمالي ${result.total.toFixed(2)}.`
    });
  };

  const handleDeleteRow = (index: number) => {
    setItems(prev => prev.filter((_, i) => i !== index));
  };

  const subtotal = items.reduce((acc, it) => acc + (it.quantity * it.unitPrice), 0);
  const totalDiscount = items.reduce((acc, it) => acc + it.discount, 0);
  const netTotal = items.reduce((acc, it) => acc + it.total, 0);

  return (
    <div className="flex flex-col h-full bg-slate-100 text-slate-900 font-sans select-none overflow-y-auto">
      {/* Header */}
      <div className="bg-white border-b border-slate-200 p-3 flex flex-wrap items-center justify-between gap-3 shadow-xs text-slate-800">
        <div className="flex items-center gap-3">
          {onSelectTab && (
            <button
              onClick={() => onSelectTab('SALES_STRATEGIES_HUB')}
              className="px-2.5 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 text-xs font-bold rounded flex items-center gap-1.5 border border-slate-300 transition cursor-pointer"
            >
              <ArrowRight className="w-4 h-4 text-amber-600" />
              <span>المختبر الرئيسي</span>
            </button>
          )}

          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-lg bg-sky-600 flex items-center justify-center text-white font-bold text-sm shadow-xs">
              2
            </div>
            <div>
              <h1 className="text-sm font-bold text-slate-900 flex items-center gap-2">
                <span>استراتيجية 2: شريط الإدخال السريع العلوي [Top Rapid Entry Bar]</span>
                <span className="text-[10px] bg-sky-50 text-sky-700 border border-sky-200 px-2 py-0.5 rounded-full font-mono font-bold">
                  frmSalesInvoice_RapidBar
                </span>
              </h1>
              <p className="text-[11px] text-slate-500">
                اختبار سرعة الإدخال المتسلسل عبر شريط إدخال علوي ثابت يركز على تتابع الإدخال وحفظ الجدول من الأخطاء العشوائية.
              </p>
            </div>
          </div>
        </div>

        <button
          onClick={() => setItems([])}
          className="px-3 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 text-xs font-bold rounded-lg border border-slate-300 flex items-center gap-1.5 transition cursor-pointer"
        >
          <RefreshCw className="w-3.5 h-3.5" />
          <span>تفريغ الجدول</span>
        </button>
      </div>

      <div className="p-4 flex-1 flex flex-col gap-4">
        {/* Alert */}
        {alert && (
          <div className={`p-2.5 rounded-lg border text-xs flex items-center justify-between ${
            alert.type === 'success' ? 'bg-emerald-50 border-emerald-300 text-emerald-900 font-medium' : 'bg-sky-50 border-sky-300 text-sky-900 font-medium'
          }`}>
            <span className="flex items-center gap-2">
              <Info className="w-4 h-4 shrink-0" />
              <span>{alert.text}</span>
            </span>
            <button onClick={() => setAlert(null)} className="text-slate-400 hover:text-slate-700 font-bold">✕</button>
          </div>
        )}

        {/* Customer & Invoice Info Header */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-3 bg-white p-3 rounded-xl border border-slate-200 text-xs shadow-xs">
          <div>
            <label className="block text-slate-700 mb-1 font-bold">رقم الفاتورة:</label>
            <input
              type="text"
              value={invoiceNo}
              onChange={e => setInvoiceNo(e.target.value)}
              className="w-full bg-slate-50 border border-slate-300 rounded-lg px-2.5 py-1.5 text-emerald-700 font-mono font-extrabold"
            />
          </div>
          <div className="md:col-span-2">
            <div className="flex items-center justify-between mb-1">
              <label className="block text-slate-700 font-bold">العميل والمستفيد:</label>
              {activeCustomer && (
                <span className="text-[10px] text-slate-500 font-mono">#{activeCustomer.code}</span>
              )}
            </div>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-2">
              <select
                value={customerName}
                onChange={e => setCustomerName(e.target.value)}
                className="w-full bg-slate-50 border border-slate-300 rounded-lg px-2.5 py-1.5 text-slate-900 font-bold focus:bg-white focus:border-sky-500"
              >
                {customers.map(c => (
                  <option key={c.id} value={c.name}>{c.name}</option>
                ))}
              </select>
              {activeCustomer && (
                <div>
                  {activeCustomer.currentBalance > 0 ? (
                    <div className="bg-amber-50 text-amber-900 border border-amber-300 rounded-lg px-2 py-1 font-bold flex items-center justify-between">
                      <span className="text-[10px]">عليه للمحل (مطلوب سداد):</span>
                      <span className="font-mono text-amber-800 text-xs">{activeCustomer.currentBalance.toLocaleString()} {activeCustomer.currency || 'ريال'}</span>
                    </div>
                  ) : activeCustomer.currentBalance < 0 ? (
                    <div className="bg-emerald-50 text-emerald-900 border border-emerald-300 rounded-lg px-2 py-1 font-bold flex items-center justify-between">
                      <span className="text-[10px]">له دفعة مقدمة (فائض له):</span>
                      <span className="font-mono text-emerald-800 text-xs">{Math.abs(activeCustomer.currentBalance).toLocaleString()} {activeCustomer.currency || 'ريال'}</span>
                    </div>
                  ) : (
                    <div className="bg-slate-100 text-slate-700 border border-slate-200 rounded-lg px-2 py-1 font-bold text-center text-xs">
                      <span>حساب خالص (لا توجد مديونية سابقة)</span>
                    </div>
                  )}
                </div>
              )}
            </div>
          </div>
        </div>

        {/* Top Rapid Entry Bar (The Core Strategy UI) */}
        <form onSubmit={handleAddItemFromBar} className="bg-white border-2 border-sky-500 rounded-xl p-3.5 shadow-xs flex flex-col gap-2.5">
          <div className="flex items-center justify-between border-b border-slate-100 pb-2 text-xs">
            <span className="font-bold text-sky-900 flex items-center gap-1.5">
              <Sparkles className="w-4 h-4 text-sky-600" />
              <span>شريط إدراج الأصناف الموحد (Rapid Entry Bar)</span>
            </span>
            <div className="flex items-center gap-2">
              <button
                type="button"
                onClick={() => setShowProductSelectionModal(true)}
                className="px-3 py-1 bg-sky-700 hover:bg-sky-800 text-white rounded-lg font-bold text-xs flex items-center gap-1.5 transition cursor-pointer shadow-xs"
                title="فتح شاشة خاصة ومنفصلة لعرض واختيار الأصناف والبيانات المقترحة [F3]"
              >
                <Layers className="w-3.5 h-3.5 text-amber-300" />
                <span>شاشة استعراض واختيار الأصناف [F3]</span>
              </button>
              <span className="text-[11px] text-slate-500 hidden sm:inline">
                اضغط <kbd className="bg-slate-100 text-sky-800 px-1.5 py-0.5 rounded border border-slate-300 font-mono font-bold">Enter</kbd> لإدراج الصنف فوراً
              </span>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-12 gap-2.5 text-xs items-end">
            <div className="md:col-span-4 relative">
              <label className="block text-slate-700 mb-1 font-bold flex items-center justify-between">
                <span>1. ابحث برقم الباركود أو اسم الصنف:</span>
                {activeProduct && (
                  <span className="text-[10px] text-emerald-700 font-bold bg-emerald-50 px-1.5 py-0.2 rounded border border-emerald-200">
                    محدد: {activeProduct.name}
                  </span>
                )}
              </label>
              <div className="flex items-center gap-1.5">
                <div className="relative flex-1">
                  <Search className="w-4 h-4 text-slate-400 absolute right-2.5 top-2.5" />
                  <input
                    type="text"
                    placeholder="اكتب اسم الصنف، الكود، أو امسح الباركود..."
                    value={searchQuery}
                    onChange={(e) => {
                      setSearchQuery(e.target.value);
                      setShowDropdown(true);
                    }}
                    onFocus={() => setShowDropdown(true)}
                    className="w-full bg-slate-50 border border-slate-300 text-slate-900 rounded-lg pr-9 pl-2.5 py-2 font-bold focus:bg-white focus:border-sky-500 text-xs"
                  />
                  {showDropdown && filteredMatches.length > 0 && (
                    <div className="absolute top-full right-0 left-0 mt-1 bg-white border border-slate-300 rounded-lg shadow-xl z-50 max-h-56 overflow-y-auto divide-y divide-slate-100">
                      {filteredMatches.map((p) => (
                        <button
                          key={p.id}
                          type="button"
                          onClick={() => handleProductSelect(p)}
                          className="w-full text-right px-3 py-2 text-xs hover:bg-sky-50 transition flex items-center justify-between group cursor-pointer"
                        >
                          <div>
                            <div className="font-bold text-slate-900 group-hover:text-sky-700">{p.name}</div>
                            <div className="text-[10px] text-slate-500 font-mono">
                              كود: {p.code} {p.barcode ? `| باركود: ${p.barcode}` : ''}
                            </div>
                          </div>
                          <div className="text-left font-mono">
                            <div className="font-bold text-emerald-700">{p.salePrice}</div>
                            <div className="text-[10px] text-slate-400">رصيد: {p.stock} {p.unit}</div>
                          </div>
                        </button>
                      ))}
                    </div>
                  )}
                </div>
                <button
                  type="button"
                  onClick={() => setShowProductSelectionModal(true)}
                  className="px-2.5 py-2 bg-amber-500 hover:bg-amber-600 text-slate-950 font-black rounded-lg text-xs flex items-center gap-1 transition cursor-pointer shrink-0 shadow-2xs"
                  title="فتح الشاشة المنفصلة لاختيار وتعديل الأصناف [F3]"
                >
                  <Layers className="w-3.5 h-3.5" />
                  <span>F3</span>
                </button>
              </div>
            </div>

            <div className="md:col-span-2">
              <label className="block text-slate-700 mb-1 font-bold">2. الكمية المباعة:</label>
              <input
                ref={qtyRef}
                type="number"
                step="any"
                value={quantityInput}
                onChange={e => setQuantityInput(e.target.value)}
                className="w-full bg-slate-50 border border-emerald-400 text-emerald-800 font-extrabold rounded-lg px-2.5 py-2 text-center focus:bg-white"
              />
            </div>

            <div className="md:col-span-2">
              <label className="block text-slate-700 mb-1 font-bold">3. الوحدة:</label>
              <select
                value={unitInput}
                onChange={e => setUnitInput(e.target.value)}
                className="w-full bg-slate-50 border border-slate-300 text-slate-800 rounded-lg px-2 py-2 text-center font-medium focus:bg-white"
              >
                <option value={activeProduct?.unit || 'حبة'}>{activeProduct?.unit || 'حبة'}</option>
                <option value="باكت">باكت</option>
                <option value="كرتون">كرتون</option>
              </select>
            </div>

            <div className="md:col-span-2">
              <label className="block text-slate-700 mb-1 font-bold">4. سعر الوحدة:</label>
              <input
                type="number"
                step="any"
                value={priceInput}
                onChange={e => setPriceInput(e.target.value)}
                className="w-full bg-slate-50 border border-slate-300 text-sky-800 font-bold font-mono rounded-lg px-2.5 py-2 text-center focus:bg-white"
              />
            </div>

            <div className="md:col-span-2">
              <button
                type="submit"
                className="w-full bg-sky-600 hover:bg-sky-700 text-white font-extrabold py-2 px-3 rounded-lg shadow-xs flex items-center justify-center gap-1.5 transition text-xs cursor-pointer"
              >
                <CornerDownLeft className="w-4 h-4" />
                <span>إدراج الفاتورة</span>
              </button>
            </div>
          </div>
        </form>

        {/* Locked Invoice Grid */}
        <div className="flex-1 bg-white border border-slate-200 rounded-xl overflow-hidden flex flex-col min-h-[260px] shadow-xs">
          <div className="bg-slate-50 px-3 py-2 border-b border-slate-200 text-xs font-bold text-slate-800 flex items-center justify-between">
            <span>أسطر الفاتورة (قراءة فقط — مجهزة من الشريط العلوي)</span>
            <span className="text-slate-500 font-medium">إجمالي الأصناف: {items.length}</span>
          </div>
          <div className="overflow-x-auto flex-1">
            <table className="w-full text-right text-xs border-collapse">
              <thead>
                <tr className="bg-slate-100/80 text-slate-700 border-b border-slate-200 font-bold">
                  <th className="p-2.5 text-center w-12">#</th>
                  <th className="p-2.5">اسم الصنف</th>
                  <th className="p-2.5 text-center">الوحدة</th>
                  <th className="p-2.5 text-center">الكمية</th>
                  <th className="p-2.5 text-center">السعر</th>
                  <th className="p-2.5 text-center">الإجمالي</th>
                  <th className="p-2.5 text-center">إجراء</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200">
                {items.length === 0 ? (
                  <tr>
                    <td colSpan={7} className="p-8 text-center text-slate-500">
                      لا توجد أصناف في الفاتورة. استخدم الشريط العلوي لإدراج الأصناف.
                    </td>
                  </tr>
                ) : (
                  items.map((it, idx) => (
                    <tr key={it.id} className="hover:bg-slate-50">
                      <td className="p-2 text-center text-slate-500 font-mono font-medium">{idx + 1}</td>
                      <td className="p-2 font-bold text-slate-900">{it.product.name}</td>
                      <td className="p-2 text-center text-slate-700 font-medium">{it.unit}</td>
                      <td className="p-2 text-center font-bold text-emerald-700">{it.quantity}</td>
                      <td className="p-2 text-center font-mono text-sky-800 font-bold">{it.unitPrice}</td>
                      <td className="p-2 text-center font-mono font-extrabold text-emerald-800">{it.total.toFixed(4)}</td>
                      <td className="p-2 text-center">
                        <button onClick={() => handleDeleteRow(idx)} className="text-rose-600 hover:text-rose-800 p-1 rounded hover:bg-rose-50 cursor-pointer">
                          <Trash2 className="w-4 h-4" />
                        </button>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>

        {/* Footer */}
        <div className="bg-white border border-slate-200 p-3.5 rounded-xl flex items-center justify-between text-xs shadow-xs">
          <div className="flex items-center gap-4">
            <span className="font-bold text-slate-700">الصافي: <strong className="text-emerald-800 text-sm font-mono font-extrabold">{netTotal.toFixed(4)}</strong></span>
          </div>
          <button
            onClick={() => {
              if (onSaveSuccess) onSaveSuccess(invoiceNo);
              setAlert({ type: 'success', text: `تم اعتماد الفاتورة رقم [${invoiceNo}] بنجاح في وضع الشريط السريع.` });
            }}
            className="px-4 py-2 bg-sky-600 hover:bg-sky-700 text-white font-bold rounded-lg shadow-xs flex items-center gap-2 cursor-pointer"
          >
            <CheckCircle2 className="w-4 h-4" />
            <span>حفظ واعتماد الاختبار</span>
          </button>
        </div>
      </div>

      {/* Dedicated Separate Product Selection & Defaults Screen Modal */}
      <ProductSelectionModal
        isOpen={showProductSelectionModal}
        products={products}
        initialSelectedProductId={selectedProductId}
        onConfirmItem={(result, stayOpen) => {
          handleModalConfirmItem(result);
          if (!stayOpen) {
            setShowProductSelectionModal(false);
          }
        }}
        onClose={() => setShowProductSelectionModal(false)}
      />
    </div>
  );
};
