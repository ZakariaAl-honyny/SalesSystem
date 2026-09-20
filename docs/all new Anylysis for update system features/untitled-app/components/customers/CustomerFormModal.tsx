'use client';

import React from 'react';
import { Customer } from '@/types/pos';
import { Users, X, CreditCard, CheckCircle2 } from 'lucide-react';

export interface CustomerFormModalProps {
  editingCustomer: Customer;
  customers: Customer[];
  setEditingCustomer: (c: Customer) => void;
  onClose: () => void;
  onSave: (e: React.FormEvent) => void;
}

export const CustomerFormModal: React.FC<CustomerFormModalProps> = ({
  editingCustomer,
  customers,
  setEditingCustomer,
  onClose,
  onSave,
}) => {
  const isEditing = customers.some((c) => c.id === editingCustomer.id);

  return (
    <div className="fixed inset-0 z-50 bg-black/60 backdrop-blur-xs flex items-center justify-center p-3 animate-fade-in">
      <div className="bg-white rounded-lg shadow-2xl border border-slate-300 w-full max-w-xl overflow-hidden flex flex-col">
        <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
              <Users className="w-4 h-4 text-sky-700" />
            </div>
            <div>
              <h3 className="font-bold text-sm text-slate-900 leading-none">
                {isEditing ? 'تعديل بيانات عميل' : 'إضافة بطاقة عميل جديد'}
              </h3>
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

        <form onSubmit={onSave} className="p-4 flex flex-col gap-3 text-xs overflow-y-auto max-h-[80vh]">
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-bold text-slate-700 block mb-1">كود الحساب (شجرة الحسابات):</label>
              <input
                type="text"
                required
                value={editingCustomer.code}
                onChange={(e) =>
                  setEditingCustomer({ ...editingCustomer, code: e.target.value, accountCode: e.target.value })
                }
                className="w-full bg-slate-50 border border-slate-300 rounded px-2.5 py-1.5 font-mono font-bold text-slate-800"
              />
            </div>

            <div>
              <label className="font-bold text-slate-700 block mb-1">اسم العميل أو المنشأة *:</label>
              <input
                type="text"
                required
                value={editingCustomer.name}
                onChange={(e) => setEditingCustomer({ ...editingCustomer, name: e.target.value })}
                placeholder="مثلاً: بقالة الخير والبركة"
                className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-bold text-slate-900"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-semibold text-slate-700 block mb-1">رقم الهاتف / الجوال:</label>
              <input
                type="text"
                value={editingCustomer.phone}
                onChange={(e) => setEditingCustomer({ ...editingCustomer, phone: e.target.value })}
                placeholder="770000000"
                className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-mono text-slate-800"
              />
            </div>

            <div>
              <label className="font-semibold text-slate-700 block mb-1">الشخص المسؤول / المندوب:</label>
              <input
                type="text"
                value={editingCustomer.contactPerson || ''}
                onChange={(e) => setEditingCustomer({ ...editingCustomer, contactPerson: e.target.value })}
                placeholder="اسم المشتري أو المدير"
                className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 text-slate-800"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-semibold text-slate-700 block mb-1">العنوان والموقع:</label>
              <input
                type="text"
                value={editingCustomer.address}
                onChange={(e) => setEditingCustomer({ ...editingCustomer, address: e.target.value })}
                placeholder="المدينة - الشارع - جوار..."
                className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 text-slate-800"
              />
            </div>

            <div>
              <label className="font-semibold text-slate-700 block mb-1">الرقم الضريبي (إن وجد):</label>
              <input
                type="text"
                value={editingCustomer.taxNumber || ''}
                onChange={(e) => setEditingCustomer({ ...editingCustomer, taxNumber: e.target.value })}
                placeholder="TAX-YE-..."
                className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-mono text-slate-800"
              />
            </div>
          </div>

          {/* Financial & Credit Limits */}
          <div className="p-3 bg-slate-50 border border-slate-200 rounded-lg flex flex-col gap-3">
            <span className="font-bold text-slate-800 text-xs flex items-center gap-1.5 text-sky-900">
              <CreditCard className="w-3.5 h-3.5" />
              <span>سياسة الائتمان وسقوف السحب والأسعار</span>
            </span>

            <div className="grid grid-cols-3 gap-3">
              <div>
                <label className="font-semibold text-slate-700 block mb-1">سقف الائتمان (ر.ي):</label>
                <input
                  type="number"
                  min={0}
                  value={editingCustomer.creditLimit}
                  onChange={(e) =>
                    setEditingCustomer({ ...editingCustomer, creditLimit: Math.max(0, Number(e.target.value)) })
                  }
                  className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-mono-numbers font-bold text-slate-900"
                />
              </div>

              <div>
                <label className="font-semibold text-slate-700 block mb-1">فترة الائتمان (أيام):</label>
                <input
                  type="number"
                  min={0}
                  value={editingCustomer.creditDays || 30}
                  onChange={(e) =>
                    setEditingCustomer({ ...editingCustomer, creditDays: Math.max(0, Number(e.target.value)) })
                  }
                  className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-mono font-bold text-slate-900"
                />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-3">
              <div>
                <label className="font-semibold text-slate-700 block mb-1">الرصيد الافتتاحي (مدين):</label>
                <input
                  type="number"
                  value={editingCustomer.openingBalance || 0}
                  onChange={(e) =>
                    setEditingCustomer({ ...editingCustomer, openingBalance: Number(e.target.value) })
                  }
                  className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-mono-numbers text-slate-900"
                />
              </div>

              <div>
                <label className="font-semibold text-slate-700 block mb-1">حالة العميل:</label>
                <select
                  value={editingCustomer.status || 'ACTIVE'}
                  onChange={(e) => setEditingCustomer({ ...editingCustomer, status: e.target.value as any })}
                  className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-bold text-slate-900"
                >
                  <option value="ACTIVE">نشط (مسموح بالبيع الآجل)</option>
                  <option value="BLOCKED">محظور (إيقاف البيع الآجل)</option>
                  <option value="SUSPENDED">معلق للمراجعة</option>
                </select>
              </div>
            </div>
          </div>

          <div>
            <label className="font-semibold text-slate-700 block mb-1">ملاحظات إضافية:</label>
            <textarea
              rows={2}
              value={editingCustomer.notes || ''}
              onChange={(e) => setEditingCustomer({ ...editingCustomer, notes: e.target.value })}
              placeholder="أي شروط أو اتفاقيات خاصة بالعميل..."
              className="w-full bg-white border border-slate-300 rounded p-2 text-slate-800"
            />
          </div>

          <div className="pt-2 border-t border-slate-200 flex items-center justify-end gap-2">
            <button
              type="button"
              onClick={onClose}
              className="bg-slate-200 hover:bg-slate-300 text-slate-800 font-bold px-3 py-1.5 rounded cursor-pointer"
            >
              إلغاء
            </button>
            <button
              type="submit"
              className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-4 py-1.5 rounded flex items-center gap-1.5 shadow-xs cursor-pointer"
            >
              <CheckCircle2 className="w-4 h-4" />
              <span>حفظ بطاقة العميل [F10]</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
