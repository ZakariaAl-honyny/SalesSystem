using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Settings.Forms
{
    public partial class frmUpdateCheck : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private CancellationTokenSource? _downloadCts;

        public frmUpdateCheck()
        {
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        public frmUpdateCheck(IServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        private void frmUpdateCheck_Load(object sender, EventArgs e)
        {
            _LoadCurrentVersion();
        }

        private void _LoadCurrentVersion()
        {
            try
            {
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                lblCurrentVersionValue.Text = version?.ToString() ?? "1.0.0.0";
            }
            catch
            {
                lblCurrentVersionValue.Text = "1.0.0.0";
            }
        }

        private async void btnCheckForUpdates_Click(object sender, EventArgs e)
        {
            await _CheckForUpdatesAsync();
        }

        private async Task _CheckForUpdatesAsync()
        {
            btnCheckForUpdates.Enabled = false;
            progressBar.Visible = true;
            progressBar.Value = 0;
            lblProgressText.Text = "جارٍ التحقق من التحديثات...";
            lblStatusValue.Text = "جارٍ الفحص...";
            lblStatusValue.ForeColor = System.Drawing.Color.FromArgb(243, 156, 18);

            try
            {
                // NOTE: the HTTP layer was removed from this application (2-Tier edition).
                // Update checking over the network is no longer available, so this is a
                // no-network stub that simply reports "no update available".
                await Task.CompletedTask;
                _SetNoUpdateStatus();
            }
            catch
            {
                _SetNoUpdateStatus();
            }
            finally
            {
                btnCheckForUpdates.Enabled = true;
                progressBar.Visible = false;
                lblProgressText.Text = string.Empty;
            }
        }

        private void _SetNoUpdateStatus()
        {
            lblStatusValue.Text = "لا توجد تحديثات متوفرة";
            lblStatusValue.ForeColor = System.Drawing.Color.Gray;
            lblLatestVersionValue.Text = lblCurrentVersionValue.Text;
            btnCheckForUpdates.Text = "التحقق من التحديثات";
            txtChangelog.Text = "لا توجد تحديثات متوفرة. لديك أحدث إصدار.";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _downloadCts?.Cancel();
            Close();
        }
    }
}
