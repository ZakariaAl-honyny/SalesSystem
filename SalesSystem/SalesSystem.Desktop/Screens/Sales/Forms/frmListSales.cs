using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

public partial class frmListSales : Form
{
    /// <summary>عنصر قائمة الحالة: قيمة رقمية (byte?) + نص عربي — لا ربط بالـ SelectedIndex.</summary>
    private sealed record StatusOption(byte? Value, string Text);

    private const string CashCustomerName = "عميل نقدي";

    private static readonly Color FocusColor = Color.FromArgb(0xD6, 0xE4, 0xF0);

    private readonly IServiceProvider _serviceProvider;
    private ISalesInvoiceDesktopService _salesApi => _serviceProvider.GetRequiredService<ISalesInvoiceDesktopService>();

    private readonly Dictionary<int, SalesInvoiceDto> _invoicesById = new();
    private List<SalesInvoiceDto> _allInvoices = new();
    private List<SalesInvoiceDto> _filteredInvoices = new();
    private CancellationTokenSource? _refreshCts;

    private int? _selectedInvoiceId;

    // يبدأ true حتى لا ينطلق أي تحميل من داخل المنشئ قبل اكتمال تهيئة الفلاتر.
    private bool _isInitializing = true;
    private bool _toolbarStyled;

    public frmListSales(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        _InitializeKeyboard();
        _ApplyUiTheme();
        _LoadStatusOptions();
        _WireShortcuts();
    }

    // ── Initialisation ─────────────────────────────────────────────────────────

