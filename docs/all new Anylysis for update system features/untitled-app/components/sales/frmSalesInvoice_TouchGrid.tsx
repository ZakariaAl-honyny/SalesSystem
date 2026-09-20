'use client';

import React, { useState } from 'react';
import { 
  Grid, 
  Plus, 
  Minus, 
  Trash2, 
  CheckCircle2, 
  ArrowRight, 
  Sparkles, 
  Info, 
  RefreshCw, 
  Layers, 
  Touchpad,
  ShoppingBag
} from 'lucide-react';
import { ProductItem, Customer, MainTabType } from '@/types/pos';

interface Props {
  products: ProductItem[];
  customers: Customer[];
  onSelectTab?: (tab: MainTabType) => void;
  onSaveSuccess?: (invoiceNo: string) => void;
}

export const FrmSalesInvoice_TouchGrid: React.FC<Props> = ({
  products,
  customers,
  onSelectTab,
  onSaveSuccess,
}) => {
  const [invoiceNo, setInvoiceNo] = useState('1008');
  const [selectedCategory, setSelectedCategory] = useState<string>('الكل');

  const categories = ['الكل', ...Array.from(new Set(products.map(p => p.category || 'عام')))];

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
      id: 'S5-1',
      product: products[0] || { id: 'P1', name: 'بطاطس عمان 25غ', salePrice: 0.0714, unit: 'حبة' },
      quantity: 5,
      unit: 'حبة',
      unitPrice: 0.0714,
      discount: 0,
      total: 0.357,
    }
  ]);

  const [alert, setAlert] = useState<{ type: 'success' | 'info' | 'warning'; text: string } | null>({
    type: 'info',
    text: '🔲 وضع الاختبار 5 [Touch Visual Grid]: اختيار الأصناف بالنقر المباشر على بطاقات الأصناف والتصنيفات الشاشات باللمس.',
  });

  const filteredProducts = selectedCategory === 'الكل'
    ? products
    : products.filter(p => (p.category || 'عام') === selectedCategory);

  const handleTouchAddProduct = (prod: ProductItem) => {
    setItems(prev => {
      const idx = prev.findIndex(it => it.product.id === prod.id);
      if (idx > -1) {
        const updated = [...prev];
        const current = updated[idx];
        const newQty = current.quantity + 1;
        updated[idx] = {
          ...current,
          quantity: newQty,
          total: Number((newQty * current.unitPrice - current.discount).toFixed(4)),
        };
        return updated;
      }
      return [
        ...prev,
        {
          id: `S5-${Date.now()}`,
          product: prod,
          quantity: 1,
          unit: prod.unit || 'حبة',
          unitPrice: prod.salePrice || 10,
          discount: 0,
          total: prod.salePrice || 10,
        }
      ];
    });

    setAlert({ type: 'success', text: `تم نقر وتحديد الصنف [${prod.name}] بزيادة (+1) بنجاح.` });
  };

  const handleUpdateQty = (index: number, delta: number) => {
    setItems(prev => {
      const updated = [...prev];
      const current = { ...updated[index] };
      const newQty = Math.max(0.001, current.quantity + delta);
      current.quantity = newQty;
      current.total = Math.max(0, (newQty * current.unitPrice) - current.discount);
      updated[index] = current;
      return updated;
    });
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
            <div className="w-8 h-8 rounded-lg bg-amber-600 flex items-center justify-center text-white font-bold text-sm shadow-xs">
              5
            </div>
            <div>
              <h1 className="text-sm font-bold text-slate-900 flex items-center gap-2">
                <span>استراتيجية 5: الشبكة المرئية للأصناف [Touch Visual Category Grid]</span>
                <span className="text-[10px] bg-amber-50 text-amber-800 border border-amber-200 px-2 py-0.5 rounded-full font-mono font-bold">
                  frmSalesInvoice_TouchGrid
                </span>
              </h1>
              <p className="text-[11px] text-slate-500">
                اختبار سرعة اختيار الأصناف عبر أزرار التصنيفات الملونة وبطاقات الشاشات اللمسية (Touch Monitor).
              </p>
            </div>
          </div>
        </div>

        <button
          onClick={() => setItems([])}
          className="px-3 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 text-xs font-bold rounded-lg border border-slate-300 flex items-center gap-1.5 transition cursor-pointer"
        >
          <RefreshCw className="w-3.5 h-3.5" />
          <span>تفريغ الفاتورة</span>
        </button>
      </div>

      <div className="p-4 flex-1 flex flex-col gap-4">
        {/* Alert */}
        {alert && (
          <div className={`p-2.5 rounded-lg border text-xs flex items-center justify-between ${
            alert.type === 'success' ? 'bg-emerald-50 border-emerald-300 text-emerald-900 font-medium' : 'bg-amber-50 border-amber-300 text-amber-900 font-medium'
          }`}>
            <span className="flex items-center gap-2">
              <Info className="w-4 h-4 shrink-0" />
              <span>{alert.text}</span>
            </span>
            <button onClick={() => setAlert(null)} className="text-slate-400 hover:text-slate-700 font-bold">✕</button>
          </div>
        )}

        {/* Split Touch Interface */}
        <div className="grid grid-cols-1 lg:grid-cols-12 gap-4 flex-1">
          {/* Left Side: Invoice Items List */}
          <div className="lg:col-span-5 bg-white border border-slate-200 rounded-xl overflow-hidden flex flex-col min-h-[300px] shadow-xs">
            <div className="bg-slate-50 p-2.5 border-b border-slate-200 text-xs font-bold text-slate-800 flex items-center justify-between">
              <span className="flex items-center gap-1.5">
                <ShoppingBag className="w-4 h-4 text-amber-600" />
                <span>سلة الفاتورة الحالية ({items.length} أصناف)</span>
              </span>
              <span className="font-mono text-emerald-700 font-extrabold">{netTotal.toFixed(4)}</span>
            </div>

            <div className="flex-1 overflow-y-auto p-2 space-y-2">
              {items.length === 0 ? (
                <div className="text-center py-12 text-slate-500 text-xs">
                  السلة فارغة. انقر على بطاقات الأصناف من الشبكة لإضافتها.
                </div>
              ) : (
                items.map((it, idx) => (
                  <div key={it.id} className="bg-slate-50 border border-slate-200 rounded-lg p-2.5 flex items-center justify-between text-xs">
                    <div>
                      <div className="font-bold text-slate-900">{it.product.name}</div>
                      <div className="text-[10px] text-slate-500 font-mono">
                        {it.unitPrice} × {it.quantity} {it.unit}
                      </div>
                    </div>

                    <div className="flex items-center gap-2">
                      <div className="flex items-center bg-white border border-slate-300 rounded-lg overflow-hidden shadow-xs">
                        <button onClick={() => handleUpdateQty(idx, -1)} className="px-2 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 cursor-pointer">
                          <Minus className="w-3.5 h-3.5" />
                        </button>
                        <span className="px-2.5 font-bold text-amber-900 font-mono">{it.quantity}</span>
                        <button onClick={() => handleUpdateQty(idx, 1)} className="px-2 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 cursor-pointer">
                          <Plus className="w-3.5 h-3.5" />
                        </button>
                      </div>

                      <span className="font-mono font-extrabold text-emerald-800 w-16 text-left">{it.total.toFixed(2)}</span>

                      <button onClick={() => setItems(prev => prev.filter((_, i) => i !== idx))} className="text-rose-600 hover:text-rose-800 p-1 cursor-pointer">
                        <Trash2 className="w-4 h-4" />
                      </button>
                    </div>
                  </div>
                ))
              )}
            </div>

            <div className="bg-slate-50 border-t border-slate-200 p-3 flex items-center justify-between text-xs">
              <span className="font-bold text-slate-700">الصافي: <strong className="text-amber-800 font-mono text-base font-extrabold">{netTotal.toFixed(4)}</strong></span>
              <button
                onClick={() => {
                  if (onSaveSuccess) onSaveSuccess(invoiceNo);
                  setAlert({ type: 'success', text: `تم اعتماد وحفظ الفاتورة رقم [${invoiceNo}] باللمس المرئي بنجاح.` });
                }}
                className="px-4 py-2 bg-amber-600 hover:bg-amber-700 text-white font-bold rounded-lg shadow-xs flex items-center gap-1.5 cursor-pointer"
              >
                <CheckCircle2 className="w-4 h-4" />
                <span>حفظ باللمس</span>
              </button>
            </div>
          </div>

          {/* Right Side: Category Filter & Product Cards Grid */}
          <div className="lg:col-span-7 flex flex-col gap-3">
            {/* Category Buttons */}
            <div className="flex items-center gap-1.5 overflow-x-auto pb-1 text-xs">
              {categories.map(cat => (
                <button
                  key={cat}
                  onClick={() => setSelectedCategory(cat)}
                  className={`px-3 py-1.5 rounded-lg font-bold border transition shrink-0 cursor-pointer ${
                    selectedCategory === cat
                      ? 'bg-amber-600 border-amber-600 text-white shadow-xs'
                      : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50'
                  }`}
                >
                  {cat}
                </button>
              ))}
            </div>

            {/* Product Cards Grid */}
            <div className="grid grid-cols-2 sm:grid-cols-3 gap-2.5 overflow-y-auto max-h-[420px] p-1">
              {filteredProducts.map(p => (
                <div
                  key={p.id}
                  onClick={() => handleTouchAddProduct(p)}
                  className="bg-white hover:bg-amber-50/50 border border-slate-200 hover:border-amber-500 rounded-xl p-3 cursor-pointer transition flex flex-col justify-between h-28 shadow-xs group"
                >
                  <div>
                    <div className="font-bold text-xs text-slate-900 group-hover:text-amber-800 line-clamp-1">{p.name}</div>
                    <div className="text-[10px] text-slate-500 font-mono">كود: {p.code}</div>
                  </div>

                  <div className="flex items-end justify-between pt-2 border-t border-slate-100">
                    <span className="text-[10px] text-emerald-800 bg-emerald-50 px-1.5 py-0.5 rounded border border-emerald-200 font-mono font-medium">
                      رصيد: {p.stock || 100}
                    </span>
                    <span className="font-mono font-extrabold text-amber-800 text-xs">
                      {p.salePrice} ريال
                    </span>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
