using Microsoft.Extensions.DependencyInjection;
using System.Data;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Roles.Forms;

public partial class frmListRoles : Form
{
    private readonly IServiceProvider _serviceProvider;
    private DataTable _dtRoles = new();
    private DataView _dvRoles = new();

    private static readonly Dictionary<string, string> _filterColumns =
        new()
        {
            { "رقم الدور", "RoleID" },
            { "اسم الدور", "Name" },
        };

    public frmListRoles(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvRoles.Columns.Clear();

        var columns = new[]
        {
            new { Name = "RoleID",              Header = "رقم الدور",     Fill = 6,  Min = 60 },
            new { Name = "Name",                Header = "اسم الدور",     Fill = 14, Min = 150 },
            new { Name = "Description",         Header = "الوصف",         Fill = 16, Min = 160 },
            new { Name = "SalesPermissions",    Header = "صلاحيات المبيعات", Fill = 10, Min = 100 },
            new { Name = "InventoryPermissions",Header = "صلاحيات المخزون",  Fill = 10, Min = 100 },
            new { Name = "AccountingPermissions",Header = "صلاحيات المحاسبة", Fill = 10, Min = 100 },
            new { Name = "SystemPermissions",   Header = "صلاحيات النظام",   Fill = 10, Min = 100 },
            new { Name = "IsSystem",            Header = "نظام",          Fill = 5,  Min = 50 },
            new { Name = "IsActive",            Header = "نشط",           Fill = 5,  Min = 50 },
        };

        foreach (var col in columns)
        {
            dgvRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }
    }

    // ── Permission Display Helper ────────────────────────────────────────────────

    private string FormatPermissions(long permissions)
    {
        if (permissions == -1)
            return "صلاحيات كاملة";
        if (permissions == 0)
            return "---";
        return $"0x{permissions:X}";
    }

    // ── Data Loading ───────────────────────────────────────────────────────────

    private async void _RefreshRolesList()
    {
        Cursor = dgvRoles.Cursor = Cursors.WaitCursor;

        try
        {
            _dtRoles = new DataTable();
            _dtRoles.Columns.Add("RoleID", typeof(int));
            _dtRoles.Columns.Add("Name", typeof(string));
            _dtRoles.Columns.Add("Description", typeof(string));
            _dtRoles.Columns.Add("SalesPermissions", typeof(string));
            _dtRoles.Columns.Add("InventoryPermissions", typeof(string));
            _dtRoles.Columns.Add("AccountingPermissions", typeof(string));
            _dtRoles.Columns.Add("SystemPermissions", typeof(string));
            _dtRoles.Columns.Add("IsSystem", typeof(string));
            _dtRoles.Columns.Add("IsActive", typeof(string));

            var result = await _serviceProvider.GetRequiredService<IRoleApiService>().GetAllAsync(includeInactive: true);

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var r in result.Value.OrderByDescending(x => x.Id))
                {
                    _dtRoles.Rows.Add(
                        r.Id,
                        r.Name,
                        r.Description ?? "",
                        FormatPermissions(r.SalesPermissions),
                        FormatPermissions(r.InventoryPermissions),
                        FormatPermissions(r.AccountingPermissions),
                        FormatPermissions(r.SystemPermissions),
                        r.IsSystem ? "نعم" : "لا",
                        r.IsActive ? "نعم" : "لا"
                    );
                }
            }
            else
            {
                MessageBox.Show("خطأ في تحميل الأدوار: " + result.Error + result.IsSuccess , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _dvRoles = new DataView(_dtRoles);
            dgvRoles.DataSource = _dvRoles;
            lblRecordsCount.Text = _dvRoles.Count.ToString();
        }
        catch (Exception ex)
        {
            MessageBox.Show("خطأ في تحميل الأدوار: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = dgvRoles.Cursor = Cursors.Default;
        }
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshRolesList();
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private int _GetSelectedRoleId()
    {
        if (dgvRoles.CurrentRow == null) return -1;
        return Convert.ToInt32(dgvRoles.CurrentRow.Cells[0].Value);
    }

    // ── Form Events ────────────────────────────────────────────────────────────

    private void frmListRoles_Load(object sender, EventArgs e)
    {
        _ReloadForm();
        cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ── Filter Events ──────────────────────────────────────────────────────────

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFilterValue.Visible = cbFilterBy.Text != "لا شيئ";
        if (txtFilterValue.Visible)
        {
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }
        _dvRoles.RowFilter = "";
        lblRecordsCount.Text = _dvRoles.Count.ToString();
    }

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtFilterValue.Text.Trim();

        if (string.IsNullOrEmpty(filterText) || !_filterColumns.TryGetValue(cbFilterBy.Text, out var columnName))
        {
            _dvRoles.RowFilter = "";
        }
        else if (columnName == "RoleID")
        {
            _dvRoles.RowFilter = string.Format("CONVERT({0}, 'System.String') LIKE '{1}%'",
                columnName, filterText.Replace("'", "''"));
        }
        else
        {
            _dvRoles.RowFilter = string.Format("{0} LIKE '%{1}%'",
                columnName, filterText.Replace("'", "''"));
        }

        lblRecordsCount.Text = _dvRoles.Count.ToString();
    }

    private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            return;
        }

        if (cbFilterBy.Text == "رقم الدور" && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
        {
            e.Handled = true;
            clsInputFeedback.ShowRejectTooltip(txtFilterValue, "يُسمح فقط بإدخال الأرقام في هذا الحقل.");
        }

        if (char.IsPunctuation(e.KeyChar))
        {
            e.Handled = true;
            clsInputFeedback.ShowRejectTooltip(txtFilterValue, "يُسمح فقط بإدخال الأرقام أو أحرف.");
        }
    }

    // ── Grid Events ────────────────────────────────────────────────────────────

    private void dgvRoles_DoubleClick(object sender, EventArgs e)
        => editToolStripMenuItem_Click(sender, e);

    private void cmsRoles_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsRoles.Enabled = dgvRoles.Rows.Count > 0 && dgvRoles.CurrentRow != null;
    }

    // ── Toolbar / Button Events ────────────────────────────────────────────────

    private void btnAdd_Click(object sender, EventArgs e)
    {
        var editor = _serviceProvider.GetRequiredService<frmAddUpdateRole>();
        editor.ShowDialog();
        _ReloadForm();
    }

    private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        => btnAdd_Click(sender, e);

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedRoleId();
        if (id <= 0) return;

        var editor = _serviceProvider.GetRequiredService<frmAddUpdateRole>();
        editor._SetEditMode(id);
        editor.ShowDialog();
        _ReloadForm();
    }

    private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        => _ReloadForm();

    private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedRoleId();
        if (id <= 0) return;

        var row = _dtRoles.AsEnumerable()
            .FirstOrDefault(r => Convert.ToInt32(r["RoleID"]) == id);
        if (row != null && row["IsSystem"].ToString() == "نعم")
        {
            MessageBox.Show("لا يمكن حذف دور نظام — الدور محمي.", "محظور",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Cursor = dgvRoles.Cursor = Cursors.WaitCursor;

        try
        {
            var result = await _serviceProvider.GetRequiredService<IRoleApiService>().DeleteAsync(id);
            if (result.IsSuccess)
            {
                MessageBox.Show("تم إلغاء تنشيط الدور بنجاح", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في حذف الدور", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("خطأ في حذف الدور: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = dgvRoles.Cursor = Cursors.Default;
        }
    }
}
