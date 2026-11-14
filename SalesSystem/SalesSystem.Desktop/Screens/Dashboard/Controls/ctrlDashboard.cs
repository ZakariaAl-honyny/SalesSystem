// -----------------------------------------------------------------------------
// ctrlDashboard.cs — لوحة المعلومات كمكوّن قابل للتضمين (RULE-N64…RULE-N69)
//
// • المكوّن يحمل كل محتوى لوحة التحكم وسلوكها (البطاقات الست + أهلاً + الأرقام
//   + الاشتراكات في EventBus) — لا يوجد فيه زر إغلاق: الإغلاق مسؤولية الحاضن.
// • يعمل بلا تعديل داخل frmDashboard أو داخل أي حاضن آخر (frmMain لاحقاً).
// • كل خدماته تُحلّ من DI عبر I*DesktopService فقط — لا وصول مباشر لقاعدة البيانات
//   ولا أي طبقة أدنى (RULE-N05 / RULE-N06).
//
// التجميع (§2.34 / RULE-N135…N138) — من أعلى لأسفل، كل كتلة داخل GroupBox:
//   (1) العنوان lblMode  (2) gbCommands «أوامر اللوحة»  (3) gbIndicators «مؤشرات اليوم»
//   (4) gbDetails «ملخص الأرقام»  — ولا صندوق داخل صندوق، ولا gbFilter/pnlSummary
//   لأن اللوحة شاشة عرض لا قائمة سجلات.
//
// ملاحظة الاشتراك (RULE-N69): نستخدم Control.OnLoad — وهو يُطلق فعلياً
// لمكوّن UserControl يُضاف إلى حاضن حتى لو أُضيف قبل عرض النموذج؛ يُطلق مرة
// واحدة عند إنشاء مقبض النافذة (CreateControl) لحظة ظهور النموذج الحاضن.
//
// ملاحظة الاختصارات (RULE-N47): `KeyboardHelper.WireGlobalShortcuts` يعمل على
// النماذج فقط (Form)، لذا F5 يُوصَّل في النموذج الحاضن على `RefreshAsync` —
// كل حاضن (frmDashboard الآن، وأي حاضن آخر لاحقاً) يسطّر استدعاءً واحداً.
// -----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using Serilog;

namespace SalesSystem.Desktop.Screens.Dashboard.Controls;

/// <summary>
/// لوحة معلومات قابلة للتضمين: بطاقات المؤشرات + ملخص الأرقام + تحديث حيّ
/// عند تغيّر فواتير البيع/الشراء أو المخزون.
/// </summary>
public partial class ctrlDashboard : UserControl
{
    /// <summary>أدنى فاصل زمني بين تحديثين متتاليين (يمنع سيل التحديثات عند ترحيل فاتورة).</summary>
    private const int RefreshThrottleMs = 500;

    private readonly IServiceProvider _serviceProvider;

    private IDisposable? _subSaleInvoice;
    private IDisposable? _subPurchaseInvoice;
    private IDisposable? _subStock;

    private Form? _hostForm;
    private bool _subscribed;
    private DateTime _lastRefreshUtc = DateTime.MinValue;

    // ── الخدمات (RULE-N04) ───────────────────────────────────────────────────
    private IDashboardDesktopService DesktopService => _serviceProvider.GetRequiredService<IDashboardDesktopService>();
    private ISessionService SessionService => _serviceProvider.GetRequiredService<ISessionService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    public ctrlDashboard(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        InitializeComponent();

        // ── الهوية البصرية (RULE-N20 / RULE-N21) ──
        // UserControl لا يملك خاصية RightToLeftLayout (خاصة بالنماذج)؛ انعكاس
        // التخطيط يأتي من RightToLeftLayout للنموذج الحاضن، وRightToLeft هنا
        // يكفي لعكس اتجاه العناصر داخل المكوّن.
        RightToLeft = RightToLeft.Yes;
        Font = new Font("Tahoma", 10F);

        // ── لوحة المفاتيح (RULE-N40…RULE-N46) — لوحة عرض بلا حقول إدخال ──
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnRefresh);                       // RULE-N43

        // ── تلميحات الأزرار مع اختصاراتها (RULE-N47) ──
        toolTip1.SetToolTip(btnRefresh, "تحديث أرقام اللوحة من الخدمة (F5)");
        toolTip1.SetToolTip(lblPeriod, "الفترة التي تخصّها أرقام البطاقات");

