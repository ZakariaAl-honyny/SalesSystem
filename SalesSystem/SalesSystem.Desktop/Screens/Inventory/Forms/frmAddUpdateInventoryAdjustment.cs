using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

public partial class frmAddUpdateInventoryAdjustment : Form
{
    private readonly IServiceProvider _serviceProvider;
    private int? _adjustmentId;
    private DataTable _linesTable = new();
    private DataView _linesView = new();
    private bool _isDraft;

    public frmAddUpdateInventoryAdjustment(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── Public Loader ──────────────────────────────────────────────────────────────

    public void LoadAdjustment(int adjustmentId)
    {
        _adjustmentId = adjustmentId;
        Text = "تعديل تسوية مخزون";
        lblMode.Text = "تعديل تسوية مخزون";
        _LoadAdjustmentData();
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────────

    private DataGridViewColumn _MakeColumn(string name, string header, int fill, int min, bool readOnly = true, string? format = null)
    {
        var col = new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = header,
            DataPropertyName = name,
            MinimumWidth = min,
            FillWeight = fill,
            ReadOnly = readOnly,
        };
        if (format is not null)
            col.DefaultCellStyle.Format = format;
        return col;
    }

    private void _SetupLinesGrid()
    {
        dgvLines.Columns.Clear();

        dgvLines.Columns.Add(_MakeColumn("ProductUnitName", "المنتج/الوحدة", 25, 200));
        dgvLines.Columns.Add(_MakeColumn("ExpectedQuantity", "الكمية المتوقعة", 15, 100, format: "N3"));
        dgvLines.Columns.Add(_MakeColumn("ActualQuantity", "الكمية الفعلية", 15, 100, format: "N3"));
        dgvLines.Columns.Add(_MakeColumn("UnitCost", "تكلفة الوحدة", 12, 100, format: "N2"));
        dgvLines.Columns.Add(_MakeColumn("Difference", "الفرق", 12, 100, format: "N3"));
    }

    // ── Data Loading ───────────────────────────────────────────────────────────────

    private void _LoadAdjustmentData()
    {
        if (_adjustmentId is null) return;

        var api = _serviceProvider.GetRequiredService<IInventoryAdjustmentApiService>();
        var result = api.GetByIdAsync(_adjustmentId.Value).GetAwaiter().GetResult();

        if (!result.IsSuccess || result.Value is null)
        {
            MessageBox.Show("تعذر تحميل بيانات التسوية", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var adj = result.Value;

        // Populate header fields
        txtAdjustmentNo.Text = adj.AdjustmentNo;
        cmbWarehouse.Text = adj.WarehouseName ?? "";
        cmbAdjustmentType.Text = adj.AdjustmentTypeName ?? "";
        txtReason.Text = adj.Reason ?? "";
        _isDraft = adj.Status == 1;

        btnPost.Enabled = _isDraft;

        // Load lines
        _linesTable = new DataTable();
        _linesTable.Columns.Add("ProductUnitName", typeof(string));
        _linesTable.Columns.Add("ExpectedQuantity", typeof(decimal));
        _linesTable.Columns.Add("ActualQuantity", typeof(decimal));
        _linesTable.Columns.Add("UnitCost", typeof(decimal));
        _linesTable.Columns.Add("Difference", typeof(decimal));

        if (adj.Lines is { Count: > 0 })
        {
            foreach (var line in adj.Lines)
            {
                decimal diff = line.ActualQuantity - line.ExpectedQuantity;
                _linesTable.Rows.Add(
                    line.ProductUnitName ?? "",
                    line.ExpectedQuantity,
                    line.ActualQuantity,
                    line.UnitCost,
                    Math.Round(diff, 3));
            }
        }

        _linesView = new DataView(_linesTable);
        dgvLines.DataSource = _linesView;
        lblLinesCount.Text = _linesView.Count.ToString();
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

    private void _LoadAdjustmentTypes()
    {
        var types = new List<KeyValuePair<byte, string>>
        {
            new(1, "إضافة"),
            new(2, "خصم"),
            new(3, "تصحيح"),
        };
        cmbAdjustmentType.DisplayMember = "Value";
        cmbAdjustmentType.ValueMember = "Key";
        cmbAdjustmentType.DataSource = types;
    }

    // ── Actions ────────────────────────────────────────────────────────────────────

    private void _SaveDraft()
    {
        if (!_ValidateForm()) return;

        var api = _serviceProvider.GetRequiredService<IInventoryAdjustmentApiService>();
        var request = new CreateInventoryAdjustmentRequest(
            WarehouseId: (short)(cmbWarehouse.SelectedValue ?? 0),
            AdjustmentType: (byte)(cmbAdjustmentType.SelectedValue ?? 1),
            Reason: txtReason.Text.Trim());

        var result = api.CreateAsync(request).GetAwaiter().GetResult();

        if (result.IsSuccess && result.Value is not null)
        {
            _adjustmentId = result.Value.Id;
            MessageBox.Show("تم حفظ مسودة التسوية بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnPost.Enabled = true;
        }
        else
        {
            MessageBox.Show($"خطأ في الحفظ: {result.Error}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void _PostAdjustment()
    {
        if (_adjustmentId is null)
        {
            _SaveDraft();
            if (_adjustmentId is null) return;
        }

        var api = _serviceProvider.GetRequiredService<IInventoryAdjustmentApiService>();
        var result = api.PostAsync(_adjustmentId.Value).GetAwaiter().GetResult();

        if (result.IsSuccess)
        {
            MessageBox.Show("تم ترحيل تسوية المخزون بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        if (cmbAdjustmentType.SelectedValue is null)
        {
            errorProvider.SetError(cmbAdjustmentType, "يرجى اختيار نوع التسوية");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbAdjustmentType, "");
        }

        if (string.IsNullOrWhiteSpace(txtReason.Text))
        {
            errorProvider.SetError(txtReason, "سبب التعديل مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtReason, "");
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

    // ── Form Events ────────────────────────────────────────────────────────────────

    private void frmInventoryAdjustmentEditor_Load(object sender, EventArgs e)
    {
        _LoadWarehouses();
        _LoadAdjustmentTypes();
        _SetupLinesGrid();

        if (_adjustmentId.HasValue)
            _LoadAdjustmentData();
    }

    private void btnSaveDraft_Click(object sender, EventArgs e)
    {
        _SaveDraft();
    }

    private void btnPost_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("هل أنت متأكد من ترحيل التسوية؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        _PostAdjustment();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}
