using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

public partial class frmInventoryCountEditor : Form
{
    private readonly IServiceProvider _serviceProvider;
    private int? _countId;
    private bool _isDraft;

    public frmInventoryCountEditor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    public void LoadCount(int countId)
    {
        _countId = countId;
        Text = "تعديل جرد مخزون";
        lblMode.Text = "تعديل جرد مخزون";
        _LoadCountData();
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────────

    private DataGridViewColumn _MakeColumn(string name, string header, int fill, int min, bool readOnly = true, string? format = null)
    {
        var col = new DataGridViewTextBoxColumn
        {
            Name = name, HeaderText = header, DataPropertyName = name,
            MinimumWidth = min, FillWeight = fill, ReadOnly = readOnly,
        };
        if (format is not null) col.DefaultCellStyle.Format = format;
        return col;
    }

    private void _SetupGrid()
    {
        dgvLines.Columns.Clear();
        dgvLines.Columns.Add(_MakeColumn("ProductUnitName", "المنتج/الوحدة", 25, 200));
        dgvLines.Columns.Add(_MakeColumn("ExpectedQuantity", "الكمية المتوقعة", 15, 100, format: "N3"));
        dgvLines.Columns.Add(_MakeColumn("ActualQuantity", "الكمية الفعلية", 15, 100, format: "N3"));
        dgvLines.Columns.Add(_MakeColumn("Difference", "الفرق", 15, 100, format: "N3"));
        dgvLines.Columns.Add(_MakeColumn("Notes", "ملاحظات", 15, 150));
    }

    // ── Data Loading ───────────────────────────────────────────────────────────────

    private void _LoadCountData()
    {
        if (_countId is null) return;

        var api = _serviceProvider.GetRequiredService<IInventoryCountApiService>();
        var result = api.GetByIdAsync(_countId.Value).GetAwaiter().GetResult();

        if (!result.IsSuccess || result.Value is null)
        {
            MessageBox.Show("تعذر تحميل بيانات الجرد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var count = result.Value;
        txtCountNo.Text = count.CountNo;
        cmbWarehouse.Text = count.WarehouseName ?? "";
        txtNotes.Text = count.Notes ?? "";
        _isDraft = count.Status == 1;
        btnPost.Enabled = _isDraft;

        var linesTable = new DataTable();
        linesTable.Columns.Add("ProductUnitName", typeof(string));
        linesTable.Columns.Add("ExpectedQuantity", typeof(decimal));
        linesTable.Columns.Add("ActualQuantity", typeof(decimal));
        linesTable.Columns.Add("Difference", typeof(decimal));
        linesTable.Columns.Add("Notes", typeof(string));

        if (count.Lines is { Count: > 0 })
        {
            foreach (var line in count.Lines)
            {
                linesTable.Rows.Add(
                    line.ProductUnitName ?? "",
                    line.ExpectedQuantity,
                    line.ActualQuantity,
                    Math.Round(line.Difference, 3),
                    line.Notes ?? "");
            }
        }

        dgvLines.DataSource = new DataView(linesTable);
    }

    private void _LoadWarehouses()
    {
        var api = _serviceProvider.GetRequiredService<IWarehouseApiService>();
        var result = api.GetAllAsync().GetAwaiter().GetResult();
        if (result.IsSuccess && result.Value is { Count: > 0 })
        {
            cmbWarehouse.DisplayMember = "Name";
            cmbWarehouse.ValueMember = "Id";
            cmbWarehouse.DataSource = result.Value.ToList();
        }
    }

    // ── Save ───────────────────────────────────────────────────────────────────────

    private void _SaveDraft()
    {
        if (!_ValidateForm()) return;

        var api = _serviceProvider.GetRequiredService<IInventoryCountApiService>();
        var request = new CreateInventoryCountRequest(
            WarehouseId: (short)(cmbWarehouse.SelectedValue ?? 0),
            Notes: txtNotes.Text.Trim());

        var result = api.CreateAsync(request).GetAwaiter().GetResult();

        if (result.IsSuccess && result.Value is not null)
        {
            _countId = result.Value.Id;
            MessageBox.Show("تم حفظ مسودة الجرد بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnPost.Enabled = true;
        }
        else
        {
            MessageBox.Show($"خطأ في الحفظ: {result.Error}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void _PostCount()
    {
        if (_countId is null)
        {
            _SaveDraft();
            if (_countId is null) return;
        }

        var api = _serviceProvider.GetRequiredService<IInventoryCountApiService>();
        var result = api.PostAsync(_countId.Value).GetAwaiter().GetResult();

        if (result.IsSuccess)
        {
            MessageBox.Show("تم ترحيل الجرد بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            MessageBox.Show($"خطأ في الترحيل: {result.Error}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbWarehouse.SelectedValue is null || (short)cmbWarehouse.SelectedValue <= 0)
        {
            errorProvider.SetError(cmbWarehouse, "يرجى اختيار المستودع");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbWarehouse, "");
        }

        if (dgvLines.Rows.Count == 0)
        {
            errorProvider.SetError(dgvLines, "يجب إضافة صنف واحد على الأقل");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(dgvLines, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    // ── Events ─────────────────────────────────────────────────────────────────────

    private void frmInventoryCountEditor_Load(object sender, EventArgs e)
    {
        _LoadWarehouses();
        _SetupGrid();
        if (_countId.HasValue) _LoadCountData();
    }

    private void btnSaveDraft_Click(object sender, EventArgs e) { _SaveDraft(); }
    private void btnPost_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("هل أنت متأكد من ترحيل الجرد؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        _PostCount();
    }
    private void btnClose_Click(object sender, EventArgs e) { Close(); }
}
