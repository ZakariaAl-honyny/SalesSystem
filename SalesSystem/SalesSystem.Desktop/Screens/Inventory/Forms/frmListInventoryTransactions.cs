using System.Data;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// قائمة حركات المخزون — شاشة عرض للقراءة فقط (RULE-N100 لا تنطبق: مستند/حركة ⇒ frmList*).
/// التصفية كلها داخل <c>gbFilter</c> وفورية (RULE-N56…N61)، والثيم أخضر (RULE-N50…N54).
/// </summary>
public partial class frmListInventoryTransactions : Form
{
    /// <summary>RULE-N46: لون الحقل النشط الموحّد.</summary>
    private static readonly Color FocusColor = Color.FromArgb(0xD6, 0xE4, 0xF0);

    private readonly IServiceProvider _serviceProvider;
    private readonly CancellationTokenSource _loadCts = new();

    private DataTable _transactionsTable = new();
    private DataView _transactionsView = new();

    // RULE-N148: الوضع الافتراضي لأدوات التصفية («هذا الشهر») — يُلتقط مرة عند الإنشاء
    // وتُعاد إليه أدوات التصفية عند «مسح التصفية» وعند «تحديث».
    private readonly DateTime _defaultFrom;
    private readonly DateTime _defaultTo;

    // يبدأ true حتى لا تنطلق تصفية من داخل المنشئ/التحميل قبل اكتمال تهيئة أدوات التصفية.
    private bool _isInitializing = true;

    // حارس بسيط ضد التنفيذ المزدوج لعرض التفاصيل (نقر مزدوج + قائمة سياق أثناء الانتظار).
    private bool _isViewingDetails;

    public frmListInventoryTransactions(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        var today = DateTime.Today;
        _defaultFrom = new DateTime(today.Year, today.Month, 1);
        _defaultTo = today;

        // RULE-N40…N46 — تنقل وتحديد وتظليل وأسهم RTL.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — الأزرار المعطّلة تظهر رمادية باهتة.
        KeyboardHelper.WireButtonDisabledVisual(btnRefresh, btnClose, btnClearFilters);

        // RULE-N47 — F5 تحديث · Esc مسح كل التصفية وإرجاع «هذا الشهر» (لا يوجد زر بحث: RULE-N59).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, _ClearAllFilters));

        // RULE-N147: شاشة قائمة بلا «جديد» ⇒ Enter = الزر الأساسي «تحديث» (لا آلة حالة إدخال).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        FormClosing += (s, e) =>
        {
            try { _loadCts.Cancel(); } catch { /* تجاهل */ }
        };
        FormClosed += (s, e) => _loadCts.Dispose();

        _SetupToolTips();
        ApplyUiTheme();
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    private IInventoryDesktopService DesktopService
        => _serviceProvider.GetRequiredService<IInventoryDesktopService>();

    /// <summary>RULE-N147: الزر الأساسي للشاشة — «تحديث» (لا زر «جديد» ولا آلة حالة إدخال في شاشة القائمة).</summary>
    private Button? _PrimaryActionButton() => btnRefresh;


    /// <summary>تلميحات الأزرار مع اختصاراتها (RULE-N47).</summary>
    private void _SetupToolTips()
    {
        toolTip1.SetToolTip(btnRefresh, "تحديث القائمة من الخدمة (F5)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة (Esc يمسح التصفية فقط)");
        toolTip1.SetToolTip(btnRangeToday, "حركات اليوم فقط");
        toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
        toolTip1.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّي به");
        toolTip1.SetToolTip(txtFilterValue, "اكتب قيمة التصفية — التصفية فورية");
        toolTip1.SetToolTip(cbMovementType, "نوع الحركة — الكل أو نوع محدد");
        toolTip1.SetToolTip(chkOnlyIn, "إظهار الحركات الواردة فقط (كمية موجبة)");
        toolTip1.SetToolTip(chkOnlyOut, "إظهار الحركات الصادرة فقط (كمية سالبة)");
    }

    // ═════════════════════════════════════════════════════════════════
    //  الثيم الأخضر (RULE-N50…RULE-N54)
    // ═════════════════════════════════════════════════════════════════

    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvTransactions);

