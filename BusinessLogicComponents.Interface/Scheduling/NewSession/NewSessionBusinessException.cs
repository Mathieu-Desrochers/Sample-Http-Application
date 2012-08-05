
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewSession
{
    /// <summary>
    /// Represents the NewSession business exception.
    /// </summary>
    public class NewSessionBusinessException : BusinessException
    {
        /// <summary>
        /// The possible error codes.
        /// </summary>
        public enum ErrorCodes
        {
            InvalidSessionCode,
            DuplicateSessionCode,
            InvalidName
        }

        /// <summary>
        /// The actual errors.
        /// </summary>
        public ErrorBusinessExceptionElement[] Errors;
        public class ErrorBusinessExceptionElement
        {
            /// <summary>
            /// The error code.
            /// </summary>
            public ErrorCodes ErrorCode;

            /// <summary>
            /// The erroneous value.
            /// </summary>
            public object Value;
        }
    }
}
