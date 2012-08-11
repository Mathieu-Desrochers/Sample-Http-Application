
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
    public class ErrorCodeServiceException : Exception
    {
        /// <summary>
        /// The error message.
        /// </summary>
        private string errorMessage;
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = value; }
        }

        /// <summary>
        /// Gets the exception's message.
        /// </summary>
        public override string Message
        {
            get { return this.errorMessage; }
        }

        /// <summary>
        /// The error codes.
        /// </summary>
        public ErrorCodeServiceExceptionElement[] ErrorCodes;
        public class ErrorCodeServiceExceptionElement
        {
            /// <summary>
            /// The error code.
            /// </summary>
            public string ErrorCode;

            /// <summary>
            /// The erroneous value.
            /// </summary>
            public object Value;
        }
    }
}
