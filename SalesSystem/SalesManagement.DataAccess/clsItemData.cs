using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SalesManagement.DataAccess
{
    public static class clsItemData
    {
        public static bool GetItemInfoByID(int ItemID, ref int? CompanyID, ref int CategoryID, ref int StoreID,
            ref int UnitID, ref string ItemName, ref int MinimumQuantity, ref bool IsActiveSale,
            ref bool IsActiveBuy, ref bool PreventFraction, ref bool PreventDiscount, ref float BuyPrice,
            ref float SalePrice,ref string ItemCode, ref string? ImagePath)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetItemInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ItemID", ItemID);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                CategoryID = (int)reader["CategoryID"];
                                StoreID = (int)reader["StoreID"];
                                UnitID = (int)reader["UnitID"];
                                ItemName = (string)reader["ItemName"];
                                MinimumQuantity = (int)reader["MinimumQuantity"];
                                IsActiveSale = (bool)reader["IsActiveSale"];
                                IsActiveBuy = (bool)reader["IsActiveBuy"];
                                PreventFraction = (bool)reader["PreventFraction"];
                                PreventDiscount = (bool)reader["PreventDiscount"];
                                BuyPrice = Convert.ToSingle(reader["BuyPrice"]);
                                SalePrice = Convert.ToSingle(reader["SalePrice"]);
                                ItemCode = (string)reader["ItemCode"];

                                //ImagePath: allows null in database so we should handle null
                                if (reader["ImagePath"] != DBNull.Value)
                                {
                                    ImagePath = (string)reader["ImagePath"];
                                }
                                else
                                {
                                    ImagePath = "";
                                }

                                if (reader["CompanyID"] != DBNull.Value)
                                {
                                    CompanyID = (int)reader["CompanyID"];
                                }
                                else
                                {
                                    BuyPrice = -1;
                                }

                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }

                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
                isFound = false;
            }

            return isFound;
        }

        public static bool GetItemInfoByCompanyID(int? CompanyID, ref int ItemID, ref int CategoryID, ref int StoreID,
            ref int UnitID, ref string ItemName, ref int MinimumQuantity, ref bool IsActiveSale,
            ref bool IsActiveBuy, ref bool PreventFraction, ref bool PreventDiscount, ref float BuyPrice,
            ref float SalePrice, ref string ItemCode, ref string? ImagePath)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetItemInfoByCompanyID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure; 
                        command.Parameters.AddWithValue("@CompanyID", CompanyID);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                ItemID = (int)reader["ItemID"];
                                CategoryID = (int)reader["CategoryID"];
                                StoreID = (int)reader["StoreID"];
                                UnitID = (int)reader["UnitID"];
                                ItemName = (string)reader["ItemName"];
                                MinimumQuantity = (int)reader["MinimumQuantity"];
                                IsActiveSale = (bool)reader["IsActiveSale"];
                                IsActiveBuy = (bool)reader["IsActiveBuy"];
                                PreventFraction = (bool)reader["PreventFraction"];
                                PreventDiscount = (bool)reader["PreventDiscount"];
                                BuyPrice = Convert.ToSingle(reader["BuyPrice"]);
                                SalePrice = Convert.ToSingle(reader["SalePrice"]);
                                ItemCode = (string)reader["ItemCode"];

                                //ImagePath: allows null in database so we should handle null
                                if (reader["ImagePath"] != DBNull.Value)
                                {
                                    ImagePath = (string)reader["ImagePath"];
                                }
                                else
                                {
                                    ImagePath = "";
                                }

                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }

                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
                isFound = false;
            }

            return isFound;
        }

        //public static bool GetItemInfoByFullName(string FullName, ref int? CompanyID, ref int ID, ref int StoreID,
        //    ref int UnitID, ref string ItemName, ref int MinimumQuantity, ref bool IsActiveSale,
        //    ref bool IsActiveBuy, ref bool PreventFraction, ref bool PreventDiscount, ref float BuyPrice,
        //    ref float SalePrice, ref string ItemCode, ref string ImagePath)
        //        {

        //            bool isFound = false;

        //            try
        //            {

        //                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        //                {

        //                    using (SqlCommand command = new SqlCommand("SP_GetItemInfoByFullName", connection))
        //                    {
        //                        command.CommandType = CommandType.StoredProcedure;
        //                        command.Parameters.AddWithValue("@FullName", FullName);


        //                        connection.Open();
        //                        using (SqlDataReader reader = command.ExecuteReader())
        //                        {
        //                            if (reader.Read())
        //                            {
        //                                // The record was found
        //                                isFound = true;

        //                                ID = (int)reader["ItemID"];
        //                                CompanyID = (int)reader["CompanyID"];
        //                                StoreID = (int)reader["StoreID"];
        //                                UnitID = (int)reader["UnitID"];
        //                                MinimumQuantity = (int)reader["MinimumQuantity"];
        //                                IsActiveSale = (bool)reader["IsActiveSale"];
        //                                IsActiveBuy = (bool)reader["IsActiveBuy"];
        //                                PreventFraction = (bool)reader["PreventFraction"];
        //                                PreventDiscount = (bool)reader["PreventDiscount"];
        //                                SalePrice = Convert.ToSingle(reader["SalePrice"]);
        //                                ItemCode = (string)reader["ItemCode"];

        //                                //ImagePath: allows null in database so we should handle null
        //                                if (reader["ImagePath"] != DBNull.Value)
        //                                {
        //                                    ImagePath = (string)reader["ImagePath"];
        //                                }
        //                                else
        //                                {
        //                                    ImagePath = "";
        //                                }

        //                                if (reader["BuyPrice"] != DBNull.Value)
        //                                {
        //                                    BuyPrice = Convert.ToSingle(reader["BuyPrice"]);
        //                                }
        //                                else
        //                                {
        //                                    BuyPrice = "";
        //                                }

        //                                if (reader["ItemName"] != DBNull.Value)
        //                                {
        //                                    ItemName = (string)reader["ItemName"];
        //                                }
        //                                else
        //                                {
        //                                    ItemName = "";
        //                                }

        //                            }
        //                            else
        //                            {
        //                                // The record was not found
        //                                isFound = false;
        //                            }

        //                        }
        //                    }
        //                }

        //            }
        //            catch (SqlException ex)
        //            {
        //                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
        //                isFound = false;
        //            }

        //            return isFound;
        //        }

        public static int AddNewItem(int? CompanyID,int CategoryID, int StoreID,
            int UnitID, string ItemName, int MinimumQuantity, bool IsActiveSale,
            bool IsActiveBuy, bool PreventFraction, bool PreventDiscount, float BuyPrice,
            float SalePrice, string ItemCode, string? ImagePath)
        {
            //this function will return the new Item id if succeeded and -1 if not.
            int NewItemID = -1;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_AddNewItem", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StoreID", StoreID);
                        command.Parameters.AddWithValue("@CategoryID", CategoryID);
                        command.Parameters.AddWithValue("@UnitID", UnitID);
                        command.Parameters.AddWithValue("@ItemName", ItemName);
                        command.Parameters.AddWithValue("@MinimumQuantity", MinimumQuantity);
                        command.Parameters.AddWithValue("@IsActiveSale", IsActiveSale);
                        command.Parameters.AddWithValue("@IsActiveBuy", IsActiveBuy);
                        command.Parameters.AddWithValue("@PreventFraction", PreventFraction);
                        command.Parameters.AddWithValue("@PreventDiscount", PreventDiscount);
                        command.Parameters.AddWithValue("@BuyPrice", BuyPrice);
                        command.Parameters.AddWithValue("@SalePrice", SalePrice);
                        command.Parameters.AddWithValue("@ItemCode", ItemCode);

                        if (ImagePath != "" && ImagePath != null)
                            command.Parameters.AddWithValue("@ImagePath", ImagePath);
                        else
                            command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

                        if (CompanyID != -1 && CompanyID != null) 
                            command.Parameters.AddWithValue("@CompanyID", CompanyID);
                        else
                            command.Parameters.AddWithValue("@CompanyID", System.DBNull.Value);


                        SqlParameter outputParam = new SqlParameter("@NewItemID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        object result = command.Parameters["@NewItemID"].Value;
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            NewItemID = insertedID;
                        }
                    }
                }

            }

            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
            }

            return NewItemID;
        }

        public static bool UpdateItem(int ID, int? CompanyID, int CategoryID, int StoreID,
            int UnitID, string ItemName, int MinimumQuantity, bool IsActiveSale,
            bool IsActiveBuy, bool PreventFraction, bool PreventDiscount, float BuyPrice,
            float SalePrice, string ItemCode, string? ImagePath)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_UpdateItem", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ItemID", ID); 
                        command.Parameters.AddWithValue("@CategoryID", CategoryID);
                        command.Parameters.AddWithValue("@StoreID", StoreID);
                        command.Parameters.AddWithValue("@UnitID", UnitID);
                        command.Parameters.AddWithValue("@ItemName", ItemName);
                        command.Parameters.AddWithValue("@MinimumQuantity", MinimumQuantity);
                        command.Parameters.AddWithValue("@IsActiveSale", IsActiveSale);
                        command.Parameters.AddWithValue("@IsActiveBuy", IsActiveBuy);
                        command.Parameters.AddWithValue("@PreventFraction", PreventFraction);
                        command.Parameters.AddWithValue("@PreventDiscount", PreventDiscount);
                        command.Parameters.AddWithValue("@BuyPrice", BuyPrice);
                        command.Parameters.AddWithValue("@SalePrice", SalePrice);
                        command.Parameters.AddWithValue("@ItemCode", ItemCode);

                        if (ImagePath != "" && ImagePath != null)
                            command.Parameters.AddWithValue("@ImagePath", ImagePath);
                        else
                            command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

                        if (CompanyID != -1 && CompanyID != null) 
                            command.Parameters.AddWithValue("@CompanyID", CompanyID);
                        else
                            command.Parameters.AddWithValue("@CompanyID", System.DBNull.Value);



                        connection.Open();
                        rowsAffected = (int)command.ExecuteScalar();

                    }
                }

            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
                return false;
            }

            return (rowsAffected > 0);

        }

        public static DataView GetAllItemsAsync()
        {

            DataTable dt = new DataTable();

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetAllItems", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)

                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }

            }

            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
            }

            return dt.DefaultView;

        }

        public static bool DeleteItem(int ItemID)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_DeleteItem", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ItemID", ItemID);


                        connection.Open();
                        rowsAffected = (int)command.ExecuteScalar();

                    }
                }

            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
            }

            return (rowsAffected > 0);

        }
        
        public static bool IsItemExist(int ItemID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsItemExistByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ItemID", ItemID);

                        SqlParameter returnParameter = new SqlParameter("@ReturnVal", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue,
                        };
                        command.Parameters.Add(returnParameter);

                        connection.Open();
                        command.ExecuteNonQuery();

                        isFound = ( (int)returnParameter.Value == 1 );
                    }
                }

            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
                isFound = false;
            }

            return isFound;
        }

        public static bool IsItemExist(int? CompanyID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsItemExistByCompanyID", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CompanyID", CompanyID);

                        SqlParameter returnParameter = new SqlParameter("@ReturnVal", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue,
                        };
                        command.Parameters.Add(returnParameter);

                        connection.Open();
                        command.ExecuteNonQuery();

                        isFound = ((int)returnParameter.Value == 1);
                    }
                }

            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
                isFound = false;
            }

            return isFound;
        }

    }


}
