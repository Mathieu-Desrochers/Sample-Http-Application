
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.DataAccessComponents.Interface.Session;

namespace SampleHttpApplication.DataAccessComponents.Code.Session
{
    /// <summary>
    /// Represents the Session data access component.
    /// </summary>
    public class SessionDataAccessComponent : ISessionDataAccessComponent
    {
        /// <summary>
        /// Gets the values from the specified SQL data reader.
        /// </summary>
        private SessionDataRow GetSqlDataReaderValues(SqlDataReader sqlDataReader)
        {
            // Build the Session data row.
            SessionDataRow sessionDataRow = new SessionDataRow();

            // Read the values.
            sessionDataRow.SessionID = (int)sqlDataReader["SessionID"];
            sessionDataRow.SessionCode = (string)sqlDataReader["SessionCode"];
            sessionDataRow.Name = (string)sqlDataReader["Name"];
            sessionDataRow.StartDate = (DateTime)sqlDataReader["StartDate"];

            // Return the Session data row.
            return sessionDataRow;
        }

        /// <summary>
        /// Sets the parameter values on the specified SQL command.
        /// </summary>
        private void SetSqlCommandParameterValues(SqlCommand sqlCommand, SessionDataRow sessionDataRow, bool setPrimaryKeyValue)
        {
            // Set the primary key if requested.
            if (setPrimaryKeyValue)
            {
                sqlCommand.Parameters.Add("@sessionID", SqlDbType.Int).Value = sessionDataRow.SessionID;
            }

            // Set the other parameters.
            sqlCommand.Parameters.Add("@sessionCode", SqlDbType.NVarChar, 50).Value = sessionDataRow.SessionCode;
            sqlCommand.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = sessionDataRow.Name;
            sqlCommand.Parameters.Add("@startDate", SqlDbType.Date).Value = sessionDataRow.StartDate;
        }

        /// <summary>
        /// Creates the specified Session data row.
        /// </summary>
        public async Task Create(IDatabaseConnection databaseConnection, SessionDataRow sessionDataRow)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Session] VALUES (@sessionCode, @name, @startDate); SELECT CAST(SCOPE_IDENTITY() AS INT);"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                this.SetSqlCommandParameterValues(sqlCommand, sessionDataRow, setPrimaryKeyValue: false);

                // Execute the SQL command.
                int sessionID = (int)await sqlCommand.ExecuteScalarAsync();

                // Assign the generated Session ID.
                sessionDataRow.SessionID = sessionID;
            }
        }

        /// <summary>
        /// Reads the single Session data row matching the specified SessionID.
        /// </summary>
        public async Task<SessionDataRow> ReadBySessionID(IDatabaseConnection databaseConnection, int sessionID)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Session] WHERE [SessionID] = @sessionID;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                sqlCommand.Parameters.Add("@sessionID", SqlDbType.Int).Value = sessionID;

                // Execute the SQL command.
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                // Read the Session data row.
                SessionDataRow sessionDataRow = null;
                if (await sqlDataReader.ReadAsync())
                {
                    sessionDataRow = this.GetSqlDataReaderValues(sqlDataReader);
                }

                // Return the Session data row.
                return sessionDataRow;
            }
        }

        /// <summary>
        /// Reads the single Session data row matching the specified SessionCode.
        /// </summary>
        public async Task<SessionDataRow> ReadBySessionCode(IDatabaseConnection databaseConnection, string sessionCode)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Session] WHERE [SessionCode] = @sessionCode;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                sqlCommand.Parameters.Add("@sessionCode", SqlDbType.NVarChar, 50).Value = sessionCode;

                // Execute the SQL command.
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                // Read the Session data row.
                SessionDataRow sessionDataRow = null;
                if (await sqlDataReader.ReadAsync())
                {
                    sessionDataRow = this.GetSqlDataReaderValues(sqlDataReader);
                }

                // Return the Session data row.
                return sessionDataRow;
            }
        }

        /// <summary>
        /// Reads all the Session data rows.
        /// </summary>
        public async Task<SessionDataRow[]> ReadAll(IDatabaseConnection databaseConnection)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Session];"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Execute the SQL command.
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                // Read the Session data rows.
                List<SessionDataRow> sessionDataRows = new List<SessionDataRow>();
                while (await sqlDataReader.ReadAsync())
                {
                    SessionDataRow sessionDataRow = this.GetSqlDataReaderValues(sqlDataReader);
                    sessionDataRows.Add(sessionDataRow);
                }

                // Return the Session data rows.
                return sessionDataRows.ToArray();
            }
        }

        /// <summary>
        /// Updates the specified Session data row.
        /// </summary>
        public async Task Update(IDatabaseConnection databaseConnection, SessionDataRow sessionDataRow)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("UPDATE [Session] SET [SessionCode] = @sessionCode, [Name] = @name, [StartDate] = @startDate WHERE [SessionID] = @sessionID;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                this.SetSqlCommandParameterValues(sqlCommand, sessionDataRow, setPrimaryKeyValue: true);

                // Execute the SQL command.
                await sqlCommand.ExecuteScalarAsync();
            }
        }

        /// <summary>
        /// Deletes the specified Session data row.
        /// </summary>
        public async Task Delete(IDatabaseConnection databaseConnection, SessionDataRow sessionDataRow)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM [Session] WHERE [SessionID] = @sessionID;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                sqlCommand.Parameters.Add("@sessionID", SqlDbType.Int).Value = sessionDataRow.SessionID;

                // Execute the SQL command.
                await sqlCommand.ExecuteScalarAsync();
            }
        }
    }
}
