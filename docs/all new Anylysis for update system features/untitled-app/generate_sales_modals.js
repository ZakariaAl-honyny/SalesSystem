const fs = require('fs');

const createModalFile = (name, content) => {
  fs.writeFileSync(`components/sales/${name}.tsx`, content);
};

createModalFile('SalesCurrencyModal', `import React from 'react';

interface SalesCurrencyModalProps {
  showCurrencyModal: boolean;
  setShowCurrencyModal: (v: boolean) => void;
  selectedCurrency: string;
  setSelectedCurrency: (v: string) => void;
  currency: string;
}

export const SalesCurrencyModal: React.FC<SalesCurrencyModalProps> = ({
  showCurrencyModal, setShowCurrencyModal, selectedCurrency, setSelectedCurrency, currency
}) => {
  if (!showCurrencyModal) return null;
  return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-xs">
          <div className="bg-white rounded-xl shadow-2xl border border-slate-200 p-4 max-w-sm w-full animate-in fade-in zoom-in-95 duration-150">
            <h3 className="font-bold text-slate-800 text-sm mb-3">اختر عملة الفاتورة</h3>
            <div className="space-y-2">
              <button
                type="button"
                onClick={() => {
                  setSelectedCurrency(currency);
                  setShowCurrencyModal(false);
                }}
                className={\`w-full p-3 rounded-lg border text-right font-bold transition \${
                  selectedCurrency === currency
                    ? 'bg-sky-50 border-sky-500 text-sky-800 shadow-sm'
                    : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50'
                }\`}
              >
                المحلي ({currency})
              </button>
              <button
                type="button"
                onClick={() => {
                  setSelectedCurrency('SAR');
                  setShowCurrencyModal(false);
                }}
                className={\`w-full p-3 rounded-lg border text-right font-bold transition \${
                  selectedCurrency === 'SAR'
                    ? 'bg-sky-50 border-sky-500 text-sky-800 shadow-sm'
                    : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50'
                }\`}
              >
                سعودي (SAR)
              </button>
              <button
                type="button"
                onClick={() => {
                  setSelectedCurrency('USD');
                  setShowCurrencyModal(false);
                }}
                className={\`w-full p-3 rounded-lg border text-right font-bold transition \${
                  selectedCurrency === 'USD'
                    ? 'bg-sky-50 border-sky-500 text-sky-800 shadow-sm'
                    : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50'
                }\`}
              >
                دولار أمريكي (USD)
              </button>
            </div>
            <button
              type="button"
              onClick={() => setShowCurrencyModal(false)}
              className="mt-4 w-full bg-slate-100 hover:bg-slate-200 text-slate-700 py-2 rounded font-bold text-xs"
            >
              إلغاء
            </button>
          </div>
        </div>
  );
};
`);

