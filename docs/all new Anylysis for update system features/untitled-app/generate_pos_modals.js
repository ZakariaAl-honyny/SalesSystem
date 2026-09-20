const fs = require('fs');

const createModalFile = (name, content) => {
  fs.writeFileSync(`components/pos/${name}.tsx`, content);
};

createModalFile('POSHeldOrdersModal', `import React from 'react';
import { PauseCircle, RotateCcw } from 'lucide-react';
import { HeldOrder } from '@/types/pos';

interface POSHeldOrdersModalProps {
  showHeldModal: boolean;
  setShowHeldModal: (v: boolean) => void;
  filteredHeldOrders: HeldOrder[];
  heldOrders: HeldOrder[];
  handleRecallHeldOrder: (id: string) => void;
  currency: string;
}

export const POSHeldOrdersModal: React.FC<POSHeldOrdersModalProps> = ({
  showHeldModal, setShowHeldModal, filteredHeldOrders, heldOrders, handleRecallHeldOrder, currency
}) => {
  if (!showHeldModal) return null;
  return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-xs">
          <div className="bg-white rounded-xl max-w-lg w-full max-h-[85vh] flex flex-col shadow-2xl border border-slate-200 overflow-hidden animate-in fade-in zoom-in-95 duration-150">
            <div className="bg-slate-100 border-b border-slate-200 px-4 py-3 flex items-center justify-between shrink-0">
              <div className="flex items-center gap-2">
                <PauseCircle className="w-5 h-5 text-amber-500" />
                <h3 className="font-bold text-slate-800 text-sm">استرجاع الفواتير المعلقة (F4)</h3>
              </div>
              <div className="flex items-center gap-2">
                <input
                  type="text"
                  placeholder="بحث برقم أو اسم..."
                  className="bg-white border border-slate-300 rounded px-2 py-1 text-xs focus:border-sky-500 outline-none w-32 sm:w-48"
                  // onChange left un-implemented in original or we pass it
                />
                <button
                  type="button"
                  onClick={() => setShowHeldModal(false)}
                  className="text-slate-400 hover:text-slate-700 bg-white border border-slate-200 p-1 rounded-md"
                >
                  ✕
                </button>
              </div>
            </div>

            <div className="p-3 overflow-y-auto flex flex-col gap-2.5 text-xs flex-1">
              {filteredHeldOrders.length === 0 ? (
                <div className="text-center py-12 text-slate-400 flex flex-col items-center justify-center gap-2">
                  <PauseCircle className="w-8 h-8 text-slate-300" />
                  <span className="font-semibold text-slate-600">لا توجد فواتير معلقة حالياً</span>
                  <span className="text-[11px] text-slate-400">
                    عند وقوف زبون، يمكنك تعليق فاتورته بالضغط على [F4] لخدمة الزبون التالي فوراً
                  </span>
                </div>
              ) : (
                filteredHeldOrders.map((h) => {
                  const totalAmt = h.grandTotal || h.items.reduce((s, i) => s + i.total, 0);
                  const itemCount = h.items.reduce((s, i) => s + i.quantity, 0);
                  return (
                    <div
                      key={h.id}
                      className="bg-slate-50 p-3 rounded-lg border border-slate-300 hover:border-sky-500 hover:bg-sky-50/40 transition flex flex-col sm:flex-row sm:items-center justify-between gap-3 shadow-xs"
                    >
                      <div className="flex-1">
                        <div className="flex items-center gap-2 mb-1">
                          <span className="font-bold text-slate-900 text-sm">{h.customerName}</span>
                          <span className="bg-amber-100 text-amber-900 text-[10px] font-bold px-1.5 py-0.2 rounded border border-amber-300 font-mono">
                            {h.id}
                          </span>
                        </div>
                        <div className="text-slate-500 text-[11px] flex items-center gap-3">
                          <span>وقت التعليق: <strong className="text-slate-700">{h.heldAt}</strong></span>
                          <span>•</span>
                          <span>الأصناف: <strong className="text-slate-700">{itemCount} قطعة ({h.items.length} أصناف)</strong></span>
                        </div>
                        <div className="mt-1.5 text-[11px] text-slate-600 bg-white p-1.5 rounded border border-slate-200 line-clamp-1">
                          {h.items.map((it) => \`\${it.item.name} (\${it.quantity})\`).join(' ، ')}
                        </div>
                      </div>

                      <div className="flex sm:flex-col items-center sm:items-end justify-between sm:justify-center gap-2 shrink-0 border-t sm:border-t-0 pt-2 sm:pt-0 border-slate-200">
                        <div className="font-mono-numbers font-black text-sky-800 text-sm sm:text-base">
                          {totalAmt.toLocaleString()} {currency}
                        </div>
                        <button
                          type="button"
                          onClick={() => handleRecallHeldOrder(h.id)}
                          className="bg-sky-700 hover:bg-sky-800 text-white px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 shadow-sm cursor-pointer"
                        >
                          <RotateCcw className="w-3.5 h-3.5" />
                          <span>استرجاع للسلة</span>
                        </button>
                      </div>
                    </div>
                  );
                })
              )}
            </div>

            <div className="bg-slate-100 p-2.5 border-t border-slate-200 flex items-center justify-between shrink-0">
              <span className="text-[11px] text-slate-500">
                إجمالي المعلق: <strong className="text-slate-800">{heldOrders.length} فواتير</strong>
              </span>
              <button
                onClick={() => setShowHeldModal(false)}
                className="bg-slate-200 hover:bg-slate-300 text-slate-800 text-xs px-4 py-1.5 rounded font-semibold cursor-pointer"
              >
                إغلاق
              </button>
            </div>
          </div>
        </div>
  );
};
`);

