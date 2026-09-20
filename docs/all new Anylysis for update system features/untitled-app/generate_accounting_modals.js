const fs = require('fs');

const createModalFile = (name, content) => {
  fs.writeFileSync(`components/accounting/${name}.tsx`, content);
};

createModalFile('AccountingAddAccountModal', `import React from 'react';
import { AccountNode } from '@/types/pos';
import { Folder, X } from 'lucide-react';

interface AccountingAddAccountModalProps {
  isOpen: boolean;
  onClose: () => void;
  handleSaveNewAccount: (e: React.FormEvent) => void;
  addAccountParentCode: string;
  allAccountsFlat: AccountNode[];
  accountForm: any;
  setAccountForm: React.Dispatch<React.SetStateAction<any>>;
}

export const AccountingAddAccountModal: React.FC<AccountingAddAccountModalProps> = ({
  isOpen,
  onClose,
  handleSaveNewAccount,
  addAccountParentCode,
  allAccountsFlat,
  accountForm,
  setAccountForm,
}) => {
  if (!isOpen) return null;
  return (
        <div className="fixed inset-0 bg-slate-950/60 backdrop-blur-xs flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-lg overflow-hidden animate-in fade-in zoom-in-95 duration-150">
            <div className="bg-[#0f2d4a] text-white px-4 py-3 flex items-center justify-between">
              <div className="flex items-center gap-2">
                <Folder className="w-4 h-4 text-amber-300" />
                <h3 className="font-bold text-sm">إضافة حساب جديد في شجرة الحسابات</h3>
              </div>
              <button
                onClick={onClose}
                className="text-slate-300 hover:text-white cursor-pointer"
              >
                <X className="w-4 h-4" />
              </button>
            </div>

            <form onSubmit={handleSaveNewAccount} className="p-4 flex flex-col gap-3 text-xs">
              <div className="p-2 bg-sky-50 rounded border border-sky-200 text-sky-900">
                <span className="font-semibold block">الحساب الأب المحدد:</span>
                <span className="font-mono font-bold">[{addAccountParentCode}]</span>{' '}
                <span>{allAccountsFlat.find((a) => a.code === addAccountParentCode)?.name}</span>
              </div>

              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="font-semibold text-slate-700 block mb-1">رقم / كود الحساب المقترح:</label>
                  <input
                    type="text"
                    value={accountForm.code}
                    onChange={(e) => setAccountForm({ ...accountForm, code: e.target.value })}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 font-mono font-bold text-slate-900"
                    required
                  />
                </div>
                <div>
                  <label className="font-semibold text-slate-700 block mb-1">طبيعة الحساب المعتمدة:</label>
                  <select
                    value={accountForm.nature}
                    onChange={(e) => setAccountForm({ ...accountForm, nature: e.target.value as any })}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 font-semibold text-slate-800"
                  >
                    <option value="DEBIT">مدين (Debit)</option>
                    <option value="CREDIT">دائن (Credit)</option>
                  </select>
                </div>
              </div>

              <div>
                <label className="font-semibold text-slate-700 block mb-1">اسم الحساب (عربي):</label>
                <input
                  type="text"
                  value={accountForm.name}
                  onChange={(e) => setAccountForm({ ...accountForm, name: e.target.value })}
                  placeholder="مثال: صندوق فرع المعلا، مصروفات قرطاسية..."
                  className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900"
                  required
                />
              </div>

              <div>
                <label className="font-semibold text-slate-700 block mb-1">اسم الحساب (إنجليزي - اختياري):</label>
                <input
                  type="text"
                  value={accountForm.nameEn}
                  onChange={(e) => setAccountForm({ ...accountForm, nameEn: e.target.value })}
                  placeholder="e.g. Branch Petty Cash"
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
                    id="chkAllowCC"
                    checked={accountForm.allowCostCenter}
                    onChange={(e) => setAccountForm({ ...accountForm, allowCostCenter: e.target.checked })}
                    className="w-4 h-4 rounded text-sky-600 cursor-pointer"
                  />
                  <label htmlFor="chkAllowCC" className="font-semibold text-slate-700 cursor-pointer">
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
                  إضافة الحساب
                </button>
              </div>
            </form>
          </div>
        </div>
  );
};
`);

createModalFile('AccountingEditAccountModal', `import React from 'react';
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
            <div className="bg-[#0f2d4a] text-white px-4 py-3 flex items-center justify-between">
              <div className="flex items-center gap-2">
                <Edit3 className="w-4 h-4 text-sky-300" />
                <h3 className="font-bold text-sm">تعديل بيانات الحساب [{accountForm.code}]</h3>
              </div>
              <button
                onClick={onClose}
                className="text-slate-300 hover:text-white cursor-pointer"
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
`);

