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
            KeyboardHelper.WireSelectAllOnFocus(this);
            KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));   // RULE-N46
            KeyboardHelper.WireArrowNavigation(this);                                    // RULE-N45
        }

        public frmSupplierSelection(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
    }

    private void frmSupplierSelection_Load(object sender, EventArgs e)
    {
        _LoadSuppliers();
        ApplyGreenTheme();
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyGrid(dgvSuppliers);

        // RULE-N22/RULE-N52: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        dgvSuppliers.ReadOnly = true;
        dgvSuppliers.AllowUserToAddRows = false;
        dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSuppliers.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
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
