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
    public partial class frmAddUpdateAccountCategory : Form
    {
        private readonly IServiceProvider _serviceProvider;

        public delegate void DataBackEventHandler(object sender, int categoryId);
        public event DataBackEventHandler? DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        private int _CategoryId = -1;
        private AccountCategoryDto? _Category;

        public frmAddUpdateAccountCategory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            _Mode = enMode.AddNew;
        }

        private void _ResetDefaultValues()
        {
            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "إضافة تصنيف حسابات جديد";
                this.Text = "إضافة تصنيف حسابات جديد";
            }
            else
            {
                lblMode.Text = "تعديل تصنيف الحسابات";
                this.Text = "تعديل تصنيف الحسابات";
                btnSave.Text = "تعديل";
            }

            txtName.Clear();
            txtDescription.Clear();
            lblCategoryId.Text = "---";
        }

        private async void _LoadData()
        {
            var result = await _serviceProvider.GetRequiredService<IAccountCategoryApiService>().GetByIdAsync(_CategoryId);

            if (!result.IsSuccess || result.Value == null)
            {
                MessageBox.Show($"لا يوجد تصنيف بهذا المعرف: {_CategoryId}", "التصنيف غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            _Category = result.Value;
            lblCategoryId.Text = _CategoryId.ToString();
            txtName.Text = _Category.Name;
            txtDescription.Text = _Category.Description ?? "";
        }

        private void frmAccountCategoryEditor_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }

        private bool _ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorProvider.SetError(txtName, "اسم التصنيف مطلوب.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtName, "");
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
                    var request = new CreateAccountCategoryRequest(
                        Name: txtName.Text.Trim(),
                        Description: string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
                    );

                    var result = await _serviceProvider.GetRequiredService<IAccountCategoryApiService>().CreateAsync(request);

                    if (result.IsSuccess && result.Value != null)
                    {
                        _Category = result.Value;
                        _CategoryId = result.Value.Id;
                        MessageBox.Show("تم حفظ التصنيف بنجاح.", "تم الحفظ",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataBack?.Invoke(this, _CategoryId);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(result.Error ?? "لم يتم حفظ التصنيف.", "خطأ",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var request = new UpdateAccountCategoryRequest(
                        Name: txtName.Text.Trim(),
                        Description: string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
                    );

                    var result = await _serviceProvider.GetRequiredService<IAccountCategoryApiService>().UpdateAsync(_CategoryId, request);

                    if (result.IsSuccess && result.Value != null)
                    {
                        MessageBox.Show("تم تعديل التصنيف بنجاح.", "تم التعديل",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataBack?.Invoke(this, _CategoryId);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(result.Error ?? "لم يتم تعديل التصنيف.", "خطأ",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving account category {CategoryId}.", _CategoryId);
                MessageBox.Show("حدث خطأ في حفظ التصنيف. يرجى المحاولة مرة أخرى.", "خطأ",
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

        private void txtName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtName, "اسم التصنيف مطلوب.");
            }
            else
            {
                errorProvider.SetError(txtName, null);
            }
        }

        /// <summary>
        /// Sets the editor to Update mode for the given category ID.
        /// Called after DI resolution (parameterless constructor).
        /// </summary>
        public void _SetUpdateMode(int categoryId)
        {
            _CategoryId = categoryId;
            _Mode = enMode.Update;
        }
    }
}
