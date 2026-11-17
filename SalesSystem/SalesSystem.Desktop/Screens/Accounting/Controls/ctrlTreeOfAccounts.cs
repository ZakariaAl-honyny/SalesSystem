// -----------------------------------------------------------------------------
// ctrlTreeOfAccounts.cs — شجرة عرض دليل الحسابات (قراءة فقط)
//
// • عنصر قابل للتضمين يعرض دليل الحسابات هرمياً (أب ← أبناء) من
//   IAccountDesktopService.GetTreeAsync — لا وصول لأي طبقة أدنى (RULE-N05 / RULE-N06).
// • عرض فقط: لا أزرار ولا تصفية ولا شريط ملخص داخل العنصر — الأوامر كلها ملك
//   الشاشة الحاضنة (frmAccounts)، وهذا العنصر يعرض الشكل الشجري ويُبلّغ عن الاختيار.
// • المستوى الأول ثابت: خمس عقد طبائع («الأصول (1) — 24 حساباً» … «المصروفات (5)»)،
//   ثم الحسابات الجذرية تحت طبيعتها وفروعها كما وصلت من الخدمة (بلا حد للعمق)، فيرى
//   المحاسب هيكل الدليل كاملاً حتى لو كانت طبيعة ما بلا حسابات.
// • الاختيار يُبلَّغ بحدث واحد AccountSelected (وقد يكون null على عقدة طبيعة):
//   الحاضن يستخدمه لتعبئة المحرّر وتحديد السطر في الشبكة.
// • الشجرة هي **البيت الواحد** لمعرفة الهيكل (RULE-N169): GetSubtreeIds (الحساب وفروعه)
//   وGetNatureIds (كل حسابات طبيعة) وSelectedNature — فلا تُحسب الهرمية في الحاضن.
// • RULE-N40…RULE-N46 — تنقل لوحة المفاتيح يُوصَّل مرة واحدة في المُنشئ.
// • RULE-N205: try/catch واحد عند حدّ التحميل (RefreshAsync) — لا catch داخل مسارات أخرى.
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
/// شجرة عرض دليل الحسابات: مستوى أول ثابت بخمس طبائع ثم هرمية الحسابات الكاملة (أي عمق).
/// <para><b>الاستخدام:</b> <see cref="Attach"/> ثم <see cref="RefreshAsync"/>، والاستماع إلى
/// <see cref="AccountSelected"/>، وقيادة التحديد عبر <see cref="SelectAccount"/>.</para>
/// </summary>
public partial class ctrlTreeOfAccounts : UserControl
{
    // ── رسائل عربية (RULE-N28) ────────────────────────────────────────────────
    private const string NotAttachedMessage = "لم يتم تهيئة شجرة الحسابات — تعذر الوصول إلى خدمة دليل الحسابات.";
    private const string LoadFailedMessage = "تعذر تحميل دليل الحسابات.";

    /// <summary>الفاصل بين الكود والاسم في نص العقدة (قرار الشكل: «الكود — الاسم»).</summary>
    private const string NodeSeparator = " — ";

    /// <summary>بادئة اسم عقدة الطبيعة («nature1» … «nature5») — مصدر واحد للاسم ولقراءته.</summary>
    private const string NatureNodePrefix = "nature";

    /// <summary>
    /// الطبائع الخمس بترتيبها الرقمي (AccountNature) — **المصدر الوحيد** لتسميات المستوى الأول
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

    private IServiceProvider? _serviceProvider;

    /// <summary>الشجرة الكاملة كما وصلت من الخدمة (المصدر الوحيد لإعادة البناء).</summary>
    private List<AccountTreeNodeDto> _roots = [];

    /// <summary>الحساب المحدد حالياً (null عند اختيار عقدة طبيعة أو لا اختيار).</summary>
    private AccountTreeNodeDto? _selectedAccount;

    private bool _busy;

    /// <summary>إعادة بناء جارية: تُكتم أحداث الاختيار العابرة حتى لا يسمع الحاضن تحديداً مؤقتاً.</summary>
    private bool _rebuilding;

    private Font? _groupFont;

    /// <summary>يُلغى عند تفريغ المكوّن فيوقف أي تحميل جارٍ بدل أن يكتب في عناصر مُنتهية.</summary>
    private CancellationTokenSource? _lifetimeCts = new();

