using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// منتقي فاتورة شراء (TASK-016): قائمة الفواتير + **بيانات الفاتورة الكاملة** (رأس + سطور)
/// ثم «اختيار» يُعيد الفاتورة إلى شاشة مرتجع الشراء لتعبئتها.
/// <para>
/// لا مسار بيانات جديد: العقدان القائمان يكفيان (<c>GetAllAsync</c> للقائمة و<c>GetByIdAsync</c>
/// للسطور) — فلا `Business/**` ولا `DataAccess/**` ولا `Services/**` (RULE-N165 مستثنى).
/// </para>
/// <para>
/// ولا حساب مالي هنا: كل مبلغ يُعرض من الـDTO عبر <see cref="clsFormat"/> — لا جمع ولا طرح
/// ولا تصفية محلية على مهمة مالية (التصفية كلها في الخدمة).
/// </para>
/// </summary>
public partial class frmPurchaseInvoiceSelection : Form
{
    /// <summary>عنصر قائمة الحالة: قيمة رقمية (byte?) + نص عربي — لا ربط بالـ SelectedIndex.</summary>
    private sealed record StatusOption(byte? Value, string Text);

    private const string NoInvoiceSelectedText = "لم تُحدَّد فاتورة";

    private static Color FocusColor => ThemePalette.DefaultFor(ThemePalette.SelectionRowColor);

    private readonly Dictionary<int, PurchaseInvoiceDto> _invoicesById = new();
    private List<PurchaseInvoiceDto> _invoices = new();

    private readonly DateTime _defaultFrom;
    private readonly DateTime _defaultTo;

    private CancellationTokenSource? _listCts;
    private CancellationTokenSource? _detailCts;

    private IServiceProvider? _serviceProvider;

    /// <summary>الفاتورة الكاملة (بالسطور) بعد <c>GetByIdAsync</c> — هي ما يُعاد للمستدعي.</summary>
    private PurchaseInvoiceDto? _detailInvoice;
    private int? _detailInvoiceId;

    private int? _selectedInvoiceId;

    // يبدأ true حتى لا ينطلق أي تحميل من داخل المنشئ قبل اكتمال تهيئة أدوات التصفية.
    private bool _isInitializing = true;

    // أثناء إعادة تعبئة الشبكة تتغيّر الخلية الحالية عشرات المرات — فلا نطلب تفاصيل
    // إلا مرة واحدة بعد استقرار التحديد (بدل سلسلة نداءات تُلغى إحداها الأخرى).
    private bool _populating;

    /// <summary>الفاتورة المختارة كاملةً (رأس + سطور) — تُقرأ من شاشة المرتجع بعد <c>DialogResult.OK</c>.</summary>
    public PurchaseInvoiceDto? SelectedInvoice { get; private set; }

    private IPurchaseInvoiceDesktopService PurchaseApi =>
        _serviceProvider!.GetRequiredService<IPurchaseInvoiceDesktopService>();

    /// <summary>مُنشئ التصميم — بلا حاوية خدمات (يفتح في مصمّم VS فقط).</summary>
    private frmPurchaseInvoiceSelection()
    {
        _defaultFrom = DateTime.Today.AddMonths(-12);
        _defaultTo = DateTime.Today;

        InitializeComponent();
        _InitializeKeyboard();
        _LoadStatusOptions();
    }

    public frmPurchaseInvoiceSelection(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // RULE-N63/RULE-N145: يتبنّى btnToggleFilters المُعلَن في المصمّم ويوصِّل طيّ التصفية + F9
        // وحفظ الحالة. آخر سطر في المُنشئ العام.
        FilterPanelHelper.Attach(this, serviceProvider);
    }

    // ── التهيئة ────────────────────────────────────────────────────────────────

