using SalesManagement.Busniess;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SalesManagement.Unit
{
    public partial class frmAddUpdateUnit : Form
    {
        // Delegate defines the signature for sending data back to the caller form
        public delegate void DataBackEventHandler(object sender, int? unitID);
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode;
        private int _UnitID = -1;
        private clsUnit _Unit = null!;

        public frmAddUpdateUnit()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateUnit(int unitID)
        {
            InitializeComponent();
            _UnitID = unitID;
            _Mode = enMode.Update;
        }

        // ── Private Methods ────────────────────────────────────────────────────────

        private void _ResetDefaultValues()
        {
            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = this.Text = "إضافة وحدة جديدة";
                _Unit = new clsUnit();
            }
            else
            {
                lblTitle.Text = this.Text = "تعديل وحدة";
                btnSave.Text = "تعديل";
                btnSave.Enabled = true;
            }

            txtUnitID.Text = "0";
            txtUnitName.Text = "";
            txtUnitDescription.Text = "";
        }

        private void _LoadData()
        {
            _Unit = clsUnit.FindByUnitID(_UnitID);

            if (_Unit == null)
            {
                MessageBox.Show(
                    $"لا توجد وحدة بهذا الرقم: {_UnitID}\nسيتم إغلاق الشاشة.",
                    "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            txtUnitID.Text = _Unit.UnitID.ToString();
            txtUnitName.Text = _Unit.UnitName;
            txtUnitDescription.Text = _Unit.UnitDescription;
        }

        private void _SwitchToUpdateMode()
        {
            _Mode = enMode.Update;
            lblTitle.Text = this.Text = "تعديل وحدة";
            btnSave.Text = "تعديل";
            txtUnitID.Text = _Unit.UnitID.ToString();
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmAddUpdateUnit_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void frmAddUpdateUnit_FormClosing(object sender, FormClosingEventArgs e)
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

            _Unit.UnitID = Convert.ToInt32(txtUnitID.Text);

            // Replace single quotes to prevent SQL injection issues at the UI level
            _Unit.UnitName = txtUnitName.Text.Trim().Replace("'", "");
            _Unit.UnitDescription = txtUnitDescription.Text.Trim().Replace("'", "");

            if (_Unit.Save())
            {
                _SwitchToUpdateMode();
                MessageBox.Show("تم حفظ بيانات الوحدة بنجاح.", "تم الحفظ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Notify the caller form with the saved UnitID
                DataBack?.Invoke(this, _Unit.UnitID);
            }
            else
            {
                MessageBox.Show("لم يتم حفظ بيانات الوحدة.", "خطأ",
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

        // ── Validation ─────────────────────────────────────────────────────────────

        // Generic reusable handler — attached to multiple TextBox Validating events
        private void _ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "هذا الحقل مطلوب.");
            }
            else
            {
                errorProvider1.SetError(textBox, null);
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Block punctuation characters from being entered
            e.Handled = char.IsPunctuation(e.KeyChar);
        }
    }
}