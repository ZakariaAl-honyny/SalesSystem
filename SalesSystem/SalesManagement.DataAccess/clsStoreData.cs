using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SalesManagement.DataAccess
{
    public static class clsStoreData
    {
        public static bool GetStoreInfoByID(int StoreID, ref string StoreName, ref string StoreAddress,
            ref string? StorePhone, ref string? StoreDetails, ref int? StoreManagerID)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetStoreInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StoreID", StoreID);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                StoreName = (string)reader["StoreName"];
                                StoreAddress = (string)reader["StoreAddress"];

                                //StorePhone: allows null in database so we should handle null
                                if (reader["StorePhone"] != DBNull.Value)
                                {
                                    StorePhone = (string)reader["StorePhone"];
                                }
                                else
                                {
                                    StorePhone = "";
                                }

                                if (reader["StoreManagerID"] != DBNull.Value)
                                {
                                    StoreManagerID = (int)reader["StoreManagerID"];
                                }
                                else
                                {
                                    StoreManagerID = -1;
                                }

                                if (reader["StoreDetails"] != DBNull.Value)
                                {
                                    StoreDetails = (string)reader["StoreDetails"];
                                }
                                else
                                {
                                    StoreDetails = "";
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

        public static bool GetStoreInfoByStoreName(string StoreName, ref int StoreID , ref string StoreAddress,
            ref string? StorePhone, ref string? StoreDetails, ref int? StoreManagerID)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetStoreInfoByStoreName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure; 
                        command.Parameters.AddWithValue("@StoreName", StoreName);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                StoreID = (int)reader["StoreID"];
                                StoreAddress = (string)reader["StoreAddress"];

                                //StorePhone: allows null in database so we should handle null
                                if (reader["StorePhone"] != DBNull.Value)
                                {
                                    StorePhone = (string)reader["StorePhone"];
                                }
                                else
                                {
                                    StorePhone = "";
                                }

                                if (reader["StoreManagerID"] != DBNull.Value)
                                {
                                    StoreManagerID = (int)reader["StoreManagerID"];
                                }
                                else
                                {
                                    StoreManagerID = -1;
                                }

                                if (reader["StoreDetails"] != DBNull.Value)
                                {
                                    StoreDetails = (string)reader["StoreDetails"];
                                }
                                else
                                {
                                    StoreDetails = "";
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

        public static int AddNewStore(string StoreName, string StoreAddress,
            string? StorePhone, string? StoreDetails, int? StoreManagerID)
        {
            //this function will return the new Store id if succeeded and -1 if not.
            int NewStoreID = -1;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_AddNewStore", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StoreName", StoreName);
                        command.Parameters.AddWithValue("@StoreAddress", StoreAddress);

                        if (StorePhone != "" && StorePhone != null)
                            command.Parameters.AddWithValue("@StorePhone", StorePhone);
                        else
                            command.Parameters.AddWithValue("@StorePhone", System.DBNull.Value);

                        if (StoreDetails != "" && StoreDetails != null)
                            command.Parameters.AddWithValue("@StoreDetails", StoreDetails);
                        else
                            command.Parameters.AddWithValue("@StoreDetails", System.DBNull.Value);

                        if (StoreManagerID != -1 && StoreManagerID != null)
                            command.Parameters.AddWithValue("@StoreManagerID", StoreManagerID);
                        else
                            command.Parameters.AddWithValue("@StoreManagerID", System.DBNull.Value);

                        SqlParameter outputParam = new SqlParameter("@NewStoreID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        object result = command.Parameters["@NewStoreID"].Value;
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            NewStoreID = insertedID;
                        }
                    }
                }

            }

            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
            }

            return NewStoreID;
        }

        public static bool UpdateStore(int ID, string StoreName, string StoreAddress,
            string? StorePhone, string? StoreDetails, int? StoreManagerID)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_UpdateStore", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StoreID", ID);
                        command.Parameters.AddWithValue("@StoreName", StoreName);
                        command.Parameters.AddWithValue("@StoreAddress", StoreAddress);

                        if (StorePhone != "" && StorePhone != null)
                            command.Parameters.AddWithValue("@StorePhone", StorePhone);
                        else
                            command.Parameters.AddWithValue("@StorePhone", System.DBNull.Value);

                        if (StoreDetails != "" && StoreDetails != null)
                            command.Parameters.AddWithValue("@StoreDetails", StoreDetails);
                        else
                            command.Parameters.AddWithValue("@StoreDetails", System.DBNull.Value);

                        if (StoreManagerID != -1 && StoreManagerID != null)
                            command.Parameters.AddWithValue("@StoreManagerID", StoreManagerID);
                        else
                            command.Parameters.AddWithValue("@StoreManagerID", System.DBNull.Value);



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

        public static DataView GetAllStoresAsync()
        {

            DataTable dt = new DataTable();

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetAllStores", connection))
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

        public static bool DeleteStore(int StoreID)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_DeleteStore", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StoreID", StoreID);


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
        
        public static bool IsStoreExist(int StoreID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsStoreExistByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StoreID", StoreID);

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

        public static bool IsStoreExist(string StoreName)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsStoreExistByStoreName", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StoreName", StoreName);

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
