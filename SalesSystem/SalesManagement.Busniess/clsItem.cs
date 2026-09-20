using SalesManagement.DataAccess;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SalesManagement.Busniess
{

    /// <summary>
    /// This class represents Item information.
    /// </summary>
    public class clsItem
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ItemID { set; get; }
        public int CategoryID { set; get; }

        public int? CompanyID { set; get; }

        public int StoreID { set; get; }

        public int UnitID { set; get; }

        public string ItemName { set; get; }

        public int MinimumQuantity { set; get; }

        public bool IsActiveSale { set; get; }

        public bool IsActiveBuy { set; get; }

        public bool PreventFraction { set; get; }

        public bool PreventDiscount { set; get; }

        public float BuyPrice { set; get; }
        public float SalePrice { set; get; }
        public string ItemCode { set; get; }


        private string _ImagePath;

        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }


        public clsItem()

        {
            this.ItemID = -1;
            this.CompanyID = -1;
            this.CategoryID = -1;
            this.StoreID = -1;
            this.UnitID = -1;
            this.ItemName = "";
            this.MinimumQuantity = 0;
            this.IsActiveSale = false;
            this.IsActiveBuy = false;
            this.PreventFraction = true;
            this.PreventDiscount = true;
            this.BuyPrice = 0;
            this.SalePrice = 0;
            this.ItemCode = "";
            this.ImagePath = "";

            Mode = enMode.AddNew;

        }

        private clsItem(int ItemID, int? CompanyID,int CategoryID, int StoreID, int UnitID,
            string ItemName, int MinimumQuantity, bool IsActiveSale, bool IsActiveBuy,
            bool PreventFraction, bool PreventDiscount, float BuyPrice, float SalePrice, string ItemCode, string? ImagePath)

        {
            this.ItemID = ItemID;
            this.CompanyID = CompanyID;
            this.CategoryID = CategoryID;
            this.StoreID = StoreID;
            this.UnitID = UnitID;
            this.ItemName = ItemName;
            this.MinimumQuantity = MinimumQuantity;
            this.IsActiveSale = IsActiveSale;
            this.IsActiveBuy = IsActiveBuy;
            this.PreventFraction = PreventFraction;
            this.PreventDiscount = PreventDiscount;
            this.BuyPrice = BuyPrice;
            this.SalePrice = SalePrice;
            this.ItemCode = ItemCode;
            this.ImagePath = ImagePath;

            Mode = enMode.Update;

        }

        /// <summary>
        /// Add new Item and returns the result.
        /// </summary>
        /// <returns>If Item added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewItem()
        {
            //call DataAccess Layer 

            this.ItemID = clsItemData.AddNewItem(this.CompanyID,this.CategoryID, this.StoreID, this.UnitID,
                this.ItemName, this.MinimumQuantity, this.IsActiveSale, this.IsActiveBuy, this.PreventFraction,
                this.PreventDiscount, this.BuyPrice,this.SalePrice, this.ItemCode, this.ImagePath);

            return (this.ItemID != -1);
        }

        /// <summary>
        /// Update Item info and returns the result.
        /// </summary>
        /// <returns>If Item updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateItem()
        {
            //call DataAccess Layer 

            return clsItemData.UpdateItem(this.ItemID, this.CompanyID, this.CategoryID, this.StoreID, this.UnitID,
                this.ItemName, this.MinimumQuantity, this.IsActiveSale,this.IsActiveBuy, this.PreventFraction,
                this.PreventDiscount, this.BuyPrice, this.SalePrice, this.ItemCode, this.ImagePath);

        }

        /// <summary>
        /// Find Item info by <paramref name="ItemID"/> and returns the result.
        /// </summary>
        /// <param name="ItemID">The Item id for find Item info.</param>
        /// <returns>If Item find successfully return Item <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsItem Find(int ItemID)
        {

            string ItemName = "", ItemCode = "";
            string? ImagePath = "";
            int CategoryID = -1, StoreID = -1, UnitID = -1, MinimumQuantity = 0;
            int? CompanyID = -1;
            bool IsActiveBuy = false, IsActiveSale = false, PreventFraction = false, PreventDiscount = false;
            float BuyPrice = 0, SalePrice = 0;

            if (clsItemData.GetItemInfoByID(ItemID, ref CompanyID, ref CategoryID,ref StoreID, ref UnitID,
                ref ItemName, ref MinimumQuantity, ref IsActiveSale, ref IsActiveBuy, ref PreventFraction, ref PreventDiscount,
                ref BuyPrice, ref SalePrice, ref ItemCode, ref ImagePath))

                return new clsItem(ItemID, CompanyID, CategoryID, StoreID, UnitID,
                    ItemName, MinimumQuantity, IsActiveSale, IsActiveBuy, PreventFraction, PreventDiscount,
                    BuyPrice, SalePrice, ItemCode, ImagePath);
            else
                return null!;

        }

        /// <summary>
        /// Find Item info by <paramref name="CompanyID"/> and returns the result.
        /// </summary>
        /// <param name="CompanyID">The National No for find Item info.</param>
        /// <returns>If Item find successfully return Item <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        public static clsItem Find(int? CompanyID)
        {

            string ItemName = "", ItemCode = "";
            string? ImagePath = "";
            int CategoryID = -1,StoreID = -1, UnitID = -1, MinimumQuantity = 0;
            int ItemID = -1;
            bool IsActiveBuy = false, IsActiveSale = false, PreventFraction = false, PreventDiscount = false;
            float BuyPrice = 0, SalePrice = 0;

            if (clsItemData.GetItemInfoByCompanyID(CompanyID, ref ItemID , ref CategoryID, ref StoreID, ref UnitID,
                ref ItemName, ref MinimumQuantity, ref IsActiveSale, ref IsActiveBuy, ref PreventFraction, ref PreventDiscount,
                ref BuyPrice, ref SalePrice, ref ItemCode, ref ImagePath))

                return new clsItem(ItemID, CompanyID, CategoryID, StoreID, UnitID,
                    ItemName, MinimumQuantity, IsActiveSale, IsActiveBuy, PreventFraction, PreventDiscount,
                    BuyPrice, SalePrice, ItemCode, ImagePath);
            else
                return null!;

        }

        /// <summary>
        /// Find Item info by <paramref name="FullName"/> and returns the result.
        /// </summary>
        /// <param name="FullName">The National No for find Item info.</param>
        /// <returns>If Item find successfully return Item <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
        //public static clsItem FindByFullName(string FullName)
        //{

        //    int ItemID = -1;
        //    string ItemName = "", PreventDiscount = "", ImagePath = "";
        //    int? CompanyID = "", StoreID = "", UnitID = "", MinimumQuantity = "", PreventFraction = "", IsActiveBuy = "";
        //    bool IsActiveSale = DateTime.Now;
        //    float BuyPrice = -1;
        //    byte Gender = (byte)clsItem.enGender.Male;

        //    if (clsItemData.GetItemInfoByFullName(FullName, ref CompanyID, ref ItemID, ref StoreID, ref UnitID,
        //        ref ItemName, ref MinimumQuantity, ref IsActiveSale, ref Gender, ref IsActiveBuy, ref PreventFraction, ref PreventDiscount,
        //        ref BuyPrice, ref ImagePath))

        //        return new clsItem(ItemID, CompanyID, StoreID, UnitID,
        //            ItemName, MinimumQuantity, IsActiveSale, (clsItem.enGender)Gender, IsActiveBuy, PreventFraction, PreventDiscount,
        //            BuyPrice, ImagePath);
        //    else
        //        return null!;

        //}

        /// <summary>
        /// If Mode add new so will add new Item and returns the result, 
        /// if Mode update so will Save Item changes and returns the result.
        /// </summary>
        /// <returns>If Item save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewItem())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateItem();

            }




            return false;
        }

        /// <summary>
        /// This function will get all Items from database. 
        /// </summary>
        /// <returns>All Items info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetAllItems()
        {
            return  clsItemData.GetAllItemsAsync();
        }

        /// <summary>
        /// This function will delete Item from database if <paramref name="ItemID"/> is exists and returns the result.
        /// </summary>
        /// <param name="ItemID">The Item ID for delete Item info.</param>
        /// <returns>If Item deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteItem(int ItemID)
        {
            return clsItemData.DeleteItem(ItemID);
        }

        /// <summary>
        /// This function will check if Item exists into database by <paramref name="ItemID"/> and returns the result, 
        /// </summary>
        /// <param name="ItemID">The Item ID for check Item is exists.</param>
        /// <returns>If Item exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isItemExist(int ItemID)
        {
            return clsItemData.IsItemExist(ItemID);
        }

        ///// <summary>
        ///// This function will check if Item exists in database by <paramref name="CompanyID"/> and returns the result, 
        ///// </summary>
        ///// <param name="CompanyID">The National No for check Item is exists.</param>
        ///// <returns>If Item exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isItemExist(int? CompanyID)
        {
            return clsItemData.IsItemExist(CompanyID);
        }

    }
}
