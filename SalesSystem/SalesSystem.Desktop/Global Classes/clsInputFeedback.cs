using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace SalesSystem.Desktop.GlobalClasses
{
    public static class clsInputFeedback
    {
        public static void ShowRejectTooltip(Control control, string message, ToolTip? toolTip = null)
        {
            SystemSounds.Exclamation.Play();
            FlashControl(control);

            bool disposeAfter = false;

            if (toolTip == null)
            {
                toolTip = new ToolTip
                {
                    IsBalloon = true,
                    ToolTipIcon = ToolTipIcon.Warning,
                    ToolTipTitle = "تنبيه",
                    UseAnimation = true,
                    UseFading = true,
                };
                disposeAfter = true;
            }

            toolTip.Show(message, control, control.Width / 2, -60, 2000);

            if (disposeAfter)
            {
                System.Windows.Forms.Timer disposeTimer = new System.Windows.Forms.Timer { Interval = 2500 };
                disposeTimer.Tick += (s, e) =>
                {
                    toolTip.Dispose();
                    disposeTimer.Stop();
                    disposeTimer.Dispose();
                };
                disposeTimer.Start();
            }
        }

        private static void FlashControl(Control control)
        {
            Color originalColor = control.BackColor;

            control.BackColor = Color.FromArgb(255, 220, 220);

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 200 };
            timer.Tick += (s, e) =>
            {
                control.BackColor = originalColor;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }
    }
}
