using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using System.Data;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Suppliers.Forms;

public partial class frmSupplierSelection : Form
{
    private readonly IServiceProvider _serviceProvider;
    private DataTable _suppliersTable;
    private DataView _suppliersView;

    public SupplierDto? SelectedSupplier { get; private set; }

        public frmSupplierSelection()
        {
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        public frmSupplierSelection(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
    }

    private void frmSupplierSelection_Load(object sender, EventArgs e)
    {
        _LoadSuppliers();
    }

    private async void _LoadSuppliers()
    {
        Cursor = dgvSuppliers.Cursor = Cursors.WaitCursor;

        _suppliersTable = new DataTable();
        _suppliersTable.Columns.Add("Id", typeof(int));
        _suppliersTable.Columns.Add("Name", typeof(string));
        _suppliersTable.Columns.Add("Phone", typeof(string));
        _suppliersTable.Columns.Add("Email", typeof(string));

        var result = await _serviceProvider.GetRequiredService<ISupplierApiService>().GetAllAsync();

        if (result.IsSuccess && result.Value != null)
        {
            foreach (var s in result.Value)
            {
                _suppliersTable.Rows.Add(s.Id, s.Name, s.Phone ?? string.Empty, s.Email ?? string.Empty);
            }
        }

        _suppliersView = new DataView(_suppliersTable);
        dgvSuppliers.DataSource = _suppliersView;

        Cursor = dgvSuppliers.Cursor = Cursors.Default;
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string search = txtSearch.Text.Trim();

        if (string.IsNullOrEmpty(search))
        {
            _suppliersView.RowFilter = string.Empty;
        }
        else
        {
            string safeSearch = search.Replace("'", "''");
            _suppliersView.RowFilter = string.Format(
                "Name LIKE '%{0}%' OR Phone LIKE '%{0}%'",
                safeSearch);
        }
    }

    private void dgvSuppliers_DoubleClick(object sender, EventArgs e)
    {
        _SelectCurrent();
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
        _SelectCurrent();
    }

    private void _SelectCurrent()
    {
        if (dgvSuppliers.CurrentRow == null)
        {
            return;
        }

        DataRowView row = (DataRowView)dgvSuppliers.CurrentRow.DataBoundItem;
        int id = (int)row["Id"];
        string name = row["Name"] as string ?? string.Empty;
        string phone = row["Phone"] as string ?? string.Empty;
        string email = row["Email"] as string ?? string.Empty;

        SelectedSupplier = new SupplierDto(
            id,
            name,
            string.IsNullOrEmpty(phone) ? null : phone,
            string.IsNullOrEmpty(email) ? null : email,
            null,
            null,
            null,
            0,
            true,
            0
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