        // RULE-N22: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه (RULE-N52).
        dgvTransactions.ReadOnly = true;
        dgvTransactions.AllowUserToAddRows = false;
        dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTransactions.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ═════════════════════════════════════════════════════════════════
    //  الشبكة
    // ═════════════════════════════════════════════════════════════════

    private static DataGridViewTextBoxColumn _MakeColumn(string name, string header, int fill, int min, string? format = null)
    {
        var column = new DataGridViewTextBoxColumn
        {
            Name = name, HeaderText = header, DataPropertyName = name,
            MinimumWidth = min, FillWeight = fill, ReadOnly = true,
        };

        if (!string.IsNullOrEmpty(format))
        {
            // FormatProvider ثابت (ميلادي/أرقام لاتينية) حتى لا يتأثر العرض بثقافة جهاز المستخدم.
            column.DefaultCellStyle.Format = format!;
            column.DefaultCellStyle.FormatProvider = CultureInfo.InvariantCulture;
        }

        return column;
    }

    private void _SetupGridColumns()
    {
        // فصل البيانات قبل إعادة بناء الأعمدة (تفادي تعديل مجموعة أعمدة مرتبطة).
        dgvTransactions.DataSource = null;
        dgvTransactions.AutoGenerateColumns = false;
        dgvTransactions.Columns.Clear();

        dgvTransactions.Columns.Add(_MakeColumn("Id", "رقم", 5, 60));
        dgvTransactions.Columns.Add(_MakeColumn("TransactionNo", "رقم الحركة", 8, 100));
        dgvTransactions.Columns.Add(_MakeColumn("MovementTypeDisplay", "نوع الحركة", 12, 120));
        dgvTransactions.Columns.Add(_MakeColumn("WarehouseName", "المستودع", 12, 130));
        dgvTransactions.Columns.Add(_MakeColumn("CreatedAt", "التاريخ", 10, 130, "yyyy-MM-dd HH:mm"));
        dgvTransactions.Columns.Add(_MakeColumn("Quantity", "الكمية", 8, 100, "N3"));
        dgvTransactions.Columns.Add(_MakeColumn("Notes", "ملاحظات", 15, 200));

        // الشاشة عرض فقط: لا تحرير ولا إضافة أسطر (RULE-N22).
        dgvTransactions.AllowUserToAddRows = false;
    }

    // ═════════════════════════════════════════════════════════════════
    //  تحميل البيانات (F5)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>RULE-N25/N26: تحميل غير متزامن مع Cursor.WaitCursor واستعادته في finally.</summary>
    private async Task _RefreshListAsync()
    {
        Cursor = Cursors.WaitCursor;
        dgvTransactions.Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("TransactionNo", typeof(string));
            table.Columns.Add("MovementType", typeof(byte));
            table.Columns.Add("MovementTypeDisplay", typeof(string));
            table.Columns.Add("WarehouseName", typeof(string));
            table.Columns.Add("Products", typeof(string));
            table.Columns.Add("ReferenceNo", typeof(string));
            table.Columns.Add("CreatedAt", typeof(DateTime));
            table.Columns.Add("CreatedDate", typeof(string));
            table.Columns.Add("Quantity", typeof(decimal));
            table.Columns.Add("Notes", typeof(string));

            var result = await DesktopService.GetMovementsAsync(pageSize: 500, ct: _loadCts.Token);

            if (result.IsSuccess && result.Value is { Count: > 0 })
            {
                foreach (var t in result.Value)
                {
                    table.Rows.Add(
                        t.Id,
                        t.TransactionNo,
                        t.MovementType,
                        t.MovementTypeDisplay,
                        t.WarehouseName ?? "",
                        _ProductsOf(t),
                        t.ReferenceId?.ToString() ?? "",
                        t.CreatedAt,
                        t.CreatedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        _SignedQuantity(t),
                        t.Notes ?? "");
                }
            }
            else if (!result.IsSuccess)
            {
                clsDialogs.Error(result.Error ?? "حدث خطأ في تحميل قائمة الحركات");
            }

            _transactionsTable = table;
            _transactionsView = new DataView(_transactionsTable);
            dgvTransactions.DataSource = _transactionsView;

            _ApplyFilter();
        }
        catch (OperationCanceledException)
        {
            // أُغلقت الشاشة أثناء التحميل — لا رسالة.
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة حركات المخزون في الشاشة");
            clsDialogs.Error("تعذر تحميل حركات المخزون. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
            {
                btnRefresh.Enabled = true;
                Cursor = Cursors.Default;
                dgvTransactions.Cursor = Cursors.Default;
            }
        }
    }

