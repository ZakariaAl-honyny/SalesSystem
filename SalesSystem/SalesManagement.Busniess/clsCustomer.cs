using SalesManagement.DataAccess;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SalesManagement.Busniess
{

    /// <summary>
    /// This class represents Customer information.
    /// </summary>
    public class clsCustomer
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enGender { Male = 0, Female = 1 };
        public enGender Gender { set; get; }

        public int CustomerID { set; get; }
        
        public int PersonID { set; get; }

        public DateTime StartDate { set; get; }

        public string? OtherDetails { set; get; }

        public clsCustomer()

        {
            this.CustomerID = -1;
            this.PersonID = -1;
            this.StartDate = DateTime.Now;
            this.OtherDetails = "";

            Mode = enMode.AddNew;

        }

        private clsCustomer(int CustomerID, int PersonID, DateTime StartDate, string? OtherDetails)

        {
            this.CustomerID = CustomerID;
            this.PersonID = PersonID;
            this.StartDate = StartDate;
            this.OtherDetails = OtherDetails;

            Mode = enMode.Update;

        }

        /// <summary>
        /// Add new Customer and returns the result.
        /// </summary>
        /// <returns>If Customer added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewCustomer()
        {
            //call DataAccess Layer 

            this.CustomerID = clsCustomerData.AddNewCustomer(this.PersonID, this.StartDate, this.OtherDetails);

            return (this.CustomerID != -1);
        }

        /// <summary>
        /// Update Customer info and returns the result.
        /// </summary>
        /// <returns>If Customer updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateCustomer()
        {
            //call DataAccess Layer 

            return clsCustomerData.UpdateCustomer(this.CustomerID, this.PersonID, this.StartDate, this.OtherDetails);

        }

        /// <summary>
        /// Find Customer info by <paramref name="CustomerID"/> and returns the result.
        /// </summary>
        /// <param name="CustomerID">The Customer id for find Customer info.</param>
        /// <returns>If Customer find successfully return Customer <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsCustomer Find(int CustomerID)
        {
            string? OtherDetails = "";
            int PersonID = -1;
            DateTime StartDate = DateTime.Now;

            if (clsCustomerData.GetCustomerInfoByID(CustomerID, ref PersonID, ref StartDate, ref OtherDetails))

                return new clsCustomer(CustomerID, PersonID, StartDate, OtherDetails);
            else
                return null!;

        }

        /// <summary>
        /// Find Customer info by <paramref name="PersonID"/> and returns the result.
        /// </summary>
        /// <param name="PersonID">The National No for find Customer info.</param>
        /// <returns>If Customer find successfully return Customer <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsCustomer FindByPersonID(int PersonID)
        {

            string? OtherDetails = "";
            int CustomerID = -1;
            DateTime StartDate = DateTime.Now;

            if (clsCustomerData.GetCustomerInfoByPersonID(PersonID, ref CustomerID, ref StartDate, ref OtherDetails))

                return new clsCustomer(PersonID, CustomerID, StartDate, OtherDetails);
            else
                return null!;

        }

        //public static string FindEmployeeFullNameByEmployeeID(int? EmployeeID)
        //{
        //    return clsEmployee.FindEmployeeFullNameByEmployeeID(EmployeeID);
        //}

        /// <summary>
        /// Find Employee FullName by <paramref name="FullName"/> and returns the result.
        /// </summary>
        /// <param name="FullName">The Person ID for find Employee ID.</param>
        /// <returns>If Employee find successfully return Employee <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        //public static int FindEmployeeIDByEmployeeFullName(string FullName)
        //{
        //    return clsEmployee.FindEmployeeIDByEmployeeFullName(FullName);
        //}


        /// <summary>
        /// If Mode add new so will add new Customer and returns the result, 
        /// if Mode update so will Save Customer changes and returns the result.
        /// </summary>
        /// <returns>If Customer save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCustomer())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateCustomer();
            }




            return false;
        }

        /// <summary>
        /// This function will get all Customers from database. 
        /// </summary>
        /// <returns>All Customers info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetAllCustomers()
        {
            return clsCustomerData.GetAllCustomersAsync();
        }

        /// <summary>
        /// This function will delete Customer from database if <paramref name="CustomerID"/> is exists and returns the result.
        /// </summary>
        /// <param name="CustomerID">The Customer ID for delete Customer info.</param>
        /// <returns>If Customer deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteCustomer(int CustomerID)
        {
            return clsCustomerData.DeleteCustomer(CustomerID);
        }

        /// <summary>
        /// This function will check if Customer exists into database by <paramref name="CustomerID"/> and returns the result, 
        /// </summary>
        /// <param name="CustomerID">The Customer ID for check Customer is exists.</param>
        /// <returns>If Customer exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isCustomerExist(int CustomerID)
        {
            return clsCustomerData.IsCustomerExist(CustomerID);
        }

        /// <summary>
        /// This function will check if Customer exists in database by <paramref name="PersonID"/> and returns the result, 
        /// </summary>
        /// <param name="PersonID">The National No for check Customer is exists.</param>
        /// <returns>If Customer exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isCustomerExistByPersonID(int PersonID)
        {
            return clsCustomerData.IsCustomerExistByPersonID(PersonID);
        }


        /// <summary>
        /// This function will get all Employees from database. 
        /// </summary>
        /// <returns>All Employees info into <see langword="DataView"/> class.</returns>
        //public static DataView GetAllEmployees()
        //{
        //    return clsEmployee.GetAllEmployees();
        //}


    }
}
