using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using System.Data;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Customers.Forms;

public partial class frmCustomerSelection : Form
{
    private readonly IServiceProvider _serviceProvider;
    private DataTable _customersTable;
    private DataView _customersView;

    public CustomerDto? SelectedCustomer { get; private set; }

    public frmCustomerSelection()
    {
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    public frmCustomerSelection(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
    }

    private void frmCustomerSelection_Load(object sender, EventArgs e)
    {
        _LoadCustomers();
    }

    private async void _LoadCustomers()
    {
        Cursor = dgvCustomers.Cursor = Cursors.WaitCursor;

        _customersTable = new DataTable();
        _customersTable.Columns.Add("Id", typeof(int));
        _customersTable.Columns.Add("Name", typeof(string));
        _customersTable.Columns.Add("Phone", typeof(string));
        _customersTable.Columns.Add("Email", typeof(string));

        var result = await _serviceProvider.GetRequiredService<ICustomerApiService>().GetAllAsync();

        if (result.IsSuccess && result.Value != null)
        {
            foreach (var c in result.Value)
            {
                _customersTable.Rows.Add(c.Id, c.Name, c.Phone ?? string.Empty, c.Email ?? string.Empty);
            }
        }

        _customersView = new DataView(_customersTable);
        dgvCustomers.DataSource = _customersView;

        Cursor = dgvCustomers.Cursor = Cursors.Default;
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string search = txtSearch.Text.Trim();

        if (string.IsNullOrEmpty(search))
        {
            _customersView.RowFilter = string.Empty;
        }
        else
        {
            string safeSearch = search.Replace("'", "''");
            _customersView.RowFilter = string.Format(
                "Name LIKE '%{0}%' OR Phone LIKE '%{0}%'",
                safeSearch);
        }
    }

    private void dgvCustomers_DoubleClick(object sender, EventArgs e)
    {
        _SelectCurrent();
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
        _SelectCurrent();
    }

    private void _SelectCurrent()
    {
        if (dgvCustomers.CurrentRow == null)
        {
            return;
        }

        DataRowView row = (DataRowView)dgvCustomers.CurrentRow.DataBoundItem;
        int id = (int)row["Id"];
        string name = row["Name"] as string ?? string.Empty;
        string phone = row["Phone"] as string ?? string.Empty;
        string email = row["Email"] as string ?? string.Empty;

        SelectedCustomer = new CustomerDto(
            id,
            name,
            string.IsNullOrEmpty(phone) ? null : phone,
            string.IsNullOrEmpty(email) ? null : email,
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
