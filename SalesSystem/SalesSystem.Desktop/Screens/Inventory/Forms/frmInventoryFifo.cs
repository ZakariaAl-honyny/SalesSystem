using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Products.Forms;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Responses;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// عرض طبقات المخزون (FIFO/FEFO) لصنف معيّن — شاشة عرض للقراءة فقط.
/// الهيكل (RULE-N79 / RULE-N135…N138): العنوان ← بيانات الصنف ← الطبقات ← الإجماليات/شريط الأسفل.
/// <para>
/// <b>مصدر البيانات:</b> <see cref="IInventoryBatchDesktopService.GetByProductAsync"/> وحده —
/// تُعرض الطبقات كما أعادتها الخدمة (بلا صفوف مُختلقة داخل الشاشة)، ولا يُحسب أي رقم محلياً
/// سوى مجموع الصفوف المعروضة نفسها في شريط الإجماليات.
/// </para>
/// <para>
/// <b>اختيار الصنف:</b> عبر منتقي الأصناف القائم <see cref="frmProductSelection"/> (لا عنصر جديد يُبنى
/// وقت التشغيل — RULE-N145): يُفتح بـ <c>F2</c> أو بالنقر المزدوج على الشبكة، وعند الإلغاء تبقى
/// الشبكة فارغة بسطر «لا توجد بيانات» (لا تُختلق صفوف).
/// </para>
/// <para>
/// <b>الوضع الحقيقي:</b> العقد <see cref="IInventoryBatchDesktopService"/> مُسجَّل في فرعي
/// Mock وDirect معاً، فلا حاجة إلى حارس وضع، وتُقرأ الطبقات في الوضعين بنفس المسار.
/// </para>
/// </summary>
public partial class frmInventoryFifo : Form
{
    private static readonly Font _boldGridFont = new("Tahoma", 10F, FontStyle.Bold);

    private readonly IServiceProvider _serviceProvider;
    private DataTable _dtLayers = new();
    private DataView _dvLayers = new();

    /// <summary>الصنف المعروض حالياً — <c>null</c> حتى يختار المستخدم صنفاً من المنتقي.</summary>
    private ProductDto? _product;

    /// <summary>طبقات الصنف كما أعادتها الخدمة (مرجع إعادة التحميل).</summary>
    private IReadOnlyList<InventoryBatchDto> _layers = Array.Empty<InventoryBatchDto>();

    /// <summary>نص سطر «لا توجد بيانات» عندما لا يكون هناك صنف محدد.</summary>
    private const string NoProductEmptyText = "لم يتم اختيار صنف بعد — اضغط F2 لاختيار صنف";

    /// <summary>نص سطر «لا توجد بيانات» عندما يكون الصنف محدداً بلا طبقات مخزون.</summary>
    private const string NoLayersEmptyText = "لا توجد طبقات مخزون لهذا الصنف";

    /// <summary>نص شريط العنوان الأساسي — تُضاف إليه حالة العرض (RULE-N44).</summary>
    private const string FormTitle = "طبقات الصرف FIFO / FEFO";

    public frmInventoryFifo(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…RULE-N46 — تنقل Enter/الأسهم + تحديد القيمة + تظليل الحقل النشط.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للزر عند تعطيله.
        KeyboardHelper.WireButtonDisabledVisual(btnClose);

        // RULE-N47 — F2 = اختيار الصنف (نفس اختصار شاشات المخزون الأخرى) · Esc يغلق الشاشة.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => _ChooseProductAsync()),
            (Keys.Escape, () => { if (btnClose.Enabled) btnClose.PerformClick(); }));

        // تغيير الصنف بالنقر المزدوج على الشبكة — لا عنصر جديد (RULE-N145).
        dgvLayers.CellDoubleClick += dgvLayers_CellDoubleClick;
        dgvLayers.DataError += (_, e) => e.ThrowException = false;

        tooltip.SetToolTip(dgvLayers, "الطبقة الأولى هي التالية للصرف — F2 أو نقر مزدوج لتغيير الصنف");

