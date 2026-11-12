using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Expenses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Expenses.Forms;

public partial class frmListExpenses : Form
{
    /// <summary>RULE-N46: لون الحقل النشط الموحّد.</summary>
    private static readonly Color FocusColor = Color.FromArgb(0xD6, 0xE4, 0xF0);

    private readonly IServiceProvider _serviceProvider;
    private List<ExpenseDto> _allExpenses = new();
    private List<ExpenseDto> _filteredExpenses = new();

    // يبدأ true حتى لا تنطلق تصفية من داخل المنشئ قبل اكتمال تهيئة أدوات التصفية.
    private bool _isInitializing = true;

    public frmListExpenses(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…N46 — تنقل وتحديد وتظليل وأسهم RTL.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — الأزرار المعطّلة تظهر رمادية باهتة.
        KeyboardHelper.WireButtonDisabledVisual(btnAddNew, btnRefresh, btnClose);

        // RULE-N47 — اختصارات الشاشة: F2 جديد · F5 تحديث · Delete حذف السطر · Esc مسح التصفية.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAddNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, () =>
            {
                if (dgvExpenses.ContainsFocus)
                    deleteToolStripMenuItem.PerformClick();
            }),
            (Keys.Escape, _ClearAllFilters));

        // RULE-N97 — التفقيط الحيّ كتلميح على حقل المبلغ في التصفية.
        LiveTafqeetHelper.Attach(liveTip, (txtSearch, "المبلغ", LiveTafqeetHelper.ReadMode.Money));

        _SetupToolTips();
    }

    /// <summary>تلميحات الأزرار مع اختصاراتها (RULE-N47).</summary>
    private void _SetupToolTips()
    {
        toolTip1.SetToolTip(btnAddNew, "مصروف جديد (F2)");
        toolTip1.SetToolTip(btnRefresh, "تحديث القائمة من الخدمة (F5)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة");
        toolTip1.SetToolTip(btnRangeToday, "تصفية مصروفات اليوم فقط");
        toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
        toolTip1.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّي به");
        toolTip1.SetToolTip(txtSearch, "اكتب قيمة التصفية — التصفية فورية (Esc يمسحها)");
        toolTip1.SetToolTip(chkOnlyCancelled, "إظهار المصروفات الملغاة فقط");
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvExpenses);

        // RULE-N22: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه (RULE-N52).
        dgvExpenses.ReadOnly = true;
        dgvExpenses.AllowUserToAddRows = false;
        dgvExpenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvExpenses.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    private void _SetupGridColumns()
    {
        dgvExpenses.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",              Header = "رقم المصروف",    Fill = 8,  Min = 100 },
            new { Name = "ExpenseNo",       Header = "رقم السند",       Fill = 8,  Min = 100 },
            new { Name = "ExpenseDate",     Header = "التاريخ",         Fill = 12, Min = 120 },
            new { Name = "ExpenseAccountName", Header = "حساب المصروف", Fill = 15, Min = 150 },
            new { Name = "CashBoxName",     Header = "الصندوق",         Fill = 12, Min = 120 },
            new { Name = "Amount",          Header = "المبلغ",          Fill = 10, Min = 100 },
            new { Name = "Notes",           Header = "الوصف",           Fill = 15, Min = 150 },
            new { Name = "StatusName",      Header = "الحالة",          Fill = 10, Min = 100 },
        };

        foreach (var col in columns)
        {
            dgvExpenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }
    }

    // ── تحميل البيانات (F5) ─────────────────────────────────────────────────────

    /// <summary>RULE-N25/N26: معالج async void مع try/catch و Cursor.WaitCursor واستعادته في finally.</summary>
    private async void _RefreshList()
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            var result = await _serviceProvider.GetRequiredService<IExpenseApiService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                _allExpenses = result.Value.ToList();
            }
            else
            {
                _allExpenses = new List<ExpenseDto>();
                MessageBox.Show(result.Error ?? "فشل في تحميل بيانات المصروفات", "خطأ في تحميل البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _ApplyFilter();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة المصروفات في شاشة الدليل");
            MessageBox.Show("تعذر تحميل بيانات المصروفات. يرجى إعادة المحاولة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
            {
                btnRefresh.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }

    // ── التصفية الفورية (RULE-N56…N61) ─────────────────────────────────────────

    /// <summary>
    /// التصفية الفورية: النطاق الزمني + الحالة + «الملغاة فقط» + الحقل المختار في «تصفية بـ».
    /// تُطبَّق عند كل تغيير — لا يوجد زر بحث (RULE-N59)، والمقارنة بـ SelectedIndex لا بالنص العربي (RULE-N60).
    /// </summary>
    private void _ApplyFilter()
    {
        if (_isInitializing)
            return;

        IEnumerable<ExpenseDto> query = _allExpenses;

        // 1) النطاق الزمني — نطاق معكوس = نُبادل الطرفين بدل إسقاط النتائج.
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;
        if (from > to)
            (from, to) = (to, from);

        query = query.Where(x => x.ExpenseDate.Date >= from && x.ExpenseDate.Date <= to);

        // 2) الحالة — 0 = الكل، 1 = مسودة، 2 = مرحّل، 3 = ملغى.
        byte? status = _GetStatusFilterValue();
        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        // 3) «الملغاة فقط».
        if (chkOnlyCancelled.Checked)
            query = query.Where(x => x.Status == 3);

        // 4) نص التصفية على الحقل المختار.
        string text = txtSearch.Text.Trim();
        if (text.Length > 0)
        {
            query = cbFilterBy.SelectedIndex switch
            {
                1 => query.Where(x => x.ExpenseNo.ToString().Contains(text, StringComparison.Ordinal)),
                2 => query.Where(x => (x.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
                3 => query.Where(x => (x.ExpenseAccountName ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
                4 => query.Where(x => (x.CashBoxName ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
                5 => _FilterByAmount(query, text),
                _ => query.Where(x => x.ExpenseNo.ToString().Contains(text, StringComparison.Ordinal)
                                   || (x.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
            };
        }

        _filteredExpenses = query.ToList();
        _PopulateGrid();
        _UpdateSummary();
    }

    /// <summary>تصفية المبلغ: مطابقة رقمية إن أمكن، وإلا مطابقة نصية منسّقة.</summary>
    private static IEnumerable<ExpenseDto> _FilterByAmount(IEnumerable<ExpenseDto> query, string text)
    {
        string normalized = text.Replace(",", string.Empty);

        if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
            return query.Where(x => x.Amount == amount
                                 || x.Amount.ToString("N2").Contains(text, StringComparison.OrdinalIgnoreCase));

        return query.Where(x => x.Amount.ToString("N2").Contains(text, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>قيمة الحالة المختارة — null = الكل.</summary>
    private byte? _GetStatusFilterValue()
    {
        if (cbStatus.SelectedIndex <= 0)
            return null;

        return (byte)cbStatus.SelectedIndex;   // 1 مسودة · 2 مرحّل · 3 ملغى
    }

    private void _PopulateGrid()
    {
        dgvExpenses.AutoGenerateColumns = false;
        dgvExpenses.Rows.Clear();

        foreach (var ex in _filteredExpenses)
        {
            dgvExpenses.Rows.Add(
                ex.Id,
                ex.ExpenseNo,
                ex.ExpenseDate.ToString("yyyy-MM-dd"),
                ex.ExpenseAccountName ?? "",
                ex.CashBoxName ?? "",
                ex.Amount.ToString("N2"),
                ex.Notes ?? "",
                ex.StatusName ?? ""
            );
        }

        // سطر «لا توجد بيانات» — طبقة نصية فوق الشبكة عند فراغ النتيجة.
        lblEmpty.Visible = _filteredExpenses.Count == 0;

        if (lblEmpty.Visible)
            lblEmpty.BringToFront();
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshList();
    }

    /// <summary>RULE-N61: شريط الملخص — المعروض/الكل + الإجمالي N2 + عدّاد المرحّل والملغى.</summary>
    private void _UpdateSummary()
    {
        decimal total = 0m;
        int posted = 0;
        int cancelled = 0;

        foreach (var ex in _filteredExpenses)
        {
            total += ex.Amount;

            if (ex.Status == 2)
                posted++;
            else if (ex.Status == 3)
                cancelled++;
        }

        lblSummary.Text =
            $"المعروض: {_filteredExpenses.Count} من {_allExpenses.Count} — الإجمالي: {total.ToString("N2")} · مرحّل: {posted} · ملغى: {cancelled}";

        _UpdateRecordsCount();
    }

    private void _UpdateRecordsCount()
    {
        lblRecordsCount.Text = _filteredExpenses.Count.ToString();
    }

    private int _GetSelectedId()
    {
        if (dgvExpenses.CurrentRow == null) return -1;
        if (dgvExpenses.CurrentRow.Cells[0].Value is not int id) return -1;
        return id;
    }

    private byte _GetSelectedStatus()
    {
        if (dgvExpenses.CurrentRow == null) return 0;
        var item = _filteredExpenses.FirstOrDefault(x => x.Id == _GetSelectedId());
        return item?.Status ?? 0;
    }

    private void _OpenForm(Form form)
    {
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    // ── الأحداث ────────────────────────────────────────────────────────────────

    private void frmListExpenses_Load(object sender, EventArgs e)
    {
        _isInitializing = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
            chkOnlyCancelled.Checked = false;
            txtSearch.Clear();
            _SetDateRangeThisMonth();
        }
        finally
        {
            _isInitializing = false;
        }

        _ReloadForm();
        ApplyGreenTheme();   // RULE-N50 — الأخير في مسار التحميل
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateExpense>());
    }

    private void btnRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    // ── أحداث التصفية الفورية ──────────────────────────────────────────────────

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    /// <summary>RULE-N59: Enter يطبّق التصفية فوراً (وهي مطبَّقة أصلاً عند كل كتابة).</summary>
    private void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None)
            return;

        _ApplyFilter();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
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

    private void btnRangeToday_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today, DateTime.Today);

    private void btnRangeMonth_Click(object sender, EventArgs e)
        => _SetDateRangeThisMonth();

    private void btnRange30_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

    // ── أدوات التصفية ──────────────────────────────────────────────────────────

    /// <summary>يضبط النطاق بلا إطلاق أحداث وسيطة ثم يُصفّي مرة واحدة.</summary>
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
            txtSearch.Clear();
            _SetDateRangeThisMonth();
        }
        finally
        {
            _isInitializing = false;
        }

        _ApplyFilter();
        txtSearch.Focus();
    }

    // ── قائمة السياق والإجراءات ────────────────────────────────────────────────

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateExpense>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void postToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status != 1)
        {
            MessageBox.Show("لا يمكن ترحيل المصروف إلا في حالة مسودة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show("هل أنت متأكد من ترحيل هذا المصروف؟", "تأكيد الترحيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        _ = PostAsync(id);
    }

    private async Task PostAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<IExpenseApiService>().PostAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم ترحيل المصروف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في الترحيل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل ترحيل المصروف {ExpenseId} من شاشة الدليل", id);
            MessageBox.Show("تعذر ترحيل المصروف. يرجى إعادة المحاولة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status == 3)
        {
            MessageBox.Show("السند ملغي بالفعل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show("هل أنت متأكد من إلغاء هذا المصروف؟", "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        _ = CancelAsync(id);
    }

    private async Task CancelAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<IExpenseApiService>().CancelAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم إلغاء المصروف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في الإلغاء", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إلغاء المصروف {ExpenseId} من شاشة الدليل", id);
            MessageBox.Show("تعذر إلغاء المصروف. يرجى إعادة المحاولة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status != 1)
        {
            MessageBox.Show("لا يمكن حذف المصروف إلا في حالة مسودة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show("هل أنت متأكد من حذف هذا المصروف؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        _ = DeleteAsync(id);
    }

    private async Task DeleteAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<IExpenseApiService>().DeleteAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم حذف المصروف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في الحذف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل حذف المصروف {ExpenseId} من شاشة الدليل", id);
            MessageBox.Show("تعذر حذف المصروف. يرجى إعادة المحاولة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private void dgvExpenses_DoubleClick(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateExpense>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void cmsExpenses_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsExpenses.Enabled = dgvExpenses.Rows.Count > 0 && dgvExpenses.CurrentRow != null;
    }
}
