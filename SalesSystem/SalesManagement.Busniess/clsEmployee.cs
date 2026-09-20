using SalesManagement.DataAccess;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SalesManagement.Busniess
{
    /// <summary>
    /// This class represents Employee information.
    /// </summary>
    public class clsEmployee
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enEmployeeQualifications { eNothing = 1, eBachelors = 2, eDiploma = 3, eHighSchoolCertificate = 4, eIntermediateCertificate = 5 };
        public enum enEmployeeStatus { eSingle = 1, eMarried = 2, eWidow = 3, eDivorced = 4 };

        public int EmployeeID { set; get; }
        public int PersonID { set; get; }
        public clsPerson PersonInfo;

        public enEmployeeQualifications EmployeeQualification { set; get; }
        public enEmployeeStatus EmployeeState { set; get; }
        public int CreatedByUserID { set; get; }
        public DateTime CreatedDate { set; get; }
        public clsEmployee()

        {
            this.EmployeeID = -1;
            this.PersonID = -1;
            this.EmployeeQualification = enEmployeeQualifications.eNothing;
            this.EmployeeState = enEmployeeStatus.eSingle;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;

            Mode = enMode.AddNew;

        }

        private clsEmployee(int EmployeeID, int PersonID, enEmployeeQualifications EmployeeQualification, enEmployeeStatus EmployeeState,
            int CreatedByUserID, DateTime CreatedDate)

        {
            this.EmployeeID = EmployeeID;
            this.EmployeeQualification = EmployeeQualification;
            this.EmployeeState = EmployeeState;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;

            Mode = enMode.Update;

        }

        /// <summary>
        /// Add new Employee into database and returns the result.
        /// </summary>
        /// <returns>If Employee added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewEmployee()
        {
            //call DataAccess Layer 

            this.EmployeeID = clsEmployeeData.AddNewEmployee(this.PersonID, (byte) this.EmployeeQualification, (byte)this.EmployeeState, this.CreatedByUserID);

            return (this.EmployeeID != -1);
        }

        /// <summary>
        /// Update Employee info and returns the result.
        /// </summary>
        /// <returns>If Employee updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateEmployee()
        {
            //call DataAccess Layer 

            return clsEmployeeData.UpdateEmployee(this.EmployeeID, this.PersonID, (byte)this.EmployeeQualification, (byte)this.EmployeeState,
                this.CreatedByUserID, this.CreatedDate);

        }

        /// <summary>
        /// Find Employee info by <paramref name="EmployeeID"/> and returns the result.
        /// </summary>
        /// <param name="EmployeeID">The Employee ID for find Employee info.</param>
        /// <returns>If Employee find successfully return Employee <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsEmployee FindByEmployeeID(int EmployeeID)
        {

            int PersonID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;
            byte EmployeeQualification = (byte)enEmployeeQualifications.eNothing;
            byte EmployeeState = (byte)enEmployeeStatus.eSingle;

            if (clsEmployeeData.GetEmployeeInfoByEmployeeID(EmployeeID, ref PersonID, ref EmployeeQualification, ref EmployeeState,
                ref CreatedByUserID, ref CreatedDate))

                return new clsEmployee(EmployeeID, PersonID, (enEmployeeQualifications)EmployeeQualification, (enEmployeeStatus)EmployeeState, CreatedByUserID, CreatedDate);
            else
                return null!;
        }



        /// <summary>
        /// Find Employee info by <paramref name="PersonID"/> and returns the result.
        /// </summary>
        /// <param name="PersonID">The Person ID for find Employee info.</param>
        /// <returns>If Employee find successfully return Employee <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsEmployee FindByPersonID(int PersonID)
        {

            int EmployeeID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;
            byte EmployeeQualification = (byte)enEmployeeQualifications.eNothing;
            byte EmployeeState = (byte)enEmployeeStatus.eSingle;

            if (clsEmployeeData.GetEmployeeInfoByPersonID(PersonID, ref EmployeeID, ref EmployeeQualification, ref EmployeeState,
                ref CreatedByUserID, ref CreatedDate))

                return new clsEmployee(EmployeeID, PersonID, (enEmployeeQualifications)EmployeeQualification, (enEmployeeStatus)EmployeeState, CreatedByUserID, CreatedDate);
            else
                return null!;

        }

        /// <summary>
        /// Find Employee FullName by <paramref name="EmployeeID"/> and returns the result.
        /// </summary>
        /// <param name="EmployeeID">The Person ID for find Employee info.</param>
        /// <returns>If Employee find successfully return Employee <seealso cref="object"/>, otherwise <see langword="string.Empty"/>.</returns>
        public static string FindEmployeeFullNameByEmployeeID(int? EmployeeID)
        {

            string FullName = string.Empty;

            if (clsEmployeeData.GetEmployeeFullNameByEmployeeID( EmployeeID, ref FullName))

                return FullName;
            else
                return string.Empty;

        }

        /// <summary>
        /// Find Employee FullName by <paramref name="FullName"/> and returns the result.
        /// </summary>
        /// <param name="FullName">The Person ID for find Employee ID.</param>
        /// <returns>If Employee find successfully return Employee <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static int FindEmployeeIDByEmployeeFullName(string FullName)
        {

            int EmployeeID = -1;

            if (clsEmployeeData.GetEmployeeIDByEmployeeFullName(FullName, ref EmployeeID))

                return EmployeeID;
            else
                return -1;

        }

        /// <summary>
        /// If Mode add new so will add new Employee and returns the result, 
        /// if Mode update so will Save Employee changes and returns the result.
        /// </summary>
        /// <returns>If Employee save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewEmployee())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateEmployee();

            }




            return false;
        }

        /// <summary>
        /// This function will get all Employees from database. 
        /// </summary>
        /// <returns>All Employees info into <see langword="DataView"/> class.</returns>
        public static DataView GetAllEmployees()
        {
            return clsEmployeeData.GetAllEmployees();

        }

        /// <summary>
        /// This function will delete Employee from database if <paramref name="EmployeeID"/> is exists and returns the result.
        /// </summary>
        /// <param name="EmployeeID">The Employee ID for delete Employee info.</param>
        /// <returns>If Employee deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteEmployee(int EmployeeID)
        {
            return clsEmployeeData.DeleteEmployee(EmployeeID);
        }

        /// <summary>
        /// This function will check if Employee exists into database by <paramref name="EmployeeID"/> and returns the result, 
        /// </summary>
        /// <param name="EmployeeID">The Employee ID for check Employee is exists.</param>
        /// <returns>If Employee exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isEmployeeExistByID(int EmployeeID)
        {
            return clsEmployeeData.IsEmployeeExist(EmployeeID);
        }

        /// <summary>
        /// This function will check if Employee exists into database by <paramref name="PersonID"/> and returns the result, 
        /// </summary>
        /// <param name="PersonID">The Employee ID for check Employee is exists.</param>
        /// <returns>If Employee exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isEmployeeExistByPersonID(int PersonID)
        {
            return clsEmployeeData.IsEmployeeExistByPersonID(PersonID);
        }

        /// <summary>
        /// This function will get Employee ID for <paramref name="PersonID"/> from database and returns the result. 
        /// </summary>
        /// <param name="PersonID">The Person ID for get Employee ID.</param>
        /// <returns>Employee ID if exists, otherwise <see langword="-1"/>.</returns>
        public static int GetEmployeeIDByPersonID(int PersonID)
        {
            return clsEmployeeData.GetEmployeeIDByPersonID(PersonID);
        }
        /// <summary>
        /// This function will get Employee ID for current object from database and returns the result. 
        /// </summary>
        /// <returns>Employee ID if exists, otherwise <see langword="-1"/>.</returns>
        public int GetEmployeeID()
        {
            return GetEmployeeIDByPersonID(this.PersonID);
        }

    }
}
