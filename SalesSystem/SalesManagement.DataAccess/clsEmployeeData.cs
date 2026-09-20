using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace SalesManagement.DataAccess
{
    public static class clsEmployeeData
    {
        public static bool GetEmployeeInfoByEmployeeID(int EmployeeID, ref int PersonID, ref byte EmployeeQualification, ref byte EmployeeState,
            ref int CreatedByUserID,ref DateTime CreatedDate)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetEmployeeInfoByEmployeeID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmployeeID", EmployeeID);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                PersonID = (int)reader["PersonID"];
                                EmployeeQualification = (byte)reader["EmployeeQualification"];
                                EmployeeState = (byte)reader["EmployeeState"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];

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

        public static bool GetEmployeeInfoByPersonID(int PersonID, ref int EmployeeID, ref byte EmployeeQualification, ref byte EmployeeState,
            ref int CreatedByUserID, ref DateTime CreatedDate)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetEmployeeInfoByPersonID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonID);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                EmployeeID = (int)reader["EmployeeID"];
                                EmployeeQualification = (byte)reader["EmployeeQualification"];
                                EmployeeState = (byte)reader["EmployeeState"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];

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

        public static bool GetEmployeeFullNameByEmployeeID(int? EmployeeID, ref string FullName)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetEmployeeFullNameByEmployeeID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmployeeID", EmployeeID);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                FullName = (string)reader["FullName"];

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

        public static bool GetEmployeeIDByEmployeeFullName(string FullName, ref int EmployeeID)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetEmployeeIDByEmployeeFullName", connection))
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

                                EmployeeID = (int)reader["EmployeeID"];

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


        public static int AddNewEmployee(int PersonID, byte EmployeeQualification, byte EmployeeState, int CreatedByUserID)
        {
            //this function will return the new Employee id if succeeded and -1 if not.
            int NewEmployeeID = -1;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_AddNewEmployee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@EmployeeQualification", EmployeeQualification);
                        command.Parameters.AddWithValue("@EmployeeState", EmployeeState);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        SqlParameter outputParam = new SqlParameter("@NewEmployeeID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        object result = command.Parameters["@NewEmployeeID"].Value;
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            NewEmployeeID = insertedID;
                        }
                    }
                }

            }

            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
            }

            return NewEmployeeID;
        }

        public static bool UpdateEmployee(int ID, int PersonID, byte EmployeeQualification, byte EmployeeState,
            int CreatedByUserID, DateTime CreatedDate)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_UpdateEmployee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmployeeID", ID);
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@EmployeeQualification", EmployeeQualification);
                        command.Parameters.AddWithValue("@EmployeeState", EmployeeState);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@CreatedDate", CreatedDate);


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

        public static DataView GetAllEmployees()
        {

            DataTable dt = new DataTable();

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetAllEmployees", connection))
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

        public static bool DeleteEmployee(int EmployeeID)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_DeleteEmployee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmployeeID", EmployeeID);


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

        public static bool IsEmployeeExist(int ID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsEmployeeExistByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmployeeID", ID);

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

        public static bool IsEmployeeExistByPersonID(int PersonID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsEmployeeExistByPersonID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonID);

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


        public static int GetEmployeeIDByPersonID(int PersonID)
        {
            int EmployeeID = -1;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetEmployeeIDByPersonID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonID);


                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            EmployeeID = ID;
                        }
                    }
                }

            }

            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
            }

            return EmployeeID;

        }


    }
}
