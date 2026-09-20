using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.Accounting;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Models.Accounting.Enums;
using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    public partial class frmAddUpdateJournalEntry : Form
    {
        private readonly IServiceProvider _serviceProvider;

        public delegate void DataBackEventHandler(object sender, int entryId);
        public event DataBackEventHandler? DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        private int _EntryId = -1;
        private JournalEntryDetailDto? _Entry;

        // In-memory lines collection
        private List<JournalEntryLineDetailDto> _lines = new();

        // Next temporary line ID for new lines (negative values = unsaved)
        private int _nextLineTempId = -1;

        public frmAddUpdateJournalEntry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            _Mode = enMode.AddNew;
        }

        private void _ResetDefaultValues()
        {
            // Entry type
            cmbEntryType.Items.Clear();
            cmbEntryType.Items.AddRange(new object[]
            {
                "يدوي",        // Manual = 0
                "مبيعات",      // Sales = 1
                "مشتريات",     // Purchases = 2
                "مقبوضات",     // Receipts = 3
                "مدفوعات",     // Payments = 4
                "الافتتاحي",   // Opening = 5
                "الإغلاق",     // Closing = 6
                "تسوية",       // Adjustment = 7
            });
            cmbEntryType.SelectedIndex = 0;

            dtpEntryDate.Value = DateTime.Today;

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "إضافة قيد يومي جديد";
                this.Text = "إضافة قيد يومي جديد";
                _lines.Clear();
                _nextLineTempId = -1;
                _RefreshLinesGrid();
                UpdateTotals();
            }
            else
            {
                lblMode.Text = "تعديل القيد اليومي";
                this.Text = "تعديل القيد اليومي";
                btnSave.Text = "تعديل";
            }
        }

        private async void _LoadData()
        {
            var result = await _serviceProvider.GetRequiredService<IJournalEntryApiService>().GetByIdAsync(_EntryId);

            if (!result.IsSuccess || result.Value == null)
            {
                MessageBox.Show($"لا يوجد قيد بهذا المعرف: {_EntryId}", "القيد غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            _Entry = result.Value;
            lblEntryId.Text = _EntryId.ToString();
            txtEntryNumber.Text = _Entry.EntryNumber;
            dtpEntryDate.Value = _Entry.EntryDate;
            txtDescription.Text = _Entry.Description;
            txtReferenceNumber.Text = _Entry.ReferenceNumber ?? "";

            // Set entry type combo
            int typeIndex = _Entry.EntryType switch
            {
                "يدوي" => 0,
                "مبيعات" => 1,
                "مشتريات" => 2,
                "مقبوضات" => 3,
                "مدفوعات" => 4,
                "الافتتاحي" => 5,
                "الإغلاق" => 6,
                "تسوية" => 7,
                _ => 0
            };
            cmbEntryType.SelectedIndex = typeIndex >= 0 ? typeIndex : 0;

            // Load lines
            _lines = _Entry.Lines?.ToList() ?? new();
            _RefreshLinesGrid();
            UpdateTotals();

            // Disable editing for posted/cancelled entries
            bool canEdit = _Entry.Status == 1; // Draft
            txtDescription.ReadOnly = !canEdit;
            txtDescription.BackColor = canEdit ? SystemColors.Window : Color.LightGray;
            btnAddLine.Enabled = canEdit;
            btnRemoveLine.Enabled = canEdit;
            btnSave.Enabled = canEdit;
        }

        private void frmJournalEntryEditor_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }

        // ── Lines Grid ─────────────────────────────────────────────────────────────

        private void _SetupLinesGrid()
        {
            dgvLines.Columns.Clear();

            var columns = new[]
            {
                new { Name = "TempId",       Header = "المعرف",      Fill = 0, Min = 0  },  // Hidden
                new { Name = "AccountName",  Header = "الحساب",      Fill = 25, Min = 200 },
                new { Name = "Description",  Header = "البيان",      Fill = 20, Min = 150 },
                new { Name = "Debit",        Header = "مدين",        Fill = 12, Min = 100 },
                new { Name = "Credit",       Header = "دائن",        Fill = 12, Min = 100 },
            };

            foreach (var col in columns)
            {
                dgvLines.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    DataPropertyName = col.Name,
                    MinimumWidth = col.Min,
                    FillWeight = col.Fill,
                    ReadOnly = true,
                });
            }

            // Hide TempId column
            if (dgvLines.Columns["TempId"] != null)
                dgvLines.Columns["TempId"]!.Visible = false;
        }

        private void _RefreshLinesGrid()
        {
            dgvLines.AutoGenerateColumns = false;
            dgvLines.Rows.Clear();

            foreach (var line in _lines)
            {
                dgvLines.Rows.Add(
                    line.Id,
                    line.AccountName ?? "",
                    line.Description ?? "",
                    line.Debit > 0 ? line.Debit.ToString("N2") : "",
                    line.Credit > 0 ? line.Credit.ToString("N2") : ""
                );
            }
        }

        private void UpdateTotals()
        {
            decimal totalDebit = _lines.Sum(l => l.Debit);
            decimal totalCredit = _lines.Sum(l => l.Credit);
            lblTotalDebit.Text = totalDebit.ToString("N2");
            lblTotalCredit.Text = totalCredit.ToString("N2");

            bool balanced = Math.Abs(totalDebit - totalCredit) < 0.01m;
            lblBalanceStatus.Text = balanced ? "✓ متوازن" : "✗ غير متوازن";
            lblBalanceStatus.ForeColor = balanced ? Color.Green : Color.Red;
        }

        // ── Add/Remove Lines ───────────────────────────────────────────────────────

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            string accountName = Microsoft.VisualBasic.Interaction.InputBox(
                "أدخل اسم الحساب:", "إضافة بند", "", -1, -1);
            if (string.IsNullOrWhiteSpace(accountName)) return;

            string debitStr = Microsoft.VisualBasic.Interaction.InputBox(
                "أدخل مبلغ المدين (0 إذا لا يوجد):", "المدين", "0", -1, -1);
            if (!decimal.TryParse(debitStr, out var debit)) debit = 0;

            string creditStr = Microsoft.VisualBasic.Interaction.InputBox(
                "أدخل مبلغ الدائن (0 إذا لا يوجد):", "الدائن", "0", -1, -1);
            if (!decimal.TryParse(creditStr, out var credit)) credit = 0;

            if (debit <= 0 && credit <= 0)
            {
                MessageBox.Show("يجب إدخال مبلغ المدين أو الدائن.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string desc = Microsoft.VisualBasic.Interaction.InputBox(
                "أدخل بيان البند (اختياري):", "البيان", "", -1, -1);

            _lines.Add(new JournalEntryLineDetailDto(
                Id: _nextLineTempId--,
                AccountId: 0,
                AccountName: accountName,
                AccountCode: null,
                Debit: debit,
                Credit: credit,
                Description: string.IsNullOrWhiteSpace(desc) ? null : desc
            ));

            _RefreshLinesGrid();
            UpdateTotals();
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (dgvLines.CurrentRow == null) return;

            int tempId = (int)dgvLines.CurrentRow.Cells["TempId"].Value;
            var line = _lines.FirstOrDefault(l => l.Id == tempId);
            if (line != null)
            {
                _lines.Remove(line);
                _RefreshLinesGrid();
                UpdateTotals();
            }
        }

        // ── Save ───────────────────────────────────────────────────────────────────

        private bool _ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                errorProvider.SetError(txtDescription, "بيان القيد مطلوب.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtDescription, "");
            }

            if (_lines.Count == 0)
            {
                errorProvider.SetError(dgvLines, "يجب إضافة بند واحد على الأقل للقيد.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(dgvLines, "");
            }

            decimal totalDebit = _lines.Sum(l => l.Debit);
            decimal totalCredit = _lines.Sum(l => l.Credit);
            if (_lines.Count > 0 && Math.Abs(totalDebit - totalCredit) > 0.01m)
            {
                errorProvider.SetError(lblBalanceStatus, "مجموع المدين لا يساوي مجموع الدائن. يجب أن يكون القيد متوازناً.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(lblBalanceStatus, "");
            }

            if (!isValid)
            {
                MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return isValid;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!_ValidateForm()) return;

            Cursor = Cursors.WaitCursor;
            try
            {
                var request = new CreateJournalEntryRequest(
                    EntryDate: dtpEntryDate.Value,
                    Description: txtDescription.Text.Trim(),
                    EntryType: (JournalEntryType)cmbEntryType.SelectedIndex,
                    ReferenceType: null,
                    ReferenceId: null,
                    ReferenceNumber: string.IsNullOrWhiteSpace(txtReferenceNumber.Text) ? null : txtReferenceNumber.Text.Trim(),
                    Lines: _lines.Select(l => new JournalEntryLineRequest(
                        AccountId: 0,
                        Debit: l.Debit,
                        Credit: l.Credit,
                        Description: l.Description
                    )).ToList()
                );

                var result = await _serviceProvider.GetRequiredService<IJournalEntryApiService>().CreateAsync(request);

                if (result.IsSuccess)
                {
                    _EntryId = result.Value;
                    MessageBox.Show("تم حفظ القيد بنجاح.", "تم الحفظ",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DataBack?.Invoke(this, _EntryId);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "لم يتم حفظ القيد.", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving journal entry {EntryId}.", _EntryId);
                MessageBox.Show("حدث خطأ في حفظ القيد. يرجى المحاولة مرة أخرى.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ── Validation ─────────────────────────────────────────────────────────────

        private void txtDescription_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtDescription, "البيان مطلوب.");
            }
            else
            {
                errorProvider.SetError(txtDescription, null);
            }
        }

        /// <summary>
        /// Sets the editor to Update mode for the given entry ID.
        /// Called after DI resolution (parameterless constructor).
        /// </summary>
        public void _SetUpdateMode(int entryId)
        {
            _EntryId = entryId;
            _Mode = enMode.Update;
        }
    }
}
