using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Products.Forms;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// مستند تسوية مخزنية (إدخال/ترحيل): العنوان ← أوامر التسوية ← بيانات التسوية ← الأصناف ← الإجماليات.
/// <para>
/// المعيار المطبَّق: §2.34 (تجميع وترتيب الكتل) + §2.27 (الستة من KeyboardHelper)
/// + §2.19/§2.35 (الثيم والمصمّم = وقت التشغيل) + RULE-N97 (التفقيط الحيّ على المبلغ).
/// مستند إدخال: بلا <c>gbFilter</c>/<c>pnlSummary</c>.
/// </para>
/// <para>
/// ⚠️ منطق التسوية نفسه لم يُمَس: حساب الفرق لكل سطر، تعديل <c>WarehouseStocks</c>،
/// تسجيل <c>InventoryTransactions</c>، والترحيل داخل <c>BeginTransactionAsync</c>
/// كلها في طبقة الخدمة — الشاشة تستدعي <see cref="IInventoryAdjustmentDesktopService"/> فقط.
/// </para>
/// <para>
/// <b>مسار إضافة الصنف (بند الإغلاق):</b> «اختيار صنف» ⇒ <c>frmProductSelection</c> (منتقي الأصناف القائم)
/// ⇒ «إضافة صنف» يقرأ الكميتين بـ<c>clsFormat.TryParseQuantity</c> ويضيف السطر إلى جدول البنود، ثم
/// تُرسل السطور عند الحفظ عبر <c>AddLineAsync</c> — وهو المسار الوحيد للبنود في العقد القائم
/// (نفس نمط <c>frmOpeningStock</c>) لأن <c>CreateInventoryAdjustmentRequest</c> لا يحمل بنوداً.
/// </para>
/// </summary>
public partial class frmAddUpdateInventoryAdjustment : Form
{
    /// <summary>RULE-N46 + RULE-N54b: لون الحقل النشط يُقرأ حيّاً من لوحة الثيم (خاصية لا حقل ثابت).</summary>
    private static Color FocusColor => ThemePalette.DefaultFor(ThemePalette.SelectionRowColor);

    private readonly IServiceProvider _serviceProvider;
    private int? _adjustmentId;
    private DataTable _linesTable = new();
    private DataView _linesView = new();
    private bool _isDraft;

    /// <summary>اسم حالة المستند كما ورد من الخدمة (مسودة · مرحّل · ملغي) — يُعرض في <c>lblMode</c> (RULE-N44).</summary>
    /// <remarks>RULE-N171/N172: نص المسودة من البيت الواحد <see cref="DocumentTextHelper.StatusText(byte)"/> (1 = Draft).</remarks>
    private string _statusName = DocumentTextHelper.StatusText(1);

    /// <summary>الوحدة المختارة من منتقي الأصناف — التكلفة تُملأ من سعرها وتُستهلك عند «إضافة صنف».</summary>
    private ProductUnitDto? _pickedUnit;

    /// <summary>اسم الصنف المختار كما يُعرض في حقل الاختيار ثم في عمود «المنتج/الوحدة».</summary>
    private string _pickedProductName = string.Empty;

    public frmAddUpdateInventoryAdjustment(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط وأسهم RTL.
        // RULE-N191 — Enter داخل بيانات التسوية (groupHeader) يمشي بترتيب العرض صفاً صفاً ثم يُركِّز زر الوضع
        // بلا تنفيذ؛ وحقوله خارج النطاق لا تتغيّر، والشبكة ليست حقل إدخال أصلاً (RULE-N153 — Enter فيها لا يُرحِّل).
        KeyboardHelper.WireEnterAsTab(groupHeader, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N180…N183 — تغذية الحقول المطلوبة حيّاً (أيقونة ✗ + الرسالة العربية + ومضة الرفض) من البيت
        // الواحد RequiredFieldHelper. موضع الاستدعاء إلزامي: **بعد** WireFocusHighlight (RULE-N183) لأن ترتيب
        // اشتراك Leave هو ما يحسم مَن يرسم الخلفية أخيراً. الرأس يُطلب على مستند جديد فقط (لا UpdateAsync في العقد).
        RequiredFieldHelper.Attach(this, errorProvider,
            new RequiredField(cmbWarehouse, "يرجى اختيار المستودع", () => _adjustmentId is null),
            new RequiredField(cmbAdjustmentType, "يرجى اختيار نوع التسوية", () => _adjustmentId is null),
            new RequiredField(txtReason, "سبب التعديل مطلوب", () => _adjustmentId is null));

        // RULE-N43 — مظهر واضح للأزرار المعطّلة (زر «ترحيل» يُعطَّل بعد الترحيل، وأزرار البنود تُعطَّل على مستند غير مسودة).
        KeyboardHelper.WireButtonDisabledVisual(btnSaveDraft, btnPost, btnClose, btnPickProduct, btnAddLine);

        // RULE-N47 — F2 اختيار صنف · F4 ترحيل · F5 تحديث · Esc إغلاق.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnPickProduct.PerformClick()),
            (Keys.F4, () => btnPost.PerformClick()),
            (Keys.F5, () => _Reload()),
            (Keys.Escape, () => btnClose.PerformClick()));

        // RULE-N147 — Enter = الزر الأساسي: «حفظ مسودة» قبل وجود مسودة و«ترحيل» بعدها.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        // RULE-N97 — التفقيط الحيّ على حقول المبالغ والكميات المُدخَلة يدوياً (الكمية المتوقعة/الفعلية/تكلفة الوحدة).
        LiveTafqeetHelper.Attach(liveTip,
            (txtTotalsValue, "قيمة الفرق", LiveTafqeetHelper.ReadMode.Money),
            (txtEntryUnitCost, "تكلفة الوحدة", LiveTafqeetHelper.ReadMode.Money),
            (txtEntryExpected, "الكمية المتوقعة", LiveTafqeetHelper.ReadMode.Quantity),
            (txtEntryActual, "الكمية الفعلية", LiveTafqeetHelper.ReadMode.Quantity));

        _SetupToolTips();

        ApplyUiTheme();   // RULE-N50 — ينتهي بالثيم الأخضر
    }

