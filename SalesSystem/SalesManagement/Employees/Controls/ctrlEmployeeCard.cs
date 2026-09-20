//using SalesManagement.Busniess;
//using SalesManagement.GlobalClasses;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace SalesManagement.Employees.Controls
//{
//    public partial class ctrlEmployeeCard : UserControl
//    {

//        clsEmployee _Employee;
//        int _EmployeeId = -1;

//        public int EmployeeID
//        {
//            get { return _EmployeeId; }
//        }
//        public clsEmployee SelectedEmployeeInfo
//        {
//            get { return _Employee; }
//        }

//        public ctrlEmployeeCard()
//        {
//            InitializeComponent();
//        }

//        public void ResetEmployeeInfo()
//        {
//            _EmployeeId = -1;
//            ctrlPersonCard1.ResetPersonInfo();
//            txtEmployeeID.Text = "N/A";
//            txtEmployeeQualification.Text = "[????]";
//            txtEmployeeState.Text = "[????]";
//        }

//        private void _FillEmployeeInfo()
//        {

//            ctrlPersonCard1.LoadPersonInfo(_Employee.PersonID);

//            txtEmployeeID.Text = _Employee.EmployeeID.ToString();

//            switch (_Employee.EmployeeQualification)
//            {
//                case clsEmployee.enEmployeeQualifications.eNothing:
//                    txtEmployeeQualification.Text = "لا يوجد";
//                    break;
//                case clsEmployee.enEmployeeQualifications.eBachelors:
//                    txtEmployeeQualification.Text = "بكالوريوس";
//                    break;
//                case clsEmployee.enEmployeeQualifications.eDiploma:
//                    txtEmployeeQualification.Text = "دبلوم";
//                    break;
//                case  clsEmployee.enEmployeeQualifications.eHighSchoolCertificate:
//                    txtEmployeeQualification.Text = "ثانوية عامة";
//                    break;
//                case  clsEmployee.enEmployeeQualifications.eIntermediateCertificate:
//                    txtEmployeeQualification.Text = "تاسع أساسي";
//                    break;
//                default:
//                    txtEmployeeQualification.Text = "غير معروف";
//                    break;
//            }

//            switch (_Employee.EmployeeState)
//            {
//                case clsEmployee.enEmployeeStatus.eSingle:
//                    txtEmployeeState.Text = "أعزب";
//                    break;
//                case clsEmployee.enEmployeeStatus.eMarried:
//                    txtEmployeeState.Text = "متزوج";
//                    break;
//                case clsEmployee.enEmployeeStatus.eWidow:
//                    txtEmployeeState.Text = "أرمل";
//                    break;
//                case clsEmployee.enEmployeeStatus.eDivorced:
//                    txtEmployeeState.Text = "مطلق";
//                    break;
//                default:
//                    txtEmployeeState.Text = "غير معروف";
//                    break;
//            }

//        }

//        public void LoadEmployeeInfoByEmployeeID(int EmployeeID)
//        {
//            _EmployeeId = EmployeeID;
//            _Employee = clsEmployee.FindByEmployeeID(EmployeeID);

//            if (_Employee == null)
//            {
//                ResetEmployeeInfo();
//                MessageBox.Show($"لا يوجد موظف يملك رقم = {EmployeeID} ", "غير موجود",
//                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
//                return;
//            }

//            _FillEmployeeInfo();

//        }

//        public void LoadEmployeeInfoByFullName(string FullName)
//        {
//            clsPerson clsPerson = clsPerson.FindByFullName(FullName);

//            if (clsPerson == null)
//            {
//                ResetEmployeeInfo();
//                MessageBox.Show($"لا يوجد شخص بهذا لأسم = {FullName} ", "غير موجود",
//                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
//                return;
//            }

//            _Employee = clsEmployee.FindByPersonID(clsPerson.PersonID);

//            if (_Employee == null)
//            {
//                ResetEmployeeInfo();
//                MessageBox.Show($"لا يوجد موظف بهذا لأسم = {FullName} ", "غير موجود",
//                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
//                return;
//            }

//            _EmployeeId = _Employee!.EmployeeID;
//            _FillEmployeeInfo();

//        }


//        private void ctrlEmployeeCard_Load(object sender, EventArgs e)
//        {
//            ResetEmployeeInfo();
//        }

//        private void llEditEmployeeInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
//        {
//            frmAddUpdateEmployee frm1 = new frmAddUpdateEmployee(_EmployeeId);

//            frm1.DataBack += DataBackEvent;

//            clsGlobal._OpenForm(frm1);

//            //frm1.ShowDialog();
//        }


