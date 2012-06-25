
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.DataAccessComponents.Interface.Session
{
    /// <summary>
    /// Represents a Session data access component.
    /// </summary>
    public interface ISessionDataAccessComponent
    {
        /// <summary>
        /// Creates the specified Session data row.
        /// </summary>
        Task Create(IDatabaseConnection databaseConnection, SessionDataRow sessionDataRow);

        /// <summary>
        /// Reads the single Session data row matching the specified SessionID.
        /// </summary>
        Task<SessionDataRow> ReadBySessionID(IDatabaseConnection databaseConnection, int sessionID);

        /// <summary>
        /// Reads the single Session data row matching the specified SessionCode.
        /// </summary>
        Task<SessionDataRow> ReadBySessionCode(IDatabaseConnection databaseConnection, string sessionCode);

        /// <summary>
        /// Updates the specified Session data row.
        /// </summary>
        Task Update(IDatabaseConnection databaseConnection, SessionDataRow sessionDataRow);

        /// <summary>
        /// Deletes the specified Session data row.
        /// </summary>
        Task Delete(IDatabaseConnection databaseConnection, SessionDataRow sessionDataRow);
    }
}
