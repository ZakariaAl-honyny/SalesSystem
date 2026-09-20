using SalesManagement.DataAccess;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SalesManagement.Busniess
{

    /// <summary>
    /// This class represents Company information.
    /// </summary>
    public class clsCompany
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int CompanyID { set; get; }
        
        public string CompanyName { set; get; }

        public string CompanyPhone { set; get; }

        public string Address { set; get; }

        public string? OtherDetails { set; get; }


        public clsCompany()

        {
            this.CompanyID = -1;
            this.CompanyName = "";
            this.CompanyPhone = "";
            this.Address = "";
            this.OtherDetails = "";

            Mode = enMode.AddNew;

        }

        private clsCompany(int CompanyID, string CompanyName, string CompanyPhone, string Address,
            string? OtherDetails)

        {
            this.CompanyID = CompanyID;
            this.CompanyName = CompanyName;
            this.CompanyPhone = CompanyPhone;
            this.Address = Address;
            this.OtherDetails = OtherDetails;

            Mode = enMode.Update;

        }

        /// <summary>
        /// Add new Company and returns the result.
        /// </summary>
        /// <returns>If Company added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewCompany()
        {
            //call DataAccess Layer 

            this.CompanyID = clsCompanyData.AddNewCompany(this.CompanyName, this.CompanyPhone, this.Address,
                this.OtherDetails);

            return (this.CompanyID != -1);
        }

        /// <summary>
        /// Update Company info and returns the result.
        /// </summary>
        /// <returns>If Company updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateCompany()
        {
            //call DataAccess Layer 

            return clsCompanyData.UpdateCompany(this.CompanyID, this.CompanyName, this.CompanyPhone, this.Address,
                this.OtherDetails);

        }

        /// <summary>
        /// Find Company info by <paramref name="CompanyID"/> and returns the result.
        /// </summary>
        /// <param name="CompanyID">The Company id for find Company info.</param>
        /// <returns>If Company find successfully return Company <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsCompany Find(int CompanyID)
        {

            string? OtherDetails = "";
            string CompanyName = "", CompanyPhone = "", Address = "";

            if (clsCompanyData.GetCompanyInfoByID(CompanyID, ref CompanyName, ref CompanyPhone, ref Address,
                ref OtherDetails))

                return new clsCompany(CompanyID, CompanyName, CompanyPhone, Address,
                    OtherDetails);
            else
                return null!;

        }

        /// <summary>
        /// Find Company info by <paramref name="CompanyID"/> and returns the result.
        /// </summary>
        /// <param name="CompanyID">The Company id for find Company info.</param>
        /// <returns>If Company find successfully return Company <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsCompany Find(int? CompanyID)
        {
            int ID = -1;
            if (CompanyID != null)
                ID = (int)CompanyID;
            string? OtherDetails = "";
            string CompanyName = "", CompanyPhone = "", Address = "";

            if (clsCompanyData.GetCompanyInfoByID(ID, ref CompanyName, ref CompanyPhone, ref Address,
                ref OtherDetails))

                return new clsCompany(ID, CompanyName, CompanyPhone, Address,
                    OtherDetails);
            else
                return null!;

        }


        /// <summary>
        /// Find Company info by <paramref name="CompanyName"/> and returns the result.
        /// </summary>
        /// <param name="CompanyName">The National No for find Company info.</param>
        /// <returns>If Company find successfully return Company <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsCompany Find(string CompanyName)
        {

            int CompanyID = -1;
            string? OtherDetails = "";
            string CompanyPhone = "", Address = "";

            if (clsCompanyData.GetCompanyInfoByCompanyName(CompanyName, ref CompanyID, ref CompanyPhone, ref Address,
                ref OtherDetails))

                return new clsCompany(CompanyID, CompanyName, CompanyPhone, Address,
                    OtherDetails);
            else
                return null!;

        }

        /// <summary>
        /// If Mode add new so will add new Company and returns the result, 
        /// if Mode update so will Save Company changes and returns the result.
        /// </summary>
        /// <returns>If Company save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCompany())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateCompany();

            }




            return false;
        }

        /// <summary>
        /// This function will get all Companys from database. 
        /// </summary>
        /// <returns>All Companys info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetAllCompanies()
        {
            return clsCompanyData.GetAllCompaniesAsync();
        }

        /// <summary>
        /// This function will delete Company from database if <paramref name="CompanyID"/> is exists and returns the result.
        /// </summary>
        /// <param name="CompanyID">The Company ID for delete Company info.</param>
        /// <returns>If Company deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteCompany(int CompanyID)
        {
            return clsCompanyData.DeleteCompany(CompanyID);
        }

        /// <summary>
        /// This function will check if Company exists into database by <paramref name="CompanyID"/> and returns the result, 
        /// </summary>
        /// <param name="CompanyID">The Company ID for check Company is exists.</param>
        /// <returns>If Company exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isCompanyExist(int CompanyID)
        {
            return clsCompanyData.IsCompanyExist(CompanyID);
        }

        /// <summary>
        /// This function will check if Company exists in database by <paramref name="CompanyName"/> and returns the result, 
        /// </summary>
        /// <param name="CompanyName">The National No for check Company is exists.</param>
        /// <returns>If Company exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isCompanyExist(string CompanyName)
        {
            return clsCompanyData.IsCompanyExist(CompanyName);
        }

    }
}
