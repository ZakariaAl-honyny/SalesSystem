using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using Serilog;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// منتقي فاتورة البيع (TASK-016) — شاشة اختيار للقراءة تُفتح نمطياً وتُرجع الفاتورة
/// <b>ببياناتها الكاملة</b> (الرأس + السطور) في <see cref="SelectedInvoice"/> مع
/// <see cref="DialogResult.OK"/>، ليَبني منها المستدعي مرتجع بيع (RULE-N09 · RULE-N101).
/// <para>
/// الهيكل منقول حرفياً من <c>frmListSales</c> (§2.34): العنوان · الأوامر · «بحث وتصفية» ·
/// قائمة الفواتير + رأس الفاتورة المحددة + سطورها · الملخّص · الأسفل.
/// </para>
/// <para>
/// <b>لا مسار بيانات جديد</b>: العقد القائم <see cref="ISalesInvoiceDesktopService"/> وحده
/// (<c>GetAllAsync</c> للقائمة · <c>GetByIdAsync</c> للفاتورة الكاملة) — ولا حساب مبالغ هنا:
/// كل الأرقام تُعرض من الـ DTO عبر <see cref="clsFormat"/> (RULE-N111).
/// </para>
/// </summary>
public partial class frmSalesInvoiceSelection : Form
{
    /// <summary>عنصر قائمة الحالة: قيمة رقمية (byte?) + نص عربي — لا ربط بالـ SelectedIndex.</summary>
    private sealed record StatusOption(byte? Value, string Text);

    private const string NoSelectionText = "لم تُحدَّد فاتورة";

    private readonly IServiceProvider? _serviceProvider;

    private ISalesInvoiceDesktopService Service
        => _serviceProvider!.GetRequiredService<ISalesInvoiceDesktopService>();

    /// <summary>الفاتورة المختارة — العقد الذي تقرأه شاشة المرتجع (لا يُغيَّر إلا من «اختيار»).</summary>
    public SalesInvoiceDto? SelectedInvoice { get; private set; }

    /// <summary>الفواتير المعروضة الآن بترتيب الخدمة (يُعرض كما وصل — لا إعادة ترتيب في الشاشة).</summary>
    private List<SalesInvoiceDto> _invoices = new();

    /// <summary>الفاتورة الكاملة (بالسطور) المحمَّلة للسطر المحدد عبر <c>GetByIdAsync</c>.</summary>
    private SalesInvoiceDto? _selectedInvoice;

    private int? _selectedInvoiceId;

    /// <summary>تسلسل طلب التفاصيل — يُبطل نتيجة طلب قديم تغيّر التحديد بعده.</summary>
    private int _detailsRequestId;

    private CancellationTokenSource? _loadCts;

    /// <summary>يمنع تحميلاً من داخل المنشئ قبل اكتمال تهيئة أدوات التصفية.</summary>
    private bool _isInitializing = true;

    /// <summary>يمنع رشقات <c>SelectionChanged</c> أثناء إعادة تعبئة الشبكة.</summary>
    private bool _isPopulating;

    // ── التهيئة ────────────────────────────────────────────────────────────────

    /// <summary>مُنشئ التصميم — الشاشة تُنشأ من الحاوية فقط، لا يدوياً (RULE-N02).</summary>
    private frmSalesInvoiceSelection()
    {
        InitializeComponent();
        _LoadStatusOptions();
    }

