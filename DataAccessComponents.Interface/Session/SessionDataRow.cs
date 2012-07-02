
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.DataAccessComponents.Interface.Session
{
    /// <summary>
    /// Represents the Session data row.
    /// </summary>
    public class SessionDataRow
    {
        /// <summary>
        /// Gets or sets the SessionID.
        /// </summary>
        public int SessionID;

        /// <summary>
        /// Gets or sets the SessionCode.
        /// </summary>
        public string SessionCode;

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets or sets the StartDate.
        /// </summary>
        public DateTime StartDate;
    }
}
