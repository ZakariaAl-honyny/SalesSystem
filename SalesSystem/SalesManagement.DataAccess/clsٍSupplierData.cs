using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SalesManagement.DataAccess
{
    public static class clsSupplierData
    {
        public static bool GetSupplierInfoByID(int SupplierID, ref string OwnerName, ref string OwnerPhone,
            ref string ManagerName, ref string ManagerPhone, ref int PersonID)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetSupplierInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SupplierID", SupplierID);


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                OwnerName = (string)reader["OwnerName"];
                                OwnerPhone = (string)reader["OwnerPhone"];
                                ManagerName = (string)reader["ManagerName"];
                                ManagerPhone = (string)reader["ManagerPhone"];
                                PersonID = (int)reader["PersonID"];

                                //ManagerName: allows null in database so we should handle null
                                if (reader["ManagerName"] != DBNull.Value)
                                {
                                }
                                else
                                {
                                    ManagerName = "";
                                }

                                if (reader["PersonID"] != DBNull.Value)
                                {
                                }
                                else
                                {
                                    PersonID = -1;
                                }

                                if (reader["ManagerPhone"] != DBNull.Value)
                                {
                                }
                                else
                                {
                                    ManagerPhone = "";
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

        public static bool GetSupplierInfoByPersonID(int PersonID, ref string OwnerName, ref int SupplierID , ref string OwnerPhone,
            ref string ManagerName, ref string ManagerPhone)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetSupplierInfoByPersonID", connection))
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

                                SupplierID = (int)reader["SupplierID"];
                                OwnerName = (string)reader["OwnerName"];
                                OwnerPhone = (string)reader["OwnerPhone"];
                                ManagerName = (string)reader["ManagerName"];
                                ManagerPhone = (string)reader["ManagerPhone"];

                                //ManagerName: allows null in database so we should handle null
                                if (reader["ManagerName"] != DBNull.Value)
                                {
                                }
                                else
                                {
                                    ManagerName = "";
                                }

                                if (reader["PersonID"] != DBNull.Value)
                                {
                                }
                                else
                                {
                                    PersonID = -1;
                                }

                                if (reader["ManagerPhone"] != DBNull.Value)
                                {
                                }
                                else
                                {
                                    ManagerPhone = "";
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

        public static int AddNewSupplier(string OwnerName, string OwnerPhone,
            string ManagerName, string ManagerPhone, int PersonID)
        {
            //this function will return the new Supplier id if succeeded and -1 if not.
            int NewSupplierID = -1;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_AddNewSupplier", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@OwnerName", OwnerName);
                        command.Parameters.AddWithValue("@OwnerPhone", OwnerPhone);
                        command.Parameters.AddWithValue("@ManagerName", ManagerName);
                        command.Parameters.AddWithValue("@ManagerPhone", ManagerPhone);
                        command.Parameters.AddWithValue("@PersonID", PersonID);


                        //if (ManagerName != "" && ManagerName != null)
                        //    command.Parameters.AddWithValue("@ManagerName", ManagerName);
                        //else
                        //    command.Parameters.AddWithValue("@ManagerName", System.DBNull.Value);

                        //if (ManagerPhone != "" && ManagerPhone != null)
                        //    command.Parameters.AddWithValue("@ManagerPhone", ManagerPhone);
                        //else
                        //    command.Parameters.AddWithValue("@ManagerPhone", System.DBNull.Value);

                        //if (PersonID != -1)
                        //    command.Parameters.AddWithValue("@PersonID", PersonID);
                        //else
                        //    command.Parameters.AddWithValue("@PersonID", System.DBNull.Value);

                        SqlParameter outputParam = new SqlParameter("@NewSupplierID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        object result = command.Parameters["@NewSupplierID"].Value;
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            NewSupplierID = insertedID;
                        }
                    }
                }

            }

            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
            }

            return NewSupplierID;
        }

        public static bool UpdateSupplier(int ID, string OwnerName, string OwnerPhone,
            string ManagerName, string ManagerPhone, int PersonID)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_UpdateSupplier", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SupplierID", ID);
                        command.Parameters.AddWithValue("@OwnerName", OwnerName);
                        command.Parameters.AddWithValue("@OwnerPhone", OwnerPhone);
                        command.Parameters.AddWithValue("@ManagerName", ManagerName);
                        command.Parameters.AddWithValue("@ManagerPhone", ManagerPhone);
                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        //if (ManagerName != "" && ManagerName != null)
                        //    command.Parameters.AddWithValue("@ManagerName", ManagerName);
                        //else
                        //    command.Parameters.AddWithValue("@ManagerName", System.DBNull.Value);

                        //if (ManagerPhone != "" && ManagerPhone != null)
                        //    command.Parameters.AddWithValue("@ManagerPhone", ManagerPhone);
                        //else
                        //    command.Parameters.AddWithValue("@ManagerPhone", System.DBNull.Value);

                        //if (PersonID != -1 && PersonID != null)
                        //    command.Parameters.AddWithValue("@PersonID", PersonID);
                        //else
                        //    command.Parameters.AddWithValue("@PersonID", System.DBNull.Value);



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

        public static DataView GetAllSuppliersAsync()
        {

            DataTable dt = new DataTable();

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_GetAllSuppliers", connection))
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

        public static bool DeleteSupplier(int SupplierID)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_DeleteSupplier", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SupplierID", SupplierID);


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
        
        public static bool IsSupplierExist(int SupplierID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsSupplierExistByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SupplierID", SupplierID);

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

        public static bool IsSupplierExistByPersonID(int PersonID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand("SP_IsSupplierExistByPersonID", connection))
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
