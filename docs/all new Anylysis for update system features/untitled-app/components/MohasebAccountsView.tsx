'use client';

import React, { useState, useMemo } from 'react';
import { AccountNode, JournalEntry } from '@/types/pos';
import { generateNextChildAccountCode, insertAccountIntoTree, getAllAccountsFlat } from '@/lib/mock/accounts';
import { SmartAccountCreationModal } from '@/components/accounting/SmartAccountCreationModal';
import {
  ArrowRight,
  Search,
  Plus,
  Filter,
  MoreVertical,
  ChevronDown,
  ChevronLeft,
  RotateCcw,
  FileText,
  X,
  Check,
  Folder,
  FolderOpen,
  FolderPlus,
  Sparkles,
  Layers,
  ChevronRight,
  Eye,
  EyeOff,
  ShieldCheck,
  FileSpreadsheet
} from 'lucide-react';

interface MohasebAccountsViewProps {
  initialSubView?: 'CHART_OF_ACCOUNTS' | 'CATEGORIES' | 'ACCOUNT_STATEMENT';
  chartOfAccounts: AccountNode[];
  journalEntries: JournalEntry[];
  onAddAccount?: (account: AccountNode) => void;
  onUpdateChartOfAccounts?: (updated: AccountNode[]) => void;
  onBack: () => void;
}

