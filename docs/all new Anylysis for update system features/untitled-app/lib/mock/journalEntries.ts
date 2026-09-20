import { JournalEntry } from '@/types/pos';

export const INITIAL_JOURNAL_ENTRIES: JournalEntry[] = [
  {
    id: 'JV-2025-001',
    entryNo: '101',
    date: '2026-08-30',
    reference: 'افتتاح الخزينة والوردية الأولى',
    description: 'قيد إيداع الرصيد الافتتاحي في الصندوق الرئيسي',
    lines: [
      { id: 'L1', accountCode: '1111', accountName: 'الصندوق الرئيسي (الخزينة المركزية)', debit: 500000, credit: 0, note: 'رصيد افتتاحي كاش' },
      { id: 'L2', accountCode: '221', accountName: 'رأس المال المدفوع', debit: 0, credit: 500000, note: 'إيداع من رأس المال' },
    ],
    totalDebit: 500000,
    totalCredit: 500000,
    isBalanced: true,
    status: 'POSTED',
  },
  {
    id: 'JV-2025-002',
    entryNo: '102',
    date: '2026-08-31',
    reference: 'سداد دفعة للمورد هائل سعيد',
    description: 'تحويل بنكي من حساب الكريمي لحساب المورد هائل سعيد',
    lines: [
      { id: 'L3', accountCode: '211', accountName: 'حسابات الموردين والدائنون', debit: 200000, credit: 0, note: 'سداد دفعة من حساب هائل سعيد' },
      { id: 'L4', accountCode: '1121', accountName: 'حساب بنك الكريمي للتمويل الأصغر (YER)', debit: 0, credit: 200000, note: 'تحويل عبر تطبيق الكريمي المميز' },
    ],
    totalDebit: 200000,
    totalCredit: 200000,
    isBalanced: true,
    status: 'POSTED',
  },
  {
    id: 'JV-2025-003',
    entryNo: '103',
    date: '2026-09-01',
    reference: 'سداد إيجار وكهرباء الفروع التشغيلية',
    description: 'صرف إيجار فرع حدة وفرع التحرير نقداً محمل على مراكز التكلفة',
    lines: [
      { 
        id: 'L5', 
        accountCode: '421', 
        accountName: 'مصروفات الإيجار والكهرباء والمياه', 
        costCenterId: 'CC-201', 
        costCenterCode: '201', 
        costCenterName: 'فرع حدة الرئيسي (صنعاء)', 
        debit: 45000, 
        credit: 0, 
        note: 'إيجار فرع حدة لشهر سبتمبر' 
      },
      { 
        id: 'L6', 
        accountCode: '421', 
        accountName: 'مصروفات الإيجار والكهرباء والمياه', 
        costCenterId: 'CC-202', 
        costCenterCode: '202', 
        costCenterName: 'فرع التحرير التجاري (صنعاء)', 
        debit: 30000, 
        credit: 0, 
        note: 'إيجار فرع التحرير لشهر سبتمبر' 
      },
      { 
        id: 'L7', 
        accountCode: '1111', 
        accountName: 'الصندوق الرئيسي (الخزينة المركزية)', 
        debit: 0, 
        credit: 75000, 
        note: 'صرف كاش بموجب سند صرف رقم 2002' 
      },
    ],
    totalDebit: 75000,
    totalCredit: 75000,
    isBalanced: true,
    status: 'POSTED',
  },
  {
    id: 'JV-2025-004',
    entryNo: '104',
    date: '2026-09-02',
    reference: 'صيانة خوادم وشبكات نقاط البيع',
    description: 'فاتورة صيانة وتحديث أجهزة IT محملة على مركز تقنية المعلومات',
    lines: [
      { 
        id: 'L8', 
        accountCode: '423', 
        accountName: 'مصروفات الصيانة وضيافة المحل', 
        costCenterId: 'CC-102', 
        costCenterCode: '102', 
        costCenterName: 'إدارة تكنولوجيا المعلومات ونقاط البيع IT', 
        debit: 8000, 
        credit: 0, 
        note: 'صيانة سيرفرات وشبكة POS الفرعية' 
      },
      { 
        id: 'L9', 
        accountCode: '1111', 
        accountName: 'الصندوق الرئيسي (الخزينة المركزية)', 
        debit: 0, 
        credit: 8000, 
        note: 'سداد نقدي لمهندس الصيانة' 
      },
    ],
    totalDebit: 8000,
    totalCredit: 8000,
    isBalanced: true,
    status: 'POSTED',
  },
];
