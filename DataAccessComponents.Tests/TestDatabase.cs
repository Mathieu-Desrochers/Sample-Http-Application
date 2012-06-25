
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleHttpApplication.DataAccessComponents.Code;

namespace SampleHttpApplication.DataAccessComponents.Tests
{
    /// <summary>
    /// Represents the test database.
    /// </summary>
    public static class TestDatabase
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                // Get the connection string.
                ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["SampleHttpApplication"];
                string connectionString = connectionStringSettings.ConnectionString;

                // Return the connection string.
                return connectionString;
            }
        }

        /// <summary>
        /// Executes the specified non query command.
        /// </summary>
        public static void ExecuteNonQuery(string commandText)
        {
            // Build the database connection.
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                // Open the database connection.
                sqlConnection.Open();

                // Execute the specified command.
                SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes the specified scalar command.
        /// </summary>
        public static T ExecuteScalar<T>(string commandText)
        {
            // Build the database connection.
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                // Open the database connection.
                sqlConnection.Open();

                // Execute the specified command.
                SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);
                T sqlCommandResult = (T)sqlCommand.ExecuteScalar();

                // Return the command's result.
                return sqlCommandResult;
            }
        }

        /// <summary>
        /// Executes the specified reader command.
        /// </summary>
        public static SqlDataReader ExecuteReader(string commandText)
        {
            // Build the database connection.
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            // Open the database connection.
            sqlConnection.Open();

            // Execute the specified command.
            SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader.
            return sqlDataReader;
        }
    }
}
