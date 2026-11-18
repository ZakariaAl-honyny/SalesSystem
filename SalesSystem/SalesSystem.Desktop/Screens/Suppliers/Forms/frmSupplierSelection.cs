using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using System.Data;
using Serilog;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Suppliers.Forms;

/// <summary>
/// منتقي المورد — شاشة اختيار للقراءة تُفتح نمطياً وتُرجع <see cref="SelectedSupplier"/>
/// مع <see cref="DialogResult.OK"/> (RULE-N09 · RULE-N100).
/// </summary>
public partial class frmSupplierSelection : Form
{
    private readonly IServiceProvider? _serviceProvider;

    private DataTable? _suppliersTable;
    private DataView? _suppliersView;

    /// <summary>سجلات الخدمة الحقيقية — مصدر <see cref="SelectedSupplier"/> (لا يُبنى المورد من نص الشبكة).</summary>
    private readonly List<SupplierDto> _suppliers = [];

    /// <summary>المورد المختار — العقد الذي تقرأه الشاشات المستدعية (لا يُغيَّر).</summary>
    public SupplierDto? SelectedSupplier { get; private set; }

    private ISupplierDesktopService Service
        => _serviceProvider!.GetRequiredService<ISupplierDesktopService>();

    public frmSupplierSelection()
    {
        InitializeComponent();

        // RULE-N40…RULE-N46 — تنقل وتحديد وتظليل الحقل النشط والأسهم.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor));   // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                    // RULE-N45
        KeyboardHelper.WireSearchToGrid(txtSearch, dgvSuppliers);                    // ↓ من البحث إلى الشبكة

        // RULE-N43: «تحديد» معطّل بلا سطر محدد + «مسح التصفية» معطّل بلا تصفية نشطة — بمظهر رمادي باهت.
        KeyboardHelper.WireButtonDisabledVisual(btnSelect, btnClearFilters);

        // RULE-N47: F5 تحديث. Enter/Esc يُديرهما AcceptButton/CancelButton ومعالج الشبكة — بلا تكرار.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F5, () => _LoadSuppliers()));

        _UpdateButtonsState();
    }

    public frmSupplierSelection(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // RULE-N63: طيّ/إظهار صندوق التصفية — يتبنّى btnToggleFilters المُعلَن في المصمّم (RULE-N145).
        // آخر سطر في المُنشئ (الأداة آمنة مع null).
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    private void frmSupplierSelection_Load(object sender, EventArgs e)
    {
        _LoadSuppliers();
        ApplyGreenTheme();

        // بعد الربط والثيم: ضبط ActiveControl هنا هو المسار الموثوق لبدء المؤشر في حقل التصفية،
        // لأن أول DataSource يُلغي أي تركيز سابق. الكتابة مقصودة — التحذير الموثَّق يخصّ **قراءة**
        // Form.ActiveControl كحارس (تُرجع الحاوية لا الحقل)، لا إسنادها.
        ActiveControl = txtSearch;   // المؤشر جاهز في حقل التصفية عند الفتح
        txtSearch.Focus();
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — ينتهي بـ Apply(this) دائماً.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);        // RULE-N53 — شريط عنوان أخضر بنص أبيض
        GreenThemeHelper.ApplyGrid(dgvSuppliers);     // RULE-N51

        // RULE-N22/RULE-N52: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        dgvSuppliers.ReadOnly = true;
        dgvSuppliers.AllowUserToAddRows = false;
        dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSuppliers.MultiSelect = false;

        lblRecordsCount.ForeColor = GreenThemeHelper.TextStrong;   // RULE-N53 — نص شريط الملخص

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ── التحميل ───────────────────────────────────────────────────────────────

    private async void _LoadSuppliers()
    {
        if (_serviceProvider is null) return;

        Cursor = dgvSuppliers.Cursor = Cursors.WaitCursor;
        try
        {
            var result = await Service.GetAllAsync();

            if (!result.IsSuccess || result.Value is null)
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة الموردين. يرجى المحاولة مرة أخرى.");
                return;
            }

            _suppliers.Clear();
            _suppliers.AddRange(result.Value);

            _suppliersTable = new DataTable();
            _suppliersTable.Columns.Add("Id", typeof(int));
            _suppliersTable.Columns.Add("Name", typeof(string));
            _suppliersTable.Columns.Add("Phone", typeof(string));
            _suppliersTable.Columns.Add("Email", typeof(string));

            foreach (var s in _suppliers)
                _suppliersTable.Rows.Add(s.Id, s.Name, s.Phone ?? string.Empty, s.Email ?? string.Empty);

            _suppliersView = new DataView(_suppliersTable);
            dgvSuppliers.DataSource = _suppliersView;

            _ApplyFilter();   // يحترم نص البحث المكتوب قبل التحديث
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الموردين");
            clsDialogs.Error("تعذر تحميل قائمة الموردين. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = dgvSuppliers.Cursor = Cursors.Default;
            _UpdateButtonsState();
        }
    }

    // ── البحث الفوري (RULE-N24/RULE-N59/RULE-N60) ────────────────────────────

    private void txtSearch_TextChanged(object sender, EventArgs e) => _ApplyFilter();

    /// <summary>
    /// «مسح التصفية» — يمسح أداة التصفية وحدها ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للسطر المحدد (RULE-N148). «تحديث» (F5) هو المسؤول عن إعادة التحميل.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        if (txtSearch.TextLength == 0) return;

        txtSearch.Clear();   // TextChanged ⇒ _ApplyFilter (تصفية فورية بلا تحميل)
        txtSearch.Focus();
    }

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
        // RULE-N148: «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة — يُعاد حسابه مع كل تغيير.
        btnClearFilters.Enabled = !string.IsNullOrWhiteSpace(txtSearch.Text);

