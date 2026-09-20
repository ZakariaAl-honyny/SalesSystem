using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SalesManagement.DataAccess
{
    public static class clsDelegateData
    {
        public static bool GetDelegateInfoByID(int DelegateID, ref int PersonID)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetDelegateInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DelegateID", DelegateID);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                    PersonID = (int)reader["PersonID"];
                                
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

        public static bool GetDelegateInfoByPersonID(int PersonID, ref int DelegateID)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetDelegateInfoByPersonID", connection))
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

                                DelegateID = (int)reader["DelegateID"];


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

        public static int AddNewDelegate(int PersonID)
        {
            //this function will return the new Delegate id if succeeded and -1 if not.
            int NewDelegateID = -1;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_AddNewDelegate", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        SqlParameter outputParam = new SqlParameter("@NewDelegateID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        object result = command.Parameters["@NewDelegateID"].Value;
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            NewDelegateID = insertedID;
                        }
                    }
                }

            }

            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
            }

            return NewDelegateID;
        }

        public static bool UpdateDelegate(int ID, int PersonID)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_UpdateDelegate", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DelegateID", ID);
                        command.Parameters.AddWithValue("@PersonID", PersonID);


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

        public static DataView GetAllDelegatesAsync()
        {

            DataTable dt = new DataTable();

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetAllDelegates", connection))
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

        public static bool DeleteDelegate(int DelegateID)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_DeleteDelegate", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DelegateID", DelegateID);


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
        
        public static bool IsDelegateExist(int DelegateID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsDelegateExistByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DelegateID", DelegateID);

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

        public static bool IsDelegateExistPersonID(int PersonID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsDelegateExistByPersonID", connection))
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

    }
}