    private async Task _ReloadFormAsync()
    {
        _SetupGridColumns();
        await _RefreshListAsync();

        // إعادة ضبط الاختيار بعد كل تحميل (RULE-N148: «تحديث» يُرجع الاختيار للافتراضي).
        if (!IsDisposed)
            dgvTransactions.ClearSelection();
    }

    /// <summary>أسماء الأصناف في الحركة (من سطور المستند) — تُستخدم في التصفية والبيان.</summary>
    private static string _ProductsOf(InventoryTransactionDto t)
    {
        if (t.Lines is not { Count: > 0 })
            return string.Empty;

        var names = new List<string>();

        foreach (var line in t.Lines)
        {
            var name = line.ProductUnitName;

            if (string.IsNullOrWhiteSpace(name))
                continue;

            if (!names.Contains(name))
                names.Add(name);
        }

        return string.Join("، ", names);
    }

    /// <summary>
    /// الكمية الموقّعة للحركة: سالبة للصادر (2 مرتجع مشتريات · 3 مبيعات · 5 تحويل خارج · 9 تلف · 11 صرف داخلي)،
    /// موجبة للوارد (1 مشتريات · 4 مرتجع مبيعات · 6 تحويل داخل · 10 رصيد افتتاحي · 12 استلام داخلي)،
    /// والجرد/التسوية تُحفظ إشارتها كما هي.
    /// </summary>
    private static decimal _SignedQuantity(InventoryTransactionDto t)
    {
        decimal quantity = 0m;

        if (t.Lines is { Count: > 0 })
        {
            foreach (var line in t.Lines)
                quantity += line.Quantity;
        }

        bool outgoing = t.MovementType is 2 or 3 or 5 or 9 or 11;

        if (outgoing)
            return -Math.Abs(quantity);

        bool incoming = t.MovementType is 1 or 4 or 6 or 10 or 12;

        return incoming ? Math.Abs(quantity) : quantity;
    }

    // ═════════════════════════════════════════════════════════════════
    //  التصفية الفورية (RULE-N56…N61)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// التصفية الفورية على <see cref="DataView.RowFilter"/>: النطاق الزمني + نوع الحركة + الوارد/الصادر
    /// + نص التصفية على الحقل المختار. لا يوجد زر بحث (RULE-N59)، والمقارنة بـ SelectedIndex لا بالنص (RULE-N60).
    /// </summary>
    private void _ApplyFilter()
    {
        if (_isInitializing)
            return;

        var conditions = new List<string>();

        // 1) النطاق الزمني — نطاق معكوس = نُبادل الطرفين، ومقارنة نصية بصيغة yyyy-MM-dd (بلا اعتماد على ثقافة الجهاز).
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;

        if (from > to)
            (from, to) = (to, from);

        string fromKey = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        string toKey = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        conditions.Add($"CreatedDate >= '{fromKey}'");
        conditions.Add($"CreatedDate <= '{toKey}'");

        // 2) نوع الحركة — SelectedIndex 1..12 = قيمة MovementType الرقمية.
        int movementType = cbMovementType.SelectedIndex;

        if (movementType > 0)
            conditions.Add($"MovementType = {movementType}");

        // 3) الوارد/الصادر — حسب إشارة الكمية.
        if (chkOnlyIn.Checked)
            conditions.Add("Quantity > 0");

        if (chkOnlyOut.Checked)
            conditions.Add("Quantity < 0");

        // 4) نص التصفية على عمود معروف فقط (RULE-N60).
        string text = txtFilterValue.Text.Trim();

        if (text.Length > 0)
        {
            string column = cbFilterBy.SelectedIndex switch
            {
                1 => "WarehouseName",
                2 => "MovementTypeDisplay",
                3 => "ReferenceNo",
                4 => "Notes",
                _ => "Products",
            };

            conditions.Add(RowFilterHelper.Like(column, text));
        }

        _transactionsView.RowFilter = conditions.Count == 0 ? string.Empty : string.Join(" AND ", conditions);

        _UpdateSummary();
        _UpdateClearFiltersState();   // RULE-N148: حالة زر «مسح التصفية» بعد كل تطبيق
    }