createModalFile('AccountingAddCostCenterModal', `import React from 'react';
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
            <div className="bg-[#0f2d4a] text-white px-4 py-3 flex items-center justify-between">
              <div className="flex items-center gap-2">
                <Building2 className="w-4 h-4 text-indigo-300" />
                <h3 className="font-bold text-sm">إضافة مركز تكلفة جديد</h3>
              </div>
              <button
                onClick={onClose}
                className="text-slate-300 hover:text-white cursor-pointer"
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
`);

createModalFile('AccountingEditCostCenterModal', `import React from 'react';
import { Edit3, X } from 'lucide-react';

interface AccountingEditCostCenterModalProps {
  isOpen: boolean;
  onClose: () => void;
  handleSaveEditCostCenter: (e: React.FormEvent) => void;
  costCenterForm: any;
  setCostCenterForm: React.Dispatch<React.SetStateAction<any>>;
  currency: string;
}

export const AccountingEditCostCenterModal: React.FC<AccountingEditCostCenterModalProps> = ({
  isOpen,
  onClose,
  handleSaveEditCostCenter,
  costCenterForm,
  setCostCenterForm,
  currency,
}) => {
  if (!isOpen) return null;
  return (
        <div className="fixed inset-0 bg-slate-950/60 backdrop-blur-xs flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-lg overflow-hidden animate-in fade-in zoom-in-95 duration-150">
            <div className="bg-[#0f2d4a] text-white px-4 py-3 flex items-center justify-between">
              <div className="flex items-center gap-2">
                <Edit3 className="w-4 h-4 text-indigo-300" />
                <h3 className="font-bold text-sm">تعديل مركز التكلفة [{costCenterForm.code}]</h3>
              </div>
              <button
                onClick={onClose}
                className="text-slate-300 hover:text-white cursor-pointer"
              >
                <X className="w-4 h-4" />
              </button>
            </div>

            <form onSubmit={handleSaveEditCostCenter} className="p-4 flex flex-col gap-3 text-xs">
              <div>
                <label className="font-semibold text-slate-700 block mb-1">اسم مركز التكلفة (عربي):</label>
                <input
                  type="text"
                  value={costCenterForm.name}
                  onChange={(e) => setCostCenterForm({ ...costCenterForm, name: e.target.value })}
                  className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900"
                  required
                />
              </div>

              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="font-semibold text-slate-700 block mb-1">اسم المدير المسؤول:</label>
                  <input
                    type="text"
                    value={costCenterForm.managerName}
                    onChange={(e) => setCostCenterForm({ ...costCenterForm, managerName: e.target.value })}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900"
                  />
                </div>
                <div>
                  <label className="font-semibold text-slate-700 block mb-1">الموازنة المعتمدة ({currency}):</label>
                  <input
                    type="number"
                    min={0}
                    value={costCenterForm.budget}
                    onChange={(e) => setCostCenterForm({ ...costCenterForm, budget: parseFloat(e.target.value) || 0 })}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 font-mono-numbers font-bold text-slate-900"
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
                  حفظ التعديلات
                </button>
              </div>
            </form>
          </div>
        </div>
  );
};
`);

