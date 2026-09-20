using System.ComponentModel;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Controls.Common;

public sealed class MoneyTextBox : TextBox
{
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public decimal DecimalValue
    {
        get => decimal.TryParse(Text, out var v) ? v : 0m;
        set => Text = value.ToString("F2");
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
        if (decimal.TryParse(Text, out var value))
        {
            Text = value.ToString("F2");
        }
        else
        {
            Text = "0.00";
        }
    }
}



