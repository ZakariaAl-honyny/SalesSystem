// -----------------------------------------------------------------------------
// ReportNavigationHelper.cs — شريط أزرار التنقل بين تقارير المجموعة الواحدة
//
// المشكلة: أُلغيت تبويبات التقارير (RULE-N70 / RULE-N77) وأصبح كل تقرير نموذجاً
// مستقلاً، فلم يبقَ للمستخدم سبيل للانتقال من تقرير إلى آخر في التصنيف نفسه إلا
// العودة إلى القائمة الرئيسية. هذه الأداة تُعيد التنقل داخل النموذج نفسه بأزرار
// (RULE-N78) بدل TabControl المحذوف.
//
// الاستهلاك — سطر واحد كآخر سطر في مُنشئ التقرير بعد InitializeComponent():
//     ReportNavigationHelper.Attach(this, _serviceProvider);
//
// ما تفعله الأداة (وما لا تفعله):
//   • تُضيف pnlReportNav بارتفاع 58 أسفل النموذج (Dock = Bottom) وتُكبّر ارتفاع
//     النموذج بمقداره أولاً حتى لا يُغطّى أي عنصر قائم.
//   • عند تغيير ارتفاع النموذج يُعيد WinForms حساب مواضع العناصر المرساة (Anchor
//     Bottom) فتهبط إلى منطقة الشريط الجديد؛ لذلك تُحفظ مواضع الأبناء غير المرساة
//     (Dock = None) وتُعاد كما هي بعد التكبير — فتبقى الشاشة بتخطيطها المطلق
//     الأصلي ولا يُغطّى شيء (RULE-N52 لا يُمسّ هنا).
//   • أزرار التقارير الستة والثلاثون مأخوذة من كتالوج ثابت؛ عنوان كل زر هو الاسم
//     العربي الحقيقي الذي يُصرّح به التقرير في IExportableReport.GetReportName().
//   • زر التقرير الحالي معطّل وبلون تحديد الثيم + تلميح «أنت هنا».
//   • النقر يفتح التقرير المطلوب من حاوية DI (RULE-N02) ثم يُغلق التقرير الحالي.
//   • لا تُنشئ أي تقرير بـ new، ولا تلمس منطق أي تقرير، ولا تُطلق استثناءً.
//   • آمنة للاستدعاء المتكرر (idempotent): وجود pnlReportNav يمنع التركيب المزدوج.
//
// كل الألوان من GreenThemeHelper — لا قيمة لون خام هنا (RULE-N54 / RULE-N112).
// -----------------------------------------------------------------------------

using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

using SalesSystem.Desktop.Screens.Reports.Forms;

namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// شريط أزرار التنقل بين تقارير المجموعة الواحدة، يُثبَّت أسفل نموذج التقرير
/// ويحلّ محل التبويبات المحذوفة (RULE-N78). يُستهلك بسطر واحد لكل تقرير (RULE-N112).
/// </summary>
public static class ReportNavigationHelper
{
    /// <summary>اسم اللوحة — يُستخدم كعلامة «مُركَّب مسبقاً».</summary>
    private const string NavPanelName = "pnlReportNav";

    /// <summary>اسم حاوية الأزرار.</summary>
    private const string NavFlowName = "flpReportNav";

    /// <summary>اسم تسمية الشريط.</summary>
    private const string NavCaptionName = "lblNavCaption";

    /// <summary>ارتفاع الشريط بالبكسل.</summary>
    private const int NavPanelHeight = 58;

    /// <summary>ارتفاع زر التقرير بالبكسل.</summary>
    private const int NavButtonHeight = 40;

    /// <summary>عرض زر التقرير بالبكسل.</summary>
    private const int NavButtonWidth = 150;

    /// <summary>عرض تسمية الشريط بالبكسل.</summary>
    private const int NavCaptionWidth = 150;

    /// <summary>نص تسمية الشريط.</summary>
    private const string NavCaptionText = "تقارير هذه المجموعة:";

    /// <summary>تلميح الزر الحالي.</summary>
    private const string CurrentReportTip = "أنت هنا";

    /// <summary>رسالة الخطأ الموحّدة عند تعذّر فتح تقرير.</summary>
    private const string OpenFailedMessage = "تعذّر فتح التقرير المطلوب.";