    /// <summary>RULE-N40…RULE-N47 + RULE-N95: الستة كاملة + ↓ من البحث إلى القائمة (منتقي).</summary>
    private void _InitializeKeyboard()
    {
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);   // RULE-N46 + RULE-N54b — لون الحقل النشط حيّاً
        KeyboardHelper.WireArrowNavigation(this);              // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnSelect, btnClearFilters);   // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,               // RULE-N47 — Esc يملكه CancelButton
            (Keys.F5, _RefreshInvoicesList));

        KeyboardHelper.WireSearchToGrid(txtSearch, dgvInvoices);
    }

    /// <summary>
    /// قائمة الحالة: «الكل» (null) ثم قيم enum الحقيقية 1/2/3 من البيت الواحد
    /// <see cref="DocumentTextHelper.StatusOptions"/> — لا تُكتب الأزواج في الشاشة (RULE-N169).
    /// </summary>
    private void _LoadStatusOptions()
    {
        var options = new List<StatusOption> { new(null, "الكل") };   // العنصر المحايد محلّي — ليس نص حالة

        foreach (var (value, text) in DocumentTextHelper.StatusOptions)
            options.Add(new(value, text));

        cbStatus.DisplayMember = nameof(StatusOption.Text);
        cbStatus.ValueMember = nameof(StatusOption.Value);
        cbStatus.DataSource = options;
        cbStatus.SelectedIndex = 0;
    }

    /// <summary>الثيم (RULE-N50…RULE-N53): الشبكتان + الملخص، ثم <c>Apply(this)</c> آخر سطر.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvInvoices);
        GreenThemeHelper.ApplyGrid(dgvLines);

        lblRecordsCount.ForeColor = GreenThemeHelper.TextStrong;      // RULE-N53
        lblInvoiceSummary.ForeColor = GreenThemeHelper.TextStrong;

        // RULE-N22/RULE-N52: سلوك الشبكتين تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        dgvInvoices.ReadOnly = true;
        dgvInvoices.AllowUserToAddRows = false;
        dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvInvoices.MultiSelect = false;
        dgvInvoices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        dgvLines.ReadOnly = true;
        dgvLines.AllowUserToAddRows = false;
        dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLines.MultiSelect = false;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ── تحميل القائمة ──────────────────────────────────────────────────────────

    /// <summary>
    /// قراءة القائمة من الخدمة بالمعايير الحالية (لا تصفية محلية — TASK-016 §5.2)،
    /// مع إلغاء أي قراءة سابقة لم تكتمل حتى لا يتغلّب ردٌّ قديم على الأحدث.
    /// </summary>
    private async void _RefreshInvoicesList()
    {
        if (_isInitializing || _serviceProvider is null)
            return;

        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;

        // حماية النطاق: لا ننادي الخدمة إذا كان النطاق معكوساً.
        if (from > to)
        {
            clsDialogs.Warning("تاريخ البداية «من» يجب أن يكون قبل تاريخ النهاية «إلى» أو مساوياً له.");
            return;
        }

        _listCts?.Cancel();
        _listCts?.Dispose();
        _listCts = new CancellationTokenSource();
        CancellationToken ct = _listCts.Token;

        Cursor = Cursors.WaitCursor;
        var loaded = new List<PurchaseInvoiceDto>();
        bool succeeded = false;

        try
        {
            byte? status = cbStatus.SelectedValue as byte?;
            string? search = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();

            // «إلى» حدّ شامل في العقد والتنفيذين (`InvoiceDate <= to`) — فلا يُزاد يوماً.
            var result = await PurchaseApi.GetAllAsync(search, from, to, status, ct);

            if (ct.IsCancellationRequested)
                return;

            if (result.IsSuccess && result.Value != null)
            {
                loaded = result.Value.ToList();
                succeeded = true;
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل فواتير الشراء. يرجى إعادة المحاولة.");
            }
        }
        catch (OperationCanceledException)
        {
            return;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة فواتير الشراء في شاشة اختيار الفاتورة");

            if (!ct.IsCancellationRequested)
            {
                clsDialogs.Error("تعذر تحميل فواتير الشراء. يرجى إعادة المحاولة.");
            }
            return;
        }
        finally
        {
            if (!IsDisposed && _listCts != null && _listCts.Token == ct)
                Cursor = Cursors.Default;
        }

        if (ct.IsCancellationRequested || IsDisposed)
            return;

        _invoices = succeeded ? loaded : new List<PurchaseInvoiceDto>();
        _PopulateInvoicesGrid();
    }

    private void _PopulateInvoicesGrid()
    {
        int? keepSelectionId = _selectedInvoiceId;

        _populating = true;
        try
        {
            dgvInvoices.Rows.Clear();
            _invoicesById.Clear();

            foreach (var inv in _invoices)
            {
                int rowIndex = dgvInvoices.Rows.Add(
                    inv.Id,
                    inv.InvoiceNo,
                    clsFormat.DateToShort(inv.InvoiceDate.ToDateTime(TimeOnly.MinValue)),
                    inv.SupplierName ?? string.Empty,
                    inv.PaymentTypeDisplay,
                    clsFormat.Money(inv.TotalAmount),
                    clsFormat.Money(inv.PaidAmount),
                    clsFormat.Money(inv.DueAmount),
                    DocumentTextHelper.StatusText(inv.Status));

                var row = dgvInvoices.Rows[rowIndex];

                // RULE: الربط بالمعرّف لا بترتيب السطر.
                row.Tag = inv.Id;
                _invoicesById[inv.Id] = inv;

                // تلوين خفيف لكامل السطر حسب الحالة — وتصفير صريح للسطر السليم
                // حتى لا يبقى لون سطر سابق على سطر معاد استخدامه.
                row.DefaultCellStyle.BackColor = inv.Status switch
                {
                    3 => GreenThemeHelper.StatusDangerBack,
                    1 => GreenThemeHelper.StatusWarningBack,
                    _ => Color.Empty
                };
            }
        }
        finally
        {
            _populating = false;
        }

        lblRecordsCount.Text = $"النتائج: {clsFormat.Number(_invoices.Count)}";

        lblNoData.Visible = _invoices.Count == 0;
        if (lblNoData.Visible)
            lblNoData.BringToFront();

        _RestoreSelection(keepSelectionId);
        _UpdateActionButtons();
        _UpdateClearFiltersState();
        _LoadInvoiceDetailsAsync(_GetSelectedInvoice()?.Id);   // تفاصيل واحدة بعد استقرار التحديد
    }

    /// <summary>يعيد التحديد على نفس الفاتورة (بالمعرّف) بعد إعادة تعبئة الشبكة.</summary>
    private void _RestoreSelection(int? invoiceId)
    {
        if (dgvInvoices.Rows.Count == 0)
        {
            _selectedInvoiceId = null;
            _ClearDetails();
            return;
        }

        int targetRow = 0;

        if (invoiceId.HasValue)
        {
            foreach (DataGridViewRow row in dgvInvoices.Rows)
            {
                if (row.Tag is int id && id == invoiceId.Value)
                {
                    targetRow = row.Index;
                    break;
                }
            }
        }

        dgvInvoices.ClearSelection();
        var target = dgvInvoices.Rows[targetRow];
        dgvInvoices.CurrentCell = target.Cells[colInvNo.Index];
        target.Selected = true;
    }

    // ── بيانات الفاتورة المحددة (الرأس + السطور) ───────────────────────────────

    /// <summary>
    /// نداء <c>GetByIdAsync</c> واحد يعطي الفاتورة كاملة بسطورها (TASK-016 §5.3).
    /// فشله ⇒ رسالة عربية والقائمة تبقى كما هي بلا مسح.
    /// </summary>
    private async void _LoadInvoiceDetailsAsync(int? invoiceId)
    {
        _detailCts?.Cancel();
        _detailCts?.Dispose();
        _detailCts = null;

        if (!invoiceId.HasValue || invoiceId.Value <= 0 || _serviceProvider is null)
        {
            _ClearDetails();
            return;
        }

        var cts = new CancellationTokenSource();
        _detailCts = cts;
        CancellationToken ct = cts.Token;

        Cursor = Cursors.WaitCursor;

        try
        {
            var result = await PurchaseApi.GetByIdAsync(invoiceId.Value, ct);

            if (ct.IsCancellationRequested || IsDisposed)
                return;

            if (!result.IsSuccess || result.Value is null)
            {
                _ClearDetails();
                clsDialogs.Error(result.Error ?? "تعذر تحميل بيانات الفاتورة المحددة. يرجى إعادة المحاولة.");
                return;
            }

            _detailInvoice = result.Value;
            _detailInvoiceId = result.Value.Id;
            _ShowInvoiceDetails(result.Value);
        }
        catch (OperationCanceledException)
        {
            // اختيار أحدث ألغى هذا النداء — لا رسالة.
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل تفاصيل فاتورة الشراء {InvoiceId} في شاشة الاختيار", invoiceId.Value);

            if (!ct.IsCancellationRequested && !IsDisposed)
                clsDialogs.Error("تعذر تحميل بيانات الفاتورة المحددة. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed && ReferenceEquals(_detailCts, cts))
                Cursor = Cursors.Default;
        }
    }

    /// <summary>ملء شبكة السطور + رأس الفاتورة — عرض فقط من الـDTO بلا أي حساب.</summary>
    private void _ShowInvoiceDetails(PurchaseInvoiceDto invoice)
    {
        lblInvoiceSummary.Text =
            $"فاتورة رقم {invoice.InvoiceNo}"
            + $" · {clsFormat.DateToShort(invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue))}"
            + $" · {invoice.SupplierName}"
            + $" · نوع الدفع: {invoice.PaymentTypeDisplay}"
            + $" · الإجمالي: {clsFormat.Money(invoice.TotalAmount)}"
            + $" · المدفوع: {clsFormat.Money(invoice.PaidAmount)}"
            + $" · المتبقي: {clsFormat.Money(invoice.DueAmount)}"
            + $" · الحالة: {DocumentTextHelper.StatusText(invoice.Status)}";

        dgvLines.Rows.Clear();

        var items = invoice.Items;

        if (items is null)
            return;

        foreach (var line in items)
        {
            dgvLines.Rows.Add(
                line.ProductName,
                clsFormat.Quantity(line.Quantity),
                clsFormat.Money(line.LandedUnitCost),   // سعر التكلفة الواصل — العضو الحقيقي في الـDTO
                clsFormat.Money(line.DiscountAmount),
                clsFormat.Money(line.LineTotal));
        }
    }

    /// <summary>يمسح تفاصيل الفاتورة المحددة (لا تُمسّ القائمة).</summary>
    private void _ClearDetails()
    {
        _detailInvoice = null;
        _detailInvoiceId = null;

        dgvLines.Rows.Clear();
        lblInvoiceSummary.Text = NoInvoiceSelectedText;
    }

    // ── المساعدات ──────────────────────────────────────────────────────────────

    /// <summary>يحلّ الفاتورة المحددة عبر معرّفها المحفوظ في Row.Tag (لا بترتيب السطر).</summary>
    private PurchaseInvoiceDto? _GetSelectedInvoice()
    {
        if (dgvInvoices.CurrentRow?.Tag is not int invoiceId)
            return null;

        return _invoicesById.TryGetValue(invoiceId, out var invoice) ? invoice : null;
    }

    /// <summary>RULE-N43: «اختيار» مفعّل فقط عند وجود فاتورة محددة.</summary>
    private void _UpdateActionButtons()
    {
        btnSelect.Enabled = _GetSelectedInvoice() is not null;
    }

    /// <summary>RULE-N148/RULE-N43: «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtSearch.Text)
            || cbStatus.SelectedIndex > 0
            || dtpFrom.Value.Date != _defaultFrom.Date
            || dtpTo.Value.Date != _defaultTo.Date;
    }

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل).</summary>
    private void _ResetFilterInputs()
    {
        txtSearch.Clear();
        if (cbStatus.Items.Count > 0)
            cbStatus.SelectedIndex = 0;   // «الكل»

        dtpFrom.Value = _defaultFrom;
        dtpTo.Value = _defaultTo;
    }

    /// <summary>
    /// «مسح التصفية» (RULE-N148): يعيد أدوات التصفية إلى افتراضها ويعيد الشبكة إلى وضعها
    /// الافتراضي — **بلا** إعادة تحميل كاملة للشاشة (لا دلائل ولا حفظ التحديد ولا وضع عرض)،
    /// ولكن بنداء قراءة واحد بالمعايير الافتراضية، لأن التصفية على الخدمة لا في الشاشة.
    /// </summary>
    private void _ClearAllFilters()
    {
        _isInitializing = true;
        try
        {
            _ResetFilterInputs();
        }
        finally
        {
            _isInitializing = false;
        }

        _UpdateClearFiltersState();
        _RefreshInvoicesList();
    }

    // ── الأحداث ────────────────────────────────────────────────────────────────

    private void frmPurchaseInvoiceSelection_Load(object? sender, EventArgs e)
    {
        _isInitializing = true;
        try
        {
            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
            cbStatus.SelectedIndex = 0;
            txtSearch.Clear();
        }
        finally
        {
            _isInitializing = false;
        }

        ApplyGreenTheme();
        _UpdateClearFiltersState();
        _UpdateActionButtons();
        _RefreshInvoicesList();

        // المؤشر يبدأ في البحث (TASK-016 §9 — ثم ↓ إلى القائمة ثم Enter اختيار).
        ActiveControl = txtSearch;
        txtSearch.Focus();
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    /// <summary>
    /// «اختيار» — يرفض الفاتورة الملغاة برسالة عربية (RULE-N167)، ويُعيد الفاتورة **بسطورها**
    /// (يُكمِل بالسطور من <c>GetByIdAsync</c> إن لم تكن محمّلة بعد) ثم يُغلق بـ <c>OK</c>.
    /// </summary>
    private async void btnSelect_Click(object? sender, EventArgs e)
    {
        var invoice = _GetSelectedInvoice();

        if (invoice is null)
        {
            clsDialogs.Warning("اختر فاتورة من القائمة أولاً.");
            return;
        }

        // لا مرتجع من فاتورة ملغاة — الرفض عند الاختيار فقط حتى يرى المستخدم سبب المنع.
        if (invoice.Status == 3)
        {
            clsDialogs.Warning("لا يمكن إنشاء مرتجع من فاتورة ملغاة. اختر فاتورة مُرحَّلة.");
            return;
        }

        if (_detailInvoice is null || _detailInvoiceId != invoice.Id)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var result = await PurchaseApi.GetByIdAsync(invoice.Id);

                if (!result.IsSuccess || result.Value is null)
                {
                    clsDialogs.Error(result.Error ?? "تعذر تحميل بيانات الفاتورة المختارة. يرجى إعادة المحاولة.");
                    return;
                }

                _detailInvoice = result.Value;
                _detailInvoiceId = result.Value.Id;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "فشل تحميل فاتورة الشراء المختارة {InvoiceId} قبل إرجاعها", invoice.Id);
                clsDialogs.Error("تعذر تحميل بيانات الفاتورة المختارة. يرجى إعادة المحاولة.");
                return;
            }
            finally
            {
                if (!IsDisposed)
                    Cursor = Cursors.Default;
            }
        }

        SelectedInvoice = _detailInvoice;
        DialogResult = DialogResult.OK;
        Close();
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية
    /// (RULE-N148): أدوات التصفية ← الفاتورة المحددة ← القائمة ← السطور.
    /// وليس بديلاً عن «مسح التصفية».
    /// </summary>
    private void btnRefresh_Click(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _isInitializing = true;
        try
        {
            _ResetFilterInputs();
        }
        finally
        {
            _isInitializing = false;
        }

        _selectedInvoiceId = null;
        _ClearDetails();
        _UpdateClearFiltersState();
        _RefreshInvoicesList();
    }

    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtSearch.Focus();
    }

    // ── أحداث التصفية ──────────────────────────────────────────────────────────

    private void txtSearch_TextChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _UpdateClearFiltersState();
        _RefreshInvoicesList();
    }

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _RefreshInvoicesList();
    }

    private void dtpFrom_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _RefreshInvoicesList();
    }

    private void dtpTo_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _RefreshInvoicesList();
    }

    // ── أحداث الشبكة ───────────────────────────────────────────────────────────

    private void dgvInvoices_SelectionChanged(object? sender, EventArgs e)
    {
        var invoice = _GetSelectedInvoice();
        _selectedInvoiceId = invoice?.Id;

        _UpdateActionButtons();

        // أثناء التعبئة لا نطلب التفاصيل — تُطلب مرة واحدة في نهاية _PopulateInvoicesGrid.
        if (_populating)
            return;

        _UpdateClearFiltersState();
        _LoadInvoiceDetailsAsync(invoice?.Id);
    }

    private void dgvInvoices_DoubleClick(object? sender, EventArgs e)
    {
        if (btnSelect.Enabled)
            btnSelect.PerformClick();
    }

    // ── قائمة السياق (RULE-N23) ────────────────────────────────────────────────

    private void cmsRows_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (_GetSelectedInvoice() is null)
        {
            e.Cancel = true;
            return;
        }

        cmsSelect.Enabled = btnSelect.Enabled;
    }

    private void cmsSelect_Click(object? sender, EventArgs e)
    {
        if (btnSelect.Enabled)
            btnSelect.PerformClick();
    }

    private void cmsRefresh_Click(object? sender, EventArgs e) => btnRefresh_Click(sender, e);

    // ── الإغلاق ─────────────────────────────────────────────────────────────────

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _listCts?.Cancel();
        _listCts?.Dispose();
        _listCts = null;

        _detailCts?.Cancel();
        _detailCts?.Dispose();
        _detailCts = null;

        base.OnFormClosed(e);
    }
}
