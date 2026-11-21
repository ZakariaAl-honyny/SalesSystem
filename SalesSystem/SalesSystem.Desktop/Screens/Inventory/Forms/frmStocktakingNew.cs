using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Configuration;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// «محضر جرد فعلي» — شاشة إدخال كميات الجرد الفعلية.
/// الهيكل (RULE-N79 / RULE-N135…N138): العنوان ← أوامر المحضر ← بيانات المحضر ← لجنة الجرد ← الأصناف ← ملخص الجرد.
/// سلوك إدخال الكميات وحساب الفروق بقي كما هو حرفياً.
/// <para>
/// <b>مسار الحفظ (المرحلة أ):</b> الشاشة تُنشئ المسودة وترحّلها عبر
/// <see cref="IStocktakingDesktopService"/> فقط — كل قواعد الرفض (بلا أصناف · كمية سالبة ·
/// ترحيل مرحّل · إلغاء غير مرحّل · نقص رصيد) تبقى في الخدمة وحدها وتُعرض رسالتها العربية حرفياً
/// (RULE-N167 / RULE-N172). ولا حساب للأثر على الرصيد هنا.
/// </para>
/// <para>
/// <b>الوضع الحقيقي (RULE-N165/N168):</b> العقد مُسجَّل في فرع Mock وحده، فحلّه في الوضع الحقيقي يفشل.
/// لذلك يُفحص <see cref="AppSettings.Current"/>.UseMockData قبل أي حلّ للخدمة، وتُعطَّل أوامر
/// الحفظ/الاعتماد وتُعرض لافتة عربية — الشاشة تفتح وتُقرأ ولا يُفلت أي استثناء.
/// </para>
/// </summary>
public partial class frmStocktakingNew : Form
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>وضع التجربة؟ (RULE-N165/N168) — بيت كشف الوضع الوحيد هو <see cref="AppSettings.Current"/>.</summary>
    private readonly bool _isMockMode;

    /// <summary>لافتة الوضع الحقيقي — نص واحد يظهر في شريط الوضع وعند الضغط على أي أمر محجوب.</summary>
    private const string RealModeNotice = "هذه الشاشة تعمل في وضع التجربة فقط حتى اعتماد الباك-إند";

    // ── بيانات لجنة الجرد (نصوص حرة — لا معرّفات في العقد) ────────────────────────
    private readonly string[] _employees = { "أحمد محمد", "خالد عبدالله", "سارة علي", "منى حسن", "عمر يوسف" };

    /// <summary>أصناف بيانات التجربة كما وردت من العقد — مرجع تحويل اسم السطر إلى <c>ProductId</c>.</summary>
    private readonly List<ProductDto> _products = new();

    /// <summary>المستند المُنشأ في هذه الجلسة (يُملأ بعد نجاح <c>CreateAsync</c>) — هدف <c>PostAsync</c>/<c>CancelAsync</c>.</summary>
    private int? _documentId;

    /// <summary>هل رُحّل المحضر؟ — يمنع ترحيلاً ثانياً ويُثبّت الشاشة للقراءة على هذا المستند.</summary>
    private bool _isPosted;

    /// <summary>رقم المستند كما أعادته الخدمة (يُعرض في شريط العنوان والرسائل).</summary>
    private string _documentNo = string.Empty;

    private static Color SurplusGreen => GreenThemeHelper.StatusSuccess;   // فائض +
    private static Color DeficitRed => GreenThemeHelper.StatusDanger;      // عجز -

    /// <summary>نص شريط العنوان الأساسي — تُضاف إليه حالة المحضر (RULE-N44).</summary>
    private const string FormTitle = "محضر جرد فعلي";

    public frmStocktakingNew(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _isMockMode = AppSettings.Current.UseMockData;   // RULE-N165/N168 — قبل أي حلّ للخدمة

        InitializeComponent();

        // RULE-N40…RULE-N46 — تنقل Enter/الأسهم + تحديد القيمة + تظليل الحقل النشط.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnSaveDraft, btnPost, btnCancel);

        // RULE-N47 — الاختصارات الموجودة فعلاً (F4 = اعتماد الجرد؛ Esc يغلقه CancelButton).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F4, () => btnPost.PerformClick()));

        // RULE-N147 — Enter = الزر الأساسي للوضع (اعتماد الجرد)، ومحجوب داخل شبكة العدّ.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // خطأ تحرير خلية لا يُسقط الشاشة.
        dgvLines.DataError += (s, e) => e.ThrowException = false;

        ApplyUiTheme();

        // RULE-N168 — في الوضع الحقيقي لا تُحلّ عقود المرحلة أ: الأوامر معطّلة من أول لحظة.
        _ApplyModeState();
    }

    // ── عقود الطبقة المكتبية (RULE-N04/N05) ─────────────────────────────

    /// <summary>عقد محاضر الجرد الفعلي — مسودة · ترحيل · إلغاء (المرحلة أ: Mock فقط).</summary>
    private IStocktakingDesktopService StocktakingService
        => _serviceProvider.GetRequiredService<IStocktakingDesktopService>();

    /// <summary>عقد الأصناف — يملأ شبكة العدّ، ويحلّ باركود الوحدة إلى صنف عبر <c>ResolveBarcodeAsync</c>.</summary>
    private IProductDesktopService ProductService
        => _serviceProvider.GetRequiredService<IProductDesktopService>();

    /// <summary>عقد المستودعات — قائمة المستودع الحقيقية بمعرّفاتها (الطلب يحمل <c>WarehouseId</c>).</summary>
    private IWarehouseDesktopService WarehouseService
        => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();

    // ── الثيم (RULE-N50…RULE-N54) ─────────────────────────────────────

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply(this)</c> آخر سطر دائماً.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvLines);

        // RULE-N179 — تنسيق أعمدة الكميات وقت التشغيل من البيت الواحد.
        // المصمّم يحمل القيمة التصميمية فقط (RULE-N134)، وهذه السطور تجعلها حيّة.
        GridFormatHelper.ApplyColumns(dgvLines, GridFormatKind.Quantity,
            "colBookQty", "colActualQty", "colDifference");

        // سلامة البيانات: عمودا هوية الصنف يُملآن برمجياً فقط — فلا يُغيَّر المعروض في السطر
        // عن المعرّف الذي يُرسل فعلاً (Tag/ProductId) عند الحفظ.
        dgvLines.Columns[colProduct.Index].ReadOnly = true;
        dgvLines.Columns[colBarcode.Index].ReadOnly = true;

        lblSurplusLabel.ForeColor = GreenThemeHelper.TextStrong;
        lblDeficitLabel.ForeColor = GreenThemeHelper.TextStrong;
        lblItemCountLabel.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ═════════════════════════════════════════════════════════════════
    //  LOAD — lookups من العقود + تعبئة شبكة العدّ من الأصناف
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// RULE-N25/N26 — تحميل غير متزامن مع try/catch و WaitCursor يُستعاد في finally.
    /// في الوضع الحقيقي لا يُحلّ أي عقد: تُعرض اللافتة وتُترك الشبكة فارغة للقراءة (RULE-N168).
    /// </summary>
    private async void frmStocktakingNew_Load(object sender, EventArgs e)
    {
        dtpDate.Value = DateTime.Now;
        txtCountNo.Text = "جديد";

        if (!_isMockMode)
        {
            _ApplyModeState();
            _UpdateModeHeader();
            return;
        }

        Cursor = Cursors.WaitCursor;

        try
        {
            await _LoadLookupsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحميل شاشة محضر الجرد");
            clsDialogs.Error("تعذر تحميل بيانات الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
            {
                _ApplyModeState();
                Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>يقرأ المستودعات ولجنة الجرد والأصناف من عقودها — قائمة الصنف هي مصدر شبكة العدّ.</summary>
    private async Task _LoadLookupsAsync()
    {
        // المستودعات: القائمة بمعرّفاتها (DropDownList ⇒ الإسناد بالمفتاح لا بالنص — RULE-N60).
        var warehouses = await WarehouseService.GetAllAsync();

        if (warehouses.IsSuccess && warehouses.Value is { Count: > 0 } warehouseList)
        {
            cmbWarehouse.DisplayMember = "Name";
            cmbWarehouse.ValueMember = "Id";
            cmbWarehouse.DataSource = warehouseList.ToList();
            cmbWarehouse.SelectedIndex = 0;
        }
        else
        {
            clsDialogs.Error(warehouses.Error ?? "تعذر تحميل قائمة المستودعات");
        }

        // لجنة الجرد: نصوص حرة تُرسل كما هي في الطلب.
        cmbCommitteeHead.Items.Clear();
        cmbCommitteeHead.Items.AddRange(_employees);
        cmbCommitteeMember.Items.Clear();
        cmbCommitteeMember.Items.AddRange(_employees);
        cmbStorekeeper.Items.Clear();
        cmbStorekeeper.Items.AddRange(_employees);

        // الأصناف: شبكة العدّ تُبنى من العقد (اسم · باركود · رصيد دفترى) ويُحمل معرّف الصنف في Tag.
        var products = await ProductService.GetAllAsync(includeInactive: false);

        if (!products.IsSuccess || products.Value is not { Count: > 0 } productList)
        {
            clsDialogs.Error(products.Error ?? "تعذر تحميل قائمة الأصناف");
            _RecalculateTotals();
            return;
        }

        _products.Clear();
        _products.AddRange(productList);

        dgvLines.Rows.Clear();

        foreach (var product in _products)
        {
            int rowIndex = dgvLines.Rows.Add();

            dgvLines.Rows[rowIndex].Cells[colProduct.Index].Value = product.Name;
            dgvLines.Rows[rowIndex].Cells[colBarcode.Index].Value = _BaseUnitBarcodeOf(product);
            dgvLines.Rows[rowIndex].Cells[colBookQty.Index].Value = product.CurrentStock;
            dgvLines.Rows[rowIndex].Cells[colActualQty.Index].Value = null;
            dgvLines.Rows[rowIndex].Cells[colDifference.Index].Value = null;

            // معرّف الصنف يُحمل مع السطر — لا سطر يُرسل بمعرّف مخترع (RULE-N167).
            dgvLines.Rows[rowIndex].Tag = product.Id;
        }

        _RecalculateTotals();
    }

    /// <summary>
    /// آلة الحالة المحلية (RULE-N149…N151): «حفظ كمسودة» مفعّل قبل حفظ المسودة فقط، و«اعتماد الجرد»
    /// مفعّل ما لم يُعتمد، والوضع الحقيقي يعطّل الاثنين لأن عقد المرحلة أ غير مسجَّل فيه (RULE-N165/N168).
    /// </summary>
    private void _ApplyModeState()
    {
        bool draftSaved = _documentId is not null;

        btnSaveDraft.Enabled = _isMockMode && !draftSaved && !_isPosted;   // لا مسودة ثانية لنفس الشاشة
        btnPost.Enabled = _isMockMode && !_isPosted;                       // اعتماد واحد للمحضر

        // «إلغاء» يبقى مفعّلاً دائماً: وظيفته إغلاق الشاشة (CancelButton/Esc) —
        // تعطيله يحبس النافذة بلا مخرج.
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>RULE-N168 — حارس موحّد: يرفض العمل في الوضع الحقيقي ويعرض اللافتة العربية.</summary>
    private bool _EnsureMockMode()
    {
        if (_isMockMode) return true;

        clsDialogs.Warning(RealModeNotice);
        return false;
    }

    // ═════════════════════════════════════════════════════════════════
    //  GRID — difference auto-compute + totals
    // ═════════════════════════════════════════════════════════════════

    private void dgvLines_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvLines.Rows.Count) return;
        if (e.ColumnIndex != colActualQty.Index) return;

        _UpdateRowDifference(e.RowIndex);
        _RecalculateTotals();
    }

    private void dgvLines_UserDeletedRow(object? sender, DataGridViewRowEventArgs e)
    {
        _RecalculateTotals();
    }

    /// <summary>
    /// الفرق = الكمية الفعلية − الكمية الدفترية — فائض (أخضر) / عجز (أحمر).
    /// </summary>
    private void _UpdateRowDifference(int rowIndex)
    {
        var row = dgvLines.Rows[rowIndex];
        if (row.IsNewRow) return;

        var actualCell = row.Cells[colActualQty.Index];
        decimal? actual = _ToNullableDecimal(actualCell.Value);

        // نص غير رقمي: تُفرَّغ الخلية (لا تُحسب) ويُبلَّغ المستخدم صراحةً بدل مسحه صامتاً.
        if (actual is null && actualCell.Value is string typed && !string.IsNullOrWhiteSpace(typed))
        {
            actualCell.Value = null;
            clsInputFeedback.ShowRejectTooltip(dgvLines, "الكمية الفعلية غير صحيحة — أدخل رقماً صالحاً", tooltip);
        }

        decimal book = _ToDecimal(row.Cells[colBookQty.Index].Value);

        var diffCell = row.Cells[colDifference.Index];
        if (actual.HasValue)
        {
            decimal diff = actual.Value - book;
            diffCell.Value = diff;
            diffCell.Style.ForeColor = diff > 0 ? SurplusGreen : diff < 0 ? DeficitRed : GreenThemeHelper.StatusMuted;
        }
        else
        {
            diffCell.Value = null;
            diffCell.Style.ForeColor = GreenThemeHelper.StatusMuted;
        }
    }

    /// <summary>
    /// Totals footer: إجمالي الفائض (أخضر) + إجمالي العجز (أحمر) + عدد الأصناف.
    /// Only rows with an entered physical quantity are counted.
    /// </summary>
    private void _RecalculateTotals()
    {
        decimal surplus = 0m, deficit = 0m;
        int counted = 0;

        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.IsNewRow) continue;

            decimal? actual = _ToNullableDecimal(row.Cells[colActualQty.Index].Value);
            if (!actual.HasValue) continue;

            counted++;
            decimal book = _ToDecimal(row.Cells[colBookQty.Index].Value);
            decimal diff = actual.Value - book;
            if (diff > 0) surplus += diff;
            else if (diff < 0) deficit += Math.Abs(diff);
        }

        lblSurplusValue.Text = clsFormat.Quantity(surplus);
        lblDeficitValue.Text = clsFormat.Quantity(deficit);
        lblItemCountValue.Text = clsFormat.Number(counted);

        _UpdateModeHeader();
    }

    /// <summary>
    /// مؤشر الوضع (RULE-N44): جرد جديد ← مسودة محفوظة ← معتمد، مع عدد الأصناف المعدودة — أو لافتة الوضع الحقيقي.
    /// </summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = !_isMockMode
            ? $"{FormTitle} — {RealModeNotice}"
            : _isPosted
                ? $"{FormTitle} — معتمد رقم {_documentNo} (تم ترحيل فروق الجرد على الرصيد)"
                : _documentId is not null
                    ? $"{FormTitle} — مسودة محفوظة رقم {_documentNo} (عدد الأصناف المعدودة: {lblItemCountValue.Text})"
                    : $"{FormTitle} — جرد جديد (عدد الأصناف المعدودة: {lblItemCountValue.Text})";

        // RULE-N147 — مفتاح Enter الافتراضي يتبع الوضع، ويتحرّر تلقائياً عندما تكون شبكة العدّ نشطة.
        AcceptButton = _PrimaryActionButton();
    }

    // ── RULE-N147 — Enter = الزر الأساسي للوضع ────────────────────────

    /// <summary>
    /// الزر الأساسي لضغطة <c>Enter</c> حسب وضع الشاشة (RULE-N147).
    /// يُرجع <c>null</c> عندما تكون شبكة العدّ في التركيز أو خليةٌ قيد التحرير بداخلها،
    /// لأن <c>Enter</c> داخل شبكة الجرد = تثبيت الخلية والانتقال للتالي — لا اعتماد المحضر
    /// (حماية سلامة الجرد — §2.7). ويُزيل الزر بعد الاعتماد أو في الوضع الحقيقي (RULE-N168).
    /// </summary>
    private Button? _PrimaryActionButton()
        => _isMockMode && !_isPosted && !_IsCountGridActive() ? btnPost : null;

    /// <summary>شبكة العدّ تحمل التركيز (أو خلية قيد التحرير داخلها).</summary>
    private bool _IsCountGridActive()
        => dgvLines is { IsDisposed: false } && (dgvLines.Focused || dgvLines.ContainsFocus);

    /// <summary>
    /// قفل حقول الإدخال على المستند المحفوظ: المسودة المحفوظة هي ما سيُعتمد، فلا تُعدَّل شبكتها
    /// أو رأسها بعد الحفظ (وإلا اعتُمد شيء غير المعروض). الأزرار تديرها <see cref="_ApplyModeState"/>.
    /// </summary>
    private void _SetInputsLocked()
    {
        cmbWarehouse.Enabled = false;
        dtpDate.Enabled = false;
        cmbCommitteeHead.Enabled = false;
        cmbCommitteeMember.Enabled = false;
        cmbStorekeeper.Enabled = false;

        dgvLines.ReadOnly = true;
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;

        AcceptButton = _PrimaryActionButton();
    }

    // ═════════════════════════════════════════════════════════════════
    //  FOOTER ACTIONS — save draft / post / cancel
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// RULE-N25/N26 — حفظ المسودة: يبني الطلب من الرأس + السطور المعدودة ويستدعي
    /// <c>CreateAsync</c>. الرفض (بلا أصناف · كمية سالبة) يأتي من الخدمة بنصه العربي.
    /// <para>
    /// <b>الشاشة لا تُغلق بعد الحفظ:</b> يُحفظ رقم المحضر ومعرّفه ويُقفل «حفظ كمسودة» ويُفعَّل «اعتماد الجرد»،
    /// فلا يضيع على المستخدم اعتماد ما حفظه ولا يُنشأ محضر ثانٍ بالضغط مرة أخرى.
    /// </para>
    /// </summary>
    private async void btnSaveDraft_Click(object? sender, EventArgs e)
    {
        if (!_EnsureMockMode()) return;

        // المسودة تُنشأ مرة واحدة فقط: الحفظ الثاني كان سيُنتج محضراً مكرراً.
        if (_documentId is not null)
        {
            clsDialogs.Info($"المحضر رقم {_documentNo} محفوظ كمسودة — استخدم «اعتماد الجرد» لترحيل الفروق.");
            return;
        }

        if (!_ValidateHeader()) return;

        var lines = await _BuildLinesAsync();
        if (lines is null) return;

        Cursor = Cursors.WaitCursor;
        btnSaveDraft.Enabled = false;

        try
        {
            var result = await StocktakingService.CreateAsync(_BuildRequest(lines));

            if (!result.IsSuccess || result.Value is null)
            {
                clsDialogs.Error(result.Error ?? "تعذر حفظ مسودة محضر الجرد");
                return;
            }

            _documentId = result.Value.Id;
            _documentNo = result.Value.DocumentNo;
            txtCountNo.Text = result.Value.DocumentNo;

            _SetInputsLocked();     // المسودة المحفوظة هي ما سيُعتمد — فلا تُعدَّل بعد الحفظ
            _RecalculateTotals();   // شريط الملخص والوضع (مسودة محفوظة)

            // الشاشة تبقى مفتوحة: من حفظ المسودة يعتمدها من نفس النافذة.
            clsDialogs.Success($"تم حفظ مسودة محضر الجرد رقم {_documentNo} — يمكنك اعتماده الآن");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر حفظ مسودة محضر الجرد");
            clsDialogs.Error("تعذر حفظ مسودة المحضر. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
            {
                _ApplyModeState();
                Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>
    /// RULE-N25/N26 + RULE-N167 — اعتماد الجرد: تُنشأ المسودة أولاً (إن لم توجد) ثم يُرحّل <b>نفس المستند</b>
    /// عبر <c>PostAsync</c>؛ فالمعرّف المحفوظ يمنع إنشاء محضر ثانٍ عند الاعتماد أو عند تكرار الضغط.
    /// أي رفض من الخدمة (نقص رصيد · محضر مرحّل/ملغى) يُعرض حرفياً وتبقى الشاشة مفتوحة.
    /// </summary>
    private async void btnPost_Click(object? sender, EventArgs e)
    {
        if (!_EnsureMockMode()) return;

        if (_isPosted)
        {
            clsDialogs.Info($"المحضر رقم {_documentNo} معتمد بالفعل.");
            return;
        }

        // التحقق والعدّ قبل التأكيد فقط عند عدم وجود مسودة محفوظة.
        if (_documentId is null && !_ValidateHeader()) return;
        if (_documentId is null && !_ValidateCountedRows()) return;

        // صدق الرسالة: الأسطر غير المعدودة لا تُرسل — فيُذكر العدد المعدود من الإجمالي قبل الاعتماد.
        int countedRows = 0, totalRows = 0;
        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.IsNewRow) continue;
            totalRows++;
            if (_ToNullableDecimal(row.Cells[colActualQty.Index].Value).HasValue) countedRows++;
        }

        string countedNote = countedRows < totalRows
            ? $"\nعدد الأصناف المعدودة {clsFormat.Number(countedRows)} من أصل {clsFormat.Number(totalRows)} — الأصناف غير المعدودة لن تُرسل."
            : string.Empty;

        if (!clsDialogs.Confirm($"هل أنت متأكد من اعتماد محضر الجرد الفعلي؟{countedNote}"))
            return;

        Cursor = Cursors.WaitCursor;
        btnPost.Enabled = false;

        try
        {
            if (_documentId is null && !await _CreateDraftAsync())
                return;

            if (_documentId is not int documentId) return;

            var posted = await StocktakingService.PostAsync(documentId);

            if (!posted.IsSuccess || posted.Value is null)
            {
                clsDialogs.Error(posted.Error ?? "تعذر اعتماد محضر الجرد");
                return;
            }

            _documentNo = posted.Value.DocumentNo;
            txtCountNo.Text = _documentNo;
            _isPosted = true;

            _SetInputsLocked();      // المحضر معتمد: للقراءة فقط
            _RecalculateTotals();    // مؤشر الوضع «معتمد رقم …»

            clsDialogs.Success($"تم اعتماد محضر الجرد رقم {_documentNo} — تم ترحيل فروق الجرد على الرصيد");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر اعتماد محضر الجرد");
            clsDialogs.Error("تعذر اعتماد المحضر. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
            {
                _ApplyModeState();
                Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>ينشئ المسودة مرة واحدة ويحفظ معرّفها — يُستهلك من «اعتماد الجرد» فقط.</summary>
    private async Task<bool> _CreateDraftAsync()
    {
        var lines = await _BuildLinesAsync();
        if (lines is null) return false;

        var created = await StocktakingService.CreateAsync(_BuildRequest(lines));

        if (!created.IsSuccess || created.Value is null)
        {
            clsDialogs.Error(created.Error ?? "تعذر حفظ مسودة محضر الجرد");
            return false;
        }

        _documentId = created.Value.Id;
        _documentNo = created.Value.DocumentNo;
        txtCountNo.Text = created.Value.DocumentNo;

        _SetInputsLocked();   // المسودة أُنشئت فعلاً: تُقفل الحقول قبل الاعتماد

        return true;
    }

    /// <summary>
    /// «إلغاء» — يبقى إغلاقاً بلا خدمة (Esc/<c>CancelButton</c>) كما كان، ويصبح إلغاءً للمستند
    /// عبر <c>CancelAsync</c> فقط إذا أُنشئ مستند في هذه الجلسة <b>ولم يُعتمد بعد</b>
    /// (مثال: فشل الاعتماد بعد إنشاء المسودة). رفض الخدمة (غير مرحّل) يُعرض نصاً عربياً ثم تُغلق الشاشة.
    /// <para>
    /// <b>بعد نجاح الاعتماد لا يُلغي الإغلاقُ المحضرَ:</b> إلغاء مستند معتمد يعكس أثره على الرصيد،
    /// وهو فعل صريح من شاشة القائمة لا نتيجة ضغطة Esc — فالمسار هنا إغلاق خالص.
    /// </para>
    /// </summary>
    private async void btnCancel_Click(object? sender, EventArgs e)
    {
        if (!_isMockMode || _isPosted || _documentId is not int documentId)
        {
            Close();
            return;
        }

        if (!clsDialogs.Confirm($"هل تريد إلغاء محضر الجرد رقم {_documentNo}؟"))
        {
            Close();
            return;
        }

        Cursor = Cursors.WaitCursor;

        try
        {
            var result = await StocktakingService.CancelAsync(documentId);

            if (result.IsSuccess)
                clsDialogs.Success($"تم إلغاء محضر الجرد رقم {_documentNo}");
            else
                clsDialogs.Error(result.Error ?? "تعذر إلغاء محضر الجرد");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر إلغاء محضر الجرد رقم {DocumentNo}", _documentNo);
            clsDialogs.Error("تعذر إلغاء المحضر. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }

        Close();
    }

    // ── بناء الطلب من الرأس والشبكة ───────────────────────────────────

    /// <summary>رأس المحضر كما أُدخل في الشاشة (المستودع بالمفتاح لا بالنص — RULE-N60).</summary>
    private CreateStocktakingRequest _BuildRequest(IReadOnlyList<CreateStocktakingLineRequest> lines)
        => new(
            WarehouseId: _SelectedWarehouseId(),
            Date: dtpDate.Value,
            Notes: null,
            CommitteeHead: _SelectedText(cmbCommitteeHead),
            CommitteeMember: _SelectedText(cmbCommitteeMember),
            Storekeeper: _SelectedText(cmbStorekeeper),
            Lines: lines);

    /// <summary>معرّف المستودع المختار — القائمة مُقيَّدة بعنصر <see cref="WarehouseDto"/>.</summary>
    private int _SelectedWarehouseId()
        => cmbWarehouse.SelectedItem is WarehouseDto warehouse ? warehouse.Id : 0;

    /// <summary>نص العنصر المختار في قائمة أعضاء اللجنة (نص حر — لا معرّف في العقد).</summary>
    private static string? _SelectedText(ComboBox combo)
    {
        string? text = combo.SelectedItem as string ?? combo.Text;

        return string.IsNullOrWhiteSpace(text) ? null : text.Trim();
    }

    /// <summary>
    /// سطور الطلب: كل سطر أُدخلت كميته الفعلية فقط (السطر غير المعدود لا يُرسل).
    /// المعرّف يُحلّ من <c>Tag</c> الذي ملأه تحميل الأصناف، أو من باركود الوحدة عبر <c>ResolveBarcodeAsync</c>،
    /// أو من اسم الصنف في القائمة المحمّلة (للأسطر المضافة يدوياً) — ولا يُخترع معرّف ولا يُكتب رقم ثابت.
    /// يُرجع <c>null</c> عند تعذّر تحديد صنف سطر (بعد بلاغ عربي وتحديد خانة الباركود).
    /// </summary>
    private async Task<List<CreateStocktakingLineRequest>?> _BuildLinesAsync()
    {
        var lines = new List<CreateStocktakingLineRequest>();

        for (int rowIndex = 0; rowIndex < dgvLines.Rows.Count; rowIndex++)
        {
            var row = dgvLines.Rows[rowIndex];

            if (row.IsNewRow) continue;

            decimal? actual = _ToNullableDecimal(row.Cells[colActualQty.Index].Value);

            if (!actual.HasValue) continue;   // لم يُعدّ هذا الصنف بعد

            int productId = await _ResolveProductIdAsync(row);

            if (productId <= 0)
            {
                clsDialogs.Warning($"تعذر تحديد صنف السطر رقم {rowIndex + 1} — أدخل باركوداً مسجّلاً أو اسم صنف صحيحاً.");
                dgvLines.CurrentCell = row.Cells[colBarcode.Index];
                return null;
            }

            lines.Add(new CreateStocktakingLineRequest(
                ProductId: productId,
                BookQuantity: _ToDecimal(row.Cells[colBookQty.Index].Value),
                CountedQuantity: actual.Value));
        }

        return lines;
    }

    /// <summary>
    /// يحوّل سطر الشبكة إلى <c>ProductId</c>: أولاً المعرّف المحمول في <c>Tag</c>، ثم باركود الوحدة عبر
    /// <see cref="IProductDesktopService.ResolveBarcodeAsync"/> (قرار المالك 2026-11-20: الباركود ملك الوحدة
    /// لا الصنف، فلا مطابقة على قائمة الأصناف)، ثم اسم الصنف من القائمة المحمّلة — بلا أرقام ثابتة.
    /// يُعيد صفراً حين لا يُحسم الصنف.
    /// </summary>
    private async Task<int> _ResolveProductIdAsync(DataGridViewRow row)
    {
        if (row.Tag is int tagged && tagged > 0) return tagged;

        string? barcode = (row.Cells[colBarcode.Index].Value as string)?.Trim();
        string? name = (row.Cells[colProduct.Index].Value as string)?.Trim();

        if (!string.IsNullOrWhiteSpace(barcode))
        {
            var resolved = await ProductService.ResolveBarcodeAsync(barcode);

            if (resolved.IsSuccess && resolved.Value is { ProductId: > 0 } resolution)
            {
                row.Tag = resolution.ProductId;   // تثبيت المعرّف فلا تُعاد المطابقة في المرة القادمة

                // العمود للقراءة فقط ويُملأ برمجياً (RULE-N179): يُعرض باركود الوحدة المحلولة المعتمد.
                row.Cells[colBarcode.Index].Value = string.IsNullOrWhiteSpace(resolution.Barcode)
                    ? barcode
                    : resolution.Barcode!.Trim();

                return resolution.ProductId;
            }
        }

        if (string.IsNullOrWhiteSpace(name)) return 0;

        var product = _products.FirstOrDefault(p =>
            string.Equals(p.Name.Trim(), name, StringComparison.OrdinalIgnoreCase));

        if (product is null) return 0;

        row.Tag = product.Id;
        return product.Id;
    }

    /// <summary>
    /// باركود الوحدة الأساسية للصنف — <see cref="ProductDto.Barcode"/> قيمة مشتقة من <c>ProductUnits.Barcode</c>
    /// للوحدة الأساسية، أو فراغ إن كانت بلا باركود. شاشة الجرد تعدّ الصنف بوحدته الأساسية (عقد الترحيل يحمل
    /// <c>ProductId</c> فقط)، وباركود الوحدة المحلولة يُصحّح المعروض عند البحث اليدوي.
    /// </summary>
    private static string _BaseUnitBarcodeOf(ProductDto product)
        => product.Barcode?.Trim() ?? string.Empty;

    // ── Context menu (RULE-N23) ───────────────────────────────────────

    private void miPost_Click(object? sender, EventArgs e)
        => btnPost.PerformClick();

    private void miDeleteRow_Click(object? sender, EventArgs e)
    {
        // بعد حفظ المسودة/الاعتماد الشاشة للقراءة: حذف سطر يجعل المعروض يخالف ما سيُعتمد.
        if (_documentId is not null || _isPosted)
        {
            clsDialogs.Info("لا يمكن حذف سطر من محضر محفوظ — الشاشة للقراءة بعد الحفظ.");
            return;
        }

        if (dgvLines.CurrentRow is not { IsNewRow: false } row) return;

        dgvLines.Rows.Remove(row);
        _RecalculateTotals();
    }

    // ═════════════════════════════════════════════════════════════════
    //  VALIDATION
    // ═════════════════════════════════════════════════════════════════

    private bool _ValidateHeader()
    {
        if (cmbWarehouse.SelectedIndex < 0)
        {
            errorProvider.SetError(cmbWarehouse, "يرجى اختيار المستودع");
            clsInputFeedback.ShowRejectTooltip(cmbWarehouse, "يرجى اختيار المستودع", tooltip);
            clsDialogs.Warning("يرجى اختيار المستودع.");
            cmbWarehouse.Focus();
            return false;
        }

        errorProvider.SetError(cmbWarehouse, "");
        return true;
    }

    private bool _ValidateCountedRows()
    {
        int counted = 0;

        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.IsNewRow) continue;

            decimal? actual = _ToNullableDecimal(row.Cells[colActualQty.Index].Value);
            if (!actual.HasValue) continue;

            if (actual.Value < 0)
            {
                clsDialogs.Warning("الكمية الفعلية لا يمكن أن تكون سالبة.");
                dgvLines.CurrentCell = row.Cells[colActualQty.Index];
                return false;
            }
            counted++;
        }

        if (counted == 0)
        {
            clsDialogs.Warning("يجب إدخال الكمية الفعلية لصنف واحد على الأقل قبل الاعتماد.");
            return false;
        }
        return true;
    }

    // ── Cell value helpers ─────────────────────────────────────────────

    private static decimal _ToDecimal(object? value)
        => _ToNullableDecimal(value) ?? 0m;

    /// <summary>
    /// قراءة كمية من خلية — القارئ الوحيد هو <c>clsFormat.TryParseQuantity</c> (RULE-N169):
    /// يطبّع الأرقام العربية والفواصل وفق الإعدادات الحيّة ولا يرمي استثناءً.
    /// </summary>
    private static decimal? _ToNullableDecimal(object? value)
    {
        if (value == null) return null;
        if (value is decimal dec) return dec;
        if (value is string s && string.IsNullOrWhiteSpace(s)) return null;

        return clsFormat.TryParseQuantity(value.ToString(), out decimal parsed) ? parsed : null;
    }
}
