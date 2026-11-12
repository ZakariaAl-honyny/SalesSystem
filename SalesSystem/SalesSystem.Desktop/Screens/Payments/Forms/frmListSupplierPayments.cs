using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Screens.Payments;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Payments.Forms;

public partial class frmListSupplierPayments : Form
{
    private readonly IServiceProvider _serviceProvider;
    private DataTable _supplierTable;
    private DataView _supplierView;

    public frmListSupplierPayments(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);                                        // RULE-N40
        KeyboardHelper.WireSelectAllOnFocus(this);                                  // RULE-N41
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                   // RULE-N45
        _supplierTable = new DataTable();
        _supplierView = new DataView();
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvSupplierPayments);

        // RULE-N22: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه (RULE-N52).
        dgvSupplierPayments.ReadOnly = true;
        dgvSupplierPayments.AllowUserToAddRows = false;
        dgvSupplierPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSupplierPayments.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    private void _SetupGrid()
    {
        dgvSupplierPayments.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",             Header = "رقم السند",     Width = 80 },
            new { Name = "SupplierName",   Header = "اسم المورد",    Width = 200 },
            new { Name = "Amount",         Header = "المبلغ",        Width = 120 },
            new { Name = "PaymentDate",    Header = "التاريخ",       Width = 110 },
            new { Name = "PaymentMethod",  Header = "طريقة الدفع",   Width = 120 },
            new { Name = "ReferenceNo",    Header = "رقم المرجع",    Width = 100 },
        };

        foreach (var c in columns)
        {
            dgvSupplierPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = c.Name,
                HeaderText = c.Header,
                DataPropertyName = c.Name,
                Width = c.Width,
                ReadOnly = true,
            });
        }
    }

    private async void _RefreshPayments()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _supplierTable = new DataTable();
            _supplierTable.Columns.Add("Id", typeof(int));
            _supplierTable.Columns.Add("SupplierName", typeof(string));
            _supplierTable.Columns.Add("Amount", typeof(decimal));
            _supplierTable.Columns.Add("PaymentDate", typeof(string));
            _supplierTable.Columns.Add("PaymentMethod", typeof(string));
            _supplierTable.Columns.Add("ReferenceNo", typeof(string));

            var result = await _serviceProvider.GetRequiredService<ISupplierPaymentApiService>().GetAllAsync();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var p in result.Value)
                {
                    _supplierTable.Rows.Add(
                        p.Id,
                        p.SupplierName ?? "",
                        p.Amount,
                        p.PaymentDate.ToString("yyyy/MM/dd"),
                        p.PaymentTypeDisplay,
                        p.PaymentNo ?? "");
                }

                _supplierView = new DataView(_supplierTable);
                dgvSupplierPayments.DataSource = _supplierView;
                lblRecordsCount.Text = _supplierView.Count.ToString();
            }
            else
            {
                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل مدفوعات الموردين", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                dgvSupplierPayments.DataSource = null;
                lblRecordsCount.Text = "0";
            }
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "فشل تحميل مدفوعات الموردين في شاشة مدفوعات الموردين");
            MessageBox.Show("حدث خطأ أثناء تحميل المدفوعات.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void _ReloadForm()
    {
        _SetupGrid();
        _RefreshPayments();
    }

    private void frmListSupplierPayments_Load(object sender, EventArgs e)
    {
        _ReloadForm();
        ApplyGreenTheme();   // RULE-N50 — الأخير في مسار التحميل
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        _RefreshPayments();
    }

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        _serviceProvider.GetRequiredService<frmSupplierPaymentDialog>().ShowDialog();
        _RefreshPayments();
    }

    private void cmsSupplierPayments_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        bool hasRows = dgvSupplierPayments.Rows.Count > 0 && dgvSupplierPayments.CurrentRow != null;
        tsmiRefresh.Enabled = true;
        tsmiEdit.Enabled = hasRows;
        tsmiAdd.Enabled = true;
        cmsSupplierPayments.Enabled = hasRows;
    }

    private void tsmiRefresh_Click(object sender, EventArgs e)
    {
        _RefreshPayments();
    }

    private void tsmiEdit_Click(object sender, EventArgs e)
    {
        if (dgvSupplierPayments.CurrentRow == null) return;
        btnAddNew_Click(sender, e);
    }

    private void tsmiAdd_Click(object sender, EventArgs e)
    {
        btnAddNew_Click(sender, e);
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string filter = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(filter))
        {
            _supplierView.RowFilter = "";
        }
        else
        {
            _supplierView.RowFilter = "SupplierName LIKE '%" + filter.Replace("'", "''") + "%'";
        }
        lblRecordsCount.Text = _supplierView.Count.ToString();
    }
}
