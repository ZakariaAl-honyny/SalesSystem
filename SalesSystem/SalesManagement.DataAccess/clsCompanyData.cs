using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SalesManagement.DataAccess
{
    public static class clsCompanyData
    {
        public static bool GetCompanyInfoByID(int CompanyID, ref string CompanyName, ref string CompanyPhone,
            ref string Address, ref string? OtherDetails)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetCompanyInfoByID", connection))
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

                                CompanyName = (string)reader["CompanyName"];
                                CompanyPhone = (string)reader["CompanyPhone"];
                                Address = (string)reader["Address"];

                                if (reader["OtherDetails"] != DBNull.Value)
                                {
                                    OtherDetails = (string)reader["OtherDetails"];
                                }
                                else
                                {
                                    OtherDetails = "";
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

        public static bool GetCompanyInfoByCompanyName(string CompanyName, ref int ID, ref string CompanyPhone,
            ref string Address, ref string? OtherDetails)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetCompanyInfoByCompanyName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure; 
                        command.Parameters.AddWithValue("@CompanyName", CompanyName);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                ID = (int)reader["CompanyID"];
                                CompanyPhone = (string)reader["CompanyPhone"];
                                Address = (string)reader["Address"];

                                if (reader["OtherDetails"] != DBNull.Value)
                                {
                                    OtherDetails = (string)reader["OtherDetails"];
                                }
                                else
                                {
                                    OtherDetails = "";
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

        public static bool GetCompanyInfoByFullName(string FullName, ref string CompanyName, ref int ID, ref string CompanyPhone,
    ref string Address, ref string? OtherDetails)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetCompanyInfoByFullName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FullName", FullName);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                ID = (int)reader["CompanyID"];
                                CompanyName = (string)reader["CompanyName"];
                                CompanyPhone = (string)reader["CompanyPhone"];
                                Address = (string)reader["Address"];

                                if (reader["OtherDetails"] != DBNull.Value)
                                {
                                    OtherDetails = (string)reader["OtherDetails"];
                                }
                                else
                                {
                                    OtherDetails = "";
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

        public static int AddNewCompany(string CompanyName, string CompanyPhone,
            string Address, string? OtherDetails)
        {
            //this function will return the new Company id if succeeded and -1 if not.
            int NewCompanyID = -1;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_AddNewCompany", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CompanyName", CompanyName);
                        command.Parameters.AddWithValue("@CompanyPhone", CompanyPhone);
                        command.Parameters.AddWithValue("@Address", Address);

                        if (OtherDetails != "" && OtherDetails != null)
                            command.Parameters.AddWithValue("@OtherDetails", OtherDetails);
                        else
                            command.Parameters.AddWithValue("@OtherDetails", System.DBNull.Value);

                        SqlParameter outputParam = new SqlParameter("@NewCompanyID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        object result = command.Parameters["@NewCompanyID"].Value;
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            NewCompanyID = insertedID;
                        }
                    }
                }

            }

            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
            }

            return NewCompanyID;
        }

        public static bool UpdateCompany(int ID, string CompanyName, string CompanyPhone,
            string Address, string? OtherDetails)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_UpdateCompany", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CompanyID", ID);
                        command.Parameters.AddWithValue("@CompanyName", CompanyName);
                        command.Parameters.AddWithValue("@CompanyPhone", CompanyPhone);
                        command.Parameters.AddWithValue("@Address", Address);

                        if (OtherDetails != "" && OtherDetails != null)
                            command.Parameters.AddWithValue("@OtherDetails", OtherDetails);
                        else
                            command.Parameters.AddWithValue("@OtherDetails", System.DBNull.Value);

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

        public static DataView GetAllCompaniesAsync()
        {

            DataTable dt = new DataTable();

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetAllCompanies", connection))
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

        public static bool DeleteCompany(int CompanyID)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_DeleteCompany", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CompanyID", CompanyID);


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
        
        public static bool IsCompanyExist(int CompanyID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsCompanyExistByID", connection))
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

        public static bool IsCompanyExist(string CompanyName)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsCompanyExistByCompanyName", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CompanyName", CompanyName);

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
