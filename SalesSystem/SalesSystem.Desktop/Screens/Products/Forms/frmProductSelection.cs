using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using System.ComponentModel;
using System.Data;
using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Products.Forms;

public partial class frmProductSelection : Form
{
    private readonly IServiceScope _scope;
    private IServiceProvider ServiceProvider => _scope.ServiceProvider;
    private DataTable _productsTable;
    private DataView _productsView;

    public ProductDto? SelectedProduct { get; private set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public short WarehouseId { get; set; }

    private frmProductSelection()
    {
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        dgvProducts.AutoGenerateColumns = false;
    }

    public frmProductSelection(IServiceScopeFactory scopeFactory) : this()
    {
        _scope = scopeFactory.CreateScope();
    }

    private void frmProductSelection_Load(object sender, EventArgs e)
    {
        _LoadProducts();
    }

    private async void _LoadProducts()
    {
        Cursor = dgvProducts.Cursor = Cursors.WaitCursor;
        try
        {
            _productsTable = new DataTable();
            _productsTable.Columns.Add("Id", typeof(int));
            _productsTable.Columns.Add("Name", typeof(string));
            _productsTable.Columns.Add("CategoryName", typeof(string));
            _productsTable.Columns.Add("Barcode", typeof(string));

            var result = await ServiceProvider.GetRequiredService<IProductApiService>().GetAllAsync();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var p in result.Value)
                {
                    _productsTable.Rows.Add(
                        p.Id,
                        p.Name,
                        p.CategoryName ?? string.Empty,
                        p.Barcode ?? string.Empty);
                }
            }
            else if (!result.IsSuccess)
            {
                MessageBox.Show(result.Error ?? "حدث خطأ في تحميل قائمة المنتجات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _productsView = new DataView(_productsTable);
            dgvProducts.DataSource = _productsView;
            lblRecordsCount.Text = _productsView.Count.ToString();

            if (_productsView.Count == 0)
            {
                MessageBox.Show("لا توجد منتجات متاحة.", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة المنتجات");
            MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = dgvProducts.Cursor = Cursors.Default;
        }
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        if (_productsView == null) return;

        string search = txtSearch.Text.Trim();

        if (string.IsNullOrEmpty(search))
        {
            _productsView.RowFilter = string.Empty;
        }
        else
        {
            string safeSearch = search.Replace("'", "''");
            _productsView.RowFilter = string.Format(
                "Name LIKE '%{0}%' OR Barcode LIKE '%{0}%' OR CategoryName LIKE '%{0}%'",
                safeSearch);
        }

        lblRecordsCount.Text = _productsView.Count.ToString();
    }

    private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            btnSelect.PerformClick();
        }
    }

    private void dgvProducts_DoubleClick(object sender, EventArgs e)
    {
        _SelectCurrent();
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
        _SelectCurrent();
    }

    private void _SelectCurrent()
    {
        if (dgvProducts.CurrentRow?.DataBoundItem is not DataRowView row) return;

        int id = (int)row["Id"];
        string name = row["Name"] as string ?? string.Empty;

        SelectedProduct = new ProductDto(
            id,
            name,
            0,
            row["CategoryName"] as string,
            null,
            0,
            false,
            null,
            true,
            row["Barcode"] as string
        );

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
