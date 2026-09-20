import React from 'react';
import {
  X, Wallet, Calendar, FileText, CheckCircle2,
  ArrowDownLeft, ArrowUpRight, AlertCircle, Landmark
} from 'lucide-react';
import { AccountNode, Customer, Supplier, CostCenterNode } from '@/types/pos';
import { tafqeet } from '@/lib/tafqeet';

interface CashVoucherNewModalProps {
  showNewModal: boolean;
  setShowNewModal: (v: boolean) => void;
  newVoucherType: 'RECEIPT' | 'PAYMENT';
  setNewVoucherType: (v: 'RECEIPT' | 'PAYMENT') => void;
  handleSaveVoucher: (e: React.FormEvent) => void;
  formDate: string;
  setFormDate: (v: string) => void;
  formPaymentMethod: 'CASH' | 'BANK';
  handlePaymentMethodSwitch: (method: 'CASH' | 'BANK') => void;
  formCashAccountCode: string;
  setFormCashAccountCode: (v: string) => void;
  formPartyType: 'CUSTOMER' | 'SUPPLIER' | 'EXPENSE' | 'OTHER';
  handlePartyTypeChange: (newPartyType: 'CUSTOMER' | 'SUPPLIER' | 'EXPENSE' | 'OTHER') => void;
  formSelectedPartyId: string;
  handlePartySelect: (id: string) => void;
  formPartyName: string;
  setFormPartyName: (v: string) => void;
  formContraAccountCode: string;
  setFormContraAccountCode: (v: string) => void;
  formCostCenterId: string;
  setFormCostCenterId: (v: string) => void;
  formAmount: string;
  setFormAmount: (v: string) => void;
  formReference: string;
  setFormReference: (v: string) => void;
  formDescription: string;
  setFormDescription: (v: string) => void;
  formError: string | null;
  selectableTreasuryAccounts: AccountNode[];
  contraEligibleAccounts: AccountNode[];
  customers: Customer[];
  suppliers: Supplier[];
  expenseAccounts: AccountNode[];
  leafCostCenters: CostCenterNode[];
  currency: string;
}

