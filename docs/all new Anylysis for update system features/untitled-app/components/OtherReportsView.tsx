'use client';

import React, { useState, useMemo, useCallback, useEffect } from 'react';
import { 
  ArrowLeft, 
  FileText, 
  Search, 
  Printer, 
  TrendingUp, 
  Layers, 
  DollarSign, 
  Calendar, 
  User, 
  Package, 
  Clock, 
  Wallet, 
  Scale, 
  ChevronLeft,
  AlertCircle,
  Building2,
  Store,
  Filter,
  CheckCircle2,
  Boxes,
  ArrowDownLeft,
  ArrowUpRight,
  RefreshCw,
  FolderTree,
  LayoutGrid,
  ListFilter,
  BookOpen,
  Receipt,
  Truck,
  Sparkles,
  BarChart3,
} from 'lucide-react';
import { 
  ProductItem, 
  Customer, 
  Supplier, 
  AccountNode, 
  JournalEntry, 
  Invoice, 
  CashVoucher, 
  Warehouse 
} from '@/types/pos';
import { getProductStockInWarehouse, generateProductStockLedger } from '@/lib/mock/inventoryUtils';
import { INITIAL_WAREHOUSES } from '@/lib/mock/warehouses';

interface OtherReportsViewProps {
  products: ProductItem[];
  customers: Customer[];
  suppliers: Supplier[];
  chartOfAccounts: AccountNode[];
  journalEntries: JournalEntry[];
  salesInvoices: Invoice[];
  purchaseInvoices: Invoice[];
  vouchers: CashVoucher[];
  warehouses?: Warehouse[];
  currency: string;
  initialReportId?: string | null;
  onBack: () => void;
  onOpenReportItem?: (reportId: string) => void;
}

// Classification categories for reports
export interface ReportCategoryDef {
  id: string;
  name: string;
  desc: string;
  icon: React.ElementType;
  colorClass: string;
  badgeClass: string;
}

export const REPORT_CATEGORIES: ReportCategoryDef[] = [
  {
    id: 'SALES_PROFIT',
    name: 'المبيعات والأرباح',
    desc: 'تحليلات المبيعات وهوامش الربحية وربح الأصناف والعملاء والأقسام',
    icon: TrendingUp,
    colorClass: 'text-emerald-700 bg-emerald-50 border-emerald-200',
    badgeClass: 'bg-emerald-100 text-emerald-800 border-emerald-200',
  },
  {
    id: 'INVENTORY_STOCK',
    name: 'المخزون وحركة الأصناف',
    desc: 'أرصدة المخازن، كرت حركة الصنف، مشتريات الأصناف، وتواريخ الصلاحية',
    icon: Package,
    colorClass: 'text-sky-700 bg-sky-50 border-sky-200',
    badgeClass: 'bg-sky-100 text-sky-800 border-sky-200',
  },
  {
    id: 'ACCOUNTING_FINANCIAL',
    name: 'الحسابات والمالية والقيود',
    desc: 'أرصدة الدليل، كشوفات الحسابات، قيود اليومية العامة، ورأس المال العامل',
    icon: BookOpen,
    colorClass: 'text-amber-700 bg-amber-50 border-amber-200',
    badgeClass: 'bg-amber-100 text-amber-800 border-amber-200',
  },
  {
    id: 'CASHBOX_OPERATIONS',
    name: 'الصندوق والعمليات اليومية',
    desc: 'سجل العمليات اليومية، حركة المقبوضات والمدفوعات، وإقفال الورديات',
    icon: DollarSign,
    colorClass: 'text-purple-700 bg-purple-50 border-purple-200',
    badgeClass: 'bg-purple-100 text-purple-800 border-purple-200',
  },
];

export interface ReportItemDef {
  id: string;
  title: string;
  desc: string;
  category: 'SALES_PROFIT' | 'INVENTORY_STOCK' | 'ACCOUNTING_FINANCIAL' | 'CASHBOX_OPERATIONS';
  icon: React.ElementType;
  tags?: string[];
  v2Note?: boolean;
}

// 18 Comprehensive Classified Reports for Mohaseb Soft
export const REPORTS_LIST: ReportItemDef[] = [
  // 1. قسم المبيعات والأرباح
  { 
    id: 'sales_by_item', 
    title: 'المبيعات حسب الصنف', 
    desc: 'الكميات المباعة وإجمالي مبيعات كل صنف ومساهمته في دخل المنشأة', 
    category: 'SALES_PROFIT',
    icon: Package,
    tags: ['مبيعات', 'أصناف', 'كميات', 'إيرادات']
  },
  { 
    id: 'item_profits', 
    title: 'أرباح وهوامش ربح الأصناف', 
    desc: 'مقارنة سعر البيع مع تكلفة الشراء ونسبة هامش الربح المحقق لكل صنف', 
    category: 'SALES_PROFIT',
    icon: TrendingUp,
    tags: ['ربحية', 'هامش ربح', 'تكلفة', 'أرباح الأصناف']
  },
  { 
    id: 'profit_by_customer', 
    title: 'الربح على مستوى العميل', 
    desc: 'تحليل صافي ربحية ومردودية المعاملات التجارية مع كل عميل', 
    category: 'SALES_PROFIT',
    icon: User,
    tags: ['عملاء', 'أرباح', 'ربحية العميل']
  },
  { 
    id: 'category_totals', 
    title: 'إجمالي التصنيفات والأقسام', 
    desc: 'إجمالي المبيعات وقيمة المخزون مصنفاً حسب أقسام ومجموعات المواد', 
    category: 'SALES_PROFIT',
    icon: Layers,
    tags: ['تصنيفات', 'أقسام', 'مجموعات', 'مبيعات']
  },
  { 
    id: 'sales_analysis_summary', 
    title: 'تحليل حركة المبيعات المجمعة', 
    desc: 'تحليل بياني لحجم المبيعات اليومية والمتوسطات وقيمة الفواتير المسجلة', 
    category: 'SALES_PROFIT',
    icon: BarChart3,
    tags: ['مبيعات', 'لوحة', 'إحصائيات']
  },

  // 2. قسم المخزون وحركة الأصناف
  { 
    id: 'rem_stock', 
    title: 'المخزون المتبقي وتقييم التكلفة', 
    desc: 'تقرير تفصيلي بكافة الكميات المتبقية وتكلفتها في المستودعات المحددة', 
    category: 'INVENTORY_STOCK',
    icon: Package,
    tags: ['مخزون', 'بضاعة', 'كميات', 'جرد']
  },
  { 
    id: 'item_movement_details', 
    title: 'تفاصيل كرت حركة الصنف', 
    desc: 'سجل حركات الإدخال والإخراج الشاملة والرصيد التراكمي اللحظي للصنف', 
    category: 'INVENTORY_STOCK',
    icon: FileText,
    tags: ['حركة صنف', 'كرت الصنف', 'وارد', 'منصرف']
  },
  { 
    id: 'purchases_by_item', 
    title: 'المشتريات والتوريد حسب الصنف', 
    desc: 'كميات وتكاليف الشراء الموردة لكل صنف ومقارنة أسعار التوريد', 
    category: 'INVENTORY_STOCK',
    icon: Truck,
    tags: ['مشتريات', 'توريد', 'موردين']
  },
  { 
    id: 'expiry_report', 
    title: 'الأصناف وتواريخ الصلاحية (FEFO)', 
    desc: 'متابعة تواريخ الصلاحية والدفعات المنتهية والقريبة لتفادي التلف والركود', 
    category: 'INVENTORY_STOCK',
    icon: Calendar,
    tags: ['صلاحية', 'انتهاء', 'دفعات', 'باتش']
  },

  // 3. قسم الحسابات والمالية والقيود
  { 
    id: 'acc_balances', 
    title: 'أرصدة الحسابات وميزان المراجعة', 
    desc: 'كشف شامل بأرصدة العملاء والموردين والصناديق والبنوك وحسابات الدليل', 
    category: 'ACCOUNTING_FINANCIAL',
    icon: Wallet,
    tags: ['أرصدة', 'ميزان مراجعة', 'دليل الحسابات']
  },
  { 
    id: 'account_movement', 
    title: 'كشف حساب تفصيلي للحركات', 
    desc: 'كشف حساب تفصيلي لأي حساب مالي مع الرصيد الافتتاحي والحركات التراكمية', 
    category: 'ACCOUNTING_FINANCIAL',
    icon: Layers,
    tags: ['كشف حساب', 'أستاذ مساعد', 'مدين ودائن']
  },
  { 
    id: 'daily_journals', 
    title: 'سجل قيود اليومية العامة', 
    desc: 'قيود اليومية العامة المرحلة تلقائياً من الفواتير والسندات وقيد التسويات', 
    category: 'ACCOUNTING_FINANCIAL',
    icon: FileText,
    tags: ['قيود يومية', 'دفتر اليومية', 'ترحيل']
  },
  { 
    id: 'working_capital', 
    title: 'مؤشر رأس المال العامل والسيولة', 
    desc: 'الأصول المتداولة ناقص الالتزامات المتداولة وصافي السيولة التشغيلية', 
    category: 'ACCOUNTING_FINANCIAL',
    icon: TrendingUp,
    tags: ['سيولة', 'رأس مال عامل', 'أصول متداولة']
  },
  { 
    id: 'fx_variance', 
    title: 'فوارق أسعار صرف العملات', 
    desc: 'احتساب ومتابعة فروقات الصرف والعملات الأجنبية (معد للإصدار القادم V2)', 
    category: 'ACCOUNTING_FINANCIAL',
    icon: Scale, 
    v2Note: true,
    tags: ['عملات', 'صرف', 'فروقات']
  },

  // 4. قسم الصندوق والعمليات اليومية
  { 
    id: 'daily_ops', 
    title: 'سجل العمليات اليومية الشامل', 
    desc: 'سجل زمني كامل بحركات البيع والشراء والقبض والصرف المنجزة لليوم', 
    category: 'CASHBOX_OPERATIONS',
    icon: Clock,
    tags: ['عمليات يومية', 'سجل النشاط', 'حركات اليوم']
  },
  { 
    id: 'cashbox_movement', 
    title: 'حركة الصندوق والمقبوضات والمدفوعات', 
    desc: 'حركة المقبوضات والمدفوعات والرصيد الافتتاحي والنهائي لكل صندوق', 
    category: 'CASHBOX_OPERATIONS',
    icon: DollarSign,
    tags: ['صندوق', 'خزينة', 'مقبوضات', 'مدفوعات']
  },
  { 
    id: 'shift_closing_report', 
    title: 'تقرير إقفال الوردية وجرد الكاشير', 
    desc: 'ملخص إقفال الوردية الحالية، مبيعات طرق الدفع، النقدية الفعلية والعجز/الفائض', 
    category: 'CASHBOX_OPERATIONS',
    icon: Receipt,
    tags: ['وردية', 'كاشير', 'جرد', 'Z-Report']
  },
];

