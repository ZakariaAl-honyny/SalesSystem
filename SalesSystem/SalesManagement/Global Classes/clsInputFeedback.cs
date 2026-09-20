using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace SalesManagement.GlobalClasses
{
    public static class clsInputFeedback
    {
        // Plays the system beep and briefly flashes the control's background
        // to visually + audibly notify the user that input was rejected
        public static void RejectInput(Control control)
        {
            SystemSounds.Beep.Play();
            FlashControl(control);
        }

        // Quick red flash on the control to draw attention
        private static void FlashControl(Control control)
        {
            Color originalColor = control.BackColor;

            control.BackColor = Color.FromArgb(255, 220, 220); // Light red

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 200 };
            timer.Tick += (s, e) =>
            {
                control.BackColor = originalColor;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        // Shows a tooltip near the control explaining why input was blocked
        public static void ShowRejectTooltip(Control control, string message, ToolTip toolTip = null)
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
    }
}