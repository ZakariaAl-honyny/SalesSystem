using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.Accounting;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    public partial class frmAddUpdateAccount : Form
    {
        private readonly IServiceProvider _serviceProvider;

        public delegate void DataBackEventHandler(object sender, int accountId);
        public event DataBackEventHandler? DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        private int _AccountId = -1;
        private AccountDto? _Account;

        public frmAddUpdateAccount(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            _Mode = enMode.AddNew;
        }

        private void _ResetDefaultValues()
        {
            // Nature
            cmbNature.Items.Clear();
            cmbNature.Items.AddRange(new object[] { "أصل", "خصم", "حق ملكية", "إيراد", "مصروف" });
            cmbNature.SelectedIndex = 0;

            // Level
            cmbLevel.Items.Clear();
            cmbLevel.Items.AddRange(new object[] { "المستوى 1", "المستوى 2", "المستوى 3", "المستوى 4" });
            cmbLevel.SelectedIndex = 0;

            // IsLeaf / AllowTransactions
            chkAllowTransactions.Checked = false;

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "إضافة حساب جديد";
                this.Text = "إضافة حساب جديد";
                txtAccountCode.Text = "(تلقائي)";
            }
            else
            {
                lblMode.Text = "تعديل الحساب";
                this.Text = "تعديل الحساب";
                btnSave.Text = "تعديل";
                txtAccountCode.ReadOnly = true;
                txtAccountCode.BackColor = Color.LightGray;
            }

            txtNameAr.Clear();
            txtNameEn.Clear();
            txtDescription.Clear();
            txtNotes.Clear();
            txtColorCode.Clear();
            chkIsSystem.Checked = false;
            chkAllowTransactions.Checked = false;
            lblAccountId.Text = "---";
        }

        private async void _LoadData()
        {
            var result = await _serviceProvider.GetRequiredService<IAccountApiService>().GetByIdAsync(_AccountId);

            if (!result.IsSuccess || result.Value == null)
            {
                MessageBox.Show($"لا يوجد حساب بهذا المعرف: {_AccountId}", "الحساب غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            _Account = result.Value;
            lblAccountId.Text = _AccountId.ToString();
            txtAccountCode.Text = _Account.AccountCode;
            txtNameAr.Text = _Account.NameAr;
            txtNameEn.Text = _Account.NameEn ?? "";
            txtDescription.Text = _Account.Description ?? "";
            txtNotes.Text = _Account.Notes ?? "";
            txtColorCode.Text = _Account.ColorCode ?? "";

            // Nature
            int natureIndex = Math.Clamp(_Account.Nature - 1, 0, 4);
            cmbNature.SelectedIndex = natureIndex;

            // Level
            int levelIndex = Math.Clamp(_Account.Level - 1, 0, 3);
            cmbLevel.SelectedIndex = levelIndex;

            chkIsSystem.Checked = _Account.IsSystem;
            chkAllowTransactions.Checked = _Account.IsLeaf;

            // Disable system account editing
            if (_Account.IsSystem && _Mode == enMode.Update)
            {
                btnSave.Enabled = false;
                MessageBox.Show("هذا الحساب نظامي ولا يمكن تعديله.", "حساب نظامي",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void frmAccountEditor_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }

        private bool _ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtNameAr.Text))
            {
                errorProvider.SetError(txtNameAr, "اسم الحساب بالعربية مطلوب.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtNameAr, "");
            }

            if (cmbNature.SelectedIndex < 0)
            {
                errorProvider.SetError(cmbNature, "يرجى اختيار طبيعة الحساب.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(cmbNature, "");
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
                if (_Mode == enMode.AddNew)
                {
                    var request = new CreateAccountRequest(
                        NameAr: txtNameAr.Text.Trim(),
                        NameEn: string.IsNullOrWhiteSpace(txtNameEn.Text) ? null : txtNameEn.Text.Trim(),
                        Nature: (byte)(cmbNature.SelectedIndex + 1),
                        IsLeaf: chkAllowTransactions.Checked,
                        ParentId: null,
                        IsSystem: false,
                        CategoryId: null,
                        Description: string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim(),
                        Notes: string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim(),
                        OpeningBalance: null
                    );

                    var result = await _serviceProvider.GetRequiredService<IAccountApiService>().CreateAsync(request);

                    if (result.IsSuccess)
                    {
                        _AccountId = result.Value;
                        MessageBox.Show("تم حفظ الحساب بنجاح.", "تم الحفظ",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataBack?.Invoke(this, _AccountId);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(result.Error ?? "لم يتم حفظ الحساب.", "خطأ",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var request = new UpdateAccountRequest(
                        NameAr: txtNameAr.Text.Trim(),
                        NameEn: string.IsNullOrWhiteSpace(txtNameEn.Text) ? null : txtNameEn.Text.Trim(),
                        Nature: (byte)(cmbNature.SelectedIndex + 1),
                        IsLeaf: chkAllowTransactions.Checked,
                        ParentId: null,
                        CategoryId: null,
                        Description: string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim(),
                        Notes: string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim()
                    );

                    var result = await _serviceProvider.GetRequiredService<IAccountApiService>().UpdateAsync(_AccountId, request);

                    if (result.IsSuccess)
                    {
                        MessageBox.Show("تم تعديل الحساب بنجاح.", "تم التعديل",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataBack?.Invoke(this, _AccountId);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(result.Error ?? "لم يتم تعديل الحساب.", "خطأ",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving account {AccountId}.", _AccountId);
                MessageBox.Show("حدث خطأ في حفظ الحساب. يرجى المحاولة مرة أخرى.", "خطأ",
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

        private void txtNameAr_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNameAr.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtNameAr, "اسم الحساب بالعربية مطلوب.");
            }
            else
            {
                errorProvider.SetError(txtNameAr, null);
            }
        }

        private void txtNameEn_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNameEn.Text) && txtNameEn.Text.Length > 150)
            {
                e.Cancel = true;
                errorProvider.SetError(txtNameEn, "الحد الأقصى 150 حرفاً.");
            }
            else
            {
                errorProvider.SetError(txtNameEn, null);
            }
        }

        private void txtColorCode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtColorCode.Text))
            {
                errorProvider.SetError(txtColorCode, null);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtColorCode.Text.Trim(), @"^#[0-9A-Fa-f]{6}$"))
            {
                e.Cancel = true;
                errorProvider.SetError(txtColorCode, "صيغة اللون غير صحيحة. مثال: #FF5722");
            }
            else
            {
                errorProvider.SetError(txtColorCode, null);
            }
        }

        /// <summary>
        /// Sets the editor to Update mode for the given account ID.
        /// Called after DI resolution (parameterless constructor).
        /// </summary>
        public void _SetUpdateMode(int accountId)
        {
            _AccountId = accountId;
            _Mode = enMode.Update;
        }
    }
}
