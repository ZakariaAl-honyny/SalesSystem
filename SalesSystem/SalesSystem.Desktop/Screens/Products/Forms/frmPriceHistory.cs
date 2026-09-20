using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.Business.Services;
using SalesSystem.Desktop.Helpers;
using SalesSystem.DTO.Responses;
using System.Data;
using Serilog;
using SalesSystem.Desktop.Products.Forms;

namespace SalesSystem.Desktop.Screens.Products.Forms;

public partial class frmPriceHistory : Form
{
    private readonly IServiceScope _scope;
    private IServiceProvider ServiceProvider => _scope.ServiceProvider;
    private int _productId;
    private DataTable _dtPrices = new();
    private bool _isBinding = false;
    private bool _isLoading = false;

    public frmPriceHistory(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();
        InitializeComponent();
        dgvPrices.AutoGenerateColumns = false;
        ThemeHelper.ApplyDataGridViewStyle(dgvPrices);
        cmbUnit.SelectedIndexChanged += cmbUnit_SelectedIndexChanged;
        ApplyUiPolish();
    }

    // UI polish (ThemeHelper)

    private void ApplyUiPolish()
    {
        ThemeHelper.ApplyButtonStyle(btnAddPrice, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        // Enter-as-Tab for fields without existing Enter handling
        cmbUnit.KeyDown += FieldEnter_KeyDown;
    }

    private void FieldEnter_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyData == Keys.Enter)
        {
            this.SelectNextControl((Control)sender, true, true, true, true);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }

    public void LoadForProduct(int productId)
    {
        _productId = productId;
        this.Text = "ط³ط¬ظ„ ط£ط³ط¹ط§ط± ط§ظ„ظ…ظ†طھط¬";
        lblMode.Text = "ط³ط¬ظ„ ط£ط³ط¹ط§ط± ط§ظ„ظ…ظ†طھط¬";
    }