createModalFile('SalesItemKeypadModal', `import React from 'react';
import { Minus, Plus, RefreshCw, Layers } from 'lucide-react';
import { CartItem } from '@/types/pos';

interface SalesItemKeypadModalProps {
  selectedItemForEdit: CartItem | null;
  setSelectedItemForEdit: (v: CartItem | null) => void;
  keypadInput: string;
  setKeypadInput: (v: string) => void;
  keypadMode: 'QTY' | 'PRICE' | 'DISCOUNT';
  setKeypadMode: (v: 'QTY' | 'PRICE' | 'DISCOUNT') => void;
  handleItemKeypadPress: (key: string) => void;
  selectedCurrency: string;
}

export const SalesItemKeypadModal: React.FC<SalesItemKeypadModalProps> = ({
  selectedItemForEdit, setSelectedItemForEdit, keypadInput, setKeypadInput, keypadMode, setKeypadMode,
  handleItemKeypadPress, selectedCurrency
}) => {
  if (!selectedItemForEdit) return null;
  return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-xs">
          <div className="bg-white rounded-xl shadow-2xl border border-slate-200 p-4 max-w-sm w-full animate-in fade-in zoom-in-95 duration-150">
            <div className="flex items-center justify-between border-b pb-2 mb-3">
              <h3 className="font-bold text-sm text-slate-800 truncate pl-2">
                {selectedItemForEdit.item.name}
              </h3>
              <button
                type="button"
                onClick={() => setSelectedItemForEdit(null)}
                className="text-slate-400 hover:text-slate-600 p-1 bg-slate-100 rounded"
              >
                ✕
              </button>
            </div>

            <div className="grid grid-cols-3 gap-2 mb-3">
              <button
                type="button"
                onClick={() => {
                  setKeypadMode('QTY');
                  setKeypadInput(selectedItemForEdit.quantity.toString());
                }}
                className={\`py-2 rounded font-bold text-xs transition \${
                  keypadMode === 'QTY' ? 'bg-emerald-600 text-white shadow-sm' : 'bg-slate-100 text-slate-700'
                }\`}
              >
                الكمية ({selectedItemForEdit.quantity})
              </button>
              <button
                type="button"
                onClick={() => {
                  setKeypadMode('PRICE');
                  setKeypadInput(selectedItemForEdit.unitPrice.toString());
                }}
                className={\`py-2 rounded font-bold text-xs transition \${
                  keypadMode === 'PRICE' ? 'bg-[#0277bd] text-white shadow-sm' : 'bg-slate-100 text-slate-700'
                }\`}
              >
                السعر ({selectedItemForEdit.unitPrice})
              </button>
              <button
                type="button"
                onClick={() => {
                  setKeypadMode('DISCOUNT');
                  setKeypadInput(selectedItemForEdit.discount ? selectedItemForEdit.discount.toString() : '0');
                }}
                className={\`py-2 rounded font-bold text-[11px] transition \${
                  keypadMode === 'DISCOUNT' ? 'bg-amber-500 text-white shadow-sm' : 'bg-slate-100 text-slate-700'
                }\`}
              >
                الخصم ({selectedItemForEdit.discount || 0})
              </button>
            </div>

            <div className="bg-slate-50 border border-slate-200 rounded-lg p-3 text-right mb-4">
              <span className="text-2xl font-black font-mono text-slate-800 tracking-wider">
                {keypadInput || '0'}
              </span>
            </div>

            <div className="grid grid-cols-4 gap-2">
              <div className="col-span-3 grid grid-cols-3 gap-2">
                {[7, 8, 9, 4, 5, 6, 1, 2, 3].map((num) => (
                  <button
                    key={num}
                    type="button"
                    onClick={() => handleItemKeypadPress(num.toString())}
                    className="bg-white border border-slate-200 hover:bg-sky-50 hover:border-sky-300 text-slate-800 font-bold text-lg py-3 rounded-lg shadow-sm transition"
                  >
                    {num}
                  </button>
                ))}
                <button
                  type="button"
                  onClick={() => handleItemKeypadPress('00')}
                  className="bg-white border border-slate-200 hover:bg-sky-50 hover:border-sky-300 text-slate-800 font-bold text-lg py-3 rounded-lg shadow-sm transition"
                >
                  00
                </button>
                <button
                  type="button"
                  onClick={() => handleItemKeypadPress('0')}
                  className="bg-white border border-slate-200 hover:bg-sky-50 hover:border-sky-300 text-slate-800 font-bold text-lg py-3 rounded-lg shadow-sm transition"
                >
                  0
                </button>
                <button
                  type="button"
                  onClick={() => handleItemKeypadPress('.')}
                  className="bg-white border border-slate-200 hover:bg-sky-50 hover:border-sky-300 text-slate-800 font-bold text-lg py-3 rounded-lg shadow-sm transition"
                >
                  .
                </button>
              </div>
              <div className="col-span-1 grid grid-cols-1 gap-2">
                <button
                  type="button"
                  onClick={() => handleItemKeypadPress('CLEAR')}
                  className="bg-rose-100 hover:bg-rose-200 text-rose-700 font-bold text-xs py-3 rounded-lg transition"
                >
                  C
                </button>
                <button
                  type="button"
                  onClick={() => handleItemKeypadPress('+1')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold py-3 rounded-lg flex items-center justify-center transition"
                >
                  <Plus className="w-5 h-5" />
                </button>
                <button
                  type="button"
                  onClick={() => handleItemKeypadPress('-1')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold py-3 rounded-lg flex items-center justify-center transition"
                >
                  <Minus className="w-5 h-5" />
                </button>
                <button
                  type="button"
                  onClick={() => handleItemKeypadPress('ENTER')}
                  className={\`font-bold text-sm py-4 rounded-lg shadow-sm transition text-white \${
                    keypadMode === 'QTY' ? 'bg-emerald-600 hover:bg-emerald-500' :
                    keypadMode === 'PRICE' ? 'bg-[#0277bd] hover:bg-[#01579b]' :
                    'bg-amber-500 hover:bg-amber-400'
                  }\`}
                >
                  إدخال
                </button>
              </div>
            </div>

            {selectedItemForEdit.item.units && selectedItemForEdit.item.units.length > 0 && (
              <div className="mt-4 pt-3 border-t border-slate-200">
                <div className="text-xs font-bold text-slate-600 mb-2 flex items-center gap-1.5">
                  <Layers className="w-3.5 h-3.5" />
                  <span>تغيير الوحدة:</span>
                </div>
                <div className="grid grid-cols-3 gap-2">
                  <button
                    type="button"
                    onClick={() => {
                      if (selectedItemForEdit) {
                        setSelectedItemForEdit({ ...selectedItemForEdit, selectedUnit: selectedItemForEdit.item.unit });
                        if (keypadMode === 'PRICE') setKeypadInput(selectedItemForEdit.item.salePrice.toString());
                      }
                    }}
                    className={\`py-1.5 px-1 text-[10px] rounded font-bold transition border \${
                      selectedItemForEdit.selectedUnit === selectedItemForEdit.item.unit
                        ? 'bg-sky-50 border-sky-400 text-sky-800'
                        : 'bg-white border-slate-200 text-slate-600 hover:bg-slate-50'
                    }\`}
                  >
                    {selectedItemForEdit.item.unit}
                  </button>
                  {selectedItemForEdit.item.units.map((u: any, idx: number) => (
                    <button
                      key={idx}
                      type="button"
                      onClick={() => {
                        if (selectedItemForEdit) {
                          setSelectedItemForEdit({ ...selectedItemForEdit, selectedUnit: u.name });
                          if (keypadMode === 'PRICE') setKeypadInput(u.price.toString());
                        }
                      }}
                      className={\`py-1.5 px-1 text-[10px] rounded font-bold transition border \${
                        selectedItemForEdit.selectedUnit === u.name
                          ? 'bg-sky-50 border-sky-400 text-sky-800'
                          : 'bg-white border-slate-200 text-slate-600 hover:bg-slate-50'
                      }\`}
                    >
                      {u.name}
                    </button>
                  ))}
                </div>
              </div>
            )}
          </div>
        </div>
  );
};
`);

