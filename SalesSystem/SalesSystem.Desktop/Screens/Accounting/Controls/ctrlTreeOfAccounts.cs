// -----------------------------------------------------------------------------
// ctrlTreeOfAccounts.cs — شجرة دليل الحسابات (F-003 · المرحلة أ: Mock/واجهة فقط)
//
// • عنصر قابل للتضمين: يعرض دليل الحسابات هرمياً (أب ← أبناء) من
//   IAccountDesktopService.GetTreeAsync — لا وصول لأي طبقة أدنى (RULE-N05 / RULE-N06).
// • المستوى الأول ثابت: خمس عقد طبائع («الأصول (1) — 24 حساباً» … «المصروفات (5)»)
//   ثم الحسابات الجذرية تحت طبيعتها وفروعها كما وصلت من الخدمة (بلا حد للعمق)، فيرى
//   المحاسب هيكل الدليل كاملاً حتى لو كانت طبيعة ما بلا حسابات.
// • الغرض: **إيجاد الحساب بسرعة** — تصفية فورية بالكود أو بالاسم (كبح 250 مللي)
//   تُظهر المطابقات **مع أسلافها** وتُبقي عقد الطبائع الخمس (السياق لا يُفقد)، ثم تُوسّع
//   النتيجة؛ وبلا مطابقة على مستوى الشجرة كلها عقدة واحدة باهتة «لا توجد حسابات مطابقة».
// • يعرّض للحاضن ما يحتاجه لتصفية شبكته: GetSubtreeIds (الفرع) و GetNatureIds (الطبيعة).
// • CRUD من الشجرة بقرار المالك، بلا بناء محرّر ثانٍ (RULE-N100): «إضافة حساب فرعي»
//   و«تعديل» يُطلقان حدثاً فقط ويُفتح frmAccounts في الحاضن؛ و«حذف»/«حذف نهائي»
//   ينادِيان الخدمة بعد clsDialogs.Confirm.
// • شريط الأوامر الملوّن هو العنصر المشترك ctrlCrudToolbar (البيت الواحد — Controls/Common):
//   يُعلَن في المصمّم ويُغذَّى بالمدخلات من `_ApplyActionState()` وحدها، فلا نسخة ثانية
//   من الألوان ولا من آلة الحالة، ولا أزرار CRUD خاصة بهذه الشاشة.
// • آلة الحالة واحدة `_ApplyActionState()` تُستدعى بعد كل تغيير (اختيار · تحديث · انشغال)
//   — ولا يُضبط Enabled/Visible داخل معالج (النظير `RULE-N150` للعناصر).
// • الحدود الوحيدة لـtry/catch (RULE-N205): RefreshAsync · قراءة بيانات الحساب
//   (fire-and-forget) · معالجا الحذف (async void). وما عداها بلا catch.
//
// التجميع (§2.34 / RULE-N135…N138) — كشاشة المرجع frmAddUpdateSalesInvoice:
//   «أوامر الحسابات» ← «بحث وتصفية» ← «شجرة الحسابات» ← شريط «ملخص».
// -----------------------------------------------------------------------------

using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Controls;

/// <summary>
/// شجرة دليل الحسابات: مستوى أول ثابت بخمس طبائع، ثم هرمية الحسابات الكاملة (أي عمق)،
/// وبحث فوري يُبقي السياق، وآلتا تصفية للحاضن (<see cref="GetSubtreeIds"/> ·
/// <see cref="GetNatureIds"/>)، وعمليات إضافة فرعي · تعديل · حذف · حذف نهائي.
/// <para>الاستخدام: <see cref="Attach"/> ثم <see cref="RefreshAsync"/> — ويُفضَّل أن يملك الحاضن
/// الاختصارات (F5/Esc) لأن <c>KeyboardHelper.WireGlobalShortcuts</c> تخصّ النماذج.</para>
/// </summary>
public partial class ctrlTreeOfAccounts : UserControl
{
    // ── رسائل عربية (RULE-N28) ────────────────────────────────────────────────
    private const string NotAttachedMessage = "لم يتم تهيئة شجرة الحسابات — تعذر الوصول إلى خدمة دليل الحسابات.";
    private const string LoadFailedMessage = "تعذر تحميل دليل الحسابات";
    private const string NoSelectionMessage = "يرجى اختيار حساب من الشجرة أولاً.";
    private const string DeleteRefusedSystemMessage = "لا يمكن حذف حساب نظامي نهائياً.";
    private const string DeleteRefusedChildrenMessage = "لا يمكن الحذف النهائي لحساب له حسابات فرعية.";
    private const string DeleteRefusedUnknownMessage = "تعذر التحقق من بيانات الحساب — لا يمكن الحذف النهائي الآن.";
    private const string AddUnderLeafMessage = "لا يمكن إضافة حساب فرعي تحت حساب حركة نهائي — اختر حساباً تجميعياً.";

    private const string NoMatchNodeText = "لا توجد حسابات مطابقة";

    /// <summary>الفاصل بين الكود والاسم في نص العقدة (قرار الشكل: «الكود — الاسم»).</summary>
    private const string NodeSeparator = " — ";

    /// <summary>
    /// الطبائع الخمس بترتيبها الرقمي — **المصدر الوحيد** لتسميات المستوى الأول ونص شريط الملخص
    /// (أسماء جمعية بقرار المالك، وليست <c>AccountDto.NatureDisplay</c> المفردة).
    /// </summary>
    private static readonly (byte Nature, string Name)[] NatureNames =
    [
        (1, "الأصول"),
        (2, "الخصوم"),
        (3, "حقوق الملكية"),
        (4, "الإيرادات"),
        (5, "المصروفات"),
    ];

