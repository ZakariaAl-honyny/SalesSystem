'use client';

import React, { useState, useEffect, useRef, useCallback, useMemo } from 'react';
import { 
  FileText, 
  ShoppingCart, 
  Package, 
  Truck, 
  BookOpen, 
  BarChart3, 
  Calculator, 
  FileCode2, 
  Printer, 
  PauseCircle, 
  PlayCircle, 
  Settings, 
  LogOut, 
  Database, 
  FolderDown, 
  FolderUp, 
  FileSpreadsheet, 
  Users, 
  UserPlus, 
  CreditCard, 
  Receipt, 
  ArrowLeftRight, 
  RotateCcw, 
  AlertTriangle, 
  CheckSquare, 
  HelpCircle, 
  Layers, 
  ChevronLeft, 
  ChevronRight,
  Sparkles,
  Key,
  Laptop,
  Lock,
  UserCheck,
  Calendar,
  ClipboardCheck,
  SlidersHorizontal,
  ClipboardList,
  Table,
  CornerDownLeft,
  Scan,
  Search,
  Grid,
  ArrowDownToLine,
  DollarSign,
  Wallet,
  TrendingUp,
  Clock,
  Scale,
  User,
  RefreshCw,
} from 'lucide-react';
import { MainTabType } from '@/types/pos';

export interface MenuItemDef {
  id: string;
  label: string;
  mnemonicKey?: string; // e.g. 'F', 'S', 'I', 'P', 'A', 'R', 'T'
  shortcut?: string; // e.g. 'Ctrl+N', 'F1', 'Alt+F4'
  icon?: React.ReactNode;
  disabled?: boolean;
  checked?: boolean;
  action?: () => void;
  dividerBefore?: boolean;
  subItems?: MenuItemDef[];
}

export interface MenuCategoryDef {
  id: string;
  label: string;
  mnemonicLetter: string; // The character underlined in Arabic/English
  altKey: string; // e.g. 'f', 's', 'i', 'p', 'a', 'r', 't'
  items: MenuItemDef[];
}

interface WinFormsMenuStripProps {
  onSelectTab: (tab: MainTabType) => void;
  onNewBill: () => void;
  onHoldBill: () => void;
  onOpenHeldOrders: () => void;
  onOpenCalculator: () => void;
  onOpenArchitecture: () => void;
  onPrintReport: () => void;
  heldOrdersCount: number;
  onOpenLogin?: () => void;
  onLockScreen?: () => void;
  onOpenReportItem?: (reportId: string) => void;
  onOpenBackupRestore?: () => void;
  onOpenShowDialogShowcase?: () => void;
}

