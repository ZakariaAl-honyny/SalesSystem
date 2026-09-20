'use client';

import React from 'react';
import { Info, X, Calendar, Calculator } from 'lucide-react';
import { ProductItem } from '@/types/pos';

export interface PurchaseItemModalProps {
  isOpen: boolean;
  modalProduct: ProductItem | null;
  activeModalField: 'quantity' | 'unitPrice' | 'total';
  setActiveModalField: (field: 'quantity' | 'unitPrice' | 'total') => void;
  modalQty: string;
  modalPrice: string;
  modalTotal: string;
  modalUnit: string;
  itemModalMode: 'ADD' | 'EDIT';
  handleAppendZeros: () => void;
  handleItemKeypadPress: (key: string) => void;
  handleSaveItemModal: (closeAfter: boolean) => void;
  setShowItemInfoModal: (val: boolean) => void;
  setShowOtherDetailsModal: (val: boolean) => void;
  onClose: () => void;
}

export const PurchaseItemModal: React.FC<PurchaseItemModalProps> = ({
  isOpen,
  modalProduct,
  activeModalField,
  setActiveModalField,
  modalQty,
  modalPrice,
  modalTotal,
  modalUnit,
  itemModalMode,
  handleAppendZeros,
  handleItemKeypadPress,
  handleSaveItemModal,
  setShowItemInfoModal,
  setShowOtherDetailsModal,
  onClose,
}) => {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-3 bg-black/60 animate-in fade-in">
      <div className="bg-white rounded-2xl max-w-sm w-full p-4 shadow-2xl border border-slate-200 text-right space-y-3">
        {/* Header: Item Name with (x) close on left, (i) info on right */}
        <div className="flex items-center justify-between border-b pb-2">
          <button
            type="button"
            onClick={() => setShowItemInfoModal(true)}
            className="text-slate-400 hover:text-sky-600 transition p-1"
            title="معلومات الصنف"
          >
            <Info className="w-5 h-5" />
          </button>

          <h3 className="font-bold text-base text-slate-900 truncate">
            {modalProduct?.name || 'تفاصيل الصنف'}
          </h3>

          <button
            type="button"
            onClick={onClose}
            className="text-slate-400 hover:text-slate-600 transition p-1 cursor-pointer"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* Input Row 1: الكمية with +000 and تاريخ الإنتهاء buttons (Screenshot 1, 5, 8) */}
        <div className="flex items-center gap-2">
          <span className="w-16 font-bold text-xs text-slate-700">الكمية</span>

          <div
            onClick={() => setActiveModalField('quantity')}
            className={`flex-1 flex items-center border-b-2 py-1 px-1 transition cursor-pointer ${
              activeModalField === 'quantity' ? 'border-[#e91e63]' : 'border-slate-300'
            }`}
          >
            <span className="font-mono font-bold text-base text-slate-900 flex-1">
              {modalQty}
            </span>
          </div>

          {/* +000 Button */}
          <button
            type="button"
            onClick={handleAppendZeros}
            className="px-2 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded text-xs font-bold font-mono transition cursor-pointer"
          >
            +000
          </button>

          {/* تاريخ الإنتهاء Button */}
          <button
            type="button"
            onClick={() => setShowOtherDetailsModal(true)}
            className="flex items-center gap-1 px-2 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded text-[11px] font-bold transition cursor-pointer"
          >
            <Calendar className="w-3.5 h-3.5" />
            <span>تاريخ الإنتهاء</span>
          </button>
        </div>

        {/* Input Row 2: السعر with +000 button */}
        <div className="flex items-center gap-2">
          <span className="w-16 font-bold text-xs text-slate-700">السعر</span>

          <div
            onClick={() => setActiveModalField('unitPrice')}
            className={`flex-1 flex items-center border-b-2 py-1 px-1 transition cursor-pointer ${
              activeModalField === 'unitPrice' ? 'border-[#e91e63]' : 'border-slate-300'
            }`}
          >
            <span className="font-mono font-bold text-base text-slate-900 flex-1">
              {modalPrice}
            </span>
          </div>

          {/* +000 Button */}
          <button
            type="button"
            onClick={handleAppendZeros}
            className="px-2 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded text-xs font-bold font-mono transition cursor-pointer"
          >
            +000
          </button>
        </div>

        {/* Input Row 3: أو الإجمالي with Calculator icon */}
        <div className="flex items-center gap-2">
          <span className="w-16 font-bold text-xs text-slate-700">أو الإجمالي</span>

          <div
            onClick={() => setActiveModalField('total')}
            className={`flex-1 flex items-center border-b-2 py-1 px-1 transition cursor-pointer ${
              activeModalField === 'total' ? 'border-[#e91e63]' : 'border-slate-300'
            }`}
          >
            <span className="font-mono font-bold text-base text-slate-900 flex-1">
              {modalTotal}
            </span>
            <Calculator className="w-4 h-4 text-slate-400" />
          </div>
        </div>

        {/* Row 4: Unit display & تفاصيل أخرى button */}
        <div className="flex items-center justify-between text-xs pt-1">
          <span className="font-bold text-slate-800 bg-slate-100 px-3 py-1 rounded">
            {modalUnit}
          </span>

          <button
            type="button"
            onClick={() => setShowOtherDetailsModal(true)}
            className="text-[#0277bd] font-bold hover:underline cursor-pointer"
          >
            تفاصيل أخرى
          </button>
        </div>

        {/* Row 5: Action Buttons (Screenshots 1, 5, 8) */}
        <div className="flex items-center gap-2 pt-2 border-t">
          {itemModalMode === 'ADD' ? (
            <>
              <button
                type="button"
                onClick={() => handleSaveItemModal(false)}
                className="flex-1 bg-[#0277bd] hover:bg-[#01579b] text-white font-bold py-2 rounded-lg text-xs transition cursor-pointer"
              >
                إضافة
              </button>
              <button
                type="button"
                onClick={onClose}
                className="flex-1 bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold py-2 rounded-lg text-xs transition cursor-pointer"
              >
                إلغاء
              </button>
              <button
                type="button"
                onClick={() => handleSaveItemModal(true)}
                className="flex-1 bg-emerald-600 hover:bg-emerald-700 text-white font-bold py-2 rounded-lg text-xs transition cursor-pointer"
              >
                إغلاق+
              </button>
            </>
          ) : (
            <>
              <button
                type="button"
                onClick={() => handleSaveItemModal(true)}
                className="flex-[2] bg-emerald-600 hover:bg-emerald-700 text-white font-bold py-2 rounded-lg text-xs transition cursor-pointer"
              >
                حفظ التعديلات
              </button>
              <button
                type="button"
                onClick={onClose}
                className="flex-1 bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold py-2 rounded-lg text-xs transition cursor-pointer"
              >
                إلغاء
              </button>
            </>
          )}
        </div>

        {/* Numeric Keypad for Items */}
        <div className="grid grid-cols-4 gap-2 pt-2 dir-ltr">
          {['1', '2', '3'].map((n) => (
            <button
              key={n}
              type="button"
              onClick={() => handleItemKeypadPress(n)}
              className="py-2.5 bg-white hover:bg-sky-50 active:bg-sky-100 rounded-lg font-bold font-mono text-slate-800 text-base shadow-sm transition"
            >
              {n}
            </button>
          ))}
          
          {/* Backspace Button */}
          <button
            type="button"
            onClick={() => handleItemKeypadPress('BACKSPACE')}
            className="py-2.5 bg-rose-50 hover:bg-rose-100 active:bg-rose-200 text-rose-600 rounded-lg font-bold text-xs shadow-sm transition flex items-center justify-center cursor-pointer"
          >
            ←
          </button>

          {['4', '5', '6'].map((n) => (
            <button
              key={n}
              type="button"
              onClick={() => handleItemKeypadPress(n)}
              className="py-2.5 bg-white hover:bg-sky-50 active:bg-sky-100 rounded-lg font-bold font-mono text-slate-800 text-base shadow-sm transition"
            >
              {n}
            </button>
          ))}

          {/* Next Button */}
          <button
            type="button"
            onClick={() => handleItemKeypadPress('Next')}
            className="py-2.5 bg-[#0277bd] text-white hover:bg-[#01579b] rounded-lg font-bold text-xs shadow-sm transition cursor-pointer"
          >
            Next
          </button>

          {['7', '8', '9', '.'].map((n) => (
            <button
              key={n}
              type="button"
              onClick={() => handleItemKeypadPress(n)}
              className="py-2.5 bg-white hover:bg-sky-50 active:bg-sky-100 rounded-lg font-bold font-mono text-slate-800 text-base shadow-sm transition"
            >
              {n}
            </button>
          ))}

          <div />
          <button
            type="button"
            onClick={() => handleItemKeypadPress('0')}
            className="py-2.5 bg-white hover:bg-sky-50 active:bg-sky-100 rounded-lg font-bold font-mono text-slate-800 text-base shadow-sm transition"
          >
            0
          </button>
          <div />
          <button
            type="button"
            onClick={() => handleItemKeypadPress('CLEAR')}
            className="py-2.5 bg-slate-100 hover:bg-slate-200 rounded-lg font-bold text-slate-600 text-xs shadow-sm transition cursor-pointer"
          >
            C
          </button>
        </div>
      </div>
    </div>
  );
};