    /// <summary>عقد الطبقة المكتبية لتسويات المخزون (RULE-N04/N05).</summary>
    private IInventoryAdjustmentDesktopService AdjustmentService
        => _serviceProvider.GetRequiredService<IInventoryAdjustmentDesktopService>();

    /// <summary>عقد الطبقة المكتبية للمستودعات (RULE-N04/N05).</summary>
    private IWarehouseDesktopService WarehouseService
        => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();

    /// <summary>عقد الطبقة المكتبية للأصناف — لقراءة وحدات الصنف المختار (RULE-N04/N05).</summary>
    private IProductDesktopService ProductService
        => _serviceProvider.GetRequiredService<IProductDesktopService>();

    // ── الثيم ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// الثيم الأخضر (RULE-N50…RULE-N53): يبدأ بـ ThemeHelper وينتهي بـ
    /// <c>GreenThemeHelper.Apply(this)</c> **كآخر سطر**، والقيم النهائية مطابقة حرفياً
    /// لما أُعلن في <c>frmAddUpdateInventoryAdjustment.Designer.cs</c> (RULE-N134/N140…N144).
    /// </summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);   // يفرض أبيض/Segoe UI ⇒ يردّه الثيم الأخضر أدناه

        GreenThemeHelper.ApplyHeader(lblMode);              // RULE-N53
        GreenThemeHelper.ApplyGrid(dgvLines);              // RULE-N51/N52

