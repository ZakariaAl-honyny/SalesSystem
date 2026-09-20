'use client';

import React, { useState, useMemo, useEffect } from 'react';
import { AccountNode } from '@/types/pos';
import { generateNextChildAccountCode, getAllAccountsFlat } from '@/lib/mock/accounts';
import {
  Folder,
  FolderPlus,
  X,
  Check,
  Sparkles,
  HelpCircle,
  AlertCircle,
  Building2,
  Wallet,
  Landmark,
  Users,
  Truck,
  DollarSign,
  TrendingUp,
  Receipt,
  Briefcase,
  ShieldCheck,
  ArrowRight,
  Info,
  ChevronDown
} from 'lucide-react';

export interface SmartAccountCreationModalProps {
  isOpen: boolean;
  onClose: () => void;
  chartOfAccounts: AccountNode[];
  initialParentCode?: string;
  onSaveAccount: (newAccount: AccountNode) => void;
}

// Preset archetypes for non-specialists (Quick & Safe business setups)
interface AccountArchetype {
  id: string;
  title: string;
  subtitle: string;
  icon: React.ComponentType<{ className?: string }>;
  suggestedParentCode: string;
  fallbackParentCode: string;
  accountType: 'ASSET' | 'LIABILITY' | 'EQUITY' | 'REVENUE' | 'EXPENSE';
  nature: 'DEBIT' | 'CREDIT';
  financialStatement: 'BALANCE_SHEET' | 'INCOME_STATEMENT';
  exampleNames: string[];
  explanation: string;
  natureExplanation: string;
  colorClass: string;
  borderClass: string;
  badgeBg: string;
}

