using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement.Expenses
{
    public partial class frmListExpenses : Form
    {

        private static DataView dvExpenses;

        public frmListExpenses()
        {
            InitializeComponent();
        }

        private int _ExpenseID_dgvExpenses()
        {
            return (int)dgvExpenses.CurrentRow.Cells[0].Value;
        }

        //private async Task _RefreshExpensesList()
        //{

        //    //--------------
        //    //using DataView

        //    dvExpenses = await clsExpense.GetAllExpenses();

        //    dgvExpenses.DataSource = dvExpenses;
        //    lblRecordsCount.Text = dvExpenses.Count.ToString();
        //}

        private void _RefreshExpensesList()
        {

            //--------------
            //using DataView

            dvExpenses = clsExpense.GetAllExpenses();

            dgvExpenses.DataSource = dvExpenses;
            lblRecordsCount.Text = dvExpenses.Count.ToString();
        }

        private void frmListExpenses_Load(object sender, EventArgs e)
        {
            //Task _ = _RefreshExpensesList();
            _RefreshExpensesList();

            //select the default Filter
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");


            if (dgvExpenses.Rows.Count > 0)
            {
                dgvExpenses.Columns[0].HeaderText = "رقم المصروف";
                dgvExpenses.Columns[0].Width = 180;
                //dgvExpenses.ColumnHeadersHeight = 50;

                dgvExpenses.Columns[1].HeaderText = "اسم المصروف";
                dgvExpenses.Columns[1].Width = 180;

                dgvExpenses.Columns[2].HeaderText = "تفاصيل المصروف";
                dgvExpenses.Columns[2].Width = 250;
            }


        }

        private void btnAddNewExpense_Click(object sender, EventArgs e)
        {
            frmAddUpdateExpense frm1 = new frmAddUpdateExpense();
            clsGlobal._OpenForm(frm1);

            //clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListExpenses_Load(null!, null!);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "لا شيئ";
            //filter by cloumn name as is in the database
            switch (cbFilterBy.Text)
            {


                case "رقم المصروف":
                    FilterColumn = "ExpenseID";
                    break;
                case "اسم المصروف":
                    FilterColumn = "ExpenseName";
                    break;
                case "تفاصيل المصروف":
                    FilterColumn = "ExpenseDescription";
                    break;
                default:
                    FilterColumn = "لا شيئ";
                    break;
            }

            //Reset filter if there is nothing or FilterColumn is لا شيئ
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "لا شيئ")
            {
                dvExpenses.RowFilter = "";
                lblRecordsCount.Text = dvExpenses.Count.ToString();
                return;
            }
            if (dvExpenses.Count != 0)
            {


                if (FilterColumn == "ExpenseID")
                    dvExpenses.RowFilter = String.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
                else
                    dvExpenses.RowFilter = String.Format("{0} LIKE '%{1}%' ", FilterColumn, txtFilterValue.Text.Trim());

                lblRecordsCount.Text = dvExpenses.Count.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListExpenses_Load(null!, null!);
        }

        private void showDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmShowExpenseInfo frm1 = new frmShowExpenseInfo(_ExpenseID_dgvExpenses());
            //frm1.ShowDialog();

            frmListExpenses_Load(null!, null!);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateExpense frm1 = new frmAddUpdateExpense(_ExpenseID_dgvExpenses());
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListExpenses_Load(null!, null!);
        }

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateExpense frm1 = new frmAddUpdateExpense();
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListExpenses_Load(null!, null!);
        }

        private void DeleteExpense_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من حذف بيانات هذا المصروف؟", "تأكيد الحذف",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {
                //Perform Delele
                if (clsExpense.DeleteExpense(_ExpenseID_dgvExpenses()))
                {
                    MessageBox.Show("تم حذف البيانات بنجاح", "تم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListExpenses_Load(null!, null!);
                }

                else
                    MessageBox.Show("لم تم حذف البيانات.", "لم يتم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dgvAllExpenses_DoubleClick(object sender, EventArgs e)
        {
            if (dgvExpenses.Rows.Count > 0)
            {
                frmAddUpdateExpense frm1 = new frmAddUpdateExpense(_ExpenseID_dgvExpenses());
                clsGlobal._OpenForm(frm1);

                //frm1.ShowDialog();

                frmListExpenses_Load(null!, null!);
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "لا شيئ");
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase Expense id or Expense id are selected.
            if (cbFilterBy.Text == "رقم المصروف")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            else
                e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void cmsExpenses_Opening(object sender, CancelEventArgs e)
        {
            cmsExpenses.Enabled = (dgvExpenses.Rows.Count > 0);
        }

    }
}
