using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

public partial class frmListInventoryAdjustments : Form
{
    /// <summary>RULE-N46: لون الحقل النشط الموحّد.</summary>
    private static readonly Color FocusColor = Color.FromArgb(0xD6, 0xE4, 0xF0);

    private readonly IServiceProvider _serviceProvider;

    private DataTable _adjustmentsTable = new();
    private DataView _adjustmentsView = new();
    private InventoryAdjustmentDto? _selectedAdjustment;

    // يبدأ true حتى لا تنطلق تصفية من داخل المنشئ قبل اكتمال تهيئة أدوات التصفية.
    private bool _isInitializing = true;

    public frmListInventoryAdjustments(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…N46 — تنقل وتحديد وتظليل وأسهم RTL.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — الأزرار المعطّلة تظهر رمادية باهتة.
        KeyboardHelper.WireButtonDisabledVisual(btnAdd, btnView, btnPost, btnCancelAdjustment, btnRefresh);

        // RULE-N47 — F2 جديد · F5 تحديث · Delete إلغاء · Esc مسح التصفية.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAdd.PerformClick()),
            (Keys.F5, _ReloadForm),
            (Keys.Delete, () =>
            {
                if (dgvAdjustments.ContainsFocus)
                    _CancelAdjustment();
            }),
            (Keys.Escape, _ClearAllFilters));

