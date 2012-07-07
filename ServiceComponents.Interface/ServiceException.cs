
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.ServiceComponents.Interface
{
    /// <summary>
    /// Represents the service exception.
    /// </summary>
    public class ServiceException : ServiceExceptionBase
    {
        /// <summary>
        /// Initialization constructor.
        /// </summary>
        public ServiceException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        /// <summary>
        /// Represents the service exception details.
        /// </summary>
        public class ServiceExceptionDetails
        {
            /// <summary>
            /// Gets or sets the ErrorCode.
            /// </summary>
            public string ErrorCode { get; set; }
        }
    }
}
