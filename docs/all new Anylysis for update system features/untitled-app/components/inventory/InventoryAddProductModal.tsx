import React from 'react';
import { Package, Tags, Layers, Plus, Trash2, AlertTriangle, Calendar, ShieldCheck, ShieldOff, Check } from 'lucide-react';

interface InventoryAddProductModalProps {
  isOpen: boolean;
  onClose: () => void;
  editingItem: any;
  handleSaveItem: (e: React.FormEvent) => void;
  formCode: string;
  setFormCode: (v: string) => void;
  formBarcode: string;
  setFormBarcode: (v: string) => void;
  formName: string;
  setFormName: (v: string) => void;
  formCategory: string;
  setFormCategory: (v: string) => void;
  categories: string[];
  formUnits: any[];
  handleApplyMarkupToUnits: (pct: number) => void;
  handleSyncSubUnitsFromBase: () => void;
  handleAddUnitRow: () => void;
  handleUpdateUnitRow: (idx: number, field: string, value: any) => void;
  handleRemoveUnitRow: (idx: number) => void;
  formHasExpiryDate: boolean;
  setFormHasExpiryDate: (v: boolean) => void;
  setFormValuationMethod: (v: any) => void;
  formExpiryDate: string;
  setFormExpiryDate: (v: string) => void;
  formStock: number;
  setFormStock: (v: number) => void;
  formMinStock: number;
  setFormMinStock: (v: number) => void;
  formBatchNo: string;
  setFormBatchNo: (v: string) => void;
  currency: string;
}

