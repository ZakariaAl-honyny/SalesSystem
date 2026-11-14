using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Requests;
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
/// </summary>
public partial class frmAddUpdateInventoryAdjustment : Form
{
    /// <summary>RULE-N46: لون الحقل النشط الموحّد.</summary>
    private static readonly Color FocusColor = Color.FromArgb(0xD6, 0xE4, 0xF0);

    private readonly IServiceProvider _serviceProvider;
    private int? _adjustmentId;
    private DataTable _linesTable = new();
    private DataView _linesView = new();
    private bool _isDraft;

    public frmAddUpdateInventoryAdjustment(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط وأسهم RTL.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للأزرار المعطّلة (زر «ترحيل» يُعطَّل بعد الترحيل).
        KeyboardHelper.WireButtonDisabledVisual(btnSaveDraft, btnPost, btnClose);

        // RULE-N47 — F4 ترحيل · F5 تحديث · Esc إغلاق.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F4, () => btnPost.PerformClick()),
            (Keys.F5, () => _Reload()),
            (Keys.Escape, () => btnClose.PerformClick()));

        // RULE-N147 — Enter = الزر الأساسي: «حفظ مسودة» قبل وجود مسودة و«ترحيل» بعدها.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        // RULE-N97 — التفقيط الحيّ على حقل «قيمة الفرق» (مبلغ).
        LiveTafqeetHelper.Attach(liveTip,
            (txtTotalsValue, "قيمة الفرق", LiveTafqeetHelper.ReadMode.Money));

        _SetupToolTips();

