import { AccountNode, CostCenterNode } from '@/types/pos';

// Helper to generate the next child code under any parent account in an infinite-depth chart
export const generateNextChildAccountCode = (parentCode: string, existingTree: AccountNode[]): { code: string; level: number; treePath: string } => {
  // Find the parent node in the tree recursively
  const findNode = (nodes: AccountNode[], code: string): AccountNode | null => {
    for (const node of nodes) {
      if (node.code === code) return node;
      if (node.children && node.children.length > 0) {
        const found = findNode(node.children, code);
        if (found) return found;
      }
    }
    return null;
  };

  const parentNode = findNode(existingTree, parentCode);
  const parentLevel = parentNode?.level ?? (parentCode ? parentCode.length : 1);
  const parentPath = parentNode?.treePath ?? `/${parentCode}/`;
  const nextLevel = parentLevel + 1;

  const children = parentNode?.children || [];
  
  // Find highest numeric suffix among direct children
  let maxSuffix = 0;
  children.forEach((child) => {
    if (child.code.startsWith(parentCode)) {
      const suffixStr = child.code.substring(parentCode.length);
      const numericSuffix = parseInt(suffixStr, 10);
      if (!isNaN(numericSuffix) && numericSuffix > maxSuffix) {
        maxSuffix = numericSuffix;
      }
    }
  });

  const nextSuffix = maxSuffix + 1;
  // Format suffix based on level (2 digits for lower sub-levels, 4 digits for level >= 4)
  const paddingLength = nextLevel >= 4 ? 4 : 2;
  const suffixFormatted = String(nextSuffix).padStart(paddingLength, '0');
  const newCode = `${parentCode}${suffixFormatted}`;
  const newTreePath = `${parentPath}${newCode}/`;

  return {
    code: newCode,
    level: nextLevel,
    treePath: newTreePath,
  };
};

// Helper to flatten the entire recursive account tree
export const getAllAccountsFlat = (nodes: AccountNode[]): AccountNode[] => {
  let list: AccountNode[] = [];
  nodes.forEach((acc) => {
    list.push(acc);
    if (acc.children && acc.children.length > 0) {
      list = list.concat(getAllAccountsFlat(acc.children));
    }
  });
  return list;
};

// Recursive helper to insert a child account into its parent's node in the tree
export const insertAccountIntoTree = (nodes: AccountNode[], newAccount: AccountNode): AccountNode[] => {
  const parentCode = newAccount.parentCode;
  if (!parentCode) {
    return [...nodes, newAccount];
  }
  return nodes.map((node) => {
    if (node.code === parentCode) {
      const currentChildren = node.children || [];
      return {
        ...node,
        isLeaf: false,
        isSubAccount: false,
        children: [...currentChildren, newAccount],
      };
    }
    if (node.children && node.children.length > 0) {
      return {
        ...node,
        children: insertAccountIntoTree(node.children, newAccount),
      };
    }
    return node;
  });
};

// Recursive helper to calculate bottom-up balances across all infinite levels
export const calculateTreeBalances = (nodes: AccountNode[]): AccountNode[] => {
  return nodes.map((node) => {
    if (node.children && node.children.length > 0) {
      const updatedChildren = calculateTreeBalances(node.children);
      const sumBalance = updatedChildren.reduce((sum, child) => sum + (child.balance || 0), 0);
      return {
        ...node,
        isLeaf: false,
        isSubAccount: false,
        balance: sumBalance,
        children: updatedChildren,
      };
    }
    return {
      ...node,
      isLeaf: node.isLeaf ?? true,
      isSubAccount: node.isSubAccount ?? (node.isLeaf ?? true),
    };
  });
};

