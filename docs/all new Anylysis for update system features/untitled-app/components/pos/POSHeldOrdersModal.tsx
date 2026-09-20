import React from 'react';
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
                          {h.items.map((it) => `${it.item.name} (${it.quantity})`).join(' ، ')}
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
