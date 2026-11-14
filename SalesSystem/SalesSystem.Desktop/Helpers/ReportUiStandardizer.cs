// -----------------------------------------------------------------------------
// ReportUiStandardizer.cs — معيار واجهة موحّد لكل شاشات التقارير (31 تقريراً)
//
// المشكلة: شاشات التقارير بُنيت على نمطين قديمين — بعضها يبني شبكته بـ Tahoma
// يدوياً وخلفية بيضاء، وبعضها يستدعي ThemeHelper.ApplyDataGridViewStyle فتخرج
// رؤوس رمادية وخط Segoe UI، وكلها بلا تنقل بالوحة المفاتيح. إصلاح ذلك يدوياً
// في 62 ملفاً مكرر ومحفوف بالانحراف، لذلك يُطبَّق المعيار من نقطة واحدة.
//
// البنية: كل تقرير اليوم نموذج واحد frm*Report : Form بملفَي *.cs + *.Designer.cs
// ومنطقه داخله — لم يعد هناك مجلد Tabs ولا توابع UserControl وسيطة لكل تقرير.
//
// الاستهلاك: سطر واحد كآخر عبارة في مُنشئ النموذج:
//     ReportUiStandardizer.ApplyForm(this);   // من داخل frm*Report : Form — المدخل الوحيد لشاشات التقارير
//     ReportUiStandardizer.Apply(this);       // لأي سطح تقرير مضمَّن (UserControl / Panel)
//
// مظهر الشاشة المرجعية (اختياري لكل تقرير على حدة) — ترتيب الأسطر إلزامي:
//     ReportUiStandardizer.ApplyForm(this);            // (1) الهيكل + الثيم الأخضر
//     ReportUiStandardizer.ApplyReferenceLook(this);   // (2) مظهر frmAddUpdateSalesInvoice
//     ReportNavigationHelper.Attach(this, _serviceProvider);   // (3) يبقى الأخير دائماً
//
// ما تفعله الأداة:
//   0) في ApplyForm وحدها، وقبل أي تنسيق، تبني الهيكل المرجعي للتقرير RULE-N79(أ)
//      مرة واحدة وقت التشغيل: pnlReportRoot (جدول 6 صفوف) ← pnlHeader + pnlToolbar
//      + pnlFilters + pnlGrid + pnlSummary + pnlFooter، بنقل العناصر القائمة (Reparent)
//      لا بإعادة إنشائها، وبلا إحداثيات مطلقة؛ وتبقى loadingPanel شفافة فوق الهيكل كله.
//   1) تمرّ على شجرة العناصر وتُطبّق GreenThemeHelper.ApplyGrid على **كل**
//      DataGridView — وهذا يتجاوز أي ThemeHelper.ApplyDataGridViewStyle سابق
//      (RULE-N51: الشبكة الخضراء هي النمط المعتمد، والتجاوز مقصود).
//   2) تفرض خط Tahoma على العناصر التي تلمسها (شبكات/تسميات 9F، أزرار 10F)،
//      ولا تُغيّر حجم الخط إن كان Tahoma أصلاً (حفاظاً على أحجام العناوين
//      المقصودة في المصمّم) — تتغيّر الأحجام فقط عند التحويل من Segoe UI،
//      وتُرحّل التسميات الحمراء القديمة (Firebrick) إلى أخضر الثيم الداكن (RULE-N54).
//   3) تُوصِّل تنقل لوحة المفاتيح مرة واحدة لكل شجرة:
//      WireEnterAsTab + WireSelectAllOnFocus + WireFocusHighlight + WireArrowNavigation
//      (RULE-N40 … RULE-N46).
//   4) تجمع عناصر التصفية في صندوق مجموعة واحد gbFilter («بحث وتصفية») داخل صفّ
//      جديد أعلى الشبكة، مع بقاء أزرار الإجراءات فوقه خارج الصندوق
//      (RULE-N56 … RULE-N62). النقل Reparent — لا إعادة إنشاء — فلا تُفقد أي
//      معالجات أحداث ولا تُلمس قيم التصفية.
//   5) تنتهي بـ GreenThemeHelper.Apply(root) — آخر خطوة دائماً (RULE-N96)،
//      لأن أي تنسيق لاحق كان سيمحو الأخضر.
//   6) للنماذج فقط (ApplyForm): تُوصِّل Esc لإغلاق التقرير و Ctrl+E لزر التصدير
//      في نداء واحد (F5 موصول سابقاً ولا يُضاعف)، وتفرض على شبكات التقرير
//      ReadOnly + AllowUserToAddRows=false + FullRowSelect (RULE-N22)، وتُلوّن
//      أزرار الإجراءات الثلاثة (تحديث/تصدير/إغلاق) من لوحة الثيم الأخضر وحدها
//      دون المساس بمقاساتها أو مواضعها أو خطوطها أو أحداثها (RULE-N50…N54، N112).
//   7) اختيارياً لكل تقرير: ApplyReferenceLook(form) — ومعها وحدها — تنسخ مظهر
//      شاشة المستند المرجعية frmAddUpdateSalesInvoice فوق الهيكل: أزرار Tahoma 10
//      عريض وارتفاع 36 بألوان الثيم، عنوان Tahoma 16 عريض موسّط، شبكات بفواصل
//      أفقية فقط، وشريط ملخص وصندوق تصفية بلون صناديق المجموعات، ثم تُغلِّف كتل
//      الشاشة الثلاث في صناديق مجموعة معنونة (gbToolbar «أوامر التقرير» ·
//      gbGrid «بيانات التقرير» · gbSummary «الإجماليات») على الترتيب الرأسي الثابت
//      نفسه — وهو معيار §2.34 (RULE-N135…N137). التقرير الذي لا يستدعيها يبقى
//      بمظهر ApplyForm كما هو تماماً.
//
// ما لا تفعله (حدود متعمّدة):
//   • لا تُعيد بناء أي منطق، ولا تُعيد تسمية عنصر قائم، ولا تلمس أي منطق تقرير
//     أو SQL أو تصفية أو تصدير أو نص عربي، ولا أي ملف مصمّم (*.Designer.cs):
//     الهيكل كله يُبنى وقت التشغيل. ما يُنشأ هو حاويات الهيكل فقط
//     (pnlReportRoot وما فيه من لوحات وصناديق) — ولا غيرها.
//   • في Apply (الحاويات المضمَّنة) لا تلمس ReadOnly / AllowUserToAddRows /
//     SelectionMode / DataSource إطلاقاً (RULE-N52) — وفرض سلوك العرض محصور في
//     شبكات نماذج التقارير عبر ApplyForm، ولا يُعمَّم على شبكات الإدخال
//     الاستثنائية (dgvItems / سلة POS) لأنها ليست شاشات تقارير (RULE-N22a).
//   • كل الأساليب دفاعية: null-safe، ولا تُطلق استثناءً إلى النموذج أبداً.
//   • آمنة للاستدعاء المتكرر (idempotent): الاستدعاء الثاني لا يُضاعف التوصيل
//     ولا يُعيد تنسيق الشبكة.
// -----------------------------------------------------------------------------

using System.Drawing;
using System.Runtime.CompilerServices;

namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// معيار واجهة موحّد لشاشات التقارير: الثيم الأخضر + تنقل لوحة المفاتيح + شبكات خضراء،
/// من سطر واحد لكل شاشة بدل تعديل كل تقرير يدوياً.
/// </summary>
public static class ReportUiStandardizer
{
    /// <summary>علامة "تم التوحيد" — تُخزَّن في <see cref="Control.Tag"/> للجذر (RULE-N96).</summary>
    private const string AppliedTag = "ReportUiStandardized";

    /// <summary>قيمة العلامة المخزَّنة في السجل الضعيف (هوية مرجعية بلا تسريب ذاكرة).</summary>
    private static readonly object AppliedMarker = new();

    /// <summary>
    /// سجل الجذور التي وُحِّدت بالفعل. <see cref="ConditionalWeakTable{TKey,TValue}"/>
    /// وليس <c>HashSet</c> حتى لا يُبقى مرجع قوي إلى النماذج المغلقة.
    /// </summary>
    private static readonly ConditionalWeakTable<Control, object> Applied = new();

    /// <summary>سجل الجذور التي وُصِّل فيها تنقل لوحة المفاتيح (منع التوصيل المزدوج).</summary>
    private static readonly ConditionalWeakTable<Control, object> WiredTrees = new();

    // لون تظليل الحقل النشط (RULE-N46) لا يُخزَّن هنا: مالكه الوحيد
    // ThemePalette.ActiveFieldColor، ويُمرَّر إلى KeyboardHelper.WireFocusHighlight
    // بالقيمة الافتراضية للمفتاح فيقرأ الحقل القيمة الحيّة عند كل دخول
    // (RULE-N54b / GAP-09) — فلا لون حرفي في هذا الملف.

    /// <summary>أسماء تسميات شريط العنوان المعتمدة.</summary>
    private static readonly string[] HeaderLabelNames = { "lblMode", "lblTitle", "lblHeader", "lblPageTitle" };

    /// <summary>اسم صندوق التصفية الموحّد (RULE-N56).</summary>
    private const string FilterGroupControlName = "gbFilter";

    /// <summary>اسم اللوحة التي تحمل صندوق التصفية كصف مستقل أعلى الشبكة.</summary>
    private const string FilterPanelControlName = "pnlFilters";

    /// <summary>اسم حاوية عناصر التصفية داخل الصندوق.</summary>
    private const string FilterFlowControlName = "flpFilterControls";

    /// <summary>نص صندوق التصفية الموحّد.</summary>
    private const string FilterGroupCaption = "بحث وتصفية";

    /// <summary>ارتفاع صف التصفية الجديد بالبكسل (RULE-N56).</summary>
    private const int FilterPanelHeight = 68;

    /// <summary>الحد الأدنى لعناصر التصفية قبل إنشاء الصندوق (RULE-N56).</summary>
    private const int MinFilterControls = 2;

    /// <summary>سجل الشجرات التي أُنشئ فيها صندوق التصفية (منع التنفيذ المزدوج).</summary>
    private static readonly ConditionalWeakTable<Control, object> FilterGrouped = new();

    // ═════════════════════════════════════════════════════════════════
    //  الهيكل المرجعي للتقرير (RULE-N79 أ) — أسماء الحاويات وارتفاعات الصفوف
    // ═════════════════════════════════════════════════════════════════

    /// <summary>جدول الهيكل المرجعي الذي يُبنى في وقت التشغيل (RULE-N79 أ).</summary>
    private const string ShellRootControlName = "pnlReportRoot";

    /// <summary>صف شريط العنوان — يستضيف <c>lblMode</c>.</summary>
    private const string ShellHeaderControlName = "pnlHeader";

    /// <summary>صف شريط الأدوات — يستضيف أزرار التحديث والتصدير.</summary>
    private const string ShellToolbarControlName = "pnlToolbar";

    /// <summary>صف الشبكة — يستضيف <c>dgvReport</c>.</summary>
    private const string ShellGridControlName = "pnlGrid";

    /// <summary>صف شريط الملخص — يستضيف عدد السجلات والإجماليات.</summary>
    private const string ShellSummaryControlName = "pnlSummary";

    /// <summary>صف التذييل — يستضيف زر الإغلاق.</summary>
    private const string ShellFooterControlName = "pnlFooter";

