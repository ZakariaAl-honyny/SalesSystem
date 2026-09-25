using ClosedXML.Excel;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;
using Serilog;
using System.Data;
using SalesSystem.Desktop.Messaging.Messages;

namespace SalesSystem.Desktop.Screens.Products.Forms;

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
    }

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
        
        dgvPreview.DataSource = _dtPreview;
    }

    private void btnSelectFile_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog { Filter = "Excel Files|*.xlsx", Title = "اختر ملف المنتجات" };
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            _ProcessExcelFile(ofd.FileName);
        }
    }

    private void _ProcessExcelFile(string filePath)
    {
        try
        {
            Cursor = Cursors.WaitCursor;
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
            lblCount.Text = $"عدد الأصناف: {_dtPreview.Rows.Count}";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في قراءة ملف Excel");
            MessageBox.Show("حدث خطأ أثناء قراءة الملف. تأكد من أن الملف غير مفتوح في تطبيق آخر.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void _ApplyGridColors()
    {
        foreach (DataGridViewRow row in dgvPreview.Rows)
        {
            var status = row.Cells["Status"].Value?.ToString();
            if (status == "خطأ") row.DefaultCellStyle.BackColor = Color.LightCoral;
            else if (status == "تحذير") row.DefaultCellStyle.BackColor = Color.LightYellow;
            else row.DefaultCellStyle.BackColor = Color.White;
        }
    }

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
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        var validRows = _dtPreview.AsEnumerable().Where(r => r.Field<bool>("IsValid")).ToList();
        if (!validRows.Any())
        {
            MessageBox.Show("لا يوجد أصناف صالحة للحفظ.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            Cursor = Cursors.WaitCursor;
            var productService = _serviceProvider.GetRequiredService<IProductApiService>();
            int successCount = 0;

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
            }

            MessageBox.Show($"تم حفظ {successCount} صنف بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            var eventBus = _serviceProvider.GetRequiredService<IEventBus>();
            eventBus.Publish(new ProductChangedMessage(0));
            this.Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error saving imported products");
            MessageBox.Show("حدث خطأ أثناء الحفظ.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void dgvPreview_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            // Simple validation on edit
            var row = dgvPreview.Rows[e.RowIndex];
            var dr = ((DataRowView)row.DataBoundItem).Row;
            if (!string.IsNullOrWhiteSpace(dr["Name"].ToString()) && !string.IsNullOrWhiteSpace(dr["Barcode"].ToString()))
            {
                dr["Status"] = "مقبول";
                dr["Message"] = "تم التعديل يدوياً";
                dr["IsValid"] = true;
            }
            _ApplyGridColors();
        }
    }

    private void btnDownloadTemplate_Click(object sender, EventArgs e)
    {
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
                MessageBox.Show("تم حفظ النموذج.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error generating template");
            MessageBox.Show("خطأ في حفظ النموذج.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
