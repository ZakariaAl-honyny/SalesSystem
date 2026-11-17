using SalesSystem.Business.Helpers;
using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لدليل الحسابات — مسطّحة من شجرة الحسابات الكاملة بأكواد السلّم الرسمي
/// (مستوى 1 = خانة · مستوى 2 = خانتان · مستوى 3 = 4 خانات · مستوى 4 = 8 خانات — `docs/database-schema.md` §4.1.2.1).
/// السمات المشتقة (`Level` · `TreePath` · `FinancialStatement` · `NormalBalance` · `CategoryLabel`) تُحسب هنا
/// تماماً كما تحسبها طبقة الأعمال، وتصنيف الحسابات الخاصة يأتي من البيت الواحد <see cref="AccountCategoryHelper"/>.
/// </summary>
internal static class MockAccountsData
{
    public static List<AccountDto> Accounts { get; } = BuildAccounts();

    private static List<AccountDto> BuildAccounts()
    {
        // Nature: 1=Asset, 2=Liability, 3=Equity, 4=Revenue, 5=Expense
        // Balance: مجموع (مدين − دائن) بإشارة الطبيعة — لا يُخزَّن في دليل الحسابات.
        var rows = new List<AccountDto>();
        var byId = new Dictionary<int, AccountDto>();

        void Add(
            int id,
            string code,
            string nameAr,
            string? nameEn,
            byte nature,
            bool isLeaf,
            int? parentId,
            bool isSystem,
            decimal balance = 0m,
            byte? normalBalance = null,
            bool allowCostCenter = false)
        {
            AccountDto? parent = parentId.HasValue && byId.TryGetValue(parentId.Value, out var p) ? p : null;
            var level = (byte)(parent is null ? 1 : parent.Level + 1);
            var category = AccountCategoryHelper.Detect(code, nameAr);

            var dto = new AccountDto(
                Id: id,
                AccountCode: code,
                NameAr: nameAr,
                NameEn: nameEn,
                Nature: nature,
                IsLeaf: isLeaf,
                ParentId: parentId,
                ParentAccountName: parent?.NameAr,
                IsSystem: isSystem,
                IsActive: true,
                CategoryId: null,
                Level: level,
                Description: null,
                ColorCode: ColorFor(nature),
                Notes: null,
                ParentAccountCode: parent?.AccountCode,
                TreePath: (parent?.TreePath ?? "/") + code + "/",
                FinancialStatement: (byte)(nature <= 3 ? 1 : 2),
                NormalBalance: normalBalance ?? 0,   // 0 = غير مخزَّن ⇒ يُشتق من Nature عبر EffectiveNormalBalance
                AllowCostCenter: allowCostCenter,
                Balance: balance,
                CategoryLabel: category.Label);

            rows.Add(dto);
            byId[id] = dto;
        }

        // ═══════════════════════════════════════════════
        // 1 — الأصول (Assets)
        // ═══════════════════════════════════════════════
        Add(1, "1", "الأصول (Assets)", "Assets", 1, false, null, true);

        // ── 11 — الأصول المتداولة ──
        Add(2, "11", "الأصول المتداولة (Current Assets)", "Current Assets", 1, false, 1, true);

        // ── 1101 — الصناديق والخزائن النقدية ──
        Add(3, "1101", "الصناديق والخزائن النقدية", "Cash on Hand & Drawers", 1, false, 2, true);

        Add(4, "11010001", "الصندوق الرئيسي (الخزينة المركزية)", "Main Vault", 1, true, 3, true, balance: 125000.00m);

        Add(5, "11010002", "صندوق نقطة بيع POS كاشير 1", "Cashier 1 Drawer", 1, true, 3, false, balance: 3500.00m);

        Add(6, "11010003", "صندوق نقطة بيع POS كاشير 2", "Cashier 2 Drawer", 1, true, 3, false, balance: 2750.00m);

        // ── 1102 — البنوك والمحافظ الإلكترونية ──
        Add(7, "1102", "البنوك والمحافظ الإلكترونية", "Banks & E-Wallets", 1, false, 2, true);

        Add(8, "11020001", "حساب بنك الكريمي للتمويل الأصغر (YER)", "Kuraimi Bank YER", 1, true, 7, true, balance: 48000.00m);

        Add(9, "11020002", "محفظة جيب الإلكترونية (JEB Wallet)", "Jeb E-Wallet", 1, true, 7, false, balance: 6200.00m);

        Add(10, "11020003", "محفظة كاش وبنك اليمن والكويت", "YKB Bank & Cash Wallet", 1, true, 7, false, balance: 9100.00m);

        // ── 1103 — حسابات العملاء ──
        Add(11, "1103", "حسابات العملاء والمدينون", "Accounts Receivable", 1, true, 2, true, balance: 12500.00m);

        // ── 1104 — بضاعة آخر المدة ──
        Add(12, "1104", "بضاعة آخر المدة (مخزون المستودعات)", "Inventory / Stock", 1, true, 2, true);

        // ── 1105 — حساب المخزون (بضاعة) لحسابات المستودعات ──
        Add(41, "1105", "المخزون (بضاعة)", "Inventory / Goods", 1, true, 2, false, balance: 38000.00m);

        // ── 1107 — عهد الموظفين (فئة تصنيف مطلوبة — AccountCategoryHelper) ──
        Add(42, "1107", "عهد الموظفين", "Employee Custody", 1, true, 2, false);

        // ── 12 — الأصول الثابتة ──
        Add(13, "12", "الأصول الثابتة (Fixed Assets)", "Fixed Assets", 1, false, 1, true);

        Add(14, "1201", "أجهزة الكمبيوتر ونقاط البيع POS", "Computers & POS Hardware", 1, true, 13, false, balance: 18000.00m);

        Add(15, "1202", "أرفف وثلاجات العرض والمعدات", "Shelves & Refrigerators", 1, true, 13, false, balance: 9500.00m);

        // ═══════════════════════════════════════════════
        // 2 — الخصوم وحقوق الملكية (Liabilities & Equity)
        // ═══════════════════════════════════════════════
        Add(16, "2", "الخصوم وحقوق الملكية (Liabilities & Equity)", "Liabilities & Equity", 2, false, null, true);

        // ── 21 — الخصوم المتداولة ──
        Add(17, "21", "الخصوم المتداولة (Current Liabilities)", "Current Liabilities", 2, false, 16, true);

        Add(18, "2101", "حسابات الموردين والدائنون", "Accounts Payable", 2, true, 17, true, balance: 8400.00m);

        Add(19, "2102", "مصروفات مستحقة الدفع وأمانات", "Accrued Expenses", 2, true, 17, false, balance: 3200.00m);

        Add(20, "2103", "ضرائب مستحقة (ضريبة القيمة المضافة)", "VAT Payable", 2, true, 17, false, balance: 1750.00m);

        // ── 22 — حقوق الملكية ──
        Add(21, "22", "حقوق الملكية ورأس المال (Equity)", "Equity & Capital", 3, false, 16, true);

        Add(22, "2201", "رأس المال المدفوع", "Paid Capital", 3, true, 21, true, balance: 120000.00m);

        Add(23, "2202", "الأرباح والخسائر المدورة (المرحلة)", "Retained Earnings", 3, true, 21, true, balance: 12500.00m);

        // حساب مضاد: مسحوبات المالك داخل حقوق الملكية بطبيعة مدينة ⇒ يُخزَّن NormalBalance = 1
        Add(24, "2203", "جاري المالك / المسحوبات الشخصية", "Owner Drawings", 3, true, 21, false, balance: 3000.00m, normalBalance: 1);

        // ═══════════════════════════════════════════════
        // 3 — الإيرادات (Revenues)
        // ═══════════════════════════════════════════════
        Add(25, "3", "الإيرادات (Revenues)", "Revenues", 4, false, null, true);

        Add(26, "31", "إيراد مبيعات البضاعة (Sales Revenue)", "Merchandise Sales Revenue", 4, true, 25, true, balance: 86000.00m);

        Add(27, "32", "إيرادات خدمات وتوصيل ونقل", "Delivery & Service Revenues", 4, true, 25, false, balance: 4200.00m);

        Add(28, "33", "أرباح وفائض الجرد المخزني", "Stock Surplus Revenue", 4, true, 25, false, balance: 800.00m);

        Add(29, "34", "الخصم المكتسب من الموردين (خصم المشتريات)", "Discounts Received from Suppliers", 4, true, 25, false, balance: 1250.00m);

        // ═══════════════════════════════════════════════
        // 4 — المصروفات (Expenses)
        // ═══════════════════════════════════════════════
        Add(30, "4", "المصروفات (Expenses)", "Expenses", 5, false, null, true);

        Add(31, "41", "تكلفة البضاعة المباعة (COGS)", "Cost of Goods Sold", 5, true, 30, true, balance: 64000.00m, allowCostCenter: true);

        // ── 42 — المصروفات العمومية ──
        Add(32, "42", "المصروفات العمومية والتشغيلية", "General & Operating Expenses", 5, false, 30, true);

        Add(33, "4201", "مصروفات الإيجار والكهرباء والمياه", "Rent, Power & Utilities", 5, true, 32, false, balance: 6000.00m, allowCostCenter: true);

        Add(34, "4202", "رواتب وأجور الموظفين والعمال", "Salaries & Wages", 5, true, 32, false, balance: 12000.00m, allowCostCenter: true);

        Add(35, "4203", "مصروفات الصيانة وضيافة المحل", "Maintenance & Hospitality", 5, true, 32, false, balance: 1500.00m, allowCostCenter: true);

        Add(36, "4204", "مصروفات أجور النقل والمواصلات", "Transportation & Freight Expenses", 5, true, 32, false, balance: 2100.00m, allowCostCenter: true);

        Add(37, "4205", "مصروفات الشحن وخدمات التوصيل", "Shipping & Delivery Services", 5, true, 32, false, balance: 1800.00m, allowCostCenter: true);

        Add(38, "4206", "مصروفات العتالة والتحميل والتفريغ", "Handling & Loading Charges", 5, true, 32, false, balance: 900.00m, allowCostCenter: true);

        Add(39, "4207", "مصروفات نثرية وتشغيلية أخرى", "Other Operating & Sundry Expenses", 5, true, 32, false, balance: 450.00m, allowCostCenter: true);

        Add(40, "43", "عجز وتلفيات الجرد المخزني", "Stock Deficit & Shrinkage Expense", 5, true, 30, false, balance: 300.00m, allowCostCenter: true);

        return rows;
    }

