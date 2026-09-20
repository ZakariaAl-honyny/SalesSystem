import React from 'react';
import { CostCenterNode } from '@/types/pos';
import { Building2, X } from 'lucide-react';

interface AccountingAddCostCenterModalProps {
  isOpen: boolean;
  onClose: () => void;
  handleSaveNewCostCenter: (e: React.FormEvent) => void;
  addCostCenterParentId: string;
  allCostCentersFlat: CostCenterNode[];
  costCenterForm: any;
  setCostCenterForm: React.Dispatch<React.SetStateAction<any>>;
  currency: string;
}

export const AccountingAddCostCenterModal: React.FC<AccountingAddCostCenterModalProps> = ({
  isOpen,
  onClose,
  handleSaveNewCostCenter,
  addCostCenterParentId,
  allCostCentersFlat,
  costCenterForm,
  setCostCenterForm,
  currency,
}) => {
  if (!isOpen) return null;
  return (
        <div className="fixed inset-0 bg-slate-950/60 backdrop-blur-xs flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-lg overflow-hidden animate-in fade-in zoom-in-95 duration-150">
            <div className="bg-white border-b border-slate-200 px-5 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
              <div className="flex items-center gap-2.5">
                <div className="w-8 h-8 rounded-lg bg-indigo-50 border border-indigo-200 flex items-center justify-center text-indigo-700 shadow-xs">
                  <Building2 className="w-4 h-4 text-indigo-700" />
                </div>
                <h3 className="font-bold text-sm text-slate-900">إضافة مركز تكلفة جديد</h3>
              </div>
              <button
                onClick={onClose}
                className="text-slate-400 hover:text-slate-700 p-1.5 rounded-lg hover:bg-slate-100 cursor-pointer transition"
              >
                <X className="w-4 h-4" />
              </button>
            </div>

            <form onSubmit={handleSaveNewCostCenter} className="p-4 flex flex-col gap-3 text-xs">
              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="font-semibold text-slate-700 block mb-1">رمز مركز التكلفة:</label>
                  <input
                    type="text"
                    value={costCenterForm.code}
                    onChange={(e) => setCostCenterForm({ ...costCenterForm, code: e.target.value })}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 font-mono font-bold text-slate-900"
                    required
                  />
                </div>
                <div>
                  <label className="font-semibold text-slate-700 block mb-1">الموازنة التقديرية ({currency}):</label>
                  <input
                    type="number"
                    min={0}
                    value={costCenterForm.budget}
                    onChange={(e) => setCostCenterForm({ ...costCenterForm, budget: parseFloat(e.target.value) || 0 })}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 font-mono-numbers font-bold text-slate-900"
                  />
                </div>
              </div>

              <div>
                <label className="font-semibold text-slate-700 block mb-1">اسم مركز التكلفة (عربي):</label>
                <input
                  type="text"
                  value={costCenterForm.name}
                  onChange={(e) => setCostCenterForm({ ...costCenterForm, name: e.target.value })}
                  placeholder="مثال: فرع المنصورة، قسم التسويق والمبيعات..."
                  className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900"
                  required
                />
              </div>

              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="font-semibold text-slate-700 block mb-1">الاسم بالإنجليزية:</label>
                  <input
                    type="text"
                    value={costCenterForm.nameEn}
                    onChange={(e) => setCostCenterForm({ ...costCenterForm, nameEn: e.target.value })}
                    placeholder="e.g. Marketing Dept"
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900"
                  />
                </div>
                <div>
                  <label className="font-semibold text-slate-700 block mb-1">اسم المدير / المشرف:</label>
                  <input
                    type="text"
                    value={costCenterForm.managerName}
                    onChange={(e) => setCostCenterForm({ ...costCenterForm, managerName: e.target.value })}
                    placeholder="اسم المسؤول..."
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900"
                  />
                </div>
              </div>

              <div className="flex items-center justify-end gap-2 pt-3 border-t border-slate-200">
                <button
                  type="button"
                  onClick={onClose}
                  className="bg-white border border-slate-300 text-slate-700 px-3 py-1.5 rounded cursor-pointer"
                >
                  إلغاء
                </button>
                <button
                  type="submit"
                  className="bg-indigo-700 hover:bg-indigo-600 text-white font-bold px-5 py-1.5 rounded cursor-pointer"
                >
                  حفظ المركز
                </button>
              </div>
            </form>
          </div>
        </div>
  );
};
