using SalesSystem.DTO;
using SalesSystem.Desktop.GlobalClasses;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Returns.Forms;

/// <summary>
/// قائمة المرتجعات — توأم <c>frmListSales</c> في الشكل والسلوك، مع فرقٍ واحد مقصود:
/// الشاشة تعرض **نوعَي مستند** (مرتجع مبيعات + مرتجع مشتريات) في تبويبين، لذلك
/// شبكتان وملخّصان يتبعان تصفيةً واحدة مشتركة داخل <c>gbFilter</c> (§2.22).
/// <para>
/// القراءة فقط: لا حفظ ولا ترحيل من هذه الشاشة — المحرّران
/// (<see cref="frmSalesReturn"/> / <see cref="frmPurchaseReturn"/>) يملكان الكتابة،
/// ويُفتحان من DI فقط ثم تُعاد قراءة الشبكة عند إغلاقهما (<c>RULE-N02</c>/<c>RULE-N132</c>).
/// </para>
/// </summary>
public partial class frmListReturns : Form
{
    /// <summary>عنصر قائمة الحالة: قيمة رقمية (byte?) + نص عربي — لا ربط بالـ SelectedIndex.</summary>
    /// <remarks>نفس نمط <c>frmListSales</c> (<c>RULE-N60</c>: التصفية بالقيمة لا بالنص العربي).</remarks>
    private sealed record StatusOption(byte? Value, string Text);

    private static Color FocusColor => ThemePalette.DefaultFor(ThemePalette.SelectionRowColor);

    private readonly IServiceProvider _serviceProvider;

    private ISalesReturnDesktopService SalesReturnService
        => _serviceProvider.GetRequiredService<ISalesReturnDesktopService>();

    private IPurchaseReturnDesktopService PurchaseReturnService
        => _serviceProvider.GetRequiredService<IPurchaseReturnDesktopService>();

    // ── مخازن البيانات ─────────────────────────────────────────────────────────
    // الربط بالمعرّف لا بترتيب السطر (Row.Tag = Id) — كما في frmListSales.
    private readonly Dictionary<int, SalesReturnDto> _salesReturnsById = new();
    private readonly Dictionary<int, PurchaseReturnDto> _purchaseReturnsById = new();

    private List<SalesReturnDto> _allSalesReturns = new();
    private List<SalesReturnDto> _filteredSalesReturns = new();
    private List<PurchaseReturnDto> _allPurchaseReturns = new();
    private List<PurchaseReturnDto> _filteredPurchaseReturns = new();

    private CancellationTokenSource? _refreshCts;

    private int? _selectedSalesReturnId;
    private int? _selectedPurchaseReturnId;

    // النطاق الزمني الافتراضي («هذا الشهر») — يُحفظ مرة واحدة ويُعاد إليه عند
    // «مسح التصفية» و«تحديث» (RULE-N148).
    private readonly DateTime _defaultFrom;
    private readonly DateTime _defaultTo;

    // يبدأ true حتى لا ينطلق أي تحميل من داخل المنشئ قبل اكتمال تهيئة الفلاتر.
    private bool _isInitializing = true;
    private bool _toolbarStyled;

    public frmListReturns(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var today = DateTime.Today;
        _defaultFrom = new DateTime(today.Year, today.Month, 1);
        _defaultTo = today;

        InitializeComponent();

        _InitializeKeyboard();
        _ApplyUiTheme();
        _LoadStatusOptions();
        _WireShortcuts();

        // RULE-N147: Enter = الزر الأساسي للشاشة («مرتجع جديد») — والزر الافتراضي للنموذج.
        KeyboardHelper.WireModeEnter(this, () => btnAdd);
        AcceptButton = btnAdd;

        FilterPanelHelper.Attach(this, _serviceProvider);
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

    /// <summary>الثيم — ينتهي بـ <c>GreenThemeHelper.Apply(this)</c> (RULE-N50) ويعلن سلوك الشبكتين (RULE-N22).</summary>
    private void _ApplyUiTheme()
    {
        if (!_toolbarStyled)
        {
            _StyleToolbar();
            _toolbarStyled = true;
        }

        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvSales);
        GreenThemeHelper.ApplyGrid(dgvPurchases);

        // خط عمود الحالة يُضبط مرة واحدة على العمود — لا يُنشأ Font داخل CellFormatting.
        if (dgvSales.Columns.Contains(colSalesStatus.Name))
            colSalesStatus.DefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);

