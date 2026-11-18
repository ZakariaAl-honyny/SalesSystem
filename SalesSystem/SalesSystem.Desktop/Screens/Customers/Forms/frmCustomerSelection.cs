using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using System.Data;
using Serilog;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Screens.Customers.Forms;

/// <summary>
/// منتقي العميل — شاشة اختيار للقراءة تُفتح نمطياً وتُرجع <see cref="SelectedCustomer"/>
/// مع <see cref="DialogResult.OK"/> (RULE-N09 · RULE-N100).
/// </summary>
public partial class frmCustomerSelection : Form
{
    private readonly IServiceProvider? _serviceProvider;

    private DataTable? _customersTable;
    private DataView? _customersView;

    /// <summary>سجلات الخدمة الحقيقية — مصدر <see cref="SelectedCustomer"/> (لا يُبنى العميل من نص الشبكة).</summary>
    private readonly List<CustomerDto> _customers = [];

    /// <summary>العميل المختار — العقد الذي تقرأه الشاشات المستدعية (لا يُغيَّر).</summary>
    public CustomerDto? SelectedCustomer { get; private set; }

    private ICustomerDesktopService Service
        => _serviceProvider!.GetRequiredService<ICustomerDesktopService>();

    public frmCustomerSelection()
    {
        InitializeComponent();

        // RULE-N40…RULE-N46 — تنقل وتحديد وتظليل الحقل النشط والأسهم.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor));   // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                    // RULE-N45
        KeyboardHelper.WireSearchToGrid(txtSearch, dgvCustomers);                    // ↓ من البحث إلى الشبكة

        // RULE-N43: «تحديد» معطّل بلا سطر محدد، و«مسح التصفية» معطّل ما لم توجد تصفية نشطة
        // (RULE-N148) — رماديان باهتان عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSelect, btnClearFilters);

        // RULE-N47: F5 تحديث. Enter/Esc يُديرهما AcceptButton/CancelButton ومعالج الشبكة — بلا تكرار.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F5, () => _LoadCustomers()));

        _UpdateButtonsState();
    }