    /// <summary>لون العرض الافتراضي بحسب طبيعة الحساب (بيانات تجريبية فقط — ألوان الواجهة من `Helpers/`).</summary>
    private static string ColorFor(byte nature) => nature switch
    {
        1 => "#2196F3", // أصول
        2 => "#E91E63", // خصوم
        3 => "#9C27B0", // حقوق ملكية
        4 => "#4CAF50", // إيرادات
        _ => "#FF5722"  // مصروفات
    };

    /// <summary>
    /// يعيد اشتقاق السمات المعتمدة على الشجرة بعد أي إضافة أو تعديل — بنفس قواعد طبقة الأعمال:
    /// المستوى من سلسلة الآباء، ومسار الشجرة من الأكواد، وتصنيف الحساب من البيت الواحد.
    /// </summary>
    public static void RefreshDerived()
    {
        var byId = Accounts.ToDictionary(a => a.Id);

        string PathOf(AccountDto account)
        {
            var codes = new List<string>();
            AccountDto? current = account;
            for (var depth = 0; current != null && depth < 10; depth++)
            {
                codes.Insert(0, current.AccountCode);
                current = current.ParentId.HasValue && byId.TryGetValue(current.ParentId.Value, out var parent)
                    ? parent
                    : null;
            }
            return "/" + string.Join("/", codes) + "/";
        }

        for (var i = 0; i < Accounts.Count; i++)
        {
            var account = Accounts[i];
            AccountDto? parent = account.ParentId.HasValue && byId.TryGetValue(account.ParentId.Value, out var p) ? p : null;
            var path = PathOf(account);
            var level = (byte)path.Split('/', StringSplitOptions.RemoveEmptyEntries).Length;
            var category = AccountCategoryHelper.Detect(account.AccountCode, account.NameAr);

            Accounts[i] = account with
            {
                Level = level,
                TreePath = path,
                ParentAccountCode = parent?.AccountCode,
                ParentAccountName = parent?.NameAr,
                CategoryLabel = category.Label
            };
        }
    }

