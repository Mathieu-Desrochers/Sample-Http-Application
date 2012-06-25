
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.DataAccessComponents.Interface
{
    /// <summary>
    /// Represents a database connection.
    /// </summary>
    public interface IDatabaseConnection : IDisposable
    {
        /// <summary>
        /// Opens the database connection.
        /// </summary>
        Task Open();
    }
}
