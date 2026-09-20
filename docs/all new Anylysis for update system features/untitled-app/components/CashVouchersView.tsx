'use client';

import React, { useState, useMemo } from 'react';
import {
  CashVoucherNewModal,
  CashVoucherTransferModal,
  CashVoucherViewModal,
  CashVoucherPrintModal,
  CashVoucherVoidModal,
  CashVoucherAdvancesModal
} from './cash-vouchers';
import { 
  Receipt, 
  CreditCard, 
  Plus, 
  Search, 
  Printer, 
  CheckCircle2, 
  AlertCircle, 
  X, 
  Ban, 
  ArrowDownLeft, 
  ArrowUpRight, 
  Building2, 
  Calendar, 
  FileText,
  DollarSign,
  Wallet,
  Landmark,
  Eye,
  RotateCcw,
  ArrowLeftRight,
  Info,
  ShieldCheck
} from 'lucide-react';
import { 
  CashVoucher, 
  AccountNode, 
  CostCenterNode, 
  Customer, 
  Supplier, 
  JournalEntry,
  FiscalPeriod 
} from '@/types/pos';
import { tafqeet } from '@/lib/tafqeet';
import { 
  isCashboxCode, 
  isBankCode, 
  isTreasuryCode, 
  getTreasuryAccounts, 
  getNonTreasuryAccounts, 
  validateTreasuryAccount 
} from '@/lib/treasuryUtils';

interface CashVouchersViewProps {
  vouchers: CashVoucher[];
  accounts: AccountNode[];
  costCenters: CostCenterNode[];
  customers: Customer[];
  suppliers: Supplier[];
  fiscalPeriods?: FiscalPeriod[];
  journalEntries: JournalEntry[];
  onAddVoucher: (voucher: CashVoucher, generatedEntry: JournalEntry) => void;
  onVoidVoucher: (voucherId: string, reason: string) => void;
  currency: string;
  defaultCashboxCode?: string;
}

