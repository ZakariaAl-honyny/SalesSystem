'use client';

import React, { useState } from 'react';
import {
  Globe,
  LayoutDashboard,
  ShoppingCart,
  Package,
  Users,
  Building2,
  FileSpreadsheet,
  Settings,
  TrendingUp,
  DollarSign,
  ArrowUpRight,
  ArrowDownLeft,
  Search,
  Filter,
  Download,
  Plus,
  CloudCheck,
  ShieldCheck,
  ChevronRight,
  Store,
  Layers,
  FileText,
  RotateCw,
  Eye,
  CheckCircle2,
  AlertTriangle,
  Receipt,
  Smartphone,
  Monitor
} from 'lucide-react';
import {
  ProductItem,
  Customer,
  Supplier,
  Invoice,
  CashVoucher,
  Warehouse,
  SystemUser,
  ShiftInfo,
  AccountNode
} from '@/types/pos';

interface WebPortalViewProps {
  products: ProductItem[];
  customers: Customer[];
  suppliers: Supplier[];
  salesInvoices: Invoice[];
  purchaseInvoices: Invoice[];
  vouchers: CashVoucher[];
  warehouses: Warehouse[];
  chartOfAccounts: AccountNode[];
  currentUser: SystemUser | null;
  currentShift: ShiftInfo;
  currency: string;
  onSwitchToDesktop: () => void;
  onSwitchToMobile: () => void;
  onOpenSalesInvoice: () => void;
}

type WebSection = 'OVERVIEW' | 'SALES' | 'PURCHASES' | 'INVENTORY' | 'FINANCE' | 'BRANCHES';