export const InventoryAddProductModal: React.FC<InventoryAddProductModalProps> = ({
  isOpen, onClose, editingItem, handleSaveItem, formCode, setFormCode, formBarcode, setFormBarcode, formName, setFormName,
  formCategory, setFormCategory, categories, formUnits, handleApplyMarkupToUnits, handleSyncSubUnitsFromBase, handleAddUnitRow,
  handleUpdateUnitRow, handleRemoveUnitRow, formHasExpiryDate, setFormHasExpiryDate, setFormValuationMethod, formExpiryDate,
  setFormExpiryDate, formStock, setFormStock, formMinStock, setFormMinStock, formBatchNo, setFormBatchNo, currency
}) => {
  if (!isOpen) return null;
  return (
        <div className="fixed inset-0 bg-slate-950/60 backdrop-blur-xs flex items-center justify-center z-50 p-4">
          <div className="bg-white text-slate-800 rounded-xl shadow-2xl border border-slate-300 w-full max-w-3xl overflow-hidden animate-in fade-in zoom-in-95 duration-150 flex flex-col max-h-[90vh]">
            <div className="bg-white border-b border-slate-200 px-5 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
              <div className="flex items-center gap-2.5">
                <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
                  <Package className="w-4 h-4 text-sky-700" />
                </div>
                <h3 className="font-bold text-sm text-slate-900">
                  {editingItem ? `تعديل بطاقة الصنف والوحدات [${editingItem.code}]` : 'إضافة بطاقة صنف جديد مع الوحدات المتعددة'}
                </h3>
              </div>
              <button
                onClick={onClose}
                className="text-slate-400 hover:text-slate-700 p-1.5 rounded-lg hover:bg-slate-100 font-bold cursor-pointer transition"
              >
                ✕
              </button>
            </div>

            <form onSubmit={handleSaveItem} className="p-4 flex flex-col gap-3 text-xs overflow-y-auto flex-1">
              <div className="bg-slate-50 p-3.5 rounded-xl border border-slate-200 space-y-3">
                <h4 className="font-bold text-sky-800 flex items-center gap-1.5 pb-1.5 border-b border-slate-200">
                  <Tags className="w-3.5 h-3.5 text-sky-600" />
                  <span>البيانات الأساسية والتصنيف</span>
                </h4>

                <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
                  <div>
                    <label className="font-semibold text-slate-700 block mb-1">كود الصنف الداخلي:</label>
                    <input
                      type="text"
                      value={formCode}
                      onChange={(e) => setFormCode(e.target.value)}
                      className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-mono text-slate-800 focus:outline-none focus:ring-1 focus:ring-sky-500"
                      required
                    />
                  </div>

                  <div className="sm:col-span-2">
                    <label className="font-semibold text-slate-700 block mb-1">الباركود الدولي الرئيسي (الوحدة الصغرى):</label>
                    <div className="flex items-center gap-1">
                      <input
                        type="text"
                        value={formBarcode}
                        onChange={(e) => setFormBarcode(e.target.value)}
                        className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-mono text-slate-800 focus:outline-none focus:ring-1 focus:ring-sky-500"
                        required
                      />
                      <button
                        type="button"
                        onClick={() => setFormBarcode(`629${Math.floor(100000000 + Math.random() * 900000000)}`)}
                        className="bg-sky-600 hover:bg-sky-500 text-white px-3 py-1.5 rounded whitespace-nowrap font-medium transition cursor-pointer shadow-2xs"
                        title="توليد باركود تلقائي متوافق"
                      >
                        توليد
                      </button>
                    </div>
                  </div>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
                  <div className="sm:col-span-2">
                    <label className="font-semibold text-slate-700 block mb-1">اسم الصنف الكامل والمواصفات:</label>
                    <input
                      type="text"
                      placeholder="مثال: شاي كبوس فرط أصلي 250 جم..."
                      value={formName}
                      onChange={(e) => setFormName(e.target.value)}
                      className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-bold text-slate-900 focus:outline-none focus:ring-1 focus:ring-sky-500"
                      required
                    />
                  </div>

                  <div>
                    <label className="font-semibold text-slate-700 block mb-1">التصنيف الرئيسي:</label>
                    <select
                      value={formCategory}
                      onChange={(e) => setFormCategory(e.target.value)}
                      className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-medium text-slate-800 focus:outline-none focus:ring-1 focus:ring-sky-500 cursor-pointer"
                    >
                      {categories.filter((c) => c !== 'ALL').map((c) => (
                        <option key={c} value={c}>{c}</option>
                      ))}
                    </select>
                  </div>
                </div>
              </div>

              <div className="bg-slate-50 p-3.5 rounded-xl border border-slate-200 space-y-2">
                <div className="flex flex-wrap items-center justify-between gap-2 pb-1.5 border-b border-slate-200">
                  <h4 className="font-bold text-amber-800 flex items-center gap-1.5 text-xs">
                    <Layers className="w-3.5 h-3.5 text-amber-600" />
                    <span>جدول الوحدات المتعددة والأسعار وهوامش الربح</span>
                  </h4>
                  <div className="flex items-center gap-1.5 flex-wrap">
                    <span className="text-[10px] text-slate-500 font-semibold">تسعير سريع:</span>
                    {[10, 15, 20, 25].map((pct) => (
                      <button
                        key={pct}
                        type="button"
                        onClick={() => handleApplyMarkupToUnits(pct)}
                        className="bg-white hover:bg-sky-50 text-sky-700 border border-sky-200 text-[10px] px-2 py-0.5 rounded font-mono font-bold transition cursor-pointer shadow-2xs"
                        title={`احتساب سعر البيع لجميع الوحدات بهامش ربح +${pct}% فوق التكلفة`}
                      >
                        +{pct}% ربح
                      </button>
                    ))}
                    <button
                      type="button"
                      onClick={handleSyncSubUnitsFromBase}
                      className="bg-white hover:bg-amber-50 text-amber-800 border border-amber-300 text-[10px] px-2 py-0.5 rounded font-bold transition cursor-pointer shadow-2xs"
                      title="مزامنة وتوزيع أسعار الوحدات الفرعية تلقائياً بناءً على سعر الحبة ومعامل التحويل"
                    >
                      مزامنة الوحدات
                    </button>
                    <button
                      type="button"
                      onClick={handleAddUnitRow}
                      className="bg-sky-600 hover:bg-sky-500 text-white px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer shadow-2xs"
                    >
                      <Plus className="w-3 h-3" /> إضافة وحدة
                    </button>
                  </div>
                </div>

                <div className="overflow-x-auto bg-white rounded-lg border border-slate-200">
                  <table className="w-full text-right text-xs border-collapse">
                    <thead className="bg-slate-100 text-slate-700 font-semibold border-b border-slate-200">
                      <tr>
                        <th className="p-2">اسم الوحدة</th>
                        <th className="p-2 w-16 text-center">المعامل</th>
                        <th className="p-2 w-24">سعر الشراء ({currency})</th>
                        <th className="p-2 w-24">سعر البيع ({currency})</th>
                        <th className="p-2 w-24 text-center">هامش الربح (%)</th>
                        <th className="p-2 w-32">الباركود الخاص</th>
                        <th className="p-2 w-8 text-center">حذف</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-slate-100">
                      {formUnits.map((u, idx) => {
                        const costVal = u.cost || 0;
                        const priceVal = u.price || 0;
                        const marginPct = costVal > 0 ? Math.round(((priceVal - costVal) / costVal) * 100) : 0;
                        const isBelowCost = costVal > 0 && priceVal <= costVal;

                        return (
                          <tr key={idx} className="hover:bg-slate-50">
                            <td className="p-1.5">
                              <input
                                type="text"
                                value={u.name}
                                onChange={(e) => handleUpdateUnitRow(idx, 'name', e.target.value)}
                                placeholder="مثال: باكت، كرتون..."
                                className="w-full bg-white border border-slate-200 rounded px-2 py-1 text-slate-800 font-medium focus:outline-none focus:border-sky-500"
                                required
                              />
                            </td>
                            <td className="p-1.5">
                              <input
                                type="number"
                                min={1}
                                disabled={idx === 0}
                                value={u.factor}
                                onChange={(e) => handleUpdateUnitRow(idx, 'factor', e.target.value)}
                                className={`w-full text-center border rounded px-1.5 py-1 font-mono font-bold ${
                                  idx === 0 ? 'bg-slate-100 text-slate-500 border-slate-200' : 'bg-white text-slate-800 border-slate-200'
                                }`}
                              />
                            </td>
                            <td className="p-1.5">
                              <input
                                type="number"
                                min={0}
                                value={u.cost}
                                onChange={(e) => handleUpdateUnitRow(idx, 'cost', e.target.value)}
                                className="w-full bg-white border border-slate-200 rounded px-2 py-1 text-slate-700 font-mono focus:outline-none focus:border-sky-500"
                              />
                            </td>
                            <td className="p-1.5">
                              <div className="relative">
                                <input
                                  type="number"
                                  min={0}
                                  value={u.price}
                                  onChange={(e) => handleUpdateUnitRow(idx, 'price', e.target.value)}
                                  className={`w-full bg-white border rounded px-2 py-1 font-mono font-bold focus:outline-none ${
                                    isBelowCost
                                      ? 'border-rose-400 text-rose-600 ring-1 ring-rose-400'
                                      : 'border-emerald-300 text-emerald-700'
                                  }`}
                                />
                              </div>
                            </td>
                            <td className="p-1.5 text-center">
                              {isBelowCost ? (
                                <span
                                  className="text-[10px] bg-rose-50 text-rose-700 border border-rose-300 px-1.5 py-0.5 rounded font-bold inline-flex items-center gap-0.5"
                                  title="سعر البيع أقل من أو مساوٍ للتكلفة!"
                                >
                                  <AlertTriangle className="w-2.5 h-2.5" />
                                  <span>خسارة</span>
                                </span>
                              ) : (
                                <span
                                  className={`font-mono font-bold text-[11px] px-1.5 py-0.5 rounded ${
                                    marginPct >= 20
                                      ? 'bg-emerald-50 text-emerald-700 border border-emerald-200'
                                      : 'bg-amber-50 text-amber-700 border border-amber-200'
                                  }`}
                                >
                                  +{marginPct}%
                                </span>
                              )}
                            </td>
                            <td className="p-1.5">
                              <div className="flex items-center gap-1">
                                <input
                                  type="text"
                                  value={u.barcode}
                                  onChange={(e) => handleUpdateUnitRow(idx, 'barcode', e.target.value)}
                                  placeholder="باركود الوحدة..."
                                  className="w-full bg-white border border-slate-200 rounded px-2 py-1 text-slate-700 font-mono text-[11px] focus:outline-none focus:border-sky-500"
                                />
                                {!u.barcode && (
                                  <button
                                    type="button"
                                    onClick={() => handleUpdateUnitRow(idx, 'barcode', `629${Math.floor(100000000 + Math.random() * 900000000)}`)}
                                    className="bg-slate-100 hover:bg-sky-50 text-[10px] text-sky-700 border border-slate-200 px-1.5 py-1 rounded whitespace-nowrap cursor-pointer"
                                    title="توليد باركود تلقائي"
                                  >
                                    توليد
                                  </button>
                                )}
                              </div>
                            </td>
                            <td className="p-1.5 text-center">
                              {idx > 0 && (
                                <button
                                  type="button"
                                  onClick={() => handleRemoveUnitRow(idx)}
                                  className="text-rose-500 hover:text-rose-700 p-1 cursor-pointer"
                                  title="حذف هذه الوحدة"
                                >
                                  <Trash2 className="w-3.5 h-3.5" />
                                </button>
                              )}
                            </td>
                          </tr>
                        );
                      })}
                    </tbody>
                  </table>
                </div>
              </div>

              <div className="bg-slate-50 p-3.5 rounded-xl border border-slate-200 space-y-3">
                <div className="flex flex-wrap items-center justify-between gap-2 pb-1.5 border-b border-slate-200">
                  <h4 className="font-bold text-emerald-800 flex items-center gap-1.5">
                    <Calendar className="w-3.5 h-3.5 text-emerald-600" />
                    <span>الرصيد الافتتاحي وتتبع الدفعات وتاريخ الانتهاء</span>
                  </h4>
                  <div className="flex items-center gap-1 bg-slate-200/70 p-0.5 rounded-lg border border-slate-300">
                    <button
                      type="button"
                      onClick={() => {
                        setFormHasExpiryDate(true);
                        setFormValuationMethod('FEFO');
                      }}
                      className={`px-2.5 py-1 rounded text-xs font-bold transition flex items-center gap-1 cursor-pointer ${
                        formHasExpiryDate
                          ? 'bg-amber-600 text-white shadow-xs'
                          : 'text-slate-600 hover:text-slate-800'
                      }`}
                    >
                      <ShieldCheck className="w-3.5 h-3.5" />
                      <span>له تاريخ انتهاء (خاضع للصلاحية)</span>
                    </button>
                    <button
                      type="button"
                      onClick={() => {
                        setFormHasExpiryDate(false);
                        setFormValuationMethod('FIFO');
                        setFormExpiryDate('');
                      }}
                      className={`px-2.5 py-1 rounded text-xs font-bold transition flex items-center gap-1 cursor-pointer ${
                        !formHasExpiryDate
                          ? 'bg-sky-600 text-white shadow-xs'
                          : 'text-slate-600 hover:text-slate-800'
                      }`}
                    >
                      <ShieldOff className="w-3.5 h-3.5" />
                      <span>بدون تاريخ انتهاء (صنف دائم)</span>
                    </button>
                  </div>
                </div>

                <div className="bg-white px-3 py-2 rounded-lg border border-slate-200 flex flex-wrap items-center justify-between gap-2 text-xs">
                  <div className="flex items-center gap-2">
                    <Layers className="w-4 h-4 text-sky-600" />
                    <span className="text-slate-700 font-semibold">طريقة التقييم وسلسلة الصرف المعتمدة:</span>
                    {formHasExpiryDate ? (
                      <span className="font-bold text-amber-800 bg-amber-50 border border-amber-200 px-2 py-0.5 rounded text-[11px] flex items-center gap-1">
                        <span>FEFO / EEFO (الأقرب انتهاءً يصرف أولاً)</span>
                      </span>
                    ) : (
                      <span className="font-bold text-sky-800 bg-sky-50 border border-sky-200 px-2 py-0.5 rounded text-[11px] flex items-center gap-1">
                        <span>FIFO (الوارد أولاً يصرف أولاً)</span>
                      </span>
                    )}
                  </div>
                  <span className="text-[11px] text-slate-500">
                    {formHasExpiryDate
                      ? 'يتم توجيه الصرف آلياً من الدفعة الأقرب لانتهاء الصلاحية لتقليل الهدر والتلف المخزني.'
                      : 'يتم توجيه الصرف آلياً من أقدم دفعة واردة للمستودع لحساب التكلفة الفعلية بدقة.'}
                  </span>
                </div>

                <div className="grid grid-cols-2 sm:grid-cols-4 gap-3">
                  <div>
                    <label className="font-semibold text-slate-700 block mb-1">الرصيد الافتتاحي بالمخزن:</label>
                    <input
                      type="number"
                      min={0}
                      value={formStock}
                      onChange={(e) => setFormStock(parseFloat(e.target.value) || 0)}
                      className="w-full bg-white border border-slate-300 rounded px-2 py-1.5 font-mono font-bold text-emerald-700 focus:outline-none focus:ring-1 focus:ring-emerald-500"
                      required
                    />
                  </div>

                  <div>
                    <label className="font-semibold text-slate-700 block mb-1">حد الطلب الأدنى للتنبيه:</label>
                    <input
                      type="number"
                      min={0}
                      value={formMinStock}
                      onChange={(e) => setFormMinStock(parseFloat(e.target.value) || 0)}
                      className="w-full bg-white border border-slate-300 rounded px-2 py-1.5 font-mono text-rose-600 focus:outline-none focus:ring-1 focus:ring-rose-500"
                    />
                  </div>

                  <div>
                    <label className="font-semibold text-slate-700 block mb-1">رقم الدفعة (Batch No):</label>
                    <input
                      type="text"
                      value={formBatchNo}
                      onChange={(e) => setFormBatchNo(e.target.value)}
                      placeholder="اختياري (مثل: B-2026)"
                      className="w-full bg-white border border-slate-300 rounded px-2 py-1.5 font-mono text-slate-800 focus:outline-none placeholder-slate-400"
                    />
                  </div>

                  <div>
                    <label className="font-semibold text-slate-700 block mb-1">تاريخ انتهاء الصلاحية:</label>
                    {formHasExpiryDate ? (
                      <input
                        type="date"
                        value={formExpiryDate}
                        onChange={(e) => setFormExpiryDate(e.target.value)}
                        className="w-full bg-white border border-amber-300 rounded px-2 py-1.5 text-slate-800 focus:outline-none focus:ring-1 focus:ring-amber-500"
                        required={formHasExpiryDate}
                      />
                    ) : (
                      <div className="w-full bg-slate-100 border border-dashed border-slate-300 rounded px-2 py-1.5 text-slate-400 text-xs flex items-center justify-center h-[34px] select-none">
                        لا ينطبق (بدون انتهاء)
                      </div>
                    )}
                  </div>
                </div>
              </div>

              <div className="flex items-center justify-end gap-2 pt-3 border-t border-slate-200">
                <button
                  type="button"
                  onClick={onClose}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-4 py-2 rounded-lg font-semibold cursor-pointer border border-slate-200"
                >
                  إلغاء
                </button>
                <button
                  type="submit"
                  className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-6 py-2 rounded-lg flex items-center gap-1.5 shadow-xs transition cursor-pointer"
                >
                  <Check className="w-4 h-4" />
                  <span>حفظ بطاقة الصنف والوحدات</span>
                </button>
              </div>
            </form>
          </div>
        </div>
  );
};
