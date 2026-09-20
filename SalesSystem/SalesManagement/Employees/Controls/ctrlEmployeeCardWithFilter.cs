using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SalesManagement.Employees.Controls
{
    public partial class ctrlEmployeeCardWithFilter : UserControl
    {
        public event EventHandler<clsEmployee> OnEmployeeSelected;

        private int _SelectedEmployeeID = -1;
        private bool _ShowAddEmployee = true;
        private bool _FilterEnabled = true;

        private readonly ToolTip _rejectToolTip;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowAddEmployee
        {
            get => _ShowAddEmployee;
            set
            {
                _ShowAddEmployee = value;
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

        public int EmployeeID => ctrlEmployeeCard1.EmployeeID;
        public clsEmployee EmployeeInfo => ctrlEmployeeCard1.SelectedEmployeeInfo;

        public ctrlEmployeeCardWithFilter()
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

        public void ResetEmployeeInfo()
            => ctrlEmployeeCard1.ResetEmployeeInfo();

        public void LoadEmployeeInfo(int employeeID)
        {
            _SelectFilter("رقم الموظف");
            txtFilterValue.Text = employeeID.ToString();
            _FindNow();
        }

        public void LoadEmployeeInfo(string fullName)
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
                case "رقم الموظف":
                    _FindByEmployeeID();
                    break;

                case "الأسم":
                    ctrlEmployeeCard1.LoadEmployeeInfoByFullName(txtFilterValue.Text.Trim());
                    break;

                default:
                    _ShowWarning("يرجى اختيار طريقة بحث صحيحة.");
                    return;
            }

            if (_FilterEnabled)
                OnEmployeeSelected?.Invoke(this, EmployeeInfo);
        }

        private void _FindByEmployeeID()
        {
            if (!int.TryParse(txtFilterValue.Text.Trim(), out int employeeID) || employeeID <= 0)
            {
                _ShowWarning("يرجى إدخال رقم موظف صحيح.");
                return;
            }

            ctrlEmployeeCard1.LoadEmployeeInfoByEmployeeID(employeeID);
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void ctrlEmployeeCardWithFilter_Load(object sender, EventArgs e)
        {
            ResetEmployeeInfo();
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
                _ShowError("بعض الحقول المطلوبة فارغة، مرر الماوس على العلامة الحمراء لمعرفة الخطأ.");
                return;
            }

            _FindNow();
        }

        private void btnAddNewEmployee_Click(object sender, EventArgs e)
        {
            frmAddUpdateEmployee frm = new frmAddUpdateEmployee();
            frm.DataBack += _OnDataBack;
            clsGlobal._OpenForm(frm);
        }

        private void _OnDataBack(object sender, int employeeID)
        {
            _SelectedEmployeeID = employeeID;
            LoadEmployeeInfo(employeeID);
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

            bool isNumericFilter = cbFilterBy.Text == "رقم الموظف";

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