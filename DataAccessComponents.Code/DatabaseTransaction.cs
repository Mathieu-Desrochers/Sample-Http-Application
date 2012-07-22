
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
    /// Represents the database transaction.
    /// </summary>
    public class DatabaseTransaction : IDatabaseTransaction
    {
        /// <summary>
        /// The underlying SQL transaction.
        /// </summary>
        public SqlTransaction SqlTransaction;

        /// <summary>
        /// Initialization consructor.
        /// </summary>
        public DatabaseTransaction(SqlTransaction sqlTransaction)
        {
            // Initialize the underlying SQL transaction.
            this.SqlTransaction = sqlTransaction;
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void Commit()
        {
            // Commit the underlying SQL transaction.
            this.SqlTransaction.Commit();
        }

        /// <summary>
        /// Disposes of the database transaction.
        /// </summary>
        public void Dispose()
        {
            // This will rollback the underlying SQL transaction
            // if it has not been committed by now.
            this.SqlTransaction.Dispose();
        }
    }
}
