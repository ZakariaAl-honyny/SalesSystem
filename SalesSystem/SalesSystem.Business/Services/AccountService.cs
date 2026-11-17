using Microsoft.Extensions.Logging;
using SalesSystem.Business.Helpers;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.Common.Exceptions;
using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Models.Accounting;
using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.Business.Services;

public class AccountService : IAccountService
{
    /// <summary>Deeper than this is refused — the schema CHECK constraint allows levels 1..10 only.</summary>
    private const string MaxDepthMessage = "لا يمكن إضافة حساب فرعي — العمق الأقصى للشجرة هو 10 مستويات";

    private readonly IUnitOfWork _uow;
    private readonly ILogger<AccountService> _logger;
    private readonly IAccountCodeGeneratorService _codeGenerator;

    public AccountService(
        IUnitOfWork uow,
        ILogger<AccountService> logger,
        IAccountCodeGeneratorService codeGenerator)
    {
        _uow = uow;
        _logger = logger;
        _codeGenerator = codeGenerator;
    }

    public async Task<Result<List<AccountTreeNodeDto>>> GetTreeAsync(CancellationToken ct)
    {
        var lookups = await LoadLookupsAsync(includeBalances: false, includeInactive: false, ct);
        var roots = lookups.ById.Values
            .Where(a => a.ParentId == null && a.IsActive)
            .OrderBy(a => a.AccountCode)
            .Select(a => BuildTreeNode(a, lookups))
            .ToList();
        return Result<List<AccountTreeNodeDto>>.Success(roots);
    }

    private static AccountTreeNodeDto BuildTreeNode(Account account, AccountLookups lookups)
    {
        Account? parent = account.ParentId.HasValue && lookups.ById.TryGetValue(account.ParentId.Value, out var p)
            ? p
            : null;

        var category = AccountCategoryHelper.Detect(account.AccountCode, account.NameAr);

        return new AccountTreeNodeDto(
            account.Id,
            account.AccountCode,
            account.NameAr,
            account.Nature,
            account.IsLeaf,
            null,
            account.Level,
            account.ColorCode,
            account.Description,
            lookups.ById.Values
                .Where(c => c.ParentId == account.Id && c.IsActive)
                .OrderBy(c => c.AccountCode)
                .Select(c => BuildTreeNode(c, lookups))
                .ToList(),
            ParentAccountCode: parent?.AccountCode,
            TreePath: BuildTreePath(account, lookups.ById),
            FinancialStatement: account.FinancialStatement,
            NormalBalance: account.NormalBalance ?? 0,
            AllowCostCenter: account.AllowCostCenter,
            CategoryLabel: category.Label
        );
    }

