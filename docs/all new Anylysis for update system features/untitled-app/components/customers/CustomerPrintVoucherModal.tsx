'use client';

import React from 'react';
import { CashVoucher } from '@/types/pos';
import { tafqeet } from '@/lib/tafqeet';
import { Printer, X } from 'lucide-react';

export interface CustomerPrintVoucherModalProps {
  voucher: CashVoucher;
  onClose: () => void;
}

export const CustomerPrintVoucherModal: React.FC<CustomerPrintVoucherModalProps> = ({
  voucher,
  onClose,
}) => {
  const isReceipt = voucher.type === 'RECEIPT';

  return (
    <div className="fixed inset-0 z-50 bg-black/70 backdrop-blur-xs flex items-center justify-center p-4 animate-fade-in">
      <div className="bg-white rounded-lg shadow-2xl border border-slate-300 w-full max-w-2xl overflow-hidden flex flex-col">
        <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs print:hidden">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-emerald-50 border border-emerald-200 flex items-center justify-center text-emerald-700 shadow-xs">
              <Printer className="w-4 h-4 text-emerald-700" />
            </div>
            <div>
              <h3 className="font-bold text-sm text-slate-900 leading-none">
                معاينة وطباعة {isReceipt ? 'سند قبض مالي' : 'سند صرف مالي'}
              </h3>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <button
              type="button"
              onClick={() => window.print()}
              className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-3 py-1.5 rounded-lg flex items-center gap-1.5 cursor-pointer shadow-xs transition active:scale-95"
            >
              <Printer className="w-3.5 h-3.5" />
              <span>طباعة</span>
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

        <div className="p-6 bg-white overflow-y-auto text-slate-900 font-sans text-xs">
          {/* Header */}
          <div className="border-b-2 border-slate-800 pb-3 mb-4 flex items-center justify-between">
            <div>
              <h2 className="text-base font-bold text-slate-900">مؤسسة البركة التجارية</h2>
              <p className="text-[11px] text-slate-500">صنعاء - الجمهورية اليمنية</p>
            </div>
            <div className="text-center border-2 border-slate-800 px-4 py-1.5 rounded-lg bg-slate-100">
              <h1 className="text-base font-extrabold">{isReceipt ? 'سند قبض مالي' : 'سند صرف مالي'}</h1>
              <p className="text-xs font-mono font-bold text-sky-900">{voucher.voucherNo}</p>
            </div>
            <div className="text-left font-mono text-[11px] text-slate-600">
              <p>التاريخ: {voucher.date}</p>
              <p>الوقت: {voucher.createdAt?.split(' ')[1] || '12:00 م'}</p>
            </div>
          </div>

          {/* Body Information */}
          <div className="space-y-3 mb-6 bg-slate-50 p-4 rounded border border-slate-200">
            <div className="flex items-center justify-between pb-2 border-b border-slate-200">
              <span className="text-slate-600">المبلغ المحصل / المصروف:</span>
              <span className="font-mono-numbers font-bold text-base text-slate-900">
                {voucher.amount.toLocaleString()} ريال يمني
              </span>
            </div>

            <div className="pb-2 border-b border-slate-200">
              <span className="text-slate-600">المبلغ كتابةً: </span>
              <strong className="text-slate-900 font-bold">{tafqeet(voucher.amount)}</strong>
            </div>

            <div className="pb-2 border-b border-slate-200 flex items-center justify-between">
              <div>
                <span className="text-slate-600">{isReceipt ? 'استلمنا من الأخ / السادة:' : 'صرفنا إلى الأخ / السادة:'} </span>
                <strong className="text-slate-900 text-sm">{voucher.partyName}</strong>
              </div>
              <span className="text-slate-500 font-mono">كود: {voucher.contraAccountCode}</span>
            </div>

            <div className="pb-2 border-b border-slate-200 flex items-center justify-between">
              <div>
                <span className="text-slate-600">طريقة الدفع: </span>
                <strong>
                  {voucher.paymentMethod === 'CASH'
                    ? 'نقداً بالصندوق'
                    : voucher.paymentMethod === 'BANK'
                    ? `تحويل بنكي (${voucher.bankName || 'البنك'})`
                    : `شيك رقم ${voucher.checkNumber || '-'}`}
                </strong>
              </div>
              <span className="text-slate-600 font-mono">حساب: [{voucher.cashAccountCode}] {voucher.cashAccountName}</span>
            </div>

            <div>
              <span className="text-slate-600">وذلك عن: </span>
              <strong className="text-slate-800">{voucher.description}</strong>
            </div>
          </div>

          {/* Signatures */}
          <div className="grid grid-cols-3 gap-4 pt-6 text-center border-t border-slate-300">
            <div>
              <p className="font-bold text-slate-700 mb-6">أمين الصندوق / المحاسب</p>
              <p className="border-t border-slate-400 pt-1 text-slate-500 font-mono">{voucher.createdBy}</p>
            </div>
            <div>
              <p className="font-bold text-slate-700 mb-6">المدير المالي</p>
              <p className="border-t border-slate-400 pt-1 text-slate-500 font-mono">الاعتماد والختم</p>
            </div>
            <div>
              <p className="font-bold text-slate-700 mb-6">{isReceipt ? 'المودع / العميل' : 'المستلم'}</p>
              <p className="border-t border-slate-400 pt-1 text-slate-500 font-mono">التوقيع</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
