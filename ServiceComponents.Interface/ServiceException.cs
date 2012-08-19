
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
    public class ServiceException : Exception
    {
        /// <summary>
        /// Initialization constructor.
        /// </summary>
        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// The errors.
        /// </summary>
        public ErrorServiceExceptionElement[] Errors;
        public class ErrorServiceExceptionElement
        {
            /// <summary>
            /// The error code.
            /// </summary>
            public string ErrorCode;

            /// <summary>
            /// The erroneous value.
            /// </summary>
            public object ErroneousValue;
        }
    }
}
