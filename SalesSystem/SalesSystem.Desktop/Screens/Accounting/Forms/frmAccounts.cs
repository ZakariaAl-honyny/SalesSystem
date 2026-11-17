// -----------------------------------------------------------------------------
// frmAccounts.cs — دليل الحسابات (شجرة يسار + محرّر يمين + شبكة أسفل)
//
// • الشكل (RULE-N135…N138 + الشاشة المرجعية frmAddUpdateSalesInvoice):
//     «شريط العنوان» ← «أوامر الحسابات» ← «بحث وتصفية» ← [شجرة | محرّر] ←
//     «بيانات الحسابات» ← «الإجماليات» — كل كتلة صندوق واحد بعنوان، ولا صندوق داخل صندوق.
//
// • العناصر الثلاثة القابلة للتضمين (RULE-N169 — لا نسخة ثانية من منطقها هنا):
//     ctrlTreeOfAccounts   عرض هرمي + بحث + اختيار سريع (ShowCrudActions = false:
//                          العمليات كلها من شريط الشاشة، RULE-N100).
//     ctrlAccountEditor    بيانات الحساب + معاينة الكود + الحقول الهرمية + التحقق.
//     ctrlCrudToolbar      الأزرار الملوّنة وآلة حالتها.
//   فلا نسخة ثانية من: حقول المحرر · خيارات الأنواع · حكم «مطلوب» · تلوين الأزرار.
//
// • آلة الحالة واحدة `_ApplyModeState()` (النظير RULE-N150): هي **المكان الوحيد** الذي
//   يُغذّي الشريط المشترك (SetMode/HasSelection/AllowDelete) ويضبط Enabled لأزرار الشاشة.
//
// • الوضع: Browsing (فتح/بعد الحفظ/إلغاء الإضافة) · Adding («+ إضافة») · Editing (اختيار
//   حساب من الشجرة أو الشبكة). المحرّر يعرض الوضع على حقوله بنفسه (§2.36).
//
// • الاختصارات: F2 = إضافة · F3 = تعديل المحدد · F5 = تحديث كامل · Delete = تعطيل ·
//   Esc = مسح التصفية (داخل صندوقها) / إلغاء الإضافة-التعديل / إغلاق (RULE-N47).
//   ولا زر أساسي للنموذج (AcceptButton = null): أزرار الشريط المشترك لا تُنقَر بـEnter
//   أبداً — فلا حفظ صامت من داخل حقول المحرّر أو شبكة السطور (RULE-N153/RULE-N193).
//
// • RULE-N205: `catch` واحد لكل حدّ فعلي — معالجات `async void` عبر `_RunSafeAsync`،
//   ومعالج الحذف النهائي بنفسه، و`finally` تُعيد المؤشر والعلم دائماً.
// -----------------------------------------------------------------------------

using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Screens.Accounting.Controls;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Forms;

/// <summary>
/// شاشة دليل الحسابات: شجرة هرمية + محرّر حساب + شبكة عرض/تصفية، وشريط أوامر موحّد.
/// <para>
/// كل القراءات والكتابات تمرّ عبر <see cref="IAccountDesktopService"/> من حزمة الخدمات
/// المحقونة (RULE-N05/N06) — لا DbContext ولا HTTP ولا أي طبقة أدنى.
/// </para>
/// </summary>
public partial class frmAccounts : Form
{
    // ── رسائل عربية (RULE-N28) ────────────────────────────────────────────────
    private const string LoadFailedMessage = "تعذر تحميل دليل الحسابات. يرجى إعادة المحاولة.";
    private const string LoadUnexpectedMessage = "حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.";
    private const string RefreshFailedMessage = "تعذر تحديث البيانات. يرجى إعادة المحاولة.";
    private const string SaveFailedMessage = "حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.";
    private const string DeleteFailedMessage = "حدث خطأ أثناء التعطيل. يرجى إعادة المحاولة.";
    private const string PermanentDeleteFailedMessage = "حدث خطأ أثناء الحذف النهائي. يرجى إعادة المحاولة.";
    private const string SelectFirstMessage = "يرجى اختيار حساب من الشجرة أو الشبكة أولاً.";
    private const string SystemAccountMessage =
        "هذا حساب نظامي — لا يمكن تعطيله أو تعديله أو حذفه من هذه الشاشة.";

    private readonly IServiceProvider _serviceProvider = null!;

    private IAccountDesktopService AccountService => _serviceProvider.GetRequiredService<IAccountDesktopService>();

    // ── العناصر القابلة للتضمين (معلنة في المصمّم — RULE-N145) ─────────────────
    private ctrlCrudToolbar Toolbar => ctrlCrudToolbar1;

    private ctrlTreeOfAccounts Tree => ctrlTreeOfAccounts1;

    private ctrlAccountEditor Editor => ctrlAccountEditor1;

    // ── State ────────────────────────────────────────────────────────────────
    private readonly List<AccountDto> _accounts = new();
    private readonly Dictionary<int, AccountDto> _accountsById = new();

    private DataView _accountsView = new();
    private int _totalCount;

    /// <summary>الحساب المعروض/المعدَّل (<c>null</c> = لا سجل محدد).</summary>
    private int? _editId;

    /// <summary>وضع الشاشة الوحيد: عرض · إضافة · تعديل (§2.36).</summary>
    private CrudMode _mode = CrudMode.Browsing;