const ARCHETYPES: AccountArchetype[] = [
  {
    id: 'CASHBOX',
    title: 'صندوق نقدي / درج كاشير',
    subtitle: 'نقود ورقية وخزائن داخل المحل',
    icon: Wallet,
    suggestedParentCode: '111', // الصناديق والخزائن
    fallbackParentCode: '11',  // الأصول المتداولة
    accountType: 'ASSET',
    nature: 'DEBIT',
    financialStatement: 'BALANCE_SHEET',
    exampleNames: ['درج الكاشير 2', 'صندوق المبيعات المسائي', 'خزينة الفرع الثانوي'],
    explanation: 'يُستخدم لحفظ النقدية وتتبع حركة المقبوضات والمصروفات اليومية للكاشير.',
    natureExplanation: 'طبيعة مدينة: يزداد الحساب بالقبض وينقص بالصرف.',
    colorClass: 'text-emerald-700',
    borderClass: 'border-emerald-300 hover:border-emerald-500 hover:bg-emerald-50/50',
    badgeBg: 'bg-emerald-100 text-emerald-800',
  },
  {
    id: 'BANK',
    title: 'حساب بنكي / شبكة ومدى',
    subtitle: 'حساب جاري في بنك أو جهاز دفع شبكة',
    icon: Landmark,
    suggestedParentCode: '112', // البنوك والمحافظ
    fallbackParentCode: '11',
    accountType: 'ASSET',
    nature: 'DEBIT',
    financialStatement: 'BALANCE_SHEET',
    exampleNames: ['بنك الكريمي - جاري', 'بنك التضامن الإسلامي', 'حساب شبكة نقاط البيع POS'],
    explanation: 'يُستخدم للتحويلات البنكية والإيداعات ومتحصلات الشبكة وأجهزة الدفع.',
    natureExplanation: 'طبيعة مدينة: يزداد بالإيداعات البنكية وينقص بالتحويلات والمسحوبات.',
    colorClass: 'text-blue-700',
    borderClass: 'border-blue-300 hover:border-blue-500 hover:bg-blue-50/50',
    badgeBg: 'bg-blue-100 text-blue-800',
  },
  {
    id: 'E_WALLET',
    title: 'محفظة إلكترونية رقمية',
    subtitle: 'جيب، جوالي، كاش، ون كاش...',
    icon: Landmark,
    suggestedParentCode: '112', // البنوك والمحافظ
    fallbackParentCode: '11',
    accountType: 'ASSET',
    nature: 'DEBIT',
    financialStatement: 'BALANCE_SHEET',
    exampleNames: ['محفظة جيب كاش', 'محفظة جوالي التجارية', 'محفظة ون كاش'],
    explanation: 'يُستخدم لتسوية واستلام مدفوعات وتطبيقات المحافظ المالية الإلكترونية.',
    natureExplanation: 'طبيعة مدينة: يزداد بتحويلات الزبائن وينقص بتغذية الصناديق.',
    colorClass: 'text-indigo-700',
    borderClass: 'border-indigo-300 hover:border-indigo-500 hover:bg-indigo-50/50',
    badgeBg: 'bg-indigo-100 text-indigo-800',
  },
  {
    id: 'CUSTOMER',
    title: 'عميل تجاري / زبون آجل',
    subtitle: 'شخص أو شركة تشتري بالآجل (ذمم مدينة)',
    icon: Users,
    suggestedParentCode: '113', // العملاء والمدينون
    fallbackParentCode: '11',
    accountType: 'ASSET',
    nature: 'DEBIT',
    financialStatement: 'BALANCE_SHEET',
    exampleNames: ['شركة الهلال التجارية', 'محلات الأمل للمواد الغذائية', 'العميل محمد أحمد صالح'],
    explanation: 'حساب زبون لتسجيل فواتير المبيعات الآجلة والدفعات المسددة منه.',
    natureExplanation: 'طبيعة مدينة: الحساب مدين بما عليه من بضاعة، ويقل بالسداد.',
    colorClass: 'text-sky-700',
    borderClass: 'border-sky-300 hover:border-sky-500 hover:bg-sky-50/50',
    badgeBg: 'bg-sky-100 text-sky-800',
  },
  {
    id: 'SUPPLIER',
    title: 'مورد بضاعة أو خدمات',
    subtitle: 'جهة أو تاجر جملة نشتري منه (ذمم دائنة)',
    icon: Truck,
    suggestedParentCode: '211', // الموردون والدائنون
    fallbackParentCode: '21',  // الالتزامات المتداولة
    accountType: 'LIABILITY',
    nature: 'CREDIT',
    financialStatement: 'BALANCE_SHEET',
    exampleNames: ['شركة نادك للألبان', 'مؤسسة البركة للاستيراد', 'المورد سعيد التاجر'],
    explanation: 'حساب مورد لتسجيل فواتير المشتريات الآجلة والدفعات النقدية المسددة له.',
    natureExplanation: 'طبيعة دائنة: الحساب دائن بقيمة الفاتورة، ويقل عندما نقوم بسداده.',
    colorClass: 'text-amber-700',
    borderClass: 'border-amber-300 hover:border-amber-500 hover:bg-amber-50/50',
    badgeBg: 'bg-amber-100 text-amber-800',
  },
  {
    id: 'EXPENSE',
    title: 'مصروف تشغيلي أو إداري',
    subtitle: 'إيجار، كهرباء، نظافة، ضيافة، وقود...',
    icon: Receipt,
    suggestedParentCode: '41', // المصروفات العمومية والإدارية
    fallbackParentCode: '4',   // المصروفات
    accountType: 'EXPENSE',
    nature: 'DEBIT',
    financialStatement: 'INCOME_STATEMENT',
    exampleNames: ['إيجار المحل الشهري', 'فاتورة كهرباء ومولد', 'مصاريف ضيافة وبوفيه', 'وقود ونقل بضائع'],
    explanation: 'نفقات استهلاكية تشغيلية للمحل وتظهر في قائمة الدخل لخصمها من الإيرادات.',
    natureExplanation: 'طبيعة مدينة: تسجل دائماً في الطرف المدين وتخفض الأرباح.',
    colorClass: 'text-rose-700',
    borderClass: 'border-rose-300 hover:border-rose-500 hover:bg-rose-50/50',
    badgeBg: 'bg-rose-100 text-rose-800',
  },
  {
    id: 'ASSET_FIXED',
    title: 'أصل ثابت أو معدات تشغيل',
    subtitle: 'سيارة، جهاز كمبيوتر، ثلاجة، ديكور...',
    icon: Building2,
    suggestedParentCode: '12', // الأصول الثابتة
    fallbackParentCode: '1',  // الأصول
    accountType: 'ASSET',
    nature: 'DEBIT',
    financialStatement: 'BALANCE_SHEET',
    exampleNames: ['سيارة نقل التوزيع رقم 1', 'ثلاجة عرض مركزية', 'أجهزة كمبيوتر ونقاط بيع'],
    explanation: 'ممتلكات معمرة يمتلكها المحل لمساعدته في التشغيل وليس بغرض بيعها السريع.',
    natureExplanation: 'طبيعة مدينة: تمثل ممتلكات للمنشأة وتستهلك على عدة سنوات.',
    colorClass: 'text-purple-700',
    borderClass: 'border-purple-300 hover:border-purple-500 hover:bg-purple-50/50',
    badgeBg: 'bg-purple-100 text-purple-800',
  },
  {
    id: 'REVENUE',
    title: 'إيراد مبيعات أو نشاط إضافي',
    subtitle: 'إيراد خدمات، توصيل، مبيعات أصناف خاصة...',
    icon: TrendingUp,
    suggestedParentCode: '31', // إيرادات النشاط
    fallbackParentCode: '3',   // الإيرادات
    accountType: 'REVENUE',
    nature: 'CREDIT',
    financialStatement: 'INCOME_STATEMENT',
    exampleNames: ['إيرادات خدمات التوصيل السريع', 'إيرادات عمولات ووساطة', 'إيرادات تغليف هدايا'],
    explanation: 'عوائد مالية تدخل المنشأة وتزيد من صافي ربح النشاط في قائمة الدخل.',
    natureExplanation: 'طبيعة دائنة: تسجل في الطرف الدائن وتزيد من أرباح المنشأة.',
    colorClass: 'text-teal-700',
    borderClass: 'border-teal-300 hover:border-teal-500 hover:bg-teal-50/50',
    badgeBg: 'bg-teal-100 text-teal-800',
  },
  {
    id: 'EQUITY',
    title: 'حساب شريك أو رأس مال',
    subtitle: 'حساب جاري الشريك، رأس المال، مسحوبات...',
    icon: Briefcase,
    suggestedParentCode: '22', // حقوق الملكية
    fallbackParentCode: '2',   // الخصوم وحقوق الملكية
    accountType: 'EQUITY',
    nature: 'CREDIT',
    financialStatement: 'BALANCE_SHEET',
    exampleNames: ['جاري الشريك أحمد', 'رأس مال الشريك الثاني', 'مسحوبات الشركاء الشخصية'],
    explanation: 'حسابات حقوق الملاك والشركاء في المنشأة واستثماراتهم ومسحوباتهم.',
    natureExplanation: 'طبيعة دائنة: تمثل التزاماً من المنشأة تجاه مالكها أو الشركاء.',
    colorClass: 'text-amber-800',
    borderClass: 'border-amber-300 hover:border-amber-500 hover:bg-amber-50/50',
    badgeBg: 'bg-amber-100 text-amber-900',
  },
];

