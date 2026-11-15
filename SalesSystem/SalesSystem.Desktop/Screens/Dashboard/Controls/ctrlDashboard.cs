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
// ملاحظة دورة الحياة (حماية من التسريب والتحديث المضلِّل):
//   • ناقل الأحداث يحفظ المعالج بمرجع ضعيف ⇒ نحتفظ بالمعالجات في حقول صريحة،
//     وإلا جمعها الـ GC فتوقّف التحديث الحيّ بلا رسالة.
//   • حدث من خيط آخر يُمرَّر إلى خيط الواجهة قبل أي لمس لعنصر (InvokeRequired).
//   • تحديث واحد في الوقت (لا نتيجتان متعاكستان تكتبان فوق بعضهما) + تجميع
//     الأحداث المتقاربة بدل إسقاطها (حتى لا تبقى أرقام قديمة بعد ترحيل فاتورة).
//   • Dispose يلغي التحميل الجاري ويوقف المؤقّت ويفكّ الاشتراكات كلها.
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

    /// <summary>بديل الرقم عندما لا يوجد رقم حقيقي من الخدمة — لا نعرض صفراً مُفترضاً.</summary>
    private const string Placeholder = "—";

    private readonly IServiceProvider _serviceProvider;

    // معالجات الحدث في حقول صريحة: ناقل الأحداث يحفظ المعالج بمرجع ضعيف (WeakReference)،
    // فلو مُرّر كـ lambda داخلية لجُمع بالـ GC وتوقّف التحديث الحيّ بلا أي رسالة.
    private readonly Action<SaleInvoiceChangedMessage> _onSaleInvoiceChanged;
    private readonly Action<PurchaseInvoiceChangedMessage> _onPurchaseInvoiceChanged;
    private readonly Action<StockChangedMessage> _onStockChanged;

    private IDisposable? _subSaleInvoice;
    private IDisposable? _subPurchaseInvoice;
    private IDisposable? _subStock;

    private Form? _hostForm;
    private bool _subscribed;
    private bool _isRefreshing;
    private bool _refreshPending;
    private System.Windows.Forms.Timer? _refreshTimer;

    /// <summary>يُلغى عند تفريغ المكوّن فيوقف أي تحميل جارٍ بدل أن يكتب في عناصر مُنتهية.</summary>
    private CancellationTokenSource? _lifetimeCts = new();

    private DateTime _lastRefreshUtc = DateTime.MinValue;

    // ── الخدمات (RULE-N04) ───────────────────────────────────────────────────
    private IDashboardDesktopService DesktopService => _serviceProvider.GetRequiredService<IDashboardDesktopService>();
    private ISessionService SessionService => _serviceProvider.GetRequiredService<ISessionService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    public ctrlDashboard(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        // مراجع قوية للمعالجات (انظر تعليق الحقول) — تُنشأ مرة واحدة لكل مكوّن
        _onSaleInvoiceChanged = _ => ScheduleRefresh();
        _onPurchaseInvoiceChanged = _ => ScheduleRefresh();
        _onStockChanged = _ => ScheduleRefresh();

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
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnRefresh);                       // RULE-N43

        // ── تلميحات الأزرار مع اختصاراتها (RULE-N47) ──
        toolTip1.SetToolTip(btnRefresh, "تحديث أرقام اللوحة من الخدمة (F5)");
        toolTip1.SetToolTip(lblPeriod, "الفترة التي تخصّها أرقام البطاقات");

        ApplyRoleVisibility();
        ApplyWelcomeText();
        ClearCards();          // لا أصفار مُفترضة قبل أول تحميل — «—» حتى تصل الأرقام
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
        if (IsDisposed || Disposing) return;

        // تحميل جارٍ بالفعل: لا نُطلق تحميلاً متوازياً (نتيجتان متعاكستان تكتبان
        // فوق بعضهما فتبقى أرقام قديمة) — نُسجّل طلباً واحداً يُنفَّذ بعد الانتهاء
        if (_isRefreshing)
        {
            _refreshPending = true;
            return;
        }

        _isRefreshing = true;
        var previousCursor = Cursor;

        try
        {
            Cursor = Cursors.WaitCursor;             // RULE-N26
            SetStatus("جارٍ تحديث الأرقام…");

            _lastRefreshUtc = DateTime.UtcNow;

            var result = await DesktopService.GetSummaryAsync(RefreshToken).ConfigureAwait(true);

            if (IsDisposed || Disposing) return;

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
        catch (OperationCanceledException)
        {
            // الشاشة أُغلقت أثناء التحميل — لا شيء لعرضه
        }
        catch (Exception ex)
        {
            // فشل غير حرج: نُسجّل تحذيراً ونعرض «لا توجد بيانات» بدل نافذة خطأ
            Log.Warning(ex, "لوحة المعلومات: فشل تحديث الأرقام — ستُعاد المحاولة عند الحدث التالي");
            if (!IsDisposed && !Disposing) ShowEmptyState();
        }
        finally
        {
            _isRefreshing = false;

            if (!IsDisposed && !Disposing) Cursor = previousCursor;   // RULE-N26 — الاستعادة دائماً

            if (_refreshPending)
            {
                _refreshPending = false;
                if (!IsDisposed && !Disposing) _ = RefreshAsync();
            }
        }
    }

    /// <summary>رمز إلغاء عمر المكوّن — يبقى صالحاً (ملغى) بعد تفريغ المكوّن بلا استثناء.</summary>
    private CancellationToken RefreshToken
        => _lifetimeCts?.Token ?? new CancellationToken(canceled: true);

    // ═════════════════════════════════════════════════════════════════
    //  عرض البيانات
    // ═════════════════════════════════════════════════════════════════

    private void ApplySummary(DashboardSummaryDto s)
    {
        // المبالغ والأعداد من البيت الواحد فقط (RULE-N169): clsFormat.Money للمال (RULE-001)
        // وclsFormat.Number للعدادات — لا صيغة رقمية مكرّرة ولا ToString() داخل الشاشة.
        cardSalesValue.Text = clsFormat.Money(s.TotalSalesToday);
        cardPurchasesValue.Text = clsFormat.Money(s.TotalPurchasesToday);
        cardCustomersValue.Text = clsFormat.Number(s.ActiveCustomersCount);
        cardLowStockValue.Text = clsFormat.Number(s.LowStockItemsCount);
        cardReceivablesValue.Text = clsFormat.Money(s.TotalReceivables);
        cardPayablesValue.Text = clsFormat.Money(s.TotalPayables);

        // الأسطر الثانوية داخل البطاقات
        lblSalesSub.Text = $"عدد الفواتير اليوم: {clsFormat.Number(s.NumberOfSalesToday)}\nهذا الشهر: {clsFormat.Money(s.TotalSalesMonth)}";
        lblPurchasesSub.Text = $"هذا الشهر: {clsFormat.Money(s.TotalPurchasesMonth)}";
        lblCustomersSub.Text = $"الموردون النشطون: {clsFormat.Number(s.ActiveSuppliersCount)}";
        lblStockSub.Text = $"إجمالي الأصناف: {clsFormat.Number(s.TotalProductsCount)}";

        // كتلة التفاصيل «ملخص الأرقام» — كل المبالغ من clsFormat.Money والأعداد من clsFormat.Number
        lblDetails.Text = BuildDetailsText(s);

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

    /// <summary>
    /// نص «ملخص الأرقام». يحترم ما أخفاه <see cref="ApplyRoleVisibility"/>:
    /// لا يُعاد رقم بطاقة مخفية داخل النص (أمين الصندوق لا يرى المشتريات ولا المستحقات علينا).
    /// </summary>
    private string BuildDetailsText(DashboardSummaryDto s)
    {
        bool showPurchases = cardPurchases.Visible;
        bool showPayables = cardPayables.Visible;

        var lines = new List<string>
        {
            $"مبيعات اليوم: {clsFormat.Money(s.TotalSalesToday)}   |   عدد فواتير اليوم: {clsFormat.Number(s.NumberOfSalesToday)}",
            showPurchases
                ? $"مبيعات الشهر: {clsFormat.Money(s.TotalSalesMonth)}   |   مشتريات الشهر: {clsFormat.Money(s.TotalPurchasesMonth)}"
                : $"مبيعات الشهر: {clsFormat.Money(s.TotalSalesMonth)}"
        };

        if (showPurchases)
            lines.Add($"مشتريات اليوم: {clsFormat.Money(s.TotalPurchasesToday)}");

        lines.Add(showPayables
            ? $"مستحقات لنا (عملاء): {clsFormat.Money(s.TotalReceivables)}   |   مستحقات علينا (موردون): {clsFormat.Money(s.TotalPayables)}"
            : $"مستحقات لنا (عملاء): {clsFormat.Money(s.TotalReceivables)}");

        lines.Add($"العملاء النشطون: {clsFormat.Number(s.ActiveCustomersCount)}   |   الموردون النشطون: {clsFormat.Number(s.ActiveSuppliersCount)}");
        lines.Add($"إجمالي الأصناف: {clsFormat.Number(s.TotalProductsCount)}   |   أصناف بمخزون منخفض: {clsFormat.Number(s.LowStockItemsCount)}");

        return string.Join(Environment.NewLine, lines);
    }

    /// <summary>حالة «لا توجد بيانات» — تُعرض بدل أي استثناء أو نافذة خطأ.</summary>
    private void ShowEmptyState()
    {
        // لا نُبقي أرقام تحميل سابق: بطاقة تعرض رقماً لم تُرجعه الخدمة الآن = بيانات كاذبة
        ClearCards();

        lblDetails.Text = "لا توجد بيانات لعرضها حالياً.\nستُحدَّث الأرقام تلقائياً عند تسجيل أول حركة.";
        SetStatus("لا توجد بيانات — آخر محاولة: " + DateTime.Now.ToString("HH:mm:ss"));
    }

    /// <summary>تفريغ أرقام البطاقات (تُستدعى عند الفشل وعند أول تحميل) — بلا أي رقم مُفترض.</summary>
    private void ClearCards()
    {
        cardSalesValue.Text = Placeholder;
        cardPurchasesValue.Text = Placeholder;
        cardCustomersValue.Text = Placeholder;
        cardLowStockValue.Text = Placeholder;
        cardReceivablesValue.Text = Placeholder;
        cardPayablesValue.Text = Placeholder;

        lblSalesSub.Text = Placeholder;
        lblPurchasesSub.Text = Placeholder;
        lblCustomersSub.Text = Placeholder;
        lblStockSub.Text = Placeholder;
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
            // نمرّر الحقول لا lambda داخلية: الناقل يحفظ المعالج بمرجع ضعيف
            _subSaleInvoice = bus.Subscribe<SaleInvoiceChangedMessage>(_onSaleInvoiceChanged);
            _subPurchaseInvoice = bus.Subscribe<PurchaseInvoiceChangedMessage>(_onPurchaseInvoiceChanged);
            _subStock = bus.Subscribe<StockChangedMessage>(_onStockChanged);
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

    /// <summary>
    /// تحديث مُجمَّع: يمنع عشرات الاستدعاءات عند ترحيل فاتورة بسطور كثيرة،
    /// ولا يُسقط الحدث — الإسقاط كان يُبقي أرقاماً قديمة على الشاشة بعد الترحيل.
    /// </summary>
    private void ScheduleRefresh()
    {
        if (IsDisposed || Disposing) return;

        // قد يصل الحدث من خيط غير خيط الواجهة — يُمرَّر إلى خيط الواجهة أولاً
        if (InvokeRequired)
        {
            try
            {
                BeginInvoke(new Action(ScheduleRefresh));
            }
            catch (InvalidOperationException)
            {
                // أُغلق المقبض أو فُرِّغ المكوّن بين الفحص والتنفيذ (يشمل ObjectDisposedException)
            }

            return;
        }

        if (_isRefreshing)
        {
            _refreshPending = true;
            return;
        }

        var remainingMs = RefreshThrottleMs - (DateTime.UtcNow - _lastRefreshUtc).TotalMilliseconds;

        if (remainingMs <= 0)
        {
            _ = RefreshAsync();
            return;
        }

        // داخل نافذة الحظر: نجمع الأحداث في مؤقّت واحد يطلق التحديث عند انتهائها
        var timer = _refreshTimer ??= CreateRefreshTimer();
        timer.Stop();
        timer.Interval = Math.Max(1, (int)Math.Ceiling(remainingMs));
        timer.Start();
    }

    private System.Windows.Forms.Timer CreateRefreshTimer()
    {
        var timer = new System.Windows.Forms.Timer { Interval = RefreshThrottleMs };
        timer.Tick += RefreshTimer_Tick;
        return timer;
    }

    private void RefreshTimer_Tick(object? sender, EventArgs e)
    {
        _refreshTimer?.Stop();

        if (IsDisposed || Disposing) return;

        if (_isRefreshing)
        {
            _refreshPending = true;
            return;
        }

        _ = RefreshAsync();
    }

    // ═════════════════════════════════════════════════════════════════
    //  التنظيف (RULE-N30)
    // ═════════════════════════════════════════════════════════════════

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // إيقاف أي تحميل جارٍ أولاً — فلا يكتب في عناصر فُرِّغت بعد قليل
            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();
            _lifetimeCts = null;

            if (_refreshTimer != null)
            {
                _refreshTimer.Stop();
                _refreshTimer.Tick -= RefreshTimer_Tick;
                _refreshTimer.Dispose();
                _refreshTimer = null;
            }

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