export const WebPortalView: React.FC<WebPortalViewProps> = ({
  products,
  customers,
  suppliers,
  salesInvoices,
  purchaseInvoices,
  vouchers,
  warehouses,
  chartOfAccounts,
  currentUser,
  currentShift,
  currency,
  onSwitchToDesktop,
  onSwitchToMobile,
  onOpenSalesInvoice,
}) => {
  const [activeSection, setActiveSection] = useState<WebSection>('OVERVIEW');
  const [selectedBranch, setSelectedBranch] = useState<string>('BR-01');
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [isSyncing, setIsSyncing] = useState<boolean>(false);
  const [syncTimestamp, setSyncTimestamp] = useState<string>('منذ 2 ثانية');

  const branches = [
    { id: 'BR-01', name: 'المركز الرئيسي - صنعاء', status: 'ONLINE', warehousesCount: 2, todaySales: 450000 },
    { id: 'BR-02', name: 'فرع حدة التجاري', status: 'ONLINE', warehousesCount: 1, todaySales: 280000 },
    { id: 'BR-03', name: 'فرع تعز / الموزعين', status: 'ONLINE', warehousesCount: 1, todaySales: 165000 },
  ];

  const totalSalesRevenue = salesInvoices.reduce((acc, inv) => acc + inv.grandTotal, 0);
  const totalPurchasesCost = purchaseInvoices.reduce((acc, inv) => acc + inv.grandTotal, 0);
  const totalStockValue = products.reduce((acc, p) => acc + p.stock * p.costPrice, 0);
  const totalCustomerReceivables = customers.reduce((acc, c) => acc + (c.currentBalance > 0 ? c.currentBalance : 0), 0);

  const handleRefreshSync = () => {
    setIsSyncing(true);
    setTimeout(() => {
      setIsSyncing(false);
      setSyncTimestamp('الآن');
    }, 800);
  };

  return (
    <div className="flex-1 flex flex-col bg-slate-100 overflow-hidden font-sans select-none" dir="rtl">
      {/* 1. Cloud Web ERP Top Header (Light SaaS Theme) */}
      <header className="bg-white text-slate-800 px-4 py-2.5 flex flex-wrap items-center justify-between border-b border-slate-200 shrink-0 shadow-2xs">
        <div className="flex items-center gap-3">
          <div className="p-2 bg-sky-600 text-white rounded-xl shadow-xs flex items-center justify-center">
            <Globe className="w-5 h-5" />
          </div>
          <div>
            <div className="flex items-center gap-2">
              <h1 className="font-bold text-sm text-slate-900">منظومة المحاسب السحابية (Cloud Web ERP Portal)</h1>
              <span className="text-[10px] bg-sky-100 text-sky-800 px-2 py-0.5 rounded-full border border-sky-300 font-mono font-bold">
                v2.4 Web Edition
              </span>
            </div>
            <p className="text-[11px] text-slate-500">إدارة مركزية متعددة الفروع والمستودعات متوافقة مع كافة المتصفحات والأجهزة</p>
          </div>
        </div>

        {/* Multi-Branch Selector & Cloud Sync Status */}
        <div className="flex items-center gap-3">
          {/* Branch Dropdown */}
          <div className="flex items-center gap-1.5 bg-slate-100 px-3 py-1.5 rounded-xl border border-slate-300 text-xs">
            <Store className="w-4 h-4 text-sky-600" />
            <span className="text-slate-600 font-medium">الفرع المالي:</span>
            <select
              value={selectedBranch}
              onChange={(e) => setSelectedBranch(e.target.value)}
              className="bg-transparent text-slate-900 font-bold outline-hidden cursor-pointer"
            >
              {branches.map((b) => (
                <option key={b.id} value={b.id} className="bg-white text-slate-900">
                  {b.name}
                </option>
              ))}
            </select>
          </div>

          {/* Sync status indicator */}
          <button
            onClick={handleRefreshSync}
            className="flex items-center gap-1.5 bg-emerald-50 text-emerald-800 border border-emerald-300 px-2.5 py-1.5 rounded-xl text-xs hover:bg-emerald-100 transition cursor-pointer"
            title="انقر لتحديث المزامنة السحابية اللحظية"
          >
            <RotateCw className={`w-3.5 h-3.5 text-emerald-600 ${isSyncing ? 'animate-spin' : ''}`} />
            <span className="font-bold">سحابة نشطة ({syncTimestamp})</span>
          </button>

          {/* Platform Switcher Buttons */}
          <div className="flex items-center gap-1.5 border-r border-slate-200 pr-3">
            <button
              onClick={onSwitchToDesktop}
              className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-3 py-1.5 rounded-xl text-xs font-bold transition flex items-center gap-1.5 cursor-pointer border border-slate-300"
              title="التبديل إلى بيئة سطح المكتب"
            >
              <Monitor className="w-3.5 h-3.5 text-sky-400" />
              <span>🖥️ سطح المكتب</span>
            </button>
            <button
              onClick={onSwitchToMobile}
              className="bg-slate-800 hover:bg-slate-700 text-slate-200 px-3 py-1.5 rounded-xl text-xs font-bold transition flex items-center gap-1.5 cursor-pointer border border-slate-700"
              title="التبديل إلى بيئة تطبيق الموبايل والأجهزة المحمولة"
            >
              <Smartphone className="w-3.5 h-3.5 text-emerald-400" />
              <span>📱 تطبيق الموبايل</span>
            </button>
          </div>
        </div>
      </header>

      {/* 2. Web Portal Main Body Layout (Sidebar + Content) */}
      <div className="flex-1 flex overflow-hidden">
        {/* Sleek Web Left/Right Sidebar */}
        <aside className="w-60 bg-white border-l border-slate-200 flex flex-col justify-between shrink-0 shadow-xs select-none">
          <div className="p-3 space-y-1">
            <div className="px-3 py-2 text-[11px] font-bold text-slate-400 uppercase tracking-wider">
              لوحة التحكم والإدارة
            </div>

            <button
              onClick={() => setActiveSection('OVERVIEW')}
              className={`w-full flex items-center gap-2.5 px-3 py-2 rounded-xl text-xs font-bold transition cursor-pointer ${
                activeSection === 'OVERVIEW'
                  ? 'bg-sky-50 text-sky-800 border border-sky-200 shadow-2xs'
                  : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900'
              }`}
            >
              <LayoutDashboard className="w-4 h-4 text-sky-600" />
              <span>نظرة عامة والمؤشرات</span>
            </button>

            <button
              onClick={() => setActiveSection('SALES')}
              className={`w-full flex items-center gap-2.5 px-3 py-2 rounded-xl text-xs font-bold transition cursor-pointer ${
                activeSection === 'SALES'
                  ? 'bg-sky-50 text-sky-800 border border-sky-200 shadow-2xs'
                  : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900'
              }`}
            >
              <ShoppingCart className="w-4 h-4 text-emerald-600" />
              <span>فواتير ومبيعات الفروع</span>
            </button>

            <button
              onClick={() => setActiveSection('PURCHASES')}
              className={`w-full flex items-center gap-2.5 px-3 py-2 rounded-xl text-xs font-bold transition cursor-pointer ${
                activeSection === 'PURCHASES'
                  ? 'bg-sky-50 text-sky-800 border border-sky-200 shadow-2xs'
                  : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900'
              }`}
            >
              <FileSpreadsheet className="w-4 h-4 text-indigo-600" />
              <span>المشتريات والتوريد</span>
            </button>

            <button
              onClick={() => setActiveSection('INVENTORY')}
              className={`w-full flex items-center gap-2.5 px-3 py-2 rounded-xl text-xs font-bold transition cursor-pointer ${
                activeSection === 'INVENTORY'
                  ? 'bg-sky-50 text-sky-800 border border-sky-200 shadow-2xs'
                  : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900'
              }`}
            >
              <Package className="w-4 h-4 text-amber-600" />
              <span>المخزون والمستودعات</span>
            </button>

            <button
              onClick={() => setActiveSection('FINANCE')}
              className={`w-full flex items-center gap-2.5 px-3 py-2 rounded-xl text-xs font-bold transition cursor-pointer ${
                activeSection === 'FINANCE'
                  ? 'bg-sky-50 text-sky-800 border border-sky-200 shadow-2xs'
                  : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900'
              }`}
            >
              <DollarSign className="w-4 h-4 text-emerald-700" />
              <span>المالية ودفتر الأستاذ</span>
            </button>

            <button
              onClick={() => setActiveSection('BRANCHES')}
              className={`w-full flex items-center gap-2.5 px-3 py-2 rounded-xl text-xs font-bold transition cursor-pointer ${
                activeSection === 'BRANCHES'
                  ? 'bg-sky-50 text-sky-800 border border-sky-200 shadow-2xs'
                  : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900'
              }`}
            >
              <Building2 className="w-4 h-4 text-purple-600" />
              <span>إدارة وتكامل الفروع</span>
            </button>
          </div>

          {/* User badge at bottom of sidebar */}
          <div className="p-3 border-t border-slate-200 bg-slate-50">
            <div className="flex items-center gap-2">
              <div className="w-8 h-8 rounded-full bg-sky-700 text-white flex items-center justify-center font-bold text-xs shadow-xs">
                {currentUser?.fullName?.slice(0, 1) || 'A'}
              </div>
              <div className="flex-1 min-w-0">
                <span className="font-bold text-slate-900 text-xs truncate block">{currentUser?.fullName || 'مدير النظام'}</span>
                <span className="text-[10px] text-slate-500 font-mono block">الدور: {currentUser?.roleName || 'مدير عام'}</span>
              </div>
            </div>
          </div>
        </aside>

        {/* Web Main Content Canvas */}
        <main className="flex-1 overflow-y-auto p-4 space-y-4">
          {/* SECTION 1: OVERVIEW & DASHBOARD */}
          {activeSection === 'OVERVIEW' && (
            <div className="space-y-4">
              {/* Executive KPI Metric Cards */}
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-3.5">
                {/* 1. Total Sales */}
                <div className="bg-white p-4 rounded-2xl border border-slate-200 shadow-2xs space-y-2">
                  <div className="flex items-center justify-between">
                    <span className="text-xs font-bold text-slate-500">إجمالي المبيعات المحققة</span>
                    <div className="p-2 bg-emerald-50 text-emerald-700 rounded-xl">
                      <TrendingUp className="w-4 h-4" />
                    </div>
                  </div>
                  <div className="font-mono text-xl font-black text-slate-900">
                    {totalSalesRevenue.toLocaleString()}{' '}
                    <span className="text-xs font-normal text-slate-500 font-sans">{currency}</span>
                  </div>
                  <div className="flex items-center gap-1 text-[11px] text-emerald-700 font-bold">
                    <ArrowUpRight className="w-3.5 h-3.5" />
                    <span>+14.2% نمو مقارنة بالفترة السابقة</span>
                  </div>
                </div>

                {/* 2. Total Purchases */}
                <div className="bg-white p-4 rounded-2xl border border-slate-200 shadow-2xs space-y-2">
                  <div className="flex items-center justify-between">
                    <span className="text-xs font-bold text-slate-500">إجمالي المشتريات والتوريدات</span>
                    <div className="p-2 bg-indigo-50 text-indigo-700 rounded-xl">
                      <ShoppingCart className="w-4 h-4" />
                    </div>
                  </div>
                  <div className="font-mono text-xl font-black text-slate-900">
                    {totalPurchasesCost.toLocaleString()}{' '}
                    <span className="text-xs font-normal text-slate-500 font-sans">{currency}</span>
                  </div>
                  <div className="text-[11px] text-slate-500">
                    عدد الفواتير المعتمدة: <b>{purchaseInvoices.length} فاتورة</b>
                  </div>
                </div>

                {/* 3. Inventory Stock Valuation */}
                <div className="bg-white p-4 rounded-2xl border border-slate-200 shadow-2xs space-y-2">
                  <div className="flex items-center justify-between">
                    <span className="text-xs font-bold text-slate-500">تقييم المخزون بالتكلفة (FIFO)</span>
                    <div className="p-2 bg-amber-50 text-amber-700 rounded-xl">
                      <Package className="w-4 h-4" />
                    </div>
                  </div>
                  <div className="font-mono text-xl font-black text-slate-900">
                    {totalStockValue.toLocaleString()}{' '}
                    <span className="text-xs font-normal text-slate-500 font-sans">{currency}</span>
                  </div>
                  <div className="text-[11px] text-slate-500">
                    إجمالي الأصناف المعرفة: <b>{products.length} صنف</b>
                  </div>
                </div>

                {/* 4. Customer Receivables */}
                <div className="bg-white p-4 rounded-2xl border border-slate-200 shadow-2xs space-y-2">
                  <div className="flex items-center justify-between">
                    <span className="text-xs font-bold text-slate-500">أرصدة العملاء المدينة (الديون)</span>
                    <div className="p-2 bg-rose-50 text-rose-700 rounded-xl">
                      <DollarSign className="w-4 h-4" />
                    </div>
                  </div>
                  <div className="font-mono text-xl font-black text-rose-700">
                    {totalCustomerReceivables.toLocaleString()}{' '}
                    <span className="text-xs font-normal text-slate-500 font-sans">{currency}</span>
                  </div>
                  <div className="text-[11px] text-slate-500">
                    عدد العملاء المدينين: <b>{customers.filter((c) => c.currentBalance > 0).length} عملاء</b>
                  </div>
                </div>
              </div>

              {/* Branch Network Performance Table */}
              <div className="bg-white rounded-2xl border border-slate-200 p-4 shadow-2xs space-y-3">
                <div className="flex items-center justify-between">
                  <div className="flex items-center gap-2">
                    <Building2 className="w-4 h-4 text-sky-600" />
                    <h3 className="font-bold text-slate-900 text-sm">مؤشرات أداء شبكة الفروع والمستودعات السحابية</h3>
                  </div>
                  <span className="text-xs text-slate-500">محدث لحظياً عبر تقنية WebSockets</span>
                </div>

                <div className="overflow-x-auto">
                  <table className="w-full text-right text-xs border-collapse">
                    <thead>
                      <tr className="bg-slate-50 text-slate-700 border-y border-slate-200">
                        <th className="py-2.5 px-3 font-bold">اسم الفرع</th>
                        <th className="py-2.5 px-3 font-bold text-center">حالة الاتصال</th>
                        <th className="py-2.5 px-3 font-bold text-center">المستودعات</th>
                        <th className="py-2.5 px-3 font-bold text-center">مبيعات اليوم</th>
                        <th className="py-2.5 px-3 font-bold text-center">الإجراء</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-slate-100">
                      {branches.map((b) => (
                        <tr key={b.id} className="hover:bg-slate-50/80 transition">
                          <td className="py-3 px-3 font-bold text-slate-900 flex items-center gap-2">
                            <Store className="w-4 h-4 text-slate-400" />
                            <span>{b.name}</span>
                          </td>
                          <td className="py-3 px-3 text-center">
                            <span className="inline-flex items-center gap-1 text-[11px] bg-emerald-50 text-emerald-700 border border-emerald-200 px-2 py-0.5 rounded-full font-bold">
                              <span className="w-1.5 h-1.5 rounded-full bg-emerald-500 animate-pulse"></span>
                              متصل بالسحابة
                            </span>
                          </td>
                          <td className="py-3 px-3 text-center font-mono text-slate-700">{b.warehousesCount} مستودع</td>
                          <td className="py-3 px-3 text-center font-mono font-bold text-emerald-700">
                            {b.todaySales.toLocaleString()} {currency}
                          </td>
                          <td className="py-3 px-3 text-center">
                            <button
                              onClick={() => {
                                setSelectedBranch(b.id);
                                alert(`تم التبديل لمراقبة بيانات: ${b.name}`);
                              }}
                              className="text-[11px] font-bold text-sky-700 hover:text-sky-900 bg-sky-50 px-2.5 py-1 rounded-lg border border-sky-200 cursor-pointer"
                            >
                              عرض التقارير
                            </button>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          )}

          {/* SECTION 2: SALES INVOICES WEB GRID */}
          {activeSection === 'SALES' && (
            <div className="bg-white rounded-2xl border border-slate-200 p-4 shadow-2xs space-y-3">
              <div className="flex flex-wrap items-center justify-between gap-2">
                <div className="flex items-center gap-2">
                  <ShoppingCart className="w-4 h-4 text-emerald-600" />
                  <h3 className="font-bold text-slate-900 text-sm">سجل فواتير المبيعات السحابية (Web Invoices Grid)</h3>
                  <span className="text-xs bg-slate-100 text-slate-700 px-2 py-0.5 rounded-full font-mono font-bold">
                    {salesInvoices.length} فاتورة
                  </span>
                </div>

                <div className="flex items-center gap-2">
                  <div className="relative">
                    <Search className="w-4 h-4 absolute right-2.5 top-2 text-slate-400" />
                    <input
                      type="text"
                      placeholder="بحث برقم الفاتورة أو العميل..."
                      value={searchQuery}
                      onChange={(e) => setSearchQuery(e.target.value)}
                      className="bg-slate-50 border border-slate-300 rounded-lg pr-8 pl-3 py-1 text-xs text-slate-800 placeholder-slate-400 focus:outline-hidden"
                    />
                  </div>

                  <button
                    onClick={onOpenSalesInvoice}
                    className="bg-emerald-600 hover:bg-emerald-500 text-white px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 shadow-xs cursor-pointer"
                  >
                    <Plus className="w-3.5 h-3.5" />
                    <span>إصدار فاتورة جديدة</span>
                  </button>
                </div>
              </div>

              <div className="overflow-x-auto">
                <table className="w-full text-right text-xs border-collapse">
                  <thead>
                    <tr className="bg-slate-50 text-slate-700 border-y border-slate-200">
                      <th className="py-2.5 px-3 font-bold">رقم الفاتورة</th>
                      <th className="py-2.5 px-3 font-bold">التاريخ والوقت</th>
                      <th className="py-2.5 px-3 font-bold">اسم العميل</th>
                      <th className="py-2.5 px-3 font-bold text-center">طريقة الدفع</th>
                      <th className="py-2.5 px-3 font-bold text-center">الإجمالي الصافي</th>
                      <th className="py-2.5 px-3 font-bold text-center">الكاشير / المندوب</th>
                      <th className="py-2.5 px-3 font-bold text-center">الحالة</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {salesInvoices
                      .filter(
                        (inv) =>
                          inv.invoiceNo.toString().includes(searchQuery) ||
                          inv.customerOrSupplierName.toLowerCase().includes(searchQuery.toLowerCase())
                      )
                      .map((inv) => (
                        <tr key={inv.id} className="hover:bg-slate-50/80 transition">
                          <td className="py-2.5 px-3 font-mono font-bold text-sky-800">#{inv.invoiceNo}</td>
                          <td className="py-2.5 px-3 font-mono text-slate-600">
                            {inv.date} {inv.time}
                          </td>
                          <td className="py-2.5 px-3 font-bold text-slate-900">{inv.customerOrSupplierName}</td>
                          <td className="py-2.5 px-3 text-center">
                            <span
                              className={`text-[10px] px-2 py-0.5 rounded-full font-bold border ${
                                inv.paymentType === 'CASH'
                                  ? 'bg-emerald-50 text-emerald-800 border-emerald-200'
                                  : inv.paymentType === 'CREDIT'
                                  ? 'bg-amber-50 text-amber-800 border-amber-200'
                                  : 'bg-sky-50 text-sky-800 border-sky-200'
                              }`}
                            >
                              {inv.paymentType === 'CASH' ? 'نقدي' : inv.paymentType === 'CREDIT' ? 'آجل' : 'شبكة/كارت'}
                            </span>
                          </td>
                          <td className="py-2.5 px-3 text-center font-mono font-bold text-emerald-800">
                            {inv.grandTotal.toLocaleString()} {currency}
                          </td>
                          <td className="py-2.5 px-3 text-center text-slate-700">{inv.cashierName}</td>
                          <td className="py-2.5 px-3 text-center">
                            <span className="text-[10px] bg-slate-100 text-slate-700 px-2 py-0.5 rounded font-bold">
                              مرحلة محاسبياً
                            </span>
                          </td>
                        </tr>
                      ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* SECTION 3: INVENTORY WEB LEDGER */}
          {activeSection === 'INVENTORY' && (
            <div className="bg-white rounded-2xl border border-slate-200 p-4 shadow-2xs space-y-3">
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <Package className="w-4 h-4 text-amber-600" />
                  <h3 className="font-bold text-slate-900 text-sm">أرصدة وتقييم المخزون الموحد عبر الفروع</h3>
                  <span className="text-xs bg-slate-100 text-slate-700 px-2 py-0.5 rounded-full font-mono font-bold">
                    {products.length} صنف
                  </span>
                </div>
              </div>

              <div className="overflow-x-auto">
                <table className="w-full text-right text-xs border-collapse">
                  <thead>
                    <tr className="bg-slate-50 text-slate-700 border-y border-slate-200">
                      <th className="py-2.5 px-3 font-bold">كود الصنف</th>
                      <th className="py-2.5 px-3 font-bold">اسم الصنف</th>
                      <th className="py-2.5 px-3 font-bold">التصنيف</th>
                      <th className="py-2.5 px-3 font-bold text-center">الرصيد الإجمالي</th>
                      <th className="py-2.5 px-3 font-bold text-center">سعر التكلفة</th>
                      <th className="py-2.5 px-3 font-bold text-center">سعر البيع</th>
                      <th className="py-2.5 px-3 font-bold text-center">إجمالي القيمة</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {products.map((p) => (
                      <tr key={p.id} className="hover:bg-slate-50/80 transition">
                        <td className="py-2.5 px-3 font-mono font-bold text-sky-800">{p.code}</td>
                        <td className="py-2.5 px-3 font-bold text-slate-900">{p.name}</td>
                        <td className="py-2.5 px-3 text-slate-500">{p.category}</td>
                        <td className="py-2.5 px-3 text-center font-mono font-bold text-slate-800">
                          {p.stock} {p.unit}
                        </td>
                        <td className="py-2.5 px-3 text-center font-mono text-slate-700">
                          {p.costPrice} {currency}
                        </td>
                        <td className="py-2.5 px-3 text-center font-mono text-emerald-700 font-bold">
                          {p.salePrice} {currency}
                        </td>
                        <td className="py-2.5 px-3 text-center font-mono font-bold text-slate-900">
                          {(p.stock * p.costPrice).toLocaleString()} {currency}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* SECTION 4, 5, 6: FINANCE & BRANCHES QUICK VIEW */}
          {(activeSection === 'PURCHASES' || activeSection === 'FINANCE' || activeSection === 'BRANCHES') && (
            <div className="bg-white rounded-2xl border border-slate-200 p-5 shadow-2xs space-y-4">
              <div className="flex items-center gap-2">
                <ShieldCheck className="w-5 h-5 text-sky-600" />
                <h3 className="font-bold text-slate-900 text-sm">
                  {activeSection === 'PURCHASES'
                    ? 'إدارة المشتريات وأوامر الشراء المركزية'
                    : activeSection === 'FINANCE'
                    ? 'دفتر الأستاذ والقيود المحاسبية الآلية المجمعة'
                    : 'إدارة الفروع وربط الخوادم السحابية'}
                </h3>
              </div>

              <p className="text-xs text-slate-600 leading-relaxed">
                في المنظومة السحابية (Web Cloud Edition)، تتم كافة العمليات الميدانية وعمليات الفروع من خلال قواعد بيانات موزعة مع مزامنة لحظية متقدمة (Event-Driven Cloud Synchronization)، مما يوفر للإدارة العليا مراقبة حية لجميع التدفقات النقدية والمخزنية.
              </p>

              <div className="grid grid-cols-1 sm:grid-cols-3 gap-3 text-xs pt-2">
                <div className="bg-slate-50 p-3 rounded-xl border border-slate-200 space-y-1">
                  <span className="font-bold text-slate-800 block">المزامنة الفورية:</span>
                  <span className="text-slate-500">ترحيل مباشر لسندات القبض وفواتير البيع من الموبايل وسطح المكتب.</span>
                </div>
                <div className="bg-slate-50 p-3 rounded-xl border border-slate-200 space-y-1">
                  <span className="font-bold text-slate-800 block">الصلاحيات المركزية:</span>
                  <span className="text-slate-500">تحكم كامل بصلاحيات المستخدمين والكاشيرات عبر لوحة تحكم واحدة.</span>
                </div>
                <div className="bg-slate-50 p-3 rounded-xl border border-slate-200 space-y-1">
                  <span className="font-bold text-slate-800 block">التقارير التحليلية:</span>
                  <span className="text-slate-500">قوائم دخل مجمعة ومنفصلة لكل فرع مع مقارنات أداء دورية.</span>
                </div>
              </div>
            </div>
          )}
        </main>
      </div>
    </div>
  );
};