export const SmartAccountCreationModal: React.FC<SmartAccountCreationModalProps> = ({
  isOpen,
  onClose,
  chartOfAccounts,
  initialParentCode,
  onSaveAccount,
}) => {
  // Mode: SIMPLE (لغير المتخصصين وأصحاب المنشآت) vs ADVANCED (للمحاسبين والمتخصصين)
  const [modalMode, setModalMode] = useState<'SIMPLE' | 'ADVANCED'>('SIMPLE');

  // Flattened accounts list for lookups and validations
  const allAccountsFlat = useMemo(() => {
    return getAllAccountsFlat(chartOfAccounts);
  }, [chartOfAccounts]);

  // Selected Archetype in Simple Mode
  const [selectedArchetypeId, setSelectedArchetypeId] = useState<string>('CASHBOX');
  const activeArchetype = useMemo(() => {
    return ARCHETYPES.find((a) => a.id === selectedArchetypeId) || ARCHETYPES[0];
  }, [selectedArchetypeId]);

  // Form Fields
  const [accountName, setAccountName] = useState('');
  const [accountNameEn, setAccountNameEn] = useState('');
  const [parentCode, setParentCode] = useState<string>(initialParentCode || '111');
  const [customCode, setCustomCode] = useState<string>('');
  const [nature, setNature] = useState<'DEBIT' | 'CREDIT'>('DEBIT');
  const [accountType, setAccountType] = useState<'ASSET' | 'LIABILITY' | 'EQUITY' | 'REVENUE' | 'EXPENSE'>('ASSET');
  const [isLeaf, setIsLeaf] = useState<boolean>(true);
  const [financialStatement, setFinancialStatement] = useState<'BALANCE_SHEET' | 'INCOME_STATEMENT'>('BALANCE_SHEET');
  const [currency, setCurrency] = useState('YER');
  const [notes, setNotes] = useState('');

  // When initialParentCode is provided, default to it
  useEffect(() => {
    if (initialParentCode) {
      setParentCode(initialParentCode);
      const parentNode = allAccountsFlat.find((a) => a.code === initialParentCode);
      if (parentNode) {
        setAccountType(parentNode.type);
        setNature(parentNode.nature);
        setFinancialStatement(
          parentNode.financialStatement ||
            (parentNode.type === 'ASSET' || parentNode.type === 'LIABILITY' || parentNode.type === 'EQUITY'
              ? 'BALANCE_SHEET'
              : 'INCOME_STATEMENT')
        );
      }
    }
  }, [initialParentCode, allAccountsFlat]);

  // Resolve best parent code in Simple Mode
  const resolvedSimpleParentCode = useMemo(() => {
    if (initialParentCode) return initialParentCode;
    // Try finding exact suggested parent
    const hasSuggested = allAccountsFlat.some((a) => a.code === activeArchetype.suggestedParentCode);
    if (hasSuggested) return activeArchetype.suggestedParentCode;
    const hasFallback = allAccountsFlat.some((a) => a.code === activeArchetype.fallbackParentCode);
    if (hasFallback) return activeArchetype.fallbackParentCode;
    // Fallback to top level
    const topCode = activeArchetype.accountType === 'ASSET' ? '1' :
                    activeArchetype.accountType === 'LIABILITY' || activeArchetype.accountType === 'EQUITY' ? '2' :
                    activeArchetype.accountType === 'REVENUE' ? '3' : '4';
    return topCode;
  }, [initialParentCode, activeArchetype, allAccountsFlat]);

  // Active Parent Code based on mode
  const effectiveParentCode = modalMode === 'SIMPLE' ? resolvedSimpleParentCode : parentCode;

  // Selected Parent Account Object
  const parentAccountNode = useMemo(() => {
    return allAccountsFlat.find((a) => a.code === effectiveParentCode);
  }, [allAccountsFlat, effectiveParentCode]);

  // Auto-calculated next sequential pure-integer account code
  const autoSuggestedCodeInfo = useMemo(() => {
    if (!effectiveParentCode) return { code: '1001', level: 1, treePath: '/1001/' };
    return generateNextChildAccountCode(effectiveParentCode, chartOfAccounts);
  }, [effectiveParentCode, chartOfAccounts]);

  // Code to use
  const finalCode = (modalMode === 'ADVANCED' && customCode.trim()) ? customCode.trim() : autoSuggestedCodeInfo.code;

  // Validation Checks
  const validation = useMemo(() => {
    const errors: string[] = [];
    const warnings: string[] = [];

    if (!accountName.trim()) {
      errors.push('يرجى إدخال اسم الحساب');
    }

    if (!finalCode) {
      errors.push('تعذر توليد كود حساب تسلسلي، اختر حساباً أباً صالحاً');
    } else {
      // Check if code is pure integer
      if (!/^\d+$/.test(finalCode)) {
        errors.push('رقم الحساب يجب أن يتكون من أرقام صحيحة متسلسلة فقط');
      }
      // Check duplicate code
      const isDuplicateCode = allAccountsFlat.some((a) => a.code === finalCode);
      if (isDuplicateCode) {
        errors.push(`رقم الحساب [${finalCode}] مستخدم بالفعل، اختر رقماً آخر`);
      }
    }

    // Check duplicate name under same parent
    const isDuplicateName = allAccountsFlat.some(
      (a) => a.parentCode === effectiveParentCode && a.name.trim() === accountName.trim()
    );
    if (isDuplicateName) {
      warnings.push(`يوجد حساب آخر بنفس الاسم [${accountName}] تحت نفس الحساب الأب`);
    }

    // Advanced: Check if code starts with parent code
    if (modalMode === 'ADVANCED' && effectiveParentCode && !finalCode.startsWith(effectiveParentCode)) {
      warnings.push(`يُفضل أن يبدأ كود الحساب ببادئة الحساب الأب [${effectiveParentCode}] لضمان التسلسل الهرمي`);
    }

    return {
      isValid: errors.length === 0,
      errors,
      warnings,
    };
  }, [accountName, finalCode, effectiveParentCode, allAccountsFlat, modalMode]);

  // Hierarchy Path string for visual clarity
  const hierarchyPath = useMemo(() => {
    if (!parentAccountNode) return '';
    const rawPath = parentAccountNode.treePath || `/${parentAccountNode.code}/`;
    const codes = rawPath.split('/').filter(Boolean);
    const names = codes.map((c) => {
      const found = allAccountsFlat.find((a) => a.code === c);
      return found ? found.name.split('(')[0].trim() : c;
    });
    return [...names, accountName.trim() || 'الحساب الجديد'].join(' ❯ ');
  }, [parentAccountNode, allAccountsFlat, accountName]);

  // Apply archetype settings when archetype is selected
  const handleSelectArchetype = (archetypeId: string) => {
    setSelectedArchetypeId(archetypeId);
    const arch = ARCHETYPES.find((a) => a.id === archetypeId);
    if (!arch) return;
    setAccountType(arch.accountType);
    setNature(arch.nature);
    setFinancialStatement(arch.financialStatement);
  };

  // Submit Handler
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!validation.isValid) return;

    const parentLevel = parentAccountNode?.level || effectiveParentCode.length;
    const parentPath = parentAccountNode?.treePath || `/${effectiveParentCode}/`;
    const resolvedLevel = autoSuggestedCodeInfo.level || parentLevel + 1;
    const resolvedTreePath = `${parentPath}${finalCode}/`;

    const newAccount: AccountNode = {
      code: finalCode,
      name: accountName.trim(),
      nameEn: accountNameEn.trim(),
      type: modalMode === 'SIMPLE' ? activeArchetype.accountType : (parentAccountNode?.type || accountType),
      nature: modalMode === 'SIMPLE' ? activeArchetype.nature : nature,
      financialStatement: modalMode === 'SIMPLE' ? activeArchetype.financialStatement : financialStatement,
      balance: 0,
      openingBalance: 0,
      level: resolvedLevel,
      treePath: resolvedTreePath,
      isLeaf: modalMode === 'SIMPLE' ? true : isLeaf,
      isSubAccount: modalMode === 'SIMPLE' ? true : isLeaf,
      currency,
      allowCostCenter: false,
      status: 'ACTIVE',
      notes: notes.trim(),
      parentCode: effectiveParentCode,
      children: [],
    };

    onSaveAccount(newAccount);
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div
      id="smart-account-modal-overlay"
      className="fixed inset-0 z-50 flex items-center justify-center p-2 sm:p-4 bg-slate-950/70 backdrop-blur-xs select-none"
      dir="rtl"
    >
      <div
        id="smart-account-modal-container"
        className="bg-white rounded-2xl w-full max-w-2xl max-h-[92vh] shadow-2xl border border-slate-300 flex flex-col overflow-hidden text-right animate-in fade-in zoom-in-95 duration-150"
      >
        {/* ============================================================== */}
        {/* MODAL HEADER                                                    */}
        {/* ============================================================== */}
        <div className="bg-[#0277bd] text-white px-5 py-3.5 flex items-center justify-between shrink-0 shadow-xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-white/20 flex items-center justify-center font-bold text-white shadow-xs">
              <FolderPlus className="w-5 h-5 text-amber-300" />
            </div>
            <div>
              <h2 className="text-sm sm:text-base font-bold flex items-center gap-2">
                <span>إضافة وتأسيس حساب جديد في شجرة الحسابات</span>
                <span className="text-[10px] bg-white/20 text-white px-2 py-0.5 rounded-full font-bold">
                  مساعد النظام الذكي
                </span>
              </h2>
              <p className="text-[11px] text-sky-100">
                تصنيف محاسبي سليم وضبط تلقائي للأب والأرقام المتسلسلة لمنع الأخطاء
              </p>
            </div>
          </div>

          <button
            onClick={onClose}
            className="p-1 hover:bg-white/20 rounded-full transition text-white cursor-pointer"
            title="إغلاق"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* ============================================================== */}
        {/* MODE SWITCHER TABS (المبسط لغير المتخصصين vs المتقدم للمحاسبين)  */}
        {/* ============================================================== */}
        <div className="bg-slate-100 px-4 py-2 border-b border-slate-200 flex items-center justify-between gap-2 shrink-0">
          <div className="flex items-center gap-1 bg-white p-1 rounded-xl border border-slate-300 shadow-2xs">
            <button
              type="button"
              onClick={() => setModalMode('SIMPLE')}
              className={`px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
                modalMode === 'SIMPLE'
                  ? 'bg-[#0277bd] text-white shadow-xs'
                  : 'text-slate-600 hover:text-slate-900 hover:bg-slate-100'
              }`}
            >
              <Sparkles className="w-3.5 h-3.5 text-amber-300" />
              <span>الوضع المبسط (لأصحاب المنشآت وغير المتخصصين)</span>
            </button>
            <button
              type="button"
              onClick={() => setModalMode('ADVANCED')}
              className={`px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
                modalMode === 'ADVANCED'
                  ? 'bg-[#0277bd] text-white shadow-xs'
                  : 'text-slate-600 hover:text-slate-900 hover:bg-slate-100'
              }`}
            >
              <Briefcase className="w-3.5 h-3.5" />
              <span>الوضع المحاسبي المتقدم (للمحاسبين والمتخصصين)</span>
            </button>
          </div>

          <div className="hidden sm:flex items-center gap-1 text-[11px] text-slate-500 font-semibold">
            <ShieldCheck className="w-3.5 h-3.5 text-emerald-600" />
            <span>نظام الحسابات القياسي اليمني</span>
          </div>
        </div>

        {/* ============================================================== */}
        {/* MODAL BODY (SCROLLABLE)                                         */}
        {/* ============================================================== */}
        <form onSubmit={handleSubmit} className="flex-1 overflow-y-auto p-4 sm:p-5 space-y-4 text-xs">
          {/* ------------------------------------------------------------ */}
          {/* 1. SIMPLE MODE: ARCHETYPE CARDS SELECTOR                     */}
          {/* ------------------------------------------------------------ */}
          {modalMode === 'SIMPLE' && (
            <div className="space-y-3">
              <div>
                <label className="block font-bold text-slate-800 text-xs mb-1.5">
                  1. ما هو نوع الحساب الذي ترغب في إنشائه؟ (اختر من النماذج الشائعة):
                </label>
                <div className="grid grid-cols-1 sm:grid-cols-3 gap-2">
                  {ARCHETYPES.map((arch) => {
                    const isSelected = selectedArchetypeId === arch.id;
                    const IconComponent = arch.icon;
                    return (
                      <button
                        key={arch.id}
                        type="button"
                        onClick={() => handleSelectArchetype(arch.id)}
                        className={`p-2.5 rounded-xl border text-right transition cursor-pointer flex flex-col justify-between gap-1.5 relative ${
                          isSelected
                            ? 'border-[#0277bd] bg-sky-50/80 ring-2 ring-[#0277bd]/30 shadow-xs'
                            : `border-slate-200 bg-white hover:border-slate-300 hover:bg-slate-50`
                        }`}
                      >
                        {isSelected && (
                          <div className="absolute top-2 left-2 w-4 h-4 rounded-full bg-[#0277bd] text-white flex items-center justify-center">
                            <Check className="w-3 h-3 stroke-[3]" />
                          </div>
                        )}
                        <div className="flex items-center gap-2">
                          <div
                            className={`w-7 h-7 rounded-lg flex items-center justify-center shrink-0 ${
                              isSelected ? 'bg-[#0277bd] text-white' : 'bg-slate-100 text-slate-700'
                            }`}
                          >
                            <IconComponent className="w-4 h-4" />
                          </div>
                          <span className="font-bold text-slate-900 text-xs leading-tight">
                            {arch.title}
                          </span>
                        </div>
                        <p className="text-[10px] text-slate-500 line-clamp-2 leading-relaxed">
                          {arch.subtitle}
                        </p>
                      </button>
                    );
                  })}
                </div>
              </div>

              {/* Active Archetype explanation */}
              <div className="p-3 bg-sky-50 rounded-xl border border-sky-200 text-sky-950 flex items-start gap-2.5">
                <Info className="w-4 h-4 text-[#0277bd] shrink-0 mt-0.5" />
                <div className="space-y-1">
                  <div className="font-bold text-xs text-[#0277bd]">
                    مساعدة النظام التلقائية لهذا النوع ({activeArchetype.title}):
                  </div>
                  <div className="text-[11px] text-slate-700 leading-relaxed">
                    {activeArchetype.explanation}
                  </div>
                  <div className="flex flex-wrap items-center gap-2 pt-1">
                    <span className="text-[10px] font-bold px-2 py-0.5 rounded-md bg-white border border-sky-200 text-sky-900">
                      الحساب الأب: {parentAccountNode ? parentAccountNode.name : activeArchetype.suggestedParentCode}
                    </span>
                    <span className={`text-[10px] font-bold px-2 py-0.5 rounded-md ${activeArchetype.badgeBg}`}>
                      {activeArchetype.natureExplanation}
                    </span>
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* ------------------------------------------------------------ */}
          {/* 2. ADVANCED MODE: PARENT SELECTOR & CONFIGURATION            */}
          {/* ------------------------------------------------------------ */}
          {modalMode === 'ADVANCED' && (
            <div className="space-y-3 bg-slate-50 p-3.5 rounded-xl border border-slate-200">
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                {/* Parent Account Dropdown */}
                <div>
                  <label className="block font-bold text-slate-700 text-xs mb-1">
                    الحساب الأب الرئيسي (Parent Account):
                  </label>
                  <select
                    value={parentCode}
                    onChange={(e) => {
                      setParentCode(e.target.value);
                      const parentNode = allAccountsFlat.find((a) => a.code === e.target.value);
                      if (parentNode) {
                        setAccountType(parentNode.type);
                        setNature(parentNode.nature);
                        setFinancialStatement(
                          parentNode.financialStatement ||
                            (parentNode.type === 'ASSET' || parentNode.type === 'LIABILITY' || parentNode.type === 'EQUITY'
                              ? 'BALANCE_SHEET'
                              : 'INCOME_STATEMENT')
                        );
                      }
                    }}
                    className="w-full border border-slate-300 rounded-lg p-2 bg-white text-xs text-slate-900 font-semibold focus:ring-2 focus:ring-[#0277bd] outline-hidden"
                  >
                    {allAccountsFlat.map((acc) => (
                      <option key={acc.code} value={acc.code}>
                        {acc.code} - {acc.name} ({acc.level === 1 ? 'رئيسي عام' : `مستوى ${acc.level}`})
                      </option>
                    ))}
                  </select>
                </div>

                {/* Custom Account Code */}
                <div>
                  <label className="block font-bold text-slate-700 text-xs mb-1 flex items-center justify-between">
                    <span>رقم / كود الحساب المقترح:</span>
                    <span className="text-[10px] text-sky-700 font-normal">توليد آلي تسلسلي</span>
                  </label>
                  <input
                    type="text"
                    value={customCode || autoSuggestedCodeInfo.code}
                    onChange={(e) => setCustomCode(e.target.value)}
                    placeholder={autoSuggestedCodeInfo.code}
                    className="w-full border border-slate-300 rounded-lg p-2 bg-white font-mono font-bold text-slate-900 focus:ring-2 focus:ring-[#0277bd] outline-hidden"
                    required
                  />
                </div>
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-3 gap-3 pt-1">
                {/* Account Nature */}
                <div>
                  <label className="block font-bold text-slate-700 text-xs mb-1">
                    طبيعة الحساب:
                  </label>
                  <select
                    value={nature}
                    onChange={(e) => setNature(e.target.value as any)}
                    className="w-full border border-slate-300 rounded-lg p-1.5 bg-white text-xs font-bold text-slate-800"
                  >
                    <option value="DEBIT">مدين (Debit) - يزيد بالمقبوضات</option>
                    <option value="CREDIT">دائن (Credit) - يزيد بالالتزامات</option>
                  </select>
                </div>

                {/* Account Type (Leaf vs Parent) */}
                <div>
                  <label className="block font-bold text-slate-700 text-xs mb-1">
                    تصنيف الحساب:
                  </label>
                  <select
                    value={isLeaf ? 'LEAF' : 'PARENT'}
                    onChange={(e) => setIsLeaf(e.target.value === 'LEAF')}
                    className="w-full border border-slate-300 rounded-lg p-1.5 bg-white text-xs font-bold text-slate-800"
                  >
                    <option value="LEAF">حساب فرعي حركي (يقبل القيود)</option>
                    <option value="PARENT">حساب رئيسي تجميعي (مجلد فروع)</option>
                  </select>
                </div>

                {/* Financial Statement */}
                <div>
                  <label className="block font-bold text-slate-700 text-xs mb-1">
                    القائمة المالية الختامية:
                  </label>
                  <select
                    value={financialStatement}
                    onChange={(e) => setFinancialStatement(e.target.value as any)}
                    className="w-full border border-slate-300 rounded-lg p-1.5 bg-white text-xs font-bold text-slate-800"
                  >
                    <option value="BALANCE_SHEET">ميزانية عمومية (Balance Sheet)</option>
                    <option value="INCOME_STATEMENT">قائمة الدخل (الأرباح والخسائر)</option>
                  </select>
                </div>
              </div>
            </div>
          )}

          {/* ------------------------------------------------------------ */}
          {/* 3. COMMON INPUTS: NAME & DETAILS                             */}
          {/* ------------------------------------------------------------ */}
          <div className="space-y-3">
            <div>
              <label className="block font-bold text-slate-800 text-xs mb-1">
                اسم الحساب (عربي) <span className="text-red-500">*</span>:
              </label>
              <input
                type="text"
                required
                value={accountName}
                onChange={(e) => setAccountName(e.target.value)}
                placeholder="مثال: صندوق المبيعات الرئيسي، بنك الكريمي فرع التحرير، كهرباء المحل..."
                className="w-full border border-slate-300 rounded-xl p-2.5 text-xs text-slate-900 focus:ring-2 focus:ring-[#0277bd] focus:border-[#0277bd] outline-hidden font-medium"
              />

              {/* Example quick suggestions in Simple mode */}
              {modalMode === 'SIMPLE' && activeArchetype.exampleNames.length > 0 && (
                <div className="flex flex-wrap items-center gap-1.5 mt-1.5">
                  <span className="text-[10px] text-slate-500">أمثلة سريعة:</span>
                  {activeArchetype.exampleNames.map((ex) => (
                    <button
                      key={ex}
                      type="button"
                      onClick={() => setAccountName(ex)}
                      className="text-[10px] bg-slate-100 hover:bg-sky-100 text-slate-700 hover:text-sky-800 px-2 py-0.5 rounded-md border border-slate-200 transition cursor-pointer"
                    >
                      {ex}
                    </button>
                  ))}
                </div>
              )}
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
              <div>
                <label className="block font-bold text-slate-700 text-xs mb-1">
                  اسم الحساب (إنجليزي - اختياري):
                </label>
                <input
                  type="text"
                  value={accountNameEn}
                  onChange={(e) => setAccountNameEn(e.target.value)}
                  placeholder="e.g. Branch Main Cashbox"
                  className="w-full border border-slate-300 rounded-xl p-2 text-xs text-slate-900 focus:ring-2 focus:ring-[#0277bd] outline-hidden"
                />
              </div>

              <div>
                <label className="block font-bold text-slate-700 text-xs mb-1">
                  عملة الحساب الافتراضية:
                </label>
                <select
                  value={currency}
                  onChange={(e) => setCurrency(e.target.value)}
                  className="w-full border border-slate-300 rounded-xl p-2 bg-white text-xs font-bold text-slate-800 focus:ring-2 focus:ring-[#0277bd] outline-hidden"
                >
                  <option value="YER">ريال يمني (YER)</option>
                  <option value="SAR">ريال سعودي (SAR)</option>
                  <option value="USD">دولار أمريكي (USD)</option>
                </select>
              </div>
            </div>

            <div>
              <label className="block font-bold text-slate-700 text-xs mb-1">
                ملاحظات أو وصف إضافي (اختياري):
              </label>
              <input
                type="text"
                value={notes}
                onChange={(e) => setNotes(e.target.value)}
                placeholder="أي تفاصيل تود تدوينها حول الحساب..."
                className="w-full border border-slate-300 rounded-xl p-2 text-xs text-slate-900 focus:ring-2 focus:ring-[#0277bd] outline-hidden"
              />
            </div>
          </div>

          {/* ------------------------------------------------------------ */}
          {/* 4. LIVE GUIDANCE & ERROR PREVENTION SUMMARY CARD             */}
          {/* ------------------------------------------------------------ */}
          <div className="bg-slate-50 rounded-xl p-3.5 border border-slate-200 space-y-2">
            <div className="flex items-center justify-between text-[11px] font-bold text-slate-800">
              <div className="flex items-center gap-1.5">
                <ShieldCheck className="w-4 h-4 text-emerald-600" />
                <span>فحص وضمان البيانات المحاسبية الصحيحة:</span>
              </div>
              <span className="font-mono text-[#0277bd]">كود الحساب: [{finalCode}]</span>
            </div>

            {/* Tree Path */}
            <div className="text-[11px] text-slate-600 flex items-center gap-1 flex-wrap font-medium">
              <span className="text-slate-400">مسار الحساب في الشجرة:</span>
              <span className="text-slate-800 font-bold bg-white px-2 py-0.5 rounded border border-slate-200">
                {hierarchyPath || 'شجرة الحسابات'}
              </span>
            </div>

            {/* Accounting Facts Pill */}
            <div className="flex flex-wrap items-center gap-1.5 pt-1 text-[10px]">
              <span className="bg-white border border-slate-200 px-2 py-0.5 rounded font-bold text-slate-700">
                الأب: [{effectiveParentCode}] {parentAccountNode?.name.split('(')[0]}
              </span>
              <span className="bg-white border border-slate-200 px-2 py-0.5 rounded font-bold text-slate-700">
                الطبيعة: {modalMode === 'SIMPLE' ? activeArchetype.nature : nature === 'DEBIT' ? 'مدين (Debit)' : 'دائن (Credit)'}
              </span>
              <span className="bg-white border border-slate-200 px-2 py-0.5 rounded font-bold text-slate-700">
                يظهر في: {modalMode === 'SIMPLE' ? (activeArchetype.financialStatement === 'BALANCE_SHEET' ? 'الميزانية العمومية' : 'قائمة الدخل') : financialStatement === 'BALANCE_SHEET' ? 'الميزانية العمومية' : 'قائمة الدخل'}
              </span>
              <span className="bg-emerald-50 border border-emerald-200 px-2 py-0.5 rounded font-bold text-emerald-700">
                ✓ أرقام تسلسلية صحيحة (Pure Integer)
              </span>
            </div>

            {/* Validation Feedback */}
            {validation.errors.length > 0 && (
              <div className="p-2 bg-rose-50 border border-rose-200 rounded-lg text-rose-800 text-[11px] space-y-0.5 font-bold">
                {validation.errors.map((err, i) => (
                  <div key={i} className="flex items-center gap-1.5">
                    <AlertCircle className="w-3.5 h-3.5 text-rose-600 shrink-0" />
                    <span>{err}</span>
                  </div>
                ))}
              </div>
            )}

            {validation.warnings.length > 0 && (
              <div className="p-2 bg-amber-50 border border-amber-200 rounded-lg text-amber-800 text-[11px] space-y-0.5 font-medium">
                {validation.warnings.map((warn, i) => (
                  <div key={i} className="flex items-center gap-1.5">
                    <AlertCircle className="w-3.5 h-3.5 text-amber-600 shrink-0" />
                    <span>{warn}</span>
                  </div>
                ))}
              </div>
            )}
          </div>
        </form>

        {/* ============================================================== */}
        {/* MODAL FOOTER ACTIONS                                            */}
        {/* ============================================================== */}
        <div className="bg-slate-100 px-5 py-3 border-t border-slate-200 flex items-center justify-between shrink-0">
          <div className="text-[11px] text-slate-500 font-medium">
            {modalMode === 'SIMPLE' ? 'وضع الإدخال المبسط الذكي' : 'وضع المحاسبين المتقدم'}
          </div>

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
              onClick={handleSubmit}
              disabled={!validation.isValid}
              className={`px-5 py-2 rounded-xl font-bold transition flex items-center gap-1.5 shadow-sm text-xs cursor-pointer ${
                validation.isValid
                  ? 'bg-[#0277bd] hover:bg-[#01579b] text-white active:scale-95'
                  : 'bg-slate-300 text-slate-500 cursor-not-allowed'
              }`}
            >
              <Check className="w-4 h-4" />
              <span>حفظ وتثبيت الحساب في الشجرة</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
