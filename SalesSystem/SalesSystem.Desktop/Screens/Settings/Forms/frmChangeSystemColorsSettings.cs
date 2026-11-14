using Microsoft.Extensions.DependencyInjection;
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

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmChangeSystemColorsSettings()
    {
        InitializeComponent();
        _BindPreviewSamples();
    }

    public frmChangeSystemColorsSettings(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر التعطيل الموحّد لأزرار الحالة
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnReset, btnPickColor, btnClearFilters);

        // RULE-N47 — الاختصارات الموحّدة: F2 اختيار اللون · F5 تحديث · Esc إغلاق
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnPickColor.PerformClick()),
            (Keys.F5, () => { _ = _LoadAsync(); }),
            (Keys.Escape, Close));

        // RULE-N147 / RULE-N153 — Enter = «حفظ وتطبيق» خارج الشبكة فقط
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

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
        ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Secondary);

        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvColors);
        GreenThemeHelper.ApplyGrid(dgvPreview);

        lblSummary.ForeColor = GreenThemeHelper.TextStrong;
        lblNoData.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblPreviewHint.ForeColor = GreenThemeHelper.TextStrong;
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
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(result.Error ?? "تعذر تحميل ألوان النظام.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("تعذر تحميل ألوان النظام. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    /// <summary>إعادة تحديد سطر المفتاح المطلوب بعد إعادة بناء الشبكة، أو إفراغ التحديد.</summary>
    private void _RestoreSelection(string? key)
    {
        _isBinding = true;
        try
        {
            dgvColors.ClearSelection();

            if (!string.IsNullOrEmpty(key))
            {
                foreach (DataGridViewRow row in dgvColors.Rows)
                {
                    if (row.Tag is string rowKey && string.Equals(rowKey, key, StringComparison.Ordinal))
                    {
                        row.Selected = true;
                        dgvColors.CurrentCell = row.Cells["colLabel"];
                        _selectedKey = key;
                        return;
                    }
                }
            }

            _selectedKey = null;
            dgvColors.CurrentCell = null;
        }
        finally
        {
            _isBinding = false;
        }
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
            MessageBox.Show("يرجى اختيار لون من القائمة أولاً.", "تنبيه",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using var dialog = new ColorDialog
            {
                FullOpen = true,
                AllowFullOpen = true,
                AnyColor = true,
                Color = _Color(definition.Key)
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            _working[definition.Key] = dialog.Color;

            _selectedKey = definition.Key;
            _ApplyFilter();                     // السطر + HEX + الملخص + المعاينة + الحالة
            _RestoreSelection(definition.Key);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار لون النظام للمفتاح {Key}", definition.Key);
            MessageBox.Show("تعذر فتح لوحة الألوان. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void miCopyHex_Click(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtHex.Text)) return;

            Clipboard.SetText(txtHex.Text);
            MessageBox.Show("تم نسخ قيمة اللون.", "تم النسخ",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر نسخ قيمة اللون");
            MessageBox.Show("تعذر نسخ قيمة اللون. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(result.Error ?? "تعذر حفظ الألوان.", "لم يتم الحفظ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // النجاح فقط: تطبيق الألوان على كل النوافذ المفتوحة.
            ThemePalette.ApplyToOpenForms();

            string? selected = _selectedKey;
            await _LoadAsync();          // إعادة القراءة من المخزن (المصدر الوحيد للحقيقة)
            _RestoreSelection(selected);
            _UpdateSelectedColorPanel();
            _ApplyPreviewColors();

            MessageBox.Show("تم حفظ الألوان وتطبيقها.", "تم الحفظ",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ ألوان النظام");
            MessageBox.Show("حدث خطأ أثناء حفظ الألوان. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void btnReset_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("هل تريد استعادة الألوان الافتراضية؟", "تأكيد الاستعادة",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        {
            return;
        }

        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await DesktopService.ResetToDefaultsAsync();

            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Error ?? "تعذر استعادة الألوان الافتراضية.", "لم تتم الاستعادة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ThemePalette.ApplyToOpenForms();

            _selectedKey = null;
            await _LoadAsync();
            _UpdateSelectedColorPanel();
            _ApplyPreviewColors();

            MessageBox.Show("تمت استعادة الألوان الافتراضية.", "تمت الاستعادة",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء استعادة الألوان الافتراضية");
            MessageBox.Show("حدث خطأ أثناء استعادة الألوان. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        lblPreviewCaption.ForeColor = _Color(ThemePalette.TextStrongColor);
        lblPreviewHint.ForeColor = _Color(ThemePalette.TextStrongColor);
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