        // RULE-N22: سلوك الشبكة تعلنه الشاشة نفسها — ApplyGrid لا يلمسه (RULE-N52).
        dgvLines.ReadOnly = true;
        dgvLines.AllowUserToAddRows = false;
        dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLines.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>تلميحات الأزرار مع اختصاراتها (RULE-N47).</summary>
    private void _SetupToolTips()
    {
        toolTip1.SetToolTip(btnSaveDraft, "حفظ التسوية كمسودة دون تعديل الأرصدة");
        toolTip1.SetToolTip(btnPost, "ترحيل التسوية وتعديل الأرصدة (F4)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
        toolTip1.SetToolTip(btnPickProduct, "اختيار الصنف المراد تسويته (F2)");
        toolTip1.SetToolTip(btnAddLine, "إضافة الصنف بالأرقام المدخلة إلى بنود التسوية");
        toolTip1.SetToolTip(txtEntryProduct, "الصنف والوحدة المختاران");
        toolTip1.SetToolTip(txtEntryExpected, "الكمية المسجّلة في النظام قبل التسوية");
        toolTip1.SetToolTip(txtEntryActual, "الكمية الفعلية بعد الجرد");
        toolTip1.SetToolTip(txtEntryUnitCost, "تكلفة الوحدة — تُستخدم في قيمة الفرق");
        toolTip1.SetToolTip(cmbWarehouse, "المستودع الذي تُطبَّق عليه التسوية");
        toolTip1.SetToolTip(cmbAdjustmentType, "نوع التسوية: إضافة/خصم/تصحيح");
        toolTip1.SetToolTip(txtReason, "سبب التعديل — مطلوب للترحيل");
        toolTip1.SetToolTip(dgvLines, "بنود التسوية للقراءة فقط");
    }

    // ── Public Loader ──────────────────────────────────────────────────────────────

    /// <summary>تحميل تسوية قائمة للتعديل/الترحيل (يُستدعى قبل <c>ShowDialog</c>).</summary>
    public void LoadAdjustment(int adjustmentId)
    {
        _adjustmentId = adjustmentId;
        Text = "تعديل تسوية مخزون";
        lblMode.Text = "تعديل تسوية مخزون";

        if (IsHandleCreated)
            _ = _LoadAdjustmentDataAsync();
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────────

    private DataGridViewColumn _MakeColumn(string name, string header, int fill, int min, bool readOnly = true)
    {
        return new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = header,
            DataPropertyName = name,
            MinimumWidth = min,
            FillWeight = fill,
            ReadOnly = readOnly,
        };
    }

    /// <summary>
    /// جدول البنود — المصدر الوحيد لبيانات الشبكة وللطلب (RULE-N169): أعمدة <c>LineId</c>/
    /// <c>ProductUnitId</c> مخفيّة عن الشبكة (لا عمود لها في المصمّم و<c>AutoGenerateColumns = false</c>)
    /// ويستهلكها <c>_FlushPendingLinesAsync</c> وحارس التكرار.
    /// </summary>
    private void _CreateLinesTable()
    {
        _linesTable = new DataTable();
        _linesTable.Columns.Add("LineId", typeof(int));
        _linesTable.Columns.Add("ProductUnitId", typeof(int));
        _linesTable.Columns.Add("ProductUnitName", typeof(string));
        _linesTable.Columns.Add("ExpectedQuantity", typeof(decimal));
        _linesTable.Columns.Add("ActualQuantity", typeof(decimal));
        _linesTable.Columns.Add("UnitCost", typeof(decimal));
        _linesTable.Columns.Add("Difference", typeof(decimal));

        _linesView = new DataView(_linesTable);
    }

    private void _SetupLinesGrid()
    {
        _CreateLinesTable();

        dgvLines.Columns.Clear();

        dgvLines.Columns.Add(_MakeColumn("ProductUnitName", "المنتج/الوحدة", 25, 200));
        dgvLines.Columns.Add(_MakeColumn("ExpectedQuantity", "الكمية المتوقعة", 15, 100));
        dgvLines.Columns.Add(_MakeColumn("ActualQuantity", "الكمية الفعلية", 15, 100));
        dgvLines.Columns.Add(_MakeColumn("UnitCost", "تكلفة الوحدة", 12, 100));
        dgvLines.Columns.Add(_MakeColumn("Difference", "الفرق", 12, 100));

        // RULE-N179 — تنسيق الأعمدة من البيت الواحد (RULE-N169): كمية 3 / مبلغ 2 حسب إعدادات التنسيق الحيّة.
        GridFormatHelper.ApplyColumns(dgvLines, GridFormatKind.Quantity,
            "ExpectedQuantity", "ActualQuantity", "Difference");
        GridFormatHelper.ApplyColumn(dgvLines, "UnitCost", GridFormatKind.Money);

        dgvLines.DataSource = _linesView;
    }

    /// <summary>يضيف سطراً واحداً إلى جدول البنود — الفرق يُحسب مرة واحدة هنا (RULE-N172).</summary>
    private void _AddLineRow(int lineId, int productUnitId, string productUnitName,
        decimal expectedQuantity, decimal actualQuantity, decimal unitCost)
    {
        var row = _linesTable.NewRow();

        row["LineId"] = lineId;
        row["ProductUnitId"] = productUnitId;
        row["ProductUnitName"] = productUnitName;
        row["ExpectedQuantity"] = expectedQuantity;
        row["ActualQuantity"] = actualQuantity;
        row["UnitCost"] = unitCost;
        row["Difference"] = Math.Round(actualQuantity - expectedQuantity, 3);

        _linesTable.Rows.Add(row);
    }

    // ── Data Loading ───────────────────────────────────────────────────────────────

    /// <summary>RULE-N25/N26: تحميل غير متزامن مع WaitCursor واستعادته في finally.</summary>
    private async Task<bool> _LoadAdjustmentDataAsync()
    {
        if (_adjustmentId is null) return false;

        var result = await AdjustmentService.GetByIdAsync(_adjustmentId.Value);

        if (!result.IsSuccess || result.Value is null)
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل بيانات التسوية");
            return false;
        }

        var adj = result.Value;

        // Populate header fields
        txtAdjustmentNo.Text = adj.AdjustmentNo;

        // الاختيار بالمفتاح لا بالنص: القائمة مُقيَّدة (DropDownList) فلا يعمل إسناد Text فيها.
        cmbWarehouse.SelectedValue = adj.WarehouseId;
        if (cmbWarehouse.SelectedIndex < 0 && !string.IsNullOrEmpty(adj.WarehouseName))
            cmbWarehouse.Text = adj.WarehouseName;

        cmbAdjustmentType.SelectedValue = adj.AdjustmentType;
        if (cmbAdjustmentType.SelectedIndex < 0 && !string.IsNullOrEmpty(adj.AdjustmentTypeName))
            cmbAdjustmentType.Text = adj.AdjustmentTypeName;

        txtReason.Text = adj.Reason ?? "";
        _isDraft = adj.Status == 1;
        _statusName = string.IsNullOrWhiteSpace(adj.StatusName) ? DocumentTextHelper.StatusText(1) : adj.StatusName!;   // 1 = Draft (RULE-019)

        // Load lines
        _CreateLinesTable();

        if (adj.Lines is { Count: > 0 })
        {
            foreach (var line in adj.Lines)
            {
                _AddLineRow(
                    line.Id,
                    line.ProductUnitId,
                    line.ProductUnitName ?? "",
                    line.ExpectedQuantity,
                    line.ActualQuantity,
                    line.UnitCost);
            }
        }

        dgvLines.DataSource = _linesView;

        _UpdateTotals();
        _ApplyDocumentState();   // RULE-N43/N44 — حالة الأزرار والحقول تتبع حالة المستند (مسودة/غيرها)

        return true;
    }