    /// <summary>حاوية تدفّق أزرار شريط الأدوات.</summary>
    private const string ToolbarFlowControlName = "flpToolbar";

    /// <summary>حاوية تدفّق تسميات شريط الملخص.</summary>
    private const string SummaryFlowControlName = "flpSummary";

    /// <summary>حاوية تدفّق زر التذييل.</summary>
    private const string FooterFlowControlName = "flpFooter";

    /// <summary>لوحة التحميل الشفافة التي تبقى فوق الهيكل كله.</summary>
    private const string LoadingPanelControlName = "loadingPanel";

    /// <summary>ارتفاع صف شريط العنوان بالبكسل (RULE-N79 أ).</summary>
    private const int ShellHeaderRowHeight = 46;

    /// <summary>ارتفاع صف شريط الأدوات بالبكسل (RULE-N79 أ).</summary>
    private const int ShellToolbarRowHeight = 56;

    /// <summary>
    /// ارتفاع صف التصفية بالبكسل (RULE-N79 أ) — يُطوى إلى صفر إذا لا توجد عناصر تصفية.
    /// يُستخدم حين تبقى عناصر التصفية في سطر واحد: ثلاثة عناصر أو أقل.
    /// </summary>
    private const int ShellFiltersRowHeight = 96;

    /// <summary>
    /// ارتفاع صف التصفية عندما تتجاوز عناصره ثلاثة عناصر: الحاوية تلتف
    /// (<c>WrapContents = true</c>) إلى سطر ثانٍ فيُقطع السطر الثاني بلا مساحة،
    /// لذلك يرتفع الصف إلى هذا الثابت — داخل المدى المعتمد 96–120 بكسل (RULE-N56).
    /// </summary>
    private const int ShellFiltersRowHeightTall = 120;

    /// <summary>أقصى عدد عناصر تصفية يبقى في سطر واحد بارتفاع <see cref="ShellFiltersRowHeight"/>.</summary>
    private const int ShellFiltersSingleLineMax = 3;

    /// <summary>ارتفاع صف الملخص بالبكسل (RULE-N79 أ).</summary>
    private const int ShellSummaryRowHeight = 34;

    /// <summary>ارتفاع صف التذييل بالبكسل (RULE-N79 أ).</summary>
    private const int ShellFooterRowHeight = 56;

    /// <summary>
    /// سماحية الصف البصري بالبكسل عند جمع عناصر التصفية (RULE-N57): العنصران ينتميان
    /// إلى الصف نفسه ما دام الفرق بين إحداثيّي <see cref="Control.Top"/> لا يتجاوز هذه
    /// القيمة. الأساس العملي المقيس: تسميات المصمّم ذات الموضع المطلق تهبط 4–9 بكسل
    /// تحت حقلها، فالسماحية يجب أن تكون ≥ 10 وإلا انقسم الصف البصري الواحد إلى صفّين
    /// وبقيت التسمية معزولة عن حقلها في تدفّق RTL. أسوأ حالة مقيسة هي
    /// <c>frmAccountStatementReport</c>: الحقول عند 53/54/56/56 وتسمياتها عند 58/58/62/62
    /// (أقصى فرق = 9)، فتُجمَّع كلها في صف واحد بهذه القيمة. وفي المقابل تبقى فجوة الصف
    /// الحقيقية أكبر بكثير (<c>frmProductMovementReport</c>: 42 بكسل بين صفّيه)، فلا دمج
    /// خاطئ بين صفّين بصريين متمايزين.
    /// </summary>
    private const int RowTopTolerance = 12;

    /// <summary>أرقام صفوف الهيكل الستة في <c>pnlReportRoot</c>.</summary>
    private const int ShellHeaderRow = 0;
    private const int ShellToolbarRow = 1;
    private const int ShellFiltersRow = 2;
    private const int ShellGridRow = 3;
    private const int ShellSummaryRow = 4;
    private const int ShellFooterRow = 5;

    /// <summary>تسميات شريط الملخص بترتيبها داخل <c>flpSummary</c> (RULE-N79 أ).</summary>
    private static readonly string[] SummaryLabelNames = { "lblSummary", "label1", "lblRecordsCount" };

    /// <summary>تسميات لوحة التحميل — لا تدخل صندوق التصفية إطلاقاً.</summary>
    private static readonly string[] OverlayLabelNames = { "ll", "loadingLabel" };

