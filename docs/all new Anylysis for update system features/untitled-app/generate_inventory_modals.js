const fs = require('fs');

const createModalFile = (name, content) => {
  fs.writeFileSync(`components/inventory/${name}.tsx`, content);
};

createModalFile('InventoryAddProductModal', `import React from 'react';
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
  setFormValuationMethod: (v: string) => void;
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
        <div className="fixed inset-0 bg-black/75 backdrop-blur-xs flex items-center justify-center z-50 p-4">
          <div className="bg-[#102741] text-slate-100 rounded-xl shadow-2xl border border-[#275382] w-full max-w-3xl overflow-hidden animate-in fade-in zoom-in-95 duration-150 flex flex-col max-h-[90vh]">
            <div className="bg-[#143254] border-b border-[#1d4672] px-4 py-3 flex items-center justify-between text-white">
              <div className="flex items-center gap-2">
                <Package className="w-5 h-5 text-sky-400" />
                <h3 className="font-bold text-sm">
                  {editingItem ? \`تعديل بطاقة الصنف والوحدات [\${editingItem.code}]\` : 'إضافة بطاقة صنف جديد مع الوحدات المتعددة'}
                </h3>
              </div>
              <button
                onClick={onClose}
                className="text-slate-400 hover:text-white text-lg font-bold cursor-pointer"
              >
                ✕
              </button>
            </div>

            <form onSubmit={handleSaveItem} className="p-4 flex flex-col gap-3 text-xs overflow-y-auto flex-1">
              <div className="bg-[#0b1b2d] p-3 rounded-lg border border-[#1b3f66] space-y-3">
                <h4 className="font-bold text-sky-300 flex items-center gap-1.5 pb-1 border-b border-[#1d4672]">
                  <Tags className="w-3.5 h-3.5" />
                  <span>البيانات الأساسية والتصنيف</span>
                </h4>

                <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
                  <div>
                    <label className="font-semibold text-slate-300 block mb-1">كود الصنف الداخلي:</label>
                    <input
                      type="text"
                      value={formCode}
                      onChange={(e) => setFormCode(e.target.value)}
                      className="w-full bg-[#102741] border border-[#275382] rounded px-2.5 py-1.5 font-mono text-white focus:outline-none focus:ring-1 focus:ring-sky-400"
                      required
                    />
                  </div>

                  <div className="sm:col-span-2">
                    <label className="font-semibold text-slate-300 block mb-1">الباركود الدولي الرئيسي (الوحدة الصغرى):</label>
                    <div className="flex items-center gap-1">
                      <input
                        type="text"
                        value={formBarcode}
                        onChange={(e) => setFormBarcode(e.target.value)}
                        className="w-full bg-[#102741] border border-[#275382] rounded px-2.5 py-1.5 font-mono text-white focus:outline-none focus:ring-1 focus:ring-sky-400"
                        required
                      />
                      <button
                        type="button"
                        onClick={() => setFormBarcode(\`629\${Math.floor(100000000 + Math.random() * 900000000)}\`)}
                        className="bg-[#1c436d] hover:bg-sky-600 text-white px-3 py-1.5 rounded whitespace-nowrap font-medium transition cursor-pointer"
                        title="توليد باركود تلقائي متوافق"
                      >
                        توليد
                      </button>
                    </div>
                  </div>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
                  <div className="sm:col-span-2">
                    <label className="font-semibold text-slate-300 block mb-1">اسم الصنف الكامل والمواصفات:</label>
                    <input
                      type="text"
                      placeholder="مثال: شاي كبوس فرط أصلي 250 جم..."
                      value={formName}
                      onChange={(e) => setFormName(e.target.value)}
                      className="w-full bg-[#102741] border border-[#275382] rounded px-2.5 py-1.5 font-bold text-white focus:outline-none focus:ring-1 focus:ring-sky-400"
                      required
                    />
                  </div>

                  <div>
                    <label className="font-semibold text-slate-300 block mb-1">التصنيف الرئيسي:</label>
                    <select
                      value={formCategory}
                      onChange={(e) => setFormCategory(e.target.value)}
                      className="w-full bg-[#102741] border border-[#275382] rounded px-2.5 py-1.5 font-medium text-white focus:outline-none focus:ring-1 focus:ring-sky-400 cursor-pointer"
                    >
                      {categories.filter((c) => c !== 'ALL').map((c) => (
                        <option key={c} value={c}>{c}</option>
                      ))}
                    </select>
                  </div>
                </div>
              </div>

              <div className="bg-[#0b1b2d] p-3 rounded-lg border border-[#1b3f66] space-y-2">
                <div className="flex flex-wrap items-center justify-between gap-2 pb-1 border-b border-[#1d4672]">
                  <h4 className="font-bold text-amber-300 flex items-center gap-1.5 text-xs">
                    <Layers className="w-3.5 h-3.5" />
                    <span>جدول الوحدات المتعددة والأسعار وهوامش الربح</span>
                  </h4>
                  <div className="flex items-center gap-1.5 flex-wrap">
                    <span className="text-[10px] text-slate-400 font-semibold">تسعير سريع:</span>
                    {[10, 15, 20, 25].map((pct) => (
                      <button
                        key={pct}
                        type="button"
                        onClick={() => handleApplyMarkupToUnits(pct)}
                        className="bg-[#153456] hover:bg-sky-600 text-sky-200 text-[10px] px-2 py-0.5 rounded font-mono font-bold transition cursor-pointer"
                        title={\`احتساب سعر البيع لجميع الوحدات بهامش ربح +\${pct}% فوق التكلفة\`}
                      >
                        +\{pct}% ربح
                      </button>
                    ))}
                    <button
                      type="button"
                      onClick={handleSyncSubUnitsFromBase}
                      className="bg-[#1a416a] hover:bg-amber-600 text-amber-200 text-[10px] px-2 py-0.5 rounded font-bold transition cursor-pointer"
                      title="مزامنة وتوزيع أسعار الوحدات الفرعية تلقائياً بناءً على سعر الحبة ومعامل التحويل"
                    >
                      مزامنة الوحدات
                    </button>
                    <button
                      type="button"
                      onClick={handleAddUnitRow}
                      className="bg-sky-600 hover:bg-sky-500 text-white px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer"
                    >
                      <Plus className="w-3 h-3" /> إضافة وحدة
                    </button>
                  </div>
                </div>

                <div className="overflow-x-auto">
                  <table className="w-full text-right text-xs border-collapse">
                    <thead className="bg-[#143254] text-slate-300">
                      <tr>
                        <th className="p-1.5">اسم الوحدة</th>
                        <th className="p-1.5 w-16 text-center">المعامل</th>
                        <th className="p-1.5 w-24">سعر الشراء ({currency})</th>
                        <th className="p-1.5 w-24">سعر البيع ({currency})</th>
                        <th className="p-1.5 w-24 text-center">هامش الربح (%)</th>
                        <th className="p-1.5 w-32">الباركود الخاص</th>
                        <th className="p-1.5 w-8 text-center">حذف</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-[#183a60]">
                      {formUnits.map((u, idx) => {
                        const costVal = u.cost || 0;
                        const priceVal = u.price || 0;
                        const marginPct = costVal > 0 ? Math.round(((priceVal - costVal) / costVal) * 100) : 0;
                        const isBelowCost = costVal > 0 && priceVal <= costVal;

                        return (
                          <tr key={idx} className="hover:bg-[#102741]">
                            <td className="p-1.5">
                              <input
                                type="text"
                                value={u.name}
                                onChange={(e) => handleUpdateUnitRow(idx, 'name', e.target.value)}
                                placeholder="مثال: باكت، كرتون..."
                                className="w-full bg-[#102741] border border-[#275382] rounded px-2 py-1 text-white font-medium focus:outline-none"
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
                                className={\`w-full text-center border rounded px-1.5 py-1 font-mono font-bold \${
                                  idx === 0 ? 'bg-slate-800 text-slate-400 border-slate-700' : 'bg-[#102741] text-amber-300 border-[#275382]'
                                }\`}
                              />
                            </td>
                            <td className="p-1.5">
                              <input
                                type="number"
                                min={0}
                                value={u.cost}
                                onChange={(e) => handleUpdateUnitRow(idx, 'cost', e.target.value)}
                                className="w-full bg-[#102741] border border-[#275382] rounded px-2 py-1 text-slate-200 font-mono focus:outline-none"
                              />
                            </td>
                            <td className="p-1.5">
                              <div className="relative">
                                <input
                                  type="number"
                                  min={0}
                                  value={u.price}
                                  onChange={(e) => handleUpdateUnitRow(idx, 'price', e.target.value)}
                                  className={\`w-full bg-[#102741] border rounded px-2 py-1 font-mono font-bold focus:outline-none \${
                                    isBelowCost
                                      ? 'border-rose-500 text-rose-300 ring-1 ring-rose-500'
                                      : 'border-emerald-600 text-emerald-300'
                                  }\`}
                                />
                              </div>
                            </td>
                            <td className="p-1.5 text-center">
                              {isBelowCost ? (
                                <span
                                  className="text-[10px] bg-rose-950 text-rose-300 border border-rose-600 px-1.5 py-0.5 rounded font-bold inline-flex items-center gap-0.5"
                                  title="سعر البيع أقل من أو مساوٍ للتكلفة!"
                                >
                                  <AlertTriangle className="w-2.5 h-2.5" />
                                  <span>خسارة</span>
                                </span>
                              ) : (
                                <span
                                  className={\`font-mono font-bold text-[11px] px-1.5 py-0.5 rounded \${
                                    marginPct >= 20
                                      ? 'bg-emerald-950 text-emerald-300 border border-emerald-600/50'
                                      : 'bg-amber-950 text-amber-300 border border-amber-600/50'
                                  }\`}
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
                                  className="w-full bg-[#102741] border border-[#275382] rounded px-2 py-1 text-slate-300 font-mono text-[11px] focus:outline-none"
                                />
                                {!u.barcode && (
                                  <button
                                    type="button"
                                    onClick={() => handleUpdateUnitRow(idx, 'barcode', \`629\${Math.floor(100000000 + Math.random() * 900000000)}\`)}
                                    className="bg-slate-700 hover:bg-sky-600 text-[10px] text-white px-1.5 py-1 rounded whitespace-nowrap cursor-pointer"
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
                                  className="text-rose-400 hover:text-rose-300 p-1 cursor-pointer"
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

              <div className="bg-[#0b1b2d] p-3 rounded-lg border border-[#1b3f66] space-y-3">
                <div className="flex flex-wrap items-center justify-between gap-2 pb-1 border-b border-[#1d4672]">
                  <h4 className="font-bold text-emerald-300 flex items-center gap-1.5">
                    <Calendar className="w-3.5 h-3.5" />
                    <span>الرصيد الافتتاحي وتتبع الدفعات وتاريخ الانتهاء</span>
                  </h4>
                  <div className="flex items-center gap-1 bg-[#102741] p-0.5 rounded-lg border border-[#275382]">
                    <button
                      type="button"
                      onClick={() => {
                        setFormHasExpiryDate(true);
                        setFormValuationMethod('FEFO');
                      }}
                      className={\`px-2.5 py-1 rounded text-xs font-bold transition flex items-center gap-1 cursor-pointer \${
                        formHasExpiryDate
                          ? 'bg-amber-600 text-white shadow-xs'
                          : 'text-slate-400 hover:text-slate-200'
                      }\`}
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
                      className={\`px-2.5 py-1 rounded text-xs font-bold transition flex items-center gap-1 cursor-pointer \${
                        !formHasExpiryDate
                          ? 'bg-sky-600 text-white shadow-xs'
                          : 'text-slate-400 hover:text-slate-200'
                      }\`}
                    >
                      <ShieldOff className="w-3.5 h-3.5" />
                      <span>بدون تاريخ انتهاء (صنف دائم)</span>
                    </button>
                  </div>
                </div>

                <div className="bg-[#102741] px-3 py-2 rounded-lg border border-[#275382] flex flex-wrap items-center justify-between gap-2 text-xs">
                  <div className="flex items-center gap-2">
                    <Layers className="w-4 h-4 text-sky-400" />
                    <span className="text-slate-300 font-semibold">طريقة التقييم وسلسلة الصرف المعتمدة:</span>
                    {formHasExpiryDate ? (
                      <span className="font-bold text-amber-300 bg-amber-950/80 border border-amber-600/50 px-2 py-0.5 rounded text-[11px] flex items-center gap-1">
                        <span>FEFO / EEFO (الأقرب انتهاءً يصرف أولاً)</span>
                      </span>
                    ) : (
                      <span className="font-bold text-sky-300 bg-sky-950/80 border border-sky-600/50 px-2 py-0.5 rounded text-[11px] flex items-center gap-1">
                        <span>FIFO (الوارد أولاً يصرف أولاً)</span>
                      </span>
                    )}
                  </div>
                  <span className="text-[11px] text-slate-400">
                    {formHasExpiryDate
                      ? 'يتم توجيه الصرف آلياً من الدفعة الأقرب لانتهاء الصلاحية لتقليل الهدر والتلف المخزني.'
                      : 'يتم توجيه الصرف آلياً من أقدم دفعة واردة للمستودع لحساب التكلفة الفعلية بدقة.'}
                  </span>
                </div>

                <div className="grid grid-cols-2 sm:grid-cols-4 gap-3">
                  <div>
                    <label className="font-semibold text-slate-300 block mb-1">الرصيد الافتتاحي بالمخزن:</label>
                    <input
                      type="number"
                      min={0}
                      value={formStock}
                      onChange={(e) => setFormStock(parseFloat(e.target.value) || 0)}
                      className="w-full bg-[#102741] border border-[#275382] rounded px-2 py-1.5 font-mono font-bold text-emerald-400 focus:outline-none"
                      required
                    />
                  </div>

                  <div>
                    <label className="font-semibold text-slate-300 block mb-1">حد الطلب الأدنى للتنبيه:</label>
                    <input
                      type="number"
                      min={0}
                      value={formMinStock}
                      onChange={(e) => setFormMinStock(parseFloat(e.target.value) || 0)}
                      className="w-full bg-[#102741] border border-[#275382] rounded px-2 py-1.5 font-mono text-rose-400 focus:outline-none"
                    />
                  </div>

                  <div>
                    <label className="font-semibold text-slate-300 block mb-1">رقم الدفعة (Batch No):</label>
                    <input
                      type="text"
                      value={formBatchNo}
                      onChange={(e) => setFormBatchNo(e.target.value)}
                      placeholder="اختياري (مثل: B-2026)"
                      className="w-full bg-[#102741] border border-[#275382] rounded px-2 py-1.5 font-mono text-white focus:outline-none placeholder-slate-500"
                    />
                  </div>

                  <div>
                    <label className="font-semibold text-slate-300 block mb-1">تاريخ انتهاء الصلاحية:</label>
                    {formHasExpiryDate ? (
                      <input
                        type="date"
                        value={formExpiryDate}
                        onChange={(e) => setFormExpiryDate(e.target.value)}
                        className="w-full bg-[#102741] border border-amber-500/60 rounded px-2 py-1.5 text-white focus:outline-none"
                        required={formHasExpiryDate}
                      />
                    ) : (
                      <div className="w-full bg-[#102741]/50 border border-dashed border-[#275382] rounded px-2 py-1.5 text-slate-400 text-xs flex items-center justify-center h-[34px] select-none">
                        لا ينطبق (بدون انتهاء)
                      </div>
                    )}
                  </div>
                </div>
              </div>

              <div className="flex items-center justify-end gap-2 pt-3 border-t border-[#1d4672]">
                <button
                  type="button"
                  onClick={onClose}
                  className="bg-[#143254] hover:bg-slate-700 text-slate-200 px-4 py-2 rounded font-semibold cursor-pointer"
                >
                  إلغاء
                </button>
                <button
                  type="submit"
                  className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-6 py-2 rounded flex items-center gap-1.5 shadow transition cursor-pointer"
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
`);

