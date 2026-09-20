import React from 'react';
import { Barcode, Printer } from 'lucide-react';

interface InventoryBarcodeModalProps {
  barcodePreviewItem: any;
  setBarcodePreviewItem: (v: any) => void;
  selectedUnitForBarcode: any;
  setSelectedUnitForBarcode: (v: any) => void;
  currency: string;
}

export const InventoryBarcodeModal: React.FC<InventoryBarcodeModalProps> = ({
  barcodePreviewItem, setBarcodePreviewItem, selectedUnitForBarcode, setSelectedUnitForBarcode, currency
}) => {
  if (!barcodePreviewItem) return null;
  return (
        <div className="fixed inset-0 bg-slate-950/60 backdrop-blur-xs flex items-center justify-center z-50 p-4">
          <div className="bg-white text-slate-800 rounded-xl shadow-2xl border border-slate-300 w-full max-w-sm overflow-hidden animate-in fade-in zoom-in-95">
            <div className="bg-white border-b border-slate-200 px-4 py-3 flex items-center justify-between text-slate-800 shadow-2xs">
              <div className="flex items-center gap-2">
                <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
                  <Barcode className="w-4 h-4 text-sky-700" />
                </div>
                <h3 className="font-bold text-sm text-slate-900">طباعة ملصق الباركود الحراري</h3>
              </div>
              <button
                onClick={() => setBarcodePreviewItem(null)}
                className="text-slate-400 hover:text-slate-700 p-1 rounded-lg hover:bg-slate-100 font-bold cursor-pointer transition"
              >
                ✕
              </button>
            </div>

            <div className="p-4 flex flex-col items-center justify-center gap-3 bg-slate-50/50">
              {barcodePreviewItem.units && barcodePreviewItem.units.length > 1 && (
                <div className="w-full">
                  <label className="block text-xs font-semibold text-slate-700 mb-1">اختر الوحدة لطباعة الباركود والسعر الخاص بها:</label>
                  <select
                    value={selectedUnitForBarcode?.name || barcodePreviewItem.unit}
                    onChange={(e) => {
                      const u = barcodePreviewItem.units?.find((unit: any) => unit.name === e.target.value);
                      if (u) setSelectedUnitForBarcode(u);
                    }}
                    className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 text-xs text-slate-800 focus:outline-none focus:ring-1 focus:ring-sky-500"
                  >
                    {barcodePreviewItem.units.map((u: any, i: number) => (
                      <option key={i} value={u.name}>
                        {u.name} (السعر: {u.price.toLocaleString()} {currency})
                      </option>
                    ))}
                  </select>
                </div>
              )}

              <div className="w-64 p-3 border-2 border-dashed border-slate-300 rounded-lg bg-white text-slate-900 text-center flex flex-col items-center shadow-md">
                <span className="font-bold text-xs">{barcodePreviewItem.name}</span>
                <span className="text-[10px] text-slate-600 font-semibold">
                  الوحدة: {selectedUnitForBarcode ? selectedUnitForBarcode.name : barcodePreviewItem.unit}
                </span>
                <span className="text-lg font-black font-mono text-sky-900 mt-1">
                  {(selectedUnitForBarcode ? selectedUnitForBarcode.price : barcodePreviewItem.salePrice).toLocaleString()} {currency}
                </span>
                <div className="my-2 bg-slate-900 h-10 w-48 flex items-center justify-center text-white text-[10px] tracking-widest font-mono rounded">
                  ||| | |||| || ||| | |||
                </div>
                <span className="font-mono text-xs text-slate-800 font-bold tracking-wider">
                  {selectedUnitForBarcode ? selectedUnitForBarcode.barcode : barcodePreviewItem.barcode}
                </span>
                <span className="text-[10px] text-slate-500 mt-0.5">
                  دفعة: {barcodePreviewItem.batchNo || 'عام'} {barcodePreviewItem.hasExpiryDate !== false && barcodePreviewItem.expiryDate ? `| انتهاء: ${barcodePreviewItem.expiryDate}` : '| غير خاضع للصلاحية'}
                </span>
              </div>
            </div>

            <div className="bg-slate-50 px-4 py-3 border-t border-slate-200 flex items-center justify-between">
              <button
                onClick={() => setBarcodePreviewItem(null)}
                className="bg-white border border-slate-300 text-slate-700 px-3 py-1.5 rounded-lg text-xs cursor-pointer hover:bg-slate-100 font-medium transition"
              >
                إغلاق
              </button>
              <button
                onClick={() => {
                  window.print();
                  setBarcodePreviewItem(null);
                }}
                className="bg-emerald-600 hover:bg-emerald-500 text-white px-4 py-1.5 rounded-lg text-xs font-bold flex items-center gap-1.5 cursor-pointer shadow-xs transition"
              >
                <Printer className="w-3.5 h-3.5" />
                <span>طباعة الملصق الآن</span>
              </button>
            </div>
          </div>
        </div>
  );
};
