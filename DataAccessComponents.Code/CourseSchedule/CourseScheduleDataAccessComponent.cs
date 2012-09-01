
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.DataAccessComponents.Interface.CourseSchedule;

namespace SampleHttpApplication.DataAccessComponents.Code.CourseSchedule
{
    /// <summary>
    /// Represents the CourseSchedule data access component.
    /// </summary>
    public class CourseScheduleDataAccessComponent : ICourseScheduleDataAccessComponent
    {
        /// <summary>
        /// Gets the values from the specified SQL data reader.
        /// </summary>
        private CourseScheduleDataRow GetSqlDataReaderValues(SqlDataReader sqlDataReader)
        {
            // Build the CourseSchedule data row.
            CourseScheduleDataRow courseScheduleDataRow = new CourseScheduleDataRow();

            // Read the values.
            courseScheduleDataRow.CourseScheduleID = (int)sqlDataReader["CourseScheduleID"];
            courseScheduleDataRow.CourseScheduleCode = (string)sqlDataReader["CourseScheduleCode"];
            courseScheduleDataRow.SessionID = (int)sqlDataReader["SessionID"];
            courseScheduleDataRow.DayOfWeek = (int)sqlDataReader["DayOfWeek"];
            courseScheduleDataRow.Time = (TimeSpan)sqlDataReader["Time"];

            // Return the CourseSchedule data row.
            return courseScheduleDataRow;
        }

        /// <summary>
        /// Sets the parameter values on the specified SQL command.
        /// </summary>
        private void SetSqlCommandParameterValues(SqlCommand sqlCommand, CourseScheduleDataRow courseScheduleDataRow, bool setPrimaryKeyValue)
        {
            // Set the primary key if requested.
            if (setPrimaryKeyValue)
            {
                sqlCommand.Parameters.Add("@courseScheduleID", SqlDbType.Int).Value = courseScheduleDataRow.CourseScheduleID;
            }

            // Set the other parameters.
            sqlCommand.Parameters.Add("@courseScheduleCode", SqlDbType.NVarChar, 50).Value = courseScheduleDataRow.CourseScheduleCode;
            sqlCommand.Parameters.Add("@sessionID", SqlDbType.Int).Value = courseScheduleDataRow.SessionID;
            sqlCommand.Parameters.Add("@dayOfWeek", SqlDbType.Int).Value = courseScheduleDataRow.DayOfWeek;
            sqlCommand.Parameters.Add("@time", SqlDbType.Time).Value = courseScheduleDataRow.Time;
        }

        /// <summary>
        /// Creates the specified CourseSchedule data row.
        /// </summary>
        public async Task Create(IDatabaseConnection databaseConnection, CourseScheduleDataRow courseScheduleDataRow)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO [CourseSchedule] VALUES (@courseScheduleCode, @sessionID, @dayOfWeek, @time); SELECT CAST(SCOPE_IDENTITY() AS INT);"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                this.SetSqlCommandParameterValues(sqlCommand, courseScheduleDataRow, setPrimaryKeyValue: false);

                // Execute the SQL command.
                int courseScheduleID = (int)await sqlCommand.ExecuteScalarAsync();

                // Assign the generated CourseScheduleID.
                courseScheduleDataRow.CourseScheduleID = courseScheduleID;
            }
        }

        /// <summary>
        /// Reads the single CourseSchedule data row matching the specified CourseScheduleID.
        /// </summary>
        public async Task<CourseScheduleDataRow> ReadByCourseScheduleID(IDatabaseConnection databaseConnection, int courseScheduleID)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [CourseSchedule] WHERE [CourseScheduleID] = @courseScheduleID;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                sqlCommand.Parameters.Add("@courseScheduleID", SqlDbType.Int).Value = courseScheduleID;

                // Execute the SQL command.
                using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                {
                    // Read the CourseSchedule data row.
                    CourseScheduleDataRow courseScheduleDataRow = null;
                    if (await sqlDataReader.ReadAsync())
                    {
                        courseScheduleDataRow = this.GetSqlDataReaderValues(sqlDataReader);
                    }

                    // Return the CourseSchedule data row.
                    return courseScheduleDataRow;
                }
            }
        }

        /// <summary>
        /// Reads the single CourseSchedule data row matching the specified CourseScheduleCode.
        /// </summary>
        public async Task<CourseScheduleDataRow> ReadByCourseScheduleCode(IDatabaseConnection databaseConnection, string courseScheduleCode)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [CourseSchedule] WHERE [CourseScheduleCode] = @courseScheduleCode;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                sqlCommand.Parameters.Add("@courseScheduleCode", SqlDbType.NVarChar, 50).Value = courseScheduleCode;

                // Execute the SQL command.
                using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                {
                    // Read the CourseSchedule data row.
                    CourseScheduleDataRow courseScheduleDataRow = null;
                    if (await sqlDataReader.ReadAsync())
                    {
                        courseScheduleDataRow = this.GetSqlDataReaderValues(sqlDataReader);
                    }

                    // Return the CourseSchedule data row.
                    return courseScheduleDataRow;
                }
            }
        }

        /// <summary>
        /// Updates the specified CourseSchedule data row.
        /// </summary>
        public async Task Update(IDatabaseConnection databaseConnection, CourseScheduleDataRow courseScheduleDataRow)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("UPDATE [CourseSchedule] SET [CourseScheduleCode] = @courseScheduleCode, [SessionID] = @sessionID, [DayOfWeek] = @dayOfWeek, [Time] = @time WHERE [CourseScheduleID] = @courseScheduleID;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                this.SetSqlCommandParameterValues(sqlCommand, courseScheduleDataRow, setPrimaryKeyValue: true);

                // Execute the SQL command.
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Deletes the specified CourseSchedule data row.
        /// </summary>
        public async Task Delete(IDatabaseConnection databaseConnection, CourseScheduleDataRow courseScheduleDataRow)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM [CourseSchedule] WHERE [CourseScheduleID] = @courseScheduleID;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                sqlCommand.Parameters.Add("@courseScheduleID", SqlDbType.Int).Value = courseScheduleDataRow.CourseScheduleID;

                // Execute the SQL command.
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
    }
}
