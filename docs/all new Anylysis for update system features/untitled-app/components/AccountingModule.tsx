'use client';

import React, { useState, useMemo } from 'react';
import {
  AccountingAddAccountModal,
  AccountingEditAccountModal,
  AccountingAddCostCenterModal,
  AccountingEditCostCenterModal,
  AccountingNewEntryModal,
} from './accounting';
import {
  AccountNode,
  CostCenterNode,
  JournalEntry,
  JournalEntryLine,
  Customer,
  Supplier,
  CashVoucher,
  FiscalPeriod,
  MainTabType,
} from '@/types/pos';
import {
  BookOpen,
  Folder,
  FolderOpen,
  FileText,
  ChevronDown,
  ChevronRight,
  ChevronLeft,
  Plus,
  Trash2,
  Edit3,
  CheckCircle2,
  AlertCircle,
  Search,
  Layers,
  Building2,
  FileSpreadsheet,
  RefreshCw,
  SlidersHorizontal,
  X,
  ExternalLink,
  DollarSign,
  Receipt,
  Calendar,
  Lock,
  Wallet,
  Landmark,
  Coins,
  TrendingUp,
  Eye,
  EyeOff,
} from 'lucide-react';
import { CashVouchersView } from './CashVouchersView';
import { FiscalPeriodsView } from './FiscalPeriodsView';
import { INITIAL_VOUCHERS, INITIAL_FISCAL_PERIODS, calculateTreeBalances, generateNextChildAccountCode } from '@/lib/mock';

interface AccountingModuleProps {
  chartOfAccounts: AccountNode[];
  journalEntries: JournalEntry[];
  costCenters?: CostCenterNode[];
  vouchers?: CashVoucher[];
  fiscalPeriods?: FiscalPeriod[];
  customers: Customer[];
  suppliers: Supplier[];
  onAddJournalEntry: (entry: JournalEntry) => void;
  onAddVoucher?: (voucher: CashVoucher, generatedEntry: JournalEntry) => void;
  onVoidVoucher?: (voucherId: string, reason: string) => void;
  onCloseFiscalPeriod?: (periodId: string, closingEntry: JournalEntry, notes: string) => void;
  onReopenFiscalPeriod?: (periodId: string) => void;
  onUpdateChartOfAccounts?: (accounts: AccountNode[]) => void;
  onUpdateCostCenters?: (costCenters: CostCenterNode[]) => void;
  currency: string;
  defaultCashboxCode?: string;
  onSelectTab?: (tab: MainTabType) => void;
}

// --- FLAT HELPERS ---
const getAllAccountsFlat = (nodes: AccountNode[]): AccountNode[] => {
  let list: AccountNode[] = [];
  nodes.forEach((n) => {
    list.push(n);
    if (n.children && n.children.length > 0) {
      list = list.concat(getAllAccountsFlat(n.children));
    }
  });
  return list;
};

const getLeafAccounts = (nodes: AccountNode[]): AccountNode[] => {
  let list: AccountNode[] = [];
  nodes.forEach((n) => {
    if (!n.children || n.children.length === 0) {
      list.push(n);
    } else {
      list = list.concat(getLeafAccounts(n.children));
    }
  });
  return list;
};

const getAllCostCentersFlat = (nodes: CostCenterNode[]): CostCenterNode[] => {
  let list: CostCenterNode[] = [];
  nodes.forEach((c) => {
    list.push(c);
    if (c.children && c.children.length > 0) {
      list = list.concat(getAllCostCentersFlat(c.children));
    }
  });
  return list;
};

const getLeafCostCenters = (nodes: CostCenterNode[]): CostCenterNode[] => {
  let list: CostCenterNode[] = [];
  nodes.forEach((c) => {
    if (!c.children || c.children.length === 0 || c.isLeaf) {
      list.push(c);
    } else {
      list = list.concat(getLeafCostCenters(c.children));
    }
  });
  return list;
};

