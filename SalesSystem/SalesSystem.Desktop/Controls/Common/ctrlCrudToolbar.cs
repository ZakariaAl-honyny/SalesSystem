// -----------------------------------------------------------------------------
// ctrlCrudToolbar.cs — شريط أوامر CRUD موحّد (البيت الواحد — RULE-N169…RULE-N172)
//
// • الغرض: صف الأزرار الملوّن «+ إضافة · تعديل · حفظ · إلغاء التعديل · حذف · تحديث بيانات ·
//   خروج» بنفس شكل شاشة المرجع (frmAddUpdateSalesInvoice / «أوامر الفاتورة»)، ليُستهلك في
//   كل الشاشات لاحقاً بلا نسخة ثانية من الألوان أو آلة الحالة.
// • العنصر **لا يعرف شيئاً** عن البيانات: لا خدمة · لا حوار · لا استثناء · لا async.
//   يرفع حدثاً فقط، والحاضن يقرّر ماذا يفعل (RULE-N109 — code-behind كلاسيكي).
// • الحالة تُحسب من أربعة مدخلات فقط: Mode · HasSelection · AllowDelete · ShowSave/ShowClose
//   — والآلة الوحيدة RefreshState() (§2.36 النظير RULE-N150).
// • الألوان من اللوحة الحيّة حرفياً في المصمّم (RULE-N204)، ولا لون مكتوب هنا (RULE-N54).
// -----------------------------------------------------------------------------

using System.ComponentModel;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Controls.Common;

/// <summary>وضع شريط الأوامر: عرض (الافتراضي) · إضافة · تعديل.</summary>
public enum CrudMode
{
    Browsing = 0,
    Adding = 1,
    Editing = 2,
}

/// <summary>
/// شريط أوامر CRUD قابل للتضمين: أزرار ملوّنة + آلة حالة واحدة. لا منطق بيانات ولا حوارات.
/// <para>
/// الاستخدام (نموذجٌ عام): <c>toolbar.SetMode(CrudMode.Adding)</c> عند «إضافة»،
/// <c>toolbar.HasSelection = true</c> عند اختيار سجل، <c>toolbar.AllowDelete = false</c>
/// للسجلات النظامية — ثم استهلك الأحداث السبعة.
/// </para>
/// </summary>
public partial class ctrlCrudToolbar : UserControl
{
    private CrudMode _mode = CrudMode.Browsing;
    private bool _hasSelection;
    private bool _allowDelete = true;
    private bool _showSave;
    private bool _showClose = true;

    public ctrlCrudToolbar()
    {
        InitializeComponent();

        // RULE-N43 / RULE-N154 — مظهر التعطيل الموحّد لأزرار الحالة كلها.
        // (الأربعة المسمّاة إضافة/تعديل/حذف/تحديث + حفظ/إلغاء التعديل لأن حالتهما تتغيّر بالوضع أيضاً؛
        //  الدالة idempotent فلا ضرر من التوصيل المزدوج مع GreenThemeHelper.Apply أدناه.)
        KeyboardHelper.WireButtonDisabledVisual(btnAdd, btnEdit, btnSave, btnCancelEdit, btnDelete, btnRefresh);

        RefreshState();

        GreenThemeHelper.Apply(this);   // آخر سطر دائماً (RULE-N50)
    }

    // ═════════════════════════════════════════════════════════════════
    //  الأحداث — العنصر يرفع فقط، والحاضن ينفّذ
    // ═════════════════════════════════════════════════════════════════

    /// <summary>«+ إضافة» — إضافة سجل جديد (أو حساب فرعي في شاشة الشجرة).</summary>
    public event EventHandler? AddRequested;

    /// <summary>«تعديل» — تعديل السجل المحدد.</summary>
    public event EventHandler? EditRequested;

    /// <summary>«حفظ» — يظهر فقط عند <see cref="ShowSave"/> (شاشات المحرّر).</summary>
    public event EventHandler? SaveRequested;

    /// <summary>«إلغاء التعديل» — إلغاء الإضافة/التعديل والعودة إلى العرض.</summary>
    public event EventHandler? CancelEditRequested;

    /// <summary>«حذف» — حذف/تعطيل السجل المحدد (التأكيد مسؤولية الحاضن).</summary>
    public event EventHandler? DeleteRequested;

    /// <summary>«تحديث بيانات» — إعادة تحميل البيانات من الخدمة.</summary>
    public event EventHandler? RefreshRequested;

    /// <summary>«خروج» — إغلاق الشاشة الحاضنة.</summary>
    public event EventHandler? CloseRequested;

