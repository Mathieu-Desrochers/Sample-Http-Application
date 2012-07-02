
using System;
using System.Collections.Generic;
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