    /// <summary>
    /// تلميح واحد مشترك لكل أزرار التنقل — يبقى حياً ما دام التطبيق حياً
    /// (ToolTip ليس عنصراً من Controls فلا يمكن أن يملكه النموذج).
    /// </summary>
    private static readonly ToolTip NavToolTip = new();

    /// <summary>تقرير واحد: عنوانه العربي الحقيقي + نوع نموذجه.</summary>
    private sealed record ReportEntry(string Title, Type FormType);

    /// <summary>تصنيف ثابت: عنوان المجموعة + تقاريره بترتيب القائمة.</summary>
    private sealed record ReportCategory(string Caption, ReportEntry[] Reports);

    /// <summary>
    /// كتالوج التقارير الستة (31 تقريراً). العناوين منقولة حرفياً من
    /// <c>GetReportName()</c> في كل نموذج — تُحدَّث هنا إن تغيّر عنوان تقرير.
    /// </summary>
    private static readonly ReportCategory[] Catalog =
    {
        new("تقارير المبيعات", new[]
        {
            new ReportEntry("تقرير المبيعات", typeof(frmSalesReport)),
            new ReportEntry("المبيعات اليومية", typeof(frmDailySalesReport)),
            new ReportEntry("المبيعات حسب العميل", typeof(frmSalesByCustomerReport)),
            new ReportEntry("المبيعات حسب المنتج", typeof(frmSalesByProductReport)),
            new ReportEntry("المبيعات حسب التصنيف", typeof(frmSalesByCategoryReport)),
            new ReportEntry("الربح حسب العميل", typeof(frmProfitByCustomerReport)),
            new ReportEntry("أرباح الأصناف", typeof(frmProductProfitabilityReport)),
            new ReportEntry("تقرير المرتجعات", typeof(frmReturnsReport)),
        }),

        new("تقارير المشتريات", new[]
        {
            new ReportEntry("تقرير المشتريات", typeof(frmPurchasesReport)),
            new ReportEntry("المشتريات حسب المورد", typeof(frmPurchasesBySupplierReport)),
            new ReportEntry("المشتريات حسب المنتج", typeof(frmPurchasesByProductReport)),
        }),

        new("تقارير المخزون", new[]
        {
            new ReportEntry("تقرير المخزون", typeof(frmInventoryReport)),
            new ReportEntry("رصيد المخزون", typeof(frmStockBalanceReport)),
            new ReportEntry("حركات المستودع", typeof(frmWarehouseMovementReport)),
            new ReportEntry("دفتر الأستاذ التفصيلي للمخزون", typeof(frmDetailedStockLedgerReport)),
            new ReportEntry("حركة منتج", typeof(frmProductMovementReport)),
            new ReportEntry("تنبيه المخزون المنخفض", typeof(frmLowStockReport)),
            new ReportEntry("المنتجات منتهية الصلاحية", typeof(frmExpiredProductsReport)),
        }),

        new("التقارير المحاسبية والمالية", new[]
        {
            new ReportEntry("ميزان المراجعة", typeof(frmTrialBalanceReport)),
            new ReportEntry("دفتر الأستاذ العام", typeof(frmGeneralLedgerReport)),
            new ReportEntry("قائمة الدخل", typeof(frmIncomeStatementReport)),
            new ReportEntry("الميزانية العمومية", typeof(frmBalanceSheetReport)),
            new ReportEntry("تقرير التدفقات النقدية", typeof(frmCashFlowReport)),
            new ReportEntry("رأس المال العامل", typeof(frmWorkingCapitalReport)),
            new ReportEntry("تقرير ضريبة القيمة المضافة", typeof(frmVatReport)),
            new ReportEntry("كشف حساب الطرف", typeof(frmAccountStatementReport)),
        }),

        new("تقارير العملاء والموردين", new[]
        {
            new ReportEntry("كشف حساب عملاء", typeof(frmCustomerBalanceReport)),
            new ReportEntry("كشف حساب موردين", typeof(frmSupplierBalanceReport)),
            new ReportEntry("تقرير الأعمار المدينة", typeof(frmAgingReport)),
        }),

        new("تقارير الصندوق", new[]
        {
            new ReportEntry("ملخص الصناديق النقدية", typeof(frmCashBoxSummaryReport)),
            new ReportEntry("التقرير اليومي للصندوق", typeof(frmDailyClosureReport)),
        }),
    };

