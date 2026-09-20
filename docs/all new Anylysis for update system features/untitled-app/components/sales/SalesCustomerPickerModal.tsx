'use client';

import React, { useState } from 'react';
import { Customer } from '@/types/pos';
import { Users, Search, UserCheck } from 'lucide-react';

export interface SalesCustomerPickerModalProps {
  customers: Customer[];
  selectedCustomerId: string;
  isCash: boolean;
  selectedCurrency: string;
  onSelectCustomer: (customerId: string, customerName: string) => void;
  onClose: () => void;
}

export function SalesCustomerPickerModal({
  customers,
  selectedCustomerId,
  isCash,
  selectedCurrency,
  onSelectCustomer,
  onClose,
}: SalesCustomerPickerModalProps) {
  const [searchQuery, setSearchQuery] = useState('');

  const filteredCustomers = customers.filter((c) => {
    if (!searchQuery.trim()) return true;
    const q = searchQuery.toLowerCase();
    return (
      c.name.toLowerCase().includes(q) ||
      (c.code && c.code.toLowerCase().includes(q)) ||
      (c.phone && c.phone.includes(q))
    );
  });

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-xs">
      <div className="bg-white rounded-xl max-w-lg w-full p-4 shadow-2xl border border-slate-200 text-right space-y-3 max-h-[85vh] flex flex-col">
        <div className="flex items-center justify-between border-b pb-2 shrink-0">
          <div className="flex items-center gap-2">
            <Users className="w-5 h-5 text-[#0277bd]" />
            <div>
              <h3 className="font-bold text-sm text-slate-800">اختيار حساب العميل (سريع)</h3>
              <p className="text-[11px] text-slate-500">اختر العميل لربطه بالفاتورة واحتساب الأرباح حتى لو كان الدفع نقداً</p>
            </div>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="text-slate-400 hover:text-slate-600 p-1 rounded-md cursor-pointer"
          >
            ✕
          </button>
        </div>

        {/* Search Input */}
        <div className="relative shrink-0">
          <input
            type="text"
            autoFocus
            placeholder="ابحث بالاسم، الكود، أو رقم الهاتف..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="w-full bg-slate-50 border border-slate-300 rounded-lg pr-9 pl-3 py-2 text-xs font-bold text-slate-800 focus:bg-white focus:border-[#0277bd] outline-hidden shadow-inner"
          />
          <Search className="w-4 h-4 text-slate-400 absolute right-3 top-2.5" />
        </div>

        {/* Quick Option: General Cash Customer */}
        <div className="shrink-0 flex gap-2">
          <button
            type="button"
            onClick={() => {
              onSelectCustomer('', isCash ? 'عميل نقدي عام' : 'عميل عام');
              onClose();
            }}
            className="flex-1 p-2 rounded-lg border border-dashed border-slate-300 hover:border-slate-400 bg-slate-50 hover:bg-slate-100 text-xs font-bold text-slate-700 flex items-center justify-center gap-1.5 transition cursor-pointer"
          >
            <UserCheck className="w-4 h-4 text-slate-500" />
            <span>عميل نقدي عام (بدون تحديد تاجر)</span>
          </button>
        </div>

        {/* Customer List */}
        <div className="flex-1 overflow-y-auto space-y-1.5 divide-y divide-slate-100 pr-1">
          {filteredCustomers.map((c) => {
            const isSelected = selectedCustomerId === c.id;
            return (
              <div
                key={c.id}
                onClick={() => {
                  onSelectCustomer(c.id, c.name);
                  onClose();
                }}
                className={`p-2.5 rounded-lg flex items-center justify-between cursor-pointer transition ${
                  isSelected
                    ? 'bg-sky-50 border border-sky-300 text-sky-900 font-bold'
                    : 'hover:bg-slate-50 border border-transparent'
                }`}
              >
                <div className="space-y-0.5">
                  <div className="flex items-center gap-2">
                    <span className="font-bold text-xs text-slate-900">{c.name}</span>
                    <span className="text-[10px] bg-slate-100 text-slate-600 px-1.5 py-0.2 rounded font-mono">{c.code}</span>
                  </div>
                  <div className="text-[11px] text-slate-500 flex items-center gap-3">
                    {c.phone && <span>الهاتف: <b className="font-mono">{c.phone}</b></span>}
                    {c.address && <span>العنوان: <b>{c.address}</b></span>}
                  </div>
                </div>

                <div className="text-left shrink-0">
                  {c.currentBalance > 0 ? (
                    <div className="bg-amber-50 border border-amber-200 rounded px-2 py-1 text-right">
                      <div className="text-[10px] text-amber-800 font-bold">عليه للمحل (مطلوب سداد):</div>
                      <div className="text-xs font-black text-amber-900 font-mono">
                        {c.currentBalance.toLocaleString()} {c.currency || selectedCurrency}
                      </div>
                      <div className="text-[9px] text-amber-600 font-normal">رصيد مدين</div>
                    </div>
                  ) : c.currentBalance < 0 ? (
                    <div className="bg-emerald-50 border border-emerald-200 rounded px-2 py-1 text-right">
                      <div className="text-[10px] text-emerald-800 font-bold">له دفعة سابقة (فائض):</div>
                      <div className="text-xs font-black text-emerald-900 font-mono">
                        {Math.abs(c.currentBalance).toLocaleString()} {c.currency || selectedCurrency}
                      </div>
                      <div className="text-[9px] text-emerald-600 font-normal">رصيد دائن</div>
                    </div>
                  ) : (
                    <div className="bg-slate-50 border border-slate-200 rounded px-2 py-1 text-right">
                      <div className="text-[10px] text-emerald-700 font-bold">حساب خالص</div>
                      <div className="text-[11px] text-slate-500 font-mono">لا توجد مديونية (0)</div>
                    </div>
                  )}
                </div>
              </div>
            );
          })}
        </div>

        <div className="border-t pt-2 shrink-0 flex items-center justify-between text-xs text-slate-500">
          <span>إجمالي العملاء: {customers.length}</span>
          <button
            type="button"
            onClick={onClose}
            className="px-4 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-md font-bold cursor-pointer"
          >
            إغلاق
          </button>
        </div>
      </div>
    </div>
  );
}
