import React from 'react';
import { Tag } from 'lucide-react';

interface POSDiscountModalProps {
  showDiscountModal: boolean;
  setShowDiscountModal: (v: boolean) => void;
  discountType: 'AMOUNT' | 'PERCENT';
  setDiscountType: (v: 'AMOUNT' | 'PERCENT') => void;
  discountOverall: number;
  setDiscountOverall: (v: number) => void;
  taxPercentVal: string;
  setTaxPercentVal: (v: string) => void;
  setIsTaxEnabled: (v: boolean) => void;
  otherChargesAccountCode: string;
  setOtherChargesAccountCode: (v: string) => void;
  setOtherChargesName: (v: string) => void;
  EXPENSE_ACCOUNTS_OPTIONS: any[];
  otherChargesVal: string;
  setOtherChargesVal: (v: string) => void;
  subtotal: number;
  totalDiscount: number;
  calculatedTax: number;
  otherCharges: number;
  otherChargesName: string;
  grandTotal: number;
  currency: string;
}

export const POSDiscountModal: React.FC<POSDiscountModalProps> = ({
  showDiscountModal, setShowDiscountModal, discountType, setDiscountType, discountOverall, setDiscountOverall,
  taxPercentVal, setTaxPercentVal, setIsTaxEnabled, otherChargesAccountCode, setOtherChargesAccountCode,
  setOtherChargesName, EXPENSE_ACCOUNTS_OPTIONS, otherChargesVal, setOtherChargesVal, subtotal, totalDiscount,
  calculatedTax, otherCharges, otherChargesName, grandTotal, currency
}) => {
  if (!showDiscountModal) return null;
  return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-xs">
          <div className="bg-white rounded-xl max-w-sm w-full p-4 shadow-2xl border border-slate-200 text-right space-y-3 animate-in fade-in zoom-in-95 duration-150">
            <div className="flex items-center justify-between border-b pb-2">
              <h3 className="font-bold text-sm text-slate-800 flex items-center gap-1.5">
                <Tag className="w-4 h-4 text-[#0277bd]" />
                <span>الخصم والمصاريف والدفع [F7]</span>
              </h3>
              <button
                type="button"
                onClick={() => setShowDiscountModal(false)}
                className="text-slate-400 hover:text-slate-600 p-1 rounded-md cursor-pointer"
              >
                ✕
              </button>
            </div>

            <div className="flex items-center justify-center gap-6 text-xs font-bold text-slate-800 py-1 bg-slate-50 rounded-lg border border-slate-200">
              <label className="flex items-center gap-1.5 cursor-pointer">
                <input
                  type="radio"
                  name="posDiscRadio"
                  checked={discountType === 'AMOUNT'}
                  onChange={() => setDiscountType('AMOUNT')}
                  className="w-4 h-4 text-[#0277bd]"
                />
                <span>مبلغ</span>
              </label>
              <label className="flex items-center gap-1.5 cursor-pointer">
                <input
                  type="radio"
                  name="posDiscRadio"
                  checked={discountType === 'PERCENT'}
                  onChange={() => setDiscountType('PERCENT')}
                  className="w-4 h-4 text-[#0277bd]"
                />
                <span>نسبة</span>
              </label>
            </div>

            <div className="space-y-2 text-xs">
              <div className="flex items-center justify-between gap-2">
                <span className="font-bold text-slate-700 w-24">الخصم التجاري</span>
                <input
                  type="number"
                  min={0}
                  value={discountOverall || ''}
                  onChange={(e) => setDiscountOverall(parseFloat(e.target.value) || 0)}
                  placeholder="0"
                  className="flex-1 text-center py-1.5 font-bold font-mono rounded border border-slate-300 bg-slate-50 focus:bg-white focus:border-[#0277bd] outline-none"
                />
              </div>

              <div className="flex items-center justify-between gap-2">
                <span className="font-bold text-slate-700 w-24">الضريبة%</span>
                <select
                  value={taxPercentVal}
                  onChange={(e) => {
                    setTaxPercentVal(e.target.value);
                    setIsTaxEnabled(parseFloat(e.target.value) > 0);
                  }}
                  className="flex-1 text-center py-1.5 font-bold font-mono rounded border border-slate-300 bg-slate-50 focus:bg-white focus:border-[#0277bd] outline-none cursor-pointer"
                >
                  <option value="0">0 % (معفي من الضريبة)</option>
                  <option value="5">5 % (ضريبة مبيعات عامة)</option>
                  <option value="15">15 % (ضريبة القيمة المضافة)</option>
                </select>
              </div>

              <div className="flex items-center justify-between gap-2">
                <span className="font-bold text-slate-700 w-24">حساب المصروف</span>
                <div className="flex-1 flex items-center gap-1">
                  <select
                    value={otherChargesAccountCode}
                    onChange={(e) => {
                      const code = e.target.value;
                      setOtherChargesAccountCode(code);
                      const match = EXPENSE_ACCOUNTS_OPTIONS.find((a: any) => a.code === code);
                      if (match) setOtherChargesName(match.name);
                    }}
                    className="flex-1 text-right py-1.5 px-2 font-bold rounded border border-slate-300 bg-slate-50 focus:bg-white focus:border-[#0277bd] outline-none text-xs cursor-pointer"
                  >
                    {EXPENSE_ACCOUNTS_OPTIONS.map((acc: any) => (
                      <option key={acc.code} value={acc.code}>
                        {acc.code} - {acc.name}
                      </option>
                    ))}
                  </select>
                  <input
                    type="number"
                    min={0}
                    value={otherChargesVal || ''}
                    onChange={(e) => setOtherChargesVal(e.target.value)}
                    placeholder="0"
                    className="w-20 text-center py-1.5 font-bold font-mono rounded border border-slate-300 bg-slate-50 focus:bg-white focus:border-[#0277bd] outline-none text-xs"
                  />
                </div>
              </div>

              <div className="bg-slate-50 p-2.5 rounded-lg border border-slate-200 space-y-1 text-[11px]">
                <div className="flex items-center justify-between text-slate-600">
                  <span>المجموع الفرعي:</span>
                  <span className="font-mono font-bold">{subtotal.toLocaleString()} {currency}</span>
                </div>
                {totalDiscount > 0 && (
                  <div className="flex items-center justify-between text-rose-600 font-semibold">
                    <span>إجمالي الخصم:</span>
                    <span className="font-mono font-bold">-{totalDiscount.toLocaleString()} {currency}</span>
                  </div>
                )}
                {calculatedTax > 0 && (
                  <div className="flex items-center justify-between text-emerald-700 font-semibold">
                    <span>مبلغ الضريبة ({taxPercentVal}%):</span>
                    <span className="font-mono font-bold">+{calculatedTax.toLocaleString()} {currency}</span>
                  </div>
                )}
                {otherCharges > 0 && (
                  <div className="flex items-center justify-between text-sky-700 font-semibold">
                    <span>{otherChargesName || 'رسوم إضافية'}:</span>
                    <span className="font-mono font-bold">+{otherCharges.toLocaleString()} {currency}</span>
                  </div>
                )}
                <div className="flex items-center justify-between pt-1 border-t border-slate-200 text-xs font-black text-slate-900">
                  <span>الصافي الإجمالي:</span>
                  <span className="font-mono text-emerald-700 font-black text-sm">{grandTotal.toLocaleString()} {currency}</span>
                </div>
              </div>

              <div className="flex items-center gap-2 pt-2 border-t border-slate-200">
                <button
                  type="button"
                  onClick={() => setShowDiscountModal(false)}
                  className="flex-1 bg-[#0277bd] hover:bg-[#01579b] text-white py-2 rounded-lg font-bold text-xs shadow-sm transition cursor-pointer"
                >
                  موافق
                </button>
                <button
                  type="button"
                  onClick={() => setShowDiscountModal(false)}
                  className="flex-1 bg-slate-100 hover:bg-slate-200 text-slate-700 py-2 rounded-lg font-bold text-xs transition cursor-pointer"
                >
                  إلغاء
                </button>
              </div>
            </div>
          </div>
        </div>
  );
};
