
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.DataAccessComponents.Interface;

namespace SampleHttpApplication.DataAccessComponents.Code
{
    /// <summary>
    /// Represents the database connection.
    /// </summary>
    public class DatabaseConnection : IDatabaseConnection
    {
        /// <summary>
        /// The underlying SQL connection.
        /// </summary>
        public SqlConnection SqlConnection;

        /// <summary>
        /// Initialization constructor.
        /// </summary>
        public DatabaseConnection(string connectionString)
        {
            // Build the underlying SQL connection.
            this.SqlConnection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Opens the database connection.
        /// </summary>
        public async Task Open()
        {
            // Open the underlying SQL connection.
            await this.SqlConnection.OpenAsync();
        }

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        public IDatabaseTransaction BeginDatabaseTransaction()
        {
            // Begin the underlying SQL transaction.
            SqlTransaction sqlTransaction = this.SqlConnection.BeginTransaction(IsolationLevel.Serializable);

            // Return the database transaction.
            DatabaseTransaction databaseTransaction = new DatabaseTransaction(sqlTransaction);
            return databaseTransaction;
        }

        /// <summary>
        /// Disposes of the database connection.
        /// </summary>
        public void Dispose()
        {
            // Close the underlying SQL connection.
            this.SqlConnection.Close();
            this.SqlConnection.Dispose();
        }
    }
}
