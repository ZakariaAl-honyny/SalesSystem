using SalesManagement.DataAccess;
using System.Data;

namespace SalesManagement.Busniess
{
    public class clsUnit
    {

        public enum enMode { AddNew = 0, Update = 1 };
        protected enMode _Mode = enMode.AddNew;

        public int UnitID { set; get; }
        //public int PersonID { set; get; }
        //public clsPerson PersonInfo;

        public string UnitName { set; get; }

        public string UnitDescription { set; get; }
        public clsUnit()

        {
            this.UnitID = -1;
            this.UnitName = "";
            this.UnitDescription = "";

            _Mode = enMode.AddNew;

        }

        private clsUnit(int UnitID, string UnitName,
            string UnitDescription)

        {
            this.UnitID = UnitID;
            //this.PersonID = PersonID;
            //PersonInfo = clsPerson.Find(PersonID);
            this.UnitName = UnitName;
            this.UnitDescription = UnitDescription;

            _Mode = enMode.Update;

        }

        /// <summary>
        /// Add new Unit and returns the result.
        /// </summary>
        /// <returns>If Unit added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewUnit()
        {
            //call DataAccess Layer 

            this.UnitID = clsUnitData.AddNewUnit(this.UnitName,
                this.UnitDescription);

            return (this.UnitID != -1);
        }

        /// <summary>
        /// Update Unit info and returns the result.
        /// </summary>
        /// <returns>If Unit updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateUnit()
        {
            //call DataAccess Layer 

            //return clsUnitData.UpdateUnit(this.UnitID, this.PersonID,
            //    this.UnitName, this.UnitDescription, this.isActive);


            return clsUnitData.UpdateUnit(this.UnitID,
                this.UnitName, this.UnitDescription);

        }

        /// <summary>
        /// Find Unit info by <paramref name="UnitID"/> and returns the result.
        /// </summary>
        /// <param name="UnitID">The Unit ID for find Unit info.</param>
        /// <returns>If Unit find successfully return Unit <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsUnit FindByUnitID(int UnitID)
        {

            string UnitName = "", UnitDescription = "";

            if (clsUnitData.GetUnitInfoByID(UnitID,
                ref UnitName, ref UnitDescription))

                return new clsUnit(UnitID, UnitName,
                    UnitDescription);
            else
                return null!;

        }

        /// <summary>
        /// Find Unit info by <paramref name="UnitName"/> and returns the result.
        /// </summary>
        /// <param name="UnitName">The Unit Name for find Unit info.</param>
        /// <returns>If Unit find successfully return Unit <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsUnit FindByUnitName(string UnitName)
        {

            int UnitID = -1;
            string UnitDescription = "";

            if (clsUnitData.GetUnitInfoByUnitName(UnitName, ref UnitID, ref UnitDescription))

                return new clsUnit(UnitID, UnitName,
                    UnitDescription);
            else
                return null!;

        }

        /// <summary>
        /// If Mode add new so will add new Unit and returns the result, 
        /// if Mode update so will Save Unit changes and returns the result.
        /// </summary>
        /// <returns>If Unit save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {

            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUnit())
                    {

                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateUnit();

            }

            return false;
        }

        /// <summary>
        /// This function will get all Units from database. 
        /// </summary>
        /// <returns>All Units info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetAllUnits()
        {
            return clsUnitData.GetAllUnits();
        }

        /// <summary>
        /// This function will delete Unit from database if <paramref name="UnitID"/> is exists and returns the result.
        /// </summary>
        /// <param name="UnitID">The Unit ID for delete Unit info.</param>
        /// <returns>If Unit deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteUnit(int UnitID)
        {
            return clsUnitData.DeleteUnit(UnitID);
        }


        /// <summary>
        /// This function will check if Unit exists into database by <paramref name="UnitID"/> and returns the result, 
        /// </summary>
        /// <param name="UnitID">The Unit ID for check Unit is exists.</param>
        /// <returns>If Unitexists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isUnitExist(int UnitID)
        {
            return clsUnitData.IsUnitExist(UnitID);
        }

        /// <summary>
        /// This function will check if Unit exists into database by <paramref name="UnitName"/> and returns the result, 
        /// </summary>
        /// <param name="UnitName">The Unit Name for check Unit is exists.</param>
        /// <returns>If Unitexists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isUnitExist(string UnitName)
        {
            return clsUnitData.IsUnitExist(UnitName);
        }

    }
}
