'use client';

import React, { useState, useMemo } from 'react';
import { 
  Calendar, 
  Lock, 
  Unlock, 
  CheckCircle2, 
  AlertCircle, 
  FileSpreadsheet, 
  ArrowRight, 
  TrendingUp, 
  TrendingDown, 
  ShieldCheck, 
  X, 
  Layers, 
  FileText, 
  HelpCircle,
  RotateCcw,
  Sparkles
} from 'lucide-react';
import { 
  FiscalPeriod, 
  AccountNode, 
  JournalEntry 
} from '@/types/pos';

interface FiscalPeriodsViewProps {
  periods: FiscalPeriod[];
  accounts: AccountNode[];
  journalEntries: JournalEntry[];
  onClosePeriod: (periodId: string, closingEntry: JournalEntry, notes: string) => void;
  onReopenPeriod: (periodId: string) => void;
  currency: string;
}

export const FiscalPeriodsView: React.FC<FiscalPeriodsViewProps> = ({
  periods,
  accounts,
  journalEntries,
  onClosePeriod,
  onReopenPeriod,
  currency,
}) => {
  const [selectedPeriod, setSelectedPeriod] = useState<FiscalPeriod | null>(null);
  const [showClosingWizard, setShowClosingWizard] = useState(false);
  const [closingNotes, setClosingNotes] = useState('');
  const [closingError, setClosingError] = useState<string | null>(null);

  // Helper to extract flat leaf accounts
  const flatLeafAccounts = useMemo(() => {
    const list: AccountNode[] = [];
    const traverse = (nodes: AccountNode[]) => {
      nodes.forEach((n) => {
        if (!n.children || n.children.length === 0 || n.isLeaf) {
          list.push(n);
        } else {
          traverse(n.children);
        }
      });
    };
    traverse(accounts);
    return list;
  }, [accounts]);

  // Current financial state calculations for active period
  const revenueAccounts = useMemo(() => {
    return flatLeafAccounts.filter((a) => a.type === 'REVENUE' || a.code.startsWith('4'));
  }, [flatLeafAccounts]);

  const expenseAccounts = useMemo(() => {
    return flatLeafAccounts.filter((a) => a.type === 'EXPENSE' || a.code.startsWith('5'));
  }, [flatLeafAccounts]);

  const totalRevenues = useMemo(() => {
    return revenueAccounts.reduce((sum, a) => sum + (a.balance || 0), 0);
  }, [revenueAccounts]);

  const totalExpenses = useMemo(() => {
    return expenseAccounts.reduce((sum, a) => sum + (a.balance || 0), 0);
  }, [expenseAccounts]);

  const netIncome = totalRevenues - totalExpenses;

  // Open Closing Wizard for a specific period
  const handleOpenWizard = (period: FiscalPeriod) => {
    if (period.isClosed) {
      alert('هذه الفترة مقفلة بالفعل.');
      return;
    }
    setSelectedPeriod(period);
    setClosingNotes(`إقفال الحسابات الختامية وترحيل صافي النشاط للفترة ${period.periodName}`);
    setClosingError(null);
    setShowClosingWizard(true);
  };

  // Execute closing
  const handleExecuteClosing = () => {
    if (!selectedPeriod) return;
    setClosingError(null);

    // Build the closing journal entry lines
    const lines = [];
    let lineId = 1;

    // 1. Debit all revenue accounts (closing credit balances)
    revenueAccounts.forEach((rev) => {
      if (rev.balance > 0) {
        lines.push({
          id: `CLS-L${lineId++}`,
          accountCode: rev.code,
          accountName: rev.name,
          debit: rev.balance,
          credit: 0,
          note: `إقفال حساب الإيراد في حساب الأرباح والخسائر - ${selectedPeriod.periodName}`,
        });
      }
    });

    // 2. Credit all expense accounts (closing debit balances)
    expenseAccounts.forEach((exp) => {
      if (exp.balance > 0) {
        lines.push({
          id: `CLS-L${lineId++}`,
          accountCode: exp.code,
          accountName: exp.name,
          debit: 0,
          credit: exp.balance,
          note: `إقفال حساب المصروف في حساب الأرباح والخسائر - ${selectedPeriod.periodName}`,
        });
      }
    });

    // 3. Balance the difference into Equity (3201 Retained Earnings / Current Net Profit)
    const equityAcc = flatLeafAccounts.find((a) => a.code === '3201') || {
      code: '3201',
      name: 'الأرباح المبقاة (الفائض المحتجز والمدور)',
    };

    if (netIncome > 0) {
      // Net Profit: Credit Equity
      lines.push({
        id: `CLS-L${lineId++}`,
        accountCode: equityAcc.code,
        accountName: equityAcc.name,
        debit: 0,
        credit: netIncome,
        note: `ترحيل صافي الأرباح المحققة للفترة (${selectedPeriod.periodName}) لحساب الأرباح المبقاة`,
      });
    } else if (netIncome < 0) {
      // Net Loss: Debit Equity
      lines.push({
        id: `CLS-L${lineId++}`,
        accountCode: equityAcc.code,
        accountName: equityAcc.name,
        debit: Math.abs(netIncome),
        credit: 0,
        note: `ترحيل صافي العجز/الخسائر للفترة (${selectedPeriod.periodName}) على حقوق الملكية`,
      });
    }

    const totalDebit = lines.reduce((s, l) => s + l.debit, 0);
    const totalCredit = lines.reduce((s, l) => s + l.credit, 0);

    const closingEntryId = `JV-CLS-${Date.now()}`;
    const closingEntry: JournalEntry = {
      id: closingEntryId,
      entryNo: `JV-CLS-${selectedPeriod.fiscalYear}`,
      date: selectedPeriod.endDate,
      reference: `قيد إقفال ختامي للفترة: ${selectedPeriod.periodName}`,
      description: `قيد إقفال الحسابات الاسمية (الإيرادات والمصروفات) وترحيل صافي النتيجة لحقوق الملكية`,
      lines,
      totalDebit,
      totalCredit,
      isBalanced: Math.abs(totalDebit - totalCredit) < 0.01,
      status: 'POSTED',
    };

    onClosePeriod(selectedPeriod.id, closingEntry, closingNotes);
    setShowClosingWizard(false);
  };

  return (
    <div className="flex-1 flex flex-col h-full bg-[#f1f5f9] text-slate-800 select-none overflow-hidden font-sans p-2 gap-2">
      {/* 1. Overview Strip */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-2 shrink-0">
        <div className="bg-white p-2.5 rounded border border-slate-300 shadow-xs flex items-center justify-between">
          <div>
            <div className="text-[11px] text-slate-500 font-bold flex items-center gap-1">
              <Calendar className="w-3.5 h-3.5 text-sky-600" />
              <span>السنة المالية الحالية</span>
            </div>
            <div className="text-base font-bold font-mono text-slate-900 mt-0.5">
              عام 2026 م
            </div>
            <div className="text-[10px] text-slate-500 mt-0.5">
              عدد الفترات المسجلة: <span className="font-bold">{periods.length}</span> فترات
            </div>
          </div>
          <div className="w-9 h-9 rounded-full bg-sky-50 flex items-center justify-center text-sky-700">
            <Layers className="w-5 h-5" />
          </div>
        </div>

        <div className="bg-white p-2.5 rounded border border-slate-300 shadow-xs flex items-center justify-between">
          <div>
            <div className="text-[11px] text-emerald-700 font-bold flex items-center gap-1">
              <TrendingUp className="w-3.5 h-3.5" />
              <span>إجمالي الإيرادات الحالية</span>
            </div>
            <div className="text-base font-bold font-mono-numbers text-slate-900 mt-0.5">
              {totalRevenues.toLocaleString()} <span className="text-xs font-normal">{currency}</span>
            </div>
            <div className="text-[10px] text-slate-500 mt-0.5">
              حسابات فئة (4) - إيرادات تشغيلية
            </div>
          </div>
          <div className="w-9 h-9 rounded-full bg-emerald-50 flex items-center justify-center text-emerald-700">
            <TrendingUp className="w-5 h-5" />
          </div>
        </div>

        <div className="bg-white p-2.5 rounded border border-slate-300 shadow-xs flex items-center justify-between">
          <div>
            <div className="text-[11px] text-rose-700 font-bold flex items-center gap-1">
              <TrendingDown className="w-3.5 h-3.5" />
              <span>إجمالي المصروفات الحالية</span>
            </div>
            <div className="text-base font-bold font-mono-numbers text-slate-900 mt-0.5">
              {totalExpenses.toLocaleString()} <span className="text-xs font-normal">{currency}</span>
            </div>
            <div className="text-[10px] text-slate-500 mt-0.5">
              حسابات فئة (5) - تكاليف ومصاريف
            </div>
          </div>
          <div className="w-9 h-9 rounded-full bg-rose-50 flex items-center justify-center text-rose-700">
            <TrendingDown className="w-5 h-5" />
          </div>
        </div>

        <div className="bg-white p-2.5 rounded border border-slate-300 shadow-xs flex items-center justify-between">
          <div>
            <div className="text-[11px] text-indigo-700 font-bold flex items-center gap-1">
              <ShieldCheck className="w-3.5 h-3.5" />
              <span>صافي الربح التقديري (Net P&L)</span>
            </div>
            <div
              className={`text-base font-bold font-mono-numbers mt-0.5 ${
                netIncome >= 0 ? 'text-emerald-700' : 'text-rose-700'
              }`}
            >
              {netIncome.toLocaleString()} <span className="text-xs font-normal">{currency}</span>
            </div>
            <div className="text-[10px] text-slate-500 mt-0.5">
              {netIncome >= 0 ? 'فائض أرباح نشاط' : 'عجز خسائر نشاط'}
            </div>
          </div>
          <div className="w-9 h-9 rounded-full bg-indigo-50 flex items-center justify-center text-indigo-700">
            <FileSpreadsheet className="w-5 h-5" />
          </div>
        </div>
      </div>

      {/* 2. Fiscal Periods DataGridView */}
      <div className="flex-1 bg-white rounded border border-slate-300 shadow-xs overflow-hidden flex flex-col">
        <div className="p-2.5 bg-slate-100 border-b border-slate-300 flex items-center justify-between text-xs">
          <div className="font-bold text-slate-800 flex items-center gap-1.5">
            <Calendar className="w-4 h-4 text-sky-700" />
            <span>سجل الفترات المحاسبية وإقفالات السنة المالية (Fiscal Periods Table)</span>
          </div>
          <div className="text-[11px] text-slate-500">
            عند إقفال أي فترة مالية، يمنع النظام آلياً إضافة أو تعديل أي سندات أو قيود ضمن نطاقها الزمني
          </div>
        </div>

        <div className="overflow-x-auto flex-1">
          <table className="w-full text-right text-xs border-collapse font-sans">
            <thead className="bg-slate-100 text-slate-700 border-b border-slate-200 sticky top-0 z-10 select-none text-[11px] font-bold">
              <tr>
                <th className="p-2 border-l border-slate-200 w-16 text-center">السنة</th>
                <th className="p-2 border-l border-slate-200">اسم الفترة المالية</th>
                <th className="p-2 border-l border-slate-200 w-28">تاريخ البدء</th>
                <th className="p-2 border-l border-slate-200 w-28">تاريخ الانتهاء</th>
                <th className="p-2 border-l border-slate-200 w-24 text-center">الحالة</th>
                <th className="p-2 border-l border-slate-200 text-left w-32">الإيرادات ({currency})</th>
                <th className="p-2 border-l border-slate-200 text-left w-32">المصروفات ({currency})</th>
                <th className="p-2 border-l border-slate-200 text-left w-32">صافي الربح ({currency})</th>
                <th className="p-2 border-l border-slate-200">ملاحظات واعتماد الإقفال</th>
                <th className="p-2 w-32 text-center">إجراءات الإقفال</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-200">
              {periods.map((p, idx) => {
                const isClosed = p.isClosed;
                return (
                  <tr
                    key={p.id}
                    className={`transition ${
                      isClosed
                        ? 'bg-slate-50/80 hover:bg-slate-100'
                        : idx % 2 === 0
                        ? 'bg-white hover:bg-sky-50/60'
                        : 'bg-slate-50/40 hover:bg-sky-50/60'
                    }`}
                  >
                    <td className="p-2 font-mono font-bold text-center border-l border-slate-200 text-slate-700">
                      {p.fiscalYear}
                    </td>

                    <td className="p-2 border-l border-slate-200 font-bold text-slate-900">
                      <div className="flex items-center gap-1.5">
                        {isClosed ? (
                          <Lock className="w-3.5 h-3.5 text-rose-600 shrink-0" />
                        ) : (
                          <Unlock className="w-3.5 h-3.5 text-emerald-600 shrink-0" />
                        )}
                        <span>{p.periodName}</span>
                      </div>
                    </td>

                    <td className="p-2 font-mono text-slate-600 border-l border-slate-200 text-[11px]">
                      {p.startDate}
                    </td>

                    <td className="p-2 font-mono text-slate-600 border-l border-slate-200 text-[11px]">
                      {p.endDate}
                    </td>

                    <td className="p-2 border-l border-slate-200 text-center">
                      {isClosed ? (
                        <span className="bg-rose-100 text-rose-800 border border-rose-300 px-2 py-0.5 rounded text-[10px] font-bold inline-flex items-center gap-1">
                          <Lock className="w-3 h-3" />
                          <span>مقفلة</span>
                        </span>
                      ) : (
                        <span className="bg-emerald-100 text-emerald-800 border border-emerald-300 px-2 py-0.5 rounded text-[10px] font-bold inline-flex items-center gap-1">
                          <Unlock className="w-3 h-3" />
                          <span>مفتوحة</span>
                        </span>
                      )}
                    </td>

                    <td className="p-2 border-l border-slate-200 font-mono-numbers text-left text-slate-800">
                      {p.totalRevenues !== undefined
                        ? p.totalRevenues.toLocaleString()
                        : isClosed
                        ? '-'
                        : totalRevenues.toLocaleString()}
                    </td>

                    <td className="p-2 border-l border-slate-200 font-mono-numbers text-left text-slate-800">
                      {p.totalExpenses !== undefined
                        ? p.totalExpenses.toLocaleString()
                        : isClosed
                        ? '-'
                        : totalExpenses.toLocaleString()}
                    </td>

                    <td className="p-2 border-l border-slate-200 font-mono-numbers font-bold text-left">
                      {p.netProfitLoss !== undefined ? (
                        <span className={p.netProfitLoss >= 0 ? 'text-emerald-700' : 'text-rose-700'}>
                          {p.netProfitLoss.toLocaleString()}
                        </span>
                      ) : isClosed ? (
                        '-'
                      ) : (
                        <span className={netIncome >= 0 ? 'text-emerald-700' : 'text-rose-700'}>
                          {netIncome.toLocaleString()}
                        </span>
                      )}
                    </td>

                    <td className="p-2 border-l border-slate-200 text-[11px] text-slate-600 max-w-[200px] truncate" title={p.closingNotes}>
                      {p.closingNotes ? (
                        <div>
                          <div>{p.closingNotes}</div>
                          {p.closedAt && (
                            <div className="text-[10px] text-slate-400">
                              أقفل في: {p.closedAt} ({p.closedBy})
                            </div>
                          )}
                        </div>
                      ) : (
                        <span className="text-slate-400 italic">نشطة - تقبل القيود</span>
                      )}
                    </td>

                    <td className="p-2 text-center whitespace-nowrap">
                      {!isClosed ? (
                        <button
                          onClick={() => handleOpenWizard(p)}
                          className="bg-rose-600 hover:bg-rose-700 text-white px-2.5 py-1 rounded text-[11px] font-bold shadow-xs transition flex items-center justify-center gap-1 cursor-pointer mx-auto"
                        >
                          <Lock className="w-3 h-3" />
                          <span>إقفال الفترة</span>
                        </button>
                      ) : (
                        <button
                          onClick={() => {
                            if (confirm(`هل تريد بالتأكيد إعادة فتح الفترة المالية: ${p.periodName}؟\nتنبيه: سيتيح ذلك إضافة وتعديل القيود مرة أخرى.`)) {
                              onReopenPeriod(p.id);
                            }
                          }}
                          className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-300 px-2 py-1 rounded text-[10px] font-semibold transition flex items-center justify-center gap-1 cursor-pointer mx-auto"
                        >
                          <RotateCcw className="w-3 h-3 text-slate-500" />
                          <span>إعادة فتح</span>
                        </button>
                      )}
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>

        {/* Footer Note */}
        <div className="bg-slate-50 text-slate-700 px-3 py-1.5 text-xs flex items-center justify-between shrink-0 border-t border-slate-200">
          <div className="flex items-center gap-2 text-slate-600 text-[11px]">
            <HelpCircle className="w-3.5 h-3.5 text-sky-600" />
            <span>
              نظام الإقفال الآلي يضمن تصفير الحسابات الاسمية وترحيل الفائض تلقائياً إلى حساب الأرباح المبقاة (3201).
            </span>
          </div>
          <div className="text-slate-500 text-[11px]">
            معايير المحاسبة الدولية (IAS 1 & IFRS)
          </div>
        </div>
      </div>

      {/* 3. Closing Wizard Modal (معالج إقفال الفترة المالية) */}
      {showClosingWizard && selectedPeriod && (
        <div className="fixed inset-0 bg-black/60 z-50 flex items-center justify-center p-3 backdrop-blur-xs">
          <div className="bg-white rounded-lg shadow-2xl border border-slate-400 w-full max-w-2xl overflow-hidden flex flex-col max-h-[92vh]">
            <div className="bg-[#7f1d1d] text-white p-3 flex items-center justify-between shrink-0">
              <div className="flex items-center gap-2 font-bold text-sm">
                <Lock className="w-4 h-4 text-rose-300" />
                <span>معالج إقفال الفترة المالية (Fiscal Closing Wizard): {selectedPeriod.periodName}</span>
              </div>
              <button
                onClick={() => setShowClosingWizard(false)}
                className="text-white/80 hover:text-white p-1 hover:bg-white/10 rounded cursor-pointer"
              >
                <X className="w-4 h-4" />
              </button>
            </div>

            <div className="p-4 space-y-3 overflow-y-auto flex-1 text-xs">
              {closingError && (
                <div className="p-2.5 bg-rose-50 border border-rose-300 text-rose-800 rounded font-semibold flex items-center gap-2">
                  <AlertCircle className="w-4 h-4 text-rose-600 shrink-0" />
                  <span>{closingError}</span>
                </div>
              )}

              {/* Pre-Closing Audit Summary */}
              <div className="p-3 bg-slate-50 border border-slate-200 rounded space-y-2">
                <div className="font-bold text-slate-800 text-xs flex items-center gap-1.5">
                  <ShieldCheck className="w-4 h-4 text-emerald-600" />
                  <span>تقرير فحص وتدقيق ما قبل الإقفال (Pre-Closing Audit Report)</span>
                </div>

                <div className="grid grid-cols-3 gap-2 pt-1 text-center">
                  <div className="p-2 bg-white rounded border border-slate-200">
                    <div className="text-[10px] text-slate-500">إجمالي الإيرادات (الدائنة)</div>
                    <div className="font-bold font-mono-numbers text-emerald-700 text-sm mt-0.5">
                      {totalRevenues.toLocaleString()} {currency}
                    </div>
                  </div>

                  <div className="p-2 bg-white rounded border border-slate-200">
                    <div className="text-[10px] text-slate-500">إجمالي المصروفات (المدينة)</div>
                    <div className="font-bold font-mono-numbers text-rose-700 text-sm mt-0.5">
                      {totalExpenses.toLocaleString()} {currency}
                    </div>
                  </div>

                  <div className="p-2 bg-white rounded border border-slate-200">
                    <div className="text-[10px] text-slate-500">صافي الأرباح المحولة لحقوق الملكية</div>
                    <div className={`font-bold font-mono-numbers text-sm mt-0.5 ${netIncome >= 0 ? 'text-emerald-700' : 'text-rose-700'}`}>
                      {netIncome.toLocaleString()} {currency}
                    </div>
                  </div>
                </div>
              </div>

              {/* Proposed Closing Journal Entry Preview */}
              <div>
                <label className="block text-[11px] font-bold text-slate-800 mb-1">
                  معاينة قيد إقفال الحسابات الاسمية الآلي المقترح:
                </label>
                <div className="border border-slate-200 rounded max-h-48 overflow-y-auto">
                  <table className="w-full text-right text-[11px] border-collapse">
                    <thead className="bg-slate-100 text-slate-700 sticky top-0">
                      <tr>
                        <th className="p-1.5 border-b">رقم الحساب</th>
                        <th className="p-1.5 border-b">اسم الحساب</th>
                        <th className="p-1.5 border-b text-left">مدين</th>
                        <th className="p-1.5 border-b text-left">دائن</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-slate-100 font-mono">
                      {revenueAccounts.map((rev) => (
                        <tr key={rev.code} className="bg-emerald-50/40">
                          <td className="p-1.5">{rev.code}</td>
                          <td className="p-1.5 font-sans font-semibold">{rev.name}</td>
                          <td className="p-1.5 text-left text-emerald-800 font-bold">{rev.balance.toLocaleString()}</td>
                          <td className="p-1.5 text-left text-slate-400">0</td>
                        </tr>
                      ))}
                      {expenseAccounts.map((exp) => (
                        <tr key={exp.code} className="bg-rose-50/40">
                          <td className="p-1.5">{exp.code}</td>
                          <td className="p-1.5 font-sans font-semibold">{exp.name}</td>
                          <td className="p-1.5 text-left text-slate-400">0</td>
                          <td className="p-1.5 text-left text-rose-800 font-bold">{exp.balance.toLocaleString()}</td>
                        </tr>
                      ))}
                      <tr className="bg-indigo-50 font-bold">
                        <td className="p-1.5 text-indigo-900">3201</td>
                        <td className="p-1.5 font-sans text-indigo-900">الأرباح المبقاة (الفائض المحتجز والمدور)</td>
                        <td className="p-1.5 text-left">{netIncome < 0 ? Math.abs(netIncome).toLocaleString() : '0'}</td>
                        <td className="p-1.5 text-left">{netIncome > 0 ? netIncome.toLocaleString() : '0'}</td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              {/* Notes input */}
              <div>
                <label className="block text-[11px] font-bold text-slate-700 mb-1">
                  ملاحظات وتوصيات الإقفال المحاسبي:
                </label>
                <textarea
                  rows={2}
                  value={closingNotes}
                  onChange={(e) => setClosingNotes(e.target.value)}
                  placeholder="اكتب ملاحظات التدقيق أو محضر الإقفال الدوري..."
                  className="w-full p-2 bg-slate-50 border border-slate-300 rounded text-xs focus:bg-white focus:outline-none focus:border-rose-500"
                />
              </div>

              <div className="p-2.5 bg-amber-50 border border-amber-300 rounded text-amber-900 text-[11px] flex items-start gap-2">
                <AlertCircle className="w-4 h-4 text-amber-600 shrink-0 mt-0.5" />
                <div>
                  <b>تنبيه نظام الرقابة المحاسبية:</b> عند تأكيد الإقفال، سيتم توليد وترحيل القيد أعلاه فورياً وتصفير أرصدة الإيرادات والمصروفات للفترة القادمة، وقفل الفترة ضد أي تعديلات.
                </div>
              </div>

              {/* Action Buttons */}
              <div className="flex items-center justify-end gap-2 pt-2 border-t border-slate-200">
                <button
                  type="button"
                  onClick={() => setShowClosingWizard(false)}
                  className="px-4 py-1.5 border border-slate-300 rounded text-slate-700 hover:bg-slate-100 font-bold cursor-pointer"
                >
                  إلغاء الأمر
                </button>
                <button
                  type="button"
                  onClick={handleExecuteClosing}
                  className="px-5 py-1.5 bg-rose-700 hover:bg-rose-800 text-white rounded font-bold shadow-xs transition flex items-center gap-1.5 cursor-pointer"
                >
                  <Lock className="w-3.5 h-3.5" />
                  <span>اعتماد الإقفال وترحيل القيد الختامي</span>
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
