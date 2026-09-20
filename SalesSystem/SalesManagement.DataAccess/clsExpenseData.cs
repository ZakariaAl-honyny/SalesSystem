using Microsoft.Data.SqlClient;
using System.Data;

namespace SalesManagement.DataAccess
{
    public static class clsExpenseData
    {
        public static bool GetExpenseInfoByID(int ExpenseID, ref string ExpenseName,
            ref string ExpenseDescription)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetExpenseInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ExpenseID", ExpenseID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                ExpenseName = (string)reader["ExpenseName"];

                                // allows null failed in database so we should handle null
                                if (reader["ExpenseDescription"] != DBNull.Value) 
                                    ExpenseDescription = (string)reader["ExpenseDescription"];
                                else
                                    ExpenseDescription = "";

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
        public static bool GetExpenseInfoByExpenseName(string ExpenseName, ref int ExpenseID,
            ref string ExpenseDescription)
        {

            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetExpenseInfoByExpenseName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ExpenseName", ExpenseName);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                ExpenseID = (int)reader["ExpenseID"];

                                // allows null failed in database so we should handle null
                                if (reader["ExpenseDescription"] != DBNull.Value)
                                    ExpenseDescription = (string)reader["ExpenseDescription"];
                                else
                                    ExpenseDescription = "";


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

        public static int AddNewExpense(string ExpenseName,
            string ExpenseDescription)
        {
            //this function will return the new Expense id if succeeded and -1 if not.
            int NewExpenseID = -1;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewExpense", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ExpenseName", ExpenseName);
                        if (ExpenseDescription != "" && ExpenseDescription != null)
                            command.Parameters.AddWithValue("@ExpenseDescription", ExpenseDescription);
                        else
                            command.Parameters.AddWithValue("@ExpenseDescription", DBNull.Value);


                        SqlParameter outputParam = new SqlParameter("@NewExpenseID", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                        command.Parameters.Add(outputParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        object result = command.Parameters["@NewExpenseID"].Value;
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            NewExpenseID = insertedID;
                        }
                    }
                }
            }

            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent("Database connection error: " + ex.Message);
            }

            return NewExpenseID;
        }

        public static bool UpdateExpense(int ExpenseID, string ExpenseName,
            string ExpenseDescription)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateExpense", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ExpenseID", ExpenseID);
                        command.Parameters.AddWithValue("@ExpenseName", ExpenseName);
                        if (ExpenseDescription != "" && ExpenseDescription != null)
                            command.Parameters.AddWithValue("@ExpenseDescription", ExpenseDescription);
                        else
                            command.Parameters.AddWithValue("@ExpenseDescription", DBNull.Value);

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

        public static DataView GetAllExpenses()
        {

            DataTable dt = new DataTable();

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllExpenses", connection))
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

        public static bool DeleteExpense(int ExpenseID)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteExpense", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ExpenseID", ExpenseID);

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

        public static bool IsExpenseExist(int ExpenseID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsExpenseExistByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ExpenseID", ExpenseID);

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
        public static bool IsExpenseExist(string ExpenseName)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsExpenseExistByExpenseName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ExpenseName", ExpenseName);

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
