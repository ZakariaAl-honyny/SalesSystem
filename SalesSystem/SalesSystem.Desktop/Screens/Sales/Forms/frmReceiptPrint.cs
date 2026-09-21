using System.Drawing.Drawing2D;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// نافذة طباعة وصل مبيعات — معاينة إيصال حراري (80مم) قبل الطباعة.
/// نموذج أولي للواجهة (UI Shape / UI-First) يعمل ببيانات تجريبية داخل الذاكرة
/// بدون أي اعتماد على API أو قاعدة بيانات، ويُعرض كـ modal عبر ShowDialog().
/// </summary>
public partial class frmReceiptPrint : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ═══════════════════════════════════════════════
    // بيانات الوصل التجريبية — Mock (بدون API / قاعدة بيانات)
    // ═══════════════════════════════════════════════
    private sealed class MockReceiptLine
    {
        public required string ItemName { get; init; }
        public required decimal Quantity { get; init; }
        public required decimal UnitPrice { get; init; }
        public decimal LineTotal => Quantity * UnitPrice; // (الكمية × السعر) بدون خصم على البند
    }

    private sealed class MockReceipt
    {
        public required string StoreName { get; init; }
        public required string StoreInfo { get; init; }
        public required string InvoiceNo { get; init; }
        public required DateTime Date { get; init; }
        public required string Customer { get; init; }
        public required List<MockReceiptLine> Lines { get; init; }
        public required decimal Discount { get; init; }   // خصم على الفاتورة
        public required decimal Tax { get; init; }        // ضريبة
        public required decimal PaidAmount { get; init; } // المدفوع

        public decimal SubTotal => Lines.Sum(l => l.LineTotal);
        public decimal NetTotal => SubTotal - Discount + Tax;
        public decimal Remaining => NetTotal - PaidAmount;
    }

    // فاتورة 1004 — عميل نقدي — 3 أصناف — الإجمالي 15,000 ريال يمني
    private readonly MockReceipt _receipt = new()
    {
        StoreName = "مؤسسة النور للتجارة العامة",
        StoreInfo = "صنعاء — شارع الزبيري | هاتف: 01-234567",
        InvoiceNo = "1004",
        Date = new DateTime(2026, 9, 21),
        Customer = "عميل نقدي",
        Discount = 500m,
        Tax = 200m,
        PaidAmount = 15000m,
        Lines = new List<MockReceiptLine>
        {
            new() { ItemName = "رز بسمتي 5 كجم",  Quantity = 2m, UnitPrice = 5000m }, // 10,000
            new() { ItemName = "سكر أبيض 10 كجم", Quantity = 1m, UnitPrice = 3300m }, //  3,300
            new() { ItemName = "شاي أحمر 250 غ",   Quantity = 4m, UnitPrice = 500m  }, //  2,000
        },
    };

    public frmReceiptPrint(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج — الخدمات تُحل عند ربط الواجهة بالخلفية الفعلية.
        _serviceProvider = serviceProvider;
        InitializeComponent();
        _BindReceipt();
    }

    // ═══════════════════════════════════════════════
    // ربط بيانات الوصل (Mock) بعناصر المعاينة
    // ═══════════════════════════════════════════════
    private void _BindReceipt()
    {
        // رأس المتجر + عنوان الوصل
        lblStoreName.Text = _receipt.StoreName;
        lblStoreInfo.Text = _receipt.StoreInfo;

        // حقول المعلومات
        lblInvoiceNoValue.Text = _receipt.InvoiceNo;
        lblDateValue.Text = _receipt.Date.ToString("dd/MM/yyyy");
        lblCustomerValue.Text = _receipt.Customer;

        // جدول الأصناف
        dgvItems.Rows.Clear();
        foreach (var line in _receipt.Lines)
        {
            dgvItems.Rows.Add(
                line.ItemName,
                line.Quantity.ToString("0.##"),
                line.UnitPrice.ToString("N0"),
                line.LineTotal.ToString("N0"));
        }

        // الملخص المالي
        lblSubTotalValue.Text = _receipt.SubTotal.ToString("N0") + " ريال";
        lblDiscountValue.Text = _receipt.Discount.ToString("N0") + " ريال";
        lblTaxValue.Text = _receipt.Tax.ToString("N0") + " ريال";
        lblNetTotalValue.Text = _receipt.NetTotal.ToString("N0") + " ريال";
        lblPaidValue.Text = _receipt.PaidAmount.ToString("N0") + " ريال";
        lblRemainingValue.Text = _receipt.Remaining.ToString("N0") + " ريال";
    }

    // ═══════════════════════════════════════════════
    // أحداث الواجهة
    // ═══════════════════════════════════════════════
    private async void btnPrint_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            // محاكاة إرسال الوصل إلى الطابعة الحرارية (80مم) — UI-first
            await Task.Delay(450);

            MessageBox.Show(
                $"تمت طباعة وصل المبيعات رقم [{_receipt.InvoiceNo}] بنجاح.\n" +
                $"الإجمالي: {_receipt.NetTotal:N0} ريال يمني\n" +
                "(وضع تجريبي — الطباعة الفعلية ستتوفر عند ربط الخلفية).",
                "طباعة وصل مبيعات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception)
        {
            MessageBox.Show("تعذرت الطباعة، يرجى المحاولة مرة أخرى.",
                "خطأ في الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    /// <summary>رسم خط متقطع (فاصل الوصل) بعرض اللوحة.</summary>
    private void pnlDashedSeparator_Paint(object? sender, PaintEventArgs e)
    {
        var panel = (Panel)sender!;
        int y = panel.Height / 2;
        using var pen = new Pen(Color.FromArgb(160, 170, 180), 1.5f) { DashStyle = DashStyle.Dash };
        e.Graphics.DrawLine(pen, 6, y, panel.Width - 6, y);
    }
}