    public async Task<Result<List<AccountDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var lookups = await LoadLookupsAsync(includeBalances: true, includeInactive, ct);
        var dtos = lookups.ById.Values
            .Where(a => includeInactive || a.IsActive)
            .OrderBy(a => a.AccountCode)
            .Select(a => MapToDto(a, lookups))
            .ToList();
        return Result<List<AccountDto>>.Success(dtos);
    }

    public async Task<Result<AccountDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var account = await _uow.Accounts.GetByIdAsync(id, ct);
        if (account == null)
            return Result<AccountDto>.Failure("الحساب غير موجود", ErrorCodes.NotFound);

        var lookups = await LoadLookupsAsync(includeBalances: true, includeInactive: false, ct);
        return Result<AccountDto>.Success(MapToDto(account, lookups));
    }

    public async Task<Result<List<AccountDto>>> GetByTypeAsync(AccountType type, CancellationToken ct)
    {
        byte nature = (byte)type;
        var lookups = await LoadLookupsAsync(includeBalances: true, includeInactive: false, ct);
        var dtos = lookups.ById.Values
            .Where(a => a.Nature == nature && a.IsActive)
            .OrderBy(a => a.AccountCode)
            .Select(a => MapToDto(a, lookups))
            .ToList();
        return Result<List<AccountDto>>.Success(dtos);
    }

    /// <inheritdoc />
    public async Task<Result<AccountCodePreviewDto>> GetNextChildCodeAsync(int parentId, CancellationToken ct)
    {
        var parent = await _uow.Accounts.GetByIdAsync(parentId, ct);
        if (parent == null)
            return Result<AccountCodePreviewDto>.Failure("الحساب غير موجود", ErrorCodes.NotFound);

        if (parent.IsLeaf)
            return Result<AccountCodePreviewDto>.Failure(
                "لا يمكن إضافة حساب فرعي تحت حساب ورقة", ErrorCodes.ValidationError);

        var level = (byte)(parent.Level + 1);
        if (level > 10)
            return Result<AccountCodePreviewDto>.Failure(MaxDepthMessage, ErrorCodes.ValidationError);

        var codeResult = await _codeGenerator.GenerateCodeAsync(parentId, level, ct);
        if (!codeResult.IsSuccess)
            return Result<AccountCodePreviewDto>.Failure(codeResult.Error!, codeResult.ErrorCode);

        var lookups = await LoadLookupsAsync(includeBalances: false, includeInactive: false, ct);
        var nextCode = codeResult.Value!;
        var treePath = BuildTreePath(parent, lookups.ById) + nextCode + "/";

        return Result<AccountCodePreviewDto>.Success(
            new AccountCodePreviewDto(nextCode, parent.AccountCode, level, treePath));
    }

    public async Task<Result<AccountDto>> CreateAsync(CreateAccountRequest request, int userId, CancellationToken ct)
    {
        // Step 1: Validate parent if provided
        Account? parent = null;
        if (request.ParentId.HasValue)
        {
            parent = await _uow.Accounts.GetByIdAsync(request.ParentId.Value, ct);
            if (parent == null)
                return Result<AccountDto>.Failure("الحساب الأب غير موجود", ErrorCodes.NotFound);
            if (!parent.IsActive)
                return Result<AccountDto>.Failure("الحساب الأب غير نشط", ErrorCodes.InvalidOperation);
            if (parent.IsLeaf)
                return Result<AccountDto>.Failure(
                    "لا يمكن إضافة حساب فرعي لحساب تفصيلي — الحساب الأب يجب أن يكون مجموعة",
                    ErrorCodes.ValidationError);
        }

        // Step 2: Check for duplicate account by parent + nameAr
        if (request.ParentId.HasValue)
        {
            var duplicateName = await _uow.Accounts.AnyAsync(
                a => a.ParentId == request.ParentId.Value && a.NameAr == request.NameAr.Trim() && a.IsActive, ct);
            if (duplicateName)
                return Result<AccountDto>.Failure(
                    $"يوجد حساب بنفس الاسم '{request.NameAr}' تحت نفس الحساب الأب",
                    ErrorCodes.DuplicateEntry);
        }

        // Step 3: Compute level — level = parent.Level + 1 (1 for a root). A leaf is allowed at any level (§4.1.2).
        var level = (byte)(parent is null ? 1 : parent.Level + 1);
        if (level > 10)
            return Result<AccountDto>.Failure(MaxDepthMessage, ErrorCodes.ValidationError);

        // Step 4: Generate account code
        var codeResult = await _codeGenerator.GenerateCodeAsync(request.ParentId, level, ct);
        if (!codeResult.IsSuccess)
            return Result<AccountDto>.Failure(codeResult.Error!, codeResult.ErrorCode);

        // Step 5: Auto-generate color code from nature
        var colorCode = IAccountCodeGeneratorService.GetColorCode(request.Nature);

        // Step 5a: Reject negative opening balance explicitly
        if (request.OpeningBalance.HasValue && request.OpeningBalance.Value < 0)
            return Result<AccountDto>.Failure("لا يمكن أن يكون الرصيد الافتتاحي سالباً", ErrorCodes.ValidationError);

        // Step 6: Create the account entity
        // ملاحظة: إشارات IsSystem من الطلب تُجاهل عمداً — الحسابات النظامية يُنشأ من البذور فقط
        Account account;
        try
        {
            account = Account.Create(
                accountCode: codeResult.Value!,
                nameAr: request.NameAr,
                nameEn: request.NameEn,
                nature: request.Nature,
                isLeaf: request.IsLeaf,
                parentId: request.ParentId,
                isSystem: false,
                level: level,
                description: request.Description,
                colorCode: colorCode,
                notes: request.Notes,
                normalBalance: request.NormalBalance,
                allowCostCenter: request.AllowCostCenter);
        }
        catch (DomainException ex)
        {
            // قواعد الكيان هي البيت الواحد للتحقق — تُترجم هنا إلى نتيجة عربية بدل تسريب استثناء
            return Result<AccountDto>.Failure(ex.Message, ErrorCodes.ValidationError);
        }

        // Step 7: Persist atomically — account + optional opening balance journal entry
        await _uow.ExecuteTransactionAsync(async () =>
        {
            await _uow.Accounts.AddAsync(account, ct);
            await _uow.SaveChangesAsync(ct);

            // If OpeningBalance > 0, create a Journal Entry
            if (request.OpeningBalance.HasValue && request.OpeningBalance.Value > 0)
            {
                var systemMapping = await _uow.SystemAccountMappings.FirstOrDefaultAsync(
                    m => m.MappingKey == SystemAccountKey.OpeningBalanceEquity.ToString(), ct);

                var entryNumber = $"OB-{DateTime.UtcNow:yyyyMMdd}-{account.Id:D4}";

                var je = JournalEntry.Create(
                    entryNumber: entryNumber,
                    entryNo: account.Id,
                    entryDate: DateTime.UtcNow,
                    description: $"الرصيد الافتتاحي للحساب {account.NameAr}",
                    entryType: JournalEntryType.OpeningBalance,
                    referenceType: "Account",
                    referenceId: account.Id);

                // Debit-normal accounts (EffectiveNormalBalance == 1): Dr the account, Cr OpeningBalanceEquity
                // Uses EffectiveNormalBalance (NormalBalance ?? Nature-based) to correctly sign contra accounts.
                if (account.EffectiveNormalBalance == 1)
                {
                    je.AddDebitLine(account.Id, request.OpeningBalance.Value, "الرصيد الافتتاحي");
                    if (systemMapping != null)
                        je.AddCreditLine(systemMapping.AccountId, request.OpeningBalance.Value, "الرصيد الافتتاحي");
                }
                else
                {
                    // Credit-normal accounts (EffectiveNormalBalance == 2): Cr the account, Dr OpeningBalanceEquity
                    je.AddCreditLine(account.Id, request.OpeningBalance.Value, "الرصيد الافتتاحي");
                    if (systemMapping != null)
                        je.AddDebitLine(systemMapping.AccountId, request.OpeningBalance.Value, "الرصيد الافتتاحي");
                }

                await _uow.JournalEntries.AddAsync(je, ct);
                await _uow.SaveChangesAsync(ct);
            }
        }, ct);

        _logger.LogInformation(
            "Account {AccountCode} ({NameAr}) created — Level {Level}, Nature {Nature}, IsLeaf {IsLeaf}",
            account.AccountCode, account.NameAr, account.Level, account.Nature, account.IsLeaf);

        // Map with the freshly loaded tree/balance lookups (no per-account queries)
        var lookups = await LoadLookupsAsync(includeBalances: true, includeInactive: false, ct);
        return Result<AccountDto>.Success(MapToDto(account, lookups));
    }

    public async Task<Result<AccountDto>> UpdateAsync(int id, UpdateAccountRequest request, int userId, CancellationToken ct)
    {
        var account = await _uow.Accounts.GetByIdAsync(id, ct);
        if (account == null)
            return Result<AccountDto>.Failure("الحساب غير موجود", ErrorCodes.NotFound);

        if (account.IsSystem)
            return Result<AccountDto>.Failure("لا يمكن تعديل حساب نظامي", ErrorCodes.InvalidOperation);

        // Self-reference check: account cannot be its own parent
        if (request.ParentId.HasValue && request.ParentId.Value == id)
            return Result<AccountDto>.Failure("لا يمكن أن يكون الحساب أباً لنفسه", ErrorCodes.ValidationError);

        // Compute level from parent — level = parent.Level + 1 (1 for a root). A leaf is allowed at any level (§4.1.2).
        Account? parent = null;
        if (request.ParentId.HasValue)
        {
            parent = await _uow.Accounts.GetByIdAsync(request.ParentId.Value, ct);
            if (parent == null)
                return Result<AccountDto>.Failure("الحساب الأب غير موجود", ErrorCodes.NotFound);
            if (parent.IsLeaf)
                return Result<AccountDto>.Failure(
                    "لا يمكن إضافة حساب فرعي لحساب تفصيلي",
                    ErrorCodes.ValidationError);
        }

        // Duplicate name check: prevent duplicate account names under the same parent
        if (request.ParentId.HasValue)
        {
            var duplicateName = await _uow.Accounts.AnyAsync(
                a => a.ParentId == request.ParentId.Value
                     && a.NameAr == request.NameAr.Trim()
                     && a.Id != id, ct);
            if (duplicateName)
                return Result<AccountDto>.Failure(
                    $"يوجد حساب بنفس الاسم '{request.NameAr}' تحت نفس الحساب الأب",
                    ErrorCodes.DuplicateEntry);
        }
        else
        {
            // Root-level duplicate check
            var duplicateRootName = await _uow.Accounts.AnyAsync(
                a => a.ParentId == null
                     && a.NameAr == request.NameAr.Trim()
                     && a.Id != id, ct);
            if (duplicateRootName)
                return Result<AccountDto>.Failure(
                    $"يوجد حساب بنفس الاسم '{request.NameAr}' على مستوى الجذر",
                    ErrorCodes.DuplicateEntry);
        }

        var level = (byte)(parent is null ? 1 : parent.Level + 1);
        if (level > 10)
            return Result<AccountDto>.Failure(MaxDepthMessage, ErrorCodes.ValidationError);

        // Auto-generate color code from nature
        var colorCode = IAccountCodeGeneratorService.GetColorCode(request.Nature);

        // Prevent converting an account with children to IsLeaf = true
        if (request.IsLeaf)
        {
            var hasChildren = await _uow.Accounts.AnyAsync(a => a.ParentId == id, ct);
            if (hasChildren)
                return Result<AccountDto>.Failure(
                    "لا يمكن تحويل حساب إلى تفصيلي بينما لديه حسابات فرعية — احذف الحسابات الفرعية أولاً",
                    ErrorCodes.ValidationError);
        }

        try
        {
            account.Update(
                nameAr: request.NameAr,
                nameEn: request.NameEn,
                nature: request.Nature,
                isLeaf: request.IsLeaf,
                parentId: request.ParentId,
                level: level,
                description: request.Description,
                colorCode: colorCode,
                notes: request.Notes,
                normalBalance: request.NormalBalance,
                allowCostCenter: request.AllowCostCenter);
        }
        catch (DomainException ex)
        {
            // قواعد الكيان هي البيت الواحد للتحقق — تُترجم هنا إلى نتيجة عربية بدل تسريب استثناء
            return Result<AccountDto>.Failure(ex.Message, ErrorCodes.ValidationError);
        }

        await _uow.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Account {Id} ({AccountCode}) updated — Level {Level}, Nature {Nature}",
            id, account.AccountCode, account.Level, account.Nature);

        // Map with the freshly loaded tree/balance lookups (no per-account queries)
        var lookups = await LoadLookupsAsync(includeBalances: true, includeInactive: false, ct);
        return Result<AccountDto>.Success(MapToDto(account, lookups));
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct)
    {
        var account = await _uow.Accounts.GetByIdAsync(id, ct);
        if (account == null)
            return Result.Failure("الحساب غير موجود", ErrorCodes.NotFound);

        if (account.IsSystem)
            return Result.Failure("لا يمكن حذف حساب نظامي", ErrorCodes.InvalidOperation);

        // Use database query instead of HasChildren() — navigation property is not eagerly loaded
        var hasChildren = await _uow.Accounts.AnyAsync(
            a => a.ParentId == id, ct);
        if (hasChildren)
            return Result.Failure(
                "لا يمكن حذف حساب رئيسي لديه حسابات فرعية — احذف الحسابات الفرعية أولاً",
                ErrorCodes.ReferencedByOtherEntities);

        var hasTransactions = await _uow.JournalEntryLines.AnyAsync(l => l.AccountId == id, ct);
        if (hasTransactions)
            return Result.Failure(
                "لا يمكن إلغاء تنشيط حساب مسجل عليه حركات مالية أو أرصدة",
                ErrorCodes.ReferencedByOtherEntities);

        account.MarkAsDeleted();
        await _uow.SaveChangesAsync(ct);

        _logger.LogInformation("Account {Id} ({AccountCode}) soft-deleted", id, account.AccountCode);
        return Result.Success();
    }

    public async Task<Result> PermanentDeleteAsync(int id, CancellationToken ct)
    {
        try
        {
            var account = await _uow.Accounts.GetByIdAsync(id, ct);
            if (account == null)
                return Result.Failure("الحساب غير موجود", ErrorCodes.NotFound);

            if (account.IsSystem)
                return Result.Failure("لا يمكن حذف حساب نظامي", ErrorCodes.InvalidOperation);

            var hasChildren = await _uow.Accounts.AnyAsync(
                a => a.ParentId == id, ct);
            if (hasChildren)
                return Result.Failure(
                    "لا يمكن حذف حساب رئيسي لديه حسابات فرعية",
                    ErrorCodes.ReferencedByOtherEntities);

            // Guard: must be deactivated first
            if (account.IsActive)
            {
                _logger.LogWarning(
                    "Permanent delete refused for Account {Id}: الحساب نشط ويجب تعطيله أولاً",
                    id);
                return Result.Failure(
                    "لا يمكن الحذف النهائي لحساب نشط. عطّل الحساب أولاً.",
                    ErrorCodes.InvalidOperation);
            }

            // Explicit reference checks before deletion
            var hasJournalEntries = await _uow.JournalEntryLines.AnyAsync(l => l.AccountId == id, ct);
            if (hasJournalEntries)
            {
                _logger.LogWarning(
                    "Permanent delete refused for Account {Id}: الحساب مرتبط بحركات يومية",
                    id);
                return Result.Failure(
                    "لا يمكن الحذف النهائي: الحساب مرتبط بحركات يومية",
                    ErrorCodes.ReferencedByOtherEntities);
            }

            var hasCustomers = await _uow.Customers.AnyAsync(c => c.AccountId == id, ct);
            if (hasCustomers)
            {
                _logger.LogWarning(
                    "Permanent delete refused for Account {Id}: الحساب مرتبط بعملاء",
                    id);
                return Result.Failure(
                    "لا يمكن الحذف النهائي: الحساب مرتبط بعملاء",
                    ErrorCodes.ReferencedByOtherEntities);
            }

            var hasSuppliers = await _uow.Suppliers.AnyAsync(s => s.AccountId == id, ct);
            if (hasSuppliers)
            {
                _logger.LogWarning(
                    "Permanent delete refused for Account {Id}: الحساب مرتبط بموردين",
                    id);
                return Result.Failure(
                    "لا يمكن الحذف النهائي: الحساب مرتبط بموردين",
                    ErrorCodes.ReferencedByOtherEntities);
            }

            var hasCashBoxes = await _uow.CashBoxes.AnyAsync(cb => cb.AccountId == id, ct);
            if (hasCashBoxes)
            {
                _logger.LogWarning(
                    "Permanent delete refused for Account {Id}: الحساب مرتبط بصناديق نقدية",
                    id);
                return Result.Failure(
                    "لا يمكن الحذف النهائي: الحساب مرتبط بصناديق نقدية",
                    ErrorCodes.ReferencedByOtherEntities);
            }

            var hasSystemMappings = await _uow.SystemAccountMappings.AnyAsync(m => m.AccountId == id, ct);
            if (hasSystemMappings)
            {
                _logger.LogWarning(
                    "Permanent delete refused for Account {Id}: الحساب مرتبط بتعيينات النظام",
                    id);
                return Result.Failure(
                    "لا يمكن الحذف النهائي: الحساب مرتبط بتعيينات النظام",
                    ErrorCodes.ReferencedByOtherEntities);
            }

            _uow.Accounts.DeleteRange(new[] { account });
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Account {Id} permanently deleted", id);
            return Result.Success();
        }
        catch (Exception ex) when (ex is System.InvalidOperationException || IsDbUpdateException(ex))
        {
            _logger.LogError(ex, "Cannot permanently delete Account {Id}: {Message}", id, ex.InnerException?.Message ?? ex.Message);
            return Result.Failure("لا يمكن حذف هذا الحساب لأنه مرتبط بمعاملات أخرى", ErrorCodes.ReferencedByOtherEntities);
        }
    }

    // ─── Mapping & lookups (single query per call — see §4.1.1: nothing here is stored) ───

    /// <summary>Loaded chart snapshot: every account by id + the debit/credit totals per account id.</summary>
    private sealed record AccountLookups(
        IReadOnlyDictionary<int, Account> ById,
        IReadOnlyDictionary<int, (decimal Debit, decimal Credit)> Balances);

    private async Task<AccountLookups> LoadLookupsAsync(bool includeBalances, bool includeInactive, CancellationToken ct)
    {
        var accounts = includeInactive
            ? await _uow.Accounts.ToListIgnoreFiltersAsync(ct)
            : await _uow.Accounts.ToListAsync(ct);
        var byId = accounts.ToDictionary(a => a.Id);

        var balances = includeBalances
            ? await LoadBalancesAsync(ct)
            : new Dictionary<int, (decimal Debit, decimal Credit)>();

        return new AccountLookups(byId, balances);
    }

    /// <summary>
    /// ONE grouped query per call: sums Debit/Credit per account from <b>posted</b> JournalEntryLines only.
    /// Draft and cancelled entries are excluded — only <see cref="JournalEntryStatus.Posted"/> counts
    /// toward the account balance (§4.1.1). The balance is never stored on the account row.
    /// </summary>
    private async Task<Dictionary<int, (decimal Debit, decimal Credit)>> LoadBalancesAsync(CancellationToken ct)
    {
        var lines = await _uow.JournalEntryLines.ToListAsync(
            l => l.JournalEntry!.Status == JournalEntryStatus.Posted,
            ct: ct);
        return lines
            .GroupBy(l => l.AccountId)
            .ToDictionary(
                g => g.Key,
                g => (Debit: g.Sum(l => l.Debit), Credit: g.Sum(l => l.Credit)));
    }

    /// <summary>Builds the tree path from the loaded set only — never from the database column (there is none).</summary>
    private static string BuildTreePath(Account account, IReadOnlyDictionary<int, Account> byId)
    {
        var codes = new List<string>();
        Account? current = account;
        for (var depth = 0; current != null && depth < 10; depth++)
        {
            codes.Insert(0, current.AccountCode);
            current = current.ParentId.HasValue && byId.TryGetValue(current.ParentId.Value, out var parent)
                ? parent
                : null;
        }
        return "/" + string.Join("/", codes) + "/";
    }

    private static AccountDto MapToDto(Account a, AccountLookups lookups)
    {
        lookups.ById.TryGetValue(a.ParentId ?? 0, out var parent);
        lookups.Balances.TryGetValue(a.Id, out var totals);

        // Balance is signed by the effective normal side: debit-normal ⇒ Dr−Cr · credit-normal ⇒ Cr−Dr
        var effectiveNormalBalance = a.EffectiveNormalBalance;
        var balance = effectiveNormalBalance == 1
            ? totals.Debit - totals.Credit
            : totals.Credit - totals.Debit;

        var category = AccountCategoryHelper.Detect(a.AccountCode, a.NameAr);

        return new AccountDto(
            a.Id,
            a.AccountCode,
            a.NameAr,
            a.NameEn,
            a.Nature,
            a.IsLeaf,
            a.ParentId,
            parent?.NameAr,
            a.IsSystem,
            a.IsActive,
            null,
            a.Level,
            a.Description,
            a.ColorCode,
            a.Notes,
            ParentAccountCode: parent?.AccountCode,
            TreePath: BuildTreePath(a, lookups.ById),
            FinancialStatement: a.FinancialStatement,
            NormalBalance: a.NormalBalance ?? 0,
            AllowCostCenter: a.AllowCostCenter,
            Balance: balance,
            CategoryLabel: category.Label);
    }

    /// <summary>
    /// Checks if the exception is a database update exception by type name.
    /// Avoids direct dependency on EF Core in the Application layer.
    /// </summary>
    private static bool IsDbUpdateException(Exception ex)
    {
        var typeName = ex.GetType().FullName ?? "";
        return typeName.Contains("DbUpdateException", StringComparison.Ordinal);
    }
}
