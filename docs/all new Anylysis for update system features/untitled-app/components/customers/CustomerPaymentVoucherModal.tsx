'use client';

import React, { useState, useMemo } from 'react';
import { Customer, AccountNode, CostCenterNode, SystemUser, CashVoucher } from '@/types/pos';
import { generateNextId, getCurrentDateString, getCurrentTimeString } from '@/lib/idGenerator';
import { tafqeet } from '@/lib/tafqeet';
import { ArrowUpRight, X } from 'lucide-react';

export interface CustomerPaymentVoucherModalProps {
  customer: Customer;
  customers: Customer[];
  chartOfAccounts?: AccountNode[];
  costCenters?: CostCenterNode[];
  currentUser: SystemUser | null;
  onClose: () => void;
  onSave: (voucher: CashVoucher) => void;
}

export const CustomerPaymentVoucherModal: React.FC<CustomerPaymentVoucherModalProps> = ({
  customer,
  customers,
  currentUser,
  onClose,
  onSave,
}) => {
  const [selectedCustId, setSelectedCustId] = useState(customer.id);
  const currentCust = useMemo(() => customers.find((c) => c.id === selectedCustId) || customer, [customers, selectedCustId, customer]);

  const [voucherNo] = useState(() => `${Math.floor(2001 + Math.random() * 900)}`);
  const [voucherDate, setVoucherDate] = useState(getCurrentDateString());
  const [amount, setAmount] = useState<number>(20000);
  const [cashAccountCode, setCashAccountCode] = useState<'1111' | '1121'>('1111');
  const [description, setDescription] = useState(`رد مبلغ تأمين أو رصيد دائن للعميل ${currentCust.name}`);

  const handleSave = (e: React.FormEvent) => {
    e.preventDefault();
    if (amount <= 0) return;

    const newVoucher: CashVoucher = {
      id: generateNextId('VOUCH'),
      voucherNo,
      type: 'PAYMENT',
      paymentMethod: 'CASH',
      date: voucherDate,
      amount,
      currency: 'ريال يمني',
      cashAccountCode,
      cashAccountName: cashAccountCode === '1111' ? 'الصندوق الرئيسي (الخزينة المركزية)' : 'بنك الكريمي',
      partyType: 'CUSTOMER',
      partyId: currentCust.id,
      partyName: currentCust.name,
      contraAccountCode: currentCust.accountCode,
      contraAccountName: `حساب العميل: ${currentCust.name}`,
      description,
      status: 'POSTED',
      createdBy: currentUser?.userName || 'accountant',
      createdAt: `${voucherDate} ${getCurrentTimeString()}`,
    };

    onSave(newVoucher);
  };

  return (
    <div className="fixed inset-0 z-50 bg-black/65 backdrop-blur-xs flex items-center justify-center p-3 animate-fade-in">
      <div className="bg-white rounded-lg shadow-2xl border border-slate-300 w-full max-w-lg overflow-hidden flex flex-col">
        <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-amber-50 border border-amber-200 flex items-center justify-center text-amber-700 shadow-xs">
              <ArrowUpRight className="w-4 h-4 text-amber-700" />
            </div>
            <div>
              <h3 className="font-bold text-sm text-slate-900 leading-none">سند صرف / رد مبالغ للعميل (Payment Voucher)</h3>
            </div>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg text-sm font-bold cursor-pointer transition"
          >
            <X className="w-4 h-4" />
          </button>
        </div>

        <form onSubmit={handleSave} className="p-4 flex flex-col gap-3 text-xs">
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-bold text-slate-700 block mb-1">رقم السند:</label>
              <input type="text" readOnly value={voucherNo} className="w-full bg-slate-100 border border-slate-300 rounded px-2 py-1 font-mono font-bold" />
            </div>
            <div>
              <label className="font-bold text-slate-700 block mb-1">التاريخ:</label>
              <input type="date" value={voucherDate} onChange={(e) => setVoucherDate(e.target.value)} className="w-full bg-white border border-slate-300 rounded px-2 py-1" />
            </div>
          </div>

          <div>
            <label className="font-bold text-slate-700 block mb-1">العميل المصروف له:</label>
            <select
              value={selectedCustId}
              onChange={(e) => setSelectedCustId(e.target.value)}
              className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-bold"
            >
              {customers.map((c) => (
                <option key={c.id} value={c.id}>[{c.code}] {c.name}</option>
              ))}
            </select>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-bold text-slate-700 block mb-1">المبلغ المصروف (ر.ي):</label>
              <input
                type="number"
                min={1}
                required
                value={amount}
                onChange={(e) => setAmount(Number(e.target.value))}
                className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-mono-numbers font-bold text-base"
              />
            </div>
            <div>
              <label className="font-bold text-slate-700 block mb-1">الصرف من خزينة / بنك:</label>
              <select
                value={cashAccountCode}
                onChange={(e) => setCashAccountCode(e.target.value as any)}
                className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-bold"
              >
                <option value="1111">[1111] الصندوق الرئيسي</option>
                <option value="1121">[1121] بنك الكريمي</option>
              </select>
            </div>
          </div>

          <div className="text-[11px] text-amber-900 bg-amber-50 p-2 rounded border border-amber-200">
            <span>المبلغ كتابةً: </span>
            <span className="font-bold">{tafqeet(amount)}</span>
          </div>

          <div>
            <label className="font-semibold text-slate-700 block mb-1">البيان والشرح:</label>
            <input
              type="text"
              required
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5"
            />
          </div>

          <div className="pt-2 border-t border-slate-200 flex items-center justify-end gap-2">
            <button type="button" onClick={onClose} className="bg-slate-200 hover:bg-slate-300 text-slate-800 font-bold px-3 py-1.5 rounded cursor-pointer">
              إلغاء
            </button>
            <button type="submit" className="bg-amber-600 hover:bg-amber-500 text-white font-bold px-4 py-1.5 rounded shadow-xs cursor-pointer">
              حفظ وترحيل سند الصرف
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
