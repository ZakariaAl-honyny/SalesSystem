using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// UI SHAPE — "محضر جرد فعلي" (New Stocktaking Session) dialog.
/// Mock data only (in-memory). No backend / API calls yet.
/// Demonstrates: header (رقم/تاريخ/مستودع), counting committee row,
/// editable count grid with auto-computed difference (الفرق),
/// surplus/deficit/item-count totals, and draft/post/cancel actions.
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

    private static readonly Color SurplusGreen = Color.FromArgb(25, 135, 84);   // فائض +
    private static readonly Color DeficitRed = Color.FromArgb(220, 53, 69);     // عجز -

    public frmStocktakingNew(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        dgvLines.DataError += (s, e) => e.ThrowException = false;
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

        lblSurplusValue.Text = surplus.ToString("N3");
        lblDeficitValue.Text = deficit.ToString("N3");
        lblItemCountValue.Text = counted.ToString();
    }

    // ═════════════════════════════════════════════════════════════════
    //  FOOTER ACTIONS — save draft / post / cancel (mock only)
    // ═════════════════════════════════════════════════════════════════

    private void btnSaveDraft_Click(object? sender, EventArgs e)
    {
        if (!_ValidateHeader()) return;

        MessageBox.Show("تم حفظ المحضر كمسودة (وضع تجريبي — لا يتم حفظ فعلي).",
            "حفظ المسودة", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnPost_Click(object? sender, EventArgs e)
    {
        if (!_ValidateHeader()) return;
        if (!_ValidateCountedRows()) return;

        if (MessageBox.Show("هل أنت متأكد من اعتماد محضر الجرد الفعلي؟",
                "تأكيد الاعتماد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        MessageBox.Show("تم اعتماد محضر الجرد بنجاح (وضع تجريبي — لا ترحيل فعلي للمخزون).",
            "اعتماد الجرد", MessageBoxButtons.OK, MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        Close();
    }

    // ═════════════════════════════════════════════════════════════════
    //  VALIDATION
    // ═════════════════════════════════════════════════════════════════

    private bool _ValidateHeader()
    {
        if (cmbWarehouse.SelectedIndex < 0)
        {
            MessageBox.Show("يرجى اختيار المستودع.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cmbWarehouse.Focus();
            return false;
        }
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
                MessageBox.Show("الكمية الفعلية لا يمكن أن تكون سالبة.", "بيانات غير صحيحة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvLines.CurrentCell = row.Cells[colActualQty.Index];
                return false;
            }
            counted++;
        }

        if (counted == 0)
        {
            MessageBox.Show("يجب إدخال الكمية الفعلية لصنف واحد على الأقل قبل الاعتماد.",
                "بيانات غير مكتملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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