        if (_suppliersView is null)
        {
            lblRecordsCount.Text = $"النتائج: {clsFormat.Number(0)}";
            return;
        }

        string search = txtSearch.Text.Trim();

        // القيمة تُهرَّب عبر RowFilterHelper — لا إدراج خام في RowFilter.
        _suppliersView.RowFilter = string.IsNullOrEmpty(search)
            ? string.Empty
            : RowFilterHelper.Like("Name", search) + " OR " + RowFilterHelper.Like("Phone", search);

        lblNoData.Visible = _suppliersView.Count == 0;
        lblRecordsCount.Text = $"النتائج: {clsFormat.Number(_suppliersView.Count)}";   // RULE-N61 — شريط الملخص
        _UpdateButtonsState();
    }

    // ── الاختيار ─────────────────────────────────────────────────────────────

    private void dgvSuppliers_SelectionChanged(object sender, EventArgs e) => _UpdateButtonsState();

    private void dgvSuppliers_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;   // يمنع تنفيذ AcceptButton مرة ثانية
        _SelectCurrent();
    }

    private void dgvSuppliers_DoubleClick(object sender, EventArgs e) => _SelectCurrent();

    private void btnSelect_Click(object sender, EventArgs e) => _SelectCurrent();

    /// <summary>آلة حالة الأزرار (RULE-N43).</summary>
    private void _UpdateButtonsState()
    {
        btnSelect.Enabled = dgvSuppliers.CurrentRow is not null && dgvSuppliers.Rows.Count > 0;
    }

    /// <summary>
    /// يُرجع <c>SupplierDto</c> الحقيقي القادم من الخدمة — لا نسخة مبنيّة من نص الشبكة،
    /// فيبقى حدّ الدين والرقم الضريبي كما هي في قاعدة البيانات.
    /// </summary>
    private void _SelectCurrent()
    {
        if (dgvSuppliers.CurrentRow?.DataBoundItem is not DataRowView row) return;

        if (row["Id"] is not int id || id <= 0) return;

        // لا يُبنى المورد من نص الشبكة: لو غاب سجل الخدمة نرفض التحديد برسالة عربية صريحة (لا نجاح كاذب).
        SupplierDto? supplier = _suppliers.Find(s => s.Id == id);
        if (supplier is null)
        {
            clsDialogs.Warning("تعذر جلب بيانات المورد المحدد. حدّث القائمة (F5) ثم أعد المحاولة.");
            return;
        }

        SelectedSupplier = supplier;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    // ── Context Menu (RULE-N23) ───────────────────────────────────────────────

    /// <summary>
    /// «اختيار» من قائمة السياق تستدعي نفس زر «تحديد» (نفس مسار النقر المزدوج/Enter)
    /// ولا تُنفَّذ إن كان الزر معطّلاً — بلا أي منطق مكرر (RULE-N23).
    /// </summary>
    private void cmsSelect_Click(object? sender, EventArgs e)
    {
        if (btnSelect.Enabled) btnSelect.PerformClick();
    }

    /// <summary>«تحديث» من قائمة السياق تستدعي نفس معالج F5 الموجود في الشاشة (RULE-N23/RULE-N47).</summary>
    private void cmsRefresh_Click(object? sender, EventArgs e) => _LoadSuppliers();

    /// <summary>تُلغى القائمة عند عدم وجود سطر محدد في الشبكة (RULE-N23).</summary>
    private void cmsRows_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (dgvSuppliers.CurrentRow?.DataBoundItem is not DataRowView)
        {
            e.Cancel = true;
            return;
        }

        cmsSelect.Enabled = btnSelect.Enabled;
    }
}