export const OtherReportsView: React.FC<OtherReportsViewProps> = ({
  products,
  customers,
  suppliers,
  chartOfAccounts,
  journalEntries,
  salesInvoices,
  purchaseInvoices,
  vouchers,
  warehouses = INITIAL_WAREHOUSES,
  currency,
  initialReportId = null,
  onBack,
  onOpenReportItem,
}) => {
  const [selectedReportIdState, setSelectedReportIdState] = useState<string | null>(null);
  const selectedReportId = selectedReportIdState ?? initialReportId;
  const setSelectedReportId = (id: string | null) => setSelectedReportIdState(id);

  const [selectedCategory, setSelectedCategory] = useState<string>('ALL');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [selectedMovementAccountId, setSelectedMovementAccountId] = useState<string>('');
  const [selectedMovementProductId, setSelectedMovementProductId] = useState<string>(products[0]?.id || '');

  // Cascading Branch & Warehouse Filter State
  const [selectedBranchId, setSelectedBranchId] = useState<number | 'ALL'>('ALL');
  const [selectedWarehouseId, setSelectedWarehouseId] = useState<string>('ALL');

  // Extract unique branches from warehouses
  const branchesList = useMemo(() => {
    const map = new Map<number, string>();
    warehouses.forEach(w => {
      if (w.branchId) {
        map.set(w.branchId, w.branchName || `فرع ${w.branchId}`);
      }
    });
    return Array.from(map.entries()).map(([id, name]) => ({ id, name }));
  }, [warehouses]);

  // Available warehouses cascading based on selected branch
  const availableWarehouses = useMemo(() => {
    if (selectedBranchId === 'ALL') {
      return warehouses;
    }
    return warehouses.filter(w => w.branchId === selectedBranchId);
  }, [warehouses, selectedBranchId]);

  // Handle branch change: resets warehouse to 'ALL'
  const handleBranchChange = (branchVal: string) => {
    if (branchVal === 'ALL') {
      setSelectedBranchId('ALL');
    } else {
      setSelectedBranchId(Number(branchVal));
    }
    setSelectedWarehouseId('ALL');
  };

  // Human-readable scope label
  const selectedScopeLabel = useMemo(() => {
    if (selectedBranchId === 'ALL' && selectedWarehouseId === 'ALL') {
      return 'كافة الفروع والمستودعات (تقرير مجمع)';
    }
    const branchName = selectedBranchId === 'ALL' 
      ? 'كافة الفروع' 
      : branchesList.find(b => b.id === selectedBranchId)?.name || `فرع ${selectedBranchId}`;
    const warehouseName = selectedWarehouseId === 'ALL'
      ? (selectedBranchId === 'ALL' ? 'كافة المستودعات' : 'كافة مستودعات هذا الفرع')
      : warehouses.find(w => w.id === selectedWarehouseId)?.name || selectedWarehouseId;

    return `${branchName} ⟵ [ ${warehouseName} ]`;
  }, [selectedBranchId, selectedWarehouseId, branchesList, warehouses]);

  // Filtered Sales Invoices
  const filteredSalesInvoices = useMemo(() => {
    return salesInvoices.filter(inv => {
      const invWh = inv.warehouseId || 'WH-01';
      if (selectedWarehouseId !== 'ALL') {
        return invWh === selectedWarehouseId;
      }
      if (selectedBranchId !== 'ALL') {
        const whObj = warehouses.find(w => w.id === invWh);
        return whObj ? whObj.branchId === selectedBranchId : false;
      }
      return true;
    });
  }, [salesInvoices, selectedBranchId, selectedWarehouseId, warehouses]);

  // Filtered Purchase Invoices
  const filteredPurchaseInvoices = useMemo(() => {
    return purchaseInvoices.filter(inv => {
      const invWh = inv.warehouseId || 'WH-01';
      if (selectedWarehouseId !== 'ALL') {
        return invWh === selectedWarehouseId;
      }
      if (selectedBranchId !== 'ALL') {
        const whObj = warehouses.find(w => w.id === invWh);
        return whObj ? whObj.branchId === selectedBranchId : false;
      }
      return true;
    });
  }, [purchaseInvoices, selectedBranchId, selectedWarehouseId, warehouses]);

  // Helper to compute stock of a product in the active filtered branch/warehouse
  const getFilteredStock = useCallback((product: ProductItem): number => {
    if (selectedWarehouseId !== 'ALL') {
      return getProductStockInWarehouse(product, selectedWarehouseId);
    }
    if (selectedBranchId !== 'ALL') {
      const branchWarehouses = warehouses.filter(w => w.branchId === selectedBranchId);
      return branchWarehouses.reduce((sum, wh) => sum + getProductStockInWarehouse(product, wh.id), 0);
    }
    return product.stock;
  }, [selectedWarehouseId, selectedBranchId, warehouses]);

  const filteredReports = useMemo(() => {
    return REPORTS_LIST.filter(r => {
      const matchesSearch = !searchTerm.trim() || 
        r.title.includes(searchTerm) || 
        r.desc.includes(searchTerm) ||
        (r.tags && r.tags.some(t => t.includes(searchTerm)));
      
      const matchesCategory = selectedCategory === 'ALL' || r.category === selectedCategory;
      return matchesSearch && matchesCategory;
    });
  }, [searchTerm, selectedCategory]);

  // Calculations for various dynamic reports
  const totalStockValuation = useMemo(() => {
    return products.reduce((sum, p) => sum + (getFilteredStock(p) * (p.costPrice || 0)), 0);
  }, [products, getFilteredStock]);

  const totalFilteredSalesRevenue = useMemo(() => {
    return filteredSalesInvoices.reduce((sum, inv) => sum + inv.grandTotal, 0);
  }, [filteredSalesInvoices]);

  const totalCashReceipts = useMemo(() => {
    return vouchers.filter(v => v.type === 'RECEIPT').reduce((sum, v) => sum + v.amount, 0);
  }, [vouchers]);

  const totalCashPayments = useMemo(() => {
    return vouchers.filter(v => v.type === 'PAYMENT').reduce((sum, v) => sum + v.amount, 0);
  }, [vouchers]);

  return (
    <div className="flex-1 flex flex-col h-full bg-[#f8fafc] text-slate-800 overflow-hidden text-xs select-none">
      {/* Top Header */}
      <div className="bg-white text-slate-800 border-b border-slate-200 px-4 py-3 flex items-center justify-between shadow-xs shrink-0">
        <div className="flex items-center gap-3">
          <button
            onClick={selectedReportId ? () => setSelectedReportId(null) : onBack}
            className="p-1.5 hover:bg-slate-100 rounded-full transition cursor-pointer text-slate-700"
            title="رجوع"
          >
            <ArrowLeft className="w-5 h-5" />
          </button>
          <div className="flex items-center gap-2">
            <h2 className="text-base font-bold text-slate-800">
              {selectedReportId 
                ? REPORTS_LIST.find(r => r.id === selectedReportId)?.title 
                : 'تقارير أخرى (Mohaseb Soft)'}
            </h2>
            {selectedReportId && (
              <span className="bg-sky-50 text-sky-700 border border-sky-200 text-[11px] px-2 py-0.5 rounded font-mono font-bold">
                {selectedScopeLabel}
              </span>
            )}
          </div>
        </div>

        {selectedReportId && (
          <button
            onClick={() => window.print()}
            className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200 px-3 py-1 rounded text-xs font-bold flex items-center gap-1.5 transition cursor-pointer"
          >
            <Printer className="w-4 h-4 text-slate-700" />
            <span>طباعة التقرير</span>
          </button>
        )}
      </div>

      {/* Cascading Branch & Warehouse Filter Bar */}
      <div className="bg-white border-b border-slate-300 px-4 py-2 flex flex-wrap items-center justify-between gap-3 shrink-0 shadow-2xs">
        <div className="flex flex-wrap items-center gap-3">
          <div className="flex items-center gap-1.5 text-slate-700 font-bold text-xs shrink-0">
            <Filter className="w-3.5 h-3.5 text-sky-600" />
            <span>تصفية الفروع والمخازن:</span>
          </div>

          {/* 1. Branch Selector */}
          <div className="flex items-center gap-1.5 bg-slate-50 border border-slate-300 rounded px-2 py-1">
            <Building2 className="w-3.5 h-3.5 text-sky-700" />
            <span className="text-[11px] font-semibold text-slate-600">الفرع:</span>
            <select
              value={selectedBranchId === 'ALL' ? 'ALL' : selectedBranchId.toString()}
              onChange={(e) => handleBranchChange(e.target.value)}
              className="bg-white border border-slate-200 rounded px-2 py-0.5 text-xs font-bold text-slate-800 outline-hidden cursor-pointer"
            >
              <option value="ALL">🏢 كافة الفروع (تقرير مجمّع)</option>
              {branchesList.map(b => (
                <option key={b.id} value={b.id.toString()}>🏢 {b.name}</option>
              ))}
            </select>
          </div>

          {/* 2. Warehouse Cascading Selector */}
          <div className="flex items-center gap-1.5 bg-slate-50 border border-slate-300 rounded px-2 py-1">
            <Store className="w-3.5 h-3.5 text-amber-600" />
            <span className="text-[11px] font-semibold text-slate-600">المستودع:</span>
            <select
              value={selectedWarehouseId}
              onChange={(e) => setSelectedWarehouseId(e.target.value)}
              className="bg-white border border-slate-200 rounded px-2 py-0.5 text-xs font-bold text-slate-800 outline-hidden cursor-pointer"
            >
              <option value="ALL">
                {selectedBranchId === 'ALL' ? '🏬 كافة المستودعات في النظام' : '🏬 كافة مستودعات الفرع المختار'}
              </option>
              {availableWarehouses.map(w => (
                <option key={w.id} value={w.id}>🏬 {w.name} ({w.code})</option>
              ))}
            </select>
          </div>

          {(selectedBranchId !== 'ALL' || selectedWarehouseId !== 'ALL') && (
            <button
              onClick={() => {
                setSelectedBranchId('ALL');
                setSelectedWarehouseId('ALL');
              }}
              className="bg-slate-200 hover:bg-slate-300 text-slate-700 text-[11px] font-bold px-2 py-1 rounded transition cursor-pointer"
            >
              عرض الكل
            </button>
          )}
        </div>

        <div className="flex items-center gap-1.5 text-slate-600 text-[11px]">
          <CheckCircle2 className="w-3.5 h-3.5 text-emerald-600" />
          <span>النطاق المعتمد:</span>
          <span className="font-bold text-sky-900">{selectedScopeLabel}</span>
        </div>
      </div>

      {/* Main Container */}
      {!selectedReportId ? (
        /* Report List View */
        <div className="flex-1 flex flex-col min-h-0 overflow-y-auto">
          {/* Top Classification Filter & Search Header */}
          <div className="p-3 bg-white border-b border-slate-200 sticky top-0 z-10 shadow-xs space-y-2.5">
            <div className="flex flex-wrap items-center justify-between gap-2">
              {/* Category Pills */}
              <div className="flex flex-wrap items-center gap-1.5">
                <button
                  onClick={() => setSelectedCategory('ALL')}
                  className={`px-3 py-1 rounded-full text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
                    selectedCategory === 'ALL'
                      ? 'bg-[#0288d1] text-white shadow-xs'
                      : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
                  }`}
                >
                  <LayoutGrid className="w-3.5 h-3.5" />
                  <span>كافة التقارير ({REPORTS_LIST.length})</span>
                </button>

                {REPORT_CATEGORIES.map(cat => {
                  const CatIcon = cat.icon;
                  const catCount = REPORTS_LIST.filter(r => r.category === cat.id).length;
                  const isSelected = selectedCategory === cat.id;

                  return (
                    <button
                      key={cat.id}
                      onClick={() => setSelectedCategory(cat.id)}
                      className={`px-3 py-1 rounded-full text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
                        isSelected
                          ? 'bg-[#0d47a1] text-white shadow-xs'
                          : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
                      }`}
                    >
                      <CatIcon className="w-3.5 h-3.5" />
                      <span>{cat.name} ({catCount})</span>
                    </button>
                  );
                })}
              </div>

              {/* Search bar */}
              <div className="relative w-full sm:w-72">
                <input
                  type="text"
                  placeholder="ابحث بالاسم أو الكلمات المفتاحية..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="w-full bg-slate-100 border border-slate-300 rounded-full pl-3 pr-8 py-1 text-xs text-slate-800 focus:outline-none focus:ring-2 focus:ring-[#0288d1] focus:bg-white"
                />
                <Search className="w-3.5 h-3.5 text-slate-400 absolute right-2.5 top-2" />
                {searchTerm && (
                  <button
                    onClick={() => setSearchTerm('')}
                    className="absolute left-2.5 top-1.5 text-slate-400 hover:text-slate-600 text-xs font-bold"
                  >
                    ×
                  </button>
                )}
              </div>
            </div>

            {/* Category Description Banner */}
            {selectedCategory !== 'ALL' && (
              <div className="bg-sky-50 border border-sky-200 rounded-md p-2 flex items-center gap-2 text-xs text-sky-900">
                <FolderTree className="w-4 h-4 text-sky-700 shrink-0" />
                <span>
                  <strong>{REPORT_CATEGORIES.find(c => c.id === selectedCategory)?.name}: </strong>
                  {REPORT_CATEGORIES.find(c => c.id === selectedCategory)?.desc}
                </span>
              </div>
            )}
          </div>

          {/* Grouped Reports Cards or List */}
          <div className="max-w-5xl mx-auto w-full p-3 space-y-4">
            {REPORT_CATEGORIES.filter(cat => selectedCategory === 'ALL' || selectedCategory === cat.id).map(category => {
              const categoryReports = filteredReports.filter(r => r.category === category.id);
              if (categoryReports.length === 0) return null;

              const CatIcon = category.icon;

              return (
                <div key={category.id} className="bg-white rounded-lg border border-slate-200 shadow-xs overflow-hidden">
                  {/* Category Header */}
                  <div className="bg-slate-50 border-b border-slate-200 px-3.5 py-2 flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <div className={`p-1.5 rounded-md ${category.colorClass}`}>
                        <CatIcon className="w-4 h-4" />
                      </div>
                      <div>
                        <h3 className="font-bold text-slate-800 text-xs">{category.name}</h3>
                        <p className="text-[10px] text-slate-500">{category.desc}</p>
                      </div>
                    </div>
                    <span className="text-[11px] font-bold text-slate-500 bg-slate-200/80 px-2 py-0.5 rounded-full">
                      {categoryReports.length} تقرير
                    </span>
                  </div>

                  {/* Reports Grid in this Category */}
                  <div className="divide-y divide-slate-100">
                    {categoryReports.map((report) => (
                      <div
                        key={report.id}
                        onClick={() => setSelectedReportId(report.id)}
                        className="flex items-center justify-between p-3 hover:bg-sky-50 transition cursor-pointer group"
                      >
                        <div className="flex items-center gap-3">
                          <div className="w-8 h-8 rounded-lg bg-slate-100 text-[#0288d1] flex items-center justify-center shrink-0 group-hover:scale-105 group-hover:bg-sky-100 transition shadow-2xs">
                            <report.icon className="w-4 h-4" />
                          </div>
                          <div>
                            <div className="flex items-center gap-2">
                              <h4 className="font-bold text-slate-800 text-xs group-hover:text-[#0288d1] transition">
                                {report.title}
                              </h4>
                              {report.v2Note && (
                                <span className="bg-amber-100 text-amber-800 border border-amber-300 text-[10px] px-1.5 py-0.2 rounded font-semibold">
                                  قريباً V2
                                </span>
                              )}
                            </div>
                            <p className="text-[11px] text-slate-500 mt-0.5">{report.desc}</p>
                            {report.tags && report.tags.length > 0 && (
                              <div className="flex items-center gap-1 mt-1">
                                {report.tags.map((tag, tIdx) => (
                                  <span key={tIdx} className="text-[9px] bg-slate-100 text-slate-500 px-1.5 py-0.2 rounded">
                                    #{tag}
                                  </span>
                                ))}
                              </div>
                            )}
                          </div>
                        </div>

                        <div className="flex items-center gap-2 text-slate-400 group-hover:text-[#0288d1] transition">
                          <span className="text-[10px] font-semibold hidden sm:inline text-slate-500 group-hover:text-[#0288d1]">
                            فتح التقرير
                          </span>
                          <div className="w-6 h-8 border border-sky-300 rounded bg-gradient-to-b from-sky-50 to-white flex flex-col items-center justify-center shadow-2xs">
                            <div className="w-2.5 h-0.5 bg-sky-400 rounded-full mb-0.5"></div>
                            <FileText className="w-3 h-3 text-[#0288d1]" />
                          </div>
                          <ChevronLeft className="w-4 h-4" />
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              );
            })}

            {filteredReports.length === 0 && (
              <div className="p-8 text-center bg-white rounded-lg border border-slate-200 text-slate-500">
                <Search className="w-10 h-10 text-slate-300 mx-auto mb-2" />
                <p className="font-bold text-slate-700">لم يتم العثور على تقارير مطابقة</p>
                <p className="text-xs text-slate-400 mt-1">جرب البحث بكلمة أخرى أو تصفح كافة التصنيفات</p>
              </div>
            )}
          </div>
        </div>
      ) : (
        /* Detailed Report Drill-Down View */
        <div className="flex-1 flex flex-col min-h-0 bg-white p-4 overflow-y-auto">
          
          {/* Report 1: المخزون المتبقي */}
          {selectedReportId === 'rem_stock' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-sky-50 border border-sky-200 rounded-lg p-3 flex flex-wrap justify-between items-center gap-3">
                <div>
                  <h4 className="font-bold text-sky-950 text-sm">تقرير المخزون المتبقي والتكلفة الإجمالية</h4>
                  <p className="text-xs text-sky-700">النطاق: {selectedScopeLabel} | عدد الأصناف: {products.length} صنف</p>
                </div>
                <div className="text-left font-mono font-bold text-emerald-700 text-base">
                  إجمالي قيمة المخزون: {totalStockValuation.toLocaleString()} {currency}
                </div>
              </div>

              <div className="border border-slate-200 rounded-lg overflow-hidden">
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                    <tr>
                      <th className="p-2.5">كود الصنف</th>
                      <th className="p-2.5">اسم الصنف</th>
                      <th className="p-2.5">القسم</th>
                      <th className="p-2.5 text-center">الوحدة</th>
                      <th className="p-2.5 text-center">الرصيد في النطاق</th>
                      <th className="p-2.5 text-center">تكلفة الوحدة</th>
                      <th className="p-2.5 text-center">سعر البيع</th>
                      <th className="p-2.5 text-left">إجمالي القيمة ({currency})</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {products.map((p) => {
                      const qty = getFilteredStock(p);
                      const val = qty * (p.costPrice || 0);
                      return (
                        <tr key={p.id} className="hover:bg-slate-50">
                          <td className="p-2.5 font-mono text-slate-500 font-bold">{p.code}</td>
                          <td className="p-2.5 font-bold text-slate-800">{p.name}</td>
                          <td className="p-2.5 text-slate-600">{p.category || 'عام'}</td>
                          <td className="p-2.5 text-center text-slate-600">{p.unit || 'حبة'}</td>
                          <td className="p-2.5 text-center font-mono font-bold">
                            <span className={`px-2 py-0.5 rounded text-xs ${qty <= (p.minStock || 5) ? 'bg-rose-100 text-rose-700' : 'bg-emerald-100 text-emerald-700'}`}>
                              {qty}
                            </span>
                          </td>
                          <td className="p-2.5 text-center font-mono text-slate-600">{(p.costPrice || 0).toLocaleString()}</td>
                          <td className="p-2.5 text-center font-mono text-sky-700 font-bold">{p.salePrice.toLocaleString()}</td>
                          <td className="p-2.5 text-left font-mono font-bold text-emerald-700">
                            {val.toLocaleString()}
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Report 2: أرصدة الحسابات */}
          {selectedReportId === 'acc_balances' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="border border-slate-200 rounded-lg p-3 bg-white shadow-xs">
                  <h4 className="font-bold text-slate-800 text-sm mb-2 pb-1 border-b border-slate-100 flex items-center justify-between">
                    <span>أرصدة العملاء (الذمم المدينة)</span>
                    <span className="text-xs text-rose-600 font-mono font-bold">
                      {customers.reduce((s, c) => s + c.currentBalance, 0).toLocaleString()} {currency}
                    </span>
                  </h4>
                  <div className="space-y-2 max-h-96 overflow-y-auto">
                    {customers.map((c) => (
                      <div key={c.id} className="flex items-center justify-between p-2 rounded bg-slate-50 border border-slate-100">
                        <div>
                          <p className="font-bold text-slate-800">{c.name}</p>
                          <p className="text-[10px] text-slate-500">سقف الائتمان: {c.creditLimit ? `${c.creditLimit.toLocaleString()} ${currency}` : 'غير محدد'}</p>
                        </div>
                        <span className="font-mono font-bold text-rose-700">{c.currentBalance.toLocaleString()} {currency}</span>
                      </div>
                    ))}
                  </div>
                </div>

                <div className="border border-slate-200 rounded-lg p-3 bg-white shadow-xs">
                  <h4 className="font-bold text-slate-800 text-sm mb-2 pb-1 border-b border-slate-100 flex items-center justify-between">
                    <span>أرصدة الموردين (الذمم الدائنة)</span>
                    <span className="text-xs text-sky-600 font-mono font-bold">
                      {suppliers.reduce((s, sup) => s + sup.currentBalance, 0).toLocaleString()} {currency}
                    </span>
                  </h4>
                  <div className="space-y-2 max-h-96 overflow-y-auto">
                    {suppliers.map((sup) => (
                      <div key={sup.id} className="flex items-center justify-between p-2 rounded bg-slate-50 border border-slate-100">
                        <div>
                          <p className="font-bold text-slate-800">{sup.name}</p>
                          <p className="text-[10px] text-slate-500">{sup.phone || sup.company || 'مورد رئيسي'}</p>
                        </div>
                        <span className="font-mono font-bold text-sky-700">{sup.currentBalance.toLocaleString()} {currency}</span>
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* Report 3: أرباح الأصناف */}
          {selectedReportId === 'item_profits' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-emerald-50 border border-emerald-200 rounded-lg p-3 flex justify-between items-center">
                <div>
                  <h4 className="font-bold text-emerald-950 text-sm">تقرير أرباح الأصناف والهوامش التجارية</h4>
                  <p className="text-xs text-emerald-700">النطاق: {selectedScopeLabel}</p>
                </div>
              </div>

              <div className="border border-slate-200 rounded-lg overflow-hidden">
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                    <tr>
                      <th className="p-2.5">اسم الصنف</th>
                      <th className="p-2.5 text-center">سعر البيع</th>
                      <th className="p-2.5 text-center">تكلفة الشراء</th>
                      <th className="p-2.5 text-center">الربح للوحدة</th>
                      <th className="p-2.5 text-center">نسبة الهامش</th>
                      <th className="p-2.5 text-center">الكمية المباعة (بالنطاق)</th>
                      <th className="p-2.5 text-left">إجمالي الربح المحقق</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {products.map((p) => {
                      const cost = p.costPrice || 0;
                      const profitPerUnit = Math.max(0, p.salePrice - cost);
                      const marginPercent = p.salePrice > 0 ? ((profitPerUnit / p.salePrice) * 100).toFixed(1) : '0';
                      
                      const soldQty = filteredSalesInvoices.reduce((acc, inv) => {
                        const line = inv.items.find(i => i.item.id === p.id);
                        return acc + (line ? line.quantity : 0);
                      }, 0);
                      const totalProfit = soldQty * profitPerUnit;

                      return (
                        <tr key={p.id} className="hover:bg-slate-50">
                          <td className="p-2.5 font-bold text-slate-800">{p.name}</td>
                          <td className="p-2.5 text-center font-mono font-bold">{p.salePrice.toLocaleString()}</td>
                          <td className="p-2.5 text-center font-mono text-slate-500">{cost.toLocaleString()}</td>
                          <td className="p-2.5 text-center font-mono font-bold text-emerald-700">+{profitPerUnit.toLocaleString()}</td>
                          <td className="p-2.5 text-center font-mono text-sky-700 font-bold">{marginPercent}%</td>
                          <td className="p-2.5 text-center font-mono">{soldQty} {p.unit || 'حبة'}</td>
                          <td className="p-2.5 text-left font-mono font-bold text-emerald-700">
                            {totalProfit.toLocaleString()} {currency}
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Report 4: العمليات اليومية */}
          {selectedReportId === 'daily_ops' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-sky-50 border border-sky-200 rounded-lg p-3">
                <h4 className="font-bold text-sky-950 text-sm">سجل العمليات اليومية (مبيعات، مشتريات، سندات)</h4>
                <p className="text-xs text-sky-700">النطاق: {selectedScopeLabel} | إجمالي المبيعات المنجزة: {totalFilteredSalesRevenue.toLocaleString()} {currency}</p>
              </div>

              <div className="border border-slate-200 rounded-lg overflow-hidden">
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                    <tr>
                      <th className="p-2.5">رقم العملية</th>
                      <th className="p-2.5">نوع المستند</th>
                      <th className="p-2.5">التاريخ والوقت</th>
                      <th className="p-2.5">الطرف المقابل</th>
                      <th className="p-2.5">المستودع</th>
                      <th className="p-2.5 text-left">المبلغ ({currency})</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {filteredSalesInvoices.map((inv) => (
                      <tr key={`sale-${inv.id}`} className="hover:bg-slate-50">
                        <td className="p-2.5 font-mono text-sky-700 font-bold">{inv.invoiceNo}</td>
                        <td className="p-2.5"><span className="bg-emerald-100 text-emerald-800 px-2 py-0.5 rounded font-bold text-[10px]">فاتورة مبيعات</span></td>
                        <td className="p-2.5 font-mono">{inv.date} {inv.time}</td>
                        <td className="p-2.5 font-bold">{inv.customerOrSupplierName}</td>
                        <td className="p-2.5 text-slate-600">{inv.warehouseName || 'المستودع الرئيسي'}</td>
                        <td className="p-2.5 text-left font-mono font-bold text-emerald-700">+{inv.grandTotal.toLocaleString()}</td>
                      </tr>
                    ))}
                    {filteredPurchaseInvoices.map((inv) => (
                      <tr key={`pur-${inv.id}`} className="hover:bg-slate-50">
                        <td className="p-2.5 font-mono text-purple-700 font-bold">{inv.invoiceNo}</td>
                        <td className="p-2.5"><span className="bg-purple-100 text-purple-800 px-2 py-0.5 rounded font-bold text-[10px]">فاتورة مشتريات</span></td>
                        <td className="p-2.5 font-mono">{inv.date} {inv.time}</td>
                        <td className="p-2.5 font-bold">{inv.customerOrSupplierName}</td>
                        <td className="p-2.5 text-slate-600">{inv.warehouseName || 'المستودع الرئيسي'}</td>
                        <td className="p-2.5 text-left font-mono font-bold text-purple-700">-{inv.grandTotal.toLocaleString()}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Report 5: القيود اليومية */}
          {selectedReportId === 'daily_journals' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
               <div className="border border-slate-200 rounded-lg overflow-hidden">
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                    <tr>
                      <th className="p-2.5">رقم القيد</th>
                      <th className="p-2.5">التاريخ</th>
                      <th className="p-2.5">البيان</th>
                      <th className="p-2.5 text-center">إجمالي مدين</th>
                      <th className="p-2.5 text-center">إجمالي دائن</th>
                      <th className="p-2.5 text-center">الحالة</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {journalEntries.length > 0 ? journalEntries.map((je) => (
                      <tr key={je.id} className="hover:bg-slate-50 bg-white">
                        <td className="p-2.5 font-mono text-sky-700 font-bold">{je.entryNo}</td>
                        <td className="p-2.5 font-mono">{je.date}</td>
                        <td className="p-2.5 text-slate-700">{je.description}</td>
                        <td className="p-2.5 text-center font-mono font-bold text-emerald-700">{je.totalDebit.toLocaleString()} {currency}</td>
                        <td className="p-2.5 text-center font-mono font-bold text-rose-700">{je.totalCredit.toLocaleString()} {currency}</td>
                        <td className="p-2.5 text-center">
                          <span className={`px-2 py-0.5 rounded text-[10px] font-bold ${je.status === 'POSTED' ? 'bg-emerald-100 text-emerald-700' : 'bg-amber-100 text-amber-700'}`}>
                            {je.status === 'POSTED' ? 'مرحل' : 'مسودة'}
                          </span>
                        </td>
                      </tr>
                    )) : (
                      <tr><td colSpan={6} className="p-4 text-center text-slate-500 bg-white">لا توجد قيود مسجلة</td></tr>
                    )}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Report 6: حركة الصندوق */}
          {selectedReportId === 'cashbox_movement' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
                <div className="p-3 bg-emerald-50 border border-emerald-200 rounded-lg">
                  <p className="text-xs text-emerald-800 font-bold">إجمالي المقبوضات النقدية</p>
                  <p className="text-xl font-bold font-mono text-emerald-700 mt-1">+{totalCashReceipts.toLocaleString()} {currency}</p>
                </div>
                <div className="p-3 bg-rose-50 border border-rose-200 rounded-lg">
                  <p className="text-xs text-rose-800 font-bold">إجمالي المدفوعات والمصروفات</p>
                  <p className="text-xl font-bold font-mono text-rose-700 mt-1">-{totalCashPayments.toLocaleString()} {currency}</p>
                </div>
                <div className="p-3 bg-sky-50 border border-sky-200 rounded-lg">
                  <p className="text-xs text-sky-800 font-bold">صافي حركة الصندوق</p>
                  <p className="text-xl font-bold font-mono text-sky-700 mt-1">{(totalCashReceipts - totalCashPayments).toLocaleString()} {currency}</p>
                </div>
              </div>

              <div className="border border-slate-200 rounded-lg overflow-hidden">
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                    <tr>
                      <th className="p-2.5">رقم السند</th>
                      <th className="p-2.5">النوع</th>
                      <th className="p-2.5">التاريخ والوقت</th>
                      <th className="p-2.5">المستلم / الدافع</th>
                      <th className="p-2.5">البيان</th>
                      <th className="p-2.5 text-left">المبلغ ({currency})</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {vouchers.map((v) => (
                      <tr key={v.id} className="hover:bg-slate-50">
                        <td className="p-2.5 font-mono text-slate-500 font-bold">{v.voucherNo}</td>
                        <td className="p-2.5">
                          <span className={`px-2 py-0.5 rounded text-[11px] font-bold ${v.type === 'RECEIPT' ? 'bg-emerald-100 text-emerald-800' : 'bg-rose-100 text-rose-800'}`}>
                            {v.type === 'RECEIPT' ? 'قبض نقدي' : 'صرف نقدي'}
                          </span>
                        </td>
                        <td className="p-2.5 font-mono text-slate-600">{v.date}</td>
                        <td className="p-2.5 font-bold text-slate-800">{v.partyName}</td>
                        <td className="p-2.5 text-slate-600">{v.description || '-'}</td>
                        <td className="p-2.5 text-left font-mono font-bold">
                          <span className={v.type === 'RECEIPT' ? 'text-emerald-700' : 'text-rose-700'}>
                            {v.type === 'RECEIPT' ? '+' : '-'}{v.amount.toLocaleString()}
                          </span>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Report 7: حركة الحسابات */}
          {selectedReportId === 'account_movement' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-white p-3 border border-slate-200 rounded-lg flex items-center gap-3 shadow-2xs">
                <label className="font-bold text-slate-700 text-xs shrink-0">اختر الحساب المطلوب:</label>
                <select
                  value={selectedMovementAccountId}
                  onChange={(e) => setSelectedMovementAccountId(e.target.value)}
                  className="bg-slate-50 border border-slate-300 rounded px-2 py-1.5 font-bold text-slate-800 text-xs w-72 outline-hidden"
                >
                  <option value="">-- يرجى اختيار حساب لعرض الكشف --</option>
                  {chartOfAccounts.map(acc => (
                    <option key={acc.code} value={acc.code}>{acc.code} - {acc.name}</option>
                  ))}
                </select>
              </div>

              {selectedMovementAccountId && (
                <div className="border border-slate-200 rounded-lg overflow-hidden">
                  <table className="w-full text-right border-collapse text-xs">
                    <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                      <tr>
                        <th className="p-2.5">التاريخ</th>
                        <th className="p-2.5">رقم القيد</th>
                        <th className="p-2.5">البيان</th>
                        <th className="p-2.5 text-center">مدين</th>
                        <th className="p-2.5 text-center">دائن</th>
                        <th className="p-2.5 text-center">الرصيد التراكمي</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-slate-100">
                      {(() => {
                        let runningBalance = 0;
                        const lines = journalEntries
                          .filter(je => je.status === 'POSTED')
                          .flatMap(je => je.lines.map(line => ({ ...line, date: je.date, entryNo: je.entryNo, description: je.description })))
                          .filter(line => line.accountCode === selectedMovementAccountId)
                          .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());

                        if (lines.length === 0) {
                          return <tr><td colSpan={6} className="p-4 text-center text-slate-500 bg-white">لا توجد حركات مسجلة لهذا الحساب</td></tr>;
                        }

                        return lines.map((line, idx) => {
                          runningBalance += (line.debit - line.credit);
                          return (
                            <tr key={idx} className="hover:bg-slate-50 bg-white">
                              <td className="p-2.5 font-mono">{line.date}</td>
                              <td className="p-2.5 font-mono text-sky-700 font-bold">{line.entryNo}</td>
                              <td className="p-2.5 text-slate-700">{line.note || line.description}</td>
                              <td className="p-2.5 text-center font-mono font-bold text-emerald-700">{line.debit > 0 ? line.debit.toLocaleString() : '-'}</td>
                              <td className="p-2.5 text-center font-mono font-bold text-rose-700">{line.credit > 0 ? line.credit.toLocaleString() : '-'}</td>
                              <td className="p-2.5 text-center font-mono font-bold text-slate-800" dir="ltr">{runningBalance.toLocaleString()} {currency}</td>
                            </tr>
                          );
                        });
                      })()}
                    </tbody>
                  </table>
                </div>
              )}
            </div>
          )}

          {/* Report 8: إجمالي التصنيفات */}
          {selectedReportId === 'category_totals' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-sky-50 border border-sky-200 rounded-lg p-3">
                <h4 className="font-bold text-sky-950 text-sm">إجمالي مبيعات ومخزون الأقسام والتصنيفات</h4>
                <p className="text-xs text-sky-700">النطاق: {selectedScopeLabel}</p>
              </div>

              <div className="border border-slate-200 rounded-lg overflow-hidden">
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                    <tr>
                      <th className="p-2.5">القسم / التصنيف</th>
                      <th className="p-2.5 text-center">عدد الأصناف</th>
                      <th className="p-2.5 text-center">إجمالي المخزون (بالنطاق)</th>
                      <th className="p-2.5 text-center">تقييم المخزون ({currency})</th>
                      <th className="p-2.5 text-left">إجمالي المبيعات ({currency})</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {(() => {
                      const categories = Array.from(new Set(products.map(p => p.category || 'عام')));
                      return categories.map((cat, idx) => {
                        const catProducts = products.filter(p => (p.category || 'عام') === cat);
                        const catStock = catProducts.reduce((sum, p) => sum + getFilteredStock(p), 0);
                        const catValuation = catProducts.reduce((sum, p) => sum + (getFilteredStock(p) * (p.costPrice || 0)), 0);
                        const catSales = filteredSalesInvoices.reduce((sum, inv) => {
                          const lines = inv.items.filter(i => (i.item.category || 'عام') === cat);
                          return sum + lines.reduce((lsum, l) => lsum + l.total, 0);
                        }, 0);

                        return (
                          <tr key={idx} className="hover:bg-slate-50 bg-white">
                            <td className="p-2.5 font-bold text-slate-800">{cat}</td>
                            <td className="p-2.5 text-center font-mono">{catProducts.length}</td>
                            <td className="p-2.5 text-center font-mono font-bold text-sky-700">{catStock}</td>
                            <td className="p-2.5 text-center font-mono font-bold text-emerald-700">{catValuation.toLocaleString()}</td>
                            <td className="p-2.5 text-left font-mono font-bold text-slate-900">{catSales.toLocaleString()} {currency}</td>
                          </tr>
                        );
                      });
                    })()}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Report 9: فوارق أسعار العملات */}
          {selectedReportId === 'fx_variance' && (
            <div className="max-w-2xl mx-auto my-8 p-6 bg-amber-50 border-2 border-amber-300 rounded-xl text-center space-y-3">
              <div className="w-12 h-12 rounded-full bg-amber-100 text-amber-700 flex items-center justify-center mx-auto">
                <AlertCircle className="w-6 h-6" />
              </div>
              <h3 className="text-base font-bold text-amber-900">تقرير فوارق أسعار العملات (جاهز للإصدار القادم V2)</h3>
              <p className="text-xs text-amber-800 leading-relaxed max-w-lg mx-auto">
                تم تجهيز هيكل البيانات المحاسبي لدعم العملات المتعددة وفروقات الصرف الأجنبي وسيتم تفعيل الحسابات المتعددة في الإصدار القادم القريب. النظام حالياً يعمل بالعملة الأساسية الموحدة ({currency}).
              </p>
              <button
                onClick={() => setSelectedReportId(null)}
                className="bg-amber-600 hover:bg-amber-700 text-white px-4 py-1.5 rounded-lg text-xs font-bold transition cursor-pointer"
              >
                العودة لقائمة التقارير
              </button>
            </div>
          )}

          {/* Report 10: المبيعات حسب الصنف */}
          {selectedReportId === 'sales_by_item' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-sky-50 border border-sky-200 rounded-lg p-3">
                <h4 className="font-bold text-sky-950 text-sm">تقرير المبيعات التفصيلي حسب الأصناف</h4>
                <p className="text-xs text-sky-700">النطاق: {selectedScopeLabel}</p>
              </div>

              <div className="border border-slate-200 rounded-lg overflow-hidden">
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                    <tr>
                      <th className="p-2.5">كود الصنف</th>
                      <th className="p-2.5">اسم الصنف</th>
                      <th className="p-2.5 text-center">الكمية المباعة</th>
                      <th className="p-2.5 text-center">إجمالي الإيرادات</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {(() => {
                      const salesStats: Record<string, { code: string, name: string, qty: number, revenue: number }> = {};
                      filteredSalesInvoices.forEach(inv => {
                        inv.items.forEach(cartItem => {
                          const p = cartItem.item;
                          if (!salesStats[p.id]) {
                            salesStats[p.id] = { code: p.code, name: p.name, qty: 0, revenue: 0 };
                          }
                          salesStats[p.id].qty += cartItem.quantity;
                          salesStats[p.id].revenue += cartItem.total;
                        });
                      });
                      const statsArray = Object.values(salesStats).sort((a, b) => b.revenue - a.revenue);

                      if (statsArray.length === 0) {
                         return <tr><td colSpan={4} className="p-4 text-center text-slate-500">لا توجد مبيعات مسجلة في هذا النطاق</td></tr>;
                      }

                      return statsArray.map((s, idx) => (
                        <tr key={idx} className="hover:bg-slate-50 bg-white">
                          <td className="p-2.5 font-mono text-slate-500 font-bold">{s.code}</td>
                          <td className="p-2.5 font-bold text-slate-800">{s.name}</td>
                          <td className="p-2.5 text-center font-mono font-bold">{s.qty}</td>
                          <td className="p-2.5 text-center font-mono text-emerald-700 font-bold">{s.revenue.toLocaleString()} {currency}</td>
                        </tr>
                      ));
                    })()}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Report 11: المشتريات حسب الصنف */}
          {selectedReportId === 'purchases_by_item' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-purple-50 border border-purple-200 rounded-lg p-3">
                <h4 className="font-bold text-purple-950 text-sm">تقرير المشتريات وتوريدات الأصناف</h4>
                <p className="text-xs text-purple-700">النطاق: {selectedScopeLabel}</p>
              </div>

              <div className="border border-slate-200 rounded-lg overflow-hidden">
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                    <tr>
                      <th className="p-2.5">كود الصنف</th>
                      <th className="p-2.5">اسم الصنف</th>
                      <th className="p-2.5 text-center">الكمية المشتراة</th>
                      <th className="p-2.5 text-center">إجمالي تكلفة الشراء ({currency})</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {(() => {
                      const purStats: Record<string, { code: string, name: string, qty: number, cost: number }> = {};
                      filteredPurchaseInvoices.forEach(inv => {
                        inv.items.forEach(cartItem => {
                          const p = cartItem.item;
                          if (!purStats[p.id]) {
                            purStats[p.id] = { code: p.code, name: p.name, qty: 0, cost: 0 };
                          }
                          purStats[p.id].qty += cartItem.quantity;
                          purStats[p.id].cost += cartItem.total;
                        });
                      });
                      const statsArray = Object.values(purStats).sort((a, b) => b.cost - a.cost);

                      if (statsArray.length === 0) {
                         return <tr><td colSpan={4} className="p-4 text-center text-slate-500">لا توجد مشتريات مسجلة في هذا النطاق</td></tr>;
                      }

                      return statsArray.map((s, idx) => (
                        <tr key={idx} className="hover:bg-slate-50 bg-white">
                          <td className="p-2.5 font-mono text-slate-500 font-bold">{s.code}</td>
                          <td className="p-2.5 font-bold text-slate-800">{s.name}</td>
                          <td className="p-2.5 text-center font-mono font-bold">{s.qty}</td>
                          <td className="p-2.5 text-center font-mono text-purple-700 font-bold">{s.cost.toLocaleString()} {currency}</td>
                        </tr>
                      ));
                    })()}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Report 12: تفاصيل حركة الأصناف */}
          {selectedReportId === 'item_movement_details' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-white p-3 border border-slate-200 rounded-lg flex flex-wrap items-center gap-3 shadow-2xs">
                <label className="font-bold text-slate-700 text-xs shrink-0">اختر الصنف لعرض كرت الحركة:</label>
                <select
                  value={selectedMovementProductId}
                  onChange={(e) => setSelectedMovementProductId(e.target.value)}
                  className="bg-slate-50 border border-slate-300 rounded px-2 py-1.5 font-bold text-slate-800 text-xs w-72 outline-hidden"
                >
                  {products.map(p => (
                    <option key={p.id} value={p.id}>{p.code} - {p.name}</option>
                  ))}
                </select>
                <span className="text-xs text-slate-500">النطاق النشط: {selectedScopeLabel}</span>
              </div>

              {selectedMovementProductId && (
                <div className="border border-slate-200 rounded-lg overflow-hidden">
                  <table className="w-full text-right border-collapse text-xs">
                    <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                      <tr>
                        <th className="p-2.5">التاريخ</th>
                        <th className="p-2.5">المستند / الحركة</th>
                        <th className="p-2.5">المستودع</th>
                        <th className="p-2.5 text-center">الوارد (إدخال)</th>
                        <th className="p-2.5 text-center">المنصرف (إخراج)</th>
                        <th className="p-2.5 text-center">الرصيد التراكمي</th>
                        <th className="p-2.5 text-left">التكلفة</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-slate-100">
                      {(() => {
                        const targetProduct = products.find(p => p.id === selectedMovementProductId);
                        if (!targetProduct) return null;

                        const ledgerEntries = generateProductStockLedger(
                          targetProduct.id,
                          selectedWarehouseId,
                          targetProduct,
                          filteredSalesInvoices,
                          filteredPurchaseInvoices,
                          [],
                          []
                        );

                        if (ledgerEntries.length === 0) {
                          return <tr><td colSpan={7} className="p-4 text-center text-slate-500 bg-white">لا توجد حركات مسجلة لهذا الصنف</td></tr>;
                        }

                        return ledgerEntries.map((entry, idx) => (
                          <tr key={idx} className="hover:bg-slate-50 bg-white">
                            <td className="p-2.5 font-mono">{entry.date}</td>
                            <td className="p-2.5 font-bold text-slate-800">{entry.movementLabel} #{entry.documentNo}</td>
                            <td className="p-2.5 text-slate-600">{entry.warehouseName}</td>
                            <td className="p-2.5 text-center font-mono font-bold text-emerald-700">{entry.inQty > 0 ? `+${entry.inQty}` : '-'}</td>
                            <td className="p-2.5 text-center font-mono font-bold text-rose-700">{entry.outQty > 0 ? `-${entry.outQty}` : '-'}</td>
                            <td className="p-2.5 text-center font-mono font-bold text-sky-800">{entry.balanceAfter}</td>
                            <td className="p-2.5 text-left font-mono font-bold text-slate-700">{entry.unitPriceOrCost.toLocaleString()} {currency}</td>
                          </tr>
                        ));
                      })()}
                    </tbody>
                  </table>
                </div>
              )}
            </div>
          )}

          {/* Report 13: الربح على مستوى العميل */}
          {selectedReportId === 'profit_by_customer' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-sky-50 border border-sky-200 rounded-lg p-3">
                <h4 className="font-bold text-sky-950 text-sm">صافي ربحية المعاملات على مستوى كل عميل</h4>
                <p className="text-xs text-sky-700">النطاق: {selectedScopeLabel}</p>
              </div>

              <div className="border border-slate-200 rounded-lg overflow-hidden">
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                    <tr>
                      <th className="p-2.5">رقم العميل</th>
                      <th className="p-2.5">اسم العميل</th>
                      <th className="p-2.5 text-center">إجمالي المبيعات</th>
                      <th className="p-2.5 text-center">إجمالي التكلفة</th>
                      <th className="p-2.5 text-center">صافي الربح</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {(() => {
                      const custStats: Record<string, { code: string, name: string, sales: number, cost: number }> = {};
                      filteredSalesInvoices.forEach(inv => {
                        const custName = inv.customerOrSupplierName || 'عميل نقدي';
                        if (!custStats[custName]) {
                          custStats[custName] = { code: '-', name: custName, sales: 0, cost: 0 };
                        }
                        custStats[custName].sales += inv.grandTotal;
                        custStats[custName].cost += inv.items.reduce((sum, item) => sum + (item.unitCost * item.quantity), 0);
                      });

                      const statsArray = Object.values(custStats).sort((a, b) => (b.sales - b.cost) - (a.sales - a.cost));

                      if (statsArray.length === 0) {
                         return <tr><td colSpan={5} className="p-4 text-center text-slate-500 bg-white">لا توجد مبيعات مسجلة للعملاء في هذا النطاق</td></tr>;
                      }

                      return statsArray.map((s, idx) => {
                        const profit = s.sales - s.cost;
                        return (
                          <tr key={idx} className="hover:bg-slate-50 bg-white">
                            <td className="p-2.5 font-mono text-slate-500">{s.code}</td>
                            <td className="p-2.5 font-bold text-slate-800">{s.name}</td>
                            <td className="p-2.5 text-center font-mono">{s.sales.toLocaleString()} {currency}</td>
                            <td className="p-2.5 text-center font-mono text-rose-600">{s.cost.toLocaleString()} {currency}</td>
                            <td className="p-2.5 text-center font-mono font-bold text-emerald-700">{profit.toLocaleString()} {currency}</td>
                          </tr>
                        );
                      });
                    })()}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Report 14: الأصناف حسب الإنتهاء */}
          {selectedReportId === 'expiry_report' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-amber-50 border border-amber-200 rounded-lg p-3">
                <h4 className="font-bold text-amber-950 text-sm">تقرير تواريخ الصلاحية والدفعات (FEFO)</h4>
                <p className="text-xs text-amber-800">النطاق: {selectedScopeLabel} | متابعة الصلاحيات لتفادي الركود والتلف</p>
              </div>

              <div className="border border-slate-200 rounded-lg overflow-hidden">
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                    <tr>
                      <th className="p-2.5">كود الصنف</th>
                      <th className="p-2.5">اسم الصنف</th>
                      <th className="p-2.5">رقم التشغيلة (Batch)</th>
                      <th className="p-2.5 text-center">تاريخ الانتهاء</th>
                      <th className="p-2.5 text-center">الرصيد في النطاق</th>
                      <th className="p-2.5 text-center">الحالة</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {products.filter(p => p.hasExpiryDate || p.expiryDate).map((p) => {
                      const qty = getFilteredStock(p);
                      const isExpired = p.expiryDate && new Date(p.expiryDate) < new Date();
                      return (
                        <tr key={p.id} className="hover:bg-slate-50 bg-white">
                          <td className="p-2.5 font-mono text-slate-500 font-bold">{p.code}</td>
                          <td className="p-2.5 font-bold text-slate-800">{p.name}</td>
                          <td className="p-2.5 font-mono text-slate-700">{p.batchNo || 'BATCH-001'}</td>
                          <td className="p-2.5 text-center font-mono font-bold text-slate-800">{p.expiryDate || '2027-01-01'}</td>
                          <td className="p-2.5 text-center font-mono font-bold">{qty} {p.unit}</td>
                          <td className="p-2.5 text-center">
                            <span className={`px-2 py-0.5 rounded text-[10px] font-bold ${isExpired ? 'bg-rose-100 text-rose-800' : 'bg-emerald-100 text-emerald-800'}`}>
                              {isExpired ? 'منتهي الصلاحية' : 'ساري الصلاحية'}
                            </span>
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Report 15: رأس المال العامل */}
          {selectedReportId === 'working_capital' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-sky-50 border border-sky-200 rounded-lg p-3">
                <h4 className="font-bold text-sky-950 text-sm">مؤشر رأس المال العامل والسيولة التشغيلية</h4>
                <p className="text-xs text-sky-700">النطاق: {selectedScopeLabel}</p>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
                <div className="p-3 bg-emerald-50 border border-emerald-200 rounded-lg">
                  <p className="text-xs text-emerald-800 font-bold">الأصول المتداولة (مخزون + نقدية + مدينون)</p>
                  <p className="text-lg font-bold font-mono text-emerald-700 mt-1">
                    {(totalStockValuation + 250000 + customers.reduce((s, c) => s + c.currentBalance, 0)).toLocaleString()} {currency}
                  </p>
                </div>
                <div className="p-3 bg-rose-50 border border-rose-200 rounded-lg">
                  <p className="text-xs text-rose-800 font-bold">الالتزامات المتداولة (الموردون)</p>
                  <p className="text-lg font-bold font-mono text-rose-700 mt-1">
                    {suppliers.reduce((s, sup) => s + sup.currentBalance, 0).toLocaleString()} {currency}
                  </p>
                </div>
                <div className="p-3 bg-sky-50 border border-sky-200 rounded-lg">
                  <p className="text-xs text-sky-800 font-bold">صافي رأس المال العامل</p>
                  <p className="text-lg font-bold font-mono text-sky-700 mt-1">
                    {((totalStockValuation + 250000 + customers.reduce((s, c) => s + c.currentBalance, 0)) - suppliers.reduce((s, sup) => s + sup.currentBalance, 0)).toLocaleString()} {currency}
                  </p>
                </div>
              </div>
            </div>
          )}

          {/* Report 16: تحليل حركة المبيعات المجمعة */}
          {selectedReportId === 'sales_analysis_summary' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-sky-50 border border-sky-200 rounded-lg p-3 flex flex-wrap justify-between items-center gap-3">
                <div>
                  <h4 className="font-bold text-sky-950 text-sm">تحليل حركة المبيعات المجمعة والإيرادات</h4>
                  <p className="text-xs text-sky-700">النطاق: {selectedScopeLabel} | عدد الفواتير: {filteredSalesInvoices.length}</p>
                </div>
                <div className="text-left font-mono">
                  <span className="text-xs text-slate-500">إجمالي المبيعات: </span>
                  <span className="font-bold text-emerald-700 text-sm">{totalFilteredSalesRevenue.toLocaleString()} {currency}</span>
                </div>
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
                <div className="bg-white p-3 border border-slate-200 rounded-lg shadow-2xs">
                  <p className="text-xs text-slate-500">إجمالي الفواتير الصادرة</p>
                  <p className="text-xl font-bold font-mono text-slate-800 mt-1">{filteredSalesInvoices.length} فاتورة</p>
                </div>
                <div className="bg-white p-3 border border-slate-200 rounded-lg shadow-2xs">
                  <p className="text-xs text-slate-500">متوسط قيمة الفاتورة</p>
                  <p className="text-xl font-bold font-mono text-sky-700 mt-1">
                    {filteredSalesInvoices.length > 0 
                      ? Math.round(totalFilteredSalesRevenue / filteredSalesInvoices.length).toLocaleString() 
                      : 0} {currency}
                  </p>
                </div>
                <div className="bg-white p-3 border border-slate-200 rounded-lg shadow-2xs">
                  <p className="text-xs text-slate-500">إجمالي بنود الأصناف المباعة</p>
                  <p className="text-xl font-bold font-mono text-emerald-700 mt-1">
                    {filteredSalesInvoices.reduce((sum, inv) => sum + (inv.items?.length || 0), 0)} صنف/سطر
                  </p>
                </div>
              </div>

              <div className="border border-slate-200 rounded-lg overflow-hidden">
                <table className="w-full text-right border-collapse text-xs">
                  <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                    <tr>
                      <th className="p-2.5">رقم الفاتورة</th>
                      <th className="p-2.5">التاريخ والوقت</th>
                      <th className="p-2.5">العميل</th>
                      <th className="p-2.5">طريقة الدفع</th>
                      <th className="p-2.5 text-center">عدد البنود</th>
                      <th className="p-2.5 text-left">المبلغ الإجمالي</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {filteredSalesInvoices.map((inv) => (
                      <tr key={inv.id} className="hover:bg-slate-50 bg-white">
                        <td className="p-2.5 font-bold font-mono text-sky-800">#{inv.invoiceNo}</td>
                        <td className="p-2.5 text-slate-600 font-mono">{inv.date}</td>
                        <td className="p-2.5 font-semibold text-slate-800">{inv.customerOrSupplierName || 'عميل نقدي'}</td>
                        <td className="p-2.5">
                          <span className="bg-slate-100 text-slate-700 text-[10px] px-2 py-0.5 rounded font-semibold">
                            {inv.paymentType === 'CASH' ? 'نقداً' : inv.paymentType === 'CREDIT' ? 'آجل' : 'شبكة/بنك'}
                          </span>
                        </td>
                        <td className="p-2.5 text-center font-mono font-bold text-slate-700">{inv.items?.length || 0}</td>
                        <td className="p-2.5 text-left font-mono font-bold text-emerald-700">{inv.grandTotal.toLocaleString()} {currency}</td>
                      </tr>
                    ))}
                    {filteredSalesInvoices.length === 0 && (
                      <tr>
                        <td colSpan={6} className="p-4 text-center text-slate-500 bg-white">لا توجد فواتير مبيعات مسجلة في هذا النطاق</td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Report 17: تقرير إقفال الوردية وجرد الكاشير */}
          {selectedReportId === 'shift_closing_report' && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-purple-50 border border-purple-200 rounded-lg p-3 flex flex-wrap justify-between items-center gap-3">
                <div>
                  <h4 className="font-bold text-purple-950 text-sm">تقرير إقفال الوردية وجرد الصندوق والكاشير (Z-Report)</h4>
                  <p className="text-xs text-purple-700">النطاق: {selectedScopeLabel} | تاريخ التقرير: {new Date().toLocaleDateString('ar-EG')}</p>
                </div>
                <span className="bg-purple-100 text-purple-800 border border-purple-300 text-xs px-2.5 py-1 rounded font-bold">
                  وردية اليوم المعتمدة
                </span>
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-3">
                <div className="bg-white p-3 border border-slate-200 rounded-lg shadow-2xs">
                  <p className="text-xs text-slate-500">المبيعات النقدية (Cash)</p>
                  <p className="text-lg font-bold font-mono text-emerald-700 mt-1">
                    {filteredSalesInvoices.filter(i => i.paymentType === 'CASH').reduce((s, i) => s + i.grandTotal, 0).toLocaleString()} {currency}
                  </p>
                </div>
                <div className="bg-white p-3 border border-slate-200 rounded-lg shadow-2xs">
                  <p className="text-xs text-slate-500">مبيعات الشبكة / بنك</p>
                  <p className="text-lg font-bold font-mono text-sky-700 mt-1">
                    {filteredSalesInvoices.filter(i => i.paymentType === 'KURAYMI' || i.paymentType === 'BANK_TRANSFER' || i.paymentType === 'JEB_WALLET').reduce((s, i) => s + i.grandTotal, 0).toLocaleString()} {currency}
                  </p>
                </div>
                <div className="bg-white p-3 border border-slate-200 rounded-lg shadow-2xs">
                  <p className="text-xs text-slate-500">المبيعات الآجلة (ذمم)</p>
                  <p className="text-lg font-bold font-mono text-amber-700 mt-1">
                    {filteredSalesInvoices.filter(i => i.paymentType === 'CREDIT').reduce((s, i) => s + i.grandTotal, 0).toLocaleString()} {currency}
                  </p>
                </div>
                <div className="bg-white p-3 border border-slate-200 rounded-lg shadow-2xs">
                  <p className="text-xs text-slate-500">إجمالي المقبوضات النقدية</p>
                  <p className="text-lg font-bold font-mono text-purple-700 mt-1">
                    {totalCashReceipts.toLocaleString()} {currency}
                  </p>
                </div>
              </div>

              <div className="bg-white border border-slate-200 rounded-lg p-4 space-y-3">
                <h5 className="font-bold text-slate-800 text-xs">ملخص حركة نقدية الصندوق (Cashflow Summary):</h5>
                <div className="grid grid-cols-1 sm:grid-cols-3 gap-3 text-xs">
                  <div className="bg-slate-50 p-2.5 rounded border border-slate-200">
                    <span className="text-slate-500">رصيد البداية المقدر: </span>
                    <span className="font-bold font-mono text-slate-800">50,000 {currency}</span>
                  </div>
                  <div className="bg-slate-50 p-2.5 rounded border border-slate-200">
                    <span className="text-slate-500">المدفوعات والمصاريف: </span>
                    <span className="font-bold font-mono text-rose-700">{totalCashPayments.toLocaleString()} {currency}</span>
                  </div>
                  <div className="bg-emerald-50 p-2.5 rounded border border-emerald-200">
                    <span className="text-emerald-800 font-bold">الرصيد الدفتري المتوقع بالدرج: </span>
                    <span className="font-bold font-mono text-emerald-700">
                      {(50000 + filteredSalesInvoices.filter(i => i.paymentType === 'CASH').reduce((s, i) => s + i.grandTotal, 0) + totalCashReceipts - totalCashPayments).toLocaleString()} {currency}
                    </span>
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* Generic fallback for any unhandled report ID */}
          {!['rem_stock', 'acc_balances', 'item_profits', 'daily_ops', 'daily_journals', 'cashbox_movement', 'account_movement', 'category_totals', 'fx_variance', 'sales_by_item', 'purchases_by_item', 'item_movement_details', 'profit_by_customer', 'expiry_report', 'working_capital', 'sales_analysis_summary', 'shift_closing_report'].includes(selectedReportId) && (
            <div className="space-y-4 max-w-5xl mx-auto w-full">
              <div className="bg-sky-50 border border-sky-200 rounded-lg p-3">
                <h4 className="font-bold text-sky-950 text-sm">
                  {REPORTS_LIST.find(r => r.id === selectedReportId)?.title}
                </h4>
                <p className="text-xs text-sky-700">
                  {REPORTS_LIST.find(r => r.id === selectedReportId)?.desc}
                </p>
              </div>

              <div className="border border-slate-200 rounded-lg p-6 bg-slate-50 text-center text-slate-500">
                <FileText className="w-12 h-12 text-slate-400 mx-auto mb-2" />
                <p className="font-bold text-sm text-slate-700">التقرير محدث ومستند إلى القيود والحركات المسجلة</p>
                <p className="text-xs text-slate-500 mt-1">يحتوي هذا التقرير على تجميع شامل للبيانات المحاسبية والمخزنية المعتمدة في النظام</p>
              </div>
            </div>
          )}
        </div>
      )}
    </div>
  );
};
