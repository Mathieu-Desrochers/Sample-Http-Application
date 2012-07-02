
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling
{
    /// <summary>
    /// Represents the GetSessions business response.
    /// </summary>
    public class GetSessionsBusinessResponse
    {
        /// <summary>
        /// The sessions.
        /// </summary>
        public SessionBusinessResponseElement[] Sessions;
        public class SessionBusinessResponseElement
        {
            /// <summary>
            /// Gets or sets the SessionID.
            /// </summary>
            public int SessionID { get; set; }

            /// <summary>
            /// Gets or sets the SessionCode.
            /// </summary>
            public string SessionCode { get; set; }

            /// <summary>
            /// Gets or sets the Name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the StartDate.
            /// </summary>
            public DateTime StartDate { get; set; }
        }
    }
}
