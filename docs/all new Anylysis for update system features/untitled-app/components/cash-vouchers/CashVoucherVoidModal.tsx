import React from 'react';
import { Trash2, AlertTriangle, X, Ban } from 'lucide-react';
import { CashVoucher } from '@/types/pos';

interface CashVoucherVoidModalProps {
  voidingVoucher: CashVoucher | null;
  setVoidingVoucher: (v: CashVoucher | null) => void;
  voidReasonInput: string;
  setVoidReasonInput: (v: string) => void;
  handleConfirmVoid: () => void;
  currency: string;
}

export const CashVoucherVoidModal: React.FC<CashVoucherVoidModalProps> = ({
  voidingVoucher, setVoidingVoucher, voidReasonInput, setVoidReasonInput, handleConfirmVoid, currency
}) => {
  if (!voidingVoucher) return null;
  return (
        <div className="fixed inset-0 bg-black/60 z-50 flex items-center justify-center p-3 backdrop-blur-xs">
          <div className="bg-white rounded-lg shadow-2xl border border-slate-400 w-full max-w-md overflow-hidden flex flex-col">
            <div className="bg-rose-900 text-white p-3 flex items-center justify-between">
              <div className="flex items-center gap-2 font-bold text-xs">
                <Ban className="w-4 h-4 text-rose-300" />
                <span>إلغاء السند المالي (Void Voucher)</span>
              </div>
              <button
                onClick={() => setVoidingVoucher(null)}
                className="text-white/80 hover:text-white p-1 hover:bg-white/10 rounded cursor-pointer"
              >
                <X className="w-4 h-4" />
              </button>
            </div>

            <div className="p-4 space-y-3 text-xs">
              <div className="p-2.5 bg-rose-50 border border-rose-200 rounded text-rose-900 space-y-1">
                <div className="font-bold">هل أنت متأكد من إلغاء السند: {voidingVoucher.voucherNo}؟</div>
                <div className="text-[11px] text-rose-700">
                  المبلغ: <b>{voidingVoucher.amount.toLocaleString()} {currency}</b> - {voidingVoucher.partyName}
                </div>
                <div className="text-[10px] text-rose-600">
                  تحذير: سيتم وسم السند كـ (ملغي VOID) مع إثبات عملية الإلغاء في سجل الرقابة المحاسبية.
                </div>
              </div>

              <div>
                <label className="block text-[11px] font-bold text-slate-700 mb-1">
                  سبب الإلغاء (إلزامي لأغراض التدقيق): <span className="text-rose-500">*</span>
                </label>
                <textarea
                  rows={2}
                  value={voidReasonInput}
                  onChange={(e) => setVoidReasonInput(e.target.value)}
                  placeholder="مثال: خطأ في المبلغ أو إلغاء الشيك أو استرجاع الدفعة..."
                  required
                  className="w-full p-2 bg-slate-50 border border-slate-300 rounded text-xs focus:bg-white focus:outline-none focus:border-rose-500"
                />
              </div>

              <div className="flex items-center justify-end gap-2 pt-2 border-t border-slate-200">
                <button
                  type="button"
                  onClick={() => setVoidingVoucher(null)}
                  className="px-3 py-1.5 border border-slate-300 rounded text-slate-700 hover:bg-slate-100 font-bold cursor-pointer"
                >
                  تراجع
                </button>
                <button
                  type="button"
                  onClick={handleConfirmVoid}
                  className="px-4 py-1.5 bg-rose-600 hover:bg-rose-700 text-white rounded font-bold cursor-pointer flex items-center gap-1"
                >
                  <Ban className="w-3.5 h-3.5" />
                  <span>تأكيد الإلغاء</span>
                </button>
              </div>
            </div>
          </div>
        </div>
      );
};