    /// <summary>حقلا التصفية: الاسم يُقرأ من خاصية <c>Display</c> (DisplayMember) لا من نص حر (RULE-N60).</summary>
    private static readonly SearchField[] SearchFields =
    [
        new("الكود", ByName: false),
        new("الاسم", ByName: true),
    ];

    private IServiceProvider? _serviceProvider;

    /// <summary>الشجرة الكاملة كما وصلت من الخدمة (المصدر الوحيد لإعادة البناء عند التصفية).</summary>
    private List<AccountTreeNodeDto> _roots = [];

    /// <summary>الحساب المحدد حالياً (مرجع الشجرة، وقد يكون نسخة مُقلَّمة أثناء التصفية).</summary>
    private AccountTreeNodeDto? _selectedAccount;

    /// <summary>الطبيعة المحددة حالياً (عندما تكون العقدة المحددة عقدة طبيعة لا حساباً).</summary>
    private byte? _selectedNature;

    /// <summary>ذاكرة بيانات الحساب لكل معرّف — الحكم «نظامي؟» لا يأتي من عقدة الشجرة.</summary>
    private readonly Dictionary<int, AccountDto?> _accountById = [];

    /// <summary>معرّفات الحسابات التي لها أبناء في الشجرة **الكاملة** (لا المقلَّمة بالتصفية).</summary>
    private readonly HashSet<int> _parentIds = [];

    private bool _showCrudActions = true;
    private bool _busy;

    /// <summary>إعادة بناء جارية: تُكتم أحداث الاختيار العابرة حتى لا يسمع الحاضن تحديداً مؤقتاً.</summary>
    private bool _rebuilding;

    private Font? _groupFont;

    /// <summary>يُلغى عند تفريغ المكوّن فيوقف أي تحميل جارٍ بدل أن يكتب في عناصر مُنتهية.</summary>
    private CancellationTokenSource? _lifetimeCts = new();

    public ctrlTreeOfAccounts()
    {
        InitializeComponent();

        // «تصفية بـ» من نموذج مُسمّى: العرض من DisplayMember، والقرار من ByName (RULE-N60).
        cbSearchBy.DisplayMember = nameof(SearchField.Display);
        cbSearchBy.DataSource = SearchFields.ToList();

        // شريط الأوامر المشترك: الشجرة تستمع لأحداثه فقط — ولا تبنيه ولا تحكم على أزراره بنفسها.
        ctrlCrudToolbar1.AddRequested += CtrlToolbar_AddRequested;
        ctrlCrudToolbar1.EditRequested += CtrlToolbar_EditRequested;
        ctrlCrudToolbar1.DeleteRequested += CtrlToolbar_DeleteRequested;
        ctrlCrudToolbar1.RefreshRequested += CtrlToolbar_RefreshRequested;
        ctrlCrudToolbar1.CloseRequested += CtrlToolbar_CloseRequested;
        // SaveRequested/CancelEditRequested غير موصولين عمداً: لا محرّر داخل الشجرة (RULE-N100)،
        // وShowSave يبقى false فيُخفى «حفظ» أصلاً.

        // RULE-N40…RULE-N46 — لوحة المفاتيح (WireGlobalShortcuts تخصّ النماذج ⇒ في الحاضن).
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                        // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(                                                          // RULE-N43
            btnExpandAll, btnCollapseAll, btnDeletePermanent);

