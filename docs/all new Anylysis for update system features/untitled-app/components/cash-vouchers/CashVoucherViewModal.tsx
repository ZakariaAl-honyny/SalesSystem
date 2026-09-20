import React from 'react';
import { Eye, FileText, CheckCircle2, ShieldAlert, BookOpen, AlertCircle, Calendar, Hash, Tag, Coins, X, Ban, Printer } from 'lucide-react';
import { CashVoucher, AccountNode } from '@/types/pos';

interface CashVoucherViewModalProps {
  viewingVoucher: CashVoucher | null;
  setViewingVoucher: (v: CashVoucher | null) => void;
  setPrintingVoucher: (v: CashVoucher | null) => void;
  currency: string;
  chartOfAccounts: AccountNode[];
}

export const CashVoucherViewModal: React.FC<CashVoucherViewModalProps> = ({
  viewingVoucher, setViewingVoucher, setPrintingVoucher, currency, chartOfAccounts
}) => {
  if (!viewingVoucher) return null;
  return (
        <div className="fixed inset-0 bg-black/60 z-50 flex items-center justify-center p-3 backdrop-blur-xs">
          <div className="bg-white rounded-lg shadow-2xl border border-slate-400 w-full max-w-lg overflow-hidden flex flex-col">
            <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
              <div className="flex items-center gap-2.5 font-bold text-xs text-slate-900">
                <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
                  <FileText className="w-4 h-4 text-sky-700" />
                </div>
                <span>تفاصيل السند المالي: <span className="font-mono text-sky-700">{viewingVoucher.voucherNo}</span></span>
              </div>
              <button
                onClick={() => setViewingVoucher(null)}
                className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg cursor-pointer transition"
              >
                <X className="w-4 h-4" />
              </button>
            </div>

            <div className="p-4 space-y-3 text-xs overflow-y-auto max-h-[80vh]">
              <div className="grid grid-cols-2 gap-2 bg-slate-50 p-2.5 rounded border border-slate-200">
                <div>
                  <span className="text-slate-500">نوع السند: </span>
                  <b className={viewingVoucher.type === 'RECEIPT' ? 'text-emerald-700' : 'text-rose-700'}>
                    {viewingVoucher.type === 'RECEIPT' ? 'سند قبض' : 'سند صرف'}
                  </b>
                </div>
                <div>
                  <span className="text-slate-500">التاريخ: </span>
                  <b className="font-mono">{viewingVoucher.date}</b>
                </div>
                <div>
                  <span className="text-slate-500">المبلغ: </span>
                  <b className="font-mono-numbers text-sm text-slate-900">{viewingVoucher.amount.toLocaleString()} {currency}</b>
                </div>
                <div>
                  <span className="text-slate-500">طريقة الدفع: </span>
                  <b>{viewingVoucher.paymentMethod === 'CASH' ? 'نقداً (صندوق)' : 'بنك / شيك'}</b>
                </div>
              </div>

              <div className="space-y-1.5 border border-slate-200 p-2.5 rounded">
                <div>
                  <span className="text-slate-500">الجهة / الطرف المقابل: </span>
                  <b className="text-slate-900">{viewingVoucher.partyName}</b>
                </div>
                <div>
                  <span className="text-slate-500">حساب الخزينة/البنك: </span>
                  <span className="font-mono text-slate-700">({viewingVoucher.cashAccountCode}) {viewingVoucher.cashAccountName}</span>
                </div>
                <div>
                  <span className="text-slate-500">الحساب المقابل: </span>
                  <span className="font-mono text-slate-700">({viewingVoucher.contraAccountCode}) {viewingVoucher.contraAccountName}</span>
                </div>
                {viewingVoucher.costCenterName && (
                  <div>
                    <span className="text-slate-500">مركز التكلفة: </span>
                    <span className="text-indigo-700 font-semibold">{viewingVoucher.costCenterName}</span>
                  </div>
                )}
                <div>
                  <span className="text-slate-500">المرجع: </span>
                  <span>{viewingVoucher.reference || '-'}</span>
                </div>
                <div>
                  <span className="text-slate-500">البيان: </span>
                  <p className="p-1.5 bg-slate-100 rounded text-slate-800 mt-1">{viewingVoucher.description}</p>
                </div>
              </div>

              {viewingVoucher.status === 'VOID' && (
                <div className="p-2 bg-rose-50 border border-rose-300 rounded text-rose-800">
                  <div className="font-bold flex items-center gap-1">
                    <Ban className="w-3.5 h-3.5" />
                    <span>تم إلغاء هذا السند (VOID):</span>
                  </div>
                  <div className="text-[11px] mt-0.5">السبب: {viewingVoucher.voidReason || 'إلغاء محاسبي'}</div>
                </div>
              )}

              <div className="flex items-center justify-end gap-2 pt-2 border-t border-slate-200">
                <button
                  onClick={() => {
                    const v = viewingVoucher;
                    setViewingVoucher(null);
                    setPrintingVoucher(v);
                  }}
                  className="bg-sky-600 hover:bg-sky-500 text-white px-3 py-1.5 rounded font-bold flex items-center gap-1 cursor-pointer"
                >
                  <Printer className="w-3.5 h-3.5" />
                  <span>معاينة وطباعة السند</span>
                </button>
              </div>
            </div>
          </div>
        </div>
      );
};