        ApplyRoleVisibility();
        ApplyWelcomeText();
        ApplyUiTheme();
    }

    // ═════════════════════════════════════════════════════════════════
    //  الواجهة العامة — يستدعيها الحاضن متى شاء (آمنة من أي حاضن)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يحمّل ملخص لوحة المعلومات ويعرضه. لا يرمي استثناءً أبداً:
    /// فشل التحديث غير حرج ويُسجَّل كتحذير، وتظهر حالة «لا توجد بيانات» بدل خطأ (RULE-N29).
    /// </summary>
    public async Task RefreshAsync()
    {
        // الحاضن قد يغلق الشاشة قبل وصول الرد — لا نلمس عناصر مُنتهية
        if (IsDisposed) return;

        var previousCursor = Cursor;

        try
        {
            Cursor = Cursors.WaitCursor;             // RULE-N26
            SetStatus("جارٍ تحديث الأرقام…");

            _lastRefreshUtc = DateTime.UtcNow;

            var result = await DesktopService.GetSummaryAsync().ConfigureAwait(true);

            if (IsDisposed) return;

            if (result is null)
            {
                Log.Warning("لوحة المعلومات: استجابة فارغة من خدمة الملخص");
                ShowEmptyState();
                return;
            }

            if (!result.IsSuccess || result.Value is null)
            {
                Log.Warning("لوحة المعلومات: تعذر تحميل الملخص — {Error}", result.Error);
                ShowEmptyState();
                return;
            }

            ApplySummary(result.Value);
        }
        catch (Exception ex)
        {
            // فشل غير حرج: نُسجّل تحذيراً ونعرض «لا توجد بيانات» بدل نافذة خطأ
            Log.Warning(ex, "لوحة المعلومات: فشل تحديث الأرقام — ستُعاد المحاولة عند الحدث التالي");
            if (!IsDisposed) ShowEmptyState();
        }
        finally
        {
            if (!IsDisposed) Cursor = previousCursor;   // RULE-N26 — الاستعادة دائماً
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  عرض البيانات
    // ═════════════════════════════════════════════════════════════════

    private void ApplySummary(DashboardSummaryDto s)
    {
        // المبالغ N2 (RULE-001) — والعدادات أعداد صحيحة (لا كميات في هذا الملخص)
        cardSalesValue.Text = clsFormat.Money(s.TotalSalesToday);
        cardPurchasesValue.Text = clsFormat.Money(s.TotalPurchasesToday);
        cardCustomersValue.Text = s.ActiveCustomersCount.ToString();
        cardLowStockValue.Text = s.LowStockItemsCount.ToString();
        cardReceivablesValue.Text = clsFormat.Money(s.TotalReceivables);
        cardPayablesValue.Text = clsFormat.Money(s.TotalPayables);

        // الأسطر الثانوية داخل البطاقات
        lblSalesSub.Text = $"عدد الفواتير اليوم: {s.NumberOfSalesToday}\nهذا الشهر: {s.TotalSalesMonth:N2}";
        lblPurchasesSub.Text = $"هذا الشهر: {s.TotalPurchasesMonth:N2}";
        lblCustomersSub.Text = $"الموردون النشطون: {s.ActiveSuppliersCount}";
        lblStockSub.Text = $"إجمالي الأصناف: {s.TotalProductsCount}";

        // كتلة التفاصيل «ملخص الأرقام» — كل المبالغ N2 والأعداد بلا كسور
        lblDetails.Text =
            $"مبيعات اليوم: {s.TotalSalesToday:N2}   |   عدد فواتير اليوم: {s.NumberOfSalesToday}\n" +
            $"مبيعات الشهر: {s.TotalSalesMonth:N2}   |   مشتريات الشهر: {s.TotalPurchasesMonth:N2}\n" +
            $"مشتريات اليوم: {s.TotalPurchasesToday:N2}\n" +
            $"مستحقات لنا (عملاء): {s.TotalReceivables:N2}   |   مستحقات علينا (موردون): {s.TotalPayables:N2}\n" +
            $"العملاء النشطون: {s.ActiveCustomersCount}   |   الموردون النشطون: {s.ActiveSuppliersCount}\n" +
            $"إجمالي الأصناف: {s.TotalProductsCount}   |   أصناف بمخزون منخفض: {s.LowStockItemsCount}";

        // هل هناك حركة فعلاً؟ إن لا، نقولها للمستخدم بصراحة بدل أصفار صامتة
        bool noMovement = s.TotalSalesToday == 0m
                          && s.TotalPurchasesToday == 0m
                          && s.NumberOfSalesToday == 0
                          && s.TotalReceivables == 0m
                          && s.TotalPayables == 0m;

        SetStatus(noMovement
            ? "لا توجد حركة اليوم — ستُحدَّث الأرقام تلقائياً عند أول فاتورة"
            : "آخر تحديث: " + DateTime.Now.ToString("HH:mm:ss"));
    }

    /// <summary>حالة «لا توجد بيانات» — تُعرض بدل أي استثناء أو نافذة خطأ.</summary>
    private void ShowEmptyState()
    {
        lblDetails.Text = "لا توجد بيانات لعرضها حالياً.\nستُحدَّث الأرقام تلقائياً عند تسجيل أول حركة.";
        SetStatus("لا توجد بيانات — آخر محاولة: " + DateTime.Now.ToString("HH:mm:ss"));
    }

    private void SetStatus(string text) => lblDataStatus.Text = text;

    // ═════════════════════════════════════════════════════════════════
    //  الأوامر
    // ═════════════════════════════════════════════════════════════════

    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        try
        {
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            // RefreshAsync لا يرمي، لكن نلتزم بقاعدة async void (RULE-N25)
            Log.Error(ex, "لوحة المعلومات: تعذر تنفيذ التحديث اليدوي");
            clsDialogs.Warning("تعذر تحديث أرقام اللوحة. حاول مرة أخرى.");
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  الصلاحيات والترحيب
    // ═════════════════════════════════════════════════════════════════

    private void ApplyRoleVisibility()
    {
        if (SessionService.Current?.Role == SalesSystem.Common.Enums.UserRole.Cashier)
        {
            cardPurchases.Visible = false;
            cardPayables.Visible = false;
        }
    }

    private void ApplyWelcomeText()
    {
        lblWelcome.Text = "أهلاً " + (SessionService.Current?.FullName ?? "المستخدم");
    }

    // ═════════════════════════════════════════════════════════════════
    //  المظهر (RULE-N50…RULE-N54) — ينتهي بـ Apply(this) دائماً
    // ═════════════════════════════════════════════════════════════════

    private void ApplyUiTheme()
    {
        // الأسطر الثانوية والنصوص المهمة: من ثوابت الثيم لا من أرقام مكرّرة (RULE-N54)
        // (القيم نفسها معلنة في المصمّم — RULE-N134 — فالاستدعاء تثبيت لا تغيير)
        lblSalesSub.ForeColor = GreenThemeHelper.TextStrong;
        lblPurchasesSub.ForeColor = GreenThemeHelper.TextStrong;
        lblCustomersSub.ForeColor = GreenThemeHelper.TextStrong;
        lblStockSub.ForeColor = GreenThemeHelper.TextStrong;
        lblWelcome.ForeColor = GreenThemeHelper.TextStrong;
        lblPeriod.ForeColor = GreenThemeHelper.TextStrong;
        lblDetails.ForeColor = GreenThemeHelper.TextStrong;
        lblDataStatus.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.ApplyHeader(lblMode);   // RULE-N53
        GreenThemeHelper.Apply(this);            // آخر سطر دائماً (RULE-N50 / RULE-N69)
    }

    // ═════════════════════════════════════════════════════════════════
    //  دورة الحياة: اشتراكات EventBus + التحديث الحيّ
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// OnLoad يُطلق لمكوّن <see cref="UserControl"/> عند أول إنشاء لمقبضه،
    /// أي عند عرض النموذج الحاضن — حتى لو أُضيف المكوّن في مُنشئ الحاضن.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        SubscribeToEvents();
        _ = RefreshAsync();
    }

    protected override void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);
        AttachHostForm();
    }

    private void SubscribeToEvents()
    {
        if (_subscribed) return;

        try
        {
            var bus = EventBus;
            _subSaleInvoice = bus.Subscribe<SaleInvoiceChangedMessage>(_ => ScheduleRefresh());
            _subPurchaseInvoice = bus.Subscribe<PurchaseInvoiceChangedMessage>(_ => ScheduleRefresh());
            _subStock = bus.Subscribe<StockChangedMessage>(_ => ScheduleRefresh());
            _subscribed = true;
        }
        catch (Exception ex)
        {
            // غياب ناقل الأحداث لا يجوز أن يمنع ظهور اللوحة
            Log.Warning(ex, "لوحة المعلومات: تعذر الاشتراك في أحداث التغيير");
        }
    }

    /// <summary>يربط حدث تنشيط النموذج الحاضن حتى تتحدث الأرقام عند العودة إليه.</summary>
    private void AttachHostForm()
    {
        var form = FindForm();
        if (ReferenceEquals(form, _hostForm)) return;

        if (_hostForm != null)
            _hostForm.Activated -= HostForm_Activated;

        _hostForm = form;

        if (_hostForm != null)
            _hostForm.Activated += HostForm_Activated;
    }

    private void HostForm_Activated(object? sender, EventArgs e) => ScheduleRefresh();

    /// <summary>تحديث محدود التكرار: يمنع عشرات الاستدعاءات عند ترحيل فاتورة بسطور كثيرة.</summary>
    private void ScheduleRefresh()
    {
        if ((DateTime.UtcNow - _lastRefreshUtc).TotalMilliseconds < RefreshThrottleMs) return;
        _ = RefreshAsync();
    }

    // ═════════════════════════════════════════════════════════════════
    //  التنظيف (RULE-N30)
    // ═════════════════════════════════════════════════════════════════

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _subSaleInvoice?.Dispose();
            _subPurchaseInvoice?.Dispose();
            _subStock?.Dispose();
            _subSaleInvoice = null;
            _subPurchaseInvoice = null;
            _subStock = null;
            _subscribed = false;

            if (_hostForm != null)
            {
                _hostForm.Activated -= HostForm_Activated;
                _hostForm = null;
            }

            components?.Dispose();
        }

        base.Dispose(disposing);
    }
}
