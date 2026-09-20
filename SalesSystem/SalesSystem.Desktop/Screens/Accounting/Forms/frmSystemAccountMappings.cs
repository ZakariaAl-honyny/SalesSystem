using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Screens.Accounting;
using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    public partial class frmSystemAccountMappings : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private List<SystemAccountMappingDto> _allMappings = new();

        public frmSystemAccountMappings(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvMappings.Columns.Clear();

            var columns = new[]
            {
                new { Name = "Id",              Header = "المعرف",         Fill = 5,  Min = 50  },
                new { Name = "MappingKeyName",  Header = "المفتاح",        Fill = 20, Min = 200 },
                new { Name = "AccountCode",     Header = "كود الحساب",     Fill = 10, Min = 100 },
                new { Name = "AccountName",     Header = "اسم الحساب",     Fill = 25, Min = 250 },
                new { Name = "BranchId",        Header = "الفرع",          Fill = 8,  Min = 80  },
            };

            foreach (var col in columns)
            {
                dgvMappings.Columns.Add(new DataGridViewTextBoxColumn
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

        // ── Data Loading ───────────────────────────────────────────────────────────

        private async void _RefreshMappings()
        {
            Cursor = dgvMappings.Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<ISystemAccountMappingApiService>().GetAllAsync();
                if (result.IsSuccess && result.Value != null)
                    _allMappings = result.Value.ToList();
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل البيانات", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _allMappings = new();
                }
                else
                    _allMappings = new();

                _PopulateGrid();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error loading system account mappings.");
                MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = dgvMappings.Cursor = Cursors.Default;
            }
        }

        private void _PopulateGrid()
        {
            dgvMappings.AutoGenerateColumns = false;
            dgvMappings.Rows.Clear();

            foreach (var m in _allMappings)
            {
                dgvMappings.Rows.Add(
                    m.Id,
                    m.MappingKeyName ?? m.MappingKey.ToString(),
                    m.AccountCode ?? "",
                    m.AccountName ?? "",
                    m.BranchId?.ToString() ?? ""
                );
            }
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshMappings();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedMappingId()
        {
            if (dgvMappings.CurrentRow == null) return -1;
            return (int)dgvMappings.CurrentRow.Cells[0].Value;
        }

        private SystemAccountMappingDto? _GetSelectedMapping()
        {
            int id = _GetSelectedMappingId();
            return _allMappings.FirstOrDefault(m => m.Id == id);
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmSystemAccountMappings_Load(object sender, EventArgs e)
        {
            _ReloadForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
            => Close();

        // ── Grid Events ────────────────────────────────────────────────────────────

        private void dgvMappings_DoubleClick(object sender, EventArgs e)
        {
            var mapping = _GetSelectedMapping();
            if (mapping == null) return;

            // Edit the selected mapping
            _EditMapping(mapping);
        }

        // ── Edit Mapping ───────────────────────────────────────────────────────────

        private async void _EditMapping(SystemAccountMappingDto mapping)
        {
            string newAccountCode = Microsoft.VisualBasic.Interaction.InputBox(
                $"أدخل كود الحساب الجديد للربط:\n\nالمفتاح: {mapping.MappingKeyName ?? mapping.MappingKey.ToString()}\nالحساب الحالي: {mapping.AccountCode} - {mapping.AccountName}",
                "تعديل ربط الحساب النظامي",
                mapping.AccountCode ?? "",
                -1, -1);

            if (string.IsNullOrWhiteSpace(newAccountCode))
                return;

            // We need an AccountId to map — for simplicity, prompt for it too
            string accountIdStr = Microsoft.VisualBasic.Interaction.InputBox(
                "أدخل معرف الحساب (Account ID):",
                "معرف الحساب",
                "",
                -1, -1);

            if (!int.TryParse(accountIdStr, out int accountId) || accountId <= 0)
            {
                MessageBox.Show("معرف الحساب غير صالح.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<ISystemAccountMappingApiService>().UpdateAsync(
                    mapping.Id,
                    new UpdateSystemAccountMappingRequest(AccountId: accountId)
                );

                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تحديث ربط الحساب النظامي بنجاح.", "تم التحديث",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ReloadForm();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل تحديث الربط.", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating system account mapping {Id}.", mapping.Id);
                MessageBox.Show("حدث خطأ في تحديث الربط. يرجى المحاولة مرة أخرى.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