        if (dgvPurchases.Columns.Contains(colPurchaseStatus.Name))
            colPurchaseStatus.DefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);

        // RULE-N22/RULE-N52: السلوك تعلنه الشاشة — ApplyGrid لا يلمس هذه الخصائص.
        dgvSales.ReadOnly = true;
        dgvSales.AllowUserToAddRows = false;
        dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSales.MultiSelect = false;

        dgvPurchases.ReadOnly = true;
        dgvPurchases.AllowUserToAddRows = false;
        dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPurchases.MultiSelect = false;

        GreenThemeHelper.Apply(this); // LAST (RULE-N50)
    }

    /// <summary>تنسيق أزرار شريط الأوامر + التلميحات — مرة واحدة فقط (نمط frmListSales).</summary>
    private void _StyleToolbar()
    {
        ThemeHelper.ApplyButtonStyle(btnAdd, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnView, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        foreach (var btn in new[] { btnAdd, btnView, btnRefresh, btnClose })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false; // يُظهر لون الثيم بدل ألوان النظام
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btn.Size = new Size(140, 40);
            btn.Margin = new Padding(4, 0, 4, 0);
        }

        toolTip.SetToolTip(btnAdd, "مرتجع جديد — مبيعات أو مشتريات حسب التبويب (F2)");
        toolTip.SetToolTip(btnView, "عرض أو تعديل المرتجع المحدد (F3)");
        toolTip.SetToolTip(btnRefresh, "إعادة تحميل القائمة وتصفير التصفية (F5)");
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة");

        // أزرار النطاقات السريعة + «مسح التصفية» — الصف الثاني داخل صندوق البحث.
        ThemeHelper.ApplyButtonStyle(btnRangeToday, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnRangeMonth, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnRange30, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnClearFilters, ThemeHelper.ButtonType.Neutral);

        foreach (var btn in new[] { btnRangeToday, btnRangeMonth, btnRange30, btnClearFilters })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.MinimumSize = Size.Empty; // حتى لا يُفرض ارتفاع 35 على صف الأزرار الثاني
            btn.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        }

        btnRangeToday.Size = new Size(100, 30);
        btnRangeMonth.Size = new Size(131, 30);
        btnRange30.Size = new Size(128, 30);
        btnClearFilters.Size = new Size(200, 32);

        toolTip.SetToolTip(btnRangeToday, "تصفية مرتجعات اليوم فقط");
        toolTip.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip.SetToolTip(btnRange30, "آخر 30 يوماً");

        // RULE-N43: الأزرار المعطّلة تظهر رمادية باهتة وليس مجرّد Enabled = false.
        KeyboardHelper.WireButtonDisabledVisual(btnView, btnClearFilters);
    }

    /// <summary>قائمة الحالة: «الكل» (null) ثم قيم enum الحقيقية من البيت الواحد (RULE-N169).</summary>
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

    /// <summary>RULE-N47: اختصارات موحّدة F2 جديد · F3 عرض/تعديل · F5 تحديث · Esc.</summary>
    private void _WireShortcuts()
    {
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAdd.PerformClick()),
            (Keys.F3, () => { if (btnView.Enabled) btnView.PerformClick(); }),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, _GlobalEscape));
    }

    /// <summary>
    /// Esc — RULE-N59: المؤشر داخل صندوق التصفية ⇒ مسح كل أدوات التصفية (بلا إعادة تحميل —
    /// RULE-N148)؛ وخارجها ⇒ إغلاق الشاشة (RULE-N47 — Esc خروج).
    /// </summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        Close();
    }

    // ── مسح التصفية (RULE-N148) ────────────────────────────────────────────────

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكتين إلى وضعهما الافتراضي،
    /// بلا إعادة تحميل. «تحديث» (F5) هو المسؤول عن إعادة قراءة البيانات.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>تفريغ أدوات التصفية وحدها إلى قيمها الافتراضية (بلا تطبيق ولا تحميل).</summary>
    private void _ClearFilterInputs()
    {
        cbFilterBy.SelectedIndex = 0;          // العميل/المورد
        txtFilterValue.Clear();
        if (cbStatus.Items.Count > 0)
            cbStatus.SelectedIndex = 0;        // «الكل»
        chkOnlyCancelled.Checked = false;
        dtpFrom.Value = _defaultFrom;
        dtpTo.Value = _defaultTo;
    }

    /// <summary>يمسح كل أدوات التصفية ثم يعيد تطبيقها على البيانات المحمّلة — بلا إعادة تحميل.</summary>
    private void _ClearAllFilters()
    {
        _isInitializing = true;
        try
        {
            _ClearFilterInputs();
        }
        finally
        {
            _isInitializing = false;
        }

        _ApplyFilters();
    }

    /// <summary>RULE-N148/RULE-N43: زر «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || (cbStatus.SelectedValue as byte?).HasValue
            || chkOnlyCancelled.Checked
            || dtpFrom.Value.Date != _defaultFrom.Date
            || dtpTo.Value.Date != _defaultTo.Date;
    }

    // ── Data Loading ───────────────────────────────────────────────────────────

    /// <summary>
    /// تحميل المرتجعين معاً. الشاشة تُحمِّل **كل** المرتجعات ثم تُصفّي في الذاكرة،
    /// لأن تصفية واحدة تخدم شبكتين وتحديث فوري بلا رحلة إلى الخدمة عند كل حرف
    /// (<c>RULE-N59</c>)؛ المدى الزمني يبقى أداة تصفية لا معيار استعلام.
    /// </summary>
    private async void _RefreshReturns()
    {
        if (_isInitializing)
            return;

        _refreshCts?.Cancel();
        _refreshCts?.Dispose();
        _refreshCts = new CancellationTokenSource();
        CancellationToken ct = _refreshCts.Token;

        Cursor = Cursors.WaitCursor;

        var loadedSales = new List<SalesReturnDto>();
        var loadedPurchases = new List<PurchaseReturnDto>();

        // فشل تحميل جهة لا يمحو بيانات الجهة الأخرى المحمَّلة سابقاً ⇒ تُستبدل الذاكرة عند النجاح فقط.
        var salesLoaded = false;
        var purchasesLoaded = false;

        try
        {
            var salesResult = await SalesReturnService.GetAllAsync(ct: ct);

            if (ct.IsCancellationRequested)
                return;

            if (salesResult.IsSuccess && salesResult.Value != null)
            {
                loadedSales = salesResult.Value.ToList();
                salesLoaded = true;
            }
            else
            {
                clsDialogs.Error(salesResult.Error ?? "تعذر تحميل مرتجعات المبيعات. يرجى إعادة المحاولة.");
            }

            var purchaseResult = await PurchaseReturnService.GetAllAsync(ct: ct);

            if (ct.IsCancellationRequested)
                return;

            if (purchaseResult.IsSuccess && purchaseResult.Value != null)
            {
                loadedPurchases = purchaseResult.Value.ToList();
                purchasesLoaded = true;
            }
            else
            {
                clsDialogs.Error(purchaseResult.Error ?? "تعذر تحميل مرتجعات المشتريات. يرجى إعادة المحاولة.");
            }
        }
        catch (OperationCanceledException)
        {
            return;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة المرتجعات في شاشة دليل المرتجعات");

            if (!ct.IsCancellationRequested)
            {
                clsDialogs.Error("تعذر تحميل المرتجعات. يرجى إعادة المحاولة.");
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

        if (salesLoaded)
            _allSalesReturns = loadedSales;

        if (purchasesLoaded)
            _allPurchaseReturns = loadedPurchases;

        _ApplyFilters();
    }

    private void _ReloadForm() => _RefreshReturns();

    // ── Filtering & Grids ──────────────────────────────────────────────────────

    /// <summary>
    /// تصفية فورية واحدة تُطبَّق على الشبكتين معاً (RULE-N59/N60): المدى الزمني ثم الحالة
    /// ثم «الملغاة فقط» ثم النص حسب نوع التصفية. لا يوجد زر بحث.
    /// </summary>
    private void _ApplyFilters()
    {
        if (_isInitializing)
            return;

        var fromDate = dtpFrom.Value.Date;
        var toDate = dtpTo.Value.Date;

        // نطاق معكوس ⇒ يُبادَل طرفاه بدل إسقاط النتائج (التصفية في الذاكرة، لا استعلام).
        if (toDate < fromDate)
            (fromDate, toDate) = (toDate, fromDate);

        IEnumerable<SalesReturnDto> sales = _allSalesReturns
            .Where(r => r.ReturnDate.Date >= fromDate && r.ReturnDate.Date <= toDate);

        // PurchaseReturnDto.ReturnDate = DateOnly (يعود DateTime)، فيُؤخذ تاريخه فقط.
        IEnumerable<PurchaseReturnDto> purchases = _allPurchaseReturns
            .Where(r =>
            {
                var date = r.ReturnDate.ToDateTime(TimeOnly.MinValue).Date;
                return date >= fromDate && date <= toDate;
            });

        // (2) الحالة — بالقيمة الرقمية للعنصر المختار لا بنص عربي (RULE-N60).
        byte? status = cbStatus.SelectedValue as byte?;
        if (status.HasValue)
        {
            sales = sales.Where(r => r.Status == status.Value);
            purchases = purchases.Where(r => r.Status == status.Value);
        }

        // (3) «الملغاة فقط» (RULE-N58).
        if (chkOnlyCancelled.Checked)
        {
            sales = sales.Where(r => r.Status == 3);
            purchases = purchases.Where(r => r.Status == 3);
        }

        // (4) النص على أعمدة معروفة فقط.
        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 0: // العميل / المورد
                    sales = sales.Where(r => DocumentTextHelper.CashCustomerNameOr(r.CustomerName)
                        .Contains(text, StringComparison.OrdinalIgnoreCase));
                    purchases = purchases.Where(r => (r.SupplierName ?? string.Empty)
                        .Contains(text, StringComparison.OrdinalIgnoreCase));
                    break;

                case 1: // رقم المرتجع
                    sales = sales.Where(r => r.ReturnNo.ToString().StartsWith(text, StringComparison.Ordinal));
                    purchases = purchases.Where(r => r.ReturnNo.ToString().StartsWith(text, StringComparison.Ordinal));
                    break;

                case 2: // البيان
                    sales = sales.Where(r =>
                        (r.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)
                        || (r.ReturnReason ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase));
                    purchases = purchases.Where(r => (r.Notes ?? string.Empty)
                        .Contains(text, StringComparison.OrdinalIgnoreCase));
                    break;

                case 3: // المبلغ — البحث في النص المعروض نفسه (clsFormat.Money) لا في صيغة مكتوبة هنا
                    sales = sales.Where(r => clsFormat.Money(r.TotalAmount).Contains(text, StringComparison.Ordinal));
                    purchases = purchases.Where(r => clsFormat.Money(r.TotalAmount).Contains(text, StringComparison.Ordinal));
                    break;
            }
        }

        _filteredSalesReturns = sales.ToList();
        _filteredPurchaseReturns = purchases.ToList();

        _PopulateSalesGrid();
        _PopulatePurchaseGrid();
        _UpdateSummary();

        _UpdateClearFiltersState();   // RULE-N148: حالة زر «مسح التصفية» تتبع التصفية النشطة
    }

    private void _PopulateSalesGrid()
    {
        int? keepSelection = _selectedSalesReturnId;

        dgvSales.Rows.Clear();
        _salesReturnsById.Clear();

        foreach (var r in _filteredSalesReturns)
        {
            int rowIndex = dgvSales.Rows.Add(
                r.ReturnNo,
                clsFormat.DateToShort(r.ReturnDate),
                DocumentTextHelper.CashCustomerNameOr(r.CustomerName),
                clsFormat.Money(r.TotalAmount),
                DocumentTextHelper.StatusText(r.Status));   // البيت الواحد لنص الحالة (RULE-N169)

            var row = dgvSales.Rows[rowIndex];
            row.Tag = r.Id;                                  // الربط بالمعرّف لا بترتيب السطر
            _salesReturnsById[r.Id] = r;

            // تلوين خفيف للسطر حسب الحالة — ويُصفَّر صراحةً حتى لا يبقى لون سطر سابق.
            row.DefaultCellStyle.BackColor = r.Status switch
            {
                3 => GreenThemeHelper.StatusDangerBack,      // ملغي
                1 => GreenThemeHelper.StatusWarningBack,     // مسودة
                _ => Color.Empty                             // مُرحَّل/سليم
            };
        }

        lblNoSales.Visible = _filteredSalesReturns.Count == 0;
        if (lblNoSales.Visible)
            lblNoSales.BringToFront();

        _RestoreSelection(dgvSales, keepSelection);
        _UpdateActionButtons();
    }

    private void _PopulatePurchaseGrid()
    {
        int? keepSelection = _selectedPurchaseReturnId;

        dgvPurchases.Rows.Clear();
        _purchaseReturnsById.Clear();

        foreach (var r in _filteredPurchaseReturns)
        {
            int rowIndex = dgvPurchases.Rows.Add(
                r.ReturnNo,
                clsFormat.DateToShort(r.ReturnDate.ToDateTime(TimeOnly.MinValue)),   // ReturnDate = DateOnly
                r.SupplierName,
                clsFormat.Money(r.TotalAmount),
                DocumentTextHelper.StatusText(r.Status));

            var row = dgvPurchases.Rows[rowIndex];
            row.Tag = r.Id;
            _purchaseReturnsById[r.Id] = r;

            row.DefaultCellStyle.BackColor = r.Status switch
            {
                3 => GreenThemeHelper.StatusDangerBack,
                1 => GreenThemeHelper.StatusWarningBack,
                _ => Color.Empty
            };
        }

        lblNoPurchases.Visible = _filteredPurchaseReturns.Count == 0;
        if (lblNoPurchases.Visible)
            lblNoPurchases.BringToFront();

        _RestoreSelection(dgvPurchases, keepSelection);
        _UpdateActionButtons();
    }

    /// <summary>يعيد التحديد على نفس المستند (بالمعرّف) بعد إعادة تعبئة الشبكة — للشبكتين معاً.</summary>
    private static void _RestoreSelection(DataGridView grid, int? id)
    {
        if (grid.Rows.Count == 0)
            return;

        int targetRow = 0;

        if (id.HasValue)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Tag is int rowId && rowId == id.Value)
                {
                    targetRow = row.Index;
                    break;
                }
            }
        }

        grid.ClearSelection();
        var target = grid.Rows[targetRow];
        grid.CurrentCell = target.Cells[0];
        target.Selected = true;
    }

    /// <summary>RULE-N43: «عرض/تعديل» مفعّل فقط عند وجود سطر محدد في التبويب النشط.</summary>
    private void _UpdateActionButtons()
    {
        btnView.Enabled = _IsSalesTabActive()
            ? _GetSelectedSalesReturn() != null
            : _GetSelectedPurchaseReturn() != null;
    }

    /// <summary>RULE-N61 — عدد المعروض + «المعروض من الإجمالي» + إجمالي كل نوع (<c>clsFormat.Money</c>).</summary>
    private void _UpdateSummary()
    {
        int shown = _filteredSalesReturns.Count + _filteredPurchaseReturns.Count;
        int total = _allSalesReturns.Count + _allPurchaseReturns.Count;

        lblRecordsCount.Text = clsFormat.Number(shown);
        lblSalesTotal.Text = clsFormat.Money(_filteredSalesReturns.Sum(r => r.TotalAmount));
        lblPurchaseTotal.Text = clsFormat.Money(_filteredPurchaseReturns.Sum(r => r.TotalAmount));
        lblSummary.Text = $"المعروض: {clsFormat.Number(shown)} من {clsFormat.Number(total)}";
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private bool _IsSalesTabActive() => tabReturns.SelectedIndex == 0;

    /// <summary>يحلّ المرتجع المحدد في تبويب المبيعات عبر معرّفه المحفوظ في Row.Tag.</summary>
    private SalesReturnDto? _GetSelectedSalesReturn()
    {
        if (dgvSales.CurrentRow?.Tag is not int id)
            return null;

        return _salesReturnsById.TryGetValue(id, out var dto) ? dto : null;
    }

    /// <summary>يحلّ المرتجع المحدد في تبويب المشتريات عبر معرّفه المحفوظ في Row.Tag.</summary>
    private PurchaseReturnDto? _GetSelectedPurchaseReturn()
    {
        if (dgvPurchases.CurrentRow?.Tag is not int id)
            return null;

        return _purchaseReturnsById.TryGetValue(id, out var dto) ? dto : null;
    }

    /// <summary>لون نص الحالة — بالحالة الرقمية لا بنص عربي (RULE-N019 / RULE-N171).</summary>
    private static Color? _StatusForeColor(byte? status) => status switch
    {
        1 => GreenThemeHelper.StatusInfo,      // مسودة
        2 => GreenThemeHelper.StatusSuccess,   // مرحل
        3 => GreenThemeHelper.StatusDanger,    // ملغي
        _ => null
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

        _ApplyFilters();
    }

    /// <summary>
    /// فتح المحرر — إعادة القراءة عند **إغلاقه** لا فور فتحه (RULE-N151/RULE-N132):
    /// القراءة الفورية كانت تسبق الحفظ فتبقى القائمة قديمة بعد «تم الحفظ».
    /// </summary>
    private void _OpenEditor(Form form)
    {
        form.FormClosed -= _OnEditorClosed;
        form.FormClosed += _OnEditorClosed;

        clsGlobal._OpenForm(form);

        // نسخة مفتوحة أُعيد تنشيطها بدل فتح هذه ⇒ لا إغلاق قادم لها، فتُحدَّث القائمة الآن.
        if (!form.Visible)
            _ReloadForm();
    }

    private void _OnEditorClosed(object? sender, FormClosedEventArgs e)
    {
        if (sender is Form form)
            form.FormClosed -= _OnEditorClosed;

        if (!IsDisposed)
            _ReloadForm();
    }

    /// <summary>فتح محرر المرتجع المحدد (أو فارغاً عند «جديد») حسب التبويب النشط.</summary>
    private void _OpenSalesEditor(SalesReturnDto? dto)
    {
        var editor = _serviceProvider.GetRequiredService<frmSalesReturn>();
        editor.LoadData(dto);
        _OpenEditor(editor);
    }

    private void _OpenPurchaseEditor(PurchaseReturnDto? dto)
    {
        var editor = _serviceProvider.GetRequiredService<frmPurchaseReturn>();
        editor.LoadData(dto);
        _OpenEditor(editor);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _refreshCts?.Cancel();
        _refreshCts?.Dispose();
        _refreshCts = null;
        base.OnFormClosed(e);
    }

    // ── Form Events ────────────────────────────────────────────────────────────

    private void frmListReturns_Load(object? sender, EventArgs e)
    {
        _isInitializing = true;
        try
        {
            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
            cbStatus.SelectedIndex = 0;
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Clear();
        }
        finally
        {
            _isInitializing = false;
        }

        _ApplyUiTheme();
        _UpdateClearFiltersState();   // RULE-N148: الشاشة تُفتح على التصفية الافتراضية ⇒ الزر معطّل
        _RefreshReturns();
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية
    /// (RULE-N148): أدوات التصفية ← دليل الحالة ← المرتجعات ← الشبكتان والملخص والتحديد.
    /// وليس بديلاً عن «مسح التصفية» الذي لا يُعيد التحميل.
    /// </summary>
    private void btnRefresh_Click(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _isInitializing = true;
        try
        {
            _ClearFilterInputs();   // التصفية تعود للافتراضي
            _LoadStatusOptions();   // إعادة بناء دليل الحالة (قد تتغيّر إعداداته)
        }
        finally
        {
            _isInitializing = false;
        }

        _selectedSalesReturnId = null;      // التحديد يعود إلى الافتراضي
        _selectedPurchaseReturnId = null;

        _RefreshReturns();                  // إعادة قراءة المرتجعات (تطبّق التصفية والملخص)
    }

    // ── Filter Events (تصفية فورية — RULE-N59) ────────────────────────────────

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        // المعيار الأول هو تصفية حقيقية (العميل/المورد) لا «بلا تصفية»، فالحقل ظاهر دائماً.
        txtFilterValue.Clear();
        txtFilterValue.Focus();
        _ApplyFilters();
    }

    private void txtFilterValue_TextChanged(object? sender, EventArgs e) => _ApplyFilters();

    private void txtFilterValue_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            return;
        }

        bool isNumericFilter = cbFilterBy.SelectedIndex == 1;   // رقم المرتجع

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

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilters();
    }

    private void dtpFrom_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilters();
    }

    private void dtpTo_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilters();
    }

    private void chkOnlyCancelled_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilters();
    }

    private void btnRangeToday_Click(object? sender, EventArgs e)
        => _SetDateRange(DateTime.Today, DateTime.Today);

    private void btnRangeMonth_Click(object? sender, EventArgs e)
    {
        var today = DateTime.Today;
        _SetDateRange(new DateTime(today.Year, today.Month, 1), today);
    }

    private void btnRange30_Click(object? sender, EventArgs e)
        => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

    // ── Grid Events ────────────────────────────────────────────────────────────

    private void tabReturns_SelectedIndexChanged(object? sender, EventArgs e) => _UpdateActionButtons();

    private void dgvSales_SelectionChanged(object? sender, EventArgs e)
    {
        _selectedSalesReturnId = _GetSelectedSalesReturn()?.Id;
        _UpdateActionButtons();
    }

    private void dgvPurchases_SelectionChanged(object? sender, EventArgs e)
    {
        _selectedPurchaseReturnId = _GetSelectedPurchaseReturn()?.Id;
        _UpdateActionButtons();
    }

    private void dgvSales_DoubleClick(object? sender, EventArgs e)
    {
        if (_GetSelectedSalesReturn() != null)
            btnView.PerformClick();
    }

    private void dgvPurchases_DoubleClick(object? sender, EventArgs e)
    {
        if (_GetSelectedPurchaseReturn() != null)
            btnView.PerformClick();
    }

    private void dgvSales_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        var statusColumn = dgvSales.Columns["colSalesStatus"];
        if (statusColumn is null || e.ColumnIndex != statusColumn.Index || e.RowIndex < 0)
            return;

        // الحالة تُقرأ من الـDTO عبر المعرّف المحفوظ في Row.Tag — لا بقيمة الخلية النصية (RULE-N179).
        if (dgvSales.Rows[e.RowIndex].Tag is not int id || !_salesReturnsById.TryGetValue(id, out var dto))
            return;

        if (_StatusForeColor(dto.Status) is Color color)
            e.CellStyle.ForeColor = color;
    }

    private void dgvPurchases_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        var statusColumn = dgvPurchases.Columns["colPurchaseStatus"];
        if (statusColumn is null || e.ColumnIndex != statusColumn.Index || e.RowIndex < 0)
            return;

        if (dgvPurchases.Rows[e.RowIndex].Tag is not int id || !_purchaseReturnsById.TryGetValue(id, out var dto))
            return;

        if (_StatusForeColor(dto.Status) is Color color)
            e.CellStyle.ForeColor = color;
    }

    // ── Context Menu (RULE-N23) ────────────────────────────────────────────────

    private void ctxMenu_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
        => _UpdateActionButtons();

    private void tsmiOpen_Click(object? sender, EventArgs e) => btnView.PerformClick();

    private void tsmiRefresh_Click(object? sender, EventArgs e) => btnRefresh.PerformClick();

    // ── Toolbar Events ─────────────────────────────────────────────────────────

    private void btnAdd_Click(object? sender, EventArgs e)
    {
        if (_IsSalesTabActive())
            _OpenSalesEditor(null);
        else
            _OpenPurchaseEditor(null);
    }

    private void btnView_Click(object? sender, EventArgs e)
    {
        if (_IsSalesTabActive())
        {
            var dto = _GetSelectedSalesReturn();
            if (dto is null)
                return;

            _OpenSalesEditor(dto);
            return;
        }

        var purchase = _GetSelectedPurchaseReturn();
        if (purchase is null)
            return;

        _OpenPurchaseEditor(purchase);
    }
}
