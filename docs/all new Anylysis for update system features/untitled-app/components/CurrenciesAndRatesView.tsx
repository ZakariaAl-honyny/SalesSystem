'use client';

import React, { useState } from 'react';
import { CurrencyItem, CurrencyRateItem } from '@/types/pos';
import {
  ArrowRight,
  Plus,
  Trash2,
  ChevronDown,
  ChevronUp,
  Coins,
  TrendingUp,
  X,
  Check,
  AlertCircle
} from 'lucide-react';

interface CurrenciesAndRatesViewProps {
  initialMode?: 'CURRENCIES' | 'RATES';
  currencies: CurrencyItem[];
  currencyRates: CurrencyRateItem[];
  onAddCurrency: (currency: CurrencyItem) => void;
  onAddCurrencyRate: (rate: CurrencyRateItem) => void;
  onDeleteCurrencyRate?: (id: string) => void;
  onBack: () => void;
}

export const CurrenciesAndRatesView: React.FC<CurrenciesAndRatesViewProps> = ({
  initialMode = 'CURRENCIES',
  currencies,
  currencyRates,
  onAddCurrency,
  onAddCurrencyRate,
  onDeleteCurrencyRate,
  onBack,
}) => {
  const [activeMode, setActiveMode] = useState<'CURRENCIES' | 'RATES'>(initialMode);
  const [openGroups, setOpenGroups] = useState<Record<string, boolean>>({
    'دولار': true,
    'سعودي': true,
  });

  // Modal State for adding Currency
  const [showAddCurrencyModal, setShowAddCurrencyModal] = useState(false);
  const [newCurrName, setNewCurrName] = useState('');
  const [newCurrSubUnit, setNewCurrSubUnit] = useState('');
  const [newCurrCode, setNewCurrCode] = useState('');

  // Modal State for adding Currency Rate
  const [showAddRateModal, setShowAddRateModal] = useState(false);
  const [newRateCurrency, setNewRateCurrency] = useState('دولار');
  const [newRateValue, setNewRateValue] = useState<number | ''>(700);
  const [newRateMin, setNewRateMin] = useState<number | ''>(500);
  const [newRateMax, setNewRateMax] = useState<number | ''>(850);
  const [rateRangeError, setRateRangeError] = useState<string | null>(null);
  const [newRateFromDate, setNewRateFromDate] = useState('2026-06-04');
  const [newRateToDate, setNewRateToDate] = useState('');

  const toggleGroup = (curr: string) => {
    setOpenGroups((prev) => ({
      ...prev,
      [curr]: !prev[curr],
    }));
  };

  const handleSaveCurrency = (e: React.FormEvent) => {
    e.preventDefault();
    if (!newCurrName.trim()) return;
    const item: CurrencyItem = {
      id: `CUR-${Date.now()}`,
      name: newCurrName.trim(),
      subUnit: newCurrSubUnit.trim() || 'فلس',
      code: newCurrCode.trim() || newCurrName.substring(0, 3).toUpperCase(),
      accountsCount: 0,
      isLocal: false,
    };
    onAddCurrency(item);
    setShowAddCurrencyModal(false);
    setNewCurrName('');
    setNewCurrSubUnit('');
    setNewCurrCode('');
  };

  const handleSaveRate = (e: React.FormEvent) => {
    e.preventDefault();
    if (!newRateValue || Number(newRateValue) <= 0) return;

    const rateVal = Number(newRateValue);
    const minVal = newRateMin !== '' ? Number(newRateMin) : undefined;
    const maxVal = newRateMax !== '' ? Number(newRateMax) : undefined;

    if (minVal !== undefined && rateVal < minVal) {
      setRateRangeError(`تنبيه: سعر الصرف المدخل (${rateVal}) أقل من الحد الأدنى المسموح به (${minVal})!`);
      return;
    }
    if (maxVal !== undefined && rateVal > maxVal) {
      setRateRangeError(`تنبيه: سعر الصرف المدخل (${rateVal}) أكبر من الحد الأقصى المسموح به (${maxVal})!`);
      return;
    }
    setRateRangeError(null);

    const item: CurrencyRateItem = {
      id: `CR-${Date.now()}`,
      currencyName: newRateCurrency,
      rate: rateVal,
      minRate: minVal,
      maxRate: maxVal,
      fromDate: newRateFromDate || new Date().toISOString().split('T')[0],
      toDate: newRateToDate || undefined,
      isCurrent: !newRateToDate,
    };
    onAddCurrencyRate(item);
    setShowAddRateModal(false);
    setNewRateValue(700);
    setNewRateMin(500);
    setNewRateMax(850);
    setNewRateToDate('');
  };

  // Group rates by currency
  const groupedRates: Record<string, CurrencyRateItem[]> = {};
  currencyRates.forEach((r) => {
    if (!groupedRates[r.currencyName]) {
      groupedRates[r.currencyName] = [];
    }
    groupedRates[r.currencyName].push(r);
  });

  return (
    <div id="currencies-rates-view" className="flex flex-col h-full bg-white font-sans select-none" dir="rtl">
      {/* 1. Top Header Bar (matches Screenshot 2 & 3) */}
      <header className="bg-[#0277bd] text-white px-4 py-3 flex items-center justify-between shadow-md shrink-0">
        <div className="flex items-center gap-3">
          <button
            onClick={onBack}
            className="p-1 hover:bg-white/20 rounded-full transition cursor-pointer text-white"
            title="رجوع"
          >
            <ArrowRight className="w-6 h-6" />
          </button>
          <h1 className="text-lg font-bold">
            {activeMode === 'CURRENCIES' ? 'العملات' : 'أسعار العملات'}
          </h1>
        </div>

        {/* Quick Mode Toggle */}
        <div className="flex bg-black/20 rounded-lg p-0.5 text-xs font-bold">
          <button
            onClick={() => setActiveMode('CURRENCIES')}
            className={`px-3 py-1 rounded-md transition cursor-pointer ${
              activeMode === 'CURRENCIES'
                ? 'bg-white text-[#0277bd] shadow-xs'
                : 'text-white/80 hover:text-white'
            }`}
          >
            العملات
          </button>
          <button
            onClick={() => setActiveMode('RATES')}
            className={`px-3 py-1 rounded-md transition cursor-pointer ${
              activeMode === 'RATES'
                ? 'bg-white text-[#0277bd] shadow-xs'
                : 'text-white/80 hover:text-white'
            }`}
          >
            أسعار العملات
          </button>
        </div>
      </header>

      {/* MVP Single Current Rate & Base Currency Notice Banner */}
      <div className="bg-sky-50 border-b border-sky-200 px-4 py-2.5 flex items-center justify-between text-xs text-sky-900">
        <div className="flex items-center gap-2">
          <AlertCircle className="w-4 h-4 text-sky-700 shrink-0" />
          <span>
            <strong>العملة الرئيسية وأسعار الصرف الحالية:</strong> العملة الأساسية للنظام ثابتة كمرجع قياسي (يمكن تعديل اسمها ورمزها)، والعملات الأجنبية تحول مباشرة بناءً على <strong>سعر الصرف الحالي المباشر</strong> دون تتبع تاريخي أو فروق عملات.
          </span>
        </div>
        <span className="bg-sky-200/80 text-sky-950 px-2 py-0.5 rounded-full font-bold text-[10px] shrink-0">
          سعر مباشر
        </span>
      </div>

      {/* 2. Main Body Content */}
      <main className="flex-1 overflow-y-auto bg-slate-50">
        {activeMode === 'CURRENCIES' ? (
          /* View 1: "العملات" (Screenshot 3) */
          <div className="w-full">
            <table className="w-full text-right text-sm border-collapse">
              <thead>
                <tr className="bg-[#0277bd] text-white border-b border-[#01579b]">
                  <th className="py-2.5 px-4 font-bold text-sm">العملة</th>
                  <th className="py-2.5 px-4 font-bold text-sm">الفكة</th>
                  <th className="py-2.5 px-4 font-bold text-sm">رمز العملة</th>
                  <th className="py-2.5 px-4 font-bold text-sm">عدد الحسابات</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200 bg-white">
                {currencies.map((curr) => (
                  <tr key={curr.id} className="hover:bg-sky-50/50 transition">
                    <td className="py-3 px-4 font-bold text-slate-800">{curr.name}</td>
                    <td className="py-3 px-4 text-slate-700">{curr.subUnit}</td>
                    <td className="py-3 px-4 text-slate-600 font-mono font-semibold">{curr.code}</td>
                    <td className="py-3 px-4 text-slate-700 font-semibold">{curr.accountsCount}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ) : (
          /* View 2: "أسعار العملات" (Screenshot 2) */
          <div className="w-full">
            <table className="w-full text-right text-sm border-collapse">
              <thead>
                <tr className="bg-[#0277bd] text-white border-b border-[#01579b]">
                  <th className="py-2.5 px-4 font-bold text-sm">العملة</th>
                  <th className="py-2.5 px-4 font-bold text-sm">السعر</th>
                  <th className="py-2.5 px-4 font-bold text-sm">النطاق المسموح (أقل / أكثر)</th>
                  <th className="py-2.5 px-4 font-bold text-sm">من تاريخ</th>
                  <th className="py-2.5 px-4 font-bold text-sm">إلى تاريخ</th>
                </tr>
              </thead>
              <tbody className="bg-white">
                {Object.keys(groupedRates).map((currName) => {
                  const rates = groupedRates[currName];
                  const isOpen = openGroups[currName] ?? true;

                  return (
                    <React.Fragment key={currName}>
                      {/* Currency Group Header (like 'دولار' with up/down arrow) */}
                      <tr
                        onClick={() => toggleGroup(currName)}
                        className="bg-slate-100 hover:bg-slate-200 border-y border-slate-300 cursor-pointer font-bold text-slate-800"
                      >
                        <td colSpan={5} className="py-2 px-4">
                          <div className="flex items-center justify-between">
                            <span className="text-sm font-bold text-slate-900">{currName}</span>
                            {isOpen ? (
                              <ChevronUp className="w-4 h-4 text-slate-600" />
                            ) : (
                              <ChevronDown className="w-4 h-4 text-slate-600" />
                            )}
                          </div>
                        </td>
                      </tr>

                      {/* Currency Rate Rows */}
                      {isOpen &&
                        rates.map((rateItem) => (
                          <tr
                            key={rateItem.id}
                            className="border-b border-slate-100 hover:bg-sky-50/50 transition"
                          >
                            <td className="py-2.5 px-4 text-slate-800 font-medium">{rateItem.currencyName}</td>
                            <td className="py-2.5 px-4 text-slate-900 font-bold font-mono text-base">
                              {rateItem.rate.toLocaleString()}
                            </td>
                            <td className="py-2.5 px-4 text-slate-600 font-mono text-xs">
                              {rateItem.minRate !== undefined || rateItem.maxRate !== undefined ? (
                                <span className="inline-flex items-center gap-1 bg-amber-50 text-amber-800 px-2 py-0.5 rounded border border-amber-200 font-bold">
                                  <span>{rateItem.minRate ?? '—'}</span>
                                  <span>←</span>
                                  <span>{rateItem.maxRate ?? '—'}</span>
                                </span>
                              ) : (
                                <span className="text-slate-400">غير محدد</span>
                              )}
                            </td>
                            <td className="py-2.5 px-4 text-slate-600 font-mono text-xs">
                              {rateItem.fromDate}
                            </td>
                            <td className="py-2.5 px-4 text-slate-600 font-mono text-xs">
                              <div className="flex items-center justify-between">
                                <span>{rateItem.toDate || ''}</span>
                                {onDeleteCurrencyRate && (
                                  <button
                                    onClick={() => onDeleteCurrencyRate(rateItem.id)}
                                    className="text-slate-400 hover:text-rose-600 p-1 rounded transition"
                                    title="حذف السعر"
                                  >
                                    <Trash2 className="w-4 h-4" />
                                  </button>
                                )}
                              </div>
                            </td>
                          </tr>
                        ))}
                    </React.Fragment>
                  );
                })}
              </tbody>
            </table>
          </div>
        )}
      </main>

      {/* 3. Bottom Blue Bar with Centered Round (+) Button (Exact Match to Screenshots) */}
      <footer className="bg-[#0277bd] h-14 flex items-center justify-center relative shadow-lg shrink-0">
        <button
          onClick={() => {
            if (activeMode === 'CURRENCIES') {
              setShowAddCurrencyModal(true);
            } else {
              setShowAddRateModal(true);
            }
          }}
          className="w-11 h-11 bg-white hover:bg-sky-50 text-[#0277bd] rounded-full flex items-center justify-center shadow-lg transition active:scale-95 cursor-pointer border-2 border-white"
          title={activeMode === 'CURRENCIES' ? 'إضافة عملة جديدة' : 'إضافة سعر عملة جديد'}
        >
          <Plus className="w-6 h-6 stroke-[3]" />
        </button>
      </footer>

      {/* Modal: Add Currency */}
      {showAddCurrencyModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60">
          <div className="bg-white rounded-xl max-w-sm w-full p-4 shadow-2xl border border-slate-200 text-right space-y-4">
            <div className="flex items-center justify-between border-b pb-2">
              <h3 className="font-bold text-sm text-slate-800 flex items-center gap-2">
                <Coins className="w-4 h-4 text-[#0277bd]" />
                <span>إضافة عملة جديدة</span>
              </h3>
              <button
                onClick={() => setShowAddCurrencyModal(false)}
                className="text-slate-400 hover:text-slate-600"
              >
                ✕
              </button>
            </div>

            <form onSubmit={handleSaveCurrency} className="space-y-3 text-xs">
              <div>
                <label className="block font-bold text-slate-700 mb-1">اسم العملة:</label>
                <input
                  type="text"
                  required
                  placeholder="مثال: يورو، درهم..."
                  value={newCurrName}
                  onChange={(e) => setNewCurrName(e.target.value)}
                  className="w-full border border-slate-300 rounded-lg p-2 focus:ring-2 focus:ring-[#0277bd] outline-hidden"
                />
              </div>

              <div>
                <label className="block font-bold text-slate-700 mb-1">الفكة (الكسور):</label>
                <input
                  type="text"
                  placeholder="مثال: سنت، فلس..."
                  value={newCurrSubUnit}
                  onChange={(e) => setNewCurrSubUnit(e.target.value)}
                  className="w-full border border-slate-300 rounded-lg p-2 focus:ring-2 focus:ring-[#0277bd] outline-hidden"
                />
              </div>

              <div>
                <label className="block font-bold text-slate-700 mb-1">رمز العملة الدولي:</label>
                <input
                  type="text"
                  placeholder="مثال: EUR, AED..."
                  value={newCurrCode}
                  onChange={(e) => setNewCurrCode(e.target.value)}
                  className="w-full border border-slate-300 rounded-lg p-2 font-mono uppercase focus:ring-2 focus:ring-[#0277bd] outline-hidden"
                />
              </div>

              <div className="flex justify-end gap-2 pt-2 border-t">
                <button
                  type="button"
                  onClick={() => setShowAddCurrencyModal(false)}
                  className="px-3 py-1.5 rounded-lg text-slate-600 hover:bg-slate-100 font-bold"
                >
                  إلغاء
                </button>
                <button
                  type="submit"
                  className="bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-1.5 rounded-lg font-bold shadow-xs cursor-pointer"
                >
                  حفظ العملة
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Modal: Add Currency Rate */}
      {showAddRateModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60">
          <div className="bg-white rounded-xl max-w-sm w-full p-4 shadow-2xl border border-slate-200 text-right space-y-4">
            <div className="flex items-center justify-between border-b pb-2">
              <h3 className="font-bold text-sm text-slate-800 flex items-center gap-2">
                <TrendingUp className="w-4 h-4 text-[#0277bd]" />
                <span>إضافة سعر صرف عملة</span>
              </h3>
              <button
                onClick={() => setShowAddRateModal(false)}
                className="text-slate-400 hover:text-slate-600"
              >
                ✕
              </button>
            </div>

            <form onSubmit={handleSaveRate} className="space-y-3 text-xs">
              <div>
                <label className="block font-bold text-slate-700 mb-1">العملة:</label>
                <select
                  value={newRateCurrency}
                  onChange={(e) => setNewRateCurrency(e.target.value)}
                  className="w-full border border-slate-300 rounded-lg p-2 bg-white focus:ring-2 focus:ring-[#0277bd] outline-hidden"
                >
                  {currencies
                    .filter((c) => !c.isLocal)
                    .map((c) => (
                      <option key={c.id} value={c.name}>
                        {c.name} ({c.code})
                      </option>
                    ))}
                </select>
              </div>

              <div>
                <label className="block font-bold text-slate-700 mb-1">سعر الصرف (مقابل المحلي):</label>
                <input
                  type="number"
                  step="any"
                  required
                  value={newRateValue}
                  onChange={(e) => {
                    setNewRateValue(e.target.value === '' ? '' : Number(e.target.value));
                    setRateRangeError(null);
                  }}
                  className="w-full border border-slate-300 rounded-lg p-2 font-mono text-sm focus:ring-2 focus:ring-[#0277bd] outline-hidden"
                />
              </div>

              {/* Min & Max Rate Allowed Range */}
              <div className="bg-amber-50/70 border border-amber-200 rounded-lg p-2.5">
                <span className="block font-bold text-amber-900 text-[11px] mb-1.5">
                  نطاق سعر الصرف المسموح به (لتقليل أخطاء الإدخال بالريال اليمني):
                </span>
                <div className="grid grid-cols-2 gap-2">
                  <div>
                    <label className="block text-slate-600 text-[10px] mb-0.5">أقل سعر صرف مسموح:</label>
                    <input
                      type="number"
                      step="any"
                      placeholder="مثلاً: 500"
                      value={newRateMin}
                      onChange={(e) => {
                        setNewRateMin(e.target.value === '' ? '' : Number(e.target.value));
                        setRateRangeError(null);
                      }}
                      className="w-full border border-amber-300 bg-white rounded-md p-1.5 font-mono text-xs focus:ring-2 focus:ring-amber-500 outline-hidden"
                    />
                  </div>
                  <div>
                    <label className="block text-slate-600 text-[10px] mb-0.5">أعلى سعر صرف مسموح:</label>
                    <input
                      type="number"
                      step="any"
                      placeholder="مثلاً: 850"
                      value={newRateMax}
                      onChange={(e) => {
                        setNewRateMax(e.target.value === '' ? '' : Number(e.target.value));
                        setRateRangeError(null);
                      }}
                      className="w-full border border-amber-300 bg-white rounded-md p-1.5 font-mono text-xs focus:ring-2 focus:ring-amber-500 outline-hidden"
                    />
                  </div>
                </div>
              </div>

              {rateRangeError && (
                <div className="bg-rose-50 border border-rose-200 text-rose-700 p-2 rounded-lg text-xs font-bold animate-shake">
                  {rateRangeError}
                </div>
              )}

              <div className="grid grid-cols-2 gap-2">
                <div>
                  <label className="block font-bold text-slate-700 mb-1">من تاريخ:</label>
                  <input
                    type="date"
                    required
                    value={newRateFromDate}
                    onChange={(e) => setNewRateFromDate(e.target.value)}
                    className="w-full border border-slate-300 rounded-lg p-1.5 text-xs font-mono"
                  />
                </div>
                <div>
                  <label className="block font-bold text-slate-700 mb-1">إلى تاريخ (اختياري):</label>
                  <input
                    type="date"
                    value={newRateToDate}
                    onChange={(e) => setNewRateToDate(e.target.value)}
                    className="w-full border border-slate-300 rounded-lg p-1.5 text-xs font-mono"
                  />
                </div>
              </div>

              <div className="flex justify-end gap-2 pt-2 border-t">
                <button
                  type="button"
                  onClick={() => setShowAddRateModal(false)}
                  className="px-3 py-1.5 rounded-lg text-slate-600 hover:bg-slate-100 font-bold"
                >
                  إلغاء
                </button>
                <button
                  type="submit"
                  className="bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-1.5 rounded-lg font-bold shadow-xs cursor-pointer"
                >
                  حفظ السعر
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};
