'use client';

import React, { useState, useMemo } from 'react';
import { 
  ShoppingCart, 
  Package, 
  Truck, 
  BookOpen, 
  BarChart3, 
  FileCode2, 
  PauseCircle, 
  PlusCircle, 
  RefreshCw, 
  Coins, 
  Calculator, 
  RotateCcw, 
  Shield, 
  Users, 
  Store, 
  Receipt, 
  UserCheck, 
  FileSpreadsheet, 
  Sparkles,
  Layers,
  FileText,
  Sliders,
  UploadCloud,
  ChevronLeft,
  ChevronRight,
  Wallet,
  LayoutGrid,
  Database,
} from 'lucide-react';
import { MainTabType } from '@/types/pos';
import { WinFormsMenuStrip } from './WinFormsMenuStrip';

interface DesktopMenuBarProps {
  activeTab: MainTabType;
  onSelectTab: (tab: MainTabType) => void;
  heldOrdersCount: number;
  onOpenHeldOrders: () => void;
  onNewPOSBill: () => void;
  onHoldBill: () => void;
  onQuickSearch: () => void;
  onOpenCalculator: () => void;
  onOpenArchitecture: () => void;
  onPrintReport: () => void;
  onOpenLogin?: () => void;
  onLockScreen?: () => void;
  onOpenReportItem?: (reportId: string) => void;
  onOpenBackupRestore?: () => void;
  onOpenShowDialogShowcase?: () => void;
  currency: string;
  drawerCash: number;
}

export interface OperationalPhase {
  id: number;
  number: string;
  name: string;
  shortName: string;
  badgeBg: string;
  activeColor: string;
  tabs: {
    id: MainTabType;
    label: string;
    shortcut?: string;
    icon: React.ReactNode;
    badge?: string;
    isLab?: boolean;
    isPrimary?: boolean;
    titleAttr?: string;
  }[];
}

