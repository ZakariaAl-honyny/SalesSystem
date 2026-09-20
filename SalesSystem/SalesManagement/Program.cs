using SalesManagement.Categories;
using SalesManagement.Companies;
using SalesManagement.Customers;
using SalesManagement.Delegates;
using SalesManagement.Employees;
using SalesManagement.Expenses;
using SalesManagement.Items;
using SalesManagement.People;
using SalesManagement.Revenues;
using SalesManagement.Stores;
using SalesManagement.Suppliers;
using SalesManagement.Unit;
using SalesManagement.Units;
using SalesManagement.Users;

namespace SalesManagement
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            //frmMain frm = new frmMain(null!);
            //GlobalClasses.clsGlobal.frmMdiMain = frm;
            //Application.Run(frm);


            //Application.Run(new frmAddUpdatePerson());
            //Application.Run(new frmAddUpdateCustomer());
            //Application.Run(new frmListPeople());
            //Application.Run(new frmTest());
            //Application.Run(new frmListEmployees());
            //Application.Run(new frmListItems());
            Application.Run(new frmLogin());
        }
    }
}