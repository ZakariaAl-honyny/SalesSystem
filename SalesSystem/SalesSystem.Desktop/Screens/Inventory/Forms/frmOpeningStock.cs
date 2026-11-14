using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Products.Forms;
using System.Data;
using Serilog;
using System.ComponentModel;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// «بضاعة أول المدة» — مستند إدخال أرصدة افتتاحية.
/// الهيكل (RULE-N79 / RULE-N135…N138): العنوان ← أوامر المستند ← بيانات المستند ← الأصناف ← الإجماليات.
/// منطق الترحيل (تسوية افتتاحية + اعتمادها) لم يُمسّ إطلاقاً.
/// </summary>
public partial class frmOpeningStock : Form
{
    private readonly IServiceScope _scope;
    private IServiceProvider ServiceProvider => _scope.ServiceProvider;
    private DataTable _dtLines = new();
    private Dictionary<int, List<ProductUnitDto>> _productUnitsCache = [];

    /// <summary>نص شريط العنوان الأساسي — يُحدَّث بعدّاد الأصناف لإظهار وضع المستند (RULE-N44).</summary>
    private const string FormTitle = "بضاعة أول المدة";

    public frmOpeningStock(IServiceScopeFactory scopeFactory)
    {
        InitializeComponent();
        _scope = scopeFactory.CreateScope();
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        KeyPreview = true;
        KeyDown += frmOpeningStock_KeyDown;

        // RULE-N40…RULE-N46 — تنقل Enter/الأسهم + تحديد القيمة + تظليل الحقل النشط.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnAddProduct, btnRemoveProduct, btnSave);

        // RULE-N47 — الاختصارات الموجودة فعلاً على هذه الشاشة فقط.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAddProduct.PerformClick()),
            (Keys.F4, _SaveIfReady));

        // RULE-N147 — Enter = الزر الأساسي للوضع الحالي («حفظ وترحيل»)، ولا يعمل داخل شبكة السطور.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        // الزر الافتراضي لا يُطلق الترحيل من داخل الشبكة (Enter هناك تنقّل بين الخلايا — RULE-N40).
        dgvLines.Enter += (s, e) => AcceptButton = null;
        dgvLines.Leave += (s, e) => AcceptButton = _PrimaryActionButton();

        // RULE-N97 — التفقيط الحيّ لقيمة الإجمالي المالي.
        LiveTafqeetHelper.Attach(liveTip,
            (txtTotalCost, "إجمالي التكلفة", LiveTafqeetHelper.ReadMode.Money));

        // خطأ تحرير خلية (مثل وحدة غير صالحة) لا يُسقط الشاشة.
        dgvLines.DataError += (s, e) => e.ThrowException = false;
        dgvLines.SelectionChanged += (s, e) => _UpdateButtonsState();

