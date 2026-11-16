using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Products.Forms;
using System.Data;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// «بضاعة أول المدة» — مستند إدخال أرصدة افتتاحية.
/// الهيكل (RULE-N79 / RULE-N135…N138): العنوان ← أوامر المستند ← بيانات المستند ← الأصناف ← الإجماليات.
/// منطق الترحيل (تسوية افتتاحية + اعتمادها) لم يُمسّ إطلاقاً.
/// </summary>
public partial class frmOpeningStock : Form
{
    private readonly IServiceScope _scope;
    private IServiceProvider ServiceProvider => _scope.ServiceProvider;
    private DataTable _dtLines = new();
    private Dictionary<int, List<ProductUnitDto>> _productUnitsCache = [];

    /// <summary>نص شريط العنوان الأساسي — يُحدَّث بعدّاد الأصناف لإظهار وضع المستند (RULE-N44).</summary>
    private const string FormTitle = "بضاعة أول المدة";

    public frmOpeningStock(IServiceScopeFactory scopeFactory)
    {
        InitializeComponent();
        _scope = scopeFactory.CreateScope();
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        KeyPreview = true;
        KeyDown += frmOpeningStock_KeyDown;

        // RULE-N40…RULE-N46 — تنقل Enter/الأسهم + تحديد القيمة + تظليل الحقل النشط.
        // RULE-N191 — Enter داخل «بيانات المستند» (gbInfo) يمشي بترتيب العرض صفاً صفاً ثم يُركِّز زر الوضع
        // بلا تنفيذ؛ وحقوله خارج النطاق تبقى بسلوك SelectNextControl، والشبكة ليست حقل إدخال أصلاً
        // (RULE-N153 — Enter فيها تنقّل خلايا ولا يُرحِّل المستند).
        KeyboardHelper.WireEnterAsTab(gbInfo, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnAddProduct, btnRemoveProduct, btnSave);

        // RULE-N47 — الاختصارات الموجودة فعلاً على هذه الشاشة فقط.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAddProduct.PerformClick()),
            (Keys.F4, _SaveIfReady));

        // RULE-N147 — Enter = الزر الأساسي للوضع الحالي («حفظ وترحيل»)، ولا يعمل داخل شبكة السطور.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        // الزر الافتراضي لا يُطلق الترحيل من داخل الشبكة (Enter هناك تنقّل بين الخلايا — RULE-N40).
        dgvLines.Enter += (s, e) => AcceptButton = null;
        dgvLines.Leave += (s, e) => AcceptButton = _PrimaryActionButton();

        // RULE-N97 — التفقيط الحيّ لقيمة الإجمالي المالي.
        LiveTafqeetHelper.Attach(liveTip,
            (txtTotalCost, "إجمالي التكلفة", LiveTafqeetHelper.ReadMode.Money));

        // خطأ تحرير خلية (مثل وحدة غير صالحة) لا يُسقط الشاشة.
        dgvLines.DataError += (s, e) => e.ThrowException = false;
        dgvLines.SelectionChanged += (s, e) => _UpdateButtonsState();

        // RULE-N180…N183 — تغذية الحقول المطلوبة حيّاً (أيقونة ✗ + الرسالة العربية + ومضة الرفض) من البيت
        // الواحد RequiredFieldHelper. موضع الاستدعاء إلزامي: **بعد** WireFocusHighlight (RULE-N183) لأن ترتيب
        // اشتراك Leave هو ما يحسم مَن يرسم الخلفية أخيراً، و**قبل** GreenThemeHelper.Apply في ApplyUiTheme أدناه.
        // المخزن هو الحقل المطلوب الوحيد في هذا المستند؛ أما «سطر واحد على الأقل» و«الكمية > 0» و«الوحدة > 0»
        // فقواعد مستند رقمية تبقى في _ValidateRangeRules (لا نسخة ثانية من حكم «فارغ» — RULE-N172).
        RequiredFieldHelper.Attach(this, errorProvider,
            new RequiredField(cmbWarehouse, "يرجى اختيار المخزن"));

