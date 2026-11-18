using ClosedXML.Excel;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common.Helpers;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using Serilog;
using System.Data;
using System.Text;
using SalesSystem.Desktop.Messaging.Messages;

namespace SalesSystem.Desktop.Screens.Products.Forms;

/// <summary>
/// استيراد الأصناف من ملف Excel: اختيار الملف ← خيارات الاستيراد ← معاينة الصفوف ← اعتماد.
/// الشاشة عرض وتنظيم فقط: قراءة الملف والتحقق من الصفوف وتمرير الطلبات تبقى كما هي،
/// وكل ما يمسّ الخادم يمرّ عبر <see cref="IProductDesktopService"/> (RULE-N06).
/// </summary>
public partial class frmImportProductsExcel : Form
{
    private readonly IServiceProvider _serviceProvider;
    private DataTable _dtPreview = new();
    private int _generatedBarcodeSeq = 1000;

    /// <summary>دليل التصنيفات — يُحلّ منه اسم التصنيف المكتوب في الملف إلى معرّفه (لا معرّف ثابت).</summary>
    private List<CategoryDto> _categories = new();

    /// <summary>دليل الوحدات — يُحلّ منه اسم/رمز الوحدة في الملف إلى معرّفها.</summary>
    private List<UnitDto> _units = new();

    /// <summary>باركودات الأصناف المسجّلة فعلاً — أساس التحذير «الباركود مسجل مسبقاً».</summary>
    private readonly HashSet<string> _existingBarcodes = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// الملف يحمل رصيداً افتتاحياً (كمية &gt; 0) ولا مسار لترحيله من هذه الشاشة،
    /// فيُوقف الاعتماد كله بدل إسقاط الكميات بصمت.
    /// </summary>
    private bool _openingStockBlocked;

    public frmImportProductsExcel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        _SetupGrid();

