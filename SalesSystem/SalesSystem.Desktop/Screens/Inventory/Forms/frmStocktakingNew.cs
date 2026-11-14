using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// «محضر جرد فعلي» — شاشة إدخال كميات الجرد الفعلية.
/// الهيكل (RULE-N79 / RULE-N135…N138): العنوان ← أوامر المحضر ← بيانات المحضر ← لجنة الجرد ← الأصناف ← ملخص الجرد.
/// سلوك إدخال الكميات وحساب الفروق بقي كما هو حرفياً.
/// </summary>
public partial class frmStocktakingNew : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── Mock data (in-memory, Arabic) ─────────────────────────────────────────────
    private readonly string[] _warehouses = { "المستودع الرئيسي", "مخزن التجزئة", "مخزن الجملة" };
    private readonly string[] _employees = { "أحمد محمد", "خالد عبدالله", "سارة علي", "منى حسن", "عمر يوسف" };

    private sealed record MockProduct(string Name, string Barcode, decimal BookQty);

    private static readonly MockProduct[] _mockProducts =
    {
        new("سكر أبيض 1 كجم",        "6254612001234", 120.500m),
        new("أرز بسمتي 5 كجم",       "6254612002231",  45.000m),
        new("زيت دوار الشمس 1 لتر",  "6254612003245", 210.000m),
        new("شاي أحمر 500 جم",       "6254612004252",  85.250m),
        new("دقيق فاخر 1 كجم",       "6254612005260",  60.750m),
        new("مكرونة اسباجتي 400 جم", "6254612006271", 140.000m),
        new("صلصة طماطم 340 جم",     "6254612007282",  95.500m),
        new("جبنة بيضاء 500 جم",     "6254612008293",  30.000m),
    };

    private static Color SurplusGreen => GreenThemeHelper.StatusSuccess;   // فائض +
    private static Color DeficitRed => GreenThemeHelper.StatusDanger;      // عجز -

    /// <summary>نص شريط العنوان الأساسي — تُضاف إليه حالة المحضر (RULE-N44).</summary>
    private const string FormTitle = "محضر جرد فعلي";

    public frmStocktakingNew(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…RULE-N46 — تنقل Enter/الأسهم + تحديد القيمة + تظليل الحقل النشط.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnSaveDraft, btnPost, btnCancel);

        // RULE-N47 — الاختصارات الموجودة فعلاً (F4 = اعتماد الجرد؛ Esc يغلقه CancelButton).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F4, () => btnPost.PerformClick()));

        // RULE-N147 — Enter = الزر الأساسي للوضع (اعتماد الجرد)، ومحجوب داخل شبكة العدّ.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // خطأ تحرير خلية لا يُسقط الشاشة.
        dgvLines.DataError += (s, e) => e.ThrowException = false;

        ApplyUiTheme();
    }

    // ── الثيم (RULE-N50…RULE-N54) ─────────────────────────────────────

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply(this)</c> آخر سطر دائماً.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvLines);

        lblSurplusLabel.ForeColor = GreenThemeHelper.TextStrong;
        lblDeficitLabel.ForeColor = GreenThemeHelper.TextStrong;
        lblItemCountLabel.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ═════════════════════════════════════════════════════════════════
    //  LOAD — populate mock lookups + seed the count grid
    // ═════════════════════════════════════════════════════════════════

    private void frmStocktakingNew_Load(object sender, EventArgs e)
    {
        cmbWarehouse.Items.AddRange(_warehouses);
        cmbWarehouse.SelectedIndex = 0;

        cmbCommitteeHead.Items.AddRange(_employees);
        cmbCommitteeMember.Items.AddRange(_employees);
        cmbStorekeeper.Items.AddRange(_employees);

        dtpDate.Value = DateTime.Now;
        txtCountNo.Text = "جديد";

        foreach (var p in _mockProducts)
        {
            int rowIndex = dgvLines.Rows.Add();
            dgvLines.Rows[rowIndex].Cells[colProduct.Index].Value = p.Name;
            dgvLines.Rows[rowIndex].Cells[colBarcode.Index].Value = p.Barcode;
            dgvLines.Rows[rowIndex].Cells[colBookQty.Index].Value = p.BookQty;
            dgvLines.Rows[rowIndex].Cells[colActualQty.Index].Value = null;
            dgvLines.Rows[rowIndex].Cells[colDifference.Index].Value = null;
        }

        _RecalculateTotals();
    }

    // ═════════════════════════════════════════════════════════════════
    //  GRID — difference auto-compute + totals
    // ═════════════════════════════════════════════════════════════════

    private void dgvLines_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvLines.Rows.Count) return;
        if (e.ColumnIndex != colActualQty.Index) return;

        _UpdateRowDifference(e.RowIndex);
        _RecalculateTotals();
    }

    private void dgvLines_UserDeletedRow(object? sender, DataGridViewRowEventArgs e)
    {
        _RecalculateTotals();
    }

    /// <summary>
    /// الفرق = الكمية الفعلية − الكمية الدفترية — فائض (أخضر) / عجز (أحمر).
    /// </summary>
    private void _UpdateRowDifference(int rowIndex)
    {
        var row = dgvLines.Rows[rowIndex];
        if (row.IsNewRow) return;

        var actualCell = row.Cells[colActualQty.Index];
        decimal? actual = _ToNullableDecimal(actualCell.Value);

        // Invalid text typed by the user → reset the cell to "uncounted"
        if (actual is null && actualCell.Value is string s && !string.IsNullOrWhiteSpace(s))
        {
            actualCell.Value = null;
        }

        decimal book = _ToDecimal(row.Cells[colBookQty.Index].Value);

        var diffCell = row.Cells[colDifference.Index];
        if (actual.HasValue)
        {
            decimal diff = actual.Value - book;
            diffCell.Value = diff;
            diffCell.Style.ForeColor = diff > 0 ? SurplusGreen : diff < 0 ? DeficitRed : Color.DimGray;
        }
        else
        {
            diffCell.Value = null;
            diffCell.Style.ForeColor = Color.DimGray;
        }
    }

    /// <summary>
    /// Totals footer: إجمالي الفائض (أخضر) + إجمالي العجز (أحمر) + عدد الأصناف.
    /// Only rows with an entered physical quantity are counted.
    /// </summary>
    private void _RecalculateTotals()
    {
        decimal surplus = 0m, deficit = 0m;
        int counted = 0;

        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.IsNewRow) continue;

            decimal? actual = _ToNullableDecimal(row.Cells[colActualQty.Index].Value);
            if (!actual.HasValue) continue;

            counted++;
            decimal book = _ToDecimal(row.Cells[colBookQty.Index].Value);
            decimal diff = actual.Value - book;
            if (diff > 0) surplus += diff;
            else if (diff < 0) deficit += Math.Abs(diff);
        }

        lblSurplusValue.Text = clsFormat.Quantity(surplus);
        lblDeficitValue.Text = clsFormat.Quantity(deficit);
        lblItemCountValue.Text = clsFormat.Number(counted);

        _UpdateModeHeader();
    }

    /// <summary>مؤشر الوضع (RULE-N44): محضر جديد + عدد الأصناف المعدودة.</summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = $"{FormTitle} — جرد جديد (عدد الأصناف المعدودة: {lblItemCountValue.Text})";

        // RULE-N147 — مفتاح Enter الافتراضي يتبع الوضع، ويتحرّر تلقائياً عندما تكون شبكة العدّ نشطة.
        AcceptButton = _PrimaryActionButton();
    }

    // ── RULE-N147 — Enter = الزر الأساسي للوضع ────────────────────────

    /// <summary>
    /// الزر الأساسي لضغطة <c>Enter</c> حسب وضع الشاشة (RULE-N147).
    /// يُرجع <c>null</c> عندما تكون شبكة العدّ في التركيز أو خليةٌ قيد التحرير بداخلها،
    /// لأن <c>Enter</c> داخل شبكة الجرد = تثبيت الخلية والانتقال للتالي — لا اعتماد المحضر
    /// (حماية سلامة الجرد — §2.7).
    /// </summary>
    private Button? _PrimaryActionButton()
        => _IsCountGridActive() ? null : btnPost;

    /// <summary>شبكة العدّ تحمل التركيز (أو خلية قيد التحرير داخلها).</summary>
    private bool _IsCountGridActive()
        => dgvLines is { IsDisposed: false } && (dgvLines.Focused || dgvLines.ContainsFocus);

    // ═════════════════════════════════════════════════════════════════
    //  FOOTER ACTIONS — save draft / post / cancel (mock only)
    // ═════════════════════════════════════════════════════════════════

    private void btnSaveDraft_Click(object? sender, EventArgs e)
    {
        if (!_ValidateHeader()) return;

        clsDialogs.Success("تم حفظ المحضر كمسودة (وضع تجريبي — لا يتم حفظ فعلي).");
    }

    private void btnPost_Click(object? sender, EventArgs e)
    {
        if (!_ValidateHeader()) return;
        if (!_ValidateCountedRows()) return;

        if (!clsDialogs.Confirm("هل أنت متأكد من اعتماد محضر الجرد الفعلي؟"))
            return;

        clsDialogs.Success("تم اعتماد محضر الجرد بنجاح (وضع تجريبي — لا ترحيل فعلي للمخزون).");

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        Close();
    }

    // ── Context menu (RULE-N23) ───────────────────────────────────────

    private void miPost_Click(object? sender, EventArgs e)
        => btnPost.PerformClick();

    private void miDeleteRow_Click(object? sender, EventArgs e)
    {
        if (dgvLines.CurrentRow is not { IsNewRow: false } row) return;

        dgvLines.Rows.Remove(row);
        _RecalculateTotals();
    }

    // ═════════════════════════════════════════════════════════════════
    //  VALIDATION
    // ═════════════════════════════════════════════════════════════════

    private bool _ValidateHeader()
    {
        if (cmbWarehouse.SelectedIndex < 0)
        {
            errorProvider.SetError(cmbWarehouse, "يرجى اختيار المستودع");
            clsInputFeedback.ShowRejectTooltip(cmbWarehouse, "يرجى اختيار المستودع", tooltip);
            clsDialogs.Warning("يرجى اختيار المستودع.");
            cmbWarehouse.Focus();
            return false;
        }

        errorProvider.SetError(cmbWarehouse, "");
        return true;
    }

    private bool _ValidateCountedRows()
    {
        int counted = 0;

        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.IsNewRow) continue;

            decimal? actual = _ToNullableDecimal(row.Cells[colActualQty.Index].Value);
            if (!actual.HasValue) continue;

            if (actual.Value < 0)
            {
                clsDialogs.Warning("الكمية الفعلية لا يمكن أن تكون سالبة.");
                dgvLines.CurrentCell = row.Cells[colActualQty.Index];
                return false;
            }
            counted++;
        }

        if (counted == 0)
        {
            clsDialogs.Warning("يجب إدخال الكمية الفعلية لصنف واحد على الأقل قبل الاعتماد.");
            return false;
        }
        return true;
    }

    // ── Cell value helpers ─────────────────────────────────────────────

    private static decimal _ToDecimal(object? value)
        => value == null ? 0m : Convert.ToDecimal(value);

    private static decimal? _ToNullableDecimal(object? value)
    {
        if (value == null) return null;
        if (value is decimal dec) return dec;
        if (value is string s && string.IsNullOrWhiteSpace(s)) return null;

        // Current culture first (e.g. 12.5 / 12,5), invariant as a fallback
        if (decimal.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.CurrentCulture, out var v) ||
            decimal.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out v))
            return v;

        return null;
    }
}
