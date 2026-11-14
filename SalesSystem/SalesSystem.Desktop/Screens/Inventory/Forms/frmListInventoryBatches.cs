using System.Data;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// شاشة عرض دفعات المخزون (قراءة فقط).
/// التصفية موحّدة داخل <c>gbFilter</c> (§2.22) وفورية بلا زر بحث،
/// والتاريخ المستخدم في النطاق الزمني هو <b>تاريخ انتهاء الدفعة (ExpiryDate)</b>.
/// </summary>
public partial class frmListInventoryBatches : Form
{
    /// <summary>لون تظليل الحقل النشط (RULE-N46).</summary>
    private static Color FocusColor => ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor);

    /// <summary>عدد الأيام التي تُعدّ الدفعة بعده «قاربت الانتهاء».</summary>
    private const int ExpiryAlertDays = 30;

    /// <summary>العمود البديل للدفعات بلا تاريخ انتهاء.</summary>
    private const string NoExpiryToken = "-";

    private readonly IServiceProvider _serviceProvider;

    private DataTable _batchesTable = new();
    private DataView _batchesView = new();

    /// <summary>يمنع انطلاق التصفية من داخل المنشئ/التحميل قبل اكتمال التهيئة.</summary>
    private bool _isInitializing = true;

    /// <summary>يمنع تحميلين متزامنين (تغيير المنتج أثناء التحميل).</summary>
    private bool _isLoading;

    /// <summary>
    /// نطاق تاريخ الانتهاء لا يُطبَّق إلا بعد أن يلمسه المستخدم؛
    /// لأن معظم الدفعات تنتهي مستقبلاً وقد يخفيها نطاق «هذا الشهر» فور فتح الشاشة.
    /// </summary>
    private bool _dateRangeActive;

    /// <summary>قيمة «من» الافتراضية لتاريخ التصفية كما تُفتح بها الشاشة — تُعاد بها «مسح التصفية» (RULE-N148).</summary>
    private DateTime _defaultRangeFrom;

    /// <summary>قيمة «إلى» الافتراضية لتاريخ التصفية كما تُفتح بها الشاشة — تُعاد بها «مسح التصفية» (RULE-N148).</summary>
    private DateTime _defaultRangeTo;

    public frmListInventoryBatches(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // قيمتا تاريخ التصفية الافتراضيتان تُلتقطان عند البناء (قبل أي تفاعل) — RULE-N148.
        _SetRangeToThisMonth();
        _defaultRangeFrom = dtpFrom.Value;
        _defaultRangeTo = dtpTo.Value;

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط وأسهم RTL.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);
        KeyboardHelper.WireArrowNavigation(this);

        _ApplyUiTheme();

        // RULE-N43 / RULE-N47 — مظهر التعطيل واختصارات الشاشة.
        KeyboardHelper.WireButtonDisabledVisual(btnRefresh, btnClose, btnClearFilters);
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, () => _ClearAllFilters()));

        // RULE-N147 — Enter = الزر الأساسي؛ شاشة عرض بلا «جديد» ⇒ «تحديث».
        KeyboardHelper.WireModeEnter(this, () => btnRefresh);
        AcceptButton = btnRefresh;

        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    // ── Theme & Shortcuts ──────────────────────────────────────────────────────────

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply</c> آخر خطوة على النموذج (RULE-N50).</summary>
    private void _ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);

        ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnRangeToday, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnRange30, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnRangeMonth, ThemeHelper.ButtonType.Neutral);

        // ThemeHelper يفرض Segoe UI ويوسّع الأزرار — نثبّت Tahoma والأحجام (RULE-N21).
        foreach (var btn in new[] { btnRefresh, btnClose })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btn.Size = new Size(124, 36);
            btn.Margin = new Padding(4, 0, 4, 0);
        }

        foreach (var btn in new[] { btnRangeToday, btnRange30, btnRangeMonth })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.MinimumSize = Size.Empty;
            btn.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        }
        btnRangeToday.Size = new Size(100, 30);
        btnRange30.Size = new Size(128, 30);
        btnRangeMonth.Size = new Size(100, 30);

        toolTip1.SetToolTip(btnRefresh, "إعادة تحميل الدفعات من المصدر (F5)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة");
        toolTip1.SetToolTip(btnRangeToday, "تاريخ انتهاء الدفعة = اليوم");
        toolTip1.SetToolTip(btnRangeMonth, "تاريخ انتهاء الدفعة من بداية الشهر حتى اليوم");
        toolTip1.SetToolTip(btnRange30, "تاريخ انتهاء الدفعة خلال آخر 30 يوماً");
        toolTip1.SetToolTip(txtFilterValue, "تصفية فورية — Enter يطبّق و Esc يمسح كل التصفية");
        toolTip1.SetToolTip(cbFilterBy, "العمود الذي يُبحث فيه");
        toolTip1.SetToolTip(cbStatus, "حالة الصلاحية المشتقة من تاريخ الانتهاء");
        toolTip1.SetToolTip(dtpFrom, "بداية نطاق تاريخ الانتهاء");
        toolTip1.SetToolTip(dtpTo, "نهاية نطاق تاريخ الانتهاء");

        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvBatches);

        // RULE-N22 / RULE-N52 — سلوك شبكة العرض تعلنه الشاشة نفسها.
        dgvBatches.ReadOnly = true;
        dgvBatches.AllowUserToAddRows = false;
        dgvBatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBatches.MultiSelect = false;
        dgvBatches.AutoGenerateColumns = false;

        GreenThemeHelper.Apply(this); // LAST (RULE-N50)
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────────

    private DataGridViewColumn _MakeColumn(string name, string header, int fill, int min, string? format = null)
    {
        var col = new DataGridViewTextBoxColumn
        {
            Name = name, HeaderText = header, DataPropertyName = name,
            MinimumWidth = min, FillWeight = fill, ReadOnly = true,
        };
        if (format is not null) col.DefaultCellStyle.Format = format;
        return col;
    }

    private void _SetupGridColumns()
    {
        dgvBatches.Columns.Clear();

        dgvBatches.Columns.Add(_MakeColumn("Id", "رقم", 4, 50));
        dgvBatches.Columns.Add(_MakeColumn("ProductName", "المنتج", 18, 180));
        dgvBatches.Columns.Add(_MakeColumn("BatchNo", "رقم الدفعة", 10, 120));
        dgvBatches.Columns.Add(_MakeColumn("WarehouseName", "المستودع", 10, 120));
        dgvBatches.Columns.Add(_MakeColumn("QuantityReceived", "الكمية المستلمة", 10, 100, "N3"));
        dgvBatches.Columns.Add(_MakeColumn("QuantityRemaining", "الكمية المتبقية", 10, 100, "N3"));
        dgvBatches.Columns.Add(_MakeColumn("UnitCost", "تكلفة الوحدة", 8, 90, "N2"));
        dgvBatches.Columns.Add(_MakeColumn("TotalValue", "القيمة الإجمالية", 10, 100, "N2"));
        dgvBatches.Columns.Add(_MakeColumn("ExpiryDate", "تاريخ الانتهاء", 8, 90));
        dgvBatches.Columns.Add(_MakeColumn("ExpiryStatus", "حالة الصلاحية", 8, 80));
    }

    // ── Data Loading ───────────────────────────────────────────────────────────────

    private async void frmListInventoryBatches_Load(object sender, EventArgs e)
    {
        try
        {
            _isInitializing = true;
            try
            {
                cbFilterBy.SelectedIndex = 0;
                cbStatus.SelectedIndex = 0;
                _SetRangeToThisMonth();
                _dateRangeActive = false;
            }
            finally
            {
                _isInitializing = false;
            }

            await _ReloadFormAsync();
            _UpdateClearFiltersState();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تهيئة شاشة دفعات المخزون");
            clsDialogs.Error("تعذر تهيئة الشاشة. يرجى إعادة المحاولة.");
        }
    }

    private async Task _ReloadFormAsync()
    {
        bool previous = _isInitializing;
        _isInitializing = true;
        try
        {
            _SetupGridColumns();
            await _LoadProductsAsync();
        }
        finally
        {
            _isInitializing = previous;
        }

        await _RefreshListAsync();
    }

    private async Task _LoadProductsAsync()
    {
        try
        {
            var api = _serviceProvider.GetRequiredService<IProductDesktopService>();
            var result = await api.GetAllAsync();

            if (!result.IsSuccess || result.Value is not { Count: > 0 })
                return;

            var items = new List<KeyValuePair<int, string>>
            {
                new(0, "--- الجميع ---")
            };
            items.AddRange(result.Value.Select(p => new KeyValuePair<int, string>(p.Id, p.Name)));

            cmbProduct.DisplayMember = "Value";
            cmbProduct.ValueMember = "Key";
            cmbProduct.DataSource = items;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة الأصناف في شاشة دفعات المخزون");
            clsDialogs.Error("تعذر تحميل قائمة الأصناف. يرجى إعادة المحاولة.");
        }
    }

    private async Task _RefreshListAsync()
    {
        if (_isLoading)
            return;

        _isLoading = true;
        Cursor = dgvBatches.Cursor = Cursors.WaitCursor;

        try
        {
            var api = _serviceProvider.GetRequiredService<IInventoryBatchDesktopService>();
            var allBatches = new List<InventoryBatchDto>();

            int? filterProductId = cmbProduct.SelectedValue is int pid && pid > 0 ? pid : null;

            if (filterProductId.HasValue)
            {
                var result = await api.GetByProductAsync(filterProductId.Value);
                if (result.IsSuccess && result.Value is not null)
                    allBatches.AddRange(result.Value);
                else if (!result.IsSuccess)
                    clsDialogs.Error(result.Error ?? "تعذر تحميل دفعات الصنف المحدد.");
            }
            else
            {
                // تحميل دفعات كل الأصناف من خلال قائمة الأصناف (حد أقصى 50 صنفاً لتفادي البطء).
                var productApi = _serviceProvider.GetRequiredService<IProductDesktopService>();
                var productsResult = await productApi.GetAllAsync();

                if (productsResult.IsSuccess && productsResult.Value is { Count: > 0 })
                {
                    foreach (var prod in productsResult.Value.Take(50))
                    {
                        var result = await api.GetByProductAsync(prod.Id);
                        if (result.IsSuccess && result.Value is not null)
                            allBatches.AddRange(result.Value);
                    }
                }
            }

            _BuildBatchesTable(allBatches);
            _batchesView = new DataView(_batchesTable);
            dgvBatches.DataSource = _batchesView;

            _ApplyFilter();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل دفعات المخزون");
            clsDialogs.Error("تعذر تحميل دفعات المخزون. يرجى إعادة المحاولة.");
        }
        finally
        {
            _isLoading = false;
            if (!IsDisposed)
                Cursor = dgvBatches.Cursor = Cursors.Default;
        }
    }

    /// <summary>يبني جدول العرض الكامل (بلا تصفية) — التصفية تُطبَّق بعده عبر DataView.RowFilter.</summary>
    private void _BuildBatchesTable(List<InventoryBatchDto> batches)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("ProductName", typeof(string));
        table.Columns.Add("BatchNo", typeof(string));
        table.Columns.Add("WarehouseName", typeof(string));
        table.Columns.Add("QuantityReceived", typeof(decimal));
        table.Columns.Add("QuantityRemaining", typeof(decimal));
        table.Columns.Add("UnitCost", typeof(decimal));
        table.Columns.Add("TotalValue", typeof(decimal));
        table.Columns.Add("ExpiryDate", typeof(string));
        table.Columns.Add("ExpiryStatus", typeof(string));

        foreach (var b in batches)
        {
            table.Rows.Add(
                b.Id,
                b.ProductName ?? string.Empty,
                b.BatchNo,
                b.WarehouseName ?? string.Empty,
                b.QuantityReceived,
                b.QuantityRemaining,
                b.UnitCost,
                b.TotalValue,
                b.ExpiryDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? NoExpiryToken,
                b.ExpiryStatus);
        }

        _batchesTable = table;
    }

    // ── Filtering (§2.22) ──────────────────────────────────────────────────────────

    /// <summary>تعبير «منتهية» — مشتق من تاريخ الانتهاء.</summary>
    private static string _ExpiredCondition => $"ExpiryDate <> '{NoExpiryToken}' AND ExpiryDate <= '{_TodayIso}'";

    /// <summary>تعبير «قاربت الانتهاء» — تنتهي خلال <see cref="ExpiryAlertDays"/> يوماً.</summary>
    private static string _ExpiringSoonCondition =>
        $"ExpiryDate <> '{NoExpiryToken}' AND ExpiryDate > '{_TodayIso}' AND ExpiryDate <= '{_SoonIso}'";

    /// <summary>تعبير «سارية» — غير منتهية (تشمل الدفعات بلا تاريخ انتهاء).</summary>
    private static string _ValidCondition =>
        $"(ExpiryDate = '{NoExpiryToken}' OR ExpiryDate > '{_TodayIso}')";

    private static string _TodayIso => DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static string _SoonIso =>
        DateTime.Today.AddDays(ExpiryAlertDays).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    /// <summary>تصفية فورية على DataView.RowFilter — بلا زر بحث (RULE-N59/N60).</summary>
    private void _ApplyFilter()
    {
        if (_batchesView is null || _batchesView.Table is null)
        {
            _UpdateSummary();
            _UpdateClearFiltersState();
            return;
        }

        var conditions = new List<string>();

        // (1) البحث النصي — يُهرَّب عبر RowFilterHelper قبل بناء التعبير (RULE-N60/N112).
        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            string productName = RowFilterHelper.Like("ProductName", text);
            string batchNo = RowFilterHelper.Like("BatchNo", text);
            string warehouse = RowFilterHelper.Like("WarehouseName", text);

            conditions.Add(cbFilterBy.SelectedIndex switch
            {
                1 => productName,
                2 => batchNo,
                3 => warehouse,
                _ => $"({productName} OR {batchNo} OR {warehouse})"
            });
        }

        // (2) حالة الصلاحية (مشتقة من تاريخ الانتهاء).
        switch (cbStatus.SelectedIndex)
        {
            case 1: conditions.Add(_ValidCondition); break;
            case 2: conditions.Add(_ExpiringSoonCondition); break;
            case 3: conditions.Add(_ExpiredCondition); break;
        }

        // (3) النطاق الزمني على تاريخ انتهاء الدفعة — يُطبَّق بعد أن يلمسه المستخدم.
        if (_dateRangeActive)
        {
            string from = dtpFrom.Value.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string to = dtpTo.Value.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (string.CompareOrdinal(from, to) > 0)
                (from, to) = (to, from);

            conditions.Add($"ExpiryDate <> '{NoExpiryToken}' AND ExpiryDate >= '{from}' AND ExpiryDate <= '{to}'");
        }

        // (4) مربعات الاختيار القياسية (RULE-N58).
        if (chkOnlyExpiringSoon.Checked)
            conditions.Add(_ExpiringSoonCondition);

        if (chkOnlyExpired.Checked)
            conditions.Add(_ExpiredCondition);

        try
        {
            _batchesView.RowFilter = conditions.Count == 0 ? string.Empty : string.Join(" AND ", conditions);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق تصفية دفعات المخزون");
            _batchesView.RowFilter = string.Empty;
        }

        _UpdateSummary();
        _UpdateClearFiltersState();
    }

    /// <summary>شريط الملخص (RULE-N61): المعروض من الإجمالي + الكميات + حالات الانتهاء + القيمة.</summary>
    private void _UpdateSummary()
    {
        int total = _batchesTable.Rows.Count;
        int shown = 0;
        int expired = 0;
        int expiringSoon = 0;
        decimal quantity = 0m;
        decimal value = 0m;

        if (_batchesView?.Table is null)
        {
            lblSummary.Text =
                $"المعروض: 0 من {clsFormat.Number(total)} — إجمالي الكمية: {clsFormat.Quantity(0m)}" +
                $" · منتهية: 0 · قاربت الانتهاء: 0 · إجمالي القيمة: {clsFormat.Money(0m)}";

            _UpdateEmptyState(0);
            return;
        }

        foreach (DataRowView rowView in _batchesView)
        {
            var row = rowView.Row;
            shown++;

            if (row["QuantityRemaining"] is decimal qty)
                quantity += qty;

            if (row["TotalValue"] is decimal amount)
                value += amount;

            string expiry = row["ExpiryDate"] as string ?? NoExpiryToken;
            if (_IsExpired(expiry))
                expired++;
            else if (_IsExpiringSoon(expiry))
                expiringSoon++;
        }

        lblSummary.Text =
            $"المعروض: {clsFormat.Number(shown)} من {clsFormat.Number(total)} — إجمالي الكمية: {clsFormat.Quantity(quantity)}" +
            $" · منتهية: {clsFormat.Number(expired)} · قاربت الانتهاء: {clsFormat.Number(expiringSoon)}" +
            $" · إجمالي القيمة: {clsFormat.Money(value)}";

        _UpdateEmptyState(shown);
    }

    /// <summary>سطر «لا توجد بيانات» فوق الشبكة عند فراغ النتيجة (RULE-N22).</summary>
    private void _UpdateEmptyState(int shown)
    {
        bool empty = shown == 0;

        lblEmpty.Visible = empty;

        if (empty)
            lblEmpty.BringToFront();
    }

    private static bool _IsExpired(string expiryIso)
        => !string.Equals(expiryIso, NoExpiryToken, StringComparison.Ordinal)
           && string.CompareOrdinal(expiryIso, _TodayIso) <= 0;

    private static bool _IsExpiringSoon(string expiryIso)
        => !string.Equals(expiryIso, NoExpiryToken, StringComparison.Ordinal)
           && string.CompareOrdinal(expiryIso, _TodayIso) > 0
           && string.CompareOrdinal(expiryIso, _SoonIso) <= 0;

    // ── Filter reset (RULE-N148) ───────────────────────────────────────────────────

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس لبيانات المصدر (RULE-N148). «تحديث» هو المسؤول عن إعادة التحميل.
    /// </summary>
    private void btnClearFilters_Click(object sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>
    /// تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح التصفية» و«تحديث» (RULE-N148).
    /// التاريخان يعودان إلى قيمتهما الافتراضية الملتقطة عند البناء، والنطاق الزمني يعود غير مفعّل.
    /// </summary>
    private void _ClearFilterInputs()
    {
        bool previous = _isInitializing;
        _isInitializing = true;
        try
        {
            txtFilterValue.Clear();
            cbFilterBy.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
            chkOnlyExpiringSoon.Checked = false;
            chkOnlyExpired.Checked = false;
            dtpFrom.Value = _defaultRangeFrom;
            dtpTo.Value = _defaultRangeTo;
            _dateRangeActive = false;
        }
        finally
        {
            _isInitializing = previous;
        }
    }

    /// <summary>يمسح كل أدوات التصفية ثم يُعيد تطبيق الوضع الافتراضي على الشبكة — بلا أي إعادة تحميل (RULE-N148).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
    }

    /// <summary>RULE-N43 / RULE-N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية تخالف الافتراضي.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || cbFilterBy.SelectedIndex > 0
            || cbStatus.SelectedIndex > 0
            || chkOnlyExpiringSoon.Checked
            || chkOnlyExpired.Checked
            || _dateRangeActive;
    }

    private void _SetRangeToThisMonth()
    {
        var today = DateTime.Today;
        dtpFrom.Value = new DateTime(today.Year, today.Month, 1);
        dtpTo.Value = today;
    }

    private void _SetDateRange(DateTime from, DateTime to)
    {
        _isInitializing = true;
        try
        {
            dtpFrom.Value = from;
            dtpTo.Value = to;
        }
        finally
        {
            _isInitializing = false;
        }

        _dateRangeActive = true;
        _ApplyFilter();
    }

    // ── Cell Formatting ────────────────────────────────────────────────────────────

    private void dgvBatches_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        if (e.ColumnIndex == dgvBatches.Columns["ExpiryStatus"].Index && e.Value is string status)
        {
            e.CellStyle.ForeColor = string.Equals(status, "منتهي", StringComparison.Ordinal)
                ? GreenThemeHelper.StatusDanger
                : GreenThemeHelper.TextStrong;
            e.CellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        }
    }

    // ── Filter Events (فورية — بلا زر بحث) ────────────────────────────────────────

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            _ApplyFilter();
        }
    }

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void chkOnlyExpiringSoon_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void chkOnlyExpired_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _dateRangeActive = true;
        _ApplyFilter();
    }

    private void dtpTo_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _dateRangeActive = true;
        _ApplyFilter();
    }

    private void btnRangeToday_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today, DateTime.Today);

    private void btnRangeMonth_Click(object sender, EventArgs e)
    {
        var today = DateTime.Today;
        _SetDateRange(new DateTime(today.Year, today.Month, 1), today);
    }

    private void btnRange30_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

    // ── Toolbar Events ─────────────────────────────────────────────────────────────

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← الدلائل (الأصناف) ← الشبكة ← الملخص والتحديد.
    /// </summary>
    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();          // التصفية تعود للافتراضي — بلا تحميل
            await _ReloadFormAsync();      // الدلائل + الشبكة + الملخص
            dgvBatches.ClearSelection();
            _UpdateClearFiltersState();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحديث دفعات المخزون");
            clsDialogs.Error("تعذر تحديث دفعات المخزون. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object sender, EventArgs e) => Close();

    private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ = _RefreshListAsync();
    }

    // ── قائمة السياق والنقر المزدوج (RULE-N23) ────────────────────────────────────

    /// <summary>نقر يمين على الشبكة = تحديد السطر تحت المؤشر ثم إظهار قائمة السياق.</summary>
    private void dgvBatches_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;

        var hit = dgvBatches.HitTest(e.X, e.Y);
        if (hit.RowIndex < 0) return;

        dgvBatches.ClearSelection();
        dgvBatches.Rows[hit.RowIndex].Selected = true;
        dgvBatches.CurrentCell = dgvBatches.Rows[hit.RowIndex].Cells[0];
    }

    private void dgvBatches_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        _ShowSelectedBatchDetails();
    }

    private void tsmiView_Click(object sender, EventArgs e) => _ShowSelectedBatchDetails();

    private void tsmiRefresh_Click(object sender, EventArgs e) => btnRefresh_Click(sender, e);

    private void tsmiExport_Click(object sender, EventArgs e) => _ExportToExcel();

    /// <summary>تفاصيل الدفعة المحددة من صفوف الشبكة — بلا استدعاء خدمة جديدة.</summary>
    private void _ShowSelectedBatchDetails()
    {
        try
        {
            if (dgvBatches.SelectedRows.Count == 0)
            {
                clsDialogs.Warning("يرجى اختيار دفعة من القائمة أولاً.");
                return;
            }

            var row = dgvBatches.SelectedRows[0];

            string Cell(string columnName)
                => row.Cells[columnName]?.Value?.ToString() ?? NoExpiryToken;

            var details = new System.Text.StringBuilder();
            details.AppendLine($"المنتج: {Cell("ProductName")}");
            details.AppendLine($"رقم الدفعة: {Cell("BatchNo")}");
            details.AppendLine($"المستودع: {Cell("WarehouseName")}");
            details.AppendLine($"الكمية المستلمة: {Cell("QuantityReceived")}");
            details.AppendLine($"الكمية المتبقية: {Cell("QuantityRemaining")}");
            details.AppendLine($"تكلفة الوحدة: {Cell("UnitCost")}");
            details.AppendLine($"القيمة الإجمالية: {Cell("TotalValue")}");
            details.AppendLine($"تاريخ الانتهاء: {Cell("ExpiryDate")}");
            details.AppendLine($"حالة الصلاحية: {Cell("ExpiryStatus")}");

            clsDialogs.Info(details.ToString());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل عرض تفاصيل دفعة المخزون");
            clsDialogs.Error("تعذر عرض تفاصيل الدفعة. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>تصدير الشبكة عبر الأداة الموحّدة <see cref="ExportHelper"/> (RULE-N112).</summary>
    private void _ExportToExcel()
    {
        try
        {
            if (dgvBatches.Rows.Count == 0)
            {
                clsDialogs.Warning("لا توجد بيانات للتصدير.");
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"دفعات_المخزون_{DateTime.Now:yyyyMMdd}.xlsx",
                Title = "تصدير دفعات المخزون إلى Excel"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;

            ExportHelper.ExportToExcel(dgvBatches, "دفعات المخزون", dialog.FileName);

            clsDialogs.Success("تم تصدير البيانات بنجاح.");
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تصدير دفعات المخزون إلى Excel");
            clsDialogs.Error("تعذر تصدير البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}