export const AccountingModule: React.FC<AccountingModuleProps> = ({
  chartOfAccounts,
  journalEntries,
  costCenters: initialCostCenters = [],
  vouchers: propVouchers,
  fiscalPeriods: propFiscalPeriods,
  customers,
  suppliers,
  onAddJournalEntry,
  onAddVoucher,
  onVoidVoucher,
  onCloseFiscalPeriod,
  onReopenFiscalPeriod,
  onUpdateChartOfAccounts,
  onUpdateCostCenters,
  currency,
  defaultCashboxCode,
  onSelectTab,
}) => {
  // Navigation Tabs
  const [activeTab, setActiveTab] = useState<
    | 'CHART_OF_ACCOUNTS'
    | 'COST_CENTERS'
    | 'CASH_VOUCHERS'
    | 'JOURNAL_ENTRIES'
    | 'FISCAL_PERIODS'
    | 'ACCOUNT_STATEMENT'
    | 'TRIAL_BALANCE'
  >('CHART_OF_ACCOUNTS');

  // Internal fallback state for accounts, cost centers, vouchers, and fiscal periods
  const [internalAccounts, setInternalAccounts] = useState<AccountNode[]>(chartOfAccounts);
  const [internalCostCenters, setInternalCostCenters] = useState<CostCenterNode[]>(initialCostCenters);
  const [internalVouchers, setInternalVouchers] = useState<CashVoucher[]>(propVouchers || INITIAL_VOUCHERS);
  const [internalFiscalPeriods, setInternalFiscalPeriods] = useState<FiscalPeriod[]>(propFiscalPeriods || INITIAL_FISCAL_PERIODS);

  // Derived active trees and lists
  const localAccounts = onUpdateChartOfAccounts ? chartOfAccounts : internalAccounts;
  const localCostCenters =
    onUpdateCostCenters && initialCostCenters && initialCostCenters.length > 0
      ? initialCostCenters
      : internalCostCenters;
  const localVouchers = propVouchers || internalVouchers;
  const localFiscalPeriods = propFiscalPeriods || internalFiscalPeriods;

  // Notifications / Feedback message banner
  const [feedbackMsg, setFeedbackMsg] = useState<{ text: string; type: 'SUCCESS' | 'ERROR' } | null>(null);
  const showFeedback = (text: string, type: 'SUCCESS' | 'ERROR' = 'SUCCESS') => {
    setFeedbackMsg({ text, type });
    setTimeout(() => setFeedbackMsg(null), 4000);
  };

  // Internal handlers for Cash Vouchers
  const handleAddVoucherInternal = (voucher: CashVoucher, generatedEntry: JournalEntry) => {
    if (onAddVoucher) {
      onAddVoucher(voucher, generatedEntry);
    } else {
      setInternalVouchers((prev) => [voucher, ...prev]);
      onAddJournalEntry(generatedEntry);
    }

    // Update account balances in tree
    const updateBalancesInTree = (nodes: AccountNode[]): AccountNode[] => {
      return nodes.map((node) => {
        let newBal = node.balance;
        generatedEntry.lines.forEach((l) => {
          if (l.accountCode === node.code) {
            if (node.nature === 'DEBIT') {
              newBal += l.debit - l.credit;
            } else {
              newBal += l.credit - l.debit;
            }
          }
        });
        const updatedChildren = node.children ? updateBalancesInTree(node.children) : undefined;
        return {
          ...node,
          balance: newBal,
          children: updatedChildren,
        };
      });
    };

    const updatedAccounts = updateBalancesInTree(localAccounts);
    setInternalAccounts(updatedAccounts);
    if (onUpdateChartOfAccounts) onUpdateChartOfAccounts(updatedAccounts);

    showFeedback(`تم إصدار وترحيل السند بنجاح برقم ${voucher.voucherNo} وإنشاء القيد الآلي ${generatedEntry.entryNo}`);
  };

  const handleVoidVoucherInternal = (voucherId: string, reason: string) => {
    if (onVoidVoucher) {
      onVoidVoucher(voucherId, reason);
    } else {
      setInternalVouchers((prev) =>
        prev.map((v) =>
          v.id === voucherId
            ? { ...v, status: 'VOID' as const, voidReason: reason, voidedAt: new Date().toISOString() }
            : v
        )
      );
    }
    showFeedback(`تم إلغاء السند المالي بنجاح وإثبات عملية الإلغاء في سجل الرقابة المحاسبية.`);
  };

  // Internal handlers for Fiscal Periods
  const handleClosePeriodInternal = (periodId: string, closingEntry: JournalEntry, notes: string) => {
    if (onCloseFiscalPeriod) {
      onCloseFiscalPeriod(periodId, closingEntry, notes);
    } else {
      setInternalFiscalPeriods((prev) =>
        prev.map((p) =>
          p.id === periodId
            ? {
                ...p,
                status: 'CLOSED' as const,
                isClosed: true,
                closedAt: new Date().toLocaleString('ar-YE'),
                closedBy: 'admin',
                closingNotes: notes,
                closingEntryId: closingEntry.id,
              }
            : p
        )
      );
      onAddJournalEntry(closingEntry);
    }

    // Reset revenue and expense account balances to zero in tree, and update retained earnings (3201)
    const resetNominalAccountsInTree = (nodes: AccountNode[]): AccountNode[] => {
      return nodes.map((node) => {
        let newBal = node.balance;
        if (node.type === 'REVENUE' || node.type === 'EXPENSE') {
          newBal = 0;
        } else if (node.code === '3201') {
          const closingEquityLine = closingEntry.lines.find((l) => l.accountCode === '3201');
          if (closingEquityLine) {
            newBal += closingEquityLine.credit - closingEquityLine.debit;
          }
        }
        const updatedChildren = node.children ? resetNominalAccountsInTree(node.children) : undefined;
        return {
          ...node,
          balance: newBal,
          children: updatedChildren,
        };
      });
    };

    const updatedAccounts = resetNominalAccountsInTree(localAccounts);
    setInternalAccounts(updatedAccounts);
    if (onUpdateChartOfAccounts) onUpdateChartOfAccounts(updatedAccounts);

    showFeedback(`تم إقفال الفترة المالية بنجاح وترحيل قيد الإقفال الختامي ${closingEntry.entryNo}`);
  };

  const handleReopenPeriodInternal = (periodId: string) => {
    if (onReopenFiscalPeriod) {
      onReopenFiscalPeriod(periodId);
    } else {
      setInternalFiscalPeriods((prev) =>
        prev.map((p) =>
          p.id === periodId
            ? { ...p, status: 'OPEN' as const, isClosed: false }
            : p
        )
      );
    }
    showFeedback(`تمت إعادة فتح الفترة المالية بنجاح للتدقيق المحاسبي.`);
  };

  const allAccountsFlat = useMemo(() => getAllAccountsFlat(localAccounts), [localAccounts]);
  const leafAccounts = useMemo(() => getLeafAccounts(localAccounts), [localAccounts]);
  const allCostCentersFlat = useMemo(() => getAllCostCentersFlat(localCostCenters), [localCostCenters]);
  const leafCostCenters = useMemo(() => getLeafCostCenters(localCostCenters), [localCostCenters]);

  // --- CHART OF ACCOUNTS STATE ---
  const [accountSearchQuery, setAccountSearchQuery] = useState<string>('');
  const [accountTypeFilter, setAccountTypeFilter] = useState<string>('ALL');
  const [selectedAccountCode, setSelectedAccountCode] = useState<string>('1111');
  const [showTreeBalances, setShowTreeBalances] = useState<boolean>(false);
  const [expandedNodes, setExpandedNodes] = useState<Record<string, boolean>>({
    '1': true,
    '11': true,
    '111': true,
    '112': true,
    '12': true,
    '2': true,
    '21': true,
    '3': true,
    '4': true,
    '5': true,
  });

  const toggleNode = (code: string) => {
    setExpandedNodes((prev) => ({ ...prev, [code]: !prev[code] }));
  };

  const expandAllAccounts = () => {
    const next: Record<string, boolean> = {};
    allAccountsFlat.forEach((a) => {
      next[a.code] = true;
    });
    setExpandedNodes(next);
  };

  const collapseAllAccounts = () => {
    setExpandedNodes({});
  };

  const selectedAccount = useMemo(() => {
    return allAccountsFlat.find((a) => a.code === selectedAccountCode) || allAccountsFlat[0] || null;
  }, [allAccountsFlat, selectedAccountCode]);

  // Hierarchical Breadcrumbs for the selected account
  const accountBreadcrumbs = useMemo(() => {
    if (!selectedAccount) return [];
    const crumbs: AccountNode[] = [];
    let current: AccountNode | undefined = selectedAccount;
    const visited = new Set<string>();
    while (current && !visited.has(current.code)) {
      visited.add(current.code);
      crumbs.unshift(current);
      if (!current.parentCode) break;
      current = allAccountsFlat.find((a) => a.code === current?.parentCode);
    }
    return crumbs;
  }, [selectedAccount, allAccountsFlat]);

  // Journal Entries connected to the selected account
  const accountJournalLines = useMemo(() => {
    if (!selectedAccount) return [];
    const entries: {
      date: string;
      entryNo: string;
      reference: string;
      line: JournalEntryLine;
    }[] = [];
    journalEntries.forEach((entry) => {
      entry.lines.forEach((l) => {
        if (l.accountCode === selectedAccount.code) {
          entries.push({
            date: entry.date,
            entryNo: entry.entryNo,
            reference: entry.reference,
            line: l,
          });
        }
      });
    });
    return entries;
  }, [journalEntries, selectedAccount]);

  // Modals for Account Management
  const [showAddAccountModal, setShowAddAccountModal] = useState<boolean>(false);
  const [showEditAccountModal, setShowEditAccountModal] = useState<boolean>(false);
  const [addAccountParentCode, setAddAccountParentCode] = useState<string>('111');
  const [accountForm, setAccountForm] = useState({
    code: '',
    name: '',
    nameEn: '',
    type: 'ASSET' as AccountNode['type'],
    nature: 'DEBIT' as AccountNode['nature'],
    currency: 'YER',
    allowCostCenter: false,
    isLeaf: true,
    notes: '',
  });

  // Open Add Account modal with intelligent defaults based on parent
  const handleOpenAddAccount = (parent?: AccountNode) => {
    const p = parent || selectedAccount || allAccountsFlat[0];
    if (!p) return;

    // Generate dynamic child code suggestion for infinite-depth hierarchy
    const { code: suggestedCode } = generateNextChildAccountCode(p.code, localAccounts);

    setAddAccountParentCode(p.code);
    setAccountForm({
      code: suggestedCode,
      name: '',
      nameEn: '',
      type: p.type,
      nature: p.nature,
      currency: p.currency || 'YER',
      allowCostCenter: p.type === 'EXPENSE' || p.type === 'REVENUE',
      isLeaf: true,
      notes: '',
    });
    setShowAddAccountModal(true);
  };

  // Open Edit Account modal
  const handleOpenEditAccount = (acc: AccountNode) => {
    setAccountForm({
      code: acc.code,
      name: acc.name,
      nameEn: acc.nameEn || '',
      type: acc.type,
      nature: acc.nature,
      currency: acc.currency || 'YER',
      allowCostCenter: acc.allowCostCenter ?? false,
      isLeaf: acc.isLeaf ?? true,
      notes: acc.notes || '',
    });
    setShowEditAccountModal(true);
  };

  // Save new sub-account into the tree with Parent Transition and Recursive Rollup rules
  const handleSaveNewAccount = (e: React.FormEvent) => {
    e.preventDefault();
    const cleanCode = accountForm.code.trim();
    const cleanName = accountForm.name.trim();

    if (!cleanCode || !cleanName) {
      showFeedback('يرجى ملء رقم الحساب واسم الحساب بشكل صحيح', 'ERROR');
      return;
    }

    // Check duplicate code
    if (allAccountsFlat.some((a) => a.code === cleanCode)) {
      showFeedback(`رقم الحساب [${cleanCode}] مستخدم بالفعل في شجرة الحسابات، اختر رقماً آخر`, 'ERROR');
      return;
    }

    const parent = allAccountsFlat.find((a) => a.code === addAccountParentCode);
    const parentLevel = parent?.level || addAccountParentCode.length;
    const parentPath = parent?.treePath || `/${addAccountParentCode}/`;
    const inheritedStatement = parent?.financialStatement || 
      (parent?.type === 'ASSET' || parent?.type === 'LIABILITY' || parent?.type === 'EQUITY' ? 'BALANCE_SHEET' : 'INCOME_STATEMENT');

    const newAccount: AccountNode = {
      code: cleanCode,
      name: cleanName,
      nameEn: accountForm.nameEn.trim(),
      type: parent?.type || accountForm.type,
      nature: parent?.nature || accountForm.nature,
      financialStatement: inheritedStatement,
      balance: 0,
      openingBalance: 0,
      level: parentLevel + 1,
      treePath: `${parentPath}${cleanCode}/`,
      isLeaf: true,
      isSubAccount: true,
      currency: accountForm.currency,
      allowCostCenter: accountForm.allowCostCenter,
      status: 'ACTIVE',
      notes: accountForm.notes.trim(),
      parentCode: addAccountParentCode,
      children: [],
    };

    // Golden Rule 1: Parent Transition Rule (يتحول الأب فوراً إلى رئيسي تجميعي عند إضافة أول ابن)
    const insertIntoTree = (nodes: AccountNode[]): AccountNode[] => {
      return nodes.map((node) => {
        if (node.code === addAccountParentCode) {
          const children = node.children ? [...node.children, newAccount] : [newAccount];
          return {
            ...node,
            isLeaf: false,
            isSubAccount: false,
            children,
          };
        }
        if (node.children && node.children.length > 0) {
          return { ...node, children: insertIntoTree(node.children) };
        }
        return node;
      });
    };

    // Golden Rule 3: Recursive Balance Rollup
    const updatedTree = calculateTreeBalances(insertIntoTree(localAccounts));
    setInternalAccounts(updatedTree);
    if (onUpdateChartOfAccounts) onUpdateChartOfAccounts(updatedTree);

    // Expand parent and select newly created account
    setExpandedNodes((prev) => ({ ...prev, [addAccountParentCode]: true }));
    setSelectedAccountCode(newAccount.code);
    setShowAddAccountModal(false);
    showFeedback(`تمت إضافة الحساب [${newAccount.code}] ${newAccount.name} بنجاح وتحويل الحساب الأب [${addAccountParentCode}] إلى حساب رئيسي تجميعي.`);
  };

  // Save edited account
  const handleSaveEditAccount = (e: React.FormEvent) => {
    e.preventDefault();
    if (!accountForm.name.trim()) {
      showFeedback('اسم الحساب مطلوب', 'ERROR');
      return;
    }

    const updateInTree = (nodes: AccountNode[]): AccountNode[] => {
      return nodes.map((node) => {
        if (node.code === accountForm.code) {
          return {
            ...node,
            name: accountForm.name.trim(),
            nameEn: accountForm.nameEn.trim(),
            currency: accountForm.currency,
            allowCostCenter: accountForm.allowCostCenter,
            notes: accountForm.notes.trim(),
          };
        }
        if (node.children && node.children.length > 0) {
          return { ...node, children: updateInTree(node.children) };
        }
        return node;
      });
    };

    const updatedTree = calculateTreeBalances(updateInTree(localAccounts));
    setInternalAccounts(updatedTree);
    if (onUpdateChartOfAccounts) onUpdateChartOfAccounts(updatedTree);

    setShowEditAccountModal(false);
    showFeedback(`تم تحديث بيانات الحساب [${accountForm.code}] بنجاح`);
  };

  // Delete account (validation check)
  const handleDeleteAccount = (acc: AccountNode) => {
    if (acc.children && acc.children.length > 0) {
      showFeedback(`لا يمكن حذف الحساب [${acc.code}] لأنه حساب رئيسي يحتوي على حسابات فرعية تحته!`, 'ERROR');
      return;
    }

    if (acc.balance !== 0) {
      showFeedback(`لا يمكن حذف الحساب [${acc.code}] لأن رصيده الحالي ليس صفراً (${acc.balance} ${currency})!`, 'ERROR');
      return;
    }

    const hasTransactions = journalEntries.some((entry) =>
      entry.lines.some((l) => l.accountCode === acc.code)
    );
    if (hasTransactions) {
      showFeedback(`لا يمكن حذف الحساب [${acc.code}] لوجود قيود يومية محاسبية سابقة مسجلة عليه!`, 'ERROR');
      return;
    }

    const removeFromTree = (nodes: AccountNode[]): AccountNode[] => {
      return nodes
        .filter((node) => node.code !== acc.code)
        .map((node) => {
          if (node.children && node.children.length > 0) {
            const updatedChildren = removeFromTree(node.children);
            return {
              ...node,
              isLeaf: updatedChildren.length === 0,
              isSubAccount: updatedChildren.length === 0,
              children: updatedChildren,
            };
          }
          return node;
        });
    };

    const updatedTree = calculateTreeBalances(removeFromTree(localAccounts));
    setInternalAccounts(updatedTree);
    if (onUpdateChartOfAccounts) onUpdateChartOfAccounts(updatedTree);

    setSelectedAccountCode(acc.parentCode || '1');
    showFeedback(`تم حذف الحساب [${acc.code}] ${acc.name} بنجاح`);
  };

  // --- COST CENTERS ENGINE STATE ---
  const [selectedCostCenterId, setSelectedCostCenterId] = useState<string>('CC-201');
  const [costCenterSearchQuery, setCostCenterSearchQuery] = useState<string>('');
  const [expandedCostCenters, setExpandedCostCenters] = useState<Record<string, boolean>>({
    'CC-1': true,
    'CC-2': true,
    'CC-3': true,
  });

  const toggleCostCenterNode = (id: string) => {
    setExpandedCostCenters((prev) => ({ ...prev, [id]: !prev[id] }));
  };

  const expandAllCostCenters = () => {
    const next: Record<string, boolean> = {};
    allCostCentersFlat.forEach((c) => {
      next[c.id] = true;
    });
    setExpandedCostCenters(next);
  };

  const collapseAllCostCenters = () => {
    setExpandedCostCenters({});
  };

  const selectedCostCenter = useMemo(() => {
    return allCostCentersFlat.find((c) => c.id === selectedCostCenterId) || allCostCentersFlat[0] || null;
  }, [allCostCentersFlat, selectedCostCenterId]);

  // Transactions tagged with selected cost center
  const costCenterJournalLines = useMemo(() => {
    if (!selectedCostCenter) return [];
    const rows: {
      date: string;
      entryNo: string;
      reference: string;
      line: JournalEntryLine;
    }[] = [];
    journalEntries.forEach((entry) => {
      entry.lines.forEach((line) => {
        if (
          line.costCenterId === selectedCostCenter.id ||
          line.costCenterCode === selectedCostCenter.code
        ) {
          rows.push({
            date: entry.date,
            entryNo: entry.entryNo,
            reference: entry.reference,
            line,
          });
        }
      });
    });
    return rows;
  }, [journalEntries, selectedCostCenter]);

  // Calculate actual expenses and revenues for this cost center
  const costCenterActuals = useMemo(() => {
    let actualExpenses = 0;
    let actualRevenues = 0;
    costCenterJournalLines.forEach((r) => {
      actualExpenses += r.line.debit;
      actualRevenues += r.line.credit;
    });
    return { actualExpenses, actualRevenues };
  }, [costCenterJournalLines]);

  // Modal for Cost Centers
  const [showAddCostCenterModal, setShowAddCostCenterModal] = useState<boolean>(false);
  const [showEditCostCenterModal, setShowEditCostCenterModal] = useState<boolean>(false);
  const [addCostCenterParentId, setAddCostCenterParentId] = useState<string>('CC-2');
  const [costCenterForm, setCostCenterForm] = useState({
    code: '',
    name: '',
    nameEn: '',
    managerName: '',
    budget: 0,
    isLeaf: true,
    notes: '',
  });

  const handleOpenAddCostCenter = (parent?: CostCenterNode) => {
    const p = parent || selectedCostCenter || allCostCentersFlat[0];
    if (!p) return;

    const siblings = p.children || [];
    let suggestedCode = `${p.code}01`;
    if (siblings.length > 0) {
      const highest = siblings.reduce((max, s) => {
        const num = parseInt(s.code, 10);
        return !isNaN(num) && num > max ? num : max;
      }, 0);
      if (highest > 0) suggestedCode = String(highest + 1);
    }

    setAddCostCenterParentId(p.id);
    setCostCenterForm({
      code: suggestedCode,
      name: '',
      nameEn: '',
      managerName: '',
      budget: 100000,
      isLeaf: true,
      notes: '',
    });
    setShowAddCostCenterModal(true);
  };

  const handleOpenEditCostCenter = (cc: CostCenterNode) => {
    setCostCenterForm({
      code: cc.code,
      name: cc.name,
      nameEn: cc.nameEn || '',
      managerName: cc.managerName || '',
      budget: cc.budget || 0,
      isLeaf: cc.isLeaf,
      notes: cc.notes || '',
    });
    setShowEditCostCenterModal(true);
  };

  const handleSaveNewCostCenter = (e: React.FormEvent) => {
    e.preventDefault();
    if (!costCenterForm.code.trim() || !costCenterForm.name.trim()) {
      showFeedback('يرجى كتابة رمز واسم مركز التكلفة بشكل صحيح', 'ERROR');
      return;
    }

    if (allCostCentersFlat.some((c) => c.code === costCenterForm.code.trim())) {
      showFeedback(`رمز مركز التكلفة [${costCenterForm.code}] مستخدم بالفعل`, 'ERROR');
      return;
    }

    const parent = allCostCentersFlat.find((c) => c.id === addCostCenterParentId);
    const newCC: CostCenterNode = {
      id: `CC-${costCenterForm.code.trim()}`,
      code: costCenterForm.code.trim(),
      name: costCenterForm.name.trim(),
      nameEn: costCenterForm.nameEn.trim(),
      parentCode: parent?.code,
      level: (parent?.level || 1) + 1,
      isLeaf: costCenterForm.isLeaf,
      budget: Number(costCenterForm.budget) || 0,
      managerName: costCenterForm.managerName.trim(),
      status: 'ACTIVE',
      notes: costCenterForm.notes.trim(),
      children: [],
    };

    const insertIntoCCTree = (nodes: CostCenterNode[]): CostCenterNode[] => {
      return nodes.map((node) => {
        if (node.id === addCostCenterParentId) {
          const children = node.children ? [...node.children, newCC] : [newCC];
          return { ...node, isLeaf: false, children };
        }
        if (node.children && node.children.length > 0) {
          return { ...node, children: insertIntoCCTree(node.children) };
        }
        return node;
      });
    };

    const updated = insertIntoCCTree(localCostCenters);
    setInternalCostCenters(updated);
    if (onUpdateCostCenters) onUpdateCostCenters(updated);

    setExpandedCostCenters((prev) => ({ ...prev, [addCostCenterParentId]: true }));
    setSelectedCostCenterId(newCC.id);
    setShowAddCostCenterModal(false);
    showFeedback(`تمت إضافة مركز التكلفة [${newCC.code}] ${newCC.name} بنجاح`);
  };

  const handleSaveEditCostCenter = (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedCostCenter) return;

    const updateInCCTree = (nodes: CostCenterNode[]): CostCenterNode[] => {
      return nodes.map((node) => {
        if (node.id === selectedCostCenter.id) {
          return {
            ...node,
            name: costCenterForm.name.trim(),
            nameEn: costCenterForm.nameEn.trim(),
            managerName: costCenterForm.managerName.trim(),
            budget: Number(costCenterForm.budget) || 0,
            notes: costCenterForm.notes.trim(),
          };
        }
        if (node.children && node.children.length > 0) {
          return { ...node, children: updateInCCTree(node.children) };
        }
        return node;
      });
    };

    const updated = updateInCCTree(localCostCenters);
    setInternalCostCenters(updated);
    if (onUpdateCostCenters) onUpdateCostCenters(updated);

    setShowEditCostCenterModal(false);
    showFeedback(`تم تحديث مركز التكلفة [${selectedCostCenter.code}] بنجاح`);
  };

  // --- NEW JOURNAL ENTRY MODAL STATE ---
  const [showNewEntryModal, setShowNewEntryModal] = useState<boolean>(false);
  const [entryRef, setEntryRef] = useState<string>('قيد تسوية ومصروفات');
  const [entryDesc, setEntryDesc] = useState<string>('قيد محاسبي مع توزيع مراكز التكلفة');
  const [entryDate, setEntryDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [entryLines, setEntryLines] = useState<JournalEntryLine[]>([
    {
      id: '1',
      accountCode: '52',
      accountName: 'مصروفات الإيجار والكهرباء والمياه',
      costCenterId: 'CC-201',
      costCenterCode: '201',
      costCenterName: 'فرع حدة الرئيسي (صنعاء)',
      debit: 15000,
      credit: 0,
      note: 'إيجار فرع حدة',
    },
    {
      id: '2',
      accountCode: '1111',
      accountName: 'الصندوق الرئيسي (الخزينة المركزية)',
      costCenterId: '',
      costCenterCode: '',
      costCenterName: '',
      debit: 0,
      credit: 15000,
      note: 'صرف نقدي من الصندوق',
    },
  ]);

  const totalDebit = useMemo(() => entryLines.reduce((sum, l) => sum + (Number(l.debit) || 0), 0), [entryLines]);
  const totalCredit = useMemo(() => entryLines.reduce((sum, l) => sum + (Number(l.credit) || 0), 0), [entryLines]);
  const isBalanced = totalDebit > 0 && totalDebit === totalCredit;
  const balanceDifference = Math.abs(totalDebit - totalCredit);

  const handleAddLine = () => {
    setEntryLines((prev) => [
      ...prev,
      {
        id: `L-${prev.length + 1}`,
        accountCode: leafAccounts[0]?.code || '1111',
        accountName: leafAccounts[0]?.name || 'الصندوق الرئيسي',
        costCenterId: '',
        costCenterCode: '',
        costCenterName: '',
        debit: 0,
        credit: 0,
        note: '',
      },
    ]);
  };

  const handleUpdateLine = (index: number, field: keyof JournalEntryLine, val: any) => {
    setEntryLines((prev) => {
      const updated = [...prev];
      if (field === 'accountCode') {
        const foundAcc = allAccountsFlat.find((a) => a.code === val);
        updated[index] = {
          ...updated[index],
          accountCode: val,
          accountName: foundAcc ? foundAcc.name : updated[index].accountName,
        };
      } else if (field === 'costCenterCode') {
        const foundCC = allCostCentersFlat.find((c) => c.code === val);
        updated[index] = {
          ...updated[index],
          costCenterCode: val,
          costCenterId: foundCC ? foundCC.id : '',
          costCenterName: foundCC ? foundCC.name : '',
        };
      } else {
        updated[index] = {
          ...updated[index],
          [field]: val,
        };
      }
      return updated;
    });
  };

  const handleRemoveLine = (idx: number) => {
    if (entryLines.length <= 2) {
      showFeedback('يجب أن يحتوي القيد على سطرين على الأقل (طرف مدين وطرف دائن)', 'ERROR');
      return;
    }
    setEntryLines((prev) => prev.filter((_, i) => i !== idx));
  };

  const handleSaveEntry = (e: React.FormEvent) => {
    e.preventDefault();
    if (!isBalanced) {
      showFeedback(`طرفا القيد غير متساويين! يوجد فارق قدره ${balanceDifference.toLocaleString()} ${currency}`, 'ERROR');
      return;
    }

    const entrySeq = journalEntries.length + 1;
    const newEntry: JournalEntry = {
      id: `JV-${100 + entrySeq}`,
      entryNo: String(100 + entrySeq),
      date: entryDate,
      reference: entryRef || 'قيد محاسبي يدوي',
      description: entryDesc || 'ترحيل قيد يومية',
      lines: entryLines,
      totalDebit,
      totalCredit,
      isBalanced: true,
      status: 'POSTED',
    };

    onAddJournalEntry(newEntry);
    setShowNewEntryModal(false);
    showFeedback(`تم ترحيل قيد اليومية [${newEntry.entryNo}] بنجاح بحجم ${totalDebit.toLocaleString()} ${currency}`);
  };

  // --- ACCOUNT STATEMENT VIEWER STATE ---
  const [selectedStatementAccountCode, setSelectedStatementAccountCode] = useState<string>('1111');
  const [statementDateFrom, setStatementDateFrom] = useState<string>('2026-08-01');
  const [statementDateTo, setStatementDateTo] = useState<string>(new Date().toISOString().split('T')[0]);

  const statementLines = useMemo(() => {
    const acc = allAccountsFlat.find((a) => a.code === selectedStatementAccountCode);
    if (!acc) return [];

    let running = acc.openingBalance || 0;
    const history: {
      date: string;
      entryNo: string;
      reference: string;
      note: string;
      costCenterName?: string;
      debit: number;
      credit: number;
      runningBalance: number;
    }[] = [];

    journalEntries
      .filter((e) => e.date >= statementDateFrom && e.date <= statementDateTo)
      .forEach((entry) => {
        entry.lines.forEach((l) => {
          if (l.accountCode === selectedStatementAccountCode) {
            if (acc.nature === 'DEBIT') {
              running += l.debit - l.credit;
            } else {
              running += l.credit - l.debit;
            }
            history.push({
              date: entry.date,
              entryNo: entry.entryNo,
              reference: entry.reference,
              note: l.note || entry.description,
              costCenterName: l.costCenterName,
              debit: l.debit,
              credit: l.credit,
              runningBalance: running,
            });
          }
        });
      });

    return history;
  }, [allAccountsFlat, journalEntries, selectedStatementAccountCode, statementDateFrom, statementDateTo]);

  // --- TRIAL BALANCE CALCULATION ---
  const trialBalanceData = useMemo(() => {
    const rows = leafAccounts.map((acc) => {
      let debitSum = 0;
      let creditSum = 0;

      journalEntries.forEach((entry) => {
        entry.lines.forEach((l) => {
          if (l.accountCode === acc.code) {
            debitSum += l.debit;
            creditSum += l.credit;
          }
        });
      });

      // Net balance
      const net = acc.nature === 'DEBIT' ? debitSum - creditSum : creditSum - debitSum;
      const balDebit = acc.nature === 'DEBIT' ? Math.max(0, net) : net < 0 ? Math.abs(net) : 0;
      const balCredit = acc.nature === 'CREDIT' ? Math.max(0, net) : net < 0 ? Math.abs(net) : 0;

      return {
        code: acc.code,
        name: acc.name,
        type: acc.type,
        nature: acc.nature,
        debitMovements: debitSum,
        creditMovements: creditSum,
        balanceDebit: balDebit,
        balanceCredit: balCredit,
      };
    });

    const grandTotalDebitMovements = rows.reduce((sum, r) => sum + r.debitMovements, 0);
    const grandTotalCreditMovements = rows.reduce((sum, r) => sum + r.creditMovements, 0);
    const grandTotalDebitBalances = rows.reduce((sum, r) => sum + r.balanceDebit, 0);
    const grandTotalCreditBalances = rows.reduce((sum, r) => sum + r.balanceCredit, 0);

    return {
      rows,
      grandTotalDebitMovements,
      grandTotalCreditMovements,
      grandTotalDebitBalances,
      grandTotalCreditBalances,
    };
  }, [leafAccounts, journalEntries]);

  // Filtered Accounts Tree
  const filteredAccountsTree = useMemo(() => {
    const q = accountSearchQuery.trim().toLowerCase();
    const type = accountTypeFilter;

    const filterNode = (node: AccountNode): AccountNode | null => {
      const matchSearch =
        !q ||
        node.code.toLowerCase().includes(q) ||
        node.name.toLowerCase().includes(q) ||
        (node.nameEn && node.nameEn.toLowerCase().includes(q));

      const matchType = type === 'ALL' || node.type === type;

      let filteredChildren: AccountNode[] = [];
      if (node.children && node.children.length > 0) {
        filteredChildren = node.children
          .map((child) => filterNode(child))
          .filter((c): c is AccountNode => c !== null);
      }

      if (filteredChildren.length > 0 || (matchSearch && matchType)) {
        return {
          ...node,
          children: filteredChildren,
        };
      }

      return null;
    };

    return localAccounts
      .map((n) => filterNode(n))
      .filter((n): n is AccountNode => n !== null);
  }, [localAccounts, accountSearchQuery, accountTypeFilter]);

  // Render Recursive Account Tree Node
  const renderAccountNode = (node: AccountNode, depth = 0) => {
    const hasChildren = Boolean(node.children && node.children.length > 0);
    const isExpanded = expandedNodes[node.code] ?? false;
    const isSelected = selectedAccountCode === node.code;
    const isParentNode = hasChildren || node.isSubAccount === false;

    return (
      <div key={node.code} className="text-xs">
        <div
          onClick={() => setSelectedAccountCode(node.code)}
          className={`group flex items-center justify-between p-2 rounded transition border-b border-slate-100 cursor-pointer ${
            isSelected
              ? 'bg-sky-800 text-white font-bold shadow-xs'
              : depth === 0
              ? 'bg-slate-100/90 hover:bg-slate-200/80 font-bold text-slate-800'
              : depth === 1
              ? 'bg-slate-50 hover:bg-sky-50 font-semibold text-slate-800'
              : 'hover:bg-sky-50 text-slate-700'
          }`}
          style={{ paddingRight: `${depth * 16 + 8}px` }}
        >
          <div className="flex items-center gap-1.5 min-w-0 flex-1">
            {hasChildren ? (
              <button
                type="button"
                onClick={(e) => {
                  e.stopPropagation();
                  toggleNode(node.code);
                }}
                className="p-0.5 hover:bg-slate-300/40 rounded cursor-pointer"
                title={isExpanded ? 'طي الفروع' : 'توسيع الفروع'}
              >
                {isExpanded ? (
                  <ChevronDown className={`w-3.5 h-3.5 ${isSelected ? 'text-white' : 'text-slate-500'}`} />
                ) : (
                  <ChevronRight className={`w-3.5 h-3.5 ${isSelected ? 'text-white' : 'text-slate-500'}`} />
                )}
              </button>
            ) : (
              <span className="w-3.5 h-3.5 inline-block" />
            )}

            {/* Level indicator */}
            <span
              className={`text-[9px] px-1 py-0.2 rounded font-mono font-bold ${
                isSelected
                  ? 'bg-sky-800 text-sky-200'
                  : 'bg-slate-200/80 text-slate-600'
              }`}
              title={`المستوى ${node.level || depth + 1}`}
            >
              م{node.level || depth + 1}
            </span>

            {hasChildren ? (
              isExpanded ? (
                <FolderOpen className={`w-4 h-4 shrink-0 ${isSelected ? 'text-amber-300' : 'text-amber-600'}`} />
              ) : (
                <Folder className={`w-4 h-4 shrink-0 ${isSelected ? 'text-amber-300' : 'text-amber-600'}`} />
              )
            ) : (
              <FileSpreadsheet className={`w-3.5 h-3.5 shrink-0 ${isSelected ? 'text-sky-300' : 'text-sky-600'}`} />
            )}

            <span className={`font-mono font-bold px-1 rounded text-[11px] ${
              isSelected ? 'bg-sky-900 text-sky-200' : 'bg-slate-200 text-slate-700'
            }`}>
              {node.code}
            </span>

            <span className="truncate">{node.name}</span>

            {/* Parent (Rollup) vs SubAccount (Leaf) indicator */}
            {isParentNode ? (
              <span
                className={`text-[9px] px-1 rounded font-normal shrink-0 ${
                  isSelected
                    ? 'bg-amber-900/80 text-amber-200'
                    : 'bg-amber-50 text-amber-800 border border-amber-200'
                }`}
                title="حساب رئيسي تجميعي (تراكمي لأرصدة الأبناء - لا يقبل قيود مباشرة)"
              >
                تجميعي
              </span>
            ) : (
              <span
                className={`text-[9px] px-1 rounded font-normal shrink-0 ${
                  isSelected
                    ? 'bg-emerald-900/80 text-emerald-200'
                    : 'bg-emerald-50 text-emerald-800 border border-emerald-200'
                }`}
                title="حساب فرعي تحليلي (يقبل ترحيل القيود)"
              >
                فرعي
              </span>
            )}

            {node.allowCostCenter && (
              <span className={`text-[9px] px-1 rounded font-normal shrink-0 hidden sm:inline ${
                isSelected ? 'bg-indigo-900 text-indigo-200' : 'bg-indigo-50 text-indigo-700 border border-indigo-200'
              }`}>
                مركز تكلفة
              </span>
            )}
          </div>

          <div className="flex items-center gap-1.5 shrink-0">
            {/* Quick Inline Add Child Button */}
            <button
              type="button"
              onClick={(e) => {
                e.stopPropagation();
                handleOpenAddAccount(node);
              }}
              title={`إضافة حساب ابن متفرع من [${node.code}] ${node.name}`}
              className={`p-1 rounded transition hover:scale-110 cursor-pointer ${
                isSelected
                  ? 'bg-sky-800 text-sky-100 hover:bg-sky-700'
                  : 'opacity-70 group-hover:opacity-100 bg-slate-200/80 text-slate-700 hover:bg-sky-600 hover:text-white'
              }`}
            >
              <Plus className="w-3 h-3" />
            </button>

            <span
              className={`text-[10px] px-1.5 py-0.5 rounded font-semibold ${
                isSelected
                  ? 'bg-white/20 text-white'
                  : node.nature === 'DEBIT'
                  ? 'bg-blue-50 text-blue-800'
                  : 'bg-emerald-50 text-emerald-800'
              }`}
            >
              {node.nature === 'DEBIT' ? 'مدين' : 'دائن'}
            </span>
            {showTreeBalances && (
              <span className={`font-mono-numbers font-bold text-[11px] ${isSelected ? 'text-white' : 'text-slate-900'}`}>
                {node.balance.toLocaleString()} {currency}
              </span>
            )}
          </div>
        </div>

        {hasChildren && isExpanded && (
          <div>{node.children!.map((child) => renderAccountNode(child, depth + 1))}</div>
        )}
      </div>
    );
  };

  // Render Recursive Cost Center Node
  const renderCostCenterNode = (node: CostCenterNode, depth = 0) => {
    const hasChildren = node.children && node.children.length > 0;
    const isExpanded = expandedCostCenters[node.id] ?? false;
    const isSelected = selectedCostCenterId === node.id;

    return (
      <div key={node.id} className="text-xs">
        <div
          onClick={() => setSelectedCostCenterId(node.id)}
          className={`flex items-center justify-between p-2 rounded transition border-b border-slate-100 cursor-pointer ${
            isSelected
              ? 'bg-sky-800 text-white font-bold shadow-xs'
              : depth === 0
              ? 'bg-slate-100 font-bold text-slate-800'
              : 'hover:bg-sky-50 text-slate-700'
          }`}
          style={{ paddingRight: `${depth * 18 + 8}px` }}
        >
          <div className="flex items-center gap-1.5 min-w-0">
            {hasChildren ? (
              <button
                type="button"
                onClick={(e) => {
                  e.stopPropagation();
                  toggleCostCenterNode(node.id);
                }}
                className="p-0.5 hover:bg-slate-300/40 rounded cursor-pointer"
              >
                {isExpanded ? (
                  <ChevronDown className={`w-3.5 h-3.5 ${isSelected ? 'text-white' : 'text-slate-500'}`} />
                ) : (
                  <ChevronRight className={`w-3.5 h-3.5 ${isSelected ? 'text-white' : 'text-slate-500'}`} />
                )}
              </button>
            ) : (
              <span className="w-3.5 h-3.5 inline-block" />
            )}

            <Building2 className={`w-3.5 h-3.5 shrink-0 ${isSelected ? 'text-sky-300' : 'text-indigo-600'}`} />

            <span className={`font-mono font-bold px-1 rounded text-[11px] ${
              isSelected ? 'bg-sky-900 text-sky-200' : 'bg-slate-200 text-slate-700'
            }`}>
              {node.code}
            </span>

            <span className="truncate">{node.name}</span>

            {node.managerName && (
              <span className={`text-[10px] hidden sm:inline px-1.5 rounded ${
                isSelected ? 'bg-white/20 text-white' : 'text-slate-500'
              }`}>
                ({node.managerName})
              </span>
            )}
          </div>

          <div className="flex items-center gap-2 shrink-0">
            <span className={`text-[10px] px-1.5 py-0.5 rounded font-mono ${
              isSelected ? 'bg-sky-900 text-sky-200' : 'bg-slate-100 text-slate-600 border border-slate-200'
            }`}>
              الموازنة: {node.budget.toLocaleString()} {currency}
            </span>
          </div>
        </div>

        {hasChildren && isExpanded && (
          <div>{node.children!.map((child) => renderCostCenterNode(child, depth + 1))}</div>
        )}
      </div>
    );
  };

  return (
    <div className="flex-1 flex flex-col h-full bg-[#f8fafc] text-slate-800 p-2 gap-2 select-none overflow-hidden font-sans">
      {/* 1. Phase 4 Operational Stage Ribbon (شريط المرحلة الرابعة: الحسابات العامة ودفتر الأستاذ والتقارير المالية) */}
      <div className="bg-white border border-slate-200 px-3 py-1.5 rounded-lg flex flex-wrap items-center justify-between gap-2 text-xs shadow-xs shrink-0 text-slate-800">
        {/* Left: Phase Title & Workflow Steps Navigation */}
        <div className="flex items-center gap-2 overflow-x-auto">
          {/* Phase Badge */}
          <div className="flex items-center gap-1.5 bg-blue-700 text-white px-2.5 py-0.5 rounded-full border border-blue-800 shadow-xs">
            <span className="w-4 h-4 rounded-full bg-white text-blue-900 flex items-center justify-center font-black text-[10px]">
              4
            </span>
            <span className="font-bold text-[11px]">المرحلة الرابعة: الحسابات العامة ودفتر الأستاذ والعملات</span>
          </div>

          <div className="h-4 w-px bg-slate-200" />

          {/* Direct Switchers between Phase 4 Tabs */}
          <div className="flex items-center gap-1">
            <button
              type="button"
              onClick={() => setActiveTab('CHART_OF_ACCOUNTS')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                activeTab === 'CHART_OF_ACCOUNTS'
                  ? 'bg-blue-700 text-white border border-blue-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="شجرة ودليل الحسابات التجميعية والفرعية"
            >
              <Folder className="w-3 h-3 text-amber-500" />
              <span>دليل الحسابات</span>
            </button>

            <button
              type="button"
              onClick={() => setActiveTab('COST_CENTERS')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                activeTab === 'COST_CENTERS'
                  ? 'bg-blue-700 text-white border border-blue-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="مراكز التكلفة والمشاريع"
            >
              <Building2 className="w-3 h-3 text-indigo-500" />
              <span>مراكز التكلفة</span>
            </button>

            <button
              type="button"
              onClick={() => setActiveTab('JOURNAL_ENTRIES')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                activeTab === 'JOURNAL_ENTRIES'
                  ? 'bg-blue-700 text-white border border-blue-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="قيود اليومية العامة وترحيل الدفاتر [F7]"
            >
              <BookOpen className="w-3 h-3 text-sky-600" />
              <span>قيود اليومية [F7]</span>
            </button>

            <button
              type="button"
              onClick={() => setActiveTab('FISCAL_PERIODS')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                activeTab === 'FISCAL_PERIODS'
                  ? 'bg-blue-700 text-white border border-blue-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="الفترات المالية والإقفال الشهري والسنوي"
            >
              <Calendar className="w-3 h-3 text-rose-600" />
              <span>الفترات والإقفال</span>
            </button>

            <button
              type="button"
              onClick={() => setActiveTab('TRIAL_BALANCE')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                activeTab === 'TRIAL_BALANCE'
                  ? 'bg-blue-700 text-white border border-blue-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="ميزان المراجعة بالمجاميع والأرصدة"
            >
              <FileSpreadsheet className="w-3 h-3 text-teal-600" />
              <span>ميزان المراجعة</span>
            </button>

            <button
              type="button"
              onClick={() => setActiveTab('ACCOUNT_STATEMENT')}
              className={`px-2 py-0.5 rounded text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                activeTab === 'ACCOUNT_STATEMENT'
                  ? 'bg-blue-700 text-white border border-blue-800 shadow-xs'
                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
              }`}
              title="كشف حساب تفصيلي لأي حساب في الدليل"
            >
              <FileText className="w-3 h-3 text-emerald-600" />
              <span>كشف حساب</span>
            </button>

            {onSelectTab && (
              <>
                <button
                  type="button"
                  onClick={() => onSelectTab('MOHASEB_ACCOUNTS')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="دليل الحسابات والصناديق والبنوك الشامل"
                >
                  <Landmark className="w-3 h-3 text-amber-600" />
                  <span>دليل الصناديق والبنوك</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('CURRENCIES')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="دليل العملات النقدية"
                >
                  <Coins className="w-3 h-3 text-yellow-600" />
                  <span>دليل العملات</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('CURRENCY_RATES')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="أسعار الصرف وتقلبات العملات الأجنبية"
                >
                  <TrendingUp className="w-3 h-3 text-cyan-600" />
                  <span>أسعار الصرف</span>
                </button>
              </>
            )}
          </div>
        </div>

        {/* Right: Architectural Rules & Prev/Next Stage Shortcuts */}
        <div className="flex items-center gap-2">
          {/* Rules Indicators */}
          <div className="hidden xl:flex items-center gap-1 text-[10px] text-slate-600">
            <span className="bg-emerald-50 text-emerald-700 px-1.5 py-0.5 rounded border border-emerald-200 font-mono font-bold">
              ✓ قيود اليومية: 101, 102...
            </span>
            <span className="bg-blue-50 text-blue-700 px-1.5 py-0.5 rounded border border-blue-200 font-bold">
              ✓ القيد المتوازن إلزامي
            </span>
            <span className="bg-amber-50 text-amber-700 px-1.5 py-0.5 rounded border border-amber-200 font-bold">
              ✓ تجميع الأرصدة تلقائياً
            </span>
          </div>

          {onSelectTab && (
            <div className="flex items-center gap-1">
              <button
                type="button"
                onClick={() => onSelectTab('CUSTOMERS')}
                className="bg-slate-100 hover:bg-slate-200 text-slate-700 text-[11px] font-bold px-2 py-1 rounded flex items-center gap-1 transition border border-slate-200 cursor-pointer"
                title="الرجوع إلى المرحلة الثالثة: دورة العملاء والسيولة والعهد"
              >
                <ChevronRight className="w-3.5 h-3.5" />
                <span>المرحلة 3: العملاء والسيولة</span>
              </button>

              <button
                type="button"
                onClick={() => onSelectTab('REPORTS')}
                className="bg-purple-700 hover:bg-purple-600 text-white text-[11px] font-bold px-2.5 py-1 rounded flex items-center gap-1 transition shadow-xs border border-purple-800 cursor-pointer"
                title="الانتقال إلى المرحلة الخامسة: التقارير الشاملة وإدارة الموظفين والنظام"
              >
                <span>المرحلة 5: التقارير والنظام</span>
                <ChevronLeft className="w-3.5 h-3.5" />
              </button>
            </div>
          )}
        </div>
      </div>

      {/* Feedback message banner */}
      {feedbackMsg && (
        <div
          className={`p-2.5 rounded-lg text-xs font-bold flex items-center justify-between border shadow-xs animate-in fade-in slide-in-from-top-2 duration-200 ${
            feedbackMsg.type === 'SUCCESS'
              ? 'bg-emerald-50 text-emerald-900 border-emerald-300'
              : 'bg-rose-50 text-rose-900 border-rose-300'
          }`}
        >
          <div className="flex items-center gap-2">
            {feedbackMsg.type === 'SUCCESS' ? (
              <CheckCircle2 className="w-4 h-4 text-emerald-600" />
            ) : (
              <AlertCircle className="w-4 h-4 text-rose-600" />
            )}
            <span>{feedbackMsg.text}</span>
          </div>
          <button
            onClick={() => setFeedbackMsg(null)}
            className="text-slate-400 hover:text-slate-600 cursor-pointer"
          >
            <X className="w-3.5 h-3.5" />
          </button>
        </div>
      )}

      {/* Main Accounting Navigation Bar (WinForms Style) */}
      <div className="bg-white p-2 rounded-lg border border-slate-200 shadow-xs flex flex-wrap items-center justify-between gap-2 shrink-0">
        <div className="flex items-center flex-wrap gap-1.5">
          <button
            onClick={() => setActiveTab('CHART_OF_ACCOUNTS')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              activeTab === 'CHART_OF_ACCOUNTS'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <Folder className="w-3.5 h-3.5 text-amber-500" />
            <span>شجرة الحسابات (Chart of Accounts)</span>
          </button>

          <button
            onClick={() => setActiveTab('COST_CENTERS')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              activeTab === 'COST_CENTERS'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <Building2 className="w-3.5 h-3.5 text-indigo-500" />
            <span>مراكز التكلفة (Cost Centers)</span>
          </button>

          <button
            onClick={() => setActiveTab('CASH_VOUCHERS')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              activeTab === 'CASH_VOUCHERS'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <Receipt className="w-3.5 h-3.5 text-amber-500" />
            <span>سندات القبض والصرف ({localVouchers.length})</span>
          </button>

          <button
            onClick={() => setActiveTab('JOURNAL_ENTRIES')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              activeTab === 'JOURNAL_ENTRIES'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <BookOpen className="w-3.5 h-3.5 text-sky-500" />
            <span>قيود اليومية العامة ({journalEntries.length})</span>
          </button>

          <button
            onClick={() => setActiveTab('FISCAL_PERIODS')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              activeTab === 'FISCAL_PERIODS'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <Calendar className="w-3.5 h-3.5 text-rose-500" />
            <span>الفترات المالية والإقفال ({localFiscalPeriods.length})</span>
          </button>

          <button
            onClick={() => setActiveTab('ACCOUNT_STATEMENT')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              activeTab === 'ACCOUNT_STATEMENT'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <FileText className="w-3.5 h-3.5 text-emerald-500" />
            <span>كشف حساب تفصيلي</span>
          </button>

          <button
            onClick={() => setActiveTab('TRIAL_BALANCE')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              activeTab === 'TRIAL_BALANCE'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <FileSpreadsheet className="w-3.5 h-3.5 text-teal-600" />
            <span>ميزان المراجعة</span>
          </button>
        </div>

        <button
          onClick={() => setShowNewEntryModal(true)}
          className="bg-emerald-600 hover:bg-emerald-500 text-white text-xs font-bold px-3 py-1.5 rounded flex items-center gap-1.5 shadow-xs transition cursor-pointer"
        >
          <Plus className="w-3.5 h-3.5" />
          <span>إنشاء قيد يومية متوازن [F7]</span>
        </button>
      </div>

      {/* Main Content Workspace */}
      <div className="flex-1 min-h-0 bg-white rounded-lg border border-slate-300 shadow-xs overflow-hidden flex flex-col">
        {/* ========================================================= */}
        {/* TAB 1: CHART OF ACCOUNTS (Interactive Dual-Pane WinForms)  */}
        {/* ========================================================= */}
        {activeTab === 'CHART_OF_ACCOUNTS' && (
          <div className="flex-1 flex flex-col h-full overflow-hidden">
            {/* Tree Toolbar */}
            <div className="p-2 bg-slate-100 border-b border-slate-300 flex flex-wrap items-center justify-between gap-2 text-xs">
              <div className="flex items-center gap-2 flex-1 min-w-[280px]">
                <div className="relative flex-1">
                  <Search className="w-3.5 h-3.5 text-slate-400 absolute right-2.5 top-2.5" />
                  <input
                    type="text"
                    value={accountSearchQuery}
                    onChange={(e) => setAccountSearchQuery(e.target.value)}
                    placeholder="بحث برقم الحساب أو الاسم (عربي / إنجليزي)..."
                    className="w-full bg-white border border-slate-300 rounded pr-8 pl-2.5 py-1 text-slate-900 focus:outline-hidden focus:border-sky-500"
                  />
                  {accountSearchQuery && (
                    <button
                      onClick={() => setAccountSearchQuery('')}
                      className="absolute left-2 top-2 text-slate-400 hover:text-slate-600 cursor-pointer"
                    >
                      <X className="w-3 h-3" />
                    </button>
                  )}
                </div>

                <div className="flex items-center gap-1">
                  <SlidersHorizontal className="w-3.5 h-3.5 text-slate-500" />
                  <select
                    value={accountTypeFilter}
                    onChange={(e) => setAccountTypeFilter(e.target.value)}
                    className="bg-white border border-slate-300 rounded px-2 py-1 font-semibold text-slate-800"
                  >
                    <option value="ALL">جميع الأصول والفروع (الهيكل الرباعي 1-4)</option>
                    <option value="ASSET">1 - الأصول (Assets)</option>
                    <option value="LIABILITY">2 - الخصوم وحقوق الملكية (Liabilities & Equity)</option>
                    <option value="REVENUE">3 - الإيرادات (Revenues)</option>
                    <option value="EXPENSE">4 - المصروفات (Expenses)</option>
                  </select>
                </div>
              </div>

              <div className="flex items-center gap-1.5">
                <button
                  type="button"
                  onClick={() => setShowTreeBalances(!showTreeBalances)}
                  className={`px-2 py-1 rounded text-xs font-bold flex items-center gap-1 transition border cursor-pointer ${
                    showTreeBalances
                      ? 'bg-sky-100 text-sky-800 border-sky-300'
                      : 'bg-white text-slate-700 border-slate-300 hover:bg-slate-50'
                  }`}
                  title={showTreeBalances ? 'إخفاء أرقام الأرصدة من الشجرة لتسهيل القراءة وتخفيف المعلومات' : 'عرض أرقام الأرصدة على فروع الشجرة'}
                >
                  {showTreeBalances ? <Eye className="w-3.5 h-3.5 text-sky-700" /> : <EyeOff className="w-3.5 h-3.5 text-slate-400" />}
                  <span>{showTreeBalances ? 'إخفاء أرقام الشجرة' : 'عرض الأرصدة في الشجرة'}</span>
                </button>

                <button
                  onClick={expandAllAccounts}
                  className="bg-white border border-slate-300 hover:bg-slate-50 px-2 py-1 rounded text-slate-700 font-semibold cursor-pointer"
                >
                  توسيع الكل
                </button>
                <button
                  onClick={collapseAllAccounts}
                  className="bg-white border border-slate-300 hover:bg-slate-50 px-2 py-1 rounded text-slate-700 font-semibold cursor-pointer"
                >
                  طي الكل
                </button>
                <button
                  onClick={() => handleOpenAddAccount(selectedAccount || undefined)}
                  className="bg-sky-700 hover:bg-sky-600 text-white px-2.5 py-1 rounded font-bold flex items-center gap-1 cursor-pointer"
                >
                  <Plus className="w-3.5 h-3.5" />
                  <span>إضافة حساب فرعي</span>
                </button>
              </div>
            </div>

            {/* Dual Pane View */}
            <div className="flex-1 flex flex-col md:flex-row min-h-0 overflow-hidden">
              {/* Left Pane: Hierarchical Tree View */}
              <div className="w-full md:w-1/2 lg:w-5/12 border-b md:border-b-0 md:border-l border-slate-300 overflow-y-auto p-2 bg-slate-50/50 flex flex-col gap-0.5">
                <div className="p-1.5 bg-slate-200/70 rounded text-[11px] font-bold text-slate-700 mb-1 flex items-center justify-between">
                  <span>شجرة الحسابات اليمنية القياسية (دليل الحسابات الموحد)</span>
                  <span className="font-mono text-slate-500">إجمالي الحسابات: {allAccountsFlat.length}</span>
                </div>

                {filteredAccountsTree.length > 0 ? (
                  filteredAccountsTree.map((node) => renderAccountNode(node))
                ) : (
                  <div className="p-8 text-center text-slate-400 text-xs">
                    لم يتم العثور على حسابات مطابقة لمعايير البحث.
                  </div>
                )}
              </div>

              {/* Right Pane: Account Master Inspector & Operations */}
              <div className="w-full md:w-1/2 lg:w-7/12 flex-1 flex flex-col overflow-y-auto bg-white p-3 gap-3">
                {selectedAccount ? (
                  <>
                    {/* Hierarchical Breadcrumb Navigation */}
                    <div className="flex items-center gap-1.5 flex-wrap text-xs bg-slate-100 p-2 rounded-lg border border-slate-200">
                      <span className="font-bold text-slate-500 text-[11px]">المسار الهرمي:</span>
                      {accountBreadcrumbs.map((crumb, idx) => (
                        <React.Fragment key={crumb.code}>
                          {idx > 0 && <span className="text-slate-400 font-bold">/</span>}
                          <button
                            type="button"
                            onClick={() => setSelectedAccountCode(crumb.code)}
                            className={`px-1.5 py-0.5 rounded transition cursor-pointer text-[11px] ${
                              crumb.code === selectedAccount.code
                                ? 'bg-[#0f2d4a] text-white font-bold shadow-xs'
                                : 'bg-white text-slate-700 hover:bg-sky-100 border border-slate-200'
                            }`}
                          >
                            <span className="font-mono text-[10px] ml-1 opacity-75">{crumb.code}</span>
                            <span>{crumb.name}</span>
                          </button>
                        </React.Fragment>
                      ))}
                    </div>

                    {/* Account Header Card */}
                    <div className="bg-slate-50 rounded-lg border border-slate-300 p-3 flex flex-col gap-3">
                      <div className="flex flex-wrap items-center justify-between gap-2 pb-2 border-b border-slate-200">
                        <div className="flex items-center gap-2">
                          <span className="font-mono text-sm bg-[#0f2d4a] text-white px-2.5 py-1 rounded font-bold">
                            {selectedAccount.code}
                          </span>
                          <div>
                            <h3 className="font-bold text-sm text-slate-900">{selectedAccount.name}</h3>
                            {selectedAccount.nameEn && (
                              <p className="text-[11px] text-slate-500 font-mono">{selectedAccount.nameEn}</p>
                            )}
                          </div>
                        </div>

                        <div className="flex items-center gap-1.5">
                          <button
                            onClick={() => handleOpenAddAccount(selectedAccount)}
                            className="bg-emerald-600 hover:bg-emerald-500 text-white text-xs font-semibold px-2.5 py-1 rounded flex items-center gap-1 cursor-pointer"
                          >
                            <Plus className="w-3.5 h-3.5" />
                            <span>إضافة فرعي</span>
                          </button>
                          <button
                            onClick={() => handleOpenEditAccount(selectedAccount)}
                            className="bg-slate-200 hover:bg-slate-300 text-slate-800 text-xs font-semibold px-2.5 py-1 rounded flex items-center gap-1 cursor-pointer"
                          >
                            <Edit3 className="w-3.5 h-3.5" />
                            <span>تعديل</span>
                          </button>
                          <button
                            onClick={() => handleDeleteAccount(selectedAccount)}
                            className="bg-rose-50 hover:bg-rose-100 text-rose-700 border border-rose-300 text-xs font-semibold px-2 py-1 rounded flex items-center gap-1 cursor-pointer"
                            title="حذف الحساب (يشترط رصيد صفر وبدون أبناء أو قيود)"
                          >
                            <Trash2 className="w-3.5 h-3.5" />
                          </button>
                        </div>
                      </div>

                      {/* Status Banner */}
                      {selectedAccount.children && selectedAccount.children.length > 0 ? (
                        <div className="bg-amber-50 border border-amber-200 rounded p-2 text-amber-900 text-xs flex items-start gap-2">
                          <AlertCircle className="w-4 h-4 text-amber-600 shrink-0 mt-0.5" />
                          <div>
                            <span className="font-bold block">حساب رئيسي تجميعي (المستوى {selectedAccount.level || 1})</span>
                            <span className="text-[11px]">
                              رصيد هذا الحساب ({selectedAccount.balance.toLocaleString()} {currency}) محسوب آلياً من مجموع {selectedAccount.children.length} حسابات متفرعة تحته. يُمنع تسجيل قيود محاسبية أو سندات عليه مباشرة.
                            </span>
                          </div>
                        </div>
                      ) : (
                        <div className="bg-emerald-50 border border-emerald-200 rounded p-2 text-emerald-900 text-xs flex items-start gap-2">
                          <CheckCircle2 className="w-4 h-4 text-emerald-600 shrink-0 mt-0.5" />
                          <div>
                            <span className="font-bold block">حساب فرعي تحليلي (يقبل قيود وسندات - المستوى {selectedAccount.level || 1})</span>
                            <span className="text-[11px]">
                              هذا الحساب مخصص لترحيل القيود المحاسبية. وعند إضافة أي حساب فرعي تحته، سيتحول آلياً إلى حساب رئيسي تجميعي.
                            </span>
                          </div>
                        </div>
                      )}

                      {/* Key Attributes Grid */}
                      <div className="grid grid-cols-2 sm:grid-cols-4 gap-2 text-xs">
                        <div className="bg-white p-2 rounded border border-slate-200">
                          <span className="text-slate-500 block text-[10px]">النوع والتصنيف:</span>
                          <span className="font-bold text-slate-800">
                            {selectedAccount.type === 'ASSET'
                              ? 'أصول (Assets)'
                              : selectedAccount.type === 'LIABILITY'
                              ? 'خصوم (Liabilities)'
                              : selectedAccount.type === 'EQUITY'
                              ? 'حقوق ملكية (Equity)'
                              : selectedAccount.type === 'REVENUE'
                              ? 'إيرادات (Revenues)'
                              : 'مصروفات (Expenses)'}
                          </span>
                        </div>

                        <div className="bg-white p-2 rounded border border-slate-200">
                          <span className="text-slate-500 block text-[10px]">طبيعة الحساب:</span>
                          <span
                            className={`font-bold inline-block px-1.5 py-0.5 rounded text-[11px] ${
                              selectedAccount.nature === 'DEBIT'
                                ? 'bg-blue-100 text-blue-800'
                                : 'bg-emerald-100 text-emerald-800'
                            }`}
                          >
                            {selectedAccount.nature === 'DEBIT' ? 'مدين (Debit)' : 'دائن (Credit)'}
                          </span>
                        </div>

                        <div className="bg-white p-2 rounded border border-slate-200">
                          <span className="text-slate-500 block text-[10px]">القائمة المالية:</span>
                          <span className="font-bold text-slate-800">
                            {selectedAccount.financialStatement === 'BALANCE_SHEET'
                              ? 'الميزانية العمومية'
                              : 'قائمة الدخل'}
                          </span>
                        </div>

                        <div className="bg-white p-2 rounded border border-slate-200">
                          <span className="text-slate-500 block text-[10px]">مراكز التكلفة:</span>
                          <span className="font-bold text-slate-800">
                            {selectedAccount.allowCostCenter ? (
                              <span className="text-emerald-700">يقبل مركز تكلفة ✓</span>
                            ) : (
                              <span className="text-slate-400">لا يقبل مراكز تكلفة</span>
                            )}
                          </span>
                        </div>
                      </div>

                      {/* Current Balance Banner */}
                      <div className="bg-white p-2.5 rounded-lg border border-slate-200 flex items-center justify-between">
                        <div>
                          <span className="text-slate-500 block text-[11px]">الرصيد المالي الحالي:</span>
                          <span className="text-lg font-mono-numbers font-extrabold text-[#0f2d4a]">
                            {selectedAccount.balance.toLocaleString()} {currency}
                          </span>
                        </div>
                        <button
                          onClick={() => {
                            setSelectedStatementAccountCode(selectedAccount.code);
                            setActiveTab('ACCOUNT_STATEMENT');
                          }}
                          className="text-sky-700 hover:text-sky-800 text-xs font-bold flex items-center gap-1 underline cursor-pointer"
                        >
                          <span>عرض كشف الحساب الكامل</span>
                          <ExternalLink className="w-3.5 h-3.5" />
                        </button>
                      </div>
                    </div>

                    {/* Mini-Ledger (Recent entries for this account) */}
                    <div className="flex-1 flex flex-col min-h-0 border border-slate-300 rounded-lg overflow-hidden">
                      <div className="bg-slate-100 px-3 py-1.5 border-b border-slate-300 text-xs font-bold text-slate-700 flex items-center justify-between">
                        <span>سجل القيود المحاسبية للحساب [{selectedAccount.code}]</span>
                        <span className="font-normal text-slate-500">
                          عدد الحركات المسجلة: {accountJournalLines.length}
                        </span>
                      </div>

                      <div className="flex-1 overflow-auto">
                        <table className="w-full text-right border-collapse text-xs">
                          <thead>
                            <tr className="bg-slate-200/80 text-slate-700 font-bold border-b border-slate-300 sticky top-0">
                              <th className="p-2 w-24">التاريخ</th>
                              <th className="p-2 w-24">رقم القيد</th>
                              <th className="p-2">المرجع / الشرح</th>
                              <th className="p-2 w-28 text-left">مدين ({currency})</th>
                              <th className="p-2 w-28 text-left">دائن ({currency})</th>
                            </tr>
                          </thead>
                          <tbody>
                            {accountJournalLines.length > 0 ? (
                              accountJournalLines.map((row, idx) => (
                                <tr key={idx} className="border-b border-slate-200 hover:bg-sky-50/50">
                                  <td className="p-2 font-mono text-slate-600">{row.date}</td>
                                  <td className="p-2 font-mono text-sky-800 font-bold">{row.entryNo}</td>
                                  <td className="p-2 text-slate-700">
                                    <span>{row.reference}</span>
                                    {row.line.note && (
                                      <span className="text-slate-500 text-[11px] block">
                                        {row.line.note}
                                      </span>
                                    )}
                                  </td>
                                  <td className="p-2 text-left font-mono-numbers font-bold text-blue-700">
                                    {row.line.debit > 0 ? row.line.debit.toLocaleString() : '-'}
                                  </td>
                                  <td className="p-2 text-left font-mono-numbers font-bold text-emerald-700">
                                    {row.line.credit > 0 ? row.line.credit.toLocaleString() : '-'}
                                  </td>
                                </tr>
                              ))
                            ) : (
                              <tr>
                                <td colSpan={5} className="p-6 text-center text-slate-400">
                                  لا توجد قيود يومية مسجلة على هذا الحساب بعد.
                                </td>
                              </tr>
                            )}
                          </tbody>
                        </table>
                      </div>
                    </div>
                  </>
                ) : (
                  <div className="p-8 text-center text-slate-400">يرجى تحديد حساب من الشجرة لعرض بياناته.</div>
                )}
              </div>
            </div>
          </div>
        )}

        {/* ========================================================= */}
        {/* TAB 2: COST CENTERS ENGINE (شجرة مراكز التكلفة والتحليل)  */}
        {/* ========================================================= */}
        {activeTab === 'COST_CENTERS' && (
          <div className="flex-1 flex flex-col h-full overflow-hidden">
            {/* Cost Centers Toolbar */}
            <div className="p-2 bg-slate-100 border-b border-slate-300 flex flex-wrap items-center justify-between gap-2 text-xs">
              <div className="flex items-center gap-2 flex-1 min-w-[280px]">
                <div className="relative flex-1">
                  <Search className="w-3.5 h-3.5 text-slate-400 absolute right-2.5 top-2.5" />
                  <input
                    type="text"
                    value={costCenterSearchQuery}
                    onChange={(e) => setCostCenterSearchQuery(e.target.value)}
                    placeholder="بحث في مراكز التكلفة..."
                    className="w-full bg-white border border-slate-300 rounded pr-8 pl-2.5 py-1 text-slate-900 focus:outline-hidden"
                  />
                </div>
              </div>

              <div className="flex items-center gap-1.5">
                <button
                  onClick={expandAllCostCenters}
                  className="bg-white border border-slate-300 hover:bg-slate-50 px-2 py-1 rounded text-slate-700 font-semibold cursor-pointer"
                >
                  توسيع الكل
                </button>
                <button
                  onClick={collapseAllCostCenters}
                  className="bg-white border border-slate-300 hover:bg-slate-50 px-2 py-1 rounded text-slate-700 font-semibold cursor-pointer"
                >
                  طي الكل
                </button>
                <button
                  onClick={() => handleOpenAddCostCenter(selectedCostCenter || undefined)}
                  className="bg-indigo-700 hover:bg-indigo-600 text-white px-2.5 py-1 rounded font-bold flex items-center gap-1 cursor-pointer"
                >
                  <Plus className="w-3.5 h-3.5" />
                  <span>إضافة مركز تكلفة جديد</span>
                </button>
              </div>
            </div>

            {/* Dual Pane View */}
            <div className="flex-1 flex flex-col md:flex-row min-h-0 overflow-hidden">
              {/* Left Pane: Cost Centers Tree */}
              <div className="w-full md:w-1/2 lg:w-5/12 border-b md:border-b-0 md:border-l border-slate-300 overflow-y-auto p-2 bg-slate-50/50 flex flex-col gap-0.5">
                <div className="p-1.5 bg-slate-200/70 rounded text-[11px] font-bold text-slate-700 mb-1 flex items-center justify-between">
                  <span>هيكل مراكز التكلفة والمشاريع التشغيلية</span>
                  <span className="font-mono text-slate-500">إجمالي المراكز: {allCostCentersFlat.length}</span>
                </div>

                {localCostCenters.map((cc) => renderCostCenterNode(cc))}
              </div>

              {/* Right Pane: Cost Center Inspector & Budget Dashboard */}
              <div className="w-full md:w-1/2 lg:w-7/12 flex-1 flex flex-col overflow-y-auto bg-white p-3 gap-3">
                {selectedCostCenter ? (
                  <>
                    {/* Master Header Card */}
                    <div className="bg-slate-50 rounded-lg border border-slate-300 p-3 flex flex-col gap-3">
                      <div className="flex flex-wrap items-center justify-between gap-2 pb-2 border-b border-slate-200">
                        <div className="flex items-center gap-2">
                          <span className="font-mono text-sm bg-indigo-900 text-white px-2.5 py-1 rounded font-bold">
                            {selectedCostCenter.code}
                          </span>
                          <div>
                            <h3 className="font-bold text-sm text-slate-900">{selectedCostCenter.name}</h3>
                            {selectedCostCenter.managerName && (
                              <p className="text-[11px] text-slate-600 font-semibold">
                                المدير المسؤول: {selectedCostCenter.managerName}
                              </p>
                            )}
                          </div>
                        </div>

                        <div className="flex items-center gap-1.5">
                          <button
                            onClick={() => handleOpenAddCostCenter(selectedCostCenter)}
                            className="bg-emerald-600 hover:bg-emerald-500 text-white text-xs font-semibold px-2.5 py-1 rounded flex items-center gap-1 cursor-pointer"
                          >
                            <Plus className="w-3.5 h-3.5" />
                            <span>مركز فرعي</span>
                          </button>
                          <button
                            onClick={() => handleOpenEditCostCenter(selectedCostCenter)}
                            className="bg-slate-200 hover:bg-slate-300 text-slate-800 text-xs font-semibold px-2.5 py-1 rounded flex items-center gap-1 cursor-pointer"
                          >
                            <Edit3 className="w-3.5 h-3.5" />
                            <span>تعديل</span>
                          </button>
                        </div>
                      </div>

                      {/* Financial KPI Cards */}
                      <div className="grid grid-cols-1 sm:grid-cols-3 gap-2 text-xs">
                        <div className="bg-white p-2.5 rounded border border-slate-200">
                          <span className="text-slate-500 block text-[10px]">الموازنة التقديرية (Budget):</span>
                          <span className="text-base font-mono-numbers font-bold text-slate-800">
                            {selectedCostCenter.budget.toLocaleString()} {currency}
                          </span>
                        </div>

                        <div className="bg-white p-2.5 rounded border border-slate-200">
                          <span className="text-slate-500 block text-[10px]">المصروفات الفعلية المحملة:</span>
                          <span className="text-base font-mono-numbers font-bold text-rose-700">
                            {costCenterActuals.actualExpenses.toLocaleString()} {currency}
                          </span>
                        </div>

                        <div className="bg-white p-2.5 rounded border border-slate-200">
                          <span className="text-slate-500 block text-[10px]">الفارق المتبقي:</span>
                          <span className="text-base font-mono-numbers font-bold text-emerald-700">
                            {(selectedCostCenter.budget - costCenterActuals.actualExpenses).toLocaleString()} {currency}
                          </span>
                        </div>
                      </div>

                      {/* Budget Utilization Progress Bar */}
                      {selectedCostCenter.budget > 0 && (
                        <div className="bg-white p-2.5 rounded border border-slate-200 flex flex-col gap-1.5">
                          <div className="flex items-center justify-between text-xs">
                            <span className="font-semibold text-slate-700">معدل استهلاك الموازنة التشغيلية:</span>
                            <span className="font-bold font-mono">
                              {((costCenterActuals.actualExpenses / selectedCostCenter.budget) * 100).toFixed(1)}%
                            </span>
                          </div>
                          <div className="w-full bg-slate-200 h-2.5 rounded-full overflow-hidden">
                            <div
                              className={`h-full rounded-full transition-all ${
                                (costCenterActuals.actualExpenses / selectedCostCenter.budget) > 0.9
                                  ? 'bg-rose-600'
                                  : (costCenterActuals.actualExpenses / selectedCostCenter.budget) > 0.75
                                  ? 'bg-amber-500'
                                  : 'bg-emerald-600'
                              }`}
                              style={{
                                width: `${Math.min(
                                  100,
                                  (costCenterActuals.actualExpenses / selectedCostCenter.budget) * 100
                                )}%`,
                              }}
                            />
                          </div>
                        </div>
                      )}
                    </div>

                    {/* Attribution Ledger for this Cost Center */}
                    <div className="flex-1 flex flex-col min-h-0 border border-slate-300 rounded-lg overflow-hidden">
                      <div className="bg-slate-100 px-3 py-1.5 border-b border-slate-300 text-xs font-bold text-slate-700 flex items-center justify-between">
                        <span>القيود والحركات المحملة على مركز [{selectedCostCenter.code}]</span>
                        <span className="font-normal text-slate-500">
                          عدد القيود: {costCenterJournalLines.length}
                        </span>
                      </div>

                      <div className="flex-1 overflow-auto">
                        <table className="w-full text-right border-collapse text-xs">
                          <thead>
                            <tr className="bg-slate-200/80 text-slate-700 font-bold border-b border-slate-300 sticky top-0">
                              <th className="p-2 w-24">التاريخ</th>
                              <th className="p-2 w-24">رقم القيد</th>
                              <th className="p-2">الحساب المحاسبي</th>
                              <th className="p-2 w-28 text-left">مدين ({currency})</th>
                              <th className="p-2 w-28 text-left">دائن ({currency})</th>
                              <th className="p-2">شرح البند</th>
                            </tr>
                          </thead>
                          <tbody>
                            {costCenterJournalLines.length > 0 ? (
                              costCenterJournalLines.map((row, idx) => (
                                <tr key={idx} className="border-b border-slate-200 hover:bg-sky-50/50">
                                  <td className="p-2 font-mono text-slate-600">{row.date}</td>
                                  <td className="p-2 font-mono text-sky-800 font-bold">{row.entryNo}</td>
                                  <td className="p-2 font-semibold text-slate-800">
                                    [{row.line.accountCode}] {row.line.accountName}
                                  </td>
                                  <td className="p-2 text-left font-mono-numbers font-bold text-blue-700">
                                    {row.line.debit > 0 ? row.line.debit.toLocaleString() : '-'}
                                  </td>
                                  <td className="p-2 text-left font-mono-numbers font-bold text-emerald-700">
                                    {row.line.credit > 0 ? row.line.credit.toLocaleString() : '-'}
                                  </td>
                                  <td className="p-2 text-slate-600 text-[11px]">{row.line.note || row.reference}</td>
                                </tr>
                              ))
                            ) : (
                              <tr>
                                <td colSpan={6} className="p-6 text-center text-slate-400">
                                  لا توجد قيود يومية محملة على مركز التكلفة هذا حالياً.
                                </td>
                              </tr>
                            )}
                          </tbody>
                        </table>
                      </div>
                    </div>
                  </>
                ) : (
                  <div className="p-8 text-center text-slate-400">يرجى اختيار مركز تكلفة من الشجرة.</div>
                )}
              </div>
            </div>
          </div>
        )}

        {/* ========================================================= */}
        {/* TAB 3: GENERAL JOURNAL ENTRIES (قيود اليومية العامة)       */}
        {/* ========================================================= */}
        {activeTab === 'JOURNAL_ENTRIES' && (
          <div className="flex-1 overflow-auto p-2 flex flex-col gap-3">
            {journalEntries.map((entry) => (
              <div key={entry.id} className="border border-slate-300 rounded-lg overflow-hidden bg-slate-50 shadow-2xs">
                {/* Entry Header Strip */}
                <div className="bg-slate-100 border-b border-slate-200 text-slate-800 px-3 py-2 flex flex-wrap items-center justify-between text-xs font-semibold gap-2">
                  <div className="flex items-center gap-2">
                    <span className="font-mono bg-sky-100 text-sky-800 border border-sky-200 px-2 py-0.5 rounded font-bold">
                      {entry.entryNo}
                    </span>
                    <span className="text-slate-700">التاريخ: {entry.date}</span>
                    <span className="text-slate-300">|</span>
                    <span className="text-sky-700 font-bold">{entry.reference}</span>
                  </div>
                  <div className="flex items-center gap-2">
                    <span className="bg-emerald-100 text-emerald-800 border border-emerald-200 text-[11px] px-2 py-0.5 rounded font-bold">
                      مرحل وموزون
                    </span>
                    <span className="font-mono-numbers text-slate-900 font-bold">
                      الإجمالي: {entry.totalDebit.toLocaleString()} {currency}
                    </span>
                  </div>
                </div>

                {/* Entry Lines DataGridView */}
                <table className="w-full text-right border-collapse text-xs">
                  <thead>
                    <tr className="bg-slate-200 text-slate-700 font-bold border-b border-slate-300">
                      <th className="p-2 w-24">رقم الحساب</th>
                      <th className="p-2">اسم الحساب</th>
                      <th className="p-2 w-48">مركز التكلفة (Cost Center)</th>
                      <th className="p-2 w-32 text-left">مدين ({currency})</th>
                      <th className="p-2 w-32 text-left">دائن ({currency})</th>
                      <th className="p-2">البيان / الشرح</th>
                    </tr>
                  </thead>
                  <tbody>
                    {entry.lines.map((line, idx) => (
                      <tr key={idx} className="border-b border-slate-200 bg-white hover:bg-slate-50">
                        <td className="p-2 font-mono text-slate-700 font-bold">{line.accountCode}</td>
                        <td className="p-2 font-bold text-slate-900">{line.accountName}</td>
                        <td className="p-2">
                          {line.costCenterName ? (
                            <span className="bg-indigo-50 text-indigo-800 border border-indigo-200 px-2 py-0.5 rounded text-[11px] font-semibold">
                              [{line.costCenterCode}] {line.costCenterName}
                            </span>
                          ) : (
                            <span className="text-slate-400">-</span>
                          )}
                        </td>
                        <td className="p-2 text-left font-mono-numbers font-bold text-blue-700">
                          {line.debit > 0 ? line.debit.toLocaleString() : '-'}
                        </td>
                        <td className="p-2 text-left font-mono-numbers font-bold text-emerald-700">
                          {line.credit > 0 ? line.credit.toLocaleString() : '-'}
                        </td>
                        <td className="p-2 text-slate-600 text-[11px]">{line.note || entry.description}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            ))}
          </div>
        )}

        {/* ========================================================= */}
        {/* TAB 4: GENERAL ACCOUNT STATEMENT (كشف حساب تفصيلي)        */}
        {/* ========================================================= */}
        {activeTab === 'ACCOUNT_STATEMENT' && (
          <div className="flex-1 flex flex-col p-3 gap-3 overflow-y-auto text-xs">
            {/* Filter Bar */}
            <div className="bg-slate-50 p-3 rounded-lg border border-slate-200 flex flex-wrap items-center justify-between gap-3">
              <div className="flex items-center gap-2 flex-1 min-w-[300px]">
                <label className="font-bold text-slate-700 shrink-0">اختر الحساب المحاسبي:</label>
                <select
                  value={selectedStatementAccountCode}
                  onChange={(e) => setSelectedStatementAccountCode(e.target.value)}
                  className="flex-1 bg-white border border-slate-300 rounded px-2.5 py-1.5 font-bold text-slate-900"
                >
                  {leafAccounts.map((acc) => (
                    <option key={acc.code} value={acc.code}>
                      [{acc.code}] {acc.name} - الرصيد: {acc.balance.toLocaleString()} {currency} (
                      {acc.nature === 'DEBIT' ? 'مدين' : 'دائن'})
                    </option>
                  ))}
                </select>
              </div>

              <div className="flex items-center gap-2">
                <label className="font-semibold text-slate-700">من تاريخ:</label>
                <input
                  type="date"
                  value={statementDateFrom}
                  onChange={(e) => setStatementDateFrom(e.target.value)}
                  className="bg-white border border-slate-300 rounded px-2 py-1"
                />
                <label className="font-semibold text-slate-700">إلى تاريخ:</label>
                <input
                  type="date"
                  value={statementDateTo}
                  onChange={(e) => setStatementDateTo(e.target.value)}
                  className="bg-white border border-slate-300 rounded px-2 py-1"
                />
              </div>
            </div>

            {/* Statement Table */}
            <div className="flex-1 border border-slate-300 rounded-lg overflow-auto">
              <table className="w-full text-right border-collapse text-xs">
                <thead>
                  <tr className="bg-slate-200 text-slate-800 font-bold border-b border-slate-300 sticky top-0">
                    <th className="p-2.5 w-24">التاريخ</th>
                    <th className="p-2.5 w-24">رقم القيد</th>
                    <th className="p-2.5">المرجع / الشرح</th>
                    <th className="p-2.5 w-36">مركز التكلفة</th>
                    <th className="p-2.5 w-32 text-left">مدين ({currency})</th>
                    <th className="p-2.5 w-32 text-left">دائن ({currency})</th>
                    <th className="p-2.5 w-36 text-left">الرصيد المتحرك ({currency})</th>
                  </tr>
                </thead>
                <tbody>
                  {statementLines.length > 0 ? (
                    statementLines.map((row, idx) => (
                      <tr key={idx} className="border-b border-slate-200 hover:bg-slate-50">
                        <td className="p-2.5 font-mono text-slate-600">{row.date}</td>
                        <td className="p-2.5 font-mono text-sky-800 font-bold">{row.entryNo}</td>
                        <td className="p-2.5 text-slate-800">
                          <span className="font-semibold">{row.reference}</span>
                          {row.note && <span className="text-slate-500 block text-[11px]">{row.note}</span>}
                        </td>
                        <td className="p-2.5">
                          {row.costCenterName ? (
                            <span className="bg-indigo-50 text-indigo-700 border border-indigo-200 px-2 py-0.5 rounded text-[11px]">
                              {row.costCenterName}
                            </span>
                          ) : (
                            <span className="text-slate-400">-</span>
                          )}
                        </td>
                        <td className="p-2.5 text-left font-mono-numbers font-bold text-blue-700">
                          {row.debit > 0 ? row.debit.toLocaleString() : '-'}
                        </td>
                        <td className="p-2.5 text-left font-mono-numbers font-bold text-emerald-700">
                          {row.credit > 0 ? row.credit.toLocaleString() : '-'}
                        </td>
                        <td className="p-2.5 text-left font-mono-numbers font-bold text-slate-900 bg-slate-50/70">
                          {row.runningBalance.toLocaleString()}
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan={7} className="p-8 text-center text-slate-400">
                        لا توجد حركات مسجلة لهذا الحساب خلال الفترة المحددة.
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {/* ========================================================= */}
        {/* TAB 5: TRIAL BALANCE (ميزان المراجعة بالمجاميع والأرصدة)    */}
        {/* ========================================================= */}
        {activeTab === 'TRIAL_BALANCE' && (
          <div className="flex-1 flex flex-col p-3 gap-2 overflow-y-auto text-xs">
            <div className="p-2 bg-slate-100 rounded border border-slate-300 font-semibold text-slate-700 flex items-center justify-between">
              <span>ميزان المراجعة العام بالمجاميع والأرصدة لجميع الحسابات التحليلية الفرعية</span>
              <span className="font-mono text-slate-500">عملة العرض: {currency}</span>
            </div>

            <div className="flex-1 border border-slate-300 rounded-lg overflow-auto">
              <table className="w-full text-right border-collapse text-xs">
                <thead>
                  <tr className="bg-slate-100 text-slate-800 font-bold border-b border-slate-300">
                    <th className="p-2 w-20 text-center" rowSpan={2}>رقم الحساب</th>
                    <th className="p-2" rowSpan={2}>اسم الحساب</th>
                    <th className="p-2 w-24 text-center" rowSpan={2}>طبيعة الحساب</th>
                    <th className="p-2 text-center border-l border-r border-slate-300" colSpan={2}>
                      مجاميع الحركات ({currency})
                    </th>
                    <th className="p-2 text-center" colSpan={2}>
                      الأرصدة الصافية ({currency})
                    </th>
                  </tr>
                  <tr className="bg-slate-200/80 text-slate-700 text-[11px] font-semibold border-b border-slate-300">
                    <th className="p-1.5 w-32 text-left border-r border-slate-300">مجموع مدين</th>
                    <th className="p-1.5 w-32 text-left border-l border-slate-300">مجموع دائن</th>
                    <th className="p-1.5 w-32 text-left border-r border-slate-300">رصيد مدين</th>
                    <th className="p-1.5 w-32 text-left">رصيد دائن</th>
                  </tr>
                </thead>
                <tbody>
                  {trialBalanceData.rows.map((row) => (
                    <tr key={row.code} className="border-b border-slate-200 hover:bg-sky-50/50">
                      <td className="p-2 font-mono text-center font-bold text-slate-700">{row.code}</td>
                      <td className="p-2 font-semibold text-slate-900">{row.name}</td>
                      <td className="p-2 text-center">
                        <span className={`text-[10px] px-1.5 py-0.5 rounded font-bold ${
                          row.nature === 'DEBIT' ? 'bg-blue-50 text-blue-700' : 'bg-emerald-50 text-emerald-700'
                        }`}>
                          {row.nature === 'DEBIT' ? 'مدين' : 'دائن'}
                        </span>
                      </td>
                      <td className="p-2 text-left font-mono-numbers text-slate-700">
                        {row.debitMovements > 0 ? row.debitMovements.toLocaleString() : '-'}
                      </td>
                      <td className="p-2 text-left font-mono-numbers text-slate-700">
                        {row.creditMovements > 0 ? row.creditMovements.toLocaleString() : '-'}
                      </td>
                      <td className="p-2 text-left font-mono-numbers font-bold text-blue-700">
                        {row.balanceDebit > 0 ? row.balanceDebit.toLocaleString() : '-'}
                      </td>
                      <td className="p-2 text-left font-mono-numbers font-bold text-emerald-700">
                        {row.balanceCredit > 0 ? row.balanceCredit.toLocaleString() : '-'}
                      </td>
                    </tr>
                  ))}
                </tbody>
                <tfoot>
                  <tr className="bg-slate-200 text-slate-900 font-extrabold border-t-2 border-slate-400">
                    <td colSpan={3} className="p-2 text-left">الإجمالي العام لميزان المراجعة:</td>
                    <td className="p-2 text-left font-mono-numbers">
                      {trialBalanceData.grandTotalDebitMovements.toLocaleString()}
                    </td>
                    <td className="p-2 text-left font-mono-numbers">
                      {trialBalanceData.grandTotalCreditMovements.toLocaleString()}
                    </td>
                    <td className="p-2 text-left font-mono-numbers text-blue-800">
                      {trialBalanceData.grandTotalDebitBalances.toLocaleString()}
                    </td>
                    <td className="p-2 text-left font-mono-numbers text-emerald-800">
                      {trialBalanceData.grandTotalCreditBalances.toLocaleString()}
                    </td>
                  </tr>
                </tfoot>
              </table>
            </div>
          </div>
        )}

        {/* ========================================================= */}
        {/* TAB: CASH & BANK VOUCHERS (سندات القبض والصرف)              */}
        {/* ========================================================= */}
        {activeTab === 'CASH_VOUCHERS' && (
          <CashVouchersView
            vouchers={localVouchers}
            accounts={allAccountsFlat}
            costCenters={allCostCentersFlat}
            customers={customers}
            suppliers={suppliers}
            fiscalPeriods={localFiscalPeriods}
            journalEntries={journalEntries}
            onAddVoucher={handleAddVoucherInternal}
            onVoidVoucher={handleVoidVoucherInternal}
            currency={currency}
            defaultCashboxCode={defaultCashboxCode}
          />
        )}

        {/* ========================================================= */}
        {/* TAB: FISCAL PERIODS & CLOSING (الفترات المالية والإقفال)    */}
        {/* ========================================================= */}
        {activeTab === 'FISCAL_PERIODS' && (
          <FiscalPeriodsView
            periods={localFiscalPeriods}
            accounts={allAccountsFlat}
            journalEntries={journalEntries}
            onClosePeriod={handleClosePeriodInternal}
            onReopenPeriod={handleReopenPeriodInternal}
            currency={currency}
          />
        )}
      </div>

            {/* ========================================================= */}
      {/* MODAL: ADD SUB-ACCOUNT (إضافة حساب فرعي جديد)              */}
      {/* ========================================================= */}
      <AccountingAddAccountModal
        isOpen={showAddAccountModal}
        onClose={() => setShowAddAccountModal(false)}
        handleSaveNewAccount={handleSaveNewAccount}
        addAccountParentCode={addAccountParentCode}
        allAccountsFlat={allAccountsFlat}
        accountForm={accountForm}
        setAccountForm={setAccountForm}
      />

      {/* ========================================================= */}
      {/* MODAL: EDIT ACCOUNT (تعديل بيانات الحساب)                   */}
      {/* ========================================================= */}
      <AccountingEditAccountModal
        isOpen={showEditAccountModal}
        onClose={() => setShowEditAccountModal(false)}
        handleSaveEditAccount={handleSaveEditAccount}
        accountForm={accountForm}
        setAccountForm={setAccountForm}
      />

      {/* ========================================================= */}
      {/* MODAL: ADD COST CENTER (إضافة مركز تكلفة جديد)             */}
      {/* ========================================================= */}
      <AccountingAddCostCenterModal
        isOpen={showAddCostCenterModal}
        onClose={() => setShowAddCostCenterModal(false)}
        handleSaveNewCostCenter={handleSaveNewCostCenter}
        addCostCenterParentId={addCostCenterParentId}
        allCostCentersFlat={allCostCentersFlat}
        costCenterForm={costCenterForm}
        setCostCenterForm={setCostCenterForm}
        currency={currency}
      />

      {/* ========================================================= */}
      {/* MODAL: EDIT COST CENTER (تعديل بيانات مركز التكلفة)         */}
      {/* ========================================================= */}
      <AccountingEditCostCenterModal
        isOpen={showEditCostCenterModal}
        onClose={() => setShowEditCostCenterModal(false)}
        handleSaveEditCostCenter={handleSaveEditCostCenter}
        costCenterForm={costCenterForm}
        setCostCenterForm={setCostCenterForm}
        currency={currency}
      />

      {/* ========================================================= */}
      {/* MODAL: NEW BALANCED JOURNAL ENTRY (إنشاء قيد يومية متوازن) */}
      {/* ========================================================= */}
      <AccountingNewEntryModal
        isOpen={showNewEntryModal}
        onClose={() => setShowNewEntryModal(false)}
        handleSaveEntry={handleSaveEntry}
        entryRef={entryRef}
        setEntryRef={setEntryRef}
        entryDate={entryDate}
        setEntryDate={setEntryDate}
        entryDesc={entryDesc}
        setEntryDesc={setEntryDesc}
        entryLines={entryLines}
        handleAddLine={handleAddLine}
        handleUpdateLine={handleUpdateLine}
        handleRemoveLine={handleRemoveLine}
        allAccountsFlat={allAccountsFlat}
        leafAccounts={leafAccounts}
        leafCostCenters={leafCostCenters}
        totalDebit={totalDebit}
        totalCredit={totalCredit}
        isBalanced={isBalanced}
        balanceDifference={balanceDifference}
        currency={currency}
      />
    </div>
  );
};
