using System.Data;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// شاشة «كشف حساب مورد» — نموذج واجهة (UI Shape) يعمل ببيانات وهمية في الذاكرة
/// دون أي اتصال بقاعدة البيانات (وضع التجربة / UI-First).
/// <para>
/// الرصيد الجاري يُحسب تراكمياً حسب الترتيب الزمني (منطق محفوظ كما هو):
/// فاتورة شراء = مدين (تزيد الرصيد) · دفع / مردود = دائن (تخفض الرصيد).
/// </para>
/// <para>§2.22 — التصفية داخل <c>gbFilter</c> فورية عبر <see cref="DataView.RowFilter"/> و<see cref="RowFilterHelper"/>.</para>
/// </summary>
public partial class frmSupplierStatement : Form
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>RULE-N46: لون الحقل النشط — القيمة المعتمدة في §2.21.</summary>
    private static readonly Color FocusColor = Color.FromArgb(0xD6, 0xE4, 0xF0);

    private const string RowKindOpening = "Opening";
    private const string RowKindNormal = "Normal";
    private const string NoValue = "—";

    /// <summary>اسم المورد المعروض (موضوع الكشف) — قيمة وضع التجربة يُعاد قراءتها عند «تحديث» (RULE-N148).</summary>
    private const string StatementSupplierName = "شركة النور للتجارة";

    // ── نموذج حركة الكشف (بيانات وهمية في الذاكرة) ────────────────────────────

    private sealed class StatementLine
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";        // فاتورة / دفع / مردود
        public string Reference { get; set; } = "";
        public decimal Debit { get; set; }            // مدين (فاتورة شراء)
        public decimal Credit { get; set; }           // دائن (دفع / مردود)
    }

    private readonly List<StatementLine> _allLines = new()
    {
        new() { Date = new DateTime(2026, 9, 1),  Type = "فاتورة", Reference = "مشتريات-2026-0001", Debit = 250000.00m },
        new() { Date = new DateTime(2026, 9, 3),  Type = "دفع",    Reference = "سند-صرف-2026-0142", Credit = 100000.00m },
        new() { Date = new DateTime(2026, 9, 7),  Type = "فاتورة", Reference = "مشتريات-2026-0005", Debit = 180000.00m },
        new() { Date = new DateTime(2026, 9, 10), Type = "مردود",  Reference = "مرتجع-2026-0002",    Credit = 25000.00m },
        new() { Date = new DateTime(2026, 9, 12), Type = "دفع",    Reference = "سند-صرف-2026-0150", Credit = 200000.00m },
        new() { Date = new DateTime(2026, 9, 15), Type = "فاتورة", Reference = "مشتريات-2026-0009", Debit = 320000.00m },
        new() { Date = new DateTime(2026, 9, 18), Type = "دفع",    Reference = "سند-صرف-2026-0161", Credit = 150000.00m },
        new() { Date = new DateTime(2026, 9, 20), Type = "فاتورة", Reference = "مشتريات-2026-0012", Debit = 95000.00m },
    };

    // ── حالة العرض: جدول العرض + عرضه المصفّى (RULE-N24/N59/N60) ──────────────
    private DataTable _table = new();
    private DataView _view = null!;

    /// <summary>يمنع التصفية/إعادة البناء الفورية أثناء التهيئة (RULE-N59).</summary>
    private bool _isInitializing = true;

    // ── RULE-N148: النطاق الافتراضي الذي تعود إليه «مسح التصفية» ───────────────
    private DateTime _defaultFrom;
    private DateTime _defaultTo;

    public frmSupplierStatement(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…RULE-N47 + RULE-N95 — الستة من KeyboardHelper.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);                    // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                               // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnRefresh, btnExport, btnClose, btnClearFilters); // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                // RULE-N47
            (Keys.F5, () => btnRefresh.PerformClick()),                        // RULE-N148: F5 = تحديث كامل
            (Keys.Escape, _GlobalEscape));

        // Enter في حقل التصفية = تطبيق فوري (RULE-N59).
        KeyboardHelper.WireEnterToClick(txtFilterValue, _ApplyFilter);

        // RULE-N147: Enter = الزر الأساسي للشاشة — «تحديث الكشف» (عرض الكشف).
        KeyboardHelper.WireModeEnter(this, () => btnRefresh);
        AcceptButton = btnRefresh;

        // RULE-N60: أدوات التصفية تُقرأ بالمؤشر — «تصفية بـ: النوع» افتراضياً.
        cbFilterBy.SelectedIndex = 0;

        // RULE-N148: التقاط النطاق الافتراضي ليعود إليه «مسح التصفية» بلا لمس البيانات.
        _CaptureDefaultRange();

        ApplyUiTheme(); // RULE-N50

        // طيّ/إظهار صندوق التصفية (RULE-N63) — آخر سطر في المُنشئ.
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    // ═══════════════════════════════════════════════════════════
    // الثيم (RULE-N50…RULE-N54)
    // ═══════════════════════════════════════════════════════════

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply</c> آخر سطر (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);

        ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnExport, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnRangeToday, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnRangeMonth, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnRange30, ThemeHelper.ButtonType.Secondary);

        // ThemeHelper يفرض Segoe UI ويوسّع الأزرار ⇒ نثبّت Tahoma والأحجام (RULE-N21).
        foreach (var btn in new[] { btnRefresh, btnExport, btnClose })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btn.Size = new Size(134, 38);
            btn.Margin = new Padding(4);
        }

        btnClose.Size = new Size(110, 38);
        btnRefresh.ForeColor = Color.White;
        btnExport.ForeColor = Color.White;

        foreach (var btn in new[] { btnRangeToday, btnRangeMonth, btnRange30 })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.MinimumSize = Size.Empty;
            btn.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btn.Height = 30;
        }

        btnRangeToday.Size = new Size(100, 30);
        btnRangeMonth.Size = new Size(103, 30);
        btnRange30.Size = new Size(123, 30);

        // تلميحات الأزرار مع اختصاراتها (RULE-N47).
        toolTip1.SetToolTip(btnRefresh, "إعادة بناء الكشف من المصدر (F5)");
        toolTip1.SetToolTip(btnExport, "تصدير الكشف المعروض إلى Excel");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
        toolTip1.SetToolTip(btnRangeToday, "حركات اليوم فقط");
        toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
        toolTip1.SetToolTip(cbFilterBy, "الحقل الذي تُصفّى به الحركات");
        toolTip1.SetToolTip(txtFilterValue, "تصفية فورية — Enter يطبّق، Esc يمسح كل التصفية");
        toolTip1.SetToolTip(dtpFrom, "بداية نطاق تاريخ الحركة");
        toolTip1.SetToolTip(dtpTo, "نهاية نطاق تاريخ الحركة");
        toolTip1.SetToolTip(chkOnlyDue, "إظهار الحركات التي يكون فيها الرصيد التراكمي مديناً فقط");
        toolTip1.SetToolTip(dgvStatement, "زر الفأرة الأيمن: تفاصيل الحركة · تصدير Excel · تحديث");

        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvStatement);

        // RULE-N22/RULE-N52 — سلوك شبكة العرض تعلنه الشاشة نفسها.
        dgvStatement.ReadOnly = true;
        dgvStatement.AllowUserToAddRows = false;
        dgvStatement.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStatement.MultiSelect = false;

        lblSummary.ForeColor = GreenThemeHelper.TextStrong;   // RULE-N53
        lblClosingCaption.ForeColor = GreenThemeHelper.TextStrong;

        // RULE-N97 — التفقيط الحيّ لرصيد الكشف (حقل قراءة فقط قابل للتركيز).
        LiveTafqeetHelper.Attach(tipTafqeet,
            (txtClosingBalance, "الرصيد", LiveTafqeetHelper.ReadMode.Money));

        emptyState.BringToFront();

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ═══════════════════════════════════════════════════════════
    // التحميل الأول
    // ═══════════════════════════════════════════════════════════

    private async void frmSupplierStatement_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _SetupTable();
            _SetupGridColumns();
            _BindGrid();

            // لا يوجد اتصال بيانات (وضع تجربة) — الإبقاء على نمط المعالجات غير المتزامنة.
            await Task.Yield();

            _ReloadLookups();
            _SetDefaultRange();
            _CaptureDefaultRange();       // RULE-N148: الافتراضي المعتمد = النطاق الكامل للكشف
            _RefreshStatement();
            _UpdateClearFiltersState();   // RULE-N148: الزر معطّل عند الفتح (لا تصفية نشطة)
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحميل كشف حساب المورد");
            clsDialogs.Error("تعذر تحميل كشف الحساب. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // بناء جدول العرض والأعمدة
    // ═══════════════════════════════════════════════════════════

    /// <summary>أعمدة جدول العرض: نصوص للعرض + أرقام مساعدة للملخص والتصفية.</summary>
    private void _SetupTable()
    {
        _table = new DataTable("SupplierStatement");
        _table.Columns.Add("RowKind", typeof(string));
        _table.Columns.Add("DateText", typeof(string));
        _table.Columns.Add("TypeText", typeof(string));
        _table.Columns.Add("ReferenceText", typeof(string));
        _table.Columns.Add("DebitText", typeof(string));
        _table.Columns.Add("CreditText", typeof(string));
        _table.Columns.Add("BalanceText", typeof(string));
        _table.Columns.Add("DebitValue", typeof(decimal));
        _table.Columns.Add("CreditValue", typeof(decimal));
        _table.Columns.Add("BalanceValue", typeof(decimal));
    }

    private void _BindGrid()
    {
        _view = new DataView(_table);
        dgvStatement.AutoGenerateColumns = false;
        dgvStatement.DataSource = _view;
    }

    /// <summary>ينشئ عمود عرض مرتبطاً بعمود الجدول المعلن (RULE-N22).</summary>
    private static DataGridViewTextBoxColumn _MakeColumn(
        string name, string header, string property, int fillWeight, int minWidth,
        DataGridViewContentAlignment alignment)
    {
        return new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = header,
            DataPropertyName = property,
            FillWeight = fillWeight,
            MinimumWidth = minWidth,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = alignment }
        };
    }

    private void _SetupGridColumns()
    {
        dgvStatement.AutoGenerateColumns = false;
        dgvStatement.Columns.Clear();

        dgvStatement.Columns.Add(_MakeColumn("ColDate", "التاريخ", "DateText",
            13, 90, DataGridViewContentAlignment.MiddleCenter));
        dgvStatement.Columns.Add(_MakeColumn("ColType", "النوع", "TypeText",
            11, 85, DataGridViewContentAlignment.MiddleCenter));
        dgvStatement.Columns.Add(_MakeColumn("ColReference", "المرجع", "ReferenceText",
            17, 110, DataGridViewContentAlignment.MiddleCenter));
        dgvStatement.Columns.Add(_MakeColumn("ColDebit", "مدين", "DebitText",
            14, 90, DataGridViewContentAlignment.MiddleRight));
        dgvStatement.Columns.Add(_MakeColumn("ColCredit", "دائن", "CreditText",
            14, 90, DataGridViewContentAlignment.MiddleRight));
        dgvStatement.Columns.Add(_MakeColumn("ColBalance", "الرصيد", "BalanceText",
            15, 100, DataGridViewContentAlignment.MiddleRight));
    }

    // ═══════════════════════════════════════════════════════════
    // بناء الكشف (الرصيد الافتتاحي والتراكمي — منطق محفوظ كما هو)
    // ═══════════════════════════════════════════════════════════

    private void _RefreshStatement()
    {
        if (_isInitializing || _view is null) return;

        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;

        // حماية النطاق: لا نبني كشفاً بنطاق معكوس.
        if (from > to)
        {
            clsDialogs.Warning("تاريخ البداية «من» يجب أن يكون قبل تاريخ النهاية «إلى» أو مساوياً له.");
            return;
        }

        Cursor = Cursors.WaitCursor;
        try
        {
            // الرصيد السابق = صافي حركات المورد قبل تاريخ البداية (يُحمَل من فترة سابقة)
            decimal opening = _allLines.Where(l => l.Date < from).Sum(l => l.Debit - l.Credit);

            var visible = _allLines
                .Where(l => l.Date >= from && l.Date <= to)
                .OrderBy(l => l.Date)
                .ToList();

            _table.Rows.Clear();

            // صف الرصيد السابق (افتتاحي) إن وُجد
            if (opening != 0)
            {
                _table.Rows.Add(
                    RowKindOpening,
                    clsFormat.DateToDayMonthYear(from),
                    "رصيد سابق",
                    NoValue,
                    string.Empty,
                    string.Empty,
                    clsFormat.Money(opening),
                    0m,
                    0m,
                    opening);
            }

            // الحركات مع الرصيد الجاري التراكمي
            decimal running = opening;
            foreach (var line in visible)
            {
                running += line.Debit - line.Credit;

                _table.Rows.Add(
                    RowKindNormal,
                    clsFormat.DateToDayMonthYear(line.Date),
                    line.Type,
                    line.Reference,
                    line.Debit > 0 ? clsFormat.Money(line.Debit) : string.Empty,
                    line.Credit > 0 ? clsFormat.Money(line.Credit) : string.Empty,
                    clsFormat.Money(running),
                    line.Debit,
                    line.Credit,
                    running);
            }

            _ApplyFilter();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر بناء كشف حساب المورد");
            clsDialogs.Error("تعذر بناء كشف الحساب. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // التصفية الفورية (RULE-N24 · RULE-N59 · RULE-N60)
    // ═══════════════════════════════════════════════════════════

    private void _ApplyFilter()
    {
        if (_view is null) return;

        var conditions = new List<string>();

        // (1) البحث النصي — القيمة تُهرَّب قبل بنائها في RowFilter، وأسماء الأعمدة ثابتة.
        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            conditions.Add(cbFilterBy.SelectedIndex switch
            {
                1 => RowFilterHelper.Like("ReferenceText", text),
                _ => RowFilterHelper.Like("TypeText", text)
            });
        }

        // (2) «المستحقة فقط» = الرصيد التراكمي مدين (RULE-N58).
        if (chkOnlyDue.Checked) conditions.Add("BalanceValue > 0");

        try
        {
            _view.RowFilter = conditions.Count == 0 ? string.Empty : string.Join(" AND ", conditions);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق تصفية كشف حساب المورد");
            _view.RowFilter = string.Empty;
        }

        _UpdateSummary();
        _UpdateClearFiltersState();   // RULE-N43/RULE-N148: حالة الزر تتبع وجود تصفية نشطة
    }

    /// <summary>شريط الملخص (RULE-N61): عدد الحركات + مدين + دائن + الرصيد.</summary>
    private void _UpdateSummary()
    {
        int moves = 0;
        decimal debit = 0m;
        decimal credit = 0m;
        decimal closing = 0m;
        bool any = false;

        if (_view is not null)
        {
            foreach (DataRowView row in _view)
            {
                if (row["RowKind"] as string == RowKindNormal) moves++;

                debit += (decimal)row["DebitValue"];
                credit += (decimal)row["CreditValue"];
                closing = (decimal)row["BalanceValue"];
                any = true;
            }
        }

        if (!any) closing = 0m;

        lblSummary.Text = $"عدد الحركات: {moves} · مدين: {debit:N2} · دائن: {credit:N2}";
        txtClosingBalance.Text = clsFormat.Money(closing);

        emptyState.Visible = _view is null || _view.Count == 0;
        if (emptyState.Visible) emptyState.BringToFront();
    }

    // ═══════════════════════════════════════════════════════════
    // «مسح التصفية» مقابل «تحديث» (RULE-N148)
    // ═══════════════════════════════════════════════════════════

    /// <summary>RULE-N148: مسح أدوات التصفية ثم إعادة التركيز على حقل التصفية.</summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>
    /// يمسح **أدوات التصفية فقط** ويعيدها إلى الافتراضي، بلا أي إعادة تحميل وبلا لمس المحرر أو
    /// البيانات (RULE-N148): حقل التصفية · «تصفية بـ» · «المستحقة فقط» · نطاق التاريخ.
    /// <para>
    /// **لا يمس المورد المعروض** (<c>lblSupplierValue</c>) لأنه موضوع الكشف لا أداة تصفية،
    /// ولا يمس السطر المحدد في الشبكة.
    /// </para>
    /// </summary>
    private void _ClearFilterInputs()
    {
        _isInitializing = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Clear();
            chkOnlyDue.Checked = false;
            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _isInitializing = false;
        }
    }

    /// <summary>مسح + إعادة تطبيق على البيانات المحمّلة في الذاكرة (بلا إعادة تحميل — RULE-N148).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _RefreshStatement();   // يعيد بناء الصفوف من الذاكرة ويطبّق التصفية والملخص والرصيد
    }

    /// <summary>RULE-N43/RULE-N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية تختلف عن الافتراضي.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || cbFilterBy.SelectedIndex > 0
            || chkOnlyDue.Checked
            || dtpFrom.Value.Date != _defaultFrom
            || dtpTo.Value.Date != _defaultTo;
    }

    /// <summary>إعادة قراءة الدلائل (المورد المعروض) — ثابت في وضع التجربة، والبنية جاهزة لوصل الخدمة.</summary>
    private void _ReloadLookups()
    {
        lblSupplierValue.Text = StatementSupplierName;
    }

    /// <summary>يلتقط النطاق الافتراضي المعتمد الذي يعود إليه «مسح التصفية» (RULE-N148).</summary>
    private void _CaptureDefaultRange()
    {
        _defaultFrom = dtpFrom.Value.Date;
        _defaultTo = dtpTo.Value.Date;
    }

    /// <summary>RULE-N47 + RULE-N59 + RULE-N148: Esc داخل صندوق التصفية = مسحها، وخارجه = إغلاق الشاشة.</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        Close();
    }

    // ═══════════════════════════════════════════════════════════
    // النطاقات الزمنية
    // ═══════════════════════════════════════════════════════════

    /// <summary>النطاق الافتراضي: كامل فترة بيانات الكشف المتاحة.</summary>
    private void _ApplyDefaultRangeCore()
    {
        if (_allLines.Count == 0) return;

        dtpFrom.Value = _allLines.Min(l => l.Date);
        dtpTo.Value = _allLines.Max(l => l.Date);
    }

    private void _SetDefaultRange()
    {
        _isInitializing = true;
        try
        {
            _ApplyDefaultRangeCore();
        }
        finally
        {
            _isInitializing = false;
        }
    }

    private void _SetDateRange(DateTime from, DateTime to)
    {
        _isInitializing = true;
        try
        {
            dtpFrom.Value = from.Date;
            dtpTo.Value = to.Date;
        }
        finally
        {
            _isInitializing = false;
        }

        _RefreshStatement();
    }

    // ═══════════════════════════════════════════════════════════
    // أحداث الواجهة
    // ═══════════════════════════════════════════════════════════

    private void dtpFrom_ValueChanged(object? sender, EventArgs e) => _RefreshStatement();

    private void dtpTo_ValueChanged(object? sender, EventArgs e) => _RefreshStatement();

    private void txtFilterValue_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyDue_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void btnRangeToday_Click(object? sender, EventArgs e)
        => _SetDateRange(DateTime.Today, DateTime.Today);

    private void btnRangeMonth_Click(object? sender, EventArgs e)
    {
        var today = DateTime.Today;
        _SetDateRange(new DateTime(today.Year, today.Month, 1), today);
    }

    private void btnRange30_Click(object? sender, EventArgs e)
    {
        var today = DateTime.Today;
        _SetDateRange(today.AddDays(-29), today);
    }

    /// <summary>
    /// «تحديث» (F5) — **إعادة تحميل كاملة** (RULE-N148): أدوات التصفية ← الدلائل (المورد) ←
    /// إعادة بناء الكشف ونطاقه الافتراضي من المصدر ← إعادة الملخص والرصيد.
    /// وليس مسح تصفية فقط.
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();   // التصفية تعود للافتراضي
            _ReloadLookups();       // إعادة قراءة المورد المعروض

            // لا قراءة خدمة في وضع التجربة — الإبقاء على نمط المعالجات غير المتزامنة.
            await Task.Yield();

            _SetDefaultRange();     // النطاق الافتراضي الكامل من المصدر
            _RefreshStatement();    // إعادة بناء الكشف + تطبيق التصفية + الملخص والرصيد
            _UpdateClearFiltersState();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحديث كشف حساب المورد");
            clsDialogs.Error("تعذر تحديث الكشف. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    private void tsmiRefresh_Click(object? sender, EventArgs e) => btnRefresh_Click(sender, e);

    private void btnClose_Click(object sender, EventArgs e) => Close();

    // ═══════════════════════════════════════════════════════════
    // تفاصيل الحركة (نقر مزدوج / عنصر القائمة)
    // ═══════════════════════════════════════════════════════════

    private void dgvStatement_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        _ShowSelectedRowDetails();
    }

    private void tsmiDetails_Click(object? sender, EventArgs e) => _ShowSelectedRowDetails();

    private void _ShowSelectedRowDetails()
    {
        try
        {
            var row = dgvStatement.CurrentRow;
            if (row is null) return;

            clsDialogs.Info(
                $"التاريخ: {row.Cells["ColDate"].Value}\n" +
                $"النوع: {row.Cells["ColType"].Value}\n" +
                $"المرجع: {row.Cells["ColReference"].Value}\n" +
                $"مدين: {row.Cells["ColDebit"].Value}\n" +
                $"دائن: {row.Cells["ColCredit"].Value}\n" +
                $"الرصيد: {row.Cells["ColBalance"].Value}");
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر عرض تفاصيل حركة كشف حساب المورد");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // التصدير (RULE-N23 · RULE-N112)
    // ═══════════════════════════════════════════════════════════

    private void btnExport_Click(object? sender, EventArgs e) => _ExportToExcel();

    private void tsmiExport_Click(object? sender, EventArgs e) => _ExportToExcel();

    private void _ExportToExcel()
    {
        try
        {
            if (dgvStatement.Rows.Count == 0)
            {
                clsDialogs.Warning("لا توجد بيانات معروضة للتصدير.");
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"كشف_حساب_مورد_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                Title = "تصدير كشف الحساب إلى Excel"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            ExportHelper.ExportToExcel(dgvStatement, "كشف حساب مورد", dialog.FileName);

            clsDialogs.Success("تم تصدير البيانات بنجاح.");
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "خطأ أثناء تصدير كشف حساب المورد إلى Excel");
            clsDialogs.Error("تعذر تصدير البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // تنسيق الشبكة (ألوان من الـHelpers فقط — RULE-N54)
    // ═══════════════════════════════════════════════════════════

    private void dgvStatement_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvStatement.Rows.Count) return;
        if (e.ColumnIndex < 0 || e.ColumnIndex >= dgvStatement.Columns.Count) return;

        bool isOpeningRow =
            dgvStatement.Rows[e.RowIndex].DataBoundItem is DataRowView view
            && view["RowKind"] as string == RowKindOpening;

        if (isOpeningRow)
        {
            e.CellStyle.BackColor = GreenThemeHelper.GridAlternate;
            e.CellStyle.ForeColor = GreenThemeHelper.TextStrong;
            e.CellStyle.Font = new Font(dgvStatement.Font, FontStyle.Bold);
            return;
        }

        string type = dgvStatement.Rows[e.RowIndex].Cells["ColType"].Value?.ToString() ?? "";

        switch (dgvStatement.Columns[e.ColumnIndex].Name)
        {
            case "ColType":
                e.CellStyle.Font = new Font(dgvStatement.Font, FontStyle.Bold);
                e.CellStyle.ForeColor = type switch
                {
                    "فاتورة" => ThemeHelper.Primary,
                    "دفع" => ThemeHelper.Success,
                    "مردود" => ThemeHelper.Danger,
                    _ => GreenThemeHelper.TextStrong
                };
                break;

            case "ColDebit":
                if (e.Value is string debitText && debitText.Length > 0)
                {
                    e.CellStyle.ForeColor = ThemeHelper.Primary;
                    e.CellStyle.Font = new Font(dgvStatement.Font, FontStyle.Bold);
                }
                break;

            case "ColCredit":
                if (e.Value is string creditText && creditText.Length > 0)
                {
                    e.CellStyle.ForeColor = ThemeHelper.Success;
                    e.CellStyle.Font = new Font(dgvStatement.Font, FontStyle.Bold);
                }
                break;

            case "ColBalance":
                e.CellStyle.Font = new Font(dgvStatement.Font, FontStyle.Bold);
                e.CellStyle.ForeColor = (e.Value?.ToString() ?? "").StartsWith("-", StringComparison.Ordinal)
                    ? ThemeHelper.Danger
                    : GreenThemeHelper.TextStrong;
                break;
        }
    }
}
