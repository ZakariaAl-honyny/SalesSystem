'use client';

import React, { useState } from 'react';
import { CurrencyItem, CurrencyRateItem } from '@/types/pos';
import { Coins, Check, AlertCircle, ArrowLeftRight } from 'lucide-react';

export interface CurrencySelectionModalProps {
  isOpen?: boolean;
  showCurrencyModal?: boolean;
  onClose?: () => void;
  setShowCurrencyModal?: (show: boolean) => void;
  selectedCurrency: string;
  setSelectedCurrency?: (curr: string) => void;
  exchangeRate?: number;
  onSelect?: (currencyName: string, rate: number) => void;
  currencies?: CurrencyItem[];
  currencyRates?: CurrencyRateItem[];
  currency?: string;
  localCurrencyName?: string;
}

export const CurrencySelectionModal: React.FC<CurrencySelectionModalProps> = (props) => {
  const isModalOpen = props.isOpen ?? props.showCurrencyModal ?? false;
  if (!isModalOpen) return null;

  return <CurrencySelectionModalContent key={`${props.selectedCurrency}-${props.exchangeRate || 1}`} {...props} />;
};

const CurrencySelectionModalContent: React.FC<CurrencySelectionModalProps> = ({
  isOpen,
  showCurrencyModal,
  onClose,
  setShowCurrencyModal,
  selectedCurrency,
  setSelectedCurrency,
  exchangeRate = 1,
  onSelect,
  currencies = [
    { id: 'CUR-01', name: 'محلي', subUnit: 'فلس', code: 'YR', accountsCount: 2, isLocal: true },
    { id: 'CUR-02', name: 'دولار', subUnit: 'سنت', code: 'USD', accountsCount: 1, isLocal: false },
    { id: 'CUR-03', name: 'سعودي', subUnit: 'فلس', code: 'SAR', accountsCount: 0, isLocal: false },
  ],
  currencyRates = [
    { id: 'CR-01', currencyName: 'دولار', rate: 700, minRate: 500, maxRate: 850, fromDate: '2026-05-25', toDate: '', isCurrent: true },
    { id: 'CR-05', currencyName: 'سعودي', rate: 190, minRate: 130, maxRate: 250, fromDate: '2026-05-25', toDate: '', isCurrent: true },
  ],
  currency,
  localCurrencyName = currency || 'ريال يمني',
}) => {
  const [activeCurrency, setActiveCurrency] = useState<string>(selectedCurrency);
  const [customRate, setCustomRate] = useState<number | string>(exchangeRate || 1);
  const [validationError, setValidationError] = useState<string | null>(null);

  const handleClose = () => {
    if (onClose) onClose();
    if (setShowCurrencyModal) setShowCurrencyModal(false);
  };

  // Find the active rate record for the selected currency
  const currentRateObj = currencyRates.find(
    (r) => (r.currencyName === activeCurrency || r.currencyName.toLowerCase() === activeCurrency.toLowerCase()) && (r.isCurrent !== false)
  ) || currencyRates.find((r) => r.currencyName === activeCurrency);

  const isLocalSelected = activeCurrency === 'محلي' || activeCurrency === 'YR' || activeCurrency === 'YER' || activeCurrency === localCurrencyName;

  const handleCurrencyTabClick = (currName: string) => {
    setActiveCurrency(currName);
    setValidationError(null);
    if (currName === 'محلي' || currName === 'YR' || currName === 'YER' || currName === localCurrencyName) {
      setCustomRate(1);
    } else {
      const foundRate = currencyRates.find(
        (r) => (r.currencyName === currName || r.currencyName.toLowerCase() === currName.toLowerCase()) && (r.isCurrent !== false)
      ) || currencyRates.find((r) => r.currencyName === currName);
      setCustomRate(foundRate ? foundRate.rate : 1);
    }
  };

  const handleConfirm = () => {
    if (isLocalSelected) {
      if (onSelect) onSelect(activeCurrency, 1);
      if (setSelectedCurrency) setSelectedCurrency(activeCurrency);
      handleClose();
      return;
    }

    const numRate = Number(customRate);
    if (isNaN(numRate) || numRate <= 0) {
      setValidationError('يرجى إدخال سعر صرف صالح أكبر من الصفر');
      return;
    }

    // Min / Max validation if defined
    if (currentRateObj) {
      if (currentRateObj.minRate !== undefined && numRate < currentRateObj.minRate) {
        setValidationError(`تنبيه: سعر الصرف (${numRate}) أقل من الحد الأدنى المسموح به (${currentRateObj.minRate} ريال) لمنع أخطاء الإدخال`);
        return;
      }
      if (currentRateObj.maxRate !== undefined && numRate > currentRateObj.maxRate) {
        setValidationError(`تنبيه: سعر الصرف (${numRate}) أكبر من الحد الأقصى المسموح به (${currentRateObj.maxRate} ريال)`);
        return;
      }
    }

    if (onSelect) onSelect(activeCurrency, numRate);
    if (setSelectedCurrency) setSelectedCurrency(activeCurrency);
    handleClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-3 bg-black/60 backdrop-blur-xs animate-in fade-in" dir="rtl">
      <div className="bg-white rounded-2xl shadow-2xl border border-slate-200 max-w-md w-full overflow-hidden flex flex-col text-slate-800 animate-in zoom-in-95 duration-150">
        {/* Header */}
        <div className="bg-gradient-to-r from-[#0277bd] to-[#01579b] text-white px-4 py-3 flex items-center justify-between">
          <div className="flex items-center gap-2">
            <Coins className="w-5 h-5 text-sky-200" />
            <h3 className="font-bold text-sm">اختيار عملة الفاتورة وسعر الصرف</h3>
          </div>
          <button
            type="button"
            onClick={handleClose}
            className="text-sky-200 hover:text-white p-1 rounded-full text-xs font-bold"
          >
            ✕
          </button>
        </div>

        <div className="p-4 space-y-4">
          {/* Currency Selection Options */}
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-2">
              العملة المختارة:
            </label>
            <div className="grid grid-cols-3 gap-2">
              {currencies.map((c) => {
                const isSelected = activeCurrency === c.name || (c.isLocal && isLocalSelected);
                return (
                  <button
                    key={c.id}
                    type="button"
                    onClick={() => handleCurrencyTabClick(c.name)}
                    className={`p-2.5 rounded-xl border text-center font-bold text-xs transition cursor-pointer flex flex-col items-center gap-1 ${
                      isSelected
                        ? 'bg-sky-50 border-sky-500 text-sky-900 shadow-xs ring-2 ring-sky-500/20'
                        : 'bg-slate-50/70 border-slate-200 text-slate-700 hover:bg-slate-100'
                    }`}
                  >
                    <span className="text-sm font-black">{c.name}</span>
                    <span className="text-[10px] text-slate-500 font-mono">
                      {c.isLocal ? 'العملة الأساسية' : c.code}
                    </span>
                    {isSelected && (
                      <span className="w-2 h-2 rounded-full bg-sky-600 mt-0.5"></span>
                    )}
                  </button>
                );
              })}
            </div>
          </div>

          {/* Exchange Rate Box (Only for Foreign Currencies) */}
          {!isLocalSelected ? (
            <div className="bg-slate-50 border border-slate-200 rounded-xl p-3.5 space-y-3">
              <div className="flex items-center justify-between border-b border-slate-200 pb-2">
                <div className="flex items-center gap-1.5 text-xs font-bold text-slate-700">
                  <ArrowLeftRight className="w-4 h-4 text-sky-600" />
                  <span>سعر الصرف المعتمد للعملية:</span>
                </div>
                {currentRateObj && (
                  <span className="text-[11px] bg-sky-100 text-sky-800 px-2 py-0.5 rounded font-mono font-bold">
                    سعر النظام: {currentRateObj.rate} ريال
                  </span>
                )}
              </div>

              <div>
                <div className="flex items-center justify-between text-xs font-semibold text-slate-600 mb-1">
                  <span>1 {activeCurrency} = </span>
                  {currentRateObj && (currentRateObj.minRate !== undefined || currentRateObj.maxRate !== undefined) && (
                    <span className="text-[10px] text-slate-500 font-mono">
                      (الحدود المسموحة: {currentRateObj.minRate || 0} - {currentRateObj.maxRate || '∞'} ريال)
                    </span>
                  )}
                </div>

                <div className="flex items-center gap-2">
                  <div className="relative flex-1">
                    <input
                      type="number"
                      step="any"
                      min={currentRateObj?.minRate || 0}
                      max={currentRateObj?.maxRate || 999999}
                      value={customRate}
                      onChange={(e) => {
                        setCustomRate(e.target.value);
                        setValidationError(null);
                      }}
                      className="w-full bg-white border border-sky-300 rounded-lg px-3 py-2 text-sm font-black font-mono text-slate-900 focus:outline-hidden focus:ring-2 focus:ring-sky-500/30"
                      placeholder="أدخل سعر الصرف..."
                    />
                    <span className="absolute left-3 top-2.5 text-xs font-bold text-slate-400">
                      ريال يمني
                    </span>
                  </div>

                  {currentRateObj && (
                    <button
                      type="button"
                      onClick={() => {
                        setCustomRate(currentRateObj.rate);
                        setValidationError(null);
                      }}
                      className="px-2.5 py-2 bg-slate-200 hover:bg-slate-300 text-slate-700 rounded-lg text-xs font-bold transition shrink-0 cursor-pointer"
                      title="استعادة سعر صرف النظام الحالي"
                    >
                      استعادة
                    </button>
                  )}
                </div>
              </div>

              {/* Real-time equivalence feedback */}
              <div className="text-[11px] text-slate-600 bg-sky-50/70 rounded-lg p-2 border border-sky-100">
                <span className="font-bold text-sky-900">توضيح: </span>
                سيتم تقييد مبالغ الفاتورة بعملة ({activeCurrency}) وتحويل المعادل المحلي ({Number(customRate || 0).toLocaleString()} ريال/{activeCurrency}) في السجلات المحاسبية.
              </div>

              {/* Validation Warning */}
              {validationError && (
                <div className="bg-rose-50 border border-rose-300 text-rose-800 p-2 rounded-lg text-xs font-bold flex items-center gap-1.5 animate-in fade-in">
                  <AlertCircle className="w-4 h-4 text-rose-600 shrink-0" />
                  <span>{validationError}</span>
                </div>
              )}
            </div>
          ) : (
            <div className="bg-emerald-50 border border-emerald-200 rounded-xl p-3 text-emerald-900 text-xs font-semibold flex items-center gap-2">
              <Check className="w-4 h-4 text-emerald-600 shrink-0" />
              <span>العملة الأساسية للنظام (الريال المحلي) - عامل التحويل ثابت = 1.0000</span>
            </div>
          )}
        </div>

        {/* Footer Actions */}
        <div className="bg-slate-50 border-t border-slate-200 px-4 py-3 flex items-center justify-end gap-2">
          <button
            type="button"
            onClick={handleClose}
            className="px-4 py-2 bg-white hover:bg-slate-100 text-slate-700 rounded-lg font-bold text-xs border border-slate-300 transition cursor-pointer"
          >
            إلغاء
          </button>
          <button
            type="button"
            onClick={handleConfirm}
            className="px-5 py-2 bg-[#0277bd] hover:bg-[#01579b] text-white rounded-lg font-bold text-xs shadow-sm transition flex items-center gap-1 cursor-pointer"
          >
            <Check className="w-4 h-4" />
            <span>تأكيد واعتماد العملة</span>
          </button>
        </div>
      </div>
    </div>
  );
};
