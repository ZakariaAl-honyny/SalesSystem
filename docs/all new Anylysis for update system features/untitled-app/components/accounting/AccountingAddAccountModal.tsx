'use client';

import React, { useState, useMemo } from 'react';
import { AccountNode } from '@/types/pos';
import {
  Folder,
  X,
  Sparkles,
  Briefcase,
  ShieldCheck,
  AlertCircle,
  Check,
  Wallet,
  Landmark,
  Users,
  Truck,
  Receipt,
  Building2,
  TrendingUp,
  Info
} from 'lucide-react';

interface AccountingAddAccountModalProps {
  isOpen: boolean;
  onClose: () => void;
  handleSaveNewAccount: (e: React.FormEvent) => void;
  addAccountParentCode: string;
  allAccountsFlat: AccountNode[];
  accountForm: any;
  setAccountForm: React.Dispatch<React.SetStateAction<any>>;
}

interface QuickArchetype {
  id: string;
  title: string;
  desc: string;
  icon: React.ComponentType<{ className?: string }>;
  suggestedParentCode: string;
  nature: 'DEBIT' | 'CREDIT';
  sampleNames: string[];
  explanation: string;
}

const QUICK_ARCHETYPES: QuickArchetype[] = [
  {
    id: 'CASH',
    title: 'صندوق نقدي / كاشير',
    desc: 'نقود وخزائن داخل المحل',
    icon: Wallet,
    suggestedParentCode: '111',
    nature: 'DEBIT',
    sampleNames: ['درج الكاشير 2', 'صندوق المبيعات المسائي', 'خزينة الفرع'],
    explanation: 'طبيعة مدينة: يزداد الحساب بالقبض وينقص بالصرف.',
  },
  {
    id: 'BANK',
    title: 'حساب بنكي / مدى',
    desc: 'حساب بنكي أو نقاط بيع',
    icon: Landmark,
    suggestedParentCode: '112',
    nature: 'DEBIT',
    sampleNames: ['بنك الكريمي جاري', 'بنك التضامن', 'شبكة مدى POS'],
    explanation: 'طبيعة مدينة: يزداد بالإيداعات وينقص بالتحويلات.',
  },
  {
    id: 'CUSTOMER',
    title: 'عميل آجل (زبون)',
    desc: 'ذمم مدينة للزبائن',
    icon: Users,
    suggestedParentCode: '113',
    nature: 'DEBIT',
    sampleNames: ['العميل محمد أحمد', 'شركة الأمل التجارية'],
    explanation: 'طبيعة مدينة: مدين بما اشترى، ويقل بالسداد.',
  },
  {
    id: 'SUPPLIER',
    title: 'مورد بضاعة (دائن)',
    desc: 'موردين وشركات توريد',
    icon: Truck,
    suggestedParentCode: '211',
    nature: 'CREDIT',
    sampleNames: ['شركة نادك للألبان', 'مؤسسة البركة للاستيراد'],
    explanation: 'طبيعة دائنة: دائن بما ورد لنا، ويقل بالسداد له.',
  },
  {
    id: 'EXPENSE',
    title: 'مصروف تشغيلي',
    desc: 'إيجار، كهرباء، نظافة...',
    icon: Receipt,
    suggestedParentCode: '41',
    nature: 'DEBIT',
    sampleNames: ['فاتورة الكهرباء والمولد', 'إيجار المحل الشهري', 'ضيافة وبوفيه'],
    explanation: 'طبيعة مدينة: يسجل دائماً مدين ويخصم من أرباح النشاط.',
  },
  {
    id: 'ASSET_FIXED',
    title: 'أصل ثابت ومعدات',
    desc: 'سيارة، أجهزة، ديكور...',
    icon: Building2,
    suggestedParentCode: '12',
    nature: 'DEBIT',
    sampleNames: ['سيارة التوزيع رقم 1', 'ثلاجة عرض مركزية'],
    explanation: 'طبيعة مدينة: ممتلكات للمنشأة تستمر لسنوات.',
  },
];

