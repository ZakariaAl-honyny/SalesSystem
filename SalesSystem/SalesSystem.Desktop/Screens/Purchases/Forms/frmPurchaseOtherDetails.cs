using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// نافذة "تفاصيل وتكاليف إضافية" لمشتريات — نموذج واجهة (UI Shape) يعمل ببيانات
/// وهمية في الذاكرة فقط دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// تُعرض عبر ShowDialog وتُعيد قيم التكاليف الإضافية (الشحن / الجمارك / أخرى + بيان)
/// والملاحظات إلى النموذج الأب عبر خصائص عامة بعد الموافقة.
/// </summary>
public partial class frmPurchaseOtherDetails : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ═══════════════════════════════════════════════════════════
    // بيانات تجريبية (Mock) — تكاليف إضافية افتراضية بالريال اليمني
    // (قيم decimal داخل الذاكرة فقط — لا يوجد اتصال بقاعدة البيانات)
    // ═══════════════════════════════════════════════════════════
    private const decimal MockShippingCost = 1500.00m;   // تكلفة الشحن
    private const decimal MockCustomsCost  = 800.00m;    // رسوم الجمارك
    private const decimal MockOtherCost    = 250.00m;    // تكاليف أخرى (تغليف ونقل داخلي)
    private const string  MockOtherStatement = "تغليف ونقل داخلي";
    private const string  MockNotes        = "فاتورة استيراد مواد غذائية — الدفعة الأولى.";

    public frmPurchaseOtherDetails(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);

        LoadMockData();
        Recalculate();
    }

    // ═══════════════════════════════════════════════════════════
    // نتائج النافذة — يقرأها النموذج الأب بعد ShowDialog
    // ═══════════════════════════════════════════════════════════
    public decimal ShippingCost => numShipping.Value;
    public decimal CustomsCost => numCustoms.Value;
    public decimal OtherCost => numOther.Value;
    public string OtherStatement => txtStatement.Text.Trim();
    public string Notes => txtNotes.Text.Trim();
    public decimal TotalAdditionalCosts => numShipping.Value + numCustoms.Value + numOther.Value;

    // ═══════════════════════════════════════════════════════════
    // تحميل البيانات التجريبية (Mock)
    // ═══════════════════════════════════════════════════════════
    private void LoadMockData()
    {
        numShipping.Value = MockShippingCost;
        numCustoms.Value = MockCustomsCost;
        numOther.Value = MockOtherCost;
        txtStatement.Text = MockOtherStatement;
        txtNotes.Text = MockNotes;
    }

    // ═══════════════════════════════════════════════════════════
    // حساب فوري — مجموع التكاليف الإضافية الثلاثة
    // ═══════════════════════════════════════════════════════════
    private void numCost_ValueChanged(object? sender, EventArgs e) => Recalculate();

    private void Recalculate()
    {
        lblTotalValue.Text = TotalAdditionalCosts.ToString("N2");
    }

    // ═══════════════════════════════════════════════════════════
    // الإجراءات — موافق / إلغاء
    // ═══════════════════════════════════════════════════════════
    private void btnOk_Click(object? sender, EventArgs e)
    {
        try
        {
            // حقول الإدخال محصورة بين 0 والحد الأقصى — تحقق احتياطي فقط
            if (TotalAdditionalCosts < 0)
            {
                MessageBox.Show("لا يمكن أن تكون التكاليف الإضافية قيمة سالبة.", "تحقق من القيم",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // في النسخة النهائية تُعاد القيم إلى فاتورة الشراء عبر خصائص النافذة
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception)
        {
            // واجهة تجريبية: لا يوجد اتصال بالخادم بعد — رسالة عامة فقط
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