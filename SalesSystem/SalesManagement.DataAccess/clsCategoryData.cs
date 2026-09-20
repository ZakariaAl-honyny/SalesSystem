using Microsoft.Data.SqlClient;
using System.Data;

namespace SalesManagement.DataAccess
{
    public static class clsCategoryData
    {
        public static bool GetCategoryInfoByID(int CategoryID, ref string CategoryName,
            ref string CategoryDescription)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetCategoryInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CategoryID", CategoryID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                CategoryName = (string)reader["CategoryName"];

                                // allows null failed in database so we should handle null
                                if (reader["CategoryDescription"] != DBNull.Value) 
                                    CategoryDescription = (string)reader["CategoryDescription"];
                                else
                                    CategoryDescription = "";

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
        public static bool GetCategoryInfoByCategoryName(string CategoryName, ref int CategoryID,
            ref string CategoryDescription)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetCategoryInfoByCategoryName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CategoryName", CategoryName);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                CategoryID = (int)reader["CategoryID"];

                                // allows null failed in database so we should handle null
                                if (reader["CategoryDescription"] != DBNull.Value)
                                    CategoryDescription = (string)reader["CategoryDescription"];
                                else
                                    CategoryDescription = "";


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

        public static int AddNewCategory(string CategoryName,
            string CategoryDescription)
        {
            //this function will return the new Category id if succeeded and -1 if not.
            int NewCategoryID = -1;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        if (CategoryDescription != "" && CategoryDescription != null)
                            command.Parameters.AddWithValue("@CategoryDescription", CategoryDescription);
                        else
                            command.Parameters.AddWithValue("@CategoryDescription", DBNull.Value);


                        SqlParameter outputParam = new SqlParameter("@NewCategoryID", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                        command.Parameters.Add(outputParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        object result = command.Parameters["@NewCategoryID"].Value;
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            NewCategoryID = insertedID;
                        }
                    }
                }
            }

            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
            }

            return NewCategoryID;
        }

        public static bool UpdateCategory(int CategoryID, string CategoryName,
            string CategoryDescription)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CategoryID", CategoryID);
                        command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        if (CategoryDescription != "" && CategoryDescription != null)
                            command.Parameters.AddWithValue("@CategoryDescription", CategoryDescription);
                        else
                            command.Parameters.AddWithValue("@CategoryDescription", DBNull.Value);

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

        public static DataView GetAllCategories()
        {

            DataTable dt = new DataTable();

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllCategories", connection))
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

        public static bool DeleteCategory(int CategoryID)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CategoryID", CategoryID);

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

        public static bool IsCategoryExist(int CategoryID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsCategoryExistByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CategoryID", CategoryID);

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
        public static bool IsCategoryExist(string CategoryName)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsCategoryExistByCategoryName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CategoryName", CategoryName);

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
