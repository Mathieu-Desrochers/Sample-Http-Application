
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.DataAccessComponents.Interface;

namespace SampleHttpApplication.BusinessLogicComponents.Tests
{
    /// <summary>
    /// Represents the mocked database connection.
    /// </summary>
    public class MockedDatabaseConnection : IDatabaseConnection
    {
        /// <summary>
        /// Opens the database connection.
        /// </summary>
        public Task Open()
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Begins a database transaction
        /// </summary>
        public IDatabaseTransaction BeginDatabaseTransaction()
        {
            return null;
        }

        /// <summary>
        /// Disposes of the database connection.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
