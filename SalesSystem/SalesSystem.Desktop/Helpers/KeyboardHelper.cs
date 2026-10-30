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
