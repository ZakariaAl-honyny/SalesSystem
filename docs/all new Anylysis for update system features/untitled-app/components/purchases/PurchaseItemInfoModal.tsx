'use client';

import React from 'react';
import { Info } from 'lucide-react';
import { ProductItem } from '@/types/pos';

export interface PurchaseItemInfoModalProps {
  isOpen: boolean;
  product: ProductItem | null;
  currency: string;
  onClose: () => void;
}

export const PurchaseItemInfoModal: React.FC<PurchaseItemInfoModalProps> = ({
  isOpen,
  product,
  currency,
  onClose,
}) => {
  if (!isOpen || !product) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 animate-in fade-in">
      <div className="bg-white rounded-xl max-w-xs w-full p-4 shadow-2xl border border-slate-200 text-right space-y-3">
        <h3 className="font-bold text-sm text-slate-900 border-b pb-2 flex items-center gap-1.5">
          <Info className="w-4 h-4 text-[#0277bd]" />
          <span>بطاقة بيانات الصنف</span>
        </h3>
        <div className="space-y-1 text-xs text-slate-700">
          <p><span className="font-bold">اسم الصنف:</span> {product.name}</p>
          <p><span className="font-bold">الكود:</span> {product.code}</p>
          <p><span className="font-bold">الباركود:</span> {product.barcode}</p>
          <p><span className="font-bold">الوحدة:</span> {product.unit}</p>
          <p><span className="font-bold">سعر التكلفة:</span> {product.costPrice} {currency}</p>
          <p><span className="font-bold">سعر البيع:</span> {product.salePrice} {currency}</p>
          <p><span className="font-bold">المخزون المتوفر:</span> {product.stock} {product.unit}</p>
        </div>
        <div className="pt-2 flex justify-end">
          <button
            type="button"
            onClick={onClose}
            className="bg-[#0277bd] text-white text-xs font-bold px-4 py-1.5 rounded-lg cursor-pointer"
          >
            حسناً
          </button>
        </div>
      </div>
    </div>
  );
};
