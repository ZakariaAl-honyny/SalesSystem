// -----------------------------------------------------------------------------
// ctrlDashboard.cs — لوحة المعلومات كمكوّن قابل للتضمين (RULE-N64…RULE-N69)
//
// • المكوّن يحمل كل محتوى لوحة التحكم وسلوكها (البطاقات الست + أهلاً + الأرقام
//   + الاشتراكات في EventBus) — لا يوجد فيه زر إغلاق: الإغلاق مسؤولية الحاضن.
// • يعمل بلا تعديل داخل frmDashboard أو داخل أي حاضن آخر (frmMain لاحقاً).
// • كل خدماته تُحلّ من DI عبر I*DesktopService فقط — لا وصول مباشر لقاعدة البيانات
//   ولا أي طبقة أدنى (RULE-N05 / RULE-N06).
//
// ملاحظة الاشتراك (RULE-N69): نستخدم Control.OnLoad — وهو يُطلق فعلياً
// لمكوّن UserControl يُضاف إلى حاضن حتى لو أُضيف قبل عرض النموذج؛ يُطلق مرة
// واحدة عند إنشاء مقبض النافذة (CreateControl) لحظة ظهور النموذج الحاضن.
// -----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
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

        // لوحة عرض للقراءة فقط: لا حقول إدخال — يكفي ربط Enter/Tab (RULE-N40)
        KeyboardHelper.WireEnterAsTab(this);

        ApplyRoleVisibility();
        ApplyWelcomeText();
        ApplyUiTheme();
    }

    // ═════════════════════════════════════════════════════════════════
    //  الواجهة العامة — يستدعيها الحاضن متى شاء (آمنة من أي حاضن)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يحمّل ملخص لوحة المعلومات ويعرضه. لا يرمي استثناءً أبداً:
    /// فشل التحديث غير حرج ويُسجَّل كتحذير فقط (RULE-N29).
    /// </summary>
    public async Task RefreshAsync()
    {
        var previousCursor = Cursor;
        Cursor = Cursors.WaitCursor;

        try
        {
            _lastRefreshUtc = DateTime.UtcNow;

            var result = await DesktopService.GetSummaryAsync().ConfigureAwait(true);

            if (result is null)
            {
                Log.Warning("لوحة المعلومات: استجابة فارغة من خدمة الملخص");
                return;
            }

            if (!result.IsSuccess || result.Value is null)
            {
                Log.Warning("لوحة المعلومات: تعذر تحميل الملخص — {Error}", result.Error);
                return;
            }

            ApplySummary(result.Value);
        }
        catch (Exception ex)
        {
            // فشل غير حرج: نُسجّل تحذيراً ولا نُظهر خطأً للمستخدم
            Log.Warning(ex, "لوحة المعلومات: فشل تحديث الأرقام — ستُعاد المحاولة عند الحدث التالي");
        }
        finally
        {
            Cursor = previousCursor;
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  عرض البيانات
    // ═════════════════════════════════════════════════════════════════

    private void ApplySummary(DashboardSummaryDto s)
    {
        cardSalesValue.Text = s.TotalSalesToday.ToString("N2");
        cardPurchasesValue.Text = s.TotalPurchasesToday.ToString("N2");
        cardCustomersValue.Text = s.ActiveCustomersCount.ToString();
        cardLowStockValue.Text = s.LowStockItemsCount.ToString();
        cardReceivablesValue.Text = s.TotalReceivables.ToString("N2");
        cardPayablesValue.Text = s.TotalPayables.ToString("N2");

        // الأسطر الثانوية — تستخدم أعضاء DTO التي كانت مهجورة
        lblSalesSub.Text = $"عدد الفواتير اليوم: {s.NumberOfSalesToday}\nهذا الشهر: {s.TotalSalesMonth.ToString("N2")}";
        lblPurchasesSub.Text = $"هذا الشهر: {s.TotalPurchasesMonth.ToString("N2")}";
        lblCustomersSub.Text = $"الموردون النشطون: {s.ActiveSuppliersCount}";
        lblStockSub.Text = $"إجمالي الأصناف: {s.TotalProductsCount}";
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
    //  المظهر (RULE-N50…RULE-N54)
    // ═════════════════════════════════════════════════════════════════

    private void ApplyUiTheme()
    {
        // لون الأسطر الثانوية من الثيم الأخضر — بلا قيم لون مكرّرة (RULE-N54)
        lblSalesSub.ForeColor = GreenThemeHelper.TextStrong;
        lblPurchasesSub.ForeColor = GreenThemeHelper.TextStrong;
        lblCustomersSub.ForeColor = GreenThemeHelper.TextStrong;
        lblStockSub.ForeColor = GreenThemeHelper.TextStrong;
        lblWelcome.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.ApplyHeader(lblMode);   // RULE-N53
        GreenThemeHelper.Apply(this);            // آخر سطر دائماً (RULE-N50)
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
