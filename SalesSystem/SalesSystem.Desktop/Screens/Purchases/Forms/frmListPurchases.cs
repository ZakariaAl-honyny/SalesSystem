using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Purchases;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

public partial class frmListPurchases : Form
{
    // ── ثوابت التصفية (RULE-N60: التصفية بالفهرس لا بمقارنة نص عربي) ──
    private const int FilterNone = 0;
    private const int FilterSupplier = 1;
    private const int FilterInvoiceNo = 2;
    private const int FilterNotes = 3;
    private const int FilterAmount = 4;

    /// <summary>RULE-N46: لون الحقل النشط الموحّد.</summary>
    private static readonly Color FocusColor = Color.FromArgb(0xD6, 0xE4, 0xF0);

    private readonly IServiceProvider _serviceProvider;
    private IPurchaseInvoiceApiService _purchaseApi => _serviceProvider.GetRequiredService<IPurchaseInvoiceApiService>();

    /// <summary>ربط السطر بالفاتورة بالمعرّف لا بترتيب السطر.</summary>
    private readonly Dictionary<int, PurchaseInvoiceDto> _invoicesById = new();

    private List<PurchaseInvoiceDto> _allInvoices = new();
    private List<PurchaseInvoiceDto> _filteredInvoices = new();
    private int? _selectedInvoiceId;

    private bool _isInitializing = true;
    private bool _toolbarStyled;

    public frmListPurchases(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40 / RULE-N45 / RULE-N46 — تنقل وتحديد وتظليل الحقل النشط.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N22 — سلوك شبكة العرض تعلنه الشاشة (ApplyGrid لا يلمسه، RULE-N52).
        dgvPurchases.ReadOnly = true;
        dgvPurchases.AllowUserToAddRows = false;
        dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPurchases.MultiSelect = false;

        _SetupGridColumns();
        _WireShortcuts();       // RULE-N47
        ApplyUiTheme();         // RULE-N50 — ينتهي بالثيم الأخضر

        // RULE-N43 — بعد تلوين الأزرار حتى يلتقط الـ Helper لون الثيم الصحيح.
        KeyboardHelper.WireButtonDisabledVisual(btnView, btnPost, btnCancel);
    }

    // ── التهيئة ────────────────────────────────────────────────────────────────

