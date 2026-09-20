using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// نافذة إدخال صنف مشتريات (تشغيلة + صلاحية) — واجهة أولية ببيانات تجريبية فقط (Mock).
/// تُعرض عبر ShowDialog وتُرجع قيم الصنف عبر الخصائص العامة عند الموافقة.
/// </summary>
public partial class frmPurchaseItem : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ═══════════════════════════════════════════════════════════
    // بيانات تجريبية (Mock) — وحدات وأصناف مشتريات
    // ═══════════════════════════════════════════════════════════
    private sealed record MockProduct(string Name, string Barcode, string DefaultUnit, bool TrackExpiry);
    private sealed record MockUnit(string Name, decimal ConversionFactor);

    private static readonly MockProduct[] MockProducts =
    {
        new("شاي ليبتون 250غ",      "102030", "كيس",  false),
        new("زيت نباتي 1 لتر",      "102031", "عبوة", false),
        new("أرز بسمتي 5 كغ",       "102032", "كيس",  false),
        new("سكر أبيض 1 كغ",        "102033", "كيس",  false),
        new("دقيق فاخر 2 كغ",       "102034", "كيس",  false),
        new("معجون طماطم 400غ",     "102035", "عبوة", true),
        new("تونا معلبة 185غ",      "102036", "عبوة", true),
        new("حليب مجفف 900غ",       "102037", "عبوة", true),
    };

    private static readonly MockUnit[] MockUnits =
    {
        new("كيس",   1m),
        new("كرتون", 12m),
        new("عبوة",  6m),
    };

    public frmPurchaseItem(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);

        numQuantity.Value = 1;
        LoadMockData();
        CalculateTotal();
    }

    // ═══════════════════════════════════════════════════════════
    // نتائج النافذة — يقرأها النموذج الأب بعد ShowDialog
    // ═══════════════════════════════════════════════════════════
    public string ProductName => SelectedProduct?.Name ?? "";
    public string Barcode => lblBarcodeValue.Text;
    public string UnitName => cmbUnit.SelectedItem?.ToString() ?? "";
    public decimal Quantity => numQuantity.Value;
    public decimal UnitCost => numCost.Value;
    public decimal Discount => numDiscount.Value;
    public string BatchNumber => txtBatch.Text.Trim();
    public bool HasExpiry => chkExpiry.Checked;
    public DateTime? ExpiryDate => chkExpiry.Checked ? dtpExpiry.Value.Date : null;
    public decimal Total => (numQuantity.Value * numCost.Value) - numDiscount.Value;

    // ═══════════════════════════════════════════════════════════
    // تحميل البيانات التجريبية
    // ═══════════════════════════════════════════════════════════
    private void LoadMockData()
    {
        cmbUnit.Items.AddRange(MockUnits.Select(u => u.Name).ToArray());
        cmbUnit.SelectedIndex = 0;

        cmbProduct.Items.AddRange(MockProducts.Select(p => p.Name).ToArray());
        cmbProduct.SelectedIndex = 0;
    }

    private MockProduct? SelectedProduct
        => cmbProduct.SelectedIndex >= 0 ? MockProducts[cmbProduct.SelectedIndex] : null;

    // ═══════════════════════════════════════════════════════════
    // أحداث الواجهة
    // ═══════════════════════════════════════════════════════════
    private void cmbProduct_SelectedIndexChanged(object? sender, EventArgs e)
    {
        var product = SelectedProduct;
        if (product == null) return;

        lblBarcodeValue.Text = product.Barcode;

        int unitIndex = Array.FindIndex(MockUnits, u => u.Name == product.DefaultUnit);
        if (unitIndex >= 0) cmbUnit.SelectedIndex = unitIndex;

        // الأصناف القابلة للتلف تتفعّل تلقائياً حقل تاريخ الصلاحية
        chkExpiry.Checked = product.TrackExpiry;
    }

    private void cmbUnit_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbUnit.SelectedIndex >= 0)
            lblConversionFactor.Text = MockUnits[cmbUnit.SelectedIndex].ConversionFactor.ToString("0.##");
    }

    private void chkExpiry_CheckedChanged(object? sender, EventArgs e)
    {
        dtpExpiry.Enabled = chkExpiry.Checked;
    }

    private void num_ValueChanged(object? sender, EventArgs e) => CalculateTotal();

    private void CalculateTotal()
    {
        lblTotalValue.Text = Total.ToString("N2");
    }

    // ═══════════════════════════════════════════════════════════
    // التحقق من صحة البيانات
    // ═══════════════════════════════════════════════════════════
    private bool ValidateForm()
    {
        bool isValid = true;

        if (numQuantity.Value <= 0)
        {
            errorProvider.SetError(numQuantity, "الكمية يجب أن تكون أكبر من صفر.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numQuantity, "");
        }

        if (numCost.Value < 0)
        {
            errorProvider.SetError(numCost, "التكلفة يجب أن تكون أكبر من أو تساوي صفر.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numCost, "");
        }

        if (chkExpiry.Checked && dtpExpiry.Value.Date < DateTime.Today)
        {
            errorProvider.SetError(dtpExpiry, "تاريخ الصلاحية يجب ألا يكون في الماضي.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(dtpExpiry, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    private void btnOk_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!ValidateForm()) return;
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            // واجهة تجريبية: لا يوجد اتصال بالخادم بعد — يظهر رسالة عامة فقط
            MessageBox.Show("حدث خطأ غير متوقع. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}