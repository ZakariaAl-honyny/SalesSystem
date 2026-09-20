import React from 'react';
import { Layers, ShieldOff, ShieldCheck, Sparkles, XCircle, CheckCircle2 } from 'lucide-react';
import { getOrInitFifoLayers } from '@/lib/mock';

interface InventoryFifoModalProps {
  fifoModalProduct: any;
  setFifoModalProduct: (v: any) => void;
  currency: string;
}

export const InventoryFifoModal: React.FC<InventoryFifoModalProps> = ({
  fifoModalProduct, setFifoModalProduct, currency
}) => {
  if (!fifoModalProduct) return null;
  
  const isFefo = fifoModalProduct.hasExpiryDate !== false;
  const rawLayers = getOrInitFifoLayers(fifoModalProduct);
  const layers = [...rawLayers].sort((a, b) => {
    if (isFefo) {
      if (a.expiryDate && b.expiryDate) {
        const diff = new Date(a.expiryDate).getTime() - new Date(b.expiryDate).getTime();
        if (diff !== 0) return diff;
      } else if (a.expiryDate && !b.expiryDate) return -1;
      else if (!a.expiryDate && b.expiryDate) return 1;
      return new Date(a.date).getTime() - new Date(b.date).getTime();
    } else {
      return new Date(a.date).getTime() - new Date(b.date).getTime();
    }
  });
  const activeLayers = layers.filter((l) => l.remainingQuantity > 0);
  const totalValuation = activeLayers.reduce((sum, l) => sum + (l.remainingQuantity * l.unitCost), 0);
  const currentActiveLayer = activeLayers[0];

  return (
          <div className="fixed inset-0 z-50 bg-black/60 backdrop-blur-xs flex items-center justify-center p-4">
            <div className="bg-white border border-slate-200 rounded-xl shadow-2xl w-full max-w-4xl max-h-[90vh] flex flex-col overflow-hidden animate-in fade-in zoom-in-95 duration-150">
              <div className="bg-white px-5 py-3.5 border-b border-slate-200 flex items-center justify-between text-slate-800 shadow-2xs">
                <div className="flex items-center gap-3">
                  <div className={`w-9 h-9 rounded-lg border flex items-center justify-center shadow-xs ${
                    isFefo 
                      ? 'bg-amber-50 border-amber-200 text-amber-700' 
                      : 'bg-emerald-50 border-emerald-200 text-emerald-700'
                  }`}>
                    <Layers className="w-5 h-5" />
                  </div>
                  <div>
                    <div className="flex items-center gap-2 flex-wrap">
                      <h3 className="text-sm font-bold text-slate-900">
                        {isFefo 
                          ? 'سجل طبقات التكلفة والصرف بنظام (FEFO / EEFO) - الأقرب انتهاءً يصرف أولاً'
                          : 'سجل طبقات التكلفة والصرف بنظام (FIFO) - الوارد أولاً يصرف أولاً'}
                      </h3>
                      {isFefo ? (
                        <span className="bg-amber-50 text-amber-800 text-[10px] font-mono px-2 py-0.5 rounded border border-amber-300 font-bold">
                          First-Expired, First-Out (FEFO / EEFO)
                        </span>
                      ) : (
                        <span className="bg-emerald-50 text-emerald-800 text-[10px] font-mono px-2 py-0.5 rounded border border-emerald-300 font-bold">
                          First-In, First-Out (FIFO)
                        </span>
                      )}
                    </div>
                    <div className="flex items-center gap-2 flex-wrap mt-0.5">
                      <p className="text-xs text-slate-600">
                        الصنف: <strong className="text-slate-900">{fifoModalProduct.name}</strong> ({fifoModalProduct.code || fifoModalProduct.barcode})
                      </p>
                      {fifoModalProduct.hasExpiryDate === false ? (
                        <span className="inline-flex items-center gap-1 bg-slate-100 text-slate-700 text-[10px] px-2 py-0.5 rounded border border-slate-200">
                          <ShieldOff className="w-3 h-3 text-slate-500" />
                          <span>صنف دائم (بدون تاريخ انتهاء)</span>
                        </span>
                      ) : (
                        <span className="inline-flex items-center gap-1 bg-amber-50 text-amber-800 text-[10px] px-2 py-0.5 rounded border border-amber-300">
                          <ShieldCheck className="w-3 h-3 text-amber-600" />
                          <span>صنف خاضع لتاريخ الانتهاء والصلاحية</span>
                        </span>
                      )}
                    </div>
                  </div>
                </div>
                <button
                  onClick={() => setFifoModalProduct(null)}
                  className="text-slate-400 hover:text-slate-700 p-1.5 rounded-lg hover:bg-slate-100 transition cursor-pointer"
                >
                  <XCircle className="w-5 h-5" />
                </button>
              </div>

              <div className="p-5 overflow-y-auto space-y-4 text-xs bg-slate-50/50">
                <div className={`border rounded-lg p-3 flex items-start gap-2.5 ${
                  isFefo ? 'bg-amber-50 border-amber-200 text-amber-900' : 'bg-sky-50 border-sky-200 text-sky-900'
                }`}>
                  <Sparkles className={`w-4 h-4 shrink-0 mt-0.5 ${isFefo ? 'text-amber-600' : 'text-sky-600'}`} />
                  <div className="text-[11px] leading-relaxed">
                    {isFefo ? (
                      <>
                        <span className="font-bold text-amber-800">مبدأ العمل في نظام FEFO / EEFO (الأقرب انتهاءً يصرف أولاً): </span>
                        نظراً لأن هذا الصنف خاضع لتاريخ الصلاحية، يتم توجيه الصرف وتكلفة البضاعة المباعة آلياً بدءاً من <strong>الدفعة ذات تاريخ الانتهاء الأقرب</strong>، بغض النظر عما إذا كانت قد دخلت المستودع أولاً أم لا. هذا النظام يمنع تلف البضائع في المستودعات ويضمن أعلى معايير الجودة والسلامة وتقليل الهدر.
                      </>
                    ) : (
                      <>
                        <span className="font-bold text-sky-800">مبدأ العمل في نظام FIFO (الوارد أولاً يصرف أولاً): </span>
                        نظراً لأن الصنف دائم وغير خاضع لتاريخ الصلاحية، يتم تسعير تكلفة البضاعة المباعة وصرفها بدءاً من <strong>أقدم طبقة مشتريات واردة للمستودع</strong> بسعر تكلفتها التاريخي الفعلي، مع انتقال النظام آلياً إلى الطبقات الأحدث عند نفاد القديمة.
                      </>
                    )}
                  </div>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
                  <div className="bg-white p-3 rounded-xl border border-slate-200 shadow-2xs">
                    <span className="text-slate-500 text-[11px] block font-medium">إجمالي الرصيد المتوفر</span>
                    <div className="text-lg font-black text-sky-700 font-mono mt-1">
                      {fifoModalProduct.stock} {fifoModalProduct.unit}
                    </div>
                    <span className="text-[10px] text-slate-400 mt-1 block">
                      موزعة على {activeLayers.length} طبقة نشطة
                    </span>
                  </div>

                  <div className="bg-white p-3 rounded-xl border border-slate-200 shadow-2xs">
                    <span className="text-slate-500 text-[11px] block font-medium">
                      {isFefo ? 'تكلفة الصرف القادمة (الدفعة الأقرب انتهاءً)' : 'تكلفة الصرف القادمة (أقدم دفعة واردة)'}
                    </span>
                    <div className="text-lg font-black text-amber-600 font-mono mt-1">
                      {(currentActiveLayer ? currentActiveLayer.unitCost : fifoModalProduct.costPrice).toLocaleString()} {currency}
                    </div>
                    <span className="text-[10px] text-emerald-600 font-semibold mt-1 block">
                      سعر البيع الحالي: {fifoModalProduct.salePrice.toLocaleString()} {currency}
                    </span>
                  </div>

                  <div className="bg-white p-3 rounded-xl border border-slate-200 shadow-2xs">
                    <span className="text-slate-500 text-[11px] block font-medium">
                      {isFefo ? 'إجمالي القيمة الدفترية للمخزون (FEFO)' : 'إجمالي القيمة الدفترية للمخزون (FIFO)'}
                    </span>
                    <div className="text-lg font-black text-emerald-700 font-mono mt-1">
                      {totalValuation.toLocaleString()} {currency}
                    </div>
                    <span className="text-[10px] text-slate-400 mt-1 block">
                      بناءً على التكلفة الفعلية لكل طبقة واردة
                    </span>
                  </div>
                </div>

                <div className="border border-slate-200 rounded-xl overflow-hidden bg-white shadow-2xs">
                  <div className="bg-slate-100 border-b border-slate-200 px-3.5 py-2.5 text-slate-800 font-bold text-xs flex items-center justify-between">
                    <span>
                      {isFefo 
                        ? 'قائمة الدفعات مرتبة حسب أسبقية الصرف (FEFO / EEFO - الأقرب انتهاءً أولاً)' 
                        : 'قائمة طبقات التوريد مرتبة زمنياً (FIFO - الوارد أولاً يصرف أولاً)'}
                    </span>
                    <span className="text-[11px] text-slate-500 font-normal">
                      {isFefo ? 'مرتبة من تاريخ الانتهاء الأقرب إلى الأبعد' : 'مرتبة زمنياً من أقدم توريد إلى الأحدث'}
                    </span>
                  </div>
                  <table className="w-full text-right text-xs">
                    <thead className="bg-slate-50 text-slate-700 border-b border-slate-200 font-semibold">
                      <tr>
                        <th className="p-2.5">رقم الطبقة / الدفعة</th>
                        <th className="p-2.5">تاريخ التوريد</th>
                        <th className="p-2.5 text-center">تاريخ الصلاحية</th>
                        <th className="p-2.5">فاتورة الشراء</th>
                        <th className="p-2.5 text-center">الكمية الواردة</th>
                        <th className="p-2.5 text-center">الكمية المتبقية</th>
                        <th className="p-2.5 text-center">تكلفة الوحدة</th>
                        <th className="p-2.5 text-center">القيمة المتبقية</th>
                        <th className="p-2.5 text-center">أولوية الصرف</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-slate-100">
                      {layers.length === 0 ? (
                        <tr>
                          <td colSpan={9} className="p-4 text-center text-slate-400">
                            لا توجد طبقات مسجلة لهذا الصنف بعد
                          </td>
                        </tr>
                      ) : (
                        layers.map((layer, index) => {
                          const isActive = layer.remainingQuantity > 0 && index === 0;
                          const isQueued = layer.remainingQuantity > 0 && index > 0;
                          const isDepleted = layer.remainingQuantity <= 0;

                          return (
                            <tr
                              key={layer.id}
                              className={`hover:bg-slate-50 transition ${
                                isActive ? (isFefo ? 'bg-amber-50/60' : 'bg-emerald-50/60') : ''
                              }`}
                            >
                              <td className="p-2.5 font-mono text-slate-900 font-semibold">
                                {layer.batchNo || layer.id}
                              </td>
                              <td className="p-2.5 text-slate-600 font-mono">
                                {layer.date}
                              </td>
                              <td className="p-2.5 text-center font-mono text-[11px]">
                                {fifoModalProduct.hasExpiryDate === false ? (
                                  <span className="text-slate-500 bg-slate-100 px-1.5 py-0.5 rounded text-[10px]">
                                    غير خاضع
                                  </span>
                                ) : (
                                  <span className={layer.expiryDate ? 'text-amber-700 font-bold' : 'text-slate-400'}>
                                    {layer.expiryDate || '-'}
                                  </span>
                                )}
                              </td>
                              <td className="p-2.5 text-slate-600 font-mono">
                                {layer.purchaseInvoiceNo || 'رصيد افتتاحي'}
                              </td>
                              <td className="p-2.5 text-center font-mono text-slate-600">
                                {layer.initialQuantity}
                              </td>
                              <td className="p-2.5 text-center font-mono font-bold text-slate-900">
                                {layer.remainingQuantity}
                              </td>
                              <td className="p-2.5 text-center font-mono text-amber-700 font-semibold">
                                {layer.unitCost.toLocaleString()} {currency}
                              </td>
                              <td className="p-2.5 text-center font-mono text-emerald-700 font-semibold">
                                {(layer.remainingQuantity * layer.unitCost).toLocaleString()} {currency}
                              </td>
                              <td className="p-2.5 text-center">
                                {isActive && (
                                  <span className={`inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[10px] font-bold border ${
                                    isFefo
                                      ? 'bg-amber-100 text-amber-800 border-amber-300'
                                      : 'bg-emerald-100 text-emerald-800 border-emerald-300'
                                  }`}>
                                    <CheckCircle2 className="w-3 h-3" />
                                    <span>{isFefo ? 'تصرف أولاً (الأقرب انتهاءً)' : 'تصرف أولاً (الأقدم وروداً)'}</span>
                                  </span>
                                )}
                                {isQueued && (
                                  <span className="inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[10px] font-semibold bg-sky-50 text-sky-700 border border-sky-200">
                                    <span>في قائمة الانتظار #{index + 1}</span>
                                  </span>
                                )}
                                {isDepleted && (
                                  <span className="inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[10px] font-semibold bg-slate-100 text-slate-500 border border-slate-200">
                                    <span>استنفدت بالكامل</span>
                                  </span>
                                )}
                              </td>
                            </tr>
                          );
                        })
                      )}
                    </tbody>
                  </table>
                </div>
              </div>

              <div className="bg-slate-50 px-5 py-3 border-t border-slate-200 flex items-center justify-between">
                <span className="text-[11px] text-slate-500">
                  {isFefo 
                    ? 'يتم اختيار الدفعة المنصرفة في المبيعات آلياً وفق الأقرب انتهاءً (FEFO / EEFO) لحماية المخزون من انتهاء الصلاحية'
                    : 'يتم استنزاف الطبقات زمنياً وفق أسبقية التوريد (FIFO) لحساب التكلفة بدقة'}
                </span>
                <button
                  onClick={() => setFifoModalProduct(null)}
                  className="bg-white hover:bg-slate-100 text-slate-700 border border-slate-300 px-4 py-1.5 rounded-lg text-xs font-bold transition cursor-pointer shadow-2xs"
                >
                  إغلاق
                </button>
              </div>
            </div>
          </div>
  );
};