    // ═════════════════════════════════════════════════════════════════
    //  Attach — المدخل الوحيد
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يُثبّت شريط التنقل أسفل نموذج التقرير مرة واحدة. إن لم يكن النموذج مدرجاً
    /// في الكتالوج يُسجَّل تحذير ولا يُضاف شيء (لا استثناء، لا تغيير في الشاشة).
    /// </summary>
    /// <param name="form">نموذج التقرير الحالي — قد يكون null.</param>
    /// <param name="serviceProvider">حاوية DI لفتح التقرير التالي — قد تكون null.</param>
    public static void Attach(Form? form, IServiceProvider? serviceProvider)
    {
        if (form is null || form.IsDisposed) return;

        try
        {
            // (1) مُركَّب مسبقاً ⇒ لا شيء يُفعل (idempotent).
            if (FindNavPanel(form) is not null) return;

            var category = FindCategory(form.GetType());
            if (category is null)
            {
                Log.Warning(
                    "التقرير {Report} غير مدرج في كتالوج التنقل — لم يُركَّب شريط الأزرار",
                    form.GetType().Name);
                return;
            }

            // نبني الشريط كاملاً قبل تغيير مقاس النموذج: أي فشل في البناء يترك الشاشة كما هي.
            var panel = BuildPanel(form, category, serviceProvider);

            // (4) نُكبّر النموذج بمقدار ارتفاع الشريط ثم نُثبّته في الأسفل.
            ReserveNavSpace(form, NavPanelHeight);
            form.Controls.Add(panel);

            // (7) الثيم الأخضر على الشريط من مصدره الوحيد (RULE-N54 / RULE-N96).
            GreenThemeHelper.Apply(panel);
            panel.BackColor = GreenThemeHelper.BarBackground;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تركيب شريط التنقل بين التقارير في {Report}", form.GetType().Name);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  بناء الشريط
    // ═════════════════════════════════════════════════════════════════

    /// <summary>ينشئ اللوحة وتسميتها وحاوية الأزرار ويملأ أزرار المجموعة.</summary>
    private static Panel BuildPanel(Form host, ReportCategory category, IServiceProvider? serviceProvider)
    {
        var hostType = host.GetType();

        var panel = new Panel
        {
            Name = NavPanelName,
            Dock = DockStyle.Bottom,
            Height = NavPanelHeight,
            RightToLeft = RightToLeft.Yes
        };

        var caption = new Label
        {
            Name = NavCaptionName,
            Dock = DockStyle.Right,
            Width = NavCaptionWidth,
            Text = NavCaptionText,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Tahoma", 9F, FontStyle.Bold),
            ForeColor = GreenThemeHelper.TextStrong
        };

        NavToolTip.SetToolTip(caption, category.Caption);   // اسم المجموعة كتلميح على تسمية الشريط

        var flow = new FlowLayoutPanel
        {
            Name = NavFlowName,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            AutoScroll = true,
            Margin = new Padding(0)
        };

        foreach (var entry in category.Reports)
        {
            flow.Controls.Add(BuildButton(entry, host, hostType, serviceProvider));
        }

        panel.Controls.Add(flow);
        panel.Controls.Add(caption);   // يُرسى أولاً فيحتفظ بعرضه ويأخذ التدفّق الباقي
        return panel;
    }

    /// <summary>ينشئ زر تقرير واحداً: الحالي معطّل ومميّز، وغيره يفتح تقريره.</summary>
    private static Button BuildButton(ReportEntry entry, Form host, Type hostType, IServiceProvider? serviceProvider)
    {
        var button = new Button
        {
            Name = "btnNav" + entry.FormType.Name,
            Text = entry.Title,
            Size = new Size(NavButtonWidth, NavButtonHeight),
            Margin = new Padding(4),
            FlatStyle = FlatStyle.Popup,
            Font = new Font("Tahoma", 9F, FontStyle.Bold),
            RightToLeft = RightToLeft.Yes,
            UseVisualStyleBackColor = false
        };

        if (entry.FormType == hostType)
        {
            // (5) التقرير الحالي: معطّل + لون التحديد من الثيم + تلميح «أنت هنا».
            button.Enabled = false;
            button.BackColor = GreenThemeHelper.SelectionBack;
            button.ForeColor = GreenThemeHelper.TextStrong;
            NavToolTip.SetToolTip(button, CurrentReportTip);
            return button;
        }

        NavToolTip.SetToolTip(button, "فتح: " + entry.Title);

        var target = entry.FormType;
        button.Click += (s, e) => OpenReport(host, target, serviceProvider);
        return button;
    }

    // ═════════════════════════════════════════════════════════════════
    //  الانتقال إلى تقرير آخر
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يفتح التقرير المطلوب من حاوية DI (RULE-N02) ويُخفي الحالي أثناء العرض ثم
    /// يُغلقه — فيبقى المستخدم في تصنيفه بدل العودة إلى الشاشة الرئيسية (RULE-N78).
    /// </summary>
    private static void OpenReport(Form host, Type reportType, IServiceProvider? serviceProvider)
    {
        try
        {
            if (serviceProvider is null)
            {
                Log.Warning("لا توجد حاوية DI لفتح التقرير {Report}", reportType.Name);
                ShowOpenFailed();
                return;
            }

            // GetService لا GetRequiredService: تقرير غير مسجَّل (أثناء التجهيز) يجب
            // أن يُظهر رسالة عربية لا أن يُطلق استثناءً.
            if (serviceProvider.GetService(reportType) is not Form next)
            {
                Log.Warning("التقرير {Report} غير مسجَّل في حاوية DI — تعذّر فتحه", reportType.Name);
                ShowOpenFailed();
                return;
            }

            host.Hide();
            try
            {
                next.ShowDialog();
            }
            finally
            {
                host.Close();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر فتح تقرير من شريط التنقل: {Report}", reportType.Name);
            ShowOpenFailed();
        }
    }

    /// <summary>رسالة الخطأ الموحّدة — بلا تفاصيل استثناء للمستخدم (RULE-N29).</summary>
    private static void ShowOpenFailed()
        => MessageBox.Show(OpenFailedMessage, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

    // ═════════════════════════════════════════════════════════════════
    //  حجز المساحة أسفل النموذج
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يُكبّر ارتفاع النموذج بمقدار <paramref name="height"/> ثم يُعيد كل عنصر غير
    /// مرساة (Dock = None) إلى موضعه السابق، لأن WinForms يُعيد حساب مواضع العناصر
    /// المرساة على الحافة السفلية عند تغيير ارتفاع النموذج فتهبط إلى منطقة الشريط
    /// الجديد. العناصر المرساة (Dock) تتكفّل بنفسها.
    /// </summary>
    private static void ReserveNavSpace(Form form, int height)
    {
        var snapshot = new Dictionary<Control, Rectangle>();

        foreach (Control child in form.Controls)
        {
            if (child.Dock == DockStyle.None)
                snapshot[child] = child.Bounds;
        }

        form.ClientSize = new Size(form.ClientSize.Width, form.ClientSize.Height + height);

        foreach (var pair in snapshot)
        {
            var control = pair.Key;
            if (control.IsDisposed || control.Dock != DockStyle.None) continue;
            if (control.Bounds != pair.Value) control.Bounds = pair.Value;
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  مساعدات البحث
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يبحث عن الشريط بين أبناء النموذج المباشرين.</summary>
    private static Control? FindNavPanel(Form form)
    {
        foreach (Control child in form.Controls)
        {
            if (string.Equals(child.Name, NavPanelName, StringComparison.OrdinalIgnoreCase))
                return child;
        }

        return null;
    }

    /// <summary>يُرجع تصنيف النموذج في الكتالوج، أو null إن لم يكن مدرجاً.</summary>
    private static ReportCategory? FindCategory(Type formType)
    {
        foreach (var category in Catalog)
        {
            foreach (var entry in category.Reports)
            {
                if (entry.FormType == formType) return category;
            }
        }

        return null;
    }
}