    /// <summary>
    /// ينشئ حساباً فرعياً في دليل الـMock بكود من <see cref="NextChildCode"/> ويضيفه إلى <see cref="Accounts"/>،
    /// ثم يعيد السجل كما استقرّ في الدليل (بمعرّفه الحقيقي) — **البيت الواحد** لإنشاء حساب في وضع التجربة:
    /// تستهلكه خدمة الحسابات (شاشة دليل الحسابات) وخدمة الصناديق النقدية، فلا نسخة ثانية من بناء الحساب.
    /// </summary>
    public static AccountDto AddChildAccount(
        int? parentId,
        string nameAr,
        string? nameEn,
        byte nature,
        bool isLeaf,
        string colorCode,
        decimal balance = 0m,
        byte? normalBalance = null,
        bool allowCostCenter = false,
        short? categoryId = null,
        string? description = null,
        string? notes = null)
    {
        AccountDto? parent = parentId.HasValue
            ? Accounts.FirstOrDefault(a => a.Id == parentId.Value)
            : null;

        var code = NextChildCode(parentId);

        var created = new AccountDto(
            Id: NextId(),
            AccountCode: code,
            NameAr: nameAr,
            NameEn: nameEn,
            Nature: nature,
            IsLeaf: isLeaf,
            ParentId: parentId,
            ParentAccountName: parent?.NameAr,
            IsSystem: false,
            IsActive: true,
            CategoryId: categoryId,
            Level: (byte)(parent is null ? 1 : parent.Level + 1),
            Description: description,
            ColorCode: colorCode,
            Notes: notes,
            ParentAccountCode: parent?.AccountCode,
            TreePath: (parent?.TreePath ?? "/") + code + "/",
            FinancialStatement: (byte)(nature <= 3 ? 1 : 2),
            NormalBalance: normalBalance ?? 0,   // 0 = غير مخزَّن ⇒ يُشتق من Nature
            AllowCostCenter: allowCostCenter,
            Balance: balance,
            CategoryLabel: null);

        Accounts.Add(created);
        RefreshDerived();

        // السجل بعد إعادة اشتقاق السمات (RefreshDerived يستبدل العناصر بسجلات جديدة)
        return Accounts.First(a => a.Id == created.Id);
    }