export const DesktopMenuBar: React.FC<DesktopMenuBarProps> = ({
  activeTab,
  onSelectTab,
  heldOrdersCount,
  onOpenHeldOrders,
  onNewPOSBill,
  onHoldBill,
  onOpenCalculator,
  onOpenArchitecture,
  onPrintReport,
  onOpenLogin,
  onLockScreen,
  onOpenReportItem,
  onOpenBackupRestore,
  onOpenShowDialogShowcase,
  currency,
  drawerCash,
}) => {
  // Navigation mode: 'PHASED' (organized by stages) or 'ALL' (classic all tabs)
  const [viewMode, setViewMode] = useState<'PHASED' | 'ALL'>('PHASED');
  const [manualPhaseId, setManualPhaseId] = useState<number | null>(null);

  // Map activeTab to its business lifecycle phase
  const autoPhaseId = useMemo(() => {
    if ([
      'SALES_INVOICE', 'POS', 'SALES_STRATEGIES_HUB', 
      'SALES_STRATEGY_INLINE_GRID', 'SALES_STRATEGY_RAPID_BAR', 
      'SALES_STRATEGY_SCANNER', 'SALES_STRATEGY_SMART_F3', 
      'SALES_STRATEGY_TOUCH_GRID', 'SALES_STRATEGY_IMPORT_CONVERT', 
      'PRICE_QUOTATION', 'SALES_RETURNS'
    ].includes(activeTab)) return 1;

    if ([
      'INVENTORY', 'PURCHASES', 'PURCHASE_ORDER', 
      'COMMERCIAL_PURCHASE', 'MOHASEB_ITEMS', 
      'EXCEL_IMPORT', 'SALES_PRICES', 'ITEM_UNITS'
    ].includes(activeTab)) return 2;

    if (['CUSTOMERS', 'PETTY_CASH'].includes(activeTab)) return 3;

    if ([
      'ACCOUNTING', 'MOHASEB_ACCOUNTS', 
      'CURRENCIES', 'CURRENCY_RATES'
    ].includes(activeTab)) return 4;

    if ([
      'REPORTS', 'OTHER_REPORTS', 'EMPLOYEES', 
      'SECURITY', 'ARCHITECTURE'
    ].includes(activeTab)) return 5;

    return 1; // Default to Phase 1 when on Dashboard
  }, [activeTab]);

  // Current active phase
  const activePhaseId = manualPhaseId ?? autoPhaseId;

  // The 5 Comprehensive ERP Workflow Phases
  const phases: OperationalPhase[] = useMemo(() => [
    {
      id: 1,
      number: '1',
      name: 'المبيعات ونقاط البيع',
      shortName: 'المبيعات',
      badgeBg: 'bg-sky-500/20 text-sky-300 border-sky-500/40',
      activeColor: 'from-sky-600 to-blue-700',
      tabs: [
        {
          id: 'SALES_INVOICE',
          label: 'فاتورة مبيعات [frmSalesInvoice]',
          shortcut: 'Ctrl+I',
          icon: <Receipt className="w-3.5 h-3.5" />,
          isPrimary: true,
          titleAttr: 'فاتورة مبيعات تجارية شاملة',
        },
        {
          id: 'POS',
          label: 'نقطة البيع السريعة [frmPOS]',
          shortcut: 'F1',
          icon: <ShoppingCart className="w-3.5 h-3.5" />,
          isPrimary: true,
          titleAttr: 'نقطة بيع التجزئة السريعة للكاشير',
        },
        {
          id: 'SALES_STRATEGIES_HUB',
          label: 'مختبر الـ 6 استراتيجيات',
          badge: 'UX Lab',
          isLab: true,
          icon: <Sparkles className="w-3.5 h-3.5 text-amber-400" />,
          titleAttr: 'مختبر تجربة ومقارنة استراتيجيات إدخال الفواتير الست',
        },
        {
          id: 'PRICE_QUOTATION',
          label: 'عروض الأسعار',
          shortcut: 'Ctrl+Q',
          icon: <FileText className="w-3.5 h-3.5" />,
          titleAttr: 'إصدار ومتابعة عروض أسعار العملاء',
        },
        {
          id: 'SALES_RETURNS',
          label: 'مردودات المبيعات',
          shortcut: 'Ctrl+R',
          icon: <RotateCcw className="w-3.5 h-3.5" />,
          titleAttr: 'إشعار دائن وتسجيل بضاعة مرتجعة',
        },
      ],
    },
    {
      id: 2,
      number: '2',
      name: 'المخازن والمشتريات والتوريد',
      shortName: 'المخازن والتوريد',
      badgeBg: 'bg-amber-500/20 text-amber-300 border-amber-500/40',
      activeColor: 'from-amber-600 to-orange-700',
      tabs: [
        {
          id: 'INVENTORY',
          label: 'المخزون وبطاقة الأصناف [F5]',
          shortcut: 'F5',
          icon: <Package className="w-3.5 h-3.5" />,
          isPrimary: true,
          titleAttr: 'إدارة المخزون، الجرد، والمستودعات',
        },
        {
          id: 'PURCHASES',
          label: 'فواتير المشتريات [F6]',
          shortcut: 'F6',
          icon: <Truck className="w-3.5 h-3.5" />,
          isPrimary: true,
          titleAttr: 'تسجيل فواتير المشتريات وتوريد البضاعة',
        },
        {
          id: 'PURCHASE_ORDER',
          label: 'أوامر الشراء والتوريد',
          shortcut: 'Ctrl+O',
          icon: <FileText className="w-3.5 h-3.5" />,
          titleAttr: 'أوامر التوريد وطلبيات الموردين',
        },
        {
          id: 'COMMERCIAL_PURCHASE',
          label: 'فاتورة مشتريات تجارية',
          icon: <Receipt className="w-3.5 h-3.5" />,
          titleAttr: 'واجهة مشتريات تجارية موسعة',
        },
        {
          id: 'MOHASEB_ITEMS',
          label: 'دليل الأصناف وبطاقة الصنف',
          icon: <Layers className="w-3.5 h-3.5" />,
          titleAttr: 'دليل الأصناف الشامل والباركود',
        },
        {
          id: 'SALES_PRICES',
          label: 'تسعير الأصناف وقوائم الأسعار',
          icon: <Coins className="w-3.5 h-3.5" />,
          titleAttr: 'تعديل وتحديد أسعار البيع المعتمدة',
        },
        {
          id: 'EXCEL_IMPORT',
          label: 'استيراد الأصناف من إكسل',
          icon: <UploadCloud className="w-3.5 h-3.5" />,
          titleAttr: 'استيراد بضاعة وقوائم إكسل',
        },
        {
          id: 'ITEM_UNITS',
          label: 'وحدات القياس ومعاملات التحويل',
          icon: <Sliders className="w-3.5 h-3.5" />,
          titleAttr: 'إدارة الوحدات (حبة، باكت، كرتون...) ومعاملات الضرب',
        },
      ],
    },
    {
      id: 3,
      number: '3',
      name: 'العملاء والسيولة والعهد',
      shortName: 'العملاء والسيولة',
      badgeBg: 'bg-emerald-500/20 text-emerald-300 border-emerald-500/40',
      activeColor: 'from-emerald-600 to-teal-700',
      tabs: [
        {
          id: 'CUSTOMERS',
          label: 'العملاء والذمم وسندات القبض [F4]',
          shortcut: 'F4',
          icon: <Users className="w-3.5 h-3.5" />,
          isPrimary: true,
          titleAttr: 'كشوفات حسابات العملاء، الذمم، وسندات القبض والصرف',
        },
        {
          id: 'PETTY_CASH',
          label: 'إدارة العهد والمصروفات النثرية',
          shortcut: 'Ctrl+P',
          icon: <Wallet className="w-3.5 h-3.5" />,
          titleAttr: 'صرف وتسوية العهد المالية والنثريات اليومية',
        },
      ],
    },
    {
      id: 4,
      number: '4',
      name: 'الحسابات والمالية والعملات',
      shortName: 'الحسابات والعملات',
      badgeBg: 'bg-cyan-500/20 text-cyan-300 border-cyan-500/40',
      activeColor: 'from-cyan-600 to-blue-700',
      tabs: [
        {
          id: 'ACCOUNTING',
          label: 'القيود ودليل الحسابات [F7]',
          shortcut: 'F7',
          icon: <BookOpen className="w-3.5 h-3.5" />,
          isPrimary: true,
          titleAttr: 'القيود اليومية المتوازنة، الأستاذ العام، وميزان المراجعة',
        },
        {
          id: 'MOHASEB_ACCOUNTS',
          label: 'شجرة الحسابات والصناديق',
          icon: <Layers className="w-3.5 h-3.5" />,
          titleAttr: 'دليل الحسابات الشجري وكشف حركة الصناديق',
        },
        {
          id: 'CURRENCIES',
          label: 'دليل العملات النقدية',
          icon: <Coins className="w-3.5 h-3.5" />,
          titleAttr: 'إدارة العملات (ريال يمني، سعودي، دولار...)',
        },
        {
          id: 'CURRENCY_RATES',
          label: 'أسعار الصرف اليومية',
          icon: <RefreshCw className="w-3.5 h-3.5" />,
          titleAttr: 'سجل وتاريخ تحديث أسعار صرف العملات',
        },
      ],
    },
    {
      id: 5,
      number: '5',
      name: 'التقارير والرقابة والأمان',
      shortName: 'التقارير والأمان',
      badgeBg: 'bg-purple-500/20 text-purple-300 border-purple-500/40',
      activeColor: 'from-purple-600 to-indigo-700',
      tabs: [
        {
          id: 'REPORTS',
          label: 'لوحة التقارير والأرباح P&L [F8]',
          shortcut: 'F8',
          icon: <BarChart3 className="w-3.5 h-3.5" />,
          isPrimary: true,
          titleAttr: 'لوحة التحليلات، الأرباح، والخسائر',
        },
        {
          id: 'OTHER_REPORTS',
          label: 'تقارير أخرى مصنفة (18 تقريراً)',
          badge: '18 تقرير',
          icon: <FileSpreadsheet className="w-3.5 h-3.5 text-emerald-300" />,
          isPrimary: true,
          titleAttr: 'تقارير تفصيلية مصنفة على 4 مراحل (المبيعات، المخازن، الحسابات، الذمم)',
        },
        {
          id: 'EMPLOYEES',
          label: 'إدارة الموظفين والكوادر [Ctrl+E]',
          shortcut: 'Ctrl+E',
          icon: <UserCheck className="w-3.5 h-3.5 text-sky-300" />,
          titleAttr: 'بطاقات الموظفين، ربط المبيعات، والرواتب والعهد',
        },
        {
          id: 'SECURITY',
          label: 'الأمان ومصفوفة الصلاحيات (RBAC)',
          icon: <Shield className="w-3.5 h-3.5 text-purple-300" />,
          titleAttr: 'إدارة المستخدمين، الأدوار، وسجلات الدخول',
        },
        {
          id: 'ARCHITECTURE',
          label: 'معمارية C# WinForms [F10]',
          shortcut: 'F10',
          icon: <FileCode2 className="w-3.5 h-3.5 text-purple-300" />,
          titleAttr: 'مخطط معمارية C# WinForms N-Layer',
        },
      ],
    },
  ], []);

  const activePhase = phases.find((p) => p.id === activePhaseId) || phases[0];

  const handleNextPhase = () => {
    const nextId = activePhaseId < phases.length ? activePhaseId + 1 : 1;
    setManualPhaseId(nextId);
    const targetPhase = phases.find((p) => p.id === nextId);
    if (targetPhase) {
      const primaryTab = targetPhase.tabs.find((t) => t.isPrimary)?.id || targetPhase.tabs[0].id;
      onSelectTab(primaryTab);
    }
  };

  const handlePrevPhase = () => {
    const prevId = activePhaseId > 1 ? activePhaseId - 1 : phases.length;
    setManualPhaseId(prevId);
    const targetPhase = phases.find((p) => p.id === prevId);
    if (targetPhase) {
      const primaryTab = targetPhase.tabs.find((t) => t.isPrimary)?.id || targetPhase.tabs[0].id;
      onSelectTab(primaryTab);
    }
  };

  return (
    <nav className="bg-white border-b border-slate-200 shadow-xs select-none flex flex-col">
      {/* 1. WinForms MenuStrip Component (Top Menu with Alt+Keys, Submenus & Arrow Navigation) */}
      <WinFormsMenuStrip
        onSelectTab={(tab) => {
          setManualPhaseId(null);
          onSelectTab(tab);
        }}
        onNewBill={onNewPOSBill}
        onHoldBill={onHoldBill}
        onOpenHeldOrders={onOpenHeldOrders}
        onOpenCalculator={onOpenCalculator}
        onOpenArchitecture={onOpenArchitecture}
        onPrintReport={onPrintReport}
        onOpenLogin={onOpenLogin}
        onLockScreen={onLockScreen}
        onOpenReportItem={onOpenReportItem}
        onOpenBackupRestore={onOpenBackupRestore}
        heldOrdersCount={heldOrdersCount}
      />

      {/* 2. Stages Header Navigation Strip (شريط مراحل دورة العمل المحاسبي الخمس) */}
      <div className="flex flex-wrap items-center justify-between px-3 py-1.5 bg-slate-100 border-b border-slate-200 text-xs shadow-xs">
        {/* Left: Stages Pills */}
        <div className="flex items-center gap-1.5 overflow-x-auto py-0.5 no-scrollbar">
          {/* Main Dashboard Home Button */}
          <button
            id="tab-btn-dashboard"
            onClick={() => {
              setManualPhaseId(null);
              onSelectTab('DASHBOARD');
            }}
            className={`flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-bold transition-all cursor-pointer border ${
              activeTab === 'DASHBOARD'
                ? 'bg-amber-500 text-white shadow-xs border-amber-600 ring-2 ring-amber-300/50'
                : 'bg-white text-slate-700 hover:bg-slate-50 hover:text-slate-900 border-slate-200 shadow-xs'
            }`}
            title="شاشة المحاسب الذكي الرئيسية [F2]"
          >
            <Store className="w-3.5 h-3.5 text-amber-600" />
            <span>الرئيسية [F2]</span>
          </button>

          <div className="h-5 w-[1px] bg-slate-300 mx-1" />

          {/* Mode Switcher: Phased Stages vs All Tabs */}
          {phases.map((phase) => {
            const isPhaseActive = viewMode === 'PHASED' && activePhaseId === phase.id;
            return (
              <button
                key={phase.id}
                id={`stage-btn-${phase.id}`}
                onClick={() => {
                  setViewMode('PHASED');
                  setManualPhaseId(phase.id);
                  const isCurrentInPhase = phase.tabs.some(t => t.id === activeTab);
                  if (!isCurrentInPhase) {
                    const primaryTab = phase.tabs.find(t => t.isPrimary)?.id || phase.tabs[0].id;
                    onSelectTab(primaryTab);
                  }
                }}
                className={`flex items-center gap-2 px-3 py-1.5 rounded-lg text-xs font-bold transition-all cursor-pointer border ${
                  isPhaseActive
                    ? 'bg-white text-sky-900 shadow-sm border-sky-400 ring-2 ring-sky-500/20'
                    : 'bg-slate-200/70 text-slate-700 hover:bg-white hover:text-slate-900 border-slate-300/70'
                }`}
                title={`المرحلة ${phase.number}: ${phase.name}`}
              >
                <span className={`w-4 h-4 rounded-full flex items-center justify-center text-[10px] font-black ${
                  isPhaseActive ? 'bg-sky-600 text-white' : 'bg-slate-300 text-slate-700'
                }`}>
                  {phase.number}
                </span>
                <span>{phase.shortName}</span>
              </button>
            );
          })}

          <div className="h-5 w-[1px] bg-slate-300 mx-1" />

          {/* Toggle All Tabs View Button */}
          <button
            id="btn-all-tabs-view"
            onClick={() => setViewMode(viewMode === 'ALL' ? 'PHASED' : 'ALL')}
            className={`flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-semibold transition-all cursor-pointer border ${
              viewMode === 'ALL'
                ? 'bg-emerald-600 text-white shadow-xs border-emerald-700 ring-2 ring-emerald-300/40'
                : 'bg-white text-slate-700 hover:bg-slate-50 hover:text-slate-900 border-slate-200 shadow-xs'
            }`}
            title="عرض جميع التبويبات في شريط واحد مدمج"
          >
            <LayoutGrid className="w-3.5 h-3.5 text-emerald-600" />
            <span>عرض الكل</span>
          </button>
        </div>

        {/* Right: Stage Stepper & Direct Action Navigation */}
        <div className="flex items-center gap-2">
          {viewMode === 'PHASED' && (
            <div className="flex items-center gap-1.5 text-[11px] text-slate-700 bg-white px-2.5 py-1 rounded-lg border border-slate-200 shadow-xs font-medium">
              <span>مرحلة {activePhase.number} من 5</span>
              <button
                onClick={handlePrevPhase}
                className="p-0.5 hover:bg-slate-100 rounded text-slate-700 cursor-pointer"
                title="المرحلة السابقة"
              >
                <ChevronRight className="w-3.5 h-3.5" />
              </button>
              <button
                onClick={handleNextPhase}
                className="p-0.5 hover:bg-slate-100 rounded text-slate-700 cursor-pointer"
                title="المرحلة التالية"
              >
                <ChevronLeft className="w-3.5 h-3.5" />
              </button>
            </div>
          )}
        </div>
      </div>

      {/* 3. WinForms ToolStrip Component (Active Stage Buttons & Quick Tools) */}
      <div className="flex flex-wrap items-center justify-between px-3 py-1.5 bg-white border-b border-slate-200 text-xs shadow-xs">
        {/* ToolStrip Item Buttons for Active Phase OR All Tabs */}
        <div className="flex items-center gap-1.5 overflow-x-auto py-0.5">
          {viewMode === 'PHASED' ? (
            // Render the buttons belonging to the ACTIVE PHASE
            activePhase.tabs.map((tab) => {
              const isSelected = activeTab === tab.id || (
                tab.id === 'SALES_STRATEGIES_HUB' && [
                  'SALES_STRATEGIES_HUB',
                  'SALES_STRATEGY_INLINE_GRID',
                  'SALES_STRATEGY_RAPID_BAR',
                  'SALES_STRATEGY_SCANNER',
                  'SALES_STRATEGY_SMART_F3',
                  'SALES_STRATEGY_TOUCH_GRID',
                  'SALES_STRATEGY_IMPORT_CONVERT',
                ].includes(activeTab)
              );

              return (
                <button
                  key={tab.id}
                  id={`tab-btn-${tab.id.toLowerCase().replace(/_/g, '-')}`}
                  onClick={() => {
                    if (tab.id === 'OTHER_REPORTS' && onOpenReportItem) {
                      onOpenReportItem('');
                    }
                    if (tab.id === 'ARCHITECTURE') {
                      onOpenArchitecture();
                      return;
                    }
                    onSelectTab(tab.id);
                  }}
                  className={`flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-bold transition-all cursor-pointer whitespace-nowrap border ${
                    isSelected
                      ? tab.isLab
                        ? 'bg-amber-500 text-white shadow-xs border-amber-600 ring-2 ring-amber-300/40'
                        : tab.id === 'OTHER_REPORTS'
                        ? 'bg-emerald-600 text-white shadow-xs border-emerald-700 ring-2 ring-emerald-300/40'
                        : 'bg-sky-600 text-white shadow-xs border-sky-700 ring-2 ring-sky-300/40'
                      : tab.isLab
                      ? 'bg-amber-50 text-amber-800 hover:bg-amber-100 border-amber-200'
                      : 'bg-slate-50 text-slate-700 hover:bg-slate-100 hover:text-slate-900 border-slate-200'
                  }`}
                  title={tab.titleAttr || tab.label}
                >
                  {tab.icon}
                  <span>{tab.label}</span>
                  {tab.badge && (
                    <span className="bg-amber-400 text-slate-900 text-[10px] px-1.5 py-0.2 rounded-full font-black">
                      {tab.badge}
                    </span>
                  )}
                </button>
              );
            })
          ) : (
            // Render ALL TABS with grouped stage badges
            phases.map((phase, idx) => (
              <React.Fragment key={phase.id}>
                {idx > 0 && <div className="h-5 w-[1px] bg-slate-200 mx-1" />}
                {phase.tabs.filter((t) => t.isPrimary || t.isLab).map((tab) => {
                  const isSelected = activeTab === tab.id || (
                    tab.id === 'SALES_STRATEGIES_HUB' && [
                      'SALES_STRATEGIES_HUB',
                      'SALES_STRATEGY_INLINE_GRID',
                      'SALES_STRATEGY_RAPID_BAR',
                      'SALES_STRATEGY_SCANNER',
                      'SALES_STRATEGY_SMART_F3',
                      'SALES_STRATEGY_TOUCH_GRID',
                      'SALES_STRATEGY_IMPORT_CONVERT',
                    ].includes(activeTab)
                  );

                  return (
                    <button
                      key={tab.id}
                      onClick={() => {
                        if (tab.id === 'OTHER_REPORTS' && onOpenReportItem) {
                          onOpenReportItem('');
                        }
                        if (tab.id === 'ARCHITECTURE') {
                          onOpenArchitecture();
                          return;
                        }
                        onSelectTab(tab.id);
                      }}
                      className={`flex items-center gap-1.5 px-2.5 py-1.5 rounded-lg text-xs font-semibold transition-all cursor-pointer whitespace-nowrap border ${
                        isSelected
                          ? 'bg-sky-600 text-white shadow-xs border-sky-700 font-bold'
                          : 'bg-slate-50 text-slate-700 hover:bg-slate-100 hover:text-slate-900 border-slate-200'
                      }`}
                      title={tab.titleAttr || tab.label}
                    >
                      {tab.icon}
                      <span>{tab.label}</span>
                    </button>
                  );
                })}
              </React.Fragment>
            ))
          )}
        </div>

        {/* Global Action Tools (Right / Left Side in RTL) */}
        <div className="flex items-center gap-2 shrink-0">
          <div className="h-5 w-[1px] bg-slate-200 mx-1" />

          {/* Quick New Bill Button */}
          <button
            id="btn-quick-new-bill"
            onClick={onNewPOSBill}
            className="flex items-center gap-1 bg-emerald-600 hover:bg-emerald-700 text-white text-xs px-3 py-1.5 rounded-lg font-bold transition shadow-xs cursor-pointer"
            title="فاتورة مبيعات جديدة [F1]"
          >
            <PlusCircle className="w-3.5 h-3.5" />
            <span className="hidden sm:inline">فاتورة جديدة</span>
          </button>

          {/* Quick Held Bills Button */}
          <button
            id="btn-quick-held-bills"
            onClick={onOpenHeldOrders}
            className="flex items-center gap-1 bg-amber-600 hover:bg-amber-700 text-white text-xs px-3 py-1.5 rounded-lg font-bold transition shadow-xs relative cursor-pointer"
            title="استرجاع الفواتير المعلقة [F4]"
          >
            <PauseCircle className="w-3.5 h-3.5" />
            <span className="hidden sm:inline">المعلقات</span>
            {heldOrdersCount > 0 && (
              <span className="bg-rose-600 text-white text-[10px] px-1.5 py-0.2 rounded-full font-bold">
                {heldOrdersCount}
              </span>
            )}
          </button>

          {/* Quick Calculator Button */}
          <button
            id="btn-quick-calc"
            onClick={onOpenCalculator}
            className="flex items-center gap-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 text-xs px-2.5 py-1.5 rounded-lg font-medium transition cursor-pointer border border-slate-200"
            title="الآلة الحاسبة السريعة [F9]"
          >
            <Calculator className="w-3.5 h-3.5 text-amber-600" />
            <span className="hidden md:inline">الحاسبة [F9]</span>
          </button>

          {/* Quick SQL Database Backup Button */}
          <button
            id="btn-quick-db-backup"
            onClick={onOpenBackupRestore}
            className="flex items-center gap-1.5 bg-slate-100 hover:bg-indigo-50 hover:text-indigo-700 text-slate-700 text-xs px-2.5 py-1.5 rounded-lg font-medium transition cursor-pointer border border-slate-200"
            title="النسخ الاحتياطي لقاعدة بيانات SQL Server واستعادتها"
          >
            <Database className="w-3.5 h-3.5 text-indigo-600" />
            <span className="hidden lg:inline">قاعدة البيانات (SQL)</span>
          </button>

          {/* Quick WinForms ShowDialog & ErrorProvider Showcase Button */}
          {onOpenShowDialogShowcase && (
            <button
              id="btn-quick-showdialog"
              onClick={onOpenShowDialogShowcase}
              className="flex items-center gap-1.5 bg-amber-50 hover:bg-amber-100 text-amber-900 border border-amber-300 text-xs px-2.5 py-1.5 rounded-lg font-bold transition cursor-pointer shadow-2xs"
              title="محاكي رسائل WinForms الحوارية والتركيز الآلي على الخطأ (ShowDialog & ErrorProvider)"
            >
              <Sparkles className="w-3.5 h-3.5 text-amber-600" />
              <span className="hidden xl:inline">نوافذ ShowDialog والخطأ</span>
            </button>
          )}

          {/* Real-time Cash in Drawer Indicator */}
          <div className="flex items-center gap-1.5 bg-emerald-50 text-emerald-800 border border-emerald-300 px-3 py-1 rounded-lg shadow-xs text-xs font-semibold">
            <Coins className="w-3.5 h-3.5 text-emerald-600" />
            <span className="hidden sm:inline">الصندوق:</span>
            <span className="font-mono font-bold text-emerald-900 tracking-wide text-xs">
              {drawerCash.toLocaleString()} {currency}
            </span>
          </div>
        </div>
      </div>
    </nav>
  );
};
