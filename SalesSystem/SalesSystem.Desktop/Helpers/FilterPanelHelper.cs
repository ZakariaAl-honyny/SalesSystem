// -----------------------------------------------------------------------------
// FilterPanelHelper.cs — زر «إخفاء/إظهار التصفية» وحفظ حالته (RULE-N63)
//
// الهدف: كسب مساحة للشبكة حين لا يحتاج المستخدم أدوات التصفية، مع تذكّر اختياره
// بين الجلسات في SystemSettings تحت المفتاح UI.FiltersCollapsed ("true" = مطوية).
//
// الاستهلاك: سطر واحد لكل شاشة (RULE-N112 — لا يُنسخ السلوك داخل الشاشة):
//     FilterPanelHelper.Attach(this, _serviceProvider);
//
// ما تفعله الأداة:
//   1) تجد حاوية التصفية: pnlFilters، وإلا اللوحة التي تضم gbFilter مباشرةً.
//   2) تتبنّى زر btnToggleFilters **المُعلَن في المصمّم** (RULE-N145) وتُوصِّل سلوكه فقط
//      (نقرة + F9 + التلميح) دون تغيير شكله أو موضعه؛ وإن لم يكن معلناً تُنشئه وقت التشغيل
//      كبديل احتياطي (Dock = Left) بعد gbFilter فيُقتطع عرض الزر أولاً ويملأ gbFilter الباقي.
//   3) تطوي/توسّع: gbFilter.Visible + ارتفاع اللوحة (34 عند الطي)، وتُحدّث نص الزر
//      وتلميحه. تحفظ الارتفاع الأصلي كما هو في المصمّم (لا رقم ثابت: 112/68/…).
//   4) تحفظ الحالة في SystemSettings، وتقرأها عند form.Load (لا تُعطّل المُنشئ).
//
// ما لا تفعله:
//   • لا تلمس أدوات التصفية نفسها ولا التصفية الفورية ولا أي منطق شاشة.
//   • لا تكتب أي قيمة لون: الألوان من ThemeHelper/GreenThemeHelper فقط (RULE-N54).
//   • لا تُوصِّل تنقّل لوحة المفاتيح: الشاشة تستدعي الستة بنفسها (§2.27).
//   • آمنة للاستدعاء المتكرر: وجود btnToggleFilters بالاسم يوقف العمل.
//   • لا تُطلق استثناءً أبداً (RULE-N14/§2.21): أي فشل يُسجَّل في Serilog فقط.
// -----------------------------------------------------------------------------

using System.Drawing;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// زر «إخفاء/إظهار التصفية» لكل شاشة فيها <c>pnlFilters</c> + <c>gbFilter</c>
/// (RULE-N63): يطوي لوحة التصفية لكسب مساحة الشبكة، ويحفظ حالته في
/// <c>SystemSettings</c> تحت المفتاح <c>UI.FiltersCollapsed</c>.
/// </summary>
public static class FilterPanelHelper
{
    // ── ثوابت الأسماء والنصوص (RULE-N56/N58) ─────────────────────────
    private const string ToggleButtonName = "btnToggleFilters";
    private const string FilterPanelControlName = "pnlFilters";
    private const string FilterGroupControlName = "gbFilter";

    /// <summary>مفتاح الإعداد في SystemSettings (RULE-N63). القيمة "true" = مطوية.</summary>
    private const string CollapsedSettingKey = "UI.FiltersCollapsed";

    /// <summary>ارتفاع لوحة التصفية المطوية — يكفي لزر واحد (RULE-N63).</summary>
    private const int CollapsedPanelHeight = 34;

    /// <summary>السجلات المُوصَّلة — يمنع توصيل زر واحد مرتين بلا تلويث <c>Tag</c> (RULE-N63).</summary>
    private static readonly ConditionalWeakTable<Button, string> WiredButtons = new();

    /// <summary>عرض زر التبديل.</summary>
    private const int ToggleButtonWidth = 140;

    private const string TextCollapse = "إخفاء التصفية";
    private const string TextExpand = "إظهار التصفية";
    private const string TipToggle = "إخفاء/إظهار لوحة التصفية — F9";
    private const string TipCollapse = "إخفاء لوحة التصفية — F9";
    private const string TipExpand = "إظهار لوحة التصفية — F9";

    /// <summary>
    /// حالة ربط واحدة لكل شاشة: الحاوية، الصندوق، الزر، التلميح، الارتفاع الأصلي،
    /// ونمط صف TableLayoutPanel الأصلي (لتقرير/شاشات تُدرج اللوحة كصفّ في جدول).
    /// </summary>
    private sealed class FilterPanelState
    {
        public Control Host = null!;
        public Control Group = null!;
        public Button Button = null!;
        public ToolTip Tip = null!;
        public int ExpandedHeight;
        public bool HasRowStyle;
        public SizeType RowSizeType;
        public float RowHeight;
        public bool Collapsed;
    }

