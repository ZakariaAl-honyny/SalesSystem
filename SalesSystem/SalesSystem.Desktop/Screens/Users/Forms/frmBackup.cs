// -----------------------------------------------------------------------------
// frmBackup — شاشة النسخ الاحتياطي (عرض · إنشاء · استعادة)
//
// الشكل: شاشة قائمة بمعيار §2.21 (RULE-N79) وترتيب §2.34 (RULE-N135):
//   pnlHeader → pnlToolbar (gbActions) → pnlFilters (gbFilter) → pnlGrid (gbBackups)
//   → pnlSummary (gbSummary) → pnlFooter
//
// المسؤوليات مفصولة تماماً:
//   • الشاشة: عرضٌ وتصفيةٌ وتأكيدٌ فقط (RULE-N11).
//   • البيانات: IBackupDesktopService (Mock في المرحلة أ · Direct في المرحلة ب) —
//     لا قرص ولا قاعدة بيانات ولا Business/** من هنا إطلاقاً.
//   • اصطلاح اسم الملف وتاريخه: BackupFileNameHelper وحده (RULE-N169).
//   • التنسيق: clsFormat وحده · الرسائل: clsDialogs وحده · الألوان والتنقل: Helpers.
//
// الإصلاح الجوهري لهذه الشاشة: العمود «تاريخ النسخة» كان فارغاً دائماً لأن الشاشة
// القديمة كانت تقسم الاسم على '-' ولا يوجد '-' في الاسم الحقيقي
// {DatabaseName}_{yyyyMMdd_HHmmss}.bak؛ الآن تُقرأ اللحظة من الـHelper فيظهر التاريخ
// الحقيقي. وعمود «الحجم» أُزيل نهائياً لأن العقد لا يعيد حجماً (لا نُزيّف رقماً).
// -----------------------------------------------------------------------------

using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Users.Forms;

/// <summary>
/// إدارة النسخ الاحتياطي: عرض المتاح + إنشاء نسخة جديدة + استعادة نسخة محددة.
/// </summary>
public partial class frmBackup : Form
{
    /// <summary>حاوية الخدمات — كل خدمة تُحلّ عند الاستخدام (RULE-N03/N04).</summary>
    private readonly IServiceProvider _serviceProvider;

    private IBackupDesktopService BackupService => _serviceProvider.GetRequiredService<IBackupDesktopService>();

    // ── بيانات الشاشة ─────────────────────────────────────────────────────────
    private DataTable _backupsTable = new();
    private DataView _backupsView = new();

    // ── حالة الشاشة ───────────────────────────────────────────────────────────
    /// <summary>عملية جارية (إنشاء/استعادة/تحميل): يمنع الضغط المزدوج ويُعطّل الأزرار.</summary>
    private bool _isBusy;

    /// <summary>هل نطاق التاريخ مشارك في التصفية؟ (يُفعّل من التاريخين أو الأزرار السريعة).</summary>
    private bool _dateRangeActive;

    /// <summary>يمنع تفعيل التصفية أثناء تهيئة أدوات التاريخ برمجياً.</summary>
    private bool _suppressDateChange;

    /// <summary>تُصبح true بعد اكتمال التوصيل في المُنشئ — تحمي من أحداث المصمّم المبكرة.</summary>
    private bool _uiReady;

    /// <summary>
    /// حارس ضغطة Esc الواحدة: نفس الضغطة قد تُمرَّر على مسارين — KeyDown في
    /// <see cref="KeyboardHelper.WireGlobalShortcuts"/> و<c>ProcessDialogKey</c> الذي يُنفّذ
    /// <c>CancelButton</c> — فيمنع المسار الثاني من تكرار المسح/الإغلاق.
    /// </summary>
    private bool _escapeInProgress;

    // ── نصوص ثابتة (لا تُكرَّر في المصمّم) ─────────────────────────────────────
    private const string EmptyNoDataText =
        "لا توجد نسخ احتياطية بعد — اضغط «إنشاء نسخة احتياطية الآن» لإنشاء أول نسخة.";

    private const string EmptyNoMatchText =
        "لا توجد نتائج مطابقة للتصفية الحالية — اضغط «مسح التصفية» لعرض كل النسخ.";

    private const string BusyCreateText = "جارٍ إنشاء النسخة الاحتياطية…";
    private const string BusyRestoreText = "جارٍ استعادة النسخة…";

