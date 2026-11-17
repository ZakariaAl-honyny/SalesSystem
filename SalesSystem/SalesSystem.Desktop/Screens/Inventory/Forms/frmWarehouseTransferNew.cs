using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Configuration;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// «سند تحويل مخزني» — شاشة إدخال تحويل بين مستودعين.
/// الهيكل (RULE-N79 / RULE-N135…N138): العنوان ← أوامر السند ← بيانات السند ← الأصناف ← شريط الملاحظة السفلي.
/// منطق التحقق وحساب الكميات بقي كما هو حرفياً.
/// <para>
/// <b>مسار الحفظ والترحيل (المرحلة أ):</b> السند يُنشأ كمسودة عبر <see cref="IWarehouseTransferDesktopService"/>
/// ثم يُرحّل من نفس الشاشة (`CreateAsync` ← `PostAsync`) — الشاشة لا تُغلق بعد حفظ المسودة.
/// كل قواعد الرفض (بلا أصناف · كمية صفر أو سالبة · مصدر = هدف · ترحيل مرحّل · إلغاء غير مرحّل ·
/// نقص رصيد المصدر) تبقى في الخدمة وحدها وتُعرض رسالتها العربية حرفياً (RULE-N167 / RULE-N172).
/// </para>
/// <para>
/// <b>الوضع الحقيقي (RULE-N165/N168):</b> العقد مُسجَّل في فرع Mock وحده، فحلّه في الوضع الحقيقي يفشل.
/// لذلك يُفحص <see cref="AppSettings.Current"/>.UseMockData قبل أي حلّ للخدمة، ويُعطَّل أمر الحفظ
/// وتُعرض لافتة عربية — الشاشة تفتح وتُقرأ ولا يُفلت أي استثناء.
/// </para>
/// </summary>
public partial class frmWarehouseTransferNew : Form
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>وضع التجربة؟ (RULE-N165/N168) — بيت كشف الوضع الوحيد هو <see cref="AppSettings.Current"/>.</summary>
    private readonly bool _isMockMode;

    /// <summary>لافتة الوضع الحقيقي — نص واحد يظهر في شريط الوضع وعند الضغط على الأمر المحجوب.</summary>
    private const string RealModeNotice = "هذه الشاشة تعمل في وضع التجربة فقط حتى اعتماد الباك-إند";

    // ── وحدة السطر: نص حر للعرض فقط (العقد لا يحمل وحدة في سطر التحويل) ────────────────
    private readonly string[] _units = { "كيس", "كرتونة", "قطعة", "عبوة", "زجاجة", "علبة" };

    /// <summary>أصناف بيانات التجربة كما وردت من العقد — مصدر قائمة عمود الصنف ويُحلّ منها المعرّف.</summary>
    private readonly List<ProductDto> _products = new();

    /// <summary>
    /// المستند المُنشأ في هذه الجلسة (يُملأ بعد نجاح <c>CreateAsync</c>) — هدف <c>PostAsync</c>.
    /// وجوده يعني أن المسودة محفوظة، فيُقفل «حفظ السند» فلا يُنشأ سند مكرر عند الترحيل.
    /// </summary>
    private int? _documentId;

    /// <summary>هل رُحّل السند؟ — يمنع ترحيلاً ثانياً ويُثبّت الشاشة للقراءة على هذا المستند.</summary>
    private bool _isPosted;

    /// <summary>رقم السند كما أعادته الخدمة (يُعرض في شريط العنوان والرسائل).</summary>
    private string _documentNo = string.Empty;

    private static Color QtyPositive => GreenThemeHelper.StatusSuccess;    // كمية موجبة
    private static Color QtyZero => GreenThemeHelper.StatusMuted;          // بلا كمية

    /// <summary>
    /// خط الكمية الموجبة — نسخة واحدة تُعاد (لا <c>new Font</c> لكل خلية في كل تحديث) فلا تتسرّب
    /// مقابض GDI+؛ والخط العادي يُعاد إلى وراثة الشبكة بـ<c>null</c>.
    /// </summary>
    private static readonly Font QtyPositiveFont = new("Tahoma", 10F, FontStyle.Bold);

    /// <summary>نص شريط العنوان الأساسي — تُضاف إليه حالة السند (RULE-N44).</summary>
    private const string FormTitle = "سند تحويل مخزني";

    /// <summary>التركيز داخل شبكة السطور — Enter هناك تنقّل داخل الشبكة لا حفظ (RULE-N147 / RULE-N40).</summary>
    private bool _linesGridHasFocus;

    public frmWarehouseTransferNew(IServiceProvider serviceProvider)
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
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnPost, btnCancel);

        // RULE-N47 — F4 يخدم الخطوة المتاحة: حفظ المسودة ثم ترحيلها (Esc يغلقه CancelButton).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F4, () =>
            {
                if (_documentId is null) btnSave.PerformClick();
                else btnPost.PerformClick();
            }
        ));

        // RULE-N147 — Enter = الزر الأساسي للوضع الحالي («حفظ السند» ثم «ترحيل» بعد الحفظ)، ولا يعمل داخل شبكة السطور.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        // حماية الشبكة (RULE-N153): طالما التركيز داخل السطور لا يُسند النموذج Enter إلى الحفظ/الترحيل.
        dgvLines.Enter += (s, e) =>
        {
            _linesGridHasFocus = true;
            AcceptButton = _PrimaryActionButton();
        };
        dgvLines.Leave += (s, e) =>
        {
            _linesGridHasFocus = false;
            AcceptButton = _PrimaryActionButton();
        };

        // خطأ تحرير خلية (صنف/وحدة من قائمة) لا يُسقط الشاشة.
        dgvLines.DataError += (s, e) => e.ThrowException = false;

        ApplyUiTheme();

        // RULE-N168 — في الوضع الحقيقي لا تُحلّ عقود المرحلة أ: الأمر معطّل من أول لحظة.
        _ApplyModeState();
    }

    // ── عقود الطبقة المكتبية (RULE-N04/N05) ─────────────────────────────

    /// <summary>عقد سندات التحويل المخزني — إنشاء · ترحيل · إلغاء (المرحلة أ: Mock فقط).</summary>
    private IWarehouseTransferDesktopService TransferService
        => _serviceProvider.GetRequiredService<IWarehouseTransferDesktopService>();

    /// <summary>عقد الأصناف — يملأ عمود الصنف ويحوّل قيمة السطر إلى <c>ProductId</c>.</summary>
    private IProductDesktopService ProductService
        => _serviceProvider.GetRequiredService<IProductDesktopService>();

    /// <summary>عقد المستودعات — قائمتا الإرسال/الاستلام بمعرّفاتهما الحقيقية.</summary>
    private IWarehouseDesktopService WarehouseService
        => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();

    // ── الثيم (RULE-N50…RULE-N54) ─────────────────────────────────────

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply(this)</c> آخر سطر دائماً.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvLines);

        // RULE-N179 — تنسيق الأعمدة وقت التشغيل من البيت الواحد (كمية + تاريخ انتهاء).
        // المصمّم يحمل القيمة التصميمية فقط (RULE-N134)، وهذه السطور تجعلها حيّة.
        GridFormatHelper.ApplyColumn(dgvLines, "colQuantity", GridFormatKind.Quantity);
        GridFormatHelper.ApplyColumn(dgvLines, "colExpiryDate", GridFormatKind.DateDayMonthYear);

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ═════════════════════════════════════════════════════════════════
    //  LOAD — lookups من العقود + تعبئة سطور السند من الأصناف
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// RULE-N25/N26 — تحميل غير متزامن مع try/catch و WaitCursor يُستعاد في finally.
    /// في الوضع الحقيقي لا يُحلّ أي عقد: تُعرض اللافتة ويُترك المحرر فارغاً للقراءة (RULE-N168).
    /// </summary>
    private async void frmWarehouseTransferNew_Load(object sender, EventArgs e)
    {
        dtpDate.Value = DateTime.Now;
        txtTransferNo.Text = "جديد";

        if (!_isMockMode)
        {
            _ApplyModeState();
            _ApplyQuantityStyling();
            return;
        }

        Cursor = Cursors.WaitCursor;

        try
        {
            await _LoadLookupsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحميل شاشة سند التحويل المخزني");
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

    /// <summary>يقرأ المستودعات والأصناف من عقودها — قائمة الأصناف هي مصدر عمود الصنف وسطور السند.</summary>
    private async Task _LoadLookupsAsync()
    {
        // المستودعات: القائمة بمعرّفاتها (DropDownList ⇒ الإسناد بالمفتاح لا بالنص — RULE-N60).
        var warehouses = await WarehouseService.GetAllAsync();

        if (warehouses.IsSuccess && warehouses.Value is { Count: > 0 } warehouseList)
        {
            var data = warehouseList.ToList();

            cmbFromWarehouse.DisplayMember = "Name";
            cmbFromWarehouse.ValueMember = "Id";
            cmbFromWarehouse.DataSource = data.ToList();   // نسخة مستقلة لكل قائمة (DataSource مشترك يوحّد الاختيار)
            cmbFromWarehouse.SelectedIndex = 0;

            cmbToWarehouse.DisplayMember = "Name";
            cmbToWarehouse.ValueMember = "Id";
            cmbToWarehouse.DataSource = data;
            cmbToWarehouse.SelectedIndex = data.Count > 1 ? 1 : 0;
        }
        else
        {
            clsDialogs.Error(warehouses.Error ?? "تعذر تحميل قائمة المستودعات");
        }

        // الأصناف: عمود الصنف يحمل المعرّف (ValueMember = Id) ويعرض الاسم (DisplayMember = Name).
        var products = await ProductService.GetAllAsync(includeInactive: false);

        if (!products.IsSuccess || products.Value is not { Count: > 0 } productList)
        {
            clsDialogs.Error(products.Error ?? "تعذر تحميل قائمة الأصناف");
            _ApplyQuantityStyling();
            return;
        }

        _products.Clear();
        _products.AddRange(productList);

        colUnit.Items.Clear();
        colUnit.Items.AddRange(_units);

        colProduct.DataSource = _products.ToList();
        colProduct.DisplayMember = "Name";
        colProduct.ValueMember = "Id";

        dgvLines.Rows.Clear();

        foreach (var product in _products)
        {
            int rowIndex = dgvLines.Rows.Add();

            dgvLines.Rows[rowIndex].Cells[colProduct.Index].Value = product.Id;
            dgvLines.Rows[rowIndex].Cells[colQuantity.Index].Value = 0m;
            dgvLines.Rows[rowIndex].Cells[colBatchNo.Index].Value = null;
            dgvLines.Rows[rowIndex].Cells[colExpiryDate.Index].Value = null;
        }

        _ApplyQuantityStyling();
    }

    /// <summary>
    /// آلة الحالة المحلية (RULE-N149…N151): الحفظ مفعّل للإضافة فقط، والترحيل مفعّل ما لم يُرحّل السند،
    /// والوضع الحقيقي يعطّل الاثنين لأن عقد المرحلة أ غير مسجَّل فيه (RULE-N165/N168).
    /// </summary>
    private void _ApplyModeState()
    {
        bool draftSaved = _documentId is not null;

        btnSave.Enabled = _isMockMode && !draftSaved && !_isPosted;   // لا مسودة ثانية لنفس الشاشة
        btnPost.Enabled = _isMockMode && !_isPosted;                  // ترحيل واحد للمستند

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
    //  GRID — quantity editing polish
    // ═════════════════════════════════════════════════════════════════

    private void dgvLines_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvLines.Rows.Count) return;
        if (e.ColumnIndex != colQuantity.Index) return;

        var cell = dgvLines.Rows[e.RowIndex].Cells[colQuantity.Index];
        decimal? qty = _ToNullableDecimal(cell.Value);

        // نص غير رقمي: تُفرَّغ الخلية (لا تُرسل) ويُبلَّغ المستخدم صراحةً بدل المسح الصامت.
        if (qty is null && cell.Value is string typed && !string.IsNullOrWhiteSpace(typed))
        {
            cell.Value = null;
            clsInputFeedback.ShowRejectTooltip(dgvLines, "الكمية غير صحيحة — أدخل رقماً صالحاً", tooltip);
        }

        _ApplyQuantityStyling();
    }

    private void dgvLines_UserAddedRow(object? sender, DataGridViewRowEventArgs e)
    {
        e.Row.Cells[colQuantity.Index].Value = 0m;
        _ApplyQuantityStyling();
    }

    private void dgvLines_UserDeletedRow(object? sender, DataGridViewRowEventArgs e)
    {
        _ApplyQuantityStyling();
    }

    /// <summary>كمية > 0 بالخط العريض الأخضر، وإلا رمادي خافت — ويُحدَّث مؤشر الوضع بعدّاد الأسطر.</summary>
    private void _ApplyQuantityStyling()
    {
        int linesWithQty = 0;

        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.IsNewRow) continue;

            var cell = row.Cells[colQuantity.Index];
            decimal? qty = _ToNullableDecimal(cell.Value);

            if (qty.HasValue && qty.Value > 0) linesWithQty++;

            cell.Style.ForeColor = qty.HasValue && qty.Value > 0 ? QtyPositive : QtyZero;

            // خط الكمية الموجبة من النسخة الواحدة، وسالب/فارغ يُعاد لوراثة خط الشبكة (null) — لا خط جديد لكل خلية.
            cell.Style.Font = qty.HasValue && qty.Value > 0 ? QtyPositiveFont : null;
        }

        _UpdateModeHeader(linesWithQty);
    }

    /// <summary>
    /// مؤشر الوضع (RULE-N44): جديد ← مسودة محفوظة ← مرحّل، مع عدّاد الأسطر ذات الكمية —
    /// ويُحدَّث معه زر Enter الأساسي (RULE-N147).
    /// </summary>
    private void _UpdateModeHeader(int linesWithQty)
    {
        lblMode.Text = !_isMockMode
            ? $"{FormTitle} — {RealModeNotice}"
            : _isPosted
                ? $"{FormTitle} — مرحّل رقم {_documentNo} (تم خصم الكميات من مستودع المصدر)"
                : _documentId is not null
                    ? $"{FormTitle} — مسودة محفوظة رقم {_documentNo} (أسطر بكمية: {linesWithQty})"
                    : $"{FormTitle} — مستند جديد (أسطر بكمية: {linesWithQty})";

        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147): «حفظ السند» قبل الحفظ، ثم «ترحيل» بعد حفظ المسودة،
    /// و<b>لا زر</b> بعد الترحيل (المستند مُقفل) أو في الوضع الحقيقي (RULE-N168).
    /// يُرجع <c>null</c> عندما يكون التركيز داخل شبكة السطور، فيبقى Enter تنقّلاً داخل الشبكة
    /// ولا يُرحّل السند (RULE-N153 + حماية المخزون §2.7).
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (!_isMockMode || _isPosted) return null;

        if (_linesGridHasFocus || dgvLines.ContainsFocus || dgvLines.IsCurrentCellInEditMode)
            return null;

        return _documentId is null ? btnSave : btnPost;
    }

    /// <summary>
    /// قفل حقول الإدخال على المستند المحفوظ: المسودة المحفوظة هي ما سيُرحّل، فلا تُعدَّل شبكتها
    /// أو رأسها بعد الحفظ (وإلا رُحّل شيء غير المعروض). الأزرار تديرها <see cref="_ApplyModeState"/>.
    /// </summary>
    private void _SetInputsLocked()
    {
        cmbFromWarehouse.Enabled = false;
        cmbToWarehouse.Enabled = false;
        dtpDate.Enabled = false;
        txtDriverName.Enabled = false;
        txtVehiclePlate.Enabled = false;
        txtReference.Enabled = false;

        dgvLines.ReadOnly = true;
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;

        AcceptButton = _PrimaryActionButton();
    }

    // ═════════════════════════════════════════════════════════════════
    //  FOOTER ACTIONS — save draft / post / cancel
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// RULE-N25/N26 — حفظ السند كمسودة: يبني الطلب من الرأس + السطور ذات الكمية الموجبة ويستدعي
    /// <c>CreateAsync</c>. الرفض (بلا أصناف · كمية صفر · مصدر = هدف) يأتي من الخدمة بنصه العربي.
    /// <para>
    /// <b>الشاشة لا تُغلق بعد الحفظ:</b> يُحفظ رقم السند ومعرّفه ويُقفل زر «حفظ» ويُفعَّل «ترحيل»،
    /// فلا يضيع على المستخدم ترحيل ما حفظه ولا يُنشأ سند ثانٍ بالضغط مرة أخرى.
    /// </para>
    /// </summary>
    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (!_EnsureMockMode()) return;

        // المسودة تُنشأ مرة واحدة فقط: الحفظ الثاني كان سيُنتج سنداً مكرراً.
        if (_documentId is not null)
        {
            clsDialogs.Info($"السند رقم {_documentNo} محفوظ كمسودة — استخدم «ترحيل» لخصم الكميات من المصدر.");
            return;
        }

        if (!_ValidateHeader()) return;
        if (!_ValidateLines()) return;
        if (!_TryBuildLines(out var lines)) return;

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            var result = await TransferService.CreateAsync(new CreateWarehouseTransferRequest(
                FromWarehouseId: _SelectedWarehouseId(cmbFromWarehouse),
                ToWarehouseId: _SelectedWarehouseId(cmbToWarehouse),
                Date: dtpDate.Value,
                Notes: _ComposeNotes(),
                Lines: lines));

            if (!result.IsSuccess || result.Value is null)
            {
                clsDialogs.Error(result.Error ?? "تعذر حفظ سند التحويل");
                return;
            }

            _documentId = result.Value.Id;
            _documentNo = result.Value.DocumentNo;
            txtTransferNo.Text = result.Value.DocumentNo;

            _SetInputsLocked();        // المسودة المحفوظة هي ما سيُرحّل — فلا تُعدَّل بعد الحفظ
            _ApplyQuantityStyling();   // مؤشر الوضع (مسودة محفوظة) + عدّاد الأسطر

            // الشاشة تبقى مفتوحة: من حفظ المسودة يُرحّلها من نفس النافذة.
            clsDialogs.Success($"تم حفظ سند التحويل رقم {_documentNo} كمسودة — يمكنك ترحيله الآن");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر حفظ سند التحويل المخزني");
            clsDialogs.Error("تعذر حفظ سند التحويل. يرجى إعادة المحاولة.");
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
    /// RULE-N25/N26 + RULE-N167 — ترحيل السند: إن لم تكن المسودة محفوظة تُنشأ أولاً <b>مرة واحدة</b>
    /// ثم يُرحّل المعرّف نفسه عبر <c>PostAsync</c> (فالترحيل لا يقبل مستنداً غير منشأ).
    /// أي رفض من الخدمة (نقص رصيد المصدر · مستند مُرحّل · سند ملغى…) يُعرض نصه العربي حرفياً وتبقى الشاشة مفتوحة.
    /// </summary>
    private async void btnPost_Click(object? sender, EventArgs e)
    {
        if (!_EnsureMockMode()) return;

        if (_isPosted)
        {
            clsDialogs.Info($"السند رقم {_documentNo} مُرحّل بالفعل.");
            return;
        }

        // التحقق والسطور قبل التأكيد فقط عند عدم وجود مسودة (المسودة المحفوظة رُفضت خطأً من الخدمة سابقاً).
        List<CreateWarehouseTransferLineRequest>? lines = null;

        if (_documentId is null)
        {
            if (!_ValidateHeader()) return;
            if (!_ValidateLines()) return;
            if (!_TryBuildLines(out var built)) return;

            lines = built;
        }

        if (!clsDialogs.Confirm("هل أنت متأكد من ترحيل سند التحويل؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnPost.Enabled = false;

        try
        {
            // 1) إنشاء المسودة مرة واحدة فقط — وجود المعرّف يمنع أي إنشاء ثانٍ عند الضغط ثانية.
            if (_documentId is null)
            {
                var created = await TransferService.CreateAsync(new CreateWarehouseTransferRequest(
                    FromWarehouseId: _SelectedWarehouseId(cmbFromWarehouse),
                    ToWarehouseId: _SelectedWarehouseId(cmbToWarehouse),
                    Date: dtpDate.Value,
                    Notes: _ComposeNotes(),
                    Lines: lines));

                if (!created.IsSuccess || created.Value is null)
                {
                    clsDialogs.Error(created.Error ?? "تعذر حفظ سند التحويل قبل ترحيله");
                    return;
                }

                _documentId = created.Value.Id;
                _documentNo = created.Value.DocumentNo;
                txtTransferNo.Text = created.Value.DocumentNo;

                _SetInputsLocked();   // المسودة أُنشئت فعلاً: تُقفل الحقول قبل الترحيل
            }

            if (_documentId is not int documentId) return;

            // 2) ترحيل نفس المستند (خصم المصدر وإضافة الهدف داخل الخدمة وحدها).
            var posted = await TransferService.PostAsync(documentId);

            if (!posted.IsSuccess || posted.Value is null)
            {
                clsDialogs.Error(posted.Error ?? "تعذر ترحيل سند التحويل");
                return;
            }

            _documentNo = posted.Value.DocumentNo;
            txtTransferNo.Text = _documentNo;
            _isPosted = true;

            _SetInputsLocked();            // المستند مُقفل للقراءة بعد الترحيل
            _ApplyQuantityStyling();       // مؤشر الوضع «مرحّل رقم …»

            clsDialogs.Success($"تم ترحيل سند التحويل رقم {_documentNo} — تمت تسوية رصيد المستودعين");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر ترحيل سند التحويل المخزني");
            clsDialogs.Error("تعذر ترحيل سند التحويل. يرجى إعادة المحاولة.");
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

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        Close();
    }

    // ── بناء الطلب من الرأس والشبكة ───────────────────────────────────

    /// <summary>معرّف المستودع المختار — القائمة مُقيَّدة بعنصر <see cref="WarehouseDto"/>.</summary>
    private static int _SelectedWarehouseId(ComboBox combo)
        => combo.SelectedItem is WarehouseDto warehouse ? warehouse.Id : 0;

    /// <summary>
    /// ملاحظات السند: العقد يحمل <c>Notes</c> فقط، فتُجمع بيانات الرأس الحرة (المرجع · السائق ·
    /// لوحة المركبة) في نص واحد بدل إسقاطها صامتاً عند الحفظ.
    /// </summary>
    private string? _ComposeNotes()
    {
        var parts = new List<string>(3);

        if (!string.IsNullOrWhiteSpace(txtReference.Text)) parts.Add($"المرجع: {txtReference.Text.Trim()}");
        if (!string.IsNullOrWhiteSpace(txtDriverName.Text)) parts.Add($"السائق: {txtDriverName.Text.Trim()}");
        if (!string.IsNullOrWhiteSpace(txtVehiclePlate.Text)) parts.Add($"لوحة المركبة: {txtVehiclePlate.Text.Trim()}");

        return parts.Count == 0 ? null : string.Join(" — ", parts);
    }

    /// <summary>
    /// سطور الطلب: كل سطر كميته أكبر من صفر فقط (السطور الصفرية لا تُرسل).
    /// المعرّف يُقرأ من قيمة عمود الصنف (<c>ValueMember = Id</c>) أو يُطابق بالاسم عبر نفس قائمة الأصناف.
    /// </summary>
    private bool _TryBuildLines(out List<CreateWarehouseTransferLineRequest> lines)
    {
        lines = new List<CreateWarehouseTransferLineRequest>();

        for (int rowIndex = 0; rowIndex < dgvLines.Rows.Count; rowIndex++)
        {
            var row = dgvLines.Rows[rowIndex];

            if (row.IsNewRow) continue;

            decimal? quantity = _ToNullableDecimal(row.Cells[colQuantity.Index].Value);

            if (!quantity.HasValue || quantity.Value <= 0m) continue;

            if (!_TryResolveProductId(row, out int productId))
            {
                clsDialogs.Warning($"تعذر تحديد صنف السطر رقم {rowIndex + 1} — اختر الصنف من القائمة.");
                dgvLines.CurrentCell = row.Cells[colProduct.Index];
                return false;
            }

            string? batchNo = (row.Cells[colBatchNo.Index].Value as string)?.Trim();

            lines.Add(new CreateWarehouseTransferLineRequest(
                ProductId: productId,
                Quantity: quantity.Value,
                BatchNo: string.IsNullOrWhiteSpace(batchNo) ? null : batchNo,
                ExpiryDate: _ToDateOnly(row.Cells[colExpiryDate.Index].Value)));
        }

        return true;
    }

    /// <summary>
    /// يحوّل سطر الشبكة إلى <c>ProductId</c>: قيمة عمود الصنف (معرّف من <c>ValueMember</c>) أولاً،
    /// ثم مطابقة الاسم من قائمة الأصناف المحمّلة — بلا أرقام ثابتة وبلا خدمة إضافية.
    /// </summary>
    private bool _TryResolveProductId(DataGridViewRow row, out int productId)
    {
        object? value = row.Cells[colProduct.Index].Value;

        if (value is int id && id > 0)
        {
            productId = id;
            return true;
        }

        if (value is short shortId && shortId > 0)
        {
            productId = shortId;
            return true;
        }

        string? name = (value as string)?.Trim();

        var product = string.IsNullOrWhiteSpace(name)
            ? null
            : _products.FirstOrDefault(p => string.Equals(p.Name.Trim(), name, StringComparison.OrdinalIgnoreCase));

        productId = product?.Id ?? 0;

        return productId > 0;
    }

    /// <summary>تاريخ الصلاحية من الخلية (تاريخ أو نص) — <c>null</c> عند الفراغ أو عدم الصلاحية.</summary>
    private static DateOnly? _ToDateOnly(object? value)
    {
        if (value is DateTime dateTime) return DateOnly.FromDateTime(dateTime);
        if (value is DateOnly dateOnly) return dateOnly;
        if (value is string text
            && DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out var parsed))
            return DateOnly.FromDateTime(parsed);

        return null;
    }

    // ── Context menu (RULE-N23) ───────────────────────────────────────

    private void miSave_Click(object? sender, EventArgs e)
        => btnSave.PerformClick();

    private void miDeleteRow_Click(object? sender, EventArgs e)
    {
        // بعد حفظ المسودة/الترحيل الشاشة للقراءة: حذف سطر يجعل المعروض يخالف ما سيُرحّل.
        if (_documentId is not null || _isPosted)
        {
            clsDialogs.Info("لا يمكن حذف سطر من سند محفوظ — الشاشة للقراءة بعد الحفظ.");
            return;
        }

        if (dgvLines.CurrentRow is not { IsNewRow: false } row) return;

        dgvLines.Rows.Remove(row);
        _ApplyQuantityStyling();
    }

    // ═════════════════════════════════════════════════════════════════
    //  VALIDATION — Arabic messages
    // ═════════════════════════════════════════════════════════════════

    private bool _ValidateHeader()
    {
        if (cmbFromWarehouse.SelectedIndex < 0 || cmbToWarehouse.SelectedIndex < 0)
        {
            clsDialogs.Warning("يرجى اختيار مستودع الإرسال ومستودع الاستلام.");
            return false;
        }

        if (cmbFromWarehouse.SelectedIndex == cmbToWarehouse.SelectedIndex)
        {
            clsDialogs.Warning("لا يمكن أن يكون مستودع الإرسال ومستودع الاستلام متطابقين.");
            cmbToWarehouse.Focus();
            return false;
        }

        return true;
    }

    private bool _ValidateLines()
    {
        int linesWithQty = 0;

        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.IsNewRow) continue;

            decimal? qty = _ToNullableDecimal(row.Cells[colQuantity.Index].Value);

            if (qty.HasValue && qty.Value < 0)
            {
                clsDialogs.Warning("الكمية لا يمكن أن تكون سالبة.");
                dgvLines.CurrentCell = row.Cells[colQuantity.Index];
                return false;
            }

            if (qty.HasValue && qty.Value > 0) linesWithQty++;
        }

        if (linesWithQty == 0)
        {
            clsDialogs.Warning("يجب إدخال كمية أكبر من صفر لصنف واحد على الأقل.");
            return false;
        }

        return true;
    }

    // ── Cell value helpers ─────────────────────────────────────────────

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

    private void lblDriverName_Click(object sender, EventArgs e)
    {

    }
}