    /// <summary>RULE-N47: اختصارات موحّدة — F2 جديد · F3 عرض/تعديل · F4 ترحيل · Delete إلغاء · F5 تحديث · Esc مسح التصفية.</summary>
    private void _WireShortcuts()
    {
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAdd.PerformClick()),
            (Keys.F3, () => { if (btnView.Enabled) btnView.PerformClick(); }),
            (Keys.F4, () => { if (btnPost.Enabled) btnPost.PerformClick(); }),
            (Keys.Delete, () =>
            {
                if (dgvPurchases.ContainsFocus && btnCancel.Enabled)
                    btnCancel.PerformClick();
            }),
            (Keys.F5, () => _RefreshPurchasesList()),
            (Keys.Escape, _ClearFilters));
    }

    /// <summary>الثيم الأخضر — ينتهي بـ <c>GreenThemeHelper.Apply(this)</c> كآخر سطر (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        if (!_toolbarStyled)
        {
            _StyleToolbar();
            _toolbarStyled = true;
        }

        GreenThemeHelper.ApplyHeader(lblMode);          // RULE-N53
        GreenThemeHelper.ApplyGrid(dgvPurchases);       // RULE-N51

        // RULE-N22/RULE-N52: ApplyGrid لا يلمس السلوك — نؤكّده بعد التلوين.
        dgvPurchases.ReadOnly = true;
        dgvPurchases.AllowUserToAddRows = false;
        dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPurchases.MultiSelect = false;
        dgvPurchases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // نصوص شريط التصفية والملخص: اللون من الثيم لا من أرقام داخل الشاشة (RULE-N54).
        foreach (var lbl in new[] { label1, label2, label3, label4, label5, lblSummary, lblRecordsCount })
            lbl.ForeColor = GreenThemeHelper.TextStrong;

        foreach (var chk in new[] { chkOnlyDue, chkOnlyCancelled })
            chk.ForeColor = GreenThemeHelper.TextStrong;

        emptyState.ForeColor = GreenThemeHelper.TextStrong;
        emptyState.BringToFront();                      // طبقة «لا توجد بيانات» فوق الشبكة

        GreenThemeHelper.Apply(this);                   // ← الأخير (RULE-N50)
    }

    /// <summary>تنسيق أزرار الإجراءات والنطاقات السريعة + تلميحاتها (RULE-N47).</summary>
    private void _StyleToolbar()
    {
        ThemeHelper.ApplyButtonStyle(btnAdd, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnView, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnPost, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnCancel, ThemeHelper.ButtonType.Danger);
        ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        foreach (var btn in new[] { btnAdd, btnView, btnPost, btnCancel, btnRefresh, btnClose })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btn.Size = new Size(140, 40);
            btn.Margin = new Padding(4, 0, 4, 0);
        }

        btnAdd.ForeColor = Color.White;
        btnPost.ForeColor = Color.White;
        btnCancel.ForeColor = Color.White;
        btnClose.ForeColor = Color.White;

        toolTip1.SetToolTip(btnAdd, "فاتورة مشتريات جديدة (F2)");
        toolTip1.SetToolTip(btnView, "عرض أو تعديل الفاتورة المحددة (F3)");
        toolTip1.SetToolTip(btnPost, "ترحيل الفاتورة المحددة (F4)");
        toolTip1.SetToolTip(btnCancel, "إلغاء الفاتورة المحددة (Delete)");
        toolTip1.SetToolTip(btnRefresh, "تحديث القائمة (F5)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة");

        // أزرار النطاقات السريعة — الصف الثاني داخل gbFilter (RULE-N57).
        foreach (var rangeBtn in new[] { btnRangeToday, btnRange30, btnRangeMonth })
        {
            ThemeHelper.ApplyButtonStyle(rangeBtn, ThemeHelper.ButtonType.Neutral);
            rangeBtn.AutoSize = false;
            rangeBtn.UseVisualStyleBackColor = false;
            rangeBtn.MinimumSize = Size.Empty;
            rangeBtn.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            rangeBtn.Height = 30;
            rangeBtn.ForeColor = Color.White;
        }

        btnRangeToday.Size = new Size(100, 30);
        btnRange30.Size = new Size(128, 30);
        btnRangeMonth.Size = new Size(100, 30);

        toolTip1.SetToolTip(btnRangeToday, "تصفية فواتير اليوم فقط");
        toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
        toolTip1.SetToolTip(cbFilterBy, "اختر الحقل المطلوب البحث فيه");
        toolTip1.SetToolTip(txtFilterValue, "اكتب قيمة البحث (Enter يطبّق · Esc يمسح)");
        toolTip1.SetToolTip(chkOnlyDue, "إظهار الفواتير التي عليها مبلغ متبقٍ فقط");
        toolTip1.SetToolTip(chkOnlyCancelled, "إظهار الفواتير الملغاة فقط");
    }

    // ── تحميل البيانات ─────────────────────────────────────────────────────────

    private async void _RefreshPurchasesList()
    {
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;

        // حماية النطاق: لا ننادي الخدمة إذا كان النطاق معكوساً.
        if (from > to)
        {
            MessageBox.Show("تاريخ البداية «من» يجب أن يكون قبل تاريخ النهاية «إلى» أو مساوياً له.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Cursor = Cursors.WaitCursor;
        var loaded = new List<PurchaseInvoiceDto>();
        bool succeeded = false;

        try
        {
            byte? status = cbStatus.SelectedIndex > 0 ? (byte?)cbStatus.SelectedIndex : null;

            var result = await _purchaseApi.GetAllAsync(
                null, from, to.AddDays(1), status);

            if (result.IsSuccess && result.Value != null)
            {
                loaded = result.Value.ToList();
                succeeded = true;
            }
            else
            {
                MessageBox.Show(result.Error ?? "حدث خطأ في تحميل البيانات", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "حدث خطأ أثناء تحميل فواتير الشراء");
            MessageBox.Show("حدث خطأ أثناء تحميل البيانات",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }

        if (IsDisposed)
            return;

        _allInvoices = succeeded ? loaded : new List<PurchaseInvoiceDto>();
        _ApplyFilter();
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshPurchasesList();
    }

    // ── التصفية الفورية (RULE-N59) ─────────────────────────────────────────────

    /// <summary>يطبّق التصفية النشطة (نص + غير المسددة + الملغاة) على القائمة الكاملة.</summary>
    private void _ApplyFilter()
    {
        IEnumerable<PurchaseInvoiceDto> query = _allInvoices;

        int filterBy = cbFilterBy.SelectedIndex;
        string text = txtFilterValue.Text.Trim();

        if (filterBy > FilterNone && text.Length > 0)
        {
            query = filterBy switch
            {
                FilterSupplier => query.Where(i => (i.SupplierName ?? string.Empty)
                    .Contains(text, StringComparison.OrdinalIgnoreCase)),

                FilterInvoiceNo => query.Where(i => i.InvoiceNo.ToString()
                    .StartsWith(text, StringComparison.Ordinal)),

                FilterNotes => query.Where(i => (i.Notes ?? string.Empty)
                    .Contains(text, StringComparison.OrdinalIgnoreCase)),

                FilterAmount => query.Where(i =>
                    i.NetTotal.ToString("N2").Contains(text, StringComparison.Ordinal) ||
                    i.NetTotal.ToString("0.##").StartsWith(text, StringComparison.Ordinal)),

                _ => query
            };
        }

        if (chkOnlyDue.Checked)
            query = query.Where(i => i.RemainingAmount > 0);

        if (chkOnlyCancelled.Checked)
            query = query.Where(i => i.Status == 3);

        _filteredInvoices = query.ToList();
        _PopulateGrid();
    }

    private void _PopulateGrid()
    {
        int? keepSelectionId = _selectedInvoiceId;

        dgvPurchases.Rows.Clear();
        _invoicesById.Clear();

        foreach (var inv in _filteredInvoices)
        {
            int rowIndex = dgvPurchases.Rows.Add(
                inv.InvoiceNo,
                clsFormat.DateToShort(inv.InvoiceDate.ToDateTime(TimeOnly.MinValue)),
                inv.SupplierName ?? "",
                inv.TotalAmount.ToString("N2"),
                inv.PaidAmount.ToString("N2"),
                inv.DueAmount.ToString("N2"),
                _GetStatusText(inv.Status)
            );

            // ربط السطر بالمعرّف لا بالترتيب.
            dgvPurchases.Rows[rowIndex].Tag = inv.Id;
            _invoicesById[inv.Id] = inv;
        }

        emptyState.Visible = _filteredInvoices.Count == 0;
        if (emptyState.Visible)
            emptyState.BringToFront();

        _RestoreSelection(keepSelectionId);
        _UpdateSummary();
        _UpdateActionButtons();
    }

    /// <summary>يعيد التحديد على نفس الفاتورة (بالمعرّف) بعد إعادة تعبئة الشبكة.</summary>
    private void _RestoreSelection(int? invoiceId)
    {
        if (dgvPurchases.Rows.Count == 0)
        {
            _selectedInvoiceId = null;
            return;
        }

        int targetRow = 0;

        if (invoiceId.HasValue)
        {
            foreach (DataGridViewRow row in dgvPurchases.Rows)
            {
                if (row.Tag is int id && id == invoiceId.Value)
                {
                    targetRow = row.Index;
                    break;
                }
            }
        }

        dgvPurchases.ClearSelection();
        var target = dgvPurchases.Rows[targetRow];
        dgvPurchases.CurrentCell = target.Cells[0];
        target.Selected = true;
    }

    /// <summary>RULE-N43: آلة حالة الأزرار حسب السطر المحدد وحالة الفاتورة.</summary>
    private void _UpdateActionButtons()
    {
        var inv = _GetSelectedInvoice();
        bool hasSelection = inv != null;

        btnView.Enabled = hasSelection;
        btnPost.Enabled = hasSelection && inv!.Status == 1;
        btnCancel.Enabled = hasSelection && inv!.Status != 3;
    }

    /// <summary>RULE-N61: شريط الملخص — المعروض من الإجمالي + الصافي/المدفوع/المتبقي (N2).</summary>
    private void _UpdateSummary()
    {
        decimal net = 0m;
        decimal paid = 0m;
        decimal due = 0m;

        foreach (var inv in _filteredInvoices)
        {
            net += inv.NetTotal;
            paid += inv.PaidAmount;
            due += inv.RemainingAmount;
        }

        lblSummary.Text =
            $"المعروض: {_filteredInvoices.Count} من {_allInvoices.Count} — الصافي: {net.ToString("N2")} · " +
            $"المدفوع: {paid.ToString("N2")} · المتبقي: {due.ToString("N2")}";

        lblRecordsCount.Text = _filteredInvoices.Count.ToString();
    }

    // ── إعداد الشبكة ───────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        if (dgvPurchases.Columns.Count > 0)
            return;

        dgvPurchases.Columns.Add("InvoiceNo", "رقم الفاتورة");
        dgvPurchases.Columns.Add("InvoiceDate", "التاريخ");
        dgvPurchases.Columns.Add("SupplierName", "المورد");
        dgvPurchases.Columns.Add("TotalAmount", "الإجمالي");
        dgvPurchases.Columns.Add("PaidAmount", "المدفوع");
        dgvPurchases.Columns.Add("DueAmount", "المتبقي");
        dgvPurchases.Columns.Add("StatusName", "الحالة");

        foreach (var col in dgvPurchases.Columns)
        {
            if (col is DataGridViewColumn c)
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            if (col is DataGridViewTextBoxColumn txtCol)
                txtCol.ReadOnly = true;
        }
    }

    private static string _GetStatusText(byte status) => status switch
    {
        1 => "مسودة",
        2 => "مرحل",
        3 => "ملغي",
        _ => "غير معروف"
    };

    // ── مساعدات ────────────────────────────────────────────────────────────────

    /// <summary>يحلّ الفاتورة المحددة عبر المعرّف المحفوظ في Row.Tag.</summary>
    private PurchaseInvoiceDto? _GetSelectedInvoice()
    {
        if (dgvPurchases.CurrentRow?.Tag is not int invoiceId)
            return null;

        return _invoicesById.TryGetValue(invoiceId, out var invoice) ? invoice : null;
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

        _RefreshPurchasesList();
    }

    /// <summary>Esc: يمسح كل أدوات التصفية ويعيد النطاق إلى «هذا الشهر» (RULE-N59).</summary>
    private void _ClearFilters()
    {
        _isInitializing = true;
        try
        {
            cbFilterBy.SelectedIndex = FilterNone;
            txtFilterValue.Clear();
            txtFilterValue.Visible = false;
            cbStatus.SelectedIndex = 0;
            chkOnlyDue.Checked = false;
            chkOnlyCancelled.Checked = false;

            var today = DateTime.Today;
            dtpFrom.Value = new DateTime(today.Year, today.Month, 1);
            dtpTo.Value = today;
        }
        finally
        {
            _isInitializing = false;
        }

        _RefreshPurchasesList();
    }

    private void _OpenForm(Form form)
    {
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    private async void _PostSelectedAsync()
    {
        var inv = _GetSelectedInvoice();
        if (inv == null) return;

        if (inv.Status != 1)
        {
            MessageBox.Show("يمكن ترحيل الفواتير المسودة فقط.", "تنبيه",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"هل تريد ترحيل الفاتورة رقم {inv.InvoiceNo}؟",
                "تأكيد الترحيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        try
        {
            var res = await _purchaseApi.PostAsync(inv.Id);
            if (res.IsSuccess)
            {
                MessageBox.Show("تم الترحيل بنجاح", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshPurchasesList();
            }
            else
            {
                MessageBox.Show(res.Error ?? "حدث خطأ أثناء الترحيل.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "حدث خطأ أثناء ترحيل فاتورة الشراء {InvoiceId}", inv.Id);
            MessageBox.Show("حدث خطأ أثناء الترحيل",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private async void _CancelSelectedAsync()
    {
        var inv = _GetSelectedInvoice();
        if (inv == null) return;

        if (inv.Status == 3)
        {
            MessageBox.Show("الفاتورة ملغية بالفعل.", "تنبيه",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"هل تريد إلغاء الفاتورة رقم {inv.InvoiceNo}؟",
                "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        try
        {
            var res = await _purchaseApi.CancelAsync(inv.Id);
            if (res.IsSuccess)
            {
                MessageBox.Show("تم الإلغاء بنجاح", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshPurchasesList();
            }
            else
            {
                MessageBox.Show(res.Error ?? "حدث خطأ أثناء الإلغاء.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "حدث خطأ أثناء إلغاء فاتورة الشراء {InvoiceId}", inv.Id);
            MessageBox.Show("حدث خطأ أثناء الإلغاء",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    // ── أحداث النموذج ──────────────────────────────────────────────────────────

    private void frmListPurchases_Load(object sender, EventArgs e)
    {
        _isInitializing = true;
        try
        {
            var today = DateTime.Today;
            dtpFrom.Value = new DateTime(today.Year, today.Month, 1);
            dtpTo.Value = today;
            cbFilterBy.SelectedIndex = FilterNone;
            txtFilterValue.Visible = false;
            cbStatus.SelectedIndex = 0;
            chkOnlyDue.Checked = false;
            chkOnlyCancelled.Checked = false;
        }
        finally
        {
            _isInitializing = false;
        }

        ApplyUiTheme();          // RULE-N50 — آمن للاستدعاء المتكرر (idempotent)
        _RefreshPurchasesList();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnRefresh_Click(object sender, EventArgs e)
        => _RefreshPurchasesList();

    // ── أحداث التصفية ──────────────────────────────────────────────────────────

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        txtFilterValue.Visible = cbFilterBy.SelectedIndex > FilterNone;

        if (txtFilterValue.Visible)
        {
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }

        _ApplyFilter();
    }

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
            return;
        }

        bool isNumericFilter = cbFilterBy.SelectedIndex is FilterInvoiceNo or FilterAmount;

        if (isNumericFilter && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
        {
            e.Handled = true;
            clsInputFeedback.ShowRejectTooltip(
                txtFilterValue,
                "يُسمح فقط بإدخال الأرقام في هذا الحقل.");
            return;
        }

        if (!isNumericFilter && char.IsPunctuation(e.KeyChar))
        {
            e.Handled = true;
            clsInputFeedback.ShowRejectTooltip(
                txtFilterValue,
                "لا يُسمح بإدخال علامات الترقيم.");
        }
    }

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _RefreshPurchasesList();
    }

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _RefreshPurchasesList();
    }

    private void dtpTo_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _RefreshPurchasesList();
    }

    private void chkOnlyDue_CheckedChanged(object sender, EventArgs e)
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
    {
        var today = DateTime.Today;
        _SetDateRange(new DateTime(today.Year, today.Month, 1), today);
    }

    private void btnRange30_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

    // ── أحداث الشبكة ───────────────────────────────────────────────────────────

    private void dgvPurchases_SelectionChanged(object? sender, EventArgs e)
    {
        _selectedInvoiceId = _GetSelectedInvoice()?.Id;
        _UpdateActionButtons();
    }

    private void dgvPurchases_DoubleClick(object sender, EventArgs e)
    {
        var inv = _GetSelectedInvoice();
        if (inv != null)
            btnView.PerformClick();
    }

    private void dgvPurchases_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        var statusColumn = dgvPurchases.Columns["StatusName"];
        if (statusColumn == null || e.ColumnIndex != statusColumn.Index || e.Value == null)
            return;

        e.CellStyle.ForeColor = e.Value.ToString() switch
        {
            "مرحل" => Color.FromArgb(25, 135, 84),
            "ملغي" => Color.FromArgb(176, 42, 55),
            _ => Color.FromArgb(13, 110, 253)
        };
        e.CellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
    }

    // ── قائمة السياق (RULE-N23) ────────────────────────────────────────────────

    private void ctxMenu_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
        => _UpdateActionButtons();

    private void tsmiView_Click(object? sender, EventArgs e) => btnView.PerformClick();

    private void tsmiPost_Click(object? sender, EventArgs e) => btnPost.PerformClick();

    private void tsmiCancel_Click(object? sender, EventArgs e) => btnCancel.PerformClick();

    private void tsmiCopyInvoiceNo_Click(object? sender, EventArgs e)
    {
        var inv = _GetSelectedInvoice();
        if (inv == null)
            return;

        try
        {
            Clipboard.SetText(inv.InvoiceNo.ToString());
            MessageBox.Show("تم نسخ رقم الفاتورة إلى الحافظة.", "نسخ",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر نسخ رقم الفاتورة {InvoiceNo} إلى الحافظة", inv.InvoiceNo);
            MessageBox.Show("تعذر النسخ إلى الحافظة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── أزرار شريط الإجراءات ───────────────────────────────────────────────────

    private void btnAdd_Click(object sender, EventArgs e)
    {
        var editor = _serviceProvider.GetRequiredService<frmPurchaseInvoice>();
        editor.LoadData(null);
        _OpenForm(editor);
    }

    private void btnView_Click(object sender, EventArgs e)
    {
        var inv = _GetSelectedInvoice();
        if (inv == null) return;

        var editor = _serviceProvider.GetRequiredService<frmPurchaseInvoice>();
        editor.LoadData(inv);
        _OpenForm(editor);
    }

    private void btnPost_Click(object sender, EventArgs e)
        => _PostSelectedAsync();

    private void btnCancel_Click(object sender, EventArgs e)
        => _CancelSelectedAsync();
}
