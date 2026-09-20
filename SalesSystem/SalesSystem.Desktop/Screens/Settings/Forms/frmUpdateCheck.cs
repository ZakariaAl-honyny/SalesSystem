using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services;
using SalesSystem.DTO.Common;
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
                // Attempt to check updates via API
                var httpClient = _serviceProvider.GetRequiredService<HttpClientService>();
                var result = await httpClient.GetAsync<UpdateCheckResponse>("api/v1/updates/check");
                if (result.IsSuccess && result.Value != null)
                {
                    var update = result.Value;
                    lblLatestVersionValue.Text = update.LatestVersion;
                    txtChangelog.Text = string.Join(Environment.NewLine, update.Changelog ?? new List<string>());

                    if (update.HasUpdate)
                    {
                            lblStatusValue.Text = "تحديث متوفر!";
                            lblStatusValue.ForeColor = System.Drawing.Color.FromArgb(46, 204, 113);
                            btnCheckForUpdates.Text = "تحميل التحديث";
                        _downloadCts = new CancellationTokenSource();
                    }
                    else
                    {
                            lblStatusValue.Text = "لديك أحدث إصدار";
                            lblStatusValue.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
                            btnCheckForUpdates.Text = "التحقق من التحديثات";
                    }
                }
                else
                {
                    _SetNoUpdateStatus();
                }
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
            lblStatusValue.Text = "لا توجد تحديثات متاحة";
            lblStatusValue.ForeColor = System.Drawing.Color.Gray;
            lblLatestVersionValue.Text = lblCurrentVersionValue.Text;
            txtChangelog.Text = "تعذر الاتصال بخادم التحديثات. يرجى المحاولة لاحقاً.";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _downloadCts?.Cancel();
            Close();
        }

        /// <summary>
        /// Response model for the update check API endpoint.
        /// </summary>
        private class UpdateCheckResponse
        {
            public string LatestVersion { get; set; } = string.Empty;
            public bool HasUpdate { get; set; }
            public List<string>? Changelog { get; set; }
        }
    }
}