    // ═════════════════════════════════════════════════════════════════
    //  المدخلات (كلها واجهة وقت تشغيل — لا تظهر في المصمّم، WFO1000)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>الوضع الحالي (يُضبط عبر <see cref="SetMode"/> فقط).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public CrudMode Mode => _mode;

    /// <summary>هل يوجد سجل محدد؟ (يُفعّل «تعديل/حذف»).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool HasSelection
    {
        get => _hasSelection;
        set
        {
            if (_hasSelection == value) return;
            _hasSelection = value;
            RefreshState();
        }
    }

    /// <summary>هل يُسمح بالحذف؟ <c>false</c> يُعطّل «حذف» دائماً (مثل السجلات النظامية).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool AllowDelete
    {
        get => _allowDelete;
        set
        {
            if (_allowDelete == value) return;
            _allowDelete = value;
            RefreshState();
        }
    }

    /// <summary>إظهار زر «حفظ» (مخفي افتراضياً — الشاشات بلا محرّر داخلي لا تحفظ).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ShowSave
    {
        get => _showSave;
        set
        {
            if (_showSave == value) return;
            _showSave = value;
            RefreshState();
        }
    }

    /// <summary>إظهار زر «خروج» (يُخفى عندما يملك الحاضن زر إغلاقه الخاص).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ShowClose
    {
        get => _showClose;
        set
        {
            if (_showClose == value) return;
            _showClose = value;
            RefreshState();
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  آلة الحالة الوحيدة
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يضبط الوضع ويُعيد حساب تفعيل الأزرار ووسم «حفظ»/«إلغاء التعديل» بحسب الوضع.</summary>
    public void SetMode(CrudMode mode)
    {
        if (_mode == mode) return;

        _mode = mode;
        RefreshState();
    }

    /// <summary>
    /// الحالة الوحيدة للأزرار (نظير <c>_ApplyModeState</c> في شاشات الماستر — RULE-N150):
    /// تُستدعى بعد كل تغيير وضع/تحديد/سماحية — ولا يُضبط <c>Enabled</c> خارجها.
    /// </summary>
    public void RefreshState()
    {
        bool browsing = _mode == CrudMode.Browsing;
        bool editMode = !browsing;
        bool adding = _mode == CrudMode.Adding;

        // وضع العرض: إضافة/تعديل/حذف/تحديث بحسب التحديد والسماحية
        btnAdd.Enabled = browsing;
        btnEdit.Enabled = browsing && _hasSelection;
        btnDelete.Enabled = browsing && _hasSelection && _allowDelete;
        btnRefresh.Enabled = browsing;   // لا تحديث فوق إدخال غير محفوظ

        // أزرار المحرّر: تظهر فقط إن طُلبت، وتُفعَّل في وضعي الإضافة/التعديل.
        // الوسم يتبع الوضع (نص عربي واحد لكل وضع — لا حكم على النص في أي مكان آخر).
        btnSave.Visible = _showSave;
        btnSave.Enabled = _showSave && editMode;
        btnSave.Text = adding ? "حفظ الإضافة" : _mode == CrudMode.Editing ? "حفظ التعديل" : "حفظ";
        btnCancelEdit.Enabled = editMode;
        btnCancelEdit.Text = adding ? "إلغاء الإضافة" : "إلغاء التعديل";

        // «خروج» يتبع الحاضن
        btnClose.Visible = _showClose;
    }

    // ═════════════════════════════════════════════════════════════════
    //  التوصيل — كل زر يرفع حدثه فقط (صفر منطق)
    // ═════════════════════════════════════════════════════════════════

    private void btnAdd_Click(object? sender, EventArgs e) => AddRequested?.Invoke(this, EventArgs.Empty);

    private void btnEdit_Click(object? sender, EventArgs e) => EditRequested?.Invoke(this, EventArgs.Empty);

    private void btnSave_Click(object? sender, EventArgs e) => SaveRequested?.Invoke(this, EventArgs.Empty);

    private void btnCancelEdit_Click(object? sender, EventArgs e) => CancelEditRequested?.Invoke(this, EventArgs.Empty);

    private void btnDelete_Click(object? sender, EventArgs e) => DeleteRequested?.Invoke(this, EventArgs.Empty);

    private void btnRefresh_Click(object? sender, EventArgs e) => RefreshRequested?.Invoke(this, EventArgs.Empty);

    private void btnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);

    // ═════════════════════════════════════════════════════════════════
    //  التنظيف (RULE-N30) — الحاوية تملك ToolTip فلا تُترك حيّة
    // ═════════════════════════════════════════════════════════════════

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();

        base.Dispose(disposing);
    }
}
