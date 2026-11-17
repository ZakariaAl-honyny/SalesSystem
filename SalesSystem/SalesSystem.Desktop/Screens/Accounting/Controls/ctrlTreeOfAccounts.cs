// -----------------------------------------------------------------------------
// ctrlTreeOfAccounts.cs — شجرة دليل الحسابات (F-003 · المرحلة أ: Mock/واجهة فقط)
//
// • عنصر قابل للتضمين: يعرض دليل الحسابات هرمياً (أب ← أبناء) من
//   IAccountDesktopService.GetTreeAsync — لا وصول لأي طبقة أدنى (RULE-N05 / RULE-N06).
// • الغرض: **إيجاد الحساب بسرعة** — تصفية فورية بالكود أو بالاسم (كبح 250 مللي)
//   تُظهر الحسابات المطابقة **مع أسلافها** ثم تُوسّع النتيجة، وبلا مطابقة تُعرض
//   عقدة واحدة باهتة «لا توجد حسابات مطابقة» (لا استثناء ولا MessageBox).
// • CRUD من الشجرة بقرار المالك، بلا بناء محرّر ثانٍ (RULE-N100): «إضافة حساب فرعي»
//   و«تعديل» يُطلقان حدثاً فقط ويُفتح frmAccounts في الحاضن؛ و«حذف»/«حذف نهائي»
//   ينادِيان الخدمة بعد clsDialogs.Confirm.
// • آلة الحالة واحدة `_ApplyActionState()` تُستدعى بعد كل تغيير (اختيار · تحديث · انشغال)
//   — ولا يُضبط Enabled/Visible داخل معالج (النظير `RULE-N150` للعناصر).
// • الحدود الوحيدة لـtry/catch (RULE-N205): RefreshAsync · قراءة بيانات الحساب
//   (fire-and-forget) · معالجا الحذف (async void). وما عداها بلا catch.
//
// التجميع (§2.34 / RULE-N135…N138): أوامر الحسابات ← بحث وتصفية ← الشجرة ← شريط الحالة.
// -----------------------------------------------------------------------------

using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Controls;

