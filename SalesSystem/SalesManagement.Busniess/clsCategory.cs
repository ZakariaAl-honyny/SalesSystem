using SalesManagement.DataAccess;
using System.Data;

namespace SalesManagement.Busniess
{
    public class clsCategory
    {

        public enum enMode { AddNew = 0, Update = 1 };
        protected enMode _Mode = enMode.AddNew;

        public int CategoryID { set; get; }
        //public int PersonID { set; get; }
        //public clsPerson PersonInfo;

        public string CategoryName { set; get; }

        public string CategoryDescription { set; get; }
        public clsCategory()

        {
            this.CategoryID = -1;
            this.CategoryName = "";
            this.CategoryDescription = "";

            _Mode = enMode.AddNew;

        }

        private clsCategory(int CategoryID, string CategoryName,
            string CategoryDescription)

        {
            this.CategoryID = CategoryID;
            //this.PersonID = PersonID;
            //PersonInfo = clsPerson.Find(PersonID);
            this.CategoryName = CategoryName;
            this.CategoryDescription = CategoryDescription;

            _Mode = enMode.Update;

        }

        /// <summary>
        /// Add new Category and returns the result.
        /// </summary>
        /// <returns>If Category added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewCategory()
        {
            //call DataAccess Layer 

            this.CategoryID = clsCategoryData.AddNewCategory(this.CategoryName,
                this.CategoryDescription);

            return (this.CategoryID != -1);
        }

        /// <summary>
        /// Update Category info and returns the result.
        /// </summary>
        /// <returns>If Category updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateCategory()
        {
            //call DataAccess Layer 

            //return clsCategoryData.UpdateCategory(this.CategoryID, this.PersonID,
            //    this.CategoryName, this.CategoryDescription, this.isActive);


            return clsCategoryData.UpdateCategory(this.CategoryID,
                this.CategoryName, this.CategoryDescription);

        }

        /// <summary>
        /// Find Category info by <paramref name="CategoryID"/> and returns the result.
        /// </summary>
        /// <param name="CategoryID">The Category ID for find Category info.</param>
        /// <returns>If Category find successfully return Category <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsCategory FindByCategoryID(int CategoryID)
        {

            string CategoryName = "", CategoryDescription = "";

            if (clsCategoryData.GetCategoryInfoByID(CategoryID,
                ref CategoryName, ref CategoryDescription))

                return new clsCategory(CategoryID, CategoryName,
                    CategoryDescription);
            else
                return null!;

        }

        /// <summary>
        /// Find Category info by <paramref name="CategoryName"/> and returns the result.
        /// </summary>
        /// <param name="CategoryName">The Category Name for find Category info.</param>
        /// <returns>If Category find successfully return Category <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsCategory FindByCategoryName(string CategoryName)
        {

            int CategoryID = -1;
            string CategoryDescription = "";

            if (clsCategoryData.GetCategoryInfoByCategoryName(CategoryName, ref CategoryID, ref CategoryDescription))

                return new clsCategory(CategoryID, CategoryName,
                    CategoryDescription);
            else
                return null!;

        }

        /// <summary>
        /// If Mode add new so will add new Category and returns the result, 
        /// if Mode update so will Save Category changes and returns the result.
        /// </summary>
        /// <returns>If Category save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {

            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCategory())
                    {

                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateCategory();

            }

            return false;
        }

        /// <summary>
        /// This function will get all Categorys from database. 
        /// </summary>
        /// <returns>All Categorys info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetAllCategories()
        {
            return clsCategoryData.GetAllCategories();
        }

        /// <summary>
        /// This function will delete Category from database if <paramref name="CategoryID"/> is exists and returns the result.
        /// </summary>
        /// <param name="CategoryID">The Category ID for delete Category info.</param>
        /// <returns>If Category deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteCategory(int CategoryID)
        {
            return clsCategoryData.DeleteCategory(CategoryID);
        }


        /// <summary>
        /// This function will check if Category exists into database by <paramref name="CategoryID"/> and returns the result, 
        /// </summary>
        /// <param name="CategoryID">The Category ID for check Category is exists.</param>
        /// <returns>If Categoryexists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isCategoryExist(int CategoryID)
        {
            return clsCategoryData.IsCategoryExist(CategoryID);
        }

        /// <summary>
        /// This function will check if Category exists into database by <paramref name="CategoryName"/> and returns the result, 
        /// </summary>
        /// <param name="CategoryName">The Category Name for check Category is exists.</param>
        /// <returns>If Categoryexists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isCategoryExist(string CategoryName)
        {
            return clsCategoryData.IsCategoryExist(CategoryName);
        }

    }
}
