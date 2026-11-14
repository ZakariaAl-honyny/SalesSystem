using System.Data;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Suppliers.Forms;

/// <summary>
/// النموذج المرجعي (Master Form) لدليل الموردين.
/// يجمع: بحث مباشر + شبكة + نموذج إدخال/تعديل في شاشة واحدة،
/// ويعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmSupplierMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private ISupplierDesktopService SupplierService => _serviceProvider.GetRequiredService<ISupplierDesktopService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, SupplierDto> _suppliersById = new();
    private DataTable _suppliersTable = new();
    private DataView _suppliersView = new();

    /// <summary>null = إضافة مورد جديد، &gt; 0 = تعديل مورد قائم.</summary>
    private int? _editId;

    /// <summary>حارس مؤقت: يمنع تنفيذ إجرائين غير متزامنين معاً ويُعطّل أزرار الإجراءات أثناءه.</summary>
    private bool _actionButtonsBusy;

    /// <summary>
    /// حالة الشاشة (آلة الحالة): تبدأ دائماً بوضع العرض <see cref="EntryMode.Browsing"/>
    /// حيث كل حقول الإدخال **معطّلة**، ولا تُفعَّل إلا بزر «جديد» أو باختيار سجل من الشبكة (RULE-N149).
    /// </summary>
    private EntryMode _mode = EntryMode.Browsing;

    /// <summary>نص شريط العنوان الأساسي، تُضاف إليه لاحقة الوضع الحالي.</summary>
    private const string FormTitle = "(( دليل الموردين ))";

    /// <summary>أوضاع شاشة الإدخال — آلة الحالة التي تحكم تفعيل الحقول والأزرار (RULE-N150).</summary>
    private enum EntryMode
    {
        /// <summary>عرض فقط: كل حقول الإدخال معطّلة حتى يضغط المستخدم «جديد» أو يحدّد سجلاً (RULE-N149).</summary>
        Browsing,

        /// <summary>إضافة مورد جديد: الحقول مفعّلة وزر «إضافة» متاح فقط.</summary>
        Adding,

        /// <summary>تعديل مورد محدد: الحقول مفعّلة وأزرار «تعديل/حذف/إعادة التنشيط» حسب حالة السجل.</summary>
        Editing
    }

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmSupplierMaster()
    {
        InitializeComponent();
    }

    public frmSupplierMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this); // أسهم RTL: يمين=رجوع، يسار=تالي

        // أزرار الوضع (إضافة/تعديل/حذف/إعادة تنشيط) تتغير حالة تفعيلها حسب السجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل (RULE-N43/N152).
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnReactivate, btnClearFilters);

        // الاختصارات الموحّدة (RULE-N47): F2 جديد · F5 تحديث · Delete تعطيل · Esc إلغاء/مسح التصفية.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, _GlobalDelete),
            (Keys.Escape, _GlobalEscape));

        // Enter = الزر الأساسي للوضع الحالي: عرض ⇒ «جديد» · إضافة ⇒ «إضافة/حفظ» · تعديل ⇒ «تعديل» (RULE-N147).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // RULE-N97: التفقيط الحيّ (تلميح) على الحقل المالي — حد الائتمان.
        LiveTafqeetHelper.Attach(liveTip,
            (numCreditLimit, "حد الائتمان", LiveTafqeetHelper.ReadMode.Money));

        // RULE-N60: أدوات التصفية تُقرأ بالمؤشر — «الحالة: الكل» و«تصفية بـ: الاسم» افتراضياً.
        cbStatus.SelectedIndex = 0;
        cbFilterBy.SelectedIndex = 0;

        // RULE-N63/RULE-N145: يتبنّى btnToggleFilters المُعلَن في المصمّم — آخر سطر دائماً.
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    // ── Global Shortcuts ──────────────────────────────────────────────────────

    /// <summary>Delete: تعطيل السجل المحدد — يُتجاهل أثناء الكتابة داخل أي حقل إدخال.</summary>
    private void _GlobalDelete()
    {
        if (_IsTextInputFocused()) return;
        if (btnDelete.Enabled) btnDelete.PerformClick();
    }

    /// <summary>Esc: مسح كل التصفية إن كان المؤشر داخل صندوق التصفية، وإلا إلغاء النموذج (RULE-N47).</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        _ResetForm();
    }

    /// <summary>هل المؤشر داخل حقل إدخال؟ (لئلا يُنفَّذ اختصار Delete أثناء الكتابة).</summary>
    private bool _IsTextInputFocused()
    {
        foreach (Control input in new Control[]
                 {
                     txtSearch, cbFilterBy, cbStatus, txtName, txtPhone, txtEmail,
                     txtTaxNumber, txtAddress, txtNotes, numCreditLimit
                 })
        {
            if (input.Focused) return true;
        }

        return false;
    }

    // ── Theme ─────────────────────────────────────────────────────────────────

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvSuppliers);
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;   // RULE-N53

        // RULE-N22/RULE-N52: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        dgvSuppliers.ReadOnly = true;
        dgvSuppliers.AllowUserToAddRows = false;
        dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSuppliers.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// يعيد <c>false</c> إذا كان هناك حقل مطلوب ناقص.
    /// </summary>
    private bool _FocusFirstMissingField(bool edit)
    {
        _ = edit; // الحقول المطلوبة متطابقة في وضعي الإضافة والتعديل.

        errorProvider.SetError(txtName, "");

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المورد مطلوب");
            txtName.Focus();
            return false;
        }

        return true;
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← الموردون ← المحرر ووضع العرض. لا يُستخدم لمسح التصفية وحده.
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();        // التصفية تعود للافتراضي
            await _LoadSuppliersAsync(); // إعادة قراءة الموردين (يعيد تطبيق التصفية والملخّص)
            _ResetForm();                // تفريغ المحرر + وضع العرض + الحقول معطّلة
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث البيانات");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmSupplierMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadSuppliersAsync();
            _ResetForm();
            ApplyGreenTheme();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل الموردين");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadSuppliersAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Phone", typeof(string));
        table.Columns.Add("Email", typeof(string));
        table.Columns.Add("TaxNumber", typeof(string));
        table.Columns.Add("CreditLimit", typeof(decimal));
        table.Columns.Add("CurrentBalance", typeof(decimal));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await SupplierService.GetAllAsync(includeInactive: true);

            _suppliersById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var s in result.Value)
                {
                    _suppliersById[s.Id] = s;
                    table.Rows.Add(
                        s.Id,
                        s.Name,
                        s.Phone ?? "",
                        s.Email ?? "",
                        s.TaxNumber ?? "",
                        s.CreditLimit,
                        s.CurrentBalance,
                        s.IsActive);
                }
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة الموردين.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الموردين");
            clsDialogs.Error("تعذر تحميل قائمة الموردين. يرجى إعادة المحاولة.");
        }
        finally
        {
            _suppliersTable = table;
            _suppliersView = new DataView(_suppliersTable);
            dgvSuppliers.DataSource = _suppliersView;
            _ApplyGridHeaders();
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    /// <summary>
    /// عناوين أعمدة الشبكة بالعربية.
    /// لا يستخدم DataGridView عنوان <c>DataColumn.Caption</c> عند الربط بـ <see cref="DataView"/>،
    /// لذا تُضبط <c>HeaderText</c> صراحةً بعد إسناد <c>DataSource</c>.
    /// </summary>
    private void _ApplyGridHeaders()
    {
        foreach (DataGridViewColumn column in dgvSuppliers.Columns)
        {
            column.HeaderText = column.DataPropertyName switch
            {
                "Id" => "رقم المورد",
                "Name" => "اسم المورد",
                "Phone" => "الهاتف",
                "Email" => "البريد الإلكتروني",
                "TaxNumber" => "الرقم الضريبي",
                "CreditLimit" => "حد الائتمان",
                "IsActive" => "نشط",
                _ => column.HeaderText
            };
        }

        // عمود الرصيد الحالي: يُستخدم في التصفية والملخص فقط ولا يُعرض في الشبكة (RULE-N60).
        if (dgvSuppliers.Columns["CurrentBalance"] is { } balanceColumn)
        {
            balanceColumn.HeaderText = "الرصيد الحالي";
            balanceColumn.Visible = false;
        }
    }

    private void _ApplyRowColoring()
    {
        if (!dgvSuppliers.Columns.Contains("IsActive")) return;

        foreach (DataGridViewRow row in dgvSuppliers.Rows)
        {
            if (row.Cells["IsActive"].Value is bool isActive && isActive) continue;

            row.DefaultCellStyle.BackColor = Color.MistyRose;
            row.DefaultCellStyle.ForeColor = Color.DarkRed;
        }
    }

    // ── Search / Filter (DataView.RowFilter) ──────────────────────────────────

    private void txtSearch_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkShowInactive_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyWithBalance_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyOverLimit_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    /// <summary>Enter يطبّق التصفية فوراً · Esc يمسح كل أدوات التصفية (RULE-N59).</summary>
    private void txtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            _ApplyFilter();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Escape)
        {
            _ClearAllFilters();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }

    /// <summary>
    /// يمنع إدخال علامات الترقيم (مثل ' و % و [ ]) التي تفسد فلتر البحث DataView.RowFilter.
    /// </summary>
    private void txtSearch_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (char.IsPunctuation(e.KeyChar))
        {
            e.Handled = true;
            clsInputFeedback.ShowRejectTooltip(txtSearch, "لا يُسمح بإدخال علامات الترقيم.");
        }
    }

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للمحرر (RULE-N148). «تحديث» هو المسؤول عن إعادة تحميل البيانات.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtSearch.Focus();
    }

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح» و«تحديث».</summary>
    private void _ClearFilterInputs()
    {
        txtSearch.Clear();
        cbStatus.SelectedIndex = 0;
        cbFilterBy.SelectedIndex = 0;
        chkShowInactive.Checked = false;
        chkOnlyWithBalance.Checked = false;
        chkOnlyOverLimit.Checked = false;
    }

    /// <summary>يمسح كل أدوات التصفية ويعيد الشبكة إلى حالتها الافتراضية (RULE-N59).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();

        _ApplyFilter();
    }

    /// <summary>RULE-N43/N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtSearch.Text)
            || cbFilterBy.SelectedIndex > 0
            || cbStatus.SelectedIndex > 0
            || chkShowInactive.Checked
            || chkOnlyWithBalance.Checked
            || chkOnlyOverLimit.Checked;
    }

    /// <summary>
    /// التصفية الفورية (RULE-N59/N60): كل الحالات تُبنى من أسماء أعمدة معروفة،
    /// والاختيار يُقرأ بالمؤشر لا بالنص العربي، والقيمة تُهرَّب قبل بنائها في RowFilter.
    /// </summary>
    private void _ApplyFilter()
    {
        if (_suppliersView is null || _suppliersView.Table is null) return;
        if (txtSearch is null || cbFilterBy is null || cbStatus is null
            || chkShowInactive is null || chkOnlyWithBalance is null || chkOnlyOverLimit is null)
            return;

        string text = txtSearch.Text.Trim();

        var conditions = new List<string>();

        // ── الحالة: الكل / نشط / معطّل (cbStatus بالمؤشر) ──
        switch (cbStatus.SelectedIndex)
        {
            case 1: // نشط
                conditions.Add("IsActive = true");
                break;

            case 2: // معطّل — يُظهر المعطّل حتى لو لم يُفعَّل «عرض غير النشط»
                conditions.Add("IsActive = false");
                break;

            default: // الكل — يبقى «عرض غير النشط» هو الحاكم كما كان
                if (!chkShowInactive.Checked)
                    conditions.Add("IsActive = true");
                break;
        }

        // ── البحث النصي: عمود واحد فقط يحدده cbFilterBy ──
        if (text.Length > 0)
            conditions.Add(RowFilterHelper.Like(_FilterColumn(), text));

        // ── مربعات الاختيار المالية ──
        if (chkOnlyWithBalance.Checked)
            conditions.Add("CurrentBalance <> 0");

        if (chkOnlyOverLimit.Checked)
            conditions.Add("CurrentBalance > CreditLimit");

        // RULE-N60: القيمة مُهرَّبة، وإن فشل بناء الصيغة تُترك التصفية السابقة كما هي (لا سقوط إلى «بلا تصفية»).
        try
        {
            _suppliersView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : "";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تطبيق تصفية دليل الموردين — أُبقيت التصفية السابقة كما هي");
        }

        _ApplyRowColoring();
        _UpdateSummary();
        _ToggleEmptyState();
        _UpdateClearFiltersState();   // RULE-N148: حالة زر «مسح التصفية» بعد كل تصفية
    }

    /// <summary>عمود البحث المقابل لاختيار «تصفية بـ» — أسماء أعمدة ثابتة داخل الكود (RULE-N60).</summary>
    private string _FilterColumn() => cbFilterBy.SelectedIndex switch
    {
        1 => "Phone",
        2 => "Email",
        3 => "TaxNumber",
        _ => "Name"
    };

    /// <summary>
    /// شريط الملخص (RULE-N61): المعروض من الإجمالي + النشط/المعطّل + إجمالي المستحقات،
    /// ويُعاد حسابه بعد كل تصفية وكل تحميل.
    /// </summary>
    private void _UpdateSummary()
    {
        if (lblSummary is null || _suppliersView?.Table is null) return;

        int total = _suppliersTable.Rows.Count;
        int shown = _suppliersView.Count;

        int active = 0, inactive = 0;
        decimal due = 0m;

        foreach (DataRowView row in _suppliersView)
        {
            if (row["IsActive"] is bool isActive && isActive) active++;
            else inactive++;

            decimal current = _AsDecimal(row["CurrentBalance"]);
            if (current != 0m) due += current;
        }

        lblSummary.Text =
            $"المعروض: {shown} من {total} — نشط: {active} · معطّل: {inactive} · " +
            $"المستحقات: {due:N2}";
    }

    private static decimal _AsDecimal(object? value) => value is decimal d ? d : 0m;

    // ── Grid Selection ────────────────────────────────────────────────────────

    private async void dgvSuppliers_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvSuppliers.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            await _FillFormAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار مورد من الشبكة");
            clsDialogs.Error("تعذر عرض بيانات المورد المحدد. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>RULE-N23: يتم تحديد السطر بالنقر بالزر الأيمن قبل فتح قائمة السياق.</summary>
    private async void dgvSuppliers_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right || e.RowIndex < 0 || e.ColumnIndex < 0) return;

        try
        {
            dgvSuppliers.CurrentCell = dgvSuppliers.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (dgvSuppliers.Rows[e.RowIndex].DataBoundItem is DataRowView row
                && row["Id"] is int id && id > 0)
            {
                await _FillFormAsync(id);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديد مورد من الشبكة بالزر الأيمن");
        }
    }

    /// <summary>RULE-N23: النقر المزدوج ينقل المؤشر إلى أول حقل قابل للتعديل في السجل المحدد.</summary>
    private void dgvSuppliers_DoubleClick(object? sender, EventArgs e)
    {
        if (_editId is null) return;

        txtName.Focus();
        txtName.SelectAll();
    }

    // ── Context Menu (RULE-N23) ───────────────────────────────────────────────

    /// <summary>RULE-N23: عناصر القائمة تستدعي نفس أزرار الشاشة — لا منطق مكرر.</summary>
    private void cmsEdit_Click(object? sender, EventArgs e)
    {
        if (btnEdit.Enabled) btnEdit.PerformClick();
    }

    private void cmsDelete_Click(object? sender, EventArgs e)
    {
        if (btnDelete.Enabled) btnDelete.PerformClick();
    }

    private void cmsReactivate_Click(object? sender, EventArgs e)
    {
        if (btnReactivate.Enabled) btnReactivate.PerformClick();
    }

    private void cmsRefresh_Click(object? sender, EventArgs e) => btnRefresh.PerformClick();

    private void cmsExport_Click(object? sender, EventArgs e) => _ExportGridToExcel();

    /// <summary>
    /// حالة عناصر القائمة عند الفتح (RULE-N43/RULE-N92): تُقرأ من السطر الحالي في الشبكة
    /// — التعديل لسجل محدد، والتعطيل للنشط فقط، وإعادة التنشيط للمعطّل فقط.
    /// </summary>
    private void cmsGrid_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        bool hasRow = dgvSuppliers.CurrentRow?.DataBoundItem is DataRowView;

        bool isActive = hasRow
            && dgvSuppliers.CurrentRow!.Cells["IsActive"].Value is bool flag
            && flag;

        cmsEdit.Enabled = hasRow;
        cmsDelete.Enabled = hasRow && isActive;
        cmsReactivate.Enabled = hasRow && !isActive;
        cmsRefresh.Enabled = true;
        cmsExport.Enabled = _suppliersView is { Count: > 0 };
    }

    /// <summary>تصدير الشبكة الحالية إلى Excel عبر <see cref="ExportHelper"/> الموحّد.</summary>
    private void _ExportGridToExcel()
    {
        if (_suppliersView is null || _suppliersView.Count == 0)
        {
            clsDialogs.Warning("لا توجد بيانات للتصدير.");
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "ملف Excel (*.xlsx)|*.xlsx",
            FileName = $"دليل الموردين {DateTime.Now:yyyy-MM-dd}.xlsx",
            Title = "تصدير دليل الموردين"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            ExportHelper.ExportToExcel(dgvSuppliers, "الموردون", dialog.FileName);

            clsDialogs.Success("تم تصدير البيانات بنجاح.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تصدير دليل الموردين إلى Excel");
            clsDialogs.Error("تعذر تصدير البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>RULE-N22: سطر «لا توجد بيانات» يُعرض فوق الشبكة عند فراغ نتيجة التصفية.</summary>
    private void _ToggleEmptyState()
    {
        if (lblEmptyState is null) return;

        bool isEmpty = _suppliersView?.Table is null || _suppliersView.Count == 0;

        lblEmptyState.Visible = isEmpty;

        if (isEmpty) lblEmptyState.BringToFront();
    }

    /// <summary>
    /// تعبئة المحرر بالسجل المحدد وتحويل الشاشة إلى وضع التعديل (RULE-N151)،
    /// ثم ضبط الحقول والأزرار وشريط الوضع عبر <see cref="_ApplyModeState"/>.
    /// </summary>
    private async Task _FillFormAsync(int supplierId)
    {
        if (!_suppliersById.TryGetValue(supplierId, out var s)) return;

        // تحديث البيانات من الخدمة، مع الاعتماد على الذاكرة المحلية عند التعذر
        try
        {
            var fresh = await SupplierService.GetByIdAsync(supplierId);
            if (fresh.IsSuccess && fresh.Value != null)
                s = fresh.Value;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "تعذر تحديث بيانات المورد {SupplierId} من الخدمة", supplierId);
        }

        _editId = s.Id;
        _mode = EntryMode.Editing;   // RULE-N151: اختيار سجل من الشبكة = وضع التعديل

        // ضمان أن حالة التنشيط المحسوبة للأزرار مأخوذة من أحدث نسخة من السجل.
        _suppliersById[s.Id] = s;

        txtId.Text = s.Id.ToString();
        txtName.Text = s.Name;
        txtPhone.Text = s.Phone ?? "";
        txtEmail.Text = s.Email ?? "";
        txtAddress.Text = s.Address ?? "";
        txtTaxNumber.Text = s.TaxNumber ?? "";
        numCreditLimit.Value = _Clamp(numCreditLimit, s.CreditLimit);
        txtNotes.Text = s.Notes ?? "";

        // بعد تعبئة الحقول: تفعيل الحقول + ضبط الأزرار + تحديث شريط الوضع (RULE-N150).
        _ApplyModeState();
    }

    // ── Mode state machine (RULE-N149…N152) ───────────────────────────────────

    /// <summary>
    /// آلة الحالة المركزية للشاشة: تفعيل/تعطيل حقول الإدخال، وحالة كل زر، ونص شريط الوضع،
    /// وزر Enter الافتراضي. تُستدعى بعد **كل** انتقال حالة:
    /// تحميل · «جديد» · اختيار سجل · حفظ ناجح · حذف/تعطيل · إعادة تنشيط · «تحديث» · Esc.
    /// </summary>
    private void _ApplyModeState()
    {
        bool editingSession = _mode != EntryMode.Browsing;

        // RULE-N149: في وضع العرض تبقى كل حقول الإدخال معطّلة (txtId للقراءة دائماً).
        _SetEditorEnabled(editingSession);

        // آلة حالة الأزرار (RULE-N43): «جديد/تحديث/خروج» دائماً متاحة.
        btnNew.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;

        btnSave.Enabled = _mode == EntryMode.Adding && !_actionButtonsBusy;
        btnEdit.Enabled = _mode == EntryMode.Editing && !_actionButtonsBusy;
        btnDelete.Enabled = _mode == EntryMode.Editing && _SelectedSupplierIsActive() && !_actionButtonsBusy;

        _UpdateReactivateButton();   // RULE-N92: إعادة التنشيط للسجل المعطّل فقط
        _UpdateModeHeader();         // RULE-N44

        // RULE-N147: Enter يُنفِّذ الزر الأساسي للوضع الحالي.
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147): عرض ⇒ «جديد» · إضافة ⇒ «إضافة/حفظ» · تعديل ⇒ «تعديل».
    /// يُقرأ عند كل ضغطة Enter وعند كل انتقال حالة.
    /// </summary>
    private Button? _PrimaryActionButton() => _mode switch
    {
        EntryMode.Adding => btnSave,
        EntryMode.Editing => btnEdit,
        _ => btnNew
    };

    /// <summary>هل السجل المحدد نشط؟ (التعطيل للنشط فقط — RULE-N92).</summary>
    private bool _SelectedSupplierIsActive()
        => _editId is int id && _suppliersById.TryGetValue(id, out var supplier) && supplier.IsActive;

    /// <summary>حقول المحرر التي تُعطَّل في وضع العرض وتُفعَّل عند «جديد» أو عند تحديد سجل.</summary>
    private Control[] _EditorInputs() => new Control[]
    {
        txtName, txtPhone, txtEmail, txtTaxNumber, txtAddress, txtNotes, numCreditLimit
    };

    /// <summary>تفعيل أو تعطيل كل حقول المحرر دفعة واحدة (txtId يبقى للقراءة دائماً).</summary>
    private void _SetEditorEnabled(bool enabled)
    {
        foreach (var input in _EditorInputs())
            input.Enabled = enabled;
    }

    /// <summary>
    /// شريط الوضع (RULE-N44): «وضع العرض» افتراضياً · «إضافة مورد جديد» · «تعديل مورد: &lt;الاسم&gt;».
    /// </summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _mode switch
        {
            EntryMode.Adding => $"{FormTitle} — إضافة مورد جديد",
            EntryMode.Editing => $"{FormTitle} — تعديل مورد: {txtName.Text.Trim()}",
            _ => $"{FormTitle} — وضع العرض"
        };
    }

    /// <summary>
    /// آلة حالة زر «إعادة تنشيط» (RULE-N92): مفعّل فقط عند تحديد سجل **غير نشط**،
    /// ومعطّل عند عدم وجود تحديد أو عند كون السجل نشطاً أو أثناء عملية جارية.
    /// </summary>
    private void _UpdateReactivateButton()
    {
        bool hasSelection = _editId.HasValue;
        bool isActive = hasSelection
            && _suppliersById.TryGetValue(_editId!.Value, out var supplier)
            && supplier.IsActive;

        btnReactivate.Enabled = hasSelection && !isActive && !_actionButtonsBusy;
    }

    // ── Reset / New entry ─────────────────────────────────────────────────────

    /// <summary>
    /// إعادة الشاشة إلى **وضع العرض** (الحالة الافتراضية): تفريغ الحقول، تعطيل كل حقول الإدخال،
    /// وإتاحة «جديد/تحديث/خروج» فقط، مع وضع التركيز على «جديد» (RULE-N149).
    /// </summary>
    private void _ResetForm()
    {
        _editId = null;
        _mode = EntryMode.Browsing;

        errorProvider.Clear();

        txtId.Clear();
        txtName.Clear();
        txtPhone.Clear();
        txtEmail.Clear();
        txtAddress.Clear();
        txtTaxNumber.Clear();
        txtNotes.Clear();
        numCreditLimit.Value = 0;

        dgvSuppliers.ClearSelection();

        _ApplyModeState();   // الحقول معطّلة + «جديد» فقط + شريط الوضع = وضع العرض
        btnNew.Focus();
    }

    /// <summary>
    /// بدء إدخال مورد جديد (زر «جديد» / F2): تفريغ الحقول ثم **تفعيلها** وتحويل شريط الوضع
    /// إلى «إضافة مورد جديد» (RULE-N151).
    /// </summary>
    private void _StartNewEntry()
    {
        _ResetForm();

        _mode = EntryMode.Adding;
        _ApplyModeState();

        txtName.Focus();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المورد مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (numCreditLimit.Value < 0)
        {
            errorProvider.SetError(numCreditLimit, "حد الائتمان يجب أن يكون قيمة موجبة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numCreditLimit, "");
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    // ── Buttons ───────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _StartNewEntry();

    /// <summary>زر الخروج: إغلاق شاشة دليل الموردين.</summary>
    private void btnClose_Click(object? sender, EventArgs e) => Close();

    /// <summary>زر الإضافة: يُضيف مورداً جديداً دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث المورد المحدد فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: true);

    /// <summary>
    /// حفظ المورد: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
    /// </summary>
    private async Task _SaveAsync(bool edit)
    {
        // التعديل بدون سجل محدد لا معنى له.
        if (edit && _editId is null) return;

        // الحقول المطلوبة الناقصة: نقل المؤشر إلى أول حقل ناقص مع أيقونة خطأ (الأزرار تبقى مفعّلة).
        if (!_FocusFirstMissingField(edit)) return;

        if (!_ValidateForm()) return;

        // رسالة تأكيد قبل التعديل (مثل رسالة تأكيد الحذف).
        if (edit)
        {
            string editName = txtName.Text.Trim();
            if (!clsDialogs.ConfirmEdit($"هل أنت متأكد من تعديل المورد \"{editName}\"؟"))
                return;
        }

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;   // حارس مؤقت: يمنع تكرار الإجراء ويُعطّل بقية أزرار الإجراءات
        _ApplyModeState();

        try
        {
            string name = txtName.Text.Trim();
            string? phone = _ToNullIfEmpty(txtPhone.Text);
            string? email = _ToNullIfEmpty(txtEmail.Text);
            string? address = _ToNullIfEmpty(txtAddress.Text);
            string? taxNumber = _ToNullIfEmpty(txtTaxNumber.Text);
            string? notes = _ToNullIfEmpty(txtNotes.Text);
            decimal creditLimit = numCreditLimit.Value;

            Result<SupplierDto> result;

            if (edit)
            {
                int id = _editId!.Value;

                // الحفاظ على حالة التنشيط الحالية للمورد أثناء التعديل (RULE-N90/N92)
                bool isActive = _suppliersById.TryGetValue(id, out var current) && current.IsActive;

                var request = new UpdateSupplierRequest(
                    name,
                    phone,
                    email,
                    address,
                    taxNumber,
                    notes,
                    creditLimit,
                    CategoryId: null,
                    IsActive: isActive);

                result = await SupplierService.UpdateAsync(id, request);
            }
            else
            {
                var request = new CreateSupplierRequest(
                    name,
                    phone,
                    email,
                    address,
                    taxNumber,
                    notes,
                    creditLimit);

                result = await SupplierService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                EventBus.Publish(new SupplierChangedMessage(result.Value.Id));

                clsDialogs.Success(edit ? "تم تعديل المورد بنجاح." : "تمت إضافة المورد بنجاح.");

                // بعد الحفظ الناجح (إضافة أو تعديل): يصبح السجل محفوظاً ومحدداً → وضع التعديل،
                // ثم تحديث القائمة وإعادة تعبئة الحقول من البيانات المحفوظة (تحديث تلقائي — RULE-N151).
                _editId = result.Value.Id;
                _mode = EntryMode.Editing;

                await _LoadSuppliersAsync();
                await _FillFormAsync(result.Value.Id);   // يعبّئ الحقول ويضبط الأزرار وشريط الوضع
                _SelectGridRow(result.Value.Id);
                _ApplyModeState();                       // ضمان الحالة حتى لو لم يُعثر على السطر
            }
            else
            {
                clsDialogs.Error($"لم يتم حفظ المورد.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ المورد");
            clsDialogs.Error("حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.");
        }
        finally
        {
            // استعادة الحالة الكاملة (حقول + أزرار + شريط الوضع) حسب الوضع الحالي للنموذج.
            _actionButtonsBusy = false;
            _ApplyModeState();

            Cursor = Cursors.Default;
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار مورد من القائمة أولاً.");
            return;
        }

        string name = txtName.Text.Trim();

        if (!clsDialogs.Confirm($"هل أنت متأكد من حذف (تعطيل) المورد \"{name}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _ApplyModeState();

        try
        {
            var result = await SupplierService.DeactivateAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم تعطيل المورد بنجاح.");

                EventBus.Publish(new SupplierChangedMessage(id));

                // بعد الحذف: الرجوع إلى وضع العرض (الحقول معطّلة) ثم تحديث القائمة تلقائياً.
                _ResetForm();
                await _LoadSuppliersAsync();
                _ApplyModeState();
            }
            else
            {
                clsDialogs.Error($"لم يتم تعطيل المورد.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل المورد {SupplierId}", id);
            clsDialogs.Error("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.");
        }
        finally
        {
            _actionButtonsBusy = false;
            _ApplyModeState();

            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// إعادة تنشيط المورد المعطَّل (RULE-N90…RULE-N94):
    /// الزر مفعّل فقط على سطر غير نشط، والتنفيذ داخل الخدمة (BLL) داخل معاملة.
    /// </summary>
    private async void btnReactivate_Click(object? sender, EventArgs e)
    {
        // حارس إعادة الدخول: لا تُنفَّذ عمليتان في وقت واحد.
        if (_actionButtonsBusy) return;

        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار مورد معطّل من القائمة أولاً.");
            return;
        }

        if (!_suppliersById.TryGetValue(id, out var supplier))
        {
            clsDialogs.Warning("تعذر العثور على بيانات المورد المحدد. يرجى تحديث القائمة.");
            return;
        }

        if (supplier.IsActive)
        {
            clsDialogs.Warning("المورد المحدد نشط بالفعل.");
            _UpdateReactivateButton();
            return;
        }

        if (!clsDialogs.Confirm($"هل تريد إعادة تنشيط المورد \"{supplier.Name}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _ApplyModeState();

        try
        {
            var result = await SupplierService.ReactivateAsync(id);

            if (result.IsSuccess)
            {
                Log.Information("تم إعادة تنشيط المورد {SupplierId}", id);

                EventBus.Publish(new SupplierChangedMessage(id));

                await _LoadSuppliersAsync();

                // RULE-N94: يبقى السطر المُعاد تنشيطه محدداً، وتُعاد تعبئة الحقول
                // من البيانات المحدَّثة في وضع التعديل (تحديث تلقائي).
                _editId = id;
                await _FillFormAsync(id);
                _SelectGridRow(id);
                _ApplyModeState();   // السجل نشط بعد إعادة التنشيط → زر إعادة التنشيط معطّل

                clsDialogs.Success("تم إعادة تنشيط المورد بنجاح.");
            }
            else
            {
                clsDialogs.Error($"تعذر إعادة تنشيط المورد.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إعادة تنشيط المورد {SupplierId}", id);
            clsDialogs.Error("حدث خطأ أثناء إعادة التنشيط. يرجى إعادة المحاولة.");
        }
        finally
        {
            _actionButtonsBusy = false;
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>تحديد السطر المطلوب في الشبكة وإظهاره (RULE-N94: يبقى السطر المتأثر محدداً ومرئياً).</summary>
    private void _SelectGridRow(int supplierId)
    {
        foreach (DataGridViewRow row in dgvSuppliers.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == supplierId)
            {
                row.Selected = true;

                // ضمان ظهور السطر داخل منطقة العرض — تُتجاهل السطور التي تخفيها التصفية الحالية.
                if (row.Visible)
                {
                    dgvSuppliers.CurrentCell = row.Cells["Name"];
                    dgvSuppliers.FirstDisplayedScrollingRowIndex = row.Index;
                }

                break;
            }
        }
    }

    private static string? _ToNullIfEmpty(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static decimal _Clamp(NumericUpDown control, decimal value)
        => Math.Min(Math.Max(value, control.Minimum), control.Maximum);
}
