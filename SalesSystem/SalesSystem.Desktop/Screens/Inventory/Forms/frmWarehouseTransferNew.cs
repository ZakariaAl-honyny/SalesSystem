using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// «سند تحويل مخزني» — شاشة إدخال تحويل بين مستودعين.
/// الهيكل (RULE-N79 / RULE-N135…N138): العنوان ← أوامر السند ← بيانات السند ← الأصناف ← شريط الملاحظة السفلي.
/// منطق التحقق وحساب الكميات بقي كما هو حرفياً.
/// </summary>
public partial class frmWarehouseTransferNew : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── Mock data (in-memory, Arabic) ─────────────────────────────────────────────
    private readonly string[] _warehouses = { "المستودع الرئيسي", "مخزن المعرض", "مخزن التجزئة" };
    private readonly string[] _units = { "كيس", "كرتونة", "قطعة", "عبوة", "زجاجة", "علبة" };

    private sealed record MockProduct(string Name, string Unit);

    private static readonly MockProduct[] _mockProducts =
    {
        new("سكر أبيض 1 كجم",        "كيس"),
        new("أرز بسمتي 5 كجم",       "كيس"),
        new("زيت دوار الشمس 1 لتر",  "زجاجة"),
        new("شاي أحمر 500 جم",       "علبة"),
        new("دقيق فاخر 1 كجم",       "كيس"),
        new("مكرونة اسباجتي 400 جم", "كرتونة"),
        new("صلصة طماطم 340 جم",     "عبوة"),
        new("جبنة بيضاء 500 جم",     "قطعة"),
    };

    private static readonly Color QtyPositive = Color.FromArgb(25, 135, 84);    // كمية موجبة
    private static readonly Color QtyZero = Color.FromArgb(173, 181, 189);       // بلا كمية

    /// <summary>نص شريط العنوان الأساسي — تُضاف إليه حالة السند (RULE-N44).</summary>
    private const string FormTitle = "سند تحويل مخزني";

    /// <summary>التركيز داخل شبكة السطور — Enter هناك تنقّل داخل الشبكة لا ترحيل (RULE-N147 / RULE-N40).</summary>
    private bool _linesGridHasFocus;

    public frmWarehouseTransferNew(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…RULE-N46 — تنقل Enter/الأسهم + تحديد القيمة + تظليل الحقل النشط.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnCancel);

        // RULE-N47 — الاختصارات الموجودة فعلاً (F4 = حفظ السند؛ Esc يغلقه CancelButton).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F4, () => btnSave.PerformClick()));

        // RULE-N147 — Enter = الزر الأساسي للوضع الحالي («حفظ السند»)، ولا يعمل داخل شبكة السطور.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        // حماية الشبكة (RULE-N40): طالما التركيز داخل السطور لا يُسند النموذج Enter إلى «حفظ السند».
        dgvLines.Enter += (s, e) =>
        {
            _linesGridHasFocus = true;
            AcceptButton = _PrimaryActionButton();
        };
        dgvLines.Leave += (s, e) =>
        {
            _linesGridHasFocus = false;
            AcceptButton = _PrimaryActionButton();
        };

        // خطأ تحرير خلية (صنف/وحدة من قائمة) لا يُسقط الشاشة.
        dgvLines.DataError += (s, e) => e.ThrowException = false;

        ApplyUiTheme();
    }

    // ── الثيم (RULE-N50…RULE-N54) ─────────────────────────────────────

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply(this)</c> آخر سطر دائماً.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvLines);

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ═════════════════════════════════════════════════════════════════
    //  LOAD — populate mock lookups + seed the transfer lines
    // ═════════════════════════════════════════════════════════════════

    private void frmWarehouseTransferNew_Load(object sender, EventArgs e)
    {
        cmbFromWarehouse.Items.AddRange(_warehouses);
        cmbFromWarehouse.SelectedIndex = 0;

        cmbToWarehouse.Items.AddRange(_warehouses);
        cmbToWarehouse.SelectedIndex = 1;

        dtpDate.Value = DateTime.Now;
        txtTransferNo.Text = "جديد";

        colProduct.Items.AddRange(_mockProducts.Select(p => p.Name).ToArray());
        colUnit.Items.AddRange(_units);

        foreach (var p in _mockProducts)
        {
            int rowIndex = dgvLines.Rows.Add();
            dgvLines.Rows[rowIndex].Cells[colProduct.Index].Value = p.Name;
            dgvLines.Rows[rowIndex].Cells[colUnit.Index].Value = p.Unit;
            dgvLines.Rows[rowIndex].Cells[colQuantity.Index].Value = 0m;
            dgvLines.Rows[rowIndex].Cells[colBatchNo.Index].Value = null;
            dgvLines.Rows[rowIndex].Cells[colExpiryDate.Index].Value = null;
        }

        _ApplyQuantityStyling();
    }

    // ═════════════════════════════════════════════════════════════════
    //  GRID — quantity editing polish
    // ═════════════════════════════════════════════════════════════════

    private void dgvLines_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvLines.Rows.Count) return;
        if (e.ColumnIndex != colQuantity.Index) return;

        var cell = dgvLines.Rows[e.RowIndex].Cells[colQuantity.Index];
        decimal? qty = _ToNullableDecimal(cell.Value);

        // Invalid text typed by the user → reset the cell to blank
        if (qty is null && cell.Value is string s && !string.IsNullOrWhiteSpace(s))
        {
            cell.Value = null;
        }

        _ApplyQuantityStyling();
    }

    private void dgvLines_UserAddedRow(object? sender, DataGridViewRowEventArgs e)
    {
        e.Row.Cells[colQuantity.Index].Value = 0m;
        _ApplyQuantityStyling();
    }

    private void dgvLines_UserDeletedRow(object? sender, DataGridViewRowEventArgs e)
    {
        _ApplyQuantityStyling();
    }

    /// <summary>كمية > 0 بالخط العريض الأخضر، وإلا رمادي خافت — ويُحدَّث مؤشر الوضع بعدّاد الأسطر.</summary>
    private void _ApplyQuantityStyling()
    {
        int linesWithQty = 0;

        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.IsNewRow) continue;

            var cell = row.Cells[colQuantity.Index];
            decimal? qty = _ToNullableDecimal(cell.Value);

            if (qty.HasValue && qty.Value > 0) linesWithQty++;

            cell.Style.ForeColor = qty.HasValue && qty.Value > 0 ? QtyPositive : QtyZero;
            cell.Style.Font = new Font("Tahoma", 10F,
                qty.HasValue && qty.Value > 0 ? FontStyle.Bold : FontStyle.Regular);
        }

        _UpdateModeHeader(linesWithQty);
    }

    /// <summary>مؤشر الوضع (RULE-N44): سند جديد + عدد الأسطر ذات الكمية — ويُحدَّث معه زر Enter الأساسي (RULE-N147).</summary>
    private void _UpdateModeHeader(int linesWithQty)
    {
        lblMode.Text = $"{FormTitle} — مستند جديد (أسطر بكمية: {linesWithQty})";
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147): «حفظ السند».
    /// يُرجع <c>null</c> عندما يكون التركيز داخل شبكة السطور، فيبقى Enter تنقّلاً داخل الشبكة
    /// ولا يُرحّل السند (RULE-N40 + حماية المخزون §2.7).
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (_linesGridHasFocus || dgvLines.ContainsFocus || dgvLines.IsCurrentCellInEditMode)
            return null;

        return btnSave;
    }

    // ═════════════════════════════════════════════════════════════════
    //  FOOTER ACTIONS — save / cancel (mock only)
    // ═════════════════════════════════════════════════════════════════

    private void btnSave_Click(object? sender, EventArgs e)
    {
        if (!_ValidateHeader()) return;
        if (!_ValidateLines()) return;

        MessageBox.Show("تم حفظ سند التحويل (وضع تجريبي — لا خصم فعلي من المخزون).",
            "حفظ السند", MessageBoxButtons.OK, MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        Close();
    }

    // ── Context menu (RULE-N23) ───────────────────────────────────────

    private void miSave_Click(object? sender, EventArgs e)
        => btnSave.PerformClick();

    private void miDeleteRow_Click(object? sender, EventArgs e)
    {
        if (dgvLines.CurrentRow is not { IsNewRow: false } row) return;

        dgvLines.Rows.Remove(row);
        _ApplyQuantityStyling();
    }

    // ═════════════════════════════════════════════════════════════════
    //  VALIDATION — Arabic messages
    // ═════════════════════════════════════════════════════════════════

    private bool _ValidateHeader()
    {
        if (cmbFromWarehouse.SelectedIndex < 0 || cmbToWarehouse.SelectedIndex < 0)
        {
            MessageBox.Show("يرجى اختيار مستودع الإرسال ومستودع الاستلام.",
                "بيانات غير مكتملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (cmbFromWarehouse.SelectedIndex == cmbToWarehouse.SelectedIndex)
        {
            MessageBox.Show("لا يمكن أن يكون مستودع الإرسال ومستودع الاستلام متطابقين.",
                "بيانات غير صحيحة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cmbToWarehouse.Focus();
            return false;
        }

        return true;
    }

    private bool _ValidateLines()
    {
        int linesWithQty = 0;

        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.IsNewRow) continue;

            decimal? qty = _ToNullableDecimal(row.Cells[colQuantity.Index].Value);

            if (qty.HasValue && qty.Value < 0)
            {
                MessageBox.Show("الكمية لا يمكن أن تكون سالبة.",
                    "بيانات غير صحيحة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvLines.CurrentCell = row.Cells[colQuantity.Index];
                return false;
            }

            if (qty.HasValue && qty.Value > 0) linesWithQty++;
        }

        if (linesWithQty == 0)
        {
            MessageBox.Show("يجب إدخال كمية أكبر من صفر لصنف واحد على الأقل.",
                "بيانات غير مكتملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    // ── Cell value helpers ─────────────────────────────────────────────

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