export const BASE_RAW_CHART_OF_ACCOUNTS: AccountNode[] = [
  {
    code: '1',
    name: 'الأصول (Assets)',
    nameEn: 'Assets',
    type: 'ASSET',
    financialStatement: 'BALANCE_SHEET',
    balance: 4250000,
    nature: 'DEBIT',
    level: 1,
    treePath: '/1/',
    isLeaf: false,
    isSubAccount: false,
    status: 'ACTIVE',
    currency: 'YER',
    allowCostCenter: false,
    children: [
      {
        code: '11',
        name: 'الأصول المتداولة (Current Assets)',
        nameEn: 'Current Assets',
        type: 'ASSET',
        financialStatement: 'BALANCE_SHEET',
        balance: 3450000,
        nature: 'DEBIT',
        level: 2,
        treePath: '/1/11/',
        isLeaf: false,
        isSubAccount: false,
        parentCode: '1',
        status: 'ACTIVE',
        currency: 'YER',
        allowCostCenter: false,
        children: [
          {
            code: '111',
            name: 'الصناديق والخزائن النقدية',
            nameEn: 'Cash on Hand & Drawers',
            type: 'ASSET',
            financialStatement: 'BALANCE_SHEET',
            balance: 850000,
            nature: 'DEBIT',
            level: 3,
            treePath: '/1/11/111/',
            isLeaf: false,
            isSubAccount: false,
            parentCode: '11',
            status: 'ACTIVE',
            currency: 'YER',
            allowCostCenter: false,
            children: [
              { code: '1111', name: 'الصندوق الرئيسي (الخزينة المركزية)', nameEn: 'Main Vault', type: 'ASSET', financialStatement: 'BALANCE_SHEET', balance: 650000, nature: 'DEBIT', level: 4, treePath: '/1/11/111/1111/', isLeaf: true, isSubAccount: true, parentCode: '111', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
              { code: '1112', name: 'صندوق نقطة بيع POS كاشير 1', nameEn: 'Cashier 1 Drawer', type: 'ASSET', financialStatement: 'BALANCE_SHEET', balance: 120000, nature: 'DEBIT', level: 4, treePath: '/1/11/111/1112/', isLeaf: true, isSubAccount: true, parentCode: '111', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
              { code: '1113', name: 'صندوق نقطة بيع POS كاشير 2', nameEn: 'Cashier 2 Drawer', type: 'ASSET', financialStatement: 'BALANCE_SHEET', balance: 80000, nature: 'DEBIT', level: 4, treePath: '/1/11/111/1113/', isLeaf: true, isSubAccount: true, parentCode: '111', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
            ],
          },
          {
            code: '112',
            name: 'البنوك والمحافظ الإلكترونية',
            nameEn: 'Banks & E-Wallets',
            type: 'ASSET',
            financialStatement: 'BALANCE_SHEET',
            balance: 1250000,
            nature: 'DEBIT',
            level: 3,
            treePath: '/1/11/112/',
            isLeaf: false,
            isSubAccount: false,
            parentCode: '11',
            status: 'ACTIVE',
            currency: 'YER',
            allowCostCenter: false,
            children: [
              { code: '1121', name: 'حساب بنك الكريمي للتمويل الأصغر (YER)', nameEn: 'Kuraimi Bank YER', type: 'ASSET', financialStatement: 'BALANCE_SHEET', balance: 950000, nature: 'DEBIT', level: 4, treePath: '/1/11/112/1121/', isLeaf: true, isSubAccount: true, parentCode: '112', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
              { code: '1122', name: 'محفظة جيب الإلكترونية (JEB Wallet)', nameEn: 'Jeb E-Wallet', type: 'ASSET', financialStatement: 'BALANCE_SHEET', balance: 180000, nature: 'DEBIT', level: 4, treePath: '/1/11/112/1122/', isLeaf: true, isSubAccount: true, parentCode: '112', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
              { code: '1123', name: 'محفظة كاش وبنك اليمن والكويت', nameEn: 'YKB Bank & Cash Wallet', type: 'ASSET', financialStatement: 'BALANCE_SHEET', balance: 120000, nature: 'DEBIT', level: 4, treePath: '/1/11/112/1123/', isLeaf: true, isSubAccount: true, parentCode: '112', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
            ],
          },
          { code: '113', name: 'حسابات العملاء والمدينون', nameEn: 'Accounts Receivable', type: 'ASSET', financialStatement: 'BALANCE_SHEET', balance: 890000, nature: 'DEBIT', level: 3, treePath: '/1/11/113/', isLeaf: true, isSubAccount: true, parentCode: '11', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
          { code: '114', name: 'بضاعة آخر المدة (مخزون المستودعات)', nameEn: 'Inventory / Stock', type: 'ASSET', financialStatement: 'BALANCE_SHEET', balance: 460000, nature: 'DEBIT', level: 3, treePath: '/1/11/114/', isLeaf: true, isSubAccount: true, parentCode: '11', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
        ],
      },
      {
        code: '12',
        name: 'الأصول الثابتة (Fixed Assets)',
        nameEn: 'Fixed Assets',
        type: 'ASSET',
        financialStatement: 'BALANCE_SHEET',
        balance: 800000,
        nature: 'DEBIT',
        level: 2,
        treePath: '/1/12/',
        isLeaf: false,
        isSubAccount: false,
        parentCode: '1',
        status: 'ACTIVE',
        currency: 'YER',
        allowCostCenter: false,
        children: [
          { code: '121', name: 'أجهزة الكمبيوتر ونقاط البيع POS', nameEn: 'Computers & POS Hardware', type: 'ASSET', financialStatement: 'BALANCE_SHEET', balance: 350000, nature: 'DEBIT', level: 3, treePath: '/1/12/121/', isLeaf: true, isSubAccount: true, parentCode: '12', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
          { code: '122', name: 'أرفف وثلاجات العرض والمعدات', nameEn: 'Shelves & Refrigerators', type: 'ASSET', financialStatement: 'BALANCE_SHEET', balance: 450000, nature: 'DEBIT', level: 3, treePath: '/1/12/122/', isLeaf: true, isSubAccount: true, parentCode: '12', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
        ],
      },
    ],
  },
  {
    code: '2',
    name: 'الخصوم وحقوق الملكية (Liabilities & Equity)',
    nameEn: 'Liabilities & Equity',
    type: 'LIABILITY',
    financialStatement: 'BALANCE_SHEET',
    balance: 3968000,
    nature: 'CREDIT',
    level: 1,
    treePath: '/2/',
    isLeaf: false,
    isSubAccount: false,
    status: 'ACTIVE',
    currency: 'YER',
    allowCostCenter: false,
    children: [
      {
        code: '21',
        name: 'الخصوم المتداولة (Current Liabilities)',
        nameEn: 'Current Liabilities',
        type: 'LIABILITY',
        financialStatement: 'BALANCE_SHEET',
        balance: 968000,
        nature: 'CREDIT',
        level: 2,
        treePath: '/2/21/',
        isLeaf: false,
        isSubAccount: false,
        parentCode: '2',
        status: 'ACTIVE',
        currency: 'YER',
        allowCostCenter: false,
        children: [
          { code: '211', name: 'حسابات الموردين والدائنون', nameEn: 'Accounts Payable', type: 'LIABILITY', financialStatement: 'BALANCE_SHEET', balance: 968000, nature: 'CREDIT', level: 3, treePath: '/2/21/211/', isLeaf: true, isSubAccount: true, parentCode: '21', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
          { code: '212', name: 'مصروفات مستحقة الدفع وأمانات', nameEn: 'Accrued Expenses', type: 'LIABILITY', financialStatement: 'BALANCE_SHEET', balance: 0, nature: 'CREDIT', level: 3, treePath: '/2/21/212/', isLeaf: true, isSubAccount: true, parentCode: '21', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
          { code: '213', name: 'ضرائب مستحقة (ضريبة القيمة المضافة)', nameEn: 'VAT Payable', type: 'LIABILITY', financialStatement: 'BALANCE_SHEET', balance: 0, nature: 'CREDIT', level: 3, treePath: '/2/21/213/', isLeaf: true, isSubAccount: true, parentCode: '21', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
        ],
      },
      {
        code: '22',
        name: 'حقوق الملكية ورأس المال (Equity)',
        nameEn: 'Equity & Capital',
        type: 'EQUITY',
        financialStatement: 'BALANCE_SHEET',
        balance: 3000000,
        nature: 'CREDIT',
        level: 2,
        treePath: '/2/22/',
        isLeaf: false,
        isSubAccount: false,
        parentCode: '2',
        status: 'ACTIVE',
        currency: 'YER',
        allowCostCenter: false,
        children: [
          { code: '221', name: 'رأس المال المدفوع', nameEn: 'Paid Capital', type: 'EQUITY', financialStatement: 'BALANCE_SHEET', balance: 2500000, nature: 'CREDIT', level: 3, treePath: '/2/22/221/', isLeaf: true, isSubAccount: true, parentCode: '22', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
          { code: '222', name: 'الأرباح والخسائر المدورة (المرحلة)', nameEn: 'Retained Earnings', type: 'EQUITY', financialStatement: 'BALANCE_SHEET', balance: 500000, nature: 'CREDIT', level: 3, treePath: '/2/22/222/', isLeaf: true, isSubAccount: true, parentCode: '22', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
          { code: '223', name: 'جاري المالك / المسحوبات الشخصية', nameEn: 'Owner Drawings', type: 'EQUITY', financialStatement: 'BALANCE_SHEET', balance: 0, nature: 'DEBIT', level: 3, treePath: '/2/22/223/', isLeaf: true, isSubAccount: true, parentCode: '22', status: 'ACTIVE', currency: 'YER', allowCostCenter: false },
        ],
      },
    ],
  },
  {
    code: '3',
    name: 'الإيرادات (Revenues)',
    nameEn: 'Revenues',
    type: 'REVENUE',
    financialStatement: 'INCOME_STATEMENT',
    balance: 1543000,
    nature: 'CREDIT',
    level: 1,
    treePath: '/3/',
    isLeaf: false,
    isSubAccount: false,
    status: 'ACTIVE',
    currency: 'YER',
    allowCostCenter: true,
    children: [
      { code: '31', name: 'إيراد مبيعات البضاعة (Sales Revenue)', nameEn: 'Merchandise Sales Revenue', type: 'REVENUE', financialStatement: 'INCOME_STATEMENT', balance: 1480000, nature: 'CREDIT', level: 2, treePath: '/3/31/', isLeaf: true, isSubAccount: true, parentCode: '3', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
      { code: '32', name: 'إيرادات خدمات وتوصيل ونقل', nameEn: 'Delivery & Service Revenues', type: 'REVENUE', financialStatement: 'INCOME_STATEMENT', balance: 60000, nature: 'CREDIT', level: 2, treePath: '/3/32/', isLeaf: true, isSubAccount: true, parentCode: '3', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
      { code: '33', name: 'أرباح وفائض الجرد المخزني', nameEn: 'Stock Surplus Revenue', type: 'REVENUE', financialStatement: 'INCOME_STATEMENT', balance: 3000, nature: 'CREDIT', level: 2, treePath: '/3/33/', isLeaf: true, isSubAccount: true, parentCode: '3', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
      { code: '34', name: 'الخصم المكتسب من الموردين (خصم المشتريات)', nameEn: 'Discounts Received from Suppliers', type: 'REVENUE', financialStatement: 'INCOME_STATEMENT', balance: 0, nature: 'CREDIT', level: 2, treePath: '/3/34/', isLeaf: true, isSubAccount: true, parentCode: '3', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
    ],
  },
  {
    code: '4',
    name: 'المصروفات (Expenses)',
    nameEn: 'Expenses',
    type: 'EXPENSE',
    financialStatement: 'INCOME_STATEMENT',
    balance: 1269800,
    nature: 'DEBIT',
    level: 1,
    treePath: '/4/',
    isLeaf: false,
    isSubAccount: false,
    status: 'ACTIVE',
    currency: 'YER',
    allowCostCenter: true,
    children: [
      { code: '41', name: 'تكلفة البضاعة المباعة (COGS)', nameEn: 'Cost of Goods Sold', type: 'EXPENSE', financialStatement: 'INCOME_STATEMENT', balance: 1140000, nature: 'DEBIT', level: 2, treePath: '/4/41/', isLeaf: true, isSubAccount: true, parentCode: '4', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
      {
        code: '42',
        name: 'المصروفات العمومية والتشغيلية',
        nameEn: 'General & Operating Expenses',
        type: 'EXPENSE',
        financialStatement: 'INCOME_STATEMENT',
        balance: 118000,
        nature: 'DEBIT',
        level: 2,
        treePath: '/4/42/',
        isLeaf: false,
        isSubAccount: false,
        parentCode: '4',
        status: 'ACTIVE',
        currency: 'YER',
        allowCostCenter: true,
        children: [
          { code: '421', name: 'مصروفات الإيجار والكهرباء والمياه', nameEn: 'Rent, Power & Utilities', type: 'EXPENSE', financialStatement: 'INCOME_STATEMENT', balance: 75000, nature: 'DEBIT', level: 3, treePath: '/4/42/421/', isLeaf: true, isSubAccount: true, parentCode: '42', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
          { code: '422', name: 'رواتب وأجور الموظفين والعمال', nameEn: 'Salaries & Wages', type: 'EXPENSE', financialStatement: 'INCOME_STATEMENT', balance: 35000, nature: 'DEBIT', level: 3, treePath: '/4/42/422/', isLeaf: true, isSubAccount: true, parentCode: '42', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
          { code: '423', name: 'مصروفات الصيانة وضيافة المحل', nameEn: 'Maintenance & Hospitality', type: 'EXPENSE', financialStatement: 'INCOME_STATEMENT', balance: 8000, nature: 'DEBIT', level: 3, treePath: '/4/42/423/', isLeaf: true, isSubAccount: true, parentCode: '42', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
          { code: '424', name: 'مصروفات أجور النقل والمواصلات', nameEn: 'Transportation & Freight Expenses', type: 'EXPENSE', financialStatement: 'INCOME_STATEMENT', balance: 12000, nature: 'DEBIT', level: 3, treePath: '/4/42/424/', isLeaf: true, isSubAccount: true, parentCode: '42', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
          { code: '425', name: 'مصروفات الشحن وخدمات التوصيل', nameEn: 'Shipping & Delivery Services', type: 'EXPENSE', financialStatement: 'INCOME_STATEMENT', balance: 9500, nature: 'DEBIT', level: 3, treePath: '/4/42/425/', isLeaf: true, isSubAccount: true, parentCode: '42', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
          { code: '426', name: 'مصروفات العتالة والتحميل والتفريغ', nameEn: 'Handling & Loading Charges', type: 'EXPENSE', financialStatement: 'INCOME_STATEMENT', balance: 4000, nature: 'DEBIT', level: 3, treePath: '/4/42/426/', isLeaf: true, isSubAccount: true, parentCode: '42', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
          { code: '429', name: 'مصروفات نثرية وتشغيلية أخرى', nameEn: 'Other Operating & Sundry Expenses', type: 'EXPENSE', financialStatement: 'INCOME_STATEMENT', balance: 2500, nature: 'DEBIT', level: 3, treePath: '/4/42/429/', isLeaf: true, isSubAccount: true, parentCode: '42', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
        ],
      },
      { code: '43', name: 'عجز وتلفيات الجرد المخزني', nameEn: 'Stock Deficit & Shrinkage Expense', type: 'EXPENSE', financialStatement: 'INCOME_STATEMENT', balance: 11800, nature: 'DEBIT', level: 2, treePath: '/4/43/', isLeaf: true, isSubAccount: true, parentCode: '4', status: 'ACTIVE', currency: 'YER', allowCostCenter: true },
    ],
  },
];

export const INITIAL_CHART_OF_ACCOUNTS: AccountNode[] = calculateTreeBalances(BASE_RAW_CHART_OF_ACCOUNTS);

export const INITIAL_COST_CENTERS: CostCenterNode[] = [
  {
    id: 'CC-1',
    code: '1',
    name: 'مراكز الإدارة العامة والخدمات',
    nameEn: 'General & Administrative Centers',
    level: 1,
    isLeaf: false,
    budget: 800000,
    status: 'ACTIVE',
    notes: 'مراكز التكلفة التابعة للإدارة العامة',
    children: [
      {
        id: 'CC-101',
        code: '101',
        name: 'إدارة الموارد البشرية والشؤون الإدارية',
        nameEn: 'HR & Administration',
        parentCode: '1',
        level: 2,
        isLeaf: true,
        budget: 350000,
        managerName: 'أ. خالد الوادعي',
        status: 'ACTIVE',
      },
      {
        id: 'CC-102',
        code: '102',
        name: 'إدارة تكنولوجيا المعلومات ونقاط البيع IT',
        nameEn: 'IT & POS Systems',
        parentCode: '1',
        level: 2,
        isLeaf: true,
        budget: 450000,
        managerName: 'م. أحمد الشامي',
        status: 'ACTIVE',
      },
    ],
  },
  {
    id: 'CC-2',
    code: '2',
    name: 'مراكز الفروع والمنافذ التشغيلية',
    nameEn: 'Operational Branches Centers',
    level: 1,
    isLeaf: false,
    budget: 2500000,
    status: 'ACTIVE',
    notes: 'مراكز الإيرادات والمصروفات المباشرة للفروع',
    children: [
      {
        id: 'CC-201',
        code: '201',
        name: 'فرع حدة الرئيسي (صنعاء)',
        nameEn: 'Hadda Main Branch',
        parentCode: '2',
        level: 2,
        isLeaf: true,
        budget: 1200000,
        managerName: 'فهد الحميري',
        status: 'ACTIVE',
      },
      {
        id: 'CC-202',
        code: '202',
        name: 'فرع التحرير التجاري (صنعاء)',
        nameEn: 'Al-Tahreer Branch',
        parentCode: '2',
        level: 2,
        isLeaf: true,
        budget: 800000,
        managerName: 'طارق الأصبحي',
        status: 'ACTIVE',
      },
      {
        id: 'CC-203',
        code: '203',
        name: 'فرع الستين والمستودع السريع',
        nameEn: 'Al-Sitteen Branch & Depot',
        parentCode: '2',
        level: 2,
        isLeaf: true,
        budget: 500000,
        managerName: 'سامي القدسي',
        status: 'ACTIVE',
      },
    ],
  },
  {
    id: 'CC-3',
    code: '3',
    name: 'مراكز النقل وسلسلة التوريد',
    nameEn: 'Logistics & Supply Chain',
    level: 1,
    isLeaf: false,
    budget: 900000,
    status: 'ACTIVE',
    notes: 'مركبات التوزيع ومستودعات التخزين المركزية',
    children: [
      {
        id: 'CC-301',
        code: '301',
        name: 'أسطول شاحنات التوزيع الخفيف',
        nameEn: 'Light Delivery Fleet',
        parentCode: '3',
        level: 2,
        isLeaf: true,
        budget: 550000,
        managerName: 'جمال الصبري',
        status: 'ACTIVE',
      },
      {
        id: 'CC-302',
        code: '302',
        name: 'المستودع المركزي للبضائع الجافة',
        nameEn: 'Central Dry Goods Warehouse',
        parentCode: '3',
        level: 2,
        isLeaf: true,
        budget: 350000,
        managerName: 'هشام اليافعي',
        status: 'ACTIVE',
      },
    ],
  },
];
