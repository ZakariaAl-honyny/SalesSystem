import React from 'react';
import {  Plus, X, Search, FileText, Calendar, Users, ClipboardList , ClipboardCheck, Check } from 'lucide-react';
import { Warehouse, ProductItem } from '@/types/pos';

interface StocktakingNewModalProps {
  showNewStocktakeModal: boolean;
  setShowNewStocktakeModal: (v: boolean) => void;
  handleStartNewStocktake: (e: React.FormEvent) => void;
  newStWarehouseId: string;
  setNewStWarehouseId: (v: string) => void;
  newStNotes: string;
  setNewStNotes: (v: string) => void;
  newStHeadMember: string;
  setNewStHeadMember: (v: string) => void;
  newStMember2: string;
  setNewStMember2: (v: string) => void;
  newStMember3: string;
  setNewStMember3: (v: string) => void;
  warehouses: Warehouse[];
  products: ProductItem[];
}

export const StocktakingNewModal: React.FC<StocktakingNewModalProps> = ({
  showNewStocktakeModal, setShowNewStocktakeModal, handleStartNewStocktake, newStWarehouseId, setNewStWarehouseId,
  newStNotes, setNewStNotes, newStHeadMember, setNewStHeadMember, newStMember2, setNewStMember2, newStMember3, setNewStMember3,
  warehouses, products
}) => {
  if (!showNewStocktakeModal) return null;
  return (
        <div className="fixed inset-0 z-50 bg-slate-950/60 backdrop-blur-xs flex items-center justify-center p-4">
          <div className="bg-white border border-slate-300 rounded-xl shadow-2xl max-w-xl w-full text-slate-800 overflow-hidden animate-in fade-in zoom-in duration-150">
            <div className="bg-white px-4 py-3.5 border-b border-slate-200 flex items-center justify-between text-slate-800 shadow-2xs">
              <div className="flex items-center gap-2.5">
                <div className="w-8 h-8 rounded-lg bg-emerald-50 border border-emerald-200 flex items-center justify-center text-emerald-700 shadow-xs">
                  <ClipboardCheck className="w-4 h-4 text-emerald-700" />
                </div>
                <h3 className="font-bold text-sm text-slate-900">
                  فتح محضر جرد فعلي جديد لمستودع
                </h3>
              </div>
              <button
                onClick={() => setShowNewStocktakeModal(false)}
                className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg cursor-pointer transition"
              >
                ✕
              </button>
            </div>

            <div className="p-4 space-y-3.5 text-xs bg-slate-50/50">
              <div>
                <label className="block text-slate-700 font-bold mb-1">
                  اختر المستودع المراد جرده:
                </label>
                <select
                  value={newStWarehouseId}
                  onChange={(e) => setNewStWarehouseId(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded-lg px-3 py-2 text-slate-900 font-bold focus:outline-none focus:ring-1 focus:ring-emerald-500 cursor-pointer shadow-2xs"
                >
                  {warehouses.map((w) => (
                    <option key={w.id} value={w.id}>
                      {w.name} - المسؤول: {w.managerName}
                    </option>
                  ))}
                </select>
                <span className="text-[11px] text-slate-500 mt-1 block">
                  * سيتم تحميل كافة الأصناف المسجلة في النظام مع رصيدها الدفتري الحالي في هذا المستودع.
                </span>
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
                <div>
                  <label className="block text-slate-700 font-semibold mb-1">
                    رئيس لجنة الجرد:
                  </label>
                  <input
                    type="text"
                    value={newStHeadMember}
                    onChange={(e) => setNewStHeadMember(e.target.value)}
                    className="w-full bg-white border border-slate-300 rounded-lg px-2.5 py-1.5 text-slate-900 font-medium focus:outline-none focus:ring-1 focus:ring-emerald-500"
                  />
                </div>
                <div>
                  <label className="block text-slate-700 font-semibold mb-1">
                    عضو ومراجع:
                  </label>
                  <input
                    type="text"
                    value={newStMember2}
                    onChange={(e) => setNewStMember2(e.target.value)}
                    className="w-full bg-white border border-slate-300 rounded-lg px-2.5 py-1.5 text-slate-900 font-medium focus:outline-none focus:ring-1 focus:ring-emerald-500"
                  />
                </div>
                <div>
                  <label className="block text-slate-700 font-semibold mb-1">
                    أمين المستودع:
                  </label>
                  <input
                    type="text"
                    value={newStMember3}
                    onChange={(e) => setNewStMember3(e.target.value)}
                    className="w-full bg-white border border-slate-300 rounded-lg px-2.5 py-1.5 text-slate-900 font-medium focus:outline-none focus:ring-1 focus:ring-emerald-500"
                  />
                </div>
              </div>

              <div>
                <label className="block text-slate-700 font-semibold mb-1">
                  بيان وملاحظات الجرد:
                </label>
                <input
                  type="text"
                  placeholder="مثال: الجرد الدوري الشهري لشهر سبتمبر 2026..."
                  value={newStNotes}
                  onChange={(e) => setNewStNotes(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded-lg px-3 py-1.5 text-slate-900 font-medium focus:outline-none focus:ring-1 focus:ring-emerald-500"
                />
              </div>
            </div>

            <div className="bg-slate-50 px-4 py-3 border-t border-slate-200 flex items-center justify-between">
              <button
                type="button"
                onClick={() => setShowNewStocktakeModal(false)}
                className="bg-white border border-slate-300 text-slate-700 px-3.5 py-1.5 rounded-lg text-xs hover:bg-slate-100 font-medium cursor-pointer transition"
              >
                إلغاء
              </button>
              <button
                type="button"
                onClick={handleStartNewStocktake}
                className="bg-emerald-600 hover:bg-emerald-500 text-white px-4 py-1.5 rounded-lg text-xs font-bold flex items-center gap-1.5 cursor-pointer shadow-xs transition"
              >
                <Check className="w-4 h-4" />
                <span>بدء الجرد الفعلي وتوليد القائمة</span>
              </button>
            </div>
          </div>
        </div>
      );
};
