'use client';

import React, { useState, useEffect } from 'react';
import { 
  Monitor, 
  RefreshCw, 
  Layers, 
  ShieldCheck, 
  Clock, 
  Wifi, 
  Lock, 
  UserCheck, 
  KeyRound, 
  LogOut, 
  ChevronDown,
  Globe,
  Smartphone,
  Sparkles
} from 'lucide-react';
import { SystemUser } from '@/types/pos';

interface DesktopTitleBarProps {
  shiftNumber: number;
  cashierName: string;
  activeTabTitle: string;
  currentUser?: SystemUser | null;
  currentPlatform?: 'DESKTOP' | 'WEB' | 'MOBILE';
  onChangePlatform?: (platform: 'DESKTOP' | 'WEB' | 'MOBILE') => void;
  onOpenPlatformRoadmap?: () => void;
  onOpenArchitecture: () => void;
  onOpenCalculator: () => void;
  onOpenLogin?: () => void;
  onLockScreen?: () => void;
  onChangePassword?: () => void;
}

export const DesktopTitleBar: React.FC<DesktopTitleBarProps> = ({
  shiftNumber,
  cashierName,
  activeTabTitle,
  currentUser,
  currentPlatform = 'DESKTOP',
  onChangePlatform,
  onOpenPlatformRoadmap,
  onOpenArchitecture,
  onOpenCalculator,
  onOpenLogin,
  onLockScreen,
  onChangePassword,
}) => {
  const [timeString, setTimeString] = useState<string>('');
  const [showUserDropdown, setShowUserDropdown] = useState<boolean>(false);

  useEffect(() => {
    const update = () => {
      const now = new Date();
      setTimeString(
        now.toLocaleTimeString('ar-YE', {
          hour: '2-digit',
          minute: '2-digit',
          second: '2-digit',
          hour12: true,
        })
      );
    };
    update();
    const timer = setInterval(update, 1000);
    return () => clearInterval(timer);
  }, []);

  return (
    <header className="bg-white text-slate-800 flex items-center justify-between px-3.5 py-2 border-b border-slate-200 select-none text-xs relative shadow-xs">
      {/* Right Side in RTL: System Logo & Window Title */}
      <div className="flex items-center gap-2.5">
        <div className="bg-sky-700 text-white p-1.5 rounded-lg font-bold flex items-center justify-center shadow-xs">
          <Monitor className="w-3.5 h-3.5" />
        </div>
        <div className="flex items-center gap-2">
          <span className="font-bold text-slate-900 tracking-wide text-sm">نظام المحاسب الذكي لإدارة الأعمال</span>
          <span className="text-slate-300">|</span>
          <span className="text-emerald-800 font-bold bg-emerald-50 px-2.5 py-0.5 rounded-full border border-emerald-200 text-[11px] flex items-center gap-1">
            <span className="w-1.5 h-1.5 rounded-full bg-emerald-600"></span>
            {activeTabTitle}
          </span>
          <span className="text-slate-500 font-medium text-[11px] hidden sm:inline bg-slate-50 px-2 py-0.5 rounded-md border border-slate-200">
            {activeTabTitle.includes('المخزون')
              ? 'إدارة المخزون والتوريدات الذكية'
              : activeTabTitle.includes('المشتريات')
              ? 'متابعة المشتريات والموردين'
              : activeTabTitle.includes('العملاء')
              ? 'إدارة حسابات وذمم العملاء'
              : activeTabTitle.includes('الحسابات') || activeTabTitle.includes('القيود')
              ? 'دفتر الأستاذ والتقارير المالية'
              : activeTabTitle.includes('التقارير')
              ? 'لوحة المؤشرات والرقابة المتقدمة'
              : activeTabTitle.includes('الأمان')
              ? 'منظومة الأمان والصلاحيات'
              : 'نقطة البيع وإصدار الفواتير الفورية'}
          </span>
        </div>
      </div>

      {/* Middle: Shift & Cashier Badge with User Control Dropdown */}
      <div className="hidden md:flex items-center gap-3 text-slate-600">
        {/* Interactive User Account Badge */}
        <div className="relative">
          <button
            onClick={() => setShowUserDropdown(!showUserDropdown)}
            className="flex items-center gap-1.5 bg-slate-50 hover:bg-slate-100 px-3 py-1 rounded-lg border border-slate-200 text-xs cursor-pointer transition shadow-2xs text-slate-700"
            title="إدارة جلسة المستخدم وتبديل الحساب وقفل الشاشة"
          >
            <ShieldCheck className="w-3.5 h-3.5 text-sky-600" />
            <span>المستخدم: <strong className="text-slate-900 font-bold">{currentUser?.fullName || cashierName}</strong></span>
            {currentUser && (
              <span className="text-[10px] bg-sky-50 text-sky-800 px-1.5 py-0.5 rounded border border-sky-200 font-mono font-bold">
                {currentUser.roleName}
              </span>
            )}
            <ChevronDown className="w-3 h-3 text-slate-400 ml-0.5" />
          </button>

          {/* User Account Context Menu */}
          {showUserDropdown && (
            <div 
              className="absolute right-0 mt-1 w-56 bg-white border border-slate-200 rounded-xl shadow-xl py-1.5 z-50 text-xs flex flex-col divide-y divide-slate-100 ring-1 ring-black/5"
              onMouseLeave={() => setShowUserDropdown(false)}
            >
              <div className="px-3 py-2 text-[11px] text-slate-500">
                <span className="block font-bold text-slate-900">{currentUser?.fullName || cashierName}</span>
                <span className="block text-sky-700 font-mono">@{currentUser?.userName || 'cashier1'}</span>
                <span className="block text-slate-400 text-[10px]">{currentUser?.branchName || 'المركز الرئيسي'}</span>
              </div>

              <div className="py-1">
                {onOpenLogin && (
                  <button
                    onClick={() => {
                      setShowUserDropdown(false);
                      onOpenLogin();
                    }}
                    className="w-full text-right px-3 py-1.5 hover:bg-slate-50 text-slate-700 flex items-center gap-2 cursor-pointer"
                  >
                    <UserCheck className="w-3.5 h-3.5 text-sky-600" />
                    <span>تبديل المستخدم [Ctrl+L]</span>
                  </button>
                )}

                {onLockScreen && (
                  <button
                    onClick={() => {
                      setShowUserDropdown(false);
                      onLockScreen();
                    }}
                    className="w-full text-right px-3 py-1.5 hover:bg-slate-50 text-amber-800 flex items-center gap-2 cursor-pointer"
                  >
                    <Lock className="w-3.5 h-3.5 text-amber-600" />
                    <span>قفل الشاشة مؤقتاً [Ctrl+Alt+L]</span>
                  </button>
                )}

                {onChangePassword && (
                  <button
                    onClick={() => {
                      setShowUserDropdown(false);
                      onChangePassword();
                    }}
                    className="w-full text-right px-3 py-1.5 hover:bg-slate-50 text-slate-700 flex items-center gap-2 cursor-pointer"
                  >
                    <KeyRound className="w-3.5 h-3.5 text-purple-600" />
                    <span>تغيير كلمة المرور الخاصة بي</span>
                  </button>
                )}
              </div>

              {onOpenLogin && (
                <div className="py-1">
                  <button
                    onClick={() => {
                      setShowUserDropdown(false);
                      onOpenLogin();
                    }}
                    className="w-full text-right px-3 py-1.5 hover:bg-rose-50 text-rose-700 flex items-center gap-2 cursor-pointer"
                  >
                    <LogOut className="w-3.5 h-3.5 text-rose-600" />
                    <span>تسجيل الخروج وإنهاء الوردية</span>
                  </button>
                </div>
              )}
            </div>
          )}
        </div>

        <div className="flex items-center gap-1.5 bg-slate-50 px-2.5 py-1 rounded-lg border border-slate-200">
          <span>الوردية: <strong className="text-amber-800 font-mono font-bold">#{shiftNumber}</strong></span>
        </div>

        <div className="flex items-center gap-1.5 text-slate-500 bg-slate-50 px-2 py-1 rounded-lg border border-slate-200">
          <Clock className="w-3.5 h-3.5 text-sky-600" />
          <span className="font-mono font-bold text-slate-800">{timeString}</span>
        </div>
      </div>

      {/* Left Side: System Tools & Window Mock Controls */}
      <div className="flex items-center gap-2">
        {/* 3-Platform Viewport Switcher */}
        {onChangePlatform && (
          <div className="flex items-center bg-slate-100 p-0.5 rounded-lg border border-slate-200">
            <button
              type="button"
              onClick={() => onChangePlatform('DESKTOP')}
              className={`px-2.5 py-1 rounded-md text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                currentPlatform === 'DESKTOP'
                  ? 'bg-white text-sky-800 shadow-2xs border border-slate-200'
                  : 'text-slate-600 hover:text-slate-900'
              }`}
              title="واجهة تطبيق سطح المكتب ونقاط البيع السريعة"
            >
              <Monitor className="w-3.5 h-3.5 text-sky-700" />
              <span className="hidden sm:inline">سطح المكتب</span>
            </button>

            <button
              type="button"
              onClick={() => onChangePlatform('WEB')}
              className={`px-2.5 py-1 rounded-md text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                currentPlatform === 'WEB'
                  ? 'bg-white text-sky-800 shadow-2xs border border-slate-200'
                  : 'text-slate-600 hover:text-slate-900'
              }`}
              title="واجهة منصة الويب السحابية وإدارة الفروع"
            >
              <Globe className="w-3.5 h-3.5 text-indigo-600" />
              <span className="hidden sm:inline">منصة الويب</span>
            </button>

            <button
              type="button"
              onClick={() => onChangePlatform('MOBILE')}
              className={`px-2.5 py-1 rounded-md text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                currentPlatform === 'MOBILE'
                  ? 'bg-white text-sky-800 shadow-2xs border border-slate-200'
                  : 'text-slate-600 hover:text-slate-900'
              }`}
              title="واجهة تطبيق الهاتف المحمول وأجهزة الجرد الكفية"
            >
              <Smartphone className="w-3.5 h-3.5 text-emerald-600" />
              <span className="hidden sm:inline">الموبايل / كفي</span>
            </button>
          </div>
        )}

        {onOpenPlatformRoadmap && (
          <button
            onClick={onOpenPlatformRoadmap}
            className="bg-amber-50 hover:bg-amber-100 text-amber-900 border border-amber-300 font-bold px-2.5 py-1 rounded-lg text-xs flex items-center gap-1 transition shadow-2xs cursor-pointer"
            title="مقارنة تجربة المستخدم والمعمارية عبر المنصات الثلاث"
          >
            <Sparkles className="w-3.5 h-3.5 text-amber-600" />
            <span className="hidden md:inline">دليل المنصات 3</span>
          </button>
        )}

        {onLockScreen && (
          <button
            onClick={onLockScreen}
            className="bg-slate-50 hover:bg-amber-50 hover:border-amber-300 text-amber-800 font-medium px-2.5 py-1 rounded-lg text-xs flex items-center gap-1 border border-slate-200 transition cursor-pointer"
            title="قفل الشاشة مؤقتاً [Ctrl+Alt+L]"
          >
            <Lock className="w-3.5 h-3.5 text-amber-600" />
            <span className="hidden sm:inline font-bold">قفل [Ctrl+Alt+L]</span>
          </button>
        )}

        <button
          id="btn-nav-csharp-blueprint"
          onClick={onOpenArchitecture}
          className="bg-indigo-600 hover:bg-indigo-700 text-white font-bold px-3 py-1 rounded-lg text-xs flex items-center gap-1.5 transition-all shadow-xs cursor-pointer"
          title="عرض الدليل الهندسي للمنظومة"
        >
          <Layers className="w-3.5 h-3.5 text-indigo-100" />
          <span>الدليل الهندسي [F10]</span>
        </button>

        <button
          id="btn-quick-calc"
          onClick={onOpenCalculator}
          className="bg-slate-50 hover:bg-slate-100 text-slate-700 px-2.5 py-1 rounded-lg transition-colors flex items-center gap-1 cursor-pointer border border-slate-200 font-bold"
          title="الآلة الحاسبة السريعة [F9]"
        >
          <span className="font-mono">123</span>
          <span className="hidden sm:inline">حاسبة [F9]</span>
        </button>

        {/* Window action buttons simulating desktop WinForms */}
        <div className="flex items-center gap-1 ml-1 border-r border-slate-200 pr-2">
          <button 
            className="w-5 h-5 rounded bg-slate-100 hover:bg-slate-200 flex items-center justify-center text-slate-600 text-xs font-bold cursor-pointer"
            title="تصغير النافذة"
          >
            _
          </button>
          <button 
            className="w-5 h-5 rounded bg-slate-100 hover:bg-slate-200 flex items-center justify-center text-slate-600 text-xs font-bold cursor-pointer"
            title="تكبير / استعادة"
          >
            □
          </button>
          <button 
            onClick={onOpenLogin}
            className="w-5 h-5 rounded bg-rose-100 hover:bg-rose-600 hover:text-white flex items-center justify-center text-rose-700 text-xs font-bold cursor-pointer transition-colors"
            title="تسجيل الخروج وإقفال الجلسة"
          >
            ✕
          </button>
        </div>
      </div>
    </header>
  );
};