export const WinFormsMenuStrip: React.FC<WinFormsMenuStripProps> = ({
  onSelectTab,
  onNewBill,
  onHoldBill,
  onOpenHeldOrders,
  onOpenCalculator,
  onOpenArchitecture,
  onPrintReport,
  heldOrdersCount,
  onOpenLogin,
  onLockScreen,
  onOpenReportItem,
  onOpenBackupRestore,
  onOpenShowDialogShowcase,
}) => {
  const [activeMenuIndex, setActiveMenuIndex] = useState<number | null>(null);
  const [focusedItemIndex, setFocusedItemIndex] = useState<number>(0);
  const [activeSubmenuIndex, setActiveSubmenuIndex] = useState<number | null>(null);
  const [focusedSubItemIndex, setFocusedSubItemIndex] = useState<number>(0);
  const [isAltPressed, setIsAltPressed] = useState<boolean>(false);

  const menuContainerRef = useRef<HTMLDivElement>(null);

  // Build the complete WinForms Menu Hierarchy
  const menuCategories: MenuCategoryDef[] = useMemo(() => [
    {
      id: 'file',
      label: 'ملف',
      mnemonicLetter: 'م',
      altKey: 'f',
      items: [
        {
          id: 'file-new-bill',
          label: 'فاتورة مبيعات جديدة',
          shortcut: 'F1',
          icon: <ShoppingCart className="w-3.5 h-3.5 text-sky-400" />,
          action: () => {
            onSelectTab('POS');
            onNewBill();
          },
        },
        {
          id: 'file-hold-bill',
          label: 'تعليق الفاتورة الحالية',
          shortcut: 'F4',
          icon: <PauseCircle className="w-3.5 h-3.5 text-amber-400" />,
          action: onHoldBill,
        },
        {
          id: 'file-recall-held',
          label: `استرجاع الفواتير المعلقة (${heldOrdersCount})`,
          shortcut: 'Ctrl+H',
          icon: <PlayCircle className="w-3.5 h-3.5 text-emerald-400" />,
          action: onOpenHeldOrders,
        },
        {
          id: 'file-sep-1',
          label: '',
          dividerBefore: true,
        },
        {
          id: 'file-export-import',
          label: 'تصدير واستيراد البيانات',
          icon: <FolderDown className="w-3.5 h-3.5 text-slate-300" />,
          subItems: [
            {
              id: 'export-excel-accounts',
              label: 'تصدير كشف الحسابات (Excel)',
              shortcut: 'Ctrl+Shift+E',
              icon: <FileSpreadsheet className="w-3.5 h-3.5 text-emerald-400" />,
              action: () => alert('تم تصدير دليل الحسابات إلى ملف Excel بنجاح.'),
            },
            {
              id: 'export-csv-products',
              label: 'تصدير قائمة المنتجات (CSV)',
              icon: <FileText className="w-3.5 h-3.5 text-sky-400" />,
              action: () => alert('تم تصدير الأصناف إلى ملف CSV بنجاح.'),
            },
            {
              id: 'import-prices',
              label: 'استيراد تحديثات الأسعار من ملف',
              icon: <FolderUp className="w-3.5 h-3.5 text-amber-400" />,
              action: () => alert('يرجى اختيار ملف Excel لتحديث الأسعار تلقائياً.'),
            },
          ],
        },
        {
          id: 'file-database-backup',
          label: 'النسخ الاحتياطي لقاعدة البيانات (SQL Server)',
          icon: <Database className="w-3.5 h-3.5 text-sky-400" />,
          action: () => {
            if (onOpenBackupRestore) {
              onOpenBackupRestore();
            } else {
              alert('جاري فتح نافذة إدارة النسخ الاحتياطي لقاعدة بيانات SQL Server (YemenPOS_DB.bak)...');
            }
          },
        },
        {
          id: 'file-database-restore',
          label: 'استعادة قاعدة البيانات (SQL Server Restore)',
          icon: <RefreshCw className="w-3.5 h-3.5 text-rose-400" />,
          action: () => {
            if (onOpenBackupRestore) {
              onOpenBackupRestore();
            }
          },
        },
        {
          id: 'file-sep-2',
          label: '',
          dividerBefore: true,
        },
        {
          id: 'file-print-page',
          label: 'طباعة التقرير أو الشاشة الحالية',
          shortcut: 'Ctrl+P',
          icon: <Printer className="w-3.5 h-3.5 text-slate-300" />,
          action: onPrintReport,
        },
        {
          id: 'file-switch-user',
          label: 'تسجيل الدخول / تبديل المستخدم',
          shortcut: 'Ctrl+L',
          icon: <UserCheck className="w-3.5 h-3.5 text-sky-400" />,
          action: onOpenLogin,
        },
        {
          id: 'file-lock-screen',
          label: 'قفل شاشة العمل مؤقتاً',
          shortcut: 'Ctrl+Alt+L',
          icon: <Lock className="w-3.5 h-3.5 text-amber-400" />,
          action: onLockScreen,
        },
        {
          id: 'file-close',
          label: 'تسجيل الخروج وإغلاق النظام',
          shortcut: 'Alt+F4',
          icon: <LogOut className="w-3.5 h-3.5 text-rose-400" />,
          action: onOpenLogin || (() => alert('تم إغلاق جلسة الكاشير الحالية بنجاح.')),
        },
      ],
    },
    {
      id: 'sales',
      label: 'المبيعات',
      mnemonicLetter: 'ب',
      altKey: 's',
      items: [
        {
          id: 'sales-invoice-commercial',
          label: 'فاتورة مبيعات تجارية [frmSalesInvoice]',
          shortcut: 'F2',
          icon: <Receipt className="w-3.5 h-3.5 text-emerald-400" />,
          action: () => onSelectTab('SALES_INVOICE'),
        },
        {
          id: 'sales-strategies-lab-menu',
          label: 'مختبر مقارنة استراتيجيات الإدخال الـ 6 (UX Lab)',
          icon: <Sparkles className="w-3.5 h-3.5 text-amber-400" />,
          subItems: [
            {
              id: 'strat-lab-hub',
              label: 'لوحة مختبر وتقييم الاستراتيجيات',
              icon: <Layers className="w-3.5 h-3.5 text-indigo-400" />,
              action: () => onSelectTab('SALES_STRATEGIES_HUB'),
            },
            {
              id: 'strat-1-grid',
              label: '1. الجدول التفاعلي المباشر (Inline Grid)',
              icon: <Table className="w-3.5 h-3.5 text-emerald-400" />,
              action: () => onSelectTab('SALES_STRATEGY_INLINE_GRID'),
            },
            {
              id: 'strat-2-bar',
              label: '2. شريط الإدخال السريع العلوي (Rapid Bar)',
              icon: <CornerDownLeft className="w-3.5 h-3.5 text-sky-400" />,
              action: () => onSelectTab('SALES_STRATEGY_RAPID_BAR'),
            },
            {
              id: 'strat-3-scanner',
              label: '3. أسلوب المسح الضوئي (Continuous Scanner)',
              icon: <Scan className="w-3.5 h-3.5 text-emerald-400" />,
              action: () => onSelectTab('SALES_STRATEGY_SCANNER'),
            },
            {
              id: 'strat-4-f3',
              label: '4. البحث الذكي (Smart F3 Lookup)',
              icon: <Search className="w-3.5 h-3.5 text-purple-400" />,
              action: () => onSelectTab('SALES_STRATEGY_SMART_F3'),
            },
            {
              id: 'strat-5-touch',
              label: '5. شبكة اللمس والمرئيات (Touch Grid)',
              icon: <Grid className="w-3.5 h-3.5 text-amber-400" />,
              action: () => onSelectTab('SALES_STRATEGY_TOUCH_GRID'),
            },
            {
              id: 'strat-6-convert',
              label: '6. تحويل العروض بنقرة (Import/Convert)',
              icon: <ArrowDownToLine className="w-3.5 h-3.5 text-blue-400" />,
              action: () => onSelectTab('SALES_STRATEGY_IMPORT_CONVERT'),
            },
          ],
        },
        {
          id: 'sales-quotations',
          label: 'عروض الأسعار للعملاء [Quotations DataGrid]',
          shortcut: 'Ctrl+Q',
          icon: <FileText className="w-3.5 h-3.5 text-amber-400" />,
          action: () => onSelectTab('PRICE_QUOTATION'),
        },
        {
          id: 'sales-pos',
          label: 'شاشة نقطة البيع السريعة (POS)',
          shortcut: 'F1',
          icon: <ShoppingCart className="w-3.5 h-3.5 text-sky-400" />,
          action: () => onSelectTab('POS'),
        },
        {
          id: 'sales-returns',
          label: 'مردودات ومبيعات المرتجع [Sales Returns]',
          shortcut: 'Ctrl+R',
          icon: <RotateCcw className="w-3.5 h-3.5 text-rose-400" />,
          action: () => onSelectTab('SALES_RETURNS'),
        },
        {
          id: 'sales-sep-1',
          label: '',
          dividerBefore: true,
        },
        {
          id: 'sales-customers-sub',
          label: 'العملاء والذمم المدينة',
          icon: <Users className="w-3.5 h-3.5 text-purple-400" />,
          subItems: [
            {
              id: 'cust-list',
              label: 'دليل العملاء وسقوف الائتمان',
              shortcut: 'F4',
              icon: <Users className="w-3.5 h-3.5 text-slate-300" />,
              action: () => onSelectTab('CUSTOMERS'),
            },
            {
              id: 'cust-add',
              label: 'إضافة بطاقة عميل جديد',
              shortcut: 'Ctrl+Shift+C',
              icon: <UserPlus className="w-3.5 h-3.5 text-emerald-400" />,
              action: () => onSelectTab('CUSTOMERS'),
            },
            {
              id: 'cust-statement',
              label: 'كشف حساب عميل تفصيلي',
              icon: <Receipt className="w-3.5 h-3.5 text-sky-400" />,
              action: () => onSelectTab('CUSTOMERS'),
            },
          ],
        },
        {
          id: 'sales-shift-close',
          label: 'محضر إقفال الوردية وجرد الصندوق',
          shortcut: 'Ctrl+Shift+Z',
          icon: <CreditCard className="w-3.5 h-3.5 text-emerald-400" />,
          action: () => onSelectTab('REPORTS'),
        },
      ],
    },
    {
      id: 'inventory',
      label: 'المخازن',
      mnemonicLetter: 'خ',
      altKey: 'i',
      items: [
        {
          id: 'inv-items',
          label: 'بطاقة الأصناف والمخزون',
          shortcut: 'F5',
          icon: <Package className="w-3.5 h-3.5 text-sky-400" />,
          action: () => onSelectTab('INVENTORY'),
        },
        {
          id: 'inv-new-item',
          label: 'إضافة صنف جديد بالوحدات والباركود',
          shortcut: 'Ctrl+Shift+N',
          icon: <Package className="w-3.5 h-3.5 text-emerald-400" />,
          action: () => onSelectTab('INVENTORY'),
        },
        {
          id: 'inv-sep-1',
          label: '',
          dividerBefore: true,
        },
        {
          id: 'inv-stock-alerts',
          label: 'تنبيهات نواقص المخزون والصلاحيات',
          icon: <AlertTriangle className="w-3.5 h-3.5 text-rose-400" />,
          action: () => onSelectTab('INVENTORY'),
        },
        {
          id: 'inv-transfers',
          label: 'تحويلات ونقل بضاعة بين الفروع والمخازن',
          icon: <ArrowLeftRight className="w-3.5 h-3.5 text-amber-400" />,
          action: () => onSelectTab('INVENTORY'),
        },
        {
          id: 'inv-sep-2',
          label: '',
          dividerBefore: true,
        },
        {
          id: 'inv-stocktake',
          label: 'محاضر الجرد المخزني الفعلي ومطابقة الأرصدة',
          shortcut: 'F3',
          icon: <ClipboardCheck className="w-3.5 h-3.5 text-emerald-400" />,
          action: () => onSelectTab('INVENTORY'),
        },
        {
          id: 'inv-adjustments',
          label: 'سندات تسوية فروقات الجرد (عجز وفائض)',
          shortcut: 'F4',
          icon: <SlidersHorizontal className="w-3.5 h-3.5 text-purple-400" />,
          action: () => onSelectTab('INVENTORY'),
        },
        {
          id: 'inv-stock-cards',
          label: 'كرت حركة الصنف والرقابة المخزنية',
          shortcut: 'F5',
          icon: <BookOpen className="w-3.5 h-3.5 text-teal-400" />,
          action: () => onSelectTab('INVENTORY'),
        },
      ],
    },
    {
      id: 'purchases',
      label: 'المشتريات',
      mnemonicLetter: 'ش',
      altKey: 'p',
      items: [
        {
          id: 'pur-invoices',
          label: 'فواتير المشتريات وتوريد البضاعة',
          shortcut: 'F6',
          icon: <Truck className="w-3.5 h-3.5 text-sky-400" />,
          action: () => onSelectTab('PURCHASES'),
        },
        {
          id: 'pur-orders',
          label: 'أوامر وطلبات الشراء [Purchase Orders DataGrid]',
          shortcut: 'Ctrl+Shift+O',
          icon: <ClipboardList className="w-3.5 h-3.5 text-emerald-400" />,
          action: () => onSelectTab('PURCHASE_ORDER'),
        },
        {
          id: 'pur-suppliers',
          label: 'دليل الموردين والحسابات الدائنة',
          icon: <Users className="w-3.5 h-3.5 text-purple-400" />,
          action: () => onSelectTab('PURCHASES'),
        },
        {
          id: 'pur-returns',
          label: 'مردودات المشتريات إلى الموردين',
          icon: <RotateCcw className="w-3.5 h-3.5 text-rose-400" />,
          action: () => onSelectTab('PURCHASES'),
        },
      ],
    },
    {
      id: 'accounting',
      label: 'الحسابات',
      mnemonicLetter: 'ح',
      altKey: 'a',
      items: [
        {
          id: 'acc-chart',
          label: 'دليل الحسابات الشجري اليمني',
          shortcut: 'F7',
          icon: <BookOpen className="w-3.5 h-3.5 text-sky-400" />,
          action: () => onSelectTab('ACCOUNTING'),
        },
        {
          id: 'acc-journal',
          label: 'سجل قيود اليومية العامة المركبة',
          icon: <FileText className="w-3.5 h-3.5 text-emerald-400" />,
          action: () => onSelectTab('ACCOUNTING'),
        },
        {
          id: 'acc-sep-1',
          label: '',
          dividerBefore: true,
        },
        {
          id: 'acc-receipt-voucher',
          label: 'سند قبض نقدي / بنكي',
          shortcut: 'Ctrl+Shift+R',
          icon: <Receipt className="w-3.5 h-3.5 text-amber-400" />,
          action: () => onSelectTab('ACCOUNTING'),
        },
        {
          id: 'acc-payment-voucher',
          label: 'سند صرف نقدي / بنكي',
          shortcut: 'Ctrl+Shift+P',
          icon: <CreditCard className="w-3.5 h-3.5 text-rose-400" />,
          action: () => onSelectTab('ACCOUNTING'),
        },
        {
          id: 'acc-petty-cash',
          label: 'إدارة العهد المالية والتسويات',
          icon: <FileText className="w-3.5 h-3.5 text-sky-400" />,
          action: () => onSelectTab('PETTY_CASH'),
        },
        {
          id: 'acc-fiscal-periods',
          label: 'الفترات المالية والإقفال الدوري',
          shortcut: 'Ctrl+Shift+F',
          icon: <Calendar className="w-3.5 h-3.5 text-purple-400" />,
          action: () => onSelectTab('ACCOUNTING'),
        },
      ],
    },
    {
      id: 'reports',
      label: 'التقارير',
      mnemonicLetter: 'ت',
      altKey: 'r',
      items: [
        {
          id: 'rep-all-hub',
          label: 'مركز وشجرة التقارير الشاملة [18 تقريراً مصنفاً]',
          shortcut: 'Ctrl+Shift+R',
          icon: <BarChart3 className="w-3.5 h-3.5 text-sky-400 font-bold" />,
          action: () => {
            if (onOpenReportItem) onOpenReportItem('');
            onSelectTab('OTHER_REPORTS');
          },
        },
        {
          id: 'rep-dashboard',
          label: 'لوحة المؤشرات العامة التفاعلية (Dashboard)',
          shortcut: 'F8',
          icon: <BarChart3 className="w-3.5 h-3.5 text-sky-400" />,
          action: () => onSelectTab('REPORTS'),
        },
        {
          id: 'rep-sep-main',
          label: '',
          dividerBefore: true,
        },
        {
          id: 'rep-sub-sales',
          label: 'تقارير المبيعات والأرباح',
          icon: <TrendingUp className="w-3.5 h-3.5 text-emerald-400" />,
          subItems: [
            {
              id: 'sub-rep-sales-analysis',
              label: 'تحليل حركة المبيعات والأصناف الأكثر طلباً',
              icon: <BarChart3 className="w-3.5 h-3.5 text-emerald-400" />,
              action: () => onSelectTab('REPORTS'),
            },
            {
              id: 'sub-rep-pnl',
              label: 'قائمة الأرباح والخسائر التقديرية (P&L)',
              icon: <CreditCard className="w-3.5 h-3.5 text-amber-400" />,
              action: () => onSelectTab('REPORTS'),
            },
            {
              id: 'sub-rep-sales-by-item',
              label: 'المبيعات التفصيلية حسب الصنف',
              icon: <Package className="w-3.5 h-3.5 text-sky-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('sales_by_item');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-item-profits',
              label: 'أرباح وهوامش ربح الأصناف',
              icon: <TrendingUp className="w-3.5 h-3.5 text-emerald-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('item_profits');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-profit-by-customer',
              label: 'صافي الربح على مستوى العميل',
              icon: <User className="w-3.5 h-3.5 text-purple-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('profit_by_customer');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-category-totals',
              label: 'إجمالي المبيعات والمخزون حسب الأقسام',
              icon: <Layers className="w-3.5 h-3.5 text-amber-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('category_totals');
                onSelectTab('OTHER_REPORTS');
              },
            },
          ],
        },
        {
          id: 'rep-sub-inventory',
          label: 'تقارير المخزون وحركة الأصناف',
          icon: <Package className="w-3.5 h-3.5 text-sky-400" />,
          subItems: [
            {
              id: 'sub-rep-rem-stock',
              label: 'المخزون المتبقي وتقييم التكلفة',
              icon: <Package className="w-3.5 h-3.5 text-sky-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('rem_stock');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-item-movement',
              label: 'تفاصيل كرت حركة الصنف التراكمية',
              icon: <FileText className="w-3.5 h-3.5 text-teal-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('item_movement_details');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-purchases-by-item',
              label: 'المشتريات والتوريد حسب الصنف',
              icon: <Truck className="w-3.5 h-3.5 text-indigo-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('purchases_by_item');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-expiry',
              label: 'الأصناف وتواريخ الصلاحية والانتهاء',
              icon: <Calendar className="w-3.5 h-3.5 text-rose-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('expiry_report');
                onSelectTab('OTHER_REPORTS');
              },
            },
          ],
        },
        {
          id: 'rep-sub-accounting',
          label: 'تقارير الحسابات والمالية والقيود',
          icon: <BookOpen className="w-3.5 h-3.5 text-amber-400" />,
          subItems: [
            {
              id: 'sub-rep-acc-balances',
              label: 'أرصدة الحسابات وميزان المراجعة المصغر',
              icon: <Wallet className="w-3.5 h-3.5 text-amber-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('acc_balances');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-acc-movement',
              label: 'كشف حساب تفصيلي خلال فترة',
              icon: <Layers className="w-3.5 h-3.5 text-sky-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('account_movement');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-daily-journals',
              label: 'سجل قيود اليومية العامة المركبة',
              icon: <FileText className="w-3.5 h-3.5 text-emerald-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('daily_journals');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-working-capital',
              label: 'مؤشر رأس المال العامل والسيولة',
              icon: <TrendingUp className="w-3.5 h-3.5 text-purple-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('working_capital');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-fx-variance',
              label: 'فوارق أسعار صرف العملات [V2]',
              icon: <Scale className="w-3.5 h-3.5 text-slate-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('fx_variance');
                onSelectTab('OTHER_REPORTS');
              },
            },
          ],
        },
        {
          id: 'rep-sub-cashbox',
          label: 'تقارير الصندوق والعمليات اليومية',
          icon: <DollarSign className="w-3.5 h-3.5 text-purple-400" />,
          subItems: [
            {
              id: 'sub-rep-daily-ops',
              label: 'سجل العمليات اليومية الشاملة',
              icon: <Clock className="w-3.5 h-3.5 text-sky-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('daily_ops');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-cashbox-movement',
              label: 'حركة الصندوق والمقبوضات والمدفوعات',
              icon: <DollarSign className="w-3.5 h-3.5 text-amber-400" />,
              action: () => {
                if (onOpenReportItem) onOpenReportItem('cashbox_movement');
                onSelectTab('OTHER_REPORTS');
              },
            },
            {
              id: 'sub-rep-z-report',
              label: 'تقرير إقفال الوردية وجرد الصندوق (Z-Report)',
              icon: <Receipt className="w-3.5 h-3.5 text-purple-400" />,
              action: () => onSelectTab('REPORTS'),
            },
          ],
        },
      ],
    },
    {
      id: 'security',
      label: 'الأمان والموظفين',
      mnemonicLetter: 'أ',
      altKey: 'u',
      items: [
        {
          id: 'sec-employees',
          label: 'إدارة بطاقات الموظفين والكوادر (V1)',
          shortcut: 'Ctrl+E',
          icon: <UserCheck className="w-3.5 h-3.5 text-sky-400" />,
          action: () => onSelectTab('EMPLOYEES'),
        },
        {
          id: 'sec-users',
          label: 'إدارة المستخدمين وحسابات الكاشير',
          shortcut: 'Ctrl+U',
          icon: <Users className="w-3.5 h-3.5 text-purple-400" />,
          action: () => onSelectTab('SECURITY'),
        },
        {
          id: 'sec-roles',
          label: 'إدارة الأدوار ومصفوفة الصلاحيات (RBAC)',
          icon: <Key className="w-3.5 h-3.5 text-amber-400" />,
          action: () => onSelectTab('SECURITY'),
        },
        {
          id: 'sec-sessions',
          label: 'مراقبة الجلسات النشطة وإنهاء الجلسات',
          icon: <Laptop className="w-3.5 h-3.5 text-sky-400" />,
          action: () => onSelectTab('SECURITY'),
        },
      ],
    },
    {
      id: 'tools',
      label: 'الأدوات والمساعدة',
      mnemonicLetter: 'د',
      altKey: 't',
      items: [
        {
          id: 'tools-calc',
          label: 'الآلة الحاسبة السريعة',
          shortcut: 'F9',
          icon: <Calculator className="w-3.5 h-3.5 text-amber-400" />,
          action: onOpenCalculator,
        },
        {
          id: 'tools-showdialog-showcase',
          label: 'نوافذ ShowDialog والتركيز على الخطأ (MessageBox & ErrorProvider)',
          shortcut: 'F11',
          icon: <Sparkles className="w-3.5 h-3.5 text-amber-400" />,
          action: () => {
            if (onOpenShowDialogShowcase) onOpenShowDialogShowcase();
          },
        },
        {
          id: 'tools-arch',
          label: 'مخطط معمارية C# WinForms N-Layer',
          shortcut: 'F10',
          icon: <FileCode2 className="w-3.5 h-3.5 text-purple-400" />,
          action: onOpenArchitecture,
        },
        {
          id: 'tools-sep-1',
          label: '',
          dividerBefore: true,
        },
        {
          id: 'tools-about',
          label: 'حول نظام نقاط البيع والمحاسبة ERP',
          icon: <HelpCircle className="w-3.5 h-3.5 text-sky-400" />,
          action: () => alert('نظام إدارة المبيعات ونقاط البيع والمحاسبة اليمني v2.5\nمحاكاة دقيقة لبيئة C# .NET WinForms Desktop'),
        },
      ],
    },
  ], [onSelectTab, onOpenHeldOrders, onNewBill, onHoldBill, onOpenArchitecture, onOpenCalculator, onPrintReport, onOpenLogin, onLockScreen, onOpenReportItem, onOpenBackupRestore, onOpenShowDialogShowcase, heldOrdersCount]);

  // Helper to close all menus
  const closeAllMenus = useCallback(() => {
    setActiveMenuIndex(null);
    setActiveSubmenuIndex(null);
    setFocusedItemIndex(0);
    setFocusedSubItemIndex(0);
  }, []);

  // Click outside to close menus
  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (menuContainerRef.current && !menuContainerRef.current.contains(e.target as Node)) {
        closeAllMenus();
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [closeAllMenus]);

  // Global Alt & Keyboard Navigation Handler for WinForms MenuStrip
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      // Toggle Alt underlines
      if (e.key === 'Alt') {
        setIsAltPressed(true);
      }

      // Check Alt+Mnemonic combinations (e.g. Alt+F, Alt+S, Alt+I, Alt+P, Alt+A, Alt+R, Alt+T)
      if (e.altKey && !e.ctrlKey && !e.shiftKey) {
        const pressedKey = e.key.toLowerCase();
        const foundCategoryIdx = menuCategories.findIndex((c) => c.altKey === pressedKey);
        if (foundCategoryIdx !== -1) {
          e.preventDefault();
          setActiveMenuIndex(foundCategoryIdx);
          setFocusedItemIndex(0);
          setActiveSubmenuIndex(null);
          return;
        }
      }

      // If a menu is actively open, manage Arrow keys, Enter, Escape
      if (activeMenuIndex !== null) {
        const currentCategory = menuCategories[activeMenuIndex];
        const validItems = currentCategory.items.filter((item) => !item.dividerBefore || item.label);

        if (e.key === 'Escape') {
          e.preventDefault();
          if (activeSubmenuIndex !== null) {
            setActiveSubmenuIndex(null);
          } else {
            closeAllMenus();
          }
          return;
        }

        // Navigate Down in open menu
        if (e.key === 'ArrowDown') {
          e.preventDefault();
          if (activeSubmenuIndex !== null) {
            const currentItem = currentCategory.items[activeSubmenuIndex];
            const subItems = currentItem?.subItems || [];
            setFocusedSubItemIndex((prev) => (prev + 1) % subItems.length);
          } else {
            setFocusedItemIndex((prev) => (prev + 1) % currentCategory.items.length);
          }
          return;
        }

        // Navigate Up in open menu
        if (e.key === 'ArrowUp') {
          e.preventDefault();
          if (activeSubmenuIndex !== null) {
            const currentItem = currentCategory.items[activeSubmenuIndex];
            const subItems = currentItem?.subItems || [];
            setFocusedSubItemIndex((prev) => (prev - 1 + subItems.length) % subItems.length);
          } else {
            setFocusedItemIndex((prev) => (prev - 1 + currentCategory.items.length) % currentCategory.items.length);
          }
          return;
        }

        // RTL-Aware Horizontal Arrow Navigation:
        // In Arabic RTL: ArrowLeft opens next menu category or enters submenu; ArrowRight goes back / opens previous menu.
        if (e.key === 'ArrowLeft') {
          e.preventDefault();
          const currentItem = currentCategory.items[focusedItemIndex];
          if (currentItem && currentItem.subItems && activeSubmenuIndex === null) {
            // Open submenu
            setActiveSubmenuIndex(focusedItemIndex);
            setFocusedSubItemIndex(0);
          } else {
            // Move to next top menu category (in RTL, left is next)
            const nextIdx = (activeMenuIndex + 1) % menuCategories.length;
            setActiveMenuIndex(nextIdx);
            setFocusedItemIndex(0);
            setActiveSubmenuIndex(null);
          }
          return;
        }

        if (e.key === 'ArrowRight') {
          e.preventDefault();
          if (activeSubmenuIndex !== null) {
            // Close submenu and return to parent
            setActiveSubmenuIndex(null);
          } else {
            // Move to previous top menu category
            const prevIdx = (activeMenuIndex - 1 + menuCategories.length) % menuCategories.length;
            setActiveMenuIndex(prevIdx);
            setFocusedItemIndex(0);
            setActiveSubmenuIndex(null);
          }
          return;
        }

        // Enter key to trigger selected item
        if (e.key === 'Enter') {
          e.preventDefault();
          if (activeSubmenuIndex !== null) {
            const currentItem = currentCategory.items[activeSubmenuIndex];
            const selectedSubItem = currentItem?.subItems?.[focusedSubItemIndex];
            if (selectedSubItem && selectedSubItem.action && !selectedSubItem.disabled) {
              selectedSubItem.action();
              closeAllMenus();
            }
          } else {
            const selectedItem = currentCategory.items[focusedItemIndex];
            if (selectedItem) {
              if (selectedItem.subItems) {
                setActiveSubmenuIndex(focusedItemIndex);
                setFocusedSubItemIndex(0);
              } else if (selectedItem.action && !selectedItem.disabled) {
                selectedItem.action();
                closeAllMenus();
              }
            }
          }
          return;
        }
      }
    };

    const handleKeyUp = (e: KeyboardEvent) => {
      if (e.key === 'Alt') {
        setIsAltPressed(false);
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    window.addEventListener('keyup', handleKeyUp);
    return () => {
      window.removeEventListener('keydown', handleKeyDown);
      window.removeEventListener('keyup', handleKeyUp);
    };
  }, [activeMenuIndex, focusedItemIndex, activeSubmenuIndex, focusedSubItemIndex, menuCategories, closeAllMenus]);

  return (
    <div 
      ref={menuContainerRef} 
      className="bg-slate-100 border-b border-slate-200 text-xs text-slate-700 select-none relative z-50 flex items-center justify-between px-3 py-1 shadow-xs"
    >
      {/* MenuStrip Top-Level Bar */}
      <div className="flex items-center gap-1">
        {menuCategories.map((category, catIdx) => {
          const isOpen = activeMenuIndex === catIdx;

          return (
            <div key={category.id} className="relative">
              <button
                id={`menu-top-${category.id}`}
                onClick={() => {
                  if (isOpen) {
                    closeAllMenus();
                  } else {
                    setActiveMenuIndex(catIdx);
                    setFocusedItemIndex(0);
                    setActiveSubmenuIndex(null);
                  }
                }}
                onMouseEnter={() => {
                  if (activeMenuIndex !== null && activeMenuIndex !== catIdx) {
                    setActiveMenuIndex(catIdx);
                    setFocusedItemIndex(0);
                    setActiveSubmenuIndex(null);
                  }
                }}
                className={`px-3 py-1 rounded-md text-xs transition flex items-center gap-1.5 font-medium cursor-pointer ${
                  isOpen 
                    ? 'bg-sky-600 text-white shadow-xs' 
                    : 'text-slate-700 hover:bg-slate-200/80 hover:text-slate-900'
                }`}
              >
                <span>
                  {category.label}
                  {isAltPressed && (
                    <span className="text-[10px] text-sky-600 font-mono mr-1 font-bold">
                      (Alt+{category.altKey.toUpperCase()})
                    </span>
                  )}
                </span>
              </button>

              {/* First-level Dropdown Menu (WinForms ContextMenuStrip style) */}
              {isOpen && (
                <div 
                  className="absolute right-0 top-full mt-1.5 w-68 bg-white border border-slate-200 shadow-2xl rounded-xl py-1.5 text-slate-800 z-50 animate-in fade-in zoom-in-95 duration-100 ring-1 ring-black/5"
                  dir="rtl"
                >
                  {category.items.map((item, itemIdx) => {
                    if (item.dividerBefore && !item.label) {
                      return (
                        <div 
                          key={item.id} 
                          className="my-1.5 border-t border-slate-100 mx-2" 
                        />
                      );
                    }

                    const isFocused = focusedItemIndex === itemIdx;
                    const hasSubmenu = Boolean(item.subItems && item.subItems.length > 0);
                    const isSubmenuOpen = activeSubmenuIndex === itemIdx;

                    return (
                      <div key={item.id} className="relative px-1">
                        <button
                          id={`menuitem-${item.id}`}
                          onClick={() => {
                            if (hasSubmenu) {
                              setActiveSubmenuIndex(isSubmenuOpen ? null : itemIdx);
                              setFocusedSubItemIndex(0);
                            } else if (item.action && !item.disabled) {
                              item.action();
                              closeAllMenus();
                            }
                          }}
                          onMouseEnter={() => {
                            setFocusedItemIndex(itemIdx);
                            if (hasSubmenu) {
                              setActiveSubmenuIndex(itemIdx);
                              setFocusedSubItemIndex(0);
                            } else {
                              setActiveSubmenuIndex(null);
                            }
                          }}
                          disabled={item.disabled}
                          className={`w-full text-right px-3 py-1.5 rounded-lg flex items-center justify-between text-xs transition cursor-pointer ${
                            isFocused
                              ? 'bg-sky-50 text-sky-900 font-bold'
                              : 'text-slate-700 hover:bg-slate-50 hover:text-slate-900'
                          } ${item.disabled ? 'opacity-40 cursor-not-allowed' : ''}`}
                        >
                          <div className="flex items-center gap-2.5">
                            <span className={`w-4 h-4 flex items-center justify-center ${isFocused ? 'text-sky-600' : 'text-slate-500'}`}>
                              {item.icon}
                            </span>
                            <span>{item.label}</span>
                          </div>

                          <div className="flex items-center gap-1.5 text-[11px] text-slate-400">
                            {item.shortcut && (
                              <span className={`font-mono px-1.5 py-0.5 rounded text-[10px] ${isFocused ? 'bg-sky-100 text-sky-800 font-bold' : 'bg-slate-100 text-slate-500'}`}>
                                {item.shortcut}
                              </span>
                            )}
                            {hasSubmenu && (
                              <ChevronLeft className="w-3.5 h-3.5 text-slate-400" />
                            )}
                          </div>
                        </button>

                        {/* Nested Submenu (Level 2 Dropdown) */}
                        {hasSubmenu && isSubmenuOpen && item.subItems && (
                          <div 
                            className="absolute right-full top-0 mr-1 w-64 bg-white border border-slate-200 shadow-2xl rounded-xl py-1.5 text-slate-800 z-50 animate-in fade-in zoom-in-95 duration-100 ring-1 ring-black/5"
                            dir="rtl"
                          >
                            {item.subItems.map((sub, subIdx) => {
                              const isSubFocused = focusedSubItemIndex === subIdx;
                              return (
                                <div key={sub.id} className="px-1">
                                  <button
                                    id={`submenuitem-${sub.id}`}
                                    onClick={() => {
                                      if (sub.action && !sub.disabled) {
                                        sub.action();
                                        closeAllMenus();
                                      }
                                    }}
                                    onMouseEnter={() => setFocusedSubItemIndex(subIdx)}
                                    disabled={sub.disabled}
                                    className={`w-full text-right px-3 py-1.5 rounded-lg flex items-center justify-between text-xs transition cursor-pointer ${
                                      isSubFocused
                                        ? 'bg-sky-50 text-sky-900 font-bold'
                                        : 'text-slate-700 hover:bg-slate-50 hover:text-slate-900'
                                    } ${sub.disabled ? 'opacity-40 cursor-not-allowed' : ''}`}
                                  >
                                    <div className="flex items-center gap-2.5">
                                      <span className={`w-4 h-4 flex items-center justify-center ${isSubFocused ? 'text-sky-600' : 'text-slate-500'}`}>
                                        {sub.icon}
                                      </span>
                                      <span>{sub.label}</span>
                                    </div>
                                    {sub.shortcut && (
                                      <span className="text-[10px] font-mono bg-slate-100 text-slate-500 px-1.5 py-0.5 rounded">
                                        {sub.shortcut}
                                      </span>
                                    )}
                                  </button>
                                </div>
                              );
                            })}
                          </div>
                        )}
                      </div>
                    );
                  })}
                </div>
              )}
            </div>
          );
        })}
      </div>

      {/* WinForms Title & Alt Key Indicator Tip */}
      <div className="flex items-center gap-2 text-[11px] text-slate-500">
        <span className="bg-white border border-slate-200 px-2.5 py-0.5 rounded-md text-[10px] text-sky-700 font-mono font-bold shadow-2xs">
          قوائم النظام السريعة [Alt + الحرف]
        </span>
      </div>
    </div>
  );
};
