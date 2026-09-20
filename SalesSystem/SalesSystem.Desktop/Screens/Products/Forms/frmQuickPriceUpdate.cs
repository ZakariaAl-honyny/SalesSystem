using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Products.Forms;

/// <summary>
/// UI SHAPE — "تحديث سريع للأسعار" (Quick Price Update) dialog.
/// Mock data only (in-memory). No backend / API calls yet.
/// Demonstrates: bulk price operations (زيادة نسبة ٪ / خصم نسبة ٪ / سعر ثابت)
/// applied to selected or all rows, an editable "السعر الجديد" grid column,
/// and an auto-computed هامش الربح (margin) per row.
/// </summary>
public partial class frmQuickPriceUpdate : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── Mock data (in-memory, Arabic) ─────────────────────────────────────────────

    private sealed record MockProduct(string Code, string Name, string Unit, decimal CurrentPrice, decimal Cost);

    private static readonly MockProduct[] _mockProducts =
    {
        new("P-1001", "سكر أبيض 1 كجم",        "كيس",   42.00m, 36.00m),
        new("P-1002", "أرز بسمتي 5 كجم",       "كيس",  145.00m, 120.00m),
        new("P-1003", "زيت دوار الشمس 1 لتر",  "عبوة",  78.00m,  65.00m),
        new("P-1004", "شاي أحمر 500 جم",       "علبة",  65.00m,  52.00m),
        new("P-1005", "دقيق فاخر 1 كجم",       "كيس",   22.50m,  17.00m),
        new("P-1006", "مكرونة اسباجتي 400 جم", "عبوة",  12.00m,   9.50m),
        new("P-1007", "صلصة طماطم 340 جم",     "علبة",  18.00m,  14.00m),
        new("P-1008", "جبنة بيضاء 500 جم",     "عبوة",  95.00m,  78.00m),
    };

    // ── Visual helpers ────────────────────────────────────────────────────────────

    private static readonly Color ChangedFore = Color.FromArgb(13, 110, 253);    // سعر معدّل (أزرق)
    private static readonly Color MarginPositive = Color.FromArgb(25, 135, 84);   // هامش موجب (أخضر)
    private static readonly Color MarginNegative = Color.FromArgb(220, 53, 69);   // هامش سالب (أحمر)
    private static readonly Font FontBold = new("Tahoma", 10F, FontStyle.Bold);
    private static readonly Font FontRegular = new("Tahoma", 10F, FontStyle.Regular);

    public frmQuickPriceUpdate(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        dgvPrices.DataError += (s, e) => e.ThrowException = false;
    }

    // ═════════════════════════════════════════════════════════════════
    //  LOAD — seed mock rows
    // ═════════════════════════════════════════════════════════════════

    private void frmQuickPriceUpdate_Load(object? sender, EventArgs e)
    {
        cmbOperation.SelectedIndex = 0;   // "زيادة نسبة ٪" افتراضياً
        numValue.Value = 5m;              // قيمة افتراضية 5٪

        foreach (var p in _mockProducts)
        {
            int rowIndex = dgvPrices.Rows.Add();
            dgvPrices.Rows[rowIndex].Cells[colCode.Index].Value = p.Code;
            dgvPrices.Rows[rowIndex].Cells[colName.Index].Value = p.Name;
            dgvPrices.Rows[rowIndex].Cells[colUnit.Index].Value = p.Unit;
            dgvPrices.Rows[rowIndex].Cells[colCurrentPrice.Index].Value = p.CurrentPrice;
            dgvPrices.Rows[rowIndex].Cells[colNewPrice.Index].Value = p.CurrentPrice; // السعر الجديد = الحالي افتراضياً
            dgvPrices.Rows[rowIndex].Cells[colCost.Index].Value = p.Cost;
            _RefreshRow(rowIndex);
        }

        _RefreshStatus();
    }

    // ═════════════════════════════════════════════════════════════════
    //  BULK OPERATION BAR
    // ═════════════════════════════════════════════════════════════════

    private void cmbOperation_SelectedIndexChanged(object? sender, EventArgs e)
    {
        bool isPercent = cmbOperation.SelectedIndex == 0 || cmbOperation.SelectedIndex == 1;
        lblValue.Text = isPercent ? "النسبة (٪):" : "السعر الثابت:";

        lblHint.Text = cmbOperation.SelectedIndex switch
        {
            0 => "زيادة سعر البيع بنسبة مئوية من السعر الحالي.",
            1 => "تخفيض سعر البيع بنسبة مئوية من السعر الحالي.",
            _ => "تطبيق نفس السعر الثابت على جميع الأصناف المحددة.",
        };
    }

    private void btnApplySelected_Click(object? sender, EventArgs e)
    {
        var indices = _GetSelectedRowIndices();
        if (indices.Count == 0)
        {
            MessageBox.Show("يرجى تحديد صف واحد على الأقل من الجدول قبل التطبيق.",
                "لا يوجد تحديد", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _ApplyToRows(indices);
    }

    private void btnApplyAll_Click(object? sender, EventArgs e)
    {
        var indices = new List<int>();
        foreach (DataGridViewRow row in dgvPrices.Rows)
        {
            if (!row.IsNewRow)
                indices.Add(row.Index);
        }

        _ApplyToRows(indices);
    }

    /// <summary>
    /// تطبق العملية الحالية (نسبة زيادة / نسبة خصم / سعر ثابت) على الصفوف المحددة.
    /// </summary>
    private void _ApplyToRows(List<int> rowIndices)
    {
        if (!_ValidateOperationValue()) return;

        int applied = 0;
        foreach (int rowIndex in rowIndices.Distinct())
        {
            if (rowIndex < 0 || rowIndex >= dgvPrices.Rows.Count) continue;
            var row = dgvPrices.Rows[rowIndex];
            if (row.IsNewRow) continue;

            decimal current = _ToDecimal(row.Cells[colCurrentPrice.Index].Value);
            decimal? newPrice = _ComputeNewPrice(current);
            if (!newPrice.HasValue) continue;

            row.Cells[colNewPrice.Index].Value = newPrice.Value;
            _RefreshRow(rowIndex);
            applied++;
        }

        _RefreshStatus();

        if (applied == 0)
        {
            MessageBox.Show("لم يتم تطبيق العملية على أي صف.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private decimal? _ComputeNewPrice(decimal currentPrice)
    {
        decimal value = numValue.Value;
        switch (cmbOperation.SelectedIndex)
        {
            case 0: // زيادة نسبة ٪
                return currentPrice * (1m + value / 100m);

            case 1: // خصم نسبة ٪ (لا ينزل السعر تحت الصفر)
                return Math.Max(0m, currentPrice * (1m - value / 100m));

            case 2: // تحديد سعر ثابت
                return value;

            default:
                return null;
        }
    }

    private bool _ValidateOperationValue()
    {
        if (numValue.Value <= 0)
        {
            bool isPercent = cmbOperation.SelectedIndex != 2;
            MessageBox.Show(isPercent
                    ? "يرجى إدخال نسبة أكبر من صفر."
                    : "يرجى إدخال سعر ثابت أكبر من صفر.",
                "قيمة غير صحيحة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            numValue.Focus();
            return false;
        }
        return true;
    }

    /// <summary>
    /// يجمع فهارس الصفوف المحددة (صفوف كاملة عند التحديد بالسحب أو Ctrl+نقرة).
    /// </summary>
    private List<int> _GetSelectedRowIndices()
    {
        var indices = new List<int>();

        if (dgvPrices.SelectedCells.Count == 0)
        {
            if (dgvPrices.CurrentRow != null && !dgvPrices.CurrentRow.IsNewRow)
                indices.Add(dgvPrices.CurrentRow.Index);
            return indices;
        }

        foreach (DataGridViewCell cell in dgvPrices.SelectedCells)
        {
            if (!indices.Contains(cell.RowIndex))
                indices.Add(cell.RowIndex);
        }

        return indices;
    }

    // ═════════════════════════════════════════════════════════════════
    //  GRID — manual price edit + auto-computed margin
    // ═════════════════════════════════════════════════════════════════

    private void dgvPrices_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvPrices.Rows.Count) return;
        if (e.ColumnIndex != colNewPrice.Index) return;

        var cell = dgvPrices.Rows[e.RowIndex].Cells[colNewPrice.Index];
        decimal? newPrice = _ToNullableDecimal(cell.Value);

        // نص غير صالح أدخله المستخدم → إعادة تعيين الخلية
        if (newPrice is null && cell.Value is string s && !string.IsNullOrWhiteSpace(s))
        {
            cell.Value = null;
        }

        _RefreshRow(e.RowIndex);
        _RefreshStatus();
    }

    /// <summary>
    /// هامش الربح = (السعر الجديد − التكلفة) / السعر الجديد — وتحديث تنسيق الخلية.
    /// </summary>
    private void _RefreshRow(int rowIndex)
    {
        var row = dgvPrices.Rows[rowIndex];
        if (row.IsNewRow) return;

        decimal? newPrice = _ToNullableDecimal(row.Cells[colNewPrice.Index].Value);
        decimal cost = _ToDecimal(row.Cells[colCost.Index].Value);

        var marginCell = row.Cells[colMargin.Index];
        if (newPrice.HasValue && newPrice.Value > 0)
        {
            decimal margin = (newPrice.Value - cost) / newPrice.Value;
            marginCell.Value = margin;
            marginCell.Style.ForeColor = margin > 0 ? MarginPositive : margin < 0 ? MarginNegative : Color.DimGray;
        }
        else
        {
            marginCell.Value = null;
            marginCell.Style.ForeColor = Color.DimGray;
        }

        // تمييز السعر المعدّل (يختلف عن السعر الحالي)
        decimal current = _ToDecimal(row.Cells[colCurrentPrice.Index].Value);
        bool changed = newPrice.HasValue &&
                       decimal.Round(newPrice.Value, 2) != decimal.Round(current, 2);

        var newPriceCell = row.Cells[colNewPrice.Index];
        newPriceCell.Style.ForeColor = changed ? ChangedFore : Color.Black;
        newPriceCell.Style.Font = changed ? FontBold : FontRegular;
    }

    // ═════════════════════════════════════════════════════════════════
    //  FOOTER — save (mock only) / close
    // ═════════════════════════════════════════════════════════════════

    private void btnSave_Click(object? sender, EventArgs e)
    {
        int changed = _CountChangedRows();
        if (changed == 0)
        {
            MessageBox.Show("لم يتم تعديل أي سعر. أدخل الأسعار الجديدة أولاً أو استخدم عملية دفعة.",
                "لا توجد تغييرات", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"سيتم حفظ الأسعار الجديدة لعدد {changed} صنف.\nهل أنت متأكد من المتابعة؟",
                "تأكيد الحفظ", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        MessageBox.Show("تم حفظ الأسعار الجديدة بنجاح (وضع تجريبي — لا يتم الحفظ فعلياً بعد).",
            "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnClose_Click(object? sender, EventArgs e)
        => Close();

    // ═════════════════════════════════════════════════════════════════
    //  HELPERS
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// عدد الصفوف التي تغيّر سعرها الجديد فعلياً عن السعر الحالي (بتقريب منزلتين).
    /// </summary>
    private int _CountChangedRows()
    {
        int changed = 0;
        foreach (DataGridViewRow row in dgvPrices.Rows)
        {
            if (row.IsNewRow) continue;

            decimal? newPrice = _ToNullableDecimal(row.Cells[colNewPrice.Index].Value);
            decimal current = _ToDecimal(row.Cells[colCurrentPrice.Index].Value);

            if (newPrice.HasValue && decimal.Round(newPrice.Value, 2) != decimal.Round(current, 2))
                changed++;
        }
        return changed;
    }

    private void _RefreshStatus()
    {
        int total = dgvPrices.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
        int changed = _CountChangedRows();

        lblStatus.Text = changed == 0
            ? $"{total} صنف — لم يتم تعديل أي سعر بعد."
            : $"تم تعديل {changed} من {total} صنف.";
    }

    private static decimal _ToDecimal(object? value)
        => value == null ? 0m : Convert.ToDecimal(value);

    private static decimal? _ToNullableDecimal(object? value)
    {
        if (value == null) return null;
        if (value is decimal dec) return dec;
        if (value is string s && string.IsNullOrWhiteSpace(s)) return null;

        // اللغة الحالية أولاً (12.5 / 12,5) ثم اللغة الثابتة كاحتياط
        if (decimal.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.CurrentCulture, out var v) ||
            decimal.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out v))
            return v;

        return null;
    }
}