using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Employees.Forms;

public partial class frmListEmployees : Form
{
    private readonly IServiceProvider _serviceProvider;
    private DataTable _dtEmployees = new();
    private DataView _dvEmployees = new();

    private static readonly Dictionary<string, string> _filterColumns =
        new()
        {
            { "الاسم",   "Name" },
            { "المسمى",  "JobTitle" },
        };

    public frmListEmployees(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvEmployees.Columns.Clear();

        var columns = new[]
        {
            new { Name = "EmployeeID", Header = "رقم الموظف",  Fill = 6,  Min = 90  },
            new { Name = "Name",       Header = "الاسم",       Fill = 20, Min = 180 },
            new { Name = "JobTitle",   Header = "المسمى الوظيفي", Fill = 14, Min = 130 },
            new { Name = "Phone",      Header = "الهاتف",      Fill = 11, Min = 110 },
            new { Name = "BaseSalary", Header = "الراتب الأساسي", Fill = 12, Min = 120 },
            new { Name = "Status",     Header = "الحالة",      Fill = 8,  Min = 80  },
        };

        foreach (var col in columns)
        {
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }

        if (dgvEmployees.Columns["BaseSalary"] is DataGridViewTextBoxColumn salaryCol)
        {
            salaryCol.DefaultCellStyle.Format = "N2";
        }
    }

    // ── Data Loading (in-memory mock — UI shape only) ───────────────────────────

    private void _RefreshEmployeesList()
    {
        Cursor = dgvEmployees.Cursor = Cursors.WaitCursor;

        try
        {
            _dtEmployees = new DataTable();
            _dtEmployees.Columns.Add("EmployeeID", typeof(int));
            _dtEmployees.Columns.Add("Name", typeof(string));
            _dtEmployees.Columns.Add("JobTitle", typeof(string));
            _dtEmployees.Columns.Add("Phone", typeof(string));
            _dtEmployees.Columns.Add("BaseSalary", typeof(decimal));
            _dtEmployees.Columns.Add("Status", typeof(string));

            foreach (var emp in EmployeeMockStore.Employees)
            {
                _dtEmployees.Rows.Add(
                    emp.Id,
                    emp.Name,
                    emp.JobTitle,
                    emp.Phone,
                    emp.BaseSalary,
                    emp.IsActive ? "نشط" : "موقوف");
            }

            _dvEmployees = new DataView(_dtEmployees);
            dgvEmployees.DataSource = _dvEmployees;
            lblRecordsCount.Text = _dvEmployees.Count.ToString();
        }
        catch (Exception ex)
        {
            MessageBox.Show("خطأ في تحميل بيانات الموظفين: " + ex.Message, "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        Cursor = dgvEmployees.Cursor = Cursors.Default;
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshEmployeesList();
    }

    // ── Helpers ─────────────────────────────────────────────────────────────────

    private EmployeeMockData? _GetSelectedEmployee()
    {
        if (dgvEmployees.CurrentRow == null) return null;
        int id = Convert.ToInt32(dgvEmployees.CurrentRow.Cells["EmployeeID"].Value);
        return EmployeeMockStore.Employees.FirstOrDefault(e => e.Id == id);
    }

    private void _OpenEditor(EmployeeMockData? existing)
    {
        var editor = _serviceProvider.GetRequiredService<frmAddUpdateEmployee>();
        editor.LoadData(existing);
        if (editor.ShowDialog() == DialogResult.OK)
        {
            _RefreshEmployeesList();
        }
    }

    // ── Form Events ─────────────────────────────────────────────────────────────

    private void frmListEmployees_Load(object sender, EventArgs e)
    {
        _ReloadForm();
        cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيء");
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ── Filter Events ───────────────────────────────────────────────────────────

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFilterValue.Visible = cbFilterBy.Text != "لا شيء";
        if (txtFilterValue.Visible)
        {
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }
        _dvEmployees.RowFilter = "";
        lblRecordsCount.Text = _dvEmployees.Count.ToString();
    }

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtFilterValue.Text.Trim();

        if (string.IsNullOrEmpty(filterText) || !_filterColumns.TryGetValue(cbFilterBy.Text, out var columnName))
        {
            _dvEmployees.RowFilter = "";
        }
        else
        {
            _dvEmployees.RowFilter = string.Format("{0} LIKE '%{1}%'",
                columnName, filterText.Replace("'", "''"));
        }

        lblRecordsCount.Text = _dvEmployees.Count.ToString();
    }

    private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            return;
        }