createModalFile('InventoryBarcodeModal', `import React from 'react';
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
        <div className="fixed inset-0 bg-black/75 backdrop-blur-xs flex items-center justify-center z-50 p-4">
          <div className="bg-[#102741] text-slate-100 rounded-xl shadow-2xl border border-[#275382] w-full max-w-sm overflow-hidden animate-in fade-in zoom-in-95">
            <div className="bg-[#143254] border-b border-[#1d4672] px-4 py-3 flex items-center justify-between text-white">
              <div className="flex items-center gap-2">
                <Barcode className="w-4 h-4 text-sky-400" />
                <h3 className="font-bold text-sm">طباعة ملصق الباركود الحراري</h3>
              </div>
              <button
                onClick={() => setBarcodePreviewItem(null)}
                className="text-slate-400 hover:text-white font-bold cursor-pointer"
              >
                ✕
              </button>
            </div>

            <div className="p-4 flex flex-col items-center justify-center gap-3">
              {barcodePreviewItem.units && barcodePreviewItem.units.length > 1 && (
                <div className="w-full">
                  <label className="block text-xs font-semibold text-slate-300 mb-1">اختر الوحدة لطباعة الباركود والسعر الخاص بها:</label>
                  <select
                    value={selectedUnitForBarcode?.name || barcodePreviewItem.unit}
                    onChange={(e) => {
                      const u = barcodePreviewItem.units?.find((unit: any) => unit.name === e.target.value);
                      if (u) setSelectedUnitForBarcode(u);
                    }}
                    className="w-full bg-[#0b1b2d] border border-[#275382] rounded px-2.5 py-1.5 text-xs text-white"
                  >
                    {barcodePreviewItem.units.map((u: any, i: number) => (
                      <option key={i} value={u.name}>
                        {u.name} (السعر: {u.price.toLocaleString()} {currency})
                      </option>
                    ))}
                  </select>
                </div>
              )}

              <div className="w-64 p-3 border-2 border-dashed border-slate-400 rounded bg-white text-slate-900 text-center flex flex-col items-center shadow-lg">
                <span className="font-bold text-xs">{barcodePreviewItem.name}</span>
                <span className="text-[10px] text-slate-600 font-semibold">
                  الوحدة: {selectedUnitForBarcode ? selectedUnitForBarcode.name : barcodePreviewItem.unit}
                </span>
                <span className="text-lg font-black font-mono text-sky-900 mt-1">
                  {(selectedUnitForBarcode ? selectedUnitForBarcode.price : barcodePreviewItem.salePrice).toLocaleString()} {currency}
                </span>
                <div className="my-2 bg-slate-900 h-10 w-48 flex items-center justify-center text-white text-[10px] tracking-widest font-mono">
                  ||| | |||| || ||| | |||
                </div>
                <span className="font-mono text-xs text-slate-800 font-bold tracking-wider">
                  {selectedUnitForBarcode ? selectedUnitForBarcode.barcode : barcodePreviewItem.barcode}
                </span>
                <span className="text-[10px] text-slate-500 mt-0.5">
                  دفعة: {barcodePreviewItem.batchNo || 'عام'} {barcodePreviewItem.hasExpiryDate !== false && barcodePreviewItem.expiryDate ? \`| انتهاء: \${barcodePreviewItem.expiryDate}\` : '| غير خاضع للصلاحية'}
                </span>
              </div>
            </div>

            <div className="bg-[#143254] px-4 py-3 border-t border-[#1d4672] flex items-center justify-between">
              <button
                onClick={() => setBarcodePreviewItem(null)}
                className="bg-[#1c436d] text-slate-200 px-3 py-1.5 rounded text-xs cursor-pointer hover:bg-slate-700"
              >
                إغلاق
              </button>
              <button
                onClick={() => {
                  window.print();
                  setBarcodePreviewItem(null);
                }}
                className="bg-emerald-600 hover:bg-emerald-500 text-white px-4 py-1.5 rounded text-xs font-bold flex items-center gap-1 cursor-pointer"
              >
                <Printer className="w-3.5 h-3.5" />
                <span>طباعة الملصق الآن</span>
              </button>
            </div>
          </div>
        </div>
  );
};
`);

