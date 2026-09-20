'use client';

import React, { useState } from 'react';
import { SystemUser } from '@/types/pos';
import { KeyRound, ShieldAlert, CheckCircle2, Eye, EyeOff, Lock, X } from 'lucide-react';

interface ChangePasswordModalProps {
  isOpen: boolean;
  user: SystemUser;
  isMandatory?: boolean;
  onClose: () => void;
  onPasswordChanged: (userId: string, newPassword: string) => void;
}

export const ChangePasswordModal: React.FC<ChangePasswordModalProps> = ({
  isOpen,
  user,
  isMandatory = true,
  onClose,
  onPasswordChanged,
}) => {
  const [currentPassword, setCurrentPassword] = useState<string>('');
  const [newPassword, setNewPassword] = useState<string>('');
  const [confirmPassword, setConfirmPassword] = useState<string>('');
  const [showPasswords, setShowPasswords] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>('');

  if (!isOpen) return null;

  // Password strength calculation
  const getPasswordStrength = (pass: string) => {
    if (!pass) return { text: 'غير محددة', score: 0, color: 'text-slate-400', bg: 'bg-slate-700' };
    if (pass.length < 6) return { text: 'قصيرة جداً (أقل من 6 خانات)', score: 1, color: 'text-rose-400', bg: 'bg-rose-600' };
    let score = 1;
    if (pass.length >= 8) score++;
    if (/[A-Z]/.test(pass) || /[0-9]/.test(pass)) score++;
    if (/[^A-Za-z0-9]/.test(pass)) score++;

    if (score <= 2) return { text: 'مقبولة', score: 2, color: 'text-amber-400', bg: 'bg-amber-500' };
    if (score === 3) return { text: 'جيدة', score: 3, color: 'text-sky-400', bg: 'bg-sky-500' };
    return { text: 'قوية وممتازة', score: 4, color: 'text-emerald-400', bg: 'bg-emerald-500' };
  };

  const strength = getPasswordStrength(newPassword);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMessage('');

    // Check old password
    const expectedOld = user.password || '1';
    if (currentPassword !== expectedOld) {
      setErrorMessage('كلمة المرور الحالية غير صحيحة.');
      return;
    }

    // Check new password length (minimum 4 chars)
    if (newPassword.length < 4) {
      setErrorMessage('كلمة المرور الجديدة يجب أن تكون مكونة من 4 أحرف على الأقل.');
      return;
    }

    // Check new != old
    if (newPassword === currentPassword) {
      setErrorMessage('يجب أن تكون كلمة المرور الجديدة مختلفة عن كلمة المرور الحالية.');
      return;
    }

    // Check confirmation match
    if (newPassword !== confirmPassword) {
      setErrorMessage('كلمة المرور وتأكيد كلمة المرور غير متطابقين.');
      return;
    }

    // Successfully updated!
    onPasswordChanged(user.id, newPassword);
  };

  return (
    <div className="fixed inset-0 bg-slate-900/60 backdrop-blur-xs flex items-center justify-center z-50 p-4 font-sans select-none" dir="rtl">
      <div className="bg-white text-slate-900 rounded-2xl shadow-2xl border border-slate-200 w-full max-w-md overflow-hidden animate-in fade-in zoom-in-95 duration-150 flex flex-col">
        {/* WinForms Window Title Bar */}
        <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-amber-50 border border-amber-200 flex items-center justify-center text-amber-700 shadow-xs">
              <KeyRound className="w-4 h-4 text-amber-700" />
            </div>
            <span className="font-bold text-xs sm:text-sm text-slate-900 tracking-wide">
              {isMandatory ? 'تغيير كلمة المرور الإلزامي (MustChangePassword)' : 'تعديل كلمة مرور الحساب'}
            </span>
          </div>
          {!isMandatory && (
            <button
              onClick={onClose}
              className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg text-sm cursor-pointer transition"
            >
              <X className="w-4 h-4" />
            </button>
          )}
        </div>

        {/* Informative Banner */}
        {isMandatory && (
          <div className="bg-amber-50 border-b border-amber-200 p-3 flex items-start gap-2.5 text-amber-900 text-xs">
            <ShieldAlert className="w-5 h-5 text-amber-600 shrink-0 mt-0.5" />
            <div>
              <p className="font-bold text-amber-950">سياسة الأمان للمستخدمين الجدد:</p>
              <p className="text-[11px] text-amber-800 leading-relaxed mt-0.5">
                حسابك مسجل بكلمة مرور مؤقتة، ويلزمك تعيين كلمة مرور جديدة قوية وخاصة بك للمتابعة والدخول للنظام.
              </p>
            </div>
          </div>
        )}

        <form onSubmit={handleSubmit} className="p-4 flex flex-col gap-3 text-xs">
          {/* User Details */}
          <div className="bg-slate-50 p-2.5 rounded-xl border border-slate-200 flex items-center justify-between">
            <div>
              <span className="text-slate-500 text-[11px] block font-semibold">المستخدم:</span>
              <span className="font-bold text-slate-900 text-xs">{user.fullName}</span>
            </div>
            <div className="text-left font-mono">
              <span className="text-[10px] bg-sky-50 text-sky-700 px-2 py-0.5 rounded-md border border-sky-200 font-bold">
                @{user.userName}
              </span>
            </div>
          </div>

          {errorMessage && (
            <div className="bg-rose-50 border border-rose-200 p-2.5 rounded-xl text-rose-800 text-xs flex items-center gap-2 font-semibold">
              <ShieldAlert className="w-4 h-4 text-rose-600 shrink-0" />
              <span>{errorMessage}</span>
            </div>
          )}

          {/* Current Password */}
          <div>
            <label className="block text-slate-700 font-bold mb-1">
              كلمة المرور الحالية (الافتراضية: 1):
            </label>
            <div className="relative">
              <input
                type={showPasswords ? 'text' : 'password'}
                value={currentPassword}
                onChange={(e) => setCurrentPassword(e.target.value)}
                placeholder="أدخل كلمة المرور الحالية..."
                required
                autoFocus
                className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-2 text-slate-900 font-mono text-xs font-bold focus:bg-white focus:outline-hidden focus:ring-1 focus:ring-sky-500 pr-8"
              />
              <Lock className="w-3.5 h-3.5 text-slate-500 absolute right-2.5 top-2.5 pointer-events-none" />
            </div>
          </div>

          {/* New Password */}
          <div>
            <div className="flex items-center justify-between mb-1">
              <label className="text-slate-700 font-bold">كلمة المرور الجديدة (6 خانات فأكثر):</label>
              <button
                type="button"
                onClick={() => setShowPasswords(!showPasswords)}
                className="text-sky-700 hover:text-sky-800 text-[11px] font-bold flex items-center gap-1 cursor-pointer"
              >
                {showPasswords ? <EyeOff className="w-3 h-3" /> : <Eye className="w-3 h-3 text-slate-500" />}
                <span>{showPasswords ? 'إخفاء' : 'إظهار'}</span>
              </button>
            </div>
            <div className="relative">
              <input
                type={showPasswords ? 'text' : 'password'}
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                placeholder="أدخل كلمة مرور جديدة قوية..."
                required
                className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-2 text-slate-900 font-mono text-xs font-bold focus:bg-white focus:outline-hidden focus:ring-1 focus:ring-sky-500 pr-8"
              />
              <KeyRound className="w-3.5 h-3.5 text-slate-500 absolute right-2.5 top-2.5 pointer-events-none" />
            </div>

            {/* Strength Bar */}
            {newPassword && (
              <div className="mt-1.5 space-y-1">
                <div className="flex items-center justify-between text-[10px]">
                  <span className="text-slate-500 font-semibold">مستوى الأمان:</span>
                  <span className={`font-bold ${strength.color}`}>{strength.text}</span>
                </div>
                <div className="w-full bg-slate-200 h-1.5 rounded-full overflow-hidden">
                  <div
                    className={`h-full transition-all duration-300 ${strength.bg}`}
                    style={{ width: `${(strength.score / 4) * 100}%` }}
                  />
                </div>
              </div>
            )}
          </div>

          {/* Confirm New Password */}
          <div>
            <label className="block text-slate-700 font-bold mb-1">تأكيد كلمة المرور الجديدة:</label>
            <div className="relative">
              <input
                type={showPasswords ? 'text' : 'password'}
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                placeholder="أعد كتابة كلمة المرور الجديدة..."
                required
                className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-2 text-slate-900 font-mono text-xs font-bold focus:bg-white focus:outline-hidden focus:ring-1 focus:ring-sky-500 pr-8"
              />
              <CheckCircle2 className="w-3.5 h-3.5 text-slate-500 absolute right-2.5 top-2.5 pointer-events-none" />
            </div>
          </div>

          {/* Buttons */}
          <div className="flex items-center justify-end gap-2 pt-3 border-t border-slate-200 mt-1">
            {!isMandatory && (
              <button
                type="button"
                onClick={onClose}
                className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200 px-3.5 py-1.5 rounded-lg font-bold cursor-pointer"
              >
                إلغاء
              </button>
            )}
            <button
              type="submit"
              className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-5 py-2 rounded-lg flex items-center gap-1.5 shadow-xs transition cursor-pointer text-xs"
            >
              <CheckCircle2 className="w-4 h-4" />
              <span>حفظ وتفعيل كلمة المرور والدخول</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