        if (char.IsPunctuation(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    // ── Grid Events ─────────────────────────────────────────────────────────────

    private void dgvEmployees_DoubleClick(object sender, EventArgs e)
    {
        var emp = _GetSelectedEmployee();
        if (emp != null) _OpenEditor(emp);
    }

    // ── Button / Context Menu Events ────────────────────────────────────────────

    private void btnAdd_Click(object sender, EventArgs e)
        => _OpenEditor(null);

    private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        => btnAdd_Click(sender, e);

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var emp = _GetSelectedEmployee();
        if (emp != null) _OpenEditor(emp);
    }

    private void disableToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var emp = _GetSelectedEmployee();
        if (emp == null) return;

        var confirm = MessageBox.Show(
            $"هل تريد تعطيل الموظف «{emp.Name}»؟",
            "تأكيد التعطيل",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm == DialogResult.Yes)
        {
            emp.IsActive = false;
            _RefreshEmployeesList();
            MessageBox.Show("تم تعطيل الموظف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void cmsEmployees_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsEmployees.Enabled = dgvEmployees.Rows.Count > 0 && dgvEmployees.CurrentRow != null;
    }
}

// ── Mock Model + Store (UI shape only — replaced by DTO/API services later) ─────

public class EmployeeMockData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public decimal BaseSalary { get; set; }
    public decimal CommissionRate { get; set; }
    public DateTime HireDate { get; set; } = DateTime.Today;
    public bool IsActive { get; set; } = true;
    public string LinkedUser { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public static class EmployeeMockStore
{
    public static readonly List<EmployeeMockData> Employees = new()
    {
        new EmployeeMockData
        {
            Id = 1,
            Name = "أحمد محمد الحميري",
            JobTitle = "بائع",
            Phone = "777123456",
            NationalId = "01010101010",
            BaseSalary = 150000m,
            CommissionRate = 2m,
            HireDate = new DateTime(2022, 3, 1),
            IsActive = true,
            LinkedUser = "أحمد الحميري",
            Notes = "مسؤول مبيعات قسم الإلكترونيات.",
        },
        new EmployeeMockData
        {
            Id = 2,
            Name = "فاطمة علي الشرعبي",
            JobTitle = "كاشير",
            Phone = "771234567",
            NationalId = "02020202020",
            BaseSalary = 140000m,
            CommissionRate = 0m,
            HireDate = new DateTime(2021, 8, 15),
            IsActive = true,
            LinkedUser = "فاطمة الشرعبي",
            Notes = string.Empty,
        },
        new EmployeeMockData
        {
            Id = 3,
            Name = "خالد صالح العمراني",
            JobTitle = "مندوب",
            Phone = "733456789",
            NationalId = "03030303030",
            BaseSalary = 160000m,
            CommissionRate = 5m,
            HireDate = new DateTime(2023, 1, 10),
            IsActive = true,
            LinkedUser = "خالد العمراني",
            Notes = "مندوب مبيعات خارجي.",
        },
        new EmployeeMockData
        {
            Id = 4,
            Name = "سمير عبدالله المقطري",
            JobTitle = "سائق",
            Phone = "735567890",
            NationalId = "04040404040",
            BaseSalary = 120000m,
            CommissionRate = 0m,
            HireDate = new DateTime(2020, 5, 20),
            IsActive = false,
            LinkedUser = string.Empty,
            Notes = "إجازة غير مدفوعة.",
        },
        new EmployeeMockData
        {
            Id = 5,
            Name = "نبيل حسن الصبري",
            JobTitle = "عامل",
            Phone = "770112233",
            NationalId = "05050505050",
            BaseSalary = 100000m,
            CommissionRate = 0m,
            HireDate = new DateTime(2024, 2, 5),
            IsActive = true,
            LinkedUser = string.Empty,
            Notes = "عامل مخزن.",
        },
    };

    public static int NextId => Employees.Max(e => e.Id) + 1;
}