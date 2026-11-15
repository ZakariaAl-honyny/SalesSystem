using System.Data;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common.Helpers;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Forms;

/// <summary>
/// شاشة دليل الحسابات (Chart of Accounts) — نموذج Master يجمع:
/// نموذج إدخال/تعديل الحساب + شبكة عرض هرمية + صندوق تصفية موحّد + شريط ملخص.
/// كل الخدمات تُحل عبر <see cref="IServiceProvider"/> من واجهة <see cref="IAccountDesktopService"/> فقط
/// (RULE-N05/RULE-N06) — لا وصول لأي طبقة أخرى، ولا اتصال شبكي، ولا DbContext.
/// </summary>
public partial class frmAccounts : Form
{
    private readonly IServiceProvider _serviceProvider = null!;

    private IAccountDesktopService AccountService => _serviceProvider.GetRequiredService<IAccountDesktopService>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly List<AccountDto> _accounts = new();
    private readonly Dictionary<int, AccountDto> _accountsById = new();

    private DataView _accountsView = new();
    private int _totalCount;

    /// <summary>null = وضع الإضافة، &gt; 0 = تعديل الحساب المحدد.</summary>
    private int? _editId;

    private bool _actionButtonsBusy;

    /// <summary>يمنع مزامنة التحديد مع المحرر أثناء إعادة تحميل الشبكة أو إعادة ضبطها.</summary>
    private bool _loadingSelection;

    /// <summary>خيار نوع الحساب — الربط بالقيمة لا بالنص العربي (RULE-N60).</summary>
    private sealed record NatureOption(byte Value, string Text)
    {
        public override string ToString() => Text;
    }

    /// <summary>خيار الحساب الأب — <c>null</c> = حساب رئيسي بلا أب.</summary>
    private sealed record ParentOption(int? Id, string Text)
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