    /// <summary>RULE-N61: شريط الملخص — المعروض من الكل + كميات الوارد والصادر (3 منازل).</summary>
    private void _UpdateSummary()
    {
        int shown = 0;
        decimal inQuantity = 0m;
        decimal outQuantity = 0m;

        foreach (DataRowView row in _transactionsView)
        {
            shown++;

            decimal quantity = row["Quantity"] is decimal value ? value : 0m;

            if (quantity > 0m)
                inQuantity += quantity;
            else if (quantity < 0m)
                outQuantity += -quantity;
        }

        lblSummary.Text =
            $"المعروض: {shown} من {_transactionsTable.Rows.Count} — وارد: {inQuantity:N3} · صادر: {outQuantity:N3}";

        lblRecordsCount.Text = shown.ToString();

        // سطر «لا توجد بيانات» — طبقة نصية فوق الشبكة عند فراغ النتيجة.
        bool empty = shown == 0;
        lblEmpty.Visible = empty;

        if (empty)
            lblEmpty.BringToFront();
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

        _ApplyFilter();
    }

    /// <summary>«هذا الشهر» = النطاق الافتراضي (من أول الشهر الحالي حتى اليوم) — زر النطاق السريع.</summary>
    private void _SetDateRangeThisMonth()
        => _SetDateRange(_defaultFrom, _defaultTo);

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل (RULE-N148)، ويُعيد التركيز إلى حقل التصفية.
    /// </summary>
    private void btnClearFilters_Click(object sender, EventArgs e)
        => _ClearAllFilters();

    /// <summary>
    /// تفريغ أدوات التصفية وحدها إلى الوضع الافتراضي (بلا تطبيق ولا إعادة تحميل) — RULE-N148.
    /// تُستهلك من «مسح التصفية» و«تحديث» وعند فتح الشاشة.
    /// </summary>
    private void _ClearFilterInputs()
    {
        bool wasInitializing = _isInitializing;

        // منع تصفية وسيطة عند كل ضبط حقل — تُطبَّق مرة واحدة بعد التفريغ.
        _isInitializing = true;
        try
        {
            txtFilterValue.Clear();
            cbFilterBy.SelectedIndex = 0;
            cbMovementType.SelectedIndex = 0;   // «الكل»
            chkOnlyIn.Checked = false;
            chkOnlyOut.Checked = false;
            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _isInitializing = wasInitializing;
        }
    }