createModalFile('AccountingNewEntryModal', `import React from 'react';
import { AccountNode, CostCenterNode } from '@/types/pos';
import { BookOpen, Plus, Trash2, CheckCircle2, AlertCircle, X } from 'lucide-react';

interface AccountingNewEntryModalProps {
  isOpen: boolean;
  onClose: () => void;
  handleSaveEntry: (e: React.FormEvent) => void;
  entryRef: string;
  setEntryRef: React.Dispatch<React.SetStateAction<string>>;
  entryDate: string;
  setEntryDate: React.Dispatch<React.SetStateAction<string>>;
  entryDesc: string;
  setEntryDesc: React.Dispatch<React.SetStateAction<string>>;
  entryLines: any[];
  handleAddLine: () => void;
  handleUpdateLine: (idx: number, field: string, value: any) => void;
  handleRemoveLine: (idx: number) => void;
  allAccountsFlat: AccountNode[];
  leafAccounts: AccountNode[];
  leafCostCenters: CostCenterNode[];
  totalDebit: number;
  totalCredit: number;
  isBalanced: boolean;
  balanceDifference: number;
  currency: string;
}

export const AccountingNewEntryModal: React.FC<AccountingNewEntryModalProps> = ({
  isOpen,
  onClose,
  handleSaveEntry,
  entryRef,
  setEntryRef,
  entryDate,
  setEntryDate,
  entryDesc,
  setEntryDesc,
  entryLines,
  handleAddLine,
  handleUpdateLine,
  handleRemoveLine,
  allAccountsFlat,
  leafAccounts,
  leafCostCenters,
  totalDebit,
  totalCredit,
  isBalanced,
  balanceDifference,
  currency,
}) => {
  if (!isOpen) return null;
  return (
        <div className="fixed inset-0 bg-slate-950/60 backdrop-blur-xs flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-4xl overflow-hidden animate-in fade-in zoom-in-95 duration-150">
            <div className="bg-[#0f2d4a] text-white px-4 py-3 flex items-center justify-between">
              <div className="flex items-center gap-2">
                <BookOpen className="w-5 h-5 text-sky-300" />
                <h3 className="font-bold text-sm">إنشاء وترحيل قيد يومية محاسبي مع توزيع مراكز التكلفة</h3>
              </div>
              <button
                onClick={onClose}
                className="text-slate-300 hover:text-white font-bold cursor-pointer"
              >
                <X className="w-5 h-5" />
              </button>
            </div>

            <form onSubmit={handleSaveEntry} className="p-4 flex flex-col gap-3 text-xs max-h-[85vh] overflow-y-auto">
              <div className="grid grid-cols-1 sm:grid-cols-3 gap-3 bg-slate-50 p-3 rounded-lg border border-slate-200">
                <div>
                  <label className="font-semibold text-slate-700 block mb-1">المرجع / السند:</label>
                  <input
                    type="text"
                    value={entryRef}
                    onChange={(e) => setEntryRef(e.target.value)}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900 bg-white"
                    required
                  />
                </div>
                <div>
                  <label className="font-semibold text-slate-700 block mb-1">تاريخ القيد:</label>
                  <input
                    type="date"
                    value={entryDate}
                    onChange={(e) => setEntryDate(e.target.value)}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900 bg-white"
                    required
                  />
                </div>
                <div>
                  <label className="font-semibold text-slate-700 block mb-1">البيان العام للقيد:</label>
                  <input
                    type="text"
                    value={entryDesc}
                    onChange={(e) => setEntryDesc(e.target.value)}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 text-slate-900 bg-white"
                    required
                  />
                </div>
              </div>

              {/* Journal Entry Lines Table */}
              <div className="border border-slate-300 rounded-lg overflow-hidden flex flex-col">
                <div className="bg-slate-100 p-2 border-b border-slate-300 flex items-center justify-between">
                  <span className="font-bold text-slate-700">بنود وأطراف القيد المحاسبي:</span>
                  <button
                    type="button"
                    onClick={handleAddLine}
                    className="bg-sky-700 hover:bg-sky-600 text-white px-2.5 py-1 rounded flex items-center gap-1 font-semibold cursor-pointer"
                  >
                    <Plus className="w-3.5 h-3.5" />
                    <span>إضافة سطر [طرف قيد]</span>
                  </button>
                </div>

                <div className="overflow-x-auto max-h-60">
                  <table className="w-full text-right border-collapse text-xs">
                    <thead>
                      <tr className="bg-slate-200 text-slate-700 font-bold border-b border-slate-300 sticky top-0">
                        <th className="p-2 w-56">الحساب المحاسبي</th>
                        <th className="p-2 w-48">مركز التكلفة</th>
                        <th className="p-2 w-28 text-left">مدين ({currency})</th>
                        <th className="p-2 w-28 text-left">دائن ({currency})</th>
                        <th className="p-2">شرح البند</th>
                        <th className="p-2 w-10 text-center">حذف</th>
                      </tr>
                    </thead>
                    <tbody>
                      {entryLines.map((line, idx) => {
                        const accObj = allAccountsFlat.find((a) => a.code === line.accountCode);
                        const allowsCC = accObj?.allowCostCenter ?? false;

                        return (
                          <tr key={line.id} className="border-b border-slate-200 bg-white hover:bg-slate-50">
                            <td className="p-1.5">
                              <select
                                value={line.accountCode}
                                onChange={(e) => handleUpdateLine(idx, 'accountCode', e.target.value)}
                                className="w-full border border-slate-300 rounded p-1 font-semibold text-slate-800 bg-white"
                              >
                                {leafAccounts.map((acc) => (
                                  <option key={acc.code} value={acc.code}>
                                    [{acc.code}] {acc.name}
                                  </option>
                                ))}
                              </select>
                            </td>

                            <td className="p-1.5">
                              <select
                                value={line.costCenterCode || ''}
                                onChange={(e) => handleUpdateLine(idx, 'costCenterCode', e.target.value)}
                                className={\`w-full border rounded p-1 text-[11px] \${
                                  allowsCC
                                    ? 'border-indigo-300 bg-indigo-50/50 font-semibold text-indigo-950'
                                    : 'border-slate-200 text-slate-600 bg-white'
                                }\`}
                              >
                                <option value="">-- بدون مركز تكلفة --</option>
                                {leafCostCenters.map((cc) => (
                                  <option key={cc.id} value={cc.code}>
                                    [{cc.code}] {cc.name}
                                  </option>
                                ))}
                              </select>
                            </td>

                            <td className="p-1.5">
                              <input
                                type="number"
                                min={0}
                                value={line.debit || ''}
                                onChange={(e) => handleUpdateLine(idx, 'debit', parseFloat(e.target.value) || 0)}
                                className="w-full border border-slate-300 rounded p-1 text-left font-mono-numbers font-bold text-blue-800"
                              />
                            </td>

                            <td className="p-1.5">
                              <input
                                type="number"
                                min={0}
                                value={line.credit || ''}
                                onChange={(e) => handleUpdateLine(idx, 'credit', parseFloat(e.target.value) || 0)}
                                className="w-full border border-slate-300 rounded p-1 text-left font-mono-numbers font-bold text-emerald-800"
                              />
                            </td>

                            <td className="p-1.5">
                              <input
                                type="text"
                                value={line.note}
                                onChange={(e) => handleUpdateLine(idx, 'note', e.target.value)}
                                placeholder="شرح البند..."
                                className="w-full border border-slate-300 rounded p-1"
                              />
                            </td>

                            <td className="p-1.5 text-center">
                              <button
                                type="button"
                                onClick={() => handleRemoveLine(idx)}
                                disabled={entryLines.length <= 2}
                                className="text-rose-500 hover:text-rose-700 disabled:opacity-30 cursor-pointer"
                              >
                                <Trash2 className="w-4 h-4" />
                              </button>
                            </td>
                          </tr>
                        );
                      })}
                    </tbody>
                  </table>
                </div>
              </div>

              {/* Totals & Balance Verification */}
              <div className="bg-slate-100 p-3 rounded-lg border border-slate-300 flex flex-wrap items-center justify-between gap-3">
                <div className="flex items-center gap-6">
                  <div>
                    <span className="text-slate-500 block text-[11px]">إجمالي المدين:</span>
                    <span className="font-mono-numbers font-bold text-blue-800 text-sm">
                      {totalDebit.toLocaleString()} {currency}
                    </span>
                  </div>
                  <div>
                    <span className="text-slate-500 block text-[11px]">إجمالي الدائن:</span>
                    <span className="font-mono-numbers font-bold text-emerald-800 text-sm">
                      {totalCredit.toLocaleString()} {currency}
                    </span>
                  </div>
                  <div>
                    <span className="text-slate-500 block text-[11px]">الفارق:</span>
                    <span className={\`font-mono-numbers font-bold text-sm \${isBalanced ? 'text-emerald-600' : 'text-rose-600'}\`}>
                      {balanceDifference.toLocaleString()} {currency}
                    </span>
                  </div>
                </div>

                <div>
                  {isBalanced ? (
                    <span className="bg-emerald-100 text-emerald-800 border border-emerald-300 px-3 py-1 rounded font-bold flex items-center gap-1.5">
                      <CheckCircle2 className="w-4 h-4 text-emerald-600" />
                      <span>القيد متوازن وجاهز للترحيل</span>
                    </span>
                  ) : (
                    <span className="bg-rose-100 text-rose-800 border border-rose-300 px-3 py-1 rounded font-bold flex items-center gap-1.5">
                      <AlertCircle className="w-4 h-4 text-rose-600" />
                      <span>القيد غير متوازن! يوجد فارق {balanceDifference.toLocaleString()} {currency}</span>
                    </span>
                  )}
                </div>
              </div>

              {/* Form Actions */}
              <div className="flex items-center justify-end gap-2 pt-2 border-t border-slate-200">
                <button
                  type="button"
                  onClick={onClose}
                  className="bg-white border border-slate-300 text-slate-700 px-3.5 py-1.5 rounded cursor-pointer"
                >
                  إلغاء
                </button>
                <button
                  type="submit"
                  disabled={!isBalanced}
                  className="bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 text-white font-bold px-6 py-1.5 rounded cursor-pointer"
                >
                  حفظ وترحيل القيد المحاسبي
                </button>
              </div>
            </form>
          </div>
        </div>
  );
};
`);

