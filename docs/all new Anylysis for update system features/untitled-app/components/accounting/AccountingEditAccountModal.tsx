import React from 'react';
import { Edit3, X } from 'lucide-react';

interface AccountingEditAccountModalProps {
  isOpen: boolean;
  onClose: () => void;
  handleSaveEditAccount: (e: React.FormEvent) => void;
  accountForm: any;
  setAccountForm: React.Dispatch<React.SetStateAction<any>>;
}

export const AccountingEditAccountModal: React.FC<AccountingEditAccountModalProps> = ({
  isOpen,
  onClose,
  handleSaveEditAccount,
  accountForm,
  setAccountForm,
}) => {
  if (!isOpen) return null;
  return (
        <div className="fixed inset-0 bg-slate-950/60 backdrop-blur-xs flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-lg overflow-hidden animate-in fade-in zoom-in-95 duration-150">
            <div className="bg-white border-b border-slate-200 px-5 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
              <div className="flex items-center gap-2.5">
                <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
                  <Edit3 className="w-4 h-4 text-sky-700" />
                </div>
                <h3 className="font-bold text-sm text-slate-900">تعديل بيانات الحساب [{accountForm.code}]</h3>
              </div>
              <button
                onClick={onClose}
                className="text-slate-400 hover:text-slate-700 p-1.5 rounded-lg hover:bg-slate-100 cursor-pointer transition"
              >
                <X className="w-4 h-4" />
              </button>
            </div>

            <form onSubmit={handleSaveEditAccount} className="p-4 flex flex-col gap-3 text-xs">
              <div>
                <label className="font-semibold text-slate-700 block mb-1">اسم الحساب (عربي):</label>
                <input
                  type="text"
                  value={accountForm.name}
                  onChange={(e) => setAccountForm({ ...accountForm, name: e.target.value })}
                  className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900"
                  required
                />
              </div>

              <div>
                <label className="font-semibold text-slate-700 block mb-1">اسم الحساب (إنجليزي):</label>
                <input
                  type="text"
                  value={accountForm.nameEn}
                  onChange={(e) => setAccountForm({ ...accountForm, nameEn: e.target.value })}
                  className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900"
                />
              </div>

              <div className="bg-slate-50 border border-slate-200 rounded p-2.5 flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <span className="text-slate-500 font-semibold">العملة:</span>
                  <span className="font-bold text-slate-800 bg-white border border-slate-300 px-2 py-0.5 rounded">
                    ريال يمني (عملة موحدة)
                  </span>
                </div>

                <div className="flex items-center gap-2">
                  <input
                    type="checkbox"
                    id="chkAllowCCEdit"
                    checked={accountForm.allowCostCenter}
                    onChange={(e) => setAccountForm({ ...accountForm, allowCostCenter: e.target.checked })}
                    className="w-4 h-4 rounded text-sky-600 cursor-pointer"
                  />
                  <label htmlFor="chkAllowCCEdit" className="font-semibold text-slate-700 cursor-pointer">
                    يقبل الربط بمراكز التكلفة
                  </label>
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
                  className="bg-sky-700 hover:bg-sky-600 text-white font-bold px-5 py-1.5 rounded cursor-pointer"
                >
                  حفظ التعديلات
                </button>
              </div>
            </form>
          </div>
        </div>
  );
};
