'use client';

import React from 'react';
import { Printer } from 'lucide-react';
import { Invoice } from '@/types/pos';
import { tafqeet } from '@/lib/tafqeet';

export interface PurchaseInvoicePrintModalProps {
  invoice: Invoice | null;
  currency: string;
  onClose: () => void;
}

export const PurchaseInvoicePrintModal: React.FC<PurchaseInvoicePrintModalProps> = ({
  invoice,
  currency,
  onClose,
}) => {
  if (!invoice) return null;

  return (
    <div className="fixed inset-0 bg-slate-950/80 backdrop-blur-xs flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-4xl max-h-[92vh] flex flex-col overflow-hidden">
        {/* Modal Header Bar */}
        <div className="bg-white border-b border-slate-200 px-5 py-3 flex items-center justify-between print:hidden text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
              <Printer className="w-4 h-4 text-sky-700" />
            </div>
            <h3 className="font-bold text-sm text-slate-900">
              معاينة وطباعة {invoice.type === 'PURCHASE_RETURN' ? 'سند مردودات مشتريات' : 'فاتورة مشتريات رسمية'}
            </h3>
          </div>
          <div className="flex items-center gap-2">
            <button
              type="button"
              onClick={() => window.print()}
              className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-3 py-1.5 rounded-lg text-xs flex items-center gap-1.5 cursor-pointer shadow-xs"
            >
              <Printer className="w-3.5 h-3.5" />
              <span>طباعة [Ctrl+P]</span>
            </button>
            <button
              type="button"
              onClick={onClose}
              className="text-slate-400 hover:text-slate-700 p-1.5 rounded-lg hover:bg-slate-100 font-bold cursor-pointer text-sm transition"
            >
              ✕
            </button>
          </div>
        </div>

        {/* Printable A4 Body */}
        <div className="p-6 flex-1 overflow-y-auto bg-white text-slate-900 font-sans print:p-0 print:overflow-visible">
          {/* Official Header */}
          <div className="border-b-2 border-slate-800 pb-3 mb-4 flex items-center justify-between">
            <div>
              <h1 className="text-lg font-black text-slate-900">اليمن للمحاسبة ونقاط البيع المحدودة</h1>
              <p className="text-xs text-slate-600">المركز الرئيسي - صنعاء - الجمهورية اليمنية</p>
              <p className="text-[11px] text-slate-500 font-mono">س.ت: 108442 | الرقم الضريبي: TAX-YE-778210</p>
            </div>

            <div className="text-center border-2 border-slate-800 px-4 py-1.5 rounded-lg">
              <h2 className="text-base font-black text-slate-900">
                {invoice.type === 'PURCHASE_RETURN' ? 'سند مردودات مشتريات' : 'فاتورة مشتريات وتوريد مخزني'}
              </h2>
              <span className="font-mono font-bold text-xs text-slate-700 block">
                {invoice.invoiceNo}
              </span>
            </div>

            <div className="text-left text-xs font-mono">
              <p><span className="text-slate-500">التاريخ:</span> {invoice.date}</p>
              <p><span className="text-slate-500">الوقت:</span> {invoice.time}</p>
              <p><span className="text-slate-500">طريقة الدفع:</span> {invoice.paymentType === 'CREDIT' ? 'آجل' : 'نقدي'}</p>
            </div>
          </div>

          {/* Metadata Grid */}
          <div className="grid grid-cols-2 gap-4 bg-slate-50 p-3 rounded border border-slate-200 text-xs mb-4">
            <div>
              <p className="mb-1"><span className="font-bold text-slate-700">المورد:</span> {invoice.customerOrSupplierName}</p>
              <p className="mb-1"><span className="font-bold text-slate-700">رقم الفاتورة المرجعي:</span> <span className="font-mono">{invoice.referenceNo || 'بدون'}</span></p>
              <p><span className="font-bold text-slate-700">البيان:</span> {invoice.notes}</p>
            </div>
            <div>
              <p className="mb-1"><span className="font-bold text-slate-700">المستودع المستلم:</span> {invoice.warehouseName || 'المستودع الرئيسي'}</p>
              <p className="mb-1"><span className="font-bold text-slate-700">مسؤول الإدخال:</span> {invoice.cashierName}</p>
              <p><span className="font-bold text-slate-700">حالة الفاتورة:</span> معتمدة ومرحلة للأستاذ العام</p>
            </div>
          </div>

          {/* Items Table */}
          <div className="border border-slate-400 rounded overflow-hidden mb-4">
            <table className="w-full text-right border-collapse text-xs">
              <thead className="bg-slate-200 text-slate-800 font-bold border-b border-slate-400">
                <tr>
                  <th className="w-8 text-center p-2">#</th>
                  <th className="w-20 p-2">الكود</th>
                  <th className="p-2">الصنف والبيان</th>
                  <th className="w-16 text-center p-2">الوحدة</th>
                  <th className="w-16 text-center p-2">الكمية</th>
                  <th className="w-24 text-left p-2">السعر</th>
                  <th className="w-20 text-left p-2">الخصم</th>
                  <th className="w-28 text-left p-2">الإجمالي ({currency})</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-300">
                {invoice.items.map((item, idx) => (
                  <tr key={idx}>
                    <td className="text-center font-mono-numbers p-2">{idx + 1}</td>
                    <td className="font-mono p-2">{item.item.code}</td>
                    <td className="font-bold p-2">
                      {item.item.name}
                      {item.batchNo && (
                        <span className="text-[10px] text-slate-500 block font-mono">
                          دفعة: {item.batchNo} | انتهاء: {item.expiryDate}
                        </span>
                      )}
                    </td>
                    <td className="text-center p-2">{item.selectedUnit}</td>
                    <td className="text-center font-mono-numbers font-bold p-2">{item.quantity}</td>
                    <td className="text-left font-mono-numbers p-2">{item.unitPrice.toLocaleString()}</td>
                    <td className="text-left font-mono-numbers p-2">{item.discount > 0 ? item.discount.toLocaleString() : '0'}</td>
                    <td className="text-left font-mono-numbers font-black p-2">{item.total.toLocaleString()}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {/* Totals & Tafqeet */}
          <div className="flex justify-between items-start mb-6 text-xs">
            <div className="max-w-md bg-slate-50 p-2.5 rounded border border-slate-200">
              <span className="font-bold text-slate-700 block mb-1">المبلغ كتابة وتفقيطاً:</span>
              <p className="font-bold text-sky-950">{tafqeet(invoice.grandTotal, currency)}</p>
            </div>

            <div className="w-64 border border-slate-300 rounded p-2 text-xs space-y-1">
              <div className="flex justify-between">
                <span className="text-slate-600">الإجمالي:</span>
                <span className="font-mono-numbers">{invoice.subtotal.toLocaleString()} {currency}</span>
              </div>
              {invoice.discountTotal > 0 && (
                <div className="flex justify-between text-rose-600">
                  <span>الخصم التجاري:</span>
                  <span className="font-mono-numbers">-{invoice.discountTotal.toLocaleString()} {currency}</span>
                </div>
              )}
              {invoice.taxTotal > 0 && (
                <div className="flex justify-between">
                  <span>ضريبة القيمة المضافة:</span>
                  <span className="font-mono-numbers">{invoice.taxTotal.toLocaleString()} {currency}</span>
                </div>
              )}
              <div className="flex justify-between border-t border-slate-400 pt-1 font-black text-sm text-slate-900">
                <span>الصافي النهائي:</span>
                <span className="font-mono-numbers">{invoice.grandTotal.toLocaleString()} {currency}</span>
              </div>
            </div>
          </div>

          {/* Signatures */}
          <div className="grid grid-cols-4 gap-4 text-center text-xs border-t border-slate-300 pt-6 mt-8">
            <div>
              <p className="font-bold mb-8 text-slate-700">المستلم / أمين المخزن</p>
              <p className="border-t border-dashed border-slate-400 pt-1 font-mono text-[11px]">التوقيع والختم</p>
            </div>
            <div>
              <p className="font-bold mb-8 text-slate-700">مندوب / ناقل البضاعة</p>
              <p className="border-t border-dashed border-slate-400 pt-1 font-mono text-[11px]">التوقيع</p>
            </div>
            <div>
              <p className="font-bold mb-8 text-slate-700">المحاسب المسؤول</p>
              <p className="border-t border-dashed border-slate-400 pt-1 font-mono text-[11px]">{invoice.cashierName}</p>
            </div>
            <div>
              <p className="font-bold mb-8 text-slate-700">المدير المالي / الاعتماد</p>
              <p className="border-t border-dashed border-slate-400 pt-1 font-mono text-[11px]">الاعتماد والتأشير</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
