'use client';

import React, { useState } from 'react';
import { Building2 } from 'lucide-react';
import { Supplier } from '@/types/pos';

export interface AddSupplierModalProps {
  isOpen: boolean;
  currency: string;
  onClose: () => void;
  onSave: (supplierData: {
    name: string;
    company: string;
    phone: string;
    taxNumber: string;
    creditLimit: number;
    address: string;
    notes: string;
  }) => void;
}

export const AddSupplierModal: React.FC<AddSupplierModalProps> = ({
  isOpen,
  currency,
  onClose,
  onSave,
}) => {
  const [supName, setSupName] = useState('');
  const [supCompany, setSupCompany] = useState('');
  const [supPhone, setSupPhone] = useState('');
  const [supTaxNumber, setSupTaxNumber] = useState('');
  const [supCreditLimit, setSupCreditLimit] = useState(0);
  const [supAddress, setSupAddress] = useState('');
  const [supNotes, setSupNotes] = useState('');

  if (!isOpen) return null;

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!supName.trim() || !supCompany.trim()) return;
    onSave({
      name: supName.trim(),
      company: supCompany.trim(),
      phone: supPhone.trim(),
      taxNumber: supTaxNumber.trim(),
      creditLimit: supCreditLimit,
      address: supAddress.trim(),
      notes: supNotes.trim(),
    });
    // Reset form
    setSupName('');
    setSupCompany('');
    setSupPhone('');
    setSupTaxNumber('');
    setSupCreditLimit(0);
    setSupAddress('');
    setSupNotes('');
  };

  return (
    <div className="fixed inset-0 bg-slate-950/70 backdrop-blur-xs flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-lg overflow-hidden">
        <div className="bg-white border-b border-slate-200 px-5 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
              <Building2 className="w-4 h-4 text-sky-700" />
            </div>
            <h3 className="font-bold text-sm text-slate-900">إضافة بطاقة مورد معتمد جديد</h3>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="text-slate-400 hover:text-slate-700 p-1.5 rounded-lg hover:bg-slate-100 font-bold cursor-pointer transition"
          >
            ✕
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-4 flex flex-col gap-3 text-xs">
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-semibold text-slate-700 block mb-1">اسم المورد / المسؤول:</label>
              <input
                type="text"
                value={supName}
                onChange={(e) => setSupName(e.target.value)}
                placeholder="مثال: صالح اليافعي"
                className="w-full border border-slate-300 rounded px-2.5 py-1.5 font-bold text-slate-900 focus:outline-none focus:border-sky-600"
                required
              />
            </div>

            <div>
              <label className="font-semibold text-slate-700 block mb-1">الشركة / المؤسسة التجارية:</label>
              <input
                type="text"
                value={supCompany}
                onChange={(e) => setSupCompany(e.target.value)}
                placeholder="مثال: شركة المواد الغذائية المتحدة"
                className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900 focus:outline-none focus:border-sky-600"
                required
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-semibold text-slate-700 block mb-1">رقم الهاتف / الاتصال:</label>
              <input
                type="text"
                value={supPhone}
                onChange={(e) => setSupPhone(e.target.value)}
                placeholder="770000000"
                className="w-full border border-slate-300 rounded px-2.5 py-1.5 font-mono text-slate-900 focus:outline-none focus:border-sky-600"
              />
            </div>

            <div>
              <label className="font-semibold text-slate-700 block mb-1">الرقم الضريبي (إن وجد):</label>
              <input
                type="text"
                value={supTaxNumber}
                onChange={(e) => setSupTaxNumber(e.target.value)}
                placeholder="TAX-YE-..."
                className="w-full border border-slate-300 rounded px-2.5 py-1.5 font-mono text-slate-900 focus:outline-none focus:border-sky-600"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-semibold text-slate-700 block mb-1">الحد الائتماني المسموح به ({currency}):</label>
              <input
                type="number"
                value={supCreditLimit}
                onChange={(e) => setSupCreditLimit(Math.max(0, parseFloat(e.target.value) || 0))}
                className="w-full border border-slate-300 rounded px-2.5 py-1.5 font-mono-numbers text-slate-900 focus:outline-none focus:border-sky-600"
              />
            </div>

            <div>
              <label className="font-semibold text-slate-700 block mb-1">العنوان والفرع الرئيسي:</label>
              <input
                type="text"
                value={supAddress}
                onChange={(e) => setSupAddress(e.target.value)}
                placeholder="صنعاء - شارع الزبيري"
                className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900 focus:outline-none focus:border-sky-600"
              />
            </div>
          </div>

          <div>
            <label className="font-semibold text-slate-700 block mb-1">ملاحظات وشروط التوريد:</label>
            <input
              type="text"
              value={supNotes}
              onChange={(e) => setSupNotes(e.target.value)}
              placeholder="سداد آجل خلال 30 يوم..."
              className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900 focus:outline-none focus:border-sky-600"
            />
          </div>

          <div className="flex items-center justify-end gap-2 pt-3 border-t border-slate-200">
            <button
              type="button"
              onClick={onClose}
              className="bg-white border border-slate-300 text-slate-700 px-4 py-1.5 rounded cursor-pointer"
            >
              إلغاء
            </button>
            <button
              type="submit"
              className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-5 py-1.5 rounded cursor-pointer transition shadow-xs"
            >
              حفظ بطاقة المورد
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