        // RULE-N40…RULE-N47 — كل الستة من KeyboardHelper (RULE-N95)
        // RULE-N191 — Enter داخل كتلة الملف يمشي بترتيب العرض ثم يُسلِّم **تركيزاً** لزر الوضع بلا تنفيذ.
        // نطاق التنقل هو كتلة الملف (gbFile) لا النموذج كله: حقل البحث txtSearch تصفيةٌ تبقى على SelectNextControl (RULE-N59).
        KeyboardHelper.WireEnterAsTab(gbFile, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 — الحقل النشط
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45 — أسهم RTL
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnGenerateBarcodes);      // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
            (Keys.F5, () => btnSelectFile.PerformClick()),
            (Keys.F2, () => btnSave.PerformClick()),
            (Keys.Escape, () => btnClose.PerformClick()));

        // RULE-N147 — Enter = الزر الأساسي لوضع الاستيراد الحالي (يُقرأ عند كل ضغطة لا مرة واحدة)
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        _SyncPrimaryAction();

        // RULE-N180…N183 — تغذية الحقل المطلوب حيّاً (أيقونة ✗ + ومضة رفض) للحقل المطلوب الوحيد في الشاشة: مسار الملف.
        // ترتيب الاستدعاء ملزم: يأتي **بعد** WireFocusHighlight أعلاه (ترتيب اشتراك Leave يحسم مَن يرسم الخلفية أخيراً)
        // و**قبل** GreenThemeHelper.Apply في ApplyUiTheme أدناه.
        RequiredFieldHelper.Attach(this, errorProvider,
            new RequiredField(txtFilePath, "يجب اختيار ملف إكسل أولاً"));

        ApplyUiTheme(); // RULE-N50
    }

    // ── Enter = الزر الأساسي للوضع (RULE-N147) ─────────────────────────────────

    /// <summary>
    /// الزر الأساسي لوضع الاستيراد الحالي — شاشة معالج (Wizard) فلا يجوز أن يُعتمد الاستيراد بالخطأ:
    /// <list type="bullet">
    /// <item>قبل اختيار الملف (لا صفوف في المعاينة) ⇒ «اختر ملف إكسل» — الخطوة التالية الطبيعية وبلا كتابة بيانات.</item>
    /// <item>بعد ظهور المعاينة ⇒ <c>null</c>: زر «اعتماد وحفظ» يكتب بيانات ولا يُربط بـ<c>Enter</c> أبداً.</item>
    /// <item>شبكة المعاينة في التركيز ⇒ <c>null</c> دائماً (حماية إضافية من اعتماد/فتح ملف غير مقصود).</item>
    /// </list>
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        // شبكة المعاينة (أو محرر خلية داخلها) في التركيز ⇒ لا إجراء أساسي إطلاقاً.
        if (dgvPreview.ContainsFocus) return null;

        // وُجد ملف ومعه صفوف معاينة ⇒ المرحلة الحاسمة: الاعتماد بنقرة صريحة فقط.
        if (_dtPreview.Rows.Count > 0) return null;

        return btnSelectFile;
    }

    /// <summary>
    /// يزامن <see cref="Form.AcceptButton"/> مع الزر الأساسي للوضع الحالي (RULE-N147).
    /// لا حالة جديدة: الوضع مشتق من وجود صفوف في المعاينة.
    /// </summary>
    private void _SyncPrimaryAction()
    {
        try
        {
            AcceptButton = _PrimaryActionButton();
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "تعذر مزامنة زر Enter الأساسي في شاشة استيراد الأصناف");
        }
    }

    // ── الشبكة ────────────────────────────────────────────────────────────────

    private void _SetupGrid()
    {
        dgvPreview.AutoGenerateColumns = false;
        dgvPreview.Columns.Clear();

        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "الحالة", DataPropertyName = "Status", Width = 120, ReadOnly = true });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "RowNo", HeaderText = "الصف", DataPropertyName = "RowNo", Width = 60, ReadOnly = true });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "اسم الصنف", DataPropertyName = "Name", Width = 200 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Barcode", HeaderText = "الباركود", DataPropertyName = "Barcode", Width = 150 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Qty", HeaderText = "رصيد افتتاحي", DataPropertyName = "Qty", Width = 100 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Cost", HeaderText = "سعر التكلفة", DataPropertyName = "Cost", Width = 100 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "سعر البيع", DataPropertyName = "Price", Width = 100 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Unit", HeaderText = "الوحدة", DataPropertyName = "Unit", Width = 80 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Category", HeaderText = "التصنيف", DataPropertyName = "Category", Width = 140 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Message", HeaderText = "ملاحظات", DataPropertyName = "Message", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });

        _dtPreview.Columns.Add("Status", typeof(string));
        _dtPreview.Columns.Add("RowNo", typeof(int));
        _dtPreview.Columns.Add("Name", typeof(string));
        _dtPreview.Columns.Add("Barcode", typeof(string));
        _dtPreview.Columns.Add("Qty", typeof(decimal));
        _dtPreview.Columns.Add("Cost", typeof(decimal));
        _dtPreview.Columns.Add("Price", typeof(decimal));
        _dtPreview.Columns.Add("Unit", typeof(string));
        _dtPreview.Columns.Add("Category", typeof(string));
        _dtPreview.Columns.Add("Message", typeof(string));
        _dtPreview.Columns.Add("IsValid", typeof(bool));

        // العرض عبر DataView حتى تعمل التصفية الفورية (RULE-N24 / RULE-N60)
        dgvPreview.DataSource = _dtPreview.DefaultView;

        // RULE-N147 — الزر الأساسي يتبع التركيز: فرع «شبكة المعاينة في التركيز ⇒ لا إجراء أساسي»
        // في _PrimaryActionButton لا يعمل ما لم يُزامَن AcceptButton عند دخول الشبكة وخروجها.
        dgvPreview.Enter += (_, _) => _SyncPrimaryAction();
        dgvPreview.Leave += (_, _) => _SyncPrimaryAction();
    }

    // ── اختيار الملف والقراءة ──────────────────────────────────────────────────

    private async void btnSelectFile_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog { Filter = "Excel Files|*.xlsx", Title = "اختر ملف المنتجات" };
        if (ofd.ShowDialog() != DialogResult.OK) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            // الدلائل إلزامية قبل التحقق: اسم التصنيف والوحدة في الملف يُحلّان إلى معرّف من الدليل،
            // وباركودات الأصناف المسجّلة تُقرأ من الخدمة (لا من مجموعة فارغة).
            if (!await _LoadLookupsAsync())
            {
                clsDialogs.Error("تعذر تحميل دليل التصنيفات والوحدات، فلا يمكن التحقق من الملف الآن. يرجى إعادة المحاولة.");
                return;
            }

            txtFilePath.Text = ofd.SafeFileName;
            _ProcessExcelFile(ofd.FileName);
            _SyncPrimaryAction(); // RULE-N147 — بعد ظهور المعاينة يصبح Enter بلا زر أساسي
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحميل دلائل استيراد الأصناف");
            clsDialogs.Error("تعذر تحميل بيانات التصنيفات والوحدات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// تحميل دلائل التصنيفات والوحدات وباركودات الأصناف القائمة عبر العقود المكتبية فقط.
    /// يُرجع <c>false</c> إذا تعذر تحميل أي دليل — فلا يُتحقق من الملف بقائمة فارغة تُرفض كل صفوفه.
    /// </summary>
    private async Task<bool> _LoadLookupsAsync()
    {
        var categoryRes = await _serviceProvider.GetRequiredService<ICategoryDesktopService>().GetAllAsync();
        var unitRes = await _serviceProvider.GetRequiredService<IUnitDesktopService>().GetAllAsync();

        if (!categoryRes.IsSuccess || categoryRes.Value is null || !unitRes.IsSuccess || unitRes.Value is null)
        {
            Log.Warning("تعذر تحميل دلائل الاستيراد: التصنيفات={CategoryError} الوحدات={UnitError}",
                categoryRes.Error, unitRes.Error);
            return false;
        }

        _categories = categoryRes.Value.ToList();
        _units = unitRes.Value.ToList();

        _existingBarcodes.Clear();
        var productRes = await _serviceProvider.GetRequiredService<IProductDesktopService>().GetAllAsync(includeInactive: true);
        if (productRes.IsSuccess && productRes.Value is not null)
        {
            foreach (var product in productRes.Value)
                if (!string.IsNullOrWhiteSpace(product.Barcode))
                    _existingBarcodes.Add(product.Barcode.Trim());
        }

        return true;
    }

    private void _ProcessExcelFile(string filePath)
    {
        Cursor = Cursors.WaitCursor;

        try
        {
            _dtPreview.Clear();

            using var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet(1);

            // RangeUsed() = null على ورقة فارغة — الرسالة تقول الحقيقة بدل تحميل NullReference
            // على «الملف مفتوح في تطبيق آخر» وهي ليست السبب.
            if (ws.RangeUsed() is not { } usedRange)
            {
                _ApplyGridColors();
                _ApplyPreviewFilter();
                _UpdateOpeningStockBlock(announce: true);
                clsDialogs.Warning("الملف فارغ — لا توجد صفوف لاستيرادها.", "لا بيانات");
                return;
            }

            var rows = usedRange.RowsUsed().Skip(1); // skip header

            foreach (var row in rows)
            {
                var name = row.Cell(1).GetString().Trim();
                if (string.IsNullOrEmpty(name)) continue;

                var qty = row.Cell(2).TryGetValue<decimal>(out var q) ? q : 0m;
                var cost = row.Cell(3).TryGetValue<decimal>(out var c) ? c : 0m;
                var price = row.Cell(4).TryGetValue<decimal>(out var p) ? p : 0m;
                var unit = row.Cell(5).GetString().Trim();
                if (string.IsNullOrEmpty(unit)) unit = "حبة";
                var barcode = row.Cell(6).GetString().Trim();
                var category = row.Cell(7).GetString().Trim();   // عمود «التصنيف» في النموذج

                var dr = _dtPreview.NewRow();
                dr["RowNo"] = row.RowNumber();
                dr["Name"] = name;
                dr["Barcode"] = barcode;
                dr["Qty"] = qty;
                dr["Cost"] = cost;
                dr["Price"] = price;
                dr["Unit"] = unit;
                dr["Category"] = category;
                _dtPreview.Rows.Add(dr);

                // تحقق واحد لكل صف من نفس المكان (يُستهلك أيضاً عند التعديل اليدوي في المعاينة).
                _ValidatePreviewRow(dr);
            }

            _ApplyGridColors();
            _ApplyPreviewFilter(); // يُحدّث الإجماليات وحالة «لا توجد بيانات»
            _UpdateOpeningStockBlock(announce: true);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في قراءة ملف Excel");
            clsDialogs.Error("تعذر قراءة الملف. تأكد من أن الملف غير مفتوح في تطبيق آخر.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── تحقق صف المعاينة (بيت واحد يُستهلك عند القراءة والتعديل اليدوي) ─────────

    /// <summary>
    /// يضع حالة الصف وسببه: يتحقق من الاسم والباركود والرصيد الافتتاحي والأسعار والتصنيف والوحدة.
    /// التصنيف والوحدة يُحلّان **بالاسم** من دلائل الخدمة (لا معرّف ثابت)، والفشل في الحل رفضٌ للصف
    /// برسالة تسمّي الصف والقيمة — فلا يُرسَل صنف على تصنيف أو وحدة غير موجودة.
    /// </summary>
    /// <param name="dr">صف المعاينة.</param>
    /// <param name="manualEdit">عند التعديل اليدوي في الشبكة تبقى عبارة «تم التعديل يدوياً» عند القبول.</param>
    private void _ValidatePreviewRow(DataRow dr, bool manualEdit = false)
    {
        int rowNo = dr["RowNo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RowNo"]);
        string rowPrefix = rowNo > 0 ? $"الصف {rowNo}: " : string.Empty;

        string name = (dr["Name"]?.ToString() ?? string.Empty).Trim();
        string barcode = (dr["Barcode"]?.ToString() ?? string.Empty).Trim();
        string categoryName = (dr["Category"]?.ToString() ?? string.Empty).Trim();
        string unitName = (dr["Unit"]?.ToString() ?? string.Empty).Trim();
        decimal qty = dr["Qty"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["Qty"]);
        decimal cost = dr["Cost"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["Cost"]);
        decimal price = dr["Price"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["Price"]);

        string status = "مقبول";
        string message = manualEdit ? "تم التعديل يدوياً" : string.Empty;

        if (string.IsNullOrEmpty(name))
        {
            status = "خطأ"; message = "اسم الصنف مفقود";
        }
        else if (!string.IsNullOrEmpty(barcode) &&
                 _dtPreview.AsEnumerable().Count(r => (r["Barcode"]?.ToString() ?? string.Empty).Trim().SameText(barcode)) > 1)
        {
            status = "تحذير"; message = "الباركود مكرر في هذا الملف";
        }
        else if (!string.IsNullOrEmpty(barcode) && _existingBarcodes.Contains(barcode))
        {
            status = "تحذير"; message = "الباركود مسجل مسبقاً";
        }
        else if (qty > 0m)
        {
            // لا مسار لترحيل رصيد افتتاحي من هذه الشاشة ⇒ إسقاطه بصمت ممنوع.
            status = "خطأ"; message = $"{rowPrefix}الرصيد الافتتاحي ({clsFormat.Quantity(qty)}) غير قابل للاستيراد بعد";
        }
        else if (cost < 0m)
        {
            status = "خطأ"; message = $"{rowPrefix}سعر التكلفة لا يمكن أن يكون سالباً";
        }
        else if (price < 0m)
        {
            status = "خطأ"; message = $"{rowPrefix}سعر البيع لا يمكن أن يكون سالباً";
        }
        else if (string.IsNullOrEmpty(categoryName))
        {
            status = "خطأ"; message = $"{rowPrefix}التصنيف مفقود — اكتب اسم تصنيف مسجل في عمود «التصنيف»";
        }
        else if (!_categories.Any(c => c.Name.SameText(categoryName)))
        {
            status = "خطأ"; message = $"{rowPrefix}التصنيف «{categoryName}» غير مسجل في دليل التصنيفات";
        }
        else if (string.IsNullOrEmpty(unitName))
        {
            status = "خطأ"; message = $"{rowPrefix}الوحدة مفقودة — اكتب اسم وحدة مسجلة في عمود «الوحدة الأساسية»";
        }
        else if (!_units.Any(u => u.Name.SameText(unitName) || (u.Symbol ?? string.Empty).SameText(unitName)))
        {
            status = "خطأ"; message = $"{rowPrefix}الوحدة «{unitName}» غير مسجلة في دليل الوحدات";
        }

        dr["Status"] = status;
        dr["Message"] = message;
        dr["IsValid"] = status == "مقبول";
    }

    /// <summary>
    /// يُوقف «اعتماد وحفظ» إذا حمل الملف رصيداً افتتاحياً (كمية &gt; 0): لا يوجد في العقد المكتبي
    /// مسار ترحيل رصيد افتتاحي تحدده هذه الشاشة (الشاشة لا تحدد مستودعاً، و
    /// <c>CreateInventoryAdjustmentRequest</c> يحتاج <c>WarehouseId</c>) — فالإسقاط الصامت ممنوع،
    /// والرفض معلن برسالة عربية واحدة.
    /// </summary>
    /// <param name="announce">يُعلن الرفض برسالة عربية مرة واحدة عند قراءة الملف (لا عند كل تعديل صف).</param>
    private void _UpdateOpeningStockBlock(bool announce = false)
    {
        int affectedRows = _dtPreview.AsEnumerable()
            .Count(r => r["Qty"] != DBNull.Value && Convert.ToDecimal(r["Qty"]) > 0m);

        _openingStockBlocked = affectedRows > 0;
        btnSave.Enabled = !_openingStockBlocked;

        if (_openingStockBlocked && announce)
        {
            clsDialogs.Warning(
                $"الملف يحتوي عمود «الكمية الافتتاحية» بقيم أكبر من صفر في {affectedRows} صفاً، " +
                "ولا يمكن استيراد الرصيد الافتتاحي من هذه الشاشة بعد (لا يوجد في العقد المكتبي مسار رصيد افتتاحي يحدد مستودعاً).\n" +
                "لن يُعتمد أي صنف من هذا الملف حتى تُصفَّر الكميات الافتتاحية أو يُحذف عمودها — " +
                "ثم تُدخل الأرصدة من شاشة «بضاعة أول المدة».",
                "الاستيراد موقوف");
        }
    }

    // ── التصفية والإجماليات ────────────────────────────────────────────────────

    private void txtSearch_TextChanged(object? sender, EventArgs e) => _ApplyPreviewFilter();

    /// <summary>تصفية فورية على عمودين معروفين فقط (Name/Barcode) وبهروب آمن (RULE-N60).</summary>
    private void _ApplyPreviewFilter()
    {
        try
        {
            string value = txtSearch.Text.Trim();

            _dtPreview.DefaultView.RowFilter = string.IsNullOrEmpty(value)
                ? string.Empty
                : $"{RowFilterHelper.Like("Name", value)} OR {RowFilterHelper.Like("Barcode", value)}";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تصفية معاينة استيراد الأصناف");
            _dtPreview.DefaultView.RowFilter = string.Empty;
            clsDialogs.Warning("تعذر تطبيق البحث على المعاينة.");
        }

        _UpdateSummary();
    }

    /// <summary>RULE-N61 — شريط الملخص: العدد المعروض + المقبولة/المرفوضة + حالة «لا توجد بيانات».</summary>
    private void _UpdateSummary()
    {
        int accepted = 0;
        int rejected = 0;

        foreach (DataRow row in _dtPreview.Rows)
        {
            bool valid = row["IsValid"] != DBNull.Value && (bool)row["IsValid"];
            if (valid) accepted++;
            else rejected++;
        }

        lblCount.Text = $"عدد الأصناف: {_dtPreview.DefaultView.Count}";
        lblSummary.Text = $"المقبولة: {accepted}  ·  المرفوضة: {rejected}";
        lblNoData.Visible = _dtPreview.DefaultView.Count == 0;
    }

    // ── تلوين حالة الصفوف ──────────────────────────────────────────────────────

    private void _ApplyGridColors()
    {
        string currentFilter = _dtPreview.DefaultView.RowFilter;

        try
        {
            // إزالة التصفية مؤقتاً لتلوين كل الصفوف لا المعروض منها فقط.
            _dtPreview.DefaultView.RowFilter = string.Empty;

            foreach (DataGridViewRow row in dgvPreview.Rows)
            {
                var status = GridRowHelper.GetText(row, "Status");
                if (status == "خطأ") row.DefaultCellStyle.BackColor = GreenThemeHelper.StatusDangerBack;
                else if (status == "تحذير") row.DefaultCellStyle.BackColor = GreenThemeHelper.StatusWarningBack;
                else row.DefaultCellStyle.BackColor = Color.Empty;   // الصف السليم يورّث تنسيق الشبكة (RULE-N51a)
            }
        }
        finally
        {
            _dtPreview.DefaultView.RowFilter = currentFilter;
        }
    }

    // ── الأوامر ────────────────────────────────────────────────────────────────

    private void btnGenerateBarcodes_Click(object sender, EventArgs e)
    {
        foreach (DataRow row in _dtPreview.Rows)
        {
            if (string.IsNullOrWhiteSpace(row["Barcode"].ToString()))
            {
                row["Barcode"] = $"629{_generatedBarcodeSeq++}";

                // إعادة التحقق من نفس البيت: الصف الذي كان مرفوضاً لسبب آخر (تصنيف/وحدة/رصيد)
                // لا يُقبل لمجرد توليد باركود له.
                _ValidatePreviewRow(row);
                if (row["IsValid"] is true)
                    row["Message"] = "تم توليد باركود";
            }
        }

        _ApplyGridColors();
        _ApplyPreviewFilter();
        _UpdateOpeningStockBlock();
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        // RULE-N180…N183 — المتطلب الوحيد في الشاشة: ملف مختار. لا اعتماد بلا ملف،
        // والحكم في البيت الواحد RequiredFieldHelper (أيقونة ✗ + ومضة رفض + التركيز على أول ناقص).
        if (!RequiredFieldHelper.Validate(this))
        {
            if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();
            return;
        }

        if (_openingStockBlocked)
        {
            // دفاع مزدوج: الزر معطّل، والرسالة تُعيد بيان السبب لو استُدعي المعالج بأي مسار آخر.
            clsDialogs.Warning("الاستيراد موقوف: الملف يحتوي كميات افتتاحية لا يمكن استيرادها من هذه الشاشة.");
            return;
        }

        var validRows = _dtPreview.AsEnumerable().Where(r => r.Field<bool>("IsValid")).ToList();
        if (!validRows.Any())
        {
            clsDialogs.Warning("لا توجد أصناف صالحة للاعتماد. راجع عمود «ملاحظات» لمعرفة سبب الرفض.");
            return;
        }

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            var productService = _serviceProvider.GetRequiredService<IProductDesktopService>();
            int successCount = 0;                                 // اسم + تصنيف + وحدة + أسعار
            int incompleteCount = 0;                              // أُنشئ الصنف وتعذرت وحدته/سعره
            var refusals = new List<string>();                    // رفض كامل قبل الإنشاء
            var incompleteReasons = new List<string>();           // الصنف أُنشئ ناقصاً

            foreach (var r in validRows)
            {
                string name = (r.Field<string>("Name") ?? string.Empty).Trim();
                string barcode = (r.Field<string>("Barcode") ?? string.Empty).Trim();
                string categoryName = (r.Field<string>("Category") ?? string.Empty).Trim();
                string unitName = (r.Field<string>("Unit") ?? string.Empty).Trim();
                decimal price = r["Price"] == DBNull.Value ? 0m : Convert.ToDecimal(r["Price"]);
                decimal cost = r["Cost"] == DBNull.Value ? 0m : Convert.ToDecimal(r["Cost"]);

                // (1) التصنيف يُحلّ بالاسم من الدليل — لا معرّف ثابت (كان CategoryId: 1).
                var category = _categories.FirstOrDefault(c => c.Name.SameText(categoryName));
                if (category is null)
                {
                    _RejectPreviewRow(r, $"التصنيف «{categoryName}» غير مسجل في دليل التصنيفات");
                    refusals.Add($"الصنف «{name}»: التصنيف «{categoryName}» غير مسجل");
                    continue;
                }

                // (2) الوحدة تُحلّ بالاسم أو الرمز من الدليل.
                var unit = _units.FirstOrDefault(u => u.Name.SameText(unitName) || (u.Symbol ?? string.Empty).SameText(unitName));
                if (unit is null)
                {
                    _RejectPreviewRow(r, $"الوحدة «{unitName}» غير مسجلة في دليل الوحدات");
                    refusals.Add($"الصنف «{name}»: الوحدة «{unitName}» غير مسجلة");
                    continue;
                }

                // (3) إرسال ما يدعمه العقد فعلاً: الاسم + التصنيف الحقيقي + الباركود.
                var req = new CreateProductRequest(
                    Name: name,
                    CategoryId: category.Id,
                    Description: "استيراد Excel",
                    ReorderLevel: 10,
                    TrackExpiry: false,
                    Barcode: string.IsNullOrEmpty(barcode) ? null : barcode);

                var res = await productService.CreateAsync(req);
                if (!res.IsSuccess || res.Value is not { } product)
                {
                    string reason = res.Error ?? "تعذر الاعتماد";
                    _RejectPreviewRow(r, reason);
                    refusals.Add($"الصنف «{name}»: {reason}");
                    Log.Warning("تعذر اعتماد صنف مستورد: {ProductName} — {Error}", name, res.Error);
                    continue;
                }

                // (4) ثم الوحدة الأساسية بسعر البيع والتكلفة والباركود — نفس نمط frmProductMaster.
                var unitRes = await productService.AddProductUnitAsync(product.Id, new AddProductUnitRequest(
                    UnitId: (short)unit.Id,
                    Factor: 1m,
                    IsBaseUnit: true,
                    Barcode: string.IsNullOrEmpty(barcode) ? null : barcode,
                    SalePrice: price,
                    WholesalePrice: null,
                    CostPrice: cost));

                if (!unitRes.IsSuccess || unitRes.Value is null)
                {
                    string reason = unitRes.Error ?? "تعذر تسجيل الوحدة والسعر";
                    incompleteCount++;
                    // لا يُعاد إرسال هذا الصف: الصنف أُنشئ فعلاً ويرفضه العقد باسم مكرر.
                    _RejectPreviewRow(r, $"أُنشئ الصنف لكن لم تُسجَّل وحدته وسعره: {reason}");
                    incompleteReasons.Add($"الصنف «{name}» أُنشئ بلا وحدة/سعر: {reason}");
                    Log.Error("تعذر إضافة وحدة الصنف المستورد {ProductId}: {Error}", product.Id, unitRes.Error);
                    continue;
                }

                successCount++;
            }

            // الرسالة تقول الحقيقة: كم استُورد فعلاً، وكم رُفض وبأي سبب — لا «تم اعتماد N» عند إسقاط أسعار/كميات.
            int skippedCount = _dtPreview.Rows.Count - validRows.Count;
            var message = new StringBuilder();
            message.AppendLine($"تم استيراد {successCount} صنفاً كاملاً (الاسم · التصنيف · الوحدة · الباركود · سعر البيع · سعر التكلفة).");

            if (incompleteCount > 0)
            {
                message.AppendLine($"أُنشئ {incompleteCount} صنفاً بلا وحدة/سعر — راجع عمود «ملاحظات»:");
                foreach (var reason in incompleteReasons.Take(10))
                    message.AppendLine("• " + reason);
            }
            if (refusals.Count > 0)
            {
                message.AppendLine($"لم يُستورد {refusals.Count} صنفاً، والأسباب:");
                foreach (var reason in refusals.Take(10))
                    message.AppendLine("• " + reason);
                if (refusals.Count > 10)
                    message.AppendLine($"• … و{refusals.Count - 10} سبب آخر في عمود «ملاحظات».");
            }
            if (skippedCount > 0)
                message.AppendLine($"{skippedCount} صفاً لم يُرسل لأنه غير صالح (سبب الرفض في عمود «ملاحظات»).");

            if (successCount > 0)
                clsDialogs.Info(message.ToString().TrimEnd(), "نتيجة الاستيراد");
            else
                clsDialogs.Warning(message.ToString().TrimEnd(), "تعذر الاستيراد");

            _ApplyGridColors();
            _ApplyPreviewFilter();

            if (successCount > 0)
            {
                var eventBus = _serviceProvider.GetRequiredService<IEventBus>();
                eventBus.Publish(new ProductChangedMessage(0));

                // الإغلاق فقط عند استيراد نظيف تماماً؛ وإلا تبقى الشاشة ليرى المستخدم أسباب الرفض سطراً بسطر.
                if (skippedCount == 0 && refusals.Count == 0 && incompleteCount == 0) Close();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اعتماد الأصناف المستوردة");
            clsDialogs.Error("تعذر اعتماد الأصناف. يرجى إعادة المحاولة.");
        }
        finally
        {
            // الزر يبقى معطّلاً إن كان الملف يحمل رصيداً افتتاحياً (الاستيراد موقوف).
            btnSave.Enabled = !_openingStockBlocked;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>يوسم صف المعاينة برفض فعلي وسببه (لا يُرسَل ولا يبقى «مقبولاً» في العدّ).</summary>
    private void _RejectPreviewRow(DataRow row, string reason)
    {
        row["Status"] = "خطأ";
        row["Message"] = reason;
        row["IsValid"] = false;
    }

    private void btnDownloadTemplate_Click(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;

        try
        {
            using var sfd = new SaveFileDialog { Filter = "Excel Files|*.xlsx", FileName = "نموذج_استيراد_الاصناف.xlsx" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Products");
                ws.RightToLeft = true;
                ws.Cell(1, 1).Value = "اسم الصنف";
                ws.Cell(1, 2).Value = "الكمية الافتتاحية";
                ws.Cell(1, 3).Value = "سعر التكلفة";
                ws.Cell(1, 4).Value = "سعر البيع";
                ws.Cell(1, 5).Value = "الوحدة الأساسية";
                ws.Cell(1, 6).Value = "الباركود";
                ws.Cell(1, 7).Value = "التصنيف";
                ws.Row(1).Style.Font.Bold = true;
                ws.Row(1).Style.Fill.BackgroundColor = XLColor.LightGray;
                wb.SaveAs(sfd.FileName);
                clsDialogs.Success("تم حفظ النموذج.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في إنشاء نموذج الاستيراد");
            clsDialogs.Error("تعذر حفظ النموذج. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>تحقق سريع عند تحرير صف في المعاينة (شبكة العرض مقروءة، والمعالج يبقى تحوطاً).</summary>
    private void dgvPreview_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var row = dgvPreview.Rows[e.RowIndex];
        if (row.DataBoundItem is not DataRowView view) return;

        var dr = view.Row;

        // التحقق من البيت الواحد: لا تُقبل خلية معدّلة يدوياً إن كان الصف مرفوضاً لسبب آخر
        // (تصنيف/وحدة/رصيد افتتاحي/سعر سالب) — نفس قواعد قراءة الملف.
        _ValidatePreviewRow(dr, manualEdit: true);

        _ApplyGridColors();
        _ApplyPreviewFilter();
        _UpdateOpeningStockBlock();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    // ── قائمة سطر السياق (RULE-N23) ────────────────────────────────────────────

    /// <summary>
    /// RULE-N23: لا قائمة بلا سطر محدد — تُلغى إذا لم يكن هناك سطر معاينة محدد.
    /// </summary>
    private void cmsRows_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
        => e.Cancel = dgvPreview.CurrentRow is null || dgvPreview.CurrentRow.IsNewRow;

    /// <summary>
    /// «نسخ قيمة الخلية»: نسخ نص الخلية الحالية إلى الحافظة — عرض فقط، لا منطق أعمال ولا كتابة بيانات.
    /// </summary>
    private void tsmiCopyCellValue_Click(object? sender, EventArgs e)
    {
        try
        {
            string? value = dgvPreview.CurrentCell?.Value?.ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                clsInputFeedback.ShowInfoTooltip(dgvPreview, "الخلية فارغة — لا يوجد ما يُنسخ.");
                return;
            }

            Clipboard.SetText(value);
            clsInputFeedback.ShowSuccessTooltip(dgvPreview, "تم نسخ القيمة.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر نسخ قيمة خلية من معاينة استيراد الأصناف");
            clsDialogs.Error("تعذر نسخ القيمة. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// «توليد الباركود المفقود»: يستدعي زر «توليد باركود للأصناف الفارغة» الموجود نفسه
    /// (<c>btnGenerateBarcodes</c>) بلا أي منطق مكرّر.
    /// </summary>
    private void tsmiGenerateMissingBarcode_Click(object? sender, EventArgs e)
    {
        if (btnGenerateBarcodes.Enabled)
            btnGenerateBarcodes.PerformClick();
    }

    // ── الثيم ─────────────────────────────────────────────────────────────────

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستهلك من الـHelper فقط (RULE-N112).</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvPreview);

        // RULE-N22 — شبكة معاينة للعرض فقط: السلوك تعلنه الشاشة (ApplyGrid لا يلمسه — RULE-N52)
        dgvPreview.ReadOnly = true;
        dgvPreview.AllowUserToAddRows = false;
        dgvPreview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPreview.MultiSelect = false;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }
}
