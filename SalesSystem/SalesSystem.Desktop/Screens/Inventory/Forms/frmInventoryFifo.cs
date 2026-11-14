using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// عرض طبقات المخزون (FIFO/FEFO) لصنف معيّن — شاشة عرض للقراءة فقط.
/// الهيكل (RULE-N79 / RULE-N135…N138): العنوان ← بيانات الصنف ← الطبقات ← الإجماليات/شريط الأسفل.
/// </summary>
public partial class frmInventoryFifo : Form
{
    private static readonly Font _boldGridFont = new("Tahoma", 10F, FontStyle.Bold);

    private readonly IServiceProvider _serviceProvider;
    private DataTable _dtLayers = new();
    private DataView _dvLayers = new();

    /// <summary>نص شريط العنوان الأساسي — تُضاف إليه حالة العرض (RULE-N44).</summary>
    private const string FormTitle = "طبقات الصرف FIFO / FEFO";

    public frmInventoryFifo(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…RULE-N46 — تنقل Enter/الأسهم + تحديد القيمة + تظليل الحقل النشط.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للزر عند تعطيله.
        KeyboardHelper.WireButtonDisabledVisual(btnClose);

        // RULE-N47 — Esc يغلق الشاشة (لا توجد أوامر أخرى على شاشة عرض).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.Escape, () => { if (btnClose.Enabled) btnClose.PerformClick(); }));

        ApplyUiTheme();
    }

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

    private DataGridViewColumn _MakeColumn(string name, string header, int fill, int min, string? format = null)
    {
        var col = new DataGridViewTextBoxColumn
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
        if (format is not null)
            col.DefaultCellStyle.Format = format;
        return col;
    }

    private void _SetupGridColumns()
    {
        dgvLayers.Columns.Clear();

        dgvLayers.Columns.Add(_MakeColumn("LayerNo", "الطبقة #", 6, 60));
        dgvLayers.Columns.Add(_MakeColumn("BatchNo", "رقم التشغيلة", 13, 100));
        dgvLayers.Columns.Add(_MakeColumn("ReceiveDate", "تاريخ الاستلام", 12, 80));
        dgvLayers.Columns.Add(_MakeColumn("ExpiryDate", "تاريخ الصلاحية", 12, 80));
        dgvLayers.Columns.Add(_MakeColumn("RemainingQty", "الكمية المتبقية", 11, 80, "N3"));
        dgvLayers.Columns.Add(_MakeColumn("UnitCost", "تكلفة الوحدة", 10, 70, "N2"));
        dgvLayers.Columns.Add(_MakeColumn("TotalCost", "إجمالي التكلفة", 11, 80, "N2"));
        dgvLayers.Columns.Add(_MakeColumn("DispatchPriority", "أولوية الصرف (1 = الأقدم)", 9, 80));
    }

    // ── Mock Data (UI shape — no backend) ──────────────────────────────────────────

    private void _LoadMockLayers()
    {
        _dtLayers = new DataTable();
        _dtLayers.Columns.Add("LayerNo", typeof(int));
        _dtLayers.Columns.Add("BatchNo", typeof(string));
        _dtLayers.Columns.Add("ReceiveDate", typeof(string));
        _dtLayers.Columns.Add("ExpiryDate", typeof(string));
        _dtLayers.Columns.Add("RemainingQty", typeof(decimal));
        _dtLayers.Columns.Add("UnitCost", typeof(decimal));
        _dtLayers.Columns.Add("TotalCost", typeof(decimal));
        _dtLayers.Columns.Add("DispatchPriority", typeof(int));
        _dtLayers.Columns.Add("IsNext", typeof(bool));   // الطبقة التالية للصرف
        _dtLayers.Columns.Add("IsTotal", typeof(bool));  // صف الإجمالي

        // 5 طبقات مرتبة من الأقدم إلى الأحدث (أساس ترتيب الصرف FIFO)
        _dtLayers.Rows.Add(1, "B-2026-001", "2026-01-05", "2027-01-05", 120m, 4.50m, 540.00m, 1, true, false);
        _dtLayers.Rows.Add(2, "B-2026-002", "2026-02-10", "2027-02-10", 80m, 4.75m, 380.00m, 2, false, false);
        _dtLayers.Rows.Add(3, "B-2026-003", "2026-03-15", "2026-12-30", 60m, 4.60m, 276.00m, 3, false, false);
        _dtLayers.Rows.Add(4, "B-2026-004", "2026-04-20", "2027-04-20", 40m, 5.00m, 200.00m, 4, false, false);
        _dtLayers.Rows.Add(5, "B-2026-005", "2026-05-25", "2027-05-25", 25m, 5.25m, 131.25m, 5, false, false);

        // صف الإجمالي: 325.000 وحدة — إجمالي التكلفة 1,527.25
        _dtLayers.Rows.Add(DBNull.Value, "", "", "", 325m, DBNull.Value, 1527.25m, DBNull.Value, false, true);

        _dvLayers = new DataView(_dtLayers);
        dgvLayers.DataSource = _dvLayers;
    }

    private void _ApplyTotals()
    {
        decimal totalQty = _dtLayers.AsEnumerable()
            .Where(r => !r.Field<bool>("IsTotal"))
            .Sum(r => r.Field<decimal>("RemainingQty"));

        decimal totalCost = _dtLayers.AsEnumerable()
            .Where(r => !r.Field<bool>("IsTotal"))
            .Sum(r => r.Field<decimal>("TotalCost"));

        lblTotalQtyValue.Text = clsFormat.Quantity(totalQty);
        lblTotalCostValue.Text = clsFormat.Money(totalCost);
    }

    /// <summary>سطر «لا توجد بيانات» — يظهر فقط حين لا تكون هناك طبقات (RULE-N22).</summary>
    private void _UpdateEmptyState()
    {
        int layerRows = _dtLayers.Rows.Count - 1; // بلا صف الإجمالي
        bool empty = layerRows <= 0;

        lblEmptyState.Visible = empty;
        if (empty) lblEmptyState.BringToFront();

        lblMode.Text = $"{FormTitle} — (عدد الطبقات: {Math.Max(0, layerRows)})";
    }

    // ── Form Events ────────────────────────────────────────────────────────────────

    private void frmInventoryFifo_Load(object? sender, EventArgs e)
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            _SetupGridColumns();
            _LoadMockLayers();
            _ApplyTotals();
            _UpdateEmptyState();

            // تحديد الطبقة التالية للصرف (الأقدم) عند فتح النموذج
            if (dgvLayers.Rows.Count > 0)
            {
                dgvLayers.ClearSelection();
                dgvLayers.Rows[0].Selected = true;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل طبقات FIFO/FEFO");
            MessageBox.Show("حدث خطأ في تحميل بيانات الطبقات. يرجى المحاولة مرة أخرى.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
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
            MessageBox.Show("السطر المحدد لا يحتوي رقم تشغيلة.", "تنبيه",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            Clipboard.SetText(batchNo);
            MessageBox.Show($"تم نسخ رقم التشغيلة: {batchNo}", "تم",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر نسخ رقم التشغيلة");
            MessageBox.Show("تعذر نسخ رقم التشغيلة. يرجى المحاولة مرة أخرى.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void miClose_Click(object? sender, EventArgs e)
        => Close();

    private void btnClose_Click(object? sender, EventArgs e)
        => Close();
}