//        private void DataBackEvent(object sender, int EmployeeID)
//        {
//            LoadEmployeeInfoByEmployeeID(EmployeeID);
//        }

//    }
//}

using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SalesManagement.Employees.Controls
{
    public partial class ctrlEmployeeCard : UserControl
    {
        private clsEmployee _Employee;
        private int _EmployeeID = -1;

        public int EmployeeID => _EmployeeID;
        public clsEmployee SelectedEmployeeInfo => _Employee;

        // Maps enum values to their Arabic display text
        private static readonly Dictionary<clsEmployee.enEmployeeQualifications, string> _qualificationMap =
            new Dictionary<clsEmployee.enEmployeeQualifications, string>
            {
                { clsEmployee.enEmployeeQualifications.eNothing,                  "لا يوجد"       },
                { clsEmployee.enEmployeeQualifications.eBachelors,                "بكالوريوس"     },
                { clsEmployee.enEmployeeQualifications.eDiploma,                  "دبلوم"         },
                { clsEmployee.enEmployeeQualifications.eHighSchoolCertificate,    "ثانوية عامة"   },
                { clsEmployee.enEmployeeQualifications.eIntermediateCertificate,  "تاسع أساسي"   },
            };

        private static readonly Dictionary<clsEmployee.enEmployeeStatus, string> _statusMap =
            new Dictionary<clsEmployee.enEmployeeStatus, string>
            {
                { clsEmployee.enEmployeeStatus.eSingle,   "أعزب"   },
                { clsEmployee.enEmployeeStatus.eMarried,  "متزوج"  },
                { clsEmployee.enEmployeeStatus.eWidow,    "أرمل"   },
                { clsEmployee.enEmployeeStatus.eDivorced, "مطلق"   },
            };

        public ctrlEmployeeCard()
        {
            InitializeComponent();
        }

        public void ResetEmployeeInfo()
        {
            _EmployeeID = -1;
            _Employee = null;

            ctrlPersonCard1.ResetPersonInfo();

            txtEmployeeID.Text = "N/A";
            txtEmployeeQualification.Text = "غير محدد";
            txtEmployeeState.Text = "غير محدد";
        }

        private void _FillEmployeeInfo()
        {
            ctrlPersonCard1.LoadPersonInfo(_Employee.PersonID);

            txtEmployeeID.Text = _Employee.EmployeeID.ToString();

            // TryGetValue returns the Arabic text if found, otherwise "غير معروف"
            txtEmployeeQualification.Text = _qualificationMap
                .TryGetValue(_Employee.EmployeeQualification, out string qual)
                ? qual : "غير معروف";

            txtEmployeeState.Text = _statusMap
                .TryGetValue(_Employee.EmployeeState, out string status)
                ? status : "غير معروف";
        }

        public void LoadEmployeeInfoByEmployeeID(int employeeID)
        {
            _Employee = clsEmployee.FindByEmployeeID(employeeID);

            if (!_HandleResult(_Employee, $"لا يوجد موظف بهذا الرقم: {employeeID}"))
                return;

            _EmployeeID = _Employee.EmployeeID;
            _FillEmployeeInfo();
        }

        public void LoadEmployeeInfoByFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                _ShowWarning("يرجى إدخال اسم الموظف.");
                return;
            }

            clsPerson person = clsPerson.FindByFullName(fullName);

            if (!_HandleResult(person, $"لا يوجد شخص بهذا الاسم: {fullName}"))
                return;

            _Employee = clsEmployee.FindByPersonID(person.PersonID);

            if (!_HandleResult(_Employee, $"لا يوجد موظف بهذا الاسم: {fullName}"))
                return;

            _EmployeeID = _Employee.EmployeeID;
            _FillEmployeeInfo();
        }

        // Returns true if the object is found, false otherwise (resets + shows error)
        private bool _HandleResult(object result, string errorMessage)
        {
            if (result != null)
                return true;

            ResetEmployeeInfo();
            _ShowError(errorMessage);
            return false;
        }

        private void ctrlEmployeeCard_Load(object sender, EventArgs e)
        {
            ResetEmployeeInfo();
        }

        private void llEditEmployeeInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_EmployeeID <= 0)
            {
                _ShowWarning("يرجى تحميل بيانات موظف أولاً.");
                return;
            }

            frmAddUpdateEmployee frm = new frmAddUpdateEmployee(_EmployeeID);
            frm.DataBack += _OnDataBack;
            clsGlobal._OpenForm(frm);
        }

        private void _OnDataBack(object sender, int employeeID)
        {
            LoadEmployeeInfoByEmployeeID(employeeID);
        }

        private void _ShowError(string message)
            => MessageBox.Show(message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void _ShowWarning(string message)
            => MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