        ApplyUiTheme();
    }

    // ── عقود الطبقة المكتبية (RULE-N04/N05) ─────────────────────────────

    /// <summary>عقد دفعات المخزون — مصدر طبقات الصرف الوحيد لهذه الشاشة.</summary>
    private IInventoryBatchDesktopService BatchService
        => _serviceProvider.GetRequiredService<IInventoryBatchDesktopService>();

    // ── الثيم (RULE-N50…RULE-N54) ─────────────────────────────────────

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply(this)</c> آخر سطر دائماً.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvLayers);

        lblTotalQtyCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblTotalCostCaption.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
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
            // الحفاظ على ترتيب الطبقات (الأقدم أولاً ثم الإجمالي) — يمنع إعادة الترتيب بالنقر على رأس العمود
            SortMode = DataGridViewColumnSortMode.NotSortable,
        };
    }

    private void _SetupGridColumns()
    {
        // الأعمدة تُبنى صريحاً ومطابقة لأعمدة الجدول — لا توليد تلقائي لأعمدة مساعدة (IsNext/IsTotal).
        dgvLayers.AutoGenerateColumns = false;
        dgvLayers.Columns.Clear();

        dgvLayers.Columns.Add(_MakeColumn("LayerNo", "الطبقة #", 5, 55));
        dgvLayers.Columns.Add(_MakeColumn("BatchNo", "رقم التشغيلة", 12, 95));
        dgvLayers.Columns.Add(_MakeColumn("Warehouse", "المستودع", 11, 80));
        dgvLayers.Columns.Add(_MakeColumn("ExpiryDate", "تاريخ الصلاحية", 10, 80));
        dgvLayers.Columns.Add(_MakeColumn("ExpiryStatus", "حالة الصلاحية", 8, 70));
        dgvLayers.Columns.Add(_MakeColumn("RemainingQty", "الكمية المتبقية", 10, 80));
        dgvLayers.Columns.Add(_MakeColumn("UnitCost", "تكلفة الوحدة", 9, 70));
        dgvLayers.Columns.Add(_MakeColumn("TotalCost", "إجمالي التكلفة", 10, 80));
        dgvLayers.Columns.Add(_MakeColumn("DispatchPriority", "أولوية الصرف (1 = الأقدم)", 10, 80));

        // RULE-N179 — تنسيق الأعمدة من البيت الواحد (RULE-N169): كمية 3 / مبلغ 2 حسب إعدادات التنسيق الحيّة.
        GridFormatHelper.ApplyColumn(dgvLayers, "RemainingQty", GridFormatKind.Quantity);
        GridFormatHelper.ApplyColumns(dgvLayers, GridFormatKind.Money, "UnitCost", "TotalCost");
    }

    // ── الطبقات: من الخدمة فقط (لا صفوف مُختلقة) ──────────────────────────────────

    /// <summary>يبني مخطط جدول الطبقات — أعمدة منطقية مطابقة لأعمدة الشبكة وأعمدة <see cref="InventoryBatchDto"/>.</summary>
    private static DataTable _CreateLayerTable()
    {
        var table = new DataTable();
        table.Columns.Add("LayerNo", typeof(int));
        table.Columns.Add("BatchNo", typeof(string));
        table.Columns.Add("Warehouse", typeof(string));
        table.Columns.Add("ExpiryDate", typeof(string));
        table.Columns.Add("ExpiryStatus", typeof(string));
        table.Columns.Add("RemainingQty", typeof(decimal));
        table.Columns.Add("UnitCost", typeof(decimal));
        table.Columns.Add("TotalCost", typeof(decimal));
        table.Columns.Add("DispatchPriority", typeof(int));
        table.Columns.Add("IsNext", typeof(bool));   // الطبقة التالية للصرف
        table.Columns.Add("IsTotal", typeof(bool));  // صف الإجمالي
        return table;
    }

    /// <summary>يُفرِّغ الشبكة ويُصفّر الإجماليات — الحالة الافتراضية قبل اختيار صنف.</summary>
    private void _ResetLayers()
    {
        _layers = Array.Empty<InventoryBatchDto>();
        _dtLayers = _CreateLayerTable();
        _dvLayers = new DataView(_dtLayers);
        dgvLayers.DataSource = _dvLayers;

        lblTotalQtyValue.Text = clsFormat.Quantity(0m);
        lblTotalCostValue.Text = clsFormat.Money(0m);
    }

    /// <summary>يملأ الشبكة من طبقات الخدمة (الترتيب بتسلسل تسجيل الدفعات = الأقدم أولاً) + صف الإجمالي.</summary>
    private void _FillLayers(IReadOnlyList<InventoryBatchDto> layers)
    {
        _dtLayers = _CreateLayerTable();

        // الطبقة التالية للصرف = أقدم دفعة لم تُستهلك بالكامل (لا يُختلق إن لم توجد).
        int? nextLayerId = layers.FirstOrDefault(b => !b.IsFullyConsumed)?.Id;

        int priority = 1;
        foreach (var batch in layers)
        {
            _dtLayers.Rows.Add(
                priority,
                batch.BatchNo,
                string.IsNullOrWhiteSpace(batch.WarehouseName) ? "—" : batch.WarehouseName,
                batch.ExpiryDate is DateOnly expiry
                    ? clsFormat.DateToShort(expiry.ToDateTime(TimeOnly.MinValue))
                    : "—",
                batch.ExpiryStatus,
                batch.QuantityRemaining,
                batch.UnitCost,
                batch.TotalValue,
                priority,
                batch.Id == nextLayerId,
                false);

            priority++;
        }

        if (layers.Count > 0)
        {
            // صف الإجمالي محسوب من الصفوف المعروضة نفسها (لا من رقم ثابت).
            _dtLayers.Rows.Add(
                DBNull.Value, "", "", "", "",
                layers.Sum(b => b.QuantityRemaining),
                DBNull.Value,
                layers.Sum(b => b.TotalValue),
                DBNull.Value, false, true);
        }

        _dvLayers = new DataView(_dtLayers);
        dgvLayers.DataSource = _dvLayers;
    }

    private void _ApplyTotals()
    {
        decimal totalQty = _layers.Sum(b => b.QuantityRemaining);
        decimal totalCost = _layers.Sum(b => b.TotalValue);

        lblTotalQtyValue.Text = clsFormat.Quantity(totalQty);
        lblTotalCostValue.Text = clsFormat.Money(totalCost);
    }

    /// <summary>سطر «لا توجد بيانات» وشريط الوضع (RULE-N22 / RULE-N44).</summary>
    private void _UpdateEmptyState()
    {
        bool empty = _layers.Count == 0;

        lblEmptyState.Text = _product is null ? NoProductEmptyText : NoLayersEmptyText;
        lblEmptyState.Visible = empty;
        if (empty) lblEmptyState.BringToFront();

        lblMode.Text = _product is null
            ? $"{FormTitle} — لم يتم اختيار صنف"
            : $"{FormTitle} — {_product.Name} (عدد الطبقات: {_layers.Count})";
    }

    /// <summary>بيانات الصنف المعروض كما وردت من دليل الأصناف + المستودعات الموجودة فعلاً في الطبقات.</summary>
    private void _UpdateProductInfo()
    {
        if (_product is null)
        {
            lblProduct.Text = "الصنف: —";
            lblWarehouse.Text = "المستودع: —";
            lblNote.Text = "ملاحظة: اضغط F2 (أو انقر مزدوجاً على الشبكة) لاختيار الصنف المراد عرض طبقاته";
            return;
        }

        lblProduct.Text = $"الصنف: {_product.Name}";

        var warehouses = _layers
            .Select(b => b.WarehouseName)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Distinct()
            .ToList();

        lblWarehouse.Text = warehouses.Count switch
        {
            0 => "المستودع: —",
            1 => $"المستودع: {warehouses[0]}",
            _ => $"المستودع: أكثر من مستودع ({warehouses.Count})",
        };

        lblNote.Text = "ملاحظة: الطبقة الأولى هي التالية للصرف — الصفوف كما أعادتها خدمة دفعات المخزون (F2 لتغيير الصنف)";
    }

    // ── اختيار الصنف + التحميل ─────────────────────────────────────────────────────

    /// <summary>
    /// يفتح منتقي الأصناف القائم ويحمّل طبقات الصنف المختار. الإلغاء يترك الشبكة فارغة
    /// (سطر «لا توجد بيانات») — لا تُختلق صفوف. يبتلع أخطاءه بنفسه لأنه يُستدعى من اختصار وحدث.
    /// </summary>
    private async Task _ChooseProductAsync()
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            using var dialog = _serviceProvider.GetRequiredService<frmProductSelection>();
            if (dialog.ShowDialog(this) != DialogResult.OK || dialog.SelectedProduct is not ProductDto product)
            {
                _UpdateEmptyState();
                return;
            }

            await _LoadLayersAsync(product);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر فتح منتقي الأصناف في شاشة طبقات FIFO/FEFO");
            clsDialogs.Error("تعذر فتح قائمة الأصناف. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>يقرأ طبقات صنف من <see cref="IInventoryBatchDesktopService.GetByProductAsync"/> ويملأ الشبكة.</summary>
    private async Task _LoadLayersAsync(ProductDto product)
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            _product = product;

            var result = await BatchService.GetByProductAsync(product.Id);
            if (!result.IsSuccess)
            {
                Log.Warning("تعذر تحميل طبقات الصنف {ProductId}: {Error}", product.Id, result.Error);

                _layers = Array.Empty<InventoryBatchDto>();
                _ResetLayers();
                _UpdateProductInfo();
                _UpdateEmptyState();

                clsDialogs.Error(result.Error ?? "تعذّر تحميل طبقات المخزون لهذا الصنف.");
                return;
            }

            // الترتيب: تسلسل تسجيل الدفعات (الأقدم أولاً) — لا يُضاف أي سطر من الشاشة.
            _layers = (result.Value ?? Array.Empty<InventoryBatchDto>())
                .OrderBy(b => b.Id)
                .ToList();

            _FillLayers(_layers);
            _ApplyTotals();
            _UpdateProductInfo();
            _UpdateEmptyState();

            // تحديد الطبقة التالية للصرف إن وُجدت
            if (dgvLayers.Rows.Count > 0)
            {
                dgvLayers.ClearSelection();

                var nextRow = dgvLayers.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => r.DataBoundItem is DataRowView drv
                                         && drv.Row.Field<bool>("IsNext"));

                (nextRow ?? dgvLayers.Rows[0]).Selected = true;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل طبقات FIFO/FEFO للصنف {ProductId}", product.Id);
            clsDialogs.Error("حدث خطأ في تحميل بيانات الطبقات. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Form Events ────────────────────────────────────────────────────────────────

    private void frmInventoryFifo_Load(object? sender, EventArgs e)
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            _SetupGridColumns();
            _ResetLayers();
            _UpdateProductInfo();
            _UpdateEmptyState();

            // لا صفوف مُختلقة: تُقرأ الطبقات من الخدمة بعد اختيار الصنف.
            // يُؤجَّل نداء المنتقي حتى يكتمل عرض النموذج (ShowDialog داخل Load غير مستحسن).
            BeginInvoke((Action)(() => { _ = _ChooseProductAsync(); }));
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل طبقات FIFO/FEFO");
            clsDialogs.Error("حدث خطأ في تحميل بيانات الطبقات. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void dgvLayers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;   // النقر على رأس العمود لا يغيّر الصنف
        _ = _ChooseProductAsync();
    }

    private void dgvLayers_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
    {
        if (e.RowIndex < 0) return;

        if (dgvLayers.Rows[e.RowIndex].DataBoundItem is not DataRowView drv)
            return;

        if (drv.Row.Field<bool>("IsTotal"))
        {
            // صف الإجمالي: خلفية الشريط وخط عريض
            dgvLayers.Rows[e.RowIndex].DefaultCellStyle.BackColor = GreenThemeHelper.BarBackground;
            dgvLayers.Rows[e.RowIndex].DefaultCellStyle.ForeColor = GreenThemeHelper.TextStrong;
            dgvLayers.Rows[e.RowIndex].DefaultCellStyle.Font = _boldGridFont;
        }
        else if (drv.Row.Field<bool>("IsNext"))
        {
            // الطبقة التالية للصرف: تمييز بخلفية الشبكة والنص المميّز
            dgvLayers.Rows[e.RowIndex].DefaultCellStyle.BackColor = GreenThemeHelper.GridBackground;
            dgvLayers.Rows[e.RowIndex].DefaultCellStyle.ForeColor = GreenThemeHelper.TextStrong;
            dgvLayers.Rows[e.RowIndex].DefaultCellStyle.Font = _boldGridFont;
        }
    }

    private void dgvLayers_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

        if (dgvLayers.Rows[e.RowIndex].DataBoundItem is not DataRowView drv)
            return;

        if (!drv.Row.Field<bool>("IsTotal"))
            return;

        // عرض نص "الإجمالي" في خلية رقم الطبقة بصف الإجمالي
        if (e.ColumnIndex == dgvLayers.Columns["LayerNo"].Index)
        {
            e.Value = "الإجمالي";
            e.FormattingApplied = true;
        }
    }

    // ── Context menu (RULE-N23) ───────────────────────────────────────

    private void miCopyBatch_Click(object? sender, EventArgs e)
    {
        if (dgvLayers.CurrentRow?.DataBoundItem is not DataRowView drv) return;

        string batchNo = drv.Row.Field<string>("BatchNo") ?? string.Empty;
        if (string.IsNullOrWhiteSpace(batchNo))
        {
            clsDialogs.Warning("السطر المحدد لا يحتوي رقم تشغيلة.");
            return;
        }

        try
        {
            Clipboard.SetText(batchNo);
            clsDialogs.Success($"تم نسخ رقم التشغيلة: {batchNo}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر نسخ رقم التشغيلة");
            clsDialogs.Error("تعذر نسخ رقم التشغيلة. يرجى المحاولة مرة أخرى.");
        }
    }

    private void miClose_Click(object? sender, EventArgs e)
        => Close();

    private void btnClose_Click(object? sender, EventArgs e)
        => Close();
}
