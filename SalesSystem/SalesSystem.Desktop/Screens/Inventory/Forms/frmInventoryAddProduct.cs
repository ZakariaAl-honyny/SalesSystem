using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// UI SHAPE — "إضافة صنف" (Add Product) dialog.
/// Mock data only (in-memory). No backend / API calls yet.
/// Demonstrates: multi-unit + multi-barcode grid, price/cost per unit,
/// expiry tracking toggle and opening-stock (FIFO/FEFO) block.
/// </summary>
public partial class frmInventoryAddProduct : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── Mock data (in-memory, Arabic) ─────────────────────────────────────────────
    private readonly string[] _categories = { "مواد غذائية", "مشروبات", "تنظيف", "أخرى" };
    private readonly string[] _warehouses = { "المستودع الرئيسي", "مخزن التجزئة", "مخزن الجملة" };

    public frmInventoryAddProduct(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    private void frmInventoryAddProduct_Load(object sender, EventArgs e)
    {
        _LoadMockData();
    }

    /// <summary>
    /// Populates the mock lists and seeds one default unit row so the
    /// multi-unit structure is immediately visible in the UI shape.
    /// </summary>
    private void _LoadMockData()
    {
        cmbCategory.Items.AddRange(_categories);
        cmbCategory.SelectedIndex = 0;

        cmbWarehouse.Items.AddRange(_warehouses);
        cmbWarehouse.SelectedIndex = 0;

        dtpExpiry.Value = DateTime.Today;
        dtpExpiry.Enabled = chkTrackExpiry.Checked;

        int rowIndex = dgvUnits.Rows.Add();
        dgvUnits.Rows[rowIndex].Cells["colUnit"].Value = "كيس";
        dgvUnits.Rows[rowIndex].Cells["colFactor"].Value = 1m;
        dgvUnits.Rows[rowIndex].Cells["colBarcode"].Value = "";
        dgvUnits.Rows[rowIndex].Cells["colSellPrice"].Value = 0m;
        dgvUnits.Rows[rowIndex].Cells["colCost"].Value = 0m;
    }

    private void chkTrackExpiry_CheckedChanged(object sender, EventArgs e)
    {
        dtpExpiry.Enabled = chkTrackExpiry.Checked;
        if (chkTrackExpiry.Checked)
            dtpExpiry.Value = DateTime.Today;
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الصنف مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (string.IsNullOrWhiteSpace(txtBarcode.Text))
        {
            errorProvider.SetError(txtBarcode, "الباركود مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtBarcode, "");
        }

        bool hasUnitRow = false;
        foreach (DataGridViewRow row in dgvUnits.Rows)
        {
            if (row.IsNewRow) continue;
            string? unit = row.Cells["colUnit"].Value as string;
            if (!string.IsNullOrWhiteSpace(unit))
            {
                hasUnitRow = true;
                break;
            }
        }

        if (!hasUnitRow)
        {
            errorProvider.SetError(dgvUnits, "يجب إضافة وحدة واحدة على الأقل");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(dgvUnits, "");
        }

        if (nudOpeningQty.Value < 0)
        {
            errorProvider.SetError(nudOpeningQty, "الكمية الافتتاحية يجب أن تكون أكبر من أو تساوي صفر");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(nudOpeningQty, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        try
        {
            Cursor = Cursors.WaitCursor;

            // Mock-only: no backend call yet.
            // Collected shape: txtName, txtBarcode, cmbCategory, nudReorderLevel,
            // chkTrackExpiry + dgvUnits rows (الوحدة / معامل التحويل / الباركود /
            // سعر البيع / التكلفة) + cmbWarehouse, nudOpeningQty, txtBatchNo, dtpExpiry.

            MessageBox.Show("تم إضافة الصنف بنجاح", "نجاح",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء إضافة الصنف");
            MessageBox.Show("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        Close();
    }
}