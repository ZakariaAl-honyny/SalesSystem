import React from 'react';
import {
  ArrowLeftRight, X, Package, Plus, Trash2, Calendar, FileText,
  AlertTriangle, Clock, Truck, ShieldAlert, CheckCircle2, User, Car
} from 'lucide-react';
import { Warehouse, ProductItem, StockTransferItem } from '@/types/pos';

interface WarehouseNewTransferModalProps {
  showNewTransferModal: boolean;
  setShowNewTransferModal: (v: boolean) => void;
  handleSubmitTransfer: (dispatchImmediately: boolean) => void;
  transferDate: string;
  setTransferDate: (v: string) => void;
  fromWarehouseId: string;
  setFromWarehouseId: (v: string) => void;
  toWarehouseId: string;
  setToWarehouseId: (v: string) => void;
  driverName: string;
  setDriverName: (v: string) => void;
  vehiclePlate: string;
  setVehiclePlate: (v: string) => void;
  transferRef: string;
  setTransferRef: (v: string) => void;
  transferNotes: string;
  setTransferNotes: (v: string) => void;
  transferItems: StockTransferItem[];
  transferError: string | null;
  warehouses: Warehouse[];
  products: ProductItem[];
  selectedProductId: string;
  setSelectedProductId: (v: string) => void;
  selectedQty: number;
  setSelectedQty: (v: number) => void;
  selectedUnit: string;
  setSelectedUnit: (v: string) => void;
  handleAddItemToTransfer: () => void;
  handleRemoveTransferItem: (itemId: string) => void;
  getProductStockInWarehouse: (product: ProductItem, warehouseId: string) => number;
  currency: string;
}

