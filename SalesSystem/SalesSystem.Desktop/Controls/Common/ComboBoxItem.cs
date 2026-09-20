namespace SalesSystem.Desktop.Controls.Common;

/// <summary>
/// Shared ComboBoxItem for report tab filters.
/// </summary>
public class ComboBoxItem
{
    public string Text { get; set; } = "";
    public object? Value { get; set; }
    public override string ToString() => Text;
}
