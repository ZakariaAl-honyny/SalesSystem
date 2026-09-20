'use client';

import React from 'react';
import { TrendingUp } from 'lucide-react';

export interface SalesInvoiceProfitItem {
  id?: string;
  product: {
    id?: string;
    name: string;
    costPrice?: number;
    [key: string]: any;
  };
  quantity: number;
  unitPrice?: number;
  unitCost?: number;
  discount?: number;
  total: number;
  unit?: string;
  unitFactor?: number;
  [key: string]: any;
}

export interface SalesProfitBreakdownModalProps {
  customerName: string;
  isCash: boolean;
  totalCost: number;
  rawSubtotal: number;
  commercialDiscount: number;
  invoiceNetProfit: number;
  invoiceProfitMargin: number;
  selectedCurrency: string;
  invoiceItems: SalesInvoiceProfitItem[];
  onClose: () => void;
}

export function SalesProfitBreakdownModal({
  customerName,
  isCash,
  totalCost,
  rawSubtotal,
  commercialDiscount,
  invoiceNetProfit,
  invoiceProfitMargin,
  selectedCurrency,
  invoiceItems,
  onClose,
}: SalesProfitBreakdownModalProps) {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-xs">
      <div className="bg-white rounded-xl max-w-lg w-full p-4 shadow-2xl border border-slate-200 text-right space-y-3 max-h-[85vh] flex flex-col">
        <div className="flex items-center justify-between border-b pb-2 shrink-0">
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-lg bg-emerald-100 text-emerald-700 flex items-center justify-center">
              <TrendingUp className="w-5 h-5" />
            </div>
            <div>
              <h3 className="font-bold text-sm text-slate-800">تحليل أرباح العميل والفاتورة</h3>
              <p className="text-[11px] text-slate-500">
                العميل: <span className="font-bold text-slate-700">{customerName || 'عميل نقدي عام'}</span> | نوع السداد: <span className="font-bold text-emerald-600">{isCash ? 'نقدي (فوري)' : 'آجل (ذمم)'}</span>
              </p>
            </div>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="text-slate-400 hover:text-slate-600 p-1 rounded-md cursor-pointer"
          >
            ✕
          </button>
        </div>

        {/* Profit Metrics Cards */}
        <div className="grid grid-cols-3 gap-2 shrink-0">
          <div className="bg-slate-50 p-2.5 rounded-lg border border-slate-200 text-center">
            <div className="text-[10px] text-slate-500 font-bold">إجمالي التكلفة</div>
            <div className="text-sm font-mono font-bold text-slate-800 mt-1">
              {totalCost.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 4 })}
            </div>
            <div className="text-[9px] text-slate-400">{selectedCurrency}</div>
          </div>

          <div className="bg-sky-50 p-2.5 rounded-lg border border-sky-200 text-center">
            <div className="text-[10px] text-sky-700 font-bold">صافي المبيعات</div>
            <div className="text-sm font-mono font-bold text-sky-900 mt-1">
              {(rawSubtotal - commercialDiscount).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 4 })}
            </div>
            <div className="text-[9px] text-sky-500">{selectedCurrency}</div>
          </div>

          <div className={`p-2.5 rounded-lg border text-center ${invoiceNetProfit >= 0 ? 'bg-emerald-50 border-emerald-300' : 'bg-rose-50 border-rose-300'}`}>
            <div className={`text-[10px] font-bold ${invoiceNetProfit >= 0 ? 'text-emerald-800' : 'text-rose-800'}`}>
              صافي الربح
            </div>
            <div className={`text-sm font-mono font-black mt-1 ${invoiceNetProfit >= 0 ? 'text-emerald-600' : 'text-rose-600'}`}>
              {invoiceNetProfit.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 4 })}
            </div>
            <div className="text-[9px] font-bold text-emerald-700 font-mono">
              هامش: {invoiceProfitMargin.toFixed(1)}%
            </div>
          </div>
        </div>

        {/* Item-by-item Profit Breakdown */}
        <div className="flex-1 overflow-y-auto border border-slate-200 rounded-lg">
          <table className="w-full text-right text-xs">
            <thead className="bg-slate-100 text-slate-600 sticky top-0 font-bold text-[11px]">
              <tr>
                <th className="p-2">الصنف</th>
                <th className="p-2 text-center">الكمية</th>
                <th className="p-2 text-left">التكلفة</th>
                <th className="p-2 text-left">سعر البيع</th>
                <th className="p-2 text-left">ربح الصنف</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100">
              {invoiceItems.map((item, index) => {
                const factor = item.unitFactor || 1;
                const itemUnitCost = item.unitCost || (item.product.costPrice || 0) * factor;
                const itemTotalCost = itemUnitCost * item.quantity;
                const itemNetProfit = item.total - itemTotalCost;
                const itemMargin = item.total > 0 ? (itemNetProfit / item.total) * 100 : 0;

                return (
                  <tr key={item.id || index} className="hover:bg-slate-50 font-mono text-[11px]">
                    <td className="p-2 font-sans font-bold text-slate-800">
                      {item.product.name}
                      <span className="block text-[10px] text-slate-400 font-normal">الوحدة: {item.unit}</span>
                    </td>
                    <td className="p-2 text-center text-slate-700">{item.quantity}</td>
                    <td className="p-2 text-left text-slate-500">{itemTotalCost.toLocaleString(undefined, { maximumFractionDigits: 2 })}</td>
                    <td className="p-2 text-left font-bold text-slate-800">{item.total.toLocaleString(undefined, { maximumFractionDigits: 2 })}</td>
                    <td className="p-2 text-left">
                      <span className={`font-bold ${itemNetProfit >= 0 ? 'text-emerald-600' : 'text-rose-600'}`}>
                        {itemNetProfit.toLocaleString(undefined, { maximumFractionDigits: 2 })}
                      </span>
                      <span className="block text-[9px] text-slate-400">({itemMargin.toFixed(0)}%)</span>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>

        <div className="border-t pt-2 shrink-0 flex items-center justify-between">
          <span className="text-[11px] text-slate-500">
            يتم ربط هذه الأرباح بحساب العميل في تقارير كشف حساب التاجر وتحليلات الربحية
          </span>
          <button
            type="button"
            onClick={onClose}
            className="px-4 py-1.5 bg-[#0277bd] text-white rounded-md text-xs font-bold shadow-xs hover:bg-[#01579b] cursor-pointer"
          >
            إغلاق
          </button>
        </div>
      </div>
    </div>
  );
}