fs.writeFileSync('components/accounting/index.ts', `export * from './AccountingAddAccountModal';
export * from './AccountingEditAccountModal';
export * from './AccountingAddCostCenterModal';
export * from './AccountingEditCostCenterModal';
export * from './AccountingNewEntryModal';
`);
console.log('Accounting modals generated.');

let moduleContent = fs.readFileSync('components/AccountingModule.tsx', 'utf8');

// Insert imports
const importRegex = /import React, \{ useState, useMemo \} from 'react';/;
const newImports = `import React, { useState, useMemo } from 'react';
import {
  AccountingAddAccountModal,
  AccountingEditAccountModal,
  AccountingAddCostCenterModal,
  AccountingEditCostCenterModal,
  AccountingNewEntryModal,
} from './accounting';`;
moduleContent = moduleContent.replace(importRegex, newImports);

// Replace the JSX for the modals
const startIndex = moduleContent.indexOf('{/* ========================================================= */}\n      {/* MODAL: ADD SUB-ACCOUNT');
const endIndex = moduleContent.lastIndexOf('</div>\n  );\n};');

if (startIndex === -1 || endIndex === -1) {
  console.log('Error: Could not find modal slice');
  process.exit(1);
}

const replacementJSX = `      {/* ========================================================= */}
      {/* MODAL: ADD SUB-ACCOUNT (إضافة حساب فرعي جديد)              */}
      {/* ========================================================= */}
      <AccountingAddAccountModal
        isOpen={showAddAccountModal}
        onClose={() => setShowAddAccountModal(false)}
        handleSaveNewAccount={handleSaveNewAccount}
        addAccountParentCode={addAccountParentCode}
        allAccountsFlat={allAccountsFlat}
        accountForm={accountForm}
        setAccountForm={setAccountForm}
      />

      {/* ========================================================= */}
      {/* MODAL: EDIT ACCOUNT (تعديل بيانات الحساب)                   */}
      {/* ========================================================= */}
      <AccountingEditAccountModal
        isOpen={showEditAccountModal}
        onClose={() => setShowEditAccountModal(false)}
        handleSaveEditAccount={handleSaveEditAccount}
        accountForm={accountForm}
        setAccountForm={setAccountForm}
      />

      {/* ========================================================= */}
      {/* MODAL: ADD COST CENTER (إضافة مركز تكلفة جديد)             */}
      {/* ========================================================= */}
      <AccountingAddCostCenterModal
        isOpen={showAddCostCenterModal}
        onClose={() => setShowAddCostCenterModal(false)}
        handleSaveNewCostCenter={handleSaveNewCostCenter}
        addCostCenterParentId={addCostCenterParentId}
        allCostCentersFlat={allCostCentersFlat}
        costCenterForm={costCenterForm}
        setCostCenterForm={setCostCenterForm}
        currency={currency}
      />

      {/* ========================================================= */}
      {/* MODAL: EDIT COST CENTER (تعديل بيانات مركز التكلفة)         */}
      {/* ========================================================= */}
      <AccountingEditCostCenterModal
        isOpen={showEditCostCenterModal}
        onClose={() => setShowEditCostCenterModal(false)}
        handleSaveEditCostCenter={handleSaveEditCostCenter}
        costCenterForm={costCenterForm}
        setCostCenterForm={setCostCenterForm}
        currency={currency}
      />

      {/* ========================================================= */}
      {/* MODAL: NEW BALANCED JOURNAL ENTRY (إنشاء قيد يومية متوازن) */}
      {/* ========================================================= */}
      <AccountingNewEntryModal
        isOpen={showNewEntryModal}
        onClose={() => setShowNewEntryModal(false)}
        handleSaveEntry={handleSaveEntry}
        entryRef={entryRef}
        setEntryRef={setEntryRef}
        entryDate={entryDate}
        setEntryDate={setEntryDate}
        entryDesc={entryDesc}
        setEntryDesc={setEntryDesc}
        entryLines={entryLines}
        handleAddLine={handleAddLine}
        handleUpdateLine={handleUpdateLine}
        handleRemoveLine={handleRemoveLine}
        allAccountsFlat={allAccountsFlat}
        leafAccounts={leafAccounts}
        leafCostCenters={leafCostCenters}
        totalDebit={totalDebit}
        totalCredit={totalCredit}
        isBalanced={isBalanced}
        balanceDifference={balanceDifference}
        currency={currency}
      />
    `;

const newModuleContent = moduleContent.substring(0, startIndex) + replacementJSX + moduleContent.substring(endIndex);
fs.writeFileSync('components/AccountingModule.tsx', newModuleContent);
console.log('AccountingModule.tsx refactored.');
