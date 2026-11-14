using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace SalesSystem.Desktop.GlobalClasses
{
    /// <summary>
    /// تغذية بصرية موحّدة لحقول الإدخال: وميض لون قصير يُعاد بعده لون المصمّم + تلميح فوق الحقل.
    /// البيت الواحد لكل تلميح حقل في النظام (<c>RULE-N169</c>): تلميح مشترك واحد لكل حقل، ومؤقّت استعادة
    /// واحد لكل وميض، والتلميح الذي يمرّره المستدعي لا يُتلف أبداً.
    /// </summary>
    public static class clsInputFeedback
    {
        /// <summary>مدة الوميض قبل استعادة لون المصمّم (مللي ثانية).</summary>
        private const int FlashIntervalMs = 200;

        /// <summary>مدة ظهور التلميح (مللي ثانية).</summary>
        private const int TooltipDurationMs = 2000;

        /// <summary>إزاحة التلميح رأسياً فوق الحقل.</summary>
        private const int TooltipOffsetY = -60;

        /// <summary>لون وميض الرفض — ثابت واحد يُستهلك في كل الشاشات.</summary>
        private static readonly Color RejectFlashBack = Color.FromArgb(255, 220, 220);

        /// <summary>تلميح مشترك واحد لكل حقل (يُنشأ هنا ويُتلف هنا فقط).</summary>
        private static readonly Dictionary<Control, ToolTip> SharedToolTips = new();

        /// <summary>مؤقّت الاستعادة الحالي لكل حقل — يُوقَف ويُتلف قبل بدء وميض جديد.</summary>
        private static readonly Dictionary<Control, System.Windows.Forms.Timer> RestoreTimers = new();

        /// <summary>لون المصمّم الأصلي لكل حقل أثناء الوميض — يُحفظ مرة واحدة فقط.</summary>
        private static readonly Dictionary<Control, Color> OriginalBackColors = new();

        /// <summary>رفض إدخال: صوت تنبيه + وميض أحمر + تلميح بالسبب.</summary>
        public static void ShowRejectTooltip(Control control, string message, ToolTip? toolTip = null)
        {
            ShowFeedback(control, message, toolTip, RejectFlashBack, true, ToolTipIcon.Warning, clsDialogs.WarningTitle);
        }

        /// <summary>معلومة إرشادية عن الحقل: تلميح بلا وميض وبلا صوت.</summary>
        public static void ShowInfoTooltip(Control control, string message, ToolTip? toolTip = null)
        {
            ShowFeedback(control, message, toolTip, null, false, ToolTipIcon.Info, clsDialogs.InfoTitle);
        }

        /// <summary>تأكيد نجاح إدخال الحقل: تلميح بلا وميض وبلا صوت.</summary>
        public static void ShowSuccessTooltip(Control control, string message, ToolTip? toolTip = null)
        {
            ShowFeedback(control, message, toolTip, null, false, ToolTipIcon.Info, clsDialogs.SuccessTitle);
        }

        /// <summary>الجسم الوحيد لكل أنواع التلميحات — لا نسخ مكرّرة للسلوك.</summary>
        private static void ShowFeedback(
            Control control,
            string message,
            ToolTip? callerToolTip,
            Color? flashColor,
            bool playAlert,
            ToolTipIcon icon,
            string title)
        {
            if (control is null || control.IsDisposed) return;

            PruneDisposedEntries();

            if (playAlert) SystemSounds.Exclamation.Play();
            if (flashColor.HasValue) Flash(control, flashColor.Value);

            if (control.IsDisposed) return;

            // تلميح المستدعي لا يُحفظ ولا يُتلف — يُستخدم للعرض فقط.
            ToolTip tip = callerToolTip ?? GetSharedToolTip(control, icon, title);

            try
            {
                tip.Show(message ?? string.Empty, control, control.Width / 2, TooltipOffsetY, TooltipDurationMs);
            }
            catch (Exception ex)
            {
                // التلميح تغذية بصرية فقط: فشله لا يُفشل الإدخال ولا يُظهر رسالة للمستخدم.
                Serilog.Log.Error(ex, "تعذر عرض تلميح الحقل {Control}", control.Name);
            }
        }

        /// <summary>يُعيد التلميح المشترك للحقل بعد مطابقة أيقونته وعنوانه، وينشئه أول مرة.</summary>
        private static ToolTip GetSharedToolTip(Control control, ToolTipIcon icon, string title)
        {
            if (SharedToolTips.TryGetValue(control, out ToolTip? existing) && existing is not null)
            {
                existing.ToolTipIcon = icon;
                existing.ToolTipTitle = title;
                return existing;
            }

            ToolTip created = new()
            {
                IsBalloon = true,
                ToolTipIcon = icon,
                ToolTipTitle = title,
                UseAnimation = true,
                UseFading = true,
            };

            SharedToolTips[control] = created;
            return created;
        }

        /// <summary>يُومض الحقل بلون الرفض ويستعيد لون المصمّم بعد <see cref="FlashIntervalMs"/>.</summary>
        private static void Flash(Control control, Color flashColor)
        {
            // لون المصمّم يُحفظ مرة واحدة حتى لا يتحوّل وميض متكرر إلى اللون الأصلي.
            if (!OriginalBackColors.ContainsKey(control))
            {
                OriginalBackColors[control] = control.BackColor;
            }

            StopRestoreTimer(control);

            control.BackColor = flashColor;

            System.Windows.Forms.Timer timer = new() { Interval = FlashIntervalMs };
            timer.Tick += (_, _) => RestoreColor(control, timer);

            RestoreTimers[control] = timer;
            timer.Start();
        }

        /// <summary>يستعيد لون المصمّم ويتلف مؤقّته — يُستدعى من المؤقّت نفسه.</summary>
        private static void RestoreColor(Control control, System.Windows.Forms.Timer timer)
        {
            timer.Stop();
            timer.Dispose();

            if (RestoreTimers.TryGetValue(control, out System.Windows.Forms.Timer? current) &&
                ReferenceEquals(current, timer))
            {
                RestoreTimers.Remove(control);
            }

            if (!OriginalBackColors.TryGetValue(control, out Color original)) return;

            OriginalBackColors.Remove(control);

            if (!control.IsDisposed) control.BackColor = original;
        }

        /// <summary>يوقف أي وميض جارٍ على الحقل ويتلف مؤقّته قبل بدء وميض جديد (بلا تسريب مؤقّتات).</summary>
        private static void StopRestoreTimer(Control control)
        {
            if (!RestoreTimers.TryGetValue(control, out System.Windows.Forms.Timer? running)) return;

            running.Stop();
            running.Dispose();
            RestoreTimers.Remove(control);
        }

        /// <summary>يُنظّف مدخلات الحقول المُتلفة كي لا تتراكم التلميحات والمؤقّتات في الذاكرة.</summary>
        private static void PruneDisposedEntries()
        {
            List<Control> dead = new();

            foreach (Control key in SharedToolTips.Keys)
            {
                if (key.IsDisposed && !dead.Contains(key)) dead.Add(key);
            }

            foreach (Control key in RestoreTimers.Keys)
            {
                if (key.IsDisposed && !dead.Contains(key)) dead.Add(key);
            }

            foreach (Control key in OriginalBackColors.Keys)
            {
                if (key.IsDisposed && !dead.Contains(key)) dead.Add(key);
            }

            foreach (Control key in dead)
            {
                if (SharedToolTips.TryGetValue(key, out ToolTip? tip))
                {
                    tip.Dispose();
                    SharedToolTips.Remove(key);
                }

                if (RestoreTimers.TryGetValue(key, out System.Windows.Forms.Timer? timer))
                {
                    timer.Stop();
                    timer.Dispose();
                    RestoreTimers.Remove(key);
                }

                OriginalBackColors.Remove(key);
            }
        }
    }
}
