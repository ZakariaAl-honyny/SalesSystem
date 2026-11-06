namespace SalesSystem.Desktop.Helpers;

public static class KeyboardHelper
{
    /// <summary>اسم حقل البحث — لا يُطبَّق عليه تحديد النص عند دخول المؤشر (لتسهيل الكتابة المتتابعة).</summary>
    private const string SearchControlName = "txtSearch";

    /// <summary>
    /// يحوّل مفتاح Enter إلى انتقال للحقل التالي (مثل Tab) في كل أنواع حقول الإدخال:
    /// TextBox / MaskedTextBox / ComboBox / NumericUpDown / DateTimePicker.
    /// </summary>
    public static void WireEnterAsTab(Control parent)
    {
        foreach (Control ctrl in parent.Controls)
        {
            if (IsEnterNavigationInput(ctrl))
            {
                var input = ctrl;
                input.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
                    {
                        // البحث عن الحقل التالي من جذر النموذج حتى يعبر حدود الحاويات (GroupBox / Panel).
                        var container = input.FindForm() ?? parent;
                        container.SelectNextControl(input, true, true, true, true);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                };
            }

            if (ctrl.HasChildren)
                WireEnterAsTab(ctrl);
        }
    }

    /// <summary>
    /// عند دخول المؤشر (Enter/got-focus) إلى حقل إدخال، يحدد كل قيمته الحالية
    /// حتى تستبدلها الكتابة مباشرة دون استخدام الفأرة.
    /// لا يُطبَّق على حقل البحث (<c>txtSearch</c>) ولا على القوائم المنسدلة المغلقة (DropDownList).
    /// </summary>
    public static void WireSelectAllOnFocus(Control parent)
    {
        foreach (Control ctrl in parent.Controls)
        {
            if (IsSelectAllTarget(ctrl))
                ctrl.Enter += SelectAllCurrentValue;

            if (ctrl.HasChildren)
                WireSelectAllOnFocus(ctrl);
        }
    }

    /// <summary>
    /// تظليل الحقل النشط: عند دخول المؤشر إلى حقل إدخال تُصبغ خلفيته بلون التظليل،
    /// وعند مغادرته تُعاد خلفيته الأصلية (يُحفظ لون كل حقل في قاموس منفصل).
    /// </summary>
    public static void WireFocusHighlight(Control parent, Color focusColor)
    {
        var originalColors = new Dictionary<Control, Color>();

        foreach (Control ctrl in parent.Controls)
        {
            if (IsFocusHighlightTarget(ctrl))
            {
                var input = ctrl;
                originalColors[input] = input.BackColor;

                input.Enter += (s, e) => input.BackColor = focusColor;
                input.Leave += (s, e) => input.BackColor = originalColors.TryGetValue(input, out var original)
                    ? original
                    : SystemColors.Window;
            }

            if (ctrl.HasChildren)
                WireFocusHighlight(ctrl, focusColor);
        }
    }

    /// <summary>
    /// التنقل بالأسهم بين حقول الإدخال: السهم الأعلى/الأيسر ينتقل للحقل السابق،
    /// والأسفل/الأيمن ينتقل للحقل التالي (مثل Tab). لا يُطبَّق على القوائم المنسدلة
    /// ولا على الشبكات حتى تحتفظ بسلوك الأسهم الأصلي.
    /// </summary>
    public static void WireArrowNavigation(Control parent)
    {
        foreach (Control ctrl in parent.Controls)
        {
            if (IsArrowNavigationInput(ctrl))
            {
                var input = ctrl;
                input.KeyDown += (s, e) =>
                {
                    if (e.Modifiers != Keys.None) return;

                    bool backward = e.KeyCode == Keys.Up || e.KeyCode == Keys.Left;
                    bool forward = e.KeyCode == Keys.Down || e.KeyCode == Keys.Right;

                    if (!backward && !forward) return;

                    // البحث عن الحقل التالي من جذر النموذج حتى يعبر حدود الحاويات (GroupBox / Panel).
                    var container = input.FindForm() ?? parent;
                    container.SelectNextControl(input, forward, true, true, true);

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                };
            }

            if (ctrl.HasChildren)
                WireArrowNavigation(ctrl);
        }
    }

    private static bool IsFocusHighlightTarget(Control ctrl)
        => ctrl is TextBox or MaskedTextBox or NumericUpDown or ComboBox;

    private static bool IsArrowNavigationInput(Control ctrl)
        => ctrl is TextBox or MaskedTextBox or NumericUpDown;

    private static bool IsEnterNavigationInput(Control ctrl)
        => ctrl is TextBox or MaskedTextBox or ComboBox or NumericUpDown or DateTimePicker;

    private static bool IsSelectAllTarget(Control ctrl)
    {
        if (string.Equals(ctrl.Name, SearchControlName, StringComparison.OrdinalIgnoreCase))
            return false;

        return ctrl switch
        {
            MaskedTextBox => true,
            TextBox => true,
            NumericUpDown => true,
            ComboBox cb => cb.DropDownStyle != ComboBoxStyle.DropDownList,
            _ => false,
        };
    }

    private static void SelectAllCurrentValue(object? sender, EventArgs e)
    {
        switch (sender)
        {
            case MaskedTextBox mtb:
                mtb.SelectAll();
                break;

            case TextBox tb when tb.CanSelect:
                tb.SelectAll();
                break;

            case NumericUpDown nud:
                nud.Select(0, nud.Text.Length);
                break;

            case ComboBox cb when cb.DropDownStyle != ComboBoxStyle.DropDownList:
                cb.SelectAll();
                break;
        }
    }

    public static void WireEnterToClick(Control trigger, Button target)
    {
        trigger.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
            {
                target.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        };
    }

    public static void WireEnterToClick(Control trigger, Action action)
    {
        trigger.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
            {
                action();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        };
    }

    public static void WireGlobalShortcuts(Form form, params (Keys Key, Action Action)[] shortcuts)
    {
        form.KeyPreview = true;
        form.KeyDown += (s, e) =>
        {
            foreach (var (key, action) in shortcuts)
            {
                if (e.KeyData == key)
                {
                    action();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                }
            }
        };
    }
}