        // RULE-N40…RULE-N47 + RULE-N95: الستة من KeyboardHelper في المُنشئ.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
        KeyboardHelper.WireGlobalShortcuts(this,                                   // RULE-N47
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F3, () => btnEdit.PerformClick()),
            (Keys.F5, () => _ = _RefreshAsync()),
            (Keys.Control | Keys.S, () => _PrimaryActionButton().PerformClick()),
            (Keys.Delete, _HandleDeleteShortcut),
            (Keys.Escape, _HandleEscape));

        // RULE-N147: Enter = الزر الأساسي للوضع الحالي (يُقرأ عند كل ضغطة)، و«AcceptButton»
        // يُضبط مركزياً في _UpdateActionButtons (RULE-N150) لا هنا.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        ApplyUiTheme();

        // RULE-N43: يُوصَّل بعد الثيم حتى يلتقط ألوان الأزرار النهائية.
        KeyboardHelper.WireButtonDisabledVisual(
            btnSave, btnEdit, btnDelete, btnPermanentDelete, btnClearFilters);

        _LoadNatureOptions();
        _SetupToolTips();

        // RULE-N97: التفقيط الحيّ على الحقل المالي الوحيد في الشاشة.
        LiveTafqeetHelper.Attach(liveTip,
            (numOpeningBalance, "الرصيد الافتتاحي", LiveTafqeetHelper.ReadMode.Money));
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    // ── Theme (RULE-N50…RULE-N54) ────────────────────────────────────────────

    /// <summary>الثيم الأخضر — ينتهي بـ <c>GreenThemeHelper.Apply(this)</c> كآخر سطر (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this); // يفرض أبيض/Segoe UI ⇒ يُمحى لاحقاً

        ThemeHelper.ApplyButtonStyle(btnNew, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnSave, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnEdit, ThemeHelper.ButtonType.Warning);
        ThemeHelper.ApplyButtonStyle(btnDelete, ThemeHelper.ButtonType.Danger);
        ThemeHelper.ApplyButtonStyle(btnPermanentDelete, ThemeHelper.ButtonType.Danger);
        ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        // RULE-N21: Tahoma لكل الأزرار (ThemeHelper يفرض Segoe UI).
        foreach (var btn in new[]
                 { btnNew, btnSave, btnEdit, btnDelete, btnPermanentDelete, btnRefresh, btnClose })
        {
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btn.UseVisualStyleBackColor = false;
        }

        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvAccounts);

        // RULE-N22/RULE-N52: سلوك الشبكة تعلنه الشاشة (ApplyGrid لا يلمسه).
        dgvAccounts.ReadOnly = true;
        dgvAccounts.AllowUserToAddRows = false;
        dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAccounts.MultiSelect = false;

        // طبقة «لا توجد بيانات» فوق الشبكة (RULE-N54: الألوان من الـ Helper فقط).
        lblEmptyState.BackColor = Color.White;
        lblEmptyState.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── Options / ToolTips ───────────────────────────────────────────────────

    /// <summary>خيارات النوع (المحرر + التصفية) مربوطة بالقيمة الرقمية لا بالنص (RULE-N60).</summary>
    private void _LoadNatureOptions()
    {
        cbNature.Items.Clear();
        cbNature.Items.Add(new NatureOption(1, "أصول"));
        cbNature.Items.Add(new NatureOption(2, "خصوم"));
        cbNature.Items.Add(new NatureOption(3, "حقوق ملكية"));
        cbNature.Items.Add(new NatureOption(4, "إيرادات"));
        cbNature.Items.Add(new NatureOption(5, "مصروفات"));
        cbNature.SelectedIndex = 0;

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

    /// <summary>RULE-N47: كل زر يحمل اختصاره في تلميحه.</summary>
    private void _SetupToolTips()
    {
        toolTip.SetToolTip(btnNew, "حساب جديد (F2)");
        toolTip.SetToolTip(btnSave, "حفظ الحساب — إضافة أو تعديل (Ctrl+S)");
        toolTip.SetToolTip(btnEdit, "تعديل الحساب المحدد (F3)");
        toolTip.SetToolTip(btnDelete, "تعطيل / حذف الحساب المحدد (Delete)");
        toolTip.SetToolTip(btnPermanentDelete, "حذف نهائي للحساب المحدد — غير النظامي فقط");
        toolTip.SetToolTip(btnRefresh, "تحديث البيانات (F5)");
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
        toolTip.SetToolTip(txtFilterValue, "بحث فوري — Esc يمسح كل التصفية");
        toolTip.SetToolTip(chkIsSystem,
            "الحسابات النظامية تُنشأ من بذور النظام ومزامنته فقط — العلم للعرض ولا يُنشئ حساباً نظامياً من الشاشة");
        toolTip.SetToolTip(txtAccountCode, "رمز الحساب يُولّده النظام ولا يُعدَّل يدوياً");
    }

    // ── Load ─────────────────────────────────────────────────────────────────

    private async void frmAccounts_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadAsync();
            _ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل الحسابات");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// «تحديث» (F5) — تحديث **كامل** (RULE-N148): تُمسح أدوات التصفية أولاً، ثم تُعاد قراءة
    /// الحسابات من الخدمة (ومنها تُبنى خيارات الحساب الأب) وتُعاد الشبكة والملخص، ويُعاد
    /// التحديد والمحرر إلى الحالة الافتراضية — ولا يقتصر على مسح التصفية.
    /// </summary>
    private async Task _RefreshAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();   // التصفية تعود للافتراضي
            _editId = null;         // إعادة ضبط التحديد قبل إعادة القراءة
            await _LoadAsync();     // إعادة بناء الشبكة والدلائل والملخص من الخدمة
            _ResetForm();           // محرر فارغ + بلا تحديد + شريط الوضع والأزرار
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث دليل الحسابات");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ─────────────────────────────────────────────────────────

    /// <summary>
    /// يحمّل كل الحسابات (بما فيها غير النشطة) ثم يبني جدول العرض ويطبّق التصفية.
    /// تُترك أعلام التنشيط للحقل <c>IsActive</c> في الجدول كي تصفّيها <c>chkShowInactive</c>
    /// من طرف العميل (مثل <c>frmCustomerMaster</c>).
    /// </summary>
    private async Task _LoadAsync()
    {
        var result = await AccountService.GetAllAsync(includeInactive: true);

        if (!result.IsSuccess || result.Value is null)
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل دليل الحسابات. يرجى إعادة المحاولة.");
            return;
        }

        _accounts.Clear();
        _accountsById.Clear();

        foreach (var account in result.Value)
        {
            _accounts.Add(account);
            _accountsById[account.Id] = account;
        }

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

        foreach (var a in _accounts.OrderBy(a => a.AccountCode, StringComparer.Ordinal))
        {
            table.Rows.Add(
                a.Id,
                a.AccountCode,
                a.NameAr,
                _IndentedName(a),
                a.NameEn ?? string.Empty,
                a.NatureDisplay,      // عربي جاهز من الـ DTO — لا نُعيد تعيينه
                a.LevelDisplay,       // عربي جاهز من الـ DTO
                a.ParentAccountName ?? string.Empty,
                a.IsLeaf,
                a.IsSystem,
                a.IsActive,
                a.IsLeaf ? "نعم" : "لا",
                a.IsSystem ? "نعم" : "لا",
                a.IsActive ? "نشط" : "معطّل",
                a.Nature);
        }

        _totalCount = table.Rows.Count;

        _loadingSelection = true;
        try
        {
            _accountsView = new DataView(table);
            dgvAccounts.DataSource = _accountsView;
        }
        finally
        {
            _loadingSelection = false;
        }

        _LoadParentOptions();
        _ApplyFilter();

        if (_editId is int id && _accountsById.ContainsKey(id))
        {
            _SelectGridRow(id);
        }
        else
        {
            _loadingSelection = true; // ClearSelection لا يجب أن ينقل المحرر إلى السطر الأول
            try
            {
                _editId = null;
                dgvAccounts.ClearSelection();
            }
            finally
            {
                _loadingSelection = false;
            }
        }

        _UpdateModeHeader();
        _UpdateActionButtons();
        _UpdateClearFiltersState(); // RULE-N148: حالة «مسح التصفية» بعد التحميل
    }

    /// <summary>إبراز التسلسل الهرمي في عمود الاسم العربي دون TreeView (RULE: عرض هرمي).</summary>
    private static string _IndentedName(AccountDto account)
        => account.Level > 1
            ? new string(' ', (account.Level - 1) * 2) + "└ " + account.NameAr
            : account.NameAr;

    // ── Combo: الحساب الأب ───────────────────────────────────────────────────

    /// <summary>
    /// يعبّئ خيارات الحساب الأب بالحسابات غير الورقية فقط،
    /// مع استثناء الحساب الجاري تعديله وكل فروعه (منع الدوران).
    /// </summary>
    private void _LoadParentOptions()
    {
        int? previousParentId = (cbParent.SelectedItem as ParentOption)?.Id;

        // الحساب الأب الحالي للحساب المفتوح يبقى في القائمة دائماً (ولو كان معطّلاً أو ورقياً):
        // لو اختفى لظهر المحرر على «بلا أب» وحُفظ التعديل بالحساب في الجذر — فقدان صلة صامت.
        int? currentParentId = _editId is int openId && _accountsById.TryGetValue(openId, out var openAccount)
            ? openAccount.ParentId
            : null;

        cbParent.Items.Clear();
        cbParent.Items.Add(new ParentOption(null, "(بلا أب — حساب رئيسي)"));

        foreach (var candidate in _accounts
                     .Where(a => a.Id == currentParentId || (!a.IsLeaf && a.IsActive))
                     .OrderBy(a => a.AccountCode, StringComparer.Ordinal))
        {
            if (_editId is int editingId
                && (candidate.Id == editingId || _IsDescendantOf(candidate.Id, editingId)))
            {
                continue;
            }

            cbParent.Items.Add(new ParentOption(candidate.Id, $"{candidate.AccountCode} — {candidate.NameAr}"));
        }

        // عند فتح حساب: يُحدَّد أبوه الفعلي (لا التحديد السابق للقائمة).
        int? selectionId = _editId is not null ? currentParentId : previousParentId;

        var match = cbParent.Items.OfType<ParentOption>()
            .FirstOrDefault(o => o.Id == selectionId);

        cbParent.SelectedItem = match ?? cbParent.Items[0];
    }

    /// <summary>هل الحساب <paramref name="accountId"/> فرع من <paramref name="ancestorId"/>؟</summary>
    private bool _IsDescendantOf(int accountId, int ancestorId)
    {
        var current = accountId;

        // حدّ أقصى للدورات يحمي من بيانات فاسدة (دورة آباء) بدل حلقة لا نهائية.
        for (var guard = 0; guard < 64; guard++)
        {
            if (!_accountsById.TryGetValue(current, out var node) || node.ParentId is not int parentId)
                return false;

            if (parentId == ancestorId)
                return true;

            current = parentId;
        }

        return false;
    }

    // ── Editor: Fill / Reset / Mode ──────────────────────────────────────────

    private void _LoadIntoEditor(int accountId)
    {
        if (!_accountsById.TryGetValue(accountId, out var account))
            return;

        bool wasLoadingSelection = _loadingSelection;
        _loadingSelection = true;
        try
        {
            _editId = account.Id;

            txtAccountCode.Text = account.AccountCode;
            txtAccountCode.Enabled = true;            // الرمز للقراءة فقط ويظهر في وضع التعديل
            txtNameAr.Text = account.NameAr;
            txtNameEn.Text = account.NameEn ?? string.Empty;

            var natureOption = cbNature.Items.OfType<NatureOption>()
                .FirstOrDefault(o => o.Value == account.Nature);
            if (natureOption is not null)
                cbNature.SelectedItem = natureOption;

            chkIsLeaf.Checked = account.IsLeaf;

            // حساب نظامي: العلم للعرض فقط في وضع التعديل (لا يُغيَّر بعد الإنشاء).
            chkIsSystem.Checked = account.IsSystem;
            chkIsSystem.Enabled = false;

            // الرصيد الافتتاحي يُقيَّد عند الإنشاء فقط (ترحيله يتم بالقيد الافتتاحي).
            numOpeningBalance.Value = 0;
            numOpeningBalance.Enabled = false;

            txtDescription.Text = account.Description ?? string.Empty;
            txtNotes.Text = account.Notes ?? string.Empty;

            // الحساب النظامي للعرض فقط: الخدمة ترفض تعديله، فلا تُترك حقول مفتوحة لا يمكن حفظها.
            _SetEditorReadOnly(account.IsSystem);

            _LoadParentOptions();

            var parentOption = cbParent.Items.OfType<ParentOption>()
                .FirstOrDefault(o => o.Id == account.ParentId);
            if (parentOption is not null)
                cbParent.SelectedItem = parentOption;

            errorProvider.Clear();
        }
        finally
        {
            _loadingSelection = wasLoadingSelection;
        }

        _UpdateModeHeader();
        _UpdateActionButtons();
    }

    /// <summary>
    /// إعادة المحرر إلى وضع «إضافة حساب جديد»: تفريغ الحقول + بلا تحديد في الشبكة + تحديث شريط الوضع والأزرار.
    /// </summary>
    /// <param name="focusNameAr">
    /// <c>false</c> عند إعادة الضبط بسبب تصفية أزالت السجل المعروض — لا يُسلَب التركيز من صندوق البحث.
    /// </param>
    private void _ResetForm(bool focusNameAr = true)
    {
        bool wasLoadingSelection = _loadingSelection;
        _loadingSelection = true;
        try
        {
            _editId = null;

            txtAccountCode.Clear();
            txtAccountCode.Enabled = false;           // الرمز نظامي ويُولَّد عند الحفظ
            txtNameAr.Clear();
            txtNameEn.Clear();

            if (cbNature.Items.Count > 0)
                cbNature.SelectedIndex = 0;

            chkIsLeaf.Checked = false;
            // الحسابات النظامية تُنشأ من بذور/مزامنة النظام فقط، والخدمة تتجاهل هذا العلم —
            // فالعلم للعرض فقط دائماً ولا يوهم المستخدم بإنشاء حساب نظامي (RULE-N167).
            chkIsSystem.Checked = false;
            chkIsSystem.Enabled = false;
            errorProvider.SetError(chkIsSystem, string.Empty);

            _SetEditorReadOnly(false);                // وضع الإضافة: كل حقول الإدخال مفتوحة

            numOpeningBalance.Value = 0;
            numOpeningBalance.Enabled = true;         // الرصيد الافتتاحي في وضع الإنشاء فقط

            txtDescription.Clear();
            txtNotes.Clear();

            _LoadParentOptions();
            cbParent.SelectedIndex = 0;

            errorProvider.Clear();
            dgvAccounts.ClearSelection();
        }
        finally
        {
            _loadingSelection = wasLoadingSelection;
        }

        _UpdateModeHeader();
        _UpdateActionButtons();

        if (focusNameAr)
            txtNameAr.Focus();
    }

    /// <summary>RULE-N44: مؤشر الوضع أعلى الشاشة.</summary>
    private void _UpdateModeHeader()
    {
        if (_editId is int id && _accountsById.TryGetValue(id, out var account))
        {
            lblMode.Text = account.IsSystem
                ? $"دليل الحسابات — حساب نظامي (عرض فقط): {account.NameAr}"
                : $"دليل الحسابات — تعديل الحساب: {account.NameAr}";
            return;
        }

        lblMode.Text = "دليل الحسابات — إضافة حساب جديد";
    }

    /// <summary>
    /// RULE-N43/RULE-N149: قفل حقول المحرر للحساب النظامي (الخدمة ترفض تعديله) —
    /// فلا يُترك إدخال يمكن كتابته بلا أي طريق للحفظ. تُفتح كلها في وضع الإضافة.
    /// </summary>
    private void _SetEditorReadOnly(bool readOnly)
    {
        txtNameAr.ReadOnly = readOnly;
        txtNameEn.ReadOnly = readOnly;
        txtDescription.ReadOnly = readOnly;
        txtNotes.ReadOnly = readOnly;

        cbNature.Enabled = !readOnly;
        cbParent.Enabled = !readOnly;
        chkIsLeaf.Enabled = !readOnly;
    }

    /// <summary>
    /// RULE-N43/RULE-N147/RULE-N150: آلة الحالة الوحيدة للأزرار وقائمة السياق والزر الأساسي —
    /// تُستدعى بعد كل انتقال (تحميل · جديد · اختيار سطر · حفظ · تعطيل · حذف · تحديث · تصفية تُخفي السجل).
    /// </summary>
    private void _UpdateActionButtons()
    {
        bool hasSelection = _editId.HasValue;
        bool isSystem = hasSelection
                        && _accountsById.TryGetValue(_editId!.Value, out var account)
                        && account.IsSystem;

        btnNew.Enabled = !_actionButtonsBusy;                                    // «جديد» متاح دائماً للخروج من وضع التعديل
        btnSave.Enabled = !hasSelection && !_actionButtonsBusy;                   // الحفظ في وضع الإضافة
        // الحساب النظامي محمي: الخدمة ترفض تعديله وتعطيله وحذفه نهائياً — فلا يُفعَّل زر لعملية مرفوضة.
        btnEdit.Enabled = hasSelection && !isSystem && !_actionButtonsBusy;
        btnDelete.Enabled = hasSelection && !isSystem && !_actionButtonsBusy;
        btnPermanentDelete.Enabled = hasSelection && !isSystem && !_actionButtonsBusy;

        // RULE-N43: بنود قائمة السياق تتبع نفس الآلة — لا بند يُنقَر بلا أثر (الزر المعطّل لا يُنفَّذ بـ PerformClick).
        cmsEdit.Enabled = btnEdit.Enabled;
        cmsDelete.Enabled = btnDelete.Enabled;

        // RULE-N147/RULE-N150: الزر الأساسي لِـ Enter يُضبط في المكان الواحد لا في المُنشئ.
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>RULE-N147: الزر الأساسي للوضع — إضافة ⇒ «حفظ» · تعديل ⇒ «تعديل».</summary>
    private Button _PrimaryActionButton() => _editId.HasValue ? btnEdit : btnSave;

    // ── Grid ─────────────────────────────────────────────────────────────────

    private void dgvAccounts_SelectionChanged(object? sender, EventArgs e)
    {
        if (_loadingSelection) return;

        if (_SelectedAccountId() is not int id) return;
        if (_editId == id) return;

        _LoadIntoEditor(id);
    }

    private void dgvAccounts_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        if (_SelectedAccountId() is int id)
        {
            _LoadIntoEditor(id);
            txtNameAr.Focus(); // نقر مزدوج = وضع التعديل (RULE §2.21 #14)
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

            _loadingSelection = true;
            try
            {
                row.Selected = true;
                dgvAccounts.CurrentCell = row.Cells["colNameAr"];
            }
            finally
            {
                _loadingSelection = false;
            }

            break;
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
    /// Esc في صندوق التصفية = مسح كل أدوات التصفية والعودة لعرض كامل (RULE-N59).
    /// يبقى المعالج باسمه القديم ويستهلك منطق «مسح التصفية» نفسه (RULE-N148).
    /// </summary>
    private void _ClearFilters() => _ClearAllFilters();

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للمحرر (RULE-N148). «تحديث» هو المسؤول عن إعادة تحميل البيانات.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح التصفية» و«تحديث».</summary>
    private void _ClearFilterInputs()
    {
        bool wasLoadingSelection = _loadingSelection;
        _loadingSelection = true; // التصفية لا تلمس المحرر: نمنع مزامنة التحديد أثناء المسح
        try
        {
            txtFilterValue.Clear();

            if (cbFilterBy.Items.Count > 0)
                cbFilterBy.SelectedIndex = 0;       // «الكود» = القيمة الافتراضية

            if (cbNatureFilter.Items.Count > 0)
                cbNatureFilter.SelectedIndex = 0;   // «كل الأنواع» = القيمة الافتراضية

            chkShowInactive.Checked = false;
            chkOnlyLeaf.Checked = false;
            chkOnlySystem.Checked = false;
            chkOnlyParents.Checked = false;
        }
        finally
        {
            _loadingSelection = wasLoadingSelection;
        }
    }

    /// <summary>يمسح كل أدوات التصفية ثم يعيد تطبيق التصفية على نفس البيانات (بلا إعادة تحميل).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();

        _ApplyFilter();
    }

    /// <summary>RULE-N43/RULE-N148: «مسح التصفية» مفعّل فقط عند وجود تصفية تختلف عن الافتراضي.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || cbFilterBy.SelectedIndex > 0
            || cbNatureFilter.SelectedIndex > 0
            || chkShowInactive.Checked
            || chkOnlyLeaf.Checked
            || chkOnlySystem.Checked
            || chkOnlyParents.Checked;
    }

    private void _ApplyFilter()
    {
        if (_accountsView.Table is null)
        {
            _UpdateSummary();
            _UpdateEmptyState();
            _UpdateClearFiltersState();
            return;
        }

        var conditions = new List<string>();

        if (!chkShowInactive.Checked)
            conditions.Add("IsActive = true");

        if (chkOnlyLeaf.Checked)
            conditions.Add("IsLeaf = true");

        if (chkOnlyParents.Checked)
            conditions.Add("IsLeaf = false");

        if (chkOnlySystem.Checked)
            conditions.Add("IsSystem = true");

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

        try
        {
            bool wasLoadingSelection = _loadingSelection;
            _loadingSelection = true; // تغيير التصفية لا ينقل المحرر إلى سطر آخر
            try
            {
                _accountsView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : string.Empty;
            }
            finally
            {
                _loadingSelection = wasLoadingSelection;
            }
        }
        catch (Exception ex)
        {
            // نص بحث غير صالح لا يُسقط الشاشة — نُسجّل ونُظهر كل البيانات.
            Log.Warning(ex, "تعذر تطبيق تصفية دليل الحسابات");
            _accountsView.RowFilter = string.Empty;
        }

        // RULE-N150: التصفية قد تُخفي السجل المفتوح في المحرر — فلا يبقى «تعديل/تعطيل» فعّالاً
        // على سجل غير معروض في الشبكة (والتركيز يبقى في صندوق البحث).
        if (_editId is int editedId && !_IsRowVisible(editedId))
            _ResetForm(focusNameAr: false);

        _UpdateSummary();
        _UpdateEmptyState();
        _UpdateClearFiltersState(); // RULE-N148: حالة زر «مسح التصفية» بعد كل تطبيق
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
            + $"حقوق ملكية: {clsFormat.Number(equity)} · إيرادات: {clsFormat.Number(revenue)} · مصروفات: {clsFormat.Number(expenses)}";
    }

    /// <summary>عند عدم وجود نتائج تُعرض طبقة «لا توجد بيانات» مكان جسم الشبكة.</summary>
    private void _UpdateEmptyState()
    {
        bool isEmpty = _accountsView.Table is null || _accountsView.Count == 0;

        lblEmptyState.Visible = isEmpty;
        if (isEmpty)
            lblEmptyState.BringToFront();
    }

    // ── Validation (RULE-N28/N29 — رسائل عربية فقط) ─────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        errorProvider.SetError(txtNameAr, string.Empty);
        errorProvider.SetError(cbNature, string.Empty);
        errorProvider.SetError(cbParent, string.Empty);
        errorProvider.SetError(numOpeningBalance, string.Empty);
        errorProvider.SetError(chkIsLeaf, string.Empty);

        if (!clsValidation.IsRequired(txtNameAr.Text))
        {
            errorProvider.SetError(txtNameAr, "الاسم العربي للحساب مطلوب");
            clsInputFeedback.ShowRejectTooltip(txtNameAr, "الاسم العربي للحساب مطلوب", toolTip);   // GAP-08b
            isValid = false;
        }

        if (cbNature.SelectedItem is not NatureOption)
        {
            errorProvider.SetError(cbNature, "يجب اختيار نوع الحساب");
            clsInputFeedback.ShowRejectTooltip(cbNature, "يجب اختيار نوع الحساب", toolTip);   // GAP-08b
            isValid = false;
        }

        int? parentId = (cbParent.SelectedItem as ParentOption)?.Id;

        if (_editId is int editingId && parentId is int candidateParentId
            && (candidateParentId == editingId || _IsDescendantOf(candidateParentId, editingId)))
        {
            errorProvider.SetError(cbParent, "لا يمكن اختيار الحساب نفسه أو أحد فروعه كحساب أب");
            clsInputFeedback.ShowRejectTooltip(cbParent,
                "لا يمكن اختيار الحساب نفسه أو أحد فروعه كحساب أب", toolTip);   // GAP-08b
            isValid = false;
        }

        // حساب له فروع لا يصبح «قابلاً للترحيل»: يصير أباً ورقةً فينكسر تسلسل الشجرة (الخدمة لا تفحصه).
        if (_editId is int leafTargetId && chkIsLeaf.Checked
            && _accounts.Exists(a => a.ParentId == leafTargetId))
        {
            const string leafError =
                "هذا الحساب له حسابات فرعية — لا يمكن تحويله إلى حساب فرعي قابل للترحيل قبل نقل فروعه";
            errorProvider.SetError(chkIsLeaf, leafError);
            clsInputFeedback.ShowRejectTooltip(chkIsLeaf, leafError, toolTip);
            isValid = false;
        }

        // الترحيل على الحسابات التفصيلية فقط: رصيد افتتاحي على حساب تجميعي يفسد أرصدة الشجرة.
        if (_editId is null && !chkIsLeaf.Checked && numOpeningBalance.Value > 0)
        {
            const string openingError =
                "الرصيد الافتتاحي يُسجَّل على حساب فرعي قابل للترحيل — فعّل «حساب فرعي قابل للترحيل»";
            errorProvider.SetError(numOpeningBalance, openingError);
            clsInputFeedback.ShowRejectTooltip(numOpeningBalance, openingError, toolTip);
            isValid = false;
        }

        if (_editId is null && numOpeningBalance.Value < 0)
        {
            errorProvider.SetError(numOpeningBalance, "الرصيد الافتتاحي لا يمكن أن يكون سالباً");
            clsInputFeedback.ShowRejectTooltip(numOpeningBalance,
                "الرصيد الافتتاحي لا يمكن أن يكون سالباً", toolTip);   // GAP-08b
            isValid = false;
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");

            if (!string.IsNullOrWhiteSpace(errorProvider.GetError(txtNameAr)))
                txtNameAr.Focus();
        }

        return isValid;
    }

    // ── Buttons ──────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _ResetForm();

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    /// <summary>Esc: داخل صندوق التصفية يمسح التصفية، وخارجها يُغلق الشاشة.</summary>
    private void _HandleEscape()
    {
        if (txtFilterValue.Focused)
        {
            _ClearFilters();
            return;
        }

        Close();
    }

    /// <summary>Delete يُعطّل الحساب فقط عندما لا يكون المؤشر داخل حقل نصي.</summary>
    private void _HandleDeleteShortcut()
    {
        if (ActiveControl is TextBoxBase)
            return;

        btnDelete.PerformClick();
    }

    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync();

    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveAsync();

    private async void btnRefresh_Click(object? sender, EventArgs e) => await _RefreshAsync();

    /// <summary>
    /// حفظ الحساب: <c>_editId == null</c> ⇒ <c>CreateAsync</c>، وإلا <c>UpdateAsync</c>.
    /// <c>AccountCode</c> يُولَّد في الخدمة — لا يُرسَل من الشاشة.
    /// </summary>
    private async Task _SaveAsync()
    {
        // حماية من النقر المزدوج: لا ندخل الحفظ مرتين قبل أن يُصفَّر العلم في finally.
        if (_actionButtonsBusy) return;

        if (!_ValidateForm())
            return;

        bool isUpdate = _editId is int;
        int? requestedId = _editId;

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _UpdateActionButtons();

        try
        {
            string nameAr = txtNameAr.Text.Trim();
            string? nameEn = txtNameEn.Text?.Trim().ToNullIfEmpty();
            byte nature = ((NatureOption)cbNature.SelectedItem!).Value;
            bool isLeaf = chkIsLeaf.Checked;
            int? parentId = (cbParent.SelectedItem as ParentOption)?.Id;
            string? description = txtDescription.Text?.Trim().ToNullIfEmpty();
            string? notes = txtNotes.Text?.Trim().ToNullIfEmpty();

            int? savedId;

            if (isUpdate && requestedId is int id)
            {
                // التصنيف (CategoryId) ليس له حقل في هذه الشاشة — نُمرّر المحفوظ كما هو
                // بدل تصفيره، وإلا فُقد تصنيف الحساب صامتاً عند أول تعديل.
                short? categoryId = _accountsById.TryGetValue(id, out var stored) ? stored.CategoryId : null;

                var request = new UpdateAccountRequest(
                    nameAr, nameEn, nature, isLeaf, parentId, categoryId, description, notes);

                var result = await AccountService.UpdateAsync(id, request);

                if (!result.IsSuccess)
                {
                    clsDialogs.Error($"لم يتم حفظ التعديل.\n{result.Error}");
                    return;
                }

                savedId = id;
            }
            else
            {
                // الحسابات النظامية تُنشأ من بذور/مزامنة النظام فقط، والخدمة تُهمل هذا العلم وتُنشئ
                // الحساب عادياً (AccountService.CreateAsync) — فإرسال ما لا يُطبَّق نجاح كاذب؛ القيمة ثابتة false.
                var request = new CreateAccountRequest(
                    nameAr, nameEn, nature, isLeaf, parentId, false,
                    null, description, notes, numOpeningBalance.Value);

                var result = await AccountService.CreateAsync(request);

                if (!result.IsSuccess)
                {
                    clsDialogs.Error($"لم يتم حفظ الحساب.\n{result.Error}");
                    return;
                }

                savedId = result.Value;
            }

            clsDialogs.Success(isUpdate ? "تم تعديل الحساب بنجاح." : "تمت إضافة الحساب بنجاح.");

            _editId = savedId;
            await _LoadAsync();

            if (savedId is int newId)
            {
                _SelectGridRow(newId);
                _LoadIntoEditor(newId);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ الحساب");
            clsDialogs.Error("حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.");
        }
        finally
        {
            _actionButtonsBusy = false;
            _UpdateActionButtons();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>تعطيل الحساب (حذف ناعم) — الحسابات النظامية يرفضها BLL وتُعرَض رسالته.</summary>
    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id || !_accountsById.TryGetValue(id, out var account))
        {
            clsDialogs.Warning("يرجى اختيار حساب من القائمة أولاً.");
            return;
        }

        if (account.IsSystem)
        {
            // الحساب النظامي محمي في الخدمة نفسها: لا تعديل ولا تعطيل ولا حذف نهائي.
            clsDialogs.Warning("هذا حساب نظامي — لا يمكن تعطيله أو تعديله أو حذفه من هذه الشاشة.");
            return;
        }

        if (!clsDialogs.Confirm($"هل أنت متأكد من تعطيل الحساب \"{account.NameAr}\"؟\n" +
                                "لن يظهر في القوائم بعد التعطيل، ويمكن إظهاره بخيار «إظهار غير النشط»."))
        {
            return;
        }

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _UpdateActionButtons();

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
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل الحساب {AccountId}", id);
            clsDialogs.Error("حدث خطأ أثناء التعطيل. يرجى إعادة المحاولة.");
        }
        finally
        {
            _actionButtonsBusy = false;
            _UpdateActionButtons();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// حذف نهائي — تحذير أقوى، ويكون الزر معطّلاً للحسابات النظامية (RULE-N92/N93).
    /// كل رفض من BLL (حساب مستخدم/له فروق) يُعرَض نصّه العربي كما هو.
    /// </summary>
    private async void btnPermanentDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id || !_accountsById.TryGetValue(id, out var account))
        {
            clsDialogs.Warning("يرجى اختيار حساب من القائمة أولاً.");
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
        _UpdateActionButtons();

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
            Log.Error(ex, "خطأ أثناء الحذف النهائي للحساب {AccountId}", id);
            clsDialogs.Error("حدث خطأ أثناء الحذف النهائي. يرجى إعادة المحاولة.");
        }
        finally
        {
            _actionButtonsBusy = false;
            _UpdateActionButtons();
            Cursor = Cursors.Default;
        }
    }

    // ── Context menu (RULE-N23) ──────────────────────────────────────────────

    private void cmsEdit_Click(object? sender, EventArgs e)
    {
        if (_SelectedAccountId() is int id)
        {
            _LoadIntoEditor(id);
            txtNameAr.Focus();
        }
    }

    private void cmsDelete_Click(object? sender, EventArgs e) => btnDelete.PerformClick();

    private async void cmsRefresh_Click(object? sender, EventArgs e) => await _RefreshAsync();

    private async void cmsTree_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await AccountService.GetTreeAsync();

            if (!result.IsSuccess || result.Value is null)
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل شجرة الحسابات.");
                return;
            }

            var builder = new StringBuilder();
            foreach (var root in result.Value)
                _AppendTreeNode(builder, root, 0);

            const int maxLength = 4000;
            string text = builder.Length > maxLength
                ? builder.ToString(0, maxLength) + "\n… (الشجرة أطول من أن تُعرض كاملة)"
                : builder.ToString();

            clsDialogs.Info(text);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء عرض شجرة الحسابات");
            clsDialogs.Error("تعذر عرض شجرة الحسابات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>يبني تمثيلاً نصياً متدرّجاً للشجرة (بدل TreeView داخل شاشة الدليل).</summary>
    private static void _AppendTreeNode(StringBuilder builder, AccountTreeNodeDto node, int depth)
    {
        builder.Append(new string(' ', depth * 2));
        builder.Append("└ ");
        builder.Append(node.AccountCode);
        builder.Append(" — ");
        builder.AppendLine(node.NameAr);

        foreach (var child in node.Children)
            _AppendTreeNode(builder, child, depth + 1);
    }
}
