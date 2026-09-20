import { 
  Customer, 
  Invoice, 
  CashVoucher, 
  JournalEntry, 
  CustomerStatementMovement, 
  CustomerAgingBucket 
} from '@/types/pos';

export const INITIAL_CUSTOMERS: Customer[] = [
  {
    id: 'CUST-001',
    code: '113001',
    name: 'عميل نقدي عام (نقاط البيع)',
    phone: '770000000',
    address: 'المحل الرئيسي',
    creditLimit: 0,
    currentBalance: 0,
    accountCode: '113001',
    status: 'ACTIVE',
    contactPerson: 'كاشير الصالة',
    notes: 'حساب افتراضي لعمليات البيع النقدي المباشر',
  },
  {
    id: 'CUST-002',
    code: '113002',
    name: 'بقالة الأمل والخير - شارع حدة',
    phone: '771234567',
    address: 'صنعاء - تقاطع شارع حدة مع الدائري',
    creditLimit: 300000,
    currentBalance: 145000,
    accountCode: '113002',
    taxNumber: 'TAX-YE-102938',
    email: 'amal.store@gmail.com',
    contactPerson: 'الأخ أحمد عبده (المدير المسؤول)',
    creditDays: 30,
    status: 'ACTIVE',
    notes: 'عميل جملة تجزئة ملتزم بالسداد خلال فترة السماح',
    openingBalance: 50000,
    openingBalanceDate: '2026-01-01',
  },
  {
    id: 'CUST-003',
    code: '113003',
    name: 'سوبرماركت المدينة المنورة',
    phone: '777889900',
    address: 'صنعاء - حي الأصبحي / شارع المقالح',
    creditLimit: 500000,
    currentBalance: 85000,
    accountCode: '113003',
    taxNumber: 'TAX-YE-554433',
    email: 'madina.pos@yahoo.com',
    contactPerson: 'الشيخ محمد المطري',
    creditDays: 45,
    status: 'ACTIVE',
    notes: 'عميل رئيسي وموزع تجاري',
    openingBalance: 0,
    openingBalanceDate: '2026-01-01',
  },
  {
    id: 'CUST-004',
    code: '113004',
    name: 'كافيه وكافتيريا السعادة الحديثة',
    phone: '711223344',
    address: 'صنعاء - حدة المدينة / جوار بريد حدة',
    creditLimit: 150000,
    currentBalance: 0,
    accountCode: '113004',
    taxNumber: 'TAX-YE-998811',
    email: 'saadah.cafe@gmail.com',
    contactPerson: 'عصام الحكيمي',
    creditDays: 15,
    status: 'ACTIVE',
    notes: 'سداد نقدي أسبوعي منتظم',
    openingBalance: 0,
    openingBalanceDate: '2026-01-01',
  },
  {
    id: 'CUST-005',
    code: '113005',
    name: 'فندق بابل الدولي ومطاعمه',
    phone: '734112233',
    address: 'عدن - خور مكسر / الكورنيش',
    creditLimit: 600000,
    currentBalance: 520000,
    accountCode: '113005',
    taxNumber: 'TAX-YE-44102',
    email: 'purchasing@babel-hotel.com',
    contactPerson: 'أ. وضاح اليافعي (مدير المشتريات)',
    creditDays: 60,
    status: 'ACTIVE',
    notes: 'متجاوز لسقف الائتمان جزئياً - يحتاج متابعة تحصيل',
    openingBalance: 150000,
    openingBalanceDate: '2026-01-01',
  },
];

/**
 * Calculates a detailed Customer Statement of Account (كشف حساب عميل تفصيلي)
 * Supports running balance, date ranges, and prior opening balance accumulation.
 */
