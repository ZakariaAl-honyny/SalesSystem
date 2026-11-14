using System.ComponentModel;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Controls.Common;

public sealed class MoneyTextBox : TextBox
{
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public decimal DecimalValue
    {
        get => clsFormat.TryParseMoney(Text, out var v) ? v : 0m;
        set => Text = clsFormat.Money(value);
    }

    public MoneyTextBox()
    {
        TextAlign = HorizontalAlignment.Left;
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);

        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
        {
            e.Handled = true;
            clsInputFeedback.ShowRejectTooltip(this, "يُسمح فقط بالأرقام والنقطة العشرية.");
        }

        if (e.KeyChar == '.' && Text.Contains('.'))
        {
            e.Handled = true;
            clsInputFeedback.ShowRejectTooltip(this, "يوجد نقطة عشرية بالفعل.");
        }
    }

    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);

        // العرض وإرجاع الصفر يستهلكان البيت الواحد (clsFormat) فلا منازل مثبّتة ولا قيمة أولية حرفية (GAP-12 / RULE-N179)
        Text = clsFormat.TryParseMoney(Text, out var value) ? clsFormat.Money(value) : clsFormat.Money(0m);
    }
}



