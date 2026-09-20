namespace SalesSystem.Desktop.Helpers;

public static class KeyboardHelper
{
    public static void WireEnterAsTab(Control parent)
    {
        foreach (Control ctrl in parent.Controls)
        {
            if (ctrl is TextBox or ComboBox)
            {
                ctrl.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
                    {
                        parent.SelectNextControl(ctrl, true, true, true, true);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                };
            }

            if (ctrl.HasChildren)
                WireEnterAsTab(ctrl);
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
