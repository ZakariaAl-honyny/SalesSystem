using SalesManagement.DataAccess;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SalesManagement.Busniess
{

    /// <summary>
    /// This class represents Supplier information.
    /// </summary>
    public class clsSupplier
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enGender { Male = 0, Female = 1 };
        public enGender Gender { set; get; }

        public int SupplierID { set; get; }
        
        public string OwnerName { set; get; }

        public string OwnerPhone { set; get; }

        public string ManagerName { set; get; }

        public string ManagerPhone { set; get; }

        public int PersonID { set; get; }
        public clsSupplier()

        {
            this.SupplierID = -1;
            this.OwnerName = "";
            this.OwnerPhone = "";
            this.ManagerName = "";
            this.ManagerPhone = "";
            this.PersonID = -1;

            Mode = enMode.AddNew;

        }

        private clsSupplier(int SupplierID, string OwnerName, string OwnerPhone, string ManagerName,
            string ManagerPhone, int PersonID)

        {
            this.SupplierID = SupplierID;
            this.OwnerName = OwnerName;
            this.OwnerPhone = OwnerPhone;
            this.ManagerName = ManagerName;
            this.ManagerPhone = ManagerPhone;
            this.PersonID = PersonID;

            Mode = enMode.Update;

        }

        /// <summary>
        /// Add new Supplier and returns the result.
        /// </summary>
        /// <returns>If Supplier added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewSupplier()
        {
            //call DataAccess Layer 

            this.SupplierID = clsSupplierData.AddNewSupplier(this.OwnerName, this.OwnerPhone, this.ManagerName,
                this.ManagerPhone, this.PersonID);

            return (this.SupplierID != -1);
        }

        /// <summary>
        /// Update Supplier info and returns the result.
        /// </summary>
        /// <returns>If Supplier updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateSupplier()
        {
            //call DataAccess Layer 

            return clsSupplierData.UpdateSupplier(this.SupplierID, this.OwnerName, this.OwnerPhone, this.ManagerName,
                this.ManagerPhone, this.PersonID);

        }

        /// <summary>
        /// Find Supplier info by <paramref name="SupplierID"/> and returns the result.
        /// </summary>
        /// <param name="SupplierID">The Supplier id for find Supplier info.</param>
        /// <returns>If Supplier find successfully return Supplier <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsSupplier Find(int SupplierID)
        {
            int PersonID = -1;
            string ManagerPhone = "" , ManagerName = "";
            string OwnerName = "", OwnerPhone = "";

            if (clsSupplierData.GetSupplierInfoByID(SupplierID, ref OwnerName, ref OwnerPhone, ref ManagerName,
                ref ManagerPhone, ref PersonID))

                return new clsSupplier(SupplierID, OwnerName, OwnerPhone, ManagerName,
                    ManagerPhone, PersonID);
            else
                return null!;

        }

        /// <summary>
        /// Find Supplier info by <paramref name="PersonID"/> and returns the result.
        /// </summary>
        /// <param name="PersonID">The National No for find Supplier info.</param>
        /// <returns>If Supplier find successfully return Supplier <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsSupplier FindByPersonID(int PersonID)
        {

            int SupplierID = -1;
            string ManagerPhone = "", ManagerName = "";
            string OwnerName = "",OwnerPhone = "";

            if (clsSupplierData.GetSupplierInfoByPersonID(PersonID,ref OwnerName, ref SupplierID, ref OwnerPhone, ref ManagerName,
                ref ManagerPhone))

                return new clsSupplier(SupplierID, OwnerName, OwnerPhone, ManagerName,
                    ManagerPhone, PersonID);
            else
                return null!;

        }

        public static string FindEmployeeFullNameByEmployeeID(int? EmployeeID)
        {
            return clsEmployee.FindEmployeeFullNameByEmployeeID(EmployeeID);
        }

        /// <summary>
        /// Find Employee FullName by <paramref name="FullName"/> and returns the result.
        /// </summary>
        /// <param name="FullName">The Person ID for find Employee ID.</param>
        /// <returns>If Employee find successfully return Employee <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static int FindEmployeeIDByEmployeeFullName(string FullName)
        {
            return clsEmployee.FindEmployeeIDByEmployeeFullName(FullName);
        }


        /// <summary>
        /// If Mode add new so will add new Supplier and returns the result, 
        /// if Mode update so will Save Supplier changes and returns the result.
        /// </summary>
        /// <returns>If Supplier save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewSupplier())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateSupplier();
            }




            return false;
        }

        /// <summary>
        /// This function will get all Suppliers from database. 
        /// </summary>
        /// <returns>All Suppliers info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetAllSuppliers()
        {
            return clsSupplierData.GetAllSuppliersAsync();
        }

        /// <summary>
        /// This function will delete Supplier from database if <paramref name="SupplierID"/> is exists and returns the result.
        /// </summary>
        /// <param name="SupplierID">The Supplier ID for delete Supplier info.</param>
        /// <returns>If Supplier deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteSupplier(int SupplierID)
        {
            return clsSupplierData.DeleteSupplier(SupplierID);
        }

        /// <summary>
        /// This function will check if Supplier exists into database by <paramref name="SupplierID"/> and returns the result, 
        /// </summary>
        /// <param name="SupplierID">The Supplier ID for check Supplier is exists.</param>
        /// <returns>If Supplier exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isSupplierExist(int SupplierID)
        {
            return clsSupplierData.IsSupplierExist(SupplierID);
        }

        /// <summary>
        /// This function will check if Supplier exists in database by <paramref name="PersonID"/> and returns the result, 
        /// </summary>
        /// <param name="PersonID">The National No for check Supplier is exists.</param>
        /// <returns>If Supplier exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isSupplierExistByPersonID(int PersonID)
        {
            return clsSupplierData.IsSupplierExistByPersonID(PersonID);
        }


        /// <summary>
        /// This function will get all Employees from database. 
        /// </summary>
        /// <returns>All Employees info into <see langword="DataView"/> class.</returns>
        public static DataView GetAllEmployees()
        {
            return clsEmployee.GetAllEmployees();
        }


    }
}