    public frmSalesInvoiceSelection(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // RULE-N40…RULE-N46 — تنقل وتحديد وتظليل الحقل النشط والأسهم.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor));   // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                    // RULE-N45
        KeyboardHelper.WireSearchToGrid(txtSearch, dgvInvoices);                     // TASK-015: ↓ من البحث إلى القائمة

        // RULE-N43: «اختيار» معطّل بلا سطر محدد، و«مسح التصفية» معطّل ما لم توجد تصفية نشطة
        // (RULE-N148) — رماديان باهتان عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSelect, btnClearFilters);

        // RULE-N47: F5 تحديث. (F9 ملك FilterPanelHelper — لا يُوصَّل مرتين، RULE-N169.)
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F5, () => _LoadInvoices()));

        _UpdateButtonsState();

        // RULE-N63/RULE-N145: يتبنّى btnToggleFilters المُعلَن في المصمّم ويوصِّل طيّ التصفية + F9
        // وحفظ الحالة. آخر سطر في المُنشئ — الأداة null-safe (بلا حاوية يُلغى الحفظ فقط).
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    /// <summary>
    /// قائمة الحالة: «الكل» (null) ثم قيم الحالة الحقيقية 1/2/3 من البيت الواحد
    /// <see cref="DocumentTextHelper.StatusOptions"/> — لا تُكتب نصوص الحالات في الشاشة (RULE-N169).
    /// </summary>
    private void _LoadStatusOptions()
    {
        var options = new List<StatusOption>
        {
            new(null, "الكل")   // عنصر محايد محلي — ليس نص حالة
        };

        foreach (var (value, text) in DocumentTextHelper.StatusOptions)
            options.Add(new(value, text));

        cbStatus.DisplayMember = nameof(StatusOption.Text);
        cbStatus.ValueMember = nameof(StatusOption.Value);
        cbStatus.DataSource = options;
        cbStatus.SelectedIndex = 0;
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — ينتهي بـ <c>Apply(this)</c> دائماً.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);                          // RULE-N53 — شريط العنوان
        GreenThemeHelper.ApplyGrid(dgvInvoices);                        // RULE-N51 — قائمة الفواتير
        GreenThemeHelper.ApplyGrid(dgvLines);                           // RULE-N51 — سطور الفاتورة
        lblRecordsCount.ForeColor = GreenThemeHelper.TextStrong;        // RULE-N53 — شريط الملخّص

        // RULE-N22/RULE-N52: سلوك الشبكتين تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        foreach (var grid in new[] { dgvInvoices, dgvLines })
        {
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    private void frmSalesInvoiceSelection_Load(object sender, EventArgs e)
    {
        _isInitializing = false;

        ApplyGreenTheme();
        _UpdateButtonsState();
        _LoadInvoices();

        // RULE-N59/TASK-015: المؤشر يبدأ في البحث، و↓ ينقل إلى القائمة ثم Enter يختار.
        ActiveControl = txtSearch;
        txtSearch.Focus();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _loadCts?.Cancel();
        _loadCts?.Dispose();
        _loadCts = null;
        base.OnFormClosed(e);
    }

    // ── تحميل القائمة (GetAllAsync) ─────────────────────────────────────────────

    /// <summary>
    /// يقرأ القائمة من الخدمة بمعايير التصفية الحالية (تصفية على الخدمة لا على العميل —
    /// مهمة مالية). النطاق الزمني يُرسَل بقيمة <c>.Date</c> و«إلى» شامل حسب العقد.
    /// </summary>
    private async void _LoadInvoices()
    {
        if (_serviceProvider is null || _isInitializing)
            return;

        DateTime? from = dtpFrom.Checked ? dtpFrom.Value.Date : null;
        DateTime? to = dtpTo.Checked ? dtpTo.Value.Date : null;

        if (from.HasValue && to.HasValue && from.Value > to.Value)
        {
            clsDialogs.Warning("تاريخ البداية «من» يجب أن يكون قبل تاريخ النهاية «إلى» أو مساوياً له.");
            return;
        }

        _loadCts?.Cancel();
        _loadCts?.Dispose();
        _loadCts = new CancellationTokenSource();
        CancellationToken ct = _loadCts.Token;

        Cursor = Cursors.WaitCursor;

        try
        {
            byte? status = cbStatus.SelectedValue as byte?;

            var result = await Service.GetAllAsync(txtSearch.Text.Trim(), from, to, status, ct);

            if (ct.IsCancellationRequested || IsDisposed)
                return;

            if (!result.IsSuccess || result.Value is null)
            {
                // فشل القراءة لا يمسح القائمة المعروضة — رسالة عربية فقط.
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة فواتير البيع. يرجى إعادة المحاولة.");
                return;
            }

            _invoices = result.Value.ToList();

            _PopulateInvoices();
        }
        catch (OperationCanceledException)
        {
            // طلب أُلغي لأن أحدث منه انطلق — لا رسالة ولا تحميل.
        }
        catch (Exception ex)
        {
            Log.Error(ex, "فشل تحميل قائمة فواتير البيع في شاشة اختيار الفاتورة");

            if (!IsDisposed)
                clsDialogs.Error("تعذر تحميل قائمة فواتير البيع. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed && _loadCts is not null && _loadCts.Token == ct)
            {
                Cursor = Cursors.Default;
                _UpdateButtonsState();
            }
        }
    }

    /// <summary>يعيد تعبئة شبكة الفواتير ويستعيد التحديد بالمعرّف ثم يحمّل تفاصيله.</summary>
    private void _PopulateInvoices()
    {
        int? keepId = _selectedInvoiceId;

        _isPopulating = true;
        try
        {
            dgvInvoices.Rows.Clear();

            foreach (var invoice in _invoices)
            {
                int rowIndex = dgvInvoices.Rows.Add(
                    invoice.Id,                                          // colInvId (مخفي)
                    invoice.InvoiceNo,
                    clsFormat.DateToShort(invoice.InvoiceDate),
                    DocumentTextHelper.CashCustomerNameOr(invoice.CustomerName),
                    invoice.PaymentTypeDisplay,
                    clsFormat.Money(invoice.NetTotal),
                    clsFormat.Money(invoice.PaidAmount),
                    clsFormat.Money(invoice.RemainingAmount),
                    invoice.StatusDisplay);

                var row = dgvInvoices.Rows[rowIndex];
                row.Tag = invoice.Id;   // الربط بالمعرّف لا بترتيب السطر

                // تلوين خفيف لكامل السطر حسب الحالة (ألوان البيت الواحد) — ومسح صريح لسطر معاد استخدامه.
                row.DefaultCellStyle.BackColor = invoice.Status switch
                {
                    3 => GreenThemeHelper.StatusDangerBack,     // ملغاة — لا مرتجع منها
                    1 => GreenThemeHelper.StatusWarningBack,    // مسودة
                    _ => Color.Empty
                };
            }

            lblNoData.Visible = dgvInvoices.Rows.Count == 0;

            if (dgvInvoices.Rows.Count > 0)
                _SelectRowByInvoiceId(keepId);
        }
        finally
        {
            _isPopulating = false;
        }

        _UpdateRecordsCount();
        _UpdateButtonsState();

        if (dgvInvoices.CurrentRow?.Tag is int currentId)
        {
            _selectedInvoiceId = currentId;

            // نفس الفاتورة ما زالت محددة وتفاصيلها محمَّلة ⇒ لا نداء قراءة ثانٍ بلا داعٍ.
            if (_selectedInvoice is null || _selectedInvoice.Id != currentId)
                _StartDetailsLoad(currentId);
        }
        else
        {
            _selectedInvoiceId = null;
            _ClearDetails();
        }
    }

    /// <summary>يحدد السطر بنفس الفاتورة إن بقيت في القائمة، وإلا السطر الأول.</summary>
    private void _SelectRowByInvoiceId(int? invoiceId)
    {
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

    private void _UpdateRecordsCount()
        => lblRecordsCount.Text = $"النتائج: {clsFormat.Number(dgvInvoices.Rows.Count)}";

    // ── تفاصيل الفاتورة (GetByIdAsync) ─────────────────────────────────────────

    private void dgvInvoices_SelectionChanged(object? sender, EventArgs e)
    {
        if (_isInitializing || _isPopulating)
            return;

        _selectedInvoiceId = dgvInvoices.CurrentRow?.Tag as int?;
        _UpdateButtonsState();

        if (_selectedInvoiceId is int invoiceId)
            _StartDetailsLoad(invoiceId);
    }

    /// <summary>
    /// يحمّل الفاتورة الكاملة (بالسطور) من <c>GetByIdAsync</c> ويعرض رأسها وسطورها.
    /// فشل القراءة يُبقي القائمة كما هي ويُظهر رسالة عربية (لا نجاح كاذب).
    /// </summary>
    private async void _StartDetailsLoad(int invoiceId)
    {
        if (_serviceProvider is null)
            return;

        int requestId = ++_detailsRequestId;
        Cursor = Cursors.WaitCursor;

        try
        {
            var result = await Service.GetByIdAsync(invoiceId);

            // تغيّر التحديد أثناء الانتظار ⇒ نتيجة هذا الطلب قديمة ولا تُعرض.
            if (IsDisposed || requestId != _detailsRequestId)
                return;

            if (!result.IsSuccess || result.Value is null)
            {
                _ClearDetails();
                clsDialogs.Error(result.Error ?? "تعذر تحميل تفاصيل الفاتورة المحددة. يرجى إعادة المحاولة.");
                return;
            }

            _selectedInvoice = result.Value;
            _selectedInvoiceId = _selectedInvoice.Id;

            _PopulateLines(_selectedInvoice);
            _ShowInvoiceHeader(_selectedInvoice);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "فشل تحميل تفاصيل فاتورة البيع {InvoiceId} في شاشة اختيار الفاتورة", invoiceId);

            if (!IsDisposed)
            {
                _ClearDetails();
                clsDialogs.Error("تعذر تحميل تفاصيل الفاتورة المحددة. يرجى إعادة المحاولة.");
            }
        }
        finally
        {
            if (!IsDisposed)
            {
                Cursor = Cursors.Default;
                _UpdateButtonsState();
            }
        }
    }

    /// <summary>سطور الفاتورة — القيم تُعرض من الـ DTO كما هي (لا حساب في الشاشة).</summary>
    private void _PopulateLines(SalesInvoiceDto invoice)
    {
        dgvLines.Rows.Clear();

        foreach (var line in invoice.Items)
        {
            dgvLines.Rows.Add(
                line.ProductName,
                clsFormat.Quantity(line.Quantity),
                clsFormat.Money(line.UnitPrice),
                clsFormat.Money(line.DiscountAmount),
                clsFormat.Money(line.LineTotal));
        }
    }

    /// <summary>رأس الفاتورة المحددة: رقم · تاريخ · عميل · إجمالي · مدفوع · متبقي · حالة.</summary>
    private void _ShowInvoiceHeader(SalesInvoiceDto invoice)
        => lblInvoiceSummary.Text =
            $"فاتورة رقم {invoice.InvoiceNo} · {clsFormat.DateToShort(invoice.InvoiceDate)}"
            + $" · {DocumentTextHelper.CashCustomerNameOr(invoice.CustomerName)}"
            + $" · الإجمالي {clsFormat.Money(invoice.NetTotal)}"
            + $" · المدفوع {clsFormat.Money(invoice.PaidAmount)}"
            + $" · المتبقي {clsFormat.Money(invoice.RemainingAmount)}"
            + $" · {invoice.StatusDisplay}";

    private void _ClearDetails()
    {
        _selectedInvoice = null;
        dgvLines.Rows.Clear();
        lblInvoiceSummary.Text = NoSelectionText;
    }

    // ── آلة الحالة والتصفية ─────────────────────────────────────────────────────

    /// <summary>RULE-N43/RULE-N148: «اختيار» يتبع وجود سطر، و«مسح التصفية» يتبع وجود تصفية نشطة.</summary>
    private void _UpdateButtonsState()
    {
        btnSelect.Enabled = dgvInvoices.Rows.Count > 0 && dgvInvoices.CurrentRow is not null;
        btnClearFilters.Enabled = _IsFilterActive();
    }

    private bool _IsFilterActive()
        => !string.IsNullOrWhiteSpace(txtSearch.Text)
           || cbStatus.SelectedIndex > 0
           || dtpFrom.Checked
           || dtpTo.Checked;

    /// <summary>
    /// «مسح التصفية» يمسح **أدوات التصفية فقط** ثم يعيد الاستعلام (RULE-N148):
    /// لا يلمس الفاتورة المختارة ولا يغيّر البيانات المحمَّلة يدوياً.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        if (!_IsFilterActive())
            return;

        _isInitializing = true;
        try
        {
            txtSearch.Clear();
            if (cbStatus.Items.Count > 0)
                cbStatus.SelectedIndex = 0;   // «الكل»
            dtpFrom.Checked = false;
            dtpTo.Checked = false;
        }
        finally
        {
            _isInitializing = false;
        }

        _UpdateButtonsState();
        _LoadInvoices();
        txtSearch.Focus();
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _UpdateButtonsState();
        _LoadInvoices();
    }

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _UpdateButtonsState();
        _LoadInvoices();
    }

    private void dtpFrom_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _UpdateButtonsState();
        _LoadInvoices();
    }

    private void dtpTo_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _UpdateButtonsState();
        _LoadInvoices();
    }

    // ── الاختيار ───────────────────────────────────────────────────────────────

    private void btnRefresh_Click(object? sender, EventArgs e) => _LoadInvoices();

    /// <summary>حدّ الخطأ الوحيد لمسار الاختيار (RULE-N205) — لا <c>catch</c> ثانٍ داخل المنطق.</summary>
    private async void btnSelect_Click(object? sender, EventArgs e)
    {
        try
        {
            await _SelectCurrentAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "فشل اختيار فاتورة البيع في شاشة اختيار الفاتورة");

            if (!IsDisposed)
                clsDialogs.Error("تعذر اختيار الفاتورة. يرجى إعادة المحاولة.");
        }
    }

    private void dgvInvoices_DoubleClick(object? sender, EventArgs e)
    {
        if (btnSelect.Enabled)
            btnSelect.PerformClick();
    }

    /// <summary>
    /// يُرجع الفاتورة المحددة **بسطورها** إلى المستدعي. الفاتورة الملغاة تُرفض برسالة
    /// عربية صريحة ولا تُغلق الشاشة — لا مرتجع من فاتورة ملغاة (RULE-N167 · RULE-019).
    /// </summary>
    private async Task _SelectCurrentAsync()
    {
        if (_serviceProvider is null || _isInitializing)
            return;

        if (dgvInvoices.CurrentRow?.Tag is not int invoiceId)
        {
            _UpdateButtonsState();
            return;
        }

        // الفاتورة الكاملة: المحمَّلة إن كانت لنفس السطر، وإلا نجلبها من العقد (GetByIdAsync).
        SalesInvoiceDto? invoice = _selectedInvoice is not null && _selectedInvoice.Id == invoiceId
            ? _selectedInvoice
            : null;

        if (invoice is null)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                var result = await Service.GetByIdAsync(invoiceId);

                if (IsDisposed)
                    return;

                if (!result.IsSuccess || result.Value is null)
                {
                    clsDialogs.Error(result.Error ?? "تعذر تحميل بيانات الفاتورة المحددة. يرجى إعادة المحاولة.");
                    return;
                }

                invoice = result.Value;
            }
            finally
            {
                if (!IsDisposed)
                    Cursor = Cursors.Default;
            }
        }

        if (invoice.Status == 3)
        {
            clsDialogs.Warning($"لا يمكن إنشاء مرتجع من فاتورة ملغاة (فاتورة رقم {invoice.InvoiceNo}). اختر فاتورة مُرحَّلة أو مسودة.");
            return;
        }

        SelectedInvoice = invoice;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    // ── Context Menu (RULE-N23) ────────────────────────────────────────────────

    /// <summary>«اختيار» من قائمة السياق تستدعي نفس زر الشاشة (بلا منطق مكرر).</summary>
    private void cmsSelect_Click(object? sender, EventArgs e)
    {
        if (btnSelect.Enabled)
            btnSelect.PerformClick();
    }

    /// <summary>«تحديث» من قائمة السياق = نفس معالج F5.</summary>
    private void cmsRefresh_Click(object? sender, EventArgs e) => _LoadInvoices();

    /// <summary>تُلغى القائمة عند عدم وجود سطر محدد في الشبكة (RULE-N23).</summary>
    private void cmsInvoices_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (dgvInvoices.CurrentRow is null)
        {
            e.Cancel = true;
            return;
        }

        cmsSelect.Enabled = btnSelect.Enabled;
    }
}
