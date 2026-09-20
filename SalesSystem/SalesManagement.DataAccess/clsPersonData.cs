using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace SalesManagement.DataAccess
{
    public static class clsPersonData
    {
        // ── Helpers ────────────────────────────────────────────────────────────────

        // Reads a nullable string column from the reader.
        // Returns "" instead of null to keep the rest of the code null-safe.
        private static string _ReadNullableString(SqlDataReader reader, string column)
            => reader[column] != DBNull.Value ? (string)reader[column] : "";

        // Converts an optional string value to DBNull when empty/null,
        // because SQL Server stored procedures expect NULL — not an empty string — for optional fields.
        private static object _ToDbValue(string? value)
            => string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;

        // Reads all shared person fields from a reader row into the ref parameters.
        // Extracted to avoid repeating the same 12 lines in every Get method.
        private static void _ReadPersonFields(SqlDataReader reader,
            ref string firstName, ref string secondName, ref string? thirdName,
            ref string lastName, ref DateTime dateOfBirth, ref byte gender,
            ref string address, ref string phone, ref string? email,
            ref int nationalityCountryID, ref string imagePath)
        {
            firstName = (string)reader["FirstName"];
            secondName = (string)reader["SecondName"];
            lastName = (string)reader["LastName"];
            dateOfBirth = (DateTime)reader["DateOfBirth"];
            gender = (byte)reader["Gender"];
            address = (string)reader["Address"];
            phone = (string)reader["Phone"];
            nationalityCountryID = (int)reader["NationalityCountryID"];

            // These three columns are nullable in the database
            thirdName = _ReadNullableString(reader, "ThirdName");
            email = _ReadNullableString(reader, "Email");
            imagePath = _ReadNullableString(reader, "ImagePath");
        }

        // ── Get Methods ────────────────────────────────────────────────────────────

        public static bool GetPersonInfoByID(int personID,
            ref string nationalNo, ref string firstName, ref string secondName,
            ref string? thirdName, ref string lastName, ref DateTime dateOfBirth,
            ref byte gender, ref string address, ref string phone,
            ref string? email, ref int nationalityCountryID, ref string imagePath)
        {
            bool isFound = false;

            try
            {
                using SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
                using SqlCommand command = new SqlCommand("SP_GetPersonInfoByID", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PersonID", personID);

                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    nationalNo = (string)reader["NationalNo"];

                    _ReadPersonFields(reader,
                        ref firstName, ref secondName, ref thirdName,
                        ref lastName, ref dateOfBirth, ref gender,
                        ref address, ref phone, ref email,
                        ref nationalityCountryID, ref imagePath);
                }
            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent($"GetPersonInfoByID failed: {ex.Message}");
            }

            return isFound;
        }

        public static bool GetPersonInfoByNationalNo(string nationalNo,
            ref int personID, ref string firstName, ref string secondName,
            ref string? thirdName, ref string lastName, ref DateTime dateOfBirth,
            ref byte gender, ref string address, ref string phone,
            ref string? email, ref int nationalityCountryID, ref string imagePath)
        {
            bool isFound = false;

            try
            {
                using SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
                using SqlCommand command = new SqlCommand("SP_GetPersonInfoByNationalNo", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NationalNo", nationalNo);

                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    personID = (int)reader["PersonID"];

                    _ReadPersonFields(reader,
                        ref firstName, ref secondName, ref thirdName,
                        ref lastName, ref dateOfBirth, ref gender,
                        ref address, ref phone, ref email,
                        ref nationalityCountryID, ref imagePath);
                }
            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent($"GetPersonInfoByNationalNo failed: {ex.Message}");
            }

            return isFound;
        }

        public static bool GetPersonInfoByFullName(string fullName,
            ref string nationalNo, ref int personID, ref string firstName,
            ref string secondName, ref string? thirdName, ref string lastName,
            ref DateTime dateOfBirth, ref byte gender, ref string address,
            ref string phone, ref string? email, ref int nationalityCountryID,
            ref string imagePath)
        {
            bool isFound = false;

            try
            {
                using SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
                using SqlCommand command = new SqlCommand("SP_GetPersonInfoByFullName", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FullName", fullName);

                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    personID = (int)reader["PersonID"];
                    nationalNo = (string)reader["NationalNo"];

                    _ReadPersonFields(reader,
                        ref firstName, ref secondName, ref thirdName,
                        ref lastName, ref dateOfBirth, ref gender,
                        ref address, ref phone, ref email,
                        ref nationalityCountryID, ref imagePath);
                }
            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent($"GetPersonInfoByFullName failed: {ex.Message}");
            }

            return isFound;
        }

        // ── Write Methods ──────────────────────────────────────────────────────────

        // Adds shared person parameters to any insert/update command.
        // Avoids repeating the same 12 AddWithValue lines in both Add and Update.
        private static void _AddPersonParameters(SqlCommand command,
            string nationalNo, string firstName, string secondName, string? thirdName,
            string lastName, DateTime dateOfBirth, byte gender,
            string address, string phone, string? email,
            int nationalityCountryID, string imagePath)
        {
            command.Parameters.AddWithValue("@NationalNo", nationalNo);
            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@SecondName", secondName);
            command.Parameters.AddWithValue("@LastName", lastName);
            command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
            command.Parameters.AddWithValue("@Gender", gender);
            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@NationalityCountryID", nationalityCountryID);

            // Optional fields — send DBNull when empty so SP handles them correctly
            command.Parameters.AddWithValue("@ThirdName", _ToDbValue(thirdName));
            command.Parameters.AddWithValue("@Email", _ToDbValue(email));
            command.Parameters.AddWithValue("@ImagePath", _ToDbValue(imagePath));
        }

        // Returns the new PersonID on success, or -1 on failure.
        public static int AddNewPerson(string nationalNo, string firstName,
            string secondName, string? thirdName, string lastName, DateTime dateOfBirth,
            byte gender, string address, string phone, string? email,
            int nationalityCountryID, string imagePath)
        {
            int newPersonID = -1;

            try
            {
                using SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
                using SqlCommand command = new SqlCommand("SP_AddNewPerson", connection);

                command.CommandType = CommandType.StoredProcedure;

                _AddPersonParameters(command, nationalNo, firstName, secondName, thirdName,
                    lastName, dateOfBirth, gender, address, phone, email,
                    nationalityCountryID, imagePath);

                // OUTPUT parameter — SP writes the new identity value into it after INSERT
                SqlParameter outputParam = new SqlParameter("@NewPersonID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                connection.Open();
                command.ExecuteNonQuery();

                // Read the output parameter value after execution
                if (int.TryParse(command.Parameters["@NewPersonID"].Value?.ToString(), out int insertedID))
                    newPersonID = insertedID;
            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent($"AddNewPerson failed: {ex.Message}");
            }

            return newPersonID;
        }

        public static bool UpdatePerson(int personID, string nationalNo, string firstName,
            string secondName, string? thirdName, string lastName, DateTime dateOfBirth,
            byte gender, string address, string phone, string? email,
            int nationalityCountryID, string imagePath)
        {
            int rowsAffected = 0;

            try
            {
                using SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
                using SqlCommand command = new SqlCommand("SP_UpdatePerson", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PersonID", personID);

                _AddPersonParameters(command, nationalNo, firstName, secondName, thirdName,
                    lastName, dateOfBirth, gender, address, phone, email,
                    nationalityCountryID, imagePath);

                connection.Open();
                rowsAffected = (int)command.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent($"UpdatePerson failed: {ex.Message}");
            }

            return rowsAffected > 0;
        }

        // ── Query Methods ──────────────────────────────────────────────────────────

        // Note: method name kept as-is for compatibility — it is synchronous despite the name
        public static DataView GetAllPeopleAsync()
        {
            DataTable dt = new DataTable();

            try
            {
                using SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
                using SqlCommand command = new SqlCommand("SP_GetAllPeople", connection);

                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    dt.Load(reader);
            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent($"GetAllPeople failed: {ex.Message}");
            }

            return dt.DefaultView;
        }

        public static bool DeletePerson(int personID)
        {
            int rowsAffected = 0;

            try
            {
                using SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
                using SqlCommand command = new SqlCommand("SP_DeletePerson", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PersonID", personID);

                connection.Open();
                rowsAffected = (int)command.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent($"DeletePerson failed: {ex.Message}");
            }

            return rowsAffected > 0;
        }

        // ── Existence Checks ───────────────────────────────────────────────────────

        // Uses SP RETURN VALUE (not OUTPUT parameter) to get a 1/0 boolean result.
        // RETURN VALUE is read via ParameterDirection.ReturnValue after ExecuteNonQuery.
        private static bool _IsPersonExist(string spName, string paramName, object paramValue)
        {
            bool isFound = false;

            try
            {
                using SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
                using SqlCommand command = new SqlCommand(spName, connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(paramName, paramValue);

                SqlParameter returnParam = new SqlParameter("@ReturnVal", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                command.Parameters.Add(returnParam);

                connection.Open();
                command.ExecuteNonQuery();

                isFound = (int)returnParam.Value == 1;
            }
            catch (SqlException ex)
            {
                clsEventsLog.LoggingErrorEvent($"{spName} failed: {ex.Message}");
            }

            return isFound;
        }

        public static bool IsPersonExist(int personID)
            => _IsPersonExist("SP_IsPersonExistByID", "@PersonID", personID);

        public static bool IsPersonExist(string nationalNo)
            => _IsPersonExist("SP_IsPersonExistByNationalNo", "@NationalNo", nationalNo);
    }
}