createModalFile('POSDiscountModal', `import React from 'react';
import { Tag } from 'lucide-react';

interface POSDiscountModalProps {
  showDiscountModal: boolean;
  setShowDiscountModal: (v: boolean) => void;
  discountType: 'AMOUNT' | 'PERCENT';
  setDiscountType: (v: 'AMOUNT' | 'PERCENT') => void;
  discountOverall: number;
  setDiscountOverall: (v: number) => void;
  taxPercentVal: string;
  setTaxPercentVal: (v: string) => void;
  setIsTaxEnabled: (v: boolean) => void;
  otherChargesAccountCode: string;
  setOtherChargesAccountCode: (v: string) => void;
  setOtherChargesName: (v: string) => void;
  EXPENSE_ACCOUNTS_OPTIONS: any[];
  otherChargesVal: string;
  setOtherChargesVal: (v: string) => void;
  subtotal: number;
  totalDiscount: number;
  calculatedTax: number;
  otherCharges: number;
  otherChargesName: string;
  grandTotal: number;
  currency: string;
}

export const POSDiscountModal: React.FC<POSDiscountModalProps> = ({
  showDiscountModal, setShowDiscountModal, discountType, setDiscountType, discountOverall, setDiscountOverall,
  taxPercentVal, setTaxPercentVal, setIsTaxEnabled, otherChargesAccountCode, setOtherChargesAccountCode,
  setOtherChargesName, EXPENSE_ACCOUNTS_OPTIONS, otherChargesVal, setOtherChargesVal, subtotal, totalDiscount,
  calculatedTax, otherCharges, otherChargesName, grandTotal, currency
}) => {
  if (!showDiscountModal) return null;
  return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-xs">
          <div className="bg-white rounded-xl max-w-sm w-full p-4 shadow-2xl border border-slate-200 text-right space-y-3 animate-in fade-in zoom-in-95 duration-150">
            <div className="flex items-center justify-between border-b pb-2">
              <h3 className="font-bold text-sm text-slate-800 flex items-center gap-1.5">
                <Tag className="w-4 h-4 text-[#0277bd]" />
                <span>الخصم والمصاريف والدفع [F7]</span>
              </h3>
              <button
                type="button"
                onClick={() => setShowDiscountModal(false)}
                className="text-slate-400 hover:text-slate-600 p-1 rounded-md cursor-pointer"
              >
                ✕
              </button>
            </div>

            <div className="flex items-center justify-center gap-6 text-xs font-bold text-slate-800 py-1 bg-slate-50 rounded-lg border border-slate-200">
              <label className="flex items-center gap-1.5 cursor-pointer">
                <input
                  type="radio"
                  name="posDiscRadio"
                  checked={discountType === 'AMOUNT'}
                  onChange={() => setDiscountType('AMOUNT')}
                  className="w-4 h-4 text-[#0277bd]"
                />
                <span>مبلغ</span>
              </label>
              <label className="flex items-center gap-1.5 cursor-pointer">
                <input
                  type="radio"
                  name="posDiscRadio"
                  checked={discountType === 'PERCENT'}
                  onChange={() => setDiscountType('PERCENT')}
                  className="w-4 h-4 text-[#0277bd]"
                />
                <span>نسبة</span>
              </label>
            </div>

            <div className="space-y-2 text-xs">
              <div className="flex items-center justify-between gap-2">
                <span className="font-bold text-slate-700 w-24">الخصم التجاري</span>
                <input
                  type="number"
                  min={0}
                  value={discountOverall || ''}
                  onChange={(e) => setDiscountOverall(parseFloat(e.target.value) || 0)}
                  placeholder="0"
                  className="flex-1 text-center py-1.5 font-bold font-mono rounded border border-slate-300 bg-slate-50 focus:bg-white focus:border-[#0277bd] outline-none"
                />
              </div>

              <div className="flex items-center justify-between gap-2">
                <span className="font-bold text-slate-700 w-24">الضريبة%</span>
                <select
                  value={taxPercentVal}
                  onChange={(e) => {
                    setTaxPercentVal(e.target.value);
                    setIsTaxEnabled(parseFloat(e.target.value) > 0);
                  }}
                  className="flex-1 text-center py-1.5 font-bold font-mono rounded border border-slate-300 bg-slate-50 focus:bg-white focus:border-[#0277bd] outline-none cursor-pointer"
                >
                  <option value="0">0 % (معفي من الضريبة)</option>
                  <option value="5">5 % (ضريبة مبيعات عامة)</option>
                  <option value="15">15 % (ضريبة القيمة المضافة)</option>
                </select>
              </div>

              <div className="flex items-center justify-between gap-2">
                <span className="font-bold text-slate-700 w-24">حساب المصروف</span>
                <div className="flex-1 flex items-center gap-1">
                  <select
                    value={otherChargesAccountCode}
                    onChange={(e) => {
                      const code = e.target.value;
                      setOtherChargesAccountCode(code);
                      const match = EXPENSE_ACCOUNTS_OPTIONS.find((a: any) => a.code === code);
                      if (match) setOtherChargesName(match.name);
                    }}
                    className="flex-1 text-right py-1.5 px-2 font-bold rounded border border-slate-300 bg-slate-50 focus:bg-white focus:border-[#0277bd] outline-none text-xs cursor-pointer"
                  >
                    {EXPENSE_ACCOUNTS_OPTIONS.map((acc: any) => (
                      <option key={acc.code} value={acc.code}>
                        {acc.code} - {acc.name}
                      </option>
                    ))}
                  </select>
                  <input
                    type="number"
                    min={0}
                    value={otherChargesVal || ''}
                    onChange={(e) => setOtherChargesVal(e.target.value)}
                    placeholder="0"
                    className="w-20 text-center py-1.5 font-bold font-mono rounded border border-slate-300 bg-slate-50 focus:bg-white focus:border-[#0277bd] outline-none text-xs"
                  />
                </div>
              </div>

              <div className="bg-slate-50 p-2.5 rounded-lg border border-slate-200 space-y-1 text-[11px]">
                <div className="flex items-center justify-between text-slate-600">
                  <span>المجموع الفرعي:</span>
                  <span className="font-mono font-bold">{subtotal.toLocaleString()} {currency}</span>
                </div>
                {totalDiscount > 0 && (
                  <div className="flex items-center justify-between text-rose-600 font-semibold">
                    <span>إجمالي الخصم:</span>
                    <span className="font-mono font-bold">-{totalDiscount.toLocaleString()} {currency}</span>
                  </div>
                )}
                {calculatedTax > 0 && (
                  <div className="flex items-center justify-between text-emerald-700 font-semibold">
                    <span>مبلغ الضريبة ({taxPercentVal}%):</span>
                    <span className="font-mono font-bold">+{calculatedTax.toLocaleString()} {currency}</span>
                  </div>
                )}
                {otherCharges > 0 && (
                  <div className="flex items-center justify-between text-sky-700 font-semibold">
                    <span>{otherChargesName || 'رسوم إضافية'}:</span>
                    <span className="font-mono font-bold">+{otherCharges.toLocaleString()} {currency}</span>
                  </div>
                )}
                <div className="flex items-center justify-between pt-1 border-t border-slate-200 text-xs font-black text-slate-900">
                  <span>الصافي الإجمالي:</span>
                  <span className="font-mono text-emerald-700 font-black text-sm">{grandTotal.toLocaleString()} {currency}</span>
                </div>
              </div>

              <div className="flex items-center gap-2 pt-2 border-t border-slate-200">
                <button
                  type="button"
                  onClick={() => setShowDiscountModal(false)}
                  className="flex-1 bg-[#0277bd] hover:bg-[#01579b] text-white py-2 rounded-lg font-bold text-xs shadow-sm transition cursor-pointer"
                >
                  موافق
                </button>
                <button
                  type="button"
                  onClick={() => setShowDiscountModal(false)}
                  className="flex-1 bg-slate-100 hover:bg-slate-200 text-slate-700 py-2 rounded-lg font-bold text-xs transition cursor-pointer"
                >
                  إلغاء
                </button>
              </div>
            </div>
          </div>
        </div>
  );
};
`);

