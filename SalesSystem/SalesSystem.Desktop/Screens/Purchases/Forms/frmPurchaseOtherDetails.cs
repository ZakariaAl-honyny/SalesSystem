using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// حوار «تفاصيل وتكاليف إضافية» للمشتريات — واجهة أولية ببيانات وهمية في الذاكرة (وضع التجربة).
/// يُعرض عبر <c>ShowDialog</c> ويُعيد التكاليف الإضافية (شحن / جمارك / أخرى + بيان) والملاحظات.
/// <para>RULE-N40…RULE-N47 / RULE-N50…RULE-N54 / RULE-N97 — التنقل والثيم والتفقيط الحيّ.</para>
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

        // RULE-N40…RULE-N47 + RULE-N95 — الستة من KeyboardHelper في كل حوار إدخال.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnOk, btnCancel);                 // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                   // RULE-N47
            (Keys.Escape, () => DialogResult = DialogResult.Cancel));

        // RULE-N97 — التفقيط الحيّ على التكاليف الثلاث.
        LiveTafqeetHelper.Attach(tipTafqeet,
            (numShipping, "تكلفة الشحن", LiveTafqeetHelper.ReadMode.Money),
            (numCustoms, "رسوم الجمارك", LiveTafqeetHelper.ReadMode.Money),
            (numOther, "تكاليف أخرى", LiveTafqeetHelper.ReadMode.Money));

        ApplyUiTheme(); // RULE-N50

        LoadMockData();
        Recalculate();
    }

    // ═══════════════════════════════════════════════════════════
    // الثيم (RULE-N50…RULE-N54)
    // ═══════════════════════════════════════════════════════════

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply</c> آخر سطر (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);

        ThemeHelper.ApplyButtonStyle(btnOk, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnCancel, ThemeHelper.ButtonType.Secondary);

        // ThemeHelper يفرض Segoe UI ويوسّع الأزرار ⇒ نثبّت Tahoma والأحجام (RULE-N21).
        foreach (var btn in new[] { btnOk, btnCancel })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        }

        btnOk.Size = new Size(140, 40);
        btnOk.ForeColor = Color.White;
        btnOk.Margin = new Padding(0, 0, 8, 0);
        btnCancel.Size = new Size(92, 40);
        btnCancel.Margin = new Padding(0);

        toolTip.SetToolTip(btnOk, "موافق — اعتماد التكاليف الإضافية (Enter)");
        toolTip.SetToolTip(btnCancel, "إلغاء وإغلاق الحوار بلا حفظ (Esc)");
        toolTip.SetToolTip(numShipping, "تكلفة الشحن المضافة على الفاتورة");
        toolTip.SetToolTip(numCustoms, "رسوم الجمارك والتخليص");
        toolTip.SetToolTip(numOther, "أي تكاليف أخرى — مع بيانها في الحقل المجاور");

        GreenThemeHelper.ApplyHeader(lblTitle);
        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ═══════════════════════════════════════════════════════════
    // نتائج الحوار — يقرأها النموذج الأب بعد ShowDialog
    // ═══════════════════════════════════════════════════════════
    public decimal ShippingCost => numShipping.Value;
    public decimal CustomsCost => numCustoms.Value;
    public decimal OtherCost => numOther.Value;
    public string OtherStatement => txtStatement.Text.Trim();
    public string Notes => txtNotes.Text.Trim();

    /// <summary>مجموع التكاليف الإضافية — منطق الحساب محفوظ كما هو.</summary>
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
        lblTotalValue.Text = clsFormat.Money(TotalAdditionalCosts);
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
                clsDialogs.Warning("لا يمكن أن تكون التكاليف الإضافية قيمة سالبة.");
                return;
            }

            // في النسخة النهائية تُعاد القيم إلى فاتورة الشراء عبر خصائص الحوار
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "خطأ في حوار التكاليف الإضافية للمشتريات");
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
