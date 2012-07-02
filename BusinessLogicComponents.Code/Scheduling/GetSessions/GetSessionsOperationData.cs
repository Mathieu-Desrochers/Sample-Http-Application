
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.DataAccessComponents.Interface.Session;

namespace SampleHttpApplication.BusinessLogicComponents.Code.Scheduling
{
    /// <summary>
    /// Represents the GetSessions operation data.
    /// </summary>
    public class GetSessionsOperationData
    {
        /// <summary>
        /// The data rows.
        /// </summary>
        public SessionDataRow[] SessionDataRows;
    }
}
