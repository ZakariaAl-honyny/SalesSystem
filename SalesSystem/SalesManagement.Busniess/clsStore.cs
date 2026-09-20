using SalesManagement.DataAccess;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SalesManagement.Busniess
{

    /// <summary>
    /// This class represents Store information.
    /// </summary>
    public class clsStore
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enGender { Male = 0, Female = 1 };
        public enGender Gender { set; get; }

        public int StoreID { set; get; }
        
        public string StoreName { set; get; }

        public string StoreAddress { set; get; }

        public string? StorePhone { set; get; }

        public string? StoreDetails { set; get; }

        public int? StoreManagerID { set; get; }
        public clsStore()

        {
            this.StoreID = -1;
            this.StoreName = "";
            this.StoreAddress = "";
            this.StorePhone = "";
            this.StoreDetails = "";
            this.StoreManagerID = -1;

            Mode = enMode.AddNew;

        }

        private clsStore(int StoreID, string StoreName, string StoreAddress, string? StorePhone,
            string? StoreDetails, int? StoreManagerID)

        {
            this.StoreID = StoreID;
            this.StoreName = StoreName;
            this.StoreAddress = StoreAddress;
            this.StorePhone = StorePhone;
            this.StoreDetails = StoreDetails;
            this.StoreManagerID = StoreManagerID;

            Mode = enMode.Update;

        }

        /// <summary>
        /// Add new Store and returns the result.
        /// </summary>
        /// <returns>If Store added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewStore()
        {
            //call DataAccess Layer 

            this.StoreID = clsStoreData.AddNewStore(this.StoreName, this.StoreAddress, this.StorePhone,
                this.StoreDetails, this.StoreManagerID);

            return (this.StoreID != -1);
        }

        /// <summary>
        /// Update Store info and returns the result.
        /// </summary>
        /// <returns>If Store updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateStore()
        {
            //call DataAccess Layer 

            return clsStoreData.UpdateStore(this.StoreID, this.StoreName, this.StoreAddress, this.StorePhone,
                this.StoreDetails, this.StoreManagerID);

        }

        /// <summary>
        /// Find Store info by <paramref name="StoreID"/> and returns the result.
        /// </summary>
        /// <param name="StoreID">The Store id for find Store info.</param>
        /// <returns>If Store find successfully return Store <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsStore Find(int StoreID)
        {
            int? StoreManagerID = -1;
            string? StoreDetails = "" , StorePhone = "";
            string StoreName = "", StoreAddress = "";

            if (clsStoreData.GetStoreInfoByID(StoreID, ref StoreName, ref StoreAddress, ref StorePhone,
                ref StoreDetails, ref StoreManagerID))

                return new clsStore(StoreID, StoreName, StoreAddress, StorePhone,
                    StoreDetails, StoreManagerID);
            else
                return null!;

        }

        /// <summary>
        /// Find Store info by <paramref name="StoreName"/> and returns the result.
        /// </summary>
        /// <param name="StoreName">The National No for find Store info.</param>
        /// <returns>If Store find successfully return Store <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsStore Find(string StoreName)
        {

            int StoreID = -1;
            int? StoreManagerID = -1;
            string? StoreDetails = "", StorePhone = "";
            string StoreAddress = "";

            if (clsStoreData.GetStoreInfoByStoreName(StoreName, ref StoreID, ref StoreAddress, ref StorePhone,
                ref StoreDetails, ref StoreManagerID))

                return new clsStore(StoreID, StoreName, StoreAddress, StorePhone,
                    StoreDetails, StoreManagerID);
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
        /// If Mode add new so will add new Store and returns the result, 
        /// if Mode update so will Save Store changes and returns the result.
        /// </summary>
        /// <returns>If Store save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewStore())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateStore();
            }




            return false;
        }

        /// <summary>
        /// This function will get all Stores from database. 
        /// </summary>
        /// <returns>All Stores info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetAllStores()
        {
            return clsStoreData.GetAllStoresAsync();
        }

        /// <summary>
        /// This function will delete Store from database if <paramref name="StoreID"/> is exists and returns the result.
        /// </summary>
        /// <param name="StoreID">The Store ID for delete Store info.</param>
        /// <returns>If Store deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteStore(int StoreID)
        {
            return clsStoreData.DeleteStore(StoreID);
        }

        /// <summary>
        /// This function will check if Store exists into database by <paramref name="StoreID"/> and returns the result, 
        /// </summary>
        /// <param name="StoreID">The Store ID for check Store is exists.</param>
        /// <returns>If Store exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isStoreExist(int StoreID)
        {
            return clsStoreData.IsStoreExist(StoreID);
        }

        /// <summary>
        /// This function will check if Store exists in database by <paramref name="StoreName"/> and returns the result, 
        /// </summary>
        /// <param name="StoreName">The National No for check Store is exists.</param>
        /// <returns>If Store exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isStoreExist(string StoreName)
        {
            return clsStoreData.IsStoreExist(StoreName);
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
