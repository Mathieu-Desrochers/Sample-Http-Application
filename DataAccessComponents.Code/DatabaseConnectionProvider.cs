
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.DataAccessComponents.Interface;

namespace SampleHttpApplication.DataAccessComponents.Code
{
    /// <summary>
    /// Represents the database connection provider.
    /// </summary>
    public class DatabaseConnectionProvider : IDatabaseConnectionProvider
    {
        /// <summary>
        /// Opens a database connection.
        /// </summary>
        public IDatabaseConnection OpenDatabaseConnection()
        {
            // Get the configured connection string.
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["SampleHttpApplication"];
            string connectionString = connectionStringSettings.ConnectionString;

            // Build the database connection.
            IDatabaseConnection databaseConnection = new DatabaseConnection(connectionString);
            databaseConnection.Open();

            // Return the database connection.
            return databaseConnection;
        }
    }
}
