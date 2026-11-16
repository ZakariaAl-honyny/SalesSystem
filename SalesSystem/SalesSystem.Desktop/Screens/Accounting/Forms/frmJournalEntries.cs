using System.ComponentModel;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common.Helpers;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Models.Accounting.Enums;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Forms;

/// <summary>
/// شاشة القيود المحاسبية — قائمة + تصفية موحّدة (gbFilter) + محرر قيد مزدوج + ترحيل/إلغاء/حذف مسودة.
/// كل البيانات تُقرأ وتُكتب عبر <see cref="IJournalEntryDesktopService"/> و<see cref="IAccountDesktopService"/> فقط
/// (RULE-N05/RULE-N06) — لا DbContext، ولا استدعاء شبكي، ولا أي طبقة أعمال مباشرة من الشاشة.
/// </summary>
public partial class frmJournalEntries : Form
{
    // ── ثوابت الحالة ─────────────────────────────────────────────────────────
    private const int StatusDraft = 1;
    private const int StatusPosted = 2;
    private const int StatusCancelled = 3;

    private readonly IServiceProvider _serviceProvider = null!;

    private IJournalEntryDesktopService JournalService
        => _serviceProvider.GetRequiredService<IJournalEntryDesktopService>();

    private IAccountDesktopService AccountService
        => _serviceProvider.GetRequiredService<IAccountDesktopService>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly BindingList<LineRow> _lines = new();
    private readonly List<JournalEntryListDto> _entries = new();

    private DataView _entriesView = new();

    /// <summary>السجل المحمّل في المحرر (null = وضع الإضافة).</summary>
    private int? _currentEntryId;

    private int _currentStatus;
    private string _currentEntryNumber = string.Empty;
    private string _currentStatusDisplay = string.Empty;

    /// <summary>
    /// مرجع المستند المصدر للقيد (فاتورة/سند) كما حُمّل من الخدمة — يُعاد في طلب التحديث
    /// حتى لا يُقطع ارتباط القيد بمستنده عند حفظ مسودة مُولَّدة آلياً.
    /// </summary>
    private string? _currentReferenceType;
    private int? _currentReferenceId;

    /// <summary>بصمة المحرر عند آخر تحميل/حفظ — تكشف التعديلات غير المحفوظة قبل الترحيل.</summary>
    private string _editorSignatureOnLoad = string.Empty;

    private bool _busy;
    private bool _loadingSelection;
    private bool _suppressFilterEvents = true;
    private bool _loaded;

    /// <summary>هل قيّد المستخدم التاريخ «من/إلى»؟ (افتراضياً بلا تقييد زمني حتى تظهر كل القيود).</summary>
    private bool _dateRangeActive;

    /// <summary>قيمة «من» الافتراضية — تُلتقط عند الإنشاء ويعود إليها «مسح التصفية» (RULE-N148).</summary>
    private DateTime _defaultFrom;

    /// <summary>قيمة «إلى» الافتراضية — تُلتقط عند الإنشاء ويعود إليها «مسح التصفية» (RULE-N148).</summary>
    private DateTime _defaultTo;

    private bool _filterBusy;
    private bool _filterPending;

    // ── خيارات مربوطة بالقيمة لا بالنص العربي (RULE-N60) ─────────────────────

    /// <summary>خيار نوع القيد — الاسم العربي من <see cref="JournalEntryTypeDisplay"/> فقط (RULE-N98).</summary>
    private sealed record EntryTypeOption(JournalEntryType Value, string ArabicName);

    /// <summary>خيار تصفية النوع — <c>null</c> = كل الأنواع.</summary>
    private sealed record EntryTypeFilterOption(JournalEntryType? Value, string ArabicName);

    /// <summary>خيار تصفية الحالة — <c>0</c> = كل الحالات، والقيم 1/2/3 هي قيم قاعدة البيانات.</summary>
    private sealed record StatusOption(byte Value, string Text);

    /// <summary>خيار حساب البند — العرض «الكود — الاسم».</summary>
    private sealed record AccountOption(int Id, string AccountCode, string AccountName)
    {
        public string Display => $"{AccountCode} — {AccountName}";

        public override string ToString() => Display;
    }

    /// <summary>بند القيد كما تعرضه شبكة البنود (قبل الحفظ).</summary>
    private sealed class LineRow
    {
        public int AccountId { get; init; }
        public string AccountCode { get; init; } = string.Empty;
        public string AccountName { get; init; } = string.Empty;
        public decimal Debit { get; init; }
        public decimal Credit { get; init; }
        public string Description { get; init; } = string.Empty;
    }

    // ── المُنشئ ──────────────────────────────────────────────────────────────

    /// <summary>منشئ المصمم — لمصمم Visual Studio فقط.</summary>
    private frmJournalEntries()
    {
        InitializeComponent();
    }

    public frmJournalEntries(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // RULE-N40…RULE-N47 + RULE-N95: الستة من KeyboardHelper.
        // RULE-N191: Enter داخل لوحة المحرر (بيانات القيد) يمشي بترتيب العرض — صفاً صفاً من الأعلى إلى
        // الأسفل، وفي كل صف من اليمين إلى اليسار — ثم يُركِّز زر الوضع بلا تنفيذ. وحقول التصفية وشريط
        // البند خارج النطاق فتبقى على SelectNextControl كما هي.
        KeyboardHelper.WireEnterAsTab(pnlEntryHeader, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
        KeyboardHelper.WireGlobalShortcuts(this,                                   // RULE-N47
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F3, () => { if (btnEdit.Enabled) btnEdit.PerformClick(); }),
            (Keys.F4, () => { if (btnPost.Enabled) btnPost.PerformClick(); }),
            (Keys.F5, () => _ = _RefreshAsync()),
            (Keys.Control | Keys.S, () => { if (btnSave.Enabled) btnSave.PerformClick(); }),
            (Keys.Delete, _HandleDeleteShortcut),
            (Keys.Escape, _HandleEscape));

        // RULE-N147: Enter = الزر الأساسي للوضع الحالي — ويُستثنى منه ما لا يملك Enter فيه معنى الزر.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        // RULE-N153 — Enter ملك الشبكة أينما حلّ: يُفرَّغ الزر الافتراضي عند دخول أي شبكة ويُعاد عند
        // مغادرتها، فلا يُحفظ/يُرحَّل مستند من داخل شبكة السطور ولا يُنفَّذ فعل من شبكة القيود.
        dgvLines.Enter += (_, _) => AcceptButton = null;
        dgvLines.Leave += (_, _) => AcceptButton = _PrimaryActionButton();
        dgvEntries.Enter += (_, _) => AcceptButton = null;
        dgvEntries.Leave += (_, _) => AcceptButton = _PrimaryActionButton();

        // RULE-N180…N183 — تغذية الحقول المطلوبة حيّة: أيقونة ✗ + الرسالة + ومضة رفض تعود إلى لون
        // المصمّم. الاستدعاء **بعد** WireFocusHighlight (ترتيب Leave يحسم آخر لون) و**قبل**
        // GreenThemeHelper.Apply داخلة في ApplyUiTheme. والشاشة تُعلن قواعدها فقط (RULE-N172).
        RequiredFieldHelper.Attach(this, errorProvider,
            new RequiredField(txtEntryDescription, "بيان القيد مطلوب"));

        ApplyUiTheme();

        // RULE-N43: يُوصَّل بعد الثيم حتى يلتقط الألوان النهائية للأزرار.
        KeyboardHelper.WireButtonDisabledVisual(
            btnNew, btnSave, btnEdit, btnPost, btnCancelEntry, btnDeleteDraft, btnRefresh, btnClearFilters);

        _BindEntryTypes();
        _BindStatusFilter();
        _BindFilterBy();
        _SetupToolTips();
        _ResetFilterDates();

        // RULE-N97: التفقيط الحيّ على حقلي المبلغ المالي.
        LiveTafqeetHelper.Attach(liveTip,
            (numLineDebit, "المدين", LiveTafqeetHelper.ReadMode.Money),
            (numLineCredit, "الدائن", LiveTafqeetHelper.ReadMode.Money));

        dgvLines.DataSource = _lines;
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    // ── الثيم (RULE-N50…RULE-N54) ────────────────────────────────────────────

    /// <summary>الثيم الأخضر — ينتهي بـ <c>GreenThemeHelper.Apply(this)</c> كآخر سطر (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this); // يفرض أبيض/Segoe UI ⇒ يُمحى أدناه

        ThemeHelper.ApplyButtonStyle(btnNew, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnSave, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnEdit, ThemeHelper.ButtonType.Warning);
        ThemeHelper.ApplyButtonStyle(btnPost, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnCancelEntry, ThemeHelper.ButtonType.Danger);
        ThemeHelper.ApplyButtonStyle(btnDeleteDraft, ThemeHelper.ButtonType.Danger);
        ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnRangeMonth, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnAddLine, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnRemoveLine, ThemeHelper.ButtonType.Danger);