export const CashVouchersView: React.FC<CashVouchersViewProps> = ({
  vouchers,
  accounts,
  costCenters,
  customers,
  suppliers,
  fiscalPeriods = [],
  journalEntries,
  onAddVoucher,
  onVoidVoucher,
  currency,
  defaultCashboxCode,
}) => {
  // Filters & Search
  const [searchTerm, setSearchTerm] = useState('');
  const [typeFilter, setTypeFilter] = useState<'ALL' | 'RECEIPT' | 'PAYMENT' | 'TRANSFER'>('ALL');
  const [methodFilter, setMethodFilter] = useState<'ALL' | 'CASH' | 'BANK'>('ALL');
  const [statusFilter, setStatusFilter] = useState<'ALL' | 'POSTED' | 'VOID'>('ALL');

  // Modals
  const [showNewModal, setShowNewModal] = useState(false);
  const [newVoucherType, setNewVoucherType] = useState<'RECEIPT' | 'PAYMENT'>('RECEIPT');
  const [viewingVoucher, setViewingVoucher] = useState<CashVoucher | null>(null);
  const [printingVoucher, setPrintingVoucher] = useState<CashVoucher | null>(null);
  const [voidingVoucher, setVoidingVoucher] = useState<CashVoucher | null>(null);
  const [voidReasonInput, setVoidReasonInput] = useState('');

  // Treasury Transfer Modal State (تحويل بين الصناديق والبنوك)
  const [showTransferModal, setShowTransferModal] = useState(false);
  const [transferFromCode, setTransferFromCode] = useState(defaultCashboxCode || '1111');
  const [transferToCode, setTransferToCode] = useState('1121');
  const [transferAmount, setTransferAmount] = useState<string>('');
  const [transferDate, setTransferDate] = useState(() => new Date().toISOString().split('T')[0]);
  const [transferReference, setTransferReference] = useState('');
  const [transferNote, setTransferNote] = useState('تحويل وتغذية سيولة نقدية');
  const [transferError, setTransferError] = useState<string | null>(null);

  // Employee Advances Analysis Modal State (ملاحظة تسوية العهد المالية للموظفين)
  const [showAdvancesInfoModal, setShowAdvancesInfoModal] = useState(false);

  // Form State for New Voucher
  const [formDate, setFormDate] = useState(() => new Date().toISOString().split('T')[0]);
  const [formPaymentMethod, setFormPaymentMethod] = useState<'CASH' | 'BANK'>('CASH');
  const [formCashAccountCode, setFormCashAccountCode] = useState(defaultCashboxCode || '1111');
  const [formPartyType, setFormPartyType] = useState<'CUSTOMER' | 'SUPPLIER' | 'EXPENSE' | 'OTHER'>('CUSTOMER');
  const [formSelectedPartyId, setFormSelectedPartyId] = useState('');
  const [formPartyName, setFormPartyName] = useState('');
  const [formContraAccountCode, setFormContraAccountCode] = useState('1131');
  const [formCostCenterId, setFormCostCenterId] = useState('');
  const [formAmount, setFormAmount] = useState<string>('');
  const [formReference, setFormReference] = useState('');
  const [formDescription, setFormDescription] = useState('');
  const [formError, setFormError] = useState<string | null>(null);

  // Specialized Tree Accounts Filtering for Treasury & Safe Accounting
  // 1. All Treasury Accounts (Cashboxes 111 & Banks/Wallets 112)
  const allTreasuryAccounts = useMemo(() => {
    return getTreasuryAccounts(accounts);
  }, [accounts]);

  // 2. Cashboxes only (111...)
  const cashOnlyAccounts = useMemo(() => {
    return getTreasuryAccounts(accounts, 'CASHBOX_ONLY');
  }, [accounts]);

  // 3. Banks & E-Wallets only (112...)
  const bankOnlyAccounts = useMemo(() => {
    return getTreasuryAccounts(accounts, 'BANK_ONLY');
  }, [accounts]);

  // 4. Selectable treasury accounts according to chosen payment method (CASH vs BANK)
  const selectableTreasuryAccounts = useMemo(() => {
    if (formPaymentMethod === 'CASH') {
      return cashOnlyAccounts.length > 0 ? cashOnlyAccounts : allTreasuryAccounts.filter(a => isCashboxCode(a.code));
    }
    return bankOnlyAccounts.length > 0 ? bankOnlyAccounts : allTreasuryAccounts.filter(a => isBankCode(a.code));
  }, [formPaymentMethod, cashOnlyAccounts, bankOnlyAccounts, allTreasuryAccounts]);

  // 5. Eligible Contra Accounts (Strictly excluding any Cashbox 111 or Bank 112 to prevent misrouting)
  const contraEligibleAccounts = useMemo(() => {
    return getNonTreasuryAccounts(accounts);
  }, [accounts]);

  const expenseAccounts = useMemo(() => {
    return accounts.filter((a) => a.type === 'EXPENSE' && (!a.children || a.children.length === 0) && a.isLeaf !== false && a.isSubAccount !== false);
  }, [accounts]);

  const leafCostCenters = useMemo(() => {
    return costCenters.filter((c) => c.isLeaf);
  }, [costCenters]);

  // Statistics
  const stats = useMemo(() => {
    const validVouchers = vouchers.filter((v) => v.status !== 'VOID');
    const totalReceipts = validVouchers
      .filter((v) => v.type === 'RECEIPT')
      .reduce((sum, v) => sum + v.amount, 0);
    const totalPayments = validVouchers
      .filter((v) => v.type === 'PAYMENT')
      .reduce((sum, v) => sum + v.amount, 0);
    const totalTransfers = validVouchers
      .filter((v) => v.type === 'TRANSFER')
      .reduce((sum, v) => sum + v.amount, 0);
    const countReceipts = validVouchers.filter((v) => v.type === 'RECEIPT').length;
    const countPayments = validVouchers.filter((v) => v.type === 'PAYMENT').length;
    const countTransfers = validVouchers.filter((v) => v.type === 'TRANSFER').length;
    const netCashFlow = totalReceipts - totalPayments;

    return {
      totalReceipts,
      totalPayments,
      totalTransfers,
      countReceipts,
      countPayments,
      countTransfers,
      netCashFlow,
    };
  }, [vouchers]);

  // Filtered Vouchers List
  const filteredVouchers = useMemo(() => {
    return vouchers.filter((v) => {
      if (typeFilter !== 'ALL' && v.type !== typeFilter) return false;
      if (methodFilter !== 'ALL' && v.paymentMethod !== methodFilter) return false;
      if (statusFilter !== 'ALL' && v.status !== statusFilter) return false;
      if (searchTerm.trim()) {
        const query = searchTerm.toLowerCase();
        const match =
          v.voucherNo.toLowerCase().includes(query) ||
          v.partyName.toLowerCase().includes(query) ||
          v.description.toLowerCase().includes(query) ||
          (v.reference && v.reference.toLowerCase().includes(query)) ||
          v.cashAccountName.toLowerCase().includes(query) ||
          v.contraAccountName.toLowerCase().includes(query);
        if (!match) return false;
      }
      return true;
    });
  }, [vouchers, typeFilter, methodFilter, statusFilter, searchTerm]);

  // Open modal with pre-configured type
  const handleOpenNewVoucher = (type: 'RECEIPT' | 'PAYMENT') => {
    setNewVoucherType(type);
    setFormDate(new Date().toISOString().split('T')[0]);
    setFormPaymentMethod('CASH');
    setFormCashAccountCode(cashOnlyAccounts[0]?.code || '1111');
    setFormAmount('');
    setFormCostCenterId('');
    setFormError(null);

    if (type === 'RECEIPT') {
      setFormPartyType('CUSTOMER');
      setFormSelectedPartyId(customers[0]?.id || '');
      setFormPartyName(customers[0]?.name || 'عميل عام');
      setFormContraAccountCode('1131');
      setFormReference('إيصال قبض رقم ...');
      setFormDescription(`تحصيل نقدي من ${customers[0]?.name || 'العميل'}`);
    } else {
      setFormPartyType('SUPPLIER');
      setFormSelectedPartyId(suppliers[0]?.id || '');
      setFormPartyName(suppliers[0]?.name || 'مورد عام');
      setFormContraAccountCode('2111');
      setFormReference('سند صرف رقم ...');
      setFormDescription(`سداد نقدي لـ ${suppliers[0]?.name || 'المورد'}`);
    }

    setShowNewModal(true);
  };

  // Open Treasury Transfer Modal
  const handleOpenTransferModal = () => {
    setTransferError(null);
    setTransferDate(new Date().toISOString().split('T')[0]);
    const defaultFrom = cashOnlyAccounts[0]?.code || allTreasuryAccounts[0]?.code || '1111';
    const defaultTo = bankOnlyAccounts[0]?.code || allTreasuryAccounts.find(a => a.code !== defaultFrom)?.code || '1121';
    setTransferFromCode(defaultFrom);
    setTransferToCode(defaultTo);
    setTransferAmount('');
    setTransferReference('');
    setTransferNote('تحويل وتغذية سيولة نقدية بين الصناديق والبنوك');
    setShowTransferModal(true);
  };

  // Handle payment method switch (CASH vs BANK) and auto-align account
  const handlePaymentMethodSwitch = (method: 'CASH' | 'BANK') => {
    setFormPaymentMethod(method);
    if (method === 'CASH') {
      if (!isCashboxCode(formCashAccountCode)) {
        setFormCashAccountCode(cashOnlyAccounts[0]?.code || '1111');
      }
    } else {
      if (!isBankCode(formCashAccountCode)) {
        setFormCashAccountCode(bankOnlyAccounts[0]?.code || '1121');
      }
    }
  };

  // Handle party type change in form
  const handlePartyTypeChange = (newPartyType: 'CUSTOMER' | 'SUPPLIER' | 'EXPENSE' | 'OTHER') => {
    setFormPartyType(newPartyType);
    if (newPartyType === 'CUSTOMER') {
      const c = customers[0];
      setFormSelectedPartyId(c?.id || '');
      setFormPartyName(c?.name || 'عميل');
      setFormContraAccountCode('1131');
      setFormDescription(newVoucherType === 'RECEIPT' ? `تحصيل من العميل ${c?.name}` : `صرف للعميل ${c?.name}`);
    } else if (newPartyType === 'SUPPLIER') {
      const s = suppliers[0];
      setFormSelectedPartyId(s?.id || '');
      setFormPartyName(s?.name || 'مورد');
      setFormContraAccountCode('2111');
      setFormDescription(newVoucherType === 'PAYMENT' ? `سداد للمورد ${s?.name}` : `استرداد من المورد ${s?.name}`);
    } else if (newPartyType === 'EXPENSE') {
      const exp = expenseAccounts[0];
      setFormSelectedPartyId(exp?.code || '');
      setFormPartyName(exp?.name || 'مصروف عام');
      setFormContraAccountCode(exp?.code || '52');
      setFormDescription(`صرف ${exp?.name || 'مصروفات تشغيلية'}`);
    } else {
      setFormSelectedPartyId('');
      setFormPartyName('');
      // In OTHER, select a safe non-treasury contra account (such as accruals or other liabilities/receivables)
      const safeNonTreasury = contraEligibleAccounts.find(a => !isTreasuryCode(a.code));
      setFormContraAccountCode(safeNonTreasury?.code || '2121');
      setFormDescription(newVoucherType === 'RECEIPT' ? 'استلام مبالغ أخرى' : 'صرف مبالغ أخرى');
    }
  };

  // Auto update contra account when party selects change
  const handlePartySelect = (id: string) => {
    setFormSelectedPartyId(id);
    if (formPartyType === 'CUSTOMER') {
      const cust = customers.find((c) => c.id === id);
      if (cust) {
        setFormPartyName(cust.name);
        setFormDescription(newVoucherType === 'RECEIPT' ? `تحصيل من العميل ${cust.name}` : `صرف للعميل ${cust.name}`);
      }
    } else if (formPartyType === 'SUPPLIER') {
      const supp = suppliers.find((s) => s.id === id);
      if (supp) {
        setFormPartyName(supp.name);
        setFormDescription(newVoucherType === 'PAYMENT' ? `سداد للمورد ${supp.name}` : `استرداد من المورد ${supp.name}`);
      }
    } else if (formPartyType === 'EXPENSE') {
      const exp = expenseAccounts.find((e) => e.code === id);
      if (exp) {
        setFormPartyName(exp.name);
        setFormContraAccountCode(exp.code);
        setFormDescription(`صرف ${exp.name}`);
      }
    }
  };

  // Submit New Voucher
  const handleSaveVoucher = (e: React.FormEvent) => {
    e.preventDefault();
    setFormError(null);

    const amountNum = parseFloat(formAmount);
    if (isNaN(amountNum) || amountNum <= 0) {
      setFormError('يرجى إدخال مبلغ صحيح أكبر من الصفر.');
      return;
    }

    if (!formPartyName.trim()) {
      setFormError('يرجى تحديد أو كتابة اسم الجهة المستلمة أو المسلمة.');
      return;
    }

    // Strict Treasury Pre-Posting Validation
    const expectedType = formPaymentMethod === 'CASH' ? 'CASHBOX' : 'BANK';
    const validationRes = validateTreasuryAccount(formCashAccountCode, expectedType);
    if (!validationRes.isValid) {
      setFormError(validationRes.message || 'حساب الصندوق أو البنك المختار غير صحيح.');
      return;
    }

    // Prevent selecting a treasury account as contra account in standard receipt/payment vouchers
    if (isTreasuryCode(formContraAccountCode)) {
      setFormError(
        'خطأ محاسبي: لا يمكن اختيار حساب صندوق (111) أو بنك (112) كطرف مقابل في سند قبض أو صرف اعتيادي. للتحويل ونقل السيولة بين الصناديق والبنوك، يرجى استخدام نافذة "تحويل بين الصناديق والبنوك".'
      );
      return;
    }

    // Check if the date falls in a closed fiscal period
    const closedPeriod = fiscalPeriods.find(
      (fp) => fp.isClosed && formDate >= fp.startDate && formDate <= fp.endDate
    );
    if (closedPeriod) {
      setFormError(
        `لا يمكن إصدار سند بتاريخ ${formDate} لأن هذه الفترة المالية (${closedPeriod.periodName}) مقفلة ومحمية ضد التعديل.`
      );
      return;
    }

    const cashAcc = accounts.find((a) => a.code === formCashAccountCode);
    const contraAcc = accounts.find((a) => a.code === formContraAccountCode);
    const selectedCC = leafCostCenters.find((c) => c.id === formCostCenterId);

    const cashAccName = cashAcc?.name || 'حساب الصندوق / البنك';
    const contraAccName = contraAcc?.name || 'الحساب المقابل';

    const voucherSeq = vouchers.length + 1;
    const voucherNo = String(newVoucherType === 'RECEIPT' ? 1000 + voucherSeq : 2000 + voucherSeq);
    const voucherId = `VOUCH-${Date.now()}`;
    const entryId = `JV-VOUCH-${Date.now()}`;
    const entrySeq = journalEntries.length + 1;
    const entryNo = String(100 + entrySeq);

    // Generate balanced journal entry
    let debitLine = {
      id: 'L1',
      accountCode: formCashAccountCode,
      accountName: cashAccName,
      costCenterId: selectedCC?.id,
      costCenterCode: selectedCC?.code,
      costCenterName: selectedCC?.name,
      debit: amountNum,
      credit: 0,
      note: formDescription || `${voucherNo} - ${formPartyName}`,
    };

    let creditLine = {
      id: 'L2',
      accountCode: formContraAccountCode,
      accountName: contraAccName,
      costCenterId: selectedCC?.id,
      costCenterCode: selectedCC?.code,
      costCenterName: selectedCC?.name,
      debit: 0,
      credit: amountNum,
      note: formDescription || `${voucherNo} - ${formPartyName}`,
    };

    // If PAYMENT, debit is the contra account (e.g. supplier/expense) and credit is cash/bank
    if (newVoucherType === 'PAYMENT') {
      debitLine = {
        id: 'L1',
        accountCode: formContraAccountCode,
        accountName: contraAccName,
        costCenterId: selectedCC?.id,
        costCenterCode: selectedCC?.code,
        costCenterName: selectedCC?.name,
        debit: amountNum,
        credit: 0,
        note: formDescription || `${voucherNo} - ${formPartyName}`,
      };
      creditLine = {
        id: 'L2',
        accountCode: formCashAccountCode,
        accountName: cashAccName,
        costCenterId: selectedCC?.id,
        costCenterCode: selectedCC?.code,
        costCenterName: selectedCC?.name,
        debit: 0,
        credit: amountNum,
        note: formDescription || `${voucherNo} - ${formPartyName}`,
      };
    }

    const generatedEntry: JournalEntry = {
      id: entryId,
      entryNo,
      date: formDate,
      reference: `${voucherNo} (${formReference || 'سند آلي'})`,
      description: `${newVoucherType === 'RECEIPT' ? 'سند قبض' : 'سند صرف'}: ${formDescription} - ${formPartyName}`,
      lines: [debitLine, creditLine],
      totalDebit: amountNum,
      totalCredit: amountNum,
      isBalanced: true,
      status: 'POSTED',
    };

    const newVoucher: CashVoucher = {
      id: voucherId,
      voucherNo,
      type: newVoucherType,
      paymentMethod: formPaymentMethod,
      date: formDate,
      amount: amountNum,
      currency,
      cashAccountCode: formCashAccountCode,
      cashAccountName: cashAccName,
      partyType: formPartyType,
      partyId: formSelectedPartyId || undefined,
      partyName: formPartyName,
      contraAccountCode: formContraAccountCode,
      contraAccountName: contraAccName,
      costCenterId: selectedCC?.id,
      costCenterCode: selectedCC?.code,
      costCenterName: selectedCC?.name,
      reference: formReference,
      description: formDescription,
      status: 'POSTED',
      journalEntryId: entryId,
      createdBy: 'محاسب الفرع',
      createdAt: `${formDate} ${new Date().toLocaleTimeString('ar-YE', { hour: '2-digit', minute: '2-digit' })}`,
    };

    onAddVoucher(newVoucher, generatedEntry);
    setShowNewModal(false);
    // Open preview/print immediately for user convenience
    setPrintingVoucher(newVoucher);
  };

  // Submit Treasury Transfer (تحويل بين الصناديق والبنوك)
  const handleSaveTransfer = (e: React.FormEvent) => {
    e.preventDefault();
    setTransferError(null);

    const amountNum = parseFloat(transferAmount);
    if (isNaN(amountNum) || amountNum <= 0) {
      setTransferError('يرجى إدخال مبلغ تحويل صحيح أكبر من الصفر.');
      return;
    }

    if (!transferFromCode || !transferToCode) {
      setTransferError('يرجى تحديد حساب الصندوق/البنك المحول منه والمحول إليه.');
      return;
    }

    if (transferFromCode === transferToCode) {
      setTransferError('لا يمكن التحويل لنفس الحساب. يرجى اختيار حسابين مختلفين.');
      return;
    }

    if (!isTreasuryCode(transferFromCode) || !isTreasuryCode(transferToCode)) {
      setTransferError('يجب أن تكون حسابات التحويل مقتصرة حصراً على حسابات الصناديق (111) والبنوك (112).');
      return;
    }

    // Check if the date falls in a closed fiscal period
    const closedPeriod = fiscalPeriods.find(
      (fp) => fp.isClosed && transferDate >= fp.startDate && transferDate <= fp.endDate
    );
    if (closedPeriod) {
      setTransferError(
        `لا يمكن إجراء التحويل بتاريخ ${transferDate} لأن الفترة المالية (${closedPeriod.periodName}) مقفلة ومحمية.`
      );
      return;
    }

    const fromAcc = accounts.find((a) => a.code === transferFromCode);
    const toAcc = accounts.find((a) => a.code === transferToCode);
    const fromAccName = fromAcc ? fromAcc.name : transferFromCode;
    const toAccName = toAcc ? toAcc.name : transferToCode;

    const seq = vouchers.length + 1;
    const voucherNo = String(3000 + seq);
    const voucherId = `VOUCH-TR-${Date.now()}`;
    const entryId = `JV-TR-${Date.now()}`;
    const entrySeq = journalEntries.length + 1;
    const entryNo = String(100 + entrySeq);

    // Double Entry Accounting for Treasury Transfer:
    // Debit: Receiving Treasury (الحساب المستلم / المحول إليه)
    // Credit: Sending Treasury (الحساب المسلم / المحول منه)
    const debitLine = {
      id: 'L1',
      accountCode: transferToCode,
      accountName: toAccName,
      debit: amountNum,
      credit: 0,
      note: `استلام تحويل من [${transferFromCode} - ${fromAccName}] | ${transferNote}`,
    };

    const creditLine = {
      id: 'L2',
      accountCode: transferFromCode,
      accountName: fromAccName,
      debit: 0,
      credit: amountNum,
      note: `إرسال تحويل إلى [${transferToCode} - ${toAccName}] | ${transferNote}`,
    };

    const generatedEntry: JournalEntry = {
      id: entryId,
      entryNo,
      date: transferDate,
      reference: `${voucherNo} (${transferReference || 'تحويل خزانة'})`,
      description: `تحويل وتغذية سيولة: من ${fromAccName} إلى ${toAccName}`,
      lines: [debitLine, creditLine],
      totalDebit: amountNum,
      totalCredit: amountNum,
      isBalanced: true,
      status: 'POSTED',
    };

    const isBankInvolved = isBankCode(transferFromCode) || isBankCode(transferToCode);

    const newTransferVoucher: CashVoucher = {
      id: voucherId,
      voucherNo,
      type: 'TRANSFER',
      paymentMethod: isBankInvolved ? 'BANK' : 'CASH',
      date: transferDate,
      amount: amountNum,
      currency,
      cashAccountCode: transferFromCode,
      cashAccountName: fromAccName,
      partyType: 'OTHER',
      partyName: `تحويل خزانة: ${fromAccName} ➔ ${toAccName}`,
      contraAccountCode: transferToCode,
      contraAccountName: toAccName,
      reference: transferReference,
      description: transferNote,
      status: 'POSTED',
      journalEntryId: entryId,
      createdBy: 'محاسب الخزينة',
      createdAt: `${transferDate} ${new Date().toLocaleTimeString('ar-YE', { hour: '2-digit', minute: '2-digit' })}`,
    };

    onAddVoucher(newTransferVoucher, generatedEntry);
    setShowTransferModal(false);
    setPrintingVoucher(newTransferVoucher);
  };

  // Void confirmation
  const handleConfirmVoid = () => {
    if (!voidingVoucher) return;
    if (!voidReasonInput.trim()) {
      alert('يرجى ذكر سبب إلغاء السند المحاسبي للتدقيق الداخلي.');
      return;
    }
    onVoidVoucher(voidingVoucher.id, voidReasonInput);
    setVoidingVoucher(null);
    setVoidReasonInput('');
  };

  return (
    <div className="flex-1 flex flex-col h-full bg-[#f1f5f9] text-slate-800 select-none overflow-hidden font-sans p-2 gap-2">
      {/* 1. Summary Header Cards (WinForms High-Contrast Stat Strip) */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-2 shrink-0">
        <div className="bg-white p-2.5 rounded border border-slate-300 shadow-xs flex items-center justify-between">
          <div>
            <div className="text-[11px] text-emerald-700 font-bold flex items-center gap-1">
              <ArrowDownLeft className="w-3.5 h-3.5" />
              <span>إجمالي سندات القبض (Receipts)</span>
            </div>
            <div className="text-base font-bold font-mono-numbers text-slate-900 mt-0.5">
              {stats.totalReceipts.toLocaleString()} <span className="text-xs font-normal">{currency}</span>
            </div>
            <div className="text-[10px] text-slate-500 mt-0.5">
              عدد السندات: <span className="font-bold">{stats.countReceipts}</span> سند
            </div>
          </div>
          <div className="w-9 h-9 rounded-full bg-emerald-100 flex items-center justify-center text-emerald-700">
            <Receipt className="w-5 h-5" />
          </div>
        </div>

        <div className="bg-white p-2.5 rounded border border-slate-300 shadow-xs flex items-center justify-between">
          <div>
            <div className="text-[11px] text-rose-700 font-bold flex items-center gap-1">
              <ArrowUpRight className="w-3.5 h-3.5" />
              <span>إجمالي سندات الصرف (Payments)</span>
            </div>
            <div className="text-base font-bold font-mono-numbers text-slate-900 mt-0.5">
              {stats.totalPayments.toLocaleString()} <span className="text-xs font-normal">{currency}</span>
            </div>
            <div className="text-[10px] text-slate-500 mt-0.5">
              عدد السندات: <span className="font-bold">{stats.countPayments}</span> سند
            </div>
          </div>
          <div className="w-9 h-9 rounded-full bg-rose-100 flex items-center justify-center text-rose-700">
            <CreditCard className="w-5 h-5" />
          </div>
        </div>

        <div className="bg-white p-2.5 rounded border border-slate-300 shadow-xs flex items-center justify-between">
          <div>
            <div className="text-[11px] text-sky-700 font-bold flex items-center gap-1">
              <DollarSign className="w-3.5 h-3.5" />
              <span>صافي التدفق المالي (Net Flow)</span>
            </div>
            <div className={`text-base font-bold font-mono-numbers mt-0.5 ${stats.netCashFlow >= 0 ? 'text-emerald-700' : 'text-rose-700'}`}>
              {stats.netCashFlow.toLocaleString()} <span className="text-xs font-normal">{currency}</span>
            </div>
            <div className="text-[10px] text-slate-500 mt-0.5">
              {stats.netCashFlow >= 0 ? 'فائض تدفق نقدي' : 'عجز تدفق نقدي'}
            </div>
          </div>
          <div className="w-9 h-9 rounded-full bg-sky-100 flex items-center justify-center text-sky-700">
            <Wallet className="w-5 h-5" />
          </div>
        </div>

        <div className="bg-white p-2 rounded border border-slate-300 shadow-xs flex flex-col justify-between gap-1.5">
          <div className="flex items-center gap-1.5">
            <button
              onClick={() => handleOpenNewVoucher('RECEIPT')}
              className="flex-1 bg-emerald-600 hover:bg-emerald-700 text-white p-1.5 rounded text-xs font-bold flex items-center justify-center gap-1 shadow-xs transition cursor-pointer"
            >
              <Plus className="w-3.5 h-3.5" />
              <span>سند قبض</span>
            </button>

            <button
              onClick={() => handleOpenNewVoucher('PAYMENT')}
              className="flex-1 bg-rose-600 hover:bg-rose-700 text-white p-1.5 rounded text-xs font-bold flex items-center justify-center gap-1 shadow-xs transition cursor-pointer"
            >
              <Plus className="w-3.5 h-3.5" />
              <span>سند صرف</span>
            </button>
          </div>

          <div className="flex items-center gap-1.5">
            <button
              onClick={handleOpenTransferModal}
              className="flex-1 bg-sky-700 hover:bg-sky-800 text-white p-1.5 rounded text-xs font-bold flex items-center justify-center gap-1 shadow-xs transition cursor-pointer"
              title="تحويل وتغذية سيولة بين الصناديق والبنوك (111 ⇄ 112)"
            >
              <ArrowLeftRight className="w-3.5 h-3.5" />
              <span>تحويل خزانة (111 ⇄ 112)</span>
            </button>

            <button
              onClick={() => setShowAdvancesInfoModal(true)}
              className="px-2 py-1.5 bg-amber-50 hover:bg-amber-100 text-amber-900 border border-amber-300 rounded text-[11px] font-semibold flex items-center gap-1 transition cursor-pointer"
              title="عرض تحليل وتوثيق تسوية عهد الموظفين (خارطة طريق ما بعد MVP)"
            >
              <Info className="w-3.5 h-3.5 text-amber-700 shrink-0" />
              <span>العهد (115)</span>
            </button>
          </div>
        </div>
      </div>

      {/* 2. Filter & Search Toolbar */}
      <div className="bg-white p-2 rounded border border-slate-300 shadow-xs flex flex-wrap items-center justify-between gap-2 shrink-0 text-xs">
        <div className="flex items-center flex-wrap gap-2 flex-1 min-w-[280px]">
          <div className="relative flex-1 max-w-xs">
            <Search className="w-3.5 h-3.5 text-slate-400 absolute right-2.5 top-2.5" />
            <input
              type="text"
              placeholder="بحث برقم السند، الجهة، المرجع، الشرح..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-2 pr-8 py-1.5 bg-slate-50 border border-slate-300 rounded text-xs focus:bg-white focus:outline-none focus:border-sky-500 font-sans"
            />
          </div>

          <div className="flex items-center gap-1 bg-slate-100 p-0.5 rounded border border-slate-200">
            <button
              onClick={() => setTypeFilter('ALL')}
              className={`px-2.5 py-1 rounded text-[11px] font-bold transition cursor-pointer ${
                typeFilter === 'ALL' ? 'bg-[#0f2d4a] text-white' : 'text-slate-600 hover:bg-slate-200'
              }`}
            >
              الكل ({vouchers.length})
            </button>
            <button
              onClick={() => setTypeFilter('RECEIPT')}
              className={`px-2.5 py-1 rounded text-[11px] font-bold transition cursor-pointer flex items-center gap-1 ${
                typeFilter === 'RECEIPT' ? 'bg-emerald-700 text-white' : 'text-emerald-700 hover:bg-emerald-50'
              }`}
            >
              <ArrowDownLeft className="w-3 h-3" />
              <span>قبض</span>
            </button>
            <button
              onClick={() => setTypeFilter('PAYMENT')}
              className={`px-2.5 py-1 rounded text-[11px] font-bold transition cursor-pointer flex items-center gap-1 ${
                typeFilter === 'PAYMENT' ? 'bg-rose-700 text-white' : 'text-rose-700 hover:bg-rose-50'
              }`}
            >
              <ArrowUpRight className="w-3 h-3" />
              <span>صرف</span>
            </button>
            <button
              onClick={() => setTypeFilter('TRANSFER')}
              className={`px-2.5 py-1 rounded text-[11px] font-bold transition cursor-pointer flex items-center gap-1 ${
                typeFilter === 'TRANSFER' ? 'bg-sky-700 text-white' : 'text-sky-700 hover:bg-sky-50'
              }`}
            >
              <ArrowLeftRight className="w-3 h-3" />
              <span>تحويل</span>
            </button>
          </div>

          <div className="flex items-center gap-1 bg-slate-100 p-0.5 rounded border border-slate-200">
            <button
              onClick={() => setMethodFilter('ALL')}
              className={`px-2 py-1 rounded text-[11px] font-bold transition cursor-pointer ${
                methodFilter === 'ALL' ? 'bg-slate-700 text-white' : 'text-slate-600 hover:bg-slate-200'
              }`}
            >
              الكل
            </button>
            <button
              onClick={() => setMethodFilter('CASH')}
              className={`px-2 py-1 rounded text-[11px] font-bold transition cursor-pointer flex items-center gap-1 ${
                methodFilter === 'CASH' ? 'bg-amber-700 text-white' : 'text-amber-800 hover:bg-amber-50'
              }`}
            >
              <Wallet className="w-3 h-3" />
              <span>نقداً</span>
            </button>
            <button
              onClick={() => setMethodFilter('BANK')}
              className={`px-2 py-1 rounded text-[11px] font-bold transition cursor-pointer flex items-center gap-1 ${
                methodFilter === 'BANK' ? 'bg-indigo-700 text-white' : 'text-indigo-800 hover:bg-indigo-50'
              }`}
            >
              <Landmark className="w-3 h-3" />
              <span>بنك/شيك</span>
            </button>
          </div>
        </div>

        <div className="flex items-center gap-2">
          <select
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value as any)}
            className="bg-slate-50 border border-slate-300 rounded px-2 py-1 text-xs text-slate-700 font-sans focus:outline-none"
          >
            <option value="ALL">جميع الحالات</option>
            <option value="POSTED">المرحلة والمثبتة (POSTED)</option>
            <option value="VOID">الملغاة فقط (VOID)</option>
          </select>
        </div>
      </div>

      {/* 3. Vouchers DataGridView (WinForms Style) */}
      <div className="flex-1 bg-white rounded border border-slate-300 shadow-xs overflow-hidden flex flex-col">
        <div className="overflow-x-auto flex-1">
          <table className="w-full text-right text-xs border-collapse font-sans">
            <thead className="bg-[#1e3a5f] text-white sticky top-0 z-10 select-none text-[11px]">
              <tr>
                <th className="p-2 border-l border-slate-700 w-28">رقم السند</th>
                <th className="p-2 border-l border-slate-700 w-24">التاريخ</th>
                <th className="p-2 border-l border-slate-700 w-28">نوع السند</th>
                <th className="p-2 border-l border-slate-700 w-28">طريقة الدفع</th>
                <th className="p-2 border-l border-slate-700">الصندوق / البنك</th>
                <th className="p-2 border-l border-slate-700">الحساب المقابل / الجهة</th>
                <th className="p-2 border-l border-slate-700">مركز التكلفة</th>
                <th className="p-2 border-l border-slate-700 text-left w-28">المبلغ ({currency})</th>
                <th className="p-2 border-l border-slate-700 w-36">المرجع / الشيك</th>
                <th className="p-2 border-l border-slate-700 w-20 text-center">الحالة</th>
                <th className="p-2 w-28 text-center">إجراءات</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-200">
              {filteredVouchers.length === 0 ? (
                <tr>
                  <td colSpan={11} className="p-8 text-center text-slate-400 text-xs">
                    لا توجد سندات مطابقة لشروط البحث والتصفية المحددة.
                  </td>
                </tr>
              ) : (
                filteredVouchers.map((v, idx) => {
                  const isVoid = v.status === 'VOID';
                  return (
                    <tr
                      key={v.id}
                      className={`transition ${
                        isVoid
                          ? 'bg-rose-50/50 text-rose-900 line-through opacity-70'
                          : idx % 2 === 0
                          ? 'bg-white hover:bg-sky-50/60'
                          : 'bg-slate-50/60 hover:bg-sky-50/60'
                      }`}
                    >
                      <td className="p-2 font-mono font-bold text-slate-900 border-l border-slate-200 whitespace-nowrap">
                        <span
                          className={`px-1.5 py-0.5 rounded text-[10px] ${
                            v.type === 'RECEIPT'
                              ? 'bg-emerald-100 text-emerald-800 border border-emerald-300'
                              : v.type === 'PAYMENT'
                              ? 'bg-rose-100 text-rose-800 border border-rose-300'
                              : 'bg-sky-100 text-sky-800 border border-sky-300'
                          }`}
                        >
                          {v.voucherNo}
                        </span>
                      </td>

                      <td className="p-2 font-mono text-slate-600 border-l border-slate-200 whitespace-nowrap text-[11px]">
                        {v.date}
                      </td>

                      <td className="p-2 border-l border-slate-200 whitespace-nowrap">
                        {v.type === 'RECEIPT' ? (
                          <span className="inline-flex items-center gap-1 text-emerald-700 font-bold text-[11px]">
                            <ArrowDownLeft className="w-3.5 h-3.5" />
                            <span>سند قبض</span>
                          </span>
                        ) : v.type === 'PAYMENT' ? (
                          <span className="inline-flex items-center gap-1 text-rose-700 font-bold text-[11px]">
                            <ArrowUpRight className="w-3.5 h-3.5" />
                            <span>سند صرف</span>
                          </span>
                        ) : (
                          <span className="inline-flex items-center gap-1 text-sky-700 font-bold text-[11px]">
                            <ArrowLeftRight className="w-3.5 h-3.5" />
                            <span>تحويل خزانة</span>
                          </span>
                        )}
                      </td>

                      <td className="p-2 border-l border-slate-200 whitespace-nowrap">
                        <span
                          className={`px-1.5 py-0.5 rounded text-[10px] font-semibold ${
                            v.paymentMethod === 'CASH'
                              ? 'bg-amber-100 text-amber-900'
                              : 'bg-indigo-100 text-indigo-900'
                          }`}
                        >
                          {v.paymentMethod === 'CASH' ? 'نقداً (خزينة)' : 'بنك / شيك'}
                        </span>
                      </td>

                      <td className="p-2 border-l border-slate-200 max-w-[160px] truncate" title={v.cashAccountName}>
                        <div className="font-semibold text-slate-800 truncate">{v.cashAccountName}</div>
                        <div className="font-mono text-[10px] text-slate-500">{v.cashAccountCode}</div>
                      </td>

                      <td className="p-2 border-l border-slate-200 max-w-[200px]" title={v.partyName}>
                        <div className="font-bold text-slate-900 truncate">{v.partyName}</div>
                        <div className="text-[10px] text-slate-500 truncate flex items-center gap-1">
                          <span className="font-mono">({v.contraAccountCode})</span>
                          <span>{v.contraAccountName}</span>
                        </div>
                      </td>

                      <td className="p-2 border-l border-slate-200 whitespace-nowrap text-[11px]">
                        {v.costCenterName ? (
                          <span className="bg-indigo-50 text-indigo-700 border border-indigo-200 px-1.5 py-0.5 rounded text-[10px] font-medium flex items-center gap-1">
                            <Building2 className="w-3 h-3 shrink-0" />
                            <span className="truncate max-w-[120px]">{v.costCenterName}</span>
                          </span>
                        ) : (
                          <span className="text-slate-400">-</span>
                        )}
                      </td>

                      <td className="p-2 border-l border-slate-200 font-mono-numbers font-bold text-left whitespace-nowrap">
                        <span className={v.type === 'RECEIPT' ? 'text-emerald-700' : 'text-rose-700'}>
                          {v.amount.toLocaleString()}
                        </span>
                      </td>

                      <td className="p-2 border-l border-slate-200 text-slate-600 text-[11px] truncate max-w-[140px]" title={v.reference || v.description}>
                        <div>{v.reference || '-'}</div>
                        <div className="text-[10px] text-slate-400 truncate">{v.description}</div>
                      </td>

                      <td className="p-2 border-l border-slate-200 text-center whitespace-nowrap">
                        {isVoid ? (
                          <span className="bg-rose-200 text-rose-900 px-1.5 py-0.5 rounded font-bold text-[10px]">
                            ملغي (VOID)
                          </span>
                        ) : (
                          <span className="bg-emerald-100 text-emerald-800 px-1.5 py-0.5 rounded font-bold text-[10px] flex items-center justify-center gap-0.5">
                            <CheckCircle2 className="w-3 h-3" />
                            <span>مرحل</span>
                          </span>
                        )}
                      </td>

                      <td className="p-2 text-center whitespace-nowrap">
                        <div className="flex items-center justify-center gap-1">
                          <button
                            onClick={() => setPrintingVoucher(v)}
                            title="معاينة وطباعة السند"
                            className="p-1 text-slate-600 hover:text-sky-700 hover:bg-slate-200 rounded transition cursor-pointer"
                          >
                            <Printer className="w-3.5 h-3.5" />
                          </button>

                          <button
                            onClick={() => setViewingVoucher(v)}
                            title="عرض تفاصيل السند والقيد المحاسبي"
                            className="p-1 text-slate-600 hover:text-emerald-700 hover:bg-slate-200 rounded transition cursor-pointer"
                          >
                            <Eye className="w-3.5 h-3.5" />
                          </button>

                          {!isVoid && (
                            <button
                              onClick={() => setVoidingVoucher(v)}
                              title="إلغاء السند (Void)"
                              className="p-1 text-slate-400 hover:text-rose-600 hover:bg-rose-50 rounded transition cursor-pointer"
                            >
                              <Ban className="w-3.5 h-3.5" />
                            </button>
                          )}
                        </div>
                      </td>
                    </tr>
                  );
                })
              )}
            </tbody>
          </table>
        </div>

        {/* Footer Summary Row */}
        <div className="bg-[#1e293b] text-white px-3 py-1.5 text-xs flex items-center justify-between shrink-0 border-t border-slate-300">
          <div className="flex items-center gap-4">
            <span>عدد السندات المعروضة: <b>{filteredVouchers.length}</b></span>
            <span>إجمالي المقبوضات المعروضة: <b className="text-emerald-400 font-mono-numbers">{filteredVouchers.filter(v => v.type === 'RECEIPT' && v.status !== 'VOID').reduce((s, v) => s + v.amount, 0).toLocaleString()}</b> {currency}</span>
            <span>إجمالي المدفوعات المعروضة: <b className="text-rose-400 font-mono-numbers">{filteredVouchers.filter(v => v.type === 'PAYMENT' && v.status !== 'VOID').reduce((s, v) => s + v.amount, 0).toLocaleString()}</b> {currency}</span>
          </div>
          <div className="text-slate-400 text-[11px]">
            نظام السندات المحاسبية المتكامل - متوافق مع معايير المحاسبة العامة
          </div>
        </div>
      </div>

      {/* 4. Modal: Issue New Voucher (WinForms Dialog) */}
      <CashVoucherNewModal
        showNewModal={showNewModal}
        setShowNewModal={setShowNewModal}
        newVoucherType={newVoucherType}
        setNewVoucherType={setNewVoucherType}
        handleSaveVoucher={handleSaveVoucher}
        formDate={formDate}
        setFormDate={setFormDate}
        formPaymentMethod={formPaymentMethod}
        handlePaymentMethodSwitch={handlePaymentMethodSwitch}
        formCashAccountCode={formCashAccountCode}
        setFormCashAccountCode={setFormCashAccountCode}
        formPartyType={formPartyType}
        handlePartyTypeChange={handlePartyTypeChange}
        formSelectedPartyId={formSelectedPartyId}
        handlePartySelect={handlePartySelect}
        formPartyName={formPartyName}
        setFormPartyName={setFormPartyName}
        formContraAccountCode={formContraAccountCode}
        setFormContraAccountCode={setFormContraAccountCode}
        formCostCenterId={formCostCenterId}
        setFormCostCenterId={setFormCostCenterId}
        formAmount={formAmount}
        setFormAmount={setFormAmount}
        formReference={formReference}
        setFormReference={setFormReference}
        formDescription={formDescription}
        setFormDescription={setFormDescription}
        formError={formError}
        selectableTreasuryAccounts={selectableTreasuryAccounts}
        contraEligibleAccounts={contraEligibleAccounts}
        customers={customers}
        suppliers={suppliers}
        expenseAccounts={expenseAccounts}
        leafCostCenters={leafCostCenters}
        currency={currency}
      />

      {/* 5. Printable Official Voucher Modal (Print / Preview Voucher) */}
      <CashVoucherPrintModal
        printingVoucher={printingVoucher}
        setPrintingVoucher={setPrintingVoucher}
        currency={currency}
      />

      {/* 6. Modal: View Voucher Details & Linked Journal Entry */}
      <CashVoucherViewModal
        viewingVoucher={viewingVoucher}
        setViewingVoucher={setViewingVoucher}
        setPrintingVoucher={setPrintingVoucher}
        currency={currency}
        chartOfAccounts={accounts}
      />

      {/* 7. Modal: Void Voucher Confirmation */}
      <CashVoucherVoidModal
        voidingVoucher={voidingVoucher}
        setVoidingVoucher={setVoidingVoucher}
        voidReasonInput={voidReasonInput}
        setVoidReasonInput={setVoidReasonInput}
        handleConfirmVoid={handleConfirmVoid}
        currency={currency}
      />

      {/* 6. Modal: Treasury Transfer (تحويل وتغذية سيولة بين الصناديق والبنوك 111 ⇄ 112) */}
      <CashVoucherTransferModal
        showTransferModal={showTransferModal}
        setShowTransferModal={setShowTransferModal}
        handleTreasuryTransfer={handleSaveTransfer}
        transferFromCode={transferFromCode}
        setTransferFromCode={setTransferFromCode}
        transferToCode={transferToCode}
        setTransferToCode={setTransferToCode}
        transferAmount={transferAmount}
        setTransferAmount={setTransferAmount}
        transferDate={transferDate}
        setTransferDate={setTransferDate}
        transferReference={transferReference}
        setTransferReference={setTransferReference}
        transferNote={transferNote}
        setTransferNote={setTransferNote}
        transferError={transferError}
        treasuryAndBankAccounts={allTreasuryAccounts}
        currency={currency}
      />

      {/* 7. Modal: Employee Advances & Custody Roadmap (توثيق تحليل العهد المالية للموظفين) */}
      <CashVoucherAdvancesModal
        showAdvancesInfoModal={showAdvancesInfoModal}
        setShowAdvancesInfoModal={setShowAdvancesInfoModal}
      />

    </div>
  );
};
