
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.DataAccessComponents.Interface
{
    /// <summary>
    /// Represents the database connection.
    /// </summary>
    public interface IDatabaseConnection : IDisposable
    {
        /// <summary>
        /// Opens the database connection.
        /// </summary>
        Task Open();

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        IDatabaseTransaction BeginDatabaseTransaction();
    }
}
