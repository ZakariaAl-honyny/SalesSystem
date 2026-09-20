import React from 'react';
import { Printer, Calendar, FileText, Database, Package } from 'lucide-react';
import { ProductItem } from '@/types/pos';

interface StocktakingPrintCardModalProps {
  printStockCard: boolean;
  setPrintStockCard: (v: boolean) => void;
  selectedCardProduct: ProductItem | null;
  ledgerEntries: any[];
  warehouseName: string;
  currency: string;
}

export const StocktakingPrintCardModal: React.FC<StocktakingPrintCardModalProps> = ({
  printStockCard, setPrintStockCard, selectedCardProduct, ledgerEntries, warehouseName, currency
}) => {
  if (!printStockCard || !selectedCardProduct) return null;
  return (
        <div className="fixed inset-0 z-50 bg-black/80 flex items-center justify-center p-4 overflow-y-auto">
          <div className="bg-white text-slate-900 rounded-lg shadow-2xl max-w-4xl w-full p-6 my-8 print:m-0 print:p-0">
            <div className="border-b-2 border-slate-900 pb-3 mb-4 flex justify-between items-start text-right">
              <div>
                <h1 className="text-lg font-black">كرت حركة الصنف (Stock Movement Ledger)</h1>
                <h2 className="text-sm font-bold text-slate-800">
                  الصنف: {selectedCardProduct.name} [كود: {selectedCardProduct.code}]
                </h2>
                <div className="text-xs text-slate-600 mt-1">
                  الباركود: {selectedCardProduct.barcode} | الوحدة: {selectedCardProduct.unit} | التكلفة: {selectedCardProduct.costPrice.toLocaleString()} {currency}
                </div>
              </div>
              <div className="text-left text-xs font-mono">
                <div>المستودع: {warehouseName}</div>
                <div>تاريخ التقرير: {new Date().toLocaleDateString('ar-YE')}</div>
              </div>
            </div>

            <table className="w-full text-right text-xs border-collapse border border-slate-400 mb-4">
              <thead className="bg-slate-200">
                <tr>
                  <th className="border border-slate-400 p-1.5 text-center">#</th>
                  <th className="border border-slate-400 p-1.5">التاريخ</th>
                  <th className="border border-slate-400 p-1.5">نوع الحركة</th>
                  <th className="border border-slate-400 p-1.5">رقم السند</th>
                  <th className="border border-slate-400 p-1.5 text-center">وارد (+)</th>
                  <th className="border border-slate-400 p-1.5 text-center">منصرف (-)</th>
                  <th className="border border-slate-400 p-1.5 text-center">الرصيد</th>
                  <th className="border border-slate-400 p-1.5 text-center">سعر الوحدة</th>
                  <th className="border border-slate-400 p-1.5">البيان</th>
                </tr>
              </thead>
              <tbody>
                {ledgerEntries.map((m, idx) => (
                  <tr key={m.id}>
                    <td className="border border-slate-400 p-1.5 text-center font-mono">{idx + 1}</td>
                    <td className="border border-slate-400 p-1.5 font-mono">{m.date}</td>
                    <td className="border border-slate-400 p-1.5 font-bold">{m.movementLabel}</td>
                    <td className="border border-slate-400 p-1.5 font-mono">{m.documentNo}</td>
                    <td className="border border-slate-400 p-1.5 text-center font-mono text-emerald-700 font-bold">
                      {m.inQty > 0 ? `+${m.inQty}` : '-'}
                    </td>
                    <td className="border border-slate-400 p-1.5 text-center font-mono text-rose-700 font-bold">
                      {m.outQty > 0 ? `-${m.outQty}` : '-'}
                    </td>
                    <td className="border border-slate-400 p-1.5 text-center font-mono font-black">
                      {m.balanceAfter}
                    </td>
                    <td className="border border-slate-400 p-1.5 text-center font-mono">
                      {m.unitPriceOrCost.toLocaleString()}
                    </td>
                    <td className="border border-slate-400 p-1.5 text-[11px]">
                      {m.partyName ? `${m.partyName} - ` : ''}{m.notes || ''}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>

            <div className="bg-slate-100 p-2.5 rounded border border-slate-400 text-xs flex justify-between font-bold mb-6">
              <span>إجمالي الوارد: {ledgerEntries.reduce((acc, m) => acc + m.inQty, 0)} {selectedCardProduct.unit}</span>
              <span>إجمالي المنصرف: {ledgerEntries.reduce((acc, m) => acc + m.outQty, 0)} {selectedCardProduct.unit}</span>
              <span>الرصيد الختامي الحالي: {ledgerEntries[ledgerEntries.length - 1]?.balanceAfter ?? 0} {selectedCardProduct.unit}</span>
            </div>

            <div className="mt-6 flex justify-end gap-2 print:hidden">
              <button
                onClick={() => setPrintStockCard(false)}
                className="bg-slate-200 text-slate-800 px-4 py-1.5 rounded text-xs font-bold cursor-pointer"
              >
                إغلاق
              </button>
              <button
                onClick={() => window.print()}
                className="bg-sky-700 text-white px-5 py-1.5 rounded text-xs font-bold flex items-center gap-1.5 cursor-pointer"
              >
                <Printer className="w-4 h-4" />
                <span>طباعة الكرت الآن</span>
              </button>
            </div>
          </div>
        </div>
      );
};
