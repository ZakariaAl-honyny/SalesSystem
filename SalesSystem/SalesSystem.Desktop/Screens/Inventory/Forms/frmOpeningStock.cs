using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Products.Forms;
using System.Data;
using Serilog;
using System.ComponentModel;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

public partial class frmOpeningStock : Form
{
    private readonly IServiceScope _scope;
    private IServiceProvider ServiceProvider => _scope.ServiceProvider;
    private DataTable _dtLines = new();
    private Dictionary<int, List<ProductUnitDto>> _productUnitsCache = [];

    public frmOpeningStock(IServiceScopeFactory scopeFactory)
    {
        InitializeComponent();
        _scope = scopeFactory.CreateScope();
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        KeyPreview = true;
        KeyDown += frmOpeningStock_KeyDown;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
    }

    private async void frmOpeningStock_Load(object sender, EventArgs e)
    {
        await LoadWarehousesAsync();
        dtpDate.Value = DateTime.Today;
        SetupLinesGrid();
    }

    private async Task LoadWarehousesAsync()
    {
        try
        {
            var result = await ServiceProvider.GetRequiredService<IWarehouseApiService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "Id";
                cmbWarehouse.DataSource = new List<WarehouseDto>(result.Value);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل المخازن");
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
    }

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
            var result = await ServiceProvider.GetRequiredService<IProductApiService>()
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

            var adjustmentApi = ServiceProvider.GetRequiredService<IInventoryAdjustmentApiService>();

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
