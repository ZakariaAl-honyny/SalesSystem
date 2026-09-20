/**
 * Treasury & Chart of Accounts Linking Utilities (MVP Standard)
 * 
 * تحليلات وضوابط الحسابات النقدية والبنكية لمنع الأخطاء المحاسبية
 * 
 * =========================================================================================
 * التحليل المحاسبي للنظام المصغر (MVP Core):
 * 1. حسابات الصناديق والخزائن النقدية:
 *    - تبدأ حصرياً بالكود (111...) في الدليل المحاسبي.
 * 2. حسابات البنوك والمحافظ الإلكترونية:
 *    - تبدأ حصرياً بالكود (112...) في الدليل المحاسبي.
 * 3. منع الإدراج الخاطئ (Anti-Mistake Validation):
 *    - حقول الصناديق/البنوك في جميع الشاشات تعزل وتصفي الشجرة لتقتصر على (111 و 112).
 *    - الطرف المقابل في سندات القبض والصرف يستثني حسابات (111 و 112) لمنع التكرار
 *      (إلا في شاشة التحويل بين الصناديق والبنوك Treasury Transfer).
 * 4. سياسة السداد في المبيعات:
 *    - نقد (Cash): يختار المستخدم صندوقاً أو بنكاً، ويجب السداد بالكامل (Paid = GrandTotal).
 *    - آجل (Credit): يمكن سداد دفعة مقدمة (أو 0). المبلغ المدفوع يرحل فوراً إلى
 *      الصندوق/البنك المختار، والمتبقي يرحل إلى حساب العميل في المدينون (113).
 * 5. فواتير المشتريات والمصروفات:
 *    - الدفع النقدي يتم حصرياً عبر حسابات الصناديق أو البنوك المعتمدة.
 * 6. التحويل بين الصناديق والبنوك:
 *    - المحول منه: صندوق أو بنك فقط.
 *    - المحول إليه: صندوق أو بنك آخر فقط، مع استبعاد الحساب نفسه.
 * 7. تسوية العهد المالية للموظفين (Employee Petty Cash):
 *    - [ملاحظة تحليلية]: مؤجلة لما بعد الـ MVP نظراً لأن المنشأة الحالية متجر صغير
 *      يدار بواسطة المالك/الكاشير المباشر دون كادر وظيفي معقد. عند تفعيلها لاحقاً
 *      تكون حركة العهد حصراً بين حساب العهدة الوسيط (115) والصناديق/البنوك.
 * 8. مرفقات الصور وتعدد البنود:
 *    - إرفاق صور الفواتير بالكاميرا مؤجل للإصدارات اللاحقة.
 *    - اعتماد السند البسيط (صرف/قبض من صندوق لحساب واحد فقط) لتسهيل وتسريع الإدخال.
 * =========================================================================================
 */

import { AccountNode } from '@/types/pos';

/**
 * فحص ما إذا كان كود الحساب يتبع الصناديق النقدية (111...)
 */
export const isCashboxCode = (code?: string): boolean => {
  if (!code) return false;
  return code.trim().startsWith('111');
};

/**
 * فحص ما إذا كان كود الحساب يتبع البنوك أو المحافظ الإلكترونية (112...)
 */
export const isBankCode = (code?: string): boolean => {
  if (!code) return false;
  return code.trim().startsWith('112');
};

/**
 * فحص ما إذا كان الحساب ينتمي إلى الخزينة (صندوق نقدي أو بنك/محفظة)
 */
export const isTreasuryCode = (code?: string): boolean => {
  return isCashboxCode(code) || isBankCode(code);
};

/**
 * تحديد نوع حساب الخزينة
 */
export const getTreasuryType = (code?: string): 'CASHBOX' | 'BANK' | 'OTHER' => {
  if (isCashboxCode(code)) return 'CASHBOX';
  if (isBankCode(code)) return 'BANK';
  return 'OTHER';
};

/**
 * تفكيك شجرة الحسابات واستخراج كافة الحسابات الفرعية التحليلية (Leaf Accounts)
 */
export const flattenAccountTree = (nodes: AccountNode[]): AccountNode[] => {
  const result: AccountNode[] = [];
  const recurse = (list: AccountNode[]) => {
    for (const node of list) {
      if (!node.children || node.children.length === 0) {
        result.push(node);
      } else {
        recurse(node.children);
      }
    }
  };
  recurse(nodes);
  return result;
};

/**
 * استخراج حسابات الخزينة فقط (الصناديق 111 والبنوك 112) المؤهلة للعمليات النقدية
 */
export const getTreasuryAccounts = (
  treeOrFlat: AccountNode[],
  filterType: 'ALL' | 'CASHBOX_ONLY' | 'BANK_ONLY' = 'ALL'
): AccountNode[] => {
  // إذا كانت الشجرة متداخلة يتم تفكيكها أولاً
  const flat = treeOrFlat.some((n) => n.children && n.children.length > 0)
    ? flattenAccountTree(treeOrFlat)
    : treeOrFlat;

  return flat.filter((acc) => {
    if (acc.status === 'INACTIVE') return false;
    if (filterType === 'CASHBOX_ONLY') return isCashboxCode(acc.code);
    if (filterType === 'BANK_ONLY') return isBankCode(acc.code);
    return isTreasuryCode(acc.code);
  });
};

/**
 * استخراج الحسابات المحاسبية الأخرى المستبعد منها حسابات الصناديق والبنوك
 * تستخدم للطرف المقابل في سندات الصرف والقبض وفواتير المبيعات/المشتريات لمنع الخطأ
 */