        ApplyUiTheme();   // RULE-N50 — ينتهي بالثيم الأخضر
    }

    /// <summary>عقد الطبقة المكتبية لتسويات المخزون (RULE-N04/N05).</summary>
    private IInventoryAdjustmentDesktopService AdjustmentService
        => _serviceProvider.GetRequiredService<IInventoryAdjustmentDesktopService>();

    /// <summary>عقد الطبقة المكتبية للمستودعات (RULE-N04/N05).</summary>
    private IWarehouseDesktopService WarehouseService
        => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();

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

    private DataGridViewColumn _MakeColumn(string name, string header, int fill, int min, bool readOnly = true, string? format = null)
    {
        var col = new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = header,
            DataPropertyName = name,
            MinimumWidth = min,
            FillWeight = fill,
            ReadOnly = readOnly,
        };
        if (format is not null)
            col.DefaultCellStyle.Format = format;
        return col;
    }

    private void _SetupLinesGrid()
    {
        dgvLines.Columns.Clear();

        dgvLines.Columns.Add(_MakeColumn("ProductUnitName", "المنتج/الوحدة", 25, 200));
        dgvLines.Columns.Add(_MakeColumn("ExpectedQuantity", "الكمية المتوقعة", 15, 100, format: "N3"));
        dgvLines.Columns.Add(_MakeColumn("ActualQuantity", "الكمية الفعلية", 15, 100, format: "N3"));
        dgvLines.Columns.Add(_MakeColumn("UnitCost", "تكلفة الوحدة", 12, 100, format: "N2"));
        dgvLines.Columns.Add(_MakeColumn("Difference", "الفرق", 12, 100, format: "N3"));
    }

    // ── Data Loading ───────────────────────────────────────────────────────────────

    /// <summary>RULE-N25/N26: تحميل غير متزامن مع WaitCursor واستعادته في finally.</summary>
    private async Task _LoadAdjustmentDataAsync()
    {
        if (_adjustmentId is null) return;

        var result = await AdjustmentService.GetByIdAsync(_adjustmentId.Value);

        if (!result.IsSuccess || result.Value is null)
        {
            MessageBox.Show(result.Error ?? "تعذر تحميل بيانات التسوية", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
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

        btnPost.Enabled = _isDraft;
        AcceptButton = _PrimaryActionButton();   // RULE-N147 — يتبع حالة المستند (مسودة ⇒ ترحيل)

        // Load lines
        _linesTable = new DataTable();
        _linesTable.Columns.Add("ProductUnitName", typeof(string));
        _linesTable.Columns.Add("ExpectedQuantity", typeof(decimal));
        _linesTable.Columns.Add("ActualQuantity", typeof(decimal));
        _linesTable.Columns.Add("UnitCost", typeof(decimal));
        _linesTable.Columns.Add("Difference", typeof(decimal));

        if (adj.Lines is { Count: > 0 })
        {
            foreach (var line in adj.Lines)
            {
                decimal diff = line.ActualQuantity - line.ExpectedQuantity;
                _linesTable.Rows.Add(
                    line.ProductUnitName ?? "",
                    line.ExpectedQuantity,
                    line.ActualQuantity,
                    line.UnitCost,
                    Math.Round(diff, 3));
            }
        }

        _linesView = new DataView(_linesTable);
        dgvLines.DataSource = _linesView;

        _UpdateTotals();
    }

    private async Task _LoadWarehousesAsync()
    {
        var result = await WarehouseService.GetAllAsync();

        if (result.IsSuccess && result.Value is { Count: > 0 })
        {
            cmbWarehouse.DisplayMember = "Name";
            cmbWarehouse.ValueMember = "Id";
            cmbWarehouse.DataSource = result.Value.ToList();
        }
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

        foreach (DataRowView row in _linesView)
        {
            decimal diff = row["Difference"] is decimal d ? d : 0m;
            decimal cost = row["UnitCost"] is decimal c ? c : 0m;

            totalDifference += diff;
            totalValue += diff * cost;
        }

        lblLinesCount.Text = _linesView.Count.ToString();
        lblTotalsDiff.Text = clsFormat.Quantity(totalDifference);
        txtTotalsValue.Text = clsFormat.Money(Math.Abs(totalValue));

        lblEmpty.Visible = _linesView.Count == 0;

        if (lblEmpty.Visible)
            lblEmpty.BringToFront();
    }

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

        return _isDraft && _adjustmentId.HasValue ? btnPost : btnSaveDraft;
    }

    /// <summary>RULE-N25/N26: حفظ المسودة — معالج async مع try/catch و WaitCursor.</summary>
    private async Task _SaveDraftAsync()
    {
        if (!_ValidateForm()) return;

        Cursor = Cursors.WaitCursor;
        btnSaveDraft.Enabled = false;

        try
        {
            short warehouseId = cmbWarehouse.SelectedValue is short id ? id : (short)0;
            byte adjustmentType = cmbAdjustmentType.SelectedValue is byte type ? type : (byte)1;

            var request = new CreateInventoryAdjustmentRequest(
                WarehouseId: warehouseId,
                AdjustmentType: adjustmentType,
                Reason: txtReason.Text.Trim());

            var result = await AdjustmentService.CreateAsync(request);

            if (result.IsSuccess && result.Value is not null)
            {
                _adjustmentId = result.Value.Id;
                _isDraft = true;
                txtAdjustmentNo.Text = result.Value.AdjustmentNo;
                btnPost.Enabled = true;
                AcceptButton = _PrimaryActionButton();   // RULE-N147 — صار الزر الأساسي «ترحيل»

                MessageBox.Show("تم حفظ مسودة التسوية بنجاح", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر حفظ مسودة التسوية.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر حفظ مسودة تسوية المخزون");
            MessageBox.Show("تعذر حفظ مسودة التسوية. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
            {
                btnSaveDraft.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>RULE-N25/N26: ترحيل التسوية — الخدمة تُنفّذه داخل معاملة (RULE-N03).</summary>
    private async Task _PostAdjustmentAsync()
    {
        if (_adjustmentId is null)
        {
            await _SaveDraftAsync();
            if (_adjustmentId is null) return;
        }

        Cursor = Cursors.WaitCursor;
        btnPost.Enabled = false;

        try
        {
            var result = await AdjustmentService.PostAsync(_adjustmentId.Value);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم ترحيل تسوية المخزون بنجاح", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر ترحيل التسوية.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر ترحيل تسوية المخزون");
            MessageBox.Show("تعذر ترحيل التسوية. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
            {
                btnPost.Enabled = _isDraft && _adjustmentId is not null;
                AcceptButton = _PrimaryActionButton();   // RULE-N147 — إعادة مزامنة الزر الافتراضي
                Cursor = Cursors.Default;
            }
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbWarehouse.SelectedValue is not short warehouseId || warehouseId <= 0)
        {
            errorProvider.SetError(cmbWarehouse, "يرجى اختيار المستودع");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbWarehouse, "");
        }

        if (cmbAdjustmentType.SelectedValue is null)
        {
            errorProvider.SetError(cmbAdjustmentType, "يرجى اختيار نوع التسوية");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbAdjustmentType, "");
        }

        if (string.IsNullOrWhiteSpace(txtReason.Text))
        {
            errorProvider.SetError(txtReason, "سبب التعديل مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtReason, "");
        }

        if (dgvLines.Rows.Count == 0)
        {
            errorProvider.SetError(dgvLines, "يجب إضافة صنف واحد على الأقل");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(dgvLines, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    /// <summary>تحديث بيانات المستند من الخدمة (F5).</summary>
    private async void _Reload()
    {
        if (!IsHandleCreated) return;

        Cursor = Cursors.WaitCursor;

        try
        {
            if (_adjustmentId.HasValue)
                await _LoadAdjustmentDataAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحديث بيانات تسوية المخزون");
            MessageBox.Show("تعذر تحديث بيانات التسوية. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                await _LoadAdjustmentDataAsync();
            else
                _UpdateTotals();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحميل شاشة تسوية المخزون");
            MessageBox.Show("تعذر تحميل بيانات الشاشة. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private async void btnSaveDraft_Click(object sender, EventArgs e)
    {
        await _SaveDraftAsync();
    }

    private async void btnPost_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("هل أنت متأكد من ترحيل التسوية؟", "تأكيد",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        await _PostAdjustmentAsync();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    // ── Context Menu ──────────────────────────────────────────────────────────────

    private void miCopyCell_Click(object sender, EventArgs e)
    {
        var cell = dgvLines.CurrentCell;

        if (cell?.Value is null)
        {
            MessageBox.Show("يرجى تحديد خلية تحتوي قيمة أولاً.", "نسخ",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            Clipboard.SetText(cell.Value?.ToString() ?? string.Empty);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر نسخ قيمة خلية من بنود التسوية");
            MessageBox.Show("تعذر نسخ القيمة. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void miRefresh_Click(object sender, EventArgs e)
        => _Reload();
}
