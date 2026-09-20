'use client';

import React from 'react';
import { ClosedShift } from '@/types/pos';
import { 
  Printer, 
  X, 
  CheckCircle2, 
  AlertTriangle, 
  ArrowDownLeft, 
  Clock, 
  Coins, 
  Building2, 
  UserCheck, 
  FileText,
  CreditCard,
  Wallet
} from 'lucide-react';

interface ZReportModalProps {
  closedShift: ClosedShift | null;
  currency: string;
  onClose: () => void;
}

export const ZReportModal: React.FC<ZReportModalProps> = ({
  closedShift,
  currency,
  onClose,
}) => {
  if (!closedShift) return null;

  const isMatched = closedShift.cashDifference === 0;
  const isOverage = closedShift.cashDifference > 0;
  const isShortage = closedShift.cashDifference < 0;

  return (
    <div 
      id="z-report-modal-backdrop"
      className="fixed inset-0 bg-black/80 backdrop-blur-xs flex items-center justify-center z-50 p-4 font-sans select-none overflow-y-auto" 
      dir="rtl"
    >
      <div 
        id="z-report-modal-container"
        className="bg-white text-slate-900 rounded-xl shadow-2xl border border-slate-300 w-full max-w-2xl overflow-hidden animate-in fade-in zoom-in-95 my-auto"
      >
        {/* Modal Window Header */}
        <div 
          id="z-report-header-bar"
          className="bg-white text-slate-800 px-4 py-3.5 flex items-center justify-between border-b border-slate-200 shadow-2xs print:hidden"
        >
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
              <Coins className="w-4 h-4 text-sky-700" />
            </div>
            <div>
              <h3 className="font-bold text-sm text-slate-900">محضر إقفال الوردية وجرد الصندوق المالي (Z-Report)</h3>
              <p className="text-[11px] text-slate-500">التقرير المالي المعتمد لترحيل المبيعات وتصفية الصندوق</p>
            </div>
          </div>

          <div className="flex items-center gap-2">
            <button
              id="btn-print-z-report"
              onClick={() => window.print()}
              className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-3.5 py-1.5 rounded-lg text-xs flex items-center gap-1.5 shadow-xs transition cursor-pointer"
            >
              <Printer className="w-3.5 h-3.5" />
              <span>طباعة فورية</span>
            </button>
            <button
              id="btn-close-z-report-top"
              onClick={onClose}
              className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg transition cursor-pointer"
              title="إغلاق"
            >
              <X className="w-4 h-4" />
            </button>
          </div>
        </div>

        {/* Printable Z-Report Sheet */}
        <div id="z-report-printable-area" className="p-6 text-xs flex flex-col gap-4 bg-slate-50 print:bg-white print:p-2">
          {/* Store & Document Title */}
          <div className="text-center border-b-2 border-dashed border-slate-300 pb-3">
            <h2 className="text-base font-black text-slate-900">نظام المحاسب سوفت لإدارة المبيعات ونقاط البيع</h2>
            <div className="inline-block bg-slate-900 text-white font-mono font-bold px-3 py-1 rounded mt-1 text-xs">
              محضر إقفال الوردية والتسليم المالي (Z-REPORT #{closedShift.shiftNumber})
            </div>
            <p className="text-[11px] text-slate-500 mt-1">
              الفرع: <strong>{closedShift.branchName || 'الفرع الرئيسي'}</strong> | التاريخ المالي: {closedShift.closedAt.split(' ')[0]}
            </p>
          </div>

          {/* Shift Metadata Badges */}
          <div className="grid grid-cols-2 sm:grid-cols-4 gap-2 bg-white p-3 rounded-lg border border-slate-200">
            <div>
              <span className="text-[10px] text-slate-500 block">رقم الوردية:</span>
              <span className="font-mono font-bold text-sky-800 text-sm">#{closedShift.shiftNumber}</span>
            </div>
            <div>
              <span className="text-[10px] text-slate-500 block">الكاشير المسؤول:</span>
              <span className="font-bold text-slate-800 text-[11px] truncate block">{closedShift.cashierName}</span>
            </div>
            <div>
              <span className="text-[10px] text-slate-500 block">وقت الفتح:</span>
              <span className="font-mono text-slate-700">{closedShift.openedAt}</span>
            </div>
            <div>
              <span className="text-[10px] text-slate-500 block">وقت الإقفال:</span>
              <span className="font-mono font-bold text-slate-900">{closedShift.closedAt}</span>
            </div>
          </div>

          {/* Section 1: Sales & Receipts Summary */}
          <div className="bg-white rounded-lg border border-slate-200 p-3.5 flex flex-col gap-2">
            <h4 className="font-bold text-slate-800 border-b border-slate-100 pb-1.5 flex items-center justify-between">
              <span className="flex items-center gap-1.5">
                <FileText className="w-4 h-4 text-sky-700" />
                ملخص المبيعات والإيرادات بالوردية
              </span>
              <span className="text-[11px] font-normal text-slate-500">
                عدد الفواتير الصادرة: <strong>{closedShift.invoiceCount} فاتورة</strong>
              </span>
            </h4>

            <div className="grid grid-cols-2 sm:grid-cols-3 gap-2.5 pt-1">
              <div className="bg-emerald-50/60 p-2.5 rounded border border-emerald-100">
                <span className="text-[11px] text-emerald-900 block font-medium">المبيعات النقدية (كاش):</span>
                <span className="font-mono font-bold text-emerald-800 text-sm">
                  {closedShift.cashSales.toLocaleString()} {currency}
                </span>
              </div>

              <div className="bg-sky-50/60 p-2.5 rounded border border-sky-100">
                <span className="text-[11px] text-sky-900 block font-medium">مبيعات الشبكة والبطاقات:</span>
                <span className="font-mono font-bold text-sky-800 text-sm">
                  {closedShift.cardSales.toLocaleString()} {currency}
                </span>
              </div>

              <div className="bg-amber-50/60 p-2.5 rounded border border-amber-100">
                <span className="text-[11px] text-amber-900 block font-medium">المبيعات الآجلة (ذمم):</span>
                <span className="font-mono font-bold text-amber-800 text-sm">
                  {closedShift.creditSales.toLocaleString()} {currency}
                </span>
              </div>
            </div>

            <div className="flex items-center justify-between pt-2 border-t border-slate-200 mt-1">
              <span className="font-bold text-slate-700">إجمالي المبيعات الشاملة للوردية:</span>
              <span className="font-mono font-black text-base text-slate-900">
                {closedShift.totalSales.toLocaleString()} {currency}
              </span>
            </div>
          </div>

          {/* Section 2: Cash Reconciliation (جرد ومطابقة النقدية) */}
          <div className="bg-white rounded-lg border border-slate-200 p-3.5 flex flex-col gap-2.5">
            <h4 className="font-bold text-slate-800 border-b border-slate-100 pb-1.5 flex items-center justify-between">
              <span className="flex items-center gap-1.5">
                <Coins className="w-4 h-4 text-emerald-700" />
                مطابقة وجرد الصندوق النقدي (Cash Drawer Reconciliation)
              </span>
              <span className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 ${
                isMatched 
                  ? 'bg-emerald-100 text-emerald-800' 
                  : isOverage 
                    ? 'bg-amber-100 text-amber-800' 
                    : 'bg-rose-100 text-rose-800'
              }`}>
                {isMatched ? (
                  <>
                    <CheckCircle2 className="w-3.5 h-3.5" />
                    <span>الجرد متطابق تماماً</span>
                  </>
                ) : isOverage ? (
                  <>
                    <AlertTriangle className="w-3.5 h-3.5" />
                    <span>يوجد فائض في الصندوق</span>
                  </>
                ) : (
                  <>
                    <AlertTriangle className="w-3.5 h-3.5" />
                    <span>يوجد عجز نقدي في الصندوق</span>
                  </>
                )}
              </span>
            </h4>

            <div className="flex flex-col gap-1.5 text-[11px]">
              <div className="flex justify-between py-1 border-b border-slate-100">
                <span className="text-slate-600">الرصيد الافتتاحي للصندوق (عهدة البداية):</span>
                <span className="font-mono font-bold text-slate-800">
                  {closedShift.openingCash.toLocaleString()} {currency}
                </span>
              </div>
              <div className="flex justify-between py-1 border-b border-slate-100">
                <span className="text-slate-600">المقبوضات النقدية المحصلة من المبيعات:</span>
                <span className="font-mono font-bold text-emerald-700">
                  +{closedShift.cashSales.toLocaleString()} {currency}
                </span>
              </div>
              <div className="flex justify-between py-1.5 border-b border-slate-200 font-bold bg-slate-50 px-2 rounded">
                <span className="text-slate-800">المبلغ المتوقع نظاماً في الدرج:</span>
                <span className="font-mono text-sky-800 font-bold">
                  {closedShift.expectedCash.toLocaleString()} {currency}
                </span>
              </div>
              <div className="flex justify-between py-1.5 border-b border-slate-200 font-bold bg-emerald-50 px-2 rounded">
                <span className="text-emerald-900">المبلغ الفعلي المقبوض بالجرد (العد اليدوي):</span>
                <span className="font-mono text-emerald-900 text-sm font-black">
                  {closedShift.actualCash.toLocaleString()} {currency}
                </span>
              </div>
              <div className="flex justify-between py-1 px-2">
                <span className="font-bold text-slate-700">الفارق المالي (فائض / عجز):</span>
                <span className={`font-mono font-bold text-sm ${
                  isMatched ? 'text-emerald-600' : isOverage ? 'text-amber-600' : 'text-rose-600'
                }`}>
                  {closedShift.cashDifference > 0 ? `+${closedShift.cashDifference.toLocaleString()}` : closedShift.cashDifference.toLocaleString()} {currency}
                </span>
              </div>
            </div>
          </div>

          {/* Section 3: Treasury Transfer Handover (تسليم الفلوس للخزينة) */}
          {closedShift.transferredAmount !== undefined && closedShift.transferredAmount > 0 && (
            <div className="bg-sky-50/70 border border-sky-200 rounded-lg p-3 flex flex-col gap-1.5">
              <h4 className="font-bold text-sky-950 flex items-center gap-1.5 text-xs">
                <ArrowDownLeft className="w-4 h-4 text-sky-700" />
                سند تحويل وتسليم النقدية إلى الخزينة الرئيسية (Treasury Transfer)
              </h4>
              <div className="grid grid-cols-2 sm:grid-cols-3 gap-2 pt-1 text-[11px]">
                <div>
                  <span className="text-slate-500 block">رقم سند التحويل:</span>
                  <span className="font-mono font-bold text-sky-800">
                    سند تحويل رقم #{closedShift.transferVoucherNo || '3001'}
                  </span>
                </div>
                <div>
                  <span className="text-slate-500 block">الجهة المستلمة:</span>
                  <span className="font-bold text-slate-800">
                    {closedShift.transferToAccount || '1111 - الخزينة المركزية'}
                  </span>
                </div>
                <div>
                  <span className="text-slate-500 block">المبلغ المورد والمرحل:</span>
                  <span className="font-mono font-bold text-emerald-700 text-xs">
                    {closedShift.transferredAmount.toLocaleString()} {currency}
                  </span>
                </div>
              </div>
              {closedShift.notes && (
                <p className="text-[10px] text-slate-600 mt-1 italic border-t border-sky-200/60 pt-1">
                  ملاحظات الاستلام: {closedShift.notes}
                </p>
              )}
            </div>
          )}

          {/* Signatures Block */}
          <div className="grid grid-cols-2 gap-6 pt-4 border-t border-slate-300 mt-1 text-center">
            <div className="flex flex-col items-center gap-8">
              <div className="font-bold text-slate-700">
                <span>توقيع الكاشير المسلّم</span>
                <span className="block text-[10px] text-slate-400 font-normal">({closedShift.cashierName})</span>
              </div>
              <div className="w-36 border-b-2 border-dashed border-slate-400"></div>
            </div>

            <div className="flex flex-col items-center gap-8">
              <div className="font-bold text-slate-700">
                <span>توقيع أمين الخزينة / مشرف الصالة</span>
                <span className="block text-[10px] text-slate-400 font-normal">(استلمت المبلغ أصولاً وطابقته)</span>
              </div>
              <div className="w-36 border-b-2 border-dashed border-slate-400"></div>
            </div>
          </div>
        </div>

        {/* Footer Controls */}
        <div className="bg-slate-100 px-4 py-3 border-t border-slate-200 flex items-center justify-between print:hidden">
          <span className="text-[11px] text-slate-500">
            تم تسجيل السند والقيد المحاسبي في دفتر اليومية برقم الوردية.
          </span>

          <div className="flex items-center gap-2">
            <button
              onClick={() => window.print()}
              className="bg-sky-700 hover:bg-sky-600 text-white font-bold px-4 py-2 rounded-lg text-xs flex items-center gap-1.5 shadow transition cursor-pointer"
            >
              <Printer className="w-4 h-4" />
              <span>طباعة التقرير</span>
            </button>
            <button
              onClick={onClose}
              className="bg-slate-300 hover:bg-slate-400 text-slate-800 font-semibold px-4 py-2 rounded-lg text-xs transition cursor-pointer"
            >
              إغلاق
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