    /// <summary>RULE-N148: يمسح أدوات التصفية ثم يعيد تطبيق التصفية الافتراضية على المحمَّل (بلا تحميل).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
        txtFilterValue.Focus();
    }

    /// <summary>RULE-N43/RULE-N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية تختلف عن الوضع الافتراضي.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || cbFilterBy.SelectedIndex > 0
            || cbMovementType.SelectedIndex > 0
            || chkOnlyIn.Checked
            || chkOnlyOut.Checked
            || dtpFrom.Value.Date != _defaultFrom.Date
            || dtpTo.Value.Date != _defaultTo.Date;
    }

    // ═════════════════════════════════════════════════════════════════
    //  التفاصيل
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// عرض تفاصيل الحركة المحددة — async (RULE-N25/N26/N29): انتظار غير متزامن بلا
    /// <c>GetAwaiter().GetResult()</c>، مع مؤشر انتظار واستعادته في <c>finally</c>،
    /// وحارس ضد التنفيذ المزدوج، ورسالة عربية عند الفشل (بلا <c>ex.Message</c>).
    /// </summary>
    private async void _ViewDetails()
    {
        if (_isViewingDetails) return;   // حارس: لا تنفيذ مزدوج أثناء الانتظار
        if (dgvTransactions.SelectedRows.Count == 0) return;
        var row = dgvTransactions.SelectedRows[0];
        if (row.DataBoundItem is not DataRowView drv) return;

        int id = Convert.ToInt32(drv["Id"]);

        _isViewingDetails = true;
        Cursor = Cursors.WaitCursor;
        dgvTransactions.Cursor = Cursors.WaitCursor;

        try
        {
            var result = await DesktopService.GetInventoryTransactionByIdAsync(id);

            if (!result.IsSuccess || result.Value is null)
            {
                clsDialogs.Error("تعذر تحميل تفاصيل الحركة");
                return;
            }

            var tx = result.Value;
            var details = new System.Text.StringBuilder();
            details.AppendLine($"رقم الحركة: {tx.TransactionNo}");
            details.AppendLine($"النوع: {tx.MovementTypeDisplay}");
            details.AppendLine($"المستودع: {tx.WarehouseName ?? "-"}");
            details.AppendLine($"التاريخ: {tx.CreatedAt.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}");
            details.AppendLine($"ملاحظات: {tx.Notes ?? "-"}");
            details.AppendLine($"--- البنود ({tx.Lines?.Count ?? 0}) ---");

            if (tx.Lines is { Count: > 0 })
            {
                foreach (var line in tx.Lines)
                {
                    details.AppendLine($"- {line.ProductUnitName ?? "?"} | كمية: {line.Quantity:N3} | تكلفة: {line.UnitCost:N2}");
                }
            }

            clsDialogs.Info(details.ToString());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل عرض تفاصيل حركة المخزون {TransactionId}", id);
            clsDialogs.Error("تعذر تحميل تفاصيل الحركة. يرجى إعادة المحاولة.");
        }
        finally
        {
            _isViewingDetails = false;

            if (!IsDisposed)
            {
                Cursor = Cursors.Default;
                dgvTransactions.Cursor = Cursors.Default;
            }
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  Form Events
    // ═════════════════════════════════════════════════════════════════

    private async void frmListInventoryTransactions_Load(object sender, EventArgs e)
    {
        _isInitializing = true;
        try
        {
            // الشاشة تفتح على الوضع الافتراضي لأدوات التصفية (RULE-N148).
            _ClearFilterInputs();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تهيئة أدوات تصفية حركات المخزون");
        }
        finally
        {
            _isInitializing = false;
        }

        try
        {
            await _ReloadFormAsync();
            _UpdateClearFiltersState();   // RULE-N148: حالة الزر بعد التحميل مباشرة
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل شاشة حركات المخزون");
        }
    }

    private void btnClose_Click(object sender, EventArgs e) { Close(); }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة (RULE-N148): أدوات التصفية إلى الافتراضي
    /// ← الشبكة من الخدمة ← الملخص والاختيار. «مسح التصفية» وحده يمسح التصفية بلا إعادة تحميل.
    /// </summary>
    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            _ClearFilterInputs();
            await _ReloadFormAsync();
            _UpdateClearFiltersState();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحديث قائمة حركات المخزون");
            clsDialogs.Error("تعذر تحديث قائمة الحركات. يرجى إعادة المحاولة.");
        }
    }

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;

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
        if (_isInitializing) return;

        _ApplyFilter();
    }

    private void cbMovementType_SelectedIndexChanged(object sender, EventArgs e)
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

    private void chkOnlyIn_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;

        if (chkOnlyIn.Checked && chkOnlyOut.Checked)
        {
            _isInitializing = true;
            try { chkOnlyOut.Checked = false; } finally { _isInitializing = false; }
        }

        _ApplyFilter();
    }

    private void chkOnlyOut_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;

        if (chkOnlyOut.Checked && chkOnlyIn.Checked)
        {
            _isInitializing = true;
            try { chkOnlyIn.Checked = false; } finally { _isInitializing = false; }
        }

        _ApplyFilter();
    }

    private void btnRangeToday_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today, DateTime.Today);

    private void btnRangeMonth_Click(object sender, EventArgs e)
        => _SetDateRangeThisMonth();

    private void btnRange30_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

    private void dgvTransactions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        _ViewDetails();
    }

    private void ShowContextMenu(MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        var hit = dgvTransactions.HitTest(e.X, e.Y);
        if (hit.RowIndex < 0) return;
        dgvTransactions.ClearSelection();
        dgvTransactions.Rows[hit.RowIndex].Selected = true;
        contextMenuStrip.Show(dgvTransactions, e.Location);
    }

    private void dgvTransactions_MouseDown(object sender, MouseEventArgs e)
    {
        ShowContextMenu(e);
    }

    private void tsmiView_Click(object sender, EventArgs e) { _ViewDetails(); }
}