/// <summary>
/// شجرة دليل الحسابات: عرض هرمي + بحث فوري + عمليات (إضافة فرعي · تعديل · حذف · حذف نهائي).
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

    private const string NoMatchNodeText = "لا توجد حسابات مطابقة";
    private const string EmptyTreeText = "لا توجد حسابات في الدليل";

    /// <summary>الفاصل بين الكود والاسم في نص العقدة (قرار الشكل: «الكود — الاسم»).</summary>
    private const string NodeSeparator = " — ";

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

        // RULE-N40…RULE-N46 — لوحة المفاتيح (WireGlobalShortcuts تخصّ النماذج ⇒ في الحاضن).
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                        // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(                                                          // RULE-N43
            btnAddChild, btnEdit, btnDelete, btnDeletePermanent, btnRefresh, btnExpandAll, btnCollapseAll);

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

    /// <summary>يُطلق عند كل تغيّر للاختيار (وعند تفريغه يصل <c>null</c>).</summary>
    public event EventHandler<AccountTreeNodeDto?>? AccountSelected;

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
    /// يوسّع أسلاف الحساب ويحدده ويُطلق <see cref="AccountSelected"/>.
    /// يُعيد <c>false</c> إن لم يكن الحساب معروضاً الآن (مثلاً استبعده بحث نشط).
    /// </summary>
    public bool SelectAccount(int id) => _SelectNode(id, focusTree: true);

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
    /// إعادة بناء العقد المعروضة من <c>_roots</c> مع التصفية النشطة.
    /// بلا مطابقة: عقدة واحدة باهتة بلا معرّف — لا استثناء ولا MessageBox.
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
            tvAccounts.Nodes.Clear();

            List<AccountTreeNodeDto> visible = filtering
                ? _Filter(_roots, term, SelectedSearchField)
                : _roots;

            if (visible.Count == 0)
            {
                tvAccounts.Nodes.Add(_CreatePlaceholderNode(filtering ? NoMatchNodeText : EmptyTreeText));
            }
            else
            {
                foreach (var root in visible) tvAccounts.Nodes.Add(_CreateNode(root));
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

    /// <summary>بناء عقدة واحدة وأبنائها recursively — نصها «الكود — الاسم» ووسمها الحساب نفسه.</summary>
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

        var account = e.Node?.Tag as AccountTreeNodeDto;
        _selectedAccount = account;

        // الحكم «نظامي؟» لا يوجد في عقدة الشجرة — يُقرأ من الخدمة مرة واحدة لكل معرّف
        if (account is not null) _ = _EnsureAccountInfoAsync(account.Id);

        _ApplyActionState();
        _UpdateStatus();
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
    /// آلة الحالة الوحيدة للأزرار وعناصر قائمة السياق (النظير `RULE-N150` للعناصر):
    /// تُستدعى بعد كل اختيار/تحديث/انشغال — وممنوع ضبط Enabled/Visible داخل أي معالج.
    /// </summary>
    private void _ApplyActionState()
    {
        bool crudVisible = _showCrudActions;
        bool hasSelection = _selectedAccount is not null;
        bool idle = !_busy;
        bool hasNodes = tvAccounts.Nodes.Count > 0;

        btnAddChild.Visible = crudVisible;
        btnEdit.Visible = crudVisible;
        btnDelete.Visible = crudVisible;
        btnDeletePermanent.Visible = crudVisible;

        miAddChild.Visible = crudVisible;
        miEdit.Visible = crudVisible;
        miDelete.Visible = crudVisible;
        miDeletePermanent.Visible = crudVisible;

        btnAddChild.Enabled = idle && hasSelection;
        btnEdit.Enabled = idle && hasSelection;
        btnDelete.Enabled = idle && hasSelection;
        btnDeletePermanent.Enabled = idle && _CanDeletePermanently(_selectedAccount);

        btnRefresh.Enabled = idle;
        btnExpandAll.Enabled = idle && hasNodes;
        btnCollapseAll.Enabled = idle && hasNodes;

        // قائمة السياق تعرض حالة الأزرار نفسها — لا حكم ثانٍ ولا نسخة ثانية (RULE-N169)
        miAddChild.Enabled = btnAddChild.Enabled;
        miEdit.Enabled = btnEdit.Enabled;
        miDelete.Enabled = btnDelete.Enabled;
        miDeletePermanent.Enabled = btnDeletePermanent.Enabled;
        miRefresh.Enabled = btnRefresh.Enabled;
        miExpandAll.Enabled = btnExpandAll.Enabled;
        miCollapseAll.Enabled = btnCollapseAll.Enabled;
    }

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

    /// <summary>شريط الحالة: عدد الحسابات المعروضة (من <c>clsFormat.Number</c>) + الحساب المحدد.</summary>
    private void _UpdateStatus()
    {
        string text = "عدد الحسابات المعروضة: " + clsFormat.Number(_CountAccounts(tvAccounts.Nodes));

        if (_selectedAccount is not null)
            text += "   |   المحدد: " + _selectedAccount.AccountCode + NodeSeparator + _selectedAccount.NameAr;

        lblStatus.Text = text;
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

    // ═════════════════════════════════════════════════════════════════
    //  الأوامر
    // ═════════════════════════════════════════════════════════════════

    private void btnAddChild_Click(object? sender, EventArgs e)
    {
        if (_selectedAccount is not AccountTreeNodeDto account)
        {
            clsDialogs.Warning(NoSelectionMessage);
            return;
        }

        // لا محرّر هنا (RULE-N100): الحدث يفتح frmAccounts في الحاضن
        AddChildRequested?.Invoke(this, account.Id);
    }

    private void btnEdit_Click(object? sender, EventArgs e)
    {
        if (_selectedAccount is not AccountTreeNodeDto account)
        {
            clsDialogs.Warning(NoSelectionMessage);
            return;
        }

        EditRequested?.Invoke(this, account.Id);
    }

    /// <summary>«تحديث» — يُطلق التحميل بلا انتظار؛ و<see cref="RefreshAsync"/> لا يرمي (RULE-N205).</summary>
    private void btnRefresh_Click(object? sender, EventArgs e) => _ = RefreshAsync();

    private void btnExpandAll_Click(object? sender, EventArgs e) => ExpandAll();

    private void btnCollapseAll_Click(object? sender, EventArgs e) => CollapseAll();

    private async void btnDelete_Click(object? sender, EventArgs e)
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
        lblStatus.ForeColor = GreenThemeHelper.TextStrong;

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
}