    public frmCustomerSelection(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // RULE-N63/RULE-N145: يتبنّى btnToggleFilters المُعلَن في المصمّم ويوصِّل طيّ التصفية + F9
        // وحفظ الحالة. آخر سطر في المُنشئ العام — الأداة null-safe (بلا حاوية يُلغى الحفظ فقط).
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    private void frmCustomerSelection_Load(object sender, EventArgs e)
    {
        _LoadCustomers();
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
        GreenThemeHelper.ApplyHeader(lblMode);                        // RULE-N53 — شريط العنوان
        GreenThemeHelper.ApplyGrid(dgvCustomers);
        lblRecordsCount.ForeColor = GreenThemeHelper.TextStrong;      // RULE-N53 — شريط الملخّص

        // RULE-N22/RULE-N52: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        dgvCustomers.ReadOnly = true;
        dgvCustomers.AllowUserToAddRows = false;
        dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCustomers.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ── التحميل ───────────────────────────────────────────────────────────────

    private async void _LoadCustomers()
    {
        if (_serviceProvider is null) return;

        Cursor = dgvCustomers.Cursor = Cursors.WaitCursor;
        try
        {
            var result = await Service.GetAllAsync();

            if (!result.IsSuccess || result.Value is null)
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة العملاء. يرجى المحاولة مرة أخرى.");
                return;
            }

            _customers.Clear();
            _customers.AddRange(result.Value);

            _customersTable = new DataTable();
            _customersTable.Columns.Add("Id", typeof(int));
            _customersTable.Columns.Add("Name", typeof(string));
            _customersTable.Columns.Add("Phone", typeof(string));
            _customersTable.Columns.Add("Email", typeof(string));

            foreach (var c in _customers)
                _customersTable.Rows.Add(c.Id, c.Name, c.Phone ?? string.Empty, c.Email ?? string.Empty);

            _customersView = new DataView(_customersTable);
            dgvCustomers.DataSource = _customersView;

            _ApplyFilter();   // يحترم نص البحث المكتوب قبل التحديث
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة العملاء");
            clsDialogs.Error("تعذر تحميل قائمة العملاء. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = dgvCustomers.Cursor = Cursors.Default;
            _UpdateButtonsState();
        }
    }

    // ── البحث الفوري (RULE-N24/RULE-N59/RULE-N60) ────────────────────────────

    private void txtSearch_TextChanged(object sender, EventArgs e) => _ApplyFilter();

    /// <summary>Enter في حقل البحث = اختيار أفضل نتيجة فوراً (RULE-N42/RULE-N47).</summary>
    private void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;
        _SelectCurrent();
    }

    /// <summary>
    /// التصفية الفورية (RULE-N59) + عدّ النتائج (RULE-N61) + آلة حالة «مسح التصفية» (RULE-N148).
    /// لا تُعيد التحميل ولا تلمس التحديد — تعمل على <see cref="_customersView"/> القائم.
    /// </summary>
    private void _ApplyFilter()
    {
        // RULE-N148: مفعّل فقط عند وجود تصفية نشطة — يُحدَّث في كل تغيير وقبل أي خروج مبكر.
        btnClearFilters.Enabled = !string.IsNullOrWhiteSpace(txtSearch.Text);

        if (_customersView is null)
        {
            lblRecordsCount.Text = $"النتائج: {clsFormat.Number(0)}";
            return;
        }

        string search = txtSearch.Text.Trim();

        // القيمة تُهرَّب عبر RowFilterHelper — لا إدراج خام في RowFilter.
        _customersView.RowFilter = string.IsNullOrEmpty(search)
            ? string.Empty
            : RowFilterHelper.Like("Name", search) + " OR " + RowFilterHelper.Like("Phone", search);

        lblNoData.Visible = _customersView.Count == 0;
        lblRecordsCount.Text = $"النتائج: {clsFormat.Number(_customersView.Count)}";
        _UpdateButtonsState();
    }

    /// <summary>
    /// «مسح التصفية» يمسح أداة التصفية وحدها ثم يُعيد التصفية فوراً (RULE-N148):
    /// بلا إعادة تحميل للبيانات وبلا تغيير للسطر المحدد — الاحترام الكامل لباقي حالة الشاشة.
    /// </summary>
    private void btnClearFilters_Click(object sender, EventArgs e)
    {
        if (txtSearch.TextLength == 0) return;

        txtSearch.Clear();   // TextChanged ⇒ _ApplyFilter (تصفية فورية بلا تحميل)
        txtSearch.Focus();
    }

    // ── الاختيار ─────────────────────────────────────────────────────────────

    private void dgvCustomers_SelectionChanged(object sender, EventArgs e) => _UpdateButtonsState();

    private void dgvCustomers_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;   // يمنع تنفيذ AcceptButton مرة ثانية
        _SelectCurrent();
    }

    private void dgvCustomers_DoubleClick(object sender, EventArgs e) => _SelectCurrent();

    private void btnSelect_Click(object sender, EventArgs e) => _SelectCurrent();

    /// <summary>آلة حالة الأزرار (RULE-N43).</summary>
    private void _UpdateButtonsState()
    {
        btnSelect.Enabled = dgvCustomers.CurrentRow is not null && dgvCustomers.Rows.Count > 0;
    }

    /// <summary>
    /// يُرجع <c>CustomerDto</c> الحقيقي القادم من الخدمة — لا نسخة مبنيّة من نص الشبكة،
    /// فيبقى حدّ الدين والرصيد والرقم الضريبي كما هي في قاعدة البيانات.
    /// </summary>
    private void _SelectCurrent()
    {
        if (dgvCustomers.CurrentRow?.DataBoundItem is not DataRowView row) return;

        if (row["Id"] is not int id || id <= 0) return;

        // لا يُبنى العميل من نص الشبكة: لو غاب سجل الخدمة نرفض التحديد برسالة عربية صريحة (لا نجاح كاذب).
        CustomerDto? customer = _customers.Find(c => c.Id == id);
        if (customer is null)
        {
            clsDialogs.Warning("تعذر جلب بيانات العميل المحدد. حدّث القائمة (F5) ثم أعد المحاولة.");
            return;
        }

        SelectedCustomer = customer;

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
    private void cmsRefresh_Click(object? sender, EventArgs e) => _LoadCustomers();

    /// <summary>تُلغى القائمة عند عدم وجود سطر محدد في الشبكة (RULE-N23).</summary>
    private void cmsRows_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (dgvCustomers.CurrentRow?.DataBoundItem is not DataRowView)
        {
            e.Cancel = true;
            return;
        }

        cmsSelect.Enabled = btnSelect.Enabled;
    }
}