export const AccountingAddAccountModal: React.FC<AccountingAddAccountModalProps> = ({
  isOpen,
  onClose,
  handleSaveNewAccount,
  addAccountParentCode,
  allAccountsFlat,
  accountForm,
  setAccountForm,
}) => {
  const [entryMode, setEntryMode] = useState<'SIMPLE' | 'ADVANCED'>('SIMPLE');

  const selectedParentNode = useMemo(() => {
    return allAccountsFlat.find((a) => a.code === addAccountParentCode);
  }, [allAccountsFlat, addAccountParentCode]);

  // Validation
  const errors = useMemo(() => {
    const list: string[] = [];
    if (!accountForm.name?.trim()) {
      list.push('يرجى إدخال اسم الحساب');
    }
    if (!accountForm.code?.trim()) {
      list.push('رقم الحساب مطلوب');
    } else {
      if (allAccountsFlat.some((a) => a.code === accountForm.code.trim())) {
        list.push(`رقم الحساب [${accountForm.code.trim()}] مستخدم بالفعل`);
      }
    }
    return list;
  }, [accountForm.name, accountForm.code, allAccountsFlat]);

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-slate-950/60 backdrop-blur-xs flex items-center justify-center z-50 p-2 sm:p-4 select-none" dir="rtl">
      <div className="bg-white rounded-2xl shadow-2xl border border-slate-300 w-full max-w-xl max-h-[92vh] flex flex-col overflow-hidden animate-in fade-in zoom-in-95 duration-150 text-right">
        {/* Header */}
        <div className="bg-white border-b border-slate-200 px-5 py-3.5 flex items-center justify-between shrink-0 text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-amber-50 border border-amber-200 flex items-center justify-center text-amber-700 shadow-xs">
              <Folder className="w-5 h-5 text-amber-600" />
            </div>
            <div>
              <h3 className="font-bold text-sm sm:text-base flex items-center gap-2 text-slate-900">
                <span>إضافة حساب جديد في شجرة الحسابات</span>
                <span className="text-[10px] bg-amber-100 text-amber-800 border border-amber-200 font-extrabold px-2 py-0.5 rounded-full">
                  المساعد الذكي
                </span>
              </h3>
              <p className="text-[11px] text-slate-500">
                تصنيف محاسبي سليم وضبط تلقائي للأب والأرقام المتسلسلة
              </p>
            </div>
          </div>
          <button
            onClick={onClose}
            className="text-slate-400 hover:text-slate-700 p-1.5 rounded-lg hover:bg-slate-100 transition cursor-pointer"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* Mode Switcher */}
        <div className="bg-slate-100 px-4 py-2 border-b border-slate-200 flex items-center justify-between gap-2 shrink-0">
          <div className="flex items-center gap-1 bg-white p-1 rounded-xl border border-slate-300 shadow-2xs">
            <button
              type="button"
              onClick={() => setEntryMode('SIMPLE')}
              className={`px-3 py-1 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
                entryMode === 'SIMPLE'
                  ? 'bg-sky-700 text-white shadow-xs'
                  : 'text-slate-600 hover:bg-slate-100'
              }`}
            >
              <Sparkles className="w-3.5 h-3.5 text-amber-300" />
              <span>الوضع المبسط (لغير المتخصصين)</span>
            </button>
            <button
              type="button"
              onClick={() => setEntryMode('ADVANCED')}
              className={`px-3 py-1 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
                entryMode === 'ADVANCED'
                  ? 'bg-sky-700 text-white shadow-xs'
                  : 'text-slate-600 hover:bg-slate-100'
              }`}
            >
              <Briefcase className="w-3.5 h-3.5" />
              <span>الوضع المتقدم (للمحاسبين)</span>
            </button>
          </div>

          <div className="hidden sm:flex items-center gap-1 text-[11px] text-slate-500 font-semibold">
            <ShieldCheck className="w-3.5 h-3.5 text-emerald-600" />
            <span>نظام الحسابات القياسي</span>
          </div>
        </div>

        {/* Form Body */}
        <form onSubmit={handleSaveNewAccount} className="p-4 sm:p-5 flex-1 overflow-y-auto space-y-4 text-xs">
          {/* Simple Mode: Quick Archetype Cards */}
          {entryMode === 'SIMPLE' && (
            <div className="space-y-2">
              <label className="block font-bold text-slate-800 text-xs">
                اختر نوع الحساب المقترح للضبط السريع:
              </label>
              <div className="grid grid-cols-2 sm:grid-cols-3 gap-2">
                {QUICK_ARCHETYPES.map((arch) => {
                  const Icon = arch.icon;
                  return (
                    <button
                      key={arch.id}
                      type="button"
                      onClick={() => {
                        setAccountForm((prev: any) => ({
                          ...prev,
                          nature: arch.nature,
                        }));
                      }}
                      className="p-2 rounded-xl border border-slate-200 bg-white hover:border-sky-500 hover:bg-sky-50/40 text-right transition cursor-pointer flex flex-col gap-1"
                    >
                      <div className="flex items-center gap-1.5">
                        <Icon className="w-3.5 h-3.5 text-sky-700" />
                        <span className="font-bold text-slate-900 text-xs">{arch.title}</span>
                      </div>
                      <span className="text-[10px] text-slate-500 line-clamp-1">{arch.desc}</span>
                    </button>
                  );
                })}
              </div>
            </div>
          )}

          {/* Parent account badge */}
          <div className="p-3 bg-sky-50 rounded-xl border border-sky-200 text-sky-950 flex items-center justify-between">
            <div>
              <span className="text-[11px] text-slate-500 block">الحساب الأب المعتمد:</span>
              <div className="flex items-center gap-2 mt-0.5">
                <span className="font-mono font-bold text-xs bg-white px-2 py-0.5 rounded border border-sky-300 text-sky-800">
                  [{addAccountParentCode}]
                </span>
                <span className="font-bold text-xs text-slate-900">
                  {selectedParentNode?.name || 'الحساب الرئيسي'}
                </span>
              </div>
            </div>
            <span className="text-[10px] font-bold bg-white text-slate-600 px-2 py-1 rounded border border-slate-200">
              مستوى {selectedParentNode?.level ? selectedParentNode.level + 1 : 4}
            </span>
          </div>

          {/* Code & Nature */}
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
            <div>
              <label className="font-bold text-slate-700 block mb-1">
                رقم / كود الحساب المقترح (أرقام صحيحة):
              </label>
              <input
                type="text"
                value={accountForm.code}
                onChange={(e) => setAccountForm({ ...accountForm, code: e.target.value })}
                className="w-full border border-slate-300 rounded-xl p-2 font-mono font-bold text-slate-900 focus:ring-2 focus:ring-sky-600 outline-hidden bg-slate-50 focus:bg-white"
                required
              />
            </div>

            <div>
              <label className="font-bold text-slate-700 block mb-1">
                طبيعة الحساب:
              </label>
              <select
                value={accountForm.nature}
                onChange={(e) => setAccountForm({ ...accountForm, nature: e.target.value as any })}
                className="w-full border border-slate-300 rounded-xl p-2 font-bold text-slate-800 bg-white focus:ring-2 focus:ring-sky-600 outline-hidden"
              >
                <option value="DEBIT">مدين (Debit) - يزيد بالمقبوضات</option>
                <option value="CREDIT">دائن (Credit) - يزيد بالالتزامات</option>
              </select>
            </div>
          </div>

          {/* Account Name Arabic */}
          <div>
            <label className="font-bold text-slate-800 block mb-1">
              اسم الحساب (عربي) <span className="text-red-500">*</span>:
            </label>
            <input
              type="text"
              value={accountForm.name}
              onChange={(e) => setAccountForm({ ...accountForm, name: e.target.value })}
              placeholder="مثال: صندوق فرع المعلا، مصروفات قرطاسية ومطبوعات..."
              className="w-full border border-slate-300 rounded-xl p-2.5 text-slate-900 focus:ring-2 focus:ring-sky-600 outline-hidden font-medium"
              required
            />
          </div>

          {/* Account Name English */}
          <div>
            <label className="font-bold text-slate-700 block mb-1">
              اسم الحساب (إنجليزي - اختياري):
            </label>
            <input
              type="text"
              value={accountForm.nameEn}
              onChange={(e) => setAccountForm({ ...accountForm, nameEn: e.target.value })}
              placeholder="e.g. Branch Cashbox 2"
              className="w-full border border-slate-300 rounded-xl p-2 text-slate-900 focus:ring-2 focus:ring-sky-600 outline-hidden"
            />
          </div>

          {/* Currency and options */}
          <div className="bg-slate-50 border border-slate-200 rounded-xl p-3 flex flex-wrap items-center justify-between gap-3">
            <div className="flex items-center gap-2">
              <span className="text-slate-500 font-bold">العملة:</span>
              <span className="font-bold text-slate-800 bg-white border border-slate-300 px-2 py-0.5 rounded text-xs">
                ريال يمني (YER)
              </span>
            </div>

            <div className="flex items-center gap-2">
              <input
                type="checkbox"
                id="chkAllowCC"
                checked={accountForm.allowCostCenter}
                onChange={(e) => setAccountForm({ ...accountForm, allowCostCenter: e.target.checked })}
                className="w-4 h-4 rounded text-sky-600 cursor-pointer"
              />
              <label htmlFor="chkAllowCC" className="font-bold text-slate-700 cursor-pointer text-xs">
                يقبل الربط بمراكز التكلفة
              </label>
            </div>
          </div>

          {/* Error / Validation Feedback */}
          {errors.length > 0 && (
            <div className="p-2.5 bg-rose-50 border border-rose-200 rounded-xl text-rose-800 text-[11px] space-y-1 font-bold">
              {errors.map((err, i) => (
                <div key={i} className="flex items-center gap-1.5">
                  <AlertCircle className="w-3.5 h-3.5 text-rose-600 shrink-0" />
                  <span>{err}</span>
                </div>
              ))}
            </div>
          )}
        </form>

        {/* Footer */}
        <div className="bg-slate-100 px-5 py-3 border-t border-slate-200 flex items-center justify-between shrink-0">
          <span className="text-[11px] text-slate-500">
            {entryMode === 'SIMPLE' ? 'الوضع الذكي السلس' : 'الوضع المحاسبي المتقدم'}
          </span>
          <div className="flex items-center gap-2">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 rounded-xl text-slate-600 hover:bg-slate-200 font-bold transition cursor-pointer text-xs"
            >
              إلغاء
            </button>
            <button
              type="button"
              onClick={handleSaveNewAccount}
              disabled={errors.length > 0}
              className={`px-5 py-2 rounded-xl font-bold transition flex items-center gap-1.5 shadow-sm text-xs cursor-pointer ${
                errors.length === 0
                  ? 'bg-sky-700 hover:bg-sky-800 text-white active:scale-95'
                  : 'bg-slate-300 text-slate-500 cursor-not-allowed'
              }`}
            >
              <Check className="w-4 h-4" />
              <span>حفظ الحساب في الشجرة</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