export const WarehouseNewTransferModal: React.FC<WarehouseNewTransferModalProps> = ({
  showNewTransferModal,
  setShowNewTransferModal,
  handleSubmitTransfer,
  transferDate,
  setTransferDate,
  fromWarehouseId,
  setFromWarehouseId,
  toWarehouseId,
  setToWarehouseId,
  driverName,
  setDriverName,
  vehiclePlate,
  setVehiclePlate,
  transferRef,
  setTransferRef,
  transferNotes,
  setTransferNotes,
  transferItems,
  transferError,
  warehouses,
  products,
  selectedProductId,
  setSelectedProductId,
  selectedQty,
  setSelectedQty,
  selectedUnit,
  setSelectedUnit,
  handleAddItemToTransfer,
  handleRemoveTransferItem,
  getProductStockInWarehouse,
  currency,
}) => {
  if (!showNewTransferModal) return null;

  const currentProd = products.find((p) => p.id === selectedProductId) || products[0];
  const sourceStock = currentProd ? getProductStockInWarehouse(currentProd, fromWarehouseId) : 0;
  const totalCost = transferItems.reduce((sum, item) => sum + item.unitCost * item.quantity, 0);

  return (
    <div className="fixed inset-0 bg-black/60 z-50 flex items-center justify-center p-3 backdrop-blur-xs">
      <div className="bg-white rounded-lg shadow-2xl border border-slate-400 w-full max-w-4xl overflow-hidden flex flex-col max-h-[92vh]">
        {/* Header */}
        <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between shrink-0 text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5 font-bold text-sm text-slate-900">
            <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
              <ArrowLeftRight className="w-4 h-4 text-sky-700" />
            </div>
            <span>إنشاء أمر ومستند مناقلة مخزنية بين المستودعات والفروع</span>
          </div>
          <button
            type="button"
            onClick={() => setShowNewTransferModal(false)}
            className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg cursor-pointer transition"
          >
            <X className="w-4 h-4" />
          </button>
        </div>

        {/* Body */}
        <div className="p-4 space-y-3 overflow-y-auto flex-1 text-xs">
          {transferError && (
            <div className="p-2.5 bg-rose-50 border border-rose-300 rounded text-rose-800 flex items-center gap-2 font-bold">
              <AlertTriangle className="w-4 h-4 shrink-0 text-rose-600" />
              <span>{transferError}</span>
            </div>
          )}

          {/* Form Top Row: Warehouses & Date */}
          <div className="grid grid-cols-1 md:grid-cols-4 gap-3 bg-slate-50 p-3 rounded-lg border border-slate-200">
            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">
                من مستودع (المصدر):
              </label>
              <select
                value={fromWarehouseId}
                onChange={(e) => setFromWarehouseId(e.target.value)}
                className="w-full p-2 bg-white border border-slate-300 rounded font-bold text-slate-800 focus:outline-none focus:border-sky-500"
              >
                {warehouses.map((w) => (
                  <option key={w.id} value={w.id} disabled={w.id === toWarehouseId}>
                    {w.name} {w.isDefault ? '⭐' : ''}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">
                إلى مستودع (الوجهة):
              </label>
              <select
                value={toWarehouseId}
                onChange={(e) => setToWarehouseId(e.target.value)}
                className="w-full p-2 bg-white border border-slate-300 rounded font-bold text-slate-800 focus:outline-none focus:border-sky-500"
              >
                {warehouses.map((w) => (
                  <option key={w.id} value={w.id} disabled={w.id === fromWarehouseId}>
                    {w.name} {w.isDefault ? '⭐' : ''}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">
                تاريخ المناقلة:
              </label>
              <input
                type="date"
                value={transferDate}
                onChange={(e) => setTransferDate(e.target.value)}
                className="w-full p-2 bg-white border border-slate-300 rounded text-xs font-mono focus:outline-none focus:border-sky-500"
              />
            </div>

            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">
                المرجع / رقم الطلب:
              </label>
              <input
                type="text"
                value={transferRef}
                onChange={(e) => setTransferRef(e.target.value)}
                placeholder="رقم طلب المناقلة أو المرجع"
                className="w-full p-2 bg-white border border-slate-300 rounded text-xs focus:outline-none focus:border-sky-500"
              />
            </div>
          </div>

          {/* Transport Info */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3 bg-slate-50 p-2.5 rounded-lg border border-slate-200">
            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">
                اسم السائق / الناقل:
              </label>
              <div className="relative">
                <User className="w-3.5 h-3.5 text-slate-400 absolute right-2.5 top-2.5" />
                <input
                  type="text"
                  value={driverName}
                  onChange={(e) => setDriverName(e.target.value)}
                  placeholder="اسم سائق شاحنة النقل"
                  className="w-full p-1.5 pr-8 bg-white border border-slate-300 rounded text-xs focus:outline-none focus:border-sky-500"
                />
              </div>
            </div>
            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">
                رقم لوحة المركبة / الشاحنة:
              </label>
              <div className="relative">
                <Car className="w-3.5 h-3.5 text-slate-400 absolute right-2.5 top-2.5" />
                <input
                  type="text"
                  value={vehiclePlate}
                  onChange={(e) => setVehiclePlate(e.target.value)}
                  placeholder="رقم لوحة المركبة"
                  className="w-full p-1.5 pr-8 bg-white border border-slate-300 rounded text-xs focus:outline-none focus:border-sky-500"
                />
              </div>
            </div>
          </div>

          {/* Item Selector Row */}
          <div className="bg-sky-50/60 p-3 rounded-lg border border-sky-200">
            <div className="font-bold text-xs text-sky-900 mb-2 flex items-center gap-1.5">
              <Package className="w-4 h-4 text-sky-700" />
              <span>إضافة أصناف لأمر التحويل:</span>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-12 gap-2 items-end">
              <div className="md:col-span-6">
                <label className="block text-[11px] font-bold text-slate-700 mb-1">الصنف:</label>
                <select
                  value={selectedProductId}
                  onChange={(e) => {
                    setSelectedProductId(e.target.value);
                    const p = products.find((x) => x.id === e.target.value);
                    if (p) setSelectedUnit(p.unit || 'حبة');
                  }}
                  className="w-full p-2 bg-white border border-slate-300 rounded text-xs font-bold focus:outline-none focus:border-sky-500"
                >
                  {products.map((p) => (
                    <option key={p.id} value={p.id}>
                      {p.code} - {p.name} (رصيد المصدر: {getProductStockInWarehouse(p, fromWarehouseId)})
                    </option>
                  ))}
                </select>
              </div>

              <div className="md:col-span-2">
                <label className="block text-[11px] font-bold text-slate-700 mb-1">الوحدة:</label>
                <select
                  value={selectedUnit}
                  onChange={(e) => setSelectedUnit(e.target.value)}
                  className="w-full p-2 bg-white border border-slate-300 rounded text-xs focus:outline-none focus:border-sky-500"
                >
                  <option value={currentProd?.unit || 'حبة'}>{currentProd?.unit || 'حبة'}</option>
                  {currentProd?.units?.map((u, i) => (
                    <option key={i} value={u.name}>
                      {u.name}
                    </option>
                  ))}
                </select>
              </div>

              <div className="md:col-span-2">
                <label className="block text-[11px] font-bold text-slate-700 mb-1">الكمية:</label>
                <input
                  type="number"
                  min={1}
                  max={sourceStock > 0 ? sourceStock : 9999}
                  value={selectedQty}
                  onChange={(e) => setSelectedQty(Math.max(1, Number(e.target.value)))}
                  className="w-full p-2 bg-white border border-slate-300 rounded text-xs font-bold text-center focus:outline-none focus:border-sky-500"
                />
              </div>

              <div className="md:col-span-2">
                <button
                  type="button"
                  onClick={handleAddItemToTransfer}
                  className="w-full py-2 bg-sky-700 hover:bg-sky-800 text-white rounded text-xs font-bold flex items-center justify-center gap-1 shadow-xs transition cursor-pointer"
                >
                  <Plus className="w-4 h-4" />
                  <span>إضافة</span>
                </button>
              </div>
            </div>
          </div>

          {/* Items Table */}
          <div className="border border-slate-300 rounded-lg overflow-hidden">
            <table className="w-full text-right text-xs">
              <thead className="bg-[#123e68] text-white">
                <tr>
                  <th className="p-2 w-10 text-center">#</th>
                  <th className="p-2">كود الصنف</th>
                  <th className="p-2">اسم الصنف</th>
                  <th className="p-2 text-center">الوحدة</th>
                  <th className="p-2 text-center">الكمية</th>
                  <th className="p-2 text-center">التكلفة الفردية</th>
                  <th className="p-2 text-center">الإجمالي</th>
                  <th className="p-2 w-12 text-center">حذف</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200">
                {transferItems.length === 0 ? (
                  <tr>
                    <td colSpan={8} className="p-4 text-center text-slate-400">
                      لم يتم إضافة أصناف بعد. اختر الصنف والكمية واضغط &quot;إضافة&quot;.
                    </td>
                  </tr>
                ) : (
                  transferItems.map((item, idx) => (
                    <tr key={idx} className="hover:bg-slate-50">
                      <td className="p-2 text-center font-mono text-slate-500">{idx + 1}</td>
                      <td className="p-2 font-mono font-bold text-slate-700">{item.productCode}</td>
                      <td className="p-2 font-bold text-slate-800">{item.productName}</td>
                      <td className="p-2 text-center">{item.unit}</td>
                      <td className="p-2 text-center font-bold text-emerald-700">{item.quantity}</td>
                      <td className="p-2 text-center font-mono">{item.unitCost.toFixed(2)}</td>
                      <td className="p-2 text-center font-mono font-bold">
                        {(item.unitCost * item.quantity).toFixed(2)}
                      </td>
                      <td className="p-2 text-center">
                        <button
                          type="button"
                          onClick={() => handleRemoveTransferItem(item.productId)}
                          className="text-rose-600 hover:text-rose-800 p-1 hover:bg-rose-50 rounded cursor-pointer"
                        >
                          <Trash2 className="w-4 h-4" />
                        </button>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
              {transferItems.length > 0 && (
                <tfoot className="bg-slate-100 font-bold border-t border-slate-300">
                  <tr>
                    <td colSpan={4} className="p-2 text-slate-700">
                      الإجمالي ({transferItems.length} صنف):
                    </td>
                    <td className="p-2 text-center text-emerald-700 font-bold">
                      {transferItems.reduce((sum, it) => sum + it.quantity, 0)}
                    </td>
                    <td></td>
                    <td className="p-2 text-center font-mono font-bold text-sky-900">
                      {totalCost.toFixed(2)} {currency}
                    </td>
                    <td></td>
                  </tr>
                </tfoot>
              )}
            </table>
          </div>

          {/* Notes */}
          <div>
            <label className="block text-[11px] font-bold text-slate-700 mb-1">
              ملاحظات وتعليمات الشحنة:
            </label>
            <textarea
              rows={2}
              value={transferNotes}
              onChange={(e) => setTransferNotes(e.target.value)}
              placeholder="ملاحظات التسليم، حالة الطرود، أو سبب المناقلة..."
              className="w-full p-2 bg-slate-50 border border-slate-300 rounded text-xs focus:bg-white focus:outline-none focus:border-sky-500"
            />
          </div>

          {/* Action Buttons */}
          <div className="flex items-center justify-between pt-2 border-t border-slate-200">
            <div className="text-[11px] text-slate-500">
              عند الصرف المباشر يتم حسم الكميات من مستودع المصدر وإدخالها في الترانزيت
            </div>
            <div className="flex items-center gap-2">
              <button
                type="button"
                onClick={() => setShowNewTransferModal(false)}
                className="px-3.5 py-1.5 border border-slate-300 rounded text-slate-700 hover:bg-slate-100 font-bold cursor-pointer"
              >
                إلغاء الأمر
              </button>
              <button
                type="button"
                onClick={() => handleSubmitTransfer(false)}
                className="px-3.5 py-1.5 bg-slate-700 hover:bg-slate-800 text-white rounded font-bold shadow-xs transition flex items-center gap-1 cursor-pointer"
              >
                <Clock className="w-3.5 h-3.5" />
                <span>حفظ كمسودة [Draft]</span>
              </button>
              <button
                type="button"
                onClick={() => handleSubmitTransfer(true)}
                className="px-4 py-1.5 bg-emerald-700 hover:bg-emerald-800 text-white rounded font-bold shadow-xs transition flex items-center gap-1 cursor-pointer"
              >
                <Truck className="w-3.5 h-3.5" />
                <span>صرف وإرسال الشحنة فـوراً</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
