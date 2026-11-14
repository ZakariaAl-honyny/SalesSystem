using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Units.Forms;

/// <summary>
/// نموذج رئيسي (Master Form) لدليل الوحدات.
/// يجمع: بحث مباشر + شبكة + محرر إدخال/تعديل في شاشة واحدة (§2.28)،
/// ويبدأ دائماً في **وضع العرض** حيث كل حقول الإدخال معطّلة (§2.36).
/// </summary>
public partial class frmUnitMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IUnitDesktopService UnitService => _serviceProvider.GetRequiredService<IUnitDesktopService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, UnitDto> _unitsById = new();
    private DataTable _unitsTable = new();
    private DataView _unitsView = new();

    /// <summary>null = لا سجل محدد، &gt; 0 = السجل المعروض/المحفوظ في المحرّر.</summary>
    private int? _editId;

    /// <summary>
    /// حالة الشاشة (آلة الحالة): تبدأ دائماً بوضع العرض <see cref="EntryMode.Browsing"/>
    /// حيث كل حقول الإدخال **معطّلة**، ولا تُفعَّل إلا بزر «جديد» أو باختيار سجل من الشبكة (RULE-N149).
    /// </summary>
    private EntryMode _mode = EntryMode.Browsing;

    /// <summary>يمنع تكرار تنفيذ الحفظ/الحذف أثناء العملية الجارية (بدل تعطيل الأزرار متفرقاً في المعالجات).</summary>
    private bool _busy;

    /// <summary>نص شريط العنوان الأساسي، تُضاف إليه لاحقة الوضع الحالي.</summary>
    private const string FormTitle = "(( دليل الوحدات ))";

    /// <summary>أوضاع شاشة الإدخال — آلة الحالة التي تحكم تفعيل الحقول والأزرار (RULE-N149…RULE-N152).</summary>
    private enum EntryMode
    {
        /// <summary>عرض فقط: كل حقول الإدخال معطّلة حتى يضغط المستخدم «جديد» أو يحدّد سجلاً (RULE-N149).</summary>
        Browsing,

        /// <summary>إضافة وحدة جديدة: الحقول مفعّلة وزر «إضافة» متاح فقط.</summary>
        Adding,

        /// <summary>تعديل وحدة محددة: الحقول مفعّلة وأزرار «تعديل/حذف» متاحة.</summary>
        Editing
    }

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmUnitMaster()
    {
        InitializeComponent();
    }

    public frmUnitMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this); // أسهم RTL: يمين=رجوع، يسار=تالي

        // أزرار الوضع (إضافة/تعديل/حذف/إعادة تنشيط/مسح التصفية) تتغير حالة تفعيلها حسب وضع الشاشة،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل (RULE-N43).
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnReactivate, btnRefresh, btnClose, btnClearFilters);

        // الاختصارات الموحّدة (RULE-N47): F2 جديد · F5 تحديث · Delete تعطيل · Esc مسح التصفية/إلغاء النموذج.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, _GlobalDelete),
            (Keys.Escape, _GlobalEscape));

        // RULE-N147: Enter = الزر الأساسي للوضع الحالي (عرض ⇒ جديد · إضافة ⇒ إضافة · تعديل ⇒ تعديل).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // RULE-N60: أدوات التصفية تُقرأ بالمؤشر — «الحالة: الكل» و«تصفية بـ: الاسم» افتراضياً.
        cbStatus.SelectedIndex = 0;
        cbFilterBy.SelectedIndex = 0;
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    // ── Global Shortcuts (RULE-N47) ───────────────────────────────────────────

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

        _ResetForm();   // يعيد الشاشة إلى وضع العرض عبر _ApplyModeState
    }

    /// <summary>هل المؤشر داخل حقل إدخال؟ (لئلا يُنفَّذ اختصار Delete أثناء الكتابة).</summary>
    private bool _IsTextInputFocused()
    {
        foreach (Control input in new Control[] { txtSearch, cbFilterBy, cbStatus, txtName, txtSymbol })
        {
            if (input.Focused) return true;
        }

        return false;
    }

    // ── Validation helpers ────────────────────────────────────────────────────

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// يعيد false إن وُجد حقل ناقص.
    /// </summary>
    private bool _FocusFirstMissingField(bool edit)
    {
        _ = edit;   // الحقول المطلوبة واحدة في وضعي الإضافة والتعديل

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الوحدة مطلوب");
            txtName.Focus();
            return false;
        }

        return true;
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← الشبكة ← المحرر ووضع العرض. وليس مسحاً للتصفية فقط.
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();        // التصفية تعود للافتراضي
            await _LoadUnitsAsync();     // إعادة قراءة الوحدات (يعيد تطبيق التصفية والملخّص)
            _ResetForm();                // تفريغ المحرر + وضع العرض + الحقول معطّلة
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

    /// <summary>زر الخروج: إغلاق شاشة دليل الوحدات.</summary>
    private void btnClose_Click(object? sender, EventArgs e) => Close();

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmUnitMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadUnitsAsync();
            _ResetForm();
            ApplyGreenTheme(); // RULE-N50 — آخر خطوة في مسار التحميل
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

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvUnits);
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;   // RULE-N53

        // RULE-N22 — شبكة عرض: السلوك تعلنه الشاشة نفسها (ApplyGrid لا يلمسه)
        dgvUnits.ReadOnly = true;
        dgvUnits.AllowUserToAddRows = false;
        dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUnits.MultiSelect = false;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
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
            _ApplyGridHeaders();
            _ApplyFilter();   // يطبّق التصفية ويحدّث الملخص وألوان الأسطر وحالة «مسح التصفية»
        }
    }

    /// <summary>
    /// عناوين أعمدة الشبكة بالعربية.
    /// لا يستخدم DataGridView عنوان <c>DataColumn.Caption</c> عند الربط بـ <see cref="DataView"/>،
    /// لذا تُضبط <c>HeaderText</c> صراحةً بعد إسناد <c>DataSource</c>.
    /// </summary>
    private void _ApplyGridHeaders()
    {
        foreach (DataGridViewColumn column in dgvUnits.Columns)
        {
            column.HeaderText = column.DataPropertyName switch
            {
                "Id" => "رقم الوحدة",
                "Name" => "اسم الوحدة",
                "Symbol" => "الرمز",
                "IsActive" => "نشط",
                _ => column.HeaderText
            };
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

    private void chkShowInactive_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

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

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح» و«تحديث» (RULE-N148).</summary>
    private void _ClearFilterInputs()
    {
        txtSearch.Clear();
        chkShowInactive.Checked = false;
        cbStatus.SelectedIndex = 0;
        cbFilterBy.SelectedIndex = 0;
    }

    /// <summary>يمسح كل أدوات التصفية ويعيد الشبكة إلى حالتها الافتراضية (RULE-N59).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
    }

    /// <summary>RULE-N43: زر «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        if (btnClearFilters is null) return;

        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtSearch.Text)
            || chkShowInactive.Checked
            || cbStatus.SelectedIndex > 0
            || cbFilterBy.SelectedIndex > 0;
    }

    /// <summary>
    /// التصفية الفورية (RULE-N59/N60): تُبنى من أسماء أعمدة معروفة فقط،
    /// والاختيار يُقرأ بالمؤشر لا بالنص العربي، والقيمة تُهرَّب قبل بنائها في RowFilter.
    /// </summary>
    private void _ApplyFilter()
    {
        if (_unitsView is null || _unitsView.Table is null) return;
        if (txtSearch is null || cbFilterBy is null || cbStatus is null || chkShowInactive is null)
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

            default: // الكل — يبقى «عرض غير النشط» هو الحاكم
                if (!chkShowInactive.Checked)
                    conditions.Add("IsActive = true");
                break;
        }

        // ── البحث النصي: عمود واحد فقط يحدده cbFilterBy ──
        if (text.Length > 0)
            conditions.Add(_FilterExpression(text));

        // RULE-N60: القيمة مُهرَّبة، وإن فشل بناء الصيغة تُترك التصفية السابقة كما هي (لا سقوط إلى «بلا تصفية»).
        try
        {
            _unitsView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : "";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تطبيق تصفية دليل الوحدات — أُبقيت التصفية السابقة كما هي");
        }

        _ApplyRowColoring();
        _UpdateSummary();
        _UpdateClearFiltersState();
    }

    /// <summary>تعبير البحث المقابل لاختيار «تصفية بـ» — أسماء أعمدة ثابتة داخل الكود (RULE-N60).</summary>
    private string _FilterExpression(string text) => cbFilterBy.SelectedIndex switch
    {
        1 => RowFilterHelper.Like("Symbol", text),
        2 => $"CONVERT(Id, 'System.String') LIKE '%{RowFilterHelper.Escape(text)}%'",
        _ => RowFilterHelper.Like("Name", text)
    };

    /// <summary>
    /// شريط الملخص (RULE-N61): المعروض من الإجمالي + النشط/المعطّل،
    /// ويُعاد حسابه بعد كل تصفية وكل تحميل.
    /// </summary>
    private void _UpdateSummary()
    {
        if (lblSummary is null || _unitsView?.Table is null) return;

        int total = _unitsTable.Rows.Count;
        int shown = _unitsView.Count;

        int active = 0, inactive = 0;

        foreach (DataRowView row in _unitsView)
        {
            if (row["IsActive"] is bool isActive && isActive) active++;
            else inactive++;
        }

        lblSummary.Text = $"المعروض: {shown} من {total} — نشط: {active} · معطّل: {inactive}";

        // «لا توجد بيانات» يُعلن في شريط الملخص بدل إضافة صف وهمي إلى الشبكة
        // (الصف الوهمي يُفسد التصدير والتصفية — RULE-N22 / RULE-N61).
        if (shown == 0) lblSummary.Text += " — لا توجد بيانات مطابقة";
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

    /// <summary>تعبئة المحرّر بالسجل المحدد والانتقال إلى وضع التعديل (RULE-N151).</summary>
    private void _FillForm(int unitId)
    {
        if (!_unitsById.TryGetValue(unitId, out var u)) return;

        _editId = u.Id;
        _mode = EntryMode.Editing;   // RULE-N151: اختيار سجل من الشبكة = وضع التعديل

        txtCode.Text = u.Id.ToString();
        txtName.Text = u.Name;
        txtSymbol.Text = u.Symbol ?? "";

        errorProvider.Clear();
        _ApplyModeState();           // تفعيل الحقول + ضبط الأزرار + شريط الوضع
    }

    // ── Actions state — آلة الحالة المركزية (RULE-N149…RULE-N152) ─────────────

    /// <summary>
    /// آلة الحالة المركزية للشاشة: تفعيل/تعطيل حقول الإدخال، وحالة كل زر، ونص شريط الوضع.
    /// تُستدعى بعد **كل** انتقال حالة: تحميل · «جديد» · اختيار سطر · حفظ ناجح · حذف · «تحديث» · Esc.
    /// </summary>
    private void _ApplyModeState()
    {
        bool editingSession = _mode != EntryMode.Browsing;

        _SetEditorEnabled(editingSession);

        // آلة حالة الأزرار (RULE-N43): «جديد/حفظ» في الإضافة، «تعديل/حذف» عند وجود سجل محدد.
        btnNew.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;
        btnSave.Enabled = _mode == EntryMode.Adding && !_busy;
        btnEdit.Enabled = _mode == EntryMode.Editing && !_busy;
        btnDelete.Enabled = _mode == EntryMode.Editing && !_busy;

        _UpdateReactivateButton();   // RULE-N92: إعادة التنشيط للسجل المعطّل فقط

        _UpdateModeHeader();         // RULE-N44

        // RULE-N147: Enter يُنفِّذ الزر الأساسي للوضع الحالي (ويُبرزه كزر افتراضي للشاشة).
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147): عرض ⇒ «جديد» · إضافة ⇒ «إضافة» · تعديل ⇒ «تعديل».
    /// يُقرأ عند كل ضغطة Enter وعند كل انتقال حالة.
    /// </summary>
    private Button? _PrimaryActionButton() => _mode switch
    {
        EntryMode.Adding => btnSave,
        EntryMode.Editing => btnEdit,
        _ => btnNew
    };

    /// <summary>
    /// آلة حالة زر «إعادة تنشيط» (RULE-N90…RULE-N94): مفعّل فقط في وضع التعديل على سجل **معطّل**،
    /// ومعطّل في وضع العرض/الإضافة، أو على سجل نشط، أو أثناء عملية جارية.
    /// </summary>
    private void _UpdateReactivateButton()
    {
        bool isInactiveRecord = _mode == EntryMode.Editing
            && _editId is int id
            && _unitsById.TryGetValue(id, out var unit)
            && !unit.IsActive;

        btnReactivate.Enabled = isInactiveRecord && !_busy;   // RULE-N92
    }

    /// <summary>حقول المحرر التي تُعطَّل في وضع العرض وتُفعَّل عند «جديد» أو عند تحديد سجل (txtCode للقراءة دائماً).</summary>
    private Control[] _EditorInputs() => new Control[] { txtName, txtSymbol };

    /// <summary>تفعيل أو تعطيل كل حقول المحرر دفعة واحدة (RULE-N149).</summary>
    private void _SetEditorEnabled(bool enabled)
    {
        foreach (var input in _EditorInputs())
            input.Enabled = enabled;
    }

    /// <summary>
    /// شريط الوضع (RULE-N44): «وضع العرض» افتراضياً · «إضافة وحدة جديدة» · «تعديل وحدة: &lt;الاسم&gt;».
    /// </summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _mode switch
        {
            EntryMode.Adding => $"{FormTitle} — إضافة وحدة جديدة",
            EntryMode.Editing => $"{FormTitle} — تعديل وحدة: {txtName.Text.Trim()}",
            _ => $"{FormTitle} — وضع العرض"
        };
    }

    // ── Reset / New entry ─────────────────────────────────────────────────────

    /// <summary>
    /// إعادة الشاشة إلى **وضع العرض** (الحالة الافتراضية): تفريغ الحقول، تعطيل كل حقول الإدخال،
    /// وإتاحة زر «جديد» فقط (RULE-N149).
    /// </summary>
    private void _ResetForm()
    {
        _editId = null;
        _mode = EntryMode.Browsing;

        errorProvider.Clear();

        txtCode.Clear();
        txtName.Clear();
        txtSymbol.Clear();

        dgvUnits.ClearSelection();

        _ApplyModeState();   // الحقول معطّلة + «جديد» فقط + شريط الوضع = وضع العرض
        btnNew.Focus();
    }

    /// <summary>
    /// بدء إدخال وحدة جديدة (زر «جديد» / F2): تفريغ الحقول ثم **تفعيلها** وتحويل شريط الوضع
    /// إلى «إضافة وحدة جديدة» (RULE-N151).
    /// </summary>
    private void _StartNewEntry()
    {
        _ResetForm();

        _mode = EntryMode.Adding;
        _ApplyModeState();

        txtName.Focus();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    /// <summary>قواعد التحقق الإضافية (الأطوال) — بلا رسالة منبثقة، بلا تعطيل أزرار.</summary>
    private bool _ValidateForm()
    {
        if (txtName.Text.Trim().Length > 50)
        {
            errorProvider.SetError(txtName, "اسم الوحدة أطول من المسموح (50 حرفاً)");
            txtName.Focus();
            return false;
        }

        errorProvider.SetError(txtName, "");

        if (txtSymbol.Text.Trim().Length > 20)
        {
            errorProvider.SetError(txtSymbol, "رمز الوحدة أطول من المسموح (20 حرفاً)");
            txtSymbol.Focus();
            return false;
        }

        errorProvider.SetError(txtSymbol, "");

        return true;
    }

    // ── Buttons ───────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _StartNewEntry();

    /// <summary>زر الإضافة: يُضيف وحدة جديدة دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث الوحدة المحددة فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: true);

    /// <summary>
    /// حفظ الوحدة: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
    /// بعد النجاح: تحديث تلقائي للشبكة والمحرر مع إبقاء السطر المحفوظ محدداً (RULE-N151).
    /// </summary>
    private async Task _SaveAsync(bool edit)
    {
        if (_busy) return;

        // التعديل بدون سجل محدد لا معنى له.
        if (edit && _editId is null) return;

        // الحقل المطلوب الناقص: نقل المؤشر إلى الحقل مع أيقونة خطأ (الأزرار تبقى مفعّلة).
        if (!_FocusFirstMissingField(edit)) return;

        if (!_ValidateForm()) return;

        // رسالة تأكيد قبل التعديل (مثل رسالة تأكيد الحذف).
        if (edit)
        {
            string editName = txtName.Text.Trim();
            if (MessageBox.Show($"هل أنت متأكد من تعديل الوحدة \"{editName}\"؟",
                    "تأكيد التعديل", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                return;
        }

        Cursor = Cursors.WaitCursor;
        _busy = true;
        _ApplyModeState();   // تعطيل أزرار الوضع أثناء العملية (المصدر الوحيد لحالة الأزرار)

        try
        {
            string name = txtName.Text.Trim();
            string? symbol = string.IsNullOrWhiteSpace(txtSymbol.Text)
                ? null
                : txtSymbol.Text.Trim();

            Result<UnitDto> result;

            if (edit)
            {
                int id = _editId!.Value;

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

                MessageBox.Show(edit ? "تم تعديل الوحدة بنجاح." : "تمت إضافة الوحدة بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // بعد الحفظ الناجح (إضافة أو تعديل): يصبح السجل محفوظاً ومحدداً → وضع التعديل،
                // ثم تحديث القائمة وإعادة تعبئة الحقول من البيانات المحفوظة (تحديث تلقائي بلا إعادة فتح).
                _editId = result.Value.Id;
                _mode = EntryMode.Editing;

                await _LoadUnitsAsync();
                _FillForm(result.Value.Id);   // يعبّئ الحقول ويضبط الأزرار وشريط الوضع
                _ApplyModeState();            // ضمان الحالة حتى لو لم يُعثر على السطر
                _SelectGridRow(result.Value.Id);
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
            _busy = false;
            _ApplyModeState();   // استعادة الحالة الكاملة (حقول + أزرار + شريط الوضع)
            Cursor = Cursors.Default;
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_busy) return;

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
        _busy = true;
        _ApplyModeState();   // تعطيل أزرار الوضع أثناء العملية (RULE-N43)

        try
        {
            var result = await UnitService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل الوحدة بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventBus.Publish(new UnitChangedMessage(id));

                // بعد الحذف: الرجوع إلى وضع العرض (الحقول معطّلة) ثم تحديث القائمة تلقائياً.
                _ResetForm();
                await _LoadUnitsAsync();
                _ApplyModeState();
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
            _busy = false;
            _ApplyModeState();   // بعد الحذف الناجح يعود النموذج إلى وضع العرض → «جديد» فقط متاح
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// إعادة تنشيط الوحدة المعطَّلة (RULE-N90…RULE-N94):
    /// الزر مفعّل فقط على سطر معطّل، والتنفيذ داخل الخدمة (BLL) داخل معاملة.
    /// </summary>
    private async void btnReactivate_Click(object? sender, EventArgs e)
    {
        if (_busy) return;

        if (_editId is not int id)
        {
            MessageBox.Show("يرجى اختيار وحدة معطّلة من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!_unitsById.TryGetValue(id, out var unit))
        {
            MessageBox.Show("تعذر العثور على بيانات الوحدة المحددة. يرجى تحديث القائمة.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (unit.IsActive)
        {
            MessageBox.Show("الوحدة المحددة نشطة بالفعل.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _UpdateReactivateButton();
            return;
        }

        if (MessageBox.Show($"هل تريد إعادة تنشيط الوحدة \"{unit.Name}\"؟",
                "تأكيد إعادة التنشيط", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        _busy = true;
        _ApplyModeState();   // تعطيل أزرار الوضع أثناء العملية (RULE-N43)

        try
        {
            var result = await UnitService.ReactivateAsync(id);

            if (result.IsSuccess)
            {
                Log.Information("أُعيد تنشيط الوحدة {UnitId} ({UnitName}) بنجاح", id, unit.Name);

                EventBus.Publish(new UnitChangedMessage(id));

                await _LoadUnitsAsync();

                // RULE-N94: يبقى السطر المُعاد تنشيطه محدداً وتُعاد تعبئة المحرّر (تحديث تلقائي بلا إعادة فتح).
                _editId = id;
                _FillForm(id);

                if (!_SelectGridRow(id))
                {
                    // تصفية «معطّل» (أو بحث نصي) قد تُخفي السجل بعد إعادة تنشيطه → نوسّعها ليظهر السطر ويبقى محدداً.
                    cbStatus.SelectedIndex = 0;
                    chkShowInactive.Checked = true;
                    _ApplyFilter();
                    _SelectGridRow(id);
                }

                MessageBox.Show("تم إعادة تنشيط الوحدة بنجاح.", "تم إعادة التنشيط",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Log.Warning("تعذر إعادة تنشيط الوحدة {UnitId}. السبب: {Error}", id, result.Error);

                MessageBox.Show($"تعذر إعادة تنشيط الوحدة.\n{result.Error}",
                    "فشل إعادة التنشيط", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إعادة تنشيط الوحدة {UnitId}", id);
            MessageBox.Show("حدث خطأ أثناء إعادة التنشيط. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _busy = false;
            _ApplyModeState();   // بعد النجاح: السجل نشط → زر إعادة التنشيط معطّل (RULE-N92)
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>يحدّد سطر الوحدة في الشبكة ويُعيد true إن ظهر السطر في التصفية الحالية.</summary>
    private bool _SelectGridRow(int unitId)
    {
        foreach (DataGridViewRow row in dgvUnits.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == unitId)
            {
                row.Selected = true;
                dgvUnits.CurrentCell = row.Cells["Name"];
                return true;
            }
        }

        return false;
    }

    // ── Context Menu / Double click (RULE-N23) ────────────────────────────────

    /// <summary>النقر المزدوج على السطر = تحميل بياناته في المحرّر (نفس مسار النقر المفرد).</summary>
    private void dgvUnits_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        dgvUnits_CellClick(sender, e);
    }

    /// <summary>
    /// حالة بنود قائمة السياق قبل ظهورها (RULE-N23): بنود التعديل/التعطيل تُفعَّل فقط عند
    /// وجود سطر محدد، وتتبع آلة حالة الأزرار نفسها (RULE-N43) بلا منطق مكرر.
    /// </summary>
    private void cmsGrid_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        bool hasRow = dgvUnits.CurrentRow?.DataBoundItem is DataRowView;

        mnuEdit.Enabled = hasRow;
        mnuDeactivate.Enabled = hasRow && btnDelete.Enabled;
        mnuRefresh.Enabled = btnRefresh.Enabled;
        mnuExport.Enabled = dgvUnits.Rows.Count > 0;
    }

    /// <summary>تعديل السطر المحدد = تحميله في المحرّر ثم نقل المؤشر إلى أول حقل.</summary>
    private void mnuEdit_Click(object? sender, EventArgs e)
    {
        if (dgvUnits.CurrentRow is not DataGridViewRow row) return;

        dgvUnits_CellClick(dgvUnits, new DataGridViewCellEventArgs(0, row.Index));
        txtName.Focus();
    }

    /// <summary>تعطيل السطر المحدد — يستدعي معالج زر الحذف القائم (بلا منطق مكرر).</summary>
    private void mnuDeactivate_Click(object? sender, EventArgs e) => btnDelete.PerformClick();

    /// <summary>تحديث البيانات — يستدعي معالج زر التحديث القائم.</summary>
    private void mnuRefresh_Click(object? sender, EventArgs e) => btnRefresh.PerformClick();

    /// <summary>تصدير الشبكة المعروضة إلى Excel عبر <see cref="ExportHelper"/> (RULE-N112).</summary>
    private void mnuExport_Click(object? sender, EventArgs e) => _ExportGridToExcel();

    private void _ExportGridToExcel()
    {
        try
        {
            if (dgvUnits.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات معروضة للتصدير.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"دليل_الوحدات_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                Title = "تصدير دليل الوحدات إلى Excel"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            ExportHelper.ExportToExcel(dgvUnits, "الوحدات", dialog.FileName);

            MessageBox.Show("تم تصدير البيانات بنجاح.", "تم التصدير",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تصدير دليل الوحدات إلى Excel");
            MessageBox.Show("تعذر تصدير البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}