    // ═════════════════════════════════════════════════════════════════
    //  Attach — المدخل الوحيد
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يضيف زر «إخفاء/إظهار التصفية» إلى شاشة وتُحفظ حالته في SystemSettings (RULE-N63).</summary>
    /// <param name="form">الشاشة — قد تكون null (لا شيء يُفعل).</param>
    /// <param name="serviceProvider">حاوية الخدمات لحفظ الحالة — قد تكون null (يُلغى الحفظ فقط).</param>
    public static void Attach(Form? form, IServiceProvider? serviceProvider)
    {
        if (form is null) return;

        try
        {
            // (1) الحاوية: pnlFilters، وإلا اللوحة التي تضم gbFilter مباشرةً.
            var host = FindFilterHost(form);
            if (host is null || host.IsDisposed)
            {
                // شاشات التقارير/الطباعة بلا pnlFilters: تخطٍّ صامت مقصود (RULE-N121).
                Serilog.Log.Debug("لا لوحة تصفية في {Form} — لم يُضَف زر إخفاء/إظهار التصفية", form.GetType().Name);
                return;
            }

            var group = FindControl(host, FilterGroupControlName);
            if (group is null || group.IsDisposed)
            {
                Serilog.Log.Debug("لا صندوق gbFilter داخل {Host} في {Form} — لم يُضَف زر التصفية",
                    host.Name, form.GetType().Name);
                return;
            }

            // (2) الزر: يُتبنّى من المصمّم إن كان مُعلَناً هناك (RULE-N145) فيتساوى
            //     المصمّم ووقت التشغيل، وإلا يُنشأ هنا كبديل احتياطي للشاشات غير المرحَّلة.
            var existing = FindControl(form, ToggleButtonName) as Button;
            bool createdHere = existing is null;
            var button = existing ?? CreateToggleButton();

            // idempotent — نفس الشاشة لا تُوصِّل الزر مرتين (RULE-N63).
            if (WiredButtons.TryGetValue(button, out _)) return;
            WiredButtons.Add(button, ToggleButtonName);

            // (3) ارتفاع اللوحة الحالي كما ضبطه المصمّم (112/68/…) — لا رقم ثابت.
            var state = new FilterPanelState
            {
                Host = host,
                Group = group,
                Button = button,
                ExpandedHeight = host.Height
            };

            CaptureRowStyle(state);

            // (4) التلميح + النقرة.
            var tip = new ToolTip { InitialDelay = 300, ReshowDelay = 100 };
            tip.SetToolTip(button, TipToggle);

            state.Tip = tip;

            button.Click += (_, _) => Toggle(state, serviceProvider);

            // زر المصمّم يحتفظ بإرسائه/شكله كما هو؛ ولا يُضاف إلى الحاوية إلا إن أُنشئ هنا.
            if (createdHere)
            {
                // يُضاف بعد gbFilter (نهاية Controls) فيُقتطع الزر أولاً ويملأ gbFilter الباقي.
                host.Controls.Add(button);
            }

            // (5) اختصار F9 — عبر الأداة الموحّدة (RULE-N47)، بلا تكرار منطق مفاتيح هنا.
            KeyboardHelper.WireGlobalShortcuts(form, (Keys.F9, () => Toggle(state, serviceProvider)));

            // (6) الحالة المحفوظة — بعد اكتمال بناء النموذج، لا في المُنشئ.
            if (form.IsHandleCreated)
                form.BeginInvoke((Action)(() => _ = LoadSavedStateAsync(form, state, serviceProvider)));
            else
                form.Load += (_, _) => _ = LoadSavedStateAsync(form, state, serviceProvider);
        }
        catch (Exception ex)
        {
            // never throw — شاشة بلا زر أفضل من شاشة لا تُفتح (RULE-N99).
            Serilog.Log.Error(ex, "تعذر إضافة زر إخفاء/إظهار التصفية إلى {Form}", form.GetType().Name);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  البحث عن عناصر الشاشة
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يبحث عن عنصر بالاسم داخل الشجرة (بلا استثناء، null-safe).</summary>
    private static Control? FindControl(Control? parent, string name)
    {
        if (parent is null) return null;

        try
        {
            foreach (Control child in parent.Controls)
            {
                if (string.Equals(child.Name, name, StringComparison.OrdinalIgnoreCase))
                    return child;

                if (child.HasChildren)
                {
                    var nested = FindControl(child, name);
                    if (nested is not null) return nested;
                }
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر البحث عن عنصر {Name} في شجرة الشاشة", name);
        }

        return null;
    }

    /// <summary>
    /// حاوية التصفية: <c>pnlFilters</c> إن وُجدت؛ وإلا اللوحة (<see cref="Panel"/>)
    /// التي تحتوي <c>gbFilter</c> مباشرةً؛ وإلا null (تخطٍّ صامت).
    /// </summary>
    private static Control? FindFilterHost(Form form)
    {
        if (FindControl(form, FilterPanelControlName) is Control named)
            return named;

        if (FindControl(form, FilterGroupControlName)?.Parent is Panel panel)
            return panel;

        return null;
    }

    // ═════════════════════════════════════════════════════════════════
    //  إنشاء الزر (RULE-N63 + RULE-N21 + RULE-N54)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// ينشئ زر التبديل: Dock = Left، عرض 140، Tahoma 9 Bold، Popup.
    /// الألوان تُؤخذ من <see cref="ThemeHelper"/> — ممنوع تكرار قيم لون هنا (RULE-N54).
    /// </summary>
    private static Button CreateToggleButton()
    {
        var button = new Button
        {
            Name = ToggleButtonName,
            Text = TextCollapse,
            Dock = DockStyle.Left,
            Width = ToggleButtonWidth,
            Cursor = Cursors.Hand,
            FlatStyle = FlatStyle.Popup,
            RightToLeft = RightToLeft.Yes,
            TabStop = true,
            Margin = new Padding(0)
        };

        // الألوان والحدود من الثيم القائم، ثم نضبط الشكل المطلوب (Dock/عرض/خط Tahoma).
        ThemeHelper.ApplyButtonStyle(button, ThemeHelper.ButtonType.Secondary);

        button.AutoSize = false;
        button.UseVisualStyleBackColor = false;   // حتى يُحترم لون الثيم (Popup)
        button.Dock = DockStyle.Left;
        button.Width = ToggleButtonWidth;
        button.Height = ThemeHelper.StandardControlHeight;
        button.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        button.FlatStyle = FlatStyle.Popup;
        button.Cursor = Cursors.Hand;

        return button;
    }

    // ═════════════════════════════════════════════════════════════════
    //  التبديل والتخطيط
    // ═════════════════════════════════════════════════════════════════

    /// <summary>القلب المشترك: يقلب الحالة ثم يحفظها.</summary>
    private static void Toggle(FilterPanelState state, IServiceProvider? serviceProvider)
    {
        try
        {
            if (state.Host.IsDisposed || state.Group.IsDisposed || state.Button.IsDisposed) return;

            var form = state.Host.FindForm();

            ApplyState(state, form, !state.Collapsed);
            _ = SaveStateAsync(state, form, serviceProvider);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تبديل لوحة التصفية");
        }
    }

    /// <summary>يطبّق الحالة (مطوية/موسّعة) على الصندوق واللوحة والزر والتلميح.</summary>
    private static void ApplyState(FilterPanelState state, Form? form, bool collapsed)
    {
        try
        {
            if (state.Host.IsDisposed || state.Group.IsDisposed || state.Button.IsDisposed) return;

            bool groupHadFocus = state.Group.ContainsFocus;

            state.Collapsed = collapsed;
            state.Group.Visible = !collapsed;
            ApplyHostHeight(state, collapsed);

            state.Button.Text = collapsed ? TextExpand : TextCollapse;
            state.Tip.SetToolTip(state.Button, collapsed ? TipExpand : TipCollapse);

            state.Host.PerformLayout();

            // التركيز كان داخل صندوق أصبح مخفياً: ننقله إلى زر التبديل بدل ضياعه.
            if (collapsed && groupHadFocus && state.Button.CanSelect)
                state.Button.Focus();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق حالة لوحة التصفية");
        }
    }

    /// <summary>
    /// يضبط ارتفاع الحاوية: مطوية ⇒ 34؛ موسّعة ⇒ الارتفاع الأصلي.
    /// وإن كانت اللوحة صفّاً في <see cref="TableLayoutPanel"/> تُضبط قاعدة الصف أيضاً
    /// (وإلا تجاهل الجدول الارتفاع المرئي) ثم تُستعاد قاعدتها الأصلية عند التوسيع.
    /// </summary>
    private static void ApplyHostHeight(FilterPanelState state, bool collapsed)
    {
        try
        {
            state.Host.Height = collapsed ? CollapsedPanelHeight : state.ExpandedHeight;

            if (state.Host.Parent is not TableLayoutPanel layout) return;

            int row = layout.GetRow(state.Host);
            if (row < 0 || row >= layout.RowStyles.Count) return;

            layout.RowStyles[row] = collapsed
                ? new RowStyle(SizeType.Absolute, CollapsedPanelHeight)
                : new RowStyle(
                    state.HasRowStyle ? state.RowSizeType : SizeType.Absolute,
                    state.HasRowStyle ? state.RowHeight : state.ExpandedHeight);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر ضبط ارتفاع لوحة التصفية");
        }
    }

    /// <summary>يحفظ قاعدة صف الجدول الأصلية إن كانت اللوحة صفّاً في TableLayoutPanel.</summary>
    private static void CaptureRowStyle(FilterPanelState state)
    {
        try
        {
            if (state.Host.Parent is not TableLayoutPanel layout) return;

            int row = layout.GetRow(state.Host);
            if (row < 0 || row >= layout.RowStyles.Count) return;

            state.HasRowStyle = true;
            state.RowSizeType = layout.RowStyles[row].SizeType;
            state.RowHeight = layout.RowStyles[row].Height;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر قراءة نمط صف لوحة التصفية");
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  الحفظ والقراءة في SystemSettings (RULE-N63)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يقرأ <c>UI.FiltersCollapsed</c> ويطبّقها بعد اكتمال بناء النموذج.
    /// أي فشل يُسجَّل في Serilog ويُترك الشاشة موسّعة كما هي.
    /// </summary>
    private static async Task LoadSavedStateAsync(Form form, FilterPanelState state, IServiceProvider? serviceProvider)
    {
        try
        {
            if (!TryResolveSettings(serviceProvider, out var settings)) return;

            var result = await settings.GetAllSystemSettingsAsync();
            if (!result.IsSuccess || result.Value is null) return;

            if (!TryReadBool(result.Value, CollapsedSettingKey, out bool collapsed)) return;
            if (collapsed == state.Collapsed) return;
            if (form.IsDisposed || state.Host.IsDisposed) return;

            if (form.InvokeRequired)
                form.BeginInvoke((Action)(() => ApplyState(state, form, collapsed)));
            else
                ApplyState(state, form, collapsed);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر قراءة حالة لوحة التصفية للشاشة {Form}", form.GetType().Name);
        }
    }

    /// <summary>
    /// يحفظ الحالة الجديدة. <c>SystemSettingsRepository.SetBatchSystemSettingsAsync</c>
    /// هو Upsert لكل مفتاح (يُحدِّث الموجود ويُضيف غير الموجود، ويترك بقية المفاتيح كما هي)،
    /// لذلك كتابة المفتاح الواحد آمنة ولا تحتاج قراءة القاموس كله أولاً.
    /// </summary>
    private static async Task SaveStateAsync(FilterPanelState state, Form? form, IServiceProvider? serviceProvider)
    {
        try
        {
            if (!TryResolveSettings(serviceProvider, out var settings)) return;

            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [CollapsedSettingKey] = state.Collapsed ? "true" : "false"
            };

            var result = await settings.UpdateSystemSettingsAsync(values);
            if (result.IsSuccess) return;

            Serilog.Log.Warning("تعذر حفظ حالة لوحة التصفية للشاشة {Form}: {Error}",
                form?.GetType().Name, result.Error);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر حفظ حالة لوحة التصفية");
        }
    }

    /// <summary>يستخرج خدمة الإعدادات من الحاوية — false إن كانت الحاوية/الخدمة غير متاحة.</summary>
    private static bool TryResolveSettings(IServiceProvider? serviceProvider, out ISettingsDesktopService settings)
    {
        settings = null!;
        if (serviceProvider is null) return false;

        try
        {
            settings = serviceProvider.GetRequiredService<ISettingsDesktopService>();
            return settings is not null;
        }
        catch (Exception ex)
        {
            // الوضع التجريبي/اختبار بلا حاوية: نتجاهل الحفظ بصمت.
            Serilog.Log.Debug(ex, "خدمة الإعدادات غير متاحة — لم تُحفظ حالة لوحة التصفية");
            return false;
        }
    }

    /// <summary>يقرأ قيمة منطقية من قاموس الإعدادات، بغضّ النظر عن حالة الأحرف في المفتاح.</summary>
    private static bool TryReadBool(Dictionary<string, string> values, string key, out bool result)
    {
        result = false;

        if (values.TryGetValue(key, out var direct) && !string.IsNullOrWhiteSpace(direct))
            return bool.TryParse(direct.Trim(), out result);

        foreach (var pair in values)
        {
            if (!string.Equals(pair.Key, key, StringComparison.OrdinalIgnoreCase)) continue;
            if (string.IsNullOrWhiteSpace(pair.Value)) return false;

            return bool.TryParse(pair.Value.Trim(), out result);
        }

        return false;
    }
}
