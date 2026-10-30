using System.Data;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Units.Forms;

/// <summary>
/// نموذج رئيسي (Master Form) لدليل الوحدات.
/// إضافة/تعديل/تعطيل في شاشة واحدة، ويعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmUnitMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IUnitApiService UnitService => _serviceProvider.GetRequiredService<IUnitApiService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, UnitDto> _unitsById = new();
    private DataTable _unitsTable = new();
    private DataView _unitsView = new();

    /// <summary>null = إضافة وحدة جديدة، &gt; 0 = تعديل وحدة قائمة.</summary>
    private int? _editId;

    public frmUnitMaster(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        _WireEvents();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmUnitMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvUnits.CellClick += dgvUnits_CellClick;
    }

    // ── Grid Setup ────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvUnits.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",       Header = "رقم الوحدة", Fill = 10, Min = 80  },
            new { Name = "Name",     Header = "اسم الوحدة", Fill = 50, Min = 220 },
            new { Name = "Symbol",   Header = "الرمز",      Fill = 25, Min = 120 },
            new { Name = "IsActive", Header = "نشط",        Fill = 10, Min = 60  },
        };

        foreach (var col in columns)
        {
            dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }

        ApplyDevColumns();
    }

    /// <summary>عرض أسماء أعمدة الشبكة — للتطوير فقط (وضع البيانات التجريبية).</summary>
    private void ApplyDevColumns()
    {
        if (SalesSystem.Desktop.Configuration.AppSettings.Current.UseMockData)
        {
            lblDevColumns.Text = "الأعمدة: " + string.Join(" | ", dgvUnits.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText));
            lblDevColumns.Visible = true;
        }
    }

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmUnitMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _SetupGridColumns();
            await _LoadUnitsAsync();
            _ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل الوحدات");
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadUnitsAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Symbol", typeof(string));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await UnitService.GetAllAsync(includeInactive: true);

            _unitsById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var u in result.Value)
                {
                    _unitsById[u.Id] = u;
                    table.Rows.Add(u.Id, u.Name, u.Symbol ?? "", u.IsActive);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة الوحدات.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الوحدات");
            MessageBox.Show("تعذر تحميل قائمة الوحدات. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _unitsTable = table;
            _unitsView = new DataView(_unitsTable);
            dgvUnits.DataSource = _unitsView;
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvUnits.Rows)
        {
            bool isActive = row.Cells["IsActive"].Value is bool b && b;
            if (!isActive)
            {
                row.DefaultCellStyle.BackColor = Color.MistyRose;
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
            }
        }
    }

    // ── Search / Filter (DataView.RowFilter) ──────────────────────────────────

    private void txtSearch_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void _ApplyFilter()
    {
        if (_unitsView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        if (string.IsNullOrEmpty(text))
        {
            _unitsView.RowFilter = "";
            return;
        }

        _unitsView.RowFilter =
            $"Name LIKE '%{text}%' OR Symbol LIKE '%{text}%' " +
            $"OR CONVERT(Id, 'System.String') LIKE '{text}%'";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private void dgvUnits_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvUnits.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            _FillForm(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار وحدة من الشبكة");
            MessageBox.Show("تعذر عرض بيانات الوحدة المحددة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void _FillForm(int unitId)
    {
        if (!_unitsById.TryGetValue(unitId, out var u)) return;

        _editId = u.Id;

        txtCode.Text = u.Id.ToString();
        txtName.Text = u.Name;
        txtSymbol.Text = u.Symbol ?? "";

        errorProvider.Clear();
        _UpdateModeHeader();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( دليل الوحدات )) — تعديل وحدة"
            : "(( دليل الوحدات )) — إضافة وحدة جديدة";

    private void _ResetForm()
    {
        _editId = null;

        errorProvider.Clear();

        txtCode.Clear();
        txtName.Clear();
        txtSymbol.Clear();

        dgvUnits.ClearSelection();
        _UpdateModeHeader();
        txtName.Focus();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الوحدة مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (txtName.Text.Trim().Length > 50)
        {
            errorProvider.SetError(txtName, "اسم الوحدة أطول من المسموح (50 حرفاً)");
            isValid = false;
        }

        if (txtSymbol.Text.Trim().Length > 20)
        {
            errorProvider.SetError(txtSymbol, "رمز الوحدة أطول من المسموح (20 حرفاً)");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtSymbol, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.",
                "بيانات غير مكتملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    // ── Buttons ───────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _ResetForm();

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        bool wasEditing = _editId.HasValue;
        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            string name = txtName.Text.Trim();
            string? symbol = string.IsNullOrWhiteSpace(txtSymbol.Text)
                ? null
                : txtSymbol.Text.Trim();

            Result<UnitDto> result;

            if (_editId is int id)
            {
                // الحفاظ على حالة التنشيط الحالية للسجل أثناء التعديل
                bool isActive = _unitsById.TryGetValue(id, out var current) && current.IsActive;

                var request = new UpdateUnitRequest(name, symbol, isActive);
                result = await UnitService.UpdateAsync(id, request);
            }
            else
            {
                var request = new CreateUnitRequest(name, symbol);
                result = await UnitService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                EventBus.Publish(new UnitChangedMessage(result.Value.Id));

                MessageBox.Show(wasEditing ? "تم تعديل الوحدة بنجاح." : "تمت إضافة الوحدة بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _editId = result.Value.Id;
                await _LoadUnitsAsync();
                _SelectGridRow(result.Value.Id);
                _UpdateModeHeader();
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ الوحدة.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ الوحدة");
            MessageBox.Show("حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            MessageBox.Show("يرجى اختيار وحدة من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) الوحدة \"{name}\"؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await UnitService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل الوحدة بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventBus.Publish(new UnitChangedMessage(id));
                _ResetForm();
                await _LoadUnitsAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل الوحدة.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل الوحدة {UnitId}", id);
            MessageBox.Show("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnDelete.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int unitId)
    {
        foreach (DataGridViewRow row in dgvUnits.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == unitId)
            {
                row.Selected = true;
                dgvUnits.CurrentCell = row.Cells["Name"];
                break;
            }
        }
    }
}