    private async void frmPriceHistory_Load(object sender, EventArgs e)
    {
        try
        {
            _isBinding = true;
            await LoadUnitsAsync();
            _isBinding = false;
            await RefreshPricesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "ط®ط·ط£ ط£ط«ظ†ط§ط، طھط­ظ…ظٹظ„ ط³ط¬ظ„ ط§ظ„ط£ط³ط¹ط§ط±");
            MessageBox.Show("ط­ط¯ط« ط®ط·ط£ ط£ط«ظ†ط§ط، طھط­ظ…ظٹظ„ ط§ظ„ط¨ظٹط§ظ†ط§طھ.", "ط®ط·ط£", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadUnitsAsync()
    {
        try
        {
            var unitsResult = await ServiceProvider.GetRequiredService<IProductUnitService>()
                .GetByProductIdAsync(_productId, default);
            if (!unitsResult.IsSuccess || unitsResult.Value == null)
            {
                Log.Error("ظپط´ظ„ طھط­ظ…ظٹظ„ ظˆط­ط¯ط§طھ ط§ظ„ظ…ظ†طھط¬: {Error}", unitsResult.Error);
                MessageBox.Show("ظپط´ظ„ طھط­ظ…ظٹظ„ ظˆط­ط¯ط§طھ ط§ظ„ظ…ظ†طھط¬.", "ط®ط·ط£", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var units = unitsResult.Value;

            // Detach to prevent re-entrant call from SelectedIndexChanged
            cmbUnit.SelectedIndexChanged -= cmbUnit_SelectedIndexChanged;
            cmbUnit.DisplayMember = "UnitName";
            cmbUnit.ValueMember = "Id";
            cmbUnit.DataSource = new List<ProductUnitDto>(units);
            cmbUnit.SelectedIndexChanged += cmbUnit_SelectedIndexChanged;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "ط®ط·ط£ ط£ط«ظ†ط§ط، طھط­ظ…ظٹظ„ ظˆط­ط¯ط§طھ ط§ظ„ظ…ظ†طھط¬");
            MessageBox.Show("ظپط´ظ„ طھط­ظ…ظٹظ„ ظˆط­ط¯ط§طھ ط§ظ„ظ…ظ†طھط¬.", "ط®ط·ط£", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task RefreshPricesAsync()
    {
        if (_isLoading) return;
        _isLoading = true;
        Cursor = Cursors.WaitCursor;
        try
        {
            _dtPrices = new DataTable();
            _dtPrices.Columns.Add("Id", typeof(int));
            _dtPrices.Columns.Add("UnitName", typeof(string));
            _dtPrices.Columns.Add("Price", typeof(decimal));
            _dtPrices.Columns.Add("EffectiveFrom", typeof(string));
            _dtPrices.Columns.Add("EffectiveTo", typeof(string));
            _dtPrices.Columns.Add("Status", typeof(string));

            var unitListResult = await ServiceProvider.GetRequiredService<IProductUnitService>()
                .GetByProductIdAsync(_productId, default);
            if (!unitListResult.IsSuccess || unitListResult.Value == null)
            {
                Log.Error("ظپط´ظ„ طھط­ظ…ظٹظ„ ظˆط­ط¯ط§طھ ط§ظ„ظ…ظ†طھط¬: {Error}", unitListResult.Error);
                MessageBox.Show("ظپط´ظ„ طھط­ظ…ظٹظ„ ظˆط­ط¯ط§طھ ط§ظ„ظ…ظ†طھط¬.", "ط®ط·ط£", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var unitList = unitListResult.Value;

            IEnumerable<ProductUnitDto> filteredUnits = unitList;
            if (cmbUnit.SelectedValue is int selectedUnitId && selectedUnitId > 0)
            {
                filteredUnits = filteredUnits.Where(u => u.Id == selectedUnitId);
            }

            foreach (var unit in filteredUnits)
            {
                var pricesResult = await ServiceProvider.GetRequiredService<IProductPriceService>()
                    .GetByProductUnitAsync(unit.Id, default);
                if (!pricesResult.IsSuccess || pricesResult.Value == null)
                {
                    Log.Warning("ظپط´ظ„ طھط­ظ…ظٹظ„ ط£ط³ط¹ط§ط± ط§ظ„ظˆط­ط¯ط© {UnitId}: {Error}", unit.Id, pricesResult.Error);
                    continue;
                }
                foreach (var p in pricesResult.Value)
                {
                    _dtPrices.Rows.Add(
                        p.Id,
                        p.ProductUnitName ?? unit.UnitName ?? "",
                        p.Price,
                        p.EffectiveFrom.ToString("yyyy-MM-dd"),
                        p.EffectiveTo?.ToString("yyyy-MM-dd") ?? "ظ…ظپطھظˆط­",
                        p.IsCurrentlyEffective ? "ظ†ط´ط·" : "ظ…ظ†طھظ‡ظٹ");
                }
            }

            dgvPrices.DataSource = _dtPrices;
            if (dgvPrices.Columns["Id"] is { } idCol)
                idCol.Visible = false;
            if (dgvPrices.Columns["UnitName"] is { } unitCol)
                unitCol.HeaderText = "ط§ظ„ظˆط­ط¯ط©";
            if (dgvPrices.Columns["Price"] is { } priceCol)
                priceCol.HeaderText = "ط§ظ„ط³ط¹ط±";
            if (dgvPrices.Columns["EffectiveFrom"] is { } fromCol)
                fromCol.HeaderText = "ظ…ظ† طھط§ط±ظٹط®";
            if (dgvPrices.Columns["EffectiveTo"] is { } toCol)
                toCol.HeaderText = "ط¥ظ„ظ‰ طھط§ط±ظٹط®";
            if (dgvPrices.Columns["Status"] is { } statusCol)
                statusCol.HeaderText = "ط§ظ„ط­ط§ظ„ط©";

            lblRecordsCount.Text = _dtPrices.Rows.Count > 0 ? _dtPrices.Rows.Count.ToString() : "ظ„ط§ طھظˆط¬ط¯ ط£ط³ط¹ط§ط±";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while loading price history");
            MessageBox.Show("ط­ط¯ط« ط®ط·ط£ ط£ط«ظ†ط§ط، طھط­ظ…ظٹظ„ ط§ظ„ط¨ظٹط§ظ†ط§طھ.", "ط®ط·ط£", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
            _isLoading = false;
        }
    }

    private async void btnAddPrice_Click(object? sender, EventArgs e)
    {
        try
        {
            var dialog = ServiceProvider.GetRequiredService<frmAddUpdateProductPrice>();
            dialog.LoadForProduct(_productId, null,
                cmbUnit.DataSource as List<ProductUnitDto>);
            if (dialog.ShowDialog(this) == DialogResult.OK)
                await RefreshPricesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "ط®ط·ط£ ظپظٹ ظپطھط­ ظ†ط§ظپط°ط© ط¥ط¶ط§ظپط© ط§ظ„ط³ط¹ط±");
            MessageBox.Show("ط­ط¯ط« ط®ط·ط£ ط£ط«ظ†ط§ط، ظپطھط­ ظ†ط§ظپط°ط© ط¥ط¶ط§ظپط© ط§ظ„ط³ط¹ط±.", "ط®ط·ط£", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        this.Close();
    }

    private async void cmbUnit_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isBinding) return;
        try
        {
            if (cmbUnit.SelectedValue is int unitId && unitId > 0)
            {
                await RefreshPricesAsync();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "ط®ط·ط£ ظپظٹ طھطµظپظٹط© ط§ظ„ط£ط³ط¹ط§ط± ط­ط³ط¨ ط§ظ„ظˆط­ط¯ط©");
            MessageBox.Show("ط­ط¯ط« ط®ط·ط£ ط£ط«ظ†ط§ط، طھطµظپظٹط© ط§ظ„ط£ط³ط¹ط§ط±.", "ط®ط·ط£", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

