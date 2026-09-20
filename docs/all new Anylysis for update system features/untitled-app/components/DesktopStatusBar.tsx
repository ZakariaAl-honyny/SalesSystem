'use client';

import React, { useState, useEffect } from 'react';
import { Database, User, ShieldCheck, Clock, KeyRound, HardDrive } from 'lucide-react';

interface DesktopStatusBarProps {
  cashierName: string;
  shiftNumber: number;
  currency: string;
  totalProductsCount: number;
  totalInvoicesCount: number;
  onOpenLogin?: () => void;
}

export const DesktopStatusBar: React.FC<DesktopStatusBarProps> = ({
  cashierName,
  shiftNumber,
  currency,
  totalProductsCount,
  totalInvoicesCount,
  onOpenLogin,
}) => {
  const [currentTime, setCurrentTime] = useState<string>('00:00:00');
  const [capsLockActive, setCapsLockActive] = useState<boolean>(false);
  const [numLockActive, setNumLockActive] = useState<boolean>(true);

  // Update real-time clock for the StatusStrip panel
  useEffect(() => {
    const updateTime = () => {
      const now = new Date();
      setCurrentTime(now.toLocaleTimeString('ar-YE', { hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: true }));
    };
    updateTime();
    const interval = setInterval(updateTime, 1000);
    return () => clearInterval(interval);
  }, []);

  // Detect Caps Lock key state
  useEffect(() => {
    const handleKeyChange = (e: KeyboardEvent) => {
      if (typeof e.getModifierState === 'function') {
        setCapsLockActive(e.getModifierState('CapsLock'));
        setNumLockActive(e.getModifierState('NumLock'));
      }
    };
    window.addEventListener('keydown', handleKeyChange);
    window.addEventListener('keyup', handleKeyChange);
    return () => {
      window.removeEventListener('keydown', handleKeyChange);
      window.removeEventListener('keyup', handleKeyChange);
    };
  }, []);

  return (
    <footer 
      id="winforms-statusstrip"
      className="bg-slate-100 text-slate-700 border-t border-slate-300 px-3 py-1 text-[11px] flex flex-wrap items-center justify-between select-none font-sans shadow-xs"
    >
      {/* Right Side: Primary Status Panels */}
      <div className="flex items-center gap-2 overflow-x-auto">
        {/* StatusStrip Panel 1: Ready Status */}
        <div className="flex items-center gap-1.5 bg-white px-2.5 py-0.5 rounded-md border border-slate-200 text-emerald-700 font-semibold shadow-xs">
          <span className="w-2 h-2 rounded-full bg-emerald-500 animate-pulse"></span>
          <span>النظام متصل وجاهز</span>
        </div>

        {/* StatusStrip Panel 2: User & Role */}
        <button 
          onClick={onOpenLogin}
          className="flex items-center gap-1.5 bg-white hover:bg-slate-50 px-2.5 py-0.5 rounded-md border border-slate-200 text-slate-700 transition cursor-pointer shadow-xs"
          title="انقر لتبديل المستخدم أو تسجيل الدخول"
        >
          <User className="w-3.5 h-3.5 text-sky-600" />
          <span>المستخدم: <strong className="text-slate-900 font-bold">{cashierName}</strong></span>
        </button>

        {/* StatusStrip Panel 3: Shift Number */}
        <div className="flex items-center gap-1 bg-white px-2.5 py-0.5 rounded-md border border-slate-200 text-slate-700 shadow-xs">
          <span>الوردية: <strong className="text-amber-700 font-bold">#{shiftNumber}</strong></span>
        </div>

        {/* StatusStrip Panel 4: Quick Shortcuts reminder */}
        <div className="hidden md:flex items-center gap-1 text-[11px] text-slate-500 mr-2">
          <span>[Alt] القوائم | [F1..F10] الشاشات السريعة</span>
        </div>
      </div>

      {/* Left Side: Secondary System Status Panels */}
      <div className="flex items-center gap-2 text-slate-600">
        {/* Key Lock Indicators (CAPS / NUM) */}
        <div className="flex items-center gap-1 font-mono text-[10px]">
          <span className={`px-1.5 py-0.5 rounded border ${capsLockActive ? 'bg-amber-600 text-white border-amber-500 font-bold' : 'bg-slate-200/80 text-slate-400 border-slate-300'}`}>
            CAPS
          </span>
          <span className={`px-1.5 py-0.5 rounded border ${numLockActive ? 'bg-sky-600 text-white border-sky-500 font-bold' : 'bg-slate-200/80 text-slate-400 border-slate-300'}`}>
            NUM
          </span>
        </div>

        {/* Database Connection Panel */}
        <div className="flex items-center gap-1.5 bg-white px-2.5 py-0.5 rounded-md border border-slate-200 text-slate-700 shadow-xs">
          <Database className="w-3.5 h-3.5 text-sky-600" />
          <span className="font-medium">قاعدة البيانات المحلية النشطة</span>
        </div>

        {/* Currency Panel */}
        <div className="flex items-center gap-1 bg-white px-2.5 py-0.5 rounded-md border border-slate-200 text-amber-800 font-bold shadow-xs">
          <span>{currency}</span>
        </div>

        {/* Real-time Clock Panel */}
        <div className="flex items-center gap-1.5 bg-white px-2.5 py-0.5 rounded-md border border-slate-200 text-sky-800 font-mono font-bold shadow-xs">
          <Clock className="w-3.5 h-3.5 text-sky-600" />
          <span>{currentTime}</span>
        </div>
      </div>
    </footer>
  );
};