    /// <summary>عملية جارية (حفظ/تعطيل/تحديث) — يُعطّل كل ما قد يُطلق عملية أخرى.</summary>
    private bool _actionButtonsBusy;

    /// <summary>مزامنة تحديد (شبكة ↔ شجرة ↔ محرّر): تُكتم أحداث التحديد العابرة.</summary>
    private bool _syncingSelection;

    /// <summary>خيار نوع الحساب في التصفية — الربط بالقيمة لا بالنص العربي (RULE-N60).</summary>
    private sealed record NatureOption(byte Value, string Text)
    {
        public override string ToString() => Text;
    }

    /// <summary>منشئ المصمم — لمصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmAccounts()
    {
        InitializeComponent();
    }

    public frmAccounts(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // RULE-N40…RULE-N47 — لوحة المفاتيح على حقول الشاشة الباقية (التصفية) وعلى ما
        // لم توصّله مكوّناتها بنفسها: الحقول التي وصّلها المحرّر/الشجرة مسبقاً تُتجاهل
        // بحارس التوصيل نفسه في KeyboardHelper (MarkWired) فلا توصيل مزدوج.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                        // RULE-N45
        KeyboardHelper.WireGlobalShortcuts(this,                                                        // RULE-N47
            (Keys.F2, () => _StartNewEntry(Tree.SelectedAccountId)),
            (Keys.F3, _StartEditFromSelection),
            (Keys.F5, () => _ = _RunSafeAsync(_RefreshAsync, RefreshFailedMessage)),
            (Keys.Delete, _HandleDeleteShortcut),
            (Keys.Escape, _HandleEscape));

        // RULE-N147: لا زر أساسي للنموذج — أزرار الوضع تعيش في الشريط المشترك الذي لا
        // يُعرِّض زرّه (خارج نطاق هذه الموجة)، و`_PrimaryActionButton` تُرجع null فيبقى
        // AcceptButton فارغاً: Enter لا يحفظ ولا يُرحّل من أي مكان (RULE-N153/RULE-N193).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        _LoadFilterOptions();
        _SetupToolTips();

        ApplyUiTheme();

        // RULE-N43: يُوصَّل بعد الثيم حتى يلتقط ألوان الأزرار النهائية.
        KeyboardHelper.WireButtonDisabledVisual(btnPermanentDelete, btnClearFilters);

        // ── المكوّنات القابلة للتضمين ─────────────────────────────────────────
        Tree.Attach(_serviceProvider);    // + ShowCrudActions = false من المصمّم
        Editor.Attach(_serviceProvider);  // قائمة الآباء + معاينة الكود

        Tree.AccountSelected += Tree_AccountSelected;
        Tree.AddChildRequested += Tree_AddChildRequested;
        Tree.EditRequested += Tree_EditRequested;

        Toolbar.AddRequested += Toolbar_AddRequested;
        Toolbar.EditRequested += Toolbar_EditRequested;
        Toolbar.SaveRequested += _OnSaveRequested;
        Toolbar.CancelEditRequested += _OnCancelRequested;
        Toolbar.DeleteRequested += Toolbar_DeleteRequested;
        Toolbar.RefreshRequested += Toolbar_RefreshRequested;
        Toolbar.CloseRequested += Toolbar_CloseRequested;

        // الزرّان الداخليان للمحرّر يرفعان حدثين فقط: نفس مساري «حفظ» و«إلغاء التعديل»
        // (معالج واحد لكل مسار — فلا يبقى زر مرئي في المحرّر بلا أثر ولا منطق ثانٍ).
        Editor.SaveRequested += _OnSaveRequested;
        Editor.CancelRequested += _OnCancelRequested;

        FilterPanelHelper.Attach(this, _serviceProvider);

        _ApplyModeState();   // فتح الشاشة = وضع العرض (§2.36)
    }

    // ── Theme (RULE-N50…RULE-N54) ────────────────────────────────────────────

