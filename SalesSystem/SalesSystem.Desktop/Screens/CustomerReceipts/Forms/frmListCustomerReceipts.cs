using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.CustomerReceipts.Forms;

public partial class frmListCustomerReceipts : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<CustomerReceiptDto> _allReceipts = new();
    private List<CustomerReceiptDto> _filteredReceipts = new();

    /// <summary>يمنع التصفية الفورية أثناء التهيئة الأولية (RULE-N59).</summary>
    private bool _isInitializing = true;

    /// <summary>المدى الافتراضي وقت الإنشاء — يعيده «مسح التصفية» و«تحديث» (RULE-N148).</summary>
    private readonly DateTime _defaultFrom;
    private readonly DateTime _defaultTo;

    private const byte StatusDraft = 1;
    private const byte StatusPosted = 2;
    private const byte StatusCancelled = 3;

    public frmListCustomerReceipts(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40/RULE-N95: الستة من KeyboardHelper — تنقل + تحديد + تظليل + أسهم + أزرار + اختصارات.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));   // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                     // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnAddNew, btnRefresh, btnClose, btnClearFilters); // RULE-N43

        KeyboardHelper.WireGlobalShortcuts(this,                                      // RULE-N47
            (Keys.F2, () => btnAddNew.PerformClick()),
            (Keys.Delete, () =>
            {
                if (dgvReceipts.ContainsFocus && dgvReceipts.CurrentRow != null)
                    cancelToolStripMenuItem.PerformClick();
            }),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, _GlobalEscape));

        // Enter في حقل التصفية = تطبيق فوري (RULE-N59).
        KeyboardHelper.WireEnterToClick(txtFilterValue, () => _ApplyFilter());

        // RULE-N147: Enter = الزر الأساسي للشاشة («سند جديد») — ولا يعمل داخل حقول الإدخال.
        KeyboardHelper.WireModeEnter(this, () => btnAddNew);
        AcceptButton = btnAddNew;

        _LoadStatusOptions();
        _defaultFrom = _StartOfCurrentMonth();
        _defaultTo = DateTime.Today;
        _SetDateRange(_defaultFrom, _defaultTo);   // «هذا الشهر» افتراضياً
        _SetupToolTips();
        _isInitializing = false;
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    /// <summary>
    /// RULE-N147 + RULE-N40: «Enter» يعمل زرَّ الوضع الأساسي («سند جديد») عبر <c>AcceptButton</c>،
    /// **إلا** داخل حقول «بحث وتصفية» فيبقى سلوك العنصر نفسه: تطبيق التصفية فوراً أو الانتقال للحقل التالي،
    /// وعلى أزرار التصفية = تنفيذ الزر. (AcceptButton في WinForms يسبق KeyDown ويلتقط Enter).
    /// </summary>
    protected override bool ProcessDialogKey(Keys keyData)
    {
        if (keyData == Keys.Enter && gbFilter.ContainsFocus && ActiveControl is not ButtonBase)
            return false;   // غير مُعالج ⇒ يمرّ إلى KeyDown فتُنفَّذ قواعد التنقل/التصفية القائمة.

        return base.ProcessDialogKey(keyData);
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvReceipts);

        // RULE-N22/RULE-N52: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        dgvReceipts.ReadOnly = true;
        dgvReceipts.AllowUserToAddRows = false;
        dgvReceipts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReceipts.MultiSelect = false;

        _ApplyTextTheme();

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>RULE-N54: ألوان النصوص من الثيم الأخضر فقط — لا قيم لونية خام داخل الشاشة.</summary>
    private void _ApplyTextTheme()
    {
        foreach (var lbl in new[] { label1, label2, label3, label4, lblSummary })
            lbl.ForeColor = GreenThemeHelper.TextStrong;

        lblCountCaption.ForeColor = GreenThemeHelper.HeaderForeground;
        lblCountCaption.BackColor = GreenThemeHelper.HeaderBackground;
        lblRecordsCount.ForeColor = GreenThemeHelper.HeaderForeground;
        lblRecordsCount.BackColor = GreenThemeHelper.HeaderBackground;
    }

    private void _SetupToolTips()
    {
        toolTip1.SetToolTip(btnAddNew, "سند قبض جديد (F2)");
        toolTip1.SetToolTip(btnRefresh, "تحديث القائمة (F5)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة");

        toolTip1.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّى به القائمة");
        toolTip1.SetToolTip(txtFilterValue, "تصفية فورية — Enter يطبّق، Esc يمسح كل التصفية");
        toolTip1.SetToolTip(cbStatus, "تصفية بحالة السند");
        toolTip1.SetToolTip(dtpFrom, "تاريخ السند — من");
        toolTip1.SetToolTip(dtpTo, "تاريخ السند — إلى");

        toolTip1.SetToolTip(btnRangeToday, "سندات اليوم فقط");
        toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
        toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip1.SetToolTip(chkOnlyUnapplied, "السندات التي لم يُخصَّص كامل مبلغها بعد");
    }

    // ── خيارات التصفية ─────────────────────────────────────────────────────────

    /// <summary>RULE-N60: التصفية بالـ SelectedIndex لا بمقارنة نص عربي.</summary>
    private void _LoadStatusOptions()
    {
        cbStatus.Items.Clear();
        cbStatus.Items.Add("الكل");
        cbStatus.Items.Add("مرحّل");
        cbStatus.Items.Add("ملغى");
        cbStatus.SelectedIndex = 0;
    }

    /// <summary>0 = الكل · 1 = مرحّل (2) · 2 = ملغى (3).</summary>
    private byte? _GetStatusFilter() => cbStatus.SelectedIndex switch
    {
        1 => StatusPosted,
        2 => StatusCancelled,
        _ => null
    };

    private static DateTime _StartOfCurrentMonth()
    {
        var today = DateTime.Today;
        return new DateTime(today.Year, today.Month, 1);
    }

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
    }

    // ── البيانات ───────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvReceipts.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",                Header = "رقم السند",     Fill = 8,  Min = 100 },
            new { Name = "ReceiptNo",         Header = "رقم السند",     Fill = 8,  Min = 100 },
            new { Name = "ReceiptDate",       Header = "التاريخ",       Fill = 12, Min = 120 },
            new { Name = "CustomerName",      Header = "العميل",        Fill = 15, Min = 150 },
            new { Name = "Amount",            Header = "المبلغ",        Fill = 10, Min = 100 },
            new { Name = "PaymentMethodName", Header = "طريقة الدفع",  Fill = 12, Min = 120 },
            new { Name = "CashBoxName",       Header = "الصندوق",       Fill = 12, Min = 120 },
            new { Name = "StatusName",        Header = "الحالة",        Fill = 10, Min = 100 },
        };

        foreach (var col in columns)
        {
            dgvReceipts.Columns.Add(new DataGridViewTextBoxColumn
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

    private async void _RefreshList()
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;
        try
        {
            var result = await _serviceProvider.GetRequiredService<ICustomerReceiptDesktopService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                _allReceipts = result.Value.ToList();
            }
            else
            {
                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل سندات القبض", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                _allReceipts = new List<CustomerReceiptDto>();
            }
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "حدث خطأ أثناء تحميل سندات القبض");
            MessageBox.Show("حدث خطأ أثناء تحميل سندات القبض",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
            {
                _ApplyFilter();
                btnRefresh.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }

    // ── التصفية ────────────────────────────────────────────────────────────────

    /// <summary>RULE-N56…N60: تصفية فورية بلا زر «بحث» — الحالة + النطاق + غير المخصصة + البحث النصي.</summary>
    private void _ApplyFilter()
    {
        if (_isInitializing)
            return;

        IEnumerable<CustomerReceiptDto> query = _allReceipts;

        byte? status = _GetStatusFilter();
        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        // نطاق التاريخ: نُبدّل الطرفين بدل إظهار رسالة مع كل كتابة (RULE-N59).
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;
        if (from > to)
            (from, to) = (to, from);

        query = query.Where(r => r.ReceiptDate.Date >= from && r.ReceiptDate.Date <= to);

        if (chkOnlyUnapplied.Checked)
            query = query.Where(r => _GetUnappliedAmount(r) > 0m);

        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            int filterBy = cbFilterBy.SelectedIndex;
            query = query.Where(r => _MatchesText(r, filterBy, text));
        }

        _filteredReceipts = query.ToList();
        _PopulateGrid();
        _UpdateSummary();
        _UpdateClearFiltersState();   // RULE-N148: حالة زر «مسح التصفية» تتبع وجود تصفية نشطة
    }

    private static bool _MatchesText(CustomerReceiptDto r, int filterBy, string text) => filterBy switch
    {
        1 => r.ReceiptNo.ToString().Contains(text, StringComparison.Ordinal),
        2 => (r.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase),
        3 => clsFormat.Money(r.Amount).Contains(text, StringComparison.Ordinal)
             || r.Amount.ToString("0.###").Contains(text, StringComparison.Ordinal),
        _ => (r.CustomerName ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase),
    };

    private static decimal _GetAppliedAmount(CustomerReceiptDto r)
        => r.Applications?.Where(a => a.IsActive).Sum(a => a.AppliedAmount) ?? 0m;

    private static decimal _GetUnappliedAmount(CustomerReceiptDto r)
        => Math.Max(0m, r.Amount - _GetAppliedAmount(r));

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للبيانات (RULE-N148). «تحديث» هو المسؤول عن إعادة التحميل.
    /// </summary>
    private void btnClearFilters_Click(object sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح» و«تحديث» (RULE-N148).</summary>
    private void _ClearFilterInputs()
    {
        bool wasInitializing = _isInitializing;
        _isInitializing = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Clear();
            cbStatus.SelectedIndex = 0;
            chkOnlyUnapplied.Checked = false;
            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _isInitializing = wasInitializing;
        }
    }

    /// <summary>يمسح كل أدوات التصفية ويعيد الشبكة إلى حالتها الافتراضية بلا إعادة تحميل (RULE-N59).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
    }

    /// <summary>RULE-N43/RULE-N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            cbFilterBy.SelectedIndex > 0
            || cbStatus.SelectedIndex > 0
            || chkOnlyUnapplied.Checked
            || !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || dtpFrom.Value.Date != _defaultFrom.Date
            || dtpTo.Value.Date != _defaultTo.Date;
    }

    /// <summary>RULE-N47 + RULE-N59: Esc داخل صندوق التصفية = مسحها، وخارجه = إغلاق الشاشة.</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        Close();
    }

    private void _PopulateGrid()
    {
        dgvReceipts.AutoGenerateColumns = false;
        dgvReceipts.Rows.Clear();

        foreach (var r in _filteredReceipts)
        {
            int rowIndex = dgvReceipts.Rows.Add(
                r.Id,
                r.ReceiptNo,
                r.ReceiptDate.ToString("yyyy-MM-dd"),
                r.CustomerName ?? "",
                clsFormat.Money(r.Amount),
                r.PaymentMethodName ?? "",
                r.CashBoxName ?? "",
                r.StatusName ?? ""
            );

            // الربط بالمعرّف لا بترتيب السطر.
            dgvReceipts.Rows[rowIndex].Tag = r.Id;
        }

        emptyState.Visible = _filteredReceipts.Count == 0;
        if (emptyState.Visible)
            emptyState.BringToFront();
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshList();
    }

    /// <summary>RULE-N61: شريط الملخص — المعروض/الإجمالي/عدد المرحّل والملغى.</summary>
    private void _UpdateSummary()
    {
        int shown = _filteredReceipts.Count;
        decimal total = _filteredReceipts.Sum(r => r.Amount);
        int posted = _filteredReceipts.Count(r => r.Status == StatusPosted);
        int cancelled = _filteredReceipts.Count(r => r.Status == StatusCancelled);

        lblSummary.Text =
            $"المعروض: {shown} من {_allReceipts.Count} — الإجمالي: {total:N2} · مرحّل: {posted} · ملغى: {cancelled}";

        _UpdateRecordsCount();
    }

    private void _UpdateRecordsCount()
    {
        lblRecordsCount.Text = _filteredReceipts.Count.ToString();
    }

    private int _GetSelectedId()
    {
        if (dgvReceipts.CurrentRow == null) return -1;
        return dgvReceipts.CurrentRow.Cells[0].Value is int id ? id : -1;
    }

    private byte _GetSelectedStatus()
    {
        if (dgvReceipts.CurrentRow == null) return 0;
        var item = _filteredReceipts.FirstOrDefault(x => x.Id == _GetSelectedId());
        return item?.Status ?? 0;
    }

    private void _OpenForm(Form form)
    {
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    private void frmListCustomerReceipts_Load(object sender, EventArgs e)
    {
        _isInitializing = false;
        _ReloadForm();
        _UpdateClearFiltersState();   // RULE-N148 — حالة الزر بعد أول تحميل
        ApplyGreenTheme();   // RULE-N50 — آخر خطوة في مسار التحميل
    }

    // ── أحداث التصفية (فورية — بلا زر بحث) ─────────────────────────────────────

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void dtpTo_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void chkOnlyUnapplied_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void btnRangeToday_Click(object sender, EventArgs e)
    {
        _SetDateRange(DateTime.Today, DateTime.Today);
        _ApplyFilter();
    }

    private void btnRangeMonth_Click(object sender, EventArgs e)
    {
        _SetDateRange(_StartOfCurrentMonth(), DateTime.Today);
        _ApplyFilter();
    }

    private void btnRange30_Click(object sender, EventArgs e)
    {
        _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);
        _ApplyFilter();
    }

    // ── الإجراءات ──────────────────────────────────────────────────────────────

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateCustomerReceipt>());
    }

    private void btnRefresh_Click(object sender, EventArgs e)
        => _FullRefresh();

    /// <summary>
    /// RULE-N148: «تحديث» = إعادة تحميل كاملة — تصفية افتراضية ← إعادة ضبط الخيارات
    /// ← تحميل الشبكة من الخدمة ← ملخص/تحديد افتراضي. لا يمسح التصفية وحدها أبداً.
    /// </summary>
    private void _FullRefresh()
    {
        // تُنفَّذ إعادة الضبط بصمت حتى لا تُطبَّق تصفية وسيطة على بيانات قديمة قبل التحميل.
        bool wasInitializing = _isInitializing;
        _isInitializing = true;
        try
        {
            _ClearFilterInputs();   // أدوات التصفية تعود للافتراضي
            _LoadStatusOptions();   // خيارات الحالة تُعاد بناؤها
        }
        finally
        {
            _isInitializing = wasInitializing;
        }

        _ReloadForm();          // إعادة تحميل الشبكة والملخص وتحديد السطر
    }

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateCustomerReceipt>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void postToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status != StatusDraft)
        {
            MessageBox.Show("لا يمكن ترحيل السند إلا في حالة مسودة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show("هل أنت متأكد من ترحيل سند القبض؟", "تأكيد الترحيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        _ = PostAsync(id);
    }

    private async Task PostAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<ICustomerReceiptDesktopService>().PostAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم ترحيل سند القبض بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في الترحيل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "حدث خطأ أثناء ترحيل سند القبض");
            MessageBox.Show("حدث خطأ أثناء الترحيل",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status == StatusCancelled)
        {
            MessageBox.Show("السند ملغي بالفعل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show("هل أنت متأكد من إلغاء سند القبض؟", "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        _ = CancelAsync(id);
    }

    private async Task CancelAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<ICustomerReceiptDesktopService>().CancelAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم إلغاء سند القبض بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في الإلغاء", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "حدث خطأ أثناء إلغاء سند القبض");
            MessageBox.Show("حدث خطأ أثناء الإلغاء",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void dgvReceipts_DoubleClick(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateCustomerReceipt>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void cmsReceipts_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsReceipts.Enabled = dgvReceipts.Rows.Count > 0 && dgvReceipts.CurrentRow != null;
    }
}
