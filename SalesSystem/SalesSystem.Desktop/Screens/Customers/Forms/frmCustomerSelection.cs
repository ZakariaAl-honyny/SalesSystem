using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using System.Data;
using Serilog;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Screens.Customers.Forms;

/// <summary>
/// منتقي العميل — شاشة اختيار للقراءة تُفتح نمطياً وتُرجع <see cref="SelectedCustomer"/>
/// مع <see cref="DialogResult.OK"/> (RULE-N09 · RULE-N100).
/// </summary>
public partial class frmCustomerSelection : Form
{
    private readonly IServiceProvider? _serviceProvider;

    private DataTable? _customersTable;
    private DataView? _customersView;

    /// <summary>العميل المختار — العقد الذي تقرأه الشاشات المستدعية (لا يُغيَّر).</summary>
    public CustomerDto? SelectedCustomer { get; private set; }

    private ICustomerDesktopService Service
        => _serviceProvider!.GetRequiredService<ICustomerDesktopService>();

    public frmCustomerSelection()
    {
        InitializeComponent();

        // RULE-N40…RULE-N46 — تنقل وتحديد وتظليل الحقل النشط والأسهم.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));   // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                    // RULE-N45

        // RULE-N43: «تحديد» معطّل بلا سطر محدد + مظهر رمادي باهت عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSelect);

        // RULE-N47: F5 تحديث. Enter/Esc يُديرهما AcceptButton/CancelButton ومعالج الشبكة — بلا تكرار.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F5, () => _LoadCustomers()));

        _UpdateButtonsState();
    }

    public frmCustomerSelection(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
    }

    private void frmCustomerSelection_Load(object sender, EventArgs e)
    {
        _LoadCustomers();
        ApplyGreenTheme();
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — ينتهي بـ Apply(this) دائماً.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyGrid(dgvCustomers);

        // RULE-N22/RULE-N52: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        dgvCustomers.ReadOnly = true;
        dgvCustomers.AllowUserToAddRows = false;
        dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCustomers.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ── التحميل ───────────────────────────────────────────────────────────────

    private async void _LoadCustomers()
    {
        if (_serviceProvider is null) return;

        Cursor = dgvCustomers.Cursor = Cursors.WaitCursor;
        try
        {
            var result = await Service.GetAllAsync();

            if (!result.IsSuccess || result.Value is null)
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة العملاء. يرجى المحاولة مرة أخرى.");
                return;
            }

            _customersTable = new DataTable();
            _customersTable.Columns.Add("Id", typeof(int));
            _customersTable.Columns.Add("Name", typeof(string));
            _customersTable.Columns.Add("Phone", typeof(string));
            _customersTable.Columns.Add("Email", typeof(string));

            foreach (var c in result.Value)
                _customersTable.Rows.Add(c.Id, c.Name, c.Phone ?? string.Empty, c.Email ?? string.Empty);

            _customersView = new DataView(_customersTable);
            dgvCustomers.DataSource = _customersView;

            _ApplyFilter();   // يحترم نص البحث المكتوب قبل التحديث
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة العملاء");
            clsDialogs.Error("تعذر تحميل قائمة العملاء. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = dgvCustomers.Cursor = Cursors.Default;
            _UpdateButtonsState();
        }
    }

    // ── البحث الفوري (RULE-N24/RULE-N59/RULE-N60) ────────────────────────────

    private void txtSearch_TextChanged(object sender, EventArgs e) => _ApplyFilter();

    /// <summary>Enter في حقل البحث = اختيار أفضل نتيجة فوراً (RULE-N42/RULE-N47).</summary>
    private void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;
        _SelectCurrent();
    }

    private void _ApplyFilter()
    {
        if (_customersView is null) return;

        string search = txtSearch.Text.Trim();

        // القيمة تُهرَّب عبر RowFilterHelper — لا إدراج خام في RowFilter.
        _customersView.RowFilter = string.IsNullOrEmpty(search)
            ? string.Empty
            : RowFilterHelper.Like("Name", search) + " OR " + RowFilterHelper.Like("Phone", search);

        lblNoData.Visible = _customersView.Count == 0;
        _UpdateButtonsState();
    }

    // ── الاختيار ─────────────────────────────────────────────────────────────

    private void dgvCustomers_SelectionChanged(object sender, EventArgs e) => _UpdateButtonsState();

    private void dgvCustomers_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;   // يمنع تنفيذ AcceptButton مرة ثانية
        _SelectCurrent();
    }

    private void dgvCustomers_DoubleClick(object sender, EventArgs e) => _SelectCurrent();

    private void btnSelect_Click(object sender, EventArgs e) => _SelectCurrent();

    /// <summary>آلة حالة الأزرار (RULE-N43).</summary>
    private void _UpdateButtonsState()
    {
        btnSelect.Enabled = dgvCustomers.CurrentRow is not null && dgvCustomers.Rows.Count > 0;
    }

    private void _SelectCurrent()
    {
        if (dgvCustomers.CurrentRow?.DataBoundItem is not DataRowView row) return;

        int id = row["Id"] == DBNull.Value ? 0 : Convert.ToInt32(row["Id"]);
        if (id <= 0) return;

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