export const getNonTreasuryAccounts = (treeOrFlat: AccountNode[]): AccountNode[] => {
  const flat = treeOrFlat.some((n) => n.children && n.children.length > 0)
    ? flattenAccountTree(treeOrFlat)
    : treeOrFlat;

  return flat.filter((acc) => {
    if (acc.status === 'INACTIVE') return false;
    return !isTreasuryCode(acc.code);
  });
};

/**
 * التحقق الأمني المحاسبي قبل إدراج أو ترحيل العملية المالية
 */
export interface TreasuryValidationResult {
  isValid: boolean;
  message?: string;
  treasuryType?: 'CASHBOX' | 'BANK' | 'OTHER';
}

export const validateTreasuryAccount = (
  accountCode?: string,
  allowedType: 'ANY' | 'CASHBOX' | 'BANK' = 'ANY'
): TreasuryValidationResult => {
  if (!accountCode) {
    return {
      isValid: false,
      message: 'يرجى اختيار الصندوق أو الحساب البنكي المرتبط بالعملية.',
    };
  }

  const type = getTreasuryType(accountCode);

  if (type === 'OTHER') {
    return {
      isValid: false,
      message: `الحساب رقم (${accountCode}) غير مصنف كصندوق أو حساب بنكي. يرجى اختيار حساب يبدأ بـ 111 (صندوق) أو 112 (بنك).`,
      treasuryType: 'OTHER',
    };
  }

  if (allowedType === 'CASHBOX' && type !== 'CASHBOX') {
    return {
      isValid: false,
      message: 'العملية تتطلب اختيار صندوق نقدي (يبدأ بـ 111).',
      treasuryType: type,
    };
  }

  if (allowedType === 'BANK' && type !== 'BANK') {
    return {
      isValid: false,
      message: 'العملية تتطلب اختيار حساب بنكي أو محفظة إلكترونية (يبدأ بـ 112).',
      treasuryType: type,
    };
  }

  return {
    isValid: true,
    treasuryType: type,
  };
};

/**
 * احتساب وتوزيع القيود المحاسبية للمبيعات النقدية والآجلة
 */
export interface SalesAccountingResult {
  isValid: boolean;
  errorMessage?: string;
  paidAmount: number;
  remainingCreditAmount: number;
  treasuryAccountCode: string;
  customerAccountCode?: string;
  entrySummary: string;
}

export const calculateSalesInvoiceAccounting = (params: {
  grandTotal: number;
  paidAmount: number;
  isCash: boolean;
  selectedTreasuryCode: string;
  customerName?: string;
  customerAccountCode?: string;
}): SalesAccountingResult => {
  const { grandTotal, paidAmount, isCash, selectedTreasuryCode, customerName, customerAccountCode } = params;

  // 1. التحقق من كود الصندوق أو البنك
  const treasuryCheck = validateTreasuryAccount(selectedTreasuryCode);
  if (!treasuryCheck.isValid) {
    return {
      isValid: false,
      errorMessage: treasuryCheck.message,
      paidAmount: 0,
      remainingCreditAmount: 0,
      treasuryAccountCode: selectedTreasuryCode,
      entrySummary: '',
    };
  }

  // 2. حالة البيع النقدي (Cash)
  if (isCash) {
    if (paidAmount < grandTotal) {
      return {
        isValid: false,
        errorMessage: `في حالة البيع النقدي يجب سداد المبلغ بالكامل. الإجمالي: ${grandTotal.toLocaleString()} - المدفوع: ${paidAmount.toLocaleString()}`,
        paidAmount,
        remainingCreditAmount: Math.max(0, grandTotal - paidAmount),
        treasuryAccountCode: selectedTreasuryCode,
        entrySummary: '',
      };
    }

    return {
      isValid: true,
      paidAmount: grandTotal,
      remainingCreditAmount: 0,
      treasuryAccountCode: selectedTreasuryCode,
      entrySummary: `سداد نقدي كامل: من حـ/ [${selectedTreasuryCode}] إلى حـ/ المبيعات بمبلغ ${grandTotal.toLocaleString()}`,
    };
  }

  // 3. حالة البيع الآجل (Credit)
  if (!customerName || !customerName.trim()) {
    return {
      isValid: false,
      errorMessage: 'في الفواتير الآجلة، يجب تحديد العميل لترحيل المتبقي إلى ذمته المالية.',
      paidAmount,
      remainingCreditAmount: Math.max(0, grandTotal - paidAmount),
      treasuryAccountCode: selectedTreasuryCode,
      entrySummary: '',
    };
  }

  const sanitizedPaid = Math.min(Math.max(0, paidAmount), grandTotal);
  const remaining = grandTotal - sanitizedPaid;

  const targetCustomerCode = customerAccountCode || '1131'; // الافتراضي: ذمم العملاء

  let summary = '';
  if (sanitizedPaid > 0 && remaining > 0) {
    summary = `فاتورة آجلة بدفعة مقدمة: سداد ${sanitizedPaid.toLocaleString()} في حـ/ [${selectedTreasuryCode}]، وترحيل المتبقي ${remaining.toLocaleString()} لذمة العميل [${customerName} (${targetCustomerCode})]`;
  } else if (remaining === grandTotal) {
    summary = `فاتورة آجلة كاملة: ترحيل كامل المبلغ ${grandTotal.toLocaleString()} لذمة العميل [${customerName} (${targetCustomerCode})]`;
  } else {
    summary = `سداد كامل بالصندوق/البنك [${selectedTreasuryCode}] بمبلغ ${grandTotal.toLocaleString()}`;
  }

  return {
    isValid: true,
    paidAmount: sanitizedPaid,
    remainingCreditAmount: remaining,
    treasuryAccountCode: selectedTreasuryCode,
    customerAccountCode: targetCustomerCode,
    entrySummary: summary,
  };
};