    private async Task _LoadWarehousesAsync()
    {
        var result = await WarehouseService.GetAllAsync();

        if (result.IsSuccess && result.Value is { Count: > 0 })
        {
            cmbWarehouse.DisplayMember = "Name";
            cmbWarehouse.ValueMember = "Id";
            cmbWarehouse.DataSource = result.Value.ToList();
            return;
        }

        // فشل صامت كان يُترك المستخدم أمام قائمة فارغة بلا سبب — الرسالة العربية من الخدمة تُعرض الآن.
        clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة المستودعات");
    }

    private void _LoadAdjustmentTypes()
    {
        var types = new List<KeyValuePair<byte, string>>
        {
            new(1, "إضافة"),
            new(2, "خصم"),
            new(3, "تصحيح"),
        };
        cmbAdjustmentType.DisplayMember = "Value";
        cmbAdjustmentType.ValueMember = "Key";
        cmbAdjustmentType.DataSource = types;
    }

    /// <summary>RULE-N61 (بلا pnlSummary): شريط الإجماليات — عدد البنود + فرق الكمية + قيمة الفرق.</summary>
    private void _UpdateTotals()
    {
        decimal totalDifference = 0m;
        decimal totalValue = 0m;

        foreach (DataRow row in _linesTable.Rows)
        {
            if (row.RowState == DataRowState.Deleted) continue;

            decimal diff = row["Difference"] is decimal d ? d : 0m;
            decimal cost = row["UnitCost"] is decimal c ? c : 0m;

            totalDifference += diff;
            totalValue += diff * cost;
        }

        lblLinesCount.Text = clsFormat.Number(_linesTable.Rows.Count);
        lblTotalsDiff.Text = clsFormat.Quantity(totalDifference);
        txtTotalsValue.Text = clsFormat.Money(Math.Abs(totalValue));

        lblEmpty.Visible = _linesTable.Rows.Count == 0;

        if (lblEmpty.Visible)
            lblEmpty.BringToFront();
    }

    /// <summary>
    /// RULE-N43/RULE-N44/N150 — آلة حالة واحدة تُستدعى بعد كل انتقال (تحميل · إنشاء · حفظ · ترحيل · تحديث):
    /// الحقول والبنود قابلة للتعديل على مستند جديد أو مسودة فقط، والرأس لا يُعدَّل بعد إنشائه
    /// لأن العقد لا يعرّض <c>UpdateAsync</c> (تعديل الرأس بعد الحفظ لا يُحفظ ⇒ يُقفل بدل أن يُضيع صامتاً).
    /// </summary>
    private void _ApplyDocumentState()
    {
        bool isNewDocument = _adjustmentId is null;
        bool canEditLines = isNewDocument || _isDraft;

        cmbWarehouse.Enabled = isNewDocument;
        cmbAdjustmentType.Enabled = isNewDocument;
        txtReason.Enabled = isNewDocument;

        pnlLineEntry.Enabled = canEditLines;
        btnPickProduct.Enabled = canEditLines;
        btnAddLine.Enabled = canEditLines;

        btnSaveDraft.Enabled = canEditLines;
        btnPost.Enabled = _isDraft && _adjustmentId.HasValue;

        AcceptButton = _PrimaryActionButton();   // RULE-N147 — الزر الافتراضي يتبع حالة المستند
        _UpdateModeHeader();
    }

    /// <summary>RULE-N44 — مؤشر الوضع: جديد / مسودة رقم … / حالة أخرى للقراءة فقط.</summary>
    private void _UpdateModeHeader()
    {
        string documentNo = string.IsNullOrWhiteSpace(txtAdjustmentNo.Text)
            ? string.Empty
            : $" رقم {txtAdjustmentNo.Text.Trim()}";

        lblMode.Text = _adjustmentId is null
            ? "تسوية مخزون جديدة"
            : _isDraft
                ? $"تسوية مخزون — مسودة{documentNo}"
                : $"تسوية مخزون — {_statusName}{documentNo} (للقراءة فقط)";
    }

    /// <summary>هل يوجد سطر لم يُرسل إلى الخدمة بعد؟ (يُحدَّد بـ<c>LineId = 0</c>).</summary>
    private bool _HasPendingLines()
        => _linesTable.Rows.Cast<DataRow>()
            .Any(r => r.RowState != DataRowState.Deleted
                      && (r["LineId"] is not int lineId || lineId <= 0));

    // ── Actions ────────────────────────────────────────────────────────────────────