    /// <summary>الثيم الأخضر — ينتهي بـ <c>GreenThemeHelper.Apply(this)</c> كآخر سطر (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this); // يفرض أبيض/Segoe UI ⇒ يُمحى لاحقاً

        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvAccounts);

        // RULE-N22/RULE-N52: سلوك الشبكة تعلنه الشاشة (ApplyGrid لا يلمسه).
        dgvAccounts.ReadOnly = true;
        dgvAccounts.AllowUserToAddRows = false;
        dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAccounts.MultiSelect = false;

        // طبقة «لا توجد بيانات» وسطر الملخص: النص المميّز من البيت الواحد.
        lblEmptyState.BackColor = Color.White;
        lblEmptyState.ForeColor = GreenThemeHelper.TextStrong;
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── Options / ToolTips ───────────────────────────────────────────────────

    /// <summary>خيارات التصفية فقط — خيارات المحرّر ملك عنصر المحرّر (RULE-N169).</summary>
    private void _LoadFilterOptions()
    {
        cbNatureFilter.Items.Clear();
        cbNatureFilter.Items.Add(new NatureOption(0, "كل الأنواع"));
        cbNatureFilter.Items.Add(new NatureOption(1, "أصول"));
        cbNatureFilter.Items.Add(new NatureOption(2, "خصوم"));
        cbNatureFilter.Items.Add(new NatureOption(3, "حقوق ملكية"));
        cbNatureFilter.Items.Add(new NatureOption(4, "إيرادات"));
        cbNatureFilter.Items.Add(new NatureOption(5, "مصروفات"));
        cbNatureFilter.SelectedIndex = 0;

        cbFilterBy.Items.Clear();
        cbFilterBy.Items.Add("الكود");
        cbFilterBy.Items.Add("الاسم العربي");
        cbFilterBy.Items.Add("الاسم الإنجليزي");
        cbFilterBy.SelectedIndex = 0;
    }

    /// <summary>RULE-N47: كل زر/أداة تحمل تلميحها — والتلميح المكرّر لا يُكتب مرتين.</summary>
    private void _SetupToolTips()
    {
        toolTip.SetToolTip(cbFilterBy, "عمود البحث: الكود أو الاسم العربي أو الاسم الإنجليزي");
        toolTip.SetToolTip(cbNatureFilter, "تصفية بنوع الحساب (أصول · خصوم · حقوق ملكية · إيرادات · مصروفات)");
        toolTip.SetToolTip(dgvAccounts, "نقر مزدوج = فتح الحساب في المحرّر للتعديل");
    }

    // ── Load ─────────────────────────────────────────────────────────────────

    private async void frmAccounts_Load(object? sender, EventArgs e)
        => await _RunSafeAsync(async () =>
        {
            await _LoadAsync();
            _ResetForm();
        }, LoadUnexpectedMessage);

    /// <summary>
    /// «تحديث» (F5) — تحديث **كامل** (RULE-N148): تُمسح أدوات التصفية أولاً، ثم تُعاد قراءة
    /// الحسابات (ومنها خيارات المحرّر والشجرة) وتُعاد الشبكة والملخص، ويُعاد التحديد
    /// والمحرّر إلى الحالة الافتراضية — ولا يقتصر على مسح التصفية.
    /// </summary>
    private async Task _RefreshAsync()
    {
        if (_actionButtonsBusy) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();
            _editId = null;
            _mode = CrudMode.Browsing;

            await _LoadAsync();
            _ResetForm();
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ─────────────────────────────────────────────────────────

    /// <summary>
    /// يحمّل كل الحسابات (بما فيها غير النشطة) ثم يبني جدول العرض ويطبّق التصفية،
    /// ويُغذّي عنصري المحرّر والشجرة من نفس القراءة (لا قراءة ثانية للشاشة).
    /// </summary>
    private async Task _LoadAsync()
    {
        var result = await AccountService.GetAllAsync(includeInactive: true);

        if (!result.IsSuccess || result.Value is null)
        {
            clsDialogs.Error(result.Error ?? LoadFailedMessage);
            return;
        }

        _accounts.Clear();
        _accountsById.Clear();

        foreach (var account in result.Value)
        {
            _accounts.Add(account);
            _accountsById[account.Id] = account;
        }

        var table = _BuildGridTable();
        _totalCount = table.Rows.Count;

        _syncingSelection = true;
        try
        {
            _accountsView = new DataView(table);
            dgvAccounts.DataSource = _accountsView;
        }
        finally
        {
            _syncingSelection = false;
        }

        // الدلائل: آباء المحرّر (واستثناء فروعه) + الشجرة — تحت حارس واحد حتى لا يُشعل
        // تحديث الشجرة حدث اختيار يفتح المحرّر أثناء التحميل.
        var wasSyncing = _syncingSelection;
        _syncingSelection = true;
        try
        {
            Editor.BindParents(_accounts, _editId);
            await Tree.RefreshAsync();
        }
        finally
        {
            _syncingSelection = wasSyncing;
        }

        _ApplyFilter();

        if (_editId is int selectedId && _accountsById.ContainsKey(selectedId))
        {
            _SelectGridRow(selectedId);
            _SelectTreeRow(selectedId);
        }
        else
        {
            _editId = null;
            _ClearGridSelection();
        }

        _UpdateModeHeader();
        _ApplyModeState();
    }

    /// <summary>يبني جدول عرض الشاشة: أعمدة الشبكة الـ13 + أعمدة الحكم/التصفية — مصدر واحد.</summary>
    private DataTable _BuildGridTable()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("AccountCode", typeof(string));
        table.Columns.Add("NameAr", typeof(string));
        table.Columns.Add("NameArIndented", typeof(string));
        table.Columns.Add("NameEn", typeof(string));
        table.Columns.Add("NatureDisplay", typeof(string));
        table.Columns.Add("LevelDisplay", typeof(string));
        table.Columns.Add("ParentAccountName", typeof(string));
        table.Columns.Add("IsLeaf", typeof(bool));
        table.Columns.Add("IsSystem", typeof(bool));
        table.Columns.Add("IsActive", typeof(bool));
        table.Columns.Add("IsLeafText", typeof(string));
        table.Columns.Add("IsSystemText", typeof(string));
        table.Columns.Add("Status", typeof(string));
        table.Columns.Add("NatureValue", typeof(byte));
        table.Columns.Add("TreePath", typeof(string));            // مشتق في الـDTO — لا يُخزَّن
        table.Columns.Add("Balance", typeof(string));             // مُنسَّق بـ clsFormat.Money (البيت الواحد)
        table.Columns.Add("FinancialStatement", typeof(string));  // عربي جاهز من الـ DTO
        table.Columns.Add("Category", typeof(string));            // التصنيف أو «حساب مالي عام»

        foreach (var account in _accounts.OrderBy(a => a.AccountCode, StringComparer.Ordinal))
        {
            table.Rows.Add(
                account.Id,
                account.AccountCode,
                account.NameAr,
                _IndentedName(account),
                account.NameEn ?? string.Empty,
                account.NatureDisplay,      // عربي جاهز من الـ DTO — لا نُعيد تعيينه
                account.LevelDisplay,       // عربي جاهز من الـ DTO
                account.ParentAccountName ?? string.Empty,
                account.IsLeaf,
                account.IsSystem,
                account.IsActive,
                account.IsLeaf ? "نعم" : "لا",
                account.IsSystem ? "نعم" : "لا",
                account.IsActive ? "نشط" : "معطّل",
                account.Nature,
                account.TreePath,
                clsFormat.Money(account.Balance),   // RULE-N169: التنسيق من البيت الواحد لا من الشاشة
                account.FinancialStatementDisplay,  // عربي جاهز من الـ DTO
                account.CategoryLabel ?? "حساب مالي عام");
        }

        return table;
    }

    /// <summary>إبراز التسلسل الهرمي في عمود الاسم العربي دون TreeView (RULE: عرض هرمي).</summary>
    private static string _IndentedName(AccountDto account)
        => account.Level > 1
            ? new string(' ', (account.Level - 1) * 2) + "└ " + account.NameAr
            : account.NameAr;

    // ── آلة الحالة الوحيدة (§2.36 / نظير RULE-N150) ───────────────────────────

    /// <summary>
    /// المكان **الوحيد** الذي يُغذّي الشريط المشترك ويضبط تفعيل أزرار الشاشة
    /// (حذف نهائي · مسح التصفية · بنود قائمة السياق) ويُزامن وضع المحرّر.
    /// تُستدعى بعد كل انتقال: تحميل · إضافة · اختيار · حفظ · تعطيل · تحديث · تصفية.
    /// </summary>
    private void _ApplyModeState()
    {
        bool busy = _actionButtonsBusy;
        bool hasSelection = _editId is int selectedId && _accountsById.ContainsKey(selectedId);
        bool systemAccount = hasSelection && _accountsById[_editId!.Value].IsSystem;

        // زامن وضع المحرّر مع وضع الشاشة: نفس المدخل الوحيد (idempotent — لا حالة ثانية).
        Editor.SetMode(_mode);

        // الشريط المشترك: ثلاثة مدخلات فقط ويحكم هو على أزراره (RULE-N169).
        Toolbar.SetMode(_mode);
        Toolbar.HasSelection = hasSelection;
        Toolbar.AllowDelete = hasSelection && !systemAccount && !busy;
        Toolbar.ShowSave = true;
        Toolbar.ShowClose = true;

        // الصف الثانوي الخاص بالشاشة + قائمة سياق الشبكة: نفس المدخلات فلا حكم ثانٍ.
        btnPermanentDelete.Enabled = hasSelection && !systemAccount && !busy;
        cmsEdit.Enabled = hasSelection && !busy;
        cmsDelete.Enabled = hasSelection && !systemAccount && !busy;

        // RULE-N148: «مسح التصفية» مفعّل فقط عند وجود تصفية تختلف عن الافتراضي.
        btnClearFilters.Enabled = _HasActiveFilter() && !busy;

        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>
    /// الزر الأساسي لـ <c>Enter</c> (RULE-N147) — <c>null</c> بقصد: أزرار الوضع تعيش في
    /// <see cref="ctrlCrudToolbar"/> الذي لا يُعرِّض زرّه، وبقاء <c>AcceptButton</c> فارغاً
    /// يضمن أن <c>Enter</c> لا يحفظ ولا يُرحّل من أي حقل أو شبكة (RULE-N153/RULE-N193).
    /// والانتقال بين حقول المحرّر بـ<c>Enter</c> يملكه المحرّر نفسه (RULE-N191).
    /// </summary>
    private Button? _PrimaryActionButton() => null;

    // ── Transitions — كل انتقال يمرّ من هنا ───────────────────────────────────

    /// <summary>
    /// «جديد» / F2 / «إضافة فرعي»: يفتح المحرّر للإضافة مع الأب المطلوب (أو المحدد في الشجرة).
    /// </summary>
    private void _StartNewEntry(int? parentId)
    {
        _editId = null;
        _mode = CrudMode.Adding;

        _ClearGridSelection();

        Editor.SetMode(CrudMode.Adding);
        Editor.ResetEditor(parentId);

        _UpdateModeHeader();
        _ApplyModeState();
    }

    /// <summary>
    /// انتقال إلى «تعديل»/«عرض» السجل — المصدر الوحيد للتحديد من الشجرة **و** الشبكة.
    /// </summary>
    private void _OnRecordSelected(int accountId)
    {
        if (_syncingSelection) return;
        if (!_accountsById.TryGetValue(accountId, out var account)) return;

        _syncingSelection = true;
        try
        {
            _editId = account.Id;
            _mode = CrudMode.Editing;

            // المحرّر: بيانات السجل ثم الوضع (الترتيب يهمّ: إعادة الضبط تركّز حقلاً في وضع العرض).
            Editor.LoadAccount(account);
            Editor.SetMode(CrudMode.Editing);

            _SelectGridRow(account.Id);
            _SelectTreeRow(account.Id);
        }
        finally
        {
            _syncingSelection = false;
        }

        _UpdateModeHeader();
        _ApplyModeState();
    }

    /// <summary>
    /// إعادة الشاشة إلى **وضع العرض**: بلا تحديد، أو مع السجل المطلوب (بعد الحفظ/الإلغاء).
    /// </summary>
    private void _ResetForm(int? selectId = null)
    {
        _editId = selectId;
        _mode = CrudMode.Browsing;

        if (selectId is int id)
        {
            _SelectGridRow(id);
            _SelectTreeRow(id);
        }
        else
        {
            _ClearGridSelection();
        }

        Editor.SetMode(CrudMode.Browsing);
        Editor.LoadAccount(selectId is int selected && _accountsById.TryGetValue(selected, out var account) ? account : null);

        _UpdateModeHeader();
        _ApplyModeState();
    }

    /// <summary>RULE-N44: مؤشر الوضع أعلى الشاشة — يتحدث مع كل تغيير حالة.</summary>
    private void _UpdateModeHeader()
    {
        if (_mode == CrudMode.Adding)
        {
            // نفس أهلية خيارات «الحساب الأب» في المحرّر: تُذكر صلة الأب فقط إن كان الحساب
            // المحدد في الشجرة مؤهَّلاً فعلاً (غير ورقي ونشط) — وإلا فالإضافة إلى الجذر.
            lblMode.Text = Tree.SelectedAccountId is int parentId
                           && _accountsById.TryGetValue(parentId, out var parent)
                           && !parent.IsLeaf
                           && parent.IsActive
                ? $"دليل الحسابات — إضافة حساب فرعي تحت: {parent.NameAr}"
                : "دليل الحسابات — إضافة حساب جديد";
            return;
        }

        if (_editId is int id && _accountsById.TryGetValue(id, out var account))
        {
            lblMode.Text = account.IsSystem
                ? $"دليل الحسابات — حساب نظامي (عرض فقط): {account.NameAr}"
                : _mode == CrudMode.Editing
                    ? $"دليل الحسابات — تعديل الحساب: {account.NameAr}"
                    : $"دليل الحسابات — وضع العرض: {account.NameAr}";
            return;
        }

        lblMode.Text = "دليل الحسابات — وضع العرض";
    }

    // ── Grid ─────────────────────────────────────────────────────────────────

    private void dgvAccounts_SelectionChanged(object? sender, EventArgs e)
    {
        if (_syncingSelection) return;
        if (_SelectedAccountId() is not int id) return;
        if (_editId == id && _mode == CrudMode.Editing) return;

        _OnRecordSelected(id);
    }

    private void dgvAccounts_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        if (_SelectedAccountId() is int id)
        {
            _OnRecordSelected(id);
            Editor.Focus();   // نقر مزدوج = تعديل مباشر (RULE §2.21 #14)
        }
    }

    private void dgvAccounts_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgvAccounts.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;

        // تصفير صريح قبل الحكم: الخلية قد تكون معاد استخدامها بعد إعادة الربط فيبقى لون حالة سابق
        e.CellStyle.BackColor = Color.Empty;
        e.CellStyle.ForeColor = Color.Empty;
        e.CellStyle.SelectionForeColor = Color.Empty;

        if (row["IsActive"] is bool isActive && !isActive)
        {
            e.CellStyle.BackColor = GreenThemeHelper.StatusDangerBack;
            e.CellStyle.ForeColor = GreenThemeHelper.StatusDanger;
            e.CellStyle.SelectionForeColor = GreenThemeHelper.StatusDanger;
        }
    }

    private int? _SelectedAccountId()
        => dgvAccounts.CurrentRow?.DataBoundItem is DataRowView row && row["Id"] is int id ? id : null;

    private void _SelectGridRow(int accountId)
    {
        foreach (DataGridViewRow row in dgvAccounts.Rows)
        {
            if (row.DataBoundItem is not DataRowView view || view["Id"] is not int rowId || rowId != accountId)
                continue;

            var wasSyncing = _syncingSelection;
            _syncingSelection = true;
            try
            {
                row.Selected = true;
                dgvAccounts.CurrentCell = row.Cells["colNameAr"];
            }
            finally
            {
                _syncingSelection = wasSyncing;
            }

            break;
        }
    }

    private void _ClearGridSelection()
    {
        var wasSyncing = _syncingSelection;
        _syncingSelection = true;
        try
        {
            dgvAccounts.ClearSelection();
        }
        finally
        {
            _syncingSelection = wasSyncing;
        }
    }

    /// <summary>تحديد العقدة في الشجرة بلا إشعال حدث الاختيار (يمنع دورة شبكة ↔ شجرة).</summary>
    private void _SelectTreeRow(int accountId)
    {
        var wasSyncing = _syncingSelection;
        _syncingSelection = true;
        try
        {
            Tree.SelectAccount(accountId);
        }
        finally
        {
            _syncingSelection = wasSyncing;
        }
    }

    // ── Filter (RULE-N56…RULE-N63) — تصفية فورية بلا زر بحث ─────────────────

    private void txtFilterValue_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbNatureFilter_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkShowInactive_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyLeaf_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlySystem_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyParents_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void txtFilterValue_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Escape) return;

        _ClearAllFilters();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للمحرر (RULE-N148). «تحديث» هو المسؤول عن إعادة التحميل.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>هل توجد تصفية تختلف عن الافتراضي؟ (RULE-N148 — حكم واحد لزر «مسح التصفية»)</summary>
    private bool _HasActiveFilter()
        => !string.IsNullOrWhiteSpace(txtFilterValue.Text)
           || cbFilterBy.SelectedIndex > 0
           || cbNatureFilter.SelectedIndex > 0
           || chkShowInactive.Checked
           || chkOnlyLeaf.Checked
           || chkOnlySystem.Checked
           || chkOnlyParents.Checked;

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح» و«تحديث».</summary>
    private void _ClearFilterInputs()
    {
        var wasSyncing = _syncingSelection;
        _syncingSelection = true; // التصفية لا تلمس المحرر: نمنع مزامنة التحديد أثناء المسح
        try
        {
            txtFilterValue.Clear();

            if (cbFilterBy.Items.Count > 0) cbFilterBy.SelectedIndex = 0;       // «الكود»
            if (cbNatureFilter.Items.Count > 0) cbNatureFilter.SelectedIndex = 0; // «كل الأنواع»

            chkShowInactive.Checked = false;
            chkOnlyLeaf.Checked = false;
            chkOnlySystem.Checked = false;
            chkOnlyParents.Checked = false;
        }
        finally
        {
            _syncingSelection = wasSyncing;
        }
    }

    /// <summary>يمسح كل أدوات التصفية ثم يعيد تطبيق التصفية على نفس البيانات (بلا إعادة تحميل).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
    }

    private void _ApplyFilter()
    {
        if (_accountsView.Table is null)
        {
            _UpdateSummary();
            _UpdateEmptyState();
            _ApplyModeState();
            return;
        }

        var conditions = new List<string>();

        if (!chkShowInactive.Checked) conditions.Add("IsActive = true");
        if (chkOnlyLeaf.Checked) conditions.Add("IsLeaf = true");
        if (chkOnlyParents.Checked) conditions.Add("IsLeaf = false");
        if (chkOnlySystem.Checked) conditions.Add("IsSystem = true");

        if (cbNatureFilter.SelectedItem is NatureOption { Value: > 0 } nature)
            conditions.Add($"NatureValue = {nature.Value}");

        // RULE-N60: العمود يُحدَّد بالـ SelectedIndex لا بمقارنة نص عربي،
        // والقيمة تُهرَّب في RowFilterHelper لا داخل الشاشة (RULE-N112).
        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            string column = cbFilterBy.SelectedIndex switch
            {
                1 => "NameAr",
                2 => "NameEn",
                _ => "AccountCode"
            };

            conditions.Add(RowFilterHelper.Like(column, text));
        }

        var wasSyncing = _syncingSelection;
        _syncingSelection = true; // تغيير التصفية لا ينقل المحرر إلى سطر آخر
        try
        {
            _accountsView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : string.Empty;
        }
        finally
        {
            _syncingSelection = wasSyncing;
        }

        // RULE-N150: التصفية قد تُخفي السجل المعروض — فلا يبقى «تعديل/تعطيل» فعّالاً على
        // سجل غير معروض، ولا يبقى المحرّر على سجل خارج الشبكة.
        if (_editId is int editedId && !_IsRowVisible(editedId))
            _ResetForm();

        _UpdateSummary();
        _UpdateEmptyState();
        _ApplyModeState();
    }

    /// <summary>هل السجل <paramref name="accountId"/> معروض في الشبكة بعد التصفية الحالية؟</summary>
    private bool _IsRowVisible(int accountId)
    {
        foreach (DataGridViewRow row in dgvAccounts.Rows)
        {
            if (row.DataBoundItem is DataRowView view && view["Id"] is int rowId && rowId == accountId)
                return true;
        }

        return false;
    }

    /// <summary>RULE-N61: شريط الملخص — عدد المعروض من الإجمالي + توزيع الأنواع.</summary>
    private void _UpdateSummary()
    {
        int shown = _accountsView.Table is null ? 0 : _accountsView.Count;

        int assets = 0, liabilities = 0, equity = 0, revenue = 0, expenses = 0;

        if (_accountsView.Table is not null)
        {
            foreach (DataRowView row in _accountsView)
            {
                switch (row["NatureValue"] is byte nature ? nature : (byte)0)
                {
                    case 1: assets++; break;
                    case 2: liabilities++; break;
                    case 3: equity++; break;
                    case 4: revenue++; break;
                    case 5: expenses++; break;
                }
            }
        }

        // الأعداد تُنسَّق من البيت الواحد clsFormat.Number — لا إدراج خام في النص (RULE-N169)
        lblSummary.Text =
            $"المعروض: {clsFormat.Number(shown)} من {clsFormat.Number(_totalCount)} — "
            + $"أصول: {clsFormat.Number(assets)} · خصوم: {clsFormat.Number(liabilities)} · "
            + $"حقوق ملكية: {clsFormat.Number(equity)} · إيرادات: {clsFormat.Number(revenue)} · "
            + $"مصروفات: {clsFormat.Number(expenses)}";
    }

    /// <summary>عند عدم وجود نتائج تُعرض طبقة «لا توجد بيانات» مكان جسم الشبكة.</summary>
    private void _UpdateEmptyState()
    {
        bool isEmpty = _accountsView.Table is null || _accountsView.Count == 0;

        lblEmptyState.Visible = isEmpty;
        if (isEmpty) lblEmptyState.BringToFront();
    }

    // ── Toolbar events — الشريط المشترك يرفع، والشاشة تنفّذ ───────────────────

    private void Toolbar_AddRequested(object? sender, EventArgs e) => _StartNewEntry(Tree.SelectedAccountId);

    /// <summary>«تعديل» — يُفعّل عند وجود سجل محدد في وضع العرض فقط.</summary>
    private void Toolbar_EditRequested(object? sender, EventArgs e) => _StartEditFromSelection();

    /// <summary>«حفظ» — من الشريط المشترك ومن الزر الداخلي للمحرّر (مسار واحد).</summary>
    private async void _OnSaveRequested(object? sender, EventArgs e)
        => await _RunSafeAsync(_SaveAsync, SaveFailedMessage);

    /// <summary>«إلغاء التعديل» — من الشريط المشترك ومن الزر الداخلي للمحرّر (مسار واحد).</summary>
    private void _OnCancelRequested(object? sender, EventArgs e) => _ResetForm(_editId);

    private async void Toolbar_DeleteRequested(object? sender, EventArgs e)
        => await _RunSafeAsync(_DeleteAsync, DeleteFailedMessage);

    private async void Toolbar_RefreshRequested(object? sender, EventArgs e)
        => await _RunSafeAsync(_RefreshAsync, RefreshFailedMessage);

    private void Toolbar_CloseRequested(object? sender, EventArgs e) => Close();

    /// <summary>F3 / «تعديل»: ينتقل إلى وضع التعديل للسجل المحدد.</summary>
    private void _StartEditFromSelection()
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning(SelectFirstMessage);
            return;
        }

        _OnRecordSelected(id);

        if (_editId == id && _mode == CrudMode.Editing)
        {
            // المحرّر أُعيد تحميله في وضع التعديل: تُركّز أولى حقوله للكتابة الفورية.
            Editor.Focus();
        }
    }

    // ── Tree events — العمليات كلها من الشريط (RULE-N100) ─────────────────────

    private void Tree_AccountSelected(object? sender, AccountTreeNodeDto? node)
    {
        if (_syncingSelection || node is null) return;

        _OnRecordSelected(node.Id);
    }

    /// <summary>
    /// «إضافة فرعي» من الشجرة — الحدث موصول كما يقتضي العقد، ولا يُطلق الآن لأن
    /// <c>ShowCrudActions = false</c> (الشاشة تملك العمليات كلها).
    /// </summary>
    private void Tree_AddChildRequested(object? sender, int parentId) => _StartNewEntry(parentId);

    /// <summary>«تعديل» من الشجرة — نفس المسار، ونفس السبب أعلاه.</summary>
    private void Tree_EditRequested(object? sender, int id) => _OnRecordSelected(id);

    // ── Save / Delete ────────────────────────────────────────────────────────

    /// <summary>
    /// حفظ الحساب: <c>Adding</c> ⇒ <c>CreateAsync</c>، و<c>Editing</c> ⇒ <c>UpdateAsync</c>.
    /// الطلب يُبنى في المحرّر (<c>TryBuildCreate/TryBuildUpdate</c>) — التحقق كله هناك،
    /// و<c>AccountCode</c> يولّده النظام فلا يُرسَل من الشاشة.
    /// </summary>
    private async Task _SaveAsync()
    {
        // حماية من النقر المزدوج: لا ندخل الحفظ مرتين قبل أن يُصفَّر العلم في finally.
        if (_actionButtonsBusy) return;

        bool isUpdate = _mode == CrudMode.Editing && _editId is not null;

        // الحارسان «|| x is null» لتدفّق النوع عند المُصرِّف فقط: TryBuild* لا يُعيد true
        // إلا بطلب صالح غير فارغ — فلا تغيير في السلوك.
        CreateAccountRequest? createRequest = null;
        UpdateAccountRequest? updateRequest = null;

        if (isUpdate)
        {
            if (!Editor.TryBuildUpdate(out var update) || update is null) return;   // الرسالة والحقل في المحرّر
            updateRequest = update;
        }
        else
        {
            if (!Editor.TryBuildCreate(out var create) || create is null) return;
            createRequest = create;
        }

        int? targetId = isUpdate ? _editId : null;

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _ApplyModeState();

        try
        {
            int? savedId;

            if (updateRequest is not null && targetId is int id)
            {
                var result = await AccountService.UpdateAsync(id, updateRequest);

                if (!result.IsSuccess)
                {
                    clsDialogs.Error($"لم يتم حفظ التعديل.\n{result.Error}");
                    return;
                }

                savedId = id;
            }
            else if (createRequest is not null)
            {
                var result = await AccountService.CreateAsync(createRequest);

                if (!result.IsSuccess)
                {
                    clsDialogs.Error($"لم يتم حفظ الحساب.\n{result.Error}");
                    return;
                }

                savedId = result.Value;
            }
            else
            {
                // لا طلب صالح: الحارسان أعلاه يمنعان الوصول إلى هنا — لا شيء يُحفظ.
                return;
            }

            clsDialogs.Success(updateRequest is not null ? "تم تعديل الحساب بنجاح." : "تمت إضافة الحساب بنجاح.");

            _editId = savedId;      // إعادة التحميل تُحدد السجل المحفوظ في الشبكة والشجرة
            await _LoadAsync();
            _ResetForm(savedId);    // وضع العرض مع بقاء السجل المحفوظ معروضاً
        }
        finally
        {
            _actionButtonsBusy = false;
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>تعطيل الحساب (حذف ناعم) — الحسابات النظامية يرفضها BLL وتُعرَض رسالته.</summary>
    private async Task _DeleteAsync()
    {
        if (_actionButtonsBusy) return;

        if (_editId is not int id || !_accountsById.TryGetValue(id, out var account))
        {
            clsDialogs.Warning(SelectFirstMessage);
            return;
        }

        if (account.IsSystem)
        {
            // الحساب النظامي محمي في الخدمة نفسها: لا تعديل ولا تعطيل ولا حذف نهائي.
            clsDialogs.Warning(SystemAccountMessage);
            return;
        }

        if (!clsDialogs.Confirm($"هل أنت متأكد من تعطيل الحساب \"{account.NameAr}\"؟\n" +
                                "لن يظهر في القوائم بعد التعطيل، ويمكن إظهاره بخيار «إظهار غير النشط»."))
        {
            return;
        }

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _ApplyModeState();

        try
        {
            var result = await AccountService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم تعطيل الحساب بنجاح.");

                _ResetForm();
                await _LoadAsync();
            }
            else
            {
                clsDialogs.Error($"لم يتم تعطيل الحساب.\n{result.Error}");
            }
        }
        finally
        {
            _actionButtonsBusy = false;
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// حذف نهائي — تحذير أقوى، ويبقى الزر معطّلاً للحسابات النظامية من آلة الحالة.
    /// كل رفض من BLL يُعرَض نصّه العربي كما هو.
    /// </summary>
    private async void btnPermanentDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id || !_accountsById.TryGetValue(id, out var account))
        {
            clsDialogs.Warning(SelectFirstMessage);
            return;
        }

        if (account.IsSystem)
        {
            clsDialogs.Warning("لا يمكن حذف حساب نظامي نهائياً.");
            return;
        }

        if (!clsDialogs.Confirm($"تحذير: سيتم حذف الحساب \"{account.NameAr}\" نهائياً من قاعدة البيانات " +
                                "ولا يمكن التراجع عن هذه العملية.\n" +
                                "إن كان الحساب مستخدماً في قيود أو فواتير فسيرفض النظام الحذف.\n\n" +
                                "هل أنت متأكد من الحذف النهائي؟"))
        {
            return;
        }

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _ApplyModeState();

        try
        {
            var result = await AccountService.PermanentDeleteAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم حذف الحساب نهائياً.");

                _ResetForm();
                await _LoadAsync();
            }
            else
            {
                clsDialogs.Error($"لم يتم حذف الحساب نهائياً.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            // حدّ الأمان لمعالج async void (RULE-N25/N205): رسالة عربية بلا نص الاستثناء.
            Log.Error(ex, "خطأ أثناء الحذف النهائي للحساب {AccountId}", id);
            clsDialogs.Error(PermanentDeleteFailedMessage);
        }
        finally
        {
            _actionButtonsBusy = false;
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    // ── Shortcuts ────────────────────────────────────────────────────────────

    /// <summary>Esc: داخل صندوق التصفية يمسحها، وفي وضع الإضافة/التعديل يلغي، وإلا يُغلق.</summary>
    private void _HandleEscape()
    {
        if (txtFilterValue.Focused)
        {
            _ClearAllFilters();
            return;
        }

        if (_mode != CrudMode.Browsing)
        {
            _ResetForm(_editId);
            return;
        }

        Close();
    }

    /// <summary>Delete يُعطّل الحساب فقط في وضع العرض وعندما لا يكون المؤشر داخل حقل نصي.</summary>
    private void _HandleDeleteShortcut()
    {
        // الفحص عبر بيت واحد (RULE-N169): ActiveControl يعيد الحاوية لا الحقل المركَّز نفسه.
        if (KeyboardHelper.IsTextInputFocused(this)) return;
        if (_mode != CrudMode.Browsing) return;

        _ = _RunSafeAsync(_DeleteAsync, DeleteFailedMessage);
    }

    // ── Context menu (RULE-N23) ──────────────────────────────────────────────

    private void cmsEdit_Click(object? sender, EventArgs e) => _StartEditFromSelection();

    private void cmsDelete_Click(object? sender, EventArgs e) => _ = _RunSafeAsync(_DeleteAsync, DeleteFailedMessage);

    private async void cmsRefresh_Click(object? sender, EventArgs e)
        => await _RunSafeAsync(_RefreshAsync, RefreshFailedMessage);

    // ── حدّ الأمان الوحيد للمعالجات غير المتزامنة (RULE-N205) ──────────────────

    /// <summary>
    /// ينفّذ عملية غير متزامنة ويعرض رسالة عربية واحدة عند فشلها — بدل تكرار
    /// <c>try/catch</c> في كل معالج. لا يرمي أبداً.
    /// </summary>
    private static async Task _RunSafeAsync(Func<Task> operation, string failureMessage)
    {
        try
        {
            await operation();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "دليل الحسابات: {FailureMessage}", failureMessage);
            clsDialogs.Error(failureMessage);
        }
    }
}
