using System.Data;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Forms;

/// <summary>
/// شاشة دليل الحسابات (Chart of Accounts) — نموذج Master يجمع:
/// نموذج إدخال/تعديل الحساب + شبكة عرض هرمية + صندوق تصفية موحّد + شريط ملخص.
/// كل الخدمات تُحل عبر <see cref="IServiceProvider"/> من واجهة <see cref="IAccountApiService"/> فقط
/// (RULE-N05/RULE-N06) — لا وصول لأي طبقة أخرى، ولا اتصال شبكي، ولا DbContext.
/// </summary>
public partial class frmAccounts : Form
{
    private readonly IServiceProvider _serviceProvider = null!;

    private IAccountApiService AccountService => _serviceProvider.GetRequiredService<IAccountApiService>();

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
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
        KeyboardHelper.WireGlobalShortcuts(this,                                   // RULE-N47
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F3, () => btnEdit.PerformClick()),
            (Keys.F5, () => _ = _RefreshAsync()),
            (Keys.Control | Keys.S, () => btnSave.PerformClick()),
            (Keys.Delete, _HandleDeleteShortcut),
            (Keys.Escape, _HandleEscape));

        ApplyUiTheme();

        // RULE-N43: يُوصَّل بعد الثيم حتى يلتقط ألوان الأزرار النهائية.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnPermanentDelete);

        _LoadNatureOptions();
        _SetupToolTips();

        // RULE-N97: التفقيط الحيّ على الحقل المالي الوحيد في الشاشة.
        LiveTafqeetHelper.Attach(liveTip,
            (numOpeningBalance, "الرصيد الافتتاحي", LiveTafqeetHelper.ReadMode.Money));
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
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task _RefreshAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث دليل الحسابات");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show(result.Error ?? "تعذر تحميل دليل الحسابات. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        cbParent.Items.Clear();
        cbParent.Items.Add(new ParentOption(null, "(بلا أب — حساب رئيسي)"));

        foreach (var candidate in _accounts
                     .Where(a => !a.IsLeaf && a.IsActive)
                     .OrderBy(a => a.AccountCode, StringComparer.Ordinal))
        {
            if (_editId is int editingId
                && (candidate.Id == editingId || _IsDescendantOf(candidate.Id, editingId)))
            {
                continue;
            }

            cbParent.Items.Add(new ParentOption(candidate.Id, $"{candidate.AccountCode} — {candidate.NameAr}"));
        }

        var match = cbParent.Items.OfType<ParentOption>()
            .FirstOrDefault(o => o.Id == previousParentId);

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

            _LoadParentOptions();

            var parentOption = cbParent.Items.OfType<ParentOption>()
                .FirstOrDefault(o => o.Id == account.ParentId);
            if (parentOption is not null)
                cbParent.SelectedItem = parentOption;

            errorProvider.Clear();
        }
        finally
        {
            _loadingSelection = false;
        }

        _UpdateModeHeader();
        _UpdateActionButtons();
    }

    private void _ResetForm()
    {
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
            chkIsSystem.Checked = false;
            chkIsSystem.Enabled = true;               // ذو معنى في وضع الإنشاء فقط

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
            _loadingSelection = false;
        }

        _UpdateModeHeader();
        _UpdateActionButtons();
        txtNameAr.Focus();
    }

    /// <summary>RULE-N44: مؤشر الوضع أعلى الشاشة.</summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _editId is int id && _accountsById.TryGetValue(id, out var account)
            ? $"دليل الحسابات — تعديل الحساب: {account.NameAr}"
            : "دليل الحسابات — إضافة حساب جديد";
    }

    /// <summary>RULE-N43/RULE-N92: آلة حالة الأزرار حسب الوضع والسجل المحدد.</summary>
    private void _UpdateActionButtons()
    {
        bool hasSelection = _editId.HasValue;
        bool isSystem = hasSelection
                        && _accountsById.TryGetValue(_editId!.Value, out var account)
                        && account.IsSystem;

        btnNew.Enabled = !_actionButtonsBusy;                                    // «جديد» متاح دائماً للخروج من وضع التعديل
        btnSave.Enabled = !hasSelection && !_actionButtonsBusy;                   // الحفظ في وضع الإضافة
        btnEdit.Enabled = hasSelection && !_actionButtonsBusy;
        btnDelete.Enabled = hasSelection && !_actionButtonsBusy;
        btnPermanentDelete.Enabled = hasSelection && !isSystem && !_actionButtonsBusy; // النظامي لا يُحذف نهائياً
    }

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

        if (row["IsActive"] is bool isActive && !isActive)
        {
            e.CellStyle.BackColor = Color.MistyRose;
            e.CellStyle.ForeColor = Color.DarkRed;
            e.CellStyle.SelectionForeColor = Color.DarkRed;
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

        _ClearFilters();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    /// <summary>Esc في صندوق التصفية = مسح كل أدوات التصفية والعودة لعرض كامل.</summary>
    private void _ClearFilters()
    {
        _loadingSelection = true; // لا علاقة للتصفية بالتحديد، لكن نمنع إعادة الدخول
        try
        {
            txtFilterValue.Clear();

            if (cbFilterBy.Items.Count > 0)
                cbFilterBy.SelectedIndex = 0;

            if (cbNatureFilter.Items.Count > 0)
                cbNatureFilter.SelectedIndex = 0;

            chkShowInactive.Checked = false;
            chkOnlyLeaf.Checked = false;
            chkOnlySystem.Checked = false;
            chkOnlyParents.Checked = false;
        }
        finally
        {
            _loadingSelection = false;
        }

        _ApplyFilter();
    }

    private void _ApplyFilter()
    {
        if (_accountsView.Table is null)
        {
            _UpdateSummary();
            _UpdateEmptyState();
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
        // والمدخل يُهرَّب من علامات الاقتباس التي تُفسد DataView.RowFilter.
        string text = txtFilterValue.Text.Trim().Replace("'", "''");
        if (text.Length > 0)
        {
            string column = cbFilterBy.SelectedIndex switch
            {
                1 => "NameAr",
                2 => "NameEn",
                _ => "AccountCode"
            };

            conditions.Add($"{column} LIKE '%{text}%'");
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

        _UpdateSummary();
        _UpdateEmptyState();
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

        lblSummary.Text =
            $"المعروض: {shown} من {_totalCount} — أصول: {assets} · خصوم: {liabilities} · " +
            $"حقوق ملكية: {equity} · إيرادات: {revenue} · مصروفات: {expenses}";
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

        if (string.IsNullOrWhiteSpace(txtNameAr.Text))
        {
            errorProvider.SetError(txtNameAr, "الاسم العربي للحساب مطلوب");
            isValid = false;
        }

        if (cbNature.SelectedItem is not NatureOption)
        {
            errorProvider.SetError(cbNature, "يجب اختيار نوع الحساب");
            isValid = false;
        }

        int? parentId = (cbParent.SelectedItem as ParentOption)?.Id;

        if (_editId is int editingId && parentId is int candidateParentId
            && (candidateParentId == editingId || _IsDescendantOf(candidateParentId, editingId)))
        {
            errorProvider.SetError(cbParent, "لا يمكن اختيار الحساب نفسه أو أحد فروعه كحساب أب");
            isValid = false;
        }

        if (_editId is null && numOpeningBalance.Value < 0)
        {
            errorProvider.SetError(numOpeningBalance, "الرصيد الافتتاحي لا يمكن أن يكون سالباً");
            isValid = false;
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.",
                "بيانات غير مكتملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث دليل الحسابات");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

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
            string? nameEn = _ToNullIfEmpty(txtNameEn.Text);
            byte nature = ((NatureOption)cbNature.SelectedItem!).Value;
            bool isLeaf = chkIsLeaf.Checked;
            int? parentId = (cbParent.SelectedItem as ParentOption)?.Id;
            string? description = _ToNullIfEmpty(txtDescription.Text);
            string? notes = _ToNullIfEmpty(txtNotes.Text);

            int? savedId;

            if (isUpdate && requestedId is int id)
            {
                var request = new UpdateAccountRequest(
                    nameAr, nameEn, nature, isLeaf, parentId, null, description, notes);

                var result = await AccountService.UpdateAsync(id, request);

                if (!result.IsSuccess)
                {
                    MessageBox.Show($"لم يتم حفظ التعديل.\n{result.Error}",
                        "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                savedId = id;
            }
            else
            {
                var request = new CreateAccountRequest(
                    nameAr, nameEn, nature, isLeaf, parentId, chkIsSystem.Checked,
                    null, description, notes, numOpeningBalance.Value);

                var result = await AccountService.CreateAsync(request);

                if (!result.IsSuccess)
                {
                    MessageBox.Show($"لم يتم حفظ الحساب.\n{result.Error}",
                        "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                savedId = result.Value;
            }

            MessageBox.Show(isUpdate ? "تم تعديل الحساب بنجاح." : "تمت إضافة الحساب بنجاح.",
                "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            MessageBox.Show("حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("يرجى اختيار حساب من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (account.IsSystem)
        {
            MessageBox.Show("هذا حساب نظامي لا يمكن تعطيله. يمكنك تعديل بياناته فقط.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"هل أنت متأكد من تعطيل الحساب \"{account.NameAr}\"؟\n" +
                            "لن يظهر في القوائم بعد التعطيل، ويمكن إظهاره بخيار «إظهار غير النشط».",
                "تأكيد التعطيل", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
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
                MessageBox.Show("تم تعطيل الحساب بنجاح.", "تم التعطيل",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _ResetForm();
                await _LoadAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل الحساب.\n{result.Error}",
                    "فشل التعطيل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل الحساب {AccountId}", id);
            MessageBox.Show("حدث خطأ أثناء التعطيل. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("يرجى اختيار حساب من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (account.IsSystem)
        {
            MessageBox.Show("لا يمكن حذف حساب نظامي نهائياً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"تحذير: سيتم حذف الحساب \"{account.NameAr}\" نهائياً من قاعدة البيانات " +
                            "ولا يمكن التراجع عن هذه العملية.\n" +
                            "إن كان الحساب مستخدماً في قيود أو فواتير فسيرفض النظام الحذف.\n\n" +
                            "هل أنت متأكد من الحذف النهائي؟",
                "تأكيد الحذف النهائي", MessageBoxButtons.YesNo, MessageBoxIcon.Stop,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
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
                MessageBox.Show("تم حذف الحساب نهائياً.", "تم الحذف النهائي",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _ResetForm();
                await _LoadAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم حذف الحساب نهائياً.\n{result.Error}",
                    "فشل الحذف النهائي", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء الحذف النهائي للحساب {AccountId}", id);
            MessageBox.Show("حدث خطأ أثناء الحذف النهائي. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    private async void cmsRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث دليل الحسابات من قائمة السياق");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void cmsTree_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await AccountService.GetTreeAsync();

            if (!result.IsSuccess || result.Value is null)
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل شجرة الحسابات.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var builder = new StringBuilder();
            foreach (var root in result.Value)
                _AppendTreeNode(builder, root, 0);

            const int maxLength = 4000;
            string text = builder.Length > maxLength
                ? builder.ToString(0, maxLength) + "\n… (الشجرة أطول من أن تُعرض كاملة)"
                : builder.ToString();

            MessageBox.Show(text, "شجرة الحسابات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء عرض شجرة الحسابات");
            MessageBox.Show("تعذر عرض شجرة الحسابات. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string? _ToNullIfEmpty(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
