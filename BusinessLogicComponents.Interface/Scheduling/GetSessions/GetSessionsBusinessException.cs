
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.GetSessions
{
    /// <summary>
    /// Represents the GetSessions business exception.
    /// </summary>
    public class GetSessionsBusinessException : BusinessException
    {
        /// <summary>
        /// The possible error codes.
        /// </summary>
        public enum ErrorCodes
        {
        }

        /// <summary>
        /// The errors.
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
            public object ErroneousValue;
        }
    }
}
