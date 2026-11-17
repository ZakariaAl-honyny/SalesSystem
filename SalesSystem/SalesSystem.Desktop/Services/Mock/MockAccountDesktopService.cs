using SalesSystem.Business.Interfaces.Services;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة دليل الحسابات — تعيد بيانات من MockAccountsData وتفرض نفس رفض طبقة الأعمال (`RULE-N167`):
/// اسم فارغ · أب غير موجود · أب غير نشط · أب ورقة · تكرار الاسم تحت نفس الأب · عمق يتجاوز 10 ·
/// طبيعة مدينة/دائنة غير صالحة · نوع حساب خارج 1-5 · رصيد افتتاحي سالب ·
/// تعديل أو حذف حساب نظامي · حذف حساب له أبناء أو حركات · حذف نهائي لحساب نشط أو مرتبط بمعاملات.
/// كل رسالة عربية هنا منقولة **حرفياً** من `AccountService`/`Account`.
/// </summary>
public class MockAccountDesktopService : IAccountDesktopService
{
    /// <summary>رسالة العمق الأقصى — مطابقة لرسالة طبقة الأعمال (`AccountService.MaxDepthMessage`).</summary>
    private const string MaxDepthMessage = "لا يمكن إضافة حساب فرعي — العمق الأقصى للشجرة هو 10 مستويات";

    /// <summary>رسالة الاسم الفارغ — مطابقة لرسالة الكيان (`Account.Create`/`Account.Update`).</summary>
    private const string NameRequiredMessage = "اسم الحساب بالعربية مطلوب";

    /// <summary>رسالة نوع الحساب غير الصالح — مطابقة لرسالة الكيان (`Account.Create`/`Account.Update`).</summary>
    private const string InvalidNatureMessage = "نوع الحساب غير صالح — القيم المسموحة: 1-5";

    /// <summary>رسالة الحساب الأب غير النشط — مطابقة لرسالة طبقة الأعمال (`AccountService.CreateAsync`).</summary>
    private const string InactiveParentMessage = "الحساب الأب غير نشط";

    /// <summary>رسالة الرصيد الافتتاحي السالب — مطابقة لرسالة طبقة الأعمال (`AccountService.CreateAsync`).</summary>
    private const string NegativeOpeningBalanceMessage = "لا يمكن أن يكون الرصيد الافتتاحي سالباً";

    /// <summary>رسالة الطبيعة غير الصالحة — مطابقة لرسالة الكيان (`Account.Create`/`Account.Update`).</summary>
    private const string InvalidNormalBalanceMessage =
        "الطبيعة المدينة/الدائنة غير صالحة — القيم المسموحة: 1 (مدين) أو 2 (دائن)";

    public Task<Result<IReadOnlyList<AccountDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<AccountDto> query = MockAccountsData.Accounts;

        if (!includeInactive)
            query = query.Where(a => a.IsActive);