createModalFile('InventoryFifoModal', `import React from 'react';
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
          <div className="fixed inset-0 z-50 bg-black/75 backdrop-blur-xs flex items-center justify-center p-4">
            <div className="bg-[#0f233a] border border-[#1e4a77] rounded-xl shadow-2xl w-full max-w-4xl max-h-[90vh] flex flex-col overflow-hidden animate-in fade-in zoom-in-95 duration-150">
              <div className="bg-gradient-to-r from-[#143254] to-[#0f2642] px-5 py-3 border-b border-[#1d4672] flex items-center justify-between">
                <div className="flex items-center gap-3">
                  <div className={\`w-9 h-9 rounded-lg border flex items-center justify-center \${
                    isFefo 
                      ? 'bg-amber-950/80 border-amber-500/40 text-amber-400' 
                      : 'bg-emerald-950/80 border-emerald-500/40 text-emerald-400'
                  }\`}>
                    <Layers className="w-5 h-5" />
                  </div>
                  <div>
                    <div className="flex items-center gap-2 flex-wrap">
                      <h3 className="text-sm font-bold text-white">
                        {isFefo 
                          ? 'سجل طبقات التكلفة والصرف بنظام (FEFO / EEFO) - الأقرب انتهاءً يصرف أولاً'
                          : 'سجل طبقات التكلفة والصرف بنظام (FIFO) - الوارد أولاً يصرف أولاً'}
                      </h3>
                      {isFefo ? (
                        <span className="bg-amber-900/80 text-amber-300 text-[10px] font-mono px-2 py-0.5 rounded border border-amber-500/40 font-bold">
                          First-Expired, First-Out (FEFO / EEFO)
                        </span>
                      ) : (
                        <span className="bg-emerald-900/80 text-emerald-300 text-[10px] font-mono px-2 py-0.5 rounded border border-emerald-500/40 font-bold">
                          First-In, First-Out (FIFO)
                        </span>
                      )}
                    </div>
                    <div className="flex items-center gap-2 flex-wrap mt-0.5">
                      <p className="text-xs text-slate-300">
                        الصنف: <strong className="text-white">{fifoModalProduct.name}</strong> ({fifoModalProduct.code || fifoModalProduct.barcode})
                      </p>
                      {fifoModalProduct.hasExpiryDate === false ? (
                        <span className="inline-flex items-center gap-1 bg-slate-800 text-slate-300 text-[10px] px-2 py-0.5 rounded border border-slate-600">
                          <ShieldOff className="w-3 h-3 text-slate-400" />
                          <span>صنف دائم (بدون تاريخ انتهاء)</span>
                        </span>
                      ) : (
                        <span className="inline-flex items-center gap-1 bg-amber-950 text-amber-300 text-[10px] px-2 py-0.5 rounded border border-amber-600/50">
                          <ShieldCheck className="w-3 h-3 text-amber-400" />
                          <span>صنف خاضع لتاريخ الانتهاء والصلاحية</span>
                        </span>
                      )}
                    </div>
                  </div>
                </div>
                <button
                  onClick={() => setFifoModalProduct(null)}
                  className="text-slate-400 hover:text-white p-1 rounded hover:bg-[#1a3d66] transition cursor-pointer"
                >
                  <XCircle className="w-5 h-5" />
                </button>
              </div>

              <div className="p-5 overflow-y-auto space-y-4 text-xs">
                <div className={\`border rounded-lg p-3 text-slate-200 flex items-start gap-2.5 \${
                  isFefo ? 'bg-amber-950/30 border-amber-600/40' : 'bg-[#132c4a] border-[#234e7a]'
                }\`}>
                  <Sparkles className={\`w-4 h-4 shrink-0 mt-0.5 \${isFefo ? 'text-amber-400' : 'text-emerald-400'}\`} />
                  <div className="text-[11px] leading-relaxed">
                    {isFefo ? (
                      <>
                        <span className="font-bold text-amber-300">مبدأ العمل في نظام FEFO / EEFO (الأقرب انتهاءً يصرف أولاً): </span>
                        نظراً لأن هذا الصنف خاضع لتاريخ الصلاحية، يتم توجيه الصرف وتكلفة البضاعة المباعة آلياً بدءاً من <strong>الدفعة ذات تاريخ الانتهاء الأقرب</strong>، بغض النظر عما إذا كانت قد دخلت المستودع أولاً أم لا. هذا النظام يمنع تلف البضائع في المستودعات ويضمن أعلى معايير الجودة والسلامة وتقليل الهدر.
                      </>
                    ) : (
                      <>
                        <span className="font-bold text-emerald-300">مبدأ العمل في نظام FIFO (الوارد أولاً يصرف أولاً): </span>
                        نظراً لأن الصنف دائم وغير خاضع لتاريخ الصلاحية، يتم تسعير تكلفة البضاعة المباعة وصرفها بدءاً من <strong>أقدم طبقة مشتريات واردة للمستودع</strong> بسعر تكلفتها التاريخي الفعلي، مع انتقال النظام آلياً إلى الطبقات الأحدث عند نفاد القديمة.
                      </>
                    )}
                  </div>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
                  <div className="bg-[#0b1b2d] p-3 rounded-lg border border-[#1b3f66]">
                    <span className="text-slate-400 text-[11px] block">إجمالي الرصيد المتوفر</span>
                    <div className="text-lg font-black text-sky-400 font-mono mt-1">
                      {fifoModalProduct.stock} {fifoModalProduct.unit}
                    </div>
                    <span className="text-[10px] text-slate-400 mt-1 block">
                      موزعة على {activeLayers.length} طبقة نشطة
                    </span>
                  </div>

                  <div className="bg-[#0b1b2d] p-3 rounded-lg border border-[#1b3f66]">
                    <span className="text-slate-400 text-[11px] block">
                      {isFefo ? 'تكلفة الصرف القادمة (الدفعة الأقرب انتهاءً)' : 'تكلفة الصرف القادمة (أقدم دفعة واردة)'}
                    </span>
                    <div className="text-lg font-black text-amber-300 font-mono mt-1">
                      {(currentActiveLayer ? currentActiveLayer.unitCost : fifoModalProduct.costPrice).toLocaleString()} {currency}
                    </div>
                    <span className="text-[10px] text-emerald-400 mt-1 block">
                      سعر البيع الحالي: {fifoModalProduct.salePrice.toLocaleString()} {currency}
                    </span>
                  </div>

                  <div className="bg-[#0b1b2d] p-3 rounded-lg border border-[#1b3f66]">
                    <span className="text-slate-400 text-[11px] block">
                      {isFefo ? 'إجمالي القيمة الدفترية للمخزون (FEFO)' : 'إجمالي القيمة الدفترية للمخزون (FIFO)'}
                    </span>
                    <div className="text-lg font-black text-emerald-400 font-mono mt-1">
                      {totalValuation.toLocaleString()} {currency}
                    </div>
                    <span className="text-[10px] text-slate-400 mt-1 block">
                      بناءً على التكلفة الفعلية لكل طبقة واردة
                    </span>
                  </div>
                </div>

                <div className="border border-[#1b3f66] rounded-lg overflow-hidden">
                  <div className="bg-[#143254] px-3 py-2 text-white font-bold text-xs flex items-center justify-between">
                    <span>
                      {isFefo 
                        ? 'قائمة الدفعات مرتبة حسب أسبقية الصرف (FEFO / EEFO - الأقرب انتهاءً أولاً)' 
                        : 'قائمة طبقات التوريد مرتبة زمنياً (FIFO - الوارد أولاً يصرف أولاً)'}
                    </span>
                    <span className="text-[11px] text-slate-300 font-normal">
                      {isFefo ? 'مرتبة من تاريخ الانتهاء الأقرب إلى الأبعد' : 'مرتبة زمنياً من أقدم توريد إلى الأحدث'}
                    </span>
                  </div>
                  <table className="w-full text-right text-xs">
                    <thead className="bg-[#0b1b2d] text-slate-300 border-b border-[#1b3f66]">
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
                    <tbody className="divide-y divide-[#1b3f66]">
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
                              className={\`hover:bg-[#153456]/40 transition \${
                                isActive ? (isFefo ? 'bg-amber-950/20' : 'bg-emerald-950/20') : ''
                              }\`}
                            >
                              <td className="p-2.5 font-mono text-white font-semibold">
                                {layer.batchNo || layer.id}
                              </td>
                              <td className="p-2.5 text-slate-300 font-mono">
                                {layer.date}
                              </td>
                              <td className="p-2.5 text-center font-mono text-[11px]">
                                {fifoModalProduct.hasExpiryDate === false ? (
                                  <span className="text-slate-500 bg-slate-800/60 px-1.5 py-0.5 rounded text-[10px]">
                                    غير خاضع
                                  </span>
                                ) : (
                                  <span className={layer.expiryDate ? 'text-amber-300 font-semibold' : 'text-slate-400'}>
                                    {layer.expiryDate || '-'}
                                  </span>
                                )}
                              </td>
                              <td className="p-2.5 text-slate-400 font-mono">
                                {layer.purchaseInvoiceNo || 'رصيد افتتاحي'}
                              </td>
                              <td className="p-2.5 text-center font-mono text-slate-300">
                                {layer.initialQuantity}
                              </td>
                              <td className="p-2.5 text-center font-mono font-bold text-white">
                                {layer.remainingQuantity}
                              </td>
                              <td className="p-2.5 text-center font-mono text-amber-300 font-semibold">
                                {layer.unitCost.toLocaleString()} {currency}
                              </td>
                              <td className="p-2.5 text-center font-mono text-emerald-400 font-semibold">
                                {(layer.remainingQuantity * layer.unitCost).toLocaleString()} {currency}
                              </td>
                              <td className="p-2.5 text-center">
                                {isActive && (
                                  <span className={\`inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[10px] font-bold border \${
                                    isFefo
                                      ? 'bg-amber-950 text-amber-300 border-amber-500/50'
                                      : 'bg-emerald-950 text-emerald-300 border-emerald-500/50'
                                  }\`}>
                                    <CheckCircle2 className="w-3 h-3" />
                                    <span>{isFefo ? 'تصرف أولاً (الأقرب انتهاءً)' : 'تصرف أولاً (الأقدم وروداً)'}</span>
                                  </span>
                                )}
                                {isQueued && (
                                  <span className="inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[10px] font-semibold bg-sky-950 text-sky-300 border border-sky-500/50">
                                    <span>في قائمة الانتظار #{index + 1}</span>
                                  </span>
                                )}
                                {isDepleted && (
                                  <span className="inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[10px] font-semibold bg-slate-800 text-slate-400 border border-slate-700">
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

              <div className="bg-[#143254] px-5 py-3 border-t border-[#1d4672] flex items-center justify-between">
                <span className="text-[11px] text-slate-300">
                  {isFefo 
                    ? 'يتم اختيار الدفعة المنصرفة في المبيعات آلياً وفق الأقرب انتهاءً (FEFO / EEFO) لحماية المخزون من انتهاء الصلاحية'
                    : 'يتم استنزاف الطبقات زمنياً وفق أسبقية التوريد (FIFO) لحساب التكلفة بدقة'}
                </span>
                <button
                  onClick={() => setFifoModalProduct(null)}
                  className="bg-[#1c436d] hover:bg-[#25568a] text-white px-4 py-1.5 rounded text-xs font-semibold transition cursor-pointer"
                >
                  إغلاق
                </button>
              </div>
            </div>
          </div>
  );
};
`);

