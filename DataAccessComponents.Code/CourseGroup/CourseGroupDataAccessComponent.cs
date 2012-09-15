
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.DataAccessComponents.Interface.CourseGroup;

namespace SampleHttpApplication.DataAccessComponents.Code.CourseGroup
{
    /// <summary>
    /// Represents the CourseGroup data access component.
    /// </summary>
    public class CourseGroupDataAccessComponent : ICourseGroupDataAccessComponent
    {
        /// <summary>
        /// Gets the values from the specified SQL data reader.
        /// </summary>
        private CourseGroupDataRow GetSqlDataReaderValues(SqlDataReader sqlDataReader)
        {
            // Build the CourseGroup data row.
            CourseGroupDataRow courseGroupDataRow = new CourseGroupDataRow();

            // Read the values.
            courseGroupDataRow.CourseGroupID = (int)sqlDataReader["CourseGroupID"];
            courseGroupDataRow.CourseGroupCode = (string)sqlDataReader["CourseGroupCode"];
            courseGroupDataRow.CourseScheduleID = (int)sqlDataReader["CourseScheduleID"];
            courseGroupDataRow.PlacesCount = (int)sqlDataReader["PlacesCount"];

            // Return the CourseGroup data row.
            return courseGroupDataRow;
        }

        /// <summary>
        /// Sets the parameter values on the specified SQL command.
        /// </summary>
        private void SetSqlCommandParameterValues(SqlCommand sqlCommand, CourseGroupDataRow courseGroupDataRow, bool setPrimaryKeyValue)
        {
            // Set the primary key if requested.
            if (setPrimaryKeyValue)
            {
                sqlCommand.Parameters.Add("@courseGroupID", SqlDbType.Int).Value = courseGroupDataRow.CourseGroupID;
            }

            // Set the other parameters.
            sqlCommand.Parameters.Add("@courseGroupCode", SqlDbType.NVarChar, 50).Value = courseGroupDataRow.CourseGroupCode;
            sqlCommand.Parameters.Add("@courseScheduleID", SqlDbType.Int).Value = courseGroupDataRow.CourseScheduleID;
            sqlCommand.Parameters.Add("@placesCount", SqlDbType.Int).Value = courseGroupDataRow.PlacesCount;
        }

        /// <summary>
        /// Creates the specified CourseGroup data row.
        /// </summary>
        public async Task Create(IDatabaseConnection databaseConnection, CourseGroupDataRow courseGroupDataRow)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO [CourseGroup] VALUES (@courseGroupCode, @courseScheduleID, @placesCount); SELECT CAST(SCOPE_IDENTITY() AS INT);"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                this.SetSqlCommandParameterValues(sqlCommand, courseGroupDataRow, setPrimaryKeyValue: false);

                // Execute the SQL command.
                int courseGroupID = (int)await sqlCommand.ExecuteScalarAsync();

                // Assign the generated CourseGroupID.
                courseGroupDataRow.CourseGroupID = courseGroupID;
            }
        }

        /// <summary>
        /// Reads the single CourseGroup data row matching the specified CourseGroupID.
        /// </summary>
        public async Task<CourseGroupDataRow> ReadByCourseGroupID(IDatabaseConnection databaseConnection, int courseGroupID)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [CourseGroup] WHERE [CourseGroupID] = @courseGroupID;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                sqlCommand.Parameters.Add("@courseGroupID", SqlDbType.Int).Value = courseGroupID;

                // Execute the SQL command.
                using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                {
                    // Read the CourseGroup data row.
                    CourseGroupDataRow courseGroupDataRow = null;
                    if (await sqlDataReader.ReadAsync())
                    {
                        courseGroupDataRow = this.GetSqlDataReaderValues(sqlDataReader);
                    }

                    // Return the CourseGroup data row.
                    return courseGroupDataRow;
                }
            }
        }

        /// <summary>
        /// Reads the single CourseGroup data row matching the specified CourseGroupCode.
        /// </summary>
        public async Task<CourseGroupDataRow> ReadByCourseGroupCode(IDatabaseConnection databaseConnection, string courseGroupCode)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [CourseGroup] WHERE [CourseGroupCode] = @courseGroupCode;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                sqlCommand.Parameters.Add("@courseGroupCode", SqlDbType.NVarChar, 50).Value = courseGroupCode;

                // Execute the SQL command.
                using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                {
                    // Read the CourseGroup data row.
                    CourseGroupDataRow courseGroupDataRow = null;
                    if (await sqlDataReader.ReadAsync())
                    {
                        courseGroupDataRow = this.GetSqlDataReaderValues(sqlDataReader);
                    }

                    // Return the CourseGroup data row.
                    return courseGroupDataRow;
                }
            }
        }

        /// <summary>
        /// Reads the multiple CourseGroup data rows matching the specified CourseScheduleID.
        /// </summary>
        public async Task<CourseGroupDataRow[]> ReadByCourseScheduleID(IDatabaseConnection databaseConnection, int courseScheduleID)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [CourseGroup] WHERE [CourseScheduleID] = @courseScheduleID;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                sqlCommand.Parameters.Add("@courseScheduleID", SqlDbType.Int).Value = courseScheduleID;

                // Execute the SQL command.
                using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                {
                    // Read the CourseGroup data rows.
                    List<CourseGroupDataRow> courseGroupDataRows = new List<CourseGroupDataRow>();
                    while (await sqlDataReader.ReadAsync())
                    {
                        CourseGroupDataRow courseGroupDataRow = this.GetSqlDataReaderValues(sqlDataReader);
                        courseGroupDataRows.Add(courseGroupDataRow);
                    }

                    // Return the CourseGroup data rows.
                    return courseGroupDataRows.ToArray();
                }
            }
        }

        /// <summary>
        /// Updates the specified CourseGroup data row.
        /// </summary>
        public async Task Update(IDatabaseConnection databaseConnection, CourseGroupDataRow courseGroupDataRow)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("UPDATE [CourseGroup] SET [CourseGroupCode] = @courseGroupCode, [CourseScheduleID] = @courseScheduleID, [PlacesCount] = @placesCount WHERE [CourseGroupID] = @courseGroupID;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                this.SetSqlCommandParameterValues(sqlCommand, courseGroupDataRow, setPrimaryKeyValue: true);

                // Execute the SQL command.
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Deletes the specified CourseGroup data row.
        /// </summary>
        public async Task Delete(IDatabaseConnection databaseConnection, CourseGroupDataRow courseGroupDataRow)
        {
            // Build the SQL command.
            using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM [CourseGroup] WHERE [CourseGroupID] = @courseGroupID;"))
            {
                // Use the specified database connection.
                SqlConnection sqlConnection = (databaseConnection as DatabaseConnection).SqlConnection;
                sqlCommand.Connection = sqlConnection;

                // Set the SQL command parameter values.
                sqlCommand.Parameters.Add("@courseGroupID", SqlDbType.Int).Value = courseGroupDataRow.CourseGroupID;

                // Execute the SQL command.
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
    }
}
