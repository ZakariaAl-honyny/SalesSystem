'use client';

import React from 'react';

export interface PurchaseOtherDetailsModalProps {
  isOpen: boolean;
  expiryDate: string;
  onExpiryDateChange: (val: string) => void;
  unit: string;
  onUnitChange: (val: string) => void;
  onClose: () => void;
}

export const PurchaseOtherDetailsModal: React.FC<PurchaseOtherDetailsModalProps> = ({
  isOpen,
  expiryDate,
  onExpiryDateChange,
  unit,
  onUnitChange,
  onClose,
}) => {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 animate-in fade-in">
      <div className="bg-white rounded-xl max-w-xs w-full p-4 shadow-2xl border border-slate-200 text-right space-y-3">
        <h3 className="font-bold text-sm text-slate-900 border-b pb-2">
          تفاصيل إضافية للصنف
        </h3>
        <div className="space-y-2 text-xs">
          <div>
            <label className="block font-bold text-slate-700 mb-1">تاريخ الإنتهاء:</label>
            <input
              type="date"
              value={expiryDate}
              onChange={(e) => onExpiryDateChange(e.target.value)}
              className="w-full border border-slate-300 rounded p-1.5 text-xs font-mono"
            />
          </div>
          <div>
            <label className="block font-bold text-slate-700 mb-1">رقم الدفعة / التشغيلة (Batch):</label>
            <input
              type="text"
              placeholder="رقم الدفعة..."
              className="w-full border border-slate-300 rounded p-1.5 text-xs"
            />
          </div>
          <div>
            <label className="block font-bold text-slate-700 mb-1">الوحدة:</label>
            <select
              value={unit}
              onChange={(e) => onUnitChange(e.target.value)}
              className="w-full border border-slate-300 rounded p-1.5 text-xs font-bold"
            >
              <option value="حبة">حبة</option>
              <option value="باكت">باكت</option>
              <option value="كرتون">كرتون</option>
            </select>
          </div>
        </div>
        <div className="pt-2 flex justify-end">
          <button
            type="button"
            onClick={onClose}
            className="bg-[#0277bd] text-white text-xs font-bold px-4 py-1.5 rounded-lg cursor-pointer"
          >
            تأكيد
          </button>
        </div>
      </div>
    </div>
  );
};
