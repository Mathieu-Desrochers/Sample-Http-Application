
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewSession
{
    /// <summary>
    /// Represents the NewSession business response.
    /// </summary>
    public class NewSessionBusinessResponse
    {
        /// <summary>
        /// The new session.
        /// </summary>
        public SessionBusinessResponseElement Session { get; set; }
        public class SessionBusinessResponseElement
        {
            /// <summary>
            /// Gets or sets the SessionCode.
            /// </summary>
            public string SessionCode { get; set; }
        }
    }
}
