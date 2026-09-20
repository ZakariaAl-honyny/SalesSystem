'use client';

import React, { useState } from 'react';
import { ShiftInfo, SystemUser, ShiftMode } from '@/types/pos';
import { Play, X, Coins, Clock, Building2, User, Sparkles } from 'lucide-react';

interface OpenShiftModalProps {
  isOpen: boolean;
  onClose: () => void;
  onOpenShift: (newShift: ShiftInfo) => void;
  lastShiftNumber: number;
  users: SystemUser[];
  currentUser: SystemUser | null;
  currency: string;
  shiftMode: ShiftMode;
}

export const OpenShiftModal: React.FC<OpenShiftModalProps> = ({
  isOpen,
  onClose,
  onOpenShift,
  lastShiftNumber,
  users,
  currentUser,
  currency,
  shiftMode,
}) => {
  const nextShiftNumber = (lastShiftNumber || 100) + 1;
  const [shiftNumber, setShiftNumber] = useState<number>(nextShiftNumber);
  const [selectedUserId, setSelectedUserId] = useState<string>(currentUser?.id || users[0]?.id || 'u1');
  const [selectedBranch, setSelectedBranch] = useState<string>(currentUser?.branchName || 'الفرع الرئيسي');
  const [openingCashInput, setOpeningCashInput] = useState<string>('50000');

  if (!isOpen) return null;

  const selectedUser = users.find((u) => u.id === selectedUserId);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const openingAmount = parseFloat(openingCashInput) || 0;
    const now = new Date();
    const dateStr = now.toISOString().split('T')[0];
    const timeStr = now.toLocaleTimeString('ar-YE', { hour: '2-digit', minute: '2-digit' });

    const newShift: ShiftInfo = {
      shiftNumber,
      cashierId: selectedUser?.id,
      cashierName: selectedUser?.fullName || currentUser?.fullName || 'كاشير المحل',
      branchName: selectedBranch,
      openedAt: `${dateStr} ${timeStr}`,
      openingCash: openingAmount,
      currentCashInDrawer: openingAmount,
      totalSales: 0,
      totalReturns: 0,
      totalCreditSales: 0,
      status: 'OPEN',
      mode: shiftMode,
    };

    onOpenShift(newShift);
    onClose();
  };

  return (
    <div 
      id="open-shift-modal-backdrop"
      className="fixed inset-0 bg-slate-950/60 backdrop-blur-xs flex items-center justify-center z-50 p-4 font-sans select-none" 
      dir="rtl"
    >
      <div 
        id="open-shift-modal-container"
        className="bg-white text-slate-800 rounded-xl shadow-2xl border border-slate-300 w-full max-w-md overflow-hidden animate-in fade-in zoom-in-95"
      >
        <div className="bg-white px-4 py-3.5 border-b border-slate-200 flex items-center justify-between text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-emerald-50 border border-emerald-200 flex items-center justify-center text-emerald-700 shadow-xs">
              <Play className="w-4 h-4 text-emerald-700" />
            </div>
            <div>
              <h3 className="font-bold text-xs sm:text-sm text-slate-900">
                {shiftMode === 'DAILY_SIMPLE' ? 'بدء الوردية اليومية للمحل' : 'فتح وردية كاشير جديدة (New Shift)'}
              </h3>
              <p className="text-[11px] text-slate-500">
                {shiftMode === 'DAILY_SIMPLE' 
                  ? 'تسجيل بدء مبيعات اليوم وتحديد عهدة الفكة' 
                  : 'تجهيز جلسة الصندوق وتعيين العهدة للكاشير'}
              </p>
            </div>
          </div>

          <button
            onClick={onClose}
            className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg cursor-pointer transition"
          >
            <X className="w-4 h-4" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-5 space-y-4 text-xs bg-slate-50/50">
          {/* Shift Number & Mode */}
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="block text-slate-700 font-semibold mb-1">رقم الوردية التسلسلي:</label>
              <div className="relative">
                <input
                  type="number"
                  value={shiftNumber}
                  onChange={(e) => setShiftNumber(parseInt(e.target.value) || nextShiftNumber)}
                  className="w-full bg-white border border-slate-300 rounded-lg px-3 py-2 text-slate-900 font-mono font-bold text-sm focus:outline-none focus:ring-1 focus:ring-sky-500 shadow-2xs"
                  required
                />
                <Clock className="w-4 h-4 text-slate-400 absolute left-2.5 top-2.5" />
              </div>
            </div>

            <div>
              <label className="block text-slate-700 font-semibold mb-1">النمط التشغيلي:</label>
              <div className="bg-white border border-slate-300 rounded-lg px-3 py-2 text-sky-700 font-bold text-xs flex items-center gap-1.5 h-[38px] shadow-2xs">
                <Sparkles className="w-3.5 h-3.5 text-amber-500" />
                <span>{shiftMode === 'DAILY_SIMPLE' ? 'وردية يومية' : 'مؤسسي متعدد'}</span>
              </div>
            </div>
          </div>

          {/* Cashier Selection */}
          <div>
            <label className="block text-slate-700 font-semibold mb-1">الكاشير المسندة إليه الوردية:</label>
            <div className="relative">
              <select
                value={selectedUserId}
                onChange={(e) => setSelectedUserId(e.target.value)}
                className="w-full bg-white border border-slate-300 rounded-lg px-3 py-2 text-slate-900 font-medium text-xs focus:outline-none focus:ring-1 focus:ring-sky-500 pr-8 shadow-2xs cursor-pointer"
              >
                {users.map((u) => (
                  <option key={u.id} value={u.id}>
                    {u.fullName} ({u.roleName || 'مستخدم'})
                  </option>
                ))}
              </select>
              <User className="w-4 h-4 text-slate-400 absolute right-2.5 top-2.5" />
            </div>
          </div>

          {/* Branch */}
          <div>
            <label className="block text-slate-700 font-semibold mb-1">الفرع / نقطة البيع:</label>
            <div className="relative">
              <select
                value={selectedBranch}
                onChange={(e) => setSelectedBranch(e.target.value)}
                className="w-full bg-white border border-slate-300 rounded-lg px-3 py-2 text-slate-900 font-medium text-xs focus:outline-none focus:ring-1 focus:ring-sky-500 pr-8 shadow-2xs cursor-pointer"
              >
                <option value="الفرع الرئيسي">الفرع الرئيسي - العاصمة</option>
                <option value="فرع مأرب">فرع مأرب - Branch 02</option>
                <option value="فرع تعز">فرع تعز - Branch 03</option>
                <option value="فرع الحديدة">فرع الحديدة - Branch 04</option>
              </select>
              <Building2 className="w-4 h-4 text-slate-400 absolute right-2.5 top-2.5" />
            </div>
          </div>

          {/* Opening Float Cash */}
          <div>
            <label className="block text-slate-700 font-semibold mb-1">
              الرصيد الافتتاحي للدرج (عهدة الفكة / Cash Float):
            </label>
            <div className="relative">
              <input
                type="number"
                value={openingCashInput}
                onChange={(e) => setOpeningCashInput(e.target.value)}
                placeholder="0"
                className="w-full bg-white border-2 border-emerald-500 rounded-lg px-3 py-2 text-emerald-800 font-mono font-black text-sm focus:outline-none focus:ring-2 focus:ring-emerald-400 pl-16 shadow-2xs"
                required
              />
              <span className="absolute left-3 top-2.5 text-xs font-bold text-slate-500 font-mono">
                {currency}
              </span>
              <Coins className="w-4 h-4 text-emerald-600 absolute right-2.5 top-2.5" />
            </div>
            <p className="text-[11px] text-slate-500 mt-1">
              المبلغ النقدي المسلم للكاشير في بداية الفترة لإرجاع الباقي للزبائن.
            </p>
          </div>

          {/* Action Buttons */}
          <div className="pt-3 border-t border-slate-200 flex items-center justify-end gap-2">
            <button
              type="button"
              onClick={onClose}
              className="bg-white border border-slate-300 hover:bg-slate-100 text-slate-700 px-4 py-2 rounded-lg text-xs font-medium cursor-pointer transition"
            >
              إلغاء
            </button>
            <button
              type="submit"
              className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-5 py-2 rounded-lg text-xs flex items-center gap-1.5 shadow-xs transition cursor-pointer"
            >
              <Play className="w-3.5 h-3.5" />
              <span>بدء الوردية رسمياً</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
