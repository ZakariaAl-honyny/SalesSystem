'use client';

import React, { useState } from 'react';
import { ProductItem } from '@/types/pos';
import { Upload, FileCheck, Copy } from 'lucide-react';

export interface ImportItemPayload {
  productId: string;
  qty: number;
  unit: string;
  price: number;
  discount: number;
}

export interface SalesImportConvertModalProps {
  products: ProductItem[];
  selectedCurrency: string;
  onImportItems: (sourceTitle: string, items: ImportItemPayload[]) => void;
  onClose: () => void;
}

export function SalesImportConvertModal({
  products,
  selectedCurrency,
  onImportItems,
  onClose,
}: SalesImportConvertModalProps) {
  const [importSourceType, setImportSourceType] = useState<'QUOTATION' | 'PAST_INVOICE'>('QUOTATION');

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/70 backdrop-blur-xs">
      <div className="bg-white rounded-xl max-w-2xl w-full p-4 shadow-2xl border border-slate-300 text-right space-y-3 max-h-[90vh] flex flex-col">
        <div className="flex items-center justify-between border-b pb-2 shrink-0">
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-lg bg-indigo-100 text-indigo-700 flex items-center justify-center font-black">
              <Upload className="w-5 h-5" />
            </div>
            <div>
              <h3 className="font-bold text-sm text-slate-900">التحويل والاستيراد التلقائي إلى الفاتورة</h3>
              <p className="text-[11px] text-slate-500">استورد فوراً من عروض الأسعار المسجلة، أو كرر فاتورة سابقة لعميل، أو استورد من ملف</p>
            </div>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="text-slate-400 hover:text-slate-600 p-1 rounded-md text-base font-bold cursor-pointer"
          >
            ✕
          </button>
        </div>

        {/* Import Tabs */}
        <div className="flex items-center gap-2 border-b pb-2 shrink-0">
          <button
            type="button"
            onClick={() => setImportSourceType('QUOTATION')}
            className={`px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              importSourceType === 'QUOTATION'
                ? 'bg-indigo-600 text-white shadow-xs'
                : 'bg-slate-100 hover:bg-slate-200 text-slate-700'
            }`}
          >
            <FileCheck className="w-3.5 h-3.5" />
            <span>عروض الأسعار المعتمدة</span>
          </button>

          <button
            type="button"
            onClick={() => setImportSourceType('PAST_INVOICE')}
            className={`px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              importSourceType === 'PAST_INVOICE'
                ? 'bg-indigo-600 text-white shadow-xs'
                : 'bg-slate-100 hover:bg-slate-200 text-slate-700'
            }`}
          >
            <Copy className="w-3.5 h-3.5" />
            <span>تكرار فاتورة سابقة</span>
          </button>
        </div>

        {/* Tab 1: Quotations List */}
        {importSourceType === 'QUOTATION' && (
          <div className="flex-1 overflow-y-auto space-y-2">
            <div className="p-3 rounded-lg border border-indigo-200 bg-indigo-50/50 hover:bg-indigo-50 transition flex items-center justify-between">
              <div className="space-y-1">
                <div className="flex items-center gap-2">
                  <span className="font-bold text-xs text-indigo-950">عرض سعر #QT-2026-001</span>
                  <span className="text-[10px] bg-emerald-100 text-emerald-800 px-1.5 py-0.5 rounded font-bold">معتمد وموافق عليه</span>
                </div>
                <p className="text-xs text-slate-600 font-bold">العميل: شركة الأمل للمقاولات والتجارة</p>
                <p className="text-[11px] text-slate-500">يتضمن 3 أصناف بإجمالي: 24.50 {selectedCurrency}</p>
              </div>
              <button
                type="button"
                onClick={() => {
                  onImportItems('عرض سعر QT-2026-001', [
                    { productId: products[0]?.id || '1', qty: 20, unit: 'حبة', price: products[0]?.salePrice || 1, discount: 0 },
                    { productId: products[1]?.id || '2', qty: 5, unit: 'حبة', price: products[1]?.salePrice || 2, discount: 0 },
                    { productId: products[2]?.id || '3', qty: 10, unit: 'حبة', price: products[2]?.salePrice || 1.5, discount: 0.5 },
                  ]);
                  onClose();
                }}
                className="bg-indigo-600 hover:bg-indigo-700 text-white px-3 py-2 rounded-lg text-xs font-bold transition flex items-center gap-1 shadow-xs cursor-pointer"
              >
                <Upload className="w-3.5 h-3.5" />
                <span>تحويل للفاتورة فوراً</span>
              </button>
            </div>

            <div className="p-3 rounded-lg border border-slate-200 bg-white hover:bg-slate-50 transition flex items-center justify-between">
              <div className="space-y-1">
                <div className="flex items-center gap-2">
                  <span className="font-bold text-xs text-slate-900">عرض سعر #QT-2026-002</span>
                  <span className="text-[10px] bg-amber-100 text-amber-800 px-1.5 py-0.5 rounded font-bold">بانتظار التعميد</span>
                </div>
                <p className="text-xs text-slate-600 font-bold">العميل: مؤسسة البركة للتسويق</p>
                <p className="text-[11px] text-slate-500">يتضمن صنفين بإجمالي: 12.00 {selectedCurrency}</p>
              </div>
              <button
                type="button"
                onClick={() => {
                  onImportItems('عرض سعر QT-2026-002', [
                    { productId: products[0]?.id || '1', qty: 12, unit: 'باكت', price: 0.80, discount: 0 },
                    { productId: products[1]?.id || '2', qty: 2, unit: 'كرتون', price: 14.00, discount: 1 },
                  ]);
                  onClose();
                }}
                className="bg-slate-800 hover:bg-slate-700 text-white px-3 py-2 rounded-lg text-xs font-bold transition flex items-center gap-1 shadow-xs cursor-pointer"
              >
                <Upload className="w-3.5 h-3.5" />
                <span>استيراد للفاتورة</span>
              </button>
            </div>
          </div>
        )}

        {/* Tab 2: Past Invoices */}
        {importSourceType === 'PAST_INVOICE' && (
          <div className="flex-1 overflow-y-auto space-y-2">
            <div className="p-3 rounded-lg border border-slate-200 bg-white hover:bg-slate-50 transition flex items-center justify-between">
              <div className="space-y-1">
                <span className="font-bold text-xs text-slate-900">فاتورة سابقة #INV-2026-889</span>
                <p className="text-xs text-slate-600">العميل: مجموعة العطير التجارية (تاريخ: أمس)</p>
                <p className="text-[11px] text-slate-500">أصناف الفاتورة: بطاطس عمان + عصير برتقال</p>
              </div>
              <button
                type="button"
                onClick={() => {
                  onImportItems('فاتورة سابقة INV-889', [
                    { productId: products[0]?.id || '1', qty: 50, unit: 'حبة', price: 0.07143, discount: 0 },
                    { productId: products[1]?.id || '2', qty: 10, unit: 'حبة', price: 1.25, discount: 0 },
                  ]);
                  onClose();
                }}
                className="bg-indigo-600 hover:bg-indigo-700 text-white px-3 py-2 rounded-lg text-xs font-bold transition flex items-center gap-1 shadow-xs cursor-pointer"
              >
                <Copy className="w-3.5 h-3.5" />
                <span>تكرار الأصناف هنا</span>
              </button>
            </div>
          </div>
        )}

        <div className="border-t pt-2 shrink-0 flex items-center justify-end">
          <button
            type="button"
            onClick={onClose}
            className="px-4 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-md font-bold text-xs cursor-pointer"
          >
            إلغاء
          </button>
        </div>
      </div>
    </div>
  );
}