// update index.ts
fs.appendFileSync('components/sales/index.ts', `export * from './SalesCurrencyModal';
export * from './SalesItemKeypadModal';
`);

let moduleContent = fs.readFileSync('components/CommercialSalesInvoiceView.tsx', 'utf8');

// Insert imports
const importRegex = /import \{\n  SalesDiscountExpensesModal,\n  SalesCameraAttachmentModal,\n  SalesCustomerPickerModal,\n  SalesProfitBreakdownModal,\n  SalesSmartF3LookupModal,\n  SalesImportConvertModal,\n\} from '\.\/sales';/;
const newImports = `import {
  SalesDiscountExpensesModal,
  SalesCameraAttachmentModal,
  SalesCustomerPickerModal,
  SalesProfitBreakdownModal,
  SalesSmartF3LookupModal,
  SalesImportConvertModal,
  SalesCurrencyModal,
  SalesItemKeypadModal,
} from './sales';`;
moduleContent = moduleContent.replace(importRegex, newImports);

const startIndex = moduleContent.indexOf('{/* ============================================================== */}');
const endIndex = moduleContent.indexOf('{/* ============================================================== */}', moduleContent.indexOf('{/* MODAL: Touch Discount & Expenses Modal (Screenshot 10) */}'));

if (startIndex === -1 || endIndex === -1) {
  console.log('Error: Could not find sales modal slice');
  process.exit(1);
}

const replacementJSX = `{/* ============================================================== */}
      {/* MODAL: Currency Selection Modal */}
      {/* ============================================================== */}
      <SalesCurrencyModal
        showCurrencyModal={showCurrencyModal}
        setShowCurrencyModal={setShowCurrencyModal}
        selectedCurrency={selectedCurrency}
        setSelectedCurrency={setSelectedCurrency}
        currency={currency}
      />

      {/* ============================================================== */}
      {/* MODAL: Item Touch Modal with Keypad (Screenshots 1, 5, 8) */}
      {/* ============================================================== */}
      <SalesItemKeypadModal
        selectedItemForEdit={selectedItemForEdit}
        setSelectedItemForEdit={setSelectedItemForEdit}
        keypadInput={keypadInput}
        setKeypadInput={setKeypadInput}
        keypadMode={keypadMode}
        setKeypadMode={setKeypadMode}
        handleItemKeypadPress={handleItemKeypadPress}
        selectedCurrency={selectedCurrency}
      />\n\n      `;

const newModuleContent = moduleContent.substring(0, startIndex) + replacementJSX + moduleContent.substring(endIndex);
fs.writeFileSync('components/CommercialSalesInvoiceView.tsx', newModuleContent);
console.log('CommercialSalesInvoiceView.tsx refactored.');