export const CashVoucherNewModal: React.FC<CashVoucherNewModalProps> = ({
  showNewModal,
  setShowNewModal,
  newVoucherType,
  setNewVoucherType,
  handleSaveVoucher,
  formDate,
  setFormDate,
  formPaymentMethod,
  handlePaymentMethodSwitch,
  formCashAccountCode,
  setFormCashAccountCode,
  formPartyType,
  handlePartyTypeChange,
  formSelectedPartyId,
  handlePartySelect,
  formPartyName,
  setFormPartyName,
  formContraAccountCode,
  setFormContraAccountCode,
  formCostCenterId,
  setFormCostCenterId,
  formAmount,
  setFormAmount,
  formReference,
  setFormReference,
  formDescription,
  setFormDescription,
  formError,
  selectableTreasuryAccounts,
  contraEligibleAccounts,
  customers,
  suppliers,
  expenseAccounts,
  leafCostCenters,
  currency,
}) => {
  if (!showNewModal) return null;

  return (
    <div className="fixed inset-0 bg-black/60 z-50 flex items-center justify-center p-3 backdrop-blur-xs">
      <div className="bg-white rounded-lg shadow-2xl border border-slate-400 w-full max-w-2xl overflow-hidden flex flex-col animate-in fade-in zoom-in-95 duration-150 max-h-[92vh]">
        {/* Modal Title Bar */}
        <div
          className={`p-3 text-white flex items-center justify-between shrink-0 ${
            newVoucherType === 'RECEIPT' ? 'bg-[#064e3b]' : 'bg-[#7f1d1d]'
          }`}
        >
          <div className="flex items-center gap-2 font-bold text-sm">
            {newVoucherType === 'RECEIPT' ? (
              <>
                <ArrowDownLeft className="w-4 h-4 text-emerald-300" />
                <span>إصدار سند قبض نقدي / بنكي جديد (New Receipt Voucher)</span>
              </>
            ) : (
              <>
                <ArrowUpRight className="w-4 h-4 text-rose-300" />
                <span>إصدار سند صرف نقدي / بنكي جديد (New Payment Voucher)</span>
              </>
            )}
          </div>
          <button
            type="button"
            onClick={() => setShowNewModal(false)}
            className="text-white/80 hover:text-white p-1 hover:bg-white/10 rounded cursor-pointer"
          >
            <X className="w-4 h-4" />
          </button>
        </div>

        {/* Modal Body Form */}
        <form onSubmit={handleSaveVoucher} className="p-3.5 space-y-3 overflow-y-auto flex-1 text-xs">
          {formError && (
            <div className="p-2.5 bg-rose-50 border border-rose-300 text-rose-800 rounded font-semibold flex items-center gap-2">
              <AlertCircle className="w-4 h-4 shrink-0 text-rose-600" />
              <span>{formError}</span>
            </div>
          )}

          {/* Type Switcher & Method Selector */}
          <div className="grid grid-cols-2 gap-2 p-2 bg-slate-100 rounded border border-slate-200">
            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">نوع السند:</label>
              <div className="grid grid-cols-2 gap-1">
                <button
                  type="button"
                  onClick={() => {
                    setNewVoucherType('RECEIPT');
                    handlePartyTypeChange('CUSTOMER');
                  }}
                  className={`py-1.5 rounded text-xs font-bold transition flex items-center justify-center gap-1 cursor-pointer ${
                    newVoucherType === 'RECEIPT'
                      ? 'bg-emerald-700 text-white shadow-xs'
                      : 'bg-white text-slate-700 border border-slate-300'
                  }`}
                >
                  <ArrowDownLeft className="w-3.5 h-3.5" />
                  <span>سند قبض</span>
                </button>

                <button
                  type="button"
                  onClick={() => {
                    setNewVoucherType('PAYMENT');
                    handlePartyTypeChange('SUPPLIER');
                  }}
                  className={`py-1.5 rounded text-xs font-bold transition flex items-center justify-center gap-1 cursor-pointer ${
                    newVoucherType === 'PAYMENT'
                      ? 'bg-rose-700 text-white shadow-xs'
                      : 'bg-white text-slate-700 border border-slate-300'
                  }`}
                >
                  <ArrowUpRight className="w-3.5 h-3.5" />
                  <span>سند صرف</span>
                </button>
              </div>
            </div>

            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">طريقة الدفع:</label>
              <div className="grid grid-cols-2 gap-1">
                <button
                  type="button"
                  onClick={() => handlePaymentMethodSwitch('CASH')}
                  className={`py-1.5 rounded text-xs font-bold transition flex items-center justify-center gap-1 cursor-pointer ${
                    formPaymentMethod === 'CASH'
                      ? 'bg-amber-700 text-white shadow-xs'
                      : 'bg-white text-slate-700 border border-slate-300'
                  }`}
                >
                  <Wallet className="w-3.5 h-3.5" />
                  <span>نقداً (صندوق 111)</span>
                </button>

                <button
                  type="button"
                  onClick={() => handlePaymentMethodSwitch('BANK')}
                  className={`py-1.5 rounded text-xs font-bold transition flex items-center justify-center gap-1 cursor-pointer ${
                    formPaymentMethod === 'BANK'
                      ? 'bg-indigo-700 text-white shadow-xs'
                      : 'bg-white text-slate-700 border border-slate-300'
                  }`}
                >
                  <Landmark className="w-3.5 h-3.5" />
                  <span>بنك / محفظة (112)</span>
                </button>
              </div>
            </div>
          </div>

          {/* Date & Cash/Bank Account */}
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-2">
            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">
                تاريخ السند: <span className="text-rose-500">*</span>
              </label>
              <div className="relative">
                <Calendar className="w-3.5 h-3.5 text-slate-400 absolute right-2.5 top-2.5" />
                <input
                  type="date"
                  value={formDate}
                  onChange={(e) => setFormDate(e.target.value)}
                  required
                  className="w-full pl-2 pr-8 py-1.5 bg-slate-50 border border-slate-300 rounded font-mono text-xs focus:bg-white focus:outline-none focus:border-sky-500"
                />
              </div>
            </div>

            <div>
              <div className="flex items-center justify-between mb-1">
                <label className="text-[11px] font-bold text-slate-700">
                  حساب {formPaymentMethod === 'CASH' ? 'الصندوق / الخزينة' : 'البنك'}: <span className="text-rose-500">*</span>
                </label>
                <span className="text-[10px] text-sky-700 font-mono font-bold bg-sky-50 px-1 rounded border border-sky-200">
                  {formPaymentMethod === 'CASH' ? 'مفلتر كود 111' : 'مفلتر كود 112'}
                </span>
              </div>
              <select
                value={formCashAccountCode}
                onChange={(e) => setFormCashAccountCode(e.target.value)}
                required
                className="w-full p-1.5 bg-slate-50 border border-slate-300 rounded text-xs font-semibold focus:bg-white focus:outline-none focus:border-sky-500"
              >
                {selectableTreasuryAccounts.map((acc) => (
                  <option key={acc.code} value={acc.code}>
                    {acc.code} - {acc.name} (الرصيد: {acc.balance.toLocaleString()} {currency})
                  </option>
                ))}
              </select>
            </div>
          </div>

          {/* Party Type & Party Selection */}
          <div className="p-2.5 bg-slate-50 border border-slate-200 rounded space-y-2">
            <div className="flex items-center justify-between">
              <label className="text-[11px] font-bold text-slate-800">
                الجهة {newVoucherType === 'RECEIPT' ? 'المستلم منها (الدافع)' : 'المصروف لها (المستلم)'}:
              </label>
              <div className="flex items-center gap-2">
                <label className="inline-flex items-center gap-1 cursor-pointer">
                  <input
                    type="radio"
                    name="partyType"
                    checked={formPartyType === 'CUSTOMER'}
                    onChange={() => handlePartyTypeChange('CUSTOMER')}
                    className="text-sky-600 focus:ring-0"
                  />
                  <span className="text-[11px]">عميل</span>
                </label>

                <label className="inline-flex items-center gap-1 cursor-pointer">
                  <input
                    type="radio"
                    name="partyType"
                    checked={formPartyType === 'SUPPLIER'}
                    onChange={() => handlePartyTypeChange('SUPPLIER')}
                    className="text-sky-600 focus:ring-0"
                  />
                  <span className="text-[11px]">مورد</span>
                </label>

                <label className="inline-flex items-center gap-1 cursor-pointer">
                  <input
                    type="radio"
                    name="partyType"
                    checked={formPartyType === 'EXPENSE'}
                    onChange={() => handlePartyTypeChange('EXPENSE')}
                    className="text-sky-600 focus:ring-0"
                  />
                  <span className="text-[11px]">حساب مصروف</span>
                </label>

                <label className="inline-flex items-center gap-1 cursor-pointer">
                  <input
                    type="radio"
                    name="partyType"
                    checked={formPartyType === 'OTHER'}
                    onChange={() => handlePartyTypeChange('OTHER')}
                    className="text-sky-600 focus:ring-0"
                  />
                  <span className="text-[11px]">أخرى / يدوي</span>
                </label>
              </div>
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-2 pt-1">
              {formPartyType === 'CUSTOMER' ? (
                <div>
                  <label className="block text-[10px] text-slate-600 font-semibold mb-1">اختر العميل:</label>
                  <select
                    value={formSelectedPartyId}
                    onChange={(e) => handlePartySelect(e.target.value)}
                    className="w-full p-1.5 bg-white border border-slate-300 rounded text-xs font-semibold"
                  >
                    {customers.map((c) => (
                      <option key={c.id} value={c.id}>
                        {c.name} (رصيد آجل: {c.currentBalance.toLocaleString()} {currency})
                      </option>
                    ))}
                  </select>
                </div>
              ) : formPartyType === 'SUPPLIER' ? (
                <div>
                  <label className="block text-[10px] text-slate-600 font-semibold mb-1">اختر المورد:</label>
                  <select
                    value={formSelectedPartyId}
                    onChange={(e) => handlePartySelect(e.target.value)}
                    className="w-full p-1.5 bg-white border border-slate-300 rounded text-xs font-semibold"
                  >
                    {suppliers.map((s) => (
                      <option key={s.id} value={s.id}>
                        {s.name} (مستحق: {s.currentBalance.toLocaleString()} {currency})
                      </option>
                    ))}
                  </select>
                </div>
              ) : formPartyType === 'EXPENSE' ? (
                <div>
                  <label className="block text-[10px] text-slate-600 font-semibold mb-1">اختر حساب المصروف:</label>
                  <select
                    value={formSelectedPartyId}
                    onChange={(e) => handlePartySelect(e.target.value)}
                    className="w-full p-1.5 bg-white border border-slate-300 rounded text-xs font-semibold"
                  >
                    {expenseAccounts.map((e) => (
                      <option key={e.code} value={e.code}>
                        {e.code} - {e.name}
                      </option>
                    ))}
                  </select>
                </div>
              ) : null}

              <div>
                <label className="block text-[10px] text-slate-600 font-semibold mb-1">
                  اسم الجهة المكتوب في السند: <span className="text-rose-500">*</span>
                </label>
                <input
                  type="text"
                  value={formPartyName}
                  onChange={(e) => setFormPartyName(e.target.value)}
                  placeholder="اسم العميل أو المورد أو المستلم..."
                  required
                  className="w-full p-1.5 bg-white border border-slate-300 rounded text-xs focus:outline-none focus:border-sky-500"
                />
              </div>
            </div>

            {/* Contra Account and Cost Center */}
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-2 pt-1 border-t border-slate-200">
              <div>
                <div className="flex items-center justify-between mb-1">
                  <label className="text-[10px] text-slate-600 font-semibold">
                    الحساب المحاسبي المقابل: <span className="text-rose-500">*</span>
                  </label>
                  <span className="text-[9px] text-amber-700 bg-amber-50 px-1 rounded border border-amber-200">
                    مستبعد الصناديق والبنوك
                  </span>
                </div>
                <select
                  value={formContraAccountCode}
                  onChange={(e) => setFormContraAccountCode(e.target.value)}
                  required
                  className="w-full p-1.5 bg-white border border-slate-300 rounded text-xs"
                >
                  {contraEligibleAccounts.map((acc) => (
                    <option key={acc.code} value={acc.code}>
                      {acc.code} - {acc.name} ({acc.nature === 'DEBIT' ? 'مدين' : 'دائن'})
                    </option>
                  ))}
                </select>
                <p className="text-[9px] text-slate-400 mt-0.5">
                  لتحويل الأموال بين صندوق وبنك استخدم زر التحويل المخصص.
                </p>
              </div>

              <div>
                <label className="block text-[10px] text-slate-600 font-semibold mb-1">
                  مركز التكلفة (اختياري):
                </label>
                <select
                  value={formCostCenterId}
                  onChange={(e) => setFormCostCenterId(e.target.value)}
                  className="w-full p-1.5 bg-white border border-slate-300 rounded text-xs"
                >
                  <option value="">-- بدون مركز تكلفة --</option>
                  {leafCostCenters.map((c) => (
                    <option key={c.id} value={c.id}>
                      {c.code} - {c.name}
                    </option>
                  ))}
                </select>
              </div>
            </div>
          </div>

          {/* Amount & Reference */}
          <div className="grid grid-cols-1 sm:grid-cols-3 gap-2">
            <div className="sm:col-span-1">
              <label className="block text-[11px] font-bold text-slate-700 mb-1">
                المبلغ: <span className="text-rose-500">*</span>
              </label>
              <div className="relative">
                <input
                  type="number"
                  step="any"
                  min="1"
                  value={formAmount}
                  onChange={(e) => setFormAmount(e.target.value)}
                  placeholder="0.00"
                  required
                  className="w-full pl-8 pr-2 py-1.5 bg-slate-50 border border-slate-300 rounded font-mono-numbers font-bold text-sm text-slate-900 focus:bg-white focus:outline-none focus:border-sky-500"
                />
                <span className="absolute left-2.5 top-2 text-[11px] text-slate-500 font-bold">{currency}</span>
              </div>
            </div>

            <div className="sm:col-span-2">
              <label className="block text-[11px] font-bold text-slate-700 mb-1">
                رقم المرجع / الشيك / الحوالة:
              </label>
              <input
                type="text"
                value={formReference}
                onChange={(e) => setFormReference(e.target.value)}
                placeholder="مثال: شيك رقم 44012 أو إشعار تحويل 889..."
                className="w-full p-1.5 bg-slate-50 border border-slate-300 rounded text-xs focus:bg-white focus:outline-none focus:border-sky-500"
              />
            </div>
          </div>

          {/* Tafqeet Preview Banner */}
          {parseFloat(formAmount) > 0 && (
            <div className="p-2 bg-amber-50 border border-amber-200 rounded text-amber-900 text-xs flex items-center gap-2">
              <FileText className="w-3.5 h-3.5 text-amber-700 shrink-0" />
              <span className="font-semibold">{tafqeet(parseFloat(formAmount), currency)}</span>
            </div>
          )}

          {/* Description */}
          <div>
            <label className="block text-[11px] font-bold text-slate-700 mb-1">البيان / الشرح التفصيلي:</label>
            <textarea
              rows={2}
              value={formDescription}
              onChange={(e) => setFormDescription(e.target.value)}
              placeholder="اكتب شرح العملية المحاسبية وسند الاستلام أو الصرف..."
              className="w-full p-1.5 bg-slate-50 border border-slate-300 rounded text-xs focus:bg-white focus:outline-none focus:border-sky-500"
            />
          </div>

          {/* Journal Entry Auto-Preview Strip */}
          <div className="p-2 bg-sky-50/70 border border-sky-200 rounded text-sky-950 text-[11px]">
            <div className="font-bold flex items-center justify-between mb-1">
              <span>معاينة القيد المحاسبي المولد آلياً (Automated Journal Entry):</span>
              <span className="font-mono text-[10px] text-sky-700">قيد ثنائي متوازن</span>
            </div>
            {newVoucherType === 'RECEIPT' ? (
              <div className="space-y-0.5 font-mono text-[11px]">
                <div className="text-emerald-800">
                  من حـ/ {formCashAccountCode} (الصندوق/البنك) : {formAmount || '0'} {currency} (مدين)
                </div>
                <div className="text-slate-700 mr-4">
                  إلى حـ/ {formContraAccountCode} ({formPartyName || 'الحساب المقابل'}) : {formAmount || '0'} {currency} (دائن)
                </div>
              </div>
            ) : (
              <div className="space-y-0.5 font-mono text-[11px]">
                <div className="text-rose-800">
                  من حـ/ {formContraAccountCode} ({formPartyName || 'المورد/المصروف'}) : {formAmount || '0'} {currency} (مدين)
                </div>
                <div className="text-slate-700 mr-4">
                  إلى حـ/ {formCashAccountCode} (الصندوق/البنك) : {formAmount || '0'} {currency} (دائن)
                </div>
              </div>
            )}
          </div>

          {/* Footer Actions */}
          <div className="flex items-center justify-end gap-2 pt-2 border-t border-slate-200 shrink-0">
            <button
              type="button"
              onClick={() => setShowNewModal(false)}
              className="px-4 py-1.5 border border-slate-300 rounded text-slate-700 hover:bg-slate-100 font-bold cursor-pointer"
            >
              إلغاء الأمر
            </button>
            <button
              type="submit"
              className={`px-5 py-1.5 rounded font-bold text-white shadow-xs transition flex items-center gap-1.5 cursor-pointer ${
                newVoucherType === 'RECEIPT'
                  ? 'bg-emerald-600 hover:bg-emerald-500'
                  : 'bg-rose-600 hover:bg-rose-500'
              }`}
            >
              <CheckCircle2 className="w-3.5 h-3.5" />
              <span>حفظ وترحيل السند وطباعته</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
