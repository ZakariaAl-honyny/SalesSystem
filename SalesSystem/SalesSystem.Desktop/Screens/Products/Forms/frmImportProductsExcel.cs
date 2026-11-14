using ClosedXML.Excel;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using Serilog;
using System.Data;
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

    public frmImportProductsExcel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        _SetupGrid();

        // RULE-N40…RULE-N47 — كل الستة من KeyboardHelper (RULE-N95)
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46 — الحقل النشط
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45 — أسهم RTL
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnGenerateBarcodes);      // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
            (Keys.F5, () => btnSelectFile.PerformClick()),
            (Keys.F2, () => btnSave.PerformClick()),
            (Keys.Escape, () => btnClose.PerformClick()));

        // RULE-N147 — Enter = الزر الأساسي لوضع الاستيراد الحالي (يُقرأ عند كل ضغطة لا مرة واحدة)
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        _SyncPrimaryAction();

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
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "اسم الصنف", DataPropertyName = "Name", Width = 200 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Barcode", HeaderText = "الباركود", DataPropertyName = "Barcode", Width = 150 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Qty", HeaderText = "رصيد افتتاحي", DataPropertyName = "Qty", Width = 100 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Cost", HeaderText = "سعر التكلفة", DataPropertyName = "Cost", Width = 100 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "سعر البيع", DataPropertyName = "Price", Width = 100 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Unit", HeaderText = "الوحدة", DataPropertyName = "Unit", Width = 80 });
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn { Name = "Message", HeaderText = "ملاحظات", DataPropertyName = "Message", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });

        _dtPreview.Columns.Add("Status", typeof(string));
        _dtPreview.Columns.Add("Name", typeof(string));
        _dtPreview.Columns.Add("Barcode", typeof(string));
        _dtPreview.Columns.Add("Qty", typeof(decimal));
        _dtPreview.Columns.Add("Cost", typeof(decimal));
        _dtPreview.Columns.Add("Price", typeof(decimal));
        _dtPreview.Columns.Add("Unit", typeof(string));
        _dtPreview.Columns.Add("Message", typeof(string));
        _dtPreview.Columns.Add("IsValid", typeof(bool));

        // العرض عبر DataView حتى تعمل التصفية الفورية (RULE-N24 / RULE-N60)
        dgvPreview.DataSource = _dtPreview.DefaultView;
    }

    // ── اختيار الملف والقراءة ──────────────────────────────────────────────────

    private void btnSelectFile_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog { Filter = "Excel Files|*.xlsx", Title = "اختر ملف المنتجات" };
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            lblFilePath.Text = ofd.SafeFileName;
            _ProcessExcelFile(ofd.FileName);
            _SyncPrimaryAction(); // RULE-N147 — بعد ظهور المعاينة يصبح Enter بلا زر أساسي
        }
    }

    private void _ProcessExcelFile(string filePath)
    {
        Cursor = Cursors.WaitCursor;

        try
        {
            _dtPreview.Clear();

            using var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet(1);
            var rows = ws.RangeUsed().RowsUsed().Skip(1); // skip header

            var existingBarcodes = new HashSet<string>(); // Ideally fetched from DB
            var seenBarcodesInFile = new HashSet<string>();

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

                string status = "مقبول";
                string msg = "";
                bool isValid = true;

                if (string.IsNullOrEmpty(name))
                {
                    status = "خطأ";
                    msg = "اسم الصنف مفقود";
                    isValid = false;
                }
                else if (!string.IsNullOrEmpty(barcode) && seenBarcodesInFile.Contains(barcode))
                {
                    status = "تحذير";
                    msg = "الباركود مكرر في هذا الملف";
                    isValid = false; // Need manual fix in grid
                }
                else if (!string.IsNullOrEmpty(barcode) && existingBarcodes.Contains(barcode))
                {
                    status = "تحذير";
                    msg = "الباركود مسجل مسبقاً";
                    isValid = false;
                }

                if (!string.IsNullOrEmpty(barcode))
                    seenBarcodesInFile.Add(barcode);

                _dtPreview.Rows.Add(status, name, barcode, qty, cost, price, unit, msg, isValid);
            }

            _ApplyGridColors();
            _ApplyPreviewFilter(); // يُحدّث الإجماليات وحالة «لا توجد بيانات»
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
                var status = row.Cells["Status"].Value?.ToString();
                if (status == "خطأ") row.DefaultCellStyle.BackColor = Color.LightCoral;
                else if (status == "تحذير") row.DefaultCellStyle.BackColor = Color.LightYellow;
                else row.DefaultCellStyle.BackColor = Color.White;
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
                if (row["Status"].ToString() != "خطأ")
                {
                    row["Status"] = "مقبول";
                    row["Message"] = "تم توليد باركود";
                    row["IsValid"] = true;
                }
            }
        }

        _ApplyGridColors();
        _ApplyPreviewFilter();
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
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
            int successCount = 0;
            int failedCount = 0;

            foreach (var r in validRows)
            {
                var req = new CreateProductRequest(
                    Name: r.Field<string>("Name")!,
                    CategoryId: 1, // Mock
                    Description: "استيراد Excel",
                    ReorderLevel: 10,
                    TrackExpiry: false,
                    Barcode: r.Field<string>("Barcode")
                );

                var res = await productService.CreateAsync(req);
                if (res.IsSuccess)
                {
                    successCount++;
                }
                else
                {
                    failedCount++;
                    Log.Warning("تعذر اعتماد صنف مستورد: {ProductName}", req.Name);
                }
            }

            int skippedCount = _dtPreview.Rows.Count - validRows.Count;
            string message = $"تم اعتماد {successCount} صنفاً بنجاح.";

            if (failedCount > 0)
                message += $"\nتعذّر اعتماد {failedCount} صنفاً — راجع عمود «ملاحظات».";
            if (skippedCount > 0)
                message += $"\n{skippedCount} صفاً لم يُرسل لأنه غير صالح (سبب الرفض في عمود «ملاحظات»).";

            MessageBox.Show(message,
                successCount > 0 ? "نتيجة الاستيراد" : "تعذر الاستيراد",
                MessageBoxButtons.OK,
                successCount > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (successCount > 0)
            {
                var eventBus = _serviceProvider.GetRequiredService<IEventBus>();
                eventBus.Publish(new ProductChangedMessage(0));
                Close();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اعتماد الأصناف المستوردة");
            clsDialogs.Error("تعذر اعتماد الأصناف. يرجى إعادة المحاولة.");
        }
        finally
        {
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
        }
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
        if (!string.IsNullOrWhiteSpace(dr["Name"].ToString()) && !string.IsNullOrWhiteSpace(dr["Barcode"].ToString()))
        {
            dr["Status"] = "مقبول";
            dr["Message"] = "تم التعديل يدوياً";
            dr["IsValid"] = true;
        }

        _ApplyGridColors();
        _UpdateSummary();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
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
