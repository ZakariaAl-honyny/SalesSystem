'use client';

import React from 'react';
import { Percent, X } from 'lucide-react';
import { PaymentType } from '@/types/pos';

export interface SalesDiscountExpensesModalProps {
  headerDiscountType: 'PERCENT' | 'AMOUNT';
  setHeaderDiscountType: (type: 'PERCENT' | 'AMOUNT') => void;
  headerDiscountVal: string;
  setHeaderDiscountVal: (val: string) => void;
  taxPercentVal: string;
  setTaxPercentVal: (val: string) => void;
  setIsTaxEnabled: (enabled: boolean) => void;
  otherChargesAccountCode: string;
  setOtherChargesAccountCode: (code: string) => void;
  setOtherChargesName: (name: string) => void;
  expenseAccountsOptions: { code: string; name: string }[];
  otherChargesVal: string;
  setOtherChargesVal: (val: string) => void;
  paymentType: PaymentType;
  splitCash: number;
  setSplitCash: (val: number) => void;
  splitNetwork: number;
  setSplitNetwork: (val: number) => void;
  paidAmountVal: string;
  setPaidAmountVal: (val: string) => void;
  onClose: () => void;
}

export function SalesDiscountExpensesModal({
  headerDiscountType,
  setHeaderDiscountType,
  headerDiscountVal,
  setHeaderDiscountVal,
  taxPercentVal,
  setTaxPercentVal,
  setIsTaxEnabled,
  otherChargesAccountCode,
  setOtherChargesAccountCode,
  setOtherChargesName,
  expenseAccountsOptions,
  otherChargesVal,
  setOtherChargesVal,
  paymentType,
  splitCash,
  setSplitCash,
  splitNetwork,
  setSplitNetwork,
  paidAmountVal,
  setPaidAmountVal,
  onClose,
}: SalesDiscountExpensesModalProps) {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-2 bg-black/60">
      <div className="bg-white rounded-xl max-w-sm w-full shadow-2xl border border-slate-200 overflow-hidden text-right flex flex-col max-h-[95vh]">
        <div className="bg-white border-b border-slate-200 px-4 py-3 flex items-center justify-between shadow-2xs text-slate-800">
          <div className="flex items-center gap-2">
            <div className="w-7 h-7 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
              <Percent className="w-3.5 h-3.5 text-sky-700" />
            </div>
            <h3 className="font-bold text-sm text-slate-900">الخصم والمصاريف والدفع</h3>
          </div>
          <button onClick={onClose} className="text-slate-400 hover:text-slate-700 p-1 hover:bg-slate-100 rounded-lg cursor-pointer transition">
            <X className="w-4 h-4" />
          </button>
        </div>

        <div className="p-3 space-y-2.5 overflow-y-auto flex-1 text-xs">
          <div className="flex items-center justify-center gap-6 pb-1 border-b border-slate-200 font-bold text-slate-700">
            <label className="flex items-center gap-1.5 cursor-pointer">
              <input
                type="radio"
                name="modalDiscRadio"
                checked={headerDiscountType === 'AMOUNT'}
                onChange={() => setHeaderDiscountType('AMOUNT')}
              />
              <span>مبلغ</span>
            </label>
            <label className="flex items-center gap-1.5 cursor-pointer">
              <input
                type="radio"
                name="modalDiscRadio"
                checked={headerDiscountType === 'PERCENT'}
                onChange={() => setHeaderDiscountType('PERCENT')}
              />
              <span>نسبة</span>
            </label>
          </div>

          <div className="space-y-2">
            <div className="flex items-center justify-between gap-2">
              <span className="font-bold text-slate-700 w-24">الخصم التجاري</span>
              <input
                type="text"
                value={headerDiscountVal}
                onChange={(e) => setHeaderDiscountVal(e.target.value)}
                className="flex-1 text-center py-1.5 font-bold font-mono rounded border border-slate-300 bg-slate-50"
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
                className="flex-1 text-center py-1.5 font-bold font-mono rounded border border-slate-300 bg-slate-50"
              >
                <option value="0">0</option>
                <option value="5">5</option>
                <option value="15">15</option>
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
                    const match = expenseAccountsOptions.find((a) => a.code === code);
                    if (match) setOtherChargesName(match.name);
                  }}
                  className="flex-1 text-right py-1.5 px-2 font-bold rounded border border-slate-300 bg-slate-50 text-xs"
                >
                  {expenseAccountsOptions.map((acc) => (
                    <option key={acc.code} value={acc.code}>
                      {acc.code} - {acc.name}
                    </option>
                  ))}
                </select>
                <input
                  type="number"
                  min={0}
                  value={otherChargesVal}
                  onChange={(e) => setOtherChargesVal(e.target.value)}
                  placeholder="0"
                  className="w-20 text-center py-1.5 font-bold font-mono rounded border border-slate-300 bg-slate-50"
                />
              </div>
            </div>

            {paymentType === 'MIXED' ? (
              <div className="flex flex-col gap-2 pt-1 border-t border-slate-200">
                <div className="flex items-center justify-between gap-2">
                  <span className="font-bold text-slate-800 w-24">المدفوع نقد</span>
                  <input
                    type="number"
                    value={splitCash || ''}
                    onChange={(e) => setSplitCash(parseFloat(e.target.value) || 0)}
                    className="flex-1 text-center py-1.5 font-bold font-mono rounded border border-emerald-500 bg-emerald-50 text-emerald-700 outline-hidden"
                  />
                </div>
                <div className="flex items-center justify-between gap-2">
                  <span className="font-bold text-slate-800 w-24">المدفوع بنك</span>
                  <input
                    type="number"
                    value={splitNetwork || ''}
                    onChange={(e) => setSplitNetwork(parseFloat(e.target.value) || 0)}
                    className="flex-1 text-center py-1.5 font-bold font-mono rounded border border-sky-500 bg-sky-50 text-sky-700 outline-hidden"
                  />
                </div>
              </div>
            ) : (
              paymentType !== 'CASH' && (
                <div className="flex items-center justify-between gap-2 pt-1 border-t border-slate-200">
                  <span className="font-bold text-slate-800 w-24">المبلغ المدفوع</span>
                  <input
                    type="text"
                    value={paidAmountVal}
                    onChange={(e) => setPaidAmountVal(e.target.value)}
                    className="flex-1 text-center py-1.5 font-bold font-mono rounded border-2 border-pink-500 bg-pink-50 text-pink-700"
                  />
                </div>
              )
            )}
          </div>

          <div className="flex items-center gap-2 pt-2 border-t border-slate-200">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 bg-[#0277bd] text-white py-2 rounded-lg font-bold text-xs cursor-pointer hover:bg-[#0266a3]"
            >
              موافق
            </button>
            <button
              type="button"
              onClick={onClose}
              className="flex-1 bg-slate-100 text-slate-700 py-2 rounded-lg font-bold text-xs cursor-pointer hover:bg-slate-200"
            >
              إلغاء
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
