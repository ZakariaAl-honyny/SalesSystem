using SalesManagement.DataAccess;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SalesManagement.Busniess
{

    /// <summary>
    /// This class represents Delegate information.
    /// </summary>
    public class clsDelegate
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int DelegateID { set; get; }

        public int PersonID { set; get; }
        public clsDelegate()

        {
            this.DelegateID = -1;
            this.PersonID = -1;

            Mode = enMode.AddNew;

        }

        private clsDelegate(int DelegateID, int PersonID)

        {
            this.DelegateID = DelegateID;
            this.PersonID = PersonID;

            Mode = enMode.Update;

        }

        /// <summary>
        /// Add new Delegate and returns the result.
        /// </summary>
        /// <returns>If Delegate added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewDelegate()
        {
            //call DataAccess Layer 

            this.DelegateID = clsDelegateData.AddNewDelegate(this.PersonID);

            return (this.DelegateID != -1);
        }

        /// <summary>
        /// Update Delegate info and returns the result.
        /// </summary>
        /// <returns>If Delegate updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateDelegate()
        {
            //call DataAccess Layer 

            return clsDelegateData.UpdateDelegate(this.DelegateID, this.PersonID);

        }

        /// <summary>
        /// Find Delegate info by <paramref name="DelegateID"/> and returns the result.
        /// </summary>
        /// <param name="DelegateID">The Delegate id for find Delegate info.</param>
        /// <returns>If Delegate find successfully return Delegate <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsDelegate Find(int DelegateID)
        {
            int PersonID = -1;

            if (clsDelegateData.GetDelegateInfoByID(DelegateID,  ref PersonID))

                return new clsDelegate(DelegateID, PersonID);
            else
                return null!;

        }

        /// <summary>
        /// Find Delegate info by <paramref name="PersonID"/> and returns the result.
        /// </summary>
        /// <param name="PersonID">The National No for find Delegate info.</param>
        /// <returns>If Delegate find successfully return Delegate <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsDelegate FindByPersonID(int PersonID)
        {

            int DelegateID = -1;

            if (clsDelegateData.GetDelegateInfoByPersonID(PersonID, ref DelegateID))

                return new clsDelegate(DelegateID, PersonID);
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
        /// If Mode add new so will add new Delegate and returns the result, 
        /// if Mode update so will Save Delegate changes and returns the result.
        /// </summary>
        /// <returns>If Delegate save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDelegate())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDelegate();
            }




            return false;
        }

        /// <summary>
        /// This function will get all Delegates from database. 
        /// </summary>
        /// <returns>All Delegates info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetAllDelegates()
        {
            return clsDelegateData.GetAllDelegatesAsync();
        }

        /// <summary>
        /// This function will delete Delegate from database if <paramref name="DelegateID"/> is exists and returns the result.
        /// </summary>
        /// <param name="DelegateID">The Delegate ID for delete Delegate info.</param>
        /// <returns>If Delegate deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteDelegate(int DelegateID)
        {
            return clsDelegateData.DeleteDelegate(DelegateID);
        }

        /// <summary>
        /// This function will check if Delegate exists into database by <paramref name="DelegateID"/> and returns the result, 
        /// </summary>
        /// <param name="DelegateID">The Delegate ID for check Delegate is exists.</param>
        /// <returns>If Delegate exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isDelegateExist(int DelegateID)
        {
            return clsDelegateData.IsDelegateExist(DelegateID);
        }

        /// <summary>
        /// This function will check if Delegate exists in database by <paramref name="PersonID"/> and returns the result, 
        /// </summary>
        /// <param name="PersonID">The National No for check Delegate is exists.</param>
        /// <returns>If Delegate exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isDelegateExistByPersonID(int PersonID)
        {
            return clsDelegateData.IsDelegateExist(PersonID);
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
