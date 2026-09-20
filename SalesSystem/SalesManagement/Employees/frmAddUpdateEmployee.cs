using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SalesManagement.Employees
{
    public partial class frmAddUpdateEmployee : Form
    {
        public delegate void DataBackEventHandler(object sender, int employeeID);
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode;
        private int _EmployeeID = -1;
        private clsEmployee _Employee;

        private ToolTip _rejectToolTip;

        // Maps Arabic ComboBox text → enum values
        private static readonly Dictionary<string, clsEmployee.enEmployeeQualifications> _qualificationMap =
            new Dictionary<string, clsEmployee.enEmployeeQualifications>
            {
                { "لا يوجد",      clsEmployee.enEmployeeQualifications.eNothing                },
                { "بكالوريوس",    clsEmployee.enEmployeeQualifications.eBachelors               },
                { "دبلوم",        clsEmployee.enEmployeeQualifications.eDiploma                 },
                { "ثانوية عامة",  clsEmployee.enEmployeeQualifications.eHighSchoolCertificate   },
                { "تاسع أساسي",  clsEmployee.enEmployeeQualifications.eIntermediateCertificate },
            };

        private static readonly Dictionary<string, clsEmployee.enEmployeeStatus> _statusMap =
            new Dictionary<string, clsEmployee.enEmployeeStatus>
            {
                { "أعزب",  clsEmployee.enEmployeeStatus.eSingle   },
                { "متزوج", clsEmployee.enEmployeeStatus.eMarried  },
                { "أرمل",  clsEmployee.enEmployeeStatus.eWidow    },
                { "مطلق",  clsEmployee.enEmployeeStatus.eDivorced },
            };

        public frmAddUpdateEmployee()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            _InitToolTip();
        }

        public frmAddUpdateEmployee(int employeeID)
        {
            InitializeComponent();
            _EmployeeID = employeeID;
            _Mode = enMode.Update;
            _InitToolTip();
        }

        private void _InitToolTip()
        {
            _rejectToolTip = new ToolTip
            {
                IsBalloon = true,
                ToolTipIcon = ToolTipIcon.Warning,
                ToolTipTitle = "تنبيه",
                UseAnimation = true,
                UseFading = true,
            };
        }

        // ── Private Methods ────────────────────────────────────────────────────────

        private void _ResetDefaultValues()
        {
            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = this.Text = "إضافة موظف جديد";
                _Employee = new clsEmployee();
                tpEmployeeInfo.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = this.Text = "تعديل بيانات الموظف";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";
                tpEmployeeInfo.Enabled = true;
                btnSave.Enabled = true;
                btnSaveAndClose.Enabled = true;
            }

            lblEmployeeID.Text = "N/A";
            cbxEmployeeQualification.SelectedIndex = cbxEmployeeQualification.FindString("لا يوجد");
            cbxEmployeeState.SelectedIndex = cbxEmployeeState.FindString("أعزب");
        }

        private void _LoadData()
        {
            _Employee = clsEmployee.FindByEmployeeID(_EmployeeID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_Employee == null)
            {
                MessageBox.Show(
                    $"لا يوجد موظف بهذا الرقم: {_EmployeeID}\nسيتم إغلاق الشاشة.",
                    "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            lblEmployeeID.Text = _Employee.EmployeeID.ToString();

            cbxEmployeeQualification.SelectedIndex = cbxEmployeeQualification.FindString(
                _GetKeyByValue(_qualificationMap, _Employee.EmployeeQualification));

            cbxEmployeeState.SelectedIndex = cbxEmployeeState.FindString(
                _GetKeyByValue(_statusMap, _Employee.EmployeeState));

            ctrlPersonCardWithFilter1.LoadPersonInfo(_Employee.PersonID);
        }

        private void _SwitchToUpdateMode()
        {
            _Mode = enMode.Update;
            lblTitle.Text = this.Text = "تعديل بيانات الموظف";
            btnSave.Text = "تعديل";
            btnSaveAndClose.Text = "تعديل وإغلاق";
            lblEmployeeID.Text = _Employee.EmployeeID.ToString();
            ctrlPersonCardWithFilter1.FilterEnabled = false;
        }

        private void _EnableEmployeeTab()
        {
            btnSave.Enabled = true;
            btnSaveAndClose.Enabled = true;
            tpEmployeeInfo.Enabled = true;
            tcEmployeeInfo.SelectedTab = tcEmployeeInfo.TabPages["tpEmployeeInfo"];
        }

        private static string _GetKeyByValue<TKey, TValue>(Dictionary<TKey, TValue> dict, TValue value)
        {
            foreach (var pair in dict)
            {
                if (pair.Value.Equals(value))
                    return pair.Key.ToString();
            }
            return "";
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmAddUpdateEmployee_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void frmAddUpdateEmployee_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }

        private void frmAddUpdateEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }

        // ── Button Events ──────────────────────────────────────────────────────────

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show(
                    "بعض الحقول المطلوبة فارغة، مرر الماوس على العلامة الحمراء لمعرفة الخطأ.",
                    "خطأ - البيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Employee.PersonID = ctrlPersonCardWithFilter1.PersonID;

            _Employee.EmployeeQualification = _qualificationMap.TryGetValue(
                cbxEmployeeQualification.Text, out var qual)
                ? qual
                : clsEmployee.enEmployeeQualifications.eNothing;

            _Employee.EmployeeState = _statusMap.TryGetValue(
                cbxEmployeeState.Text, out var status)
                ? status
                : clsEmployee.enEmployeeStatus.eSingle;

            if (_Employee.Save())
            {
                _SwitchToUpdateMode();
                MessageBox.Show("تم حفظ البيانات بنجاح.", "تم الحفظ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBack?.Invoke(this, _Employee.EmployeeID);
            }
            else
            {
                MessageBox.Show("لم يتم حفظ البيانات.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            btnSave_Click(null!, null!);
            btnClose_Click(null!, null!);
        }

        private void btnClose_Click(object sender, EventArgs e)
            => Close();

        private void btnNextEmployeeInfo_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                _EnableEmployeeTab();
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("يرجى اختيار شخص أولاً.", "لم يتم الاختيار",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }

            if (clsEmployee.isEmployeeExistByPersonID(ctrlPersonCardWithFilter1.PersonID))
            {
                MessageBox.Show("هذا الشخص لديه حساب موظف مسبقاً، يرجى اختيار شخص آخر.",
                    "موظف موجود مسبقاً", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }

            _EnableEmployeeTab();
            cbxEmployeeQualification.Focus();
        }

        private void btnPrevPersonInfo_Click(object sender, EventArgs e)
        {
            tcEmployeeInfo.SelectedTab = tcEmployeeInfo.TabPages["tpPersonInfo"];
        }

        // ── Control Events ─────────────────────────────────────────────────────────

        private void ctrlPersonCardWithFilter1_OnPersonSelected(object sender, clsPerson e)
        {
            bool personLoaded = ctrlPersonCardWithFilter1.PersonID != -1;

            btnSave.Enabled = personLoaded;
            btnSaveAndClose.Enabled = personLoaded;
            tpEmployeeInfo.Enabled = personLoaded;
        }

        // ── Validation ─────────────────────────────────────────────────────────────

        private void _ValidateEmptyComboBox(object sender, CancelEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (string.IsNullOrWhiteSpace(comboBox.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(comboBox, "هذا الحقل مطلوب.");
            }
            else
            {
                errorProvider1.SetError(comboBox, null);
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;

                clsInputFeedback.ShowRejectTooltip(
                    (Control)sender,
                    "لا يُسمح بإدخال علامات الترقيم.",
                    _rejectToolTip);
            }
        }
    }
}