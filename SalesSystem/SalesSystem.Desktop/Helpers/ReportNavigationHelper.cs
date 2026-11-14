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
//   • إن أعلن مصمّم التقرير الشريط مسبقاً (pnlReportNav + flpReportNav +
//     btnNavAllReports + lblNavCaption — RULE-N78a / RULE-N146) فإن الأداة **تتبنّاه**
//     كما هو: تملأ حاوية أزراره بأزرار تصنيفه، وتوصّل زر «كل التقارير»، وتضع تلميح
//     التسمية — بلا إنشاء لوحة، وبلا تغيير مقاس النموذج، وبلا إعادة تلوين (RULE-N145 /
//     RULE-N142)، فيتساوى المصمّم والتشغيل في اللون والموضع.
//   • وإن لم يُعلنه (تقرير لم يُرحَّل بعد) تُضيفه وقت التشغيل: pnlReportNav بارتفاع 58
//     أسفل النموذج (Dock = Bottom) وتُكبّر ارتفاع النموذج بمقداره أولاً حتى لا يُغطّى
//     أي عنصر قائم.
//   • عند تغيير ارتفاع النموذج يُعيد WinForms حساب مواضع العناصر المرساة (Anchor
//     Bottom) فتهبط إلى منطقة الشريط الجديد؛ لذلك تُحفظ مواضع الأبناء غير المرساة
//     (Dock = None) وتُعاد كما هي بعد التكبير — فتبقى الشاشة بتخطيطها المطلق
//     الأصلي ولا يُغطّى شيء (RULE-N52 لا يُمسّ هنا).
//   • أزرار التقارير الستة والثلاثون مأخوذة من كتالوج ثابت؛ عنوان كل زر هو الاسم
//     العربي الحقيقي الذي يُصرّح به التقرير في IExportableReport.GetReportName().
//   • زر التقرير الحالي معطّل وبلون تحديد الثيم + تلميح «أنت هنا».
//   • زر «كل التقارير» في الطرف الآخر من الشريط يفتح قائمة مُصنَّفة بالتصنيفات
//     الستة كلها (31 تقريراً) — فيصل المستخدم من أي تقرير إلى أي تقرير آخر في
//     نقرتين بلا العودة إلى القائمة الرئيسية، وتُستثنى منه شاشة التقرير الحالية.
//   • النقر يفتح التقرير المطلوب من حاوية DI (RULE-N02) في **نافذة جديدة** ويُبقي
//     التقرير الحالي مفتوحاً كما هو — بنمط clsGlobal._OpenForm نفسه الذي تفتح به
//     القائمة الرئيسية كل تقاريرها، فيتطابق سلوك الشريط مع سلوك القائمة (RULE-N78).
//   • إن كان التقرير المطلوب مفتوحاً مسبقاً في نافذة أخرى تُستعاد نافذته وتُنشَّط
//     بدل إنشاء نسخة مكرّرة.
//   • لا تُنشئ أي تقرير بـ new، ولا تلمس منطق أي تقرير، ولا تُطلق استثناءً.
//   • آمنة للاستدعاء المتكرر (idempotent): وجود pnlReportNav يمنع التركيب المزدوج.
//
// كل الألوان من GreenThemeHelper — لا قيمة لون خام هنا (RULE-N54 / RULE-N112).
// -----------------------------------------------------------------------------

using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