        ApplyUiTheme();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
    }

    // ── الثيم (RULE-N50…RULE-N54) ─────────────────────────────────────

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply(this)</c> آخر سطر دائماً.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvLines);

        // RULE-N179 — تنسيق الأعمدة وقت التشغيل من البيت الواحد (يتبع إعدادات التنسيق الحيّة).
        // المصمّم يحمل القيمة التصميمية فقط (RULE-N134)، وهذه السطور تجعلها حيّة.
        GridFormatHelper.ApplyColumn(dgvLines, "colQuantity", GridFormatKind.Quantity);
        GridFormatHelper.ApplyColumns(dgvLines, GridFormatKind.Money, "colUnitCost", "colTotalCost");

        lblItemCountCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblTotalCostCaption.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── Shortcuts ─────────────────────────────────────────────────────

    /// <summary>
    /// RULE-N147 — الزر الأساسي للوضع الحالي: «حفظ وترحيل» (زر حقيقي من المصمّم، وحالته — مفعّل/معطّل —
    /// يملكها <see cref="_UpdateButtonsState"/> فيتبعها <c>AcceptButton</c>).
    /// <para>
    /// <b>RULE-N153:</b> يرجع <c>null</c> عندما يكون التركيز داخل شبكة السطور (<c>dgvLines</c>) أو خليتها
    /// في وضع التحرير، فلا يُرحَّل المستند من داخل الشبكة أبداً — الترحيل المالي يبقى على زرّه و<kbd>F4</kbd>
    /// بعد تأكيد صريح، والترحيل من شبكة السطور ممنوع.
    /// </para>
    /// </summary>
    private Button? _PrimaryActionButton()
        => dgvLines.ContainsFocus || dgvLines.IsCurrentCellInEditMode ? null : btnSave;

    /// <summary>F4 — ترحيل المستند إن كان جاهزاً.</summary>
    private void _SaveIfReady()
    {
        if (btnSave.Enabled) btnSave.PerformClick();
    }

    // ── التحميل ───────────────────────────────────────────────────────

    private async void frmOpeningStock_Load(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await LoadWarehousesAsync();
            dtpDate.Value = DateTime.Today;
            SetupLinesGrid();
            _UpdateButtonsState();
            _UpdateModeHeader();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة بضاعة أول المدة");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task LoadWarehousesAsync()
    {
        try
        {
            var result = await ServiceProvider.GetRequiredService<IWarehouseDesktopService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "Id";
                cmbWarehouse.DataSource = new List<WarehouseDto>(result.Value);
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة المخازن.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل المخازن");
            clsDialogs.Error("تعذر تحميل قائمة المخازن. يرجى إعادة المحاولة.");
        }
    }

    private void SetupLinesGrid()
    {
        _dtLines = new DataTable();
        _dtLines.Columns.Add("ProductId", typeof(int));
        _dtLines.Columns.Add("ProductUnitId", typeof(int));
        _dtLines.Columns.Add("ProductName", typeof(string));
        _dtLines.Columns.Add("UnitName", typeof(string));
        _dtLines.Columns.Add("Quantity", typeof(decimal));
        _dtLines.Columns.Add("UnitCost", typeof(decimal));
        _dtLines.Columns.Add("TotalCost", typeof(decimal));

        dgvLines.DataSource = new DataView(_dtLines);
        _UpdateTotals();
    }

    // ── مؤشر الوضع + آلة حالة الأزرار (RULE-N43/RULE-N44) ─────────────

    /// <summary>مؤشر الوضع: مستند جديد + عدد الأصناف المدخلة.</summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = $"{FormTitle} — مستند جديد (عدد الأصناف: {_dtLines.Rows.Count})";
    }

    /// <summary>«حذف السطر» يحتاج سطراً محدّداً، و«حفظ وترحيل» يحتاج صنفاً واحداً على الأقل.</summary>
    private void _UpdateButtonsState()
    {
        bool hasLines = _dtLines.Rows.Count > 0;
        bool hasSelectedLine = dgvLines.CurrentRow is { IsNewRow: false };

        btnRemoveProduct.Enabled = hasLines && hasSelectedLine;
        btnSave.Enabled = hasLines;
        miRemoveLine.Enabled = btnRemoveProduct.Enabled;

        // RULE-N147 — الزر الافتراضي يتبع الزر الأساسي الحالي (وnull داخل شبكة السطور).
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>شريط الإجماليات: عدد الأصناف + إجمالي التكلفة (N2).</summary>
    private void _UpdateTotals()
    {
        decimal total = 0m;

        foreach (DataRow row in _dtLines.Rows)
            total += _DecimalOf(row, "TotalCost");

        lblItemCountValue.Text = clsFormat.Number(_dtLines.Rows.Count);
        txtTotalCost.Text = clsFormat.Money(total);

        _UpdateButtonsState();
        _UpdateModeHeader();
    }

    /// <summary>
    /// قراءة عشرية من سطر جدول المستند: الخلية الفارغة (<c>DBNull</c>) تُقرأ صفراً بلا رمي —
    /// كان <c>Convert.ToDecimal</c> عليها يُسقط الشاشة إذا فرّغ المستخدم خلية كمية أو تكلفة.
    /// </summary>
    private static decimal _DecimalOf(DataRow row, string column)
        => row[column] is decimal value ? value : 0m;

    // ── إضافة/حذف الأصناف ─────────────────────────────────────────────

    private async void btnAddProduct_Click(object? sender, EventArgs e)
    {
        try
        {
            var dialog = ServiceProvider.GetRequiredService<frmProductSelection>();
            if (dialog.ShowDialog(this) != DialogResult.OK || dialog.SelectedProduct == null)
                return;

            var existing = _dtLines.AsEnumerable()
                .FirstOrDefault(r => r.Field<int>("ProductId") == dialog.SelectedProduct.Id);
            if (existing != null)
            {
                clsDialogs.Warning("هذا المنتج مضاف مسبقاً للقائمة.");
                return;
            }

            var units = await LoadProductUnitsAsync(dialog.SelectedProduct.Id);
            if (units.Count == 0)
            {
                // بلا وحدات لا يمكن تحديد وحدتها الأساسية ⇒ سطر لا يُحفظ. كان يُضاف سطر
                // بوحدة صفرية فيرفضه التحقق لاحقاً بلا سبب ظاهر للمستخدم.
                clsDialogs.Warning("تعذر تحميل وحدات هذا المنتج — لا يمكن إضافته. راجع وحدات المنتج في دليل الأصناف.");
                return;
            }

            var baseUnit = units.FirstOrDefault(pu => pu.IsBaseUnit) ?? units.FirstOrDefault();

            _dtLines.Rows.Add(
                dialog.SelectedProduct.Id,
                baseUnit?.Id ?? 0,
                dialog.SelectedProduct.Name,
                baseUnit?.UnitName ?? "",
                1m,
                0m,
                0m);

            _UpdateTotals();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إضافة منتج");
            clsDialogs.Error("حدث خطأ أثناء إضافة المنتج.");
        }
    }

    private async Task<List<ProductUnitDto>> LoadProductUnitsAsync(int productId)
    {
        if (_productUnitsCache.TryGetValue(productId, out var cached))
            return cached;

        try
        {
            var result = await ServiceProvider.GetRequiredService<IProductDesktopService>()
                .GetProductUnitsAsync(productId);

            if (result.IsSuccess && result.Value != null)
            {
                var list = result.Value.ToList();
                _productUnitsCache[productId] = list;
                return list;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل وحدات المنتج {ProductId}", productId);
        }

        return [];
    }

    // ── DGV Event Handlers ─────────────────────────────────────────────

    private void dgvLines_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
    {
        if (dgvLines.CurrentCell.ColumnIndex != dgvLines.Columns["colUnit"].Index)
            return;

        if (e.Control is DataGridViewComboBoxEditingControl combo)
        {
            combo.DropDownStyle = ComboBoxStyle.DropDownList;

            // Unsubscribe first to prevent multiple subscriptions (WinForms gotcha)
            combo.SelectedIndexChanged -= Combo_SelectedIndexChanged;
            combo.SelectedIndexChanged += Combo_SelectedIndexChanged;

            combo.DataSource = null;

            if (dgvLines.CurrentRow?.DataBoundItem is DataRowView row)
            {
                int productId = Convert.ToInt32(row["ProductId"]);
                if (_productUnitsCache.TryGetValue(productId, out var units))
                {
                    combo.DataSource = new List<ProductUnitDto>(units);
                    combo.DisplayMember = "UnitName";
                    combo.ValueMember = "Id";
                }
            }
        }
    }

    private void Combo_SelectedIndexChanged(object? sender, EventArgs e)
    {
        dgvLines.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void dgvLines_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (dgvLines.CurrentCell.ColumnIndex == dgvLines.Columns["colUnit"].Index)
        {
            dgvLines.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }

    private void dgvLines_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        int colIndex = e.ColumnIndex;

        if (colIndex == dgvLines.Columns["colQuantity"].Index ||
            colIndex == dgvLines.Columns["colUnitCost"].Index)
        {
            UpdateRowTotal(e.RowIndex);
        }
        else if (colIndex == dgvLines.Columns["colUnit"].Index)
        {
            UpdateUnitDisplay(e.RowIndex);
            UpdateRowTotal(e.RowIndex);
        }
    }

    private void UpdateRowTotal(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= _dtLines.Rows.Count)
            return;

        var row = _dtLines.Rows[rowIndex];
        decimal qty = _DecimalOf(row, "Quantity");
        decimal cost = _DecimalOf(row, "UnitCost");
        row["TotalCost"] = qty * cost;

        _UpdateTotals();
    }

    private void UpdateUnitDisplay(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= _dtLines.Rows.Count)
            return;

        var row = _dtLines.Rows[rowIndex];
        int unitId = Convert.ToInt32(row["ProductUnitId"]);
        int productId = Convert.ToInt32(row["ProductId"]);
        if (_productUnitsCache.TryGetValue(productId, out var units))
        {
            var unit = units.FirstOrDefault(u => u.Id == unitId);
            row["UnitName"] = unit?.UnitName ?? "";
        }
    }

    // ── Actions ────────────────────────────────────────────────────────

    private void btnRemoveProduct_Click(object? sender, EventArgs e)
    {
        if (dgvLines.CurrentRow?.DataBoundItem is not DataRowView row) return;
        _dtLines.Rows.Remove(row.Row);
        _UpdateTotals();
    }

    private void miAddProduct_Click(object? sender, EventArgs e)
        => btnAddProduct.PerformClick();

    private void miRemoveLine_Click(object? sender, EventArgs e)
    {
        if (btnRemoveProduct.Enabled) btnRemoveProduct.PerformClick();
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
            // يُوسَم كل ناقص من الـHelper (أيقونة ✗ + الرسالة العربية + ومضة الرفض)، ثم ينتقل المؤشر إلى أوله.
            if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();

            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
            return false;
        }

        return _ValidateRangeRules();
    }

    /// <summary>
    /// قواعد المستند الرقمية — **منقولة حرفياً** من التحقق القديم (نفس الرسائل العربية ونفس ترتيب الفحص):
    /// سطر واحد على الأقل · وحدة صالحة لكل سطر · كمية أكبر من صفر.
    /// ولا حكم على «فارغ» هنا: المخزن حقل مطلوب يملكه <see cref="RequiredFieldHelper"/> وحده (RULE-N172).
    /// </summary>
    private bool _ValidateRangeRules()
    {
        bool isValid = true;

        if (_dtLines.Rows.Count == 0)
        {
            clsDialogs.Warning("يرجى إضافة منتج واحد على الأقل.");
            isValid = false;
        }

        foreach (DataRow row in _dtLines.Rows)
        {
            int puId = Convert.ToInt32(row["ProductUnitId"]);
            if (puId <= 0)
            {
                clsDialogs.Warning("يرجى اختيار الوحدة المناسبة لكل المنتجات.");
                isValid = false;
                break;
            }

            decimal qty = _DecimalOf(row, "Quantity");
            if (qty <= 0)
            {
                clsDialogs.Warning("الكمية يجب أن تكون أكبر من صفر.");
                isValid = false;
                break;
            }
        }

        return isValid;
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        errorProvider.Clear();
        RequiredFieldHelper.Clear(this);   // RULE-N183 — تُمسح علامات الحقول المطلوبة مع أيقونات الخطأ
        if (!_ValidateBeforeSave()) return;

        btnSave.Enabled = false;
        try
        {
            Cursor = Cursors.WaitCursor;

            if (cmbWarehouse.SelectedValue is not short warehouseId)
            {
                clsDialogs.Error("يرجى اختيار المخزن");
                return;
            }

            var adjustmentApi = ServiceProvider.GetRequiredService<IInventoryAdjustmentDesktopService>();

            var createRequest = new CreateInventoryAdjustmentRequest(
                WarehouseId: warehouseId,
                AdjustmentType: 1); // Opening

            var createResult = await adjustmentApi.CreateAsync(createRequest);
            if (!createResult.IsSuccess || createResult.Value == null)
            {
                clsDialogs.Error(createResult.Error ?? "حدث خطأ أثناء إنشاء التسوية.");
                return;
            }

            int adjustmentId = createResult.Value.Id;
            int successCount = 0;
            int failedCount = 0;

            foreach (DataRow row in _dtLines.Rows)
            {
                int productUnitId = Convert.ToInt32(row["ProductUnitId"]);
                decimal quantity = _DecimalOf(row, "Quantity");
                decimal unitCost = _DecimalOf(row, "UnitCost");

                if (productUnitId <= 0 || quantity <= 0) continue;

                var lineRequest = new AddInventoryAdjustmentLineRequest(
                    InventoryAdjustmentId: adjustmentId,
                    ProductUnitId: productUnitId,
                    ExpectedQuantity: 0m,
                    ActualQuantity: quantity,
                    UnitCost: unitCost);

                var lineResult = await adjustmentApi.AddLineAsync(adjustmentId, lineRequest);
                if (lineResult.IsSuccess)
                    successCount++;
                else
                    failedCount++;   // لا تسقيط صامت: يُحسب ويُبلَّغ قبل الترحيل
            }

            if (successCount == 0)
            {
                clsDialogs.Error("فشل إضافة جميع المنتجات. تحقق من البيانات.");
                return;
            }

            if (failedCount > 0)
            {
                // لا ترحيل بمستند ناقص: تبقى التسوية مسودة ويُبلَّغ المستخدم بالعدد الحقيقي
                // (كان يُرحَّل الناقص وتقول الرسالة إن كل شيء نجح).
                clsDialogs.Error(
                    $"تعذر إضافة {clsFormat.Number(failedCount)} من {clsFormat.Number(_dtLines.Rows.Count)} صنفاً، ولم يُرحَّل المستند. راجع بيانات الأصناف المتعذرة وأعد المحاولة.");
                return;
            }

            var postResult = await adjustmentApi.PostAsync(adjustmentId);
            if (!postResult.IsSuccess)
            {
                clsDialogs.Error(postResult.Error ?? "حدث خطأ أثناء ترحيل التسوية.");
                return;
            }

            clsDialogs.Success($"تم حفظ وترحيل {clsFormat.Number(successCount)} منتج بنجاح كبضاعة أول المدة.");

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء حفظ الكميات الافتتاحية");
            clsDialogs.Error("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل.");
        }
        finally
        {
            Cursor = Cursors.Default;
            btnSave.Enabled = true;
            _UpdateButtonsState();
        }
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void frmOpeningStock_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.S)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            btnSave.PerformClick();
        }
    }
}