    /// <summary>
    /// الزر الأساسي لـ <c>Enter</c> (RULE-N147): المستند لا يُرحَّل قبل حفظ مسودته، فالزر الأساسي
    /// هو «حفظ مسودة» قبل الحفظ و«ترحيل» بعد وجود مسودة — ويُعاد <c>null</c> عندما يكون التركيز
    /// داخل شبكة البنود حتى يبقى <c>Enter</c> فيها تنقّلاً بين السطور (RULE-N40) ولا يُرحِّل
    /// المستند أثناء مراجعة السطور (سلامة المخزون — §2.7).
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (dgvLines.ContainsFocus) return null;

        if (_isDraft && _adjustmentId.HasValue) return btnPost;

        // مستند جديد أو مسودة ⇒ «حفظ مسودة»؛ ومستند مرحّل/ملغي للقراءة فقط ⇒ لا زر أساسي.
        return _adjustmentId is null || _isDraft ? btnSaveDraft : null;
    }

    // ── Add Line (المسار الوحيد لبنود التسوية) ──────────────────────────────────────

    /// <summary>
    /// 「اختيار صنف」 (F2) — منتقي الأصناف القائم <c>frmProductSelection</c> (نفس نمط <c>frmOpeningStock</c>).
    /// لا يُضاف السطر هنا: يُملأ الصنف والوحدة والتكلفة الافتراضية، ثم «إضافة صنف» يقرأ الكميات ويضيف السطر.
    /// </summary>
    private async void btnPickProduct_Click(object sender, EventArgs e)
    {
        var dialog = _serviceProvider.GetRequiredService<frmProductSelection>();

        if (dialog.ShowDialog(this) != DialogResult.OK || dialog.SelectedProduct is null)
            return;

        var product = dialog.SelectedProduct;
        Cursor = Cursors.WaitCursor;

        try
        {
            var unit = dialog.SelectedUnit ?? await _GetBaseUnitAsync(product.Id);

            if (unit is null)
            {
                clsDialogs.Warning("تعذر تحديد وحدة هذا الصنف. يرجى اختيار صنف له وحدة معرَّفة.");
                return;
            }

            if (_HasLineFor(unit.Id))
            {
                clsDialogs.Warning("هذا الصنف مضاف مسبقاً في بنود هذه التسوية.");
                return;
            }

            _pickedUnit = unit;
            _pickedProductName = $"{product.Name} — {unit.UnitName}";

            txtEntryProduct.Text = _pickedProductName;
            txtEntryUnitCost.Text = clsFormat.Money(unit.CostPrice);
            txtEntryExpected.Focus();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تجهيز الصنف المختار في تسوية المخزون");
            clsDialogs.Error("تعذر تجهيز الصنف المختار. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// 「إضافة صنف」 — يقرأ الكميتين بـ<c>clsFormat.TryParseQuantity</c> والتكلفة بـ<c>clsFormat.TryParseMoney</c>،
    /// يرفض الكمية غير الرقمية وبلا قيمة موجبة، ثم يضيف السطر إلى جدول البنود ويعيد حساب الإجماليات.
    /// رفض «الفرق حسب نوع التسوية» يبقى في الخدمة وحدها (RULE-N167/RULE-N172) وتُعرض رسالتها العربية.
    /// </summary>
    private void btnAddLine_Click(object sender, EventArgs e)
    {
        errorProvider.Clear();
        RequiredFieldHelper.Clear(this);   // RULE-N183 — إعادة الضبط تمسح علامات الحقول المطلوبة مع أيقوناتها

        var pickedUnit = _pickedUnit;

        if (pickedUnit is null)
        {
            clsDialogs.Warning("يرجى اختيار الصنف أولاً.");
            btnPickProduct.Focus();
            return;
        }

        if (!clsFormat.TryParseQuantity(txtEntryExpected.Text, out decimal expectedQuantity)
            || !clsFormat.TryParseQuantity(txtEntryActual.Text, out decimal actualQuantity))
        {
            const string quantityMessage = "الكمية غير صحيحة — أدخل أرقاماً صالحة في الكمية المتوقعة والفعلية";

            errorProvider.SetError(txtEntryActual, quantityMessage);
            clsInputFeedback.ShowRejectTooltip(txtEntryActual, quantityMessage, toolTip1);
            clsDialogs.Warning(quantityMessage);
            txtEntryActual.Focus();
            return;
        }

        if (expectedQuantity < 0m || actualQuantity < 0m)
        {
            const string negativeMessage = "الكمية لا يمكن أن تكون سالبة";

            errorProvider.SetError(txtEntryActual, negativeMessage);
            clsInputFeedback.ShowRejectTooltip(txtEntryActual, negativeMessage, toolTip1);
            clsDialogs.Warning(negativeMessage);
            return;
        }

        if (expectedQuantity == 0m && actualQuantity == 0m)
        {
            const string zeroMessage = "الكمية يجب أن تكون أكبر من صفر";

            errorProvider.SetError(txtEntryExpected, zeroMessage);
            clsInputFeedback.ShowRejectTooltip(txtEntryExpected, zeroMessage, toolTip1);
            clsDialogs.Warning(zeroMessage);
            txtEntryExpected.Focus();
            return;
        }

        if (expectedQuantity == actualQuantity)
        {
            const string sameMessage = "الكمية الفعلية مساوية للمتوقعة — لا يوجد فرق يُسوّى";

            errorProvider.SetError(txtEntryActual, sameMessage);
            clsInputFeedback.ShowRejectTooltip(txtEntryActual, sameMessage, toolTip1);
            clsDialogs.Warning(sameMessage);
            txtEntryActual.Focus();
            return;
        }

        decimal unitCost = 0m;

        if (!string.IsNullOrWhiteSpace(txtEntryUnitCost.Text)
            && !clsFormat.TryParseMoney(txtEntryUnitCost.Text, out unitCost))
        {
            const string costMessage = "تكلفة الوحدة غير صحيحة — أدخل رقماً صالحاً";

            errorProvider.SetError(txtEntryUnitCost, costMessage);
            clsInputFeedback.ShowRejectTooltip(txtEntryUnitCost, costMessage, toolTip1);
            clsDialogs.Warning(costMessage);
            txtEntryUnitCost.Focus();
            return;
        }

        if (_HasLineFor(pickedUnit.Id))
        {
            clsDialogs.Warning("هذا الصنف مضاف مسبقاً في بنود هذه التسوية.");
            return;
        }

        _AddLineRow(0, pickedUnit.Id, _pickedProductName, expectedQuantity, actualQuantity, unitCost);

        _ResetEntry();
        _UpdateTotals();
        _ApplyDocumentState();
    }

    /// <summary>تفريغ حقل الإدخال بعد إضافة السطر وإعادة التركيز إلى اختيار الصنف.</summary>
    private void _ResetEntry()
    {
        _pickedUnit = null;
        _pickedProductName = string.Empty;

        txtEntryProduct.Text = string.Empty;
        txtEntryExpected.Clear();
        txtEntryActual.Clear();
        txtEntryUnitCost.Clear();

        errorProvider.SetError(txtEntryExpected, "");
        errorProvider.SetError(txtEntryActual, "");
        errorProvider.SetError(txtEntryUnitCost, "");

        btnPickProduct.Focus();
    }

    /// <summary>هل وحدة الصنف هذه موجودة في البنود أصلاً؟ (حارس تكرار قبل الإضافة).</summary>
    private bool _HasLineFor(int productUnitId)
        => _linesTable.Rows.Cast<DataRow>()
            .Any(r => r.RowState != DataRowState.Deleted
                      && r["ProductUnitId"] is int existing && existing == productUnitId);

    /// <summary>الوحدة الأساسية للصنف — تُقرأ من عقد الأصناف القائم (لا حساب محلي — RULE-N169).</summary>
    private async Task<ProductUnitDto?> _GetBaseUnitAsync(int productId)
    {
        var result = await ProductService.GetProductUnitsAsync(productId);

        if (!result.IsSuccess || result.Value is not { Count: > 0 } units)
            return null;

        return units.FirstOrDefault(u => u.IsBaseUnit) ?? units[0];
    }

    // ── Save / Post ───────────────────────────────────────────────────────────────

    /// <summary>RULE-N25/N26: حفظ المسودة — معالج async مع try/catch و WaitCursor.</summary>
    private async Task _SaveDraftAsync()
    {
        if (!_ValidateBeforeSave()) return;

        Cursor = Cursors.WaitCursor;
        btnSaveDraft.Enabled = false;

        try
        {
            await _EnsureSavedAsync(showSuccess: true);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر حفظ مسودة تسوية المخزون");
            clsDialogs.Error("تعذر حفظ مسودة التسوية. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
            {
                _ApplyDocumentState();
                Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>
    /// ينشئ رأس المستند مرة واحدة (لا <c>UpdateAsync</c> في العقد) ثم يُرسل **كل سطر معلّق** عبر
    /// <c>AddLineAsync</c> — وهو المسار الوحيد للبنود — وأخيراً يُعيد تحميل المستند من الخدمة
    /// لتُثبَّت معرّفات البنود فلا يُرسل السطر مرتين عند حفظ متكرر (RULE-N167).
    /// </summary>
    private async Task<bool> _EnsureSavedAsync(bool showSuccess)
    {
        bool headerCreatedNow = false;

        if (_adjustmentId is null)
        {
            // لا قيمة افتراضية صامتة: مستودع/نوع غير مقروءين من القائمة ⇒ رفض عربي بدل مستند بمخزن صفر أو نوع «إضافة» مُخترع.
            if (cmbWarehouse.SelectedValue is not short warehouseId || warehouseId <= 0
                || cmbAdjustmentType.SelectedValue is not byte adjustmentType)
            {
                clsDialogs.Error("بيانات رأس التسوية غير مكتملة — يرجى اختيار المستودع ونوع التسوية.");
                _ApplyDocumentState();
                return false;
            }

            var request = new CreateInventoryAdjustmentRequest(
                WarehouseId: warehouseId,
                AdjustmentType: adjustmentType,
                Reason: txtReason.Text.Trim());

            var created = await AdjustmentService.CreateAsync(request);

            if (!created.IsSuccess || created.Value is null)
            {
                clsDialogs.Error(created.Error ?? "تعذر حفظ مسودة التسوية.");
                return false;
            }

            _adjustmentId = created.Value.Id;
            _isDraft = true;
            _statusName = DocumentTextHelper.StatusText(1);   // 1 = Draft (RULE-019)
            headerCreatedNow = true;
            txtAdjustmentNo.Text = created.Value.AdjustmentNo;
        }

        if (!await _FlushPendingLinesAsync())
        {
            // تعويض: رأس أُنشئ في هذه المحاولة ولم تُحفظ بنوده ⇒ يُلغى حتى لا تبقى مسودة فارغة في القائمة.
            // (سبب الرفض يظهر للمستخدم من رسالة الخدمة العربية أعلاه.)
            // ولا يُفرَّغ المعرّف إلا إذا نجح الإلغاء فعلاً — وإلا لحُذف من الشاشة وبقي في القائمة
            // فأنشأ الحفظ التالي رأساً ثانياً مكرراً.
            if (headerCreatedNow && _adjustmentId is int orphanId && await _CancelDraftQuietlyAsync(orphanId))
            {
                _adjustmentId = null;
                _isDraft = false;
                txtAdjustmentNo.Text = string.Empty;
            }

            _ApplyDocumentState();
            return false;
        }

        bool reloaded = await _LoadAdjustmentDataAsync();   // إعادة تحميل موثوقة: معرّفات البنود تأتي من الخدمة

        // لا «نجاح» بعد رسالة فشل التحميل: الرسالة تتبع نتيجة إعادة التحميل فعلاً.
        if (showSuccess && reloaded)
            clsDialogs.Success("تم حفظ مسودة التسوية بنجاح");

        return true;
    }

    /// <summary>يُرسل السطور المعلّقة (<c>LineId = 0</c>) إلى الخدمة، ويُظهر رسالة الرفض العربية عند أول فشل.</summary>
    private async Task<bool> _FlushPendingLinesAsync()
    {
        if (_adjustmentId is not int adjustmentId) return false;

        foreach (DataRow row in _linesTable.Rows)
        {
            if (row.RowState == DataRowState.Deleted) continue;
            if (row["LineId"] is int lineId && lineId > 0) continue;

            var request = new AddInventoryAdjustmentLineRequest(
                InventoryAdjustmentId: adjustmentId,
                ProductUnitId: row["ProductUnitId"] is int productUnitId ? productUnitId : 0,
                ExpectedQuantity: row["ExpectedQuantity"] is decimal expected ? expected : 0m,
                ActualQuantity: row["ActualQuantity"] is decimal actual ? actual : 0m,
                UnitCost: row["UnitCost"] is decimal unitCost ? unitCost : 0m);

            var result = await AdjustmentService.AddLineAsync(adjustmentId, request);

            if (!result.IsSuccess)
            {
                clsDialogs.Error(result.Error ?? "تعذر إضافة صنف إلى التسوية.");
                return false;
            }
        }

        return true;
    }

    /// <summary>إلغاء مسودة أُنشئت في محاولة فاشلة — بلا رسالة ولا إسقاط للشاشة، ويُعيد نتيجة الإلغاء.</summary>
    private async Task<bool> _CancelDraftQuietlyAsync(int adjustmentId)
    {
        try
        {
            var result = await AdjustmentService.CancelAsync(adjustmentId);

            if (!result.IsSuccess)
                Log.Warning("تعذر إلغاء مسودة تسوية فارغة رقم {AdjustmentId}: {Error}", adjustmentId, result.Error);

            return result.IsSuccess;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر إلغاء مسودة تسوية فارغة رقم {AdjustmentId}", adjustmentId);
            return false;
        }
    }

    /// <summary>RULE-N25/N26: ترحيل التسوية — الخدمة تُنفّذه داخل معاملة (RULE-N03).</summary>
    private async Task _PostAdjustmentAsync()
    {
        if (!_ValidateBeforeSave()) return;

        Cursor = Cursors.WaitCursor;
        btnPost.Enabled = false;

        try
        {
            // الترحيل يشمل السطور المعلّقة: تُحفظ أولاً ثم يُرحَّل المستند كاملاً.
            if (!await _EnsureSavedAsync(showSuccess: false)) return;

            if (_adjustmentId is not int adjustmentId) return;

            var result = await AdjustmentService.PostAsync(adjustmentId);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم ترحيل تسوية المخزون بنجاح");

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر ترحيل التسوية.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر ترحيل تسوية المخزون");
            clsDialogs.Error("تعذر ترحيل التسوية. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
            {
                _ApplyDocumentState();   // RULE-N43/RULE-N147 — إعادة مزامنة الأزرار والزر الافتراضي
                Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>
    /// بوابة الحفظ/الترحيل: (1) الحقول المطلوبة — الحكم والعَلم في البيت الواحد
    /// <see cref="RequiredFieldHelper"/> فلا نسخة ثانية لحكم «فارغ» هنا (RULE-N172/RULE-N184)،
    /// ثم (2) قواعد المستند الرقمية في <see cref="_ValidateRangeRules"/> كما هي.
    /// </summary>
    private bool _ValidateBeforeSave()
    {
        if (!RequiredFieldHelper.Validate(this))
        {
            // يُوسَم كل ناقص من الـHelper، ثم ينتقل المؤشر إلى أوله (لا حكم ثانٍ على «فارغ»).
            if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();

            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
            return false;
        }

        return _ValidateRangeRules();
    }

    /// <summary>
    /// قواعد المستند غير المتعلقة بالحقول المطلوبة — **منقولة حرفياً** من التحقق القديم
    /// (نفس الرسالة العربية ونفس <c>errorProvider</c>): لا حفظ ولا ترحيل بلا بند واحد على الأقل.
    /// <para>
    /// فحوص الكميات (سالب · صفر · مساوٍ للمتوقعة · تكلفة غير رقمية) تبقى **مكانها الأصلي**
    /// <c>btnAddLine_Click</c> بلا مساس — تُقيَّم عند إضافة السطر لا عند الحفظ (RULE-N167: رفض الفرق
    /// حسب نوع التسوية يبقى في الخدمة وحدها).
    /// </para>
    /// </summary>
    private bool _ValidateRangeRules()
    {
        if (_linesTable.Rows.Count == 0)
        {
            const string noLinesMessage = "يجب إضافة صنف واحد على الأقل";

            errorProvider.SetError(dgvLines, noLinesMessage);
            clsInputFeedback.ShowRejectTooltip(dgvLines, noLinesMessage, toolTip1);
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
            return false;
        }

        errorProvider.SetError(dgvLines, "");
        return true;
    }

    /// <summary>تحديث بيانات المستند من الخدمة (F5).</summary>
    private async void _Reload()
    {
        if (!IsHandleCreated) return;

        // مستند جديد لم يُحفظ بعد: لا شيء يُحدَّث من الخدمة — البنود المدخلة تبقى كما هي.
        if (!_adjustmentId.HasValue) return;

        if (_HasPendingLines()
            && !clsDialogs.Confirm("لديك بنود غير محفوظة — تحديث البيانات سيُهملها. هل تريد المتابعة؟"))
            return;

        Cursor = Cursors.WaitCursor;

        try
        {
            await _LoadAdjustmentDataAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحديث بيانات تسوية المخزون");
            clsDialogs.Error("تعذر تحديث بيانات التسوية. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    // ── Form Events ────────────────────────────────────────────────────────────────

    /// <summary>RULE-N25/N26: تحميل الشاشة غير متزامن مع try/catch و WaitCursor.</summary>
    private async void frmInventoryAdjustmentEditor_Load(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;

        try
        {
            await _LoadWarehousesAsync();
            _LoadAdjustmentTypes();
            _SetupLinesGrid();

            if (_adjustmentId.HasValue)
            {
                await _LoadAdjustmentDataAsync();
            }
            else
            {
                _UpdateTotals();
                _ApplyDocumentState();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحميل شاشة تسوية المخزون");
            clsDialogs.Error("تعذر تحميل بيانات الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
            {
                // تفعيل/تعطيل حقول الإدخال النهائي بعد ظهور الشاشة (RULE-N152).
                _ApplyDocumentState();
                Cursor = Cursors.Default;
            }
        }
    }

    private async void btnSaveDraft_Click(object sender, EventArgs e)
    {
        await _SaveDraftAsync();
    }

    private async void btnPost_Click(object sender, EventArgs e)
    {
        if (!clsDialogs.Confirm("هل أنت متأكد من ترحيل التسوية؟"))
            return;

        await _PostAdjustmentAsync();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        // بنود أُدخلت ولم تُرسل إلى الخدمة بعد تُفقد بالإغلاق — تأكيد صريح بدل الضياع الصامت.
        if (_HasPendingLines()
            && !clsDialogs.Confirm("لديك بنود غير محفوظة — الإغلاق سيُهملها. هل تريد المتابعة؟"))
        {
            return;
        }

        Close();
    }

    // ── Context Menu ──────────────────────────────────────────────────────────────

    private void miCopyCell_Click(object sender, EventArgs e)
    {
        var cell = dgvLines.CurrentCell;

        if (cell?.Value is null)
        {
            clsDialogs.Warning("يرجى تحديد خلية تحتوي قيمة أولاً.");
            return;
        }

        try
        {
            Clipboard.SetText(cell.Value?.ToString() ?? string.Empty);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر نسخ قيمة خلية من بنود التسوية");
            clsDialogs.Error("تعذر نسخ القيمة. يرجى إعادة المحاولة.");
        }
    }

    private void miRefresh_Click(object sender, EventArgs e)
        => _Reload();

    private void lblLinesCount_Click(object sender, EventArgs e)
    {

    }
}
