using SalesManagement.Categories;
using SalesManagement.Companies;
using SalesManagement.Customers;
using SalesManagement.Delegates;
using SalesManagement.Employees;
using SalesManagement.GlobalClasses;
using SalesManagement.Items;
using SalesManagement.Revenues;
using SalesManagement.Stores;
using SalesManagement.Suppliers;
using SalesManagement.Unit;
using SalesManagement.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement
{
    public partial class frmMain : System.Windows.Forms.Form
    {
        private frmLogin _frmLogin;

        public frmMain(frmLogin frmLogin)
        {
            InitializeComponent();
            _frmLogin = frmLogin;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Top = 0;

            var MdiContainerPage = this.Controls.OfType<MdiClient>().FirstOrDefault();
            if (MdiContainerPage != null)
                MdiContainerPage.BackColor = Color.WhiteSmoke;
            //MdiContainerPage.BackColor = Color.DarkKhaki;
            //MdiContainerPage.BackColor = Color.DarkGoldenrod;
            //MdiContainerPage.BackColor = Color.LightGray;
            //MdiContainerPage.BackColor = Color.WhiteSmoke;

        }

        private void msbtnLogout_Click(object sender, EventArgs e)
        {
            _frmLogin.Show();
            this.Close();
        }

        //private void clsGlobal._OpenForm(System.Windows.Forms.Form form)
        //{
        //    foreach (System.Windows.Forms.Form frm in Application.OpenForms)
        //    {
        //        if (frm.Name == "frmLogin" || frm.Name == "frmMain") continue;
        //        if (frm.Name == form.Name)
        //        {
        //            frm.Activate(); // Focus
        //            //frm.BringToFront();
        //            frm.WindowState = FormWindowState.Normal;
        //            return;
        //        }
        //    }

        //    form.Icon = this.Icon;
        //    form.MdiParent = this;
        //    form.Show();
        //    //form.ShowDialog();
        //}

        private void msbtnAddCategory_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmAddUpdateCategory());
            //frmAddUpdateCategory frm = new frmAddUpdateCategory();
            //frm.MdiParent = this;
            //frm.Show();
        }

        private void msbtnManageItems_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListItems());
        }

        private void msbtnAddItem_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmAddUpdateItem());
        }

        private void msbtnUpdateItem_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListItems());
        }

        private void msbtnDeleteItem_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListItems());
        }

        private void msbtnSearchItem_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListItems());
        }

        private void msbtnManageSuppliers_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListSuppliers());
        }

        private void msbtnAddSupplier_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmAddUpdateSupplier());
        }

        private void msbtnUpdateSupplier_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListSuppliers());
        }

        private void msbtnDeleteSupplier_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListSuppliers());
        }

        private void msbtnSearchSupplier_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListSuppliers());
        }

        private void msbtnManageCustomers_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCustomers());

        }

        private void msbtnAddCustomer_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmAddUpdateCustomer());

        }

        private void msbtnUpdateCustomer_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCustomers());

        }

        private void msbtnDeleteCustomer_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCustomers());

        }

        private void msbtnSearchCustomer_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCustomers());

        }

        private void msbtnManageStores_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListStores());

        }

        private void msbtnAddStore_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmAddUpdateStore());

        }

        private void msbtnUpdateStore_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListStores());

        }

        private void msbtnDeleteStore_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListStores());

        }

        private void msbtnSearchStore_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListStores());

        }

        private void msbtnManageCategories_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCategories());

        }

        private void msbtnUpdateCategory_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCategories());

        }

        private void msbtnDeleteCategory_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCategories());

        }

        private void msbtnSearchCategory_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCategories());

        }

        private void msbtnManageUnits_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListUnits());

        }

        private void msbtnAddUnit_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmAddUpdateUnit());

        }

        private void msbtnUpdateUnit_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListUnits());

        }

        private void msbtnDeleteUnit_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListUnits());

        }

        private void msbtnSearchUnit_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListUnits());

        }

        private void msbtnManageCompanies_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCompanies());

        }

        private void msbtnAddCompany_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmAddUpdateCompany());

        }

        private void msbtnUpdateCompany_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCompanies());

        }

        private void msbtnDeleteCompany_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCompanies());

        }

        private void msbtnSearchCompany_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListCompanies());

        }

        private void msbtnManageDelegates_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListDelegates());

        }

        private void msbtnAddDelegate_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmAddUpdateDelegate());

        }

        private void msbtnUpdateDelegate_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListDelegates());

        }

        private void msbtnDeleteDelegate_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListDelegates());

        }

        private void msbtnSearchDelegate_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListDelegates());

        }

        private void msbtnManageRevenues_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListRevenues());

        }

        private void msbtnAddRevenue_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmAddUpdateRevenue());

        }

        private void msbtnUpdateRevenue_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListRevenues());

        }

        private void msbtnDeleteRevenue_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListRevenues());

        }

        private void msbtnSearchRevenue_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListRevenues());

        }

        private void msbtnManageEmployees_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListEmployees());

        }

        private void msbtnAddEmployee_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmAddUpdateEmployee());

        }

        private void msbtnUpdateEmployee_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListEmployees());

        }

        private void msbtnDeleteEmployee_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListEmployees());

        }

        private void msbtnSearchEmployee_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(new frmListEmployees());

        }
    }
}
