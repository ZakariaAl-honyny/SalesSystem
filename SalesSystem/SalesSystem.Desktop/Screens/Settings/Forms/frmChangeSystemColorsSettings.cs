using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Settings.Forms;

/// <summary>
/// شاشة ألوان النظام (المرحلة أ — Mock · UI &amp; UX QA).
/// <para>
/// شاشة إعدادات واحدة: تعرض الألوان الـ23 القادمة من <see cref="ThemePalette.Definitions"/>
/// (المصدر الوحيد للصفوف والتسميات والمجموعات والقيم الافتراضية)، وتسمح بتغيير لون واحد
/// بمربع حوار الألوان، مع معاينة فورية داخل الشاشة **بلا** مسّ بقية التطبيق حتى الضغط على
/// «حفظ وتطبيق».
/// </para>
/// <para>
/// لا تحفظ الشاشة في أي قاعدة بيانات: تخاطب <see cref="IThemeColorDesktopService"/> فقط
/// (المرحلة أ: <c>MockThemeColorDesktopService</c>). ولا تُطبَّق الألوان على النوافذ المفتوحة
/// إلا بعد نجاح الحفظ عبر <c>ThemePalette.ApplyToOpenForms()</c>.
/// </para>
/// </summary>
public partial class frmChangeSystemColorsSettings : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IThemeColorDesktopService DesktopService
        => _serviceProvider.GetRequiredService<IThemeColorDesktopService>();

    // مفاتيح الألوان كلها من ThemePalette فقط (RULE-N54/RULE-N112):
    // لا نص مفتاح مكرّر هنا — الصفوف والتسميات والمجموعات والافتراضي من ThemePalette.Definitions،
    // والمفاتيح المفردة من ثوابت ThemePalette.Xxx.

    private const string Title = "ألوان النظام — إعدادات العرض";
    private const string AllGroups = "كل المجموعات";
    private const string FilterAll = "الكل";

    // ── State ────────────────────────────────────────────────────────────────
    /// <summary>الألوان الحالية في الشاشة (مسودّة غير محفوظة بعد).</summary>
    private readonly Dictionary<string, Color> _working = new(StringComparer.Ordinal);

    /// <summary>الألوان كما هي في المخزن — أساس حساب «المعدّل».</summary>
    private readonly Dictionary<string, Color> _baseline = new(StringComparer.Ordinal);

    private IReadOnlyList<ThemeColorDefinition> _definitions = Array.Empty<ThemeColorDefinition>();

    /// <summary>يمنع إعادة الدخول إلى التصفية/الملخص أثناء البناء البرمجي.</summary>
    private bool _isBinding;

    private string? _selectedKey;

    /// <summary>خط أزرار الاقتراحات (مشترك — لا خط جديد لكل زر في كل إعادة بناء).</summary>
    private static readonly Font SuggestionFont = new("Tahoma", 8F, FontStyle.Bold);

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmChangeSystemColorsSettings()
    {
        InitializeComponent();
        _BindPreviewSamples();
    }

    public frmChangeSystemColorsSettings(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط (اللون من اللوحة القابلة للتخصيص)
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر التعطيل الموحّد لأزرار الحالة
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnReset, btnPickColor, btnBestSuggestion, btnClearFilters);

        // RULE-N47 — الاختصارات الموحّدة: F2 اختيار اللون · Ctrl+B أفضل اقتراح · Ctrl+V لصق قيمة اللون
        //                  · F5 تحديث · Esc إغلاق
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnPickColor.PerformClick()),
            (Keys.Control | Keys.B, () => btnBestSuggestion.PerformClick()),
            (Keys.Control | Keys.V, () => _PasteColor()),
            (Keys.F5, () => { _ = _LoadAsync(); }),
            (Keys.Escape, Close));

        // RULE-N147 / RULE-N153 — Enter = «حفظ وتطبيق» خارج الشبكة فقط
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // حقل HEX يقبل الكتابة واللصق اليدوي: Enter يقرأ القيمة فوراً، وLeave يثبّتها قبل مغادرة الحقل.
        // التحليل نفسه في ThemePalette.TryParseColorText وحده (RULE-N169…N172) — لا تحليل هنا.
        txtHex.KeyDown += txtHex_KeyDown;
        txtHex.Leave += txtHex_Leave;

        ApplyUiTheme();

        // RULE-N63 — طيّ/إظهار صندوق التصفية (يتبنّى btnToggleFilters المُعلَن في المصمّم)
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    // ── Theme (RULE-N50…RULE-N54) ─────────────────────────────────────────────

    /// <summary>يُطبَّق مرة واحدة في المُنشئ، وينتهي بـ <c>GreenThemeHelper.Apply(this)</c> (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);

        ThemeHelper.ApplyButtonStyle(btnSave, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnReset, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnFooterClose, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnPickColor, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnBestSuggestion, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Secondary);

        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvColors);
        GreenThemeHelper.ApplyGrid(dgvPreview);

        lblSummary.ForeColor = GreenThemeHelper.TextStrong;
        lblNoData.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewHint.ForeColor = GreenThemeHelper.TextStrong;
        lblSuggestions.ForeColor = GreenThemeHelper.TextStrong;
        lblLadder.ForeColor = GreenThemeHelper.TextStrong;
        lblFooterHint.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً
    }

    // ── Load / Reload ─────────────────────────────────────────────────────────

    private async void frmChangeSystemColorsSettings_Load(object? sender, EventArgs e)
    {
        try
        {
            await _LoadAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة ألوان النظام");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// يقرأ الألوان من المخزن ويبني الشبكة والمعاينة وكل الحالات. لا يرمي استثناءً
    /// (كل المسارات محمية) لذلك يصلح للاستدعاء من «تحديث»/F5 مباشرة.
    /// </summary>
    private async Task _LoadAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await DesktopService.GetAsync();
            var settings = result.Value;

            if (!result.IsSuccess || settings is null)
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل ألوان النظام.");
                return;
            }

            _definitions = ThemePalette.Definitions;

            _working.Clear();
            _baseline.Clear();

            foreach (var definition in _definitions)
            {
                Color color = definition.DefaultValue;

                if (settings.Colors is not null
                    && settings.Colors.TryGetValue(definition.Key, out var hex)
                    && ThemePalette.TryParseHex(hex, out var parsed))
                {
                    color = parsed;
                }

                _working[definition.Key] = color;
                _baseline[definition.Key] = color;
            }

            _BindGroupFilter();
            _ApplyFilter();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحميل ألوان النظام");
            clsDialogs.Error("تعذر تحميل ألوان النظام. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>صيغة <c>cbGroup</c>: «كل المجموعات» + المجموعات الخمس من التعريفات.</summary>
    private void _BindGroupFilter()
    {
        _isBinding = true;
        try
        {
            string selected = cbGroup.SelectedItem as string ?? AllGroups;

            cbGroup.Items.Clear();
            cbGroup.Items.Add(AllGroups);

            foreach (var group in _definitions.Select(d => d.GroupLabel).Distinct(StringComparer.Ordinal))
                cbGroup.Items.Add(group);

            int index = cbGroup.Items.IndexOf(selected);
            cbGroup.SelectedIndex = index >= 0 ? index : 0;
        }
        finally
        {
            _isBinding = false;
        }
    }

    private void _BindPreviewSamples()
    {
        dgvPreview.Rows.Clear();
        dgvPreview.Rows.Add("فاتورة 001", "120.00");
        dgvPreview.Rows.Add("فاتورة 002", "75.50");
        dgvPreview.ClearSelection();
    }

    // ── Filter (RULE-N56…N63 / RULE-N148) ─────────────────────────────────────

    private void Filter_Changed(object? sender, EventArgs e)
    {
        if (_isBinding) return;
        _ApplyFilter();
    }

    /// <summary>
    /// تصفية **فورية** في الذاكرة على التعريفات (RULE-N59/N60): لا <c>DataView.RowFilter</c>
    /// ولا استخدام خام لإدخال المستخدم. تُعيد بناء صفوف الشبكة والملخص والمعاينة.
    /// </summary>
    private void _ApplyFilter()
    {
        string text = txtFilterValue.Text.Trim();
        string mode = cbFilterBy.SelectedItem as string ?? FilterAll;
        string group = cbGroup.SelectedItem as string ?? AllGroups;
        bool onlyChanged = chkOnlyChanged.Checked;

        string? selected = _selectedKey;
        int visible = 0;
        int changed = 0;
        int number = 0;

        _isBinding = true;
        try
        {
            dgvColors.Rows.Clear();

            foreach (var definition in _definitions)
            {
                number++;

                bool isChanged = _IsChanged(definition.Key);
                if (isChanged) changed++;

                if (!_Matches(definition, text, mode, group, onlyChanged, isChanged)) continue;

                visible++;

                int rowIndex = dgvColors.Rows.Add(
                    number.ToString(),
                    definition.GroupLabel,
                    definition.ArabicLabel,
                    definition.Key,
                    ThemePalette.ToHex(_Color(definition.Key)),
                    string.Empty);

                dgvColors.Rows[rowIndex].Tag = definition.Key;
            }

            lblSummary.Text = $"عدد الألوان: {visible} · المعدّل: {changed}";

            // الحالة الفارغة: تظهر فقط عندما لا تُطابق التصفية أي لون (لا سطر وهمي في الشبكة).
            lblNoData.Visible = visible == 0;
        }
        finally
        {
            _isBinding = false;
        }

        _RestoreSelection(selected);
        _UpdateSelectedColorPanel();
        _RebuildSuggestions();
        _ApplyPreviewColors();
        _UpdateClearFiltersState();
        _ApplyModeState();
    }

    private static bool _Matches(
        ThemeColorDefinition definition,
        string text,
        string mode,
        string group,
        bool onlyChanged,
        bool isChanged)
    {
        if (onlyChanged && !isChanged) return false;

        if (!string.Equals(group, AllGroups, StringComparison.Ordinal)
            && !string.Equals(definition.GroupLabel, group, StringComparison.Ordinal))
            return false;

        if (string.IsNullOrWhiteSpace(text)) return true;

        bool labelHit = definition.ArabicLabel.Contains(text, StringComparison.OrdinalIgnoreCase);
        bool keyHit = definition.Key.Contains(text, StringComparison.OrdinalIgnoreCase);
        bool groupHit = definition.GroupLabel.Contains(text, StringComparison.OrdinalIgnoreCase);

        return mode switch
        {
            "اللون" => labelHit,
            "المفتاح" => keyHit,
            "المجموعة" => groupHit,
            _ => labelHit || keyHit || groupHit
        };
    }

    /// <summary>RULE-N148: «مسح التصفية» يمسح الأدوات فقط بلا إعادة تحميل ولا لمس للألوان.</summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearFilterInputs();
        _ApplyFilter();
        txtFilterValue.Focus();
    }

    private void _ClearFilterInputs()
    {
        _isBinding = true;
        try
        {
            txtFilterValue.Clear();
            cbFilterBy.SelectedIndex = cbFilterBy.Items.Count > 0 ? 0 : -1;
            cbGroup.SelectedIndex = cbGroup.Items.Count > 0 ? 0 : -1;
            chkOnlyChanged.Checked = false;
        }
        finally
        {
            _isBinding = false;
        }
    }

    /// <summary>RULE-N43: الزر مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || cbFilterBy.SelectedIndex > 0
            || cbGroup.SelectedIndex > 0
            || chkOnlyChanged.Checked;
    }

    // ── Grid ──────────────────────────────────────────────────────────────────

    private void dgvColors_SelectionChanged(object? sender, EventArgs e)
    {
        if (_isBinding) return;

        if (dgvColors.CurrentRow?.Tag is string key)
            _selectedKey = key;

        _UpdateSelectedColorPanel();
        _RebuildSuggestions();
        _ApplyModeState();
    }

    /// <summary>عمود «عيّنة» يُلوَّن بلون السطر نفسه (RULE-N22 لا يُمسّ: الشبكة للقراءة فقط).</summary>
    private void dgvColors_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgvColors.Rows[e.RowIndex].Tag is not string key) return;
        if (dgvColors.Columns[e.ColumnIndex].Name != "colSample") return;

        var color = _Color(key);
        e.CellStyle.BackColor = color;
        e.CellStyle.SelectionBackColor = color;
        e.Value = string.Empty;
        e.FormattingApplied = true;
    }

    private void dgvColors_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgvColors.Rows[e.RowIndex].Tag is string key) _selectedKey = key;

        _UpdateSelectedColorPanel();
        _PickColor();
    }

    /// <summary>
    /// إعادة تحديد سطر المفتاح المطلوب بعد إعادة بناء الشبكة. إن تعذّرت الاستعادة (لا سطر سابق
    /// أو لم يعد مطابقاً للتصفية) يُحدَّد السطر الأول بدل حالة «لا لون محدد» — فتُملأ العيّنة
    /// و HEX والاقتراحات دائماً ما دامت الشبكة غير فارغة (RULE-N44).
    /// </summary>
    private void _RestoreSelection(string? key)
    {
        bool autoSelected = false;

        _isBinding = true;
        try
        {
            dgvColors.ClearSelection();
            _selectedKey = null;

            if (!string.IsNullOrEmpty(key) && _SelectRow(key, out var restored))
            {
                _selectedKey = restored;
            }
            else if (_SelectRow(null, out var first))
            {
                _selectedKey = first;
                autoSelected = true;
            }
            else
            {
                dgvColors.CurrentCell = null;
            }
        }
        finally
        {
            _isBinding = false;
        }

        // التحديد التلقائي يمرّ بمسار تغيّر التحديد نفسه فيُحدّث الشريط والتلميحات (لا منطق ثانٍ).
        if (autoSelected) dgvColors_SelectionChanged(dgvColors, EventArgs.Empty);
    }

    /// <summary>
    /// يحدّد سطر المفتاح المطلوب (أو السطر الأول عندما يكون <paramref name="key"/> فارغاً)
    /// ويضع المؤشر على خلية الاسم، ويُعيد المفتاح المحدَّد. يُستدعى داخل <c>_isBinding</c> فقط.
    /// </summary>
    private bool _SelectRow(string? key, out string? selectedKey)
    {
        selectedKey = null;

        foreach (DataGridViewRow row in dgvColors.Rows)
        {
            if (row.Tag is not string rowKey) continue;
            if (key is not null && !string.Equals(rowKey, key, StringComparison.Ordinal)) continue;

            row.Selected = true;
            dgvColors.CurrentCell = row.Cells["colLabel"];
            selectedKey = rowKey;
            return true;
        }

        return false;
    }

    private void _UpdateSelectedColorPanel()
    {
        var definition = _FindDefinition(_selectedKey);

        if (definition is null)
        {
            lblSelectedColor.Text = "اللون المحدد: —";
            pnlSwatch.BackColor = Color.White;
            txtHex.Text = string.Empty;
            return;
        }

        var color = _Color(definition.Key);
        lblSelectedColor.Text = $"اللون المحدد: {definition.ArabicLabel} ({definition.Key})";
        pnlSwatch.BackColor = color;
        txtHex.Text = ThemePalette.ToHex(color);
    }

    // ── Actions ───────────────────────────────────────────────────────────────

    private void btnPickColor_Click(object? sender, EventArgs e) => _PickColor();

    private void miPickColor_Click(object? sender, EventArgs e) => _PickColor();

    /// <summary>
    /// اختيار لون للسطر المحدد — يعدّل **المسودّة** فقط: السطر والمعاينة والملخص،
    /// ولا يمسّ بقية التطبيق حتى «حفظ وتطبيق».
    /// </summary>
    private void _PickColor()
    {
        var definition = _FindDefinition(_selectedKey);

        if (definition is null)
        {
            clsDialogs.Warning("يرجى اختيار لون من القائمة أولاً.");
            return;
        }

        try
        {
            // اللون الحالي هو نقطة البداية في الحوار (لا الأبيض) — الشفافية تأخذ اللون المختار.
            var current = _Color(definition.Key);

            using var dialog = new ColorDialog
            {
                FullOpen = true,
                AllowFullOpen = true,
                AnyColor = true,
                Color = current,
                // الصف «المخصّص» يعرض اللون الحالي كاملاً حتى لو لم يكن من الألوان الأساسية:
                // ColorDialog.CustomColors يتوقّع BGR لا RGB ⇒ R + G*256 + B*65536.
                CustomColors = BuildCustomColors(current)
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            // نفس مسار الاقتراحات الذكية: تعديل المسودّة فقط بلا مسّ بقية التطبيق.
            _ApplyColorToWorking(definition.Key, dialog.Color);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار لون النظام للمفتاح {Key}", definition.Key);
            clsDialogs.Error("تعذر فتح لوحة الألوان. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// صف الألوان المخصّصة في <see cref="ColorDialog"/>: اللون الحالي في كل الخانات الست عشرة
    /// حتى يظهر «محدَّداً» بجلاء (الحوار الجديد يبدأ بخانات فارغة) — تحويل BGR كما يشترطه الحوار.
    /// </summary>
    private static int[] BuildCustomColors(Color color)
    {
        int bgr = color.R + (color.G * 256) + (color.B * 65536);

        var custom = new int[16];
        for (int i = 0; i < custom.Length; i++)
            custom[i] = bgr;

        return custom;
    }

    private void miPasteColor_Click(object? sender, EventArgs e) => _PasteColor();

    /// <summary>
    /// لصق قيمة لون من الحافظة على السطر المحدد (قائمة السياق · Ctrl+V). التحليل في
    /// <see cref="ThemePalette.TryParseColorText"/> وحده، والتطبيق مسودّة فقط
    /// (<see cref="_ApplyColorToWorking"/>) — لا مسّ ببقية التطبيق حتى «حفظ وتطبيق».
    /// </summary>
    private void _PasteColor()
    {
        var definition = _FindDefinition(_selectedKey);

        if (definition is null)
        {
            clsDialogs.Warning("يرجى اختيار لون من القائمة أولاً.");
            return;
        }

        try
        {
            if (!Clipboard.ContainsText())
            {
                clsDialogs.Warning("الحافظة فارغة — انسخ قيمة لون مثل #0EA567 ثم ألصقها.");
                return;
            }

            if (!ThemePalette.TryParseColorText(Clipboard.GetText(), out var color))
            {
                clsDialogs.Warning("قيمة اللون غير واضحة. استخدم صيغة مثل #0EA567 أو (14,165,103).");
                return;
            }

            _ApplyColorToWorking(definition.Key, color);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر لصق قيمة اللون للمفتاح {Key}", definition.Key);
            clsDialogs.Error("تعذر لصق قيمة اللون. يرجى إعادة المحاولة.");
        }
    }

    private void txtHex_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;
        _CommitHexText();
    }

    private void txtHex_Leave(object? sender, EventArgs e) => _CommitHexText();

    /// <summary>
    /// يقرأ ما كُتب/لُصق في حقل HEX ويطبّقه على المسودّة عبر <see cref="ThemePalette.TryParseColorText"/>
    /// وحده. النص غير المفهوم **لا يغيّر المسودّة** ويُعيد الحقل إلى القيمة الحقيقية حتى لا يكذب.
    /// </summary>
    private void _CommitHexText()
    {
        if (IsDisposed || Disposing) return;

        var definition = _FindDefinition(_selectedKey);

        if (definition is null)
        {
            if (!string.IsNullOrWhiteSpace(txtHex.Text))
            {
                txtHex.Text = string.Empty;
                clsDialogs.Warning("يرجى اختيار لون من القائمة أولاً.");
            }

            return;
        }

        var current = _Color(definition.Key);

        if (!ThemePalette.TryParseColorText(txtHex.Text, out var color))
        {
            txtHex.Text = ThemePalette.ToHex(current);   // لا مسودّة على نص ملتبس + الحقل لا يكذب
            clsDialogs.Warning("قيمة اللون غير واضحة. استخدم صيغة مثل #0EA567 أو (14,165,103).");
            return;
        }

        if (color.ToArgb() == current.ToArgb())
        {
            txtHex.Text = ThemePalette.ToHex(current);   // تطبيع العرض فقط بلا إعادة بناء
            return;
        }

        _ApplyColorToWorking(definition.Key, color);
    }

    // ── Suggestions (ColorSuggestionHelper — اقتراحات جاهزة مجرّبة التباين) ────

    private void btnBestSuggestion_Click(object? sender, EventArgs e) => _ApplyBestSuggestion();

    /// <summary>
    /// «أفضل اقتراح» (Ctrl+B): يطبّق <see cref="ColorSuggestionHelper.BestFor"/> على السطر المحدد
    /// **بنفس مسار اختيار اللون** — تعديل مسودّة غير محفوظة.
    /// </summary>
    private void _ApplyBestSuggestion()
    {
        var definition = _FindDefinition(_selectedKey);
        if (definition is null)
        {
            clsDialogs.Warning("يرجى اختيار لون من القائمة أولاً.");
            return;
        }

        try
        {
            var best = ColorSuggestionHelper.BestFor(definition.Key);
            if (best is null)
            {
                clsDialogs.Warning("لا يوجد اقتراح مناسب لهذا اللون.");
                return;
            }

            _ApplyColorToWorking(definition.Key, best.Value);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تطبيق أفضل اقتراح للمفتاح {Key}", definition.Key);
            clsDialogs.Error("تعذر تطبيق الاقتراح. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// المسار الوحيد لتغيير لون في المسودّة (اختيار يدوي · اقتراح ذكي · أفضل اقتراح):
    /// يحدّث <c>_working</c> ثم السطر والعيّنة و HEX والملخص والمعاينة والحالة — بلا تطبيق على النظام.
    /// </summary>
    private void _ApplyColorToWorking(string key, Color color)
    {
        if (_FindDefinition(key) is null) return;

        _working[key] = color;
        _selectedKey = key;

        _ApplyFilter();          // يعيد بناء السطر + الملخص + المعاينة + الاقتراحات + الحالة
        _RestoreSelection(key);
    }

    /// <summary>
    /// يعيد بناء شريطي الاقتراحات للسطر المحدد (تحميل · تحديد · تغيير لون · حفظ · استعادة).
    /// الأزرار **محتوى بيانات** تُبنى وقت التشغيل (كأزرار تنقل التقارير)؛ أما اللوحات
    /// والعناوين فمُعلَنة في المصمّم (RULE-N145). لا استثناء يخرج من هنا.
    /// </summary>
    private void _RebuildSuggestions()
    {
        try
        {
            _ClearSuggestionPanel(flpSuggestions);
            _ClearSuggestionPanel(flpLadder);

            var definition = _FindDefinition(_selectedKey);
            if (definition is null) return;

            // الاقتراحات المسمّاة ثم سلّم التدرّج — كلها من الـHelper وليس من الشاشة.
            _AddSuggestionButtons(flpSuggestions, definition.Key,
                ColorSuggestionHelper.For(definition.Key), 96, 30);

            _AddSuggestionButtons(flpLadder, definition.Key,
                ColorSuggestionHelper.LadderFor(definition.Key), 62, 30);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر بناء الاقتراحات الذكية للمفتاح {Key}", _selectedKey);
        }
    }

    /// <summary>إفراغ لوحة الاقتراحات مع فصل التلميح والتخلّص من الأزرار (لا تسرّب موارد).</summary>
    private void _ClearSuggestionPanel(FlowLayoutPanel panel)
    {
        while (panel.Controls.Count > 0)
        {
            var control = panel.Controls[0];
            panel.Controls.RemoveAt(0);
            toolTip.SetToolTip(control, string.Empty);   // فصل التلميح قبل التخلص من الزر
            control.Dispose();
        }
    }

    private void _AddSuggestionButtons(
        FlowLayoutPanel panel,
        string key,
        IReadOnlyList<ColorSuggestion> suggestions,
        int width,
        int height)
    {
        if (suggestions is null) return;

        foreach (var suggestion in suggestions)
        {
            var color = suggestion.Color;

            var button = new Button
            {
                Text = suggestion.ArabicLabel,
                Tag = color,
                Size = new Size(width, height),
                Margin = new Padding(0, 0, 4, 4),
                FlatStyle = FlatStyle.Flat,
                Font = SuggestionFont,
                UseVisualStyleBackColor = false,
                BackColor = color,
                // RULE-N54c/N169: «أي نص مقروء فوق هذا اللون؟» قرار واحد في ColorSuggestionHelper
                // (الحد الأدنى وعيّنة النص داخل الـHelper — لا رقم ولا لون أبيض في الشاشة).
                ForeColor = ColorSuggestionHelper.ReadableForeFor(color),
                Cursor = Cursors.Hand,
                TabStop = false
            };

            button.FlatAppearance.BorderSize = 0;
            toolTip.SetToolTip(button, ThemePalette.ToHex(color));

            // الضغط يعدّل المسودّة فقط بلا حفظ ولا تطبيق على بقية التطبيق.
            button.Click += (_, _) => _ApplyColorToWorking(key, color);

            panel.Controls.Add(button);
        }
    }

    /// <summary>هل للسطر المحدد «أفضل اقتراح» جاهز؟ يُدار به تفعيل الزر (RULE-N43).</summary>
    private bool _HasBestSuggestion()
    {
        var definition = _FindDefinition(_selectedKey);
        if (definition is null) return false;

        try
        {
            return ColorSuggestionHelper.BestFor(definition.Key) is not null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر قراءة أفضل اقتراح للمفتاح {Key}", definition.Key);
            return false;
        }
    }

    private void miCopyHex_Click(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtHex.Text)) return;

            Clipboard.SetText(txtHex.Text);
            clsDialogs.Info("تم نسخ قيمة اللون.", "تم النسخ");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر نسخ قيمة اللون");
            clsDialogs.Error("تعذر نسخ قيمة اللون. يرجى إعادة المحاولة.");
        }
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var colors = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var definition in _definitions)
                colors[definition.Key] = ThemePalette.ToHex(_Color(definition.Key));

            var result = await DesktopService.SaveAsync(new ThemeColorSettings(colors));

            if (!result.IsSuccess)
            {
                // رفض المحاكاة (RULE-N167): لا تغيير في الشاشة ولا في التطبيق.
                clsDialogs.Warning(result.Error ?? "تعذر حفظ الألوان.", "لم يتم الحفظ");
                return;
            }

            // النجاح فقط: تطبيق الألوان على كل النوافذ المفتوحة.
            ThemePalette.ApplyToOpenForms();

            string? selected = _selectedKey;
            await _LoadAsync();          // إعادة القراءة من المخزن (المصدر الوحيد للحقيقة)
            _RestoreSelection(selected);
            _UpdateSelectedColorPanel();
            _ApplyPreviewColors();

            clsDialogs.Success("تم حفظ الألوان وتطبيقها.", "تم الحفظ");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ ألوان النظام");
            clsDialogs.Error("حدث خطأ أثناء حفظ الألوان. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void btnReset_Click(object? sender, EventArgs e)
    {
        if (!clsDialogs.Confirm("هل تريد استعادة الألوان الافتراضية؟", "تأكيد الاستعادة"))
        {
            return;
        }

        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await DesktopService.ResetToDefaultsAsync();

            if (!result.IsSuccess)
            {
                clsDialogs.Warning(result.Error ?? "تعذر استعادة الألوان الافتراضية.", "لم تتم الاستعادة");
                return;
            }

            ThemePalette.ApplyToOpenForms();

            _selectedKey = null;
            await _LoadAsync();
            _UpdateSelectedColorPanel();
            _ApplyPreviewColors();

            clsDialogs.Success("تمت استعادة الألوان الافتراضية.", "تمت الاستعادة");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء استعادة الألوان الافتراضية");
            clsDialogs.Error("حدث خطأ أثناء استعادة الألوان. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>«تحديث» (F5): إعادة تحميل كاملة + مسح التصفية والتحديد (RULE-N148).</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        try
        {
            _selectedKey = null;
            _ClearFilterInputs();
            await _LoadAsync();
            _UpdateSelectedColorPanel();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث شاشة ألوان النظام");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    // ── State machine (RULE-N43 / RULE-N44 / RULE-N147 / RULE-N153) ───────────

    /// <summary>
    /// حالة الشاشة المركزية: تفعيل «حفظ» عند وجود تغيير، وتفعيل «اختيار اللون» عند وجود
    /// سطر محدد، وتحديث شريط الوضع، وربط <c>AcceptButton</c> بالزر الأساسي.
    /// تُستدعى بعد كل انتقال (تحميل · تحديد · تغيير لون · حفظ · استعادة · تحديث · تصفية).
    /// </summary>
    private void _ApplyModeState()
    {
        bool hasChanges = _working.Any(pair => _IsChanged(pair.Key));
        bool hasSelection = _FindDefinition(_selectedKey) is not null;

        btnSave.Enabled = hasChanges;
        btnReset.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;
        btnFooterClose.Enabled = true;
        btnPickColor.Enabled = hasSelection;
        btnBestSuggestion.Enabled = hasSelection && _HasBestSuggestion();

        lblMode.Text = hasChanges ? $"{Title} — تغييرات غير محفوظة" : Title;

        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>RULE-N153: Enter داخل الشبكة لا يحفظ أبداً.</summary>
    private Button? _PrimaryActionButton()
        => dgvColors.ContainsFocus || dgvColors.IsCurrentCellInEditMode ? null : btnSave;

    // ── Live preview ──────────────────────────────────────────────────────────

    /// <summary>
    /// يرسم لوحة المعاينة من الألوان **الحالية في الشاشة** (وليس من الثيم المطبَّق)،
    /// لذلك تُظهر المسودّة قبل الحفظ. تُستهلك <c>ThemePalette.Get</c> للألوان الحيّة.
    /// </summary>
    private void _ApplyPreviewColors()
    {
        pnlPreviewShell.BackColor = _Color(ThemePalette.FrmMainColor);

        lblPreviewHeader.BackColor = _Color(ThemePalette.HeaderBarColor);
        lblPreviewHeader.ForeColor = _Color(ThemePalette.HeaderTextColor);

        gbPreview.BackColor = _Color(ThemePalette.GroupBoxColor);
        flpPreviewButtons.BackColor = _Color(ThemePalette.GroupBoxColor);
        flpSuggestions.BackColor = _Color(ThemePalette.GroupBoxColor);
        flpLadder.BackColor = _Color(ThemePalette.GroupBoxColor);

        lblPreviewCaption.ForeColor = _Color(ThemePalette.TextStrongColor);
        lblPreviewHint.ForeColor = _Color(ThemePalette.TextStrongColor);
        lblSuggestions.ForeColor = _Color(ThemePalette.TextStrongColor);
        lblLadder.ForeColor = _Color(ThemePalette.TextStrongColor);
        lblSelectedColor.ForeColor = _Color(ThemePalette.TextStrongColor);
        lblPreviewField.ForeColor = _Color(ThemePalette.TextStrongColor);
        lblFooterHint.ForeColor = _Color(ThemePalette.TextStrongColor);

        txtPreviewInput.BackColor = _Color(ThemePalette.GridCellBackgroundColor);
        txtPreviewInput.ForeColor = _Color(ThemePalette.GridCellTextColor);

        _PaintPreviewButton(btnPreviewAdd, _Color(ThemePalette.BtnAddNewButtonColor));
        _PaintPreviewButton(btnPreviewSave, _Color(ThemePalette.BtnSaveButtonColor));
        _PaintPreviewButton(btnPreviewDelete, _Color(ThemePalette.BtnDeleteButtonColor));
        _PaintPreviewButton(btnPreviewCancel, _Color(ThemePalette.BtnCancelButtonColor));
        _PaintPreviewButton(btnPreviewInfo, _Color(ThemePalette.BtnInfoButtonColor));

        txtPreviewDisabled.BackColor = _Color(ThemePalette.DisabledBackColor);
        txtPreviewDisabled.ForeColor = _Color(ThemePalette.DisabledForeColor);

        dgvPreview.BackgroundColor = _Color(ThemePalette.GridBackgroundColor);
        dgvPreview.GridColor = _Color(ThemePalette.GridLineColor);

        dgvPreview.ColumnHeadersDefaultCellStyle.BackColor = _Color(ThemePalette.GridHeaderColor);
        dgvPreview.ColumnHeadersDefaultCellStyle.SelectionBackColor = _Color(ThemePalette.GridHeaderColor);
        dgvPreview.ColumnHeadersDefaultCellStyle.ForeColor = _Color(ThemePalette.GridHeaderTextColor);

        dgvPreview.DefaultCellStyle.BackColor = _Color(ThemePalette.GridCellBackgroundColor);
        dgvPreview.DefaultCellStyle.ForeColor = _Color(ThemePalette.GridCellTextColor);
        dgvPreview.DefaultCellStyle.SelectionBackColor = _Color(ThemePalette.SelectionRowColor);
        dgvPreview.DefaultCellStyle.SelectionForeColor = _Color(ThemePalette.GridCellTextColor);

        dgvPreview.AlternatingRowsDefaultCellStyle.BackColor = _Color(ThemePalette.GridAlternateRowColor);
        dgvPreview.AlternatingRowsDefaultCellStyle.ForeColor = _Color(ThemePalette.GridCellTextColor);

        dgvPreview.Invalidate();
    }

    private static void _PaintPreviewButton(Button button, Color backColor)
    {
        button.BackColor = backColor;
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.UseVisualStyleBackColor = false;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>اللون الحالي في الشاشة، أو قيمة الثيم إن لم يكن محمّلاً بعد.</summary>
    private Color _Color(string key)
        => _working.TryGetValue(key, out var color) ? color : ThemePalette.Get(key);

    private bool _IsChanged(string key)
        => _working.TryGetValue(key, out var current)
        && _baseline.TryGetValue(key, out var baseline)
        && current.ToArgb() != baseline.ToArgb();

    private ThemeColorDefinition? _FindDefinition(string? key)
    {
        if (string.IsNullOrEmpty(key)) return null;

        foreach (var definition in _definitions)
        {
            if (string.Equals(definition.Key, key, StringComparison.Ordinal))
                return definition;
        }

        return null;
    }
}