        _SetupToolTips();
        ApplyUiTheme();
    }

    /// <summary>RULE-N04 — الخدمة تُحل من الحاوية ولا تُنشأ يدوياً.</summary>
    private IInventoryAdjustmentDesktopService DesktopService
        => _serviceProvider.GetRequiredService<IInventoryAdjustmentDesktopService>();

    // ── المظهر ─────────────────────────────────────────────────────────────────────

    /// <summary>RULE-N47 — تلميح لكل زر مع اختصاره.</summary>
    private void _SetupToolTips()
    {
        toolTip1.SetToolTip(btnAdd, "تسوية مخزون جديدة (F2)");
        toolTip1.SetToolTip(btnView, "عرض تفاصيل التسوية المحددة");
        toolTip1.SetToolTip(btnPost, "ترحيل التسوية المحددة (مسودة فقط)");
        toolTip1.SetToolTip(btnCancelAdjustment, "إلغاء ترحيل التسوية المحددة (Delete)");
        toolTip1.SetToolTip(btnRefresh, "تحديث القائمة من الخدمة (F5)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة");
        toolTip1.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّي به");
        toolTip1.SetToolTip(txtFilterValue, "اكتب قيمة التصفية — التصفية فورية (Esc يمسحها)");
        toolTip1.SetToolTip(cbStatus, "تصفية حسب حالة التسوية");
        toolTip1.SetToolTip(dtpFrom, "بداية النطاق الزمني لتاريخ الإنشاء");
        toolTip1.SetToolTip(dtpTo, "نهاية النطاق الزمني لتاريخ الإنشاء");
        toolTip1.SetToolTip(btnRangeToday, "تسويات اليوم فقط");
        toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
        toolTip1.SetToolTip(chkOnlyCancelled, "إظهار التسويات الملغاة فقط");
        toolTip1.SetToolTip(chkIncludeCancelled, "تحميل التسويات الملغاة من الخدمة أيضاً");
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N54) — الترتيب إلزامي.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);      // RULE-N53
        GreenThemeHelper.ApplyGrid(dgvAdjustments); // RULE-N51

        // RULE-N22: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه (RULE-N52).
        dgvAdjustments.ReadOnly = true;
        dgvAdjustments.AllowUserToAddRows = false;
        dgvAdjustments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAdjustments.MultiSelect = false;

        GreenThemeHelper.Apply(this);               // ← الأخير دائماً (RULE-N50)
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────────

    private DataGridViewColumn _MakeColumn(string name, string header, int fill, int min)
    {
        return new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = header,
            DataPropertyName = name,
            MinimumWidth = min,
            FillWeight = fill,
            ReadOnly = true,
        };
    }

    private void _SetupGridColumns()
    {
        dgvAdjustments.Columns.Clear();

        dgvAdjustments.Columns.Add(_MakeColumn("Id", "رقم التسوية", 5, 60));
        dgvAdjustments.Columns.Add(_MakeColumn("AdjustmentNo", "رقم المستند", 8, 100));
        dgvAdjustments.Columns.Add(_MakeColumn("WarehouseName", "المستودع", 12, 130));
        dgvAdjustments.Columns.Add(_MakeColumn("AdjustmentTypeName", "نوع التسوية", 10, 100));
        dgvAdjustments.Columns.Add(_MakeColumn("StatusName", "الحالة", 8, 80));
        dgvAdjustments.Columns.Add(_MakeColumn("Reason", "السبب", 15, 150));
        dgvAdjustments.Columns.Add(_MakeColumn("CreatedAt", "تاريخ الإنشاء", 12, 130));
    }

    // ── Data Loading ───────────────────────────────────────────────────────────────

    /// <summary>RULE-N25/N26: معالج async void مع try/catch و WaitCursor واستعادته في finally.</summary>
    private async void _RefreshList()
    {
        Cursor = dgvAdjustments.Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            _adjustmentsTable = _CreateTable();
            _adjustmentsView = new DataView(_adjustmentsTable);

            var result = await DesktopService.GetAllAsync(includeInactive: chkIncludeCancelled.Checked);

            if (result.IsSuccess && result.Value is { Count: > 0 })
            {
                foreach (var a in result.Value)
                {
                    _adjustmentsTable.Rows.Add(
                        a.Id,
                        a.AdjustmentNo,
                        a.WarehouseName ?? "",
                        a.AdjustmentTypeName ?? "",
                        a.StatusName ?? "",
                        a.Reason ?? "",
                        a.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                        a.Status);
                }
            }
            else if (!result.IsSuccess)
            {
                MessageBox.Show(result.Error ?? "حدث خطأ في تحميل قائمة التسويات", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvAdjustments.DataSource = _adjustmentsView;
            _ApplyFilter();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة تسويات المخزون في شاشة القائمة");
            MessageBox.Show("تعذر تحميل قائمة تسويات المخزون. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
            {
                btnRefresh.Enabled = true;
                Cursor = dgvAdjustments.Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>جدول الذاكرة: الأعمدة المعروضة + عمود الحالة الخفي اللازم للتصفية والملخص.</summary>
    private static DataTable _CreateTable()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("AdjustmentNo", typeof(string));
        table.Columns.Add("WarehouseName", typeof(string));
        table.Columns.Add("AdjustmentTypeName", typeof(string));
        table.Columns.Add("StatusName", typeof(string));
        table.Columns.Add("Reason", typeof(string));
        table.Columns.Add("CreatedAt", typeof(string));   // "yyyy-MM-dd HH:mm" — مقارنة ISO قابلة للترتيب
        table.Columns.Add("Status", typeof(int));         // 1 مسودة · 2 مرحّل · 3 ملغى (لا يُعرض)
        return table;
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshList();
    }

    // ── التصفية الفورية (RULE-N56…N61) ─────────────────────────────────────────────

    /// <summary>
    /// التصفية الفورية بالحالة + «الملغاة فقط» + النطاق الزمني + نص التصفية على الحقل المختار.
    /// لا يوجد زر بحث (RULE-N59)، والمقارنة بـ SelectedIndex لا بالنص العربي (RULE-N60).
    /// </summary>
    private void _ApplyFilter()
    {
        if (_isInitializing || _adjustmentsView.Table is null)
            return;

        var parts = new List<string>();

        // 1) الحالة — 1 مسودة · 2 مرحّل · 3 ملغى (القيم الحقيقية لهذا الكيان).
        byte? status = _GetStatusFilterValue();
        if (status.HasValue)
            parts.Add($"Status = {status.Value}");

        // 2) «الملغاة فقط» — يحتاج عمود الحالة الموجود أصلاً.
        if (chkOnlyCancelled.Checked)
            parts.Add("Status = 3");

        // 3) النطاق الزمني على تاريخ الإنشاء (نطاق معكوس = نُبادل الطرفين).
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;
        if (from > to)
            (from, to) = (to, from);

        parts.Add($"CreatedAt >= '{from:yyyy-MM-dd}' AND CreatedAt < '{to.AddDays(1):yyyy-MM-dd}'");

        // 4) نص التصفية على الحقل المختار.
        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            string like = $"%{_EscapeLike(text)}%";

            parts.Add(cbFilterBy.SelectedIndex switch
            {
                1 => $"AdjustmentNo LIKE '{like}'",
                2 => $"WarehouseName LIKE '{like}'",
                3 => $"Reason LIKE '{like}'",
                _ => $"(AdjustmentNo LIKE '{like}' OR WarehouseName LIKE '{like}' OR Reason LIKE '{like}')",
            });
        }

        try
        {
            _adjustmentsView.RowFilter = string.Join(" AND ", parts);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق تصفية تسويات المخزون");
            _adjustmentsView.RowFilter = string.Empty;
        }

        _UpdateSummary();
        _UpdateButtonsState();
    }

    /// <summary>تهريب محارف LIKE الخاصة بتعبير DataView حتى لا يفسد إدخال المستخدم التعبير.</summary>
    private static string _EscapeLike(string value)
    {
        var sb = new System.Text.StringBuilder(value.Length + 8);

        foreach (char c in value)
        {
            switch (c)
            {
                case '\'':
                    sb.Append("''");
                    break;
                case '%':
                case '*':
                case '[':
                case ']':
                    sb.Append('[').Append(c).Append(']');
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        return sb.ToString();
    }

    /// <summary>قيمة الحالة المختارة — null = الكل.</summary>
    private byte? _GetStatusFilterValue()
    {
        if (cbStatus.SelectedIndex <= 0)
            return null;

        return (byte)cbStatus.SelectedIndex;   // 1 مسودة · 2 مرحّل · 3 ملغى
    }

    /// <summary>RULE-N61: شريط الملخص — المعروض من الكل + عدّاد كل حالة.</summary>
    private void _UpdateSummary()
    {
        int draft = 0, posted = 0, cancelled = 0;

        foreach (DataRowView row in _adjustmentsView)
        {
            switch (Convert.ToInt32(row["Status"]))
            {
                case 1: draft++; break;
                case 2: posted++; break;
                case 3: cancelled++; break;
            }
        }

        int shown = _adjustmentsView.Count;
        int total = _adjustmentsTable.Rows.Count;

        lblSummary.Text =
            $"المعروض: {shown} من {total} — مسودات: {draft} · مُرحَّلة: {posted} · ملغاة: {cancelled}";

        lblRecordsCount.Text = shown.ToString();
    }

    /// <summary>RULE-N43: أزرار الإجراءات مفعّلة فقط عند وجود سجل محدد.</summary>
    private void _UpdateButtonsState()
    {
        bool hasSelection = dgvAdjustments.SelectedRows.Count > 0;

        btnView.Enabled = hasSelection;
        btnPost.Enabled = hasSelection;
        btnCancelAdjustment.Enabled = hasSelection;
    }

    // ── Selection ──────────────────────────────────────────────────────────────────

    private async Task _SelectAdjustmentAsync()
    {
        _selectedAdjustment = null;

        if (dgvAdjustments.SelectedRows.Count == 0)
            return;

        var row = dgvAdjustments.SelectedRows[0];
        if (row.DataBoundItem is not DataRowView drv)
            return;

        int id = Convert.ToInt32(drv["Id"]);
        var result = await DesktopService.GetByIdAsync(id);
        _selectedAdjustment = result.IsSuccess ? result.Value : null;
    }

    // ── Actions ────────────────────────────────────────────────────────────────────

    private void _AddAdjustment()
    {
        var editor = _serviceProvider.GetRequiredService<frmAddUpdateInventoryAdjustment>();
        editor.ShowDialog();
        _ReloadForm();
    }

    private async void _ViewDetails()
    {
        try
        {
            if (dgvAdjustments.SelectedRows.Count == 0)
                return;

            Cursor = Cursors.WaitCursor;
            await _SelectAdjustmentAsync();

            if (_selectedAdjustment is null)
                return;

            var editor = _serviceProvider.GetRequiredService<frmAddUpdateInventoryAdjustment>();
            editor.LoadAdjustment(_selectedAdjustment.Id);
            editor.ShowDialog();
            _ReloadForm();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل فتح تفاصيل تسوية المخزون من شاشة القائمة");
            MessageBox.Show("تعذر فتح تفاصيل التسوية. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private async void _PostAdjustment()
    {
        try
        {
            if (dgvAdjustments.SelectedRows.Count == 0)
                return;

            Cursor = Cursors.WaitCursor;
            await _SelectAdjustmentAsync();

            if (_selectedAdjustment is null || _selectedAdjustment.Status != 1)   // Draft only
                return;

            if (MessageBox.Show("هل أنت متأكد من ترحيل تسوية المخزون؟", "تأكيد الترحيل",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var result = await DesktopService.PostAsync(_selectedAdjustment.Id);

            if (result.IsSuccess)
                MessageBox.Show("تم ترحيل تسوية المخزون بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show($"خطأ في الترحيل: {result.Error}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            _ReloadForm();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل ترحيل تسوية المخزون {AdjustmentId} من شاشة القائمة", _selectedAdjustment?.Id);
            MessageBox.Show("تعذر ترحيل تسوية المخزون. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private async void _CancelAdjustment()
    {
        try
        {
            if (dgvAdjustments.SelectedRows.Count == 0)
                return;

            Cursor = Cursors.WaitCursor;
            await _SelectAdjustmentAsync();

            if (_selectedAdjustment is null || _selectedAdjustment.Status != 2)   // Posted only
                return;

            if (MessageBox.Show("هل أنت متأكد من إلغاء تسوية المخزون؟ لا يمكن التراجع عن هذا الإجراء.",
                "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            var result = await DesktopService.CancelAsync(_selectedAdjustment.Id);

            if (result.IsSuccess)
                MessageBox.Show("تم إلغاء تسوية المخزون بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show($"خطأ في الإلغاء: {result.Error}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            _ReloadForm();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إلغاء تسوية المخزون {AdjustmentId} من شاشة القائمة", _selectedAdjustment?.Id);
            MessageBox.Show("تعذر إلغاء تسوية المخزون. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    // ── Form Events ────────────────────────────────────────────────────────────────

    private void frmListInventoryAdjustments_Load(object sender, EventArgs e)
    {
        _isInitializing = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
            chkOnlyCancelled.Checked = false;
            chkIncludeCancelled.Checked = false;
            txtFilterValue.Clear();
            _SetDateRangeThisMonth();
        }
        finally
        {
            _isInitializing = false;
        }

        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        _AddAdjustment();
    }

    private void btnView_Click(object sender, EventArgs e)
    {
        _ViewDetails();
    }

    private void btnPost_Click(object sender, EventArgs e)
    {
        _PostAdjustment();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        _CancelAdjustment();
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        txtFilterValue.Clear();
        _ReloadForm();
    }

    // ── أحداث التصفية الفورية ──────────────────────────────────────────────────────

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

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    /// <summary>RULE-N59: Enter يطبّق التصفية فوراً.</summary>
    private void txtFilterValue_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None)
            return;

        _ApplyFilter();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            _ApplyFilter();
        }
    }

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void dtpTo_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void chkOnlyCancelled_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void chkIncludeCancelled_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ReloadForm();
    }

    private void btnRangeToday_Click(object sender, EventArgs e)
    {
        _SetDateRange(DateTime.Today, DateTime.Today);
    }

    private void btnRangeMonth_Click(object sender, EventArgs e)
    {
        _SetDateRangeThisMonth();
    }

    private void btnRange30_Click(object sender, EventArgs e)
    {
        _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);
    }

    // ── أدوات التصفية ──────────────────────────────────────────────────────────────

    /// <summary>يضبط النطاق بلا إطلاق أحداث وسيطة ثم يُصفّي مرة واحدة.</summary>
    private void _SetDateRange(DateTime from, DateTime to)
    {
        bool wasInitializing = _isInitializing;
        _isInitializing = true;
        try
        {
            dtpFrom.Value = from;
            dtpTo.Value = to;
        }
        finally
        {
            _isInitializing = wasInitializing;
        }

        _ApplyFilter();
    }

    /// <summary>«هذا الشهر» = من أول الشهر الحالي حتى اليوم (الافتراضي ومسح Esc).</summary>
    private void _SetDateRangeThisMonth()
    {
        var today = DateTime.Today;
        dtpFrom.Value = new DateTime(today.Year, today.Month, 1);
        dtpTo.Value = today;

        if (!_isInitializing)
            _ApplyFilter();
    }

    /// <summary>Esc: مسح كل أدوات التصفية وإعادة النطاق إلى «هذا الشهر».</summary>
    private void _ClearAllFilters()
    {
        _isInitializing = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
            chkOnlyCancelled.Checked = false;
            txtFilterValue.Clear();
            _SetDateRangeThisMonth();
        }
        finally
        {
            _isInitializing = false;
        }

        _ApplyFilter();
        txtFilterValue.Focus();
    }

    // ── الشبكة وقائمة السياق ───────────────────────────────────────────────────────

    private void dgvAdjustments_SelectionChanged(object sender, EventArgs e)
    {
        _UpdateButtonsState();
    }

    private void dgvAdjustments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        _ViewDetails();
    }

    private void ShowContextMenu(MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        var hit = dgvAdjustments.HitTest(e.X, e.Y);
        if (hit.RowIndex < 0) return;
        dgvAdjustments.ClearSelection();
        dgvAdjustments.Rows[hit.RowIndex].Selected = true;
        contextMenuStrip.Show(dgvAdjustments, e.Location);
    }

    private void dgvAdjustments_MouseDown(object sender, MouseEventArgs e)
    {
        ShowContextMenu(e);
    }

    private void tsmiView_Click(object sender, EventArgs e)
    {
        _ViewDetails();
    }

    private void tsmiPost_Click(object sender, EventArgs e)
    {
        _PostAdjustment();
    }

    private void tsmiCancel_Click(object sender, EventArgs e)
    {
        _CancelAdjustment();
    }
}
