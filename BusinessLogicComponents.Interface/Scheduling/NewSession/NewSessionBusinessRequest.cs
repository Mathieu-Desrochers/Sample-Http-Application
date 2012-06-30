
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling
{
    /// <summary>
    /// Represents a NewSession business request.
    /// </summary>
    public class NewSessionBusinessRequest
    {
        /// <summary>
        /// The new session.
        /// </summary>
        public SessionBusinessRequestElement Session;
        public class SessionBusinessRequestElement
        {
            /// <summary>
            /// Gets or sets the SessionCode.
            /// </summary>
            [Required]
            [StringLength(50)]
            public string SessionCode { get; set; }

            /// <summary>
            /// Gets or sets the Name.
            /// </summary>
            [Required]
            [StringLength(50)]
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the StartDate.
            /// </summary>
            public DateTime StartDate { get; set; }
        }
    }
}