        return Task.FromResult(Result<IReadOnlyList<AccountDto>>.Success(
            query.OrderBy(a => a.AccountCode).ToList().AsReadOnly()));
    }

    public Task<Result<AccountDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var account = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == id);
        return account is null
            ? Task.FromResult(Result<AccountDto>.Failure("الحساب غير موجود"))
            : Task.FromResult(Result<AccountDto>.Success(account));
    }

    public Task<Result<IReadOnlyList<AccountTreeNodeDto>>> GetTreeAsync(CancellationToken ct = default)
    {
        var all = MockAccountsData.Accounts;
        var roots = all.Where(a => a.ParentId == null && a.IsActive)
            .OrderBy(a => a.AccountCode)
            .Select(a => BuildTreeNode(a, all))
            .ToList();

        return Task.FromResult(Result<IReadOnlyList<AccountTreeNodeDto>>.Success(roots.AsReadOnly()));
    }

    private static AccountTreeNodeDto BuildTreeNode(AccountDto account, IReadOnlyList<AccountDto> all)
    {
        return new AccountTreeNodeDto(
            account.Id,
            account.AccountCode,
            account.NameAr,
            account.Nature,
            account.IsLeaf,
            account.CategoryId,
            account.Level,
            account.ColorCode,
            account.Description,
            all.Where(c => c.ParentId == account.Id && c.IsActive)
                .OrderBy(c => c.AccountCode)
                .Select(c => BuildTreeNode(c, all))
                .ToList(),
            ParentAccountCode: account.ParentAccountCode,
            TreePath: account.TreePath,
            FinancialStatement: account.FinancialStatement,
            NormalBalance: account.NormalBalance,
            AllowCostCenter: account.AllowCostCenter,
            CategoryLabel: account.CategoryLabel
        );
    }

    public Task<Result<int>> CreateAsync(CreateAccountRequest request, CancellationToken ct = default)
    {
        // تكافؤ الرفض مع الـBLL (RULE-N167) — بنفس ترتيب `AccountService.CreateAsync` ورسائله الحرفية
        if (string.IsNullOrWhiteSpace(request.NameAr))
            return Task.FromResult(Result<int>.Failure(NameRequiredMessage));

        if (request.NormalBalance.HasValue && request.NormalBalance.Value != 1 && request.NormalBalance.Value != 2)
            return Task.FromResult(Result<int>.Failure(InvalidNormalBalanceMessage));

        if (request.Nature < 1 || request.Nature > 5)
            return Task.FromResult(Result<int>.Failure(InvalidNatureMessage));

        AccountDto? parent = null;
        if (request.ParentId.HasValue)
        {
            parent = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == request.ParentId.Value);
            if (parent is null)
                return Task.FromResult(Result<int>.Failure("الحساب الأب غير موجود"));

            if (!parent.IsActive)
                return Task.FromResult(Result<int>.Failure(InactiveParentMessage));

            if (parent.IsLeaf)
                return Task.FromResult(Result<int>.Failure(
                    "لا يمكن إضافة حساب فرعي لحساب تفصيلي — الحساب الأب يجب أن يكون مجموعة"));

            var duplicate = MockAccountsData.Accounts.Any(a =>
                a.ParentId == request.ParentId.Value &&
                a.IsActive &&
                string.Equals(a.NameAr, request.NameAr.Trim(), StringComparison.Ordinal));
            if (duplicate)
                return Task.FromResult(Result<int>.Failure(
                    $"يوجد حساب بنفس الاسم '{request.NameAr}' تحت نفس الحساب الأب"));
        }

        var level = (byte)(parent is null ? 1 : parent.Level + 1);
        if (level > 10)
            return Task.FromResult(Result<int>.Failure(MaxDepthMessage));

        if (request.OpeningBalance.HasValue && request.OpeningBalance.Value < 0)
            return Task.FromResult(Result<int>.Failure(NegativeOpeningBalanceMessage));

        // الرصيد الافتتاحي: طبقة الأعمال ترحّل قيداً على الحساب في **جهة رصيده الطبيعية**،
        // و`AccountDto.Balance` يُعرض بإشارة الطبيعة نفسها (`AccountService.MapToDto`) ⇒ يبقى موجباً
        // سواء كان الحساب مديناً أو دائناً. لا مخزن قيود في الـMock، فيُعكس الأثر في الرصيد مباشرة.
        var openingBalance = request.OpeningBalance is > 0 ? request.OpeningBalance.Value : 0m;

        var created = MockAccountsData.AddChildAccount(
            parentId: request.ParentId,
            nameAr: request.NameAr.Trim(),
            nameEn: request.NameEn,
            nature: request.Nature,
            isLeaf: request.IsLeaf,
            colorCode: IAccountCodeGeneratorService.GetColorCode(request.Nature),
            balance: openingBalance,
            // يُخزَّن كما أرسله المستخدم: null ⇒ 0 (لم تُحدَّد — تُشتق من Nature)، و1/2 كما هي
            normalBalance: request.NormalBalance,
            allowCostCenter: request.AllowCostCenter,
            categoryId: request.CategoryId,
            description: request.Description,
            notes: request.Notes);

        return Task.FromResult(Result<int>.Success(created.Id));
    }

    public Task<Result> UpdateAsync(int id, UpdateAccountRequest request, CancellationToken ct = default)
    {
        var existing = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الحساب غير موجود"));

        if (existing.IsSystem)
            return Task.FromResult(Result.Failure("لا يمكن تعديل حساب نظامي"));

        // تكافؤ الرفض مع الـBLL (RULE-N167)
        if (string.IsNullOrWhiteSpace(request.NameAr))
            return Task.FromResult(Result.Failure(NameRequiredMessage));

        if (request.NormalBalance.HasValue && request.NormalBalance.Value != 1 && request.NormalBalance.Value != 2)
            return Task.FromResult(Result.Failure(InvalidNormalBalanceMessage));

        if (request.Nature < 1 || request.Nature > 5)
            return Task.FromResult(Result.Failure(InvalidNatureMessage));

        AccountDto? parent = null;
        if (request.ParentId.HasValue)
        {
            if (request.ParentId.Value == id)
                return Task.FromResult(Result.Failure("لا يمكن أن يكون الحساب أباً لنفسه"));

            parent = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == request.ParentId.Value);
            if (parent is null)
                return Task.FromResult(Result.Failure("الحساب الأب غير موجود"));

            if (!parent.IsActive)
                return Task.FromResult(Result.Failure(InactiveParentMessage));

            if (parent.IsLeaf)
                return Task.FromResult(Result.Failure("لا يمكن إضافة حساب فرعي لحساب تفصيلي"));

            var duplicate = MockAccountsData.Accounts.Any(a =>
                a.Id != id &&
                a.ParentId == request.ParentId.Value &&
                a.IsActive &&
                string.Equals(a.NameAr, request.NameAr.Trim(), StringComparison.Ordinal));
            if (duplicate)
                return Task.FromResult(Result.Failure(
                    $"يوجد حساب بنفس الاسم '{request.NameAr}' تحت نفس الحساب الأب"));
        }

        var level = (byte)(parent is null ? 1 : parent.Level + 1);
        if (level > 10)
            return Task.FromResult(Result.Failure(MaxDepthMessage));

        var updated = existing with
        {
            NameAr = request.NameAr.Trim(),
            NameEn = request.NameEn,
            Nature = request.Nature,
            IsLeaf = request.IsLeaf,
            ParentId = request.ParentId,
            CategoryId = request.CategoryId,
            Description = request.Description,
            Notes = request.Notes,
            Level = level,
            ColorCode = IAccountCodeGeneratorService.GetColorCode(request.Nature),
            FinancialStatement = (byte)(request.Nature <= 3 ? 1 : 2),
            // يُخزَّن كما أرسله المستخدم: null ⇒ 0 (لم تُحدَّد — تُشتق من Nature)، و1/2 كما هي
            NormalBalance = request.NormalBalance ?? 0,
            AllowCostCenter = request.AllowCostCenter
        };

        var index = MockAccountsData.Accounts.IndexOf(existing);
        MockAccountsData.Accounts[index] = updated;
        MockAccountsData.RefreshDerived();

        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الحساب غير موجود"));

        if (existing.IsSystem)
            return Task.FromResult(Result.Failure("لا يمكن حذف حساب نظامي"));

        var hasChildren = MockAccountsData.Accounts.Any(a => a.ParentId == id && a.IsActive);
        if (hasChildren)
            return Task.FromResult(Result.Failure("لا يمكن حذف حساب رئيسي لديه حسابات فرعية — احذف الحسابات الفرعية أولاً"));

        if (existing.Balance != 0m)
            return Task.FromResult(Result.Failure("لا يمكن إلغاء تنشيط حساب مسجل عليه حركات مالية أو أرصدة"));

        var index = MockAccountsData.Accounts.IndexOf(existing);
        MockAccountsData.Accounts[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> PermanentDeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الحساب غير موجود"));

        if (existing.IsSystem)
            return Task.FromResult(Result.Failure("لا يمكن حذف حساب نظامي"));

        var hasChildren = MockAccountsData.Accounts.Any(a => a.ParentId == id);
        if (hasChildren)
            return Task.FromResult(Result.Failure("لا يمكن حذف حساب رئيسي لديه حسابات فرعية"));

        // حارس «التعطيل أولاً» — مطابق لطبقة الأعمال (`AccountService.PermanentDeleteAsync`)
        if (existing.IsActive)
            return Task.FromResult(Result.Failure("لا يمكن الحذف النهائي لحساب نشط. عطّل الحساب أولاً."));

        // فحص الارتباطات قبل الحذف — رسائل طبقة الأعمال حرفياً، وبمخازن الـMock نفسها
        if (MockJournalEntriesData.EntryDetails.Any(e => e.Lines.Any(l => l.AccountId == id)))
            return Task.FromResult(Result.Failure("لا يمكن الحذف النهائي: الحساب مرتبط بحركات يومية"));

        if (MockData.Customers.Any(c => c.AccountId == id))
            return Task.FromResult(Result.Failure("لا يمكن الحذف النهائي: الحساب مرتبط بعملاء"));

        if (MockSuppliersData.Suppliers.Any(s => s.AccountId == id))
            return Task.FromResult(Result.Failure("لا يمكن الحذف النهائي: الحساب مرتبط بموردين"));

        if (MockData.CashBoxes.Any(cb => cb.AccountId == id))
            return Task.FromResult(Result.Failure("لا يمكن الحذف النهائي: الحساب مرتبط بصناديق نقدية"));

        MockAccountsData.Accounts.Remove(existing);
        return Task.FromResult(Result.Success());
    }

    public Task<Result<AccountCodePreviewDto>> GetNextChildCodeAsync(int parentId, CancellationToken ct = default)
    {
        var parent = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == parentId);
        if (parent is null)
            return Task.FromResult(Result<AccountCodePreviewDto>.Failure("الحساب غير موجود"));

        if (!parent.IsActive)
            return Task.FromResult(Result<AccountCodePreviewDto>.Failure(InactiveParentMessage));

        if (parent.IsLeaf)
            return Task.FromResult(Result<AccountCodePreviewDto>.Failure(
                "لا يمكن إضافة حساب فرعي تحت حساب ورقة"));

        var level = (byte)(parent.Level + 1);
        if (level > 10)
            return Task.FromResult(Result<AccountCodePreviewDto>.Failure(MaxDepthMessage));

        var code = MockAccountsData.NextChildCode(parentId);
        var treePath = parent.TreePath + code + "/";

        return Task.FromResult(Result<AccountCodePreviewDto>.Success(
            new AccountCodePreviewDto(code, parent.AccountCode, level, treePath)));
    }
}
