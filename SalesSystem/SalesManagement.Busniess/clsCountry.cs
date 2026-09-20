using SalesManagement.DataAccess;
using System.Data;

namespace SalesManagement.Busniess
{
    /// <summary>
    /// This class represents Country information.
    /// </summary>
    public class clsCountry
    {

        public int CountryID { set; get; }
        public string CountryName { set; get; }


        public clsCountry()

        {
            this.CountryID = -1;
            this.CountryName = "";

        }

        private clsCountry(int CountryID, string CountryName)

        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }

        /// <summary>
        /// Find Country info by <paramref name="CountryID"/> and returns the result.
        /// </summary>
        /// <param name="CountryID">The Country ID for find Country info.</param>
        /// <returns>If Country find successfully return Country <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsCountry Find(int CountryID)
        {

            string CountryName = "";

            if (clsCountryData.GetCountryInfoByID(CountryID, ref CountryName))

                return new clsCountry(CountryID, CountryName);
            else
                return null;

        }

        /// <summary>
        /// Find Country info by Country Name and returns the result.
        /// </summary>
        /// <param name="CountryName">The Country Name for find Country info.</param>
        /// <returns>If Country find successfully return Country <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsCountry Find(string CountryName)
        {

            int CountryID = -1;

            if (clsCountryData.GetCountryInfoByName(CountryName, ref CountryID))

                return new clsCountry(CountryID, CountryName);
            else
                return null;

        }


        /// <summary>
        /// This function will get all Countries from database. 
        /// </summary>
        /// <returns>All Countries info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetAllCountries()
        {
            return clsCountryData.GetAllCountries();

        }

        /// <summary>
        /// This function will check if Country exists into database by Country ID and returns the result, 
        /// </summary>
        /// <param name="CountryID">The Country ID for check Country is exists.</param>
        /// <returns>If Country exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isCountryExist(int CountryID)
        {
            return clsCountryData.IsCountryExist(CountryID);
        }

        /// <summary>
        /// This function will check if Country exists into database by <paramref name="CountryName"/> and returns the result, 
        /// </summary>
        /// <param name="CountryName">The Country Name for check Country is exists.</param>
        /// <returns>If Country exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isCountryExist(string CountryName)
        {
            return clsCountryData.IsCountryExist(CountryName);
        }

    }

}
