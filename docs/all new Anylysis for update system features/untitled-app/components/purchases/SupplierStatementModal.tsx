'use client';

import React from 'react';
import { FileText } from 'lucide-react';
import { Supplier, Invoice } from '@/types/pos';

export interface SupplierStatementModalProps {
  supplier: Supplier | null;
  purchaseInvoices: Invoice[];
  currency: string;
  onClose: () => void;
}

export const SupplierStatementModal: React.FC<SupplierStatementModalProps> = ({
  supplier,
  purchaseInvoices,
  currency,
  onClose,
}) => {
  if (!supplier) return null;

  const invoicesForSupplier = purchaseInvoices.filter(
    (i) => i.customerOrSupplierId === supplier.id
  );

  return (
    <div className="fixed inset-0 bg-slate-950/70 backdrop-blur-xs flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-3xl max-h-[85vh] flex flex-col overflow-hidden">
        <div className="bg-white border-b border-slate-200 px-5 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
              <FileText className="w-4 h-4 text-sky-700" />
            </div>
            <div>
              <h3 className="font-bold text-sm text-slate-900">كشف حساب المورد: {supplier.name}</h3>
              <p className="text-[11px] text-slate-500">
                {supplier.company} | كود الحساب: {supplier.accountCode}
              </p>
            </div>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="text-slate-400 hover:text-slate-700 p-1.5 rounded-lg hover:bg-slate-100 font-bold cursor-pointer transition"
          >
            ✕
          </button>
        </div>

        <div className="p-4 flex-1 overflow-y-auto flex flex-col gap-3 text-xs">
          {/* Supplier Info Summary */}
          <div className="grid grid-cols-3 gap-3 bg-slate-50 p-3 rounded-lg border border-slate-200">
            <div>
              <span className="text-slate-500 block">الهاتف:</span>
              <span className="font-mono font-bold text-slate-800">{supplier.phone}</span>
            </div>
            <div>
              <span className="text-slate-500 block">الحد الائتماني:</span>
              <span className="font-mono-numbers font-bold text-slate-800">
                {supplier.creditLimit?.toLocaleString() || 'غير محدد'} {currency}
              </span>
            </div>
            <div>
              <span className="text-slate-500 block">الرصيد الدائن المستحق:</span>
              <span className="font-mono-numbers font-black text-rose-700 text-base">
                {supplier.currentBalance.toLocaleString()} {currency}
              </span>
            </div>
          </div>

          {/* Invoices belonging to this supplier */}
          <div className="border border-slate-300 rounded overflow-hidden">
            <table className="w-full text-right border-collapse datagrid-table">
              <thead className="bg-slate-100 text-slate-700">
                <tr>
                  <th className="w-24 p-2">رقم الفاتورة</th>
                  <th className="w-24 p-2">النوع</th>
                  <th className="w-24 p-2">التاريخ</th>
                  <th className="p-2">البيان والمستودع</th>
                  <th className="w-24 text-center p-2">طريقة الدفع</th>
                  <th className="w-32 text-left p-2">المبلغ ({currency})</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200">
                {invoicesForSupplier.length === 0 ? (
                  <tr>
                    <td colSpan={6} className="text-center py-6 text-slate-400">
                      لا توجد فواتير أو حركات مسجلة لهذا المورد
                    </td>
                  </tr>
                ) : (
                  invoicesForSupplier.map((inv) => {
                    const isRet = inv.type === 'PURCHASE_RETURN';
                    return (
                      <tr key={inv.id} className="hover:bg-slate-50">
                        <td className="font-mono font-bold text-sky-800 p-2">{inv.invoiceNo}</td>
                        <td className="p-2">
                          <span
                            className={`text-[10px] px-1.5 py-0.5 rounded font-bold ${
                              isRet ? 'bg-rose-100 text-rose-800' : 'bg-sky-100 text-sky-800'
                            }`}
                          >
                            {isRet ? 'مردود' : 'شراء'}
                          </span>
                        </td>
                        <td className="font-mono text-slate-600 p-2">{inv.date}</td>
                        <td className="text-slate-700 p-2">{inv.notes || inv.warehouseName}</td>
                        <td className="text-center font-semibold p-2">
                          {inv.paymentType === 'CREDIT' ? 'آجل' : 'نقدي'}
                        </td>
                        <td
                          className={`text-left font-mono-numbers font-bold p-2 ${
                            isRet ? 'text-rose-700' : 'text-slate-900'
                          }`}
                        >
                          {isRet ? '-' : ''}
                          {inv.grandTotal.toLocaleString()}
                        </td>
                      </tr>
                    );
                  })
                )}
              </tbody>
            </table>
          </div>
        </div>

        <div className="bg-slate-100 p-3 border-t border-slate-300 flex justify-end">
          <button
            type="button"
            onClick={onClose}
            className="bg-slate-800 hover:bg-slate-700 text-white font-bold px-4 py-1.5 rounded cursor-pointer text-xs"
          >
            إغلاق
          </button>
        </div>
      </div>
    </div>
  );
};
