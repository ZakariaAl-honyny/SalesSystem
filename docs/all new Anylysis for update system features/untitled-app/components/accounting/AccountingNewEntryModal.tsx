import React from 'react';
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
  handleUpdateLine: (idx: number, field: any, value: any) => void;
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
            <div className="bg-white border-b border-slate-200 px-5 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
              <div className="flex items-center gap-2.5">
                <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
                  <BookOpen className="w-4 h-4 text-sky-700" />
                </div>
                <h3 className="font-bold text-sm text-slate-900">إنشاء وترحيل قيد يومية محاسبي مع توزيع مراكز التكلفة</h3>
              </div>
              <button
                onClick={onClose}
                className="text-slate-400 hover:text-slate-700 p-1.5 rounded-lg hover:bg-slate-100 font-bold cursor-pointer transition"
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
                                className={`w-full border rounded p-1 text-[11px] ${
                                  allowsCC
                                    ? 'border-indigo-300 bg-indigo-50/50 font-semibold text-indigo-950'
                                    : 'border-slate-200 text-slate-600 bg-white'
                                }`}
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
                    <span className={`font-mono-numbers font-bold text-sm ${isBalanced ? 'text-emerald-600' : 'text-rose-600'}`}>
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