    /// <summary>RULE-N40: الأربعة معاً — Enter/Tab + تحديد القيمة + تظليل الحقل + أسهم RTL.</summary>
    private void _InitializeKeyboard()
    {
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);
        KeyboardHelper.WireArrowNavigation(this);
    }

    /// <summary>الثيم الأخضر — يُطبَّق كآخر خطوة (RULE-N50) مع تثبيت سلوك شبكة العرض (RULE-N22).</summary>
    private void _ApplyUiTheme()
    {
        if (!_toolbarStyled)
        {
            _StyleToolbar();
            _toolbarStyled = true;
        }

        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvSales);

        // RULE-N22/RULE-N52: السلوك تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        dgvSales.ReadOnly = true;
        dgvSales.AllowUserToAddRows = false;
        dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSales.MultiSelect = false;
        dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        emptyState.BringToFront(); // طبقة «لا توجد بيانات» فوق الشبكة
        GreenThemeHelper.Apply(this); // LAST (RULE-N50)
    }

    /// <summary>تنسيق أزرار شريط الأدوات + التلميحات، مرة واحدة فقط.</summary>
    private void _StyleToolbar()
    {
        ThemeHelper.ApplyButtonStyle(btnAdd, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnView, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnPost, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnCancel, ThemeHelper.ButtonType.Danger);
        ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        foreach (var btn in new[] { btnAdd, btnView, btnPost, btnCancel, btnRefresh, btnClose })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false; // يُظهر لون الثيم بدل ألوان النظام
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btn.Size = new Size(140, 40);
            btn.Margin = new Padding(4, 0, 4, 0);
        }

        // النص الأبيض للأزرار الملوّنة فقط (RULE-N21: Tahoma).
        btnAdd.ForeColor = Color.White;
        btnPost.ForeColor = Color.White;
        btnCancel.ForeColor = Color.White;

        toolTip1.SetToolTip(btnAdd, "فاتورة جديدة (F2)");
        toolTip1.SetToolTip(btnView, "عرض أو تعديل الفاتورة المحددة (F3)");
        toolTip1.SetToolTip(btnPost, "ترحيل الفاتورة المحددة (F4)");
        toolTip1.SetToolTip(btnCancel, "إلغاء الفاتورة المحددة (Delete)");
        toolTip1.SetToolTip(btnRefresh, "تحديث القائمة (F5)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة");

        // أزرار النطاقات السريعة — صف ثانٍ داخل صندوق البحث.
        ThemeHelper.ApplyButtonStyle(btnRange30, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnRangeMonth, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnRangeToday, ThemeHelper.ButtonType.Neutral);

        btnRange30.AutoSize = false;
        btnRange30.UseVisualStyleBackColor = false;
        btnRange30.MinimumSize = Size.Empty; // حتى لا يُفرض ارتفاع 35 على صف الأزرار الثاني
        btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRange30.Size = new Size(128, 30);
        btnRangeMonth.AutoSize = false;
        btnRangeMonth.UseVisualStyleBackColor = false;
        btnRangeMonth.MinimumSize = Size.Empty;
        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeMonth.Size = new Size(100, 30);
        btnRangeToday.AutoSize = false;
        btnRangeToday.UseVisualStyleBackColor = false;
        btnRangeToday.MinimumSize = Size.Empty;
        btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeToday.Size = new Size(100, 30);

        toolTip1.SetToolTip(btnRangeToday, "تصفية فواتير اليوم فقط");
        toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");

        // RULE-N43: الأزرار المعطّلة تظهر رمادية باهتة وليس مجرّد Enabled = false.
        KeyboardHelper.WireButtonDisabledVisual(btnView, btnPost, btnCancel);
    }

    /// <summary>قائمة الحالة: «الكل» (null) ثم قيم enum الحقيقية 1/2/3.</summary>
    private void _LoadStatusOptions()
    {
        var options = new List<StatusOption>
        {
            new(null, "الكل"),
            new(1, "مسودة"),
            new(2, "مرحل"),
            new(3, "ملغي")
        };

        cbStatus.DisplayMember = nameof(StatusOption.Text);
        cbStatus.ValueMember = nameof(StatusOption.Value);
        cbStatus.DataSource = options;
        cbStatus.SelectedIndex = 0;
    }

    /// <summary>RULE-N47: اختصارات موحّدة F2/F3/F4/Delete/F5/Esc.</summary>
    private void _WireShortcuts()
    {
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAdd.PerformClick()),
            (Keys.F3, () => { if (btnView.Enabled) btnView.PerformClick(); }),
            (Keys.F4, () => { if (btnPost.Enabled) btnPost.PerformClick(); }),
            (Keys.Delete, () =>
            {
                if (dgvSales.ContainsFocus && btnCancel.Enabled)
                    btnCancel.PerformClick();
            }),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, Close));
    }

    // ── Data Loading ───────────────────────────────────────────────────────────

    private async void _RefreshSalesList()
    {
        if (_isInitializing)
            return;

        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;

        // حماية النطاق: لا ننادي الخدمة إذا كان النطاق معكوساً.
        if (from > to)
        {
            MessageBox.Show("تاريخ البداية «من» يجب أن يكون قبل تاريخ النهاية «إلى» أو مساوياً له.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _refreshCts?.Cancel();
        _refreshCts?.Dispose();
        _refreshCts = new CancellationTokenSource();
        CancellationToken ct = _refreshCts.Token;

        Cursor = Cursors.WaitCursor;
        var loaded = new List<SalesInvoiceDto>();
        bool succeeded = false;

        try
        {
            byte? status = cbStatus.SelectedValue as byte?;

            var result = await _salesApi.GetAllAsync(null, from, to.AddDays(1), status, ct);

            if (ct.IsCancellationRequested)
                return;

            if (result.IsSuccess && result.Value != null)
            {
                loaded = result.Value.ToList();
                succeeded = true;
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل فواتير البيع. يرجى إعادة المحاولة.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (OperationCanceledException)
        {
            return;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة فواتير البيع في شاشة دليل المبيعات");

            if (!ct.IsCancellationRequested)
            {
                MessageBox.Show("تعذر تحميل فواتير البيع. يرجى إعادة المحاولة.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;
        }
        finally
        {
            if (!IsDisposed && _refreshCts != null && _refreshCts.Token == ct)
                Cursor = Cursors.Default;
        }

        if (ct.IsCancellationRequested || IsDisposed)
            return;

        _allInvoices = succeeded ? loaded : new List<SalesInvoiceDto>();
        _ApplyFilters();
    }

    private void _ReloadForm() => _RefreshSalesList();

    // ── Filtering & Grid ───────────────────────────────────────────────────────

    /// <summary>يطبّق التصفية النشطة (نص + «غير المسددة فقط») على القائمة الكاملة.</summary>
    private void _ApplyFilters()
    {
        IEnumerable<SalesInvoiceDto> query = _allInvoices;

        int filterBy = cbFilterBy.SelectedIndex;
        string text = txtFilterValue.Text.Trim();

        if (filterBy > 0 && text.Length > 0)
        {
            query = filterBy switch
            {
                1 => query.Where(i => i.InvoiceNo.ToString().StartsWith(text, StringComparison.Ordinal)),
                2 => query.Where(i => (i.CustomerName ?? CashCustomerName).Contains(text, StringComparison.OrdinalIgnoreCase)),
                3 => query.Where(i => _GetStatusText(i.Status).Contains(text, StringComparison.Ordinal)),
                _ => query
            };
        }

        if (chkOnlyDue.Checked)
            query = query.Where(i => i.DueAmount > 0);

        _filteredInvoices = query.ToList();
        _PopulateGrid();
        _UpdateRecordsCount();
    }

    private void _PopulateGrid()
    {
        int? keepSelectionId = _selectedInvoiceId;

        dgvSales.Rows.Clear();
        _invoicesById.Clear();

        foreach (var inv in _filteredInvoices)
        {
            int rowIndex = dgvSales.Rows.Add(
                inv.InvoiceNo,
                clsFormat.DateToShort(inv.InvoiceDate),
                string.IsNullOrWhiteSpace(inv.CustomerName) ? CashCustomerName : inv.CustomerName,
                inv.TotalAmount.ToString("N2"),
                inv.PaidAmount.ToString("N2"),
                inv.DueAmount.ToString("N2"),
                _GetStatusText(inv.Status),
                inv.Notes ?? string.Empty);

            var row = dgvSales.Rows[rowIndex];

            // RULE: الربط بالمعرّف لا بترتيب السطر.
            row.Tag = inv.Id;
            _invoicesById[inv.Id] = inv;

            // تلوين خفيف لكامل السطر حسب حالة الفاتورة.
            switch (inv.Status)
            {
                case 3: // ملغي
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                    break;
                case 1: // مسودة
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 252, 230);
                    break;
            }
        }

        emptyState.Visible = _filteredInvoices.Count == 0;
        emptyState.BringToFront();

        _RestoreSelection(keepSelectionId);
        _UpdateActionButtons();
    }

    /// <summary>يعيد التحديد على نفس الفاتورة (بالمعرّف) بعد إعادة تعبئة الشبكة.</summary>
    private void _RestoreSelection(int? invoiceId)
    {
        if (dgvSales.Rows.Count == 0)
        {
            _selectedInvoiceId = null;
            return;
        }

        int targetRow = 0;

        if (invoiceId.HasValue)
        {
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.Tag is int id && id == invoiceId.Value)
                {
                    targetRow = row.Index;
                    break;
                }
            }
        }

        dgvSales.ClearSelection();
        var target = dgvSales.Rows[targetRow];
        dgvSales.CurrentCell = target.Cells[0];
        target.Selected = true;
    }

    /// <summary>RULE-N43: آلة حالة الأزرار على أساس الفاتورة المحددة وحالتها.</summary>
    private void _UpdateActionButtons()
    {
        var inv = _GetSelectedInvoice();
        bool hasSelection = inv != null;

        btnView.Enabled = hasSelection;
        btnPost.Enabled = hasSelection && inv!.Status == 1;
        btnCancel.Enabled = hasSelection && inv!.Status != 3;
    }

    /// <summary>ملخص القائمة المصفّاة: العدد + الإجمالي + المدفوع + المتبقي.</summary>
    private void _UpdateRecordsCount()
    {
        decimal total = 0m;
        decimal paid = 0m;
        decimal due = 0m;

        foreach (var inv in _filteredInvoices)
        {
            total += inv.TotalAmount;
            paid += inv.PaidAmount;
            due += inv.DueAmount;
        }

        lblRecordsCount.Text = _filteredInvoices.Count.ToString();
        lblTotalAmount.Text = total.ToString("N2");
        lblTotalPaid.Text = paid.ToString("N2");
        lblTotalDue.Text = due.ToString("N2");
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    /// <summary>يحلّ الفاتورة المحددة عبر معرّفها المحفوظ في Row.Tag (لا بترتيب السطر).</summary>
    private SalesInvoiceDto? _GetSelectedInvoice()
    {
        if (dgvSales.CurrentRow?.Tag is not int invoiceId)
            return null;

        return _invoicesById.TryGetValue(invoiceId, out var invoice) ? invoice : null;
    }

    private static string _GetStatusText(byte status) => status switch
    {
        1 => "مسودة",
        2 => "مرحل",
        3 => "ملغي",
        _ => "غير معروف"
    };

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

        _RefreshSalesList();
    }

    private void _OpenForm(Form form)
    {
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    private async void _PostSelectedAsync()
    {
        var inv = _GetSelectedInvoice();
        if (inv == null)
            return;

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
            var res = await _salesApi.PostAsync(inv.Id);

            if (res.IsSuccess)
            {
                MessageBox.Show("تم الترحيل بنجاح.", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshSalesList();
            }
            else
            {
                MessageBox.Show(res.Error ?? "تعذر ترحيل الفاتورة. يرجى إعادة المحاولة.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل ترحيل فاتورة البيع {InvoiceId} من شاشة الدليل", inv.Id);
            MessageBox.Show("تعذر ترحيل الفاتورة. يرجى إعادة المحاولة.",
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
        if (inv == null)
            return;

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
            var res = await _salesApi.CancelAsync(inv.Id);

            if (res.IsSuccess)
            {
                MessageBox.Show("تم إلغاء الفاتورة بنجاح.", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshSalesList();
            }
            else
            {
                MessageBox.Show(res.Error ?? "تعذر إلغاء الفاتورة. يرجى إعادة المحاولة.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إلغاء فاتورة البيع {InvoiceId} من شاشة الدليل", inv.Id);
            MessageBox.Show("تعذر إلغاء الفاتورة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _refreshCts?.Cancel();
        _refreshCts?.Dispose();
        _refreshCts = null;
        base.OnFormClosed(e);
    }

    // ── Form Events ────────────────────────────────────────────────────────────

    private void frmListSales_Load(object sender, EventArgs e)
    {
        _isInitializing = true;
        try
        {
            dtpFrom.Value = DateTime.Today.AddMonths(-1);
            dtpTo.Value = DateTime.Today;
            cbStatus.SelectedIndex = 0;
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Visible = false;
        }
        finally
        {
            _isInitializing = false;
        }

        _ApplyUiTheme();
        _RefreshSalesList();
    }

    private void btnClose_Click(object sender, EventArgs e) => Close();

    private void btnRefresh_Click(object sender, EventArgs e) => _RefreshSalesList();

    // ── Filter Events ──────────────────────────────────────────────────────────

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        txtFilterValue.Visible = cbFilterBy.SelectedIndex > 0;

        if (txtFilterValue.Visible)
        {
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }

        _ApplyFilters();
    }

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        _ApplyFilters();
    }

    private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            return;
        }

        bool isNumericFilter = cbFilterBy.SelectedIndex == 1;

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

        _RefreshSalesList();
    }

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _RefreshSalesList();
    }

    private void dtpTo_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _RefreshSalesList();
    }

    private void chkOnlyDue_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilters();
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

    // ── Grid Events ────────────────────────────────────────────────────────────

    private void dgvSales_SelectionChanged(object? sender, EventArgs e)
    {
        _selectedInvoiceId = _GetSelectedInvoice()?.Id;
        _UpdateActionButtons();
    }

    private void dgvSales_DoubleClick(object sender, EventArgs e)
    {
        if (_GetSelectedInvoice() != null)
            btnView.PerformClick();
    }

    private void dgvSales_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        var statusColumn = dgvSales.Columns["colStatus"];
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

    // ── Context Menu ───────────────────────────────────────────────────────────

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

    // ── Toolbar Events ─────────────────────────────────────────────────────────

    private void btnAdd_Click(object sender, EventArgs e)
    {
        var editor = _serviceProvider.GetRequiredService<frmAddUpdateSalesInvoice>();
        editor.LoadData(null);
        _OpenForm(editor);
    }

    private void btnView_Click(object sender, EventArgs e)
    {
        var inv = _GetSelectedInvoice();
        if (inv == null)
            return;

        var editor = _serviceProvider.GetRequiredService<frmAddUpdateSalesInvoice>();
        editor.LoadData(inv);
        _OpenForm(editor);
    }

    private void btnPost_Click(object sender, EventArgs e) => _PostSelectedAsync();

    private void btnCancel_Click(object sender, EventArgs e) => _CancelSelectedAsync();
}
