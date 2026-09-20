using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SalesManagement.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public event EventHandler<clsPerson> OnPersonSelected;

        private int _SelectedPersonID = -1;
        private bool _ShowAddPerson = true;
        private bool _FilterEnabled = true;

        private readonly ToolTip _rejectToolTip;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowAddPerson
        {
            get => _ShowAddPerson;
            set
            {
                _ShowAddPerson = value;
                btnAddNewPerson.Visible = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FilterEnabled
        {
            get => _FilterEnabled;
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = value;
            }
        }

        public int PersonID => ctrlPersonCard1.PersonID;
        public clsPerson PersonInfo => ctrlPersonCard1.SelectedPersonInfo;

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();

            _rejectToolTip = new ToolTip
            {
                IsBalloon = true,
                ToolTipIcon = ToolTipIcon.Warning,
                ToolTipTitle = "تنبيه",
                UseAnimation = true,
                UseFading = true,
            };
        }

        // ── Public API ─────────────────────────────────────────────────────────────

        public void ResetPersonInfo()
            => ctrlPersonCard1.ResetPersonInfo();

        public void LoadPersonInfo(int? personID)
        {
            _SelectFilter("رقم الشخص");
            txtFilterValue.Text = personID.ToString();
            _FindNow();
        }

        public void LoadPersonInfo(string fullName)
        {
            _SelectFilter("الأسم");
            txtFilterValue.Text = fullName;
            _FindNow();
        }

        public void FilterFocus()
        {
            txtFilterValue.Focus();
            txtFilterValue.SelectAll();
        }

        // ── Private Helpers ────────────────────────────────────────────────────────

        private void _SelectFilter(string filterName)
        {
            int index = cbFilterBy.FindString(filterName);

            if (index >= 0)
                cbFilterBy.SelectedIndex = index;
        }

        private void _FindNow()
        {
            switch (cbFilterBy.Text)
            {
                case "رقم الشخص":
                    _FindByPersonID();
                    break;

                case "الأسم":
                    ctrlPersonCard1.LoadPersonInfoByFullName(txtFilterValue.Text.Trim());
                    break;

                case "الرقم الوطني":
                    ctrlPersonCard1.LoadPersonInfoByNationalNo(txtFilterValue.Text.Trim());
                    break;

                default:
                    _ShowWarning("يرجى اختيار طريقة بحث صحيحة.");
                    return;
            }

            if (_FilterEnabled)
                OnPersonSelected?.Invoke(this, PersonInfo);
        }

        private void _FindByPersonID()
        {
            if (!int.TryParse(txtFilterValue.Text.Trim(), out int personID) || personID <= 0)
            {
                _ShowWarning("يرجى إدخال رقم شخص صحيح.");
                return;
            }

            ctrlPersonCard1.LoadPersonInfo(personID);
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            ResetPersonInfo();
            _SelectFilter("الأسم");
        }

        // ── Control Events ─────────────────────────────────────────────────────────

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                _ShowError("بعض الحقول غير صالحة، مرر الماوس على الأيقونة الحمراء لمعرفة الخطأ.");
                return;
            }

            _FindNow();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.DataBack += _OnDataBack;
            clsGlobal._OpenForm(frm);
        }

        private void _OnDataBack(object sender, int personID)
        {
            _SelectedPersonID = personID;
            LoadPersonInfo(personID);
        }

        // ── Input Validation + Feedback ────────────────────────────────────────────

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnFind.PerformClick();
                e.Handled = true;
                return;
            }

            bool isNumericFilter = cbFilterBy.Text == "رقم الشخص";

            if (isNumericFilter && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                clsInputFeedback.ShowRejectTooltip(
                    txtFilterValue,
                    "يُسمح فقط بإدخال الأرقام في هذا الحقل.",
                    _rejectToolTip);
                return;
            }

            if (!isNumericFilter && char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                clsInputFeedback.ShowRejectTooltip(
                    txtFilterValue,
                    "لا يُسمح بإدخال علامات الترقيم.",
                    _rejectToolTip);
            }
        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterValue.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterValue, "هذا الحقل مطلوب!");
            }
            else
            {
                errorProvider1.SetError(txtFilterValue, null);
            }
        }

        // ── Message Helpers ────────────────────────────────────────────────────────

        private void _ShowError(string message)
            => MessageBox.Show(message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void _ShowWarning(string message)
            => MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}