export function calculateCustomerStatement(params: {
  customerId: string;
  customers: Customer[];
  salesInvoices: Invoice[];
  vouchers: CashVoucher[];
  journalEntries: JournalEntry[];
  dateFrom?: string;
  dateTo?: string;
}): {
  customer?: Customer;
  openingBalance: number;
  movements: CustomerStatementMovement[];
  totalDebit: number;
  totalCredit: number;
  closingBalance: number;
} {
  const { customerId, customers, salesInvoices, vouchers, journalEntries, dateFrom, dateTo } = params;
  const customer = customers.find((c) => c.id === customerId);
  if (!customer) {
    return { openingBalance: 0, movements: [], totalDebit: 0, totalCredit: 0, closingBalance: 0 };
  }

  interface RawCustomerMovement {
    id: string;
    date: string;
    time?: string;
    sortKey: string;
    type: 'OPENING_BALANCE' | 'SALE_INVOICE' | 'SALE_RETURN' | 'RECEIPT_VOUCHER' | 'PAYMENT_VOUCHER' | 'JOURNAL_ENTRY';
    typeLabel: string;
    documentNo: string;
    referenceNo?: string;
    description: string;
    debit: number;
    credit: number;
    status?: string;
  }

  const rawMovements: RawCustomerMovement[] = [];

  // 1. Sales Invoices & Credit Sales
  salesInvoices.forEach((inv) => {
    if (inv.customerOrSupplierId !== customerId) return;

    if (inv.type === 'SALE') {
      // If full credit sale
      if (inv.paymentType === 'CREDIT') {
        rawMovements.push({
          id: `INV-SALE-${inv.id}`,
          date: inv.date,
          time: inv.time || '10:00 ص',
          sortKey: `${inv.date}T${inv.time || '10:00:00'}`,
          type: 'SALE_INVOICE',
          typeLabel: 'فاتورة مبيعات آجلة',
          documentNo: inv.invoiceNo,
          referenceNo: inv.id,
          description: `فاتورة مبيعات آجلة [${inv.items.length} صنف] - ${inv.notes || 'سحب بضاعة على الحساب'}`,
          debit: inv.grandTotal,
          credit: 0,
          status: inv.remainingAmount === 0 ? 'مسددة بالكامل' : inv.paidAmount > 0 ? 'مسددة جزئياً' : 'مستحقة غير مسددة',
        });

        // If a down payment was paid at invoice generation
        if (inv.paidAmount > 0) {
          rawMovements.push({
            id: `INV-DOWNPAY-${inv.id}`,
            date: inv.date,
            time: inv.time || '10:01 ص',
            sortKey: `${inv.date}T${inv.time || '10:00:01'}`,
            type: 'RECEIPT_VOUCHER',
            typeLabel: 'دفعة نقدية مع الفاتورة',
            documentNo: `DP-${inv.invoiceNo}`,
            referenceNo: inv.invoiceNo,
            description: `دفعة نقدية مسددة فورياً مع الفاتورة ${inv.invoiceNo}`,
            debit: 0,
            credit: inv.paidAmount,
            status: 'مقبوضة',
          });
        }
      } else {
        // Cash or e-wallet sale assigned to this customer
        rawMovements.push({
          id: `INV-CASH-${inv.id}`,
          date: inv.date,
          time: inv.time || '10:00 ص',
          sortKey: `${inv.date}T${inv.time || '10:00:00'}`,
          type: 'SALE_INVOICE',
          typeLabel: 'فاتورة مبيعات نقدية للعميل',
          documentNo: inv.invoiceNo,
          referenceNo: inv.id,
          description: `فاتورة مبيعات فورية مسددة (${inv.paymentType === 'CASH' ? 'نقداً' : 'تحويل/محفظة'})`,
          debit: inv.grandTotal,
          credit: inv.grandTotal,
          status: 'مسددة فوري',
        });
      }
    } else if (inv.type === 'SALE_RETURN') {
      // Sales return
      rawMovements.push({
        id: `INV-RET-${inv.id}`,
        date: inv.date,
        time: inv.time || '11:00 ص',
        sortKey: `${inv.date}T${inv.time || '11:00:00'}`,
        type: 'SALE_RETURN',
        typeLabel: 'مردود مبيعات',
        documentNo: inv.invoiceNo,
        referenceNo: inv.id,
        description: `مردود مبيعات قيد دائن للعميل [${inv.items.length} صنف]`,
        debit: 0,
        credit: inv.grandTotal,
        status: 'مردودة',
      });
    }
  });

  // 2. Receipt and Payment Vouchers
  vouchers.forEach((vouch) => {
    if (vouch.status === 'VOID') return;
    const isThisCustomer =
      vouch.partyId === customerId ||
      vouch.contraAccountCode === customer.accountCode ||
      vouch.partyName.includes(customer.name);

    if (!isThisCustomer) return;

    if (vouch.type === 'RECEIPT') {
      const payMethodLabel =
        vouch.paymentMethod === 'CASH'
          ? 'نقداً (الصندوق)'
          : vouch.paymentMethod === 'BANK'
          ? `تحويل بنكي (${vouch.bankName || 'البنك'})`
          : `شيك رقم (${vouch.checkNumber || vouch.reference || '-'})`;

      rawMovements.push({
        id: `VOUCH-${vouch.id}`,
        date: vouch.date,
        time: '12:00 م',
        sortKey: `${vouch.date}T12:00:00`,
        type: 'RECEIPT_VOUCHER',
        typeLabel: 'سند قبض مالي',
        documentNo: vouch.voucherNo,
        referenceNo: vouch.reference,
        description: `سند قبض ${payMethodLabel} - ${vouch.description || 'سداد من الحساب'}`,
        debit: 0,
        credit: vouch.amount,
        status: 'مرحل ومقيد',
      });
    } else if (vouch.type === 'PAYMENT') {
      rawMovements.push({
        id: `VOUCH-${vouch.id}`,
        date: vouch.date,
        time: '12:30 م',
        sortKey: `${vouch.date}T12:30:00`,
        type: 'PAYMENT_VOUCHER',
        typeLabel: 'سند صرف / رد مالي',
        documentNo: vouch.voucherNo,
        referenceNo: vouch.reference,
        description: `سند صرف / رد تأمين أو مبلغ دائن للعميل - ${vouch.description}`,
        debit: vouch.amount,
        credit: 0,
        status: 'مرحل ومقيد',
      });
    }
  });

  // 3. Direct Journal Entries linked to Customer Account
  journalEntries.forEach((entry) => {
    if (entry.status !== 'POSTED') return;
    entry.lines.forEach((line) => {
      if (line.accountCode === customer.accountCode || line.accountCode === `11300${customer.code.slice(-1)}`) {
        rawMovements.push({
          id: `JV-${entry.id}-${line.id}`,
          date: entry.date,
          time: '01:00 م',
          sortKey: `${entry.date}T13:00:00`,
          type: 'JOURNAL_ENTRY',
          typeLabel: 'قيد تسوية يومية',
          documentNo: entry.entryNo,
          referenceNo: entry.reference,
          description: `قيد محاسبي: ${line.note || entry.description}`,
          debit: line.debit,
          credit: line.credit,
          status: 'مرحل',
        });
      }
    });
  });

  // Sort chronologically
  rawMovements.sort((a, b) => a.sortKey.localeCompare(b.sortKey));

  // Partition into pre-period and period
  let runningOpeningBalance = customer.openingBalance || 0;
  const filteredPeriodMovements: RawCustomerMovement[] = [];

  rawMovements.forEach((m) => {
    if (dateFrom && m.date < dateFrom) {
      runningOpeningBalance += (m.debit - m.credit);
    } else if (dateTo && m.date > dateTo) {
      // after period, ignore for this statement
    } else {
      filteredPeriodMovements.push(m);
    }
  });

  let currentBalance = runningOpeningBalance;
  let totalDebit = 0;
  let totalCredit = 0;

  const finalMovements: CustomerStatementMovement[] = filteredPeriodMovements.map((m) => {
    currentBalance += (m.debit - m.credit);
    totalDebit += m.debit;
    totalCredit += m.credit;

    return {
      id: m.id,
      date: m.date,
      time: m.time,
      type: m.type,
      typeLabel: m.typeLabel,
      documentNo: m.documentNo,
      referenceNo: m.referenceNo,
      description: m.description,
      debit: m.debit,
      credit: m.credit,
      balanceAfter: currentBalance,
      status: m.status,
    };
  });

  return {
    customer,
    openingBalance: runningOpeningBalance,
    movements: finalMovements,
    totalDebit,
    totalCredit,
    closingBalance: currentBalance,
  };
}

