using SalesManagement.DataAccess;
using System.Data;

namespace SalesManagement.Busniess
{
    public class clsRevenue
    {

        public enum enMode { AddNew = 0, Update = 1 };
        protected enMode _Mode = enMode.AddNew;

        public int RevenueID { set; get; }
        //public int PersonID { set; get; }
        //public clsPerson PersonInfo;

        public string RevenueName { set; get; }

        public string RevenueDescription { set; get; }
        public clsRevenue()

        {
            this.RevenueID = -1;
            this.RevenueName = "";
            this.RevenueDescription = "";

            _Mode = enMode.AddNew;

        }

        private clsRevenue(int RevenueID, string RevenueName,
            string RevenueDescription)

        {
            this.RevenueID = RevenueID;
            //this.PersonID = PersonID;
            //PersonInfo = clsPerson.Find(PersonID);
            this.RevenueName = RevenueName;
            this.RevenueDescription = RevenueDescription;

            _Mode = enMode.Update;

        }

        /// <summary>
        /// Add new Revenue and returns the result.
        /// </summary>
        /// <returns>If Revenue added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewRevenue()
        {
            //call DataAccess Layer 

            this.RevenueID = clsRevenueData.AddNewRevenue(this.RevenueName,
                this.RevenueDescription);

            return (this.RevenueID != -1);
        }

        /// <summary>
        /// Update Revenue info and returns the result.
        /// </summary>
        /// <returns>If Revenue updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateRevenue()
        {
            //call DataAccess Layer 

            //return clsRevenueData.UpdateRevenue(this.RevenueID, this.PersonID,
            //    this.RevenueName, this.RevenueDescription, this.isActive);


            return clsRevenueData.UpdateRevenue(this.RevenueID,
                this.RevenueName, this.RevenueDescription);

        }

        /// <summary>
        /// Find Revenue info by <paramref name="RevenueID"/> and returns the result.
        /// </summary>
        /// <param name="RevenueID">The Revenue ID for find Revenue info.</param>
        /// <returns>If Revenue find successfully return Revenue <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsRevenue FindByRevenueID(int RevenueID)
        {

            string RevenueName = "", RevenueDescription = "";

            if (clsRevenueData.GetRevenueInfoByID(RevenueID,
                ref RevenueName, ref RevenueDescription))

                return new clsRevenue(RevenueID, RevenueName,
                    RevenueDescription);
            else
                return null!;

        }

        /// <summary>
        /// Find Revenue info by <paramref name="RevenueName"/> and returns the result.
        /// </summary>
        /// <param name="RevenueName">The Revenue Name for find Revenue info.</param>
        /// <returns>If Revenue find successfully return Revenue <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsRevenue FindByRevenueName(string RevenueName)
        {

            int RevenueID = -1;
            string RevenueDescription = "";

            if (clsRevenueData.GetRevenueInfoByRevenueName(RevenueName, ref RevenueID, ref RevenueDescription))

                return new clsRevenue(RevenueID, RevenueName,
                    RevenueDescription);
            else
                return null!;

        }

        /// <summary>
        /// If Mode add new so will add new Revenue and returns the result, 
        /// if Mode update so will Save Revenue changes and returns the result.
        /// </summary>
        /// <returns>If Revenue save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {

            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewRevenue())
                    {

                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateRevenue();

            }

            return false;
        }

        /// <summary>
        /// This function will get all Revenues from database. 
        /// </summary>
        /// <returns>All Revenues info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetAllRevenues()
        {
            return clsRevenueData.GetAllRevenues();
        }

        /// <summary>
        /// This function will delete Revenue from database if <paramref name="RevenueID"/> is exists and returns the result.
        /// </summary>
        /// <param name="RevenueID">The Revenue ID for delete Revenue info.</param>
        /// <returns>If Revenue deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteRevenue(int RevenueID)
        {
            return clsRevenueData.DeleteRevenue(RevenueID);
        }


        /// <summary>
        /// This function will check if Revenue exists into database by <paramref name="RevenueID"/> and returns the result, 
        /// </summary>
        /// <param name="RevenueID">The Revenue ID for check Revenue is exists.</param>
        /// <returns>If Revenueexists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isRevenueExist(int RevenueID)
        {
            return clsRevenueData.IsRevenueExist(RevenueID);
        }

        /// <summary>
        /// This function will check if Revenue exists into database by <paramref name="RevenueName"/> and returns the result, 
        /// </summary>
        /// <param name="RevenueName">The Revenue Name for check Revenue is exists.</param>
        /// <returns>If Revenueexists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isRevenueExist(string RevenueName)
        {
            return clsRevenueData.IsRevenueExist(RevenueName);
        }

    }
}