fs.writeFileSync('components/inventory/index.ts', `export * from './InventoryAddProductModal';
export * from './InventoryBarcodeModal';
export * from './InventoryFifoModal';
`);

let moduleContent = fs.readFileSync('components/InventoryModule.tsx', 'utf8');

// Insert imports
const importRegex = /import React, \{ useState, useEffect \} from 'react';/;
const newImports = `import React, { useState, useEffect } from 'react';
import {
  InventoryAddProductModal,
  InventoryBarcodeModal,
  InventoryFifoModal,
} from './inventory';`;
moduleContent = moduleContent.replace(importRegex, newImports);

// Replace the JSX for the modals
const startIndex = moduleContent.indexOf('{/* Add / Edit Product & Multi-Units Modal */}');
const endIndex = moduleContent.lastIndexOf('</>\n      )}\n    </div>\n  );\n};');

if (startIndex === -1 || endIndex === -1) {
  console.log('Error: Could not find inventory modal slice');
  process.exit(1);
}

const replacementJSX = `{/* Add / Edit Product & Multi-Units Modal */}
      <InventoryAddProductModal
        isOpen={showItemModal}
        onClose={() => setShowItemModal(false)}
        editingItem={editingItem}
        handleSaveItem={handleSaveItem}
        formCode={formCode}
        setFormCode={setFormCode}
        formBarcode={formBarcode}
        setFormBarcode={setFormBarcode}
        formName={formName}
        setFormName={setFormName}
        formCategory={formCategory}
        setFormCategory={setFormCategory}
        categories={categories}
        formUnits={formUnits}
        handleApplyMarkupToUnits={handleApplyMarkupToUnits}
        handleSyncSubUnitsFromBase={handleSyncSubUnitsFromBase}
        handleAddUnitRow={handleAddUnitRow}
        handleUpdateUnitRow={handleUpdateUnitRow}
        handleRemoveUnitRow={handleRemoveUnitRow}
        formHasExpiryDate={formHasExpiryDate}
        setFormHasExpiryDate={setFormHasExpiryDate}
        setFormValuationMethod={setFormValuationMethod}
        formExpiryDate={formExpiryDate}
        setFormExpiryDate={setFormExpiryDate}
        formStock={formStock}
        setFormStock={setFormStock}
        formMinStock={formMinStock}
        setFormMinStock={setFormMinStock}
        formBatchNo={formBatchNo}
        setFormBatchNo={setFormBatchNo}
        currency={currency}
      />

      {/* Barcode Label Print Modal with Unit Selector */}
      <InventoryBarcodeModal
        barcodePreviewItem={barcodePreviewItem}
        setBarcodePreviewItem={setBarcodePreviewItem}
        selectedUnitForBarcode={selectedUnitForBarcode}
        setSelectedUnitForBarcode={setSelectedUnitForBarcode}
        currency={currency}
      />

      {/* FIFO / FEFO Stock Layers Inspection Modal */}
      <InventoryFifoModal
        fifoModalProduct={fifoModalProduct}
        setFifoModalProduct={setFifoModalProduct}
        currency={currency}
      />
    `;

const newModuleContent = moduleContent.substring(0, startIndex) + replacementJSX + moduleContent.substring(endIndex);
fs.writeFileSync('components/InventoryModule.tsx', newModuleContent);
console.log('InventoryModule.tsx refactored.');