/**
 * Calculates Debt Aging Buckets (تحليل أعمار ديون العملاء)
 */
export function calculateCustomerAging(params: {
  customerId?: string;
  salesInvoices: Invoice[];
  asOfDate?: string;
}): {
  buckets: CustomerAgingBucket[];
  totalOverdue: number;
  oldestInvoiceDays: number;
} {
  const { customerId, salesInvoices, asOfDate } = params;
  const referenceDate = asOfDate ? new Date(asOfDate) : new Date();

  const creditInvoices = salesInvoices.filter((inv) => {
    if (inv.type !== 'SALE' || inv.paymentType !== 'CREDIT') return false;
    if (inv.remainingAmount <= 0) return false;
    if (customerId && inv.customerOrSupplierId !== customerId) return false;
    return true;
  });

  let b0_30 = { count: 0, amount: 0 };
  let b31_60 = { count: 0, amount: 0 };
  let b61_90 = { count: 0, amount: 0 };
  let b91_plus = { count: 0, amount: 0 };
  let oldestDays = 0;

  creditInvoices.forEach((inv) => {
    const invDate = new Date(inv.date);
    const diffTime = Math.max(0, referenceDate.getTime() - invDate.getTime());
    const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24));

    if (diffDays > oldestDays) {
      oldestDays = diffDays;
    }

    if (diffDays <= 30) {
      b0_30.count += 1;
      b0_30.amount += inv.remainingAmount;
    } else if (diffDays <= 60) {
      b31_60.count += 1;
      b31_60.amount += inv.remainingAmount;
    } else if (diffDays <= 90) {
      b61_90.count += 1;
      b61_90.amount += inv.remainingAmount;
    } else {
      b91_plus.count += 1;
      b91_plus.amount += inv.remainingAmount;
    }
  });

  const totalOverdue = b0_30.amount + b31_60.amount + b61_90.amount + b91_plus.amount;

  const buckets: CustomerAgingBucket[] = [
    {
      periodLabel: '0 - 30 يوم (حالي وغير متأخر)',
      amount: b0_30.amount,
      invoiceCount: b0_30.count,
      percentage: totalOverdue > 0 ? Math.round((b0_30.amount / totalOverdue) * 100) : 0,
    },
    {
      periodLabel: '31 - 60 يوم (متأخر درجة أولى)',
      amount: b31_60.amount,
      invoiceCount: b31_60.count,
      percentage: totalOverdue > 0 ? Math.round((b31_60.amount / totalOverdue) * 100) : 0,
    },
    {
      periodLabel: '61 - 90 يوم (متأخر درجة ثانية)',
      amount: b61_90.amount,
      invoiceCount: b61_90.count,
      percentage: totalOverdue > 0 ? Math.round((b61_90.amount / totalOverdue) * 100) : 0,
    },
    {
      periodLabel: '+90 يوم (ديون متأخرة وحرجة)',
      amount: b91_plus.amount,
      invoiceCount: b91_plus.count,
      percentage: totalOverdue > 0 ? Math.round((b91_plus.amount / totalOverdue) * 100) : 0,
    },
  ];

  return { buckets, totalOverdue, oldestInvoiceDays: oldestDays };
}