        ApplyUiTheme();
        _ApplyActionState();
        _UpdateStatus();
    }

    // ═════════════════════════════════════════════════════════════════
    //  الواجهة العامة (العقد المتفق عليه — لا يُغيَّر شكله)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>معرّف الحساب المحدد (أو <c>null</c> عند عدم وجود اختيار).</summary>
    // WFO1000: واجهة وقت تشغيل لا خاصية مصمّم.
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int? SelectedAccountId => _selectedAccount?.Id;

    /// <summary>عقدة الحساب المحددة (أو <c>null</c>).</summary>
    // WFO1000: واجهة وقت تشغيل لا خاصية مصمّم.
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AccountTreeNodeDto? SelectedAccount => _selectedAccount;

    /// <summary>إخفاء أزرار/عناصر الإضافة والتعديل والحذف (وضع عرض للقراءة فقط).</summary>
    // WFO1000: واجهة وقت تشغيل لا خاصية مصمّم.
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ShowCrudActions
    {
        get => _showCrudActions;
        set
        {
            if (_showCrudActions == value) return;
            _showCrudActions = value;
            _ApplyActionState();
        }
    }

    /// <summary>يُطلق عند كل تغيّر للاختيار (وعند تفريغه يصل <c>null</c>، ومنه عقدة الطبيعة).</summary>
    public event EventHandler<AccountTreeNodeDto?>? AccountSelected;

    /// <summary>يُطلق عند تفعيل عقدة طبيعة (المستوى الأول) بالرقم 1..5 — للحاضن لتصفية شبكته.</summary>
    public event EventHandler<byte>? NatureSelected;

    /// <summary>طلب إضافة حساب فرعي تحت الحساب المحدد — الحاضن يفتح محرّر الحسابات (RULE-N100).</summary>
    public event EventHandler<int>? AddChildRequested;

    /// <summary>طلب تعديل الحساب المحدد — الحاضن يفتح محرّر الحسابات (RULE-N100).</summary>
    public event EventHandler<int>? EditRequested;

    /// <summary>
    /// يربط المكوّن بحاوية الخدمات — إلزامي قبل الاستخدام، وآمن للاستدعاء أكثر من مرة
    /// (نفس الحاوية لا تُعيد التهيئة؛ وحاوية أخرى تُفرّغ ذاكرة الحسابات).
    /// </summary>
    public void Attach(IServiceProvider serviceProvider)
    {
        if (serviceProvider is null)
        {
            clsDialogs.Warning(NotAttachedMessage);
            return;
        }

        if (ReferenceEquals(_serviceProvider, serviceProvider)) return;

        _serviceProvider = serviceProvider;
        _accountById.Clear();   // ذاكرة حكم «نظامي» تتبع الخدمة التي جاءت منها
    }

    /// <summary>يحمّل الشجرة من الخدمة ويعيد بناء العقد (يحفظ الاختيار الحالي إن بقي).</summary>
    public async Task RefreshAsync(CancellationToken ct = default)
    {
        if (IsDisposed || Disposing) return;
        if (_busy) return;      // لا تحديثين متوازيين يكتبان فوق بعضهما

        if (_serviceProvider is null)
        {
            clsDialogs.Warning(NotAttachedMessage);
            return;
        }

        int? previousSelection = _selectedAccount?.Id;
        _busy = true;
        _ApplyActionState();
        var previousCursor = Cursor;

        try
        {
            Cursor = Cursors.WaitCursor;   // RULE-N26

            using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct, LifetimeToken);
            var result = await Service.GetTreeAsync(linked.Token);

            if (IsDisposed || Disposing) return;

            if (!result.IsSuccess || result.Value is null)
            {
                Log.Warning("شجرة دليل الحسابات: تعذر تحميل الشجرة — {Error}", result.Error);
                _roots = [];
                _parentIds.Clear();
                _accountById.Clear();
                _RebuildTree();
                clsDialogs.Warning(LoadFailedMessage);
                return;
            }

            _roots = result.Value.ToList();
            _accountById.Clear();      // شجرة جديدة ⇒ لا حكم نظامي قديم
            _parentIds.Clear();
            _IndexFullTree(_roots);

            _RebuildTree();

            if (previousSelection is int id) _SelectNode(id, focusTree: false);
        }
        catch (OperationCanceledException)
        {
            // أُغلقت الشاشة أو أُلغي الطلب — لا شيء لعرضه
        }
        catch (Exception ex)
        {
            // الحدّ الوحيد لهذا المسار (RULE-N205): رسالة عربية بلا ex.Message (RULE-N28/N29)
            Log.Error(ex, "شجرة دليل الحسابات: خطأ أثناء تحميل الشجرة");
            if (IsDisposed || Disposing) return;

            _roots = [];
            _parentIds.Clear();
            _accountById.Clear();
            _RebuildTree();
            clsDialogs.Warning(LoadFailedMessage);
        }
        finally
        {
            _busy = false;

            if (!IsDisposed && !Disposing) Cursor = previousCursor;   // RULE-N26 — الاستعادة دائماً

            _ApplyActionState();
        }
    }

    /// <summary>توسيع كل عقد الشجرة.</summary>
    public void ExpandAll()
    {
        tvAccounts.ExpandAll();

        // إعادة الاختيار إلى مرماه بعد التوسيع حتى لا يضيع السطر المحدد أسفل الشجرة
        if (_selectedAccount is not null)
            _FindNode(tvAccounts.Nodes, _selectedAccount.Id)?.EnsureVisible();
    }

    /// <summary>طيّ كل عقد الشجرة.</summary>
    public void CollapseAll() => tvAccounts.CollapseAll();

    /// <summary>
    /// يوسّع أسلاف الحساب (ومنهم عقدة الطبيعة) ويحدده ويُطلق <see cref="AccountSelected"/>.
    /// إن كان الحساب خارج نتيجة التصفية الحالية تُمسح التصفية أولاً ثم يُحدَّد.
    /// يُعيد <c>false</c> إن لم يوجد الحساب في الشجرة المحمّلة أصلاً.
    /// </summary>
    public bool SelectAccount(int id)
    {
        if (_SelectNode(id, focusTree: true)) return true;

        // الحساب ليس في العرض المصفّى ⇒ امسح التصفية (بلا كبح) وأعد المحاولة
        if (txtSearch.TextLength == 0) return false;

        _ResetSearch();
        return _SelectNode(id, focusTree: true);
    }

    /// <summary>
    /// معرّفات الحساب وكل أبنائه (ترتيب الشجرة: الحساب ثم أبناؤه) — من الشجرة **الكاملة**
    /// لا من العرض المصفّى، لتصفية شبكة الحاضن. تُعيد قائمة فارغة إن لم يوجد المعرّف.
    /// </summary>
    public IReadOnlyList<int> GetSubtreeIds(int accountId)
    {
        var account = _FindInFullTree(_roots, accountId);
        if (account is null) return Array.Empty<int>();

        var ids = new List<int>();
        _CollectTreeIds(account, ids);
        return ids;
    }

    /// <summary>معرّفات كل حسابات طبيعة (1..5) وفروعها — من الشجرة الكاملة، لتصفية شبكة الحاضن.</summary>
    public IReadOnlyList<int> GetNatureIds(byte nature)
    {
        var ids = new List<int>();

        foreach (var root in _roots)
        {
            if (root.Nature != nature) continue;

            _CollectTreeIds(root, ids);
        }

        return ids;
    }

    /// <summary>يجمع المعرّفات بترتيب الشجرة: الحساب أولاً ثم أبناءه recursively.</summary>
    private static void _CollectTreeIds(AccountTreeNodeDto account, List<int> ids)
    {
        ids.Add(account.Id);

        foreach (var child in account.Children) _CollectTreeIds(child, ids);
    }

    /// <summary>يبحث عن حساب في الشجرة الكاملة (لا في العقد المعروضة).</summary>
    private static AccountTreeNodeDto? _FindInFullTree(IReadOnlyList<AccountTreeNodeDto> nodes, int accountId)
    {
        foreach (var node in nodes)
        {
            if (node.Id == accountId) return node;

            var found = _FindInFullTree(node.Children, accountId);
            if (found is not null) return found;
        }

        return null;
    }

    // ═════════════════════════════════════════════════════════════════
    //  الخدمة (RULE-N04)
    // ═════════════════════════════════════════════════════════════════

    private IAccountDesktopService Service => _serviceProvider!.GetRequiredService<IAccountDesktopService>();

    /// <summary>رمز إلغاء عمر المكوّن — يبقى صالحاً (ملغى) بعد تفريغ المكوّن بلا استثناء.</summary>
    private CancellationToken LifetimeToken
        => _lifetimeCts?.Token ?? new CancellationToken(canceled: true);

    // ═════════════════════════════════════════════════════════════════
    //  بناء الشجرة والتصفية
    // ═════════════════════════════════════════════════════════════════

    /// <summary>خط المجموعات (غير الورقية) — خط واحد مشترك يُفرَّغ مع المكوّن.</summary>
    private Font GroupFont => _groupFont ??= new Font("Tahoma", tvAccounts.Font.Size, FontStyle.Bold);

    /// <summary>
    /// إعادة بناء العقد المعروضة من <c>_roots</c> مع التصفية النشطة — **بمستوى أول ثابت**:
    /// خمس عقد طبائع (1..5) ثم الحسابات الجذرية تحت طبيعتها وفروعها كما وصلت من الخدمة.
    /// بلا مطابقة على مستوى الشجرة كلها: عقدة واحدة باهتة بلا معرّف (لا خمس عقد فارغة).
    /// </summary>
    private void _RebuildTree()
    {
        string term = txtSearch.Text.Trim();
        bool filtering = term.Length > 0;

        _rebuilding = true;
        tvAccounts.BeginUpdate();
        try
        {
            _selectedAccount = null;
            _selectedNature = null;
            tvAccounts.Nodes.Clear();

            List<AccountTreeNodeDto> visible = filtering
                ? _Filter(_roots, term, SelectedSearchField)
                : _roots;

            if (filtering && _CountAccountsIn(visible) == 0)
            {
                // «لا توجد حسابات مطابقة» عقدة واحدة للشجرة كلها — لا واحدة لكل طبيعة
                tvAccounts.Nodes.Add(_CreatePlaceholderNode(NoMatchNodeText));
            }
            else
            {
                // الطبائع الخمس حاضرة دائماً (ولو فارغة) فيرى المحاسب هيكل الدليل كاملاً.
                // الطبيعة مضمونة 1..5 من الخدمة (الكيان يرفض غيرها)، فلا تنشأ سادسة.
                foreach (var (nature, _) in NatureNames)
                {
                    var accounts = visible
                        .Where(root => root.Nature == nature)
                        .OrderBy(root => root.AccountCode, StringComparer.Ordinal)
                        .ToList();

                    tvAccounts.Nodes.Add(_CreateNatureNode(nature, accounts));
                }
            }
        }
        finally
        {
            tvAccounts.EndUpdate();
            _rebuilding = false;
        }

        // نتيجة البحث تُعرض مفتوحة كاملة — الغرض أن يرى المستخدم كل مطابقة فوراً
        if (filtering) tvAccounts.ExpandAll();

        _UpdateStatus();
    }

    /// <summary>
    /// عقدة طبيعة (المستوى الأول): خط عريض + لون النص المميّز + وسم <see cref="NatureTag"/>
    /// ونصها «الأصول (1) — 24 حساباً» (العدد من الشجرة المعروضة نفسها).
    /// </summary>
    private TreeNode _CreateNatureNode(byte nature, IReadOnlyList<AccountTreeNodeDto> accounts)
    {
        int count = _CountAccountsIn(accounts);

        var node = new TreeNode(
            NatureDisplayName(nature) + " (" + nature + ") — " + DescribeAccountCount(count))
        {
            Name = "nature" + nature,
            Tag = new NatureTag(nature),
            ForeColor = GreenThemeHelper.TextStrong,
            NodeFont = GroupFont,
        };

        foreach (var account in accounts) node.Nodes.Add(_CreateNode(account));

        return node;
    }

    /// <summary>بناء عقدة حساب وأبنائها recursively — نصها «الكود — الاسم» ووسمها الحساب نفسه.</summary>
    private TreeNode _CreateNode(AccountTreeNodeDto account)
    {
        var node = new TreeNode(account.AccountCode + NodeSeparator + account.NameAr)
        {
            Name = "node" + account.Id,
            Tag = account,
        };

        foreach (var child in account.Children) node.Nodes.Add(_CreateNode(child));

        // المجموعة (غير الورقية) بخط عريض، والحساب الورقي بخط الشجرة العادي
        if (!account.IsLeaf || node.Nodes.Count > 0) node.NodeFont = GroupFont;

        return node;
    }

    /// <summary>عقدة بديلة باهتة بلا معرّف — الاختيار عليها لا يُفعّل أي زر (Tag = null).</summary>
    private static TreeNode _CreatePlaceholderNode(string text)
        => new(text) { ForeColor = GreenThemeHelper.DisabledFore };

    /// <summary>عدد الحسابات في مجموعة عقد (مع الأبناء) — لعدّاد عقدة الطبيعة.</summary>
    private static int _CountAccountsIn(IReadOnlyList<AccountTreeNodeDto> nodes)
    {
        int count = 0;

        foreach (var node in nodes)
        {
            count++;
            count += _CountAccountsIn(node.Children);
        }

        return count;
    }

    /// <summary>اسم الطبيعة (جمع) من الجدول الواحد — لا نص عربي مكرّر في أي موضع آخر.</summary>
    private static string NatureDisplayName(byte nature)
    {
        foreach (var (value, name) in NatureNames)
        {
            if (value == nature) return name;
        }

        return "الطبيعة " + nature;
    }

    /// <summary>صياغة عربية سليمة لعدّ الحسابات داخل عقدة الطبيعة.</summary>
    private static string DescribeAccountCount(int count) => count switch
    {
        0 => "لا حسابات",
        1 => "حساب واحد",
        2 => "حسابان",
        <= 10 => clsFormat.Number(count) + " حسابات",
        _ => clsFormat.Number(count) + " حساباً",
    };

    /// <summary>
    /// تصفية recursive: يُبقي الحساب المطابق **بكل أبنائه** (سياق كامل)، ويُبقي أسلاف
    /// المطابقات وحدها حين لا يطابق الأب نفسه. والنسخ بـ<c>with</c> لا يمسّ الشجرة الأصلية.
    /// </summary>
    private static List<AccountTreeNodeDto> _Filter(
        IReadOnlyList<AccountTreeNodeDto> nodes, string term, SearchField field)
    {
        var result = new List<AccountTreeNodeDto>();

        foreach (var node in nodes)
        {
            if (_Matches(node, term, field))
            {
                result.Add(node);
                continue;
            }

            var matchingChildren = _Filter(node.Children, term, field);
            if (matchingChildren.Count > 0) result.Add(node with { Children = matchingChildren });
        }

        return result;
    }

    /// <summary>مطابقة جزئية على حقل واحد (الكود أو الاسم) — بلا حساسية لحالة الأحرف.</summary>
    private static bool _Matches(AccountTreeNodeDto account, string term, SearchField field)
    {
        string value = field.ByName ? account.NameAr : account.AccountCode;
        return value.Contains(term, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>حقل التصفية المختار — القرار من النموذج لا من النص المعروض (RULE-N60).</summary>
    private SearchField SelectedSearchField
        => cbSearchBy.SelectedItem as SearchField ?? SearchFields[0];

    /// <summary>فهرسة الشجرة **الكاملة**: من له أبناء فعلاً — أساس حكم «لا حذف نهائي لأب».</summary>
    private void _IndexFullTree(IReadOnlyList<AccountTreeNodeDto> nodes)
    {
        foreach (var node in nodes)
        {
            if (node.Children.Count > 0) _parentIds.Add(node.Id);
            _IndexFullTree(node.Children);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  الاختيار وآلة الحالة
    // ═════════════════════════════════════════════════════════════════

    private void tvAccounts_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        // أثناء إعادة البناء لا تُطلق أحداث عابرة: الحدث الحقيقي يأتي من _SelectNode بعده
        if (_rebuilding) return;

        var tag = e.Node?.Tag;
        var account = tag as AccountTreeNodeDto;
        var nature = (tag as NatureTag)?.Nature;

        _selectedAccount = account;
        _selectedNature = nature;

        // الحكم «نظامي؟» لا يوجد في عقدة الشجرة — يُقرأ من الخدمة مرة واحدة لكل معرّف
        if (account is not null) _ = _EnsureAccountInfoAsync(account.Id);

        _ApplyActionState();
        _UpdateStatus();

        // عقدة الطبيعة: الحاضن يعرف الطبيعة من حدثها، ويبقى AccountSelected(null) لمن لا يسمعه
        if (nature is byte selectedNature) NatureSelected?.Invoke(this, selectedNature);

        AccountSelected?.Invoke(this, account);
    }

    /// <summary>يقرأ بيانات الحساب (مرة واحدة لكل معرّف) لمعرفة «نظامي» قبل السماح بالحذف النهائي.</summary>
    private async Task _EnsureAccountInfoAsync(int id)
    {
        if (_accountById.ContainsKey(id) || _serviceProvider is null) return;

        try
        {
            var result = await Service.GetByIdAsync(id);
            _accountById[id] = result.IsSuccess ? result.Value : null;
        }
        catch (Exception ex)
        {
            // الحدّ هنا: نداء خدمة من مسار fire-and-forget (RULE-N205).
            // «مجهول» لا يُسمح له بالحذف النهائي — الحكم يفشل مغلقاً.
            Log.Warning(ex, "شجرة دليل الحسابات: تعذر قراءة بيانات الحساب {AccountId}", id);
            _accountById[id] = null;
        }

        if (IsDisposed || Disposing) return;

        _ApplyActionState();
    }

    /// <summary>
    /// آلة الحالة الوحيدة للأزرار وقائمة السياق (النظير `RULE-N150` للعناصر):
    /// تُستدعى بعد كل اختيار/تحديث/انشغال — وممنوع ضبط Enabled/Visible داخل أي معالج.
    /// <para>
    /// شريط الأوامر المشترك لا يحكم بنفسه: يُغذَّى بثلاثة مدخلات من هنا فقط
    /// (<c>HasSelection</c> · <c>AllowDelete</c> · <c>SetMode</c>) ويحسب أزراره داخلياً.
    /// </para>
    /// </summary>
    private void _ApplyActionState()
    {
        bool idle = !_busy;
        bool hasSelection = _selectedAccount is not null;
        bool allowDelete = idle && !_IsSystemAccount(_selectedAccount);
        bool canDeletePermanently = idle && _CanDeletePermanently(_selectedAccount);
        bool hasNodes = tvAccounts.Nodes.Count > 0;

        // حارس حساب الحركة: لا «إضافة فرعي» تحت حساب ورقي نهائي (RULE: الترحيل على الأوراق فقط)
        bool leafSelection = _selectedAccount is { IsLeaf: true };
        bool canAddChild = idle && hasSelection && !leafSelection;

        // ── الشريط المشترك (البيت الواحد) ──
        ctrlCrudToolbar1.Visible = _showCrudActions;
        if (_showCrudActions)
        {
            ctrlCrudToolbar1.HasSelection = hasSelection && idle;
            ctrlCrudToolbar1.AllowDelete = allowDelete;
            // الشجرة ليست محرّراً: إنشاء/تعديل الحساب بيته frmAccounts (RULE-N100) ⇒ وضع العرض دائماً
            ctrlCrudToolbar1.SetMode(CrudMode.Browsing);
        }

        // ── الصف الثانوي الخاص بالشجرة ──
        btnExpandAll.Enabled = idle && hasNodes;
        btnCollapseAll.Enabled = idle && hasNodes;
        btnDeletePermanent.Visible = _showCrudActions;
        btnDeletePermanent.Enabled = canDeletePermanently;

        // ── قائمة السياق تعكس نفس المدخلات (لا حكم ثالث) ──
        miAddChild.Visible = _showCrudActions;
        miEdit.Visible = _showCrudActions;
        miDelete.Visible = _showCrudActions;
        miDeletePermanent.Visible = _showCrudActions;

        // «إضافة حساب فرعي» يُعطَّل على الورقة هنا (مكان الحكم الوحيد) — لا داخل معالج نقر
        miAddChild.Enabled = canAddChild;
        miEdit.Enabled = hasSelection && idle;
        miDelete.Enabled = hasSelection && allowDelete;
        miDeletePermanent.Enabled = canDeletePermanently;
        miRefresh.Enabled = idle;
        miExpandAll.Enabled = btnExpandAll.Enabled;
        miCollapseAll.Enabled = btnCollapseAll.Enabled;
    }

    /// <summary>
    /// هل الحساب نظامي **مؤكَّد**؟ «المجهول» ليس نظامياً هنا: الحذف العادي ترفضه الخدمة
    /// نفسها للحسابات النظامية، أما الحذف النهائي فيبقى يفشل مغلقاً في <see cref="_CanDeletePermanently"/>.
    /// </summary>
    private bool _IsSystemAccount(AccountTreeNodeDto? account)
        => account is not null
           && _accountById.TryGetValue(account.Id, out var dto)
           && dto is { IsSystem: true };

    /// <summary>
    /// الحذف النهائي مسموح فقط لحساب: غير نظامي **مؤكَّد**، وبلا حسابات فرعية.
    /// «غير معروف» (لم تصل بياناته من الخدمة) = غير مسموح.
    /// </summary>
    private bool _CanDeletePermanently(AccountTreeNodeDto? account)
    {
        if (account is null) return false;
        if (_HasChildren(account)) return false;

        return _accountById.TryGetValue(account.Id, out var dto) && dto is { IsSystem: false };
    }

    /// <summary>
    /// هل الحساب أب؟ الحكم من الشجرة **الكاملة** لا من العقدة المقلَّمة بالتصفية
    /// (وإلا ظهر أب بلا أبناء أثناء بحث فسُمح بحذفه).
    /// </summary>
    private bool _HasChildren(AccountTreeNodeDto account)
        => !account.IsLeaf || _parentIds.Contains(account.Id);

    /// <summary>تحديد برمجي — يُوسّع الأسلاف ويُطلق الحدث (يدوياً عند عدم تغيّر الاختيار).</summary>
    private bool _SelectNode(int id, bool focusTree)
    {
        var node = _FindNode(tvAccounts.Nodes, id);
        if (node is null) return false;

        _ExpandAncestors(node);

        if (ReferenceEquals(tvAccounts.SelectedNode, node))
        {
            // الشجرة تُطلق AfterSelect عند تغيّر الاختيار فقط — نُطلق الحدث يدوياً كي لا يضيع الطلب
            _selectedAccount = node.Tag as AccountTreeNodeDto;
            _selectedNature = (node.Tag as NatureTag)?.Nature;
            _ApplyActionState();
            _UpdateStatus();
            AccountSelected?.Invoke(this, _selectedAccount);
        }
        else
        {
            tvAccounts.SelectedNode = node;   // يُطلق AfterSelect
        }

        node.EnsureVisible();
        if (focusTree) tvAccounts.Focus();

        return true;
    }

    private static void _ExpandAncestors(TreeNode node)
    {
        for (var parent = node.Parent; parent is not null; parent = parent.Parent) parent.Expand();
    }

    private static TreeNode? _FindNode(TreeNodeCollection nodes, int id)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.Tag is AccountTreeNodeDto account && account.Id == id) return node;

            var found = _FindNode(node.Nodes, id);
            if (found is not null) return found;
        }

        return null;
    }

    /// <summary>
    /// شريط «ملخص»: عدد الحسابات المعروضة (من <c>clsFormat.Number</c>) + المحدد
    /// (حساب: «الكود — الاسم» · عقدة طبيعة: اسمها وعدد المعروض من حساباتها).
    /// </summary>
    private void _UpdateStatus()
    {
        string text = "عدد الحسابات المعروضة: " + clsFormat.Number(_CountAccounts(tvAccounts.Nodes));

        if (_selectedAccount is not null)
        {
            text += "   |   المحدد: " + _selectedAccount.AccountCode + NodeSeparator + _selectedAccount.NameAr;
        }
        else if (_selectedNature is byte nature)
        {
            var natureNode = _FindNatureNode(tvAccounts.Nodes, nature);
            int shown = natureNode is null ? 0 : _CountAccounts(natureNode.Nodes);

            text += "   |   المحدد: " + NatureDisplayName(nature) + " (" + nature + ") — " + DescribeAccountCount(shown);
        }

        lblSummary.Text = text;
    }

    /// <summary>عقدة الطبيعة المعروضة (المستوى الأول دائماً) — للعدّ والوصف في شريط الملخص.</summary>
    private static TreeNode? _FindNatureNode(TreeNodeCollection nodes, byte nature)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.Tag is NatureTag tag && tag.Nature == nature) return node;
        }

        return null;
    }

    /// <summary>يعد الحسابات المعروضة (لا يعدّ العقدة البديلة التي بلا معرّف).</summary>
    private static int _CountAccounts(TreeNodeCollection nodes)
    {
        int count = 0;

        foreach (TreeNode node in nodes)
        {
            if (node.Tag is AccountTreeNodeDto) count++;
            count += _CountAccounts(node.Nodes);
        }

        return count;
    }

    // ═════════════════════════════════════════════════════════════════
    //  التصفية الفورية (بالكبح) والقوائم
    // ═════════════════════════════════════════════════════════════════

    private void txtSearch_TextChanged(object? sender, EventArgs e)
    {
        // تصفية فورية بكبح 250 مللي: إعادة بناء الشجرة مع كل ضغطة تُرهق الدليل الكبير
        searchTimer.Stop();
        searchTimer.Start();
    }

    private void searchTimer_Tick(object? sender, EventArgs e)
    {
        searchTimer.Stop();
        _RebuildTree();
    }

    private void cbSearchBy_SelectedIndexChanged(object? sender, EventArgs e)
    {
        // تغيير الحقل المصفّى يُعيد التصفية فوراً — إلا إن لم يكن هناك نص فالقائمة كاملة أصلاً
        if (txtSearch.TextLength == 0) return;
        _RebuildTree();
    }

    /// <summary>مسح التصفية **فوراً** (بلا كبح المؤقّت) — يستخدمه <see cref="SelectAccount"/>.</summary>
    private void _ResetSearch()
    {
        if (txtSearch.TextLength == 0) return;

        txtSearch.Text = string.Empty;   // يُطلق TextChanged فيُشغّل المؤقّت
        searchTimer.Stop();              // لا كبح: البناء الآن لا بعد 250 مللي
        _RebuildTree();
    }

    // ═════════════════════════════════════════════════════════════════
    //  الأوامر
    // ═════════════════════════════════════════════════════════════════

    // ── أوامر الشريط المشترك: كل حدث يصل إلى دالة واحدة هنا (لا منطق في العنصر) ──

    /// <summary>«+ إضافة» ⇒ نطلب إضافة حساب فرعي تحت المحدد (المحرّر في frmAccounts — RULE-N100).</summary>
    private void CtrlToolbar_AddRequested(object? sender, EventArgs e)
    {
        if (_selectedAccount is not AccountTreeNodeDto account)
        {
            clsDialogs.Warning(NoSelectionMessage);
            return;
        }

        // حارس حساب الحركة: الورقة لا تقبل أبناء (الترحيل على التفصيلي فقط) ⇒ لا نُطلق الحدث إطلاقاً
        if (account.IsLeaf)
        {
            clsDialogs.Warning(AddUnderLeafMessage);
            return;
        }

        AddChildRequested?.Invoke(this, account.Id);
    }

    /// <summary>«تعديل» ⇒ نطلب تعديل الحساب المحدد.</summary>
    private void CtrlToolbar_EditRequested(object? sender, EventArgs e)
    {
        if (_selectedAccount is not AccountTreeNodeDto account)
        {
            clsDialogs.Warning(NoSelectionMessage);
            return;
        }

        EditRequested?.Invoke(this, account.Id);
    }

    /// <summary>«حذف» — يُطلق التحميل بلا انتظار؛ والتعطيل الفعلي في <see cref="_DeleteAsync"/>.</summary>
    private async void CtrlToolbar_DeleteRequested(object? sender, EventArgs e)
    {
        try
        {
            await _DeleteAsync(permanent: false);
        }
        catch (Exception ex)
        {
            // الحدّ الوحيد لمسار الحذف (RULE-N205) — رسالة عربية بلا ex.Message
            Log.Error(ex, "شجرة دليل الحسابات: تعذر تعطيل الحساب");
            clsDialogs.Error("تعذر تعطيل الحساب. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>«تحديث بيانات» — يُطلق التحميل بلا انتظار؛ و<see cref="RefreshAsync"/> لا يرمي (RULE-N205).</summary>
    private void CtrlToolbar_RefreshRequested(object? sender, EventArgs e) => _ = RefreshAsync();

    /// <summary>«خروج» — يُغلق الشاشة الحاضنة (العنصر نفسه لا يملك نوافذ).</summary>
    private void CtrlToolbar_CloseRequested(object? sender, EventArgs e) => FindForm()?.Close();

    private void btnExpandAll_Click(object? sender, EventArgs e) => ExpandAll();

    private void btnCollapseAll_Click(object? sender, EventArgs e) => CollapseAll();

    private async void btnDeletePermanent_Click(object? sender, EventArgs e)
    {
        try
        {
            await _DeleteAsync(permanent: true);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "شجرة دليل الحسابات: تعذر الحذف النهائي للحساب");
            clsDialogs.Error("تعذر الحذف النهائي للحساب. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// تعطيل أو حذف نهائي — بلا catch (حدّه معالج الضغط) وبلا تكرار للحكم:
    /// كل الشروط في مكان واحد، وتأكيد صريح قبل كل عملية.
    /// </summary>
    private async Task _DeleteAsync(bool permanent)
    {
        if (_serviceProvider is null)
        {
            clsDialogs.Warning(NotAttachedMessage);
            return;
        }

        if (_selectedAccount is not AccountTreeNodeDto account)
        {
            clsDialogs.Warning(NoSelectionMessage);
            return;
        }

        if (_busy) return;

        if (permanent)
        {
            await _EnsureAccountInfoAsync(account.Id);

            if (_HasChildren(account))
            {
                clsDialogs.Warning(DeleteRefusedChildrenMessage);
                return;
            }

            if (!_CanDeletePermanently(account))
            {
                // «نظامي» و«مجهول» رفضان مختلفان: لا نوهم المستخدم بأنه حساب نظامي بلا دليل
                bool known = _accountById.TryGetValue(account.Id, out var info) && info is not null;
                clsDialogs.Warning(known ? DeleteRefusedSystemMessage : DeleteRefusedUnknownMessage);
                return;
            }

            if (!clsDialogs.Confirm(
                    "تحذير: سيُحذف الحساب \"" + account.NameAr + "\" (" + account.AccountCode + ") نهائياً " +
                    "من قاعدة البيانات ولا يمكن التراجع عن هذه العملية.\n" +
                    "إن كان الحساب مستخدماً في قيود أو فواتير فسيرفض النظام الحذف.\n\n" +
                    "هل أنت متأكد من الحذف النهائي؟"))
            {
                return;
            }
        }
        else if (!clsDialogs.Confirm("سيتم تعطيل الحساب المحدد. هل تريد المتابعة؟"))
        {
            return;
        }

        _busy = true;
        _ApplyActionState();
        var previousCursor = Cursor;
        bool deleted = false;

        try
        {
            Cursor = Cursors.WaitCursor;   // RULE-N26

            var result = permanent
                ? await Service.PermanentDeleteAsync(account.Id)
                : await Service.DeleteAsync(account.Id);

            if (result.IsSuccess)
            {
                deleted = true;
                clsDialogs.Success(permanent ? "تم حذف الحساب نهائياً." : "تم تعطيل الحساب بنجاح.");
            }
            else
            {
                // نص الخدمة العربي كما هو (RULE-N12) — لا ex.Message
                clsDialogs.Error(result.Error ?? "تعذر تنفيذ العملية. يرجى إعادة المحاولة.");
            }
        }
        finally
        {
            _busy = false;
            if (!IsDisposed && !Disposing) Cursor = previousCursor;
            _ApplyActionState();
        }

        if (deleted) await RefreshAsync();
    }

    // ═════════════════════════════════════════════════════════════════
    //  المظهر والتنظيف
    // ═════════════════════════════════════════════════════════════════

    private void ApplyUiTheme()
    {
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this);   // آخر سطر دائماً (RULE-N50)
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // إيقاف أي تحميل جارٍ أولاً — فلا يكتب في عناصر فُرِّغت بعد قليل
            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();
            _lifetimeCts = null;

            if (_groupFont is not null)
            {
                _groupFont.Dispose();
                _groupFont = null;
            }

            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    // ── عناصر داخلية ──────────────────────────────────────────────────────────

    /// <summary>حقل تصفية: نص معروض (DisplayMember) + هل التصفية بالاسم؟ — لا مقارنة بنص عربي.</summary>
    private sealed record SearchField(string Display, bool ByName)
    {
        // احتياط عرض إن سقط DisplayMember لأي سبب — القرار يبقى من ByName لا من النص.
        public override string ToString() => Display;
    }

    /// <summary>وسم عقدة الطبيعة (المستوى الأول الثابت) — رقم الطبيعة فقط، والاسم من الجدول الواحد.</summary>
    private sealed record NatureTag(byte Nature);
}
