'use client';

import React, { useState, useEffect } from 'react';
import { 
  Search, 
  Trash2, 
  CheckCircle2, 
  ArrowRight, 
  Sparkles, 
  Info, 
  RefreshCw, 
  Layers, 
  PackageCheck,
  Maximize2,
  X
} from 'lucide-react';
import { ProductItem, Customer, MainTabType } from '@/types/pos';

interface Props {
  products: ProductItem[];
  customers: Customer[];
  onSelectTab?: (tab: MainTabType) => void;
  onSaveSuccess?: (invoiceNo: string) => void;
}

export const FrmSalesInvoice_SmartF3Lookup: React.FC<Props> = ({
  products,
  customers,
  onSelectTab,
  onSaveSuccess,
}) => {
  const [invoiceNo, setInvoiceNo] = useState('1007');
  const [showF3Modal, setShowF3Modal] = useState(false);
  const [f3SearchText, setF3SearchText] = useState('');
  const [selectedProductForModal, setSelectedProductForModal] = useState<ProductItem | null>(products[0] || null);

  const [items, setItems] = useState<Array<{
    id: string;
    product: ProductItem;
    quantity: number;
    unit: string;
    unitPrice: number;
    discount: number;
    total: number;
  }>>([
    {
      id: 'S4-1',
      product: products[0] || { id: 'P1', name: 'بطاطس عمان 25غ', salePrice: 0.0714, unit: 'حبة' },
      quantity: 20,
      unit: 'حبة',
      unitPrice: 0.0714,
      discount: 0,
      total: 1.428,
    }
  ]);

  const [alert, setAlert] = useState<{ type: 'success' | 'info' | 'warning'; text: string } | null>({
    type: 'info',
    text: '🔍 وضع الاختبار 4 [Smart F3 Lookup]: البحث المتقدم عبر نافذة مستقلة تتيح الاطلاع على أرصدة الوحدات وأسعار الجملة قبل الإدراج.',
  });

  // Listen to keyboard F3 press
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'F3') {
        e.preventDefault();
        setShowF3Modal(true);
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, []);

  const filteredProducts = products.filter(p => 
    p.name.toLowerCase().includes(f3SearchText.toLowerCase()) ||
    p.code?.toLowerCase().includes(f3SearchText.toLowerCase()) ||
    p.barcode?.toLowerCase().includes(f3SearchText.toLowerCase())
  );

  const handleSelectProductFromModal = (prod: ProductItem, selectedUnitName?: string, priceVal?: number) => {
    const unitName = selectedUnitName || prod.unit || 'حبة';
    const price = priceVal !== undefined ? priceVal : prod.salePrice || 10;

    setItems(prev => [
      ...prev,
      {
        id: `S4-${Date.now()}`,
        product: prod,
        quantity: 1,
        unit: unitName,
        unitPrice: price,
        discount: 0,
        total: price,
      }
    ]);

    setAlert({ type: 'success', text: `تم إدراج الصنف [${prod.name}] عبر نافذة البحث F3 بالوحدة (${unitName}) بسعر ${price}` });
    setShowF3Modal(false);
  };

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
            <div className="w-8 h-8 rounded-lg bg-purple-600 flex items-center justify-center text-white font-bold text-sm shadow-xs">
              4
            </div>
            <div>
              <h1 className="text-sm font-bold text-slate-900 flex items-center gap-2">
                <span>استراتيجية 4: البحث الذكي بالزر F3 [Smart F3 Lookup Window]</span>
                <span className="text-[10px] bg-purple-50 text-purple-700 border border-purple-200 px-2 py-0.5 rounded-full font-mono font-bold">
                  frmSalesInvoice_SmartF3Lookup
                </span>
              </h1>
              <p className="text-[11px] text-slate-500">
                اختبار البحث الشامل باختصار F3 واستعراض بطاقات الوحدات المتعددة والأرصدة الحالية لكل صنف.
              </p>
            </div>
          </div>
        </div>

        <button
          onClick={() => setShowF3Modal(true)}
          className="px-3.5 py-1.5 bg-purple-600 hover:bg-purple-700 text-white text-xs font-bold rounded-lg shadow-xs flex items-center gap-2 transition cursor-pointer"
        >
          <Search className="w-4 h-4" />
          <span>فتح نافذة البحث الذكي (F3)</span>
        </button>
      </div>

      <div className="p-4 flex-1 flex flex-col gap-4">
        {/* Alert */}
        {alert && (
          <div className={`p-2.5 rounded-lg border text-xs flex items-center justify-between ${
            alert.type === 'success' ? 'bg-emerald-50 border-emerald-300 text-emerald-900 font-medium' : 'bg-purple-50 border-purple-300 text-purple-900 font-medium'
          }`}>
            <span className="flex items-center gap-2">
              <Info className="w-4 h-4 shrink-0" />
              <span>{alert.text}</span>
            </span>
            <button onClick={() => setAlert(null)} className="text-slate-400 hover:text-slate-700 font-bold">✕</button>
          </div>
        )}

        {/* F3 Launch Banner */}
        <div className="bg-purple-50 border border-purple-200 rounded-xl p-3 flex items-center justify-between text-xs shadow-xs">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 rounded-lg bg-purple-600 text-white flex items-center justify-center font-extrabold font-mono text-sm shadow-xs">
              F3
            </div>
            <div>
              <span className="font-bold text-purple-950 block">اضغط زر F3 من الكيبورد أو انقر فوق الزر لاستئناف البحث الذكي</span>
              <span className="text-[11px] text-slate-600 font-medium">يتيح اختيار الوحدات المتعددة واستعراض مخزون الصنف في المستودعات</span>
            </div>
          </div>
          <button
            onClick={() => setShowF3Modal(true)}
            className="px-4 py-2 bg-purple-700 hover:bg-purple-800 text-white font-bold rounded-lg text-xs shadow-xs transition cursor-pointer"
          >
            تجربة البحث المنبثق الآن
          </button>
        </div>

        {/* Invoice Grid */}
        <div className="flex-1 bg-white border border-slate-200 rounded-xl overflow-hidden flex flex-col min-h-[260px] shadow-xs">
          <div className="bg-slate-50 px-3 py-2 border-b border-slate-200 text-xs font-bold text-slate-800 flex items-center justify-between">
            <span>أسطر الفاتورة ({items.length} أصناف)</span>
            <span className="text-purple-800 font-bold text-[11px]">مُدرجة عبر البحث المتقدم F3</span>
          </div>
          <div className="overflow-x-auto flex-1">
            <table className="w-full text-right text-xs border-collapse">
              <thead>
                <tr className="bg-slate-100/80 text-slate-700 border-b border-slate-200 font-bold">
                  <th className="p-2.5 text-center w-12">#</th>
                  <th className="p-2.5">اسم الصنف</th>
                  <th className="p-2.5 text-center">الوحدة المحددة</th>
                  <th className="p-2.5 text-center">الكمية</th>
                  <th className="p-2.5 text-center">السعر</th>
                  <th className="p-2.5 text-center">الإجمالي</th>
                  <th className="p-2.5 text-center w-16">إجراء</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200">
                {items.length === 0 ? (
                  <tr>
                    <td colSpan={7} className="p-8 text-center text-slate-500">
                      لا توجد أصناف في الفاتورة بعد. اضغط F3 للبحث والإدراج.
                    </td>
                  </tr>
                ) : (
                  items.map((it, idx) => (
                    <tr key={it.id} className="hover:bg-slate-50">
                      <td className="p-2 text-center text-slate-500 font-mono font-medium">{idx + 1}</td>
                      <td className="p-2 font-bold text-slate-900">{it.product.name}</td>
                      <td className="p-2 text-center text-purple-800 font-bold">{it.unit}</td>
                      <td className="p-2 text-center font-bold text-emerald-800">{it.quantity}</td>
                      <td className="p-2 text-center font-mono text-sky-800 font-bold">{it.unitPrice}</td>
                      <td className="p-2 text-center font-mono font-extrabold text-emerald-800">{it.total.toFixed(4)}</td>
                      <td className="p-2 text-center">
                        <button onClick={() => setItems(prev => prev.filter((_, i) => i !== idx))} className="text-rose-600 hover:text-rose-800 p-1 cursor-pointer">
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
          <div>
            <span className="font-bold text-slate-700">الصافي النهائي: <strong className="text-purple-900 text-base font-mono font-extrabold mr-2">{netTotal.toFixed(4)}</strong></span>
          </div>
          <button
            onClick={() => {
              if (onSaveSuccess) onSaveSuccess(invoiceNo);
              setAlert({ type: 'success', text: `تم اعتماد الفاتورة رقم [${invoiceNo}] بنجاح في وضع البحث F3!` });
            }}
            className="px-4 py-2 bg-purple-600 hover:bg-purple-700 text-white font-bold rounded-lg shadow-xs flex items-center gap-2 cursor-pointer"
          >
            <CheckCircle2 className="w-4 h-4" />
            <span>حفظ واعتماد البحث F3</span>
          </button>
        </div>
      </div>

      {/* Standalone F3 Smart Search Modal */}
      {showF3Modal && (
        <div className="fixed inset-0 bg-slate-900/60 backdrop-blur-xs z-50 flex items-center justify-center p-4">
          <div className="bg-white border border-purple-300 rounded-xl max-w-4xl w-full shadow-2xl overflow-hidden flex flex-col max-h-[85vh]">
            <div className="bg-purple-50 p-3 px-4 border-b border-purple-200 flex items-center justify-between text-purple-900">
              <span className="font-extrabold text-sm flex items-center gap-2">
                <Search className="w-4 h-4 text-purple-700" />
                <span>نافذة البحث الذكي المتقدم عن الأصناف [F3 Smart Search Modal]</span>
              </span>
              <button onClick={() => setShowF3Modal(false)} className="text-purple-600 hover:text-purple-900 cursor-pointer p-1 rounded-lg hover:bg-purple-100 transition">
                <X className="w-5 h-5" />
              </button>
            </div>

            <div className="p-4 flex flex-col gap-3 flex-1 overflow-y-auto">
              <div className="relative">
                <Search className="w-4 h-4 text-slate-400 absolute right-3 top-3" />
                <input
                  type="text"
                  autoFocus
                  value={f3SearchText}
                  onChange={e => setF3SearchText(e.target.value)}
                  placeholder="ابحث بالاسم، الكود، الباركود أو التصنيف..."
                  className="w-full bg-slate-50 border-2 border-purple-500 text-slate-900 rounded-lg pr-10 pl-4 py-2 text-xs font-bold focus:bg-white focus:outline-none"
                />
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-3 flex-1">
                {/* Product List Left */}
                <div className="bg-slate-50 border border-slate-200 rounded-lg p-2 max-h-[350px] overflow-y-auto divide-y divide-slate-200">
                  {filteredProducts.map(p => (
                    <div
                      key={p.id}
                      onClick={() => setSelectedProductForModal(p)}
                      className={`p-2.5 cursor-pointer rounded-lg transition flex items-center justify-between text-xs ${
                        selectedProductForModal?.id === p.id ? 'bg-purple-100 border border-purple-400 text-purple-950 font-bold' : 'hover:bg-slate-100 text-slate-800'
                      }`}
                    >
                      <div>
                        <div className="font-bold">{p.name}</div>
                        <div className="text-[10px] text-slate-500 font-mono">كود: {p.code} | {p.category}</div>
                      </div>
                      <div className="text-right">
                        <div className="text-purple-800 font-mono font-bold">{p.salePrice} ريال</div>
                        <div className="text-[10px] text-emerald-700 font-medium">رصيد: {p.stock || 100}</div>
                      </div>
                    </div>
                  ))}
                </div>

                {/* Selected Product Units Card Right */}
                {selectedProductForModal ? (
                  <div className="bg-slate-50 border border-purple-200 rounded-lg p-3 flex flex-col justify-between text-xs">
                    <div>
                      <h3 className="font-bold text-sm text-purple-950 mb-1">{selectedProductForModal.name}</h3>
                      <p className="text-[11px] text-slate-600 mb-3 font-medium">تفاصيل الوحدات المتاحة والأرصدة لهذا الصنف:</p>

                      <div className="space-y-2">
                        {/* Units list */}
                        {(selectedProductForModal.units || [
                          { name: selectedProductForModal.unit || 'حبة', factor: 1, price: selectedProductForModal.salePrice, cost: selectedProductForModal.costPrice },
                          { name: 'باكت', factor: 12, price: (selectedProductForModal.salePrice || 10) * 11, cost: 0 },
                          { name: 'كرتون', factor: 48, price: (selectedProductForModal.salePrice || 10) * 44, cost: 0 },
                        ]).map((u, uIdx) => (
                          <div key={uIdx} className="bg-white border border-slate-200 p-2 rounded-lg flex items-center justify-between shadow-xs">
                            <div>
                              <span className="font-bold text-slate-900">{u.name}</span>
                              <span className="text-[10px] text-slate-500 mr-2 font-mono">(معامل: {u.factor})</span>
                            </div>
                            <div className="flex items-center gap-3">
                              <span className="font-mono text-emerald-800 font-extrabold">{u.price} ريال</span>
                              <button
                                onClick={() => handleSelectProductFromModal(selectedProductForModal, u.name, u.price)}
                                className="px-2.5 py-1 bg-purple-600 hover:bg-purple-700 text-white rounded-lg font-bold text-[11px] cursor-pointer"
                              >
                                إدراج هاهنا
                              </button>
                            </div>
                          </div>
                        ))}
                      </div>
                    </div>

                    <div className="pt-3 border-t border-slate-200 text-[11px] text-slate-600 flex items-center justify-between font-medium">
                      <span>إجمالي رصيد المستودع: <strong className="text-slate-900">{selectedProductForModal.stock || 100}</strong></span>
                      <button
                        onClick={() => handleSelectProductFromModal(selectedProductForModal)}
                        className="px-3 py-1.5 bg-emerald-600 hover:bg-emerald-700 text-white font-bold rounded-lg cursor-pointer"
                      >
                        إدراج بالوحدة الأساسية
                      </button>
                    </div>
                  </div>
                ) : (
                  <div className="bg-slate-50 border border-slate-200 rounded-lg p-6 text-center text-slate-500 text-xs flex items-center justify-center font-medium">
                    اختر صنفاً لاستعراض كرت الوحدات والأسعار
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
