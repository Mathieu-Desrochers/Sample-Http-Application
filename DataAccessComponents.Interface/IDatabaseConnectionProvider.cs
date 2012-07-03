
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.DataAccessComponents.Interface
{
    /// <summary>
    /// Represents the database connection provider.
    /// </summary>
    public interface IDatabaseConnectionProvider
    {
        /// <summary>
        /// Opens a database connection.
        /// </summary>
        IDatabaseConnection OpenDatabaseConnection();
    }
}