        // RULE-N21: Tahoma لكل الأزرار (ThemeHelper يفرض Segoe UI).
        foreach (var button in new[]
                 {
                     btnNew, btnSave, btnEdit, btnPost, btnCancelEntry, btnDeleteDraft,
                     btnRefresh, btnClose, btnRangeMonth, btnAddLine, btnRemoveLine
                 })
        {
            button.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            button.UseVisualStyleBackColor = false;
        }

        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnAddLine.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRemoveLine.Font = new Font("Tahoma", 9F, FontStyle.Bold);

        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvEntries);
        GreenThemeHelper.ApplyGrid(dgvLines);

        // RULE-N22/RULE-N52: سلوك الشبكات تعلنه الشاشة (ApplyGrid لا يلمسه).
        dgvEntries.ReadOnly = true;
        dgvEntries.AllowUserToAddRows = false;
        dgvEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvEntries.MultiSelect = false;

        dgvLines.ReadOnly = true;
        dgvLines.AllowUserToAddRows = false;
        dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLines.MultiSelect = false;

        // تنسيق أعمدة المال والتاريخ من البيت الواحد (clsFormat الحيّ) لا نص حرفي في المصمّم (RULE-N169).
        GridFormatHelper.ApplyColumns(dgvEntries, GridFormatKind.Money, "colTotalDebit", "colTotalCredit");
        GridFormatHelper.ApplyColumn(dgvEntries, "colEntryDate", GridFormatKind.DateDayMonthYear);
        GridFormatHelper.ApplyColumns(dgvLines, GridFormatKind.Money, "colLineDebit", "colLineCredit");

        // خلفية lblEmptyState مُعلنة في المصمّم — لا تُكتب هنا كلون حرفي (RULE-N134).
        lblEmptyState.ForeColor = GreenThemeHelper.TextStrong;
        lblEntryInfo.ForeColor = GreenThemeHelper.TextStrong;
        lblTotals.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── الخيارات والتلميحات ──────────────────────────────────────────────────

    /// <summary>نوع القيد (المحرر + التصفية) من المصدر الوحيد <see cref="JournalEntryTypeDisplay"/>.</summary>
    private void _BindEntryTypes()
    {
        var options = JournalEntryTypeDisplay.All
            .Select(t => new EntryTypeOption(t.Value, t.ArabicName))
            .ToList();

        cbEntryType.DataSource = options;
        cbEntryType.DisplayMember = nameof(EntryTypeOption.ArabicName);
        cbEntryType.ValueMember = nameof(EntryTypeOption.Value);
        cbEntryType.SelectedIndex = 0;

        var filterOptions = new List<EntryTypeFilterOption> { new(null, "كل الأنواع") };
        filterOptions.AddRange(JournalEntryTypeDisplay.All
            .Select(t => new EntryTypeFilterOption(t.Value, t.ArabicName)));

        cbEntryTypeFilter.DataSource = filterOptions;
        cbEntryTypeFilter.DisplayMember = nameof(EntryTypeFilterOption.ArabicName);
        cbEntryTypeFilter.ValueMember = nameof(EntryTypeFilterOption.Value);
        cbEntryTypeFilter.SelectedIndex = 0;
    }

    /// <summary>حالة القيد: «كل الحالات» = 0 (محلي) ثم قيم قاعدة البيانات 1/2/3 من البيت الواحد (RULE-N169).</summary>
    private void _BindStatusFilter()
    {
        cbStatusFilter.Items.Clear();

        // العرض من حقل النص العربي لا من ToString الـrecord — وإلا ظهر «StatusOption { Value = 0, Text = ... }».
        cbStatusFilter.DisplayMember = nameof(StatusOption.Text);

        cbStatusFilter.Items.Add(new StatusOption(0, "كل الحالات"));   // عنصر محايد محلي — ليس نص حالة

        // أزواج (القيمة ↔ النص) من DocumentTextHelper.StatusOptions — لا تُكتب في الشاشة.
        foreach (var (value, text) in DocumentTextHelper.StatusOptions)
            cbStatusFilter.Items.Add(new StatusOption(value, text));

        cbStatusFilter.SelectedIndex = 0;
    }

    /// <summary>عمود البحث — يُحدَّد بالـ SelectedIndex لا بمقارنة نص عربي (RULE-N60).</summary>
    private void _BindFilterBy()
    {
        cbFilterBy.Items.Clear();
        cbFilterBy.Items.Add("رقم القيد");
        cbFilterBy.Items.Add("الوصف");
        cbFilterBy.Items.Add("المرجع");
        cbFilterBy.SelectedIndex = 0;
    }

    /// <summary>RULE-N47: كل زر يحمل اختصاره في تلميحه.</summary>
    private void _SetupToolTips()
    {
        toolTip.SetToolTip(btnNew, "قيد محاسبي جديد (F2)");
        toolTip.SetToolTip(btnSave, "حفظ القيد — إضافة أو تعديل مسودة (Ctrl+S)");
        toolTip.SetToolTip(btnEdit, "تعديل القيد المحدد — المسودات فقط (F3)");
        toolTip.SetToolTip(btnPost, "ترحيل القيد المحدد — يصبح غير قابل للتعديل (F4)");
        toolTip.SetToolTip(btnCancelEntry, "إلغاء القيد المُرحَّل — لا يمكن التراجع");
        toolTip.SetToolTip(btnDeleteDraft, "حذف مسودة القيد المحدد (Delete)");
        toolTip.SetToolTip(btnRefresh, "تحديث البيانات (F5)");
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
        toolTip.SetToolTip(txtFilterValue, "بحث فوري — Esc يمسح كل التصفية");
        toolTip.SetToolTip(btnRangeMonth, "تصفية قيود الشهر الحالي");
        toolTip.SetToolTip(btnAddLine, "إضافة البند المُدخل إلى شبكة البنود");
        toolTip.SetToolTip(btnRemoveLine, "حذف البند المحدد من شبكة البنود");
    }

    private void _ResetFilterDates()
    {
        var today = DateTime.Today;

        _defaultFrom = new DateTime(today.Year, today.Month, 1);
        _defaultTo = today;

        _suppressFilterEvents = true;
        try
        {
            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _suppressFilterEvents = false;
        }
    }

    // ── التحميل ──────────────────────────────────────────────────────────────

    private async void frmJournalEntries_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadAccountsAsync();
            await _LoadEntriesAsync();
            _ResetForm();
            _UpdateClearFiltersState(); // RULE-N43: الحالة الابتدائية لزر «مسح التصفية»
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة القيود المحاسبية");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            _loaded = true;
            _suppressFilterEvents = false;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>يحمّل الحسابات التفصيلية (الأوراق) لخيارات بنود القيد.</summary>
    private async Task _LoadAccountsAsync()
    {
        var result = await AccountService.GetAllAsync(includeInactive: false);

        if (!result.IsSuccess || result.Value is null)
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل دليل الحسابات. يرجى إعادة المحاولة.");
            return;
        }

        var options = result.Value
            .Where(a => a.IsLeaf)
            .OrderBy(a => a.AccountCode, StringComparer.Ordinal)
            .Select(a => new AccountOption(a.Id, a.AccountCode, a.NameAr))
            .ToList();

        cbLineAccount.DataSource = options;
        cbLineAccount.DisplayMember = nameof(AccountOption.Display);
        cbLineAccount.ValueMember = nameof(AccountOption.Id);
        cbLineAccount.SelectedIndex = -1;
    }

    // ── قائمة القيود (الخادم ثم DataView للتضييق) ────────────────────────────

    /// <summary>
    /// يحمّل القيود من الخدمة بكل أدوات <c>gbFilter</c> (النوع/الحالة/النطاق/البحث) — RULE-N59.
    /// </summary>
    private async Task _LoadEntriesAsync(int? selectId = null)
    {
        var result = await JournalService.GetAllAsync(
            CancellationToken.None,
            _SelectedEntryTypeFilter(),
            _SelectedStatusFilter(),
            _FromDateOrNull(),
            _ToDateOrNull(),
            _SearchText());

        if (!result.IsSuccess || result.Value is null)
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل القيود المحاسبية. يرجى إعادة المحاولة.");
            return;
        }

        _entries.Clear();

        foreach (var entry in result.Value)
        {
            _entries.Add(entry);
        }

        _BuildEntriesTable(selectId);
    }

    /// <summary>يبني جدول العرض (المرتب من الخدمة) ويعيد ربط الشبكة دون فقدان التحديد.</summary>
    private void _BuildEntriesTable(int? selectId)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("EntryNumber", typeof(string));
        table.Columns.Add("EntryDate", typeof(DateTime));
        table.Columns.Add("EntryType", typeof(string));
        table.Columns.Add("Description", typeof(string));
        table.Columns.Add("Reference", typeof(string));
        table.Columns.Add("TotalDebit", typeof(decimal));
        table.Columns.Add("TotalCredit", typeof(decimal));
        table.Columns.Add("Status", typeof(int));
        table.Columns.Add("StatusDisplay", typeof(string));

        foreach (var entry in _entries)
        {
            table.Rows.Add(
                entry.Id,
                entry.EntryNumber,
                entry.EntryDate,
                entry.EntryType,
                entry.Description,
                entry.ReferenceNumber ?? string.Empty,
                entry.TotalDebit,
                entry.TotalCredit,
                entry.Status,
                string.IsNullOrWhiteSpace(entry.StatusDisplay)
                    ? _StatusText(entry.Status)
                    : entry.StatusDisplay);
        }

        _loadingSelection = true;
        try
        {
            _entriesView = new DataView(table);
            dgvEntries.DataSource = _entriesView;
        }
        finally
        {
            _loadingSelection = false;
        }

        _ApplyFilter(selectId);
    }

    // ── التصفية (RULE-N56…RULE-N63) — فورية بلا زر بحث ───────────────────────

    private void _OnFiltersChanged()
    {
        if (_suppressFilterEvents || !_loaded) return;

        _ = _RefreshFromFiltersAsync();
    }

    /// <summary>يُسلسل نداءات التصفية المتتابعة (كتابة سريعة) دون إسقاط آخر تغيير.</summary>
    private async Task _RefreshFromFiltersAsync()
    {
        if (_filterBusy)
        {
            _filterPending = true;
            return;
        }

        _filterBusy = true;
        Cursor = Cursors.WaitCursor;

        try
        {
            do
            {
                _filterPending = false;
                await _LoadEntriesAsync(_currentEntryId);
            }
            while (_filterPending);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تصفية القيود المحاسبية");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            _filterBusy = false;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// يطبّق تضييق عمود البحث المختار على النتيجة التي أعادها الخادم (RULE-N24/RULE-N60).
    /// النوع والحالة والنطاق الزمني والبحث تُرسَل كلها إلى <c>GetAllAsync</c>، وهذا التضييق
    /// يجعل «تصفية بـ» تعني العمود المختار فعلاً بدل مطابقة عابرة للأعمدة الثلاثة.
    /// </summary>
    private void _ApplyFilter(int? selectId = null)
    {
        if (_entriesView.Table is null)
        {
            _UpdateSummary();
            _UpdateEmptyState();
            _UpdateClearFiltersState();
            return;
        }

        string rowFilter = string.Empty;
        string text = txtFilterValue.Text.Trim();

        if (text.Length > 0)
        {
            string column = cbFilterBy.SelectedIndex switch
            {
                1 => "Description",
                2 => "Reference",
                _ => "EntryNumber"
            };

            // RULE-N60/N112: التهريب في RowFilterHelper لا داخل الشاشة.
            rowFilter = RowFilterHelper.Like(column, text);
        }

        var previousSelectionGuard = _loadingSelection;
        _loadingSelection = true;
        try
        {
            _entriesView.RowFilter = rowFilter;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "تعذر تطبيق تصفية القيود المحاسبية");
            _entriesView.RowFilter = string.Empty;
        }
        finally
        {
            _loadingSelection = previousSelectionGuard;
        }

        _UpdateSummary();
        _UpdateEmptyState();
        _ApplySelection(selectId);
        _UpdateClearFiltersState(); // RULE-N43
    }

    private void _ApplySelection(int? selectId)
    {
        bool shouldSelect = selectId is int id && _RowVisible(id);

        _loadingSelection = true;
        try
        {
            if (shouldSelect)
                _SelectGridRowCore(selectId!.Value);
            else
                dgvEntries.ClearSelection();
        }
        finally
        {
            _loadingSelection = false;
        }
    }

    private bool _RowVisible(int entryId)
    {
        if (_entriesView.Table is null) return false;

        foreach (DataRowView row in _entriesView)
        {
            if (row["Id"] is int id && id == entryId) return true;
        }

        return false;
    }

    /// <summary>RULE-N61: شريط الملخص — عدد المعروض وتوزيع الحالات وإجمالي المدين/الدائن.</summary>
    private void _UpdateSummary()
    {
        int shown = 0, draft = 0, posted = 0, cancelled = 0;
        decimal totalDebit = 0m, totalCredit = 0m;

        if (_entriesView.Table is not null)
        {
            foreach (DataRowView row in _entriesView)
            {
                shown++;

                switch (row["Status"] is int status ? status : 0)
                {
                    case StatusDraft: draft++; break;
                    case StatusPosted: posted++; break;
                    case StatusCancelled: cancelled++; break;
                }

                totalDebit += row["TotalDebit"] is decimal debit ? debit : 0m;
                totalCredit += row["TotalCredit"] is decimal credit ? credit : 0m;
            }
        }

        lblSummary.Text =
            $"المعروض: {clsFormat.Number(shown)} — مسودات: {clsFormat.Number(draft)} · مُرحَّلة: {clsFormat.Number(posted)} · ملغاة: {clsFormat.Number(cancelled)} · " +
            $"إجمالي مدين: {clsFormat.Money(totalDebit)} · إجمالي دائن: {clsFormat.Money(totalCredit)}";
    }

    private void _UpdateEmptyState()
    {
        bool isEmpty = _entriesView.Table is null || _entriesView.Count == 0;

        lblEmptyState.Visible = isEmpty;
        if (isEmpty)
            lblEmptyState.BringToFront();
    }

    private JournalEntryType? _SelectedEntryTypeFilter()
        => cbEntryTypeFilter.SelectedItem is EntryTypeFilterOption option ? option.Value : null;

    private byte? _SelectedStatusFilter()
    {
        if (chkOnlyDraft.Checked) return StatusDraft;
        if (chkOnlyCancelled.Checked) return StatusCancelled;

        return cbStatusFilter.SelectedItem is StatusOption { Value: > 0 } option ? option.Value : null;
    }

    private string? _SearchText()
    {
        string text = txtFilterValue.Text.Trim();
        return text.Length == 0 ? null : text;
    }

    private DateTime? _FromDateOrNull()
        => _dateRangeActive ? dtpFrom.Value.Date : null;

    /// <summary>
    /// نهاية النطاق = آخر لحظة في اليوم المختار (شامل لليوم نفسه) — الخدمة تقارن بـ <c>&lt;=</c>،
    /// فتمرير بداية اليوم التالي كان يُدخل قيود يومٍ زائد في الشبكة والمجاميع.
    /// </summary>
    private DateTime? _ToDateOrNull()
        => _dateRangeActive ? clsValidation.EndOfDay(dtpTo.Value) : null;

    private void txtFilterValue_TextChanged(object? sender, EventArgs e) => _OnFiltersChanged();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _OnFiltersChanged();

    private void cbEntryTypeFilter_SelectedIndexChanged(object? sender, EventArgs e) => _OnFiltersChanged();

    private void cbStatusFilter_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suppressFilterEvents) return;

        // اختيار حالة بعينها يُلغي مربعات الاختيار حتى تبقى الحالة مصدراً واحداً.
        _suppressFilterEvents = true;
        try
        {
            chkOnlyDraft.Checked = false;
            chkOnlyCancelled.Checked = false;
        }
        finally
        {
            _suppressFilterEvents = false;
        }

        _OnFiltersChanged();
    }

    private void chkOnlyDraft_CheckedChanged(object? sender, EventArgs e)
    {
        if (_suppressFilterEvents) return;

        if (chkOnlyDraft.Checked)
        {
            _suppressFilterEvents = true;
            try
            {
                chkOnlyCancelled.Checked = false;
                if (cbStatusFilter.SelectedIndex != 0) cbStatusFilter.SelectedIndex = 0;
            }
            finally
            {
                _suppressFilterEvents = false;
            }
        }

        _OnFiltersChanged();
    }

    private void chkOnlyCancelled_CheckedChanged(object? sender, EventArgs e)
    {
        if (_suppressFilterEvents) return;

        if (chkOnlyCancelled.Checked)
        {
            _suppressFilterEvents = true;
            try
            {
                chkOnlyDraft.Checked = false;
                if (cbStatusFilter.SelectedIndex != 0) cbStatusFilter.SelectedIndex = 0;
            }
            finally
            {
                _suppressFilterEvents = false;
            }
        }

        _OnFiltersChanged();
    }

    private void dtpFrom_ValueChanged(object? sender, EventArgs e)
    {
        if (_suppressFilterEvents) return;

        _dateRangeActive = true;
        _OnFiltersChanged();
    }

    private void dtpTo_ValueChanged(object? sender, EventArgs e)
    {
        if (_suppressFilterEvents) return;

        _dateRangeActive = true;
        _OnFiltersChanged();
    }

    private void btnRangeMonth_Click(object? sender, EventArgs e)
    {
        var today = DateTime.Today;

        _dateRangeActive = true;
        _suppressFilterEvents = true;
        try
        {
            dtpFrom.Value = new DateTime(today.Year, today.Month, 1);
            dtpTo.Value = today;
        }
        finally
        {
            _suppressFilterEvents = false;
        }

        _OnFiltersChanged();
    }

    private void txtFilterValue_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Escape) return;

        _ClearAllFilters();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد شبكة القيود إلى قائمتها الافتراضية،
    /// بلا لمس محرر القيد ولا شبكة البنود ولا إعادة تحميل دليل الحسابات (RULE-N148).
    /// «تحديث» (F5) هو المسؤول عن إعادة قراءة **كل** بيانات الشاشة وإرجاع المحرر للافتراضي.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>
    /// تفريغ أدوات التصفية وحدها إلى الوضع الافتراضي (بلا تطبيق ولا إعادة تحميل) — RULE-N148.
    /// تُستهلك من «مسح التصفية» و«تحديث» — ولا تمس محرر القيد ولا شبكة البنود.
    /// </summary>
    private void _ClearFilterInputs()
    {
        bool wasSuppressed = _suppressFilterEvents;

        // منع تصفية وسيطة عند كل ضبط حقل — تُطبَّق مرة واحدة بعد التفريغ.
        _suppressFilterEvents = true;
        try
        {
            txtFilterValue.Clear();
            cbFilterBy.SelectedIndex = 0;
            cbEntryTypeFilter.SelectedIndex = 0;
            cbStatusFilter.SelectedIndex = 0;
            chkOnlyDraft.Checked = false;
            chkOnlyCancelled.Checked = false;
            _dateRangeActive = false;
            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _suppressFilterEvents = wasSuppressed;
        }
    }

    /// <summary>
    /// يمسح كل أدوات التصفية ثم يستعيد القائمة الافتراضية — RULE-N59.
    /// <para>
    /// التصفية هنا تُنفَّذ على الخادم (النوع/الحالة/النطاق/البحث)، فمسح أدواتها لا يكفي لاستعادة
    /// كل القيود: تُعاد قراءة **القائمة وحدها** (لا المحرر ولا دليل الحسابات ولا إعدادات الشاشة)،
    /// وإلا بقي المعروض مجموعة مفلترة بينما الأدوات تقول «كل الحالات» (RULE-N148).
    /// </para>
    /// </summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();

        if (_loaded)
            _ = _RefreshFromFiltersAsync();
        else
            _ApplyFilter(_currentEntryId);
    }

    /// <summary>RULE-N43/RULE-N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية تختلف عن الوضع الافتراضي.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || cbFilterBy.SelectedIndex > 0
            || cbEntryTypeFilter.SelectedIndex > 0
            || cbStatusFilter.SelectedIndex > 0
            || chkOnlyDraft.Checked
            || chkOnlyCancelled.Checked
            || _dateRangeActive
            || dtpFrom.Value.Date != _defaultFrom.Date
            || dtpTo.Value.Date != _defaultTo.Date;
    }

    // ── تحميل قيد في المحرر ──────────────────────────────────────────────────

    private async Task _LoadEntryAsync(int entryId)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await JournalService.GetByIdAsync(entryId);

            if (!result.IsSuccess || result.Value is null)
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل القيد المحاسبي المحدد.");
                return;
            }

            var entry = result.Value;

            _currentEntryId = entry.Id;
            _currentStatus = entry.Status;
            _currentEntryNumber = entry.EntryNumber;
            _currentReferenceType = entry.ReferenceType;
            _currentReferenceId = entry.ReferenceId;
            _currentStatusDisplay = string.IsNullOrWhiteSpace(entry.StatusDisplay)
                ? _StatusText(entry.Status)
                : entry.StatusDisplay;

            _suppressFilterEvents = true;
            try
            {
                dtpEntryDate.Value = _ClampToPickerRange(entry.EntryDate);
                _SelectEntryType(entry.EntryTypeValue);
                txtEntryDescription.Text = entry.Description;
                txtReferenceNumber.Text = entry.ReferenceNumber ?? string.Empty;
            }
            finally
            {
                _suppressFilterEvents = false;
            }

            _lines.Clear();
            foreach (var line in entry.Lines)
            {
                _lines.Add(new LineRow
                {
                    AccountId = line.AccountId,
                    AccountCode = line.AccountCode ?? string.Empty,
                    AccountName = line.AccountName ?? string.Empty,
                    Debit = line.Debit,
                    Credit = line.Credit,
                    Description = line.Description ?? string.Empty
                });
            }

            errorProvider.Clear();
            RequiredFieldHelper.Clear(this);   // RULE-N183 — العلامات تزول مع كل إعادة ضبط (تحميل قيد)
            _ResetLineEntry();
            _UpdateTotals();
            _UpdateModeHeader();
            _UpdateActionButtons();

            // بصمة المحرر بعد التحميل — أساس كشف التعديلات غير المحفوظة قبل الترحيل.
            _editorSignatureOnLoad = _EditorSignature();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل القيد المحاسبي {EntryId}", entryId);
            clsDialogs.Error("حدث خطأ غير متوقع أثناء فتح القيد المحاسبي. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>RULE-N44: مؤشر الوضع أعلى الشاشة.</summary>
    private void _UpdateModeHeader()
    {
        if (_currentEntryId is null)
        {
            lblMode.Text = "قيد محاسبي جديد";
            lblEntryInfo.Text = "رقم القيد: — · الحالة: مسودة غير محفوظة";
            return;
        }

        if (_currentStatus == StatusDraft)
        {
            lblMode.Text = $"تعديل القيد رقم {_currentEntryNumber}";
            lblEntryInfo.Text =
                $"رقم القيد: {_currentEntryNumber} · الحالة: {_currentStatusDisplay} — يمكن التعديل أو الترحيل أو الحذف";
        }
        else
        {
            lblMode.Text = $"عرض القيد رقم {_currentEntryNumber} — {_currentStatusDisplay}";
            lblEntryInfo.Text = _currentStatus == StatusPosted
                ? $"رقم القيد: {_currentEntryNumber} · الحالة: {_currentStatusDisplay} — القيد مُرحَّل ولا يمكن تعديله، ويمكن إلغاؤه فقط"
                : $"رقم القيد: {_currentEntryNumber} · الحالة: {_currentStatusDisplay} — القيد ملغى ولا يمكن تعديله";
        }
    }

    /// <summary>RULE-N43/RULE-N92: آلة حالة الأزرار وحقول المحرر حسب الوضع وحالة السجل.</summary>
    private void _UpdateActionButtons()
    {
        bool hasEntry = _currentEntryId.HasValue;
        bool isDraft = hasEntry && _currentStatus == StatusDraft;
        bool isPosted = hasEntry && _currentStatus == StatusPosted;
        bool addMode = !hasEntry;

        // القيد المُرحَّل/الملغى غير قابل للتعديل (RULE-004 / §2.4) — والمسودة قابلة للتعديل فوراً.
        bool editable = !_busy && (addMode || isDraft);

        btnNew.Enabled = !_busy;
        btnRefresh.Enabled = !_busy;
        btnSave.Enabled = editable;
        btnEdit.Enabled = !_busy && isDraft;
        btnPost.Enabled = !_busy && isDraft;
        btnCancelEntry.Enabled = !_busy && isPosted;
        btnDeleteDraft.Enabled = !_busy && isDraft;

        dtpEntryDate.Enabled = editable;
        cbEntryType.Enabled = editable;
        txtEntryDescription.Enabled = editable;
        txtReferenceNumber.Enabled = editable;

        cbLineAccount.Enabled = editable;
        numLineDebit.Enabled = editable;
        numLineCredit.Enabled = editable;
        txtLineDescription.Enabled = editable;
        btnAddLine.Enabled = editable;
        btnRemoveLine.Enabled = editable && _lines.Count > 0;

        // RULE-N147: الزر الافتراضي يتبع الوضع — ويُلغى في القيد المُرحَّل/الملغى (RULE-N153).
        AcceptButton = _PrimaryActionButton();
    }

    private void _ResetForm()
    {
        _currentEntryId = null;
        _currentStatus = 0;
        _currentEntryNumber = string.Empty;
        _currentStatusDisplay = string.Empty;
        _currentReferenceType = null;
        _currentReferenceId = null;

        _suppressFilterEvents = true;
        try
        {
            dtpEntryDate.Value = _ClampToPickerRange(DateTime.Today);
            _SelectEntryType(JournalEntryType.Manual);
            txtEntryDescription.Clear();
            txtReferenceNumber.Clear();
        }
        finally
        {
            _suppressFilterEvents = false;
        }

        _lines.Clear();
        _ResetLineEntry();
        errorProvider.Clear();
        RequiredFieldHelper.Clear(this);   // RULE-N183 — «جديد»/التحديث يبدآن بلا أي علامة رفض
        _UpdateTotals();

        _loadingSelection = true;
        try
        {
            dgvEntries.ClearSelection();
        }
        finally
        {
            _loadingSelection = false;
        }

        _UpdateModeHeader();
        _UpdateActionButtons();
        _editorSignatureOnLoad = _EditorSignature();
        txtEntryDescription.Focus();
    }

    private void _ResetLineEntry()
    {
        errorProvider.SetError(cbLineAccount, string.Empty);
        errorProvider.SetError(numLineDebit, string.Empty);
        errorProvider.SetError(numLineCredit, string.Empty);

        cbLineAccount.SelectedIndex = -1;
        numLineDebit.Value = 0m;
        numLineCredit.Value = 0m;
        txtLineDescription.Clear();
    }

    private void _SelectEntryType(JournalEntryType entryType)
    {
        for (int index = 0; index < cbEntryType.Items.Count; index++)
        {
            if (cbEntryType.Items[index] is EntryTypeOption option && option.Value == entryType)
            {
                cbEntryType.SelectedIndex = index;
                return;
            }
        }

        if (cbEntryType.Items.Count > 0)
            cbEntryType.SelectedIndex = 0;
    }

    private JournalEntryType _CurrentEntryType()
        => cbEntryType.SelectedItem is EntryTypeOption option ? option.Value : JournalEntryType.Manual;

    private DateTime _ClampToPickerRange(DateTime value)
    {
        if (value < dtpEntryDate.MinDate) return dtpEntryDate.MinDate;
        if (value > dtpEntryDate.MaxDate) return dtpEntryDate.MaxDate;
        return value;
    }

    // ── شبكة القيود (RULE-N22/N23) ───────────────────────────────────────────

    private int? _SelectedEntryId()
        => dgvEntries.CurrentRow is { Selected: true } row
           && row.DataBoundItem is DataRowView view
           && view["Id"] is int id
            ? id
            : null;

    private async void dgvEntries_SelectionChanged(object? sender, EventArgs e)
    {
        try
        {
            if (_loadingSelection) return;
            if (_SelectedEntryId() is not int id) return;
            if (_currentEntryId == id) return;

            await _LoadEntryAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تغيير القيد المحدد");
        }
    }

    private async void dgvEntries_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        try
        {
            if (_SelectedEntryId() is not int id) return;

            if (_currentEntryId != id)
                await _LoadEntryAsync(id);

            if (_currentStatus == StatusDraft)
            {
                txtEntryDescription.Focus();
                txtEntryDescription.SelectAll();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء فتح القيد المحاسبي بالنقر المزدوج");
            clsDialogs.Error("تعذر فتح القيد المحاسبي المحدد. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>خلفيات الحالات: مسودة صفراء فاتحة، مُرحَّل أبيض، ملغى وردي/أحمر داكن.</summary>
    private void dgvEntries_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgvEntries.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;

        switch (row["Status"] is int status ? status : 0)
        {
            case StatusDraft:
                e.CellStyle.BackColor = GreenThemeHelper.StatusWarningBack;
                e.CellStyle.ForeColor = GreenThemeHelper.StatusWarning;
                e.CellStyle.SelectionForeColor = GreenThemeHelper.StatusWarning;
                break;

            case StatusCancelled:
                e.CellStyle.BackColor = GreenThemeHelper.StatusDangerBack;
                e.CellStyle.ForeColor = GreenThemeHelper.StatusDanger;
                e.CellStyle.SelectionForeColor = GreenThemeHelper.StatusDanger;
                break;

            default:
                // لون الحالة من الثيم لا من لون حرفي (RULE-N54/RULE-N142).
                e.CellStyle.BackColor = GreenThemeHelper.StatusNeutralBack;
                e.CellStyle.ForeColor = GreenThemeHelper.StatusNeutral;
                e.CellStyle.SelectionForeColor = GreenThemeHelper.StatusNeutral;
                break;
        }
    }

    private void _SelectGridRowCore(int entryId)
    {
        foreach (DataGridViewRow row in dgvEntries.Rows)
        {
            if (row.DataBoundItem is not DataRowView view || view["Id"] is not int rowId || rowId != entryId)
                continue;

            row.Selected = true;
            dgvEntries.CurrentCell = row.Cells["colEntryNumber"];
            break;
        }
    }

    // ── بنود القيد ───────────────────────────────────────────────────────────

    private void btnAddLine_Click(object? sender, EventArgs e) => _AddLineFromInput();

    private void _AddLineFromInput()
    {
        errorProvider.SetError(cbLineAccount, string.Empty);
        errorProvider.SetError(numLineDebit, string.Empty);
        errorProvider.SetError(numLineCredit, string.Empty);

        if (cbLineAccount.SelectedItem is not AccountOption account)
        {
            errorProvider.SetError(cbLineAccount, "يجب اختيار الحساب");
            clsInputFeedback.ShowRejectTooltip(cbLineAccount, "يجب اختيار الحساب", toolTip);   // GAP-08b
            clsDialogs.Warning("يرجى اختيار الحساب أولاً قبل إضافة البند.");
            cbLineAccount.Focus();
            return;
        }

        decimal debit = numLineDebit.Value;
        decimal credit = numLineCredit.Value;

        if (debit > 0m && credit > 0m)
        {
            errorProvider.SetError(numLineDebit, "لا يجوز أن يكون البند مديناً ودائناً معاً");
            errorProvider.SetError(numLineCredit, "لا يجوز أن يكون البند مديناً ودائناً معاً");
            clsInputFeedback.ShowRejectTooltip(numLineDebit,
                "لا يجوز أن يكون البند مديناً ودائناً معاً", toolTip);   // GAP-08b
            clsInputFeedback.ShowRejectTooltip(numLineCredit,
                "لا يجوز أن يكون البند مديناً ودائناً معاً", toolTip);   // GAP-08b
            clsDialogs.Warning("لا يمكن أن يحتوي البند على مبلغ مدين ومبلغ دائن في الوقت نفسه.");
            numLineDebit.Focus();
            return;
        }

        if (debit <= 0m && credit <= 0m)
        {
            errorProvider.SetError(numLineDebit, "أدخل مبلغ المدين أو الدائن");
            clsInputFeedback.ShowRejectTooltip(numLineDebit, "أدخل مبلغ المدين أو الدائن", toolTip);   // GAP-08b
            clsDialogs.Warning("يجب إدخال مبلغ مدين أو مبلغ دائن أكبر من صفر.");
            numLineDebit.Focus();
            return;
        }

        _lines.Add(new LineRow
        {
            AccountId = account.Id,
            AccountCode = account.AccountCode,
            AccountName = account.AccountName,
            Debit = debit,
            Credit = credit,
            Description = txtLineDescription.Text.Trim()
        });

        _ResetLineEntry();
        _UpdateTotals();
        _UpdateActionButtons();
        cbLineAccount.Focus();
    }

    private void btnRemoveLine_Click(object? sender, EventArgs e)
    {
        if (dgvLines.CurrentRow?.DataBoundItem is not LineRow row)
        {
            clsDialogs.Warning("يرجى تحديد بند من شبكة البنود أولاً.");
            return;
        }

        _lines.Remove(row);
        _UpdateTotals();
        _UpdateActionButtons();
    }

    private void _UpdateTotals()
    {
        decimal debit = 0m, credit = 0m;

        foreach (var line in _lines)
        {
            debit += line.Debit;
            credit += line.Credit;
        }

        decimal difference = debit - credit;

        lblTotals.Text =
            $"مجموع المدين: {clsFormat.Money(debit)} · مجموع الدائن: {clsFormat.Money(credit)} · الفرق: {clsFormat.Money(difference)}";

        lblTotals.ForeColor = difference == 0m
            ? GreenThemeHelper.TextStrong
            : GreenThemeHelper.StatusDanger;
    }

    // ── التحقق (RULE-N28/N29) ────────────────────────────────────────────────

    /// <summary>
    /// حكم توازن القيد المزدوج — يُستهلك في التحقق قبل الحفظ وفي التحقق قبل الترحيل،
    /// فلا تتكرر المقارنة المالية في موضعين (RULE-N172). الأرضية <c>0.001</c> مطابقة لتسامح الخدمة.
    /// </summary>
    private static bool _IsBalanced(decimal totalDebit, decimal totalCredit)
        => Math.Abs(totalDebit - totalCredit) <= 0.001m;

    /// <summary>
    /// تحقق **الشروط المحاسبية** قبل الحفظ — لا حكم «فارغ» فيه: بيان القيد المطلوب صار قاعدة معلنة في
    /// البيت الواحد <see cref="RequiredFieldHelper"/> (RULE-N172/RULE-N180…N183). أمّا شروط القيد المزدوج
    /// (بندان على الأقل · حساب لكل بند · لا سالب · لا مدين ودائن معاً · لا بند بلا مبلغ · التوازن)
    /// فهي **حرفياً كما كانت** — لا تُغيَّر ولا تُنقل.
    /// </summary>
    private bool _ValidateRangeRules()
    {
        errorProvider.SetError(dgvLines, string.Empty);
        errorProvider.SetError(lblTotals, string.Empty);

        // الحقول المطلوبة: أيقونة ✗ + الرسالة + ومضة رفض من البيت الواحد (RULE-N180…N183).
        bool isValid = RequiredFieldHelper.Validate(this);

        if (_lines.Count < 2)
        {
            errorProvider.SetError(dgvLines, "القيد المحاسبي المزدوج يتطلب بندين على الأقل");
            clsInputFeedback.ShowRejectTooltip(dgvLines,
                "القيد المحاسبي المزدوج يتطلب بندين على الأقل", toolTip);   // GAP-08b
            isValid = false;
        }

        if (_lines.Any(l => l.AccountId <= 0))
        {
            errorProvider.SetError(dgvLines, "كل بند يجب أن يحمل حساباً");
            clsInputFeedback.ShowRejectTooltip(dgvLines, "كل بند يجب أن يحمل حساباً", toolTip);   // GAP-08b
            isValid = false;
        }

        if (_lines.Any(l => l.Debit < 0m || l.Credit < 0m))
        {
            errorProvider.SetError(dgvLines, "لا يمكن أن يكون مبلغ المدين أو الدائن سالباً");
            clsInputFeedback.ShowRejectTooltip(dgvLines,
                "لا يمكن أن يكون مبلغ المدين أو الدائن سالباً", toolTip);   // GAP-08b
            isValid = false;
        }

        if (_lines.Any(l => l.Debit > 0m && l.Credit > 0m))
        {
            errorProvider.SetError(dgvLines, "لا يمكن أن يكون البند مديناً ودائناً في الوقت نفسه");
            clsInputFeedback.ShowRejectTooltip(dgvLines,
                "لا يمكن أن يكون البند مديناً ودائناً في الوقت نفسه", toolTip);   // GAP-08b
            isValid = false;
        }

        if (_lines.Any(l => l.Debit == 0m && l.Credit == 0m))
        {
            errorProvider.SetError(dgvLines, "لا يمكن أن يكون البند بلا مبلغ");
            clsInputFeedback.ShowRejectTooltip(dgvLines, "لا يمكن أن يكون البند بلا مبلغ", toolTip);   // GAP-08b
            isValid = false;
        }

        decimal totalDebit = _lines.Sum(l => l.Debit);
        decimal totalCredit = _lines.Sum(l => l.Credit);

        if (!_IsBalanced(totalDebit, totalCredit))
        {
            string balanceMessage =
                $"القيد غير متوازن — مجموع المدين {clsFormat.Money(totalDebit)} لا يساوي مجموع الدائن {clsFormat.Money(totalCredit)}";

            errorProvider.SetError(lblTotals, balanceMessage);
            clsInputFeedback.ShowRejectTooltip(lblTotals, balanceMessage, toolTip);   // GAP-08b
            isValid = false;
        }

        if (isValid) return true;

        clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل الحفظ.");

        // أول حقل مطلوب ناقص من البيت الواحد (لا حكم «فارغ» ثانٍ في الشاشة) — ثم الحساب عند نقص البنود.
        if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing)
            firstMissing.Focus();
        else if (_lines.Count < 2)
            cbLineAccount.Focus();

        return false;
    }

    // ── الحفظ ────────────────────────────────────────────────────────────────

    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync();

    /// <summary>حفظ القيد: بلا سجل ⇒ <c>CreateAsync</c>، وبسجل مسودة ⇒ <c>UpdateAsync</c>.</summary>
    private async Task _SaveAsync()
    {
        if (_busy) return;
        if (_currentEntryId is int existingId && _currentStatus != StatusDraft)
        {
            clsDialogs.Warning("لا يمكن حفظ قيد مُرحَّل أو ملغى. القيود المُرحَّلة غير قابلة للتعديل.");
            return;
        }

        if (!_ValidateRangeRules())
            return;

        bool isUpdate = _currentEntryId is int;
        int? requestedId = _currentEntryId;

        var lines = _lines
            .Select(l => new JournalEntryLineRequest(
                l.AccountId,
                l.Debit,
                l.Credit,
                string.IsNullOrWhiteSpace(l.Description) ? null : l.Description))
            .ToList();

        DateTime entryDate = dtpEntryDate.Value.Date;
        string description = txtEntryDescription.Text.Trim();
        JournalEntryType entryType = _CurrentEntryType();
        string? referenceNumber = txtReferenceNumber.Text?.Trim().ToNullIfEmpty();

        Cursor = Cursors.WaitCursor;
        _busy = true;
        _UpdateActionButtons();

        try
        {
            int? savedId;

            if (isUpdate && requestedId is int id)
            {
                var request = new UpdateJournalEntryRequest(
                    entryDate, description, entryType,
                    _currentReferenceType, _currentReferenceId,   // المرجع المصدر يُعاد كما حُمّل — لا يُقطع ارتباط القيد بمستنده
                    referenceNumber, lines);

                var result = await JournalService.UpdateAsync(id, request);

                if (!result.IsSuccess)
                {
                    clsDialogs.Error($"لم يتم حفظ تعديل القيد.\n{result.Error}");
                    return;
                }

                savedId = id;
            }
            else
            {
                var request = new CreateJournalEntryRequest(
                    entryDate, description, entryType, null, null, referenceNumber, lines);

                var result = await JournalService.CreateAsync(request);

                if (!result.IsSuccess)
                {
                    clsDialogs.Error($"لم يتم حفظ القيد المحاسبي.\n{result.Error}");
                    return;
                }

                savedId = result.Value;
            }

            clsDialogs.Success(isUpdate
                    ? "تم تعديل القيد المحاسبي بنجاح."
                    : "تم حفظ القيد المحاسبي كمسودة بنجاح.");

            await _LoadEntriesAsync(savedId);

            if (savedId is int newId)
                await _LoadEntryAsync(newId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ القيد المحاسبي");
            clsDialogs.Error("حدث خطأ أثناء حفظ القيد المحاسبي. يرجى إعادة المحاولة.");
        }
        finally
        {
            _busy = false;
            _UpdateActionButtons();
            Cursor = Cursors.Default;
        }
    }

    // ── الترحيل / الإلغاء / حذف المسودة ──────────────────────────────────────

    private void btnPost_Click(object? sender, EventArgs e) => _ = _PostAsync();

    private async Task _PostAsync()
    {
        if (_currentEntryId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار قيد محاسبي من القائمة أولاً.");
            return;
        }

        if (_currentStatus != StatusDraft)
        {
            clsDialogs.Warning("لا يمكن ترحيل هذا القيد — القيود المُرحَّلة أو الملغاة لا تُرحَّل مرة أخرى.");
            return;
        }

        // الترحيل يُرسل نسخة الخادم: لو كان المحرر يحمل تعديلات غير محفوظة لَرُحِّل القيد ببيانات
        // قديمة وأُسقط ما يراه المستخدم على الشاشة — يُطلب الحفظ أولاً.
        if (_EditorSignature() != _editorSignatureOnLoad)
        {
            clsDialogs.Warning(
                "توجد تعديلات غير محفوظة على القيد — احفظها أولاً (Ctrl+S) ثم أعد المحاولة،\n" +
                "حتى لا يُرحَّل القيد ببيانات سابقة لإدخالك.");
            return;
        }

        // لا يُرحَّل قيد غير متوازن — الترحيل المالي لا يمرّ بلا تحقق من التوازن.
        decimal totalDebit = _lines.Sum(l => l.Debit);
        decimal totalCredit = _lines.Sum(l => l.Credit);

        if (_lines.Count < 2 || !_IsBalanced(totalDebit, totalCredit))
        {
            clsDialogs.Error(
                $"لا يمكن ترحيل قيد غير متوازن أو ناقص البنود — " +
                $"مجموع المدين {clsFormat.Money(totalDebit)} ومجموع الدائن {clsFormat.Money(totalCredit)}، " +
                $"وعدد البنود {clsFormat.Number(_lines.Count)}.");
            return;
        }

        if (!clsDialogs.Confirm(
                $"هل تريد ترحيل القيد رقم {_currentEntryNumber}؟\n" +
                "بعد الترحيل لا يمكن تعديل القيد أو حذفه، ويمكن إلغاؤه فقط."))
        {
            return;
        }

        Cursor = Cursors.WaitCursor;
        _busy = true;
        _UpdateActionButtons();

        try
        {
            var result = await JournalService.PostAsync(id);

            if (!result.IsSuccess)
            {
                clsDialogs.Error($"لم يتم ترحيل القيد.\n{result.Error}");
                return;
            }

            clsDialogs.Success("تم ترحيل القيد المحاسبي بنجاح.");

            await _LoadEntriesAsync(id);
            await _LoadEntryAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء ترحيل القيد المحاسبي {EntryId}", id);
            clsDialogs.Error("حدث خطأ أثناء ترحيل القيد المحاسبي. يرجى إعادة المحاولة.");
        }
        finally
        {
            _busy = false;
            _UpdateActionButtons();
            Cursor = Cursors.Default;
        }
    }

    private void btnCancelEntry_Click(object? sender, EventArgs e) => _ = _CancelEntryAsync();

    private async Task _CancelEntryAsync()
    {
        if (_currentEntryId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار قيد محاسبي من القائمة أولاً.");
            return;
        }

        if (_currentStatus != StatusPosted)
        {
            clsDialogs.Warning("لا يمكن إلغاء هذا القيد — الإلغاء متاح للقيود المُرحَّلة فقط.");
            return;
        }

        if (!clsDialogs.Confirm(
                $"تحذير: سيتم إلغاء القيد رقم {_currentEntryNumber} نهائياً.\n" +
                "لا يمكن التراجع عن هذه العملية، وسيبقى القيد في السجل بحالة «ملغى».\n\n" +
                "هل أنت متأكد من إلغاء القيد؟"))
        {
            return;
        }

        Cursor = Cursors.WaitCursor;
        _busy = true;
        _UpdateActionButtons();

        try
        {
            var result = await JournalService.CancelAsync(id);

            if (!result.IsSuccess)
            {
                clsDialogs.Error($"لم يتم إلغاء القيد.\n{result.Error}");
                return;
            }

            clsDialogs.Success("تم إلغاء القيد المحاسبي بنجاح.");

            await _LoadEntriesAsync(id);
            await _LoadEntryAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إلغاء القيد المحاسبي {EntryId}", id);
            clsDialogs.Error("حدث خطأ أثناء إلغاء القيد المحاسبي. يرجى إعادة المحاولة.");
        }
        finally
        {
            _busy = false;
            _UpdateActionButtons();
            Cursor = Cursors.Default;
        }
    }

    private void btnDeleteDraft_Click(object? sender, EventArgs e) => _ = _DeleteDraftAsync();

    private async Task _DeleteDraftAsync()
    {
        if (_currentEntryId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار قيد محاسبي من القائمة أولاً.");
            return;
        }

        if (_currentStatus != StatusDraft)
        {
            clsDialogs.Warning("حذف المسودة متاح للقيود بحالة «مسودة» فقط. القيود المُرحَّلة تُلغى ولا تُحذف.");
            return;
        }

        if (!clsDialogs.Confirm($"هل تريد حذف مسودة القيد رقم {_currentEntryNumber}؟"))
        {
            return;
        }

        Cursor = Cursors.WaitCursor;
        _busy = true;
        _UpdateActionButtons();

        try
        {
            var result = await JournalService.DeleteDraftAsync(id);

            if (!result.IsSuccess)
            {
                clsDialogs.Error($"لم يتم حذف المسودة.\n{result.Error}");
                return;
            }

            clsDialogs.Success("تم حذف مسودة القيد المحاسبي بنجاح.");

            await _LoadEntriesAsync();
            _ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حذف مسودة القيد المحاسبي {EntryId}", id);
            clsDialogs.Error("حدث خطأ أثناء حذف المسودة. يرجى إعادة المحاولة.");
        }
        finally
        {
            _busy = false;
            _UpdateActionButtons();
            Cursor = Cursors.Default;
        }
    }

    // ── أزرار الشريط ─────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _ResetForm();

    private void btnEdit_Click(object? sender, EventArgs e)
    {
        if (_currentEntryId is not int)
        {
            clsDialogs.Warning("يرجى اختيار قيد محاسبي من القائمة أولاً.");
            return;
        }

        if (_currentStatus != StatusDraft)
        {
            clsDialogs.Warning("لا يمكن تعديل قيد مُرحَّل أو ملغى — القيود المُرحَّلة غير قابلة للتعديل.");
            return;
        }

        txtEntryDescription.Focus();
        txtEntryDescription.SelectAll();
    }

    private void btnRefresh_Click(object? sender, EventArgs e) => _ = _RefreshAsync();

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← دليل الحسابات ← القيود ← الملخص والتحديد والمحرر.
    /// «مسح التصفية» لا يفعل ذلك — هو يمسح أدوات التصفية فقط.
    /// </summary>
    private async Task _RefreshAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();        // التصفية تعود للافتراضي
            await _LoadAccountsAsync();  // قد يتغيّر دليل الحسابات من شاشة الحسابات
            await _LoadEntriesAsync();   // إعادة قراءة القيود (يعيد تطبيق التصفية والملخّص والتحديد)
            _ResetForm();                // تفريغ المحرر + إلغاء التحديد
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث شاشة القيود المحاسبية");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── الاختصارات ───────────────────────────────────────────────────────────

    /// <summary>Esc: داخل صندوق التصفية يمسح التصفية (بلا إعادة تحميل — RULE-N148)، وخارجها يُغلق الشاشة.</summary>
    private void _HandleEscape()
    {
        if (gbFilter.ContainsFocus || txtFilterValue.Focused)
        {
            _ClearAllFilters();
            return;
        }

        Close();
    }

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147): وضع الإضافة (بلا سجل) ⇒ «حفظ» · مسودة محمَّلة ⇒ «تعديل».
    /// يُقرأ عند كل ضغطة Enter وعند كل انتقال حالة.
    /// <para>
    /// <b>يُعاد <c>null</c> — أي لا فعل أساسي لـ Enter — في ثلاث حالات (RULE-N153):</b>
    /// (1) التركيز داخل شبكة السطور <c>dgvLines</c> أو خلية قيد التحرير — Enter هناك مكان الإدخال؛
    /// (2) التركيز داخل شبكة القيود <c>dgvEntries</c> — Enter فيها تنقّل بين السطور؛
    /// (3) القيد المُرحَّل/الملغى — للقراءة فقط فلا يُحفظ ولا يُرحَّل (RULE-004 / §2.4).
    /// </para>
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (dgvLines.ContainsFocus || dgvLines.IsCurrentCellInEditMode) return null;   // RULE-N153
        if (dgvEntries.ContainsFocus) return null;

        if (_busy) return null;

        // قيد محمَّل غير مسودة = مقفل للقراءة فقط ⇒ لا زر أساسي.
        if (_currentEntryId is not null && _currentStatus != StatusDraft) return null;

        return _currentEntryId is null ? btnSave : btnEdit;
    }

    /// <summary>
    /// Delete: داخل شبكة البنود يحذف **البند** المحدد (الشبكة مكان الإدخال — RULE-N153)،
    /// وداخل حقل إدخال لا يفعل شيئاً، وخارجها يحذف مسودة القيد المحدد.
    /// </summary>
    private void _HandleDeleteShortcut()
    {
        if (dgvLines.ContainsFocus)
        {
            if (btnRemoveLine.Enabled) btnRemoveLine.PerformClick();
            return;
        }

        if (KeyboardHelper.IsTextInputFocused(this)) return;
        if (!btnDeleteDraft.Enabled) return;

        btnDeleteDraft.PerformClick();
    }

    // ── قائمة السياق (RULE-N23) ──────────────────────────────────────────────

    private void cmsEdit_Click(object? sender, EventArgs e)
    {
        if (btnEdit.Enabled)
            btnEdit.PerformClick();
    }

    private void cmsPost_Click(object? sender, EventArgs e) => btnPost.PerformClick();

    private void cmsCancel_Click(object? sender, EventArgs e) => btnCancelEntry.PerformClick();

    private void cmsDeleteDraft_Click(object? sender, EventArgs e) => btnDeleteDraft.PerformClick();

    private void cmsRefresh_Click(object? sender, EventArgs e) => _ = _RefreshAsync();

    // ── أدوات ────────────────────────────────────────────────────────────────

    /// <summary>
    /// بصمة المحرر (التاريخ · النوع · البيان · المرجع · بنود الشبكة) — تُقارن ببصمة آخر تحميل/حفظ
    /// لكشف التعديلات غير المحفوظة قبل الترحيل. تعتمد التنسيق الثابت للثقافة حتى لا تتغيّر البصمة
    /// بتغيّر إعدادات العرض، وهي للمقارنة الداخلية فقط ولا تُعرض للمستخدم.
    /// </summary>
    private string _EditorSignature()
    {
        string lines = string.Join('|', _lines.Select(l =>
            FormattableString.Invariant($"{l.AccountId}:{l.Debit}:{l.Credit}:{l.Description}")));

        return FormattableString.Invariant(
            $"{dtpEntryDate.Value.Date.Ticks}|{_CurrentEntryType()}|{txtEntryDescription.Text.Trim()}|{txtReferenceNumber.Text.Trim()}|{lines}");
    }

    /// <summary>
    /// نص حالة القيد للعرض — يُفوَّض إلى البيت الواحد <see cref="DocumentTextHelper.StatusText(byte)"/>
    /// (RULE-N171/N172): كانت هذه الدالة تكرّر الترجمة محلياً بشكل مختلف عن بقية الشاشات.
    /// </summary>
    private static string _StatusText(int status) => DocumentTextHelper.StatusText((byte)status);
}