using SalesSystem.Desktop.GlobalClasses;
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

    /// <summary>اسم زر «كل التقارير» — يُستخدم للتحقق من عدم التكرار.</summary>
    private const string AllReportsButtonName = "btnNavAllReports";

    /// <summary>اسم قائمة «كل التقارير» المنسدلة.</summary>
    private const string AllReportsMenuName = "cmsAllReports";

    /// <summary>نص زر «كل التقارير» — قصير وعربي.</summary>
    private const string AllReportsText = "كل التقارير ▾";

    /// <summary>تلميح زر «كل التقارير».</summary>
    private const string AllReportsTip = "الانتقال إلى أي تقرير آخر";

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
    /// يُثبّت شريط التنقل أسفل نموذج التقرير مرة واحدة. إن كان الشريط مُعلَناً في
    /// المصمّم (RULE-N78a / RULE-N146) يُتبنّى كما هو؛ وإن لم يكن يُبنى وقت التشغيل
    /// (احتياطياً). وإن لم يكن النموذج مدرجاً في الكتالوج يُسجَّل تحذير ولا يُضاف شيء
    /// (لا استثناء، لا تغيير في الشاشة).
    /// </summary>
    /// <param name="form">نموذج التقرير الحالي — قد يكون null.</param>
    /// <param name="serviceProvider">حاوية DI لفتح التقرير التالي — قد تكون null.</param>
    public static void Attach(Form? form, IServiceProvider? serviceProvider)
    {
        if (form is null || form.IsDisposed) return;

        try
        {
            // (1) التصنيف أولاً: تقرير غير مدرج في الكتالوج ⇒ تحذير ولا يُضاف شيء.
            var category = FindCategory(form.GetType());
            if (category is null)
            {
                Log.Warning(
                    "التقرير {Report} غير مدرج في كتالوج التنقل — لم يُركَّب شريط الأزرار",
                    form.GetType().Name);
                return;
            }

            // (2) الشريط مُعلَن في المصمّم (RULE-N78a / RULE-N146) ⇒ يُتبنّى كما هو:
            //     لا BuildPanel، لا ReserveNavSpace، لا إضافة إلى Form.Controls، ولا
            //     إعادة تلوين — ألوانه وخطوطه مؤلَّفة في *.Designer.cs (RULE-N142 /
            //     RULE-N145)، وإعادة طلائه تنقض «المصمّم = التشغيل».
            var declaredNav = FindNavPanel(form);
            if (declaredNav is Panel declaredPanel)
            {
                AdoptDeclaredNavPanel(form, declaredPanel, category, serviceProvider);
                return;
            }

            // (3) عنصر باسم الشريط ليس لوحة (تركيب سابق بشكل غير متوقع) ⇒ لا تكرار.
            if (declaredNav is not null) return;

            // (4) المسار الاحتياطي: تقرير لم يُرحَّل بعد — بناء الشريط وقت التشغيل كما كان.
            //     نبني الشريط كاملاً قبل تغيير مقاس النموذج: أي فشل في البناء يترك الشاشة كما هي.
            var panel = BuildPanel(form, category, serviceProvider);

            // (5) نُكبّر النموذج بمقدار ارتفاع الشريط ثم نُثبّته في الأسفل.
            ReserveNavSpace(form, NavPanelHeight);
            form.Controls.Add(panel);

            // (6) الثيم الأخضر على الشريط من مصدره الوحيد (RULE-N54 / RULE-N96).
            GreenThemeHelper.Apply(panel);
            panel.BackColor = GreenThemeHelper.BarBackground;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تركيب شريط التنقل بين التقارير في {Report}", form.GetType().Name);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  تبنّي الشريط المُعلَن في المصمّم (RULE-N78a / RULE-N145 / RULE-N146)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يتبنّى شريط التنقل الذي أعلنه مصمّم التقرير (RULE-N146) بدل إنشائه: يملأ
    /// <c>flpReportNav</c> بأزرار تصنيفه، ويوصّل <c>btnNavAllReports</c> المعلَن بقائمة
    /// «كل التقارير»، ويضع تلميح اسم المجموعة على <c>lblNavCaption</c>. اللوحة نفسها
    /// ومقاس النموذج ونصوص المصمّم ومقاساته وألوانه لا تُمسّ إطلاقاً
    /// (RULE-N142 / RULE-N145) — فالمصمّم هو المرجع البصري.
    /// <para>آمنة للاستدعاء المتكرر: وجود أي زر تصنيف مسبقاً في الحاوية يعني انتهاء
    /// العمل، فلا تتضاعف الأزرار. ولا تُطلق استثناءً: الخطأ يُسجَّل ويبقى الشريط
    /// كما أعلنه المصمّم.</para>
    /// </summary>
    /// <param name="form">نموذج التقرير الحالي.</param>
    /// <param name="panel">اللوحة <c>pnlReportNav</c> المُعلَنة في المصمّم.</param>
    /// <param name="category">تصنيف التقرير في الكتالوج (أزراره وتلميح تسميته).</param>
    /// <param name="serviceProvider">حاوية DI لفتح التقرير التالي — قد تكون null.</param>
    private static void AdoptDeclaredNavPanel(
        Form form, Panel panel, ReportCategory category, IServiceProvider? serviceProvider)
    {
        if (panel.IsDisposed) return;

        try
        {
            var hostType = form.GetType();

            // (1) حاوية الأزرار المُعلَنة — بحث متداخل (قد تُعلَن داخل حاوية وسيطة).
            var declaredFlow = FindControlByName(panel, NavFlowName);
            if (declaredFlow is not null && declaredFlow is not FlowLayoutPanel)
            {
                Log.Warning(
                    "حاوية أزرار التنقل في {Report} ليست FlowLayoutPanel — لم يُركَّب شريط التنقل",
                    hostType.Name);
                return;
            }

            var flow = declaredFlow as FlowLayoutPanel ?? CreateDeclaredNavFlow(panel);

            // (2) idempotent: استدعاء Attach ثانٍ لا يُضيف زراً واحداً.
            if (HasCategoryNavButtons(flow)) return;

            // (3) زر «كل التقارير» المعلَن — يبقى **آخر** أبناء الحاوية كما في المصمّم.
            var allReportsButton = FindDirectChild(flow, AllReportsButtonName) as Button;

            // (4) أزرار التصنيف: نفس بناء المسار الاحتياطي حرفياً (BuildButton) — لا تكرار لمنطقه.
            foreach (var entry in category.Reports)
            {
                var button = BuildButton(entry, form, hostType, serviceProvider);
                flow.Controls.Add(button);

                // (5) تُدرَج **قبل** زر «كل التقارير»: مع RightToLeft يبقى الزر الأخير في
                //     أقصى اليسار — وهو موضعه في المصمّم (RULE-N146)، فلا يقفز الزر.
                if (allReportsButton is not null)
                    flow.Controls.SetChildIndex(button, flow.Controls.GetChildIndex(allReportsButton));
            }

            // (6) توصيل زر «كل التقارير»: قائمته ونقرته وتلميحه فقط — بلا تغيير في
            //     Text/Size/Font/الألوان (مؤلَّفة في المصمّم).
            if (allReportsButton is not null)
            {
                AdoptAllReportsButton(allReportsButton, form, hostType, serviceProvider);
            }
            else
            {
                // بديل احتياطي موثَّق: الشريط معلَن بلا زر «كل التقارير» ⇒ يُنشأ ويُضاف أخيراً.
                flow.Controls.Add(BuildAllReportsButton(form, hostType, serviceProvider));
            }

            // (7) تلميح التسمية: اسم المجموعة — بلا تغيير في نصها أو مقاسها أو لونها.
            if (FindControlByName(panel, NavCaptionName) is Control caption)
                NavToolTip.SetToolTip(caption, category.Caption);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تبنّي شريط التنقل المُعلَن في {Report}", form.GetType().Name);
        }
    }

    /// <summary>
    /// يوصّل زر «كل التقارير» الذي أعلنه المصمّم: قائمة الكتالوج نفسها (<see cref="BuildAllReportsMenu"/>)
    /// ومسار الفتح ونفس التلميح — وبنفس جسم معالج النقر المستخدم في المسار الاحتياطي،
    /// مع الحفاظ على مقاس الزر ونصه وخطه وألوانه كما أعلنها المصمّم (RULE-N146).
    /// </summary>
    private static void AdoptAllReportsButton(
        Button button, Form host, Type hostType, IServiceProvider? serviceProvider)
    {
        var menu = BuildAllReportsMenu(host, hostType, serviceProvider);
        button.ContextMenuStrip = menu;   // يبقى مملوكاً للزر (لا تسريب) ومتاحاً بالزر الأيمن أيضاً

        button.Click += (s, e) =>
        {
            try
            {
                menu.Show(button, new Point(0, button.Height));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "تعذر عرض قائمة كل التقارير في {Report}", host.GetType().Name);
            }
        };

        NavToolTip.SetToolTip(button, AllReportsTip);
    }

    /// <summary>
    /// ينشئ حاوية الأزرار احتياطياً حين يُعلن المصمّم اللوحة بلا حاويتها (بديل موثَّق،
    /// بلا استثناء): وتُضاف **أول** ابن للوحة كما في <see cref="BuildPanel"/>، فتُرسى
    /// التسمية المرساة يميناً أولاً (لأنها آخر الأبناء) ثم تأخذ الحاوية المساحة الباقية.
    /// </summary>
    private static FlowLayoutPanel CreateDeclaredNavFlow(Panel panel)
    {
        var flow = new FlowLayoutPanel
        {
            Name = NavFlowName,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            AutoScroll = true,
            Margin = new Padding(0),
            BackColor = GreenThemeHelper.BarBackground
        };

        panel.Controls.Add(flow);
        panel.Controls.SetChildIndex(flow, 0);
        return flow;
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

        // «كل التقارير» يُضاف **أخيراً**: مع FlowDirection.RightToLeft يصبح أقصى
        // اليسار فلا يُزاحم أزرار التصنيف الحالي ولا يغيّر ترتيبها.
        if (!HasAllReportsButton(flow))
            flow.Controls.Add(BuildAllReportsButton(host, hostType, serviceProvider));

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

    /// <summary>
    /// زر «كل التقارير»: قائمة مُصنَّفة بكل تصنيفات الكتالوج تفتح عند النقر،
    /// وكل تقرير فيها يُفتح بـ <see cref="OpenReport"/> نفسها — لا منطق انتقال مكرر.
    /// </summary>
    private static Button BuildAllReportsButton(Form host, Type hostType, IServiceProvider? serviceProvider)
    {
        var button = new Button
        {
            Name = AllReportsButtonName,
            Text = AllReportsText,
            Size = new Size(NavButtonWidth, NavButtonHeight),
            Margin = new Padding(4),
            FlatStyle = FlatStyle.Popup,
            Font = new Font("Tahoma", 9F, FontStyle.Bold),
            RightToLeft = RightToLeft.Yes,
            UseVisualStyleBackColor = false,
            BackColor = GreenThemeHelper.GroupBackground,
            ForeColor = GreenThemeHelper.TextStrong
        };

        NavToolTip.SetToolTip(button, AllReportsTip);

        var menu = BuildAllReportsMenu(host, hostType, serviceProvider);
        button.ContextMenuStrip = menu;   // يبقى مملوكاً للزر (لا تسريب) ومتاحاً بالزر الأيمن أيضاً
        button.Click += (s, e) =>
        {
            try
            {
                menu.Show(button, new Point(0, button.Height));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "تعذر عرض قائمة كل التقارير في {Report}", host.GetType().Name);
            }
        };

        return button;
    }

    /// <summary>
    /// يبني قائمة «كل التقارير» من <see cref="Catalog"/> نفسه (بلا كتالوج ثانٍ):
    /// عنصر أعلى لكل تصنيف، وتحته تقاريره، مع استثناء نوع النموذج الحالي حتى لا
    /// تُعيد القائمة فتح نفسها. التصنيف الذي لا يبقى فيه تقرير آخر لا يُعرض.
    /// </summary>
    private static ContextMenuStrip BuildAllReportsMenu(Form host, Type hostType, IServiceProvider? serviceProvider)
    {
        var menu = new ContextMenuStrip
        {
            Name = AllReportsMenuName,
            RightToLeft = RightToLeft.Yes,
            Font = new Font("Tahoma", 9F),
            ShowImageMargin = false,
            BackColor = GreenThemeHelper.GroupBackground,
            ForeColor = GreenThemeHelper.TextStrong
        };

        foreach (var category in Catalog)
        {
            var categoryItem = new ToolStripMenuItem(category.Caption)
            {
                Name = "miCategory" + (menu.Items.Count + 1),
                ForeColor = GreenThemeHelper.TextStrong
            };

            foreach (var entry in category.Reports)
            {
                // (2) استثناء التقرير الحالي: لا تظهر شاشة التقرير في قائمتها.
                if (entry.FormType == hostType) continue;

                var target = entry.FormType;
                var reportItem = new ToolStripMenuItem(entry.Title)
                {
                    Name = "miReport" + target.Name,
                    ForeColor = GreenThemeHelper.TextStrong
                };

                // (3) نفس مسار الفتح المستخدم في أزرار التصنيف.
                reportItem.Click += (s, e) => OpenReport(host, target, serviceProvider);
                categoryItem.DropDownItems.Add(reportItem);
            }

            if (categoryItem.DropDownItems.Count == 0) continue;   // لا تقرير آخر في هذا التصنيف

            // (4) مرور المؤشر على التصنيف يفتح قائمته الفرعية — والنقر يفتحها أصلاً.
            categoryItem.MouseEnter += (s, e) =>
            {
                try
                {
                    categoryItem.ShowDropDown();
                }
                catch (Exception ex)
                {
                    Log.Debug(ex, "تعذر فتح القائمة الفرعية للتصنيف {Category}", category.Caption);
                }
            };

            menu.Items.Add(categoryItem);
        }

        return menu;
    }

    // ═════════════════════════════════════════════════════════════════
    //  الانتقال إلى تقرير آخر
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يفتح التقرير المطلوب في <b>نموذج جديد</b> ويُبقي التقرير الحالي مفتوحاً كما هو
    /// (RULE-N78) — فيبقى المستخدم في تصنيفه بدل العودة إلى الشاشة الرئيسية.
    /// <para>
    /// الفتح يجري بنمط <c>clsGlobal._OpenForm</c> المعتمد في القائمة الرئيسية
    /// (نموذج فرعي MDI غير نمطي، وتُنشَّط النافذة المفتوحة بدل تكرارها)، فلا يوجد
    /// مسار فتح ثانٍ في التطبيق. والتقارير شاشات قراءة فقط، فيجوز فتحها غير نمطياً
    /// (<c>RULE-N09</c> تخص شاشات الإدخال حصراً).
    /// </para>
    /// </summary>
    private static void OpenReport(Form host, Type reportType, IServiceProvider? serviceProvider)
    {
        try
        {
            // (1) نافذة مفتوحة لنفس التقرير ⇒ تُستعاد وتُنشَّط بدل إنشاء نسخة ثانية.
            //     الفحص يسبق حلّ النموذج من DI حتى لا تُنشأ نسخة تُهملها _OpenForm.
            if (FindOpenReport(host, reportType) is Form existing)
            {
                RestoreAndActivate(existing);
                return;
            }

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

            // (2) نفس مسار فتح القائمة الرئيسية: النموذج الحالي يبقى مفتوحاً.
            clsGlobal._OpenForm(next);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر فتح تقرير من شريط التنقل: {Report}", reportType.Name);
            ShowOpenFailed();
        }
    }

    /// <summary>
    /// يبحث في النوافذ المفتوحة عن نافذة لنفس نوع التقرير المطلوب — يتجاهل نموذج
    /// الاستدعاء نفسه والنوافذ المحذوفة، ويقارن الاسم بلا حساسية لحالة الأحرف.
    /// </summary>
    private static Form? FindOpenReport(Form host, Type reportType)
    {
        foreach (Form openForm in Application.OpenForms)
        {
            if (openForm is null || ReferenceEquals(openForm, host)) continue;
            if (openForm.IsDisposed) continue;

            if (string.Equals(openForm.Name, reportType.Name, StringComparison.OrdinalIgnoreCase))
                return openForm;
        }

        return null;
    }

    /// <summary>يُعيد النافذة إلى وضعها الطبيعي إن كانت مُصغّرة ثم يُنشّطها ويُقدّمها.</summary>
    private static void RestoreAndActivate(Form form)
    {
        if (form.WindowState == FormWindowState.Minimized)
            form.WindowState = FormWindowState.Normal;

        form.Activate();
        form.BringToFront();
    }

    /// <summary>رسالة الخطأ الموحّدة — بلا تفاصيل استثناء للمستخدم (RULE-N29).</summary>
    private static void ShowOpenFailed()
        => clsDialogs.Error(OpenFailedMessage);

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

    /// <summary>يتحقق من وجود زر «كل التقارير» مسبقاً في حاوية الشريط (idempotent).</summary>
    private static bool HasAllReportsButton(Control flow)
    {
        foreach (Control child in flow.Controls)
        {
            if (string.Equals(child.Name, AllReportsButtonName, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    /// <summary>
    /// يتحقق من وجود أي زر تنقل مسبقاً في الحاوية عدا «كل التقارير» — هو علامة
    /// «تبنٍّ سابق» فلا يُضيف استدعاء <c>Attach</c> الثاني زراً واحداً (idempotent).
    /// البادئة <c>btnNav</c> هي نفس بادئة <c>BuildButton</c>.
    /// </summary>
    private static bool HasCategoryNavButtons(Control? flow)
    {
        if (flow is null) return false;

        foreach (Control child in flow.Controls)
        {
            if (string.Equals(child.Name, AllReportsButtonName, StringComparison.OrdinalIgnoreCase)) continue;
            if (child.Name?.StartsWith("btnNav", StringComparison.OrdinalIgnoreCase) == true) return true;
        }

        return false;
    }

    /// <summary>يبحث عن عنصر بالاسم في شجرة <paramref name="root"/> كلها (بحث متداخل).</summary>
    private static Control? FindControlByName(Control? root, string name)
    {
        if (root is null) return null;

        foreach (Control child in root.Controls)
        {
            if (string.Equals(child.Name, name, StringComparison.OrdinalIgnoreCase)) return child;
            if (FindControlByName(child, name) is Control nested) return nested;
        }

        return null;
    }

    /// <summary>يبحث عن عنصر بالاسم بين أبناء <paramref name="parent"/> المباشرين.</summary>
    private static Control? FindDirectChild(Control? parent, string name)
    {
        if (parent is null) return null;

        foreach (Control child in parent.Controls)
        {
            if (string.Equals(child.Name, name, StringComparison.OrdinalIgnoreCase)) return child;
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