    // ── أعمدة البيانات (أسماء ثابتة — لا يُبنى منها اسم من إدخال المستخدم) ─────
    private const string ColumnFileName = "FileName";
    private const string ColumnCreatedAt = "CreatedAt";
    private const string ColumnCreatedAtKey = "CreatedAtKey";
    private const string ColumnDateText = "DateText";

    /// <summary>المُنشئ — كل الخدمات تُحلّ من الحاوية (RULE-N02/N03).</summary>
    public frmBackup(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        InitializeComponent();

        // RULE-N40…RULE-N47 · RULE-N95 — الستة كاملة من KeyboardHelper
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)); // RULE-N54b — لون حيّ
        KeyboardHelper.WireArrowNavigation(this);                                                        // RULE-N45 — أسهم RTL
        KeyboardHelper.WireButtonDisabledVisual(btnCreateBackup, btnRestoreBackup, btnRefresh, btnClearFilters); // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                                         // RULE-N47
            (Keys.F2, () => _PerformClick(btnCreateBackup)),
            (Keys.F4, () => _PerformClick(btnRestoreBackup)),
            (Keys.F5, () => _PerformClick(btnRefresh)),
            (Keys.Escape, _GlobalEscape));

        // Enter = الزر الأساسي للوضع (RULE-N147) — وفارغ عند التركيز على الشبكة (RULE-N153)
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // زر «إخفاء/إظهار التصفية» المُعلَن في المصمّم + F9 + الحالة المحفوظة (RULE-N63/N145)
        FilterPanelHelper.Attach(this, _serviceProvider);

        _InitializeFilterControls();

        _uiReady = true;
    }

    // ── التهيئة ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Esc: مع تصفية نشطة ⇒ مسحها (بلا إغلاق) · بلا تصفية ⇒ إغلاق الشاشة (RULE-N47/N59).
    /// <para>
    /// هذا هو **المسار الوحيد** لـ Esc: يُستدعى من اختصار
    /// <see cref="KeyboardHelper.WireGlobalShortcuts"/> ومن <see cref="ProcessDialogKey"/>،
    /// والحارس <see cref="_escapeInProgress"/> يضمن أن ضغطة واحدة تُنفّذ فعلاً واحداً فقط
    /// (كانت الضغطة الواحدة تمسح التصفية **ثم** تُغلق الشاشة عبر <c>CancelButton</c>).
    /// </para>
    /// </summary>
    private void _GlobalEscape()
    {
        if (IsDisposed || _escapeInProgress) return;

        _escapeInProgress = true;

        try
        {
            if (_IsFilterActive())
            {
                _ClearFilterInputs();
                _ApplyFilter();
                txtFilterValue.Focus();
                return;
            }

            btnClose.PerformClick();
        }
        finally
        {
            // يُرفع الحارس بعد انتهاء معالجة ضغطة المفتاح **نفسها** (KeyDown ثم ProcessDialogKey)،
            // لا فوراً — وإلا نفّذ المسار الثاني المسح أو الإغلاق مرة أخرى.
            if (IsHandleCreated && !IsDisposed)
            {
                BeginInvoke(new Action(() =>
                {
                    _escapeInProgress = false;
                }));
            }
            else
            {
                _escapeInProgress = false;
            }
        }
    }

    /// <summary>
    /// <c>CancelButton = btnClose</c> (عقد الحوار كما هو في المصمّم) يجعل النموذج يعالج Esc في
    /// <c>ProcessDialogKey</c> — وهو مسار مستقل عن <c>KeyDown</c> ولذلك كانت الضغطة الواحدة
    /// تمسح التصفية وتُغلق معاً؛ فنحوّل Esc هنا إلى المسار الواحد <see cref="_GlobalEscape"/>
    /// ونُعيد <c>true</c> كي لا يُنفّذ <c>CancelButton</c> إغلاقاً فوقه. الإغلاق نفسه باقٍ
    /// (يُطلب من داخل المسار عندما لا توجد تصفية) — لم تُلغَ وظيفة Esc للإغلاق.
    /// </summary>
    protected override bool ProcessDialogKey(Keys keyData)
    {
        if (keyData != Keys.Escape)
            return base.ProcessDialogKey(keyData);

        _GlobalEscape();
        return true;
    }

    /// <summary>
    /// تهيئة أدوات التصفية: الفهرس 0 («اسم الملف») هو الافتراضي، ونطاق التاريخ يبدأ
    /// غير مشارك في التصفية حتى يختار المستخدم نطاقاً (RULE-N57/N60).
    /// </summary>
    private void _InitializeFilterControls()
    {
        _suppressDateChange = true;

        try
        {
            cbFilterBy.SelectedIndex = 0;

            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;

            _dateRangeActive = false;
            txtFilterValue.Clear();
        }
        finally
        {
            _suppressDateChange = false;
        }
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستهلك من الـHelpers فقط (RULE-N112).</summary>
    private void ApplyUiTheme()
    {
        // لا ThemeHelper.ApplyMainFormStyle هنا: المصمّم هو مرجع الخط (Tahoma) والخلفية (أخضر)،
        // وهي تفرض أبيض/Segoe UI فتُعيد تشكيل الشاشة بعد التحميل (RULE-N134/N139).
        ThemeHelper.ApplyButtonStyle(btnCreateBackup, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnRestoreBackup, ThemeHelper.ButtonType.Danger);
        ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        _ApplyTahomaFont(btnCreateBackup, btnRestoreBackup, btnRefresh, btnClose);

        // أزرار النطاقات و«مسح التصفية» موضوعة بإحداثيات مطلقة داخل gbFilter:
        // لا تُمرّر على ThemeHelper (يفرض AutoSize فيُزحزح التخطيط)، وتأخذ لونها من الثيم.
        foreach (var rangeButton in new[] { btnRangeToday, btnRangeMonth, btnRange30 })
        {
            rangeButton.BackColor = GreenThemeHelper.ActionInfoBackground;
            rangeButton.ForeColor = Color.White;
        }

        btnClearFilters.BackColor = GreenThemeHelper.ActionNeutralBackground; // RULE-N148
        btnClearFilters.ForeColor = Color.White;

        GreenThemeHelper.ApplyHeader(lblMode);
        lblSubtitle.BackColor = GreenThemeHelper.HeaderBackground;
        lblSubtitle.ForeColor = GreenThemeHelper.HeaderForeground;

        gbActions.ForeColor = GreenThemeHelper.TextStrong;
        gbFilter.ForeColor = GreenThemeHelper.TextStrong;
        gbBackups.ForeColor = GreenThemeHelper.TextStrong;
        gbSummary.ForeColor = GreenThemeHelper.TextStrong;

        lblSummary.ForeColor = GreenThemeHelper.TextStrong;
        lblEmpty.ForeColor = GreenThemeHelper.TextStrong;
        lblStatus.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.ApplyGrid(dgvBackups);

        // RULE-N22 — شبكة عرض: السلوك تعلنه الشاشة نفسها (ApplyGrid لا يلمسه — RULE-N52)
        dgvBackups.ReadOnly = true;
        dgvBackups.AllowUserToAddRows = false;
        dgvBackups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBackups.MultiSelect = false;
        dgvBackups.RowHeadersVisible = false;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>
    /// RULE-N21/RULE-N134: <see cref="ThemeHelper"/> يفرض Segoe UI على الأزرار — نُعيد
    /// Tahoma كما في المصمّم ليتساوى المصمّم ووقت التشغيل.
    /// </summary>
    private static void _ApplyTahomaFont(params Button[] buttons)
    {
        foreach (var button in buttons)
            button.Font = new Font("Tahoma", 9F, FontStyle.Bold);
    }

    // ── مسار التحميل ──────────────────────────────────────────────────────────

    private async void frmBackup_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;

        try
        {
            await _LoadBackupListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة النسخ الاحتياطي");
            clsDialogs.Error("تعذر تحميل قائمة النسخ الاحتياطية. يرجى إعادة المحاولة.");
        }
        finally
        {
            ApplyUiTheme();  // RULE-N50 — آخر خطوة في مسار التحميل
            _ApplyFilter();  // تصفية + ملخّص + فراغ + حالة (RULE-N61/N43/N148)
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// إعادة قراءة القائمة من الخدمة وبناء جدول العرض، ثم إعادة التصفية والملخّص
    /// والفراغ **دائماً** (حتى عند الخطأ) كي لا تبقى الشاشة على بيانات قديمة.
    /// </summary>
    private async Task _LoadBackupListAsync()
    {
        var table = _BuildTable();
        string? errorMessage = null;

        try
        {
            var result = await BackupService.GetBackupListAsync();

            if (result.IsSuccess && result.Value is not null)
            {
                foreach (var fileName in result.Value)
                {
                    if (!string.IsNullOrWhiteSpace(fileName))
                        table.Rows.Add(_BuildRowValues(fileName));
                }
            }
            else
            {
                errorMessage = result.Error ?? "تعذر تحميل قائمة النسخ الاحتياطية.";
            }
        }
        finally
        {
            _backupsTable = table;
            _backupsView = new DataView(_backupsTable);

            dgvBackups.DataSource = _backupsView;
            _ApplyFilter();

            // التصفية قد تُعيد تحديد الصف الأول: تُفرَّغ بعدها كي يبدأ المستخدم بلا تحديد
            // (الاستعادة معطّلة حتى يختار نسخة — RULE-N43).
            dgvBackups.ClearSelection();
            _ApplyState();
        }

        if (errorMessage is not null) clsDialogs.Error(errorMessage);
    }

    private static DataTable _BuildTable()
    {
        var table = new DataTable();

        table.Columns.Add(ColumnFileName, typeof(string));
        table.Columns.Add(ColumnCreatedAt, typeof(DateTime));   // تاريخ حقيقي ⇒ ترتيب صحيح برأس العمود
        table.Columns.Add(ColumnCreatedAtKey, typeof(int));     // مفتاح yyyyMMdd لتصفية النطاق
        table.Columns.Add(ColumnDateText, typeof(string));      // إسقاط نصي للتصفية بالتاريخ فقط

        return table;
    }

    /// <summary>
    /// قيم صف واحد: التاريخ يُقرأ من البيت الواحد للاصطلاح، والاسم غير المطابق
    /// يحصل على DBNull في التاريخ وعلى «—» في عمود التاريخ المعروض.
    /// </summary>
    private static object[] _BuildRowValues(string fileName)
    {
        if (BackupFileNameHelper.TryGetCreatedAt(fileName, out var createdAt))
        {
            return new object[]
            {
                fileName,
                createdAt,
                _DateKey(createdAt),
                BackupFileNameHelper.GetDisplayDate(fileName)
            };
        }

        return new object[]
        {
            fileName,
            DBNull.Value,
            0,                                          // بلا مفتاح تاريخ ⇒ لا يدخل أي نطاق زمني
            BackupFileNameHelper.GetDisplayDate(fileName)
        };
    }

    /// <summary>مفتاح تاريخ قابل للمقارنة (سنة*10000 + شهر*100 + يوم) — لتصفية النطاق بلا نص.</summary>
    private static int _DateKey(DateTime date) => (date.Year * 10000) + (date.Month * 100) + date.Day;

    /// <summary>
    /// عمود «تاريخ النسخة» يحمل <see cref="DateTime"/> حقيقياً (فالترتيب صحيح)، ويُعرض
    /// نصّه عبر البيت الواحد <see cref="BackupFileNameHelper.GetDisplayDate"/> —
    /// والاسم غير المطابق يظهر «—» بلا كسر للشبكة.
    /// </summary>
    private void dgvBackups_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != colCreatedAt.Index) return;

        var fileName = dgvBackups.Rows[e.RowIndex].Cells[colFileName.Index].Value as string;

        e.Value = BackupFileNameHelper.GetDisplayDate(fileName);
        e.FormattingApplied = true;
    }

    // ── التصفية الفورية (RULE-N56…RULE-N63) ───────────────────────────────────

    private void txtFilterValue_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    /// <summary>Enter يطبّق التصفية فوراً (RULE-N59) — Esc يعالجه الاختصار الموحّد.</summary>
    private void txtFilterValue_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        _ApplyFilter();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void dtpRange_ValueChanged(object? sender, EventArgs e)
    {
        if (_suppressDateChange) return;

        _dateRangeActive = true;
        _ApplyFilter();
    }

    private void btnRangeToday_Click(object? sender, EventArgs e)
        => _ApplyRange(DateTime.Today, DateTime.Today);

    private void btnRangeMonth_Click(object? sender, EventArgs e)
    {
        var today = DateTime.Today;
        _ApplyRange(new DateTime(today.Year, today.Month, 1), today);
    }

    private void btnRange30_Click(object? sender, EventArgs e)
        => _ApplyRange(DateTime.Today.AddDays(-29), DateTime.Today);

    /// <summary>يضبط نطاقاً جاهزاً ويُفعّل تصفية التاريخ فوراً (RULE-N59).</summary>
    private void _ApplyRange(DateTime from, DateTime to)
    {
        _suppressDateChange = true;

        try
        {
            dtpFrom.Value = from;
            dtpTo.Value = to;
            _dateRangeActive = true;
        }
        finally
        {
            _suppressDateChange = false;
        }

        _ApplyFilter();
    }

    /// <summary>
    /// «مسح التصفية» — يمسح أدوات التصفية فقط ويعيد الشبكة إلى وضعها الافتراضي
    /// **بلا إعادة تحميل** (RULE-N148).
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearFilterInputs();
        _ApplyFilter();
        txtFilterValue.Focus();
    }

    /// <summary>تفريغ أدوات التصفية وحدها (يُستهلك من «مسح التصفية» و«Esc» و«تحديث»).</summary>
    private void _ClearFilterInputs()
    {
        _suppressDateChange = true;

        try
        {
            txtFilterValue.Clear();
            cbFilterBy.SelectedIndex = 0;

            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;

            _dateRangeActive = false;
        }
        finally
        {
            _suppressDateChange = false;
        }
    }

    /// <summary>هل توجد تصفية نشطة فعلاً؟ (RULE-N148 — الزر معطّل بلا تصفية).</summary>
    private bool _IsFilterActive()
        => !string.IsNullOrWhiteSpace(txtFilterValue.Text) || _dateRangeActive;

    /// <summary>
    /// التصفية الفورية: عمود البحث يُختار بالمؤشر لا بمقارنة نص عربي (RULE-N60)،
    /// وقيمة البحث تُهرَّب في البيت الواحد <see cref="RowFilterHelper"/>،
    /// ونطاق التاريخ يُقارن بمفتاح رقمي فلا يتأثر بثقافة التواريخ.
    /// </summary>
    private void _ApplyFilter()
    {
        if (_backupsView?.Table is null) return;
        if (txtFilterValue is null || cbFilterBy is null) return;

        var text = txtFilterValue.Text.Trim();
        var conditions = new List<string>();

        if (text.Length > 0)
        {
            conditions.Add(cbFilterBy.SelectedIndex switch
            {
                1 => RowFilterHelper.Like(ColumnDateText, text),  // «تاريخ النسخة»
                _ => RowFilterHelper.Like(ColumnFileName, text)    // «اسم الملف»
            });
        }

        if (_dateRangeActive)
        {
            var fromKey = _DateKey(dtpFrom.Value.Date);
            var toKey = _DateKey(dtpTo.Value.Date);

            if (toKey < fromKey)
                (fromKey, toKey) = (toKey, fromKey); // لا نطاق سالب — يُقرأ معكوساً بلا رسالة

            conditions.Add($"{ColumnCreatedAtKey} >= {fromKey} AND {ColumnCreatedAtKey} <= {toKey}");
        }

        try
        {
            _backupsView.RowFilter = conditions.Count > 0
                ? string.Join(" AND ", conditions)
                : string.Empty;
        }
        catch (Exception ex)
        {
            // صيغة مرفوضة لا تُسقط الشاشة ولا تُفرّغ التصفية السابقة (RULE-N60).
            Log.Error(ex, "تعذر تطبيق تصفية قائمة النسخ الاحتياطية");
        }

        _UpdateSummary();
        _UpdateEmptyState();
        _ApplyState();
    }

    // ── الملخّص وحالة الفراغ ──────────────────────────────────────────────────

    /// <summary>شريط الملخّص (RULE-N61): عدد المعروض + تاريخ أحدث نسخة معروضة.</summary>
    private void _UpdateSummary()
    {
        if (lblSummary is null) return;

        var shown = _backupsView?.Count ?? 0;
        var newest = DateTime.MinValue;
        var hasDate = false;

        if (_backupsView is not null)
        {
            foreach (DataRowView row in _backupsView)
            {
                if (row[ColumnCreatedAt] is not DateTime createdAt) continue;
                if (hasDate && createdAt <= newest) continue;

                newest = createdAt;
                hasDate = true;
            }
        }

        var lastText = hasDate ? clsFormat.DateToLong(newest) : BackupFileNameHelper.NoDateText;

        lblSummary.Text = $"عدد النسخ: {clsFormat.Number(shown)} · آخر نسخة: {lastText}";
    }

    /// <summary>
    /// حالة الفراغ: تُعرض <c>lblEmpty</c> ويُخفى صندوق الشبكة، أو العكس — تبادل
    /// <c>Visible</c> بين شقيقين كلاهما <c>Dock = Fill</c> فلا يتنازعان المساحة
    /// (RULE-N130/RULE-N135). والنص يفرّق بين «لا نسخ بعد» و«لا نتائج للتصفية».
    /// </summary>
    private void _UpdateEmptyState()
    {
        if (lblEmpty is null) return;

        var isEmpty = _backupsView is null || _backupsView.Count == 0;

        lblEmpty.Text = _backupsTable.Rows.Count == 0 ? EmptyNoDataText : EmptyNoMatchText;

        lblEmpty.Visible = isEmpty;
        gbBackups.Visible = !isEmpty;
    }

    // ── آلة الحالة (RULE-N43/N152) ────────────────────────────────────────────

    /// <summary>
    /// تُستدعى بعد **كل** انتقال: تحميل · تصفية · تغيير تحديد · بداية/نهاية عملية · تحديث.
    /// </summary>
    private void _ApplyState()
    {
        if (!_uiReady) return;
        if (btnCreateBackup is null || createBackupToolStripMenuItem is null) return;

        var hasSelection = _GetSelectedFileName() is not null;

        btnCreateBackup.Enabled = !_isBusy;
        btnRefresh.Enabled = !_isBusy;
        btnRestoreBackup.Enabled = !_isBusy && hasSelection;
        btnClearFilters.Enabled = !_isBusy && _IsFilterActive(); // RULE-N148

        createBackupToolStripMenuItem.Enabled = btnCreateBackup.Enabled;
        refreshToolStripMenuItem.Enabled = btnRefresh.Enabled;
        restoreToolStripMenuItem.Enabled = btnRestoreBackup.Enabled;
    }

    /// <summary>
    /// الزر الأساسي لـ Enter: «استعادة النسخة المحددة» عند وجود تحديد، و<b>null</b>
    /// أثناء أي عملية أو حين يكون التركيز داخل الشبكة — فلا استعادة من داخل الشبكة أبداً (RULE-N153).
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (_isBusy) return null;
        if (dgvBackups is null || dgvBackups.ContainsFocus || dgvBackups.IsCurrentCellInEditMode) return null;

        return btnRestoreBackup.Enabled ? btnRestoreBackup : null;
    }

    /// <summary>يُعلِّم الشاشة بحالة الانشغال: نص الحالة + تعطيل الأزرار (RULE-N43).</summary>
    private void _SetBusy(bool busy, string? statusText)
    {
        _isBusy = busy;
        lblStatus.Text = busy ? statusText ?? string.Empty : string.Empty;
        _ApplyState();
    }

    /// <summary>يمنع الضغط على زر معطّل (زميل <c>PerformClick</c> — احترام حالة التعطيل).</summary>
    private static void _PerformClick(Button button)
    {
        if (button is null || !button.Enabled || !button.Visible) return;

        button.PerformClick();
    }

    private string? _GetSelectedFileName()
    {
        if (dgvBackups is null || colFileName is null) return null;
        if (dgvBackups.SelectedRows.Count == 0) return null;

        var row = dgvBackups.CurrentRow ?? dgvBackups.SelectedRows[0];

        return row.Cells[colFileName.Index].Value as string;
    }

    /// <summary>تحديد الصف الأول (الأحدث) بعد الإنشاء.</summary>
    private void _SelectFirstRow()
    {
        if (dgvBackups.Rows.Count == 0) return;

        dgvBackups.CurrentCell = dgvBackups.Rows[0].Cells[colFileName.Index];
        dgvBackups.Rows[0].Selected = true;

        _ApplyState();
    }

    // ── الإجراءات ─────────────────────────────────────────────────────────────

    /// <summary>إنشاء نسخة احتياطية الآن (F2) — ثم إعادة تحميل وتحديد الصف الأول.</summary>
    private async void btnCreateBackup_Click(object? sender, EventArgs e)
    {
        if (_isBusy) return;

        _SetBusy(true, BusyCreateText);
        Cursor = Cursors.WaitCursor;

        var created = false;

        try
        {
            var result = await BackupService.CreateBackupAsync();

            if (result.IsSuccess)
            {
                created = true;
                clsDialogs.Success("تم إنشاء النسخة الاحتياطية بنجاح.");
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر إنشاء النسخة الاحتياطية.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إنشاء النسخة الاحتياطية");
            clsDialogs.Error("تعذر إنشاء النسخة الاحتياطية. يرجى إعادة المحاولة.");
        }
        finally
        {
            // قيمة الإنشاء في الوضع الحقيقي مسار كامل — لا تُستخدم كاسم ملف، فالتحديث هو المصدر.
            _SetBusy(false, null);
            Cursor = Cursors.Default;
        }

        if (!created) return;

        Cursor = Cursors.WaitCursor;

        try
        {
            await _LoadBackupListAsync();
            _SelectFirstRow();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث قائمة النسخ الاحتياطية بعد الإنشاء");
            clsDialogs.Error("تعذر تحديث قائمة النسخ الاحتياطية. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
            _ApplyState();
        }
    }

    /// <summary>استعادة النسخة المحددة (F4) — تأكيد صريح افتراضه «لا».</summary>
    private async void btnRestoreBackup_Click(object? sender, EventArgs e)
    {
        if (_isBusy) return;

        var fileName = _GetSelectedFileName();

        if (string.IsNullOrWhiteSpace(fileName))
        {
            clsDialogs.Warning("الرجاء تحديد نسخة احتياطية من القائمة أولاً.");
            return;
        }

        var message =
            "هل تريد استعادة هذه النسخة الاحتياطية؟" + Environment.NewLine + Environment.NewLine +
            "الملف: " + fileName + Environment.NewLine +
            "تاريخ النسخة: " + BackupFileNameHelper.GetDisplayDate(fileName) + Environment.NewLine + Environment.NewLine +
            "تحذير: ستُستبدل كل البيانات الحالية ببيانات هذه النسخة، ولا يمكن التراجع عن هذه العملية.";

        if (!clsDialogs.Confirm(message)) return; // Confirm افتراضه «لا» — عملية مدمّرة

        _SetBusy(true, BusyRestoreText);
        Cursor = Cursors.WaitCursor;

        try
        {
            var result = await BackupService.RestoreBackupAsync(fileName);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم استعادة النسخة الاحتياطية بنجاح.");
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر استعادة النسخة الاحتياطية.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء استعادة النسخة الاحتياطية");
            clsDialogs.Error("تعذر استعادة النسخة الاحتياطية. يرجى إعادة المحاولة.");
        }
        finally
        {
            _SetBusy(false, null);
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاع أدوات التصفية إلى
    /// الافتراضي. وليس بديلاً عن «مسح التصفية» (RULE-N148).
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        if (_isBusy) return;

        Cursor = Cursors.WaitCursor;

        try
        {
            _ClearFilterInputs();
            await _LoadBackupListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث قائمة النسخ الاحتياطية");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
            _ApplyState();
        }
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    // ── الشبكة: التحديد والنقر المزدوج وقائمة السياق (RULE-N23) ────────────────

    private void dgvBackups_SelectionChanged(object? sender, EventArgs e) => _ApplyState();

    /// <summary>النقر المزدوج على سطر = مسار الاستعادة لنفس السطر المحدد.</summary>
    private void dgvBackups_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (btnRestoreBackup.Enabled) btnRestoreBackup.PerformClick();
    }

    private void cmsBackups_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        createBackupToolStripMenuItem.Enabled = btnCreateBackup.Enabled;
        restoreToolStripMenuItem.Enabled = btnRestoreBackup.Enabled;
        refreshToolStripMenuItem.Enabled = btnRefresh.Enabled;
    }

    private void createBackupToolStripMenuItem_Click(object? sender, EventArgs e) => btnCreateBackup.PerformClick();

    private void restoreToolStripMenuItem_Click(object? sender, EventArgs e) => btnRestoreBackup.PerformClick();

    private void refreshToolStripMenuItem_Click(object? sender, EventArgs e) => btnRefresh.PerformClick();
}