    // ═════════════════════════════════════════════════════════════════
    //  Apply — لسطح تقرير مضمَّن أو أي حاوية (UserControl / Panel)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// للاستدعاء على أي سطح تقرير مضمَّن (UserControl / Panel) — أما نماذج التقارير
    /// <c>frm*Report</c> فمدخلها الوحيد <see cref="ApplyForm"/>.
    /// يوحّد واجهة الحاوية: شبكات خضراء + خط Tahoma + تنقل لوحة مفاتيح + الثيم الأخضر.
    /// آمن للاستدعاء المتكرر: الاستدعاء الثاني لا يفعل شيئاً.
    /// </summary>
    /// <param name="root">جذر الشجرة (سطح تقرير مضمَّن عادةً) — قد يكون null.</param>
    public static void Apply(Control? root)
    {
        if (root is null) return;

        try
        {
            if (IsApplied(root)) return;

            StyleTree(root, skipAppliedSubtrees: false);
            WireKeyboard(root);

            // RULE-N56 … RULE-N62: صندوق التصفية الموحّد قبل الثيم الأخير.
            EnsureFilterGroup(root);

            // RULE-N96: الثيم الأخضر آخر خطوة — بعد أي تنسيق سابق في المُنشئ.
            GreenThemeHelper.Apply(root);
        }
        catch (Exception ex)
        {
            // لا نُفشل فتح تقرير بسبب لون أو خط: نسجّل الخطأ فقط.
            Serilog.Log.Error(ex, "تعذر توحيد واجهة تقرير: {Root}", root.GetType().Name);
        }
        finally
        {
            MarkApplied(root);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  ApplyForm — لنموذج تقرير (frm*Report)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يوحّد نموذج التقرير: يطبّق <see cref="Apply"/> على شجرته، ويُلوّن تسمية العنوان
    /// العلوية إن وُجدت، ويُوصِّل <c>F5</c> بزر التحديث المرئي إن أمكن العثور عليه.
    /// هذا هو المدخل الوحيد لشاشات التقارير <c>frm*Report</c> — يُستدعى كآخر عبارة
    /// في مُنشئ النموذج بعد <c>InitializeComponent()</c> (RULE-N50 … N62، N96).
    /// </summary>
    /// <param name="form">نموذج التقرير — قد يكون null.</param>
    public static void ApplyForm(Form? form)
    {
        if (form is null) return;

        try
        {
            if (IsApplied(form)) return;

            // RULE-N79(أ): هيكل شاشة التقرير المرجعي يُبنى أولاً — قبل أي تنسيق —
            // حتى يعمل الثيم وتوصيل لوحة المفاتيح على الشجرة النهائية لا على القديمة.
            BuildReportShell(form);

            // النماذج التي تستضيف تبويباً موحَّداً سبق أن وُصِّل تنقله داخل التبويب؛
            // إعادة التوصيل من جذر النموذج تُضاعف معالج Enter فيتخطّى التركيز حقلين.
            if (HasStandardizedSubtree(form))
            {
                StyleTree(form, skipAppliedSubtrees: true);
                GreenThemeHelper.Apply(form);
                MarkApplied(form);
            }
            else
            {
                Apply(form);   // النموذج المستقل: توحيد كامل + توصيل كامل
            }

            ApplyTitleHeader(form);
            StyleActionButtons(form);            // أزرار الإجراءات بلوحة الثيم الأخضر (RULE-N50…N54)
            WireRefreshShortcut(form);           // F5 — موصول مرة واحدة هنا فقط
            WireCloseAndExportShortcuts(form);   // Esc / Ctrl+E — نداء واحد
            EnforceReportGridRules(form);        // RULE-N22 — شبكات التقارير للعرض فقط
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر توحيد واجهة نموذج تقرير: {Form}", form.GetType().Name);
        }
        finally
        {
            MarkApplied(form);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  ApplyReferenceLook — مظهر شاشة المستند المرجعية (frmAddUpdateSalesInvoice)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>ارتفاع أزرار الإجراءات في المظهر المرجعي بالبكسل.</summary>
    private const int ReferenceButtonHeight = 36;

    /// <summary>
    /// حشو داخلي أفقي لزر الإجراء بالبكسل — كافٍ للأيقونة والمسافة بعدها والحرف الأخير،
    /// فتُقاس سعة الزر من نصه الفعلي بدل رقم ثابت (العناوين تختلف طولاً بين التقارير).
    /// </summary>
    private const int ReferenceButtonPaddingHorizontal = 24;

    /// <summary>الحد الأدنى لعرض زر الإجراء بالبكسل — يُوسَّع الزر تحته، ولا يُصغَّر أبداً.</summary>
    private const int ReferenceButtonMinWidth = 100;

    /// <summary>حجم خط شريط العنوان في المظهر المرجعي.</summary>
    private const float ReferenceHeaderFontSize = 16F;

    /// <summary>
    /// يطبّق مظهر شاشة المستند المرجعية <c>frmAddUpdateSalesInvoice</c> فوق هيكل التقرير
    /// الذي بناه <see cref="ApplyForm"/>: أزرار الإجراءات (تحديث/تصدير/إغلاق) بخط Tahoma 10
    /// عريض وارتفاع 36 وألوان الثيم، وشريط العنوان بخط Tahoma 16 عريض موسّط، وشبكات بفواصل
    /// أفقية بلا خطوط رأسية، وشريط الملخص وصندوق التصفية بألوان الثيم، ثم تجميع كتل الشاشة
    /// في صناديق مجموعة معنونة كما في الشاشة المرجعية (§2.34 — <c>RULE-N135</c>…<c>N137</c>).
    /// <para>ترتيب الاستدعاء إلزامي: **بعد** <see cref="ApplyForm"/> — لأن
    /// <see cref="GreenThemeHelper.Apply"/> يعيد طلاء اللوحات فيمحو أي تنسيق سابق — و**قبل**
    /// <c>ReportNavigationHelper.Attach</c> الذي يحجز مساحة شريط التنقل ويبقى آخر سطر
    /// في المُنشئ.</para>
    /// <para>لا يُعيد تسمية أي عنصر ولا يُنشئ عنصراً جديداً (RULE-N113)، ولا يمسّ
    /// <c>Text</c> ولا <c>Location</c> ولا <c>TabIndex</c> ولا <c>DialogResult</c> ولا أي
    /// معالج حدث — و<see cref="Control.Width"/> لا يُقلَّص أبداً، يُوسَّع فقط ليتّسع لنص
    /// الزر المقيس — ولا سلوك الشبكة:
    /// <c>ReadOnly</c> / <c>SelectionMode</c> / <c>AllowUserToAddRows</c> / <c>DataSource</c>
    /// تبقى كما هي (RULE-N52). ولا تُطلق استثناءً أبداً.</para>
    /// </summary>
    /// <param name="form">نموذج التقرير — قد يكون null.</param>
    public static void ApplyReferenceLook(Form? form)
    {
        if (form is null || form.IsDisposed) return;

        try
        {
            ApplyReferenceActionButtons(form);   // (1) أزرار الإجراءات الثلاثة
            ApplyReferenceHeaders(form);         // (2) شريط العنوان
            ApplyReferenceGridLook(form);        // (3) الشبكات: فواصل أفقية فقط
            ApplyReferenceStrips(form);          // (4) شريط الملخص + صندوق التصفية
            ApplyReferenceGrouping(form);        // (5) صناديق المجموعة المعنونة (§2.34)
        }
        catch (Exception ex)
        {
            // لا نُفشل فتح تقرير بسبب مظهر: نسجّل الخطأ فقط ويبقى الشكل السابق.
            Serilog.Log.Error(ex, "تعذر تطبيق مظهر الشاشة المرجعية على التقرير: {Form}", form.GetType().Name);
        }
    }

    /// <summary>
    /// (1) أزرار الإجراءات المعروفة (<c>btnRefresh</c> / <c>btnExport</c> / <c>btnClose</c>)
    /// بمقاس وخط المظهر المرجعي وألوان الثيم وحدها (RULE-N54 / RULE-N112).
    /// يُستثنى شريط تنقل التقارير لأنه يُنسّق نفسه في <c>ReportNavigationHelper</c>.
    /// </summary>
    private static void ApplyReferenceActionButtons(Form form)
    {
        foreach (var button in EnumerateButtons(form))
        {
            if (button is null || button.IsDisposed) continue;
            if (IsInReportNavStrip(button)) continue;

            var colors = ResolveReferenceButtonColors(button.Name);
            if (colors is null) continue;   // ليس زر إجراء تقرير معروفاً ⇒ يُترك كما هو

            ApplyReferenceButtonLook(button, colors.Value.Back, colors.Value.Fore);
        }
    }

    /// <summary>
    /// ألوان زر الإجراء في المظهر المرجعي من ثوابت <see cref="GreenThemeHelper"/> فقط،
    /// أو <c>null</c> إن لم يكن من أزرار التقارير المعروفة.
    /// </summary>
    private static (Color Back, Color Fore)? ResolveReferenceButtonColors(string? name)
    {
        if (string.IsNullOrEmpty(name)) return null;

        if (string.Equals(name, "btnRefresh", StringComparison.OrdinalIgnoreCase))
            return (GreenThemeHelper.HeaderBackground, GreenThemeHelper.HeaderForeground);

        if (string.Equals(name, "btnExport", StringComparison.OrdinalIgnoreCase))
            return (GreenThemeHelper.ActionInfoBackground, GreenThemeHelper.HeaderForeground);

        if (string.Equals(name, "btnClose", StringComparison.OrdinalIgnoreCase))
            return (GreenThemeHelper.ActionNeutralBackground, GreenThemeHelper.HeaderForeground);

        return null;
    }

    /// <summary>
    /// يضبط شكل زر إجراء واحد كمظهر الشاشة المرجعية: العرض يبقى كما صمّمته الشاشة
    /// ويُوسَّع فقط ليتّسع لنصه المقيس — فلا يُقطع عنوان مهما طال (مثل «📤 تصدير Excel»)
    /// ولا ينزل زر قصير عن <see cref="ReferenceButtonMinWidth"/> — والارتفاع وحده
    /// يصبح <see cref="ReferenceButtonHeight"/>.
    /// </summary>
    private static void ApplyReferenceButtonLook(Button button, Color backColor, Color foreColor)
    {
        try
        {
            button.AutoSize = false;
            button.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            button.Height = ReferenceButtonHeight;
            button.Padding = Padding.Empty;
            button.Margin = new Padding(4);
            button.MinimumSize = Size.Empty;

            // العرض لا يُقلَّص إطلاقاً — يُوسَّع فقط ليتّسع لنصه المقيس (الأيقونة تزيد العرض).
            // نص فارغ أو null ⇒ يبقى الحد الأدنى وحده (بلا استثناء ولا قياس).
            int needed = ReferenceButtonMinWidth;
            if (!string.IsNullOrEmpty(button.Text))
            {
                int measured = TextRenderer.MeasureText(button.Text, button.Font).Width
                               + ReferenceButtonPaddingHorizontal;
                if (measured > needed) needed = measured;
            }

            if (button.Width < needed)
                button.Width = needed;

            button.BackColor = backColor;
            button.ForeColor = foreColor;

            // FlatStyle.Flat + إلغاء VisualStyles حتى يظهر لون الثيم كما هو.
            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = GreenThemeHelper.HeaderBackground;
            button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.1f);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق مظهر زر إجراء مرجعي: {Button}", button?.Name);
        }
    }

    /// <summary>
    /// (2) شريط العنوان (<c>lblMode</c>/<c>lblTitle</c>/<c>lblHeader</c>/<c>lblPageTitle</c>):
    /// Tahoma 16 عريض موسّط، والألوان من <see cref="GreenThemeHelper.ApplyHeader"/> (RULE-N53).
    /// </summary>
    private static void ApplyReferenceHeaders(Control root)
    {
        foreach (Control child in root.Controls)
        {
            if (child is Label label && !label.IsDisposed
                && !string.IsNullOrEmpty(label.Name)
                && HeaderLabelNames.Any(n => string.Equals(n, label.Name, StringComparison.OrdinalIgnoreCase)))
            {
                label.Font = new Font("Tahoma", ReferenceHeaderFontSize, FontStyle.Bold);
                label.AutoSize = false;
                label.TextAlign = ContentAlignment.MiddleCenter;
                GreenThemeHelper.ApplyHeader(label);   // الخلفية/النص من الثيم وحده
            }

            if (child.HasChildren)
                ApplyReferenceHeaders(child);
        }
    }

    /// <summary>
    /// (3) كل شبكة في الشجرة: فواصل أفقية بلا خطوط رأسية (مظهر <c>dgvItems</c>) ورؤوس
    /// ثابتة الارتفاع وخلفية/خط من الثيم، مع ترك سلوك الشبكة كما هو (RULE-N52).
    /// </summary>
    private static void ApplyReferenceGridLook(Control root)
    {
        foreach (Control child in root.Controls)
        {
            if (child is DataGridView grid && !grid.IsDisposed)
            {
                grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                grid.MultiSelect = false;
                grid.BackgroundColor = GreenThemeHelper.GridBackground;
                grid.GridColor = GreenThemeHelper.GridLine;

                // RULE-N52: ReadOnly / SelectionMode / AllowUserToAddRows / DataSource / Columns
                // لا تُمسّ هنا — كل شبكة تعلن سلوكها بنفسها، وApplyGrid سبق أن طبّق الثيم.
            }

            if (child.HasChildren)
                ApplyReferenceGridLook(child);
        }
    }

    /// <summary>
    /// (4) شريط الملخص (<c>pnlSummary</c> + تسميات <c>flpSummary</c>) وصندوق التصفية
    /// (<c>gbFilter</c>): خلفية صناديق المجموعات وخط Tahoma 10 عريض بلون النص المميّز.
    /// لا يُنشئ ولا يُعيد تسمية أي عنصر (RULE-N113) — ضبط عناصر قائمة فقط.
    /// </summary>
    private static void ApplyReferenceStrips(Control root)
    {
        if (FindChildByName(root, ShellSummaryControlName) is Control summary && !summary.IsDisposed)
        {
            summary.BackColor = GreenThemeHelper.GroupBackground;
            summary.Padding = new Padding(0);

            if (FindChildByName(summary, SummaryFlowControlName) is Control flow && !flow.IsDisposed)
            {
                foreach (Control child in flow.Controls)
                {
                    if (child is not Label label || label.IsDisposed) continue;

                    label.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                    label.ForeColor = GreenThemeHelper.TextStrong;
                    label.AutoSize = true;
                    label.Margin = new Padding(10, 6, 0, 0);   // فاصل مقروء بين العدد والإجماليات
                }
            }
        }

        if (FindChildByName(root, FilterGroupControlName) is GroupBox filterGroup && !filterGroup.IsDisposed)
        {
            filterGroup.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            filterGroup.ForeColor = GreenThemeHelper.TextStrong;
            filterGroup.BackColor = GreenThemeHelper.GroupBackground;
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  (5) تجميع كتل الشاشة في صناديق مجموعة معنونة — §2.34 (RULE-N135…N137)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>صندوق مجموعة شريط الأدوات في التقرير المرجعي.</summary>
    private const string ReferenceToolbarGroupControlName = "gbToolbar";

    /// <summary>عنوان صندوق أوامر التقرير (RULE-N136).</summary>
    private const string ReferenceToolbarGroupCaption = "أوامر التقرير";

    /// <summary>صندوق مجموعة الشبكة في التقرير المرجعي.</summary>
    private const string ReferenceGridGroupControlName = "gbGrid";

    /// <summary>عنوان صندوق بيانات التقرير (RULE-N136).</summary>
    private const string ReferenceGridGroupCaption = "بيانات التقرير";

    /// <summary>صندوق مجموعة شريط الملخص في التقرير المرجعي.</summary>
    private const string ReferenceSummaryGroupControlName = "gbSummary";

    /// <summary>عنوان صندوق الإجماليات (RULE-N136).</summary>
    private const string ReferenceSummaryGroupCaption = "الإجماليات";

    /// <summary>اسم شبكة التقرير التي تُغلَّف في صندوق «بيانات التقرير».</summary>
    private const string GridControlName = "dgvReport";

    /// <summary>
    /// أدنى ارتفاع لصف الأدوات بعد تجميعه — عنوان الصندوق يحتاج مساحة،
    /// ويُوسَّع الصف فوقه إن كان محتواه أطول (فلا يُقطع أي زر — <c>RULE-N137</c>).
    /// </summary>
    private const int ReferenceToolbarRowHeight = 72;

    /// <summary>أدنى ارتفاع لصف الملخص بعد تجميعه (لسبب الارتفاع نفسه).</summary>
    private const int ReferenceSummaryRowHeight = 60;

    /// <summary>
    /// هامش عنوان صندوق المجموعة بالبكسل فوق ارتفاع خطه — قياس ثابت في WinForms
    /// لارتفاع شريط العنوان المحجوز أعلى منطقة محتوى الصندوق.
    /// </summary>
    private const int ReferenceGroupCaptionPadding = 8;

    /// <summary>
    /// (5) يُغلِّف كتل الشاشة الثلاث — أوامر التقرير والشبكة والإجماليات — في صناديق
    /// مجموعة معنونة، على الترتيب الرأسي الثابت نفسه: العنوان (بلا صندوق) ← أوامر التقرير ←
    /// بحث وتصفية (صندوقه موجود أصلاً) ← بيانات التقرير ← الإجماليات ← شريط الأسفل
    /// (بلا صندوق) — كما في <c>frmAddUpdateSalesInvoice</c> (<c>RULE-N135</c>…<c>N137</c>).
    /// <para>ثلاثة صفوف فقط تُغلَّف: <c>pnlToolbar</c> ⇒ <c>gbToolbar</c> «أوامر التقرير»،
    /// و<c>pnlGrid</c> ⇒ <c>gbGrid</c> «بيانات التقرير»، و<c>pnlSummary</c> ⇒
    /// <c>gbSummary</c> «الإجماليات». أما <c>pnlFilters</c> فصندوقه <c>gbFilter</c> قائم
    /// فلا يُغلَّف داخل صندوق (ممنوع صندوق داخل صندوق)، و<c>pnlHeader</c>/<c>pnlFooter</c>
    /// بلا صندوق (شريط العنوان وشريط الأزرار في الشاشة المرجعية).
    /// <para><b>لا نقل حذف:</b> المحتوى القائم يُنقل (Reparent) بـ <see cref="MoveControl"/>
    /// الذي يُلغي الـAnchor المطلق (<c>RULE-N126</c>) ويحفظ النص والخط والمقاس والمعالجات
    /// وسلوك الشبكة (<c>RULE-N52</c>)؛ ولا تُمسّ <c>loadingPanel</c> لأنها ابن النموذج
    /// لا ابن أي صف. و<b>لا صندوق فارغ</b>: يُتخطّى الصف كاملاً إذا غاب محتواه أو كان
    /// محذوفاً. <b>وآمنة للاستدعاء المتكرر</b>: وجود الصندوق ابناً للصف يعني انتهاء العمل
    /// فلا يُضاف ثانٍ. ولا تُطلق استثناءً أبداً.</para>
    /// </summary>
    /// <param name="form">نموذج التقرير — قد يكون null.</param>
    private static void ApplyReferenceGrouping(Form form)
    {
        if (form is null || form.IsDisposed) return;

        try
        {
            if (FindDirectChild(form, ShellRootControlName) is not TableLayoutPanel root || root.IsDisposed)
                return;   // لا هيكل ⇒ لا تجميع (تقرير لم يمرّ بـ ApplyForm)

            // (أ) أوامر التقرير: flpToolbar داخل gbToolbar، ثم ارتفاع الصف بالكفاية.
            if (WrapShellRowInGroup(
                    root, ShellToolbarControlName, ReferenceToolbarGroupControlName,
                    ReferenceToolbarGroupCaption, ToolbarFlowControlName) is GroupBox toolbarGroup)
            {
                FitShellRowHeight(root, ShellToolbarRow, toolbarGroup, ReferenceToolbarRowHeight);
            }

            // (ب) بيانات التقرير: الشبكة داخل gbGrid — وصفّها يبقى Percent بلا ارتفاع ثابت.
            WrapShellRowInGroup(
                root, ShellGridControlName, ReferenceGridGroupControlName,
                ReferenceGridGroupCaption, GridControlName, contentIsGrid: true);

            // (ج) الإجماليات: flpSummary داخل gbSummary.
            if (WrapShellRowInGroup(
                    root, ShellSummaryControlName, ReferenceSummaryGroupControlName,
                    ReferenceSummaryGroupCaption, SummaryFlowControlName) is GroupBox summaryGroup)
            {
                FitShellRowHeight(root, ShellSummaryRow, summaryGroup, ReferenceSummaryRowHeight);
            }
        }
        catch (Exception ex)
        {
            // لا نُفشل فتح تقرير بسبب تجميع: يبقى الشكل السابق وتُسجَّل المشكلة فقط.
            Serilog.Log.Error(ex, "تعذر تجميع كتل مظهر الشاشة المرجعية في تقرير: {Form}", form.GetType().Name);
        }
    }

    /// <summary>
    /// يُغلِّف محتوى صفّ واحد في صندوق مجموعة معنون من ألوان الثيم وداخل الصف نفسه.
    /// يُعيد الصندوق (المُنشأ أو القائم) أو <c>null</c> إن تخطّى الصف.
    /// <para>التخطّي بلا استثناء ولا صندوق فارغ: الصف غير موجود/محذوف، أو محتواه
    /// غير موجود/محذوف/ليس ابناً له مباشرةً. والتكرار محكوم: وجود الصندوق ابناً للصف
    /// يُنهي العمل فوراً (idempotent).</para>
    /// </summary>
    private static GroupBox? WrapShellRowInGroup(
        Control? shellRoot,
        string rowControlName,
        string groupControlName,
        string caption,
        string contentControlName,
        bool contentIsGrid = false)
    {
        if (shellRoot is null || shellRoot.IsDisposed) return null;

        var row = FindChildByName(shellRoot, rowControlName);
        if (row is null || row.IsDisposed) return null;

        // (1) الصندوق موجود مسبقاً ⇒ لا صندوق فوق صندوق ولا تغليف مزدوج (RULE-N137).
        if (FindDirectChild(row, groupControlName) is GroupBox existing && !existing.IsDisposed)
            return existing;

        // (2) المحتوى المراد تغليفه: الشبكة بالاسم ثم أول شبكة مباشرة، وإلا الحاوية بالاسم.
        Control? content = null;
        if (contentIsGrid)
            content = FindDirectChild(row, contentControlName) as DataGridView ?? FindDirectGrid(row);
        else
            content = FindDirectChild(row, contentControlName);

        // (3) لا صندوق فارغ: لا تغليف إلا لمحتوى موجود فعلاً داخل هذا الصف.
        if (content is null || content.IsDisposed || content.Parent != row) return null;

        var group = new GroupBox
        {
            Name = groupControlName,
            Text = caption,
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes,
            TabStop = false,
            Font = new Font("Tahoma", 10F, FontStyle.Bold),
            BackColor = GreenThemeHelper.GroupBackground,   // RULE-N54 — من الثيم وحده
            ForeColor = GreenThemeHelper.TextStrong,
            Padding = new Padding(8, 2, 8, 4),              // مضغوط: لا يأكل العنوان الصف
            Margin = new Padding(0)
        };

        // (4) نقل (Reparent) لا حذف — MoveControl يُلغي الـAnchor المطلق (RULE-N126).
        if (!MoveControl(content, group)) return null;

        content.Dock = DockStyle.Fill;
        row.Controls.Add(group);

        return group;
    }

    /// <summary>
    /// يضبط ارتفاع صفّ الصندوق في <c>pnlReportRoot</c> مرة واحدة بعد التجميع:
    /// <paramref name="minimumHeight"/> حدّ أدنى، ويُوسَّع فوقه إن كان عنوان الصندوق
    /// وحشوه ومحتواه تحتاج أكثر — توسيع فقط، ولا تقليص لصفّ قائم.
    /// <para>الصفوف النسبية (<c>SizeType.Percent</c>) لا تُمسّ، والفهرس خارج المدى
    /// يُتجاهل بصمت — فلا يفشل التقرير بسبب قياس.</para>
    /// </summary>
    private static void FitShellRowHeight(TableLayoutPanel? root, int rowIndex, GroupBox? group, int minimumHeight)
    {
        if (root is null || root.IsDisposed || group is null || group.IsDisposed) return;

        try
        {
            if (rowIndex < 0 || rowIndex >= root.RowStyles.Count) return;

            var style = root.RowStyles[rowIndex];
            if (style is null || style.SizeType != SizeType.Absolute) return;   // صف الشبكة (Percent) يبقى كما هو

            int wanted = Math.Max(minimumHeight, MeasureGroupedRowHeight(group));

            if (style.Height < wanted) style.Height = wanted;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر ضبط ارتفاع صف في مظهر التقرير المرجعي: {Row}", rowIndex);
        }
    }

    /// <summary>
    /// أقل ارتفاع يكفي الصندوق بلا قطع: أعلى محتوى (بأبعاده المفضّلة التي تشمل حشوه
    /// وهوامش أبنائه) + ارتفاع سطر عنوان الصندوق + هامشه + حشو الصندوق الرأسي.
    /// يُرجع صفراً حين يتعذّر القياس أو لا محتوى — فيبقى الحد الأدنى وحده.
    /// </summary>
    private static int MeasureGroupedRowHeight(GroupBox group)
    {
        try
        {
            int contentHeight = 0;

            foreach (Control child in group.Controls)
            {
                if (child.IsDisposed) continue;

                int height = child.PreferredSize.Height;
                if (height > contentHeight) contentHeight = height;
            }

            if (contentHeight <= 0) return 0;

            return contentHeight + group.Font.Height + ReferenceGroupCaptionPadding + group.Padding.Vertical;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر قياس ارتفاع محتوى صندوق المجموعة: {Group}", group?.Name);
            return 0;
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  الهيكل المرجعي لشاشة التقرير (RULE-N79 أ) — يُبنى مرة واحدة وقت التشغيل
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يبني هيكل شاشة التقرير المرجعي <c>RULE-N79(أ)</c> داخل النموذج نفسه:
    /// <c>pnlHeader</c> + <c>pnlToolbar</c> + <c>pnlFilters</c> + <c>pnlGrid</c> +
    /// <c>pnlSummary</c> + <c>pnlFooter</c> داخل <c>pnlReportRoot</c> (جدول بستة صفوف).
    /// العناصر القائمة تُنقل (Reparent) ولا تُعاد إنشاءها، فلا تُفقد معالجات الأحداث
    /// ولا تُلمس الأسماء ولا الخطوط ولا أحجام الأزرار ولا <c>DialogResult</c> ولا
    /// سلوك الشبكة (<c>RULE-N52</c>) — ولا يُعدَّل أي ملف مصمّم.
    /// <para>آمنة للاستدعاء المتكرر: وجود <c>pnlReportRoot</c> يعني انتهاء العمل.
    /// ولا تُطلق استثناءً أبداً: الخطأ يُسجَّل فقط ويبقى التقرير على تخطيطه السابق.</para>
    /// </summary>
    /// <param name="form">نموذج التقرير — قد يكون null.</param>
    private static void BuildReportShell(Form? form)
    {
        if (form is null || form.IsDisposed) return;

        try
        {
            // (1) الهيكل مكتوب في المصمّم (design == runtime) ⇒ لا يُبنى مرة ثانية،
            //     لكن لوحة التحميل تبقى في المقدمة كما في المسار المبني (RULE-N79 أ).
            if (FindDirectChild(form, ShellRootControlName) is not null)
            {
                KeepLoadingOverlayOnTop(form);
                return;
            }

            // (2) لقطة عناصر التصفية بترتيبها البصري (RULE-N57) قبل أي نقل (Reparent).
            var filterControls = CollectDirectFilterControls(form);
            bool hasFilters = filterControls.Count > 0;

            // (2أ) ارتفاع صف التصفية حسب عدد عناصره: سطر واحد أو سطران ملتفّان (RULE-N56).
            int filtersRowHeight = hasFilters ? ResolveFiltersRowHeight(filterControls.Count) : 0;

            // (3) جدول الهيكل: عمود واحد وستة صفوف بلا هوامش ولا حشو.
            var root = new TableLayoutPanel
            {
                Name = ShellRootControlName,
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, ShellHeaderRowHeight));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, ShellToolbarRowHeight));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, filtersRowHeight));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // الشبكة تأخذ المساحة الباقية
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, ShellSummaryRowHeight));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, ShellFooterRowHeight));

