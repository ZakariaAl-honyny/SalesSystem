using System.Runtime.CompilerServices;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// حوار «إضافة صنف» (إدخال): بيانات الصنف + شبكة الوحدات والأسعار + المخزون الافتتاحي.
/// <para>
/// المعيار المطبَّق: §2.27 (الستة من KeyboardHelper) + §2.19/§2.35 (الثيم والمصمّم = وقت التشغيل)
/// + تحقق <c>errorProvider</c> برسائل عربية (RULE-N28) + Esc إلغاء (RULE-N47)
/// + التفقيط الحيّ على حقول المبلغ (RULE-N97). شاشة حوار إدخال: بلا <c>gbFilter</c>/<c>pnlSummary</c>.
/// </para>
/// <para>
/// البيانات تجريبية في الذاكرة (UI-First) — لا استدعاء لأي خدمة.
/// </para>
/// </summary>
public partial class frmInventoryAddProduct : Form
{
    /// <summary>RULE-N46: لون الحقل النشط الموحّد.</summary>
    private static Color FocusColor => ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor);

    private readonly IServiceProvider _serviceProvider;

    /// <summary>محررات خلايا المبالغ التي وُصِّل بها التفقيط الحيّ (تمنع التوصيل المزدوج).</summary>
    private readonly ConditionalWeakTable<Control, object> _tafqeetWiredEditors = new();

    // ── Mock data (in-memory, Arabic) ─────────────────────────────────────────────
    private readonly string[] _categories = { "مواد غذائية", "مشروبات", "تنظيف", "أخرى" };
    private readonly string[] _warehouses = { "المستودع الرئيسي", "مخزن التجزئة", "مخزن الجملة" };

    public frmInventoryAddProduct(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط وأسهم RTL.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnCancel);

        // RULE-N47 — Esc إلغاء وإغلاق الحوار.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.Escape, () => btnCancel.PerformClick()));

        // RULE-N147 — Enter = الزر الأساسي للوضع (حفظ/إضافة)، و AcceptButton هو الزر نفسه.
        // ملاحظة أمان: الدالة تُرجع null عند تركيز شبكة الوحدات أو حقل الباركود (أدناه).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        // RULE-N97 — التفقيط الحيّ على الحقول الرقمية (الكمية الافتتاحية).
        LiveTafqeetHelper.Attach(liveTip,
            (nudOpeningQty, "الكمية الافتتاحية", LiveTafqeetHelper.ReadMode.Quantity));

        _SetupToolTips();

        ApplyUiTheme();   // RULE-N50 — ينتهي بالثيم الأخضر
    }

    // ── الثيم ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// الثيم الأخضر (RULE-N50…RULE-N53): يبدأ بـ ThemeHelper وينتهي بـ
    /// <c>GreenThemeHelper.Apply(this)</c> **كآخر سطر**، والقيم النهائية مطابقة حرفياً
    /// لما أُعلن في <c>frmInventoryAddProduct.Designer.cs</c> (RULE-N134/N140…N144).
    /// </summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);   // يفرض أبيض/Segoe UI ⇒ يردّه الثيم الأخضر أدناه

        GreenThemeHelper.ApplyHeader(lblMode);          // RULE-N53
        GreenThemeHelper.ApplyGrid(dgvUnits);           // RULE-N51/N52

        // شبكة إدخال سطور (تُدخَل فيها الوحدات والأسعار مباشرةً): تعلن سلوكها بنفسها،
        // و ApplyGrid لا يلمس ReadOnly/SelectionMode/AllowUserToAddRows (RULE-N52).
        dgvUnits.ReadOnly = false;
        dgvUnits.AllowUserToAddRows = true;
        dgvUnits.SelectionMode = DataGridViewSelectionMode.CellSelect;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>تلميحات الحقول والأزرار مع اختصاراتها (RULE-N47).</summary>
    private void _SetupToolTips()
    {
        toolTip1.SetToolTip(txtName, "اسم الصنف كما سيظهر في الفواتير");
        toolTip1.SetToolTip(txtBarcode, "باركود الوحدة الرئيسية — يُقرأ بالماسح الضوئي");
        toolTip1.SetToolTip(cmbCategory, "مجموعة الصنف");
        toolTip1.SetToolTip(nudReorderLevel, "حد الطلب: عند بلوغ الرصيد هذا الحد يظهر تنبيه");
        toolTip1.SetToolTip(chkTrackExpiry, "تفعيل متابعة تاريخ الصلاحية وتشغيل FEFO");
        toolTip1.SetToolTip(dgvUnits, "أضف وحدة واحدة على الأقل مع سعر البيع والتكلفة");
        toolTip1.SetToolTip(cmbWarehouse, "المستودع الذي يُسجَّل فيه المخزون الافتتاحي");
        toolTip1.SetToolTip(nudOpeningQty, "الكمية الافتتاحية بالوحدة الأساسية (تظهر بالحروف أسفلها)");
        toolTip1.SetToolTip(txtBatchNo, "رقم التشغيلة (Batch) — يُستخدم مع متابعة الصلاحية");
        toolTip1.SetToolTip(dtpExpiry, "تاريخ صلاحية التشغيلة");
        toolTip1.SetToolTip(btnSave, "حفظ الصنف (Enter)");
        toolTip1.SetToolTip(btnCancel, "إلغاء وإغلاق (Esc)");
    }

    private void frmInventoryAddProduct_Load(object sender, EventArgs e)
    {
        try
        {
            _LoadMockData();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تهيئة حقول حوار إضافة صنف");
            clsDialogs.Error("تعذر تهيئة شاشة إضافة الصنف. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// Populates the mock lists and seeds one default unit row so the
    /// multi-unit structure is immediately visible in the UI shape.
    /// </summary>
    private void _LoadMockData()
    {
        cmbCategory.Items.AddRange(_categories);
        cmbCategory.SelectedIndex = 0;

        cmbWarehouse.Items.AddRange(_warehouses);
        cmbWarehouse.SelectedIndex = 0;

        dtpExpiry.Value = DateTime.Today;
        dtpExpiry.Enabled = chkTrackExpiry.Checked;

        int rowIndex = dgvUnits.Rows.Add();
        dgvUnits.Rows[rowIndex].Cells["colUnit"].Value = "كيس";
        dgvUnits.Rows[rowIndex].Cells["colFactor"].Value = 1m;
        dgvUnits.Rows[rowIndex].Cells["colBarcode"].Value = "";
        dgvUnits.Rows[rowIndex].Cells["colSellPrice"].Value = 0m;
        dgvUnits.Rows[rowIndex].Cells["colCost"].Value = 0m;
    }

    private void chkTrackExpiry_CheckedChanged(object sender, EventArgs e)
    {
        dtpExpiry.Enabled = chkTrackExpiry.Checked;
        if (chkTrackExpiry.Checked)
            dtpExpiry.Value = DateTime.Today;
    }

    // ── التفقيط الحيّ داخل خلايا المبالغ (RULE-N97) ────────────────────────────

    /// <summary>
    /// يوصِّل التفقيط الحيّ بمحرر الخلية عند تحرير «سعر البيع» أو «التكلفة»، مرة واحدة لكل محرر.
    /// </summary>
    private void dgvUnits_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
    {
        if (dgvUnits.CurrentCell is null || e.Control is not Control editor) return;

        string columnName = dgvUnits.CurrentCell.OwningColumn?.Name ?? string.Empty;
        if (columnName is not ("colSellPrice" or "colCost")) return;

        if (_tafqeetWiredEditors.TryGetValue(editor, out _)) return;
        _tafqeetWiredEditors.Add(editor, new object());

        LiveTafqeetHelper.Attach(liveTip,
            (editor,
             columnName == "colCost" ? "التكلفة" : "سعر البيع",
             LiveTafqeetHelper.ReadMode.Money));
    }

    // ── قائمة سطر السياق (RULE-N23) ────────────────────────────────────────────

    /// <summary>
    /// RULE-N23: لا قائمة بلا سطر محدد — تُلغى إذا لم يكن هناك سطر وحدة حقيقي محدد
    /// (سطر الإضافة الجديد ليس سطراً قابلاً للحذف).
    /// </summary>
    private void cmsRows_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
        => e.Cancel = dgvUnits.CurrentRow is null || dgvUnits.CurrentRow.IsNewRow;

    /// <summary>
    /// «حذف السطر»: يحذف سطر الوحدة المحدد من شبكة الإدخال — قدرة الشبكة المُعلنة أصلاً
    /// في المصمّم (<c>AllowUserToDeleteRows = true</c>)، ولا يحتاج الإضافة لأن سطر الإضافة
    /// السفلي متاح أصلاً (<c>AllowUserToAddRows = true</c>).
    /// </summary>
    private void tsmiDeleteUnitRow_Click(object? sender, EventArgs e)
    {
        try
        {
            dgvUnits.EndEdit();

            var row = dgvUnits.CurrentRow;
            if (row is null || row.IsNewRow) return;

            dgvUnits.Rows.Remove(row);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر حذف سطر الوحدة في حوار إضافة صنف");
            clsDialogs.Error("تعذر حذف السطر. يرجى إعادة المحاولة.");
        }
    }

    // ── الزر الأساسي للوضع (RULE-N147) ─────────────────────────────────────────

    /// <summary>
    /// الزر الأساسي للوضع الحالي — في هذا الحوار زر التأكيد/الإضافة فقط
    /// (<c>btnSave</c>). تُقرأ الدالة عند كل ضغطة <c>Enter</c> لا مرة واحدة.
    /// <para>
    /// <b>حماية إلزامية:</b> تُرجع <c>null</c> عند تركيز شبكة الوحدات (<c>dgvUnits</c>)
    /// أو حقل الباركود (<c>txtBarcode</c>) — لأن <c>Enter</c> هناك يجب أن يبقى لسلوك
    /// العنصر نفسه (تحرير/انتقال داخل الشبكة، وقراءة الباركود)، فلا يُضاف الصنف بالخطأ.
    /// </para>
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (dgvUnits.Focused || dgvUnits.ContainsFocus) return null;   // الشبكة: Enter = تنقل/تحرير خلية
        if (txtBarcode.Focused || txtBarcode.ContainsFocus) return null; // الباركود: Enter = قراءة/تحقق

        return btnSave;
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (!clsValidation.IsRequired(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الصنف مطلوب");
            clsInputFeedback.ShowRejectTooltip(txtName, "اسم الصنف مطلوب", toolTip1);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (!clsValidation.IsRequired(txtBarcode.Text))
        {
            errorProvider.SetError(txtBarcode, "الباركود مطلوب");
            clsInputFeedback.ShowRejectTooltip(txtBarcode, "الباركود مطلوب", toolTip1);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtBarcode, "");
        }

        bool hasUnitRow = false;
        foreach (DataGridViewRow row in dgvUnits.Rows)
        {
            if (row.IsNewRow) continue;
            string? unit = row.Cells["colUnit"].Value as string;
            if (clsValidation.IsRequired(unit))
            {
                hasUnitRow = true;
                break;
            }
        }

        if (!hasUnitRow)
        {
            errorProvider.SetError(dgvUnits, "يجب إضافة وحدة واحدة على الأقل");
            clsInputFeedback.ShowRejectTooltip(dgvUnits, "يجب إضافة وحدة واحدة على الأقل", toolTip1);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(dgvUnits, "");
        }

        if (nudOpeningQty.Value < 0)
        {
            errorProvider.SetError(nudOpeningQty, "الكمية الافتتاحية يجب أن تكون أكبر من أو تساوي صفر");
            clsInputFeedback.ShowRejectTooltip(nudOpeningQty, "الكمية الافتتاحية يجب أن تكون أكبر من أو تساوي صفر", toolTip1);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(nudOpeningQty, "");
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        try
        {
            Cursor = Cursors.WaitCursor;

            // Mock-only: no backend call yet.
            // Collected shape: txtName, txtBarcode, cmbCategory, nudReorderLevel,
            // chkTrackExpiry + dgvUnits rows (الوحدة / معامل التحويل / الباركود /
            // سعر البيع / التكلفة) + cmbWarehouse, nudOpeningQty, txtBatchNo, dtpExpiry.

            clsDialogs.Success("تم إضافة الصنف بنجاح");

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء إضافة الصنف");
            clsDialogs.Error("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        Close();
    }
}
