import React from 'react';
import { Building2, X, MapPin, Phone, User, CheckCircle2, Warehouse as WarehouseIcon } from 'lucide-react';
import { Warehouse } from '@/types/pos';

interface WarehouseManageModalProps {
  showNewWarehouseModal: boolean;
  setShowNewWarehouseModal: (v: boolean) => void;
  handleSaveWarehouse: () => void;
  editingWarehouse: Warehouse | null;
  whCode: string;
  setWhCode: (v: string) => void;
  whName: string;
  setWhName: (v: string) => void;
  whBranchId: number;
  setWhBranchId: (v: number) => void;
  whBranchName: string;
  setWhBranchName: (v: string) => void;
  whManager: string;
  setWhManager: (v: string) => void;
  whPhone: string;
  setWhPhone: (v: string) => void;
  whLocation: string;
  setWhLocation: (v: string) => void;
  whAccountCode: string;
  setWhAccountCode: (v: string) => void;
  whNotes: string;
  setWhNotes: (v: string) => void;
  whIsDefault: boolean;
  setWhIsDefault: (v: boolean) => void;
}

export const WarehouseManageModal: React.FC<WarehouseManageModalProps> = ({
  showNewWarehouseModal,
  setShowNewWarehouseModal,
  handleSaveWarehouse,
  editingWarehouse,
  whCode,
  setWhCode,
  whName,
  setWhName,
  whBranchId,
  setWhBranchId,
  whBranchName,
  setWhBranchName,
  whManager,
  setWhManager,
  whPhone,
  setWhPhone,
  whLocation,
  setWhLocation,
  whAccountCode,
  setWhAccountCode,
  whNotes,
  setWhNotes,
  whIsDefault,
  setWhIsDefault,
}) => {
  if (!showNewWarehouseModal) return null;

  return (
    <div className="fixed inset-0 bg-black/60 z-50 flex items-center justify-center p-3 backdrop-blur-xs">
      <div className="bg-white rounded-lg shadow-2xl border border-slate-400 w-full max-w-lg overflow-hidden flex flex-col">
        {/* Header */}
        <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between shrink-0 text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5 font-bold text-sm text-slate-900">
            <div className="w-8 h-8 rounded-lg bg-emerald-50 border border-emerald-200 flex items-center justify-center text-emerald-700 shadow-xs">
              <WarehouseIcon className="w-4 h-4 text-emerald-700" />
            </div>
            <span>{editingWarehouse ? 'تعديل بيانات المستودع' : 'إضافة بطاقة مستودع / فرع جديد'}</span>
          </div>
          <button
            type="button"
            onClick={() => setShowNewWarehouseModal(false)}
            className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg cursor-pointer transition"
          >
            <X className="w-4 h-4" />
          </button>
        </div>

        {/* Body */}
        <div className="p-4 space-y-3 text-xs">
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">كود المستودع:</label>
              <input
                type="text"
                value={whCode}
                onChange={(e) => setWhCode(e.target.value)}
                placeholder="مثال: WH-03"
                className="w-full p-2 bg-slate-50 border border-slate-300 rounded font-mono font-bold focus:bg-white focus:outline-none focus:border-sky-500"
              />
            </div>
            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">اسم المستودع:</label>
              <input
                type="text"
                value={whName}
                onChange={(e) => setWhName(e.target.value)}
                placeholder="مثال: مستودع تعز المركزي"
                className="w-full p-2 bg-slate-50 border border-slate-300 rounded font-bold focus:bg-white focus:outline-none focus:border-sky-500"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">أمين المستودع:</label>
              <input
                type="text"
                value={whManager}
                onChange={(e) => setWhManager(e.target.value)}
                placeholder="اسم المسؤول"
                className="w-full p-2 bg-slate-50 border border-slate-300 rounded focus:bg-white focus:outline-none focus:border-sky-500"
              />
            </div>
            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">رقم الهاتف:</label>
              <input
                type="text"
                value={whPhone}
                onChange={(e) => setWhPhone(e.target.value)}
                placeholder="770000000"
                className="w-full p-2 bg-slate-50 border border-slate-300 rounded font-mono focus:bg-white focus:outline-none focus:border-sky-500"
              />
            </div>
          </div>

          <div>
            <label className="block text-[11px] font-bold text-slate-700 mb-1">الموقع / العنوان:</label>
            <input
              type="text"
              value={whLocation}
              onChange={(e) => setWhLocation(e.target.value)}
              placeholder="المدينة، الشارع، رقم المبنى"
              className="w-full p-2 bg-slate-50 border border-slate-300 rounded focus:bg-white focus:outline-none focus:border-sky-500"
            />
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="block text-[11px] font-bold text-slate-700 mb-1">رقم الحساب المحاسبي:</label>
              <input
                type="text"
                value={whAccountCode}
                onChange={(e) => setWhAccountCode(e.target.value)}
                placeholder="1141"
                className="w-full p-2 bg-slate-50 border border-slate-300 rounded font-mono focus:bg-white focus:outline-none focus:border-sky-500"
              />
            </div>
            <div className="flex items-center pt-5">
              <label className="flex items-center gap-2 cursor-pointer font-bold text-slate-700">
                <input
                  type="checkbox"
                  checked={whIsDefault}
                  onChange={(e) => setWhIsDefault(e.target.checked)}
                  className="rounded text-emerald-600"
                />
                <span>تعيين كمستودع افتراضي</span>
              </label>
            </div>
          </div>

          <div>
            <label className="block text-[11px] font-bold text-slate-700 mb-1">ملاحظات إضافية:</label>
            <textarea
              rows={2}
              value={whNotes}
              onChange={(e) => setWhNotes(e.target.value)}
              placeholder="ملاحظات حول سعة المستودع أو شروط التخزين..."
              className="w-full p-2 bg-slate-50 border border-slate-300 rounded focus:bg-white focus:outline-none focus:border-sky-500"
            />
          </div>

          {/* Action Buttons */}
          <div className="flex items-center justify-end gap-2 pt-3 border-t border-slate-200">
            <button
              type="button"
              onClick={() => setShowNewWarehouseModal(false)}
              className="px-4 py-1.5 border border-slate-300 rounded text-slate-700 hover:bg-slate-100 font-bold cursor-pointer"
            >
              إلغاء
            </button>
            <button
              type="button"
              onClick={handleSaveWarehouse}
              className="px-5 py-1.5 bg-[#143254] hover:bg-[#1a406c] text-white rounded font-bold shadow-xs transition flex items-center gap-1.5 cursor-pointer"
            >
              <CheckCircle2 className="w-4 h-4 text-emerald-400" />
              <span>{editingWarehouse ? 'حفظ التعديلات' : 'إضافة المستودع'}</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