    public ctrlTreeOfAccounts()
    {
        InitializeComponent();

        // RULE-N40…RULE-N46 — لوحة المفاتيح (WireGlobalShortcuts تخصّ النماذج ⇒ في الحاضن).
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                        // RULE-N45

        ApplyUiTheme();
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

    /// <summary>
    /// طبيعة الفرع المختار (1..5) — عقدة حساب ⇒ طبيعتها من وسمها، وعقدة طبيعة ⇒ رقمها من
    /// اسمها، ولا اختيار ⇒ <c>null</c>. تُستهلك لتصفية الشبكة على طبيعة كاملة.
    /// </summary>
    // WFO1000: واجهة وقت تشغيل لا خاصية مصمّم.
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public byte? SelectedNature { get; private set; }

    /// <summary>يُطلق عند كل تغيّر للاختيار (وعند اختيار عقدة طبيعة أو تفريغه يصل <c>null</c>).</summary>
    public event EventHandler<AccountTreeNodeDto?>? AccountSelected;

    /// <summary>
    /// يربط المكوّن بحاوية الخدمات — إلزامي قبل الاستخدام، وآمن للاستدعاء أكثر من مرة.
    /// </summary>
    public void Attach(IServiceProvider serviceProvider)
    {
        if (serviceProvider is null)
        {
            clsDialogs.Warning(NotAttachedMessage);
            return;
        }

        _serviceProvider = serviceProvider;
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
                _RebuildTree();
                clsDialogs.Warning(LoadFailedMessage);
                return;
            }

            _roots = result.Value.ToList();
            _RebuildTree();

            if (previousSelection is int id) SelectAccount(id);
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
            _RebuildTree();
            clsDialogs.Warning(LoadFailedMessage);
        }
        finally
        {
            _busy = false;

            if (!IsDisposed && !Disposing) Cursor = previousCursor;   // RULE-N26 — الاستعادة دائماً
        }
    }

    /// <summary>
    /// يوسّع أسلاف الحساب (ومنهم عقدة الطبيعة) ويحدده ويُطلق <see cref="AccountSelected"/>.
    /// يُعيد <c>false</c> إن لم يوجد الحساب في الشجرة المحمّلة.
    /// </summary>
    public bool SelectAccount(int id)
    {
        var node = _FindNode(tvAccounts.Nodes, id);
        if (node is null) return false;

        _ExpandAncestors(node);

        // الطبيعة تُضبط هنا أيضاً: الحدث اليدوي أدناه لا يمرّ بـAfterSelect فلا يُترك الحقل قديماً
        SelectedNature = _NatureOf(node);

        if (ReferenceEquals(tvAccounts.SelectedNode, node))
        {
            // الشجرة تُطلق AfterSelect عند تغيّر الاختيار فقط — نُطلق الحدث يدوياً كي لا يضيع الطلب
            _selectedAccount = node.Tag as AccountTreeNodeDto;
            AccountSelected?.Invoke(this, _selectedAccount);
        }
        else
        {
            tvAccounts.SelectedNode = node;   // يُطلق AfterSelect
        }

        node.EnsureVisible();

        return true;
    }

    /// <summary>
    /// يُفرغ اختيار الشجرة (بلا إطلاق حدث تحديد يدوي): «مسح التصفية» و«تحديث» في الحاضن
    /// يريدان رفع تصفية العقدة، فلا يبقى سطر محدد يوهم بأن التصفية قائمة.
    /// </summary>
    public void ClearSelection()
    {
        if (tvAccounts.SelectedNode is null) return;

        // الحقلان يُفرغان صراحةً: لا نعتمد على AfterSelect فقد لا يُرفع بلا اختيار سابق.
        _selectedAccount = null;
        SelectedNature = null;

        tvAccounts.SelectedNode = null;
    }

    // ═════════════════════════════════════════════════════════════════
    //  استعلامات الهيكل (البيت الواحد — RULE-N169)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// معرّفات الحساب وفروعه (هو نفسه أولاً) من الشجرة المحمّلة <c>_roots</c> — بلا قراءة ثانية.
    /// قائمة فارغة إن لم يوجد الحساب.
    /// </summary>
    public IReadOnlyList<int> GetSubtreeIds(int accountId)
    {
        var found = _FindInRoots(_roots, accountId);
        if (found is null) return [];

        var ids = new List<int>();
        _CollectIds(found, ids);

        return ids;
    }

    /// <summary>معرّفات كل حسابات طبيعة (1..5) وفروعها — لعقدة الطبيعة في المستوى الأول.</summary>
    public IReadOnlyList<int> GetNatureIds(byte nature)
    {
        var ids = new List<int>();

        foreach (var root in _roots)
        {
            if (root.Nature != nature) continue;

            _CollectIds(root, ids);
        }

        return ids;
    }

    /// <summary>
    /// يجمع معرّف العقدة ومعرّفات أبنائها (أي عمق) في القائمة المعطاة. المشي على `_roots`
    /// لا على `TreeView` — فمعرفة الهيكل تعيش هنا مرة واحدة.
    /// </summary>
    private static void _CollectIds(AccountTreeNodeDto node, List<int> ids)
    {
        ids.Add(node.Id);

        foreach (var child in node.Children) _CollectIds(child, ids);
    }

    /// <summary>البحث عن حساب داخل شجرة الخدمة (بأي عمق) — لا يمسّ `TreeView`.</summary>
    private static AccountTreeNodeDto? _FindInRoots(IReadOnlyList<AccountTreeNodeDto> nodes, int id)
    {
        foreach (var node in nodes)
        {
            if (node.Id == id) return node;

            var found = _FindInRoots(node.Children, id);
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
    //  بناء الشجرة
    // ═════════════════════════════════════════════════════════════════

    /// <summary>خط المجموعات (غير الورقية) — خط واحد مشترك يُفرَّغ مع المكوّن.</summary>
    private Font GroupFont => _groupFont ??= new Font("Tahoma", tvAccounts.Font.Size, FontStyle.Bold);

    /// <summary>
    /// إعادة بناء العقد المعروضة من <c>_roots</c> — **بمستوى أول ثابت**:
    /// خمس عقد طبائع (1..5) ثم الحسابات الجذرية تحت طبيعتها وفروعها كما وصلت من الخدمة.
    /// الطبائع الخمس حاضرة دائماً (ولو فارغة) فيرى المحاسب هيكل الدليل كاملاً.
    /// </summary>
    private void _RebuildTree()
    {
        _rebuilding = true;
        tvAccounts.BeginUpdate();
        try
        {
            _selectedAccount = null;
            SelectedNature = null;   // لا فرع مختار بعد إعادة البناء
            tvAccounts.Nodes.Clear();

            // الطبيعة مضمونة 1..5 من الخدمة (الكيان يرفض غيرها)، فلا تنشأ سادسة.
            foreach (var (nature, _) in NatureNames)
            {
                var accounts = _roots
                    .Where(root => root.Nature == nature)
                    .OrderBy(root => root.AccountCode, StringComparer.Ordinal)
                    .ToList();

                tvAccounts.Nodes.Add(_CreateNatureNode(nature, accounts));
            }
        }
        finally
        {
            tvAccounts.EndUpdate();
            _rebuilding = false;
        }
    }

    /// <summary>
    /// عقدة طبيعة (المستوى الأول): خط عريض + لون النص المميّز
    /// ونصها «الأصول (1) — 24 حساباً» (العدد من الشجرة المعروضة نفسها).
    /// </summary>
    private TreeNode _CreateNatureNode(byte nature, IReadOnlyList<AccountTreeNodeDto> accounts)
    {
        int count = _CountAccountsIn(accounts);

        var node = new TreeNode(
            NatureName(nature) + " (" + nature + ") — " + DescribeAccountCount(count))
        {
            Name = NatureNodePrefix + nature,
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

    /// <summary>
    /// اسم الطبيعة (جمع) من الجدول الواحد — **النص الوحيد** لتسمية الطبيعة في النظام كله:
    /// تستدعيه الشجرة وعناصر التصفية في الشاشة الحاضنة (RULE-N172).
    /// </summary>
    public static string NatureName(byte nature)
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

    // ═════════════════════════════════════════════════════════════════
    //  الاختيار
    // ═════════════════════════════════════════════════════════════════

    private void tvAccounts_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        // أثناء إعادة البناء لا تُطلق أحداث عابرة: الحدث الحقيقي يأتي من SelectAccount بعده
        if (_rebuilding) return;

        _selectedAccount = e.Node?.Tag as AccountTreeNodeDto;

        // الطبيعة تُشتق من العقدة نفسها: حساب ⇒ طبيعته، طبيعة ⇒ رقمها — والحاضن يصفّي بها
        SelectedNature = _NatureOf(e.Node);

        AccountSelected?.Invoke(this, _selectedAccount);
    }

    /// <summary>
    /// طبيعة العقدة المختارة: عقدة حساب ⇒ طبيعتها من وسمها (<c>Tag</c>)؛ عقدة طبيعة ⇒ رقمها
    /// من اسمها («nature1» … «nature5»)؛ لا اختيار ⇒ <c>null</c>. حكم واحد في مكان واحد.
    /// </summary>
    private static byte? _NatureOf(TreeNode? node)
    {
        if (node?.Tag is AccountTreeNodeDto account) return account.Nature;

        string name = node?.Name ?? string.Empty;
        if (!name.StartsWith(NatureNodePrefix, StringComparison.Ordinal)) return null;

        return byte.TryParse(name.AsSpan(NatureNodePrefix.Length), out byte nature) ? nature : null;
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

    // ═════════════════════════════════════════════════════════════════
    //  المظهر والتنظيف
    // ═════════════════════════════════════════════════════════════════

    private void ApplyUiTheme()
    {
        tvAccounts.BackColor = Color.White;
        tvAccounts.ForeColor = Color.FromArgb(33, 37, 41);

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
}