fs.writeFileSync('components/pos/index.ts', `export * from './POSHeldOrdersModal';
export * from './POSDiscountModal';
`);

let moduleContent = fs.readFileSync('components/POSModule.tsx', 'utf8');

// Insert imports
const importRegex = /import React, \{ useState, useRef, useEffect, useCallback, useMemo \} from 'react';/;
const newImports = `import React, { useState, useRef, useEffect, useCallback, useMemo } from 'react';
import {
  POSHeldOrdersModal,
  POSDiscountModal,
} from './pos';`;
moduleContent = moduleContent.replace(importRegex, newImports);

const startIndex = moduleContent.indexOf('{showHeldModal && (');
const endIndex = moduleContent.lastIndexOf('</>\n      )}\n    </div>\n  );\n};');

if (startIndex === -1 || endIndex === -1) {
  console.log('Error: Could not find pos modal slice');
  process.exit(1);
}

const replacementJSX = `<POSHeldOrdersModal
        showHeldModal={showHeldModal}
        setShowHeldModal={setShowHeldModal}
        filteredHeldOrders={filteredHeldOrders}
        heldOrders={heldOrders}
        handleRecallHeldOrder={handleRecallHeldOrder}
        currency={currency}
      />

      {/* ============================================================== */}
      {/* MODAL: Touch Discount, Tax % & Other Charges (Screenshot 10)  */}
      {/* ============================================================== */}
      <POSDiscountModal
        showDiscountModal={showDiscountModal}
        setShowDiscountModal={setShowDiscountModal}
        discountType={discountType}
        setDiscountType={setDiscountType}
        discountOverall={discountOverall}
        setDiscountOverall={setDiscountOverall}
        taxPercentVal={taxPercentVal}
        setTaxPercentVal={setTaxPercentVal}
        setIsTaxEnabled={setIsTaxEnabled}
        otherChargesAccountCode={otherChargesAccountCode}
        setOtherChargesAccountCode={setOtherChargesAccountCode}
        setOtherChargesName={setOtherChargesName}
        EXPENSE_ACCOUNTS_OPTIONS={EXPENSE_ACCOUNTS_OPTIONS}
        otherChargesVal={otherChargesVal}
        setOtherChargesVal={setOtherChargesVal}
        subtotal={subtotal}
        totalDiscount={totalDiscount}
        calculatedTax={calculatedTax}
        otherCharges={otherCharges}
        otherChargesName={otherChargesName}
        grandTotal={grandTotal}
        currency={currency}
      />
    `;

const newModuleContent = moduleContent.substring(0, startIndex) + replacementJSX + moduleContent.substring(endIndex);
fs.writeFileSync('components/POSModule.tsx', newModuleContent);
console.log('POSModule.tsx refactored.');
