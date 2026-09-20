'use client';

import React, { useState, useRef, useEffect } from 'react';
import { 
  Scan, 
  Zap, 
  Trash2, 
  CheckCircle2, 
  ArrowRight, 
  Volume2, 
  Sparkles, 
  Info,
  RefreshCw,
  ShieldCheck
} from 'lucide-react';
import { ProductItem, Customer, MainTabType } from '@/types/pos';

interface Props {
  products: ProductItem[];
  customers: Customer[];
  onSelectTab?: (tab: MainTabType) => void;
  onSaveSuccess?: (invoiceNo: string) => void;
}

export const FrmSalesInvoice_ContinuousScanner: React.FC<Props> = ({
  products,
  customers,
  onSelectTab,
  onSaveSuccess,
}) => {
  const [invoiceNo, setInvoiceNo] = useState('1006');
  const [barcodeInput, setBarcodeInput] = useState('');
  const [isScannerFlash, setIsScannerFlash] = useState(false);
  const [lastScannedItem, setLastScannedItem] = useState<{ name: string; barcode: string; time: string } | null>(null);

  const barcodeRef = useRef<HTMLInputElement>(null);

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
      id: 'S3-1',
      product: products[0] || { id: 'P1', barcode: '629100099999', name: 'بطاطس عمان 25غ', salePrice: 0.0714, unit: 'حبة' },
      quantity: 1,
      unit: 'حبة',
      unitPrice: 0.0714,
      discount: 0,
      total: 0.0714,
    }
  ]);

  const [alert, setAlert] = useState<{ type: 'success' | 'info' | 'warning'; text: string } | null>({
    type: 'info',
    text: '⚡ وضع الاختبار 3 [Continuous Scanner]: محاكاة قراءة الباركود الليزرية المباشرة المتواصلة. انقر على الحقل وأدخل أي باركود (أو اختر من القائمة النموذجية).',
  });

  // Keep scanner input auto-focused
  useEffect(() => {
    if (barcodeRef.current) barcodeRef.current.focus();
  }, []);

  const handleBarcodeSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const code = barcodeInput.trim().toLowerCase();
    if (!code) return;

    // Find matching product by barcode or code
    let matchedProd = products.find(p => p.barcode?.toLowerCase() === code || p.code?.toLowerCase() === code || p.id.toLowerCase() === code);
    
    // If not matched, pick first or create dummy demo product
    if (!matchedProd) {
      matchedProd = products[0] || { id: 'P1', barcode: code, name: `صنف باركود (${code})`, salePrice: 15, unit: 'حبة' };
    }

    // Trigger visual laser flash
    setIsScannerFlash(true);
    setTimeout(() => setIsScannerFlash(false), 450);

    // Audio frequency simulated feedback
    setLastScannedItem({
      name: matchedProd.name,
      barcode: code,
      time: new Date().toLocaleTimeString('ar-YE'),
    });

    // Add or increment
    setItems(prev => {
      const idx = prev.findIndex(it => it.product.id === matchedProd?.id);
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
          id: `S3-${Date.now()}`,
          product: matchedProd!,
          quantity: 1,
          unit: matchedProd!.unit || 'حبة',
          unitPrice: matchedProd!.salePrice || 10,
          discount: 0,
          total: matchedProd!.salePrice || 10,
        }
      ];
    });

    setAlert({ type: 'success', text: `⚡ مسح ضوئي ناجح: تم إدراج [${matchedProd.name}] بزيادة تلقائية (+1)` });
    setBarcodeInput('');
    if (barcodeRef.current) barcodeRef.current.focus();
  };

  const netTotal = items.reduce((acc, it) => acc + it.total, 0);

  return (
    <div className="flex flex-col h-full bg-slate-100 text-slate-900 font-sans select-none overflow-y-auto">
      {/* Laser Header */}
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
            <div className="w-8 h-8 rounded-lg bg-emerald-600 flex items-center justify-center text-white font-bold text-sm shadow-xs">
              3
            </div>
            <div>
              <h1 className="text-sm font-bold text-slate-900 flex items-center gap-2">
                <span>استراتيجية 3: أسلوب المسح الليزري الفوري [Continuous Scanner]</span>
                <span className="text-[10px] bg-emerald-50 text-emerald-700 border border-emerald-200 px-2 py-0.5 rounded-full font-mono font-bold">
                  frmSalesInvoice_ContinuousScanner
                </span>
              </h1>
              <p className="text-[11px] text-slate-500">
                اختبار إدخال الكميات بالمسح المتكرر لباركود الأصناف مع مؤثرات بصرية وصوتية للتأكيد الفوري.
              </p>
            </div>
          </div>
        </div>

        <button
          onClick={() => setItems([])}
          className="px-3 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 text-xs font-bold rounded-lg border border-slate-300 flex items-center gap-1.5 transition cursor-pointer"
        >
          <RefreshCw className="w-3.5 h-3.5" />
          <span>تفريغ السجل</span>
        </button>
      </div>

      <div className="p-4 flex-1 flex flex-col gap-4">
        {/* Alert */}
        {alert && (
          <div className={`p-2.5 rounded-lg border text-xs flex items-center justify-between ${
            alert.type === 'success' ? 'bg-emerald-50 border-emerald-300 text-emerald-900 font-medium' : 'bg-sky-50 border-sky-300 text-sky-900 font-medium'
          }`}>
            <span className="flex items-center gap-2">
              <Zap className="w-4 h-4 text-emerald-600 shrink-0" />
              <span>{alert.text}</span>
            </span>
            <button onClick={() => setAlert(null)} className="text-slate-400 hover:text-slate-700 font-bold">✕</button>
          </div>
        )}

        {/* Laser Scanner Control Panel */}
        <div className={`transition-all duration-300 rounded-xl p-4 border-2 ${
          isScannerFlash 
            ? 'bg-emerald-100 border-emerald-500 shadow-md scale-[1.01]' 
            : 'bg-white border-emerald-500 shadow-xs'
        }`}>
          <form onSubmit={handleBarcodeSubmit} className="flex flex-col gap-3">
            <div className="flex items-center justify-between text-xs">
              <span className="font-extrabold text-emerald-800 flex items-center gap-2">
                <Scan className="w-5 h-5 text-emerald-600 animate-pulse" />
                <span>حقل القارئ الآلي المباشر (Barcode Laser Port)</span>
              </span>
              <span className="text-[11px] text-emerald-800 bg-emerald-50 px-2.5 py-1 rounded-full border border-emerald-200 flex items-center gap-1 font-bold">
                <ShieldCheck className="w-3.5 h-3.5 text-emerald-600" />
                <span>القارئ جاهز للعمل المتواصل (Continuous Scan ON)</span>
              </span>
            </div>

            <div className="flex items-center gap-3">
              <input
                ref={barcodeRef}
                type="text"
                value={barcodeInput}
                onChange={e => setBarcodeInput(e.target.value)}
                placeholder="امسح الباركود هنا (أو أدخل مثل 629100099999)..."
                className="flex-1 bg-slate-50 border-2 border-emerald-500 text-slate-900 font-mono font-extrabold text-base rounded-lg px-4 py-2.5 focus:outline-none focus:bg-white focus:ring-4 focus:ring-emerald-500/20"
              />
              <button
                type="submit"
                className="bg-emerald-600 hover:bg-emerald-700 text-white font-extrabold px-6 py-2.5 rounded-lg text-xs shadow-xs flex items-center gap-2 cursor-pointer transition"
              >
                <Zap className="w-4 h-4" />
                <span>محاكاة مسح الباركود</span>
              </button>
            </div>

            {/* Quick Demo Barcode Buttons */}
            <div className="flex flex-wrap items-center gap-2 text-xs pt-1">
              <span className="text-slate-600 text-[11px] font-medium">أمثلة باركود سريعة للاختبار:</span>
              {products.slice(0, 4).map(p => (
                <button
                  key={p.id}
                  type="button"
                  onClick={() => {
                    setBarcodeInput(p.barcode || p.code || p.id);
                  }}
                  className="bg-slate-100 hover:bg-slate-200 border border-slate-300 text-slate-800 px-2.5 py-1 rounded-lg font-mono text-[11px] transition cursor-pointer font-medium"
                >
                  {p.name} ({p.barcode || p.code})
                </button>
              ))}
            </div>
          </form>

          {/* Last Scanned Item Card */}
          {lastScannedItem && (
            <div className="mt-3 bg-emerald-50 border border-emerald-200 rounded-lg p-2.5 flex items-center justify-between text-xs">
              <div className="flex items-center gap-2 text-emerald-900 font-medium">
                <Volume2 className="w-4 h-4 text-emerald-600" />
                <span>آخر مسح ناجح: <strong>{lastScannedItem.name}</strong></span>
                <span className="font-mono text-slate-600">({lastScannedItem.barcode})</span>
              </div>
              <span className="text-[10px] text-slate-500 font-mono font-medium">{lastScannedItem.time}</span>
            </div>
          )}
        </div>

        {/* Live Scanned Items DataGrid */}
        <div className="flex-1 bg-white border border-slate-200 rounded-xl overflow-hidden flex flex-col min-h-[260px] shadow-xs">
          <div className="bg-slate-50 px-3 py-2 border-b border-slate-200 text-xs font-bold text-slate-800 flex items-center justify-between">
            <span>قائمة الأصناف الممسوحة بالباركود ({items.length} أصناف)</span>
            <span className="text-emerald-700 font-bold text-[11px]">⚡ تزيد الكمية تلقائياً عند مسح نفس الصنف</span>
          </div>

          <div className="overflow-x-auto flex-1">
            <table className="w-full text-right text-xs border-collapse">
              <thead>
                <tr className="bg-slate-100/80 text-slate-700 border-b border-slate-200 font-bold">
                  <th className="p-2.5 text-center w-12">#</th>
                  <th className="p-2.5">اسم الصنف</th>
                  <th className="p-2.5 text-center">الباركود</th>
                  <th className="p-2.5 text-center bg-emerald-50 text-emerald-800">الكمية (+1 كل مسحة)</th>
                  <th className="p-2.5 text-center">السعر</th>
                  <th className="p-2.5 text-center">الإجمالي</th>
                  <th className="p-2.5 text-center w-16">إجراء</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200">
                {items.length === 0 ? (
                  <tr>
                    <td colSpan={7} className="p-8 text-center text-slate-500">
                      لا توجد أصناف ممسوحة بعد. استخدم خانة الباركود أعلاه أو انقر على الأمثلة للتجربة.
                    </td>
                  </tr>
                ) : (
                  items.map((it, idx) => (
                    <tr key={it.id} className="hover:bg-slate-50">
                      <td className="p-2 text-center text-slate-500 font-mono font-medium">{idx + 1}</td>
                      <td className="p-2 font-bold text-slate-900">{it.product.name}</td>
                      <td className="p-2 text-center font-mono text-slate-600 font-medium">{it.product.barcode || it.product.code}</td>
                      <td className="p-2 text-center font-extrabold text-emerald-800 bg-emerald-50/50 text-sm">
                        {it.quantity} {it.unit}
                      </td>
                      <td className="p-2 text-center font-mono text-sky-800 font-bold">{it.unitPrice}</td>
                      <td className="p-2 text-center font-mono font-extrabold text-emerald-800">{it.total.toFixed(4)}</td>
                      <td className="p-2 text-center">
                        <button onClick={() => setItems(prev => prev.filter((_, i) => i !== idx))} className="text-rose-600 hover:text-rose-800 p-1 rounded hover:bg-rose-50 cursor-pointer">
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
            <span className="font-bold text-slate-700">الإجمالي: <strong className="text-emerald-800 text-base font-mono font-extrabold mr-2">{netTotal.toFixed(4)}</strong></span>
          </div>
          <button
            onClick={() => {
              if (onSaveSuccess) onSaveSuccess(invoiceNo);
              setAlert({ type: 'success', text: `تم حفظ واختبار الفاتورة رقم [${invoiceNo}] بالمسح الضوئي بنجاح!` });
            }}
            className="px-4 py-2 bg-emerald-600 hover:bg-emerald-700 text-white font-bold rounded-lg shadow-xs flex items-center gap-2 cursor-pointer"
          >
            <CheckCircle2 className="w-4 h-4" />
            <span>حفظ واعتماد المسح الضوئي</span>
          </button>
        </div>
      </div>
    </div>
  );
};