        ApplyUiTheme();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
    }

    // ── الثيم (RULE-N50…RULE-N54) ─────────────────────────────────────

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply(this)</c> آخر سطر دائماً.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvLines);

        lblItemCountCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblTotalCostCaption.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── Shortcuts ─────────────────────────────────────────────────────

    /// <summary>
    /// RULE-N147 — الزر الأساسي للوضع الحالي: «حفظ وترحيل».
    /// يرجع <c>null</c> عندما يكون التركيز داخل شبكة السطور (<c>dgvLines</c>) حتى يبقى
    /// <c>Enter</c> تنقلاً بين الخلايا ولا يُرحّل المستند سهواً.
    /// </summary>
    private Button? _PrimaryActionButton()
        => dgvLines.ContainsFocus ? null : btnSave;

    /// <summary>F4 — ترحيل المستند إن كان جاهزاً.</summary>
    private void _SaveIfReady()
    {
        if (btnSave.Enabled) btnSave.PerformClick();
    }

    // ── التحميل ───────────────────────────────────────────────────────

    private async void frmOpeningStock_Load(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await LoadWarehousesAsync();
            dtpDate.Value = DateTime.Today;
            SetupLinesGrid();
            _UpdateButtonsState();
            _UpdateModeHeader();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة بضاعة أول المدة");
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task LoadWarehousesAsync()
    {
        try
        {
            var result = await ServiceProvider.GetRequiredService<IWarehouseDesktopService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "Id";
                cmbWarehouse.DataSource = new List<WarehouseDto>(result.Value);
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة المخازن.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل المخازن");
            MessageBox.Show("تعذر تحميل قائمة المخازن. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SetupLinesGrid()
    {
        _dtLines = new DataTable();
        _dtLines.Columns.Add("ProductId", typeof(int));
        _dtLines.Columns.Add("ProductUnitId", typeof(int));
        _dtLines.Columns.Add("ProductName", typeof(string));
        _dtLines.Columns.Add("UnitName", typeof(string));
        _dtLines.Columns.Add("Quantity", typeof(decimal));
        _dtLines.Columns.Add("UnitCost", typeof(decimal));
        _dtLines.Columns.Add("TotalCost", typeof(decimal));

        dgvLines.DataSource = new DataView(_dtLines);
        _UpdateTotals();
    }

    // ── مؤشر الوضع + آلة حالة الأزرار (RULE-N43/RULE-N44) ─────────────

    /// <summary>مؤشر الوضع: مستند جديد + عدد الأصناف المدخلة.</summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = $"{FormTitle} — مستند جديد (عدد الأصناف: {_dtLines.Rows.Count})";
    }

    /// <summary>«حذف السطر» يحتاج سطراً محدّداً، و«حفظ وترحيل» يحتاج صنفاً واحداً على الأقل.</summary>
    private void _UpdateButtonsState()
    {
        bool hasLines = _dtLines.Rows.Count > 0;
        bool hasSelectedLine = dgvLines.CurrentRow is { IsNewRow: false };

        btnRemoveProduct.Enabled = hasLines && hasSelectedLine;
        btnSave.Enabled = hasLines;
        miRemoveLine.Enabled = btnRemoveProduct.Enabled;

        // RULE-N147 — الزر الافتراضي يتبع الزر الأساسي الحالي (وnull داخل شبكة السطور).
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>شريط الإجماليات: عدد الأصناف + إجمالي التكلفة (N2).</summary>
    private void _UpdateTotals()
    {
        decimal total = 0m;

        foreach (DataRow row in _dtLines.Rows)
        {
            if (row.IsNull("TotalCost")) continue;
            total += Convert.ToDecimal(row["TotalCost"]);
        }

        lblItemCountValue.Text = clsFormat.Number(_dtLines.Rows.Count);
        txtTotalCost.Text = clsFormat.Money(total);

        _UpdateButtonsState();
        _UpdateModeHeader();
    }

    // ── إضافة/حذف الأصناف ─────────────────────────────────────────────

    private async void btnAddProduct_Click(object? sender, EventArgs e)
    {
        try
        {
            var dialog = ServiceProvider.GetRequiredService<frmProductSelection>();
            if (dialog.ShowDialog(this) != DialogResult.OK || dialog.SelectedProduct == null)
                return;

            var existing = _dtLines.AsEnumerable()
                .FirstOrDefault(r => r.Field<int>("ProductId") == dialog.SelectedProduct.Id);
            if (existing != null)
            {
                MessageBox.Show("هذا المنتج مضاف مسبقاً للقائمة.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var units = await LoadProductUnitsAsync(dialog.SelectedProduct.Id);
            var baseUnit = units.FirstOrDefault(pu => pu.IsBaseUnit) ?? units.FirstOrDefault();

            _dtLines.Rows.Add(
                dialog.SelectedProduct.Id,
                baseUnit?.Id ?? 0,
                dialog.SelectedProduct.Name,
                baseUnit?.UnitName ?? "",
                1m,
                0m,
                0m);

            _UpdateTotals();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إضافة منتج");
            MessageBox.Show("حدث خطأ أثناء إضافة المنتج.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task<List<ProductUnitDto>> LoadProductUnitsAsync(int productId)
    {
        if (_productUnitsCache.TryGetValue(productId, out var cached))
            return cached;

        try
        {
            var result = await ServiceProvider.GetRequiredService<IProductDesktopService>()
                .GetProductUnitsAsync(productId);

            if (result.IsSuccess && result.Value != null)
            {
                var list = result.Value.ToList();
                _productUnitsCache[productId] = list;
                return list;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل وحدات المنتج {ProductId}", productId);
        }

        return [];
    }

    // ── DGV Event Handlers ─────────────────────────────────────────────

    private void dgvLines_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
    {
        if (dgvLines.CurrentCell.ColumnIndex != dgvLines.Columns["colUnit"].Index)
            return;

        if (e.Control is DataGridViewComboBoxEditingControl combo)
        {
            combo.DropDownStyle = ComboBoxStyle.DropDownList;

            // Unsubscribe first to prevent multiple subscriptions (WinForms gotcha)
            combo.SelectedIndexChanged -= Combo_SelectedIndexChanged;
            combo.SelectedIndexChanged += Combo_SelectedIndexChanged;

            combo.DataSource = null;

            if (dgvLines.CurrentRow?.DataBoundItem is DataRowView row)
            {
                int productId = Convert.ToInt32(row["ProductId"]);
                if (_productUnitsCache.TryGetValue(productId, out var units))
                {
                    combo.DataSource = new List<ProductUnitDto>(units);
                    combo.DisplayMember = "UnitName";
                    combo.ValueMember = "Id";
                }
            }
        }
    }

    private void Combo_SelectedIndexChanged(object? sender, EventArgs e)
    {
        dgvLines.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void dgvLines_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (dgvLines.CurrentCell.ColumnIndex == dgvLines.Columns["colUnit"].Index)
        {
            dgvLines.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }

    private void dgvLines_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        int colIndex = e.ColumnIndex;

        if (colIndex == dgvLines.Columns["colQuantity"].Index ||
            colIndex == dgvLines.Columns["colUnitCost"].Index)
        {
            UpdateRowTotal(e.RowIndex);
        }
        else if (colIndex == dgvLines.Columns["colUnit"].Index)
        {
            UpdateUnitDisplay(e.RowIndex);
            UpdateRowTotal(e.RowIndex);
        }
    }

    private void UpdateRowTotal(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= _dtLines.Rows.Count)
            return;

        var row = _dtLines.Rows[rowIndex];
        decimal qty = Convert.ToDecimal(row["Quantity"]);
        decimal cost = Convert.ToDecimal(row["UnitCost"]);
        row["TotalCost"] = qty * cost;

        _UpdateTotals();
    }

    private void UpdateUnitDisplay(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= _dtLines.Rows.Count)
            return;

        var row = _dtLines.Rows[rowIndex];
        int unitId = Convert.ToInt32(row["ProductUnitId"]);
        int productId = Convert.ToInt32(row["ProductId"]);

        if (_productUnitsCache.TryGetValue(productId, out var units))
        {
            var unit = units.FirstOrDefault(u => u.Id == unitId);
            row["UnitName"] = unit?.UnitName ?? "";
        }
    }

    // ── Actions ────────────────────────────────────────────────────────

    private void btnRemoveProduct_Click(object? sender, EventArgs e)
    {
        if (dgvLines.CurrentRow?.DataBoundItem is not DataRowView row) return;
        _dtLines.Rows.Remove(row.Row);
        _UpdateTotals();
    }

    private void miAddProduct_Click(object? sender, EventArgs e)
        => btnAddProduct.PerformClick();

    private void miRemoveLine_Click(object? sender, EventArgs e)
    {
        if (btnRemoveProduct.Enabled) btnRemoveProduct.PerformClick();
    }

    private void _ValidateEmptyTextBox(object sender, CancelEventArgs e)
    {
        if (sender is TextBox txt && string.IsNullOrEmpty(txt.Text.Trim()))
        {
            e.Cancel = true;
            errorProvider.SetError(txt, "هذا الحقل مطلوب");
        }
        else
        {
            e.Cancel = false;
            errorProvider.SetError((Control)sender, null);
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbWarehouse.SelectedValue == null)
        {
            errorProvider.SetError(cmbWarehouse, "يرجى اختيار المخزن");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbWarehouse, "");
        }

        if (_dtLines.Rows.Count == 0)
        {
            MessageBox.Show("يرجى إضافة منتج واحد على الأقل.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            isValid = false;
        }

        foreach (DataRow row in _dtLines.Rows)
        {
            int puId = Convert.ToInt32(row["ProductUnitId"]);
            if (puId <= 0)
            {
                MessageBox.Show("يرجى اختيار الوحدة المناسبة لكل المنتجات.", "بيانات غير مكتملة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
                break;
            }

            decimal qty = Convert.ToDecimal(row["Quantity"]);
            if (qty <= 0)
            {
                MessageBox.Show("الكمية يجب أن تكون أكبر من صفر.", "بيانات غير صحيحة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
                break;
            }
        }

        return isValid;
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        errorProvider.Clear();
        if (!ValidateChildren()) return;
        if (!_ValidateForm()) return;

        btnSave.Enabled = false;
        try
        {
            Cursor = Cursors.WaitCursor;

            if (cmbWarehouse.SelectedValue is not short warehouseId)
            {
                MessageBox.Show("يرجى اختيار المخزن", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var adjustmentApi = ServiceProvider.GetRequiredService<IInventoryAdjustmentDesktopService>();

            var createRequest = new CreateInventoryAdjustmentRequest(
                WarehouseId: warehouseId,
                AdjustmentType: 1); // Opening

            var createResult = await adjustmentApi.CreateAsync(createRequest);
            if (!createResult.IsSuccess || createResult.Value == null)
            {
                MessageBox.Show(createResult.Error ?? "حدث خطأ أثناء إنشاء التسوية.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int adjustmentId = createResult.Value.Id;
            int successCount = 0;

            foreach (DataRow row in _dtLines.Rows)
            {
                int productUnitId = Convert.ToInt32(row["ProductUnitId"]);
                decimal quantity = Convert.ToDecimal(row["Quantity"]);
                decimal unitCost = Convert.ToDecimal(row["UnitCost"]);

                if (productUnitId <= 0 || quantity <= 0) continue;

                var lineRequest = new AddInventoryAdjustmentLineRequest(
                    InventoryAdjustmentId: adjustmentId,
                    ProductUnitId: productUnitId,
                    ExpectedQuantity: 0m,
                    ActualQuantity: quantity,
                    UnitCost: unitCost);

                var lineResult = await adjustmentApi.AddLineAsync(adjustmentId, lineRequest);
                if (lineResult.IsSuccess)
                    successCount++;
            }

            if (successCount == 0)
            {
                MessageBox.Show("فشل إضافة جميع المنتجات. تحقق من البيانات.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var postResult = await adjustmentApi.PostAsync(adjustmentId);
            if (!postResult.IsSuccess)
            {
                MessageBox.Show(postResult.Error ?? "حدث خطأ أثناء ترحيل التسوية.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"تم حفظ وترحيل {successCount} منتج بنجاح كبضاعة أول المدة.", "تم",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء حفظ الكميات الافتتاحية");
            MessageBox.Show("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
            btnSave.Enabled = true;
            _UpdateButtonsState();
        }
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void frmOpeningStock_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.S)
        {
            e.SuppressKeyPress = true;
            btnSave.PerformClick();
        }
    }
}
