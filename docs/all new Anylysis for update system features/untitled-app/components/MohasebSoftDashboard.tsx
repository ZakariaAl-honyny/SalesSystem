'use client';

import React, { useState, useMemo } from 'react';
import { 
  Menu, 
  Bell, 
  Share2, 
  ChevronDown, 
  ChevronUp, 
  ChevronRight, 
  ChevronLeft,
  Plus, 
  ShoppingCart, 
  ArrowDownCircle, 
  ArrowUpCircle, 
  HandCoins, 
  Receipt, 
  FileSpreadsheet, 
  Package, 
  Layers, 
  DollarSign, 
  BarChart3, 
  Truck, 
  Repeat, 
  Sliders, 
  Warehouse as WarehouseIcon, 
  ClipboardCheck, 
  BookOpen, 
  KeyRound, 
  Coins, 
  TrendingUp, 
  FileText, 
  ShieldCheck, 
  Sparkles,
  Info,
  Youtube,
  Store,
  RefreshCw,
  RotateCcw,
  Monitor,
  UserCheck
} from 'lucide-react';
import { 
  ProductItem, 
  Warehouse, 
  Customer, 
  Supplier, 
  AccountNode, 
  JournalEntry, 
  Invoice, 
  CashVoucher,
  MainTabType 
} from '@/types/pos';

interface MohasebSoftDashboardProps {
  onNavigate: (tab: MainTabType, subView?: string) => void;
  warehouses: Warehouse[];
  currentWarehouseId: string;
  onChangeWarehouse: (id: string) => void;
  productsCount: number;
  customersCount: number;
  salesCount: number;
  onOpenQuickSale: () => void;
  onOpenQuickPurchase: () => void;
  onOpenReceiptVoucher: () => void;
  onOpenPaymentVoucher: () => void;
  onOpenChartOfAccounts: () => void;
  onOpenCashMovement: () => void;
  onOpenOtherReports: () => void;
  onToggleDesktopPOSView?: () => void;
}

