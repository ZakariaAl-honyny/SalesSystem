using System.ComponentModel;
using System.Data;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common.Enums;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Models;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using Serilog;

namespace SalesSystem.Desktop.Screens.Returns.Forms;

/// <summary>
/// شاشة مرتجع المشتريات المدمجة (<c>frm*Master</c>) — تحلّ محل <c>frmPurchaseReturn</c> + قائمة
/// المرتجعات المدمجة: محرر مستند (رأس + سطور) + تصفية + ملخص + شبكة في نافذة واحدة،
/// بنمط <c>frmProductMaster</c> المُشهَد (<c>RULE-N184</c>).
/// <para>
/// <b>لا وضع تعديل (<c>D-05</c>):</b> العقد <c>IPurchaseReturnDesktopService</c> يعرّض
/// <c>Create/Post/Cancel</c> فقط — لا <c>UpdateAsync</c>. فاختيار مرتجع قائم ⇒
/// <see cref="EntryMode.Editing"/> = عرض للقراءة فقط + «ترحيل» (إن كان مسودة) + «إلغاء المستند»،
/// و<see cref="_PrimaryActionButton"/> تُعيد <c>null</c> فلا زر حفظ يوهم بالتعديل.
/// </para>
/// </summary>
public partial class frmPurchaseReturnMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IPurchaseReturnDesktopService ReturnService => _serviceProvider.GetRequiredService<IPurchaseReturnDesktopService>();
    private ISupplierDesktopService SupplierService => _serviceProvider.GetRequiredService<ISupplierDesktopService>();
    private IProductDesktopService ProductService => _serviceProvider.GetRequiredService<IProductDesktopService>();
    private IWarehouseDesktopService WarehouseService => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── ذاكرة الشاشة ────────────────────────────────────────────────────────
    private readonly BindingList<InvoiceLineItem> _lines = new();
    private List<ProductDto> _allProducts = new();

    private DataTable? _returnsTable;
    private DataView? _returnsView;

    /// <summary>المستند المعروض في المحرر — <c>null</c> = لا مستند (وضع العرض أو إضافة جديدة).</summary>
    private PurchaseReturnDto? _invoice;

    /// <summary>
    /// معرّف المرتجع الذي أنشأته الشاشة فعلاً. يمنع إنشاء مستند ثانٍ عند إعادة المحاولة
    /// (نجح الإنشاء وفشل الترحيل) — فتُرحَّل المسودة نفسها بدل تكرار المستند المالي.
    /// </summary>
    private int? _createdId;

    /// <summary>يمنع إعادة حساب/تعبئة السطور أثناء الربط البرمجي.</summary>
    private bool _isBindingLines;

    /// <summary>يمنع تطبيق التصفية أثناء تهيئة أدواتها (RULE-N59).</summary>
    private bool _isInitializingFilters = true;

    // ── حدود التصفية الافتراضية (RULE-N148) ──────────────────────────────────
    private DateTime _defaultFrom;
    private DateTime _defaultTo;

    /// <summary>حالة الشاشة (RULE-N150): تبدأ دائماً في وضع العرض.</summary>
    private EntryMode _mode = EntryMode.Browsing;

    /// <summary>نص شريط العنوان الأساسي، تُضاف إليه لاحقة الوضع الحالي.</summary>
    private const string FormTitle = "(( مرتجع مشتريات ))";

    /// <summary>أوضاع شاشة الإدخال — آلة الحالة الواحدة التي تحكم الحقول والأزرار (RULE-N150).</summary>
    private enum EntryMode
    {
        /// <summary>وضع العرض: كل حقول الإدخال معطّلة حتى يضغط المستخدم «جديد» أو يحدّد مرتجعاً.</summary>
        Browsing,

        /// <summary>إضافة مرتجع جديد: الحقول والسطور مفعّلة.</summary>
        Adding,

        /// <summary>مرتجع محدد: عرض للقراءة فقط + ترحيل (مسودة) + إلغاء — <c>D-05</c>.</summary>
        Editing
    }

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmPurchaseReturnMaster()
    {
        _serviceProvider = null!;
        InitializeComponent();
    }

    public frmPurchaseReturnMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // RULE-N148 — المدى الافتراضي «هذا الشهر» (يُلتقط قبل بناء الشاشة).
        var today = DateTime.Today;
        _defaultFrom = new DateTime(today.Year, today.Month, 1);
        _defaultTo = today;

        _LoadStatusOptions();   // نصوص الحالة من البيت الواحد — مرة واحدة (RULE-N169)

        // RULE-N191 — Enter داخل لوحة المحرر يمشي بترتيب العرض (صفاً صفاً من اليمين لليسار)
        // ثم يُركِّز زر الوضع الأساسي بلا تنفيذ. الثنائي الوسيط فقط — لا الأحادي.
        KeyboardHelper.WireEnterAsTab(pnlFields, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45

        // RULE-N43 — مظهر واضح للأزرار المعطّلة (رمادية بنص باهت).
        KeyboardHelper.WireButtonDisabledVisual(
            btnSaveDraft, btnPost, btnCancelInvoice, btnAddItem, btnRemoveItem, btnRefresh, btnClearFilters);

        // RULE-N47 — الاختصارات الموحّدة: F2 جديد · F5 تحديث · Esc إلغاء تحرير/مسح تصفية/إغلاق.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, _GlobalEscape));

        // RULE-N147 — Enter = الزر الأساسي للوضع الحالي (يُقرأ عند كل ضغطة لا مرة واحدة).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // RULE-N180…N183 — تغذية الحقلين المطلوبين حيّاً (أيقونة ✗ + رسالة + ومضة رفض).
        // ترتيب الاستدعاء إلزامي: **بعد** WireFocusHighlight و**قبل** GreenThemeHelper.Apply.
        RequiredFieldHelper.Attach(this, errorProvider,
            new RequiredField(cmbSupplier, "يرجى اختيار المورد"),
            new RequiredField(cmbWarehouse, "يرجى اختيار المستودع"));

        SetupLineGrid();

        ApplyUiTheme();                       // ينتهي بالثيم الأخضر (RULE-N50)

        FilterPanelHelper.Attach(this, _serviceProvider);   // يتبنّى btnToggleFilters المُعلَن في المصمّم

        _ApplyModeState();                    // الشاشة تفتح في وضع العرض
    }

    // ── الثيم ────────────────────────────────────────────────────────────────

    /// <summary>الثيم الأخضر — آخر خطوة دائماً (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvMain);
        GreenThemeHelper.ApplyGrid(dgvItems);

        lblSummary.ForeColor = GreenThemeHelper.TextStrong;
        lblTotals.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ── التحميل ──────────────────────────────────────────────────────────────

    private async void frmPurchaseReturnMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadLookupsAsync();
            _InitializeFilters();
            await _LoadReturnsAsync();
            _ResetToBrowsing();

            ApplyUiTheme();   // الثيم آخر خطوة بعد ربط البيانات (RULE-N50)
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة مرتجع المشتريات");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task _LoadLookupsAsync()
    {
        // includeInactive: مرتجع قديم لمورد/مستودع مُعطَّل يجب أن يُربط بجهته الصحيحة.
        var warehouseResult = await WarehouseService.GetAllAsync(includeInactive: true);
        if (warehouseResult.IsSuccess && warehouseResult.Value != null)
        {
            cmbWarehouse.DisplayMember = "Name";
            cmbWarehouse.ValueMember = "Id";
            cmbWarehouse.DataSource = warehouseResult.Value.ToList();
        }
        else if (!warehouseResult.IsSuccess)
        {
            clsDialogs.Error(warehouseResult.Error ?? "تعذر تحميل قائمة المستودعات.");
        }

        var supplierResult = await SupplierService.GetAllAsync(includeInactive: true);
        if (supplierResult.IsSuccess && supplierResult.Value != null)
        {
            cmbSupplier.DisplayMember = "Name";
            cmbSupplier.ValueMember = "Id";
            cmbSupplier.DataSource = supplierResult.Value.ToList();
        }
        else if (!supplierResult.IsSuccess)
        {
            clsDialogs.Error(supplierResult.Error ?? "تعذر تحميل قائمة الموردين.");
        }

        var productResult = await ProductService.GetAllAsync();
        if (productResult.IsSuccess && productResult.Value != null)
        {
            _allProducts = productResult.Value.ToList();
        }
        else if (!productResult.IsSuccess)
        {
            clsDialogs.Error(productResult.Error ?? "تعذر تحميل قائمة الأصناف.");
        }
    }

    private static DataTable _CreateReturnsTable()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("ReturnNo", typeof(int));
        table.Columns.Add("ReturnNoText", typeof(string));
        table.Columns.Add("ReturnDate", typeof(DateTime));
        table.Columns.Add("DateText", typeof(string));
        table.Columns.Add("SupplierName", typeof(string));
        table.Columns.Add("Notes", typeof(string));
        table.Columns.Add("TotalAmount", typeof(decimal));
        table.Columns.Add("TotalText", typeof(string));
        table.Columns.Add("Status", typeof(byte));
        table.Columns.Add("StatusText", typeof(string));
        return table;
    }

    /// <summary>
    /// تحميل قائمة مرتجعات المشتريات (المصدر الوحيد للبيانات المعروضة).
    /// **لا تُفرَّغ الذاكرة عند فشل التحميل** — يبقى العرض السابق بدل شاشة فارغة (RULE-N61).
    /// </summary>
    private async Task _LoadReturnsAsync()
    {
        var result = await ReturnService.GetAllAsync();

        if (!result.IsSuccess || result.Value == null)
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل مرتجعات المشتريات.");
            _UpdateNoDataState();
            return;
        }

        var table = _CreateReturnsTable();

        foreach (var r in result.Value)
        {
            var date = r.ReturnDate.ToDateTime(TimeOnly.MinValue);

            table.Rows.Add(
                r.Id,
                r.ReturnNo,
                r.ReturnNo.ToString(CultureInfo.InvariantCulture),
                date,
                clsFormat.DateToShort(date),
                r.SupplierName ?? string.Empty,
                r.Notes ?? string.Empty,
                r.TotalAmount,
                clsFormat.Money(r.TotalAmount),
                r.Status,
                DocumentTextHelper.StatusText(r.Status));
        }

        _returnsTable = table;
        _returnsView = new DataView(_returnsTable);
        dgvMain.DataSource = _returnsView;

        _ApplyFilter();
    }

    // ── التصفية (RULE-N59/N60/N61) ───────────────────────────────────────────

    /// <summary>
    /// خيارات تصفية الحالة: «الكل» عنصر محايد محلي (ليس نص حالة)، وبقية النصوص من البيت الواحد
    /// <see cref="DocumentTextHelper.StatusOptions"/>. الترتيب «الكل» ثم 1 · 2 · 3، فيبقى
    /// <c>cbStatus.SelectedIndex</c> دالاً على قيمة الحالة الرقمية (RULE-N60).
    /// </summary>
    private void _LoadStatusOptions()
    {
        cbStatus.Items.Clear();
        cbStatus.Items.Add("الكل");

        foreach (var option in DocumentTextHelper.StatusOptions)
            cbStatus.Items.Add(option.Text);

        cbStatus.SelectedIndex = 0;
    }

    private void _InitializeFilters()
    {
        _isInitializingFilters = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
            chkOnlyCancelled.Checked = false;
            txtFilterValue.Clear();

            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _isInitializingFilters = false;
        }
    }

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isInitializingFilters) return;
        _ApplyFilter();
    }

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isInitializingFilters) return;
        _ApplyFilter();
    }

    private void txtFilterValue_TextChanged(object? sender, EventArgs e)
    {
        if (_isInitializingFilters) return;
        _ApplyFilter();
    }

    private void chkOnlyCancelled_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isInitializingFilters) return;
        _ApplyFilter();
    }

    private void dtpFrom_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializingFilters) return;
        _ApplyFilter();
    }

    private void dtpTo_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializingFilters) return;
        _ApplyFilter();
    }

    /// <summary>Enter يطبّق التصفية فوراً — لا يوجد زر بحث (RULE-N59).</summary>
    private void txtFilterValue_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None) return;

        _ApplyFilter();
        e.Handled = true;
        e.SuppressKeyPress = true;
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

    private void _SetDateRange(DateTime from, DateTime to)
    {
        _isInitializingFilters = true;
        try
        {
            dtpFrom.Value = from;
            dtpTo.Value = to;
        }
        finally
        {
            _isInitializingFilters = false;
        }

        _ApplyFilter();
    }

    /// <summary>
    /// التصفية الفورية: المدى الزمني (.Date — شامل اليوم) ثم الحالة ثم «الملغاة فقط» ثم النص
    /// حسب نوع التصفية. كل بناء على أعمدة معروضة ثابتة عبر <see cref="RowFilterHelper"/> (RULE-N60).
    /// </summary>
    private void _ApplyFilter()
    {
        if (_isInitializingFilters) return;
        if (_returnsView is null) return;

        var conditions = new List<string>
        {
            RowFilterHelper.DateRange("ReturnDate", dtpFrom.Value, dtpTo.Value)
        };

        int statusIndex = cbStatus.SelectedIndex;
        if (statusIndex > 0 && statusIndex <= DocumentTextHelper.StatusOptions.Count)
            conditions.Add(RowFilterHelper.EqualsNumber("Status", DocumentTextHelper.StatusOptions[statusIndex - 1].Value));

        if (chkOnlyCancelled.Checked)
            conditions.Add(RowFilterHelper.EqualsNumber("Status", 3));

        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 0: // العميل / المورد
                    conditions.Add(RowFilterHelper.Like("SupplierName", text));
                    break;

                case 1: // رقم الفاتورة (رقم المرتجع)
                    conditions.Add(RowFilterHelper.Like("ReturnNoText", text));
                    break;

                case 2: // البيان
                    conditions.Add(RowFilterHelper.Like("Notes", text));
                    break;

                case 3: // المبلغ — البحث في النص المعروض نفسه (clsFormat.Money) للعمود المعروض
                    conditions.Add(RowFilterHelper.Like("TotalText", text));
                    break;
            }
        }

        try
        {
            _returnsView.RowFilter = RowFilterHelper.Build(conditions);
        }
        catch (Exception ex)
        {
            // RULE-N60 — القيمة مُهرَّبة، وإن فشل البناء تُترك التصفية السابقة كما هي.
            Log.Error(ex, "تعذر تطبيق تصفية مرتجعات المشتريات — أُبقيت التصفية السابقة كما هي");
        }

        _UpdateSummary();
        _UpdateClearFiltersState();
        _UpdateNoDataState();
    }

    /// <summary>تفريغ أدوات التصفية وحدها إلى قيمها الافتراضية (RULE-N148) — بلا تحميل.</summary>
    private void _ClearFilterInputs() => _InitializeFilters();

    /// <summary>يمسح كل أدوات التصفية ثم يعيد تطبيقها على البيانات المحمّلة — بلا إعادة تحميل (RULE-N148).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
    }

    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>RULE-N43/N148: «مسح التصفية» مفعّل فقط عند وجود تصفية مختلفة عن الافتراضي.</summary>
    private void _UpdateClearFiltersState()
    {
        if (btnClearFilters is null) return;

        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || cbFilterBy.SelectedIndex > 0
            || cbStatus.SelectedIndex > 0
            || chkOnlyCancelled.Checked
            || dtpFrom.Value.Date != _defaultFrom.Date
            || dtpTo.Value.Date != _defaultTo.Date;
    }

    /// <summary>RULE-N61 — عدد المعروض من الإجمالي + عدّ الحالات + إجمالي المعروض (N2).</summary>
    private void _UpdateSummary()
    {
        if (lblSummary is null) return;

        if (_returnsView is null || _returnsTable is null)
        {
            lblSummary.Text = "المعروض: 0 من 0";
            return;
        }

        int total = _returnsTable.Rows.Count;
        int shown = _returnsView.Count;

        int draft = 0, posted = 0, cancelled = 0;
        decimal sum = 0m;

        foreach (DataRowView row in _returnsView)
        {
            if (row["TotalAmount"] is decimal amount) sum += amount;

            switch (row["Status"])
            {
                case byte status when status == (byte)InvoiceStatus.Draft: draft++; break;
                case byte status when status == (byte)InvoiceStatus.Posted: posted++; break;
                case byte status when status == (byte)InvoiceStatus.Cancelled: cancelled++; break;
            }
        }

        lblSummary.Text =
            $"المعروض: {clsFormat.Number(shown)} من {clsFormat.Number(total)} — " +
            $"مسودة: {draft} · مرحل: {posted} · ملغي: {cancelled} — " +
            $"إجمالي المعروض: {clsFormat.Money(sum)}";
    }

    /// <summary>صف «لا توجد بيانات» فوق الشبكة عند خلوها من السطور (RULE-N22).</summary>
    private void _UpdateNoDataState()
    {
        if (lblNoData is null) return;

        lblNoData.Visible = _returnsView is null || _returnsView.Count == 0;

        if (lblNoData.Visible)
            lblNoData.BringToFront();
    }

    // ── اختيار مرتجع من الشبكة ───────────────────────────────────────────────

    private async void dgvMain_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvMain.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;
            if (_invoice is not null && _invoice.Id == id) return;

            if (!_ConfirmDiscardUnsaved()) return;

            await _LoadDocumentAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار مرتجع من الشبكة");
            clsDialogs.Error("تعذر عرض بيانات المرتجع المحدد. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>حماية العمل غير المحفوظ عند الانتقال إلى مرتجع آخر أو بدء مستند جديد.</summary>
    private bool _ConfirmDiscardUnsaved()
    {
        if (_mode != EntryMode.Adding) return true;
        if (_lines.Count == 0 && string.IsNullOrWhiteSpace(txtNotes.Text)) return true;

        return clsDialogs.Confirm("لديك مرتجع جديد غير محفوظ. هل تريد تجاهله والمتابعة؟");
    }

    /// <summary>تحميل مرتجع قائم (سطور كاملة عبر العقد) وعرضه للقراءة فقط — <c>D-05</c>.</summary>
    private async Task _LoadDocumentAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await ReturnService.GetByIdAsync(id);

            if (!result.IsSuccess || result.Value is null)
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل بيانات المرتجع المحدد.");
                return;
            }

            _invoice = result.Value;
            _createdId = _invoice.Id;

            _BindDocument(_invoice);

            _mode = EntryMode.Editing;
            _ApplyModeState();
            _SelectGridRow(id);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>إعادة قراءة مستند من الخدمة بعد عملية ناجحة (ترحيل/إلغاء) لتحديث حالته.</summary>
    private async Task _ReloadDocumentAsync(int id)
    {
        var result = await ReturnService.GetByIdAsync(id);

        if (!result.IsSuccess || result.Value is null)
        {
            Log.Warning("تعذر إعادة تحميل المرتجع {ReturnId}. السبب: {Error}", id, result.Error);
            return;
        }

        _invoice = result.Value;
        _createdId = _invoice.Id;
        _BindDocument(_invoice);
    }

    private void _BindDocument(PurchaseReturnDto document)
    {
        _isBindingLines = true;
        try
        {
            lblInvoiceNo.Text = document.ReturnNo.ToString(CultureInfo.InvariantCulture);
            dtpDate.Value = document.ReturnDate.ToDateTime(TimeOnly.MinValue);
            cmbSupplier.SelectedValue = document.SupplierId;

            // WarehouseDto.Id من نوع short ⇒ الإسناد بـ short وإلا لم تُطابق أي عنصر.
            if (document.WarehouseId is >= short.MinValue and <= short.MaxValue)
                cmbWarehouse.SelectedValue = (short)document.WarehouseId;

            txtNotes.Text = document.Notes ?? string.Empty;

            _lines.Clear();
            foreach (var item in document.Items)
            {
                // نفس دلالة المصدر: السعر المعروض = UnitCost المحفوظ (InvoiceLineItem.UnitPrice).
                _lines.Add(new InvoiceLineItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitCost,
                    DiscountAmount = 0m
                });
            }
        }
        finally
        {
            _isBindingLines = false;
        }

        _CalculateTotals();
    }

    // ── آلة الحالة (RULE-N150) ───────────────────────────────────────────────

    /// <summary>
    /// آلة الحالة المركزية: تفعيل/تعطيل الحقول والسطور، وحالة كل زر، ونص شريط الوضع،
    /// والزر الافتراضي لـ Enter. تُستدعى بعد **كل** انتقال: تحميل · جديد · اختيار سطر ·
    /// حفظ · ترحيل · إلغاء · تحديث.
    /// </summary>
    private void _ApplyModeState()
    {
        bool adding = _mode == EntryMode.Adding;
        bool hasDocument = _invoice is not null;
        bool cancelled = _invoice?.Status == (byte)InvoiceStatus.Cancelled;
        bool draft = _invoice?.Status == (byte)InvoiceStatus.Draft;

        if (!adding) RequiredFieldHelper.Clear(this);   // وضع العرض: لا علامات مطلوبة على حقول معطّلة

        _SetEditorEnabled(adding);

        // RULE-N22a — شبكة السطور شبكة إدخال في وضع الإضافة فقط، وللعرض في بقية الأوضاع.
        dgvItems.ReadOnly = !adding;
        dgvItems.AllowUserToAddRows = adding;
        dgvItems.SelectionMode = adding
            ? DataGridViewSelectionMode.CellSelect
            : DataGridViewSelectionMode.FullRowSelect;
        dgvItems.MultiSelect = adding;

        // آلة حالة الأزرار (RULE-N43) — الأزرار الثلاثة تبقى **ظاهرة دائماً** ويُعطّلها/يُفعّلها
        // `Enabled` وحده كما في الشاشات التوأم؛ والتعطيل يُرسم رمادياً بـ WireButtonDisabledVisual
        // (RULE-N154) فلا يختفي الزر من الشاشة بتبدّل الوضع.
        btnNew.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;

        btnAddItem.Enabled = adding;
        btnRemoveItem.Enabled = adding;

        btnSaveDraft.Enabled = adding;

        // «ترحيل»: مستند جديد (إنشاء + ترحيل) أو مسودة قائمة — ولا يُرحَّل ما هو مرحَّل/ملغي.
        bool postable = _mode != EntryMode.Browsing && !cancelled && (_invoice is null || draft);
        btnPost.Enabled = postable;

        bool cancellable = _mode == EntryMode.Editing && hasDocument && !cancelled;
        btnCancelInvoice.Enabled = cancellable;

        _UpdateModeHeader();                       // RULE-N44
        AcceptButton = _PrimaryActionButton();     // RULE-N147
    }

    /// <summary>
    /// الزر الأساسي الذي ينفّذه <c>Enter</c> في الوضع الحالي (RULE-N147 / D-05):
    /// عرض ⇒ «جديد» · إضافة ⇒ «حفظ مسودة» · مرتجع محدد ⇒ <c>null</c> (لا تعديل ولا حفظ).
    /// ويُعاد <c>null</c> إلزامياً أثناء التركيز داخل شبكة السطور أو تحرير خلية (RULE-N153).
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        // RULE-N153 — الشبكة تملك Enter: إدخال سطر لا حفظ/ترحيل، ولا نقر لزر الوضع من داخلها.
        if (dgvItems.ContainsFocus || dgvItems.IsCurrentCellInEditMode) return null;

        return _mode switch
        {
            EntryMode.Adding => btnSaveDraft is { Visible: true, Enabled: true } ? btnSaveDraft : null,
            EntryMode.Editing => null,   // D-05 — العقد بلا UpdateAsync: لا زر حفظ يوهم بالتعديل
            _ => btnNew
        };
    }

    /// <summary>مزامنة <see cref="Form.AcceptButton"/> عند دخول/مغادرة شبكة السطور (RULE-N153).</summary>
    private void _UpdateAcceptButton() => AcceptButton = _PrimaryActionButton();

    /// <summary>حقول المحرر التي تُعطَّل في وضع العرض وتُفعَّل عند «جديد» فقط.</summary>
    private Control[] _EditorInputs() => new Control[]
    {
        dtpDate, cmbSupplier, cmbWarehouse, txtNotes
    };

    private void _SetEditorEnabled(bool enabled)
    {
        foreach (var input in _EditorInputs())
            input.Enabled = enabled;
    }

    /// <summary>شريط الوضع (RULE-N44) + نص الحالة من البيت الواحد DocumentTextHelper.</summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _mode switch
        {
            EntryMode.Adding => $"{FormTitle} — إضافة مرتجع جديد",
            EntryMode.Editing => _invoice is null
                ? $"{FormTitle} — عرض المستند"
                : $"{FormTitle} — مرتجع رقم {_invoice.ReturnNo}: {DocumentTextHelper.StatusText(_invoice.Status)}",
            _ => $"{FormTitle} — وضع العرض"
        };

        lblStatus.Text = _invoice is null ? "جديد" : DocumentTextHelper.StatusText(_invoice.Status);
    }

    // ── إعادة الضبط / جديد ───────────────────────────────────────────────────

    /// <summary>إعادة الشاشة إلى وضع العرض: تفريغ المحرر والسطور وتعطيل الحقول (RULE-N149).</summary>
    private void _ResetToBrowsing()
    {
        _invoice = null;
        _createdId = null;

        _isBindingLines = true;
        try
        {
            lblInvoiceNo.Text = "جديد";
            dtpDate.Value = DateTime.Now;
            txtNotes.Clear();

            if (cmbSupplier.Items.Count > 0) cmbSupplier.SelectedIndex = 0;
            if (cmbWarehouse.Items.Count > 0) cmbWarehouse.SelectedIndex = 0;

            _lines.Clear();
        }
        finally
        {
            _isBindingLines = false;
        }

        errorProvider.SetError(dgvItems, "");
        _CalculateTotals();

        dgvMain.ClearSelection();

        _mode = EntryMode.Browsing;
        _ApplyModeState();
        btnNew.Focus();
    }

    /// <summary>بدء إدخال مرتجع جديد (زر «جديد» / F2).</summary>
    private void _StartNewEntry()
    {
        if (!_ConfirmDiscardUnsaved()) return;

        _ResetToBrowsing();

        _mode = EntryMode.Adding;
        _ApplyModeState();

        cmbSupplier.Focus();
    }

    // ── الأوامر ──────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _StartNewEntry();

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى الافتراضي (RULE-N148):
    /// التصفية ← الدلائل ← المرتجعات ← المحرر ووضع العرض.
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();
            await _LoadLookupsAsync();
            await _LoadReturnsAsync();
            _ResetToBrowsing();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث بيانات مرتجعات المشتريات");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void btnSaveDraft_Click(object? sender, EventArgs e) => await _SaveAsync(post: false);

    private async void btnPost_Click(object? sender, EventArgs e) => await _SaveAsync(post: true);

    /// <summary>
    /// قواعد السطور التي لا تُوصَف كـ«حقل مطلوب» (الحقلان المطلوبان يُعلنان في
    /// <c>RequiredFieldHelper.Attach</c>): سطر واحد على الأقل · كمية &gt; 0 · سعر غير سالب ·
    /// كل سطر مرتبط بصنف مسجَّل. النصوص والأحكام منقولة حرفياً من الشاشة المصدر.
    /// </summary>
    private bool _ValidateRangeRules()
    {
        bool isValid = true;

        if (!_lines.Any(l => l.ProductId > 0))
        {
            const string itemsMessage = "يجب إضافة صنف واحد على الأقل للإرجاع";
            errorProvider.SetError(dgvItems, itemsMessage);
            clsInputFeedback.ShowRejectTooltip(dgvItems, itemsMessage, toolTip);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(dgvItems, "");
        }

        // كمية صفر/سالب تُرفض عربياً قبل نداء الخدمة (لا تصل للـBLL أصلاً).
        var invalidQtyLine = _lines.FirstOrDefault(l => l.ProductId > 0 && l.Quantity <= 0);
        var invalidPriceLine = _lines.FirstOrDefault(l => l.ProductId > 0 && l.UnitPrice < 0);
        var unresolvedLine = _lines.FirstOrDefault(l => l.ProductId <= 0 &&
            (!string.IsNullOrWhiteSpace(l.ProductCode) || l.Quantity != 0m || l.UnitPrice != 0m));

        if (invalidQtyLine != null || invalidPriceLine != null || unresolvedLine != null)
        {
            string lineMessage = invalidQtyLine != null
                ? $"الكمية يجب أن تكون أكبر من صفر ({invalidQtyLine.ProductName})"
                : invalidPriceLine != null
                    ? $"سعر الوحدة لا يمكن أن يكون سالباً ({invalidPriceLine.ProductName})"
                    : $"السطر «{unresolvedLine!.ProductCode}» غير مرتبط بصنف مسجَّل — صحّح الكود/الباركود أو احذف السطر";

            errorProvider.SetError(dgvItems, lineMessage);
            clsInputFeedback.ShowRejectTooltip(dgvItems, lineMessage, toolTip);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(dgvItems, "");   // لا يبقى تلميح رفض قديم بعد تصحيح السطور
        }

        if (!isValid)
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");

        return isValid;
    }

    /// <summary>
    /// حفظ المستند: <paramref name="post"/> = false ⇒ مسودة، true ⇒ إنشاء ثم ترحيل.
    /// <para>
    /// <b>منع تكرار المستند:</b> بعد نجاح <c>CreateAsync</c> يُثبَّت <see cref="_createdId"/>،
    /// فأي إعادة محاولة (بعد فشل الترحيل) تُرحّل **نفس** المعرّف ولا تنشئ مرتجعاً ثانياً —
    /// لا يوجد <c>UpdateAsync</c> في العقد.
    /// </para>
    /// </summary>
    private async Task _SaveAsync(bool post)
    {
        // لا حفظ من وضع العرض، ولا حفظ من مستند محمَّل لم تُنشئه الشاشة (D-05).
        if (_mode != EntryMode.Adding && _createdId is null) return;

        // منع تكرار المستند: مسودة أنشأتها هذه الشاشة لا تُحفظ مرة ثانية (لا UpdateAsync في العقد).
        if (!post && _createdId is int existingDraftId)
        {
            clsDialogs.Warning(
                $"المرتجع رقم {existingDraftId} محفوظ بالفعل كمسودة — لن تُنشأ نسخة ثانية.\n" +
                "استخدم «ترحيل» لترحيل المسودة نفسها.",
                "حفظ غير مطلوب");
            return;
        }

        // الحقول المطلوبة: الحكم والوسم في البيت الواحد RequiredFieldHelper (RULE-N172).
        if (!RequiredFieldHelper.Validate(this))
        {
            if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();
            return;
        }

        if (!_ValidateRangeRules()) return;

        if (post && !clsDialogs.Confirm("ترحيل المرتجع؟ سيتم تقليل المخزون وتقليل رصيد المورد.")) return;

        btnSaveDraft.Enabled = false;
        btnPost.Enabled = false;
        Cursor = Cursors.WaitCursor;

        try
        {
            int documentId;

            if (_createdId is int existingId)
            {
                documentId = existingId;   // إعادة المحاولة تُرحّل المستند نفسه (لا تكرار)
            }
            else
            {
                var items = _lines
                    .Where(l => l.ProductId > 0)
                    .Select(l => new CreatePurchaseReturnItemRequest(
                        0, l.ProductId, 1, l.Quantity, l.UnitPrice, l.LineTotal))
                    .ToList();

                int supplierId = Convert.ToInt32(cmbSupplier.SelectedValue ?? 0);
                int warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue ?? 0);

                var request = new CreatePurchaseReturnRequest(
                    null,
                    supplierId,
                    warehouseId,
                    dtpDate.Value,
                    null,
                    null,
                    txtNotes.Text,
                    items);

                var result = await ReturnService.CreateAsync(request);

                if (!result.IsSuccess || result.Value is not { } created)
                {
                    clsDialogs.Error(result.Error ?? "تعذر إنشاء المرتجع.");
                    return;
                }

                _createdId = created.Id;
                _invoice = created;
                lblInvoiceNo.Text = created.ReturnNo.ToString(CultureInfo.InvariantCulture);

                // المستند صار محفوظاً ⇒ عرض للقراءة فقط (D-05) — حتى لو فشل ترحيله بعده.
                _mode = EntryMode.Editing;

                documentId = created.Id;
            }

            if (post)
            {
                var postResult = await ReturnService.PostAsync(documentId);

                if (!postResult.IsSuccess)
                {
                    // المسودة محفوظة: لا إغلاق ولا مستند جديد — إعادة المحاولة تُرحّل نفس المعرّف.
                    _invoice = _invoice is null ? null : _invoice with { Status = (byte)InvoiceStatus.Draft };

                    clsDialogs.Warning(
                        $"أُنشئ المرتجع كمسودة برقم {documentId} وتعذر ترحيله.\n" +
                        "لم يُنشأ مستند جديد: أعد الضغط على «ترحيل» لترحيل نفس المسودة.\n" +
                        "سطور المرتجع مقفلة الآن لأنها محفوظة كمسودة.",
                        "لم يُرحَّل المرتجع");

                    await _ReloadDocumentAsync(documentId);
                    return;
                }

                clsDialogs.Success("تم الترحيل");

                foreach (var line in _lines.Where(l => l.ProductId > 0))
                    EventBus.Publish(new StockChangedMessage(line.ProductId));

                if (_invoice is not null)
                    _invoice = _invoice with { Status = (byte)InvoiceStatus.Posted };
            }
            else
            {
                clsDialogs.Success("تم الحفظ");
            }

            EventBus.Publish(new PurchaseReturnChangedMessage(documentId));

            await _ReloadDocumentAsync(documentId);
            await _LoadReturnsAsync();
            _SelectGridRow(documentId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في حفظ مرتجع المشتريات");
            clsDialogs.Error("خطأ في حفظ المرتجع. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            Cursor = Cursors.Default;

            // الحالة من المصدر الواحد (RULE-N150) — وتُحترم حالة النموذج بعد الإغلاق.
            if (!IsDisposed) _ApplyModeState();
        }
    }

    /// <summary>
    /// «إلغاء المستند» — يمرّ عبر <c>CancelAsync</c> بعد تأكيد صريح (<c>clsDialogs.Confirm</c>).
    /// </summary>
    private async void btnCancelInvoice_Click(object? sender, EventArgs e)
    {
        if (_invoice is not { } document)
        {
            clsDialogs.Warning("يرجى اختيار مرتجع من القائمة أولاً.");
            return;
        }

        if (document.Status == (byte)InvoiceStatus.Cancelled)
        {
            clsDialogs.Warning("المرتجع ملغي بالفعل.");
            return;
        }

        if (!clsDialogs.Confirm($"هل أنت متأكد من إلغاء المرتجع رقم {document.ReturnNo}؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnCancelInvoice.Enabled = false;

        try
        {
            var result = await ReturnService.CancelAsync(document.Id);

            if (!result.IsSuccess)
            {
                clsDialogs.Error(result.Error ?? "تعذر إلغاء المرتجع.");
                return;
            }

            EventBus.Publish(new PurchaseReturnChangedMessage(document.Id));

            _invoice = _invoice is null
                ? null
                : _invoice with { Status = (byte)InvoiceStatus.Cancelled };

            clsDialogs.Success("تم إلغاء المرتجع بنجاح.");

            await _ReloadDocumentAsync(document.Id);
            await _LoadReturnsAsync();
            _SelectGridRow(document.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إلغاء المرتجع {ReturnId}", document.Id);
            clsDialogs.Error("حدث خطأ أثناء إلغاء المرتجع. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
            if (!IsDisposed) _ApplyModeState();
        }
    }

    // ── شبكة السطور (RULE-N22a) ──────────────────────────────────────────────

    private void SetupLineGrid()
    {
        dgvItems.AutoGenerateColumns = false;
        dgvItems.DataSource = _lines;
        dgvItems.DataError += (s, e) => e.ThrowException = false;

        // RULE-N153 — الشبكة تملك Enter: يُفرَّغ AcceptButton عند دخولها ويُعاد عند مغادرتها.
        dgvItems.Enter += (s, e) => _UpdateAcceptButton();
        dgvItems.Leave += (s, e) => _UpdateAcceptButton();

        dgvItems.CellEndEdit += dgvItems_CellEndEdit;

        // تنسيق الأرقام من البيت الواحد clsFormat: كميات بثلاث منازل ومبالغ بمنزلتين (RULE-N169).
        colQuantity.DefaultCellStyle.Format = clsFormat.QuantityPattern;
        colUnitPrice.DefaultCellStyle.Format = clsFormat.MoneyPattern;
        colLineTotal.DefaultCellStyle.Format = clsFormat.MoneyPattern;
    }

    /// <summary>
    /// نهاية تحرير خلية: كود/باركود يُترجم إلى صنف مسجَّل (إدخال سريع بالسكانر — RULE-N42)،
    /// ثم تُعاد المجاميع وتُحدَّث الشبكة.
    /// </summary>
    private void dgvItems_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (_isBindingLines || e.RowIndex < 0 || e.RowIndex >= dgvItems.Rows.Count) return;

        // السطر يُقرأ من DataBoundItem لا بفهرس القائمة (صف الإدخال الجديد قد يكون خارج حدود القائمة).
        if (dgvItems.Rows[e.RowIndex].DataBoundItem is not InvoiceLineItem line) return;

        if (e.ColumnIndex >= 0 && dgvItems.Columns[e.ColumnIndex].Name == colCode.Name)
        {
            string? code = line.ProductCode?.Trim();

            if (!string.IsNullOrWhiteSpace(code))
            {
                var product = _allProducts.FirstOrDefault(p =>
                    !string.IsNullOrWhiteSpace(p.Barcode)
                    && string.Equals(p.Barcode, code, StringComparison.OrdinalIgnoreCase));

                if (product is not null)
                {
                    line.ProductId = product.Id;
                    line.ProductName = product.Name;
                    if (line.Quantity == 0m) line.Quantity = 1m;
                }
            }
        }

        _CalculateTotals();
        dgvItems.Refresh();
    }

    /// <summary>إجمالي المرتجع = مجموع السطور المرتبطة بصنف فعلاً (نفس ما سيُرسَل).</summary>
    private void _CalculateTotals()
    {
        decimal total = _lines.Where(l => l.ProductId > 0).Sum(l => l.LineTotal);
        lblTotals.Text = "إجمالي المرتجع: " + clsFormat.Money(total);
    }

    private void btnAddItem_Click(object? sender, EventArgs e)
    {
        _lines.Add(new InvoiceLineItem());
        _CalculateTotals();

        int index = _lines.Count - 1;
        if (index >= 0 && index < dgvItems.Rows.Count)
            dgvItems.CurrentCell = dgvItems.Rows[index].Cells[colCode.Index];

        dgvItems.Focus();

        if (dgvItems.CurrentCell is not null && !dgvItems.IsCurrentCellInEditMode)
            dgvItems.BeginEdit(true);
    }

    private void btnRemoveItem_Click(object? sender, EventArgs e)
    {
        if (dgvItems.CurrentRow?.DataBoundItem is InvoiceLineItem line)
        {
            _lines.Remove(line);
            _CalculateTotals();
        }
    }

    // ── عناصر مساعدة ─────────────────────────────────────────────────────────

    private void _SelectGridRow(int returnId)
    {
        foreach (DataGridViewRow row in dgvMain.Rows)
        {
            if (row.DataBoundItem is DataRowView dataRow
                && dataRow["Id"] is int id
                && id == returnId)
            {
                row.Selected = true;
                dgvMain.CurrentCell = row.Cells[colReturnNo.Index];
                break;
            }
        }
    }

    /// <summary>Esc: يُلغي تحرير الخلية أولاً، ثم يمسح التصفية إن كان المؤشر فيها، وإلا يُغلق.</summary>
    private void _GlobalEscape()
    {
        if (dgvItems.IsCurrentCellInEditMode)
        {
            dgvItems.CancelEdit();
            return;
        }

        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        Close();
    }

    // ── قائمة السياق (RULE-N23) ──────────────────────────────────────────────

    private void cmsRows_Opening(object? sender, CancelEventArgs e)
    {
        if (dgvMain.CurrentRow?.DataBoundItem is not DataRowView || dgvMain.SelectedRows.Count == 0)
        {
            e.Cancel = true;
            return;
        }

        cmsView.Enabled = true;
        cmsRefresh.Enabled = btnRefresh.Enabled;
    }

    private async void cmsView_Click(object? sender, EventArgs e)
    {
        try
        {
            if (dgvMain.CurrentRow?.DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;
            if (!_ConfirmDiscardUnsaved()) return;

            await _LoadDocumentAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء فتح المرتجع من قائمة السياق");
            clsDialogs.Error("تعذر عرض بيانات المرتجع المحدد. يرجى إعادة المحاولة.");
        }
    }

    private void cmsRefresh_Click(object? sender, EventArgs e) => btnRefresh.PerformClick();
}
