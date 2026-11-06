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

namespace SalesSystem.Desktop.Screens.Categories.Forms;

/// <summary>
/// نموذج رئيسي (Master Form) لدليل مجموعات الأصناف.
/// إضافة/تعديل/تعطيل في شاشة واحدة، ويعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmCategoryMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private ICategoryApiService CategoryService => _serviceProvider.GetRequiredService<ICategoryApiService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, CategoryDto> _categoriesById = new();
    private DataTable _categoriesTable = new();
    private DataView _categoriesView = new();

    /// <summary>null = إضافة مجموعة جديدة، &gt; 0 = تعديل مجموعة قائمة.</summary>
    private int? _editId;

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmCategoryMaster()
    {
        InitializeComponent();
    }

    public frmCategoryMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this); // أسهم RTL: يمين=رجوع، يسار=تالي

        // أزرار الوضع (إضافة/تعديل/حذف) تتغير حالة تفعيلها حسب السجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete);

        _WireEvents();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmCategoryMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnEdit.Click += btnEdit_Click;
        btnDelete.Click += btnDelete_Click;
        btnRefresh.Click += btnRefresh_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvCategories.CellClick += dgvCategories_CellClick;
    }

    /// <summary>هل الحقل المطلوب (اسم المجموعة) مكتمل؟</summary>
    private bool _RequiredFieldsValid()
        => !string.IsNullOrWhiteSpace(txtName.Text);

    /// <summary>أسماء الحقول المطلوبة الناقصة (اسم المجموعة).</summary>
    private List<string> _MissingRequiredNames()
    {
        var missing = new List<string>();

        if (string.IsNullOrWhiteSpace(txtName.Text))
            missing.Add("اسم المجموعة");

        return missing;
    }

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// </summary>
    private void _FocusFirstMissingField()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المجموعة مطلوب");
            txtName.Focus();
        }
    }

    /// <summary>تحديث يدوي لقائمة المجموعات مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadCategoriesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث البيانات");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmCategoryMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadCategoriesAsync();
            _ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل مجموعات الأصناف");
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadCategoriesAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Description", typeof(string));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await CategoryService.GetAllAsync(includeInactive: true);

            _categoriesById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var c in result.Value)
                {
                    _categoriesById[c.Id] = c;
                    table.Rows.Add(c.Id, c.Name, c.Description ?? "", c.IsActive);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة مجموعات الأصناف.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة مجموعات الأصناف");
            MessageBox.Show("تعذر تحميل قائمة مجموعات الأصناف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _categoriesTable = table;
            _categoriesView = new DataView(_categoriesTable);
            dgvCategories.DataSource = _categoriesView;
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvCategories.Rows)
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
        if (_categoriesView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        if (string.IsNullOrEmpty(text))
        {
            _categoriesView.RowFilter = "";
            return;
        }

        _categoriesView.RowFilter =
            $"Name LIKE '%{text}%' OR Description LIKE '%{text}%' " +
            $"OR CONVERT(Id, 'System.String') LIKE '{text}%'";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private void dgvCategories_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvCategories.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            _FillForm(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار مجموعة أصناف من الشبكة");
            MessageBox.Show("تعذر عرض بيانات المجموعة المحددة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void _FillForm(int categoryId)
    {
        if (!_categoriesById.TryGetValue(categoryId, out var c)) return;

        _editId = c.Id;

        // وضع التعديل: زر التعديل يُحدّث السجل المحدد، وزر الإضافة معطّل (السجل محدد)، وزر الحذف متاح.
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        btnDelete.Enabled = true;

        txtCode.Text = c.Id.ToString();
        txtName.Text = c.Name;
        txtDescription.Text = c.Description ?? "";

        errorProvider.Clear();
        _UpdateModeHeader();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( دليل مجموعات الأصناف )) — تعديل مجموعة"
            : "(( دليل مجموعات الأصناف )) — إضافة مجموعة جديدة";

    private void _ResetForm()
    {
        _editId = null;

        // وضع الإضافة: زر الإضافة متاح، وزرّي التعديل والحذف معطّلان لعدم وجود سجل محدد.
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        btnDelete.Enabled = false;

        errorProvider.Clear();

        txtCode.Clear();
        txtName.Clear();
        txtDescription.Clear();

        dgvCategories.ClearSelection();
        _UpdateModeHeader();
        txtName.Focus();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المجموعة مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (txtName.Text.Trim().Length > 100)
        {
            errorProvider.SetError(txtName, "اسم المجموعة أطول من المسموح (100 حرف)");
            isValid = false;
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

    /// <summary>زر الإضافة: يُضيف مجموعة جديدة دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveCategoryAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث المجموعة المحددة فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveCategoryAsync(edit: true);

    /// <summary>
    /// حفظ المجموعة: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
    /// </summary>
    private async Task _SaveCategoryAsync(bool edit)
    {
        // التعديل بدون سجل محدد لا معنى له.
        if (edit && _editId is null) return;

        // الحقل المطلوب الناقص: نقل المؤشر إلى الحقل مع أيقونة خطأ (الأزرار تبقى مفعّلة).
        if (!_RequiredFieldsValid())
        {
            _FocusFirstMissingField();

            var missing = string.Join("، ", _MissingRequiredNames());
            MessageBox.Show("يرجى تعبئة الحقول المطلوبة: " + missing, "بيانات ناقصة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!_ValidateForm()) return;

        // رسالة تأكيد قبل التعديل (مثل رسالة تأكيد الحذف).
        if (edit)
        {
            string editName = txtName.Text.Trim();
            if (MessageBox.Show($"هل أنت متأكد من تعديل المجموعة \"{editName}\"؟",
                    "تأكيد التعديل", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                return;
        }

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;
        btnEdit.Enabled = false;

        try
        {
            string name = txtName.Text.Trim();
            string? description = string.IsNullOrWhiteSpace(txtDescription.Text)
                ? null
                : txtDescription.Text.Trim();

            Result<CategoryDto> result;

            if (edit)
            {
                int id = _editId!.Value;

                // الحفاظ على حالة التنشيط الحالية للسجل أثناء التعديل
                bool isActive = _categoriesById.TryGetValue(id, out var current) && current.IsActive;

                var request = new UpdateCategoryRequest(name, description, isActive);
                result = await CategoryService.UpdateAsync(id, request);
            }
            else
            {
                var request = new CreateCategoryRequest(name, description);
                result = await CategoryService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                EventBus.Publish(new CategoryChangedMessage(result.Value.Id));

                MessageBox.Show(edit ? "تم تعديل المجموعة بنجاح." : "تمت إضافة المجموعة بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // بعد الحفظ الناجح يصبح السجل محدداً → وضع التعديل
                // (زر الإضافة معطّل، وزرّا التعديل والحذف متاحان). النصوص ثابتة في المصمم.
                _editId = result.Value.Id;
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

                await _LoadCategoriesAsync();
                _SelectGridRow(result.Value.Id);
                _UpdateModeHeader();
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ المجموعة.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ مجموعة الأصناف");
            MessageBox.Show("حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // استعادة حالة الأزرار حسب الوضع الحالي للنموذج.
            if (_editId is null)
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
            }
            else
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
            }

            Cursor = Cursors.Default;
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            MessageBox.Show("يرجى اختيار مجموعة من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) المجموعة \"{name}\"؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await CategoryService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل المجموعة بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventBus.Publish(new CategoryChangedMessage(id));
                _ResetForm();
                await _LoadCategoriesAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل المجموعة.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل مجموعة الأصناف {CategoryId}", id);
            MessageBox.Show("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // بعد الحذف الناجح يعود النموذج إلى وضع الإضافة → يبقى زرّا الحذف والتعديل معطّلين.
            btnDelete.Enabled = _editId.HasValue;
            btnEdit.Enabled = _editId.HasValue;
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int categoryId)
    {
        foreach (DataGridViewRow row in dgvCategories.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == categoryId)
            {
                row.Selected = true;
                dgvCategories.CurrentCell = row.Cells["Name"];
                break;
            }
        }
    }

    private void pnlFields_Paint(object sender, PaintEventArgs e)
    {

    }
}