export const MohasebSoftDashboard: React.FC<MohasebSoftDashboardProps> = ({
  onNavigate,
  warehouses,
  currentWarehouseId,
  onChangeWarehouse,
  productsCount,
  customersCount,
  salesCount,
  onOpenQuickSale,
  onOpenQuickPurchase,
  onOpenReceiptVoucher,
  onOpenPaymentVoucher,
  onOpenChartOfAccounts,
  onOpenCashMovement,
  onOpenOtherReports,
  onToggleDesktopPOSView,
}) => {
  // Accordion open/close state
  const [openSection, setOpenSection] = useState<string | null>('inventory'); // default open 'عمليات مخزنية' like screenshot 2

  // Multi-currency V2 info modal
  const [showV2CurrencyModal, setShowV2CurrencyModal] = useState<boolean>(false);
  const [showNotificationModal, setShowNotificationModal] = useState<boolean>(false);
  const [showShareModal, setShowShareModal] = useState<boolean>(false);
  const [showSideDrawer, setShowSideDrawer] = useState<boolean>(false);

  // Active warehouse navigation
  const currentWarehouseIndex = warehouses.findIndex(w => w.id === currentWarehouseId);
  const activeWarehouse = warehouses[currentWarehouseIndex] || warehouses[0] || { name: 'المخزن الرئيسي', id: 'WH-01' };

  const handleNextWarehouse = () => {
    if (warehouses.length <= 1) return;
    const nextIdx = (currentWarehouseIndex + 1) % warehouses.length;
    onChangeWarehouse(warehouses[nextIdx].id);
  };

  const handlePrevWarehouse = () => {
    if (warehouses.length <= 1) return;
    const prevIdx = (currentWarehouseIndex - 1 + warehouses.length) % warehouses.length;
    onChangeWarehouse(warehouses[prevIdx].id);
  };

  const toggleSection = (sec: string) => {
    setOpenSection(prev => (prev === sec ? null : sec));
  };

  return (
    <div className="flex-1 flex flex-col h-full bg-white select-none overflow-hidden text-slate-800 font-sans">
      {/* 1. Top Mohaseb Soft Header (Gradient Blue) */}
      <header className="bg-gradient-to-r from-[#0a4b8c] via-[#026cb6] to-[#0288d1] text-white px-4 py-3 shadow-md flex items-center justify-between shrink-0 z-20">
        <div className="flex items-center gap-3">
          <button 
            onClick={() => setShowSideDrawer(true)}
            className="p-1 hover:bg-white/15 rounded transition cursor-pointer text-white"
            title="القائمة الرئيسية"
          >
            <Menu className="w-6 h-6" />
          </button>
          <h1 className="text-xl font-black tracking-wide flex items-center gap-2">
            <span>محاسب سوفت</span>
            <span className="text-[10px] bg-white/20 px-2 py-0.5 rounded-full font-normal tracking-normal">
              نظام إدارة المحاسبة ونقاط البيع
            </span>
          </h1>
        </div>

        <div className="flex items-center gap-2">
          {/* Toggle to Classic Desktop POS view button */}
          {onToggleDesktopPOSView && (
            <button
              onClick={onToggleDesktopPOSView}
              className="bg-white/15 hover:bg-white/25 text-white px-2.5 py-1 rounded text-xs font-bold flex items-center gap-1.5 transition border border-white/20 cursor-pointer"
              title="التبديل إلى واجهة محطة الكاشير الشاملة (POS Desktop)"
            >
              <Monitor className="w-3.5 h-3.5" />
              <span className="hidden sm:inline">محطة الكاشير (POS)</span>
            </button>
          )}

          <button 
            onClick={() => setShowNotificationModal(true)}
            className="p-1.5 hover:bg-white/15 rounded-full transition cursor-pointer relative"
            title="الإشعارات والتنبيهات"
          >
            <Bell className="w-5 h-5" />
            <span className="absolute top-1 right-1 w-2 h-2 bg-amber-400 rounded-full"></span>
          </button>

          <button 
            onClick={() => setShowShareModal(true)}
            className="p-1.5 hover:bg-white/15 rounded-full transition cursor-pointer"
            title="مشاركة النظام والتقارير"
          >
            <Share2 className="w-5 h-5" />
          </button>
        </div>
      </header>

      {/* Main Scrollable Body */}
      <div className="flex-1 overflow-y-auto flex flex-col bg-[#f0f4f9]">
        {/* 2. Top Quick Action Tiles (المبيعات، قبض/صرف، المشتريات، الحسابات) - Exactly as in Screenshot 1 */}
        <div className="bg-white p-4 shadow-sm border-b border-slate-200">
          <div className="max-w-2xl mx-auto grid grid-cols-2 gap-4">
            {/* Tile 1: المبيعات with (+) button */}
            <div className="flex items-center gap-2">
              <div 
                onClick={() => onNavigate('SALES_INVOICE')}
                className="flex-1 bg-white hover:bg-sky-50 border-2 border-slate-200 hover:border-sky-500 rounded-xl p-3 flex flex-col items-center justify-center gap-2 shadow-xs hover:shadow-md transition cursor-pointer group"
              >
                {/* Custom Sales Icon matching screenshot */}
                <div className="relative w-12 h-12 flex items-center justify-center text-sky-800">
                  <div className="w-10 h-10 border-2 border-sky-800 rounded-lg flex items-center justify-center relative">
                    <span className="text-xs font-black text-sky-800">$</span>
                    <div className="absolute -top-1.5 -right-1.5 text-sky-800">
                      <ChevronRight className="w-4 h-4 stroke-[3]" />
                    </div>
                  </div>
                </div>
                <div className="bg-[#0277bd] text-white px-4 py-1 rounded-full text-xs font-bold shadow-xs group-hover:bg-[#01579b] transition">
                  المبيعات
                </div>
              </div>

              {/* Plus Button to open invoice directly */}
              <button
                onClick={onOpenQuickSale}
                className="w-8 h-8 rounded-full bg-slate-100 hover:bg-sky-600 text-slate-700 hover:text-white border border-slate-300 flex items-center justify-center transition shadow-xs cursor-pointer shrink-0"
                title="فاتورة مبيعات جديدة فورياً"
              >
                <Plus className="w-5 h-5 stroke-[2.5]" />
              </button>
            </div>

            {/* Tile 2: قبض/صرف */}
            <div className="flex items-center gap-2">
              <div 
                onClick={onOpenReceiptVoucher}
                className="flex-1 bg-white hover:bg-sky-50 border-2 border-slate-200 hover:border-sky-500 rounded-xl p-3 flex flex-col items-center justify-center gap-2 shadow-xs hover:shadow-md transition cursor-pointer group"
              >
                <div className="w-12 h-12 flex items-center justify-center text-sky-800">
                  <HandCoins className="w-9 h-9" />
                </div>
                <div className="bg-[#0277bd] text-white px-4 py-1 rounded-full text-xs font-bold shadow-xs group-hover:bg-[#01579b] transition">
                  قبض/صرف
                </div>
              </div>
            </div>

            {/* Tile 3: المشتريات with (+) button */}
            <div className="flex items-center gap-2">
              <div 
                onClick={() => onNavigate('PURCHASES')}
                className="flex-1 bg-white hover:bg-sky-50 border-2 border-slate-200 hover:border-sky-500 rounded-xl p-3 flex flex-col items-center justify-center gap-2 shadow-xs hover:shadow-md transition cursor-pointer group"
              >
                <div className="relative w-12 h-12 flex items-center justify-center text-sky-800">
                  <div className="w-10 h-10 border-2 border-sky-800 rounded-lg flex items-center justify-center relative">
                    <span className="text-xs font-black text-sky-800">$</span>
                    <div className="absolute -bottom-1.5 -left-1.5 text-sky-800">
                      <ChevronLeft className="w-4 h-4 stroke-[3]" />
                    </div>
                  </div>
                </div>
                <div className="bg-[#0277bd] text-white px-4 py-1 rounded-full text-xs font-bold shadow-xs group-hover:bg-[#01579b] transition">
                  المشتريات
                </div>
              </div>

              {/* Plus Button to open purchase invoice directly */}
              <button
                onClick={onOpenQuickPurchase}
                className="w-8 h-8 rounded-full bg-slate-100 hover:bg-sky-600 text-slate-700 hover:text-white border border-slate-300 flex items-center justify-center transition shadow-xs cursor-pointer shrink-0"
                title="فاتورة مشتريات جديدة فورياً"
              >
                <Plus className="w-5 h-5 stroke-[2.5]" />
              </button>
            </div>

            {/* Tile 4: الحسابات */}
            <div className="flex items-center gap-2">
              <div 
                onClick={onOpenChartOfAccounts}
                className="flex-1 bg-white hover:bg-sky-50 border-2 border-slate-200 hover:border-sky-500 rounded-xl p-3 flex flex-col items-center justify-center gap-2 shadow-xs hover:shadow-md transition cursor-pointer group"
              >
                <div className="w-12 h-12 flex items-center justify-center text-sky-800">
                  <FileSpreadsheet className="w-9 h-9" />
                </div>
                <div className="bg-[#0277bd] text-white px-4 py-1 rounded-full text-xs font-bold shadow-xs group-hover:bg-[#01579b] transition">
                  الحسابات
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* 3. Accordion Menus (Matching Screenshots 1, 2, 3, 4, 6) */}
        <div className="flex-1 max-w-2xl mx-auto w-full py-2 space-y-1">
          {/* Section 1: عمليات مخزنية (Inventory Operations - Image 2) */}
          <div className="border-b border-sky-800 overflow-hidden">
            <button
              onClick={() => toggleSection('inventory')}
              className="w-full bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-3 flex items-center justify-between font-bold text-sm transition cursor-pointer shadow-xs"
            >
              <span className="flex items-center gap-2">
                <span>عمليات مخزنية</span>
              </span>
              {openSection === 'inventory' ? <ChevronUp className="w-5 h-5" /> : <ChevronDown className="w-5 h-5" />}
            </button>

            {openSection === 'inventory' && (
              <div className="bg-white divide-y divide-slate-100 shadow-inner">
                <button
                  onClick={() => onNavigate('INVENTORY', 'ISSUE')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>صرف مخزني</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('INVENTORY', 'RECEIPT')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>توريد مخزني</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('INVENTORY', 'TRANSFERS')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>تحويل مخزني</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('INVENTORY', 'ADJUSTMENTS')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>تسوية مخزنية</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('INVENTORY', 'WAREHOUSES')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>إضافة مخزن</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('INVENTORY', 'STOCKTAKE')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>جرد مخزني</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
              </div>
            )}
          </div>

          {/* Section 2: قيود وحسابات (Journals & Accounts - Image 3) */}
          <div className="border-b border-sky-800 overflow-hidden">
            <button
              onClick={() => toggleSection('accounts')}
              className="w-full bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-3 flex items-center justify-between font-bold text-sm transition cursor-pointer shadow-xs"
            >
              <span className="flex items-center gap-2">
                <span>قيود وحسابات</span>
              </span>
              {openSection === 'accounts' ? <ChevronUp className="w-5 h-5" /> : <ChevronDown className="w-5 h-5" />}
            </button>

            {openSection === 'accounts' && (
              <div className="bg-white divide-y divide-slate-100 shadow-inner">
                <button
                  onClick={() => onNavigate('ACCOUNTING', 'JOURNAL')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>قيد يومي</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('ACCOUNTING', 'OPENING')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>قيد إفتتاحي</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('MOHASEB_ACCOUNTS')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>دليل الحسابات</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('MOHASEB_ACCOUNTS')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>حركة الصندوق والتصنيفات</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('ACCOUNTING', 'CLOSING')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>إقفال سنوي</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
              </div>
            )}
          </div>

          {/* Section 3: أصناف (Products & Catalog - Screenshot 9) */}
          <div className="border-b border-sky-800 overflow-hidden">
            <button
              onClick={() => toggleSection('items')}
              className="w-full bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-3 flex items-center justify-between font-bold text-sm transition cursor-pointer shadow-xs"
            >
              <span className="flex items-center gap-2">
                <span>أصناف</span>
              </span>
              {openSection === 'items' ? <ChevronUp className="w-5 h-5" /> : <ChevronDown className="w-5 h-5" />}
            </button>

            {openSection === 'items' && (
              <div className="bg-white divide-y divide-slate-100 shadow-inner">
                {/* 1. الأصناف */}
                <button
                  onClick={() => onNavigate('MOHASEB_ITEMS')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between cursor-pointer"
                >
                  <span>الأصناف</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>

                {/* 2. أسعار البيع */}
                <button
                  onClick={() => onNavigate('SALES_PRICES')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between cursor-pointer"
                >
                  <span>أسعار البيع</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>

                {/* 3. وحدات الصنف */}
                <button
                  onClick={() => onNavigate('ITEM_UNITS')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between cursor-pointer"
                >
                  <span>وحدات الصنف</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>

                {/* 4. فاتورة عرض سعر */}
                <button
                  onClick={() => onNavigate('PRICE_QUOTATION')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between cursor-pointer"
                >
                  <span>فاتورة عرض سعر</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>

                {/* 5. طلب شراء */}
                <button
                  onClick={() => onNavigate('PURCHASE_ORDER')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between cursor-pointer"
                >
                  <span>طلب شراء</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>

                {/* 6. إستيراد من إكسل XLS */}
                <button
                  onClick={() => onNavigate('EXCEL_IMPORT')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between cursor-pointer"
                >
                  <span>إستيراد من إكسل XLS</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
              </div>
            )}
          </div>

          {/* Section 4: العملات (Currencies - Deferred to V2.0) */}
          <div className="border-b border-sky-800 overflow-hidden">
            <button
              onClick={() => toggleSection('currencies')}
              className="w-full bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-3 flex items-center justify-between font-bold text-sm transition cursor-pointer shadow-xs"
            >
              <span className="flex items-center gap-2">
                <span>العملات وأسعار الصرف</span>
                <span className="text-[10px] bg-amber-400/25 text-amber-200 border border-amber-300/30 px-1.5 py-0.5 rounded font-medium">
                  مؤجل لـ V2.0
                </span>
              </span>
              {openSection === 'currencies' ? <ChevronUp className="w-5 h-5" /> : <ChevronDown className="w-5 h-5" />}
            </button>

            {openSection === 'currencies' && (
              <div className="bg-white divide-y divide-slate-100 shadow-inner">
                <button
                  onClick={() => onNavigate('CURRENCIES')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <div className="flex items-center gap-2">
                    <span>دليل العملات</span>
                    <span className="text-[10px] bg-slate-100 text-slate-500 px-1.5 py-0.5 rounded font-normal">
                      مؤجل لـ V2.0
                    </span>
                  </div>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('CURRENCY_RATES')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <div className="flex items-center gap-2">
                    <span>أسعار وتاريخ الصرف</span>
                    <span className="text-[10px] bg-slate-100 text-slate-500 px-1.5 py-0.5 rounded font-normal">
                      مؤجل لـ V2.0
                    </span>
                  </div>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('CUSTOMERS')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <div className="flex items-center gap-2">
                    <span>سقف الحساب والائتمان للعملاء</span>
                    <span className="text-[10px] bg-emerald-100 text-emerald-700 px-1.5 py-0.5 rounded font-bold">
                      MVP نشط
                    </span>
                  </div>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
              </div>
            )}
          </div>

          {/* Section 5: التقارير (Reports - Image 6) */}
          <div className="border-b border-sky-800 overflow-hidden">
            <button
              onClick={() => toggleSection('reports')}
              className="w-full bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-3 flex items-center justify-between font-bold text-sm transition cursor-pointer shadow-xs"
            >
              <span className="flex items-center gap-2">
                <span>التقارير</span>
              </span>
              {openSection === 'reports' ? <ChevronUp className="w-5 h-5" /> : <ChevronDown className="w-5 h-5" />}
            </button>

            {openSection === 'reports' && (
              <div className="bg-white divide-y divide-slate-100 shadow-inner">
                <button
                  onClick={() => onNavigate('REPORTS', 'STOCK_MOVEMENT')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>حركة الأصناف</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('REPORTS', 'TRIAL_BALANCE')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>ميزان المراجعة</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('REPORTS', 'INCOME_STATEMENT')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>قائمة الدخل</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('REPORTS', 'BALANCE_SHEET')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between"
                >
                  <span>المركز المالي</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                {/* تقارير أخرى - matching screenshot 5 */}
                <button
                  onClick={onOpenOtherReports}
                  className="w-full text-right px-6 py-3.5 bg-sky-50/70 hover:bg-sky-100 font-black text-[#0277bd] text-xs transition flex items-center justify-between"
                >
                  <span className="flex items-center gap-2">
                    <FileText className="w-4 h-4 text-[#0277bd]" />
                    <span>تقارير أخرى (15 تقريراً تفصيلياً)</span>
                  </span>
                  <ChevronLeft className="w-4 h-4 text-[#0277bd]" />
                </button>
              </div>
            )}
          </div>

          {/* Section 6: الموظفين والكوادر (Employees - V1 Lean) */}
          <div className="border-b border-sky-800 overflow-hidden">
            <button
              onClick={() => toggleSection('employees')}
              className="w-full bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-3 flex items-center justify-between font-bold text-sm transition cursor-pointer shadow-xs"
            >
              <span className="flex items-center gap-2">
                <UserCheck className="w-4 h-4 text-sky-200" />
                <span>الموظفين والكوادر</span>
                <span className="text-[10px] bg-emerald-500/30 text-emerald-200 border border-emerald-400/40 px-1.5 py-0.5 rounded font-bold">
                  V1.0 جديد
                </span>
              </span>
              {openSection === 'employees' ? <ChevronUp className="w-5 h-5" /> : <ChevronDown className="w-5 h-5" />}
            </button>

            {openSection === 'employees' && (
              <div className="bg-white divide-y divide-slate-100 shadow-inner">
                <button
                  onClick={() => onNavigate('EMPLOYEES')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between cursor-pointer"
                >
                  <div className="flex items-center gap-2">
                    <UserCheck className="w-4 h-4 text-sky-600" />
                    <span>دليل وبطاقات الموظفين (V1)</span>
                  </div>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
                <button
                  onClick={() => onNavigate('SECURITY')}
                  className="w-full text-right px-6 py-3 hover:bg-sky-50 font-bold text-slate-700 text-xs transition flex items-center justify-between cursor-pointer"
                >
                  <span>حسابات المستخدمين والصلاحيات (RBAC)</span>
                  <ChevronLeft className="w-4 h-4 text-slate-400" />
                </button>
              </div>
            )}
          </div>
        </div>
      </div>

      {/* 4. Bottom Footer Bar (Matching Screenshot: YouTube on left, < المخزن الرئيسي > in middle, logo on right) */}
      <footer className="bg-gradient-to-r from-[#0a4b8c] via-[#0277bd] to-[#0288d1] text-white px-4 py-2 flex items-center justify-between shadow-lg shrink-0 z-20">
        {/* Left: YouTube circular button */}
        <a
          href="https://www.youtube.com"
          target="_blank"
          rel="noopener noreferrer"
          className="w-9 h-9 rounded-full bg-white/20 hover:bg-white/35 border border-white/40 flex items-center justify-center transition shadow-xs text-white"
          title="شروحات نظام محاسب سوفت على يوتيوب"
        >
          <Youtube className="w-5 h-5 text-red-400 fill-white" />
        </a>

        {/* Center: Warehouse Carousel / Switcher: < المخزن الرئيسي > */}
        <div className="flex items-center gap-2 bg-black/20 px-3 py-1.5 rounded-full border border-white/20 shadow-inner">
          <button
            onClick={handlePrevWarehouse}
            className="p-1 hover:bg-white/20 rounded-full transition cursor-pointer text-white"
            title="المخزن السابق"
          >
            <ChevronRight className="w-4 h-4" />
          </button>
          
          <div className="flex items-center gap-1.5 px-2">
            <WarehouseIcon className="w-3.5 h-3.5 text-sky-200" />
            <span className="text-xs font-bold text-white tracking-wide">
              {activeWarehouse.name}
            </span>
          </div>

          <button
            onClick={handleNextWarehouse}
            className="p-1 hover:bg-white/20 rounded-full transition cursor-pointer text-white"
            title="المخزن التالي"
          >
            <ChevronLeft className="w-4 h-4" />
          </button>
        </div>

        {/* Right: Round Logo / App Switcher */}
        <div 
          onClick={onToggleDesktopPOSView}
          className="w-9 h-9 rounded-full bg-[#0288d1] hover:bg-sky-400 border-2 border-white flex items-center justify-center transition shadow-md cursor-pointer text-white"
          title="محاسب سوفت - التبديل لواجهة الكاشير POS"
        >
          <Repeat className="w-5 h-5" />
        </div>
      </footer>

      {/* Side Drawer Menu */}
      {showSideDrawer && (
        <div className="fixed inset-0 z-50 flex">
          <div 
            onClick={() => setShowSideDrawer(false)} 
            className="fixed inset-0 bg-black/60 backdrop-blur-xs"
          />
          <div className="relative w-80 max-w-[85vw] bg-white text-slate-800 flex flex-col h-full z-10 shadow-2xl animate-in slide-in-from-right">
            {/* Drawer Header */}
            <div className="bg-[#0277bd] text-white p-4 flex items-center justify-between">
              <div>
                <h3 className="text-base font-bold">محاسب سوفت</h3>
                <p className="text-xs text-sky-150">القائمة السريعة للتحكم والعمليات</p>
              </div>
              <button 
                onClick={() => setShowSideDrawer(false)}
                className="text-white hover:bg-white/20 p-1 rounded transition"
              >
                ✕
              </button>
            </div>

            {/* Drawer Items */}
            <div className="flex-1 overflow-y-auto p-3 space-y-1 text-xs">
              <button 
                onClick={() => { setShowSideDrawer(false); onNavigate('SALES_INVOICE'); }}
                className="w-full text-right p-2.5 rounded hover:bg-sky-50 flex items-center gap-2 font-bold"
              >
                <Receipt className="w-4 h-4 text-sky-600" />
                <span>فاتورة مبيعات تجارية [frmSalesInvoice]</span>
              </button>
              <button 
                onClick={() => { setShowSideDrawer(false); onNavigate('PRICE_QUOTATION'); }}
                className="w-full text-right p-2.5 rounded hover:bg-sky-50 flex items-center gap-2 font-bold"
              >
                <FileText className="w-4 h-4 text-amber-600" />
                <span>عروض الأسعار للعملاء [Price Quotations DataGrid]</span>
              </button>
              <button 
                onClick={() => { setShowSideDrawer(false); onNavigate('SALES_RETURNS'); }}
                className="w-full text-right p-2.5 rounded hover:bg-sky-50 flex items-center gap-2 font-bold"
              >
                <RotateCcw className="w-4 h-4 text-rose-600" />
                <span>مردودات المبيعات واسترجاع البضاعة [Sales Returns]</span>
              </button>
              <button 
                onClick={() => { setShowSideDrawer(false); onNavigate('POS'); }}
                className="w-full text-right p-2.5 rounded hover:bg-sky-50 flex items-center gap-2 font-bold"
              >
                <ShoppingCart className="w-4 h-4 text-emerald-600" />
                <span>نقطة البيع السريعة [frmPOS]</span>
              </button>
              <button 
                onClick={() => { setShowSideDrawer(false); onNavigate('PURCHASE_ORDER'); }}
                className="w-full text-right p-2.5 rounded hover:bg-sky-50 flex items-center gap-2 font-bold"
              >
                <FileText className="w-4 h-4 text-emerald-600" />
                <span>أوامر وطلبات الشراء [Purchase Orders DataGrid]</span>
              </button>
              <button 
                onClick={() => { setShowSideDrawer(false); onNavigate('PURCHASES'); }}
                className="w-full text-right p-2.5 rounded hover:bg-sky-50 flex items-center gap-2 font-bold"
              >
                <ArrowDownCircle className="w-4 h-4 text-indigo-600" />
                <span>فواتير المشتريات وتوريد المخزن</span>
              </button>
              <button 
                onClick={() => { setShowSideDrawer(false); onNavigate('CUSTOMERS'); }}
                className="w-full text-right p-2.5 rounded hover:bg-sky-50 flex items-center gap-2 font-bold"
              >
                <HandCoins className="w-4 h-4 text-amber-600" />
                <span>العملاء والحسابات المدينة</span>
              </button>
              <button 
                onClick={() => { setShowSideDrawer(false); onOpenOtherReports(); }}
                className="w-full text-right p-2.5 rounded hover:bg-sky-50 flex items-center gap-2 font-bold"
              >
                <FileText className="w-4 h-4 text-rose-600" />
                <span>تقارير أخرى (15 تقرير)</span>
              </button>
              <button 
                onClick={() => { setShowSideDrawer(false); onNavigate('ARCHITECTURE'); }}
                className="w-full text-right p-2.5 rounded hover:bg-sky-50 flex items-center gap-2 font-bold"
              >
                <Layers className="w-4 h-4 text-slate-600" />
                <span>الهيكل البرمجي (C# N-Layer Architecture)</span>
              </button>
            </div>

            <div className="p-3 bg-slate-50 border-t border-slate-200 text-center text-[11px] text-slate-500">
              نظام محاسب سوفت ERP v2.4
            </div>
          </div>
        </div>
      )}

      {/* Multi-Currency & Files V2 Notice Modal (As requested: "تجاهل تعدد العملات ورفع الصور في اول اصدار ويمكنك ان تجعل الشاشات المستقبلية تعمل عليها قريبا") */}
      {showV2CurrencyModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60">
          <div className="bg-white rounded-xl max-w-md w-full p-5 shadow-2xl border border-slate-200 text-right space-y-3">
            <div className="w-10 h-10 rounded-full bg-sky-100 text-sky-700 flex items-center justify-center mx-auto">
              <Coins className="w-6 h-6" />
            </div>
            <h3 className="text-base font-bold text-slate-800 text-center">
              إدارة العملات وسعر الصرف (جاهزة للإصدار القادم)
            </h3>
            <p className="text-xs text-slate-600 leading-relaxed">
              بناءً على طلبك، تم تثبيت العملة الأساسية الحالية للنظام لتفادي التعقيد في الإصدار الأول. الهيكل المحاسبي وقواعد البيانات أصبحت مهيأة بالكامل لاستقبال شاشات تعدد العملات وفروقات أسعار الصرف ورفع المرفقات في الإصدار القادم القريب.
            </p>
            <div className="bg-sky-50 p-2.5 rounded-lg border border-sky-100 text-[11px] text-sky-900 space-y-1">
              <p className="font-bold">• العملة النشطة حالياً: ريال يمني (YER)</p>
              <p className="font-bold">• العملات المهيأة للإصدار القادم: ريال سعودي (SAR) / دولار أمريكي (USD)</p>
            </div>
            <div className="pt-2 flex justify-end">
              <button
                onClick={() => setShowV2CurrencyModal(false)}
                className="bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-1.5 rounded-lg font-bold text-xs transition cursor-pointer"
              >
                حسناً، فهمت
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Notifications Modal */}
      {showNotificationModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60">
          <div className="bg-white rounded-xl max-w-md w-full p-4 shadow-2xl border border-slate-200 text-right space-y-3">
            <div className="flex items-center justify-between border-b pb-2">
              <h3 className="font-bold text-sm text-slate-800 flex items-center gap-2">
                <Bell className="w-4 h-4 text-sky-600" />
                <span>مركز الإشعارات والتنبيهات</span>
              </h3>
              <button onClick={() => setShowNotificationModal(false)} className="text-slate-400 hover:text-slate-600">✕</button>
            </div>
            <div className="space-y-2 text-xs">
              <div className="p-2.5 rounded bg-emerald-50 border border-emerald-100 flex items-start gap-2">
                <ShieldCheck className="w-4 h-4 text-emerald-600 shrink-0 mt-0.5" />
                <div>
                  <p className="font-bold text-emerald-900">النظام متصل ويعمل بكفاءة</p>
                  <p className="text-[11px] text-emerald-700">تم تهيئة شاشات محاسب سوفت وتوليد القيود الآلية بنجاح.</p>
                </div>
              </div>
              <div className="p-2.5 rounded bg-sky-50 border border-sky-100 flex items-start gap-2">
                <WarehouseIcon className="w-4 h-4 text-sky-600 shrink-0 mt-0.5" />
                <div>
                  <p className="font-bold text-sky-900">المخزن النشط: {activeWarehouse.name}</p>
                  <p className="text-[11px] text-sky-700">يمكنك التبديل بين المستودعات فورياً عبر الشريط السفلي.</p>
                </div>
              </div>
            </div>
            <div className="pt-2 flex justify-end">
              <button
                onClick={() => setShowNotificationModal(false)}
                className="bg-slate-200 hover:bg-slate-300 text-slate-700 px-3 py-1 rounded text-xs font-bold transition"
              >
                إغلاق
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Share Modal */}
      {showShareModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60">
          <div className="bg-white rounded-xl max-w-sm w-full p-4 shadow-2xl border border-slate-200 text-right space-y-3">
            <h3 className="font-bold text-sm text-slate-800 flex items-center gap-2">
              <Share2 className="w-4 h-4 text-sky-600" />
              <span>مشاركة تطبيق محاسب سوفت</span>
            </h3>
            <p className="text-xs text-slate-600">
              يمكن للمستخدمين والمحاسبين فتح هذا النظام مباشرة على الجوال أو الحاسوب اللوحي أو الشاشات المكتبية.
            </p>
            <div className="flex gap-2">
              <input 
                type="text" 
                readOnly 
                value={typeof window !== 'undefined' ? window.location.href : ''} 
                className="flex-1 bg-slate-100 border border-slate-300 rounded px-2 py-1 text-xs text-slate-700 font-mono"
              />
              <button 
                onClick={() => {
                  if (typeof window !== 'undefined') {
                    navigator.clipboard?.writeText(window.location.href);
                    alert('تم نسخ الرابط بنجاح!');
                  }
                }}
                className="bg-[#0277bd] text-white px-3 py-1 rounded text-xs font-bold hover:bg-[#01579b]"
              >
                نسخ
              </button>
            </div>
            <div className="pt-1 flex justify-end">
              <button
                onClick={() => setShowShareModal(false)}
                className="bg-slate-200 text-slate-700 px-3 py-1 rounded text-xs font-bold"
              >
                إغلاق
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
