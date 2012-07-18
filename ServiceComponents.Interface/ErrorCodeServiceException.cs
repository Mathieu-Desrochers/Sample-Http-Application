
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.ServiceComponents.Interface
{
    /// <summary>
    /// Represents the ErrorCode service exception.
    /// </summary>
    public class ErrorCodeServiceException : ServiceException
    {
        /// <summary>
        /// Initialization constructor.
        /// </summary>
        public ErrorCodeServiceException()
        {
            this.Details = new ErrorCodeServiceExceptionDetails();
        }

        /// <summary>
        /// The exception details.
        /// </summary>
        public ErrorCodeServiceExceptionDetails Details;
        public class ErrorCodeServiceExceptionDetails
        {
            /// <summary>
            /// Gets or sets the error code.
            /// </summary>
            public string ErrorCode { get; set; }
        }
    }
}
