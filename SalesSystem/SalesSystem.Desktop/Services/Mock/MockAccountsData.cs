using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لدليل الحسابات — مسطّحة من الشجرة اليمنية الكاملة.
/// </summary>
internal static class MockAccountsData
{
    public static List<AccountDto> Accounts { get; } = BuildAccounts();

    private static List<AccountDto> BuildAccounts()
    {
        // Nature: 1=Asset, 2=Liability, 3=Equity, 4=Revenue, 5=Expense
        // FinancialStatement: 1=BalanceSheet, 2=IncomeStatement
        // Ids are sequential; ParentId links by tree position.

        return new List<AccountDto>
        {
            // ═══════════════════════════════════════════════
            // 1 — الأصول (Assets)
            // ═══════════════════════════════════════════════
            new(Id: 1, AccountCode: "1", NameAr: "الأصول (Assets)", NameEn: "Assets",
                Nature: 1, IsLeaf: false, ParentId: null, ParentAccountName: null,
                IsSystem: true, IsActive: true, CategoryId: null, Level: 1,
                Description: null, ColorCode: "#2196F3", Notes: null),

            // ── 11 — الأصول المتداولة ──
            new(Id: 2, AccountCode: "11", NameAr: "الأصول المتداولة (Current Assets)", NameEn: "Current Assets",
                Nature: 1, IsLeaf: false, ParentId: 1, ParentAccountName: "الأصول (Assets)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 2,
                Description: null, ColorCode: "#2196F3", Notes: null),

            // ── 111 — الصناديق والخزائن النقدية ──
            new(Id: 3, AccountCode: "111", NameAr: "الصناديق والخزائن النقدية", NameEn: "Cash on Hand & Drawers",
                Nature: 1, IsLeaf: false, ParentId: 2, ParentAccountName: "الأصول المتداولة (Current Assets)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#2196F3", Notes: null),

            new(Id: 4, AccountCode: "1111", NameAr: "الصندوق الرئيسي (الخزينة المركزية)", NameEn: "Main Vault",
                Nature: 1, IsLeaf: true, ParentId: 3, ParentAccountName: "الصناديق والخزائن النقدية",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 4,
                Description: null, ColorCode: "#2196F3", Notes: null),

            new(Id: 5, AccountCode: "1112", NameAr: "صندوق نقطة بيع POS كاشير 1", NameEn: "Cashier 1 Drawer",
                Nature: 1, IsLeaf: true, ParentId: 3, ParentAccountName: "الصناديق والخزائن النقدية",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 4,
                Description: null, ColorCode: "#2196F3", Notes: null),

            new(Id: 6, AccountCode: "1113", NameAr: "صندوق نقطة بيع POS كاشير 2", NameEn: "Cashier 2 Drawer",
                Nature: 1, IsLeaf: true, ParentId: 3, ParentAccountName: "الصناديق والخزائن النقدية",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 4,
                Description: null, ColorCode: "#2196F3", Notes: null),

            // ── 112 — البنوك والمحافظ الإلكترونية ──
            new(Id: 7, AccountCode: "112", NameAr: "البنوك والمحافظ الإلكترونية", NameEn: "Banks & E-Wallets",
                Nature: 1, IsLeaf: false, ParentId: 2, ParentAccountName: "الأصول المتداولة (Current Assets)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#2196F3", Notes: null),

            new(Id: 8, AccountCode: "1121", NameAr: "حساب بنك الكريمي للتمويل الأصغر (YER)", NameEn: "Kuraimi Bank YER",
                Nature: 1, IsLeaf: true, ParentId: 7, ParentAccountName: "البنوك والمحافظ الإلكترونية",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 4,
                Description: null, ColorCode: "#2196F3", Notes: null),

            new(Id: 9, AccountCode: "1122", NameAr: "محفظة جيب الإلكترونية (JEB Wallet)", NameEn: "Jeb E-Wallet",
                Nature: 1, IsLeaf: true, ParentId: 7, ParentAccountName: "البنوك والمحافظ الإلكترونية",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 4,
                Description: null, ColorCode: "#2196F3", Notes: null),

            new(Id: 10, AccountCode: "1123", NameAr: "محفظة كاش وبنك اليمن والكويت", NameEn: "YKB Bank & Cash Wallet",
                Nature: 1, IsLeaf: true, ParentId: 7, ParentAccountName: "البنوك والمحافظ الإلكترونية",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 4,
                Description: null, ColorCode: "#2196F3", Notes: null),

            // ── 113 — حسابات العملاء ──
            new(Id: 11, AccountCode: "113", NameAr: "حسابات العملاء والمدينون", NameEn: "Accounts Receivable",
                Nature: 1, IsLeaf: true, ParentId: 2, ParentAccountName: "الأصول المتداولة (Current Assets)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#2196F3", Notes: null),

            // ── 114 — المخزون ──
            new(Id: 12, AccountCode: "114", NameAr: "بضاعة آخر المدة (مخزون المستودعات)", NameEn: "Inventory / Stock",
                Nature: 1, IsLeaf: true, ParentId: 2, ParentAccountName: "الأصول المتداولة (Current Assets)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#2196F3", Notes: null),

            // ── 1104 — حساب المخزون (بضاعة) لحسابات المستودعات ──
            new(Id: 41, AccountCode: "1104", NameAr: "المخزون (بضاعة)", NameEn: "Inventory / Goods",
                Nature: 1, IsLeaf: true, ParentId: 2, ParentAccountName: "الأصول المتداولة (Current Assets)",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#2196F3", Notes: null),

            // ── 12 — الأصول الثابتة ──
            new(Id: 13, AccountCode: "12", NameAr: "الأصول الثابتة (Fixed Assets)", NameEn: "Fixed Assets",
                Nature: 1, IsLeaf: false, ParentId: 1, ParentAccountName: "الأصول (Assets)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 2,
                Description: null, ColorCode: "#2196F3", Notes: null),

            new(Id: 14, AccountCode: "121", NameAr: "أجهزة الكمبيوتر ونقاط البيع POS", NameEn: "Computers & POS Hardware",
                Nature: 1, IsLeaf: true, ParentId: 13, ParentAccountName: "الأصول الثابتة (Fixed Assets)",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#2196F3", Notes: null),

            new(Id: 15, AccountCode: "122", NameAr: "أرفف وثلاجات العرض والمعدات", NameEn: "Shelves & Refrigerators",
                Nature: 1, IsLeaf: true, ParentId: 13, ParentAccountName: "الأصول الثابتة (Fixed Assets)",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#2196F3", Notes: null),

            // ═══════════════════════════════════════════════
            // 2 — الخصوم وحقوق الملكية (Liabilities & Equity)
            // ═══════════════════════════════════════════════
            new(Id: 16, AccountCode: "2", NameAr: "الخصوم وحقوق الملكية (Liabilities & Equity)", NameEn: "Liabilities & Equity",
                Nature: 2, IsLeaf: false, ParentId: null, ParentAccountName: null,
                IsSystem: true, IsActive: true, CategoryId: null, Level: 1,
                Description: null, ColorCode: "#E91E63", Notes: null),

            // ── 21 — الخصوم المتداولة ──
            new(Id: 17, AccountCode: "21", NameAr: "الخصوم المتداولة (Current Liabilities)", NameEn: "Current Liabilities",
                Nature: 2, IsLeaf: false, ParentId: 16, ParentAccountName: "الخصوم وحقوق الملكية (Liabilities & Equity)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 2,
                Description: null, ColorCode: "#E91E63", Notes: null),

            new(Id: 18, AccountCode: "211", NameAr: "حسابات الموردين والدائنون", NameEn: "Accounts Payable",
                Nature: 2, IsLeaf: true, ParentId: 17, ParentAccountName: "الخصوم المتداولة (Current Liabilities)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#E91E63", Notes: null),

            new(Id: 19, AccountCode: "212", NameAr: "مصروفات مستحقة الدفع وأمانات", NameEn: "Accrued Expenses",
                Nature: 2, IsLeaf: true, ParentId: 17, ParentAccountName: "الخصوم المتداولة (Current Liabilities)",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#E91E63", Notes: null),

            new(Id: 20, AccountCode: "213", NameAr: "ضرائب مستحقة (ضريبة القيمة المضافة)", NameEn: "VAT Payable",
                Nature: 2, IsLeaf: true, ParentId: 17, ParentAccountName: "الخصوم المتداولة (Current Liabilities)",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#E91E63", Notes: null),

            // ── 22 — حقوق الملكية ──
            new(Id: 21, AccountCode: "22", NameAr: "حقوق الملكية ورأس المال (Equity)", NameEn: "Equity & Capital",
                Nature: 3, IsLeaf: false, ParentId: 16, ParentAccountName: "الخصوم وحقوق الملكية (Liabilities & Equity)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 2,
                Description: null, ColorCode: "#9C27B0", Notes: null),

            new(Id: 22, AccountCode: "221", NameAr: "رأس المال المدفوع", NameEn: "Paid Capital",
                Nature: 3, IsLeaf: true, ParentId: 21, ParentAccountName: "حقوق الملكية ورأس المال (Equity)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#9C27B0", Notes: null),

            new(Id: 23, AccountCode: "222", NameAr: "الأرباح والخسائر المدورة (المرحلة)", NameEn: "Retained Earnings",
                Nature: 3, IsLeaf: true, ParentId: 21, ParentAccountName: "حقوق الملكية ورأس المال (Equity)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#9C27B0", Notes: null),

            new(Id: 24, AccountCode: "223", NameAr: "جاري المالك / المسحوبات الشخصية", NameEn: "Owner Drawings",
                Nature: 3, IsLeaf: true, ParentId: 21, ParentAccountName: "حقوق الملكية ورأس المال (Equity)",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#9C27B0", Notes: null),

            // ═══════════════════════════════════════════════
            // 3 — الإيرادات (Revenues)
            // ═══════════════════════════════════════════════
            new(Id: 25, AccountCode: "3", NameAr: "الإيرادات (Revenues)", NameEn: "Revenues",
                Nature: 4, IsLeaf: false, ParentId: null, ParentAccountName: null,
                IsSystem: true, IsActive: true, CategoryId: null, Level: 1,
                Description: null, ColorCode: "#4CAF50", Notes: null),

            new(Id: 26, AccountCode: "31", NameAr: "إيراد مبيعات البضاعة (Sales Revenue)", NameEn: "Merchandise Sales Revenue",
                Nature: 4, IsLeaf: true, ParentId: 25, ParentAccountName: "الإيرادات (Revenues)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 2,
                Description: null, ColorCode: "#4CAF50", Notes: null),

            new(Id: 27, AccountCode: "32", NameAr: "إيرادات خدمات وتوصيل ونقل", NameEn: "Delivery & Service Revenues",
                Nature: 4, IsLeaf: true, ParentId: 25, ParentAccountName: "الإيرادات (Revenues)",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 2,
                Description: null, ColorCode: "#4CAF50", Notes: null),

            new(Id: 28, AccountCode: "33", NameAr: "أرباح وفائض الجرد المخزني", NameEn: "Stock Surplus Revenue",
                Nature: 4, IsLeaf: true, ParentId: 25, ParentAccountName: "الإيرادات (Revenues)",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 2,
                Description: null, ColorCode: "#4CAF50", Notes: null),

            new(Id: 29, AccountCode: "34", NameAr: "الخصم المكتسب من الموردين (خصم المشتريات)", NameEn: "Discounts Received from Suppliers",
                Nature: 4, IsLeaf: true, ParentId: 25, ParentAccountName: "الإيرادات (Revenues)",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 2,
                Description: null, ColorCode: "#4CAF50", Notes: null),

            // ═══════════════════════════════════════════════
            // 4 — المصروفات (Expenses)
            // ═══════════════════════════════════════════════
            new(Id: 30, AccountCode: "4", NameAr: "المصروفات (Expenses)", NameEn: "Expenses",
                Nature: 5, IsLeaf: false, ParentId: null, ParentAccountName: null,
                IsSystem: true, IsActive: true, CategoryId: null, Level: 1,
                Description: null, ColorCode: "#FF5722", Notes: null),

            new(Id: 31, AccountCode: "41", NameAr: "تكلفة البضاعة المباعة (COGS)", NameEn: "Cost of Goods Sold",
                Nature: 5, IsLeaf: true, ParentId: 30, ParentAccountName: "المصروفات (Expenses)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 2,
                Description: null, ColorCode: "#FF5722", Notes: null),

            // ── 42 — المصروفات العمومية ──
            new(Id: 32, AccountCode: "42", NameAr: "المصروفات العمومية والتشغيلية", NameEn: "General & Operating Expenses",
                Nature: 5, IsLeaf: false, ParentId: 30, ParentAccountName: "المصروفات (Expenses)",
                IsSystem: true, IsActive: true, CategoryId: null, Level: 2,
                Description: null, ColorCode: "#FF5722", Notes: null),

            new(Id: 33, AccountCode: "421", NameAr: "مصروفات الإيجار والكهرباء والمياه", NameEn: "Rent, Power & Utilities",
                Nature: 5, IsLeaf: true, ParentId: 32, ParentAccountName: "المصروفات العمومية والتشغيلية",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#FF5722", Notes: null),

            new(Id: 34, AccountCode: "422", NameAr: "رواتب وأجور الموظفين والعمال", NameEn: "Salaries & Wages",
                Nature: 5, IsLeaf: true, ParentId: 32, ParentAccountName: "المصروفات العمومية والتشغيلية",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#FF5722", Notes: null),

            new(Id: 35, AccountCode: "423", NameAr: "مصروفات الصيانة وضيافة المحل", NameEn: "Maintenance & Hospitality",
                Nature: 5, IsLeaf: true, ParentId: 32, ParentAccountName: "المصروفات العمومية والتشغيلية",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#FF5722", Notes: null),

            new(Id: 36, AccountCode: "424", NameAr: "مصروفات أجور النقل والمواصلات", NameEn: "Transportation & Freight Expenses",
                Nature: 5, IsLeaf: true, ParentId: 32, ParentAccountName: "المصروفات العمومية والتشغيلية",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#FF5722", Notes: null),

            new(Id: 37, AccountCode: "425", NameAr: "مصروفات الشحن وخدمات التوصيل", NameEn: "Shipping & Delivery Services",
                Nature: 5, IsLeaf: true, ParentId: 32, ParentAccountName: "المصروفات العمومية والتشغيلية",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#FF5722", Notes: null),

            new(Id: 38, AccountCode: "426", NameAr: "مصروفات العتالة والتحميل والتفريغ", NameEn: "Handling & Loading Charges",
                Nature: 5, IsLeaf: true, ParentId: 32, ParentAccountName: "المصروفات العمومية والتشغيلية",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#FF5722", Notes: null),

            new(Id: 39, AccountCode: "429", NameAr: "مصروفات نثرية وتشغيلية أخرى", NameEn: "Other Operating & Sundry Expenses",
                Nature: 5, IsLeaf: true, ParentId: 32, ParentAccountName: "المصروفات العمومية والتشغيلية",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 3,
                Description: null, ColorCode: "#FF5722", Notes: null),

            new(Id: 40, AccountCode: "43", NameAr: "عجز وتلفيات الجرد المخزني", NameEn: "Stock Deficit & Shrinkage Expense",
                Nature: 5, IsLeaf: true, ParentId: 30, ParentAccountName: "المصروفات (Expenses)",
                IsSystem: false, IsActive: true, CategoryId: null, Level: 2,
                Description: null, ColorCode: "#FF5722", Notes: null),
        };
    }
}
