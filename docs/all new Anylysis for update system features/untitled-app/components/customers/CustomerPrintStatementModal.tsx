'use client';

import React from 'react';
import { Customer, CustomerStatementMovement } from '@/types/pos';
import { getCurrentDateString, getCurrentTimeString } from '@/lib/idGenerator';
import { tafqeet } from '@/lib/tafqeet';
import { Printer, X } from 'lucide-react';

export interface CustomerPrintStatementModalProps {
  customer: Customer;
  statementData: {
    openingBalance: number;
    movements: CustomerStatementMovement[];
    totalDebit: number;
    totalCredit: number;
    closingBalance: number;
  };
  dateFrom?: string;
  dateTo?: string;
  onClose: () => void;
}

export const CustomerPrintStatementModal: React.FC<CustomerPrintStatementModalProps> = ({
  customer,
  statementData,
  dateFrom,
  dateTo,
  onClose,
}) => {
  const handlePrint = () => {
    window.print();
  };

  return (
    <div className="fixed inset-0 z-50 bg-black/70 backdrop-blur-xs flex items-center justify-center p-4 animate-fade-in">
      <div className="bg-white rounded-lg shadow-2xl border border-slate-300 w-full max-w-4xl overflow-hidden flex flex-col max-h-[95vh]">
        {/* Top Action Bar */}
        <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs print:hidden">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
              <Printer className="w-4 h-4 text-sky-700" />
            </div>
            <div>
              <h3 className="font-bold text-sm text-slate-900 leading-none">معاينة كشف حساب عميل رسمي للطباعة والتصدير</h3>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <button
              type="button"
              onClick={handlePrint}
              className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-3 py-1.5 rounded-lg flex items-center gap-1.5 cursor-pointer shadow-xs transition active:scale-95"
            >
              <Printer className="w-3.5 h-3.5" />
              <span>طباعة الآن</span>
            </button>
            <button
              type="button"
              onClick={onClose}
              className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg text-sm font-bold cursor-pointer transition"
            >
              <X className="w-5 h-5" />
            </button>
          </div>
        </div>

        {/* Printable Paper Area */}
        <div className="p-8 bg-white overflow-y-auto flex-1 font-sans text-slate-900 print:p-0">
          {/* Official Letterhead */}
          <div className="border-b-2 border-slate-800 pb-4 mb-4 flex items-center justify-between">
            <div className="text-right">
              <h1 className="text-lg font-extrabold text-slate-900">مؤسسة البركة للتجارة والاستيراد</h1>
              <p className="text-xs text-slate-600">الجمهورية اليمنية - صنعاء - شارع حدة</p>
              <p className="text-[11px] text-slate-500">هاتف: 01-445566 | س.ت: 102934 | ر.ض: 459012</p>
            </div>
            <div className="text-center bg-slate-100 border border-slate-400 px-4 py-2 rounded">
              <h2 className="text-base font-extrabold text-slate-900">كشف حساب عميل تفصيلي</h2>
              <p className="text-[11px] text-slate-600 font-mono">
                الفترة: {dateFrom || 'البداية'} إلى {dateTo || getCurrentDateString()}
              </p>
            </div>
            <div className="text-left text-xs font-mono text-slate-600">
              <p>تاريخ الطباعة: {getCurrentDateString()}</p>
              <p>الوقت: {getCurrentTimeString()}</p>
            </div>
          </div>

          {/* Customer Metadata Table */}
          <div className="grid grid-cols-2 gap-4 bg-slate-50 p-3 rounded border border-slate-300 text-xs mb-4">
            <div>
              <p className="mb-1"><span className="text-slate-500">اسم العميل:</span> <strong>{customer.name}</strong></p>
              <p className="mb-1"><span className="text-slate-500">رقم الحساب:</span> <strong className="font-mono">{customer.code}</strong></p>
              <p><span className="text-slate-500">الهاتف:</span> <strong className="font-mono">{customer.phone}</strong></p>
            </div>
            <div>
              <p className="mb-1"><span className="text-slate-500">العنوان:</span> <strong>{customer.address}</strong></p>
              <p className="mb-1"><span className="text-slate-500">سقف الائتمان:</span> <strong className="font-mono-numbers">{customer.creditLimit.toLocaleString()} ر.ي</strong></p>
              <p><span className="text-slate-500">مهلة السداد:</span> <strong>{customer.creditDays || 30} يوم</strong></p>
            </div>
          </div>

          {/* Movements Grid */}
          <table className="w-full text-right border-collapse text-xs mb-4 border border-slate-300">
            <thead>
              <tr className="bg-slate-200 text-slate-800 font-bold border-b border-slate-300">
                <th className="p-2 border-l border-slate-300 w-24">التاريخ</th>
                <th className="p-2 border-l border-slate-300 w-24">رقم المستند</th>
                <th className="p-2 border-l border-slate-300 w-28">نوع الحركة</th>
                <th className="p-2 border-l border-slate-300">البيان والشرح</th>
                <th className="p-2 border-l border-slate-300 w-24 text-left">مدين (عليه)</th>
                <th className="p-2 border-l border-slate-300 w-24 text-left">دائن (له)</th>
                <th className="p-2 w-28 text-left">الرصيد التراكمي</th>
              </tr>
            </thead>
            <tbody>
              <tr className="border-b border-slate-300 bg-slate-50 font-semibold">
                <td className="p-2 border-l border-slate-300 font-mono">{dateFrom || '-'}</td>
                <td className="p-2 border-l border-slate-300 font-mono">-</td>
                <td className="p-2 border-l border-slate-300">رصيد سابق</td>
                <td className="p-2 border-l border-slate-300">الرصيد الافتتاحي المنقول</td>
                <td className="p-2 border-l border-slate-300 text-left font-mono-numbers">
                  {statementData.openingBalance > 0 ? statementData.openingBalance.toLocaleString() : '-'}
                </td>
                <td className="p-2 border-l border-slate-300 text-left font-mono-numbers">
                  {statementData.openingBalance < 0 ? Math.abs(statementData.openingBalance).toLocaleString() : '-'}
                </td>
                <td className="p-2 text-left font-mono-numbers font-bold">
                  {statementData.openingBalance.toLocaleString()}
                </td>
              </tr>

              {statementData.movements.map((m, idx) => (
                <tr key={idx} className="border-b border-slate-200">
                  <td className="p-2 border-l border-slate-200 font-mono">{m.date}</td>
                  <td className="p-2 border-l border-slate-200 font-mono font-bold">{m.documentNo}</td>
                  <td className="p-2 border-l border-slate-200">{m.typeLabel}</td>
                  <td className="p-2 border-l border-slate-200">{m.description}</td>
                  <td className="p-2 border-l border-slate-200 text-left font-mono-numbers font-bold">
                    {m.debit > 0 ? m.debit.toLocaleString() : '-'}
                  </td>
                  <td className="p-2 border-l border-slate-200 text-left font-mono-numbers font-bold">
                    {m.credit > 0 ? m.credit.toLocaleString() : '-'}
                  </td>
                  <td className="p-2 text-left font-mono-numbers font-bold bg-slate-50/50">
                    {m.balanceAfter.toLocaleString()}
                  </td>
                </tr>
              ))}

              <tr className="bg-slate-200/90 font-bold border-t-2 border-slate-800">
                <td colSpan={4} className="p-2 border-l border-slate-300 text-left">
                  الإجماليات وصافي الرصيد الختامي المستحق:
                </td>
                <td className="p-2 border-l border-slate-300 text-left font-mono-numbers text-rose-800">
                  {statementData.totalDebit.toLocaleString()}
                </td>
                <td className="p-2 border-l border-slate-300 text-left font-mono-numbers text-emerald-800">
                  {statementData.totalCredit.toLocaleString()}
                </td>
                <td className="p-2 text-left font-mono-numbers text-slate-900 text-sm">
                  {statementData.closingBalance.toLocaleString()} ر.ي
                </td>
              </tr>
            </tbody>
          </table>

          {/* Tafqeet Box */}
          <div className="p-3 bg-slate-50 border border-slate-300 rounded mb-6 text-xs">
            <span className="font-semibold text-slate-600">المبلغ المطلوب سداده كتابةً: </span>
            <strong className="text-slate-900">{tafqeet(statementData.closingBalance)}</strong>
          </div>

          {/* Signatures & Stamp Blocks */}
          <div className="grid grid-cols-3 gap-6 pt-4 text-xs text-center border-t border-slate-300">
            <div>
              <p className="font-bold text-slate-700 mb-8">تنظيم وإعداد المحاسب</p>
              <p className="border-t border-slate-400 pt-1 text-slate-500 font-mono">التوقيع والتاريخ</p>
            </div>
            <div>
              <p className="font-bold text-slate-700 mb-8">اعتماد المدير المالي</p>
              <p className="border-t border-slate-400 pt-1 text-slate-500 font-mono">التوقيع والختم</p>
            </div>
            <div>
              <p className="font-bold text-slate-700 mb-8">مصادقة واستلام العميل</p>
              <p className="border-t border-slate-400 pt-1 text-slate-500 font-mono">توقيع وختم المنشأة</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
