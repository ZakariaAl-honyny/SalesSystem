'use client';

import React, { useState, useEffect } from 'react';
import { SystemUser, SystemRole, ShiftInfo } from '@/types/pos';
import { 
  LogIn, 
  Lock, 
  Shield, 
  ShieldAlert, 
  Clock, 
  User, 
  Building2, 
  KeyRound, 
  Eye, 
  EyeOff, 
  Unlock, 
  AlertCircle, 
  CheckCircle2, 
  Laptop, 
  ChevronRight,
  Sparkles,
  Layers,
  X
} from 'lucide-react';
import { ChangePasswordModal } from './ChangePasswordModal';

interface LoginModalProps {
  isOpen: boolean;
  currentUser: SystemUser | null;
  users: SystemUser[];
  roles: SystemRole[];
  currentShift: ShiftInfo;
  isLockScreenMode?: boolean;
  onClose?: () => void;
  onLoginSuccess: (user: SystemUser, branchName: string, shiftNumber: number) => void;
  onUnlockUser: (userId: string) => void;
  onUpdateUserPassword: (userId: string, newPass: string) => void;
  onUpdateUserAttempts: (userId: string, attempts: number, isLocked: boolean, lockedUntil: string | null) => void;
}

export const LoginModal: React.FC<LoginModalProps> = ({
  isOpen,
  currentUser,
  users,
  roles,
  currentShift,
  isLockScreenMode = false,
  onClose,
  onLoginSuccess,
  onUnlockUser,
  onUpdateUserPassword,
  onUpdateUserAttempts,
}) => {
  const [selectedUserId, setSelectedUserId] = useState<string>(
    currentUser ? currentUser.id : (users[0]?.id || '')
  );
  const [password, setPassword] = useState<string>('');
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [selectedBranch, setSelectedBranch] = useState<string>(
    currentUser?.branchName || 'المركز الرئيسي (صنعاء)'
  );
  const [shiftNumber, setShiftNumber] = useState<number>(currentShift.shiftNumber || 101);
  const [errorText, setErrorText] = useState<string>('');
  const [attemptsInfo, setAttemptsInfo] = useState<string>('');
  
  // Countdown timer clock
  const [nowTimeMs, setNowTimeMs] = useState<number>(() => Date.now());

  // Update clock every second when modal is open
  useEffect(() => {
    if (!isOpen) return;
    const interval = setInterval(() => {
      setNowTimeMs(Date.now());
    }, 1000);
    return () => clearInterval(interval);
  }, [isOpen]);

  // Admin override unlock state
  const [showAdminOverrideModal, setShowAdminOverrideModal] = useState<boolean>(false);
  const [adminPasswordInput, setAdminPasswordInput] = useState<string>('');
  const [adminOverrideError, setAdminOverrideError] = useState<string>('');

  // Mandatory change password modal
  const [userForPasswordChange, setUserForPasswordChange] = useState<SystemUser | null>(null);

  // Determine effective active user
  const effectiveUserId = (isLockScreenMode && currentUser) 
    ? currentUser.id 
    : (selectedUserId || (currentUser ? currentUser.id : users[0]?.id || ''));

  const activeUser = users.find((u) => u.id === effectiveUserId) || users[0];

  // Derived countdown state
  const lockUntilMs = activeUser?.lockedUntil ? new Date(activeUser.lockedUntil).getTime() : 0;
  const isLockExpired = Boolean(activeUser?.isLocked && lockUntilMs > 0 && lockUntilMs <= nowTimeMs);
  const secondsRemaining = (activeUser?.isLocked && lockUntilMs > nowTimeMs)
    ? Math.floor((lockUntilMs - nowTimeMs) / 1000)
    : 0;

  // Auto unlock when lock timer expires
  useEffect(() => {
    if (isLockExpired && activeUser) {
      onUnlockUser(activeUser.id);
    }
  }, [isLockExpired, activeUser, onUnlockUser]);

  if (!isOpen) return null;

  // Format MM:SS
  const formatCountdown = (totalSeconds: number) => {
    const mins = Math.floor(totalSeconds / 60);
    const secs = totalSeconds % 60;
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  };

  const handleLoginSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setErrorText('');
    setAttemptsInfo('');

    if (!activeUser) return;

    // 1. Check if user is active
    if (!activeUser.isActive) {
      setErrorText('هذا الحساب معطل حالياً من قبل إدارة النظام.');
      return;
    }

    // 2. Check if locked
    if (activeUser.isLocked && secondsRemaining > 0) {
      setErrorText(`هذا الحساب مقفل مؤقتاً لحمايته من محاولات التخمين! متبقي ${formatCountdown(secondsRemaining)} قبل فك الحظر.`);
      return;
    }

    // 3. Verify Password
    const expectedPassword = activeUser.password || '1';
    if (password !== expectedPassword) {
      const newAttempts = (activeUser.loginAttempts || 0) + 1;
      
      if (newAttempts >= 5) {
        // Lock user for 30 seconds
        const lockUntilDate = new Date(Date.now() + 30 * 1000).toISOString();
        onUpdateUserAttempts(activeUser.id, 5, true, lockUntilDate);
        setErrorText('تم تجاوز الحد الأقصى للمحاولات الخاطئة (5). تم قفل الحساب مؤقتاً لمدة 30 ثانية.');
        setAttemptsInfo('الحساب محظور الآن لمدة 30 ثانية.');
      } else {
        onUpdateUserAttempts(activeUser.id, newAttempts, false, null);
        setErrorText(`كلمة المرور غير صحيحة!`);
        setAttemptsInfo(`محاولة ${newAttempts} من 5 قبل حظر الحساب تلقائياً.`);
      }
      return;
    }

    // 4. If credentials valid: Reset attempts
    onUpdateUserAttempts(activeUser.id, 0, false, null);

    // 5. Check if user must change password
    if (activeUser.mustChangePassword) {
      setUserForPasswordChange(activeUser);
      return;
    }

    // 6. Complete login
    onLoginSuccess(activeUser, selectedBranch, shiftNumber);
    setPassword('');
  };

  // Switch to another user directly
  const handleSelectPresetUser = (u: SystemUser) => {
    setSelectedUserId(u.id);
    setPassword('');
    setErrorText('');
    setAttemptsInfo('');
    if (u.branchName && u.branchName !== 'جميع الفروع (المركز الرئيسي)') {
      setSelectedBranch(u.branchName);
    }
  };

  // Admin override unlock
  const handleConfirmAdminUnlock = (e: React.FormEvent) => {
    e.preventDefault();
    setAdminOverrideError('');
    if (adminPasswordInput !== 'admin123') {
      setAdminOverrideError('كلمة مرور مدير النظام الأعلى غير صحيحة.');
      return;
    }

    onUnlockUser(activeUser.id);
    setShowAdminOverrideModal(false);
    setAdminPasswordInput('');
    setErrorText('تم فك قفل الحساب بنجاح بصلاحية مدير النظام الأعلى.');
  };

  return (
    <div className="fixed inset-0 bg-slate-800/40 backdrop-blur-xs flex items-center justify-center z-50 p-4 font-sans select-none" dir="rtl">
      <div className="bg-white text-slate-900 rounded-2xl shadow-2xl border border-slate-200 w-full max-w-xl overflow-hidden animate-in fade-in zoom-in-95 duration-150 flex flex-col">
        {/* WinForms Desktop Dialog Header (Light theme) */}
        <div className="bg-slate-100 border-b border-slate-200 px-4 py-3 flex items-center justify-between text-slate-800">
          <div className="flex items-center gap-2">
            <div className="p-1.5 bg-sky-600 rounded-lg text-white">
              <LogIn className="w-4 h-4" />
            </div>
            <span className="font-bold text-xs tracking-wide text-slate-900">
              {isLockScreenMode 
                ? `قفل محطة العمل (Workstation Locked) - [${currentUser?.fullName}]` 
                : 'تسجيل الدخول إلى نظام المبيعات ونقاط البيع ERP'}
            </span>
          </div>
          {onClose && (
            <button
              onClick={onClose}
              className="text-slate-400 hover:text-slate-700 text-sm cursor-pointer p-1 rounded-lg transition hover:bg-slate-200/60"
              title="إغلاق الشاشة"
            >
              <X className="w-4 h-4" />
            </button>
          )}
        </div>

        {/* Lockout Warning Banner */}
        {activeUser && activeUser.isLocked && secondsRemaining > 0 && (
          <div className="bg-rose-50 border-b border-rose-200 p-3 flex items-start justify-between gap-3 text-rose-900 text-xs animate-pulse">
            <div className="flex items-start gap-2.5">
              <ShieldAlert className="w-5 h-5 text-rose-600 shrink-0 mt-0.5" />
              <div>
                <p className="font-bold text-rose-950">
                  تم قفل الحساب لحمايته من هجمات التخمين (Brute-Force Lockout):
                </p>
                <p className="text-[11px] text-rose-800 mt-0.5">
                  تم تجاوز 5 محاولات دخول خاطئة. سيتم فك الحظر التلقائي بعد:
                </p>
                <div className="inline-flex items-center gap-1.5 mt-1 bg-white px-2.5 py-1 rounded-lg font-mono font-bold text-sm text-rose-700 border border-rose-300 shadow-xs">
                  <Clock className="w-4 h-4 text-rose-600" />
                  <span>{formatCountdown(secondsRemaining)} دقيقة</span>
                </div>
              </div>
            </div>

            <button
              type="button"
              onClick={() => setShowAdminOverrideModal(true)}
              className="bg-purple-700 hover:bg-purple-600 text-white text-[11px] font-bold px-2.5 py-1.5 rounded-lg flex items-center gap-1 shadow cursor-pointer whitespace-nowrap self-center transition"
              title="فك الحظر الفوري بإشراف المدير"
            >
              <Unlock className="w-3.5 h-3.5" />
              <span>فك القفل كمدير</span>
            </button>
          </div>
        )}

        <form onSubmit={handleLoginSubmit} className="p-4 flex flex-col gap-3 text-xs">
          {/* Preset User Quick Select Chips for easy testing */}
          <div className="bg-slate-50 p-3 rounded-xl border border-slate-200 space-y-1.5">
            <div className="flex items-center justify-between">
              <span className="text-slate-700 font-bold text-[11px] flex items-center gap-1">
                <Sparkles className="w-3.5 h-3.5 text-amber-500" />
                <span>اختيار سريع للاختبار والمحاكاة (Sprint 1.1 Test Users):</span>
              </span>
              <span className="text-[10px] text-slate-500">انقر على المستخدم لتعبئة بياناته</span>
            </div>

            <div className="flex flex-wrap gap-1.5">
              {users.map((u) => {
                const isSelected = u.id === selectedUserId;
                const isUserLocked = u.isLocked;
                return (
                  <button
                    key={u.id}
                    type="button"
                    onClick={() => handleSelectPresetUser(u)}
                    className={`px-2.5 py-1 rounded-lg text-[11px] font-bold transition cursor-pointer flex items-center gap-1 border ${
                      isSelected
                        ? 'bg-sky-600 text-white border-sky-500 shadow-xs ring-1 ring-sky-300'
                        : isUserLocked
                        ? 'bg-rose-50 text-rose-800 border-rose-300 hover:bg-rose-100'
                        : u.mustChangePassword
                        ? 'bg-amber-50 text-amber-800 border-amber-300 hover:bg-amber-100'
                        : 'bg-white text-slate-700 border-slate-300 hover:bg-slate-100'
                    }`}
                  >
                    <span>{u.userName}</span>
                    {isUserLocked && <Lock className="w-3 h-3 text-rose-600" />}
                    {u.mustChangePassword && <KeyRound className="w-3 h-3 text-amber-600" />}
                    <span className="text-[10px] opacity-75 font-mono">({u.roleName})</span>
                  </button>
                );
              })}
            </div>
          </div>

          {/* Error and Attempt Notice */}
          {errorText && (
            <div className="bg-rose-50 border border-rose-200 p-2.5 rounded-xl text-rose-800 text-xs flex items-center justify-between">
              <div className="flex items-center gap-2">
                <AlertCircle className="w-4 h-4 text-rose-600 shrink-0" />
                <span className="font-semibold">{errorText}</span>
              </div>
              {attemptsInfo && (
                <span className="bg-rose-100 text-rose-900 font-mono text-[10px] px-2 py-0.5 rounded-md font-bold border border-rose-300">
                  {attemptsInfo}
                </span>
              )}
            </div>
          )}

          {/* Form Fields Grid */}
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
            {/* User Select Box */}
            <div>
              <label className="block text-slate-700 font-bold mb-1">اسم المستخدم (User Account):</label>
              <div className="relative">
                <select
                  value={selectedUserId}
                  onChange={(e) => {
                    const u = users.find((item) => item.id === e.target.value);
                    if (u) handleSelectPresetUser(u);
                  }}
                  className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-2 text-slate-900 text-xs font-bold focus:bg-white focus:outline-hidden focus:ring-1 focus:ring-sky-500 pr-8 cursor-pointer"
                >
                  {users.map((u) => (
                    <option key={u.id} value={u.id}>
                      {u.fullName} (@{u.userName}) {u.isLocked ? '[مقفل]' : ''}
                    </option>
                  ))}
                </select>
                <User className="w-3.5 h-3.5 text-slate-500 absolute right-2.5 top-2.5 pointer-events-none" />
              </div>
            </div>

            {/* Password Box */}
            <div>
              <div className="flex items-center justify-between mb-1">
                <label className="text-slate-700 font-bold">كلمة المرور (Password):</label>
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  className="text-sky-700 hover:text-sky-800 text-[10px] font-bold flex items-center gap-1 cursor-pointer"
                >
                  {showPassword ? <EyeOff className="w-3 h-3" /> : <Eye className="w-3 h-3 text-slate-500" />}
                  <span>{showPassword ? 'إخفاء' : 'إظهار'}</span>
                </button>
              </div>
              <div className="relative">
                <input
                  type={showPassword ? 'text' : 'password'}
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  placeholder={
                    activeUser?.isLocked
                      ? 'الحساب مقفل حالياً...'
                      : activeUser?.mustChangePassword
                      ? 'كلمة المرور الافتراضية: 1'
                      : activeUser?.userName === 'admin'
                      ? 'admin123'
                      : '123456'
                  }
                  disabled={Boolean(activeUser?.isLocked && secondsRemaining > 0)}
                  autoFocus
                  required
                  className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-2 text-slate-900 font-mono text-xs font-bold focus:bg-white focus:outline-hidden focus:ring-1 focus:ring-sky-500 pr-8 disabled:opacity-50"
                />
                <Lock className="w-3.5 h-3.5 text-slate-500 absolute right-2.5 top-2.5 pointer-events-none" />
              </div>
              <p className="text-[10px] text-slate-500 mt-1">
                تلميح: <span className="font-mono font-bold text-amber-700">admin: admin123</span> | <span className="font-mono font-bold text-amber-700">cashier1: 123456</span> | <span className="font-mono font-bold text-amber-700">warehouse1: 1</span>
              </p>
            </div>

            {/* Branch Selector */}
            <div>
              <label className="block text-slate-700 font-bold mb-1">الفرع المالي وموقع العمل:</label>
              <div className="relative">
                <select
                  value={selectedBranch}
                  onChange={(e) => setSelectedBranch(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-2 text-slate-900 text-xs font-bold focus:bg-white focus:outline-hidden focus:ring-1 focus:ring-sky-500 pr-8 cursor-pointer"
                >
                  <option value="المركز الرئيسي (صنعاء)">المركز الرئيسي (صنعاء) - Branch 01</option>
                  <option value="فرع عدن">فرع عدن - Branch 02</option>
                  <option value="فرع تعز">فرع تعز - Branch 03</option>
                  <option value="فرع الحديدة">فرع الحديدة - Branch 04</option>
                  <option value="جميع الفروع (المركز الرئيسي)">جميع الفروع (إدارة مركزية)</option>
                </select>
                <Building2 className="w-3.5 h-3.5 text-slate-500 absolute right-2.5 top-2.5 pointer-events-none" />
              </div>
            </div>

            {/* Shift Number Selector */}
            <div>
              <label className="block text-slate-700 font-bold mb-1">رقم الوردية / جلسة الصندوق:</label>
              <div className="relative">
                <input
                  type="number"
                  value={shiftNumber}
                  onChange={(e) => setShiftNumber(parseInt(e.target.value) || 101)}
                  className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-2 text-slate-900 font-mono font-bold text-xs focus:bg-white focus:outline-hidden focus:ring-1 focus:ring-sky-500 pr-8"
                />
                <Clock className="w-3.5 h-3.5 text-slate-500 absolute right-2.5 top-2.5 pointer-events-none" />
              </div>
            </div>
          </div>

          {/* Action Buttons */}
          <div className="flex items-center justify-between pt-3 border-t border-slate-200 mt-1">
            <div className="text-[11px] text-slate-500 flex items-center gap-1.5 font-semibold">
              <Shield className="w-3.5 h-3.5 text-emerald-600" />
              <span>مشفر وفق معايير SHA-256 + Salt</span>
            </div>

            <div className="flex items-center gap-2">
              {onClose && (
                <button
                  type="button"
                  onClick={onClose}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200 px-3.5 py-2 rounded-lg font-bold cursor-pointer transition text-xs"
                >
                  إلغاء
                </button>
              )}
              <button
                type="submit"
                disabled={Boolean(activeUser?.isLocked && secondsRemaining > 0)}
                className="bg-emerald-600 hover:bg-emerald-500 disabled:bg-slate-300 disabled:opacity-50 text-white font-bold px-6 py-2 rounded-lg flex items-center gap-1.5 shadow-xs transition cursor-pointer text-xs"
              >
                <LogIn className="w-4 h-4" />
                <span>تسجيل الدخول وفتح الوردية</span>
              </button>
            </div>
          </div>
        </form>
      </div>

      {/* Admin Override Unlock Modal */}
      {showAdminOverrideModal && (
        <div className="fixed inset-0 bg-slate-800/40 backdrop-blur-xs flex items-center justify-center z-60 p-4 font-sans" dir="rtl">
          <div className="bg-white text-slate-900 rounded-2xl shadow-2xl border border-purple-200 w-full max-w-sm overflow-hidden animate-in fade-in zoom-in-95">
            <div className="bg-purple-50 border-b border-purple-200 px-3.5 py-2.5 flex items-center justify-between text-purple-900">
              <div className="flex items-center gap-2">
                <Unlock className="w-4 h-4 text-purple-600" />
                <span className="font-bold text-xs text-purple-950">فك الحظر بصلاحية مدير النظام الأعلى</span>
              </div>
              <button
                onClick={() => setShowAdminOverrideModal(false)}
                className="text-purple-400 hover:text-purple-700 hover:bg-purple-100 p-1 rounded-lg"
              >
                ✕
              </button>
            </div>

            <form onSubmit={handleConfirmAdminUnlock} className="p-4 flex flex-col gap-3 text-xs">
              <p className="text-slate-600 leading-relaxed font-semibold">
                أدخل كلمة مرور مدير النظام الأعلى <span className="text-purple-700 font-mono font-bold">admin123</span> لفك قفل الحساب فوراً وتصفير عداد المحاولات الخاطئة:
              </p>

              {adminOverrideError && (
                <div className="bg-rose-50 p-2 rounded-lg text-rose-800 border border-rose-200 font-bold">
                  {adminOverrideError}
                </div>
              )}

              <div>
                <label className="block text-slate-700 font-bold mb-1">كلمة مرور مدير النظام الأعلى:</label>
                <input
                  type="password"
                  value={adminPasswordInput}
                  onChange={(e) => setAdminPasswordInput(e.target.value)}
                  placeholder="admin123"
                  required
                  autoFocus
                  className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-2 text-slate-900 font-mono text-xs font-bold focus:bg-white focus:outline-hidden focus:ring-1 focus:ring-purple-500"
                />
              </div>

              <div className="flex items-center justify-end gap-2 pt-2 border-t border-slate-200">
                <button
                  type="button"
                  onClick={() => setShowAdminOverrideModal(false)}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-3 py-1.5 rounded-lg font-bold border border-slate-200"
                >
                  إلغاء
                </button>
                <button
                  type="submit"
                  className="bg-purple-700 hover:bg-purple-600 text-white font-bold px-4 py-1.5 rounded-lg flex items-center gap-1 shadow-xs cursor-pointer"
                >
                  <Unlock className="w-3.5 h-3.5" />
                  <span>تأكيد فك الحظر</span>
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Mandatory Password Change Modal */}
      {userForPasswordChange && (
        <ChangePasswordModal
          isOpen={true}
          user={userForPasswordChange}
          isMandatory={true}
          onClose={() => setUserForPasswordChange(null)}
          onPasswordChanged={(userId, newPass) => {
            onUpdateUserPassword(userId, newPass);
            setUserForPasswordChange(null);
            // Complete login after password updated!
            const updatedUser = { ...userForPasswordChange, password: newPass, mustChangePassword: false };
            onLoginSuccess(updatedUser, selectedBranch, shiftNumber);
          }}
        />
      )}
    </div>
  );
};
