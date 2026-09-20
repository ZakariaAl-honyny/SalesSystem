using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.Settings;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Settings.Forms
{
    public partial class frmNotifications : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private List<NotificationDto> _allNotifications = new();
        private List<NotificationDto> _filteredNotifications = new();
        private ISessionService? _sessionService;

        public frmNotifications()
        {
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        public frmNotifications(IServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        private async void frmNotifications_Load(object sender, EventArgs e)
        {
            try
            {
                _sessionService = _serviceProvider.GetRequiredService<ISessionService>();
                _SetupGridColumns();
                await _RefreshNotifications();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "حدث خطأ غير متوقع أثناء تحميل النموذج");
                System.Windows.Forms.MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل النموذج. يرجى إعادة المحاولة.", "خطأ",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void _SetupGridColumns()
        {
            dgvNotifications.Columns.Clear();

            var columns = new[]
            {
                new { Name = "Id",        Header = "رقم",              Fill = 5,  Min = 60 },
                new { Name = "Title",     Header = "العنوان",          Fill = 20, Min = 150 },
                new { Name = "Message",   Header = "الرسالة",          Fill = 35, Min = 200 },
                new { Name = "IsRead",    Header = "مقروء",            Fill = 8,  Min = 80 },
                new { Name = "CreatedAt", Header = "تاريخ الإشعار",    Fill = 12, Min = 130 },
            };

            foreach (var col in columns)
            {
                dgvNotifications.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    DataPropertyName = col.Name,
                    MinimumWidth = col.Min,
                    FillWeight = col.Fill,
                    ReadOnly = true,
                });
            }
        }

        private async Task _RefreshNotifications()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var userId = _sessionService?.Current?.UserId ?? 0;
                var result = await _serviceProvider.GetRequiredService<INotificationApiService>().GetAllAsync(
                    userId: userId > 0 ? userId : null,
                    unreadOnly: chkUnreadOnly.Checked);

                if (result.IsSuccess && result.Value != null)
                {
                    _allNotifications = result.Value.OrderByDescending(n => n.CreatedAt).ToList();
                }
                else
                {
                    if (!result.IsSuccess)
                    {
                        MessageBox.Show(result.Error ?? "حدث خطأ في تحميل الإشعارات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    _allNotifications = new List<NotificationDto>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل الإشعارات: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _filteredNotifications = _allNotifications.ToList();
                _PopulateGrid();
                _UpdateRecordsCount();
                this.Cursor = Cursors.Default;
            }
        }

        private void _PopulateGrid()
        {
            dgvNotifications.AutoGenerateColumns = false;
            dgvNotifications.Rows.Clear();

            foreach (var n in _filteredNotifications)
            {
                dgvNotifications.Rows.Add(
                    n.Id,
                    n.Title,
                    n.Message,
                    n.IsRead ? "✓" : "✗",
                    clsFormat.DateToShort(n.CreatedAt)
                );
            }
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Text = _filteredNotifications.Count.ToString();
        }

        private int _GetSelectedNotificationId()
        {
            if (dgvNotifications.CurrentRow?.Cells[0].Value is int id)
                return id;
            return -1;
        }

        private NotificationDto? _GetSelectedNotification()
        {
            int id = _GetSelectedNotificationId();
            return id > 0 ? _allNotifications.FirstOrDefault(n => n.Id == id) : null;
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await _RefreshNotifications();
        }

        private async void btnMarkAsRead_Click(object sender, EventArgs e)
        {
            var notif = _GetSelectedNotification();
            if (notif == null)
            {
                MessageBox.Show("الرجاء اختيار إشعار من القائمة.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (notif.IsRead)
            {
                MessageBox.Show("هذا الإشعار مقروء بالفعل.", "معلومات",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<INotificationApiService>().MarkAsReadAsync(notif.Id);

                if (result.IsSuccess)
                {
                    await _RefreshNotifications();
                }
                else
                {
                    MessageBox.Show("فشل في تحديث الإشعار: " + (result.Error ?? ""), "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحديث الإشعار: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void btnMarkAllAsRead_Click(object sender, EventArgs e)
        {
            if (_allNotifications.All(n => n.IsRead))
            {
                MessageBox.Show("جميع الإشعارات مقروءة بالفعل.", "معلومات",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("هل أنت متأكد من تحديد جميع الإشعارات كمقروءة؟", "تأكيد",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                var userId = _sessionService?.Current?.UserId ?? 0;
                var result = await _serviceProvider.GetRequiredService<INotificationApiService>().MarkAllAsReadAsync(userId);

                if (result.IsSuccess)
                {
                    await _RefreshNotifications();
                }
                else
                {
                    MessageBox.Show("فشل في تحديث الإشعارات: " + (result.Error ?? ""), "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحديث الإشعارات: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void chkUnreadOnly_CheckedChanged(object sender, EventArgs e)
        {
            await _RefreshNotifications();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