            // (4) يُضاف أولاً ويُدفع إلى الخلف ليبقى أي عنصر غير معروف فوقه سليماً.
            form.Controls.Add(root);
            form.Controls.SetChildIndex(root, form.Controls.Count - 1);

            AddShellRow(root, BuildShellHeader(form), ShellHeaderRow);
            AddShellRow(root, BuildShellToolbar(form), ShellToolbarRow);
            AddShellRow(root, BuildShellFilters(form, filterControls), ShellFiltersRow);
            AddShellRow(root, BuildShellGrid(form), ShellGridRow);
            AddShellRow(root, BuildShellSummary(form), ShellSummaryRow);
            AddShellRow(root, BuildShellFooter(form), ShellFooterRow);

            // (5) لوحة التحميل تبقى ابناً مباشراً للنموذج وفوق الهيكل كله.
            KeepLoadingOverlayOnTop(form);

            // (6) حارس طبقات: أي عنصر يُضاف لاحقاً (شريط تنقل التقارير مثلاً) يبقى
            //     أمام الهيكل، فلا يُغطّيه الجدول ولا يختفي شريط أُضيف بعدنا.
            KeepShellBehindForeignControls(form, root);
        }
        catch (Exception ex)
        {
            // لا نُفشل فتح تقرير بسبب تخطيط: يبقى الشكل السابق وتُسجَّل المشكلة فقط.
            Serilog.Log.Error(ex, "تعذر بناء الهيكل المرجعي لتقرير: {Form}", form.GetType().Name);
        }
    }

    /// <summary>صف العنوان: <c>lblMode</c> ممتداً على الصف (RULE-N44 + RULE-N79 أ).</summary>
    private static Panel BuildShellHeader(Form form)
    {
        var panel = CreateShellPanel(ShellHeaderControlName);

        if (FindDirectChild(form, "lblMode") is Label header && !header.IsDisposed)
        {
            header.AutoSize = false;
            header.Dock = DockStyle.Fill;   // يُلغي المقاس المطلق القديم، ويُبقي Font/Text/TextAlign
            panel.Controls.Add(header);
        }

        return panel;
    }

    /// <summary>صف الأدوات: أزرار التحديث والتصدير داخل حاوية تدفّق من اليمين إلى اليسار.</summary>
    private static Panel BuildShellToolbar(Form form)
    {
        var panel = CreateShellPanel(ShellToolbarControlName);
        var flow = CreateShellFlow(ToolbarFlowControlName, wrapContents: false, padding: new Padding(8, 8, 8, 4));

        MoveDirectChild(form, "btnRefresh", flow);
        MoveDirectChild(form, "btnExport", flow);

        panel.Controls.Add(flow);
        return panel;
    }

    /// <summary>
    /// صف التصفية: <c>gbFilter</c> («بحث وتصفية») يستضيف عناصر التصفية وتسمياتها.
    /// تقرير بلا عناصر تصفية يُطوى صفّه (ارتفاع 0 + لوحة مخفية) بلا صندوق فارغ.
    /// </summary>
    private static Panel BuildShellFilters(Form form, List<Control> filterControls)
    {
        var panel = CreateShellPanel(FilterPanelControlName);

        if (filterControls.Count == 0)
        {
            panel.Visible = false;
            return panel;
        }

        var flow = CreateShellFlow(FilterFlowControlName, wrapContents: true, padding: new Padding(0, 4, 0, 0));

        foreach (var control in filterControls)
        {
            if (control is null || control.IsDisposed) continue;

            // التسميات تُعاد بـ AutoSize وتُحاذى لليمين؛ النص والخط والحجم والمعالجات لا تُمسّ.
            MoveControl(control, flow, autoSize: control is Label, alignRight: control is Label);
        }

        var groupBox = new GroupBox
        {
            Name = FilterGroupControlName,
            Text = FilterGroupCaption,
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
            RightToLeft = RightToLeft.Yes,
            Font = new Font("Tahoma", 9F, FontStyle.Bold)
        };
        groupBox.Controls.Add(flow);

        panel.Controls.Add(groupBox);
        return panel;
    }

    /// <summary>صف الشبكة: <c>dgvReport</c> ممتدة على الصف، وخصائصها وسلوكها كما هي (RULE-N52).</summary>
    private static Panel BuildShellGrid(Form form)
    {
        var panel = CreateShellPanel(ShellGridControlName);
        panel.Padding = new Padding(8);

        var grid = FindDirectChild(form, "dgvReport") as DataGridView ?? FindDirectGrid(form);
        if (grid is not null && !grid.IsDisposed)
        {
            grid.Dock = DockStyle.Fill;
            panel.Controls.Add(grid);
        }

        return panel;
    }

    /// <summary>صف الملخص: <c>lblSummary</c> ثم <c>label1</c> ثم <c>lblRecordsCount</c> (RULE-N61).</summary>
    private static Panel BuildShellSummary(Form form)
    {
        var panel = CreateShellPanel(ShellSummaryControlName);
        var flow = CreateShellFlow(SummaryFlowControlName, wrapContents: false, padding: new Padding(8, 4, 8, 4));

        foreach (var name in SummaryLabelNames)
            MoveDirectChild(form, name, flow, autoSize: true);

        panel.Controls.Add(flow);
        return panel;
    }

    /// <summary>صف التذييل: زر الإغلاق مع الحفاظ على <c>DialogResult</c> والمقاس والخط والمعالج.</summary>
    private static Panel BuildShellFooter(Form form)
    {
        var panel = CreateShellPanel(ShellFooterControlName);
        var flow = CreateShellFlow(FooterFlowControlName, wrapContents: false, padding: new Padding(8, 6, 8, 6));

        if (!MoveDirectChild(form, "btnClose", flow)
            && form.CancelButton is Button cancel
            && cancel.Parent == form)
        {
            // اسم زر الإغلاق غير معتاد ⇒ يُنقل زر الإلغاء الحقيقي كما هو.
            MoveControl(cancel, flow);
        }

        panel.Controls.Add(flow);
        return panel;
    }

    /// <summary>
    /// يُبقي <c>loadingPanel</c> ابناً مباشراً للنموذج ممتداً على كل المساحة وفي مقدمة
    /// العناصر (SetChildIndex = 0) ليغطي الهيكل كله. شفافيته و<see cref="Control.Visible"/>
    /// لا تُمسّان إطلاقاً.
    /// </summary>
    private static void KeepLoadingOverlayOnTop(Form form)
    {
        var overlay = FindDirectChild(form, LoadingPanelControlName);
        if (overlay is null || overlay.IsDisposed) return;

        overlay.Dock = DockStyle.Fill;
        form.Controls.SetChildIndex(overlay, 0);   // 0 = المقدمة
    }

    /// <summary>
    /// يحافظ على ترتيب الطبقات بعد بناء الهيكل، مع مراعاة **ترتيب الإرساء** الذي يعتمده
    /// WinForms: المراسي (Dock) تُثبَّت من **آخر** عناصر <see cref="Control.ControlCollection"/>
    /// إلى أولها. لذلك القاعدة:
    /// <list type="bullet">
    /// <item>عنصر يُضاف **غير مراسٍ** (<c>Dock == None</c> — عنصر مطلق فوق الهيكل):
    /// يُدفع <c>pnlReportRoot</c> إلى الخلف فيراه المستخدم فوق الجدول.</item>
    /// <item>عنصر يُضاف **مراسياً** (شريط تنقل التقارير <c>pnlReportNav</c> بـ
    /// <c>Dock = Bottom</c> الذي يضيفه <c>ReportNavigationHelper</c> بعد هذه الأداة):
    /// **لا** يُعاد الترتيب — يبقى الترتيب الطبيعي <c>[التحميل(0)، الهيكل(1)، الشريط(2)]</c>،
    /// فيحتل الشريط حافة الأسفل أولاً ثم يتقلّص الجدول الممتد (Fill) فوقه. لو دُفع الجدول
    /// إلى الخلف بعد إضافة الشريط لصار الترتيب <c>[التحميل(0)، الشريط(1)، الهيكل(2)]</c>،
    /// ولَرُسم الجدول الممتد على كامل مساحة العميل ثم طُلي عليه الشريط آخر 58 بكسل —
    /// وهي بالضبط منطقة <c>pnlFooter</c> التي يقطنها <c>btnClose</c> فيختفي زر الإغلاق.</item>
    /// </list>
    /// ولوحة التحميل تبقى في المقدمة (فهرس 0) في الحالتين — فلا يختفي شريط أُضيف بعدنا
    /// ولا يغطّي الجدول زراً، وتبقى الشفافية كما هي (RULE-N79 أ).
    /// </summary>
    private static void KeepShellBehindForeignControls(Form form, Control root)
    {
        try
        {
            form.ControlAdded += (_, e) =>
            {
                try
                {
                    if (root.IsDisposed || form.IsDisposed) return;

                    // إعادة ترتيب الهيكل مقصورة على العناصر **غير المراسية**: العنصر المراسي
                    // يُرسى من آخر المجموعة فيأخذ حافته أولاً ويتقلّص الجدول فوقه تلقائياً،
                    // أما دفع الجدول خلفه فيجعل الجدول الممتد يغطّي حافة الشريط (RULE-N79 أ).
                    if (!ReferenceEquals(e.Control, root)
                        && e.Control.Dock == DockStyle.None
                        && form.Controls.Contains(root))
                    {
                        form.Controls.SetChildIndex(root, form.Controls.Count - 1);   // الهيكل في الخلف
                    }

                    var overlay = FindDirectChild(form, LoadingPanelControlName);
                    if (overlay is not null && !overlay.IsDisposed && !ReferenceEquals(overlay, e.Control))
                        form.Controls.SetChildIndex(overlay, 0);                      // التحميل في المقدمة
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error(ex, "تعذر ترتيب طبقات هيكل تقرير: {Form}", form.GetType().Name);
                }
            };
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر توصيل حارس طبقات هيكل تقرير: {Form}", form.GetType().Name);
        }
    }

    /// <summary>لوحة صف في الهيكل: ممتلئة الصف وبلا هامش حتى لا يبقى فراغ حولها.</summary>
    private static Panel CreateShellPanel(string name) => new()
    {
        Name = name,
        Dock = DockStyle.Fill,
        Margin = new Padding(0)
    };

    /// <summary>حاوية تدفّق من اليمين إلى اليسار (RULE-N20) لعناصر صف واحد من الهيكل.</summary>
    private static FlowLayoutPanel CreateShellFlow(string name, bool wrapContents, Padding padding) => new()
    {
        Name = name,
        Dock = DockStyle.Fill,
        FlowDirection = FlowDirection.RightToLeft,
        WrapContents = wrapContents,
        Padding = padding,
        Margin = new Padding(0)
    };

    /// <summary>يضع لوحة الصف في صفّها من الجدول (عمود واحد فقط).</summary>
    private static void AddShellRow(TableLayoutPanel root, Control panel, int row)
    {
        if (root is null || panel is null || root.IsDisposed || panel.IsDisposed) return;

        root.Controls.Add(panel);
        root.SetRow(panel, row);
        root.SetColumn(panel, 0);
    }

    /// <summary>
    /// ارتفاع صف التصفية حسب عدد عناصره (RULE-N56): الثابت المعتاد (96) لثلاثة عناصر
    /// أو أقل، والثابت الأعلى (120) عند تجاوزها لأن الحاوية تلتف إلى سطر ثانٍ.
    /// </summary>
    private static int ResolveFiltersRowHeight(int filterControlCount)
        => filterControlCount > ShellFiltersSingleLineMax
            ? ShellFiltersRowHeightTall
            : ShellFiltersRowHeight;

    /// <summary>
    /// يجمع عناصر التصفية **المباشرة** في النموذج بترتيبها **البصري** لا بترتيب التصريح
    /// (RULE-N57): الصفوف من أعلى إلى أسفل، وداخل الصف **من اليمين إلى اليسار** — فتلتصق
    /// كل تسمية («من:» «إلى:» «بتاريخ:» «النوع:» …) بحقلها في حاوية التدفّق، وتبقى صفوف
    /// شريط التصفية المتعدد كما هي. تُقرأ <see cref="Control.Top"/>/<see cref="Control.Left"/>
    /// وقت الجمع — **قبل** أي نقل (Reparent) — وتُحفظ في لقطة محلّية.
    /// <para>الصفوف تُجمَّع بسماحية رأسية <see cref="RowTopTolerance"/> لأن التسمية في
    /// المصمّم تهبط بضعة بكسلات عن حقلها؛ بلاه كانت الحقول تُرتَّب قبل تسمياتها فتنقلب
    /// التسمية إلى يسار حقلها في تدفّق RTL. التسميات المحجوزة (العنوان/الملخص/التحميل)
    /// لا تُجمع.</para>
    /// </summary>
    private static List<Control> CollectDirectFilterControls(Control host)
    {
        // اللقطة: العنصر + موضعه البصري الأصلي قبل أن ينتقل إلى الحاوية الجديدة.
        var visual = new List<(Control Control, int Top, int Left)>();

        try
        {
            foreach (Control child in host.Controls)
            {
                if (child is null || child.IsDisposed) continue;

                if (IsFilterInput(child))
                {
                    visual.Add((child, child.Top, child.Left));
                    continue;
                }

                if (child is Label label && !IsShellReservedLabel(label.Name))
                    visual.Add((label, label.Top, label.Left));
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر جمع عناصر تصفية التقرير: {Host}", host?.GetType().Name);
        }

        // الترتيب البصري: تجميع في صفوف بسماحية رأسية أولاً، ثم يمين ← يسار داخل الصف،
        // فتُضاف التسمية قبل حقلها مباشرةً مهما كان فرق البكسلات بين إحداثيّي Top.
        var ordered = new List<Control>();
        var row = new List<(Control Control, int Top, int Left)>();
        int currentRowTop = 0;

        foreach (var item in visual.OrderBy(cell => cell.Top))
        {
            if (row.Count == 0)
            {
                currentRowTop = item.Top;   // أول عنصر في الصف يضبط خطّ أعلاه
            }
            else if (item.Top - currentRowTop > RowTopTolerance)
            {
                // نزلنا عن خط الصف الحالي بسماحية صغيرة ⇒ الصف السابق انتهى: يُفرَّغ
                // مرتّباً من اليمين إلى اليسار (RTL: أقصى اليمين أولاً — RULE-N57).
                ordered.AddRange(row.OrderByDescending(cell => cell.Left).Select(cell => cell.Control));
                row.Clear();
                currentRowTop = item.Top;   // الصف الجديد يبدأ بخطّه الأعلى
            }

            row.Add(item);
        }

        if (row.Count > 0)
            ordered.AddRange(row.OrderByDescending(cell => cell.Left).Select(cell => cell.Control));

        return ordered;
    }

    /// <summary>
    /// تسميات لا تدخل صندوق التصفية: شريط العنوان (<c>lblMode</c>…) وشريط الملخص
    /// وتسمية لوحة التحميل.
    /// </summary>
    private static bool IsShellReservedLabel(string? name)
    {
        if (string.IsNullOrEmpty(name)) return false;

        return HeaderLabelNames.Any(n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase))
            || SummaryLabelNames.Any(n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase))
            || OverlayLabelNames.Any(n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>ينقل ابناً مباشراً للنموذج بالاسم إلى الحاوية الهدف — يُرجع false إن لم يوجد.</summary>
    private static bool MoveDirectChild(Form form, string name, Control destination, bool autoSize = false)
    {
        var child = FindDirectChild(form, name);
        if (child is null || child.IsDisposed) return false;

        return MoveControl(child, destination, autoSize, alignRight: autoSize);
    }

    /// <summary>
    /// ينقل عنصراً قائماً إلى حاوية جديدة (Reparent): يُلغي الـAnchor المطلق لأنه لا
    /// مواضع مطلقة في الهيكل، ويحفظ النص والخط والمقاس والمعالجات و<see cref="Control.TabIndex"/>.
    /// </summary>
    private static bool MoveControl(Control control, Control destination, bool autoSize = false, bool alignRight = false)
    {
        if (control is null || control.IsDisposed) return false;
        if (destination is null || destination.IsDisposed) return false;

        control.Anchor = AnchorStyles.Top | AnchorStyles.Left;

        if (autoSize && control is Label label)
        {
            label.AutoSize = true;
            if (alignRight) label.TextAlign = ContentAlignment.MiddleRight;
        }

        destination.Controls.Add(control);   // الإضافة إلى حاوية جديدة تنقل العنصر من حاويته القديمة
        return true;
    }

    /// <summary>يبحث عن ابن مباشر للنموذج بالاسم (بلا استثناء).</summary>
    private static Control? FindDirectChild(Control parent, string name)
    {
        try
        {
            foreach (Control child in parent.Controls)
            {
                if (string.Equals(child.Name, name, StringComparison.OrdinalIgnoreCase)) return child;
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر البحث عن عنصر مباشر في {Parent}", parent?.GetType().Name);
        }

        return null;
    }

    /// <summary>أول شبكة مباشرة في النموذج — احتياط إن كان اسم الشبكة غير <c>dgvReport</c>.</summary>
    private static DataGridView? FindDirectGrid(Control parent)
    {
        try
        {
            foreach (Control child in parent.Controls)
            {
                if (child is DataGridView grid && !grid.IsDisposed) return grid;
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر البحث عن شبكة تقرير مباشرة في {Parent}", parent?.GetType().Name);
        }

        return null;
    }

    // ═════════════════════════════════════════════════════════════════
    //  التنسيق: شبكات + خطوط
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يمشي على شجرة العناصر ويُطبّق الشبكة الخضراء وخط Tahoma على ما يلمسه.</summary>
    private static void StyleTree(Control parent, bool skipAppliedSubtrees)
    {
        try
        {
            StyleControl(parent);

            foreach (Control child in parent.Controls)
            {
                if (skipAppliedSubtrees && IsApplied(child)) continue;
                StyleTree(child, skipAppliedSubtrees);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تنسيق عنصر تقرير: {Control}", parent?.GetType().Name);
        }
    }

    /// <summary>يُطبّق ثيم الشبكة الأخضر ويوحّد خط العنصر حسب نوعه.</summary>
    private static void StyleControl(Control control)
    {
        if (control is null) return;

        try
        {
            switch (control)
            {
                case DataGridView grid:
                    // الخط قبل الثيم: ApplyGrid يضبط خط الخلايا/الرؤوس (Tahoma 10F) فيبقى هو الحاكم.
                    ApplyTahoma(grid, 9F);
                    GreenThemeHelper.ApplyGrid(grid);   // يتجاوز ThemeHelper.ApplyDataGridViewStyle (RULE-N51)
                    break;

                case Label label:
                    ApplyTahoma(label, 9F);

                    // تسمية لوحة التحميل (ll / loadingLabel): لونها من الثيم وقت التشغيل
                    // وحده — لا يُعتمد على لون المصمّم، لأن ملفات *.Designer.cs تُعاد
                    // إليها القيمة القديمة عند أي دوران للمصمّم (RULE-N54 / RULE-N112).
                    if (!string.IsNullOrEmpty(label.Name)
                        && OverlayLabelNames.Any(n => string.Equals(n, label.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        label.ForeColor = GreenThemeHelper.TextStrong;
                    }

                    // ترحيل اللون القديم: الأحمر الطوبي (Firebrick) كان يُستخدم في
                    // تسميات «جارٍ التحميل...» القديمة — يُردّ إلى أخضر الثيم الداكن
                    // (RULE-N54). لا يُمسّ أي تسمية بلون آخر، ولا يُغيَّر أي نص.
                    if (label.ForeColor.ToArgb() == Color.Firebrick.ToArgb())
                        label.ForeColor = GreenThemeHelper.TextStrong;
                    break;

                case Button button:
                    ApplyTahoma(button, 10F);
                    break;
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق خط Tahoma على عنصر تقرير");
        }
    }

    /// <summary>
    /// يفرض عائلة Tahoma على العنصر. الحجم يُثبَّت على <paramref name="defaultSize"/>
    /// فقط عند التحويل من عائلة أخرى (Segoe UI)، وإلا يبقى الحجم المقصود كما هو.
    /// </summary>
    private static void ApplyTahoma(Control control, float defaultSize)
    {
        try
        {
            var font = control.Font;
            if (font is null) return;

            if (string.Equals(font.FontFamily.Name, "Tahoma", StringComparison.OrdinalIgnoreCase))
                return;   // الخط صحيح — لا نلمس الحجم (RULE-N53: أحجام العناوين تبقى كما ضبطها المصمّم)

            control.Font = new Font("Tahoma", defaultSize, font.Style);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر فرض خط Tahoma على عنصر تقرير");
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  لوحة المفاتيح (RULE-N40 … RULE-N46)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يُوصِّل الأربعة معاً مرة واحدة فقط لكل جذر (RULE-N40).</summary>
    private static void WireKeyboard(Control root)
    {
        try
        {
            if (WiredTrees.TryGetValue(root, out _)) return;

            KeyboardHelper.WireEnterAsTab(root);
            KeyboardHelper.WireSelectAllOnFocus(root);          // RULE-N41
            // RULE-N46 / RULE-N54b: تُمرَّر القيمة الافتراضية للمفتاح فيقرأ الحقل
            // القيمة الحيّة من ThemePalette.ActiveFieldColor عند كل دخول.
            KeyboardHelper.WireFocusHighlight(root, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor));
            KeyboardHelper.WireArrowNavigation(root);           // RULE-N45

            WiredTrees.Add(root, AppliedMarker);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر توصيل تنقل لوحة المفاتيح في تقرير: {Root}", root?.GetType().Name);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  صندوق التصفية الموحّد (RULE-N56 … RULE-N62)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يجمع عناصر تصفية التقرير في صندوق مجموعة واحد <c>gbFilter</c> («بحث وتصفية»)
    /// داخل صفّ جديد أعلى الشبكة، ويترك أزرار الإجراءات في مكانها خارج الصندوق
    /// (RULE-N56 … RULE-N62). ينقل العناصر (Reparent) ولا يُعيد إنشاءها، فلا تُفقد
    /// أي معالجات أحداث ولا مراجع في كود التقرير.
    /// <para>حدود آمنة: لا يفعل شيئاً إذا لم تُطابق بنية التقرير المتوقعة، أو كانت
    /// عناصر التصفية أقل من عنصرين، أو كان الصندوق موجوداً مسبقاً.</para>
    /// </summary>
    private static void EnsureFilterGroup(Control root)
    {
        try
        {
            if (root is null) return;

            // (6) لا تُنفَّذ مرتين على الشجرة نفسها.
            if (FilterGrouped.TryGetValue(root, out _)) return;

            // (3) الصندوق موجود مسبقاً ⇒ لا شيء يُفعل.
            if (FindChildByName(root, FilterGroupControlName) is not null) return;

            // (1) جدول التخطيط الذي يستضيف شريط الإجراءات في صفّه الأول.
            var layout = FindReportLayout(root);
            if (layout is null) return;

            var host = FindFilterHost(layout);
            if (host is null) return;

            // (2) أقل من عنصرين ⇒ التقرير يبقى على تخطيطه الحالي.
            var filterControls = CollectFilterControls(host);
            if (filterControls.Count < MinFilterControls) return;

            BuildFilterGroup(layout, host, filterControls);
        }
        catch (Exception ex)
        {
            // لا نُفشل فتح تقرير بسبب تخطيط: نسجّل الخطأ فقط وتبقى الشاشة كما هي.
            Serilog.Log.Error(ex, "تعذر إنشاء صندوق التصفية الموحّد في تقرير: {Root}", root?.GetType().Name);
        }
        finally
        {
            MarkFilterGrouped(root);
        }
    }

    /// <summary>هل هذا العنصر حقل تصفية (لا تسمية)؟</summary>
    private static bool IsFilterInput(Control control) => control switch
    {
        DateTimePicker => true,
        ComboBox => true,
        TextBox => true,
        CheckBox => true,
        RadioButton => true,
        NumericUpDown => true,
        _ => false
    };

    /// <summary>يبحث عن عنصر بالاسم داخل الشجرة (بلا استثناء).</summary>
    private static Control? FindChildByName(Control parent, string name)
    {
        foreach (Control child in parent.Controls)
        {
            if (string.Equals(child.Name, name, StringComparison.OrdinalIgnoreCase)) return child;

            if (child.HasChildren)
            {
                var nested = FindChildByName(child, name);
                if (nested is not null) return nested;
            }
        }

        return null;
    }

    /// <summary>
    /// يجد جدول التخطيط المرجعي للتقارير: صفوفه ثلاثة على الأقل، وصفّه الأول
    /// يستضيف حاوية تضم عناصر تصفية (شريط الأدوات).
    /// </summary>
    private static TableLayoutPanel? FindReportLayout(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            if (child is TableLayoutPanel layout && layout.RowCount >= 3 && FindFilterHost(layout) is not null)
                return layout;

            if (child.HasChildren)
            {
                var nested = FindReportLayout(child);
                if (nested is not null) return nested;
            }
        }

        return null;
    }

    /// <summary>يبحث عن حاوية عناصر التصفية داخل صفوف الجدول الأول (Row = 0).</summary>
    private static Control? FindFilterHost(TableLayoutPanel layout)
    {
        foreach (Control child in layout.Controls)
        {
            if (layout.GetRow(child) != 0) continue;

            var host = FindFilterHostIn(child);
            if (host is not null) return host;
        }

        return null;
    }

    /// <summary>
    /// الحاوية الأعمق التي تضم عناصر التصفية **مباشرةً** (شريط الأدوات عادةً).
    /// لا نُعمّق البحث داخل حقول التصفية نفسها — <c>NumericUpDown</c> له مربع نص داخلي
    /// كان سيُحسب عنصر تصفية مستقلاً.
    /// </summary>
    private static Control? FindFilterHostIn(Control container)
    {
        if (!IsFilterInput(container))
        {
            foreach (Control child in container.Controls)
            {
                if (!child.HasChildren) continue;

                var nested = FindFilterHostIn(child);
                if (nested is not null) return nested;
            }
        }

        return CollectFilterControls(container).Count >= MinFilterControls ? container : null;
    }

    /// <summary>
    /// عناصر التصفية المباشرة داخل الحاوية بترتيب العرض الحالي: حقول الإدخال
    /// متبوعةً بتسمياتها كما وردت في <see cref="Control.ControlCollection"/>.
    /// التسمية (<see cref="Label"/>) تُعدّ عنواناً لحقل لأن الحاوية تضم حقلاً بالفعل.
    /// </summary>
    private static List<Control> CollectFilterControls(Control host)
    {
        var result = new List<Control>();

        bool hasInput = false;
        foreach (Control child in host.Controls)
        {
            if (!IsFilterInput(child)) continue;
            hasInput = true;
            break;
        }

        if (!hasInput) return result;

        foreach (Control child in host.Controls)
        {
            if (IsFilterInput(child) || child is Label) result.Add(child);
        }

        return result;
    }

    /// <summary>
    /// ينشئ <c>pnlFilters</c> + <c>gbFilter</c> + حاوية تدفّق، وينقل عناصر التصفية
    /// إليها بالترتيب والهوامش والمقاسات نفسها، ثم يُدخلها كصفّ جديد رقم 1.
    /// </summary>
    private static void BuildFilterGroup(TableLayoutPanel layout, Control host, List<Control> filterControls)
    {
        var flow = new FlowLayoutPanel
        {
            Name = FilterFlowControlName,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = true,
            Margin = new Padding(0)
        };

        var groupBox = new GroupBox
        {
            Name = FilterGroupControlName,
            Text = FilterGroupCaption,
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
            RightToLeft = RightToLeft.Yes,
            Font = new Font("Tahoma", 9F, FontStyle.Bold)
        };
        groupBox.Controls.Add(flow);

        // نقل (Reparent) — يحفظ المعالجات والمراجع والهوامش والمقاسات كما هي.
        foreach (var control in filterControls)
        {
            switch (control)
            {
                case CheckBox check:
                    check.AutoSize = true;
                    break;

                case Label label:
                    label.AutoSize = true;
                    label.TextAlign = ContentAlignment.MiddleRight;
                    break;
            }

            flow.Controls.Add(control);   // الإضافة إلى حاوية جديدة تنقل العنصر من حاويته القديمة
        }

        var panel = new Panel
        {
            Name = FilterPanelControlName,
            Dock = DockStyle.Top,
            Height = FilterPanelHeight,
            Margin = new Padding(0)
        };
        panel.Controls.Add(groupBox);

        InsertFilterRow(layout, panel);
    }

    /// <summary>
    /// يُدخل لوحة التصفية كصفّ جديد رقم 1: يزيد عدد الصفوف، يُزحزح صفوف العناصر
    /// القائمة بمقدار واحد، يُدرج <see cref="RowStyle"/> مطلق بارتفاع الصف الجديد،
    /// ثم يضع اللوحة في العمود 0 (ممتدة على كل الأعمدة إن كان الجدول متعدد الأعمدة).
    /// </summary>
    private static void InsertFilterRow(TableLayoutPanel layout, Control panel)
    {
        const int newRowIndex = 1;

        layout.RowCount += 1;

        var existingChildren = new List<Control>();
        foreach (Control child in layout.Controls) existingChildren.Add(child);

        foreach (var child in existingChildren)
        {
            int row = layout.GetRow(child);
            if (row >= newRowIndex) layout.SetRow(child, row + 1);
        }

        InsertRowStyle(layout, newRowIndex, FilterPanelHeight);

        layout.Controls.Add(panel);
        layout.SetRow(panel, newRowIndex);
        layout.SetColumn(panel, 0);

        if (layout.ColumnCount > 1)
            layout.SetColumnSpan(panel, layout.ColumnCount);
    }

    /// <summary>
    /// يُدرج <see cref="RowStyle"/> في موضع محدّد. <c>RowStyleCollection</c> لا يوفّر
    /// <c>Insert</c>، فتُعاد بناء القائمة بأنماطها الأصلية وترتيبها ثم يُضاف النمط الجديد
    /// في موضعه، مع مزامنة العدد مع <see cref="TableLayoutPanel.RowCount"/>.
    /// </summary>
    private static void InsertRowStyle(TableLayoutPanel layout, int index, int height)
    {
        var existing = new List<RowStyle>();
        foreach (RowStyle style in layout.RowStyles)
            existing.Add(new RowStyle(style.SizeType, style.Height));

        if (index < 0) index = 0;
        if (index > existing.Count) index = existing.Count;

        layout.RowStyles.Clear();

        for (int i = 0; i < existing.Count; i++)
        {
            if (i == index) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
            layout.RowStyles.Add(existing[i]);
        }

        if (index >= existing.Count) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, height));

        while (layout.RowStyles.Count < layout.RowCount)
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
    }

    /// <summary>يسجّل الشجرة كمعالَجة حتى لا يُنشأ الصندوق مرتين.</summary>
    private static void MarkFilterGrouped(Control root)
    {
        if (root is null) return;

        try
        {
            if (!FilterGrouped.TryGetValue(root, out _))
                FilterGrouped.Add(root, AppliedMarker);
        }
        catch
        {
            // مفتاح مُضاف بالفعل — لا شيء يُفعل.
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  عنوان النموذج + اختصار التحديث
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يُلوّن تسمية العنوان العلوية إن وُجدت، ويتجاهل بصمت إن لم توجد.</summary>
    private static void ApplyTitleHeader(Form form)
    {
        try
        {
            var header = FindTitleLabel(form);
            if (header is not null)
                GreenThemeHelper.ApplyHeader(header);   // RULE-N53
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق ثيم شريط عنوان التقرير");
        }
    }

    /// <summary>يبحث عن تسمية عنوان: بالاسم المعتمد أولاً، ثم أول تسمية ملتصقة بأعلى الحاوية.</summary>
    private static Label? FindTitleLabel(Control parent)
    {
        Label? firstTopDocked = null;

        foreach (Control child in parent.Controls)
        {
            if (child is Label lbl)
            {
                if (!string.IsNullOrEmpty(lbl.Name)
                    && HeaderLabelNames.Any(n => string.Equals(n, lbl.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    return lbl;
                }

                if (firstTopDocked is null && lbl.Dock == DockStyle.Top && !string.IsNullOrWhiteSpace(lbl.Text))
                    firstTopDocked = lbl;
            }

            if (child.HasChildren)
            {
                var nested = FindTitleLabel(child);
                if (nested is not null) return nested;
            }
        }

        return firstTopDocked;
    }

    // ═════════════════════════════════════════════════════════════════
    //  أزرار إجراءات التقرير (RULE-N50 … RULE-N54 + RULE-N112)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يُلوّن أزرار الإجراءات الثلاثة الموجودة في كل التقارير الـ31 — <c>btnRefresh</c>
    /// و<c>btnExport</c> و<c>btnClose</c> — من لوحة <see cref="GreenThemeHelper"/> وحدها
    /// (RULE-N112: لا أرقام لون داخل الشاشة). لا يُلمس المقاس ولا الموضع ولا الـAnchor
    /// ولا عائلة/حجم الخط ولا النص ولا التبويب ولا <c>DialogResult</c> ولا أي معالج حدث.
    /// <para>يُستثنى شريط تنقل التقارير (<c>pnlReportNav</c> وأزرار <c>btnNav…</c>)
    /// لأنه يُنسّق نفسه في <c>ReportNavigationHelper</c>.</para>
    /// </summary>
    private static void StyleActionButtons(Form form)
    {
        try
        {
            if (form is null) return;

            foreach (var button in EnumerateButtons(form))
            {
                if (button is null || button.IsDisposed) continue;
                if (IsInReportNavStrip(button)) continue;

                var colors = ResolveActionButtonColors(button.Name);
                if (colors is null) continue;   // ليس زر إجراء تقرير معروفاً ⇒ يُترك كما هو

                ApplyActionButtonStyle(button, colors.Value.Back, colors.Value.Fore);
            }
        }
        catch (Exception ex)
        {
            // لا نُفشل فتح تقرير بسبب لون زر: نسجّل الخطأ فقط.
            Serilog.Log.Error(ex, "تعذر تنسيق أزرار إجراءات التقرير: {Form}", form?.GetType().Name);
        }
    }

    /// <summary>
    /// ألوان زر الإجراء حسب اسمه من ثوابت <see cref="GreenThemeHelper"/> فقط،
    /// أو <c>null</c> إن لم يكن من أزرار التقارير المعروفة.
    /// </summary>
    private static (Color Back, Color Fore)? ResolveActionButtonColors(string? name)
    {
        if (string.IsNullOrEmpty(name)) return null;

        if (string.Equals(name, "btnRefresh", StringComparison.OrdinalIgnoreCase))
            return (GreenThemeHelper.HeaderBackground, GreenThemeHelper.HeaderForeground);

        if (string.Equals(name, "btnExport", StringComparison.OrdinalIgnoreCase))
            return (GreenThemeHelper.GroupBackground, GreenThemeHelper.TextStrong);

        if (string.Equals(name, "btnClose", StringComparison.OrdinalIgnoreCase))
            return (GreenThemeHelper.BarBackground, GreenThemeHelper.TextStrong);

        return null;
    }

    /// <summary>يطبّق ألوان زر الإجراء بحدود مسطّحة متناسقة مع الثيم الأخضر.</summary>
    private static void ApplyActionButtonStyle(Button button, Color backColor, Color foreColor)
    {
        try
        {
            button.BackColor = backColor;
            button.ForeColor = foreColor;

            // FlatStyle.Flat + إلغاء VisualStyles حتى يظهر اللون المعرَّف كما هو،
            // لأن النمط المرئي للنظام يمحو BackColor (نفس منطق RULE-N50).
            button.UseVisualStyleBackColor = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = GreenThemeHelper.HeaderBackground;
            button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.1f);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تنسيق زر إجراء تقرير: {Button}", button?.Name);
        }
    }

    /// <summary>
    /// هل الزر جزء من شريط تنقل التقارير؟ (اسمه يبدأ بـ <c>btnNav</c> أو أحد آبائه
    /// <c>pnlReportNav</c>) — الشريط يُنسّق نفسه في <c>ReportNavigationHelper</c>.
    /// </summary>
    private static bool IsInReportNavStrip(Control? control)
    {
        if (control is null) return false;

        try
        {
            if (!string.IsNullOrEmpty(control.Name)
                && control.Name.StartsWith("btnNav", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            for (var parent = control.Parent; parent is not null; parent = parent.Parent)
            {
                if (string.Equals(parent.Name, "pnlReportNav", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر فحص انتماء زر تقرير لشريط التنقل");
        }

        return false;
    }

    /// <summary>يُوصِّل F5 إلى زر التحديث المرئي إن أمكن العثور عليه (RULE-N47).</summary>
    private static void WireRefreshShortcut(Form form)
    {
        try
        {
            var refresh = FindRefreshButton(form);
            if (refresh is null)
            {
                // لا يوجد زر تحديث يمكن اكتشافه: نتخطى F5 بدل ربطه بإجراء عشوائي.
                Serilog.Log.Information("لا يوجد زر تحديث في {Form} — لم يُوصَّل اختصار F5", form.GetType().Name);
                return;
            }

            KeyboardHelper.WireGlobalShortcuts(form, (Keys.F5, () => RaiseRefresh(refresh)));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر توصيل اختصار F5 في تقرير: {Form}", form.GetType().Name);
        }
    }

    /// <summary>يجد زر التحديث: بالاسم <c>btnRefresh</c> ثم بنص يحتوي «تحديث».</summary>
    private static Button? FindRefreshButton(Control parent)
    {
        Button? byText = null;

        foreach (Control child in parent.Controls)
        {
            if (child is Button btn)
            {
                if (string.Equals(btn.Name, "btnRefresh", StringComparison.OrdinalIgnoreCase))
                    return btn;

                if (byText is null && !string.IsNullOrEmpty(btn.Text) && btn.Text.Contains("تحديث"))
                    byText = btn;
            }

            if (child.HasChildren)
            {
                var nested = FindRefreshButton(child);
                if (nested is not null) return nested;
            }
        }

        return byText;
    }

    /// <summary>ينقر زر التحديث برمجياً بلا استثناءات (يُتجاهل الزر المعطّل).</summary>
    private static void RaiseRefresh(Button button)
    {
        try
        {
            if (button.IsDisposed || !button.Enabled) return;
            button.PerformClick();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تنفيذ تحديث التقرير من اختصار F5");
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  اختصارات الإغلاق والتصدير (Esc / Ctrl+E) — نماذج التقارير فقط
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يُوصِّل <c>Esc</c> لإغلاق التقرير و<c>Ctrl+E</c> لزر التصدير في نداء واحد
    /// على <see cref="KeyboardHelper.WireGlobalShortcuts"/> — حتى لا يتضاعف
    /// المعالج، ولأن <c>F5</c> سبق توصيله في <see cref="WireRefreshShortcut"/>.
    /// <para>Esc يُوصَّل فقط إن وُجد زر إغلاق؛ وCtrl+E فقط إن وُجد زر تصدير.</para>
    /// </summary>
    private static void WireCloseAndExportShortcuts(Form form)
    {
        try
        {
            var shortcuts = new List<(Keys Key, Action Action)>();

            if (FindCloseButton(form) is Button closeButton)
                shortcuts.Add((Keys.Escape, () => RaiseClose(form, closeButton)));

            if (FindExportButton(form) is Button exportButton)
                shortcuts.Add((Keys.Control | Keys.E, () => RaiseExport(exportButton)));

            if (shortcuts.Count == 0)
            {
                Serilog.Log.Information(
                    "لا يوجد زر إغلاق/تصدير في {Form} — لم تُوصَّل اختصارات Esc/Ctrl+E",
                    form.GetType().Name);
                return;
            }

            KeyboardHelper.WireGlobalShortcuts(form, shortcuts.ToArray());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر توصيل اختصارات الإغلاق والتصدير في تقرير: {Form}", form.GetType().Name);
        }
    }

    /// <summary>
    /// يغلق التقرير عبر زر الإغلاق الحقيقي (فيعمل منطق النموذج كما هو). لا يُنفَّذ
    /// أثناء حوار مفتوح فوق التقرير: في تلك الحالة لا يكون للنموذج تركيز والضغط
    /// يذهب إلى الحوار، والفحص هنا احتياط إضافي.
    /// </summary>
    private static void RaiseClose(Form form, Button closeButton)
    {
        try
        {
            if (form.IsDisposed || closeButton.IsDisposed) return;
            if (!form.ContainsFocus) return;   // حوار مفتوح فوق التقرير ⇒ لا نُغلق تحته
            if (!closeButton.Enabled) return;  // التقرير في حالة تحميل ⇒ لا إغلاق

            closeButton.PerformClick();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر إغلاق التقرير من اختصار Esc: {Form}", form.GetType().Name);
        }
    }

    /// <summary>ينقر زر التصدير برمجياً — يُتجاهل الزر المعطّل/المحذوف.</summary>
    private static void RaiseExport(Button export)
    {
        try
        {
            if (export.IsDisposed || !export.Enabled) return;
            export.PerformClick();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تنفيذ تصدير التقرير من اختصار Ctrl+E");
        }
    }

    /// <summary>يجد زر الإغلاق: <c>CancelButton</c> ثم بالاسم <c>btnClose</c> ثم بنص «إغلاق».</summary>
    private static Button? FindCloseButton(Form form)
    {
        if (form.CancelButton is Button cancel) return cancel;

        Button? byText = null;

        foreach (var button in EnumerateButtons(form))
        {
            if (string.Equals(button.Name, "btnClose", StringComparison.OrdinalIgnoreCase))
                return button;

            if (byText is null && !string.IsNullOrEmpty(button.Text) && button.Text.Contains("إغلاق"))
                byText = button;
        }

        return byText;
    }

    /// <summary>يجد زر التصدير: بالاسم <c>btnExport</c> ثم بنص يحتوي «تصدير».</summary>
    private static Button? FindExportButton(Form form)
    {
        Button? byText = null;

        foreach (var button in EnumerateButtons(form))
        {
            if (string.Equals(button.Name, "btnExport", StringComparison.OrdinalIgnoreCase))
                return button;

            if (byText is null && !string.IsNullOrEmpty(button.Text) && button.Text.Contains("تصدير"))
                byText = button;
        }

        return byText;
    }

    /// <summary>يُعدّ كل الأزرار في شجرة النموذج (بلا استثناء عند شجرة غريبة).</summary>
    private static List<Button> EnumerateButtons(Control parent)
    {
        var buttons = new List<Button>();

        try
        {
            foreach (Control child in parent.Controls)
            {
                if (child is Button btn) buttons.Add(btn);
                if (child.HasChildren) buttons.AddRange(EnumerateButtons(child));
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تعداد أزرار تقرير: {Control}", parent?.GetType().Name);
        }

        return buttons;
    }

    // ═════════════════════════════════════════════════════════════════
    //  سلوك شبكات التقارير (RULE-N22)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// RULE-N22 لشبكات التقارير: عرض فقط. تُضبط خصائص السلوك هنا لأنها ليست من
    /// عمل <see cref="GreenThemeHelper.ApplyGrid"/> (RULE-N52 لا يُمسّ)، ولا تُطبَّق
    /// إلا من <see cref="ApplyForm"/> — أي على نماذج التقارير وحدها، فلا تصل إلى
    /// شبكات الإدخال الاستثنائية (dgvItems / سلة POS) لأنها ليست تقارير (RULE-N22a).
    /// </summary>
    private static void EnforceReportGridRules(Control root)
    {
        try
        {
            foreach (Control child in root.Controls)
            {
                if (child is DataGridView grid && !grid.IsDisposed)
                {
                    grid.ReadOnly = true;
                    grid.AllowUserToAddRows = false;
                    grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }

                if (child.HasChildren)
                    EnforceReportGridRules(child);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر فرض سلوك عرض شبكات التقرير: {Root}", root?.GetType().Name);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  علامات "تم التوحيد"
    // ═════════════════════════════════════════════════════════════════

    /// <summary>هل وُحِّد هذا الجذر سابقاً؟</summary>
    private static bool IsApplied(Control control)
    {
        if (control is null) return true;

        try
        {
            if (Applied.TryGetValue(control, out _)) return true;
            return control.Tag is string tag && string.Equals(tag, AppliedTag, StringComparison.Ordinal);
        }
        catch
        {
            return true;   // في حال تعذّر الفحص: لا نُعيد التوحيد
        }
    }

    /// <summary>يسجّل الجذر كموحَّد (سجل ضعيف + علامة في Tag).</summary>
    private static void MarkApplied(Control control)
    {
        if (control is null) return;

        try
        {
            if (!Applied.TryGetValue(control, out _))
                Applied.Add(control, AppliedMarker);
        }
        catch
        {
            // مفتاح مُضاف بالفعل — لا شيء يُفعل.
        }

        try
        {
            if (control.Tag is null || control.Tag is string)
                control.Tag = AppliedTag;
        }
        catch
        {
            // بعض العناصر ترفض تغيير Tag في حالات نادرة — العلامة في السجل تكفي.
        }
    }

    /// <summary>هل تحتوي الشجرة على جذر فرعي موحَّد سابقاً (تبويب داخلي)؟</summary>
    private static bool HasStandardizedSubtree(Control root)
    {
        foreach (Control child in root.Controls)
        {
            if (IsApplied(child)) return true;
            if (child.HasChildren && HasStandardizedSubtree(child)) return true;
        }

        return false;
    }
}