    /// <summary>كود مجموعة الصناديق والخزائن النقدية في الدليل — أب حسابات الصناديق (مستوى 3).</summary>
    public const string CashGroupCode = "1101";

    /// <summary>
    /// يضيف حساب صندوق نقدي (ورقة أصول) تحت مجموعة <see cref="CashGroupCode"/> بكود غير مستخدم من السلّم الرسمي،
    /// ويعيده بمعرّفه الحقيقي — أو <c>null</c> إن لم تكن مجموعة الصناديق موجودة في الدليل.
    /// </summary>
    public static AccountDto? AddCashBoxAccount(string nameAr)
    {
        var group = Accounts.FirstOrDefault(a => a.AccountCode == CashGroupCode);
        if (group is null)
            return null;

        return AddChildAccount(
            parentId: group.Id,
            nameAr: nameAr,
            nameEn: null,
            nature: 1,          // أصل
            isLeaf: true,
            colorCode: ColorFor(1));
    }

    /// <summary>معرّف جديد للحساب في دليل الـMock (أعلى معرّف + 1).</summary>
    private static int NextId() => Accounts.Count > 0 ? Accounts.Max(a => a.Id) + 1 : 1;

    /// <summary>
    /// كود الحساب الفرعي التالي تحت الأب المحدد (`null` = حساب جذر) بسلّم الأطوال الرسمي،
    /// مع تخطّي كل كود موجود فعلاً تحت نفس الأب — مطابق لمولّد الأكواد في طبقة الأعمال.
    /// </summary>
    public static string NextChildCode(int? parentId)
    {
        if (!parentId.HasValue)
        {
            var usedRootCodes = Accounts
                .Where(a => a.Level == 1)
                .Select(a => a.AccountCode)
                .ToHashSet(StringComparer.Ordinal);

            var nextRoot = 1;
            while (usedRootCodes.Contains(nextRoot.ToString()))
                nextRoot++;
            return nextRoot.ToString();
        }

        var parent = Accounts.FirstOrDefault(a => a.Id == parentId.Value);
        if (parent is null)
            return string.Empty;

        var level = (byte)(parent.Level + 1);
        var children = Accounts.Where(a => a.ParentId == parentId.Value).ToList();
        var usedChildCodes = children.Select(c => c.AccountCode).ToHashSet(StringComparer.Ordinal);

        var maxSuffix = children
            .Select(c => c.AccountCode.Length > parent.AccountCode.Length
                         && int.TryParse(c.AccountCode[parent.AccountCode.Length..], out var number)
                ? number
                : 0)
            .DefaultIfEmpty(0)
            .Max();

        var suffix = maxSuffix + 1;
        var code = FormatChildCode(parent.AccountCode, level, suffix);
        while (usedChildCodes.Contains(code))
        {
            suffix++;
            code = FormatChildCode(parent.AccountCode, level, suffix);
        }

        return code;
    }

    /// <summary>سلّم اللواحق الرسمي: مستوى 2 ⇒ خانة · 3 ⇒ خانتان · 4 ⇒ 4 خانات · أعمق ⇒ زيادة عادية.</summary>
    private static string FormatChildCode(string parentCode, byte level, int suffix) => level switch
    {
        2 => parentCode + suffix.ToString("0"),
        3 => parentCode + suffix.ToString("00"),
        4 => parentCode + suffix.ToString("0000"),
        _ => parentCode + suffix.ToString()
    };
}