export const MohasebAccountsView: React.FC<MohasebAccountsViewProps> = ({
  initialSubView = 'CHART_OF_ACCOUNTS',
  chartOfAccounts,
  journalEntries,
  onAddAccount,
  onUpdateChartOfAccounts,
  onBack,
}) => {
  const [subView, setSubView] = useState<'CHART_OF_ACCOUNTS' | 'CATEGORIES' | 'ACCOUNT_STATEMENT'>(initialSubView);
  const [searchQuery, setSearchQuery] = useState('');
  const [activeRootFilter, setActiveRootFilter] = useState<'ALL' | '1' | '2' | '3' | '4'>('ALL');
  const [showBalances, setShowBalances] = useState(false);

  // Expanded nodes map (default: level 1 and 2 expanded)
  const [expandedNodes, setExpandedNodes] = useState<Record<string, boolean>>({
    '1': true,
    '2': true,
    '3': true,
    '4': true,
    '11': true,
    '12': false,
    '21': true,
    '22': false,
    '31': true,
    '41': true,
    '42': false,
  });

  // Smart Account Modal State
  const [isSmartModalOpen, setIsSmartModalOpen] = useState(false);
  const [selectedParentCodeForAdd, setSelectedParentCodeForAdd] = useState<string | undefined>(undefined);
  const [successToast, setSuccessToast] = useState<string | null>(null);

  // Popup filter list for "عام#الصناديق" (Screenshot 8)
  const [showAccountListPopup, setShowAccountListPopup] = useState(false);
  const [selectedAccountFilter, setSelectedAccountFilter] = useState('الصناديق(محلي)عام');

  // Categories state (Screenshot 7)
  const [categoriesList] = useState([
    { id: 'CAT-1', name: 'عام', accountsCount: 23 },
    { id: 'CAT-2', name: 'فروع', accountsCount: 5 },
    { id: 'CAT-3', name: 'مشاريع', accountsCount: 2 },
  ]);

  // Flattened accounts for analytics & statistics
  const flatAccounts = useMemo(() => {
    return getAllAccountsFlat(chartOfAccounts);
  }, [chartOfAccounts]);

  const treeStats = useMemo(() => {
    const total = flatAccounts.length;
    const parentCount = flatAccounts.filter((a) => !a.isLeaf || (a.children && a.children.length > 0)).length;
    const leafCount = total - parentCount;
    return { total, parentCount, leafCount };
  }, [flatAccounts]);

  // Filter and sort the tree recursively
  const sortedAndFilteredTree = useMemo(() => {
    const sortRecursive = (nodes: AccountNode[]): AccountNode[] => {
      const sorted = [...nodes].sort((a, b) => {
        const numA = parseInt(a.code, 10) || 0;
        const numB = parseInt(b.code, 10) || 0;
        return numA - numB;
      });

      return sorted.map((node) => {
        if (node.children && node.children.length > 0) {
          return {
            ...node,
            children: sortRecursive(node.children),
          };
        }
        return node;
      });
    };

    let baseNodes = chartOfAccounts;
    if (activeRootFilter !== 'ALL') {
      baseNodes = chartOfAccounts.filter((node) => node.code === activeRootFilter);
    }

    return sortRecursive(baseNodes);
  }, [chartOfAccounts, activeRootFilter]);

  // Toggle node expansion
  const toggleExpand = (code: string) => {
    setExpandedNodes((prev) => ({
      ...prev,
      [code]: !prev[code],
    }));
  };

  // Expand all nodes
  const handleExpandAll = () => {
    const newExpanded: Record<string, boolean> = {};
    flatAccounts.forEach((acc) => {
      newExpanded[acc.code] = true;
    });
    setExpandedNodes(newExpanded);
  };

  // Collapse all except root
  const handleCollapseAll = () => {
    const newExpanded: Record<string, boolean> = {
      '1': false,
      '2': false,
      '3': false,
      '4': false,
    };
    setExpandedNodes(newExpanded);
  };

  // Open Smart Creation Modal
  const handleOpenAddModal = (parentCode?: string) => {
    setSelectedParentCodeForAdd(parentCode);
    setIsSmartModalOpen(true);
  };

  // Save new account
  const handleSaveNewAccount = (newAcc: AccountNode) => {
    const updatedTree = insertAccountIntoTree(chartOfAccounts, newAcc);
    if (onUpdateChartOfAccounts) {
      onUpdateChartOfAccounts(updatedTree);
    } else if (onAddAccount) {
      onAddAccount(newAcc);
    }

    // Auto-expand parent so new child is visible
    if (newAcc.parentCode) {
      setExpandedNodes((prev) => ({
        ...prev,
        [newAcc.parentCode!]: true,
      }));
    }

    setSuccessToast(`تمت إضافة الحساب [${newAcc.code} - ${newAcc.name}] بنجاح إلى شجرة الحسابات`);
    setTimeout(() => setSuccessToast(null), 4000);
  };

  // Accounts list options for the popup (Exact match to Screenshot 8)
  const filterOptions = [
    'الصناديق(محلي)عام',
    'البضاعة(محلي)عام',
    'راس المال(محلي)عام',
    'الأرباح والخسائر(محلي)عام',
    'المشتريات(محلي)عام',
    'تسوية المخزون(محلي)عام',
    'مصاريف عمومية وادارية(محلي)عام',
    'المبيعات(محلي)عام',
    'الصناديق(دولار)عام',
    'العملاء(دولار)عام',
    'تسوية المخزون(دولار)عام',
    'المبيعات(دولار)عام',
  ];

  // Movements for the selected account in Screenshot 8
  const accountMovements = useMemo(() => {
    return [
      { id: '1', date: '2026-06-04', docNo: 'قيد#101', desc: 'مبيعات نقدية يومية', debit: 45000, credit: 0, balance: -214400 },
      { id: '2', date: '2026-06-03', docNo: 'شراء#88', desc: 'سداد مشتريات بضاعة نقد', debit: 0, credit: 120000, balance: -259400 },
      { id: '3', date: '2026-06-02', docNo: 'صرف#12', desc: 'مصاريف إدارية وضيافة', debit: 0, credit: 15000, balance: -139400 },
      { id: '4', date: '2026-06-01', docNo: 'قيد افتتاح', desc: 'رصيد الصندوق الافتتاحي', debit: 75000, credit: 0, balance: -124400 },
    ];
  }, []);

  // Recursive Tree Node Renderer
  const renderAccountNode = (node: AccountNode, depth: number = 0): React.ReactNode => {
    const hasChildren = Boolean(node.children && node.children.length > 0);
    const isExpanded = expandedNodes[node.code] ?? ((node.level ?? 1) <= 2);
    const isParent = hasChildren || !node.isLeaf;

    // Search query matching
    const query = searchQuery.trim().toLowerCase();
    const isMatchesSearch = query
      ? node.code.toLowerCase().includes(query) ||
        node.name.toLowerCase().includes(query) ||
        (node.nameEn && node.nameEn.toLowerCase().includes(query))
      : true;

    // If search is active and this node doesn't match and none of its descendants match, hide
    const hasMatchingDescendant = (n: AccountNode): boolean => {
      if (!query) return true;
      if (
        n.code.toLowerCase().includes(query) ||
        n.name.toLowerCase().includes(query) ||
        (n.nameEn && n.nameEn.toLowerCase().includes(query))
      ) {
        return true;
      }
      return Boolean(n.children?.some((child) => hasMatchingDescendant(child)));
    };

    if (query && !hasMatchingDescendant(node)) {
      return null;
    }

    return (
      <div key={node.code} className="flex flex-col select-none">
        {/* Account Row */}
        <div
          className={`group flex items-center justify-between py-2 px-3 hover:bg-sky-50/80 transition border-b border-slate-100 ${
            node.level === 1
              ? 'bg-slate-100/90 font-bold border-slate-300'
              : node.level === 2
              ? 'bg-slate-50/50'
              : 'bg-white'
          }`}
          style={{ paddingRight: `${Math.max(12, depth * 22 + 12)}px` }}
        >
          {/* Right Side: Expand Button + Icon + Code + Name + Badges */}
          <div className="flex items-center gap-2 flex-1 min-w-0">
            {/* Expand/Collapse Chevron */}
            {isParent ? (
              <button
                type="button"
                onClick={() => toggleExpand(node.code)}
                className="w-5 h-5 flex items-center justify-center rounded hover:bg-slate-200 text-slate-600 transition cursor-pointer shrink-0"
                title={isExpanded ? 'طي الفروع' : 'توسيع الفروع'}
              >
                {isExpanded ? (
                  <ChevronDown className="w-4 h-4 text-slate-700" />
                ) : (
                  <ChevronLeft className="w-4 h-4 text-slate-700" />
                )}
              </button>
            ) : (
              <span className="w-5 h-5 shrink-0" />
            )}

            {/* Folder / Document Icon */}
            {isParent ? (
              isExpanded ? (
                <FolderOpen className="w-4 h-4 text-amber-500 shrink-0" />
              ) : (
                <Folder className="w-4 h-4 text-amber-500 shrink-0" />
              )
            ) : (
              <FileText className="w-4 h-4 text-[#0277bd] shrink-0" />
            )}

            {/* Pure Integer Account Code */}
            <span
              className={`font-mono text-xs px-2 py-0.5 rounded-md shrink-0 ${
                node.level === 1
                  ? 'bg-[#0277bd] text-white font-bold text-xs'
                  : 'bg-slate-200/80 text-slate-800 font-bold text-[11px]'
              }`}
            >
              {node.code}
            </span>

            {/* Account Name */}
            <div className="flex items-baseline gap-1.5 truncate">
              <span
                className={`text-xs truncate ${
                  node.level === 1
                    ? 'text-slate-950 font-bold text-sm'
                    : node.level === 2
                    ? 'text-slate-900 font-bold'
                    : 'text-slate-800 font-medium'
                }`}
              >
                {node.name}
              </span>
              {node.nameEn && (
                <span className="text-[10px] text-slate-400 font-mono hidden md:inline truncate">
                  ({node.nameEn})
                </span>
              )}
            </div>

            {/* Classification Badges */}
            <div className="hidden sm:flex items-center gap-1.5 shrink-0 mr-2">
              {/* Type Badge */}
              <span
                className={`text-[10px] px-1.5 py-0.5 rounded font-bold ${
                  isParent
                    ? 'bg-slate-200 text-slate-700'
                    : 'bg-emerald-50 text-emerald-700 border border-emerald-200'
                }`}
              >
                {isParent ? 'رئيسي تجميعي' : 'فرعي حركي'}
              </span>

              {/* Nature Badge */}
              <span
                className={`text-[10px] px-1.5 py-0.5 rounded font-semibold ${
                  node.nature === 'DEBIT'
                    ? 'bg-sky-50 text-[#0277bd]'
                    : 'bg-amber-50 text-amber-800'
                }`}
              >
                {node.nature === 'DEBIT' ? 'مدين' : 'دائن'}
              </span>

              {/* Level indicator */}
              <span className="text-[10px] text-slate-400 font-mono">
                م{node.level}
              </span>
            </div>
          </div>

          {/* Left Side: Actions & Optional Balance */}
          <div className="flex items-center gap-2 shrink-0">
            {/* Optional Balance View */}
            {showBalances && (
              <span className="text-xs font-mono font-bold text-slate-700 ml-2">
                {Number(node.balance || 0).toLocaleString()} <span className="text-[10px] text-slate-400">ر.ي</span>
              </span>
            )}

            {/* Quick Add Sub-Account Button */}
            <button
              type="button"
              onClick={() => handleOpenAddModal(node.code)}
              className="opacity-0 group-hover:opacity-100 focus:opacity-100 transition px-2 py-1 bg-white hover:bg-sky-50 text-[#0277bd] border border-sky-200 hover:border-[#0277bd] rounded-md text-[11px] font-bold flex items-center gap-1 shadow-2xs cursor-pointer"
              title={`إضافة حساب فرعي متفرع من [${node.name}]`}
            >
              <Plus className="w-3 h-3" />
              <span>إضافة فرع</span>
            </button>
          </div>
        </div>

        {/* Child Nodes */}
        {hasChildren && isExpanded && (
          <div className="flex flex-col border-r-2 border-slate-200/70 mr-4">
            {node.children!.map((child) => renderAccountNode(child, depth + 1))}
          </div>
        )}
      </div>
    );
  };

  return (
    <div id="mohaseb-accounts-view" className="flex flex-col h-full bg-slate-50 font-sans select-none relative" dir="rtl">
      {/* ============================================================== */}
      {/* 1. TOP HEADER BAR                                               */}
      {/* ============================================================== */}
      <header className="bg-[#0277bd] text-white px-4 py-3 flex items-center justify-between shadow-sm shrink-0">
        <div className="flex items-center gap-3">
          <button
            onClick={onBack}
            className="p-1 hover:bg-white/20 rounded-full transition cursor-pointer text-white"
            title="رجوع للرئيسية"
          >
            <ArrowRight className="w-6 h-6" />
          </button>

          <div>
            <h1 className="text-base sm:text-lg font-bold flex items-center gap-2">
              <span>شجرة الحسابات الهرمية (دليل الحسابات)</span>
              <span className="text-[11px] bg-white/20 text-white px-2 py-0.5 rounded-full font-bold">
                محاسب سوفت
              </span>
            </h1>
            <p className="text-[11px] text-sky-100 hidden sm:block">
              عرض وترتيب شجرة الحسابات مع مساعد التأسيس الذكي لتقليل الأخطاء
            </p>
          </div>
        </div>

        {/* Right Header Actions */}
        <div className="flex items-center gap-2">
          {/* Add Account Main Button */}
          <button
            onClick={() => handleOpenAddModal()}
            className="bg-amber-400 hover:bg-amber-500 text-slate-950 font-bold px-3 py-1.5 rounded-xl text-xs flex items-center gap-1.5 shadow-sm transition active:scale-95 cursor-pointer"
          >
            <Plus className="w-4 h-4 stroke-[3]" />
            <span>إضافة حساب جديد</span>
          </button>

          {/* Sub-view switcher for legacy categories/statement if needed */}
          <button
            onClick={() => setSubView(subView === 'CHART_OF_ACCOUNTS' ? 'CATEGORIES' : 'CHART_OF_ACCOUNTS')}
            className="p-1.5 hover:bg-white/20 rounded-lg transition text-white"
            title="تبديل العرض"
          >
            <MoreVertical className="w-5 h-5" />
          </button>
        </div>
      </header>

      {/* ============================================================== */}
      {/* 2. SUCCESS NOTIFICATION TOAST                                   */}
      {/* ============================================================== */}
      {successToast && (
        <div className="bg-emerald-600 text-white px-4 py-2 text-xs font-bold flex items-center justify-between shadow-md shrink-0 animate-in fade-in duration-200">
          <div className="flex items-center gap-2">
            <Check className="w-4 h-4 text-emerald-200" />
            <span>{successToast}</span>
          </div>
          <button onClick={() => setSuccessToast(null)} className="text-white/80 hover:text-white">
            <X className="w-4 h-4" />
          </button>
        </div>
      )}

      {/* ============================================================== */}
      {/* 3. TOOLBAR (SEARCH, ROOT FILTERS, CONTROLS)                     */}
      {/* ============================================================== */}
      {subView === 'CHART_OF_ACCOUNTS' && (
        <div className="bg-white border-b border-slate-200 px-4 py-2.5 flex flex-wrap items-center justify-between gap-2 shrink-0">
          {/* Search Box */}
          <div className="relative w-full sm:w-64">
            <Search className="w-4 h-4 text-slate-400 absolute right-2.5 top-1/2 -translate-y-1/2" />
            <input
              type="text"
              placeholder="بحث باسم أو رقم الحساب..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="w-full bg-slate-50 border border-slate-300 rounded-xl pr-8 pl-8 py-1.5 text-xs text-slate-900 outline-hidden focus:ring-2 focus:ring-[#0277bd] focus:bg-white transition"
            />
            {searchQuery && (
              <button
                onClick={() => setSearchQuery('')}
                className="absolute left-2.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600"
              >
                <X className="w-3.5 h-3.5" />
              </button>
            )}
          </div>

          {/* Root Categories Filter Pills */}
          <div className="flex items-center gap-1 overflow-x-auto py-1">
            <button
              type="button"
              onClick={() => setActiveRootFilter('ALL')}
              className={`px-2.5 py-1 rounded-lg text-xs font-bold transition cursor-pointer ${
                activeRootFilter === 'ALL'
                  ? 'bg-[#0277bd] text-white shadow-2xs'
                  : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
              }`}
            >
              الكل ({treeStats.total})
            </button>
            <button
              type="button"
              onClick={() => setActiveRootFilter('1')}
              className={`px-2.5 py-1 rounded-lg text-xs font-bold transition cursor-pointer ${
                activeRootFilter === '1'
                  ? 'bg-[#0277bd] text-white shadow-2xs'
                  : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
              }`}
            >
              1 الأصول
            </button>
            <button
              type="button"
              onClick={() => setActiveRootFilter('2')}
              className={`px-2.5 py-1 rounded-lg text-xs font-bold transition cursor-pointer ${
                activeRootFilter === '2'
                  ? 'bg-[#0277bd] text-white shadow-2xs'
                  : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
              }`}
            >
              2 الخصوم
            </button>
            <button
              type="button"
              onClick={() => setActiveRootFilter('3')}
              className={`px-2.5 py-1 rounded-lg text-xs font-bold transition cursor-pointer ${
                activeRootFilter === '3'
                  ? 'bg-[#0277bd] text-white shadow-2xs'
                  : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
              }`}
            >
              3 الإيرادات
            </button>
            <button
              type="button"
              onClick={() => setActiveRootFilter('4')}
              className={`px-2.5 py-1 rounded-lg text-xs font-bold transition cursor-pointer ${
                activeRootFilter === '4'
                  ? 'bg-[#0277bd] text-white shadow-2xs'
                  : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
              }`}
            >
              4 المصروفات
            </button>
          </div>

          {/* Tree Control Buttons */}
          <div className="flex items-center gap-1.5">
            <button
              type="button"
              onClick={handleExpandAll}
              className="px-2 py-1 rounded-md text-[11px] bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold transition cursor-pointer"
              title="توسيع كافة فروع الشجرة"
            >
              توسيع الكل
            </button>
            <button
              type="button"
              onClick={handleCollapseAll}
              className="px-2 py-1 rounded-md text-[11px] bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold transition cursor-pointer"
              title="طي فروع الشجرة"
            >
              طي الكل
            </button>
            <button
              type="button"
              onClick={() => setShowBalances(!showBalances)}
              className={`px-2 py-1 rounded-md text-[11px] font-bold transition cursor-pointer flex items-center gap-1 ${
                showBalances
                  ? 'bg-sky-100 text-[#0277bd] border border-sky-300'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-600'
              }`}
              title="إظهار أو إخفاء الأرصدة في الشجرة"
            >
              {showBalances ? <EyeOff className="w-3 h-3" /> : <Eye className="w-3 h-3" />}
              <span>{showBalances ? 'إخفاء الأرصدة' : 'عرض الأرصدة'}</span>
            </button>
          </div>
        </div>
      )}

      {/* ============================================================== */}
      {/* 4. MAIN CONTENT VIEW                                            */}
      {/* ============================================================== */}
      <main className="flex-1 overflow-y-auto bg-white">
        {subView === 'CHART_OF_ACCOUNTS' && (
          <div className="w-full flex flex-col">
            {/* Header info strip */}
            <div className="bg-slate-50 border-b border-slate-200 px-4 py-2 flex items-center justify-between text-xs text-slate-600">
              <div className="flex items-center gap-2">
                <span className="font-bold text-slate-800">هيكل شجرة الحسابات المحاسبية</span>
                <span className="text-slate-400">|</span>
                <span className="text-slate-600">
                  {treeStats.parentCount} حساب رئيسي تجميعي • {treeStats.leafCount} حساب فرعي حركي
                </span>
              </div>
              <div className="text-[11px] text-emerald-700 font-bold flex items-center gap-1">
                <ShieldCheck className="w-3.5 h-3.5" />
                <span>أرقام متسلسلة صحيحة (Pure Integers)</span>
              </div>
            </div>

            {/* Tree Nodes List */}
            <div className="divide-y divide-slate-100">
              {sortedAndFilteredTree.length > 0 ? (
                sortedAndFilteredTree.map((node) => renderAccountNode(node, 0))
              ) : (
                <div className="p-8 text-center text-slate-500 space-y-2">
                  <Folder className="w-8 h-8 text-slate-300 mx-auto" />
                  <p className="font-bold text-sm">لا توجد حسابات تطابق معايير البحث أو الفلترة</p>
                  <button
                    onClick={() => {
                      setSearchQuery('');
                      setActiveRootFilter('ALL');
                    }}
                    className="text-xs text-[#0277bd] hover:underline font-bold"
                  >
                    إعادة ضبط الفلترة
                  </button>
                </div>
              )}
            </div>
          </div>
        )}

        {/* ============================================================== */}
        {/* VIEW 2: التصنيفات (CATEGORIES)                                  */}
        {/* ============================================================== */}
        {subView === 'CATEGORIES' && (
          <div className="p-4 space-y-3">
            <div className="flex items-center justify-between mb-2">
              <h2 className="font-bold text-sm text-slate-800">تصنيفات شجرة الحسابات</h2>
              <button
                onClick={() => setSubView('CHART_OF_ACCOUNTS')}
                className="text-xs bg-[#0277bd] text-white px-3 py-1.5 rounded-lg font-bold"
              >
                العودة لشجرة الحسابات
              </button>
            </div>
            <div className="bg-white rounded-xl border border-slate-200 divide-y divide-slate-100">
              {categoriesList.map((cat) => (
                <div key={cat.id} className="p-3 flex items-center justify-between hover:bg-slate-50">
                  <span className="font-bold text-slate-800">{cat.name}</span>
                  <span className="text-xs text-slate-500 font-mono">{cat.accountsCount} حساب</span>
                </div>
              ))}
            </div>
          </div>
        )}

        {/* ============================================================== */}
        {/* VIEW 3: كشف حساب الصناديق (ACCOUNT_STATEMENT)                   */}
        {/* ============================================================== */}
        {subView === 'ACCOUNT_STATEMENT' && (
          <div className="p-4 space-y-3">
            <div className="flex items-center justify-between mb-2">
              <h2 className="font-bold text-sm text-slate-800">كشف حساب: {selectedAccountFilter}</h2>
              <button
                onClick={() => setSubView('CHART_OF_ACCOUNTS')}
                className="text-xs bg-[#0277bd] text-white px-3 py-1.5 rounded-lg font-bold"
              >
                العودة للشجرة
              </button>
            </div>
            <div className="bg-white rounded-xl border border-slate-200 overflow-hidden">
              <table className="w-full text-right text-xs">
                <thead className="bg-slate-100 text-slate-700 font-bold">
                  <tr>
                    <th className="p-2.5">التاريخ</th>
                    <th className="p-2.5">رقم السند</th>
                    <th className="p-2.5">البيان</th>
                    <th className="p-2.5 text-left">مدين</th>
                    <th className="p-2.5 text-left">دائن</th>
                    <th className="p-2.5 text-left">الرصيد</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {accountMovements.map((m) => (
                    <tr key={m.id} className="hover:bg-slate-50">
                      <td className="p-2.5">{m.date}</td>
                      <td className="p-2.5 font-bold font-mono">{m.docNo}</td>
                      <td className="p-2.5">{m.desc}</td>
                      <td className="p-2.5 text-left font-mono">{m.debit ? m.debit.toLocaleString() : '-'}</td>
                      <td className="p-2.5 text-left font-mono">{m.credit ? m.credit.toLocaleString() : '-'}</td>
                      <td className="p-2.5 text-left font-mono font-bold">{m.balance.toLocaleString()}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}
      </main>

      {/* ============================================================== */}
      {/* 5. SMART ACCOUNT CREATION MODAL                                 */}
      {/* ============================================================== */}
      <SmartAccountCreationModal
        isOpen={isSmartModalOpen}
        onClose={() => {
          setIsSmartModalOpen(false);
          setSelectedParentCodeForAdd(undefined);
        }}
        chartOfAccounts={chartOfAccounts}
        initialParentCode={selectedParentCodeForAdd}
        onSaveAccount={handleSaveNewAccount}
      />
    </div>
  );
};
