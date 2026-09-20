'use client';

import React from 'react';

export interface PurchaseDiscountModalProps {
  isOpen: boolean;
  discountMode: 'AMOUNT' | 'PERCENT';
  setDiscountMode: (mode: 'AMOUNT' | 'PERCENT') => void;
  activeDiscountField: 'discount' | 'otherExpense' | 'paid';
  setActiveDiscountField: (field: any) => void;
  discountValue: string;
  taxPercent: number;
  setTaxPercent: (tax: number) => void;
  handleDiscountKeypadPress: (key: string) => void;
  onClose: () => void;
}

export const PurchaseDiscountModal: React.FC<PurchaseDiscountModalProps> = ({
  isOpen,
  discountMode,
  setDiscountMode,
  activeDiscountField,
  setActiveDiscountField,
  discountValue,
  taxPercent,
  setTaxPercent,
  handleDiscountKeypadPress,
  onClose,
}) => {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-3 bg-black/60 animate-in fade-in">
      <div className="bg-white rounded-2xl max-w-sm w-full p-4 shadow-2xl border border-slate-200 text-right space-y-3">
        <h3 className="font-bold text-base text-slate-900 border-b pb-2">
          الخصم والضريبة والمصاريف
        </h3>

        {/* Radio Options: مبلغ | نسبة */}
        <div className="flex items-center gap-6 text-xs font-bold text-slate-800">
          <label className="flex items-center gap-1.5 cursor-pointer">
            <input
              type="radio"
              name="discMode"
              checked={discountMode === 'AMOUNT'}
              onChange={() => setDiscountMode('AMOUNT')}
              className="w-4 h-4 text-[#0277bd]"
            />
            <span>مبلغ</span>
          </label>

          <label className="flex items-center gap-1.5 cursor-pointer">
            <input
              type="radio"
              name="discMode"
              checked={discountMode === 'PERCENT'}
              onChange={() => setDiscountMode('PERCENT')}
              className="w-4 h-4 text-[#0277bd]"
            />
            <span>نسبة</span>
          </label>
        </div>

        {/* Row 1: الخصم التجاري */}
        <div className="flex items-center justify-between gap-2">
          <span className="font-bold text-xs text-slate-700">الخصم التجاري</span>
          <div
            onClick={() => setActiveDiscountField('discount')}
            className={`w-36 border-b-2 py-1 px-2 font-mono font-bold text-sm text-left transition cursor-pointer ${
              activeDiscountField === 'discount' ? 'border-[#e91e63]' : 'border-slate-300'
            }`}
          >
            {discountValue}
          </div>
        </div>

        {/* Row 2: الضريبة % */}
        <div className="flex items-center justify-between gap-2">
          <span className="font-bold text-xs text-slate-700">الضريبة %</span>
          <select
            value={taxPercent}
            onChange={(e) => setTaxPercent(Number(e.target.value))}
            className="w-36 bg-slate-50 border border-slate-300 rounded px-2 py-1 font-mono font-bold text-sm text-center outline-none cursor-pointer"
          >
            <option value={0}>0 %</option>
            <option value={5}>5 %</option>
            <option value={15}>15 %</option>
          </select>
        </div>

        {/* Keypad Grid (3x4) for Discount */}
        <div className="grid grid-cols-3 gap-2 mt-4 dir-ltr">
          {['1', '2', '3'].map((n) => (
            <button
              key={n}
              type="button"
              onClick={() => handleDiscountKeypadPress(n)}
              className="py-2.5 bg-white hover:bg-sky-50 active:bg-sky-100 rounded-lg font-bold font-mono text-slate-800 text-base shadow-sm transition"
            >
              {n}
            </button>
          ))}
          {['4', '5', '6'].map((n) => (
            <button
              key={n}
              type="button"
              onClick={() => handleDiscountKeypadPress(n)}
              className="py-2.5 bg-white hover:bg-sky-50 active:bg-sky-100 rounded-lg font-bold font-mono text-slate-800 text-base shadow-sm transition"
            >
              {n}
            </button>
          ))}
          {['7', '8', '9'].map((n) => (
            <button
              key={n}
              type="button"
              onClick={() => handleDiscountKeypadPress(n)}
              className="py-2.5 bg-white hover:bg-sky-50 active:bg-sky-100 rounded-lg font-bold font-mono text-slate-800 text-base shadow-sm transition"
            >
              {n}
            </button>
          ))}
          <button
            type="button"
            onClick={() => handleDiscountKeypadPress('.')}
            className="py-2.5 bg-white hover:bg-sky-50 active:bg-sky-100 rounded-lg font-bold font-mono text-slate-800 text-base shadow-sm transition"
          >
            .
          </button>
          <button
            type="button"
            onClick={() => handleDiscountKeypadPress('0')}
            className="py-2.5 bg-white hover:bg-sky-50 active:bg-sky-100 rounded-lg font-bold font-mono text-slate-800 text-base shadow-sm transition"
          >
            0
          </button>
          <button
            type="button"
            onClick={() => handleDiscountKeypadPress('CLEAR')}
            className="py-2.5 bg-slate-100 hover:bg-slate-200 rounded-lg font-bold text-slate-600 text-xs shadow-sm transition"
          >
            C
          </button>
        </div>

        <div className="pt-3 border-t mt-3 flex justify-end">
          <button
            type="button"
            onClick={onClose}
            className="bg-[#0277bd] text-white hover:bg-[#01579b] font-bold py-1.5 px-6 rounded-lg text-xs transition shadow-sm cursor-pointer"
          >
            تم
          </button>
        </div>
      </div>
    </div>
  );
};
