'use client';

import React, { useState } from 'react';
import { 
  FileCheck, 
  Trash2, 
  CheckCircle2, 
  ArrowRight, 
  Sparkles, 
  Info, 
  RefreshCw, 
  Layers, 
  FileSpreadsheet,
  ArrowDownToLine,
  Search
} from 'lucide-react';
import { ProductItem, Customer, MainTabType } from '@/types/pos';

interface Props {
  products: ProductItem[];
  customers: Customer[];
  onSelectTab?: (tab: MainTabType) => void;
  onSaveSuccess?: (invoiceNo: string) => void;
}

export const FrmSalesInvoice_ImportConvert: React.FC<Props> = ({
  products,
  customers,
  onSelectTab,
  onSaveSuccess,
}) => {
  const [invoiceNo, setInvoiceNo] = useState('1009');
  const [showImportModal, setShowImportModal] = useState(false);

  // Sample quotations available for conversion
  const [sampleQuotations] = useState([
    {
      id: 'QUOT-4001',
      quotationNo: '4001',
      customerName: 'مؤسسة البركة للتجارة',
      date: '2026-09-15',
      totalAmount: 4500,
      itemCount: 4,
    },
    {
      id: 'QUOT-4002',
      quotationNo: '4002',
      customerName: 'شركة النجاح للمقاولات',
      date: '2026-09-16',
      totalAmount: 12800,
      itemCount: 6,
    },
    {
      id: 'DRAFT-1002',
      quotationNo: 'مسودة 1002',
      customerName: 'عميل نقدي عام',
      date: '2026-09-17',
      totalAmount: 890,
      itemCount: 2,
    },
  ]);

  const [items, setItems] = useState<Array<{
    id: string;
    product: ProductItem;
    quantity: number;
    unit: string;
    unitPrice: number;
    discount: number;
    total: number;
    sourceDoc?: string;
  }>>([
    {
      id: 'S6-1',
      product: products[0] || { id: 'P1', name: 'بطاطس عمان 25غ', salePrice: 0.0714, unit: 'حبة' },
      quantity: 50,
      unit: 'باكت',
      unitPrice: 0.80,
      discount: 2.00,
      total: 38.00,
      sourceDoc: 'عرض سعر 4001',
    }
  ]);

  const [alert, setAlert] = useState<{ type: 'success' | 'info' | 'warning'; text: string } | null>({
    type: 'info',
    text: '📥 وضع الاختبار 6 [Import & Convert]: تحويل عروض الأسعار المسودة وأوامر البيع بنقرة واحدة وتعبئة الفاتورة تلقائياً.',
  });

  const handleImportQuotation = (quotation: typeof sampleQuotations[0]) => {
    // Generate bulk items from quotation
    const imported: typeof items = products.slice(0, Math.min(quotation.itemCount, products.length)).map((p, idx) => ({
      id: `IMP-${idx}-${Date.now()}`,
      product: p,
      quantity: (idx + 1) * 10,
      unit: p.unit || 'حبة',
      unitPrice: p.salePrice || 10,
      discount: idx * 0.5,
      total: Math.max(0, ((idx + 1) * 10 * (p.salePrice || 10)) - (idx * 0.5)),
      sourceDoc: `عرض سعر ${quotation.quotationNo}`,
    }));

    setItems(imported);
    setShowImportModal(false);
    setAlert({
      type: 'success',
      text: `تم استيراد ${imported.length} أصناف بنجاح من [${quotation.quotationNo} - ${quotation.customerName}] وتحويلها للفاتورة!`
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
            <div className="w-8 h-8 rounded-lg bg-blue-600 flex items-center justify-center text-white font-bold text-sm shadow-xs">
              6
            </div>
            <div>
              <h1 className="text-sm font-bold text-slate-900 flex items-center gap-2">
                <span>استراتيجية 6: تحويل العروض والاستيراد [One-Click Quotation Import]</span>
                <span className="text-[10px] bg-blue-50 text-blue-700 border border-blue-200 px-2 py-0.5 rounded-full font-mono font-bold">
                  frmSalesInvoice_ImportConvert
                </span>
              </h1>
              <p className="text-[11px] text-slate-500">
                اختبار سرعة استيراد عروض الأسعار والمسودات وتحويلها إلى فواتير مبيعات دون إعادة كتابة الأصناف.
              </p>
            </div>
          </div>
        </div>

        <button
          onClick={() => setShowImportModal(true)}
          className="px-3.5 py-1.5 bg-blue-600 hover:bg-blue-700 text-white text-xs font-bold rounded-lg shadow-xs flex items-center gap-2 transition cursor-pointer"
        >
          <ArrowDownToLine className="w-4 h-4" />
          <span>استيراد عرض سعر / مسودة</span>
        </button>
      </div>

      <div className="p-4 flex-1 flex flex-col gap-4">
        {/* Alert */}
        {alert && (
          <div className={`p-2.5 rounded-lg border text-xs flex items-center justify-between ${
            alert.type === 'success' ? 'bg-emerald-50 border-emerald-300 text-emerald-900 font-medium' : 'bg-blue-50 border-blue-300 text-blue-900 font-medium'
          }`}>
            <span className="flex items-center gap-2">
              <Info className="w-4 h-4 shrink-0" />
              <span>{alert.text}</span>
            </span>
            <button onClick={() => setAlert(null)} className="text-slate-400 hover:text-slate-700 font-bold">✕</button>
          </div>
        )}

        {/* Import Action Banner */}
        <div className="bg-blue-50 border border-blue-200 rounded-xl p-3 flex items-center justify-between text-xs shadow-xs">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 rounded-lg bg-blue-600 text-white flex items-center justify-center font-extrabold shadow-xs">
              <FileCheck className="w-5 h-5" />
            </div>
            <div>
              <span className="font-bold text-blue-950 block">اختيار وتحويل المستندات السابقة (عروض الأسعار) إلى فاتورة بنقرة</span>
              <span className="text-[11px] text-slate-600 font-medium">تحافظ على أسعار الصرف، الخصومات المتفق عليها وتفاصيل الوحدة.</span>
            </div>
          </div>
          <button
            onClick={() => setShowImportModal(true)}
            className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-lg text-xs shadow-xs transition cursor-pointer"
          >
            اختبار نافذة الاستيراد
          </button>
        </div>

        {/* Invoice Grid */}
        <div className="flex-1 bg-white border border-slate-200 rounded-xl overflow-hidden flex flex-col min-h-[260px] shadow-xs">
          <div className="bg-slate-50 px-3 py-2 border-b border-slate-200 text-xs font-bold text-slate-800 flex items-center justify-between">
            <span>الأسطر المستوردة المحولة ({items.length} أصناف)</span>
            <span className="text-blue-800 font-bold text-[11px]">مستورد بضغطة زر واحدة</span>
          </div>

          <div className="overflow-x-auto flex-1">
            <table className="w-full text-right text-xs border-collapse">
              <thead>
                <tr className="bg-slate-100/80 text-slate-700 border-b border-slate-200 font-bold">
                  <th className="p-2.5 text-center w-12">#</th>
                  <th className="p-2.5">اسم الصنف</th>
                  <th className="p-2.5 text-center">المستند المصدر</th>
                  <th className="p-2.5 text-center">الوحدة</th>
                  <th className="p-2.5 text-center">الكمية</th>
                  <th className="p-2.5 text-center">السعر</th>
                  <th className="p-2.5 text-center">الخصم</th>
                  <th className="p-2.5 text-center">الإجمالي</th>
                  <th className="p-2.5 text-center w-16">إجراء</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200">
                {items.length === 0 ? (
                  <tr>
                    <td colSpan={9} className="p-8 text-center text-slate-500">
                      لا توجد أسطر مستوردة بعد. انقر على &quot;استيراد عرض سعر / مسودة&quot; لاستيراد البيانات.
                    </td>
                  </tr>
                ) : (
                  items.map((it, idx) => (
                    <tr key={it.id} className="hover:bg-slate-50">
                      <td className="p-2 text-center text-slate-500 font-mono font-medium">{idx + 1}</td>
                      <td className="p-2 font-bold text-slate-900">{it.product.name}</td>
                      <td className="p-2 text-center text-blue-800 font-mono font-bold">{it.sourceDoc || 'عرض سعر'}</td>
                      <td className="p-2 text-center text-slate-700 font-medium">{it.unit}</td>
                      <td className="p-2 text-center font-bold text-emerald-800">{it.quantity}</td>
                      <td className="p-2 text-center font-mono text-sky-800 font-bold">{it.unitPrice}</td>
                      <td className="p-2 text-center font-mono text-amber-800 font-bold">{it.discount}</td>
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
            <span className="font-bold text-slate-700">إجمالي الفاتورة المحولة: <strong className="text-blue-900 text-base font-mono font-extrabold mr-2">{netTotal.toFixed(4)}</strong></span>
          </div>
          <button
            onClick={() => {
              if (onSaveSuccess) onSaveSuccess(invoiceNo);
              setAlert({ type: 'success', text: `تم اعتماد وتوليد الفاتورة المحولة رقم [${invoiceNo}] بنجاح!` });
            }}
            className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-lg shadow-xs flex items-center gap-2 cursor-pointer"
          >
            <CheckCircle2 className="w-4 h-4" />
            <span>حفظ واعتماد تحويل العروض</span>
          </button>
        </div>
      </div>

      {/* Import Quotation Modal */}
      {showImportModal && (
        <div className="fixed inset-0 bg-slate-900/60 backdrop-blur-xs z-50 flex items-center justify-center p-4">
          <div className="bg-white border border-blue-300 rounded-xl max-w-2xl w-full shadow-2xl overflow-hidden flex flex-col">
            <div className="bg-blue-900 p-3 px-4 border-b border-blue-800 flex items-center justify-between text-xs font-bold text-white">
              <span className="flex items-center gap-2">
                <ArrowDownToLine className="w-4 h-4 text-blue-200" />
                <span>اختر عرض السعر المراد تحويله إلى فاتورة مبيعات مباشرة</span>
              </span>
              <button onClick={() => setShowImportModal(false)} className="text-blue-200 hover:text-white cursor-pointer font-bold">✕</button>
            </div>

            <div className="p-4 space-y-3">
              <p className="text-xs text-slate-600 font-medium">انقر فوق أي من عروض الأسعار التالية لتعبئة أسطر الفاتورة تلقائياً:</p>

              <div className="space-y-2">
                {sampleQuotations.map(q => (
                  <div
                    key={q.id}
                    className="bg-slate-50 hover:bg-blue-50/50 border border-slate-200 hover:border-blue-300 rounded-lg p-3 flex items-center justify-between text-xs cursor-pointer transition shadow-xs"
                  >
                    <div>
                      <div className="font-bold text-slate-900 flex items-center gap-2">
                        <span>رقم العرض: {q.quotationNo}</span>
                        <span className="text-slate-500 font-medium">({q.customerName})</span>
                      </div>
                      <div className="text-[10px] text-slate-500 font-mono pt-0.5">التاريخ: {q.date} | الأصناف: {q.itemCount}</div>
                    </div>
                    <div className="flex items-center gap-3">
                      <span className="font-mono font-extrabold text-emerald-800 text-sm">{q.totalAmount} ريال</span>
                      <button
                        onClick={() => handleImportQuotation(q)}
                        className="px-3 py-1.5 bg-blue-600 hover:bg-blue-700 text-white rounded-lg font-bold text-xs cursor-pointer"
                      >
                        تحويل واستيراد
                      </button>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
